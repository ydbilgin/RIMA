# ISO A/B MOCK — missing iso art (cx imagegen, profile: laurethayday)

ACTIVE RULES: (1) think before generating (2) min outputs, exactly what's listed (3) surgical — only these files (4) BLOCKED if unclear.
NLM ACCESS: query RIMA canon if needed:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

## Amaç
We are building an A/B FEEL comparison: same character on (A) top-down 3/4 vs (B) true isometric. The 3/4 side uses
existing assets. The ISO side is missing art — generate ONLY the two iso pieces below. These are PLACEHOLDER mocks
(replaced by PixelLab later). After generating, append both to STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md.

## STYLE LOCK (HARD — on-brand, NOT realistic)
charcoal / blue-slate / blackened-iron (#1C1D24 -> #2E303F base) + cyan #00FFCC ONLY as a tiny sparing seal accent (<10% area).
Flat painterly / slight pixel-lean. Stone-masonry motif. TRANSPARENT PNG-32 (alpha, NOT magenta-keyed).
HARD-NO: photoreal, gloss/bevel, vector gradient, gold, parchment, neon, baked text, drop-shadow baked in.

## Generate exactly 2 assets -> Assets/Sprites/Environment/IsoMockKit/
1. `iso_floor_diamond.png` — a SINGLE seamless 2:1 ISOMETRIC (dimetric) diamond stone-floor tile. Aspect 2:1 (e.g. 256x128).
   Top face only (flat diamond), subtle stone seams, faint cyan seal rune in one corner (sparing). Must tile edge-to-edge
   as a diamond grid (left/right/top/bottom diamond edges align). Transparent outside the diamond.
2. `iso_wall_block.png` — a SINGLE 2:1 ISOMETRIC stone wall block segment (a cube-ish masonry block in dimetric
   projection: visible top + two side faces, ~1.5x the floor-tile height). Charcoal stone, darker on the shadow side,
   one faint cyan rune line. Transparent background. Pivot/footprint = bottom-center diamond.

Both: clean alpha edges (no white/magenta halo), 2-4x gen then downsample if needed, single object centered on canvas.

## Output
Write files to Assets/Sprites/Environment/IsoMockKit/ (create folder). Set import: Sprite (2D), Point filter, PPU 64,
no compression. Then append 2 lines to STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md (path + "iso A/B mock placeholder -> PixelLab later").
Report the 2 final file paths + a one-line note on tileability. No code beyond import settings.
