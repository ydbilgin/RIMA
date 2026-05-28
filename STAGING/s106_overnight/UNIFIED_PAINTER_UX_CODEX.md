# Codex Task — Unified Map Designer (Tab-Based UI/UX Consolidation)

ACTIVE RULES: (1) think before coding (2) min code, NO speculation (3) surgical — single new window, reuse existing code (4) BLOCKED if either source painter unreadable.

# AMAÇ
User reported 2 painter windows open at once causing visual overlap + bad UX:
1. `Assets/Scripts/Editor/Walls/V2/RoomPainterWindow.cs` (World Painter — abstract sockets: Walkable/Door/Alcove/PropSocket/EnemySpawn + preset rooms + Generate Room)
2. `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs` (Tile Painter — 4 themes + paint/erase/brush/undo)

Goal: Consolidate into ONE Editor window with TAB navigation. Polished UI/UX. Shared top toolbar.

**Constraint:** REUSE existing code as much as possible. Don't rewrite. Just wrap in unified shell.

# DESIGN SPEC

## Window structure
```
RIMA > Map Designer  (single Editor window)
├── Top toolbar
│   ├── Active Tilemap: [ObjectField]   (shared, used by both tabs)
│   ├── Active Scene: [ScenePath label]
│   └── [Refresh assets]
│
├── Tab bar
│   ├── [ Tile Painter ]        ← MinimalTilePainter content
│   ├── [ Room Builder ]        ← V2 RoomPainterWindow content
│   └── (future: [ Props ], [ Reference Layers ])
│
└── Tab content (scrollable, fills window)
```

## Implementation strategy
1. New EditorWindow: `Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`
2. Namespace: `RIMA.Editor.MapDesigner`
3. Menu: `RIMA > Map Designer` (primary access)
4. Old menus DEPRECATED but not removed:
   - `RIMA > Tile Painter (Minimal)` → still works, opens MinimalTilePainter as before (compat)
   - `RIMA > V2 > World Painter` → still works (compat)
5. UnifiedMapDesigner uses tabs to host the SAME UI as existing windows. Refactor the existing painters to expose their UI as static methods OR instantiate inline GUI sections.

### Code reuse pattern (recommended)
- Extract MinimalTilePainter's `OnGUI` body into static method `MinimalTilePainter.DrawUI(SerializedObject state)`
- Same for V2 RoomPainterWindow → `RoomPainterWindow.DrawUI(SerializedObject state)`
- UnifiedMapDesigner just calls the appropriate `DrawUI` based on selected tab

OR if extraction is complex (too coupled):
- UnifiedMapDesigner duplicates the GUI calls but reuses underlying logic (state classes, paint methods)

Pick simpler path. Don't over-engineer.

## UI polish requirements
1. **No overflow** — Min window size 400×700, scrollable content
2. **Visual hierarchy** — Toolbar dark, tabs medium, content area light
3. **Selected tab highlight** — clear visual feedback (cyan accent matching RIMA palette)
4. **Tooltips** — short hints on buttons (e.g., "Paint single tile" on Paint button)
5. **Status bar at bottom** — "Active Tilemap: X | Last action: paint 3×3 at (5, 2)" type info
6. **Keyboard shortcuts displayed** — small text under tools: "P=Paint, E=Erase, 1/2/3=Brush Size"

## Phase 0 — Verify (5 min)
- `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs` exists, readable (~218 lines, just built)
- `Assets/Scripts/Editor/Walls/V2/RoomPainterWindow.cs` exists, readable
- Read their OnGUI structures to understand refactoring scope

## Phase 1 — Refactor (30-45 min)
If extraction approach: extract `OnGUI` bodies to static `DrawUI(state)` methods
If duplicate approach: identify minimum surface to call from unified window

## Phase 2 — Create UnifiedMapDesigner.cs (30-45 min)
Single Editor window with:
- Top toolbar (shared Active Tilemap)
- Tab switcher (2 tabs initial)
- Tab content area (calls appropriate DrawUI)
- Status bar
- Window dimensions enforced (min 400×700)

## Phase 3 — Test (15 min)
1. Open `Assets/Scenes/Test/PlayableArena.unity`
2. `RIMA > Map Designer` → window opens at appropriate size, no overflow
3. Click Tile Painter tab → MinimalTilePainter UI shows, paint works
4. Click Room Builder tab → V2 RoomPainterWindow UI shows, preset works
5. Tab switch preserves state (Active Tilemap stays)
6. Screenshot: `STAGING/s106_overnight/unified_map_designer_v1.png` (1280×720)
7. Verify old menus still work as compat

## Phase 4 — Report
```
# UNIFIED MAP DESIGNER - <profile> - <time>

## STATUS: DONE | PARTIAL | FAILED

## Phase 1 — Refactor approach
- Extraction (static DrawUI) OR duplication (inline GUI)
- Why chosen

## Phase 2 — Window file
- Path: Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs
- Line count: N
- Menu: RIMA > Map Designer
- Tabs: Tile Painter, Room Builder
- Shared: Active Tilemap, status bar

## Phase 3 — Test
- Single window, no overlap: y/n
- Tab switch works: y/n
- Tile Painter paint test: y/n
- Room Builder preset test: y/n
- Old menus compat: y/n
- Screenshot: y/n
- Compile: 0 errors, 0 warnings

## Time: N min
```

# Constraints
- ❌ NO new painting features (consolidation only, not feature add)
- ❌ NO refactor of existing painter LOGIC (just UI shell)
- ❌ Don't break existing `RIMA > Tile Painter (Minimal)` or `RIMA > V2 > World Painter` (compat)
- ✅ Single new file, reuse existing code
- ✅ Tab UX with visual highlight
- ✅ Test screenshot proof

# Estimated total: 90-120 min
