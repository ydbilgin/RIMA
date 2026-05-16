using System;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [Serializable]
    public class PlayerSpawnSocket
    {
        public string socketId;
        public Vector2Int position;
        public RIMA.DoorDirection facing = RIMA.DoorDirection.South;
    }
}
