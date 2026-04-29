# ChatGPT Image 2 — Character Concept Pipeline
> RIMA | Son güncelleme: 2026-04-23 | Tüm 10 class için geçerli

## Pipeline Özeti

```
ADIM 1: Base Mannequin → kamera açısı + canvas fill + battle body geometry kilitle
         ↓ onaylandı → STAGING/base_pose_reference.png
ADIM 2: Warblade → base'i reference image yükle, class detay ekle
         ↓ onaylandı → STAGING/warblade_reference.png  
ADIM 3: Kalan 9 class → Warblade'i style anchor olarak kullan
         ↓ tümü onaylandı
ADIM 4: PixelLab Create Character → her class için 128×128 base sprite
ADIM 5: Custom Animation V3 (Keep First Frame ON) → idle 4-6f + run 6-8f
```

---

## ADIM 1 — Base Mannequin (Açı + Geometri Kilidi)

Amaç: kamera açısını ve battle start body pozunu kilitle. Karakter detayı yok.

**Neden battle start geometry?** V3, bu base'den idle (ince nefes/ağırlık kayması) ve run (diz bükümünden itme) üretecek. Nötr T-poz bu geçişleri zorlaştırır; hafif öne eğim + bent knees her iki animasyonu kolaylaştırır.

### Base Mannequin Prompt

`pixel art pose reference, featureless grey mannequin, no face no hair no clothing no texture no details no weapons, battle-ready body stance, one foot half-step forward, knees slightly bent not locked, both arms bent at elbows with hands raised to chest level in front guard position, slight forward weight lean on front foot, full body visible from head to feet, top-down ARPG overhead camera angle, slight overhead tilt with visible top of head and foreshortened legs, character fills 60-65% of canvas height centered vertically, clean solid dark background, no shadows no gradients`

### Base Mannequin QC Kriterleri

| Kriter | Olması gereken |
|--------|---------------|
| Baş tepesi | Görünür — kameradan kaçmıyor |
| Bacaklar | Hafif foreshortened, diz bükümü belli |
| Ön ayak | Açıkça yarım adım ileride |
| Kollar | Dirsekten bükülü, eller göğüs hizasında önde |
| Eğim | Hafif öne — hunched değil, alert |
| Canvas fill | %60-65 |

> **Onaylandı mı?** → Kaydet: `STAGING/base_pose_reference.png`

---

## ADIM 2 — Warblade (Style Anchor)

Base pose'u **reference image olarak yükle**. Warblade detaylarını üzerine ekle.

### Warblade Prompt

`pixel art character sprite, male warrior, same steep overhead camera angle as the reference image, camera positioned high above looking sharply down at 60 degrees, top of skull clearly visible from above, face angled downward toward ground and NOT facing the viewer directly, shoulders seen from above with foreshortening, mercenary build not a knight, dark grey cloth tunic and worn leather trousers, thick leather bracers on both forearms, single partial shoulder guard on right shoulder only, battered chest plate over cloth no decorative details, heavy leather boots, both hands gripping greatsword hilt with right hand upper near crossguard at chest height and left hand lower near pommel at stomach height, hilt held in front of the body slightly right of center, broad blade extending from the crossguard upward and to the left at 45 degrees with the tip reaching approximately to left shoulder height, high guard battle-ready stance like a downward swing is about to begin, weight on back foot knees slightly bent coiled and ready, slight controlled forward lean, cold blue glow runs uniformly along full length of both sharp blade edges from crossguard all the way to tip at equal steady brightness NO bright flare or concentration at tip only, short cropped dark brown hair, heavily scarred jaw and cheekbone, clean solid dark background, sharp pixel art silhouette, character fills 60-65% of canvas height`

### Warblade QC Kriterleri

| Kriter | Olması gereken |
|--------|---------------|
| Açı | Face downward, top of skull visible — yüz kameraya bakmamalı |
| Sword poz | **Blade UP-LEFT** — hilt önde-sağda, tip sol omuz üzerinde. Saldırı = aşağı sallanır |
| İki el | Her iki el hiltte açıkça görünüyor |
| Kıyafet | Cloth + partial armor, NO full plate, NO ornate |
| Cold blue | Blade boyunca **uniform** — uçta parlak flare DEĞİL |
| Kimlik | Mercenary, battle-worn — NOT a paladin, NOT a knight |
| Canvas fill | %60-65 |

> **Onaylandı mı?** → Kaydet: `STAGING/warblade_reference.png` — diğer 9 class için style anchor

---

## ADIM 3 — Kalan 9 Class

Her class için: **Warblade görselini reference image olarak yükle** + aşağıdaki promtu kullan.

---

### SHADOWBLADE (M)

**Poz rationale:** Upright threat stance — vücut dikey ve okunabilir, iki dagger farklı yükseklikte (sol öne-aşağı, sağ hip'te reverse grip). Horizontal/sprinter poz overhead kamerada sırtı gösterir, silhouette'i düzleştirir. Dikey duruştan: idle (ağırlık kayması, hood micro-shift), attack (öne lunge), run (sprint with daggers drawn) için temiz arc var.

`pixel art character sprite, male rogue, same camera angle and style as the reference image, lean wiry lightweight build clearly smaller than a warrior, fitted deep midnight blue leather vest with visible panel stitching and small metal clasps over dark navy cloth shirt, dark navy trousers with wrapped cloth strips around thighs, bone-white leather cross-harness straps over chest with small pouches and a sheathed backup blade tucked under left arm, close-fitting hood with loose tail falling over one shoulder, wrapped leather bracers with exposed knuckles, upright alert stance one foot half-step forward both knees slightly bent body angle nearly vertical, left arm extended forward and slightly downward with primary dagger pointing ahead at waist height long narrow blade with serrated spine, right arm bent at elbow keeping second dagger close to right hip in reverse grip blade pointing backward, body clearly upright and readable not crouched or hunched, no armor no cape, clean solid dark background, sharp pixel art silhouette, character fills 60-65% of image height`

---

### BRAWLER (M)

**Poz rationale:** High boxer guard — yumruklar yüz seviyesinde. 128px top-down'da baş etrafını saran iki yumruk anında okunur. Geniş duruş + çıplak üst vücut silhouette'i başka hiçbir classla karışmaz.

`pixel art character sprite, male fighter, same camera angle and style as the reference image, broad heavily muscular build, bare chest and bare arms no upper clothing no armor, burnt orange rust-brown leather trousers with thick belt, knee wraps and heavy boots, wide fighter stance both feet firmly planted, both fists raised high in guard at face level elbows angled outward chin slightly tucked, faint void-purple energy wrapping both knuckles and lower forearms, heavy scarred skin, tribal tattoos covering shoulders chest and upper arms, clean solid dark background, sharp pixel art silhouette, character fills 60-65% of image height`

---

### RAVAGER (M)

**Poz rationale:** Dual axes — Brawler'dan silhouette farkı bu. Brawler'ın yumrukları dar ve öne bakıyor; Ravager'ın iki baltası geniş ve asimetrik — sol omuzda, sağ elde aşağı sarkıyor. Primitive bone/war-paint detail Brawler'ın dövmeli-sportif görünümünden net ayırır.

`pixel art character sprite, male berserker, same camera angle and style as the reference image, massive hulking muscular build clearly larger and more savage than a fighter, bare upper body with rough thick fur mantle over both shoulders, simple leather trousers with fur trim, no refined armor just foot wraps, left hand gripping first greataxe with blade resting on left shoulder, right arm hanging low gripping second greataxe loosely at side blade near ground, two axes clearly visible at different heights creating wide asymmetric silhouette, extremely wide savage stance feet planted far apart, crude bone necklace and primitive war paint markings on chest, heavily scarred skin, clean solid dark background, sharp pixel art silhouette, character fills 60-65% of image height`

---

### RONIN (M)

**Poz rationale:** Iaido draw guard — kendine özgü low stance. Uzatılmış katana + kındaki katanın kabzası = iki silah ikisi de farklı yükseklikte okunuyor. Compose duruş Ravager/Brawler kaosundan ayrışıyor.

`pixel art character sprite, male wanderer, same camera angle and style as the reference image, lean composed build, torn weathered sage green cloth kimono-robe with partial iron chest plate visible underneath, no full armor, low iaido ready stance front knee bent, drawn katana extended forward and slightly downward at low diagonal right arm extended, left hand resting on the handle of second sheathed katana at left hip, weight centered slightly forward, calm and focused, thin cold silver-blue edge shimmer along drawn blade only no other color accents, clean solid dark background, sharp pixel art silhouette, character fills 60-65% of image height`

---

### ELEMENTALIST (F)

**Poz rationale:** Staff angled across body + raised casting hand. Planted staff pasif/idle enerjisi — battle start'ta staff raised ve elle enerji formlaniyor. Commanding, upright — mage kimliği savaşa girmeye hazır ama paniklememiş.

`pixel art character sprite, female mage, same camera angle and style as the reference image, slender upright build, flowing layered deep indigo cloth robes with leather belt, no armor, tall staff raised and held diagonally across body at chest-shoulder height with glowing warm amber golden orb at tip, opposite hand raised open palm facing forward with faint warm amber arcane energy tracing in the air, commanding upright battle stance weight balanced slightly back, warm honey-blonde hair in low bun, clean solid dark background, sharp pixel art silhouette, character fills 60-65% of image height`

---

### RANGER (F)

**Poz rationale:** Nocked-at-rest — string hiç çekilmemiş, yay tutulmuş, ok takılı. Full draw idle imkansız, partial draw de tuhaf durur. Serbest tutuştan: idle = yay hafif sallantı, attack = yayı kaldır → tam çek → bırak (tam arc), run = yay indirilmiş koşu. En temiz animasyon akışı bu.

`pixel art character sprite, female archer, same camera angle and style as the reference image, lean athletic hardened build, forest green leather vest over dark olive cloth shirt and dark olive trousers, leather bracers and knee guards, no full armor, recurve bow held forward in left hand at a low 45-degree angle relaxed and undrawn, arrow nocked on the string resting against the bow grip string completely slack, right hand resting lightly near the arrow nock ready to draw, body in a low alert forward-weight stance both feet planted weight slightly on front foot, cold focused predatory expression, hood tight, dark chestnut hair with a few strands loose, clean solid dark background, sharp pixel art silhouette, character fills 60-65% of image height`

---

### GUNSLINGER (F)

**Poz rationale:** Şapka kaldırıldı — auburn saç ve yüz öne çıkıyor, silhouette daha clean. Revolver yüksekte, diğer el belde = quick-draw enerji korunuyor.

`pixel art character sprite, female gunslinger, same camera angle and style as the reference image, lean build, long deep burgundy weathered coat over cloth shirt, wide leather gun belt with dual holsters, no armor, no hat, deep auburn red hair loose and slightly windswept, right hand raised holding revolver at shoulder height hammer pulled back, left hand raised holding second revolver pointed forward at chest height, both guns drawn and ready, wide aggressive stance, clean solid dark background, sharp pixel art silhouette, character fills 60-65% of image height`

---

### HEXER (F)

**Poz rationale:** İki implement farklı yükseklikte — lantern kaldırılmış (üst), asa öne uzatılmış (orta-alt). Raised lantern + forward staff = iki tehdit yönü, witch/curse mage kimliği tek bakışta anlaşılıyor. Öne eğim predatory enerji veriyor.

`pixel art character sprite, female curse mage, same camera angle and style as the reference image, gaunt slender build, floor-length dark crimson tattered cloth robes, no armor, right arm raised holding iron lantern high above shoulder level with cursed green-purple flame flickering inside, left arm extended forward gripping dark gnarled wooden staff angled downward toward ground, slight predatory forward-reaching lean, faint sickly decay tendrils rising from ground around feet, pale gaunt face, dark lank hair, clean solid dark background, sharp pixel art silhouette, character fills 60-65% of image height`

---

### SUMMONER (F)

**Poz rationale:** Commander-at-rest — mid-cast pozu değil, yetki pozu. Büyük ground circle battle-start'ta zaten çizilirse attack animasyonu için hiçbir şey kalmaz. Staff bir elde, diğer el wisp'i tutuyor = idle (wisp orbits), attack (staff yükselir, büyük circle açılır, summon çıkar) için tam arc var. Elementalist'ten ayrışma: bone aesthetic robes + skeletal wisp (amber orb değil cold blue wisp).

`pixel art character sprite, female necromancer, same camera angle and style as the reference image, composed slender build, layered dark grey-black cloth robes with prominent bone-white ribcage and spine pattern sewn into fabric, skull clasp at collar, no armor, left hand holding iron staff loosely at the left side angled slightly outward crystal tip at shoulder height, right arm raised and extended forward with open palm facing upward in a commanding gesture, small cold blue skeletal wisp spirit hovering just above the raised open palm, upright commanding stance weight balanced slightly forward, very faint minimal cold blue glow barely visible on the ground beneath the feet no large circle, cold blue only no purple no green, clean solid dark background, sharp pixel art silhouette, character fills 60-65% of image height`

---

## ADIM 4 — PixelLab Handoff

Her onaylı class concept'i PixelLab Create Character'a referans olarak ver:
- **Canvas:** `128 × 128`
- **Reference image:** İlgili class ChatGPT çıktısı
- **Style reference:** Warblade PixelLab çıktısı (Warblade bittikten sonra)
- **Prompt:** `full body centered, same scale as reference, no zoom-in, top-down ARPG view, [class kimliği 5 kelime]`

Detay: `GUIDES/CHARACTER_BASE_PRODUCTION_GUIDE.md`

---

## Kurallar (Tüm Pipeline)

- `partial armor over cloth` — full plate yasak. İstisna: Brawler (bare-chested), Ravager (fur mantle — zırh değil)
- `same camera angle as reference image` — açı referans görselle kilitleniyor, metinle değil
- Oyun adı, "dark fantasy", "isometric", "3/4 view", "eyes visible" YAZMA
- Her class onaylanmadan sonrakine geçme
- Accent renkler (sadece bunlar, başka renk accenti ekleme):

| Class | Accent | Konum |
|-------|--------|-------|
| Warblade | Cold blue | Blade boyunca ince glow |
| Brawler | Void purple | Knuckles + forearm energy |
| Ronin | Cold silver-blue | Çizilen katana kenarı |
| Hexer | Green-purple | Lantern alevi |
| Summoner | Cold blue | Crystal + ground circle |
| Ranger | Cold blue | Ok uçlarında subtle (Create Character aşamasında) |
| Elementalist | Warm amber | Staff orb + arcane energy trace |
| Shadowblade / Ravager / Gunslinger | — | Concept'te belirgin enerji accenti yok |
