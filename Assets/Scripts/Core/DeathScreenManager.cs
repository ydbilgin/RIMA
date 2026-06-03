using System.Collections;
using RIMA.Systems.Map;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RIMA
{
    /// <summary>
    /// Death screen + restart loop.
    /// Listens to Player's Health.OnDeath, then shows a self-built death panel and restart options.
    /// </summary>
    public class DeathScreenManager : MonoBehaviour
    {
        private const string DefaultSteamWishlistUrl = "https://store.steampowered.com/app/0/";
        private static readonly Color BrandCyan = new Color(0f, 1f, 0.8f, 1f);
        private static readonly string[] DeathLines =
        {
            "The rift remembers. You won't.",
            "Not an ending. Just a place where you stopped."
        };

        [Header("UI References (auto-found or auto-created if null)")]
        [SerializeField] private GameObject deathPanel;
        [SerializeField] private TextMeshProUGUI deathTitle;
        [SerializeField] private TextMeshProUGUI deathStats;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;

        [Header("Settings")]
        [SerializeField] private float fadeInDuration = 0.8f;
        [SerializeField] private float slowMoScale = 0.15f;
        [SerializeField] private float slowMoDuration = 1.5f;
        [SerializeField] private string steamWishlistUrl = DefaultSteamWishlistUrl;

        private Health playerHealth;
        private CanvasGroup canvasGroup;
        private bool isDead;
        private Button wishlistButton;
        private Button copySeedButton;

        public int KillCount => RunStats.Kills;

        private void Awake()
        {
            EnsureVisibleScale(transform);
            EnsureEventSystem();
            EnsurePanel();
            EnsurePanelControls();

            if (restartButton != null)
                restartButton.onClick.AddListener(RestartRun);
        }

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                    playerHealth.OnDeath.AddListener(OnPlayerDied);
            }
        }

        private void Update()
        {
            if (!isDead) return;

            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
                RestartRun();
        }

        private void OnDisable()
        {
            if (isDead)
                Time.timeScale = 1f;
        }

        /// <summary>Track kills for death/victory stats.</summary>
        public void RegisterKill()
        {
        }

        private void OnPlayerDied()
        {
            if (isDead) return;
            isDead = true;
            Debug.Log("[DeathScreenManager] Player died. Press R to restart.");
            StartCoroutine(DeathSequence());
        }

        private IEnumerator DeathSequence()
        {
            Time.timeScale = slowMoScale;
            yield return new WaitForSecondsRealtime(slowMoDuration);

            Time.timeScale = 0f;

            if (deathPanel != null)
            {
                EnsureVisibleScale(transform);
                EnsureVisibleScale(deathPanel.transform);
                deathPanel.SetActive(true);

                if (deathTitle != null)
                    deathTitle.text = DeathLines[Random.Range(0, DeathLines.Length)];

                if (deathStats != null)
                    deathStats.text = BuildRunStats();

                if (canvasGroup != null)
                {
                    canvasGroup.alpha = 0f;
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;

                    float elapsed = 0f;
                    while (elapsed < fadeInDuration)
                    {
                        elapsed += Time.unscaledDeltaTime;
                        canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeInDuration);
                        yield return null;
                    }

                    canvasGroup.alpha = 1f;
                }
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                PlayerController ctrl = player.GetComponent<PlayerController>();
                if (ctrl != null) ctrl.enabled = false;
            }
        }

        public void RestartRun()
        {
            Time.timeScale = 1f;
            if (Application.isPlaying)
            {
                MapFlowManager.Instance?.ResetRun();
                RunStats.Instance?.StartNewRun();
                SceneManager.LoadScene("_IsoGame");
            }
        }

        private void LoadMainMenu()
        {
            Time.timeScale = 1f;
            if (Application.isPlaying)
            {
                MapFlowManager.Instance?.ResetRun();
                SceneManager.LoadScene("MainMenu");
            }
        }

        private void OpenWishlist()
        {
            string url = string.IsNullOrWhiteSpace(steamWishlistUrl) ? DefaultSteamWishlistUrl : steamWishlistUrl;
            Application.OpenURL("steam://openurl/" + url);
            Application.OpenURL(url);
        }

        private void CopyBuildSeed()
        {
            GUIUtility.systemCopyBuffer = RunStats.BuildSeed;
        }

        private string BuildRunStats()
        {
            return $"Room: {RunStats.RoomReached}\nKills: {RunStats.Kills}\nTime: {FormatSeconds(RunStats.RunTimeSeconds)}\nBuild: {RunStats.BuildName}";
        }

        private void EnsurePanel()
        {
            if (deathPanel == null)
                deathPanel = FindChildGameObject("DeathScreen");

            if (deathPanel == null)
                deathPanel = CreatePanelRoot();

            EnsureVisibleScale(deathPanel.transform);
            canvasGroup = deathPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = deathPanel.AddComponent<CanvasGroup>();

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            deathPanel.SetActive(false);
        }

        private GameObject CreatePanelRoot()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            Transform parent = transform;

            if (canvas == null)
            {
                GameObject canvasGo = new GameObject("DeathScreenCanvas_Auto");
                canvasGo.transform.SetParent(transform, false);
                canvas = canvasGo.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 210;
                canvasGo.AddComponent<CanvasScaler>();
                canvasGo.AddComponent<GraphicRaycaster>();
                parent = canvasGo.transform;
            }
            else
            {
                canvas.sortingOrder = Mathf.Max(canvas.sortingOrder, 210);
                parent = canvas.transform;
            }

            Image bg = CreateImage("DeathScreen", parent, RimaUITheme.OverlayDark);
            Stretch(bg.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            // On-brand cracked-seal death backdrop (cover/crop). DeathPanel is added after → on top.
            RimaUITheme.CreateFullScreenBackdrop(bg.transform, "UI/Backgrounds/death_screen_bg", RimaUITheme.OverlayDark);

            RectTransform panel = CreatePanel("DeathPanel", bg.transform, RimaUITheme.PanelTint, new Color(0f, 1f, 0.8f, 0.45f));
            Stretch(panel, new Vector2(0.22f, 0.16f), new Vector2(0.78f, 0.84f), Vector2.zero, Vector2.zero);
            return bg.gameObject;
        }

        private void EnsurePanelControls()
        {
            if (deathTitle == null)
                deathTitle = FindChildComponent<TextMeshProUGUI>("DeathTitle");

            if (deathStats == null)
                deathStats = FindChildComponent<TextMeshProUGUI>("DeathStats");

            if (restartButton == null)
                restartButton = FindChildComponent<Button>("RestartButton");

            if (mainMenuButton == null)
                mainMenuButton = FindChildComponent<Button>("MainMenuButton");

            Transform contentRoot = FindChildRecursive(deathPanel.transform, "DeathPanel") ?? deathPanel.transform;

            if (deathTitle == null)
            {
                deathTitle = CreateText("DeathTitle", contentRoot, "", 26f, RimaUITheme.TextPrimary, TextAlignmentOptions.Center);
                Stretch(deathTitle.rectTransform, new Vector2(0.08f, 0.70f), new Vector2(0.92f, 0.90f), Vector2.zero, Vector2.zero);
            }

            if (deathStats == null)
            {
                deathStats = CreateText("DeathStats", contentRoot, "", 16f, RimaUITheme.TextMuted, TextAlignmentOptions.Center);
                Stretch(deathStats.rectTransform, new Vector2(0.12f, 0.45f), new Vector2(0.88f, 0.68f), Vector2.zero, Vector2.zero);
            }

            if (wishlistButton == null)
            {
                wishlistButton = FindNamedButton("WishlistButton");
                if (wishlistButton == null)
                {
                    wishlistButton = CreateButton("WishlistButton", contentRoot, "WISHLIST ON STEAM", BrandCyan, RimaUITheme.BackgroundDark, 18f);
                    Stretch((RectTransform)wishlistButton.transform, new Vector2(0.18f, 0.30f), new Vector2(0.82f, 0.40f), Vector2.zero, Vector2.zero);
                }
                wishlistButton.onClick.AddListener(OpenWishlist);
            }

            if (copySeedButton == null)
            {
                copySeedButton = FindNamedButton("CopyBuildSeedButton");
                if (copySeedButton == null)
                {
                    copySeedButton = CreateButton("CopyBuildSeedButton", contentRoot, "COPY BUILD SEED", RimaUITheme.PanelBorder, RimaUITheme.TextPrimary, 13f);
                    Stretch((RectTransform)copySeedButton.transform, new Vector2(0.18f, 0.20f), new Vector2(0.40f, 0.28f), Vector2.zero, Vector2.zero);
                }
                copySeedButton.onClick.AddListener(CopyBuildSeed);
            }

            if (restartButton == null)
            {
                restartButton = CreateButton("RestartButton", contentRoot, "TRY AGAIN [R]", RimaUITheme.PanelBorder, RimaUITheme.TextPrimary, 13f);
                Stretch((RectTransform)restartButton.transform, new Vector2(0.42f, 0.20f), new Vector2(0.62f, 0.28f), Vector2.zero, Vector2.zero);
            }

            if (mainMenuButton == null)
            {
                mainMenuButton = CreateButton("MainMenuButton", contentRoot, "MAIN MENU", RimaUITheme.PanelBorder, RimaUITheme.TextPrimary, 13f);
                Stretch((RectTransform)mainMenuButton.transform, new Vector2(0.64f, 0.20f), new Vector2(0.86f, 0.28f), Vector2.zero, Vector2.zero);
            }
            mainMenuButton.onClick.AddListener(LoadMainMenu);

            if (FindChildRecursive(contentRoot, "NextClassTeaser") == null)
            {
                RectTransform teaser = CreatePanel("NextClassTeaser", contentRoot, new Color(0.04f, 0.05f, 0.08f, 0.38f), new Color(0f, 1f, 0.8f, 0.24f));
                Stretch(teaser, new Vector2(0.62f, 0.04f), new Vector2(0.94f, 0.16f), Vector2.zero, Vector2.zero);
                TextMeshProUGUI teaserText = CreateText("NextClassTeaserText", teaser, "Next echo waits.", 12f, RimaUITheme.TextMuted, TextAlignmentOptions.Center);
                Stretch(teaserText.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            }
        }

        private GameObject FindChildGameObject(string objectName)
        {
            Transform child = FindChildRecursive(transform, objectName);
            if (child != null) return child.gameObject;

            GameObject activeObject = GameObject.Find(objectName);
            return activeObject;
        }

        private T FindChildComponent<T>(string objectName) where T : Component
        {
            GameObject child = FindChildGameObject(objectName);
            return child != null ? child.GetComponent<T>() : null;
        }

        private Button FindNamedButton(string objectName)
        {
            GameObject child = FindChildGameObject(objectName);
            return child != null ? child.GetComponent<Button>() : null;
        }

        private static Transform FindChildRecursive(Transform root, string objectName)
        {
            if (root == null) return null;
            if (root.name == objectName) return root;

            for (int i = 0; i < root.childCount; i++)
            {
                Transform found = FindChildRecursive(root.GetChild(i), objectName);
                if (found != null) return found;
            }

            return null;
        }

        private static void EnsureVisibleScale(Transform target)
        {
            if (target != null && target.localScale == Vector3.zero)
            {
                target.localScale = Vector3.one;
            }
        }

        private static RectTransform CreatePanel(string name, Transform parent, Color fill, Color edge)
        {
            Image panel = CreateImage(name, parent, fill);
            panel.sprite = RimaUITheme.SmallPanelFrame;
            panel.type = Image.Type.Sliced;

            Image border = CreateImage("Edge", panel.transform, edge);
            border.sprite = RimaUITheme.SmallPanelFrame;
            border.type = Image.Type.Sliced;
            border.raycastTarget = false;
            Stretch(border.rectTransform, Vector2.zero, Vector2.one, new Vector2(-3f, -3f), new Vector2(3f, 3f));
            border.transform.SetAsFirstSibling();
            return panel.rectTransform;
        }

        private static Image CreateImage(string name, Transform parent, Color color)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            Image img = go.AddComponent<Image>();
            img.color = color;
            return img;
        }

        private static TextMeshProUGUI CreateText(string name, Transform parent, string text, float size, Color color, TextAlignmentOptions alignment)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = size;
            tmp.color = color;
            tmp.alignment = alignment;
            tmp.raycastTarget = false;
            return tmp;
        }

        private static Button CreateButton(string name, Transform parent, string label, Color edge, Color textColor, float fontSize)
        {
            Image bg = CreateImage(name, parent, new Color(0.04f, 0.05f, 0.08f, 0.92f));
            bg.sprite = RimaUITheme.SmallPanelFrame;
            bg.type = Image.Type.Sliced;

            Image border = CreateImage("Edge", bg.transform, edge);
            border.sprite = RimaUITheme.SmallPanelFrame;
            border.type = Image.Type.Sliced;
            border.raycastTarget = false;
            Stretch(border.rectTransform, Vector2.zero, Vector2.one, new Vector2(-2f, -2f), new Vector2(2f, 2f));
            border.transform.SetAsFirstSibling();

            Button button = bg.gameObject.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = new Color(0.85f, 1f, 0.96f, 1f);
            colors.pressedColor = new Color(0.65f, 0.9f, 0.84f, 1f);
            button.colors = colors;

            TextMeshProUGUI text = CreateText("Label", bg.transform, label, fontSize, textColor, TextAlignmentOptions.Center);
            text.fontStyle = FontStyles.Bold;
            Stretch(text.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return button;
        }

        private static void Stretch(RectTransform rt, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            rt.anchorMin = min;
            rt.anchorMax = max;
            rt.offsetMin = offsetMin;
            rt.offsetMax = offsetMax;
        }

        private static string FormatSeconds(float seconds)
        {
            int total = Mathf.Max(0, Mathf.FloorToInt(seconds));
            return $"{total / 60:00}:{total % 60:00}";
        }

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;

            GameObject go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<InputSystemUIInputModule>();
        }
    }
}
