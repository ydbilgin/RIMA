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

        // Open-generic UnityEvent<int,int> is NOT Unity-serialized, so a SerializeField/inspector value
        // never populates it → it is null at runtime. Initialize inline (runs in ctor, before Awake) so
        // HUDController's Add/RemoveListener and subclass Invoke calls never NRE. (Health does the ??= variant.)
        public UnityEvent<int, int> OnResourceChanged = new UnityEvent<int, int>();

        public abstract bool TrySpend(int amount);
        public abstract void Add(int amount);
    }
}
