using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 1 — Aimed Shot ★
    /// 1.5s şarj → büyük hasar + %50 crit.
    /// Root sonrası → guaranteed instant.
    /// </summary>
    public class AimedShot : SkillBase
    {
        [Header("Aimed Shot")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 18f;
        [SerializeField] private int   damage          = 80;
        [SerializeField] private float chargeTime      = 1.5f;
        [SerializeField] private float critMultiplier  = 1.5f;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Aimed Shot";
            cooldown = 8f;
            resourceCost = 20;
        }

        protected override void Execute() => StartCoroutine(AimRoutine());

        private IEnumerator AimRoutine()
        {
            yield return new WaitForSeconds(chargeTime);
            Fire();
        }

        private void Fire()
        {
            if (projectilePrefab == null) return;
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            var go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var proj = go.GetComponent<PlayerProjectile>();
            proj?.Init(dir * projectileSpeed, Mathf.RoundToInt(damage * critMultiplier), life: 5f);
        }
    }
}
