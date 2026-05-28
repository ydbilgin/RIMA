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
            Protrusion
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
        private Vector3 spawnOrigin = Vector3.zero;
        private int cellPx = 24;
        private CellState[,] cells;
        private Vector2Int? doorCell;
        private int reservedCenterRadius = 0;
        private List<RectInt> waterPools = new List<RectInt>();
        private List<RectInt> interiorIslands = new List<RectInt>();
        private Vector2 canvasScroll;
        private Vector2Int hoverCell = new Vector2Int(-1, -1);
        private bool isDragging;
        private bool isBoxSelecting;
        private Vector2Int boxStart = new Vector2Int(-1, -1);
        private string lastSavedPath;
        private string lastGeneratedName;
        private bool replaceExisting = true;
        private string selectedPresetId = "";

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
        private static readonly Color HoverOutline = new Color(1f, 1f, 1f, 0.95f);

        [MenuItem("RIMA/V2/World Painter")]
        public static void ShowWindow()
        {
            var w = GetWindow<RoomPainterWindow>("World Painter", focus: true);
            w.minSize = new Vector2(880, 600);
            w.Focus();
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
                if (newW != gridWidth || newH != gridHeight) { gridWidth = newW; gridHeight = newH; EnsureGrid(); }
                cellPx = EditorGUILayout.IntSlider(new GUIContent("Zoom (px/cell)", "Canvas üzerinde her cell kaç piksel"), cellPx, MinCellPx, MaxCellPx);

                EditorGUILayout.Space(6);
                EditorGUILayout.LabelField("Oda Ayarları", EditorStyles.boldLabel);
                frontEdge = (FrontEdgeMode)EditorGUILayout.EnumPopup(new GUIContent("Ön Kenar", "Open=açık, LowWall=alçak parapet, Broken=kırık karışık"), frontEdge);
                rearWallEnabled = EditorGUILayout.Toggle(new GUIContent("Arka Duvar", "Üst (kuzey) duvarı render et"), rearWallEnabled);
                sideWallsEnabled = EditorGUILayout.Toggle(new GUIContent("Yan Duvarlar", "Sol/sağ (batı/doğu) duvarları render et"), sideWallsEnabled);
                cellSizeWorld = EditorGUILayout.Slider(new GUIContent("Cell World Size", "Sahnede her cell kaç birim"), cellSizeWorld, 0.5f, 2.0f);
                spawnOrigin = EditorGUILayout.Vector3Field("Spawn Origin", spawnOrigin);
                replaceExisting = EditorGUILayout.Toggle(new GUIContent("Eskiyi Sil", "Generate edince mevcut PaintedRoom_* objelerini sil"), replaceExisting);

                EditorGUILayout.Space(8);
                EditorGUILayout.LabelField("Eylemler", EditorStyles.boldLabel);
                if (GUILayout.Button(new GUIContent("✦ Generate Room", "Painted cell'leri WallChainRoomBuilder'a feed et + sahneye duvarları spawn et"), GUILayout.Height(32))) GenerateRoom();
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button(new GUIContent("Clear", "Tüm paint'i sıfırla"))) ClearCanvas();
                    if (GUILayout.Button(new GUIContent("Frame", "Son üretilen odayı Scene View'da framele"))) FrameInSceneView();
                }
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Save")) SaveLayout();
                    if (GUILayout.Button("Load")) LoadLayout();
                }

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
                if (GUILayout.Button(new GUIContent("Library", "Kütüphane/alcove odası — yan nişler + ortada island"))) LoadPreset_LibraryAlcove();
                if (GUILayout.Button(new GUIContent("Combat", "Dar üstlü diamond savaş odası — stair-step yan zincir"))) LoadPreset_NarrowCombat();
            }
            using (new EditorGUILayout.HorizontalScope())
            {
                if (GUILayout.Button(new GUIContent("Ritual", "Ritüel odası — diamond + ortada sunak (reserved)"))) LoadPreset_RitualDiamond();
                if (GUILayout.Button(new GUIContent("Flooded", "Açık ön cephe + 2 yan su havuzu"))) LoadPreset_OpenFlooded();
            }
        }

        private void DrawBrushPicker()
        {
            var brushes = (BrushMode[])Enum.GetValues(typeof(BrushMode));
            string[] labels = { "1. Walkable (Zemin)", "2. Erase (Sil)", "3. Door (Kapı)", "4. Alcove (Niş)", "5. Protrusion (Çıkıntı)" };
            int selected = (int)brush;
            int next = GUILayout.SelectionGrid(selected, labels, 1, GUILayout.Height(22 * labels.Length));
            if (next != selected) brush = (BrushMode)next;

            using (new EditorGUILayout.HorizontalScope())
            {
                DrawSwatch(WalkableColor, "Zemin");
                DrawSwatch(EmptyColor, "Boş");
                DrawSwatch(DoorColor, "Kapı");
                DrawSwatch(AlcoveColor, "Niş");
                DrawSwatch(ProtrusionColor, "Bump");
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
                _ => ""
            };
            EditorGUILayout.HelpBox(text, MessageType.None);
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

                // WALL PREVIEW — walkable cell sınırlarında "duvar gelecek" overlay
                DrawWallPreview(gridRect);

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
                if (e.shift)
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
                else if (isDragging && brush != BrushMode.Door) PaintAt(cx, cy);
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
                    for (int px = x0; px <= x1; px++)
                        for (int py = y0; py <= y1; py++)
                            PaintAt(px, py);
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

        private void PaintAt(int x, int y)
        {
            if (x < 0 || y < 0 || x >= gridWidth || y >= gridHeight) return;
            switch (brush)
            {
                case BrushMode.Walkable: cells[x, y] = CellState.Walkable; break;
                case BrushMode.Erase:
                    cells[x, y] = CellState.Empty;
                    if (doorCell.HasValue && doorCell.Value == new Vector2Int(x, y)) doorCell = null;
                    break;
                case BrushMode.Door:
                    doorCell = new Vector2Int(x, y);
                    if (cells[x, y] == CellState.Empty) cells[x, y] = CellState.Walkable;
                    break;
                case BrushMode.Alcove: cells[x, y] = CellState.Alcove; break;
                case BrushMode.Protrusion: cells[x, y] = CellState.Protrusion; break;
            }
            Repaint();
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
            string status = $"Brush: {brush}  |  Hover: {hover}  |  Zemin: {walk}  |  Kapı: {door}  |  Niş: {alc}  |  Çıkıntı: {prt}";
            if (!string.IsNullOrEmpty(lastGeneratedName)) status += $"  |  Son oda: {lastGeneratedName}";
            EditorGUILayout.LabelField(status, EditorStyles.miniLabel);
        }

        // ============== PRESETS ==============
        private void LoadPreset_LibraryAlcove()
        {
            ClearCanvas();
            gridWidth = 22; gridHeight = 22;
            EnsureGrid();
            FillRect(0, 0, 22, 22, CellState.Walkable);
            // Sol alcove (niche) — duvarı oyacak şekilde
            SetCells(new[] { new Vector2Int(1, 10), new Vector2Int(1, 11), new Vector2Int(1, 12) }, CellState.Alcove);
            SetCells(new[] { new Vector2Int(20, 10), new Vector2Int(20, 11), new Vector2Int(20, 12) }, CellState.Alcove);
            // Door üst orta
            doorCell = new Vector2Int(11, 21);
            cells[11, 21] = CellState.Walkable;
            frontEdge = FrontEdgeMode.Open;
            // İslands (visual marker)
            interiorIslands.Clear();
            interiorIslands.Add(new RectInt(8, 8, 3, 2));
            interiorIslands.Add(new RectInt(13, 10, 2, 2));
            waterPools.Clear();
            reservedCenterRadius = 0;
            selectedPresetId = "library_alcove";
            Repaint();
        }

        private void LoadPreset_NarrowCombat()
        {
            ClearCanvas();
            gridWidth = 22; gridHeight = 22;
            EnsureGrid();
            // Diamond stair-step: top width 8, expand 1 per row down to max
            int topW = 8;
            int cx = gridWidth / 2;
            for (int y = gridHeight - 1; y >= 0; y--)
            {
                int rowFromTop = (gridHeight - 1 - y);
                int halfW = Mathf.Min(topW / 2 + rowFromTop, gridWidth / 2);
                int x0 = Mathf.Max(0, cx - halfW);
                int x1 = Mathf.Min(gridWidth - 1, cx + halfW - 1);
                for (int x = x0; x <= x1; x++) cells[x, y] = CellState.Walkable;
            }
            doorCell = new Vector2Int(cx, gridHeight - 1);
            frontEdge = FrontEdgeMode.Open;
            interiorIslands.Clear(); waterPools.Clear();
            reservedCenterRadius = 0;
            selectedPresetId = "narrow_combat";
            Repaint();
        }

        private void LoadPreset_RitualDiamond()
        {
            ClearCanvas();
            gridWidth = 22; gridHeight = 22;
            EnsureGrid();
            int topW = 6;
            int cx = gridWidth / 2;
            for (int y = gridHeight - 1; y >= 0; y--)
            {
                int rowFromTop = (gridHeight - 1 - y);
                int halfW = Mathf.Min(topW / 2 + rowFromTop, gridWidth / 2);
                int x0 = Mathf.Max(0, cx - halfW);
                int x1 = Mathf.Min(gridWidth - 1, cx + halfW - 1);
                for (int x = x0; x <= x1; x++) cells[x, y] = CellState.Walkable;
            }
            doorCell = new Vector2Int(cx, gridHeight - 1);
            frontEdge = FrontEdgeMode.LowWall;
            interiorIslands.Clear(); waterPools.Clear();
            reservedCenterRadius = 3;
            selectedPresetId = "ritual_diamond";
            Repaint();
        }

        private void LoadPreset_OpenFlooded()
        {
            ClearCanvas();
            gridWidth = 22; gridHeight = 16;
            EnsureGrid();
            FillRect(0, 0, 22, 16, CellState.Walkable);
            frontEdge = FrontEdgeMode.Open;
            waterPools.Clear();
            waterPools.Add(new RectInt(2, 4, 3, 4));
            waterPools.Add(new RectInt(17, 4, 3, 4));
            interiorIslands.Clear();
            reservedCenterRadius = 0;
            selectedPresetId = "open_flooded";
            Repaint();
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

            int maxX = 0, maxY = 0;
            foreach (var c in walkable) { if (c.x > maxX) maxX = c.x; if (c.y > maxY) maxY = c.y; }
            var alcoves = CollectByState(CellState.Alcove);
            var protrusions = CollectByState(CellState.Protrusion);
            foreach (var p in protrusions) { if (p.x > maxX) maxX = p.x; if (p.y > maxY) maxY = p.y; }

            var spec = new RoomSpec
            {
                roomName = (string.IsNullOrEmpty(selectedPresetId) ? "Painted" : selectedPresetId) + "_" + DateTime.Now.ToString("HHmmss"),
                widthCells = maxX + 1,
                heightCells = maxY + 1,
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
                enforceCenteredRearDoor = true,
                frontMinOpeningCells = 3
            };

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

            Selection.activeGameObject = go;
            lastGeneratedName = roomGoName;
            FrameInSceneViewObj(go);
            Debug.Log($"[World Painter] Generated {roomGoName} | walkable={walkable.Count} alcove={spec.alcovePositions.Count} prot={spec.protrusionPositions.Count} door={(spec.HasDoor ? spec.doorPosition.ToString() : "none")} preset={selectedPresetId}");
        }

        private List<NicheSpec> BuildNicheSpecs(List<Vector2Int> alcoves)
        {
            var specs = new List<NicheSpec>();
            foreach (var alcove in alcoves)
            {
                specs.Add(new NicheSpec
                {
                    side = alcove.x < gridWidth / 2 ? "left" : "right",
                    anchorRow = alcove.y,
                    width = 1,
                    depth = 1,
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
            selectedPresetId = "";
            Repaint();
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
                Repaint();
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
            sb.Append("  \"schemaVersion\": 2,\n");
            sb.Append("  \"gridWidth\": ").Append(gridWidth).Append(",\n");
            sb.Append("  \"gridHeight\": ").Append(gridHeight).Append(",\n");
            sb.Append("  \"frontEdgeMode\": \"").Append(frontEdge.ToString()).Append("\",\n");
            sb.Append("  \"rearWallEnabled\": ").Append(rearWallEnabled ? "true" : "false").Append(",\n");
            sb.Append("  \"sideWallsEnabled\": ").Append(sideWallsEnabled ? "true" : "false").Append(",\n");
            sb.Append("  \"cellSize\": ").Append(cellSizeWorld.ToString(System.Globalization.CultureInfo.InvariantCulture)).Append(",\n");
            sb.Append("  \"reservedCenterRadius\": ").Append(reservedCenterRadius).Append(",\n");
            sb.Append("  \"presetId\": \"").Append(selectedPresetId ?? "").Append("\",\n");
            AppendCellArray(sb, "walkable", CollectByState(CellState.Walkable)); sb.Append(",\n");
            string door = doorCell.HasValue ? $"[{doorCell.Value.x},{doorCell.Value.y}]" : "null";
            sb.Append("  \"door\": ").Append(door).Append(",\n");
            AppendCellArray(sb, "alcoves", CollectByState(CellState.Alcove)); sb.Append(",\n");
            AppendCellArray(sb, "protrusions", CollectByState(CellState.Protrusion)); sb.Append(",\n");
            AppendRectArray(sb, "waterPools", waterPools); sb.Append(",\n");
            AppendRectArray(sb, "interiorIslands", interiorIslands);
            sb.Append("\n}\n");
            return sb.ToString();
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
            float cs = (float)p.GetNumber("cellSize", 1f);
            int rcr = (int)p.GetNumber("reservedCenterRadius", 0);
            string presetId = p.GetString("presetId", "");
            var walkable = p.GetCellArray("walkable");
            var loadedDoor = p.GetPointOrNull("door");
            var alcoves = p.GetCellArray("alcoves");
            var protrusions = p.GetCellArray("protrusions");
            var waterArr = p.GetRectArray("waterPools");
            var islandArr = p.GetRectArray("interiorIslands");

            gridWidth = Mathf.Clamp(w, MinGrid, MaxGrid);
            gridHeight = Mathf.Clamp(h, MinGrid, MaxGrid);
            cellSizeWorld = cs;
            rearWallEnabled = rw;
            sideWallsEnabled = sw;
            reservedCenterRadius = rcr;
            if (Enum.TryParse<FrontEdgeMode>(feStr, out var fe)) frontEdge = fe;
            cells = new CellState[gridWidth, gridHeight];
            doorCell = null;
            waterPools.Clear();
            interiorIslands.Clear();

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
