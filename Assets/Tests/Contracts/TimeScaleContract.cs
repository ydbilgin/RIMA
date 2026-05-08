namespace RIMA.Tests.Contracts
{
    public static class TimeScaleContract
    {
        public const float GameRunning = 1f;
        public const float GamePaused = 0f;
        public const float BootstrapDeadline = 0.5f; // seconds — must be 1 after scene load

        // Classes that set timeScale=0 — each must have Hide()/Close() that restores it
        public static readonly string[] PausingClasses =
            { "SkillOfferUI", "SettingsMenuUI", "ForgeUI", "ChestUI", "DeathScreenManager" };
    }
}
