using UnityEngine;

namespace RIMA
{
    public class HuntersStep : RangerSkillBase
    {
        [SerializeField] private float distance = 4f;
        [SerializeField] private float critWindow = 2f;

        private Rigidbody2D rb;

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponentInParent<Rigidbody2D>();
            skillName = "Hunter's Step";
            cooldown = 6f;
            resourceCost = 20;
        }

        protected override void Execute()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            Vector2 end = (Vector2)transform.position + dir.normalized * distance;
            if (rb != null) rb.position = end;
            else transform.position = end;

            SkillRuntime.State(gameObject)?.Apply("HunterStepCrit", critWindow, 1, 1);
            focus?.Add(8);
        }
    }
}
