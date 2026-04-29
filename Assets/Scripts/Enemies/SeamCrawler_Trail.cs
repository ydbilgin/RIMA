using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// SeamCrawler'ın zemine hasar izi bırakan trail mekaniği.
    /// Bu component SeamCrawler prefabına eklenir (BaseMobBehavior yanında).
    /// Trail'de duran oyuncu hasar alır.
    ///
    /// Elite SeamCrawler: bu component kaldırılır, yerine SeamCrawler_Homing eklenir.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class SeamCrawler_Trail : MonoBehaviour
    {
        [Header("Trail")]
        [SerializeField] private float trailInterval   = 0.3f;   // ne sıklıkla iz bırakır
        [SerializeField] private float trailDuration   = 3f;     // iz ne kadar kalır
        [SerializeField] private float trailRadius     = 0.6f;
        [SerializeField] private int   trailTickDamage = 5;      // 0.5s'de bir
        [SerializeField] private float trailTickRate   = 0.5f;

        private BaseMobBehavior mob;
        private float           trailTimer;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
        }

        private void Update()
        {
            if (mob.CurrentState == BaseMobBehavior.MobState.Dead) return;
            if (mob.CurrentState != BaseMobBehavior.MobState.Chase) return;

            trailTimer -= Time.deltaTime;
            if (trailTimer <= 0f)
            {
                trailTimer = trailInterval;
                SpawnTrail(transform.position);
            }
        }

        private void SpawnTrail(Vector2 pos)
        {
            var go = new GameObject("SeamTrail");
            go.transform.position = pos;

            var col = go.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = trailRadius;

            var trail = go.AddComponent<TrailDamageZone>();
            trail.Init(trailTickDamage, trailTickRate, trailDuration);
        }
    }

    internal class TrailDamageZone : MonoBehaviour
    {
        private int   tickDamage;
        private float tickRate;
        private float tickTimer;

        public void Init(int dmg, float rate, float duration)
        {
            tickDamage = dmg;
            tickRate   = rate;
            tickTimer  = rate;
            Destroy(gameObject, duration);
        }

        private void Update()
        {
            tickTimer -= Time.deltaTime;
            if (tickTimer > 0f) return;
            tickTimer = tickRate;

            // Damage any player in zone
            var hits = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);
            foreach (var h in hits)
            {
                if (!h.CompareTag("Player")) continue;
                h.GetComponent<Health>()?.TakeDamage(tickDamage);
            }
        }
    }
}
