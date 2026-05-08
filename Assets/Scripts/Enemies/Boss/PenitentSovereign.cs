using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Act 1 Boss — The Penitent Sovereign
    ///
    /// Faz 1 (100% → 50%): Ağır, öngörülebilir saldırılar.
    ///   · Chain Whip    — 6m düz hasar çizgisi
    ///   · Penitent Surge — 4m AoE itme + hasar
    ///   · Shackle Throw  — uzak mesafe zincir fırlatma (slow)
    ///   · Holy Lash      — 180° yay hasar
    ///
    /// Faz 2 (50% → 0%): Zincirler kırıldı. Hız +40%, yeni saldırılar.
    ///   · Fracture Strike  — 3 hızlı ardışık vuruş
    ///   · Chain Explosion  — zemin işaretleri, 3 sn gecikmeli patlama
    ///   · Sovereign's Wrath — tüm alan hasar, merkez güvenli bölge
    ///   · Fracture Charge  — arena boyunca dash + hasar çizgisi
    ///
    /// Ölüm → ClassSelectionTrigger.Trigger() → secondary class seçimi başlar.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Health))]
    public class PenitentSovereign : MonoBehaviour
    {
        // ─── Config ───────────────────────────────────────────────────────────

        [Header("Stats")]
        [SerializeField] private int   bossMaxHP    = 800;
        [SerializeField] private float phase1Speed  = 2.5f;
        [SerializeField] private float phase2SpeedMult = 1.4f;

        [Header("Timing")]
        [SerializeField] private float telegraphDuration   = 0.75f;
        [SerializeField] private float cooldownMin         = 1.2f;
        [SerializeField] private float cooldownMax         = 2.0f;
        [SerializeField] private float phaseTransitionTime = 1.5f;

        [Header("Phase 1 Damage")]
        [SerializeField] private int chainWhipDmg     = 20;
        [SerializeField] private int penitentSurgeDmg = 15;
        [SerializeField] private int shackleThrowDmg  = 12;
        [SerializeField] private int holyLashDmg      = 18;

        [Header("Phase 2 Damage")]
        [SerializeField] private int fractureStrikeDmg    = 16;
        [SerializeField] private int chainExplosionDmg    = 25;
        [SerializeField] private int sovereignWrathDmg    = 30;
        [SerializeField] private int fractureChargeDmg    = 22;

        [Header("Ranges")]
        [SerializeField] private float detectionRange    = 14f;
        [SerializeField] private float meleeStopRange    = 1.6f;
        [SerializeField] private float chainWhipLength   = 6f;
        [SerializeField] private float chainWhipWidth    = 1.2f;
        [SerializeField] private float surgeRadius       = 4f;
        [SerializeField] private float holyLashRadius    = 2.5f;
        [SerializeField] private float shackleThrowSpeed = 7f;
        [SerializeField] private float chargeSpeed       = 18f;

        [Header("Chain Explosion")]
        [SerializeField] private int   chainMarkerCount   = 3;
        [SerializeField] private float chainExplosionDelay = 3f;
        [SerializeField] private float chainExplosionRadius = 1.2f;

        [Header("Sovereign's Wrath")]
        [SerializeField] private float wrathSafeRadius  = 2.5f;
        [SerializeField] private float wrathOuterRadius = 12f;

        [Header("Projectile")]
        [SerializeField] private GameObject chainProjectilePrefab;  // optional; runtime fallback if null

        [Header("Colors (telegraph)")]
        [SerializeField] private Color telegraphColor   = new Color(1f, 0.4f, 0f, 1f);   // orange
        [SerializeField] private Color phase2Color      = new Color(0.7f, 0f, 1f, 1f);   // purple
        [SerializeField] private Color baseColor        = Color.white;

        // ─── State ────────────────────────────────────────────────────────────

        private enum BossPhase { Phase1, Phase2 }
        private BossPhase phase = BossPhase.Phase1;
        private bool phaseTransitionDone;
        private bool dead;

        // Phase 1 attack rotation: 0=ChainWhip, 1=Surge, 2=ShackleThrow, 3=HolyLash
        private readonly int[] p1Rotation = { 0, 2, 1, 3 };
        private int p1Idx;

        // Phase 2 attack rotation: 0=FractureStrike, 1=ChainExplosion, 2=Wrath, 3=FractureCharge
        private readonly int[] p2Rotation = { 0, 3, 1, 2 };
        private int p2Idx;

        // ─── Components ───────────────────────────────────────────────────────

        private Rigidbody2D  rb;
        private Health       health;
        private SpriteRenderer sr;
        private Transform    player;
        private BossHealthBar healthBar;

        // ─── Init ─────────────────────────────────────────────────────────────

        private void Awake()
        {
            rb     = GetComponent<Rigidbody2D>();
            health = GetComponent<Health>();
            sr     = GetComponentInChildren<SpriteRenderer>();

            rb.gravityScale  = 0f;
            rb.freezeRotation = true;

            health.SetMaxHP(bossMaxHP);
            health.OnDeath.AddListener(HandleDeath);
        }

        private void Start()
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;

            healthBar = FindAnyObjectByType<BossHealthBar>();
            healthBar?.Initialize("The Penitent Sovereign", bossMaxHP);
            healthBar?.Show();

            health.OnHealthChanged.AddListener((cur, max) => healthBar?.UpdateHP(cur));

            StartCoroutine(BossLoop());
        }

        // ─── Core Loop ────────────────────────────────────────────────────────

        private IEnumerator BossLoop()
        {
            yield return new WaitForSeconds(1.0f);   // brief intro calm

            while (!dead)
            {
                if (player == null) { yield return null; continue; }

                // Phase transition check
                if (!phaseTransitionDone && health.CurrentHP <= health.MaxHP / 2)
                {
                    phaseTransitionDone = true;
                    yield return StartCoroutine(DoPhaseTransition());
                    continue;
                }

                float dist = Vector2.Distance(transform.position, player.position);

                if (dist > detectionRange)
                {
                    rb.linearVelocity = Vector2.zero;
                    yield return null;
                    continue;
                }

                // Choose attack based on phase
                if (phase == BossPhase.Phase1)
                    yield return StartCoroutine(Phase1Turn(dist));
                else
                    yield return StartCoroutine(Phase2Turn());

                // Cooldown
                float cd = Random.Range(cooldownMin, cooldownMax);
                yield return new WaitForSeconds(cd);
            }
        }

        // ─── Phase 1 Turn ─────────────────────────────────────────────────────

        private IEnumerator Phase1Turn(float dist)
        {
            int attackId = p1Rotation[p1Idx % p1Rotation.Length];
            p1Idx++;

            // Shackle Throw works at range; others need melee
            bool needsMelee = attackId != 2;

            if (needsMelee && dist > meleeStopRange)
                yield return StartCoroutine(ApproachUntilRange(meleeStopRange, phase1Speed));

            if (dead) yield break;

            switch (attackId)
            {
                case 0: yield return StartCoroutine(Attack_ChainWhip());    break;
                case 1: yield return StartCoroutine(Attack_PenitentSurge()); break;
                case 2: yield return StartCoroutine(Attack_ShackleThrow()); break;
                case 3: yield return StartCoroutine(Attack_HolyLash());     break;
            }
        }

        // ─── Phase 2 Turn ─────────────────────────────────────────────────────

        private IEnumerator Phase2Turn()
        {
            int attackId = p2Rotation[p2Idx % p2Rotation.Length];
            p2Idx++;

            float speed = phase1Speed * phase2SpeedMult;

            // FractureStrike needs melee; others work at any range
            bool needsMelee = attackId == 0;
            if (needsMelee)
            {
                float dist = Vector2.Distance(transform.position, player.position);
                if (dist > meleeStopRange)
                    yield return StartCoroutine(ApproachUntilRange(meleeStopRange, speed));
            }

            if (dead) yield break;

            switch (attackId)
            {
                case 0: yield return StartCoroutine(Attack_FractureStrike());    break;
                case 1: yield return StartCoroutine(Attack_ChainExplosion());    break;
                case 2: yield return StartCoroutine(Attack_SovereignsWrath());   break;
                case 3: yield return StartCoroutine(Attack_FractureCharge());    break;
            }
        }

        // ─── Movement Helper ──────────────────────────────────────────────────

        private IEnumerator ApproachUntilRange(float stopRange, float speed)
        {
            float timeout = 6f;
            float elapsed = 0f;

            while (!dead && elapsed < timeout)
            {
                if (player == null) break;
                float dist = Vector2.Distance(transform.position, player.position);
                if (dist <= stopRange) break;

                Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
                rb.linearVelocity = dir * speed;
                elapsed += Time.deltaTime;
                yield return null;
            }
            rb.linearVelocity = Vector2.zero;
        }

        // ─── Phase 1 Attacks ──────────────────────────────────────────────────

        /// Chain Whip — 6m düz çizgi hasar (boss'un yönünde)
        private IEnumerator Attack_ChainWhip()
        {
            rb.linearVelocity = Vector2.zero;
            yield return StartCoroutine(Telegraph(telegraphDuration));
            if (dead || player == null) yield break;

            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            Vector2 origin = (Vector2)transform.position + dir * 0.5f;

            // Box cast along the direction
            var hits = Physics2D.BoxCastAll(
                origin,
                new Vector2(chainWhipWidth, chainWhipWidth),
                Vector2.SignedAngle(Vector2.right, dir),
                dir,
                chainWhipLength,
                LayerMask.GetMask("Player")
            );

            foreach (var h in hits)
            {
                if (h.collider.CompareTag("Player"))
                {
                    h.collider.GetComponent<Health>()?.TakeDamage(chainWhipDmg);
                    h.collider.GetComponent<KnockbackComponent>()?.ApplyKnockbackFrom(transform.position, 6f);
                }
            }

            // Fallback: simple OverlapBox if BoxCast misses (no layer setup)
            if (hits.Length == 0)
            {
                Vector2 boxCenter = origin + dir * (chainWhipLength * 0.5f);
                var overlap = Physics2D.OverlapBoxAll(boxCenter,
                    new Vector2(chainWhipLength, chainWhipWidth),
                    Vector2.SignedAngle(Vector2.right, dir));
                foreach (var col in overlap)
                {
                    if (col.CompareTag("Player"))
                    {
                        col.GetComponent<Health>()?.TakeDamage(chainWhipDmg);
                        col.GetComponent<KnockbackComponent>()?.ApplyKnockbackFrom(transform.position, 6f);
                    }
                }
            }

            Debug.DrawLine(transform.position, (Vector2)transform.position + dir * chainWhipLength, Color.red, 0.5f);
        }

        /// Penitent Surge — 4m çevreye AoE itme + hasar
        private IEnumerator Attack_PenitentSurge()
        {
            rb.linearVelocity = Vector2.zero;
            yield return StartCoroutine(Telegraph(telegraphDuration + 0.1f));
            if (dead) yield break;

            var hits = Physics2D.OverlapCircleAll(transform.position, surgeRadius);
            foreach (var h in hits)
            {
                if (!h.CompareTag("Player")) continue;
                h.GetComponent<Health>()?.TakeDamage(penitentSurgeDmg);
                h.GetComponent<KnockbackComponent>()?.ApplyKnockbackFrom(transform.position, 10f);
            }

            // Shake camera
            ScreenShake.Instance?.AddTrauma(0.5f);

            Debug.DrawRay(transform.position, Vector2.up * surgeRadius, Color.yellow, 0.5f);
        }

        /// Shackle Throw — hedefe doğru zincir fırlatma (2s slow)
        private IEnumerator Attack_ShackleThrow()
        {
            rb.linearVelocity = Vector2.zero;
            yield return StartCoroutine(Telegraph(telegraphDuration - 0.15f));
            if (dead || player == null) yield break;

            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            SpawnChainProjectile(transform.position, dir, shackleThrowDmg, shackleThrowSpeed);
        }

        /// Holy Lash — 180° yay hasar (boss'un önü)
        private IEnumerator Attack_HolyLash()
        {
            rb.linearVelocity = Vector2.zero;
            yield return StartCoroutine(Telegraph(telegraphDuration));
            if (dead || player == null) yield break;

            Vector2 forward = ((Vector2)player.position - (Vector2)transform.position).normalized;
            var hits = Physics2D.OverlapCircleAll(transform.position, holyLashRadius);

            foreach (var h in hits)
            {
                if (!h.CompareTag("Player")) continue;
                Vector2 toTarget = ((Vector2)h.transform.position - (Vector2)transform.position).normalized;
                float angle = Vector2.Angle(forward, toTarget);
                if (angle <= 90f)  // 180° arc = ±90° from forward
                {
                    h.GetComponent<Health>()?.TakeDamage(holyLashDmg);
                    h.GetComponent<KnockbackComponent>()?.ApplyKnockbackFrom(transform.position, 7f);
                }
            }
        }

        // ─── Phase 2 Attacks ──────────────────────────────────────────────────

        /// Fracture Strike — 3 hızlı ardışık vuruş
        private IEnumerator Attack_FractureStrike()
        {
            rb.linearVelocity = Vector2.zero;
            yield return StartCoroutine(Telegraph(0.3f));
            if (dead) yield break;

            for (int i = 0; i < 3; i++)
            {
                var hits = Physics2D.OverlapCircleAll(transform.position, meleeStopRange + 0.4f);
                foreach (var h in hits)
                {
                    if (!h.CompareTag("Player")) continue;
                    h.GetComponent<Health>()?.TakeDamage(fractureStrikeDmg);

                    if (i == 2) // final hit: knockback
                        h.GetComponent<KnockbackComponent>()?.ApplyKnockbackFrom(transform.position, 8f);
                }

                FlashColor(phase2Color, 0.08f);
                yield return new WaitForSeconds(0.18f);
            }
        }

        /// Chain Explosion — zemin işaretleri, 3 sn gecikmeli patlama
        private IEnumerator Attack_ChainExplosion()
        {
            rb.linearVelocity = Vector2.zero;
            yield return StartCoroutine(Telegraph(0.5f));
            if (dead || player == null) yield break;

            // Place markers near player + random offsets
            for (int i = 0; i < chainMarkerCount; i++)
            {
                Vector2 offset = (i == 0)
                    ? (Vector2)player.position
                    : (Vector2)player.position + Random.insideUnitCircle * 3f;
                StartCoroutine(ChainMarkerSequence(offset));
            }
        }

        private IEnumerator ChainMarkerSequence(Vector2 worldPos)
        {
            // Visual marker (debug)
            float elapsed = 0f;
            while (elapsed < chainExplosionDelay)
            {
                Debug.DrawRay(worldPos, Vector2.up * 0.5f, Color.magenta);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Explode
            var hits = Physics2D.OverlapCircleAll(worldPos, chainExplosionRadius);
            foreach (var h in hits)
            {
                if (!h.CompareTag("Player")) continue;
                h.GetComponent<Health>()?.TakeDamage(chainExplosionDmg);
                h.GetComponent<KnockbackComponent>()?.ApplyKnockbackFrom(worldPos, 9f);
            }

            ScreenShake.Instance?.AddTrauma(0.35f);
        }

        /// Sovereign's Wrath — tüm alan hasar, merkez güvenli bölge
        private IEnumerator Attack_SovereignsWrath()
        {
            rb.linearVelocity = Vector2.zero;
            // Longer telegraph — player needs time to find safe zone
            yield return StartCoroutine(Telegraph(telegraphDuration + 0.7f));
            if (dead) yield break;

            var hits = Physics2D.OverlapCircleAll(transform.position, wrathOuterRadius);
            foreach (var h in hits)
            {
                if (!h.CompareTag("Player")) continue;
                float dist = Vector2.Distance(transform.position, h.transform.position);
                if (dist <= wrathSafeRadius) continue;  // güvenli bölge

                h.GetComponent<Health>()?.TakeDamage(sovereignWrathDmg);
            }

            ScreenShake.Instance?.AddTrauma(0.8f);
        }

        /// Fracture Charge — arena boyunca hızlı dash + hasar çizgisi
        private IEnumerator Attack_FractureCharge()
        {
            rb.linearVelocity = Vector2.zero;
            if (player == null) yield break;

            yield return StartCoroutine(Telegraph(telegraphDuration - 0.1f));
            if (dead) yield break;

            Vector2 startPos = transform.position;
            Vector2 dir      = ((Vector2)player.position - startPos).normalized;
            Vector2 endPos   = startPos + dir * 16f;   // arena-wide

            // Dash
            float elapsed = 0f;
            float duration = 0.35f;
            bool hitPlayer = false;

            while (elapsed < duration && !dead)
            {
                float t = elapsed / duration;
                Vector2 pos = Vector2.Lerp(startPos, endPos, t);

                // Check player hit during dash
                if (!hitPlayer)
                {
                    var hits = Physics2D.OverlapCircleAll(pos, meleeStopRange + 0.3f);
                    foreach (var h in hits)
                    {
                        if (!h.CompareTag("Player")) continue;
                        h.GetComponent<Health>()?.TakeDamage(fractureChargeDmg);
                        h.GetComponent<KnockbackComponent>()?.ApplyKnockbackFrom(pos, 12f);
                        hitPlayer = true;
                    }
                }

                rb.MovePosition(pos);
                elapsed += Time.deltaTime;
                yield return null;
            }

            rb.linearVelocity = Vector2.zero;
            ScreenShake.Instance?.AddTrauma(0.4f);
        }

        // ─── Phase Transition ─────────────────────────────────────────────────

        private IEnumerator DoPhaseTransition()
        {
            rb.linearVelocity = Vector2.zero;

            // Flash and collapse animation
            float elapsed = 0f;
            float half = phaseTransitionTime * 0.5f;

            // First half: bright flash
            while (elapsed < half)
            {
                float t = elapsed / half;
                if (sr != null)
                    sr.color = Color.Lerp(baseColor, Color.white, Mathf.Sin(t * Mathf.PI * 4f) * 0.5f + 0.5f);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Transition moment: purple burst
            if (sr != null) sr.color = phase2Color;
            ScreenShake.Instance?.AddTrauma(0.7f);

            // Second half: settle into Phase 2 color
            elapsed = 0f;
            while (elapsed < half)
            {
                float t = elapsed / half;
                if (sr != null)
                    sr.color = Color.Lerp(phase2Color, baseColor, t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            if (sr != null) sr.color = baseColor;

            phase = BossPhase.Phase2;
            Debug.Log("[PenitentSovereign] ⚡ PHASE 2 BAŞLADI");
        }

        // ─── Death ────────────────────────────────────────────────────────────

        private void HandleDeath()
        {
            if (dead) return;
            dead = true;

            StopAllCoroutines();
            rb.linearVelocity = Vector2.zero;

            healthBar?.Hide();

            var col = GetComponent<Collider2D>();
            if (col != null) col.enabled = false;

            StartCoroutine(DeathSequence());
        }

        private IEnumerator DeathSequence()
        {
            // Floor crack effect + screen shake
            ScreenShake.Instance?.AddTrauma(1f);

            // Dramatic fade
            float elapsed = 0f;
            float fadeDuration = 2f;
            Color start = sr != null ? sr.color : Color.white;

            while (elapsed < fadeDuration)
            {
                float t = elapsed / fadeDuration;
                if (sr != null)
                    sr.color = Color.Lerp(start, new Color(start.r, start.g, start.b, 0f), t);
                elapsed += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);

            // Trigger secondary class selection
            var trigger = FindAnyObjectByType<ClassSelectionTrigger>();
            if (trigger != null)
            {
                trigger.Trigger();
            }
            else
            {
                // Fallback: direct trigger
                if (PlayerClassManager.Instance != null)
                    PlayerClassManager.Instance.TriggerClassSelection();
                else
                    Debug.LogWarning("[PenitentSovereign] ClassSelectionTrigger bulunamadı!");
            }

            // Notify room manager
            LegacyRuntimeRoomManager.Instance?.NotifyBossDefeated();

            Destroy(gameObject, 0.5f);
        }

        // ─── Telegraph Helper ─────────────────────────────────────────────────

        private IEnumerator Telegraph(float duration)
        {
            if (sr == null)
            {
                yield return new WaitForSeconds(duration);
                yield break;
            }

            Color warnColor = (phase == BossPhase.Phase1) ? telegraphColor : phase2Color;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float t = elapsed / duration;
                // Pulse: lerp toward warning color and back
                float pulse = Mathf.Sin(t * Mathf.PI * 3f) * 0.5f + 0.5f;
                sr.color = Color.Lerp(baseColor, warnColor, pulse);
                elapsed += Time.deltaTime;
                yield return null;
            }

            sr.color = baseColor;
        }

        private void FlashColor(Color c, float duration)
        {
            if (sr != null)
                StartCoroutine(DoFlash(c, duration));
        }

        private IEnumerator DoFlash(Color c, float duration)
        {
            if (sr == null) yield break;
            sr.color = c;
            yield return new WaitForSeconds(duration);
            sr.color = baseColor;
        }

        // ─── Projectile Spawn ─────────────────────────────────────────────────

        private void SpawnChainProjectile(Vector2 from, Vector2 dir, int dmg, float speed)
        {
            if (chainProjectilePrefab != null)
            {
                var go = Instantiate(chainProjectilePrefab, from, Quaternion.identity);
                var cp = go.GetComponent<BossChainProjectile>();
                if (cp != null) cp.Init(dmg, speed, dir, 2f);
                return;
            }

            // Runtime fallback: create simple projectile GO
            var proj = new GameObject("ChainProjectile");
            proj.tag = "EnemyProjectile";
            proj.transform.position = from;
            proj.layer = LayerMask.NameToLayer("Default");

            var rb2d = proj.AddComponent<Rigidbody2D>();
            rb2d.gravityScale = 0f;
            rb2d.freezeRotation = true;

            var col = proj.AddComponent<CircleCollider2D>();
            col.isTrigger = true;
            col.radius = 0.2f;

            // Simple colored quad sprite
            var sr2 = proj.AddComponent<SpriteRenderer>();
            var tex = new Texture2D(8, 8);
            Color chainColor = new Color(0.8f, 0.4f, 0f);
            var pixels = new Color[64];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = chainColor;
            tex.SetPixels(pixels); tex.Apply(); tex.filterMode = FilterMode.Point;
            sr2.sprite = Sprite.Create(tex, new Rect(0,0,8,8), new Vector2(0.5f,0.5f), 16f);
            proj.transform.localScale = Vector3.one * 0.5f;

            var cp2 = proj.AddComponent<BossChainProjectile>();
            cp2.Init(dmg, speed, dir, 2f);
        }

        // ─── Gizmos ───────────────────────────────────────────────────────────

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, meleeStopRange);
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
            Gizmos.DrawWireSphere(transform.position, surgeRadius);
            Gizmos.color = new Color(0.5f, 0f, 1f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, wrathSafeRadius);
        }
    }
}
