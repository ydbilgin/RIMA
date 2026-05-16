#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using RIMA.MapDesigner.Brush.Data;
using RIMA.MapDesigner.Brush.Editor.UI.Panels;
using RIMA.MapDesigner.Brush.Editor.UI.Hotkeys;
using RIMA.MapDesigner.Props.Editor;

namespace RIMA.MapDesigner.Brush.Editor.UI
{
    public enum BrushToolMode { Pick, Brush, Erase, Composite, SmartFill, Props }

    public class MapDesignerBrushWindow : EditorWindow
    {
        public const float LeftPanelWidth = 260f;
        public const float RightPanelWidth = 280f;

        [SerializeField] private MapDesignerBrushPresetSO selectedBrush;
        [SerializeField] private BrushPackSO activePack;
        [SerializeField] private BiomeSkinSO activeSkin;
        [SerializeField] private int activeSeed = 12345;
        [SerializeField] private BrushToolMode toolMode = BrushToolMode.Brush;
        [SerializeField] private float brushSize = 64f;
        [SerializeField] private string searchFilter = string.Empty;
        [SerializeField] private BrushCategory categoryFilter;
        [SerializeField] private bool categoryFilterAll = true;

        private BrushPalettePanel palettePanel;
        private BrushSettingsPanel settingsPanel;
        private LayerVisibilityPanel layerPanel;
        private BrushSceneTooling sceneTooling;
        private PropsTab propsTab;
        private static MapDesignerBrushWindow current;

        public MapDesignerBrushPresetSO SelectedBrush => selectedBrush;
        public BrushPackSO ActivePack => activePack;
        public BiomeSkinSO ActiveSkin => activeSkin;
        public int ActiveSeed => activeSeed;
        public BrushToolMode ToolMode => toolMode;
        public float BrushSize => brushSize;

        [MenuItem("RIMA/Map Designer/Brush Tool", priority = 20)]
        public static void Open()
        {
            var w = GetWindow<MapDesignerBrushWindow>("RIMA Brush Tool");
            w.minSize = new Vector2(900f, 540f);
            w.Show();
        }

        public static MapDesignerBrushWindow GetCurrent() => current;

        private void OnEnable()
        {
            current = this;
            palettePanel = new BrushPalettePanel(this);
            settingsPanel = new BrushSettingsPanel(this);
            layerPanel = new LayerVisibilityPanel();
            sceneTooling = new BrushSceneTooling(this);
            propsTab = new PropsTab(this);
            SceneView.duringSceneGui -= sceneTooling.OnSceneGUI;
            SceneView.duringSceneGui += sceneTooling.OnSceneGUI;
            SceneView.duringSceneGui -= propsTab.OnSceneGUI;
            SceneView.duringSceneGui += propsTab.OnSceneGUI;
        }

        private void OnDisable()
        {
            if (sceneTooling != null)
            {
                SceneView.duringSceneGui -= sceneTooling.OnSceneGUI;
            }
            if (propsTab != null)
            {
                SceneView.duringSceneGui -= propsTab.OnSceneGUI;
            }
            if (current == this)
            {
                current = null;
            }
        }

        public void SetMode(BrushToolMode mode)
        {
            toolMode = mode;
            if (propsTab != null)
            {
                propsTab.IsActive = mode == BrushToolMode.Props;
            }
            Repaint();
            SceneView.RepaintAll();
        }

        public void SetBrush(MapDesignerBrushPresetSO brush)
        {
            selectedBrush = brush;
            Repaint();
            SceneView.RepaintAll();
        }

        public void SelectBrushBySlot(int slot)
        {
            if (activePack == null || activePack.brushes == null) return;
            foreach (var b in activePack.brushes)
            {
                if (b != null && b.hotkeyIndex == slot)
                {
                    SetBrush(b);
                    return;
                }
            }
        }

        public void AdjustBrushSize(float delta)
        {
            brushSize = Mathf.Clamp(brushSize + delta, 8f, 512f);
            Repaint();
            SceneView.RepaintAll();
        }

        private void OnGUI()
        {
            try
            {
                EnsureInitialized();
                if (propsTab != null)
                {
                    propsTab.IsActive = toolMode == BrushToolMode.Props;
                }
                DrawTopBar();
                DrawToolbar();
                if (toolMode == BrushToolMode.Props)
                {
                    propsTab.Draw();
                }
                else
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        using (new EditorGUILayout.VerticalScope(GUILayout.Width(LeftPanelWidth)))
                        {
                            palettePanel.Draw(ref selectedBrush, activePack, ref searchFilter, ref categoryFilter, ref categoryFilterAll);
                        }
                        using (new EditorGUILayout.VerticalScope())
                        {
                            DrawCenterHint();
                        }
                        using (new EditorGUILayout.VerticalScope(GUILayout.Width(RightPanelWidth)))
                        {
                            settingsPanel.Draw(selectedBrush, ref brushSize, ref activeSeed);
                            EditorGUILayout.Space(6);
                            layerPanel.Draw();
                        }
                    }
                }
                DrawBottomBar();
            }
            catch (ExitGUIException) { throw; }
            catch (Exception ex)
            {
                EditorGUILayout.HelpBox("RIMA Brush Tool failed during repaint. See Console.", MessageType.Error);
                Debug.LogException(ex);
            }
        }

        private void EnsureInitialized()
        {
            if (palettePanel == null) palettePanel = new BrushPalettePanel(this);
            if (settingsPanel == null) settingsPanel = new BrushSettingsPanel(this);
            if (layerPanel == null) layerPanel = new LayerVisibilityPanel();
            if (sceneTooling == null) sceneTooling = new BrushSceneTooling(this);
            if (propsTab == null) propsTab = new PropsTab(this);
        }

        private void DrawTopBar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("Brush Pack:", GUILayout.Width(80));
                activePack = (BrushPackSO)EditorGUILayout.ObjectField(activePack, typeof(BrushPackSO), false, GUILayout.Width(180));
                GUILayout.Space(12);
                GUILayout.Label("Biome Skin:", GUILayout.Width(80));
                activeSkin = (BiomeSkinSO)EditorGUILayout.ObjectField(activeSkin, typeof(BiomeSkinSO), false, GUILayout.Width(180));
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Settings", EditorStyles.toolbarButton, GUILayout.Width(70)))
                {
                    EditorUtility.DisplayDialog("RIMA Brush Settings", "Hotkeys: B=Brush, E=Erase, [/]=size, Alt+1-9=slot, Shift+Click=line. Manage via Edit → Shortcuts.", "OK");
                }
            }
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                DrawToolToggle("Pick", BrushToolMode.Pick);
                DrawToolToggle("Brush (B)", BrushToolMode.Brush);
                DrawToolToggle("Erase (E)", BrushToolMode.Erase);
                DrawToolToggle("Composite", BrushToolMode.Composite);
                DrawToolToggle("Smart Fill", BrushToolMode.SmartFill);
                DrawToolToggle("Props", BrushToolMode.Props);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("↶ Undo", EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    Undo.PerformUndo();
                }
                if (GUILayout.Button("↷ Redo", EditorStyles.toolbarButton, GUILayout.Width(60)))
                {
                    Undo.PerformRedo();
                }
            }
        }

        private void DrawToolToggle(string label, BrushToolMode mode)
        {
            var on = toolMode == mode;
            var newOn = GUILayout.Toggle(on, label, EditorStyles.toolbarButton, GUILayout.Width(90));
            if (newOn && !on) SetMode(mode);
        }

        private void DrawCenterHint()
        {
            GUILayout.FlexibleSpace();
            EditorGUILayout.HelpBox(
                "Paint in the Scene view.\n\n" +
                "• Left panel: select brush\n" +
                "• Right panel: settings + layer visibility\n" +
                "• Hotkeys: B/E/Alt+1-9/[/]/Shift+Click",
                MessageType.Info);
            GUILayout.FlexibleSpace();
        }

        private void DrawBottomBar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (GUILayout.Button("Auto-Dress", EditorStyles.toolbarButton, GUILayout.Width(90)))
                {
                    if (activePack != null)
                    {
                        var room = TryGetCurrentRoom();
                        if (room.HasValue)
                            RIMA.MapDesigner.Brush.Automation.Editor.AutoDressRoom.Run(activePack, room.Value, activeSkin, activeSeed);
                    }
                }
                if (GUILayout.Button("Regenerate Decor", EditorStyles.toolbarButton, GUILayout.Width(120)))
                {
                    if (activePack != null)
                    {
                        var room = TryGetCurrentRoom();
                        if (room.HasValue)
                            RIMA.MapDesigner.Brush.Automation.Editor.RegenerateDecorativeLayers.Run(activePack, room.Value, activeSkin, activeSeed + 1);
                    }
                }
                if (GUILayout.Button("Clear Decor", EditorStyles.toolbarButton, GUILayout.Width(85)))
                {
                    RIMA.MapDesigner.Brush.Automation.Editor.RegenerateDecorativeLayers.ClearLayerContainers();
                }
                GUILayout.Space(8);
                GUILayout.Label($"Mode: {toolMode}", GUILayout.Width(110));
                GUILayout.Label($"Seed: {activeSeed}", GUILayout.Width(90));
                GUILayout.Label($"Size: {brushSize:F0}px", GUILayout.Width(80));
                GUILayout.FlexibleSpace();
                if (toolMode == BrushToolMode.Props)
                {
                    GUILayout.Label("Props placement active");
                }
                else if (selectedBrush != null)
                {
                    GUILayout.Label($"Brush: {selectedBrush.brushName} → {(selectedBrush.operations != null && selectedBrush.operations.Count > 0 ? selectedBrush.operations[0].targetLayer.ToString() : "?")}");
                }
                else
                {
                    GUILayout.Label("No brush selected");
                }
            }
        }

        private RIMA.MapDesigner.RoomData? TryGetCurrentRoom()
        {
            // For V1: fallback — let user populate Layer_L1 manually; full RoomData wiring is Sprint 8 polish.
            // Returning null means Auto-Dress quietly no-ops if scene context not available.
            return null;
        }
    }
}
#endif
