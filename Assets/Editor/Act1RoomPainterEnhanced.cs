using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace RIMA
{
    /// <summary>
    /// Act 1 test odası — geliştirilmiş versiyon.
    /// Varyasyonlu zemin, çift sıra duvar, köşe sütunları, rastgele crack'ler,
    /// dekoratif meşale ve engel yerleşimi.
    /// Menu: RIMA / Paint Act1 Test Room (Enhanced)
    /// </summary>
    public static class Act1RoomPainterEnhanced
    {
        private const int RoomW = 24;
        private const int RoomH = 18;

        // Tile asset paths
        private const string FloorBasePath   = "Assets/Art/Tiles/Act1/Act1_Floor.asset";
        private const string FloorCrackPath  = "Assets/Art/Tiles/Act1/Act1_Floor_Crack.asset";
        private const string WallPath        = "Assets/Art/Tiles/Act1/Act1_Wall.asset";

        // Sprite paths for tile creation
        private const string FloorSpritePath      = "Assets/Sprites/Tiles/Act1_floor_stone.png";
        private const string FloorCrackSpritePath = "Assets/Sprites/Tiles/stone_floor_crack.png";
        private const string WallSpritePath       = "Assets/Sprites/Tiles/Act1_wall_stone.png";

        private const string FloorTilemapName = "Tilemap_Floor";
        private const string WallTilemapName  = "Tilemap_Walls";

        [MenuItem("RIMA/Paint Act1 Test Room (Enhanced)")]
        public static void Paint()
        {
            var floorTile = GetOrCreateTile(FloorBasePath, FloorSpritePath);
            var crackTile = GetOrCreateTile(FloorCrackPath, FloorCrackSpritePath);
            var wallTile  = GetOrCreateTile(WallPath, WallSpritePath);

            if (floorTile == null) { Debug.LogError("[RoomPainter] Floor tile bulunamadı"); return; }
            if (wallTile  == null) { Debug.LogError("[RoomPainter] Wall tile bulunamadı");  return; }

            // Tilemap bul veya oluştur
            var floorTilemap = FindOrCreateTilemap(FloorTilemapName, 0);
            var wallTilemap  = FindOrCreateTilemap(WallTilemapName, 10);

            if (floorTilemap == null || wallTilemap == null) return;

            floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();

            // ─── Zemin: varyasyonlu ─────────────────────────────────────
            for (int x = 2; x < RoomW - 2; x++)
            {
                for (int y = 2; y < RoomH - 2; y++)
                {
                    // %15 crack varyasyonu
                    if (crackTile != null && Random.value < 0.15f)
                        floorTilemap.SetTile(new Vector3Int(x, y, 0), crackTile);
                    else
                        floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }

            // ─── Duvarlar: çift sıra ────────────────────────────────────
            // Dış çerçeve
            for (int x = 0; x < RoomW; x++)
            {
                wallTilemap.SetTile(new Vector3Int(x, 0, 0), wallTile);
                wallTilemap.SetTile(new Vector3Int(x, 1, 0), wallTile);
                wallTilemap.SetTile(new Vector3Int(x, RoomH - 1, 0), wallTile);
                wallTilemap.SetTile(new Vector3Int(x, RoomH - 2, 0), wallTile);
            }
            for (int y = 0; y < RoomH; y++)
            {
                wallTilemap.SetTile(new Vector3Int(0, y, 0), wallTile);
                wallTilemap.SetTile(new Vector3Int(1, y, 0), wallTile);
                wallTilemap.SetTile(new Vector3Int(RoomW - 1, y, 0), wallTile);
                wallTilemap.SetTile(new Vector3Int(RoomW - 2, y, 0), wallTile);
            }

            // Duvar altında kalan zemin (boşluk kalmasın)
            for (int x = 0; x < RoomW; x++)
            {
                for (int y = 0; y < 2; y++)
                    floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
                for (int y = RoomH - 2; y < RoomH; y++)
                    floorTilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
            }
            for (int y = 0; y < RoomH; y++)
            {
                floorTilemap.SetTile(new Vector3Int(0, y, 0), floorTile);
                floorTilemap.SetTile(new Vector3Int(1, y, 0), floorTile);
                floorTilemap.SetTile(new Vector3Int(RoomW - 1, y, 0), floorTile);
                floorTilemap.SetTile(new Vector3Int(RoomW - 2, y, 0), floorTile);
            }

            // ─── Sütun engeller (ortada 2-4 adet) ──────────────────────
            PlacePillar(wallTilemap, wallTile, 7, 6);
            PlacePillar(wallTilemap, wallTile, 16, 6);
            PlacePillar(wallTilemap, wallTile, 7, 12);
            PlacePillar(wallTilemap, wallTile, 16, 12);

            // ─── Dekoratif objeler ──────────────────────────────────────
            SpawnDecoration("Torch_L", new Vector3(2.5f, 4.5f, 0));
            SpawnDecoration("Torch_R", new Vector3(RoomW - 2.5f, 4.5f, 0));
            SpawnDecoration("Torch_L", new Vector3(2.5f, RoomH - 4.5f, 0));
            SpawnDecoration("Torch_R", new Vector3(RoomW - 2.5f, RoomH - 4.5f, 0));

            // ─── Player'ı ortaya taşı ───────────────────────────────────
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                player.transform.position = new Vector3(RoomW / 2f, RoomH / 2f, 0f);

            // ─── Düşmanları anlamlı pozisyonlara dağıt ──────────────────
            PositionEnemies();

            // Wall tilemap'e Collider ekle
            var collider = wallTilemap.GetComponent<TilemapCollider2D>();
            if (collider == null)
                wallTilemap.gameObject.AddComponent<TilemapCollider2D>();

            EditorUtility.SetDirty(floorTilemap);
            EditorUtility.SetDirty(wallTilemap);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                floorTilemap.gameObject.scene);

            Debug.Log($"[RoomPainter Enhanced] Oda çizildi — {RoomW}×{RoomH}, sütunlar + dekorasyon + düşman pozisyonları");
        }

        // ─── Sütun: 2x2 duvar bloğu ────────────────────────────────────
        private static void PlacePillar(Tilemap walls, TileBase wallTile, int cx, int cy)
        {
            walls.SetTile(new Vector3Int(cx, cy, 0), wallTile);
            walls.SetTile(new Vector3Int(cx + 1, cy, 0), wallTile);
            walls.SetTile(new Vector3Int(cx, cy + 1, 0), wallTile);
            walls.SetTile(new Vector3Int(cx + 1, cy + 1, 0), wallTile);
        }

        // ─── Dekoratif obje (SpriteRenderer ile) ────────────────────────
        private static void SpawnDecoration(string name, Vector3 pos)
        {
            // Mevcut olanı sil/atla
            var existing = GameObject.Find(name);
            if (existing != null) Object.DestroyImmediate(existing);

            var go = new GameObject(name);
            go.transform.position = pos;
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sortingOrder = 15;

            // Placeholder sprite: sarı karalık (meşale hissi)
            var tex = new Texture2D(8, 16);
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 16; y++)
                {
                    float bright = Mathf.PerlinNoise(x * 0.5f, y * 0.3f);
                    if (y > 10) // alev kısmı
                        tex.SetPixel(x, y, new Color(1f, 0.6f + bright * 0.3f, 0.1f, y > 12 ? 0.8f : 0.5f));
                    else if (y > 2) // gövde
                        tex.SetPixel(x, y, new Color(0.3f, 0.2f, 0.1f, 1f));
                    else // taban
                        tex.SetPixel(x, y, new Color(0.4f, 0.35f, 0.3f, 1f));
                }
            }
            tex.filterMode = FilterMode.Point;
            tex.Apply();
            sr.sprite = Sprite.Create(tex, new Rect(0, 0, 8, 16), new Vector2(0.5f, 0), 16f);

            // Meşale ışığı
            var lightGo = new GameObject("TorchLight");
            lightGo.transform.SetParent(go.transform);
            lightGo.transform.localPosition = new Vector3(0, 0.5f, 0);
            var light = lightGo.AddComponent<UnityEngine.Rendering.Universal.Light2D>();
            light.lightType = UnityEngine.Rendering.Universal.Light2D.LightType.Point;
            light.intensity = 0.7f;
            light.pointLightOuterRadius = 3.5f;
            light.pointLightInnerRadius = 0.5f;
            light.color = new Color(1f, 0.7f, 0.3f, 1f);

            Undo.RegisterCreatedObjectUndo(go, "Create Torch");
        }

        // ─── Düşmanları stratejik pozisyonlara yerleştir ────────────────
        private static void PositionEnemies()
        {
            var positions = new (string name, Vector3 pos)[]
            {
                ("VoidThrall_Temp",   new Vector3(6,  10, 0)),  // sol üst
                ("HalfThrall_Temp",   new Vector3(18, 10, 0)),  // sağ üst
                ("Penitent_Temp",     new Vector3(12, 14, 0)),  // üst orta
                ("ChainWarden_Temp",  new Vector3(5,  5,  0)),  // sol alt
                ("RelicCaster_Temp",  new Vector3(19, 5,  0)),  // sağ alt (ranged, uzakta)
                ("FractureImp_Temp",  new Vector3(12, 4,  0)),  // alt orta
            };

            foreach (var (name, pos) in positions)
            {
                var go = GameObject.Find(name);
                if (go != null)
                {
                    go.transform.position = pos;
                    EditorUtility.SetDirty(go);
                }
            }
        }

        // ─── Tilemap bul veya oluştur ───────────────────────────────────
        private static Tilemap FindOrCreateTilemap(string name, int sortingOrder)
        {
            var go = GameObject.Find(name);
            if (go != null)
            {
                var tm = go.GetComponent<Tilemap>();
                if (tm != null) return tm;
            }

            // Grid bul
            var grids = Object.FindObjectsByType<Grid>(FindObjectsSortMode.None);
            Grid activeGrid = null;
            foreach (var g in grids)
            {
                if (g.gameObject.activeInHierarchy)
                {
                    activeGrid = g;
                    break;
                }
            }

            if (activeGrid == null)
            {
                // Yeni Grid oluştur
                var gridGo = new GameObject("Grid", typeof(Grid));
                activeGrid = gridGo.GetComponent<Grid>();
                activeGrid.cellSize = new Vector3(1, 1, 0);
                Undo.RegisterCreatedObjectUndo(gridGo, "Create Grid");
            }

            // Tilemap oluştur
            var tmGo = new GameObject(name, typeof(Tilemap), typeof(TilemapRenderer));
            tmGo.transform.SetParent(activeGrid.transform);
            var tm2 = tmGo.GetComponent<Tilemap>();
            var tmr = tmGo.GetComponent<TilemapRenderer>();
            tmr.sortingOrder = sortingOrder;
            Undo.RegisterCreatedObjectUndo(tmGo, $"Create {name}");
            return tm2;
        }

        // ─── Tile asset bul veya oluştur ────────────────────────────────
        private static TileBase GetOrCreateTile(string tilePath, string spritePath)
        {
            // Tile path klasörünü kontrol et
            string dir = System.IO.Path.GetDirectoryName(tilePath);
            if (!AssetDatabase.IsValidFolder(dir))
            {
                string parent = System.IO.Path.GetDirectoryName(dir);
                string folderName = System.IO.Path.GetFileName(dir);
                if (!AssetDatabase.IsValidFolder(parent))
                {
                    string grandParent = System.IO.Path.GetDirectoryName(parent);
                    string parentName = System.IO.Path.GetFileName(parent);
                    AssetDatabase.CreateFolder(grandParent, parentName);
                }
                AssetDatabase.CreateFolder(parent, folderName);
            }

            var tile = AssetDatabase.LoadAssetAtPath<TileBase>(tilePath);
            if (tile != null) return tile;

            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
            if (sprite == null)
            {
                Debug.LogWarning($"[RoomPainter] Sprite bulunamadı: {spritePath}, placeholder oluşturuluyor...");
                sprite = CreatePlaceholderSprite(tilePath.Contains("Crack") ? "crack" :
                                                 tilePath.Contains("Wall") ? "wall" : "floor");
            }

            var newTile = Tile.CreateInstance<Tile>();
            newTile.sprite = sprite;
            if (tilePath.Contains("Wall"))
                newTile.colliderType = Tile.ColliderType.Grid;
            AssetDatabase.CreateAsset(newTile, tilePath);
            AssetDatabase.SaveAssets();
            Debug.Log($"[RoomPainter] Tile oluşturuldu: {tilePath}");
            return newTile;
        }

        // ─── Placeholder sprite üretici ─────────────────────────────────
        private static Sprite CreatePlaceholderSprite(string type)
        {
            int size = 32;
            var tex = new Texture2D(size, size);

            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    float noise = Mathf.PerlinNoise(x * 0.15f + 50, y * 0.15f + 50);
                    Color c;

                    switch (type)
                    {
                        case "wall":
                            // Koyu taş duvar
                            float wv = 0.18f + noise * 0.12f;
                            c = new Color(wv, wv * 0.9f, wv * 0.85f, 1f);
                            // Taş doku çizgileri
                            if (y % 8 < 1 || x % 10 < 1)
                                c *= 0.7f;
                            break;

                        case "crack":
                            // Çatlak zemin
                            float cv = 0.28f + noise * 0.1f;
                            c = new Color(cv, cv * 0.95f, cv * 0.9f, 1f);
                            // Çatlak çizgisi
                            float crackLine = Mathf.Abs(Mathf.Sin(x * 0.3f + y * 0.2f) * 3f);
                            if (crackLine < 0.5f)
                                c = new Color(0.1f, 0.08f, 0.06f, 1f);
                            break;

                        default: // floor
                            // Normal taş zemin
                            float fv = 0.3f + noise * 0.1f;
                            c = new Color(fv, fv * 0.95f, fv * 0.9f, 1f);
                            // Hafif grid doku
                            if (x % 16 < 1 || y % 16 < 1)
                                c *= 0.85f;
                            break;
                    }

                    tex.SetPixel(x, y, c);
                }
            }

            tex.filterMode = FilterMode.Point;
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 32f);
        }
    }
}
