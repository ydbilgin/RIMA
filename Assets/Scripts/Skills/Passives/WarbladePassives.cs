using UnityEngine;
using RIMA.Combat;

namespace RIMA
{
    // ─────────────────────────────────────────────────────────────
    // Warblade pasif skilleri
    // ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Blood Drinker — Her öldürme +8/+12/+16 HP iyileştirir.
    /// </summary>
    public class Passive_BloodDrinker : PassiveBase
    {
        private Health playerHealth;
        private int healPerKill;

        protected override void Awake()
        {
            base.Awake();
            passiveName = "Blood Drinker";
            playerHealth = GetComponentInParent<Health>();
        }

        private void OnEnable()
        {
            CombatEventBus.OnKill += HandleKillEvent;
        }

        private void OnDisable()
        {
            CombatEventBus.OnKill -= HandleKillEvent;
        }

        protected override void OnLevelUp(int level)
        {
            healPerKill = level switch
            {
                1 => 8,
                2 => 12,
                3 => 16,
                _ => 0
            };
            Debug.Log($"[BloodDrinker] Kill → +{healPerKill} HP");
        }

        private void HandleKillEvent(KillEvent e)
        {
            OnKill();
        }

        public void OnKill()
        {
            if (CurrentLevel == 0 || healPerKill <= 0) return;
            if (playerHealth != null)
            {
                playerHealth.Heal(healPerKill);
                Debug.Log($"[BloodDrinker] Healed +{healPerKill} HP");
            }
        }
    }

    /// <summary>
    /// Wrath Protocol — HP %50 altındayken Rage kazanımı +%20/+%35/+%50.
    /// </summary>
    public class Passive_WrathProtocol : PassiveBase
    {
        private Health playerHealth;
        private float rageGainBonus;
        private bool wasLowHP;

        protected override void Awake()
        {
            base.Awake();
            passiveName = "Wrath Protocol";
            playerHealth = GetComponentInParent<Health>();
        }

        protected override void OnLevelUp(int level)
        {
            rageGainBonus = level switch
            {
                1 => 0.20f,
                2 => 0.35f,
                3 => 0.50f,
                _ => 0f
            };
            Debug.Log($"[WrathProtocol] HP<50% → Rage gain +{rageGainBonus * 100}%");
        }

        private void Update()
        {
            if (playerHealth == null || rage == null || CurrentLevel == 0) return;

            float hpRatio = (float)playerHealth.CurrentHP / playerHealth.MaxHP;
            bool isLowHP = hpRatio < 0.5f;

            if (isLowHP && !wasLowHP)
            {
                rage.gainMultiplier += rageGainBonus;
                Debug.Log($"[WrathProtocol] Aktif — Rage gain +{rageGainBonus * 100}%");
            }
            else if (!isLowHP && wasLowHP)
            {
                rage.gainMultiplier -= rageGainBonus;
                Debug.Log($"[WrathProtocol] Deaktif");
            }

            wasLowHP = isLowHP;
        }
    }

    /// <summary>
    /// Tempered Fury — Savaşta Rage asla 20/30/40 altına inmez.
    /// </summary>
    public class Passive_TemperedFury : PassiveBase
    {
        private int rageFloor;

        protected override void Awake()
        {
            base.Awake();
            passiveName = "Tempered Fury";
        }

        protected override void OnLevelUp(int level)
        {
            rageFloor = level switch
            {
                1 => 20,
                2 => 30,
                3 => 40,
                _ => 0
            };
            Debug.Log($"[TemperedFury] Rage floor: {rageFloor}");
        }

        private void Update()
        {
            if (rage == null || CurrentLevel == 0) return;

            // Savaşta (oda temizlenmemişse) Rage floor uygula
            if (rage.CurrentRage < rageFloor)
            {
                rage.SetRage(rageFloor);
            }
        }
    }

    /// <summary>
    /// Berserker's Resolve — Her knockback aldığında Rage +5/+8/+12.
    /// </summary>
    public class Passive_BerserkerResolve : PassiveBase
    {
        private int ragePerKnockback;

        protected override void Awake()
        {
            base.Awake();
            passiveName = "Berserker's Resolve";
        }

        protected override void OnLevelUp(int level)
        {
            ragePerKnockback = level switch
            {
                1 => 5,
                2 => 8,
                3 => 12,
                _ => 0
            };
            Debug.Log($"[BerserkerResolve] Knockback → Rage +{ragePerKnockback}");
        }

        public void OnKnockback()
        {
            if (rage != null && ragePerKnockback > 0)
            {
                rage.AddRage(ragePerKnockback);
            }
        }
    }
}
