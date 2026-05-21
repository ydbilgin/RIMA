# Codex Task — 10-Repo Room Composer Library Evaluation for RIMA

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Goal
Evaluate 10 GitHub/open-source projects against RIMA's Unity Room Composer / Room Designer system. Identify what to reuse, adapt, fork, or ignore. Produce structured analysis Opus can synthesize into Phase 1.5 architecture decisions.

## RIMA CONTEXT (input constraints)

- Unity project, orthographic camera, fake-isometric / high top-down ~30-35°
- PPU = 32, base logical tile = 32×32, characters/props 64×64+
- **Room Composer**, not simple tile painter
- **RoomData = source of truth**, not Unity scene hierarchy
- Visual-only details must NOT become thousands of GameObjects
- **Wang16** only for hard feature/elevation boundaries (cliffs, raised platforms, water/lava/poison/hazard borders, hard terrain edges)
- Same-elevation natural blending must NOT use Wang16
- Same-elevation blend = simple base floor + macro floor patches + organic decals + detail scatter + shadow/glow/manual polish
- Target feeling: natural, layered, paint-like room editing similar to Alabaster Dawn / CrossCode / Hades readability
- **No ECS/DOTS migration**
- **No GameObject per decal/patch/scatter**
- v15d composition budget LOCK active: 20% neg space + 70/20/10 floor + 3 cluster cap + 8 palette/zone + 15% path. See `STAGING/CODEX_DONE_BOONA_REVIEW.md`.

## Repos to evaluate

1. **Unity 2D Tilemap Extras** — https://github.com/Unity-Technologies/2d-extras
   - Inspect: Editor/Brushes, Runtime/Tiles, Random Brush, GameObject Brush, Group Brush, Line Brush, RuleTile, RuleOverrideTile, WeightedRandomTile, GridInformation, TintBrush
2. **Unity 2D Tech Demos** — https://github.com/Unity-Technologies/2d-techdemos
   - Inspect: example scenes, Tilemap/RuleTile setup, editor workflow examples
3. **QuickRuleTileEditor** — https://github.com/stalengd/QuickRuleTileEditor
   - Inspect: RuleTile authoring, 16/15/47 patterns, sprite mapping, UI Toolkit editor
4. **Weighted Random Brush / Prefab Random Brush** — https://gist.github.com/unitycoder/71c0c1dcdf5300be42191a2911a79a50 + https://docs.unity.cn/Packages/com.unity.2d.tilemap.extras@1.5/manual/WeightedRandomTile.html
   - Inspect: weighted selection, brush paint override, FloodFill, deterministic seed
5. **Ogmo Editor 3** — https://ogmo-editor-3.github.io/ + https://ogmo-editor-3.github.io/docs/
   - Inspect: Tile/Decal/Entity/Grid layers, JSON format, decal free-position/scale/rotation, MIT license
6. **LDtk** — https://github.com/deepnight/ldtk + Unity importer https://github.com/Cammin/LDtkToUnity
   - Inspect: world/level organization, entity fields, IntGrid, AutoLayers, importer behavior
7. **Tiled + SuperTiled2Unity** — https://github.com/mapeditor/tiled + https://github.com/Seanba/SuperTiled2Unity
   - Inspect: TMX/TSX, object layers, custom properties, scripted importer, prefab replacement
8. **PathPaintTool** — https://github.com/Roland09/PathPaintTool
   - Inspect: multi-brush stroke, inner/middle/outer rings, stroke vs drag mode, preset architecture
9. **PVTUT (Procedural Virtual Texture)** — https://github.com/ACskyline/PVTUT
   - Inspect: editor preview vs runtime bake idea, dynamic decals, chunk/LOD concepts
10. **MackySoft Choice (weighted random selector)** — https://github.com/mackysoft/Choice
    - Inspect: weighted selector implementation, API ergonomics, Unity compat, license

## Methodology per repo

For each repo:
1. WebFetch the README + key source paths
2. Identify concrete systems/files/classes worth studying
3. Verdict: reuse direct / copy conceptually / fork+adapt / inspiration only / ignore
4. Map to RIMA Room Composer (which RIMA system would consume it)
5. What NOT to copy (anti-pattern warnings)
6. License + Unity version compat note
7. Phase 1 / 1.5 implementation safety

## Output

Write `STAGING/CODEX_DONE_ROOM_COMPOSER_LIBRARY_EVAL.md` with:

```
# Room Composer Library Evaluation

## 1. Ranked Tier List
- MUST STUDY (3-5)
- USEFUL REFERENCE (3-5)
- OPTIONAL (1-3)
- NOT WORTH INTEGRATING (0-3)

## 2. Per-repo deep-dive (10 sections)
For each:
- Take
- Avoid
- RIMA integration idea
- Risk
- License + Unity version

## 3. Architecture recommendation
Concrete stack proposal (similar to ChatGPT's expected pattern below):
- Unity 2D Tilemap Extras as brush/editor foundation
- RIMA custom data-first brushes instead of normal direct tile painting
- QuickRuleTileEditor/RuleTile only for hard feature boundaries
- Weighted random logic for asset variant selection
- Ogmo layer model as conceptual reference for Tile/Decal/Entity/Grid separation
- LDtk/Tiled optional external blockout only
- PathPaintTool as inspiration for multi-ring semantic brush behavior
- PVTUT as inspiration for editor preview vs runtime bake
- RoomData remains source of truth
- Chunked/batched visual renderer for visual-only details
- GameObjects only for active gameplay entities

Critique this pattern — agree, modify, reject parts.

## 4. Phase 1.5 implementation outline
- 1.5A Data model (RoomData, VisualPlacementData, SemanticMaskData, DirtyChunkData, DecalPaletteSO, ScatterPaletteSO, PatchAtlasSO, RoomVisualProfileSO)
- 1.5B Brushes (RimaOrganicBrush, RimaScatterBrush, RimaFeatureBrush, RimaPropClusterBrush)
- 1.5C Rendering (L2 Tilemap base, L3 Tilemap/Wang hard boundary, L2b/L4/L5/L6 chunked visual layer)
- 1.5D Benchmark plan (1000 GameObjects vs 1000 RoomData placements through chunked renderer)

## 5. Final recommendation
Integrate as direct dependency / fork / study only — per repo.

## 6. Confidence + caveats
HIGH/MED/LOW per major recommendation + why.
```

Constraints:
- BE CRITICAL — do not say "everything is useful"
- Maximum 600 lines total
- Concrete file/class names where possible
- Reject ECS/DOTS migration if any suggestion approaches it
- Preserve RoomData as source of truth in all recommendations

## DONE marker
`STAGING/CODEX_TASK_ROOM_COMPOSER_LIBRARY_EVAL_DONE.md`

## What NOT to do
- No code edits anywhere in repo
- No new task files
- No PR draft
- Don't fetch every file in every repo — sample 3-5 key files per repo
- Don't generate any image, no PixelLab calls
