# Kit A+B+C Logical Layout Verdict - Codex

## 1. PPU per kit
Kit A stays locked at PPU=64. The live tile imports confirm `spritePixelsToUnits: 64`, and the current 64x64 source size means every floor tile occupies 1x1 sprite units while the isometric Grid projects it through `cellSize`.

Kit B should not ship as raw 1024x1536 sprites at PPU=64, because that becomes 16x24 world units per cliff. Production target is pixelify/downscale each cliff face to 128x192 and import at PPU=64. Math: 128/64=2.0 world units wide, 192/64=3.0 world units tall. This matches the mockup's 96-128 px width and gives one cliff module a clean two-cell coverage. For throwaway raw-reference prototyping only, import the 1024x1536 files at PPU=512 to get the same 2x3 world-unit footprint.

Kit C should be split by role. Raw L0/L1 1254x1254 and L2/L4 1672x941 can import at PPU=32 for large parallax overscan: L0/L1 become 39.19x39.19 units, and L2/L4 become 52.25x29.41 units. L3 islands should be processed as modular sprites, not huge full-screen plates: 256x256 small at PPU=64 gives 4x4 units, and 512x512 large at PPU=64 gives 8x8 units.

## 2. World-space size and placement
A 12x8 arena has a logical floor span of 12x4 world units at 1x0.5 per cell. Under Unity isometric projection, its visible diamond bounds are approximately 10x5 world units: `(12+8)*0.5=10` wide and `(12+8)*0.5*0.5=5` tall. Keep camera orthographic size 5 for a 10-unit vertical viewport; at 16:9 that gives 17.78 units width, enough to include BG overscan and the south cliff. Camera position should stay near `(0, -0.35, -10)` or follow target with a slight negative Y offset so the front cliff is visible.

Use top-center pivots for Kit B. Place 6 south modules and 6 north modules at 2-cell spacing, 4 east modules and 4 west modules at 2-cell spacing, plus 4 corner sprites, for a full 24-piece 12x8 ring. South edge uses `cliff_S` and hangs below the floor rim. With top-center pivot, put the top of the sprite on the rim plus a tiny overlap, about `rimY - 0.03`; with center pivot, place center at `rimY - 1.5`. Generate positions from Tilemap/Grid cell centers, not hand-tuned screen pixels.

## 3. Sorting and parallax math
Use sorting for draw order, Z only for editor readability and parallax grouping. Current TagManager puts `Walls` after `Floor`, so do not use `Walls` for behind-floor cliff unless the sorting layer order is changed.

| Layer | Sorting Layer | Order in Layer | Z position | Parallax factor |
|---|---:|---:|---:|---:|
| Kit C L0 void | Ground | -500 | +20 | (0.03, 0.02) |
| Kit C L1 nebula | Ground | -430 | +16 | (0.05, 0.04) |
| Kit C L2 ruins | Ground | -380 | +12 | (0.08, 0.05) |
| Kit C L3 islands | Ground | -320 | +8 | (0.14, 0.08) |
| Kit C L4 fog | Ground | -260 | +5 | (0.10, 0.06) |
| Kit A floor | Floor | 0 | 0 | (1.00, 1.00) |
| Kit B cliff face | Floor | -20 | -0.05 | (1.00, 1.00) |
| Player/enemies | Characters/Player | y-sorted | 0 | (1.00, 1.00) |

Parallax should be origin-based, not cumulative drift:

```csharp
var delta = (Vector2)(camera.position - cameraStart);
var p = layerStart + new Vector3(delta.x * factor.x, delta.y * factor.y, 0f);
p.x = Mathf.Round(p.x * 64f) / 64f;
p.y = Mathf.Round(p.y * 64f) / 64f;
transform.position = p;
```

## 4. Overlap fix
Fastest fix is option A: set Grid `cellSize.y` to `39/64 = 0.609375` and round to 0.61 in inspector-facing tools. This matches the discovered 62x39 visible diamond better than the current 32 px vertical cell step. Option B, pivot edits, can hide a row seam but cannot fix the projection mismatch. Option C, regenerating Kit A as strict 64x32 dimetric tiles, is the correct long-term fix because it returns the art to standard 2:1 iso math.

Fastest Unity-side snippet:

```csharp
Grid grid = floorTilemap.layoutGrid;
grid.cellSize = new Vector3(1f, 39f / 64f, 1f);
```

## 5. Pixel filter verdict
Pixelify Kit B before production. It touches the playable rim, shares silhouettes with the floor, and will look muddy if a HD painterly cliff sits directly under true pixel tiles. Kit C can remain HD for prototype if it is treated as soft unlit parallax, desaturated, and never allowed to compete with gameplay detail. Production should pixelify L3 islands and any L4 fog that overlaps the arena edge. Engineering trade-off: pixelified B/C foreground assets atlas cleanly, use point filtering, no mipmaps, and cooperate with Pixel Perfect Camera. HD Kit C costs more texture memory and wants bilinear/mipmaps, so keep it in a separate atlas/material set. PPU does not directly change draw calls; material, atlas, lights, and renderer count do.

## 6. Prefab strategy
Kit B should be prefabbed as GameObjects with SpriteRenderer, not Tile assets. The cliff has direction-specific art, top pivots, glow overlays, and overhang depth; forcing it into a Tilemap makes corner logic harder. Create `CliffSegment_S/N/E/W/NE/NW/SE/SW` prefabs and a `CliffRing_12x8` parent prefab or generator that owns placement.

Kit C should be a `RoomBackgroundRig` prefab with child SpriteRenderers L0-L4 and a `ParallaxLayer` component on each child. L0 may use SpriteRenderer tiled draw mode or repeated quads. L2 and L4 can be repeated horizontal strips. L3 islands are separate modular SpriteRenderers for art direction placement.

## 7. Performance budget
Target BG at 3-6 batches and 8-14 SpriteRenderers. Combat rooms should stay around 35-45 draw calls, Ritual rooms 45-55, and Boss rooms no higher than 60 unless a one-off VFX burst is profiled. Use separate atlases: `RIMA_Floor_Iso35`, `RIMA_Cliff_KitB`, `RIMA_BG_KitC_HD`, and `RIMA_FX_Additive`. Do not pack point-filtered pixel sprites with bilinear HD backgrounds. Pixel Perfect Camera should use AssetsPPU=64 and RefResolution 1280x720; the live scene currently shows AssetsPPU=32, which conflicts with the 64 PPU floor target.

## 8. Production order
First, apply the Grid y fix to `Assets/Scenes/Test/PlayableArena.unity` and recheck `iso_overlap_test` with adjacent tile rows. Second, import Kit B starting with `cliff_S`, `cliff_E`, `cliff_W`, and the four corners into `Assets/Sprites/Environment/KitB_Cliff`; process to 128x192 PPU=64. Third, import Kit C starting with `bg_L0_void`, `bg_L2_ruins_A`, `bg_L4_fog`, then L3 islands into `Assets/Sprites/Environment/KitC_BG`. Fourth, create `ParallaxLayer.cs`, attach it to the `RoomBackgroundRig` children, and instance the rig behind the floor in `PlayableArena`. Fifth, build the 12x8 sample arena in order: BG rig, floor Tilemap, cliff ring, player/enemies, front-only VFX. Test with 1280x720, 1366x768, and 1920x1080 screenshots, camera pan shimmer, sorting at the south rim, and Unity Profiler draw calls.

VERDICT: KitB final=128x192 PPU=64 / KitC BG PPU=32 with L3 islands PPU=64 / Grid cellSize.y=0.609375 / Pixel Perfect Camera AssetsPPU=64 / Ground:-500..-260, Floor cliff:-20, Floor:0
