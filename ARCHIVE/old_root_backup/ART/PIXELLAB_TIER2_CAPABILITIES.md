# PIXELLAB_TIER2_CAPABILITIES.md
> **Ne zaman yükle:** PixelLab MCP çağrısı yapmadan önce limit/maliyet kontrolü.

---

## ARAÇLAR VE MALİYETLER

### create_character
| Mod | Maliyet | Yön | Süre | Seed |
|---|---|---|---|---|
| standard | 1 gen/yön | 4 veya 8 | 2-5 dk | ❌ yok |
| pro | 20-40 gen (toplam) | her zaman 8 | 3-5 dk | ❌ yok |

**ÖNEMLİ:** Karakterlerde seed yok → reproducibility sadece `character_id` ile sağlanır. ID'yi kaybet = karakteri kaybettin.

Pro mod parametreleri: sadece `description`, `name`, `body_type`, `template`, `size`, `view`.
Pro mod şunları **yoksayar**: outline, shading, detail, ai_freedom, proportions.

### animate_character
| Mod | Maliyet | Yön | Süre |
|---|---|---|---|
| template | 1 gen/yön | karakter kaç yönde oluştuysa | 2-4 dk |
| custom | 20-40 gen/yön | belirtilmezse sadece south | 2-4 dk/yön |

**KURAL:** Custom animasyon = `confirm_cost=true` ASLA ilk çağrıda kullanma. Önce maliyet göster, kullanıcı onaylasın.

Template animasyon listesi (humanoid, 47 adet):
`breathing-idle`, `fight-stance-idle-8-frames`, `walking-6-frames`, `running-6-frames`, `running-slide`, `lead-jab`, `falling-back-death` + 40 daha.

### create_tiles_pro
| Parametre | Değer aralığı | Not |
|---|---|---|
| n_tiles | 1,2,4,6,8,9,10,12,16 | 3,5,7 geçersiz |
| tile_size | 16-128px | 32px önerilen |
| tile_type | square_topdown, isometric, hex, hex_pointy, octagon | RIMA: square_topdown |
| tile_view | top-down, high top-down, low top-down, side | RIMA: low top-down |
| seed | integer | ✅ VAR — reproducible |
| outline_mode | outline, segmentation | segmentation daha temiz çıktı |
| Süre | ~15-30 saniye | async |

### create_topdown_tileset (Wang tileset)
- 16 tile (transition_size < 1.0) veya 23 tile (transition_size = 1.0)
- Base tile ID ile zincir oluşturulabilir (terrain transitions)
- Seed: ✅ (shading/detail'de)
- Süre: ~100 saniye

---

## JOB LİMİTLERİ

```
Tier 2: 8 eş zamanlı job

Pro mode (8 yön) = 1 job → aynı anda 8 tane çalıştırılabilir
Template anim (8 yön) = 1 job
Custom anim = 1 job/yön → 8 yön = 8 job, Tier 2'yi doldurur

Paralel çalıştırmadan önce aktif job sayısını kontrol et.
```

---

## BOYUT → MALİYET İLİŞKİSİ

Pro mod maliyeti boyuta göre değişir:
- Küçük karakter (≤48px): ~20 gen
- Orta (64-96px): ~30 gen
- Büyük (128px): ~40 gen

---

## BİLİNEN KISITLAMALAR

- Karakter seed yok → her üretim farklı çıkabilir
- VFX için özel araç yok → Aseprite manuel
- UI sprite için araç yok → Aseprite manuel
- Logo için araç yok → Aseprite manuel
- Quadruped template: sadece bear, cat, dog, horse, lion
- Custom animasyon pahalı → template'leri kullan, yetmezse custom
