using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Runtime.Encounter
{
    public class SubRoomEnemyPlan
    {
        public List<EnemyAssignment> Enemies = new List<EnemyAssignment>();
    }

    public class EnemyAssignment
    {
        public string socketId;
        public GameObject enemyPrefab;
        public bool isElite;
    }
}
