using UnityEngine;

namespace RIMA
{
    // ─────────────────────────────────────────────────────────────
    // 5 neutral pasif — tüm class'lara açık
    // ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Iron Body — Max HP +25/seviye.
    /// </summary>
    public class Passive_IronBody : PassiveBase
    {
        private Health playerHealth;

        protected override void Awake()
        {
            base.Awake();
            passiveName  = "Iron Body";
            playerHealth = GetComponentInParent<Health>();
        }

        protected override void OnLevelUp(int level)
        {
            if (playerHealth != null)
            {
                playerHealth.AddMaxHP(25);
                Debug.Log($"[IronBody] Max HP +25 (toplam bonus: {level * 25})");
            }
        }
    }

    /// <summary>
    /// Berserker's Blood — Rage kazanım +15%/seviye.
    /// </summary>
    public class Passive_BerserkerBlood : PassiveBase
    {
        protected override void Awake()
        {
            base.Awake();
            passiveName = "Berserker's Blood";
        }

        protected override void OnLevelUp(int level)
        {
            if (rage != null)
            {
                rage.gainMultiplier += 0.15f;
                Debug.Log($"[BerserkerBlood] Rage kazanım +15% (toplam: {level * 15}%)");
            }
        }
    }

    /// <summary>
    /// Predator's Eye — Hasar +8%/seviye (PlayerAttack üzerinden).
    /// </summary>
    public class Passive_PredatorsEye : PassiveBase
    {
        private PlayerAttack playerAttack;

        protected override void Awake()
        {
            base.Awake();
            passiveName  = "Predator's Eye";
            playerAttack = GetComponentInParent<PlayerAttack>();
        }

        protected override void OnLevelUp(int level)
        {
            if (playerAttack != null)
            {
                playerAttack.outgoingDamageMultiplier += 0.08f;
                Debug.Log($"[PredatorsEye] Hasar +8% (toplam: {level * 8}%)");
            }
        }
    }

    /// <summary>
    /// War Veteran — Düşman öldürünce Rage +5/seviye.
    /// </summary>
    public class Passive_WarVeteran : PassiveBase
    {
        private int bonusRagePerKill;

        protected override void Awake()
        {
            base.Awake();
            passiveName = "War Veteran";
        }

        protected override void OnLevelUp(int level)
        {
            bonusRagePerKill = level * 5;
            // RageSystem.OnKill event varsa bağlan — şimdilik log
            Debug.Log($"[WarVeteran] Kill → Rage +{bonusRagePerKill}");
        }

        // RageSystem.OnKill event'i geldiğinde çağrılacak
        public void OnKill()
        {
            if (rage != null && bonusRagePerKill > 0)
                rage.AddRage(bonusRagePerKill);
        }
    }

    /// <summary>
    /// Unyielding — HP %50 altına düşünce 5s hasar bağışıklığı. CD 60s.
    /// </summary>
    public class Passive_Unyielding : PassiveBase
    {
        private Health playerHealth;
        private float lastTriggerTime = -999f;
        private float cooldown = 60f;

        protected override void Awake()
        {
            base.Awake();
            passiveName  = "Unyielding";
            playerHealth = GetComponentInParent<Health>();
        }

        protected override void OnLevelUp(int level)
        {
            // Her seviye CD azaltır
            cooldown = 60f - (level - 1) * 15f; // 60 → 45 → 30s
            Debug.Log($"[Unyielding] Tetiklenme CD: {cooldown}s");
        }

        private void Update()
        {
            if (playerHealth == null || CurrentLevel == 0) return;
            if (Time.time - lastTriggerTime < cooldown) return;

            float hpRatio = (float)playerHealth.CurrentHP / playerHealth.MaxHP;
            if (hpRatio < 0.5f)
            {
                lastTriggerTime = Time.time;
                float duration = 2f + CurrentLevel * 1f; // 3/4/5s
                StartCoroutine(ImmunityWindow(duration));
            }
        }

        private System.Collections.IEnumerator ImmunityWindow(float duration)
        {
            playerHealth.SetImmune(true);
            Debug.Log($"[Unyielding] {duration}s hasar bağışıklığı aktif");
            yield return new WaitForSeconds(duration);
            if (playerHealth != null) playerHealth.SetImmune(false);
        }
    }

    /// <summary>
    /// Battle Scars — Oda temizlenince +3/+5/+8 max HP.
    /// </summary>
    public class Passive_BattleScars : PassiveBase
    {
        private Health playerHealth;
        private int hpPerRoom;

        protected override void Awake()
        {
            base.Awake();
            passiveName = "Battle Scars";
            playerHealth = GetComponentInParent<Health>();
        }

        protected override void OnLevelUp(int level)
        {
            hpPerRoom = level switch
            {
                1 => 3,
                2 => 5,
                3 => 8,
                _ => 0
            };
            Debug.Log($"[BattleScars] Oda temizlenince +{hpPerRoom} max HP");
        }

        public void OnRoomCleared()
        {
            if (playerHealth != null && hpPerRoom > 0)
            {
                playerHealth.AddMaxHP(hpPerRoom);
                Debug.Log($"[BattleScars] +{hpPerRoom} max HP (oda temizlendi)");
            }
        }
    }

    /// <summary>
    /// Adrenaline Rush — Kill sonrası 3s +%20/+%30/+%40 hareket hızı.
    /// </summary>
    public class Passive_AdrenalineRush : PassiveBase
    {
        private float speedBonus;
        private float buffDuration = 3f;
        private float buffEndTime;

        protected override void Awake()
        {
            base.Awake();
            passiveName = "Adrenaline Rush";
        }

        protected override void OnLevelUp(int level)
        {
            speedBonus = level switch
            {
                1 => 0.20f,
                2 => 0.30f,
                3 => 0.40f,
                _ => 0f
            };
            Debug.Log($"[AdrenalineRush] Kill → 3s +{speedBonus * 100}% hız");
        }

        public void OnKill()
        {
            if (player == null || CurrentLevel == 0) return;

            // Buff zaten aktifse yenile
            if (Time.time < buffEndTime)
            {
                buffEndTime = Time.time + buffDuration;
            }
            else
            {
                // Yeni buff başlat
                buffEndTime = Time.time + buffDuration;
                StartCoroutine(SpeedBuffCoroutine());
            }
        }

        private System.Collections.IEnumerator SpeedBuffCoroutine()
        {
            // TODO: PlayerController'a moveSpeedMultiplier eklenince buraya bağlanacak
            Debug.Log($"[AdrenalineRush] +{speedBonus * 100}% hız aktif");
            yield return new WaitForSeconds(buffDuration);
            Debug.Log($"[AdrenalineRush] Buff sona erdi");
        }
    }

    /// <summary>
    /// Ancient Instinct — Saldırı algılanınca hasar -%20/-%30/-%40.
    /// </summary>
    public class Passive_AncientInstinct : PassiveBase
    {
        private float damageReduction;

        protected override void Awake()
        {
            base.Awake();
            passiveName = "Ancient Instinct";
        }

        protected override void OnLevelUp(int level)
        {
            damageReduction = level switch
            {
                1 => 0.20f,
                2 => 0.30f,
                3 => 0.40f,
                _ => 0f
            };
            Debug.Log($"[AncientInstinct] Saldırı algılanınca hasar -{damageReduction * 100}%");
        }

        public float GetDamageReduction()
        {
            return CurrentLevel > 0 ? damageReduction : 0f;
        }
    }

    /// <summary>
    /// Opportunistic Strike — CC altındaki hedefe +%15/+%25/+%40 kritik şansı.
    /// </summary>
    public class Passive_OpportunisticStrike : PassiveBase
    {
        private float critBonus;

        protected override void Awake()
        {
            base.Awake();
            passiveName = "Opportunistic Strike";
        }

        protected override void OnLevelUp(int level)
        {
            critBonus = level switch
            {
                1 => 0.15f,
                2 => 0.25f,
                3 => 0.40f,
                _ => 0f
            };
            Debug.Log($"[OpportunisticStrike] CC'li hedefe +{critBonus * 100}% crit şansı");
        }

        public float GetCritBonus()
        {
            return CurrentLevel > 0 ? critBonus : 0f;
        }
    }

    /// <summary>
    /// Veteran's Scar — Oda temizlenince +2 kalıcı hasar (max +30).
    /// </summary>
    public class Passive_VeteranScar : PassiveBase
    {
        private PlayerAttack playerAttack;
        private int damagePerRoom = 2;
        private int totalDamageBonus = 0;
        private const int MaxBonus = 30;

        protected override void Awake()
        {
            base.Awake();
            passiveName = "Veteran's Scar";
            playerAttack = GetComponentInParent<PlayerAttack>();
        }

        protected override void OnLevelUp(int level)
        {
            Debug.Log($"[VeteranScar] Oda temizlenince +{damagePerRoom} kalıcı hasar (max {MaxBonus})");
        }

        public void OnRoomCleared()
        {
            if (playerAttack == null || CurrentLevel == 0) return;
            if (totalDamageBonus >= MaxBonus) return;

            totalDamageBonus += damagePerRoom;
            if (totalDamageBonus > MaxBonus)
                totalDamageBonus = MaxBonus;

            playerAttack.baseDamage += damagePerRoom;
            Debug.Log($"[VeteranScar] +{damagePerRoom} hasar (toplam: {totalDamageBonus}/{MaxBonus})");
        }
    }
}
