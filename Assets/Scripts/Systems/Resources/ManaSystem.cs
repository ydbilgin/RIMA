using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Elementalist kaynak sistemi.
    /// +8/sn otomatik regen. Skill kullanımında tükenir.
    /// </summary>
    public class ManaSystem : PlayerResourceBase
    {
        [SerializeField] private int maxMana = 100;
        [SerializeField] private float regenPerSecond = 8f;

        private float current;

        public override int Current => Mathf.RoundToInt(current);
        public override int Max => maxMana;
        public float ManaPercent => current / maxMana;

        private void Awake() => current = maxMana;

        private void Update()
        {
            if (current < maxMana)
            {
                current = Mathf.Min(current + regenPerSecond * Time.deltaTime, maxMana);
                OnResourceChanged?.Invoke(Current, maxMana);
            }
        }

        public override bool TrySpend(int amount)
        {
            if (current < amount) return false;
            current -= amount;
            OnResourceChanged?.Invoke(Current, maxMana);
            return true;
        }

        public override void Add(int amount)
        {
            current = Mathf.Clamp(current + amount, 0, maxMana);
            OnResourceChanged?.Invoke(Current, maxMana);
        }
    }
}
