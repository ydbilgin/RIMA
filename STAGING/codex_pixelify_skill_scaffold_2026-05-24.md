# Codex Task — `pixelify` Custom Skill Scaffolding (2026-05-24)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: HD PNG (gpt-image-2 vb. output) → pixel-perfect PNG converter custom skill yaz. PixelLab MCP "Create Image from Style Reference" feature'ını wrap eder. RIMA için tekrar kullanılabilir, LaurethStudio için reusable.

## Skill Spec

**Location:** `C:\Users\ydbil\.claude\skills\pixelify\`

**Trigger keywords:**
- "pixelify"
- "pixel perfect convert"
- "HD to pixel art"
- "$pixelify"
- "convert this to pixel art"

**Description (SKILL.md frontmatter):**
"Convert HD images (gpt-image-2 / Flux / Nano Banana / SDXL output) to pixel-perfect pixel art via PixelLab Style Reference. Routes through PixelLab MCP's create_image_pro with init image + style reference + AI Freedom=0. Supports custom PPU (default 64), target canvas size, palette quantization, transparent BG. Triggers on 'pixelify', 'pixel perfect convert', 'HD to pixel art', or any explicit ask to convert a non-pixel-art image to true pixel art."

## File Structure

```
~/.claude/skills/pixelify/
├── SKILL.md                    # Frontmatter + usage instructions
├── scripts/
│   ├── pixelify.py             # Main conversion logic
│   └── verify_pixel.py         # Pixel-perfectness QC (no anti-alias, palette count)
└── references/
    ├── pixellab_style_ref_guide.md  # How PixelLab Style Reference works
    └── ppu_grid_reference.md    # PPU=64 grid alignment notes
```

## SKILL.md Content (write exactly)

```markdown
---
name: pixelify
description: Convert HD images to pixel-perfect pixel art via PixelLab Style Reference. Routes through PixelLab MCP's create_image_pro with init image + AI Freedom=0. Supports custom PPU, target canvas, palette quantization. Triggers on 'pixelify', 'pixel perfect convert', 'HD to pixel art'.
---

# pixelify — HD to Pixel Art Converter

## When to use
- User has HD image (gpt-image-2 / Flux 2 / Nano Banana / SDXL output)
- Output needs to be true pixel art (sharp edges, limited palette, cell-aligned)
- RIMA / 2D pixel game asset production

## Workflow

1. Input: HD PNG path + optional target size + optional palette
2. Call PixelLab MCP `create_image_pro`:
   - `init_image_base64`: HD source encoded
   - `style_reference_base64`: HD source (same image as style anchor)
   - `ai_freedom`: 0 (strict reference adherence)
   - `target_width` / `target_height`: 1024 default, configurable
   - `style`: "pixel-art"
3. Optional post-process:
   - PIL palette quantize to N colors (default 32)
   - PIL nearest-neighbor downscale to final PPU canvas
   - Transparent BG via chroma-key (if magenta key present)
4. Output: pixel-perfect PNG

## Parameters

| Param | Default | Notes |
|---|---|---|
| input_path | required | HD source PNG |
| target_width | 1024 | PixelLab output |
| target_height | 1024 | PixelLab output |
| final_ppu | 64 | Unity PPU for downscale |
| palette_size | 32 | Quantize color count |
| transparent_bg | true | Chroma-key magenta to alpha |
| output_path | auto | Default: {input_dir}/{name}_pixel.png |

## Anti-patterns

- DO NOT use for already-pixel-art images (no-op, wastes credits)
- DO NOT set ai_freedom > 0 (composition will drift)
- DO NOT skip palette quantize for sprite production (PixelLab output may still have intermediate colors)

## Example invocation

```python
from pixelify import pixelify
pixelify(
    input_path="STAGING/concepts/ritual_chamber_room_concept_gpt_image_2_v1.png",
    target_width=2048,
    target_height=2048,
    palette_size=32,
    output_path="STAGING/concepts/ritual_chamber_pixel_v1.png"
)
```
```

## scripts/pixelify.py — Production Logic

Python script:
1. argparse: input_path, target_width, target_height, palette_size, output_path
2. PixelLab MCP client call (HTTP API):
   - Endpoint: `https://api.pixellab.ai/v1/create-image-pro` (or MCP tool call)
   - Auth: env var PIXELLAB_API_KEY veya MCP tool routing
   - Payload: init_image_base64, style_reference_base64, ai_freedom=0, style="pixel-art", target_width, target_height
3. Response handler: download PNG bytes
4. PIL post-process:
   - Palette quantize via Image.quantize(colors=palette_size)
   - Optional chroma-key alpha if magenta detected
5. Save output PNG

**Note:** Codex MCP'ye direkt erişimi yoksa, PixelLab Web UI manuel mode için fallback path documented olmalı (kullanıcıya step-by-step instruction).

## scripts/verify_pixel.py — QC

Python script:
1. Open PNG, get pixel data
2. Verify no anti-aliasing:
   - Check unique color count (<= palette_size)
   - Check pixel-perfect edges (no gradient sub-pixel)
3. Verify grid alignment:
   - At PPU=64, check if pixel pattern repeats at 64 boundaries
4. Output: PASS/FAIL with metrics

## references/pixellab_style_ref_guide.md

Belge:
- PixelLab Style Reference slot nasıl çalışır
- AI Freedom 0 = strict (recommended for pixelify)
- AI Freedom 100 = creative drift (NOT pixelify use case)
- Init image vs style reference farkı
- Best practices (palette pre-clean, contrast boost before input)

## references/ppu_grid_reference.md

Belge:
- RIMA PPU=64 LOCKED (Karar #74)
- 1 Unity unit = 64 pixels
- Tile grid 64-unit standard
- Wall canvas 128×384 (2 grid × 6 grid) example
- Pixel-perfectness criteria

## Verification

1. `~/.claude/skills/pixelify/SKILL.md` mevcut
2. `~/.claude/skills/pixelify/scripts/pixelify.py` syntax-valid
3. `~/.claude/skills/pixelify/scripts/verify_pixel.py` syntax-valid
4. references/ dosyaları mevcut
5. Custom skill listede görünür (next session başında)
6. Test invoke: `pixelify --help` çalışıyor

## Çıktı raporu

`STAGING/codex_pixelify_skill_report_2026-05-24.md` yaz:
- Created files list
- Test invoke result
- Issues / blockers
- Production-ready PASS/FAIL

git commit otomatik yapma — orchestrator review sonrası.
