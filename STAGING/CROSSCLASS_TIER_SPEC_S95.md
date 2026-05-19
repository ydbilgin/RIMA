# Cross-Class T1-T4 Tier Architecture Spec — S95 (2026-05-20)

> Opus tasarım spec. Karar #122 codification + mevcut CrossClassSkillManager refactor planı. **Onay bekliyor.**

---

## T1-T4 TIER SPEC

### T1 — Commit-Beat (FREE, default)
- **Trigger:** LMB Beat 3 (combo final) on primary class
- **Chance:** 100% deterministic
- **ICD:** 1.2s per slot (per equipped cross-class skill)
- **Echo Damage:** 35% of secondary class basic attack
- **Source unlock:** Any cross-class slot equip
- **Tag:** 2s Family Tag of secondary class

### T2 — Resonance Hit (Altar-gated)
- **Trigger:** ANY LMB hit (Beat 1, 2, 3)
- **Chance:** 15% → 25% max (+2% per Altar passive, 0-5 stacks)
- **ICD:** 0.8s per slot, INDEPENDENT from T1
- **Echo Damage:** 25% of secondary basic
- **Source unlock:** Karar #7 Altar passives (10 universal)
- **Conflict:** Beat-3'te T1+T2 ikisi roll'larsa → T1 kazanır, T2 suppressed ama ICD ticks (anti-cheese)
- **Tag:** Altar-fixed mapping (örn: Whisper of Embers → Burn/Bleed)

### T3 — Empowered Skill (Q/E/R/F evolution)
- **Trigger:** Active skill cast bound to Echo evolution
- **Chance:** 100% (binding = build-choice, not random)
- **ICD:** Yok — primary skill CD gate
- **Echo Damage:** 50% of primary skill damage
- **Source unlock:** Skill Evolution data layer (Karar #5 extension), Hades-style shrine dialog
- **Tag:** Echo + Rift residue (T4 fuel)
- **Bonus:** Primary skill CD -10% while bonded

### T4 — Rift Proc Bond (3-tag detonation)
- **Trigger:** Target 3 farklı Family Tag taşıyor, next hit detonates
- **Chance:** 100% deterministic
- **ICD:** Proc'ta yok, ama 4s post-proc Family-Tag immunity (anti-cheese)
- **Damage:** 100% primary + 100% echo + 50% armor pen
- **Animation:** 0.3s slow-mo freeze (Hades-style)
- **Boss rule:** Execute disabled, flat burst only. UI: "Rift Proc Resistant"
- **Visual cap:** Max 2 Echo procs per enemy per frame across ALL tiers

---

## DATA MODEL

### `CrossClassEffectType` enum'unu TAMAMEN replace et. Yeni:

```csharp
public enum TriggerTier
{
    T1_CommitBeat,        // Beat-3 deterministic
    T2_ResonanceAltar,    // LMB roll, altar-gated
    T3_EmpoweredSkill,    // Q/E/R/F bound evolution
    T4_RiftProcBond       // 3-tag detonation
}

public enum FamilyTag
{
    Fracture,   // Warblade, Ravager, Brawler → armor reduce
    Echo,       // Elementalist, Shadowblade, Summoner → element brand
    Veil,       // Shadowblade → crit window
    Pierce,     // Ranger, Gunslinger → ranged shared
    Bleed,      // Ravager, Hexer, Ronin, Shadowblade → DoT
    Cut,        // Ronin → attack speed debuff
    Pressure,   // Ronin, Brawler shared → intimidation
    Strike,     // Brawler → contact identity
    Burn,       // Altar-only sub-family of Bleed
    Chill,      // Altar-only sub-family of Bleed
    Rift        // META: T1/T2/T3 echo residue, fuels T4
}

public enum SourceClass {
    Warblade, Elementalist, Shadowblade, Ranger, Ravager,
    Ronin, Gunslinger, Brawler, Summoner, Hexer
}
```

### Core SO:
```csharp
[CreateAssetMenu(...)]
public class CrossClassSkillData : ScriptableObject
{
    // Identity
    public string skillName;
    public string description;
    public SourceClass sourceClass;
    public Sprite icon;
    public Color ghostColor;

    // Tier
    public TriggerTier defaultTier = TriggerTier.T1_CommitBeat;

    // Family tag (echo APPLIES on hit)
    public FamilyTag appliedTag;

    // Effect payload
    public float echoDamagePct;        // 0.35 / 0.25 / 0.50 / 1.00
    public float icdSeconds;           // 1.2 / 0.8 / skill-CD / 0
    public float tagDurationSeconds = 2f;

    // T3 binding (opsiyonel)
    public bool isT3Bondable;
    public List<FamilyTag> bondAffinity;

    // VFX
    public TierVisualIntensity vfxIntensity;
}
```

### Runtime tag tracker (NEW, enemy üstünde):
```csharp
public class EnemyFamilyTagTracker : MonoBehaviour
{
    private Dictionary<FamilyTag, float> activeTags = new();
    private float t4ImmunityUntil = 0f;

    public bool TryApply(FamilyTag tag, float duration);
    public int DistinctTagCount();
    public bool ShouldFireT4();
    public void ConsumeAllTagsAndStartImmunity(float seconds = 4f);
}
```

### Per-slot ICD (replace flat slot1CD/slot2CD):
```csharp
private struct TierICD {
    public float t1Until;
    public float t2Until;
    // T3 → skill CD
    // T4 → tag tracker immunity
}
private TierICD[] slotICDs = new TierICD[2];
```

---

## MIGRATION PATH

**Verdict: Breaking refactor. Backward-compatibility yok.**

Mevcut kodun structural mismatch'i:
1. `CrossClassEffectType` enum'unda 17 stub trigger (OnDamageTaken_Resource, OnDash_Buff vs) — Karar #122'de **hiçbir karşılığı yok**. Pre-S69 zombie passive'ler
2. Per-skill `cooldown` field T1/T2 ICD'leri **karıştırıyor**. Karar #122 TIER-local ICD istiyor (T1=1.2 fix, T2=0.8 fix), per-skill olamaz
3. `CheckPassive(...)` event-bus polling; yeni model deterministic Beat-3 hook + ANY-LMB roll + skill-cast hook + tag-stack detector
4. `TriggerWarbladeBeat3RoninQuickdraw` zaten T1 pattern'i hardcoded implement ediyor

### Mevcut Warblade→Ronin combo'sunun convert'i (tek mevcut entry)

```
CrossClassSkillData asset "CCS_Ronin_Quickdraw":
  sourceClass         = Ronin
  defaultTier         = T1_CommitBeat
  appliedTag          = Cut
  echoDamagePct       = 0.35
  icdSeconds          = 1.2
  tagDurationSeconds  = 2.0
  vfxIntensity        = T1_StandardCyan
```

Bespoke `TriggerWarbladeBeat3RoninQuickdraw()` → generic Beat-3 hook (manager `MeleeChainBehavior`/`PlayerAttack`'ten çağırır). Ronin-specific ghost spawn (`ronin.TriggerQuickdrawGhost`) → `Dictionary<SourceClass, Action<Vector2>>` echoHandlers map'ı (Awake'de her class controller register eder).

### Refactor sırası (clean cut):

1. `CrossClassEffectType` enum + 17 OnX_Y hook → `_archive_faz1/`
2. `CrossClassSkillManager` strip: slot storage, tier ICDs, 4 tier entry point, ghost spawn
3. 4 yeni entry point:
   - `OnBeat3Commit(SourceClass primary, Vector2 origin, GameObject target)` ← `MeleeChainBehavior`
   - `OnAnyLMBHit(SourceClass primary, GameObject target)` ← `PlayerAttack` hit confirmation
   - `OnSkillCast(SkillID id, GameObject target)` ← skill cast resolver
   - Auto-tagging on damage application via `EnemyFamilyTagTracker.TryApply`
4. `EnemyFamilyTagTracker` enemy prefab base'ine ekle
5. Tek mevcut Warblade→Ronin call site → `OnBeat3Commit(SourceClass.Warblade, origin, target)`

**Veri kaybı yok** — tek combo var, clean convert.

---

## OPEN QUESTIONS

1. **T3 binding storage:** Karar #122 "10 class × 8 evrim noktası = 80 SO" diyor. Ayrı `SkillEvolutionData` class mı, yoksa `CrossClassSkillData` içine `T3_Bond` sub-struct mı? **Öneri: ayrı asset** (`EchoSkillEvolutionSO`) — farklı lifecycle (build-time vs run-time slot).

2. **Family tag dimensionality (12 × 12 = 144):** **144 hand-curated entry YAZMA.** Karar #122 algoritmik + player choice. T1 tag = secondary class owned tag (10 entry). T2 tag = Altar-fixed (10 entry). T3 tag = Echo + Rift residue (sabit). T4 = 3 distinct consume. **Total spec surface: ~20 entry, 144 değil.** Combinatorics runtime.

3. **ICD shared vs class-specific:** Karar #122 → **TIER-local, SLOT-local**. T1=1.2s slot1 ve slot2'ye bağımsız uygulanır. Per-class ICD dual-secondary build'leri cezalandırır + locked spec ihlal.

4. **T2 Altar count MVP:** Karar #122 Faz 2 minimal **1 Altar (Echo Cascade) ~6h** sonra 10'a expand. Veri modeli `activeAltarPassives.Count` 0-10'u kod değişmeden desteklemeli. `ResonanceAltarRuntime` registry öneri.

5. **Slot count expansion (Faz 3 "28-56 combo"):** Mevcut cap 2 slot × 10 secondary = 20 pair. 28-56 → 3 slot VEYA sub-class variant. **Slot array `List<>` olsun, fixed `slot1/slot2` değil** — Faz 3'te tekrar refactor olmasın.

6. **Backward compat:** Yok. Mevcut stub + 17 unused enum → breaking refactor daha ucuz.

---

## OPUS KARARI

**Clean-cut breaking refactor:** `CrossClassEffectType` → `TriggerTier + FamilyTag`, polling `OnX/CheckPassive` bus → 4 deterministic tier hook, tag state manager'dan per-enemy `EnemyFamilyTagTracker`'a.

**Rationale:** Karar #122 lock'lu, mevcut kod pre-S69 stub (1 hardcoded combo + 17 zombie enum). Compatibility shim semantic noise taşır Faz 2/3'e (per-skill cooldown tier ICD karıştırır, event type'lar locked tier'a map etmez). Migration cost küçük: 1 combo convert (Warblade→Ronin → T1 reference path), `TriggerWarbladeBeat3RoninQuickdraw` zaten Beat-3 commit pattern encode ediyor — generic'leşmesi yeter.

**Load-bearing decision (veri katmanı):** T1/T2 ICD ve echo damage % **TIER constant** olmalı (1.2s/0.8s, 35%/25%), per-SO field değil — Karar #122 fix etti, per-skill drift locked balance corrupt eder. Per-SO field: source class, applied family tag, T3 bondability/affinity, VFX intensity, ghost color. Slot `List<CrossClassSkillData>` (Faz 3 expansion absorb). T2 Altar count runtime registry (`ResonanceAltarRuntime.activeCount`) → 1-Altar MVP ve 10-Altar Faz 2 aynı veri yolu.

---

## ORCHESTRATOR NEXT STEP

1. **rima-doc** → bu spec `TASARIM/CROSSCLASS_TIER_SPEC.md`'ye yaz (Karar #122 anchor)
2. **Codex dispatch** (cx_dispatch via, rima-codex değil) — allowed files:
   - `Assets/Scripts/CrossClass/CrossClassSkillManager.cs`
   - `Assets/Scripts/CrossClass/CrossClassSkillData.cs`
   - YENİ: `Assets/Scripts/CrossClass/EnemyFamilyTagTracker.cs`
   - Task: "refactor per TASARIM/CROSSCLASS_TIER_SPEC.md, archive `CrossClassEffectType` enum to `_archive_faz1/`"
3. **DOKUNMA** bu refactor'da:
   - Skill evolution (T3 SO'lar) — Faz 2 ayrı ticket
   - Altar passives (T2 data) — Faz 2 ayrı ticket

**CONFLICTS WITH LOCKED RULES:** NONE — Karar #122 + #5 + #7 direct codification.

---

**Generated:** 2026-05-20 S95 LATE NIGHT
**Status:** Spec hazır, user onayı bekleniyor
