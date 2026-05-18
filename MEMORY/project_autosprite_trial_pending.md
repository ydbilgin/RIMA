---
name: autosprite-trial-pending
description: autosprite.io MCP registered, free plan VFX pilot pending; PixelLab production primary, autosprite nis VFX trial only
metadata:
  type: project
---

# Autosprite Trial Pending

## MCP Registration (2026-05-18 S89 LATE)

- `claude mcp add autosprite` LIVE
- Transport: HTTP, Bearer token format: `vspk_a1735a0dd9_...`
- Status: Connected OK (confirmed same session)
- Tool prefix: `mcp__autosprite__*`
- **PREREQUISITE:** Claude Code restart required before tools load in new session

## Yeni Session Dogrulama

Restart sonrasi ilk is: `ToolSearch "+autosprite"` ile tool schema'lar yuklu mu kontrol et.
Yoksa: `claude mcp list` ile kayitli mi bak, gerekirse re-add.

## VFX Pilot Candidates

| # | Candidate | Spec | Oncelik |
|---|---|---|---|
| 1 | dash trail | cold blue, non-directional, 64x64 8-frame loop | PILOT |
| 2 | hitspark | white→class-color, contact burst | PILOT |
| 3 | rift portal | purple/void energy loop | backlog |
| 4 | aura/buff loop | on-character glow ring | backlog |
| 5 | coin pickup | golden burst | backlog |

Pilot ilk 2: dash trail + hitspark. Production'a girmeden once kalite + cost A/B gerek.

## Dispatch

- Agent: rima-asset
- Ilk task: dash trail VFX (cold blue, non-directional, 64x64 8-frame loop) via `mcp__autosprite__*`

## Ilgili

- [[autosprite-vs-pixellab-verdict]] -- RIMA production karari
- `STAGING/autosprite_vs_pixellab_review.md` -- community sentiment detay (May 2026)
