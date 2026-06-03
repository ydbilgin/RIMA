using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.DevTools
{
    public static class ClearTilemaps
    {
        [MenuItem("RIMA/Utilities/Clear All Tilemap Tiles")]
        public static void ClearAllTilemapTiles()
        {
            Tilemap[] tilemaps = Object.FindObjectsByType<Tilemap>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            int clearedCount = 0;
            int totalRemoved = 0;

            foreach (Tilemap tilemap in tilemaps)
            {
                int tileCount = CountTiles(tilemap);
                Debug.Log($"ClearTilemaps: {tilemap.name} had {tileCount} tiles.");

                tilemap.ClearAllTiles();
                clearedCount++;
                totalRemoved += tileCount;
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            Debug.Log($"ClearTilemaps: cleared {clearedCount} tilemaps, removed {totalRemoved} tiles.");
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        private static int CountTiles(Tilemap tilemap)
        {
            int count = 0;

            foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(position))
                {
                    count++;
                }
            }

            return count;
        }
    }
}
