# RIMA Batch17 — Chibi Master Prompt (FINAL)
**2026-05-13 | S63 | Karar #100 RESTORE — chibi 64x64 + ~35° kamera**

## Üretim Bilgisi
- **Tool:** PixelLab Create Image Pro (S-XL New)
- **Reference image:** Image #12 (chibi warrior `pixellab-64x64-pixel-art-chibi-characte-1778615086693.png`)
- **Reference açıklaması:** "Match this camera angle, chibi proportions, and south-facing idle pose exactly. Do not copy colors, armor, or weapon design from the reference."
- **Output:** 4x4 contact sheet, 256x256 toplam, 16 karakter

---

## PROMPT (COPY-PASTE READY)

```
=== GLOBAL CONFIG ===

OUTPUT LAYOUT
- 4x4 contact sheet, 16 cells, 64x64 each cell.
- Final image: 256x256. Cells indexed left-to-right, top-to-bottom: 01-16.
- Each cell isolates one character. No overlap between cells.
- Pure transparent background (alpha 0). No oval shadows, no ground plane.

NO TEXT
- No labels, no numbers, no captions, no signatures, no watermarks anywhere.

CAMERA - REFERENCE IMAGE MATCH — HARD RULE
- Match the camera angle, south-facing idle pose, and chibi proportions of
  the attached reference image (chibi warrior 64x64) exactly.
- Do NOT copy colors, armor, weapon design, or identity from the reference.
- ~35 degree high top-down ARPG (Hades match, RIMA canonical).
- All 16 cells share the same camera. No mixed angles.

TRUE SOUTH FACING — MANDATORY, NO EXCEPTIONS
- Character nose points DIRECTLY into camera. Zero rotation left or right.
- Both shoulders must appear at equal width — perfectly symmetric horizontally.
- Character spine is perfectly vertical on screen.
- Hips, chest, and head all centered on the same vertical axis.
- If a character holds something in one hand: that object is beside the body,
  NOT causing the torso to rotate. Body stays centered, arm extends to side.
- Southeast / southwest / three-quarter body rotation = GENERATION FAILURE.

PROPORTIONS - CHIBI
- Chibi-but-serious, 2.5-3 head heights.
- Head ~24-28px (large, readable, facial features clear).
- Body ~30-35px. Stocky compact build.
- NOT super-deformed cute, NOT child-proportions, NOT mature 5-6 head.
- Tone reference: Salt and Sanctuary chibi (dark gritty), NOT Hades bright theatrical.

PIXEL ART STYLE
- Crisp pixel art, NO anti-aliasing on outlines. 1px dark outline.
- 3-4 shade ramps per material. Dark fantasy palette, gritty.
- Material readability: cloth vs leather vs plate distinct at thumbnail.

WEAPON RULE (Karar #99/71)
- All weapons held in hand or sheathed at hip with visible silhouette.
- NEVER mounted across back at idle pose.
- Each weapon must form clear vertical or angled silhouette beside body.

=== CLASS BLOCKS ===

01 - WARBLADE
Male chibi warrior, broad shoulders, heavy stocky build.
Two-handed greatsword held in both hands, low guard stance - sword tip near
ground in front-right, crossguard at waist. Blade visible vertically.
Dark plate armor, matte iron grey + cool blue-tint shadows. Short dark hair,
square jaw, faint cheek scar. Crimson half-cape from left shoulder.
Palette: iron grey + crimson + steel highlight + brass buckle accent.

02 - ELEMENTALIST
Female chibi mage, slim chibi proportions.
NO STAFF. NO WAND. NO OBJECT IN HANDS.
Right hand raised palm-up at shoulder height supporting a floating golden
rune disc (~8px hovering 3px above palm, golden glow, concentric inner ring).
Left hand at hip, fingertips trailing faint golden ember wisp.
Outfit: cropped sleeveless dusty indigo top + small bare midriff strip (~4-6px)
+ flowing high-waisted skirt with side slit + cream sash at waist + dark
fitted tights + high boots.
Warm honey-golden hair in neat low bun, loose temple strands.
Palette: dusty indigo top + cream sash + deep teal skirt + warm gold disc glow.

03 - SHADOWBLADE
Male chibi assassin, slim wiry build.
Dark veil/scarf covering lower face from nose down, only eyes visible.
Hood up (interior dark but face readable).
Twin short daggers, REVERSE GRIP (blades along forearm), one per hand,
arms slightly raised in coiled-ready stance.
Layered dark cloth + leather chest harness, leather bracers, soft boots.
Palette: black + charcoal + hot magenta accent on belt/sash + worn brown leather.

04 - RANGER
Female chibi archer, athletic chibi build.
Off-white / bleached-ivory hair: half-shaved undercut left side, long loose
braid over right shoulder. Bold dark war paint stripe across eyes (2 stripes).
Compound bow held in LEFT hand at thigh-level rest, bow vertical at side.
Right hand free near hip quiver. Hood DOWN. Leather jerkin, wrapped forearms,
asymmetric battle-worn armor (heavier right pauldron, left forearm bracer).
Faint old scar across cheek or brow.
Palette: dark forest green + tan leather + off-white hair + cold rift-purple
accent on bowstring/fletching.

05 - RAVAGER
Male chibi berserker, heavy muscular chibi build, bare-armed.
DUAL HAND AXES - ONE in each hand (NEVER single, NEVER two-handed).
Each axe forearm-length, short wooden haft, single curved iron head.
Both arms SPREAD WIDE to sides, elbows away from torso, axes at hip level
flanking body. Silhouette: two hook shapes at sides.
Heavy fur mantle on shoulders, bare torso under, leather kilt, wrapped legs.
Wild dark hair, thick beard.
Palette: dirty bronze skin + dark fur + crimson cloth wraps + dull iron axes.

06 - RONIN
MALE chibi swordsman, lean disciplined chibi build.
Single katana sheathed at LEFT HIP - black saya/scabbard CLEARLY VISIBLE,
tucked into obi sash on character's left side.
Right hand resting near katana handle (iaido draw-ready).
Dark navy / near-black kimono and hakama, simple cloth obi at waist.
Dark hair tied back. Calm focused expression.
Palette: muted indigo + black + dull silver blade accent + off-white sash.

07 - GUNSLINGER
Female chibi gunner, lean chibi build.
Deep auburn red hair tied back. Dark grey-purple trench coat, brass buckles.
DUAL pistols, BOTH visible - one drawn in each hand, guns slightly raised
in kinetic stance.
Palette: dark leather brown + brass + dusty red sash + auburn hair + off-white shirt.

08 - BRAWLER
Male chibi unarmed fighter, stocky muscular chibi build.
NO WEAPONS. Both fists raised in orthodox boxing guard (right at chin, left
forward at chest height). Dark steel gauntlets covering forearms and fists.
Bare torso. GLOWING ARCANE PURPLE TATTOO LINES - geometric line pattern
along arms and chest (NOT full-body glow, NOT body paint, dark skin shows
between lines).
Dark cloth fight pants tied at waist. Shaved or short cropped dark hair.
Palette: warm bronze skin + dark steel gauntlets + dark pants + arcane purple
tattoo glow accent.

09 - SUMMONER
Female chibi caster, slender chibi build.
Long straight dark hair, pale skin.
Soul lantern in LEFT hand at hip - iron cage, inner flame COLD CYAN with
faint violet edge.
Right hand raised at shoulder height, palm forward, small cyan-violet wisp
spirit (~4px) hovering above palm.
Long fitted dark indigo high-collar robe to ankle, cyan trim at collar and
cuffs, small bone-white charms hanging at belt.
Palette: dark indigo + cyan glow + violet accent + pale skin + bone-white trim.
ZERO GREEN.

10 - HEXER
Female chibi witch, average chibi build.
Heavy hood UP, hood interior dark BUT lower face (chin, mouth, eyes) clearly
visible - explicitly lit from below by green flame from staff.
Curse staff held in RIGHT hand, taller than character, hooked top with small
skull motif emitting CURSED GREEN FLAME (brightest light source in cell).
Skull motif ONLY on staff tip - NEVER as mask, NEVER on head, NEVER on chest.
Small bound grimoire chained at belt (secondary accent, ~6px book silhouette).
Long deep violet layered robe, weathered black trim, hanging bone charms.
Palette: deep violet + weathered black + bone-white charms + green flame glow.

=== MOB BLOCKS ===

11 - FRACTURE IMP (small mob, ~40px tall in cell)
Small hunched bipedal rift creature, knee-high to a chibi human.
Cracked dark grey stone skin, cyan-purple rift glow leaking from cracks on
back/shoulders/jaw. Large bat-like ears, cyan glowing eyes (no pupils), sharp
teeth. Long thin arms reaching past knees, three-clawed hands.
Predatory hunched stance, arms wide with claws extended.
Palette: dark stone grey + cyan-purple rift glow.

12 - RELIC CASTER (tall mob, ~55px in cell)
Tall thin cursed relic-priest, robed.
Featureless dark void inside deep hood - two pinpoint cyan eye-glows only.
Floating stone tablet (~10x8px) hovers in front of chest, cyan rune glyphs.
Both hands raised beside tablet, fingers spread in casting gesture.
Pale gaunt exposed bare chest visible at robe opening, metal plaque at sternum.
Torn ragged bone-white robe, layered shoulder mantle, dirty bronze plaques.
Palette: weathered bone-white + dirty bronze + cyan glyph glow.

13 - SEAM CRAWLER (low wide mob)
Low wide armored centipede. Width: 2x human width (cell edge to edge).
Height: knee-high (~14px tall in cell).
Stitched matte black segmented armor plates fused along back, glowing cyan
seam light between each segment. Many hooked clawed legs splayed wide both
sides. Raised armored front head with dual mandibles spread, dull cyan eye
cluster on top of head.
Palette: matte black plating + cyan-purple seam glow + dull bone joints/mandibles.

14 - RIFT HOUND (small-medium mob)
Quadruped wolf-like predator. Front half SOLID - dark grey wolf head,
forelegs, chest. Cyan glowing jaw (mouth open, light from throat).
REAR HALF dissolving into floating cyan-violet rift shards (hindquarters
fragmenting). Predatory low crouch front, dissolving rear trailing behind.
Palette: dark grey fur + cyan jaw glow + cyan-violet dissolution shards.

15 - PLATE WIDOW (elite mob, ~45x35px wide in cell)
Six-legged iron-plated arachnid (NOT eight legs). Body and legs encased in
welded weathered iron plate armor (welded TO creature, not worn).
Fused human skull remnant embedded in carapace center, eye sockets faintly
glowing cyan. Six segmented iron-shod legs, low aggressive splay.
Palette: weathered iron grey + rust + bone-white skull + cyan socket glow.

16 - HOLLOW ARBITER (sub-boss / tall elite, ~58px in cell)
Empty suit of heavy ceremonial plate armor - clearly HOLLOW, no body inside.
Tarnished iron full plate: helm, gorget, pauldrons, breastplate, faulds.
Cyan-violet void light leaks from ALL gaps (joints, neck, visor slit).
Two-handed greatsword held point-down in both gauntlets, hilt at chest
(executioner rest stance). Crown-like 3-5 short spikes on helm.
Tattered deep violet half-cape from shoulders.
Palette: tarnished iron + deep violet cape + cyan-violet void glow + faint
gold filigree on breastplate.

=== NEGATIVE PROMPT ===

NO text, labels, numbers, watermarks.
NO ground shadows, ground plane, cell borders, grid lines.
NO mature proportions, NO 5-6 head heights, NO realistic adult body.
NO super-deformed mega-chibi, NO 1-2 head height baby proportions.
NO bright Hades theatrical lighting (gritty dark fantasy tone).
NO anti-aliased outlines, NO 3D render, NO photorealistic.
NO mixed camera angles between cells.
NO southeast facing, NO southwest facing, NO three-quarter body rotation.
NO torso twist caused by held objects — body stays centered, arm extends.
NO character turning to show a weapon better — weapon visible from true south.
NO weapons mounted across back (Warblade greatsword in hand, Ranger bow in hand).
NO staff on Elementalist (floating disc only).
NO green on Summoner (cyan/violet only, ZERO green).
NO single axe on Ravager (must be DUAL).
NO female Ronin (male locked).
NO bare torso on classes other than Ravager and Brawler.
NO helmets on heroes (all bare-headed).
NO background gradients, NO color bleed between cells.
NO full robe on Elementalist (cropped top + midriff + skirt set).
```

---

## QC Checklist (rima-qc after generation)

**Class identity:**
- [ ] 01 Warblade: greatsword two-handed low guard, dark plate, crimson cape
- [ ] 02 Elementalist: NO staff, floating disc above right palm, cropped top + midriff + skirt, honey-blonde bun
- [ ] 03 Shadowblade: veil up, twin daggers reverse grip, magenta accent
- [ ] 04 Ranger: off-white half-shaved + braid, war paint, compound bow LEFT hand
- [ ] 05 Ravager: DUAL short axes (one per hand), arms spread, fur mantle
- [ ] 06 Ronin: MALE, katana sheathed LEFT HIP, right hand near grip
- [ ] 07 Gunslinger: dual pistols raised, auburn hair, trench coat
- [ ] 08 Brawler: no weapons, boxing guard, arcane purple tattoo lines
- [ ] 09 Summoner: soul lantern LEFT, cyan wisp RIGHT palm, ZERO green
- [ ] 10 Hexer: hood up + face lit, green flame curse staff RIGHT hand

**Mob identity:**
- [ ] 11 Fracture Imp: hunched, cyan rift cracks
- [ ] 12 Relic Caster: floating tablet, void hood + cyan eyes
- [ ] 13 Seam Crawler: wide low centipede, cyan seams
- [ ] 14 Rift Hound: front solid, rear dissolving cyan shards
- [ ] 15 Plate Widow: 6 legs (not 8), embedded skull, cyan sockets
- [ ] 16 Hollow Arbiter: hollow armor, void glow gaps, greatsword executioner rest

**Pipeline path after PASS:**
1. Crop individual idle frames from 256x256 sheet
2. Each crop -> PixelLab Create Character (init image) -> 4 directions
3. Custom Animation V3 per direction: idle 6f, run 6f, attack 8f, hurt 4f, death 6f, dash 4f
4. Aseprite/Photoshop cleanup pass (5-10 min/sprite)
5. Unity import: PPU=64, Point filter, no compression
