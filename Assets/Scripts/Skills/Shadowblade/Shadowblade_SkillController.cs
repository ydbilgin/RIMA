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

        private float stealthTimer;
        private float evasionTimer;

        private InputAction[] skillActions;

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
        }

        public void EnterStealth(float duration) { IsInStealth = true;  stealthTimer = duration; }
        public void ExitStealth()               { IsInStealth = false; stealthTimer = 0f; }
        public void ActivateEvasion(float duration) { EvasionActive = true; evasionTimer = duration; }

        public void SetSlot(int i, SkillBase s) { if (i >= 0 && i < slots.Length) slots[i] = s; }
        public void SwapSlots(int a, int b) { if (a < slots.Length && b < slots.Length) (slots[a], slots[b]) = (slots[b], slots[a]); }
        public SkillBase GetSlot(int i) => i >= 0 && i < slots.Length ? slots[i] : null;
        public SkillBase[] GetAllSlots() => slots;

        private void Awake() => RebuildBindings();

        public void RebuildBindings()
        {
            if (skillActions != null)
                for (int i = 0; i < skillActions.Length; i++) skillActions[i]?.Disable();

            skillActions = new InputAction[4];
            for (int i = 0; i < 4; i++)
            {
                skillActions[i] = new InputAction($"Skill{i}", InputActionType.Button);
                skillActions[i].AddBinding(KeyBindManager.GetBinding(i));
            }
            if (isActiveAndEnabled) EnableActions();
        }

        private void OnEnable()  => EnableActions();
        private void OnDisable() { if (skillActions != null) foreach (var a in skillActions) a?.Disable(); }

        private void EnableActions()
        {
            if (skillActions == null) return;
            for (int i = 0; i < skillActions.Length; i++)
            {
                int idx = i;
                skillActions[i].performed += _ => TryUse(idx);
                skillActions[i].Enable();
            }
        }

        private void TryUse(int idx)
        {
            if (idx >= slots.Length || slots[idx] == null) return;
            // Stealth kırılır (Ambush hariç — kendi içinde kontrol eder)
            if (IsInStealth && !(slots[idx] is Ambush) && !(slots[idx] is Vanish))
                ExitStealth();
            slots[idx].TryActivate();
        }
    }
}
