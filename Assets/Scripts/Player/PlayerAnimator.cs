using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Tüm animator parametrelerini tek yerden yönetir.
    ///
    /// 4-way diagonal visual-direction system for isometric combat.
    /// Movement input is snapped to one of the camera-relative diagonals and the
    /// last snapped direction is preserved when movement stops.
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
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnimator : MonoBehaviour
    {
        private PlayerController controller;
        private PlayerAttack     attack;
        private Animator         anim;
        private SpriteRenderer   sr;

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
            anim       = GetComponentInChildren<Animator>();
            // Animator ile aynı GameObject'teki SR'ı al (Sprite child).
            // GetComponentInChildren root'u da arar — Player root'ta ayrı SR var,
            // yanlış olanı döndürürdü.
            sr = anim != null ? anim.GetComponent<SpriteRenderer>() : GetComponentInChildren<SpriteRenderer>();
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
            if (anim == null || anim.runtimeAnimatorController == null) return;

            bool combatOverrideActive = controller.HasCombatFacingOverride;
            bool combatOverrideJustEnded = wasCombatFacingOverride && !combatOverrideActive;

            Vector2 dir = controller.FacingDirection;
            if (dir.sqrMagnitude > 0.01f)
            {
                lastDir    = dir.normalized;
                Vector2 targetFacing = combatOverrideJustEnded && controller.IsMoving
                    ? SnapToFourDiagonal(controller.MovementFacingDirection, movementFacingBeforeCombat)
                    : SnapToFourDiagonal(lastDir, lastFacing);

                if (!combatOverrideActive && controller.IsMoving)
                    movementFacingBeforeCombat = targetFacing;

                lastFacing = combatOverrideActive || !controller.IsMoving || combatOverrideJustEnded
                    ? ApplyFacingImmediately(targetFacing)
                    : ResolveMovementFacing(targetFacing);
            }

            if (sr != null) sr.flipX = false;

            bool showMovement = controller.IsMoving && !combatOverrideActive;

            anim.speed = showMovement ? moveAnimSpeed : 1f;

            anim.SetFloat(SpeedHash,  showMovement ? 1f : 0f);
            anim.SetFloat(DirXHash,   lastFacing.x);
            anim.SetFloat(DirYHash,   lastFacing.y);
            anim.SetBool(IsDashHash,  controller.IsDashing);

            wasCombatFacingOverride = combatOverrideActive;
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
                movementFacingBeforeCombat = SnapToFourDiagonal(controller.MovementFacingDirection, lastFacing);
            ApplyCombatFacingImmediately();
        }

        // ─── Death ───────────────────────────────────────────────────────────

        /// <summary>Health.OnDeath → Inspector'dan bağla.</summary>
        public void TriggerDeath()
        {
            if (anim != null) anim.SetTrigger(IsDeadHash);
        }

        // Returns one of the 4 isometric diagonal facings: NE, NW, SW, SE.
        private static Vector2 SnapToFourDiagonal(Vector2 dir, Vector2 previousFacing)
        {
            if (dir.sqrMagnitude <= 0.0001f) return previousFacing;

            const float axisEpsilon = 0.001f;
            float x = Mathf.Abs(dir.x) > axisEpsilon
                ? Mathf.Sign(dir.x)
                : DiagonalAxisOrDefault(previousFacing.x, 1f);
            float y = Mathf.Abs(dir.y) > axisEpsilon
                ? Mathf.Sign(dir.y)
                : DiagonalAxisOrDefault(previousFacing.y, -1f);

            return new Vector2(x, y);
        }

        private static float DiagonalAxisOrDefault(float value, float fallback)
        {
            return Mathf.Abs(value) > 0.001f ? Mathf.Sign(value) : fallback;
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
            lastFacing = ApplyFacingImmediately(SnapToFourDiagonal(lastDir, lastFacing));

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
