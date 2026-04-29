# 8-Direction Concept Pipeline — ChatGPT Image 2
**Tarih:** 2026-04-25 · S41
**Sebep:** PixelLab Create Character SE/SW yön ayrımı zayıf (front-3/4 diagonals çakışıyor). Çözüm: ChatGPT Image 2'de 8 yönü ayrı ayrı üret, sonra PixelLab Edit Image ile pixel art clean-up + 128px hizala.

---

## Workflow

1. **ChatGPT Image 2** (web UI veya GPT-4o image gen):
   - Tek conversation aç (consistency için).
   - **REFERANS GÖRSEL ZORUNLU (her conversation'ın ilk mesajına yükle):** `Assets/Sprites/Characters/Warblade/base/warbladeNEW7.png` (Warblade E profile — bizim style + kamera açımızın anchor'u). ChatGPT'nin eye-level heroic-pose bias'ını kırmak için kritik.
   - **İlk request:** Referans görseli yükle + S-facing concept paragraf (`### S` prompt). Prompt'un başında "REFERENCE camera angle: match the exact perspective, vantage point, and camera angle of the attached reference image — high top-down ARPG view, NOT eye-level, NOT heroic poster pose" yaz.
   - **Sonraki 7 request:** "Same character, identical outfit/equipment/proportions/style. Same camera vantage as reference. Only direction changes to [X]." + ilgili yön tarifi.
   - **Çözünürlük:** ChatGPT 1024×1024 üretir; biz PixelLab Edit Image / Aseprite ile **128×128'e downsample + nearest-neighbor pixelize** yapacağız.

2. **QC her S çıktısında (devam etmeden önce):**
   - Top of head görünüyor mu? (saç ayrımı, örgü tepesi belli mi)
   - Omuzların ÜST yüzeyi görünüyor mu? (sadece front face değil)
   - Yüz foreshortened mi? (chin daha yakın, nose tip daha yakın okunuyor mu)
   - Feet head'den belirgin daha küçük mü? (perspective foreshortening)
   - Eyes frame'in ÜST 1/3'ünde mi? (center'da değil)
   - **Bunlardan biri bile FAIL ise REGENERATE.** ChatGPT eye-level'a kayar, ısrarcı ol — 3-5 deneme normal.

2. **PixelLab Edit Image** (per direction):
   - Kaynak: ChatGPT 1024px output
   - Operation: "Pixelize 128×128, palette-match RIMA muted desaturated weathered"
   - Style image: `warrior_idle_128.png` (palette anchor)

3. **Aseprite final pass:**
   - 128×128 doğrula
   - Sprite center alignment (her yön aynı pivot)
   - Palette unification (cross-direction tutarlılık)

4. **Unity import:**
   - `Assets/Sprites/Characters/[ClassName]/base/[classname]_S.png` ... vb.
   - Sprite Mode: Single, Pixels Per Unit 64

---

## Asimetrik Pose Standardı (Tüm Class'lar)

**Sorun:** İki el simetrik combat guard → SE/SW front diagonal yön ayrımı zayıflar.
**Çözüm:** Class-spesifik **asimetrik action-ready pose** — silah/aksesuar tek tarafa yaslanır, omuz rotasyonu görünür.

**Genel kural her class için:**
- Bir omuz hafif öne (action lead)
- Silah/ana aksesuar **tek tarafta dominant** (yön ipucu)
- Diğer kol/el farklı pozisyonda (asimetri)
- Ayaklar shoulder-width, biri hafif öne (stance ready, sprint değil)
- Hafif gövde lean (statik dik durmaz)
- Yüz hafif **silah yönüne** dönük (yön reinforcement)

---

## 10 Class — Asimetrik Pose Tarifleri

### 01 Warblade
**Pose:** Greatsword **sağ omuz üstünde dik resting** (tek el üst grip kabzadan, blade dikey yukarı doğru). Sol kol gevşek aşağıda, sol el yumruk şeklinde rahat. Sağ ayak yarım adım önde. Gövde hafif sola lean (ağırlık karşılama). Yüz hafif sola dönük (kılıç yönünden gelen tehdide bakar gibi).
**Identity:** disciplined heavy warrior, action-ready not combat-stance.

### 02 Elementalist
**Pose:** **Staff sol elde dikey** yere değecek şekilde tutulu (zemin desteği). Sağ el göğüs hizasında **havada açık avuç** — küçük element rune (ufak yanan/donmuş orb) parmak uçlarında belirgin. Sol ayak yarım adım önde. Gövde hafif sağa lean. Yüz sağ ele dönük (cast hazırlığı).
**Identity:** scholar of prime forces, mid-cast tension, scholarly grounded.

### 03 Shadowblade (REDESIGN)
**Pose:** **Low predator crouch** (dizleri hafif bükük, gövde 30° öne yaslı). **Sağ el reverse-grip** twin curved void-dagger pelvis hizasında aşağıya bakar. **Sol el normal-grip** dagger ileri doğru uzanmış (point/threat gesture). Sol ayak öne, sağ ayak arkada — sprinter ready. Yüz hafif öne, **veil mask** alt yüzü kapatıyor, gözler yukarı bakar.
**Identity:** phase assassin, asymmetric lethal stance, predator stillness.

### 04 Ranger (REDESIGN)
**Pose:** **Bone-recurve bow sol elde**, alt ucu yere yakın değecek şekilde **dikey** tutulu (yere desteklenmemiş, sol elde sıkı kavrama, yere yakın). **Sağ el bele bağlı yan-quiver'dan ok seçer** halde (ok yarım çekilmiş, parmak uçlarında). Sağ ayak yarım adım önde. Gövde hafif sola dönük (yay tarafına). Yüz **sağ elde tuttuğu ok'a bakar** (nişan hazırlığı).
**Identity:** rift stalker, calm alertness, ready-to-draw.

### 05 Ravager
**Pose:** **Two-handed great axe sağ omuz üstünde geniş resting** — kabza sağ elde sıkı, baltanın boğazı sol elde stabilize. Sol omuz hafif öne. Bare-chest + **fur mantle sol omuz üzerinde** (Brawler ile ayrım için kritik). Sol ayak yarım adım önde, geniş açık duruş. Gövde hafif öne lean (saldırgan).
**Identity:** brutal melee, anger barely contained, weight-laden.

### 06 Ronin
**Pose:** **Katana sol kalçada kınında (sheathed)**, sol el kın boğazında, sağ el katana kabzasında **iaido draw-ready** (henüz çekilmemiş, parmaklar kabza üstünde gevşek). Gövde hafif sola dönük (draw vector). Sol ayak yarım adım önde. Yüz öne, gözler düşük (zen alertness). Hakama + omuz üstü zırh.
**Identity:** disciplined draw-master, calm before strike.

### 07 Gunslinger
**Pose:** **Sağ pistol sağ elde aşağıya bakar** (relaxed gun-slinger rest, namlu yere yakın). **Sol pistol sol elde belde holster'a yarı yerleştirilmiş** (henüz tam holstered değil — ready-to-draw). Hat **eğri açıyla** kafanın bir tarafında. Sağ omuz hafif öne. Sağ ayak hafif geri (duelist stance). Gövde hafif sağa dönük. Yüz öne, hat gölgesi gözlerin üst yarısında.
**Identity:** dueling outlaw, ritualistic worn, dangerous calm.

### 08 Brawler
**Pose:** **Boxing guard:** sol omuz öne, sol yumruk yüksek (çene koruma, yüz hizası), sağ yumruk göğüs hizasında geri (chambered cross). **Gauntlet'ler** her iki elde kemik/metal protector belirgin. Sol ayak öne, sağ ayak arkada (sprinter/southpaw stance). Gövde hafif sağa dönük (yumruk açısı). Bare-chest + tribal scar/tattoo. Yüz öne, hafif aşağı bakış (peek-a-boo guard).
**Identity:** unrelenting boxer, footwork-ready, controlled aggression.

### 09 Summoner
**Pose:** **Soul lantern sol elde havada tutulu** (omuz hizasında, alt ucu sağ omza yakın — sallanır halde). Sağ el aşağıda, **avuç açık** (komut işareti — minyon yön belirtir). Skeleton-helm görünür (hood düşmüş veya kısmen geri). Sol ayak öne. Gövde hafif sola lean. Yüz öne, gözler lantern parıltısıyla aydınlanmış.
**Identity:** death commander, lantern as authority symbol, beckoning gesture.

### 10 Hexer
**Pose:** **Curse staff sağ elde dikey** yere değecek şekilde (üst ucunda küçük yeşil/mor sigil glow). **Sol el göğüs hizasında havada** — parmaklar curse-weave gesture (parmak araları arasında ufak void rünleri). Sağ ayak yarım adım önde. Gövde hafif sola dönük. Saç/cloak edge hafif geriye savrulmuş (curse aura ipucu). Yüz öne, gözler hafif yeşil/mor glow.
**Identity:** curse-binder, mid-incantation, two-handed asymmetry.

---

## ChatGPT Image 2 — Yön Tarifleri (Tüm Class'lar Ortak)

Her class concept'i için 8 prompt var; aşağıdaki yön açıklamaları **prompt template**'in ortak parçası:

| Yön | Açıklama (prompt'ta kullan) |
|---|---|
| **S** (south) | "**STRICTLY FRONTAL** — character facing camera directly, both shoulders square to camera plane, both hips square to camera plane, both feet pointing toward camera, head facing directly at camera, eyes looking at viewer. NO body rotation, NO torso twist, NO face angled toward weapon or hand. Pose asymmetry comes ONLY from arm placement and weapon hand, NEVER from torso/hip rotation. This is the baseline that every other direction rotates from — if S is even slightly angled, the rotation math for SE/SW/NE/NW breaks." |
| **SE** (south-east) | "3/4 front view rotated 45° clockwise, body angled toward lower-right of frame, right shoulder forward, viewer sees right side of body, left side partially occluded, face visible at 3/4 angle" |
| **E** (east) | "pure right-side profile, character facing fully to the right, full side silhouette, both legs visible in profile, weapon side toward viewer if right-handed" |
| **NE** (north-east) | "3/4 back view rotated 45° clockwise from full back, back of right shoulder leads, viewer sees back-right of torso, top of head, partial right side of face, weapon side partially visible" |
| **N** (north) | "full back view, character facing directly away from camera, viewer sees back of head, shoulders, full back silhouette, weapon held to one side visible past body silhouette" |
| **NW** (north-west) | "3/4 back view rotated 45° counter-clockwise from full back, back of left shoulder leads, viewer sees back-left of torso, top of head, partial left side of face" |
| **W** (west) | "pure left-side profile, character facing fully to the left, full side silhouette mirror of east, weapon held in right hand on far side of body but blade tip must remain visible past torso silhouette" |
| **SW** (south-west) | "3/4 front view rotated 45° counter-clockwise, body angled toward lower-left of frame, left shoulder forward, viewer sees left side of body, right side partially occluded, face visible at 3/4 angle" |

---

## Camera + Style Lock (Tüm Promptlarda Sabit)

**KAMERA — KRİTİK, AGRESİF VURGULAMA GEREKİR:**

```
CAMERA ANGLE — READ THIS WHOLE PARAGRAPH CAREFULLY, CRITICAL: We need a HIGH TOP-DOWN ARPG view. Match the exact camera angle of the attached reference image (the warrior holding a greatsword in profile). NOT a character poster. NOT eye-level. NOT a portrait. NOT a heroic splash art pose.

CAMERA POSITION (geometric tarif): Imagine the camera is positioned approximately 4 meters in front of the character and 3 meters above the character's head, looking down at them. The viewer looks DOWN at the character from this high vantage point.

VISIBLE CONSEQUENCES OF THIS ANGLE (must all be true in the output):
1. We CLEARLY SEE THE TOP OF THE CHARACTER'S HEAD — the part in the hair, the top of any braid or topknot, the crown of the skull is visible.
2. We see the TOP SURFACES of the shoulders, not just the front faces — like looking down at someone from a short ladder.
3. The face is FORESHORTENED — chin appears closer to camera than forehead, nose tip appears closer than the eyes, the bridge of the nose creates visible foreshortening.
4. The feet are SMALLER and lower in the frame than the head — perspective foreshortening makes the body taper downward in apparent size.
5. We see the BACK OF THE HANDS hanging down (looking down at hand tops, not palm fronts).
6. We see the TOPS of the boots from above (shoe top surface visible, not just the side of the boot).
7. EYES are positioned in the UPPER THIRD of the character's visible face area, NOT at face center (because we look down on the face).

YASAKLAR (DO NOT):
- DO NOT use eye-level perspective.
- DO NOT use heroic poster pose.
- DO NOT show the face straight-on at viewer eye height.
- DO NOT render the character as if posing for a magazine cover.
- DO NOT make the head and feet the same apparent size (perspective MUST taper).
- DO NOT center the eyes vertically in the face — they must be in the upper third due to the down-looking angle.
- DO NOT match Diablo 2 / Baldur's Gate 3 character splash art style (those are eye-level).

If after rendering you cannot clearly see the top of the character's head and the tops of the shoulders, the camera angle is WRONG and you must regenerate from a higher vantage point.

REFERENCE: the in-game character view from HADES 2, DIABLO 4 isometric character preview at the character creation top-down camera, PATH OF EXILE 2 in-game character close-up. That exact high-down angle. Match the attached reference image's perspective EXACTLY.

STYLE — PIXEL-ART CONVERSION FRIENDLY: Render in a simplified painterly concept-art style optimized for downsampling to 128x128 pixel art. Bold clean silhouette readable from a distance. Large readable shapes, NOT excessive fine detail. Limited color palette (5-7 main colors per character). Soft painterly shading, no photographic micro-detail, no individual thread/stitching/pore detail. Think Hades 2 character splash art simplified, or Path of Exile 2 character icon — bold clean shapes, readable at small size. NO hyper-realistic skin texture, NO individual hair strands, NO photographic fabric weave. Simplified.

OUTPUT: 1024x1024 PNG, black background only, no environment, no shadows on ground. Character centered, full body visible from feet to top of head with comfortable margin (about 10% margin around).

GENERAL: Grounded fantasy aesthetic, muted desaturated weathered look with one or two vivid accent colors per character (Hades-style vivid contrast, NOT grimdark, NOT dark fantasy genre tag, NOT anime, NOT chibi). Mature realistic body proportions (8 heads tall), no exaggerated heroic muscle, no big-head chibi.

IDENTITY CONSISTENCY: Across all 8 directions, the character must be 100% identical: same body proportions, same outfit details, same color palette exactly, same weapon design and exact size, same hairstyle, same facial features, same scars, same accessories in same positions. Only the camera viewpoint changes between directions.
```

---

## Class-Spesifik Visual Identity Locks

Her class için aşağıdaki **identity tarifi** prompt'a eklenmeli (concept ilk request'te detay, sonraki request'lerde "same as previous" referansı yeter):

### Warblade
Heavy disciplined warrior, partial steel plate over brown leather and cloth, dark steel chest plate + pauldrons, brown leather skirt panels, wrapped cloth belt. Short brown beard, weathered tan skin, short dark hair. Two-handed greatsword: long straight steel blade, leather-wrapped grip, simple cross guard. Color palette: dark steel gray + warm brown leather + ember orange accents. NO blue accents.

### Elementalist
Female scholarly mage, mid-30s mature, honey-blonde low bun, weathered light skin. Practical scholar robes — NOT noble/aristocratic, dusty travel-worn dark blue + cream cloth, leather belt with element-rune pouches, fingerless gloves. Staff: tall wooden staff with metal cap, small floating crystal orb at top (color shifts between fire-orange/frost-blue/radiance-gold based on context). Color palette: dusty indigo + cream + accent element color. NO heavy gold ornaments, NO aristocratic glamour.

### Shadowblade (REDESIGN)
Lean assassin, lithe muscular build, mid-20s. **Veil mask covers lower face from nose down** (dark cloth wrap), eyes and forehead visible, dark eyes with hint of magenta glow. Messy black hair falling over forehead. Body-fit dark void-armor, fragmented cloth wraps at waist (edges look pixel-shattered like phasing out). Belt with 4-5 small throwing daggers visible. **Twin curved void-daggers**: matte black blades, slight inward curve, hot magenta void crackle running along blade length. Color palette: pure void black + hot magenta (NOT cold purple) + worn leather brown.

### Ranger (REDESIGN)
**Wildlands rift-stalker, NOT generic Tolkien archer.** Lean athletic build, tribal hunter aesthetic. Female, mid-20s, short tight braid + half-shaved sides. **Vertical war-paint stripes** under eyes (rift-purple color). NO hood, NO cape. Bone/horn ornamented light armor (animal-bone shoulder guard on right shoulder), worn brown leather chest piece, embroidered cloth wrap across left shoulder. Side-hip leather quiver (NOT back quiver). **Bone-recurve bow**: carved from large animal bone, slight rift-purple crackle running through bone, sinew bowstring. Color palette: bone-white + worn brown leather + rift-purple accents. NO forest green.

### Ravager
Massive male barbarian, late-30s, scarred. Bare chest with tribal scarification, leather/fur kilt, **fur mantle draped over left shoulder** (Brawler differentiation). Wild long dark hair tied back roughly, full unkempt beard. Two-handed great axe: massive curved steel blade, wood haft wrapped in leather and bone fragments. Color palette: dirty bronze skin + dark fur + crimson blood accents.

### Ronin
Wandering samurai, slim wiry build, mid-30s. Dark hakama (wide pants), short kimono top in muted indigo, single shoulder armor piece on right shoulder, sash belt. Topknot hair, clean-shaven. **Katana in left-hip scabbard (sheathed)**, simple wrapped grip, small tsuba guard. Companion wakizashi visible at belt. Color palette: muted indigo + black + silver edge accents.

### Gunslinger
Female duelist, mid-20s, **deep auburn red hair** in messy braid, weathered light skin with freckles. **NOT puffy sleeves western fantasy** — instead worn dark leather long-coat (mid-thigh), dark trousers, leather chest piece. Wide-brim hat (worn brown). Bone/feather aksesuar on hat band. Rift-marked bandana around neck. **Twin pistols**: dual-grip revolvers, dark steel + brass accents, leather hip-holsters. Color palette: deep auburn hair + dark leather + brass + dusty red accents. NOT clean western, NOT saloon.

### Brawler
Tall lean-muscular male fighter, mid-20s. Bare chest with tribal arcane tattoos (purple ink, glowing slightly). Loose dark trousers, leather wrist wraps, **bone/metal gauntlets** on both hands (knuckles + back-of-hand armor visible, not full glove). Short black hair, clean-shaven. Color palette: bronze skin + dark cloth + arcane purple tattoo glow + dark steel gauntlets.

### Summoner
Tall slim necromancer, age ambiguous (gaunt). Dark hooded robe BUT hood pushed partially back revealing **skeleton-mask helm** (bleached skull-like face guard, eyes glow cyan). Tattered robe edges. **Soul lantern** held in left hand (small iron cage with cyan soul-flame inside). Belt with small bone fetishes. Color palette: void black + bone white + cyan soul-glow accents. (Hexer differentiation: Hexer = staff + green/violet, Summoner = lantern + cyan + skeleton helm.)

### Hexer
Female curse-binder, slim, late-20s. **Long dark robe with violet+green silk sigil-embroidery** edges, hood up but face visible (NOT skeleton mask — Summoner ayrımı). Dark loose hair under hood. **Curse staff** in right hand: tall twisted wood staff with floating green/violet sigil orb at top. Small floating soul-glow shapes orbit around her shoulders. Color palette: deep violet + cursed green + dark robe black. (Summoner ayrımı: yeşil + violet ikili palette + sigil orb staff vs Summoner cyan + lantern.)

---

## Kullanım

Her class için **PROMPTS_[CLASS].md** ayrı dosya yazılır. İlk 2 örnek (Ranger + Shadowblade) yazıldı: `PROMPTS_RANGER_SHADOWBLADE.md`.

Diğer 8 class için promptlar **kullanıcı onayından sonra** üretilir.
