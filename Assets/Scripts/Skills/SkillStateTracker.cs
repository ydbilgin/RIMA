using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    public class SkillStateTracker : MonoBehaviour
    {
        public const string Sundered = "Sundered";
        public const string Broken = "Broken";

        /// <summary>Canon: Broken stacks at/above this threshold auto-escalate to Sundered
        /// (Broken cleared, Sundered applied). See A1b.</summary>
        public const int BrokenToSunderedThreshold = 3;

        /// <summary>Duration (s) of the Sundered state created by the Broken→Sundered
        /// auto-convert. Matches Sunder Mark's 8s window so the escalation feels equivalent.</summary>
        private const float AutoSunderDuration = 8f;

        public const string RangerMarked = "RangerMarked";
        public const string Trapped = "Trapped";
        public const string RiftScar = "RiftScar";
        public const string BackstabMarked = "BackstabMarked";
        public const string DeathMarked = "DeathMarked";
        public const string SmokeVeiled = "SmokeVeiled";

        private readonly Dictionary<string, SkillState> states = new();

        /// <summary>Fired when a state key newly becomes active (0→active) or its stack count
        /// increases. Args: (key, stacks). Additive — existing readers are unaffected.
        /// Used by BrokenStateVisual (A1) to drive the Broken/Sundered visual tell.</summary>
        public event Action<string, int> OnStateEntered;

        /// <summary>Fired when an active state key expires (duration ≤ 0) and is removed.</summary>
        public event Action<string> OnStateExpired;

        public void Apply(string key, float duration, int stacks = 1, int maxStacks = 99)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            bool isNew = !states.TryGetValue(key, out SkillState state);
            if (isNew)
            {
                state = new SkillState();
                states[key] = state;
            }

            int prevStacks = state.stacks;
            state.duration = Mathf.Max(state.duration, duration);
            state.stacks = Mathf.Clamp(state.stacks + Mathf.Max(1, stacks), 1, maxStacks);
            state.maxStacks = maxStacks;

            // A1: lazily attach the visual tell so Broken/Sundered states become visible, then
            // notify (it subscribes in OnEnable and reads current state on subscribe).
            if (key == Broken || key == Sundered)
                BrokenStateVisual.Ensure(gameObject);

            if (isNew || state.stacks > prevStacks)
                OnStateEntered?.Invoke(key, state.stacks);

            // A1b (canon): Broken at/over the threshold escalates to Sundered.
            // Clear Broken first, then apply Sundered — the recursive Apply fires its own
            // OnStateEntered("Sundered", ...) so the A1 tell upgrades red → orange-red.
            // key != Broken on the recursive call, so this branch does not re-enter.
            if (key == Broken && state.stacks >= BrokenToSunderedThreshold)
            {
                states.Remove(Broken);
                OnStateExpired?.Invoke(Broken);
                Apply(Sundered, AutoSunderDuration);
            }
        }

        public bool Has(string key)
        {
            return states.TryGetValue(key, out SkillState state) && state.duration > 0f && state.stacks > 0;
        }

        public int GetStacks(string key)
        {
            return Has(key) ? states[key].stacks : 0;
        }

        public bool Consume(string key, int amount = 1)
        {
            if (!states.TryGetValue(key, out SkillState state) || state.stacks < amount) return false;
            state.stacks -= amount;
            if (state.stacks <= 0) states.Remove(key);
            return true;
        }

        public void Remove(string key)
        {
            states.Remove(key);
        }

        private void Update()
        {
            if (states.Count == 0) return;

            List<string> expired = null;
            foreach (var pair in states)
            {
                pair.Value.duration -= Time.deltaTime;
                if (pair.Value.duration <= 0f)
                {
                    expired ??= new List<string>();
                    expired.Add(pair.Key);
                }
            }

            if (expired == null) return;
            for (int i = 0; i < expired.Count; i++)
            {
                states.Remove(expired[i]);
                OnStateExpired?.Invoke(expired[i]);
            }
        }

        private sealed class SkillState
        {
            public float duration;
            public int stacks;
            public int maxStacks;
        }
    }
}
