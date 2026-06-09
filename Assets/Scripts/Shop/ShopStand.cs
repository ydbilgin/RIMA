using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RIMA.Shop
{
    /// <summary>
    /// A single shop offer stand in the Merchant room.
    /// Reuses the [G]-interact pattern from RoomRunExitDoorTrigger / RewardPickup:
    ///   - CircleCollider2D trigger detects Player proximity
    ///   - OnTriggerEnter2D → set HUD interaction prompt
    ///   - Update() → [G] pressed → attempt purchase
    ///   - Sold-out: prompt hidden, collider disabled, label updated to "SOLD"
    /// </summary>
    [RequireComponent(typeof(CircleCollider2D))]
    public sealed class ShopStand : MonoBehaviour
    {
        private const Key InteractKey = Key.G;

        private ShopOfferData offer;
        private bool soldOut;
        private bool playerInRange;

        // ── World-space label canvas ────────────────────────────────────────────────
        private Canvas labelCanvas;
        private Text nameText;
        private Text costText;
        private Text statusText;

        public bool IsSoldOut => soldOut;

        /// <summary>
        /// Called by ShopRoomController immediately after instantiation.
        /// </summary>
        public void Initialize(ShopOfferData offerData)
        {
            offer = offerData;
            RefreshLabel();
        }

        private void Awake()
        {
            CircleCollider2D trigger = GetComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius    = 1.1f;

            EnsureLabelCanvas();
        }

        private void Update()
        {
            if (soldOut || !playerInRange || offer == null) return;
            if (Keyboard.current == null || !Keyboard.current[InteractKey].wasPressedThisFrame) return;

            TryPurchase();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (soldOut || other == null || !other.CompareTag("Player")) return;

            playerInRange = true;
            string prompt = $"[G] {offer.displayName} — {offer.cost} Echo";
            HUDController.Instance?.SetInteractionPrompt(prompt);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == null || !other.CompareTag("Player")) return;
            ClearRange();
        }

        private void TryPurchase()
        {
            if (offer == null || soldOut) return;

            if (!EchoWallet.TrySpend(offer.cost))
            {
                // Insufficient Echo — flash the HUD prompt to show the reason.
                HUDController.Instance?.SetInteractionPrompt($"Not enough Echo! ({EchoWallet.Balance}/{offer.cost})");
                Debug.Log($"[ShopStand] Purchase failed — not enough Echo. Balance={EchoWallet.Balance} Cost={offer.cost}");
                return;
            }

            // Apply effect to player.
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            offer.Apply(player);

            Debug.Log($"[ShopStand] Purchased '{offer.displayName}' for {offer.cost} Echo. Remaining balance={EchoWallet.Balance}");

            MarkSoldOut();
        }

        private void MarkSoldOut()
        {
            soldOut = true;
            ClearRange();

            // Disable the collider so player re-entry doesn't trigger anything.
            CircleCollider2D trigger = GetComponent<CircleCollider2D>();
            if (trigger != null) trigger.enabled = false;

            // Update stand visuals.
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.color = new Color(0.35f, 0.35f, 0.35f, 0.7f);

            if (statusText != null)
            {
                statusText.text  = "SOLD";
                statusText.color = new Color(1f, 0.3f, 0.3f, 1f);
            }

            RefreshLabel();
        }

        private void ClearRange()
        {
            if (!playerInRange) return;
            playerInRange = false;
            HUDController.Instance?.HideInteractionPrompt();
        }

        // ── Label canvas helpers ────────────────────────────────────────────────────

        private void EnsureLabelCanvas()
        {
            if (labelCanvas != null) return;

            GameObject canvasGO = new GameObject("StandLabel");
            canvasGO.transform.SetParent(transform, false);
            canvasGO.transform.localPosition = new Vector3(0f, 0.75f, 0f);
            canvasGO.transform.localScale    = Vector3.one * 0.008f;

            labelCanvas             = canvasGO.AddComponent<Canvas>();
            labelCanvas.renderMode  = RenderMode.WorldSpace;
            labelCanvas.sortingLayerName = "Entities";
            labelCanvas.sortingOrder = 10;

            RectTransform rt = canvasGO.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(200f, 80f);

            Font builtIn = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            // Name row
            nameText = CreateLabel(canvasGO, "Name", Vector2.zero, new Vector2(200f, 26f),
                builtIn, 22, Color.white, TextAnchor.MiddleCenter);

            // Cost row
            costText = CreateLabel(canvasGO, "Cost", new Vector2(0f, -28f), new Vector2(200f, 22f),
                builtIn, 18, new Color(1f, 0.88f, 0.3f, 1f), TextAnchor.MiddleCenter);

            // Status row
            statusText = CreateLabel(canvasGO, "Status", new Vector2(0f, -52f), new Vector2(200f, 20f),
                builtIn, 16, new Color(0.6f, 1f, 0.9f, 1f), TextAnchor.MiddleCenter);
        }

        private static Text CreateLabel(GameObject parent, string childName, Vector2 offset, Vector2 size,
            Font font, int fontSize, Color color, TextAnchor alignment)
        {
            GameObject go = new GameObject(childName);
            go.transform.SetParent(parent.transform, false);

            Text text        = go.AddComponent<Text>();
            text.font        = font;
            text.fontSize    = fontSize;
            text.color       = color;
            text.alignment   = alignment;
            text.supportRichText = false;

            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = offset;
            rt.sizeDelta        = size;

            return text;
        }

        private void RefreshLabel()
        {
            if (offer == null) return;

            if (nameText  != null) nameText.text  = offer.displayName;
            if (statusText != null && !soldOut)    statusText.text = offer.description;
            if (costText   != null && !soldOut)    costText.text   = $"{offer.cost} Echo";
            if (costText   != null && soldOut)     costText.text   = "";
        }
    }
}
