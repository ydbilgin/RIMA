#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

namespace RIMA.MapDesigner.Editor.Blueprint
{
    public static class RimaV15hPlayableComposer
    {
        public const string ScenePath = RimaV15gMinimalComposer.ScenePath;
        public const string ProfilePath = "Assets/Data/Blueprint/Profiles/profile_v15h_playable.asset";
        public const string RoomPath = RimaV15gMinimalComposer.RoomPath;
        public const string WangFolder = "Assets/Data/Brush/AssetParts_v3/CombatBiome_v15h/Wang";
        public const string WangSheetPath = WangFolder + "/wang_dirt_cobble_32px.png";
        public const string WangRuleTilePath = WangFolder + "/rule_dirt_cobble_wang.asset";
        public const string V15hRootName = "Pro_Redesign_v15h_Playable_CombatRoom";
        public const string MetricsPath = "STAGING/BUILD_DONE_v15h_playable_map_metrics.txt";
        public const string ScreenshotPath = "Assets/Screenshots/PlayableRoom_combat_v15h_playable_LIVE.png";
        public const string SideBySidePath = "Assets/Screenshots/PlayableRoom_combat_v15g_vs_v15h.png";
        public const string DoneMarkerPath = "STAGING/BUILD_TASK_v15h_playable_map_DONE.md";

        private const string SourceWangSheetPath = "STAGING/pixellab_wang_v2_32px/wang_dirt_cobble_32px.png";
        private const string ZoneFolder = "Assets/Data/Blueprint/ZoneTypes/v15h";
        private const string WarbladeSpritePath = "Assets/Sprites/Characters/Anchors/01_warblade.png";
        private const string PlayerPrefabPath = "Assets/Prefabs/Player.prefab";
        private const int WangGridSize = 4;
        private const int WangTileSize = 32;
        private const int WangTileCount = 16;
        private const int DontCare = 0;

        private static readonly string[] RootsToDisable =
        {
            "Pro_Redesign_v15_BlueprintFirst_CombatRoom",
            "Pro_Redesign_v15b_FullAdjacency_CombatRoom",
            "Pro_Redesign_v15c_8LayerPainted_CombatRoom",
            "Pro_Redesign_v15d_Composition_CombatRoom",
            "Pro_Redesign_v15e_A_L8cap_CombatRoom",
            "Pro_Redesign_v15g_Minimal_PixelLab_CombatRoom",
            "PlayableRoom",
            "TransitionBrushLayer_Child",
            "DetailDecalLayer_Child",
            "AccentLayer_Child",
            "WallOverlay_Child"
        };

        public static void Build()
        {
            try
            {
                EnsureV15hAssetGraph();

                Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
                if (!scene.IsValid())
                {
                    throw new InvalidOperationException($"Failed to open scene: {ScenePath}");
                }

                BlueprintProfileSO profile = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(ProfilePath);
                RoomBlueprintSO room = AssetDatabase.LoadAssetAtPath<RoomBlueprintSO>(RoomPath);
                if (profile == null || room == null)
                {
                    throw new InvalidOperationException("Missing v15h profile or v15b room blueprint asset.");
                }

                Vector3 rootPosition = DisableHistoricalRoots();
                DestroyIfPresent(V15hRootName);
                DestroyIfPresent("Warblade_v15h_Player");

                var v15hRoot = new GameObject(V15hRootName);
                v15hRoot.transform.position = rootPosition;

                BlueprintCanvas canvas = CreatePlayableCanvas(room.ToCanvas());
                int zonePlaced = AutoPopulator.PopulateZones(canvas, profile, v15hRoot.transform, 2026);
                int adjacencyPlaced = AutoPopulator.PopulateAdjacency(canvas, profile, v15hRoot.transform, 2026);
                int wangPlaced = PaintWangTransitions(canvas, profile, v15hRoot.transform);
                GameObject player = SpawnWarblade(v15hRoot.transform, canvas);

                Metrics metrics = CollectMetrics(v15hRoot.transform, canvas.Count, zonePlaced, adjacencyPlaced, wangPlaced, player, profile);
                AutoPopulator.CompositionReport compositionReport = AutoPopulator.LastCompositionReport;

                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);

                CaptureScreenshot(v15hRoot.transform);
                CaptureSideBySide();
                WriteMetrics(metrics, compositionReport);
                WriteDoneMarker(metrics, compositionReport);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                ExitIfBatchMode(0);
            }
            catch (Exception ex)
            {
                Directory.CreateDirectory("STAGING");
                File.WriteAllText(MetricsPath, "FAILED\n" + ex);
                Debug.LogException(ex);
                ExitIfBatchMode(1);
            }
        }

        public static void EnsureV15hAssetGraph()
        {
            RimaV15gMinimalComposer.EnsureV15gAssetGraph();
            ImportWangSheetAndTiles();
            UpdateV15gDensityAssets();
            CreateOrUpdateProfile();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static string WangTilePath(int index)
        {
            return $"{WangFolder}/wang_dirt_cobble_32px_tile_{index:00}.asset";
        }

        public static BlueprintCanvas CreatePlayableCanvas(BlueprintCanvas source)
        {
            if (source == null)
            {
                return new BlueprintCanvas();
            }

            var canvas = new BlueprintCanvas(source.GridSize);
            foreach (KeyValuePair<Vector2Int, string> pair in source.IntentMap)
            {
                canvas.Paint(pair.Key, RemapZone(pair.Value), 1);
            }

            return canvas;
        }

        public static string BuildPreviewMetricsText()
        {
            EnsureV15hAssetGraph();
            BlueprintProfileSO profile = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(ProfilePath);
            RoomBlueprintSO room = AssetDatabase.LoadAssetAtPath<RoomBlueprintSO>(RoomPath);
            if (profile == null || room == null)
            {
                throw new InvalidOperationException("Missing v15h profile or v15b room blueprint asset.");
            }

            var previewRoot = new GameObject("V15hMetricsPreviewRoot");
            try
            {
                BlueprintCanvas canvas = CreatePlayableCanvas(room.ToCanvas());
                int zonePlaced = AutoPopulator.PopulateZones(canvas, profile, previewRoot.transform, 2026);
                int adjacencyPlaced = AutoPopulator.PopulateAdjacency(canvas, profile, previewRoot.transform, 2026);
                int wangPlaced = PaintWangTransitions(canvas, profile, previewRoot.transform);
                Metrics metrics = CollectMetrics(previewRoot.transform, canvas.Count, zonePlaced, adjacencyPlaced, wangPlaced, null, profile);
                return MetricsText(metrics, AutoPopulator.LastCompositionReport);
            }
            finally
            {
                Object.DestroyImmediate(previewRoot);
            }
        }

        private static void ImportWangSheetAndTiles()
        {
            Directory.CreateDirectory(WangFolder);
            if (!File.Exists(SourceWangSheetPath))
            {
                throw new FileNotFoundException("Missing Wang v2 source PNG.", SourceWangSheetPath);
            }

            File.Copy(SourceWangSheetPath, WangSheetPath, true);
            AssetDatabase.ImportAsset(WangSheetPath, ImportAssetOptions.ForceUpdate);
            ConfigureWangImporter(WangSheetPath);

            Sprite[] sprites = LoadWangSprites();
            Tile[] tiles = new Tile[WangTileCount];
            for (int i = 0; i < WangTileCount; i++)
            {
                Tile tile = ScriptableObject.CreateInstance<Tile>();
                tile.name = $"wang_dirt_cobble_32px_tile_{i:00}";
                tile.sprite = sprites[i];
                tile.colliderType = Tile.ColliderType.None;
                ReplaceAsset(tile, WangTilePath(i));
                tiles[i] = tile;
            }

            RuleTile ruleTile = CreateRuleTile(sprites);
            ReplaceAsset(ruleTile, WangRuleTilePath);
        }

        private static void ConfigureWangImporter(string texturePath)
        {
            TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
            if (importer == null)
            {
                throw new IOException($"TextureImporter not found for {texturePath}.");
            }

            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.spritePixelsPerUnit = WangTileSize;
            importer.filterMode = FilterMode.Point;
            importer.textureCompression = TextureImporterCompression.Uncompressed;
            importer.mipmapEnabled = false;
            importer.alphaIsTransparency = true;
            importer.npotScale = TextureImporterNPOTScale.None;
            importer.spritesheet = BuildWangSpriteMetadata(Path.GetFileNameWithoutExtension(texturePath));
            importer.SaveAndReimport();
        }

        private static SpriteMetaData[] BuildWangSpriteMetadata(string baseName)
        {
            var metadata = new SpriteMetaData[WangTileCount];
            for (int row = 0; row < WangGridSize; row++)
            {
                for (int column = 0; column < WangGridSize; column++)
                {
                    int index = row * WangGridSize + column;
                    metadata[index] = new SpriteMetaData
                    {
                        name = $"{baseName}_wang_{index:00}",
                        alignment = (int)SpriteAlignment.Center,
                        pivot = new Vector2(0.5f, 0.5f),
                        rect = new Rect(column * WangTileSize, (WangGridSize - 1 - row) * WangTileSize, WangTileSize, WangTileSize)
                    };
                }
            }

            return metadata;
        }

        private static Sprite[] LoadWangSprites()
        {
            Sprite[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(WangSheetPath)
                .OfType<Sprite>()
                .OrderByDescending(sprite => sprite.rect.y)
                .ThenBy(sprite => sprite.rect.x)
                .ToArray();

            if (sprites.Length != WangTileCount)
            {
                throw new InvalidDataException($"Expected {WangTileCount} Wang sprites, found {sprites.Length}.");
            }

            return sprites;
        }

        private static RuleTile CreateRuleTile(Sprite[] sprites)
        {
            var ruleTile = ScriptableObject.CreateInstance<RuleTile>();
            ruleTile.name = Path.GetFileNameWithoutExtension(WangRuleTilePath);
            ruleTile.m_DefaultSprite = sprites[0];
            ruleTile.m_DefaultColliderType = Tile.ColliderType.None;
            ruleTile.m_TilingRules = new List<RuleTile.TilingRule>();

            int[] keys = Enumerable.Range(0, WangTileCount)
                .OrderByDescending(SpecificityForKey)
                .ThenBy(key => key)
                .ToArray();

            for (int i = 0; i < keys.Length; i++)
            {
                int key = keys[i];
                var rule = new RuleTile.TilingRule
                {
                    m_Output = RuleTile.TilingRuleOutput.OutputSprite.Single,
                    m_Sprites = new[] { sprites[key] },
                    m_ColliderType = Tile.ColliderType.None,
                    m_RuleTransform = RuleTile.TilingRuleOutput.Transform.Fixed,
                    m_NeighborPositions = new List<Vector3Int>
                    {
                        new Vector3Int(0, 1, 0),
                        new Vector3Int(1, 1, 0),
                        new Vector3Int(1, 0, 0),
                        new Vector3Int(1, -1, 0),
                        new Vector3Int(0, -1, 0),
                        new Vector3Int(-1, -1, 0),
                        new Vector3Int(-1, 0, 0),
                        new Vector3Int(-1, 1, 0)
                    },
                    m_Neighbors = BuildCornerNeighborMask(key)
                };
                ruleTile.m_TilingRules.Add(rule);
            }

            return ruleTile;
        }

        private static List<int> BuildCornerNeighborMask(int key)
        {
            bool nw = (key & 8) != 0;
            bool ne = (key & 4) != 0;
            bool sw = (key & 2) != 0;
            bool se = (key & 1) != 0;

            return new List<int>
            {
                SideMatch(nw, ne),
                CornerMatch(ne),
                SideMatch(ne, se),
                CornerMatch(se),
                SideMatch(sw, se),
                CornerMatch(sw),
                SideMatch(nw, sw),
                CornerMatch(nw)
            };
        }

        private static int SpecificityForKey(int key)
        {
            return BuildCornerNeighborMask(key).Count(value => value != DontCare);
        }

        private static int SideMatch(bool a, bool b)
        {
            if (a && b)
            {
                return RuleTile.TilingRuleOutput.Neighbor.This;
            }

            if (!a && !b)
            {
                return RuleTile.TilingRuleOutput.Neighbor.NotThis;
            }

            return DontCare;
        }

        private static int CornerMatch(bool value)
        {
            return value ? RuleTile.TilingRuleOutput.Neighbor.This : RuleTile.TilingRuleOutput.Neighbor.NotThis;
        }

        private static void UpdateV15gDensityAssets()
        {
            SetZoneDefaultDensity("Assets/Data/Blueprint/ZoneTypes/v15g/zone_stone_v15g.asset", 0.85f);
            SetZoneDefaultDensity("Assets/Data/Blueprint/ZoneTypes/v15g/zone_dirt_v15g.asset", 0.85f);
        }

        private static void CreateOrUpdateProfile()
        {
            Directory.CreateDirectory(ZoneFolder);
            Directory.CreateDirectory(Path.GetDirectoryName(ProfilePath));

            BlueprintProfileSO v15g = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(RimaV15gMinimalComposer.ProfilePath);
            if (v15g == null || v15g.zones == null)
            {
                throw new InvalidOperationException("Missing v15g source profile.");
            }

            var zones = new List<BlueprintZoneTypeSO>();
            for (int i = 0; i < v15g.zones.Length; i++)
            {
                BlueprintZoneTypeSO source = v15g.zones[i];
                if (source == null)
                {
                    continue;
                }

                zones.Add(CreateOrUpdateV15hZone(source));
            }

            BlueprintProfileSO profile = LoadOrCreateAsset<BlueprintProfileSO>(ProfilePath);
            ClearSubAssets(ProfilePath, profile);
            profile.profileId = "profile_v15h_playable";
            profile.gridSize = v15g.gridSize;
            profile.zones = zones.ToArray();
            profile.wangRuleTileRef = AssetDatabase.LoadAssetAtPath<TileBase>(WangRuleTilePath);

            var rules = new List<BlueprintAdjacencyRuleSO>();
            if (v15g.adjacencyRules != null)
            {
                for (int i = 0; i < v15g.adjacencyRules.Length; i++)
                {
                    BlueprintAdjacencyRuleSO source = v15g.adjacencyRules[i];
                    if (source == null)
                    {
                        continue;
                    }

                    BlueprintAdjacencyRuleSO rule = ScriptableObject.CreateInstance<BlueprintAdjacencyRuleSO>();
                    rule.name = source.ruleId.Replace("v15g", "v15h");
                    rule.ruleId = rule.name;
                    rule.zoneIdA = source.zoneIdA;
                    rule.zoneIdB = source.zoneIdB;
                    rule.transitionPool = source.transitionPool;
                    rule.density = source.density;
                    rule.decalsPerRoomCap = 8;
                    AssetDatabase.AddObjectToAsset(rule, profile);
                    rules.Add(rule);
                }
            }

            profile.adjacencyRules = rules.ToArray();
            EditorUtility.SetDirty(profile);
        }

        private static BlueprintZoneTypeSO CreateOrUpdateV15hZone(BlueprintZoneTypeSO source)
        {
            string path = $"{ZoneFolder}/zone_{source.zoneId}_v15h.asset";
            BlueprintZoneTypeSO zone = LoadOrCreateAsset<BlueprintZoneTypeSO>(path);
            zone.zoneId = source.zoneId;
            zone.displayName = source.displayName.Replace("v15g", "v15h");
            zone.brushColor = source.brushColor;
            zone.defaultDensity = 0.85f;
            zone.maxFeatureProps = source.maxFeatureProps;
            zone.negativeSpaceRatio = 0.15f;
            zone.floorWeights = new Vector3(0.85f, 0.10f, 0.05f);
            zone.dominantFloorPool = source.dominantFloorPool;
            zone.secondaryFloorPool = source.secondaryFloorPool;
            zone.accentFloorPool = source.accentFloorPool;
            zone.pathProtect = false;
            zone.heroPropClusterCap = source.heroPropClusterCap;
            zone.heroPropClusterSize = source.heroPropClusterSize;
            zone.heroPropClusterBuffer = source.heroPropClusterBuffer;
            zone.secondaryClusterCap = source.secondaryClusterCap;
            zone.pathCellRatio = 0f;
            zone.pathMinWidth = source.pathMinWidth;
            zone.atmosphericCap = 0;
            zone.macroFillSprites = source.macroFillSprites;
            zone.baseFloorSprites = source.baseFloorSprites;
            zone.midToneOverlayPool = source.midToneOverlayPool;
            zone.midToneDensity = source.midToneDensity;
            zone.detailTexturePool = source.detailTexturePool;
            zone.detailDensity = source.detailDensity;
            zone.smallScatterPool = source.smallScatterPool;
            zone.smallScatterDensity = source.smallScatterDensity;
            zone.mediumPropPool = source.mediumPropPool;
            zone.mediumPropDensity = source.mediumPropDensity;
            zone.tallFocalPool = source.tallFocalPool;
            zone.maxTallFocalPerRegion = source.maxTallFocalPerRegion;
            zone.atmosphericPool = source.atmosphericPool;
            zone.atmosphericDensity = 0f;
            EditorUtility.SetDirty(zone);
            return zone;
        }

        private static int PaintWangTransitions(BlueprintCanvas canvas, BlueprintProfileSO profile, Transform roomRoot)
        {
            if (canvas == null || profile == null || profile.wangRuleTileRef == null || roomRoot == null)
            {
                return 0;
            }

            TileBase[] tiles = new TileBase[WangTileCount];
            for (int i = 0; i < WangTileCount; i++)
            {
                tiles[i] = AssetDatabase.LoadAssetAtPath<TileBase>(WangTilePath(i));
            }

            GameObject gridObject = new GameObject("Wang_DirtCobble_Grid");
            gridObject.transform.SetParent(roomRoot, false);
            Grid grid = gridObject.AddComponent<Grid>();
            grid.cellSize = Vector3.one;

            GameObject tilemapObject = new GameObject("Wang_DirtCobble_Transitions");
            tilemapObject.transform.SetParent(gridObject.transform, false);
            Tilemap tilemap = tilemapObject.AddComponent<Tilemap>();
            TilemapRenderer renderer = tilemapObject.AddComponent<TilemapRenderer>();
            renderer.sortingOrder = -85;

            int[,] vertices = BuildCobbleVertexGrid(canvas);
            int placed = 0;
            foreach (KeyValuePair<Vector2Int, string> pair in canvas.IntentMap)
            {
                Vector2Int cell = pair.Key;
                int key = ((vertices[cell.x, cell.y + 1] != 0 ? 1 : 0) << 3)
                    | ((vertices[cell.x + 1, cell.y + 1] != 0 ? 1 : 0) << 2)
                    | ((vertices[cell.x, cell.y] != 0 ? 1 : 0) << 1)
                    | (vertices[cell.x + 1, cell.y] != 0 ? 1 : 0);

                if (key <= 0 || key >= 15 || tiles[key] == null)
                {
                    continue;
                }

                tilemap.SetTile(new Vector3Int(cell.x, cell.y, 0), tiles[key]);
                placed++;
            }

            tilemap.RefreshAllTiles();
            return placed;
        }

        private static int[,] BuildCobbleVertexGrid(BlueprintCanvas canvas)
        {
            Vector2Int size = canvas.GridSize;
            int[,] vertices = new int[size.x + 1, size.y + 1];
            for (int y = 0; y <= size.y; y++)
            {
                for (int x = 0; x <= size.x; x++)
                {
                    int votes = 0;
                    int cobbleVotes = 0;
                    for (int dy = -1; dy <= 0; dy++)
                    {
                        for (int dx = -1; dx <= 0; dx++)
                        {
                            var cell = new Vector2Int(x + dx, y + dy);
                            string zone = canvas.GetZoneAt(cell);
                            if (string.IsNullOrEmpty(zone))
                            {
                                continue;
                            }

                            votes++;
                            if (IsCobbleZone(zone))
                            {
                                cobbleVotes++;
                            }
                        }
                    }

                    vertices[x, y] = votes > 0 && cobbleVotes * 2 >= votes ? 1 : 0;
                }
            }

            return vertices;
        }

        private static bool IsCobbleZone(string zoneId)
        {
            return string.Equals(zoneId, "stone", StringComparison.Ordinal) || string.Equals(zoneId, "path", StringComparison.Ordinal);
        }

        private static GameObject SpawnWarblade(Transform root, BlueprintCanvas canvas)
        {
            GameObject playerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PlayerPrefabPath);
            GameObject player = playerPrefab != null
                ? PrefabUtility.InstantiatePrefab(playerPrefab) as GameObject
                : new GameObject("Warblade_v15h_Player");

            if (player == null)
            {
                player = new GameObject("Warblade_v15h_Player");
            }

            player.name = "Warblade_v15h_Player";
            player.transform.SetParent(root, false);
            player.transform.localPosition = new Vector3(canvas.GridSize.x * 0.5f, canvas.GridSize.y * 0.5f, 0f);
            player.transform.localScale = new Vector3(1.25f, 1.25f, 1f);
            SetTagIfAvailable(player, "Player");

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>() ?? player.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            if (player.GetComponent<Collider2D>() == null)
            {
                CapsuleCollider2D collider = player.AddComponent<CapsuleCollider2D>();
                collider.size = new Vector2(0.5f, 0.5f);
                collider.direction = CapsuleDirection2D.Vertical;
            }

            if (player.GetComponent<RIMA.PlayerController>() == null)
            {
                player.AddComponent<RIMA.PlayerController>();
            }

            SpriteRenderer renderer = player.GetComponentInChildren<SpriteRenderer>(true);
            if (renderer == null)
            {
                renderer = player.AddComponent<SpriteRenderer>();
            }

            renderer.sprite = LoadWarbladeSprite();
            renderer.sortingOrder = 200;
            EditorUtility.SetDirty(player);
            return player;
        }

        private static Sprite LoadWarbladeSprite()
        {
            Sprite sprite = AssetDatabase.LoadAllAssetRepresentationsAtPath(WarbladeSpritePath)
                .OfType<Sprite>()
                .FirstOrDefault();
            if (sprite != null)
            {
                return sprite;
            }

            return AssetDatabase.LoadAssetAtPath<Sprite>(WarbladeSpritePath);
        }

        private static void SetTagIfAvailable(GameObject gameObject, string tag)
        {
            try
            {
                gameObject.tag = tag;
            }
            catch (UnityException)
            {
                gameObject.tag = "Untagged";
            }
        }

        private static string RemapZone(string zoneId)
        {
            if (string.Equals(zoneId, "path", StringComparison.Ordinal))
            {
                return "path";
            }

            if (string.Equals(zoneId, "stone", StringComparison.Ordinal) || string.Equals(zoneId, "wall", StringComparison.Ordinal))
            {
                return "stone";
            }

            return "dirt";
        }

        private static Metrics CollectMetrics(Transform root, int cellCount, int zonePlaced, int adjacencyPlaced, int wangPlaced, GameObject player, BlueprintProfileSO profile)
        {
            var metrics = new Metrics
            {
                cellCount = cellCount,
                zonePlaced = zonePlaced,
                adjacencyPlaced = adjacencyPlaced,
                wangPlaced = wangPlaced,
                playerSpawned = player != null,
                playerHasMovement = player != null && player.GetComponent<RIMA.PlayerController>() != null,
                totalChildren = root.GetComponentsInChildren<Transform>(true).Length - 1
            };

            var layerCells = new Dictionary<int, HashSet<string>>();
            for (int i = 1; i <= 8; i++)
            {
                layerCells[i] = new HashSet<string>();
            }

            Transform[] children = root.GetComponentsInChildren<Transform>(true);
            var regex = new Regex("^" + Regex.Escape(AutoPopulator.PlacedPrefix) + "L([1-8])_.+_(-?\\d+)_(-?\\d+)$");
            for (int i = 0; i < children.Length; i++)
            {
                Transform child = children[i];
                if (child == root)
                {
                    continue;
                }

                Match match = regex.Match(child.name);
                if (!match.Success)
                {
                    continue;
                }

                int layer = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                metrics.layerCounts[layer]++;
                string cellKey = match.Groups[2].Value + "," + match.Groups[3].Value;
                layerCells[layer].Add(cellKey);
                if (layer == 2 || layer == 3)
                {
                    metrics.floorCells.Add(cellKey);
                }
            }

            metrics.layer1Coverage = cellCount > 0 ? (float)layerCells[1].Count / cellCount : 0f;
            metrics.layer2Coverage = cellCount > 0 ? (float)layerCells[2].Count / cellCount : 0f;
            metrics.floorFillCoverage = cellCount > 0 ? (float)metrics.floorCells.Count / cellCount : 0f;
            metrics.profileHasWangRule = profile != null && profile.wangRuleTileRef != null;
            return metrics;
        }

        private static void CaptureScreenshot(Transform root)
        {
            Directory.CreateDirectory("Assets/Screenshots");

            var cameraObject = new GameObject("V15hScreenshotCamera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.03f, 0.03f, 0.04f, 1f);
            camera.transform.position = root.position + new Vector3(18f, 11f, -10f);
            camera.transform.rotation = Quaternion.identity;
            camera.orthographicSize = 13f;

            const int width = 1600;
            const int height = 1000;
            RenderTexture renderTexture = new RenderTexture(width, height, 24);
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            camera.targetTexture = renderTexture;
            RenderTexture previousActive = RenderTexture.active;
            RenderTexture.active = renderTexture;
            camera.Render();
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();

            File.WriteAllBytes(ScreenshotPath, texture.EncodeToPNG());

            RenderTexture.active = previousActive;
            camera.targetTexture = null;
            Object.DestroyImmediate(texture);
            Object.DestroyImmediate(renderTexture);
            Object.DestroyImmediate(cameraObject);
            AssetDatabase.ImportAsset(ScreenshotPath);
        }

        private static void CaptureSideBySide()
        {
            if (!File.Exists(RimaV15gMinimalComposer.ScreenshotPath) || !File.Exists(ScreenshotPath))
            {
                return;
            }

            Texture2D left = LoadTexture(RimaV15gMinimalComposer.ScreenshotPath);
            Texture2D right = LoadTexture(ScreenshotPath);
            if (left == null || right == null)
            {
                Object.DestroyImmediate(left);
                Object.DestroyImmediate(right);
                return;
            }

            int width = left.width + right.width;
            int height = Mathf.Max(left.height, right.height);
            Texture2D output = new Texture2D(width, height, TextureFormat.RGB24, false);
            Color[] background = new Color[width * height];
            for (int i = 0; i < background.Length; i++)
            {
                background[i] = new Color(0.03f, 0.03f, 0.04f, 1f);
            }

            output.SetPixels(background);
            output.SetPixels(0, height - left.height, left.width, left.height, left.GetPixels());
            output.SetPixels(left.width, height - right.height, right.width, right.height, right.GetPixels());
            output.Apply();

            File.WriteAllBytes(SideBySidePath, output.EncodeToPNG());
            Object.DestroyImmediate(left);
            Object.DestroyImmediate(right);
            Object.DestroyImmediate(output);
            AssetDatabase.ImportAsset(SideBySidePath);
        }

        private static Texture2D LoadTexture(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            var texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            return texture.LoadImage(File.ReadAllBytes(path)) ? texture : null;
        }

        private static void WriteMetrics(Metrics metrics, AutoPopulator.CompositionReport compositionReport)
        {
            Directory.CreateDirectory("STAGING");
            File.WriteAllText(MetricsPath, MetricsText(metrics, compositionReport));
        }

        private static string MetricsText(Metrics metrics, AutoPopulator.CompositionReport compositionReport)
        {
            return compositionReport.ToMetricsString("CombatRoom v15h playable") +
                "v15h playable map metrics\n" +
                $"cells={metrics.cellCount}\n" +
                $"zonePlaced={metrics.zonePlaced}\n" +
                $"adjacencyPlaced={metrics.adjacencyPlaced}\n" +
                $"wangPlaced={metrics.wangPlaced}\n" +
                $"totalChildren={metrics.totalChildren}\n" +
                $"L1={metrics.layerCounts[1]}\n" +
                $"L2={metrics.layerCounts[2]}\n" +
                $"L3={metrics.layerCounts[3]}\n" +
                $"L4={metrics.layerCounts[4]}\n" +
                $"L5={metrics.layerCounts[5]}\n" +
                $"L6={metrics.layerCounts[6]}\n" +
                $"L7={metrics.layerCounts[7]}\n" +
                $"L8={metrics.layerCounts[8]}\n" +
                $"L1Coverage={metrics.layer1Coverage.ToString("0.000", CultureInfo.InvariantCulture)}\n" +
                $"L2Coverage={metrics.layer2Coverage.ToString("0.000", CultureInfo.InvariantCulture)}\n" +
                $"FloorFillCoverage={metrics.floorFillCoverage.ToString("0.000", CultureInfo.InvariantCulture)}\n" +
                $"ProfileHasWangRule={metrics.profileHasWangRule}\n" +
                $"PlayerSpawned={metrics.playerSpawned}\n" +
                $"PlayerHasMovement={metrics.playerHasMovement}\n" +
                $"Screenshot={ScreenshotPath}\n" +
                $"SideBySide={SideBySidePath}\n";
        }

        private static void WriteDoneMarker(Metrics metrics, AutoPopulator.CompositionReport compositionReport)
        {
            Directory.CreateDirectory("STAGING");
            File.WriteAllText(
                DoneMarkerPath,
                "v15h playable map DONE\n" +
                $"Screenshot: {ScreenshotPath}\n" +
                $"SideBySide: {SideBySidePath}\n" +
                $"Metrics: {MetricsPath}\n" +
                $"Cells: {metrics.cellCount}\n" +
                $"FloorFillCoverage: {metrics.floorFillCoverage.ToString("0.000", CultureInfo.InvariantCulture)}\n" +
                $"WangTilesPlaced: {metrics.wangPlaced}\n" +
                $"PlayerSpawned: {metrics.playerSpawned}\n" +
                $"PlayerHasMovement: {metrics.playerHasMovement}\n" +
                $"BudgetFailures: {string.Join(",", compositionReport.BudgetFailures())}\n");
        }

        private static Vector3 DisableHistoricalRoots()
        {
            Vector3 rootPosition = Vector3.zero;
            for (int i = 0; i < RootsToDisable.Length; i++)
            {
                while (true)
                {
                    GameObject root = GameObject.Find(RootsToDisable[i]);
                    if (root == null)
                    {
                        break;
                    }

                    if (rootPosition == Vector3.zero)
                    {
                        rootPosition = root.transform.position;
                    }

                    root.SetActive(false);
                    EditorUtility.SetDirty(root);
                }
            }

            return rootPosition;
        }

        private static void DestroyIfPresent(string objectName)
        {
            GameObject existing = GameObject.Find(objectName);
            if (existing != null)
            {
                Object.DestroyImmediate(existing);
            }
        }

        private static T LoadOrCreateAsset<T>(string path) where T : ScriptableObject
        {
            T asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset != null)
            {
                return asset;
            }

            asset = ScriptableObject.CreateInstance<T>();
            asset.name = Path.GetFileNameWithoutExtension(path);
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        private static void ClearSubAssets(string path, Object mainAsset)
        {
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
            for (int i = 0; i < assets.Length; i++)
            {
                Object asset = assets[i];
                if (asset != null && asset != mainAsset)
                {
                    Object.DestroyImmediate(asset, true);
                }
            }
        }

        private static void ReplaceAsset(Object asset, string assetPath)
        {
            Object existing = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (existing != null)
            {
                AssetDatabase.DeleteAsset(assetPath);
            }

            AssetDatabase.CreateAsset(asset, assetPath);
        }

        private static void SetZoneDefaultDensity(string path, float density)
        {
            BlueprintZoneTypeSO zone = AssetDatabase.LoadAssetAtPath<BlueprintZoneTypeSO>(path);
            if (zone == null)
            {
                return;
            }

            zone.defaultDensity = density;
            EditorUtility.SetDirty(zone);
        }

        private static void ExitIfBatchMode(int exitCode)
        {
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(exitCode);
            }
        }

        private sealed class Metrics
        {
            public int cellCount;
            public int zonePlaced;
            public int adjacencyPlaced;
            public int wangPlaced;
            public int totalChildren;
            public readonly int[] layerCounts = new int[9];
            public float layer1Coverage;
            public float layer2Coverage;
            public float floorFillCoverage;
            public bool profileHasWangRule;
            public bool playerSpawned;
            public bool playerHasMovement;
            public readonly HashSet<string> floorCells = new HashSet<string>();
        }
    }
}
#endif
