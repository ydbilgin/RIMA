using RIMA.Systems.Map;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RIMA
{
    public class DemoCompleteOverlay : MonoBehaviour
    {
        // TODO(user, D4): replace app/0 with the real Steam App ID once the store page exists (GATED).
        private const string DefaultSteamWishlistUrl = "https://store.steampowered.com/app/0/";
        // Hidden until a real store page exists — app/0 placeholder would open a dead page in the jury demo.
        private const bool ShowWishlistButton = false;
        private static readonly Color BrandCyan = new Color(0f, 1f, 0.8f, 1f);
        private static readonly Color TarnishedGold = new Color(0.95f, 0.74f, 0.24f, 1f);

        [SerializeField] private string steamWishlistUrl = DefaultSteamWishlistUrl;

        private Canvas _canvas;

        public static void Show()
        {
            if (FindFirstObjectByType<DemoCompleteOverlay>() != null) return;

            GameObject go = new GameObject("DemoCompleteOverlay");
            go.AddComponent<DemoCompleteOverlay>().Build();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void HookDemoComplete()
        {
            RoomLoader.OnDemoComplete -= Show;
            RoomLoader.OnDemoComplete += Show;
        }

        private void Build()
        {
            Time.timeScale = 0f; // D1: full freeze — modal victory/CTA screen (UI runs unscaled, buttons still click).
            EnsureEventSystem();

            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            _canvas.sortingOrder = 200;
            gameObject.AddComponent<CanvasScaler>();
            gameObject.AddComponent<GraphicRaycaster>();

            Image bg = CreateImage("BG", transform, RimaUITheme.OverlayDark);
            Stretch(bg.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            // On-brand throne-room victory backdrop (cover/crop). VictoryRoot is a later sibling → on top.
            RimaUITheme.CreateFullScreenBackdrop(bg.transform, "UI/Backgrounds/victory_reward_bg", RimaUITheme.OverlayDark);

            RectTransform root = CreateRect("VictoryRoot", transform);
            Stretch(root, new Vector2(0.12f, 0.08f), new Vector2(0.88f, 0.92f), Vector2.zero, Vector2.zero);

            // T6.1 FIX: cyan clash → bone-white with heavy outline/shadow for legibility against the cyan VFX backdrop.
            TextMeshProUGUI title = CreateText("Title", root, Loc.T("victory.title"), 42f, new Color(0.98f, 0.95f, 0.86f, 1f), TextAlignmentOptions.Center);
            title.fontStyle = FontStyles.Bold;
            title.outlineWidth = 0.25f;
            title.outlineColor = new Color32(0, 0, 0, 210);
            Stretch(title.rectTransform, new Vector2(0f, 0.82f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);

            TextMeshProUGUI line = CreateText("VictoryLine", root, Loc.T("victory.subtitle"), 18f, RimaUITheme.TextMuted, TextAlignmentOptions.Center);
            Stretch(line.rectTransform, new Vector2(0f, 0.75f), new Vector2(1f, 0.84f), Vector2.zero, Vector2.zero);

            RectTransform summary = CreatePanel("RunSummaryPanel", root, new Color(0.04f, 0.05f, 0.08f, 0.82f), TarnishedGold);
            Stretch(summary, new Vector2(0.12f, 0.38f), new Vector2(0.88f, 0.74f), Vector2.zero, Vector2.zero);

            // T6.1 FIX: bone-white text, larger font, auto-size so content fits, outline for contrast vs cyan backdrop.
            TextMeshProUGUI summaryText = CreateText("RunSummaryText", summary, BuildRunSummary(), 20f, new Color(0.96f, 0.93f, 0.84f, 1f), TextAlignmentOptions.MidlineLeft);
            summaryText.enableAutoSizing = true;
            summaryText.fontSizeMin = 14f;
            summaryText.fontSizeMax = 22f;
            summaryText.outlineWidth = 0.15f;
            summaryText.outlineColor = new Color32(10, 10, 15, 220);
            Stretch(summaryText.rectTransform, Vector2.zero, Vector2.one, new Vector2(28f, 20f), new Vector2(-28f, -20f));

            RectTransform teaser = CreatePanel("NextClassTeaser", root, new Color(0.04f, 0.05f, 0.08f, 0.55f), new Color(0f, 1f, 0.8f, 0.32f));
            Stretch(teaser, new Vector2(0.18f, 0.35f), new Vector2(0.82f, 0.42f), Vector2.zero, Vector2.zero);

            // D2: show the generated next-class silhouette; fall back to text if the sprite isn't imported.
            Sprite silhouette = Resources.Load<Sprite>("UI/RIMA/next_class_silhouette");
            if (silhouette != null)
            {
                Image silImg = CreateImage("NextClassSilhouette", teaser, new Color(0f, 1f, 0.8f, 0.85f));
                silImg.sprite = silhouette;
                silImg.preserveAspect = true;
                silImg.raycastTarget = false;
                Stretch(silImg.rectTransform, new Vector2(0.02f, 0.05f), new Vector2(0.16f, 0.95f), Vector2.zero, Vector2.zero);

                TextMeshProUGUI teaserText = CreateText("NextClassTeaserText", teaser, Loc.T("victory.teaser"), 14f, RimaUITheme.TextMuted, TextAlignmentOptions.Left);
                Stretch(teaserText.rectTransform, new Vector2(0.18f, 0f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);
            }
            else
            {
                TextMeshProUGUI teaserText = CreateText("NextClassTeaserText", teaser, Loc.T("victory.teaser"), 14f, RimaUITheme.TextMuted, TextAlignmentOptions.Center);
                Stretch(teaserText.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            }

            if (ShowWishlistButton)
            {
                Button wishlist = CreateButton("WishlistButton", root, Loc.T("victory.btn.wishlist"), BrandCyan, RimaUITheme.BackgroundDark, 24f);
                Stretch((RectTransform)wishlist.transform, new Vector2(0.24f, 0.22f), new Vector2(0.76f, 0.33f), Vector2.zero, Vector2.zero);
                wishlist.onClick.AddListener(OpenWishlist);
            }

            // Wishlist hidden: the two bottom buttons grow into the vacated space.
            Button menu = CreateButton("MainMenuButton", root, Loc.T("victory.btn.main_menu"), RimaUITheme.PanelBorder, RimaUITheme.TextPrimary, 16f);
            Stretch((RectTransform)menu.transform, new Vector2(0.24f, 0.10f), new Vector2(0.48f, 0.22f), Vector2.zero, Vector2.zero);
            menu.onClick.AddListener(LoadMainMenu);

            Button again = CreateButton("PlayAgainButton", root, Loc.T("victory.btn.play_again"), RimaUITheme.PanelBorder, RimaUITheme.TextPrimary, 16f);
            Stretch((RectTransform)again.transform, new Vector2(0.52f, 0.10f), new Vector2(0.76f, 0.22f), Vector2.zero, Vector2.zero);
            again.onClick.AddListener(Restart);
        }

        private string BuildRunSummary()
        {
            RunStats stats = RunStats.Instance;
            int echoAward = EchoWallet.AwardRunIfNeeded(stats);

            int roomsCleared = stats != null ? stats.RoomsClearedForAward : 0;
            int kills         = stats != null ? stats.KillsForAward : 0;
            int roomLine      = roomsCleared * EchoWallet.RoomAwardEcho;
            int killLine      = kills / EchoWallet.KillsPerAwardEcho;
            int bonusLine     = Mathf.Max(0, echoAward - roomLine - killLine); // first-time / clamp

            // Her satır ayrı ayrı gösterilir (T6 Echo döküm paneli).
            return
                Loc.T("victory.stats.summary", RunStats.RoomReached, kills, FormatSeconds(RunStats.RunTimeSeconds)) + "\n" +
                $"\n" +
                $"Oda tamamlama  {roomsCleared} x 3 = +{roomLine}\n" +
                $"Kill ödülü     {kills} / 5 = +{killLine}\n" +
                (bonusLine > 0 ? $"İlk sefer bonusu = +{bonusLine}\n" : "") +
                $"─────────────────────────────\n" +
                Loc.T("death.stats.total_echo", echoAward);
        }

        private void OpenWishlist()
        {
            string url = string.IsNullOrWhiteSpace(steamWishlistUrl) ? DefaultSteamWishlistUrl : steamWishlistUrl;
            Application.OpenURL("steam://openurl/" + url);
            Application.OpenURL(url);
        }

        private void Restart()
        {
            Time.timeScale = 1f;
            MapFlowManager.Instance?.ResetRun();
            SceneManager.LoadScene("MainMenu");
        }

        private void LoadMainMenu()
        {
            Time.timeScale = 1f;
            MapFlowManager.Instance?.ResetRun();
            SceneManager.LoadScene("MainMenu");
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }

        private static string FormatSeconds(float seconds)
        {
            int total = Mathf.Max(0, Mathf.FloorToInt(seconds));
            return $"{total / 60:00}:{total % 60:00}";
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

        private static void EnsureEventSystem()
        {
            if (EventSystem.current != null) return;

            GameObject go = new GameObject("EventSystem");
            go.AddComponent<EventSystem>();
            go.AddComponent<InputSystemUIInputModule>();
        }
    }
}
