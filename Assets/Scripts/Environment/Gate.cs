// Gate canonical spec: MEMORY/gate_socket_canonical_spec.md — 8 stil variant,
// 1.5-2x karakter, lock state machine, 6-8 frame open anim.

using System;
using System.Collections;
using UnityEngine;

namespace RIMA.Environment
{
    /// <summary>
    /// Room transition gate. State machine: Locked / AwaitingFragment / Unlocked / Unrevealed.
    /// Default state = AwaitingFragment (Demo Faz 1: room cleared, gate visible but locked until fragment).
    /// Procedural 8x8 grey placeholder sprite; room type tint applied from RoomTypeData.RoomCategory.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class Gate : MonoBehaviour
    {
        // ── State ─────────────────────────────────────────────────────────────────

        public enum State { Locked, AwaitingFragment, Unlocked, Unrevealed }

        /// <summary>Current gate state.</summary>
        public State CurrentState { get; private set; } = State.AwaitingFragment;

        // ── Inspector ─────────────────────────────────────────────────────────────

        [Tooltip("Optional room type that drives the tint colour.")]
        [SerializeField] private RoomTypeData roomType;

        [Tooltip("Tag the player GameObject uses.")]
        public string playerTag = "Player";

        // ── Events ────────────────────────────────────────────────────────────────

        /// <summary>Fired when a player enters an Unlocked gate.</summary>
        public event Action<Gate> OnPlayerEntered;

        // ── Private refs ──────────────────────────────────────────────────────────

        private SpriteRenderer _sr;
        private BoxCollider2D  _col;

        // ── Room-type tint mapping (canonical) ────────────────────────────────────

        private static Color TintForCategory(RoomTypeData.RoomCategory cat)
        {
            switch (cat)
            {
                case RoomTypeData.RoomCategory.Combat:       return Color.white;
                case RoomTypeData.RoomCategory.Treasure:     return new Color(1f, 0.85f, 0.2f); // gold (Shop)
                case RoomTypeData.RoomCategory.Ritual:       return new Color(0.7f, 0.3f, 1f);  // purple (Spirit)
                case RoomTypeData.RoomCategory.BossApproach: return new Color(1f, 0.3f, 0.3f);  // red (Boss/Elite)
                case RoomTypeData.RoomCategory.Bridge:       return Color.green;                 // Event
                default:                                      return Color.gray;
            }
        }

        // ── Sprite placeholder ────────────────────────────────────────────────────

        private static Sprite _cachedPlaceholder;

        private static Sprite BuildPlaceholderSprite()
        {
            if (_cachedPlaceholder != null) return _cachedPlaceholder;
            const int size = 8;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name       = "GatePlaceholderTex",
                filterMode = FilterMode.Point,
                wrapMode   = TextureWrapMode.Clamp,
                hideFlags  = HideFlags.DontSave
            };
            var pixels = new Color32[size * size];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = new Color32(200, 200, 200, 255); // grey
            tex.SetPixels32(pixels);
            tex.Apply(false, true);
            _cachedPlaceholder = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
            _cachedPlaceholder.name      = "GatePlaceholderSprite";
            _cachedPlaceholder.hideFlags = HideFlags.DontSave;
            return _cachedPlaceholder;
        }

        // ── Unity lifecycle ───────────────────────────────────────────────────────

        private void Awake()
        {
            _sr  = GetComponent<SpriteRenderer>();
            _col = GetComponent<BoxCollider2D>();

            if (_sr.sprite == null) _sr.sprite = BuildPlaceholderSprite();
            _sr.sortingOrder = 5;

            _col.isTrigger = true;
        }

        private void OnEnable()
        {
            ApplyTint();
            ApplyVisualForState(CurrentState);
        }

        // ── Public API ────────────────────────────────────────────────────────────

        /// <summary>
        /// Transition gate to newState. Updates visuals and collider.
        /// Alpha: Locked/AwaitingFragment=0.4, Unlocked=1.0, Unrevealed=0.2.
        /// Collider: enabled only when Unlocked.
        /// </summary>
        public void SetState(State newState)
        {
            CurrentState = newState;
            ApplyVisualForState(newState);
            Debug.Log($"[Gate] State → {newState} at {transform.position}");
        }

        /// <summary>Unlock the gate: SetState(Unlocked) + play open animation.</summary>
        public void Unlock()
        {
            if (CurrentState == State.Unlocked) return;   // Codex: idempotence — repeated calls replay SFX + restart anim
            SetState(State.Unlocked);
            RIMA.Audio.AudioManager.Play(RIMA.Audio.Sfx.GateOpen);
            StartCoroutine(OpenAnimCoroutine());
        }

        // ── Trigger ───────────────────────────────────────────────────────────────

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CurrentState != State.Unlocked) return;
            if (!other.CompareTag(playerTag)) return;

            Debug.Log($"[Gate] Player entered gate at {transform.position}");
            OnPlayerEntered?.Invoke(this);
        }

        // ── Visuals ───────────────────────────────────────────────────────────────

        private void ApplyTint()
        {
            if (_sr == null) _sr = GetComponent<SpriteRenderer>();
            if (roomType != null)
                _sr.color = TintForCategory(roomType.category);
        }

        private void ApplyVisualForState(State state)
        {
            if (_sr == null) _sr = GetComponent<SpriteRenderer>();
            if (_col == null) _col = GetComponent<BoxCollider2D>();

            float alpha;
            switch (state)
            {
                case State.Locked:
                case State.AwaitingFragment:
                    alpha = 0.4f;
                    break;
                case State.Unlocked:
                    alpha = 1.0f;
                    break;
                case State.Unrevealed:
                    alpha = 0.2f;
                    break;
                default:
                    alpha = 0.4f;
                    break;
            }

            var c = _sr.color;
            c.a     = alpha;
            _sr.color = c;

            _col.enabled = (state == State.Unlocked);
        }

        // ── Open anim coroutine (6-8 frame placeholder) ───────────────────────────

        // 0.4s total: scale Y 1.0 → 0.1 → 1.0 (squash) + alpha 0.4 → 1.0
        // 8 discrete frames, ~0.05s each
        private IEnumerator OpenAnimCoroutine()
        {
            const float frameDuration = 0.05f;
            Vector3 originalScale = transform.localScale;

            // 4 frames: squash down (scaleY 1→0.1, alpha 0.4→0.7)
            float[] squashY  = { 0.8f, 0.55f, 0.3f, 0.1f };
            float[] alphaOut = { 0.50f, 0.60f, 0.65f, 0.70f };

            for (int i = 0; i < squashY.Length; i++)
            {
                var s   = originalScale;
                s.y     = squashY[i];
                transform.localScale = s;

                var c = _sr.color;
                c.a   = alphaOut[i];
                _sr.color = c;

                yield return new WaitForSeconds(frameDuration);
            }

            // 4 frames: spring back (scaleY 0.1→1.0, alpha 0.7→1.0)
            float[] springY = { 0.3f, 0.65f, 0.95f, 1.0f };
            float[] alphaIn = { 0.80f, 0.90f, 0.95f, 1.00f };

            for (int i = 0; i < springY.Length; i++)
            {
                var s   = originalScale;
                s.y     = springY[i];
                transform.localScale = s;

                var c = _sr.color;
                c.a   = alphaIn[i];
                _sr.color = c;

                yield return new WaitForSeconds(frameDuration);
            }

            // Ensure final state is clean
            transform.localScale = originalScale;
            ApplyVisualForState(State.Unlocked);
        }
    }
}
