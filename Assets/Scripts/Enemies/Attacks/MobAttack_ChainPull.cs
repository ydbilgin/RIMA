using System.Collections;
using UnityEngine;
using RIMA.Combat;

namespace RIMA
{
    /// <summary>
    /// ChainWarden'ın zincir çekme saldırısı.
    /// Oyuncuyu yakına çeker → kısa window sonra melee hasar.
    /// Void-Touched affix varsa çekme sonrası delayed zone bırakır.
    /// </summary>
    [RequireComponent(typeof(BaseMobBehavior))]
    public class MobAttack_ChainPull : MonoBehaviour
    {
        [Header("Chain Pull")]
        [SerializeField] private float pullRange    = 6f;
        [SerializeField] private float pullForce    = 20f;
        [SerializeField] private float pullDuration = 0.3f;
        [SerializeField] private int   followupDmg  = 18;
        [SerializeField] private float followupDelay = 0.5f;

        [Header("Timing")]
        [SerializeField] private float attackCooldown = 4f;

        [Header("Telegraph")]
        [SerializeField] private bool  useTelegraph      = true;
        [SerializeField] private float telegraphDuration = 0.45f;
        [SerializeField] private float telegraphWidth    = 0.35f;

        private BaseMobBehavior mob;
        private bool            pulling;
        private AttackTokenType _tokenType;

        private void Awake()
        {
            mob = GetComponent<BaseMobBehavior>();
            _tokenType = GetComponent<MobAttack_Throw>() != null || GetComponent<SeamCrawler_Homing>() != null
                ? AttackTokenType.Ranged
                : AttackTokenType.Melee;
            mob.attackCooldown = attackCooldown;
            // ChainWarden saldırısı uzak mesafede de tetiklenir
            mob.attackRange = pullRange;
            mob.OnAttackReady += TriggerPull;
        }

        private void OnDestroy() => mob.OnAttackReady -= TriggerPull;

        private void TriggerPull(Vector2 dir)
        {
            if (pulling || mob.Player == null) { ReturnToken(); return; }
            StartCoroutine(PullRoutine());
        }

        private IEnumerator PullRoutine()
        {
            pulling = true;

            if (useTelegraph && telegraphDuration > 0f && mob.Player != null)
            {
                Vector2 dir = ((Vector2)mob.Player.position - (Vector2)transform.position).normalized;
                EnemyTelegraph.SpawnLine(transform.position, dir, pullRange, telegraphWidth, telegraphDuration);
                yield return new WaitForSeconds(telegraphDuration);
            }

            if (mob.Player == null)
            {
                pulling = false;
                ReturnToken();
                yield break;
            }

            var playerRb = mob.Player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 toSelf = ((Vector2)transform.position - (Vector2)mob.Player.position).normalized;
                playerRb.AddForce(toSelf * pullForce, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(pullDuration);

            // Follow-up melee
            yield return new WaitForSeconds(followupDelay);
            if (mob.Player != null)
            {
                float dist = Vector2.Distance(transform.position, mob.Player.position);
                if (dist <= 1.5f)
                    mob.Player.GetComponent<Health>()?.TakeDamage(followupDmg);
            }

            // Void-Touched affix varsa delayed zone spawner tetiklenir (affix kendisi dinler)
            pulling = false;
            ReturnToken();
        }

        private void ReturnToken()
        {
            AttackTokenManager.Instance?.ReturnToken(gameObject, _tokenType);
        }
    }
}
