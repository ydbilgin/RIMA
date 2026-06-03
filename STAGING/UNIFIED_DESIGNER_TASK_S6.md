# RIMA — UNIFIED DESIGNER (Level/Room Authoring Tool) — TASK SPEC (S6)

**Status:** 📌 SAVED — DO NOT START. User will (a) enable ultracode, (b) say "başla". ax + cx do DETECTION first (findings only, no implementation).
**Date:** 2026-05-31 PM·6
**Owner:** Opus (orchestrator) — implementation via ultracode/workflow after sign-off.

---

## 0. WHAT THE USER ASKED (verbatim intent, TR→EN)
1. **Map Designer'da "Generate Cliff" var ama çalışmıyor** → mantıksal olarak çalışır hale getir. Cliff'ler eskiden vardı, geri gelmeli.
2. **Çok sayıda designer var** (hem custom hem "Unity designer" pencereleri) → bunları **TEK** geniş, güzel UI/UX'li, işlevsel araçta **BİRLEŞTİR**.
3. **Tertemiz odalar** yapılabilmeli (clean rooms).
4. **İki yüzey, aynı araç:** hem **oyun-içi** hem **Unity Editor**'de aynısı kurulabilmeli — **odaları seçip düzenleyebileceğim** şekilde.
5. **Gerçek bir oyun-içi tool gibi** mantıklı + kullanımı rahat bir tasarım olmalı.
6. **Asset-pack'im, room'um, her şey DÜZENLİ** olmalı.

---

## 1. CONTEXT — THIS IS NOT GREENFIELD (already-locked direction)
Per CURRENT_STATUS **PM·3** the unified tool is already the LOCKED direction
("PIVOT → Townscaper-2D MAP TOOL"). Significant code already exists:
- **M1 DONE** — shared RoomData runtime: `WangResolver.cs`, `WangRebuild.cs`,
  `RoomDataMutator.cs`, `RoomDataJson.cs`/`RoomDataPaths.cs` (RIMA.Runtime, UnityEditor-free).
- **M2 DONE** — Editor RoomPainter window v2 (`RimaRoomPainterWindow.cs`, library
  browser + CRUD + thumbnail) + in-play overlay (`InPlayMapPaintOverlay.cs`,
  RoomData load/write/recompose/save). 3 blocking bugs fixed (`ROOMTOOL_VERIFY_REPORT_S6.md`).
- **M3 TODO** — floor terrain-Wang auto-merge + generic AssetPack ScriptableObject (task #14).
- **M4 TODO** — ghost-UX, palette 3-bucket browser, Wang-variant sprite-gen.

Plan = `STAGING/ROOMTOOL_IMPROVEMENT_PLAN_S6.md`. Func spec = `ROOMTOOL_FUNC_SPEC_S6.md`.
UX spec = `ROOMTOOL_UX_SPEC_S6.md`. Verify = `ROOMTOOL_VERIFY_REPORT_S6.md`.

**So the job is CONSOLIDATION + COMPLETION, not invention:** fold the scattered
designers into the M1/M2 shared-data tool, finish M3/M4, fix cliff-generate, make
the asset-pack + room-library organized, and make the UX feel like a real in-game tool.

## 2. THE SCATTER (what must be consolidated — from menu inventory)
Editor windows / menus that currently overlap (RIMA menu + Tools/RIMA):
- **MapDesigner family:** `MapDesignerBrushWindow` (RIMA/Map Designer Brush Tool),
  `RimaVisualMapEditorWindow` (RIMA/Visual Map Designer (New)), `BlueprintPainterWindow`,
  `AssetPackBrowserWindow`, `VisualEditorScenePainter`, `OpenTilePaletteMenu`, `EncounterMenu`.
- **RoomPainter family:** `RimaRoomPainterWindow` (RIMA/Room Painter) + modes
  (Tile/Cliff/Decor/Object) + `RoomPainterPreviewPane` + scene authoring (`DecorCliffPainter`, `CliffHoverIndicator`).
- **LiveTool family:** `LiveToolPaletteWindow`, `LiveToolLauncher` (Launch/Stop/Build Both),
  `RuntimeAssetRegistryBaker`, in-play `InPlayMapPaintOverlay`.
- **Archived:** `_archive_S73~/RoomDesigner/*`, `_archive_S73~/Wang16/*` (reference only, do not revive).
- **Tile import:** `TileImportWizard` (x2 copies!), `PixelLabWangImporter`, `PixelLabPngSheetImporter`.

→ Multiple tile-palette entry points, multiple brush windows, two TileImportWizard copies,
separate asset-pack browser vs palette. This is the mess to unify.

## 3. CLIFF-GENERATE (broken — to root-cause, not yet fix)
- Runtime: `Assets/Scripts/Environment/CliffAutoPlacer.cs` — `Regenerate()` needs
  `IsReady = floorTilemap && cliffTilemap && cliffTile`. Inspector button works
  (`CliffAutoPlacerEditor.cs`), but a "Generate Cliff" entry inside a Designer window
  reportedly does nothing. Likely culprit: the designer's generate button isn't wired to
  a CliffAutoPlacer with the 3 refs assigned (or targets the wrong tilemap), OR uses an
  old code path (`DecorCliffPainter` / `VisualEditorScenePainter`).
- cx detection must find: WHICH button the user means, WHY it no-ops, and the minimal wiring
  to make cliff-generation logical again (floor → auto cliff ring, layers SEPARATE).

## 4. HARD CONSTRAINTS (carry into design)
- **Dual-surface, ONE data model:** in-game (F2 overlay) + Editor window share the SAME
  RoomData (.asset canonical + JSON sidecar mirror). Already the M1 contract — keep it.
- **Room select + edit:** library browser (pick a room → load → edit → save) must work on
  BOTH surfaces.
- **Layers SEPARATE, no bleed** (user: "hepsi birbirine geçmiş olmasın"): Floor tilemap /
  Cliff (auto) / Walls (Wang object) / Decor / Objects — distinct layers, distinct sort.
- **Organized AssetPack:** generic ScriptableObject (floor-terrain set + wall ConnectorSet +
  prop/decor buckets), portable (no RIMA-hardcoded Resources paths). Palette reads from it.
- **PixelLab floor ready:** `Assets/Sprites/Environment/PixelLabFloor/pl_floor_0-15.png`
  (16 seamless iso tiles) is the floor source. PPU 64, iso pivot.
- **Iso perspective** (PM·5 FINAL: iso cliff-floating-island). Floor = PixelLab seamless tiles;
  filling = free objects (no connected-wall Wang for walls of the room — that was abandoned);
  cliffs = auto ring under the floor.
- **Clean-room goal:** the end state is the user can build a tidy iso floating-island room
  (seamless floor + auto cliff + placed objects + brazier light + ritual center + void) with
  no gaps, easily, on either surface.

## 4B. PLACEMENT CATEGORIES + LAYER/DEPTH MODEL (user add, 2026-06-01)
The tool places NOT just floor — it places **categorized content for game flow**:
floor, **cliff, objects, portals**, etc. The palette must have **distinct CATEGORIES**
(tabs/buckets) — at minimum: Floor · Cliff · Object/Prop · Portal · (Light/Decor).
Portals matter for the game's progression (link to `PORTAL_PREVIEW_SYSTEM_SPEC_S6.md`).

### Depth-stack (user's explicit ordering — top→down, "layer N" = conceptual depth slot)
The floating-island room is built from stacked sorting layers; **layers must be SHIFTABLE**
(the user can re-order / nudge sorting per layer in the tool).
- **Layer 1 — FLOOR** (top, walkable iso tiles; PixelLab seamless `pl_floor_0-15`).
- **Layer 2 — CLIFF** (directly UNDER the floor; LOGICALLY generated for depth — "Generate Cliff"
  sweeps the floor's outer edge and drops cliff sides beneath). This is the near-depth.
- **Layer 3 — BACKGROUND DEPTH ART** (well BELOW the cliff; the Codex-generated backdrop
  images we made earlier — nebula/void/floating-ruins parallax). The room floats in air;
  cliffs + these backdrops together create the sense of depth/height.
- **BETWEEN Layer 2 and Layer 3 — "maps shown below" = the diegetic preview islands**
  (the next-room/portal-preview islands that appear DOWN in the void; per
  `PORTAL_PREVIEW_SYSTEM_SPEC_S6.md`). They sit deeper than the cliff but in front of the
  far backdrop art.
- **Cliffs are generated LOGICALLY for depth**, not hand-painted one by one (fix the broken
  Generate Cliff — §3 — so it does this).

⚠️ Reconcile with ax's flat slot list (DETECT_AX: Void0/Cliff1/Floor2/Wall3/Prop4/VFX5).
The USER ordering is authoritative for the floor/cliff/backdrop/preview depth relationship;
walls/props/VFX still slot in around it. The tool exposes a layer panel where each layer's
sorting can be nudged ("layerları kaydırabilirsin").

## 5. OUT OF SCOPE FOR NOW
- Portal/preview/orb system (separate spec `PORTAL_PREVIEW_SYSTEM_SPEC_S6.md`).
- Any asset PRODUCTION (PixelLab gen) — gated, user must approve.
- Connected-wall Wang tessellation for room walls (abandoned PM·5).

## 6. PROCESS (locked by user)
1. ax + cx produce DETECTION reports (this file's §2/§3 are their map). NO code, NO file
   edits beyond their report.
2. Opus synthesizes both + this spec into a single consolidation plan.
3. User enables ultracode + says "başla" → Opus implements via workflow.
