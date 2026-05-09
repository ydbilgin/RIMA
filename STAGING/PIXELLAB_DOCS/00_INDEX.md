# PixelLab Docs — RIMA Referans
*Çekilme tarihi: 2026-05-10*
*Kaynak: https://www.pixellab.ai/docs + https://api.pixellab.ai/mcp/docs*

---

## MCP Setup (zaten eklendi)
```
claude mcp add pixellab https://api.pixellab.ai/mcp -t http -H "Authorization: Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0"
```
Docs tip: `@https://api.pixellab.ai/mcp/docs` promptlara eklenebilir.

---

## Dosya Listesi

| Dosya | İçerik |
|---|---|
| `MCP_TOOLS.md` | MCP tool listesi + parametreler |
| `API_REFERENCE.md` | v2 endpoint listesi (llms.txt'den) |
| `ANIMATE_WITH_TEXT_PRO.md` | Animate with Text Pro parametreleri |
| `SKELETON_ANIMATION.md` | Skeleton animation tam workflow |
| `EDIT_IMAGE_PRO.md` | Edit Image Pro parametreleri + maliyet |
| `CREATE_ISOMETRIC_TILE.md` | Create Isometric Tile parametreleri |
| `CREATE_TILES_PRO.md` | Create Tiles Pro — tüm tile tipleri |
| `INTERPOLATION.md` | Interpolation tool — ⚠️ 64x64 limit |

---

## Kritik Bulgular (RIMA'ya Doğrudan Etki)

### 1. Interpolation — 64x64 SINIRI
Interpolation tool **sadece 64x64** canvas kabul ediyor.
Production Playbook'ta 128x128 sprite + Interpolate pipeline var — bu çalışmaz.
**Aksiyon:** Playbook'u revize et veya sprites'ı 64x64'e küçült, Unity'de scale et.

### 2. Animate with Text Pro — Frame tablosu
| Boyut | Output | Maliyet |
|---|---|---|
| 32x32 veya 64x64 | 16 frame (4×4 grid) | 20 gen |
| 65-128px | 4 frame (2×2 grid) | 20 gen |
| 129-170px | 4 frame (2×2 grid) | 25 gen |
| 171-256px | 4 frame (2×2 grid) | 40 gen |

→ 128px'de sadece 4 frame çıkıyor. 64px'de 16 frame — daha iyi animasyon için 64px tercih et.

### 3. MCP Araçları (Web App değil MCP ile yapılabilir)
- `create_character` — 4 veya 8 yön karakter
- `animate_character` — mevcut karaktere animasyon
- `create_isometric_tile` — isometric tile
- `create_tiles_pro` — gelişmiş tile üretimi
- `create_topdown_tileset` — Wang tileset
- `create_map_object` — transparent prop

### 4. Async Workflow
MCP araçları anında job ID döndürür, işlem 2-5 dk arka planda devam eder.
`get_*` araçlarıyla durum sorgulanır.

---

## Docs Bölümleri (Web App — manuel)
- **Introduction** (5 sayfa): Kurulum, FAQ
- **Guides** (3 sayfa): Init images, Map creation, Rotating
- **Create Image** (8 araç): style ref, flux, pose-to-image
- **Edit Image** (5 araç): bg removal, resize, inpaint
- **Rotate** (2 araç): 4/8 yön
- **Animate** (10 araç): text, skeleton, interpolation
- **Map** (8 araç): tileset, isometric, terrain
- **Inpaint** (3 versiyon)
- **Reduce Colors**
- **Experimental** (3 beta)
- **Extra Tools** (5 araç)
- **Tool Options** (7 kategori)
