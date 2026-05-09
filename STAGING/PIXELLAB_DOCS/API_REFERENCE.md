# PixelLab API v2 Referans
*Kaynak: https://api.pixellab.ai/v2/llms.txt*

## Temel Bilgiler
- **Base URL:** `https://api.pixellab.ai/v2`
- **Auth:** Bearer token (header)
- **Response:** `{ success, data, error, usage }`
- **Usage tracking:** `credits_used`, `remaining_credits`, `generations_used`, `remaining_generations`

---

## Endpoint Listesi

### Karakter Oluşturma
| Endpoint | Açıklama |
|---|---|
| `POST /create-character-with-4-directions` | 4 yönlü karakter |
| `POST /create-character-with-8-directions` | 8 yönlü karakter |
| `POST /create-character-pro` | Pro mod, 8 yön |
| `POST /create-character-v3` | V3 model, 8 rotasyon |

### Animasyon
| Endpoint | Açıklama |
|---|---|
| `POST /animate-with-text` | Text-based animasyon (64×64 sabit) |
| `POST /animate-with-text-v2` | Pro text animasyon (32-256px, async) |
| `POST /animate-with-text-v3` | V3 — optional end frame |
| `POST /animate-with-skeleton` | Skeleton-guided animasyon |
| `POST /interpolation-v2` | Keyframe'ler arası frame üret |
| `POST /edit-animation-v2` | Çoklu frame düzenleme |

### Image Generation
| Endpoint | Açıklama |
|---|---|
| `POST /generate-image-v2` | Pro image gen |
| `POST /generate-with-style-v2` | Style-matched gen |
| `POST /generate-ui-v2` | UI element gen |
| `POST /create-image-pixflux` | Legacy |
| `POST /create-image-pixen` | Legacy |
| `POST /create-image-bitforge` | Legacy |

### Tileset & Map
| Endpoint | Açıklama |
|---|---|
| `POST /create-tileset` | Top-down Wang tileset (async) |
| `POST /create-tileset-sidescroller` | Platform tileset |
| `POST /create-isometric-tile` | Bireysel isometric tile |
| `POST /create-tiles-pro` | Gelişmiş tile üretimi |

### Objects
| Endpoint | Açıklama |
|---|---|
| `POST /objects` | 1 veya 8 yönlü object |
| `POST /map-objects` | Legacy map object |
| `POST /animate-object` | Object animasyonu |
| `POST /vary-object` | Object varyasyonu |

### Editing & Processing
| Endpoint | Açıklama |
|---|---|
| `POST /edit-images-v2` | Batch image edit (Pro) |
| `POST /inpaint-v3` | AI inpainting (Pro) |
| `POST /remove-background` | Arkaplan kaldır |
| `POST /resize` | Akıllı upscale |
| `POST /image-to-pixelart` | Fotoğraf → pixel art |

### Rotation
| Endpoint | Açıklama |
|---|---|
| `POST /generate-8-rotations-v2` | Pro 8-rotasyon |
| `POST /generate-8-rotations-v3` | V3 referanslı rotasyon |

### Management
| Endpoint | Açıklama |
|---|---|
| `GET /characters` | Karakter listesi |
| `GET /characters/{id}` | Karakter detayı |
| `GET /objects` | Object listesi |
| `GET /background-jobs/{id}` | Async job durumu |
| `GET /balance` | Kredi/kullanım bilgisi |

---

## HTTP Status Kodları
| Kod | Anlam |
|---|---|
| 200 | Başarı |
| 202 | Async job sıraya alındı |
| 401 | Geçersiz token |
| 402 | Yetersiz kredi |
| 422 | Validation hatası |
| 423 | Resource hâlâ üretiliyor |
| 429 | Rate limit |

---

## Ortak Parametreler
- `description`: Prompt (1-2000 karakter)
- `image_size`: `{width, height}` (pixel)
- `seed`: Reproducibility için opsiyonel
- `no_background`: Transparent output
- `async_mode`: Arka plan işleme (async endpoint'lerde otomatik)
