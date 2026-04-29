using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Void-Touched Elite Affix — saldırı anında gecikmiş hasar zone bırakır.
    /// Ayrıca düşman kısa aralıklarla rastgele teleport yapar.
    /// Act 2-3 düşmanlarına eklenir.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class MobAffix_VoidTouched : MonoBehaviour, IMobAffix
    {
        [Header("Delayed Zone")]
        [SerializeField] private float zoneDelay    = 1.5f;
        [SerializeField] private float zoneRadius   = 2f;
        [SerializeField] private int   zoneDamage   = 20;
        [SerializeField] private float zoneDuration = 0.5f;

        [Header("Teleport")]
        [SerializeField] private float teleportInterval = 6f;
        [SerializeField] private float teleportRange    = 4f;
        [SerializeField] private Color eliteColor       = new Color(0.5f, 0.1f, 0.9f, 1f);

        private BaseMobBehavior mob;
        private float           teleportTimer;

        private void Start()
        {
            mob = GetComponent<BaseMobBehavior>();
            var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.color = eliteColor;

            var hp = GetComponent<Health>();
            if (hp != null) hp.SetMaxHP(Mathf.RoundToInt(hp.MaxHP * 1.5f));

            teleportTimer = teleportInterval;

            // Saldırı anında delayed zone
            mob.OnAttackReady += SpawnDelayedZone;
        }

        private void OnDestroy()
        {
            if (mob != null) mob.OnAttackReady -= SpawnDelayedZone;
        }

        private void Update()
        {
            if (mob == null || mob.CurrentState == BaseMobBehavior.MobState.Dead) return;
            teleportTimer -= Time.deltaTime;
            if (teleportTimer <= 0f)
            {
                teleportTimer = teleportInterval;
                Teleport();
            }
        }

        private void Teleport()
        {
            if (mob.Player == null) return;
            Vector2 offset = Random.insideUnitCircle.normalized * teleportRange;
            transform.position = (Vector2)mob.Player.position + offset;
        }

        private void SpawnDelayedZone(Vector2 _)
        {
            StartCoroutine(DelayedZoneRoutine(transform.position));
        }

        private IEnumerator DelayedZoneRoutine(Vector2 pos)
        {
            yield return new WaitForSeconds(zoneDelay);

            // Zone tetiklenir
            var hits = Physics2D.OverlapCircleAll(pos, zoneRadius);
            foreach (var h in hits)
            {
                if (!h.CompareTag("Player")) continue;
                h.GetComponent<Health>()?.TakeDamage(zoneDamage);
            }
        }

        // IMobAffix — VoidTouched ChainPull sonrası zone (ChainWarden)
        public void OnProjectileSpawned(GameObject projectile) { }

        public void OnMeleeHit(StatusEffectSystem target) { }
    }
}
