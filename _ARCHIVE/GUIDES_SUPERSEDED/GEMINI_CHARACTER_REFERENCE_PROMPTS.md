# RIMA - Gemini South Reference Rebuild Guide (v4)
> Purpose: produce stable class concept references in Gemini for PixelLab production.
> v4 changes: South Pose Lock added, Hexer/Ravager/Gunslinger/Summoner/Brawler corrected.
> Note: this is concept/reference generation, not final pixel sprite generation.

---

## Mandatory Flow
1. Generate a single `base_south_lock.png` first.
2. Generate every class via `edit` from that base image.
3. Keep the same camera, framing, proportions, and readability for all classes.

---

## ⚠️ South Master Pose Lock (NEW — PixelLab Rotation Uyumu)

**Neden önemli:** Bu Gemini görseli PixelLab'e "Create from Reference → Rotate Character" ile verilecek.
PixelLab bu pozu kopyalayarak 8 yön (S/SE/E/NE/N/NW/W/SW) üretecek ve animasyon bazı olarak kullanacak.

**Zorunlu pose kuralları — tüm classlar için:**
- **Yön:** Kameraya (güneye) dönük, hafif forward-lean
- **Duruş:** Nötr combat-ready stance — weight her iki ayakta, hafif çömelme yok, ayaklar omuz genişliğinde
- **Birincil silah eli:** Silah vücudun yanında veya ön-aşağı hazır pozisyonda (~45°) — havaya kaldırılmış değil
- **İkincil el:** Kalça/bel seviyesinde guard veya ikinci silah
- **Kıyafet/kumaş:** Statik, uçuşan değil — fotoğraf an gibi dondurulmuş
- **Silhouette:** Yukarıdan bakıldığında kollar gövdeden ayrılır, üst üste değil
- **Kanvas:** Karakter baştan ayağa tam frame'i doldurur — baş üstte, ayaklar altta

**Yasak poses:**
- Saldırı anı (swing/slash/release)
- Dramatik cape/saç uçuşu
- Geniş açık kollar (rotation'da bozulur)
- Statik T-pose veya parade rest (combat kimliği kaybolur)

---

## Global Base Prompt (first image)

`single full-body playable character concept reference, stylized game-art readability, clean edge definition, controlled cel-like shading, muted grounded palette, not photoreal, not pixel art, high overhead top-down camera steep bird's eye view around 75-80 degree downward angle, top of head clearly visible, body foreshortened from above, full body readable, volumetric body forms with visible depth and thickness, long-leg mature proportions, neutral combat-ready stance facing toward camera, both feet planted shoulder-width, primary hand at side ready, plain neutral gray background, no environment scene, no floor detail, no dramatic cinematic lighting, consistent framing for character sheet, neutral non-gendered base face and body cues to allow strict male/female class edits later, cloth and hair static frozen in place no wind`

### Base Negative
`isometric, side view, front portrait composition, 3/4 camera wording, low top-down camera, 60-65 degree tilt, flat token look, paper cutout, sticker-like silhouette, chibi, dwarf body, oversized head, photoreal rendering, splash art action framing, heavy grimdark black crush, strong age-coded face details, dynamic swing or slash action, flying cloth, billowing cape, dramatic wind effects`

Save as:
`TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/base_south_lock.png`

---

## Edit Template (all classes)

`edit this image, keep exact same camera angle, exact same body proportions, exact same framing and zoom, exact same neutral combat-ready stance facing camera, cloth and accessories static no wind effects; only change class identity details to: [CLASS DETAILS]. keep grounded fractured-epic readability, keep volumetric body depth, no camera drift, no style drift, not photoreal, not pixel art`

### Global Negative (add to all class edits)
`gender swap, side view, isometric diamond, low top-down, 3/4 wording, pure top-down flat sprite, paper cutout, sticker-like, no depth, cinematic splash pose, giant VFX clouds, chibi proportions, oversized weapon, full-plate paladin tank silhouette, helmets, closed visors, face-concealing headgear, old-man face, overly feminine male class silhouette, overly masculine female class silhouette, dynamic action swing, flying billowing cloth, wind effects`

---

## Global Locks
- Camera lock: high top-down, 75-80 degree steep bird's eye.
- Pose lock: neutral combat-ready facing south, static cloth, weapon at side/ready.
- Gender lock:
  - Male: Warblade, Brawler, Ravager, Ronin, Shadowblade
  - Female: Elementalist, Gunslinger, Hexer, Ranger, Summoner
- Helmet lock: no helmets on any class, no exceptions.
- Warblade and Ravager: no full-plate tank silhouette.

---

## Class Details (paste into [CLASS DETAILS])

---

## Warblade (Male) — ⚠️ REGEN if elderly face present

`male fallen warblade from a broken martial order, bare head showing face clearly, battle-worn late-30s to early-40s veteran face (weathered but NOT elderly, NOT grey-haired, NOT wrinkled old-man), partial scavenged chest and shoulder armor over visible cloth and chain layers, dark crimson worn battle wrap, heavy two-handed greatsword held in both hands at hip level blade pointing forward-down in rest guard position, subtle cold-blue hairline fractures in blade metal only, grounded control-execute identity, not holy knight, not full plate, not old man`

---

## Brawler (Male) — ⚠️ REGEN if female/androgynous read or no bare torso

`male bare-torso close-combat pressure fighter, clearly masculine anatomy (square jaw, thick neck, broad shoulders, visible musculature), dark olive worn leather combat harness straps over bare chest, wrapped forearms, reinforced trousers and heavy boots, both fists raised in guard position at chest level, purple rift-scar tattoos across chest and arms with subtle inner glow only (no body-wide aura), not a bodybuilder but powerful athletic build, no weapons, no shirt, no magic storm`

---

## Elementalist (Female)

`female disciplined battlefield caster, flowing blue-purple layered robes with cyan rune accent trims, small hood that does not cover the face, crackling lightning orb held in one hand at chest-forward position, other arm relaxed at side or light guard, practical combat mage silhouette, controlled elemental intensity, no oversized spell storm, no staff, no wand, no second weapon`

---

## Gunslinger (Female) — ⚠️ REGEN if no orange hair or wearing western coat

`female rift-tech pistol duelist, clearly female read from top-down (defined feminine face structure and silhouette), long vivid copper-orange hair tied back or side-flowing clearly visible from above as key identity marker, long asymmetric dark teal duelist coat with copper-orange accent trim (NOT western cowboy coat — sleek duelist cut, asymmetric lapels, no fringes), tactical harness underneath, dual modified short pistols held at hip-ready with cold-silver rift trim on barrels, kinetic forward-lean stance not static aiming, no magic glow on hands or body, no wide-brim cowboy hat, no western badge or lasso, no masculine face cues`

---

## Hexer (Female) — ⚠️ REGEN if no lantern or staff merged into one item

`female curse specialist with pale severe calm expression, floor-length dark crimson tattered robes with fraying hem, dark wooden staff held in one hand at side, iron lantern with cursed green-purple flame held in other hand (lantern and staff are two separate items — NOT combined), slight ritual-hunched posture, black void decay tendrils curling at feet from ground, dual accent: both cursed green AND void purple present together, no glow on torso or robe, grounded occult presence`

---

## Ranger (Female)

`female tactical rift hunter from ruins and dungeons, not forest archer, charcoal-slate practical leather layers with minimal green, low tactical cowl and lower-face wrap leaving eyes visible, asymmetrical utility belt with trap canisters and tether spool, long thin rift-etched bow held in one hand at low-ready, single arrow nocked with cold-blue glowing tip only (no floating extra arrows), forward-lean stalker posture, kite-control identity over sniper hero pose, no bright green elf styling`

---

## Ravager (Male) — ⚠️ REGEN if single axe

`male brutal berserker from rift wars, bare head showing scarred aggressive face, most massive body in the entire roster (biggest widest silhouette), shirtless showing blood-red tribal scarification across chest and arms with deep inner glow (#8B1A1A blood red only — no blue, no purple), dark iron bracers and heavy trousers and boots, dual large single-headed notched axes one in each hand (dried blood on blades), both axes held at hip-ready low guard, partial heavy armor fragments at shoulders and belt only, not full plate, savage momentum identity`

---

## Ronin (Male)

`male wandering iaido duelist, visible topknot hair clearly readable from top-down, layered worn blue-green hakama robes with weathered asymmetric single shoulder guard, one katana drawn held in mid-guard position at forward angle (NOT full swing, mid-guard ready), other katana sheathed at waist left hand resting near grip, cold silver-blue shimmer along drawn blade edge only, calm pre-strike controlled tension, no fire, no purple, no oversized effects`

---

## Shadowblade (Male)

`male void assassin, dark purple-black segmented tactical armor with grey plates, standing coiled forward-lean combat crouch facing camera (not flat ground crouch — torso upright enough to read from overhead), dual short void blades extended at low-ready in both hands, controlled void-purple smoke wisps at blade edges only (thin wisps, not body-obscuring cloud), lower face wrapped in dark shadow cloth eyes visible and showing faint void-purple glow, lethal precision identity, no oversized cape, no full face mask`

---

## Summoner (Female) — ⚠️ REGEN if arcane circle on hand

`female battlefield summoner commander, worn practical purple-gold robes adapted for movement (field commander not throne-room mage), golden scepter with crystal tip held raised forward in primary hand, other hand extended open in directional command gesture, NO arcane circle or runic ring in hand or around body (summoning effects are Unity VFX only — not on this reference), cold-blue light from scepter crystal only, tiara with purple gemstone visible, field commander not ritual caster identity, mystical but gameplay-readable silhouette`

---

## Re-generation Priority List

| Class | Sorun | Aciliyet |
|-------|-------|---------|
| Hexer | Lantern+staff ayrı değilse | ⚠️ REGEN |
| Ravager | Tek balta varsa (dual olmalı) | ⚠️ REGEN |
| Gunslinger | Copper-orange saç yoksa veya western coat varsa | ⚠️ REGEN |
| Summoner | Elde arcane circle varsa | ⚠️ REGEN |
| Warblade | Yaşlı yüz okuma varsa (grey hair, wrinkles) | ⚠️ REGEN |
| Brawler | Bare torso yok veya female/androgynous read varsa | ⚠️ REGEN |
| Elementalist | Staff/wand varsa | ⚠️ REGEN |
| Ranger | Yeşil ton baskınsa veya forest-elf görünümü varsa | ⚠️ REGEN |
| Ronin | Full swing pose varsa (mid-guard olmalı) | DÜŞÜK |
| Shadowblade | Body-obscuring cloud varsa | DÜŞÜK |

---

## Quick QC (RED / ACCEPT)

### RED
- Camera drift from 75-80 high top-down.
- Dynamic action pose (swing/slash in progress).
- Flying/billowing cloth or hair in wind.
- Framing/zoom/proportion drift from base.
- Gender lock violation.
- Full-plate helmet/visor on any class.
- Warblade appears elderly or grey-haired.
- Brawler reads female, androgynous, or has shirt on torso.
- Gunslinger has no visible copper-orange hair.
- Gunslinger wears western cowboy coat/hat.
- Hexer has staff and lantern merged into one item.
- Ravager has single axe (should be dual).
- Summoner has arcane circle on hand or body.
- Elementalist has staff or wand.
- Ranger has forest-green elf styling.

### ACCEPT
- Same locked camera and framing across all classes.
- Neutral combat-ready stance facing south, cloth static.
- Clear volumetric forms and readable gameplay silhouette.
- Correct gender, weapon, and identity.
- Neutral/simplified background.
- Class fantasy readable at first glance.
- Weapon at side/ready — not mid-swing.

---

## PixelLab Handoff Naming
- `warblade_south_lock.png`
- `brawler_south_lock.png`
- `elementalist_south_lock.png`
- `gunslinger_south_lock.png`
- `hexer_south_lock.png`
- `ranger_south_lock.png`
- `ravager_south_lock.png`
- `ronin_south_lock.png`
- `shadowblade_south_lock.png`
- `summoner_south_lock.png`

---

## PixelLab Rotation Handoff Notu

Bu south_lock.png'ler PixelLab'de şu şekilde kullanılacak:
1. `Create from Reference → Rotate Character Pro`
2. Camera View: `high top-down`
3. Reference strength: ~80-100%
4. Yön zinciri: `S → SE → E → NE → N` ve `S → SW → W → NW`
5. Başarısız yönü zincire sokma — o yönü yeniden üret
6. Her onaylanan yön animasyon bazı olarak kullanılacak (Animate Character)
