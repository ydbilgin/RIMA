#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace RIMA
{
    /// <summary>
    /// Editor tool — builds a Hades-style single combat room procedurally.
    ///
    /// Room layout (24 × 18 tile grid):
    ///   • 2-tile stone walls on all 4 sides
    ///   • 2-tile door gaps: N / S (center column) and E / W (center row)
    ///   • Floor tiles fill the 20×14 center area (inside the walls)
    ///   • 4 stone columns (2×2 tiles) placed at inner wall corners
    ///   • 2 narrow passages on mid-edge sides (N and S mid-walls)
    ///   • Random obstacle prefabs scattered outside the center keep-clear zone
    ///
    /// Tilemap layers created under a "Room" Grid GameObject:
    ///   Floor    (sortingOrder 0)   — center 16×11 area floor tile
    ///   Wall     (sortingOrder 1)   — 2-tile walls + physics colliders
    ///   Obstacle (sortingOrder 2)   — stone column tiles overlaid on floor
    ///
    /// Obstacle GameObjects are parented under Room/ObstacleInstances.
    ///
    /// Usage: RIMA ▸ 3. Build Room    (seed 42)
    ///         RIMA ▸ 3b. Build Room (New Seed)
    /// </summary>
    public static class RoomBuilder
    {
        // ─── Room constants ────────────────────────────────────────────────
        private const int W  = 32;   // tiles wide  (total grid)
        private const int H  = 24;   // tiles tall  (total grid)
        private const int WallT = 2; // wall thickness in tiles
        private const int DW =  2;   // door gap width / height in tiles (2-tile doors)

        // Floor area: inside the walls
        // x: WallT … W-WallT-1  (cols 2..29, width=28)
        // y: WallT … H-WallT-1  (rows 2..21, height=20)
        private const int FloorX = WallT;
        private const int FloorY = WallT;
        private const int FloorW = W - WallT * 2;  // 28
        private const int FloorH = H - WallT * 2;  // 20

        // Door start indices (inclusive). Covers DoorXStart … DoorXStart + DW - 1
        // For W=32, DW=2  →  DoorXStart = 15  (tiles 15,16)
        // For H=24, DW=2  →  DoorYStart = 11  (tiles 11,12)
        private static int DoorXStart => (W - DW) / 2;   // 15
        private static int DoorYStart => (H - DW) / 2;   // 11

        // Keep-clear zone: no random obstacles placed here (room center).
        // Covers columns 12-19, rows 8-15
        private static readonly RectInt KeepClear = new RectInt(12, 8, 8, 8);

        // Tile corridors blocked in front of each door (keeps entrances open)
        private const int DoorClearDepth = 4;

        // ─── Asset paths ──────────────────────────────────────────────────
        private const string TileRootFolder  = "Assets/Tiles";
        private const string FloorTilePaths  = "Assets/Tiles/Floor.asset";
        private const string WallTilePaths   = "Assets/Tiles/Wall.asset";
        private const string ColumnTilePaths = "Assets/Tiles/Column.asset";

        // Obstacle prefab paths (relative to project root)
        private const string PfbStoneColumn   = "Assets/Prefabs/Obstacles/StoneColumn.prefab";
        private const string PfbNarrowPassage = "Assets/Prefabs/Obstacles/NarrowPassage.prefab";
        private const string PfbChasm         = "Assets/Prefabs/Obstacles/Chasm.prefab";

        // ─── Menu items ───────────────────────────────────────────────────
        [MenuItem("RIMA/3. Build Room")]
        public static void BuildDefault() => Build(42);

        [MenuItem("RIMA/3b. Build Room (New Seed)")]
        public static void BuildNewSeed() => Build(Random.Range(0, 999_999));

        // ─── Entry point ──────────────────────────────────────────────────
        /// <summary>Full room build.  Call with any seed from code.</summary>
        public static void Build(int seed)
        {
            Debug.Log($"[RoomBuilder] ▶ Building room (seed={seed}) …");
            var rng = new System.Random(seed);

            // 1. Scene hierarchy
            var roomRoot    = GetOrCreateGrid("Room");
            var floorMap    = SetupFloorTilemap(roomRoot);
            var wallMap     = SetupWallTilemap(roomRoot);
            var obstMap     = SetupObstacleTilemap(roomRoot);
            var obsParent   = GetOrCreateChildGO(roomRoot, "ObstacleInstances").transform;

            // 2. Clear previous build
            floorMap.ClearAllTiles();
            wallMap.ClearAllTiles();
            obstMap.ClearAllTiles();
            for (int i = obsParent.childCount - 1; i >= 0; i--)
                Object.DestroyImmediate(obsParent.GetChild(i).gameObject);

            // 3. Tile assets (auto-created if Assets/Tiles/ doesn't have them yet)
            EnsureFolder(TileRootFolder);
            var floorTile  = GetOrCreateTile(FloorTilePaths,  new Color32(89,  74,  54, 255));
            var wallTile   = GetOrCreateTile(WallTilePaths,   new Color32(52,  44,  36, 255));
            var columnTile = GetOrCreateTile(ColumnTilePaths, new Color32(68,  60,  50, 255));

            // 4. Floor: center 16×11 area (inside walls)
            for (int x = FloorX; x < FloorX + FloorW; x++)
            for (int y = FloorY; y < FloorY + FloorH; y++)
                floorMap.SetTile(new Vector3Int(x, y, 0), floorTile);

            // 5. Walls (2-tile thickness) with 2-tile door gaps + track occupied cells
            var occupied = new HashSet<Vector2Int>();
            PaintWalls(wallMap, wallTile, occupied);

            // 6. Corners: place 2×2 stone column tiles at the 4 inner corner positions
            PaintCornerColumns(obstMap, columnTile, occupied);

            // 7. Narrow passages on mid North and South inner walls
            PaintMidPassages(obstMap, columnTile, occupied);

            // 8. Block tile cells near each door (no obstacles in the corridors)
            MarkDoorCorridors(occupied);

            // 9. Place random obstacle prefabs
            PlaceObstacles(rng, PfbStoneColumn,   2, 3, floorMap, obsParent, occupied);
            PlaceObstacles(rng, PfbNarrowPassage, 1, 2, floorMap, obsParent, occupied);
            PlaceObstacles(rng, PfbChasm,         0, 1, floorMap, obsParent, occupied);

            // 10. Dirty
            EditorUtility.SetDirty(roomRoot);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            Debug.Log($"[RoomBuilder] ✔ Room built (seed={seed}). Grid={W}x{H}, Floor={FloorW}x{FloorH}, DoorX={DoorXStart}, DoorY={DoorYStart}.");
        }

        // ─── Wall painting ────────────────────────────────────────────────
        private static void PaintWalls(Tilemap wallMap, Tile tile, HashSet<Vector2Int> occupied)
        {
            // Horizontal walls: South rows (y=0..WallT-1) and North rows (y=H-WallT..H-1)
            for (int x = 0; x < W; x++)
            {
                for (int t = 0; t < WallT; t++)
                {
                    // South wall — skip door columns
                    if (!IsDoorX(x))
                    {
                        SetWall(wallMap, tile, x, t, occupied);
                        SetWall(wallMap, tile, x, H - 1 - t, occupied);
                    }
                }
            }

            // Vertical walls: West cols (x=0..WallT-1) and East cols (x=W-WallT..W-1)
            for (int y = 0; y < H; y++)
            {
                for (int t = 0; t < WallT; t++)
                {
                    if (!IsDoorY(y))
                    {
                        SetWall(wallMap, tile, t, y, occupied);
                        SetWall(wallMap, tile, W - 1 - t, y, occupied);
                    }
                }
            }
        }

        // ─── Corner columns (2×2 stone pillars at inner wall corners) ────────
        private static void PaintCornerColumns(Tilemap obstMap, Tile tile, HashSet<Vector2Int> occupied)
        {
            // Inner corner positions: just inside the walls (at WallT, WallT)
            // 4 corners: SW, SE, NW, NE — each a 2×2 block
            var corners = new Vector2Int[]
            {
                new Vector2Int(FloorX,            FloorY),              // SW inner corner
                new Vector2Int(FloorX + FloorW - 2, FloorY),            // SE inner corner
                new Vector2Int(FloorX,            FloorY + FloorH - 2), // NW inner corner
                new Vector2Int(FloorX + FloorW - 2, FloorY + FloorH - 2), // NE inner corner
            };

            foreach (var corner in corners)
            {
                for (int dx = 0; dx < 2; dx++)
                for (int dy = 0; dy < 2; dy++)
                {
                    int cx = corner.x + dx;
                    int cy = corner.y + dy;
                    obstMap.SetTile(new Vector3Int(cx, cy, 0), tile);
                    occupied.Add(new Vector2Int(cx, cy));
                }
            }
        }

        // ─── Mid-edge narrow passages (2-tile wide pillars on N and S inner edges) ──
        private static void PaintMidPassages(Tilemap obstMap, Tile tile, HashSet<Vector2Int> occupied)
        {
            // Narrow passage blocks: 2 tiles wide × 1 tile deep on inner N and S walls
            // Positioned just inside the wall, offset to either side of the center door
            // Left passage: DoorXStart - 3 (2 tiles wide)
            // Right passage: DoorXStart + DW + 1 (2 tiles wide)
            int[] passageXs = { DoorXStart - 3, DoorXStart + DW + 1 };

            foreach (int px in passageXs)
            {
                if (px < FloorX || px + 1 >= FloorX + FloorW) continue; // safety: don't overflow floor

                // South inner edge (just above south wall)
                for (int dx = 0; dx < 2; dx++)
                {
                    obstMap.SetTile(new Vector3Int(px + dx, FloorY, 0), tile);
                    occupied.Add(new Vector2Int(px + dx, FloorY));
                }
                // North inner edge (just below north wall)
                for (int dx = 0; dx < 2; dx++)
                {
                    obstMap.SetTile(new Vector3Int(px + dx, FloorY + FloorH - 1, 0), tile);
                    occupied.Add(new Vector2Int(px + dx, FloorY + FloorH - 1));
                }
            }
        }

        private static void SetWall(Tilemap map, Tile tile, int x, int y, HashSet<Vector2Int> occupied)
        {
            map.SetTile(new Vector3Int(x, y, 0), tile);
            occupied.Add(new Vector2Int(x, y));
        }

        private static void MarkDoorCorridors(HashSet<Vector2Int> occupied)
        {
            // South / North door corridors (column band near top/bottom edges)
            for (int x = DoorXStart; x < DoorXStart + DW; x++)
            {
                for (int d = 0; d < DoorClearDepth; d++)
                {
                    occupied.Add(new Vector2Int(x, d));           // South
                    occupied.Add(new Vector2Int(x, H - 1 - d));   // North
                }
            }

            // West / East door corridors (row band near left/right edges)
            for (int y = DoorYStart; y < DoorYStart + DW; y++)
            {
                for (int d = 0; d < DoorClearDepth; d++)
                {
                    occupied.Add(new Vector2Int(d, y));            // West
                    occupied.Add(new Vector2Int(W - 1 - d, y));   // East
                }
            }
        }

        // ─── Obstacle placement ───────────────────────────────────────────
        private static void PlaceObstacles(
            System.Random rng,
            string prefabPath,
            int min, int max,
            Tilemap floorMap,
            Transform parent,
            HashSet<Vector2Int> occupied)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogWarning($"[RoomBuilder] Prefab not found: {prefabPath} — skipping.");
                return;
            }

            int target   = rng.Next(min, max + 1);
            int placed   = 0;
            int attempts = 0;

            while (placed < target && attempts < 500)
            {
                attempts++;

                // Random position inside room, staying clear of walls (x/y ≥ 2)
                int x = rng.Next(2, W - 3);
                int y = rng.Next(2, H - 3);
                var cell = new Vector2Int(x, y);

                if (occupied.Contains(cell))    continue;
                if (KeepClear.Contains(cell))   continue;

                // Reserve a 2×2 footprint so obstacles don't crowd each other
                if (occupied.Contains(new Vector2Int(x + 1, y    ))) continue;
                if (occupied.Contains(new Vector2Int(x,     y + 1))) continue;
                if (occupied.Contains(new Vector2Int(x + 1, y + 1))) continue;

                occupied.Add(cell);
                occupied.Add(new Vector2Int(x + 1, y));
                occupied.Add(new Vector2Int(x,     y + 1));
                occupied.Add(new Vector2Int(x + 1, y + 1));

                // World position = cell center
                Vector3 worldPos  = floorMap.GetCellCenterWorld(new Vector3Int(x, y, 0));
                worldPos.z        = 0f;

                // Slight random rotation (0° or 90°) — good for passages/chasms
                float   angle = rng.Next(0, 2) * 90f;
                var     rot   = Quaternion.Euler(0f, 0f, angle);

                var go = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
                go.transform.SetPositionAndRotation(worldPos, rot);
                placed++;
            }

            if (placed < min)
                Debug.LogWarning($"[RoomBuilder] Only placed {placed}/{min} {prefab.name}. " +
                                 "Room may be too crowded — try a different seed.");
        }

        // ─── Door predicate helpers ───────────────────────────────────────
        // DoorX: columns 9-10 (center of W=20, DW=2)  → gap in S/N walls
        // DoorY: rows 6-7   (center of H=15, DW=2)    → gap in W/E walls
        private static bool IsDoorX(int x) => x >= DoorXStart && x < DoorXStart + DW;
        private static bool IsDoorY(int y) => y >= DoorYStart && y < DoorYStart + DW;

        // ─── GameObject / Tilemap helpers ─────────────────────────────────
        private static GameObject GetOrCreateGrid(string name)
        {
            // IsoGame sahnesinde zaten IsoGrid var — önce onu dene
            var go = GameObject.Find("IsoGrid") ?? GameObject.Find(name);
            if (go == null) go = new GameObject("IsoGrid");

            if (!go.TryGetComponent<Grid>(out var grid)) grid = go.AddComponent<Grid>();

            // Isometric layout + RIMA cell size (0.94 x 0.94) + scaled Y
            grid.cellLayout = GridLayout.CellLayout.Isometric;
            grid.cellSize   = new Vector3(0.94f, 0.94f, 1f);
            go.transform.localScale = new Vector3(1f, 0.5f, 1f);

            return go;
        }

        private static GameObject GetOrCreateChildGO(GameObject parent, string childName)
        {
            var existing = parent.transform.Find(childName);
            if (existing != null) return existing.gameObject;
            var child = new GameObject(childName);
            child.transform.SetParent(parent.transform, false);
            return child;
        }

        private static Tilemap SetupFloorTilemap(GameObject parent)
        {
            var go = GetOrCreateChildGO(parent, "Ground"); // IsoGrid/Ground ile eşleşir
            if (!go.TryGetComponent<Tilemap>(out _)) go.AddComponent<Tilemap>();
            var tm = go.GetComponent<Tilemap>();
            if (!go.TryGetComponent<TilemapRenderer>(out _)) go.AddComponent<TilemapRenderer>();
            var tr = go.GetComponent<TilemapRenderer>();
            if (tr != null) tr.sortingOrder = 0;
            return tm;
        }

        private static Tilemap SetupWallTilemap(GameObject parent)
        {
            var go = GetOrCreateChildGO(parent, "Walls"); // IsoGrid/Walls ile eşleşir
            if (!go.TryGetComponent<Tilemap>(out _)) go.AddComponent<Tilemap>();
            var tm = go.GetComponent<Tilemap>();
            if (!go.TryGetComponent<TilemapRenderer>(out _)) go.AddComponent<TilemapRenderer>();
            var tr = go.GetComponent<TilemapRenderer>();
            if (tr != null) tr.sortingOrder = 1;

            // Physics: static Rigidbody2D → TilemapCollider2D (composite) → CompositeCollider2D
            if (!go.TryGetComponent<Rigidbody2D>(out _)) go.AddComponent<Rigidbody2D>();
            var rb = go.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;

            if (!go.TryGetComponent<TilemapCollider2D>(out _)) go.AddComponent<TilemapCollider2D>();
            var tc = go.GetComponent<TilemapCollider2D>();
            tc.compositeOperation = Collider2D.CompositeOperation.Merge;

            if (!go.TryGetComponent<CompositeCollider2D>(out _)) go.AddComponent<CompositeCollider2D>();
            var cc = go.GetComponent<CompositeCollider2D>();
            if (cc != null) cc.geometryType = CompositeCollider2D.GeometryType.Polygons;

            return tm;
        }

        private static Tilemap SetupObstacleTilemap(GameObject parent)
        {
            var go = GetOrCreateChildGO(parent, "Obstacle");
            if (!go.TryGetComponent<Tilemap>(out _)) go.AddComponent<Tilemap>();
            var tm = go.GetComponent<Tilemap>();
            if (!go.TryGetComponent<TilemapRenderer>(out _)) go.AddComponent<TilemapRenderer>();
            var tr = go.GetComponent<TilemapRenderer>();
            if (tr != null) tr.sortingOrder = 2;

            // Obstacle tiles also block movement
            if (!go.TryGetComponent<Rigidbody2D>(out _)) go.AddComponent<Rigidbody2D>();
            var rb = go.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;

            if (!go.TryGetComponent<TilemapCollider2D>(out _)) go.AddComponent<TilemapCollider2D>();
            var tc = go.GetComponent<TilemapCollider2D>();
            tc.compositeOperation = Collider2D.CompositeOperation.Merge;

            if (!go.TryGetComponent<CompositeCollider2D>(out _)) go.AddComponent<CompositeCollider2D>();

            return tm;
        }

        // ─── Tile asset helpers ───────────────────────────────────────────

        /// <summary>
        /// Returns the Tile at <paramref name="assetPath"/>, creating it if it
        /// does not exist yet (writes a 4×4 white PNG → imports as Sprite → Tile).
        /// </summary>
        private static Tile GetOrCreateTile(string assetPath, Color32 color)
        {
            var existing = AssetDatabase.LoadAssetAtPath<Tile>(assetPath);
            if (existing != null) return existing;

            // --- Create a tiny white source texture ---
            var tex = new Texture2D(4, 4, TextureFormat.RGBA32, false);
            tex.filterMode = FilterMode.Point;
            var px = new Color32[16];
            for (int i = 0; i < px.Length; i++) px[i] = new Color32(255, 255, 255, 255);
            tex.SetPixels32(px);
            tex.Apply();

            string texPath = assetPath.Replace(".asset", "_Tex.png");
            string absPath = Path.Combine(
                Application.dataPath.TrimEnd('/'),
                texPath.Substring("Assets/".Length));

            Directory.CreateDirectory(Path.GetDirectoryName(absPath)!);
            File.WriteAllBytes(absPath, tex.EncodeToPNG());
            Object.DestroyImmediate(tex);
            AssetDatabase.ImportAsset(texPath, ImportAssetOptions.ForceUpdate);

            // --- Configure importer ---
            var importer = (TextureImporter)AssetImporter.GetAtPath(texPath);
            if (importer != null)
            {
                importer.textureType         = TextureImporterType.Sprite;
                importer.spriteImportMode    = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = 4;
                importer.filterMode          = FilterMode.Point;
                importer.SaveAndReimport();
            }

            // --- Create Tile asset ---
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(texPath);
            var tile   = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprite;
            tile.color  = color;
            tile.flags  = TileFlags.LockColor;

            AssetDatabase.CreateAsset(tile, assetPath);
            AssetDatabase.SaveAssets();

            Debug.Log($"[RoomBuilder] Created tile asset: {assetPath}");
            return tile;
        }

        private static void EnsureFolder(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath)) return;
            var parts  = folderPath.Split('/');
            var cursor = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                var next = cursor + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(cursor, parts[i]);
                cursor = next;
            }
        }
    }
}
#endif
