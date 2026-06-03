# RIMA Room System — DECISION (Model B, data-driven + painter)
Date: 2026-06-03 · Decider: Opus, synthesizing cx + Gemini 3.1 Pro High + Gemini 3.5 Flash High + user requirements.
Status: DECIDED. Implementation = phased, cx-delegated, Opus-orchestrated.

## User requirements (override advisor caution)
- Data-driven rooms; "draw a room" = author data, not bake a scene.
- A PAINTER tool: place/connect POINTS -> walkable floor polygon; place DOORS; place MOB spawns + PLAYER spawn; pick TYPE + SIZE.
- "Generate" -> turns the drawing into a real room (floor + auto-cliff + auto-boundary + doors + spawns).
- LEFT room LIST: select a room -> edit points/doors/spawns -> Regenerate.
- BOTH: Claude can auto-generate rooms procedurally AND user can intervene/edit or draw from scratch. Shared data. SIMPLE UI.
- Demo: randomness but LOGICAL rooms (non-square, flowing); big rooms; boss/elite/reward rooms.

## DECISION

### 1. Data model = single source of truth: `RoomDefinitionSO` (ScriptableObject)
- SO chosen (cx + 3.5 Flash): Unity-native asset refs (tiles/sprites/prefabs/enemies), inspector-editable, list-friendly (matches left-list ask), no parser. JSON = SIDECAR export mirror (cx + 3.1 Pro) for diff/external edit — satisfies user "JSON" intuition without fragile single-source.
- Fields: roomId, displayName, roomType, sizeClass, actId, difficultyBand, weight, tags;
  shape = polygon point list (the user's "connect dots") + baked floor cell mask (RoomData payload);
  doorAnchors, playerSpawn, mobSpawns[], rewardAnchor, cameraBounds; theme refs (floor tiles, cliff sprites, props, lighting).
- One canonical runtime schema (cx risk: kill schema drift between RoomDataJson / StreamingAssets / Act1 json).

### 2. Shape = polygon-of-points (author) -> baked cell mask (runtime)  [recipe->mask hybrid, cx]
- Author the OUTLINE as connected points (user model; even more direct than ASCII mask). Floor = iso cells whose center is inside polygon.
- "Generate" BAKES polygon -> per-cell floor mask + cliffs + boundary + markers. Runtime consumes the BAKED mask (cheap, stable), never re-interprets the polygon live.
- Auto-generator produces polygons/masks procedurally -> SAME RoomDefinitionSO. => TWO PRODUCERS (painter + auto-gen), ONE consumer (builder). User can open any auto room in the painter and tweak.

### 3. Build pipeline: `RuntimeRoomBuilder`
- Consumes a RoomDefinitionSO -> paints floor Tilemap (polygon-fill / baked mask) -> auto-cliff (REUSE `RoomCliffSolver`, directional SW/SE + inward tuck made explicit in builder) -> boundary -> place door triggers + player/mob spawn markers + reward anchor.
- Boundary for demo = `TilemapCollider2D + CompositeCollider2D` (cheaper/safer, handles donut holes) instead of rebuilding the un-saved DCEL EdgeCollider tracer (cx risk note). Support holes from day 1.
- Runtime-only: NO AssetDatabase/PrefabUtility/editor APIs (cx). Preload refs via SO, not Resources.Load per cell. Build during a fade.
- Builds into ONE Arena scene: stable IsoGrid + Ground + Cliff + Collision + Props + Entities.

### 4. Authoring tool: `RIMA Room Painter` (Editor window)  [the user's core ask]
- Left panel: room LIST (RoomDefinitionSO assets, auto-discovered by folder/type). New / Duplicate / Delete.
- Center: SCENE-VIEW drawing on the real iso grid (WYSIWYG): tool modes = Add/Move point, Door, Mob Spawn, Player Spawn, Erase. Type + Size dropdown.
- Generate / Regenerate button -> RuntimeRoomBuilder previews in Arena + saves baked mask into the SO.
- Reuse `UnifiedDesignerCore` / `RoomDataMutator` for mutation; Map Designer as front-door reference. Keep UI SIMPLE (user).

### 5. Run structure = LINEAR typed route (all 3 agree)
- Route: Start -> Combat -> Combat -> Elite -> Reward -> Combat -> Boss.
- Each slot: seeded pick from pool filtered by roomType + sizeClass + tags; anti-immediate-repeat + anti-same-shape-family.
- `RoomRunDirector` (replaces scene-based MapFlowManager for gameplay): holds run seed, route, room index, current def, completion count; calls `RuntimeRoomBuilder.Build(def)` instead of SceneManager.LoadScene.
- Preserve existing gate->reward->draft->exit ORDER (cx). Physical L/R choice door (3.5 Flash) = OPTIONAL later polish, NOT demo.
- StS-style branching graph + portal preview = LATER (post-demo). `RoomTypeData` portal weights deferred until then.

### 6. Demo pool = 9 layouts (lean middle: 3.5=7, cx-better=13, 3.1=12)
- Start 1 (small calm), Combat 4 (medium, organic flowing: cross/ell/hourglass/S-curve), Elite 2 (large, chokepoints/pillars), Reward 1 (small, central pedestal, open), Boss 1 (BigArena, wide-open circular/diamond, no obstacles, danger tint).
- sizeClass enum: Small / Medium / Large / BigArena. Drives camera bounds, spawn density, encounter budget.

### 7. Reuse / Rebuild / Archive (cx)
- REUSE: RoomData (payload), RoomType, RoomCliffSolver (runtime-safe cliff!), UnifiedDesignerCore, RoomDataMutator, RoomDataJson (export), reward/draft/gate flow.
- REBUILD: RoomDefinitionSO/RoomPoolSO, RuntimeRoomBuilder, RoomRunDirector, boundary builder.
- ARCHIVE (only after Arena path verified): RuntimeRoomManager, Map/RoomBuilder.cs, Map/Runtime/RoomLoader.cs, RoomTypeData (until branching).

### 8. Top risks (cx) + mitigations
- Disconnected floor island via bad polygon -> mob unreachable soft-lock => flood-fill connectivity validation at Generate (3.1 Pro + cx).
- Runtime build frame spike => build during fade, batched SetTilesBlock, pooled cliff renderers.
- Iso depth-sort at cliff/entity boundary => bake cliff sort by Y-grid at generation; entities use custom-axis sort (existing lock).
- Static singleton leak (MapFlowManager/RunStats/DraftManager) => RunDirector owns explicit reset + event unsubscribe.

## Implementation phases (cx-delegated, Opus-orchestrated)
1. Data model: RoomDefinitionSO + RoomSizeClass + RoomPoolSO + validation (flood-fill).
2. RuntimeRoomBuilder: polygon/mask -> floor + cliff(RoomCliffSolver) + boundary(Composite) + markers, into Arena scene. Fixed test room first.
3. Room Painter editor tool: scene-view points + doors + spawns + left list + Generate/Regenerate.
4. Auto-generator: procedural RoomDefinitions per type/size.
5. RoomRunDirector: linear typed route + pool pick + connect existing reward/draft/door chain to in-place rebuild.
6. Author demo pool (9 rooms via auto-gen + hand-tune) + archive obsolete spine.

## Files referenced
cx audit = CODEX_DONE.md. Gemini = STAGING/_ax_room_q_31pro.md / _ax_room_q_35flash.md. cx task = STAGING/cx_task_room_system_design_2026-06-03.md.

---

# PART 2 — Tool consolidation, asset layout, UI, sequenced agent plan (Opus decisions, user asked "no tool clutter + where do assets go + UI")

## A. NO TOOL CLUTTER — ONE canonical window
Menu is already organized: `RIMA/Map Designer` (UnifiedMapDesigner, priority 1) is canonical; ALL old painters already live under `RIMA/Legacy/...`; `_archive~` folders do not compile.
DECISION:
- The NEW Room Painter BECOMES the canonical tool under the prime slot `RIMA/Map Designer`. New window class = `RimaRoomForgeWindow` (distinct name; old `RimaRoomPainterWindow`/`UnifiedMapDesigner` names kept to avoid collisions).
- Old `UnifiedMapDesigner` menu is DEMOTED in the same change to `RIMA/Legacy/Map Designer (tile-brush)`. Its logic lib `UnifiedDesignerCore`/`RoomDataMutator`/`RoomCliffSolver` is REUSED by the new window (libs, not windows).
- NO new top-level menu items beyond the one prime slot. All redundant Legacy windows stay under Legacy; ARCHIVE them to `_archive~` only in Phase 6 AFTER the new tool is verified (cx). Net: exactly ONE obvious room tool.

## B. ASSET PLACEMENT (where things go when added)
```
Assets/Data/Rooms/
  Act1/                         RoomDefinitionSO assets (auto-discovered by folder + roomType)
    RoomDef_<Type>_<Name>_NN.asset
    json/                       JSON sidecar export mirror (diff/external edit; not the source of truth)
  RoomPool_Act1.asset           RoomPoolSO (or pool auto-discovers Act1/ by type)
  RoomThemes/
    Theme_ShatteredKeep.asset    RoomThemeSO: refs to floor tiles, cliff sprites, props, lighting profile
```
- NEW VISUAL assets (tiles/sprites/props via PixelLab/imagegen) go to their EXISTING homes: `Assets/Sprites/Environment/<category>/` (floor, CliffKit, props). RoomDef/painter NEVER hardcode paths — they pick from the active RoomThemeSO's lists. The THEME is the binding layer (assets decoupled from rooms).
- Enemy prefabs stay in `Assets/Prefabs/Enemies/`. RoomDef holds mob SPAWN POINTS (positions); enemy TYPES come from an encounter template / run difficulty, not the room geometry.
- Runtime scene: ONE `Assets/Scenes/_Arena.unity`. Old `_IsoGame_MapXX` scenes archived after migration verified.

## C. TOOL UI (single window `RIMA/Map Designer`)
- Left column (~240px): room LIST (auto-discovered RoomDefinitionSO, search + Type filter) · [New][Duplicate][Delete] · selected-room meta (roomType, sizeClass, weight, tags, theme).
- Top toolbar: tool modes [Point][Door][MobSpawn][PlayerSpawn][Erase] · [Generate][Regenerate][Validate] · theme dropdown.
- Drawing surface = SCENE VIEW on the live iso grid (handles/gizmos), NOT inside the window (WYSIWYG).
- Bottom: validation (flood-fill connectivity, #doors, #mobSpawns, playerSpawn present, cells count). Keep UI simple.
- TWO PRODUCERS one data: this window (manual) + auto-generator (procedural) both write the same RoomDefinitionSO; user can open any auto room and edit -> Regenerate.

## D. SEQUENCED PLAN + AGENTS (cx=laurethayday->yekta code; Opus=orchestrate/verify/decisions; rima-qc=review; Gemini=sub-decisions; PixelLab=with user)
- P1 Data model (cx): RoomDefinitionSO, RoomSizeClass, RoomThemeSO, RoomPoolSO(auto-discover), validation(flood-fill), folder `Assets/Data/Rooms/Act1/` + ONE sample TestRoom def. Additive only. [no deps]
- P2 RuntimeRoomBuilder (cx): runtime-only; RoomDef -> polygon-fill floor -> auto-cliff(RoomCliffSolver, explicit SW/SE+tuck) -> boundary(Tilemap+Composite, holes) -> doors+spawn markers; into NEW `_Arena` scene (IsoGrid/Ground/Cliff/Collision/Props/Entities); build the sample room; Opus play-verify. [dep P1]
- P3 Room Painter window (cx): canonical `RIMA/Map Designer` new impl + demote old to Legacy; list + scene-view tools + Generate/Regenerate(calls P2 builder, bakes mask) + Validate; reuse UnifiedDesignerCore. [dep P1,P2]
- P4 Auto-generator (cx + Opus): procedural RoomDefinition gen per type/size (organic polygons) -> writes SO assets. [dep P1,P2]
- P5 RoomRunDirector (cx): linear route Start->Combat->Combat->Elite->Reward->Combat->Boss; seeded pool pick(filter type/size, anti-repeat+anti-shape-family); replace MapFlowManager gameplay path; wire existing reward/draft/gate/door chain to in-place rebuild + fade; Opus full-run verify. [dep P1,P2,P5 after P2]
- P6 Demo pool + cleanup (Opus+user author 9 rooms; cx archive obsolete spine after verified) + commit (GATED). [dep all]
Dependencies: P1 first; P2 next; P3 & P4 parallel after P2; P5 after P2; P6 last. Verify (Opus/rima-qc) after each cx phase.

---

# PART 3 — PIVOT (2026-06-03, Opus): ADOPT existing RoomTemplateSO — do NOT build RoomDefinitionSO
The cx audit MISSED a mature, tested room subsystem under `Assets/Scripts/MapDesigner/Room/`. Building a new `RoomDefinitionSO` would be exactly the tool/data clutter the user forbade. DECISION: reuse it.

## What already EXISTS (reuse as canonical data model)
- `RIMA.MapDesigner.Room.Data.RoomTemplateSO` — roomId, biomeId, `RIMA.RoomType` roomType, RectInt bounds, `doorSockets` (pos/dir/width/isExit), `playerSpawn`, `enemySpawnSockets` (pos/tierHint/avoidRadius), `cameraBounds`, `prefabRef`, `backgroundLayers`, encounter/difficulty/blocker tags, `props`, and **`walkableGrid` (bool[] per-cell floor mask) + IsWalkable()**. == EXACTLY the user's "walkable area + doors + mob/player spawns + type" model.
- `RoomBankSO` — typed pools (combat/elite/boss/merchant/event) + `Pick(roomType, seed)` + `ValidateAll()`. (GAP: no reward/start/treasure/corridor lists — small extension needed.)
- `RoomTemplateValidator` + `RoomValidationIssue`; tests: WalkableGrid / SaveLoad / BankPick / RuntimeSpawn.
- Authoring: `RoomTemplateSaver`/`RoomTemplateLoader` + `RoomTemplateMenu` (Save Selection as Template / Load Into Scene / Validate) — SCENE-paint -> template flow (NOT point-draw + left-list yet).
- A typed LIBRARY already exists: `Assets/Data/Rooms/Library/` (Combat_Small/Medium/Large, Elite_01, Treasure_01, Shrine_01, Boss_Intro_01, Corridor_Linear/LShape_01, Spawn_01).
- Props system (PropPlacer / PropRuntimeSpawner / BridsonPoissonAutoPlacer / PropFootprintValidator).
- `RoomTemplateAdapter.Convert(template) -> RIMA.Data.RoomData` (walkable/wallEdges/encounters/bgLayers) + `ProceduralRoomGenerator`.

## The REAL gap (verified)
Existing runtime render is **RECTANGULAR / top-down**: `RoomBankRuntimeTester.TileToWorld = (x,y,0)` identity, RectInt bounds, wall-edge based, relies on `prefabRef` + painted bg layers. It is NOT RIMA's iso floating-island (0.96 x 0.585 diamond cells) + directional auto-cliff aesthetic. So the genuinely-NEW work is the ISO renderer, not a data model.

## REVISED build plan (supersedes Part 1/2 "Phase 1 build RoomDefinitionSO")
- ~~P1 build RoomDefinitionSO~~ -> CANCELLED. Data model = existing `RoomTemplateSO`/`RoomBankSO`. (cx Phase-1 dispatch flaked/no-op'd anyway — no RoomDefinitionSO files were created; good, do not retry it.) Only small adds: RoomBankSO reward/start lists if needed; optional `RoomSizeClass` (or reuse bounds/tags).
- **P2 IsoRoomBuilder (cx, runtime, THE core new piece):** consume RoomTemplateSO -> paint ISO floor from `walkableGrid` (iso cell map 0.96x0.585, NOT identity) -> auto-cliff via `RoomCliffSolver` (directional SW/SE + inward tuck) -> boundary (TilemapCollider2D+CompositeCollider2D, holes ok) -> place door triggers + player/enemy spawns from sockets, into ONE `_Arena` scene. Test with `Combat_Small_01` from the Library.
- **P3 Room Painter window (cx):** point-polygon draw -> bake to `walkableGrid`; place door/player/enemy sockets; LEFT room list (auto-discover Library RoomTemplateSO); Generate/Regenerate (calls IsoRoomBuilder preview). Canonical `RIMA/Map Designer`; reuse RoomTemplateSaver/Loader + UnifiedDesignerCore.
- **P4 Auto-generator (cx+Opus):** procedural RoomTemplateSO (organic walkableGrid per type/size) -> writes to Library.
- **P5 RoomRunDirector (cx):** linear typed route Start->Combat->Combat->Elite->Reward->Combat->Boss; `RoomBankSO.Pick` per slot (anti-repeat); replace MapFlowManager gameplay path; wire existing reward/draft/gate/door chain to in-place IsoRoomBuilder rebuild + fade.
- **P6 demo pool + cleanup + commit (gated).**
Net effect of pivot: ~1 phase of work saved + zero new parallel data model = honors "no tool clutter".
