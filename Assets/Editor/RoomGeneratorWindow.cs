using System;
using System.IO;
using System.Linq;
using RIMA;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    public class RoomGeneratorWindow : EditorWindow
    {
        private const float PreviewWidth = 200f;
        private const float PreviewHeight = 150f;

        private RimaMapDesignerWindow targetDesigner;
        private int selectedTemplate;
        private int seed = 12345;
        private RoomVariationProcessor.Level variation = RoomVariationProcessor.Level.Medium;
        private RimaMapDesignerWindow.MapSaveData previewData;

        [MenuItem("RIMA/Tools/Room Generator")]
        public static void Open()
        {
            Open(null);
        }

        public static void Open(RimaMapDesignerWindow designer)
        {
            RoomGeneratorWindow window = GetWindow<RoomGeneratorWindow>("Room Generator");
            window.targetDesigner = designer;
            window.minSize = new Vector2(340f, 310f);
            window.EnsurePreview();
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Room Generator", EditorStyles.boldLabel);
            selectedTemplate = EditorGUILayout.Popup("Template", selectedTemplate, RoomTemplateGenerator.TemplateNames);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Seed", GUILayout.Width(EditorGUIUtility.labelWidth - 4f));
            if (GUILayout.Button("Random", GUILayout.Width(78f)))
            {
                seed = UnityEngine.Random.Range(1, int.MaxValue);
                previewData = null;
            }

            int newSeed = EditorGUILayout.IntField(seed);
            if (newSeed != seed)
            {
                seed = newSeed;
                previewData = null;
            }

            EditorGUILayout.EndHorizontal();

            RoomVariationProcessor.Level[] levels =
            {
                RoomVariationProcessor.Level.Subtle,
                RoomVariationProcessor.Level.Medium,
                RoomVariationProcessor.Level.Wild
            };
            int currentLevel = Array.IndexOf(levels, variation);
            int selectedLevel = EditorGUILayout.Popup("Variation", Mathf.Max(0, currentLevel), new[] { "Subtle", "Medium", "Wild" });
            RoomVariationProcessor.Level newVariation = levels[Mathf.Clamp(selectedLevel, 0, levels.Length - 1)];
            if (newVariation != variation)
            {
                variation = newVariation;
                previewData = null;
            }

            EditorGUILayout.LabelField(VariationLabel(variation), EditorStyles.miniLabel);
            EditorGUILayout.Space(8f);

            Rect previewRect = GUILayoutUtility.GetRect(PreviewWidth, PreviewHeight, GUILayout.ExpandWidth(false));
            DrawPreview(previewRect);

            EditorGUILayout.Space(8f);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Preview", GUILayout.Height(28f)))
            {
                previewData = GenerateData();
                Repaint();
            }

            if (GUILayout.Button("Generate -> Map Designer", GUILayout.Height(28f)))
            {
                GenerateToMapDesigner();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void EnsurePreview()
        {
            if (previewData == null)
            {
                previewData = GenerateData();
            }
        }

        private void GenerateToMapDesigner()
        {
            RimaMapDesignerWindow designer = targetDesigner != null ? targetDesigner : GetWindow<RimaMapDesignerWindow>("RIMA Map Designer");
            designer.Show();
            designer.LoadFromGenerator(GenerateData());
            targetDesigner = designer;
        }

        private RimaMapDesignerWindow.MapSaveData GenerateData()
        {
            string templateName = RoomTemplateGenerator.TemplateNames[Mathf.Clamp(selectedTemplate, 0, RoomTemplateGenerator.TemplateNames.Length - 1)];
            var data = Clone(RoomTemplateGenerator.LoadTemplate(templateName));
            if (data == null || ((data.terrainGrid == null || data.terrainGrid.Length == 0) && data.layers == null))
            {
                throw new InvalidDataException("Invalid room template: " + templateName);
            }

            if (data.terrainGrid != null && data.terrainGrid.Length > 0)
            {
                ApplyTerrainVariation(data);
                return data;
            }

            for (int i = 0; i < data.layers.Length; i++)
            {
                var layer = data.layers[i];
                int[,] grid = RoomVariationProcessor.Unflatten(layer.vertexData, data.width, data.height);
                RoomVariationProcessor.Apply(grid, data.width, data.height, seed + i * 1009, variation);
                layer.vertexData = RoomVariationProcessor.Flatten(grid, data.width, data.height);
            }

            return data;
        }

        private void ApplyTerrainVariation(RimaMapDesignerWindow.MapSaveData data)
        {
            int[,] terrain = UnflattenTerrain(data.terrainGrid, data.width, data.height);
            int[,] result = (int[,])terrain.Clone();

            for (int terrainId = 1; terrainId <= 3; terrainId++)
            {
                int[,] mask = new int[data.width + 1, data.height + 1];
                for (int y = 0; y <= data.height; y++)
                {
                    for (int x = 0; x <= data.width; x++)
                    {
                        mask[x, y] = terrain[x, y] == terrainId ? 1 : 0;
                    }
                }

                RoomVariationProcessor.Apply(mask, data.width, data.height, seed + terrainId * 1009, variation);
                for (int y = 0; y <= data.height; y++)
                {
                    for (int x = 0; x <= data.width; x++)
                    {
                        if (mask[x, y] != 0)
                        {
                            result[x, y] = terrainId;
                        }
                        else if (terrain[x, y] == terrainId)
                        {
                            result[x, y] = 0;
                        }
                    }
                }
            }

            data.terrainGrid = FlattenTerrain(result, data.width, data.height);
        }

        private static RimaMapDesignerWindow.MapSaveData Clone(RimaMapDesignerWindow.MapSaveData source)
        {
            if (source == null)
            {
                return null;
            }

            string json = JsonUtility.ToJson(source);
            return JsonUtility.FromJson<RimaMapDesignerWindow.MapSaveData>(json);
        }

        private void DrawPreview(Rect rect)
        {
            EnsurePreview();
            EditorGUI.DrawRect(rect, new Color(0.08f, 0.08f, 0.08f, 1f));
            if (previewData == null)
            {
                return;
            }

            float scale = Mathf.Min(rect.width / previewData.width, rect.height / previewData.height);
            float ox = rect.x + (rect.width - previewData.width * scale) * 0.5f;
            float oy = rect.y + (rect.height - previewData.height * scale) * 0.5f;

            if (previewData.terrainGrid != null && previewData.terrainGrid.Length > 0)
            {
                DrawTerrainGrid(rect, previewData.terrainGrid, previewData.width, previewData.height, scale, ox, oy);
                return;
            }

            if (previewData.layers != null)
            {
                DrawLayer(rect, previewData.layers.FirstOrDefault(l => l.name == "Base"), previewData.width, previewData.height, scale, ox, oy, new Color(0.17f, 0.16f, 0.15f), new Color(0.38f, 0.31f, 0.25f));
                DrawLayer(rect, previewData.layers.FirstOrDefault(l => l.name == "Path"), previewData.width, previewData.height, scale, ox, oy, new Color(0f, 0f, 0f, 0f), new Color(0.48f, 0.46f, 0.39f, 0.86f));
                DrawLayer(rect, previewData.layers.FirstOrDefault(l => l.name == "Rift"), previewData.width, previewData.height, scale, ox, oy, new Color(0f, 0f, 0f, 0f), new Color(0.18f, 0.66f, 0.88f, 0.9f));
            }
        }

        private static void DrawTerrainGrid(Rect clip, int[] terrainData, int w, int h, float scale, float ox, float oy)
        {
            int[,] grid = UnflattenTerrain(terrainData, w, h);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int nw = grid[x, y + 1];
                    int ne = grid[x + 1, y + 1];
                    int sw = grid[x, y];
                    int se = grid[x + 1, y];
                    int terrainId = Mathf.Max(Mathf.Max(nw, ne), Mathf.Max(sw, se));
                    Color color = TerrainColor(terrainId);
                    Rect cell = new Rect(ox + x * scale, oy + (h - y - 1) * scale, Mathf.Ceil(scale), Mathf.Ceil(scale));
                    if (clip.Overlaps(cell))
                    {
                        EditorGUI.DrawRect(cell, color);
                    }
                }
            }
        }

        private static Color TerrainColor(int terrainId)
        {
            switch (terrainId)
            {
                case 1:
                    return new Color(0.38f, 0.31f, 0.25f);
                case 2:
                    return new Color(0.48f, 0.46f, 0.39f, 0.86f);
                case 3:
                    return new Color(0.18f, 0.66f, 0.88f, 0.9f);
                default:
                    return new Color(0.17f, 0.16f, 0.15f);
            }
        }

        private static int[] FlattenTerrain(int[,] grid, int w, int h)
        {
            int[] values = new int[(w + 1) * (h + 1)];
            int index = 0;
            for (int y = 0; y <= h; y++)
            {
                for (int x = 0; x <= w; x++)
                {
                    values[index++] = grid != null && x < grid.GetLength(0) && y < grid.GetLength(1)
                        ? Mathf.Max(0, grid[x, y])
                        : 0;
                }
            }

            return values;
        }

        private static int[,] UnflattenTerrain(int[] values, int w, int h)
        {
            int[,] grid = new int[w + 1, h + 1];
            int index = 0;
            for (int y = 0; y <= h; y++)
            {
                for (int x = 0; x <= w; x++)
                {
                    grid[x, y] = values != null && index < values.Length ? Mathf.Max(0, values[index]) : 0;
                    index++;
                }
            }

            return grid;
        }

        private static void DrawLayer(Rect clip, RimaMapDesignerWindow.LayerSaveData layer, int w, int h, float scale, float ox, float oy, Color zero, Color one)
        {
            if (layer == null || layer.vertexData == null)
            {
                return;
            }

            int[,] grid = RoomVariationProcessor.Unflatten(layer.vertexData, w, h);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    int nw = grid[x, y + 1];
                    int ne = grid[x + 1, y + 1];
                    int sw = grid[x, y];
                    int se = grid[x + 1, y];
                    int sum = nw + ne + sw + se;
                    Color color = sum >= 2 ? one : zero;
                    if (color.a <= 0f)
                    {
                        continue;
                    }

                    Rect cell = new Rect(ox + x * scale, oy + (h - y - 1) * scale, Mathf.Ceil(scale), Mathf.Ceil(scale));
                    if (clip.Overlaps(cell))
                    {
                        EditorGUI.DrawRect(cell, color);
                    }
                }
            }
        }

        private static string VariationLabel(RoomVariationProcessor.Level level)
        {
            int value = (int)level;
            if (value <= 2)
            {
                return "Subtle";
            }

            return value <= 4 ? "Medium" : "Wild";
        }
    }
}
