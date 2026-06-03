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
            Debug.Log("[Reward] collected");
            RunStats.Instance.RecordRewardCollected();

            if (RuntimeRoomManager.Instance != null)
                RuntimeRoomManager.Instance.OpenDoorsAfterReward();

            Destroy(gameObject);
        }
    }
}

