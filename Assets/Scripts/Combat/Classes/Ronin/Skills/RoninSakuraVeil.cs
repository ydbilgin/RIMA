using System.Collections;
using UnityEngine;

namespace RIMA
{
    public class RoninSakuraVeil : SkillBase
    {
        [SerializeField] private float deflectWindow = 0.4f;
        [SerializeField] private float counterRadius = 1.8f;
        [SerializeField] private int counterDamage = 36;
        [SerializeField] private int tensionRefund = 30;

        private TensionSystem tension;
        private bool deflectActive;
        private bool deflectSucceeded;

        protected override void Awake()
        {
            base.Awake();
            tension = GetComponentInParent<TensionSystem>();
            skillName = "Sakura Veil";
            cooldown = 6f;
            resourceCost = 0;
        }

        protected override void Execute()
        {
            RegisterCoroutine(DeflectRoutine());
        }

        public void RegisterDeflectSuccess()
        {
            if (!deflectActive || deflectSucceeded) return;

            deflectSucceeded = true;
            tension?.RefundOnDeflect(tensionRefund);

            Vector2 origin = transform.position;
            foreach (var health in SkillRuntime.EnemiesInCircle(origin, counterRadius))
                SkillRuntime.DealDamage(health, counterDamage);

            SkillRuntime.SpawnCircleVisual(origin, new Color(1f, 0.96f, 1f, 0.72f), 1.35f, 0.12f, "Ronin_SakuraVeil_Counter");
            HitStop.Instance?.Freeze(0.05f);
            LightPulse.Emit(Color.white, 1.4f, 0.05f);
        }

        private IEnumerator DeflectRoutine()
        {
            deflectActive = true;
            deflectSucceeded = false;
            SkillRuntime.SpawnCircleVisual(transform.position, new Color(0.86f, 0.72f, 1f, 0.42f), 1.15f, deflectWindow, "Ronin_SakuraVeil");

            float elapsed = 0f;
            while (elapsed < deflectWindow)
            {
                elapsed += Time.deltaTime;
                if (!deflectSucceeded && SkillRuntime.EnemiesInCircle(transform.position, counterRadius).Count > 0)
                    RegisterDeflectSuccess();
                yield return null;
            }

            deflectActive = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!deflectActive || other.CompareTag("Player")) return;
            RegisterDeflectSuccess();
        }

        private void OnDisable()
        {
            deflectActive = false;
            CancelTrackedCoroutines();
        }
    }
}
