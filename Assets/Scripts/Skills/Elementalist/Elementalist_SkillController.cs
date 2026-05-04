using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    public enum ElementalistElement { Fire, Frost, Light }

    /// <summary>
    /// Elementalist — 4 skill slotu + Fire/Frost State takibi.
    /// </summary>
    public class Elementalist_SkillController : MonoBehaviour
    {
        [Header("Skill Slots")]
        [SerializeField] private SkillBase[] slots = new SkillBase[4];

        // Elementalist state — skills query these
        public ElementalistElement ActiveElement { get; private set; } = ElementalistElement.Fire;
        public int FireState   { get; private set; }
        public int FrostState  { get; private set; }
        public int LightState  { get; private set; }
        public int FireResonance { get; private set; }
        public int FrostResonance { get; private set; }
        public bool LightStateActive { get; private set; }
        public bool CombustionActive { get; private set; }
        public bool ArcaneSurgeActive { get; private set; }
        public int RiftBoltCounter { get; private set; }

        private ElementalistElement elementBeforeLight = ElementalistElement.Fire;
        private float combustionTimer;
        private float arcaneSurgeTimer;
        private float lightStateTimer;
        private float lightbreakCooldownTimer;

        private InputAction[] skillActions;
        private bool actionsEnabled;

        private void Update()
        {
            if (lightStateTimer > 0f)
            {
                lightStateTimer -= Time.deltaTime;
                if (lightStateTimer <= 0f)
                {
                    LightStateActive = false;
                    LightState = 0;
                    ActiveElement = elementBeforeLight;
                }
            }

            if (lightbreakCooldownTimer > 0f)
                lightbreakCooldownTimer -= Time.deltaTime;

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
        public void AddLightState(int stacks) => LightState = Mathf.Clamp(LightState + stacks, 0, 5);
        public int ConsumeFireState(int stacks)  { int s = Mathf.Min(FireState, stacks);  FireState  -= s; return s; }
        public int ConsumeFrostState(int stacks) { int s = Mathf.Min(FrostState, stacks); FrostState -= s; return s; }
        public int ConsumeLightState(int stacks) { int s = Mathf.Min(LightState, stacks); LightState -= s; return s; }

        public bool RegisterRiftBoltShot()
        {
            RiftBoltCounter++;
            return RiftBoltCounter % 3 == 0;
        }

        public void RegisterRiftBoltHit(bool empowered)
        {
            GetComponent<ManaSystem>()?.Add(3);
            if (!empowered) return;

            if (LightStateActive) AddLightState(1);
            else if (ActiveElement == ElementalistElement.Fire) AddFireState(1);
            else AddFrostState(1);
        }

        public void RegisterElementCast(ElementalistElement element, int stateStacks = 1)
        {
            if (element == ElementalistElement.Fire)
            {
                AddFireState(stateStacks);
                FireResonance = Mathf.Clamp(FireResonance + 1, 0, 3);
            }
            else if (element == ElementalistElement.Frost)
            {
                AddFrostState(stateStacks);
                FrostResonance = Mathf.Clamp(FrostResonance + 1, 0, 3);
            }
            else
            {
                AddLightState(stateStacks);
            }
        }

        public void SwitchElement()
        {
            if (LightStateActive) return;
            ActiveElement = ActiveElement == ElementalistElement.Fire
                ? ElementalistElement.Frost
                : ElementalistElement.Fire;
            LightPulse.Emit(ActiveElement == ElementalistElement.Fire
                ? new Color(1f, 0.35f, 0.12f)
                : new Color(0.34f, 0.82f, 1f), 0.7f, 0.07f);
        }

        public bool TryLightbreak()
        {
            if (lightbreakCooldownTimer > 0f) return false;
            if (FireResonance < 3 || FrostResonance < 3) return false;
            if (FireState < 3 || FrostState < 3) return false;

            FireState -= 3;
            FrostState -= 3;
            FireResonance = 0;
            FrostResonance = 0;
            LightState = 3;
            LightStateActive = true;
            elementBeforeLight = ActiveElement == ElementalistElement.Light ? ElementalistElement.Fire : ActiveElement;
            ActiveElement = ElementalistElement.Light;
            lightStateTimer = 6f;
            lightbreakCooldownTimer = 8f;
            LightPulse.Emit(new Color(1f, 0.88f, 0.36f), 1.2f, 0.10f);
            return true;
        }

        public void ActivateCombustion(float duration) { CombustionActive = true;  combustionTimer  = duration; }
        public void ActivateArcaneSurge(float duration) { ArcaneSurgeActive = true; arcaneSurgeTimer = duration; }

        public void SetSlot(int i, SkillBase s) { if (i >= 0 && i < slots.Length) slots[i] = s; }
        public void SwapSlots(int a, int b) { if (a < slots.Length && b < slots.Length) (slots[a], slots[b]) = (slots[b], slots[a]); }
        public SkillBase GetSlot(int i) => i >= 0 && i < slots.Length ? slots[i] : null;
        public SkillBase[] GetAllSlots() => slots;

        public int SlotCount => slots.Length;
        public bool SecondaryUnlocked => false;

        private void Awake()
        {
            EnsureDefaultLoadout();
            RebuildBindings();
        }

        private void EnsureDefaultLoadout()
        {
            if (slots == null || slots.Length != 4)
                slots = new SkillBase[4];

            AssignDefaultSlot<Fireball>(0);
            AssignDefaultSlot<GlacialSpike>(1);
            AssignDefaultSlot<ChainLightning>(2);
            AssignDefaultSlot<Blink>(3);
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
