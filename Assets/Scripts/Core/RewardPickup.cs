using UnityEngine;

namespace RIMA
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Collider2D))]
    public class RewardPickup : MonoBehaviour
    {
        private bool collected;

        public bool WasCollected => collected;

        private void Awake()
        {
            Collider2D trigger = GetComponent<Collider2D>();
            if (trigger != null) trigger.isTrigger = true;
        }

        private void Reset()
        {
            Collider2D trigger = GetComponent<Collider2D>();
            if (trigger != null) trigger.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (collected || other == null || !other.CompareTag("Player")) return;

            collected = true;
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

