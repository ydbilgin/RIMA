# RIMA — ACT 1 TILE ÜRETİM REHBERİ
## PixelLab "Create Tiles PRO" Kullanım Kılavuzu

> **Bütçe:** 130 gen → 3 batch × 20 gen = **60 gen** → kalan **70 gen** animasyonlar için
> Her batch birden fazla tile varyasyonu üretir!

---

## ⚠️ ÖNEMLİ AYARLAR (HER BATCH İÇİN)

**Değiştirmen GEREKEN ayarlar:**

| Ayar | Mevcut (ekrandaki) | Olması Gereken | Neden |
|------|-------------------|----------------|-------|
| **Tile type** | Isometric | **Top-down** | Oyunumuz 2D top-down, isometric DEĞİL |
| **Tile size** | 32px | **32px** ✅ doğru | — |
| **Outline mode** | Outline (default) | **No outline** | Tile'lar seamless olmalı, outline kenarları bozar |

---

## BATCH 1: ZEMİN TILE'LARI (20 gen)

### Ayarlar
| Ayar | Değer |
|------|-------|
| Tile type | **Top-down** |
| Tile size | **32px** |
| View angle | **90°** (tam yukarıdan) |
| Thickness | **0%** (zemin düz, kalınlık yok) |
| Outline mode | **No outline** |

### Description (kopyala-yapıştır):
```
1) dark stone floor base - dark grey cracked stone dungeon floor
2) stone floor with cracks - same style but with visible damage cracks
3) stone floor mossy - same stone but with green moss patches
4) stone floor with old blood stains - same stone with dried dark red blood
```

### Style tiles (opsiyonel):
Varsa herhangi bir dark dungeon pixel art tile'ı upload et, yoksa boş bırak.

---

## BATCH 2: DUVAR TILE'LARI (20 gen)

### Ayarlar
| Ayar | Değer |
|------|-------|
| Tile type | **Top-down** |
| Tile size | **32px** |
| View angle | **30°** (duvarlar biraz yan görünmeli) |
| Thickness | **50%** (duvar kalınlığı belirgin olsun) |
| Outline mode | **No outline** |

### Description (kopyala-yapıştır):
```
1) dark stone wall front face - thick dark castle wall brick pattern
2) dark stone wall top surface - wall seen from above, flat stone top
3) stone wall corner piece - where two walls meet at 90 degrees
4) stone wall with iron bracket - wall with metal reinforcement detail
```

---

## BATCH 3: KAPI + DEKOR (20 gen)

### Ayarlar
| Ayar | Değer |
|------|-------|
| Tile type | **Top-down** |
| Tile size | **32px** |
| View angle | **45°** (dekoratif elementler 3/4 görünsün) |
| Thickness | **30%** |
| Outline mode | **Outline (default)** ← sadece bu batch'te outline OK |

### Description (kopyala-yapıştır):
```
1) iron gate door - closed dungeon iron bar gate, dark metal
2) stone pillar column - tall round stone pillar top-down view
3) wall torch with fire - mounted torch on wall bracket with orange flame
4) floor debris with bones - scattered bones and stone rubble on ground
```

---

## ÜRETİM SONRASI YAPILACAKLAR

### 1. Dosya kaydetme
Her batch'ten çıkan tile'ları şuraya kaydet:
```
Assets/Sprites/Tiles/Act1/
  ├── floor_stone_base.png
  ├── floor_stone_cracked.png
  ├── floor_stone_mossy.png
  ├── floor_stone_blood.png
  ├── wall_front.png
  ├── wall_top.png
  ├── wall_corner.png
  ├── wall_bracket.png
  ├── door_iron_gate.png
  ├── pillar_stone.png
  ├── torch_wall.png
  └── debris_bones.png
```

### 2. Import Settings (Unity'de)
Her PNG için Inspector'da:
- **Texture Type:** Sprite (2D and UI)
- **Pixels Per Unit:** **32**
- **Filter Mode:** **Point (no filter)** ← pixel art için ZORUNLU
- **Compression:** None
- **Max Size:** 32 veya 64

### 3. Tile Asset Oluşturma
Claude'a söyle: "Tile'lar hazır, RoomBuilder'ı güncelle"
→ Otomatik olarak Tile asset'ler oluşturulup tilemap'e atanacak.

---

## BÜTÇE ÖZETİ

| Kullanım | Gen |
|----------|-----|
| Batch 1: Zemin | 20 |
| Batch 2: Duvar | 20 |
| Batch 3: Kapı+Dekor | 20 |
| **Toplam tile** | **60** |
| **Kalan (animasyonlar)** | **70** |

---

## RENK REFERANSI
Tüm tile'lar bu palette uyumlu olmalı:
- **Ana taş:** `#3C3832` (koyu gri-kahve)
- **Zemin:** `#594A36` (sıcak kahve)
- **Aksan:** `#8B2500` (koyu kırmızı — kan, tehlike)
- **Metal:** `#5A5A6A` (soğuk gri — demir)
- **Işık:** `#FFB347` (turuncu — meşale ışığı)
- **Yosun:** `#2D5A3D` (koyu yeşil)
