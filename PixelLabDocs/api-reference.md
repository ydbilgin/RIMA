Source: https://api.pixellab.ai/v2/llms.txt

# PixelLab API v2 Reference

## Base Info
- **Base URL:** `https://api.pixellab.ai/v2`
- **Auth:** Bearer token (header)
- **Response:** `{ success, data, error, usage }`
- **Usage tracking:** `credits_used`, `remaining_credits`, `generations_used`, `remaining_generations`

---

## Endpoints

### Character Creation
| Endpoint | Description |
|---|---|
| `POST /create-character-with-4-directions` | 4-direction character |
| `POST /create-character-with-8-directions` | 8-direction character |
| `POST /create-character-pro` | Pro mode, 8 directions |
| `POST /create-character-v3` | V3 model, 8 rotations |

### Animation
| Endpoint | Description |
|---|---|
| `POST /animate-with-text` | Text-based animation (64x64 fixed) |
| `POST /animate-with-text-v2` | Pro text animation (32-256px, async) |
| `POST /animate-with-text-v3` | V3 — optional end frame |
| `POST /animate-with-skeleton` | Skeleton-guided animation |
| `POST /interpolation-v2` | Generate frames between keyframes |
| `POST /edit-animation-v2` | Multi-frame editing |

### Image Generation
| Endpoint | Description |
|---|---|
| `POST /generate-image-v2` | Pro image gen |
| `POST /generate-with-style-v2` | Style-matched gen |
| `POST /generate-ui-v2` | UI element gen |
| `POST /create-image-pixflux` | Legacy |
| `POST /create-image-pixen` | Legacy |
| `POST /create-image-bitforge` | Legacy |

### Tileset & Map
| Endpoint | Description |
|---|---|
| `POST /create-tileset` | Top-down Wang tileset (async) |
| `POST /create-tileset-sidescroller` | Platform tileset |
| `POST /create-isometric-tile` | Individual isometric tile |
| `POST /create-tiles-pro` | Advanced tile generation |

### Objects
| Endpoint | Description |
|---|---|
| `POST /objects` | 1 or 8-direction object |
| `POST /map-objects` | Legacy map object |
| `POST /animate-object` | Object animation |
| `POST /vary-object` | Object variation |

### Editing & Processing
| Endpoint | Description |
|---|---|
| `POST /edit-images-v2` | Batch image edit (Pro) |
| `POST /inpaint-v3` | AI inpainting (Pro) |
| `POST /remove-background` | Remove background |
| `POST /resize` | Smart upscale |
| `POST /image-to-pixelart` | Photo → pixel art |

### Rotation
| Endpoint | Description |
|---|---|
| `POST /generate-8-rotations-v2` | Pro 8-rotation |
| `POST /generate-8-rotations-v3` | V3 reference-based rotation |

### Management
| Endpoint | Description |
|---|---|
| `GET /characters` | Character list |
| `GET /characters/{id}` | Character detail |
| `GET /objects` | Object list |
| `GET /background-jobs/{id}` | Async job status |
| `GET /balance` | Credit/usage info |

---

## HTTP Status Codes
| Code | Meaning |
|---|---|
| 200 | Success |
| 202 | Async job queued |
| 401 | Invalid token |
| 402 | Insufficient credits |
| 422 | Validation error |
| 423 | Resource still generating |
| 429 | Rate limit |

---

## Common Parameters
- `description`: Prompt (1-2000 characters)
- `image_size`: `{width, height}` (pixels)
- `seed`: Optional for reproducibility
- `no_background`: Transparent output
- `async_mode`: Background processing (automatic for async endpoints)
