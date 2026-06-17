using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
        private bool skillCodexOpen;
        private bool pauseOpen;
        private bool _menuPaused;
        private bool directorModalOpen;
        private Canvas directorModalCanvas;

        private const int ModalBaseSortingOrder = 1200;
        private const float ScrimAlpha = 0.75f;
        private readonly List<Canvas> modalStack = new List<Canvas>(5);
        private Canvas scrimCanvas;
        private Image scrimImage;

        // ── Cached references ────────────────────────────────────────────
        private CharacterSheetUI sheetUI;
        private SettingsMenuUI   settingsUI;
        private SkillCodexUI     skillCodexUI;
        private PauseMenuUI      pauseUI;

        // ── Input ────────────────────────────────────────────────────────
        private InputAction tabAction;
        private InputAction escAction;
        private InputAction panicAction;

        public bool IsTabOpen        => tabOpen;
        public bool IsSettingsOpen   => settingsOpen;
        public bool IsSkillOfferOpen => skillOfferOpen;
        public bool IsSkillCodexOpen => skillCodexOpen;
        public bool IsPauseOpen      => pauseOpen;
        public bool IsAnyOverlayOpen => tabOpen || settingsOpen || skillOfferOpen || skillCodexOpen || pauseOpen;
        public bool IsAnyModalOpen   => IsAnyOverlayOpen || directorModalOpen;

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
            skillCodexOpen = false;
            pauseOpen = false;
            _menuPaused = false;
            directorModalOpen = false;
            directorModalCanvas = null;
            SyncModalPresentation();
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

            panicAction = new InputAction("Panic", InputActionType.Button);
            panicAction.AddBinding("<Keyboard>/f12");
        }

        private void OnEnable()
        {
            tabAction.Enable();
            escAction.Enable();
            panicAction.Enable();
            tabAction.performed += OnTab;
            escAction.performed += OnEsc;
            panicAction.performed += OnPanic;
        }

        private void OnDisable()
        {
            tabAction.performed -= OnTab;
            escAction.performed -= OnEsc;
            panicAction.performed -= OnPanic;
            tabAction.Disable();
            escAction.Disable();
            panicAction.Disable();
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

            // If codex open, close codex first
            if (skillCodexOpen) { CloseSkillCodex(); return; }

            // Toggle TAB overlay
            if (tabOpen) CloseTab();
            else         OpenTab();
        }

        private void OnEsc(InputAction.CallbackContext ctx)
        {
            // 1. Skill offer blocks ESC entirely — no pause mid-draft
            if (skillOfferOpen) return;

            // 2. Settings open → close settings; pause panel re-appears if pause is still open
            if (settingsOpen)
            {
                CloseSettings();
                if (pauseOpen && pauseUI != null) pauseUI.Open();
                return;
            }

            // 3. Skill codex open → close codex; pause panel re-appears if pause is still open
            if (skillCodexOpen)
            {
                CloseSkillCodex();
                if (pauseOpen && pauseUI != null) pauseUI.Open();
                return;
            }

            // 4. TAB overlay open → close TAB
            if (tabOpen) { CloseTab(); return; }

            // 5. Pause open → close/resume
            if (pauseOpen) { ClosePause(); return; }

            // 6. Nothing open → open pause
            OpenPause();
        }

        /// <summary>
        /// F12 — Demo panic button. Clears all UI overlays, restores timeScale, re-enables
        /// PlayerController, and opens exit doors (via RoomRunDirector) so a softlock/UI-jam
        /// can be rescued in front of the jury. No visible UI change.
        /// </summary>
        private void OnPanic(InputAction.CallbackContext ctx)
        {
            // 1. Force timeScale to 1.
            Time.timeScale = 1f;

            // 2. Close all open overlays by resetting state flags and hiding panels.
            if (skillOfferOpen) CloseSkillOffer();
            if (skillCodexOpen) CloseSkillCodex();
            if (settingsOpen)   CloseSettings();
            if (tabOpen)        CloseTab();
            if (pauseOpen)      ClosePause();
            _menuPaused = false;
            directorModalOpen = false;
            directorModalCanvas = null;
            SyncModalPresentation();

            // Ensure timeScale is 1 even if overlay-close methods set it otherwise.
            Time.timeScale = 1f;

            // 3. Re-enable PlayerController so movement works if it was locked.
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                var pc = player.GetComponent<RIMA.PlayerController>();
                if (pc != null && !pc.enabled) pc.enabled = true;
            }

            // 4. Open exit doors via RoomRunDirector (softlock recovery).
            var director = UnityEngine.Object.FindAnyObjectByType<RIMA.MapDesigner.Room.Runtime.RoomRunDirector>();
            if (director != null)
                director.ForceOpenExitDoorsFromAnyClearedState();

            Debug.Log("[PANIC] recovered");
        }

        // ─── Public API ─────────────────────────────────────────────────

        public void OpenTab()
        {
            if (tabOpen || skillOfferOpen || skillCodexOpen) return;
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
            if (skillCodexOpen) CloseSkillCodex();
            // Hide pause panel visually (keep pauseOpen=true so ESC returns to it)
            if (pauseOpen && pauseUI != null) pauseUI.Close();
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
            if (skillCodexOpen) CloseSkillCodex();
            skillOfferOpen = true;
            ApplyTimeScale();
        }

        public void CloseSkillOffer()
        {
            if (!skillOfferOpen) return;
            skillOfferOpen = false;
            ApplyTimeScale();
        }

        public void OpenSkillCodex()
        {
            if (skillCodexOpen || skillOfferOpen || settingsOpen) return;
            if (tabOpen) CloseTab();
            // Hide pause panel visually (keep pauseOpen=true so ESC returns to it)
            if (pauseOpen && pauseUI != null) pauseUI.Close();
            skillCodexOpen = true;
            ResolveSkillCodexUI();
            if (skillCodexUI != null) skillCodexUI.Open();
            ApplyTimeScale();
        }

        public void CloseSkillCodex()
        {
            if (!skillCodexOpen) return;
            skillCodexOpen = false;
            if (skillCodexUI != null) skillCodexUI.Close();
            ApplyTimeScale();
        }

        public void OpenPause()
        {
            // Pause is blocked while a skill offer (draft) is active
            if (skillOfferOpen) return;
            if (pauseOpen) return;
            pauseOpen = true;
            ResolvePauseUI();
            if (pauseUI != null) pauseUI.Open();
            ApplyTimeScale();
        }

        public void ClosePause()
        {
            if (!pauseOpen) return;
            pauseOpen = false;
            if (pauseUI != null) pauseUI.Close();
            ApplyTimeScale();
        }

        public void SetDirectorModalOpen(bool isOpen, Canvas modalCanvas)
        {
            directorModalOpen = isOpen;
            directorModalCanvas = isOpen ? modalCanvas : null;
            SyncModalPresentation();
        }

        public void TogglePause()
        {
            if (pauseOpen) ClosePause();
            else           OpenPause();
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
            skillCodexOpen = false;
            pauseOpen      = false;
            _menuPaused    = false;
            directorModalOpen = false;
            directorModalCanvas = null;
            SyncModalPresentation();
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
            SyncModalPresentation();

            if (skillOfferOpen || settingsOpen || skillCodexOpen || pauseOpen)
                Time.timeScale = 0f;
            else if (tabOpen)
                Time.timeScale = 0.1f;
            else
                Time.timeScale = 1f;
        }

        private void SyncModalPresentation()
        {
            modalStack.Clear();

            if (pauseOpen) AddModalCanvas(ResolveCanvas(pauseUI));
            if (skillCodexOpen) AddModalCanvas(ResolveCanvas(skillCodexUI));
            if (settingsOpen) AddModalCanvas(ResolveCanvas(settingsUI));
            if (skillOfferOpen) AddModalCanvas(ResolveCanvas(FindAnyObjectByType<SkillOfferUI>()));
            if (directorModalOpen && !IsAnyOverlayOpen) AddModalCanvas(directorModalCanvas);

            for (int i = 0; i < modalStack.Count; i++)
            {
                Canvas canvas = modalStack[i];
                canvas.overrideSorting = true;
                canvas.sortingOrder = ModalBaseSortingOrder + i * 10;
            }

            if (modalStack.Count == 0)
            {
                if (scrimCanvas != null) scrimCanvas.gameObject.SetActive(false);
                return;
            }

            EnsureScrim();
            scrimCanvas.sortingOrder = modalStack[modalStack.Count - 1].sortingOrder - 1;
            scrimCanvas.gameObject.SetActive(true);
        }

        private void EnsureScrim()
        {
            if (scrimCanvas != null) return;

            var go = new GameObject("UI_Scrim_Dimmer", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            go.transform.SetParent(transform, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            scrimCanvas = go.GetComponent<Canvas>();
            scrimCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            scrimCanvas.overrideSorting = true;

            var scaler = go.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            scrimImage = go.AddComponent<Image>();
            scrimImage.color = new Color(0f, 0f, 0f, ScrimAlpha);
            scrimImage.raycastTarget = true;
            go.SetActive(false);
        }

        private void AddModalCanvas(Canvas canvas)
        {
            if (canvas == null || modalStack.Contains(canvas)) return;
            modalStack.Add(canvas);
        }

        private static Canvas ResolveCanvas(Component component)
        {
            if (component == null) return null;
            Canvas canvas = component.GetComponent<Canvas>();
            if (canvas != null) return canvas;
            canvas = component.GetComponentInChildren<Canvas>(true);
            if (canvas != null) return canvas;
            return component.GetComponentInParent<Canvas>();
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

        private void ResolveSkillCodexUI()
        {
            if (skillCodexUI == null)
                skillCodexUI = SkillCodexUI.EnsureInstance();
        }

        private void ResolvePauseUI()
        {
            if (pauseUI == null)
                pauseUI = PauseMenuUI.EnsureInstance();
        }
    }
}
