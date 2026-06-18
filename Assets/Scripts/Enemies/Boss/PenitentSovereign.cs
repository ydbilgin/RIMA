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

        [Header("Demo Flow")]
        [SerializeField] private bool suppressClassSelectOnDeath = true;

        [Header("Colors (telegraph)")]
        [SerializeField] private Color telegraphColor   = new Color(1f, 0.4f, 0f, 1f);   // orange
        [SerializeField] private Color phase2Color      = new Color(0.7f, 0f, 1f, 1f);   // purple
        [SerializeField] private Color baseColor        = Color.white;

        // ─── Authored phase-break beats ───────────────────────────────────────
        // Emotional lines tied to the boss's existing phase transitions. EDIT HERE.
        // Source: STAGING/STORY_REVISION_S6.md §4 (the Sovereign tragedy) + NLM canon
        // (notebook 30ddffa5, tone = "Vivid Vulnerability": quiet grief, no performed villainy;
        // EN-first canonical, cyan-whisper styling). These are NARRATIVE only — they do NOT
        // touch combat (damage / thresholds / patterns); they fire AT the existing transitions.
        //
        //   · Fight-open whisper — canon R3 pre-fight line reused as the arena-entry whisper
        //       canon: "My chains loosen with your steps..." (TR: "Zincirlerim adımlarınla gevşiyor...")
        //   · 50% chains-break  — STORY_REVISION §4 beat line; canon TR "...artık yetmez." (...no longer enough.)
        //   · 33% Unleashed     — succumbing to the rift he held back; on-tone with canon death whisper "...finally empty."
        private const string BeatFightOpen   = "My chains loosen with your steps...";
        private const string BeatChainsBreak = "Discipline breaks before the chain does.";
        private const string BeatUnleashed   = "There is nothing left to hold.";

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

        // Phase 3 "Unleashed" overlay (33% HP) — modifier layer on the P2 roster, biased to aggression
        // (FractureStrike/Charge heavy, one ChainExplosion, no Wrath). See BOSS_MOB_DESIGN_S6 §1 P3.
        private readonly int[] p3Rotation = { 0, 3, 0, 3, 1 };
        private int p3Idx;
        private bool phase3Active;
        private bool phase3Done;

        // FIX (Phase-2 burst-skip): minimum real seconds the boss must spend in Phase 2 before the
        // 33% "Unleashed" (Phase 3) overlay can trigger. A single burst that drops HP from >50% to
        // <33% would otherwise skip Phase 2 mechanics entirely; this lock guarantees Phase 2 is shown.
        private const float Phase2MinDuration = 8f;
        private float phase2EnterTime = float.NegativeInfinity;

        // ─── Components ───────────────────────────────────────────────────────

        private Rigidbody2D  rb;
        private Health       health;
        private SpriteRenderer sr;
        private Transform    player;
        private BossHealthBar healthBar;

        // ─── Init ─────────────────────────────────────────────────────────────

        // ─── T6 Boss-A visual components ──────────────────────────────────────

        private SpriteRenderer rimSR;   // outline/rim layer
        private static readonly Vector3 BossScale = Vector3.one; // prefab sprite child already carries the 2x boss read

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
            else Debug.LogWarning("[PenitentSovereign] No GameObject tagged 'Player' at Start — will re-acquire each BossLoop frame (P0#2).");

            // Presentation scale is authored on the prefab child; avoid compounding it at runtime.
            transform.localScale = BossScale;

            // T6: Rim-light layer — crimson/stone palette, restrained so the boss does not read as neon cyan.
            rimSR = BuildRimLayer();

            healthBar = FindAnyObjectByType<BossHealthBar>();
            healthBar?.Initialize("THE PENITENT SOVEREIGN", bossMaxHP);
            // HP bar Show() → BossIntroController sekansında yapılır.

            health.OnHealthChanged.AddListener((cur, max) => healthBar?.UpdateHP(cur));

            // T6: 1.5s boss intro sekansı; tamamlanınca BossLoop başlar.
            BossIntroController.Begin(
                "THE PENITENT SOVEREIGN",
                transform,
                healthBar,
                () => StartCoroutine(BossLoop()));
        }

        /// <summary>Rim-light: sprites'in arkasına koyu crimson renkte küçük-ofset kopyası koyar.</summary>
        private SpriteRenderer BuildRimLayer()
        {
            if (sr == null) return null;

            var rimGO = new GameObject("BossRimLayer");
            rimGO.transform.SetParent(sr.transform.parent, false);
            rimGO.transform.localPosition  = sr.transform.localPosition;
            rimGO.transform.localScale     = Vector3.one * 1.04f; // %4 büyük

            var rim = rimGO.AddComponent<SpriteRenderer>();
            rim.sprite       = sr.sprite;
            rim.sortingLayerName = sr.sortingLayerName;
            rim.sortingOrder = sr.sortingOrder - 1;
            rim.color        = new Color(0.55f, 0.06f, 0.08f, 0.38f);
            rim.material     = sr.material;

            // Sprite değişimlerini izle (animasyon olursa).
            StartCoroutine(SyncRimSprite(rim));
            return rim;
        }

        private IEnumerator SyncRimSprite(SpriteRenderer rim)
        {
            while (rim != null && sr != null)
            {
                if (rim.sprite != sr.sprite) rim.sprite = sr.sprite;
                yield return new WaitForSeconds(0.05f);
            }
        }

        // ─── Core Loop ────────────────────────────────────────────────────────

        private IEnumerator BossLoop()
        {
            // Fight-open beat — fired during the existing intro calm, on top of the room-5 title card.
            // Pure narrative (RoomMonologController.Say no-ops if no controller / empty text); combat untouched.
            RoomMonologController.Say(BeatFightOpen);
            yield return new WaitForSeconds(1.0f);   // brief intro calm

            while (!dead)
            {
                // P0#2 (ChatGPT review 04 §2): re-acquire the player every loop if the reference is
                // lost (spawn-order race / player respawn), mirroring BaseMobBehavior.Update. Without
                // this the boss could resolve null once in Start() and stay permanently idle.
                if (player == null)
                {
                    var reacquired = GameObject.FindGameObjectWithTag("Player");
                    if (reacquired != null) player = reacquired.transform;
                    else { yield return null; continue; }
                }

                // Phase transition check — canon: chains break at 50% HP
                if (!phaseTransitionDone && health.CurrentHP <= Mathf.CeilToInt(health.MaxHP * 0.5f))
                {
                    phaseTransitionDone = true;
                    // FIX (Phase-2 burst-skip): stamp Phase-2 entry so the 33% overlay is locked out
                    // for Phase2MinDuration seconds even if a burst already pushed HP below 33%.
                    phase2EnterTime = Time.time;
                    yield return StartCoroutine(DoPhaseTransition());
                    continue;
                }

                // Phase 3 "Unleashed" overlay — the last chains shatter at 33% HP (modifier layer on Phase 2).
                // FIX (Phase-2 burst-skip): also require >= Phase2MinDuration s since Phase-2 entry so a
                // single burst can never skip Phase 2; the threshold is re-checked every loop until then.
                if (phaseTransitionDone && !phase3Done
                    && Time.time - phase2EnterTime >= Phase2MinDuration
                    && health.CurrentHP <= Mathf.CeilToInt(health.MaxHP * 0.33f))
                {
                    phase3Done = true;
                    phase3Active = true;
                    yield return StartCoroutine(DoPhase3Transition());
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

                // Cooldown (Phase 3 is more relentless — shorter gaps, not chaos)
                float cd = Random.Range(cooldownMin, cooldownMax);
                if (phase3Active) cd *= 0.8f;
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
            int attackId = phase3Active
                ? p3Rotation[p3Idx++ % p3Rotation.Length]
                : p2Rotation[p2Idx++ % p2Rotation.Length];

            float speed = phase1Speed * phase2SpeedMult * (phase3Active ? 1.15f : 1f);

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
            if (player == null) yield break;

            // CORRECTNESS SNAPSHOT (ChatGPT review 04 §5): lock origin + direction at telegraph
            // start; the post-windup damage cast reuses the SAME snapshot so the whip lands exactly
            // where the line telegraph was drawn (no recalc drift if the player strafes during windup).
            Vector2 snapOrigin = transform.position;
            Vector2 dir = ((Vector2)player.position - snapOrigin).normalized;

            // T6: line telegraph (Art/Telegraphs/telegraph_line_beam.png → EnemyTelegraph LineRenderer).
            EnemyTelegraph.SpawnLine(snapOrigin, dir, chainWhipLength, chainWhipWidth, telegraphDuration);

            yield return StartCoroutine(Telegraph(telegraphDuration));
            if (dead) yield break;

            Vector2 origin = snapOrigin + dir * 0.5f;

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

            EnemyTelegraph.FlashImpact(snapOrigin + dir * (chainWhipLength * 0.5f));  // snap along the whip (major event)
            Debug.DrawLine(snapOrigin, snapOrigin + dir * chainWhipLength, Color.red, 0.5f);
        }

        /// Penitent Surge — 4m çevreye AoE itme + hasar
        private IEnumerator Attack_PenitentSurge()
        {
            rb.linearVelocity = Vector2.zero;

            // T6: circle telegraph (EnemyTelegraph SpawnCircle).
            EnemyTelegraph.SpawnCircle(transform.position, surgeRadius, telegraphDuration + 0.1f);

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
            EnemyTelegraph.FlashImpact(transform.position);  // snap at surge centre
            RIMA.Combat.ScreenShakeDriver.Instance?.Shake(0.40f, 0.40f);

            Debug.DrawRay(transform.position, Vector2.up * surgeRadius, Color.yellow, 0.5f);
        }

        /// Shackle Throw — hedefe doğru zincir fırlatma (2s slow)
        private IEnumerator Attack_ShackleThrow()
        {
            rb.linearVelocity = Vector2.zero;

            // P1 teaching cue: short origin cast-flash (projectile itself stays "unannounced" by convention).
            SkillVfx.CastFlash(gameObject, VfxElement.Physical);

            yield return StartCoroutine(Telegraph(telegraphDuration - 0.15f));
            if (dead || player == null) yield break;

            Vector2 dir = ((Vector2)player.position - (Vector2)transform.position).normalized;
            SpawnChainProjectile(transform.position, dir, shackleThrowDmg, shackleThrowSpeed);
        }

        /// Holy Lash — 180° yay hasar (boss'un önü)
        private IEnumerator Attack_HolyLash()
        {
            rb.linearVelocity = Vector2.zero;
            if (player == null) yield break;

            // CORRECTNESS SNAPSHOT (ChatGPT review 04 §5): lock the arc origin + facing at telegraph
            // start; damage reuses the SAME forward so the 180° hit arc matches the drawn cone.
            Vector2 snapOrigin = transform.position;
            Vector2 forward = ((Vector2)player.position - snapOrigin).normalized;

            // Ground telegraph: 180° cone toward the player, matched to the real windup.
            EnemyTelegraph.SpawnCone(snapOrigin, forward, holyLashRadius, 180f, WindupSeconds(telegraphDuration));

            yield return StartCoroutine(Telegraph(telegraphDuration));
            if (dead) yield break;

            var hits = Physics2D.OverlapCircleAll(snapOrigin, holyLashRadius);

            foreach (var h in hits)
            {
                if (!h.CompareTag("Player")) continue;
                Vector2 toTarget = ((Vector2)h.transform.position - snapOrigin).normalized;
                float angle = Vector2.Angle(forward, toTarget);
                if (angle <= 90f)  // 180° arc = ±90° from forward
                {
                    h.GetComponent<Health>()?.TakeDamage(holyLashDmg);
                    h.GetComponent<KnockbackComponent>()?.ApplyKnockbackFrom(transform.position, 7f);
                }
            }

            // Snap-to-damage feedback at the arc origin (major event).
            EnemyTelegraph.FlashImpact(snapOrigin + forward * (holyLashRadius * 0.5f));
        }

        // ─── Phase 2 Attacks ──────────────────────────────────────────────────

        /// Fracture Strike — 3 hızlı ardışık vuruş
        private IEnumerator Attack_FractureStrike()
        {
            rb.linearVelocity = Vector2.zero;

            // CORRECTNESS SNAPSHOT (ChatGPT review 04 §5): lock the strike origin at telegraph start;
            // all 3 sub-strikes hit the SAME circle the telegraph drew (boss is rooted here anyway).
            Vector2 snapOrigin = transform.position;

            // Ground telegraph: melee strike zone, matched to the 0.3s windup.
            EnemyTelegraph.SpawnCircle(snapOrigin, meleeStopRange + 0.4f, WindupSeconds(0.3f));

            yield return StartCoroutine(Telegraph(0.3f));
            if (dead) yield break;

            for (int i = 0; i < 3; i++)
            {
                var hits = Physics2D.OverlapCircleAll(snapOrigin, meleeStopRange + 0.4f);
                foreach (var h in hits)
                {
                    if (!h.CompareTag("Player")) continue;
                    h.GetComponent<Health>()?.TakeDamage(fractureStrikeDmg);

                    if (i == 2) // final hit: knockback
                        h.GetComponent<KnockbackComponent>()?.ApplyKnockbackFrom(snapOrigin, 8f);
                }

                // Per-strike feedback is the cheap color flash. FlashImpact is RESERVED for the major
                // event (ChatGPT review 04 §5 VFX rule) — fire the strong snap only on the final strike,
                // not 3× per combo, so small hits do not over-fire the impact motor.
                FlashColor(phase2Color, 0.08f);
                if (i == 2)
                    EnemyTelegraph.FlashImpact(snapOrigin, VfxElement.Void);  // major snap: combo finisher only
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
            // Marker persists, then explodes. Phase 3 shortens the dodge window too (cx review Q3 — this
            // windup bypasses Telegraph()), floored at the reaction window so it never becomes unfair.
            float delay = phase3Active ? Mathf.Max(0.22f, chainExplosionDelay * 0.85f) : chainExplosionDelay;

            // Ground telegraph: delayed-explosion ring whose lifetime == the real blast delay
            // (ARPG-AoE convention — "this WILL detonate here in `delay` seconds").
            EnemyTelegraph.SpawnDelayedRing(worldPos, chainExplosionRadius, delay);

            float elapsed = 0f;
            while (elapsed < delay)
            {
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

            EnemyTelegraph.FlashImpact(worldPos, VfxElement.Void);  // snap at the blast
            RIMA.Combat.ScreenShakeDriver.Instance?.Shake(0.25f, 0.30f);
        }

        /// Sovereign's Wrath — tüm alan hasar, merkez güvenli bölge
        private IEnumerator Attack_SovereignsWrath()
        {
            rb.linearVelocity = Vector2.zero;

            // CORRECTNESS SNAPSHOT (ChatGPT review 04 §5): lock the AoE origin at telegraph start;
            // damage below uses the SAME snapshot so the warning ring == the hit ring.
            Vector2 wrathOrigin = transform.position;

            // Ground telegraph: outer DANGER ring (red) + inner SAFE-ZONE ring (distinct green).
            // Two identical red rings did not communicate "stand in the centre" — the safe ring now
            // reads apart by hue (red = leave, green = safe) so the player can parse where to dodge.
            float wrathWindup = WindupSeconds(telegraphDuration + 0.7f);
            EnemyTelegraph.SpawnCircle(wrathOrigin, wrathOuterRadius, wrathWindup);                           // danger (default red)
            EnemyTelegraph.SpawnCircle(wrathOrigin, wrathSafeRadius, wrathWindup, EnemyTelegraph.SafeZoneColor); // safe centre (green)

            // Longer telegraph — player needs time to find safe zone
            yield return StartCoroutine(Telegraph(telegraphDuration + 0.7f));
            if (dead) yield break;

            var hits = Physics2D.OverlapCircleAll(wrathOrigin, wrathOuterRadius);
            foreach (var h in hits)
            {
                if (!h.CompareTag("Player")) continue;
                float dist = Vector2.Distance(wrathOrigin, h.transform.position);
                if (dist <= wrathSafeRadius) continue;  // güvenli bölge

                h.GetComponent<Health>()?.TakeDamage(sovereignWrathDmg);
            }

            EnemyTelegraph.FlashImpact(wrathOrigin, VfxElement.Void);  // big snap at the boss centre (major event)
            RIMA.Combat.ScreenShakeDriver.Instance?.Shake(0.50f, 0.42f);
        }

        /// Fracture Charge — arena boyunca hızlı dash + hasar çizgisi
        private IEnumerator Attack_FractureCharge()
        {
            rb.linearVelocity = Vector2.zero;
            if (player == null) yield break;

            // CORRECTNESS SNAPSHOT (ChatGPT review 04 §5): lock the dash start + direction at telegraph
            // start; the dash below reuses the SAME start/dir so the charge follows the drawn lane exactly
            // (no recalc — telegraph and travel path stay identical even if the player moves during windup).
            Vector2 startPos = transform.position;
            Vector2 dir      = ((Vector2)player.position - startPos).normalized;
            Vector2 endPos   = startPos + dir * 16f;   // arena-wide

            // Ground telegraph: pre-draw the dash lane toward the player, matched to the windup.
            // Width = the dash hit radius (meleeStopRange + 0.3f) doubled, so the lane reads at true scale.
            EnemyTelegraph.SpawnLine(startPos, dir, 16f, (meleeStopRange + 0.3f) * 2f,
                                     WindupSeconds(telegraphDuration - 0.1f));

            yield return StartCoroutine(Telegraph(telegraphDuration - 0.1f));
            if (dead) yield break;

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
            EnemyTelegraph.FlashImpact(endPos, VfxElement.Void);  // snap at the dash terminus
            RIMA.Combat.ScreenShakeDriver.Instance?.Shake(0.35f, 0.35f);
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
            RoomMonologController.Say(BeatChainsBreak);   // authored 50% chains-break beat (null-guards itself)
            RIMA.Combat.ScreenShakeDriver.Instance?.Shake(0.45f, 0.40f);
            RIMA.Combat.HitPauseDriver.Instance?.TriggerPause(0.1f); // chains-snap freeze (design §1, 50% beat)

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

        // Phase 3 "Unleashed" — the seal fails: shorter ceremony, body floods cyan-veined, last chains shatter.
        private IEnumerator DoPhase3Transition()
        {
            rb.linearVelocity = Vector2.zero;

            float duration = Mathf.Max(0.6f, phaseTransitionTime - 0.5f); // ~1.0s — past ceremony
            float half = duration * 0.5f;

            // First half: white flash (discipline finally fails)
            float elapsed = 0f;
            while (elapsed < half)
            {
                float t = elapsed / half;
                if (sr != null) sr.color = Color.Lerp(baseColor, Color.white, Mathf.Sin(t * Mathf.PI * 5f) * 0.5f + 0.5f);
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Shatter moment: seal-energy floods the body cyan-veined (the boss-only cyan exception); monolog + heavy shake + freeze.
            baseColor = Color.Lerp(baseColor, new Color(0f, 1f, 0.8f, 1f), 0.4f);
            if (sr != null) sr.color = Color.white;
            RoomMonologController.Say(BeatUnleashed);     // authored 33% Unleashed beat (null-guards itself)
            RIMA.Combat.ScreenShakeDriver.Instance?.Shake(0.55f, 0.45f);
            RIMA.Combat.HitPauseDriver.Instance?.TriggerPause(0.1f);

            // Second half: settle into the new cyan-veined rest color
            elapsed = 0f;
            while (elapsed < half)
            {
                if (sr != null) sr.color = Color.Lerp(Color.white, baseColor, elapsed / half);
                elapsed += Time.deltaTime;
                yield return null;
            }
            if (sr != null) sr.color = baseColor;

            Debug.Log("[PenitentSovereign] ⚡⚡ PHASE 3 UNLEASHED");
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
            // T6: Boss-kill slow-mo payoff — mevcut HitPauseDriver owner-guard'lı mekanizması (0.20s boss tier).
            RIMA.Combat.HitPauseDriver.Instance?.TriggerPause(0.20f);

            // Floor crack effect + screen shake
            RIMA.Combat.ScreenShakeDriver.Instance?.Shake(0.55f, 0.45f);

            // T6: Seal fragments dağılma (ritual circle pulse + 4 fragment sprite).
            SpawnSealFragments();

            // Dramatic fade
            float elapsed = 0f;
            float fadeDuration = 2f;
            Color start = sr != null ? sr.color : Color.white;

            while (elapsed < fadeDuration)
            {
                float t = elapsed / fadeDuration;
                if (sr != null)
                    sr.color = Color.Lerp(start, new Color(start.r, start.g, start.b, 0f), t);
                if (rimSR != null)
                {
                    Color rc = rimSR.color;
                    rimSR.color = new Color(rc.r, rc.g, rc.b, Mathf.Lerp(0.55f, 0f, t));
                }
                elapsed += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(0.3f);

            if (!suppressClassSelectOnDeath)
            {
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
            }

            // Notify room manager
            RuntimeRoomManager.Instance?.NotifyBossDefeated();

            Destroy(gameObject, 0.5f);
        }

        // ─── Seal Fragments (T6 boss-kill payoff) ────────────────────────────

        /// <summary>
        /// Boss öldüğünde 4 fragment parçası dağılır.
        /// Sprite olarak Art/Boss/boss_intro_seal_ring.png kullanılır; asset yoksa renk kutusu.
        /// </summary>
        private void SpawnSealFragments()
        {
            Sprite sealSpr = Resources.Load<Sprite>("Boss/boss_intro_seal_ring");
            int count = 4;
            for (int i = 0; i < count; i++)
            {
                float angle = i * (360f / count) + Random.Range(-20f, 20f);
                Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                // MAJOR-3 fix: each fragment hosts its own coroutine via SealFragmentDriver,
                // so it is independent of this (host) MonoBehaviour's lifetime.
                SealFragmentDriver.Spawn(sealSpr, (Vector2)transform.position, dir);
            }
        }

        // ─── SealFragmentDriver — inner helper class ──────────────────────────
        // MAJOR-3: Runs FragmentFly on its OWN GO so Destroy(boss, 0.5f) cannot
        // kill the coroutine before the fragment's lifetime (up to 0.9s) completes.

        private class SealFragmentDriver : MonoBehaviour
        {
            public static void Spawn(Sprite spr, Vector2 origin, Vector2 dir)
            {
                var go = new GameObject("SealFragment");
                go.transform.position = origin;

                var fragSR = go.AddComponent<SpriteRenderer>();
                fragSR.sprite = spr;
                fragSR.sortingLayerName = "Entities";
                fragSR.sortingOrder = 10;
                go.transform.localScale = Vector3.one * 0.35f;

                if (spr == null)
                {
                    // Fragment sprite yok → küçük cyan kare.
                    Texture2D tex = new Texture2D(8, 8);
                    Color c = new Color(0.28f, 0.88f, 1f);
                    for (int y = 0; y < 8; y++) for (int x = 0; x < 8; x++) tex.SetPixel(x, y, c);
                    tex.Apply(); tex.filterMode = FilterMode.Point;
                    fragSR.sprite = Sprite.Create(tex, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.5f), 16f);
                    go.transform.localScale = Vector3.one * 0.5f;
                }

                var driver = go.AddComponent<SealFragmentDriver>();
                driver.StartCoroutine(driver.Fly(fragSR, dir));
            }

            private IEnumerator Fly(SpriteRenderer fragSR, Vector2 dir)
            {
                float speed = Random.Range(3f, 6f);
                float lifetime = Random.Range(0.5f, 0.9f);
                float elapsed = 0f;
                Color startColor = fragSR.color;

                while (elapsed < lifetime)
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / lifetime;
                    transform.position += (Vector3)(dir * speed * Time.deltaTime);
                    speed = Mathf.Lerp(speed, 0f, Time.deltaTime * 4f);
                    startColor.a = Mathf.Lerp(1f, 0f, t);
                    fragSR.color = startColor;
                    yield return null;
                }

                Destroy(gameObject);
            }
        }

        // ─── Telegraph Helper ─────────────────────────────────────────────────

        /// <summary>
        /// Mirrors the EXACT Phase-3 scaling that Telegraph() applies to a windup, so the
        /// ground telegraph (EnemyTelegraph) duration stays in lock-step with the boss
        /// color-pulse windup. Keep identical to the line in Telegraph() below.
        /// </summary>
        private float WindupSeconds(float baseDuration)
        {
            return phase3Active ? Mathf.Max(0.22f, baseDuration * 0.85f) : baseDuration;
        }

        private IEnumerator Telegraph(float duration)
        {
            // Phase 3 "Unleashed": telegraphs shorten ~15%, floored at the player's reaction window (never below 0.22s).
            if (phase3Active) duration = Mathf.Max(0.22f, duration * 0.85f);

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
