# Codex Task: Fetch PixelLab v2 REST API Docs → STAGING

## Context

PixelLab Discord cevabı (2026-05-18 S92): "you can use [Create Image Pro + V3 character animation] from https://api.pixellab.ai/v2/docs for now but we should probably look into adding them to the mcp as well".

User wants the full REST API documentation pulled locally so it can be synced to NotebookLM (NLM) for queryable reference.

**NLM notebook ID:** `30ddffa5-292f-4248-8e77-68074af901be` (RIMA design knowledge base). After Codex fetches docs, orchestrator will sync via `/nlm-sync`.

## Görev

1. **Fetch** `https://api.pixellab.ai/v2/docs` (likely Swagger/OpenAPI HTML or JSON spec).
2. If HTML: parse the structured content (endpoints, request/response schemas, examples).
3. If JSON/YAML OpenAPI spec: pretty-print, structure into sections.
4. **Write** the result to `STAGING/PIXELLAB_API_V2_REFERENCE.md` (markdown) with these sections:
   - `## Overview` — base URL, auth, common headers
   - `## Endpoints` — grouped by resource (characters, images, animations, etc.)
     - For each endpoint: method, path, params, request body schema, response schema, example
   - `## Authentication` — how API keys work
   - `## Rate limits / pricing` — if documented
   - `## RIMA-relevant endpoints` (Codex picks): Create Image Pro equivalent, V3 character animation endpoints, character/object create + status polling
   - `## Quick examples` — curl + Python snippets for the 3-5 most-used endpoints

5. Also dump raw OpenAPI spec to `STAGING/PIXELLAB_API_V2_OPENAPI.json` (if available — for tooling).

## Discovery hints (if endpoint is unclear)

- The docs page might redirect to `/v2/docs/swagger` or `/v2/openapi.json` — try common patterns.
- If site is behind auth: try public endpoints first. If totally locked, write a placeholder note "Auth wall, manual fetch needed" and return what's reachable.
- PixelLab Python SDK might document endpoints: https://github.com/pixellab-code/pixellab-python (use as cross-ref if reachable).
- Discord message confirms Create Image Pro + V3 anim are REACHABLE on this REST surface.

## Hard limits

- Read-only fetch + write to STAGING/. NO code changes elsewhere.
- Markdown clean, no broken links, ASCII tables for parameter lists.
- If response >50KB, split into `STAGING/PIXELLAB_API_V2_REFERENCE.md` (curated) + `STAGING/PIXELLAB_API_V2_RAW.md` (full dump).
- 10-min effort cap. If site is unreachable or blocked, write what you have + note the gap.

## Output

Write `CODEX_DONE_pixellab_v2_docs_fetch.md` with:
- Status: SUCCESS / PARTIAL / FAILED
- Files written (paths)
- Endpoint count discovered
- 3-5 most RIMA-relevant endpoints highlighted (Create Image Pro, V3 animate, ...)
- Any auth gaps / blocked endpoints noted

Orchestrator will then dispatch `/nlm-sync STAGING/PIXELLAB_API_V2_REFERENCE.md` so this becomes queryable via NLM.
