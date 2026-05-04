---
name: Discord automation risk and approach
description: How to safely monitor Discord without bot detection. Playwright is risky; PyAutoGUI + real Chrome is safe.
type: reference
---

## Automation approaches ranked by safety

| Method | Risk | Why |
|---|---|---|
| PyAutoGUI + real Chrome | Low | OS-level mouse/keyboard, no webdriver flag, Discord sees normal Chrome |
| Playwright with main account | HIGH | navigator.webdriver=true, browser fingerprint detected, ban risk |
| Playwright with fake account | Medium | Detected as bot but account is expendable |
| Discord API / selfbot | Very HIGH | ToS violation, immediate ban |

## Safe workflow (main account)
- User opens Chrome, navigates to Discord channel manually
- PyAutoGUI script controls mouse/keyboard at OS level
- Script scrolls slowly, takes screenshots via OS screengrab
- No token, no API, no browser automation flags

## Task file
Recurring task instructions: STAGING/discord_analysis/DISCORD_MONITOR_TASK.md
- Contains channel list, output format, last run dates
- Antigravity reads this file each session, updates Last Run Log when done
- Channels: #mcp-and-vibe-coding, #api-and-sdk, #share-your-tips-and-tricks,
  #announcements, #help-questions-support, #pixellab-art-gallery

## Output
Per-channel: STAGING/discord_analysis/<channel>/analysis.md + screenshots/
Synthesis: STAGING/discord_analysis/SYNTHESIS.md
