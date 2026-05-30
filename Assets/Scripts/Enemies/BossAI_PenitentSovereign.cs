using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Penitent Sovereign boss AI — Faz 1 (100% → 50% HP placeholder death).
    ///
    /// 4 saldırı döngüsü:
    ///   1. Zincir Kamçı   — 6m ön yönünde çizgi hasar
    ///   2. Penitent Surge — 4m AoE etrafı + knockback
    ///   3. Kelepçe Fırlatma — oyuncuya projectile linecast + slow
    ///   4. Kutsanmış Kırbaç — 180° yay, yakın mesafe
    ///
    /// %50 HP'de: collapse animasyonu → placeholder ölüm (Faz 2 Faz 2'de).
    /// </summary>
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(KnockbackReceiver))]
    [System.Obsolete("Not the live spine - see WORK_ORDER_24_48H_S6. Live boss = Enemies/Boss/PenitentSovereign.cs.")]
    public class BossAI_PenitentSovereign : MonoBehaviour
    {
        // ─── Inspector ────────────────────────────────────────────────────────

        [Header("Stats")]
        [SerializeField] private int maxHP = 500;

        [Header("Attack Damage")]
        [SerializeField] private int chainWhipDamage    = 25;
        [SerializeField] private int penitentSurgeDamage = 20;
        [SerializeField] private int shackleThrowDamage  = 15;
        [SerializeField] private int blessedWhipDamage   = 30;

        [Header("Attack Ranges")]
        [SerializeField] private float chainWhipRange    = 6f;
        [SerializeField] private float surgeRadius       = 4f;
        [SerializeField] private float blessedWhipRadius = 2.5f;
        [SerializeField] private float surgeKnockback    = 8f;

        [Header("Timing")]
        [SerializeField] private float firstAttackDelay = 2f;
        [SerializeField] private float attackInterval   = 3f;
        [SerializeField] private float windupWhip       = 0.6f;
        [SerializeField] private float windupSurge      = 0.8f;
        [SerializeField] private float windupShackle    = 0.5f;
        [SerializeField] private float windupBlessed    = 0.5f;

        [Header("References")]
        [SerializeField] private BossHealthBar bossHealthBar;
        [SerializeField] private string bossDisplayName = "Penitent Sovereign";

        // ─── Runtime ──────────────────────────────────────────────────────────

        private Health   health;
        private Rigidbody2D rb;
        private SpriteRenderer sr;
        private Transform player;

        private bool isDead;
        private bool inPhaseTransition;
        private bool phaseTriggered;
        private float attackTimer;
        private int   attackIndex;

        private static readonly int PlayerLayer = -1; // resolved in Awake

        // ─── Unity ────────────────────────────────────────────────────────────

        private void Awake()
        {
            health = GetComponent<Health>();
            health.SetMaxHP(maxHP);

            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale  = 0f;
            rb.freezeRotation = true;

            sr = GetComponentInChildren<SpriteRenderer>();
        }

        private void Start()
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) player = playerObj.transform;

            // Boss health bar
            if (bossHealthBar == null)
                bossHealthBar = FindAnyObjectByType<BossHealthBar>();

            if (bossHealthBar != null)
            {
                bossHealthBar.Initialize(bossDisplayName, maxHP);
                bossHealthBar.Show();
            }

            health.OnHealthChanged.AddListener(OnHealthChanged);
            health.OnDeath.AddListener(OnDeath);

            attackTimer = firstAttackDelay;
        }

        private void Update()
        {
            if (isDead || inPhaseTransition || player == null) return;

            FacePlayer();

            attackTimer -= Time.deltaTime;
            if (attackTimer <= 0f)
            {
                attackTimer = attackInterval;
                StartCoroutine(DoNextAttack());
            }
        }

        // ─── Attack Selection ─────────────────────────────────────────────────

        private IEnumerator DoNextAttack()
        {
            float dist = Vector2.Distance(transform.position, player.position);

            // Long-range: prefer Shackle; short-range: rotate 1-2-4
            int idx = attackIndex % 4;
            if (dist > 5f && idx == 2)
                yield return StartCoroutine(ShackleThrow());
            else
            {
                switch (idx)
                {
                    case 0: yield return StartCoroutine(ChainWhip());    break;
                    case 1: yield return StartCoroutine(PenitentSurge()); break;
                    case 2: yield return StartCoroutine(ShackleThrow());  break;
                    case 3: yield return StartCoroutine(BlessedWhip());   break;
                }
            }

            attackIndex++;
        }

        // ─── Attack 1: Zincir Kamçı ──────────────────────────────────────────
        // 6m straight line in player direction, warning: arm pulled back

        private IEnumerator ChainWhip()
        {
            yield return new WaitForSeconds(windupWhip);
            if (isDead || player == null) yield break;

            Vector2 dir    = ((Vector2)player.position - (Vector2)transform.position).normalized;
            Vector2 origin = transform.position;

            var hits = Physics2D.BoxCastAll(origin, new Vector2(0.7f, 0.7f), 0f, dir,
                                            chainWhipRange, LayerMask.GetMask("Player"));
            foreach (var h in hits)
                h.collider.GetComponent<Health>()?.TakeDamage(chainWhipDamage);

            yield return new WaitForSeconds(0.4f);
        }

        // ─── Attack 2: Penitent Surge ─────────────────────────────────────────
        // 4m AoE circle + knockback, warning: ground punch

        private IEnumerator PenitentSurge()
        {
            yield return new WaitForSeconds(windupSurge);
            if (isDead) yield break;

            var hits = Physics2D.OverlapCircleAll(transform.position, surgeRadius,
                                                  LayerMask.GetMask("Player"));
            foreach (var hit in hits)
            {
                hit.GetComponent<Health>()?.TakeDamage(penitentSurgeDamage);

                var kb = hit.GetComponent<KnockbackReceiver>();
                if (kb != null)
                {
                    Vector2 dir = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
                    kb.ApplyKnockback(dir, surgeKnockback, 0.35f);
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        // ─── Attack 3: Kelepçe Fırlatma ──────────────────────────────────────
        // Linecast to player, 2s slow (velocity reduction via coroutine on player)

        private IEnumerator ShackleThrow()
        {
            yield return new WaitForSeconds(windupShackle);
            if (isDead || player == null) yield break;

            var hit = Physics2D.Linecast(transform.position, player.position,
                                         LayerMask.GetMask("Player"));
            if (hit.collider != null)
            {
                hit.collider.GetComponent<Health>()?.TakeDamage(shackleThrowDamage);

                // 2 stacks Chill = %20 slow for 2s via StatusEffectSystem
                var sse = hit.collider.GetComponent<StatusEffectSystem>();
                sse?.ApplyEffect(StatusEffectType.Chill, 2f, 2);
            }

            yield return new WaitForSeconds(0.4f);
        }

        // ─── Attack 4: Kutsanmış Kırbaç ──────────────────────────────────────
        // 180° arc in front, close range

        private IEnumerator BlessedWhip()
        {
            yield return new WaitForSeconds(windupBlessed);
            if (isDead || player == null) yield break;

            Vector2 forward = ((Vector2)player.position - (Vector2)transform.position).normalized;
            var hits = Physics2D.OverlapCircleAll(transform.position, blessedWhipRadius,
                                                  LayerMask.GetMask("Player"));
            foreach (var hit in hits)
            {
                Vector2 toHit = ((Vector2)hit.transform.position - (Vector2)transform.position).normalized;
                if (Vector2.Dot(forward, toHit) >= 0f) // 180° front hemisphere
                    hit.GetComponent<Health>()?.TakeDamage(blessedWhipDamage);
            }

            yield return new WaitForSeconds(0.4f);
        }

        // ─── Health Events ────────────────────────────────────────────────────

        private void OnHealthChanged(int current, int max)
        {
            bossHealthBar?.UpdateHP(current);

            if (!phaseTriggered && (float)current / max <= 0.5f)
            {
                phaseTriggered = true;
                inPhaseTransition = true;
                StartCoroutine(PhaseCollapse());
            }
        }

        private IEnumerator PhaseCollapse()
        {
            // Faz 1 placeholder: boss collapses at 50%, then dies.
            // Faz 2'de gerçek phase 2 geçişi buraya eklenir.
            rb.linearVelocity = Vector2.zero;

            Debug.Log("[PenitentSovereign] %50 HP — phase collapse. Faz 2'de devam eder.");

            yield return new WaitForSeconds(1.5f);

            // Instant kill remainder HP
            health.TakeDamage(9999);
        }

        private void OnDeath()
        {
            if (isDead) return;
            isDead = true;

            bossHealthBar?.Hide();

            StopAllCoroutines();
            rb.linearVelocity = Vector2.zero;

            RuntimeRoomManager.Instance?.NotifyBossDefeated();

            StartCoroutine(DeathDelay());
        }

        private IEnumerator DeathDelay()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.SetActive(false);
        }

        // ─── Helpers ──────────────────────────────────────────────────────────

        private void FacePlayer()
        {
            if (sr == null || player == null) return;
            sr.flipX = player.position.x < transform.position.x;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, surgeRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, blessedWhipRadius);
            Gizmos.color = Color.cyan;
            if (player != null)
            {
                Vector3 dir = (player.position - transform.position).normalized;
                Gizmos.DrawLine(transform.position, transform.position + dir * chainWhipRange);
            }
        }
    }
}

