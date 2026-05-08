namespace RIMA.Tests.Contracts
{
    public static class BootstrapContract
    {
        public const string GameSceneName = "_IsoGame";
        public const string MenuSceneName = "MainMenu";
        public const float MaxBootstrapTime = 0.5f;

        // Components that must exist on the Player GameObject
        public static readonly string[] RequiredPlayerComponents =
            { "PlayerController", "PlayerAttack", "Health", "Rigidbody2D" };

        // SerializeField assignments that must not be null at runtime
        public static readonly (string className, string fieldName)[] RequiredAssignments =
        {
            ("PlayerAttack", "basicAttackProfile"),
        };
    }
}
