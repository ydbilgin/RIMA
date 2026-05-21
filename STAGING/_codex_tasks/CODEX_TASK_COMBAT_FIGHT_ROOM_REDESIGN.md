# Codex Task — Combat Fight Room Redesign (for tomorrow's playable test)

**Profile:** auto-selected (quota-aware) — yasinderyabilgin or laurethayday or laurethgame (post 03:13 reset)
**Effort:** xhigh
**Timeout:** 7200s (2 hours)
**Type:** Critique current v13 → redesign for COMBAT encounter → iterate → verify

## User feedback (verbatim 2026-05-18 night)

> "yarına oynanabilir(tam stil oturmasa bile) bi map istiyorum"
> "güzel doğal görünümllü geçişli codex imagegen ile"
> "saçma sapan itemlar olmasın etrafta"
> "gerçekten anlamlı bi fight odası gibi olsun"

Translation:
- Target: PLAYABLE map by tomorrow morning (style doesn't need to be perfect)
- Beautiful natural look + smooth biome transitions (Codex imagegen if needed)
- NO random/nonsensical items scattered around
- **Make it an ACTUAL FIGHT ENCOUNTER ROOM** — meaningful combat layout

## Authority granted

Phase A v13 PASS — current scene `PlayableRoom/Pro_Redesign_v13` looks like a beautiful **ritual chamber**. User now wants a COMBAT room. You have full autonomy to:
- Re-theme the scene as combat encounter
- Add/remove props per combat encounter design principles
- Generate new Codex imagegen assets if combat-specific elements needed (enemy spawn markers, danger zone decals, blood splatter, weapon racks)
- Adjust lighting for combat tension (warmer rim, dim ambient, focal red threat)

## CRITICAL — Unity pre-check

Unity OPEN, instance `RIMA@ed023e0b`, scene `RoomPipelineTest`. Verify before tools.

## Combat Encounter Design Principles (REQUIRED to apply)

### Layout rules

1. **OPEN CENTER** — combat space, player must be able to move + dodge. NO obstacle in middle 6×6 unit area.
2. **PERIMETER COVER** — 4-6 vertical props (columns, walls, debris piles, statues) on perimeter for tactical positioning + visual depth. NOT in center.
3. **PLAYER ENTRY** — south (bottom), clear path from edge to center (sparse trail accents OK)
4. **ENEMY EXITS / ENCOUNTER MARKERS** — 3-5 implied enemy spawn positions (visual: small ash circle / blood splatter / scorched ground). NO actual enemies spawned (placeholder game logic only)
5. **ROOM EXIT** — north arched doorway (use existing wall_11 arched variant from v3 walls)
6. **FOCAL TENSION** — 1 atmospheric accent placed strategically (rift crack pulling eye, NOT a peaceful ritual sigil)
7. **NO LOOT/REWARDS visible** — combat room, not treasure room. Treasure pile/chest = OUT.
8. **NO sacred shrines** — ritual circle removed, candelabras moved to perimeter only

### Visual mood rules

- Cooler ambient (slightly less warm than ritual chamber v13)
- 1 red/orange threat accent (a single small brazier or scorch mark in center)
- Heavy edge vignette (claustrophobic combat space)
- Floor: dark cracked stone with subtle blood-stain undertone (use Sheet 11 biome_floor_09..12 "blood" biome OR procedural blood-tint regions)

## Suggested zones (combat encounter)

| Zone | Position (in 36×22 scene, player at center) | Content |
|---|---|---|
| **Center 6×6 open** | (15-21, 8-14) | EMPTY except small focal mark — combat ground |
| **North wall border** | y=20, x=12-24 | 8 wall sprites with 1 arched doorway (wall_11) = room exit |
| **NW cover** | (8, 16) | 1 broken column + 1 debris pile (combat cover) |
| **NE cover** | (28, 16) | 1 brazier (warm orange threat light) + 1 column |
| **SW cover** | (8, 6) | 1 statue (knelt, defeated foe atmosphere) + scorch mark |
| **SE cover** | (28, 6) | 1 debris stack + ash circle |
| **South entry** | (18, 2) | Sparse pebble trail leading to center (player path indicator) |
| **Center focal threat** | (18, 11) | 1 single small rift crack OR 1 ash scorch — implies danger |
| **3-5 enemy spawn markers** | perimeter (10,18), (26,18), (10,6), (26,6), maybe (18,18) | Small ash circles / blood splats / scorched ground |

Do NOT include:
- Ritual sigil (move out, replace with scorch)
- Portal puddle (cool blue peaceful — wrong mood for combat)
- Candelabra (peaceful decoration, replace with single combat brazier or remove)
- Banner (decorative, OK to keep ONE small if combat-relevant — fallen banner OK)
- Foliage / moss / trees (combat room, not garden)
- Multiple ritual focal points

## Stages

### Stage 1 — Critique current v13

In `STAGING/CODEX_COMBAT_ROOM_CRITIQUE_v13.md`:
- What in v13 makes it feel like a ritual chamber (not combat)?
- What needs to go (ritual sigil, peaceful candelabra, portal puddle, etc.)?
- What needs to come in (combat cover, threat marks, room exit)?

### Stage 2 — Implement combat redesign

- Disable or destroy peaceful elements from v13
- Add combat cover layout per zones above
- Adjust lighting (cooler ambient + 1 red threat accent)
- Floor: blood-undertone procedural OR use Sheet 11 biome_floor 09-12

If you decide new Codex imagegen assets are needed (e.g., specific combat decals like blood splatter, weapon rack, enemy spawn markers):
- Generate via `imagegen` skill (gpt-image-1) → `STAGING/RIMA_AssetParts_v4_combat/`
- Slice + import to `Assets/Sprites/Environment/RIMA_AssetParts_v4_combat/`
- Create new PatchAtlasSO

### Stage 3 — Verify playability

Enter Play mode:
1. Player visible at south entry
2. WASD movement works (test with `rb.linearVelocity` simulation 2 sec)
3. Camera follows player
4. Combat space visually clear (open center)
5. Cover obstacles surrounding feel intentional
6. Mood feels "fight here" not "pray here"

Game view screenshot: `Assets/Screenshots/PlayableRoom_combat_v14.png`

### Stage 4 — Iterate (max 3 internal)

If self-QC FAIL, iterate v15, v16. Stop when YOU can defend "this is a real combat encounter room".

### Stage 5 — DONE marker

`STAGING/CODEX_TASK_COMBAT_FIGHT_ROOM_REDESIGN_DONE.md`:
- Iterations attempted
- Critique findings v13
- Combat redesign decisions per zone (with rationale)
- New assets generated (if any)
- Visual gate verdict
- EditMode tests (must remain 333/333)
- Play mode console errors (must be 0)

## Constraints

- DO NOT modify SO contract scripts
- DO NOT modify Phase 1.5 data-first executors
- DO NOT change PPU=32 or camera tilt
- DO NOT delete v13 prefab (preserve as `Pro_Redesign_v13_RitualChamber` for reference)
- DO use existing v2 + v3 assets first; new imagegen ONLY for combat-specific (blood splatter, ash circle, weapon rack, enemy marker)
- DO save scene in Edit mode (NOT Play mode — known save fail)
- DO use new Input System for any new player scripts

## NEXT_SIGNAL

DONE notification → orchestrator inspects screenshot. If PASS → user wakes up with playable combat room. If FAIL → orchestrator decides retry vs scope cut.
