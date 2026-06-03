// Map Fragment canonical spec: MEMORY/map_fragment_canonical_spec.md — Broken Stone Tablet,
// #00FFCC cyan, bobbing ±0.10u @ 2.2Hz, alpha pulse 0.6-1.0 @ 3Hz, G + 2.5u pickup.

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA.Environment
{
    /// <summary>
    /// A collectible map fragment. Cyan procedural placeholder sprite.
    /// Player presses G inside 2.5u pickup radius to collect.
    /// Fires MapFragment.OnAnyFragmentPickedUp (static) on pickup.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]
    public sealed class MapFragment : MonoBehaviour
    {
        // ── Public API ────────────────────────────────────────────────────────────

        /// <summary>Fired for any fragment pickup. Bridge/listeners subscribe statically.</summary>
        public static event Action<MapFragment> OnAnyFragmentPickedUp;

        /// <summary>True once this fragment has been collected.</summary>
        public bool isPickedUp { get; private set; }

        [Tooltip("Tag the player GameObject uses. Trigger fires only for this tag.")]
        public string playerTag = "Player";

        // ── Private state ─────────────────────────────────────────────────────────

        private SpriteRenderer _sr;
        private float _baseY;
        private bool _playerInRange;

        // Drop-in scale anim
        private const float DropDuration = 0.4f;
        private float _dropElapsed;

        // ── Sprite placeholder ────────────────────────────────────────────────────

        private static Sprite _cachedPlaceholder;

        private static Sprite BuildPlaceholderSprite()
        {
            if (_cachedPlaceholder != null) return _cachedPlaceholder;
            const int size = 64;
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name     = "MapFragmentPlaceholderTex",
                filterMode = FilterMode.Point,
                wrapMode   = TextureWrapMode.Clamp,
                hideFlags  = HideFlags.DontSave
            };
            var pixels = new Color32[size * size];
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    bool tablet = x >= 16 && x <= 47 && y >= 10 && y <= 53;
                    bool chip = (x < 24 && y > 45) || (x > 40 && y < 18);
                    bool crack = Mathf.Abs((x - 31) - (y - 32) / 2) <= 1 && y > 18 && y < 48;
                    Color32 color = new Color32(0, 0, 0, 0);
                    if (tablet && !chip) color = new Color32(15, 34, 40, 255);
                    if (tablet && !chip && (x <= 17 || x >= 46 || y <= 11 || y >= 52)) color = new Color32(0, 255, 204, 255);
                    if (crack) color = new Color32(0, 255, 204, 255);
                    pixels[y * size + x] = color;
                }
            }
            tex.SetPixels32(pixels);
            tex.Apply(false, true);
            _cachedPlaceholder = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 64f);
            _cachedPlaceholder.name      = "MapFragmentPlaceholderSprite";
            _cachedPlaceholder.hideFlags = HideFlags.DontSave;
            return _cachedPlaceholder;
        }

        // ── Unity lifecycle ───────────────────────────────────────────────────────

        private void Awake()
        {
            _sr = GetComponent<SpriteRenderer>();

            // Procedural cyan sprite (#00FFCC)
            if (_sr.sprite == null) _sr.sprite = BuildPlaceholderSprite();
            _sr.color       = new Color(0f, 1f, 0.8f, 1f);
            // Custom-Axis depth: live entities sort on the "Entities" layer by pivot. Keeping the
            // fragment here (not "Default") stops floor/decor from occluding the reward pickup.
            if (UnityEngine.SortingLayer.NameToID("Entities") != 0) _sr.sortingLayerName = "Entities";
            _sr.sortingOrder    = 5;
            _sr.spriteSortPoint = SpriteSortPoint.Pivot;

            // Pickup trigger collider
            var col          = GetComponent<CircleCollider2D>();
            col.isTrigger    = true;
            col.radius       = 2.5f; // canonical: 2.5u pickup radius

            _baseY       = transform.position.y;
            _dropElapsed = 0f;

            // Drop-in anim: start at scale 0
            transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            if (isPickedUp) return;

            // Drop-in scale anim (0 → 1 over 0.4s)
            if (_dropElapsed < DropDuration)
            {
                _dropElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(_dropElapsed / DropDuration);
                transform.localScale = Vector3.one * t;
                return; // skip idle anim while dropping in
            }

            // Idle bobbing: ±0.10u @ 2.2Hz (canonical)
            var pos = transform.position;
            pos.y = _baseY + 0.10f * Mathf.Sin(2f * Mathf.PI * 2.2f * Time.time);
            transform.position = pos;

            // Alpha pulse: 0.6 – 1.0 @ 3Hz (canonical)
            var c   = _sr.color;
            c.a     = Mathf.Lerp(0.6f, 1.0f, 0.5f + 0.5f * Mathf.Sin(2f * Mathf.PI * 3f * Time.time));
            _sr.color = c;

            // Bottom-screen interaction prompt "[G] ..." + G-key pickup, while in range.
            // Reuses the existing HUDController prompt (same pattern as RewardPickup).
            if (_playerInRange)
            {
                RIMA.HUDController.Instance?.SetInteractionPrompt("Parçayı Al");
                if (Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
                    Pickup();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(playerTag)) _playerInRange = true;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag(playerTag)) return;
            _playerInRange = true;

            // Safety: also catch G press here in case Enter2D was missed
            if (!isPickedUp && Keyboard.current != null && Keyboard.current.gKey.wasPressedThisFrame)
            {
                Pickup();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(playerTag))
            {
                _playerInRange = false;
                RIMA.HUDController.Instance?.HideInteractionPrompt();
            }
        }

        // ── Pickup logic ──────────────────────────────────────────────────────────

        private void Pickup()
        {
            if (isPickedUp) return;
            isPickedUp = true;
            RIMA.HUDController.Instance?.HideInteractionPrompt();
            Debug.Log($"[MapFragment] Picked up at {transform.position}");
            OnAnyFragmentPickedUp?.Invoke(this);
            Destroy(gameObject, 0.05f);
        }
    }
}
