using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// FractureImp — melee vurduğunda shard saçar (ikincil hasar).
    /// MobAttack_Melee ile birlikte aynı GO'ya eklenir.
    /// Blazing affix varsa shard'lar Burning bırakır (MobAffix_Blazing halleder).
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class FractureImp_ShardScatter : MonoBehaviour
    {
        [Header("Shard Scatter")]
        [SerializeField] private GameObject shardPrefab;
        [SerializeField] private int   shardCount  = 4;
        [SerializeField] private float shardSpeed  = 5f;
        [SerializeField] private int   shardDamage = 8;
        [SerializeField] private float shardLife   = 1.5f;
        [SerializeField] private float spreadAngle = 45f;

        private BaseMobBehavior mob;
        private IMobAffix[]     affixes;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
            mob.OnAttackReady += ScatterOnAttack;
        }

        private void Start() => affixes = GetComponents<IMobAffix>();

        private void OnDestroy() => mob.OnAttackReady -= ScatterOnAttack;

        private void ScatterOnAttack(Vector2 dir)
        {
            if (shardPrefab == null) return;

            float step = shardCount > 1 ? spreadAngle / (shardCount - 1) : 0f;

            for (int i = 0; i < shardCount; i++)
            {
                float   a = -spreadAngle * 0.5f + step * i;
                Vector2 d = Rotate(dir, a);

                var go = Instantiate(shardPrefab, transform.position, Quaternion.identity);
                var rb = go.GetComponent<Rigidbody2D>();
                if (rb != null) { rb.gravityScale = 0f; rb.linearVelocity = d * shardSpeed; }

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

                // Blazing affix
                if (affixes != null)
                    foreach (var a2 in affixes) a2.OnProjectileSpawned(go);
            }
        }

        private static Vector2 Rotate(Vector2 v, float deg)
        {
            float r = deg * Mathf.Deg2Rad;
            float c = Mathf.Cos(r), s = Mathf.Sin(r);
            return new Vector2(v.x * c - v.y * s, v.x * s + v.y * c);
        }
    }
}
