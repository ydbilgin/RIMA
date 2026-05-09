# PixelLab MCP Tools Referansı
*Kaynak: https://api.pixellab.ai/mcp/docs + https://www.pixellab.ai/mcp*

## Setup
```
claude mcp add pixellab https://api.pixellab.ai/mcp -t http -H "Authorization: Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0"
```

---

## Araç Tablosu

| Tool | Fonksiyon | Temel Parametreler |
|---|---|---|
| `create_character` | 4 veya 8 yönlü karakter sprite | `description`, `n_directions` (4/8), `size`, `proportions`, `body_type` |
| `animate_character` | Mevcut karaktere animasyon ekle | `character_id` (UUID), `template_animation_id`, `action_description` |
| `get_character` | Karakter data + rotation URL'leri | `character_id` |
| `list_characters` | Karakter envanteri (paginated) | — |
| `delete_character` | Kalıcı silme (onay gerekli) | `character_id` |
| `create_topdown_tileset` | Wang tileset (16-tile) | `lower_description`, `upper_description`, `transition_size`, `lower_base_tile_id`, `view` |
| `get_topdown_tileset` | Tileset durumu + URL'ler | `tileset_id` |
| `list_topdown_tilesets` | Tileset listesi | — |
| `create_sidescroller_tileset` | Platform tileset (16-tile) | `lower_description`, `transition_description`, `transition_size`, `base_tile_id` |
| `get_sidescroller_tileset` | Tileset + örnek map | `tileset_id` |
| `create_isometric_tile` | Bireysel isometric tile | `description`, `size` (32px önerilen), `seed` |
| `get_isometric_tile` | Tile durumu | `tile_id` |
| `list_isometric_tiles` | Tile listesi | — |
| `create_map_object` | Transparent bg prop | `description`, `background_image` |
| `create_object` | Çok yönlü/framlı object | `description`, `directions` (4/8/16), `n_frames`, ref image (base64) |
| `vary_object` | Mevcut objectin varyasyonları | `object_id` |
| `create_tiles_pro` | Gelişmiş tile üretimi | `description`, `tile_type` (isometric/square/hex), `style_images` |

---

## Workflow Notları

### Async Pattern
```
1. Tool'u çağır → anında job_id al
2. 2-5 dk arka planda işleniyor
3. get_* tool ile durumu sorgula
4. Tamamlanınca download URL (UUID = erişim anahtarı, auth gerekmez)
```

### Karakter → Animasyon Zinciri
```
create_character → character_id
animate_character(character_id, "walk") → job_id   # beklemeden sıraya ekle
animate_character(character_id, "attack") → job_id  # aynı anda
animate_character(character_id, "idle") → job_id
# Hepsi paralel işlenir
```

### Tileset Zinciri (Görsel Süreklilik)
```
create_topdown_tileset(lower="dirt") → base_tile_id
create_topdown_tileset(lower="dirt", upper="grass", lower_base_tile_id=...) → tileset
# lower_base_tile_id ile palette tutarlılığı sağlanır
```

### Isometric Tile Konsistansı
```
# Aynı seed → aynı stil
create_isometric_tile(description="stone wall", seed=42)
create_isometric_tile(description="stone floor", seed=42)  # eşleşen stil
```

---

## Desteklenen IDE/Client
Claude Code CLI, VS Code, Cursor, Zed, Claude Desktop, Continue, Windsurf

## Desteklenen Game Engine
Unity, Godot, GameMaker, Unreal, Raylib, MonoGame, Defold, Love2D, Pygame
