using UnityEngine;
// Adjust namespace/import for your existing ClassType location.
// using RIMA.Skills;

namespace RIMA.Balance
{
    [CreateAssetMenu(menuName = "RIMA/Balance/Class Stat Profile", fileName = "NewClassStatProfile")]
    public class ClassStatProfile : ScriptableObject
    {
        [Header("Identity")]
        public ClassType classType;
        public string archetype;
        public DamageType primaryDamageType = DamageType.Physical;

        [Header("Runtime Stats")]
        public int maxHP = 100;
        public float physPower = 100f;
        public float abilityPower = 100f;
        public float attackSpeedMult = 1f;
        public float moveSpeed = 4.5f;

        [Header("UI 5-Bar")]
        [Range(1, 5)] public int damage = 3;
        [Range(1, 5)] public int durability = 3;
        [Range(1, 5)] public int speed = 3;
        [Range(1, 5)] public int control = 3;
        [Range(1, 5)] public int difficulty = 3;

        [Header("Debug Only")]
        [Tooltip("Presenter/debug override only. Do not use as production class stat.")]
        public float debugGlobalDamageMult = 1f;

        public ClassStatRuntime CreateRuntimeCopy()
        {
            return new ClassStatRuntime(this);
        }
    }
}
