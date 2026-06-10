using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 1 — Fireball ★
    /// Orta hasar + Burning DoT 4s. Fire State+1.
    /// 3 ard arda kullanılırsa 3.'de Living Bomb ücretsiz tetikler.
    /// Atış yönüne göre 8-yön sprite seçer (Resources/VFX/Fireball/).
    /// </summary>
    public class Fireball : SkillBase
    {
        [Header("Fireball")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 12f;
        [SerializeField] private int damage = 30;
        [SerializeField] private float burnDuration = 4f;

        // 8-dir sprites loaded lazily from Resources/VFX/Fireball/
        // Order matches FacingDir8: S, SE, E, NE, N, NW, W, SW
        private static readonly string[] DirNames = { "south", "south-east", "east", "north-east", "north", "north-west", "west", "south-west" };
        private static Sprite[] s_DirSprites; // lazy-loaded once per domain

        private Elementalist_SkillController ctrl;
        private int consecutiveCasts;
        private float consecutiveWindow = 3f;
        private float lastCastTime;

        // Echo (Feature B): Fireball is a clean ranged-projectile guest — fires from SkillOrigin
        // toward SkillAim, so a Shadow Echo can launch it over the player's shoulder at the target.
        public override bool SupportsEchoOrigin => true;

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
            Vector2 dir = SkillAim;
            Vector3 origin = SkillOrigin;
            var go = projectilePrefab != null
                ? Instantiate(projectilePrefab, origin, Quaternion.identity)
                : CreateRuntimeFireball(origin, dir);
            var proj = go.GetComponent<PlayerProjectile>();
            if (proj != null) proj.Init(dir * projectileSpeed, damage, applyBurning: true, burnDuration: burnDuration, life: 3f);
        }

        private GameObject CreateRuntimeFireball(Vector3 origin, Vector2 aimDir)
        {
            var go = new GameObject("Fireball_Runtime");
            go.transform.position = origin;

            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;

            var col = go.AddComponent<CircleCollider2D>();
            col.radius = 0.18f;
            col.isTrigger = true;

            var renderer = go.AddComponent<SpriteRenderer>();
            renderer.sortingLayerName = "VFX";
            renderer.sortingOrder = 20;

            // Try 8-dir sprite; fall back to runtime circle on failure
            Sprite dirSprite = GetDirSprite(aimDir);
            if (dirSprite != null)
            {
                renderer.sprite = dirSprite;
                renderer.color = Color.white;
                go.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            }
            else
            {
                renderer.sprite = ElementalistRuntimeVisuals.GetCircleSprite();
                renderer.color = new Color(1f, 0.42f, 0.12f, 0.95f);
                go.transform.localScale = new Vector3(0.45f, 0.45f, 1f);
            }

            go.AddComponent<PlayerProjectile>();
            return go;
        }

        /// <summary>
        /// Returns the directional sprite for the given aim vector.
        /// Sprites are loaded once from Resources/VFX/Fireball/ and cached.
        /// </summary>
        private static Sprite GetDirSprite(Vector2 dir)
        {
            if (s_DirSprites == null)
            {
                s_DirSprites = new Sprite[8];
                for (int i = 0; i < 8; i++)
                {
                    s_DirSprites[i] = Resources.Load<Sprite>($"VFX/Fireball/{DirNames[i]}");
                    if (s_DirSprites[i] == null)
                        Debug.LogWarning($"[Fireball] VFX sprite not found: VFX/Fireball/{DirNames[i]}");
                }
            }

            int idx = (int)HandAnchorAttach.VectorToDir8(dir);
            return (idx >= 0 && idx < 8) ? s_DirSprites[idx] : null;
        }
    }
}
