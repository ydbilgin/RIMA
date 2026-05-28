using System;
using UnityEngine;

namespace RIMA.Systems.Map
{
    [CreateAssetMenu(fileName = "Phase1_Room", menuName = "RIMA/Phase1/Room Sequence Data")]
    public class RoomSequenceData : ScriptableObject
    {
        [Header("Identity")]
        public int roomIndex;
        public string displayName;

        [Header("Player")]
        public Vector3 playerStartPos;

        [Header("Mob Spawn")]
        public EnemySpawnEntry[] mobSpawns;

        [Header("Gate")]
        public Vector3 gatePosition;
        public Vector2 gateSize;

        [Header("Focal Element")]
        public GameObject focalElementPrefab;
        public Vector3 focalElementPos;

        [Header("Cliff Pattern (optional)")]
        public string cliffPatternKey;

        [Header("Misc")]
        public bool isBossRoom;
        public bool isRewardRoom;
        public float expectedDuration;

        [Header("Fragment Drop")]
        public Vector3 fragmentDropOverride;

        [Serializable]
        public class EnemySpawnEntry
        {
            public GameObject prefab;
            public Vector3 position;
            public bool isElite;
        }
    }
}
