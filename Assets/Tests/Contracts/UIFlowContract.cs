namespace RIMA.Tests.Contracts
{
    public static class UIFlowContract
    {
        // Every class that sets timeScale=0 must have a paired close method that restores it
        public static readonly (string className, string closeMethod)[] PauseRestorePairs =
        {
            ("SkillOfferUI",   "Hide"),
            ("SettingsMenuUI", "Close"),
            ("ForgeUI",        "Hide"),
            ("ChestUI",        "Hide"),
            ("PauseMenuUI",    "Close"),
        };

        // MainMenuScreen AutoInit guard field (static bool prevents double-init on domain reload)
        public const string MainMenuGuardField = "_gameStarted";
    }
}
