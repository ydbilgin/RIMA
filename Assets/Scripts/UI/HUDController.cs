using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RIMA
{
    public class HUDController : MonoBehaviour
    {
        [Header("Rage Bar")]
        [SerializeField] private RectTransform rageFill;
        [SerializeField] private Text rageLabel;

        [Header("HP Bar")]
        [SerializeField] private RectTransform hpFill;
        [SerializeField] private Text hpLabel;

        [Header("Currency")]
        [SerializeField] private Text goldLabel;
        [SerializeField] private Text goldDeltaLabel; // "+50" geçici yazı

        [Header("Room Status")]
        [SerializeField] private Text roomStatusLabel;

        [Header("Interaction Prompt")]
        [SerializeField] private RectTransform interactionPanel;
        [SerializeField] private Text          interactionText;

        [Header("Objective Arrow")]
        [SerializeField] private RectTransform objectiveArrowPanel;
        [SerializeField] private Text objectiveArrowText;
        [SerializeField] private float objectiveArrowEdgePadding = 72f;

        public static HUDController Instance { get; private set; }

        private RageSystem rageSystem;
        private Health     playerHealth;
        private Transform  playerTransform;

        private int  displayedGold;
        private Coroutine goldCountCoroutine;
        private Coroutine goldDeltaCoroutine;
        private Camera mainCamera;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            playerTransform = player.transform;
            rageSystem  = player.GetComponent<RageSystem>();
            playerHealth = player.GetComponent<Health>();
            mainCamera = Camera.main;

            // Setup programmatic prompt if missing
            if (interactionPanel == null) BuildProgrammaticPrompt();
            if (objectiveArrowPanel == null) BuildObjectiveArrow();

            if (interactionPanel != null) interactionPanel.gameObject.SetActive(false);
            if (objectiveArrowPanel != null) objectiveArrowPanel.gameObject.SetActive(false);
            
            if (rageSystem != null)
            {
                rageSystem.OnRageChanged.AddListener(OnRageChanged);
                OnRageChanged(rageSystem.Current, rageSystem.Max);
            }

            if (playerHealth != null)
            {
                playerHealth.OnHealthChanged.AddListener(OnHPChanged);
                OnHPChanged(playerHealth.CurrentHP, playerHealth.MaxHP);
            }

            if (PlayerEconomy.Instance != null)
            {
                PlayerEconomy.Instance.OnGoldChanged.AddListener(OnGoldChanged);
                displayedGold = PlayerEconomy.Instance.Gold;
                RefreshGoldText();
            }

            if (goldDeltaLabel != null) goldDeltaLabel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (rageSystem   != null) rageSystem.OnRageChanged.RemoveListener(OnRageChanged);
            if (playerHealth != null) playerHealth.OnHealthChanged.RemoveListener(OnHPChanged);
            if (PlayerEconomy.Instance != null)
                PlayerEconomy.Instance.OnGoldChanged.RemoveListener(OnGoldChanged);
        }

        private void Update()
        {
            UpdateObjectiveArrow();
        }

        // ── Rage ─────────────────────────────────────────────────

        private void OnRageChanged(int current, int max)
        {
            float pct = max > 0 ? (float)current / max : 0f;
            if (rageFill != null)
            {
                var parent = rageFill.parent as RectTransform;
                float width = parent != null ? parent.rect.width : 200f;
                rageFill.sizeDelta = new Vector2(width * pct, rageFill.sizeDelta.y);
            }
            if (rageLabel != null)
                rageLabel.text = $"RAGE {current}/{max}";
        }

        // ── HP ───────────────────────────────────────────────────

        private void OnHPChanged(int current, int max)
        {
            float pct = max > 0 ? (float)current / max : 0f;
            if (hpFill != null)
            {
                var parent = hpFill.parent as RectTransform;
                float width = parent != null ? parent.rect.width : 200f;
                hpFill.sizeDelta = new Vector2(width * pct, hpFill.sizeDelta.y);
            }
            if (hpLabel != null)
                hpLabel.text = $"HP {current}/{max}";
        }

        // ── Gold ─────────────────────────────────────────────────

        private void OnGoldChanged(int newTotal)
        {
            int delta = newTotal - displayedGold;

            // Delta animasyonu ("+50" yazısı)
            if (delta != 0 && goldDeltaLabel != null)
            {
                if (goldDeltaCoroutine != null) StopCoroutine(goldDeltaCoroutine);
                goldDeltaCoroutine = StartCoroutine(ShowDeltaText(delta));
            }

            // Count-up animasyonu
            if (goldCountCoroutine != null) StopCoroutine(goldCountCoroutine);
            goldCountCoroutine = StartCoroutine(CountUpGold(displayedGold, newTotal));
        }

        private IEnumerator CountUpGold(int from, int to)
        {
            float elapsed = 0f;
            float duration = Mathf.Clamp(Mathf.Abs(to - from) * 0.015f, 0.3f, 1.2f);
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                displayedGold = Mathf.RoundToInt(Mathf.Lerp(from, to, elapsed / duration));
                RefreshGoldText();
                yield return null;
            }
            displayedGold = to;
            RefreshGoldText();
        }

        private IEnumerator ShowDeltaText(int delta)
        {
            if (goldDeltaLabel == null) yield break;

            goldDeltaLabel.text  = delta > 0 ? $"+{delta}" : $"{delta}";
            goldDeltaLabel.color = delta > 0
                ? new Color(0.95f, 0.78f, 0.10f, 1f)
                : new Color(0.95f, 0.30f, 0.20f, 1f);
            goldDeltaLabel.gameObject.SetActive(true);

            // Soluklaştır
            float t = 0f;
            while (t < 1.2f)
            {
                t += Time.unscaledDeltaTime;
                float alpha = Mathf.Clamp01(1f - (t - 0.6f) / 0.6f);
                goldDeltaLabel.color = new Color(goldDeltaLabel.color.r,
                                                  goldDeltaLabel.color.g,
                                                  goldDeltaLabel.color.b, alpha);
                yield return null;
            }
            goldDeltaLabel.gameObject.SetActive(false);
        }

        private void RefreshGoldText()
        {
            if (goldLabel != null) goldLabel.text = $"{displayedGold} G";
        }

        // ── Public (dışarıdan çağrılabilir) ─────────────────────

        /// <summary>Manuel gold kazanımı bildirimi — HUD animasyonunu tetikler.</summary>
        public void NotifyGoldGain(int amount)
        {
            OnGoldChanged(displayedGold + amount);
        }

        /// <summary>Oda durumunu göster ("ROOM CLEARED", "⚠ Ödülünü almadın" vb.).</summary>
        public void SetRoomStatus(string text)
        {
            if (roomStatusLabel != null) roomStatusLabel.text = text;
        }

        // ── Interaction Prompt ───────────────────────────────────

        public void SetInteractionPrompt(string actionName)
        {
            if (interactionPanel == null) return;
            if (interactionText != null) interactionText.text = $"[G] {actionName}";
            interactionPanel.gameObject.SetActive(true);
        }

        public void HideInteractionPrompt()
        {
            if (interactionPanel != null) interactionPanel.gameObject.SetActive(false);
        }

        private void BuildProgrammaticPrompt()
        {
            // Panel
            var panelGO = new GameObject("InteractionPrompt", typeof(RectTransform));
            panelGO.transform.SetParent(transform, false);
            interactionPanel = panelGO.GetComponent<RectTransform>();
            interactionPanel.anchorMin = new Vector2(0.5f, 0.15f);
            interactionPanel.anchorMax = new Vector2(0.5f, 0.15f);
            interactionPanel.pivot     = new Vector2(0.5f, 0.5f);
            interactionPanel.sizeDelta = new Vector2(250f, 40f);
            interactionPanel.anchoredPosition = Vector2.zero;

            var bgImg = panelGO.AddComponent<Image>();
            bgImg.color = new Color(0f, 0f, 0f, 0.7f);

            // Text
            var txtGO = new GameObject("Text", typeof(RectTransform));
            txtGO.transform.SetParent(panelGO.transform, false);
            interactionText = txtGO.AddComponent<Text>();
            interactionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            interactionText.fontSize = 20;
            interactionText.alignment = TextAnchor.MiddleCenter;
            interactionText.color = Color.white;
            
            var txtRT = txtGO.GetComponent<RectTransform>();
            txtRT.anchorMin = Vector2.zero; txtRT.anchorMax = Vector2.one;
            txtRT.offsetMin = txtRT.offsetMax = Vector2.zero;
        }

        private void BuildObjectiveArrow()
        {
            var arrowGO = new GameObject("ObjectiveArrow", typeof(RectTransform));
            arrowGO.transform.SetParent(transform, false);
            objectiveArrowPanel = arrowGO.GetComponent<RectTransform>();
            objectiveArrowPanel.anchorMin = new Vector2(0.5f, 0.5f);
            objectiveArrowPanel.anchorMax = new Vector2(0.5f, 0.5f);
            objectiveArrowPanel.pivot = new Vector2(0.5f, 0.5f);
            objectiveArrowPanel.sizeDelta = new Vector2(46f, 46f);

            var bg = arrowGO.AddComponent<Image>();
            bg.color = new Color(0f, 0f, 0f, 0.55f);
            bg.raycastTarget = false;

            var txtGO = new GameObject("Text", typeof(RectTransform));
            txtGO.transform.SetParent(arrowGO.transform, false);
            objectiveArrowText = txtGO.AddComponent<Text>();
            objectiveArrowText.text = ">";
            objectiveArrowText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            objectiveArrowText.fontSize = 34;
            objectiveArrowText.fontStyle = FontStyle.Bold;
            objectiveArrowText.alignment = TextAnchor.MiddleCenter;
            objectiveArrowText.color = new Color(1f, 0.86f, 0.20f, 1f);
            objectiveArrowText.raycastTarget = false;

            var txtRT = txtGO.GetComponent<RectTransform>();
            txtRT.anchorMin = Vector2.zero;
            txtRT.anchorMax = Vector2.one;
            txtRT.offsetMin = Vector2.zero;
            txtRT.offsetMax = Vector2.zero;
        }

        private void UpdateObjectiveArrow()
        {
            if (objectiveArrowPanel == null) return;
            if (playerTransform == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                if (player != null) playerTransform = player.transform;
            }
            if (mainCamera == null) mainCamera = Camera.main;
            if (playerTransform == null || mainCamera == null)
            {
                objectiveArrowPanel.gameObject.SetActive(false);
                return;
            }

            Transform target = FindPendingObjectiveTarget();
            if (target == null)
            {
                objectiveArrowPanel.gameObject.SetActive(false);
                return;
            }

            Vector3 viewport = mainCamera.WorldToViewportPoint(target.position);
            bool isVisible = viewport.z > 0f &&
                             viewport.x >= 0.05f && viewport.x <= 0.95f &&
                             viewport.y >= 0.08f && viewport.y <= 0.92f;
            if (isVisible)
            {
                objectiveArrowPanel.gameObject.SetActive(false);
                return;
            }

            if (viewport.z < 0f)
            {
                viewport.x = 1f - viewport.x;
                viewport.y = 1f - viewport.y;
            }

            var root = transform as RectTransform;
            Vector2 canvasSize = root != null && root.rect.size.sqrMagnitude > 1f
                ? root.rect.size
                : new Vector2(Screen.width, Screen.height);

            float x = Mathf.Clamp(viewport.x, 0.05f, 0.95f);
            float y = Mathf.Clamp(viewport.y, 0.08f, 0.92f);
            Vector2 anchored = new Vector2(
                (x - 0.5f) * canvasSize.x,
                (y - 0.5f) * canvasSize.y);

            anchored.x = Mathf.Clamp(anchored.x, -canvasSize.x * 0.5f + objectiveArrowEdgePadding, canvasSize.x * 0.5f - objectiveArrowEdgePadding);
            anchored.y = Mathf.Clamp(anchored.y, -canvasSize.y * 0.5f + objectiveArrowEdgePadding, canvasSize.y * 0.5f - objectiveArrowEdgePadding);

            Vector2 dir = anchored.normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            objectiveArrowPanel.anchoredPosition = anchored;
            objectiveArrowPanel.localRotation = Quaternion.Euler(0f, 0f, angle);
            objectiveArrowPanel.gameObject.SetActive(true);
        }

        private Transform FindPendingObjectiveTarget()
        {
            Transform best = null;
            float bestDist = float.MaxValue;

            foreach (var reward in FindObjectsByType<RewardPickup>(FindObjectsSortMode.None))
            {
                if (reward == null || reward.WasCollected) continue;
                float dist = Vector2.Distance(playerTransform.position, reward.transform.position);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = reward.transform;
                }
            }

            foreach (var fragment in FindObjectsByType<MapFragment>(FindObjectsSortMode.None))
            {
                if (fragment == null) continue;
                float dist = Vector2.Distance(playerTransform.position, fragment.transform.position);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = fragment.transform;
                }
            }

            return best;
        }
    }
}
