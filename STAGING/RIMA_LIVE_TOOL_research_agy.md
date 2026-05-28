# agy Research Task — RIMA Live Tool Architecture + Asset Layer Architecture

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"

**RESPOND INLINE in your final message. DO NOT write any files. The dispatcher captures only inline stdout output.**

## Role
External research analyst for RIMA Unity ARPG. The orchestrator (Opus) needs you for **deep research** on live authoring tool patterns + asset layer architecture. No file writes, no codebase mutation. Pure inline synthesis.

## Context (one paragraph)
RIMA is a top-down 2D ARPG in Unity URP 2D. Visual style: HIGH TOP-DOWN 3/4 (Hades / Children of Morta / Diablo III). Locked V1 visual direction: floating-arena Hades Elysium hybrid (wall-less, cliff edges + sparse columns + cyan rune circles + warm brazier light). Pixel-art 3/4 sprites at 120x120 (chars) / 32x32 (tiles) / PPU 64. Current authoring is inside Unity Editor: a `RimaRoomPainterWindow` 3-pane window (palette / preview / inspector) — Day 5a LIVE. User now wants to evaluate moving toward a **Sang Hendrix Realtime Parallax Map Builder** style live authoring loop where the game runs in one window and the tool runs in another, with live hot-reload of edits.

## Three questions

### Q1 — Sang Hendrix Realtime Parallax Map Builder deep analysis
Reference: https://sanghendrix.itch.io/realtime-parallax-map-builder-rpg-maker-mz-plugin
Reference videos (Codex already inspected):
- Trailer: https://www.youtube.com/watch?v=IkrQIKoLw40
- Drag-n-drop: https://www.youtube.com/watch?v=erXuNhARF-8
- Grid-free painting: https://www.youtube.com/watch?v=zFzx7AkOJhs
- Tile-free tutorial: https://www.youtube.com/watch?v=dpjpdsiQNHQ
- Long how-to: https://www.youtube.com/watch?v=Q4redsN-dO8

Please answer in detail:
1. **Underlying pattern** — Is Sang's tool an in-game editor (game embeds the editor), an external tool + game with IPC, or something else? File watcher vs socket vs in-process API?
2. **Hot-reload mechanism** — When you drag a prop in the tool, what triggers the live update in the running MZ game? RPG Maker MZ runs JS in NW.js — does Sang inject a JS module, watch a JSON file, or override a class?
3. **Authoring loop UX** — What specific UX moves make the tool feel "live"? (drag/drop, freeform place, factor.x/y per layer, occlusion zones, etc.)
4. **What RPG Maker MZ specific tricks don't transfer to Unity** — RPG Maker has a JS runtime; Unity has C# + Domain Reload. Where's the mismatch?

### Q2 — Unity-side equivalents for live authoring
Compare these patterns for RIMA's use case (single-developer indie, no shipping mods yet, but mod-friendly architecture would be a plus):

| Pattern | How it works | Strengths | Weaknesses | RIMA fit |
|---|---|---|---|---|
| **Editor extension + Play Mode** (current) | Window inside Unity Editor; Play Mode runs game in same process | Zero infra; full Editor APIs | Domain Reload kills velocity; tool only works with Editor open | ? |
| **Editor extension + Standalone Player Build bridge** | Tool stays in Editor; game runs as separate `.exe`; file watcher / named pipe / OSC syncs edits | Run game fullscreen separate; closer to ship build | Need serialization layer (JSON room data); reload mechanism per asset type | ? |
| **Standalone tool (Unity-built `.exe`) + standalone game `.exe`** | Both are Unity builds; tool reads/writes JSON; game watches file | Closest to Sang's pattern; no Editor dependency at runtime | Tool loses Editor goodies (Selection API, Handles, etc.) | ? |
| **In-game editor (toggle via F12)** | Single build; runtime mode switch between Play and Edit | Roblox Studio / Trackmania pattern; ship-with mod support | UX has to be built from scratch in IMGUI/UI Toolkit Runtime; bigger scope | ? |
| **Hot Reload (Singularity Group) / FastScriptReload** | Avoids Domain Reload by patching IL at runtime | Cheap velocity boost; works with existing Editor flow | Doesn't solve "game fullscreen separate window"; paid (HR) or limited (FSR) | ? |

For each, give: **(a)** technical mechanism, **(b)** dev velocity impact, **(c)** RIMA-specific risks (single-dev, Unity 6.x URP 2D, no Addressables installed yet, runtime room loading via `RoomManifestSO + TextAsset jsonLayout` already exists), **(d)** estimated implementation scope in days.

### Q3 — Asset layer architecture in indie 2D top-down ARPGs
Study the layer/sorting conventions in: **Hades** (Supergiant), **Children of Morta** (Dead Mage), **Hyper Light Drifter** (Heart Machine), **Death's Door** (Acid Nerve). For each, describe:
1. **Floor layer(s)** — how many tile layers? Decals as separate layer or merged?
2. **Cliff / boundary layer** — visual-only sprite layer or actual collider?
3. **Walkable decor layer** — bones, rune circles, vines: are these props with `colliderType=None`, or stamped into tiles?
4. **Wall / blocker layer** — pillars, columns, broken statues with colliders: what shape (Box / Circle / Capsule / Polygon) is conventional?
5. **Gameplay object layer** — chests, NPCs, interactables: trigger colliders or solid colliders?
6. **Sorting order convention** — what numeric ranges do they use (or what does the layering look like visually)?

Then propose a **5-layer or 6-layer architecture** for RIMA's wall-less Hades Elysium V1, accounting for:
- RIMA already has these prefab folders: `Assets/Prefabs/Environment/Walls/AssetPackV3/` (8 wall prefabs + Placeholders), `Assets/Prefabs/Obstacles/` (Chasm, NarrowPassage, StoneColumn), `Assets/Prefabs/Props/ShatteredKeep_PixelLab/` (mounting_*.prefab x15, statue_*.prefab x14 — these are the "cliff face decor" the user is asking about)
- RIMA has tile-based floor + cliff (`CliffAutoPlacer` + `DirectionalCliffTile_Hades.asset` 8-direction variant TileBase)
- RIMA Phase 1 demo locked: only Warblade + 5 rooms + 4 mobs + Map Fragment + Gate
- User's verbatim 4-layer suggestion: Cliff (alt dekor) / Walkable decor (yer, NO RB) / Wall decor (cave/pillar/küçük duvar, RB boyutu kadar) / Gameplay object (chest/interactable)
- User asked: 5-layer alternative where Cliff splits into "Cliff base" + "Cliff face/mount decor" (mounting_*.png pattern)

Recommend: 4 / 5 / 6 / 7 layer split with concrete justification. Include **sorting order numeric range** suggestion per layer.

### Q4 (bonus) — Indie game live editing references beyond Sang
Briefly (1-2 sentences each):
- **Hades** modding (SGGModUtil) — what's the authoring pattern?
- **Stardew Valley Content Patcher** — JSON-driven content reload, runtime or restart?
- **Tilesetter Pro** — standalone tilemap tool, Unity export workflow?
- **LDtk** — standalone editor + Unity `LDtkToLevelManager` package — true live reload or restart?
- **Tiled Map Editor** + Unity Super Tiled2Unity — same question

Which of these models is closest to what Sang does, and which is the best technical match for RIMA?

## Output format
Plain markdown, ~800-1200 words total. Section per Q. Be concrete (file names, packages, version numbers where relevant). End with a **3-bullet "Opus should know" summary** of the most critical findings for the synthesis document.

## Constraints
- Respond inline, NO file writes
- 5 minutes of focused research, no rabbit holes
- If a question cannot be answered confidently, mark it `[low confidence]` and continue — don't bail
