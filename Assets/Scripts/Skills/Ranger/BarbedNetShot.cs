using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 3 — Barbed Net Shot
    /// Ağ fırlatır: 2s root + 4s bleed %40/sn.
    /// Disengage sonrası → ağ 4m alana yayılır.
    /// </summary>
    public class BarbedNetShot : SkillBase
    {
        [Header("Barbed Net Shot")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 10f;
        [SerializeField] private int   damage          = 15;
        [SerializeField] private float rootDuration    = 2f;
        [SerializeField] private float bleedDuration   = 4f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Barbed Net Shot";
            cooldown = 10f;
            resourceCost = 20;
        }

        protected override void Execute()
        {
            if (projectilePrefab == null) return;
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            var go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var proj = go.GetComponent<PlayerProjectile>();
            proj?.Init(dir * projectileSpeed, damage, applyPoison: true, poisonDuration: bleedDuration);

            var hit = go.AddComponent<OnHitApplyEffect>();
            hit.Init(StatusEffectType.Stunned, rootDuration);
        }
    }
}
