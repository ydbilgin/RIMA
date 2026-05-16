using System;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [Serializable]
    public struct CameraBounds
    {
        public RectInt tileRect;

        public static CameraBounds FromBounds(RectInt bounds)
        {
            return new CameraBounds { tileRect = bounds };
        }
    }
}
