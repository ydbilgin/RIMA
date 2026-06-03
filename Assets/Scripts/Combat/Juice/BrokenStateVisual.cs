using System.Collections;
using UnityEngine;
using RIMA.Audio;
using RIMA.Combat;

namespace RIMA
{
    /// <summary>
    /// A1 — Sundered Beat VISUAL TELL.
    /// Makes the Broken / Sundered combat states (already applied by Earthsplitter / SunderMark /
    /// HeatGauge and gated by DeathBlow) actually VISIBLE on the enemy:
    ///   • Broken   → red crack overlay + faint pulse.
    ///   • Sundered → orange-red split-crack overlay (brighter) + one-shot shatter-shard burst.
    /// Cyan is reserved for seal/player and is NOT used here (canon).
    ///
    /// Lazily added to an enemy by <see cref="SkillStateTracker"/> the first time it enters a
    /// Broken/Sundered state (mirrors how SkillRuntime.State / GroundBlobShadow.Ensure lazily
    /// attach), then drives itself event-driven off the tracker's OnStateEntered/OnStateExpired.
    /// Overlay is a child SpriteRenderer on sorting layer "Entities" / Pivot (Custom-Axis Y-sort
    /// per project rule — NEVER manual sortingOrder), following the enemy's facing/scale.
    /// Reuses: ElementalistRuntimeVisuals crack sprites, SkillRuntime.SpawnCircleVisual shards,
    /// AudioManager.Play (Sfx.Shatter), CombatEventBus.OnStatusApplied (VFXRouter pool hook).
    /// Placeholder crack sprites are procedural; replace with PixelLab crack decals later.
    /// </summary>
    [DisallowMultipleComponent]
    public class BrokenStateVisual : MonoBehaviour
    {
        private SkillStateTracker tracker;
        private SpriteRenderer hostRenderer;   // enemy's own SR — used for sort-point parity + sizing
        private SpriteRenderer overlay;        // child crack overlay
        private Coroutine pulseRoutine;
        private string activeKey;              // currently-shown key (null = none)

        // Canon colours. Broken = red, Sundered = orange-red. NO cyan.
        private static readonly Color BrokenColor   = new Color(0.85f, 0.12f, 0.10f, 0.85f);
        private static readonly Color SunderedColor = new Color(0.95f, 0.40f, 0.10f, 0.95f);

        /// <summary>
        /// Ensure this enemy carries the tell component. Safe to call repeatedly (lazy, idempotent).
        /// Called by SkillStateTracker when a Broken/Sundered state first appears.
        /// </summary>
        public static BrokenStateVisual Ensure(GameObject host)
        {
            if (host == null) return null;
            if (!host.TryGetComponent(out BrokenStateVisual visual))
                visual = host.AddComponent<BrokenStateVisual>();
            return visual;
        }

        private void Awake()
        {
            tracker = GetComponent<SkillStateTracker>();
            hostRenderer = GetComponentInChildren<SpriteRenderer>();
            BuildOverlay();
        }

        private void OnEnable()
        {
            if (tracker == null) tracker = GetComponent<SkillStateTracker>();
            if (tracker != null)
            {
                tracker.OnStateEntered += HandleStateEntered;
                tracker.OnStateExpired += HandleStateExpired;
            }
            // If this component pre-existed an apply (e.g. re-enable after being disabled), pick up
            // any state already present. On first-ever attach the tracker fires OnStateEntered right
            // after Ensure() returns (we are already subscribed), so this only matters on re-enable.
            if (activeKey == null) RefreshFromTracker();
        }

        private void OnDisable()
        {
            if (tracker != null)
            {
                tracker.OnStateEntered -= HandleStateEntered;
                tracker.OnStateExpired -= HandleStateExpired;
            }
        }

        private void BuildOverlay()
        {
            var go = new GameObject("BrokenStateOverlay");
            go.transform.SetParent(transform, false);
            go.transform.localPosition = Vector3.zero;

            overlay = go.AddComponent<SpriteRenderer>();
            overlay.sprite = ElementalistRuntimeVisuals.GetCrackSprite();
            // Custom-Axis Y-sort: sit on the same sorting layer as the enemy body, pivot sort point,
            // order 0 — NEVER a manual sortingOrder bump (project depth-sort rule).
            overlay.sortingLayerID = hostRenderer != null ? hostRenderer.sortingLayerID : SortingLayer.NameToID("Entities");
            overlay.sortingOrder = hostRenderer != null ? hostRenderer.sortingOrder : 0;
            overlay.spriteSortPoint = SpriteSortPoint.Pivot;

            var shader = Shader.Find("Sprites/Default");
            if (shader != null) overlay.sharedMaterial = new Material(shader);

            overlay.enabled = false;
        }

        private void RefreshFromTracker()
        {
            if (tracker == null) return;
            if (tracker.Has(SkillStateTracker.Sundered)) HandleStateEntered(SkillStateTracker.Sundered, tracker.GetStacks(SkillStateTracker.Sundered));
            else if (tracker.Has(SkillStateTracker.Broken)) HandleStateEntered(SkillStateTracker.Broken, tracker.GetStacks(SkillStateTracker.Broken));
        }

        private void HandleStateEntered(string key, int stacks)
        {
            // Sundered visually supersedes Broken; Broken doesn't override an active Sundered.
            if (key == SkillStateTracker.Sundered)
            {
                bool firstEnter = activeKey != SkillStateTracker.Sundered;
                ShowOverlay(SunderedColor, key);
                if (firstEnter)   // one-shot burst/SFX only when newly entering, not on re-apply
                {
                    SpawnShatterBurst();
                    AudioManager.Play(Sfx.Shatter);
                }
            }
            else if (key == SkillStateTracker.Broken)
            {
                if (activeKey == SkillStateTracker.Sundered) return;
                bool firstEnter = activeKey != SkillStateTracker.Broken;
                ShowOverlay(BrokenColor, key);
                if (firstEnter) AudioManager.Play(Sfx.Shatter, 0.6f);
            }
        }

        private void HandleStateExpired(string key)
        {
            if (key != activeKey) return;

            // Demote to the lower-tier tell if it is still present, else hide.
            if (key == SkillStateTracker.Sundered && tracker != null && tracker.Has(SkillStateTracker.Broken))
            {
                ShowOverlay(BrokenColor, SkillStateTracker.Broken);
                return;
            }
            HideOverlay();
        }

        private void ShowOverlay(Color color, string key)
        {
            activeKey = key;
            if (overlay == null) return;

            MatchHostBounds();
            overlay.color = color;
            overlay.enabled = true;

            if (pulseRoutine != null) StopCoroutine(pulseRoutine);
            pulseRoutine = StartCoroutine(PulseRoutine(color));
        }

        private void HideOverlay()
        {
            activeKey = null;
            if (pulseRoutine != null) { StopCoroutine(pulseRoutine); pulseRoutine = null; }
            if (overlay != null) overlay.enabled = false;
        }

        // Size the square crack sprite to roughly cover the enemy body and mirror its facing flip.
        private void MatchHostBounds()
        {
            if (overlay == null) return;
            float scale = 1f;
            bool flipX = false;
            if (hostRenderer != null && hostRenderer.sprite != null)
            {
                Vector2 size = hostRenderer.sprite.bounds.size;
                scale = Mathf.Max(0.3f, Mathf.Max(size.x, size.y));
                flipX = hostRenderer.flipX;
            }
            overlay.transform.localScale = new Vector3(scale, scale, 1f);
            overlay.flipX = flipX;
        }

        private IEnumerator PulseRoutine(Color baseColor)
        {
            // Faint breathing pulse so the tell reads as "ongoing state", not a one-frame flash.
            float t = 0f;
            while (overlay != null && overlay.enabled)
            {
                t += Time.deltaTime;
                float a = baseColor.a * (0.75f + 0.25f * Mathf.Sin(t * 6f));
                overlay.color = new Color(baseColor.r, baseColor.g, baseColor.b, a);
                // Keep tracking facing/scale in case the enemy turns while broken.
                MatchHostBounds();
                yield return null;
            }
            pulseRoutine = null;
        }

        // One-shot shard burst on entering Sundered. Reuses SkillRuntime.SpawnCircleVisual
        // (code-spawned VFX sprite, no prefab/art dependency).
        private void SpawnShatterBurst()
        {
            Vector2 origin = transform.position;
            const int shardCount = 6;
            for (int i = 0; i < shardCount; i++)
            {
                float ang = (Mathf.PI * 2f / shardCount) * i + Random.Range(-0.3f, 0.3f);
                Vector2 offset = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang)) * Random.Range(0.15f, 0.45f);
                SkillRuntime.SpawnCircleVisual(origin + offset, SunderedColor, 0.18f, 0.22f, "SunderedShard");
            }

            // Route through the shared bus too, so a future VFXRouter "status_Sundered" pool entry
            // (or any other subscriber) also fires — no behaviour change if no pool entry exists.
            CombatEventBus.PublishStatusApplied(new StatusEvent
            {
                worldPos = origin,
                target = gameObject,
                statusId = SkillStateTracker.Sundered,
                duration = 0f
            });
        }
    }
}
