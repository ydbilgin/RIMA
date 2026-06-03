using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Feature B (B5) — the curated, DATA-DRIVEN set of offerable cross-class Echoes.
    ///
    /// Each entry maps a <see cref="SourceClass"/> guest to ONE signature guest skill (by skillName,
    /// resolved through SkillDatabase by <see cref="PlayerCrossClassBinding.Bind"/>). The draft offers an
    /// "Echo of {Class}" card built from one of these; picking it binds the guest skill to key C.
    ///
    /// Built in code (not .asset files) so the curated list lives in one editable place and needs no
    /// scene/GUID wiring. To add a meta-unlockable Echo later, append an Entry here (or load real
    /// CrossClassSkillData assets and merge) — nothing else changes. The archetype field is a hint only;
    /// <see cref="PlayerCrossClassBinding.Bind"/> re-stamps it from the guest skill's own SkillTags.
    ///
    /// DEMO GUESTS: only guests whose guest skill is registered in SkillDatabase AND reports
    /// SupportsEchoOrigin = true are listed, so each offered Echo works end-to-end via SkillBase.ExecuteAt.
    /// </summary>
    public static class CrossClassCatalog
    {
        /// <summary>A curated Echo recipe. Pure data — instantiated into a runtime
        /// <see cref="CrossClassSkillData"/> on demand.</summary>
        public readonly struct Entry
        {
            public readonly SourceClass guestClass;
            public readonly string guestSkillName;   // must match a SkillData.skillName in SkillDatabase
            public readonly EchoArchetype archetypeHint;
            public readonly string description;

            public Entry(SourceClass guestClass, string guestSkillName,
                         EchoArchetype archetypeHint, string description)
            {
                this.guestClass = guestClass;
                this.guestSkillName = guestSkillName;
                this.archetypeHint = archetypeHint;
                this.description = description;
            }
        }

        // ── Curated demo set ──────────────────────────────────────────────────────
        // Fireball (Elementalist, ranged) + Cleave (Warblade, melee) are both registered in
        // SkillDatabase and migrated (SupportsEchoOrigin). Earthsplitter is the ranged-AoE option.
        // War Stomp is intentionally omitted: not registered in SkillDatabase, so Bind() would abort.
        private static readonly Entry[] Curated =
        {
            new Entry(SourceClass.Elementalist, "Fireball", EchoArchetype.Ranged,
                "Cagir bir Elementalist yankisi (C): imlece dogru ates topu firlatir."),
            new Entry(SourceClass.Warblade, "Cleave", EchoArchetype.Melee,
                "Cagir bir Warblade yankisi (C): dusmana dalip yarici darbe vurur."),
            new Entry(SourceClass.Warblade, "Earthsplitter", EchoArchetype.Zone,
                "Cagir bir Warblade yankisi (C): yeri yarip alan hasari + Kirik uygular."),
        };

        /// <summary>All curated entries (read-only).</summary>
        public static IReadOnlyList<Entry> All => Curated;

        /// <summary>
        /// Pick a curated Echo to offer for the given primary class, EXCLUDING any guest already bound.
        /// Prefers a guest of a DIFFERENT class than the player's primary (cross-class flavor), but never
        /// returns nothing offerable if only same-class guests remain. Returns a freshly-instantiated,
        /// runtime <see cref="CrossClassSkillData"/> ready for <see cref="RewardOffer.FromEcho"/>, or null
        /// if there is nothing valid to offer.
        /// </summary>
        public static CrossClassSkillData PickOffer(ClassType primary, string alreadyBoundGuestSkill)
        {
            var candidates = new List<Entry>(Curated.Length);
            var sameClassFallback = new List<Entry>(Curated.Length);

            for (int i = 0; i < Curated.Length; i++)
            {
                var e = Curated[i];
                if (!string.IsNullOrEmpty(alreadyBoundGuestSkill) && e.guestSkillName == alreadyBoundGuestSkill)
                    continue; // do not re-offer the bound guest

                // Prefer guests from a class other than the player's primary (it's "cross"-class).
                if (GuestToClassType(e.guestClass) != primary) candidates.Add(e);
                else sameClassFallback.Add(e);
            }

            if (candidates.Count == 0) candidates = sameClassFallback;
            if (candidates.Count == 0) return null;

            var chosen = candidates[Random.Range(0, candidates.Count)];
            return Build(chosen);
        }

        /// <summary>Instantiate a runtime <see cref="CrossClassSkillData"/> from a curated entry.</summary>
        public static CrossClassSkillData Build(Entry e)
        {
            var data = ScriptableObject.CreateInstance<CrossClassSkillData>();
            data.name = $"CCS_Echo_{e.guestClass}_{e.guestSkillName}";
            data.skillName = $"Echo of {e.guestClass}";
            data.description = e.description;
            data.sourceClass = e.guestClass;
            data.guestSkillName = e.guestSkillName;
            data.archetype = e.archetypeHint;     // Bind() re-stamps from the guest skill's tags
            data.ghostColor = new Color(0f, 1f, 0.8f, 1f); // cyan echo/seal identity
            return data;
        }

        /// <summary>Best-effort map a guest <see cref="SourceClass"/> to the gameplay <see cref="ClassType"/>
        /// (names align for the demo classes); falls back to None when no match exists.</summary>
        private static ClassType GuestToClassType(SourceClass cls)
            => System.Enum.TryParse(cls.ToString(), out ClassType ct) ? ct : ClassType.None;
    }
}
