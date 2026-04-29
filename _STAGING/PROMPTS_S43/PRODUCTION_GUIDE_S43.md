# RIMA S43 — Karakter Üretim Kılavuzu
**Tarih:** 2026-04-29 · Durum: AKTİF

## Genel Kurallar
- Style desc (tüm Create jobs için aynı): `dark fantasy ARPG pixel art, thick single-color outline, chunky pixel clusters, painterly weathered shading, consistent style`
- Variation prompt (tüm Create jobs için): `Show 4 distinct idle pose variations`
- Output: 128×128 · Background removal: ON
- Edit Image sonrası QC: accent doğru yerde mi, kimlik korundu mu

---

## BÖLÜM A — EDIT IMAGE (4 karakter)

> Her adımda: mevcut anchor'ı yükle → prompt yapıştır → üret → PASS mı?

---

### A1 — SHADOWBLADE
**Tool:** Edit Image
**Upload:** `_STAGING/anchors/shadowblade/shadowblade_anchor.png`

**Prompt:**
```
Add a faint void purple (#9933CC) eye glow above the face veil — both eyes emit a very subtle purple light, visible as thin glowing slits above the veil. Keep daggers, pose, clothing, hair, and silhouette completely unchanged. No aura, no smoke cloud.
```

**QC:** Veil üstünde iki ince mor ışık görünüyor mu? Poz/kıyafet/hançerler aynı mı?

---

### A2 — RANGER
**Tool:** Edit Image
**Upload:** `_STAGING/anchors/ranger/ranger_anchor.png`

**Prompt:**
```
Add a thin gold (#FFCC00) hairline crack running along the bone bow limb from grip to upper tip — like a fracture line with faint gold light inside. Keep hair, skin, quiver, pose, chest wrap, and bow hand position completely unchanged. No glow cloud, no aura.
```

**QC:** Yay kolunda ince altın çatlak var mı? Saç/quiver/poz değişmedi mi?

---

### A3 — RONIN
**Tool:** Edit Image
**Upload:** `_STAGING/anchors/ronin/ronin_anchor.png`

**Prompt:**
```
Add a very thin pure white (#FFFFFF) hairline crack along the scabbard edge only — a single fracture line on the scabbard surface with faint white light inside. Keep katana sheathed, hand on hilt, all clothing, hair, and body pose completely unchanged. No glow cloud, no aura.
```

**QC:** Kınında beyaz ince çatlak var mı? Katana kında mı kaldı?

---

### A4 — GUNSLINGER
**Tool:** Edit Image
**Upload:** `_STAGING/anchors/gunslinger/gunslinger_anchor.png`

**Prompt:**
```
Add a small brass-yellow (#FFB800) hairline crack on each pistol barrel and cylinder mechanism — thin fracture lines with faint brass light inside, on the metal parts only. Keep hair, skin, coat, bandolier, dual lowered pistol pose, and all other details completely unchanged. No aura, no glow cloud.
```

**QC:** İki tabancada da pirinç çatlak var mı? Kızıl saç / poz değişmedi mi?

---

## BÖLÜM B — REGENERATE (5 karakter)

> Her adımda: style ref image(ler) yükle → style desc yapıştır → description yapıştır → variation prompt → üret → 4 varianttan en iyisini seç → PASS mı?

---

### B1 — BRAWLER
**Tool:** Create from Style Reference PRO
**Style refs:** Warblade anchor + Shadowblade anchor (2 slot)
**Style desc:** `dark fantasy ARPG pixel art, thick single-color outline, chunky pixel clusters, painterly weathered shading, consistent style`

**Description:**
```
Male ARPG fighter, late 20s, compact muscular pit-warrior build with grounded ARPG proportions — NOT bodybuilder, NOT fighting-game exaggerated anatomy. DEEP EBONY SKIN — near-black African skin, darkest in the roster, NOT tan, NOT bronze. Tight fade haircut with one thin geometric line shaved into the side — matches tattoo theme, NOT mohawk, NOT long. Strong jaw, broken nose, weathered scarring across one shoulder. Bare chest with charcoal-purple flat ink geometric tattoo lines — NOT glowing. Rift remnants: hairline amber fracture-cracks along left shoulder down to forearm — dimensional scar lines, like cracked dry earth with amber (#FF8800) light inside, NOT tattoo, NOT aura. Layered gear: heavy leather harness strap diagonal across chest, riveted leather belt at waist, small rift-iron buckle. Loose dust-stained charcoal trousers, knotted cloth knee wraps, scuffed leather boots. Bone-and-steel knuckle gauntlets — NOT boxing gloves — bone-white plates over dark weathered steel, leather wrist straps and fabric wrap behind plate. Material separation clear: skin, leather, cloth wrap, bone plate, steel. Boxing guard: LEFT fist near chin, RIGHT fist near sternum. Restrained amber (#FF8800) glow at knuckle contact points and rift fracture lines only. AVOID: smooth fighting-game anatomy, light/tan/bronze skin — deep ebony ONLY. Clean or long hair — tight fade ONLY. Boxing gloves, shirt, glowing tattoos.
```

**QC kriterleri:** Deep ebony cilt ✓ · Tight fade ✓ · Amber fracture sol kolda ✓ · Knuckle gauntlet ✓ · Boxing guard ✓

---

### B2 — RAVAGER
**Tool:** Create from Style Reference PRO
**Style refs:** Warblade anchor + Shadowblade anchor (2 slot)
**Style desc:** `dark fantasy ARPG pixel art, thick single-color outline, chunky pixel clusters, painterly weathered shading, consistent style`

**Description:**
```
Male blood-fury pit raider, late 20s, wide-chested heavy pit-brawler build, thick neck, broad shoulders, dense muscle — compact and grounded, NOT tall lean fighter, NOT bodybuilder, NOT a barbarian hero, NOT a Viking, NOT a fantasy warrior. Weathered tan skin, pale cold eyes, harsh blunt face. HAIR: dirty ashy-blond, unwashed matted clumps, shorter sides longer top, asymmetric — NOT clean, NOT flowing, NOT anime wind-swept. Short beard one shade darker (warm brown-tinged), patchy and unkempt. Bare chest: cracked scarified flesh at sternum — old wound channels filled with dried blood red (#FF3322), like split skin not a tattoo, NOT a rune symbol, NOT an emblem. One scrap of worn fur strapped to LEFT shoulder only — ragged and small, NOT a mantle, NOT symmetrical. Leather cross-strap harness, dark rough-cloth kilt, worn leather bracers. No shirt. Two short-handled chipped cleaver-axes, one in each hand, blade angled outward-down — utilitarian brutal weapons, NOT oversized fantasy axes, chips and dried blood on blade edge only. Blood red (#FF3322) in scar channels and axe edge stains ONLY — no glow, no aura. AVOID: barbarian, Viking, fantasy hero silhouette, clean rune graphic, symmetrical fur, oversized axes, salon hair, glowing marks.
```

**QC kriterleri:** Ashy blond matted saç ✓ · Harsh face + patchy beard ✓ · Cracked scar (not rune symbol) blood red ✓ · Sol omuz tek fur scrap ✓ · Kısa chipped cleaver-axes ✓ · RIMA style norm uyumu ✓

---

### B3 — ELEMENTALIST
**Tool:** Create from Style Reference PRO
**Style refs:** Gunslinger anchor + Ranger anchor (2 slot — kadın karakter referansları)
**Style desc:** `dark fantasy ARPG pixel art, thick single-color outline, chunky pixel clusters, painterly weathered shading, consistent style`

**Description:**
```
Female arcanist, mid-20s, lean adult build with mature proportions — NOT cute, NOT chibi. Fair skin with stern focused expression, faint shadow under eyes, narrow brow. Dull honey-blonde hair in low rounded bun at nape of neck — NOT top bun, NOT high bun, hair slightly frayed and field-worn. Muted dusty-indigo sleeveless crop top with weathered cloth wear and frayed hem, midriff bare. Short dusty-indigo miniskirt with tarnished antique gold filigree hem — gold dulled, NOT shiny. Dark charcoal tights, scuffed brown leather boots, worn fingerless leather gloves with stitching visible. Material separation clear: cloth top, leather belt, metal filigree trim. Fist-sized gold-cream orb hovering above LEFT PALM at chest height — clearly visible round glowing object. Right arm relaxed at side. No staff. Restrained gold-cream glow on orb only. Nowhere else. AVOID: cute/cartoon face, oversized round head, bright saturated colors, polished mage robe, staff, covered midriff, black hair, top bun, mobile-game palette.
```

**QC kriterleri:** Sari bun ✓ · Crop top + midriff ✓ · Orb sol avuçta ✓ · Staff yok ✓ · Mature face ✓

---

### B4 — HEXER
**Tool:** Create from Style Reference PRO
**Style refs:** Gunslinger anchor + Ranger anchor (2 slot — kadın karakter referansları)
**Style desc:** `dark fantasy ARPG pixel art, thick single-color outline, chunky pixel clusters, painterly weathered shading, consistent style`

**Description:**
```
Female player-class curse-binder, mid-20s, gaunt but combat-ready stance — slightly forward, NOT recoiled, NOT a horror NPC. Near-grey pale skin, sharp cheekbones, pale yellow irises, focused intent expression — NOT haunted, NOT vacant. Dark messy hair below cropped weathered hood — hood frames face but does NOT cover it. Asymmetric layered ritual gear: dark charcoal cloth tunic with frayed edges, dirty bone-white wrap binding around torso, exposed collarbone and forearms. Brown leather belt with bone fetishes and small ritual pouches. Charcoal curse-script tattoo marks on forearms — flat ink. Dark cloth trousers, scuffed leather boots, leather forearm bracer on right arm. Material separation clear: cloth tunic, bone-white wrap, leather belt, metal lantern frame. NOT a full robe. LARGE PHYSICAL LANTERN in LEFT HAND at chest height — real iron-frame lantern object, weathered dark metal cage, NOT an orb. Cursed yellow-green (#CCFF00) light from lantern. NO STAFF. Right hand raised slightly forward at hip with faint curse tendrils curling from fingertips. Yellow-green (#CCFF00) on lantern glow and forearm tattoo marks only. AVOID: passive recoiled NPC pose, full robe, face mask, hood covering face, single-tone black silhouette, horror-mob aesthetic, staff — LANTERN only.
```

**QC kriterleri:** Fener sol elde ✓ · Hood yüzü çerçeveler ama kapatmaz ✓ · Staff yok ✓ · NPC değil, player-class duruşu ✓

---

### B5 — SUMMONER
**Tool:** Create from Style Reference PRO
**Style refs:** Gunslinger anchor + Ranger anchor (2 slot — kadın karakter referansları)
**Style desc:** `dark fantasy ARPG pixel art, thick single-color outline, chunky pixel clusters, painterly weathered shading, consistent style`

**Description:**
```
Female field-repaired ritual commander, mid-20s, tall lean build with grounded ARPG proportions — NOT willowy fashion-runway, NOT premium gothic skin. Near-white pale skin, large dark eyes with faint necro green inner glow, weathered focused expression. Silver-white hair, slightly matted and frayed at the ends — NOT salon-smooth. Hood fully pushed back — face completely visible. Layered battle-worn ritual gear: charcoal-black funeral mantle with frayed hem and visible cloth wear, dust-stained dark grey underlayer, narrow leather corset belt at waist with rough field stitching, second small leather strap diagonal across chest. Bone fetishes and small ritual pouches at belt — chipped, NOT polished. Worn leather bracers on both forearms with patches of darker repair leather. Material separation clear: heavy mantle cloth, lighter underlayer, leather belt, bone fetish, metal staff fitting. Tall dark wooden staff in LEFT HAND — tip near ground, staff head above her head, with chipped bone tip and weathered iron banding. RIGHT HAND slightly raised, open palm forward — restrained necro green (#22FF88) summoning light in palm. Necro green (#22FF88) on staff orb, palm light, and mantle hem only. Subtle. AVOID: premium gothic mage skin, salon-smooth hair, runway-thin silhouette, polished pristine cloth, staff in right hand — LEFT only, lantern (that's Hexer), hood covering face.
```

**QC kriterleri:** Staff sol elde, yere değiyor ✓ · Hood tamamen geri ✓ · Fener yok ✓ · Near-white cilt ✓ · Necro green palm ✓

---

## Sıra Önerisi

1. A1 Shadowblade Edit → görsel paylaş
2. A2 Ranger Edit → görsel paylaş
3. A3 Ronin Edit → görsel paylaş
4. A4 Gunslinger Edit → görsel paylaş
5. B1 Brawler Regen → en iyi variant seç → paylaş
6. B2 Ravager Regen → en iyi variant seç → paylaş
7. B3 Elementalist Regen → en iyi variant seç → paylaş
8. B4 Hexer Regen → en iyi variant seç → paylaş
9. B5 Summoner Regen → en iyi variant seç → paylaş
