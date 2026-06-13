using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace RIMA
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class RewardPickup : MonoBehaviour
    {
        [SerializeField] private Canvas promptCanvas;
        [SerializeField] private Text promptText;
        [SerializeField] private float pickupVisualScale = 1.1f;

        private const Key InteractKey = Key.G;
        private bool collected;
        private bool playerInRange;

        public bool WasCollected => collected;

        /// <summary>
        /// BUG-2 fix: the final resting world-scale of the pickup sprite.
        /// RoomRunDirector.RewardSpawnPop uses this so the pop animation ends at the
        /// correct size (instead of always ending at Vector3.one).
        /// </summary>
        public float VisualScale => pickupVisualScale;

        private void Awake()
        {
            Collider2D trigger = GetComponent<Collider2D>();
            if (trigger != null) trigger.isTrigger = true;

            // T6.1 FIX: if no sprite assigned in Inspector, load the cyan rift-shard crystal.
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            transform.localScale = new Vector3(pickupVisualScale, pickupVisualScale, transform.localScale.z);
            if (sr != null && sr.sprite == null)
            {
                // The chest art is a 2-sprite SHEET; Resources.Load only returns the first (wrong,
                // 10x36) slice and can't address "_1" by path — load the sheet and pick the 62x72 icon.
                Sprite[] sheet = Resources.LoadAll<Sprite>("UI/RIMA/RIMA_UI_Node_Chest");
                Sprite riftShard = sheet != null ? System.Array.Find(sheet, s => s != null && s.name == "RIMA_UI_Node_Chest_1") : null;
                if (riftShard != null)
                {
                    sr.sprite = riftShard;
                    sr.color  = new Color(0.28f, 0.88f, 1f, 1f);  // cyan tint matching RIMA brand
                }
                else
                {
                    Debug.LogWarning("[RewardPickup] Failed to load RIMA_UI_Node_Chest_1 fallback sprite from sheet UI/RIMA/RIMA_UI_Node_Chest.");
                }
            }

            EnsurePromptVisuals();
        }

        private void Reset()
        {
            Collider2D trigger = GetComponent<Collider2D>();
            if (trigger != null) trigger.isTrigger = true;
        }

        private void Update()
        {
            if (!playerInRange || collected) return;
            if (Keyboard.current == null || !Keyboard.current[InteractKey].wasPressedThisFrame) return;

            Collect();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (collected || other == null || !other.CompareTag("Player")) return;

            playerInRange = true;
            ShowPrompt();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == null || !other.CompareTag("Player")) return;
            ClearPlayerRange();
        }

        /// <summary>
        /// BUG-1 fix: auto-grant the reward after timeout — same as Collect() but
        /// does NOT require the player to be in range. Called by RoomRunDirector when
        /// RewardAutoCollectTimeoutSec elapses so the reward is never silently discarded.
        /// </summary>
        public void ForceCollect()
        {
            if (collected) return;
            Debug.Log("[Reward] ForceCollect — auto-granting skill draft after timeout");
            Collect();
        }

        private void Collect()
        {
            if (collected) return;

            collected = true;
            ClearPlayerRange();
            Debug.Log("[Reward] collected — opening skill draft");
            RunStats.Instance.RecordRewardCollected();

            // Hades-style: collecting the relic opens the 3-card skill draft.
            // Hide the relic now, but defer opening the exit doors until the player has picked a card.
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null) sr.enabled = false;
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            StartCoroutine(DraftThenOpenExit());
        }

        private void ShowPrompt()
        {
            if (promptCanvas != null)
            {
                if (Camera.main != null) promptCanvas.worldCamera = Camera.main;
                promptCanvas.enabled = true;
            }

            if (promptText != null)
                promptText.text = Loc.T("reward.prompt.take");

            HUDController.Instance?.SetInteractionPrompt(Loc.T("reward.prompt.take"));
        }

        private void ClearPlayerRange()
        {
            if (!playerInRange) return;

            playerInRange = false;
            if (promptCanvas != null) promptCanvas.enabled = false;
            HUDController.Instance?.HideInteractionPrompt();
        }

        private void EnsurePromptVisuals()
        {
            if (promptCanvas != null) return;

            GameObject canvasGO = new GameObject("RewardPromptCanvas");
            canvasGO.transform.SetParent(transform, false);
            canvasGO.transform.localPosition = new Vector3(0f, 1.25f, 0f);
            canvasGO.transform.localScale = Vector3.one * 0.01f;

            promptCanvas = canvasGO.AddComponent<Canvas>();
            promptCanvas.renderMode = RenderMode.WorldSpace;
            promptCanvas.sortingOrder = 80;

            RectTransform canvasRt = canvasGO.GetComponent<RectTransform>();
            canvasRt.sizeDelta = new Vector2(180f, 36f);

            GameObject textGO = new GameObject("PromptText");
            textGO.transform.SetParent(canvasGO.transform, false);
            promptText = textGO.AddComponent<Text>();
            promptText.text = Loc.T("reward.prompt.take");
            promptText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            promptText.fontSize = 18;
            promptText.alignment = TextAnchor.MiddleCenter;
            promptText.color = Color.white;

            RectTransform textRt = textGO.GetComponent<RectTransform>();
            textRt.anchorMin = Vector2.zero;
            textRt.anchorMax = Vector2.one;
            textRt.offsetMin = Vector2.zero;
            textRt.offsetMax = Vector2.zero;
            promptCanvas.enabled = false;
        }

        private System.Collections.IEnumerator DraftThenOpenExit()
        {
            DraftManager draft = DraftManager.Instance;
            if (draft != null)
            {
                draft.ShowDraft();
                // Let the draft flag itself active (it sets IsDraftActive synchronously),
                // then wait until the player resolves it. Time is paused during the draft,
                // so poll on unscaled time with a safety guard.
                yield return null;
                float guard = 0f;
                while (draft.IsDraftActive && guard < 90f)
                {
                    guard += Time.unscaledDeltaTime;
                    yield return null;
                }
            }

            RoomClearVictoryTrigger.ActivateExitDoors();
            if (RuntimeRoomManager.Instance != null)
                RuntimeRoomManager.Instance.OpenDoorsAfterReward();

            Destroy(gameObject);
        }
    }
}
