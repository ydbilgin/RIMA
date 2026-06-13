using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade kaynak sistemi — Energy.
    /// +15/sn pasif regen. Skill kullanımında tükenir.
    /// ComboPointSystem ayrı component olarak eklenir.
    /// </summary>
    public class EnergySystem : PlayerResourceBase
    {
        [SerializeField] private int maxEnergy = 100;
        [SerializeField] private float regenPerSecond = 15f;

        private float current;

        public override int Current => Mathf.RoundToInt(current);
        public override int Max => maxEnergy;
        public float EnergyPercent => current / maxEnergy;

        private void Awake() => current = maxEnergy;

        private void Update()
        {
            if (current < maxEnergy)
            {
                current = Mathf.Min(current + regenPerSecond * Time.deltaTime, maxEnergy);
                OnResourceChanged?.Invoke(Current, maxEnergy);
            }
        }

        public override bool TrySpend(int amount)
        {
            if (current < amount) return false;
            current -= amount;
            OnResourceChanged?.Invoke(Current, maxEnergy);
            return true;
        }

        public override void Add(int amount)
        {
            current = Mathf.Clamp(current + amount, 0, maxEnergy);
            OnResourceChanged?.Invoke(Current, maxEnergy);
        }
    }
}
