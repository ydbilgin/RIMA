using System.Collections;
using UnityEngine;

namespace RIMA
{
    public class RoninIaidoStance : SkillBase
    {
        [SerializeField] private float stanceDuration = 0.8f;
        [SerializeField] private float slashLength = 3.2f;
        [SerializeField] private float slashWidth = 0.9f;
        [SerializeField] private int empoweredDamage = 58;

        private Rigidbody2D rb;
        private TensionSystem tension;

        protected override void Awake()
        {
            base.Awake();
            rb = GetComponentInParent<Rigidbody2D>();
            tension = GetComponentInParent<TensionSystem>();
            skillName = "Iaido Stance";
            cooldown = 5f;
            resourceCost = 0;
        }

        protected override void Execute()
        {
            RegisterCoroutine(StanceRoutine());
        }

        private IEnumerator StanceRoutine()
        {
            tension?.SetIaidoStanceActive(true);

            float elapsed = 0f;
            while (elapsed < stanceDuration)
            {
                elapsed += Time.deltaTime;
                if (rb != null) rb.linearVelocity = Vector2.zero;
                yield return null;
            }

            tension?.SetIaidoStanceActive(false);
            EmpoweredSlash();
        }

        private void EmpoweredSlash()
        {
            Vector2 dir = player != null ? player.FacingDirection : Vector2.right;
            Vector2 origin = transform.position;
            foreach (var health in SkillRuntime.EnemiesInLine(origin, dir, slashLength, slashWidth))
            {
                SkillRuntime.DealDamage(health, empoweredDamage);
                SkillRuntime.State(health)?.Apply("Opened", 4f, 1, 1);
            }

            SkillRuntime.SpawnCircleVisual(origin + dir.normalized * 1.35f, new Color(0.72f, 0.42f, 1f, 0.52f), 0.95f, 0.14f, "Ronin_IaidoSlash");
            HitStop.Instance?.FreezeLight();
        }

        private void OnDisable()
        {
            tension?.SetIaidoStanceActive(false);
            CancelTrackedCoroutines();
        }
    }
}
