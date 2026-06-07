# Codex Second Opinion - ISO Wall Yukseklik Verdict

## Q1: Optimum Wall Yukseklik
C / 128x192 optimum. 128x96 and 128x128 keep production compact, but the opened chatgpt_ref rooms read as tall dungeon architecture: visible wall and pillar masses are roughly 3x character height, not 1.2x-1.6x. 128x256 gives the strongest drama, but it pushes straight wall sprites into high-occlusion territory, increases PixelLab/detail risk, and makes PPU 64 composition feel top-heavy for normal combat rooms. 128x192 keeps the existing 128 px floor-width footprint, reaches about 2.4x-3.0x against a 64-80 px character, and leaves 224-256 px height for special doors, corners, boss backwalls, or landmark pillars.

## Q2: PPU 64 vs 100 Migration
Stay on PPU 64. Moving to PPU 100 would invalidate the locked character baseline, existing floor/Wang assumptions, and grid scale for little gameplay value. Adopt Gemini's layout logic at half-scale instead: 128x64 floors, 128-width wall bases, bottom-center pivots, and sort-safe tall transparent canvas where needed.

## Q3: Wall Width = Floor Width Kurali
This rule is critical for RIMA's modular iso kit. The wall's bottom iso footprint should match the floor tile edge width so rooms snap without visual drift, diagonal seams, or manual gap hiding. Tiny art overhangs are fine inside transparent padding, but the logical base footprint should remain 128 px wide.

## Q4: Char-Wall Ratio
Ideal normal-room ratio is about 1:2.5 to 1:3.0. With a 64-80 px effective chibi character, 192 px wall height lands in the practical target band while preserving combat readability and click/telegraph space. Use 1:3.25 to 1:4.0 only for setpiece architecture, boss-room rear walls, gates, and tall pillars where occlusion is intentional and controlled.

## Q5: Final Verdict (ozet)
- Wall size: 128x192 for standard NW/NE straight walls; allow taller 128x224/128x256 special variants only for setpieces.
- Confidence: high
- Rationale: 128x192 is the best balance of chatgpt_ref drama, PPU 64 compatibility, modular floor alignment, readable combat, and feasible sprite production.

## Bonus: Gemini onerisinin degerli kisimlari
- Keep bottom-center pivots and a consistent floor-contact baseline.
- Keep wall base width equal to floor tile width for clean iso snapping.
- Use dynamic Y-sort / sort points deliberately, especially on tall transparent canvases.
- Treat straight wall, corner, and doorway assets as the same footprint family even if their visible height differs.
- Preserve Gemini's dramatic verticality target, but scale it to RIMA's locked 64 PPU pipeline instead of migrating the project.
