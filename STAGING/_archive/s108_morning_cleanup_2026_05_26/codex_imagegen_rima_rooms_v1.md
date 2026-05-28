# Codex Task — Image Gen: RIMA Rooms with Canonical Characters (Step 2)

## Amaç

User direction (2026-05-22): "ondan sonra o asset pack'i kullanarak benim karakterlerimi ve benim oyunuma uygun roomlar üretecek. boyutu önemsiz ben sadece test için istiyorum, asset pack ile bu rooms üretilebiliyor ve sonsuz üretilebiliyor kanıtlaması."

Step 1 = asset pack sheet (DONE: `STAGING/concepts/asset_pack_sheet_v1.png`)
Step 2 (BU TASK) = 3 farklı oda compose ile RIMA canonical karakterleri yerleştirilmiş

**Test reference.** Boyut önemsiz, modülerlik + RIMA karakter entegrasyonu önemli.

---

## Output

`STAGING/concepts/rima_rooms_with_characters_v1.png`

After generation: `STAGING/concepts/rima_rooms_with_characters_v1_NOTES.md`

---

## Reference materials (mandatory read)

1. **`STAGING/concepts/asset_pack_sheet_v1.png`** — 24-sprite visual envanteri. Step 2'de bu sprite'ları KULLAN, yeni asset uydurma.
2. **`STAGING/codex_dungeon_asset_pack_proposal.md`** Section 4 — 3 sample room JSON (Combat / Corridor / Boss)
3. **`STAGING/CHATGPT_TOPDOWN/*.png`** — atmosphere quality target
4. **RIMA canonical character roster** — see character details below

---

## RIMA Canonical Karakterler (use only these — NOT generic placeholders)

Per `project_canonical_character_roster_v2.md` + `project_character_visual_identity.md`:

### Warblade (primary, in Rooms 1 + 3)
- Heavy greatsword class
- Chibi 3-4 head tall, 64×64 base sprite
- Dark armor with crimson accent, weathered cloak
- Greatsword on back or held
- Tan skin, masculine, stoic warrior face
- Color identity: dark gray + crimson + steel
- Combat stance, weapon ready

### Ronin (in Room 3 team-up with Warblade)
- Katana class, sheath stays on body (Karar #144 exception)
- Chibi 3-4 head, 64×64
- Traditional dark blue/black armor + crimson sash
- Empty sheath on left hip (visible), drawn katana in right hand
- Iaido draw stance, body angled three-quarter
- Color identity: dark blue + crimson + silver edge

### Ranger (in Room 2 solo patrol)
- Recurve bow class
- Adult female, tan skin, off-white bleached-ivory hair tied low ponytail
- Battle-worn dark forest green asymmetric armor, heavier right pauldron
- Cold blue accent strips
- Bow held ready or drawn
- Predator-still alert face
- Color identity: dark forest green + blue accent

### Enemy placeholders (humanoid, generic OK)
- Generic skeletal/hooded combatants
- ~64×64 chibi same scale as player characters
- Dark armor, faces hidden
- Each with thin red HP bar overhead (1px line, optional)
- Boss silhouette in Room 3: large humanoid ~3× player scale, dark crowned figure, ominous

---

## Layout — 3 panels side-by-side (or vertical stack if better composition)

### Panel A — Combat: Broken Slab Hall (16×12)

**Scene:** Player Warblade vs 3 enemies in mid-combat moment

**Composition:**
- Hades-iso ~70-75° tilt
- Warblade at center-south, greatsword raised, mid-attack arc
- Enemy 1: close left at x=5 y=6, ready to engage
- Enemy 2: mid-right at x=11 y=6, holding weapon
- Enemy 3: far north at x=12 y=10, distant scout
- Cyan rift accent at floor center (x=8 y=6, F06 from sheet)
- Walkway strip vertical center (x=6-10 zone using F03/F04 from sheet)

**Assets used (referencing Step 1 sheet IDs):**
- Floor: F01 granite base + F03/F04 walkway strip + F05 rubble NE + F06 cyan rift center
- Walls: W01 north × 3 (top edge), W02 east + flipX west, W03 corners × 4
- Props: P01 column at x=4 y=4, P02 broken column x=11 y=7 flipX, P03 banner on north wall, P04 wall_torch × 2 (left + right wall), P06 urn NE, P07 rubble pile NE, W05 collapsed stub east
- Decals: D01 moss wall edges, D02 cracks scattered, D03 blood center, D04 dust NE

**Lighting:** warm torches (#FFA060) + cool cyan rift (#5DEFFF) dual-tone

**Mood:** Mid-combat tension, lived-in atmosphere

### Panel B — Corridor: Hairline Rift (8×24)

**Scene:** Ranger alone, walking/patrolling north through narrow corridor

**Composition:**
- Hades-iso ~70-75° tilt
- Narrow vertical room (compressed in panel layout)
- Ranger at center, bow held ready, walking pose, alert
- Subtle distant rift glow visible at far end (archway entrance to next room)

**Assets used:**
- Floor: F03 walkway (default) + F01 granite center strip + F06 cyan rift accents × 2
- Walls: W02 east + west long edges (4 segments each side), W04 inner corners, H01 archway at north end
- Props: P04 wall_torch × 2 (mid corridor), P06 urn at side, P07 rubble pile far end
- Decals: D02 crack mid, D04 dust trail, D05 cyan glyph at archway

**Lighting:** dim global (#191D23), small torch halos + cyan archway hint

**Mood:** Tense traversal, solo navigation, hint of danger ahead

### Panel C — Boss Approach: Rift Court (24×18)

**Scene:** Warblade + Ronin teaming up, entering boss arena, boss silhouette at distance

**Composition:**
- Hades-iso ~70-75° tilt
- Wide arena
- Warblade at south entry (y=4 area), greatsword drawn, advancing
- Ronin beside Warblade (x=11 y=4), katana drawn iaido stance
- Boss silhouette at x=12 y=14, large humanoid ~3× player scale, dark crowned, ominous pose
- Central cyan rift glyph circle around boss (D05 large or F06 cluster — magic summoning ring)
- Dramatic flanking columns + braziers

**Assets used:**
- Floor: F01 granite default + F04 walkway central + F05 rubble NW corner + F06 cyan rift 4×4 zone center
- Walls: W01 north × 3 segments, W03 corners NE/SE/NW/SW, W05 collapsed stubs × 2 (NW + NE flipX), H01 archway south entry
- Props: P01 columns × 2 (NW + NE), P02 broken columns × 2 (mid flanking), P03 banners × 2 north wall, P05 floor_brazier × 2 (left + right of boss), P06 urn NW, P07 rubble pile NE
- Decals: D01 moss NW, D02 crack center, D03 blood scattered, D04 dust NE, D05 glyph circle around boss

**Lighting:** very dark global (#15191F), strong cyan boss rift center (#5DEFFF intensity 2.0), 2 warm brazier halos flanking

**Mood:** Maximum drama, boss confrontation approach, ritual dread

---

## Style — overall multi-panel

- Each panel rendered at **concept art quality** matching `STAGING/concepts/dungeon_concept_minpack_combat_v1.png` style
- Pixel art aesthetic with painterly polish
- 3 panels side-by-side (or vertical stack), each with small title at top
- Dark unified background separating panels
- **CRITICAL: USE THE SAME SPRITE LIBRARY from Step 1 asset pack sheet.** No new assets invented. Same visual style across all 3 panels.
- Show MODULAR REUSE — same wall/column/torch sprites visible in multiple panels but different placement/lighting

### Size
- Önemsiz, küçük OK (1024×768 OR 1536×1024)
- 16:9 panel aspect OR 1:1 each panel

---

## Negative directives

- NO generic placeholder characters — must use Warblade + Ronin + Ranger visuals as specified
- NO new asset invention — only use Step 1 asset sheet's 24 sprites
- NO mixing art style between panels (all 3 same Hades-iso)
- NO modern UI elements, NO HUD (except subtle enemy HP bars OK)
- NO pure 90° flat top-down — must be Hades-iso ~70-75°
- NO bright cartoon palette

---

## Tool

Codex imagegen skill (gpt-image-1 force). Step 1 sheet (`STAGING/concepts/asset_pack_sheet_v1.png`) as visual reference if `init_image` / `reference_image` parameter supported. If not, embed sprite descriptions textually.

Single output PNG.

Save to: `STAGING/concepts/rima_rooms_with_characters_v1.png`

---

## After image gen — NOTES.md

`STAGING/concepts/rima_rooms_with_characters_v1_NOTES.md`:

### Modular reuse evidence
| Sprite (from Step 1 sheet) | Used in Panel A | Used in Panel B | Used in Panel C | Total uses |
|---|---|---|---|---|

### Character canon check
| Character | Visible? | Matches canonical visual identity? |
|---|---|---|
| Warblade | YES/NO | PASS/TWEAK |
| Ronin (with sheath) | | |
| Ranger (bow + ivory hair) | | |

### Quality grade per panel
| Panel | Atmosphere | Hades-iso tilt | Modular reuse readable | Overall |
|---|---|---|---|---|
| A Combat | A/B/C/F | | | |
| B Corridor | | | | |
| C Boss | | | | |

### Recommendation
- "Sonsuz oda" modüler kanıt READABLE? YES/NO
- Quality enough to greenlight PixelLab production with this asset pack? YES/NO

---

## Commit
```
[Codex] [S98 IMAGEGEN] RIMA rooms with canonical characters — Step 2 of sequential

- Codex imagegen 3-panel concept using Step 1 asset pack sheet as reference
- Warblade + Ronin + Ranger placed in 3 distinct rooms
- Combat / Corridor / Boss approach
- Modular reuse evidence — same 24 sprites across all panels
- PNG: STAGING/concepts/rima_rooms_with_characters_v1.png
```

Wall clock: ~10-15 min.
