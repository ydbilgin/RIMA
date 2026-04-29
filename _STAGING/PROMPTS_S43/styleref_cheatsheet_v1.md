# PixelLab — Create from Style Reference (PRO) Kullanım Kılavuzu
# RIMA S43 · Karakter Sprite Üretimi

## Tool Nedir?

"Create from Style Reference" aracı, yüklenen referans görsel(ler)den
stil çıkarır ve description'daki kimliği o stilde üretir.

Fark: Klasik "Create Image" araçları tüm görsel bilgiyi text'ten alır.
Bu tool'da STİL = reference image'dan, KİMLİK = description'dan gelir.
Sonuç: daha tutarlı "aynı artist" hissi, daha kısa description.

## Tool Ayarları (UI - her karakter için aynı)

| Alan                  | Değer                                                    |
|-----------------------|----------------------------------------------------------|
| Tool                  | Create from Style Reference · PRO                        |
| Maliyet               | 20 generation per run                                    |
| Çıktı                 | 128×128 px · 2×2 grid · 4 frame (idle animation strip)  |
| Background removal    | ON                                                       |

## Style Reference Images (Slot Kullanımı)

Araç maksimum 4 style reference image kabul eder. Strateji:

- Başlangıç (henüz 1 anchor): Sadece Elementalist anchor yükle
  → Dosya: _STAGING/anchors/elementalist/elementalist_anchor.png
- Warblade PASS sonrası: Elementalist + Warblade (2 slot)
  → Dosya: _STAGING/anchors/warblade/warblade_128_v7.png
- Kural: Aynı cinsiyeti ref olarak kullan (kadın karakter → Elementalist önce)
- 4 slotu doldurmak zorunlu değil — 2 güçlü anchor yeterli

## Style Description Alanı (tüm karakterler için aynı — kopyala yapıştır)

dark fantasy ARPG pixel art, thick single-color outline, chunky pixel clusters, painterly weathered shading, consistent style

## Description Alanı

Sadece karakter kimliği: ten, saç, kıyafet, silah, accent.
Stil/kamera/proporsiyon YAZMA — bunlar reference image'dan otomatik gelir.

## Çıktı Hakkında

4 frame = 128×128 idle animation strip (aynı pose, hafif varyasyon).
Variety için aynı promptu birkaç kez koştur, en iyiyi seç.
Silah/el yanlış gelirse: Aseprite'ta Image → Flip Canvas Horizontal.

## Adım Adım Kullanım

1. PixelLab → Create from Style Reference (PRO) aç
2. Style reference images: Elementalist anchor PNG yükle
3. Style description: yukarıdaki satırı yapıştır
4. Description: aşağıdan ilgili karakterin tek satırını kopyala yapıştır
5. Output: 128×128 · Background removal: ON · Üret
6. 4 frame içinden en iyisini seç, Aseprite'ta aç
7. Silah yanlış elde → Flip Horizontal
8. PASS → _STAGING/anchors/<class>/ klasörüne kaydet

---

## 01 — WARBLADE
Male heavy fighter, late 20s. Light-tan weathered skin, short dark hair, trimmed dark beard, diagonal scar on left cheek. Dark cracked steel chestplate, worn brown leather straps crossing torso, split battle-tabard at waist. Asymmetric right shoulder pauldron only. No helmet, no cape. Massive greatsword in RIGHT HAND — blade angled down at 45°, tip near right foot. Large dark steel crossguard visible. Cold blue (#66AAFF) hairline crack glow on sword fuller and one chest plate crack only. Not an aura. AVOID: raised sword, full armor, helmet, dark skin, large blue glow.

## 02 — ELEMENTALIST
Female arcanist, mid-20s, lean adult build with mature proportions — NOT cute, NOT chibi. Fair skin with stern focused expression, faint shadow under eyes, narrow brow. Dull honey-blonde hair in low rounded bun at nape of neck — NOT top bun, NOT high bun, hair slightly frayed and field-worn. Muted dusty-indigo sleeveless crop top with weathered cloth wear and frayed hem, midriff bare. Short dusty-indigo miniskirt with tarnished antique gold filigree hem — gold dulled, NOT shiny. Dark charcoal tights, scuffed brown leather boots, worn fingerless leather gloves with stitching visible. Material separation clear: cloth top, leather belt, metal filigree trim. Fist-sized gold-cream orb hovering above LEFT PALM at chest height — clearly visible round glowing object. Right arm relaxed at side. No staff. Restrained gold-cream glow on orb only. Nowhere else. AVOID: cute/cartoon face, oversized round head, bright saturated colors, polished mage robe, staff, covered midriff, black hair, top bun, mobile-game palette.

## 03 — SHADOWBLADE
Male rogue, late 20s, wiry lean build. Olive Mediterranean skin. Messy short black hair. Dark cloth veil covering nose and mouth — dark eyes visible above veil. Sharp angular jaw, deep-set mature eyes, slightly gaunt cheekbones — battle-hardened late 20s NOT youthful NOT soft-featured. Dark brown leather jacket over charcoal cloth undershirt — two distinct tones. Cloth forearm wraps, leather bracers, dark navy trousers. Slight forward lean, weight on front foot, knees softly bent — combat-ready crouch, NOT standing upright. BOTH HANDS: reverse grip daggers — blades pointing DOWN behind both wrists, edges facing outward. Arms low and relaxed at sides. Void purple (#9933CC) hairline glow on dagger edges only. Faint purple eye glow above veil. Minimal wrist smoke — hairline wisps only NOT glowing clouds. AVOID: full hood, all-black silhouette, daggers raised overhead, forward grip on either hand, upright standing pose, youthful soft face, glowing smoke clouds.

## 04 — RANGER
Female feral hunter, mid-20s, lean athletic with clearly feminine proportions — defined female figure, female chest clearly visible under bone-reinforced chest wrap, NOT masculine or androgynous silhouette. Weathered tanned skin, freckles across nose and cheeks. Platinum silver-white hair — wild and untamed, long windswept sections loose, two thin war-braids with bone beads woven in, hair disheveled and field-worn, NOT salon-smooth, NOT clean elf. Hard predator expression — jaw set, narrowed eyes locked forward, no smile — fierce apex hunter NOT cute. Prominent scar across one cheek. Minimal primitive gear: thin leather strips over shoulders, bone-reinforced chest wrap shaped to female body, exposed midriff, raw hide bindings on forearms. Side-hip quiver on RIGHT HIP only — NOT on same side as bow, NOT a back quiver. Bone-recurve bow held vertically in LEFT HAND, worn and field-repaired, no arrow nocked. RIGHT HAND relaxed at side, NOT reaching. Gold (#FFCC00) on bow limb tips and arrow fletching only. AVOID: masculine or androgynous body — female figure must be readable at 128px. Clean styled hair — wild platinum silver ONLY. Sweet or neutral expression. Hood, back quiver, arrow in hand at idle, bow in right hand.

## 05 — RAVAGER
Male berserker, late 20s, very muscular. Sunburned tan skin, pale wild eyes. HAIR MANDATORY: dirty ashy blond hair — matted, wild, medium-length past shoulders, asymmetric and windswept, NOT salon-smooth, NOT dark, NOT black. Short beard slightly darker than hair — one tone more brown-tinged. Bare scarified chest, one large geometric rune scar at sternum with blood red (#FF3322) channel glow — class scar, NOT glowing aura. Leather harness straps. Fur mantle on LEFT shoulder only. Dark kilt, leather bracers. No shirt. Dual chipped cleaver-axes, one each hand, blades angled outward-down. Blood red (#FF3322) in scar channel lines and axe edge stains only. AVOID: dark/black/brown hair — dirty ashy blond ONLY. Clean styled hair. Shirt, flames.

## 06 — RONIN
Male swordsman, late 20s, lean athletic. Medium skin, East Asian features. Calm narrow eyes, small chin scar. Black hair in loose low knot at nape. THREE dark cloth layers: charcoal gi top, dark navy-grey underlayer, dark cloth trousers — three distinct tones. Tabi boots. Broken lamellar shoulder plate remnant on right shoulder. SINGLE katana SHEATHED at LEFT HIP. Right hand on hilt. Left hand steadying scabbard. Blade NOT drawn. Pure white (#FFFFFF) hairline crack on scabbard edge only. AVOID: drawn sword, second sword, heavy armor, red robes, all-black blob, purple accent.

## 07 — BRAWLER
Male ARPG fighter, late 20s, compact muscular pit-warrior build with grounded ARPG proportions — NOT bodybuilder, NOT fighting-game exaggerated anatomy. DEEP EBONY SKIN — near-black African skin, darkest in the roster, NOT tan, NOT bronze. Tight fade haircut with one thin geometric line shaved into the side — matches tattoo theme, NOT mohawk, NOT long. Strong jaw, broken nose, weathered scarring across one shoulder. Bare chest with charcoal-purple flat ink geometric tattoo lines — NOT glowing. Rift remnants: hairline amber fracture-cracks along left shoulder down to forearm — dimensional scar lines, like cracked dry earth with amber (#FF8800) light inside, NOT tattoo, NOT aura. Layered gear: heavy leather harness strap diagonal across chest, riveted leather belt at waist, small rift-iron buckle. Loose dust-stained charcoal trousers, knotted cloth knee wraps, scuffed leather boots. Bone-and-steel knuckle gauntlets — NOT boxing gloves — bone-white plates over dark weathered steel, leather wrist straps and fabric wrap behind plate. Material separation clear: skin, leather, cloth wrap, bone plate, steel. Boxing guard: LEFT fist near chin, RIGHT fist near sternum. Restrained amber (#FF8800) glow at knuckle contact points and rift fracture lines only. AVOID: smooth fighting-game anatomy, light/tan/bronze skin — deep ebony ONLY. Clean or long hair — tight fade ONLY. Boxing gloves, shirt, glowing tattoos.

## 08 — GUNSLINGER
Female duelist, mid-20s, lean athletic. WARM LIGHT MEDIUM SKIN — sun-kissed olive-tan, warm undertone, NOT pale, NOT dark brown. Small scar on upper lip, confident relaxed smirk, one eyebrow slightly raised — cocky duelist expression. VIVID RED hair — bright vibrant red (NOT auburn, NOT dark red-brown, NOT copper-dull) — full hair, loose braid draped over one shoulder, some strands free at jaw. Dark burgundy short half-coat open over black sleeveless vest, visible midriff. Diagonal bandolier strap. Dual thigh holsters. No hat, no goggles. Two pistols — one in EACH hand, both lowered at sides. Both visible. Brass-yellow (#FFB800) on pistol cylinder and barrel mechanisms only. AVOID: pale/fair skin, dark brown skin — warm olive-tan ONLY. Dark or dull hair — vivid bright red ONLY. Hat, goggles, pistols raised, tight braid.

## 09 — HEXER
Female player-class curse-binder, mid-20s, gaunt but combat-ready stance — slightly forward, NOT recoiled, NOT a horror NPC. Near-grey pale skin, sharp cheekbones, pale yellow irises, focused intent expression — NOT haunted, NOT vacant. Dark messy hair below cropped weathered hood — hood frames face but does NOT cover it. Asymmetric layered ritual gear: dark charcoal cloth tunic with frayed edges, dirty bone-white wrap binding around torso, exposed collarbone and forearms. Brown leather belt with bone fetishes and small ritual pouches. Charcoal curse-script tattoo marks on forearms — flat ink. Dark cloth trousers, scuffed leather boots, leather forearm bracer on right arm. Material separation clear: cloth tunic, bone-white wrap, leather belt, metal lantern frame. NOT a full robe. LARGE PHYSICAL LANTERN in LEFT HAND at chest height — real iron-frame lantern object, weathered dark metal cage, NOT an orb. Cursed yellow-green (#CCFF00) light from lantern. NO STAFF. Right hand raised slightly forward at hip with faint curse tendrils curling from fingertips. Yellow-green (#CCFF00) on lantern glow and forearm tattoo marks only. AVOID: passive recoiled NPC pose, full robe, face mask, hood covering face, single-tone black silhouette, horror-mob aesthetic, staff — LANTERN only.

## 10 — SUMMONER
Female field-repaired ritual commander, mid-20s, tall lean build with grounded ARPG proportions — NOT willowy fashion-runway, NOT premium gothic skin. Near-white pale skin, large dark eyes with faint necro green inner glow, weathered focused expression. Silver-white hair, slightly matted and frayed at the ends — NOT salon-smooth. Hood fully pushed back — face completely visible. Layered battle-worn ritual gear: charcoal-black funeral mantle with frayed hem and visible cloth wear, dust-stained dark grey underlayer, narrow leather corset belt at waist with rough field stitching, second small leather strap diagonal across chest. Bone fetishes and small ritual pouches at belt — chipped, NOT polished. Worn leather bracers on both forearms with patches of darker repair leather. Material separation clear: heavy mantle cloth, lighter underlayer, leather belt, bone fetish, metal staff fitting. Tall dark wooden staff in LEFT HAND — tip near ground, staff head above her head, with chipped bone tip and weathered iron banding. RIGHT HAND slightly raised, open palm forward — restrained necro green (#22FF88) summoning light in palm. Necro green (#22FF88) on staff orb, palm light, and mantle hem only. Subtle. AVOID: premium gothic mage skin, salon-smooth hair, runway-thin silhouette, polished pristine cloth, staff in right hand — LEFT only, lantern (that's Hexer), hood covering face.

## QC Checklist (Her Üretim Sonrası)

- [ ] Warblade onaylı mı? → Elementalist + Warblade 2. ref slota ekle
- [ ] Silah doğru elde mi? (yoksa Aseprite Flip)
- [ ] Dwarf/cüce değil, canvas dolu mu?
- [ ] Transparent BG, shadow yok mu?
- [ ] Accent sadece belirtilen yerde mi?
- [ ] Kritik: Ranger BOW=LEFT · Ravager BLOND-RED · Gunslinger BROWN SKIN · Hexer LANTERN (staff değil)
