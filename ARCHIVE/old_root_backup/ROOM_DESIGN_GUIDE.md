# RIMA — Profesyonel Oda Tasarım Rehberi
> Tarih: 2026-04-10
> Amaç: Hades kalitesinde dungeon odaları — pixel art top-down

---

## 🎯 Hedef Görünüm

Karanlık zindan, renkli ışık kaynakları, zemin çukurları, büyük dekor objeleri.
Hades Tartarus'un pixel art versiyonu.

---

## ADIM 1: IŞIK SİSTEMİ (PixelLab gereksiz — Claude yapar)

Unity 2D URP Light sistemi:
- **Global Light 2D:** Intensity 0.3 (karanlık ortam)
- **Meşale ışıkları:** Point Light 2D, turuncu (#FF8833), radius 4, intensity 1.5
- **Çukur ışıkları:** Point Light 2D, kırmızı (#FF2200), radius 2, intensity 0.8
- **Kapı ışıkları:** Point Light 2D, mavi (#3388FF), radius 3, intensity 1.0

> Sonuç: Anında atmosfer. Meşaleler etrafı aydınlatır, geri kalan karanlık.

---

## ADIM 2: ZEMİN + ÇUKUR — PixelLab Üretimi

### Batch A: Çukur (Pit) Kenar Tile'ları
**Tool:** Create Tiles PRO (20 gen)
**Ayarlar:**
- Tile type: Square top-down
- View angle: Top-down
- Outline: No outline
- Size: 32x32

**Description (4 satır):**
```
1) dark stone floor pit edge top - grey stone floor tile with a dark chasm edge on the bottom side, showing depth with dark gradient falling into blackness
2) dark stone floor pit edge left - grey stone floor tile with a dark chasm edge on the right side, crumbling stone edge falling into dark void
3) dark stone floor pit corner top-left - grey stone floor tile with dark chasm edges on bottom and right, corner piece showing depth into darkness
4) dark stone floor pit center - completely dark void tile, pitch black abyss with faint red glow from below, no floor visible
```

### Batch B: Zemin Aksan Tile'ları (opsiyonel, ileride)
```
1) stone floor with thin crack line - subtle single crack across grey stone
2) stone floor with small puddle - tiny dark water puddle on stone
3) stone floor with dust pile - small pile of grey dust on stone
4) stone floor with chain links - rusty iron chain links lying on stone floor
```

---

## ADIM 3: DUVAR AUTO-TILE — PixelLab Üretimi

### Batch C: Duvar Sistemi (9 parça)
**Tool:** Create Tiles PRO (20 gen)
**Ayarlar:** Aynı (Square top-down, no outline, 32x32)

**Description:**
```
1) dungeon wall top surface - flat grey stone wall seen from above, top face of a thick wall, uniform dark grey stone blocks
2) dungeon wall front face - front-facing dark stone wall with depth shadow at bottom, showing wall thickness and height from side view
3) dungeon wall left edge - stone wall ending on the right side, showing wall depth from left perspective, dark shadow on right
4) dungeon wall right edge - stone wall ending on the left side, showing wall depth from right perspective, dark shadow on left
```

### Batch D: Duvar Köşe + Gölge
```
1) dungeon wall outer corner top-left - stone wall corner piece, two walls meeting at 90 degrees seen from above, shadows on inner sides
2) dungeon wall inner corner top-left - concave stone wall corner, two walls meeting inward, darker shadows in the corner
3) wall shadow edge horizontal - semi-transparent dark gradient shadow strip, cast by wall onto floor, fading from dark to transparent downward
4) wall shadow edge vertical - semi-transparent dark gradient shadow strip, cast by wall onto floor, fading from dark to transparent rightward
```

---

## ADIM 4: BÜYÜK DEKOR OBJELERİ — PixelLab Üretimi

### Batch E: Ana Dekor Sprite'ları
**Tool:** Create Character veya Edit Image PRO (daha ucuz!)
**Size:** 64x64 veya 64x96 (büyük objeler)

**Üretilecek objeler:**
```
1) Stone brazier with orange fire - 64x64px top-down pixel art, dark stone bowl on pedestal with bright orange flames burning, dungeon decoration
2) Tall stone column pillar - 64x96px top-down pixel art, thick round stone pillar with cracks and moss, casting shadow, seen slightly from above
3) Broken stone statue torso - 64x64px top-down pixel art, crumbled ancient warrior statue with broken sword, half collapsed
4) Iron cage with bones - 64x64px top-down pixel art, rusty hanging iron cage with skeleton remains inside, dungeon prison decoration
```

### Batch F: Ek Dekor (opsiyonel)
```
5) Blood pool on floor - 48x48px, dark red blood pool spreading on stone, splatter edges
6) Bone pile with skull - 48x48px, pile of bones with a skull on top, grey-white
7) Wooden barrel broken - 48x48px, smashed wooden barrel with splinters, top-down view
8) Wall chain shackles - 32x48px, rusty chains hanging from wall bracket, iron shackles
```

---

## ADIM 5: UNITY ENTEGRASYONU (Claude yapar)

Üretim bittikten sonra Claude şunları yapar:

### 5.1 Lighting
- [ ] URP 2D Renderer'a Light2D ekle
- [ ] Global Light intensity 0.3
- [ ] Meşale prefabı: Sprite + Point Light 2D + flicker script

### 5.2 Pit Sistemi
- [ ] Rule Tile: çukur kenarları otomatik seçilsin
- [ ] Çukur collider: hasar veya düşme
- [ ] RoomBuilder: odaya 1-3 rastgele çukur yerleştirme

### 5.3 Auto-Tile Duvarlar
- [ ] Unity Rule Tile: 9-slice duvar sistemi
- [ ] Gölge tile'ları: duvar kenarında otomatik
- [ ] Composite Collider 2D: duvarlar arası boşluk yok

### 5.4 Dekor Yerleştirme
- [ ] Dekor prefabları: SpriteRenderer + collider (optional)
- [ ] Sorting order: Floor < Decor < Player < UI
- [ ] RoomBuilder: odaya 2-4 rastgele dekor yerleştirme
- [ ] Işık kaynakları dekorlara bağlı (brazier → point light)

---

## ÜRETİM MALİYET HESABI

| Batch | İçerik | Tool | Gen Maliyeti |
|-------|--------|------|-------------|
| A | Çukur kenar (4 tile) | Create Tiles PRO | 20 gen |
| B | Zemin aksan (4 tile) | Create Tiles PRO | 20 gen |
| C | Duvar sistemi (4 tile) | Create Tiles PRO | 20 gen |
| D | Duvar köşe+gölge (4 tile) | Create Tiles PRO | 20 gen |
| E | Büyük dekor (4 sprite) | Create/Edit PRO* | 4-16 gen |
| F | Ek dekor (4 sprite) | Create/Edit PRO* | 4-16 gen |

**Toplam: 88-112 gen** (6 batch)

*Not: Dekor sprite'ları için Create Character veya Edit Image PRO
daha ucuz olabilir (4 gen/adet). Create Tiles PRO sadece tile için.*

---

## ÖNCELİK SIRASI

1. **🔥 Batch A (çukurlar)** — En büyük görsel etki
2. **🔥 Batch C (duvar sistemi)** — Duvarlar profesyonel olur
3. **🔥 Batch E (büyük dekor)** — Mekan hissi
4. ⬜ Batch D (duvar köşe) — Polish
5. ⬜ Batch B (zemin aksan) — İnce detay
6. ⬜ Batch F (ek dekor) — Son rötuş

> İlk 3 batch (A+C+E) = 44-56 gen ile odalar profesyonel görünür.

---

## IŞIK — HEMEN YAPILABİLİR (Gen gereksiz)

Işık sistemi PixelLab'sız yapılır. Claude'a "ışık kur" de, 5 dakikada hazır.
Bu tek başına bile görüntüyü %50 iyileştirir.
