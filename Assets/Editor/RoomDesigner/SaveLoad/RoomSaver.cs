namespace RIMA.Editor.RoomDesigner
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using RIMA.Runtime.Rooms;
    using UnityEditor;
    using UnityEngine;

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

                var link = prefab.GetComponent<RoomPrefabLink>() ?? prefab.AddComponent<RoomPrefabLink>();
                link.blueprint = bp;

                EditorUtility.SetDirty(bp);
                EditorUtility.SetDirty(prefab);
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
    }
}
