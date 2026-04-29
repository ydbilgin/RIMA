using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Ranger Skill 10 — Tethering Arrow
    /// İlk çarpılan düşmanı 2.5s süreyle durdurur (Stunned).
    /// Süre dolunca ya da hedef ölünce yay kopar.
    /// </summary>
    public class TetheringArrow : SkillBase
    {
        [Header("Tethering Arrow")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 17f;
        [SerializeField] private int   damage         = 30;
        [SerializeField] private float stunDuration   = 2.5f;

        protected override void Awake()
        {
            base.Awake();
            skillName    = "Tethering Arrow";
            cooldown     = 12f;
            resourceCost = 25;
        }

        protected override void Execute()
        {
            if (projectilePrefab == null) return;
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            var go   = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            var proj = go.GetComponent<PlayerProjectile>();
            proj?.Init(dir * projectileSpeed, damage, life: 4f);
            go.AddComponent<TetherEffect>().Init(stunDuration);
        }
    }

    internal class TetherEffect : MonoBehaviour
    {
        private float stunDuration;
        private bool applied;

        public void Init(float dur) => stunDuration = dur;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (applied) return;
            if (other.CompareTag("Player")) return;
            var status = other.GetComponent<StatusEffectSystem>();
            if (status == null) return;
            applied = true;
            status.ApplyEffect(StatusEffectType.Stunned, stunDuration);
        }
    }
}
