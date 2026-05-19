using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    public class RoninController : MonoBehaviour
    {
        [Header("Skill Slots")]
        [SerializeField] private SkillBase[] slots = new SkillBase[4];

        private InputAction[] skillActions;
        private bool actionsEnabled;

        public int SlotCount => slots.Length;
        public bool SecondaryUnlocked => false;

        private void Awake()
        {
            EnsureDefaultLoadout();
            RebuildBindings();
        }

        public void SetSlot(int index, SkillBase skill)
        {
            if (index >= 0 && index < slots.Length)
                slots[index] = skill;
        }

        public void SwapSlots(int a, int b)
        {
            if (a < 0 || a >= slots.Length || b < 0 || b >= slots.Length) return;
            (slots[a], slots[b]) = (slots[b], slots[a]);
        }

        public SkillBase GetSlot(int index) => index >= 0 && index < slots.Length ? slots[index] : null;
        public SkillBase[] GetAllSlots() => slots;

        public void TriggerQuickdrawGhost(Vector2 origin)
        {
            var quickdraw = GetComponent<RoninQuickdraw>();
            if (quickdraw == null) quickdraw = gameObject.AddComponent<RoninQuickdraw>();
            quickdraw.ExecuteEcho(origin);
        }

        private void EnsureDefaultLoadout()
        {
            if (slots == null || slots.Length != 4)
                slots = new SkillBase[4];

            AssignDefaultSlot<RoninQuickdraw>(0);
            AssignDefaultSlot<RoninIaidoStance>(1);
            AssignDefaultSlot<RoninSakuraVeil>(2);
            AssignDefaultSlot<RoninFinalDraw>(3);
        }

        private void AssignDefaultSlot<T>(int index) where T : SkillBase
        {
            if (index < 0 || index >= slots.Length || slots[index] is T) return;
            T skill = GetComponent<T>();
            if (skill == null) skill = gameObject.AddComponent<T>();
            slots[index] = skill;
        }

        public void RebuildBindings()
        {
            if (skillActions != null)
                for (int i = 0; i < skillActions.Length; i++) skillActions[i]?.Disable();
            actionsEnabled = false;

            skillActions = new InputAction[4];
            for (int i = 0; i < 4; i++)
            {
                skillActions[i] = new InputAction($"RoninSkill{i}", InputActionType.Button);
                skillActions[i].AddBinding(KeyBindManager.GetBinding(i));
            }

            if (isActiveAndEnabled) EnableActions();
        }

        private void OnEnable() => EnableActions();

        private void OnDisable()
        {
            if (skillActions != null)
                for (int i = 0; i < skillActions.Length; i++) skillActions[i]?.Disable();
            actionsEnabled = false;
        }

        private void EnableActions()
        {
            if (actionsEnabled || skillActions == null) return;

            for (int i = 0; i < skillActions.Length; i++)
            {
                int idx = i;
                skillActions[i].performed += _ => TryUse(idx);
                skillActions[i].Enable();
            }
            actionsEnabled = true;
        }

        private void TryUse(int index)
        {
            if (index >= slots.Length || slots[index] == null) return;
            slots[index].TryActivate();
        }
    }
}
