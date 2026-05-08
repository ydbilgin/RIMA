using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Combat.Skills
{
    /// <summary>
    /// Skill draft system per SKILL_SYSTEM_TAXONOMY_2026-05-06.
    /// Hades-style: 3 random choices offered per room reward.
    /// Draft pools: active skills (Strike/Zone/Reactive/State) + passive upgrades.
    /// Soft-guidance: early rooms weight Strike, late rooms weight State+Reactive.
    /// </summary>
    public class SkillDraftSystem : MonoBehaviour
    {
        // ----------------------------------------------------------------
        // Soft-guidance weight table (room number -> dominant type)
        // Room 1-3: Strike-heavy | Room 4-8: balanced | Room 9+: State+Reactive
        // ----------------------------------------------------------------
        private static readonly (int maxRoom, ActiveSkillType dominant)[] WeightTable =
        {
            (3,  ActiveSkillType.Strike),
            (8,  ActiveSkillType.Zone),
            (int.MaxValue, ActiveSkillType.State),
        };

        [Header("Draft Pool")]
        [SerializeField] private List<ActiveSkillData> skillPool = new();

        [Header("Draft Settings")]
        [SerializeField] private int offersPerDraft = 3;

        // Singleton
        public static SkillDraftSystem Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
        }

public event Action<List<ActiveSkillData>> OnDraftOffered;
        public event Action<ActiveSkillData> OnSkillSelected;

        private readonly System.Random rng = new();

        // ----------------------------------------------------------------
        // Public API
        // ----------------------------------------------------------------

        /// <summary>
        /// Call after a room is cleared. roomNumber is 1-indexed.
        /// Fires OnDraftOffered with 3 choices (or fewer if pool is small).
        /// </summary>
        public void TriggerDraft(int roomNumber)
        {
            if (skillPool == null || skillPool.Count == 0)
            {
                Debug.LogWarning("[SkillDraftSystem] Skill pool is empty.");
                return;
            }

            var offers = BuildOffers(roomNumber);
            OnDraftOffered?.Invoke(offers);
        }

        /// <summary>
        /// Player selected a skill from the draft offer list.
        /// Call from UI when player picks a card.
        /// </summary>
        public void SelectSkill(ActiveSkillData chosen)
        {
            if (chosen == null) return;
            OnSkillSelected?.Invoke(chosen);
        }

        // ----------------------------------------------------------------
        // Internal
        // ----------------------------------------------------------------

        private List<ActiveSkillData> BuildOffers(int roomNumber)
        {
            ActiveSkillType dominant = GetDominantType(roomNumber);

            // Partition pool: dominant type first, rest second
            var dominated = new List<ActiveSkillData>();
            var others    = new List<ActiveSkillData>();

            foreach (var skill in skillPool)
            {
                if (skill == null) continue;
                if (skill.skillType == dominant)
                    dominated.Add(skill);
                else
                    others.Add(skill);
            }

            Shuffle(dominated);
            Shuffle(others);

            // Build offer list: fill with dominant first, pad with others
            var offers = new List<ActiveSkillData>();
            foreach (var s in dominated)
            {
                if (offers.Count >= offersPerDraft) break;
                offers.Add(s);
            }
            foreach (var s in others)
            {
                if (offers.Count >= offersPerDraft) break;
                offers.Add(s);
            }

            return offers;
        }

        private static ActiveSkillType GetDominantType(int roomNumber)
        {
            foreach (var (maxRoom, dominant) in WeightTable)
                if (roomNumber <= maxRoom)
                    return dominant;
            return ActiveSkillType.State;
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
