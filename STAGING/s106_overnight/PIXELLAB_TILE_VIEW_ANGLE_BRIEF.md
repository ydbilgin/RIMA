# PixelLab Tile View Angle Brief — rima-sonnet — 2026-05-25

## Q1. view_angle convention mapping

The `create_tiles_pro` schema defines TWO separate parameters (NOT synonyms):
- `tile_view` (string, discrete): `"top-down"` | `"high top-down"` | `"low top-down"` | `"side"`
- `tile_view_angle` (number, continuous): 0–90, where 0 = side, 90 = true top-down

The b340684f bug (documented in `/STAGING/codex_task_tiles_pro_param_test.md`) is exactly this: the previous call used `tile_view = "high top-down"` (string) which the API resolved to ~75-85° angle, producing a horizontal "bottom thickness band." The fix dispatched was `tile_view_angle = 90` (number) + `tile_depth_ratio = 0`.

**Discrete label approximate mapping (inferred from context, not officially published):**
- `"top-down"` → ~90° (pure overhead, minimal 3D read)
- `"high top-down"` → ~70-85° (overhead but with slight 3D read; produces depth band at lower edge)
- `"low top-down"` → ~30-50° (oblique, strong 3D read, dimetric-adjacent)
- `"side"` → ~0° (orthographic profile)

The UI's "35° high top-down" label likely measures degrees FROM VERTICAL (zenith offset). 35° from vertical = 55° elevation from horizontal. The schema scale (0=side, 90=top-down) is elevation from horizontal.

## Q2. Dimetric math match

- True dimetric 2:1 isometric: 26.57° elevation from horizontal (arctan(0.5))
- In PixelLab scale: `tile_view_angle ≈ 27` for true dimetric
- RIMA's Karar #114 "HIGH top-down 3/4" (Hades/CoM lock at 70-80° from horizontal): `tile_view_angle ≈ 70-80`
- For RIMA floor tiles specifically (flat ground texture, not 3D iso block): `tile_view_angle = 90` + `tile_depth_ratio = 0` produces a clean overhead floor texture

**Recommendation:** `tile_view_angle = 90` (number, NOT the `tile_view` string). This matches the existing Codex fix from `codex_task_tiles_pro_param_test.md`.

## Q3. Cross-tool consistency

- `create_tiles_pro` floor tiles: `tile_view_angle = 90` (continuous number). Do NOT pass `tile_view` string simultaneously (causes conflict).
- `create_object` / `create_map_object` props: `view = "high top-down"` (only discrete; ~70-85° internally). Inherently matches Karar #114 high top-down.
- The floor tile (90° flat) and the prop ("high top-down" ~75°) will differ — THIS IS BY DESIGN. Floors are flat textures; props have slight 3D angle. Matches Hades/CoM convention.

## Q4. Tile size recommendation

- Warblade: 48px sprite
- At 64px tiles: character = 0.75 tile widths → tile dominates character visually
- At 32px tiles: character = 1.5 tile widths → matches Hades/CoM ratio
- `create_topdown_tileset` (Wang) only supports 16/32px — 32 maintains chaining compat
- Previous a1_floor 64px QC report: 4/10 score, citing strong border seams (detail density too coarse)

**Recommendation: 32px.** Caveat: if detail loss at 32 (crack depth, rift lines), test 48px (range 16-128).

## Q5. Depth ratio

- For 90° top-down floor tiles: `tile_depth_ratio = 0.0` (no visible side face from directly above)
- Non-zero adds an artifact "thickness band" at the bottom edge (the b340684f bug)
- Crack visual is TEXTURE detail (from prompt), NOT geometry depth

**Recommendation: 0.0.**

## TL;DR — Recommended regenerate params

```python
mcp__pixellab__create_tiles_pro(
    description="1) dark granite cobblestone dungeon floor 2) cracked granite + glowing cyan veins 3) packed dirt + stone fragments 4) ritual stone + arcane rune symbols — dungeon interior, stone #3A3D42, cyan accent #00FFCC, dark atmospheric",
    tile_type="square_topdown",     # NOT isometric — floor is flat overhead
    tile_size=32,                    # NOT 64 — better character/tile ratio
    tile_view_angle=90,              # NUMBER (not tile_view string) — true overhead
    tile_depth_ratio=0.0,            # no thickness band artifact
    outline_mode="segmentation",     # cleaner than outline
    seed=42
)
```

**Do NOT pass `tile_view` (string) simultaneously with `tile_view_angle` (number).** Conflict / undefined behavior per documented bug.

## Schema gaps / caveats

- Discrete-to-degree mapping for `tile_view` string is NOT officially published. Above is inferred from `codex_task_tiles_pro_param_test.md` + context. If PixelLab publishes spec externally, that takes precedence.
- UI's "35° high top-down" degree display convention is not confirmed in local docs. Interpretation above (35° from vertical = 55° elevation) is most internally consistent but inferential.

## Next-step verification

- Antigravity visual eval (parallel task `bruixo09d`) will confirm if 90° pure top-down produces the chatgpt_ref look or if a lower elevation (50-70°) is visually preferred
- If Antigravity disagrees with this 90° recommendation, the visual evidence wins
- If Antigravity confirms 90° is correct, the actual gap is tile_size (64→32) + tile_type (isometric→square_topdown) + chaos (random theme mix → weighted)
