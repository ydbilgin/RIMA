#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using RIMA.Walls.V2.EditorTools;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.MapDesigner
{
    public sealed class UnifiedMapDesigner : EditorWindow
    {
        private enum Tab
        {
            TilePainter,
            RoomBuilder
        }

        private const float ToolbarHeight = 42f;
        private const float TabHeight = 34f;
        private const float ShortcutHeight = 24f;
        private const float StatusHeight = 24f;

        private static readonly Color ToolbarColor = new Color(0.08f, 0.09f, 0.11f);
        private static readonly Color TabBarColor = new Color(0.15f, 0.17f, 0.20f);
        private static readonly Color ContentColor = new Color(0.27f, 0.29f, 0.31f);
        private static readonly Color AccentColor = new Color(0.10f, 0.86f, 1.00f);
        private static readonly Color InactiveTabColor = new Color(0.22f, 0.24f, 0.28f);
        private static readonly Color StatusColor = new Color(0.12f, 0.13f, 0.15f);

        [SerializeField] private Tilemap activeTilemap;
        [SerializeField] private Tab selectedTab;
        [SerializeField] private Vector2 contentScroll;
        [SerializeField] private string lastAction = "Ready";

        private MinimalTilePainter tilePainter;
        private RoomPainterWindow roomPainter;

        private MethodInfo tileOnGui;
        private MethodInfo tileOnEnable;
        private MethodInfo tileOnDisable;
        private MethodInfo tileLoadAssets;
        private FieldInfo tilemapField;
        private FieldInfo tileStatusField;
        private FieldInfo tileToolField;
        private FieldInfo tileBrushSizeField;

        private MethodInfo roomOnGui;
        private MethodInfo roomOnEnable;
        private FieldInfo roomBrushField;
        private FieldInfo roomPresetField;
        private FieldInfo roomGeneratedField;

        private GUIStyle toolbarLabelStyle;
        private GUIStyle tabLabelStyle;
        private GUIStyle selectedTabLabelStyle;
        private GUIStyle shortcutStyle;
        private GUIStyle statusStyle;
        private GUIStyle contentStyle;

        [MenuItem("RIMA/Map Designer", priority = 1)]
        public static void Open()
        {
            UnifiedMapDesigner window = GetWindow<UnifiedMapDesigner>("Map Designer");
            window.minSize = new Vector2(400f, 700f);
            window.Show();
            window.Focus();
        }

        private void OnEnable()
        {
            minSize = new Vector2(400f, 700f);
            EnsurePainters();
            if (activeTilemap == null) activeTilemap = FindFloorTilemap();
            PushActiveTilemapToTilePainter();
        }

        private void OnDisable()
        {
            InvokeNoArg(tileOnDisable, tilePainter);
            DestroyHiddenWindow(tilePainter);
            DestroyHiddenWindow(roomPainter);
            tilePainter = null;
            roomPainter = null;
        }

        private void OnGUI()
        {
            EnsurePainters();
            EnsureStyles();
            HandleTilePainterHotkeys();

            DrawTopToolbar();
            CliffGenerateAction.DrawButton(28f);
            DrawTabBar();
            DrawShortcutStrip();
            DrawContentArea();
            DrawUnifiedStatusBar();
        }

        private void EnsurePainters()
        {
            if (tilePainter == null)
            {
                tilePainter = CreateInstance<MinimalTilePainter>();
                tilePainter.hideFlags = HideFlags.HideAndDontSave;
                CacheTilePainterMembers();
                InvokeNoArg(tileOnEnable, tilePainter);
            }

            if (roomPainter == null)
            {
                roomPainter = CreateInstance<RoomPainterWindow>();
                roomPainter.hideFlags = HideFlags.HideAndDontSave;
                CacheRoomPainterMembers();
                InvokeNoArg(roomOnEnable, roomPainter);
            }
        }

        private void CacheTilePainterMembers()
        {
            Type type = typeof(MinimalTilePainter);
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            tileOnGui = type.GetMethod("OnGUI", flags);
            tileOnEnable = type.GetMethod("OnEnable", flags);
            tileOnDisable = type.GetMethod("OnDisable", flags);
            tileLoadAssets = type.GetMethod("LoadTileAssets", flags);
            tilemapField = type.GetField("activeTilemap", flags);
            tileStatusField = type.GetField("status", flags);
            tileToolField = type.GetField("tool", flags);
            tileBrushSizeField = type.GetField("brushSize", flags);
        }

        private void CacheRoomPainterMembers()
        {
            Type type = typeof(RoomPainterWindow);
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            roomOnGui = type.GetMethod("OnGUI", flags);
            roomOnEnable = type.GetMethod("OnEnable", flags);
            roomBrushField = type.GetField("brush", flags);
            roomPresetField = type.GetField("selectedPresetId", flags);
            roomGeneratedField = type.GetField("lastGeneratedName", flags);
        }

        private void EnsureStyles()
        {
            if (toolbarLabelStyle != null && contentStyle != null) return;

            toolbarLabelStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                normal = { textColor = new Color(0.82f, 0.88f, 0.92f) },
                alignment = TextAnchor.MiddleLeft
            };
            tabLabelStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = new Color(0.76f, 0.80f, 0.84f) },
                alignment = TextAnchor.MiddleCenter
            };
            selectedTabLabelStyle = new GUIStyle(tabLabelStyle)
            {
                normal = { textColor = Color.white }
            };
            shortcutStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                normal = { textColor = new Color(0.74f, 0.78f, 0.82f) },
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(12, 12, 0, 0)
            };
            statusStyle = new GUIStyle(EditorStyles.miniLabel)
            {
                normal = { textColor = new Color(0.82f, 0.88f, 0.90f) },
                alignment = TextAnchor.MiddleLeft,
                padding = new RectOffset(10, 10, 0, 0)
            };
            contentStyle = new GUIStyle
            {
                normal = { background = MakeTexture(ContentColor) },
                padding = new RectOffset(8, 8, 8, 8)
            };
        }

        private void DrawTopToolbar()
        {
            Rect rect = GUILayoutUtility.GetRect(1f, ToolbarHeight, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, ToolbarColor);

            float x = rect.x + 10f;
            float y = rect.y + 10f;
            GUI.Label(new Rect(x, y, 88f, 18f), "Active Tilemap", toolbarLabelStyle);
            x += 94f;

            EditorGUI.BeginChangeCheck();
            activeTilemap = (Tilemap)EditorGUI.ObjectField(new Rect(x, y - 2f, 220f, 20f), activeTilemap, typeof(Tilemap), true);
            if (EditorGUI.EndChangeCheck())
            {
                PushActiveTilemapToTilePainter();
                lastAction = "active tilemap changed";
            }
            x += 232f;

            string scenePath = SceneManager.GetActiveScene().path;
            if (string.IsNullOrEmpty(scenePath)) scenePath = "(unsaved scene)";
            Rect sceneRect = new Rect(x, y, Mathf.Max(80f, rect.xMax - x - 128f), 18f);
            GUI.Label(sceneRect, new GUIContent("Scene: " + scenePath, scenePath), toolbarLabelStyle);

            Rect refreshRect = new Rect(rect.xMax - 118f, y - 3f, 108f, 24f);
            if (GUI.Button(refreshRect, new GUIContent("Refresh assets", "Refresh AssetDatabase and reload tile assets")))
            {
                AssetDatabase.Refresh();
                InvokeNoArg(tileLoadAssets, tilePainter);
                lastAction = "assets refreshed";
                Repaint();
            }
        }

        private void DrawTabBar()
        {
            Rect rect = GUILayoutUtility.GetRect(1f, TabHeight, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, TabBarColor);

            DrawTabButton(new Rect(rect.x + 8f, rect.y + 5f, 132f, 24f), Tab.TilePainter,
                new GUIContent("Tile Painter", "Paint and erase floor tiles on the active tilemap"));
            DrawTabButton(new Rect(rect.x + 144f, rect.y + 5f, 132f, 24f), Tab.RoomBuilder,
                new GUIContent("Room Builder", "Build room footprints, doors, sockets, and wall layouts"));

            Rect futureRect = new Rect(rect.x + 286f, rect.y + 8f, rect.width - 294f, 18f);
            GUI.Label(futureRect, "Future: Props, Reference Layers", toolbarLabelStyle);
        }

        private void DrawTabButton(Rect rect, Tab tab, GUIContent label)
        {
            bool selected = selectedTab == tab;
            EditorGUI.DrawRect(rect, selected ? new Color(0.06f, 0.42f, 0.52f) : InactiveTabColor);
            if (selected)
            {
                EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - 3f, rect.width, 3f), AccentColor);
            }

            if (GUI.Button(rect, label, selected ? selectedTabLabelStyle : tabLabelStyle))
            {
                if (selectedTab != tab)
                {
                    selectedTab = tab;
                    contentScroll = Vector2.zero;
                    lastAction = "tab switched to " + label.text;
                    PushActiveTilemapToTilePainter();
                    GUI.FocusControl(null);
                }
            }
        }

        private void DrawShortcutStrip()
        {
            Rect rect = GUILayoutUtility.GetRect(1f, ShortcutHeight, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, new Color(0.18f, 0.20f, 0.23f));
            string text = selectedTab == Tab.TilePainter
                ? "Shortcuts: P=Paint, E=Erase, 1/2/3=Brush Size"
                : "Shortcuts: W=Walkable, E=Erase, D=Door, A=Alcove, S=Prop Socket, N=Enemy Spawn, O=Objective";
            GUI.Label(rect, text, shortcutStyle);
        }

        private void DrawContentArea()
        {
            using (new EditorGUILayout.VerticalScope(contentStyle, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                contentScroll = EditorGUILayout.BeginScrollView(contentScroll, true, true,
                    GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUIStyle.none,
                    GUILayout.ExpandHeight(true));
                using (new EditorGUILayout.VerticalScope(GUILayout.MinWidth(selectedTab == Tab.TilePainter ? 360f : 900f)))
                {
                    DrawActivePainter();
                }
                EditorGUILayout.EndScrollView();
            }
        }

        private void DrawActivePainter()
        {
            PushActiveTilemapToTilePainter();
            if (selectedTab == Tab.TilePainter)
            {
                InvokePainterGui(tileOnGui, tilePainter, "Tile Painter");
                PullActiveTilemapFromTilePainter();
            }
            else
            {
                InvokePainterGui(roomOnGui, roomPainter, "Room Builder");
            }
        }

        private void DrawUnifiedStatusBar()
        {
            Rect rect = GUILayoutUtility.GetRect(1f, StatusHeight, GUILayout.ExpandWidth(true));
            EditorGUI.DrawRect(rect, StatusColor);

            string tilemapName = activeTilemap != null ? activeTilemap.name : "None";
            string painterStatus = selectedTab == Tab.TilePainter ? GetTilePainterStatus() : GetRoomPainterStatus();
            string status = $"Active Tilemap: {tilemapName} | Last action: {painterStatus}";
            GUI.Label(rect, status, statusStyle);
        }

        private void HandleTilePainterHotkeys()
        {
            if (selectedTab != Tab.TilePainter) return;
            Event current = Event.current;
            if (current == null || current.type != EventType.KeyDown) return;
            if (current.alt || current.control || current.command) return;

            bool handled = true;
            switch (current.keyCode)
            {
                case KeyCode.P:
                    SetTileTool(0, "tool set to Paint");
                    break;
                case KeyCode.E:
                    SetTileTool(1, "tool set to Erase");
                    break;
                case KeyCode.Alpha1:
                case KeyCode.Keypad1:
                    SetTileBrushSize(1);
                    break;
                case KeyCode.Alpha2:
                case KeyCode.Keypad2:
                    SetTileBrushSize(2);
                    break;
                case KeyCode.Alpha3:
                case KeyCode.Keypad3:
                    SetTileBrushSize(3);
                    break;
                default:
                    handled = false;
                    break;
            }

            if (!handled) return;
            current.Use();
            Repaint();
        }

        private void SetTileTool(int toolIndex, string action)
        {
            if (tileToolField == null || tilePainter == null) return;
            tileToolField.SetValue(tilePainter, Enum.ToObject(tileToolField.FieldType, toolIndex));
            lastAction = action;
        }

        private void SetTileBrushSize(int size)
        {
            if (tileBrushSizeField == null || tilePainter == null) return;
            tileBrushSizeField.SetValue(tilePainter, size);
            lastAction = "brush size set to " + size;
        }

        private void PushActiveTilemapToTilePainter()
        {
            if (tilemapField == null || tilePainter == null) return;
            tilemapField.SetValue(tilePainter, activeTilemap);
        }

        private void PullActiveTilemapFromTilePainter()
        {
            if (tilemapField == null || tilePainter == null) return;
            activeTilemap = tilemapField.GetValue(tilePainter) as Tilemap;
        }

        private string GetTilePainterStatus()
        {
            string status = tileStatusField != null && tilePainter != null
                ? tileStatusField.GetValue(tilePainter) as string
                : null;
            return string.IsNullOrEmpty(status) ? lastAction : status;
        }

        private string GetRoomPainterStatus()
        {
            string brush = roomBrushField != null && roomPainter != null
                ? Convert.ToString(roomBrushField.GetValue(roomPainter))
                : "Room Builder";
            string preset = roomPresetField != null && roomPainter != null
                ? roomPresetField.GetValue(roomPainter) as string
                : null;
            string generated = roomGeneratedField != null && roomPainter != null
                ? roomGeneratedField.GetValue(roomPainter) as string
                : null;

            string status = "brush " + brush;
            if (!string.IsNullOrEmpty(preset)) status += " | preset " + preset;
            if (!string.IsNullOrEmpty(generated)) status += " | room " + generated;
            return status;
        }

        private void InvokePainterGui(MethodInfo method, object target, string label)
        {
            if (method == null || target == null)
            {
                EditorGUILayout.HelpBox(label + " UI is unavailable.", MessageType.Error);
                return;
            }

            try
            {
                method.Invoke(target, null);
            }
            catch (TargetInvocationException ex)
            {
                Exception inner = ex.InnerException ?? ex;
                EditorGUILayout.HelpBox(label + " failed: " + inner.Message, MessageType.Error);
                Debug.LogException(inner);
            }
        }

        private static void InvokeNoArg(MethodInfo method, object target)
        {
            if (method == null || target == null) return;
            method.Invoke(target, null);
        }

        private static void DestroyHiddenWindow(EditorWindow window)
        {
            if (window == null) return;
            DestroyImmediate(window);
        }

        private static Tilemap FindFloorTilemap()
        {
            Tilemap[] tilemaps = FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
            string[] priority = { "Floor", "Ground", "L1", "Base", "Tilemap" };
            foreach (string keyword in priority)
            {
                Tilemap match = tilemaps.FirstOrDefault(tilemap =>
                    tilemap != null && tilemap.name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);
                if (match != null) return match;
            }

            return tilemaps.Length > 0 ? tilemaps[0] : null;
        }

        private static Texture2D MakeTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.hideFlags = HideFlags.HideAndDontSave;
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}
#endif
