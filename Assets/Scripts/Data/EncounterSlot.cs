using System;
using UnityEngine;

namespace RIMA.Data
{
    [Serializable]
    public struct EncounterSlot
    {
        public Vector2Int gridPos;
        public string slotType;
        public GameObject prefabHint;
    }
}
