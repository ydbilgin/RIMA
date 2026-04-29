using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Player HP, Rage barları + room status + gold counter.
    /// Referanslar Inspector'dan atanabilir veya isimle/runtime olarak bulunur.
    /// </summary>
    public class HUDManager : MonoBehaviour
    {
        [Header("HP Bar")]
        [SerializeField] private Image hpFill;

        [Header("Rage Bar")]
        [SerializeField] private Image rageFill;

        [Header("Room Info")]
        [SerializeField] private Text roomStatusText; // legacy — yeni sahnelerde TMP kullan

        [Header("Gold (TMP, opsiyonel)")]
        [SerializeField] private TextMeshProUGUI goldText;

        private Health     playerHealth;
        private RageSystem playerRage;

        // Runtime-built gold label ref
        private TextMeshProUGUI runtimeGoldText;

        private void Awake()
        {
            if (hpFill == null)
            {
                var go = GameObject.Find("HP_Bar_Fill");
                if (go != null) hpFill = go.GetComponent<Image>();
            }
            if (rageFill == null)
            {
                var go = GameObject.Find("Rage_Bar_Fill");
                if (go != null) rageFill = go.GetComponent<Image>();
            }
            if (roomStatusText == null)
            {
                var go = GameObject.Find("RoomStatus");
                if (go != null) roomStatusText = go.GetComponent<Text>();
            }
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerHealth = player.GetComponent<Health>();
                playerRage   = player.GetComponent<RageSystem>();

                if (playerHealth != null)
                {
                    playerHealth.OnHealthChanged.AddListener(UpdateHP);
                    UpdateHP(playerHealth.CurrentHP, playerHealth.MaxHP);
                }
                if (playerRage != null)
                {
                    playerRage.OnRageChanged.AddListener(UpdateRage);
                    UpdateRage(playerRage.CurrentRage, playerRage.MaxRage);
                }
            }

            // Gold display
            if (goldText == null) BuildGoldLabel();
            if (PlayerEconomy.Instance != null)
            {
                PlayerEconomy.Instance.OnGoldChanged.AddListener(UpdateGold);
                UpdateGold(PlayerEconomy.Instance.Gold);
            }
        }

        // ── Update callbacks ─────────────────────────────────────

        private void UpdateHP(int current, int max)
        {
            if (hpFill != null)
                hpFill.fillAmount = max > 0 ? (float)current / max : 0f;
        }

        private void UpdateRage(int current, int max)
        {
            if (rageFill != null)
                rageFill.fillAmount = max > 0 ? (float)current / max : 0f;
        }

        private void UpdateGold(int amount)
        {
            var target = goldText != null ? goldText : runtimeGoldText;
            if (target != null) target.text = $"⚙ {amount}";
        }

        public void SetRoomStatus(string text)
        {
            if (roomStatusText != null) roomStatusText.text = text;
        }

        // ── Runtime gold label ───────────────────────────────────

        private void BuildGoldLabel()
        {
            var canvas = GetComponentInParent<Canvas>() ?? FindObjectOfType<Canvas>();
            if (canvas == null) return;

            var go = new GameObject("GoldLabel", typeof(RectTransform));
            go.transform.SetParent(canvas.transform, false);
            var rt = go.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.78f, 0.94f);
            rt.anchorMax = new Vector2(0.99f, 1.00f);
            rt.offsetMin = rt.offsetMax = Vector2.zero;

            runtimeGoldText = go.AddComponent<TextMeshProUGUI>();
            runtimeGoldText.text = "⚙ 0";
            runtimeGoldText.fontSize = 14;
            runtimeGoldText.fontStyle = FontStyles.Bold;
            runtimeGoldText.color = new Color(0.92f, 0.78f, 0.22f);
            runtimeGoldText.alignment = TextAlignmentOptions.Right;
            runtimeGoldText.raycastTarget = false;
        }
    }
}

