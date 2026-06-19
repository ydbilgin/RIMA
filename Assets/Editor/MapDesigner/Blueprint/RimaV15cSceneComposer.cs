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
    public static class RimaV15cSceneComposer
    {
        private const string ScenePath = "Assets/Scenes/Demo/RoomPipelineTest.unity";
        private const string ProfilePath = "Assets/Data/Blueprint/Profiles/profile_combat_room_default.asset";
        private const string RoomPath = "Assets/Data/Blueprint/Rooms/combat_room_v15b.asset";
        private const string V15bRootName = "Pro_Redesign_v15b_FullAdjacency_CombatRoom";
        private const string V15cRootName = "Pro_Redesign_v15c_8LayerPainted_CombatRoom";
        private const string MetricsPath = "STAGING/BUILD_DONE_v15e_B_secondary_cluster_cap_metrics.txt";
        private const string ScreenshotPath = "Assets/Screenshots/PlayableRoom_combat_v15e_B_secondary_cap_LIVE.png";

        public static void Build()
        {
            try
            {
                Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
                if (!scene.IsValid())
                {
                    throw new InvalidOperationException($"Failed to open scene: {ScenePath}");
                }

                BlueprintProfileSO profile = AssetDatabase.LoadAssetAtPath<BlueprintProfileSO>(ProfilePath);
                RoomBlueprintSO room = AssetDatabase.LoadAssetAtPath<RoomBlueprintSO>(RoomPath);
                if (profile == null || room == null)
                {
                    throw new InvalidOperationException("Missing profile or v15b room blueprint asset.");
                }

                EnsureV15dCombatZoneAssets(profile);

                GameObject v15bRoot = GameObject.Find(V15bRootName);
                Vector3 rootPosition = v15bRoot != null ? v15bRoot.transform.position : Vector3.zero;
                if (v15bRoot != null)
                {
                    v15bRoot.SetActive(false);
                    EditorUtility.SetDirty(v15bRoot);
                }

                GameObject previous = GameObject.Find(V15cRootName);
                if (previous != null)
                {
                    UnityEngine.Object.DestroyImmediate(previous);
                }

                var v15cRoot = new GameObject(V15cRootName);
                v15cRoot.transform.position = rootPosition;

                BlueprintCanvas canvas = room.ToCanvas();
                int zonePlaced = AutoPopulator.PopulateZones(canvas, profile, v15cRoot.transform, 2026);
                int adjacencyPlaced = AutoPopulator.PopulateAdjacency(canvas, profile, v15cRoot.transform, 2026);
                Metrics metrics = CollectMetrics(v15cRoot.transform, canvas.Count, zonePlaced, adjacencyPlaced, profile);
                AutoPopulator.CompositionReport compositionReport = AutoPopulator.LastCompositionReport;

                EditorSceneManager.MarkSceneDirty(scene);
                EditorSceneManager.SaveScene(scene);

                CaptureScreenshot(v15cRoot.transform);
                WriteMetrics(metrics, compositionReport);
                List<string> budgetFailures = compositionReport.BudgetFailures();
                if (budgetFailures.Count > 0)
                {
                    Debug.LogWarning("[v15d Metrics] Budget violation: " + string.Join(", ", budgetFailures.ToArray()));
                }

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

        private static void ExitIfBatchMode(int exitCode)
        {
            if (Application.isBatchMode)
            {
                EditorApplication.Exit(exitCode);
            }
        }

        private static void EnsureV15dCombatZoneAssets(BlueprintProfileSO profile)
        {
            if (profile == null || profile.zones == null)
            {
                return;
            }

            BlueprintPropPoolSO accentPool = AssetDatabase.LoadAssetAtPath<BlueprintPropPoolSO>("Assets/Data/Blueprint/PropPools/LayerPools/pool_layer3_atmospheric_accents.asset");
            for (int i = 0; i < profile.zones.Length; i++)
            {
                BlueprintZoneTypeSO zone = profile.zones[i];
                if (zone == null || string.IsNullOrEmpty(zone.zoneId))
                {
                    continue;
                }

                zone.negativeSpaceRatio = 0.20f;
                zone.floorWeights = new Vector3(0.70f, 0.20f, 0.10f);
                zone.dominantFloorPool = AssetDatabase.LoadAssetAtPath<BlueprintPropPoolSO>($"Assets/Data/Blueprint/PropPools/pool_{zone.zoneId}.asset");
                zone.secondaryFloorPool = zone.midToneOverlayPool != null ? zone.midToneOverlayPool : accentPool;
                zone.accentFloorPool = accentPool;
                zone.pathProtect = true;
                zone.heroPropClusterCap = 3;
                zone.heroPropClusterSize = new Vector2Int(2, 5);
                zone.heroPropClusterBuffer = 2;
                zone.secondaryClusterCap = SecondaryClusterCapForZone(zone.zoneId);
                zone.pathCellRatio = 0.15f;
                zone.pathMinWidth = 2;
                zone.atmosphericCap = AtmosphericCapForZone(zone.zoneId);
                zone.macroFillSprites = LoadLayer1Sprites(zone.zoneId);
                zone.atmosphericPool = AssetDatabase.LoadAssetAtPath<BlueprintPropPoolSO>($"Assets/Data/Blueprint/PropPools/pool_atmospheric_{zone.zoneId}.asset");
                EditorUtility.SetDirty(zone);
            }
        }

        private static int AtmosphericCapForZone(string zoneId)
        {
            if (string.Equals(zoneId, "grass", StringComparison.Ordinal))
            {
                return 8;
            }

            if (string.Equals(zoneId, "path", StringComparison.Ordinal))
            {
                return 8;
            }

            return 10;
        }

        private static int SecondaryClusterCapForZone(string zoneId)
        {
            if (string.Equals(zoneId, "grass", StringComparison.Ordinal))
            {
                return 3;
            }

            return 4;
        }

        private static Sprite[] LoadLayer1Sprites(string zoneId)
        {
            string basePath = "Assets/Data/Brush/AssetParts_v5_Layer1_8/macro_fill";
            var sprites = new List<Sprite>();
            AddSpriteIfPresent(sprites, $"{basePath}/macro_{zoneId}_v1.png");
            AddSpriteIfPresent(sprites, $"{basePath}/macro_{zoneId}_v2.png");
            return sprites.ToArray();
        }

        private static void AddSpriteIfPresent(List<Sprite> sprites, string path)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null)
            {
                sprites.Add(sprite);
            }
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

                int layer = int.Parse(match.Groups[1].Value);
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

            var cameraObject = new GameObject("V15cScreenshotCamera");
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

        private static void WriteMetrics(Metrics metrics, AutoPopulator.CompositionReport compositionReport)
        {
            Directory.CreateDirectory("STAGING");
            File.WriteAllText(
                MetricsPath,
                compositionReport.ToMetricsString("CombatRoom") +
                "v15e-B scene placement metrics\n" +
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
                $"Screenshot={ScreenshotPath}\n");
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
