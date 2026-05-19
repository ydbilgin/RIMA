# Codex Task — Wang16 Python Compositor Plan Review + Grid Form Concern

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## Context

You already produced `STAGING/CODEX_DONE_topdown_floor_pipeline_decision.md` verdict (Cool Granite + Worn Stone Path MVP, Tilesetter or manual Aseprite for Wang16 assembly, etc).

After your verdict, user pushed back on Tilesetter ($25 + GUI + 5 min/pair manual click) and asked for full automation. Orchestrator (Opus 4.7) proposed an alternative:

**Custom Python Wang16 compositor** (~200-300 lines, Pillow):
- Inputs: `material_A.png` (32x32), `material_B.png` (32x32), `corner_mask.png` (32x32 alpha), optional `edge_motif.png`
- Algorithm: for each of 16 corner configs (AAAA..BBBB), composite by sampling material_A as base, blending material_B at each "B" corner using corner_mask alpha, optionally stamping edge_motif along A↔B boundary
- Output: 16 PNG cells, ready for Unity slice + RuleTile

User also referenced `tsoding/wang-tiles` (C, procedural fragment-shader Wang generator) — orchestrator dismissed as wrong paradigm (math-art, not pixel-art compositing). Confirm or challenge.

User has two NEW concerns to address:

1. **Direction sanity check** — "are we going the right way? codex + opus collaborate". Honest review of the current plan: PixelLab create_tiles_pro flat bases + custom Python Wang16 compositor + 6-layer Karar #143 overlays. Hidden risks? Better alternative we're not seeing?

2. **Grid-form concern** — User asks: "ihtiyacımız olan direkt 32x32 ya da 64x64 kare tile'lar mı olacak? otomatik onu kare formdan da çıkarmış olacak mıyız?" Literally: "are we stuck on 32x32/64x64 square tile form, or does the approach get us OUT of grid form?"

Address both directly. Use NLM for Karar #143 layering context if needed. Web research OK for industry references (Hades, Colossus, Stardew, etc — how do they break visible grid feel while keeping Tilemap base?).

## Deliverable

`STAGING/CODEX_DONE_wang16_compositor_review.md` with this structure:

```
# VERDICT
[Are we on the right track? PASS / NEEDS_REVISION / WRONG_DIRECTION]

# 1. Python Wang16 compositor — risk analysis
[Hidden risks orchestrator missed. Quality risks. Edge cases. Multi-pair scaling concerns.]

# 2. Alternative paths reconsidered
[Tilesetter vs custom Python vs gpt-image-1 single-shot vs tsoding-style procedural — honest comparison with 2026 RIMA scope]

# 3. Grid form answer
[Honest technical answer: yes/no we stay on square Tilemap. WHY visual feel breaks grid (Karar #143 layering, irregular Wang content, L4 patches, L5 scatter). Or if there's a real escape from grid (free-form sprite composition, paint-buffer approach), discuss.]

# 4. Industry references
[How does Hades / Colossus / Stardew handle the grid-feel-vs-tilemap tradeoff? Concrete technique.]

# 5. Final recommendation
[What should the orchestrator dispatch next? Be specific.]
```

Effort: medium. 30 min ceiling. This is a sanity gate, not deep new research.
