using UnityEngine;
using System.Collections.Generic;

namespace RIMA
{
    /// <summary>
    /// Elementalist Skill 9 — Chain Lightning
    /// 5 hedefe sekiyor (yavaşlamış hedef → 7 seki).
    /// </summary>
    public class ChainLightning : SkillBase
    {
        [Header("Chain Lightning")]
        [SerializeField] private int   damage    = 40;
        [SerializeField] private int   baseChain = 5;
        [SerializeField] private float jumpRange = 6f;
        [SerializeField] private float shockedDuration = 2.25f;

        private Elementalist_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Chain Lightning";
            cooldown = 7f;
            resourceCost = 25;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute()
        {
            var first = FindNearestEnemy(transform.position, null);
            if (first == null) return;

            var hit = new HashSet<Collider2D>();
            Collider2D current = first;

            var status = first.GetComponent<StatusEffectSystem>();
            int maxChain = (status != null && status.GetStacks(StatusEffectType.Chill) > 0) ? 7 : baseChain;
            Vector2 previousPoint = transform.position;

            for (int i = 0; i < maxChain; i++)
            {
                if (current == null) break;
                var hp = current.GetComponent<Health>();
                if (hp != null && !hp.IsDead)
                {
                    int finalDamage = Mathf.Max(8, Mathf.RoundToInt(damage * Mathf.Pow(0.86f, i)));
                    SkillRuntime.DealDamage(hp, finalDamage, this);
                    current.GetComponent<StatusEffectSystem>()?.ApplyEffect(StatusEffectType.Shocked, shockedDuration);
                    SkillRuntime.SpawnCircleVisual(current.transform.position, new Color(0.98f, 0.92f, 0.26f, 0.58f), 0.58f, 0.16f, "ChainLightning_Hit");
                    SpawnArcVisual(previousPoint, current.transform.position);
                    previousPoint = current.transform.position;
                }
                hit.Add(current);

                current = FindNearestEnemy(current.transform.position, hit);
            }

            ctrl?.RegisterElementCast(ElementalistElement.Light, 1);
            LightPulse.Emit(new Color(1f, 0.88f, 0.24f), 0.8f, 0.06f);
        }

        private void SpawnArcVisual(Vector2 from, Vector2 to)
        {
            var go = new GameObject("ChainLightning_Arc");
            var line = go.AddComponent<LineRenderer>();
            line.positionCount = 3;
            line.useWorldSpace = true;
            Vector2 mid = (from + to) * 0.5f;
            Vector2 normal = Vector2.Perpendicular((to - from).normalized);
            float jitter = Mathf.Clamp(Vector2.Distance(from, to) * 0.14f, 0.08f, 0.42f);
            line.SetPosition(0, new Vector3(from.x, from.y, -0.05f));
            line.SetPosition(1, new Vector3(mid.x + normal.x * jitter, mid.y + normal.y * jitter, -0.05f));
            line.SetPosition(2, new Vector3(to.x, to.y, -0.05f));
            line.startWidth = 0.13f;
            line.endWidth = 0.05f;
            line.startColor = new Color(1f, 0.96f, 0.42f, 0.95f);
            line.endColor = new Color(0.45f, 0.92f, 1f, 0.08f);
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.sortingLayerName = "VFX";
            line.sortingOrder = 25;
            Destroy(go, 0.12f);
        }

        private Collider2D FindNearestEnemy(Vector2 from, HashSet<Collider2D> exclude)
        {
            var cols = Physics2D.OverlapCircleAll(from, jumpRange);
            float minD = float.MaxValue;
            Collider2D best = null;
            foreach (var c in cols)
            {
                if (c.CompareTag("Player")) continue;
                if (exclude != null && exclude.Contains(c)) continue;
                if (c.GetComponent<Health>() == null) continue;
                float d = Vector2.Distance(from, c.transform.position);
                if (d < minD) { minD = d; best = c; }
            }
            return best;
        }
    }
}
