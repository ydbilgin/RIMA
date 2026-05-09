using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// Central UI state controller — owns all 3 layers and timeScale.
    /// Layer 0: Combat HUD (always visible during gameplay)
    /// Layer 1: TAB overlay (CharacterSheetUI, timeScale 0.1)
    /// Layer 2: ESC menu / SkillOffer (timeScale 0)
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }
        private static bool sceneLoadHooked;

        // ── Layer state ──────────────────────────────────────────────────
        private bool tabOpen;
        private bool settingsOpen;
        private bool skillOfferOpen;
        private bool _menuPaused;

        // ── Cached references ────────────────────────────────────────────
        private CharacterSheetUI sheetUI;
        private SettingsMenuUI   settingsUI;

        // ── Input ────────────────────────────────────────────────────────
        private InputAction tabAction;
        private InputAction escAction;

        public bool IsTabOpen        => tabOpen;
        public bool IsSettingsOpen   => settingsOpen;
        public bool IsSkillOfferOpen => skillOfferOpen;
        public bool IsAnyOverlayOpen => tabOpen || settingsOpen || skillOfferOpen;

        // ─── Lifecycle ───────────────────────────────────────────────────

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInit()
        {
            EnsureSceneLoadHook();

            if (Instance != null)
            {
                // Reset overlay state on scene reload — prevents prior PauseForMenu
                // from leaking timeScale=0 into the next scene (PlayMode test order pollution).
                Instance.ResetForSceneLoad();
                Time.timeScale = 1f;
                return;
            }
            var go = new GameObject("[UIManager]");
            DontDestroyOnLoad(go);
            var manager = go.AddComponent<UIManager>();
            manager.ResetForSceneLoad();
            Time.timeScale = 1f;
        }

        private static void EnsureSceneLoadHook()
        {
            if (sceneLoadHooked) return;
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            sceneLoadHooked = true;
        }

        private static void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
        {
            if (Instance != null)
                Instance.ResetForSceneLoad();

            Time.timeScale = 1f;
        }

        public void ResetForSceneLoad()
        {
            tabOpen = false;
            settingsOpen = false;
            skillOfferOpen = false;
            _menuPaused = false;
            Time.timeScale = 1f;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;

            tabAction = new InputAction("Tab", InputActionType.Button);
            tabAction.AddBinding("<Keyboard>/tab");

            escAction = new InputAction("Escape", InputActionType.Button);
            escAction.AddBinding("<Keyboard>/escape");
        }

        private void OnEnable()
        {
            tabAction.Enable();
            escAction.Enable();
            tabAction.performed += OnTab;
            escAction.performed += OnEsc;
        }

        private void OnDisable()
        {
            tabAction.performed -= OnTab;
            escAction.performed -= OnEsc;
            tabAction.Disable();
            escAction.Disable();
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ─── Input handlers ─────────────────────────────────────────────

        private void OnTab(InputAction.CallbackContext ctx)
        {
            // Skill offer blocks all input
            if (skillOfferOpen) return;

            // If settings open, close settings first
            if (settingsOpen) { CloseSettings(); return; }

            // Toggle TAB overlay
            if (tabOpen) CloseTab();
            else         OpenTab();
        }

        private void OnEsc(InputAction.CallbackContext ctx)
        {
            // Skill offer blocks ESC
            if (skillOfferOpen) return;

            // If TAB open, close TAB first
            if (tabOpen) { CloseTab(); return; }

            // Toggle settings
            if (settingsOpen) CloseSettings();
            else              OpenSettings();
        }

        // ─── Public API ─────────────────────────────────────────────────

        public void OpenTab()
        {
            if (tabOpen || skillOfferOpen) return;
            tabOpen = true;
            ResolveSheetUI();
            if (sheetUI != null) sheetUI.Show();
            ApplyTimeScale();
        }

        public void CloseTab()
        {
            if (!tabOpen) return;
            tabOpen = false;
            if (sheetUI != null) sheetUI.Hide();
            ApplyTimeScale();
        }

        public void OpenSettings()
        {
            if (settingsOpen || skillOfferOpen) return;
            if (tabOpen) CloseTab();
            settingsOpen = true;
            ResolveSettingsUI();
            if (settingsUI != null) settingsUI.Open();
            ApplyTimeScale();
        }

        public void CloseSettings()
        {
            if (!settingsOpen) return;
            settingsOpen = false;
            if (settingsUI != null) settingsUI.Close();
            ApplyTimeScale();
        }

        public void OpenSkillOffer()
        {
            if (tabOpen) CloseTab();
            if (settingsOpen) CloseSettings();
            skillOfferOpen = true;
            ApplyTimeScale();
        }

        public void CloseSkillOffer()
        {
            if (!skillOfferOpen) return;
            skillOfferOpen = false;
            ApplyTimeScale();
        }

        /// <summary>
        /// Resets all overlay flags and restores timeScale to 1.
        /// Call before loading a new scene from a non-pause context (e.g. CharacterSelect -> game).
        /// </summary>
        public void ResumeGame()
        {
            tabOpen        = false;
            settingsOpen   = false;
            skillOfferOpen = false;
            _menuPaused    = false;
            Time.timeScale = 1f;
        }

        /// <summary>Pauses time for non-game menus (e.g. MainMenuScreen). Tracked separately from overlay state.</summary>
        public void PauseForMenu()
        {
            _menuPaused    = true;
            Time.timeScale = 0f;
        }

        /// <summary>Restores timeScale after a non-game menu closes. No-op if overlays are still open.</summary>
        public void ResumeFromMenu()
        {
            _menuPaused = false;
            if (!IsAnyOverlayOpen)
                Time.timeScale = 1f;
        }

        // ─── TimeScale (single owner) ───────────────────────────────────

        private void ApplyTimeScale()
        {
            if (skillOfferOpen || settingsOpen)
                Time.timeScale = 0f;
            else if (tabOpen)
                Time.timeScale = 0.1f;
            else
                Time.timeScale = 1f;
        }

        // ─── Resolve cached refs ────────────────────────────────────────

        private void ResolveSheetUI()
        {
            if (sheetUI == null)
                sheetUI = CharacterSheetUI.Instance;
        }

        private void ResolveSettingsUI()
        {
            if (settingsUI == null)
                settingsUI = FindAnyObjectByType<SettingsMenuUI>();
        }
    }
}
