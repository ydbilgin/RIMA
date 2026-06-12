# 01 — Detailed Findings from ChatGPT Review

Bu doküman, overnight run sonrası yapılan correctness + design review bulgularını Claude'un uygulayabileceği net parçalara ayırır.

## A) Kritik bulgular

### A1 — Melee finisher yanlışlıkla crit gibi hesaplanıyor olabilir

#### Dosya
- `Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs`

#### Gözlenen pattern
`ApplyMeleeHit` içinde combo step son adımsa:

```csharp
bool isFinisher = step == profile.comboLength - 1;
var packet = DamagePacket.Create(
    dmg,
    profile.lmbDamageType,
    profile.lmbSourceType,
    owner.gameObject,
    col.gameObject,
    "basic_lmb",
    isFinisher,
    elementTag: profile.lmbElementTag);
```

Burada `isFinisher` doğrudan `DamagePacket.Create` içindeki `isCrit` parametresine gidiyor. Bu da `DamageCalculator` içinde default crit multiplier ile 1.5x hasar çarpanı demek.

#### Neden ciddi?
Finisher/heavy hit ve crit aynı kavram değil.

- Finisher zaten `comboDamage` dizisinde daha yüksek değerle temsil ediliyor olabilir.
- Crit ileride ayrı chance/roll sistemiyle gelecek bir sistem olmalı.
- Damage popup, telemetry ve balancing yanlış veri üretir.
- Oyuncu “combo finali güçlü” diye düşünürken sistem onu “crit” diye raporlar.
- DPS baseline %50 civarı şişebilir.

#### Beklenen davranış
Finisher:
- heavy swing SFX tetikleyebilir,
- finisher event tetikleyebilir,
- combo step olarak güçlü olabilir,
- ama crit sayılmamalı.

#### Önerilen minimal fix
`DamagePacket.Create` çağrısında `isCrit: false` kullan.

```csharp
var packet = DamagePacket.Create(
    dmg,
    profile.lmbDamageType,
    profile.lmbSourceType,
    owner.gameObject,
    col.gameObject,
    "basic_lmb",
    isCrit: false,
    elementTag: profile.lmbElementTag);
```

Eğer finisher metadata gerekiyorsa:
- `sourceId: "basic_lmb_finisher"` kullanılabilir.
- Ya da ileride `HitTag.Finisher` eklenebilir.
- Ama bu task'ta yeni tag sistemi şart değil.

---

### A2 — DamageCalculator defender stat destekliyor ama production path defenderStats geçmiyor

#### Dosyalar
- `Assets/Scripts/Balance/DamageCalculator.cs`
- `Assets/Scripts/Skills/SkillRuntime.cs`
- `Assets/Scripts/Balance/ClassStatRuntime.cs`

#### Gözlenen pattern
`DamageCalculator.Calculate` imzası:

```csharp
Calculate(DamagePacket packet, ClassStatRuntime attackerStats = null, ClassStatRuntime defenderStats = null)
```

Formül:
- Physical → physPower scaling + armor mitigation
- Ability → abilityPower scaling + magicResist mitigation
- True → scaling 1.0 + defense bypass

Ama `SkillRuntime.DealDamage` attacker stat'i global player manager'dan alıyor ve defender stat geçmiyor.

#### Neden ciddi?
Armor/MR alanları var ama çoğu gerçek hasar yolunda etkisiz kalır.

Bu özellikle şunları bozar:
- Boss resist tuning
- Enemy armor/MR
- Player armor/MR
- Director Stats sekmesindeki defensive stat testleri
- Telemetry DPS/TTK gerçekliği

#### Beklenen davranış
`SkillRuntime` packet attacker/target üzerinden stat çözmeli.

Güvenli minimal mimari:

```csharp
namespace RIMA.Balance
{
    public interface ICombatStatsProvider
    {
        ClassStatRuntime CombatStats { get; }
    }
}
```

Sonra:

```csharp
private static ClassStatRuntime ResolveCombatStats(GameObject go)
{
    if (go == null) return ClassStatRuntime.Neutral;

    if (go.TryGetComponent<ICombatStatsProvider>(out var provider) && provider.CombatStats != null)
        return provider.CombatStats;

    if (go.CompareTag("Player") && PlayerClassManager.Instance != null)
        return PlayerClassManager.Instance.CurrentPrimaryStats ?? ClassStatRuntime.Neutral;

    return ClassStatRuntime.Neutral;
}
```

`TryGetComponent<ICombatStatsProvider>` Unity versiyonunda interface ile sorun çıkarırsa alternatif:
- `go.GetComponent<MonoBehaviour>()` taramak,
- ya da provider component base class kullanmak,
- ya da şimdilik player-only + target provider fallback yazmak.

#### Dikkat
Büyük enemy stats sistemi yazma. Sadece hook hazır olsun.

---

### A3 — Ranger projectile DamagePacket hattını bypass ediyor

#### Dosyalar
- `Assets/Scripts/Combat/BasicAttack/ShotCadenceBehavior.cs`
- `Assets/Scripts/Skills/PlayerProjectile.cs`

#### Gözlenen pattern
Elementalist:
- projectile oluşturuyor,
- `projectile.SetDamagePacket(...)` çağırıyor,
- DamageCalculator hattına giriyor.

Ranger:
- `SkillRuntime.SpawnProjectile(...)` çağırıyor,
- packet set etmiyor,
- `PlayerProjectile` fallback path'te doğrudan `hp.TakeDamage(finalDamage)` çağırıyor.

#### Neden ciddi?
Ranger basic attack:
- physPower scaling almaz,
- armor/MR almaz,
- telemetry `OnDamageApplied` event'ine girmeyebilir,
- damage source breakdown eksik kalır,
- Director Stats sekmesinde physPower slider etkisiz görünebilir.

Bu, “iki basic attack archetype bağlandı” hedefinin eksik kaldığını gösterir. Elementalist bağlı, Ranger bağlı değil.

#### Beklenen minimal fix
`ShotCadenceBehavior.ExecuteArrow` içinde projectile oluşturulduktan sonra packet set et:

```csharp
projectile.SetDamagePacket(DamagePacket.Create(
    damage,
    profile.lmbDamageType,
    profile.lmbSourceType,
    owner.gameObject,
    null,
    "basic_lmb",
    elementTag: profile.lmbElementTag));
```

Eğer charged shot farklı sourceId istiyorsa:
- `"basic_lmb_charged"` kullanılabilir.

---

### A4 — Director Test mode overlay/raycast sızıntısı riski

#### Dosya
- `Assets/Scripts/UI/DirectorMode.cs`

#### Canon
Sandbox/Director decision:
- DIRECTOR: timeScale 0, free-cam, edit.
- TEST: timeScale 1, oynanış.
- Sahne kümülatif kalır.
- UI gameplay input'unu yememeli.

#### Gözlenen risk
`ApplyStateText()` içinde root CanvasGroup sürekli aktif/interactable/raycast açık kalıyor gibi görünüyor.

#### Neden ciddi?
TEST modunda:
- LMB/RMB saldırı input'u UI tarafından yenebilir.
- Mouse aim bozulabilir.
- UI panel görünür kalabilir.
- “Visual unverified” commit olduğu için bu mutlaka manual doğrulanmalı.

#### Beklenen davranış
- DIRECTOR mod: overlay visible + interactable + blocksRaycasts true.
- TEST mod: gameplay input serbest.
- Eğer küçük status strip kalacaksa raycastTarget false olmalı.
- Ana root kapatılacaksa alpha 0 / interactable false / blocksRaycasts false.

#### Minimal fix taslağı

```csharp
private void ApplyStateText()
{
    bool director = State == DirectorModeState.Director;

    if (rootGroup != null)
    {
        rootGroup.alpha = director ? 1f : 0f;
        rootGroup.interactable = director;
        rootGroup.blocksRaycasts = director;
    }

    // Optional: separate non-raycast test hint if needed
}
```

Fakat dikkat:
- Eğer Start button TEST modda görünmeli deniyorsa ayrı root/panel tasarımı gerekir.
- İlk correctness fix için input yemesin yeter.

---

## B) Orta bulgular

### B1 — Zero-damage packet 1 hasara dönüşüyor

#### Dosyalar
- `DamageCalculator.cs`
- `Health.cs`
- `SkillRuntime.cs`

#### Sorun
`DamageCalculator` final damage'i minimum 1 yapıyor.
`Health.TakeDamage` de effective damage'i minimum 1 yapıyor.

Bu, “status-only” veya “mark-only” effect'lerin yanlışlıkla damage vermesine yol açar.

#### Öneri
- `packet.baseDamage <= 0` ise `finalDamage = 0`.
- `SkillRuntime.DealDamage` finalDamage 0 ise `health.TakeDamage` çağırmasın.
- Status effect uygulaması ayrı callback/onHit içinde devam edebilir.

#### Risk
Mevcut sistemde bazı testler “0 bile 1 vurur” varsayıyorsa değişir. Bu yüzden test ekle.

---

### B2 — postureOverflowDamage hesaplanıyor ama kullanılmıyor

#### Dosya
- `DamageCalculator.cs`

#### Sorun
Identity cap üstü overflow hesaplanıyor:

```csharp
postureOverflowDamage = ...
```

Ama production combat path'te bu değer herhangi bir posture/stagger sisteme akmıyor.

#### Öneri
Bu task'ta büyük posture sistemi yazma.
Sadece:
- ya yorum/TODO ile debug-only olduğu netleştir,
- ya da result alanını ileride `PostureSystem` tüketir diye document et.

---

### B3 — PlayerStats vs Health iki HP otoritesi riski

#### Dosyalar
- `PlayerStats.cs`
- `Health.cs`
- `PlayerClassManager.cs`

#### Sorun
PlayerStats ayrı HP tutuyor. Health ayrı HP tutuyor. Combat Health kullanıyor. Class stats PlayerStats'a yazılıyor.

Bu ileride:
- UI/combat HP ayrışması,
- Director Stats slider yanlış izlenimi,
- heal/death event karmaşası üretir.

#### Öneri
Bu task'ta büyük refactor şart değil. Ama karar notu şart.

Minimal seçenekler:
1. `PlayerStats` UI wrapper olur, gerçek Health ile sync eder.
2. `Health` tek otorite olur, PlayerStats kaldırılır/adapter olur.
3. Demo için `PlayerClassManager.ApplyCurrentPrimaryStatsToPlayer` hem PlayerStats hem Health set eder.

En güvenli kısa demo fix:
- PlayerStats kalır.
- Health varsa maxHP de güncellenir.
- Refill davranışı Director Stats'ta açıkça kabul edilir.

Ama production run sırasında maxHP değişimi refill etmeli mi ayrı karar.

---

### B4 — SkillRuntime attacker stat global PlayerClassManager'a bağlı

#### Dosya
- `SkillRuntime.cs`

#### Sorun
Her damage packet, attacker kim olursa olsun player primary stats ile hesaplanma riski taşıyor.

#### Etkilenenler
- Minion
- Item proc
- Enemy projectile
- Trap
- Director spawned actor
- Summoner unit

#### Öneri
A2'deki `ResolveCombatStats(packet.attacker)` bunu da çözer.

---

## C) Canon/design sapmaları

### C1 — Kümülatif sahne bug değil

Director canon “sahne kümülatif” diyor. Bu yüzden Director→Test çıkışında spawn'ların kalması bug olarak raporlanmamalı.

Fakat snapshot/Quick Reset canon içinde bekleniyor. Eğer yoksa:
- “cumulative intended”
- “Quick Reset/snapshot missing”
diye raporlanmalı.

### C2 — Director guard doğru ama state mutasyonları restore edilmeli

`#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR` mantıklı.
Ama Director:
- class seçiyor,
- skill slot atıyor,
- runtime stat değiştiriyor,
- timeScale değiştiriyor,
- spawned objects yaratıyor.

Bu yüzden restore/reset mekanizması yoksa uzun playtest oturumunda state drift olur.

### C3 — Elementalist Light vs Lightning kararı

Kodda `ElementalistElement.Light` → `ElementTag.Lightning` map ediliyor olabilir.
Eğer tasarımda üçüncü Elementalist state gerçekten “Lightning” ise sorun yok.
Eğer Light/Holy ise `ElementTag.Light` kullanılmalı.

Bu task'ta karar verilemiyorsa sadece raporla.

### C4 — 10 class stat asset numeric equality bağımsız doğrulanamadı

ClassStatDatabase 10 profile referansı taşıyor gibi görünüyor.
Ama `02_B_CLASS_NUMERIC_TABLE.md` exact path ve asset dosya isimleri erişimde netleşmediği için ChatGPT bağımsız birebir numeric doğrulama yapamadı.

Claude repo içinde dosya arama/grep ile bunu doğrulamalı:
- `02_B_CLASS_NUMERIC_TABLE.md`
- `ClassStatProfile` assetleri
- GUID referansları

Eğer tablo bulunamazsa raporla, “assumed pass” deme.
