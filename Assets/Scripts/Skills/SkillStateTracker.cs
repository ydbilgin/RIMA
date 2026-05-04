using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    public class SkillStateTracker : MonoBehaviour
    {
        public const string Sundered = "Sundered";
        public const string Broken = "Broken";
        public const string RangerMarked = "RangerMarked";
        public const string Trapped = "Trapped";
        public const string RiftScar = "RiftScar";
        public const string BackstabMarked = "BackstabMarked";
        public const string DeathMarked = "DeathMarked";
        public const string SmokeVeiled = "SmokeVeiled";

        private readonly Dictionary<string, SkillState> states = new();

        public void Apply(string key, float duration, int stacks = 1, int maxStacks = 99)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            if (!states.TryGetValue(key, out SkillState state))
            {
                state = new SkillState();
                states[key] = state;
            }

            state.duration = Mathf.Max(state.duration, duration);
            state.stacks = Mathf.Clamp(state.stacks + Mathf.Max(1, stacks), 1, maxStacks);
            state.maxStacks = maxStacks;
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
                states.Remove(expired[i]);
        }

        private sealed class SkillState
        {
            public float duration;
            public int stacks;
            public int maxStacks;
        }
    }
}
