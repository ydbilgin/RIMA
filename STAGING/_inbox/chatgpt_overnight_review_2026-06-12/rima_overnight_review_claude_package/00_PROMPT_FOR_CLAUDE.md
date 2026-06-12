# CLAUDE PROMPT — RIMA Overnight Review Fix Pass

RIMA adlı Unity 2D ARPG roguelite projemde overnight autonomous code run sonrası ChatGPT comparative correctness + design review yaptı. Bu paketteki dosyaları oku ve repo üzerinde kontrollü bir fix pass yap.

Repo:
- `https://github.com/ydbilgin/RIMA`
- branch: `master`

Ana review paketindeki dosyalar:
- `01_FINDINGS_DETAILED.md`
- `02_PATCH_PLAN_BY_FILE.md`
- `03_TEST_AND_VALIDATION_CHECKLIST.md`
- `04_ACCEPTANCE_CRITERIA.md`

## Görevin

Bu review bulgularını uygulamaya çevirmek. Öncelik correctness. Görsel cila değil. Yeni feature şişirme yok. “Çalışıyor gibi” değil, combat math ve tool-state güvenliği netleşsin.

## Mutlaka odaklan

### 1) Melee finisher yanlış crit sayılıyor olabilir

Dosya:
- `Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs`

Sorun:
- `isFinisher` bool'u `DamagePacket.Create(..., isCrit: isFinisher, ...)` gibi kullanılıyor.
- Finisher/heavy hit ile crit aynı şey değil.
- DamageCalculator crit için default 1.5x çarpıyor.
- Bu combo DPS'i yanlış şişirir, telemetry ve damage popup anlamını bozar.

Beklenen:
- Finisher, crit olarak geçmemeli.
- Crit ayrı bir mekanizma yoksa `isCrit: false` olmalı.
- Finisher metadata gerekiyorsa `sourceId`, `DamageSourceType`, ya da daha sonra eklenecek ayrı `HitTag` ile taşınmalı.
- Heavy swing SFX/finisher event aynen kalabilir.

### 2) Defender armor/MR production path'e bağlanmalı

Dosyalar:
- `Assets/Scripts/Skills/SkillRuntime.cs`
- `Assets/Scripts/Balance/DamageCalculator.cs`
- `Assets/Scripts/Balance/ClassStatRuntime.cs`
- gerekirse yeni küçük provider interface dosyası

Sorun:
- `DamageCalculator.Calculate(packet, attackerStats, defenderStats)` defenderStats destekliyor.
- Ama `SkillRuntime.DealDamage` çağrısı attacker stat'i global `PlayerClassManager.Instance.CurrentPrimaryStats` üzerinden alıp defender stat geçmiyor.
- Sonuç: armor/MR formülü var ama gerçek hedef resist çoğu yolda uygulanmıyor.

Beklenen minimal çözüm:
- `SkillRuntime` içinde attacker/defender stat çözümleme fonksiyonu oluştur.
- Player için `PlayerClassManager.Instance.CurrentPrimaryStats` kullanılabilir.
- Enemy/boss/minion için şimdilik `ICombatStatsProvider` gibi küçük bir interface eklenebilir.
- Stat provider yoksa `ClassStatRuntime.Neutral` dön.
- `DamageCalculator.Calculate(packet, attackerStats, defenderStats)` kullanılmalı.

Önerilen interface:

```csharp
namespace RIMA.Balance
{
    public interface ICombatStatsProvider
    {
        ClassStatRuntime CombatStats { get; }
    }
}
```

Sonra player/enemy tarafında hemen geniş refactor yapmadan, SkillRuntime içinde fallback çözüm kur:
- attacker player ise PlayerClassManager current primary stats
- target üzerinde `ICombatStatsProvider` varsa onu kullan
- yoksa Neutral

Not:
- Gereksiz büyük enemy stat sistemi yazma. Demo kapsamını şişirme. Sadece future-proof hook yeter.

### 3) Ranger basic projectile DamagePacket hattını bypass ediyor

Dosya:
- `Assets/Scripts/Combat/BasicAttack/ShotCadenceBehavior.cs`
- `Assets/Scripts/Skills/PlayerProjectile.cs`
- gerekirse `SkillRuntime.SpawnProjectile`

Sorun:
- Elementalist projectile `SetDamagePacket(...)` çağırıyor.
- Ranger `ShotCadenceBehavior` projectile spawn ediyor ama `DamagePacket` set etmiyor.
- `PlayerProjectile` içinde packet yoksa direkt `hp.TakeDamage(finalDamage)` çalışıyor.
- Bu DamageCalculator, stat scaling, armor/MR, telemetry `OnDamageApplied` event'ini bypass ediyor.

Beklenen:
- Ranger projectile da `DamagePacket` kullansın.
- `profile.lmbDamageType`, `profile.lmbSourceType`, `profile.lmbElementTag`, attacker owner ile packet set edilmeli.
- Mümkünse `SkillRuntime.SpawnProjectile` overload'u packet alabilir; ama minimal fix Ranger içinde `projectile.SetDamagePacket(...)` eklemek.

Örnek niyet:

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

### 4) Zero-damage packet 1 hasara dönüşmemeli

Dosya:
- `Assets/Scripts/Balance/DamageCalculator.cs`
- `Assets/Scripts/Core/Health.cs` veya çağrı kontratı

Sorun:
- `DamageCalculator` final damage'i `Mathf.Max(1, RoundToInt(rawDamage))` yapıyor.
- `Health.TakeDamage` de effective damage'i minimum 1 yapıyor.
- Hasarsız status-only hit/mark/debuff ileride yanlışlıkla 1 chip damage verir.

Beklenen:
- `packet.baseDamage <= 0` ise final damage 0 olabilmeli.
- `Health.TakeDamage(0)` davranışı net olmalı: hasar event/popup/death tetiklememeli.
- Eğer mevcut sistem `TakeDamage` her zaman en az 1 kabul ediyorsa, `SkillRuntime.DealDamage` zero-damage packet'ta `TakeDamage` çağırmamalı; sadece status/event ayrı yürütülmeli.
- Büyük refactor istemiyorum. En küçük güvenli çözümü seç.

### 5) DirectorMode Test modunda overlay/raycast sızıntısı doğrulanmalı/fixlenmeli

Dosya:
- `Assets/Scripts/UI/DirectorMode.cs`

Canon:
- Director mode oyun-içi runtime tool.
- Backtick ile DIRECTOR: timeScale=0, free-cam, edit.
- TEST: timeScale=1, oynanış.
- Sahne kümülatif kalır. Bu bug değil.
- Ama TEST modunda UI input'u gameplay input'unu yememeli.

Sorun:
- `ApplyStateText()` rootGroup'u sürekli `alpha = 1`, `interactable = true`, `blocksRaycasts = true` yapıyor gibi görünüyor.
- Bu TEST modunda LMB/RMB/mouse-aim input'unu engelleyebilir.

Beklenen:
- DIRECTOR modda overlay interactive.
- TEST modda ana overlay ya kapanmalı ya da `interactable=false`, `blocksRaycasts=false` olmalı.
- İstersen küçük non-raycast status strip kalabilir ama gameplay input'u yememeli.
- Görsel doğrulama yapılamıyorsa bunu açık raporla.
- Kümülatif sahneyi temizleme davranışı ekleme; bu canon gereği intentional. Ancak Quick Reset/snapshot yoksa sadece not düş.

### 6) PlayerStats vs Health HP authority riski için karar notu

Dosyalar:
- `Assets/Scripts/Player/PlayerStats.cs`
- `Assets/Scripts/Core/Health.cs`
- `Assets/Scripts/Systems/PlayerClassManager.cs`

Sorun:
- PlayerStats ayrı float HP tutuyor.
- Production combat Health üzerinden yürüyor.
- PlayerClassManager class stat HP'yi PlayerStats'a yazıyor.
- UI başka HP, combat başka HP okuyabilir.

Beklenen:
- Büyük refactor yapmak zorunda değilsin.
- Ama en azından açık bir `TODO`/decision note veya küçük sync bridge öner.
- Eğer hızlı ve güvenliyse `PlayerClassManager.ApplyCurrentPrimaryStatsToPlayer` içinde player `Health` de güncellensin.
- Dikkat: Health.SetMaxHP currentHP'yi full refill eder. Bu Director Stats slider için istenebilir ama gameplay run sırasında istenmeyebilir. Davranışı açık seç.

## Yapma

- Known minor'ları tekrar düzeltmeye çalışma:
  - TR localization ASCII issue
  - telemetry buffer cap
  - C1 palette pilot-wave hardcode
  - Save but no Load
  - `DirectorBypassClassUnlock` guard eksikliği
- C4 Build / C5 Map gibi blocked veya ayrı karar isteyen işleri bu task'a dahil etme.
- Büyük combat rewrite yapma.
- Görsel polish yapma.
- Yeni element resist sistemi yazma.

## Test beklentisi

En azından şunları çalıştır veya çalıştıramıyorsan nedenini raporla:

1. CombatContract tests
2. DamageCalculator unit/edit tests varsa ilgili suite
3. Yeni/varsa PlayMode projectile testleri
4. Manual smoke:
   - Warblade LMB combo finisher artık crit multiplier almıyor.
   - Elementalist projectile DamagePacket ile hasar veriyor.
   - Ranger projectile DamagePacket ile hasar veriyor.
   - Armor/MR testinde Physical armor'dan, Ability MR'dan azalıyor.
   - True damage defense bypass ediyor.
   - Director TEST modunda LMB/RMB gameplay input'u UI tarafından yenmiyor.

## Çıktı formatı

Bana şu formatta rapor ver:

```md
# RIMA Review Fix Pass Report

## Summary
- ...

## Files changed
- path: ne değişti

## Critical fixes
1. ...

## Tests run
- command/result

## Manual validation
- ...

## Remaining decisions
- HP authority final decision
- snapshot/Quick Reset if not implemented
- stat profile table independent verification if still unavailable
```

Önce plan çıkar, sonra uygula. Her değişiklik minimal ve review edilebilir olsun. İnsanlığın kod tabanına verdiği hasarı artırmadan ilerle.
