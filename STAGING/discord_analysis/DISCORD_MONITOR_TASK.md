# Discord Monitor Task — PixelLab

Read this file, execute the task, update the Last Run dates at the bottom when done.

## What you are doing
**Role: Scout / Indexer (NOT final analyzer)**
Monitoring PixelLab Discord channels to index *what* is being discussed and by *whom*, rather than doing deep technical analysis.
Context: 2D roguelite game, isometric, 128px characters, 4-directional sprites, Unity URP.
Goal: Map out conversations regarding animation techniques, API updates, MCP workflows, prompt tips, style consistency methods. 
*Important:* If a critical image/prompt/diagram is seen but cannot be fully read, do NOT guess. Flag it and ask the user: "Please send me the image/text from [Date/Time] for detailed analysis." The final engineering synthesis will be done by Claude/Codex later based on this index.

## Rules
- Read-only. No messages, reactions, uploads, downloads.
- No token, cookie, localStorage, or Discord API access.
- Use the Chrome session the user already has open.
- Scroll slowly, wait 2-5s after each scroll for content to load.
- If CAPTCHA or security warning appears: stop immediately and report.
- Human-paced movement only.

## Channels (priority order)
1. #mcp-and-vibe-coding
2. #api-and-sdk
3. #share-your-tips-and-tricks
4. #announcements
5. #help-questions-support
6. #pixellab-art-gallery

## Per channel: scroll back only to the Last Run date listed below.
If Last Run is empty, go back as far as possible (60+ days).

## Media handling
- Images: analyze what is visible on screen.
- Videos: open in player, pause at 0% / 25% / 50% / 75% / 100%, take frame screenshot.
- No bulk download.

## Output per channel
STAGING/discord_analysis/<channel-name>/
  screenshots/001.png, 002.png ...
  frames/001.png ...
  analysis.md  (append new findings, do not overwrite old ones)

analysis.md entry format:
### [YYYY-MM-DD] Conversation Topic
- Participants: User X, User Y
- Type: image / video / text
- Summary: Brief summary of what is being discussed (e.g., "Discussing async plugin issues").
- Flag for User: [None] OR [Requires manual fetch: Critical prompt/image is unreadable]
- File: screenshots/001.png

## Final step
After all channels: update STAGING/discord_analysis/SYNTHESIS.md with new findings.
Then update the Last Run dates below and save this file.

---

## Last Run Log (update after each session)

| Channel | Last Run | Scrolled back to |
|---|---|---|
| #mcp-and-vibe-coding | 2026-05-02 | 2026-03-27 |
| #api-and-sdk | 2026-05-02 | 2026-03-18 |
| #share-your-tips-and-tricks | 2026-05-02 | 2026-02-04 |
| #announcements | 2026-05-02 | 2025-11-08 |
| #help-questions-support | Skipped (Forum Channel) | — |
| #pixellab-art-gallery | 2026-05-02 | 2026-04-25 |
