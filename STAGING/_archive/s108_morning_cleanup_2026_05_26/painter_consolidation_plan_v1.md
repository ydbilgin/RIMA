# Painter Consolidation + Map Designer Integration v1

> **Goal:** Pick **ONE** primary painter, archive the rest. Add JSON map loader hook for Phase H.
> **Decision:** `RimaWorldPainterWindow.cs` = LIVE primary. Others → `_Archive_painter_alt/`.
> **Status:** Codex task ready (Phase F scope).

---

## 1. Painter inventory (current state)

| File | Lines | Status | Notes |
|---|---|---|---|
| `Assets/Editor/RimaWorldPainterWindow.cs` | **4563** | ✅ **LIVE PRIMARY** | PaintMode top-down/iso (top-down default), 4 category Floor/Wall/Prop/Mob, ScanResult system |
| `Assets/Editor/Act1RoomPainter.cs` | 96 | → ARCHIVE | Old Act 1 specific painter, likely superseded |
| `Assets/Editor/Act1RoomPainterEnhanced.cs` | 340 | → ARCHIVE | Enhanced variant, also superseded |
| `Assets/Editor/DevTools/DemoRoomPainter.cs` | 87 | → ARCHIVE | Demo-specific, dev tool |
| `Assets/Editor/DevTools/PilotRoomPainter.cs` | 60 | → ARCHIVE | Pilot test, exploratory |
| `Assets/Editor/DevTools/SceneFloorPainter.cs` | 53 | → ARCHIVE | Floor-only utility, superseded |
| `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs` | 715 | → REVIEW (Phase G) | Possibly Map Designer integration related — full audit needed before archive |
| `Assets/Editor/_archive_S73/RoomDesigner/DecalPainter.cs` | — | Already archived | |
| `Assets/Editor/_archive_S73/RoomDesigner/FloorVariantPainter.cs` | — | Already archived | |

**Total alternative painters to archive:** 5 (sum: 636 lines code), plus BlueprintPainterWindow flagged for Phase G review.

---

## 2. Why RimaWorldPainterWindow as PRIMARY

- **Biggest, most evolved** (4563 lines vs others 53-715)
- Supports **both projection modes** (PaintMode.TopDown + PaintMode.Isometric) — top-down default, iso fallback for legacy
- **4-category palette** (Floor/Wall/Prop/Mob) matches RIMA top-down composition needs
- **CollisionMode enum** (Auto/Passable/SmallFootprint/FullFootprint/WallBlock/Custom) handles top-down wall placement
- ScanResult system + RimaBiomePreset support → biome-aware brush
- Already integrated with `RIMA.Systems.Map` and `RIMA.Editor` namespaces

---

## 3. Archive plan (Phase F scope)

Codex task — mechanical move + import test:

### Step 1 — Verify no live references
```
Grep: "Act1RoomPainter|Act1RoomPainterEnhanced|DemoRoomPainter|PilotRoomPainter|SceneFloorPainter"
in Assets/ excluding the files themselves
```
If any live reference (`[MenuItem]` shared name, ScriptableObject reference, etc.) → BLOCK, do not archive without resolving.

### Step 2 — git mv to archive folder
```
mkdir Assets/Editor/_Archive_painter_alt
git mv Assets/Editor/Act1RoomPainter.cs                 Assets/Editor/_Archive_painter_alt/
git mv Assets/Editor/Act1RoomPainter.cs.meta            Assets/Editor/_Archive_painter_alt/
git mv Assets/Editor/Act1RoomPainterEnhanced.cs         Assets/Editor/_Archive_painter_alt/
git mv Assets/Editor/Act1RoomPainterEnhanced.cs.meta    Assets/Editor/_Archive_painter_alt/
git mv Assets/Editor/DevTools/DemoRoomPainter.cs        Assets/Editor/_Archive_painter_alt/
git mv Assets/Editor/DevTools/DemoRoomPainter.cs.meta   Assets/Editor/_Archive_painter_alt/
git mv Assets/Editor/DevTools/PilotRoomPainter.cs       Assets/Editor/_Archive_painter_alt/
git mv Assets/Editor/DevTools/PilotRoomPainter.cs.meta  Assets/Editor/_Archive_painter_alt/
git mv Assets/Editor/DevTools/SceneFloorPainter.cs      Assets/Editor/_Archive_painter_alt/
git mv Assets/Editor/DevTools/SceneFloorPainter.cs.meta Assets/Editor/_Archive_painter_alt/
```

### Step 3 — Wrap archive files with `#if false`
Each archived `.cs` file: prepend `#if false` after `using` lines, append `#endif` at file end. Keeps them readable but excludes from compile.

### Step 4 — Unity import refresh + console verify
- `refresh_unity` if_dirty
- Wait for domain reload
- `read_console` — 0 errors required (no missing menu items, no broken references)

### Step 5 — BlueprintPainterWindow audit
- Open `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs`
- Read header + class signature
- Search for references in other files
- Report in verdict:
  - LIVE? (still used in current workflow)
  - SUPERSEDED? (RimaWorldPainterWindow covers same use)
  - SEPARATE PURPOSE? (orthogonal feature, keep)

---

## 4. RimaWorldPainterWindow + Map Designer Integration (Phase H scope)

User direction: "bu map designları da world painter içine ekleyebiliriz".

### Hook 1 — JSON layout load button

Add new toolbar section in RimaWorldPainterWindow:

```
[ Map Designer Section ]
  [ Load JSON ▾ ]
    - Browse... → file picker → select .json
    - Recent layouts → dropdown of recent loaded
    - Reload current
  [ Save Current → JSON ]
    - Exports current scene's Tilemap state to JSON layout (round-trip)
  [ Validate JSON ]
    - Schema check against map_schema_v1.json
```

Code add (Phase H — Codex task):

```csharp
// Inside RimaWorldPainterWindow.cs, new section in OnGUI()

private void DrawMapDesignerSection() {
    EditorGUILayout.LabelField("Map Designer", EditorStyles.boldLabel);
    EditorGUILayout.BeginHorizontal();
    if (GUILayout.Button("Load JSON...", GUILayout.Height(28))) {
        var path = EditorUtility.OpenFilePanel("Load Room Layout JSON", "Assets/Data/Map", "json");
        if (!string.IsNullOrEmpty(path)) {
            RoomLoader.LoadJsonToScene(path, targetTilemap, targetParent);
        }
    }
    if (GUILayout.Button("Save → JSON", GUILayout.Height(28))) {
        var path = EditorUtility.SaveFilePanel("Save Room Layout JSON", "Assets/Data/Map", "room", "json");
        if (!string.IsNullOrEmpty(path)) {
            RoomSerializer.SerializeSceneToJson(path, targetTilemap, targetParent);
        }
    }
    if (GUILayout.Button("Validate", GUILayout.Height(28))) {
        var path = EditorUtility.OpenFilePanel("Validate Room Layout JSON", "Assets/Data/Map", "json");
        if (!string.IsNullOrEmpty(path)) {
            RoomLayoutValidator.Validate(path);
        }
    }
    EditorGUILayout.EndHorizontal();
}
```

Dependencies (Phase H new files):
- `Assets/Scripts/Map/Runtime/RoomLoader.cs`
- `Assets/Scripts/Map/Editor/RoomSerializer.cs`
- `Assets/Scripts/Map/Editor/RoomLayoutValidator.cs`

### Hook 2 — RoomManifestSO browser

Add palette tab "Rooms" alongside Floor/Wall/Prop/Mob:

```
[ Palette Tabs ]
  Floor | Wall | Prop | Mob | Rooms     ← NEW
```

When "Rooms" tab active:
- Scan `Assets/Data/Map/<Act>/json/*.json` and `*.asset` RoomManifestSO
- Display each as preview (thumbnail of pre-rendered mini-map)
- Click → load to scene OR pin as template

### Hook 3 — Door connection visualizer

When MapManifestSO loaded:
- Draw door graph as gizmos in Scene view
- Each room shown as labeled rect
- Connection lines between rooms (N→S match, etc.)
- Edit mode: drag connection to re-wire

---

## 5. Map Designer (existing infrastructure mapping)

Existing related code (from Phase G inspection):

| File | Purpose | Phase H interaction |
|---|---|---|
| `Assets/Scripts/MapDesigner/ProceduralRoomGenerator.cs` | Procedural baseline | Produces base → loader populates from JSON if override |
| `Assets/Scripts/MapDesigner/Encounter/Data/EncounterTemplateSO.cs` | Encounter slot data | Referenced from RoomManifestSO |
| `Assets/Scripts/MapDesigner/Encounter/Data/SubRoomLink.cs` | Sub-room link | RoomManifest sub-encounter ref |
| `Assets/Scripts/Data/RoomRecipe.cs` | Older room recipe SO | Phase G: evaluate — adapt to RoomManifestSO or wrap |
| `Assets/Editor/MapDesigner/Blueprint/BlueprintPainterWindow.cs` | Blueprint painter | Phase G audit; if LIVE → keep alongside RimaWorldPainterWindow |

---

## 6. Painter consolidation summary

**Single painter philosophy:** One canonical brush tool for all map composition, all biomes, both projections.

**RimaWorldPainterWindow tabs (after Phase H integration):**

```
┌─ RimaWorldPainterWindow ─────────────────────────┐
│ ┌─ Toolbar ──────────────────────────────────┐   │
│ │ Brush: Paint | Erase | Eyedrop             │   │
│ │ Projection: TopDown | Iso                  │   │
│ │ Biome: Act1_ShatteredKeep ▾                │   │
│ └────────────────────────────────────────────┘   │
│ ┌─ Map Designer (NEW Phase H) ───────────────┐   │
│ │ [Load JSON...] [Save → JSON] [Validate]    │   │
│ │ Active manifest: Act1_ShatteredKeep ▾      │   │
│ └────────────────────────────────────────────┘   │
│ ┌─ Palette ──────────────────────────────────┐   │
│ │ Floor | Wall | Prop | Mob | Rooms          │   │
│ │ ┌─────────────────────────────────────┐    │   │
│ │ │ [filter] [scan path] [refresh]     │    │   │
│ │ │ ▒░░ thumbnail grid ░░░             │    │   │
│ │ └─────────────────────────────────────┘    │   │
│ └────────────────────────────────────────────┘   │
│ ┌─ Settings ─────────────────────────────────┐   │
│ │ Collision: Auto/Passable/Wall/...          │   │
│ │ Target Tilemap: L1_Floor ▾                 │   │
│ │ Target Parent: Room_01_EntryHall ▾         │   │
│ └────────────────────────────────────────────┘   │
└──────────────────────────────────────────────────┘
```

---

## 7. Implementation Order

| # | Task | Phase | Owner |
|---|---|---|---|
| 1 | Archive 5 alt painters + `#if false` wrap + console verify | F | Codex → Opus QC |
| 2 | BlueprintPainterWindow audit | G | rima-sonnet (analyze) → Codex (mechanical archive if SUPERSEDED) |
| 3 | RimaWorldPainterWindow scan path refactor (modular_v1 floor + wall prefab scan) | F | Codex → Opus QC |
| 4 | RoomLoader.cs + RoomSerializer.cs + RoomLayoutValidator.cs (Phase H new files) | H | Codex → Opus QC |
| 5 | RimaWorldPainterWindow `DrawMapDesignerSection()` add | H | Codex → Opus QC |
| 6 | "Rooms" palette tab add + RoomManifestSO browser | H | Codex |
| 7 | Door connection visualizer gizmos | H+ | Codex (polish, optional) |

---

## 8. Open questions

1. **BlueprintPainterWindow vs RimaWorldPainterWindow overlap:** Phase G audit will decide — for now LEAVE ALONE.
2. **Save→JSON round-trip:** Phase H V1 = read-only loader (JSON → scene). Save→JSON polish post-Phase K if user finds it useful.
3. **Pre-rendered mini-map thumbnail:** Phase H+ polish — for V1, just text label per room in "Rooms" tab.
4. **Multi-tilemap targeting:** Currently single `targetTilemap`. For 4-layer painting (Floor/Walls/Decals/Decoration), painter needs auto-switch based on PaletteCategory. Phase H scope.
