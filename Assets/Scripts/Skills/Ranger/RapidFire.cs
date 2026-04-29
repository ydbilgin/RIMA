using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 9 — Rapid Fire
    /// 1.5s boyunca 6 ok atar (0.25s aralık). Her ok düşük hasar.
    /// Focus azaldıkça hasar düşer — uzak dur, çok kazan.
    /// </summary>
    public class RapidFire : SkillBase
    {
        [Header("Rapid Fire")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int   shotsPerBurst   = 6;
        [SerializeField] private int   baseDamage      = 22;
        [SerializeField] private float projectileSpeed = 18f;
        [SerializeField] private float interval        = 0.25f;
        [SerializeField] private float maxSpread       = 8f;   // degrees, random per shot

        protected override void Awake()
        {
            base.Awake();
            skillName    = "Rapid Fire";
            cooldown     = 14f;
            resourceCost = 35;
        }

        protected override void Execute() => StartCoroutine(BurstRoutine());

        private IEnumerator BurstRoutine()
        {
            if (projectilePrefab == null) yield break;

            Vector2 baseDir = player != null ? player.FacingDirection : Vector2.right;

            for (int i = 0; i < shotsPerBurst; i++)
            {
                float angle = Random.Range(-maxSpread, maxSpread);
                Vector2 dir = Rotate(baseDir, angle);

                // Focus-based damage: FocusSystem goes 0-100
                int dmg = baseDamage;
                if (resource is FocusSystem focus)
                    dmg = Mathf.RoundToInt(baseDamage * (focus.Current / focus.Max));

                var go   = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                var proj = go.GetComponent<PlayerProjectile>();
                proj?.Init(dir * projectileSpeed, Mathf.Max(8, dmg), life: 3f);

                yield return new WaitForSeconds(interval);
            }
        }

        private static Vector2 Rotate(Vector2 v, float deg)
        {
            float rad = deg * Mathf.Deg2Rad;
            float cos = Mathf.Cos(rad), sin = Mathf.Sin(rad);
            return new Vector2(v.x * cos - v.y * sin, v.x * sin + v.y * cos);
        }
    }
}
