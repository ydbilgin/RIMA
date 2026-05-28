using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Encounter
{
    public enum EncounterEnemyType
    {
        FractureImp,
        ShardWalker,
        SeamCrawler,
        PenitentBruiser,
        ChainWardenEcho,
        RelicCaster,
        RiftboundAugur,
        HollowHulk,
        VoidThrall,
        Other
    }

    [Serializable]
    public class EncounterEnemyEntry
    {
        public EncounterEnemyType enemyType;
        public GameObject prefab;
        [Min(0)] public int count = 1;
        [Min(0f)] public float threatCost = 1f;
        [Min(0f)] public float weight = 1f;
        [Min(1)] public int maxSimultaneous = 4;
        public bool t2Capable;
        public bool eliteOnly;
    }

    [CreateAssetMenu(menuName = "RIMA/Encounter/Encounter Wave", fileName = "EncounterWave_New", order = 220)]
    public class EncounterWaveSO : ScriptableObject
    {
        [Min(0f)] public float threatBudget = 10f;
        [Range(0f, 1f)] public float openingBudgetFraction = 0.4f;
        [Range(0f, 1f)] public float nextWaveKillFraction = 0.5f;
        [Min(0)] public int normalRoomT2Cap = 2;
        [Min(0)] public int eliteRoomT2Cap = 3;
        public List<EncounterEnemyEntry> entries = new List<EncounterEnemyEntry>();
    }
}
