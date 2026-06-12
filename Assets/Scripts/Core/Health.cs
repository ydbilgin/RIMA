using UnityEngine;
using UnityEngine.Events;

namespace RIMA
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHP = 100;
        private int currentHP;

        // Internal cooldown on HitImpact SFX to prevent spam from DoT / multi-hit ticks.
        private const float HitImpactIcd = 0.08f;
        private float _nextHitImpactTime;

        public int CurrentHP => currentHP;
        public int MaxHP => maxHP;
        public bool IsDead => currentHP <= 0;

        public UnityEvent<int, int> OnHealthChanged;
        public UnityEvent OnDeath;
        public UnityEvent<int> OnDamageTaken;   // raw damage amount (before multiplier)

        /// <summary>IroncladMomentum gibi skill'ler yazar. 1=normal, 0.7=%30 azaltma.</summary>
        [HideInInspector] public float incomingDamageMultiplier = 1f;

        /// <summary>CripplingBlow gibi anti-heal efektleri yazar. 1=normal, 0=iyileşme yok.</summary>
        [HideInInspector] public float healMultiplier = 1f;

        public void ScaleMaxHP(int multiplier)
        {
            maxHP *= multiplier;
            currentHP = maxHP;
        }

        public void SetMaxHP(int hp)
        {
            maxHP = hp;
            currentHP = hp;
        }

        private void Awake()
        {
            OnHealthChanged ??= new UnityEvent<int, int>();
            OnDeath ??= new UnityEvent();
            OnDamageTaken ??= new UnityEvent<int>();
            currentHP = maxHP;
        }

        public void TakeDamage(int amount)
        {
            if (IsDead || immune) return;
            if (amount <= 0) return;
            OnDamageTaken?.Invoke(amount);
            // TODO E1: 100% damage reduction still floors positive hits to 1 until DR semantics are defined.
            int effective = Mathf.Max(1, Mathf.RoundToInt(amount * incomingDamageMultiplier));
            currentHP = Mathf.Max(0, currentHP - effective);
            if (Time.time >= _nextHitImpactTime)
            {
                RIMA.Audio.AudioManager.Play(RIMA.Audio.Sfx.HitImpact);
                _nextHitImpactTime = Time.time + HitImpactIcd;
            }
            OnHealthChanged?.Invoke(currentHP, maxHP);
            if (currentHP == 0)
            {
                // Use EnemyDeath for non-player objects; Death for player (tag-based distinction)
                bool isPlayer = gameObject.CompareTag("Player");
                RIMA.Audio.AudioManager.Play(isPlayer ? RIMA.Audio.Sfx.Death : RIMA.Audio.Sfx.EnemyDeath);
                OnDeath?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            if (IsDead) return;
            int effective = Mathf.RoundToInt(amount * healMultiplier);
            if (effective <= 0) return;
            currentHP = Mathf.Min(maxHP, currentHP + effective);
            OnHealthChanged?.Invoke(currentHP, maxHP);
        }

        public void RestoreToFull()
        {
            currentHP = maxHP;
            OnHealthChanged?.Invoke(currentHP, maxHP);
        }

        /// <summary> Max HP'yi artırır, mevcut HP'ye de aynı miktarı ekler. </summary>
        public void AddMaxHP(int bonus)
        {
            maxHP    += bonus;
            currentHP = Mathf.Min(currentHP + bonus, maxHP);
            OnHealthChanged?.Invoke(currentHP, maxHP);
        }

        private bool immune;
        /// <summary> Hasar bağışıklığı — Passive_Unyielding gibi pasifler kullanır. </summary>
        public void SetImmune(bool value) => immune = value;

        // TakeDamage içindeki ilk guard'ı immune için güncelle
        // (mevcut TakeDamage'ı değiştirmek yerine override property kullanıyoruz)
        public bool IsImmune => immune;
    }
}
