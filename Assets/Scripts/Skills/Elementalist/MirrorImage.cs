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
            for (int i = 0; i < cloneCount; i++)
            {
                Vector2 offset = Random.insideUnitCircle.normalized * 1.2f;
                var go = Instantiate(clonePrefab, transform.position + (Vector3)offset, Quaternion.identity);
                var mc = go.GetComponent<MirrorClone>();
                mc?.Init(cloneHealth, cloneDuration, deathExplosionRadius, deathExplosionDamage);
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
