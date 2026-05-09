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

### ⚠️ 1. Interpolation — 64×64 SINIRI
Interpolation tool **sadece 64×64** canvas kabul eder. 128×128 sprite → çalışmaz.
Production Playbook'taki walk/attack cycle pipeline'ı revize edilmeli.
**Çözümler:**
1. 64×64'te üret → 16 frame → Unity'de scale
2. 128×128 için skeleton animation kullan
3. 64×64'te çalış → Aseprite'ta 2x nearest-neighbor → 128×128

### ⚠️ 2. Animate with Text Pro — Frame Sayısı
| Input Boyutu | Çıktı | Maliyet |
|---|---|---|
| 32×32 veya 64×64 | **16 frame** (4×4 grid) | 20 gen |
| 65–128px | **4 frame** (2×2 grid) | 20 gen |
| 129–170px | **4 frame** (2×2 grid) | 25 gen |
| 171–256px | **4 frame** (2×2 grid) | 40 gen |

**RIMA Öneri:** 64px'te üret, 16 frame → daha akıcı animasyon, aynı maliyet.

### 3. MCP Async Workflow
MCP araçları anında job_id döndürür, arka planda 2-5 dk işler. `get_*` tool ile sorgula.
Birden fazla animasyon için: hepsini sıraya ekle, paralel işlenir.

### 4. MCP Kurulum
```
claude mcp add pixellab https://api.pixellab.ai/mcp -t http -H "Authorization: Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0"
```
Docs referansı: `@https://api.pixellab.ai/mcp/docs`

---

## NLM Sync Durumu
Bu klasör NLM'e sync edilmiş — sorgu: RIMA Game Design Knowledge Base
