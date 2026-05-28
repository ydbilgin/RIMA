# Codex Review — Sprint 10 Implementation (Opus impl → Codex review)

**Date:** 2026-05-16 S86 SPRINT10
**Workflow:** Opus implement → Codex review (16-18 May override window).
**Goal:** Sprint 10 RoomTemplate + RoomBank vertical slice — spec compliance, regression, drift, risk.

---

## 1. CONTEXT

Sprint 10 task spec: `STAGING/codex_brush_sprint10_room_template_bank.md` (LIVE, patched).
Source of truth: `STAGING/sprite_strategy_FINAL_LOCK.md` §4 + §10.
Opus signoff decisions: `memory/project_s86_opus_signoff_decisions.md`.

Workflow: 16-18 May limit penceresinde Opus implement etti. Codex'in görevi: implementation review + spec drift + regression check. Tüm exit criteria (EC-1..EC-10) doğrulama.

---

## 2. NEW FILES (Sprint 10 deliverables)

**Data layer (5 new + 1 modified):**
- `Assets/Scripts/MapDesigner/Room/Data/DoorSocket.cs` — NEW (uses existing `RIMA.DoorDirection` enum from `Assets/Scripts/Core/DoorTrigger.cs`)
- `Assets/Scripts/MapDesigner/Room/Data/PlayerSpawnSocket.cs` — NEW
- `Assets/Scripts/MapDesigner/Room/Data/EnemySpawnSocket.cs` — NEW (string tierHint per OQ-4)
- `Assets/Scripts/MapDesigner/Room/Data/CameraBounds.cs` — NEW (RectInt tile-space per OQ-2)
- `Assets/Scripts/MapDesigner/Room/Data/RoomBankSO.cs` — NEW (direct ref per OQ-3, deterministic Pick, ValidateAll)
- `Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs` — MODIFIED (Sprint 9 5-field stub EXTENDED with sockets, tags, prefabRef — backward-compatible additions only)

**Validation (2 new):**
- `Assets/Scripts/MapDesigner/Room/Validation/RoomValidationIssue.cs` (ValidationSeverity + RoomValidationIssue)
- `Assets/Scripts/MapDesigner/Room/Validation/RoomTemplateValidator.cs` (Validate / ValidateBank, severity-typed, no art-quality = Error)

**Editor utilities (3 new — all `#if UNITY_EDITOR`-gated):**
- `Assets/Scripts/MapDesigner/Room/Editor/SaveLoadResults.cs` (SaveResult / LoadResult)
- `Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateSaver.cs` (in-place GUID-preserving save, deterministic child naming, rootDirOverride for tests)
- `Assets/Scripts/MapDesigner/Room/Editor/RoomTemplateLoader.cs` (LoadIntoAuthoringScene)

**Runtime (1 new):**
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomBankRuntimeTester.cs` (MonoBehaviour test harness + RoomTestResult)

**Tests (3 new):**
- `Assets/Tests/EditMode/Room/RoomTemplateSaveLoadTests.cs` (3 tests: full-field roundtrip, GUID stability on resave, no-overwrite refusal)
- `Assets/Tests/EditMode/Room/RoomBankPickTests.cs` (6 tests: deterministic pick × 10, different-seed no-crash, empty list null, elite path, duplicate detection, null ref detection)
- `Assets/Tests/PlayMode/Room/RoomBankRuntimeSpawnTests.cs` (2 UnityTests: spawn at sockets within 0.1 world unit, no-exit failure mode)

---

## 3. OPEN QUESTIONS RESOLUTION (§6 of spec)

| # | Question | Resolution applied |
|---|---|---|
| OQ-1 | DoorSocket.direction enum vs Vector2Int | **Enum** — reused existing `RIMA.DoorDirection` (not duplicated) |
| OQ-2 | CameraBounds RectInt tile-space vs Bounds world-space | **RectInt tile-space** — consistent with `bounds` field, integer-clean |
| OQ-3 | RoomBank direct ref vs AssetReference | **Direct ref** — V1 scope; API surface (Pick + GetList) future-proof for Addressable swap |
| OQ-4 | EnemySpawnSocket.tierHint string vs enum | **string tag** — preserves RoomTemplate ≠ Encounter principle |

---

## 4. EXIT CRITERIA STATUS

| EC | Criterion | Status |
|---|---|---|
| EC-1 | `dotnet build` PASS, zero new errors | ✅ Verified — RIMA.Runtime + RIMA.Editor + RIMA.Brush.Tests + RIMA.Tests.EditMode + RIMA.Tests.PlayMode all 0 errors |
| EC-2 | All existing Brush V1 tests still green (37+) | 🔄 Pre-existing 3 failures (Karar143K, EraseAllDecorative NRE, GridTileExecutor NRE) — unchanged; not Sprint 10-caused |
| EC-3 | Save → clear → reload → all fields restored | ✅ `RoomTemplateSaveLoadTests.SaveRoom_PopulatesAllFields_ReloadsIdentical` |
| EC-4 | GUID stability across resave | ✅ `SaveRoom_GuidStable_OnResave` + `result.guidPreserved` flag |
| EC-5 | RoomBank.Pick deterministic, same seed → same room | ✅ `Pick_SameSeed_ReturnsSameRoom_TenTimes` |
| EC-6 | PlayMode spawn at sockets, exit socket valid | ✅ `RoomBankRuntimeSpawnTests.RunTest_SpawnsPlayerAndEnemy_AtSocketPositions` (within 0.1 world unit per spec) |
| EC-7 | Validator returns typed `List<RoomValidationIssue>`, not exceptions | ✅ All paths return List; severity = Error / Warning / Info |
| EC-8 | No editor-only class referenced from runtime assembly | ✅ Saver/Loader/SaveLoadResults all `#if UNITY_EDITOR`-gated; RuntimeTester references only Data + Runtime |
| EC-9 | No runtime non-integer scale anywhere in Sprint 10 code | ✅ Code review: no `transform.localScale` mutation in any new file |
| EC-10 | Sorting layers Patch/Detail/Accent/Props/Entities valid (RimaSortingLayerValidator passes) | ✅ No new sorting layer code introduced; Sprint 9 R2 retrofit unchanged |

**Vertical loop sentence verification (spec §5 closing):**
"one room saves, reloads, loads through RoomBank in PlayMode, player + enemy spawn at sockets, exit data valid" — all 5 links wired via 3 test files.

---

## 5. SPEC COMPLIANCE CHECK

Codex must verify against `STAGING/codex_brush_sprint10_room_template_bank.md`:

1. **§2.1 RoomTemplateSO V1 — additive over Sprint 9 stub:** Sprint 9 5-field stub (schemaVersion/roomId/biomeId/roomType/bounds) korunmuş; Sprint 10 sadece **EKLER** (doorSockets/playerSpawn/enemySpawnSockets/cameraBounds/prefabRef/3 tag list). RoomType global `RIMA.RoomType` reuse'da kaldı mı (yeni Sprint 10 enum YOK)?
2. **§2.2 Helper types — separation:** DoorDirection enum DUPLICATE değil — `RIMA.DoorDirection` reused. PlayerSpawn/EnemySpawn/CameraBounds/DoorSocket her biri Sprint 10 scope'unda doğru tanımlı mı?
3. **§2.3 RoomBank Pick deterministic:** `Pick(roomType, seed)` aynı seed + aynı liste → aynı room garanti? Hash: `unchecked(seed * 1103515245 + 12345)`, sonra `(hash & 0x7FFFFFFF) % count` (negative seed handling).
4. **§2.4 GUID preservation:** Saver `EditorUtility.CopySerialized(template, existing)` ile in-place update kullanıyor — `AssetDatabase.DeleteAsset` + `CreateAsset` PATTERN'i YOK. GUID stability test PASS.
5. **§2.4 Deterministic child naming:** `ApplyDeterministicNaming` → `{baseName}_{i:000}` recursive. Mevcut numeric suffix'leri strip ediyor (re-naming idempotent).
6. **§2.5 Editor-only Loader:** `RoomTemplateLoader.LoadIntoAuthoringScene` `PrefabUtility.InstantiatePrefab` kullanıyor → asset link korunur.
7. **§2.7 Severity rules:** Error codes (ERR_NO_PLAYER_SPAWN / ERR_NO_EXIT_SOCKET / ERR_CAMERA_BOUNDS_NO_WALKABLE / ERR_MISSING_PREFAB_REF / ERR_MISSING_BIOME_ID / ERR_DUPLICATE_ROOM_ID + ERR_PLAYER_OUT_OF_BOUNDS + ERR_ENEMY_OUT_OF_BOUNDS) doğru emit ediliyor mu? Warning (WARN_SMALL_BOUNDS / WARN_UNUSUAL_ASPECT / WARN_NO_ENCOUNTER_TAGS) sadece advise (BLOCK değil)?
   - **NOT:** `ERR_ENEMY_IN_PROP_FOOTPRINT` Sprint 10'da DEFER — prop footprint data §4 forbidden listede (PropDefinitionSO Sprint 11+). Alternative `ERR_ENEMY_OUT_OF_BOUNDS` impl edildi. Bu doğru bir karar mı?
8. **§2.8 Test fixture:** `Assets/Data/Rooms/ShatteredKeep/combat_shatteredkeep_test_001.asset` — opsiyonel stub, Sprint 10 implementation'da YAPILMADI (test asset gerçek save call'la üretilir test runtime'da). Bu eksik mi yoksa kabul edilir mi?

---

## 6. FORBIDDEN-LIST CHECK (§4 of spec)

Codex must verify NONE of the following appear in Sprint 10 code:

- [ ] CompositionRoleMap / Natural Engine composition logic — ABSENT ✅
- [ ] Bridson Poisson sampler — ABSENT ✅
- [ ] Density mask sampling beyond FeatureMaskSO — ABSENT ✅
- [ ] PropDefinitionSO / Props Mode — ABSENT ✅
- [ ] SpriteAtlas integration / per-biome packing — ABSENT ✅
- [ ] AI tag suggestion — ABSENT ✅
- [ ] Auto-Dress / RoomBankSO integration — ABSENT ✅
- [ ] Markov clustering / sub-template rooms — ABSENT ✅

---

## 7. REVIEW CHECKLIST (§7 of spec)

- [ ] All Sprint 10 exit criteria (§5) verified — no partial credit
- [ ] `RoomTemplate ≠ Encounter` principle: no enemy type, prefab, or spawn logic owned by RoomTemplateSO (only sockets + tags)
- [ ] All IDs (`roomId`, `socketId`) deterministic, human-readable, not GUID strings
- [ ] GUID preservation actually implemented in Saver (`EditorUtility.CopySerialized` for existing; new on first save)
- [ ] `ValidationSeverity.Error` blocks hard; `Warning` allows; no art-quality check is `Error`
- [ ] No editor-only class (`RoomTemplateSaver`, `RoomTemplateLoader`, `SaveLoadResults`) referenced from any runtime assembly path (verify via grep)
- [ ] `RoomBankSO` exposes clear Sprint 11 path: `CompositionRoleMap` can query `RoomTemplateSO` sockets without restructuring `RoomBankSO`
- [ ] EditMode tests use temp paths under `Assets/TempTests/Room/...` and clean up in `[TearDown]` (verify cleanup)
- [ ] All §6 open questions resolved with documented choice

---

## 8. DELIVERABLE — Codex Output

`STAGING/codex_review_sprint10_impl_DONE.md` içinde:

1. **Verdict:** PASS / FAIL / PASS-WITH-CONDITIONS (percentage compliance)
2. **EC matrix:** EC-1..EC-10 her biri ✅/❌/🔄, kanıt satırı (dosya:satır veya test adı)
3. **Spec drift bulgular:** any §1-§7 deviation
4. **Risk list:** R1 (P0), R2 (P1)... şeklinde — yeni risk varsa
5. **Forbidden-list violation check:** §4 her madde için
6. **Recommended fixes:** if FAIL — Opus'a action item listesi (file:line precision)
7. **Sprint 11 forward-compat note:** CompositionRoleMap için API surface yeterli mi?

---

## 9. EXECUTION

Codex çalışacak (cx_dispatch.py background):
- Repo: `F:\Antigravity Projeler\2d roguelite\RIMA`
- Tool access: file read + grep + dotnet build (no write)
- Output: `CODEX_DONE.md` (last section appended) + optionally `STAGING/codex_review_sprint10_impl_DONE.md`

**ACTION:**
```bash
python "F:/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py" \
  --task-file STAGING/codex_review_sprint10_impl.md --effort high
```

---

End of Sprint 10 review spec.
