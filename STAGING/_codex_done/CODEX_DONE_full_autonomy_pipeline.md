# VERDICT
Best pipeline: custom Python Wang16 compositor plus AI-generated inputs, with PixelLab/Comfy/gpt-image used only as autonomous asset sources and motif generators. The final 16 cells must be assembled by deterministic script, not by Aseprite, Pixelorama, Tilesetter GUI work, or hand-painted mask cleanup.

Revoked fallbacks:
- No Aseprite cleanup.
- No manual mask authoring.
- No hand-painted corner cells.
- No "user fixes the bad tile" step.

Wangscape is useful prior art and possibly a non-MVP experiment, but it is not the primary RIMA 2026 path. content_aware_tiles is the most interesting AI-native research path, but it targets 256/512 diffusion textures and needs a pixel-art adaptation pass before it can be trusted for 32px chunky RIMA floors. The MVP should be a small inspectable Python tool under `STAGING/wang16_compositor/` or `tools/wang16_compositor/` that generates one Granite-to-Path pair end to end.

# 1. Wangscape evaluation
Verdict: conceptually relevant, autonomous in principle, not mature enough to be the primary RIMA MVP.

Evidence from cloned repo `STAGING/wang16_full_autonomy_research/Wangscape`:
- Language: C++ 96 percent plus CMake.
- License: MIT.
- Last commit inspected: `b01cb31`, 2017-10-29.
- GitHub API release check: only `v0.1`, prerelease, published 2017-05-29.
- Dependencies: Boost, SFML 2, Armadillo/OpenBLAS, Qt5, spotify-json, libnoise, cpp_utils, googletest.
- Build target: command-line executable `Wangscape`.
- Example invocation: `./bin/Wangscape ../doc/examples/example3/example_options.json`.
- Example JSON accepts base terrain images, tile resolution, terrain cliques, alpha calculator mode, module-group files, and output directory.

Does it accept only base terrain tiles and generate Wang tiles autonomously?
Yes, mostly. The README says it generates alpha masks and combines terrain textures from base terrain images plus JSON config. Its algorithm doc says input is terrain image files and configuration files, including module groups for generating corner masks. That means the user does not draw masks by hand.

Why it is not the primary path:
- It is built around procedural noise alpha masks. That solves autonomy but tends toward smooth terrain blending, not RIMA's chunky 32px edge-wrap look.
- It is a legacy C++/OpenGL stack with many dependencies and a 2017 prerelease surface.
- Its JSON module groups are powerful but overkill for a one-pair MVP. Driving libnoise combiner graphs from Codex would add complexity before proving the art result.
- It generates corner Wang tilesets for terrain cliques, but RIMA also needs palette quantization, hard pixel masks, motif stamping, contact sheets, randomized preview maps, and Unity RuleTile naming/mapping.

Driver-script feasibility:
- Feasible wrapper:
  - Codex writes `wangscape_driver.py`.
  - It exports `example_options.json` from a Python config.
  - It writes terrain PNGs into a temp folder.
  - It invokes `Wangscape.exe <options.json>`.
  - It parses generated `tiles.json`, `tile_groups.json`, and `tilesets.json`.
  - It repacks the target two-terrain clique into RIMA's 16-key order.
- Feasibility score: 6/10 for automation, 3/10 for RIMA MVP art control.

Use Wangscape as reference, not foundation.

# 2. content_aware_tiles evaluation
Verdict: closest "dream solution" conceptually, but not selected for first production pipeline.

Evidence from cloned repo `STAGING/wang16_full_autonomy_research/content_aware_tiles`:
- License: Apache-2.0 in `LICENSE.txt` and `pyproject.toml`.
- Last commit inspected: `c7ce770`, 2025-05-26.
- GitHub API showed repo updated in 2026 and 22 stars at research time.
- Python package requires Python 3.11, diffusers, transformers, accelerate, torch, torchvision, numpy, scikit-image, pytorch-fid, textile-metric.
- CLI entry points: `generate_tiles` and `generate_classicwang`.
- README command: `uv run generate_tiles out/orange_lily --image doc/orange_lily.input.jpg --prompt "..."`
- Default inpainting model: `stabilityai/stable-diffusion-2-inpainting`.
- Default image model: `stabilityai/stable-diffusion-xl-base-1.0`.
- Default device: `cuda`.
- Default tile kinds: `wang,dual,self,classic_wang,rolled_self`.
- Default size: 256, with README note that size must be 256 for SD2 inpainting or 512 for SDXL inpainting.

Does it read as "feed one image, get Wang set"?
Yes. It can accept `--image` plus `--prompt`, or generate the source image from the prompt. It then creates Wang, Dual Wang, self-tiling, classic Wang, and rolled self-tiling outputs.

Compatibility with RIMA:
- Good:
  - Fully AI/script driven.
  - Training-free.
  - Supports Wang and Dual Wang.
  - RTX 5080 should be enough for CUDA SD2/SDXL workflows, especially with sub-batching.
  - It has ComfyUI integration, so it can become a local automated R&D backend.
- Bad:
  - Native output is diffusion-scale 256/512, not 32px chunky pixel art.
  - The code writes JPEG outputs for many generated artifacts, which is wrong for pixel art unless adapted.
  - Diffusion output can introduce anti-aliasing, gradients, soft natural texture, visual seams, invented lighting, and palette drift.
  - The method optimizes texture continuity, not Unity corner-key semantic control.
  - It creates edge-color Wang sets, not directly RIMA's two-material 16 corner mask resolver.

Score:
- Research/R&D: 8/10.
- Direct production for 32px RIMA MVP: 4/10.
- Future path after MVP: 7/10 if wrapped with pixelization, palette quantization, strict prompt presets, PNG export, and objective reject gates.

Recommended use:
- Create a later `tools/content_aware_tiles_probe/` experiment.
- Generate 256px Wang/dual Wang candidates from prompts or source images.
- Downsample with nearest-aware pixelization only after palette control.
- Use outputs as motif/reference sheets or L4 texture inspiration, not as final 16 cells until it passes automated pixel-art gates.

# 3. Procedural mask alternative
Verdict: yes, deterministic procedural masks can fully remove the "user draws mask" problem. This is the MVP path.

The mask generator should not imitate smooth alpha painting. It should create hard, pixel-honest ownership maps:
- Inputs: `tile_size=32`, `seed`, `roughness`, `jitter_px`, `threshold`, `dither_mode`, `palette`.
- For each of the 16 corner keys, assign each pixel to material A or B using corner ownership fields.
- Add deterministic boundary noise from shared edge seeds so adjacent tiles agree along borders.
- Use binary or posterized masks by default. Smooth alpha is opt-in and should fail RIMA MVP gates.
- Stamp AI-generated motifs only after masks are generated, and only through deterministic placement rules.

Code sketch:

```python
import numpy as np
from PIL import Image

def value_noise(size, seed):
    rng = np.random.default_rng(seed)
    grid = rng.random((size + 1, size + 1))
    y, x = np.mgrid[0:size, 0:size]
    gx = x / max(1, size - 1) * (size - 1)
    gy = y / max(1, size - 1) * (size - 1)
    x0 = np.floor(gx).astype(int)
    y0 = np.floor(gy).astype(int)
    x1 = np.minimum(x0 + 1, size)
    y1 = np.minimum(y0 + 1, size)
    tx = gx - x0
    ty = gy - y0
    a = grid[y0, x0] * (1 - tx) + grid[y0, x1] * tx
    b = grid[y1, x0] * (1 - tx) + grid[y1, x1] * tx
    return a * (1 - ty) + b * ty

def corner_mask(key, size=32, seed=1, roughness=0.18):
    # bit order: NW=8, NE=4, SW=2, SE=1
    y, x = np.mgrid[0:size, 0:size]
    corners = {
        8: ((0, 0), (key & 8) != 0),
        4: ((size - 1, 0), (key & 4) != 0),
        2: ((0, size - 1), (key & 2) != 0),
        1: ((size - 1, size - 1), (key & 1) != 0),
    }
    scores_a = []
    scores_b = []
    noise = (value_noise(size, seed + key) - 0.5) * roughness * size
    for _, ((cx, cy), is_b) in corners.items():
        d = np.abs(x - cx) + np.abs(y - cy) + noise
        (scores_b if is_b else scores_a).append(-d)
    a = np.max(scores_a, axis=0) if scores_a else np.full((size, size), -9999)
    b = np.max(scores_b, axis=0) if scores_b else np.full((size, size), -9999)
    return (b > a).astype(np.uint8) * 255

def compose(a_png, b_png, out_png, key):
    a = Image.open(a_png).convert("RGBA")
    b = Image.open(b_png).convert("RGBA")
    m = Image.fromarray(corner_mask(key), "L")
    out = Image.composite(b, a, m)
    out.save(out_png)
```

This sketch is intentionally small. The production version needs:
- shared-edge deterministic border fields,
- optional ordered dither instead of smooth alpha,
- palette quantization,
- motif masks,
- contact sheet export,
- randomized tiling preview,
- objective seam tests.

# 4. Selected pipeline
Selected path: Option C, custom Python with procedural mask plus AI-generated base and AI-generated motifs.

Fully autonomous workflow:

1. Claude dispatches material pair.
   - Example: `Cool Granite` to `Worn Stone Path`.
   - No user drawing.

2. Codex runs PixelLab MCP or approved AI image backend for flat base materials.
   - `create_tiles_pro` generates 16 base candidates for each material.
   - Script selects/crops/imports the chosen base tiles from URLs or local output.
   - If PixelLab output fails, Codex regenerates with stricter prompt and seed changes.

3. Codex runs AI generation for transparent edge motifs.
   - Use PixelLab `create_object`/`create_map_object`, gpt-image, or local ComfyUI/FLUX/SD.
   - Motifs are things like stone chips, moss bites, path crumbs, cracked rim fragments.
   - Output must be transparent PNG or script-extracted mask.
   - No manual cleanup. Script handles background removal, thresholding, palette snapping, and reject/regenerate loops.

4. Codex runs `compose_wang16.py`.
   - Reads `config.yaml`.
   - Reads `material_a/*.png`, `material_b/*.png`, `motifs/*.png`.
   - Generates procedural binary masks for keys 0..15.
   - Composes 16 cells in RIMA corner order.
   - Applies palette quantization and no-antialias cleanup.
   - Emits:
     - `out/tiles/key_00.png` through `key_15.png`
     - `out/wang16_sheet.png`
     - `out/contact_sheet.png`
     - `out/random_preview_seed_*.png`
     - `out/manifest.json`

5. Codex runs objective gates.
   - Seam gate: matching edge pixels for all legal adjacent key pairs.
   - Alpha gate: no semi-transparent pixels unless explicitly allowed for overlays.
   - Palette gate: max colors per tile and palette distance threshold.
   - Gradient gate: reject long smooth ramps.
   - Frame/rim gate: reject strong uniform border strips.
   - Preview gate: generate randomized 12x12 maps and run image checks for visible seams/repetition.

6. Claude/Codex imports to Unity.
   - Use UnityMCP or editor scripts to create/update `CornerWangTileSetSO`.
   - Assign key 0..15 sprites using existing RIMA corner mapping.
   - Paint automated test rooms.
   - Capture screenshots.
   - Run EditMode tests for resolver mapping and asset existence.

7. Codex iterates without user drawing.
   - If bad: change seed, mask parameters, motif prompt, motif density, or quantization.
   - If still bad: switch generator backend or reduce motif scope.
   - Never ask user to touch pixels.

# 5. Implementation plan
Concrete next dispatch: build a one-pair MVP compositor, not a full multi-material factory.

Files to add:
- `STAGING/wang16_compositor/README.md`
- `STAGING/wang16_compositor/config.example.yaml`
- `STAGING/wang16_compositor/compose_wang16.py`
- `STAGING/wang16_compositor/validate_wang16.py`
- `STAGING/wang16_compositor/sample_inputs/` with generated or placeholder script-created test tiles only if no real AI assets are available.

Dependencies:
- Python 3.12-compatible.
- Pillow.
- numpy.
- Optional: scikit-image only if needed for connected components or metrics.
- Avoid heavyweight diffusion dependencies in the compositor itself.

MVP command shape:

```bash
python STAGING/wang16_compositor/compose_wang16.py --config STAGING/wang16_compositor/config.example.yaml --out STAGING/wang16_compositor/out/granite_path_mvp
python STAGING/wang16_compositor/validate_wang16.py --manifest STAGING/wang16_compositor/out/granite_path_mvp/manifest.json
```

Config shape:

```yaml
tile_size: 32
seed: 14031
material_a:
  name: cool_granite
  inputs: STAGING/wang16_compositor/inputs/cool_granite/*.png
material_b:
  name: worn_stone_path
  inputs: STAGING/wang16_compositor/inputs/worn_stone_path/*.png
mask:
  mode: binary_corner_noise
  roughness: 0.18
  dither: ordered_2x2
motifs:
  enabled: true
  inputs: STAGING/wang16_compositor/inputs/motifs/*.png
  max_per_tile: 3
palette:
  max_colors_per_tile: 24
  snap_to:
    - "#1A1C20"
    - "#2A2D34"
    - "#3A3D48"
    - "#4E5260"
    - "#606575"
    - "#7BA7BC"
```

Acceptance gates:
- Gate 1: script creates exactly 16 32x32 PNG cells plus one sheet, one contact sheet, one randomized preview, and one manifest.
- Gate 2: `validate_wang16.py` passes all seam comparisons.
- Gate 3: no output cell has a visible 1-pixel frame on all four sides.
- Gate 4: no output cell exceeds configured palette count after quantization.
- Gate 5: no output has semi-transparent pixels in base Wang cells.
- Gate 6: randomized preview exists and uses at least 12x12 cells.
- Gate 7: Unity import can assign keys 0..15 to `CornerWangTileSetSO` without manual sprite slicing.

Single-pair MVP first:
- Pair: Cool Granite to Worn Stone Path.
- Do not add water, rift, grass, or multi-pair batching yet.
- Do not integrate content_aware_tiles until the deterministic compositor has passed one Unity room test.

# 6. Risks specific to "user cannot draw"
Risk: AI base tiles are borderline.
Autonomous loop: regenerate with stricter prompts, change seed/backend, or run script palette snapping and texture simplification. No manual edit.

Risk: generated motifs have bad backgrounds or soft edges.
Autonomous loop: use segmentation/background removal, alpha thresholding, connected-component filtering, palette snapping, or regenerate. No manual cleanup.

Risk: procedural masks look too mathematical.
Autonomous loop: adjust deterministic roughness, use shared edge-noise seeds, add ordered dither, add motif stamps, or generate multiple mask families and score previews automatically.

Risk: outputs are seam-correct but visually repetitive.
Autonomous loop: emit 2-4 deterministic variants per key, selected by cell hash in Unity. Use L4/L5/L6 overlays via existing RIMA auto-dress logic.

Risk: diffusion approach looks better but is not pixel-clean.
Autonomous loop: keep it as source/motif/reference generation, then route through the same compositor and validators.

Risk: every automated output fails the visual target.
Autonomous loop:
- First lower ambition to readable MVP: clean flat bases plus rough binary transition.
- Then improve by adding motif stamps and variant-per-key generation.
- Then test content_aware_tiles as a separate backend.
- Still no hand-painting fallback.

Research sources inspected:
- Local clones under `STAGING/wang16_full_autonomy_research/`.
- Wangscape README, license, algorithm doc, schemas, example options.
- content_aware_tiles README, license, pyproject, source files.
- IJDykeman/wangTiles README.
- Gollum999/perlin-wang README.
- fogleman/WangTiling README and script.
- GitHub API metadata for update/license/release checks.
- NotebookLM query for RIMA full-autonomy context.
- Web pages for OpenGameArt Wang Blob template, Tilez, itsjavi/autotiler, and TilePipe.
