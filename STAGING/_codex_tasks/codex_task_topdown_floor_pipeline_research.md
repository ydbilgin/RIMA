# Codex Task — Top-Down Floor Blending Pipeline Decision

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

---

## Mission

Solve the **top-down 2D pixel-art floor↔floor blending** problem for RIMA. Multiple approaches tried and failed. We need ONE concrete, executable pipeline plan that achieves **Colossus: Eternal Blight floor visual quality** with RIMA's existing assets + tool budget.

Deliverable: `STAGING/CODEX_DONE_topdown_floor_pipeline_decision.md` with the verdict + implementation steps. No code yet — DECISION + PLAN first.

## Reference target (visual benchmark)

Colossus: Eternal Blight floor look:
- Same-elevation grass ↔ stone path with **painterly mossy organic edge wrap** (transition art baked INTO tile)
- Grass interior has **darker green soft patches** (L4 large overlays, not splat noise)
- Cliff/wall = ayrı L3 sprite (vertical 35° Hades style), NOT a floor tile
- 32px chunky pixel feel, NOT 64px smooth painted
- No black tile borders/frames — completely seamless cell-to-cell

Screenshots referenced earlier (user attached in chat):
- Image 1: grass + pond, blob darker-grass patches, water bordered by rocky transition tiles
- Image 2: stone path through grass, painterly mossy edge on each path tile, cliff drop-off on side
- Image 3: boss arena, cobblestone-grass painterly transition, mossy wall border

## Tried and failed approaches

| Approach | What we tried | Why it failed |
|---|---|---|
| PixelLab `create_topdown_tileset` (Wang16) | Generate floor_grass + floor_stone Wang corners | Algorithm assumes material A sits ON TOP of material B → produces elevation/cliff edge, not flat blob blend. User confirmed multiple times "yükseklik gibi oluyor" |
| PixelLab `create_tiles_pro` 64px single material | Granite + grass + dirt + accent base tiles | Painted-realistic 64px doesn't match Colossus chunky 32px; tiles have black borders ("tile" keyword triggers frame); no transition art |
| Splat shader (TerrainBlend) | RGBA splat map, 4-channel material blend, Perlin noise blob borders | Math bug (world-unit basis) fixed but root issue: noise blend = uniform gradient at boundary, NOT the painterly mossy edge wrap Colossus has. Looks "computer blend" not "hand painted" |
| Manual 6-layer Karar #143 | L1 base + L2 variation + L3 wall + L4 patch + L5 scatter + L6 accent | Never executed cleanly — assets generated piecemeal, wrong angles, borders, partial sets |

## Available tools + budget

- **PixelLab subscription**: ~4255/5000 gen remaining (animation reserved 4000, ~255 for map work)
- **PixelLab tools**: `create_tiles_pro` (4-type batch confirmed 25 gen = 16 tiles), `create_topdown_tileset` (Wang16 but elevation issue), `create_object`, `create_map_object`
- **Codex `gpt-image-1`** imagegen (Codex subscription, ~free per call) — supported by hybrid pipeline memory
- **Local GPU**: RTX 5080 — ComfyUI / FLUX dev/schnell / Stable Diffusion with seamless tile LoRAs (zero cost after setup)
- **Tilesetter** ($25 Steam, not yet owned) — manual assembler; takes hand-drawn base tiles, outputs Wang16 sets. Cannot be driven by AI prompts inside.
- **Aseprite** ($20, owned) — manual pixel art editor
- **Unity URP 2D Renderer + Pixel Perfect Camera + 2D Lights** stack live

## Existing RIMA assets you must review

1. `Assets/Art/Tiles/F1/Tilesets/` — **11 existing Wang topdown tilesets** generated pre-S88 via PixelLab MCP. Check:
   - Are they elevation-style (cliff edges) or flat? List each set's actual visual character.
   - RuleTile + sliced PNG per set, ready to use in Unity Tilemap.
   - Memory ref: `memory/project_f1_canonical_tilesets_discovery.md`
2. `Assets/Art/TerrainBlend/TerrainBlend_Mat.mat` + shader — current splat-blend approach, math fix applied (_TerrainTiling=0.5).
3. `Assets/Art/Rooms/Backgrounds/Spawn_01/` — earlier painted experiments, mostly Karar #143 fragment attempts.
4. `Assets/Art/Rooms/AssetPack/FloorStones/` + `LargePatches/` — Alabaster Dawn pilot assets.

## Locked architectural decisions (do NOT propose alternatives)

- 32px tile, 64 PPU, top-down PURE (NOT high/low angle, NOT isometric)
- Unity Tilemap-based composition (not pure SpriteRenderer scatter)
- 6-layer Karar #143 paradigm preserved (your plan must fit this layering)
- Act 1 cool muted palette (Cool Granite #3A3D42, Worn Stone Path #4A4842, Mud Crust #4A3C2A)
- See: `memory/project_alabaster_dawn_pipeline_lock.md`, `memory/project_paint_brush_architecture_lock.md`, `STAGING/ACT_FLOOR_TAXONOMY.md`

## Research questions to answer

1. **How does Colossus actually do it (technical inference)?** Pixel-level analysis of attached screenshots. Wang transition tile art? Per-tile painted mossy edge? Splat? Hand-placed transition sprites? Look at edges between materials in image 2 (stone path↔grass) and tell me literally what asset structure produces that.

2. **Is there a proven free/cheap pipeline that gets RIMA to Colossus floor quality?** Survey:
   - Hand-painted Wang16 transitions in Aseprite + Tilesetter assembly — time cost realistic?
   - Codex `gpt-image-1` for Wang16 transition tile set — prompt-engineering feasibility?
   - Local FLUX/SD + ControlNet tile mode — quality vs PixelLab?
   - Improved splat shader with **painted edge texture** (sample a transition strip texture at boundary) — hybrid approach?

3. **Existing F1 Wang16 tilesets** — are they salvageable? After you list their visual character, decide: usable as-is for floor↔floor, usable only for wall transitions, or scrap?

4. **Concrete 1-week MVP plan** — given Spawn_01 + Combat_Room + Elite_Room first 3 rooms, what's the asset checklist (counts + sources + generation prompts + integration steps)?

## Required output structure for `CODEX_DONE_topdown_floor_pipeline_decision.md`

```
# VERDICT
[2-3 sentence executive decision: what pipeline are we using, why]

# 1. Colossus pipeline reverse-engineered
[Technical analysis of how Colossus achieves the look — be specific about tile structure, transition art, layering]

# 2. F1 existing Wang16 review
[Per-tileset assessment, salvageable / wall-only / scrap]

# 3. Recommended RIMA pipeline
[Step-by-step: which tool produces what asset, integration in Unity, layering per Karar #143]

# 4. 1-week MVP asset checklist
[Concrete list with counts, source tool, prompts where applicable]

# 5. Risks + fallbacks
[What if approach X fails, what's plan B]

# 6. Open questions for user
[Anything you couldn't decide alone]
```

## Effort

`--effort high`. This is a foundational decision — invest reasoning time. Use web research if needed (Colossus dev interviews, indie 2D top-down pipeline articles). Use NLM for RIMA-internal context.

Do NOT write code or generate assets in this task. Decision + plan ONLY. User will review verdict before any execution.
