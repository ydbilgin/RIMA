namespace RIMA.Balance
{
    [System.Serializable]
    public class ClassStatRuntime
    {
        public static readonly ClassStatRuntime Neutral = new();

        public ClassType classType = ClassType.None;
        public string archetype = "Neutral";
        public DamageType primaryDamageType = DamageType.Physical;

        public int maxHP = 100;
        public float physPower = 100f;
        public float abilityPower = 100f;
        public float attackSpeedMult = 1f;
        public float moveSpeed = 4.5f;
        public float armor;
        public float magicResist;

        public int damage = 3;
        public int durability = 3;
        public int speed = 3;
        public int control = 3;
        public int difficulty = 3;

        public float debugGlobalDamageMult = 1f;
        public float identityBuildMultiplier = 1f;
        public float situationalMultiplier = 1f;

        public ClassStatRuntime() { }

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
            armor = profile.armor;
            magicResist = profile.magicResist;

            damage = profile.damage;
            durability = profile.durability;
            speed = profile.speed;
            control = profile.control;
            difficulty = profile.difficulty;

            debugGlobalDamageMult = profile.debugGlobalDamageMult;
        }
    }
}
