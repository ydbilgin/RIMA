using UnityEngine;
using UnityEngine.Events;

namespace RIMA
{
    public class TensionSystem : PlayerResourceBase
    {
        [SerializeField] private int maxTension = 100;
        [SerializeField] private float idleGainPerSecond = 1f;
        [SerializeField] private float movingDrainPerSecond = 2f;
        [SerializeField] private float iaidoGainPerSecond = 5f;

        private float current;
        private PlayerController player;
        private bool iaidoStanceActive;

        public override int Current => Mathf.RoundToInt(current);
        public override int Max => maxTension;
        public int CurrentTension => Current;
        public int MaxTension => maxTension;
        public float TensionPercent => maxTension > 0 ? current / maxTension : 0f;

        public UnityEvent<int, int> OnTensionChanged;

        private void Awake()
        {
            player = GetComponent<PlayerController>();
            current = 0f;
        }

        private void Update()
        {
            float delta;
            if (iaidoStanceActive)
                delta = iaidoGainPerSecond;
            else
                delta = player != null && player.IsMoving ? -movingDrainPerSecond : idleGainPerSecond;

            Modify(delta * Time.deltaTime);
        }

        public void SetIaidoStanceActive(bool active) => iaidoStanceActive = active;
        public void RefundOnHit(int amount) => Add(amount);
        public void RefundOnDeflect(int amount) => Add(amount);

        public int SpendAll()
        {
            int spent = Current;
            Modify(-current);
            return spent;
        }

        public override bool TrySpend(int amount)
        {
            if (current < amount) return false;
            Modify(-amount);
            return true;
        }

        public override void Add(int amount) => Modify(amount);

        public void SetTension(int value) => Modify(Mathf.Clamp(value, 0, maxTension) - current);

        private void Modify(float delta)
        {
            float prev = current;
            current = Mathf.Clamp(current + delta, 0f, maxTension);
            if (Mathf.Abs(current - prev) < 0.01f) return;

            OnTensionChanged?.Invoke(Current, maxTension);
            OnResourceChanged?.Invoke(Current, maxTension);
        }
    }
}
