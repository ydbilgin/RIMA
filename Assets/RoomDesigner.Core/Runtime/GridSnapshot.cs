using UnityEngine;

namespace RIMA.RoomDesigner.Core
{
    public readonly struct GridSnapshot
    {
        public GridSnapshot(byte[] floorMask, byte[] wallMask, int width, int height, Vector3Int origin, int generatorVersion)
        {
            this.floorMask = floorMask ?? System.Array.Empty<byte>();
            this.wallMask = wallMask ?? System.Array.Empty<byte>();
            this.width = width;
            this.height = height;
            this.origin = origin;
            this.generatorVersion = generatorVersion;
        }

        public readonly byte[] floorMask;
        public readonly byte[] wallMask;
        public readonly int width;
        public readonly int height;
        public readonly Vector3Int origin;
        public readonly int generatorVersion;
    }
}
