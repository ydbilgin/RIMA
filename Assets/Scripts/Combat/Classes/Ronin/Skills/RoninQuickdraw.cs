using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    public class RoninQuickdraw : SkillBase
    {
        [SerializeField] private float dashDistance = 2.4f;
        [SerializeField] private float lineLength = 2.8f;
        [SerializeField] private float lineWidth = 0.75f;
        [SerializeField] private int damage = 42;
        [SerializeField] private int tensionRefundOnHit = 10;

        private Rigidbody2D rb;
        private TensionSystem tension;

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponentInParent<Rigidbody2D>();
            tension = GetComponentInParent<TensionSystem>();
            skillName = "Quickdraw";
            cooldown = 2.2f;
            resourceCost = 20;
        }

        protected override void Execute()
        {
            Vector2 origin = transform.position;
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            DoSlash(origin, dir, true);
        }

        public void ExecuteEcho(Vector2 origin)
        {
            Vector2 dir = ((Vector2)transform.position - origin).sqrMagnitude > 0.01f
                ? ((Vector2)transform.position - origin).normalized
                : (player != null ? player.FacingDirection : Vector2.right);
            DoSlash(transform.position, dir, false);
        }

        private void DoSlash(Vector2 origin, Vector2 dir, bool moveRonin)
        {
            dir = dir.sqrMagnitude > 0.001f ? dir.normalized : Vector2.right;

            if (moveRonin)
            {
                Vector2 destination = origin + dir * dashDistance;
                if (rb != null) rb.MovePosition(destination);
                else transform.position = destination;
            }

            bool hitAny = false;
            List<Health> hits = SkillRuntime.EnemiesInLine(origin, dir, lineLength, lineWidth);
            for (int i = 0; i < hits.Count; i++)
            {
                SkillRuntime.DealDamage(hits[i], damage);
                hitAny = true;
            }

            if (hitAny && moveRonin)
                tension?.RefundOnHit(tensionRefundOnHit);

            SkillRuntime.SpawnCircleVisual(origin + dir * 1.2f, new Color(0.42f, 0.95f, 1f, 0.55f), 0.72f, 0.12f, "Ronin_Quickdraw");
            HitStop.Instance?.FreezeLight();
        }
    }
}
