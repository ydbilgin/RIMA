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
            SkillVfx.CastFlash(player != null ? player.gameObject : gameObject, VfxElement.Arcane);

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
            // Mirror Fireball: when no authored prefab is assigned, build an arcane-tinted runtime
            // projectile so the skill still fires + deals damage instead of silently no-op'ing.
            var go = projectilePrefab != null
                ? Instantiate(projectilePrefab, transform.position, Quaternion.identity)
                : CreateRuntimeProjectile();
            var proj = go.GetComponent<PlayerProjectile>();
            SkillVfx.ProjectileTrail(go, VfxElement.Arcane);
            if (proj != null)
            {
                proj.Init(dir * projectileSpeed, dmg, life: 3f,
                    attacker: player != null ? player.gameObject : null, element: "arcane");
                proj.SetOnHit(hit => SkillVfx.ImpactBurst(hit != null ? hit.transform.position : go.transform.position, VfxElement.Arcane));
            }
        }

        private GameObject CreateRuntimeProjectile()
        {
            var go = new GameObject("ArcaneBlast_Runtime");
            go.transform.position = transform.position;
            go.transform.localScale = new Vector3(0.42f, 0.42f, 1f);

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.18f;
            col.isTrigger = true;

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
            renderer.color = new Color(0.62f, 0.38f, 1f, 0.95f); // arcane violet
            renderer.sortingLayerName = "VFX";
            renderer.sortingOrder = 20;

            go.AddComponent<PlayerProjectile>();
            return go;
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
