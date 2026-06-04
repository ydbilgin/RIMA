# CHARACTER SELECT v2 — "Diegetic Roster Room" Redesign — BRIEF
Date: 2026-06-04 · For council (cx feasibility / ax-3.1 design / ax-3.5 lean)

## THE NEW VISION (user-directed, reference = a polished iso roster-room mockup)
Evolve CharacterSelect from the current 3-column-with-left-list (just shipped) into a DIEGETIC ROSTER ROOM:
- **ONE single background image** = an isometric ruined-keep room (the "empty terrain", generated via ax imagegen — NO characters baked in, NO text). Wider perspective.
- **Our 10 class characters stand IN the room** as sprites placed ON TOP of that background (idle_south now; real idle animation later — so they MUST be separate sprite objects, not baked into the bg).
- **REMOVE the left roster list** — the characters in the room ARE the roster. You select by CLICKING a character in the scene.
- **Selection effect:** the clicked/selected character is highlighted/revealed with an effect (cyan seal ring under feet + glow; others dimmed). Like the central seal circle in the reference.
- **Bottom bar (HUD strip):** selected class name + tagline/identity + signature skills + SEÇ(confirm)/GERİ(back). (Reference has Classes recap + details + skills along the bottom.)
- **Locked classes:** the 6 locked classes also stand in the room but LOCKED, with unlock conditions shown nicely (industry-standard pattern). RIMA unlock system: 4 unlocked by default (Warblade, Elementalist, Ranger, Shadowblade); others cost Echo (Ronin/Ravager 120, Gunslinger/Brawler/Summoner 180, Hexer 250 + an Elementalist run).

## CURRENT STATE (just shipped, works)
- `CharacterSelectScreen.cs` builds a 3-column layout (left roster list / center idle showcase on pedestal_seal+bg_seal_keep / right scrollable skill panel + SEÇ/GERİ) into RuntimeRoot_CharSelect; authored UI disabled. Resolution-robust 1920x1080. SkillDatabase query works (Elementalist→Fireball/GlacialSpike/Meteor...).
- This v2 will REPLACE the left-list + center-showcase with the roster-room; KEEP the skill/identity data wiring + SEÇ/GERİ + SkillDatabase query.

## ASSETS
- 10x `Resources/Characters/<Class>/<class>_idle_south.png` (PPU64, real game sprites — REUSE, place in room).
- Generate via **ax imagegen** (generate_image, ~1024x1024, on-brand pixel-art): ONE empty iso ruined-keep room backdrop (seal floor center, braziers, banners, void sky — NO characters, NO text). RIMA palette: cyan #00FFCC rift/seal, void-purple #3A1A4A, warm-orange #E89020 braziers, abyssal stone.
- Existing: pedestal_seal, bg_seal_keep (may reuse for the central selection seal / fallback).

## COUNCIL QUESTIONS
1. **Room composition + character placement:** how to arrange 10 characters in one iso keep room so it reads clean, premium, NOT cramped (reference crams 13 — we have 10 and want "more orderly"). Rows? An arc? Grid on the floor tiles? Where does the camera/perspective sit? How big are characters vs room? Where do locked vs unlocked stand?
2. **Click-to-select UX + selection effect:** per-character clickable hit area; on select → cyan seal ring under feet + glow/lift + dim others? How does the selected char read as "chosen"? Hover state? Keyboard nav fallback? Confirm = SEÇ button or double-click/click seal?
3. **Locked-class UX — INDUSTRY PATTERNS (decide in detail):** how do shipped roguelites/ARPGs present locked characters + unlock conditions (Hades, Dead Cells, Slay the Spire, Risk of Rain 2, Darkest Dungeon)? Silhouette/shadow in place? Lock icon? Condition text on hover/select? Greyed sprite? "X Echo to unlock" CTA? Recommend the cleanest on-brand RIMA approach (Echo-cost unlocks) — readable, not clutter, teaches the meta.
4. **Bottom detail bar layout:** with the left list gone, what goes in the bottom strip — selected class name + tagline + identity (motto/playstyle/resource) + signature skills (icon+name) + SEÇ/GERİ. How to fit without crowding (reference splits Classes-recap | details | skills).
5. **Background generation spec (for ax imagegen):** exact perspective (iso 3/4), aspect/framing (square 1024 framed in top ~70% with bottom bar over lower part? or wide?), content (empty keep platform + central seal + braziers + banners + void backdrop), palette hex, hard EXCLUSIONS (no characters, no text, no UI), and defined ANCHOR ZONES on the floor where character sprites will be placed (so the bg leaves room for them).
6. **Feasibility (cx):** placing sprites at screen/world positions over a bg Image in procedural UI; per-character click (Button/raycast) ; selection ring effect (reuse pedestal_seal? a glow sprite? particle?); how much of current CharacterSelectScreen is reusable vs rebuilt; keep SkillDatabase/identity/SEÇ/GERİ wiring.

## CONSTRAINTS
- ax imagegen for environment bg = ALLOWED (not PixelLab-gated). Characters stay PixelLab/real sprites.
- Reuse-first for everything except the one room backdrop.
- Output: concrete implementable spec + the ax imagegen prompt for the room backdrop.
