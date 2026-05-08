using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RIMA
{
    public enum StatusEffectType
    {
        // Ice chain: Chill x3 → Frozen → Shatter hit
        Chill,      // Slow 10% per stack (max 3) → auto-upgrade to Frozen
        Frozen,     // Full stop 2s, next hit = Ice Shatter (3x dmg + AOE)

        // Fire chain: Burning x3 → Scorch
        Burning,    // DoT 1/s per stack (max 3) → auto-upgrade to Scorch
        Scorch,     // Heavy DoT 3/s + -25% armor

        // Single-stack effects
        Poison,     // DoT 0.5/s, stacks to 8 (diminishing past 4)
        Shocked,    // -30% attack speed, next skill 0 rage cost
        Stunned,    // Full stop (enemy-only, short burst)
        Weakened,   // +25% incoming damage

        // RIMA-specific
        RiftMark,   // Void skill builds: 5 stacks = Void Burst (Warblade/Elementalist synergy)
        Exposed,    // Cross-class: CC bittikten sonra 3s Ranger hasar bonusu
        Bleed       // Warblade DeepWound DoT — CrossClassPassive_WB_Shadow tetikler
    }

    [Serializable]
    public class StatusEffectInstance
    {
        public StatusEffectType type;
        public float duration;
        public int stacks;
        public int maxStacks;
    }

    /// <summary>
    /// Attach to any entity (Player or Enemy) that can receive status effects.
    /// Handles stacking, duration ticks, ice/fire chains, and speed modifiers.
    /// </summary>
    public class StatusEffectSystem : MonoBehaviour
    {
        private readonly Dictionary<StatusEffectType, StatusEffectInstance> active
            = new Dictionary<StatusEffectType, StatusEffectInstance>();
        private readonly Dictionary<StatusEffectType, float> dotRemainders
            = new Dictionary<StatusEffectType, float>();
        // Reusable buffer — avoids per-frame GC allocation
        private readonly List<StatusEffectType> removeBuffer = new List<StatusEffectType>(8);

        /// <summary>1.0 = normal speed. Chill/Frozen reduces this.</summary>
        [HideInInspector] public float moveSpeedMultiplier = 1f;
        [HideInInspector] public float damageMultiplierIncoming = 1f;

        public UnityEvent<StatusEffectType, int> OnEffectApplied;   // type, stacks
        public UnityEvent<StatusEffectType> OnEffectRemoved;
        public UnityEvent OnIceShatter;   // hit a frozen target — callers deal 3x dmg
        public UnityEvent OnVoidBurst;    // rift mark maxed

        private Health health;

        private void Awake() => health = GetComponent<Health>();

        // ─── Public API ───────────────────────────────────────────────────────

        public void ApplyEffect(StatusEffectType type, float duration, int stacks = 1)
        {
            if (active.TryGetValue(type, out var existing))
            {
                existing.duration = Mathf.Max(existing.duration, duration);
                existing.stacks = Mathf.Min(existing.stacks + stacks, GetMaxStacks(type));
            }
            else
            {
                active[type] = new StatusEffectInstance
                {
                    type = type, duration = duration,
                    stacks = Mathf.Min(stacks, GetMaxStacks(type)),
                    maxStacks = GetMaxStacks(type)
                };
            }

            // Ice chain
            if (type == StatusEffectType.Chill && active[type].stacks >= 3)
            {
                RemoveEffect(StatusEffectType.Chill);
                ApplyEffect(StatusEffectType.Frozen, 2f, 1);
                return;
            }

            // Fire chain
            if (type == StatusEffectType.Burning && active[type].stacks >= 3)
            {
                RemoveEffect(StatusEffectType.Burning);
                ApplyEffect(StatusEffectType.Scorch, 3f, 1);
                return;
            }

            // Void burst
            if (type == StatusEffectType.RiftMark && active[type].stacks >= 5)
            {
                RemoveEffect(StatusEffectType.RiftMark);
                OnVoidBurst?.Invoke();
                return;
            }

            RecalcModifiers();
            OnEffectApplied?.Invoke(type, active[type].stacks);
        }

        /// <summary>Hit a frozen target. Returns true → caller applies 3x damage and AOE.</summary>
        public bool TryTriggerIceShatter()
        {
            if (!HasEffect(StatusEffectType.Frozen)) return false;
            RemoveEffect(StatusEffectType.Frozen);
            OnIceShatter?.Invoke();
            return true;
        }

        public void RemoveEffect(StatusEffectType type)
        {
            if (active.Remove(type))
            {
                dotRemainders.Remove(type);
                RecalcModifiers();
                OnEffectRemoved?.Invoke(type);
            }
        }

        public bool HasEffect(StatusEffectType type) => active.ContainsKey(type);
        public int GetStacks(StatusEffectType type) => active.TryGetValue(type, out var e) ? e.stacks : 0;
        public float GetDuration(StatusEffectType type) => active.TryGetValue(type, out var e) ? e.duration : 0f;
        public IReadOnlyDictionary<StatusEffectType, StatusEffectInstance> ActiveEffects => active;

        // ─── Tick ────────────────────────────────────────────────────────────

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        public void Tick(float deltaTime)
        {
            if (active.Count == 0) return;
            if (health == null) health = GetComponent<Health>();

            removeBuffer.Clear();

            foreach (var kvp in active)
            {
                kvp.Value.duration -= deltaTime;
                if (kvp.Value.duration <= 0f)
                {
                    removeBuffer.Add(kvp.Key);
                    continue;
                }

                // DoT ticks
                if (health != null)
                {
                    if (kvp.Key == StatusEffectType.Poison)
                    {
                        float rate = kvp.Value.stacks <= 4 ? kvp.Value.stacks * 0.5f : (4 * 0.5f + (kvp.Value.stacks - 4) * 0.15f);
                        ApplyDotDamage(kvp.Key, rate * deltaTime);
                    }
                    else if (kvp.Key == StatusEffectType.Burning)
                    {
                        ApplyDotDamage(kvp.Key, kvp.Value.stacks * 1f * deltaTime);
                    }
                    else if (kvp.Key == StatusEffectType.Scorch)
                    {
                        ApplyDotDamage(kvp.Key, 3f * deltaTime);
                    }
                }
            }

            for (int i = 0; i < removeBuffer.Count; i++)
                RemoveEffect(removeBuffer[i]);
        }

        // ─── Internal ────────────────────────────────────────────────────────

        private void RecalcModifiers()
        {
            float speed = 1f;
            if (HasEffect(StatusEffectType.Chill))
                speed -= 0.10f * GetStacks(StatusEffectType.Chill);
            if (HasEffect(StatusEffectType.Frozen) || HasEffect(StatusEffectType.Stunned))
                speed = 0f;
            moveSpeedMultiplier = Mathf.Max(0f, speed);

            float dmgMult = 1f;
            if (HasEffect(StatusEffectType.Weakened)) dmgMult += 0.25f;
            if (HasEffect(StatusEffectType.Scorch)) dmgMult += 0.25f;
            damageMultiplierIncoming = dmgMult;
        }

        private void ApplyDotDamage(StatusEffectType type, float damageAmount)
        {
            float total = dotRemainders.TryGetValue(type, out float remainder)
                ? remainder + damageAmount
                : damageAmount;

            int wholeDamage = Mathf.FloorToInt(total);
            dotRemainders[type] = total - wholeDamage;

            if (wholeDamage > 0)
                health.TakeDamage(wholeDamage);
        }

        private static int GetMaxStacks(StatusEffectType type) => type switch
        {
            StatusEffectType.Chill   => 3,
            StatusEffectType.Burning => 3,
            StatusEffectType.Poison  => 8,
            StatusEffectType.RiftMark => 5,
            _ => 1
        };
    }
}
