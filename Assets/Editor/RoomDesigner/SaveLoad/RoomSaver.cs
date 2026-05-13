namespace RIMA.Editor.RoomDesigner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using RIMA.Runtime.Rooms;
    using RIMA.Systems.Map;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public static class RoomSaver
    {
        public static (string prefabPath, string blueprintPath) Save(GameObject roomRoot, string roomId, BiomeType biome, int noiseSeed = 0, int roomWidth = 20, int roomHeight = 20)
        {
            if (roomRoot == null)
            {
                throw new ArgumentNullException(nameof(roomRoot));
            }

            if (string.IsNullOrWhiteSpace(roomId))
            {
                throw new ArgumentException("Room id cannot be empty.", nameof(roomId));
            }

            string dir = string.Format("Assets/_Generated/Rooms/{0}", biome.ToString());
            Directory.CreateDirectory(dir);
            string soPath = string.Format("{0}/{1}.asset", dir, roomId);
            string prefabPath = string.Format("{0}/{1}.prefab", dir, roomId);
            var made = new List<string>();

            try
            {
                var bp = ScriptableObject.CreateInstance<RoomBlueprint>();
                AssetDatabase.CreateAsset(bp, soPath);
                made.Add(soPath);
                bp.biomeType = biome;
                bp.noiseSeed = noiseSeed;
                bp.roomWidth = roomWidth;
                bp.roomHeight = roomHeight;

                PrefabUtility.SaveAsPrefabAsset(roomRoot, prefabPath, out bool ok);
                if (File.Exists(prefabPath))
                {
                    made.Add(prefabPath);
                }

                if (!ok)
                {
                    throw new InvalidOperationException("Prefab save failed");
                }

                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                bp.prefab = prefab;
                bp.roomId = roomId;
                bp.roomType = RoomType.Combat.ToString();

                var link = prefab.GetComponent<RoomPrefabLink>() ?? prefab.AddComponent<RoomPrefabLink>();
                link.blueprint = bp;

                var config = prefab.GetComponent<RoomConfig>() ?? prefab.AddComponent<RoomConfig>();
                config.roomId = bp.roomId;
                config.roomType = ParseRoomType(bp.roomType);
                config.depthBandMin = 0;
                config.depthBandMax = 99;

                Grid baseGrid = roomRoot.GetComponent<Grid>();
                config.cellSize = baseGrid != null ? baseGrid.cellSize : new Vector3(1f, 0.5f, 0f);
                config.gridLayout = GridLayout.CellLayout.Isometric;
                config.orientation = GridLayout.CellSwizzle.XYZ;

                EditorUtility.SetDirty(bp);
                EditorUtility.SetDirty(prefab);
                PrefabUtility.SavePrefabAsset(prefab);
                AssetDatabase.SaveAssets();
                return (prefabPath, soPath);
            }
            catch
            {
                foreach (var p in made)
                {
                    AssetDatabase.DeleteAsset(p);
                }

                throw;
            }
        }

        private static RoomType ParseRoomType(string value)
        {
            return Enum.TryParse(value, true, out RoomType parsed) ? parsed : RoomType.Combat;
        }
    }
}
