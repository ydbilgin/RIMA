# Map System Design v1 — Top-Down + Fake-Iso

> **Pivot LOCK:** Top-down + fake-iso (Children of Morta model)
> **Created:** 2026-05-22 (S98 planning)
> **Status:** Design draft — implementation phases G-J
> **Existing infrastructure:** RIMA already has `Assets/Scripts/MapDesigner/**` (very large), `Assets/Scripts/Map/RoomBuilder.cs`, `Assets/Scripts/Core/DoorTrigger.cs`, `LegacyRuntimeRoomManager.cs`, `RoomTransitionFX.cs`. This doc adapts that to top-down + adds JSON-driven layout layer.

---

## 1. Room Types (size + topology categories)

| Type | Grid Size | Shape | Use case | Tile complexity |
|---|---|---|---|---|
| **Spawn / Hub** | 16×12 | Square | Sprint start, NPC interact, no combat | Low — 1 material + 1 prop cluster |
| **Corridor Linear** | 8×24 (or 24×8) | Long thin | Transition between encounters | Low — 1 material flow |
| **Corridor L-shape** | 16×16 | L bent | Turn between encounters, asymmetric | Medium — 2 material at bend |
| **Combat Small** | 16×12 | Square | 2-3 mob single wave | Medium — 2 material zones + 2 props |
| **Combat Medium** | 24×18 | Square or square+notch | 4-6 mob single wave + cover | Medium-High |
| **Combat Large** | 32×24 | Square (existing RoomBuilder default) | Multi-wave 8+ mobs, multiple cover lanes | High — 3+ materials + obstacle clusters |
| **Treasure / Vault** | 12×10 | Small square | Chest + optional elite | Low — 1 material + centered prop |
| **Shrine / Lore** | 12×10 | Hex-ish (3 notches) | Narrative beat, 0-1 mob, env detail | Medium — 1 material + decorative props |
| **Elite Arena** | 20×16 | Square arena (no cover lanes) | Single elite duel, open space | Low — 1 material wide |
| **Boss Intro** | 24×12 | Wide rectangular antechamber | Tension build, no enemies, audio cue | Low |
| **Boss Arena** | 40×30 | Large oval/square with center | 1 boss multi-phase | High — center radial + outer ring |
| **Hidden Room** | 10×8 | Small | Optional, lore/cosmetic reward | Low |
| **Atrium (girinti-çıkıntı)** | 24×24 outer + 8×8 alcoves | Square + 2-4 alcoves | Architectural variety, cover variety | High |
| **Megaroom (kocaman alan)** | 48×36+ | Open expanse | Spectacle, sub-encounter cluster | Variable |

**RoomBuilder.cs current default:** 32×24 with 2-tile wall, 2-tile door gaps N/S/E/W center → maps to **Combat Large**. KEEP this as primary template. Add other types as JSON layouts.

**Grid unit:** 1 cell = 1×1 Unity unit = 64×64 px tile (matches char PPU 64).

---

## 2. Room Topology — Girinti / Çıkıntı (non-square shapes)

| Shape | Description | Implementation |
|---|---|---|
| **Square** | Simple rect | Default RoomBuilder; floor=interior, wall=perimeter |
| **L-shape** | Two rect unioned | Two `roomRect` definitions in JSON; floor union; wall on outer perimeter only |
| **T-shape** | Three rect unioned at center | Three `roomRect` definitions; floor union; wall perimeter |
| **Hex/octagonal** | Square + 4 corner cuts (45°) | Floor = square minus corner triangles (mask); wall = diagonal segments + N/S/E/W |
| **Alcove room** | Main rect + 2-4 small notches into walls | Main `roomRect` + array of `alcoves` (small rects attached to one wall edge); wall: main perimeter minus alcove openings + alcove perimeter except shared edge |
| **Atrium with center island** | Outer rect + center solid block | `roomRect` + `inner_obstacle_rect` (un-walkable, but no wall — décor pile, statue) |
| **Open expanse with pillars** | Megaroom + scattered column GameObjects | `roomRect` large + `prop_clusters[]` for pillar placement |

**JSON shape format:**
```json
{
  "shape_type": "alcove_room",
  "main_rect": { "x": 0, "y": 0, "width": 24, "height": 18 },
  "alcoves": [
    { "wall": "north", "offset_x": 8, "width": 4, "depth": 3 },
    { "wall": "east",  "offset_y": 6, "width": 4, "depth": 3 }
  ]
}
```

Loader builds floor mask + wall perimeter from shape descriptor.

---

## 3. Map Transition Flow (Door → Fade → Load → Spawn)

### Current state (existing code)
- `DoorTrigger.cs` (Core/) — invisible trigger collider, "G" key interact, calls `LegacyRuntimeRoomManager.transition`.
- `LegacyRuntimeRoomManager` — handles transition, has `OnEnemyDeath` callback for door unlock (Hades-style mid-fight lock).
- `RoomTransitionFX.cs` — fade visual.
- 4-directional door (N/S/E/W) at center of each wall.

### Target flow (Phase J polish)

```
[Player enters DoorTrigger collider]
   ↓
[Door highlight VFX + "Press G" prompt]
   ↓
[G key press]
   ↓
[1. RoomTransitionFX.FadeOut(0.3s)]   ← screen blacks
   ↓
[2. Audio duck + footstep mute]
   ↓
[3. CurrentRoom.Deactivate() — disable GameObject, freeze state]
   ↓
[4. CheckpointSystem.Save() — JSON to PersistentDataPath]
   ↓
[5. NextRoom.Activate() — enable, instantiate if not yet]
   ↓
[6. Player.position = NextRoom.spawn_point[from_door]]
   ↓
[7. CameraFollow.bounds = NextRoom.camera_bounds]
   ↓
[8. RoomTransitionFX.FadeIn(0.3s)]    ← screen restored
   ↓
[9. NextRoom.OnEnter() — mob spawn, audio cue, door lock if combat]
```

### Mid-fight door lock (Hades model)

```
[Player enters Combat room]
   ↓
[RuntimeRoomManager.LockAllDoors()]   ← N/S/E/W DoorTrigger.SetActive(false)
   ↓
[Mobs spawn]
   ↓
[Combat]
   ↓
[OnEnemyDeath callback per mob]
   ↓
[Last enemy dies → RuntimeRoomManager.UnlockExits()]
   ↓
[All DoorTrigger.SetActive(true) + unlock VFX (glow + sound)]
```

### Camera bounds per room

Each `RoomManifestSO` has `cameraBounds` Rect. `CameraFollow.cs` lerps to player but clamps to bounds. On transition, bounds swap.

Megaroom: no bounds (free follow). Small rooms: tight bounds (no scroll).

---

## 4. Scene Architecture — Single Scene vs Additive

**Decision (S98):** Use **1 scene per Act** with **Room GameObjects activated/deactivated** per transition.

Rationale:
- Cheaper than scene Additive load (no IO during transition, no async wait)
- Preserves global lighting / post-process volume / audio mixers
- Each Room is a GameObject prefab instance with its own Tilemap + props + spawn points
- Memory ok: 6-8 rooms × ~5MB tilemap = ~30-40 MB scene, far below 2 GB

**Hierarchy:**
```
Scene: Act1_ShatteredKeep.unity
├── Grid (top-level)
│   └── Tilemap_Global_Floor (rarely used — per-room override)
├── Rooms (parent)
│   ├── Room_01_EntryHall (active by default)
│   │   ├── Grid
│   │   │   ├── L1_Floor (Tilemap, granite material zones)
│   │   │   ├── L2_Walls (Tilemap)
│   │   │   └── Props_Root (children: arches, columns)
│   │   ├── Doors (4 DoorTrigger children at N/S/E/W positions)
│   │   ├── Lighting (URP 2D Lights per room mood)
│   │   ├── SpawnPoints (per direction entry, enemy spawn zones)
│   │   └── RoomMeta (RoomInstance.cs — runtime state)
│   ├── Room_02_WestChamber (inactive initially)
│   ├── Room_03_EastCorridor (inactive)
│   ├── Room_04_TreasureVault (inactive)
│   ├── Room_05_NorthAntechamber (inactive)
│   └── Room_06_ShatteredThrone (inactive)
├── Player (Warblade prefab, position set per transition)
├── Main Camera (single, with CameraFollow + bounds swap)
├── Global Light 2D (warm ambient)
├── PostProcess Volume (vignette, color grading)
└── Managers (RuntimeRoomManager, CheckpointSystem, AudioManager)
```

**Multi-act scaling:** Each Act = own scene (Act1_ShatteredKeep, Act2_..., Act3_..., Act4_Final).

---

## 5. Code Architecture (Phase H)

### Data layer (ScriptableObject)
```
Assets/Scripts/Map/Data/
├── RoomManifestSO.cs       — single room descriptor (id, dims, theme, connections, layout_json_path)
├── MapManifestSO.cs        — act-level manifest (ordered rooms, door graph, checkpoints)
└── RoomLayoutJson.cs       — DTO matching JSON schema (deserialized at load)
```

### Runtime layer
```
Assets/Scripts/Map/Runtime/
├── RuntimeRoomManager.cs   — singleton; tracks active room, handles transitions (RENAME from LegacyRuntimeRoomManager)
├── RoomLoader.cs           — parses RoomLayoutJson → Tilemap fill + prop instantiate
├── RoomInstance.cs         — per-room runtime state (mobs alive, doors locked, decals)
└── CheckpointSystem.cs     — save/load JSON checkpoint per door transition
```

### Editor layer
```
Assets/Scripts/Editor/Map/
├── RoomLayoutValidator.cs  — schema validation; missing refs warn
├── RoomLoaderMenu.cs       — menu items: "RIMA ▸ Map ▸ Load Room JSON to Scene"
└── MapManifestInspector.cs — custom inspector for MapManifestSO (graph viz)
```

### Existing (keep, adapt)
```
Assets/Scripts/Core/
├── DoorTrigger.cs          — KEEP (top-down compatible)
├── RoomTransitionFX.cs     — KEEP, polish in Phase J
└── RoomType.cs             — KEEP (enum of room types)

Assets/Scripts/Map/
├── RoomBuilder.cs          — KEEP (Hades-style 32×24 template, useful as fallback / quick test)
└── ObstaclePrefabBuilder.cs — KEEP

Assets/Scripts/MapDesigner/
├── (full audit Phase G — most projection-agnostic, some iso-era)
```

---

## 6. JSON-driven Layout — Why and What

### Why JSON?
- **User-editable** — no Unity Editor required to edit layout (text editor enough)
- **Versionable** — git diff readable, merge-friendly
- **Tool-agnostic** — generate from external tools (Tiled, custom editor, AI)
- **Validation** — schema enforces invariants (door count, material existence, etc.)
- **Procgen-ready** — generate layouts from algorithm and serialize
- **Test-friendly** — unit test on JSON without Unity playmode

### What goes in JSON?
- Grid dimensions
- Floor material zones (per-tile material name → loader picks variant from MaterialVariantPool)
- Wall positions + types (straight / corner / arch / hero)
- Prop placements (prefab name + grid position + rotation + scale variance)
- Mob spawn points (mob type + position + wave grouping)
- Door positions (4 standard + extra side doors)
- Lighting (global tint, per-light overrides)
- Music track + ambient SFX track
- Camera bounds rect

### What stays in Unity assets?
- Tile sprites (material variants — PixelLab outputs)
- Prop prefabs (wall/decoration prefabs from Phase B-C — deferred)
- Mob prefabs
- Material→Tile lookup (ScriptableObject MaterialVariantPool)
- Audio clips
- VFX prefabs

JSON references everything by **string ID** — loader resolves ID to asset via lookup tables.

---

## 7. MaterialVariantPool (Phase D — adapt to modular)

Was originally Phase D infrastructure for Wang chain. Re-purpose for modular tile pipeline:

```csharp
[CreateAssetMenu]
public class MaterialVariantPool : ScriptableObject {
    [System.Serializable]
    public class MaterialEntry {
        public string materialId;  // "granite", "rubble", "walkway", "rift"
        public TileBase[] variants; // tile_1-4 for granite, etc.
    }
    public MaterialEntry[] materials;

    public TileBase RandomVariant(string materialId, int seed) {
        var entry = materials.First(m => m.materialId == materialId);
        return entry.variants[seed % entry.variants.Length];
    }
}
```

JSON references material by id (`"granite"`); loader picks random variant per cell using deterministic seed (room id + cell coords).

---

## 8. Existing Asset Inventory (Sept S98 perspective)

| Path | Type | Status (top-down pivot) |
|---|---|---|
| `Assets/Data/Rooms/Library/Spawn_01.asset` | RoomRecipe SO | LIVE — adapt to RoomManifestSO Phase H |
| `Assets/Data/Rooms/Library/Combat_Small_01.asset` | RoomRecipe SO | LIVE — adapt |
| `Assets/Data/Rooms/Library/Combat_Medium_01.asset` | RoomRecipe SO | LIVE — adapt |
| `Assets/Data/Rooms/Library/Combat_Large_01.asset` | RoomRecipe SO | LIVE — adapt |
| `Assets/Data/Rooms/Library/Corridor_Linear_01.asset` | RoomRecipe SO | LIVE — adapt |
| `Assets/Data/Rooms/Library/Corridor_LShape_01.asset` | RoomRecipe SO | LIVE — adapt |
| `Assets/Data/Rooms/Library/Treasure_01.asset` | RoomRecipe SO | LIVE — adapt |
| `Assets/Data/Rooms/Library/Shrine_01.asset` | RoomRecipe SO | LIVE — adapt |
| `Assets/Data/Rooms/Library/Elite_01.asset` | RoomRecipe SO | LIVE — adapt |
| `Assets/Data/Rooms/Library/Boss_Intro_01.asset` | RoomRecipe SO | LIVE — adapt |
| `Assets/Data/RoomRecipe_ShatteredKeep_Combat_01.asset` | RoomRecipe | LIVE — Phase I reference |
| `Assets/Data/RoomRecipe_ShatteredKeep_Corridor_01.asset` | RoomRecipe | LIVE — Phase I reference |
| `Assets/Data/Blueprint/Rooms/combat_room_v15b.asset` | Blueprint | NEEDS_REVIEW — may be iso-era |
| `Assets/Resources/Map/DepthBands/DepthBandTileSet_F*.asset` | TileSet | NEEDS_REVIEW — iso depth bands, maybe re-purpose to top-down material zones |

**Phase G action:** Map each asset to LIVE / NEEDS_ADAPT / ARCHIVE.

---

## 9. Open Questions (for user decision)

1. **Room reuse vs Act-specific?**
   - Library `Combat_Small_01` reusable across all 4 Acts?
   - Or Act-specific `RoomRecipe_ShatteredKeep_*`? (Currently 2 Act 1 examples exist)
   - **Recommendation:** Library = abstract types, Act-specific = themed wrapper. Library template + Act theme override.

2. **Multi-floor support?**
   - Multiple Y levels per Act (Act 1 underground crypts beneath Shattered Keep)?
   - Or each Act = single floor?
   - **Recommendation:** Each Act = 1 floor for vertical slice. Multi-floor = post-Act 1.

3. **Procgen vs hand-authored?**
   - Hand-authored 6-room Act 1 vertical slice first (Phase I)?
   - Or procgen-from-template per run (roguelite — different layout each run)?
   - **Recommendation:** Hand-authored Act 1 first (vertical slice ship). Procgen Act 2+ if model validates.

4. **Save state granularity?**
   - Per-room checkpoint (current plan)?
   - Or per-Act checkpoint?
   - **Recommendation:** Per-room checkpoint — roguelite death = back to Act start, but mid-run quit = restore current room.

---

## 10. Implementation Order (Phase G-J detail)

### Phase G — MapDesigner audit (Codex + rima-sonnet, ~1h)
1. rima-sonnet: read all `Assets/Scripts/MapDesigner/**/*.cs` headers → classify each LIVE/NEEDS_ADAPT/ARCHIVE
2. Output: `STAGING/mapdesigner_audit_report.md`
3. Codex: `git mv` ARCHIVE list → `_Archive_iso_pre_topdown/`
4. Codex: compile + console verify

### Phase H — JSON loader (Codex, ~1.5h)
1. Codex: create Data layer (RoomManifestSO, MapManifestSO, RoomLayoutJson DTO)
2. Codex: create Runtime layer (RuntimeRoomManager, RoomLoader, RoomInstance)
3. Codex: create Editor layer (RoomLayoutValidator, RoomLoaderMenu)
4. Codex: write 1 test JSON (simple 16×12 spawn room) + load test in TopDownTest_Map1
5. Opus QC: review code + run loader test

### Phase I — Act 1 vertical slice (Orchestrator + Codex, ~2h)
1. Orchestrator: write `STAGING/act1_shattered_keep_layout_v1.json` (6 rooms)
2. Codex: create `Assets/Data/Map/Act1_ShatteredKeep/json/*.json` (6 files)
3. Codex: create 6 RoomManifestSO + 1 MapManifestSO
4. Codex: create `Act1_ShatteredKeep.unity` scene with Room GameObject hierarchy
5. Codex: RoomLoader run per Room → paint tilemap
6. Codex: door wiring per MapManifest graph

### Phase J — Door polish (Codex, ~1h)
1. Codex: fade transition (0.3s out + 0.3s in, audio duck)
2. Codex: mid-fight door lock (DoorTrigger.SetActive(false) on combat start)
3. Codex: CheckpointSystem.cs JSON save/load
4. Opus QC: walk-through full Act 1 in play mode

---

## 11. Future Hooks (not Phase G-J scope, design only)

- **Map streaming for very large megarooms** — split megaroom into sub-chunks, load adjacent only
- **Procgen layer** — algorithm generates JSON; same loader consumes
- **Editor visualization** — graph view of MapManifest connections in custom inspector
- **Live tile swap** — story progression changes tile materials (e.g., "Day 1 → Day 10" theme drift)
- **Multi-player join** — second player spawns at active room's spawn_point, transitions sync
- **Replay system** — record door transitions + combat for highlight reels
