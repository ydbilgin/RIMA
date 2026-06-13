namespace RIMA.Balance
{
    [System.Serializable]
    public class ClassStatRuntime
    {
        public ClassType classType;
        public string archetype;
        public DamageType primaryDamageType;

        public int maxHP;
        public float physPower;
        public float abilityPower;
        public float attackSpeedMult;
        public float moveSpeed;

        public int damage;
        public int durability;
        public int speed;
        public int control;
        public int difficulty;

        public float debugGlobalDamageMult;

        // Runtime modifiers. Keep default 1.0 until item/trait/posture systems hook in.
        public float identityBuildMultiplier = 1f;
        public float situationalMultiplier = 1f;

        public ClassStatRuntime(ClassStatProfile profile)
        {
            classType = profile.classType;
            archetype = profile.archetype;
            primaryDamageType = profile.primaryDamageType;

            maxHP = profile.maxHP;
            physPower = profile.physPower;
            abilityPower = profile.abilityPower;
            attackSpeedMult = profile.attackSpeedMult;
            moveSpeed = profile.moveSpeed;

            damage = profile.damage;
            durability = profile.durability;
            speed = profile.speed;
            control = profile.control;
            difficulty = profile.difficulty;

            debugGlobalDamageMult = profile.debugGlobalDamageMult;
        }
    }
}
