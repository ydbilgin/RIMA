---
name: PixelLab Discord analysis pipeline
description: Status and insights from PixelLab Discord export. Export scripts DELETED (account violation). Analysis complete.
type: project
---

## STATUS (2026-05-02): ANALYSIS COMPLETE

Export scripts deleted (export.ps1, run-all.ps1) -- caused Discord account restriction.
3 channels exported before token expired. Analysis done manually + via Codex.
**Insights saved: `STAGING/DISCORD_INSIGHTS_S43.md`** -- read this, not this file, for production guidance.

## Location

Scripts: DELETED (export.ps1, run-all.ps1 removed 2026-05-02)
analyze.py: `F:/Antigravity Projeler/2d roguelite/RIMA/Tools/discord_export/analyze.py` (kept)
Output: `F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/discord_pixellab/`

## Channels (9 total, channels.txt)

| Label | Focus |
|---|---|
| mcp-and-vibe-coding | MCP workflows, vibe coding -- HIGH PRIORITY |
| share-tips-tricks | Tips, prompt techniques |
| pixellab-art-gallery | Sprite outputs, UI screenshots |
| help-questions-support | Problem/solution pairs |
| api-and-sdk | API/MCP updates |
| projects | Workflow showcases |
| tutorials | Step-by-step guides |
| helpful-posts | Curated advice |
| announcements | Staff statements (authority) |

## Models (as of 2026-05-02)

TEXT:    gemma4:26b    (17GB, full model -- analysis, transcript, synthesis)
VISION:  qwen2.5vl:7b  (6GB, multimodal -- frame analysis, screenshots)
FALLBACK: gemma4:e4b   (9.6GB, edge model -- lighter tasks)

**Note:** gemma4:e4b = edge/efficiency variant, NOT the full model. gemma4:26b is significantly
more capable for analysis. User confirmed: always use 26b for text, qwen2.5vl:7b for frames.

## Pipeline steps

```
.\export.ps1 -DaysBack 60          # download JSON + media (--reuse-media, threads included)
python analyze.py --with-images --with-videos   # text + vision + video keyframes
```

## Key fixes applied (2026-05-01)

- `find_context(window=6)` -- was 3, increased for longer conversation threads
- Removed `if x["text"]` filter -- attachment-only posts now show as "(attachment only: file.mp4)"
  so conversation chains are not broken when someone posts media without text

## Re-run behavior

analyze.py skips channels where digest files already exist. To force re-analysis:
delete `STAGING/discord_pixellab/digest/` and re-run.

## YouTube pipeline (2026-05-02 -- IN PROGRESS)

Channel: `https://www.youtube.com/@PixelLab_AI/videos`
Approach: yt-dlp download + ffmpeg frames + gemma4:26b (transcript) + qwen2.5vl:7b (frames).
Gemini quick-scan done: STAGING/YOUTUBE_PIXELLAB_RESEARCH.md (MEDIUM confidence, no live crawl).
Gemma4 deep analysis: STAGING/YOUTUBE_GEMMA4_ANALYSIS.md (running, agent a1fb4af9f6c757384).
Unity closed to free GPU for Gemma4:26b during analysis.
Priority: isometric tile workflow, object/icon pipeline, mob sprites.
