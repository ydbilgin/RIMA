using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// Elementalist — 4 skill slotu + Fire/Frost State takibi.
    /// </summary>
    public class Elementalist_SkillController : MonoBehaviour
    {
        [Header("Skill Slots")]
        [SerializeField] private SkillBase[] slots = new SkillBase[4];

        // Elementalist state — skills query these
        public int FireState   { get; private set; }
        public int FrostState  { get; private set; }
        public bool CombustionActive { get; private set; }
        public bool ArcaneSurgeActive { get; private set; }

        private float combustionTimer;
        private float arcaneSurgeTimer;

        private InputAction[] skillActions;

        private void Update()
        {
            if (combustionTimer > 0f)
            {
                combustionTimer -= Time.deltaTime;
                if (combustionTimer <= 0f) CombustionActive = false;
            }
            if (arcaneSurgeTimer > 0f)
            {
                arcaneSurgeTimer -= Time.deltaTime;
                if (arcaneSurgeTimer <= 0f) ArcaneSurgeActive = false;
            }
        }

        public void AddFireState(int stacks)  => FireState  = Mathf.Clamp(FireState  + stacks, 0, 5);
        public void AddFrostState(int stacks) => FrostState = Mathf.Clamp(FrostState + stacks, 0, 5);
        public int ConsumeFireState(int stacks)  { int s = Mathf.Min(FireState, stacks);  FireState  -= s; return s; }
        public int ConsumeFrostState(int stacks) { int s = Mathf.Min(FrostState, stacks); FrostState -= s; return s; }

        public void ActivateCombustion(float duration) { CombustionActive = true;  combustionTimer  = duration; }
        public void ActivateArcaneSurge(float duration) { ArcaneSurgeActive = true; arcaneSurgeTimer = duration; }

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
            slots[idx].TryActivate();
        }
    }
}
