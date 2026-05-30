using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// 4 primary slot (Q/E/R/F) + 2 secondary slot (Z/X, boss sonrası açılır).
    /// </summary>
    public class Warblade_SkillController : MonoBehaviour
    {
        [Header("Skill Slots")]
        [SerializeField] private SkillBase[] slots = new SkillBase[6];

        public bool SecondaryUnlocked { get; private set; }

        private InputAction[] skillActions;
        private bool actionsEnabled;

        // Combo
        private int lastUsedSlot = -1;
        private float lastUseTime;
        private const float ComboWindow = 0.6f;

        // Slot 4-5 sabit Z/X bağlaması
        private static readonly string[] SecondaryBindings = { "<Keyboard>/z", "<Keyboard>/x" };

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
        public int SlotCount => SecondaryUnlocked ? 6 : 4;

        /// <summary> Boss öldükten sonra PlayerClassManager çağırır. </summary>
        public void UnlockSecondarySlots()
        {
            if (SecondaryUnlocked) return;
            SecondaryUnlocked = true;
            RebuildBindings();
            Debug.Log("[WB_SkillController] Secondary slots açıldı (Z/X).");
        }

        /// <summary> Test reset için. </summary>
        public void LockSecondarySlots()
        {
            if (!SecondaryUnlocked) return;
            SecondaryUnlocked = false;
            slots[4] = null;
            slots[5] = null;
            RebuildBindings();
        }

        private void Awake()
        {
            EnsureDefaultLoadout();
            RebuildBindings();
        }

        private void EnsureDefaultLoadout()
        {
            if (slots == null || slots.Length != 6)
                slots = new SkillBase[6];

            AssignDefaultSlot<IronCharge>(0);
            AssignDefaultSlot<GravityCleave>(1);
            AssignDefaultSlot<SunderMark>(2);
            AssignDefaultSlot<Earthsplitter>(3);
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
                for (int i = 0; i < skillActions.Length; i++)
                    skillActions[i]?.Disable();
            actionsEnabled = false;

            int total = SecondaryUnlocked ? 6 : 4;
            skillActions = new InputAction[total];

            // Primary slots: KeyBindManager'dan
            for (int i = 0; i < 4; i++)
            {
                skillActions[i] = new InputAction($"Skill{i}", InputActionType.Button);
                skillActions[i].AddBinding(KeyBindManager.GetBinding(i));
            }

            // Secondary slots: Z / X
            if (SecondaryUnlocked)
            {
                for (int i = 0; i < 2; i++)
                {
                    skillActions[4 + i] = new InputAction($"SecSkill{i}", InputActionType.Button);
                    skillActions[4 + i].AddBinding(SecondaryBindings[i]);
                }
            }

            if (isActiveAndEnabled) EnableActions();
        }

        private void OnEnable()
        {
            EnableActions();
            KeyBindManager.OnBindingsChanged += RebuildBindings; // live skill rebind (cx C1-C3 Q5)
        }

        private void OnDisable()
        {
            KeyBindManager.OnBindingsChanged -= RebuildBindings;
            DisableActions();
        }

        private void EnableActions()
        {
            if (actionsEnabled) return;
            if (skillActions == null) return;
            for (int i = 0; i < skillActions.Length; i++)
            {
                if (skillActions[i] == null) continue;
                var idx = i;
                skillActions[i].Enable();
                skillActions[i].performed += _ => TryUse(idx);
            }
            actionsEnabled = true;
        }

        private void DisableActions()
        {
            if (skillActions == null) return;
            foreach (var a in skillActions) a?.Disable();
            actionsEnabled = false;
        }

        private void Start() => EnsureDefaultLoadout();

        public void RefreshSlots()
        {
            var skills = GetComponentsInChildren<SkillBase>();
            for (int i = 0; i < Mathf.Min(slots.Length, skills.Length); i++)
                slots[i] = skills[i];
        }

        private void TryUse(int slot)
        {
            if (slot >= slots.Length || slots[slot] == null) return;

            bool used = slots[slot].TryActivate();
            if (!used) return;

            if (lastUsedSlot >= 0 && lastUsedSlot != slot && Time.time - lastUseTime < ComboWindow)
                Debug.Log($"[Combo] {slots[lastUsedSlot]?.skillName} → {slots[slot].skillName}!");

            lastUsedSlot = slot;
            lastUseTime  = Time.time;
        }
    }
}
