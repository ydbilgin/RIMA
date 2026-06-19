using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Tints sprite renderers on this GameObject (and children) based on active status effects.
    /// Dead Cells-style juice: Chill=blue, Burning=red with flicker, Frozen=cyan-blue, etc.
    /// Auto-attached by StatusEffectSystem.Start. Runs in LateUpdate at high order so it
    /// wins over any mob LateUpdate that resets SR.color.
    /// </summary>
    [DefaultExecutionOrder(200)]
    public class StatusEffectTint : MonoBehaviour
    {
        // ── Tint colours per effect ─────────────────────────────────────────
        static readonly Color TintFrozen   = new Color(0.40f, 0.70f, 1.00f);
        static readonly Color TintScorch   = new Color(1.00f, 0.45f, 0.20f);
        static readonly Color TintBurning  = new Color(1.00f, 0.35f, 0.25f);
        static readonly Color TintChill    = new Color(0.55f, 0.75f, 1.00f);
        static readonly Color TintPoison   = new Color(0.50f, 1.00f, 0.40f);
        static readonly Color TintStunned  = new Color(1.00f, 0.95f, 0.40f);
        static readonly Color TintWeakened = new Color(0.70f, 0.50f, 0.90f);
        static readonly Color TintRiftMark = new Color(0.60f, 0.40f, 1.00f);
        static readonly Color TintNone     = Color.white;

        const float BASE_LERP      = 0.45f;   // steady tint strength
        const float DOT_MIN_LERP   = 0.30f;   // DoT flicker low
        const float DOT_MAX_LERP   = 0.60f;   // DoT flicker high
        const float DOT_FREQ       = 2.5f;    // Hz
        const float FLASH_LERP     = 0.75f;   // apply-flash strength
        const float FLASH_DURATION = 0.18f;   // seconds

        // ── Cached state ────────────────────────────────────────────────────
        SpriteRenderer[] renderers;
        Color[]          originalColors;
        StatusEffectSystem ses;

        // Flash state (no alloc)
        bool  flashActive;
        float flashTimer;
        Color flashColor;

        // ── Lifecycle ───────────────────────────────────────────────────────
        void Awake()
        {
            renderers      = GetComponentsInChildren<SpriteRenderer>(true);
            originalColors = new Color[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
                originalColors[i] = renderers[i].color;
        }

        void Start()
        {
            WireToSES();
        }

        void WireToSES()
        {
            if (ses != null) return;
            ses = GetComponent<StatusEffectSystem>();
            if (ses != null && ses.OnEffectApplied != null)
                ses.OnEffectApplied.AddListener(OnEffectApplied);
        }

        void OnDestroy()
        {
            if (ses != null && ses.OnEffectApplied != null)
                ses.OnEffectApplied.RemoveListener(OnEffectApplied);
        }

        // ── Apply-flash callback ────────────────────────────────────────────
        void OnEffectApplied(StatusEffectType type, int stacks)
        {
            flashColor  = EffectColor(type);
            flashTimer  = FLASH_DURATION;
            flashActive = true;
        }

        // ── Per-frame (LateUpdate: runs after mob Update, after EnsureVisibleSprite) ──
        void LateUpdate()
        {
            if (ses == null) { WireToSES(); return; }

            // Advance flash
            if (flashActive)
            {
                flashTimer -= Time.deltaTime;
                if (flashTimer <= 0f) flashActive = false;
            }

            // Dominant effect
            bool hasEffect = TryGetDominant(out StatusEffectType dominant);
            Color targetTint = hasEffect ? EffectColor(dominant) : Color.white;
            bool isDot = hasEffect && (dominant == StatusEffectType.Burning
                      || dominant == StatusEffectType.Scorch
                      || dominant == StatusEffectType.Poison);

            // Lerp factor: flash override > DoT pulse > base
            float lerpT;
            if (flashActive)
            {
                float progress = 1f - (flashTimer / FLASH_DURATION);    // 0→1 during flash
                lerpT     = Mathf.Lerp(FLASH_LERP, BASE_LERP, progress);
                targetTint = flashColor;
            }
            else if (!hasEffect)
            {
                lerpT = 0f;
            }
            else if (isDot)
            {
                float sine = Mathf.Sin(Time.time * DOT_FREQ * Mathf.PI * 2f) * 0.5f + 0.5f; // 0..1
                lerpT = Mathf.Lerp(DOT_MIN_LERP, DOT_MAX_LERP, sine);
            }
            else
            {
                lerpT = BASE_LERP;
            }

            // Apply to each renderer
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] == null) continue;
                Color orig   = originalColors[i];
                Color tinted = orig * targetTint;
                renderers[i].color = lerpT > 0f
                    ? Color.Lerp(orig, tinted, lerpT)
                    : orig;
            }
        }

        // ── Helpers ─────────────────────────────────────────────────────────
        bool TryGetDominant(out StatusEffectType result)
        {
            result = default;
            if (ses == null) return false;

            if (ses.HasEffect(StatusEffectType.Frozen))   { result = StatusEffectType.Frozen;   return true; }
            if (ses.HasEffect(StatusEffectType.Scorch))   { result = StatusEffectType.Scorch;   return true; }
            if (ses.HasEffect(StatusEffectType.Burning))  { result = StatusEffectType.Burning;  return true; }
            if (ses.HasEffect(StatusEffectType.Chill))    { result = StatusEffectType.Chill;    return true; }
            if (ses.HasEffect(StatusEffectType.Poison))   { result = StatusEffectType.Poison;   return true; }
            if (ses.HasEffect(StatusEffectType.Stunned))  { result = StatusEffectType.Stunned;  return true; }
            if (ses.HasEffect(StatusEffectType.Weakened)) { result = StatusEffectType.Weakened; return true; }
            if (ses.HasEffect(StatusEffectType.RiftMark)) { result = StatusEffectType.RiftMark; return true; }
            return false;
        }

        static Color EffectColor(StatusEffectType t) => t switch
        {
            StatusEffectType.Frozen   => TintFrozen,
            StatusEffectType.Scorch   => TintScorch,
            StatusEffectType.Burning  => TintBurning,
            StatusEffectType.Chill    => TintChill,
            StatusEffectType.Poison   => TintPoison,
            StatusEffectType.Stunned  => TintStunned,
            StatusEffectType.Weakened => TintWeakened,
            StatusEffectType.RiftMark => TintRiftMark,
            _                         => TintNone
        };
    }
}
