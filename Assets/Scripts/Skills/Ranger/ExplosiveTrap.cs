using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 4 — Explosive Trap
    /// Zemine tuzak: düşman adım atınca 3s sonra patlama + slow 3s.
    /// </summary>
    public class ExplosiveTrap : SkillBase
    {
        [Header("Explosive Trap")]
        [SerializeField] private int   damage       = 55;
        [SerializeField] private float fuseTime     = 3f;
        [SerializeField] private float blastRadius  = 2.5f;
        [SerializeField] private float slowDuration = 3f;
        [SerializeField] private int   maxTraps     = 3;

        private static int activeTrapCount;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Explosive Trap";
            cooldown = 12f;
            resourceCost = 25;
        }

        protected override void Execute()
        {
            if (activeTrapCount >= maxTraps) return;
            activeTrapCount++;

            var go = new GameObject("ExplosiveTrap");
            go.transform.position = transform.position;

            var col = go.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.4f;

            var trap = go.AddComponent<TrapObject>();
            trap.Init(damage, fuseTime, blastRadius, slowDuration, () => activeTrapCount--);
        }
    }

    public class TrapObject : MonoBehaviour
    {
        private int damage;
        private float fuseTime;
        private float blastRadius;
        private float slowDuration;
        private System.Action onDestroyed;
        private bool triggered;

        public void Init(int dmg, float fuse, float radius, float slow, System.Action onDestroy)
        {
            damage = dmg; fuseTime = fuse; blastRadius = radius;
            slowDuration = slow; onDestroyed = onDestroy;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (triggered) return;
            if (other.CompareTag("Player")) return;
            if (other.GetComponent<Health>() == null) return;
            triggered = true;
            StartCoroutine(FuseRoutine());
        }

        private IEnumerator FuseRoutine()
        {
            yield return new WaitForSeconds(fuseTime);
            Explode();
        }

        private void Explode()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, blastRadius);
            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;
                h.GetComponent<Health>()?.TakeDamage(damage);
                h.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Chill, slowDuration);
            }
            onDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}
