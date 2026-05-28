# Phase 4: Decor Sprite Production Plan
# Session S103 | HD-2D Shattered Keep | 2026-05-23
# Status: PLAN — awaiting orchestrator approval before Codex dispatch

## 1. Decor Inventory (18 items)

| # | Item | Category | Scene Placement | PixelLab Tool | Init Image | Prompt Sketch (≤40 words) | Output Dims |
|---|------|----------|-----------------|--------------|------------|--------------------------|-------------|
| 1 | Wall Torch Sconce | Lighting | Wall-mounted N/W | `create_map_object` | ref (2) | Wrought iron wall sconce, single flame torch, medieval dungeon, pixel art, 16px, HD-2D side-front view, warm amber glow | 16x24 |
| 2 | Hanging Brazier | Lighting | Ceiling-hung (floor shadow) | `create_map_object` | ref (3) | Hanging iron brazier on chain, lit coals orange glow, dungeon ceiling fixture, pixel art 16px HD-2D | 16x32 |
| 3 | Floor Brazier | Lighting | Floor-anchored center | `create_map_object` | ref (1) | Large floor-standing iron brazier, burning coals, dungeon, pixel art 16px HD-2D orthographic, tall flame | 16x24 |
| 4 | Tattered Banner | Wall Decor | Wall-mounted N face | `create_map_object` | ref (5) | Shattered Keep heraldic banner, torn fabric, dark red, gold rune symbol, dungeon wall, pixel art 16px | 16x32 |
| 5 | Chain Loop | Wall Decor | Wall-mounted N/W | `create_map_object` | ref (6) | Heavy iron chain loop fixed to stone wall, rust-stained, dungeon, pixel art 16px HD-2D | 16x16 |
| 6 | Runic Plaque | Wall Decor | Wall-mounted N face | `create_map_object` | ref (7) | Stone wall plaque, glowing blue runes carved into granite, dungeon, pixel art 16px HD-2D | 16x16 |
| 7 | Broken Niche Statue | Wall Decor | Wall-mounted corner niche | `create_map_object` | ref (4) | Cracked stone statue torso in wall niche, medieval dungeon, dark moss, pixel art 16px HD-2D | 16x24 |
| 8 | Debris Pile | Floor Scatter | Floor-anchored random | `create_map_object` | ref (8) | Rubble pile broken stone fragments, dungeon floor, pixel art 16px HD-2D top-down slight pitch | 16x12 |
| 9 | Skull Pile | Floor Scatter | Floor-anchored corners | `create_map_object` | ref (6) | Small pile of human skulls and bones, dungeon floor, pixel art 16px HD-2D slight isometric pitch | 16x12 |
| 10 | Broken Pottery | Floor Scatter | Floor-anchored random | `create_map_object` | ref (8) | Shattered clay pots fragments, dungeon floor scatter, pixel art 16px HD-2D orthographic slight pitch | 16x10 |
| 11 | Blood Stain | Floor Scatter | Floor-anchored (decal) | `create_topdown_tileset` | ref (6) | Dried blood splatter stain, dark stone floor, dungeon, pixel art 16px top-down | 16x16 |
| 12 | Bone Scatter | Floor Scatter | Floor-anchored random | `create_map_object` | ref (8) | Scattered finger bones and rib fragments on dungeon floor, pixel art 16px HD-2D slight pitch | 16x10 |
| 13 | Ritual Sigil | Floor Scatter | Floor-anchored center | `create_topdown_tileset` | ref (4) | Carved stone floor sigil glowing faint violet, ritual circle, dungeon, pixel art 16px top-down | 32x32 |
| 14 | Ritual Altar | Centerpiece | Floor-anchored center | `create_map_object` | ref (4) | Dark stone ritual altar, candles, blood channels carved, dungeon centerpiece, pixel art 32px HD-2D | 32x32 |
| 15 | Sarcophagus | Centerpiece | Floor-anchored offset | `create_map_object` | ref (5) | Stone sarcophagus lid cracked ajar, dungeon, pixel art 32px HD-2D orthographic 35-degree pitch | 32x24 |
| 16 | Fallen Broken Pillar | Centerpiece | Floor-anchored random | `create_map_object` | ref (3) | Broken stone pillar toppled on dungeon floor, rubble base, pixel art 32px HD-2D orthographic | 32x16 |
| 17 | Gate Socket | Centerpiece | Floor-anchored doorframe base | `create_map_object` | ref (1) | Iron gate socket and hinge mount, stone arch base, dungeon entry, pixel art 16px HD-2D | 16x24 |
| 18 | Rune Obelisk | Centerpiece | Floor-anchored corner/accent | `create_map_object` | ref (7) | Black stone obelisk, carved glowing runes blue-violet, dungeon, pixel art 16px HD-2D upright | 16x40 |

Notes on init_image references: ref (N) = `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_4*.png` file N.

---

## 2. Integration — RoomDecorator.cs Companion Script

**New file:** `Assets/Scripts/Environment/Modular/RoomDecorator.cs`
**Namespace:** `RIMA.Environment.Modular`
**Runs after:** `RoomShellBuilder.Rebuild()` (call in same ContextMenu or via chained Awake order)

### DecorLibrary ScriptableObject

**New file:** `Assets/Data/Environment/DecorLibrary.asset`
**New SO class:** `Assets/Scripts/Environment/Modular/DecorLibrary.cs`

| Field | Type | Purpose |
|-------|------|---------|
| `torchSconcePrefab` | `GameObject` | Wall niche torch |
| `hangingBrazierPrefab` | `GameObject` | Ceiling anchor |
| `floorBrazierPrefab` | `GameObject` | Center floor |
| `bannerPrefabs` | `GameObject[]` | North wall banners |
| `chainPrefab` | `GameObject` | Wall chain loop |
| `debrisPrefabs` | `GameObject[]` | Floor scatter pool |
| `skullPilePrefab` | `GameObject` | Corner skulls |
| `ritualSigilPrefab` | `GameObject` | Center floor sigil |
| `altarPrefab` | `GameObject` | Center altar |
| `sarcophagusPrefab` | `GameObject` | Offset centerpiece |
| `obeliskPrefab` | `GameObject` | Corner accent |
| `torchLightPrefab` | `GameObject` | Light prefab (from Phase 3 TorchLight) |
| `riftLightPrefab` | `GameObject` | Light prefab (from Phase 3 RiftLight) |
| `maxTorches` | `int` | Default 8 |
| `maxDebrisPiles` | `int` | Default 12 |
| `maxCenterpieces` | `int` | Default 1 |
| `centerSafeRadius` | `float` | Default 4.0 (units, player-clear zone) |

### RoomDecorator Placement Logic

**Reads from:** `RoomFootprint` (via `footprint` reference on same GO), `DecorLibrary` SO

```
Execution order: RoomShellBuilder.Rebuild() → RoomDecorator.Decorate()
```

| Trigger Cell Type | Logic | Prefab Spawned |
|------------------|-------|----------------|
| North wall boundary cell, every Nth | `x % 3 == 1` and wall occupied above | `torchSconcePrefab` + `torchLightPrefab` child |
| West wall boundary cell, every Nth | `z % 3 == 1` and wall occupied left | `torchSconcePrefab` (rotated 90°) + `torchLightPrefab` child |
| NW inner corner cell | `nwInnerCornerPrefab` present | `chainPrefab` or `skullPilePrefab` (50/50) |
| Floor cell, center zone | Within footprint center 1/3, not safe zone | `ritualSigilPrefab` (once), then `debrisPrefabs` scatter |
| Floor cell, center (if big room) | `widthCells >= 12 && heightCells >= 12` | `altarPrefab` (once, replaces sigil) |
| Floor cell, random scatter | `random.NextDouble() < debrisDensity` | random from `debrisPrefabs` |

**Spawn group name:** `"Decor"` (child of RoomShellBuilder GO, parallel to `Floor_Grid`/`Wall_North`/etc.)

---

## 3. Validation Rules for Spawned Decor

| Rule | Check | Action on Fail |
|------|-------|----------------|
| Player spawn clear | No decor within `centerSafeRadius` (default 4 units) of grid center | Skip placement |
| Door opening clear | No decor within 1.5 cells of any `doorCells` position (Phase 5 field) | Skip placement |
| Max torches | Total torch spawns ≤ `DecorLibrary.maxTorches` | Stop early |
| Max debris | Total debris spawns ≤ `DecorLibrary.maxDebrisPiles` | Stop early |
| Max centerpieces | Altar/Sarcophagus/Obelisk total ≤ `DecorLibrary.maxCenterpieces` | First one wins, rest skipped |
| Overlap check (Editor) | `Physics.OverlapSphere(pos, 0.4f)` vs Decor layer | Skip placement, log warning |

---

## 4. Production Order — First 5 Items (Highest Impact / Cheapest Credit)

| Priority | Item | Reason |
|----------|------|--------|
| 1 | Wall Torch Sconce | Every room needs it; multiplied 8x per room; lights the whole scene |
| 2 | Debris Pile | Floor scatter fills empty floor fast; 3-4 variants = visual variety |
| 3 | Tattered Banner | Covers large north wall surface; instant Shattered Keep read |
| 4 | Ritual Altar | Single centerpiece that sells "dungeon" to player immediately |
| 5 | Ritual Sigil (floor) | Topdown tile; cheap to produce; pairs with altar; doubles as spawn marker |

---

## 5. Open Questions for User

1. **TorchLight / RiftLight prefab paths** — Phase 3 atmosphere prefabs are in flight; what are their final prefab paths under `Assets/Prefabs/`? RoomDecorator needs exact path to reference.
2. **Decor layer mask** — should decor objects live on a dedicated `Decor` layer, or reuse `Environment`? Affects overlap checks and camera culling.
3. **ASCII 'D' door character scope** — Phase 5 adds 'D' to footprint ASCII. Should 'D' cells also suppress decor spawning in Phase 4, or handle that in Phase 5 only?
4. **Hanging brazier anchor** — ceiling-hung objects need a Y offset. Is ceiling height fixed (e.g., 4 units above floor)? Or should RoomDecorator read a `ceilingHeight` field from somewhere?
5. **Credit budget for PixelLab** — all 18 items at `create_map_object` ~1 credit each = ~18 credits. Confirm OK, or cut list to priority-5 first batch (~5 credits)?
