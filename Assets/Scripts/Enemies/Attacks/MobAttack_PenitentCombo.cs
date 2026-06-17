using System.Collections;
using UnityEngine;
using RIMA.Combat;

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
        // P0#5 (ChatGPT review 04 §4): demo-safe combo. Old 20/25/40 = 85 vs 100 HP could near-oneshot
        // the player from full. Softened to 10/12/20 = 42 total (~40-50 target) so a full combo is a
        // heavy punish, not a death sentence. Game-wide balance is unchanged — only this attack.
        [Header("Combo Damage")]
        [SerializeField] private int hit1Damage = 10;
        [SerializeField] private int hit2Damage = 12;
        [SerializeField] private int hit3Damage = 20;

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
        // P0#5: initial telegraph lengthened 0.35 -> 0.65s so the player can read & react to the combo.
        [SerializeField] private float telegraphDuration = 0.65f;
        // P0#5: distinct pre-tell before the heavy overhead 3rd hit (separate cue from the opening ring).
        [SerializeField] private float thirdHitTellDuration = 0.3f;

        private BaseMobBehavior mob;
        private IMobAffix[]     affixes;
        private bool            comboRunning;
        private AttackTokenType _tokenType;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
            _tokenType = GetComponent<MobAttack_Throw>() != null || GetComponent<SeamCrawler_Homing>() != null
                ? AttackTokenType.Ranged
                : AttackTokenType.Melee;
            mob.attackCooldown = attackCooldown;
            mob.OnAttackReady += StartCombo;
        }

        private void Start() => affixes = GetComponents<IMobAffix>();

        private void OnDestroy() => mob.OnAttackReady -= StartCombo;

        private void StartCombo(Vector2 _)
        {
            if (comboRunning) { ReturnToken(); return; }
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

            // P0#5: distinct tell before the heavy overhead 3rd hit — a separate, larger telegraph ring
            // so the armor-breaking finisher reads differently from hits 1 & 2 and can be dodged.
            if (useTelegraph && thirdHitTellDuration > 0f)
            {
                EnemyTelegraph.SpawnCircle(transform.position, hitRadius * 1.4f, thirdHitTellDuration);
                yield return new WaitForSeconds(thirdHitTellDuration);
            }
            else
            {
                yield return new WaitForSeconds(hitInterval);
            }

            bool landed = Strike(hit3Damage, armorBreak: true);

            if (!landed)
            {
                // Overhead miss → Penitent stagger
                var rb = mob.Rb;
                if (rb != null) rb.linearVelocity = Vector2.zero;
                yield return new WaitForSeconds(staggerDuration);
            }

            comboRunning = false;
            ReturnToken();
        }

        private void ReturnToken()
        {
            AttackTokenManager.Instance?.ReturnToken(gameObject, _tokenType);
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
