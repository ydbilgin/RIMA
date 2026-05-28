# PixelLab Wall Asset Planning (Plan-Only — No Generation Yet)

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NLM ACCESS:** If you need RIMA design context, query NLM first via:
`uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

ALWAYS WRITE YOUR RESULT TO `STAGING/pixellab_wall_plan.md` AS THE FINAL STEP.

---

## Goal

Plan a **PixelLab wall asset generation strategy** that replaces the current gpt-image-1 placeholder wall textures with proper PixelLab-anchored art. User wants ChatGPT_ref aesthetic, locked to a chosen init image for style consistency. **DO NOT generate anything in this task — only produce the plan document.** Claude (orchestrator) reviews with user, then executes via PixelLab MCP.

---

## STEP 0 — Required reads

1. `STAGING/concepts/chatgpt_ref/` — list all 8 reference PNGs, identify which 2 best capture the **wall aesthetic** (carved stone, niches, runic accents, weathering)
2. `CURRENT_STATUS.md` — confirm S103 HD-2D state
3. `MEMORY/INDEX.md` and any `pixellab_pipeline*.md` you can find — note the PixelLab production pipeline rules
4. `Assets/Prefabs/Environment/WallSegment_*.prefab` — list existing wall prefab types (Straight, Cracked, Niche, NE_OuterCorner, NW_InnerCorner, Breach[Phase3], Toppled[Phase3], Heavy[Phase3])
5. `Assets/Materials/Environment/Wall*.mat` — list current materials and which textures they reference

---

## STEP 1 — Survey PixelLab MCP capabilities (no MCP calls)

The available PixelLab MCP tools (catalog only; do not invoke):

| Tool | Purpose | Init image support |
|---|---|---|
| `mcp__pixellab__create_isometric_tile` | Single isometric tile | yes |
| `mcp__pixellab__create_topdown_tileset` | Full top-down tileset with neighbors | yes |
| `mcp__pixellab__create_sidescroller_tileset` | Side-view tileset | yes |
| `mcp__pixellab__create_tiles_pro` | Modular tile variations (pro tier) | yes |
| `mcp__pixellab__create_map_object` | Single map object sprite | yes |
| `mcp__pixellab__create_1_direction_object` | Object, one facing | yes |
| `mcp__pixellab__create_8_direction_object` | Object, 8 facings | yes |
| `mcp__pixellab__create_character_state` | Character pose | yes |
| `mcp__pixellab__animate_character` / `_object` | Animation frames | n/a |

**Key consideration:** wall pieces in HD-2D Hybrid are used as **textures on 3D meshes** (not 2D sprites in scene). The art needs to be flat, tileable, look correct viewed at 35° pitch / -45° yaw orthographic camera. Final delivery: PNG textures usable on URP Lit `_BaseMap` slot.

For this purpose, ranked options:
- `create_tiles_pro` — best for modular variations of the same surface type
- `create_sidescroller_tileset` — good for ELEVATIONAL wall slices (face on, like Stardew/Eastward wall art)
- `create_topdown_tileset` — for FLOORS (separate phase), less ideal for walls
- `create_map_object` — for single hero pieces (e.g., a unique pillar, an arch)

---

## STEP 2 — Decide the production matrix

In `STAGING/pixellab_wall_plan.md` produce a table like:

| Wall prefab | PixelLab tool | Init image | Prompt sketch | Output count | Material binding |
|---|---|---|---|---|---|
| WallSegment_Straight | create_tiles_pro | chatgpt_ref/X.png | "weathered dark slate stone wall section, runic accent..." | 1 base + 3 variants | WallMat_StoneA |
| WallSegment_Cracked | (same as above pro batch) | same | "with cracks, exposed brick, damaged" | included in batch | WallMat_StoneB |
| WallSegment_Niche | create_map_object | chatgpt_ref/Y.png | "dark recessed arched niche in carved stone wall" | 1 + 2 variants | WallMat_NicheD |
| WallSegment_Pillar | create_map_object | chatgpt_ref/X.png | "tall fluted stone pillar with runic carving" | 1 | WallMat_PillarC |
| WallSegment_NE_OuterCorner | create_map_object | same | "stone wall outer corner stub" | 1 | WallMat_StoneA |
| WallSegment_NW_InnerCorner | create_map_object | same | "stone wall L-shape inner corner block" | 1 | WallMat_StoneA |
| WallSegment_Breach | create_map_object | same | "collapsed wall section with rubble pile at base" | 1 | (new) WallMat_Breach |
| WallSegment_Toppled | create_map_object | same | "half-height fallen wall remnant" | 1 | (new) WallMat_Toppled |
| WallSegment_Heavy | create_tiles_pro batch | same | "imposing tall fortified wall" | included | (new) WallMat_Heavy |

Rationale for tool choice goes in a small "Why" column.

---

## STEP 3 — Init image strategy

For each batch, recommend ONE init image from `STAGING/concepts/chatgpt_ref/`. Reason: PixelLab anchors hue, lighting direction, and pixel density to the init. Different batches can use different inits if they target different visual moods. List recommended init paths explicitly.

Constraints to honor:
- Pixel art, hard edges, min 4px clusters, no anti-aliasing
- HD-2D 35° pitch ARPG perspective
- Dark slate palette (#1A1C20 / #2A2D34 / #3A3D48 / #4E5260)
- Cold blue accent #7BA7BC for runic glow
- Norse-inspired runic carvings
- Weathered, moss/damp at base
- No characters, no enemies, no clutter — surface art only

---

## STEP 4 — Prompt sketches

Provide a draft prompt for EACH of the 9 wall prefabs (or grouped batches if `create_tiles_pro` covers multiple). Each prompt ≤ 80 words. Format:

```
[Prefab name]
Tool: <PixelLab tool>
Init: <relative path>
Resolution target: <e.g., 256x512>
Prompt: <one paragraph, 60-80 words>
```

Example skeleton for Straight:
```
WallSegment_Straight
Tool: mcp__pixellab__create_tiles_pro
Init: STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (5).png
Resolution: 256x512
Prompt: Top-down ARPG perspective pixel art of a weathered dungeon stone wall section. Dark slate masonry (#2A2D34, #3A3D48, #1A1C20), irregular block sizes, faint Norse runic accents carved on a few blocks, moss and dampness at base, hairline cracks scattered. Tileable horizontally for grid placement. Hard pixel edges, 4px minimum clusters, no anti-aliasing. Style anchor: Octopath Traveler HD-2D dungeon walls.
```

---

## STEP 5 — Validation rules (post-generation)

State the QC checks Claude will run after each PixelLab output before applying to materials:
1. Resolution matches target (within 10%)
2. No anti-aliasing visible (random 5 pixel-edge sample check, sharp transitions)
3. Hue inside dark slate palette band (eyeball)
4. Tileability: edges visually continuous (if applicable for create_tiles_pro outputs)
5. No characters / enemies / props bleed in
6. Style continuous with init image (composition + palette)
If failed: regenerate up to 2 retries per asset. After 3 fails: BLOCKED, escalate.

---

## STEP 6 — Output mapping

For each prefab, state EXACTLY:
- PNG output path (e.g., `Assets/Art/Environment/Walls/pixellab_stone_wall_a.png`)
- Texture import settings (Filter=Point, sRGB=on, MaxSize=512, WrapMode=Repeat)
- Material binding (existing `WallMat_StoneA.mat` `_BaseMap` slot)
- Backup of old texture (move existing gpt-image-1 outputs to `Assets/Art/Environment/Walls/_gpt-image-1_backup/`)

---

## STEP 7 — Cost estimate

For each batch, list:
- Number of PixelLab generations
- Expected total credit consumption (consult `MEMORY/` or `STAGING/` for any PixelLab cost notes; otherwise mark "TBD — pre-generation balance check")

If `mcp__pixellab__get_balance` results are available in any past CODEX_DONE.md or notes, reference them. Otherwise note that Claude should call this MCP before generation.

---

## STEP 8 — Write the plan document

Output everything above to `STAGING/pixellab_wall_plan.md` with sections:
1. Goal + reference selection (chosen 2 init images + why)
2. Production matrix table (STEP 2)
3. Init image strategy (STEP 3)
4. Prompt sketches (STEP 4)
5. Validation rules (STEP 5)
6. Output mapping (STEP 6)
7. Cost estimate (STEP 7)
8. Recommended dispatch order (which prefab first, why)
9. Open questions for user (3-5 max)

---

## STEP 9 — Report

Write to `CODEX_DONE.md`:
- STATUS: DONE / FAILED / PARTIAL
- PLAN_DOC: STAGING/pixellab_wall_plan.md
- TOOLS_RECOMMENDED: list
- INIT_IMAGES_RECOMMENDED: 1-3 paths
- PREFABS_COVERED: count
- OPEN_QUESTIONS: count
- NEXT_SIGNAL: "pixellab_wall_plan_ready"

---

## Constraints

- **Plan-only.** Do NOT call any PixelLab MCP tool. Do NOT generate any PNG. Do NOT modify any material or prefab.
- Allowed file writes: `STAGING/pixellab_wall_plan.md` + `CODEX_DONE.md` only.
- Do NOT commit — Claude orchestrator will commit after plan review.
- If `STAGING/concepts/chatgpt_ref/` is empty or missing: BLOCKED.
- If PixelLab MCP catalog is unavailable: list the assumed tool set from this brief and mark "unverified — Claude to confirm at execution time".
- If any prefab in the matrix lacks a clean PixelLab tool match: flag in open questions, propose fallback (e.g., gpt-image-1 retain).
- STOP after STEP 9.
