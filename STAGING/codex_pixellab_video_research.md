# Codex Research — PixelLab New Feature (Video Analysis)
**Date:** 2026-05-16 S86
**Workflow:** Codex web research → analysis report. Parallel with Gemini analysis.

## Video URL
`https://youtu.be/oCJWxfEwX-o`

## Task

PixelLab just released a new feature (this YouTube video). Research what it is using shell tools available to Codex:
- `curl https://www.youtube.com/oembed?url=https://youtu.be/oCJWxfEwX-o&format=json` for metadata
- Optionally `yt-dlp --skip-download --write-info-json https://youtu.be/oCJWxfEwX-o` if yt-dlp available
- Check `pixellab.ai` blog / changelog / Discord announcements via curl + grep
- Search Twitter/X via Nitter mirror if available
- Reddit r/PixelLab or r/gamedev recent posts

**Context — RIMA pipeline (do not re-explain back to me, just integrate):**
- 64×64 native chibi sprite, 10 character classes, 8-direction (5 produce + 3 mirror)
- Weapon-less production (Karar #144), weapons attached as Unity child SpriteRenderer
- Existing pipeline: PixelLab Web UI V3 + Create Image Pro + reference image at 30-35° low top-down
- Production prompts hand-crafted in `STAGING/character_production_prompts.md`
- Animation: Idle 4f / Run 6f / Attack 3-seg / Dash 3f / Hit 3f / Death 6-8f
- Active sprint: Sprint 11 closed, Sprint 12 (Props Mode) next

## Required output

Write to `STAGING/pixellab_new_feature_analysis_CODEX.md`:

1. **Feature name + 2-3 sentence summary**
2. **Workflow demonstrated** — step-by-step bullet list
3. **Inputs / outputs / formats**
4. **Cost / credits per gen** (if known)
5. **Pipeline fit** — does this REPLACE or AUGMENT existing RIMA pipeline?
   - Cross-check: Create Image Pro / reference image / 8-dir mirror / weapon-less / animation flow
6. **Speed advantage** — if known
7. **64×64 chibi compatibility** — works at target res?
8. **Limitations**
9. **Top 3 RIMA-actionable items** — integrate / defer / skip

## Constraints
- DO NOT modify any RIMA source files
- DO NOT edit memory directly (orchestrator will consolidate)
- Output ONLY the analysis MD + brief STATUS in CODEX_DONE_*.md

Effort: high.
