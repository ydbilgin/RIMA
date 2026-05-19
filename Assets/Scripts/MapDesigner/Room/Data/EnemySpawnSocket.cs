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
        public float avoidRadius = 1.5f;
        // TODO: MapLayerOrchestrator avoid-radius consumer - separate spec.
    }
}
