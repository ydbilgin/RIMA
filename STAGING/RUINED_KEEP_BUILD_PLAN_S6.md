# RUINED KEEP — MERGED BUILD PLAN (S6)

**Tech-art lead synthesis of three specs into ONE ordered build plan.** 2026-05-31.
Sources merged: `DEPTH_AND_WALLRUN_RECIPE_S6.md` (A, art-direction) +
`DRAG_PLACE_IMPL_PLAN_S6.md` (B, tools/code) + `WALLRUN_IMAGEGEN_CX_TASK.md` (C, imagegen).
Target look: `Assets/Art/ConceptRefs/chatgpt_ref_wall_anchor.png` (continuous tall masonry
enclosing back+sides, lit-wall/dark-floor contrast, centered arch + banners + torches, cyan
rift cracks) kept HYBRID with the floating-island front (void + cyan rim,
`STAGING/floor_perspective_concepts/03_wallless_improved.png`).

Owners: **Opus-writes** (data/composition authoring) · **cx-imagegen** (placeholder sprites)
· **cx-code** (Sonnet/Codex writer; writer != reviewer) · **Unity-MCP** (scene mutation +
screenshot verify). Unit: 1 cell = 64px = 1 world unit (PPU64).

---

## CONTRADICTIONS FOUND + RESOLVED (before any build)

1. **Sprite-size mismatch A↔B (REAL, resolved).** B §"Grounding facts" line 11 captions the
   wall kit as `128×192/128×160 walls, 64×128 pillars, 160×192 arch` and calls those the run
   modules. That is the OLD 3/4 diorama kit (verified on disk: `wall_tall_intact 128×192`,
   `wall_mid_cracked 128×160`, `pillar_tall 64×**160**` not 64×128, `corner_buttress 128×176`
   — B omitted it, `arch_gate 160×192`). A §2b correctly specifies the NEW run sprites at
   **64×192 (1 cell wide)**. **RESOLUTION:** the run kit is A's new 64-wide sprites; the
   128-wide chunks are NOT used in the run. B's `FootprintFromSpriteSize` (px/64, round up) is
   kept AS-IS — it yields the right answer for the new sprites (64→1 cell) automatically. Only
   B's stale line-11 caption is wrong; treat A §2a/§2b as the authoritative size table.

2. **Arch footprint half-cell (REAL, resolved).** `arch_gate` is 160px = 2.5 cells. A §3 NORTH
   places it across exactly **x6–7 (2 cells)**. B's `FootprintFromSpriteSize` ceil-rounds 160→3
   cells. **RESOLUTION:** the arch (and every non-1×1 hero piece: arch, corner_buttress 128px,
   pillar 64px) is placed as a **single explicit instance at one anchor cell**, NOT as a
   `BuildRun` tiled segment. Composer treats `SegmentKind.Entrance`/anchor pieces as single
   `Object.Instantiate` at `fromCell`; only `SolidWall` runs go through `BuildRun`'s per-cell
   stepping. This also matches A §2c STEP 2 (anchors placed discretely, overlapping the run end).

3. **"recipe does not exist" / data-shape blind (resolved — now aligned).** B line 12 was
   written before A existed and designed `WallSegment{kind,fromCell,toCell,piece,height}` blind.
   A §3 now supplies a concrete per-cell perimeter plan (N/E/W/S). The two ARE compatible: each
   contiguous A run = one `WallSegment{kind=SolidWall, fromCell, toCell, piece=wall_run_mid}`;
   each A void stretch = one `{kind=VoidEdge}` (skipped); the arch = one `{kind=Entrance}`
   (single-place). No code change — A's plan is transcribed into the `_segments` list. **The
   in-run variation (cracked/low swaps, A §2c STEP 3) is NOT modeled as separate segments** — it
   is a post-pass swap (STEP 9 below), so the segment list stays coarse (≈8 segments, not 56).

4. **Path mismatch B↔C (minor, resolved).** C writes new sprites to
   `Assets/Sprites/Environment/RuinedKeepKit/` (verified: the LIVE kit — 8 chunks already there).
   B references `STAGING/ruinedkeep_wallkit/` (the staging copy). **RESOLUTION:** ship target =
   `Assets/Sprites/Environment/RuinedKeepKit/` (live, registry-scanned). Staging copy is a
   backup only; the composer/palette read the live Assets path.

5. **Shared-core check (PASS, no conflict).** B §0 `WallRunBuilder.BuildRun` is genuinely the
   single core: the drag-place Prop mode (B §2) calls it per grid-crossing, and the composer
   (B §3) calls it per SolidWall segment. Both set Entities layer / Pivot sort-point / bottom-
   center pivot inside `BuildRun`. Confirmed they share one method — no divergence to fix.

**Net:** C (imagegen) and A (art) AGREE — 5 new 64-wide seamless run sprites are required (C is
NOT "no new sprites needed"). B's code is sound; only its stale size caption + the arch/anchor
single-place rule needed pinning down.

---

## ORDERED STEPS

> Dependency spine: **sprites → floor/camera confirm → wall-run rebuild → depth pass →
> drag-place code → verify.** Steps marked **∥** can run in parallel with the step(s) noted.

### STEP 1 — Generate the 5 new tileable wall-run sprites  ·  owner: **cx-imagegen**
- **Action:** run `WALLRUN_IMAGEGEN_CX_TASK.md` (C) as-is. Produce `wall_run_mid`,
  `wall_run_cracked`, `wall_run_low` (64×96), `wall_cap_left`, `wall_cap_right` — all 64×192
  except low — flat front-face, seamless L↔R brick courses, constant-height crenellated top,
  transparent PNG-32, on-brand palette per A §1b. Optional corners skipped (corner_buttress
  covers them).
- **Inputs:** A §2b spec, C art-spec, existing `wall_tall_intact.png`/`arch_gate.png` for style
  match, A §1b value table.
- **Output to:** `Assets/Sprites/Environment/RuinedKeepKit/` + append placeholder row to
  `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md`.
- **ACCEPTANCE:** each PNG is EXACT size (verify `64×192` / `64×96`), transparent, and **tiles
  seamlessly** — paste 3 copies of `wall_run_mid` side-by-side in an image viewer: brick courses
  must continue across both seams with no visible join. Front face value ≈ `#3b3950`, clearly
  lighter than floor `#15131c`.

### STEP 2 — Confirm floor value + camera lock (no rebuild yet)  ·  owner: **Unity-MCP**  ·  **∥ with STEP 1**
- **Action:** in the demo room scene, set the floor base value to `#15131c` (A §1b — lift the
  too-dark placeholder), confirm void `#050407`. Confirm camera is flat ortho, PPU64, Pixel
  Perfect 640×360 (LOCK) — do NOT change ortho size. This is the cheapest depth win (A §4 STEP 1:
  "if only one thing ships, ship the value split").
- **Inputs:** A §1b palette, `RUINED_KEEP_ROOM_LOOK_LOCK_S6.md`.
- **ACCEPTANCE:** screenshot — floor reads a full luminance step darker than the existing wall
  chunks; camera unchanged (hero ≈ same on-screen scale as before).

### STEP 3 — Import-settings the new sprites  ·  owner: **Unity-MCP**  ·  (after STEP 1)
- **Action:** set PPU=64, pivot=**Bottom-Center**, FilterMode=Point, Compression=None, Sprite
  Mode=Single, on all 5 new sprites. Trigger `RuntimeAssetRegistry` rebake so they get
  `tag="wall"` + `layer=Wall` (registry scans `Assets/`).
- **Inputs:** STEP 1 outputs.
- **ACCEPTANCE:** `read_console` clean; `RuntimeAssetRegistry.GetByTag("wall")` returns the 5
  new entries with non-null `sprite` (verify via `execute_code`).

### STEP 4 — Write `WallRunBuilder.cs` (shared core)  ·  owner: **cx-code**  ·  **∥ with STEPS 1–3**
- **Action:** create `Assets/Scripts/DevTools/WallRunBuilder.cs` per B §0 — `BuildRun(grid,
  from, to, WallPiece, parent, occupied)` + `WallPiece` struct. Bottom-center pivot snap,
  `sortingLayerName="Entities"`, order 0, `spriteSortPoint=Pivot`. **Cap ≤64 GO per stroke**
  (B risk 3). 1×1 footprint default; non-1-wide handled by `footprint.x` stepping.
- **Inputs:** B §0 code block, memory `feedback-depth-sort-custom-axis-not-manual-ysort-s6`.
- **ACCEPTANCE:** `validate_script` + `read_console` compile-clean; a smoke `execute_code` call
  to `BuildRun` over 5 cells instantiates 5 collinear GOs on Entities layer, Sort Point=Pivot.

### STEP 5 — Author the perimeter `_segments` data  ·  owner: **Opus-writes**  ·  (after STEP 3)
- **Action:** write `RuinedKeepComposer.cs` (B §3) THEN fill its `_segments` list by
  transcribing A §3 into coarse segments: N = one SolidWall x1→x4 + Entrance(arch)@x6 +
  SolidWall x9→x14 + corner_buttress single-place @x0,x15; E/W = SolidWall + VoidEdge spans +
  cap pieces at gap edges; S = VoidEdge + low-parapet stubs. **Arch/buttress/pillar = single-
  place anchor entries (contradiction #2), NOT runs.** Variation swaps deferred to STEP 9.
- **Inputs:** A §3 perimeter plan, B §3 data shape, contradiction #2/#3 resolutions.
- **ACCEPTANCE:** `RuinedKeepComposer` compiles; `_segments` count ≈ 8–12; each SolidWall
  segment's `fromCell`/`toCell` matches an A §3 contiguous run exactly.

### STEP 6 — Compose the connected wall-run room (rebuild)  ·  owner: **Unity-MCP**  ·  (after STEPS 4+5)
- **Action:** delete the old scattered chunk placement; call `RuinedKeepComposer.Compose()`.
  Builds N back-run gapless, corner buttresses (3c) anchoring ends, E/W mixed runs with cap
  jambs at the void gaps, arch centered x6–7, S left open (void). Runs are collinear, on-grid,
  zero gaps (A §2c STEP 1–2).
- **Inputs:** STEP 5 segments, STEP 4 builder, STEP 1 sprites.
- **ACCEPTANCE:** screenshot vs `chatgpt_ref` — back wall is ONE unbroken edge-to-edge masonry
  line (NO seams/gaps/floating chunks); buttresses frame corners; arch centered; S edge open to
  void. **This satisfies user feedback #2 (walls connect).**

### STEP 7 — Depth pass: height + contrast  ·  owner: **Unity-MCP**  ·  (after STEP 6)
- **Action:** confirm run faces fill top ~40% of frame (runs 2.5c, buttress 3c, arch 4c — A §1a).
  Lock the value split (A §1b): wall face `#3b3950` ≥17 L-points above floor `#15131c`.
- **ACCEPTANCE:** screenshot — tall lit wall faces dominate the upper frame; hard dark-floor/
  lit-wall contrast reads like the ref.

### STEP 8 — Depth pass: contact shadows + torch warm gradient + overlap  ·  owner: **Unity-MCP**  ·  (after STEP 7)
- **Action:** add contact shadow under EVERY wall/pillar/prop/hero (`#000` 45%, 1.1×width ×
  0.35c oval, +0.05c offset — A §1e). Add wall torches every 4–6c with warm radial gradient on
  the face (`#ff9a24`→floor over 2.5c, soft-light 60% — A §1e). Hang banners IN FRONT of N run
  face; flank arch with braziers; overlap run bases with rubble every 4–6c (A §1c, §2c STEP 4).
  Banners/torches stay SEPARATE sprites (not baked — memory `weapon-hand-separate-lock` art rule
  analog; HARD: banner/torch not baked into wall).
- **ACCEPTANCE:** screenshot — nothing looks pasted-on (shadows glue props to floor); vertical
  warm-to-dark gradient on tall faces reads "inside a room"; hero Y-sorts in front of pillars.
  **This satisfies user feedback #1 (not flat).**

### STEP 9 — In-run decay variation (swap, don't move)  ·  owner: **Unity-MCP**  ·  (after STEP 8)
- **Action:** swap ~25–35% of `wall_run_mid` tiles in-place for `wall_run_cracked`/`wall_run_low`
  (A §2c STEP 3); ±8% top-only crenellation wobble; pivots stay collinear. Add cyan rim on void
  edges only (front + side gaps), chains/falling-rock silhouettes (A §4 STEP 6). Loose interior
  scatter (rubble, fallen pillars) per the OLD organic rules, INSIDE the run only (A §2c STEP 5).
- **ACCEPTANCE:** screenshot — wall has organic decay BUT the run line stays unbroken; cyan <15%
  of pixels, only on cracks + void rim.

### STEP 10 — Tile drag-place (hold-drag) in F2 overlay  ·  owner: **cx-code**  ·  **∥ with STEPS 5–9**
- **Action:** upgrade `InPlayMapPaintOverlay.cs` (B §1): drag lifecycle in `Update` (replace
  the single-cell `PaintAtMouse`, current lines 90–116 / 120–140), Bresenham `GridLine`
  interpolation, grid-crossing throttle, Shift axis-lock, per-stroke in-memory undo + Ctrl+Z,
  RMB cancel/erase. Keep the F2 toggle + `_paletteRect` guard + registry palette path intact.
- **Inputs:** B §1, verified overlay structure (PaintAtMouse @120–140, Update @90–116,
  RebuildPalette @188, OnGUI @228).
- **ACCEPTANCE:** compile-clean; in Play, hold-drag paints a continuous tile line (no gaps when
  dragging fast); Shift = straight; RMB cancels mid-stroke; Ctrl+Z undoes the whole stroke.

### STEP 11 — Wall/Prop drag-place MODE + composer button  ·  owner: **cx-code**  ·  (after STEPS 4+10)
- **Action:** add `Prop` to the `PaintLayer` enum + layer toggle (B §2). Prop palette from
  `registry.GetByTag("wall") ∪ GetByTag("prop")`, footprint via `FootprintFromSpriteSize`
  (px/64). Per grid-crossing, call `WallRunBuilder.BuildRun` → connected wall run as you drag.
  Prop undo/cancel/erase. Add a "Compose Ruined Keep" button that calls
  `RuinedKeepComposer.Compose()`. Parenting `[DragPlace_Props]`, Entities layer (set in builder).
- **Inputs:** B §2, STEP 4 builder, STEP 5 composer.
- **ACCEPTANCE:** in Play, F2 → Prop mode → select `wall_run_mid` → hold-drag lays a connected
  wall run that VISUALLY matches a composer-built run (same spacing/pivot/sort — they share
  `BuildRun`); RMB/Ctrl+Z work. **This satisfies user feedback #3 (drag-place editor).**

### STEP 12 — Final verify + screenshot diff vs chatgpt_ref  ·  owner: **Unity-MCP**
- **Action:** Play the demo room, screenshot, place side-by-side with
  `Assets/Art/ConceptRefs/chatgpt_ref_wall_anchor.png`. Walk the hero to confirm void edge-stop +
  Y-sort in front of walls. `read_console` clean. Do NOT commit (push gated).
- **ACCEPTANCE:** the Definition of Done below all pass.

**Parallelism summary:** STEP 1 ∥ STEP 2 ∥ STEP 4 (sprites / floor-fix / core-code are
independent). STEP 10 (tile drag-place) ∥ the whole art chain STEPS 5–9. STEPS 3,5,6,7,8,9 are a
strict sequence (each consumes the prior). STEP 11 needs 4+5+10. STEP 12 is last.

---

## DEFINITION OF DONE (maps to the user's 3 feedback points + ref)

- **#1 NOT FLAT** ✅ — STEPS 2/7/8/9: floor `#15131c` a full L-step below wall face `#3b3950`;
  runs 2.5c / buttress 3c / arch 4c fill top ~40%; contact shadows under everything; torch
  warm-gradient on tall faces; foreground overlap + hero Y-sort. Room reads as "standing inside
  a tall room," not a top-down sticker sheet.
- **#2 WALLS CONNECT** ✅ — STEPS 1/6: new 64-wide seamless run sprites laid gapless, collinear,
  on-grid by `BuildRun`; buttresses anchor ends; back wall is ONE unbroken edge-to-edge line. No
  isolated floating chunks, no doubled side-face seams.
- **#3 DRAG-PLACE EDITOR** ✅ — STEPS 10/11: Conor-Dart hold-drag in F2 overlay, Tile mode AND
  Wall/Prop mode, both via shared `WallRunBuilder.BuildRun`; Shift straight, RMB cancel, Ctrl+Z
  undo; "Compose Ruined Keep" one-click rebuild.
- **chatgpt_ref comparison** ✅ — STEP 12 side-by-side: continuous tall masonry enclosing
  back+sides, centered arch + flanking banners + wall-torches, hard dark-floor/lit-wall contrast,
  cyan rift cracks (<15%), HYBRID floating-island front edge (void + cyan rim) open on the South.

---

## SUPERSEDES / NOTES
- For perimeter walls, A supersedes `RUINED_KEEP_ORGANIC_COMPOSITION_RULES.md` scatter-first
  STEPS 1–4; organic scatter applies only to loose interior decor (STEP 9 here).
- Camera/palette/collider remain governed by `RUINED_KEEP_ROOM_LOOK_LOCK_S6.md`.
- All new sprites are imagegen PLACEHOLDERS → PixelLab finals later (HARD rule); characters never
  imagegen.
- Code: writer != reviewer. After cx-code writes STEPS 4/10/11, route to a different reviewer
  (Opus or Codex) before Play-verify.
