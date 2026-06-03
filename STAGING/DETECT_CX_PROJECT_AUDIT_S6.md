# TASK — RIMA project DETECTION audit (facts only) + re-pull the keeper floor tile

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed scope only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / asset files.

Profile: laurethayday. Effort: high. Language: English report.

## Amaç (Goal)
The user feels the RIMA project is "extremely cluttered even though we have almost nothing." This is a **DETECTION-ONLY** pass: produce a factual inventory of what exists, what is actually USED vs orphan/nonsense, so a second agent (ax/Gemini) can write a cleanup plan. **Do NOT delete, move, or archive anything** (except the one re-pull action in §1). **Do NOT fix Map Designer.** **Do NOT commit.** Facts only.

This is FILE-BASED — do NOT depend on Unity being open. Read `.unity` / `.prefab` / `.asset` as text, grep `.meta` GUIDs to determine references.

## §1 — Re-pull the KEEPER floor tile (the ONLY action you take)
The user wants to keep ONE PixelLab floor tile and re-pull a clean copy: tiles_pro id `451bbfd8-bb7c-4778-8643-caa95ffddf97`.
- Download (auth-less ZIP): `https://api.pixellab.ai/mcp/tiles-pro/451bbfd8-bb7c-4778-8643-caa95ffddf97/download` (use curl with `--dangerouslyDisableSandbox` if sandbox blocks it).
- Extract the 16 tiles into the existing keeper folder `Assets/Sprites/Environment/PixelLabFloor451/` (overwrite the existing `floor451_0-15` PNGs — they may be partial; backblaze dropped mid-session before).
- Report: success/fail, how many files written, sizes. Do NOT touch import settings or .meta (orchestrator/Unity handles re-import).

## §2 — FLOOR inventory (user: "we have a lot of floors")
List EVERY floor-related folder + asset under `Assets/Sprites/Environment/` and `Assets/.../Tiles` (e.g. PixelLabFloor, PixelLabFloorFlat, PixelLabFloor451, pl_floor, flat_tile, iso_floor, granite, any tileset). For each entry give a small table:
| folder/asset | #files | referenced? (which scene/prefab/registry uses its GUIDs) | source tileset id if known |
KEEPER = 451bbfd8 (`PixelLabFloor451`). Flag every OTHER floor set as **candidate-redundant** with the evidence (referenced or orphan). Check references by grepping each set's `.meta` GUIDs across `*.unity`, `*.prefab`, `*.asset` (esp. the RuntimeAssetRegistry / AssetPack assets).

## §3 — CHARACTERS + CLIFFS (user: "characters and cliffs we keep")
- Characters: inventory `Assets/Resources/Characters/**` and any `Assets/.../Art/Characters/**`. CURRENT_STATUS flags `Art/Characters/` as a duplicate that caused a white-box class-select bug — confirm: is `Art/Characters` referenced anywhere, or is `Resources/Characters` the live one? Report the duplicate split.
- Cliffs: inventory cliff sprite/tile sets (KitB_Cliff, DirectionalCliffTile assets, etc.). Referenced or orphan?

## §4 — ORPHAN / NONSENSE candidates
CURRENT_STATUS mentions these as set-aside/abandoned: `IsoMockKit`, `KitC_BG`, `Phase0_ScaleTest`, `RIMA_AssetParts_v2`, `ShatteredKeep_PixelLab`, `_archive_imagegen*`, imagegen experiment folders, mock-render outputs. For each that exists: #files + referenced-or-orphan (GUID grep). List any OTHER large/unreferenced asset folder you find. Facts only — KEEP/DELETE verdict is ax's job.

## §5 — SCENE census (user: "the Unity scene screen changed, there's nonsense in it")
- List all `.unity` files under `Assets/Scenes/**`.
- For the demo/playable scene(s) (e.g. `PlayableArena_Test01.unity`, any `DemoMap*`, `_IsoGame`, `DiamondRoom*`), parse the file as text and extract the ROOT GameObject names (the `m_Name:` of objects whose transform has no parent). Flag obvious junk: duplicated names, leftover test objects, `_Recovery`, numbered duplicates. Report counts + suspicious names. Do NOT edit scenes.

## §6 — MAP DESIGNER technical state (facts for ax to diagnose; do NOT fix)
The user says "Map Designer doesn't work." Gather facts only:
- List the Map Designer scripts (`Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs`, `Assets/Scripts/RoomPainter/**`, composer/resolver/cliff-solver, `RoomDataJson`/`LiveRoomReloader`).
- Note any compile-blocking issues you can see by reading (missing types, broken refs) — but do NOT assume; if it reads as compiling, say so.
- CURRENT_STATUS already documents: (a) saved rooms don't appear at runtime/F5 because `RoomDataJson`↔`LiveRoomReloader` schemas are incompatible, (b) the EditorWindow GUI can't be verified from MCP. Confirm/locate these in code and report the exact files/lines. This is the raw material ax will turn into a fix plan.

## §7 — STAGING + RIMA-root clutter census (user: "STAGING is full of clutter, simplify; not just STAGING, the whole RIMA folder")
- STAGING has ~572 files + ~50 dirs. Categorize the 572 files into buckets by topic/prefix (e.g. DETECT_*, CONSULT_*, *_S6, *_S114, ROOMTOOL_*, PIXELLAB_*, playtest, concepts/, _archive*, agy_snapshots/, etc.) with a count per bucket. Identify which buckets are clearly **superseded/closed-sprint** (S101–S114, abandoned pivots) vs **live** (current S6 work referenced by the top CURRENT_STATUS block). Flag `_archive*` and `agy_snapshots/` (the latter is LIVE config per project rule — do NOT propose archiving it).
- RIMA root: 30 loose `.md` files — list them, flag which look like dead dispatch logs / one-offs vs canonical (CLAUDE/RULES/AGENTS/CURRENT_STATUS/CODEX_*).
- Output a candidate "archive these buckets" list with rough file counts. Facts + candidate list ONLY — ax decides, user approves, a later pass moves them.

## Deliverable (write to CODEX_DONE_laurethayday.md, last step)
Structured, skimmable (tables/bullets). End with a `STATUS:` line. Sections §1–§7. NO cleanup actions beyond §1 re-pull. NO commit. Keep prose tight — this report feeds ax, not a human reader.
