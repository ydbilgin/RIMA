using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace RIMA
{
    /// <summary>
    /// Sandık sistemi.
    /// Oda temizlenince RuntimeRoomManager tarafından spawn edilir.
    /// Player yakına gelince [E] prompt gösterir, E'ye basınca ödül verir.
    ///
    /// TODO (tasarım güncellenince):
    ///   - Placeholder sarı kare sprite → gerçek sandık sprite (açık/kapalı)
    ///   - E-prompt → proper world-space UI veya screen-space tooltip
    ///   - Sadece HealSmall → reward pool (altın, skill shard, class-specific loot)
    ///   - Açılış animasyonu (zincir kırılma, kapak açılma)
    ///
    /// Prefab setup:
    ///   SpriteRenderer (veya PlaceholderSprite)
    ///   CircleCollider2D (isTrigger = true, radius ~1.2)
    ///   ChestBehavior
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ChestBehavior : MonoBehaviour
    {
        [Header("Reward")]
        [SerializeField] private RewardType rewardType = RewardType.Heal;
        [SerializeField] private float healSmallPercent = 0.20f;   // 20% max HP
        [SerializeField] private float healLargePercent = 0.40f;   // 40% max HP
        [SerializeField] private int   rageRefillAmount = 60;

        [Header("Interaction")]
        [SerializeField] private float interactRadius = 1.3f;

        // ─── State ───────────────────────────────────────────────────────────
        private bool opened;
        private bool playerInRange;

        // Player refs (found in Start)
        private Transform  playerTransform;
        private Health     playerHealth;
        private RageSystem playerRage;

        // Visuals
        private SpriteRenderer sr;
        private TextMeshPro    promptTMP;

        private static readonly Color ClosedColor = new Color(0.80f, 0.65f, 0.22f); // gold
        private static readonly Color OpenedColor = new Color(0.40f, 0.30f, 0.14f); // dark

        // ─── Lifecycle ───────────────────────────────────────────────────────

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = ClosedColor;

            // World-space interaction prompt (TextMeshPro, not UGUI)
            var promptGO = new GameObject("ChestPrompt");
            promptGO.transform.SetParent(transform);
            promptGO.transform.localPosition = new Vector3(0f, 0.55f, 0f);
            promptGO.transform.localScale    = Vector3.one * 0.018f;

            promptTMP = promptGO.AddComponent<TextMeshPro>();
            promptTMP.text      = "[E] Aç";
            promptTMP.fontSize  = 40;
            promptTMP.color     = Color.yellow;
            promptTMP.alignment = TextAlignmentOptions.Center;
            promptTMP.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 60);
            promptGO.SetActive(false);
        }

        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) return;

            playerTransform = player.transform;
            playerHealth    = player.GetComponent<Health>();
            playerRage      = player.GetComponent<RageSystem>();
        }

        private void Update()
        {
            if (opened || playerTransform == null) return;

            bool inRange = Vector2.Distance(transform.position, playerTransform.position) <= interactRadius;

            if (inRange != playerInRange)
            {
                playerInRange = inRange;
                if (promptTMP != null)
                    promptTMP.transform.parent.gameObject.SetActive(playerInRange);
            }

            if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
                Open();
        }

        // ─── Open logic ──────────────────────────────────────────────────────

        private void Open()
        {
            opened = true;
            if (promptTMP != null)
                promptTMP.transform.parent.gameObject.SetActive(false);
            if (sr != null) sr.color = OpenedColor;

            // ChestUI varsa seçim ekranı göster
            if (ChestUI.Instance != null)
            {
                var offers = BuildChestOffers();
                ChestUI.Instance.Show(offers, OnChestPicked);
            }
            else
            {
                // Fallback: direkt heal ver
                playerHealth?.Heal(Mathf.RoundToInt((playerHealth?.MaxHP ?? 100) * healSmallPercent));
                Destroy(gameObject, 1.5f);
            }
        }

        private System.Collections.Generic.List<RewardOffer> BuildChestOffers()
        {
            var offers = new System.Collections.Generic.List<RewardOffer>();
            offers.Add(RewardOffer.FromHeal(Mathf.RoundToInt(healSmallPercent * 100)));
            int goldAmount = 40 + Random.Range(0, 30);
            offers.Add(RewardOffer.FromGold(goldAmount));
            // 3. slot: skill (SkillDatabase'den rastgele) veya büyük heal
            if (SkillDatabase.Instance != null)
            {
                // Skill-reward fix-up (council 2026-06-14): GetPool filters placeholders
                // (isImplemented=false), retired offers AND class scope together, so the chest
                // can no longer offer an unbindable, retired or off-class skill (e.g. a Ranger
                // skill to a Warblade). Class comes from PlayerClassManager (default Warblade /
                // None secondary). Room-depth/rarity gating intentionally deferred (separate, larger).
                var primary = PlayerClassManager.Instance?.PrimaryClass ?? ClassType.Warblade;
                var secondary = PlayerClassManager.Instance?.SecondaryClass ?? ClassType.None;
                var pool = SkillDatabase.Instance.GetPool(primary, secondary);
                if (pool != null && pool.Count > 0)
                {
                    var pick = pool[Random.Range(0, pool.Count)];
                    offers.Add(RewardOffer.FromSkill(pick));
                    return offers;
                }
            }
            offers.Add(RewardOffer.FromHeal(Mathf.RoundToInt(healLargePercent * 100)));
            return offers;
        }

        private void OnChestPicked(RewardOffer offer)
        {
            switch (offer.type)
            {
                case RewardType.Heal:
                    if (playerHealth != null)
                        playerHealth.Heal(Mathf.RoundToInt(playerHealth.MaxHP * offer.healPercent / 100f));
                    break;
                case RewardType.Gold:
                    PlayerEconomy.Instance?.AddGold(offer.goldAmount);
                    break;
                case RewardType.Skill:
                    DraftManager.Instance?.ShowDraftWithSkill(offer.skill);
                    break;
            }
            Debug.Log($"[Chest] Picked → {offer.DisplayName}");
            Destroy(gameObject, 0.5f);
        }

        // ─── Gizmo ───────────────────────────────────────────────────────────

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactRadius);
        }
    }
}
