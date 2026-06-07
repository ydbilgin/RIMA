# 04 — Ordered Patch Plan

## Commit 1 — `audit/live-flow-proof`

### Amaç
Canlı room flow'u kanıtlamak.

### Değişiklik
- Editor/debug audit utility veya manuel audit raporu.
- Runtime'da aktif componentleri, spawned gate/portal objelerini listeler.

### Test
- Unity açılır.
- Smoke 26/26 bozulmaz.
- Output dosyası yazılır.

---

## Commit 2 — `docs/stale-guards`

### Amaç
Eski docs agentları zehirlemesin. Evet, "doküman zehirlemesi" diye bir şey var. Harika sektör.

### Dosyalar
- `SYSTEM_MAP.md`
- `TASARIM/GDD.md`
- `TASARIM/CLASS_SILHOUETTE_BIBLE.md`
- `TASARIM/STYLE_BIBLE.md`
- `TASARIM/ROOM_MECHANICS.md`
- `STAGING/MASTER_PLAN_FINAL_2026-06-06.md`

### Değişiklik
- STALE/SUPERSEDED/FUTURE/PENDING uyarısı.
- AI_READER wins.
- CURRENT_STATUS wins for task state.

### Test
- Grep: `4-yön`, `PPU 128`, `physical door`, `Heal/Lore`, `Shop portal`, `Spirit portal`.
- Her hit ya current ya stale/future etiketi almış olmalı.

---

## Commit 3 — `gate/root-scale-stability`

### Amaç
Gate unlock anim root collider bozmasın.

### Değişiklik
- Open anim child visual üzerinde.
- Root scale sabit.
- Collider bounds regression test.

### Test
- `GateOpenDoesNotScaleRoot`
- `GateColliderBoundsStableDuringOpen`
- `GateUnlockedStillTriggersPlayerEnter`

---

## Commit 4 — `portal/live-binding`

### Koşul
Sadece live flow kesinleştikten sonra.

### Eğer RoomLoader live ise
- RoomSequenceData ya çoklu exit support alır ya RoomTemplate resolver'a bridge olur.
- Tek `Gate_Room{index}_Exit` kalkar.
- 1/2/3 portal spawn edilir.
- Choice-index korunur.

### Eğer RoomTemplateSO/IsoRoomBuilder live ise
- RoomLoader legacy guard alır.
- T3 portal prefab o path'e bağlanır.
- Generic `gate_arch` live olmaktan çıkar.

### Test
- 1 exit: center N
- 2 exit: NW+NE center boş
- 3 exit: NW+N+NE
- Boss center-only
- NE flipX rune/badge/label ters dönmez.

---

## Commit 5 — `skill/canon-audit`

### Amaç
Draft havuzu canonical ve implemented skill'lerden oluşsun.

### Değişiklik
- SkillDatabase.GetPool isImplemented filter doğrulama/fix.
- Eski isimlerin tablosu.
- Rename/filter kararları.

### Test
- `PlaceholderSkillsNeverOffered`
- `SkillDatabasePoolFiltersUnimplemented`
- `DraftOffersUseCanonicalSkillNames`

---

## Commit 6 — `weapon/production-override`

### Amaç
Silah üretimindeki boyut/pivot çelişkisini çözmek.

### Değişiklik
Yeni dosya:
`STAGING/chatgpt_weapon_pack/WEAPON_PRODUCTION_OVERRIDE_2026-06-07.md`

İçerik:
- target-size üretim
- horizontal-right
- grip pivot
- PPU64 Point
- no variants
- Brawler no WeaponDatabase
- Ronin scabbard baked/static torso attachment

---

## Commit 7 — `report/figure-gate`

### Amaç
Rapor figürleri debug/prototype kokmasın.

### Değişiklik
- fig01-05 ScreenshotMode new capture checklist.
- fig06/08/13/14 caption consistency.

### Test
- no debug markers
- no placeholder
- figure captions match visible content
