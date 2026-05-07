using UnityEngine;

namespace RIMA.Combat.Skills
{
    /// <summary>
    /// Active skill type per SKILL_SYSTEM_TAXONOMY.
    /// 4 active types only. Passive types (Keystone, Modifier, Resonance)
    /// are handled by a separate system.
    /// </summary>
    public enum ActiveSkillType
    {
        Strike,
        Zone,
        Reactive,
        State
    }

    /// <summary>
    /// Data definition for an active equip-slot skill.
    /// Renamed from SkillData to avoid ambiguity with RIMA.SkillData (legacy skill SO).
    /// </summary>
    [CreateAssetMenu(fileName = "ActiveSkillData", menuName = "RIMA/Combat/Active Skill Data")]
    public class ActiveSkillData : ScriptableObject
    {
        public ActiveSkillType skillType;
        public float cooldown;
        public string skillName;
        public Sprite icon;
    }
}
