using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 5 — Multi Shot
    /// Aynı anda 3 ok: merkez + sol 20° + sağ 20°.
    /// Focus yüksekse (+60) orta ok ek hasar.
    /// </summary>
    public class MultiShot : SkillBase
    {
        [Header("Multi Shot")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed  = 16f;
        [SerializeField] private int   baseDamage       = 35;
        [SerializeField] private int   centerBonusDmg   = 20;   // Focus >60 ile aktif
        [SerializeField] private float spreadAngle      = 20f;

        protected override void Awake()
        {
            base.Awake();
            skillName    = "Multi Shot";
            cooldown     = 6f;
            resourceCost = 25;
        }

        protected override void Execute()
        {
            if (projectilePrefab == null) return;

            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;

            bool highFocus = resource != null && resource.Current > 60f;
            int centerDmg  = baseDamage + (highFocus ? centerBonusDmg : 0);

            Fire(dir, centerDmg);
            Fire(Rotate(dir,  spreadAngle), baseDamage);
            Fire(Rotate(dir, -spreadAngle), baseDamage);
        }

        private void Fire(Vector2 dir, int dmg)
        {
            var go   = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var proj = go.GetComponent<PlayerProjectile>();
            proj?.Init(dir * projectileSpeed, dmg, life: 4f);
        }

        private static Vector2 Rotate(Vector2 v, float deg)
        {
            float rad = deg * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
            return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }
    }
}
