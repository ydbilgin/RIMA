namespace RIMA.Tests.Contracts
{
    public static class CombatContract
    {
        // Every class implementing IBasicAttackBehavior must provide these methods
        public static readonly string[] RequiredBehaviorMethods =
            { "OnUpdate", "OnLMBInput", "OnRMBInput" };

        // BasicAttackProfile fields that must not be zero/null/empty
        public static readonly string[] RequiredProfileFields =
            { "comboLength", "comboDamage", "hitRange", "hitRadius" };

        public const int MinComboLength = 1;
        public const int MaxComboLength = 5;

        // Number of enum values in BasicAttackBehaviorType that have explicit CreateBehavior() cases.
        // HeatGauge and MarkPulse fall through to the default (MeleeChainBehavior).
        // This constant must be updated whenever a new explicit case is added to the switch.
        // Current explicit cases: CastRhythm, ShotCadence, VeilStrike = 3
        public const int ExplicitBehaviorCases = 3;
    }
}
