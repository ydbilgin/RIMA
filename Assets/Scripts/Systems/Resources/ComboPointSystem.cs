using UnityEngine;
using UnityEngine.Events;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Combo Points (0-5).
    /// Skill'ler Add ile kazandırır, TrySpend ile finisher'lar tüketir.
    /// </summary>
    public class ComboPointSystem : MonoBehaviour
    {
        public const int MaxPoints = 5;
        private int current;

        public int Current => current;
        public bool IsFull => current >= MaxPoints;

        public UnityEvent<int> OnComboChanged;

        public void Add(int amount)
        {
            current = Mathf.Clamp(current + amount, 0, MaxPoints);
            OnComboChanged?.Invoke(current);
        }

        public bool TrySpend(int required)
        {
            if (current < required) return false;
            current = 0;
            OnComboChanged?.Invoke(current);
            return true;
        }

        /// <summary> Finisher: harcamadan önce kaçtaki puanı döndürür (hasar hesabı için). </summary>
        public int PeekAndSpend()
        {
            int pts = current;
            current = 0;
            OnComboChanged?.Invoke(current);
            return pts;
        }
    }
}
