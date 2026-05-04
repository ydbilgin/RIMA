using UnityEngine;

namespace RIMA
{
    public class FrostWall : SkillBase
    {
        [SerializeField] private float length = 4.2f;
        [SerializeField] private float width = 0.75f;
        [SerializeField] private int damage = 18;
        [SerializeField] private float slowDuration = 2f;

        private Elementalist_SkillController ctrl;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Frost Wall";
            cooldown = 10f;
            resourceCost = 35;
            ctrl = GetComponentInParent<Elementalist_SkillController>();
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            Vector2 right = new Vector2(-dir.y, dir.x).normalized;
            Vector2 center = (Vector2)transform.position + dir * 2f;

            foreach (var health in SkillRuntime.EnemiesInLine(center - right * (length * 0.5f), right, length, width))
            {
                SkillRuntime.DealDamage(health, damage);
                var status = health.GetComponent<StatusEffectSystem>();
                if (status != null)
                {
                    status.ApplyEffect(StatusEffectType.Chill, slowDuration);
                    if (status.HasEffect(StatusEffectType.Frozen))
                        status.ApplyEffect(StatusEffectType.Frozen, status.GetDuration(StatusEffectType.Frozen) + 1f);
                }
            }

            ctrl?.RegisterElementCast(ElementalistElement.Frost, 1);
            SkillRuntime.SpawnCircleVisual(center, new Color(0.58f, 0.92f, 1f, 0.55f), 1.45f, 4f, "FrostWall_Runtime");
        }
    }
}
