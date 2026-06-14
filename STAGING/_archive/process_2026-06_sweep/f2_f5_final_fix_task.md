# F2+F5 Final Fix — Opus Combined Review CONDITIONAL Fixes

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Opus combined review (`STAGING/CLIFF_F2_F3_F4F5_FIX_REVIEW.md`) F2 ve F5 için 2 P0 fix. Toplam ~7 LOC + 1 scene wire. ≤15 dk.

## Bağlam
- F3 ✅ PASS (clean)
- F4_fixed ⚠️ MINOR (per-emitter fairness, opsiyonel polish skip)
- F2 ⚠️ CONDITIONAL — cold scene reopen issue
- F5 ⚠️ CONDITIONAL — scene null reference

## F2 Fix (CliffDropShadowGenerator.cs ~2 LOC)

### Issue
- Sprite + Texture `HideFlags.DontSave` → scene save sonrası 283 hücre `fileID:0` (null) referans tutuyor
- Cold scene reopen'da OnEnable Regenerate çalışana kadar shadow görünmez
- **NOT bug**: tileCount=1 sorunu `GetUsedTilesCount()` API artifact, gerçek 283 hücre serialize edilmiş

### Fix
- File: `Assets/Scripts/Environment/CliffDropShadowGenerator.cs`
- Sil iki `hideFlags = HideFlags.DontSave` satırı:
  - Line ~45: `tex.hideFlags = HideFlags.DontSave;` SIL
  - Line ~55: `_cachedSprite.hideFlags = HideFlags.DontSave;` SIL
- Maliyet: ~2 KB scene cost (kabul edilir)
- Scene save sonrası reopen'da shadow korunur

## F5 Fix (CliffFaceIdleAnimator.cs ~2 LOC + scene wire)

### Issue
- Scene'de `cliffAutoPlacer: {fileID: 0}` NULL
- floorMap null → ComputeCliffDir her zaman S döner → 8-dir fix ÖLÜ (kod kusursuz ama bağlanmamış)

### Fix
1. **Code (~2 LOC):** `CliffFaceIdleAnimator.cs` `Awake()` içine ekle:
   ```csharp
   if (cliffAutoPlacer == null)
       cliffAutoPlacer = FindObjectOfType<CliffAutoPlacer>();
   ```
2. **Scene wire:** `PlayableArena_Test01.unity`
   - CliffTilemap GO → `CliffFaceIdleAnimator` component
   - `cliffAutoPlacer` slot'a CliffRing GameObject drag-drop
   - Scene save

## F4 — SKIP (MINOR fairness, opsiyonel polish)
Opus verdict: global cap Update loop'ta enforce, sadece per-emitter divide ilk emitter 200, son emitter 1. Cosmetic, demo blocker değil. Atla.

## Dosyalar (scope)
- `Assets/Scripts/Environment/CliffDropShadowGenerator.cs` (~2 LOC sil — HideFlags.DontSave)
- `Assets/Scripts/Environment/CliffFaceIdleAnimator.cs` (~2 LOC ekle — Awake auto-find)
- `Assets/Scenes/Test/PlayableArena_Test01.unity` (CliffFaceIdleAnimator.cliffAutoPlacer slot wire)
- Toplam ~4 LOC + 1 scene wire

## YASAK
- F3 (PASS, dokunma)
- F4 fairness polish (opsiyonel skip)
- F1 (PASS önceki tur)
- Diğer cliff dosyaları
- `mcp__UnityMCP__refresh_unity scope=all mode=force` ZORUNLU sonrası

## Verify
- 0 err / 0 warn
- F2: PlayableArena scene save → close → reopen → CliffDropShadowTilemap shadow tile'ları korunur (cold reopen test)
- F5: PlayMode → cliffAutoPlacer slot dolu, ComputeCliffDir 8 yön döndürür, cliff sprite swap yön-aware

## Output
- `STAGING/F2_F5_FINAL_FIX_DONE.md` — değişen dosyalar + cold reopen test sonucu + 8-dir verify

## Code rotation
Sen Sonnet yaz. Opus re-review opsiyonel (cliff F path final pass).
