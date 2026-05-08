using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace RIMA.Editor
{
    public static class F1TileSetup
    {
        private const string F1TileFolder = "Assets/Art/Tiles/Act1/F1";

        [MenuItem("RIMA/Setup F1 Tiles")]
        public static void SetupF1Tiles()
        {
            string[] spriteGuids = AssetDatabase.FindAssets("t:Sprite", new[] { F1TileFolder });
            List<string> spritePaths = new List<string>(spriteGuids.Length);

            foreach (string spriteGuid in spriteGuids)
            {
                spritePaths.Add(AssetDatabase.GUIDToAssetPath(spriteGuid));
            }

            spritePaths.Sort(System.StringComparer.Ordinal);

            List<TileBase> tiles = new List<TileBase>(spritePaths.Count);

            foreach (string spritePath in spritePaths)
            {
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

                if (sprite == null)
                {
                    continue;
                }

                string tilePath = Path.Combine(F1TileFolder, $"{sprite.name}.asset").Replace('\\', '/');

                if (AssetDatabase.LoadAssetAtPath<TileBase>(tilePath) == null)
                {
                    Tile tile = ScriptableObject.CreateInstance<Tile>();
                    tile.sprite = sprite;
                    AssetDatabase.CreateAsset(tile, tilePath);
                }

                TileBase tileBase = AssetDatabase.LoadAssetAtPath<TileBase>(tilePath);

                if (tileBase != null)
                {
                    tiles.Add(tileBase);
                }
            }

            RIMA.Systems.Map.DungeonLayerManager dungeonLayerManager =
                Object.FindAnyObjectByType<RIMA.Systems.Map.DungeonLayerManager>();

            if (dungeonLayerManager == null)
            {
                Debug.LogError("F1TileSetup: DungeonLayerManager not found in current scene");
                return;
            }

            TileBase[] tileArray = tiles.ToArray();
            dungeonLayerManager.f1FloorTiles = tileArray;
            EditorUtility.SetDirty(dungeonLayerManager);
            AssetDatabase.SaveAssets();

            Debug.Log($"F1TileSetup: assigned {tileArray.Length} tiles to DungeonLayerManager.f1FloorTiles");
        }
    }
}
