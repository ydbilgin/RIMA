using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    /// <summary>
    /// Tüm skill tanımları burada. Runtime'da SkillData nesneleri oluşturur.
    /// ScriptableObject asset'lerine gerek yok.
    /// </summary>
    public class SkillDatabase : MonoBehaviour
    {
        public static SkillDatabase Instance { get; private set; }

        private readonly List<SkillData> db = new List<SkillData>();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            Build();
        }

        // ─────────────────────────────────────────────────────────
        // WARBLADE
        // ─────────────────────────────────────────────────────────
        private void Build()
        {
            WB("Iron Charge",
               "Bakış yönüne ani fırla, yoldaki tüm düşmanlara hasar ver ve geri it. Her isabet Rage +15.",
               SkillTier.Common, 3f, typeof(IronCharge));

            WB("Cleave",
               "2.2m yarıçapında döner saldırı. Mevcut Rage'in %30'u kadar ekstra hasar — Rage ne kadar doluysa o kadar güçlü. Rage 20 harcar.",
               SkillTier.Common, 5f, typeof(Cleave));

            WB("Deep Wound",
               "Yakındaki düşmanı yara: anında 20 hasar, ardından 8 saniye boyunca saniyede 8 kanama hasarı. Rage +20.",
               SkillTier.Common, 8f, typeof(DeepWound));

            WB("Sunder Mark",
               "En yakın düşmanı işaretle: 8s boyunca zırh -%40. Death Blow aktifken -%60. Rage +5.",
               SkillTier.Common, 14f, typeof(SunderMark));

            WB("Crippling Blow",
               "Yakındaki düşmana ezici darbe: 3s yavaşlatır ve sersemletir. Death Blow ile zincirlenince %600 execute hasarı.",
               SkillTier.Common, 9f, typeof(CripplingBlow));

            WB("Earthsplitter",
               "Öne 3 dalga ground crack gönderir; hedefleri Broken yapar, stun verir ve Rage +25 üretir.",
               SkillTier.Rare, 10f, typeof(Earthsplitter));

            WB("Blade Rush",
               "Yüz yönüne 20 birim/s hücum, yoldaki düşmanlara 48 hasar ver ve geri fırlat. Her isabet Rage +15.",
               SkillTier.Rare, 10f, typeof(BladeRush));

            WB("Gravity Cleave",
               "Ağır yavaş cleave: çevredeki düşmanları sana doğru çeker, ardından yüksek hasar.",
               SkillTier.Rare, 8f, typeof(GravityCleave));

            WB("Iron Counter",
               "2s savunma duruşu: bu sürede gelen saldırıyı yansıt ve düşmanı geri fırlat.",
               SkillTier.Rare, 14f, typeof(IronCounter));

            WB("Iron Crush",
               "6s buff: temel saldırı hasarı +%30. Rage 30 harcar. Bladestorm zinciriyle katlanır.",
               SkillTier.Epic, 12f, typeof(IronCrush));

            WBPassive("Ironclade Momentum",
               "Hareket halindeyken saldırı hasarı +%20/seviye. Durduğunda bonus kaybolur.",
               SkillTier.Epic, typeof(IroncladMomentum));

            WBPassive("Blood Drinker",
               "Her öldürme +8/+12/+16 HP iyileştirir. Hayatta kalma için kritik.",
               SkillTier.Common, typeof(Passive_BloodDrinker));

            WBPassive("Wrath Protocol",
               "HP %50 altındayken Rage kazanımı +%20/+%35/+%50. Son stand build'i için.",
               SkillTier.Rare, typeof(Passive_WrathProtocol));

            WBPassive("Tempered Fury",
               "Savaşta Rage asla 20/30/40 altına inmez. Rage-bağımlı build'ler için.",
               SkillTier.Rare, typeof(Passive_TemperedFury));

            WBPassive("Berserker's Resolve",
               "Her knockback aldığında Rage +5/+8/+12. Savunma-Rage dengesi.",
               SkillTier.Common, typeof(Passive_BerserkerResolve));

            WB("Battle Surge",
               "8s: her Rage harcaması max HP'nin %5'ini iyileştirir. Rage 80+ ile aktive edilirse süre 12s.",
               SkillTier.Epic, 16f, typeof(BattleSurge));

            WB("Death Blow",
               "Yalnızca HP <%30 düşmana kullanılır: %400 hasar, tüm Rage boşaltılır. Crippling Blow zinciriyle %600.",
               SkillTier.Epic, 12f, typeof(DeathBlow));

            // ─────────────────────────────────────────────────────
            // ELEMENTALİST (cross-class havuz)
            // ─────────────────────────────────────────────────────
            EL("Fireball",
               "Hedef noktaya 3m patlama: 35 ateş hasarı, 1s yanma DoT uygular. Mana 15.",
               SkillTier.Common, 5f, typeof(Fireball));

            EL("Chain Lightning",
               "İlk düşmandan 5 hedefe atlar, Shocked uygular; Chill'li hedefte zincir 7 sekmeye çıkar. Mana 25.",
               SkillTier.Rare, 7f, typeof(ChainLightning));

            EL("Glacial Spike",
               "6m buz hattı: yavaşlatır, hasar verir, Frost State +2 ve Fire State 1 tüketir.",
               SkillTier.Common, 5f, typeof(GlacialSpike));

            EL("Living Bomb",
               "En yakın hedefe 5s fitil koyar; patlayınca alana hasar verir ve kill ile komşulara kopyalanır.",
               SkillTier.Common, 8f, typeof(LivingBomb));

            EL("Blink",
               "İmleç konumuna anında ışınlan — hasar almadan escape. Mana 20.",
               SkillTier.Rare, 12f, typeof(Blink));

            EL("Arcane Blast",
               "Çevredeki tüm düşmanlara 50 arkan hasarı ver, kısa süre geri fırlat. Mana 30.",
               SkillTier.Epic, 14f, typeof(ArcaneBlast));

            EL("Frozen Orb",
               "Yavaş hareket eden frost orb; temas ettiği düşmanlara Chill uygular.",
               SkillTier.Rare, 9f, typeof(FrozenOrb));

            EL("Prism Beam",
               "Düz hatta piercing radiant beam; Light stack harcarsa hasar ve uç patlaması artar.",
               SkillTier.Rare, 7f, typeof(PrismBeam));

            EL("Meteor",
               "Kısa wind-up sonrası büyük AoE hasar ve knockdown; chilled/frozen hedefe bonus.",
               SkillTier.Rare, 14f, typeof(Meteor));

            EL("Frost Wall",
               "Öne ışık-buz duvarı yerleştirir; temas edenleri yavaşlatır ve hasar verir.",
               SkillTier.Rare, 10f, typeof(FrostWall));

            EL("Solar Flare",
               "Cursor yönünde cone radiant burst; Light State aktifse ekstra pulse gücü alır.",
               SkillTier.Epic, 12f, typeof(SolarFlare));

            EL("Blizzard",
               "Bölge bağımsız slow + tick alanı; Meteor öncesi kontrol setup'ı.",
               SkillTier.Epic, 30f, typeof(Blizzard));

            // ─────────────────────────────────────────────────────
            // SHADOWBLADE (cross-class havuz)
            // ─────────────────────────────────────────────────────
            SH("Backstab",
               "Düşmanın arkasındaysan 3x hasar ver. Yoksa normal 1x.",
               SkillTier.Common, 6f, typeof(Backstab));

            SH("Phase Step",
               "Kısa phase dash; çizgideki hedeflere hasar ve Rift Scar bırakır.",
               SkillTier.Common, 5f, typeof(PhaseStep));

            SH("Backstab Mark",
               "Yakın hedefe mark + scar koyar; stealth'ten çıkarsa yüksek hasar verir.",
               SkillTier.Common, 4f, typeof(BackstabMark));

            SH("Death Mark",
               "Hedefe gecikmeli patlama mark'ı koyar; 4s sonra alana hasar verir.",
               SkillTier.Rare, 9f, typeof(DeathMark));

            SH("Shadow Clone",
               "Kısa süreli decoy phantom; çevresine pulse hasar ve Rift Scar uygular.",
               SkillTier.Rare, 14f, typeof(ShadowClone));

            SH("Shadow Step",
               "Hedef düşmanın arkasına anında geç. Enerji 30.",
               SkillTier.Rare, 8f, typeof(ShadowStep));

            SH("Fan of Knives",
               "360° bıçak yağmuru: etraftaki tüm düşmanlara kanama uygular.",
               SkillTier.Rare, 10f, typeof(FanOfKnives));

            SH("Veil Burst",
               "Etraftaki hedeflere ardışık phase teleport-strike yapar ve scar bırakır.",
               SkillTier.Rare, 11f, typeof(VeilBurst));

            SH("Severance",
               "Rift Scar taşıyan hedeflerdeki scar'ları collapse eder; her scar Sever üretir.",
               SkillTier.Rare, 8f, typeof(Severance));

            SH("Smoke Veil",
               "Kendi çevresinde smoke state açar, stealth verir ve yakındaki düşmanları yavaşlatır.",
               SkillTier.Rare, 13f, typeof(SmokeVeil));

            SH("Chain Cull",
               "Marked/scar taşıyan hedeflerden hedefe 3 hop zıplayan execute zinciri.",
               SkillTier.Epic, 10f, typeof(ChainCull));

            SH("Shadow Pin",
               "Dagger projectile; isabette root/stun ve Rift Scar uygular.",
               SkillTier.Common, 6f, typeof(ShadowPin));

            SH("Night Aperture",
               "6s aperture state; sonraki phase giriş/çıkışları mirrored scar ritmine hazırlanır.",
               SkillTier.Epic, 18f, typeof(NightAperture));

            // ─────────────────────────────────────────────────────
            // RANGER (cross-class havuz)
            // ─────────────────────────────────────────────────────
            RG("Aimed Shot",
               "Nişan alarak yüksek hasar. Dolu Focus'ta 2x kritik şansı. Focus 20.",
               SkillTier.Common, 6f, typeof(AimedShot));

            RG("Pinning Shot",
               "Rift arrow projectile; isabette root, trap state ve mark uygular.",
               SkillTier.Common, 5f, typeof(PinningShot));

            RG("Marked Detonate",
               "Marked hedefleri patlatır; mark stack sayısına göre hasar verir ve mark'ı tüketir.",
               SkillTier.Common, 7f, typeof(MarkedDetonate));

            RG("Hunter's Step",
               "Kısa reposition dash; sonraki saldırı için crit penceresi ve Focus üretir.",
               SkillTier.Common, 6f, typeof(HuntersStep));

            RG("Bone Trap",
               "Öne trap zone kurar; alandaki hedefleri root/trap yapar ve mark'lar.",
               SkillTier.Common, 9f, typeof(BoneTrap));

            RG("Disengage",
               "Geriye doğru atla, kısa süre yavaşlatma bırak. Focus 15.",
               SkillTier.Common, 7f, typeof(Disengage));

            RG("Multi Shot",
               "Yelpaze şeklinde 5 ok ateşle. CC'li hedefe +%30 hasar. Focus 25.",
               SkillTier.Rare, 10f, typeof(MultiShot));

            RG("Sweep Volley",
               "Ön cone içinde volley hasarı verir ve hedefleri mark'lar.",
               SkillTier.Rare, 8f, typeof(SweepVolley));

            RG("Predator's Mark",
               "4m AoE içindeki hedefleri mark'lar; Focus 75+ iken hedef sayısı artar.",
               SkillTier.Rare, 11f, typeof(PredatorsMark));

            RG("Final Strike",
               "Marked + Trapped hedefe büyük execute hasarı verir; iki koşul yoksa boşa harcanmaz.",
               SkillTier.Epic, 12f, typeof(FinalStrike));

            RG("Wireline Trap",
               "Ön hatta tensioned trap kurar; geçenleri snare/trap/mark yapar.",
               SkillTier.Epic, 13f, typeof(WirelineTrap));

            // ─────────────────────────────────────────────────────
            // NEUTRAL PAsifleri (ClassType.None — herkese açık)
            // ─────────────────────────────────────────────────────
            Passive("Iron Body",
                "Max HP +25/seviye. Her seviyede o anki HP de artar.",
                SkillTier.Common, typeof(Passive_IronBody));

            Passive("Berserker's Blood",
                "Rage kazanımı +%15/seviye. Her isabet ve kill daha fazla Rage getirir.",
                SkillTier.Rare, typeof(Passive_BerserkerBlood));

            Passive("Predator's Eye",
                "Tüm hasar +%8/seviye. Katlanır — üçüncü seviyede +%24.",
                SkillTier.Rare, typeof(Passive_PredatorsEye));

            Passive("War Veteran",
                "Her öldürme +5/10/15 Rage kazandırır. Rage asla bitmez.",
                SkillTier.Common, typeof(Passive_WarVeteran));

            Passive("Unyielding",
                "HP %50 altına düşünce 3/4/5s hasar bağışıklığı. CD 60/45/30s.",
                SkillTier.Epic, typeof(Passive_Unyielding));

            Passive("Battle Scars",
                "Oda temizlenince +3/+5/+8 max HP. Kalıcı güçlenme.",
                SkillTier.Common, typeof(Passive_BattleScars));

            Passive("Adrenaline Rush",
                "Kill sonrası 3s +%20/+%30/+%40 hareket hızı. Momentum build'i için.",
                SkillTier.Rare, typeof(Passive_AdrenalineRush));

            Passive("Ancient Instinct",
                "Saldırı algılanınca hasar -%20/-%30/-%40. Savunma pasifi.",
                SkillTier.Epic, typeof(Passive_AncientInstinct));

            Passive("Opportunistic Strike",
                "CC altındaki hedefe +%15/+%25/+%40 kritik şansı. Kontrol build'i için.",
                SkillTier.Rare, typeof(Passive_OpportunisticStrike));

            Passive("Veteran's Scar",
                "Oda temizlenince +2 kalıcı hasar (max +30). Uzun run'larda güçlenir.",
                SkillTier.Common, typeof(Passive_VeteranScar));
        }

        // ── Builder helpers ──────────────────────────────────────

        private SkillData WB(string name, string desc, SkillTier tier, float cd, System.Type type)
            => Add(name, desc, tier, ClassType.Warblade, cd, type, false);

        private SkillData WBPassive(string name, string desc, SkillTier tier, System.Type type)
            => Add(name, desc, tier, ClassType.Warblade, 0f, type, true);

        private SkillData EL(string name, string desc, SkillTier tier, float cd, System.Type type)
            => Add(name, desc, tier, ClassType.Elementalist, cd, type, false);

        private SkillData SH(string name, string desc, SkillTier tier, float cd, System.Type type)
            => Add(name, desc, tier, ClassType.Shadowblade, cd, type, false);

        private SkillData RG(string name, string desc, SkillTier tier, float cd, System.Type type)
            => Add(name, desc, tier, ClassType.Ranger, cd, type, false);

        private SkillData Passive(string name, string desc, SkillTier tier, System.Type type)
            => Add(name, desc, tier, ClassType.None, 0f, type, true);

        private SkillData Add(string name, string desc, SkillTier tier,
                              ClassType cls, float cd, System.Type type, bool isPassive)
        {
            var d = ScriptableObject.CreateInstance<SkillData>();
            d.skillName   = name;
            d.description = desc;
            d.tier        = tier;
            d.classType   = cls;
            d.cooldown    = cd;
            d.skillType   = type;
            d.isPassive   = isPassive;
            db.Add(d);
            return d;
        }

        // ── Query ────────────────────────────────────────────────

        public List<SkillData> GetAll() => db;

        public SkillData FindByName(string name)
        {
            foreach (var s in db)
                if (s.skillName == name) return s;
            return null;
        }

        public List<SkillData> GetPool(ClassType primary, ClassType secondary)
        {
            var pool = new List<SkillData>();
            foreach (var s in db)
            {
                if (IsRetiredOfferSkill(s.skillName)) continue;
                if (s.classType == ClassType.None ||
                    s.classType == primary ||
                    (secondary != ClassType.None && s.classType == secondary))
                    pool.Add(s);
            }
            return pool;
        }

        private static bool IsRetiredOfferSkill(string name)
        {
            return name is
                "Cleave" or
                "Blade Rush" or
                "Chain Lightning" or
                "Arcane Blast" or
                "Backstab" or
                "Shadow Step" or
                "Fan of Knives" or
                "Aimed Shot" or
                "Disengage" or
                "Multi Shot";
        }
    }
}
