using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// Ranger — 4 skill slotu. FocusSystem'e doğrudan bağlı.
    /// </summary>
    public class Ranger_SkillController : MonoBehaviour
    {
        [Header("Skill Slots")]
        [SerializeField] private SkillBase[] slots = new SkillBase[4];

        private InputAction[] skillActions;
        private bool actionsEnabled;
        private float tacticalRollCooldownTimer;
        private Rigidbody2D rb;
        private PlayerController player;
        private FocusSystem focus;

        public int SlotCount => slots.Length;
        public bool SecondaryUnlocked => false;

        private void Update()
        {
            if (tacticalRollCooldownTimer > 0f)
                tacticalRollCooldownTimer -= Time.deltaTime;
        }

        public bool TryTacticalRoll(Vector2 fallbackDirection)
        {
            if (tacticalRollCooldownTimer > 0f) return false;
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            if (player == null) player = GetComponent<PlayerController>();
            if (focus == null) focus = GetComponent<FocusSystem>();

            Vector2 dir = player != null && player.IsMoving
                ? -player.FacingDirection
                : -fallbackDirection;
            if (dir.sqrMagnitude < 0.001f) dir = Vector2.left;
            rb?.MovePosition((Vector2)transform.position + dir.normalized * 2f);
            focus?.Add(10);
            tacticalRollCooldownTimer = 1.2f;
            return true;
        }

        public void SetSlot(int i, SkillBase s) { if (i >= 0 && i < slots.Length) slots[i] = s; }
        public void SwapSlots(int a, int b) { if (a < slots.Length && b < slots.Length) (slots[a], slots[b]) = (slots[b], slots[a]); }
        public SkillBase GetSlot(int i) => i >= 0 && i < slots.Length ? slots[i] : null;
        public SkillBase[] GetAllSlots() => slots;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<PlayerController>();
            focus = GetComponent<FocusSystem>();
            EnsureDefaultLoadout();
            RebuildBindings();
        }

        private void EnsureDefaultLoadout()
        {
            if (slots == null || slots.Length != 4)
                slots = new SkillBase[4];

            AssignDefaultSlot<PinningShot>(0);
            AssignDefaultSlot<BoneTrap>(1);
            AssignDefaultSlot<MarkedDetonate>(2);
            AssignDefaultSlot<SweepVolley>(3);
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
                skillActions[i] = new InputAction($"Skill{i}", InputActionType.Button);
                skillActions[i].AddBinding(KeyBindManager.GetBinding(i));
            }
            if (isActiveAndEnabled) EnableActions();
        }

        private void OnEnable()  => EnableActions();
        private void OnDisable()
        {
            if (skillActions != null) foreach (var a in skillActions) a?.Disable();
            actionsEnabled = false;
        }

        private void EnableActions()
        {
            if (actionsEnabled) return;
            if (skillActions == null) return;
            for (int i = 0; i < skillActions.Length; i++)
            {
                int idx = i;
                skillActions[i].performed += _ => TryUse(idx);
                skillActions[i].Enable();
            }
            actionsEnabled = true;
        }

        private void TryUse(int idx)
        {
            if (idx >= slots.Length || slots[idx] == null) return;
            slots[idx].TryActivate();
        }
    }
}
