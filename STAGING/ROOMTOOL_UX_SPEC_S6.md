# ROOMTOOL UX SPEC — "Townscaper but 2D, simple" (S6)

**Author:** Opus 4.8 (senior game-tools UX). **Goal:** make the RIMA map tool feel like *Townscaper but 2D* — select a piece, place it, pieces connect — frictionless, on BOTH surfaces sharing ONE `RoomData`.
**Vision (user, translated):** "like Townscaper but 2D, simpler — I select and place a piece, and pieces connect to each other."
**Grounded in actual code I read:**
- In-game: `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` (F2 IMGUI overlay, pause, drag-paint, Prop run) + `WallRunBuilder.cs` (`BuildRun` line + corner-swap) + `RoomPlacementTypes.cs` (`WallPiece`/`WallSegment`/`SegmentKind`).
- Editor: `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (3-pane: palette+library / preview / inspector, 4 modes, layer mask, splitters, Save/Playtest) + `RoomPainterScenePlacer.cs` (SceneView ghost + click/drag paint + R-rotate + scroll-cycle cliff variants).
- Data: `Assets/Scripts/RoomPainter/RoomData.cs` (floorCells / cliffCells / wallSegments / propPlacements / placements + thumbnailPath).

---

## 0. The core diagnosis (why it does NOT feel like Townscaper yet)

Townscaper's whole feel comes from THREE things: **(a) one-click places + auto-connects** (you never think about orientation), **(b) a live ghost shows the result before you commit**, **(c) the palette is tiny and visual** (one color swatch). The current tool already has the *machinery* (grid snap, drag-runs, corner-swap in `WallRunBuilder`, a ghost in `RoomPainterScenePlacer`) but it is wired up like **Tiled**, not Townscaper:

1. **No Wang/neighbor auto-connect on single click.** `RoomPainterScenePlacer.PaintCell` drops the *exact selected sprite* at a cell — it never inspects the 4 neighbors to pick the connecting variant. Walls only auto-corner *inside a single drag* (`WallRunBuilder`, mid-run only), and never re-evaluate when you place an adjacent piece later. This is the #1 gap vs Townscaper.
2. **Two surfaces look like two different programs.** Editor = 4 modes (Tile/Cliff/Decor/Object) + 10-bit layer mask + 6 category chips + parallax tiers + "Edit Hitbox" + "Launch Live Tool". F2 overlay = 3 buttons (Floor/Cliff/Prop) + Erase + Compose. Same `RoomData`, totally different mental model. The user asked for *simpler*; the Editor is closer to Tiled clutter than Townscaper.
3. **The in-game overlay does NOT write `RoomData`.** `InPlayMapPaintOverlay` paints onto live `Tilemap`s and instantiates loose prop GameObjects under `[DragPlace_Props]` — it never touches `RoomData.floorCells/propPlacements`. So edits made in the "place into the live view" surface are **lost** and not shared with the Editor window. This silently breaks the LOCKED "ONE shared RoomData" decision.
4. **Ghost is weak.** `RoomPainterScenePlacer.DrawGhost` shows a single tinted sprite + a fixed `1 x 0.609` wirecube and always-cyan tint — no valid/invalid color, no neighbor-connection preview, no drag-path preview. F2 overlay has **no ghost at all** (you find out where the tile lands only after you click).
5. **No erase/undo feel in the Editor surface.** Undo exists (Unity `Undo`), but there is no on-canvas "you erased N" / hover-to-delete affordance like the overlay's RMB. Click-vs-drag is not signposted.

The fix is not "add more features" — it is **make place+connect automatic, make the ghost honest, and collapse the two surfaces onto one simple shared model.** Below, ranked by feel-impact.

---

## RANKED CHANGES (by feel-impact)

### ⭐1 — AUTO-CONNECT on single click (the Townscaper core) — *highest impact*
**What's missing now:** `PaintCell` places the literally-selected sprite; neighbors are ignored. The user literally said "pieces connect to each other" and that does not happen on click.

**Concrete change — introduce a "Connector Set" + a `WangResolver`:**
- New asset `ConnectorSet` (ScriptableObject) groups a family of pieces (e.g. *Ruined-Keep Wall*) by their **edge mask** — the 4-bit (or 8-bit for cliffs) N/E/S/W "has-neighbor" signature → which sprite/prefab to use (straight, corner, T, cross, cap, isolated). Re-use the existing wall-kit naming convention already half-present in `InPlayMapPaintOverlay.RebuildPropPalette` (it already looks up `name + "_corner"`).
- New shared helper `RIMA.RoomPainter.WangResolver.Resolve(RoomData room, RoomLayer layer, Vector3Int cell) → pieceVariant`. On every place/erase, recompute the cell **and its 4 neighbors** (only the dirty 3×3, cheap) and swap each to its correct connecting variant. This is the autotile every tilemap editor does; the tool currently does it only mid-drag for walls.
- Palette selection becomes "**pick the family, not the variant**": the user clicks one swatch ("Wall"), and placement chooses straight/corner/T automatically. This is exactly Townscaper's "one swatch → engine figures out the shape".
- Keep a power-user escape hatch: hold **Alt = place exact selected variant** (no auto-resolve), for hand-tuning. (Townscaper minimalism for default, Tiled control on demand.)

**Why first:** without this, nothing else makes it "feel like Townscaper". Everything below is presentation around this.

---

### ⭐2 — Honest GHOST PREVIEW (snapped, valid/invalid, shows the connection)
**What's missing now:** Editor ghost = single tinted sprite + always-cyan wirecube, no validity. Overlay = **no ghost at all**.

**Concrete change (apply to BOTH surfaces, shared draw code):**
- **Single-piece ghost** under the cursor, snapped to the cell, at the *resolved* connecting variant (calls `WangResolver` so you SEE the corner/T it will become before clicking). This is the moment that sells "they connect".
- **Valid / invalid tint:** green-cyan `(0.4,0.9,1,0.55)` when placeable, warm-red `(1,0.35,0.3,0.55)` when blocked (occupied by a same-layer piece, off the floor footprint, or outside room bounds). Replace the hardcoded `CliffGhostTint`/single color in `DrawGhost`.
- **Drag-path ghost:** while holding, draw the whole projected run (every cell along the Bresenham line `GridLine` already computes) as faint ghosts with corners pre-resolved — Satisfactory "Zoop" / Factorio belt preview. The overlay's `ContinueStroke` already builds the run; just render it before commit instead of committing per-frame.
- **Footprint outline:** draw the real footprint rect (`WallPiece.footprint`), not the fixed `1 x 0.609` cube, so multi-cell props read correctly.
- **Count/length label** at the cursor end during a drag ("x7"), matching Factorio. Cheap `Handles.Label` (Editor) / `GUI.Label` (overlay).

---

### ⭐3 — Live AUTO-CONNECT FEEDBACK before commit
**What's missing now:** corner-swap happens inside `WallRunBuilder` only at commit and only mid-run; you never see *how a new piece will rewire its existing neighbors*.

**Concrete change:**
- During hover/drag, run `WangResolver` on the hovered cell **and its neighbors**, and ghost-render the neighbors' *new* variants too (e.g. a straight wall you're about to butt into ghosts-flips to a T-junction). This is the "pieces connect to each other" promise made visible — the single most Townscaper-feeling micro-interaction.
- Use a subtle pulse/outline on neighbor cells that will change, so the user understands the placement is *relational*, not just a stamp.
- Keep it to the dirty 3×3 region per cell (perf: recompute only on grid-cell crossing, per the ax research "grid-crossing throttle").

---

### ⭐4 — Place / erase / undo feedback + click-vs-hold-drag affordance
**What's missing now:** No squash/flash on place; erase has no hover target highlight in the Editor; click vs hold-drag is undocumented (overlay help text exists but is dense; Editor has none). Undo is silent.

**Concrete change:**
- **Place feedback:** brief 1-frame white flash + tiny scale pop (1.0→1.08→1.0 over ~0.12s, editor-time) on the placed piece. Cozy "drop" juice from ax research. On a drag, a soft tick at each new cell.
- **Erase affordance:** hovering in erase mode highlights the piece-to-delete with a red outline (Editor: `Handles.DrawSolidRectangleWithOutline`; overlay: tint the `FindNearestProp` target red) *before* the click. Currently `ErasePropRun` deletes silently.
- **Click vs hold-drag, signposted identically on both:** quick click = place 1 (auto-connect); press-hold-drag = connected run (auto-corner); RMB = erase / cancel-current-drag; Shift = straight-line (`AxisLockedCell` already exists in overlay — port to Editor `RoomPainterScenePlacer`); Ctrl+Z = undo whole stroke. Show this as a **one-line hint strip** at the bottom of both surfaces (overlay already has it at `OnGUI` line ~719; Editor status bar should mirror the *same words*).
- **Undo as one transaction, surfaced:** Editor already collapses (`Undo.CollapseUndoOperations`); add a status-bar "Undid stroke (N cells)" toast. Overlay `UndoStroke` should flash the restored cells.

---

### ⭐5 — Simpler, more visual PALETTE (Floor / Wall / Prop, recently-used, search)
**What's missing now:** Editor palette is gated behind 4 modes + a 10-checkbox layer mask + 6 category chips + a 4-tab authoring row + parallax tier dropdown — that is **four overlapping filter systems** for what the user thinks of as 3 buckets. Overlay is better (3 buttons) but its thumbnails are tile-sprite-only (`TileSprite` returns null for RuleTiles/prefabs → falls back to text buttons).

**Concrete change:**
- **Collapse to 3 primary buckets everywhere: Floor · Wall · Prop.** (Cliff folds under Wall as a sub-family; Decor/Object/Parallax move to an "Advanced" disclosure twirl-down, hidden by default — Townscaper minimalism). The 10-bit `_layerFilterMask` stays as an *Advanced* power-user control, not front-and-center.
- **Recently-used row** (last 6 placed pieces) pinned at the top of the palette on both surfaces — the single biggest speed win for repeated placement (Mario Maker 2 / RPG Maker pattern). Trivial: an MRU `List<string guid>` in the window + overlay.
- **Search box** (Editor toolbar + overlay header) that filters palette by name substring — one `TextField`, filters `_assetCache` / `_palette`. The Editor has *folder path* + *filters* but no name search; with dozens of assets that is the friction.
- **Real thumbnails on the overlay:** `InPlayMapPaintOverlay.TileSprite` only handles concrete `Tile`; for prefabs/RuleTiles render the registry `e.sprite`. Fall back to a labeled colored box, never an empty button.
- **Bigger swatch + name-on-hover only** (Townscaper shows no labels until needed) — current overlay 52px grid is fine; keep names in the selected-preview box, not on every cell.

---

### ⭐6 — MAP BROWSER: thumbnail grid, new/dup/delete, name, save/dirty, PLAYTEST
**What's missing now:** Editor has a *list* (`DrawRoomLibraryPanel`: 42px thumb + name + Open per row, New/Dup/Delete buttons, Save + Playtest in toolbar, dirty `*` prefix) — functional but list-shaped and buried in the left column above the palette. Overlay has **no browser at all** (only "Compose Ruined Keep"); you cannot pick/new/save a map in-game.

**Concrete change:**
- **Editor:** convert the library list to a **thumbnail grid** (Dorfromantik/Mario-Maker card feel) — 2-up cards with the baked `RoomThumbnailBaker` image, name + dirty dot, big Open on click, hover reveals Dup/Delete. Promote it to a small top strip or a toggleable left drawer so the palette gets full height. Keep New / Duplicate / Delete / Save / **Playtest** (green, already there) — Playtest is the killer "Mario Maker test your level" loop; keep it one click.
- **Overlay:** add a minimal **map row** at the top: current map name + `*` dirty + ◀ ▶ to cycle maps + New + **Save** + **Playtest (resume play)**. This is required to make the in-game surface a real authoring tool, not a scratchpad. (Pairs with change ⭐7 below — the overlay must write `RoomData`.)
- **Save/dirty parity:** both surfaces show `* Name` when dirty and the same Save button; saving bakes the thumbnail (`RoomThumbnailBaker`) so the browser stays current.

---

### ⭐7 — CONSISTENCY: ONE shared RoomData, ONE mental model across both surfaces *(structural — enables 1–6)*
**What's missing now (the silent bug):** `InPlayMapPaintOverlay` paints to live `Tilemap`s + loose `[DragPlace_Props]` GameObjects and **never writes `RoomData`**. The Editor writes `RoomData` via `RoomDataPlacementSink`/`RoomDataComposer`. They do not share state — directly contradicts the LOCKED "BOTH surfaces share ONE RoomData asset".

**Concrete change:**
- **Route the overlay through the same data sink.** Extract the Editor's write path into a runtime-safe `RoomDataMutator` (add/remove `floorCells`/`cliffCells`/`propPlacements`) that lives in the `RIMA.Runtime` assembly (overlay already references `RIMA.RoomPainter`). The overlay paints into that, then composes the live view *from* `RoomData` — so F2 edits persist and appear in the Editor window, and vice-versa.
- **One vocabulary:** Floor / Wall / Prop labels, the same hint strip, the same green Playtest, the same `* Name` dirty marker, the same ghost. A user moving from the Editor window to the in-game F2 overlay should feel they are in the *same tool, smaller window* — not a different program.
- **Shared draw/resolve code:** `WangResolver`, the ghost renderer, and the MRU palette should be plain `RIMA.RoomPainter` (no `UnityEditor` dependency) so both the IMGUI overlay and the SceneView `Handles` path call the *same* logic and stay in sync. Editor-only flourishes (`Handles`, `Undo`, prefab-stage) wrap the shared core.

---

## Keep-it-SIMPLE checklist (Townscaper minimalism, anti-Tiled)
- **Default view = 3 buckets + recently-used + search.** Everything else (10-layer mask, parallax tiers, hitbox edit, decor/object split) lives under an **"Advanced ▸" twirl-down**, collapsed by default.
- **No manual rotate/variant-pick in the default flow** — auto-connect resolves it. `R`-rotate and Alt-exact-place stay as power-user overrides.
- **One primary verb per click:** click = place+connect, drag = run, RMB = erase. Don't add tool palettes/brush-size sliders to the default surface.
- **The ghost is the documentation** — if the ghost is honest (valid/invalid + connection preview), the user needs no manual.

## Suggested build order (small, shippable steps)
1. `WangResolver` + `ConnectorSet` (⭐1) — the engine of "connect".
2. Honest shared ghost: valid/invalid + resolved-variant + drag-path (⭐2, ⭐3).
3. Overlay → `RoomData` via `RoomDataMutator` (⭐7) — unlocks parity.
4. Palette simplification + recently-used + search (⭐5).
5. Place/erase/undo feel + hint-strip parity (⭐4).
6. Thumbnail map browser + overlay map row (⭐6).

---
*Placeholders are fine per the task — the TOOL is the deliverable. ConnectorSet can ship with the existing Ruined-Keep wall kit + placeholder floor/prop swatches; art swaps in later without touching the resolve logic.*
