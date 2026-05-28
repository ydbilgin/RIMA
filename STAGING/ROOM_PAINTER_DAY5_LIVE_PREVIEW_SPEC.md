# Room Painter — Day 5 Live Preview Pane + Visual Collider Authoring

**Status:** DESIGN ONLY — Sonnet sub-agent (S110 Phase A, 2026-05-26)
**Scope:** Day 5 redesign of `Assets/Editor/RoomPainter/*`. Replaces the previous "Day 5 = drag-drop + erase/pick/box-select" plan from `STAGING/ROOM_PAINTER_ALL_IN_ONE_UX_SPEC.md` Section 9.
**Inputs:** Day 1-4 LIVE state, Day 4 spec (`STAGING/ROOM_PAINTER_ALL_IN_ONE_UX_SPEC.md`, 5200 words), `Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs` (drag handle / ghost preview pattern), `feedback_double_auto_regen_avoid.md` (no double-trigger), `project_high_top_down_3_4_lock_2026_05_24.md` (3/4 sprite, -Y sort).
**User direktifi (verbatim):** "Şu an RIMA Room Painter'da bir sürü şey oynayıp duruyor. Ayrıca bir şeye tıklayınca yanda preview halini görecem oradan da istediğim takdirde rigid2d ekleyebilecem body'i ama mantıksal olarak ekleyebilecem 3d şeklinde."

---

## Section 1 — UI Stability Fixes (designer şikayeti #1)

After Day 4 lands, the Inspector pane holds ~30 fields across 6 foldout sections (Identity, Placement, Physics, Parallax, Visual, Metadata). The designer's verbatim "bir sürü şey oynayıp duruyor" maps to five concrete jitter sources. All five are fixed in Day 5a before the new preview pane is added — otherwise the preview pane will inherit and amplify the same instability.

### 1.1 Foldout state inconsistency
Day 4's `RoomPainterInspectorPanel.cs` (~300 LOC) creates fresh `bool` foldout flags per `Draw()` call. On every asset re-select the foldouts collapse back to default. Designer perceives this as "the panel keeps rearranging itself."

**Fix:** persist each section's foldout state in `EditorPrefs` under keys `RIMA.RoomPainter.Foldout.<SectionName>` (Identity / Placement / Physics / Parallax / Visual / Metadata). Read once in `OnEnable`, write on toggle change only — not every OnGUI. File: `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs:OnEnable`, plus one read/write per section file in `Assets/Editor/RoomPainter/Inspector/Sections/*Section.cs`.

### 1.2 Dropdown re-population on every OnGUI
`PlacementSection.cs` populates the Sorting Layer dropdown by calling `InternalEditorUtility.sortingLayerNames` every `Draw()`; `PhysicsSection.cs` does the same for Unity layers; `ParallaxSection.cs` enumerates `ParallaxTierNames` from scratch. Each re-population reallocates string arrays and causes the popup labels to "flicker" on selection change.

**Fix:** cache the arrays in static fields invalidated only on `EditorApplication.projectChanged` and `EditorApplication.hierarchyChanged`. File: new `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorCaches.cs` (~60 LOC).

### 1.3 SO-vs-instance banner flicker
The Day 4 "Editing SO: cliff_03" vs "Editing instance: Cliff_cliff_03" banner (Section 3 of `ROOM_PAINTER_ALL_IN_ONE_UX_SPEC.md`) is rebuilt as a new `GUIContent` on every repaint. Switching focus between SceneView and the painter causes Unity to repaint at ~30 fps; the banner re-allocates 30×/sec.

**Fix:** static readonly `GUIContent _bannerSO` and `_bannerInstance` with cached label + cached cyan/orange `GUIStyle`. File: `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs:DrawBanner`.

### 1.4 Section order swapping
Designer reported sections appearing in different orders. Investigation: `RoomPainterInspectorPanel.Draw` switches the draw order based on `_activeTab` (Gameplay vs Parallax). When the designer toggles tabs the panel reorders. This was an unintended Day 4 optimization.

**Fix:** lock section order to `[Identity → Placement → Physics → Parallax → Visual → Metadata]` regardless of active tab. Parallax tab just toggles greyed-out state on non-parallax sections; it does not reorder. File: `RoomPainterInspectorPanel.cs:Draw` switch removed, replaced with linear `foreach (var section in _sections)`.

### 1.5 Scroll position reset on selection change
On asset re-select the inspector's `_scrollPos` resets to (0,0). Designer who was inspecting Physics finds themselves back at Identity after every palette click.

**Fix:** persist `_scrollPos` per-section-anchor in a transient dictionary `Dictionary<string, Vector2> _scrollAnchors`. Key by SO GUID for palette-mode, by instance InstanceID for scene-mode. Clear on window close, not on selection change. File: `RoomPainterInspectorPanel.cs:DrawScrollView`.

### 1.6 Acceptance test
Manual: open painter, select Cliff asset, expand Physics + Visual foldouts, scroll to Visual, switch to Floor asset → Physics/Visual foldouts must stay expanded, scroll position must stay at Visual, section order must remain Identity-first.

---

## Section 2 — Center Pane: Live Preview Architecture

The Day 4 spec defines a 3-pane layout `[Palette | Minimap | Inspector]`. Day 5a replaces the middle minimap with the new **Live Preview Pane**. The minimap is demoted to a toggle inside the Preview pane's overflow menu (Phase B).

### 2.1 New 3-pane layout

```
+========================================================================================+
| [Gameplay] [Parallax] | Folder: Assets/Sprites/Environment [▼] | [Refresh] [⚙] [?]   |
+----------------------------------------------------------------------------------------+
| [All] [Floor] [Cliff] [Wall] [Props] [Decals] [Lighting] [Parallax] [Other]  search:[_]
+================+============================================+==========================+
| ASSET PALETTE  | LIVE PREVIEW                               | INSPECTOR                |
|   ~ 300 px     |   ~ 400 px                                 |   ~ 350 px               |
|                |                                            |                          |
| [thumb][thumb] | +----------------------------------+       | Identity ▼               |
| [thumb][thumb] | | dark checkerboard background     |       |  [128×128 preview]       |
| [thumb][thumb] | |                                  |       |  Path: …/cliff_03        |
| [thumb][thumb] | |    [256×256 sprite, zoom 2x]     |       |  Name: [Cliff03   ]      |
| [thumb][thumb] | |                                  |       |                          |
| [thumb][thumb] | |    + drop shadow ellipse         |       | Placement ▼              |
|                | |    + cliff shadow ramp (lower)   |       |  Layer: Cliff       ▼    |
| Pages: 1/4     | |    + pivot crosshair (red+green) |       |  SortLyr: Floor     ▼    |
| [<] [>]        | |    + Y-sort axis line            |       |  Order: [   5  ]         |
|                | |    + collider outline (cyan dash)|       |  Y-sort: [x]  axis -Y    |
|                | |                                  |       |                          |
|                | +----------------------------------+       | Physics ▼                |
|                | Tools: [None][Box][Circle][Caps][Poly]     |  Block: [x] yes          |
|                |        [Trigger] [Block]                   |  Body:  Static     ▼     |
|                | Zoom: [1x][2x][4x][8x]  Pan: MMB           |  Shape: Box         ▼    |
|                | [x] Show 3D mock  [x] Show shadow          |  Size:  auto [edit]      |
|                | Status: collider 64×32 / anchor (0.5, 0)   |  Trigger: [ ]            |
|                |                                            |  PhysLyr: Default   ▼    |
|                |                                            | ...                      |
+================+============================================+==========================+
| Sel: cliff_03 | Layer: Cliff | Sort: Floor/5 | Tier: — | Hint: drag to scene, R rotate |
+========================================================================================+
```

- **Window minSize:** 1200 × 700 (Day 4 minSize 1100 × 640 too narrow for preview).
- **Splitters:** vertical between Palette/Preview and Preview/Inspector, persisted to `EditorPrefs` keys `RIMA.RoomPainter.PalettePx` / `PreviewPx` / `InspectorPx`.
- **Empty state:** preview pane shows centered helpbox "Select an asset in the palette to see preview" with a small "Recent assets" mini-grid (last 5).
- **Collapse:** preview can collapse to 32 px vertical strip via the toolbar `⚙` menu. Inspector unchanged.

### 2.2 Preview rendering pipeline

Three layers from bottom to top:

1. **Background layer** — dark checkerboard (`GUI.DrawTexture` tiled with a 16×16 alpha-checker texture generated in code). Provides Unity-style transparency indicator.
2. **Sprite layer** — `EditorGUI.DrawPreviewTexture` with the sprite's normalized rect (handles sliced sprites correctly). For prefab targets without a direct sprite, fall back to `AssetPreview.GetAssetPreview`.
3. **Overlay layer** — `Handles.BeginGUI()` block draws drop shadow ellipse, pivot crosshair, Y-sort axis line, collider outline. All overlays implemented in `Assets/Editor/RoomPainter/Preview/PreviewOverlayDrawer.cs`.

**Repaint triggers** — call `previewPane.Repaint()` only when:
- Palette asset selection changes.
- `R` key pressed in preview rect (90° rotate, mirrors SceneView R-rotate).
- Inspector field change that affects preview (Y-sort axis, pivot anchor, tint, scale).
- Mouse wheel over preview rect (zoom).
- Middle-mouse drag in preview rect (pan).

Critically, do NOT repaint on every OnGUI; only repaint when one of the above triggers fires. This is the same anti-pattern fix as Section 1.3.

### 2.3 Zoom + Pan

- Mouse wheel over preview rect: zoom 1x → 2x → 4x → 8x with clamp at 0.5x..16x. Persist `_previewZoom` to `EditorPrefs.RIMA.RoomPainter.PreviewZoom` (float).
- Middle-mouse drag: pan within preview rect. `_previewPan` Vector2 in pixel coordinates.
- `F` key in preview rect: fit-to-pane (reset zoom to optimal + pan to origin).

### 2.4 "3D-mock depth" rendering (the user's "3d şeklinde")

Top-down sprite with pseudo-3D depth illusion. Three additive effects:

1. **Drop shadow ellipse** — yarı saydam koyu gri ellipse (size 80% of sprite width × 25% sprite height), drawn directly below sprite pivot. `GUIUtility.AlphaBlend(0.35f, Color.black)`. Always ON for non-floor layers.
2. **Cliff shadow ramp** — only when `RoomPainterAsset.defaultLayer == Cliff`. Bottom 40% of sprite gets a top-to-bottom gradient overlay (transparent → 35% black). Simulates the "drop face" of a cliff edge, gives the eye a depth cue.
3. **Parallax atmospheric tint** — only when `defaultLayer == Parallax`. Sprite gets a subtle blue tint multiply (color `#E8F0FF` at 30% strength) plus a 5% scale-down. Simulates atmospheric perspective.

All three effects toggleable via Preview pane bottom toolbar `[x] Show 3D mock`. Default ON.

### 2.5 Pivot + Y-sort axis indicators

- **Pivot crosshair** — red horizontal line + green vertical line through the asset's pivot point. Length 24 px regardless of zoom. Helps designer verify sprite import pivot.
- **Y-sort axis line** — single dashed vertical line at `pivot.x` extending the full preview height. Color cyan if `ySortEnabled = true && ySortAxis == -Y`, orange if `+Y`, magenta if `+Z`, invisible if disabled. Shows the designer which screen axis the Y-sort will key off.

### 2.6 Rotation feedback

`R` key while preview pane has focus rotates the preview sprite by 90°. Live rotation is purely a visual rehearsal — does not write to SO. (Writing happens via SceneView rotate.) This gives the designer immediate feedback on what a rotated paint will look like.

---

## Section 3 — Visual Collider Authoring (CRITICAL)

The user's headline ask: "rigid2d ekleyebilecem... mantıksal olarak... 3d şeklinde." This section is the design's center of gravity. Day 5b delivers the interactive collider editor inside the preview pane — designer never opens the Unity Inspector to add/edit a Collider2D.

### 3.1 Tool selector

Bottom toolbar of preview pane:

```
[None] [Box] [Circle] [Capsule] [Polygon]   |   [Trigger] [Block]   |   [Show 3D mock]
```

- `[None]` — collider authoring off; pane shows only sprite + 3D-mock overlays.
- `[Box]` — `BoxCollider2D` editing mode.
- `[Circle]` — `CircleCollider2D`.
- `[Capsule]` — `CapsuleCollider2D`.
- `[Polygon]` — `PolygonCollider2D`.
- `[Edge]` — `EdgeCollider2D`. **Phase B**, not in Day 5b scope.

Selecting a tool button activates the corresponding handle layer on the preview overlay. The selected tool is mirrored to `RoomPainterAsset.colliderShape` enum so palette-mode edits write to the SO; scene-mode edits write per-instance.

`[Trigger]` / `[Block]` are mutually exclusive toggles in the same row. They mirror `RoomPainterAsset.colliderTrigger` and the `Block` flag from Day 4's PhysicsSection. Visual feedback: Block ON → cyan outline, Trigger ON → yellow outline, both OFF → grey outline (preview-only, will not write at paint time).

### 3.2 Box collider authoring

- **Default size** — auto-inferred from sprite alpha bounds × 0.85 margin (same `RoomPainterPhysicsApplier.Apply` logic from Day 4). Stored in `RoomPainterAsset.colliderSizeManual` Vector2.
- **Handles** — 4 corner handles + 4 edge midpoint handles. Each is a `Handles.FreeMoveHandle` in pane-local coordinates, converted to world-units via the active zoom + pan transform.
  - Corner drag: resize on both axes.
  - Edge midpoint drag: resize on one axis.
  - Hold Shift: uniform scale from center.
- **Visual** — `Handles.DrawSolidRectangleWithOutline` with cyan fill at 15% alpha + cyan edge at 85% alpha (mirroring `ColliderPainter.GhostFill` / `GhostEdge` constants from `Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs:36-39`). Dashed line for Trigger, solid for Block.
- **Size label** — `Handles.Label` above the box: `"64×32 / offset (0.5, 0)"`. Updates live during drag.
- **Live write** — drag delta → updates `RoomPainterAsset.colliderSizeManual` + `colliderOffsetManual`. Calls `Undo.RecordObject(asset, "Edit collider size")` once per drag start; collapses on drag end (same pattern as `ColliderPainter.StartDrag` / `Undo.CollapseUndoOperations`).

### 3.3 Circle collider authoring

- **Default radius** — `min(sprite.bounds.size.x, sprite.bounds.size.y) * 0.5 * 0.85`.
- **Handles** — 1 radius handle (cardinal east position). Drag outward = grow, inward = shrink. + 1 center handle for offset.
- **Visual** — `Handles.DrawWireDisc` cyan, plus `Handles.DrawSolidArc` 15% alpha for the fill (Unity's wire disc has no fill; use solid arc trick from `ColliderPainter.DrawCircleGhost:167`).
- **Live write** — radius delta → `RoomPainterAsset.colliderRadiusManual` (new float field).

### 3.4 Capsule collider authoring

- **Default** — height = sprite height × 0.85, width = sprite width × 0.85, direction = Vertical.
- **Handles** — 2 endpoint handles (top + bottom of capsule axis) + 1 radius handle (perpendicular to axis). Direction toggle (Horizontal/Vertical) in a small sub-row when Capsule tool is active.
- **Visual** — two arcs + two straight lines composing the capsule outline. Cyan dashed. Implemented in `ColliderHandleDrawer.DrawCapsuleOutline(rect, size, direction)`.
- **Live write** — `RoomPainterAsset.capsuleSize` Vector2 + `capsuleDirection` enum.

### 3.5 Polygon collider authoring

The thorniest case. Two sub-paths:

**Path A — Auto-trace from sprite alpha** (default for new assets).
- On first switch to Polygon tool, the system calls Unity's built-in `Sprite.GetPhysicsShape(0, ...)` (returns the alpha-traced outline configured at sprite import). If sprite has no physics shape configured, fall back to a manually-tuned 8-vertex octagonal hull around the sprite bounds.
- The result populates `RoomPainterAsset.polygonVertices` Vector2[].

**Path B — Manual vertex editing.**
- Each vertex shows a `Handles.FreeMoveHandle` (small cyan sphere). Drag = move vertex.
- **Shift + Click on edge** = insert new vertex at click position along the edge. Click position is projected onto the nearest edge segment.
- **Alt + Click on vertex** = delete vertex (min 3 vertices to keep a valid polygon — Block on attempt to go below).
- **Ctrl + drag** = constrain motion to vertical or horizontal (whichever axis has larger initial delta).
- **Visual** — closed polyline cyan dashed (`Handles.DrawDottedLines` at width 3px). Vertices drawn as `Handles.SphereHandleCap` size 0.04 in pane-local units.

**Critical decision** — default to Path A (auto-trace) for "Apply" first, expose Path B (manual edit) only after the designer clicks "Edit vertices" button. Rationale: 90% of polygon use cases want a quick alpha-trace; manual editing is the long-tail. This mirrors how Unity's Sprite Editor itself handles physics shape.

### 3.6 3D-mock collider overlay

The user's specific phrase "3d şeklinde" applied to colliders. The collider authoring view extrudes the 2D outline downward into a pseudo-3D box.

- **Top face** — the actual collider outline at `y = 0` (sprite plane). Cyan solid edge + 15% cyan fill.
- **Side face** — extruded copy of the outline displaced downward by `extrusionDepth = 0.5 * sprite_height_units` (designer feels "this collider sits on the floor and extrudes upward to the sprite's height"). Mid-cyan transparent fill (25% alpha) + lighter cyan edge.
- **Bottom face** — same outline at `y = -extrusionDepth`, ghosted at 8% alpha (mostly invisible, just for closure).

Visualisation toggle: `[x] Show 3D mock` checkbox in preview bottom toolbar. Default ON. The extruded faces are pure visual — only the top-face outline writes to the actual `Collider2D` component. The 3D extrusion is a designer affordance ("does this collider feel like it sits in the right plane?").

### 3.7 Per-asset vs per-instance authoring

The collider editor binds to whichever target the inspector banner shows (Day 4 Section 3 SO-vs-instance banner).

- **Palette / SO mode** — edits write to `RoomPainterAsset` SO. Affects all future placements and (when the "Re-apply to instances" button is pressed) all existing instances pointing at this SO.
- **Scene / instance mode** — edits write to the instance's `RoomPainterAssetBinding` component override fields (Day 4 introduced the binding component). SO is untouched. Designer can promote to SO via the "Push to SO" button on the instance banner (Day 4).

The preview pane mirrors whichever target is active; the banner inside the preview pane shows "Editing SO: cliff_03 → all instances" or "Editing instance: Cliff_cliff_03 (123, 456) → overrides SO".

### 3.8 Apply propagation

Designer "Apply" button (preview pane top-right) writes the current shape/size/offset/trigger/block state to:

1. The bound target (SO or instance).
2. For SO mode: optionally walks the active scene's Grid children and calls `RoomPainterPhysicsApplier.Apply(go, asset)` on each child whose `RoomPainterAssetBinding.assetGuid == this.guid` and whose binding's "follow SO" flag is true. Designer is prompted: "Apply to 14 existing instances? (Yes / Only future placements / Cancel)".
3. Marks bound target dirty via `EditorUtility.SetDirty`. SO mode also calls `AssetDatabase.SaveAssetIfDirty(asset)` deferred to next editor update tick (to avoid the spammy-save anti-pattern).

### 3.9 Reuse from PainterSuite ColliderPainter

Directly liftable patterns from `Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs`:

- `GhostFill` / `GhostEdge` color constants (lines 36-37): reuse verbatim.
- `StartDrag` / `Undo.CollapseUndoOperations` pattern (lines 348-356): reuse for all drag interactions.
- `DrawBoxGhost` (lines 110-121): adapt to pane-local coordinates instead of world.
- `DrawCircleGhost` (lines 163-169): adapt + extend with solid arc fill.
- `Snap()` pixel snap (lines 370-375): reuse for "snap to 1/PPU" behavior (default ON, PPU=64 from cliff lock memory).
- `FinalizePolygonOrEdge` (lines 224-248): reuse for the "Bake polygon to PolygonCollider2D" path.

**Do NOT** instantiate `ColliderPainter` directly — it targets SceneView with a `Target` GameObject. The preview pane operates on a `RoomPainterAsset` SO or a scene instance binding. Lift the patterns into new `ColliderAuthoring.cs` and `ColliderHandleDrawer.cs` classes that share the same constants and undo idioms.

---

## Section 4 — Rigidbody2D Logical Add

The user said "rigid2d ekleyebilecem body'i ama mantıksal olarak ekleyebilecem" — the designer never AddComponent's a Rigidbody2D directly. The painter logically infers whether one is needed and applies it at paint time.

### 4.1 Body type control in Physics section

Day 4 already introduced the Physics foldout's `Body type` dropdown (Section 3.C of `ROOM_PAINTER_ALL_IN_ONE_UX_SPEC.md`). Day 5b extends it with **mantıksal validation**:

- **None (collider only)** — default for static environment. Painter adds Collider2D but no Rigidbody2D. Appropriate for tilemap-style obstacles.
- **Static** — adds `Rigidbody2D` with `bodyType = RigidbodyType2D.Static`. For walls, cliffs, pillars.
- **Dynamic** — adds `Rigidbody2D bodyType = Dynamic`. Only for enemies/NPCs/physics props.
- **Kinematic** — adds `Rigidbody2D bodyType = Kinematic`. For scripted movers (doors, platforms).

### 4.2 Warning system for nonsense combinations

When the designer picks a body type, the painter validates against the asset's inferred layer + block flag. Bad combinations show a yellow `EditorGUILayout.HelpBox` directly under the dropdown:

| Layer | Block | Body type | Warning |
|---|---|---|---|
| Cliff | YES | Dynamic | "Cliff Dynamic body — cliffs will fall under gravity. Did you mean Static?" |
| Floor | YES | * | "Floor with Block=YES — floors usually don't block. Did you mean a wall asset?" |
| Parallax | * | not None | "Parallax body type — parallax layers are background plates and shouldn't have physics. Set body to None." |
| Decals | YES | * | "Decal Block=YES — decals are visual-only. Toggle Block off." |
| Wall/Cliff/Pillar | NO | None | "Asset name suggests obstacle but Block=NO. Did you mean to block?" |

Warnings are non-blocking — designer can save anyway. The warning persists in the inspector and on the painted instance's Gizmo (small yellow `!` icon in SceneView).

### 4.3 Advanced foldout

Mass, drag, gravity scale, interpolation, collision detection are rarely-touched. They go in a `[Advanced]` foldout under the Body Type dropdown, default-collapsed. Fields:

- Mass (default 1).
- Linear damping (default 0).
- Angular damping (default 0.05).
- Gravity scale (default 0 for Static/Kinematic environment; default 1 for Dynamic enemy).
- Collision detection (Continuous / Discrete dropdown).
- Interpolation (None / Interpolate / Extrapolate).

Defaults are sane for environment art; advanced foldout exists only for the long-tail case of a physics prop that needs tuning.

### 4.4 Apply at paint time

`RoomPainterPhysicsApplier.Apply(GameObject go, RoomPainterAsset asset)` from Day 4 is extended:

```
if (asset.bodyType != None) {
  var rb = go.GetComponent<Rigidbody2D>() ?? Undo.AddComponent<Rigidbody2D>(go);
  rb.bodyType = MapBodyType(asset.bodyType);
  rb.mass = asset.mass;
  rb.linearDamping = asset.linearDamping;
  ... // advanced fields
}
```

If `asset.bodyType == None` and the GameObject has an existing Rigidbody2D (e.g. user edited the SO from Dynamic to None), remove the Rigidbody2D via `Undo.DestroyObjectImmediate`. The painter is fully bidirectional — adding AND removing components are both logical operations triggered by SO edits.

### 4.5 No-Inspector promise

Acceptance criterion for Day 5b: the designer can author a Cliff asset's full Rigidbody2D + Collider2D configuration entirely from the painter window. Verification: paint a cliff, inspect the resulting GameObject in Unity Inspector, confirm correct Rigidbody2D bodyType + Collider2D shape/size/offset/trigger + sortingLayer/order/Y-sort. Designer should never have opened Unity Inspector during the test.

---

## Section 5 — Day 5 Revised Roadmap (Day 5a + Day 5b Split)

### 5.1 Why split

Day 5 scope ballooned: Day 4's plan ("D5 = drag-drop + erase/pick/box-select") is deferred to Day 6. Day 5 absorbs all of the user's new asks: UI stability + center preview pane + 3D-mock + visual collider authoring + Rigidbody2D logical add.

Splitting Day 5 into 5a (preview infrastructure) and 5b (collider authoring on that infrastructure) lets each Codex dispatch stay under ~600 LOC of new code, keeps the compile-check cadence tight, and gives the designer a usable preview pane after 5a even if 5b slips.

### 5.2 Day 5a — UI Stability + Center Preview Pane + 3D-mock depth (1-1.5 days)

**Deliverable:** designer clicks an asset, sees the live preview pane with 3D-mock depth illusion, can zoom/pan/rotate; no jitter, foldouts stable, dropdowns cached.

Acceptance:
- All 5 UI stability fixes from Section 1 land.
- Preview pane renders sprite + drop shadow + cliff ramp + parallax tint per layer type.
- Pivot crosshair + Y-sort axis indicator visible.
- R-rotate, zoom, pan work.
- Empty state + multi-select state degrade gracefully.

### 5.3 Day 5b — Visual Collider Authoring + Rigidbody2D logical add (1.5-2 days)

**Deliverable:** 4 collider shape tools live, drag handles work, 3D-mock collider extrusion renders, body type dropdown validates nonsense combos, Apply propagates to scene instances.

Acceptance:
- Box + Circle + Capsule + Polygon tools each author their respective Collider2D.
- Per-asset (SO) and per-instance (binding override) modes both work.
- Polygon auto-trace from sprite alpha + manual vertex edit both work.
- Body type warnings show for known-bad combinations (cliff Dynamic, parallax with body, etc.).
- "Apply to N instances" propagation works with prompt.
- Manual playtest: author a complete cliff asset (collider + Rigidbody2D Static + Block + Obstacle layer) without ever opening Unity Inspector.

### 5.4 Updated Phase A roadmap

| Day | Description |
|---|---|
| D5a (1-1.5d) | UI stability fixes + Center Preview pane + 3D-mock depth rendering |
| D5b (1.5-2d) | Visual Collider Authoring (Box+Circle+Capsule+Polygon+handles) + Rigidbody2D logical add |
| D6 (1d) | Drag-drop palette→scene + Erase/Pick/Box-Select tools (was Day 5 in old plan) |
| D7 (1d) | Save/Load RoomData + Export Prefab + auto-save |
| D8 (1d) | Parallax fine controls + Mini-map (demoted to Preview overflow menu) + tooltips |
| D9 (1d) | Auto cliff edge brush (Pattern C from `cliff_pivot_manual_brush_2026_05_26.md`) + Hybrid manual override |
| D10 (1d) | Docs + demo room + screen recording |

Phase A total: 9 days → 10-11 days. Ship target: 2 weeks.

### 5.5 Risk gates

- **End of D5a:** Sonnet review of preview pane stability + 3D-mock rendering correctness; Codex regression check that Day 4 inspector still works.
- **End of D5b:** Antigravity verification of collider authoring (per `feedback_antigravity_in_every_pipeline.md`); manual playtest paint-a-cliff-without-Inspector.
- **End of D6:** end-to-end test — drag from palette, paint a cliff, edit collider in preview, save room, close Unity, reopen, load room, verify byte-identical scene.

---

## Section 6 — Codex Day 5a + Day 5b Handoff Outline

Implementable form, file-by-file, with LOC budgets and reuse pointers. Each dispatch is one Codex --effort xhigh job per `feedback_codex_effort_xhigh_2026_05_24.md`. Use `cx_dispatch.py` with `--timeout 1800` (~30 min) per `feedback_long_dispatch_via_agent_2026_05_24.md`.

### 6.1 Day 5a handoff (~10 files, ~800 LOC total)

**New files:**

| File | LOC | Purpose |
|---|---|---|
| `Assets/Editor/RoomPainter/Preview/RoomPainterPreviewPane.cs` | ~200 | Orchestrator. Owns layout rect, zoom/pan state, dispatches to renderer + overlay + tool toolbar. |
| `Assets/Editor/RoomPainter/Preview/PreviewRenderer.cs` | ~150 | Background checkerboard + sprite draw + 3D-mock depth effects (drop shadow, cliff ramp, parallax tint). |
| `Assets/Editor/RoomPainter/Preview/PreviewOverlayDrawer.cs` | ~100 | Pivot crosshair, Y-sort axis line, status label. |
| `Assets/Editor/RoomPainter/Preview/PreviewBackgroundDrawer.cs` | ~50 | Checkerboard texture generator + zoom/pan transforms. |
| `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorCaches.cs` | ~60 | Cached sorting layer / Unity layer / parallax tier arrays. |

**Modified files:**

| File | Lines touched | Purpose |
|---|---|---|
| `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` | ~80 | Insert preview pane between palette + inspector; new splitter rules; minSize 1200×700; toolbar `⚙` toggle for preview collapse. |
| `Assets/Editor/RoomPainter/Inspector/RoomPainterInspectorPanel.cs` | ~60 | Section 1.1/1.3/1.4/1.5 fixes (foldout EditorPrefs, banner static GUIContent, locked section order, scroll anchor dict). |
| `Assets/Editor/RoomPainter/Inspector/Sections/PlacementSection.cs` | ~10 | Replace inline sorting-layer enumeration with `RoomPainterInspectorCaches.SortingLayers`. |
| `Assets/Editor/RoomPainter/Inspector/Sections/PhysicsSection.cs` | ~10 | Replace inline physics-layer enumeration with cache. |
| `Assets/Editor/RoomPainter/Inspector/Sections/ParallaxSection.cs` | ~10 | Replace inline tier list with cache. |

**Reuse pointers for Codex:**
- `Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs:36-39` — `GhostFill`/`GhostEdge` color constants.
- `RimaRoomPainterWindow.DrawAssetButton` (existing) — sprite preview via `AssetPreview.GetAssetPreview` async pattern, reuse for the preview pane's prefab fallback.
- Unity API: `EditorGUI.DrawPreviewTexture` for sprite render with rect, `Handles.BeginGUI`/`EndGUI` for overlay drawing within EditorWindow.

**Codex prompt outline:**
```
Goal: implement Day 5a (UI stability + center preview pane + 3D-mock depth) per
STAGING/ROOM_PAINTER_DAY5_LIVE_PREVIEW_SPEC.md Section 1 + Section 2.

Step 1: apply Section 1's 5 stability fixes to RoomPainterInspectorPanel.cs and
the 6 Section .cs files. Verify with manual test (foldout/scroll/banner persist).

Step 2: create RoomPainterPreviewPane.cs orchestrator, PreviewRenderer.cs (sprite +
3D-mock), PreviewBackgroundDrawer.cs (checkerboard + zoom/pan),
PreviewOverlayDrawer.cs (pivot crosshair + Y-sort axis).

Step 3: modify RimaRoomPainterWindow.cs to host the new preview pane between
palette and inspector; minSize 1200x700; splitter EditorPrefs persisted.

Step 4: compile, run, paint a Cliff asset, verify drop shadow + cliff ramp render.
Verify parallax asset gets atmospheric tint. Verify R-rotate.

DO NOT: touch Day 4 RoomPainterAsset.cs schema. DO NOT add collider authoring
(that is Day 5b). DO NOT add new tools to placer.
```

### 6.2 Day 5b handoff (~6 files, ~900 LOC total)

**New files:**

| File | LOC | Purpose |
|---|---|---|
| `Assets/Editor/RoomPainter/Preview/ColliderAuthoring.cs` | ~300 | Per-shape state machines (Box/Circle/Capsule/Polygon); drag delta → SO/binding write; Apply propagation; SO-vs-instance target switch. |
| `Assets/Editor/RoomPainter/Preview/ColliderHandleDrawer.cs` | ~250 | Draws shape outlines + drag handles + 3D-mock extrusion (top/side/bottom faces); Box/Circle/Capsule/Polygon overlays. |
| `Assets/Editor/RoomPainter/Preview/ColliderApplier.cs` | ~150 | Walks scene Grid children, applies SO collider state to instances with binding match; prompt dialog "Apply to N instances?". |
| `Assets/Editor/RoomPainter/Preview/RigidbodyValidationWarnings.cs` | ~80 | Section 4.2 warning table (cliff Dynamic, parallax with body, etc.); returns HelpBox content. |

**Modified files:**

| File | Lines touched | Purpose |
|---|---|---|
| `Assets/Scripts/RoomPainter/RoomPainterAsset.cs` | ~40 | Add `polygonVertices Vector2[]`, `capsuleSize Vector2`, `capsuleDirection enum`, `colliderRadiusManual float`, `colliderOffsetManual Vector2`. |
| `Assets/Editor/RoomPainter/RoomPainterPhysicsApplier.cs` (Day 4) | ~80 | Add Rigidbody2D add/remove logic (Section 4.4 idempotent apply + advanced fields). |

**Reuse pointers for Codex:**
- `Packages/com.laureth.painter-suite/Editor/Colliders/ColliderPainter.cs:348-356` — `StartDrag` + `Undo.CollapseUndoOperations` pattern. Lift verbatim into `ColliderAuthoring.StartDrag`.
- `ColliderPainter.cs:110-121` — `DrawBoxGhost`. Adapt to pane-local coordinates.
- `ColliderPainter.cs:163-169` — `DrawCircleGhost`. Adapt + add solid arc fill.
- `ColliderPainter.cs:224-248` — `FinalizePolygonOrEdge`. Adapt for SO-write (no `Undo.AddComponent`).
- `ColliderPainter.cs:370-375` — `Snap()` PPU snap. Reuse for "snap to 1/PPU" toggle (default ON, PPU=64).
- Unity API: `Sprite.GetPhysicsShape(0, ...)` for polygon auto-trace from alpha.
- `Handles.FreeMoveHandle` for vertex / corner / radius / endpoint handles. `Handles.DrawSolidRectangleWithOutline` for box outline with fill. `Handles.DrawDottedLines` for dashed polygon edges.

**Codex prompt outline:**
```
Goal: implement Day 5b (Visual Collider Authoring + Rigidbody2D logical add) per
STAGING/ROOM_PAINTER_DAY5_LIVE_PREVIEW_SPEC.md Section 3 + Section 4.

Pre-req: Day 5a must be LIVE (preview pane exists, no jitter).

Step 1: extend RoomPainterAsset.cs schema with polygonVertices, capsuleSize,
capsuleDirection, colliderRadiusManual, colliderOffsetManual.

Step 2: create ColliderAuthoring.cs orchestrator with 4 shape state machines.
Reuse ColliderPainter.cs StartDrag/CollapseUndoOperations pattern (paths
referenced in spec Section 6.2). Both SO and instance-binding write paths.

Step 3: create ColliderHandleDrawer.cs with Box/Circle/Capsule/Polygon outline
drawers + drag handles + 3D-mock extrusion (top+side+bottom faces, cyan).

Step 4: create ColliderApplier.cs with "Apply to N instances" propagation +
EditorUtility.DisplayDialog prompt.

Step 5: create RigidbodyValidationWarnings.cs with Section 4.2 table; integrate
into PhysicsSection.cs HelpBox.

Step 6: extend RoomPainterPhysicsApplier.cs with Rigidbody2D add/remove
idempotent logic (Section 4.4).

Step 7: manual playtest — paint a cliff with full Rigidbody2D Static + Block +
BoxCollider2D authored entirely from preview pane. Verify zero Inspector touches.

DO NOT: add EdgeCollider2D support (Phase B). DO NOT add drag-drop palette→scene
(that is Day 6). DO NOT touch ParallaxSection field set.
```

### 6.3 Verification commands (per `verification-before-completion` skill)

Each dispatch ends with:
```
1. mcp__UnityMCP__read_console — confirm 0 compile errors.
2. Open RIMA/Room Painter window; verify window opens at 1200×700.
3. Select a Cliff asset; verify preview pane renders with drop shadow + cliff
   ramp + pivot crosshair.
4. (Day 5b) Activate Box tool; drag corner handle; verify cyan outline updates +
   SO colliderSize field changes; Apply; verify scene instance has BoxCollider2D
   with matching size.
```

---

## Appendix A — Files that will NOT change

- `Packages/com.laureth.painter-suite/Editor/Colliders/*.cs` — consumed read-only as a pattern reference; modifications go through the PainterSuite repo, not RIMA.
- `Assets/Scripts/Runtime/Parallax/ParallaxLayer.cs` — runtime parallax component, untouched.
- Day 1-4 SceneView paint pipeline (`RoomPainterScenePlacer.cs`) — preview pane is editor-window-side only.
- `RoomPainterAssetPostprocessor.cs` from Day 4 — auto-classification unchanged.

## Appendix B — Anti-patterns avoided

- **Double-trigger refresh** (per `feedback_double_auto_regen_avoid.md`): preview pane subscribes only to the single `RoomPainterAssetEvents.OnAssetsChanged` event bus from Day 4. No direct SO change listeners.
- **PixelLab autonomous gen** (per `feedback_pixellab_mcp_halt_strict.md`): the preview pane uses ONLY existing project assets (sprites already on disk). No generation calls.
- **Spammy save** — Apply writes to SO are deferred to next editor update tick; not saved per drag delta.
- **Component pollution** — `RoomPainterAssetBinding` (Day 4) stores a single GUID string, no Update/Awake cost.

## Appendix C — Open questions for designer review

1. **Polygon default: auto-trace vs manual?** Spec defaults to auto-trace (Path A) with manual edit as a follow-up button. Designer should confirm this matches workflow expectations.
2. **3D-mock extrusion depth: fixed 0.5×sprite_height vs per-asset?** Spec defaults to fixed. If designer wants per-asset control, add a `RoomPainterAsset.mockExtrusionDepth` float in Day 5b.
3. **Preview pane minimap fallback** — spec demotes minimap to Day 8 overflow menu. If designer relies on minimap mid-paint, restore it as a collapsible bottom strip in Day 5a instead.
