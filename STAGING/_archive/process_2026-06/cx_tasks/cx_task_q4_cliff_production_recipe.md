# CX TASK — Q4 HYBRID cliff: concrete PRODUCTION RECIPE (design/plan, no code/asset changes)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: query NLM if needed: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<q>"
Direct-read only: code / STAGING / memory / STAGING/PIXELLAB_KNOWLEDGE_BASE.md.

## Amaç (purpose)
We DECIDED the hybrid cliff (you + Gemini + orchestrator all converged): procedural MESH underlay + hand-painted rock-face TEXTURE on the mesh + hand-painted OVERLAY sprites along the boundary loop. Now give the concrete PRODUCTION RECIPE before we generate anything. PLAN ONLY — no code/asset/material/scene changes.

## Decided design
- Mesh = `Assets/Scripts/Environment/CliffMeshGenerator.cs` (continuity, inner holes, flush top, occlusion, void gradient). Turn the procedural STRATA OFF (it's the artificial culprit).
- Mesh face = a HAND-PAINTED rock-face TEXTURE on `_MainTex` (currently UNASSIGNED in `Assets/Materials/CliffVoidFade_Mesh.mat`; shader `Assets/Shaders/CliffVoidFade.shader` samples `_MainTex` at ~line 157 but it's null). UVs: u = perimeter distance / tileWorldLength, v = depth (top=1 → bottom=0).
- Overlay = hand-painted SPRITES (teeth/chunks/spires/cyan accents) spawned deterministically along the same boundary loop, jittered/clustered, breaking the bottom silhouette.

## Constraints
- On-brand "Shattered Keep": dark slate / iron grey + SPARING cyan #00FFCC + purple void. Must match floor = PixelLab `floor451` (clean 8-color pixel-art slate granite). Ref: `Assets/Sprites/Environment/PixelLabFloor451/floor451_0.png`.
- High top-down 3/4 iso, PPU 64. NATURAL: vertical/diagonal fractures, NO horizontal terraces, broken/irregular, hand-painted feel.
- Tool facts: imagegen (Imagen via ax) = **1024x1024 OPAQUE only** (no transparency/size/seamless control). PixelLab = clean pixel-art, transparent, tileable: `create_tiles_pro` (iso, style_images, seamless batch), `create_map_object` (transparent objects), `create_image_pro` (WEB-ONLY, not MCP). `Assets/Sprites/Environment/CliffKit_RefB/` = existing 8-dir transparent stone-spire sprites + corners + cyan_glow (the art the user found natural).

## Deliver the concrete PRODUCTION RECIPE (CODEX_DONE.md) — focus IMPLEMENTATION/PIPELINE
1. **Mesh `_MainTex` rock-face texture:** which tool (PixelLab `create_tiles_pro` vs imagegen)? exact spec: dimensions (e.g. 512x256? must it be POT/tileable?), HORIZONTAL seamlessness strategy for tiling along perimeter u, how to avoid visible repetition (variants? U offset per loop? macro noise?), opaque vs RGBA. Unity import settings (wrap mode Repeat, filter Point for pixel-art, PPU). Exact prompt text.
2. **Overlay sprites:** reuse `CliffKit_RefB` directly (already transparent) or produce new? If new, which tool + transparency method (PixelLab transparent vs imagegen-magenta-key)? Sizes/pivots. What 3-5 overlay modules (short tooth / long tooth / cracked slab / corner cluster / cyan fissure)?
3. **Overlay-spawn system feasibility:** how to spawn overlays from `CliffMeshGenerator`'s boundary loop deterministically (spacing, clustering, jitter, sorting just above the mesh, breaking bottom not top). Rough code shape + effort.
4. **Sequence:** what to produce/build first, palette-match steps to floor451.
Be concrete (dims, prompts, tool calls, import settings). No production. BLOCKED + reason if files unreadable.
