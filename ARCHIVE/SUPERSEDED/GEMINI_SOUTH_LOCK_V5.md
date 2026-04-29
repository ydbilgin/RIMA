# GEMINI CONCEPT REFERENCE GUIDE -- V5
> Amac: PixelLab'e verilecek game-art concept reference gorseller (pixel art DEGIL).
> Hedef aci: ~35-40 degree ARPG overhead (Diablo 2 / PoE stili) -- target range, 2-5 derece sapma normal.
> PixelLab bu gorseli alip pixel art sprite'a donusturecek ("high top-down" = 35 derece, eslesir).
> Ciktilar: `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/`

---

## NE URETIYORUZ

**Gemini:** Dogru acida game-art concept reference gorsel uretir (pixel art DEGIL).
**PixelLab:** Bu concept gorseli "Concept Image" olarak alir, pixel art sprite'a donusturur.
**Sonuc:** Her class icin 1 onaylanan south_lock.png -> PixelLab Rotate zincirine girer -> 8 yon.

---

## HEDEF ACI (35-40 derece -- target range)

```
~35-40 degree overhead ARPG camera (2-5 derece sapma kabul edilebilir)
Classic ARPG top-down perspective (Diablo 2, Path of Exile)
Top of head slightly visible from above
Face clearly readable -- not obscured
Body slightly foreshortened -- legs shorter than sidescroller
Full body head to boots in frame
NOT sidescroller (0 degree), NOT isometric diamond grid, NOT steep bird's eye (75-80 degree)
```

---

## ADIM ADIM AKIS

```
1. Gemini'yi ac (gemini.google.com -- image generation aktif olmali)
2. BASE gorsel uret (asagidaki Base Prompt)
3. Kaydet: new_gemini/dummy_base_south_lock.png
4. Her class icin EDIT yap (asagidaki Edit Template + Class Details)
5. Kaydet: new_gemini/<classname>_south_lock.png
```

**Sira:** warblade -> brawler -> elementalist -> gunslinger -> hexer -> ranger -> ravager -> ronin -> shadowblade -> summoner

---

## BASE PROMPT (DUMMY BASE -- kimlik yok, silah yok, cinsiyet-notr)

Kopyala yapistir -- degistirme:

`single full-body character dummy base concept reference for game development, classic ARPG top-down camera angle approximately 35-40 degrees overhead similar to Diablo 2 or Path of Exile, top of head slightly visible from above, face visible but completely neutral no strong personality no age markers no expression, top-of-head dominant no portrait-like eye contact face detail de-emphasized, body slightly foreshortened with shorter legs than side-view, full body from head to boots fully in frame, stylized game-art with clean edge definition, gender-neutral mature adult proportions no strong male or female read, NO WEAPONS no items in hands both arms relaxed at sides, NO armor NO accessories NO emblems NO logos NO jewelry, plain neutral dark fitted underlayer clothing only simple dark grey tunic and plain dark trousers, both feet planted shoulder-width apart both arms hanging relaxed at sides, STRICT BILATERAL SYMMETRY no asymmetric gear no single shoulder bias no single arm raised no item in either hand, cloth completely static no wind, plain solid neutral dark gray background, no environment, no floor detail, no shadow pools, no chibi, no oversized head, no class identity, no story elements, this is a pure body proportion reference mannequin`

**Base Negative:**
`weapon, sword, axe, bow, staff, armor, shoulder pad, emblem, logo, tattoo, accessory, jewelry, helmet, cloak, cape, asymmetric gear, single shoulder bias, single arm raised, hand-item bias, sidescroller, front portrait, isometric grid, steep bird's eye, 75 degree overhead, paper cutout, sticker silhouette, chibi, dwarf, oversized head, photoreal, splash art, action swing, flying cloth, billowing cloth, dynamic wind, dramatic pose, strong personality face, old face, young face, female curves, male muscles, class-specific item`

**Kaydet:** `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/new/new_gemini/dummy_base_south_lock.png`

> **Not:** Bu dosya class_south_lock'lardan FARKLI isimle kaydediliyor. Eski `base_south_lock.png` (warrior kimlikli) `ARCHIVE`'e taşınacak.

---

## POSE LOCK (NON-NEGOTIABLE)

Her class edit'inde bunlar sabit kalir -- istisna yok:

- Same camera angle: 35-40 degree ARPG overhead
- Same pelvis orientation and shoulder line tilt
- Same feet placement and distance (shoulder-width)
- Same full-body framing (head top to boots bottom)
- Same head size ratio relative to body
- Cloth and hair static, no wind
- Only class identity (clothing, weapons, colors) may change

**HAND RULE:** Arm base angles stay close to template. Weapon type can change hand content but must NOT change overall silhouette balance or stance width. EXCEPTION: Ravager has the widest/most massive body in the roster -- bulk and volume are larger, but stance feet placement and arm angles follow the same template.

---

## EDIT TEMPLATE

Her class icin **dummy_base_south_lock.png**'yi Gemini'ye yukle, sonra su template'i kullan:

`edit this image: keep identical pose lock -- same camera angle (35-40 degree ARPG overhead), same pelvis orientation, same shoulder line, same feet placement, same full-body framing, same head size ratio; cloth and accessories static no wind; change only the class identity details to: [BURAYA CLASS DETAILS YAZ]. Keep grounded dark fantasy readability, keep volumetric body depth, no camera drift, no proportion drift, not photoreal, not pixel art.`

**Global Edit Negative (her class'a ekle):**
`gender swap, side view, isometric, steep bird's eye, dynamic action swing, flying cloth, chibi, full-plate helmet, closed visor, full-face mask (lower-face wrap allowed for Shadowblade only), western coat, arcade glow, oversized VFX, pose change, stance change, framing change`

---

## CLASS DETAILS

Her class icin yalnizca koseli parantez icindeki kismi degistir.

---

### WARBLADE (Erkek) -- [!] Yasli yuz gelirse yeniden uret

`male fallen warblade from a broken martial order, bare head showing face clearly, battle-worn veteran face late 30s to early 40s weathered but NOT elderly NOT grey-haired NOT wrinkled old-man, partial scavenged chest and shoulder armor over cloth and chain layers, dark crimson worn battle wrap, heavy two-handed greatsword: RIGHT hand upper grip LEFT hand lower grip on handle, blade tip pointing downward in front of body (vertical blade, tip toward ground not horizontal), both hands gripping hilt at hip level, blade NOT swinging NOT horizontal NOT one-handed, subtle cold-blue hairline fractures in blade metal only, not holy knight not full plate`

**Kaydet:** `new_gemini/warblade_south_lock.png`

---

### BRAWLER (Erkek) -- [!] Bare torso yoksa veya kadin/androgynous gorunuyorsa yeniden uret

`male bare-torso close-combat fighter, clearly masculine anatomy square jaw thick neck broad shoulders visible musculature, dark olive worn leather combat harness straps over bare chest, wrapped forearms, reinforced trousers and heavy boots, RIGHT fist raised at right side chest level LEFT fist raised at left side chest level, both elbows bent in boxing guard, knuckles facing forward, NO weapons in hands (fists only), purple rift-scar tattoos across chest and arms with subtle inner glow only no body-wide aura, powerful athletic build not bodybuilder, no shirt no magic storm`

**Kaydet:** `new_gemini/brawler_south_lock.png`

---

### ELEMENTALIST (Kadin)

`female disciplined battlefield caster, flowing blue-purple layered robes with cyan rune accent trims, small hood that does not cover the face, RIGHT hand extended forward at chest height with crackling lightning orb hovering above open palm, LEFT arm relaxed at side hanging naturally, orb is floating above right palm NOT gripped, practical combat mage silhouette, controlled elemental intensity, no oversized spell storm, no staff, no wand, no second weapon`

**Kaydet:** `new_gemini/elementalist_south_lock.png`

---

### GUNSLINGER (Kadin) -- [!] Turuncu sac yoksa veya western coat varsa yeniden uret

`female rift-tech pistol duelist, clearly female silhouette, long vivid copper-orange hair tied back clearly visible as key identity marker, long asymmetric dark teal duelist coat with copper-orange accent trim NOT western cowboy coat sleek duelist cut asymmetric lapels no fringes, tactical harness underneath, RIGHT hand gripping pistol at right hip barrel pointing down-forward, LEFT hand gripping pistol at left hip barrel pointing down-forward, BOTH pistols at hip-ready NOT raised NOT aimed at camera, cold-silver rift trim on barrels, kinetic forward-lean stance, no magic glow on hands or body, no wide-brim cowboy hat`

**Kaydet:** `new_gemini/gunslinger_south_lock.png`

---

### HEXER (Kadin) -- [!] Fener ve asa ayri degilse yeniden uret

`female curse specialist with pale severe calm expression, floor-length dark crimson tattered robes with fraying hem, RIGHT hand gripping dark wooden staff with tip resting on ground beside right foot (staff vertical beside body), LEFT hand holding iron lantern by its handle at waist height lantern hanging down, TWO SEPARATE ITEMS: staff in RIGHT hand lantern in LEFT hand, slight ritual-hunched posture, void decay tendrils curling at feet from ground, both cursed green AND void purple accent together, no glow on torso or robe`

**Kaydet:** `new_gemini/hexer_south_lock.png`

---

### RANGER (Kadin)

`female tactical rift hunter from ruins and dungeons, not forest archer, charcoal-slate practical leather layers minimal green, low tactical cowl and lower-face wrap leaving eyes visible, asymmetrical utility belt with trap canisters and tether spool, LEFT hand gripping bow vertically at mid-body (bow pointing up and down, bowstring facing right), RIGHT hand lightly resting on nocked arrow at rest NOT drawing, single arrow with cold-blue glowing tip only, forward-lean stalker posture, no bright green elf styling`

**Kaydet:** `new_gemini/ranger_south_lock.png`

---

### RAVAGER (Erkek) -- [!] Tek balta gelirse yeniden uret

`male brutal berserker from rift wars, bare head showing scarred aggressive face, most massive widest body in the roster biggest silhouette, shirtless showing blood-red tribal scarification across chest and arms with deep inner glow blood red only no blue no purple, dark iron bracers and heavy trousers and boots, RIGHT hand gripping large single-headed notched axe handle at right hip blade facing outward-right, LEFT hand gripping large single-headed notched axe handle at left hip blade facing outward-left, BOTH axes at low-guard handles pointing down blades at hip height, dried blood on both blades, NOT one axe NOT raised overhead, partial heavy armor fragments at shoulders and belt only, not full plate, savage momentum identity`

**Kaydet:** `new_gemini/ravager_south_lock.png`

---

### RONIN (Erkek)

`male wandering iaido duelist, visible topknot hair clearly readable from top-down camera, layered worn blue-green hakama robes with weathered asymmetric single shoulder guard, RIGHT hand gripping drawn katana blade extended forward at 45-degree angle (mid-guard, blade pointing forward-right, NOT raised overhead NOT full horizontal swing), LEFT hand resting on sheath at left hip fingers near grip (katana sheathed), cold silver-blue shimmer along drawn blade edge only, calm pre-strike controlled tension, no fire no purple no oversized effects`

**Kaydet:** `new_gemini/ronin_south_lock.png`

---

### SHADOWBLADE (Erkek)

`male void assassin, dark purple-black segmented tactical armor with grey plates, coiled forward-lean combat stance facing camera torso upright enough to read, RIGHT hand gripping short void blade extended at low-ready pointing down-right, LEFT hand gripping short void blade extended at low-ready pointing down-left, both blades at low guard angled outward NOT crossed NOT raised, controlled void-purple smoke wisps at blade edges only thin wisps not body-obscuring cloud, lower face wrapped in dark shadow cloth eyes visible with faint void-purple glow, no oversized cape, no full face mask`

**Kaydet:** `new_gemini/shadowblade_south_lock.png`

---

### SUMMONER (Kadin) -- [!] Elde arcane circle varsa yeniden uret

`female battlefield summoner commander, worn practical purple-gold robes adapted for movement field commander not throne-room mage, RIGHT hand raised gripping golden scepter at chest-to-shoulder height tip pointing upward (scepter vertical NOT horizontal), LEFT hand extended open palm facing outward in directional command gesture at waist height, NO arcane circle or runic ring in hand or around body, cold-blue light from scepter crystal only, tiara with purple gemstone visible, field commander identity not ritual caster`

**Kaydet:** `new_gemini/summoner_south_lock.png`

---

## QC -- HER GORSEL ICIN

**RED (yeniden uret):**
- Kamera acisi sidescroller veya tam karsidan portre olmus
- Aksiyon pozu -- sallanma/slash ani
- Ucusan kumas/sac
- Yanlis cinsiyet
- Helmet/visor/yuz kapatan maske
- Warblade yasli/grey-haired
- Brawler bare torso yok veya kadin gorunuyor
- Gunslinger turuncu sac yok veya western coat var
- Hexer fener+asa tek item
- Ravager tek balta
- Summoner elde arcane circle var

**ACCEPT:**
- Ayni kamera acisi tum classlarda
- Notr combat-ready durus, kumas statik
- Hacimli form, okunur silhouette
- Dogru cinsiyet ve silah
- Sinif kimligi ilk bakista okunuyor

---

## PIXELLAB'E HANDOFF

Bu south_lock.png'ler PixelLab'de:
1. Create -> Create Character
2. Camera View: `high top-down` (= 35 derece, tam bu referansla eslesir)
3. Concept Image: ilgili `<class>_south_lock.png`
4. Description'a ekle: `preserve exact pose and framing from reference`
5. Generate -> best pick -> south sprite onaylaninca Rotate zinciri: S->SE->E->NE->N ve S->SW->W->NW
6. Rotate'te fail frame zincire girmiyor -- o yonu yeniden uret
