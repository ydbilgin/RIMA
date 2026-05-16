using System;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [Serializable]
    public class DoorSocket
    {
        public string socketId;
        public Vector2Int position;
        public RIMA.DoorDirection direction;
        public int widthInTiles = 2;
        public bool isExit = true;
    }
}
