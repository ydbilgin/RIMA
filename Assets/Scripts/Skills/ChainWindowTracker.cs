using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// A4 — Real BREAK→EXECUTE chain-window tracker ("Sundered Beat" mechanical heart).
    ///
    /// Player-scoped MonoBehaviour (lives on the player root, exactly like
    /// <see cref="SkillFlowTracker"/> / <see cref="RageSystem"/>). Each chaining skill OPENS a
    /// named, timed window on cast; the follow-up skill reads <see cref="IsOpen"/> to know whether
    /// it is a CHAINED beat (combo continuation) instead of the old crude
    /// <c>CooldownPercent &gt; X</c> proxy.
    ///
    /// Why this replaces the proxy: the proxy ("producer cooled-down recently") was an indirect,
    /// CD-coupled approximation. A real window expresses intent directly (producer just fired →
    /// follow-up window open for a fixed duration), is canon-driven, and gives the A5 draft UI a
    /// clean static table of which skills chain.
    /// </summary>
    public class ChainWindowTracker : MonoBehaviour
    {
        // ─── Canon window ids (string consts, mirroring SkillStateTracker's const pattern) ───
        // Producer skill → opens → window id → read by consumer skill.
        public const string IronChargeNextHit  = "IronChargeNextHit";   // Iron Charge → Crippling Blow / Gravity Cleave
        public const string CripplingExecute    = "CripplingExecute";    // Crippling Blow → Death Blow
        public const string SunderExecute        = "SunderExecute";       // Death Blow → Sunder Mark
        public const string WarStompFollowup    = "WarStompFollowup";    // War Stomp → Ironclad Momentum
        public const string BladestormChain      = "BladestormChain";     // Burst/Bladestorm → Iron Crush (Phase 2 hook)

        /// <summary>Canon default follow-up window (s). Between the 0.8s Iron-Combo reset and the
        /// existing SkillFlowTracker 1.8s skill→skill window; inside the task's ~1.5–2.5s band.
        /// Used when a producer does not specify its own duration. NLM canon: combo resets after a
        /// 0.8s pause, skill→skill flow window is 1.8s — 1.5s sits between as the chain-beat window.</summary>
        public const float DefaultWindow = 1.5f;

        private readonly Dictionary<string, float> windows = new();

        /// <summary>Fired when a window newly opens. (id, duration). Additive hook for VFX/UI.</summary>
        public event Action<string, float> OnWindowOpened;

        /// <summary>Fired when an open window expires. (id).</summary>
        public event Action<string> OnWindowClosed;

        // ─── Public API ─────────────────────────────────────────────────────────

        /// <summary>Open (or refresh) a named chain window for <paramref name="duration"/> seconds.
        /// A producer skill calls this on cast.</summary>
        public void OpenWindow(string id, float duration = DefaultWindow)
        {
            if (string.IsNullOrWhiteSpace(id) || duration <= 0f) return;
            bool wasOpen = windows.TryGetValue(id, out float prev) && prev > 0f;
            windows[id] = Mathf.Max(wasOpen ? prev : 0f, duration);
            if (!wasOpen) OnWindowOpened?.Invoke(id, duration);
        }

        /// <summary>True while the named chain window is open. A consumer skill reads this to decide
        /// whether it is a CHAINED beat. (Read-only — does NOT consume; use <see cref="Consume"/>
        /// for one-shot chains.)</summary>
        public bool IsOpen(string id)
        {
            return windows.TryGetValue(id, out float t) && t > 0f;
        }

        /// <summary>Remaining seconds on a window (0 if closed). For tell/UI; not required for logic.</summary>
        public float Remaining(string id)
        {
            return windows.TryGetValue(id, out float t) ? Mathf.Max(0f, t) : 0f;
        }

        /// <summary>Atomic check-and-close: returns true once if the window is open, then closes it
        /// (so a single chain bonus can't fire twice off one producer cast).</summary>
        public bool Consume(string id)
        {
            if (!IsOpen(id)) return false;
            windows.Remove(id);
            OnWindowClosed?.Invoke(id);
            return true;
        }

        /// <summary>Force-close a window (e.g. on combo break). No-op if already closed.</summary>
        public void Close(string id)
        {
            if (windows.Remove(id)) OnWindowClosed?.Invoke(id);
        }

        // ─── Resolution helper (mirrors SkillRuntime.State lazy-attach pattern) ───

        /// <summary>Resolve the player-scoped tracker from any skill component, lazily attaching it
        /// to the player root if absent (so the prefab need not pre-place it — same contract as
        /// <see cref="SkillRuntime.State"/>). Returns null only if no PlayerController is found.</summary>
        public static ChainWindowTracker For(Component skill)
        {
            if (skill == null) return null;
            var existing = skill.GetComponentInParent<ChainWindowTracker>();
            if (existing != null) return existing;

            // Attach to the player root so GetComponentInParent resolves it like rage/flowTracker.
            // Fall back to the ENTITY ROOT (not the skill's own GameObject) so editor/prefab/non-player
            // contexts get one tracker per root instead of one per skill (cx A4 review fix).
            var player = skill.GetComponentInParent<PlayerController>();
            var host = player != null ? player.gameObject : skill.transform.root.gameObject;
            return host.AddComponent<ChainWindowTracker>();
        }

        // ─── Static chain-link table (A5 query API — no runtime state required) ───

        /// <summary>A5 query API: the canonical producer→consumer chain links, keyed by skillName.
        /// The draft chain-UI uses this to know "do skills X and Y chain?" WITHOUT touching runtime
        /// state. Each entry: consumer skillName → the producer skillName whose window it consumes.</summary>
        private static readonly Dictionary<string, string> ChainConsumerToProducer = new()
        {
            { "Crippling Blow", "Iron Charge"   },
            { "Gravity Cleave", "Iron Charge"   },
            { "Death Blow",     "Crippling Blow" },
            { "Sunder Mark",    "Death Blow"     },
            { "Ironclad Momentum", "War Stomp"  },
        };

        /// <summary>A5 query API: returns true if casting <paramref name="producerSkillName"/> then
        /// <paramref name="consumerSkillName"/> forms an intended chain (consumer reads the producer's
        /// window). Order matters (producer first). Static — safe to call from UI/editor.</summary>
        public static bool ChainsWith(string producerSkillName, string consumerSkillName)
        {
            return !string.IsNullOrEmpty(consumerSkillName)
                && ChainConsumerToProducer.TryGetValue(consumerSkillName, out string producer)
                && producer == producerSkillName;
        }

        /// <summary>A5 query API: the producer skillName a given skill chains FROM, or null if the
        /// skill is not a chain consumer. Lets the draft UI render a "⟂ pairs with {producer}" chip.</summary>
        public static string ProducerFor(string consumerSkillName)
        {
            return !string.IsNullOrEmpty(consumerSkillName)
                && ChainConsumerToProducer.TryGetValue(consumerSkillName, out string producer)
                ? producer
                : null;
        }

        // ─── Update ─────────────────────────────────────────────────────────────

        private void Update()
        {
            if (windows.Count == 0) return;

            List<string> closed = null;
            // Tick a snapshot of keys so we can mutate the dict safely.
            _tickKeys.Clear();
            _tickKeys.AddRange(windows.Keys);
            for (int i = 0; i < _tickKeys.Count; i++)
            {
                string key = _tickKeys[i];
                float t = windows[key] - Time.deltaTime;
                if (t <= 0f)
                {
                    windows.Remove(key);
                    closed ??= new List<string>();
                    closed.Add(key);
                }
                else
                {
                    windows[key] = t;
                }
            }

            if (closed == null) return;
            for (int i = 0; i < closed.Count; i++)
                OnWindowClosed?.Invoke(closed[i]);
        }

        private readonly List<string> _tickKeys = new();
    }
}
