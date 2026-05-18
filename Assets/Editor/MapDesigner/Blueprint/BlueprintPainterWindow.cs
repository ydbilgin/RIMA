#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Editor.Blueprint
{
    public sealed class BlueprintPainterWindow : EditorWindow
    {
        public const string ActiveRoomRootPrefsKey = "RIMA_BlueprintPainter_ActiveRoomRoot_InstanceID";

        private const float LeftPanelWidth = 210f;
        private const float RightPanelWidth = 240f;
        private const float BottomHeight = 40f;
        private const float CellSize = 16f;
        private const float VisibleZoneAlpha = 1f;
        private const float HiddenZoneAlpha = 0.2f;
        private const string DefaultProfilePath = "Assets/Data/Blueprint/Profiles/profile_combat_room_default.asset";
        private const string MissingRootStatus = "Previous Active Room Root no longer exists, please re-bind";

        [SerializeField] private BlueprintProfileSO activeProfile;
        [SerializeField] private Transform activeRoomRoot;
        [SerializeField] private RoomBlueprintSO activeRoom;
        [SerializeField] private string selectedZoneId;
        [SerializeField] private int brushSize = 1;
        [SerializeField] private int seed = 1337;
        [SerializeField] private string statusText = "Set Active Profile first";
        [SerializeField] private bool layerVisibilityFoldout = true;
        [SerializeField] private List<string> hiddenZoneIds = new List<string>();
        [SerializeField] private Vector2 leftPanelScroll;
        [SerializeField] private Vector2 rightPanelScroll;

        private readonly BlueprintCanvas canvas = new BlueprintCanvas();
        private Vector2Int hoverCell = new Vector2Int(-1, -1);

        [MenuItem("Tools/RIMA/Map Designer/Blueprint Painter")]
        public static void ShowWindow()
        {
            var window = GetWindow<BlueprintPainterWindow>("Blueprint Painter");
            window.minSize = new Vector2(800f, 540f);
            window.Show();
        }

        public static int GenerateRandomSeed(int currentSeed)
        {
            int next = new System.Random(unchecked(System.Environment.TickCount * 31 + Guid.NewGuid().GetHashCode())).Next();
            return next == currentSeed ? unchecked(next + 1) : next;
        }

        public int SeedForTesting
        {
            get => seed;
            set => seed = value;
        }

        public BlueprintCanvas CanvasForTesting => canvas;
        public Transform ActiveRoomRootForTesting => activeRoomRoot;
        public string StatusTextForTesting => statusText;

        public void InvokeOnEnableForTesting()
        {
            OnEnable();
        }

        public void SetActiveRoomRootForTesting(Transform root)
        {
            SetActiveRoomRoot(root);
        }

        public void SetLayerVisibleForTesting(string zoneId, bool visible)
        {
            SetZoneVisible(zoneId, visible);
        }

        public float GetZonePaintAlphaForTesting(string zoneId)
        {
            return GetZonePaintAlpha(zoneId);
        }

        public void RandomizeSeedForTesting()
        {
            SetRandomSeed();
        }

        private void OnEnable()
        {
            minSize = new Vector2(800f, 540f);
            bool preserveMissingRootStatus = string.Equals(statusText, MissingRootStatus, StringComparison.Ordinal);
            if (activeProfile == null)
            {
                activeProfile = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(DefaultProfilePath);
            }

            bool missingRoot = RestoreActiveRoomRootBinding();
            SyncCanvasGrid();
            EnsureSelectedZone();
            SyncLayerVisibility();
            UpdateStatusForProfile();
            if (missingRoot || preserveMissingRootStatus)
            {
                statusText = MissingRootStatus;
            }
        }

        private void OnGUI()
        {
            SyncCanvasGrid();
            Rect layoutRect = GUILayoutUtility.GetRect(position.width, Mathf.Max(360f, position.height - BottomHeight), GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
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
            leftPanelScroll = EditorGUILayout.BeginScrollView(leftPanelScroll);
            GUILayout.Label("Zone Brush", EditorStyles.boldLabel);
            if (activeProfile == null || activeProfile.zones == null || activeProfile.zones.Length == 0)
            {
                EditorGUILayout.HelpBox("Set Active Profile first", MessageType.Info);
                EditorGUILayout.EndScrollView();
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

            DrawLayerVisibilityFoldout();
            EditorGUILayout.EndScrollView();
        }

        private void DrawLayerVisibilityFoldout()
        {
            EditorGUILayout.Space(8f);
            layerVisibilityFoldout = EditorGUILayout.Foldout(layerVisibilityFoldout, "Layer Visibility", true);
            if (!layerVisibilityFoldout || activeProfile == null || activeProfile.zones == null)
            {
                return;
            }

            EditorGUI.indentLevel++;
            for (int i = 0; i < activeProfile.zones.Length; i++)
            {
                BlueprintZoneTypeSO zone = activeProfile.zones[i];
                if (zone == null || string.IsNullOrEmpty(zone.zoneId))
                {
                    continue;
                }

                string label = string.IsNullOrEmpty(zone.displayName) ? zone.zoneId : zone.displayName;
                bool visible = IsZoneVisible(zone.zoneId);
                bool nextVisible = EditorGUILayout.ToggleLeft(label, visible);
                if (nextVisible != visible)
                {
                    SetZoneVisible(zone.zoneId, nextVisible);
                    Repaint();
                }
            }

            EditorGUI.indentLevel--;
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
                        color.a = GetZonePaintAlpha(zone.zoneId);
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
            rightPanelScroll = EditorGUILayout.BeginScrollView(rightPanelScroll);

            GUILayout.Label("Populate", EditorStyles.boldLabel);
            EditorGUI.BeginChangeCheck();
            BlueprintProfileSO nextProfile = (BlueprintProfileSO)EditorGUILayout.ObjectField("Active Profile", activeProfile, typeof(BlueprintProfileSO), false);
            if (EditorGUI.EndChangeCheck())
            {
                activeProfile = nextProfile;
                SyncCanvasGrid();
                EnsureSelectedZone();
                SyncLayerVisibility();
                UpdateStatusForProfile();
            }

            EditorGUI.BeginChangeCheck();
            Transform nextRoot = (Transform)EditorGUILayout.ObjectField("Active Room Root", activeRoomRoot, typeof(Transform), true);
            if (EditorGUI.EndChangeCheck())
            {
                SetActiveRoomRoot(nextRoot);
            }

            seed = EditorGUILayout.IntField("Seed", seed);

            using (new EditorGUI.DisabledScope(activeProfile == null || activeRoomRoot == null))
            {
                if (GUILayout.Button("Auto-Populate", GUILayout.Height(28f)))
                {
                    int placed = AutoPopulator.PopulateZones(canvas, activeProfile, activeRoomRoot, seed);
                    statusText = canvas.Count == 0 ? "No zones painted" : $"Auto-Populate placed {placed}";
                }
            }

            GUILayout.Label("Variant", EditorStyles.boldLabel);
            if (GUILayout.Button("Random Seed", GUILayout.Height(24f)))
            {
                SetRandomSeed();
            }

            using (new EditorGUI.DisabledScope(activeProfile == null || activeRoomRoot == null))
            {
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

            DrawRoomsSection();
            EditorGUILayout.EndScrollView();
        }

        private void DrawRoomsSection()
        {
            EditorGUILayout.Space(8f);
            GUILayout.Label("Rooms", EditorStyles.boldLabel);
            activeRoom = (RoomBlueprintSO)EditorGUILayout.ObjectField("Active Room", activeRoom, typeof(RoomBlueprintSO), false);

            using (new EditorGUI.DisabledScope(activeRoom == null))
            {
                if (GUILayout.Button("Load", GUILayout.Height(24f)))
                {
                    LoadActiveRoom(true);
                }
            }

            using (new EditorGUI.DisabledScope(activeRoom == null))
            {
                if (GUILayout.Button("Save Over", GUILayout.Height(24f)))
                {
                    SaveOverActiveRoom();
                }
            }

            if (GUILayout.Button("Save As New...", GUILayout.Height(24f)))
            {
                SaveAsNewRoom();
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

        private void LoadActiveRoom(bool confirmOverwrite)
        {
            if (activeRoom == null)
            {
                return;
            }

            if (confirmOverwrite && canvas.Count > 0 &&
                !EditorUtility.DisplayDialog("Overwrite current paint?", "Loading this room replaces the current blueprint paint.", "Yes", "No"))
            {
                return;
            }

            (BlueprintCanvas loadedCanvas, int loadedSeed) = RoomSaveLoadService.Load(activeRoom);
            if (activeRoom.profile != null)
            {
                activeProfile = activeRoom.profile;
            }

            seed = loadedSeed;
            CopyCanvas(loadedCanvas);
            EnsureSelectedZone();
            SyncLayerVisibility();
            statusText = $"Loaded {activeRoom.displayName}";
            Repaint();
        }

        private void SaveOverActiveRoom()
        {
            if (activeRoom == null)
            {
                return;
            }

            if (activeProfile != null)
            {
                activeRoom.profile = activeProfile;
            }

            RoomSaveLoadService.Overwrite(activeRoom, canvas, seed);
            statusText = $"Saved {activeRoom.displayName}";
        }

        private void SaveAsNewRoom()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Save Room Blueprint",
                SuggestedRoomFileName(),
                "asset",
                "Save room blueprint",
                RoomSaveLoadService.DefaultRoomsFolder);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            if (!path.Replace('\\', '/').StartsWith("Assets/", StringComparison.Ordinal))
            {
                EditorUtility.DisplayDialog("Invalid Path", "Room blueprints must be saved under Assets/.", "OK");
                return;
            }

            if (AssetDatabase.LoadAssetAtPath<RoomBlueprintSO>(path) != null)
            {
                EditorUtility.DisplayDialog("Asset Exists", "Use Save Over to update an existing room asset.", "OK");
                return;
            }

            if (!path.StartsWith(RoomSaveLoadService.DefaultRoomsFolder + "/", StringComparison.Ordinal))
            {
                Debug.LogWarning($"[BlueprintPainterWindow] Saved room outside default folder: {path}");
            }

            string roomId = Path.GetFileNameWithoutExtension(path);
            string displayName = ObjectNames.NicifyVariableName(roomId);
            activeRoom = RoomSaveLoadService.SaveAsNew(canvas, activeProfile, seed, path, roomId, displayName);
            statusText = $"Saved {activeRoom.displayName}";
        }

        private void CopyCanvas(BlueprintCanvas source)
        {
            canvas.SetGridSize(source.GridSize);
            canvas.Clear();
            foreach (KeyValuePair<Vector2Int, string> pair in source.IntentMap)
            {
                canvas.Paint(pair.Key, pair.Value, 1);
            }
        }

        private void SetRandomSeed()
        {
            seed = GenerateRandomSeed(seed);
            statusText = $"Seed set to {seed}";
        }

        private bool RestoreActiveRoomRootBinding()
        {
            if (activeRoomRoot != null)
            {
                return false;
            }

            int instanceId = EditorPrefs.GetInt(ActiveRoomRootPrefsKey, 0);
            if (instanceId == 0)
            {
                return false;
            }

#pragma warning disable 0618
            activeRoomRoot = EditorUtility.InstanceIDToObject(instanceId) as Transform;
#pragma warning restore 0618
            if (activeRoomRoot != null)
            {
                return false;
            }

            EditorPrefs.DeleteKey(ActiveRoomRootPrefsKey);
            return true;
        }

        private void SetActiveRoomRoot(Transform nextRoot)
        {
            activeRoomRoot = nextRoot;
            if (activeRoomRoot == null)
            {
                EditorPrefs.DeleteKey(ActiveRoomRootPrefsKey);
            }
            else
            {
                EditorPrefs.SetInt(ActiveRoomRootPrefsKey, activeRoomRoot.GetInstanceID());
            }

            UpdateStatusForProfile();
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

        private void SyncLayerVisibility()
        {
            hiddenZoneIds ??= new List<string>();
            if (activeProfile == null || activeProfile.zones == null)
            {
                hiddenZoneIds.Clear();
                return;
            }

            var validZoneIds = new HashSet<string>();
            for (int i = 0; i < activeProfile.zones.Length; i++)
            {
                if (activeProfile.zones[i] != null && !string.IsNullOrEmpty(activeProfile.zones[i].zoneId))
                {
                    validZoneIds.Add(activeProfile.zones[i].zoneId);
                }
            }

            for (int i = hiddenZoneIds.Count - 1; i >= 0; i--)
            {
                if (string.IsNullOrEmpty(hiddenZoneIds[i]) || !validZoneIds.Contains(hiddenZoneIds[i]))
                {
                    hiddenZoneIds.RemoveAt(i);
                }
            }
        }

        private bool IsZoneVisible(string zoneId)
        {
            return string.IsNullOrEmpty(zoneId) || hiddenZoneIds == null || !hiddenZoneIds.Contains(zoneId);
        }

        private void SetZoneVisible(string zoneId, bool visible)
        {
            if (string.IsNullOrEmpty(zoneId))
            {
                return;
            }

            hiddenZoneIds ??= new List<string>();
            if (visible)
            {
                hiddenZoneIds.Remove(zoneId);
            }
            else if (!hiddenZoneIds.Contains(zoneId))
            {
                hiddenZoneIds.Add(zoneId);
            }
        }

        private float GetZonePaintAlpha(string zoneId)
        {
            return IsZoneVisible(zoneId) ? VisibleZoneAlpha : HiddenZoneAlpha;
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

        private string SuggestedRoomFileName()
        {
            if (activeRoom != null && !string.IsNullOrEmpty(activeRoom.roomId))
            {
                return activeRoom.roomId + "_copy";
            }

            return "room_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
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
