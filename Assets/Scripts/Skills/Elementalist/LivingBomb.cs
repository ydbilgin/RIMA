using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 3 — Living Bomb
    /// 5s sonra patlama, öldürünce 3 komşuya kopyalanır.
    /// Glacial Spike slow altında → patlama yarıçapı 2×.
    /// </summary>
    public class LivingBomb : SkillBase
    {
        [Header("Living Bomb")]
        [SerializeField] private float fuseTime   = 5f;
        [SerializeField] private float baseRadius = 2.5f;
        [SerializeField] private int   damage     = 60;
        private Elementalist_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Living Bomb";
            cooldown = 8f;
            resourceCost = 20;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute()
        {
            var target = FindNearestEnemy();
            if (target == null) return;
            ctrl?.RegisterElementCast(ElementalistElement.Fire, 1);
            StartCoroutine(BombRoutine(target));
        }

        private IEnumerator BombRoutine(GameObject target)
        {
            yield return new WaitForSeconds(fuseTime);

            if (target == null) yield break;

            float radius = baseRadius;
            var status = target.GetComponent<StatusEffectSystem>();
            if (status != null && status.GetStacks(StatusEffectType.Chill) > 0)
                radius *= 2f;

            Explode(target.transform.position, radius, target);
        }

        private void Explode(Vector2 center, float radius, GameObject origin = null)
        {
            var hits = Physics2D.OverlapCircleAll(center, radius);
            bool killedSomeone = false;

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player")) continue;
                var hp = hit.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                bool wasDead = hp.IsDead;
                SkillRuntime.DealDamage(hp, damage, this);
                if (!wasDead && hp.IsDead) killedSomeone = true;
            }

            if (killedSomeone && origin != null)
            {
                // Patlama sonrası 3 komşuya kopyala
                int chained = 0;
                var nearby = Physics2D.OverlapCircleAll(center, radius * 2f);
                foreach (var hit in nearby)
                {
                    if (chained >= 3) break;
                    if (hit.CompareTag("Player")) continue;
                    var hp = hit.GetComponent<Health>();
                    if (hp == null || hp.IsDead) continue;
                    StartCoroutine(BombRoutine(hit.gameObject));
                    chained++;
                }
            }
        }

        private GameObject FindNearestEnemy()
        {
            float minD = float.MaxValue;
            GameObject result = null;
            foreach (var e in FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
            {
                float d = Vector2.Distance(transform.position, e.transform.position);
                if (d < minD) { minD = d; result = e.gameObject; }
            }
            return result;
        }
    }
}
