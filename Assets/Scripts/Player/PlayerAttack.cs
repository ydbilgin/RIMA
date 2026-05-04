using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// Tüm class'ların basic attack combo sistemi.
    ///
    /// Davranış:
    ///   Click 1 → ComboStep 0 animasyonu
    ///   Click 2 (comboWindow içinde) → ComboStep 1 animasyonu
    ///   Click 3 → ComboStep 2 animasyonu → reset
    ///   comboWindow dolunca → ComboStep 0'a döner
    ///
    /// Input buffer: saldırı commitment süresi içinde basılırsa
    /// commitment biter bitmez otomatik ateşlenir (combo akışını kesmez).
    ///
    /// Hasar: PlayerAnimator'a bırakmak yerine burada hesaplanır.
    /// Animator sadece görsel — hitbox bu script'te.
    /// </summary>
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAttack : MonoBehaviour
    {
        // ─── Config ──────────────────────────────────────────────────────────

        [Header("Combo")]
        [SerializeField] private int   comboLength    = 3;
        [SerializeField] private float comboWindow    = 1.2f;   // son vuruştan sonra kaç saniye bekler
        [SerializeField] private float commitment     = 0.28f;  // her vuruşun "hareket kilitli" süresi

        [Header("Damage (per combo step)")]
        [SerializeField] private int[] comboDamage = { 25, 30, 40 };

        [Header("Hitbox (per combo step)")]
        [SerializeField] private float[] hitRange = { 1.2f, 1.3f, 1.5f };
        [SerializeField] private float   hitArc   = 0.9f;

        [Header("Knockback")]
        [SerializeField] private float[] knockbackForce    = { 4f, 5f, 8f };
        [SerializeField] private float[] knockbackDuration = { 0.10f, 0.12f, 0.18f };

        [Header("VFX")]
        [SerializeField] private SlashArcVFX slashArcVFX;

        [Header("Elementalist Auto Attack")]
        [SerializeField] private int elementalistBoltDamage = 18;
        [SerializeField] private float elementalistBoltSpeed = 15f;
        [SerializeField] private float elementalistBoltCooldown = 0.34f;

        [Header("Ranger Auto Attack")]
        [SerializeField] private int rangerArrowDamage = 18;
        [SerializeField] private float rangerArrowSpeed = 18f;
        [SerializeField] private float rangerArrowCooldown = 0.32f;
        [SerializeField] private float rangerChargeThreshold = 1f;

        [Header("Shadowblade Auto Attack")]
        [SerializeField] private int shadowbladeStrikeDamage = 20;
        [SerializeField] private float shadowbladeStrikeRange = 1.35f;
        [SerializeField] private float shadowbladeStrikeRadius = 0.75f;
        [SerializeField] private float shadowbladeStrikeCooldown = 0.26f;

        [Header("RMB Actions")]
        [SerializeField] private int warbladeRageOutletCost = 30;
        [SerializeField] private int warbladeRageOutletDamage = 34;
        [SerializeField] private float warbladeRageOutletRadius = 2.2f;
        [SerializeField] private float warbladeRageOutletCooldown = 1.5f;

        // ─── Events ──────────────────────────────────────────────────────────

        /// <summary>
        /// Her vuruşta fırlar. int = ComboStep (0, 1, 2).
        /// PlayerAnimator bu event'i dinleyerek Animator'u günceller.
        /// </summary>
        public event Action<int> OnComboStep;

        // ─── State ───────────────────────────────────────────────────────────

        private int   comboStep;
        private float comboTimer;
        private float commitTimer;
        private bool  bufferedAttack;

        /// <summary>Saldırı commitment süresinde true — hareket sistemi bunu okuyabilir.</summary>
        public bool IsCommitted => commitTimer > 0f;

        // ─── Refs ────────────────────────────────────────────────────────────

        private PlayerController controller;
        private RageSystem        rage;
        private SkillFlowTracker  flowTracker;
        private InputAction       attackAction;
        private InputAction       secondaryAction;
        private InputAction       riftBreakAction;
        private float rangerAttackStartedAt = -1f;
        private float elementalistSecondaryStartedAt = -1f;
        private float warbladeRageOutletTimer;

        /// <summary>IronCrush gibi skill'ler yazar.</summary>
        [HideInInspector] public float outgoingDamageMultiplier = 1f;

        /// <summary>VeteranScar gibi pasifler yazar — flat hasar bonusu.</summary>
        [HideInInspector] public int baseDamage = 0;

        // ─── Init ────────────────────────────────────────────────────────────

        private void Awake()
        {
            controller  = GetComponent<PlayerController>();
            rage        = GetComponent<RageSystem>();
            flowTracker = GetComponent<SkillFlowTracker>();

            attackAction = new InputAction("Attack", InputActionType.Button);
            attackAction.AddBinding("<Mouse>/leftButton");
            attackAction.AddBinding("<Gamepad>/buttonWest");

            secondaryAction = new InputAction("ClassSecondary", InputActionType.Button);
            secondaryAction.AddBinding("<Mouse>/rightButton");
            secondaryAction.AddBinding("<Gamepad>/buttonEast");

            riftBreakAction = new InputAction("RiftBreak", InputActionType.Button);
            riftBreakAction.AddBinding("<Keyboard>/v");
        }

        private void OnEnable()  { attackAction.Enable(); secondaryAction.Enable(); riftBreakAction.Enable(); }
        private void OnDisable() { attackAction.Disable(); secondaryAction.Disable(); riftBreakAction.Disable(); }

        // ─── Update ──────────────────────────────────────────────────────────

        private void Update()
        {
            // Commitment timer
            if (commitTimer > 0f)
            {
                commitTimer -= Time.deltaTime;
                if (commitTimer <= 0f && bufferedAttack)
                {
                    bufferedAttack = false;
                    ExecutePrimaryAttack();
                }
            }

            // Combo reset timer
            if (comboTimer > 0f)
            {
                comboTimer -= Time.deltaTime;
                if (comboTimer <= 0f) comboStep = 0;
            }
            if (warbladeRageOutletTimer > 0f)
                warbladeRageOutletTimer -= Time.deltaTime;

            ClassType primary = PlayerClassManager.Instance != null
                ? PlayerClassManager.Instance.PrimaryClass
                : ClassType.Warblade;

            // Attack input
            if (primary == ClassType.Ranger)
            {
                if (attackAction.WasPressedThisFrame())
                    rangerAttackStartedAt = Time.time;
                if (attackAction.WasReleasedThisFrame() && rangerAttackStartedAt >= 0f)
                {
                    ExecuteRangerArrow(Time.time - rangerAttackStartedAt >= rangerChargeThreshold);
                    rangerAttackStartedAt = -1f;
                }
            }
            else if (attackAction.WasPressedThisFrame())
            {
                if (commitTimer > 0f)
                    bufferedAttack = true;
                else
                    ExecutePrimaryAttack();
            }

            if (primary == ClassType.Elementalist)
            {
                if (secondaryAction.WasPressedThisFrame())
                    elementalistSecondaryStartedAt = Time.time;
                if (secondaryAction.WasReleasedThisFrame() && elementalistSecondaryStartedAt >= 0f)
                {
                    ExecuteElementalistSecondary(Time.time - elementalistSecondaryStartedAt >= 0.2f);
                    elementalistSecondaryStartedAt = -1f;
                }
            }
            else if (secondaryAction.WasPressedThisFrame())
            {
                ExecuteClassSecondary(primary);
            }

            // V — Rift Break: tam bar harcayarak tetikle
            if (rage == null) rage = GetComponent<RageSystem>();
            if (riftBreakAction.WasPressedThisFrame() && rage != null)
            {
                if (rage.TryConsume(100))
                    Debug.Log("[RiftBreak] Triggered!");
                else
                    Debug.Log("[RiftBreak] Not enough rage.");
            }
        }

        // ─── Core ────────────────────────────────────────────────────────────

        private void ExecutePrimaryAttack()
        {
            if (PlayerClassManager.Instance != null &&
                PlayerClassManager.Instance.PrimaryClass == ClassType.Elementalist)
            {
                ExecuteElementalistBolt();
                return;
            }
            if (PlayerClassManager.Instance != null &&
                PlayerClassManager.Instance.PrimaryClass == ClassType.Shadowblade)
            {
                ExecuteShadowbladeStrike();
                return;
            }

            ExecuteAttack();
        }

        private void ExecuteElementalistBolt()
        {
            controller.FaceCombatTarget();

            commitTimer = elementalistBoltCooldown;
            comboTimer = 0f;
            comboStep = 0;

            Vector2 dir = controller.FacingDirection.sqrMagnitude > 0.01f
                ? controller.FacingDirection.normalized
                : Vector2.right;

            var elementalist = GetComponent<Elementalist_SkillController>();
            bool empowered = elementalist != null && elementalist.RegisterRiftBoltShot();
            int damage = empowered ? Mathf.RoundToInt(elementalistBoltDamage * 1.6f) : elementalistBoltDamage;

            var go = new GameObject("RiftBolt_Runtime");
            go.transform.position = transform.position + (Vector3)(dir * 0.35f);

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = empowered ? 0.22f : 0.15f;
            col.isTrigger = true;

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
            renderer.color = GetRiftBoltColor(elementalist);
            renderer.sortingLayerName = "VFX";
            renderer.sortingOrder = 20;

            float scale = empowered ? 0.52f : 0.34f;
            go.transform.localScale = new Vector3(scale, scale, 1f);

            var projectile = go.AddComponent<PlayerProjectile>();
            projectile.Init(dir * elementalistBoltSpeed, damage, life: 2.2f, piercing: false);
            projectile.SetOnHit(hit =>
            {
                elementalist?.RegisterRiftBoltHit(empowered);
                var status = hit.GetComponent<StatusEffectSystem>();
                if (status == null || elementalist == null) return;
                if (elementalist.ActiveElement == ElementalistElement.Fire)
                    status.ApplyEffect(StatusEffectType.Burning, 3f);
                else if (elementalist.ActiveElement == ElementalistElement.Frost)
                    status.ApplyEffect(StatusEffectType.Chill, 2f);
                else
                    status.ApplyEffect(StatusEffectType.RiftMark, 4f);
            });

            LightPulse.Emit(new Color(0.28f, 0.76f, 1f), 0.8f, 0.06f);
        }

        private static Color GetRiftBoltColor(Elementalist_SkillController elementalist)
        {
            if (elementalist == null) return new Color(0.34f, 0.82f, 1f, 0.92f);
            return elementalist.ActiveElement switch
            {
                ElementalistElement.Fire => new Color(1f, 0.42f, 0.16f, 0.95f),
                ElementalistElement.Frost => new Color(0.34f, 0.82f, 1f, 0.92f),
                ElementalistElement.Light => new Color(1f, 0.9f, 0.36f, 0.95f),
                _ => new Color(0.34f, 0.82f, 1f, 0.92f)
            };
        }

        private void ExecuteRangerArrow(bool charged)
        {
            if (commitTimer > 0f) return;
            controller.FaceCombatTarget();
            commitTimer = rangerArrowCooldown;
            comboTimer = 0f;
            comboStep = 0;

            Vector2 dir = controller.FacingDirection.sqrMagnitude > 0.01f ? controller.FacingDirection.normalized : Vector2.right;
            int damage = charged ? Mathf.RoundToInt(rangerArrowDamage * 1.8f) : rangerArrowDamage;
            float scale = charged ? 0.42f : 0.28f;
            var projectile = SkillRuntime.SpawnProjectile((Vector2)transform.position + dir * 0.35f, dir, rangerArrowSpeed, damage, new Color(0.4f, 0.95f, 0.42f, 0.95f), scale, 3f, "RiftArrow_Runtime");
            projectile.SetOnHit(hit =>
            {
                GetComponent<FocusSystem>()?.Add(charged ? 8 : 4);
                var health = hit.GetComponent<Health>();
                if (charged && health != null)
                    SkillRuntime.State(health)?.Apply(SkillStateTracker.RangerMarked, 8f, 2, 5);
            });
        }

        private void ExecuteShadowbladeStrike()
        {
            if (commitTimer > 0f) return;
            controller.FaceCombatTarget();
            commitTimer = shadowbladeStrikeCooldown;
            comboTimer = 0f;
            comboStep = 0;

            Vector2 facing = controller.FacingDirection.sqrMagnitude > 0.01f ? controller.FacingDirection.normalized : Vector2.right;
            Vector2 center = (Vector2)transform.position + facing * shadowbladeStrikeRange;
            foreach (var health in SkillRuntime.EnemiesInCircle(center, shadowbladeStrikeRadius))
            {
                SkillRuntime.DealDamage(health, shadowbladeStrikeDamage);
                SkillRuntime.State(health)?.Apply(SkillStateTracker.RiftScar, 6f, 1, 5);
                SkillRuntime.State(health)?.Apply(SkillStateTracker.BackstabMarked, 6f, 1, 1);
                GetComponent<Shadowblade_SkillController>()?.AddSever(8);
            }

            SkillRuntime.SpawnCircleVisual(center, new Color(0.55f, 0.22f, 0.82f, 0.48f), 0.8f, 0.12f, "VeilStrike_Runtime");
        }

        private void ExecuteElementalistSecondary(bool held)
        {
            var elementalist = GetComponent<Elementalist_SkillController>();
            if (elementalist == null) return;
            if (held && elementalist.TryLightbreak()) return;
            elementalist.SwitchElement();
        }

        private void ExecuteClassSecondary(ClassType primary)
        {
            controller.FaceCombatTarget();
            switch (primary)
            {
                case ClassType.Warblade:
                    ExecuteWarbladeRageOutlet();
                    break;
                case ClassType.Ranger:
                    if (GetComponent<Ranger_SkillController>()?.TryTacticalRoll(controller.FacingDirection) == true)
                        ExecuteRangerArrow(false);
                    break;
                case ClassType.Shadowblade:
                    GetComponent<Shadowblade_SkillController>()?.TryVeilFlicker(controller.FacingDirection);
                    break;
            }
        }

        private void ExecuteWarbladeRageOutlet()
        {
            if (rage == null) rage = GetComponent<RageSystem>();
            if (warbladeRageOutletTimer > 0f || rage == null || !rage.TryConsume(warbladeRageOutletCost))
                return;

            foreach (var health in SkillRuntime.EnemiesInCircle(transform.position, warbladeRageOutletRadius))
            {
                SkillRuntime.DealDamage(health, warbladeRageOutletDamage);
                health.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Stunned, 0.45f);
            }

            warbladeRageOutletTimer = warbladeRageOutletCooldown;
            LightPulse.Emit(new Color(0.85f, 0.36f, 0.2f), 1f, 0.08f);
        }

        private void ExecuteAttack()
        {
            controller.FaceCombatTarget();

            int  step      = comboStep;
            bool isChained = flowTracker != null && flowTracker.IsChainedToBasic;

            // Timers
            commitTimer = commitment;
            comboTimer  = comboWindow;

            // Combo advance (döngüsel)
            comboStep = (comboStep + 1) % comboLength;

            // Bildir → PlayerAnimator yakalar (step + chain bilgisi)
            OnComboStep?.Invoke(isChained ? step + comboLength : step);

            // Slash arc VFX
            slashArcVFX?.Emit(controller.FacingDirection, step);
            // step 0/1/2 = normal · step 3/4/5 = chained (Animator ComboStep param)

            // Hasar (chained ise bonus multiplier)
            float chainMult = flowTracker != null ? flowTracker.ConsumeBasicChain() : 1f;
            ApplyHit(step, chainMult);
        }

        private void ApplyHit(int step, float chainMult = 1f)
        {
            if (rage == null) rage = GetComponent<RageSystem>();
            int   dmg   = Mathf.RoundToInt((comboDamage[Mathf.Min(step, comboDamage.Length - 1)] + baseDamage) * outgoingDamageMultiplier * chainMult);
            float range = hitRange[Mathf.Min(step, hitRange.Length - 1)];

            Vector2 facing    = controller.FacingDirection;
            Vector2 hitCenter = (Vector2)transform.position + facing * range;

            foreach (var col in Physics2D.OverlapCircleAll(hitCenter, hitArc))
            {
                if (col.gameObject == gameObject) continue;
                var hp = col.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                var status = col.GetComponent<StatusEffectSystem>();
                int finalDmg = status != null
                    ? Mathf.RoundToInt(dmg * status.damageMultiplierIncoming)
                    : dmg;

                hp.TakeDamage(finalDmg);
                rage?.OnHitEnemy();
                HitStop.Instance?.FreezeLight();
                LightPulse.Emit(new Color(0.4f, 0.7f, 1f), 1.5f, 0.10f); // Warblade cold blue hit flash
                DamagePopup.Show(col.transform.position, finalDmg);

                // Finisher'da kamera sallama
                if (step == comboLength - 1)
                    CameraShake.Instance?.Shake(0.18f, 0.12f);

                // Knockback
                var kb = col.GetComponent<KnockbackReceiver>();
                if (kb != null)
                {
                    float kbForce = knockbackForce[Mathf.Min(step, knockbackForce.Length - 1)];
                    float kbDur   = knockbackDuration[Mathf.Min(step, knockbackDuration.Length - 1)];
                    kb.ApplyKnockback(facing, kbForce, kbDur);
                }
            }
        }

        // ─── Gizmos ──────────────────────────────────────────────────────────

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying || controller == null) return;
            int   idx   = Mathf.Min(comboStep, hitRange.Length - 1);
            float range = hitRange[idx];
            Gizmos.color = new Color(1f, 1f, 0f, 0.35f);
            Gizmos.DrawWireSphere((Vector2)transform.position + controller.FacingDirection * range, hitArc);
        }
    }
}
