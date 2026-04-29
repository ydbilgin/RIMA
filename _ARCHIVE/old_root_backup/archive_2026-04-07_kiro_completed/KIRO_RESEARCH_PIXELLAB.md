# KIRO TASK — KIRO_RESEARCH_PIXELLAB
*Date: 2026-04-06 | Research only — no file edits, no Unity, no PixelLab API calls*

---

## RISK LEVEL: LOW
> Deterministic · Mechanical · Isolated · Bounded · Mechanically verifiable ✓

---

## CONTEXT

Claude Code needs current, accurate information about PixelLab's Aseprite extension and website tools before writing a beginner guide for the user. Do NOT use PixelLab API. Do NOT generate any images. This is a web research task only.

---

## FILES TOUCHED

- `Assets/_STAGING/PIXELLAB_RESEARCH.md` (create this file with your findings)

Do not touch any other file.

---

## TASK — Fetch and summarize the following pages

Fetch each URL below. Read the full content. Write your findings to `Assets/_STAGING/PIXELLAB_RESEARCH.md`.

### URLs to fetch (in order):

1. `https://pixellab.ai` — main site, note what tools/products are listed
2. `https://pixellab.ai/aseprite` — Aseprite extension page (if exists)
3. `https://pixellab.ai/docs` — documentation (if exists)
4. `https://pixellab.ai/download` — download page (if exists)
5. `https://pixellab.ai/pricing` — pricing and subscription tiers
6. Search for: "PixelLab Aseprite extension tutorial 2024 2025" — note any relevant results

### For each page, record:

- Exact URL fetched
- What the page says about the Aseprite extension
- Installation steps (exact, if listed)
- Features listed
- Any limitations mentioned
- Pricing/subscription requirements

---

## OUTPUT FORMAT

Write to `Assets/_STAGING/PIXELLAB_RESEARCH.md`:

```markdown
# PixelLab Research Results
*Date: 2026-04-06 | Fetched by Kiro*

## Source: [URL]
[Full relevant content from that page]

## Source: [URL]
[Full relevant content from that page]

## Summary
[What the Aseprite extension does, how to install, workflow, limitations]
```

---

## COMPLETION LOG

Append to `Assets/_STAGING/DONE.txt`:

```
[DONE-KIRO_RESEARCH_PIXELLAB] Web research complete — PIXELLAB_RESEARCH.md created | 2026-04-06
```

---

## AFTER KIRO FINISHES — Claude Code handles

When user says "research hazır", Claude Code will:
1. Read `Assets/_STAGING/PIXELLAB_RESEARCH.md`
2. Write a beginner guide for the user based on current accurate information
