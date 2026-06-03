# TASK — Make the map render ISOMETRIC (inspect current top-down state, then apply iso recipe)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need design context, query NLM:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read: CURRENT_STATUS.md / code / scene+asset files / STAGING/ISO_TILING_LOGIC_DECISION.md.

Profile: laurethayday. Effort: high. Language: English. **No commit** (writer ≠ reviewer; Opus reviews + Unity visual-verifies after).

## Amaç (Goal)
The map / Map Designer currently renders **TOP-DOWN**; the locked direction is **ISOMETRIC**. INSPECT the current state, then APPLY the already-established iso recipe in CODE/DATA (file-based — the visual iso result will be verified in Unity afterward, you likely can't render headlessly).

## Authoritative iso recipe (from CURRENT_STATUS "🎯 İSO FLOOR KÖK-NEDEN" — user-confirmed earlier this session)
- **Floor tiles = PixelLab Floor `451bbfd8` ORIGINAL iso granite** (`Assets/Sprites/Environment/PixelLabFloor451/floor451_0-15`). NOT the flat top-down set (`ce6f15c7` / `flat_tile` / `PixelLabFloorFlat`), NOT the flattened `pl_floor` (flatten removes iso depth).
- **Grid cellSize = measured diamond ratio ≈ (0.96, 0.585)** (451 diamond ≈ 62×38 px @ PPU64). Square cellSize (e.g. 0.94×0.94) leaves vertical gaps because the diamond is wider than tall.
- **NO mathematical squash** (root scale Y=0.5) — the user explicitly rejected it as artificial.
- Per CURRENT_STATUS this cellSize was embedded in `RoomDataComposer.ComposeInto` — verify it is still there (the Unified Designer rewrite/regression may have reverted it).

## Steps
1. **INSPECT (grep/read, report findings):**
   - `RoomDataComposer.ComposeInto` (and any composer/grid setup) — what cellSize does it currently set? (0.96,0.585) iso, or square/top-down?
   - The Map Designer's default/active FLOOR group/tile — is it the 451 iso set, or a flat top-down set (`flat_tile`/`PixelLabFloorFlat`/`pl_floor`)?
   - The demo/active scene's `Grid` component cellSize + which floor tiles its Tilemap uses (read the `.unity` as text if needed).
   - State clearly: is it currently top-down or iso, and the exact reason.
2. **APPLY (surgical, file-based):**
   - Ensure composer/Grid **cellSize = (0.96, 0.585)** for iso tessellation.
   - Ensure the **default rendered floor = 451 iso granite** (PixelLabFloor451), not a flat top-down set.
   - Remove/replace any top-down or flatten default in the composer/Map-Designer path.
   - Do NOT add a mathematical Y-squash.
   - Keep changes minimal; touch only what's needed for iso rendering.
3. **VERIFY:** `dotnet build` the affected csproj(s) → report 0 errors. Do NOT claim the iso LOOK is correct (that needs Unity) — list exactly what to verify visually in Unity (iso floor tessellates seamlessly, no vertical gaps, character scale correct).

## Deliverable (CODEX_DONE_laurethayday.md, last step)
Current-state diagnosis (top-down? why?) + exact files/values changed + dotnet build result + Unity visual-verify checklist. End with `STATUS:`. No commit.
