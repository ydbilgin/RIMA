using UnityEngine;

namespace RIMA.Balance
{
    [System.Serializable]
    public struct DamagePacket
    {
        public int baseDamage;
        public DamageType damageType;
        public DamageSourceType sourceType;

        public GameObject attacker;
        public GameObject target;

        public bool canCrit;
        public string sourceId;
        public string elementTag;

        public static DamagePacket Create(
            int baseDamage,
            DamageType damageType,
            DamageSourceType sourceType,
            GameObject attacker,
            GameObject target,
            string sourceId = "",
            bool canCrit = false,
            string elementTag = "")
        {
            return new DamagePacket
            {
                baseDamage = baseDamage,
                damageType = damageType,
                sourceType = sourceType,
                attacker = attacker,
                target = target,
                sourceId = sourceId,
                canCrit = canCrit,
                elementTag = elementTag
            };
        }
    }
}
