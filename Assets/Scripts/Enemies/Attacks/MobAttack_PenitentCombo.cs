using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Penitent'e özgü 3-hit combo saldırısı.
    ///   Hit 1 — 20 hasar
    ///   Hit 2 — 25 hasar
    ///   Hit 3 — 40 hasar + Weakened (armor break)
    /// Blazing affix varsa her vuruş Burning ekler.
    /// Overhead son vuruş kaçırılırsa kısa stun → Penitent stagger.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class MobAttack_PenitentCombo : MonoBehaviour
    {
        [Header("Combo Damage")]
        [SerializeField] private int hit1Damage = 20;
        [SerializeField] private int hit2Damage = 25;
        [SerializeField] private int hit3Damage = 40;

        [Header("Timing")]
        [SerializeField] private float attackCooldown  = 3.5f;
        [SerializeField] private float hitInterval     = 0.35f;
        [SerializeField] private float hitRadius       = 1.0f;

        [Header("Armor Break")]
        [SerializeField] private float weakenDuration = 4f;

        [Header("Miss Stagger")]
        [SerializeField] private float staggerDuration = 0.8f;

        [Header("Telegraph")]
        [SerializeField] private bool  useTelegraph      = true;
        [SerializeField] private float telegraphDuration = 0.35f;

        private BaseMobBehavior mob;
        private IMobAffix[]     affixes;
        private bool            comboRunning;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
            mob.attackCooldown = attackCooldown;
            mob.OnAttackReady += StartCombo;
        }

        private void Start() => affixes = GetComponents<IMobAffix>();

        private void OnDestroy() => mob.OnAttackReady -= StartCombo;

        private void StartCombo(Vector2 _)
        {
            if (comboRunning) return;
            StartCoroutine(ComboRoutine());
        }

        private IEnumerator ComboRoutine()
        {
            comboRunning = true;

            if (useTelegraph && telegraphDuration > 0f)
            {
                EnemyTelegraph.SpawnCircle(transform.position, hitRadius, telegraphDuration);
                yield return new WaitForSeconds(telegraphDuration);
            }

            yield return new WaitForSeconds(hitInterval);
            Strike(hit1Damage, armorBreak: false);

            yield return new WaitForSeconds(hitInterval);
            Strike(hit2Damage, armorBreak: false);

            yield return new WaitForSeconds(hitInterval);
            bool landed = Strike(hit3Damage, armorBreak: true);

            if (!landed)
            {
                // Overhead miss → Penitent stagger
                var rb = mob.Rb;
                if (rb != null) rb.linearVelocity = Vector2.zero;
                yield return new WaitForSeconds(staggerDuration);
            }

            comboRunning = false;
        }

        private bool Strike(int dmg, bool armorBreak)
        {
            bool hit = false;
            var hits = Physics2D.OverlapCircleAll(transform.position, hitRadius);

            foreach (var h in hits)
            {
                if (!h.CompareTag("Player")) continue;
                hit = true;

                h.GetComponent<Health>()?.TakeDamage(dmg);

                var status = h.GetComponent<StatusEffectSystem>();
                if (status != null)
                {
                    if (armorBreak)
                        status.ApplyEffect(StatusEffectType.Weakened, weakenDuration);

                    if (affixes != null)
                        foreach (var a in affixes) a.OnMeleeHit(status);
                }
            }
            return hit;
        }
    }
}
