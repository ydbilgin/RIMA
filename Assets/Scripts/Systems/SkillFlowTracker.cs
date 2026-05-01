using System;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Skill kullanım akışını takip eder.
    ///
    /// İki şeyi sağlar:
    ///
    /// 1) SKILL → BASIC ATTACK zinciri
    ///    Son X saniye içinde bir skill kullanıldıysa,
    ///    bir sonraki basic attack "chained" olarak işaretlenir.
    ///    PlayerAnimator bunu okuyarak farklı animasyon oynatır.
    ///    PlayerAttack hasar bonusu uygulayabilir.
    ///
    /// 2) SKILL → SKILL zinciri
    ///    Belirli iki skill art arda kullanılırsa (window içinde)
    ///    OnSkillChain event'i fırlar — ilgili skill Execute sırasında
    ///    bunu dinleyerek "enhanced" versiyon çalıştırabilir.
    ///
    /// Kullanım:
    ///   Her SkillBase.TryActivate başarılı olduğunda NotifySkillUsed çağrılır.
    ///   SkillBase.Awake'te bu component'e ulaşır ve kaydeder.
    /// </summary>
    public class SkillFlowTracker : MonoBehaviour
    {
        [Header("Chain Windows")]
        [SerializeField] private float skillToBasicWindow = 2.5f;  // skill sonrası basic attack
        [SerializeField] private float skillToSkillWindow  = 1.8f;  // skill sonrası skill

        [Header("Basic Attack Bonus (chained)")]
        [SerializeField] private float chainedDamageBonus = 0.20f;  // %20 ekstra

        // ─── State ───────────────────────────────────────────────────────────

        private float skillToBasicTimer;
        private float skillToSkillTimer;
        private SkillBase lastSkillUsed;

        /// <summary>Son 2.5 saniyede bir skill kullanıldı → basic attack "chained".</summary>
        public bool IsChainedToBasic => skillToBasicTimer > 0f;

        /// <summary>Son 1.8 saniyede bir skill kullanıldı → bir sonraki skill "chained".</summary>
        public bool IsChainedToSkill => skillToSkillTimer > 0f;

        public SkillBase LastSkillUsed => lastSkillUsed;

        // ─── Events ──────────────────────────────────────────────────────────

        /// <summary>
        /// Skill → Skill zinciri oluşunca fırlar.
        /// (prevSkill, currentSkill) — örnek: Fireball → GlacialSpike = Combustion sinerji.
        /// </summary>
        public event Action<SkillBase, SkillBase> OnSkillChain;

        /// <summary>Her başarılı skill kullanımında fırlar. VFX ve UI feedback için genel hook.</summary>
        public event Action<SkillBase> OnSkillUsed;

        // ─── Public API ──────────────────────────────────────────────────────

        /// <summary>Her başarılı skill kullanımında çağrılır. SkillBase.TryActivate içinde.</summary>
        public void NotifySkillUsed(SkillBase skill)
        {
            if (IsChainedToSkill && lastSkillUsed != null && lastSkillUsed != skill)
                OnSkillChain?.Invoke(lastSkillUsed, skill);

            OnSkillUsed?.Invoke(skill);

            lastSkillUsed      = skill;
            skillToBasicTimer  = skillToBasicWindow;
            skillToSkillTimer  = skillToSkillWindow;
        }

        /// <summary>PlayerAttack'tan çağrılır — chain varsa damage multiplier döner.</summary>
        public float ConsumeBasicChain()
        {
            if (!IsChainedToBasic) return 1f;
            skillToBasicTimer = 0f;   // tüket (sadece bir kez bonus)
            return 1f + chainedDamageBonus;
        }

        // ─── Update ──────────────────────────────────────────────────────────

        private void Update()
        {
            if (skillToBasicTimer > 0f)  skillToBasicTimer  -= Time.deltaTime;
            if (skillToSkillTimer > 0f)  skillToSkillTimer  -= Time.deltaTime;
        }
    }
}
