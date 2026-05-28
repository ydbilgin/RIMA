# Codex Task — Combat Juice P0/P1/P2 (3 küçük edit)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Bağlam

Opus combat audit'i RIMA'da 3 kritik eksik tespit etti. Her biri küçük (5-10 satır), hepsi independent. Execute every step in this task.

Mevcut altyapı tamamen hazır — sadece bağlamalar eksik:
- `CombatEventBus`, 5 driver (HitPauseDriver, ScreenShakeDriver, CameraPunchController, VignetteFlashController, DamageNumberDriver) → KOD VAR ama hiç `PublishHit/Kill/Dash` çağrısı yok (dead code)
- `Health.SetImmune(bool)` API → VAR ama dash'te çağrılmıyor
- `EnemyTelegraph.SpawnCircle` → VAR, Mob system kullanıyor ama placeholder `EnemyAI.cs` kullanmıyor

## P0 — CombatEventBus'ı bağla

**Dosya:** `Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs`

`ApplyMeleeHit` metodu içinde (mevcut hp.TakeDamage çağrısından SONRA):
- Her başarılı hit için `CombatEventBus.PublishHit(new HitEvent { ... })` çağır
- HitEvent fields'leri uygun şekilde set et (damage, isCrit, position, attacker, target — şemaya bak)
- `step == profile.comboLength - 1` durumunda `isCrit = true` veya finisher flag set et
- Hedef öldüyse (hp.IsDead) `CombatEventBus.PublishKill(...)` da çağır

**ÖNEMLİ migration safety:** Mevcut legacy çağrıları (HitStop.Instance.FreezeLight, CameraShake.Instance.Shake, DamagePopup.Show, LightPulse.Emit) **SİLME** — çift juice olabilir ama bu QC'de yakalanır, ilk pas için event-publish'i ADD et legacy'yi BIRAK. Cleanup ayrı task.

## P1 — Dash i-frame ekle

**Dosya:** `Assets/Scripts/Player/PlayerController.cs`

`TryDash()` metodunda:
- Dash başlangıcında `GetComponent<Health>()?.SetImmune(true)` çağır
- Dash bittiğinde (mevcut isDashing=false set edildiği yerde veya dash sonu cleanup'ında) `SetImmune(false)` çağır
- Sadece dash süresince immune ol — passive_unyielding ve merciful dodge ile çakışmasın (immune flag cumulative değilse, dash sonunda set false yapma — onun yerine dash öncesi already-immune kontrolü yap)

Eğer şüphedeysen: dash öncesi `wasImmune = health.IsImmune` kaydet, dash sonunda `health.SetImmune(wasImmune)` — passive immunity'yi korur.

## P2 — EnemyAI windup ekle

**Dosya:** `Assets/Scripts/Enemies/EnemyAI.cs`

Placeholder attack flow şu an: mesafe < attackRange → anında TakeDamage. Bunu değiştir:
- Attack tetiklenmeden 0.35s önce `EnemyTelegraph.SpawnCircle(transform.position, attackRange, 0.35f)` çağır
- 0.35s windup sonrası TakeDamage uygula
- Windup sırasında düşman hareket etmesin (state machine veya basit flag)

EnemyTelegraph utility hazır, sadece çağrılması yeterli.

## Doğrulama

Tüm değişiklikler sonrası:
1. `dotnet build` veya Unity compile error yok
2. `Assets/Scripts/Combat/Demo/VFXBusDemo.cs` örneğine bakarak HitEvent şema doğru kullanıldığını kontrol et
3. Console hatasız

## Çıktı

Her dosya için diff özetini ver. Total satır sayısı: ~20-25 satır beklenti. 4 dosyadan fazla dokunma.
