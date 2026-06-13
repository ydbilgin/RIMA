using UnityEngine;

namespace RIMA
{
    public enum SkillTier { Common, Rare, Epic, Mythic, Legendary }

    public enum SkillTag
    {
        Melee, Ranged, Dash, AOE,
        Fire, Ice, Lightning, Void, Poison,
        Physical, Summon, Trap,
        Passive  // passive skill: slot almaz, her zaman aktif
    }

    public enum ClassType
    {
        None,
        Warblade,
        Elementalist,
        Shadowblade,
        Ranger,
        Ravager,
        Ronin,
        Gunslinger,
        Brawler,
        Summoner,
        Hexer
    }

    [CreateAssetMenu(fileName = "SkillData", menuName = "RIMA/Skill Data")]
    public class SkillData : ScriptableObject
    {
        public string skillName;
        [TextArea] public string description;
        public SkillTier tier;
        public Sprite icon;
        public int damage;
        public float cooldown;
        public SkillTag[] tags;

        [Header("Class")]
        public ClassType classType = ClassType.None;

        [Header("Passive")]
        public bool isPassive;
        [TextArea] public string passiveDescription; // tooltip'te gösterilir
        public StatusEffectType appliesEffect;       // bu skill hangi efekti uygular

        [Header("Implementation Status")]
        public bool isImplemented = true;

        /// <summary>
        /// Runtime'da SkillDatabase tarafından set edilir.
        /// Hangi SkillBase MonoBehaviour'ına karşılık geldiğini tutar.
        /// Serializelanmaz — ScriptableObject asset'te kaybolur.
        /// </summary>
        [System.NonSerialized] public System.Type skillType;
    }
}
