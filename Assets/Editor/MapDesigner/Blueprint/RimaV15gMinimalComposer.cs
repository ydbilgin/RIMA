#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RIMA.MapDesigner.Editor.Blueprint
{
    public static class RimaV15gMinimalComposer
    {
        public const string ScenePath = "Assets/Scenes/Demo/RoomPipelineTest.unity";
        public const string ProfilePath = "Assets/Data/Blueprint/Profiles/profile_v15g_minimal_pixellab.asset";
        public const string RoomPath = "Assets/Data/Blueprint/Rooms/combat_room_v15b.asset";
        public const string AssetPartsPath = "Assets/Data/Brush/AssetParts_v3/CombatBiome_v15g";
        public const string V15gRootName = "Pro_Redesign_v15g_Minimal_PixelLab_CombatRoom";
        public const string MetricsPath = "STAGING/BUILD_DONE_v15g_metrics.txt";
        public const string ScreenshotPath = "Assets/Screenshots/PlayableRoom_combat_v15g_minimal_pixellab_LIVE.png";
        public const string SideBySidePath = "Assets/Screenshots/PlayableRoom_combat_v15d_vs_v15g_minimal_pixellab.png";
        public const string DoneMarkerPath = "STAGING/BUILD_TASK_v15g_map_cleanup_redesign_DONE.md";

        private const string AutoRunFlagPath = "STAGING/RUN_V15G_COMPOSER.flag";
        private const string ZoneFolder = "Assets/Data/Blueprint/ZoneTypes/v15g";
        private const string PoolFolder = "Assets/Data/Blueprint/PropPools";
        private const string V15dScreenshotPath = "Assets/Screenshots/PlayableRoom_combat_v15d_composition_LIVE.png";
        private static bool isAutoRunRunning;

        private static readonly string[] RootsToDisable =
        {
            "Pro_Redesign_v15_BlueprintFirst_CombatRoom",
            "Pro_Redesign_v15b_FullAdjacency_CombatRoom",
            "Pro_Redesign_v15c_8LayerPainted_CombatRoom",
            "Pro_Redesign_v15d_Composition_CombatRoom",
            "Pro_Redesign_v15e_A_L8cap_CombatRoom"
        };

        [InitializeOnLoadMethod]
        private static void RegisterAutoRunFlagWatcher()
        {
            EditorApplication.update -= RunFromFlagOnEditorUpdate;
            EditorApplication.update += RunFromFlagOnEditorUpdate;
        }

        private static void RunFromFlagOnEditorUpdate()
        {
            if (isAutoRunRunning || EditorApplication.isCompiling || !File.Exists(AutoRunFlagPath))
            {
                return;
            }

            isAutoRunRunning = true;
            EditorApplication.update -= RunFromFlagOnEditorUpdate;
            try
            {
                File.Delete(AutoRunFlagPath);
                Build();
            }
            finally
            {
                isAutoRunRunning = false;
            }
        }

        public static void Build()
        {
            try
            {
                EnsureV15gAssetGraph();

                Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
                if (!scene.IsValid())
                {
                    throw new InvalidOperationException($"Failed to open scene: {ScenePath}");
                }

                BlueprintProfileSO profile = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(ProfilePath);
                RoomBlueprintSO room = AssetDatabase.LoadAssetAtPath<RoomBlueprintSO>(RoomPath);
                if (profile == null || room == null)
                {
                    throw new InvalidOperationException("Missing v15g profile or v15b room blueprint asset.");
                }

                Vector3 rootPosition = DisableHistoricalRoots();
                GameObject previous = GameObject.Find(V15gRootName);
                if (previous != null)
                {
                    UnityEngine.Object.DestroyImmediate(previous);
                }

                var v15gRoot = new GameObject(V15gRootName);
                v15gRoot.transform.position = rootPosition;

                BlueprintCanvas canvas = CreateMinimalCanvas(room.ToCanvas());
                int zonePlaced = AutoPopulator.PopulateZones(canvas, profile, v15gRoot.transform, 2026);
                int adjacencyPlaced = AutoPopulator.PopulateAdjacency(canvas, profile, v15gRoot.transform, 2026);
                Metrics metrics = CollectMetrics(v15gRoot.transform, canvas.Count, zonePlaced, adjacencyPlaced, profile);
                AutoPopulator.CompositionReport compositionReport = AutoPopulator.LastCompositionReport;

                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);

                CaptureScreenshot(v15gRoot.transform);
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

        public static void EnsureV15gAssetGraph()
        {
            Directory.CreateDirectory(AssetPartsPath);
            Directory.CreateDirectory(ZoneFolder);
            Directory.CreateDirectory(PoolFolder);
            Directory.CreateDirectory(Path.GetDirectoryName(ProfilePath));

            AssetDatabase.Refresh();
            ImportPixelLabSprites();

            Sprite[] dominantSprites = LoadPilotSprites(0, 2, 8, 10, 14);
            Sprite[] secondarySprites = LoadPilotSprites(4, 6, 12);
            Sprite[] pathSprites = LoadPilotSprites(4, 6);
            Sprite[] dirtSprites = LoadSprites("pixellab_dirt_tile_*.png");
            Sprite[] transitionSprites = Combine(LoadPilotSprites(1, 3, 5, 7, 9, 11, 13, 15), dirtSprites);

            BlueprintPropPoolSO dominantPool = CreateOrUpdatePool($"{PoolFolder}/pool_v15g_pixellab_dominant.asset", "pool_v15g_pixellab_dominant", dominantSprites);
            BlueprintPropPoolSO secondaryPool = CreateOrUpdatePool($"{PoolFolder}/pool_v15g_pixellab_secondary.asset", "pool_v15g_pixellab_secondary", secondarySprites);
            BlueprintPropPoolSO pathPool = CreateOrUpdatePool($"{PoolFolder}/pool_v15g_pixellab_path.asset", "pool_v15g_pixellab_path", pathSprites);
            BlueprintPropPoolSO dirtPool = CreateOrUpdatePool($"{PoolFolder}/pool_v15g_pixellab_dirt.asset", "pool_v15g_pixellab_dirt", dirtSprites);
            BlueprintPropPoolSO transitionPool = CreateOrUpdatePool($"{PoolFolder}/pool_v15g_pixellab_transition.asset", "pool_v15g_pixellab_transition", transitionSprites);

            BlueprintZoneTypeSO stoneZone = CreateOrUpdateZone(
                $"{ZoneFolder}/zone_stone_v15g.asset",
                "stone",
                "Stone v15g PixelLab",
                new Color(0.47f, 0.48f, 0.46f, 1f),
                dominantPool,
                secondaryPool,
                transitionPool,
                dominantSprites,
                transitionPool);

            BlueprintZoneTypeSO pathZone = CreateOrUpdateZone(
                $"{ZoneFolder}/zone_path_v15g.asset",
                "path",
                "Path v15g PixelLab",
                new Color(0.72f, 0.61f, 0.42f, 1f),
                pathPool,
                secondaryPool,
                transitionPool,
                pathSprites,
                transitionPool);

            BlueprintZoneTypeSO dirtZone = CreateOrUpdateZone(
                $"{ZoneFolder}/zone_dirt_v15g.asset",
                "dirt",
                "Dirt v15g PixelLab",
                new Color(0.36f, 0.28f, 0.20f, 1f),
                dirtPool,
                transitionPool,
                secondaryPool,
                dirtSprites,
                transitionPool);

            CreateOrUpdateProfile(stoneZone, pathZone, dirtZone, transitionPool);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static BlueprintCanvas CreateMinimalCanvas(BlueprintCanvas source)
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
            EnsureV15gAssetGraph();
            BlueprintProfileSO profile = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(ProfilePath);
            RoomBlueprintSO room = AssetDatabase.LoadAssetAtPath<RoomBlueprintSO>(RoomPath);
            if (profile == null || room == null)
            {
                throw new InvalidOperationException("Missing v15g profile or v15b room blueprint asset.");
            }

            var previewRoot = new GameObject("V15gMetricsPreviewRoot");
            try
            {
                BlueprintCanvas canvas = CreateMinimalCanvas(room.ToCanvas());
                int zonePlaced = AutoPopulator.PopulateZones(canvas, profile, previewRoot.transform, 2026);
                int adjacencyPlaced = AutoPopulator.PopulateAdjacency(canvas, profile, previewRoot.transform, 2026);
                Metrics metrics = CollectMetrics(previewRoot.transform, canvas.Count, zonePlaced, adjacencyPlaced, profile);
                return MetricsText(metrics, AutoPopulator.LastCompositionReport);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(previewRoot);
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

        private static void ImportPixelLabSprites()
        {
            string[] pngPaths = Directory.GetFiles(AssetPartsPath, "*.png", SearchOption.TopDirectoryOnly);
            if (pngPaths.Length == 0)
            {
                throw new InvalidOperationException($"No PixelLab PNGs found in {AssetPartsPath}");
            }

            for (int i = 0; i < pngPaths.Length; i++)
            {
                string assetPath = NormalizeAssetPath(pngPaths[i]);
                AssetDatabase.ImportAsset(assetPath);
                TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
                if (importer == null)
                {
                    continue;
                }

                bool changed = false;
                if (importer.textureType != TextureImporterType.Sprite)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    changed = true;
                }

                if (importer.spriteImportMode != SpriteImportMode.Single)
                {
                    importer.spriteImportMode = SpriteImportMode.Single;
                    changed = true;
                }

                if (!Mathf.Approximately(importer.spritePixelsPerUnit, 32f))
                {
                    importer.spritePixelsPerUnit = 32f;
                    changed = true;
                }

                if (importer.filterMode != FilterMode.Point)
                {
                    importer.filterMode = FilterMode.Point;
                    changed = true;
                }

                if (importer.textureCompression != TextureImporterCompression.Uncompressed)
                {
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    changed = true;
                }

                if (importer.mipmapEnabled)
                {
                    importer.mipmapEnabled = false;
                    changed = true;
                }

                if (!importer.alphaIsTransparency)
                {
                    importer.alphaIsTransparency = true;
                    changed = true;
                }

                if (changed)
                {
                    importer.SaveAndReimport();
                }
            }
        }

        private static Sprite[] LoadPilotSprites(params int[] indices)
        {
            var sprites = new List<Sprite>();
            for (int i = 0; i < indices.Length; i++)
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>($"{AssetPartsPath}/pixellab_pilot_tile_{indices[i]}.png");
                if (sprite != null)
                {
                    sprites.Add(sprite);
                }
            }

            return sprites.ToArray();
        }

        private static Sprite[] LoadSprites(string pattern)
        {
            var sprites = new List<Sprite>();
            string[] files = Directory.GetFiles(AssetPartsPath, pattern, SearchOption.TopDirectoryOnly);
            Array.Sort(files, StringComparer.Ordinal);
            for (int i = 0; i < files.Length; i++)
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(NormalizeAssetPath(files[i]));
                if (sprite != null)
                {
                    sprites.Add(sprite);
                }
            }

            return sprites.ToArray();
        }

        private static Sprite[] Combine(Sprite[] first, Sprite[] second)
        {
            var sprites = new List<Sprite>();
            if (first != null)
            {
                sprites.AddRange(first);
            }

            if (second != null)
            {
                sprites.AddRange(second);
            }

            return sprites.ToArray();
        }

        private static BlueprintPropPoolSO CreateOrUpdatePool(string path, string poolId, Sprite[] sprites)
        {
            if (sprites == null || sprites.Length == 0)
            {
                throw new InvalidOperationException($"Pool '{poolId}' has no sprites.");
            }

            BlueprintPropPoolSO pool = LoadOrCreateAsset<BlueprintPropPoolSO>(path);
            ClearSubAssets(path, pool);

            pool.poolId = poolId;
            pool.entries = new WeightedProp[sprites.Length];
            for (int i = 0; i < sprites.Length; i++)
            {
                PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
                prop.name = $"{poolId}_{sprites[i].name}";
                prop.propId = prop.name;
                prop.visual = sprites[i];
                prop.footprint = Vector2Int.one;
                prop.hasCollision = false;
                prop.blocksMovement = false;
                prop.colliderShape = PropDefinitionSO.ColliderShape.None;
                prop.colliderLayer = string.Empty;
                prop.validTerrainIds = new List<string>();
                AssetDatabase.AddObjectToAsset(prop, pool);
                pool.entries[i] = new WeightedProp { prop = prop, weight = 1f };
            }

            EditorUtility.SetDirty(pool);
            return pool;
        }

        private static BlueprintZoneTypeSO CreateOrUpdateZone(
            string path,
            string zoneId,
            string displayName,
            Color brushColor,
            BlueprintPropPoolSO dominantPool,
            BlueprintPropPoolSO secondaryPool,
            BlueprintPropPoolSO accentPool,
            Sprite[] macroSprites,
            BlueprintPropPoolSO transitionPool)
        {
            BlueprintZoneTypeSO zone = LoadOrCreateAsset<BlueprintZoneTypeSO>(path);
            zone.zoneId = zoneId;
            zone.displayName = displayName;
            zone.brushColor = brushColor;
            zone.defaultDensity = string.Equals(zoneId, "stone", StringComparison.Ordinal) || string.Equals(zoneId, "dirt", StringComparison.Ordinal) ? 0.85f : 0.55f;
            zone.maxFeatureProps = 0;
            zone.negativeSpaceRatio = 0.25f;
            zone.floorWeights = new Vector3(0.75f, 0.18f, 0.07f);
            zone.dominantFloorPool = dominantPool;
            zone.secondaryFloorPool = secondaryPool;
            zone.accentFloorPool = accentPool;
            zone.pathProtect = true;
            zone.heroPropClusterCap = 2;
            zone.heroPropClusterSize = new Vector2Int(2, 3);
            zone.heroPropClusterBuffer = 3;
            zone.pathCellRatio = 0.15f;
            zone.pathMinWidth = 2;
            zone.atmosphericCap = 5;
            zone.macroFillSprites = macroSprites ?? Array.Empty<Sprite>();
            zone.baseFloorSprites = macroSprites ?? Array.Empty<Sprite>();
            zone.midToneOverlayPool = null;
            zone.midToneDensity = 0f;
            zone.detailTexturePool = transitionPool;
            zone.detailDensity = 0f;
            zone.smallScatterPool = null;
            zone.smallScatterDensity = 0f;
            zone.mediumPropPool = null;
            zone.mediumPropDensity = 0f;
            zone.tallFocalPool = null;
            zone.maxTallFocalPerRegion = 0;
            zone.atmosphericPool = transitionPool;
            zone.atmosphericDensity = 0f;
            EditorUtility.SetDirty(zone);
            return zone;
        }

        private static void CreateOrUpdateProfile(
            BlueprintZoneTypeSO stoneZone,
            BlueprintZoneTypeSO pathZone,
            BlueprintZoneTypeSO dirtZone,
            BlueprintPropPoolSO transitionPool)
        {
            BlueprintProfileSO profile = LoadOrCreateAsset<BlueprintProfileSO>(ProfilePath);
            ClearSubAssets(ProfilePath, profile);

            profile.profileId = "profile_v15g_minimal_pixellab";
            profile.gridSize = new Vector2Int(36, 22);
            profile.zones = new[] { stoneZone, pathZone, dirtZone };
            profile.adjacencyRules = new[]
            {
                CreateRule(profile, "rule_stone_path_v15g", "stone", "path", transitionPool, 0.18f),
                CreateRule(profile, "rule_stone_dirt_v15g", "stone", "dirt", transitionPool, 0.16f),
                CreateRule(profile, "rule_path_dirt_v15g", "path", "dirt", transitionPool, 0.14f)
            };

            EditorUtility.SetDirty(profile);
        }

        private static BlueprintAdjacencyRuleSO CreateRule(
            BlueprintProfileSO profile,
            string ruleId,
            string zoneA,
            string zoneB,
            BlueprintPropPoolSO transitionPool,
            float density)
        {
            BlueprintAdjacencyRuleSO rule = ScriptableObject.CreateInstance<BlueprintAdjacencyRuleSO>();
            rule.name = ruleId;
            rule.ruleId = ruleId;
            rule.zoneIdA = zoneA;
            rule.zoneIdB = zoneB;
            rule.transitionPool = transitionPool;
            rule.density = density;
            AssetDatabase.AddObjectToAsset(rule, profile);
            return rule;
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

        private static void ClearSubAssets(string path, UnityEngine.Object mainAsset)
        {
            UnityEngine.Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
            for (int i = 0; i < assets.Length; i++)
            {
                UnityEngine.Object asset = assets[i];
                if (asset != null && asset != mainAsset)
                {
                    UnityEngine.Object.DestroyImmediate(asset, true);
                }
            }
        }

        private static Vector3 DisableHistoricalRoots()
        {
            Vector3 rootPosition = Vector3.zero;
            for (int i = 0; i < RootsToDisable.Length; i++)
            {
                GameObject root = GameObject.Find(RootsToDisable[i]);
                if (root == null)
                {
                    continue;
                }

                if (rootPosition == Vector3.zero)
                {
                    rootPosition = root.transform.position;
                }

                root.SetActive(false);
                EditorUtility.SetDirty(root);
            }

            return rootPosition;
        }

        private static Metrics CollectMetrics(Transform root, int cellCount, int zonePlaced, int adjacencyPlaced, BlueprintProfileSO profile)
        {
            var metrics = new Metrics
            {
                cellCount = cellCount,
                zonePlaced = zonePlaced,
                adjacencyPlaced = adjacencyPlaced,
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
                layerCells[layer].Add(match.Groups[2].Value + "," + match.Groups[3].Value);
            }

            metrics.layer1Coverage = cellCount > 0 ? (float)layerCells[1].Count / cellCount : 0f;
            metrics.layer2Coverage = cellCount > 0 ? (float)layerCells[2].Count / cellCount : 0f;

            if (profile != null && profile.zones != null)
            {
                for (int i = 0; i < profile.zones.Length; i++)
                {
                    BlueprintZoneTypeSO zone = profile.zones[i];
                    if (zone == null)
                    {
                        continue;
                    }

                    if (zone.macroFillSprites == null || zone.macroFillSprites.Length == 0)
                    {
                        metrics.layer1Gaps.Add(zone.zoneId);
                    }

                    if (zone.atmosphericPool == null || zone.atmosphericPool.entries == null || zone.atmosphericPool.entries.Length == 0)
                    {
                        metrics.layer8Gaps.Add(zone.zoneId);
                    }
                }
            }

            return metrics;
        }

        private static void CaptureScreenshot(Transform root)
        {
            Directory.CreateDirectory("Assets/Screenshots");

            var cameraObject = new GameObject("V15gScreenshotCamera");
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
            UnityEngine.Object.DestroyImmediate(texture);
            UnityEngine.Object.DestroyImmediate(renderTexture);
            UnityEngine.Object.DestroyImmediate(cameraObject);
            AssetDatabase.ImportAsset(ScreenshotPath);
        }

        private static void CaptureSideBySide()
        {
            if (!File.Exists(V15dScreenshotPath) || !File.Exists(ScreenshotPath))
            {
                return;
            }

            Texture2D left = LoadTexture(V15dScreenshotPath);
            Texture2D right = LoadTexture(ScreenshotPath);
            if (left == null || right == null)
            {
                UnityEngine.Object.DestroyImmediate(left);
                UnityEngine.Object.DestroyImmediate(right);
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
            UnityEngine.Object.DestroyImmediate(left);
            UnityEngine.Object.DestroyImmediate(right);
            UnityEngine.Object.DestroyImmediate(output);
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
            return compositionReport.ToMetricsString("CombatRoom v15g PixelLab") +
                   "v15g minimal PixelLab scene placement metrics\n" +
                   $"cells={metrics.cellCount}\n" +
                   $"zonePlaced={metrics.zonePlaced}\n" +
                   $"adjacencyPlaced={metrics.adjacencyPlaced}\n" +
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
                   $"Layer1Gaps={string.Join(",", metrics.layer1Gaps)}\n" +
                   $"Layer8Gaps={string.Join(",", metrics.layer8Gaps)}\n" +
                   $"Screenshot={ScreenshotPath}\n" +
                   $"SideBySide={SideBySidePath}\n";
        }

        private static void WriteDoneMarker(Metrics metrics, AutoPopulator.CompositionReport compositionReport)
        {
            Directory.CreateDirectory("STAGING");
            File.WriteAllText(
                DoneMarkerPath,
                "v15g map cleanup redesign DONE\n" +
                $"Screenshot: {ScreenshotPath}\n" +
                $"SideBySide: {SideBySidePath}\n" +
                $"Metrics: {MetricsPath}\n" +
                $"Cells: {metrics.cellCount}\n" +
                $"Placed: {metrics.zonePlaced + metrics.adjacencyPlaced}\n" +
                $"BudgetFailures: {string.Join(",", compositionReport.BudgetFailures())}\n");
        }

        private static string NormalizeAssetPath(string path)
        {
            return path.Replace('\\', '/');
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
            public int totalChildren;
            public readonly int[] layerCounts = new int[9];
            public float layer1Coverage;
            public float layer2Coverage;
            public readonly List<string> layer1Gaps = new List<string>();
            public readonly List<string> layer8Gaps = new List<string>();
        }
    }
}
#endif
