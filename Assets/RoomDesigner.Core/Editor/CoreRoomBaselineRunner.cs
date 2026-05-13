using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.RoomDesigner.Core.Editor
{
    public static class CoreRoomBaselineRunner
    {
        public static GridSnapshot Run(RoomBaselineGeneratorBase generator, GenerationInput input, Tilemap floor, Tilemap wall)
        {
            if (generator == null)
            {
                throw new System.ArgumentNullException(nameof(generator));
            }

            if (floor == null)
            {
                throw new System.ArgumentNullException(nameof(floor));
            }

            if (wall == null)
            {
                throw new System.ArgumentNullException(nameof(wall));
            }

            GridSnapshot snapshot = generator.Generate(input);
            Apply(snapshot, floor, wall);
            return snapshot;
        }

        private static void Apply(GridSnapshot snapshot, Tilemap floor, Tilemap wall)
        {
            floor.ClearAllTiles();
            wall.ClearAllTiles();

            TileBase floorTile = ScriptableObject.CreateInstance<Tile>();
            floorTile.hideFlags = HideFlags.HideAndDontSave;
            TileBase wallTile = ScriptableObject.CreateInstance<Tile>();
            wallTile.hideFlags = HideFlags.HideAndDontSave;

            for (int y = 0; y < snapshot.height; y++)
            {
                for (int x = 0; x < snapshot.width; x++)
                {
                    int index = y * snapshot.width + x;
                    Vector3Int cell = new Vector3Int(snapshot.origin.x + x, snapshot.origin.y + y, snapshot.origin.z);

                    if (index < snapshot.floorMask.Length && snapshot.floorMask[index] != 0)
                    {
                        floor.SetTile(cell, floorTile);
                    }

                    if (index < snapshot.wallMask.Length && snapshot.wallMask[index] != 0)
                    {
                        wall.SetTile(cell, wallTile);
                    }
                }
            }

            floor.RefreshAllTiles();
            wall.RefreshAllTiles();
        }
    }
}
