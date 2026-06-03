namespace RIMA
{
    public enum RewardType { Skill, Gold, Heal, CrossClassEcho }

    /// <summary>
    /// Bir oda veya sandık ödülünü temsil eder.
    /// Skill, altın, iyileşme, cross-class Echo — ileride Item/Relic/Trait eklenebilir.
    /// </summary>
    public class RewardOffer
    {
        public RewardType type;
        public SkillData  skill;       // RewardType.Skill
        public int        goldAmount;  // RewardType.Gold
        public int        healPercent; // RewardType.Heal (max HP'nin yüzdesi, örn. 20 = %20)

        /// <summary>RewardType.CrossClassEcho — the curated guest binding to activate via
        /// <see cref="PlayerCrossClassBinding.Bind"/> when this offer is picked (B5).</summary>
        public CrossClassSkillData crossClass;

        // ── Display helpers ──────────────────────────────────────

        public string DisplayName => type switch
        {
            RewardType.Gold           => $"+{goldAmount} Altın",
            RewardType.Heal           => $"İyileşme +%{healPercent}",
            RewardType.CrossClassEcho => crossClass != null ? $"Echo of {crossClass.sourceClass}" : "Echo",
            _                         => skill?.skillName ?? "???"
        };

        public string Description => type switch
        {
            RewardType.Gold           => "Sandık ve tüccardan alışveriş için kullanılır.",
            RewardType.Heal           => $"Anında max HP'nin %{healPercent}'ini iyileştirir.",
            RewardType.CrossClassEcho => crossClass != null ? crossClass.description : "",
            _                         => skill?.description ?? ""
        };

        public SkillTier  Tier      => (type == RewardType.Skill) ? (skill?.tier ?? SkillTier.Common) : SkillTier.Common;
        public bool       IsPassive => type == RewardType.Skill && (skill?.isPassive ?? false);
        public ClassType  ClassType => (type == RewardType.Skill) ? (skill?.classType ?? ClassType.None) : ClassType.None;

        // ── Factories ────────────────────────────────────────────

        public static RewardOffer FromSkill(SkillData s) =>
            new RewardOffer { type = RewardType.Skill, skill = s };

        public static RewardOffer FromGold(int amount) =>
            new RewardOffer { type = RewardType.Gold, goldAmount = amount };

        public static RewardOffer FromHeal(int percent) =>
            new RewardOffer { type = RewardType.Heal, healPercent = percent };

        public static RewardOffer FromEcho(CrossClassSkillData echo) =>
            new RewardOffer { type = RewardType.CrossClassEcho, crossClass = echo };
    }
}
