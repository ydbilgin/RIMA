# DECISION — CharacterSelect v2 "Diegetic Roster Room"
Date: 2026-06-04 · Council: cx (feasibility/least-code, laurethayday) + ax-3.1-Pro (design) + ax-3.5-Flash (lean) → Opus
Brief: STAGING/CHARSELECT_ROSTERROOM_BRIEF_2026-06-04.md

## ONE LINE
Replace the left-list + center-showcase with a diegetic iso keep ROOM: one generated empty backdrop (top ~75%) + 10 real character sprites placed in a depth-sorted arc + click-to-select with a reused pedestal_seal ring under the selected + dim others + bottom HUD strip (identity | skills | SEÇ/GERİ). KEEP all data wiring.

## LAYOUT
- **Backdrop:** generated empty iso ruined-keep room → `Assets/Resources/UI/RIMA/CharacterSelect/room_bg.png`, loaded via `RimaUITheme.CreateFullScreenBackdrop("UI/RIMA/CharacterSelect/room_bg", BackgroundDark)` (cover/crop, NOT stretch). Fills top ~75%; bottom ~25% = dark HUD strip with 2px cyan top border (frames the room).
- **Character placement (ax-3.1 arc, normalized anchors, pivot (0.5,0), sized in 1920×1080 ref units):** depth rows, FRONT = 4 unlocked, BACK = 6 locked. Approx normalized (x, y above bottom bar):
  - Front (unlocked): Warblade (.35,.45) · Elementalist (.45,.40) · Ranger (.55,.40) · Shadowblade (.65,.45)
  - Back (locked): Ronin (.20,.60) · Ravager (.32,.55) · Gunslinger (.44,.52) · Brawler (.56,.52) · Summoner (.68,.55) · Hexer (.80,.60)
  - **Draw order = sibling order by Y:** higher-Y (back) first, lower-Y (front) last → front draws over back. Each character's seal/glow sits BEHIND its own sprite in the same root.
  - Selected scale ~1.12; back row base scale ~0.85. Keep PPU64/Point/no-compress; avoid fractional localScale jitter (integer-friendly sizeDelta).

## INTERACTION + SELECTION EFFECT
- Per-character **transparent Button** hit area (MainMenuController.AddNakedButton pattern); listener captures `ClassType` → `SelectClass(cls)` (existing).
- **Selected:** reused `pedestal_seal` as a flat ring under feet + cyan pulse glow + scale 1.12 + brief flash. **Others dim** via CanvasGroup.alpha — unlocked-unselected ~0.75, locked-unselected ~0.40.
- **Hover:** raise that root's glow/outline slightly. Keyboard nav = deferred (click + SEÇ ships).
- **Confirm:** SEÇ button (or double-click selected). GERİ = back.

## LOCKED-CLASS UX (industry synthesis — readable, teaches meta)
Survey: Dead Cells / Slay the Spire / Risk of Rain 2 show the locked thing + condition (not pure mystery); Hades uses cost CTA. RIMA classes are known archetypes → DON'T full-silhouette (hides identity needlessly). Decision = **"Dimmed-in-place + lock + cost CTA":**
- Locked char stands in the room, sprite dimmed (~0.40 alpha + slight void-purple tint), small **cyan padlock glyph** above + **Echo-cost chip** under feet.
- Still clickable → `SelectClass()` updates bottom bar; identity tagline shows unlock condition (`IdentityLockText()`), skill rows empty-state.
- **SEÇ button morphs to "KİLİDİ AÇ — {cost} Echo"** (reuse `LockedButtonText()`); glows cyan if affordable, muted if not; `startButton.interactable = IsUnlocked(cls)` (existing logic at 593-601). Hexer special path (250 Echo + Elementalist run) preserved.

## BOTTOM HUD STRIP (3 zones, ax-3.1)
- Dark glass `#110817`@~0.90 + 2px cyan top border. Anchored bottom 0.0–0.25.
- **Left (~25%) Identity:** class name (bold) + tagline/motto (cyan italic) + resource (muted) — from `ClassIdentity()`/`ClassTagline()`.
- **Center (~50%) Skills:** horizontal 3–4 skill chips (48px icon + name + keyword), from `SkillDatabase.GetAll().Where(classType==sel && !isPassive)`. Locked/no-data classes → existing empty label.
- **Right (~25%) Action:** big **SEÇ** (cyan bg, dark text, white hover) + small **GERİ** (muted) top-right.

## REUSE MAP (cx — keep, don't rebuild)
KEEP: BuildScreen canvas/scaler/raycaster/RuntimeRoot/authored-disable (100–157) · SelectClass(557–604) · RefreshIdentityPanel(606–624) · RefreshSkillList(626–650) · EnsureSkillDatabase(840–859, SkillDatabase.EnsureBuilt/GetAll) · OnStartRun/OnBackClicked(700–718) · IsUnlocked/UnlockCost/LockedButtonText/IdentityLockText/CardActionText(721–758) · LoadCanonicalSprite(803–807) · RimaUITheme.ClassAccent/AnchorPath/ClassTagline/ClassIdentity.
REPLACE: BuildRosterList/BuildClassCard(198–324) + BuildCenterPanel(326–431) → `BuildRosterRoom` + `BuildRoomCharacter` (RoomEntry struct: root, sprite, hit, button, seal, glow, lock, costChip, CanvasGroup, ClassType). MOVE BuildSkillDetailPanel/BuildStartButton/BuildBackButton into bottom strip. AnimateShowcase → `AnimateRoomSelection` (pulse selected entry's seal/glow + bob). Add `RoomBackdrop` const + placement table (sort by Y desc before sibling assign).

## BACKDROP GENERATION (ax imagegen — ALLOWED, env art)
Generate ONE 1024×1024 on-brand pixel-art EMPTY iso keep room. **Central selection seal = SEPARATE reused pedestal_seal sprite (NOT baked)** so it moves/glows under the selected char → bg center stays clear. Prompt:
> Isometric 3/4 perspective pixel art of an ancient ruined gothic keep platform floating in a dark cosmos, dark fantasy roguelite style. Flat cracked abyssal-stone tile floor with faint glowing cyan (#00FFCC) rift-cracks. Tall stone braziers burning warm orange (#E89020) fire on stone columns far-left and far-right. Tattered dark banners on broken pillars in back. Swirling void-purple (#3A1A4A) starry-nebula sky. The central and lower floor must be completely EMPTY, flat, unobstructed (for character sprites placed later). NO characters, NO text, NO UI. Detailed 16-bit pixel art.
Pipeline: generate → QC (view PNG) → pixel_cleanup if needed (snap palette) → import (Sprite Single, PPU64, Point, no-compress, mip off, alphaIsTransparency, maxSize≥2048).

## SHIP ORDER (3.5 Flash)
1. Generate + import backdrop. 2. Build room + placed sprites + click-select + per-char seal. 3. Bottom HUD bar (reuse data). 4. Locked states. → feel-test unblocked early. Idle animation = LATER (user; static now).
