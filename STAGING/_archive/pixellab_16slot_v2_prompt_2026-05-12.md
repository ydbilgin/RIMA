# PixelLab Create Image Pro — 16-Slot Batch v2
# Tarih: 2026-05-12
# Değişiklikler v1'den: south-facing fix + canvas fill + Ravager dual axe + Shadowblade/Gunslinger/Ronin/Augur güçlendirme
# Kullanım: Variations = 16, aşağıdaki bloğu yapıştır

---

```
GLOBAL STYLE BLOCK (apply to ALL 16 variations):
64x64 pixel art chibi character sprite. FILL THE ENTIRE 64x64 CANVAS — head must reach near the top edge, feet near the bottom edge, body centered horizontally. No empty space padding around the character.

Camera: high top-down ARPG angle ~30-35 degrees overhead (Hades / Diablo 2 style). STRICT SOUTH-FACING — character faces directly toward the camera. Both eyes clearly visible. Face looking straight at viewer. Shoulders horizontal and perfectly symmetrical. No diagonal pose. No 3/4 view. No side-facing. No tilted torso. Centered idle stance, weight evenly distributed.

Chibi proportions: oversized head (~40% of total height), short legs, broad shoulders. Weapon integrated into sprite as 1-piece (no separate attachment point). Silhouette must be immediately readable at 64x64 and at 16px thumbnail.

Art style: hard pixel art, 1px dark outline on all edges, no anti-aliasing, no gradients, no painterly shading, no blur. Transparent background.

Palette: muted desaturated environment tones #4A4A4A / #2A2E35 / #1A2B1A as base. Neon accents #FFB000 amber / #FFF000 yellow / #00FFCC rift cyan — used sparingly per class only. Visual reference: Into Samomor (Sang Hendrix RPG Maker MZ) chibi sprite style. No 3D render, no realistic proportions, no isometric projection.

---

VARIATIONS:

1) WARBLADE — male warrior. Both hands gripping a large greatsword held low in front of body (sword tip near ground, blade crosses lower half of sprite). Dark brown leather armor #4F3A2C, brass buckle accents #C09455. Heavy stocky build. South-facing idle, both eyes visible. Sword dominates lower 45% of sprite width. Fills full 64x64 canvas.

2) RANGER — female archer. Left hand holds a compound bow at rest (arm lowered, bow vertical at left side). Slim upright build. Dark forest green armor #2A3520, cold blue trim #7BA7BC. South-facing idle, both eyes visible. Bow extends asymmetrically on left side creating left-heavy silhouette. Fills full 64x64 canvas.

3) SHADOWBLADE — male rogue. TWO short daggers in reverse-grip — both arms bent at elbows pulled back, dagger tips pointing downward behind wrists, blades clearly visible flanking torso. Near-black deep purple armor #1A1025, void purple accent #5A2A8A. Slim narrow build, hood up. South-facing idle, both eyes visible under hood. NO blade glow. Twin blades clearly separate Shadowblade from Hexer silhouette. Fills full 64x64 canvas.

4) ELEMENTALIST — female mage. No weapon. Long dark robe #2A1F35, wide at hem narrow at shoulders. Both hands raised slightly in open casting gesture, palms forward. Warm honey-blonde hair in bun. Golden accent #FFF000 at cuffs. South-facing idle, both eyes visible, face framed by hair. Wide robe hem fills lower canvas. Fills full 64x64 canvas.

5) RAVAGER — male berserker. Dual short axes — ONE axe in EACH hand (left hand + right hand), compact hatchet-sized axes (NOT massive, NOT oversized), held outward in aggressive ready stance with both arms angled out. Dark blood-red rough leather armor #3A1A0A, blood red accent #D43F3F. Massive stocky build wider than Warblade. Forward-leaning berserk posture. South-facing idle, both eyes visible. Dual axes clearly one in each hand, symmetrical threat silhouette. Fills full 64x64 canvas.

6) RONIN — male swordsman. Single katana in right hand at low ready position (blade angled down-forward). MATCHING SHEATH clearly visible on LEFT HIP (horizontal, protruding left). Dark navy near-black kimono #1A1A2E, pale gold accent #C8A87A on sword hilt and collar. Slim narrow profile. South-facing idle, both eyes visible. Katana right + sheath left = asymmetric hip silhouette. Fills full 64x64 canvas.

7) GUNSLINGER — female shooter. TWO rift-tech pistols — one in EACH hand, both arms raised slightly outward with pistols clearly flanking the torso (NOT holstered, both barrels visible). Futuristic angular barrel design. Dark grey-purple trench coat #1A1520, fire orange accent #FF4400 at collar and cuffs. Deep auburn-red hair. South-facing idle, both eyes visible. Dual pistols arms-out silhouette unmistakably wide. Fills full 64x64 canvas.

8) BRAWLER — male fighter. No weapon. Bare fists in boxing guard — left fist extended forward, right fist raised near jaw. Heavy muscular build, dark leather hand-wrappings #2A1A10, orange accent #FF8C00. Low fighting crouch. South-facing idle, both eyes visible. Raised fists create top-heavy silhouette distinct from all weapon classes. Fills full 64x64 canvas.

9) SUMMONER — female caster. Long dark staff in right hand, staff tip raised ABOVE head (top of staff near top canvas edge). Very dark green-black robe #0A1A0A. Left hand open slightly outward in conducting gesture. Neon green accents #00FF88 on staff tip and robe trim. South-facing idle, both eyes visible. Staff-above-head vertical silhouette is tallest of all classes. Fills full 64x64 canvas.

10) HEXER — female spellcaster. Dark grimoire spellbook held OPEN before chest with both hands (book spine toward viewer, pages splayed open, book center at chest height). Very dark purple-black hooded robe #1A0A1A, dark red accent #8B0000 at hood edge and page glow. Hood fully up. South-facing idle, both eyes visible under hood. Open book held outward at chest = unique front-center rectangular silhouette. Fills full 64x64 canvas.

11) SHARD WALKER MOB — crystalline humanoid creature. Sharp teal-cyan crystal shards (#00FFCC) jutting from both shoulders and spine. Muted dark grey-blue body #2A3545. Upright four-limbed posture, clawed hands at sides. South-facing idle. Crystal shoulder protrusions create jagged wide silhouette above torso. Fills full 64x64 canvas.

12) PENITENT BRUISER MOB — heavy humanoid creature. Wide heavy torso. Chain or spike wrappings on both arms (self-flagellant). Dark crimson-grey body #3A1A1A. GLOWING RED RING at ground level around feet (aura indicator #D43F3F, circle visible). Stoic forward-leaning posture, thick clawed gauntlets. South-facing idle. Red foot-ring + wide body = unmistakable mob silhouette. Fills full 64x64 canvas.

13) FRACTURE IMP MOB — small demon creature. Void-purple body #2A0A3A. Cracked glowing seams across body (rift fracture lines, hot pink #FF00AA). Jagged bat-like wings folded against back. Manic grin, clawed feet, small stature (imp fills less vertical space than humanoids — body centered in canvas, more empty space above/below). Tiny dark portal swirl near one clawed hand. South-facing. Fills full 64x64 canvas.

14) RELIC CASTER MOB — robed cultist humanoid. Dark tattered vestments #1E1E2A. Faded gold relic sigil on chest. Both hands hold a cracked arcane relic shard before body (shard glowing amber #FFB000). Hunched spellcasting posture, hood up. South-facing idle, face partially visible under hood. Glowing relic shard at chest center = key silhouette marker. Fills full 64x64 canvas.

15) RIFTBOUND AUGUR MOB — corrupted oracle humanoid. Dark teal-grey robes #1A2B2B. PROMINENT glowing teal-cyan markings #00FFCC on face (cheeks, forehead) AND both hands (visible as bright glowing patterns). Both arms raised outward in wide ominous warning gesture (arms spread creating wide silhouette). South-facing idle, glowing face marks clearly readable. Fills full 64x64 canvas.

16) HOLLOW HULK MOB — large golem creature. Very wide blocky build fills most of canvas horizontally. Cracked stone or corroded dark metal body #2A2A2A. Dark void seams with faint inner glow #5A2A8A. HOLLOW eye sockets (no pupils, black voids). Massive thick fists hanging at sides. No neck visible, head merges with shoulders. South-facing idle. Square-wide silhouette biggest of all 16 sprites. Fills full 64x64 canvas.
```

---

## Kullanım Notları

- **Variations:** 16
- **Size:** 64x64
- **Temel düzeltmeler v1'den:** south-facing direktifi güçlendirildi, canvas fill zorunlu kılındı, Ravager dual short axes, Shadowblade/Gunslinger/Ronin/Augur silhouette güçlendirildi
- **Ravager not:** Dual short compact axes (one per hand) — CLASS_SILHOUETTE_BIBLE güncellenmeli (eski: single two-handed)
- **Sonraki adım:** Batch çıktısını QC et — özellikle Ravager dual hold + south-facing kontrol
