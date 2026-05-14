namespace RIMA.Editor.RoomDesigner
{
    using System;
    using System.Diagnostics;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using UnityEngine.UIElements;
    using Debug = UnityEngine.Debug;
    using Object = UnityEngine.Object;

    public sealed class RoomDesignerCanvas : IDisposable
    {
        private const string StageName = "_RoomDesignerStage";
        private const string LayerName = "RoomDesigner";
        private const float MinZoom = 2f;
        private const float MaxZoom = 64f;
        private static readonly Color CanvasBackground = new Color(0.06f, 0.07f, 0.08f, 1f);

        private readonly RimaRoomDesignerWindow ctx;
        private readonly IMGUIContainer imguiContainer;
        private readonly Plane editPlane = new Plane(Vector3.forward, Vector3.zero);

        private Camera previewCam;
        private Grid grid;
        private GameObject stageRoot;
        private bool disposed;

#if ROOM_DESIGNER_DEBUG_PERF
        private readonly Stopwatch frameWatch = new Stopwatch();
        private double totalFrameMs;
        private int frameSamples;
#endif

        public RoomDesignerCanvas(RimaRoomDesignerWindow ctx)
        {
            this.ctx = ctx;
            imguiContainer = new IMGUIContainer(DrawCanvas)
            {
                name = "room-designer-imgui-canvas"
            };
            imguiContainer.style.flexGrow = 1f;
            imguiContainer.style.backgroundColor = new StyleColor(CanvasBackground);
            imguiContainer.focusable = true;
            CreateSceneObjects();
        }

        public VisualElement Element => imguiContainer;
        public GameObject StageRoot => stageRoot;
        public Tilemap BaseTilemap { get; private set; }
        public Tilemap DecalTilemap { get; private set; }
        public Tilemap WallFrontTilemap { get; private set; }
        public Tilemap WallTopTilemap { get; private set; }
        public Transform PropContainer { get; private set; }
        public Tilemap FloorTilemap => BaseTilemap;
        public Tilemap WallsTilemap => WallFrontTilemap;
        public Tilemap DecalsTilemap => DecalTilemap;

        public void ClearTilemaps()
        {
            BaseTilemap?.ClearAllTiles();
            DecalTilemap?.ClearAllTiles();
            WallFrontTilemap?.ClearAllTiles();
            WallTopTilemap?.ClearAllTiles();
            ClearProps();
            ctx.MarkDirty();
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            disposed = true;
            if (stageRoot != null)
            {
                Object.DestroyImmediate(stageRoot);
            }

            stageRoot = null;
            previewCam = null;
            grid = null;
            BaseTilemap = null;
            DecalTilemap = null;
            WallFrontTilemap = null;
            WallTopTilemap = null;
            PropContainer = null;
        }

        private void CreateSceneObjects()
        {
            int designerLayer = LayerMask.NameToLayer(LayerName);
            int stageLayer = designerLayer >= 0 ? designerLayer : 0;
            int cullingMask = designerLayer >= 0 ? 1 << designerLayer : 0;
            if (designerLayer < 0)
            {
                Debug.LogWarning("Add RoomDesigner Unity layer for preview");
            }

            stageRoot = new GameObject(StageName);
            stageRoot.hideFlags = HideFlags.DontSave;
            stageRoot.layer = stageLayer;

            grid = stageRoot.AddComponent<Grid>();
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            grid.cellSwizzle = GridLayout.CellSwizzle.XYZ;
            grid.cellSize = new Vector3(1f, 0.5f, 0f);

            BaseTilemap = CreateTilemap("BaseTilemap", stageLayer, 0, false);
            DecalTilemap = CreateTilemap("DecalTilemap", stageLayer, 1, false);
            WallFrontTilemap = CreateTilemap("WallsTilemap_Front", stageLayer, 2, true);
            WallTopTilemap = CreateTilemap("WallsTilemap_Top", stageLayer, 3, false);
            PropContainer = CreatePropContainer(stageLayer);

            var cameraObject = new GameObject("PreviewCamera");
            cameraObject.hideFlags = HideFlags.DontSave;
            cameraObject.layer = stageLayer;
            cameraObject.transform.SetParent(stageRoot.transform, false);
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);

            previewCam = cameraObject.AddComponent<Camera>();
            previewCam.orthographic = true;
            previewCam.orthographicSize = 8f;
            previewCam.clearFlags = CameraClearFlags.SolidColor;
            previewCam.backgroundColor = CanvasBackground;
            previewCam.nearClipPlane = 0.01f;
            previewCam.farClipPlane = 100f;
            previewCam.cullingMask = cullingMask;
            previewCam.enabled = false;
        }

        private Tilemap CreateTilemap(string name, int layer, int sortingOrder, bool hasCollider)
        {
            var tilemapObject = new GameObject(name);
            tilemapObject.hideFlags = HideFlags.DontSave;
            tilemapObject.layer = layer;
            tilemapObject.transform.SetParent(stageRoot.transform, false);

            var tilemap = tilemapObject.AddComponent<Tilemap>();
            tilemap.tileAnchor = new Vector3(0.5f, 0f, 0f);
            var renderer = tilemapObject.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = sortingOrder;
            if (hasCollider)
            {
                tilemapObject.AddComponent<TilemapCollider2D>();
            }

            return tilemap;
        }

        private Transform CreatePropContainer(int layer)
        {
            var propObject = new GameObject("PropContainer");
            propObject.hideFlags = HideFlags.DontSave;
            propObject.layer = layer;
            propObject.transform.SetParent(stageRoot.transform, false);
            return propObject.transform;
        }

        private void ClearProps()
        {
            if (PropContainer == null)
            {
                return;
            }

            for (int i = PropContainer.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(PropContainer.GetChild(i).gameObject);
            }
        }

        private void DrawCanvas()
        {
            if (disposed || previewCam == null)
            {
                return;
            }

            Event evt = Event.current;
            Rect rect = GUILayoutUtility.GetRect(
                0f,
                100000f,
                0f,
                100000f,
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));

            previewCam.aspect = Mathf.Max(0.01f, rect.width / Mathf.Max(1f, rect.height));

            bool hovered = rect.Contains(evt.mousePosition);
            ctx.SetCanvasHovered(hovered);

            if (hovered)
            {
                UpdateHoveredCell(evt);
            }

            bool shouldRepaint = ctx.ConsumeDirty();
            shouldRepaint |= HandleInput(evt, rect, hovered);

            if (evt.type == EventType.Repaint)
            {
#if ROOM_DESIGNER_DEBUG_PERF
                frameWatch.Restart();
#endif
                EditorGUI.DrawRect(rect, CanvasBackground);
                DrawPreviewCamera(rect);
                DrawGridOverlay(rect);
#if ROOM_DESIGNER_DEBUG_PERF
                frameWatch.Stop();
                totalFrameMs += frameWatch.Elapsed.TotalMilliseconds;
                frameSamples++;
                if (frameSamples >= 60)
                {
                    Debug.Log($"RoomDesignerCanvas avg repaint: {totalFrameMs / frameSamples:0.00} ms");
                    frameSamples = 0;
                    totalFrameMs = 0d;
                }
#endif
            }

            if (shouldRepaint)
            {
                imguiContainer.MarkDirtyRepaint();
            }
        }

        private void DrawPreviewCamera(Rect rect)
        {
            if (rect.width <= 0f || rect.height <= 0f)
            {
                return;
            }

            float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
            int width = Mathf.Max(1, Mathf.CeilToInt(rect.width * pixelsPerPoint));
            int height = Mathf.Max(1, Mathf.CeilToInt(rect.height * pixelsPerPoint));
            RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32);
            RenderTexture previousActive = RenderTexture.active;
            RenderTexture previousTarget = previewCam.targetTexture;

            try
            {
                previewCam.targetTexture = renderTexture;
                previewCam.aspect = width / (float)height;
                previewCam.Render();
                RenderTexture.active = previousActive;
                GUI.DrawTexture(rect, renderTexture, ScaleMode.StretchToFill, false);
            }
            finally
            {
                previewCam.targetTexture = previousTarget;
                RenderTexture.active = previousActive;
                RenderTexture.ReleaseTemporary(renderTexture);
            }
        }

        private bool HandleInput(Event evt, Rect rect, bool hovered)
        {
            if (!hovered)
            {
                return false;
            }

            bool changed = false;
            switch (evt.type)
            {
                case EventType.ScrollWheel:
                    Zoom(evt.delta.y);
                    evt.Use();
                    changed = true;
                    break;
                case EventType.MouseDown:
                    imguiContainer.Focus();
                    if (evt.button == 0 || evt.button == 1)
                    {
                        ctx.InvokeBrush(evt.button, ctx.HoveredCell);
                        evt.Use();
                        changed = true;
                    }
                    break;
                case EventType.MouseDrag:
                    if (evt.button == 2 || (evt.button == 1 && evt.alt))
                    {
                        Pan(evt.delta, rect);
                        evt.Use();
                        changed = true;
                    }
                    else if (evt.button == 0 || evt.button == 1)
                    {
                        ctx.InvokeBrush(evt.button, ctx.HoveredCell);
                        evt.Use();
                        changed = true;
                    }
                    break;
                case EventType.MouseUp:
                    if ((evt.button == 0 || evt.button == 1) && ctx is RimaRoomDesignerWindow w)
                    {
                        w.OnBrushRelease();
                        evt.Use();
                        changed = true;
                    }
                    break;
            }

            return changed;
        }

        private void UpdateHoveredCell(Event evt)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(evt.mousePosition);
            if (!editPlane.Raycast(ray, out float enter))
            {
                return;
            }

            Vector3 hit = ray.GetPoint(enter);
            Vector3Int cell = grid.WorldToCell(hit);
            if (ctx.HoveredCell != cell)
            {
                ctx.HoveredCell = cell;
            }
        }

        private void Zoom(float wheelDelta)
        {
            float scale = 1f + wheelDelta * 0.05f;
            previewCam.orthographicSize = Mathf.Clamp(previewCam.orthographicSize * scale, MinZoom, MaxZoom);
        }

        private void Pan(Vector2 mouseDelta, Rect rect)
        {
            float worldHeight = previewCam.orthographicSize * 2f;
            float worldWidth = worldHeight * previewCam.aspect;
            Vector3 offset = new Vector3(
                -mouseDelta.x / Mathf.Max(1f, rect.width) * worldWidth,
                mouseDelta.y / Mathf.Max(1f, rect.height) * worldHeight,
                0f);
            previewCam.transform.position += offset;
        }

        private void DrawGridOverlay(Rect rect)
        {
            Handles.BeginGUI();
            Color oldColor = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, 0.05f);

            const float spacing = 32f;
            for (float x = rect.xMin; x < rect.xMax; x += spacing)
            {
                GUI.DrawTexture(new Rect(x, rect.yMin, 1f, rect.height), Texture2D.whiteTexture);
            }

            for (float y = rect.yMin; y < rect.yMax; y += spacing)
            {
                GUI.DrawTexture(new Rect(rect.xMin, y, rect.width, 1f), Texture2D.whiteTexture);
            }

            GUI.color = oldColor;
            Handles.EndGUI();
        }
    }
}
