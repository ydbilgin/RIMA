# Codex Pre-Cleanup Fixes — Karar #150 LIVE migration prep

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NLM ACCESS:** If you need RIMA design context, query NLM first via:
  `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"`
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

**NOTE:** NLM pre-Karar #149 canon dönüyor — Karar #149 + #150 LIVE'ı `TASARIM/MASTER_KARAR_BELGESI.md` + memory üzerinden authoritative kabul et.

---

## Görev — 3 surgical fix, single batch

Karar #150 LIVE legacy asset cleanup'ı blokluyor 3 problem var. Hepsini bu task'ta çöz, **single PR-ready commit** olarak ship et. Cleanup execute bu task PASS sonrası yapılır.

### FIX 1 — LargeDungeonMapPainterBase broken gate reference

**Dosya:** `Assets/Scripts/Core/LegacyRuntimeRoomManager.cs` veya `LargeDungeonMapPainterBase.cs` (Grep `Resources.Load` ile path doğrula). Sonnet analizine göre **line 1041-1132 civarı** runtime'da `Resources.Load` çağrıları var:
- `Environment/StoneDungeon/Walls/RIMA_gate_arch` (PNG silindi git status D)
- `Environment/StoneDungeon/Walls/RIMA_gate_spikes` (PNG silindi)
- 6 wall + 4 decor PNG (hâlâ mevcut: `RIMA_wall_base/cap/banner/...`, `RIMA_pillar_base/debris/rift_crystal/shrine`)

**Karar #150 stance:** Mevcut Resources fake-iso pipeline'a migrate edilecek ama bu task'ta SADECE broken'ı düzelt:

1. `RIMA_gate_arch` + `RIMA_gate_spikes` load çağrıları için null-safe wrapper ekle (e.g. `if (asset != null)` veya try-catch). Asset yoksa graceful skip + `Debug.LogWarning` ("Karar #150 migration pending — gate sprite missing").
2. Diğer 10 wall+decor load aynen kalır — Karar #150 fake-iso swap ayrı task'ta yapılacak.
3. Hiçbir başka davranış değişimi yok.

**Test:** Game start crash etmemeli; gate-render code path no-op olmalı.

### FIX 2 — BrushDataTests.cs Act1 tile path migration

**Dosya:** `Assets/Tests/EditMode/.../BrushDataTests.cs` (Glob ile bul). Sonnet'in dediği line ~175-185'te hardcoded:
- `Assets/Art/Tiles/Keep/Floor/tile_1.png`
- `tile_2.png`
- `tile_3.png`

**Migration target:** `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_tile_01.png` (veya benzer Act1 path; Glob ile mevcut Act1 floor tile bul).

**Yapılacak:**
1. Test fixture path constants Act1'e güncellensin (3 file)
2. EditMode test çalıştır → PASS doğrula (`dotnet test --filter BrushData`)
3. PASS gate olmadan başka iş yapma

**Out of scope:** Diğer hardcoded path'leri tarama — sadece bu 3 path.

### FIX 3 — Wang16 dead code removal

**Bağlam:** Memory `[[project-rima-hades-style-cb-wang-split-lock]]` — RIMA Wang16 pipeline KAPATILDI (2026-05-19 S93). CB pivot oldu. Ama editor/test kod'u hâlâ aktif. Karar #150 fake-iso = Wang16 mantıken ölü.

**Sil veya disable et (cleanup'tan önce):**

1. **Editor scripts:**
   - `Assets/Editor/.../RebuildAllWangTilesets.cs` — silinsin (no longer needed for RIMA scope)
   - `Assets/Editor/.../CreateCornerWangTileSetAsset.cs` — silinsin
   - `AutoBiomePresetBuilder.cs` Wang section'ı varsa Wang-only path comment out + `[Obsolete("Karar #150 — RIMA Wang16 closed, CB scope only")]`

2. **Tests:**
   - `Assets/Tests/EditMode/.../WangImporterTests.cs` — `[Ignore("Karar #150 — RIMA Wang16 closed")]` per test method (silmek yerine ignore, CB pivot history'si için)

3. **Asset references:**
   - Kod ref olmadıktan sonra `Assets/Art/Tiles/F1/Generated/` Wang tile `.asset` dosyaları orphan olur (delete cleanup batch'inde handle edilir)
   - `Shattered_Keep_F1_BiomePreset.asset` korunsun (Karar #150 Act 1 biome reference)
   - `Alabaster_Dawn_BiomePreset.asset` korunsun (Karar #143 ref)

**PASS criteria:**
- Unity Editor compile clean (0 error, Wang-related warning OK)
- EditMode tests run: Wang tests SKIPPED/IGNORED, diğer testler PASS
- No new tests added — bu cleanup, code görevi

### Single commit message

```
[S94 LATE] Karar #150 pre-cleanup fixes — broken gate ref nullsafe + test path migrate + Wang16 dead code archive

- LargeDungeonMapPainter null-safe wrap RIMA_gate_arch/spikes (LIVE broken ref fix)
- BrushDataTests.cs path: Keep/Floor/tile_1-3 → Act1/floor_tiles/granite_tile_*
- Wang16 editor scripts deleted, tests [Ignore] (RIMA Wang16 closed S93)

Refs: Karar #150 STAGING/KARAR_150_LIVE_act_aware_dungeon_inside.md
Cleanup unblocks: STAGING/UNITY_LEGACY_CLEANUP_PLAN.md ARCHIVE batch (118 files)
```

---

## Done report

Yaz to: `STAGING/CODEX_DONE_pre_cleanup_fixes.md`

Format:

```
FIX 1 — Gate Reference: <PASS/FAIL>
- File(s) touched: <list>
- Compile: <PASS/FAIL>
- Notes: <any deviations>

FIX 2 — Test Path Migration: <PASS/FAIL>
- File touched: <path>
- Test run output: <pass count / fail count>
- Notes: <deviations>

FIX 3 — Wang16 Dead Code: <PASS/FAIL>
- Files deleted: <list>
- Files modified: <list>
- Compile: <PASS/FAIL>
- Tests: <Wang tests IGNORED count / other tests PASS count>

OVERALL: <COMMIT_READY / NEEDS_REWORK / BLOCKED>
Git status: <clean / staged changes / commit hash>
```

Effort: high (3 surgical fixes, judgment-heavy).
Profile: laurethayday (user-specified for this session).
