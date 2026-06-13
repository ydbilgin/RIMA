ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — only listed files (4) BLOCKED if unclear.

NLM ACCESS: Gerekmez (lokal küçük fix batch). Direct-read: kod + bu dosya.

# Amaç
Council vision verdikti (`STAGING/playtest_caps_2026-06-13/council_vision_verdict.md`) + Fable playtest bulgularından 4 küçük cerrahi fix. Hepsi demo cilası, davranış değişimi minimal.

## Fix 1 — Void VFX okunmazlığı (council FAIL)
`Assets/Scripts/VFX/SkillVfx.cs` → `Palette(VfxElement)` switch'inde `case VfxElement.Void`: mevcut koyu mor hex'i parlak mor çekirdeğe çek (öneri: `0xB36BFF` civarı — additive'de koyu zeminde okunur, canon mor aileden). SADECE Void satırı; diğer elementlere dokunma.

## Fix 2 — Düşman HP bar full canda kırmızı (playtest bulgu #3 + council "debug karesi" sandı)
`TierHPBar`/`Fill`'i üreten script'i bul (muhtemelen EnemyTier veya TierHPBar adlı script, `Assets/Scripts/Enemies/` civarı; Fill rengi 0.85,0.15,0.15 set ediliyor). Full canda YEŞİL→hasar aldıkça KIRMIZI gradyan yap (Color.Lerp(red, green, hpRatio)) ya da en az full canda yeşil. Bar boyut/pozisyonuna dokunma.

## Fix 3 — Ölüyken Director class-switch guard (playtest bulgu #2)
`Assets/Scripts/UI/DirectorMode.cs` → `SelectDirectorClass(ClassType)` başına: oyuncu Health.IsDead ise status mesajı yaz ve return (mevcut `SetClassSkillStatus` pattern'ini kullan; yeni loc key: `director.class_skill.status.player_dead` — Loc.cs'e TR+EN ekle, Loc YENİDEN KURMA, sadece key ekle).

## Fix 4 — VFX prefab build-safety (Task #16; rift_crystal fix'iyle aynı pattern)
`Assets/Prefabs/VFX/HitSpark.prefab` + `DeathBurst.prefab` → `Assets/Resources/Prefabs/VFX/`'e TAŞI (.meta ile birlikte, GUID korunur). `SkillVfx.LoadPrefab` çağrıları zaten "Prefabs/VFX/HitSpark" Resources yolu bekliyor — taşıma sonrası Resources.Load bulacak, kod değişikliği GEREKMEZ (ama LoadPrefab'ın TÜM çağrılarını grep'le tara: başka prefab yolu varsa onları da taşı ve raporla).

## YASAK
- Bu 4 fix dışında hiçbir şey. Refactor yok, yeni abstraction yok.

## GATE
1. Derleme 0 error (`read_console`).
2. EditMode: `RIMA_EditMode_Tests` assembly koş → fail listesi BÜYÜMEMELİ (13 bilinen fail değişiklik-dışı; VFXTests + DirectorModeValidationTests yeşil kalmalı).
3. Fix 4 sonrası: `Resources.Load<GameObject>("Prefabs/VFX/HitSpark")` null DEĞİL (execute_code ile doğrula).

## RAPOR
`CODEX_DONE_<profil>.md`: dosya listesi + her fix için 1 satır diff özeti + GATE sonuçları. Belirsizlik → BLOCKED.
