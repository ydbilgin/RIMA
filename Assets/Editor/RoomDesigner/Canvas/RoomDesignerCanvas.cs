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
            imguiContainer.focusable = true;
            CreateSceneObjects();
        }

        public VisualElement Element => imguiContainer;
        public GameObject StageRoot => stageRoot;
        public Tilemap FloorTilemap { get; private set; }
        public Tilemap WallsTilemap { get; private set; }
        public Tilemap DecalsTilemap { get; private set; }

        public void ClearTilemaps()
        {
            FloorTilemap?.ClearAllTiles();
            WallsTilemap?.ClearAllTiles();
            DecalsTilemap?.ClearAllTiles();
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
            FloorTilemap = null;
            WallsTilemap = null;
            DecalsTilemap = null;
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

            FloorTilemap = CreateTilemap("Floor", stageLayer, 0);
            WallsTilemap = CreateTilemap("Walls", stageLayer, 10);
            DecalsTilemap = CreateTilemap("Decals", stageLayer, 20);

            var cameraObject = new GameObject("PreviewCamera");
            cameraObject.hideFlags = HideFlags.DontSave;
            cameraObject.layer = stageLayer;
            cameraObject.transform.SetParent(stageRoot.transform, false);
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);

            previewCam = cameraObject.AddComponent<Camera>();
            previewCam.orthographic = true;
            previewCam.orthographicSize = 8f;
            previewCam.clearFlags = CameraClearFlags.SolidColor;
            previewCam.backgroundColor = new Color(0.06f, 0.07f, 0.08f, 1f);
            previewCam.nearClipPlane = 0.01f;
            previewCam.farClipPlane = 100f;
            previewCam.cullingMask = cullingMask;
            previewCam.enabled = false;
        }

        private Tilemap CreateTilemap(string name, int layer, int sortingOrder)
        {
            var tilemapObject = new GameObject(name);
            tilemapObject.hideFlags = HideFlags.DontSave;
            tilemapObject.layer = layer;
            tilemapObject.transform.SetParent(stageRoot.transform, false);

            var tilemap = tilemapObject.AddComponent<Tilemap>();
            tilemap.tileAnchor = new Vector3(0.5f, 0f, 0f);
            var renderer = tilemapObject.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = sortingOrder;
            return tilemap;
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

            float pixelsPerPoint = EditorGUIUtility.pixelsPerPoint;
            previewCam.aspect = Mathf.Max(0.01f, rect.width / Mathf.Max(1f, rect.height));
            previewCam.pixelRect = new Rect(0f, 0f, rect.width * pixelsPerPoint, rect.height * pixelsPerPoint);
            Handles.SetCamera(rect, previewCam);

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
                Handles.DrawCamera(rect, previewCam);
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
