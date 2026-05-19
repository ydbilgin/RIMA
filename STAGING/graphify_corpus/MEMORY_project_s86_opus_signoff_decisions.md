---
name: s86-opus-signoff-decisions
description: "S86 PREP-3 Opus signoff — 6 Codex review blocker'a karar. Sprint 9-10 + Wang tileset spec için bağlayıcı."
metadata: 
  node_type: memory
  type: project
  originSessionId: dacb3dfe-b4f5-4efb-af7d-48afdea966cd
---

# S86 PREP-3 — Opus Signoff Decisions (6 Codex Review Blocker Resolved)

**Date:** 2026-05-16
**Authority:** Opus orchestrator decision (kullanıcı "tam yetki" verdi, Codex review PASS-WITH-CONDITIONS 78% confidence).
**Scope:** M1 FINAL_LOCK + M3 Sprint 9 + M4 Sprint 10 + M5 PixelLab Wang spec patch zinciri için bağlayıcı kararlar.

---

## D1. Sprint 9 RoomTemplate = TRUE THIN STUB (5 field)

**Karar:** Sprint 9 RoomTemplateSO.cs sadece 5 field içerecek: `schemaVersion`, `roomId`, `biomeId`, `roomType`, `bounds`. **Helper types (DoorSocket, PlayerSpawnSocket, EnemySpawnSocket, CameraBounds) Sprint 10'a defer.**

**Why:** Sprint 9 stub'la Sprint 10 full arası type churn'ü önler (Codex review blocker #1). M4 spec'inde Sprint 10 = stub'a **append** helper types — Sprint 9'a dokunmaz.

**How to apply:** M3 patch'inde RoomTemplateSO impl'i 5 field'a indirildi, helper types/enum Sprint 10 scope. M4 spec'inde "Sprint 9 stub = 5 field, Sprint 10 ADDS" notu eklendi.

---

## D2. Global `RIMA.RoomType` Reuse (Combat / Elite / Boss / Merchant / Event / ...)

**Karar:** Yeni `{ Combat, Shop, Shrine, Elite, Boss }` enum YASAK. Global `RIMA.RoomType` (Combat / Elite / Boss / Chest / Merchant / Forge / Event / Curse / Corridor) **kullan**. RoomTemplate.roomType = `RIMA.RoomType`.

**Mapping:**
- Shop → **Merchant** (RIMA.RoomType.Merchant)
- Shrine → **Event** (RIMA.RoomType.Event)
- Sprint 10 RoomBank V1 surface: `combatRooms`, `eliteRooms`, `bossRooms`, `merchantRooms`, `eventRooms`
- V1+ opsiyonel: `chestRooms`, `forgeRooms`, `curseRooms`, `corridorRooms`

**Why:** Codex review blocker #2. Duplicate enum compile-fail + global ile mapping incoherence riski.

**How to apply:** M3 spec'inde RoomTemplateSO field tipi `RIMA.RoomType`. M4 spec'inde RoomBankSO list isimleri Merchant/Event'e güncellendi.

---

## D3. M3 File Path Fix — Existing Files Modify (live paths)

**Karar:** M3 spec'inde stale path'ler düzeltildi:

| Bileşen | Eski (stale) | Yeni (live) |
|---|---|---|
| `TargetLayer` enum | yeni dosya `TargetLayer.cs` | mevcut `Assets/Scripts/MapDesigner/Brush/Data/Enums.cs:17` (APPEND `SizeBucket` + `ValidationIssueSeverity`) |
| `RimaSortingLayerValidator` | `Brush/Validation/RimaSortingLayerValidator.cs` | `Assets/Editor/RimaSortingLayerValidator.cs` (MODIFY existing) |
| `DecorativeExecutorUtility` | `Brush/Execution/DecorativeExecutorUtility.cs` | **NESTED** in `Assets/Scripts/MapDesigner/Brush/Executors/Editor/FreeformDecalExecutor.cs:23` (MODIFY in-place — `PlaceAt` line 129, scale lerp line 144, localScale line 157) |

**Why:** Codex review blocker #3. Yeni dosya açmak compile-fail + duplicate symbol + refactor maliyeti.

**How to apply:** M3 patch'inde "File Scope" tablosu live paths ile güncellendi. R1 retrofit `FreeformDecalExecutor.cs` modify. R2 retrofit `Assets/Editor/RimaSortingLayerValidator.cs` modify (Detail / Accent / Props / Entities EnsureLayerAfter append).

---

## D4. PixelLab transition_description REQUIRED (S86 dispatch verified)

**Karar:** `mcp__pixellab__create_topdown_tileset` çağrısı `transition_size > 0` durumunda `transition_description` REQUIRED. S86 PREP-3 dispatch'inde transition_description verildi → dispatch başarılı (tileset_id 7b34aa6b-2031-455d-94e5-4322579c984e).

**Why:** Codex review blocker #4. Spec'te eksikti, dispatch sırasında gerekli olduğu ortaya çıktı.

**How to apply:** M5 patch'inde Section 2 parameters tablosuna transition_description REQUIRED satır eklendi + Section 3'e prompt formula eklendi.

---

## D5. Wang Bit Order = NE-NW-SE-SW, 15 Imported Variant

**Karar (iki-parçalı):**

**Bit order canonical:** **NE-NW-SE-SW** (corner-major). Variant tag pattern: **`wang_{ne}{nw}{se}{sw}`** (örn. `wang_0011` = NE=0, NW=0, SE=1, SW=1 → south edge wall).

**Import count:** PixelLab actual output = **15 unique Wang case** (format `tileset15`, 4×4 PNG grid 128×128). L3 AssetPool **15 wall/transition variant** import eder. 0000 (all-floor) `all_floor_reference` tag → AssetPool dışı, L1 zaten halleder.

**Why:** Codex review blocker #5. Eski "16 base tile" wording PixelLab gerçeğiyle çelişiyordu. Bit order standardize edilmemişti.

**How to apply:** M1 patch'inde Section 2 Wang bit order + tag pattern netleştirildi. M5 patch'inde Section 2 + Section 7 QC + Section 10 metadata.json güncellendi. BrushAtlasImporter Wang generator: 15 variant + 1 reference tile.

---

## D6. RoomBankSO = Editor-authored Runtime-readable Asset

**Karar:** `RoomBankSO` "editor-authored runtime-readable asset". Editor utilities (`RoomTemplateSaver`, `RoomTemplateLoader`) Editor-only assembly. Runtime: `RoomBank.Pick(RoomType, int seed)` callable from runtime code.

**Why:** Codex review blocker #6. Spec'te "Editor-only registry" yazıyordu ama runtime'da `Pick` çağrılmalı (Sprint 10 EC-6: PlayMode spawn test).

**How to apply:** M4 patch'inde class XML doc güncellendi: "Editor-authored runtime-readable asset. Authored Editor only. Read Runtime safe."

---

## CROSS-REFERENCES

- `STAGING/sprite_strategy_FINAL_LOCK.md` (M1 patched)
- `STAGING/codex_brush_sprint9_atlas_importer.md` (M3 patched)
- `STAGING/codex_brush_sprint10_room_template_bank.md` (M4 patched)
- `STAGING/pixellab_l3_wang_chain.md` (M5 patched)
- `STAGING/codex_review_M1_M3_M4_M5_DONE.md` (Codex review baseline)

---

## NEXT (Sprint 9 implementation green-light)

Bu 6 karar geçerli olduğu sürece Sprint 9 implementation başlayabilir:
- M7 = Sprint 9 source kod yazımı (Opus implement → Codex review window 16-18 May)
- M8 = vertical slice loop test
- M9 = GO/NO-GO

Bu memory dosyası Sprint 9 impl'i tamamlanana kadar AKTIF reference olarak kullanılacak.

---

## Cross-links

- [[brush-tool-v1-design]]
- [[room-library-architecture]]
- [[karar-143-layered-pipeline]]
- [[codex-as-reviewer-until-may18]]
