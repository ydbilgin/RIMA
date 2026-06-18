using UnityEngine;
using RIMA.Core;

namespace RIMA.Combat
{
    public sealed class OrientationSync : MonoBehaviour
    {
        [SerializeField] private Transform handAnchor;
        [SerializeField] private Transform weaponTransform;
        [SerializeField] private SpriteRenderer weaponRenderer; // auto-found from weaponTransform if null
        [SerializeField] private Vector2[] handOffsets = new Vector2[8]
        {
            new Vector2(0.00f, -0.08f),
            new Vector2(0.08f, -0.04f),
            new Vector2(0.10f, 0.00f),
            new Vector2(0.07f, 0.05f),
            new Vector2(0.00f, 0.08f),
            new Vector2(-0.07f, 0.05f),
            new Vector2(-0.10f, 0.00f),
            new Vector2(-0.08f, -0.04f)
        };
        [SerializeField] private float[] weaponRotations = new float[8]
        {
            -90f,
            -45f,
            0f,
            45f,
            90f,
            135f,
            180f,
            -135f
        };

        // ── Procedural swing state ───────────────────────────────────────────────
        // Swing composes ON TOP of the per-dir base rotation. When inactive, Sync()
        // alone drives the rotation. When active, Update() re-applies base + arc.
        private bool _swinging;
        private FacingDir8 _swingDir;
        private float _swingTimer;
        private float _swingDuration;
        private float _strikeSplit = 0.35f; // fraction of duration at which the blade crosses base (the strike frame)

        /// <summary>True while a procedural swing owns the weapon rotation.</summary>
        public bool IsSwinging => _swinging;
        // Arc shape relative to base: startup -45deg -> swing-through +90deg -> back to base.
        [SerializeField] private float swingBackswing = 45f;   // pulled back before the strike
        [SerializeField] private float swingFollowThrough = 90f; // overshoot past base on strike

        /// <summary>
        /// Wires the runtime-spawned weapon transform. Called by HandAnchorAttach after AttachWeapon().
        /// </summary>
        public void SetWeaponTransform(Transform weapon)
        {
            weaponTransform = weapon;
            // Always refresh — on weapon swap the old renderer reference is stale (destroyed instance).
            weaponRenderer = weapon != null ? weapon.GetComponentInChildren<SpriteRenderer>() : null;
        }

        public void Sync(FacingDir8 dir)
        {
            int index = (int)dir;
            if (index < 0 || index >= 8) return;

            if (handAnchor != null && handOffsets != null && index < handOffsets.Length)
            {
                handAnchor.localPosition = handOffsets[index];
                if (TryGetComponent<RIMA.CharacterJuice>(out var juice))
                    juice.SetHandBasePosition(handAnchor.localPosition);
            }

            ApplyFlipY(dir);

            // While a swing is active, Update() owns the rotation (so we don't fight it).
            if (_swinging) return;

            if (weaponTransform != null && weaponRotations != null && index < weaponRotations.Length)
            {
                weaponTransform.localRotation = Quaternion.Euler(0f, 0f, weaponRotations[index]);
            }
        }

        /// <summary>
        /// Per-direction flipY so a linear blade stays upright when its base rotation
        /// crosses the 90-270deg range. Left-facing dirs (W, NW, SW) flip; the rest do not.
        /// flipY (NOT flipX): flipX would mirror an asymmetric blade silhouette wrong.
        /// </summary>
        private void ApplyFlipY(FacingDir8 dir)
        {
            if (weaponRenderer == null) return;
            weaponRenderer.flipY = dir == FacingDir8.W || dir == FacingDir8.NW || dir == FacingDir8.SW;
        }

        /// <summary>
        /// Starts a time-based procedural swing for the given direction. The arc is
        /// applied RELATIVE to that direction's base rotation (composes with Sync()):
        /// base-backswing -> base+followThrough -> base, eased over <paramref name="duration"/>.
        /// <paramref name="strikeFraction"/> is the normalized time at which the blade crosses
        /// base (the strike frame) so it can be aligned to the mechanical hit (attackStartup).
        /// </summary>
        public void BeginSwing(FacingDir8 dir, float duration, float strikeFraction = 0.35f)
        {
            if (duration <= 0f) return;
            _swinging = true;
            _swingDir = dir;
            _swingDuration = duration;
            _swingTimer = 0f;
            _strikeSplit = Mathf.Clamp(strikeFraction, 0.05f, 0.95f);
            ApplyFlipY(dir);
        }

        private void Update()
        {
            if (!_swinging) return;
            if (weaponTransform == null) { _swinging = false; return; }

            _swingTimer += Time.deltaTime;
            float t = _swingDuration > 0f ? Mathf.Clamp01(_swingTimer / _swingDuration) : 1f;

            int index = (int)_swingDir;
            float baseRot = (weaponRotations != null && index >= 0 && index < weaponRotations.Length)
                ? weaponRotations[index]
                : 0f;

            // Phase 1 (t in [0,strikeSplit]): wind back to base-backswing, then snap through
            //   base so arc == 0 (the STRIKE) lands exactly at strikeSplit (= attackStartup).
            // Phase 2 (t in [strikeSplit,1]): follow through past base then ease back to base.
            float arc;
            float strikeSplit = _strikeSplit;
            if (t < strikeSplit)
            {
                // Wind back to -backswing over the first ~70% of the windup, then drive
                // forward so arc crosses 0 (base) precisely at t == strikeSplit (the strike).
                float p = t / strikeSplit;
                float wound = Mathf.Lerp(0f, -swingBackswing, Mathf.SmoothStep(0f, 1f, Mathf.Min(p / 0.7f, 1f)));
                arc = Mathf.Lerp(wound, 0f, Mathf.Max((p - 0.7f) / 0.3f, 0f));
            }
            else
            {
                float p = (t - strikeSplit) / (1f - strikeSplit);
                // 0 -> +followThrough -> 0, via smoothed sweep then settle back to base.
                float swept = Mathf.Lerp(0f, swingFollowThrough, Mathf.SmoothStep(0f, 1f, p));
                arc = Mathf.Lerp(swept, 0f, p * p); // settle back toward base near the end
            }

            weaponTransform.localRotation = Quaternion.Euler(0f, 0f, baseRot + arc);

            if (t >= 1f)
            {
                _swinging = false;
                // Snap exactly back to the base rotation so Sync() handoff is seamless.
                weaponTransform.localRotation = Quaternion.Euler(0f, 0f, baseRot);
            }
        }

        private void OnValidate()
        {
            EnsureLength(ref handOffsets, 8);
            EnsureLength(ref weaponRotations, 8);
        }

        private static void EnsureLength<T>(ref T[] values, int length)
        {
            if (values == null)
            {
                values = new T[length];
                return;
            }

            if (values.Length == length) return;

            T[] resized = new T[length];
            for (int i = 0; i < values.Length && i < resized.Length; i++)
            {
                resized[i] = values[i];
            }

            values = resized;
        }
    }
}
