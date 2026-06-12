# 02 — Patch Plan by File

Bu dosya Claude/Codex için dosya bazlı uygulama planıdır. Büyük refactor değil, minimal correctness pass hedeflenir.

---

## 1. `Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs`

### Amaç
Finisher'ı crit olmaktan çıkar.

### Değiştirilecek alan
`ApplyMeleeHit` içindeki `DamagePacket.Create` çağrısı.

### Mevcut riskli niyet
```csharp
bool isFinisher = step == profile.comboLength - 1;
var packet = DamagePacket.Create(..., isFinisher, ...);
```

### Beklenen
```csharp
bool isFinisher = step == profile.comboLength - 1;
string sourceId = isFinisher ? "basic_lmb_finisher" : "basic_lmb";

var packet = DamagePacket.Create(
    dmg,
    profile.lmbDamageType,
    profile.lmbSourceType,
    owner.gameObject,
    col.gameObject,
    sourceId,
    isCrit: false,
    elementTag: profile.lmbElementTag);
```

### Ek not
SFX ve `CrossClassSkillManager` finisher trigger aynen kalabilir.

### Test
- Finisher final damage artık 1.5x crit almamalı.
- Eğer comboDamage[2] zaten büyükse sadece o kadar vurmalı.
- Telemetry crit false görmeli.

---

## 2. `Assets/Scripts/Skills/SkillRuntime.cs`

### Amaç
Damage calculation attacker ve defender statleri packet actor'larından çözsün.

### Eklenecek helper taslakları

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

Eğer Unity interface `TryGetComponent` sorun çıkarırsa:

```csharp
var provider = go.GetComponent<ICombatStatsProvider>();
```

veya provider'ı component olarak tasarla:

```csharp
public abstract class CombatStatsProvider : MonoBehaviour
{
    public abstract ClassStatRuntime CombatStats { get; }
}
```

Ama önce interface dene.

### Değiştirilecek yer
`DealDamage(Health health, DamagePacket packet, ...)` içinde:

```csharp
ClassStatRuntime attackerStats = ...
var result = DamageCalculator.Calculate(packet, attackerStats);
```

yerine:

```csharp
ClassStatRuntime attackerStats = ResolveCombatStats(packet.attacker);
ClassStatRuntime defenderStats = ResolveCombatStats(packet.target != null ? packet.target : health.gameObject);
var result = DamageCalculator.Calculate(packet, attackerStats, defenderStats);
```

### Dikkat
- Eğer attacker null ise Neutral.
- Eğer target null ise health.gameObject set edildikten sonra resolver çalışmalı.
- True damage defenderStats olsa bile DamageCalculator bypass ediyor.

### Test
- Physical damage target armor ile azalmalı.
- Ability damage target MR ile azalmalı.
- True damage azalmamalı.
- No provider path eski davranışı kırmamalı.

---

## 3. Yeni dosya: `Assets/Scripts/Balance/ICombatStatsProvider.cs`

### Amaç
Stat provider hook.

### İçerik

```csharp
namespace RIMA.Balance
{
    public interface ICombatStatsProvider
    {
        ClassStatRuntime CombatStats { get; }
    }
}
```

### Alternatif
Unity serialization veya GetComponent interface sıkıntısı çıkarsa `CombatStatsProvider` abstract MonoBehaviour kullanılabilir.

---

## 4. `Assets/Scripts/Combat/BasicAttack/ShotCadenceBehavior.cs`

### Amaç
Ranger projectile DamagePacket hattına girsin.

### Değiştirilecek alan
`ExecuteArrow` içinde projectile spawn sonrası.

### Eklenecek
```csharp
projectile.SetDamagePacket(RIMA.Balance.DamagePacket.Create(
    damage,
    profile.lmbDamageType,
    profile.lmbSourceType,
    owner.gameObject,
    null,
    charged ? "basic_lmb_charged" : "basic_lmb",
    elementTag: profile.lmbElementTag));
```

### Neden burada?
Minimal fix. `SkillRuntime.SpawnProjectile` overload yazmak daha temiz ama daha fazla çağrı etkiler.

### Test
- Ranger physPower değişince damage değişmeli.
- Armor hedefte Ranger damage azaltmalı.
- Telemetry event count Ranger hit'te artmalı.

---

## 5. `Assets/Scripts/Skills/PlayerProjectile.cs`

### Amaç
Fallback direct damage path'i mümkün olduğunca azaltmak veya güvenli yapmak.

### Minimal karar
Eğer `hasDamagePacket` false ise eski davranış kalabilir, çünkü legacy çağrılar kırılmasın. Ancak yeni core kullanan player basic/skill yolları packet set etmeli.

### Önerilen TODO
Fallback path'e yorum ekle:

```csharp
// Legacy fallback: callers should prefer SetDamagePacket so stats/armor/telemetry apply.
```

### Opsiyonel daha güvenli çözüm
Fallback path de `DamagePacket.Create(... bypassStatScaling: true)` ile `SkillRuntime.DealDamage` kullanabilir.
Ama dikkat: recursive değil, doğru overload çağrılmalı.

---

## 6. `Assets/Scripts/Balance/DamageCalculator.cs`

### Amaç
Zero damage packet 1 damage üretmesin.

### Değiştirilecek alan
`finalDamage` hesaplama.

### Öneri
```csharp
int finalDamage = packet.baseDamage <= 0
    ? 0
    : Mathf.Max(1, Mathf.RoundToInt(rawDamage));
```

### Posture overflow
Şimdilik dokunma ya da TODO ekle:

```csharp
// NOTE: postureOverflowDamage is reported for future posture/stagger systems; not applied to HP damage here.
```

### Test
- baseDamage 0 → finalDamage 0.
- baseDamage 1 düşük stat/resist sonrası minimum 1 kalabilir.
- True damage 0 → 0.

---

## 7. `Assets/Scripts/Core/Health.cs`

### Amaç
Zero damage geldiğinde event/death/popup karmaşası oluşmasın.

### Opsiyonlar

#### Opsiyon A — Health.TakeDamage 0'ı no-op yapar
```csharp
public void TakeDamage(int amount)
{
    if (IsDead || immune || amount <= 0) return;
    ...
}
```

Bu en temiz minimal davranış.

#### Opsiyon B — SkillRuntime finalDamage 0 ise TakeDamage çağırmaz
Bu da güvenli ama Health başka yerden 0 alırsa yine belirsiz kalır.

### Önerim
Opsiyon A. Çünkü zero damage global olarak no-op olmalı.

### Dikkat
`OnDamageTaken` 0 için tetiklenmeyecek. Eğer “blocked hit” VFX gerekiyorsa ayrı event ister.

---

## 8. `Assets/Scripts/UI/DirectorMode.cs`

### Amaç
TEST modunda gameplay input'u UI tarafından yenmesin.

### Değiştirilecek alan
`ApplyStateText()` veya state transition UI logic.

### Minimal fix
```csharp
bool director = State == DirectorModeState.Director;
if (rootGroup != null)
{
    rootGroup.alpha = director ? 1f : 0f;
    rootGroup.interactable = director;
    rootGroup.blocksRaycasts = director;
}
```

### Eğer TEST'te mini strip kalacaksa
Ana overlay root ile mini strip ayrılmalı:
- `DirectorRoot` interactive panel
- `DirectorTestHintRoot` raycast kapalı hint

Ama bunu bu task'ta büyütme. Input correctness birinci öncelik.

### Test
- DIRECTOR açılır, butonlar tıklanır.
- TEST'e geçince LMB/RMB player attack çalışır.
- Mouse aim UI tarafından engellenmez.
- Backtick geri açar.

---

## 9. `Assets/Scripts/Systems/PlayerClassManager.cs`

### Amaç
HP authority riskini azaltmak.

### Mevcut
`ApplyCurrentPrimaryStatsToPlayer` PlayerStats'a maxHP yazıyor, PlayerController moveSpeed yazıyor, PlayerAttack attackSpeedMult yazıyor.

### Öneri
Eğer `Health` component varsa onu da sync et:

```csharp
var health = player.GetComponent<Health>();
if (health != null)
    health.SetMaxHP(CurrentPrimaryStats.maxHP);
```

### Ama dikkat
`Health.SetMaxHP` refill ediyor. Director Stats canlı slider için bu kabul edilebilir; production class swap için de muhtemelen class değişince refill normal. Ama run içinde maxHP upgrade davranışı farklı olabilir.

### Daha güvenli uzun vade
`SetMaxHP(int hp, bool refill = true)` overload'u ekle.
Ancak bu task'ta çok dokunmak istemezsen sadece TODO bırak.

---

## 10. Stat asset numeric verification

### Amaç
10 class asset değerleri `02_B_CLASS_NUMERIC_TABLE.md` ile gerçekten eşleşiyor mu?

### Claude adımları
Repo içinde ara:
- `02_B_CLASS_NUMERIC_TABLE.md`
- `ClassStatProfile`
- `ClassStatDatabase.asset`
- `maxHP:`
- `physPower:`
- `abilityPower:`

### Beklenen çıktı
Tablo:

| Class | Table maxHP | Asset maxHP | Match |
|---|---:|---:|---|

Eğer tablo bulunamazsa:
- “Could not independently verify numeric table” diye raporla.
- Asla “birebir” deme.

---

## Patch öncelik sırası

1. A1 finisher crit fix
2. A3 Ranger DamagePacket fix
3. A2 attacker/defender stat resolver
4. B1 zero damage no-op
5. A4 Director Test raycast fix
6. HP authority note/small sync
7. Tests + report
