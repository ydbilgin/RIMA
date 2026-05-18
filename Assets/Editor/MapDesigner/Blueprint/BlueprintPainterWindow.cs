#if UNITY_EDITOR
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Editor.Blueprint
{
    public sealed class BlueprintPainterWindow : EditorWindow
    {
        private const float LeftPanelWidth = 200f;
        private const float RightPanelWidth = 200f;
        private const float BottomHeight = 40f;
        private const float CellSize = 16f;
        private const string DefaultProfilePath = "Assets/Data/Blueprint/Profiles/profile_combat_room_default.asset";

        [SerializeField] private BlueprintProfileSO activeProfile;
        [SerializeField] private Transform activeRoomRoot;
        [SerializeField] private string selectedZoneId;
        [SerializeField] private int brushSize = 1;
        [SerializeField] private int seed = 1337;
        [SerializeField] private string statusText = "Set Active Profile first";

        private readonly BlueprintCanvas canvas = new BlueprintCanvas();
        private Vector2Int hoverCell = new Vector2Int(-1, -1);

        [MenuItem("Tools/RIMA/Map Designer/Blueprint Painter")]
        public static void ShowWindow()
        {
            var window = GetWindow<BlueprintPainterWindow>("Blueprint Painter");
            window.minSize = new Vector2(760f, 460f);
            window.Show();
        }

        private void OnEnable()
        {
            minSize = new Vector2(760f, 460f);
            if (activeProfile == null)
            {
                activeProfile = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(DefaultProfilePath);
            }

            SyncCanvasGrid();
            EnsureSelectedZone();
            UpdateStatusForProfile();
        }

        private void OnGUI()
        {
            SyncCanvasGrid();
            Rect layoutRect = GUILayoutUtility.GetRect(position.width, Mathf.Max(320f, position.height - BottomHeight), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            Rect leftRect = new Rect(layoutRect.x, layoutRect.y, LeftPanelWidth, layoutRect.height);
            Rect rightRect = new Rect(layoutRect.xMax - RightPanelWidth, layoutRect.y, RightPanelWidth, layoutRect.height);
            Rect centerRect = new Rect(leftRect.xMax + 8f, layoutRect.y, Mathf.Max(240f, rightRect.x - leftRect.xMax - 16f), layoutRect.height);
            Rect bottomRect = new Rect(layoutRect.x, layoutRect.yMax, layoutRect.width, BottomHeight);

            GUILayout.BeginArea(leftRect, EditorStyles.helpBox);
            DrawLeftPanel();
            GUILayout.EndArea();

            GUILayout.BeginArea(centerRect, EditorStyles.helpBox);
            DrawCanvasPanel();
            GUILayout.EndArea();

            GUILayout.BeginArea(rightRect, EditorStyles.helpBox);
            DrawRightPanel();
            GUILayout.EndArea();

            GUILayout.BeginArea(bottomRect, EditorStyles.toolbar);
            DrawBottomBar();
            GUILayout.EndArea();
        }

        private void DrawLeftPanel()
        {
            GUILayout.Label("Zone Brush", EditorStyles.boldLabel);
            if (activeProfile == null || activeProfile.zones == null || activeProfile.zones.Length == 0)
            {
                EditorGUILayout.HelpBox("Set Active Profile first", MessageType.Info);
                return;
            }

            for (int i = 0; i < activeProfile.zones.Length; i++)
            {
                BlueprintZoneTypeSO zone = activeProfile.zones[i];
                if (zone == null)
                {
                    continue;
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    Rect swatch = GUILayoutUtility.GetRect(18f, 18f, GUILayout.Width(18f), GUILayout.Height(18f));
                    EditorGUI.DrawRect(swatch, zone.brushColor);
                    bool selected = selectedZoneId == zone.zoneId;
                    if (GUILayout.Toggle(selected, string.IsNullOrEmpty(zone.displayName) ? zone.zoneId : zone.displayName, EditorStyles.miniButton) && !selected)
                    {
                        selectedZoneId = zone.zoneId;
                    }
                }
            }

            EditorGUILayout.Space(8f);
            brushSize = EditorGUILayout.IntSlider("Brush Size", brushSize, 1, 5);
            BlueprintZoneTypeSO selectedZone = activeProfile.GetZone(selectedZoneId);
            if (selectedZone != null)
            {
                EditorGUI.BeginChangeCheck();
                float nextDensity = EditorGUILayout.Slider("Density", selectedZone.defaultDensity, 0f, 1f);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(selectedZone, "Set Blueprint Zone Density");
                    selectedZone.defaultDensity = nextDensity;
                    EditorUtility.SetDirty(selectedZone);
                }
            }
        }

        private void DrawCanvasPanel()
        {
            GUILayout.Label("Intent Map", EditorStyles.boldLabel);
            Rect gridRect = GUILayoutUtility.GetRect(canvas.GridSize.x * CellSize, canvas.GridSize.y * CellSize, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            Event current = Event.current;
            hoverCell = CellFromMouse(gridRect, current.mousePosition);

            for (int y = canvas.GridSize.y - 1; y >= 0; y--)
            {
                for (int x = 0; x < canvas.GridSize.x; x++)
                {
                    Vector2Int cell = new Vector2Int(x, y);
                    Rect cellRect = CellRect(gridRect, cell);
                    Color color = Color.black;
                    color.a = 0.10f;
                    string zoneId = canvas.GetZoneAt(cell);
                    BlueprintZoneTypeSO zone = activeProfile != null ? activeProfile.GetZone(zoneId) : null;
                    if (zone != null)
                    {
                        color = zone.brushColor;
                        color.a = 0.70f;
                    }

                    EditorGUI.DrawRect(cellRect, color);
                    Handles.color = new Color(0f, 0f, 0f, 0.35f);
                    Handles.DrawLine(new Vector3(cellRect.xMin, cellRect.yMin), new Vector3(cellRect.xMax, cellRect.yMin));
                    Handles.DrawLine(new Vector3(cellRect.xMin, cellRect.yMin), new Vector3(cellRect.xMin, cellRect.yMax));
                }
            }

            if (IsHoverValid())
            {
                Rect hoverRect = CellRect(gridRect, hoverCell);
                EditorGUI.DrawRect(hoverRect, new Color(1f, 1f, 1f, 0.28f));
            }

            HandlePaintInput(current, gridRect);
        }

        private void DrawRightPanel()
        {
            GUILayout.Label("Populate", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            activeProfile = (BlueprintProfileSO)EditorGUILayout.ObjectField("Active Profile", activeProfile, typeof(BlueprintProfileSO), false);
            if (EditorGUI.EndChangeCheck())
            {
                SyncCanvasGrid();
                EnsureSelectedZone();
                UpdateStatusForProfile();
            }

            activeRoomRoot = (Transform)EditorGUILayout.ObjectField("Active Room Root", activeRoomRoot, typeof(Transform), true);
            seed = EditorGUILayout.IntField("Seed", seed);

            using (new EditorGUI.DisabledScope(activeProfile == null || activeRoomRoot == null))
            {
                if (GUILayout.Button("Auto-Populate", GUILayout.Height(28f)))
                {
                    int placed = AutoPopulator.PopulateZones(canvas, activeProfile, activeRoomRoot, seed);
                    statusText = canvas.Count == 0 ? "No zones painted" : $"Auto-Populate placed {placed}";
                }

                if (GUILayout.Button("Adjacency Pass", GUILayout.Height(28f)))
                {
                    int placed = AutoPopulator.PopulateAdjacency(canvas, activeProfile, activeRoomRoot, seed);
                    statusText = canvas.Count == 0 ? "No zones painted" : $"Adjacency Pass placed {placed}";
                }
            }

            if (GUILayout.Button("Clear Blueprint", GUILayout.Height(24f)))
            {
                canvas.Clear();
                statusText = "Blueprint cleared";
            }

            using (new EditorGUI.DisabledScope(activeRoomRoot == null))
            {
                if (GUILayout.Button("Clear Placed Props", GUILayout.Height(24f)))
                {
                    int cleared = AutoPopulator.ClearPlacedProps(activeRoomRoot);
                    statusText = $"Cleared {cleared} placed props";
                }
            }

            if (activeProfile == null)
            {
                statusText = "Set Active Profile first";
            }
            else if (activeRoomRoot == null)
            {
                statusText = "Set Active Room Root first";
            }
        }

        private void DrawBottomBar()
        {
            string coord = IsHoverValid() ? $"Tile {hoverCell.x}, {hoverCell.y}" : "Tile -, -";
            GUILayout.Label(coord, GUILayout.Width(110f));
            GUILayout.Label($"Painted: {canvas.Count}", GUILayout.Width(100f));
            GUILayout.Label(statusText ?? string.Empty);
        }

        private void HandlePaintInput(Event current, Rect gridRect)
        {
            if (!IsHoverValid() || activeProfile == null)
            {
                return;
            }

            bool left = current.button == 0;
            bool right = current.button == 1;
            if ((current.type != EventType.MouseDown && current.type != EventType.MouseDrag) || (!left && !right))
            {
                return;
            }

            if (left && current.shift)
            {
                canvas.FloodFill(hoverCell, selectedZoneId);
                statusText = $"Flood filled {selectedZoneId}";
            }
            else if (left)
            {
                canvas.Paint(hoverCell, selectedZoneId, brushSize);
                statusText = $"Painted {selectedZoneId}";
            }
            else if (right)
            {
                canvas.Erase(hoverCell, brushSize);
                statusText = "Erased cells";
            }

            current.Use();
            Repaint();
        }

        private void SyncCanvasGrid()
        {
            if (activeProfile != null)
            {
                canvas.SetGridSize(activeProfile.gridSize);
            }
        }

        private void EnsureSelectedZone()
        {
            if (activeProfile == null || activeProfile.zones == null || activeProfile.zones.Length == 0)
            {
                selectedZoneId = null;
                return;
            }

            if (activeProfile.GetZone(selectedZoneId) != null)
            {
                return;
            }

            for (int i = 0; i < activeProfile.zones.Length; i++)
            {
                if (activeProfile.zones[i] != null)
                {
                    selectedZoneId = activeProfile.zones[i].zoneId;
                    return;
                }
            }
        }

        private void UpdateStatusForProfile()
        {
            if (activeProfile == null)
            {
                statusText = "Set Active Profile first";
            }
            else if (activeRoomRoot == null)
            {
                statusText = "Set Active Room Root first";
            }
            else
            {
                statusText = "Ready";
            }
        }

        private Vector2Int CellFromMouse(Rect gridRect, Vector2 mousePosition)
        {
            if (!gridRect.Contains(mousePosition))
            {
                return new Vector2Int(-1, -1);
            }

            int x = Mathf.FloorToInt((mousePosition.x - gridRect.xMin) / CellSize);
            int yFromTop = Mathf.FloorToInt((mousePosition.y - gridRect.yMin) / CellSize);
            int y = canvas.GridSize.y - 1 - yFromTop;
            return new Vector2Int(x, y);
        }

        private Rect CellRect(Rect gridRect, Vector2Int cell)
        {
            int yFromTop = canvas.GridSize.y - 1 - cell.y;
            return new Rect(gridRect.xMin + cell.x * CellSize, gridRect.yMin + yFromTop * CellSize, CellSize - 1f, CellSize - 1f);
        }

        private bool IsHoverValid()
        {
            return hoverCell.x >= 0 && hoverCell.y >= 0 && hoverCell.x < canvas.GridSize.x && hoverCell.y < canvas.GridSize.y;
        }
    }
}
#endif
