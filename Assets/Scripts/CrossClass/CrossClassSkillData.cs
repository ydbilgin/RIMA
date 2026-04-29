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
    }
}
