using System.Collections.Generic;
using System.Linq;
using RIMA;
using RIMA.Systems.Map;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor
{
    public class BiomeQuickEditorWindow : EditorWindow
    {
        private RimaBiomePreset biome;
        private string originalJson;
        private Vector2 scroll;

        public static void Open(RimaBiomePreset biomePreset)
        {
            if (biomePreset == null)
            {
                return;
            }

            BiomeQuickEditorWindow window = GetWindow<BiomeQuickEditorWindow>(true, "Biome Quick Editor");
            window.biome = biomePreset;
            window.originalJson = JsonUtility.ToJson(biomePreset);
            window.minSize = new Vector2(640f, 520f);
            window.Show();
        }

        private void OnGUI()
        {
            if (biome == null)
            {
                EditorGUILayout.HelpBox("No biome selected.", MessageType.Info);
                if (GUILayout.Button("Close"))
                {
                    Close();
                }

                return;
            }

            if (biome.terrains == null)
            {
                biome.terrains = new List<MapTerrain>();
            }

            if (biome.tilesetPairings == null)
            {
                biome.tilesetPairings = new List<TilesetPairing>();
            }

            scroll = EditorGUILayout.BeginScrollView(scroll);
            EditorGUILayout.LabelField(biome.name, EditorStyles.boldLabel);
            EditorGUILayout.Space(8f);
            DrawTerrains();
            EditorGUILayout.Space(12f);
            DrawPairings();
            EditorGUILayout.EndScrollView();

            DrawFooter();
        }

        private void DrawTerrains()
        {
            EditorGUILayout.LabelField("Terrains", EditorStyles.boldLabel);
            for (int i = 0; i < biome.terrains.Count; i++)
            {
                MapTerrain terrain = biome.terrains[i];
                if (terrain == null)
                {
                    terrain = new MapTerrain { id = NextTerrainId() };
                    biome.terrains[i] = terrain;
                }

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("id=" + terrain.id, GUILayout.Width(52f));
                EditorGUI.BeginChangeCheck();
                string newName = EditorGUILayout.TextField(terrain.name);
                Color newColor = EditorGUILayout.ColorField(terrain.paletteColor, GUILayout.Width(76f));
                TileBase newTile = (TileBase)EditorGUILayout.ObjectField(terrain.baseTile, typeof(TileBase), false, GUILayout.Width(150f));
                bool delete = GUILayout.Button("Delete", GUILayout.Width(64f));
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(biome, "Edit Biome Terrain");
                    terrain.name = newName;
                    terrain.paletteColor = newColor;
                    terrain.baseTile = newTile;
                    EditorUtility.SetDirty(biome);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

                if (delete && EditorUtility.DisplayDialog("Delete Terrain", "Delete terrain id " + terrain.id + "?", "Delete", "Cancel"))
                {
                    Undo.RecordObject(biome, "Delete Biome Terrain");
                    biome.terrains.RemoveAt(i);
                    biome.tilesetPairings.RemoveAll(p => p != null && (p.lowerTerrainId == terrain.id || p.upperTerrainId == terrain.id));
                    EditorUtility.SetDirty(biome);
                    GUIUtility.ExitGUI();
                }
            }
        }

        private void DrawPairings()
        {
            EditorGUILayout.LabelField("Pairings", EditorStyles.boldLabel);
            string[] names = TerrainPopupNames();
            int[] ids = TerrainIds();

            for (int i = 0; i < biome.tilesetPairings.Count; i++)
            {
                TilesetPairing pairing = biome.tilesetPairings[i];
                if (pairing == null)
                {
                    pairing = new TilesetPairing();
                    biome.tilesetPairings[i] = pairing;
                }

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.BeginHorizontal();
                pairing.lowerTerrainId = DrawTerrainPopup("Lower", pairing.lowerTerrainId, ids, names);
                pairing.upperTerrainId = DrawTerrainPopup("Upper", pairing.upperTerrainId, ids, names);
                bool delete = GUILayout.Button("Delete", GUILayout.Width(64f));
                EditorGUILayout.EndHorizontal();

                pairing.tileSet = (CornerWangTileSetSO)EditorGUILayout.ObjectField("TileSet", pairing.tileSet, typeof(CornerWangTileSetSO), false);
                pairing.transitionSize = EditorGUILayout.Slider("Transition Size", pairing.transitionSize, 0f, 1f);
                pairing.transitionDescription = EditorGUILayout.TextArea(pairing.transitionDescription, GUILayout.MinHeight(42f));
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(biome, "Edit Biome Pairing");
                    EditorUtility.SetDirty(biome);
                }

                EditorGUILayout.EndVertical();

                if (delete)
                {
                    Undo.RecordObject(biome, "Delete Biome Pairing");
                    biome.tilesetPairings.RemoveAt(i);
                    EditorUtility.SetDirty(biome);
                    GUIUtility.ExitGUI();
                }
            }
        }

        private void DrawFooter()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button("+ Add Terrain", EditorStyles.toolbarButton, GUILayout.Width(100f)))
            {
                Undo.RecordObject(biome, "Add Biome Terrain");
                biome.terrains.Add(new MapTerrain
                {
                    id = NextTerrainId(),
                    name = "Terrain " + NextTerrainId(),
                    paletteColor = Color.gray
                });
                EditorUtility.SetDirty(biome);
            }

            if (GUILayout.Button("+ Add Pairing", EditorStyles.toolbarButton, GUILayout.Width(100f)))
            {
                Undo.RecordObject(biome, "Add Biome Pairing");
                int lower = biome.terrains.Count > 0 ? biome.terrains[0].id : 0;
                int upper = biome.terrains.Count > 1 ? biome.terrains[1].id : lower;
                biome.tilesetPairings.Add(new TilesetPairing
                {
                    lowerTerrainId = lower,
                    upperTerrainId = upper,
                    transitionSize = 0.25f
                });
                EditorUtility.SetDirty(biome);
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(64f)))
            {
                EditorUtility.SetDirty(biome);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Close();
            }

            if (GUILayout.Button("Cancel", EditorStyles.toolbarButton, GUILayout.Width(64f)))
            {
                if (!string.IsNullOrEmpty(originalJson))
                {
                    Undo.RecordObject(biome, "Cancel Biome Edits");
                    JsonUtility.FromJsonOverwrite(originalJson, biome);
                    EditorUtility.SetDirty(biome);
                }

                Close();
            }

            EditorGUILayout.EndHorizontal();
        }

        private int DrawTerrainPopup(string label, int currentId, int[] ids, string[] names)
        {
            EditorGUILayout.LabelField(label, GUILayout.Width(42f));
            if (ids.Length == 0)
            {
                EditorGUILayout.Popup(0, new[] { "None" });
                return currentId;
            }

            int index = Mathf.Max(0, System.Array.IndexOf(ids, currentId));
            int nextIndex = EditorGUILayout.Popup(index, names);
            return ids[Mathf.Clamp(nextIndex, 0, ids.Length - 1)];
        }

        private int NextTerrainId()
        {
            if (biome == null || biome.terrains == null || biome.terrains.Count == 0)
            {
                return 0;
            }

            return biome.terrains.Where(t => t != null).Select(t => t.id).DefaultIfEmpty(-1).Max() + 1;
        }

        private int[] TerrainIds()
        {
            return biome.terrains
                .Where(t => t != null)
                .Select(t => t.id)
                .ToArray();
        }

        private string[] TerrainPopupNames()
        {
            return biome.terrains
                .Where(t => t != null)
                .Select(t => t.id + " - " + (!string.IsNullOrEmpty(t.name) ? t.name : "Terrain " + t.id))
                .ToArray();
        }
    }
}
