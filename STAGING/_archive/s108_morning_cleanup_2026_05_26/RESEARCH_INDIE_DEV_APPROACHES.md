# RIMA Indie Dev Research — 2026-05-18
Model: default (gemini-2.5-pro-preview via ~/.gemini/settings.json)

---

## Topic 1 — Hades: Fake 3D Techniques

**Key Insight:** Supergiant paints environment assets flat in 2D, then squashes and rotates them 45° inside their engine to simulate isometric perspective — no 3D geometry needed. Characters are sculpted in ZBrush over Jen Zee's 2D portraits, textured with flat colors + hand-painted ink lines in Substance Painter, animated in Maya, then pre-rendered at 64 angles and streamed as Bink video sprites. Per-room lighting is baked via `.lua` config (ambient + diffuse values + LUT per biome); dynamic elements tint the 2D background at runtime rather than computing real lighting.

**Sources:** GDC 2021 "Hand-Crafted Variance: Designing Hades' Underworld"; Paige Carter art pipeline deep-dive (Game Developer / Noclip "Developing Hell"); GDC 2021 Greg Kasavin + Darren Korb dialogue talk.

**RIMA applicability:** RIMA's 30-35° angled camera is architecturally identical to Hades' approach — this validates the lock. Add: each RoomBank entry should carry a `LightingPreset` SO (ambient color, diffuse intensity, vignette LUT) so rooms feel distinct without runtime overhead. Stop relying on a single global light; bake biome identity into room presets.

**LaurethStudio applicability:** Add "per-room LightingPreset SO" pattern to `project_room_library_architecture.md`. Document the Hades character pipeline (ZBrush → flat texture → ink lines → pre-render) as the gold-standard ref in `PIXELLAB_PRODUCTION_GUIDE_v1.md`.

---

## Topic 2 — Painterly Floor Techniques (Organic Map Aesthetics)

**Key Insight:** Continuous painterly floors require three layered tricks: Wang tile sets (16+ variants) to break grid repetition at transitions; large overlapping irregular decal sprites placed on top of the tilemap to hide geometric seams (the "Don't Starve method"); and a global screen-space noise/canvas overlay (Multiply or Overlay blend) to unify disparate assets into one painted medium. Transform randomization (±20% scale, 0-360° rotation) on scattered clutter creates organic variance from a tiny asset set.

**Sources:** AdamCYounis pixel environment class (YouTube); Tiled documentation on Wang Tiles (doc.mapeditor.org); r/gamedev thread on procedural vs hand-crafted levels.

**RIMA applicability:** Wang Full 16 corner set (already locked per Karar #143) is correct. Add: scatter decals must use ±20% scale + full rotation randomization in `PropRuntimeSpawner`. Add a global URP post-process overlay (noise texture, Multiply, ~8% opacity) to unify the painterly pack assets. This is a one-shader fix with high visual ROI.

**LaurethStudio applicability:** Add the "Mock-up to Stamp" workflow (paint hero terrain in Krita → Offset Filter → cut into stamps) to `RIMA_MAP_PRODUCTION_SEQUENCE.md` as the standard floor asset creation method.

---

## Topic 3 — Hyper Light Drifter: Depth and Color Discipline

**Key Insight:** HLD creates depth in a flat 2D top-down world using three techniques: (1) foreground silhouette layers rendered in dark/saturated colors that move at *negative* parallax speed relative to the camera; (2) Photoshop-built scenes imported into a custom GameMaker tool that allows off-grid object placement, breaking the rigid tile snap; (3) strict palette split — highly saturated neons reserved exclusively for player/enemies/interactables, muted desaturated tones for all environmental surfaces.

**Sources:** GDC 2017 "Hyper Light Drifter: Heart Machine's Quest for Quality" (Alx Preston + Teddy Dief); Kickstarter devlogs; GDC Europe 2016 Akash Thakkar audio talk (indirect visual pipeline ref).

**RIMA applicability:** RIMA must enforce palette discipline at the asset spec level: floor/wall tiles must stay desaturated (muted stone tones); VFX, character rims, and interactable highlights must be the only high-saturation elements. Add foreground framing elements (pillars, arch edges) as a dedicated prop layer with negative parallax offset. Off-grid prop placement is already supported via `PropRuntimeSpawner` — ensure rotation is not locked to cardinal angles.

**LaurethStudio applicability:** Add the "desaturated ground / saturated subjects" rule as a hard constraint to `PIXELLAB_PRODUCTION_GUIDE_v1.md` and the style bible.

---

## Topic 4 — Indie Map Editor UX

**Key Insight:** The highest-ROI editor UX patterns: (1) Left-panel docked palette for high-frequency asset swapping — floating palettes fail beyond ~30 items. (2) Brush strokes must undo as single atomic actions, not tile-by-tile — this is the #1 editor frustration point. (3) Mixed-initiative design (Townscaper model): user paints intent, algorithm executes tile selection — reduces UI surface area while increasing output quality. (4) A "ghost/plan layer" for non-destructive previewing prevents costly mistakes.

**Sources:** Robin-Yann Storm GDC "Keeping Level Designers in the Zone through Level Editor Design" (YouTube); Townscaper (Oskar Stålberg) design philosophy; Stardew Valley community Stardew Planner tool (gap analysis); Sims 4 build mode postmortem.

**RIMA applicability:** Brush Tool V1 already ships with the correct brush-stroke model. Verify that multi-tile brush strokes register as a single undo action in `TerrainDataWriter`. Add a "Preview Layer" toggle to the editor that shows ghost placements before commit — directly mirrors RimWorld's Plan tool. The left-panel docked asset browser (already spec'd in Phase B1) is validated as the correct choice.

**LaurethStudio applicability:** Add "single-action undo for brush strokes" and "preview/ghost layer" as mandatory requirements to `SPEC_PHASE_B1_ASSET_PACK_BROWSER.md`.

---

## Topic 5 — PixelLab Community Workflow

**Key Insight:** Community-validated workflow is: generate "naked" base body at low resolution to confirm silhouette → inpaint clothing/weapons to lock the anchor → use the anchor as style reference for all states (not a fresh generation) → manual palette swap for color variants rather than AI re-generation. The "Fresh Roll" problem (proportion drift per generation) is the primary failure mode; strict style reference on every subsequent call is the mitigation. Credits are best spent on the anchor and animation frames — variations and recolors should be manual.

**Sources:** PixelLab.ai official docs; HyperGPT.ai platform overview; Reddit r/IndieGameDev discussions on style drift; PixelLab Aseprite plugin community workflows.

**RIMA applicability:** The S87 LOCK workflow (anchor → state → animate) is already correct. Reinforce: for any new class character, generate one high-quality anchor at full credit cost, then inpaint for all outfit/weapon states rather than re-generating. Color variant mobs (e.g., elite enemy recolors) must be palette-swapped manually, not AI-generated.

**LaurethStudio applicability:** Formalize the "inpaint over anchor, never fresh-roll for states" rule in `project_pixellab_character_states_workflow.md`. Add cost tier note: anchor = full cost, state = inpaint (lower cost), color variant = manual (zero cost).

---

## Topic 6 — ARPG Map Design Rules (D2 / PoE)

**Key Insight:** D2 established the "breathe" principle: alternate high-density combat arenas with empty decompression corridors — 100% monster density causes fatigue and kills the dopamine loop. Chokepoints must be minimum 2 tiles wide for multiplayer and skill-build viability; 1-tile corridors (D2's Maggot Lair) are universally disliked. PoE's key discovery: open sight lines + wide arenas maximize player satisfaction; environmental clutter with invisible collision boxes ("skill fizzling") is the top community complaint in PoE2.

**Sources:** GDC 2016 David Brevik "Diablo Postmortem"; Erich Schaefer "Postmortem: Diablo II" (Game Developer Magazine 2000); Phrozen Keep modding wiki / D2Mods.info; GGG Developer Manifestos (3.1.0 Monster Density); r/pathofexile PoE2 layout discussions (2024-2025).

**RIMA applicability:** Room design rule: every room sequence must alternate a combat-dense "arena" with a lower-density "breathe" corridor or safe zone. All props must have accurate-to-sprite collision boxes — no invisible oversized hitboxes. Minimum corridor width: 3 tiles (RIMA's dash mechanic requires more room than D2). Props placed for visual storytelling must not block projectile sight lines to the encounter center.

**LaurethStudio applicability:** Add the "breathe corridor between arenas" pattern and "3-tile minimum corridor" rule to `project_room_design.md`.

---

## TOP 5 Cross-Topic Insights — Phase B Priority Changes

1. **LightingPreset SO per RoomBank entry is blocking Phase B quality.** Hades proves that per-room lighting baked into data (not runtime) is the correct architecture. RIMA currently uses a single global light. Adding LightingPreset SOs to the RoomBank schema is a low-code / high-visual-impact Phase B task.

2. **The global screen-space painterly overlay (noise Multiply shader) should be Phase B Sprint 1.** HLD, Don't Starve, and painterly indie games all use a unifying texture overlay. RIMA has disparate asset packs — one URP post-process pass would unify them for free.

3. **Palette discipline must be enforced at asset production time, not post-hoc.** HLD's rule (desaturated ground, saturated subjects only) prevents the "everything glows equally" problem. RIMA needs a written chroma budget per asset category added to the style bible before more assets are produced.

4. **Brush stroke undo atomicity is a correctness bug, not a polish item.** If `TerrainDataWriter` records individual tile writes rather than stroke transactions, the editor is broken by industry standards. Verify before Phase B artist handoff.

5. **Prop collision boxes must match sprite bounds exactly.** PoE2's biggest map complaint is invisible collision eating skills. RIMA's dash-based combat makes this a gameplay-critical constraint, not just aesthetics. All props added via `PropRuntimeSpawner` need sprite-accurate physics colliders.

---

## TOP 3 LaurethStudio Template Additions

1. **LightingPreset SO field** — Add to RoomBank SO template spec. Fields: ambient color, diffuse intensity, vignette LUT reference, fog density. Target doc: `project_room_library_architecture.md`.

2. **Asset chroma budget rule** — "Ground/floor tiles: max 20% saturation. Wall tiles: max 30%. Props: max 50%. Interactables/VFX/characters: unrestricted." Add to `PIXELLAB_PRODUCTION_GUIDE_v1.md` as a hard constraint table.

3. **Inpaint-over-anchor cost protocol** — Formalize the three-tier cost model (anchor = full credit, state = inpaint, color variant = manual) with a decision tree: "Is this a new character class? → Full anchor. Is this a state of an existing character? → Inpaint. Is this a recolor enemy? → Manual swap." Add to `project_pixellab_character_states_workflow.md`.
