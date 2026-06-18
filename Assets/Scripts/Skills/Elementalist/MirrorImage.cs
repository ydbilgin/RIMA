using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 8 — Mirror Image
    /// 2 kopya 8s, hasar önce kopyaya gelir.
    /// Kopyalar ölünce → AoE ölüm patlaması.
    /// </summary>
    public class MirrorImage : SkillBase
    {
        [Header("Mirror Image")]
        [SerializeField] private GameObject clonePrefab;
        [SerializeField] private int   cloneCount    = 2;
        [SerializeField] private float cloneDuration = 8f;
        [SerializeField] private int   cloneHealth   = 40;
        [SerializeField] private float deathExplosionRadius = 2f;
        [SerializeField] private int   deathExplosionDamage = 30;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Mirror Image";
            cooldown = 16f;
            resourceCost = 50;
        }

        protected override void Execute()
        {
            if (clonePrefab == null) return;
            SkillVfx.CastFlash(player != null ? player.gameObject : gameObject, VfxElement.Arcane);
            for (int i = 0; i < cloneCount; i++)
            {
                Vector2 offset = Random.insideUnitCircle.normalized * 1.2f;
                Vector3 spawnPos = transform.position + (Vector3)offset;
                var go = Instantiate(clonePrefab, spawnPos, Quaternion.identity);
                var mc = go.GetComponent<MirrorClone>();
                mc?.Init(cloneHealth, cloneDuration, deathExplosionRadius, deathExplosionDamage);
                // Arcane pop as each mirror image materializes.
                SkillVfx.ImpactBurst(spawnPos, VfxElement.Arcane);
            }
        }
    }

    /// <summary> Mirror Image clone bileşeni — clonePrefab'a eklenir. </summary>
    public class MirrorClone : MonoBehaviour
    {
        private int explosionRadius;
        private int explosionDamage;

        public void Init(int hp, float duration, float expRadius, int expDamage)
        {
            explosionRadius = Mathf.RoundToInt(expRadius);
            explosionDamage = expDamage;

            var health = GetComponent<Health>();
            if (health != null)
            {
                health.SetMaxHP(hp);
                health.OnDeath.AddListener(OnCloneDeath);
            }

            Destroy(gameObject, duration);
        }

        private void OnCloneDeath()
        {
            SkillVfx.ImpactBurst(transform.position, VfxElement.Arcane);
            var hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (var h in hits)
            {
                if (h.CompareTag("Player")) continue;
                var hp = h.GetComponent<Health>();
                SkillRuntime.DealDamage(hp, explosionDamage, this);
            }
            Destroy(gameObject);
        }
    }
}
