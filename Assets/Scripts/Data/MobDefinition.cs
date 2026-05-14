using UnityEngine;

namespace RIMA.Data
{
    public enum MobRole
    {
        Swarm,
        Melee,
        Ranged,
        Caster,
        Elite,
        MiniBoss,
        Support,
        Pack
    }

    [CreateAssetMenu(menuName = "RIMA/Mob Definition", fileName = "Mob_New")]
    public class MobDefinition : ScriptableObject
    {
        [Header("Identity")]
        public string mobName = "Seam Crawler";
        public MobRole role = MobRole.Swarm;
        [TextArea] public string silhouette;
        public string biomeKey = "F1";

        [Header("Visual")]
        public Sprite idleSprite;
        public Vector2Int canvasSize = new Vector2Int(64, 64);
        public string riftPaletteAccent = "cyan #00FFCC + violet #5A2A8A";

        [Header("Stats Placeholder")]
        public float maxHp = 30f;
        public float moveSpeed = 3.5f;
        public float damage = 8f;
        public float attackRange = 1.2f;
        public float detectionRange = 6f;

        [Header("Behavior")]
        public bool isFlying = false;
        public bool isElite = false;
        public bool hasIntegratedWeapon = false;
    }
}
