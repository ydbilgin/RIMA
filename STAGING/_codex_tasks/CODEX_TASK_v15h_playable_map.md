# Codex Task — v15h Playable Map (Wang wire + density + adjacency cap + WASD test)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## User direction (verbatim)
"üretimleri şu an minimuma indir çok pixellab şeyi harcamayalım. map kısmını yapalım. bi deneyelim map ile ne yapabiliyoruz görelim. eksik parçaları üretiriz bakarız oynanabilir mapi bi yapalım sonra çeşitlendirmek kolay iş"

→ Stop more asset production. Make the existing assets compose into a PLAYABLE clean map.

## Goal — v15h playable map

Build on v15g (`profile_v15g_minimal_pixellab` + clean PixelLab tiles, no rune chaos), fix the 3 problems:
1. Sparse coverage (~50% → ~85%)
2. Hard tile edges (no Wang transitions)
3. Add Wang v2 32×32 (`STAGING/pixellab_wang_v2_32px/wang_dirt_cobble_32px.png` — 16 tile dirt→cobblestone organic transition, no grass)

Then make it PLAYABLE: spawn Warblade character (`dbfbb77d-692f-4ced-8d79-a0d539e599d5`) anchor in scene, WASD movement test in PlayMode.

## Files to modify/create

### 1. Import Wang v2 32×32 to Unity
Source: `STAGING/pixellab_wang_v2_32px/wang_dirt_cobble_32px.png` (16 tiles in single PNG, 32×32 each, 4×4 grid = 128×128 total)

Steps:
- Import as Sprite with auto-slice (4 columns × 4 rows, 32×32 each)
- Save 16 individual sprites to `Assets/Data/Brush/AssetParts_v3/CombatBiome_v15h/Wang/`
- Generate Unity Tile assets for each (16 Tile.asset)
- Create RuleTile asset that maps 4-corner Wang16 logic to these 16 tiles: `Assets/Data/Brush/AssetParts_v3/CombatBiome_v15h/Wang/rule_dirt_cobble_wang.asset`

### 2. Update profile + zone .asset
`Assets/Data/Blueprint/Profiles/profile_v15g_minimal_pixellab.asset` (duplicate to v15h):
- Add `wangRuleTileRef` pointing to rule_dirt_cobble_wang

Zone .asset updates:
- `zone_stone_v15g.asset` → density 0.50 → 0.85
- `zone_dirt_v15g.asset` → density 0.50 → 0.85
- New: `adjacencyDecalsPerRoom` cap = 8 (prevents blue rune circle scatter — currently overrides composition budget)

### 3. AdjacencyRuleSO budget field (small SO addition)
`Assets/Data/Blueprint/AdjacencyRules/AdjacencyRuleSO.cs`:
```csharp
[Range(0, 30)] public int decalsPerRoomCap = 8; // Default 8. Was unlimited before — caused rune circle scatter
```

AutoPopulator: enforce in adjacency placement loop, stop after cap reached.

### 4. New composer or extend RimaV15cSceneComposer
Add v15h Build method OR modify RimaV15cSceneComposer:
- ScenePath: same (RoomPipelineTest)
- V15hRootName: `Pro_Redesign_v15h_Playable_CombatRoom`
- Disable v15c/v15d/v15e/v15g roots
- Use profile_v15h_playable
- Spawn Warblade prefab/SpriteRenderer at scene center
- Wire WASD input (PlayerController2D if exists, else simple Transform.Translate)
- Screenshot: `Assets/Screenshots/PlayableRoom_combat_v15h_playable_LIVE.png`

### 5. EditMode tests (3 new)
- Wang RuleTile asset validates 16-tile mapping
- Adjacency decals cap respected
- Density 0.85 produces ~85% cell fill

## Acceptance
- 411 + 3 = 414 EditMode tests PASS
- v15h screenshot shows CLEAN tile coverage (~85%, no large black gaps), SMOOTH Wang transitions at edges, NO rune circle scatter, Warblade spawned visible
- DONE marker: `STAGING/CODEX_TASK_v15h_playable_map_DONE.md`
- PlayMode: WASD moves Warblade across the map (if PlayerController2D exists; if not, manual test only)
- Side-by-side: `Assets/Screenshots/PlayableRoom_combat_v15g_vs_v15h.png` (or skip if MCP image too large)

## Time budget
- ETA: 1-2 hours
- Use yasinderyabilgin profile (laurethgame keeps aborting — known issue)

## What NOT to do
- No new PixelLab calls (use existing Wang v2 + cobble v1 + dirt v1)
- No new Codex imagegen
- No character state production
- No weapon production (sword 96 already done, use existing)
- No Brush V1 file touch (user uncommitted)
- No commit

## Honest constraints
- Wang16 RuleTile setup is the biggest unknown — if PixelLab Wang ordering doesn't match Unity Tilemap Extras RuleTile expectations, mapping needs manual tweaking
- Density 85% might still feel sparse if zone shape is small — designer painted zones cover ~50% of grid; that's the max possible without re-painting
- If PlayerController2D doesn't exist, fallback to just spawning Warblade SR (manual playmode test later)
