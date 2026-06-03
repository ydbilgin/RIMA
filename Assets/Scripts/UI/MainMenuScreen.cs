using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Main menu — Ashen Glyph spec.
    /// Dark background, "RIMA" logo with cyan glow, 3 buttons: NEW RUN, SETTINGS, QUIT.
    /// </summary>
    public class MainMenuScreen : MonoBehaviour
    {
        private static bool _gameStarted = false;
        private static bool _eventSystemHooked = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInit()
        {
            EnsureEventSystemHook();
            EnsureEventSystem();

            // Never create the legacy procedural menu when loading scene-backed UI or game/test scenes directly.
            var activeScene = SceneManager.GetActiveScene().name;
            if (activeScene == "_IsoGame" ||
                activeScene == "PlayableArena_Test01" ||
                activeScene == "MainMenu" ||
                activeScene == "CharacterSelect" ||
                activeScene == "RoomPipelineTest" ||
                activeScene == "_FazMVP_Demo")
            {
                return;
            }
            if (_gameStarted) return;
            if (Object.FindFirstObjectByType<MainMenuScreen>() != null) return;
            var go = new GameObject("[MainMenuScreen]");
            DontDestroyOnLoad(go);
            go.AddComponent<MainMenuScreen>();
        }

        private static void EnsureEventSystemHook()
        {
            if (_eventSystemHooked) return;
            SceneManager.sceneLoaded += OnSceneLoaded;
            _eventSystemHooked = true;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            EnsureEventSystem();
            if (scene.name != "_IsoGame" && scene.name != "PlayableArena_Test01") return;

            foreach (var menu in Object.FindObjectsByType<MainMenuScreen>(
                         FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (Application.isPlaying) Object.Destroy(menu.gameObject);
                else Object.DestroyImmediate(menu.gameObject);
            }

            UIManager.Instance?.ResumeFromMenu();
            Time.timeScale = 1f;
        }

        private static void EnsureEventSystem()
        {
            var systems = Object.FindObjectsByType<UnityEngine.EventSystems.EventSystem>(
                FindObjectsInactive.Include, FindObjectsSortMode.None);

            if (systems.Length > 1)
            {
                var keep = systems[0];
                for (int i = 0; i < systems.Length; i++)
                {
                    if (systems[i].gameObject.activeInHierarchy)
                    {
                        keep = systems[i];
                        break;
                    }
                }

                for (int i = 0; i < systems.Length; i++)
                {
                    if (systems[i] == keep) continue;
                    if (Application.isPlaying) Object.Destroy(systems[i].gameObject);
                    else Object.DestroyImmediate(systems[i].gameObject);
                }
                return;
            }

            if (systems.Length == 1) return;

            var es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<InputSystemUIInputModule>();
            Object.DontDestroyOnLoad(es);
        }

        private void Start()
        {
            var sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "_IsoGame" || sceneName == "PlayableArena_Test01")
            {
                Destroy(gameObject);
                return;
            }

            UIManager.Instance?.PauseForMenu();
            BuildUI();
        }

        private void OnDestroy()
        {
            UIManager.Instance?.ResumeFromMenu();
        }

        private void BuildUI()
        {
            var canvasGO = new GameObject("MainMenuCanvas");
            canvasGO.transform.SetParent(transform);
            var canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            var scaler = canvasGO.AddComponent<CanvasScaler>();
            scaler.uiScaleMode         = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight  = 0.5f;
            canvasGO.AddComponent<GraphicRaycaster>();

            // Full-screen dark background (#0D0D0F)
            var root = MakeRect("Root", canvasGO.transform, Vector2.zero, Vector2.one);
            var bg = root.AddComponent<Image>();
            bg.color = RimaUITheme.BackgroundDark;

            // On-brand Shattered-Keep backdrop (cover/crop, no distortion). Falls back to the flat
            // dark fill above if the sprite isn't imported. Logo/buttons are added after → draw on top.
            RimaUITheme.CreateFullScreenBackdrop(root.transform, "UI/Backgrounds/main_menu_bg", RimaUITheme.BackgroundDark);

            // ── Logo: "RIMA" ─────────────────────────────────────────
            var logoGo = MakeRect("Logo", root.transform,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            var logoRt = logoGo.GetComponent<RectTransform>();
            logoRt.pivot = new Vector2(0.5f, 0.5f);
            logoRt.anchoredPosition = new Vector2(0f, 120f);
            logoRt.sizeDelta = new Vector2(400f, 80f);

            var logoTmp = logoGo.AddComponent<TextMeshProUGUI>();
            logoTmp.text = "RIMA";
            logoTmp.fontSize = 48f;
            logoTmp.fontStyle = FontStyles.Bold;
            logoTmp.color = Color.white;
            logoTmp.alignment = TextAlignmentOptions.Center;
            logoTmp.raycastTarget = false;

            // Subtitle with cyan glow
            var subGo = MakeRect("Subtitle", root.transform,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            var subRt = subGo.GetComponent<RectTransform>();
            subRt.pivot = new Vector2(0.5f, 0.5f);
            subRt.anchoredPosition = new Vector2(0f, 70f);
            subRt.sizeDelta = new Vector2(400f, 24f);

            var subTmp = subGo.AddComponent<TextMeshProUGUI>();
            subTmp.text = "THE SEAL BENEATH THE KEEP";
            subTmp.fontSize = 12f;
            subTmp.color = RimaUITheme.Cyan;
            subTmp.alignment = TextAlignmentOptions.Center;
            subTmp.raycastTarget = false;

            // ── Buttons ──────────────────────────────────────────────
            float y = -20f;
            AddMenuButton(root.transform, "NEW RUN", y, OnPlayClicked);
            AddMenuButton(root.transform, "SETTINGS", y - 50f, OnSettings);
            AddMenuButton(root.transform, "QUIT", y - 100f, OnQuit);

            // Version
            var verGo = MakeRect("Version", root.transform,
                new Vector2(0f, 0f), new Vector2(0f, 0f));
            var verRt = verGo.GetComponent<RectTransform>();
            verRt.pivot = new Vector2(0f, 0f);
            verRt.anchoredPosition = new Vector2(16f, 12f);
            verRt.sizeDelta = new Vector2(200f, 20f);

            var verTmp = verGo.AddComponent<TextMeshProUGUI>();
            verTmp.text = "S43 Dev Build";
            verTmp.fontSize = 10f;
            verTmp.color = new Color(0.3f, 0.35f, 0.4f, 0.6f);
            verTmp.alignment = TextAlignmentOptions.Left;
            verTmp.raycastTarget = false;
        }

        private void AddMenuButton(Transform parent, string text, float yOffset, UnityEngine.Events.UnityAction onClick)
        {
            var btnGo = MakeRect($"Btn_{text}", parent,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            var btnRt = btnGo.GetComponent<RectTransform>();
            btnRt.pivot = new Vector2(0.5f, 0.5f);
            btnRt.anchoredPosition = new Vector2(0f, yOffset);
            btnRt.sizeDelta = new Vector2(220f, 40f);

            var btnImg = btnGo.AddComponent<Image>();
            btnImg.sprite = RimaUITheme.ResourceFrame;
            btnImg.color = RimaUITheme.PanelTint;

            var btn = btnGo.AddComponent<Button>();
            var colors = btn.colors;
            colors.normalColor      = RimaUITheme.PanelTint;
            colors.highlightedColor = new Color(RimaUITheme.Cyan.r, RimaUITheme.Cyan.g, RimaUITheme.Cyan.b, 0.35f);
            colors.pressedColor     = new Color(RimaUITheme.Cyan.r * 0.7f, RimaUITheme.Cyan.g * 0.7f, RimaUITheme.Cyan.b * 0.7f, 0.5f);
            btn.colors = colors;

            var lblGo = MakeRect("Label", btnGo.transform, Vector2.zero, Vector2.one);
            var lbl = lblGo.AddComponent<TextMeshProUGUI>();
            lbl.text = text;
            lbl.fontSize = 16f;
            lbl.fontStyle = FontStyles.Bold;
            lbl.color = Color.white;
            lbl.alignment = TextAlignmentOptions.Center;
            lbl.raycastTarget = false;

            btn.onClick.AddListener(onClick);
        }

        // ── Actions ──────────────────────────────────────────────────

        private void OnPlayClicked()
        {
            _gameStarted = true;
            Destroy(gameObject);
            var go = new GameObject("[CharacterSelectScreen]");
            DontDestroyOnLoad(go);
            go.AddComponent<CharacterSelectScreen>();
        }

        private void OnSettings()
        {
            // Show the settings menu if available
            var settings = FindAnyObjectByType<SettingsMenuUI>();
            if (settings != null) settings.Open();
        }

        private void OnQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private static GameObject MakeRect(string name, Transform parent, Vector2 ancMin, Vector2 ancMax)
        {
            var go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = ancMin;
            rt.anchorMax = ancMax;
            rt.offsetMin = rt.offsetMax = Vector2.zero;
            return go;
        }
    }
}
