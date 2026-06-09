using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace RIMA.Tests
{
    /// <summary>
    /// EditMode tests for UIManager pause menu state logic.
    ///
    /// EditMode constraint: DontDestroyOnLoad is forbidden, so we cannot call
    /// PauseMenuUI.EnsureInstance() (it calls DDOL). Strategy:
    ///   - PauseMenuUI: AddComponent directly (Awake has no DDOL), inject into pauseUI field.
    ///   - SettingsMenuUI/SkillCodexUI: inject null + set state flags directly via reflection
    ///     to avoid triggering their DDOL-heavy constructors.
    /// All assertions are on public properties (IsPauseOpen, IsSettingsOpen, etc.)
    /// and Time.timeScale.
    /// </summary>
    public class UIManagerPauseTests
    {
        private GameObject managerGo;
        private UIManager  manager;
        private GameObject pauseUIHost;

        [SetUp]
        public void SetUp()
        {
            Time.timeScale = 1f;

            // UIManager host
            managerGo = new GameObject("[UIManager_Test]");
            manager = managerGo.AddComponent<UIManager>();
            SetStaticInstance(manager);

            // PauseMenuUI stub — plain AddComponent (no DontDestroyOnLoad, only EnsureInstance uses it)
            // Needs a Canvas on the same GO so Canvas/CanvasGroup can be found.
            pauseUIHost = new GameObject("[PauseMenuUI_Stub]");
            pauseUIHost.AddComponent<Canvas>();
            pauseUIHost.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            var pause = pauseUIHost.AddComponent<PauseMenuUI>();
            SetField("pauseUI", pause);

            // SettingsMenuUI and SkillCodexUI: keep null so their heavy Awake/EnsureInstance don't run.
            // Tests that touch these paths will manipulate flags directly via reflection.
            SetField("settingsUI",   null);
            SetField("skillCodexUI", null);
            SetField("sheetUI",      null);
        }

        [TearDown]
        public void TearDown()
        {
            SetStaticInstance(null);
            Time.timeScale = 1f;
            Object.DestroyImmediate(managerGo);
            if (pauseUIHost != null) Object.DestroyImmediate(pauseUIHost);
        }

        // ── Reflection helpers ────────────────────────────────────────────────

        private static void SetStaticInstance(UIManager value)
        {
            var backing = typeof(UIManager).GetField("<Instance>k__BackingField",
                BindingFlags.NonPublic | BindingFlags.Static);
            backing?.SetValue(null, value);
        }

        private void SetField(string name, object value)
        {
            var f = typeof(UIManager).GetField(name,
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(f, $"UIManager field '{name}' not found");
            f.SetValue(manager, value);
        }

        private void InvokePrivate(string methodName)
        {
            var m = typeof(UIManager).GetMethod(methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(m, $"UIManager.{methodName}() not found");
            m.Invoke(manager, null);
        }

        // ── Test 1: OpenPause sets pauseOpen + timeScale=0 ──────────────────

        [Test]
        public void OpenPause_SetsPauseOpenAndFreezesTime()
        {
            manager.OpenPause();

            Assert.IsTrue(manager.IsPauseOpen,  "IsPauseOpen must be true after OpenPause()");
            Assert.AreEqual(0f, Time.timeScale, "timeScale must be 0 when paused");
        }

        // ── Test 2: ClosePause clears flag + restores timeScale ──────────────

        [Test]
        public void ClosePause_ClearsPauseOpenAndRestoresTime()
        {
            manager.OpenPause();
            manager.ClosePause();

            Assert.IsFalse(manager.IsPauseOpen, "IsPauseOpen must be false after ClosePause()");
            Assert.AreEqual(1f, Time.timeScale,  "timeScale must be 1 after ClosePause()");
        }

        // ── Test 3: OpenPause blocked while skillOffer open ──────────────────

        [Test]
        public void OpenPause_NoOp_WhenSkillOfferOpen()
        {
            SetField("skillOfferOpen", true);
            manager.OpenPause();

            Assert.IsFalse(manager.IsPauseOpen, "Pause must not open during skill offer/draft");
        }

        // ── Test 4: TogglePause opens then closes ────────────────────────────

        [Test]
        public void TogglePause_OpensAndCloses()
        {
            manager.TogglePause();
            Assert.IsTrue(manager.IsPauseOpen,   "First TogglePause should open");

            manager.TogglePause();
            Assert.IsFalse(manager.IsPauseOpen,  "Second TogglePause should close");
        }

        // ── Test 5: pauseOpen stays true when Settings state is layered on top

        [Test]
        public void OpenSettings_WhilePaused_KeepsPauseOpenFlag()
        {
            // Simulate pause being open via direct field (pauseUI stub already set)
            manager.OpenPause();

            // Simulate Settings open: set the flag directly (settingsUI is null — no panel call).
            // This mirrors the state UIManager would be in after OpenSettings().
            SetField("settingsOpen", true);
            InvokePrivate("ApplyTimeScale");

            Assert.IsTrue(manager.IsPauseOpen,   "pauseOpen must remain true when Settings layered on top");
            Assert.IsTrue(manager.IsSettingsOpen);
        }

        // ── Test 6: CloseSettings with pause parent leaves pauseOpen=true ─────

        [Test]
        public void CloseSettings_WithPauseParent_LeavesPauseOpenTrue()
        {
            manager.OpenPause();
            SetField("settingsOpen", true);
            InvokePrivate("ApplyTimeScale");

            // CloseSettings with null settingsUI is a no-op for the panel, but clears the flag.
            manager.CloseSettings();

            Assert.IsTrue(manager.IsPauseOpen,   "pauseOpen must remain true after closing Settings");
            Assert.IsFalse(manager.IsSettingsOpen);
            Assert.AreEqual(0f, Time.timeScale,  "timeScale stays 0 — pause still active");
        }

        // ── Test 7: Full resume path ──────────────────────────────────────────

        [Test]
        public void ClosePause_AfterSettingsChildClosed_RestoresTimeScale()
        {
            manager.OpenPause();
            SetField("settingsOpen", true);
            InvokePrivate("ApplyTimeScale");

            manager.CloseSettings();
            manager.ClosePause();

            Assert.IsFalse(manager.IsPauseOpen);
            Assert.AreEqual(1f, Time.timeScale, "timeScale must be 1 after full resume");
        }

        // ── Test 8: IsAnyOverlayOpen includes pauseOpen ───────────────────────

        [Test]
        public void IsAnyOverlayOpen_TrueWhenPauseOpen()
        {
            Assert.IsFalse(manager.IsAnyOverlayOpen, "No overlays at start");
            manager.OpenPause();
            Assert.IsTrue(manager.IsAnyOverlayOpen,  "IsAnyOverlayOpen must include pauseOpen");
        }

        // ── Test 9: ResumeGame clears pauseOpen ──────────────────────────────

        [Test]
        public void ResumeGame_ClearsPauseOpen()
        {
            manager.OpenPause();
            manager.ResumeGame();

            Assert.IsFalse(manager.IsPauseOpen, "ResumeGame must clear pauseOpen");
            Assert.AreEqual(1f, Time.timeScale);
        }

        // ── Test 10: pauseOpen stays true when codex is layered on top ────────

        [Test]
        public void OpenSkillCodex_WhilePaused_KeepsPauseOpenFlag()
        {
            manager.OpenPause();

            // Simulate codex open: set flag directly (skillCodexUI is null — no panel call).
            SetField("skillCodexOpen", true);
            InvokePrivate("ApplyTimeScale");

            Assert.IsTrue(manager.IsPauseOpen,    "pauseOpen must remain true when codex layered on top");
            Assert.IsTrue(manager.IsSkillCodexOpen);
            Assert.AreEqual(0f, Time.timeScale,   "timeScale remains 0 while codex is open");
        }
    }
}
