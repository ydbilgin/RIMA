using UnityEngine;

namespace RIMA.Environment.Modular
{
    [CreateAssetMenu(fileName = "RF_NewFootprint", menuName = "RIMA/Environment/Room Footprint")]
    public class RoomFootprint : ScriptableObject
    {
        public int widthCells = 8;
        public int heightCells = 8;
        public float cellSize = 2f;

        [TextArea(4, 16)]
        public string occupancyAscii = "";

        public bool[] GetOccupancyGrid()
        {
            var grid = new bool[widthCells * heightCells];
            if (string.IsNullOrEmpty(occupancyAscii))
            {
                for (int i = 0; i < grid.Length; i++)
                {
                    grid[i] = true;
                }

                return grid;
            }

            var rows = occupancyAscii.Replace("\r", "").Split('\n');
            for (int z = 0; z < heightCells && z < rows.Length; z++)
            {
                var row = rows[heightCells - 1 - z];
                for (int x = 0; x < widthCells && x < row.Length; x++)
                {
                    grid[z * widthCells + x] = row[x] == '#';
                }
            }

            return grid;
        }
    }
}
