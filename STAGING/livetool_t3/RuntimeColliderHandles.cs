// TARGET: Assets/Scripts/LiveTool/Authoring/RuntimeColliderHandles.cs
#if RIMA_LIVE_TOOL

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using RIMA.Live;
using RIMA.RoomPainter;
using Object = UnityEngine.Object;

namespace RIMA.LiveTool
{
    /// <summary>
    /// Runtime UI Toolkit collider handles for Tool.exe.
    /// Uses VisualElement handle dots, a world LineRenderer outline, and RoomLayoutData persistence.
    /// </summary>
    public sealed class RuntimeColliderHandles
    {
        private const int UndoDepth = 32;
        private const int MaxHandles = 8;
        private const int CircleSegments = 40;
        private const float HandleHitRadius = 16f;
        private const float HandleDotSize = 10f;
        private const float MinSize = 0.01f;
        private const float LineWidth = 0.035f;

        private static readonly Color OutlineColor = new Color(0.30f, 0.95f, 0.60f, 1f);
        private static readonly Color OutlineTriggerColor = new Color(1f, 0.85f, 0.25f, 1f);

        private struct ColliderState
        {
            public ColliderShape shape;
            public Vector2 size;
            public Vector2 offset;
            public bool isTrigger;
        }

        private static readonly Vector2[] BoxCornerSigns =
        {
            new Vector2(-1f, -1f), new Vector2(+1f, -1f),
            new Vector2(+1f, +1f), new Vector2(-1f, +1f),
        };

        private static readonly Vector2[] BoxEdgeSigns =
        {
            new Vector2(0f, -1f), new Vector2(+1f, 0f),
            new Vector2(0f, +1f), new Vector2(-1f, 0f),
        };

        private readonly Stack<ColliderState> _undoStack = new Stack<ColliderState>(UndoDepth);
        private readonly VisualElement[] _handleDots = new VisualElement[MaxHandles];
        private readonly Vector2[] _handleCanvasPositions = new Vector2[MaxHandles];
        private readonly bool[] _handleVisible = new bool[MaxHandles];

        private VisualElement _canvas;
        private Camera _previewCamera;
        private LineRenderer _outline;
        private ToolBootstrap _bootstrap;

        private GameObject _target;
        private RegistryEntry _targetEntry;
        private Collider2D _cachedCollider;
        private RoomLayoutData _currentDoc;
        private string _instanceId;

        private bool _appliedDocOverride;
        private bool _hasBaseline;
        private ColliderState _baselineState;
        private bool _pendingPersist;

        private int _visibleHandleCount;
        private int _draggingHandle = -1;
        private int _activePointerId = -1;
        private bool _wasDragging;
        private Vector2 _lastPointerLocal;
        private Vector2 _lastPointerPanel;
        private Vector2 _unitScale = Vector2.one;

        public ColliderShape CurrentShape => ResolveShape(_cachedCollider);

        public void Initialize(VisualElement canvas, Camera previewCamera)
        {
            if (_canvas != null)
            {
                _canvas.UnregisterCallback<PointerDownEvent>(OnPointerDown, TrickleDown.TrickleDown);
                _canvas.UnregisterCallback<PointerMoveEvent>(OnPointerMove, TrickleDown.TrickleDown);
                _canvas.UnregisterCallback<PointerUpEvent>(OnPointerUp, TrickleDown.TrickleDown);
                _canvas.UnregisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
            }

            _canvas = canvas;
            _previewCamera = previewCamera;
            _bootstrap = Object.FindFirstObjectByType<ToolBootstrap>();

            if (_canvas == null || _previewCamera == null)
                return;

            EnsureHandleDots();
            EnsureOutline();
            HideHandles();

            _canvas.RegisterCallback<PointerDownEvent>(OnPointerDown, TrickleDown.TrickleDown);
            _canvas.RegisterCallback<PointerMoveEvent>(OnPointerMove, TrickleDown.TrickleDown);
            _canvas.RegisterCallback<PointerUpEvent>(OnPointerUp, TrickleDown.TrickleDown);
            _canvas.RegisterCallback<KeyDownEvent>(OnKeyDown, TrickleDown.TrickleDown);
        }

        public void SetTarget(GameObject target, RegistryEntry entry)
        {
            if (_target == target && _targetEntry == entry)
            {
                _cachedCollider = FindRuntimeCollider(_target);
                return;
            }

            _target = target;
            _targetEntry = entry;
            _cachedCollider = FindRuntimeCollider(_target);
            _instanceId = null;
            _appliedDocOverride = false;
            _pendingPersist = false;
            _draggingHandle = -1;
            _activePointerId = -1;
            _wasDragging = false;
            _undoStack.Clear();

            _hasBaseline = _cachedCollider != null;
            if (_hasBaseline)
                _baselineState = CaptureState(_cachedCollider);

            UpdateVisuals();
        }

        public void Tick(RoomLayoutData liveDoc)
        {
            _currentDoc = liveDoc;
            if (_bootstrap == null)
                _bootstrap = Object.FindFirstObjectByType<ToolBootstrap>();

            if (_target == null || _canvas == null || _previewCamera == null)
            {
                HideHandles();
                HideOutline();
                return;
            }

            _cachedCollider = FindRuntimeCollider(_target);
            if (_cachedCollider == null)
            {
                HideHandles();
                HideOutline();
                return;
            }

            if (!_appliedDocOverride)
            {
                ApplyExistingOverride(liveDoc);
                _appliedDocOverride = true;
            }

            UpdateVisuals();
            CommitPendingPersist(force: false);
        }

        public bool Undo()
        {
            if (_target == null || _undoStack.Count == 0)
                return false;

            ColliderState previous = _undoStack.Pop();
            ApplyColliderState(_target, previous);
            _cachedCollider = FindRuntimeCollider(_target);
            _pendingPersist = true;
            UpdateVisuals();
            CommitPendingPersist(force: true);
            return true;
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            if (evt.button != 0 || _target == null || _cachedCollider == null)
                return;

            UpdateVisuals();

            Vector2 panelPosition = evt.position;
            int hit = HitTestHandle(panelPosition);
            if (hit < 0)
                return;

            PushUndo(_cachedCollider);
            _draggingHandle = hit;
            _activePointerId = evt.pointerId;
            _wasDragging = true;
            _lastPointerPanel = panelPosition;
            _lastPointerLocal = PanelToTargetLocal(panelPosition);
            _unitScale = ComputeUnitScale();

            _canvas.CapturePointer(evt.pointerId);
            evt.StopImmediatePropagation();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_wasDragging || _draggingHandle < 0 || evt.pointerId != _activePointerId)
                return;

            Vector2 panelPosition = evt.position;
            if (ApplyDrag(panelPosition))
            {
                _pendingPersist = true;
                UpdateVisuals();
            }

            _lastPointerPanel = panelPosition;
            _lastPointerLocal = PanelToTargetLocal(panelPosition);
            evt.StopImmediatePropagation();
        }

        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!_wasDragging || evt.pointerId != _activePointerId)
                return;

            _draggingHandle = -1;
            _activePointerId = -1;
            _wasDragging = false;

            if (_canvas.HasPointerCapture(evt.pointerId))
                _canvas.ReleasePointer(evt.pointerId);

            CommitPendingPersist(force: true);
            evt.StopImmediatePropagation();
        }

        private void OnKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Z && (evt.ctrlKey || evt.commandKey) && Undo())
                evt.StopImmediatePropagation();
        }

        private bool ApplyDrag(Vector2 panelPosition)
        {
            _cachedCollider = FindRuntimeCollider(_target);
            if (_cachedCollider == null)
                return false;

            Vector2 localPoint = PanelToTargetLocal(panelPosition);
            Vector2 localDelta = localPoint - _lastPointerLocal;
            Vector2 panelDelta = panelPosition - _lastPointerPanel;
            if (localDelta.sqrMagnitude < 0.000001f && panelDelta.sqrMagnitude > 0f)
            {
                localDelta = new Vector2(
                    panelDelta.x / Mathf.Max(MinSize, _unitScale.x),
                    -panelDelta.y / Mathf.Max(MinSize, _unitScale.y));
            }

            if (_cachedCollider is BoxCollider2D box)
                return ApplyBoxDrag(box, localDelta);

            if (_cachedCollider is CircleCollider2D circle)
                return ApplyCircleDrag(circle, localPoint);

            if (_cachedCollider is CapsuleCollider2D capsule)
                return ApplyCapsuleDrag(capsule, localPoint);

            return false;
        }

        private bool ApplyBoxDrag(BoxCollider2D box, Vector2 localDelta)
        {
            Vector2 size = box.size;
            Vector2 offset = box.offset;

            if (_draggingHandle < 4)
            {
                Vector2 sign = BoxCornerSigns[_draggingHandle];
                ApplyAxisDelta(ref size, ref offset, Vector2.right * sign.x, localDelta.x);
                ApplyAxisDelta(ref size, ref offset, Vector2.up * sign.y, localDelta.y);
            }
            else
            {
                Vector2 sign = BoxEdgeSigns[_draggingHandle - 4];
                if (Mathf.Abs(sign.x) > 0.5f)
                    ApplyAxisDelta(ref size, ref offset, Vector2.right * sign.x, localDelta.x);
                else
                    ApplyAxisDelta(ref size, ref offset, Vector2.up * sign.y, localDelta.y);
            }

            if (Approximately(box.size, size) && Approximately(box.offset, offset))
                return false;

            box.size = size;
            box.offset = offset;
            return true;
        }

        private bool ApplyCircleDrag(CircleCollider2D circle, Vector2 localPoint)
        {
            float radius = Mathf.Max(MinSize, Vector2.Distance(localPoint, circle.offset));
            if (Mathf.Approximately(circle.radius, radius))
                return false;

            circle.radius = radius;
            return true;
        }

        private bool ApplyCapsuleDrag(CapsuleCollider2D capsule, Vector2 localPoint)
        {
            Vector2 size = capsule.size;
            if (_draggingHandle == 0)
            {
                float halfX = Mathf.Max(MinSize, Mathf.Abs(localPoint.x - capsule.offset.x));
                size.x = halfX * 2f;
            }
            else
            {
                float halfY = Mathf.Max(MinSize, Mathf.Abs(localPoint.y - capsule.offset.y));
                size.y = halfY * 2f;
            }

            if (Approximately(capsule.size, size))
                return false;

            capsule.size = size;
            return true;
        }

        private void UpdateVisuals()
        {
            if (_target == null || _canvas == null || _previewCamera == null)
            {
                HideHandles();
                HideOutline();
                return;
            }

            _cachedCollider = FindRuntimeCollider(_target);
            if (_cachedCollider == null)
            {
                HideHandles();
                HideOutline();
                return;
            }

            EnsureHandleDots();
            EnsureOutline();

            if (_cachedCollider is BoxCollider2D box)
            {
                DrawBox(box);
                return;
            }

            if (_cachedCollider is CircleCollider2D circle)
            {
                DrawCircle(circle);
                return;
            }

            if (_cachedCollider is CapsuleCollider2D capsule)
            {
                DrawCapsule(capsule);
                return;
            }

            HideHandles();
            HideOutline();
        }

        private void DrawBox(BoxCollider2D box)
        {
            Vector2 half = box.size * 0.5f;
            Vector2 offset = box.offset;

            Vector3[] corners =
            {
                LocalToWorld(offset + new Vector2(-half.x, -half.y)),
                LocalToWorld(offset + new Vector2(+half.x, -half.y)),
                LocalToWorld(offset + new Vector2(+half.x, +half.y)),
                LocalToWorld(offset + new Vector2(-half.x, +half.y)),
            };

            SetOutline(corners, box.isTrigger);

            for (int i = 0; i < 4; i++)
                SetHandleWorld(i, corners[i]);

            SetHandleWorld(4, LocalToWorld(offset + new Vector2(0f, -half.y)));
            SetHandleWorld(5, LocalToWorld(offset + new Vector2(+half.x, 0f)));
            SetHandleWorld(6, LocalToWorld(offset + new Vector2(0f, +half.y)));
            SetHandleWorld(7, LocalToWorld(offset + new Vector2(-half.x, 0f)));
            ShowHandleCount(8);
        }

        private void DrawCircle(CircleCollider2D circle)
        {
            Vector2 center = circle.offset;
            float radius = Mathf.Max(MinSize, circle.radius);
            Vector3[] points = new Vector3[CircleSegments];

            for (int i = 0; i < CircleSegments; i++)
            {
                float angle = i * Mathf.PI * 2f / CircleSegments;
                points[i] = LocalToWorld(center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius);
            }

            SetOutline(points, circle.isTrigger);
            SetHandleWorld(0, LocalToWorld(center + Vector2.right * radius));
            SetHandleWorld(1, LocalToWorld(center + Vector2.up * radius));
            ShowHandleCount(2);
        }

        private void DrawCapsule(CapsuleCollider2D capsule)
        {
            Vector2 half = capsule.size * 0.5f;
            Vector2 offset = capsule.offset;

            Vector3[] corners =
            {
                LocalToWorld(offset + new Vector2(-half.x, -half.y)),
                LocalToWorld(offset + new Vector2(+half.x, -half.y)),
                LocalToWorld(offset + new Vector2(+half.x, +half.y)),
                LocalToWorld(offset + new Vector2(-half.x, +half.y)),
            };

            SetOutline(corners, capsule.isTrigger);
            SetHandleWorld(0, LocalToWorld(offset + new Vector2(+half.x, 0f)));
            SetHandleWorld(1, LocalToWorld(offset + new Vector2(0f, +half.y)));
            ShowHandleCount(2);
        }

        private void SetOutline(Vector3[] positions, bool isTrigger)
        {
            if (_outline == null)
                return;

            Color color = isTrigger ? OutlineTriggerColor : OutlineColor;
            _outline.enabled = true;
            _outline.loop = true;
            _outline.positionCount = positions.Length;
            _outline.startColor = color;
            _outline.endColor = color;
            _outline.SetPositions(positions);
        }

        private void SetHandleWorld(int index, Vector3 world)
        {
            if (index < 0 || index >= _handleDots.Length || _handleDots[index] == null)
                return;

            if (!WorldToCanvasLocal(world, out Vector2 local))
            {
                _handleDots[index].style.display = DisplayStyle.None;
                _handleVisible[index] = false;
                return;
            }

            _handleCanvasPositions[index] = local;
            _handleVisible[index] = true;
            _handleDots[index].style.display = DisplayStyle.Flex;
            _handleDots[index].style.left = local.x - HandleDotSize * 0.5f;
            _handleDots[index].style.top = local.y - HandleDotSize * 0.5f;
        }

        private void ShowHandleCount(int count)
        {
            _visibleHandleCount = Mathf.Clamp(count, 0, MaxHandles);
            for (int i = _visibleHandleCount; i < _handleDots.Length; i++)
            {
                if (_handleDots[i] != null)
                    _handleDots[i].style.display = DisplayStyle.None;
                _handleVisible[i] = false;
            }
        }

        private int HitTestHandle(Vector2 panelPosition)
        {
            Vector2 local = _canvas.WorldToLocal(panelPosition);
            for (int i = 0; i < _visibleHandleCount; i++)
            {
                if (_handleDots[i] == null || !_handleVisible[i])
                    continue;

                if (Vector2.Distance(local, _handleCanvasPositions[i]) <= HandleHitRadius)
                    return i;
            }

            return -1;
        }

        private void CommitPendingPersist(bool force)
        {
            bool changed = UpsertColliderOverride(_currentDoc, force || _pendingPersist);
            if (changed)
                RequestSave();

            if (force || changed)
                _pendingPersist = false;
        }

        private bool UpsertColliderOverride(RoomLayoutData doc, bool force)
        {
            if (doc == null || _target == null || _cachedCollider == null)
                return false;

            string instanceId = ResolveInstanceId(doc);
            if (string.IsNullOrEmpty(instanceId))
                return false;

            if (doc.collider_overrides == null)
                doc.collider_overrides = new List<ColliderOverrideData>();

            ColliderState state = CaptureState(_cachedCollider);
            ColliderOverrideData existing = FindOverride(doc, instanceId);
            bool differsFromBaseline = !_hasBaseline || !StatesEqual(state, _baselineState);

            if (!force && existing == null && !differsFromBaseline)
                return false;

            if (existing == null)
            {
                existing = new ColliderOverrideData { instance_id = instanceId };
                doc.collider_overrides.Add(existing);
            }

            if (OverrideEquals(existing, state))
                return false;

            existing.instance_id = instanceId;
            existing.size = new[] { state.size.x, state.size.y };
            existing.offset = new[] { state.offset.x, state.offset.y };
            existing.shape = state.shape.ToString();
            return true;
        }

        private void ApplyExistingOverride(RoomLayoutData doc)
        {
            string instanceId = ResolveInstanceId(doc);
            if (doc == null || string.IsNullOrEmpty(instanceId))
                return;

            ColliderOverrideData existing = FindOverride(doc, instanceId);
            if (existing == null)
                return;

            ColliderState state = CaptureState(_cachedCollider);
            if (!string.IsNullOrEmpty(existing.shape) &&
                System.Enum.TryParse(existing.shape, out ColliderShape parsedShape))
            {
                state.shape = parsedShape;
            }

            state.size = ToVector2(existing.size, state.size);
            state.offset = ToVector2(existing.offset, state.offset);
            ApplyColliderState(_target, state);
            _cachedCollider = FindRuntimeCollider(_target);
        }

        private string ResolveInstanceId(RoomLayoutData doc)
        {
            if (!string.IsNullOrEmpty(_instanceId))
                return _instanceId;

            if (doc == null || doc.prop_instances == null || _target == null)
                return null;

            PropData best = null;
            float bestSqr = float.MaxValue;
            Vector3 targetPos = _target.transform.position;

            foreach (PropData prop in doc.prop_instances)
            {
                if (prop == null || string.IsNullOrEmpty(prop.instance_id))
                    continue;

                if (_targetEntry != null && !string.IsNullOrEmpty(_targetEntry.guid) &&
                    prop.prefab_guid != _targetEntry.guid)
                    continue;

                float sqr = (ToVector3(prop.position) - targetPos).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    bestSqr = sqr;
                    best = prop;
                }
            }

            if (best == null)
            {
                foreach (PropData prop in doc.prop_instances)
                {
                    if (prop == null || string.IsNullOrEmpty(prop.instance_id))
                        continue;

                    float sqr = (ToVector3(prop.position) - targetPos).sqrMagnitude;
                    if (sqr < bestSqr)
                    {
                        bestSqr = sqr;
                        best = prop;
                    }
                }
            }

            _instanceId = best?.instance_id;
            return _instanceId;
        }

        private void RequestSave()
        {
            if (_bootstrap == null)
                _bootstrap = Object.FindFirstObjectByType<ToolBootstrap>();

            if (_bootstrap != null)
                _bootstrap.RequestSave();
        }

        private void EnsureHandleDots()
        {
            if (_canvas == null)
                return;

            for (int i = 0; i < _handleDots.Length; i++)
            {
                if (_handleDots[i] != null)
                    continue;

                VisualElement dot = new VisualElement { name = "collider-handle-" + i };
                dot.AddToClassList("collider-handle");
                dot.pickingMode = PickingMode.Ignore;
                dot.style.position = Position.Absolute;
                dot.style.width = HandleDotSize;
                dot.style.height = HandleDotSize;
                dot.style.display = DisplayStyle.None;
                _canvas.Add(dot);
                _handleDots[i] = dot;
            }
        }

        private void EnsureOutline()
        {
            if (_outline != null)
                return;

            GameObject go = new GameObject("[RuntimeColliderOutline]");
            _outline = go.AddComponent<LineRenderer>();
            _outline.useWorldSpace = true;
            _outline.loop = true;
            _outline.widthMultiplier = LineWidth;
            _outline.numCapVertices = 2;
            _outline.numCornerVertices = 2;
            _outline.sortingOrder = 5000;
            _outline.enabled = false;

            Shader shader = Shader.Find("Sprites/Default");
            if (shader != null)
                _outline.material = new Material(shader);
        }

        private void HideHandles()
        {
            _visibleHandleCount = 0;
            for (int i = 0; i < _handleDots.Length; i++)
            {
                if (_handleDots[i] != null)
                    _handleDots[i].style.display = DisplayStyle.None;
                _handleVisible[i] = false;
            }
        }

        private void HideOutline()
        {
            if (_outline != null)
            {
                _outline.enabled = false;
                _outline.positionCount = 0;
            }
        }

        private Vector3 LocalToWorld(Vector2 local)
        {
            return _target.transform.TransformPoint(new Vector3(local.x, local.y, 0f));
        }

        private bool WorldToCanvasLocal(Vector3 world, out Vector2 local)
        {
            local = default;
            if (_previewCamera == null || _canvas == null)
                return false;

            Vector3 screen = _previewCamera.WorldToScreenPoint(world);
            if (screen.z < 0f)
                return false;

            Vector2 panel = new Vector2(screen.x, Screen.height - screen.y);
            local = _canvas.WorldToLocal(panel);
            return true;
        }

        private Vector2 PanelToTargetLocal(Vector2 panelPosition)
        {
            if (_target == null || _previewCamera == null)
                return Vector2.zero;

            Vector2 screen = new Vector2(panelPosition.x, Screen.height - panelPosition.y);
            Vector3 targetWorld = _target.transform.position;
            float depth = Vector3.Dot(targetWorld - _previewCamera.transform.position, _previewCamera.transform.forward);
            depth = Mathf.Max(_previewCamera.nearClipPlane, Mathf.Abs(depth));
            Vector3 world = _previewCamera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, depth));
            Vector3 local = _target.transform.InverseTransformPoint(world);
            return new Vector2(local.x, local.y);
        }

        private Vector2 ComputeUnitScale()
        {
            if (_target == null || _previewCamera == null)
                return Vector2.one;

            Vector3 origin = _target.transform.TransformPoint(Vector3.zero);
            Vector3 right = _target.transform.TransformPoint(Vector3.right);
            Vector3 up = _target.transform.TransformPoint(Vector3.up);

            if (!WorldToCanvasLocal(origin, out Vector2 o) ||
                !WorldToCanvasLocal(right, out Vector2 r) ||
                !WorldToCanvasLocal(up, out Vector2 u))
            {
                return Vector2.one;
            }

            return new Vector2(
                Mathf.Max(MinSize, Vector2.Distance(o, r)),
                Mathf.Max(MinSize, Vector2.Distance(o, u)));
        }

        private void PushUndo(Collider2D col)
        {
            if (col == null)
                return;

            if (_undoStack.Count >= UndoDepth)
                return;

            _undoStack.Push(CaptureState(col));
        }

        private static ColliderState CaptureState(Collider2D col)
        {
            ColliderState state;
            state.isTrigger = col != null && col.isTrigger;
            state.offset = col != null ? col.offset : Vector2.zero;
            state.shape = ResolveShape(col);

            if (col is BoxCollider2D box)
                state.size = box.size;
            else if (col is CircleCollider2D circle)
                state.size = new Vector2(circle.radius, circle.radius);
            else if (col is CapsuleCollider2D capsule)
                state.size = capsule.size;
            else
                state.size = Vector2.one;

            return state;
        }

        private static void ApplyColliderState(GameObject target, ColliderState state)
        {
            if (target == null)
                return;

            Collider2D col = FindRuntimeCollider(target);
            if (col == null || ResolveShape(col) != state.shape)
                col = SwapColliderShape(target, state.shape);

            if (col == null)
                return;

            col.isTrigger = state.isTrigger;
            col.offset = state.offset;

            if (col is BoxCollider2D box)
                box.size = state.size;
            else if (col is CircleCollider2D circle)
                circle.radius = Mathf.Max(MinSize, state.size.x);
            else if (col is CapsuleCollider2D capsule)
                capsule.size = state.size;
        }

        private static Collider2D SwapColliderShape(GameObject target, ColliderShape shape)
        {
            if (target == null)
                return null;

            if (shape == ColliderShape.Polygon)
            {
                Debug.LogWarning("[RuntimeColliderHandles] Polygon collider deferred - shape not changed.");
                return FindRuntimeCollider(target);
            }

            Collider2D existing = FindRuntimeCollider(target);
            if (existing != null)
                Object.Destroy(existing);

            switch (shape)
            {
                case ColliderShape.Circle:
                    return target.AddComponent<CircleCollider2D>();
                case ColliderShape.Capsule:
                    return target.AddComponent<CapsuleCollider2D>();
                case ColliderShape.Box:
                default:
                    return target.AddComponent<BoxCollider2D>();
            }
        }

        private static Collider2D FindRuntimeCollider(GameObject target)
        {
            if (target == null)
                return null;

            Collider2D[] colliders = target.GetComponents<Collider2D>();
            if (colliders == null || colliders.Length == 0)
                return null;

            return colliders[colliders.Length - 1];
        }

        private static ColliderShape ResolveShape(Collider2D col)
        {
            if (col is CircleCollider2D) return ColliderShape.Circle;
            if (col is CapsuleCollider2D) return ColliderShape.Capsule;
            if (col is PolygonCollider2D) return ColliderShape.Polygon;
            return ColliderShape.Box;
        }

        private static void ApplyAxisDelta(ref Vector2 size, ref Vector2 offset, Vector2 signedAxis, float delta)
        {
            if (Mathf.Abs(signedAxis.x) > 0.5f)
            {
                float opp = offset.x - signedAxis.x * (size.x * 0.5f);
                float drag = offset.x + signedAxis.x * (size.x * 0.5f) + signedAxis.x * delta;
                offset.x = (opp + drag) * 0.5f;
                size.x = Mathf.Max(MinSize, Mathf.Abs(drag - opp));
            }
            else
            {
                float opp = offset.y - signedAxis.y * (size.y * 0.5f);
                float drag = offset.y + signedAxis.y * (size.y * 0.5f) + signedAxis.y * delta;
                offset.y = (opp + drag) * 0.5f;
                size.y = Mathf.Max(MinSize, Mathf.Abs(drag - opp));
            }
        }

        private static ColliderOverrideData FindOverride(RoomLayoutData doc, string instanceId)
        {
            if (doc == null || doc.collider_overrides == null || string.IsNullOrEmpty(instanceId))
                return null;

            foreach (ColliderOverrideData data in doc.collider_overrides)
            {
                if (data != null && data.instance_id == instanceId)
                    return data;
            }

            return null;
        }

        private static bool OverrideEquals(ColliderOverrideData data, ColliderState state)
        {
            if (data == null)
                return false;

            if (data.shape != state.shape.ToString())
                return false;

            return Approximately(ToVector2(data.size, Vector2.zero), state.size) &&
                   Approximately(ToVector2(data.offset, Vector2.zero), state.offset);
        }

        private static bool StatesEqual(ColliderState a, ColliderState b)
        {
            return a.shape == b.shape &&
                   a.isTrigger == b.isTrigger &&
                   Approximately(a.size, b.size) &&
                   Approximately(a.offset, b.offset);
        }

        private static bool Approximately(Vector2 a, Vector2 b)
        {
            return Mathf.Abs(a.x - b.x) < 0.0001f && Mathf.Abs(a.y - b.y) < 0.0001f;
        }

        private static Vector2 ToVector2(float[] value, Vector2 fallback)
        {
            if (value == null || value.Length < 2)
                return fallback;

            return new Vector2(value[0], value[1]);
        }

        private static Vector3 ToVector3(float[] value)
        {
            if (value == null || value.Length < 3)
                return Vector3.zero;

            return new Vector3(value[0], value[1], value[2]);
        }
    }
}

#endif // RIMA_LIVE_TOOL
