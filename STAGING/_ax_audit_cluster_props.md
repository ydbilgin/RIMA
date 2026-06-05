# Audit Task — Cluster-prop asset inventory + spec draft (FILE-READ ONLY, do NOT touch Unity)

Context: RIMA council decided ([STAGING/MODULAR_PROPS_DECISION_2026-06-05.md] read it) to author 2-3
"cluster" props (e.g. boulder pile + fallen log as ONE multi-cell prop, like the reference garden game)
using existing PropDefinitionSO multi-cell footprint support. Asset GENERATION is gated (user-only) —
so first we need an inventory: what sprites exist, what must be generated.

## Do (file reading only — NO Unity MCP, NO file writes except your stdout):
1. Inventory existing prop/obstacle sprite assets. Look under:
   - `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Art\` (subfolders — list anything obstacle/prop/rock/log/debris-like)
   - `F:\Antigravity Projeler\2d roguelite\RIMA\Assets\Data\Props\` (read the 7 PropDefinitionSO .asset files — names, footprint sizes, sprite references)
   - `F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\imagegen\assets\` (Batch-1 obstacle/door/decor outputs if present)
2. For each candidate: filename, approximate purpose, single-cell or multi-cell potential.
3. Answer: can ANY existing sprite serve as a multi-cell cluster (rock pile, bone heap, rubble) TODAY without new art? Which 2-3?
4. For missing cluster art, DRAFT generation specs (2-3 items max) consistent with Act-1 canon
   (read `STAGING/OBSTACLES_DOORS_DECISION_2026-06-03.md` if present: slate-grey stone, cyan #00FFCC only as rift/seal emissive,
   bones=failed containers, 64px character scale, iso 3/4 view): for each — name, footprint (e.g. 2x2/3x2),
   pixel size, palette notes, PixelLab create_map_object-style prompt draft.
5. Output: inventory table + "usable today" list + gen-spec drafts. Short, concrete.