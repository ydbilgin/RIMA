using System.Collections;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Warblade — Battle Surge
    /// 8s: her Rage harcaması → flat HP +%5 (2s internal cooldown — canon). Chain: Rage 80+'ta
    /// aktive → süre 12s.
    /// A3: heal is event-driven off RageSystem.OnRageChanged (not per-frame polling), so any
    /// single spend — including Death Blow emptying the whole bar — reliably triggers the heal
    /// while the buff is active. This is the canon execute-payoff loop (heal is NOT on Death Blow).
    /// </summary>
    public class BattleSurge : SkillBase
    {
        [Header("Battle Surge")]
        [SerializeField] private float baseDuration = 8f;
        [SerializeField] private float chainDuration = 12f;
        [SerializeField] private float healPercent = 0.05f; // HP'nin %5'i
        [Tooltip("Canon: flat heal at most once per this many seconds (multiple spends inside the " +
                 "window do not stack extra healing).")]
        [SerializeField] private float healInternalCooldown = 2f;

        private Health playerHealth;
        private bool buffActive;
        private float lastHealTime = -999f;
        private int prevRage;
        private bool subscribed;

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

            prevRage = rage != null ? rage.CurrentRage : 0;
            Subscribe();
            // Refresh the active window (do not stack coroutines): cancel any running buff timer.
            CancelTrackedCoroutines();
            RegisterCoroutine(BuffWindow(dur));
        }

        private IEnumerator BuffWindow(float duration)
        {
            buffActive = true;
            yield return new WaitForSeconds(duration);
            buffActive = false;
            Unsubscribe();
        }

        private void Subscribe()
        {
            if (subscribed || rage == null || rage.OnRageChanged == null) return;
            rage.OnRageChanged.AddListener(OnRageChanged);
            subscribed = true;
        }

        private void Unsubscribe()
        {
            if (!subscribed || rage == null || rage.OnRageChanged == null) return;
            rage.OnRageChanged.RemoveListener(OnRageChanged);
            subscribed = false;
        }

        private void OnRageChanged(int current, int max)
        {
            if (buffActive && current < prevRage && playerHealth != null) // Rage spent
            {
                if (Time.time - lastHealTime >= healInternalCooldown)
                {
                    lastHealTime = Time.time;
                    int healAmount = Mathf.RoundToInt(playerHealth.MaxHP * healPercent);
                    playerHealth.Heal(healAmount);
                }
            }
            prevRage = current;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}
