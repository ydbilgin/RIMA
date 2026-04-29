using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Hades-tarzı context-aware draft kompozisyonu.
    ///
    /// Senaryo seçimi (öncelik sırası):
    ///   1. NewUnlock   — secondary yeni açıldıysa (nextDraftIsUnlock flag)
    ///   2. UpgradeFocus — ≥2 yükseltilebilir pasif ve player yeterince gelişmişse
    ///   3. PassiveFocus — aktif slotlar dolmak üzereyken
    ///   4. Normal       — varsayılan (2 aktif + 1 pasif)
    ///   5. PureActive   — pasif havuzu tamamen boşsa
    ///
    /// Tier ağırlıkları: Common 55 / Rare 30 / Epic 12 / Legendary 3
    /// </summary>
    public class SkillOfferGenerator : MonoBehaviour
    {
        [Header("Fallback — SkillDatabase yoksa")]
        [SerializeField] private SkillData[] allSkills;
        [SerializeField] private int offerCount = 3;

        /// <summary> DraftManager, secondary unlock sonrasında true yapar. </summary>
        [HideInInspector] public bool nextDraftIsUnlock;

        // ── Public API ───────────────────────────────────────────

        public List<RewardOffer> GenerateOffers(List<SkillData> ownedActives,
                                              ClassType primary, ClassType secondary,
                                              int maxActiveSlots = 4, int roomDepth = 1)
        {
            // ── Havuzu al ────────────────────────────────────────
            var source = GetSource(primary, secondary);
            source = FilterByDepth(source, roomDepth, primary);
            if (source.Count == 0)
            {
                Debug.LogWarning("[SkillOfferGen] Havuz boş.");
                return new List<RewardOffer>();
            }

            // ── Kategorize ───────────────────────────────────────
            var newActives     = new List<SkillData>();
            var newPassives    = new List<SkillData>();
            var upgradePassives= new List<SkillData>();

            foreach (var s in source)
            {
                if (s.isPassive)
                {
                    int lvl = DraftManager.GetPassiveLevel(s.skillName);
                    if (lvl >= PassiveBase.MaxLevel) continue;
                    if (lvl > 0) upgradePassives.Add(s);
                    else         newPassives.Add(s);
                }
                else
                {
                    if (!ownedActives.Contains(s)) newActives.Add(s);
                }
            }

            // ── Senaryo seç ──────────────────────────────────────
            var scenario = ChooseScenario(
                newActives, newPassives, upgradePassives,
                ownedActives.Count, maxActiveSlots, secondary);

            // ── Skill teklif üret ─────────────────────────────────
            var skillOffers = BuildOffers(scenario, newActives, newPassives, upgradePassives,
                                          primary, secondary);

            // ── RewardOffer'a dönüştür ────────────────────────────
            var offers = new List<RewardOffer>(skillOffers.Count);
            foreach (var s in skillOffers) offers.Add(RewardOffer.FromSkill(s));

            nextDraftIsUnlock = false;
            Shuffle(offers);
            return offers;
        }

        // ── Senaryo seçimi ───────────────────────────────────────

        private enum Scenario
        {
            Normal,        // 2 yeni aktif + 1 yeni pasif
            PassiveFocus,  // 1 aktif + 2 pasif (upgrade veya yeni)
            UpgradeFocus,  // 2 pasif upgrade + 1 yeni aktif
            NewUnlock,     // 1 secondary + 1 neutral pasif + 1 primary
            PureActive,    // 3 aktif (pasif havuzu boş)
        }

        private Scenario ChooseScenario(
            List<SkillData> newActives, List<SkillData> newPassives,
            List<SkillData> upgradePassives,
            int ownedActiveCount, int maxSlots, ClassType secondary)
        {
            bool hasAnyPassive = newPassives.Count > 0 || upgradePassives.Count > 0;
            int slotsLeft = maxSlots - ownedActiveCount;

            // Secondary yeni açıldı
            if (nextDraftIsUnlock && secondary != ClassType.None)
                return Scenario.NewUnlock;

            // Pasif yoksa → sadece aktif
            if (!hasAnyPassive)
                return Scenario.PureActive;

            // Slot dolmak üzere (≤1 kaldı) → pasif önce
            if (slotsLeft <= 1)
                return Scenario.PassiveFocus;

            // ≥2 upgrade var ve player yeterince gelişmiş (≥2 aktif aldı)
            if (upgradePassives.Count >= 2 && ownedActiveCount >= 2)
            {
                // %35 upgrade odaklı, arttır
                float upgradeChance = 0.25f + upgradePassives.Count * 0.05f;
                upgradeChance = Mathf.Min(upgradeChance, 0.50f);
                if (Random.value < upgradeChance) return Scenario.UpgradeFocus;
            }

            // Çok aktif aldıysa pasif önceliği artır
            float passiveFocusChance = 0.10f + ownedActiveCount * 0.06f;
            passiveFocusChance = Mathf.Min(passiveFocusChance, 0.30f);
            if (Random.value < passiveFocusChance) return Scenario.PassiveFocus;

            return Scenario.Normal;
        }

        // ── Teklif build ─────────────────────────────────────────

        private List<SkillData> BuildOffers(
            Scenario scenario,
            List<SkillData> newActives, List<SkillData> newPassives,
            List<SkillData> upgradePassives,
            ClassType primary, ClassType secondary)
        {
            var offers = new List<SkillData>();

            switch (scenario)
            {
                // ─ Normal: 2 aktif + 1 pasif ─────────────────────
                case Scenario.Normal:
                    TakeWeighted(newActives, offers, 2);
                    TakePassive(newPassives, upgradePassives, offers, 1, preferUpgrade: false);
                    break;

                // ─ Pasif odaklı: 1 aktif + 2 pasif ──────────────
                case Scenario.PassiveFocus:
                    TakeWeighted(newActives, offers, 1);
                    TakePassive(newPassives, upgradePassives, offers, 2, preferUpgrade: true);
                    break;

                // ─ Upgrade odaklı: 2 upgrade + 1 yeni aktif ─────
                case Scenario.UpgradeFocus:
                    TakePassive(null, upgradePassives, offers, 2, preferUpgrade: true);
                    TakeWeighted(newActives, offers, 1);
                    // Yeni pasif de ekle eğer 3 teklif dolmadıysa
                    if (offers.Count < offerCount)
                        TakePassive(newPassives, upgradePassives, offers, 1, preferUpgrade: false);
                    break;

                // ─ Yeni açılım: secondary + neutral + primary ────
                case Scenario.NewUnlock:
                    // 1 secondary aktif
                    var secActives = newActives.FindAll(s => s.classType == secondary);
                    if (secActives.Count > 0)
                    {
                        var pick = WeightedPick(secActives);
                        offers.Add(pick);
                        newActives.Remove(pick);
                    }
                    // 1 neutral pasif
                    var neutralPassives = newPassives.FindAll(s => s.classType == ClassType.None);
                    if (neutralPassives.Count > 0)
                    {
                        var pick = neutralPassives[Random.Range(0, neutralPassives.Count)];
                        offers.Add(pick);
                    }
                    // 1 primary aktif (kalanı doldur)
                    var priActives = newActives.FindAll(s => s.classType == primary);
                    if (priActives.Count > 0 && offers.Count < offerCount)
                    {
                        offers.Add(WeightedPick(priActives));
                    }
                    // Hâlâ eksikse kalan havuzdan doldur
                    TakeWeighted(newActives, offers, offerCount - offers.Count);
                    break;

                // ─ Saf aktif: 3 aktif ────────────────────────────
                case Scenario.PureActive:
                    TakeWeighted(newActives, offers, offerCount);
                    break;
            }

            // Slot 3'e ulaşamazsa kalan havuzdan doldur (güvenlik)
            if (offers.Count < offerCount)
            {
                TakeWeighted(newActives, offers, offerCount - offers.Count);
                TakePassive(newPassives, upgradePassives, offers,
                            offerCount - offers.Count, preferUpgrade: false);
            }

            return offers;
        }

        // ── Yardımcılar ──────────────────────────────────────────

        /// <summary>
        /// Pasif seç: upgradePassives ve/veya newPassives'tan.
        /// preferUpgrade=true → önce upgrade, sonra yeni.
        /// </summary>
        private static void TakePassive(
            List<SkillData> newP, List<SkillData> upgradeP,
            List<SkillData> result, int count, bool preferUpgrade)
        {
            for (int i = 0; i < count; i++)
            {
                if (result.Count >= 3) break;

                List<SkillData> primary = preferUpgrade ? upgradeP : newP;
                List<SkillData> fallback = preferUpgrade ? newP : upgradeP;

                SkillData pick = null;
                if (primary != null && primary.Count > 0)
                {
                    // Upgrade pasiflerini seviyeye göre ağırlıkla
                    // (seviye 1 > seviye 2, daha yükseltilebilir olanı önce sun)
                    if (primary == upgradeP)
                        pick = UpgradePriorityPick(primary);
                    else
                        pick = primary[Random.Range(0, primary.Count)];
                    primary.Remove(pick);
                }
                else if (fallback != null && fallback.Count > 0)
                {
                    pick = fallback[Random.Range(0, fallback.Count)];
                    fallback.Remove(pick);
                }

                if (pick != null && !result.Contains(pick))
                    result.Add(pick);
            }
        }

        /// <summary> Yükseltilebilir pasiflerden: seviyesi düşük olanı öncelikle sun. </summary>
        private static SkillData UpgradePriorityPick(List<SkillData> upgradePool)
        {
            // Seviye 1 > seviye 2 — daha "erken" olanlar daha çok çıksın
            float total = 0f;
            var weights = new float[upgradePool.Count];
            for (int i = 0; i < upgradePool.Count; i++)
            {
                int lvl = DraftManager.GetPassiveLevel(upgradePool[i].skillName);
                weights[i] = lvl == 1 ? 3f : 1f; // seviye 1 = 3x şans
                total += weights[i];
            }
            float roll = Random.Range(0f, total);
            float cum = 0f;
            for (int i = 0; i < upgradePool.Count; i++)
            {
                cum += weights[i];
                if (roll <= cum) return upgradePool[i];
            }
            return upgradePool[upgradePool.Count - 1];
        }

        /// <summary> Ağırlıklı aktif seç, result'a ekle. </summary>
        private static void TakeWeighted(List<SkillData> pool, List<SkillData> result, int count)
        {
            var remaining = new List<SkillData>(pool);
            for (int i = 0; i < count; i++)
            {
                if (result.Count >= 3 || remaining.Count == 0) break;
                var pick = WeightedPick(remaining);
                result.Add(pick);
                remaining.Remove(pick);
                pool.Remove(pick); // ana havuzdan da çıkar
            }
        }

        private static SkillData WeightedPick(List<SkillData> pool)
        {
            float total = 0f;
            foreach (var s in pool) total += TierWeight(s.tier);
            float roll = Random.Range(0f, total), cum = 0f;
            foreach (var s in pool) { cum += TierWeight(s.tier); if (roll <= cum) return s; }
            return pool[pool.Count - 1];
        }

        private static float TierWeight(SkillTier t) => t switch
        {
            SkillTier.Common    => 55f,
            SkillTier.Rare      => 27f,
            SkillTier.Epic      => 12f,
            SkillTier.Mythic    => 5f,
            SkillTier.Legendary => 3f,
            _                   => 55f,
        };

        /// <summary>
        /// Run-depth lock: Epic/Mythic Oda 3+, Legendary Oda 7+.
        /// Mythic primary-only: o class'ı primary seçmemişsen havuza girmez.
        /// </summary>
        private static List<SkillData> FilterByDepth(List<SkillData> source, int room, ClassType primary)
        {
            var filtered = new List<SkillData>(source.Count);
            foreach (var s in source)
            {
                if ((s.tier == SkillTier.Epic || s.tier == SkillTier.Mythic) && room < 3)
                    continue;
                if (s.tier == SkillTier.Legendary && room < 7)
                    continue;
                // Mythic: primary-only mastery
                if (s.tier == SkillTier.Mythic && s.classType != ClassType.None && s.classType != primary)
                    continue;
                filtered.Add(s);
            }
            return filtered;
        }

        private List<SkillData> GetSource(ClassType primary, ClassType secondary)
        {
            if (SkillDatabase.Instance != null)
                return SkillDatabase.Instance.GetPool(primary, secondary);

            var list = new List<SkillData>();
            if (allSkills == null) return list;
            foreach (var s in allSkills)
            {
                if (s == null) continue;
                if (s.classType == ClassType.None || s.classType == primary ||
                    (secondary != ClassType.None && s.classType == secondary))
                    list.Add(s);
            }
            return list;
        }

        private static void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
