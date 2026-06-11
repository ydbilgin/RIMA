using UnityEngine;

namespace RIMA.Balance
{
    [System.Serializable]
    public struct DamagePacket
    {
        public int baseDamage;
        public DamageType damageType;
        public ElementTag elementTag;
        public DamageSourceType sourceType;
        public GameObject attacker;
        public GameObject target;
        public bool isCrit;
        public bool bypassStatScaling;
        public float critMultiplier;
        public string sourceId;

        public static DamagePacket Create(
            int baseDamage,
            DamageType damageType = DamageType.Physical,
            DamageSourceType sourceType = DamageSourceType.Unknown,
            GameObject attacker = null,
            GameObject target = null,
            string sourceId = "",
            bool isCrit = false,
            float critMultiplier = 1.5f,
            ElementTag elementTag = ElementTag.None,
            bool bypassStatScaling = false)
        {
            return new DamagePacket
            {
                baseDamage = baseDamage,
                damageType = damageType,
                elementTag = elementTag,
                sourceType = sourceType,
                attacker = attacker,
                target = target,
                sourceId = sourceId,
                isCrit = isCrit,
                bypassStatScaling = bypassStatScaling,
                critMultiplier = critMultiplier
            };
        }
    }
}
