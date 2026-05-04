---
name: PixelLab Discord monitoring channels
description: Which Discord channels to screenshot for ongoing PixelLab research and technique discovery
type: reference
---

## Screenshot Priority (highest to lowest)

| # | Channel | Why |
|---|---|---|
| 1 | #mcp-and-vibe-coding | MCP workflows, Claude Code integration -- directly relevant |
| 2 | #api-and-sdk | New API features, endpoint changes, MCP updates |
| 3 | #share-your-tips-and-tricks | Community techniques, prompt tips, workflow discoveries |
| 4 | #announcements | Staff feature releases, roadmap hints |
| 5 | #help-questions-support | Problem/solution pairs, workarounds for known issues |
| 6 | #pixellab-art-gallery | Output quality reference, style inspiration |

## Workflow (current -- Antigravity automated)
Antigravity reads STAGING/discord_analysis/DISCORD_MONITOR_TASK.md and runs the task.
- Output: STAGING/discord_analysis/<channel-name>/analysis.md + screenshots/
- Synthesis: STAGING/discord_analysis/SYNTHESIS.md
- Last run dates tracked in DISCORD_MONITOR_TASK.md (Antigravity updates after each session)
- Automation note: PyAutoGUI + real Chrome (safe) or Playwright + fake account. See MEMORY/discord_automation_risk.md.

## Cadence suggestion
- #mcp-and-vibe-coding + #api-and-sdk: weekly (fast-moving)
- #share-your-tips-and-tricks: bi-weekly
- #announcements: whenever user notices new post

## To run
Tell Antigravity: "F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/discord_analysis/DISCORD_MONITOR_TASK.md dosyasini oku ve gorevi uygula."

## Staff to watch (verified MEGA/mod badges)
- Kaninen -- most active, insider knowledge, answers technical questions
- NikolaiPatricioStar (PHSR) -- shares workflow tips with before/after
- xjon -- quick answers on prompting
- YumYum -- item/icon/UI specialist