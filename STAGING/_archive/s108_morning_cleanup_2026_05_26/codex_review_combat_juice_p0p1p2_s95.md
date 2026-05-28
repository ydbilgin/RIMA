# Codex Review — Combat Juice P0/P1/P2 (S95 Audit)

## Görev
Bu 3 dosyada combat juice P0/P1/P2 implementasyonunu satır satır review et.
**Sadece read-only kod analizi yap. Değişiklik üretme.**

## Review Edilecek Dosyalar

1. **P0 — CombatEventBus integration:**
   `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs`
   - Lines 74-93: `CombatEventBus.PublishHit` ve `PublishKill` çağrıları
   - Lines 47-114: `ApplyMeleeHit` metodu tümü

2. **P1 — Dash i-frame (SetImmune + prior-state preserve):**
   `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Scripts/Player/PlayerController.cs`
   - Lines 184-198: TryDash içinde `dashWasImmune = health.IsImmune; health.SetImmune(true);`
   - Lines 220-229: Dash bitince `health.SetImmune(dashWasImmune)` restore

3. **P2 — EnemyAI windup + telegraph:**
   `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Scripts/Enemies/EnemyAI.cs`
   - Lines 87-101: `attackWindupTimer` ve `EnemyTelegraph.SpawnCircle` çağrısı
   - Lines 96-101: `state == State.Attack && attackTimer <= 0f` koşulu

## Aranan Sorunlar (öncelikli)

### A. Null reference riskleri
- `BasicAttackBehaviorBase.ApplyMeleeHit`:
  - `col.GetComponent<Health>()` → null check var, OK
  - `col.GetComponent<KnockbackReceiver>()` → null check var, OK
  - `col.gameObject == owner.gameObject` → owner null mı olabilir? Caller'da garanti mi?
  - `profile.knockbackForce` null check var, ama `profile` kendisi null olur mu? Caller'lar nereden çağırıyor (`MeleeChainBehavior`, `VeilStrikeBehavior`)?
- `PlayerController.TryDash`:
  - `health?.SetImmune(true)` — null-safe, OK
  - `dashWasImmune = health != null && health.IsImmune` — null-safe, OK
  - Update lines 220-229: dash bitince `health?.SetImmune(dashWasImmune)` restore — `health` Awake'de null olabilir, sonra dashTimer aktifken hala null mu? Edge case var mı?
- `EnemyAI.FixedUpdate`:
  - Line 92-93: `var ph = player.GetComponent<Health>(); if (ph != null) ph.TakeDamage(...)` — OK
  - Line 75: `if (player == null) return;` üstte güvende
  - ❓ `attackWindupTimer > 0f` branch'i sadece `state != Chase` iken çalışır (line 84 else). Player chase mesafesine girip dönerse windup iptal olmuyor — kasıtlı mı, yoksa bug mı?

### B. Legacy / eski çağrılar
- `BasicAttackBehaviorBase`:
  - Line 95-100: `HitStop.Instance?.FreezeLight()`, `LightPulse.Emit(...)`, `DamagePopup.Show(...)`, `CameraShake.Instance?.Shake(...)` — bunlar legacy juice. CURRENT_STATUS.md "legacy juice çağrıları korundu, cleanup ayrı task" der.
  - Sorun: Yeni `CombatEventBus.PublishHit` ile çift event mi? Subscriber tarafında çift hit-stop, çift shake riski var mı? Search yap: kim `CombatEventBus.OnHit` subscribe ediyor?

### C. Eksik null check / potansiyel NullRef
- `BasicAttackBehaviorBase.ApplyMeleeHit` line 58: `Vector2 facing = owner.Controller.FacingDirection;`
  - `owner.Controller` null check yok. `PlayerAttack.Controller` property garanti dolu mu?
- `EnemyAI.Update` line 57: `attackTimer -= Time.deltaTime;` — health.IsDead kontrolünden sonra, OK.
- `EnemyAI.FixedUpdate` line 85-103: `state == State.Attack && attackTimer <= 0f` koşulu — `attackWindupTimer` 0 iken `state` `Chase` ise hiç windup başlamayabilir. Player çok hızlı yaklaşır → State.Chase → uzaklaşır → State.Idle → windup hiç başlamaz. Saldırı dead-zone bug?

### D. State machine tutarlılığı (EnemyAI)
- `state` Update'te set ediliyor, FixedUpdate'te okunuyor — Update FixedUpdate'ten sonra çalışırsa 1 frame gecikme var.
- Windup yarım kalırsa (state Chase'e dönerse) telegraph görsel hala çiziliyor olabilir. `EnemyTelegraph.SpawnCircle` ayrı `GameObject` spawnlıyor, EnemyAI cancel etmiyor.

### E. Race / ordering
- `PlayerController.TryDash` line 187: `dashWasImmune = health != null && health.IsImmune;`
  - Eğer bir başka sistem (örn. NeutralPassives.cs satır 145) aynı anda `SetImmune(true)` çağırırsa dash bittiğinde `false`'a düşürür mü? NeutralPassives short-window immune mı, longer? Korunan state doğru mu?

## Kontrol İstenen Spesifik Sorular

1. **`BasicAttackBehaviorBase.ApplyMeleeHit`** içinde `CombatEventBus.PublishHit` ve `PublishKill` çağrıları DOĞRU mu? `HitEvent.damage` `float`, ama `finalDmg` `int` — implicit cast OK?
2. **`PlayerController.TryDash`** dash i-frame logic'i (prior state preserve) DOĞRU mu? Edge case: dash sırasında `health` null ya da `SetImmune` arada başka sistem tarafından değiştirilirse restore hatalı olabilir mi?
3. **`EnemyAI.FixedUpdate`** windup + telegraph DOĞRU mu? State.Chase'e geri dönüş windup'ı iptal etmiyor — bu kasıtlı tasarım mı, bug mu?
4. **Legacy juice çağrıları (HitStop/LightPulse/DamagePopup/CameraShake)** ile yeni `CombatEventBus` arası **çift event** riski var mı? Search yap: `CombatEventBus.OnHit` subscribe eden sınıflar.
5. **Compile-time integrity:** Şu anki 3 dosya `dotnet build` clean — bunu kabul ediyoruz. Sadece **runtime davranış doğruluğu** ve **logical bug** ara.

## Output Format

`STAGING/CODEX_DONE_combat_juice_review_s95.md` dosyasına şunu yaz:

```
## Verdict
PASS / PASS_WITH_NOTES / FAIL

## Bulgu 1: <title>
- Severity: critical / high / medium / low / info
- File: <path>:<line>
- Issue: <what>
- Suggested fix: <how>

## Bulgu 2: ...

## Çift Event Analizi
- CombatEventBus.OnHit subscriber'ları: <list>
- Çift event riski: var / yok
- Recommendation: <legacy çağrıları sub'a taşı, ya da CombatEventBus'ı geri al, vb.>

## Combat Loop Bütünlüğü Notu
- P0/P1/P2 birlikte çalışırken anlamlı bir bütün veriyor mu?
- Eksik P0/P1/P2 element var mı?
```

**Effort:** high. Read-only analysis. NO code changes.
