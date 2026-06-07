# RIMA — DEEP WEB RESEARCH (ax Gemini 3.1 Pro High): PixelLab latest capabilities + community practice + OUR pipeline gaps

You are a deep web-research + inference advisor. GOAL: research the CURRENT/LATEST state of PixelLab (tools, API, features, limits) AND how the wider community produces game weapon/sprite/item assets with PixelLab, PULL FROM THE INTERNET, then compare against RIMA's pipeline and produce a GAP report.

## ⚠️ HARD ANTI-FABRICATION RULES (a prior ax-3.1-Pro run FALSELY claimed to write a file — do NOT repeat)
- You MUST ACTUALLY WRITE the report to the output file. Do not claim you wrote it unless you did.
- LINE 1 of the report MUST state: `WEB ACCESS: YES` or `WEB ACCESS: NO` — be honest about whether you could actually browse the internet this run.
- For EVERY "latest/current" external claim, cite the SOURCE URL you actually read. If you could NOT browse, mark ALL external claims as `FROM TRAINING KNOWLEDGE — UNVERIFIED (confirm live)` and say so at the top.
- Per claim, tag `[VERIFIED:<url>]` vs `[INFERRED]` vs `[VERIFY LIVE]`. Never invent PixelLab features/endpoints/prices. If unsure → `[VERIFY LIVE]`.
- Do NOT fabricate community examples/tutorials — only cite ones you actually found (with URL).

## READ (our pipeline ground truth — committed)
- `STAGING/WEAPON_PIPELINE_AUDIT_2026-06-08.md` (our code/arch verdict + decisions)
- `STAGING/PIXELLAB_WEAPON_METHOD_DECISION_2026-06-08.md` (our LOCKED method = B-hybrid)
- `STAGING/PIXELLAB_API_V2_REFERENCE.md` (the API ref we have on file — may be outdated; compare vs live)
- `STAGING/CODEANIM_DECISION_2026-06-05.md` (demo = code-only animation)
- `.claude/PROJECT_RULES.md` Asset Üretim section (PPU64, 8-dir via flip, top-down chibi)

## RESEARCH (from the web — cite URLs)
1. PixelLab's CURRENT tool/feature set (as of now): Create Image Pro / image generation, object/item generation, character creation, animation (skeleton/inbetween), tilesets, isometric — exact names, what each outputs, transparency, size/canvas control, variations/candidates, style references, pricing/credits, API v2 endpoints. Note anything CHANGED vs our on-file `PIXELLAB_API_V2_REFERENCE.md`.
2. How OTHERS produce game weapon/item/character sprites with PixelLab: tutorials, dev blogs, YouTube, Discord/forum threads, Reddit, indie postmortems. What workflows/best-practices do they use for: isolated transparent items, non-square sprites, consistent style across a set, pivots/anchors, animation, importing to Unity at fixed PPU. Cite sources.
3. Best-practice patterns for our specific needs: weaponless body + runtime weapon attach; one-direction object + runtime 8-dir mount; hover objects (disc/lantern); dual-wield single-sprite+flipX; style-ref consistency across a weapon set.

## COMPARE → GAP REPORT (the main deliverable)
Cross-reference the web findings against OUR pipeline (audit + method decision + code). For each gap:
- What we do now (file/decision ref) → what current PixelLab / community best-practice suggests → GAP → recommendation → severity (KRİTİK / ORTA / KOZMETİK) → source.
Cover at least: production method (object-gen vs Create Image Pro — does latest PixelLab change our B-hybrid call?), non-square/transparent/target-size handling, style-ref consistency, pivots, batch limits, animation approach (does anything new make code-only suboptimal?), and anything we MISSED entirely.

## OUTPUT
Write the report to `STAGING/PIXELLAB_PIPELINE_GAP_RESEARCH_2026-06-08.md` (ACTUALLY write it).
Sections: 0) WEB ACCESS yes/no + method · 1) Latest PixelLab capabilities (vs our on-file ref) · 2) Community/best-practice workflows · 3) GAP report vs our pipeline (severity-tagged) · 4) Concrete recommendations (today / pre-production / post-demo) · 5) Open questions for VERIFY LIVE · 6) Sources (URL list).
Be decisive and lean. Tag every external claim's provenance.
