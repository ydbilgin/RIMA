using System;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [Serializable]
    public class EnemySpawnSocket
    {
        public string socketId;
        public Vector2Int position;
        public string tierHint = "standard";
    }
}
