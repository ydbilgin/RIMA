using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Pasif yeteneklerin base class'ı. Slot almaz, max 3 seviye.
    /// </summary>
    public abstract class PassiveBase : MonoBehaviour
    {
        public string passiveName = "Passive";
        public int CurrentLevel { get; private set; } = 0;
        public const int MaxLevel = 3;
        public bool CanUpgrade => CurrentLevel < MaxLevel;

        protected PlayerController player;
        protected RageSystem rage;

        protected virtual void Awake()
        {
            player = GetComponentInParent<PlayerController>();
            rage   = GetComponentInParent<RageSystem>();
        }

        /// <summary> DraftManager tarafından çağrılır. </summary>
        public void LevelUp()
        {
            if (CurrentLevel >= MaxLevel) return;
            CurrentLevel++;
            OnLevelUp(CurrentLevel);
            Debug.Log($"[Passive] {passiveName} → Seviye {CurrentLevel}");
        }

        /// <summary> Her seviye artışında çağrılır. </summary>
        protected abstract void OnLevelUp(int level);
    }
}
