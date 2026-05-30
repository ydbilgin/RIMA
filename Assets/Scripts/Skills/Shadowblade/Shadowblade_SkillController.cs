using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// Shadowblade — 4 skill slotu + stealth/evasion state yönetimi.
    /// </summary>
    public class Shadowblade_SkillController : MonoBehaviour
    {
        [Header("Skill Slots")]
        [SerializeField] private SkillBase[] slots = new SkillBase[4];

        public bool IsInStealth   { get; private set; }
        public bool EvasionActive { get; private set; }
        public int Sever { get; private set; }

        private float stealthTimer;
        private float evasionTimer;
        private float veilFlickerCooldownTimer;

        private InputAction[] skillActions;
        private bool actionsEnabled;
        private Rigidbody2D rb;
        private PlayerController player;

        private void Update()
        {
            if (stealthTimer > 0f)
            {
                stealthTimer -= Time.deltaTime;
                if (stealthTimer <= 0f) ExitStealth();
            }
            if (evasionTimer > 0f)
            {
                evasionTimer -= Time.deltaTime;
                if (evasionTimer <= 0f) EvasionActive = false;
            }
            if (veilFlickerCooldownTimer > 0f)
                veilFlickerCooldownTimer -= Time.deltaTime;
        }

        public void EnterStealth(float duration) { IsInStealth = true;  stealthTimer = duration; }
        public void ExitStealth()               { IsInStealth = false; stealthTimer = 0f; }
        public void ActivateEvasion(float duration) { EvasionActive = true; evasionTimer = duration; }
        public void AddSever(int amount) => Sever = Mathf.Clamp(Sever + amount, 0, 100);
        public bool TrySpendSever(int amount)
        {
            if (Sever < amount) return false;
            Sever -= amount;
            return true;
        }

        public bool TryVeilFlicker(Vector2 fallbackDirection)
        {
            if (veilFlickerCooldownTimer > 0f) return false;
            if (rb == null) rb = GetComponent<Rigidbody2D>();
            if (player == null) player = GetComponent<PlayerController>();

            Vector2 dir = player != null ? player.FacingDirection : fallbackDirection;
            if (dir.sqrMagnitude < 0.001f) dir = Vector2.right;
            Vector2 start = transform.position;
            Vector2 end = start + dir.normalized * 2.4f;

            foreach (var health in SkillRuntime.EnemiesInLine(start, dir, 2.6f, 0.8f))
                SkillRuntime.State(health)?.Apply(SkillStateTracker.RiftScar, 8f, 1, 5);

            if (rb != null) rb.position = end;
            else transform.position = end;

            AddSever(15);
            veilFlickerCooldownTimer = 1.2f;
            return true;
        }

        public void SetSlot(int i, SkillBase s) { if (i >= 0 && i < slots.Length) slots[i] = s; }
        public void SwapSlots(int a, int b) { if (a < slots.Length && b < slots.Length) (slots[a], slots[b]) = (slots[b], slots[a]); }
        public SkillBase GetSlot(int i) => i >= 0 && i < slots.Length ? slots[i] : null;
        public SkillBase[] GetAllSlots() => slots;

        public int SlotCount => slots.Length;
        public bool SecondaryUnlocked => false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            player = GetComponent<PlayerController>();
            EnsureDefaultLoadout();
            RebuildBindings();
        }

        private void EnsureDefaultLoadout()
        {
            if (slots == null || slots.Length != 4)
                slots = new SkillBase[4];

            AssignDefaultSlot<PhaseStep>(0);
            AssignDefaultSlot<BackstabMark>(1);
            AssignDefaultSlot<DeathMark>(2);
            AssignDefaultSlot<ShadowPin>(3);
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

        private void OnEnable()
        {
            EnableActions();
            KeyBindManager.OnBindingsChanged += RebuildBindings; // live skill rebind (cx C1-C3 Q5)
        }

        private void OnDisable()
        {
            KeyBindManager.OnBindingsChanged -= RebuildBindings;
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
            // Stealth kırılır (Ambush hariç — kendi içinde kontrol eder)
            if (IsInStealth && !(slots[idx] is SmokeVeil) && !(slots[idx] is BackstabMark))
                ExitStealth();
            slots[idx].TryActivate();
        }
    }
}
