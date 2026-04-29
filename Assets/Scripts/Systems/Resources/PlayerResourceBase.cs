using UnityEngine;
using UnityEngine.Events;

namespace RIMA
{
    /// <summary>
    /// Tüm class kaynak sistemlerinin (Rage, Mana, Energy, Focus) ortak tabanı.
    /// SkillBase bu interface üzerinden kaynak tüketir.
    /// </summary>
    public abstract class PlayerResourceBase : MonoBehaviour
    {
        public abstract int Current { get; }
        public abstract int Max { get; }
        public float Percent => Max > 0 ? (float)Current / Max : 0f;

        public UnityEvent<int, int> OnResourceChanged;

        public abstract bool TrySpend(int amount);
        public abstract void Add(int amount);
    }
}
