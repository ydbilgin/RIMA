using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using RIMA.Walls.V2;

namespace RIMA.Walls.V2.EditorTools
{
    /// <summary>
    /// World Painter Editor Window. Grid-based paint UI for room footprints.
    /// </summary>
    public class RoomPainterWindow : EditorWindow
    {
        public enum BrushMode
        {
            Walkable,
            Erase,
            Door,
            Alcove,
            Protrusion,
            Water,
            Island,
            PropSocket,
            EnemySpawn,
            ObjectiveSocket
        }

        private enum CellState : byte
        {
            Empty = 0,
            Walkable = 1,
            Alcove = 2,
            Protrusion = 3
        }

        private const int DefaultGrid = 22;
        private const int MinGrid = 8;
        private const int MaxGrid = 64;
        private const int MinCellPx = 10;
        private const int MaxCellPx = 48;

        private const string RegistryPath = "Assets/ScriptableObjects/Walls/V2/WallPieceRegistry_v1.asset";
        private const string LayoutsFolder = "STAGING/room_layouts";

        private BrushMode brush = BrushMode.Walkable;
        private int gridWidth = DefaultGrid;
        private int gridHeight = DefaultGrid;
        private float cellSizeWorld = 1f;
        private FrontEdgeMode frontEdge = FrontEdgeMode.LowWall;
        private bool rearWallEnabled = true;
        private bool sideWallsEnabled = true;
        private bool enforceCenteredRearDoor = true;
        private Vector3 spawnOrigin = Vector3.zero;
        private int cellPx = 24;
        private CellState[,] cells;
        private Vector2Int? doorCell;
        private int reservedCenterRadius = 0;
        private List<RectInt> waterPools = new List<RectInt>();
        private List<RectInt> interiorIslands = new List<RectInt>();
        private List<RoomSocket> sockets = new List<RoomSocket>();
        private SocketType propSocketType = SocketType.Torch;
        private SocketType enemySocketType = SocketType.EnemyMelee;
        private SocketType objectiveSocketType = SocketType.ObjectiveDoor;
        private Vector2 canvasScroll;
        private Vector2Int hoverCell = new Vector2Int(-1, -1);
        private bool isDragging;
        private bool isBoxSelecting;
        private Vector2Int boxStart = new Vector2Int(-1, -1);
        private string lastSavedPath;
        private string lastGeneratedName;
        private bool replaceExisting = true;
        private string selectedPresetId = "";
        private bool validationFoldout = true;
        private bool validationDirty = true;
        private bool livePreview = true;
        private bool previewDirty = true;
        private double nextValidationAt;
        private List<PainterValidator.Issue> validationIssues = new List<PainterValidator.Issue>();
        private List<PreviewPiece> previewPieces = new List<PreviewPiece>();
        private Vector2Int? jumpHighlightCell;
        private double jumpHighlightUntil;

        private static readonly Color BgColor = new Color(0.12f, 0.13f, 0.16f);
        private static readonly Color GridLineColor = new Color(0f, 0f, 0f, 0.5f);
        private static readonly Color EmptyColor = new Color(0.22f, 0.22f, 0.25f);
        private static readonly Color WalkableColor = new Color(0.25f, 0.55f, 0.95f);
        private static readonly Color AlcoveColor = new Color(0.70f, 0.30f, 0.85f);
        private static readonly Color ProtrusionColor = new Color(0.95f, 0.60f, 0.20f);
        private static readonly Color DoorColor = new Color(0.95f, 0.90f, 0.25f);
        private static readonly Color ReservedColor = new Color(0.95f, 0.30f, 0.30f, 0.35f);
        private static readonly Color WaterColor = new Color(0.30f, 0.75f, 0.95f, 0.55f);
        private static readonly Color IslandColor = new Color(0.45f, 0.30f, 0.20f, 0.55f);
        private static readonly Color PropSocketColor = new Color(1.00f, 0.78f, 0.25f, 0.95f);
        private static readonly Color EnemySocketColor = new Color(1.00f, 0.25f, 0.20f, 0.95f);
        private static readonly Color ObjectiveSocketColor = new Color(0.55f, 0.95f, 0.55f, 0.95f);
        private static readonly Color HoverOutline = new Color(1f, 1f, 1f, 0.95f);

        // [MenuItem removed — replaced by RIMA/Room Painter (Phase A)]
        public static void ShowWindow()
        {
            var w = FindStandaloneWindow();
            if (w == null)
            {
                w = CreateInstance<RoomPainterWindow>();
                w.titleContent = new GUIContent("World Painter");
            }
            w.minSize = new Vector2(880, 600);
            w.Show();
            w.Focus();
        }

        private static RoomPainterWindow FindStandaloneWindow()
        {
            foreach (RoomPainterWindow window in Resources.FindObjectsOfTypeAll<RoomPainterWindow>())
            {
                if (window != null && (window.hideFlags & HideFlags.HideInHierarchy) == 0)
                    return window;
            }

            return null;
        }

        private void OnEnable() { EnsureGrid(); wantsMouseMove = true; }

        private void EnsureGrid()
        {
            if (cells == null || cells.GetLength(0) != gridWidth || cells.GetLength(1) != gridHeight)
            {
                var fresh = new CellState[gridWidth, gridHeight];
                if (cells != null)
                {
                    int copyW = Mathf.Min(cells.GetLength(0), gridWidth);
                    int copyH = Mathf.Min(cells.GetLength(1), gridHeight);
                    for (int x = 0; x < copyW; x++)
                        for (int y = 0; y < copyH; y++)
                            fresh[x, y] = cells[x, y];
                }
                cells = fresh;
                if (doorCell.HasValue)
                {
                    var d = doorCell.Value;
                    if (d.x < 0 || d.y < 0 || d.x >= gridWidth || d.y >= gridHeight)
                        doorCell = null;
                }
            }
        }

        private void OnGUI()
        {
            EnsureGrid();
            HandleHotkeys();
            MaybeRunValidation();
            MaybeRecomputePreview();
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawLeftPanel();
                DrawCanvas();
            }
            DrawStatusBar();
            // Only repaint on actual events to avoid lag
            if (Event.current.type == EventType.MouseMove || Event.current.type == EventType.MouseDrag)
                Repaint();
        }

        private void HandleHotkeys()
        {
            var e = Event.current;
            if (e.type != EventType.KeyDown || e.alt || e.control || e.command) return;

            bool handled = true;
            switch (e.keyCode)
            {
                case KeyCode.W: brush = BrushMode.Walkable; break;
                case KeyCode.E: brush = BrushMode.Erase; break;
                case KeyCode.D: brush = BrushMode.Door; break;
                case KeyCode.A: brush = BrushMode.Alcove; break;
                case KeyCode.P: brush = BrushMode.Protrusion; break;
                case KeyCode.T: brush = BrushMode.Water; break;
                case KeyCode.I: brush = BrushMode.Island; break;
                case KeyCode.S: brush = BrushMode.PropSocket; break;
                case KeyCode.N: brush = BrushMode.EnemySpawn; break;
                case KeyCode.O: brush = BrushMode.ObjectiveSocket; break;
                default: handled = false; break;
            }

            if (!handled) return;
            e.Use();
            Repaint();
        }

        private void MarkLayoutChanged()
        {
            previewDirty = true;
            validationDirty = true;
            nextValidationAt = EditorApplication.timeSinceStartup + 0.25f;
            Repaint();
        }

        private void MaybeRunValidation()
        {
            if (!validationDirty) return;
            if (EditorApplication.timeSinceStartup < nextValidationAt) return;
            RunValidation();
        }

        private void RunValidation()
        {
            var walkable = new HashSet<Vector2Int>(CollectWalkable());
            validationIssues = PainterValidator.Validate(BuildCurrentSpec(walkable), walkable, doorCell,
                waterPools, interiorIslands, sockets);
            validationDirty = false;
        }

        private void MaybeRecomputePreview()
        {
            if (!livePreview || !previewDirty) return;
            RecomputePreview();
        }

        private void RecomputePreview()
        {
            var walkable = new HashSet<Vector2Int>(CollectWalkable());
            previewPieces = WallChainPredictor.PredictPieces(BuildCurrentSpec(walkable), walkable);
            previewDirty = false;
        }

        // ============== LEFT PANEL ==============
        private void DrawLeftPanel()
        {
            using (new EditorGUILayout.VerticalScope(GUILayout.Width(260)))
            {
                EditorGUILayout.LabelField("World Painter", EditorStyles.boldLabel);
                EditorGUILayout.HelpBox(
                    "Cell'lere tıkla → zemin (mavi) boya. Sistem zeminin sınırlarına otomatik DUVAR yerleştirir (kırmızı outline = duvar gelecek yer).\n\nKısayollar:\n• SOL TIK + drag = paint\n• SHIFT + drag = dikdörtgen seçim (alan doldur)\n• CTRL + scroll = zoom",
                    MessageType.Info);

                EditorGUILayout.Space(4);
                DrawPresetLoadRow();

                EditorGUILayout.Space(6);
                EditorGUILayout.LabelField("Brush (boyama tipi)", EditorStyles.boldLabel);
                DrawBrushPicker();
                DrawBrushExplain();

                EditorGUILayout.Space(6);
                EditorGUILayout.LabelField("Grid", EditorStyles.boldLabel);
                int newW = EditorGUILayout.IntSlider(new GUIContent("Width", "Grid hücre sayısı (X)"), gridWidth, MinGrid, MaxGrid);
                int newH = EditorGUILayout.IntSlider(new GUIContent("Height", "Grid hücre sayısı (Y)"), gridHeight, MinGrid, MaxGrid);
                if (newW != gridWidth || newH != gridHeight) { gridWidth = newW; gridHeight = newH; EnsureGrid(); MarkLayoutChanged(); }
                cellPx = EditorGUILayout.IntSlider(new GUIContent("Zoom (px/cell)", "Canvas üzerinde her cell kaç piksel"), cellPx, MinCellPx, MaxCellPx);

                EditorGUILayout.Space(6);
                EditorGUILayout.LabelField("Oda Ayarları", EditorStyles.boldLabel);
                var nextFrontEdge = (FrontEdgeMode)EditorGUILayout.EnumPopup(new GUIContent("Ön Kenar", "Open=açık, LowWall=alçak parapet, Broken=kırık karışık"), frontEdge);
                if (nextFrontEdge != frontEdge) { frontEdge = nextFrontEdge; MarkLayoutChanged(); }
                int doorMode = GUILayout.Toolbar(enforceCenteredRearDoor ? 0 : 1, new[] { "Centered Door", "User-Painted Door" });
                bool nextCenteredDoor = doorMode == 0;
                if (nextCenteredDoor != enforceCenteredRearDoor) { enforceCenteredRearDoor = nextCenteredDoor; MarkLayoutChanged(); }
                rearWallEnabled = EditorGUILayout.Toggle(new GUIContent("Arka Duvar", "Üst (kuzey) duvarı render et"), rearWallEnabled);
                sideWallsEnabled = EditorGUILayout.Toggle(new GUIContent("Yan Duvarlar", "Sol/sağ (batı/doğu) duvarları render et"), sideWallsEnabled);
                cellSizeWorld = EditorGUILayout.Slider(new GUIContent("Cell World Size", "Sahnede her cell kaç birim"), cellSizeWorld, 0.5f, 2.0f);
                spawnOrigin = EditorGUILayout.Vector3Field("Spawn Origin", spawnOrigin);
                replaceExisting = EditorGUILayout.Toggle(new GUIContent("Eskiyi Sil", "Generate edince mevcut PaintedRoom_* objelerini sil"), replaceExisting);

                EditorGUILayout.Space(8);
                EditorGUILayout.LabelField("Eylemler", EditorStyles.boldLabel);
                bool nextLivePreview = EditorGUILayout.Toggle(new GUIContent("Live Preview", "Canvas üstünde tahmini duvar ve collider footprint çiz"), livePreview);
                if (nextLivePreview != livePreview)
                {
                    livePreview = nextLivePreview;
                    previewDirty = true;
                    Repaint();
                }
                if (GUILayout.Button(new GUIContent("✦ Generate Room", "Painted cell'leri WallChainRoomBuilder'a feed et + sahneye duvarları spawn et"), GUILayout.Height(32))) GenerateRoom();
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent("Clear", "Tüm paint'i sıfırla"))) ClearCanvas();
                    if (GUILayout.Button(new GUIContent("Auto-Clean", "Normalize layout and remove obvious invalid cells"))) AutoClean();
                    if (GUILayout.Button(new GUIContent("Frame", "Son üretilen odayı Scene View'da framele"))) FrameInSceneView();
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Save")) SaveLayout();
                    if (GUILayout.Button("Load")) LoadLayout();
                }

                DrawValidationPanel();

                if (!string.IsNullOrEmpty(selectedPresetId))
                {
                    EditorGUILayout.Space(4);
                    EditorGUILayout.LabelField("Aktif preset:", EditorStyles.miniBoldLabel);
                    EditorGUILayout.LabelField(selectedPresetId, EditorStyles.wordWrappedMiniLabel);
                }
                if (!string.IsNullOrEmpty(lastSavedPath))
                {
                    EditorGUILayout.Space(4);
                    EditorGUILayout.LabelField("Last Save:", EditorStyles.miniBoldLabel);
                    EditorGUILayout.SelectableLabel(lastSavedPath, EditorStyles.wordWrappedMiniLabel, GUILayout.Height(32));
                }
            }
        }

        private void DrawPresetLoadRow()
        {
            EditorGUILayout.LabelField("Preset Odalar (tek tıkla yükle)", EditorStyles.boldLabel);
            using (new EditorGUILayout.HorizontalScope())
            {
                if (PresetButton("combat_basic", "Combat", "14x12 basic combat room")) LoadPreset_CombatBasic();
                if (PresetButton("ritual_diamond", "Ritual", "13x13 stepped ritual diamond")) LoadPreset_RitualDiamond();
                if (PresetButton("flooded_crypt", "Flooded", "14x11 crypt with two reserved water pools")) LoadPreset_FloodedCrypt();
                if (PresetButton("library_alcove", "Library", "11x11 library with grouped alcoves")) LoadPreset_LibraryAlcove();
                if (PresetButton("boss_arena", "Boss", "18x14 boss arena with rear setpiece")) LoadPreset_BossArena();
            }
        }

        private bool PresetButton(string id, string label, string tooltip)
        {
            Color old = GUI.backgroundColor;
            if (selectedPresetId == id)
                GUI.backgroundColor = new Color(0.65f, 0.95f, 1f, 1f);
            bool clicked = GUILayout.Button(new GUIContent(label, tooltip), GUILayout.Height(24));
            GUI.backgroundColor = old;
            return clicked;
        }

        private void DrawBrushPicker()
        {
            string[] labels =
            {
                "W Walkable",
                "E Erase",
                "D Door",
                "A Alcove",
                "P Protrusion",
                "T Water",
                "I Island",
                "S Prop Socket",
                "N Enemy Spawn",
                "O Objective"
            };
            int selected = (int)brush;
            int next = GUILayout.SelectionGrid(selected, labels, 2, GUILayout.Height(24 * 5));
            if (next != selected) brush = (BrushMode)next;

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawSwatch(WalkableColor, "Zemin");
                DrawSwatch(EmptyColor, "Boş");
                DrawSwatch(DoorColor, "Kapı");
                DrawSwatch(AlcoveColor, "Niş");
                DrawSwatch(ProtrusionColor, "Bump");
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                DrawSwatch(WaterColor, "Su");
                DrawSwatch(IslandColor, "Ada");
                DrawSwatch(PropSocketColor, "Prop");
                DrawSwatch(EnemySocketColor, "Enemy");
                DrawSwatch(ObjectiveSocketColor, "Obj");
            }

            DrawSocketSubToolbar();
        }

        private void DrawSocketSubToolbar()
        {
            if (brush == BrushMode.PropSocket)
            {
                var next = (SocketType)EditorGUILayout.EnumPopup(new GUIContent("Prop Type", "Painted prop socket subtype"), propSocketType);
                if (IsPropSocket(next)) propSocketType = next;
            }
            else if (brush == BrushMode.EnemySpawn)
            {
                var next = (SocketType)EditorGUILayout.EnumPopup(new GUIContent("Enemy Type", "Painted enemy spawn subtype"), enemySocketType);
                if (IsEnemySocket(next)) enemySocketType = next;
            }
            else if (brush == BrushMode.ObjectiveSocket)
            {
                var next = (SocketType)EditorGUILayout.EnumPopup(new GUIContent("Objective Type", "Painted objective socket subtype"), objectiveSocketType);
                if (IsObjectiveSocket(next)) objectiveSocketType = next;
            }
        }

        private void DrawSwatch(Color c, string label)
        {
            var r = GUILayoutUtility.GetRect(40, 20);
            EditorGUI.DrawRect(r, c);
            var style = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } };
            GUI.Label(r, label, style);
        }

        private void DrawBrushExplain()
        {
            string text = brush switch
            {
                BrushMode.Walkable => "🟦 ZEMİN — odanın yürünebilir alanı. Sistem bu mavi zeminin sınırlarını alıp etrafına otomatik DUVAR koyar. Önce zemini boya.",
                BrushMode.Erase => "⬜ SİL — cell'i boşalt. Boş alandan duvar geçer (oda dışı).",
                BrushMode.Door => "🟨 KAPI — duvarda 1 cell'lik geçit. Tek seferlik tıkla. EnforceDoorCenter true ise sistem otomatik üst-orta'ya çeker.",
                BrushMode.Alcove => "🟪 NİŞ (Alcove) — duvarın İÇİNE doğru girinti. Örnek: kütüphane raf alanı, taht oyuğu, mum nişi. Zeminden çıkarılır, etrafı InnerCorner ile çevrilir.",
                BrushMode.Protrusion => "🟧 ÇIKINTI (Bump-out) — duvardan DIŞARI taşma. Örnek: balkon, küçük balkon-cep, çıkık altar. Zemine eklenir, etrafı OuterCorner ile çevrilir.",
                BrushMode.Water => "SU — dikdörtgen sürükle; alan walkable olmaktan çıkar ve waterPools listesine eklenir.",
                BrushMode.Island => "ISLAND — dikdörtgen sürükle; alan walkable olmaktan çıkar ve interiorIslands listesine eklenir.",
                BrushMode.PropSocket => "PROP SOCKET — tek hücreye prop yerleştirme noktası ekler.",
                BrushMode.EnemySpawn => "ENEMY SPAWN — tek hücreye düşman spawn noktası ekler.",
                BrushMode.ObjectiveSocket => "OBJECTIVE — tek hücreye hedef/çıkış/sandık/ritüel noktası ekler.",
                _ => ""
            };
            EditorGUILayout.HelpBox(text, MessageType.None);
        }

        private void DrawValidationPanel()
        {
            EditorGUILayout.Space(8);
            validationFoldout = EditorGUILayout.Foldout(validationFoldout, $"Validation ({validationIssues.Count})", true);
            if (!validationFoldout) return;

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Validate", GUILayout.Width(80))) RunValidation();
                    if (GUILayout.Button("Auto-Clean", GUILayout.Width(92))) AutoClean();
                    GUILayout.FlexibleSpace();
                }

                if (validationIssues.Count == 0)
                {
                    EditorGUILayout.LabelField("No validation issues.", EditorStyles.miniLabel);
                    return;
                }

                foreach (var issue in validationIssues)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        GUILayout.Label(GetIssueIcon(issue.sev), GUILayout.Width(18));
                        EditorGUILayout.LabelField(issue.code, GUILayout.Width(40));
                        EditorGUILayout.LabelField(issue.message, EditorStyles.wordWrappedMiniLabel);
                        if (issue.cell.HasValue && GUILayout.Button("Jump", GUILayout.Width(48)))
                            JumpToCell(issue.cell.Value);
                    }
                }
            }
        }

        private static string GetIssueIcon(PainterValidator.Severity severity)
        {
            switch (severity)
            {
                case PainterValidator.Severity.Error: return "x";
                case PainterValidator.Severity.Warning: return "!";
                default: return "i";
            }
        }

        // ============== CANVAS ==============
        private void DrawCanvas()
        {
            using (var scroll = new EditorGUILayout.ScrollViewScope(canvasScroll))
            {
                canvasScroll = scroll.scrollPosition;
                int padding = 8;
                // Area = MAX possible canvas (so resize doesn't shift painted cells visually).
                // Grid sits bottom-left anchored inside this fixed area.
                Rect area = GUILayoutUtility.GetRect(
                    MaxGrid * cellPx + padding * 2,
                    MaxGrid * cellPx + padding * 2);

                Rect gridRect = new Rect(
                    area.x + padding,
                    area.yMax - padding - gridHeight * cellPx,
                    gridWidth * cellPx,
                    gridHeight * cellPx);

                // INPUT FIRST — update hoverCell before render so preview is on correct cell
                HandleCanvasInput(gridRect);

                EditorGUI.DrawRect(area, BgColor);

                // Cells
                for (int x = 0; x < gridWidth; x++)
                    for (int y = 0; y < gridHeight; y++)
                    {
                        Rect cellRect = CellRect(gridRect, x, y);
                        EditorGUI.DrawRect(cellRect, GetCellColor(x, y));
                    }

                // Door
                if (doorCell.HasValue)
                {
                    var d = doorCell.Value;
                    if (d.x >= 0 && d.y >= 0 && d.x < gridWidth && d.y < gridHeight)
                    {
                        Rect r = CellRect(gridRect, d.x, d.y);
                        EditorGUI.DrawRect(new Rect(r.x + 2, r.y + 2, r.width - 4, r.height - 4), DoorColor);
                    }
                }

                // Reserved center
                if (reservedCenterRadius > 0)
                {
                    int cx = gridWidth / 2, cy = gridHeight / 2;
                    for (int x = 0; x < gridWidth; x++)
                        for (int y = 0; y < gridHeight; y++)
                            if (Vector2Int.Distance(new Vector2Int(x, y), new Vector2Int(cx, cy)) <= reservedCenterRadius)
                                EditorGUI.DrawRect(CellRect(gridRect, x, y), ReservedColor);
                }

                // Water pools + interior islands
                foreach (var p in waterPools) DrawRectOverlay(gridRect, p, WaterColor);
                foreach (var i in interiorIslands) DrawRectOverlay(gridRect, i, IslandColor);
                DrawSocketOverlay(gridRect);
                DrawJumpHighlight(gridRect);

                // Grid lines
                Handles.color = GridLineColor;
                for (int x = 0; x <= gridWidth; x++)
                {
                    float lx = gridRect.x + x * cellPx;
                    Handles.DrawLine(new Vector3(lx, gridRect.y), new Vector3(lx, gridRect.yMax));
                }
                for (int y = 0; y <= gridHeight; y++)
                {
                    float ly = gridRect.y + y * cellPx;
                    Handles.DrawLine(new Vector3(gridRect.x, ly), new Vector3(gridRect.xMax, ly));
                }

                // WALL PREVIEW
                if (livePreview) DrawLivePreview(gridRect);
                else DrawWallPreview(gridRect);

                // BOX SELECT preview
                if (isBoxSelecting && boxStart.x >= 0 && hoverCell.x >= 0)
                {
                    int x0 = Mathf.Min(boxStart.x, hoverCell.x);
                    int x1 = Mathf.Max(boxStart.x, hoverCell.x);
                    int y0 = Mathf.Min(boxStart.y, hoverCell.y);
                    int y1 = Mathf.Max(boxStart.y, hoverCell.y);
                    Rect r0 = CellRect(gridRect, x0, y1);
                    Rect r1 = CellRect(gridRect, x1, y0);
                    Rect box = new Rect(r0.x, r0.y, r1.xMax - r0.x, r1.yMax - r0.y);
                    EditorGUI.DrawRect(box, new Color(0.4f, 0.8f, 1f, 0.25f));
                    Handles.color = new Color(0.4f, 0.9f, 1f, 1f);
                    Handles.DrawAAPolyLine(2.5f,
                        new Vector3(box.x, box.y), new Vector3(box.xMax, box.y),
                        new Vector3(box.xMax, box.yMax), new Vector3(box.x, box.yMax),
                        new Vector3(box.x, box.y));
                    int count = (x1 - x0 + 1) * (y1 - y0 + 1);
                    GUI.Label(new Rect(box.x + 4, box.y + 4, 100, 18), $"{count} cell",
                        new GUIStyle(EditorStyles.boldLabel) { normal = { textColor = Color.white } });
                }

                // HOVER PREVIEW — brush color preview + bright outline
                if (hoverCell.x >= 0 && hoverCell.y >= 0 && hoverCell.x < gridWidth && hoverCell.y < gridHeight)
                {
                    Rect hr = CellRect(gridRect, hoverCell.x, hoverCell.y);
                    Color previewCol = GetBrushPreviewColor();
                    var inner = new Rect(hr.x + 1, hr.y + 1, hr.width - 2, hr.height - 2);
                    EditorGUI.DrawRect(inner, new Color(previewCol.r, previewCol.g, previewCol.b, 0.6f));
                    // Outline (top/bottom/left/right thick borders)
                    Handles.color = HoverOutline;
                    Handles.DrawAAPolyLine(2.5f,
                        new Vector3(hr.x, hr.y),
                        new Vector3(hr.xMax, hr.y),
                        new Vector3(hr.xMax, hr.yMax),
                        new Vector3(hr.x, hr.yMax),
                        new Vector3(hr.x, hr.y));
                }
            }
        }

        private void DrawWallPreview(Rect gridRect)
        {
            // Walkable cell'in sınırlarında (komşu walkable değilse) "duvar gelecek" işareti çiz
            // Rear (üst) = mavi, Side (yan) = yeşil, Front (alt) = açık mavi
            var rearCol = new Color(0.30f, 0.55f, 0.95f, 1f);
            var sideCol = new Color(0.40f, 0.85f, 0.55f, 1f);
            var frontCol = new Color(0.55f, 0.80f, 0.95f, 1f);
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    var s = cells[x, y];
                    if (s != CellState.Walkable && s != CellState.Protrusion) continue;
                    Rect r = CellRect(gridRect, x, y);
                    // Üst kenar (Rear wall edge)
                    if (!IsFootprint(x, y + 1))
                        EditorGUI.DrawRect(new Rect(r.x, r.y, r.width, 3), rearCol);
                    // Alt kenar (Front edge)
                    if (!IsFootprint(x, y - 1))
                        EditorGUI.DrawRect(new Rect(r.x, r.yMax - 3, r.width, 3), frontCol);
                    // Sol kenar (SideLeft)
                    if (!IsFootprint(x - 1, y))
                        EditorGUI.DrawRect(new Rect(r.x, r.y, 3, r.height), sideCol);
                    // Sağ kenar (SideRight)
                    if (!IsFootprint(x + 1, y))
                        EditorGUI.DrawRect(new Rect(r.xMax - 3, r.y, 3, r.height), sideCol);
                }
            }
        }

        private void DrawLivePreview(Rect gridRect)
        {
            if (previewDirty) RecomputePreview();

            foreach (var piece in previewPieces)
            {
                Rect r = PreviewRect(gridRect, piece);
                DrawRectOutline(r, GetPreviewColor(piece.type), 2f);

                if (piece.colliderSize.x > 0f && piece.colliderSize.y > 0f)
                {
                    Rect collider = PreviewColliderRect(gridRect, piece);
                    DrawRectOutline(collider, new Color(0.25f, 1f, 0.25f, 0.95f), 1.5f);
                }
            }
        }

        private Rect PreviewRect(Rect gridRect, PreviewPiece piece)
        {
            Vector2 size = PreviewPieceSize(piece);
            return GridRectFromWorld(gridRect, piece.cell, size);
        }

        private Rect PreviewColliderRect(Rect gridRect, PreviewPiece piece)
        {
            Vector2 pieceSize = PreviewPieceSize(piece);
            Vector2 colliderSize = new Vector2(
                Mathf.Max(0.1f, piece.colliderSize.x),
                Mathf.Max(0.1f, piece.colliderSize.y));
            Vector2 origin = new Vector2(
                piece.cell.x + (pieceSize.x - colliderSize.x) * 0.5f,
                piece.cell.y + (pieceSize.y - colliderSize.y) * 0.5f);
            return GridRectFromWorld(gridRect, new Vector2Int(Mathf.RoundToInt(origin.x), Mathf.RoundToInt(origin.y)), colliderSize);
        }

        private Rect GridRectFromWorld(Rect gridRect, Vector2Int cell, Vector2 size)
        {
            float x = gridRect.x + cell.x * cellPx;
            float y = gridRect.y + (gridHeight - cell.y - size.y) * cellPx;
            return new Rect(x, y, size.x * cellPx, size.y * cellPx);
        }

        private Vector2 PreviewPieceSize(PreviewPiece piece)
        {
            int len = Mathf.Max(1, piece.lengthInCells);
            if (piece.dir == WallDirection.SideLeft || piece.dir == WallDirection.SideRight)
                return new Vector2(1, len);
            return new Vector2(len, 1);
        }

        private static Color GetPreviewColor(WallPieceType type)
        {
            switch (type)
            {
                case WallPieceType.DoorArch:
                    return new Color(0.72f, 0.30f, 1f, 1f);
                case WallPieceType.LowFront:
                case WallPieceType.OpenGap:
                    return Color.cyan;
                case WallPieceType.Connector:
                case WallPieceType.OuterCorner:
                case WallPieceType.InnerCorner:
                    return new Color(1f, 0.55f, 0.05f, 1f);
                case WallPieceType.RearWall:
                case WallPieceType.SideWall:
                    return Color.yellow;
                default:
                    return Color.white;
            }
        }

        private static void DrawRectOutline(Rect r, Color color, float width)
        {
            Handles.color = color;
            Handles.DrawAAPolyLine(width,
                new Vector3(r.x, r.y),
                new Vector3(r.xMax, r.y),
                new Vector3(r.xMax, r.yMax),
                new Vector3(r.x, r.yMax),
                new Vector3(r.x, r.y));
        }

        private bool IsFootprint(int x, int y)
        {
            if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight) return false;
            var s = cells[x, y];
            return s == CellState.Walkable || s == CellState.Protrusion;
        }

        private void DrawRectOverlay(Rect gridRect, RectInt r, Color c)
        {
            for (int x = r.xMin; x < r.xMax; x++)
                for (int y = r.yMin; y < r.yMax; y++)
                {
                    if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight) continue;
                    EditorGUI.DrawRect(CellRect(gridRect, x, y), c);
                }
        }

        private void DrawSocketOverlay(Rect gridRect)
        {
            var style = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Mathf.Clamp(cellPx / 2, 8, 13),
                normal = { textColor = Color.black }
            };

            foreach (var socket in sockets)
            {
                var c = socket.cell;
                if (c.x < 0 || c.y < 0 || c.x >= gridWidth || c.y >= gridHeight) continue;

                Rect cell = CellRect(gridRect, c.x, c.y);
                float size = Mathf.Max(10f, cellPx * 0.58f);
                Rect badge = new Rect(cell.center.x - size * 0.5f, cell.center.y - size * 0.5f, size, size);
                EditorGUI.DrawRect(badge, GetSocketColor(socket.type));
                Handles.color = Color.black;
                Handles.DrawAAPolyLine(1.5f,
                    new Vector3(badge.x, badge.y),
                    new Vector3(badge.xMax, badge.y),
                    new Vector3(badge.xMax, badge.yMax),
                    new Vector3(badge.x, badge.yMax),
                    new Vector3(badge.x, badge.y));
                GUI.Label(badge, GetSocketLabel(socket.type), style);
            }
        }

        private void DrawJumpHighlight(Rect gridRect)
        {
            if (!jumpHighlightCell.HasValue || EditorApplication.timeSinceStartup > jumpHighlightUntil) return;
            var c = jumpHighlightCell.Value;
            if (c.x < 0 || c.y < 0 || c.x >= gridWidth || c.y >= gridHeight) return;

            Rect r = CellRect(gridRect, c.x, c.y);
            Handles.color = new Color(1f, 0.95f, 0.2f, 1f);
            Handles.DrawAAPolyLine(4f,
                new Vector3(r.x, r.y),
                new Vector3(r.xMax, r.y),
                new Vector3(r.xMax, r.yMax),
                new Vector3(r.x, r.yMax),
                new Vector3(r.x, r.y));
        }

        private void JumpToCell(Vector2Int cell)
        {
            jumpHighlightCell = cell;
            jumpHighlightUntil = EditorApplication.timeSinceStartup + 1.25f;
            canvasScroll = new Vector2(
                Mathf.Max(0, cell.x * cellPx - 120),
                Mathf.Max(0, (gridHeight - 1 - cell.y) * cellPx - 120));
            Repaint();
        }

        private Rect CellRect(Rect gridRect, int x, int y)
        {
            float px = gridRect.x + x * cellPx;
            float py = gridRect.y + (gridHeight - 1 - y) * cellPx;
            return new Rect(px, py, cellPx, cellPx);
        }

        private Color GetCellColor(int x, int y)
        {
            switch (cells[x, y])
            {
                case CellState.Walkable: return WalkableColor;
                case CellState.Alcove: return AlcoveColor;
                case CellState.Protrusion: return ProtrusionColor;
                default: return EmptyColor;
            }
        }

        private Color GetBrushPreviewColor()
        {
            return brush switch
            {
                BrushMode.Walkable => WalkableColor,
                BrushMode.Erase => EmptyColor,
                BrushMode.Door => DoorColor,
                BrushMode.Alcove => AlcoveColor,
                BrushMode.Protrusion => ProtrusionColor,
                BrushMode.Water => WaterColor,
                BrushMode.Island => IslandColor,
                BrushMode.PropSocket => PropSocketColor,
                BrushMode.EnemySpawn => EnemySocketColor,
                BrushMode.ObjectiveSocket => ObjectiveSocketColor,
                _ => Color.gray
            };
        }

        private void HandleCanvasInput(Rect gridRect)
        {
            Event e = Event.current;
            if (!gridRect.Contains(e.mousePosition))
            {
                if (hoverCell.x >= 0) { hoverCell = new Vector2Int(-1, -1); Repaint(); }
                if (e.type == EventType.MouseUp) isDragging = false;
                return;
            }

            Vector2 local = e.mousePosition - new Vector2(gridRect.x, gridRect.y);
            int cx = Mathf.Clamp((int)(local.x / cellPx), 0, gridWidth - 1);
            int cyDisplay = Mathf.Clamp((int)(local.y / cellPx), 0, gridHeight - 1);
            int cy = gridHeight - 1 - cyDisplay;
            var newHover = new Vector2Int(cx, cy);
            if (newHover != hoverCell) { hoverCell = newHover; Repaint(); }

            if (e.type == EventType.MouseDown && e.button == 0)
            {
                if (e.shift || IsRectBrush())
                {
                    isBoxSelecting = true;
                    boxStart = new Vector2Int(cx, cy);
                }
                else
                {
                    isDragging = true;
                    PaintAt(cx, cy);
                }
                e.Use();
            }
            else if (e.type == EventType.MouseDrag && e.button == 0)
            {
                if (isBoxSelecting) { Repaint(); }
                else if (isDragging && AllowsDragPaint()) PaintAt(cx, cy);
                e.Use();
            }
            else if (e.type == EventType.MouseUp && e.button == 0)
            {
                if (isBoxSelecting && boxStart.x >= 0)
                {
                    int x0 = Mathf.Min(boxStart.x, cx);
                    int x1 = Mathf.Max(boxStart.x, cx);
                    int y0 = Mathf.Min(boxStart.y, cy);
                    int y1 = Mathf.Max(boxStart.y, cy);
                    if (IsRectBrush())
                    {
                        AddBrushRect(new RectInt(x0, y0, x1 - x0 + 1, y1 - y0 + 1));
                    }
                    else if (IsSocketBrush())
                    {
                        PaintAt(cx, cy);
                    }
                    else
                    {
                        for (int px = x0; px <= x1; px++)
                            for (int py = y0; py <= y1; py++)
                                PaintAt(px, py);
                    }
                }
                isDragging = false;
                isBoxSelecting = false;
                boxStart = new Vector2Int(-1, -1);
                e.Use();
            }
            else if (e.type == EventType.ScrollWheel && (e.control || e.command))
            {
                cellPx = Mathf.Clamp(cellPx + (e.delta.y < 0 ? 2 : -2), MinCellPx, MaxCellPx);
                Repaint();
                e.Use();
            }
        }

        private bool IsRectBrush()
        {
            return brush == BrushMode.Water || brush == BrushMode.Island;
        }

        private bool IsSocketBrush()
        {
            return brush == BrushMode.PropSocket || brush == BrushMode.EnemySpawn || brush == BrushMode.ObjectiveSocket;
        }

        private bool AllowsDragPaint()
        {
            return brush != BrushMode.Door && !IsSocketBrush() && !IsRectBrush();
        }

        private void PaintAt(int x, int y)
        {
            if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight) return;
            var cell = new Vector2Int(x, y);
            switch (brush)
            {
                case BrushMode.Walkable: cells[x, y] = CellState.Walkable; break;
                case BrushMode.Erase:
                    cells[x, y] = CellState.Empty;
                    if (doorCell.HasValue && doorCell.Value == cell) doorCell = null;
                    RemoveSocketsAt(cell);
                    break;
                case BrushMode.Door:
                    doorCell = cell;
                    if (cells[x, y] == CellState.Empty) cells[x, y] = CellState.Walkable;
                    break;
                case BrushMode.Alcove: cells[x, y] = CellState.Alcove; break;
                case BrushMode.Protrusion: cells[x, y] = CellState.Protrusion; break;
                case BrushMode.PropSocket:
                    AddSocket(cell, propSocketType);
                    break;
                case BrushMode.EnemySpawn:
                    AddSocket(cell, enemySocketType);
                    break;
                case BrushMode.ObjectiveSocket:
                    AddSocket(cell, objectiveSocketType);
                    break;
            }
            MarkLayoutChanged();
        }

        private void AddBrushRect(RectInt rect)
        {
            rect = ClampRectToGrid(rect);
            if (rect.width <= 0 || rect.height <= 0) return;

            var target = brush == BrushMode.Water ? waterPools : interiorIslands;
            target.Add(rect);
            for (int x = rect.xMin; x < rect.xMax; x++)
                for (int y = rect.yMin; y < rect.yMax; y++)
                {
                    cells[x, y] = CellState.Empty;
                    RemoveSocketsAt(new Vector2Int(x, y));
                    if (doorCell.HasValue && doorCell.Value == new Vector2Int(x, y))
                        doorCell = null;
                }

            MarkLayoutChanged();
        }

        private RectInt ClampRectToGrid(RectInt rect)
        {
            int xMin = Mathf.Clamp(rect.xMin, 0, gridWidth);
            int yMin = Mathf.Clamp(rect.yMin, 0, gridHeight);
            int xMax = Mathf.Clamp(rect.xMax, 0, gridWidth);
            int yMax = Mathf.Clamp(rect.yMax, 0, gridHeight);
            return new RectInt(xMin, yMin, Mathf.Max(0, xMax - xMin), Mathf.Max(0, yMax - yMin));
        }

        private void AddSocket(Vector2Int cell, SocketType type, string metadata = "")
        {
            if (cell.x < 0 || cell.y < 0 || cell.x >= gridWidth || cell.y >= gridHeight) return;
            RemoveSocketsAt(cell);
            sockets.Add(new RoomSocket { cell = cell, type = type, metadata = metadata ?? "" });
        }

        private void RemoveSocketsAt(Vector2Int cell)
        {
            sockets.RemoveAll(s => s.cell == cell);
        }

        private static bool IsPropSocket(SocketType type)
        {
            return type == SocketType.Torch
                || type == SocketType.Banner
                || type == SocketType.Bookshelf
                || type == SocketType.Sarcophagus
                || type == SocketType.Altar
                || type == SocketType.Crystal
                || type == SocketType.Cage;
        }

        private static bool IsEnemySocket(SocketType type)
        {
            return type == SocketType.EnemyMelee
                || type == SocketType.EnemyRanged
                || type == SocketType.EnemyElite
                || type == SocketType.EnemyBoss
                || type == SocketType.EnemyWave;
        }

        private static bool IsObjectiveSocket(SocketType type)
        {
            return type == SocketType.ObjectiveDoor
                || type == SocketType.ObjectiveExit
                || type == SocketType.ObjectiveChest
                || type == SocketType.ObjectiveTrigger
                || type == SocketType.ObjectiveRitual
                || type == SocketType.ObjectivePortal;
        }

        private static Color GetSocketColor(SocketType type)
        {
            if (IsEnemySocket(type)) return EnemySocketColor;
            if (IsObjectiveSocket(type)) return ObjectiveSocketColor;
            return PropSocketColor;
        }

        private static string GetSocketLabel(SocketType type)
        {
            switch (type)
            {
                case SocketType.Torch: return "T";
                case SocketType.Banner: return "B";
                case SocketType.Bookshelf: return "Bk";
                case SocketType.Sarcophagus: return "S";
                case SocketType.Altar: return "A";
                case SocketType.Crystal: return "C";
                case SocketType.Cage: return "Cg";
                case SocketType.EnemyMelee: return "M";
                case SocketType.EnemyRanged: return "R";
                case SocketType.EnemyElite: return "E";
                case SocketType.EnemyBoss: return "Boss";
                case SocketType.EnemyWave: return "Wv";
                case SocketType.ObjectiveDoor: return "D";
                case SocketType.ObjectiveExit: return "Ex";
                case SocketType.ObjectiveChest: return "Ch";
                case SocketType.ObjectiveTrigger: return "Tr";
                case SocketType.ObjectiveRitual: return "Rt";
                case SocketType.ObjectivePortal: return "Po";
                default: return "?";
            }
        }

        // ============== STATUS BAR ==============
        private void DrawStatusBar()
        {
            int walk = 0, alc = 0, prt = 0;
            for (int x = 0; x < gridWidth; x++)
                for (int y = 0; y < gridHeight; y++)
                {
                    var s = cells[x, y];
                    if (s == CellState.Walkable) walk++;
                    else if (s == CellState.Alcove) alc++;
                    else if (s == CellState.Protrusion) prt++;
                }

            string hover = (hoverCell.x >= 0 && hoverCell.y >= 0) ? $"({hoverCell.x},{hoverCell.y})" : "—";
            string door = doorCell.HasValue ? $"({doorCell.Value.x},{doorCell.Value.y})" : "yok";
            string status = $"Brush: {brush}  |  Hover: {hover}  |  Zemin: {walk}  |  Kapı: {door}  |  Niş: {alc}  |  Çıkıntı: {prt}  |  Sockets: {sockets.Count}";
            if (!string.IsNullOrEmpty(lastGeneratedName)) status += $"  |  Son oda: {lastGeneratedName}";
            EditorGUILayout.LabelField(status, EditorStyles.miniLabel);
        }

        // ============== PRESETS ==============
        private void LoadPreset_CombatBasic()
        {
            ClearCanvas();
            gridWidth = 14; gridHeight = 12;
            EnsureGrid();
            FillRect(0, 0, 14, 12, CellState.Walkable);
            doorCell = new Vector2Int(7, 11);
            frontEdge = FrontEdgeMode.LowWall;
            enforceCenteredRearDoor = true;
            reservedCenterRadius = 0;
            AddSocket(new Vector2Int(2, 2), SocketType.EnemyMelee);
            AddSocket(new Vector2Int(11, 2), SocketType.EnemyRanged);
            AddSocket(new Vector2Int(2, 9), SocketType.EnemyMelee);
            AddSocket(new Vector2Int(11, 9), SocketType.EnemyRanged);
            AddSocket(new Vector2Int(5, 10), SocketType.Banner);
            AddSocket(new Vector2Int(9, 10), SocketType.Torch);
            selectedPresetId = "combat_basic";
            MarkLayoutChanged();
        }

        private void LoadPreset_RitualDiamond()
        {
            ClearCanvas();
            gridWidth = 13; gridHeight = 13;
            EnsureGrid();
            int[] widths = { 7, 9, 11, 13, 13, 13, 13, 13, 13, 13, 11, 9, 7 };
            int cx = gridWidth / 2;
            for (int y = 0; y < gridHeight; y++)
            {
                int width = widths[y];
                int x0 = Mathf.Max(0, cx - width / 2);
                int x1 = Mathf.Min(gridWidth - 1, x0 + width - 1);
                for (int x = x0; x <= x1; x++) cells[x, y] = CellState.Walkable;
            }
            doorCell = new Vector2Int(6, 12);
            frontEdge = FrontEdgeMode.LowWall;
            enforceCenteredRearDoor = true;
            reservedCenterRadius = 0;
            AddSocket(new Vector2Int(6, 6), SocketType.ObjectiveRitual);
            AddSocket(new Vector2Int(3, 3), SocketType.Crystal, "rift");
            AddSocket(new Vector2Int(9, 3), SocketType.Crystal, "rift");
            AddSocket(new Vector2Int(3, 9), SocketType.Crystal, "rift");
            AddSocket(new Vector2Int(9, 9), SocketType.Crystal, "rift");
            AddSocket(new Vector2Int(2, 6), SocketType.EnemyMelee);
            AddSocket(new Vector2Int(10, 6), SocketType.EnemyMelee);
            AddSocket(new Vector2Int(6, 3), SocketType.EnemyRanged);
            AddSocket(new Vector2Int(6, 9), SocketType.EnemyRanged);
            selectedPresetId = "ritual_diamond";
            MarkLayoutChanged();
        }

        private void LoadPreset_FloodedCrypt()
        {
            ClearCanvas();
            gridWidth = 14; gridHeight = 11;
            EnsureGrid();
            FillRect(0, 0, 14, 11, CellState.Walkable);
            doorCell = new Vector2Int(7, 10);
            frontEdge = FrontEdgeMode.LowWall;
            enforceCenteredRearDoor = true;
            reservedCenterRadius = 0;
            AddPresetRect(waterPools, new RectInt(2, 4, 3, 3));
            AddPresetRect(waterPools, new RectInt(9, 4, 3, 3));
            AddSocket(new Vector2Int(4, 9), SocketType.Sarcophagus);
            AddSocket(new Vector2Int(10, 9), SocketType.Sarcophagus);
            AddSocket(new Vector2Int(7, 5), SocketType.Altar);
            selectedPresetId = "flooded_crypt";
            MarkLayoutChanged();
        }

        private void LoadPreset_LibraryAlcove()
        {
            ClearCanvas();
            gridWidth = 11; gridHeight = 11;
            EnsureGrid();
            FillRect(0, 0, 11, 11, CellState.Walkable);
            FillRect(0, 2, 2, 2, CellState.Alcove);
            FillRect(0, 7, 2, 2, CellState.Alcove);
            FillRect(9, 5, 2, 2, CellState.Alcove);
            doorCell = new Vector2Int(5, 10);
            frontEdge = FrontEdgeMode.LowWall;
            enforceCenteredRearDoor = true;
            reservedCenterRadius = 0;
            AddSocket(new Vector2Int(4, 9), SocketType.Bookshelf);
            AddSocket(new Vector2Int(6, 9), SocketType.Bookshelf);
            AddSocket(new Vector2Int(2, 3), SocketType.Bookshelf);
            AddSocket(new Vector2Int(2, 8), SocketType.Bookshelf);
            AddSocket(new Vector2Int(8, 6), SocketType.Bookshelf);
            AddSocket(new Vector2Int(5, 1), SocketType.ObjectiveChest, "desk");
            selectedPresetId = "library_alcove";
            MarkLayoutChanged();
        }

        private void LoadPreset_BossArena()
        {
            ClearCanvas();
            gridWidth = 18; gridHeight = 16;
            EnsureGrid();
            for (int y = 0; y < 14; y++)
            {
                int inset = y < 2 ? 3 : y < 4 ? 2 : y < 6 ? 1 : 0;
                for (int x = inset; x < gridWidth - inset; x++)
                    cells[x, y] = CellState.Walkable;
            }
            FillRect(6, 14, 6, 2, CellState.Walkable);
            doorCell = new Vector2Int(9, 0);
            frontEdge = FrontEdgeMode.Open;
            enforceCenteredRearDoor = false;
            reservedCenterRadius = 0;
            AddSocket(new Vector2Int(9, 7), SocketType.EnemyBoss);
            AddSocket(new Vector2Int(2, 7), SocketType.EnemyWave);
            AddSocket(new Vector2Int(15, 7), SocketType.EnemyWave);
            AddSocket(new Vector2Int(5, 4), SocketType.EnemyWave);
            AddSocket(new Vector2Int(12, 10), SocketType.EnemyWave);
            AddSocket(new Vector2Int(9, 15), SocketType.ObjectiveDoor, "boss_gate");
            selectedPresetId = "boss_arena";
            MarkLayoutChanged();
        }

        private void AddPresetRect(List<RectInt> target, RectInt rect)
        {
            rect = ClampRectToGrid(rect);
            if (rect.width <= 0 || rect.height <= 0) return;
            target.Add(rect);
            for (int x = rect.xMin; x < rect.xMax; x++)
                for (int y = rect.yMin; y < rect.yMax; y++)
                    cells[x, y] = CellState.Empty;
        }

        private void FillRect(int x0, int y0, int w, int h, CellState s)
        {
            for (int x = x0; x < x0 + w && x < gridWidth; x++)
                for (int y = y0; y < y0 + h && y < gridHeight; y++)
                    cells[x, y] = s;
        }

        private void SetCells(Vector2Int[] arr, CellState s)
        {
            foreach (var p in arr)
                if (p.x >= 0 && p.y >= 0 && p.x < gridWidth && p.y < gridHeight)
                    cells[p.x, p.y] = s;
        }

        // ============== GENERATE ==============
        private void GenerateRoom()
        {
            var registry = AssetDatabase.LoadAssetAtPath<WallPieceRegistry>(RegistryPath);
            if (registry == null)
            {
                EditorUtility.DisplayDialog("World Painter", "Registry asset bulunamadı:\n" + RegistryPath, "OK");
                return;
            }

            var walkable = CollectWalkable();
            if (walkable.Count == 0)
            {
                EditorUtility.DisplayDialog("World Painter", "En az 1 walkable cell çiz, sonra Generate Room.", "OK");
                return;
            }

            var spec = BuildCurrentSpec(new HashSet<Vector2Int>(walkable));

            if (replaceExisting)
            {
                var stragglers = GameObject.FindObjectsByType<WallChainRoomBuilder>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var b in stragglers)
                {
                    if (b == null || b.gameObject == null) continue;
                    if (b.gameObject.name.StartsWith("PaintedRoom_"))
                        Undo.DestroyObjectImmediate(b.gameObject);
                }
            }

            string roomGoName = "PaintedRoom_" + spec.roomName;
            var go = new GameObject(roomGoName);
            go.transform.position = spawnOrigin;
            Undo.RegisterCreatedObjectUndo(go, "Generate Painted Room");

            var pieces = new GameObject("Pieces");
            pieces.transform.SetParent(go.transform, false);

            var builder = go.AddComponent<WallChainRoomBuilder>();
            builder.registry = registry;
            builder.roomParent = pieces.transform;
            builder.cellSize = cellSizeWorld;
            builder.Build(spec);
            var debugGizmo = go.AddComponent<RoomDebugGizmo>();
            debugGizmo.Capture(spec, walkable, cellSizeWorld);

            Selection.activeGameObject = go;
            lastGeneratedName = roomGoName;
            FrameInSceneViewObj(go);
            Debug.Log($"[World Painter] Generated {roomGoName} | walkable={walkable.Count} alcove={spec.alcovePositions.Count} prot={spec.protrusionPositions.Count} door={(spec.HasDoor ? spec.doorPosition.ToString() : "none")} preset={selectedPresetId}");
        }

        private RoomSpec BuildCurrentSpec(HashSet<Vector2Int> walkableSet = null)
        {
            var walkable = walkableSet != null ? new List<Vector2Int>(walkableSet) : CollectWalkable();
            var alcoves = CollectByState(CellState.Alcove);
            var protrusions = CollectByState(CellState.Protrusion);

            int maxX = 0;
            int maxY = 0;
            Action<Vector2Int> include = c =>
            {
                if (c.x >= 0) maxX = Mathf.Max(maxX, c.x);
                if (c.y >= 0) maxY = Mathf.Max(maxY, c.y);
            };
            foreach (var c in walkable) include(c);
            foreach (var c in alcoves) include(c);
            foreach (var c in protrusions) include(c);
            if (doorCell.HasValue) include(doorCell.Value);
            foreach (var s in sockets) include(s.cell);
            foreach (var r in waterPools) include(new Vector2Int(r.xMax - 1, r.yMax - 1));
            foreach (var r in interiorIslands) include(new Vector2Int(r.xMax - 1, r.yMax - 1));

            return new RoomSpec
            {
                roomName = (string.IsNullOrEmpty(selectedPresetId) ? "Painted" : selectedPresetId) + "_" + DateTime.Now.ToString("HHmmss"),
                widthCells = Mathf.Max(1, maxX + 1),
                heightCells = Mathf.Max(1, maxY + 1),
                shapeType = RoomShapeType.Irregular,
                rearWallEnabled = rearWallEnabled,
                sideWallsEnabled = sideWallsEnabled,
                frontEdgeMode = frontEdge,
                doorPosition = doorCell ?? new Vector2Int(-1, -1),
                alcovePositions = alcoves,
                protrusionPositions = protrusions,
                nicheSpecs = BuildNicheSpecs(alcoves),
                protrusionSpecs = BuildProtrusionSpecs(protrusions),
                walkableCells = walkable,
                reservedCenterRadiusCells = reservedCenterRadius,
                waterPoolRects = new List<RectInt>(waterPools),
                interiorIslandRects = new List<RectInt>(interiorIslands),
                sockets = new List<RoomSocket>(sockets),
                enforceCenteredRearDoor = enforceCenteredRearDoor,
                frontMinOpeningCells = 3
            };
        }

        private List<NicheSpec> BuildNicheSpecs(List<Vector2Int> alcoves)
        {
            var specs = new List<NicheSpec>();
            foreach (var group in ConnectedCellGroups(alcoves))
            {
                GetGroupBounds(group, out int minX, out int minY, out int maxX, out int maxY);
                bool left = maxX < gridWidth / 2;
                specs.Add(new NicheSpec
                {
                    side = left ? "left" : "right",
                    anchorRow = minY,
                    width = Mathf.Max(1, maxY - minY + 1),
                    depth = Mathf.Max(1, maxX - minX + 1),
                    mirror = false
                });
            }
            return specs;
        }

        private List<ProtrusionSpec> BuildProtrusionSpecs(List<Vector2Int> protrusions)
        {
            var specs = new List<ProtrusionSpec>();
            foreach (var prot in protrusions)
            {
                specs.Add(new ProtrusionSpec
                {
                    side = prot.x < gridWidth / 2 ? "left" : "right",
                    anchorRow = prot.y,
                    width = 1,
                    depth = 1,
                    mirror = false
                });
            }
            return specs;
        }

        private static List<List<Vector2Int>> ConnectedCellGroups(List<Vector2Int> cellsToGroup)
        {
            var result = new List<List<Vector2Int>>();
            var remaining = new HashSet<Vector2Int>(cellsToGroup);
            while (remaining.Count > 0)
            {
                Vector2Int start = default;
                foreach (var cell in remaining) { start = cell; break; }

                var group = new List<Vector2Int>();
                var queue = new Queue<Vector2Int>();
                queue.Enqueue(start);
                remaining.Remove(start);
                while (queue.Count > 0)
                {
                    var c = queue.Dequeue();
                    group.Add(c);
                    var neighbors = new[]
                    {
                        new Vector2Int(c.x + 1, c.y),
                        new Vector2Int(c.x - 1, c.y),
                        new Vector2Int(c.x, c.y + 1),
                        new Vector2Int(c.x, c.y - 1)
                    };
                    foreach (var n in neighbors)
                    {
                        if (!remaining.Remove(n)) continue;
                        queue.Enqueue(n);
                    }
                }
                result.Add(group);
            }
            return result;
        }

        private static void GetGroupBounds(List<Vector2Int> group, out int minX, out int minY, out int maxX, out int maxY)
        {
            minX = int.MaxValue;
            minY = int.MaxValue;
            maxX = int.MinValue;
            maxY = int.MinValue;
            foreach (var c in group)
            {
                minX = Mathf.Min(minX, c.x);
                minY = Mathf.Min(minY, c.y);
                maxX = Mathf.Max(maxX, c.x);
                maxY = Mathf.Max(maxY, c.y);
            }
        }

        private List<Vector2Int> CollectWalkable()
        {
            var list = new List<Vector2Int>();
            for (int x = 0; x < gridWidth; x++)
                for (int y = 0; y < gridHeight; y++)
                {
                    var s = cells[x, y];
                    if (s == CellState.Walkable || s == CellState.Protrusion)
                        list.Add(new Vector2Int(x, y));
                }
            return list;
        }

        private List<Vector2Int> CollectByState(CellState state)
        {
            var list = new List<Vector2Int>();
            for (int x = 0; x < gridWidth; x++)
                for (int y = 0; y < gridHeight; y++)
                    if (cells[x, y] == state) list.Add(new Vector2Int(x, y));
            return list;
        }

        private void ClearCanvas()
        {
            cells = new CellState[gridWidth, gridHeight];
            doorCell = null;
            reservedCenterRadius = 0;
            waterPools.Clear();
            interiorIslands.Clear();
            sockets.Clear();
            selectedPresetId = "";
            MarkLayoutChanged();
        }

        private void AutoClean()
        {
            int before = validationIssues.Count;
            try
            {
                EditorUtility.DisplayProgressBar("World Painter", "Normalizing footprint origin", 0.2f);
                NormalizeOrigin();

                EditorUtility.DisplayProgressBar("World Painter", "Removing orphan cells", 0.5f);
                int removed = RemoveOrphanCells();

                EditorUtility.DisplayProgressBar("World Painter", "Checking front opening", 0.75f);
                if ((frontEdge == FrontEdgeMode.Open || frontEdge == FrontEdgeMode.LowWall)
                    && !HasAnyFrontEdge(new HashSet<Vector2Int>(CollectWalkable())))
                    Debug.LogWarning("[World Painter] Auto-Clean: front edge mode expects an opening, but no front edge cells are paintable.");

                // Assertion: Auto-Clean handles only deterministic fixes and must not increase autofixable layout errors.
                MarkLayoutChanged();
                RunValidation();
                Debug.Log($"[World Painter] Auto-Clean complete | removedOrphans={removed} issuesBefore={before} issuesAfter={validationIssues.Count}");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private void NormalizeOrigin()
        {
            if (!TryGetLayoutMin(out int minX, out int minY)) return;
            if (minX == 0 && minY == 0) return;

            int dx = -minX;
            int dy = -minY;
            var shifted = new CellState[gridWidth, gridHeight];
            for (int x = 0; x < gridWidth; x++)
                for (int y = 0; y < gridHeight; y++)
                {
                    if (cells[x, y] == CellState.Empty) continue;
                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && ny >= 0 && nx < gridWidth && ny < gridHeight)
                        shifted[nx, ny] = cells[x, y];
                }
            cells = shifted;

            if (doorCell.HasValue) doorCell = new Vector2Int(doorCell.Value.x + dx, doorCell.Value.y + dy);
            ShiftRects(waterPools, dx, dy);
            ShiftRects(interiorIslands, dx, dy);
            for (int i = 0; i < sockets.Count; i++)
            {
                var s = sockets[i];
                s.cell = new Vector2Int(s.cell.x + dx, s.cell.y + dy);
                sockets[i] = s;
            }
        }

        private bool TryGetLayoutMin(out int minX, out int minY)
        {
            int localMinX = int.MaxValue;
            int localMinY = int.MaxValue;
            bool found = false;
            Action<Vector2Int> include = c =>
            {
                localMinX = Mathf.Min(localMinX, c.x);
                localMinY = Mathf.Min(localMinY, c.y);
                found = true;
            };

            for (int x = 0; x < gridWidth; x++)
                for (int y = 0; y < gridHeight; y++)
                    if (cells[x, y] != CellState.Empty) include(new Vector2Int(x, y));
            if (doorCell.HasValue) include(doorCell.Value);
            foreach (var r in waterPools) include(new Vector2Int(r.xMin, r.yMin));
            foreach (var r in interiorIslands) include(new Vector2Int(r.xMin, r.yMin));
            foreach (var s in sockets) include(s.cell);
            minX = found ? localMinX : 0;
            minY = found ? localMinY : 0;
            return found;
        }

        private static void ShiftRects(List<RectInt> rects, int dx, int dy)
        {
            for (int i = 0; i < rects.Count; i++)
            {
                var r = rects[i];
                rects[i] = new RectInt(r.x + dx, r.y + dy, r.width, r.height);
            }
        }

        private int RemoveOrphanCells()
        {
            var toRemove = new List<Vector2Int>();
            for (int x = 0; x < gridWidth; x++)
                for (int y = 0; y < gridHeight; y++)
                {
                    if (!IsFootprint(x, y)) continue;
                    if (!IsFootprint(x + 1, y) && !IsFootprint(x - 1, y) && !IsFootprint(x, y + 1) && !IsFootprint(x, y - 1))
                        toRemove.Add(new Vector2Int(x, y));
                }

            foreach (var c in toRemove)
            {
                cells[c.x, c.y] = CellState.Empty;
                if (doorCell.HasValue && doorCell.Value == c) doorCell = null;
                RemoveSocketsAt(c);
            }
            return toRemove.Count;
        }

        private static bool HasAnyFrontEdge(HashSet<Vector2Int> walkable)
        {
            foreach (var c in walkable)
                if (!walkable.Contains(new Vector2Int(c.x, c.y - 1)))
                    return true;
            return false;
        }

        private void FrameInSceneView()
        {
            if (string.IsNullOrEmpty(lastGeneratedName))
            {
                EditorUtility.DisplayDialog("World Painter", "Önce bir oda Generate et.", "OK");
                return;
            }
            var go = GameObject.Find(lastGeneratedName);
            if (go == null) return;
            FrameInSceneViewObj(go);
        }

        private static void FrameInSceneViewObj(GameObject go)
        {
            if (go == null) return;
            Selection.activeGameObject = go;
            var sv = SceneView.lastActiveSceneView;
            if (sv != null) sv.FrameSelected();
        }

        // ============== SAVE/LOAD ==============
        private void SaveLayout()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string folder = Path.Combine(projectRoot, LayoutsFolder.Replace('/', Path.DirectorySeparatorChar));
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            string defaultName = "room_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".json";
            string path = EditorUtility.SaveFilePanel("Save Room Layout", folder, defaultName, "json");
            if (string.IsNullOrEmpty(path)) return;
            File.WriteAllText(path, SerializeLayout(), Encoding.UTF8);
            lastSavedPath = path;
            Debug.Log("[World Painter] Saved to " + path);
        }

        private void LoadLayout()
        {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string folder = Path.Combine(projectRoot, LayoutsFolder.Replace('/', Path.DirectorySeparatorChar));
            if (!Directory.Exists(folder)) folder = projectRoot;
            string path = EditorUtility.OpenFilePanel("Load Room Layout", folder, "json");
            if (string.IsNullOrEmpty(path)) return;
            try
            {
                DeserializeLayout(File.ReadAllText(path, Encoding.UTF8));
                lastSavedPath = path;
                MarkLayoutChanged();
                Debug.Log("[World Painter] Loaded from " + path);
            }
            catch (Exception ex)
            {
                Debug.LogError("[World Painter] Load failed: " + ex.Message);
                EditorUtility.DisplayDialog("World Painter", "Load failed:\n" + ex.Message, "OK");
            }
        }

        private string SerializeLayout()
        {
            var sb = new StringBuilder();
            sb.Append("{\n");
            sb.Append("  \"schemaVersion\": 3,\n");
            sb.Append("  \"gridWidth\": ").Append(gridWidth).Append(",\n");
            sb.Append("  \"gridHeight\": ").Append(gridHeight).Append(",\n");
            sb.Append("  \"frontEdgeMode\": \"").Append(frontEdge.ToString()).Append("\",\n");
            sb.Append("  \"rearWallEnabled\": ").Append(rearWallEnabled ? "true" : "false").Append(",\n");
            sb.Append("  \"sideWallsEnabled\": ").Append(sideWallsEnabled ? "true" : "false").Append(",\n");
            sb.Append("  \"enforceCenteredRearDoor\": ").Append(enforceCenteredRearDoor ? "true" : "false").Append(",\n");
            sb.Append("  \"cellSize\": ").Append(cellSizeWorld.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append(",\n");
            sb.Append("  \"reservedCenterRadius\": ").Append(reservedCenterRadius).Append(",\n");
            sb.Append("  \"presetId\": \"").Append(selectedPresetId ?? "").Append("\",\n");
            AppendCellArray(sb, "walkable", CollectByState(CellState.Walkable)); sb.Append(",\n");
            string door = doorCell.HasValue ? $"[{doorCell.Value.x},{doorCell.Value.y}]" : "null";
            sb.Append("  \"door\": ").Append(door).Append(",\n");
            AppendCellArray(sb, "alcoves", CollectByState(CellState.Alcove)); sb.Append(",\n");
            AppendCellArray(sb, "protrusions", CollectByState(CellState.Protrusion)); sb.Append(",\n");
            AppendRectArray(sb, "waterPools", waterPools); sb.Append(",\n");
            AppendRectArray(sb, "interiorIslands", interiorIslands); sb.Append(",\n");
            AppendSocketArray(sb, "sockets", sockets);
            sb.Append("\n}\n");
            return sb.ToString();
        }

        private static void AppendSocketArray(StringBuilder sb, string name, List<RoomSocket> list)
        {
            sb.Append("  \"").Append(name).Append("\": [");
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0) sb.Append(", ");
                var s = list[i];
                sb.Append("[").Append(s.cell.x).Append(",").Append(s.cell.y).Append(",\"")
                  .Append(s.type.ToString()).Append("\",\"")
                  .Append(EscapeJson(s.metadata ?? "")).Append("\"]");
            }
            sb.Append("]");
        }

        private static string EscapeJson(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }

        private static void AppendRectArray(StringBuilder sb, string name, List<RectInt> rects)
        {
            sb.Append("  \"").Append(name).Append("\": [");
            for (int i = 0; i < rects.Count; i++)
            {
                if (i > 0) sb.Append(", ");
                var r = rects[i];
                sb.Append("[").Append(r.x).Append(",").Append(r.y).Append(",")
                  .Append(r.width).Append(",").Append(r.height).Append("]");
            }
            sb.Append("]");
        }

        private static void AppendCellArray(StringBuilder sb, string name, List<Vector2Int> list)
        {
            sb.Append("  \"").Append(name).Append("\": [");
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0) sb.Append(", ");
                sb.Append("[").Append(list[i].x).Append(",").Append(list[i].y).Append("]");
            }
            sb.Append("]");
        }

        private void DeserializeLayout(string json)
        {
            var p = new MiniJson(json);
            int w = (int)p.GetNumber("gridWidth", DefaultGrid);
            int h = (int)p.GetNumber("gridHeight", DefaultGrid);
            string feStr = p.GetString("frontEdgeMode", frontEdge.ToString());
            bool rw = p.GetBool("rearWallEnabled", true);
            bool sw = p.GetBool("sideWallsEnabled", true);
            bool centeredDoor = p.GetBool("enforceCenteredRearDoor", true);
            float cs = (float)p.GetNumber("cellSize", 1f);
            int rcr = (int)p.GetNumber("reservedCenterRadius", 0);
            string presetId = p.GetString("presetId", "");
            var walkable = p.GetCellArray("walkable");
            var loadedDoor = p.GetPointOrNull("door");
            var alcoves = p.GetCellArray("alcoves");
            var protrusions = p.GetCellArray("protrusions");
            var waterArr = p.GetRectArray("waterPools");
            var islandArr = p.GetRectArray("interiorIslands");
            var socketArr = p.GetSocketArray("sockets");

            gridWidth = Mathf.Clamp(w, MinGrid, MaxGrid);
            gridHeight = Mathf.Clamp(h, MinGrid, MaxGrid);
            cellSizeWorld = cs;
            rearWallEnabled = rw;
            sideWallsEnabled = sw;
            enforceCenteredRearDoor = centeredDoor;
            reservedCenterRadius = rcr;
            if (Enum.TryParse<FrontEdgeMode>(feStr, out var fe)) frontEdge = fe;
            cells = new CellState[gridWidth, gridHeight];
            doorCell = null;
            waterPools.Clear();
            interiorIslands.Clear();
            sockets.Clear();

            foreach (var c in walkable) SetIfInBounds(c, CellState.Walkable);
            foreach (var c in alcoves) SetIfInBounds(c, CellState.Alcove);
            foreach (var c in protrusions) SetIfInBounds(c, CellState.Protrusion);
            if (loadedDoor.HasValue)
            {
                var d = loadedDoor.Value;
                if (d.x >= 0 && d.y >= 0 && d.x < gridWidth && d.y < gridHeight) doorCell = d;
            }
            foreach (var r in waterArr) waterPools.Add(r);
            foreach (var r in islandArr) interiorIslands.Add(r);
            foreach (var s in socketArr)
                if (s.cell.x >= 0 && s.cell.y >= 0 && s.cell.x < gridWidth && s.cell.y < gridHeight)
                    sockets.Add(s);
            selectedPresetId = presetId;
        }

        private void SetIfInBounds(Vector2Int c, CellState s)
        {
            if (c.x < 0 || c.y < 0 || c.x >= gridWidth || c.y >= gridHeight) return;
            cells[c.x, c.y] = s;
        }

        private class MiniJson
        {
            private readonly string s;
            public MiniJson(string raw) { s = raw ?? string.Empty; }
            public double GetNumber(string key, double fallback)
            {
                int i = FindKey(key);
                if (i < 0) return fallback;
                int end = s.IndexOfAny(new[] { ',', '}', '\n', '\r' }, i);
                if (end < 0) end = s.Length;
                return double.TryParse(s.Substring(i, end - i).Trim(), System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out var v) ? v : fallback;
            }
            public bool GetBool(string key, bool fallback)
            {
                int i = FindKey(key);
                if (i < 0) return fallback;
                return s.Substring(i, Mathf.Min(5, s.Length - i)).StartsWith("true");
            }
            public string GetString(string key, string fallback)
            {
                int i = FindKey(key);
                if (i < 0) return fallback;
                int q1 = s.IndexOf('"', i);
                if (q1 < 0) return fallback;
                int q2 = s.IndexOf('"', q1 + 1);
                if (q2 < 0) return fallback;
                return s.Substring(q1 + 1, q2 - q1 - 1);
            }
            public List<RectInt> GetRectArray(string key)
            {
                var list = new List<RectInt>();
                int i = FindKey(key);
                if (i < 0) return list;
                int open = s.IndexOf('[', i);
                if (open < 0) return list;
                int depth = 0, close = -1;
                for (int k = open; k < s.Length; k++)
                {
                    if (s[k] == '[') depth++;
                    else if (s[k] == ']') { depth--; if (depth == 0) { close = k; break; } }
                }
                if (close < 0) return list;
                string body = s.Substring(open + 1, close - open - 1);
                int pos = 0;
                while (pos < body.Length)
                {
                    int b1 = body.IndexOf('[', pos);
                    if (b1 < 0) break;
                    int b2 = body.IndexOf(']', b1 + 1);
                    if (b2 < 0) break;
                    string inner = body.Substring(b1 + 1, b2 - b1 - 1);
                    var parts = inner.Split(',');
                    if (parts.Length >= 4
                        && int.TryParse(parts[0].Trim(), out int rx)
                        && int.TryParse(parts[1].Trim(), out int ry)
                        && int.TryParse(parts[2].Trim(), out int rw)
                        && int.TryParse(parts[3].Trim(), out int rh))
                        list.Add(new RectInt(rx, ry, rw, rh));
                    pos = b2 + 1;
                }
                return list;
            }

            public List<RoomSocket> GetSocketArray(string key)
            {
                var list = new List<RoomSocket>();
                int i = FindKey(key);
                if (i < 0) return list;
                int open = s.IndexOf('[', i);
                if (open < 0) return list;
                int depth = 0, close = -1;
                for (int k = open; k < s.Length; k++)
                {
                    if (s[k] == '[') depth++;
                    else if (s[k] == ']') { depth--; if (depth == 0) { close = k; break; } }
                }
                if (close < 0) return list;

                string body = s.Substring(open + 1, close - open - 1);
                int pos = 0;
                while (pos < body.Length)
                {
                    int b1 = body.IndexOf('[', pos);
                    if (b1 < 0) break;
                    int b2 = FindMatchingBracket(body, b1);
                    if (b2 < 0) break;
                    string inner = body.Substring(b1 + 1, b2 - b1 - 1);
                    var parts = SplitJsonParts(inner);
                    if (parts.Count >= 3
                        && int.TryParse(parts[0].Trim(), out int x)
                        && int.TryParse(parts[1].Trim(), out int y)
                        && Enum.TryParse(TrimJsonString(parts[2]), out SocketType type))
                    {
                        string metadata = parts.Count >= 4 ? TrimJsonString(parts[3]) : "";
                        list.Add(new RoomSocket { cell = new Vector2Int(x, y), type = type, metadata = metadata });
                    }
                    pos = b2 + 1;
                }
                return list;
            }

            private static int FindMatchingBracket(string body, int open)
            {
                bool inString = false;
                bool escape = false;
                int depth = 0;
                for (int i = open; i < body.Length; i++)
                {
                    char ch = body[i];
                    if (escape) { escape = false; continue; }
                    if (ch == '\\' && inString) { escape = true; continue; }
                    if (ch == '"') { inString = !inString; continue; }
                    if (inString) continue;
                    if (ch == '[') depth++;
                    else if (ch == ']')
                    {
                        depth--;
                        if (depth == 0) return i;
                    }
                }
                return -1;
            }

            private static List<string> SplitJsonParts(string inner)
            {
                var parts = new List<string>();
                var current = new StringBuilder();
                bool inString = false;
                bool escape = false;
                for (int i = 0; i < inner.Length; i++)
                {
                    char ch = inner[i];
                    if (escape)
                    {
                        current.Append(ch);
                        escape = false;
                        continue;
                    }
                    if (ch == '\\' && inString)
                    {
                        current.Append(ch);
                        escape = true;
                        continue;
                    }
                    if (ch == '"')
                    {
                        inString = !inString;
                        current.Append(ch);
                        continue;
                    }
                    if (ch == ',' && !inString)
                    {
                        parts.Add(current.ToString());
                        current.Length = 0;
                        continue;
                    }
                    current.Append(ch);
                }
                parts.Add(current.ToString());
                return parts;
            }

            private static string TrimJsonString(string raw)
            {
                raw = (raw ?? "").Trim();
                if (raw.Length >= 2 && raw[0] == '"' && raw[raw.Length - 1] == '"')
                    raw = raw.Substring(1, raw.Length - 2);
                return raw.Replace("\\\"", "\"").Replace("\\\\", "\\");
            }

            public List<Vector2Int> GetCellArray(string key)
            {
                var list = new List<Vector2Int>();
                int i = FindKey(key);
                if (i < 0) return list;
                int open = s.IndexOf('[', i);
                if (open < 0) return list;
                int depth = 0, close = -1;
                for (int k = open; k < s.Length; k++)
                {
                    if (s[k] == '[') depth++;
                    else if (s[k] == ']') { depth--; if (depth == 0) { close = k; break; } }
                }
                if (close < 0) return list;
                string body = s.Substring(open + 1, close - open - 1);
                int pos = 0;
                while (pos < body.Length)
                {
                    int b1 = body.IndexOf('[', pos);
                    if (b1 < 0) break;
                    int b2 = body.IndexOf(']', b1 + 1);
                    if (b2 < 0) break;
                    string inner = body.Substring(b1 + 1, b2 - b1 - 1);
                    var parts = inner.Split(',');
                    if (parts.Length >= 2 && int.TryParse(parts[0].Trim(), out int x) && int.TryParse(parts[1].Trim(), out int y))
                        list.Add(new Vector2Int(x, y));
                    pos = b2 + 1;
                }
                return list;
            }

            public Vector2Int? GetPointOrNull(string key)
            {
                int i = FindKey(key);
                if (i < 0) return null;
                int open = s.IndexOf('[', i);
                int nullIdx = s.IndexOf("null", i, StringComparison.Ordinal);
                if (nullIdx >= 0 && (open < 0 || nullIdx < open)) return null;
                if (open < 0) return null;
                int close = s.IndexOf(']', open);
                if (close < 0) return null;

                var inner = s.Substring(open + 1, close - open - 1);
                var parts = inner.Split(',');
                if (parts.Length >= 2
                    && int.TryParse(parts[0].Trim(), out int x)
                    && int.TryParse(parts[1].Trim(), out int y))
                    return new Vector2Int(x, y);
                return null;
            }

            private int FindKey(string key)
            {
                string token = "\"" + key + "\"";
                int idx = s.IndexOf(token, StringComparison.Ordinal);
                if (idx < 0) return -1;
                int colon = s.IndexOf(':', idx + token.Length);
                if (colon < 0) return -1;
                return colon + 1;
            }
        }
    }
}
