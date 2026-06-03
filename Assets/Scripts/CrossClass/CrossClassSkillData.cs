using UnityEngine;

namespace RIMA
{
    public enum SourceClass
    {
        Warblade, Elementalist, Shadowblade, Ranger, Ravager,
        Ronin, Gunslinger, Brawler, Summoner, Hexer
    }

    public enum CrossClassEffectType
    {
        // Pasif tetikleyiciler
        OnHit_Stagger,          // vuruşta şans stagger
        OnDamageTaken_Resource, // hasar alınca resource+X
        OnSkillUse_Debuff,      // skill sonrası düşman debuff
        OnKill_Stealth,         // öldürünce görünmezlik
        OnDash_Buff,            // dash sonrası hasar bonus
        OnCrit_Bleed,           // crit bleed
        // Pasif stat
        Passive_DamageBoost,    // flat hasar artışı (koşullu)
        Passive_MaxHPBoost,     // max HP artışı
        Passive_CritChance,     // crit şansı +X
        Passive_DefenseBoost,   // belirli koşulda savunma
        // Aktif (CD tabanlı)
        Active_SmallAoE,        // küçük AoE hasar
        Active_CDReduce,        // tüm CD'leri azalt
        Active_Shield,          // kısa hasar engeli
        Active_Dash,            // geri çekilme + hasar
        // Diğer
        OnKill_ResourceBurst,   // öldürünce kaynak dolumu
        OnKill_SpeedBurst,      // öldürünce hız artışı
        DeathPrevention,        // ölümü bir kez engeller
    }

    /// <summary>
    /// Feature B (Shadow / Sundered Echo) archetype that drives the silhouette's spawn position.
    /// Derived from the guest SkillData's <see cref="SkillTag"/>s — NOT hardcoded per class
    /// (design spec: "read from the guest skill's own metadata"). Canon positioning:
    /// Melee → on enemy · Ranged → over player's shoulder · Zone → at cursor · SelfBuff → on player.
    /// </summary>
    public enum EchoArchetype { Melee, Ranged, Zone, SelfBuff }

    [CreateAssetMenu(menuName = "RIMA/CrossClassSkill", fileName = "CCS_New")]
    public class CrossClassSkillData : ScriptableObject
    {
        [Header("Identity")]
        public string skillName;
        [TextArea(2, 4)]
        public string description;
        public SourceClass sourceClass;
        public Sprite icon; // null OK — placeholder kullanılır

        [Header("Ghost VFX")]
        public Color ghostColor = Color.white;

        [Header("Effect")]
        public CrossClassEffectType effectType;
        public float primaryValue;   // ana değer (hasar, %, CD azalma vb.)
        public float secondaryValue; // ikincil değer (süre, şans, vb.)
        public float cooldown;       // 0 = pasif (CD yok)

        [Header("Condition")]
        [Tooltip("Bu skill'in tetiklenmesi için gereken koşul (opsiyonel açıklama)")]
        public string conditionNote;

        // ─────────────────────────────────────────────────────────────────────────
        // Feature B — Shadow Echo (transient actor that PERFORMS a guest skill).
        // These are the ONLY fields the echo path reads; the passive ApplyEffect stub
        // above is a separate (legacy) mechanic and is bypassed by CrossClassEcho.
        // ─────────────────────────────────────────────────────────────────────────
        [Header("Echo (Feature B — guest skill performed by a silhouette)")]
        [Tooltip("skillName of the guest SkillData this echo performs (resolved via SkillDatabase.FindByName). " +
                 "e.g. \"Fireball\", \"Cleave\". Empty = legacy passive-only entry, not echo-capable.")]
        public string guestSkillName;

        [Tooltip("Drives silhouette spawn position. Set from the guest skill's SkillTag when bound; " +
                 "Melee→on enemy, Ranged→over shoulder, Zone→cursor, SelfBuff→on player.")]
        public EchoArchetype archetype = EchoArchetype.Ranged;

        /// <summary>True when this entry can be performed as a Shadow Echo (has a guest skill ref).</summary>
        public bool IsEcho => !string.IsNullOrEmpty(guestSkillName);
    }
}
