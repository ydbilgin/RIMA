using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using RIMA.Walls.V2;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace RIMA.Walls.V2.EditorTools
{
    public static class StreamGCombatProofRoom
    {
        private const string ScenePath = "Assets/Scenes/Test/PainterTestE_combat_basic.unity";
        private const string RegistryPath = "Assets/ScriptableObjects/Walls/V2/WallPieceRegistry_v1.asset";
        private const string FloorTileFolder = "Assets/ScriptableObjects/Tiles/Floor";
        private const string FloorSpriteFolder = "Assets/Sprites/AssetPackV3/floor";
        private const string OutputFolder = "STAGING/s106_overnight/stream_e_rooms/combat_basic";
        private const string ReferenceImage = "STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (2).png";

        // [MenuItem removed — Stream G workflow archived]
        public static void RunFromMenu()
        {
            RunFromCommandLine();
        }

        public static void RunFromCommandLine()
        {
            var started = DateTime.Now;
            bool refreshNeeded = false;
            StreamGMetrics metrics = new StreamGMetrics();
            try
            {
                AssetDatabase.StartAssetEditing();
                try
                {
                    EnsureFolder(FloorTileFolder);
                    EnsureFloorTileAssets();
                }
                finally
                {
                    AssetDatabase.StopAssetEditing();
                }

                Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
                WallPieceRegistry registry = AssetDatabase.LoadAssetAtPath<WallPieceRegistry>(RegistryPath);
                if (registry == null)
                    throw new InvalidOperationException("Missing registry: " + RegistryPath);

                GameObject root = GameObject.Find("PaintedRoom_combat_basic");
                if (root == null)
                    root = new GameObject("PaintedRoom_combat_basic");

                Transform walls = root.transform.Find("Walls");
                if (walls == null)
                {
                    var wallsGo = new GameObject("Walls");
                    wallsGo.transform.SetParent(root.transform, false);
                    walls = wallsGo.transform;
                }

                WallChainRoomBuilder builder = root.GetComponent<WallChainRoomBuilder>();
                if (builder == null)
                    builder = root.AddComponent<WallChainRoomBuilder>();

                builder.registry = registry;
                builder.roomParent = walls;
                builder.cellSize = 1f;
                builder.preferRealAssets = true;

                metrics.wallPiecesBefore = CountWallPieces(walls);
                RoomSpec spec = BuildCombatSpec();
                builder.Build(spec);
                ApplyWallSorting(walls);
                metrics.wallPiecesAfter = CountWallPieces(walls);
                metrics.realPiecesAfter = CountRealWallPieces(walls);
                VerifyRearWallBounds(registry, ref metrics);

                Tilemap floor = EnsureFloorTilemap(root.transform);
                metrics.floorTilesBefore = CountTiles(floor);
                PaintFloor(floor);
                metrics.floorTilesAfter = CountTiles(floor);

                Camera camera = EnsureCamera();
                CaptureCamera(camera, ProjectPath(OutputFolder + "/scene_v3.png"));
                CaptureGizmoOverlay(camera, walls, ProjectPath(OutputFolder + "/gizmo_v3.png"));
                WriteComparison(ProjectPath(ReferenceImage), ProjectPath(OutputFolder + "/scene_v3.png"), ProjectPath(OutputFolder + "/comparison_v3.png"));

                EditorUtility.SetDirty(builder);
                EditorUtility.SetDirty(root);
                EditorSceneManager.MarkSceneDirty(scene);
                AssetDatabase.SaveAssets();
                EditorSceneManager.SaveScene(scene);
                refreshNeeded = true;

                metrics.elapsedSeconds = (DateTime.Now - started).TotalSeconds;
                WriteMetrics(metrics);
                Debug.Log($"[StreamG] Combat proof room rebuilt. walls {metrics.wallPiecesBefore}->{metrics.wallPiecesAfter}, floor {metrics.floorTilesBefore}->{metrics.floorTilesAfter}");
            }
            catch (Exception ex)
            {
                Debug.LogError("[StreamG] Combat proof room failed: " + ex);
                throw;
            }
            finally
            {
                if (refreshNeeded)
                    AssetDatabase.Refresh();
            }
        }

        private static RoomSpec BuildCombatSpec()
        {
            var walkable = new List<Vector2Int>();
            for (int y = 5; y <= 16; y++)
                for (int x = 4; x <= 17; x++)
                    walkable.Add(new Vector2Int(x, y));

            return new RoomSpec
            {
                roomName = "combat_basic",
                widthCells = 18,
                heightCells = 17,
                shapeType = RoomShapeType.Irregular,
                rearWallEnabled = true,
                sideWallsEnabled = true,
                frontEdgeMode = FrontEdgeMode.LowWall,
                doorPosition = new Vector2Int(10, 16),
                enforceCenteredRearDoor = true,
                frontMinOpeningCells = 3,
                walkableCells = walkable,
                sockets = new List<RoomSocket>
                {
                    new RoomSocket { cell = new Vector2Int(6, 7), type = SocketType.EnemyMelee, metadata = "inner corner SW" },
                    new RoomSocket { cell = new Vector2Int(15, 7), type = SocketType.EnemyMelee, metadata = "inner corner SE" },
                    new RoomSocket { cell = new Vector2Int(6, 14), type = SocketType.EnemyMelee, metadata = "inner corner NW" },
                    new RoomSocket { cell = new Vector2Int(15, 14), type = SocketType.EnemyMelee, metadata = "inner corner NE" },
                    new RoomSocket { cell = new Vector2Int(8, 16), type = SocketType.Torch, metadata = "rear left" },
                    new RoomSocket { cell = new Vector2Int(13, 16), type = SocketType.Torch, metadata = "rear right" }
                }
            };
        }

        private static void EnsureFloorTileAssets()
        {
            for (int i = 0; i < 16; i++)
            {
                string tilePath = $"{FloorTileFolder}/tile_{i}.asset";
                string spritePath = $"{FloorSpriteFolder}/tile_{i}.png";
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                if (sprite == null)
                    throw new InvalidOperationException("Missing floor sprite: " + spritePath);

                Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(tilePath);
                if (tile == null)
                {
                    tile = ScriptableObject.CreateInstance<Tile>();
                    AssetDatabase.CreateAsset(tile, tilePath);
                }

                tile.sprite = sprite;
                tile.colliderType = Tile.ColliderType.None;
                EditorUtility.SetDirty(tile);
            }
        }

        private static Tilemap EnsureFloorTilemap(Transform root)
        {
            Transform floorRoot = root.Find("Floor");
            if (floorRoot == null)
            {
                var go = new GameObject("Floor");
                go.transform.SetParent(root, false);
                go.AddComponent<Grid>();
                floorRoot = go.transform;
            }

            floorRoot.localPosition = new Vector3(0f, 0f, 1f);
            if (floorRoot.GetComponent<Grid>() == null)
                floorRoot.gameObject.AddComponent<Grid>();

            Transform tilemapTransform = floorRoot.Find("Tilemap");
            if (tilemapTransform == null)
            {
                var go = new GameObject("Tilemap");
                go.transform.SetParent(floorRoot, false);
                tilemapTransform = go.transform;
            }

            Tilemap tilemap = tilemapTransform.GetComponent<Tilemap>();
            if (tilemap == null)
                tilemap = tilemapTransform.gameObject.AddComponent<Tilemap>();
            TilemapRenderer renderer = tilemapTransform.GetComponent<TilemapRenderer>();
            if (renderer == null)
                renderer = tilemapTransform.gameObject.AddComponent<TilemapRenderer>();

            tilemapTransform.localPosition = Vector3.zero;
            tilemapTransform.localScale = Vector3.one;
            renderer.sortingLayerName = "Default";
            renderer.sortingOrder = -100;
            tilemap.color = Color.white;
            return tilemap;
        }

        private static void PaintFloor(Tilemap tilemap)
        {
            tilemap.ClearAllTiles();
            Tile[] tiles = new Tile[16];
            for (int i = 0; i < tiles.Length; i++)
                tiles[i] = AssetDatabase.LoadAssetAtPath<Tile>($"{FloorTileFolder}/tile_{i}.asset");

            for (int y = 5; y <= 16; y++)
            {
                for (int x = 4; x <= 17; x++)
                {
                    Tile tile = tiles[Mathf.Abs((x * 17 + y * 31) % tiles.Length)];
                    Vector3Int cell = new Vector3Int(x, y, 0);
                    tilemap.SetTile(cell, tile);
                    tilemap.SetTileFlags(cell, TileFlags.None);
                    tilemap.SetTransformMatrix(cell, TileScaleMatrix(tile));
                }
            }
            tilemap.CompressBounds();
            EditorUtility.SetDirty(tilemap);
        }

        private static Matrix4x4 TileScaleMatrix(Tile tile)
        {
            if (tile == null || tile.sprite == null)
                return Matrix4x4.identity;

            Vector3 size = tile.sprite.bounds.size;
            float sx = size.x > 0.0001f ? 1f / size.x : 1f;
            float sy = size.y > 0.0001f ? 1f / size.y : 1f;
            return Matrix4x4.Scale(new Vector3(sx, sy, 1f));
        }

        private static void ApplyWallSorting(Transform walls)
        {
            foreach (SpriteRenderer renderer in walls.GetComponentsInChildren<SpriteRenderer>(true))
            {
                renderer.sortingLayerName = "Walls";
                renderer.sortingOrder = 200 - Mathf.RoundToInt(renderer.transform.position.y * 10f);
                EditorUtility.SetDirty(renderer);
            }
        }

        private static Camera EnsureCamera()
        {
            Camera camera = Camera.main;
            if (camera == null)
                camera = Object.FindFirstObjectByType<Camera>();
            if (camera == null)
            {
                var go = new GameObject("Main Camera");
                go.tag = "MainCamera";
                camera = go.AddComponent<Camera>();
            }

            camera.orthographic = true;
            camera.orthographicSize = 9.5f;
            camera.transform.position = new Vector3(10.5f, 10.8f, -10f);
            camera.transform.rotation = Quaternion.identity;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.025f, 0.028f, 0.035f, 1f);
            EditorUtility.SetDirty(camera);
            return camera;
        }

        private static void CaptureCamera(Camera camera, string absolutePath)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath));
            var rt = new RenderTexture(1600, 900, 24, RenderTextureFormat.ARGB32);
            RenderTexture previousActive = RenderTexture.active;
            RenderTexture previousCamera = camera.targetTexture;
            try
            {
                camera.targetTexture = rt;
                RenderTexture.active = rt;
                camera.Render();
                var image = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
                image.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                image.Apply();
                File.WriteAllBytes(absolutePath, image.EncodeToPNG());
                Object.DestroyImmediate(image);
            }
            finally
            {
                camera.targetTexture = previousCamera;
                RenderTexture.active = previousActive;
                Object.DestroyImmediate(rt);
            }
        }

        private static void CaptureGizmoOverlay(Camera camera, Transform walls, string absolutePath)
        {
            CaptureCamera(camera, absolutePath);
            Texture2D image = LoadTexture(absolutePath);
            Color floorLine = new Color(0.0f, 0.8f, 1f, 0.85f);
            Color wallLine = new Color(1f, 0.9f, 0.15f, 0.9f);

            for (int y = 5; y <= 16; y++)
                for (int x = 4; x <= 17; x++)
                    DrawWorldRect(image, camera, new Rect(x, y, 1f, 1f), floorLine);

            foreach (WallPiece piece in walls.GetComponentsInChildren<WallPiece>(true))
            {
                if (piece == null || piece.data == null) continue;
                Vector3 pos = piece.transform.position + new Vector3(piece.data.anchorOffset.x, piece.data.anchorOffset.y, 0f);
                Rect rect = new Rect(
                    pos.x - piece.data.footprintSize.x * 0.5f,
                    pos.y - piece.data.footprintSize.y * 0.5f,
                    piece.data.footprintSize.x,
                    piece.data.footprintSize.y);
                DrawWorldRect(image, camera, rect, wallLine);
            }

            image.Apply();
            File.WriteAllBytes(absolutePath, image.EncodeToPNG());
            Object.DestroyImmediate(image);
        }

        private static void WriteComparison(string refPath, string scenePath, string outputPath)
        {
            Texture2D reference = LoadTexture(refPath);
            Texture2D scene = LoadTexture(scenePath);
            Texture2D left = ResizeCrop(reference, 800, 900);
            Texture2D right = ResizeCrop(scene, 800, 900);
            var comparison = new Texture2D(1600, 900, TextureFormat.RGBA32, false);

            comparison.SetPixels(0, 0, 800, 900, left.GetPixels());
            comparison.SetPixels(800, 0, 800, 900, right.GetPixels());
            comparison.Apply();
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            File.WriteAllBytes(outputPath, comparison.EncodeToPNG());

            Object.DestroyImmediate(reference);
            Object.DestroyImmediate(scene);
            Object.DestroyImmediate(left);
            Object.DestroyImmediate(right);
            Object.DestroyImmediate(comparison);
        }

        private static Texture2D ResizeCrop(Texture2D source, int width, int height)
        {
            float sourceAspect = source.width / (float)source.height;
            float targetAspect = width / (float)height;
            int cropW = source.width;
            int cropH = source.height;
            int cropX = 0;
            int cropY = 0;

            if (sourceAspect > targetAspect)
            {
                cropW = Mathf.RoundToInt(source.height * targetAspect);
                cropX = Mathf.Max(0, (source.width - cropW) / 2);
            }
            else
            {
                cropH = Mathf.RoundToInt(source.width / targetAspect);
                cropY = Mathf.Max(0, (source.height - cropH) / 2);
            }

            var target = new Texture2D(width, height, TextureFormat.RGBA32, false);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float u = cropX + (x + 0.5f) / width * cropW;
                    float v = cropY + (y + 0.5f) / height * cropH;
                    target.SetPixel(x, y, source.GetPixelBilinear(u / source.width, v / source.height));
                }
            }
            target.Apply();
            return target;
        }

        private static Texture2D LoadTexture(string absolutePath)
        {
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (!texture.LoadImage(File.ReadAllBytes(absolutePath)))
                throw new InvalidOperationException("Could not load image: " + absolutePath);
            return texture;
        }

        private static void DrawWorldRect(Texture2D image, Camera camera, Rect rect, Color color)
        {
            Vector2 a = WorldToPixel(image, camera, new Vector3(rect.xMin, rect.yMin, 0f));
            Vector2 b = WorldToPixel(image, camera, new Vector3(rect.xMax, rect.yMin, 0f));
            Vector2 c = WorldToPixel(image, camera, new Vector3(rect.xMax, rect.yMax, 0f));
            Vector2 d = WorldToPixel(image, camera, new Vector3(rect.xMin, rect.yMax, 0f));
            DrawLine(image, a, b, color);
            DrawLine(image, b, c, color);
            DrawLine(image, c, d, color);
            DrawLine(image, d, a, color);
        }

        private static Vector2 WorldToPixel(Texture2D image, Camera camera, Vector3 world)
        {
            Vector3 vp = camera.WorldToViewportPoint(world);
            return new Vector2(vp.x * (image.width - 1), vp.y * (image.height - 1));
        }

        private static void DrawLine(Texture2D image, Vector2 from, Vector2 to, Color color)
        {
            int x0 = Mathf.RoundToInt(from.x);
            int y0 = Mathf.RoundToInt(from.y);
            int x1 = Mathf.RoundToInt(to.x);
            int y1 = Mathf.RoundToInt(to.y);
            int dx = Mathf.Abs(x1 - x0);
            int dy = Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                BlendPixel(image, x0, y0, color);
                if (x0 == x1 && y0 == y1) break;
                int e2 = err * 2;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }

        private static void BlendPixel(Texture2D image, int x, int y, Color color)
        {
            if (x < 0 || y < 0 || x >= image.width || y >= image.height) return;
            Color original = image.GetPixel(x, y);
            image.SetPixel(x, y, Color.Lerp(original, color, color.a));
        }

        private static int CountWallPieces(Transform walls)
        {
            return walls == null ? 0 : walls.GetComponentsInChildren<WallPiece>(true).Length;
        }

        private static int CountRealWallPieces(Transform walls)
        {
            int count = 0;
            if (walls == null) return 0;
            foreach (WallPiece piece in walls.GetComponentsInChildren<WallPiece>(true))
            {
                if (piece != null && piece.data != null && !string.IsNullOrEmpty(piece.data.id) && piece.data.id.EndsWith("_real", StringComparison.Ordinal))
                    count++;
            }
            return count;
        }

        private static int CountTiles(Tilemap tilemap)
        {
            if (tilemap == null) return 0;
            int count = 0;
            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
                if (tilemap.HasTile(pos)) count++;
            return count;
        }

        private static void WriteMetrics(StreamGMetrics metrics)
        {
            string path = ProjectPath(OutputFolder + "/stream_g_metrics.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            File.WriteAllText(path,
                "wallPiecesBefore=" + metrics.wallPiecesBefore + System.Environment.NewLine +
                "wallPiecesAfter=" + metrics.wallPiecesAfter + System.Environment.NewLine +
                "realPiecesAfter=" + metrics.realPiecesAfter + System.Environment.NewLine +
                "floorTilesBefore=" + metrics.floorTilesBefore + System.Environment.NewLine +
                "floorTilesAfter=" + metrics.floorTilesAfter + System.Environment.NewLine +
                "rearWallBoundsX=" + metrics.rearWallBoundsX.ToString("0.000", CultureInfo.InvariantCulture) + System.Environment.NewLine +
                "rearWallBoundsY=" + metrics.rearWallBoundsY.ToString("0.000", CultureInfo.InvariantCulture) + System.Environment.NewLine +
                "elapsedSeconds=" + metrics.elapsedSeconds.ToString("0.0", CultureInfo.InvariantCulture) + System.Environment.NewLine);
        }

        private static void VerifyRearWallBounds(WallPieceRegistry registry, ref StreamGMetrics metrics)
        {
            WallPieceData data = registry.GetById("rear_wall_2x_real");
            if (data == null || data.prefab == null)
                throw new InvalidOperationException("Missing rear_wall_2x_real data or prefab.");

            GameObject go = (GameObject)PrefabUtility.InstantiatePrefab(data.prefab);
            try
            {
                go.name = "StreamG_RearWallBoundsProbe";
                go.transform.position = Vector3.zero;
                WallPiece piece = go.GetComponent<WallPiece>();
                if (piece == null)
                    throw new InvalidOperationException("rear_wall_2x_real prefab has no WallPiece component.");
                piece.Initialize(data);
                SpriteRenderer renderer = go.GetComponentInChildren<SpriteRenderer>();
                if (renderer == null)
                    throw new InvalidOperationException("rear_wall_2x_real prefab has no SpriteRenderer.");
                metrics.rearWallBoundsX = renderer.bounds.size.x;
                metrics.rearWallBoundsY = renderer.bounds.size.y;
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        private static void EnsureFolder(string assetFolder)
        {
            if (AssetDatabase.IsValidFolder(assetFolder)) return;
            string[] parts = assetFolder.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }

        private static string ProjectPath(string relativePath)
        {
            return Path.GetFullPath(Path.Combine(Directory.GetParent(Application.dataPath).FullName, relativePath.Replace('/', Path.DirectorySeparatorChar)));
        }

        private struct StreamGMetrics
        {
            public int wallPiecesBefore;
            public int wallPiecesAfter;
            public int realPiecesAfter;
            public int floorTilesBefore;
            public int floorTilesAfter;
            public float rearWallBoundsX;
            public float rearWallBoundsY;
            public double elapsedSeconds;
        }
    }
}
