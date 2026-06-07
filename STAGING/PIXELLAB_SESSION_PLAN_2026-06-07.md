# PIXELLAB BUYUK SEANS PLANI — 2026-06-07
Hazırlayan: rima-asset (hazırlık; üretim kullanıcıyla GATED)
Durum: SADECE PLAN — üretime BASMA, kullanıcı onayı gerekli

---

## ONEMLI CAKISMA NOTU (okumadan basma)

T2_MOB_PROTOTYPE_SPEC.md (LOCKED, Karar #82+84) ile COMBAT_ROSTER.md boyut bilgileri FARKLI.
Hangisi gecer?

| Kaynak | Shard Walker | Bruiser | Fracture Imp |
|---|---|---|---|
| T2_MOB_PROTOTYPE_SPEC | 64px | 64px | 64px (sub-imp 32px) |
| COMBAT_ROSTER (S43 onaylı) | 112px | 128px | 48px |

KARAR GEREKTIRIYOR: Seans basında kullanıcıya sor hangi boyut geçerli. Öneri: COMBAT_ROSTER S43 onaylı = daha yeni. Aşağıdaki plan COMBAT_ROSTER boyutlarını kullanır ama kullanıcı 64px derse trivially güncellenebilir.

4. MOB ARKETIP BELIRSIZLIGI: T2 spec 3 mob içeriyor (Walker, Bruiser, Imp). COMBAT_ROSTER demo öncelik seti şu: Walker + Void Thrall + Chain Warden + Relic Caster. "4 arketip" = bu 4'mü yoksa T2 spec 3 + 4.? Aşağıdaki plan COMBAT_ROSTER demo setini (Walker, Void Thrall, Chain Warden, Relic Caster) 4 arketip olarak kullanır — T2 spec'in Bruiser ve Fracture Imp'ini de sonraya not olarak ekler. Kullanıcı onaylasın.

---

## BÖLÜM 1 — MOBLAR (4 Arketip)

### Mob Standartları (COMBAT_ROSTER S43 bazlı)

| Özellik | Değer |
|---|---|
| Canvas | Her mob silüetine göre yukarıdaki tabloda |
| PPU | 64 |
| Filter | Point |
| Yön | 4 yön (S + N + E + W) — flipX ile E/W'den birini üret, diğeri kod flipX. 8-yön YOKTUR mob için (T2 spec: "4 yon + flipX"). |
| Animasyon | Seans 1: sadece idle + walk. Combat anim (attack/hurt/death) = ayrı seans. |
| Araç | mcp__pixellab__create_character (her mob = karakter pipeline, NOT map_object) |
| Çizim rengi | The Fracturing estetigi: kırık/echo varlıklar; kameralı duruş: MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward — not looking up at the camera. The steep overhead angle hides the eyes naturally. |

### Mob 1 — Shard Walker (Ranged Caster)

**Silüet:** 112x112 canvas, dik dar gövde, omuzlardan 3-4 keskin kristal çıkıntısı, gövdede gap'lerden rift ışığı sızıyor.
**Role:** Ranged pressure — orta mesafeden shard fırlatır, ölünce patlama.
**Telegraph eşleşmesi:** telegraph_line_beam.png (Assets/Art/Telegraphs) — projectile yönünü gösterir.

```
[ ] MOB 1 — SHARD WALKER IDLE (4 yön)

Araç: mcp__pixellab__create_character
preset: male human
canvas_width: 112
canvas_height: 112
description: "MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward — not looking up at the camera. The steep overhead angle hides the eyes naturally. Shard Walker enemy, fractured humanoid creature, body composed of jagged broken crystal slabs with light leaking through gaps, 3-4 sharp crystal shard protrusions from shoulders, muted desaturated palette, weathered field-worn, cyan rift energy cracks glowing between crystal pieces, hollow chest cavity emitting dim cyan light, no pupils (crystal voids for eyes), idle breathing stance, pixel art chibi proportions, transparent background"
style_images: [warrior_idle_128.png downscale to 112px base64]
num_candidates: 4
```

Üretim sırası: S (south) → N → E (W = runtime flipX)

Import sonrası adım: Assets/Art/Characters/Mobs/ShardWalker/ klasörüne, PPU=64, Point, Pivot=bottom-center (0.5, 0), MobAnimator wiring task cx'e.

---

### Mob 2 — Void Thrall (Splitter / Priority Target)

**Silüet:** 128x128 canvas, uzun ince gövde, void tendril uzantıları, soluk mor aura, split mechanic = ölünce iki HalfThrall'a bölünür.
**Role:** Priority target — öldür ama dikkatli öldür (AoE = 2 sorun).
**Telegraph eşleşmesi:** telegraph_circle_ring.png — death split radius gösterir.

```
[ ] MOB 2 — VOID THRALL IDLE (4 yön)

Araç: mcp__pixellab__create_character
preset: male human
canvas_width: 128
canvas_height: 128
description: "MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward — not looking up at the camera. The steep overhead angle hides the eyes naturally. Void Thrall enemy, tall slender humanoid creature, body with void tendrils drifting from limbs, faint pale violet aura around body, cracked dark skin with void light seeping through fractures, muted desaturated palette, weathered field-worn, hollow eyes glowing dim violet, idle swaying stance, ominous presence, pixel art chibi proportions, transparent background"
style_images: [warrior_idle_128.png downscale to 128px base64]
num_candidates: 4
```

Üretim sırası: S → N → E (W = runtime flipX)

Import sonrası adım: Assets/Art/Characters/Mobs/VoidThrall/ klasörüne, PPU=64, Point, Pivot=bottom-center (0.5, 0). HalfThrall için ayrı mini sprite gerekir (64px, ayrı üretim).

---

### Mob 3 — Chain Warden (Controller / Mobility Punisher)

**Silüet:** 128x128 canvas, ağır zırhlı gövde, omuzlardan 2 enerji zinciri uçuşan tendril, boyun büyük. Silueti zincirler karmaşıklaştırıyor ama zincirler belirgin.
**Role:** Dash cezalandırır — 3 zincir 45° yelpaze, 1.5s slow.
**Telegraph eşleşmesi:** telegraph_cone_fan.png — 3-zincir yelpazesi için cone.

```
[ ] MOB 3 — CHAIN WARDEN IDLE (4 yön)

Araç: mcp__pixellab__create_character
preset: male human
canvas_width: 128
canvas_height: 128
description: "MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward — not looking up at the camera. The steep overhead angle hides the eyes naturally. Chain Warden enemy, heavily armored fractured prison guard, massive armored chest with oversized shoulders, two glowing rift-energy chains trailing from shoulders like tendrils, chains made of cyan fractured light links, cracked stone-like armor plating, muted desaturated palette, weathered field-worn, dim cyan glow from armor cracks, guard stance with arms slightly raised, pixel art chibi proportions, transparent background"
style_images: [warrior_idle_128.png downscale to 128px base64]
num_candidates: 4
```

Üretim sırası: S → N → E (W = runtime flipX)

Import sonrası adım: Assets/Art/Characters/Mobs/ChainWarden/ klasörüne, PPU=64, Point, Pivot=bottom-center (0.5, 0). Zincir tendril animasyonu = ayrı particle/sprite overlay; idle spriteda statik zincir pozisyonu yeterli.

---

### Mob 4 — Relic Caster (Support / Execution Target)

**Silüet:** 80x80 canvas (KASITLI KUCUK — execution priority visual cue), ince uzun gövde, elinde dik kırık kristal relikvar tutmakta.
**Role:** Execution Target — yakın düşmana kalkan verir (kırılabilir 2s shield). Kendisi en kırılgan mob.
**Telegraph eşleşmesi:** Kalkan verme = hedef mob üzerinde altın aura (sprite-overlay, no telegraph asset gerekli).

```
[ ] MOB 4 — RELIC CASTER IDLE (4 yön)

Araç: mcp__pixellab__create_character
preset: female human
canvas_width: 80
canvas_height: 80
description: "MATCH THE EXACT CAMERA ANGLE of warrior_idle_128.png. The character has normal eyes facing forward — not looking up at the camera. The steep overhead angle hides the eyes naturally. Relic Caster enemy, deliberately small and fragile, thin tall humanoid in tattered ritual robes, holding a broken vertical crystal relic shard with both hands, relic shard glowing faint cyan at fracture lines, sunken hollow eyes, muted desaturated palette, weathered field-worn, frail hunched posture but upright casting stance, pixel art chibi proportions, transparent background"
style_images: [warrior_idle_128.png downscale to 80px base64]
num_candidates: 4
```

Üretim sırası: S → N → E (W = runtime flipX)

Import sonrası adım: Assets/Art/Characters/Mobs/RelicCaster/ klasörüne, PPU=64, Point, Pivot=bottom-center (0.5, 0). Kasıtlı 80px = oda içinde en küçük figür, execution priority okunabilir.

---

### T2 Spec Mobları (Seans Dısı — Sonraya Not)

T2_MOB_PROTOTYPE_SPEC'teki Penitent Bruiser ve Fracture Imp COMBAT_ROSTER demo setine dahil değil. Demo önceliği bitince üretilecek. Fracture Imp sub-imp'i 32px ayrı karakter seti gerektirir.

---

## BÖLÜM 2 — 3 SILAH

### Silah Standartları

| Özellik | Değer |
|---|---|
| Araç | mcp__pixellab__create_1_direction_object |
| Çizim açısı | YATAY, uç sağa (0° doğu) — canlı greatsword konvansiyonu (OrientationSync buna göre ayarlı) |
| Boyut kararı | CANLI PRATIK KAZANIR: hedef boyutta direkt üret (downscale artefaktı yok, PixelLab native piksel) |
| size + style_images birlikte VERILEMEZ | style_images verilince en büyük ref boyutu çıktıyı belirler — ref'leri hedef boyuta indir |
| Pivot | Unity Sprite Editor'da elle set edilir — PixelLab metadata vermez |
| Review akışı | create_1_direction_object → 'review' status → get_object (adayları gör) → select_object_frames(indices) tut / dismiss_review at |

---

### Silah 1 — Ranger Compound Bow (64px)

**Canon (01_CANON_WEAPONS.md):** Compound bow, sol elde, aşağı eğik at-rest, asimetrik duruş. Soğuk mavi aksent (#7BA7BC).
**Çizim:** YATAY, kiriş sağda, uç (top limb) sağ-yukarı. Birleşik yay mekanik görünümlü, orman okçusu DEĞİL taktiksel.
**Boyut:** 64x64 px (16-item tier elde etmek için style_images maks 64px olmalı).

```
[ ] SILAH 1 — RANGER BOW

Araç: mcp__pixellab__create_1_direction_object
description: "compound tactical bow, horizontal orientation tip pointing right, mechanical recurve limbs with cable and cams, slate grey polymer riser, cold blue (#7BA7BC) accent on limbs, muted desaturated palette, weathered field-worn, crisp pixel art, transparent background"
item_descriptions: [
  "compound tactical bow, horizontal, tip right, mechanical limbs, cold blue accent, pixel art",
  "compound tactical bow variant, horizontal, tip right, asymmetric grip, cold blue cable, pixel art"
]
style_images: [ranger character sprite downscale to 64px base64, existing bow asset ebc33ebf downscale to 64px base64]
```

Import sonrası adım: Assets/Art/Weapons/Ranger/ klasörüne, PPU=64, Point, spritePivot = sap noktası (sol taraf, ~0.15, 0.5). WeaponDatabase Ranger entry.

---

### Silah 2 — Shadowblade Twin Daggers (32px, TEK sprite + runtime flipX)

**Canon (01_CANON_WEAPONS.md):** Ince temiz keskin ikiz hançer, HEP reverse-grip, gövdeye yakın. Gömülü glow YASAK. Void moru aksent (#5A2A8A). ~%30-35 karakter sprite.
**Önemli:** 1 sprite üret (sağ el reverse-grip), left hand = runtime flipX.
**Boyut:** 32x32 px (bu boyutta style_images ile maks 8 aday).
**NOT:** 32px çok küçük — style_images ref de 32px olacak, detay kaybolabilir. Alternatif: 40px üret, Unity'de scale ayarla.

```
[ ] SILAH 2 — SHADOWBLADE DAGGER (TEK SPRITE)

Araç: mcp__pixellab__create_1_direction_object
description: "reverse-grip dagger single sprite, horizontal orientation blade pointing right, thin sharp blade no visible glow or emissive effect, dark steel with void purple (#5A2A8A) wrapped handle, reverse grip angle, muted desaturated palette, crisp pixel art, transparent background"
item_descriptions: [
  "reverse-grip dagger, horizontal blade right, thin clean blade, void purple handle wrap, dark steel, pixel art",
  "reverse-grip dagger variant, horizontal blade right, slightly wider blade, dark steel, void purple accent, pixel art"
]
style_images: [shadowblade character sprite downscale to 32px base64, existing dagger asset 9312ea86 downscale to 32px base64]
```

Import sonrası adım: Assets/Art/Weapons/Shadowblade/ klasörüne, PPU=64, Point, spritePivot = tutma noktası (sap merkezi, ~0.2, 0.5). WeaponDatabase Shadowblade entry; off-hand = runtime flipX (OrientationSync), AYRI sprite üretilmez.

---

### Silah 3 — Elementalist Floating Rune Disc (48px)

**Canon (01_CANON_WEAPONS.md):** Floating Golden Rune Disc — ASA/DEGNEK YASAK (Karar #146). Sag avucun ~3px ustunde doner/suzulur. Sınıfsız silah = silahsız sınıf, disc = bağımsız küçük obje.
**Önemli:** Bu weapon ATTACH DEĞİL — avuç üstü hover, muhtemelen ayrı küçük script: bob + spin. Create_1_direction_object ile YUVARLAK disk üret (not a staff, not a wand).
**Boyut:** 48px (4 aday tier, style_images ile).
**Altın/sarı aksent (#FFF000).**

```
[ ] SILAH 3 — ELEMENTALIST RUNE DISC

Araç: mcp__pixellab__create_1_direction_object
description: "small floating circular rune disc, top-down view as if hovering flat, golden yellow (#FFF000) runic inscriptions on dark disc face, geometric magic circle pattern, thin raised rim, slight glow from rune channels, no staff no wand no handle, muted palette body with bright gold runes, crisp pixel art, transparent background"
item_descriptions: [
  "floating golden rune disc, circular, runic pattern face, golden yellow glowing inscriptions, dark background disc, pixel art",
  "floating golden rune disc variant, circular, different rune arrangement, golden yellow accent, slightly raised center, pixel art"
]
style_images: [elementalist character sprite downscale to 48px base64]
```

Import sonrası adım: Assets/Art/Weapons/Elementalist/ klasörüne, PPU=64, Point, spritePivot=center (0.5, 0.5). Attach mimarisi = FARKLI: hand'a takılmaz, avuç üstü orbit/bob script. WeaponDatabase Elementalist entry. Ayrıca: OrientationSync'e Elementalist özel kod gerekebilir — flag olarak bırak.

---

## BÖLÜM 3 — KÜÇÜKLER

### 3a — Portal Rünleri (needs_regen / eksik)

Portal pack batch1 ve batch2 manifeste göre:
- rune_combat.png: MEVCUT ve PASS (STAGING/imagegen/assets/rune_combat.png)
- rune_elite.png: MEVCUT ve PASS (STAGING/imagegen/assets/rune_elite.png)
- rune_reward.png: MEVCUT ve PASS (portal_pack_2026-06-07/)
- rune_boss.png: MEVCUT ve PASS (portal_pack_2026-06-07/)

TUMU VAR. Rüne üretimi GEREKMEZ. PixelLab'a gerek yok — sadece wiring (cx task).

---

### 3b — Seal Monolith (landmark prop)

MEVCUT: prop_seal_monolith.png (portal_pack_2026-06-07/, 96x160, PASS).
PixelLab üretimine GEREK YOK. Wiring task: PropRegistry'e kaydet (cx task).

---

### 3c — Genel Rünler (portal skin seti için ek)

Portal PORTAL_PACK_DECISION'a göre 4 skin: Combat / Elite / Reward / Boss. Tüm rünler MEVCUT.
SONUC: Bölüm 3 için PixelLab seans görevi YOK. Kalan iş = wiring (ayrı cx task).

---

### 3d — Opsiyonel: HalfThrall Mini Sprite

Void Thrall ölünce 2 HalfThrall spawn eder (64px, daha hızlı versiyon). Demo için:
- Seçenek A: Void Thrall spritını runtime scale 0.5x + tint değişimi (programatik, SIFIR sanat).
- Seçenek B: PixelLab 64px HalfThrall üret.
Kullanıcı karar versin. Seçenek A önerilir (seans süresini kısaltır).

---

## SEANS AKIS SIRASI (kopyala-yapıştır hazır)

Seans başlangıcında kullanıcıya sor:
1. Mob boyutu: 64px mi (T2 spec) yoksa COMBAT_ROSTER boyutları mı (Walker=112, Bruiser=128, Imp=48)?
2. 4. arketip: COMBAT_ROSTER demo seti mi (Walker/Thrall/Warden/Caster) yoksa başka?
3. HalfThrall: programatik mi yoksa PixelLab?

Onaydan sonra sıra:
```
[ ] 1. Shard Walker S-yön → [ ] N-yön → [ ] E-yön
[ ] 2. Void Thrall S-yön → [ ] N-yön → [ ] E-yön
[ ] 3. Chain Warden S-yön → [ ] N-yön → [ ] E-yön
[ ] 4. Relic Caster S-yön → [ ] N-yön → [ ] E-yön
[ ] 5. Ranger Bow (2 item)
[ ] 6. Shadowblade Dagger (2 item)
[ ] 7. Elementalist Rune Disc (2 item)
```

Her mob üretimi: create_character → job_id al → get_character ile status kontrol → candidates seç → idle animation (opsiyonel aynı seansta, ayrı tool: mcp__pixellab__animate_character).

Her silah üretimi: create_1_direction_object → review status → get_object → select_object_frames(indices) → dismiss_review.

---

## STYLE_IMAGES HAZIRLAMA ADIMI (seans ÖNCE yapılır)

PixelLab'a style_images göndermeden önce referans spriteleri indir ve boyuta küçült:

```
[ ] warrior_idle_128.png — mevcut, kamera açısı referansı
[ ] Warblade/south.png → downscale to 112px → mob ref
[ ] Ranger/south.png → downscale to 64px → bow ref
[ ] Shadowblade/south.png → downscale to 32px → dagger ref
[ ] Elementalist/south.png → downscale to 48px → disc ref
[ ] existing bow asset (ebc33ebf) → get_object → downscale to 64px
[ ] existing dagger asset (9312ea86) → get_object → downscale to 32px
```

Tüm refler base64 PNG olarak hazırlanır. Her ref <=256px. Optimal 1-3 ref per batch.

---

## SEANS ÇIKTI KONTROL LISTESI (QC kriterleri)

### Her üretilen sprite için:

- [ ] Transparan arka plan (alpha PNG, koyu renk değil)
- [ ] Boyut spec ile eşleşiyor (112x112 / 128x128 / 80x80 / 64x64 / 32x32 / 48px)
- [ ] Kamera açısı: warrior_idle_128.png ile eşleşiyor (60-65 derece, NOT straight-down)
- [ ] Karakter yüzü: gözler öne bakıyor (NOT yukarıya), dik duruş natural
- [ ] Palet: muted/desaturated, cyan aksan mob kimliğini bozmamış (oyuncu cyan'ı ile karışmamış)
- [ ] Silüet: oda içinde birden fazla mob varken ayırt edilebilir (küçük thumbnail testinde)
- [ ] Relic Caster: oda içinde en küçük figür — execution priority okunabilir
- [ ] Silahlar: yatay çizim, uç sağa, grip noktası sol tarafta
- [ ] Silahlar: gömülü glow YOK (Shadowblade), soft hover glow var (Elementalist disc)
- [ ] Elementalist disc: yuvarlak, asa/degnek değil, flat top-down görünüm

### Import sonrası Unity kontrol:

- [ ] PPU = 64 (tüm sprite'lar)
- [ ] Filter Mode = Point (None) — Bilinear KESİNLIKLE HAYIR
- [ ] Sprite Mode = Single
- [ ] Alpha Is Transparency = enabled
- [ ] Pivot: mob = bottom-center (0.5, 0); silahlar = tutma noktası (elle set)
- [ ] Önizleme: Game View'da karakter üstünde test et (oda içinde doğal duruyor mu)

---

## UYARILAR (seans sırasında)

1. KAMERA ACISI: "80 degree straight down" YAZDIRMA. Prompt'lara warrior_idle_128.png match ifadesini HER ZAMAN ekle.
2. YASAK IFADELER: "dark fantasy", "3/4 view", "no eyes", "eyeless", "extreme top-down bird's eye" — hiçbirini kullanma.
3. ELEMENTALIST: "staff", "wand", "scepter", "orb on stick" — YASAK (Karar #146). Sadece "floating disc".
4. SHADOWBLADE glow: "glowing blade", "emissive edge" — YASAK. Glow sadece handle'da void purple wrap.
5. style_images + size BIRLIKTE VERILEMEZ: style_images verilince size parametresi geçersiz — ref boyutu çıktı boyutunu belirler.
6. Mob idle için walk animasyonu ayrı seansta: önce idle üret, walk = confirm sonra. Karıştırma.

