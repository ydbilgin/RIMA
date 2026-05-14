using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Data
{
    [CreateAssetMenu(menuName = "RIMA/Character Class Definition", fileName = "Class_New")]
    public class CharacterClassDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string className = "Warblade";
        public string anchorName = "warblade";
        [TextArea] public string roleDescription = "Two-handed greatsword fighter, mid-range melee";

        [Header("Visual")]
        public Sprite idleSprite;
        public Sprite weaponSprite;
        public Vector2Int weaponCanvas = new Vector2Int(56, 20);

        [Header("Weapon Decouple (Karar #123)")]
        public bool weaponDecoupled = true;

        [Header("Stats Placeholder")]
        public float maxHp = 100f;
        public float moveSpeed = 5f;
        public float baseAttackDamage = 10f;
        public float baseAttackCooldown = 0.6f;

        [Header("Echo Skill Tier 1 (Karar #5/#7)")]
        public string echoTier1Name;
        public string echoTier1Description;

        [Header("Identity Body Accessories (passive, Karar #18)")]
        public List<string> passiveAccessories = new List<string>();
    }
}
