using System.Collections;
using UnityEngine;
using RIMA.Combat;

namespace RIMA
{
    /// <summary>
    /// Temel melee saldırı — yakın dövüş düşmanları kullanır.
    /// Penitent için MobAttack_PenitentCombo'yu kullanın (3-hit combo).
    /// FractureImp bu component'i kullanır + FractureImp_ShardScatter eklenir.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class MobAttack_Melee : MonoBehaviour
    {
        [Header("Melee")]
        [SerializeField] private int   damage         = 14;
        [SerializeField] private float attackCooldown = 1.2f;
        [SerializeField] private float hitRadius      = 0.8f;

        [Header("Telegraph")]
        [SerializeField] private bool  useTelegraph      = true;
        [SerializeField] private float telegraphDuration = 0.25f;

        private BaseMobBehavior mob;
        private IMobAffix[]     affixes;
        private bool windingUp;
        private AttackTokenType _tokenType;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
            _tokenType = GetComponent<MobAttack_Throw>() != null || GetComponent<SeamCrawler_Homing>() != null
                ? AttackTokenType.Ranged
                : AttackTokenType.Melee;
            mob.attackCooldown = attackCooldown;
            mob.OnAttackReady += DoMelee;
        }

        private void Start() => affixes = GetComponents<IMobAffix>();

        private void OnDestroy() => mob.OnAttackReady -= DoMelee;

        private void DoMelee(Vector2 _)
        {
            if (windingUp) { ReturnToken(); return; }
            StartCoroutine(MeleeRoutine());
        }

        private IEnumerator MeleeRoutine()
        {
            windingUp = true;

            if (useTelegraph && telegraphDuration > 0f)
            {
                EnemyTelegraph.SpawnCircle(transform.position, hitRadius, telegraphDuration);
                yield return new WaitForSeconds(telegraphDuration);
            }

            var hits = Physics2D.OverlapCircleAll(transform.position, hitRadius);
            foreach (var h in hits)
            {
                if (!h.CompareTag("Player")) continue;

                h.GetComponent<Health>()?.TakeDamage(damage);

                var status = h.GetComponent<StatusEffectSystem>();
                if (status != null && affixes != null)
                    foreach (var a in affixes) a.OnMeleeHit(status);
            }

            windingUp = false;
            ReturnToken();
        }

        private void ReturnToken()
        {
            AttackTokenManager.Instance?.ReturnToken(gameObject, _tokenType);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, hitRadius);
        }
    }
}
