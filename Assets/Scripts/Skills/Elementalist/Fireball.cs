using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 1 — Fireball ★
    /// Orta hasar + Burning DoT 4s. Fire State+1.
    /// 3 ard arda kullanılırsa 3.'de Living Bomb ücretsiz tetikler.
    /// </summary>
    public class Fireball : SkillBase
    {
        [Header("Fireball")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 12f;
        [SerializeField] private int damage = 30;
        [SerializeField] private float burnDuration = 4f;

        private Elementalist_SkillController ctrl;
        private int consecutiveCasts;
        private float consecutiveWindow = 3f;
        private float lastCastTime;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Fireball";
            cooldown = 1.2f;
            resourceCost = 15;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute()
        {
            if (Time.time - lastCastTime > consecutiveWindow) consecutiveCasts = 0;
            consecutiveCasts++;
            lastCastTime = Time.time;

            FireProjectile();
            ctrl?.RegisterElementCast(ElementalistElement.Fire, 1);

            if (consecutiveCasts >= 3)
            {
                consecutiveCasts = 0;
                // Auto-trigger LivingBomb on nearest enemy
                var lb = GetComponentInParent<LivingBomb>();
                if (lb != null) lb.TryActivate();
            }
        }

        private void FireProjectile()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            var go = projectilePrefab != null
                ? Instantiate(projectilePrefab, transform.position, Quaternion.identity)
                : CreateRuntimeFireball();
            var proj = go.GetComponent<PlayerProjectile>();
            if (proj != null) proj.Init(dir * projectileSpeed, damage, applyBurning: true, burnDuration: burnDuration, life: 3f);
        }

        private GameObject CreateRuntimeFireball()
        {
            var go = new GameObject("Fireball_Runtime");
            go.transform.position = transform.position;

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.18f;
            col.isTrigger = true;

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
            renderer.color = new Color(1f, 0.42f, 0.12f, 0.95f);
            renderer.sortingLayerName = "VFX";
            renderer.sortingOrder = 20;

            go.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
            go.AddComponent<PlayerProjectile>();
            return go;
        }
    }
}
