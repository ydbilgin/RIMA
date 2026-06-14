# Codex Research Task — RIMA Live Tool + Asset Layer Codebase Reality Check

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

## Role
Codebase reality-check analyst. The orchestrator (Opus) is synthesizing a 3-decision plan with input from agy (external research) and YOU (codebase facts). DO NOT WRITE CODE. Only inventory + LOC estimates + feasibility notes.

## Output
Write your full report to `STAGING/RIMA_LIVE_TOOL_codex_appendix.md`. Use markdown. Cite **exact file paths and line numbers** for every claim.

## Three research areas

### A — Asset & Prefab Inventory (RIMA codebase)

Goal: enumerate the assets the new asset-layer architecture must accommodate. Be exhaustive but terse.

1. **Floor tile assets**
   - List all `TileBase` ScriptableObjects under `Assets/ScriptableObjects/Environment/` (count + names)
   - List PNG sprites used as floor tiles under `Assets/Sprites/Environment/PixelLab_Selected_Assets/`, `Assets/Sprites/Environment/KitB_Cliff/`, `Assets/Sprites/Environment/KitC_BG/`, `Assets/Sprites/Environment/RIMA_AssetParts_v2/` — categorize floor vs cliff vs decor by filename keyword

2. **Cliff assets**
   - `DirectionalCliffTile_Hades.asset` — read the .asset YAML and confirm: how many sprite arrays are populated (8 directions × variant count)?
   - `CliffAutoPlacer.cs` — confirm: is `cliffTile` a single `TileBase` field or `List<TileBase>`? Cite line number.
   - List PNG sprites under `Assets/Sprites/Environment/KitB_Cliff/` (cliff face / mounting / cap variants)

3. **Walkable decor prefabs**
   - Glob `Assets/Prefabs/Environment/Decorations/**/*.prefab` — list all, categorize by name (bone, vine, rune, plinth, decal)
   - For each, grep the prefab YAML for `Collider2D` presence (yes/no) — this tells us which already have colliders vs not

4. **Wall blocker prefabs**
   - List `Assets/Prefabs/Environment/Walls/AssetPackV3/*.prefab` (already inventoried at orchestrator side: 8 + ~12 placeholders)
   - For each, grep for `Collider2D` type (Box / Capsule / Circle / Polygon) and `isTrigger` flag
   - List `Assets/Prefabs/Obstacles/*.prefab` (Chasm, NarrowPassage, StoneColumn) — same collider audit

5. **Cliff-face decor / mounting prefabs**
   - List `Assets/Prefabs/Props/ShatteredKeep_PixelLab/mounting_*.prefab` (~15 expected) — Collider2D audit
   - List `Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_*.prefab` (~14 expected) — Collider2D audit
   - These are the assets the USER is calling "wall decor" / "cliff mount decor" — confirm their current pivot, sortingOrder, and bounding box size

6. **Gameplay object prefabs**
   - `Assets/Prefabs/Chest.prefab`, `Assets/Prefabs/MapFragment.prefab`, `Assets/Prefabs/RewardPickup.prefab` — collider type + isTrigger
   - `Assets/Prefabs/Environment/MapFragment.prefab`, `PlayerStartMarker.prefab` — same
   - Gate-related prefabs (grep `Gate` in `Assets/Prefabs/` and `Assets/Scripts/Environment/Gate.cs` references)

**Output as table:**

| Layer (candidate) | Asset path glob | Count | Has collider? | Collider shape | sortingOrder current |
|---|---|---|---|---|---|

### B — RoomPainter Pipeline Audit

1. **`RoomPainterPhysicsRules.cs`** (already opened — 30+ keywords known). Confirm:
   - Are there keyword GAPS for cliff-face decor (mounting / statue / pedestal)? Currently `cliff` keyword applies to ALL cliff assets — does it differentiate face decor from base?
   - Add proposal: which new keywords does the 5-layer architecture need?
2. **`RoomPainterAssetPostprocessor.cs`** — find this file (likely under `Assets/Editor/RoomPainter/AssetPipeline/`). Summarize what it does at asset import time (in 5 bullets).
3. **`RoomPainterColliderEditor.cs`** (Day 5b drag-handle) — already opened, confirm:
   - Does it operate on Scene instances only, or can it write back to the prefab asset?
   - Does it support PolygonCollider2D editing?
   - LOC count of current file
4. **Per-prefab collider authoring path**
   - Best Unity-native API for prefab modification: `PrefabUtility.SavePrefabAsset` vs `PrefabUtility.ApplyPrefabInstance` vs direct asset edit
   - Recommendation: which is the right call for "user drags handle on prefab in painter → all scene instances pick up new collider"?
5. **Visual category filtering**
   - `Assets/Editor/RoomPainter/RimaRoomPainterWindow.cs` — find the palette category enum / filter mechanism. List existing categories.

### C — Live Tool Feasibility

1. **Runtime room load infrastructure**
   - `Assets/Scripts/Map/Data/RoomManifestSO.cs` (already opened — has `TextAsset jsonLayout`)
   - `Assets/Scripts/Core/RuntimeRoomManager.cs` — find the json load + scene populate code path. Cite line range.
   - `Assets/Scripts/Map/Data/MapManifestSO.cs` — read full + summarize
   - **Question:** Can the existing JSON pipeline reload a room WHILE the game is playing (i.e., not requiring a scene reload)? If not, what's the smallest change?

2. **Hot-reload mechanism options for RIMA**
   - `com.unity.addressables` is **NOT installed** (confirmed in `Packages/manifest.json`). Would installing it for live-asset hot-reload be a net win or net cost?
   - `AssetBundle` modules are present. Is AssetBundle hot-swap viable for prefab live reload?
   - `Resources.Load` + `FileSystemWatcher` — does Unity support this pattern for runtime asset replacement?
   - JSON file watch (most lightweight option) — what part of `RuntimeRoomManager` would be the hook?

3. **External Player Build bridge (Tier 2 architecture)**
   - **Named Pipe** (`System.IO.Pipes.NamedPipeServerStream`) — Editor side ↔ Player side IPC. Feasibility?
   - **File watcher** (Editor writes `STAGING/live/room_current.json`, Player runs `FileSystemWatcher` on it) — feasibility?
   - **Loopback TCP / OSC** — alternative
   - For each, give: (a) latency expectation, (b) implementation LOC, (c) failure modes

4. **Standalone Tool feasibility (Tier 3 architecture)**
   - Unity License rules: can a Unity-built `.exe` use Editor APIs (Handles, SceneView, AssetDatabase)? **Answer: NO** (Editor-only assemblies don't ship). But what's the workaround?
   - UI Toolkit Runtime vs IMGUI Runtime for the tool's UI
   - How to share asset references between standalone tool and game? (AssetBundle catalog, JSON GUID list, or a slimmed runtime asset registry?)

5. **DirectionalCliffTile activation status**
   - `DirectionalCliffTile.cs` — confirm the 8 direction sprite arrays are present (`spritesS/SE/SW/E/W/N/NE/NW`)
   - Cite the line range that maps neighbor cell → direction
   - `DirectionalCliffTile_Hades.asset` — read YAML, how many of the 8 arrays have at least one sprite assigned?
   - Question: if user wires the existing mounting_*.png as cliff face decor variant, does the existing `DirectionalCliffTile` infrastructure consume that automatically, or does it need code changes?

### D — LOC Estimates

For each of the following work items, give: **(a)** LOC estimate, **(b)** affected files, **(c)** risk level (L/M/H).

| Work item | LOC | Files | Risk |
|---|---|---|---|
| Asset layer migration: add `RoomLayer` enum to existing prefab metadata; backfill 244+ existing prefabs | ? | ? | ? |
| Per-prefab collider drag-handle (extend `RoomPainterColliderEditor.cs` to write to prefab asset, not just scene instance) | ? | ? | ? |
| Sorting layer registration: 5-6 new sorting layers in ProjectSettings | ? | ? | ? |
| Tier 2 live tool: file-watcher JSON room reload while game in Play Mode (Editor extension stays in Editor, Player Build separate window) | ? | ? | ? |
| Tier 3 standalone tool: complete refactor from Editor extension to runtime UI Toolkit window | ? | ? | ? |
| DirectionalCliffTile activation: wire mounting_*.png variants into 8-direction arrays | ? | ? | ? |

## Constraints
- DO NOT write code or modify any files except the appendix output
- DO NOT run UnityMCP write operations (manage_*, apply_text_edits, create_script)
- Read-only: Glob / Grep / Read / find_in_file are OK
- 30-45 min budget; if you hit limits, write what you have and STOP
- Output filename: `STAGING/RIMA_LIVE_TOOL_codex_appendix.md`

## Format
Plain markdown. Use tables liberally. Cite `file:line` everywhere. Conclude with a **5-bullet "Reality Check for Opus"** highlighting the biggest codebase facts that should shape the decision.
