# Codex Task — Unity Import + PatchAtlasSO Setup (Asset Parts v2)

**Profile:** yasinderyabilgin (parallel-safe — laurethgame + laurethayday boş şu an)
**Effort:** high
**Type:** Mechanical (sprite import + SO asset creation + SpriteAtlas build)

## Context

84 sliced PNG parts ready at `STAGING/RIMA_AssetParts_v2/sliced/` from Brush V1 Asset Parts v2 pipeline. Source 8 sheets at `STAGING/RIMA_AssetParts_v2/sheet_*.png` (post alpha-clamp clean). Phase 1A SO contracts LIVE (`PatchAtlasSO`, `RoomVisualProfileSO`, `ImportAssetRole`). Phase 1.5 data-first decal migration LIVE (333/333 EditMode PASS).

This task moves 84 PNG parts into `Assets/Sprites/Environment/RIMA_AssetParts_v2/` with correct importer settings, then builds 5 `PatchAtlasSO` ScriptableObjects + SpriteAtlas for the chunked renderer.

## Inputs

Source folder: `STAGING/RIMA_AssetParts_v2/sliced/`

| Category | Files | Final px | Count |
|---|---|---|---|
| `floor/` | `floor_01..16.png` | 32×32 | 16 |
| `macro/` | `macro_01..08.png` | 128×128 | 8 |
| `moss/` | `moss_01..16.png` | 64×64 | 16 |
| `dirt/` | `dirt_01..12.png` | 64×64 | 12 |
| `pebbles/` | `pebbles_01..12.png` | 64×64 | 12 |
| `cracks_bones/` | `cracks_bones_01..12.png` | 64×64 | 12 |
| `rift/` | `rift_01..04.png` | 256×256 | 4 |
| `ritual/` | `ritual_01..04.png` | 256×256 | 4 |

**Total: 84 PNG parts**

## Tasks

### 1. Copy + import (Unity-MCP via `manage_asset`)

Target paths (mirror sliced/ structure):
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/floor/floor_01..16.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/macro/macro_01..08.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/moss/moss_01..16.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/dirt/dirt_01..12.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/pebbles/pebbles_01..12.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/cracks_bones/cracks_bones_01..12.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/rift/rift_01..04.png`
- `Assets/Sprites/Environment/RIMA_AssetParts_v2/ritual/ritual_01..04.png`

**Importer settings** (per file, via TextureImporter):
- `textureType = Sprite`
- `spriteImportMode = Single`
- `filterMode = Point` (no bilinear)
- `mipmapEnabled = false`
- `alphaIsTransparency = true`
- `pixelsPerUnit = 32` (matches `PPU=32 + cell=1 unit IMMUTABLE` hard rule)
- `wrapMode = Clamp`
- `compressionQuality = Normal`
- `textureCompression = TextureImporterCompression.Uncompressed` (Editor preview clarity)
- `spritePivot = (0.5, 0.5)` (center)

For floor tiles only: `wrapMode = Repeat` (since they may tile seamlessly).

### 2. Generate 5 `PatchAtlasSO` assets

Per Phase 1A SO contract `Assets/Scripts/MapDesigner/Brush/Data/PatchAtlasSO.cs`. Reference existing SO with `Read` first to match field names exactly.

Output paths:
- `Assets/Data/Brush/AssetParts_v2/BaseFloor.asset` — references 16 floor sprites + 8 macro sprites
- `Assets/Data/Brush/AssetParts_v2/OrganicDecal_Moss.asset` — 16 moss sprites
- `Assets/Data/Brush/AssetParts_v2/OrganicDecal_Dirt.asset` — 12 dirt sprites
- `Assets/Data/Brush/AssetParts_v2/DetailScatter_Pebbles.asset` — 12 pebble sprites
- `Assets/Data/Brush/AssetParts_v2/DetailScatter_CracksBones.asset` — 12 cracks/bones sprites
- `Assets/Data/Brush/AssetParts_v2/Accent_Rift.asset` — 4 rift sprites
- `Assets/Data/Brush/AssetParts_v2/Accent_Ritual.asset` — 4 ritual sprites

(7 SO assets total. If `PatchAtlasSO` schema requires one role per asset, split BaseFloor into `BaseFloor_Tiles.asset` + `MacroPatch.asset`.)

Assign `ImportAssetRole` enum per the existing role taxonomy.

### 3. Build SpriteAtlas

- `Assets/Data/Brush/AssetParts_v2/RIMA_AssetParts_v2.spriteatlas`
- Pack mode: Tight
- Include all 7 category folders
- Filter mode: Point
- Compression: None (preview)
- Variant: none for now (Phase 1.5 chunked renderer reference)

### 4. Compile + test

- Unity refresh, wait for `isCompiling = false`
- Run EditMode tests: expect **333/333 PASS** (no regression)
- If new test FAIL, do NOT modify SO contracts — investigate import errors via `read_console`

### 5. Write DONE marker

`STAGING/CODEX_TASK_UNITY_IMPORT_ASSET_PARTS_V2_DONE.md`:
- File count moved to Assets/
- 7 PatchAtlasSO assets paths
- SpriteAtlas path
- Test count delta (should be 333/333 PASS unchanged)
- Any importer warnings encountered
- SHA256 of one sample sprite per category (verification)

## Constraints

- Do NOT modify `BrushPipelineConfigSO` feature flags (leave `useDataFirstDecals=false` default)
- Do NOT touch existing PatchAtlasSO assets in other folders
- Do NOT modify Phase 1A SO contract scripts (read-only reference)
- If `PatchAtlasSO` requires `Sprite[]` array vs single `Texture2D`, prefer Sprite[] — atlas-friendly
- Unity must be open (active editor MCP route). If not, document failure in DONE marker
- All `.meta` files must be deterministic GUIDs (Unity auto-generates first time)

## Memory references

- `[[room-composer-paint-intent-lock]]` — paint-intent semantic brush, macro+decal+scatter layers
- `[[multi-projection-architecture-lock]]` — 6 hard rule (PPU=32 immutable, SO render-stack ban)
- `[[hybrid-asset-pipeline-lock]]` — PixelLab characters + Codex imagegen tiles/decals

## NEXT_SIGNAL after DONE

Open `RoomBankRuntimeTester` sample scene, manually assign one `BaseFloor` + one `OrganicDecal_Moss` + one `Accent_Rift` patch atlas to a sample RoomTemplateSO, and trigger Brush V1 paint test. Visual gate verdict pending after sample render.
