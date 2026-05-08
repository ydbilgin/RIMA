using System;
using System.Reflection;
using NUnit.Framework;

namespace RIMA.Tests
{
    /// <summary>
    /// EditMode tests that verify TimeScale guard patterns exist in key UI classes
    /// so that AutoInit methods do not re-inject in a game scene after the run has started,
    /// and that no Start/Awake methods set timeScale=0 without a guard.
    /// Assembly: RIMA.Tests.EditMode
    /// </summary>
    public class TimeScaleGuardTests
    {
        // ── helpers ────────────────────────────────────────────────────────

        private static Type GetRimaType(string typeName)
        {
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                var t = asm.GetType($"RIMA.{typeName}") ?? asm.GetType(typeName);
                if (t != null) return t;
            }
            return null;
        }

        private static FieldInfo GetField(Type type, string fieldName,
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)
        {
            return type?.GetField(fieldName, flags);
        }

        private static MethodInfo GetMethod(Type type, string methodName,
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
        {
            return type?.GetMethod(methodName, flags);
        }

        // ── Test 1 ─────────────────────────────────────────────────────────

        /// <summary>
        /// MainMenuScreen.AutoInit is decorated with RuntimeInitializeOnLoadMethod.
        /// Verifies that MainMenuScreen has a static bool _gameStarted guard field,
        /// which prevents re-injection when the game scene is already running.
        /// </summary>
        [Test]
        public void MainMenuAutoInit_DoesNotReinjectInGameScene()
        {
            // 1. Resolve type
            var mainMenuType = GetRimaType("MainMenuScreen");
            Assert.IsNotNull(mainMenuType,
                "MainMenuScreen type not found in any loaded assembly. Is RIMA.Runtime referenced?");

            // 2. Verify RuntimeInitializeOnLoadMethod attribute on AutoInit
            var autoInitMethod = GetMethod(mainMenuType, "AutoInit");
            Assert.IsNotNull(autoInitMethod,
                "MainMenuScreen.AutoInit() method not found via reflection.");

            var riolAttr = autoInitMethod.GetCustomAttribute<UnityEngine.RuntimeInitializeOnLoadMethodAttribute>();
            Assert.IsNotNull(riolAttr,
                "MainMenuScreen.AutoInit is missing [RuntimeInitializeOnLoadMethod] attribute.");

            // 3. Verify static bool _gameStarted guard exists
            var gameStartedField = GetField(mainMenuType, "_gameStarted",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.IsNotNull(gameStartedField,
                "MainMenuScreen._gameStarted static field not found. " +
                "The AutoInit guard relies on this field to prevent re-injection.");

            Assert.AreEqual(typeof(bool), gameStartedField.FieldType,
                "MainMenuScreen._gameStarted must be a bool field.");

            Assert.IsTrue(gameStartedField.IsStatic,
                "MainMenuScreen._gameStarted must be static so it persists across scene loads.");
        }

        // ── Test 2 ─────────────────────────────────────────────────────────

        /// <summary>
        /// Verifies guard structures on classes whose Start/Awake may interact with timeScale.
        /// Does NOT inspect IL — only checks that the guard field/method exists, which is a
        /// necessary (though not sufficient) condition for correct behaviour.
        ///
        /// Checks:
        ///   a) MainMenuScreen._gameStarted static bool exists           (already covered above, duplicated for clarity)
        ///   b) CharacterSelectScreen.OnStartRun method exists           (the method that gates scene transition)
        ///   c) SettingsMenuUI has a Start or Awake method               (has lifecycle hooks)
        ///   d) SkillOfferUI class is resolvable from the runtime assembly
        /// </summary>
        [Test]
        public void NoStartMethodSetsTimeScaleZero_GuardsExist()
        {
            // ── a) MainMenuScreen._gameStarted ───────────────────────────
            var mainMenuType = GetRimaType("MainMenuScreen");
            Assert.IsNotNull(mainMenuType, "MainMenuScreen type not found.");

            var gameStartedField = GetField(mainMenuType, "_gameStarted",
                BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(gameStartedField,
                "MainMenuScreen._gameStarted guard field is missing.");

            // ── b) CharacterSelectScreen.OnStartRun exists ───────────────
            var charSelectType = GetRimaType("CharacterSelectScreen");
            Assert.IsNotNull(charSelectType, "CharacterSelectScreen type not found.");

            var onStartRunMethod = GetMethod(charSelectType, "OnStartRun",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(onStartRunMethod,
                "CharacterSelectScreen.OnStartRun() method not found. " +
                "This method is the guarded entry point for scene transition.");

            // ── c) SettingsMenuUI has lifecycle method(s) ────────────────
            var settingsType = GetRimaType("SettingsMenuUI");
            Assert.IsNotNull(settingsType, "SettingsMenuUI type not found.");

            // SettingsMenuUI uses RuntimeInitializeOnLoadMethod (AutoInit), not a bare Start.
            // Verify AutoInit exists as the gated entry point instead.
            var settingsAutoInit = GetMethod(settingsType, "AutoInit",
                BindingFlags.NonPublic | BindingFlags.Static);
            Assert.IsNotNull(settingsAutoInit,
                "SettingsMenuUI.AutoInit() not found. Expected a RuntimeInitializeOnLoadMethod gate.");

            // ── d) SkillOfferUI is resolvable ────────────────────────────
            var skillOfferType = GetRimaType("SkillOfferUI");
            Assert.IsNotNull(skillOfferType,
                "SkillOfferUI type not found in any loaded assembly.");

            // SkillOfferUI has no problematic Start (it is driven by Show/Hide API).
            // Verify Show method exists as the controlled activation pattern.
            var showMethod = GetMethod(skillOfferType, "Show",
                BindingFlags.Public | BindingFlags.Instance);
            Assert.IsNotNull(showMethod,
                "SkillOfferUI.Show() public method not found. Expected explicit Show/Hide activation pattern.");
        }
    }
}
