# M8 Phase 2 — Vertical Slice Loop Test Checklist
**Date:** 2026-05-16 S86_LATE (UPDATED — Sprint 9/10/11 LIVE + 261/261 PASS)
**Trigger:** Sprint 9/10/11 all PASS → bu checklist user'ın PixelLab pilot ile paralel çalıştırabileceği test
**Goal:** Vertical loop FULL — paint → save → reload → RoomBank.Pick → PlayMode spawn → exit validate

---

## Pre-flight (Unity açık, Sprint 9 + 10 + 11 LIVE — S86_LATE)

- [x] Sprint 9 LIVE (BrushAtlasImporter + 21 Wang variant + 2 P0 retrofit)
- [x] Sprint 10 LIVE (RoomTemplateSO full + RoomBankSO + Saver/Loader/Validator + 11 test)
- [x] Sprint 11 LIVE (CompositionRole/Map/Generator + WangContextResolver + WallOverlayPainter context-aware + 15 test)
- [x] **Test suite 261/261 PASS** (4 pre-existing failure CLOSED 2026-05-16 Codex bd3kua3go)
- [ ] Unity Editor açık (compile clean; `mcp__unityMCP__read_console` → 0 errors)
- [ ] M8 Phase 1 LIVE: `Assets/Art/BrushAtlas/Pools/L3_Wang_ShatteredKeep.asset` (21 variants)

---

## Step 1 — Author 1 test room (manuel, Unity Editor)

**Hedef:** 1 odaya el ile L1+L2 paint + L3 Wang wall + spawn socket'leri yerleştir

- [ ] Boş bir Scene aç (`Phase1_ProceduralMap_Test` kullanılabilir veya yeni)
- [ ] Brush Window aç (RIMA → Brush → Window)
- [ ] L1 Floor brush paint — küçük bir alan (~10x8 tile)
- [ ] L2 Floor variation paint — birkaç patch
- [ ] L3 Wang wall paint — `L3_Wang_ShatteredKeep` pool kullan
- [ ] Spawn socket marker GameObject'leri ekle:
  - PlayerSpawn child GO (örn. `PlayerSpawn_01` at tile (2,2))
  - EnemySpawn child GO (örn. `EnemySpawn_01` at tile (6,5))
  - DoorSocket child GO (örn. `Door_N_01` at tile (6,9))
- [ ] Authoring root parent GO oluştur (örn. `AuthoringRoot`), tüm yukarıdakileri child et

## Step 2 — Create RoomTemplateSO + populate

- [ ] Right-click `Assets/Data/Rooms/ShatteredKeep/` → Create → RIMA → Room → RoomTemplate
- [ ] Adlandır: `combat_shatteredkeep_test_001`
- [ ] Inspector'da doldur:
  - `schemaVersion = "1.0"`
  - `roomId = "combat_shatteredkeep_test_001"`
  - `biomeId = "ShatteredKeep"`
  - `roomType = Combat`
  - `bounds = (0, 0, 12, 10)` (oda boyutu)
  - `playerSpawn.socketId = "player_spawn_01"`, `position = (2, 2)`, `facing = North`
  - `doorSockets[0] = { socketId="door_N_01", position=(6,9), direction=North, widthInTiles=2, isExit=true }`
  - `enemySpawnSockets[0] = { socketId="enemy_spawn_01", position=(6,5), tierHint="standard" }`
  - `cameraBounds.tileRect = (0, 0, 12, 10)`
  - `encounterTags = ["basic_wave"]`
- [ ] (prefabRef boş; Step 3'te SaveRoom otomatik set edecek)

## Step 3 — SaveRoom (Editor utility)

- [ ] Hierarchy'de `AuthoringRoot` GO'yu seç
- [ ] Menu: `RIMA → Room → Save Selection as Room Template...` çağır
- [ ] OpenFilePanel'da Step 2'de oluşturulan `combat_shatteredkeep_test_001.asset` seç
- [ ] SaveResult.success == true beklenir
- [ ] `Assets/Data/Rooms/ShatteredKeep/combat_shatteredkeep_test_001.prefab` oluşmalı
- [ ] `Assets/Data/Rooms/ShatteredKeep/combat_shatteredkeep_test_001.asset` (RoomTemplateSO) update edilmeli, `prefabRef` doldurulmalı
- [ ] Console'da validation issue'lar (Info + Warning OK; Error olmamalı)

## Step 4 — RoomBankSO oluştur

- [ ] Right-click `Assets/Data/Rooms/` → Create → RIMA → Room → RoomBank
- [ ] Adlandır: `RoomBank_ShatteredKeep_v1`
- [ ] Inspector'da `combatRooms` listesine `combat_shatteredkeep_test_001.asset` drag-drop
- [ ] Save

## Step 5 — Reload test (Loader)

- [ ] Sahnedeki `AuthoringRoot` GO'yu sil (veya yeni sahne aç)
- [ ] Menu: `RIMA → Room → Load Template Into Scene...`
- [ ] OpenFilePanel'da Step 2/3 template'ini seç
- [ ] Sahnede yeni instance oluşmalı, isim `combat_shatteredkeep_test_001`
- [ ] Visual'lar (paint + socket'ler) aynı görünmeli
- [ ] (Validate menu: `RIMA → Room → Validate Template` selected template için raporu görür)

## Step 6 — PlayMode integration (RoomBankRuntimeTester)

- [ ] Yeni boş sahne aç (`Sprint10_VerticalSlice_Test`)
- [ ] Camera + Directional Light ekle
- [ ] Boş GO oluştur (`RoomBankRuntimeTester`), `RoomBankRuntimeTester` component ekle
- [ ] Inspector field'ları:
  - `bank` = `RoomBank_ShatteredKeep_v1`
  - `playerPrefab` = mevcut player prefab (örn. `Assets/Prefabs/Player.prefab`) veya basit placeholder cube
  - `enemyPlaceholderPrefab` = basit cube/sphere primitive prefab
  - `testSeed` = 42
  - `roomTypeToTest` = Combat
- [ ] Play tuşuna bas
- [ ] Bir editor script veya OnEnable'da `RunTest()` çağrılmalı (Sprint 10'da otomatik trigger YOK — manuel veya test runner gerekli)

**Alternative — EditMode PlayMode test:**
- [ ] Test Runner → PlayMode → `RoomBankRuntimeSpawnTests.RunTest_SpawnsPlayerAndEnemy_AtSocketPositions` çalıştır
- [ ] Bu zaten gerçek bank'i kullanmaz (in-memory test), ama runtime flow'unu valide eder

## Step 7 — Validate loop result

**PASS criteria (M8 Phase 2):**
- [ ] Player GO sahnede (testSeed=42 deterministic)
- [ ] Player position ~ tile (2,2) world (within 0.1)
- [ ] Enemy placeholder GO sahnede position ~ tile (6,5)
- [ ] Room prefab instantiate edildi (visual paint görünür)
- [ ] DoorSocket isExit=true → result.hasExitSocket == true
- [ ] result.success == true
- [ ] Console'da exception YOK

**FAIL → Action:**
- Player position wrong → `RoomBankRuntimeTester.TileToWorld` veya `PlayerSpawnSocket.position` set yanlış
- Room prefab eksik → `template.prefabRef` set edilmedi (Step 3 SaveRoom çalışmadı)
- Exit yok → `DoorSocket.isExit` field set yanlış
- Exception → stacktrace + Codex'e dispatch

---

## M9 GO/NO-GO Karar Matrisi (UPDATED S86_LATE)

**Loop PASS:**
- Sprint 9/10/11 vertical slice **CONFIRMED green** (test taraf zaten 261/261 PASS, manuel UI flow validate)
- Sprint 12 Props Mode impl green-light (spec PREP-ready, review verdict bekleme yok bu loop için)
- Karakter pilot Warblade (Karar #145) paralel devam — engel yok
- 4 ek Wang tileset üret (biome chain) — Sprint 13 production hardening sırası
- 6 master atlas üret (L4/L5/L6 — moss/dirt/crack/rubble/rift) — Sprint 13

**Loop FAIL:**
- Architecture düzelt — yeni asset üretme
- Sprint 9 / Sprint 10 / Sprint 11 retrofit (hangisi root cause ise)
- Loop tekrar test
- **Note:** Test suite 261/261 PASS olduğu için manuel UI flow failure tahmini düşük; bekleniyor şu an save/load round-trip + RoomBankRuntimeTester PlayMode flow validation user-side

---

## Eksikler / Bilinen Defisitler (S86_LATE UPDATED)

1. ~~**Editor MenuItem for SaveRoom / LoadIntoAuthoringScene YOK.**~~ ✅ FIXED — `Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateMenu.cs` LIVE: `RIMA → Room → Save Selection as Room Template / Load Template Into Scene / Validate Template / Validate Bank` menüleri.
2. **Test fixture asset (`combat_shatteredkeep_test_001.asset`) Sprint 10'da YAZILMADI.** Bu M8 Phase 2 Step 2'de user-authored olarak yaratılır.
3. ~~**ERR_ENEMY_IN_PROP_FOOTPRINT spec'te listede ama Sprint 10'da DEFER** (PropDefinitionSO Sprint 11+).~~ → Sprint 12 spec'inde `PropFootprintValidator` ile geri eklenecek (Codex review bekleniyor); Sprint 11 LIVE'da ERR_ENEMY_OUT_OF_BOUNDS yeterli.
4. **GUID stability test geçti ama prefab GUID için ek validation gerekebilir** — `PrefabUtility.SaveAsPrefabAsset` Unity'nin internal davranışına dayanır; M8 Phase 2'de gerçek dosya GUID stable mı manuel kontrol.
5. **YENİ S86_LATE:** Sprint 11 CompositionRoleMap test loop'a entegre değil — bu checklist sadece save/load/spawn validation. Composition role-aware paint test V1.5 sprint'inde.

---

End of M8 Phase 2 checklist — Updated 2026-05-16 S86_LATE (Sprint 9/10/11 LIVE + 261/261 PASS).
