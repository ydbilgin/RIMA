# RIMA — Live Tool + Collider Authoring + Asset Layer Decision Synthesis

> ⛔ **SUPERSEDED (live tool tier):** Bu doc T2 (Editor + Player Build bridge) önerir. Kullanıcı **2026-05-27 gece T3 FULL STANDALONE**'u kilitledi (Tool.exe + Game.exe + JSON FileSystemWatcher hot-reload). **Canonical live tool kaynağı = `STAGING/T3_TOOL_FULL_DESIGN.md`.** Bu doc'un Section 1/5 T2 önerisi TARİHSEL — sadece collider drag-handle (Decision 2) + 6-layer asset arch (Decision 3) kısımları hâlâ geçerli. Karışıklık olmasın diye live-tool kararı için T3 doc'una bak.

**Status:** Triple-AI synthesis (agy ✅ DONE, Codex ⏳ running xhigh, Opus orchestrator-subagent sentez ✅).
**Scope:** 3 connected decisions — (1) live tool architecture, (2) per-prefab collider drag-handle, (3) asset layer architecture.
**Updates / supersedes:** `STAGING/UNIFIED_PAINTER_PLAN.md` (S111 rima-design verdict). This document refines Day 2-6 plan with the user's 2026-05-27 new directives.
**Format:** Plan + recommendation + alternatives + open questions. **NO CODE WRITTEN.**

---

## 0. TL;DR for the user (2 minutes)

| Decision | User intent | Opus recommendation | Why |
|---|---|---|---|
| **Live tool architecture** | Sang Hendrix style — game in one window, tool in another, live edit | **T2 (Editor Extension + Standalone Player Bridge) → T3 phased**, NOT T4 (in-game editor) | Sang's "magic" is the in-process JS overlay — that pattern collides with C# domain reload. T2 keeps the Editor's authoring power and serializes deltas to a separate Player Build via a file watcher. T3 (full standalone) is the eventual destination but costs 12-15 days vs T2's 4-5 days. **Also note:** RIMA already has a LOCKED `RoomDescriptorEditorOverlay v0` (F9 runtime overlay) from Karar #115 — T2 is the natural extension of that existing decision. |
| **Per-prefab collider drag-handle** | Edit collider on prefab, instances inherit | **EXTEND existing `RoomPainterColliderEditor.cs`** (262 LOC, Day 5b LIVE) to write back to prefab asset via `PrefabUtility.ApplyPrefabInstance` + `EditorUtility.SetDirty` on the prefab GameObject. Add `Shape` selector dropdown (Box / Circle / Capsule). | Day 5b infrastructure exists. The current code writes only to scene instances — the missing piece is the prefab-asset write path (single `if (isInstance) ApplyPrefabInstance(...)` block). Polygon collider stays out of scope V1 (too costly for 24-prefab return). |
| **Asset layer architecture** | 4 layer user proposal OR 5-layer split (cliff base + cliff face) | **6-layer architecture** confirming the canonical NLM lock (L1 Floor / L2 Cliff base / L3 Cliff face decor / L4 Walkable decor / L5 Wall blocker / L6 Gameplay object). | NLM canonical (`Karar #115` + `Path C Hybrid Lock`) already locked a 4-layer system with sorting orders. User's 4-layer suggestion maps to L3-L6 of that canonical; the 5-layer alt (cliff split) maps to L2+L3. We add Walkable decor (L4) as its own tier because mounting_*.prefab (cliff face) and bone/rune (floor decor) need different Y-sort behavior. **No conflict with canonical, only refinement.** |

**Critical conflict to surface to user:** Karar #115 (LOCKED) explicitly REJECTED "fullscreen in-game editor" as a "scope swallower". The Sang Hendrix vision = exactly that pattern (T4). User's verbatim "Sang style" pull and the existing LOCK are in tension. **Recommendation: T2 (separate Player Build + Editor stays Editor) honors both — gameplay in fullscreen separate window AND the Editor tool stays the authoring surface.**

---

## 1. Live Tool Architecture (Decision 1)

### 1.1 What Sang Hendrix actually does (agy verdict)

> "Sang's tool is a **pure in-game editor overlay**. RPG Maker MZ runs JS in NW.js (Chromium+Node). The plugin loads an editor UI in HTML/CSS/JS directly on top of the PixiJS game canvas. Editor and game share the same memory space and JS thread — the editor directly mutates the live scene graph in-process. To persist, it uses `fs.writeFileSync()` to write JSON to the project folder. No socket, no pipe, no file watcher needed — the visual is modified in memory and re-rendered next frame."
> — agy_output.txt:13-20

**Key transferability gap to Unity:**
- NW.js gives Node.js APIs INSIDE the player build → JS hot-swap is native
- Unity C# has Domain Reload — scripts cannot be patched in Play Mode without paid tooling (Hot Reload by Singularity Group, or FastScriptReload)
- Unity's `AssetDatabase` is Editor-only; standalone builds cannot use it

**So:** A literal "Sang clone" in Unity is **not possible** without rebuilding the entire editor UI for runtime UI Toolkit + serializing all asset references manually. The Sang **pattern** (live edit + game running) is achievable; the Sang **architecture** (in-process scene-graph mutation) is not.

### 1.2 Four architectural tiers

| Tier | Pattern | Mechanism | Velocity | Scope (days) | RIMA fit |
|---|---|---|---|---|---|
| **T1** | Editor Extension + Play Mode (current) | Same Unity process; Play Mode runs game in Editor | Low (Domain Reload 2-5s per transition; Play Mode mutations don't persist) | 0 (LIVE) | Currently here |
| **T2** | Editor Extension + Standalone Player Build bridge | Editor on Monitor 1; built `.exe` on Monitor 2; `FileSystemWatcher` on shared JSON; Player rebuilds room on file change | High (instant; no Domain Reload between iterations) | 4-5 | **RECOMMENDED — pragmatic Sang** |
| **T3** | Standalone Tool `.exe` + Standalone Game `.exe` | Two Unity builds; tool writes JSON; game watches; no Editor APIs in tool, so brushes built from scratch in UI Toolkit Runtime | High after build | 12-15 | Eventual destination (Phase 2+) |
| **T4** | In-game editor (single build, F12 toggle) | One `.exe`; runtime mode switch; UI Toolkit Runtime overlay; mod-friendly | High | 8-10 | **CONFLICTS with Karar #115 lock** — REJECT |

### 1.3 Why T2 (and why not T4 or T3 first)

**Why T4 (in-game editor) is REJECTED:**
- Karar #115 (LOCKED): "Fullscreen 'in-game editor' framing REJECTED — mevcut Editor Window kalir, brush UX ve toolbar polish ile evrilir" — `(NLM citation 1, source 66c6d88f)`
- T4 forces rebuilding the entire painter UI in IMGUI Runtime / UI Toolkit Runtime → 8-10 days minimum
- Loses Editor APIs (Selection, Handles, AssetDatabase) — every brush re-invented
- Mod-friendliness is a Phase 2 goal, not P1 demo necessity

**Why T3 (full standalone) is DEFERRED:**
- 12-15 days = roughly tripling current painter consolidation effort
- Same UI rebuilding cost as T4 (tool needs runtime UI too)
- Asset reference sharing (sprite GUID list between builds) adds complexity
- BUT: T3 is the eventual destination if user wants user-shippable mod kit

**Why T2 (Editor + Player Build bridge) is RECOMMENDED:**
- Authoring UX stays in Unity Editor → keeps Day 5a Preview Pane, drag-handle infra, AssetPostprocessor
- Player Build runs in fullscreen on Monitor 2 → satisfies the "game in separate window like Sang" intent
- Hot-reload mechanism: Editor writes `STAGING/live/room_current.json` whenever an asset is placed/painted, Player has `FileSystemWatcher` on that path → rebuilds the room
- Honors existing `RoomManifestSO.jsonLayout` (TextAsset) pipeline — the JSON contract already exists
- 4-5 days = same magnitude as one Codex P0 wave
- Natural extension of LOCKED `RoomDescriptorEditorOverlay v0` (F9 runtime overlay, Karar #115 follow-up)

### 1.4 T2 implementation outline (5-day phase plan)

| Day | Work | Files (no code yet, just scope) |
|---|---|---|
| D1 | JSON schema lock: room layout = `{ floorTiles[], cliffCells[], propInstances[], colliderOverrides[] }`. Versioning (v1). Editor serializer | `Assets/Scripts/Map/Data/RoomManifestSO.cs` (extend `jsonLayout` schema), new `Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs` |
| D2 | Player Build runtime watcher + reload path: `Assets/Scripts/Live/LiveRoomReloader.cs` runs `FileSystemWatcher` on `Application.streamingAssetsPath/live/room_current.json` | New: `Assets/Scripts/Live/LiveRoomReloader.cs`, `Assets/StreamingAssets/live/` directory |
| D3 | Editor → Player bridge: every PAINT/PLACE in `VisualEditorScenePainter.cs:546` and `RoomPainterScenePlacer.cs` triggers serializer + write to `Application.streamingAssetsPath/live/room_current.json` | `VisualEditorScenePainter.cs`, `RoomPainterScenePlacer.cs` |
| D4 | Build & verify: build standalone Player to `Builds/RIMA_LiveTool/`, dual-monitor smoke test | Build settings only |
| D5 | UX polish: Editor "Launch Live Player" button → spawns `Builds/RIMA_LiveTool/RIMA.exe` from inside `RimaRoomPainterWindow`; statusbar shows "Player connected"; auto-rebuild Player on script changes | `RimaRoomPainterWindow.cs` add button |

### 1.5 Risks & mitigations (T2)

| Risk | Mitigation |
|---|---|
| Player Build can't find shared assets (Editor uses AssetDatabase GUIDs, Player uses Resources/Addressables) | Build a `RuntimeAssetRegistry` ScriptableObject at build time that maps GUID → loaded Sprite/Prefab in Player. ~80 LOC. |
| FileSystemWatcher on Windows fires multiple events per write — Player rebuilds 3x per paint | Debounce by 100ms on Player side. Standard pattern. |
| Editor and Player out of sync if user edits asset metadata in inspector but not via painter | Statusbar warning "Live JSON stale" + "Force Resync" button |
| Build process slow (Unity build = 30-60s) | One-time cost. Use Development Build (faster, no IL2CPP). Re-build only on script changes. |
| Karar #115 conflict (in-game editor rejected) | T2 is NOT in-game editor — game runs fullscreen separate but is a regular Player Build with a JSON watcher. User authority confirms OR rejects. |

### 1.6 If user rejects T2, fallback

T1.5 — keep current Editor extension but install **FastScriptReload** (free, open source) to eliminate Domain Reload cost. ~1 day. Doesn't give "game in separate window" but solves 80% of the velocity pain.

---

## 2. Asset Layer Architecture (Decision 3 — the deep-dive request)

### 2.1 The canonical that already exists (NLM)

From the locked design notebook (Karar #115 + Path C Hybrid Lock):

| Layer | Sorting Order | What goes here | Physics |
|---|---|---|---|
| BG_Void | -500 | Far-back void | None |
| BG_Far | -420 | Far ruins / silhouettes | None |
| BG_Mid | -350 | Floating islands (parallax) | None |
| Floor (Cliff base) | -1 | `CliffTilemap` (faces drop into void below floor) | None |
| Floor (Walkable) | 0 | `ArenaFloor` Tilemap | None |
| Decor | 100 | Pillars, braziers, statues — props with Y-sort | Capsule/Circle |
| Player/Mob | 300 | Characters via `SortingGroup` | Capsule |
| FrontFX | 600 | Front fog, hit flashes, additive | None |

**Critical:** RIMA already has 4 functional tiers in the locked schema. The user's "4 layer" suggestion is a **refinement of L2-L5**, not a new system.

### 2.2 User's 4-layer suggestion mapped to canonical

| User layer | Maps to | Sorting order | Physics |
|---|---|---|---|
| Cliff (en alt, dekor cliff) | **L2 Cliff base** (NLM `Floor` layer, order -1) | -1 | None |
| Walkable decor (yerde, üstüne basılır, NO RB) | **L4 Walkable decor** (NEW — split from L3 Decor) | 50 | None |
| Wall decor (cave/pillar/küçük duvar, RB var boyutu kadar) | **L5 Wall blocker** (NLM `Decor` 100) | 100 (Y-sort) | Box/Circle/Capsule |
| Gameplay object (chest/interactable) | **L6 Gameplay** (NEW — split from L5 to separate triggers from blockers) | 150 (Y-sort) | Trigger BoxCollider2D |

### 2.3 The 5-layer alternative (split Cliff into Base + Face decor)

User asked: "5 layer alternatifi: cliff base + cliff mount/face decor ayrı sayılırsa". This is the **mounting_*.prefab** question — those 15 prefabs in `Assets/Prefabs/Props/ShatteredKeep_PixelLab/` are decorative props attached to cliff faces (hanging chains, wall-mounted braziers, vines). They are NOT cliff base tiles — they belong on their own tier between cliff base and walkable decor.

Opus recommendation: **YES, split into 2 cliff tiers.** This is the L2/L3 split:
- **L2 Cliff base** — `CliffTilemap` Tilemap layer, `DirectionalCliffTile_Hades.asset` 8-direction TileBase, sorting order -1. Pure visual, no physics.
- **L3 Cliff face decor** — `mounting_*.prefab` GameObjects with SpriteRenderer, sorting order 50 (above cliff base, below walkable decor characters). NO physics, NO collider. Pivot at top-center so they "hang" off the cliff edge.

### 2.4 Final 6-layer architecture (Opus recommendation)

| # | Layer | Sorting Order | Physics | Y-sort | Folder root | Asset count est. |
|---|---|---|---|---|---|---|
| **L1** | Floor base + decals | 0 (tiles), 10-30 (decals) | None | No (static tilemap) | `Assets/ScriptableObjects/Environment/` (TileBase) + `Assets/Sprites/Environment/PixelLab_Selected_Assets/` (16 floor tiles + decals) | ~30 tiles + decals |
| **L2** | Cliff base (Tilemap) | -1 | None | No | `Assets/ScriptableObjects/Environment/DirectionalCliffTile_Hades.asset` + `Assets/Sprites/Environment/KitB_Cliff/` | 1 TileBase + 8-dir variants |
| **L3** | Cliff face decor (prefab) | 50 | None | Optional Y-sort | `Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_*.prefab` | ~15 prefabs |
| **L4** | Walkable decor (prefab) | 80 | None (or trigger if interactive) | Y-sort | `Assets/Prefabs/Environment/Decorations/` (TBD content — bones, rune circles, plinths) | TBD (Codex appendix) |
| **L5** | Wall blocker (prefab) | 100 | Static + Box/Circle/Capsule | Y-sort | `Assets/Prefabs/Obstacles/` (Chasm, NarrowPassage, StoneColumn) + `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_*.prefab` + `Assets/Prefabs/Environment/Walls/AssetPackV3/*.prefab` | ~14 statue + 3 obstacles + 8 walls + 12 placeholders = ~37 |
| **L6** | Gameplay object (prefab) | 150 | Trigger BoxCollider2D | Y-sort | `Assets/Prefabs/Chest.prefab`, `Assets/Prefabs/MapFragment.prefab`, `Assets/Prefabs/RewardPickup.prefab`, Gate prefabs | ~5+ |
| **L0 (existing)** | BG (-500..-250) | -500..-250 | None | No | Parallax layers | Already locked |
| **L7 (existing)** | FrontFX | 600 | None | No | VFX folder | Already locked |

**Rationale per split decision:**
1. **L2/L3 split (cliff base vs cliff face decor)** — different render contracts. L2 is Tilemap (static, batched, no Y-sort). L3 is GameObject (sortable, per-instance pivot). Mixing them in one layer = either lose Tilemap batching OR lose per-instance Y-sort. Split is mandatory once `mounting_*.prefab` exists.
2. **L4/L5 split (walkable decor vs wall blocker)** — collider on/off. User's verbatim "walkable decor NO RB, wall decor RB boyutu kadar" matches exactly. Without the split, the AssetPostprocessor cannot keyword-detect which is which without manual metadata.
3. **L5/L6 split (wall blocker vs gameplay object)** — non-trigger vs trigger collider. User's verbatim "gameplay object (chest/interactable)" implies interaction triggers, which need `isTrigger=true`. Keyword detection (`chest`, `pickup`, `item`) already lives in `RoomPainterPhysicsRules.cs:43,40-42`.

**Why not 7 layers?** Splitting Walkable decor into "ground decal" + "floor prop" adds a tier without a render-contract justification (both are no-physics, both Y-sort at floor level). Save the granularity for keyword filters within L4.

### 2.5 RoomPainterPhysicsRules — gap audit

Current rules (32 keywords) inventory:
- ✅ Covers: wall, cliff, pillar, column, door, altar, brazier, banner, prop, ritual, enemy, npc, pickup, item, coin, chest, floor, decal, moss, crack, parallax, bg, rift, sky, torch, lamp, light, glow, flame, ember, trigger, zone, tile, wang16, dirt, sand, stone, cobble
- ❌ MISSING for the new architecture:
  - `mounting` → L3 Cliff face decor (currently falls through to default → physics applied wrongly)
  - `statue` → L5 Wall blocker (currently falls through to default)
  - `pedestal` / `plinth` → L5 Wall blocker
  - `rune_circle` / `runecircle` → L4 Walkable decor (no physics)
  - `bone_cluster` / `bones` → L4 Walkable decor
  - `vine` / `chain` → L3 Cliff face decor (if attached to cliff) or L4 walkable

**Fix:** add ~6 new keywords to `RoomPainterPhysicsRules.cs:26-66` array. Also add a `RoomLayer` field to `PhysicsConfig` struct so the painter palette can filter by layer. ~30 LOC.

### 2.6 Sorting layer registration

Existing Unity Sorting Layers (from `ProjectSettings/TagManager.asset` — Codex will confirm count): likely `Default`, `Floor`, possibly more. The 6-layer architecture wants these registered:
- `BG_Far` (-500..-250 range, already partially used)
- `Floor` (L1 + L2, orders 0 and -1)
- `Decor_Cliff` (L3, order 50) — **NEW**
- `Decor_Floor` (L4, order 80) — **NEW**
- `Default` (L5 + L6, orders 100/150 with Y-sort)
- `FrontFX` (order 600, already locked)

**Registration cost:** 2 new sorting layers in `ProjectSettings/TagManager.asset` (one-time, ~5 min in Unity Editor).

---

## 3. Per-Prefab Collider Drag-Handle (Decision 2)

### 3.1 Current state (Day 5b infrastructure)

`Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs` (262 LOC) already implements:
- ✅ SceneView `Handles.FreeMoveHandle` for Box / Circle / Capsule colliders
- ✅ 4 corner + 4 edge drag handles for box (8 total — matches user's "drag-handle" intent)
- ✅ Outline draw (green = solid collider, yellow = trigger)
- ✅ `Undo.RecordObject` + `EditorUtility.SetDirty`
- ✅ Live label showing dimensions ("Box 1.20 x 0.80")

What's MISSING:
- ❌ Shape selector dropdown — currently shape is "whatever component is on the GameObject"
- ❌ **Prefab asset write path** — current code writes only to scene instance. If user is editing a prefab, changes persist via Unity's normal prefab override system; but to ensure ALL instances pick up the change, need explicit `PrefabUtility.ApplyPrefabInstance` or operate on the prefab opened in Prefab Mode.
- ❌ PolygonCollider2D support
- ❌ Mode-aware: only show drag handles when "Collider Mode" is active in painter, not always-on

### 3.2 The recommended write path

Per Unity docs, the cleanest "edit on prefab → all instances inherit" workflow is:

**Option A — Open prefab in Prefab Mode**
- User selects prefab asset in Palette → painter calls `AssetDatabase.OpenAsset(prefab)` → Prefab Mode opens → drag handles edit the prefab root directly → save with Ctrl+S → all scene instances reload via Unity's prefab system automatically.
- Pro: Native Unity flow, robust, well-tested
- Con: Modal Prefab Mode interrupts painter workflow

**Option B — Edit scene instance + apply override**
- User selects scene instance in painter → drags handle → painter calls `PrefabUtility.ApplyPropertyOverride` on the changed `Collider2D` properties (offset, size, radius) → prefab asset updated → other instances re-evaluate.
- Pro: Stays in painter context
- Con: Need to detect "is this instance from a prefab?" and handle disconnected instances correctly

**Opus recommendation:** **Option A** for first iteration (Day 4 of phase plan). The modal Prefab Mode interruption is acceptable cost for robustness, and Unity handles all the multi-instance updates automatically. Option B can be added in V2 once V1 is shipped.

### 3.3 Shape selector + workflow

```
Painter Inspector → Selected: statue_05_*.prefab
  ┌─────────────────────────────────┐
  │ Collider:  ◉ Box  ○ Circle  ○ Capsule  │
  │            [Edit in Prefab Mode]            │
  │ Current size: 0.48 x 0.84                   │
  │ Offset:       (0, 0.32)                     │
  │ Trigger:      [ ]                           │
  │ Physics layer: Prop                         │
  └─────────────────────────────────────────────┘
```

When user clicks `[Edit in Prefab Mode]`:
1. `AssetDatabase.OpenAsset(prefab)` opens Prefab Mode
2. RoomPainterColliderEditor's `OnSceneGui` already kicks in because `Selection.activeGameObject` is the prefab root
3. User drags handles, sees live dimensions
4. Ctrl+S saves prefab → all scene instances refresh

**Polygon collider DEFERRED** to Phase 2 — most RIMA props are Box/Circle/Capsule (per agy verdict "Hades uses Circle/Capsule at pivot base for smooth sliding"). Polygon adds 4x complexity (vertex insert/delete handles).

### 3.4 Implementation scope

| Work | LOC est. | Files |
|---|---|---|
| Shape selector dropdown in Inspector | ~40 | `Assets/Editor/RoomPainter/Inspector/Sections/PhysicsSection.cs` |
| "Edit in Prefab Mode" button + open prefab + auto-select root | ~30 | `Assets/Editor/RoomPainter/Inspector/Sections/PhysicsSection.cs` |
| Collider component swap helper (Box ↔ Circle ↔ Capsule on prefab root, preserves size as best-fit) | ~80 | New: `Assets/Editor/RoomPainter/AssetPipeline/ColliderShapeSwapper.cs` |
| Statusbar "Editing prefab: foo" indicator | ~20 | `RimaRoomPainterWindow.cs` |
| **Total** | **~170** | |

---

## 4. Triple-AI summary paragraphs

### agy (external research, ~800 words inline)
> Sang Hendrix's tool is a pure in-process editor — JS overlay on PixiJS scene graph, no IPC. That pattern cannot literally transfer to Unity because C# Domain Reload and AssetDatabase are Editor-only. For Unity equivalents, the **Editor Extension + Standalone Player Bridge** pattern (Editor on Monitor 1, built `.exe` on Monitor 2, `FileSystemWatcher` syncing JSON) gives 80% of Sang's velocity at 4-5 days of work. Hades/CoM/Hyper Light Drifter use Capsule/Circle colliders at sprite base pivots for smooth sliding (not Box). RIMA's wall-less Elysium V1 fits a 6-layer architecture: BG / Floor / Cliff base / Cliff face decor / Walkable decor / Wall blocker / Gameplay object, with `mounting_*.prefab` explicitly belonging to the cliff face decor tier (sort order 50, no physics).

### Codex (codebase reality check, pending — appended below in Section 8)
*Codex dispatch `baqafto8d` is running xhigh effort against RIMA codebase. Expected output: asset inventory counts, RoomPainterPhysicsRules gap analysis, RuntimeRoomManager JSON load feasibility, LOC estimates for migration. Will be appended to Section 8 when complete.*

### Opus synthesis paragraph
The three decisions are mutually reinforcing: **T2 live tool** depends on **clean asset layer metadata** to serialize/deserialize room layouts, which depends on **per-prefab collider authoring** to ensure the metadata round-trips correctly. The canonical NLM 4-tier architecture (Karar #115 + Path C Hybrid) is the foundation — we are not designing from scratch but refining it. The user's 4-layer suggestion is consistent with the canonical (maps to L3-L6); the 5-layer alternative (split cliff into base + face) is the right answer because `mounting_*.prefab` has a fundamentally different render contract from `DirectionalCliffTile_Hades` (GameObject Y-sort vs Tilemap batch). T2 is the live tool answer because it threads the needle between Karar #115's REJECTION of in-game editors and the user's Sang-style intent (game in separate fullscreen window) — the Player Build IS a "regular game" that just happens to read a live-updating JSON. Per-prefab collider drag-handle leverages the 262 LOC `RoomPainterColliderEditor.cs` Day 5b investment by adding a "Edit in Prefab Mode" button to the Inspector — small surgical extension, not refactor. Estimated total impl effort: **9-12 days** across all 3 decisions (T2 live tool 4-5d, asset layer migration 3-4d, collider drag-handle 1-2d, sorting layer registration 0.5d, RoomPainterPhysicsRules keyword additions 0.5d). The biggest risk is **migration scope** — Codex appendix will give the exact count of prefabs that need RoomLayer metadata backfill.

---

## 5. Revised Phase Plan (supersedes UNIFIED_PAINTER_PLAN.md Days 2-6)

| Day | Content | Dispatcher | LOC | Decision tier |
|---|---|---|---|---|
| **D1** | Approval of THIS document + Open Question lock | Orchestrator | 0 | — |
| **D2** | Asset layer architecture LOCK: add `RoomLayer` enum + sorting layer registration + extend `RoomPainterPhysicsRules` keywords (mounting/statue/pedestal/rune/bone). Backfill existing prefab metadata via batch. | Sonnet impl + Codex review | ~250 | Decision 3 |
| **D3** | Painter Mode tab (Tile / Cliff / Decor / Object 4 modes) + Layer sub-filter (L1-L6) — uses `BrushCategory` + `TargetLayer` enums (already exist in `Enums.cs:3,17`) | Sonnet | ~300 | Decision 3 |
| **D4** | Per-prefab collider drag-handle: Shape selector + "Edit in Prefab Mode" button + ColliderShapeSwapper helper | Sonnet | ~170 | Decision 2 |
| **D5** | DirectionalCliffTile activation: wire mounting_*.png variants OR confirm cliff_mounting issue is metadata-only (per `UNIFIED_PAINTER_PLAN.md` Section 3.1 Fix 1) | Sonnet | ~80 | Decision 3 follow-up |
| **D6-7** | **T2 Live Tool Phase 1:** JSON schema lock + RoomLayoutSerializer + Editor "Launch Live Player" button | Sonnet + Codex review | ~250 | Decision 1 |
| **D8-9** | **T2 Live Tool Phase 2:** LiveRoomReloader on Player side + FileSystemWatcher + smoke test dual-monitor | Sonnet + Codex review | ~200 | Decision 1 |
| **D10** | UX polish, statusbar live indicators, Editor toolbar entry "Live Mode: ON/OFF" | Sonnet | ~80 | Decision 1 |

**Total LOC est:** ~1330 (vs UNIFIED_PAINTER_PLAN.md baseline ~1150). Net add ~180 for T2 live tool slice. **Total days:** 10 (vs 6 in original).

---

## 6. Open Questions (user decision required)

1. **Live tool tier — T2 (recommended) or T3 (full standalone) or T1.5 (FastScriptReload only)?**
   - T2 honors Karar #115 lock AND satisfies "game in separate window" intent
   - T3 is the eventual destination but +7-10 extra days
   - T1.5 (1 day) gives 80% velocity without the dual-window pattern

2. **Karar #115 "in-game editor REJECTED" — is T2 acceptable under that lock?**
   - T2 is NOT an in-game editor; it's a Player Build + Editor extension with JSON bridge
   - User authority needed to confirm OR revoke Karar #115 if T4 becomes desirable

3. **Asset layer count — 4 (your verbatim) / 5 (cliff split alt) / 6 (Opus recommended)?**
   - 6 is the recommendation because mounting_*.prefab vs DirectionalCliffTile_Hades have different render contracts (GameObject vs Tilemap)
   - 5 collapses Walkable + Wall decor (but then keyword detection ambiguous)
   - 4 collapses Cliff base + Cliff face (but then mounting_*.prefab loses pivot control)

4. **mounting_*.prefab pivot — top-center (hangs off cliff) or bottom-center (sits on ground)?**
   - Top-center matches "cliff face decor" intent (chain hangs DOWN from cliff lip)
   - Bottom-center matches "wall decor sitting on floor next to cliff"
   - These 15 prefabs need a one-time pivot decision

5. **Per-prefab collider authoring — Option A (Prefab Mode) or Option B (instance + apply override)?**
   - Option A recommended for V1 (robust, native Unity flow)
   - Option B for V2 if Prefab Mode interruption is too jarring

6. **Save file format — JSON (current `RoomManifestSO.jsonLayout` TextAsset) or binary (faster reload) or ScriptableObject (Unity-native)?**
   - Current pipeline is JSON → keep JSON
   - For live tool, JSON has the killer feature: human-readable, diffable, mergeable
   - Binary saves 2-5x reload speed but loses human edit; defer to perf-pass

7. **Asset migration scope — backfill all 244+ existing prefabs with RoomLayer metadata, or only the ones touched in Phase 1 demo?**
   - Phase 1 demo locks: Warblade + 5 rooms + 4 mobs + Map Fragment + Gate — only ~30 prefabs are critical
   - Full backfill = ~244 prefabs × 30s = 2 hours of mechanical work (Sonnet batch)
   - Recommendation: **Phase 1 critical (~30) first, full backfill at D2 close**

---

## 7. Risk & Migration

| Risk | Impact | Mitigation |
|---|---|---|
| Karar #115 conflict | Decision authority needed | User-facing question in Section 6 |
| 244+ prefab migration breaks compile / runtime references | High | Backfill as batch, test scene smoke after each 50 prefabs |
| T2 Player Build asset loading mismatches Editor (Resources vs Addressables vs AssetDatabase) | High | RuntimeAssetRegistry SO baked at build time; document the GUID → asset map |
| DirectionalCliffTile activation needs Day 2 not Day 5 (cliff_mounting is current pain) | Medium | Reorder phase plan to Day 2 if user wants cliff fix first |
| Sorting layer addition (2 new layers) breaks existing scene render order | Low | Test all 5 Phase 1 rooms after registration; Sorting layers are additive |
| RoomPainterPhysicsRules new keywords misfire on legacy assets (e.g., "stone" appears in `stone_floor` AND `stonecolumn`) | Medium | Audit keyword order in `Rules[]` array (`stonecolumn` before `stone`); current order has `cliff` before `cliff_mounting` would cause same — needs Codex audit |
| Live tool JSON drift between Editor and Player (race conditions) | Medium | Lock file pattern + version stamp + Editor "Force Resync" button |

---

## 8. Codex Appendix (CODEBASE REALITY CHECK)

*Codex dispatch ID: `baqafto8d`, effort xhigh, started 22:16. Will be appended when complete. Section 8.1-8.4 will mirror the Codex task structure (A/B/C/D).*

**Stub findings already verified by orchestrator direct read:**

### 8.1 Inventory (partial, from direct file system inspection)
- **TileBase assets:** 4 in `Assets/ScriptableObjects/Environment/` — `CliffPlacementRules_Hades.asset`, `CliffTile_Hades.asset`, `DirectionalCliffTile_Hades.asset`, `VoidBlocker_Tile.asset`
- **Floor sprite folders:** `KitB_Cliff/`, `KitC_BG/`, `Phase0_ScaleTest/`, `PixelLab_Selected_Assets/`, `RIMA_AssetParts_v2/`, `ShatteredKeep_PixelLab/`
- **Wall prefabs (AssetPackV3 + Placeholders):** 8 + 12 = 20 prefabs
- **Obstacle prefabs:** 3 (Chasm, NarrowPassage, StoneColumn)
- **mounting_*.prefab:** 15 (verified count `mounting_00` through `mounting_14`)
- **statue_*.prefab:** 14 (verified count `statue_00` through `statue_13`)
- **Gameplay prefabs:** Chest, MapFragment (×2 — Prefabs/ and Prefabs/Environment/), RewardPickup, PlayerStartMarker — total ~5

### 8.2 Codebase facts (orchestrator-verified)
- `CliffAutoPlacer.cs:14-17` confirms `cliffTile` is a **SINGLE `TileBase` field** (not List) — Fix 1 from UNIFIED_PAINTER_PLAN.md
- `DirectionalCliffTile.cs:8-17` confirms 8 direction `Sprite[]` arrays — already supports per-direction variants natively
- `RoomManifestSO.cs:7-12` is minimal — `roomId`, `TextAsset jsonLayout`, `Vector2Int defaultCameraBounds`, `RoomManifestSO[] connectedRooms` — schema **room layout JSON deserialization happens elsewhere (RuntimeRoomManager)**
- `RuntimeRoomManager.cs` is 1434 LOC — significant runtime infrastructure exists; will hold the Live tool reload hook
- `Enums.cs:17` already declares `TargetLayer { L1, L2, L3, L4, L5, L6 }` — **the 6-layer scaffold exists in code** even though it's not consumed by RoomPainter palette yet
- `Enums.cs:23-41` `AssetCategory` enum has 16 entries — does NOT include `CliffFaceDecor`, `WallBlocker`, `GameplayObject` — needs extension

### 8.3 Pending Codex output (~30-45 min)
- Exact LOC of `RoomPainterColliderEditor.cs` (orchestrator: 262 lines)
- AssetPostprocessor behavior summary at import time
- Sorting layer count in `ProjectSettings/TagManager.asset`
- Collider audit table for 37 wall/obstacle/statue/mounting prefabs
- LOC estimates for D2-D10 work items
- 5-bullet Reality Check

---

## 9. Conflicts with Locked Rules

| Rule | Status | Notes |
|---|---|---|
| **Karar #115 — "fullscreen in-game editor REJECTED"** | ⚠️ **CONFLICT POTENTIAL** | T2 is NOT in-game editor (it's Player Build + Editor extension with JSON bridge). T4 would conflict. User authority required if T4 is wanted. |
| `feedback_tool_visibility_4_surfaces` | ✅ COMPATIBLE | T2 keeps painter visible in 4 surfaces; live tool adds 5th surface (statusbar live indicator) |
| `project_topdown_pivot_lock` + `project_high_top_down_3_4_lock` | ✅ COMPATIBLE | 6-layer architecture preserves the 70-80° camera angle |
| `project_walless_v1_hades_elysium_lock` | ✅ REINFORCES | mounting_*.prefab (L3 Cliff face decor) is core to the Elysium aesthetic |
| `feedback_2track_gameplay_decor_strategy` | ✅ COMPATIBLE | Track A (gameplay) = L5/L6 layers; Track B (decor) = L1/L2/L3/L4 layers |
| `feedback_no_pixellab_night_autonomous` | ✅ COMPATIBLE | This plan doesn't trigger any asset generation |
| `project_s110_late_collider_visible_menu_clean` | ✅ REINFORCES | Per-prefab drag-handle is the Day 5b continuation |
| `feedback_orchestrator_delegate_dont_do_yourself` | ✅ COMPATIBLE | D2-D10 explicit Sonnet impl + Codex review delegation |

---

## 10. Relevant file paths (impl scope)

**Existing files to extend:**
- `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` (679 LOC) — shell, add "Launch Live Player" + mode tabs
- `Assets/Editor/RoomPainter/AssetPipeline/RoomPainterPhysicsRules.cs` — add 6 new keywords + RoomLayer field
- `Assets/Editor/RoomPainter/AssetPipeline/RoomPainterAssetPostprocessor.cs` — write RoomLayer metadata at import time
- `Assets/Editor/RoomPainter/SceneAuthoring/RoomPainterColliderEditor.cs` (262 LOC) — extend with Prefab Mode entry point
- `Assets/Editor/RoomPainter/Inspector/Sections/PhysicsSection.cs` — add Shape selector dropdown
- `Assets/Scripts/Core/RuntimeRoomManager.cs` (1434 LOC) — add LiveRoomReloader hook (D8)
- `Assets/Scripts/Map/Data/RoomManifestSO.cs` — extend `jsonLayout` schema (D6)
- `Assets/Scripts/MapDesigner/Brush/Data/Enums.cs` — extend `AssetCategory` with new entries
- `Assets/Scripts/Environment/CliffAutoPlacer.cs` (211 LOC) — Fix 1 (cliff_mounting wire) parallel to Day 5

**New files to create:**
- `Assets/Editor/RoomPainter/LiveTool/RoomLayoutSerializer.cs` (D6)
- `Assets/Editor/RoomPainter/AssetPipeline/ColliderShapeSwapper.cs` (D4)
- `Assets/Scripts/Live/LiveRoomReloader.cs` (D7)
- `Assets/Scripts/Live/RuntimeAssetRegistry.cs` (D6)
- `Assets/StreamingAssets/live/` (directory, D6)

---

## Orchestrator Next Steps

1. User reviews Sections 1-3 (decisions) + Section 6 (open questions)
2. User answers 7 open questions OR delegates remaining ambiguity to orchestrator
3. On approval → D2 Sonnet dispatch begins (asset layer architecture lock + RoomPainterPhysicsRules keyword extension)
4. Codex appendix (Section 8) integrated once `baqafto8d` returns

---

## Appendix A — Raw agy output

See `STAGING/RIMA_LIVE_TOOL_agy_output.txt` (154 lines, ~15KB) for the full agy research dump.

## Appendix B — Raw Codex output

Will be at `STAGING/RIMA_LIVE_TOOL_codex_appendix.md` once `baqafto8d` completes (or `STAGING/RIMA_LIVE_TOOL_codex_output.txt` if Codex prints to stdout instead).
