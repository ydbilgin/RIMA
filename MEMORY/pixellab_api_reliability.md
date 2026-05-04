---
name: PixelLab API reliability rules
description: API bugs, error handling, retry rules, and known production issues for PixelLab V2
type: feedback
---

## ALWAYS Use V2 API
- V2: `https://api.pixellab.ai/v2/docs` -- has all Pro tools
- V1: deprecated, missing most tools. Never use.
- LLM context file for agents: `https://api.pixellab.ai/v2/llms.txt`

## Rate Limiting
- 2-second minimum delay between API calls (Kaninen staff, HIGH confidence)
- Violating this causes 429 errors

## Polling Timeout: 10 Minutes Minimum
- animate-with-text-v3 at 256x256 max frames: 2-3 minutes normally, up to 8 minutes under load
- Any automation script must set polling timeout >= 10 minutes
- Standard 3-minute kill timeouts will cause false failures

## Known Bugs (as of May 2026)
| Bug | Workaround |
|---|---|
| animate-v2 stalls at 50% (90% failure rate) | Use v3 only |
| rotation_urls returns null on object completions | Download via web UI manually |
| Gray background on animate-v3 output | Post-process to remove; retry |
| Jobs stuck at "initializing" (no logs) | Email PixelLab support with job ID |
| API-generated images NOT in web gallery | Store output images/URLs locally in pipeline |

## Transient Errors (retry, do NOT discard)
- `BackgroundWebSocket.receive_json() called but no request_data...` = server-side transient, retry
- Credits are NOT consumed on failed jobs (usage.usd = 0)
- Worker shortage incidents resolve within 1-3 hours

## Custom Proportions Bounds
- proportion multipliers (arms_length, head_size, etc): range 0.5 to 2.0
- Outside this range = 422 Unprocessable Entity
- head_size recommended max: 1.7 (not 2.0)

## Canvas Auto-Expansion
- 256x256 output = ~168x168 actual character + padding for animation room
- Account for this when sizing characters for specific pixel targets

## force_colors Parameter
- `color_image` must be PNG-encoded Base64 (patched to accept both PNG and raw RGBA)
- Include palette slot meanings and skin color rules in the description text

## API-Generated Images
- NOT visible in web UI gallery -- API-only access
- Pipeline must save output locally or preserve CDN URLs from response
