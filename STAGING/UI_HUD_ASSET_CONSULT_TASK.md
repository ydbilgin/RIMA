# UI / HUD Asset Pack — production CONSULT (imagegen vs PixelLab) for RIMA

ACTIVE RULES: (1) think before answering (2) ADVICE ONLY — no files, no generation (3) be concrete + style-aware (4) flag uncertainty.

NLM ACCESS: RIMA canon in NotebookLM if useful: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "RIMA UI/HUD visual style, menu/class-select look, color palette"

## Goal
RIMA's game screens (main menu, **class-select**, in-game HUD) currently look "saçma sapan" — white boxes, no frames, unreadable. The user wants a **coherent UI/HUD asset pack** to make it look like a REAL, polished game-screen start. Decide HOW to produce it.

## CRITICAL STYLE LESSON (do not repeat)
A prior cx **imagegen** pass made REALISTIC painterly character portraits → REJECTED as off-style ("çöp, breaks our style"). RIMA's art = **pixel-chibi characters (PixelLab, 120-128px, PPU 64), dark-fantasy, slate/stone + cyan seal-energy #00FFCC accent**. So any UI assets MUST be **style-consistent** (NOT photoreal, NOT smooth-vector-generic). The whole question is: how do we get on-brand UI chrome.

## RIMA context
2D top-down action-roguelite, Unity URP 2D. Floating seal-keep world. Hades/Dead Cells adjacent. 10 classes (4 unlocked + 6 achievement-locked, Slay-the-Spire-card reveal style). Class-select layout: LEFT class list, CENTER selected char's idle sprite, RIGHT skills (some locked).

## Advise on (concise, concrete)
1. **imagegen vs PixelLab — per asset type.** For UI chrome (frames, panels, bars, card-frames, slot-frames, vignettes, glows, headers/dividers, buttons): which tool gives on-brand results? Where is imagegen (gpt-image-2) good vs where PixelLab is mandatory? The user wants to START with imagegen for speed — is that viable for UI chrome WITHOUT it looking off-style? How (prompt constraints: flat painterly stone/metal, limited palette, pixel-leaning, no photoreal)?
2. **The asset list** — exact UI/HUD elements needed to make menu + class-select + HUD look like a real game. Group by screen. Prioritize the few that give the biggest "nice game screen" jump first.
3. **Style spec** so imagegen stays ON-BRAND: palette (slate/stone + cyan #00FFCC sparing), motif (seal/rift/chain/stone-masonry), finish (flat/painterly not photoreal), to avoid the portrait failure.
4. **Sizes + format** — dimensions per asset (9-slice frames need specific structure), transparent vs magenta-chroma, and how to keep them **PixelLab-replaceable** later if needed.
5. **Your one-line verdict:** is imagegen the right START for the UI pack, or should it be PixelLab from the start? Why?

cx lens = production/technical (tools, sizes, 9-slice, pipeline, PixelLab-replaceable). agy lens = art-direction/genre (what a real game-screen needs, reference UIs, on-brand guardrails). ~600 words. This feeds an Opus decision, then cx imagegen produces.
