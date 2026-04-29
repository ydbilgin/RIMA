using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 6 — Arcane Blast
    /// Her cast +%20 hasar, +%30 mana maliyet. 4. cast Barrage açar.
    /// </summary>
    public class ArcaneBlast : SkillBase
    {
        [Header("Arcane Blast")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 14f;
        [SerializeField] private int   baseDamage     = 35;
        [SerializeField] private int   baseCost       = 20;
        [SerializeField] private int   barrageCount   = 5;

        private int castCount;
        private float resetWindow = 4f;
        private float lastCastTime;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Arcane Blast";
            cooldown = 0.8f;
            resourceCost = baseCost;
        }

        protected override void Execute()
        {
            if (Time.time - lastCastTime > resetWindow) castCount = 0;
            castCount++;
            lastCastTime = Time.time;

            int dmg  = Mathf.RoundToInt(baseDamage * Mathf.Pow(1.2f, castCount - 1));
            resourceCost = Mathf.RoundToInt(baseCost * Mathf.Pow(1.3f, castCount - 1));

            if (castCount >= 4)
            {
                castCount = 0;
                resourceCost = baseCost;
                FireBarrage(dmg);
                return;
            }

            FireProjectile(player != null ? player.FacingDirection : Vector2.right, dmg);
        }

        private void FireProjectile(Vector2 dir, int dmg)
        {
            if (projectilePrefab == null) return;
            var go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var proj = go.GetComponent<PlayerProjectile>();
            proj?.Init(dir * projectileSpeed, dmg, life: 3f);
        }

        private void FireBarrage(int dmg)
        {
            for (int i = 0; i < barrageCount; i++)
            {
                float angle = -30f + (60f / (barrageCount - 1)) * i;
                Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
                Vector2 rotDir = Quaternion.Euler(0, 0, angle) * dir;
                FireProjectile(rotDir, dmg);
            }
        }
    }
}
