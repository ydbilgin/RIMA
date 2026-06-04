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
        private const string PackButtonPath = "UI/RIMA/Pack/button_9slice";
        private static readonly string[] DeathLines =
        {
            "The rift remembers. You won't.",
            "Not an ending. Just a place where you stopped."
        };

        [Header("UI References (auto-found or auto-created if null)")]
        [SerializeField] private GameObject deathPanel;
        [SerializeField] private TextMeshProUGUI deathTitle;
        [SerializeField] private Image deathDivider;
        [SerializeField] private TextMeshProUGUI deathStats;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;

        [Header("Settings")]
        [SerializeField] private float fadeInDuration = 0.8f;
        [SerializeField] private float slowMoScale = 0.15f;
        [SerializeField] private float slowMoDuration = 1.5f;

        private Health playerHealth;
        private CanvasGroup canvasGroup;
        private bool isDead;

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

        private string BuildRunStats()
        {
            return $"ODA {RunStats.RoomReached} · KILLS {RunStats.Kills} · SÜRE {FormatSeconds(RunStats.RunTimeSeconds)}";
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

            Image bg = CreateImage("DeathScreen", parent, WithAlpha(RimaUITheme.OverlayDark, 0.24f));
            Stretch(bg.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            // On-brand cracked-seal death backdrop, translucent so the frozen combat field remains visible.
            Image backdrop = RimaUITheme.CreateFullScreenBackdrop(bg.transform, "UI/Backgrounds/death_screen_bg", WithAlpha(RimaUITheme.OverlayDark, 0.18f));
            backdrop.color = WithAlpha(backdrop.color, backdrop.sprite != null ? 0.50f : 0.18f);
            CreateVignette("Vignette", bg.transform, WithAlpha(RimaUITheme.OverlayDark, 0.78f));

            RectTransform panel = CreateRect("DeathPanel", bg.transform);
            Stretch(panel, new Vector2(0.18f, 0.16f), new Vector2(0.82f, 0.84f), Vector2.zero, Vector2.zero);
            return bg.gameObject;
        }

        private void EnsurePanelControls()
        {
            if (deathTitle == null)
                deathTitle = FindChildComponent<TextMeshProUGUI>("DeathTitle");

            if (deathStats == null)
                deathStats = FindChildComponent<TextMeshProUGUI>("DeathStats");

            if (deathDivider == null)
                deathDivider = FindChildComponent<Image>("DeathDivider");

            if (restartButton == null)
                restartButton = FindChildComponent<Button>("RestartButton");

            if (mainMenuButton == null)
                mainMenuButton = FindChildComponent<Button>("MainMenuButton");

            Transform contentRoot = FindChildRecursive(deathPanel.transform, "DeathPanel");
            if (contentRoot == null)
            {
                RectTransform generatedRoot = CreateRect("DeathPanel", deathPanel.transform);
                Stretch(generatedRoot, new Vector2(0.18f, 0.16f), new Vector2(0.82f, 0.84f), Vector2.zero, Vector2.zero);
                contentRoot = generatedRoot;
            }

            RemoveLegacyElement(contentRoot, "CopyBuildSeedButton");
            RemoveLegacyElement(contentRoot, "WishlistButton");
            RemoveLegacyElement(contentRoot, "NextClassTeaser");
            RemovePanelGraphic(contentRoot);

            if (deathTitle == null)
                deathTitle = CreateText("DeathTitle", contentRoot, "", 26f, RimaUITheme.TextPrimary, TextAlignmentOptions.Center);
            deathTitle.fontSize = 28f;
            deathTitle.color = RimaUITheme.TextPrimary;
            deathTitle.alignment = TextAlignmentOptions.Center;
            Stretch(deathTitle.rectTransform, new Vector2(0.04f, 0.62f), new Vector2(0.96f, 0.82f), Vector2.zero, Vector2.zero);

            if (deathDivider == null)
                deathDivider = CreateImage("DeathDivider", contentRoot, WithAlpha(RimaUITheme.Cyan, 0.46f));
            deathDivider.color = WithAlpha(RimaUITheme.Cyan, 0.46f);
            deathDivider.raycastTarget = false;
            Stretch(deathDivider.rectTransform, new Vector2(0.30f, 0.555f), new Vector2(0.70f, 0.555f), new Vector2(0f, -1f), new Vector2(0f, 1f));

            if (deathStats == null)
                deathStats = CreateText("DeathStats", contentRoot, "", 16f, RimaUITheme.TextMuted, TextAlignmentOptions.Center);
            deathStats.fontSize = 15f;
            deathStats.color = RimaUITheme.TextMuted;
            deathStats.alignment = TextAlignmentOptions.Center;
            Stretch(deathStats.rectTransform, new Vector2(0.06f, 0.45f), new Vector2(0.94f, 0.54f), Vector2.zero, Vector2.zero);

            if (restartButton == null)
                restartButton = CreateButton("RestartButton", contentRoot, "TEKRAR DENE [R]", 15f);
            StyleButton(restartButton, "TEKRAR DENE [R]", 15f);
            Stretch((RectTransform)restartButton.transform, new Vector2(0.20f, 0.28f), new Vector2(0.49f, 0.38f), Vector2.zero, Vector2.zero);

            if (mainMenuButton == null)
                mainMenuButton = CreateButton("MainMenuButton", contentRoot, "ANA MENÜ", 15f);
            StyleButton(mainMenuButton, "ANA MENÜ", 15f);
            Stretch((RectTransform)mainMenuButton.transform, new Vector2(0.51f, 0.28f), new Vector2(0.80f, 0.38f), Vector2.zero, Vector2.zero);
            mainMenuButton.onClick.AddListener(LoadMainMenu);
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

        private static RectTransform CreateRect(string name, Transform parent)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            return go.GetComponent<RectTransform>();
        }

        private static Image CreateImage(string name, Transform parent, Color color)
        {
            GameObject go = new GameObject(name, typeof(RectTransform));
            go.transform.SetParent(parent, false);
            Image img = go.AddComponent<Image>();
            img.color = color;
            return img;
        }

        private static void CreateVignette(string name, Transform parent, Color edgeColor)
        {
            Image vignette = CreateImage(name, parent, edgeColor);
            vignette.sprite = CreateVignetteSprite(edgeColor);
            vignette.raycastTarget = false;
            Stretch(vignette.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
        }

        private static Sprite CreateVignetteSprite(Color edgeColor)
        {
            const int size = 128;
            Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.filterMode = FilterMode.Bilinear;

            Vector2 center = new Vector2((size - 1) * 0.5f, (size - 1) * 0.5f);
            float maxDistance = center.magnitude;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float distance = Vector2.Distance(new Vector2(x, y), center) / maxDistance;
                    float alpha = Mathf.SmoothStep(0f, edgeColor.a, Mathf.InverseLerp(0.32f, 1f, distance));
                    tex.SetPixel(x, y, new Color(edgeColor.r, edgeColor.g, edgeColor.b, alpha));
                }
            }

            tex.Apply(false, true);
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
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

        private static Button CreateButton(string name, Transform parent, string label, float fontSize)
        {
            Image bg = CreateImage(name, parent, WithAlpha(RimaUITheme.PanelTint, 0.34f));
            bg.sprite = Resources.Load<Sprite>(PackButtonPath) ?? RimaUITheme.SmallPanelFrame;
            bg.type = Image.Type.Sliced;

            Image border = CreateImage("Edge", bg.transform, WithAlpha(RimaUITheme.Cyan, 0.44f));
            border.sprite = RimaUITheme.SmallPanelFrame;
            border.type = Image.Type.Sliced;
            border.raycastTarget = false;
            Stretch(border.rectTransform, Vector2.zero, Vector2.one, new Vector2(-2f, -2f), new Vector2(2f, 2f));
            border.transform.SetAsFirstSibling();

            Button button = bg.gameObject.AddComponent<Button>();
            button.targetGraphic = bg;
            ApplyButtonColors(button);

            TextMeshProUGUI text = CreateText("Label", bg.transform, label, fontSize, RimaUITheme.TextPrimary, TextAlignmentOptions.Center);
            text.fontStyle = FontStyles.Bold;
            Stretch(text.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return button;
        }

        private static void StyleButton(Button button, string label, float fontSize)
        {
            if (button == null) return;

            Image bg = button.GetComponent<Image>();
            if (bg != null)
            {
                bg.color = WithAlpha(RimaUITheme.PanelTint, 0.34f);
                bg.sprite = Resources.Load<Sprite>(PackButtonPath) ?? RimaUITheme.SmallPanelFrame;
                bg.type = Image.Type.Sliced;
                button.targetGraphic = bg;
            }

            Transform edge = FindChildRecursive(button.transform, "Edge");
            if (edge == null)
            {
                Image border = CreateImage("Edge", button.transform, WithAlpha(RimaUITheme.Cyan, 0.44f));
                border.sprite = RimaUITheme.SmallPanelFrame;
                border.type = Image.Type.Sliced;
                border.raycastTarget = false;
                Stretch(border.rectTransform, Vector2.zero, Vector2.one, new Vector2(-2f, -2f), new Vector2(2f, 2f));
                border.transform.SetAsFirstSibling();
            }
            else if (edge.TryGetComponent(out Image edgeImage))
            {
                edgeImage.color = WithAlpha(RimaUITheme.Cyan, 0.44f);
            }

            TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>(true);
            if (text == null)
            {
                text = CreateText("Label", button.transform, label, fontSize, RimaUITheme.TextPrimary, TextAlignmentOptions.Center);
                Stretch(text.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            }
            text.text = label;
            text.fontSize = fontSize;
            text.color = RimaUITheme.TextPrimary;
            text.fontStyle = FontStyles.Bold;
            text.alignment = TextAlignmentOptions.Center;

            ApplyButtonColors(button);
        }

        private static void ApplyButtonColors(Button button)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = WithAlpha(RimaUITheme.PanelTint, 0.34f);
            colors.highlightedColor = WithAlpha(RimaUITheme.Cyan, 0.24f);
            colors.pressedColor = WithAlpha(RimaUITheme.Cyan, 0.42f);
            colors.selectedColor = WithAlpha(RimaUITheme.Cyan, 0.24f);
            colors.disabledColor = WithAlpha(RimaUITheme.TextMuted, 0.18f);
            button.colors = colors;
        }

        private static void RemoveLegacyElement(Transform root, string objectName)
        {
            Transform target = FindChildRecursive(root, objectName);
            if (target == null) return;

            if (Application.isPlaying)
                Destroy(target.gameObject);
            else
                DestroyImmediate(target.gameObject);
        }

        private static void RemovePanelGraphic(Transform contentRoot)
        {
            Image panelImage = contentRoot.GetComponent<Image>();
            if (panelImage != null)
            {
                panelImage.color = Color.clear;
                panelImage.raycastTarget = false;
            }

            Transform edge = null;
            for (int i = 0; i < contentRoot.childCount; i++)
            {
                Transform child = contentRoot.GetChild(i);
                if (child.name == "Edge")
                {
                    edge = child;
                    break;
                }
            }

            if (edge != null)
            {
                if (Application.isPlaying)
                    Destroy(edge.gameObject);
                else
                    DestroyImmediate(edge.gameObject);
            }
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

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
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
