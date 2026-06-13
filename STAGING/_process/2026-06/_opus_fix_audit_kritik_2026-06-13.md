# Opus Fix Raporu — Audit DEMO-KRİTİK (D-1 / DM-1 / S-1)

Tarih: 2026-06-13 · Agent: Opus · Görev: `_cx_fix_audit_kritik.md`
Working tree'de fix'ler önceki turda uygulanmıştı; bu tur = doğrulama + kanıt + rapor. Yalnız Fix A + Fix B dosyalarına dokunuldu, git commit YAPILMADI.

---

## FIX A — PlayerAttack hiçbir yerde disable edilmiyor (D-1 + DM-1, aynı kök)

Çözüm: tek merkezî helper `PlayerController.SetPlayerActive(GameObject, bool)` → `PlayerController` + `PlayerAttack` bileşenlerini BİRLİKTE aç/kapa. Tüm çağrı noktaları bu helper'a yönlendirildi; simetri (her disable'ın karşılığı enable) garanti.

Değişen yerler:
- `Assets/Scripts/Player/PlayerController.cs:22-31` — yeni statik helper `SetPlayerActive`; `TryGetComponent` ile PlayerController + PlayerAttack `enabled = active`.
- `Assets/Scripts/Core/DeathScreenManager.cs:137` (ölüm: `DeathSequence` → `SetPlayerActive(player, false)`) ve `:162` (revive/Quick Reset: `CancelDeathForDemo` → `SetPlayerActive(player, true)`). Eski yalnız-`PlayerController.enabled` blokları kaldırıldı.
- `Assets/Scripts/UI/DirectorMode.cs` — `SetState` giriş/aynı-state dallarında (`:238`, `:246`) `SetPlayerActiveForState(state)` çağrısı; yeni özel statik metot `:267-281`: state==Director ise disable; Test'e dönüşte yalnız death AKTİF DEĞİLSE enable (ölüm penceresinde yanlış re-enable engellenir). `DemoQuickReset` (`:1039-1041`) çıkışta `SetState(Test)` + `SetPlayerActive(..., true)` ile oyuncuyu kesin saldırabilir bırakır.

## FIX B — Skill/mermi kill'leri sayılmıyor (S-1)

Çözüm: `PublishKill` semantiği TEK merkeze, `SkillRuntime` hasar hunisine taşındı. Yeni özel `SkillRuntime.PublishKillIfDead(Health, killer)` → `health.IsDead` olunca `CombatEventBus.PublishKill` (killer null ise Player'a fallback). Basic-attack hasarı zaten `SkillRuntime.DealDamage` üzerinden aktığından, çifte-publish'i önlemek için `BasicAttackBehaviorBase`'teki eski PublishKill bloğu SİLİNDİ (EnemyDeath SFX korundu).

Değişen yerler:
- `Assets/Scripts/Skills/SkillRuntime.cs:159` (`DealDamageRaw`), `:199` (`DealDamage`) — TakeDamage + telemetri + PublishSkillHit'ten sonra `PublishKillIfDead`. Her iki giriş de baştaki `health == null || health.IsDead → return 0` guard'ıyla korunuyor → bir ölüm yalnız bir kez publish.
- `Assets/Scripts/Skills/SkillRuntime.cs:206-218` — yeni `PublishKillIfDead` helper.
- `Assets/Scripts/Combat/BasicAttack/BasicAttackBehaviorBase.cs:91-92` — duplikat PublishKill bloğu kaldırıldı (artık DealDamage merkezi publish ediyor); `Sfx.EnemyDeath` korundu.

Üretimdeki tek PublishKill kaynağı artık SkillRuntime (grep doğrulandı; diğerleri yalnız `VFXBusDemo`).

---

## Doğrulama

1. Recompile → `read_console (Error)`: **0 derleme hatası** (CSxxxx yok). Konsoldaki tek "Error" = MCP paketinin kendi altyapı logu (`McpLog.cs` disposed-object, domain reload sırasında) + pre-existing sahne uyarıları (RoomLayoutData boş JSON, RoomRunDirector IsoRoomBuilder eksik) — değişikliklerle alakasız.
2. EditMode (`RIMA.Tests.EditMode`, BİR KEZ): **541 test, 11 fail** (baseline 11 ≤ şart, YENİ fail YOK). Fail'lerin tümü pre-existing ve dokunulan dosyalarla alakasız: V15g/V15h map designer (eksik PNG/dir), MCPSceneLoadModalBypass x4 (private metot imzası), PerformanceAntiPattern x2, PrefabHealth (interactRadius), SubRoomSequence (DontDestroyOnLoad edit-mode). SkillRuntime/DirectorMode/Projectile combat testi fail YOK.
3. Data-proof (execute_code):
   - (a) Director state: `Director → PC=False PA=False`; `Test → PC=True PA=True` (private `SetPlayerActiveForState` doğrudan çağrıldı, simetri PASS).
   - (a2) Death→revive simetrisi: `death → PC=False PA=False`; `revive → PC=True PA=True` (helper PASS).
   - (b) Skill-kill (`DealDamageRaw`): `deadBefore=False dealt=9999 deadAfter=True secondHitDealt=0 publishKillCount=1` → PublishKill TAM 1 kez; ölü düşmana ikinci vuruş 0 hasar + 0 publish (çifte-publish YOK).

## Sonuç

Fix A + Fix B doğrulandı: 0 derleme hatası, 11 fail (yeni yok), tüm data-proof PASS. Demo-kritik D-1/DM-1/S-1 kapandı. Git commit yapılmadı.
