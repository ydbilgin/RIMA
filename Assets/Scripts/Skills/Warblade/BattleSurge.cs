using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — Battle Surge
    /// 8s: her Rage harcaması → HP +%5.
    /// Chain: Rage 80+'ta aktive → süre 12s.
    /// </summary>
    public class BattleSurge : SkillBase
    {
        [Header("Battle Surge")]
        [SerializeField] private float baseDuration = 8f;
        [SerializeField] private float chainDuration = 12f;
        [SerializeField] private float healPercent = 0.05f; // HP'nin %5'i

        private Health playerHealth;
        private bool buffActive;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Battle Surge";
            cooldown = 16f;
            rageCost = 0;
            playerHealth = GetComponentInParent<Health>();
        }

        protected override void Execute()
        {
            bool chained = rage != null && rage.CurrentRage >= 80;
            float dur = chained ? chainDuration : baseDuration;
            StartCoroutine(SurgeBuff(dur));
        }

        private IEnumerator SurgeBuff(float duration)
        {
            buffActive = true;
            float elapsed = 0f;
            int prevRage = rage != null ? rage.CurrentRage : 0;

            while (elapsed < duration)
            {
                yield return null;
                elapsed += Time.deltaTime;

                if (rage == null || playerHealth == null) break;

                int cur = rage.CurrentRage;
                if (cur < prevRage) // Rage harcandı
                {
                    int healAmount = Mathf.RoundToInt(playerHealth.MaxHP * healPercent);
                    playerHealth.Heal(healAmount);
                }
                prevRage = cur;
            }

            buffActive = false;
        }
    }
}
