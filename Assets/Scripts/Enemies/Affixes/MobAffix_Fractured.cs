using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Fractured Elite Affix — ölünce shard saçılır veya echo spawn eder.
    /// RelicCaster Fractured: bariyer kırılınca shard'a dönüşür.
    /// Görsel: siyah + cyan çatlaklar (SpriteRenderer tint).
    /// Act 2-3 düşmanlarına eklenir.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class MobAffix_Fractured : MonoBehaviour, IMobAffix
    {
        [Header("Death Shards")]
        [SerializeField] private GameObject shardProjectilePrefab;
        [SerializeField] private int        shardCount  = 8;
        [SerializeField] private float      shardSpeed  = 6f;
        [SerializeField] private int        shardDamage = 10;
        [SerializeField] private float      shardLife   = 3f;

        [SerializeField] private Color eliteColor = new Color(0.1f, 0.1f, 0.15f, 1f);

        private BaseMobBehavior mob;

        private void Start()
        {
            mob = GetComponent<BaseMobBehavior>();

            var sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.color = eliteColor;

            var hp = GetComponent<Health>();
            if (hp != null) hp.SetMaxHP(Mathf.RoundToInt(hp.MaxHP * 1.5f));

            mob.OnDeathTriggered += ScatterShards;
        }

        private void OnDestroy()
        {
            if (mob != null) mob.OnDeathTriggered -= ScatterShards;
        }

        private void ScatterShards()
        {
            if (shardProjectilePrefab == null) return;

            for (int i = 0; i < shardCount; i++)
            {
                float   angle = i * (360f / shardCount);
                Vector2 dir   = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

                var go = Instantiate(shardProjectilePrefab, transform.position, Quaternion.identity);
                var rb = go.GetComponent<Rigidbody2D>();
                if (rb != null) { rb.gravityScale = 0f; rb.linearVelocity = dir * shardSpeed; }

                var projectile = go.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.Init(shardDamage, shardLife);
                }
                else
                {
                    var dmg = go.AddComponent<EnemyProjectileDamage>();
                    dmg.Init(shardDamage, shardLife);
                }
            }
        }

        // Bariyer kırılınca affix tetiklenir (BarrierObject → OnProjectileSpawned çağırır)
        public void OnProjectileSpawned(GameObject source) => ScatterShards();

        public void OnMeleeHit(StatusEffectSystem target) { }
    }
}
