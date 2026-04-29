# PIXELLAB_PIPELINE_DECISION.md
> **Ne zaman yükle:** Art üretim session'ı başında workflow kararı için.

---

## WORKFLOW KARARI

### Tamamen MCP ile güvenli

| İş | Araç | Koşul |
|---|---|---|
| Karakter BASE prototype | `create_character` standard | → `prototype_temp` olarak kaydet |
| Karakter BASE final | `create_character` pro | → `candidate_base`, onay beklenir |
| Template animasyon testi | `animate_character` template | → `animation_test`, onay beklenir |
| Tile üretimi (seed ile) | `create_tiles_pro` | Seed kaydet → reproducible |
| Top-down terrain tileset | `create_topdown_tileset` | Wang zinciri için base_tile_id kaydet |
| Mevcut karakteri getir | `get_character` | Sadece okuma |

### Hybrid daha iyi (MCP + Aseprite)

| İş | Neden hybrid |
|---|---|
| Palette sabitleme | MCP renkleri garanti etmez; Aseprite'ta manuel fix gerekir |
| Artifact cleanup | Dağınık piksel, kenar tüyü → Aseprite eraser |
| Seamless tile kontrolü | Tile edge birleşimi görsel kontrol ister |
| Renk varyantı üretimi | MCP base → Aseprite palette swap |

### Manuel Aseprite zorunlu (MCP araç yok)

| İş | Neden |
|---|---|
| VFX sprite sheet (slash, hit spark, parçacık) | PixelLab VFX aracı yok |
| UI elementleri (HP bar, skill slot, panel) | PixelLab UI aracı yok |
| Logo / tipografi | Piksel el işçiliği gerekiyor |
| Frame timing polish | Animation timeline düzenlemesi |
| Smear frame, silhouette correction | Piksel düzeyi kontrol |

---

## BASE APPROVAL SİSTEMİ

```
MCP üretim → prototype_temp
        ↓
Sen görürsün → candidate_base (bekleme)
        ↓
Sen onaylarsın → approved_base
        ↓
Buradan animasyon + varyant üretimi başlar
```

**KURAL: `approved_base` olmadan animasyon zinciri kurma.**
Test amaçlı `animation_test` üretilebilir ama `approved_base` referansı olmadan büyük aile üretme.

---

## ANİMASYON KALİTE KARARI

| Animasyon tipi | MCP yeterli mi? | Editor şart mı? |
|---|---|---|
| Template animasyon (walk, idle, death) | ✅ Genelde evet | Titreme varsa Aseprite |
| Custom animasyon (saldırı tarzı hareketler) | ⚠️ Pahalı, değişken | Kontrol et, gerekirse Aseprite |
| Smear frame | ❌ | Aseprite manuel |
| Frame timing ayarı | ❌ | Aseprite Timeline |
| Silhouette düzeltme | ❌ | Aseprite, piksel piksel |

---

## VALIDATION BATCH (küçük test — BÜYÜK ÜRETİMDEN ÖNCE)

| Asset | Araç | Aşama | Gen |
|---|---|---|---|
| Warblade hero | `create_character` pro, 96px, 8 yön | → `candidate_base` | ~30 |
| ShardWalker enemy | `create_character` standard, 64px, 8 yön | → `prototype_temp` | 8 |
| Broken pillar prop | `create_tiles_pro`, 32px, seed=7, 1 tile | → `prototype_temp` | — |
| Act 1 floor tile | `create_tiles_pro`, 16px, seed=42, 4 tile | → `prototype_temp` | — |
| Slash VFX concept | Aseprite manuel, 32x32, 5 frame | → `animation_test` | — |

**Toplam: ~38 gen. Onay ver → başlayayım.**
