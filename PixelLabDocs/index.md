# PixelLabDocs — RIMA Referans Klasörü

Bu klasör PixelLab dokümantasyonunun tamamını içerir.
İçerik iki kaynaktan birleştirildi:
- **Gemini 3.1 Pro High** — resmi docs sitesinden tam scrape (`https://www.pixellab.ai/docs`)
- **Claude** — MCP API docs + RIMA'ya özel notlar ve playbook referansları

*Son güncelleme: 2026-05-10*

---

## Klasördeki Dosyalar

### Genel / Başlangıç
| Dosya | İçerik |
|---|---|
| `getting-started.md` | Init image ve inpainting başlangıç rehberi |
| `ways-to-use-pixellab.md` | Web app / Aseprite / Pixelorama / API seçenekleri |
| `installation.md` | Aseprite eklentisi kurulumu |
| `introduction-pixelorama.md` | Pixelorama ile çalışma |
| `faq.md` | Sık sorulan sorular |
| `api-reference.md` | v2 API endpoint listesi (tüm endpoint'ler) |
| `mcp_docs.md` | MCP tool listesi ve parametreleri (FastMCP'den) |

### Tool Options (Ortak Ayarlar)
| Dosya | İçerik |
|---|---|
| `general.md` | Genel ayarlar (seed, output method) |
| `init-image.md` | Init image strength ve kullanımı |
| `inpainting.md` | Inpainting mask ayarları |
| `guidance.md` | Guidance weight kontrolü |
| `character.md` | Karakter view/direction ayarları |
| `color.md` | Target palette ve renk ayarları |
| `camera.md` | Kamera açısı seçenekleri |
| `projection.md` | Projeksiyon tipleri (isometric, top-down, side) |

### Create Image
| Dosya | İçerik |
|---|---|
| `consistent-style.md` | Style referansıyla tutarlı üretim (Pro) |
| `create-sl-image-pro.md` | S-L boyut image üretimi (Pro) |
| `create-image-flux.md` | M-XL image üretimi (yeni) |
| `image-to-image-depth.md` | Derinlik haritasıyla image-to-image |
| `style.md` | S-M image üretimi |
| `style_old.md` | S-M image üretimi (eski) |
| `pose-to-image.md` | Poz referansından image üretimi |
| `image-to-pixel-art.md` | Fotoğraf → pixel art dönüşümü |
| `create-instant-character.md` | Tek tıkla yürüyen karakter (Experimental) |

### Edit Image
| Dosya | İçerik |
|---|---|
| `edit-image.md` | Temel image düzenleme |
| `edit-image-pro.md` | Pro düzenleme — text/reference yöntemi + RIMA Weapon Pass notu |
| `remove-background.md` | Arkaplan kaldırma |
| `resize.md` | Akıllı yeniden boyutlandırma |
| `unzoom-pixelart.md` | Pixel art genişletme (outpainting) |
| `reshape.md` | Şekil dönüşümü |
| `reduce-colors.md` | Renk sayısı azaltma |

### Rotate
| Dosya | İçerik |
|---|---|
| `rotate.md` | 4/8 yönlü sprite üretimi |
| `create-8-rotations-pro.md` | Pro 8-yönlü sprite (referanslı) |
| `rotating-a-character.md` | Karakter döndürme rehberi |

### Animate
| Dosya | İçerik |
|---|---|
| `animate-with-text-new.md` | Animate with Text NEW parametreleri |
| `animate-with-text-pro.md` | Animate with Text Pro — frame tablosu + RIMA notu |
| `text2animation.md` | Animasyonlu object/karakter oluşturma (Pro) |
| `animation-to-animation.md` | Animasyonu başka karaktere transfer |
| `animate-with-skeleton.md` | Skeleton animation — tam workflow + generation modları |
| `edit-animation-pro.md` | Mevcut animasyonu düzenleme (Pro) |
| `transfer-outfit-pro.md` | Kıyafet transferi (Pro) |
| `re-pose.md` | Karakteri yeniden pozlandırma |
| `animation.md` | Animate with Text (eski versiyon) |
| `interpolation.md` | Keyframe arası frame üretimi — ⚠️ 64x64 limit + RIMA workaround'ları |
| `create-animations-automatic.md` | Otomatik animasyon üretimi |

### Map / Tile
| Dosya | İçerik |
|---|---|
| `create-tileset.md` | Top-down Wang tileset (16-tile, terrain transitions) |
| `create-isometric-tile.md` | Bireysel isometric tile + RIMA W1/W2/OBW notu |
| `create-tiles-pro.md` | Gelişmiş tile üretimi — tüm tile tipleri + RIMA F1-F3 notu |
| `create-texture.md` | Texture üretimi |
| `create-map.md` | Harita üretimi (pixflux) |
| `extend-map-v2.md` | Harita genişletme v2 |
| `extend-map.md` | Harita genişletme |
| `extend-map-old.md` | Harita genişletme (eski) |
| `map-tiles.md` | Harita tile rehberi |

### Inpaint
| Dosya | İçerik |
|---|---|
| `inpaint.md` | Temel inpainting |
| `inpaint-v3.md` | Inpaint v3 (Pro) |
| `inpaint-pixpatch-v2.md` | Inpaint M-L pixpatch v2 |

### UI / Extra
| Dosya | İçerik |
|---|---|
| `create-ui-elements.md` | UI element üretimi |
| `create-ui-elements-pro.md` | Pro UI üretimi |
| `try-on.md` | Kıyafet deneme (Experimental) |
| `multi-image.md` | Çoklu image işleme (Experimental) |

---

## RIMA Kritik Bulgular
*(2026-05-10 NLM sorgusu + Opus kararı — kanonik)*

### ❌ 1. Eski Interpolation ÖLÜ
64×64 constraint'li eski Interpolation tool kullanılmaz.

**Yerine iki araç:**
- **Interpolate NEW (v2):** 252×252 destekli. Run cycle ve 3-segment attack dolgusu için standart.
- **Animation-to-Animation Bridging Mode:** KF1+KF3 girişi → 2 ara frame üretir, 128px+ destekli. Geniş silahlı sınıflarda (Warblade greatsword) kırpma yaşanırsa kullan.

### ⚠️ 2. Animate with Text NEW = v3 Pixflux — ZORUNLU
Eski v2 araçlar %49 oranında çöküyor. v3 araçlar ("NEW" tag'li) zorunlu.

**Canvas: 252×252** — v3'ün dayatması, RIMA'nın tercihi değil. Warblade gibi geniş silahlarda 128px'de kırpma olur, bu yüzden 252px gerekli.

**Pixel budget formülü:** `width × height × frames ≤ 524,288`

| Canvas | Max Frame | Not |
|---|---|---|
| 252×252 | 8 | Proje standardı |
| 160×160 | 16 | Silahsız karakterler minimum safe |
| 128×128 | 16 | Silahlı karakterlerde kırpma riski |

**Unity import:** 252×252 sprite sheet → Aseprite'ta 128×128 crop → Unity.

**⚠️ Attack 9-frame pixel budget:** 252² × 9 = 571,536 → bütçe aşımı. Çözüm: 8 frame'e düş VEYA 2 clip (windup 4 + follow 4).

### 🚫 3. animate_character MCP — KALICI YASAK (2026-05-02)
Tüm karakter animasyonları için MCP kullanılmaz.
**Neden:** 4-frame limit + VFX sprite frame'lerine gömülüyor + run doğal değil.
char_id'ler memory'den kasıtlı silindi. Claude prompt dökümanı hazırlar, kullanıcı PixelLab UI'da uygular.

### 4. Web App vs MCP Karar Matrisi

| Görev | Tool |
|---|---|
| Karakter animasyon (tüm clip'ler) | **Web App ZORUNLU** |
| Karakter base 4-yön (prototip) | Web App |
| Batch 4-yön base (10 sınıf loop) | MCP OK |
| Tile / obje / static prop | MCP OK |
| Animasyon polish | Aseprite |

### 5. Brian's Extreme Pose Method — Hâlâ Geçerli
Run/walk: Extreme Pose A → flip → Pose B → **Interpolate NEW** arası doldur.
Skeleton animation: sadece complex custom attacks için saklı.

### 6. MCP Async Workflow
MCP araçları anında job_id döndürür, arka planda 2-5 dk işler. `get_*` tool ile sorgula. Birden fazla işlemi paralel sıraya ekle.

### 7. MCP Kurulum
```
claude mcp add pixellab https://api.pixellab.ai/mcp -t http -H "Authorization: Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0"
```
Docs referansı: `@https://api.pixellab.ai/mcp/docs`

---

## NLM Sync Durumu
Bu klasör NLM'e sync edilmiş — sorgu: RIMA Game Design Knowledge Base
