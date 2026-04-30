# PixelLab n8n Monitoring Plan
Date: 2026-05-01
Owner mark: CODEX_RESEARCH
Status: automation plan, not implemented

## Purpose
User asked whether we can set up n8n to continuously monitor PixelLab-related sources:
- new YouTube videos
- patch notes / changelogs
- Discord posts/messages, using an authorized method
- meaningful images and data together, without separating context from media
- periodic analysis by Gemini/Codex, then handoff to Claude for RIMA design ideas

Short answer: yes, this is feasible. The clean implementation is n8n as scheduler/orchestrator, local storage as archive, and a small analyzer job that turns raw captures into Claude-ready research notes.

## Important Constraint
Discord collection must be authorized.

Allowed approaches:
- Discord bot installed in a server/channel where we have permission.
- Discord webhook / channel export method already approved by the user/server.
- Existing local export path under `Tools/discord_export/` if that is the prior approved workflow.

Avoid:
- user-token scraping
- self-bot automation
- bypassing Discord access controls

If the PixelLab public Discord does not allow our bot, n8n can still monitor YouTube, patch notes, docs pages, X posts, and any manually exported Discord archive.

## Why n8n Fits
n8n is good here because this is mostly event routing:
- Schedule Trigger: check sources every N hours.
- RSS/HTTP nodes: pull YouTube feeds, patch pages, docs pages.
- Discord node or HTTP node: read/send messages with bot/webhook permission.
- Code node: normalize metadata and compute hashes.
- Storage node or filesystem write: save raw item bundle.
- LLM node / HTTP Request: call Gemini/OpenAI/local analyzer.
- Discord/Markdown output: send digest or create a research file.

Official n8n references checked:
- Self-host/queue mode docs: https://docs.n8n.io/hosting/scaling/queue-mode/
- Discord node docs: https://docs.n8n.io/integrations/builtin/app-nodes/n8n-nodes-base.discord/
- Discord credentials docs: https://docs.n8n.io/integrations/builtin/credentials/discord/
- Official hosting examples: https://github.com/n8n-io/n8n-hosting

## Recommended Local Architecture

### Services
Use Docker Compose:
- n8n
- Postgres
- optional Redis/worker later if volume grows

For initial local use, n8n + Postgres is enough.

Do not start queue mode until needed. Official docs note queue mode has extra constraints and is more operationally complex.

### Storage Layout
Use a dedicated archive outside design docs, for example:

```text
STAGING/pixellab_monitor/
  raw/
    youtube/
    patch_notes/
    discord/
    docs/
  media/
    images/
    videos/
    thumbnails/
  normalized/
    items.jsonl
  analysis/
    YYYY-MM-DD_digest.md
```

Rationale:
- Raw capture is preserved.
- Media stays tied to source item through manifest IDs.
- Claude sees curated notes, not raw noise.

## Data Bundle Rule
Never save an image/video alone without its context.

Every collected item should have a manifest:

```json
{
  "source": "youtube|patch_notes|discord|docs|x",
  "source_url": "",
  "source_id": "",
  "author": "",
  "published_at": "",
  "captured_at": "",
  "title": "",
  "text": "",
  "media": [
    {
      "type": "image|video|thumbnail|attachment",
      "url": "",
      "local_path": "",
      "sha256": ""
    }
  ],
  "tags": ["pixellab", "animation", "tiles", "workflow"],
  "hash": "",
  "analysis_status": "new|queued|done|ignored"
}
```

This prevents the common failure mode where a useful screenshot gets separated from the post that explains it.

## Workflow 1 - YouTube Monitor
Trigger:
- Schedule every 6 or 12 hours.

Input:
- PixelLab official YouTube channel RSS if available.
- Any known PixelLab tutorial channels.

Process:
1. Read RSS feed.
2. Filter unseen video IDs.
3. Store title, description, URL, thumbnail.
4. Optional: fetch transcript if available.
5. Queue for analysis if title/description matches:
   - PixelLab
   - animation
   - spritesheet
   - character
   - tileset
   - map object
   - interpolate
   - keyframe

Output:
- `normalized/items.jsonl`
- daily digest entry

## Workflow 2 - Patch Notes / Docs Monitor
Trigger:
- Schedule every 12 or 24 hours.

Input:
- PixelLab docs/changelog URL if known.
- MCP docs page if relevant.
- Blog/release pages.

Process:
1. Fetch page.
2. Extract meaningful content.
3. Hash normalized text.
4. If hash changed, save before/after snapshot.
5. Queue for analysis.

Output:
- "changed / unchanged" event.
- diff summary.

Why important:
PixelLab features can affect our production rules. Example: if MCP adds first/last frame params or keyframe control, it would change RIMA animation workflow.

## Workflow 3 - Discord Monitor
Trigger:
- Schedule every 4-12 hours or webhook event if bot receives messages.

Allowed input options:
- Discord bot reads specific allowed channels.
- Webhook receives mirrored posts.
- Existing export folder is periodically scanned.

Process:
1. Pull only selected channels, not whole server noise.
2. Filter by keywords:
   - update
   - release
   - animation
   - character
   - sprite
   - keyframe
   - interpolate
   - tile
   - map object
   - MCP
   - API
   - cost
   - generation
3. Save message, author, timestamp, attachments, embeds.
4. Save attachment files with hash.
5. Keep thread/reply parent context where available.
6. Queue relevant bundle for analysis.

Output:
- raw message bundles
- digest summary
- possible action items for RIMA

## Workflow 4 - Analysis Queue
Trigger:
- New item bundle or daily digest schedule.

Analyzer options:
- Gemini 3.1 Pro Preview via CLI/API for broad external summary.
- Codex/local analyzer for RIMA-specific production implications.
- Claude/orchestrator reads final digest and decides whether to persist into GDD/MASTER/CURRENT_STATUS.

Recommended output format:

```text
# PixelLab Monitor Digest - YYYY-MM-DD

## High Signal
- item -> why it matters -> confidence

## RIMA Impact
- animation pipeline
- sprite import / QC
- map object / tile production
- budget/cost
- possible design ideas

## Action Candidates
- immediate
- later
- ignore

## Source Bundles
- source_url
- local manifest path
```

## Claude Handoff Rule
The automation should not edit locked design files directly.

Allowed automated writes:
- `STAGING/pixellab_monitor/analysis/*.md`
- optionally a single inbox file such as `STAGING/pixellab_monitor/CLAUDE_INBOX.md`

Claude/orchestrator later decides whether to move insights into:
- CURRENT_STATUS.md
- MEMORY/
- TASARIM/ roadmap docs
- MASTER_KARAR_BELGESI.md

## RIMA-Specific Signals To Watch
Highest priority:
- first/last frame support in MCP/API
- keyframe params
- custom animation cost changes
- 128px animation frame count changes
- direction generation consistency
- object/tileset style matching improvements
- transparent background/object mask improvements
- PixelLab prompt recommendations
- new export/download formats

Medium priority:
- new UI-only features that still affect manual workflow
- community examples of spritesheets
- cost/budget reports
- animation interpolation examples

Low priority:
- generic AI art showcases without pipeline details
- marketing posts without workflow details

## Practical Setup Steps
1. Decide hosting:
   - local Docker Compose on this machine
   - small VPS
   - always-on home server

2. Create secrets:
   - n8n encryption key
   - Discord bot token or approved export path
   - Gemini/OpenAI key if analyzer runs automatically

3. Create source registry:
   - PixelLab YouTube channel IDs
   - PixelLab docs/changelog URLs
   - Discord channel IDs or export folder path
   - optional X accounts to monitor through approved/public methods

4. Build workflows:
   - YouTube RSS monitor
   - patch/docs diff monitor
   - Discord authorized monitor
   - analyzer/digest workflow

5. Test with dry-run:
   - capture 5 items
   - verify media + metadata stay linked
   - verify duplicate detection works
   - verify digest is concise enough for Claude

6. Only then enable schedule.

## Minimal First Version
Start small:
- YouTube RSS monitor
- PixelLab docs/patch page monitor
- scan existing `Tools/discord_export/` if present
- daily Markdown digest

Skip automatic LLM analysis at first. Once the archive/dedupe is stable, add Gemini/Codex analysis.

## Recommendation
Yes, build this, but as a research inbox, not an automatic design editor.

Best first milestone:
"PixelLab Monitor v0" that creates one daily digest with linked source bundles.

Definition of done:
- New YouTube/patch/docs/exported Discord items are detected.
- Attachments and text are stored together.
- Duplicate items are ignored.
- Daily digest contains only high-signal items.
- Claude gets action candidates, not raw logs.

