# RIMA — CharacterSelect Asset Pipeline — LEAN / SKEPTIC lens (Gemini 3.5 Flash)

READ this brief first: F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\ASSET_PIPELINE_BRIEF_2026-06-04.md

You are the LEAN / over-engineering-critic advisor (deep-art + feasibility advisors answer separately). The user wants the premium look of a generated modular asset sheet, but your job is to prevent a slicing/style-mismatch time-sink and find the highest ROI path.

Answer blunt, bullets:
1. **Smallest generate set** that delivers the premium feel: of {background, panel/card frames, skill-card composite, class icons ×10, resource bars, Ashen Glyph pedestal, VFX} — which 2–3 actually move the needle, and which are vanity or already covered by existing Pack assets (button_9slice, card_frame_9slice, panel_frame_9slice, bar_frame_9slice, pedestal_seal, bg_seal_keep)?
2. **Sheet vs separate:** which is LESS work end-to-end (gen + slice + import + transparency cleanup)? Argue the pragmatic choice.
3. **Class icons:** reuse idle_south (zero gen) vs generate 10 icons — is generating 10 cohesive class icons worth it, or a trap? Cheapest acceptable answer.
4. **Biggest time-sink / failure mode** in a generate→slice→wire pipeline for pixel-art (transparency halos, cell misalignment, PPU/scale mismatch, style drift) and how to dodge it.
5. **Ship order:** what to do FIRST so the CharSelect rebuild isn't blocked waiting on art (e.g. build with reuse placeholders, swap in generated assets later)?

Favor reuse + defer. Numbers where useful.
