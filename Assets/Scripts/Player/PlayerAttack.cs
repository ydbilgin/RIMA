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
        private InputAction       riftBreakAction;

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

            riftBreakAction = new InputAction("RiftBreak", InputActionType.Button);
            riftBreakAction.AddBinding("<Keyboard>/v");
        }

        private void OnEnable()  { attackAction.Enable(); riftBreakAction.Enable(); }
        private void OnDisable() { attackAction.Disable(); riftBreakAction.Disable(); }

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
                    ExecuteAttack();
                }
            }

            // Combo reset timer
            if (comboTimer > 0f)
            {
                comboTimer -= Time.deltaTime;
                if (comboTimer <= 0f) comboStep = 0;
            }

            // Attack input
            if (attackAction.WasPressedThisFrame())
            {
                if (commitTimer > 0f)
                    bufferedAttack = true;
                else
                    ExecuteAttack();
            }

            // V — Rift Break: tam bar harcayarak tetikle
            if (riftBreakAction.WasPressedThisFrame() && rage != null)
            {
                if (rage.TryConsume(100))
                    Debug.Log("[RiftBreak] Triggered!");
                else
                    Debug.Log("[RiftBreak] Not enough rage.");
            }
        }

        // ─── Core ────────────────────────────────────────────────────────────

        private void ExecuteAttack()
        {
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
