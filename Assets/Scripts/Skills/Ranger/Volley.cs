using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 8 — Volley
    /// Hedef noktaya 5 ok yağdırır (küçük spread, 0.15s aralık).
    /// Her ok Chill uygular. Kullanışlı: AoE + sürü kontrolü.
    /// </summary>
    public class Volley : SkillBase
    {
        [Header("Volley")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int   arrowCount     = 5;
        [SerializeField] private int   arrowDamage    = 28;
        [SerializeField] private float spreadRadius   = 1.2f;
        [SerializeField] private float projectileSpeed = 15f;
        [SerializeField] private float interval       = 0.15f;
        [SerializeField] private float chillDuration  = 2f;
        [SerializeField] private float range          = 8f;

        protected override void Awake()
        {
            base.Awake();
            skillName    = "Volley";
            cooldown     = 15f;
            resourceCost = 40;
        }

        protected override void Execute() => StartCoroutine(VolleyRoutine());

        private IEnumerator VolleyRoutine()
        {
            if (projectilePrefab == null) yield break;

            Vector2 origin = transform.position;
            Vector2 dir    = player != null ? player.FacingDirection : Vector2.right;
            Vector2 target = origin + dir * range;

            for (int i = 0; i < arrowCount; i++)
            {
                Vector2 offset  = Random.insideUnitCircle * spreadRadius;
                Vector2 impactPos = target + offset;
                Vector2 fireDir   = (impactPos - origin).normalized;

                var go   = Instantiate(projectilePrefab, origin, Quaternion.identity);
                var proj = go.GetComponent<PlayerProjectile>();
                proj?.Init(fireDir * projectileSpeed, arrowDamage, life: 3f,
                           applyChill: true, chillDuration: chillDuration);

                yield return new WaitForSeconds(interval);
            }
        }
    }
}
