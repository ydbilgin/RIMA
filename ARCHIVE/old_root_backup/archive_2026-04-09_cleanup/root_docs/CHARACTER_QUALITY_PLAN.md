# CHARACTER VISUAL QUALITY PLAN
*Karar tarihi: 2026-04-07 | Hedef: Hero Siege Season 9 seviyesi*
*Hikayeye uygun: karanlık roguelite, void/rift estetik, veteran savaşçılar*

---

## KARAKTERLERİN YENİDEN DEĞERLENDİRMESİ

| Karakter | Sorun | Karar | Öncelik |
|---|---|---|---|
| **Shadowblade** | Karanlık blob, silüet yok, bıçak görünmüyor | **YENİDEN ÜRET** | 🔴 1 |
| **Elementalist** | Çocuksu görünüm, hikayeye uymuyor | **YENİDEN ÜRET** | 🔴 2 |
| **Ranger** | Çocuksu/karikatür, gritty değil | **YENİDEN ÜRET** | 🔴 3 |
| **Warblade** | Kısmen iyi — crouching biraz garip ama işlevsel | **OPSİYONEL** | 🟡 4 |

---

## ARAÇ: pixellab.ai/create → "Create Image PRO"

Her karakter için bu flow:
1. pixellab.ai/create → **"Create Image PRO"** seç
2. **Style image:** `shadowblade_south.png` yükle (Shadowblade hariç)
3. **Size:** 128 × 128
4. **View:** Low top-down
5. Description kutusuna yapıştır → Generate
6. 4 varyasyon gelir (2×2 grid) — her varyasyon farklı yön gösterir
7. Aseprite'ta 128×128'lik 4 kareyi crop et, aşağıdaki isimlerle kaydet

**Crop ve kayıt:**
```
chars-rima/{karakter}/{karakter}_south.png   ← bize bakan yön
chars-rima/{karakter}/{karakter}_north.png   ← arkası dönük yön
chars-rima/{karakter}/{karakter}_east.png    ← sağa bakan yön
chars-rima/{karakter}/{karakter}_west.png    ← sola bakan yön
```

**Sonra:** Kiro, south.png'yi referans alarak `generate-8-rotations-v2` ile 4 diagonal yönü üretir.

> Shadowblade'i referanssız üret, beğenince diğer 3 karakterde Style Image olarak ver.

---

## ORTAK AYARLAR (tüm karakterler için)

```
Araç:    Create Image PRO
Size:    128 × 128
View:    Low top-down
Style:   shadowblade_south.png (Shadowblade hariç)
```

```
Negative (tümünde kullan):
chibi. cartoon. cute. childish. stocky. white background. deformed.
```

---

## ORTAK QC KRİTERLERİ

```
PASS (her karakter için):
  ✓ Silüet NET — karanlık arka plandan okunabilir
  ✓ Heroic oran: uzun bacaklar, dar bel, geniş omuzlar
  ✓ Silah/ekipman açıkça görünüyor
  ✓ Hikayeye uygun: savaşçı, veteran, gritty — çocuk değil
  ✓ RIMA renk paleti: sınıf renginde ama dark/muted

FAIL (yeniden dene):
  ✗ Blob/kitle görünümü
  ✗ Silah görünmüyor
  ✗ Renkler çok parlak/karikatür
  ✗ Kısa/bodur oranlar
  ✗ Chibi estetik
```

---

## 1. SHADOWBLADE — Dual-Blade Assassin

**Renk paleti:** Koyu mor + siyah + mavi-mor rift cracks
**Sınıf hissi:** Sessiz, ölümcül, gölgeden gelen — rogue/assassin

### Description
```
Low top-down pixel art, dark fantasy assassin, 128x128px.
4 directions across the 4 panels:
  south — full front, facing viewer
  north — full back to viewer
  east  — side view facing right
  west  — side view facing left

Tall slim dual-wielding assassin, heroic proportions, long legs, upright stance.
Dark hooded cloak over light tactical armor with shoulder guards and bracers.
Both hands extended, each holding a short curved dagger with glinting blades.
Near-black and dark purple. Blue-purple rift energy cracks on cloak edges.
Gritty, silent, deadly.

Avoid: chibi, stocky, daggers hidden, white background.
```

### Save Path
```
RIMA\Assets\Sprites\Characters\Shadowblade\reference\
  south.png, north.png, east.png, west.png
  south-east.png, south-west.png, north-east.png, north-west.png
```

---

## 2. ELEMENTALIST — Battle Mage

**Renk paleti:** Koyu mor robe + amber/turuncu ateş detayları + rift cracks
**Sınıf hissi:** Savaş büyücüsü — okul bitirmiş öğrenci değil, ön saflarda savaşmış veteran

### Description
```
Low top-down pixel art, dark fantasy battle mage, 128x128px. Female character.
4 directions across the 4 panels:
  south — full front, facing viewer
  north — full back to viewer
  east  — side view facing right
  west  — side view facing left

Tall female battle mage, heroic proportions, long legs, upright stance.
Weathered face visible, serious expression. Warm sandy blonde hair, muted, partially tied back.
Right hand grips a tall dark ornate staff with amber-orange glowing crystal on top.
Dark charcoal-purple battle robe with amber rune markings. Dark chest armor underneath.
Blue-purple rift cracks on robe and staff. Gritty veteran.

Avoid: chibi, cute, male face, staff hidden, white background.
```

### Save Path
```
RIMA\Assets\Sprites\Characters\Elementalist\reference\
  south.png, north.png, east.png, west.png
  south-east.png, south-west.png, north-east.png, north-west.png
```

---

## 3. RANGER — Dark Tactical Hunter

**Renk paleti:** Koyu yeşil + charcoal gri + subtle rift accent
**Sınıf hissi:** Keskin, hayatta kalan, avcı — orman perisi değil karanlık bir stalker

### Description
```
Low top-down pixel art, dark fantasy hunter, 128x128px. Female character.
4 directions across the 4 panels:
  south — full front, facing viewer
  north — full back to viewer, quiver clearly visible on back
  east  — side view facing right
  west  — side view facing left

Tall lean female hunter, heroic proportions, long legs, alert upright stance.
Face visible under partially lowered hood — focused eyes, serious expression. Dark auburn hair.
Left hand grips tall longbow away from body. Arrow quiver strapped to back.
Deep forest green and charcoal cloak over leather chest armor with metal plates.
Belt with pouches. Blue-purple rift glow on bow and quiver. Survivalist, dangerous.

Avoid: chibi, elven, male face, bow hidden, white background.
```

### Save Path
```
RIMA\Assets\Sprites\Characters\Ranger\reference\
  south.png, north.png, east.png, west.png
  south-east.png, south-west.png, north-east.png, north-west.png
```

---

## 4. WARBLADE — (Opsiyonel) Heavy Warrior

**Mevcut durum:** İşlevsel ama crouching stance biraz garip, armor daha dramatik olabilir.
**Karar:** Sen istersen yenile. İstemezsen mevcut kullanılabilir.

**Renk paleti:** Çelik mavi + dark metal armor + rift cracks
**Sınıf hissi:** Ağır, ezici, unstoppable — bir duvar gibi ilerleyen savaşçı

### Description (eğer yenilemeye karar verirsen)
```
Low top-down pixel art, dark fantasy heavy warrior, 128x128px.
4 directions across the 4 panels:
  south — full front, facing viewer
  north — full back to viewer, back of armor visible
  east  — side view facing right
  west  — side view facing left

Massive male warrior, broad shoulders, long legs, dominant upright stance.
No helmet — scarred face fully visible, short dark hair, strong jaw, stern expression.
Both hands gripping a massive two-handed greatsword, blade extended and visible.
Cold blue-white glowing edge on blade, rift energy pulsing.
Full plate armor in dark steel-blue and charcoal, battle-scarred, dented. Large pauldrons, thick gauntlets.
Blue-purple rift cracks through armor gaps. Unstoppable tank.

Avoid: chibi, slim build, sword hidden, white background.
```

### Save Path (opsiyonel)
```
RIMA\Assets\Sprites\Characters\Warblade\reference\
  south.png, north.png, east.png, west.png
  south-east.png, south-west.png, north-east.png, north-west.png
```

---

## RETRY STRATEJİSİ

| Sorun | Çözüm |
|---|---|
| Çocuksu görünüm | "battle-hardened veteran, scarred, serious" ekle |
| Çok parlak/karikatür | "dark muted color scheme, low saturation" ekle |
| Bodur/kısa | "tall heroic proportions, long legs, narrow waist" öne çıkar |
| Silah görünmüyor | Silahı description'ın başına al |
| 3 denemede geçmiyorsa | AI freedom slider'ını biraz sola çek veya description'ı sadeleştir |

---

## SONRA NE YAPACAKSIN

```
Karakteri beğenince →
  "shadowblade yeni sprite hazır"
  "elementalist yeni sprite hazır"
  "ranger yeni sprite hazır"
  "warblade yeni sprite hazır" (opsiyonel)

de. Claude şunları yapar:
  1. Yeni sprite'ı import et (PPU=48)
  2. Eski animasyon clip'lerindeki sprite referanslarını güncelle
  3. AnimatorController'daki BlendTree'leri kontrol et
  4. Fight-stance-idle + walk + run → yeni sprite ile çalışıyor mu test et
  5. ASSET_MAP ve CURRENT_STATUS güncelle
```

---

## ANİMASYON FRAME HEDEFLER (güncel)

| Animasyon | Eski | Yeni Hedef |
|---|---|---|
| Attack | 6 frame | **10 frame** |
| Dash | 4 frame | **8 frame** |
| Death | 4-7 frame | **10 frame** |
| Walk/Run | 6-8 frame | 8-10 frame (sonraki sprint) |
| Idle | 6-8 frame | 8 frame (sonraki sprint) |
