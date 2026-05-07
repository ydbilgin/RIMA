using UnityEngine;

namespace RIMA.Combat.Skills
{
    public enum SkillSlotIndex
    {
        Slot1,
        Slot2,
        Slot3,
        Slot4
    }

    [System.Serializable]
    public class SkillSlot
    {
        [SerializeField] private SkillSlotIndex slotIndex;
        [SerializeField] private ActiveSkillData skillData;

        private float cooldownRemaining;

        public SkillSlotIndex SlotIndex => slotIndex;
        public ActiveSkillData SkillData => skillData;
        public bool IsReady => cooldownRemaining <= 0f;
        public float CooldownRemaining => Mathf.Max(0f, cooldownRemaining);

        public bool TryActivate()
        {
            if (skillData == null || !IsReady)
                return false;

            cooldownRemaining = Mathf.Max(0f, skillData.cooldown);
            return true;
        }

        public void OnUpdate(float dt)
        {
            if (cooldownRemaining <= 0f)
                return;

            cooldownRemaining = Mathf.Max(0f, cooldownRemaining - dt);
        }
    }
}
