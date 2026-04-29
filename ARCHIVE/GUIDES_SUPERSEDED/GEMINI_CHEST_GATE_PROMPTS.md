# RIMA — Gemini Referans Promptları: Chest + Gate
> **Amaç:** Gemini'den referans görsel üret → PixelLab Edit Image PRO'ya ver → pixel art çıktı al
> **Bu görseller asset DEĞİL** — sadece Edit Image PRO'nun başlangıç noktası

---

## Nasıl Kullanılır

```
1. gemini.google.com → yeni sohbet
2. Aşağıdaki promptu yapıştır → üret
3. Çıktıyı KONTROL ET (açı doğru mu?)
4. Sağ tık → "Farklı kaydet" → masaüstü (geçici dosya)
5. pixellab.ai → Edit Image PRO
     Image to edit: Gemini çıktısı
     Style Image:   RIMA_DarkFantasy_Concept.png
     Method:        Edit with text
     ai_freedom:    400
     Remove background: ON
6. Edit PRO prompt'unu yaz (her adımın altında var)
```

---

## AÇI KONTROLÜ — Her Görselde Zorunlu

**60° low top-down** = kamera yukarıdan öne bakıyor, kuş bakışı DEĞİL.

| ✅ Doğru | ❌ Yanlış |
|----------|----------|
| Objenin ön yüzü açıkça görünüyor | Düz tepeden, sadece üst yüz |
| Hafif yukarıdan bakış açısı | Tam yan profil |
| Derinlik/3D hissi var | Isometric diamond grid |

Yanlışsa aynı Gemini sohbetinde yaz:
```
Regenerate with camera positioned slightly above and in front of the object.
The front face of the object must be clearly visible, not a pure top-down view.
Think Hero Siege or Diablo 2 ARPG camera angle — slightly overhead, front-facing.
```

---

## CHEST — chest_closed.png için Referans

### Gemini Prompt

```
A dark fantasy treasure chest for a top-down ARPG video game, viewed from a 
slightly elevated angle — camera positioned above and slightly in front, 
so the front face of the chest is clearly visible with the lid on top.
Think Hero Siege or Diablo 2 camera perspective.

The chest is:
- Fully closed and locked with heavy iron chains crossing diagonally over the lid,
  a large iron padlock at the chain intersection
- Ornate dark metal plating with bone and skull relief carvings on the front face
- Worn leather straps reinforcing the lid edges
- Aged, battle-worn, rust stains on metal corners
- Slightly glowing cold blue rift energy cracks running through the metal seams

Color palette: dark iron grey (#2C2C2C), worn tarnished gold accents, cold blue (#7BA7BC) crack glow
Background: pure white, isolated object only
Style: detailed concept art, clean edges, no pixel art, high contrast
Size: object centered, occupying most of the frame
```

### Kontrol
- [ ] Ön yüz görünüyor mu? (zincir + kilit nette)
- [ ] Kapak üstte, gövde önde — 3D derinlik var mı?
- [ ] Zincirler çapraz, büyük kilit ortada mı?
- [ ] Beyaz arka plan mı?

### Edit Image PRO Prompt (Pixellab'a girilecek)
```
KISA: dark fantasy iron treasure chest, closed, chained padlock, ARPG pixel art

DETAYLI:
Dark fantasy treasure chest, fully closed, heavy iron chains crossing diagonally 
with large padlock at center, bone and skull carvings on front face, 
worn leather straps, aged battle-worn metal with rust,
cold blue rift energy cracks in seams,
classic ARPG top-down perspective with slight depth and 3D feel,
visible front face of chest body and lid, slight top face visible,
Hero Siege style angle, dark fantasy pixel art,
transparent background, muted dark gold and iron color palette
```

---

## CHEST — chest_open_cursed.png için Referans

> Bu için ayrı Gemini üretimi gerekmez.
> chest_closed Gemini görselini PixelLab Inpaint'e ver, kapak + iç alanı maskele.
> Referans gerekmez — prompt yeterli.

---

## GATE — gate_base.png için Referans

### Gemini Prompt

```
A dark fantasy dungeon gateway arch for a top-down ARPG video game, viewed from 
slightly above and directly in front — camera at about 60 degrees elevation, 
facing the front of the arch. Think Diablo 2 or Hero Siege dungeon entrance.

The arch is:
- A heavy stone archway set into a dungeon wall, the wall extending left and right
- Thick carved dark stone blocks forming the arch frame, clearly visible depth/thickness
- Skull and grimacing face relief carvings along the arch frame edges
- Cold blue glowing rift energy cracks running through the stone blocks
- Faint warm torch amber glow on the stone side surfaces
- The arch interior opening is pure black void — completely empty, dark, nothing inside

The wall sections on either side of the arch:
- Dark stone brick, same texture as the arch
- Shadow at the base where wall meets floor
- Slight torch sconce bracket visible on one side

Color palette: dark stone grey (#4A3F3F), cold blue crack glow (#7BA7BC), torch amber accent
Background: pure white, isolated object only
Style: detailed concept art, front-facing architectural illustration, high contrast, no pixel art
Composition: arch centered, wall visible on sides, slight overhead perspective showing arch depth
```

### Kontrol
- [ ] Kemer ön yüzü net görünüyor mu? (taş kalınlığı, kabartmalar)
- [ ] İç kısım tamamen siyah/boş mu?
- [ ] Duvar sol ve sağda görünüyor mu?
- [ ] 60° açı: hem üst hem ön yüz görünüyor mu?
- [ ] Beyaz arka plan mı?

### Edit Image PRO Prompt (Pixellab'a girilecek)
```
KISA: dark fantasy dungeon stone arch gateway, empty dark void interior, ARPG pixel art

DETAYLI:
Heavy dark fantasy dungeon gateway arch set into a stone wall,
classic ARPG top-down perspective with slight depth and 3D feel,
visible front face of arch frame and stone thickness, Hero Siege style angle,
thick carved dark stone blocks, skull and bone relief carvings on arch edges,
cold blue rift energy cracks running through stone,
faint torch amber glow on stone side surfaces,
interior of arch opening is pure black void — completely empty,
no door, no chains, no content inside,
dark fantasy pixel art, transparent background outside the arch,
dark stone and cold blue color palette
```

---

## GATE — gate_locked.png için Referans (Opsiyonel)

> Genellikle gate_base'den Inpaint ile üretmek daha hızlı.
> Ama özellikle zincir detayı için Gemini referansı istersen:

### Gemini Prompt

```
Same dark fantasy dungeon gateway arch as before, but now the arch interior is 
sealed with heavy iron chains crossing the opening diagonally, 
a large iron padlock at the center intersection point.
Thick rusty chain links, completely blocking passage.
Deep dark fog and void visible behind the chains through the gaps.
Same stone arch frame, same camera angle (60 degrees overhead, front-facing).
Background: pure white
Style: concept art, no pixel art
```

### Edit Image PRO değil — Inpaint kullan
Bu için Edit PRO yerine Inpaint daha doğru:
- Base: `gate_base.png` (pixel art)
- Mask: kemer açıklığının içi
- Style: `RIMA_DarkFantasy_Concept.png`
- Prompt: Chest guide ADIM 6'daki prompt

---

## HIZLI REFERANS — Üretim Sırası

```
Gemini sırası:
1. chest_closed referansı → kaydet
2. gate_base referansı → kaydet
(gate_locked için Gemini isteğe bağlı — Inpaint daha hızlı)

PixelLab sırası (guide'dan):
ADIM 1: chest_closed → Edit Image PRO (Gemini referansı + RIMA style)
ADIM 2: chest_open_cursed → Inpaint (chest_closed üzerine)
ADIM 3: chest_damaged → Edit Image PRO (chest_closed üzerine)
ADIM 4: chest animasyonu → Interpolate
ADIM 5: gate_base → Edit Image PRO (Gemini referansı + RIMA style)
ADIM 6-10: gate varyantları → Inpaint (gate_base üzerine)
ADIM 8: gate animasyonu → Interpolate
```

---

## NOTLAR

- Gemini çıktısı pixel art değil, concept art — bu normal ve istenen
- Edit Image PRO düşük ai_freedom (400) ile Gemini görselinin şeklini alır, RIMA stilini uygular
- Gemini birden fazla varyasyon üretirse en net açılı olanı seç (ön yüz en görünür)
- Zayıf Gemini çıktısı sorun değil — Edit PRO onu düzeltir, sadece şekil/kompozisyon önemli
