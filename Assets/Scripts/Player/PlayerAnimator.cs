using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Tüm animator parametrelerini tek yerden yönetir.
    ///
    /// 8-way visual-direction system (S/SE/E/NE/N/NW/W/SW).
    /// Movement / facing input is snapped to the nearest of 8 directions and the
    /// last snapped direction is preserved when movement stops. The emitted DirX/DirY
    /// pair (each in {-1,0,1}) matches the Warblade controller's AnyState thresholds:
    /// cardinals zero one axis; diagonals set both to ±1.
    ///
    /// Animator'da gereken parametreler:
    ///   float  Speed       — 0=idle, 1=run
    ///   float  DirX        — hareket yönü X (-1..1)
    ///   float  DirY        — hareket yönü Y (-1..1)
    ///   bool   IsDashing   — dash animasyonu
    ///   trigger IsDead     — ölüm (geri dönmez)
    ///   int    ComboStep   — 0/1/2 = normal · 3/4/5 = skill sonrası chained
    ///   trigger Attack     — her vuruşta tetiklenir
    /// </summary>
    // NOTE: intentionally NOT [RequireComponent(typeof(PlayerController))]. The base
    // Player.prefab is controller-less by design — each concrete player instance
    // (Warblade variant, arena scene placement) supplies its own PlayerController.
    // RequireComponent here would force a second PlayerController onto the base, which
    // would then duplicate against every instance's own. PlayerAnimator null-guards the
    // controller, so the dependency is enforced softly instead.
    public class PlayerAnimator : MonoBehaviour
    {
        private PlayerController controller;
        private PlayerAttack     attack;
        private Animator         anim;
        private SpriteRenderer   sr;
        private Health           health;

        // Persistent sprite-keeper (mirrors EnemyAnimator). Some classes' idle clips point at
        // missing/stale sprite GUIDs (e.g. Elementalist idle clips → GUID 927669a7, dropped), so the
        // animator drives the Body SpriteRenderer to null EVERY frame → invisible. The one-time class
        // fallback in PlayerClassManager is overwritten next frame; this restores the cached class
        // idle sprite each LateUpdate (after the animator's write). No-ops for classes whose animator
        // produces a valid sprite (Warblade). Post-demo: re-point the broken idle clips and this
        // keeper becomes inert. Source = PlayerClassManager.SetFallbackSprite after class-apply.
        private Sprite fallbackSprite;

        // ─── Parameter hashes ────────────────────────────────────────────────

        private static readonly int SpeedHash     = Animator.StringToHash("Speed");
        private static readonly int DirXHash      = Animator.StringToHash("DirX");
        private static readonly int DirYHash      = Animator.StringToHash("DirY");
        private static readonly int IsDeadHash    = Animator.StringToHash("IsDead");
        private static readonly int IsDashHash    = Animator.StringToHash("IsDashing");
        private static readonly int ComboStepHash = Animator.StringToHash("ComboStep");
        private static readonly int AttackHash    = Animator.StringToHash("Attack");

        [Header("Movement Feel")]
        [SerializeField] private float moveAnimSpeed = 1.5f;

        [Header("Turn Feel")]
        [SerializeField] private float adjacentTurnDelay = 0.05f;
        [SerializeField] private float oppositeTurnDelay = 0.10f;

        private Vector2 lastDir    = new(1f, -1f);
        private Vector2 lastFacing = new(1f, -1f);
        private Vector2 movementFacingBeforeCombat = new(1f, -1f);
        private Vector2 pendingFacing;
        private float pendingFacingStartedAt = -1f;
        private bool wasCombatFacingOverride;

        // ─── Init ────────────────────────────────────────────────────────────

        private void Awake()
        {
            controller = GetComponent<PlayerController>();
            attack     = GetComponent<PlayerAttack>();
            health     = GetComponent<Health>();
            anim       = GetComponentInChildren<Animator>();
            // Animator ile aynı GameObject'teki SR'ı al (Sprite child).
            // GetComponentInChildren root'u da arar — Player root'ta ayrı SR var,
            // yanlış olanı döndürürdü.
            sr = anim != null ? anim.GetComponent<SpriteRenderer>() : GetComponentInChildren<SpriteRenderer>();
            // 8-full-sprite scheme: each direction is a distinct sprite, so flipX must
            // stay false (a separate weapon hand-anchor reads facing; mirroring the body
            // would desync the weapon hand side). Clear it once defensively; do NOT clear
            // it every frame.
            if (sr != null) sr.flipX = false;
        }

        private void OnEnable()
        {
            if (attack != null) attack.OnComboStep += HandleComboStep;
            if (controller != null) controller.CombatFacingChanged += HandleCombatFacingChanged;
        }

        private void OnDisable()
        {
            if (attack != null) attack.OnComboStep -= HandleComboStep;
            if (controller != null) controller.CombatFacingChanged -= HandleCombatFacingChanged;
        }

        // ─── Update ──────────────────────────────────────────────────────────

        private void Update()
        {
            if (controller == null || anim == null || anim.runtimeAnimatorController == null) return;

            bool combatOverrideActive = controller.HasCombatFacingOverride;
            bool combatOverrideJustEnded = wasCombatFacingOverride && !combatOverrideActive;

            Vector2 dir = controller.FacingDirection;
            if (dir.sqrMagnitude > 0.01f)
            {
                lastDir    = dir.normalized;
                Vector2 targetFacing = combatOverrideJustEnded && controller.IsMoving
                    ? SnapToEight(controller.MovementFacingDirection, movementFacingBeforeCombat)
                    : SnapToEight(lastDir, lastFacing);

                if (!combatOverrideActive && controller.IsMoving)
                    movementFacingBeforeCombat = targetFacing;

                lastFacing = combatOverrideActive || !controller.IsMoving || combatOverrideJustEnded
                    ? ApplyFacingImmediately(targetFacing)
                    : ResolveMovementFacing(targetFacing);
            }

            bool showMovement = controller.IsMoving && !combatOverrideActive;

            anim.speed = showMovement ? moveAnimSpeed : 1f;

            anim.SetFloat(SpeedHash,  showMovement ? 1f : 0f);
            anim.SetFloat(DirXHash,   lastFacing.x);
            anim.SetFloat(DirYHash,   lastFacing.y);
            anim.SetBool(IsDashHash,  controller.IsDashing);

            wasCombatFacingOverride = combatOverrideActive;
        }

        // Runs after the Animator's per-frame sprite write; restores the cached class idle sprite
        // when a broken idle clip drove the Body SpriteRenderer.sprite to null. Skipped while dead
        // so a death animation that clears the sprite isn't fought (mirrors EnemyAnimator._isDead).
        private void LateUpdate()
        {
            if (sr == null || fallbackSprite == null) return;
            // Health is often added to the player AFTER PlayerAnimator.Awake (RoomRunDirector /
            // ChamberSelectBootstrap AddIfMissing<Health> post-instantiate), so resolve it lazily
            // until present. Death guard: skip the restore once dead so a sprite-clearing death
            // animation isn't fought (mirrors EnemyAnimator._isDead).
            if (health == null) health = GetComponent<Health>();
            if (health != null && health.IsDead) return;
            // Broadened from `sprite == null`: a clip can also write a NON-NULL but textureless
            // sprite (the same failure class that left enemy bodies as empty squares), which the
            // null-only guard missed. Restore the cached class idle sprite in both cases.
            if (sr.sprite == null || sr.sprite.texture == null) sr.sprite = fallbackSprite;
        }

        // ─── Sprite-keeper wiring ──────────────────────────────────────────────

        /// <summary>PlayerClassManager publishes the chosen class idle sprite here after class-apply
        /// so the keeper has a valid (non-null) fallback even when the animator never produces one.</summary>
        public void SetFallbackSprite(Sprite sprite)
        {
            if (sprite != null) fallbackSprite = sprite;
        }

        // ─── Combo ───────────────────────────────────────────────────────────

        private void HandleComboStep(int step)
        {
            if (anim == null || anim.runtimeAnimatorController == null) return;
            ApplyCombatFacingImmediately();
            anim.SetInteger(ComboStepHash, step % 3);
            anim.SetTrigger(AttackHash);
        }

        private void HandleCombatFacingChanged()
        {
            if (controller != null && !controller.HasCombatFacingOverride && controller.IsMoving)
                movementFacingBeforeCombat = SnapToEight(controller.MovementFacingDirection, lastFacing);
            ApplyCombatFacingImmediately();
        }

        // ─── Death ───────────────────────────────────────────────────────────

        /// <summary>Health.OnDeath → Inspector'dan bağla.</summary>
        public void TriggerDeath()
        {
            if (anim != null) anim.SetTrigger(IsDeadHash);
        }

        // Returns one of the 8 facings (S, SE, E, NE, N, NW, W, SW) as a (DirX,DirY)
        // pair with each component in {-1,0,1}. The angle is quantised to the nearest
        // 45° sector; cardinals zero one axis, diagonals set both to ±1 — matching the
        // Warblade controller's AnyState DirX/DirY thresholds (±0.5 bands).
        private static Vector2 SnapToEight(Vector2 dir, Vector2 previousFacing)
        {
            if (dir.sqrMagnitude <= 0.0001f) return previousFacing;

            // Sector index 0..7 measured CCW from +X (East). Each sector spans 45°.
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // -180..180
            int sector = Mathf.RoundToInt(angle / 45f) & 7;          // wrap into 0..7

            // sector → (x,y) in {-1,0,1}: 0=E 1=NE 2=N 3=NW 4=W 5=SW 6=S 7=SE
            switch (sector)
            {
                case 0: return new Vector2( 1f,  0f); // E
                case 1: return new Vector2( 1f,  1f); // NE
                case 2: return new Vector2( 0f,  1f); // N
                case 3: return new Vector2(-1f,  1f); // NW
                case 4: return new Vector2(-1f,  0f); // W
                case 5: return new Vector2(-1f, -1f); // SW
                case 6: return new Vector2( 0f, -1f); // S
                default: return new Vector2( 1f, -1f); // SE (sector 7)
            }
        }

        private Vector2 ResolveMovementFacing(Vector2 targetFacing)
        {
            if (SameFacing(lastFacing, targetFacing))
            {
                ClearPendingFacing();
                return lastFacing;
            }

            if (!SameFacing(pendingFacing, targetFacing))
            {
                pendingFacing = targetFacing;
                pendingFacingStartedAt = Time.time;
                return lastFacing;
            }

            float delay = IsOppositeFacing(lastFacing, targetFacing) ? oppositeTurnDelay : adjacentTurnDelay;
            if (Time.time - pendingFacingStartedAt < delay) return lastFacing;

            return ApplyFacingImmediately(targetFacing);
        }

        private Vector2 ApplyFacingImmediately(Vector2 facing)
        {
            ClearPendingFacing();
            return facing;
        }

        private void ApplyCombatFacingImmediately()
        {
            if (controller == null) return;
            Vector2 dir = controller.FacingDirection;
            if (dir.sqrMagnitude <= 0.0001f) return;

            lastDir = dir.normalized;
            lastFacing = ApplyFacingImmediately(SnapToEight(lastDir, lastFacing));

            if (anim == null || anim.runtimeAnimatorController == null) return;
            anim.SetFloat(DirXHash, lastFacing.x);
            anim.SetFloat(DirYHash, lastFacing.y);
        }

        private void ClearPendingFacing()
        {
            pendingFacing = Vector2.zero;
            pendingFacingStartedAt = -1f;
        }

        private static bool SameFacing(Vector2 a, Vector2 b)
        {
            return (a - b).sqrMagnitude < 0.001f;
        }

        private static bool IsOppositeFacing(Vector2 a, Vector2 b)
        {
            return Mathf.Sign(a.x) != Mathf.Sign(b.x) && Mathf.Sign(a.y) != Mathf.Sign(b.y);
        }
    }
}
