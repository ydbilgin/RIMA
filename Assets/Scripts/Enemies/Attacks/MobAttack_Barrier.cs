using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// RelicCaster — geçilmez bariyer/duvar oluşturur, oyuncunun hareketini kısıtlar.
    /// Bariyer 4s sonra yok olur ya da yeterli hasar alınca kırılır.
    /// Fractured affix: bariyer kırılınca shard saçılır (affix kendi dinler).
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class MobAttack_Barrier : MonoBehaviour
    {
        [Header("Barrier")]
        [SerializeField] private GameObject barrierPrefab;
        [SerializeField] private float      barrierDuration  = 4f;
        [SerializeField] private int        barrierHP        = 40;
        [SerializeField] private float      spawnOffset      = 2f;   // oyuncunun önünde

        [Header("Timing")]
        [SerializeField] private float attackCooldown = 6f;
        [SerializeField] private int   maxBarriers    = 2;

        [Header("Telegraph")]
        [SerializeField] private bool  useTelegraph      = true;
        [SerializeField] private float telegraphDuration = 0.5f;
        [SerializeField] private float telegraphRadius   = 0.9f;

        private BaseMobBehavior mob;
        private int             activeCount;
        private bool            casting;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
            mob.attackCooldown = attackCooldown;
            mob.attackRange    = 7f;   // uzaktan cast eder
            mob.OnAttackReady += SpawnBarrier;
        }

        private void OnDestroy() => mob.OnAttackReady -= SpawnBarrier;

        private void SpawnBarrier(Vector2 _)
        {
            if (casting) return;
            if (activeCount >= maxBarriers || mob.Player == null) return;
            if (barrierPrefab == null) return;
            StartCoroutine(BarrierRoutine());
        }

        private IEnumerator BarrierRoutine()
        {
            casting = true;

            // Oyuncu ile RelicCaster arasına yerleştir
            Vector2 toPlayer = ((Vector2)mob.Player.position - (Vector2)transform.position).normalized;
            Vector2 pos      = (Vector2)mob.Player.position - toPlayer * spawnOffset;

            if (useTelegraph && telegraphDuration > 0f)
            {
                EnemyTelegraph.SpawnCircle(pos, telegraphRadius, telegraphDuration);
                yield return new WaitForSeconds(telegraphDuration);
            }

            if (activeCount >= maxBarriers || barrierPrefab == null)
            {
                casting = false;
                yield break;
            }

            var go = Instantiate(barrierPrefab, pos, Quaternion.identity);
            activeCount++;

            var barrier = go.AddComponent<BarrierObject>();
            barrier.Init(barrierHP, barrierDuration, GetComponents<IMobAffix>(), () => activeCount--);
            casting = false;
        }
    }

    internal class BarrierObject : MonoBehaviour
    {
        private int           hp;
        private IMobAffix[]   affixes;
        private System.Action onDestroyed;
        private bool          notified;

        public void Init(int maxHP, float duration, IMobAffix[] afx, System.Action onDestroy)
        {
            hp          = maxHP;
            affixes     = afx;
            onDestroyed = onDestroy;
            Destroy(gameObject, duration);
        }

        public void TakeDamage(int amount)
        {
            hp -= amount;
            if (hp <= 0) Break();
        }

        private void Break()
        {
            // Fractured affix: shard scatter
            if (affixes != null)
                foreach (var a in affixes) a.OnProjectileSpawned(gameObject);

            NotifyDestroyed();
            Destroy(gameObject);
        }

        private void OnDestroy() => NotifyDestroyed();

        private void NotifyDestroyed()
        {
            if (notified) return;
            notified = true;
            onDestroyed?.Invoke();
        }
    }
}
