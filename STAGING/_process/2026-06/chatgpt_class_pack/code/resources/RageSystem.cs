using UnityEngine;
using UnityEngine.Events;

namespace RIMA
{
    public class RageSystem : PlayerResourceBase
    {
        [SerializeField] private int maxRage = 100;
        private int currentRage;

        public override int Current => currentRage;
        public override int Max => maxRage;
        public int CurrentRage => currentRage;
        public int MaxRage => maxRage;
        public float RagePercent => (float)currentRage / maxRage;

        public UnityEvent<int, int> OnRageChanged;

        [Header("Generation")]
        [SerializeField] private int ragePerHitDealt = 1;   // normal oda: V dolmaz; boss: 2-3 V
        [HideInInspector] public float gainMultiplier = 1f;
        [SerializeField] private int ragePerHitTaken = 5;   // hasar almak anlamlı rage verir
        [SerializeField] private int ragePerKill = 3;       // kill bonus küçük, combo'yu değil kill'i ödüller
        [SerializeField] private float decayDelay = 1.5f;   // savaştan 1.5s sonra erimeye başla
        [SerializeField] private int decayPerSecond = 10;   // aktif decay — rage birikiyor hissi yok

        [Header("Thresholds")]
        [SerializeField] private int furyThreshold = 50;
        [SerializeField] private int bloodrageThreshold = 80;

        private float lastCombatTime;
        private float decayAccumulator;
        private bool isFury;
        private bool isBloodrage;

        public bool IsFury => isFury;
        public bool IsBloodrage => isBloodrage;

        public UnityEvent<bool> OnFuryStateChanged = new UnityEvent<bool>();
        public UnityEvent<bool> OnBloodrageStateChanged = new UnityEvent<bool>();

        private void Awake() => currentRage = 0;

        private void Update()
        {
            if (currentRage > 0 && Time.time - lastCombatTime > decayDelay)
            {
                decayAccumulator += decayPerSecond * Time.deltaTime;
                int decay = Mathf.FloorToInt(decayAccumulator);
                if (decay > 0)
                {
                    decayAccumulator -= decay;
                    Modify(-decay);
                }
            }
            else
            {
                decayAccumulator = 0f;
            }
        }

        // Called by PlayerAttack when hitting enemy
        public void OnDealDamage()
        {
            lastCombatTime = Time.time;
            Modify(ragePerHitDealt);
        }

        // Called by Health.OnDamageTaken
        public void OnTakeDamage(int damageAmount)
        {
            lastCombatTime = Time.time;
            Modify(ragePerHitTaken);
        }

        // Called by RuntimeRoomManager on kill
        public void OnKillEnemy()
        {
            lastCombatTime = Time.time;
            Modify(ragePerKill);
        }

        // Legacy compat
        public void OnHitEnemy() => OnDealDamage();
        public void AddRage(int amount) { lastCombatTime = Time.time; Modify(Mathf.RoundToInt(amount * gainMultiplier)); }

        public override void Add(int amount) => Modify(amount);

        public bool HasRage(int amount) => currentRage >= amount;

        public bool TryConsume(int amount)
        {
            if (currentRage < amount) return false;
            Modify(-amount);
            lastCombatTime = Time.time;
            return true;
        }

        public override bool TrySpend(int amount) => TryConsume(amount);

        public void ResetRage() => Modify(-currentRage);

        /// <summary>Rage'i belirli bir değere set eder (TemperedFury gibi pasifler kullanır).</summary>
        public void SetRage(int value) => Modify(Mathf.Clamp(value, 0, maxRage) - currentRage);

        /// <summary>MaxRage'i artırır (Deep Reserves trait).</summary>
        public void AddMaxRage(int amount) { maxRage += amount; OnRageChanged?.Invoke(currentRage, maxRage); }

        private void Modify(int delta)
        {
            int prev = currentRage;
            currentRage = Mathf.Clamp(currentRage + delta, 0, maxRage);

            if (currentRage != prev)
            {
                OnRageChanged?.Invoke(currentRage, maxRage);
                OnResourceChanged?.Invoke(currentRage, maxRage);
            }

            // Fury threshold
            bool wasFury = isFury;
            isFury = currentRage >= furyThreshold;
            if (isFury != wasFury)
                OnFuryStateChanged?.Invoke(isFury);

            // Bloodrage threshold
            bool wasBloodrage = isBloodrage;
            isBloodrage = currentRage >= bloodrageThreshold;
            if (isBloodrage != wasBloodrage)
                OnBloodrageStateChanged?.Invoke(isBloodrage);
        }
    }
}
