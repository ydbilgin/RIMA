#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RIMA
{
    public static class ObstaclePrefabBuilder
    {
        private const string PrefabDir = "Assets/Prefabs/Obstacles";

        [MenuItem("RIMA/4. Create Obstacle Prefabs")]
        public static void CreatePrefabs()
        {
            EnsureFolder(PrefabDir);

            // 1. StoneColumn
            CreatePrefab<StoneColumn>("StoneColumn", new Color(0.5f, 0.5f, 0.5f, 1f), false);

            // 2. NarrowPassage
            CreatePrefab<NarrowPassage>("NarrowPassage", new Color(0.3f, 0.3f, 0.3f), true);

            // 3. Chasm
            CreatePrefab<Chasm>("Chasm", new Color(0.3f, 0f, 0.5f, 1f), true); // Koyu mor

            AssetDatabase.SaveAssets();
            Debug.Log("[ObstaclePrefabBuilder] Obstacle prefabs created successfully.");
        }

        private static void CreatePrefab<T>(string name, Color color, bool isTrigger) where T : ObstacleBase
        {
            string prefabPath = $"{PrefabDir}/{name}.prefab";
            
            // Create a temporary GameObject
            GameObject go = new GameObject(name);
            go.layer = LayerMask.NameToLayer("Obstacle");

            // Add SpriteRenderer — Entities layer so it renders above floor
            var sr = go.AddComponent<SpriteRenderer>();
            sr.color = color;
            sr.sortingLayerName = "Entities";
            sr.sortingOrder = 5;

            // PlaceholderSprite — generates visible placeholder when no real sprite
            go.AddComponent<PlaceholderSprite>();

            // Add BoxCollider2D
            var col = go.AddComponent<BoxCollider2D>();
            col.isTrigger = isTrigger;
            
            // Add custom script component
            go.AddComponent<T>();

            // Save as Prefab
            PrefabUtility.SaveAsPrefabAsset(go, prefabPath);

            // Cleanup the temporary GameObject
            Object.DestroyImmediate(go);
        }

        private static void EnsureFolder(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath)) return;
            var parts = folderPath.Split('/');
            var cursor = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                var next = cursor + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(cursor, parts[i]);
                }
                cursor = next;
            }
        }
    }
}
#endif
