using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Shadowblade Skill 8 — Evasion
    /// 4s %100 dodge (incomingDamageMultiplier=0), her hasar girişimi +1 CP.
    /// Evasion bitince → sonraki saldırı guaranteed crit.
    /// </summary>
    public class Evasion : SkillBase
    {
        [Header("Evasion")]
        [SerializeField] private float duration = 4f;

        private ComboPointSystem combo;
        private Shadowblade_SkillController ctrl;
        private Health playerHealth;
        private float savedDamageMultiplier;

        public bool CritPending { get; private set; }
        public void ConsumeGuaranteedCrit() => CritPending = false;

        protected override void Awake()
        {
            base.Awake();
            skillName = "Evasion";
            cooldown = 18f;
            resourceCost = 0;
            combo = GetComponentInParent<ComboPointSystem>();
            ctrl  = GetComponentInParent<Shadowblade_SkillController>();
            playerHealth = GetComponentInParent<Health>();
        }

        protected override void Execute()
        {
            ctrl?.ActivateEvasion(duration);

            if (playerHealth != null)
            {
                savedDamageMultiplier = playerHealth.incomingDamageMultiplier;
                playerHealth.incomingDamageMultiplier = 0f;
                playerHealth.OnDamageTaken.AddListener(OnDodgeTrigger);
            }

            Invoke(nameof(OnEvasionEnd), duration);
        }

        private void OnDodgeTrigger(int dmg) => combo?.Add(1);

        private void OnEvasionEnd()
        {
            CritPending = true;
            if (playerHealth != null)
            {
                playerHealth.incomingDamageMultiplier = savedDamageMultiplier;
                playerHealth.OnDamageTaken.RemoveListener(OnDodgeTrigger);
            }
        }
    }
}
