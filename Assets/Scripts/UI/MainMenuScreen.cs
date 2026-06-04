using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Main menu — Ashen Glyph spec.
    /// Dark background, "RIMA" logo with quiet cyan whisper, 2 buttons: NEW RUN, QUIT.
    /// </summary>
    public class MainMenuScreen : MonoBehaviour
    {
        private const string PackButtonPath = "UI/RIMA/Pack/button_9slice";

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
            subTmp.text = "Yine geldin.";
            subTmp.fontSize = 11f;
            subTmp.fontStyle = FontStyles.Italic;
            subTmp.color = WithAlpha(RimaUITheme.Cyan, 0.78f);
            subTmp.alignment = TextAlignmentOptions.Center;
            subTmp.raycastTarget = false;

            // ── Buttons ──────────────────────────────────────────────
            float y = -20f;
            AddMenuButton(root.transform, "NEW RUN", y, OnPlayClicked, true);
            AddMenuButton(root.transform, "QUIT", y - 54f, OnQuit, false);

            // Version
            var verGo = MakeRect("Version", root.transform,
                new Vector2(1f, 0f), new Vector2(1f, 0f));
            var verRt = verGo.GetComponent<RectTransform>();
            verRt.pivot = new Vector2(1f, 0f);
            verRt.anchoredPosition = new Vector2(-16f, 12f);
            verRt.sizeDelta = new Vector2(180f, 18f);

            var verTmp = verGo.AddComponent<TextMeshProUGUI>();
            verTmp.text = "S43 Dev Build";
            verTmp.fontSize = 8f;
            verTmp.color = WithAlpha(RimaUITheme.TextMuted, 0.35f);
            verTmp.alignment = TextAlignmentOptions.Right;
            verTmp.raycastTarget = false;
        }

        private void AddMenuButton(Transform parent, string text, float yOffset, UnityEngine.Events.UnityAction onClick, bool primary)
        {
            var btnGo = MakeRect($"Btn_{text}", parent,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
            var btnRt = btnGo.GetComponent<RectTransform>();
            btnRt.pivot = new Vector2(0.5f, 0.5f);
            btnRt.anchoredPosition = new Vector2(0f, yOffset);
            btnRt.sizeDelta = new Vector2(220f, 40f);

            var btnImg = btnGo.AddComponent<Image>();
            var packButton = Resources.Load<Sprite>(PackButtonPath);
            btnImg.sprite = packButton != null ? packButton : RimaUITheme.ResourceFrame;
            btnImg.type = Image.Type.Sliced;
            btnImg.color = primary
                ? WithAlpha(RimaUITheme.Cyan, 0.22f)
                : WithAlpha(RimaUITheme.PanelTint, 0.30f);

            var btn = btnGo.AddComponent<Button>();
            btn.targetGraphic = btnImg;
            var colors = btn.colors;
            colors.normalColor      = primary ? WithAlpha(RimaUITheme.Cyan, 0.22f) : WithAlpha(RimaUITheme.PanelTint, 0.30f);
            colors.highlightedColor = WithAlpha(RimaUITheme.Cyan, primary ? 0.42f : 0.30f);
            colors.pressedColor     = WithAlpha(RimaUITheme.Cyan, primary ? 0.56f : 0.44f);
            colors.selectedColor    = WithAlpha(RimaUITheme.Cyan, primary ? 0.42f : 0.30f);
            colors.disabledColor    = WithAlpha(RimaUITheme.TextMuted, 0.18f);
            btn.colors = colors;

            var lblGo = MakeRect("Label", btnGo.transform, Vector2.zero, Vector2.one);
            var lbl = lblGo.AddComponent<TextMeshProUGUI>();
            lbl.text = text;
            lbl.fontSize = primary ? 17f : 15f;
            lbl.fontStyle = FontStyles.Bold;
            lbl.color = primary ? RimaUITheme.TextPrimary : WithAlpha(RimaUITheme.TextPrimary, 0.86f);
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

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }
    }
}
