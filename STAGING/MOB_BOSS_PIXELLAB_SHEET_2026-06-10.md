# MOB + BOSS PIXELLAB PROMPT SAYFASI — 2026-06-10

> Kullanici uretir. Claude indirir + Unity'e baglar.
> VERIMI: gorseli URETME, sadece sayfa yaz. Bu dosya referans.

---

## REFERANS ANALİZİ — Legacy Sprite Bulgulari (Mobs/_archive_legacy_S86/)

### Canvas ve Cozunurluk
- Tum legacy sprite'lar: **64x64 piksel** canvas, transparan arka plan
- Dosya boyutlari kucuk (1.3-3.0 KB) = duz renk, siki piksel yogunlugu, minimal dithering
- PPU tahmini: 64 (128px world-unit'te 1x1 karakter kaplayan sprite)

### Perspektif ve Kamera
- **High top-down, yaklasik 60-70 derece tepeden** — RIMA'nin kilitli kamera acisiyla tam eslesme
- Karakterler "hafif asagidan gorulur" degil, **dik tepeden bakan** bir acida tasarlanmis
- Golge: karakter altinda degil, gomlek/zirh ic-yuzeylerde (tepeden aydinlatma)
- Hicbir sprite'ta goz/yuz gormek zor — beklendik, tepeden perspektif hides eyes naturally

### Karakter Oranlari
- **Chibi oranlari**: buyuk bas, kisa govde, kisa bacaklar — oyuncu karakterleriyle tutarli
- Yaklasik bas:govde orani = 1:1.8 (normal insan 1:7.5'ye kiyasla cok kompakt)
- Tip eslesme: kucuk mob (Rift Acolyte, Spire Choirling) ~40px tall silhuet; buyuk mob (Hollow Hulk, Hollow Arbitter) ~56px tall
- Boss eslesme: mevcut sprite'larda 64x64 canvas siniri — boss icin 160x160 gerekli

### Outline Stili
- **1px siyah outline**, tutarli, tum silhuet cevresi — seckin ARPG okunurlugu
- Ic detaylar icin ikincil koyu renk (outline rengi degil, ana rengin 2 ton karanligi)
- Anti-aliasing YOKTUR — crisp, net piksel siniri

### Palet ve Golgeleme
- **Muted, desature ton skali** — parlak neon YOK, soguk griler hakim
- Renk per sprite: 6-12 benzersiz renk (renk ekonomisi yuksek)
- Golgeleme: **3 ton merdiveni** (highlight / orta / golge) — dithering yok, flat shading
- **Cyan (#00FFCC veya yakin) vurgular** — Rift Acolyte mor orb, Shard Walker kristal, FractureImp mavi gozler
- Void/koyu tonlar on planda: arduvaz gri, is karasi, soluk kahve
- Sicak turuncu/altin yalnizca relic/boss sinifinda

### Yon Sistemi
- Legacy sprite'larin cogu **tek yon (guney/on bakis)** — bunlar S43 oncesi eski uretim
- **Yeni uretim: 8 yon ZORUNLU** (Karar #114, 2026-05-13)
- 5 uret (S, SE, E, NE, N), 3 Unity'de mirror (W<-E, SW<-SE, NW<-NE)

### RIMA Estetigine Uyumluluk
- Soguk-melankolik ton: UYUMLU (tum sprite'lar siyah/koyu agirlikli)
- Cyan-Rift vurgusu: KISMEN VAR (Shard Walker kristal, Acolyte orb) — yeni uretimde standardize edilmeli
- Yuzebe ada / mabetsi dunya hissi: sprint giy/zirh detaylari bunu destekliyor (kircik, worn, ritual)
- Insan/humanoid + yaratik karisimi: DENGE TUTARLI (6 humanoid + 6 creature arketip)

---

## STİL SPEC BLOGU (her uretimde kopyala-yapistir)

> Bu blogu her mob/boss uretiminde sabit tut. Prompt basina ekle.

```
STIL SPEC (RIMA kanon):
- Canvas: 64x64 transparan (mob) / 160x160 transparan (boss)
- Camera: high top-down ~65 degree, MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png.
  The character has normal eyes facing forward -- not looking up at the camera.
  The steep overhead angle hides the eyes naturally.
- View: high top-down
- Outline: 1px black outline, crisp pixel art, no anti-aliasing
- Shading: 3-tone flat shading, no dithering, no gradients
- Palette: muted desaturated palette, weathered field-worn, cold greys and slates dominant,
  single cyan (#00FFCC) accent for Rift energy only, no neon, no warm orange on combat mobs
- Proportions: chibi -- large head, short torso, short legs, compact silhouette
- Background: transparent
- Style: RPG top-down pixel art, Children of Morta / Hades palette mood
```

---

## BÖLÜM 1 — DEMO SET (öncelikli, 3 mob — demo'da aktif)

Demo'da zaten animasyonlu olan: FractureImp, HalfThrall, Penitent.
Asagidaki 3 mob S43 roster'dan gelen bir sonraki kademe.

---

### M1 · SHARD WALKER (canon boyut: 112px, demo tier 1 mob)

**Tasarim ozeti (legacy 12_shard_walker.png referansi):**
Humanoid ama kristal "kiric" seklinde uzuvlara sahip; govdesi mat siyah ile donuk cyan arasi
bir renk araligi; uzuvlari keskin kristal parcaciklarindan olusuyor. Alca hizinda ama agir
izlenimi veriyor.

**Prompt (create_8_directional_pro | create_character):**
```
TYPE: humanoid creature
STYLE: top-down RPG pixel art, chibi proportions, crisp 1px black outline, no anti-aliasing,
  muted desaturated palette, weathered field-worn
HEAD: small angular head, hollow cyan glowing eye sockets, no visible nose/mouth, crystalline
  skull-like shape
BODY: compact upright torso, slate-grey and void-black colouring, 3-tone flat shading
LIMBS: arms replaced by jagged crystal shards extending outward, sharp angular edges,
  frosted cyan crystal material with matte grey base; short stumpy legs
EXTRA: shattered crystal fragments orbiting the torso at waist level
CLOTHING: none -- pure creature form
HANDS: crystal spike clusters instead of hands
SILHOUETTE: wide angular shoulders from crystal arms, compact low silhouette, distinctive
  crystalline spike shapes
COLOR: dominant slate-grey #4A4E5C and void-black #1A1A2E; single cyan accent #00D4C8 on
  crystal edges only; 3 shading tones per region; no warm tones
POSE: idle upright stance, crystal arms slightly raised and angled outward
```
- **Tool:** `create_8_directional_pro` (create_8_direction_object MCP eslesimi)
- **Size:** 64x64 (canvas) — not: S43 "112" = animasyon canvas'i; ilk karakter 64x64 yapilir, gerekirse 96x96
- **View:** High top-down
- **N_directions:** 8
- **Transparent:** evet
- **Cost:** 20 gen

**Animasyon promptlari (M1 onaylandiktan sonra):**
| Animasyon | Tool | Prompt |
|---|---|---|
| Idle | `animate_with_text` (v3) | "idle stance, crystal shards slowly rotate and pulse with faint cyan glow, subtle body sway" 8 frames |
| Walk | `animate_with_text` (v3) | "lurching forward walk, crystal arms swinging awkwardly, heavy footsteps" 8 frames |
| Attack | `animate_with_text` (v3) | "stabbing forward with crystal arm spike, quick thrust and retract" 8 frames |

---

### M2 · VOID THRALL (canon boyut: 128px, demo tier 1-2 mob)

**Tasarim ozeti:**
Bosaltilmis insan silheti; soluk ve seffaf bir gorunume sahip, sanki icindeki oz emilmis.
Karanlik-mor bir enerji cesedi sariyor. Zayif ama tehlikeli kalabalik mob turu.

**Prompt:**
```
TYPE: humanoid undead creature
STYLE: top-down RPG pixel art, chibi proportions, crisp 1px black outline, no anti-aliasing,
  muted desaturated palette, weathered field-worn
HEAD: gaunt hollow humanoid head, empty eye sockets with faint void-purple glow, mouth open
  in silent scream, tattered wisps of darkness around skull
BODY: emaciated torso wrapped in torn void-purple aura wisps, ribs barely visible through
  translucent ashen skin, hunched posture
LIMBS: thin bony arms with claw-like fingers, wisps of purple-black energy trailing from
  fingertips; stumpy legs barely visible under aura
EXTRA: floating rags of torn cloth or membrane around the body, slow drift animation
CLOTHING: remnants of tattered burial shroud, ashen grey, mostly disintegrated
HANDS: claw-like bony fingers, void energy condensed at tips
SILHOUETTE: tall and thin for a chibi, distinctive wisps extending upward on both sides,
  hunched posture creates a concave torso silhouette
COLOR: dominant ashen-grey #6B6B7A and void-black #160D1F; void-purple #5B3F8C accent on
  aura wisps; no warm tones; no cyan (void mob, not rift-crystal type)
POSE: hunched idle, arms slightly raised with claw fingers spread
```
- **Tool:** `create_8_directional_pro`
- **Size:** 64x64
- **View:** High top-down
- **N_directions:** 8
- **Transparent:** evet
- **Cost:** 20 gen

**Animasyon promptlari:**
| Animasyon | Tool | Prompt |
|---|---|---|
| Idle | `animate_with_text` (v3) | "haunting idle, wisps of void energy slowly drift around body, slight head tilt" 8 frames |
| Walk | `animate_with_text` (v3) | "shambling forward walk, arms trailing behind, void aura trailing" 8 frames |
| Attack | `animate_with_text` (v3) | "lunging claw strike forward with both arms, void energy bursting from hands on impact" 8 frames |

---

### M3 · RELIC CASTER (canon boyut: 80px, demo ranged mob)

**Tasarim ozeti (legacy relic_caster.png referansi):**
Kapuchon/capali giysili humanoid; iki eli disi tutarak parlayan bir relikviyi
(parlamis eski tasinabilir) yukariyor. Fiziksel olarak zayif ama tehlikeli uzak savasci.

**Prompt:**
```
TYPE: humanoid caster creature
STYLE: top-down RPG pixel art, chibi proportions, crisp 1px black outline, no anti-aliasing,
  muted desaturated palette, weathered field-worn
HEAD: hidden under large hooded robe, only faint glow visible where face would be (no eyes
  drawn -- steep top-down angle hides face naturally), hood fabric heavily worn and frayed
BODY: small compact torso under voluminous robe, stooped scholarly posture, robe is
  slate-grey with ritual markings barely visible
LIMBS: robed arms extended forward holding relic artifact; short stubby legs barely visible
  under robe hem
EXTRA: levitating cracked stone relic artifact held in both hands, glowing with dim
  amber-gold runes and a single cyan Rift crack along its surface
CLOTHING: full-length worn hooded robe, desaturated brown-grey, hem tattered, ritual
  stitching in dull gold thread
HANDS: skeletal or wrapped fingers gripping the relic artifact
SILHOUETTE: tall pointed hood creates strong upward silhouette spike; wide robe base;
  artifact glow source visible from top-down angle
COLOR: dominant worn-linen #7A6E58 and dark-slate #3D3A42; amber-gold #C89A30 on rune
  markings (dull, not bright); single cyan #00D4C8 crack on relic only; ash-grey skin hint
POSE: idle holding relic aloft in both hands, slight lean forward
```
- **Tool:** `create_8_directional_pro`
- **Size:** 64x64
- **View:** High top-down
- **N_directions:** 8
- **Transparent:** evet
- **Cost:** 20 gen

**Animasyon promptlari:**
| Animasyon | Tool | Prompt |
|---|---|---|
| Idle | `animate_with_text` (v3) | "idle holding relic, runes pulse slowly, slight body sway" 8 frames |
| Walk | `animate_with_text` (v3) | "shuffling forward walk while carrying relic, robes swaying" 8 frames |
| Cast | `animate_with_text` (v3) | "raising relic overhead both hands, energy beam shoots forward, recoil" 8 frames |

---

## BÖLÜM 2 — BONUS SET (zaman kalirsa, S43 roster tamamlama)

### M4 · FRACTURE IMP (zaten animasyonlu -- YENIDEN URETME SADECE STIL TUTARSIZLIGI VARSA)

**Notlar:** `fracture_imp.png` legacy sprite mevcut ve oyunda aktif. Yeniden uretmeden once
mevcut sprite'in stilini M1-M3 ile karsilastir. Esik: 2+ fark varsa yeniden uret.

**Prompt (gerekirse):**
```
TYPE: small demon creature
STYLE: top-down RPG pixel art, chibi proportions, crisp 1px black outline, no anti-aliasing,
  muted desaturated palette, weathered field-worn
HEAD: angular bat-like head, small sharp horns, glowing pale-cyan eyes, snarling mouth
BODY: compact wiry muscular torso, dark blue-black chitinous skin, slightly hunched
LIMBS: short powerful arms with clawed hands; bat-like vestigial wings folded on back
  (visible from top-down but not full wingspan); short digitigrade legs
CLOTHING: none -- creature
HANDS: three-fingered clawed hands
SILHOUETTE: compact low profile, distinctive small horn silhouette on head, folded wings
  add shoulder width
COLOR: dominant void-blue #1F2B3A and dark-charcoal #2A2A35; cyan #00C8D4 glow in eyes
  and wing membrane veins only; 3 shading tones
POSE: aggressive crouched stance, arms raised and claws spread
```
- **Tool:** `create_8_directional_pro` | **Size:** 64x64 | **View:** High top-down

### M5 · CHAIN WARDEN (canon boyut: 128px, elite mob)

**Tasarim ozeti:**
Zincirli agir zirh giyimli yuksek tehdit seviyeli mob. Yikik tapinaga ozgu bir gardiyani
hatirlatir. Yuzunde maskeli kafes kask, elinde zincir-bagli silah.

**Prompt:**
```
TYPE: armored humanoid elite enemy
STYLE: top-down RPG pixel art, chibi proportions, crisp 1px black outline, no anti-aliasing,
  muted desaturated palette, weathered field-worn
HEAD: heavy caged iron helmet, dark slot for eyes, helmet cracked and worn, no visible face
BODY: barrel-chested heavy plate armor torso, slate and rust-grey coloring, dented and
  battle-scarred, imposing wide silhouette
LIMBS: thick armored arms, one hand grips a length of heavy chain; armored greaves on legs
EXTRA: chains draped around body and trailing from right fist, some links glowing faintly
  with trapped Rift cyan energy
CLOTHING: heavy battle-worn full plate over tattered surcoat in near-black
HANDS: gauntleted left hand open; right hand gripping chain weapon
SILHOUETTE: widest of all standard mobs, heavy shoulders and helmet crest create distinctive
  bulky top profile
COLOR: dominant iron-grey #4F4F5C and rust-shadow #3A2E2A; muted tarnished silver highlights;
  single cyan #00D4C8 on chain-link Rift traces only; no warm tones on armor
POSE: wide-legged guard stance, chain hanging loose in right hand
```
- **Tool:** `create_8_directional_pro` | **Size:** 96x96 | **View:** High top-down

---

## BÖLÜM 3 — BOSS (PenitentSovereign v2 — demo boss, tam uretim)

> Mevcut boss placeholder kod tarafinda aktif. Bu prompt yeni gorsel karakter icin.
> Oyundaki kod adi: `PenitentSovereign`. Lore: kucuk bir warden/lider, Rift tarafindan
> yutulmus eski bir din adaminin kalintisi.

### B1 · BOSS KARAKTERİ (PenitentSovereign)

**Tasarim oncesi notlar:**
- Legacy `13_penitent_bruiser.png` stiline bak ama buyut + daha ikonik yap
- Canvas: **160x160** (standard mob 64px'in 2.5 kati — boss hissi icin gerekli)
- Palet: birincil = arduvaz gri + is karasi; ikincil = cyan (Rift parcalanmasi); uc nokta =
  soluk altin (eski-ceremoniyal, bozulmus, kucuk aksan)
- Siluet ayirt edici olmali: tasin etrafindaki yuzuk sardinli kirik hale + kaftanli plak zirh

**Prompt:**
```
TYPE: humanoid boss creature
STYLE: top-down RPG pixel art, chibi proportions (enlarged boss scale), crisp 1px black
  outline, no anti-aliasing, muted desaturated palette, weathered field-worn
HEAD: cracked obsidian helmet-mask fused to face, deep fracture lines with glowing cyan
  seeping through, crown of floating broken stone shard halo above -- shards orbit slowly,
  no visible eyes (steep top-down angle hides face naturally)
BODY: imposing wide torso in tattered ceremonial plate armor, heavy and scarred, obsidian
  black with slate-grey highlights; tattered ceremonial robes hanging below breastplate,
  deep grey-black, torn hem
LIMBS: thick heavy armored arms, left hand open palm raised in ritual gesture, right hand
  gripping a cracked stone warden-staff; heavy armored legs with ceremonial greave engravings
EXTRA: floating stone shard ring (broken halo) orbiting the head at helmet level; faint
  cyan Rift energy spilling from helmet cracks; ritual seal marks carved into breastplate
  glowing dully
CLOTHING: heavy obsidian plate breastplate over ceremonial robe, robe is near-black with
  barely-visible ritual embroidery, pauldrons cracked and asymmetric
HANDS: right hand grips cracked warden-staff (top-down visible as rod shape); left hand
  open, fingers spread, ritual stance
SILHOUETTE: significantly wider and taller than standard mobs, distinctive floating shard
  halo ring visible from top-down, asymmetric pauldrons add shoulder variance, staff extends
  to one side
COLOR: dominant obsidian-black #1A1A20 and slate #3D3D4A; cyan #00FFCC fracture glow on
  helmet cracks (primary accent); dull-gold #8A7A30 on ritual seal marks only (very sparse);
  3+ shading tones per material
POSE: menacing forward stance, both arms slightly raised, warden-staff angled outward
```
- **Tool:** `create_8_directional_pro`
- **Size:** 160x160 (boss)
- **View:** High top-down
- **N_directions:** 8
- **Transparent:** evet
- **Cost:** 20 gen
- **Onay gate:** Uretince goster -- bu sprite onaylanmadan B2-B4 animasyon BASLATMA

### B2 · Boss Walk/Advance
- **Tool:** `animate_with_text` (v3) | Referans: B1 onaylanan sprite (guney yonu)
- **Prompt:** `"slow heavy menacing advance, warden staff dragging slightly, shard halo rotating faster"` 8 frames loop ACIK
- **Cost:** 0 gen (free animate_with_text)

### B3 · Boss Attack (Overhead Slam)
- **Tool:** `animate_with_text` (v3)
- **Prompt:** `"massive two-arm overhead slam attack, warden staff raised high then crashes down, shockwave impact on ground"` 8 frames loop KAPALI
- **Cost:** 0 gen

### B4 · Boss Death
- **Tool:** `animate_with_text` (v3)
- **Prompt:** `"collapsing forward slowly, shard halo shattering and falling, cyan energy draining from cracks, falling to knees then forward"` 8 frames loop KAPALI
- **Cost:** 0 gen

### B5 · Boss Phase 2 Cast (zaman kalirsa)
- **Tool:** `animate_with_text` (v3)
- **Prompt:** `"raising both arms overhead summoning rift energy, stone shards orbiting fast, cyan light intensifying from helmet cracks"` 8 frames loop KAPALI

---

## UNITY IMPORT NOTLARI (Ben yaparim, sen sadece "hazir" de)

### Genel Import Ayarlari
| Ayar | Deger | Neden |
|---|---|---|
| Texture Type | Sprite (2D and UI) | Standart |
| Sprite Mode | Multiple | Çok kare sprite sheet icin |
| Pixels Per Unit (PPU) | **64** | RIMA kanon kilidi |
| Filter Mode | **Point (no filter)** | Bulaniklasma yok, crisp piksel |
| Compression | None veya Lossless | Piksel art artefakti olmasin |
| Generate Mip Maps | KAPALI | Top-down 2D'de gereksiz |

### Pivot (RIMA Konvansiyonu)
- Tum karakter/mob sprite'lari: pivot = **bottom-center** (ayak tabani merkezi)
- Boss (160x160): pivot = **bottom-center**, dusey eksen Y-sort icin
- Asla `center` kullanma -- karakterler zemine gomulur

### Sorting Layer
- Mob'lar + Boss: **"Entities"** layer
- Y-sort: Unity 2D Renderer'da `Sprite Renderer > Sorting` > `Sprite Sort Point = Pivot` + `Order in Layer = 0`
- Y-sort aktif sahne: `Transparency Sort Mode = Custom Axis`, Y ekseni

### 8-Yon Sprite Kesme ve Isimlendirme

PixelLab 8-yön export formatina gore kesme:
```
Sprite sheet duzeni (create_8_directional_pro ciktisi genellikle):
  S  SW  W  NW  N  NE  E  SE  (8 kare yan yana veya 2x4 grid)
```
Kesilen sprite'lari su sekilde adlandir:
```
<mob_adi>_idle_S.png    (veya _south, tool'a gore)
<mob_adi>_idle_SE.png
<mob_adi>_idle_E.png
<mob_adi>_idle_NE.png
<mob_adi>_idle_N.png
-- W, SW, NW = Unity'de flipX ile mirror (kendin kesme)
```

### 3 Mirror Yonu (Unity'de yapilir, PixelLab'dan uretilmez)
```
W  <-- E'nin flipX mirrorudur
SW <-- SE'nin flipX mirrorudur
NW <-- NE'nin flipX mirrorudur
```
Unity SpriteRenderer.flipX = true olarak controller'da ayarlanir.
**5 sprite uret, 3'unu PixelLab'dan isteme -- kod halleder.**

### Animator Yapisi (Mob/Boss)
- Her mob: temel `<MobAdi>.controller`
- State'ler: Idle / Walk / Attack (/ Cast Relic Caster icin)
- Boss ek state'ler: Idle / Walk / Attack / Death (/ Cast B5 opsiyonel)
- Parametre: `Speed` (float) -- 0=Idle, >0=Walk; `Attack` (trigger); `Death` (trigger)
- AnyState gecisleri; loop: Idle+Walk=ACIK, Attack+Death=KAPALI

### Import Sonrasi Checklist
- [ ] PPU=64 teyit (Inspector'da "Pixels Per Unit: 64")
- [ ] Filter Mode = Point teyit
- [ ] Sprite Mode = Multiple, dilim dogru (8 kare)
- [ ] Pivot = Bottom Center
- [ ] Sorting Layer = Entities
- [ ] Y-sort eksenine gore sirtlanma yok (test: oyuncu mobun arkasindan gecince ustu kapanmali)
- [ ] Mirror yonleri (W/SW/NW) controller'da flipX=true ile test

---

## ÜRETİM SIRASI (onerilen)

```
1. M1 Shard Walker karakter  (20 gen)
2. Goruntu onayi al
3. M1 animasyonlar: Idle + Walk + Attack  (0 gen x3 = ucretsiz)
4. M2 Void Thrall karakter  (20 gen)
5. M2 animasyonlar  (0 gen x3)
6. B1 Boss PenitentSovereign karakter  (20 gen) -- EN ONEMLI, ONCEDEN YAP
7. B1 onay -- onaysiz B2-B4 baslatma
8. B2-B4 Boss animasyonlari  (0 gen x3)
9. M3 Relic Caster karakter  (20 gen)
10. M3 animasyonlar  (0 gen x3)
-- Bonus (zaman kalirsa) --
11. M4 Fracture Imp yeniden uretim (sadece stil farki buyukse)
12. M5 Chain Warden  (20 gen)
```

**Toplam tahmini maliyet:** 80 gen (4 karakter x 20) + 0 gen animasyonlar (free tool)
Mevcut kota: ~990 gen — bol pay.

---

## STİL TUTARLILIK KONTROL LİSTESİ (her uretim sonrasi)

Yeni sprite'i legacy `fracture_imp.png` veya `relic_caster.png` ile karsilastir:
- [ ] Canvas boyutu eslesme (64x64 veya belirtilen)
- [ ] 1px siyah outline tum silhueti cevirmiyor mu
- [ ] Chibi oranlar: bas govdeye oranla buyuk
- [ ] Sicak kirmizi/pembe/parlak renkler YOK
- [ ] Cyan sadece Rift enerji aksan noktalarinda
- [ ] Perspektif yuksek tepeden, yana-donme-simulasyonu degil
- [ ] Golge taraf yukari (tepeden aydinlatma), asagi degil

Eger 2+ madde tutmuyorsa: `create_from_style_pro` ile mevcut sprite'i stil referansi olarak kullanarak yeniden uret.
