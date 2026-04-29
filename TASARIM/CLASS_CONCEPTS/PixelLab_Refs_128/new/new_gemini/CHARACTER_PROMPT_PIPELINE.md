# RIMA — Character Concept Pipeline (Tüm 10 Class)
> **Workflow:** Gemini → concept PNG → PixelLab "Create from Reference" → pixel sprite
> **Warblade ✅ DONE** — `warrior_idle_128.png` (istisna — standart isim `warblade_idle_128.png` olurdu, bu dosya mevcut)
> **Kaydet:** Her Gemini çıktısını `Characters/[ClassName]/[class]_idle_128.png` olarak kaydet

---

> **🚨 ÖNEMLİ AÇI NOTU (CLAUDE BUNU SİLME):**  
> **Bu projedeki tüm karakter concept üretimlerinde KAMERA AÇISI "Extreme Top-Down, 80-derece" (Warrior ve Elementalist referanslarındaki gibi) OLMAK ZORUNDADIR. Karakterin GÖZLERİ ASLA GÖRÜNMEMELİDİR. Gözlerin görünmesi perspective drift (açı kayması) yaratıp karakteri önden (front-facing) gösterme eğilimi yaratır. Bu kilit formülü (Strictly use first two images + 80-degree + NO eyes visible) ASLA BOZMA VEYA DEĞİŞTİRME.**

## Style Anchor (Her Gemini Promptuna Ekle)

```
STYLE REFERENCE: The attached images define the exact visual direction.
Match this style precisely — DO NOT drift toward:
- Generic medieval skull/spike fantasy armor
- Bright colorful high fantasy (no gold plate, no rainbow magic)
- Anime or chibi proportions
- Painterly or illustrative style (this must be PIXEL ART)

REQUIRED STYLE RULES:
- Pixel art, chunky visible pixels, no anti-aliasing
- Black outlines, 1-pixel thick
- Limited palette: dark base + ONE energy accent per character (exception: Hexer has two)
- Battle-worn: torn cloaks, dented armor, rust — everything has seen combat
- Grounded proportions: tall figure, normal head size, long legs, NO exaggeration
- Single character, full body visible, transparent or plain dark background
```
Attach before each prompt: `warrior_idle_128.png` + `elementalist_idle_128.png` + `rima_style_anchor.png` from `TASARIM/CLASS_CONCEPTS/PixelLab_Refs_128/`

---

## PixelLab Ayarları (Hepsi Aynı)

| Alan | Değer |
|------|-------|
| Mode | **Create from Reference** (Gemini çıktısını yükle) |
| Preset | `male human` veya `female human` — Heroic KULLANMA |
| Camera View | `high top-down` — UI'dan ver |
| Character Size | `128px` |
| Concept Image | **Gemini çıktısı PNG** |
| Outline Style | `single color black outline` |
| Shading Style | `medium shading` |
| Detail Level | `medium detailed` |
| AI Freedom | `0` |

### Animasyon Workflow Notu
- **Idle = base sprite.** Aynı prompt, aynı concept PNG. Aseprite'ta Interpolate first+last (aynı kare) → nefes/ağırlık kayması üretilir.
- **Run = ayrı PixelLab üretimi.** Aynı concept PNG, farklı pose promptu.
- **Konsept PNG re-use:** Tüm animasyon kareleri için base concept PNG yeterli — her kare için Gemini'ye dönme.

---

## 1. WARBLADE (Erkek) ✅ DONE

**Concept PNG:** `warrior_idle_128.png` — mevcut
**PixelLab (idle/base):**
`male warrior, short dark brown hair, stubble, charcoal plate armor over chainmail, crimson waist cloth, single massive dark-iron greatsword with long hilt and blue rune etchings, idle pose, sword resting flat on top of right shoulder, blade pointing backward, right hand on hilt near crossguard, left arm relaxed near body, calm stance, no second weapon, no dual wield, full-body pixel art sprite, clear silhouette`

**Animasyon Promptları (PixelLab — ayrı üretim, aynı concept PNG):**
- **Run:** `running pose, aggressive forward lean, both hands gripping same long hilt, right hand near crossguard left hand near pommel, blade dragging on ground behind, cold-blue rune glow on blade only, heavy effort posture, no second weapon, no dual wield`
- **Stop:** `deceleration stop pose, torso still forward from run, both hands still on same hilt, blade scraping ground, front leg planting to brake, no second weapon`
- **Recover:** `recovery pose, lifting dragged blade upward from ground, right hand keeps hilt control, left hand sliding to assist lift, preparing to rest sword on shoulder, no second weapon`

---

## 2. ELEMENTALİST (Kadın) 🔥

**Concept PNG kaydet:** `elementalist_idle_128.png`

**Gemini Promptu:**
```
[ATTACH style anchor as described above]

Draw a single pixel art CHARACTER SPRITE CONCEPT for the ELEMENTALIST class of a Fractured Epic action RPG.
STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY.
Camera: EXTREME TOP-DOWN, 80-DEGREE OVERHEAD VIEW. You see ONLY the TOP of the head. NO eyes visible — if eyes appear in your output, the angle is WRONG. Shoulders from directly above, extreme vertical foreshortening, feet directly beneath body. NOT front-facing whatsoever.
Single character, full body, plain LIGHT NEUTRAL background (like soft grey or parchment) so the dark character silhouette is clearly readable and stands out.

CHARACTER: Female battle mage. Tall figure, grounded realistic proportions — not anime.
HEAD: Because of the extreme top-down angle, you only see the top of her warm honey-blonde hair pulled back loosely into a low bun. Small hood pushed back. NO eyes are visible from directly above.
OUTFIT: Deep desaturated blue-purple layered robes, worn and frayed at edges. Cyan rune trim at sleeve cuffs and hem only. Muted, weathered palette — no bright fabric. No chest plate, no heavy armor — this is a caster who moves.
WEAPONS: NO staff, NO wand. Magic is channeled directly through hands.
POSE (IDLE): Right arm extended forward, open palm facing upward. Crackling cyan-blue energy orb floating above the palm — not gripped, hovering 5cm above the hand. Left arm relaxed at side, not raised.
ENERGY: Cyan-blue crackling light on the orb only. Orb must be clearly visible OUTSIDE body silhouette. No glow on the body, no aura around the figure, no energy trails on robes.
PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing. Sprite quality.
```

**PixelLab (idle/base, tek satır):**
`female battle mage, right hand extended forward open palm with crackling cyan-blue lightning orb floating above palm not gripped, left arm relaxed at side, no staff no wand no extra weapon, orb clearly visible outside body silhouette, warm honey-blonde hair pulled back loosely into low bun, small hood not covering face, deep desaturated blue-purple layered robes, cyan rune trim at sleeves and hem, full-body pixel art sprite, clear silhouette`

**Animasyon Promptları (aynı concept PNG):**
- **Run:** `running pose, forward body lean, right arm bent inward pulling orb compressed tight against chest during sprint, orb glowing close to body not extended, cyan-blue energy on orb only no body aura, left arm pumping, mid-stride, robes trailing behind, no staff no wand`

> *Tasarım notu — Run:* Orb'un koşarken göğse çekilmesi karakterin enerjiyi sprinte harcadığını gösterir. Uzanmış el koşmayı engellerdi — göğse baskı daha doğal ve okunabilir.

**QC:**
- [ ] Kadın silhouette net
- [ ] Floating lightning orb sağ avuçta — gripped değil, silhouette DIŞINDA
- [ ] Body aura / robe glow YOK
- [ ] Staff/wand YOK
- [ ] Small hood, yüz açık
- [ ] Deep blue-purple robe, cyan trim — parlak renk YOK

---

## 3. SHADOWBLADE (Erkek) 🔥

**Concept PNG kaydet:** `shadowblade_idle_128.png`

**Gemini Promptu:**
```
[ATTACH style anchor as described above]

Draw a single pixel art CHARACTER SPRITE CONCEPT for the SHADOWBLADE assassin class of a Fractured Epic action RPG.
STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY.
Camera: EXTREME TOP-DOWN, 80-DEGREE OVERHEAD VIEW. You see ONLY the TOP of the head. NO eyes visible — if eyes appear in your output, the angle is WRONG. Shoulders from directly above, extreme vertical foreshortening, feet directly beneath body. NOT front-facing whatsoever.
Single character, full body, plain LIGHT NEUTRAL background (like soft grey or parchment) so the dark character silhouette is clearly readable and stands out.

CHARACTER: Male assassin. TALL slender athletic build — long legs, elongated torso, narrow shoulders. NOT stocky, NOT short, NOT dwarf proportions. Fast, not bulky.
HEAD: Because of the extreme top-down angle, you only see the top of his short dark hair and a sliver of the dark cloth wrapping the lower face. NO eyes are visible from directly above.
OUTFIT: Near-black segmented leather armor with charcoal-purple plate overlays, grey highlight edges on plates. Dark, matte surfaces — nothing reflective or shiny.
WEAPONS: Both hands each grip a short curved blade angled outward at sides. Both blades positioned low-ready, clearly visible OUTSIDE the body silhouette — one on each side.
POSE (IDLE): Standing perfectly still in a neutral idle stance with feet planted evenly. NOT walking, NOT stepping. Low-ready stealth stance, weight balanced. Both blades drawn, angled outward.
ENERGY: Thin wisps of void-purple smoke trailing from blade EDGES only. No body aura, no glow on armor, no purple light on the figure itself.
PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.
```

**PixelLab (idle/base, tek satır):**
`male void assassin, right hand gripping short void blade angled outward at low-ready, left hand gripping short void blade angled outward at low-ready, both void blades clearly visible outside body silhouette each blade distinct, thin void-purple smoke wisps at blade edges only no body-wide aura, lower face wrapped in dark cloth, faint void-purple glow at cloth level only, near-black segmented armor with charcoal-purple plates grey highlights, short dark hair, slight forward lean, full-body pixel art sprite, clear silhouette`

**Animasyon Promptları (aynı concept PNG):**
- **Run:** `sprinting pose, extreme low body lean forward torso near-horizontal, both void blades angled sharply back behind body trailing like predator wings, void-purple smoke wisps trailing from blade edges, legs driving hard forward, aggressive low sprint`

> *Tasarım notu — Run:* Bıçaklar sprint sırasında arkaya kapanır — bir şahinin kanatlarını katlaması gibi. Tüm vücut yere paralel eğilimi hızını anlatır. Warblade'in geri sürünen kılıcına benzer ama daha hızlı ve kontrollü.

**QC:**
- [ ] İki void blade low-ready dışa açılı, her ikisi görünüyor
- [ ] Alt yüz wrap var — full maske YOK
- [ ] Void-purple sadece gözlerde + blade wisps — vücut aura YOK
- [ ] Near-black base, grey plate highlights

---

## 4. RANGER (Kadın) 🔥

**Concept PNG kaydet:** `ranger_idle_128.png`

**Gemini Promptu:**
```
[ATTACH style anchor as described above]

Draw a single pixel art CHARACTER SPRITE CONCEPT for the RANGER dungeon hunter class of a Fractured Epic action RPG.
STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY.
Camera: EXTREME TOP-DOWN, 80-DEGREE OVERHEAD VIEW. You see ONLY the TOP of the head. NO eyes visible — if eyes appear in your output, the angle is WRONG. Shoulders from directly above, extreme vertical foreshortening, feet directly beneath body. NOT front-facing whatsoever.
Single character, full body, plain LIGHT NEUTRAL background (like soft grey or parchment) so the dark character silhouette is clearly readable and stands out.

CHARACTER: Female dungeon hunter. TALL slender athletic build — long legs, elongated torso, narrow shoulders. NOT stocky, NOT short, NOT dwarf proportions. Alert predator posture.
OUTFIT: Charcoal dark-slate leather armor — NO green anywhere. Asymmetric utility belt on one hip with small trap canisters.
HEAD: Low small tactical hood covering the top of the head. Because of the extreme top-down angle, you only see the TOP of the hood and maybe a sliver of a dark scarf below it. NO eyes are visible from directly above. A few strands of dark chestnut hair visible at edges of hood.
WEAPONS: Recurve bow held vertically in the LEFT hand at mid-body. Right hand resting lightly on a single arrow nocked to bowstring — NOT drawing, just resting. Cold-blue shimmer at the arrow tip ONLY. Quiver on back. NO dagger, NO sword, NO glowing melee weapon.
POSE (IDLE): Still neutral stance, feet planted evenly. Alert, scanning. Not mid-motion.
ENERGY: Cold-blue shimmer at arrow tip only. Nothing else glows.
PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.
```

**PixelLab (idle/base, tek satır):**
`female dungeon hunter, left hand gripping bow vertically at mid-body, right hand resting on single nocked arrow with cold-blue glowing tip not drawing at rest, bow and nocked arrow clearly visible outside body silhouette, no extra weapon, low tactical hood, charcoal dark-slate leather armor no green anywhere, asymmetric utility belt with trap canisters on one hip, quiver on back, forward-lean stalker posture, full-body pixel art sprite, clear silhouette`

**Animasyon Promptları (aynı concept PNG):**
- **Run:** `running pose, athletic forward lean, bow carried in left hand low at diagonal angle not vertical, right arm pumping freely, no arrow nocked during sprint, quiver bouncing at back, mid-stride, tactical repositioning posture`

> *Tasarım notu — Run:* Dikey yay koşarken tutulmaz — yana diagonal tutulur. Sağ el serbest ve pompalama yapar, ok hazır değil. Pozisyon değiştiren bir avcı gibi — hızlı, verimli, mesleğine özgü.

**QC:**
- [ ] Kadın silhouette net
- [ ] Yay dikey sol elde, TEK nocked arrow — cold-blue tip sadece
- [ ] Hood + alt yüz wrap var — full maske YOK
- [ ] Charcoal-slate leather — yeşil YOK
- [ ] Utility belt asimetrik, trap canisters görünüyor

---

## 5. RAVAGER (Erkek) 🔥

**Concept PNG kaydet:** `ravager_idle_128.png`

**Gemini Promptu:**
```
[ATTACH style anchor as described above]

Draw a single pixel art CHARACTER SPRITE CONCEPT for the RAVAGER berserker class of a Fractured Epic action RPG.
STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY.
Camera: EXTREME TOP-DOWN, 80-DEGREE OVERHEAD VIEW. You see ONLY the TOP of the head. NO eyes visible — if eyes appear in your output, the angle is WRONG. Shoulders from directly above, extreme vertical foreshortening, feet directly beneath body. NOT front-facing whatsoever.
Single character, full body, plain LIGHT NEUTRAL background (like soft grey or parchment) so the dark character silhouette is clearly readable and stands out.

CHARACTER: Male berserker. THE MOST MASSIVE BUILD of all characters — visibly wider and more imposing than anyone else. Shirtless.
HEAD: Very short shaved hair. Because of the extreme top-down angle, you only see the top of his shaved head and thick shoulders. NO eyes are visible from directly above.
OUTFIT: Dark heavy trousers. Charcoal iron bracers on forearms and shoulder fragments — MINIMAL armor, mostly exposed skin. No shirt, no chest plate.
MARKINGS: Blood-red tribal scarification across chest and arms — dark blood-red (#8B1A1A), NOT purple, NOT blue. Rage energy, not void.
WEAPONS: Right hand gripping a large notched battle axe at right hip, blade facing outward. Left hand gripping a large notched battle axe at left hip, blade facing outward. BOTH axes at LOW GUARD position, both clearly visible OUTSIDE body silhouette — one extending out each side. Dried blood visible on both blades.
POSE (IDLE): Aggressive low stance. Standing perfectly still in a neutral idle stance. Axes held ready at sides. Weight forward.
ENERGY: Blood-red scarification inner glow on chest and arms only — no body aura, no heat shimmer around the figure.
PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.
```

**PixelLab (idle/base, tek satır):**
`male berserker shirtless, right hand gripping large notched battle axe at right hip blade facing outward, left hand gripping large notched battle axe at left hip blade facing outward, both axes clearly visible outside body silhouette at low guard, dried blood on both axe blades, blood-red tribal scarification glowing across chest and arms deep dark-red inner glow no blue no purple, very short hair shaved sides heavily scarred aggressive face, most massive broad build in the group, charcoal iron bracers and shoulder fragments, dark heavy trousers, full-body pixel art sprite, clear silhouette`

**Animasyon Promptları (aynı concept PNG):**
- **Run:** `charging run pose, massive body weight thrown hard forward head low, right axe raised and cocked back over right shoulder ready to smash on arrival, left axe swinging low at hip for momentum balance, legs driving wide, blood-red scarification blazing on chest during charge`

> *Tasarım notu — Run:* Ravager şarj ederken bir balta indirime hazır yükselir — varışta vuracak, bu görünür olmalı. Sol balta dengeyi sağlar. Kafa aşağı, bütün vücut öne fırlamış — durdurulmaz ağırlık.

**QC:**
- [ ] En büyük/geniş build, shirtless
- [ ] İki büyük notched axe, low-guard, her ikisi silhouette dışında görünüyor
- [ ] Dried blood görünüyor
- [ ] Blood-red scarification — mor/mavi YOK, body aura YOK

---

## 6. BRAWLER (Erkek) 🔥

**Concept PNG kaydet:** `brawler_idle_128.png`

**Gemini Promptu:**
```
[ATTACH style anchor as described above]

Draw a single pixel art CHARACTER SPRITE CONCEPT for the BRAWLER void-fist fighter class of a Fractured Epic action RPG.
STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY.
Camera: EXTREME TOP-DOWN, 80-DEGREE OVERHEAD VIEW. You see ONLY the TOP of the head. NO eyes visible — if eyes appear in your output, the angle is WRONG. Shoulders from directly above, extreme vertical foreshortening, feet directly beneath body. NOT front-facing whatsoever.
Single character, full body, plain LIGHT NEUTRAL background (like soft grey or parchment) so the dark character silhouette is clearly readable and stands out.

CHARACTER: Male fighter. Athletic build — fast and strong, NOT as massive as the Ravager. Shirtless.
HEAD: Because of the extreme top-down angle, you only see the top of his short dark hair and thick neck. NO eyes are visible from directly above.
OUTFIT: Dark olive leather harness straps crossing bare chest — thin straps, not full armor. Dark trousers. Cloth wraps on forearms only.
MARKINGS: Purple glowing rift-scar tattoos on chest and arms — inner glow only, faint.
WEAPONS: NO weapons. Fists ARE the weapons. Both fists raised in tight boxing guard at chest level — knuckles forward, fingers curled.
POSE (IDLE): Standing perfectly still. Classic boxing guard. Both fists raised. Arms foreshortened from top-down.
ENERGY: Faint void-purple crackling at knuckle surfaces only — like static electricity on the skin. Not large flames, not trails.
PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.
```

**PixelLab (idle/base, tek satır):**
`male shirtless fighter, both fists raised in tight boxing guard at chest level, fingers curled knuckles forward, no weapons held in either hand, fists are the weapon, faint void-purple crackling on knuckles only, cloth wraps on forearms only not on hands, short dark hair, square jaw thick neck broad shoulders, dark olive leather harness straps across bare chest, purple glowing rift-scar tattoos on chest and arms inner glow only no body aura, dark trousers heavy boots, full-body pixel art sprite, clear silhouette`

**Animasyon Promptları (aynı concept PNG):**
- **Run:** `sprinting pose, body low and forward, both fists maintained in guard height pumping in boxer sprint rhythm, void-purple crackling intensifies on knuckles during motion, chin down, aggressive ground-eating stride, ready to punch the moment he arrives`

> *Tasarım notu — Run:* Brawler koşarken guard'ı düşürmez — yumruklar göğüs hizasında pompalıyor. Rakibe ulaştığında zaten vurmaya hazır. Boksor antrenman videolarındaki sprint pozisyonu referans.

**QC:**
- [ ] Shirtless, dark olive harness
- [ ] Her iki yumruk boks gard, elde silah YOK
- [ ] Void-purple knuckle crackling — sadece yumruklarda, büyük aura yok
- [ ] Muted purple tattoo — büyük aura yok
- [ ] Erkek okuma net
- [ ] Forearm wraps var, el yok

---

## 7. RONIN (Erkek) 🔥

**Concept PNG kaydet:** `ronin_idle_128.png`

**Gemini Promptu:**
```
[ATTACH style anchor as described above]

Draw a single pixel art CHARACTER SPRITE CONCEPT for the RONIN wandering swordsman class of a Fractured Epic action RPG.
STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY.
Camera: EXTREME TOP-DOWN, 80-DEGREE OVERHEAD VIEW. You see ONLY the TOP of the head. NO eyes visible — if eyes appear in your output, the angle is WRONG. Shoulders from directly above, extreme vertical foreshortening, feet directly beneath body. NOT front-facing whatsoever.
Single character, full body, plain LIGHT NEUTRAL background (like soft grey or parchment) so the dark character silhouette is clearly readable and stands out.

CHARACTER: Male wandering samurai. Lean, precise build — economy of movement, nothing wasted.
HEAD: Because of the extreme top-down angle, you only see the top of his black topknot hair. NO eyes are visible from directly above.
OUTFIT: Layered worn blue-green hakama robes, torn at hem and sleeve edges, heavily used. Single shoulder guard on LEFT shoulder ONLY — right shoulder unprotected, deliberate asymmetry. Dark worn leather scabbard at hip. Cloth wrapping on hands and forearms.
WEAPONS: Right hand gripping a drawn katana extended forward at 45-degree angle — mid-guard, blade toward the camera. Left hand resting on a sheathed katana at left hip, fingers near the grip. BOTH katanas must be clearly visible — drawn forward, sheathed at hip.
POSE (IDLE): Calm pre-strike mid-guard. Weight balanced. Not aggressive — patient.
ENERGY: Cold silver-blue shimmer along the CUTTING EDGE of the drawn blade only. Very subtle — like reflected cold light, not a glow. NOT a flame, NOT purple, NOT bright. The blade looks like it has been touched by something cold and unnatural.
PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.
```

**PixelLab (idle/base, tek satır):**
`male wandering swordsman, right hand gripping drawn katana extended forward at 45 degree mid-guard, left hand resting on sheathed katana at left hip fingers near grip, drawn katana forward and sheathed katana at hip both clearly visible outside body silhouette, cold silver-blue shimmer on drawn blade edge only, black topknot hair visible from above, layered worn blue-green hakama robes, single shoulder guard on left shoulder only, dark worn leather scabbard at hip, calm pre-strike stance, full-body pixel art sprite, clear silhouette`

**Animasyon Promptları (aynı concept PNG):**
- **Run:** `running pose, swift controlled forward lean, drawn katana held low at right side trailing slightly behind body, left hand pressing firmly against sheathed katana scabbard at hip preventing bounce during run, cold silver-blue shimmer on drawn blade edge only, light precise footwork mid-stride, no wasted motion, robes flowing back`

> *Tasarım notu — Run:* Sol el kını sabitler — Japon kılıç taşıma geleneğinde koşarken kın tutulur aksi halde bouncing olur. Çekili katana alçakta geri süzülür. En sessiz, en kontrollü koşu — hiçbir hareket fazla değil.

**QC:**
- [ ] Topknot saç görünüyor
- [ ] Sağ elde çekilmiş katana 45° öne
- [ ] Sol elde kında katana — ikisi ayrı readable
- [ ] Cold silver-blue sadece blade kenarında
- [ ] Blue-green hakama base, torn edges

---

## 8. GUNSLINGER (Kadın) 🔥

**Concept PNG kaydet:** `gunslinger_idle_128.png`

**Gemini Promptu:**
```
[ATTACH style anchor as described above]

Draw a single pixel art CHARACTER SPRITE CONCEPT for the GUNSLINGER pistol duelist class of a Fractured Epic action RPG.
STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY.
Camera: EXTREME TOP-DOWN, 80-DEGREE OVERHEAD VIEW. You see ONLY the TOP of the head. NO eyes visible — if eyes appear in your output, the angle is WRONG. Shoulders from directly above, extreme vertical foreshortening, feet directly beneath body. NOT front-facing whatsoever.
Single character, full body, plain LIGHT NEUTRAL background (like soft grey or parchment) so the dark character silhouette is clearly readable and stands out.

CHARACTER: Female pistol duelist. Lean athletic build. Forward-leaning duelist stance.
HEAD: Because of the extreme top-down angle, you only see the top of her deep auburn red hair tied back. NO eyes are visible from directly above.
OUTFIT: Long asymmetric charcoal-dark-teal duelist coat — one side longer than the other, aged at the edges. Aged copper trim on coat lapels only — NOT gold trim. Tactical harness visible beneath open coat. Dark practical trousers, boots.
WEAPONS: Right hand gripping compact pistol at right hip — barrel pointing downward-forward. Left hand gripping compact pistol at left hip — barrel pointing downward-forward. BOTH pistols clearly visible, metal barrels showing — clearly GUNS, not daggers or wands.
POSE (IDLE): Relaxed but ready stance. Pistols at hips, not raised. Slight forward lean.
ENERGY: NO glowing hands. NO purple energy. NO magical effects on the figure. The supernatural is in the bullets — invisible until fired. She looks like a gunfighter, not a mage.
PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.
```

**PixelLab (idle/base, tek satır):**
`female pistol duelist, right hand gripping compact pistol at right hip barrel pointing downward-forward, left hand gripping compact pistol at left hip barrel pointing downward-forward, both pistols clearly visible outside body silhouette clearly guns with metal barrels not daggers, deep auburn red hair tied back, long asymmetric charcoal-dark-teal duelist coat with aged copper trim on lapels, tactical harness visible under coat, forward-leaning stance, full-body pixel art sprite, clear silhouette`

**Animasyon Promptları (aynı concept PNG):**
- **Run:** `running pose, forward lean, both compact pistols drawn and held at mid-chest height while running, charcoal-dark-teal coat swept wide open behind from sprint speed, mid-stride, deep auburn red hair streaming back, ready to fire while moving, no magical glow anywhere`

> *Tasarım notu — Run:* Tabancalar koşarken zaten çekilmiş — o durur, ateş eder, koşmaya devam eder. Ceket hızından açılır, dramatik silhouette verir. Sihirli ışık yok — bu bir tetikçi, bir büyücü değil.

**QC:**
- [ ] Kadın silhouette net
- [ ] İki tabanca kalçada, metal barrel görünüyor — dagger değil
- [ ] Deep auburn red saç (kızıl) — sarı/orange/copper YOK
- [ ] Charcoal-dark-teal coat — parlak/vibrant YOK
- [ ] Aged copper trim — altın/sarı YOK
- [ ] Ellerde glow/enerji YOK

---

## 9. HEXER (Kadın) 🔥

**Concept PNG kaydet:** `hexer_idle_128.png`

**Gemini Promptu:**
```
[ATTACH style anchor as described above]

Draw a single pixel art CHARACTER SPRITE CONCEPT for the HEXER curse mage class of a Fractured Epic action RPG.
STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY.
Camera: EXTREME TOP-DOWN, 80-DEGREE OVERHEAD VIEW. You see ONLY the TOP of the head. NO eyes visible — if eyes appear in your output, the angle is WRONG. Shoulders from directly above, extreme vertical foreshortening, feet directly beneath body. NOT front-facing whatsoever.
Single character, full body, plain LIGHT NEUTRAL background (like soft grey or parchment) so the dark character silhouette is clearly readable and stands out.

CHARACTER: Female curse mage. Tall, thin figure. Pale skin. She moves slowly and deliberately — nothing rushed.
HEAD: Because of the extreme top-down angle, you only see the top of her dark hair. NO eyes are visible from directly above.
OUTFIT: Ankle-length dark crimson robes, tattered and fraying at the hem — dragging on ground slightly. Worn leather belt at waist. Muted dark crimson, NOT bright red.
WEAPONS (TWO SEPARATE ITEMS — both must be visible):
  ITEM 1: Right hand gripping a dark wooden staff, tip resting vertically on the ground beside her right foot — staff standing tall beside her.
  ITEM 2: Left hand holding an iron lantern by its handle at waist height — lantern hanging downward from handle. Inside the lantern, a cursed flame that is BOTH green and purple simultaneously — mixed, not alternating.
POSE (IDLE): Staff in right hand, planted on ground. Lantern in left hand, hanging at waist. Standing still, weight slightly on the staff side.
GROUND EFFECT: Void decay tendrils — thin dark wisps curling from the ground at her feet. Ground-level only.
ENERGY: Green-purple mixed flame INSIDE lantern only. No glow on robes, no body aura.
PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.
```

**PixelLab (idle/base, tek satır):**
`female curse mage, right hand gripping dark wooden staff tip resting on ground vertically beside right foot, left hand holding iron lantern by handle at waist height lantern hanging down with green-purple flame inside, staff and lantern are two separate items both clearly visible outside body silhouette, void decay tendrils curling at feet from ground, no glow on body or robes, dark hair pale skin severe expression, ankle-length dark crimson tattered robes worn leather belt, full-body pixel art sprite, clear silhouette`

**Animasyon Promptları (aynı concept PNG):**
- **Run:** `running pose, dark crimson robes billowing and trailing dramatically behind, staff raised vertically in right hand lifted off ground during motion, iron lantern swinging forward in left hand as she strides with green-purple mixed flame inside lantern only, void tendrils trailing at feet even in motion, heavy deliberate stride not frantic, robes fanning outward`

> *Tasarım notu — Run:* Hexer koşmaz — sürüklenir. Yavaş kasıtlı adımları bile "koşu" olarak okunur çünkü robes büyük billowing efekt yapar. Asa yerden kalkar (artık sürünmüyor), fener öne sallanır. Yarı-çağrı yarı-koşu estetiği — kendine özgü.

**QC:**
- [ ] Kadın silhouette net
- [ ] Sağ elde dikey tahta asa — yerde duruyor
- [ ] Sol elde demir fener — hem yeşil HEM mor alev
- [ ] İkisi AYRI item, her ikisi görünüyor
- [ ] Ayak çevresinde void tendriller (ground level)
- [ ] Vücut/robe'da glow YOK

---

## 10. SUMMONER (Kadın) 🔥

**Concept PNG kaydet:** `summoner_idle_128.png`

**Gemini Promptu:**
```
[ATTACH style anchor as described above]

Draw a single pixel art CHARACTER SPRITE CONCEPT for the SUMMONER battlefield commander class of a Fractured Epic action RPG.
STRICTLY USE THE FIRST TWO ATTACHED CHARACTER IMAGES (warrior_idle_128.png AND elementalist_idle_128.png) AS YOUR CAMERA ANGLE REFERENCE. YOU MUST MATCH THEIR PERSPECTIVE EXACTLY.
Camera: EXTREME TOP-DOWN, 80-DEGREE OVERHEAD VIEW. You see ONLY the TOP of the head. NO eyes visible — if eyes appear in your output, the angle is WRONG. Shoulders from directly above, extreme vertical foreshortening, feet directly beneath body. NOT front-facing whatsoever.
Single character, full body, plain LIGHT NEUTRAL background (like soft grey or parchment) so the dark character silhouette is clearly readable and stands out.

CHARACTER: Female battlefield commander. Tall, upright posture — presence through stance, not size. Commander identity, not necromancer.
HEAD: Because of the extreme top-down angle, you only see the top of her brown hair and purple gemstone tiara. NO eyes are visible from directly above.
OUTFIT: Worn muted purple robes with aged gold-bronze trim. NOT bright purple, muted and weathered. NOT bright gold, aged bronze. Armored shoulder piece on left shoulder. Multiple robe layers visible.
WEAPONS: Right hand raised and gripping a golden scepter vertically — tip pointing UPWARD, clearly visible above head silhouette. The scepter has a crystal at the tip with a cold-blue glow — the ONLY energy effect. Left hand extended forward at waist height, open palm in a command gesture — pointing/directing.
POSE (IDLE): Right arm raised with scepter. Left arm forward in commanding gesture. Upright, authoritative. Field general stance.
ENERGY: Cold-blue glow at the scepter crystal tip ONLY. No arcane circles, no summoning effects, no energy at hands.
PIXEL ART: Chunky visible pixels, black 1px outlines, limited palette, no anti-aliasing.
```

**PixelLab (idle/base, tek satır):**
`female battlefield commander, right hand raised gripping golden scepter vertically tip pointing upward clearly visible outside silhouette, left hand extended open palm in command gesture at waist height, no arcane circles no summoning portals no extra weapon, cold-blue glow at scepter crystal tip only, brown hair, purple gemstone tiara on head, worn muted purple robes with aged gold-bronze trim, armored left shoulder piece, field commander posture, full-body pixel art sprite, clear silhouette`

**Animasyon Promptları (aynı concept PNG):**
- **Run:** `running pose, authoritative forward stride, scepter gripped in right hand angled forward and upward like a general leading a charge, left arm pumping with purpose, upright commander posture maintained even at sprint, robe layers flowing back, cold-blue scepter tip glowing`

> *Tasarım notu — Run:* Summoner koşsa da general gibi koşar. Asa öne açılı tutulur — sanki birlikleri yönlendiriyor. Robes geriye akar. Diğer classlara göre en dik gövde — çünkü o komuta eder, kaçmaz.

**QC:**
- [ ] Kadın silhouette net
- [ ] Sağ elde dikey scepter — tip yukarı, silhouette dışında görünüyor
- [ ] Sol elde açık avuç komuta jesti
- [ ] Arcane circle / portal YOK
- [ ] Tiara + purple gemstone görünüyor
- [ ] Cold-blue sadece scepter kristalinde

---

## Üretim Sırası (Önerilen)

| Sıra | Class | Neden |
|------|-------|-------|
| 1 | Elementalist | En temiz silhouette, kolay referans |
| 2 | Gunslinger | İki tabanca readable olması kritik |
| 3 | Shadowblade | Near-black armor zor — erken test et |
| 4 | Brawler | No-weapon class — enerji yerleşimi test |
| 5 | Ranger | Hood + bow + arrow üç eleman |
| 6 | Hexer | İki item (staff+lantern) en karmaşık |
| 7 | Summoner | Scepter yönü top-down'da zor |
| 8 | Ronin | İki katana ayrımı |
| 9 | Ravager | En kolay — büyük silhouette, net pose |
