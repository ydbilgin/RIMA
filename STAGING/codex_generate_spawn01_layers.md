# Codex Task: Generate Spawn_01 5-Layer Painted Stack via REST API

## Context

User onayı (S92, 2026-05-18): 5 layer multi-painter test üretimi. Strategy C (Codex verdict S92): floor base via `/generate-image-v2`, style-locked decals/props via `/generate-with-style-v2`. Bütçe kontrollü — bu generation'lar sonrası animation jobs için credit kalsın.

- API base: `https://api.pixellab.ai/v2`
- Token: `~/.codex/config.toml` → `[mcp_servers.pixellab.env].AUTH_HEADER` (`Bearer 037c...`)
- Current balance: 4524 / 5000 generations remaining
- Spawn_01 bounds artık 20×13 wu (Hades-scale, just updated by orchestrator)

## Görev: Reusable batch generation script + execute

### 1. Script yazma

Path: `STAGING/scripts/generate_painted_layers.py`

Reusable Python script:
- Token'ı `~/.codex/config.toml`'den oku (parse TOML), env'den de fallback
- Input: JSON config (layer list) — örnek: `STAGING/scripts/spawn01_layers.json`
- Per-layer:
  - POST `/generate-image-v2` veya `/generate-with-style-v2`
  - Poll `GET /background-jobs/{job_id}` her 5s, max 5 min
  - Sonuç data'sından image URL veya base64 al, PNG'ye yaz
  - Usage credit'i logla
- Output: PNG'ler `Assets/Art/Rooms/Backgrounds/Spawn_01/layer_NN_<name>.png`
- Total credit log + remaining balance check sonrası

### 2. Input config — Spawn_01 5 layer

Aşağıyı `STAGING/scripts/spawn01_layers.json` olarak yaz:

```json
{
  "output_dir": "Assets/Art/Rooms/Backgrounds/Spawn_01",
  "layers": [
    {
      "index": 0,
      "name": "floor_painted_granite",
      "endpoint": "/generate-image-v2",
      "width": 632,
      "height": 424,
      "no_background": false,
      "seed": 4201,
      "description": "A top-down 2D pixel art game scene viewed from a 30-35 degree angled perspective. Continuous painted floor surface — rounded organic oval stone shapes (NOT square grid tiles) with subtle foreshortening: top edges compressed, bottom edges wider, creating fake-3D depth. Soft directional shadows hint at elevation. Painterly hand-painted texture, no grid lines, seamless natural composition. Biome: Shattered Ruins / Shattered Keep — cracked granite flagstones in cool gray with cyan rift cracks, broken stone walls at edges, moss patches, scattered rubble. Muted Hades-tone shadows. Style: Hades / Sea of Stars painted background.",
      "negative_description": "flat top-down, square tile grid, visible grid lines, isometric diamond tiles, axonometric view, sharp square stones, repeating tile pattern, gridded floor, sprite sheet, character, creature, person"
    },
    {
      "index": 1,
      "name": "decal_rift_crack",
      "endpoint": "/generate-with-style-v2",
      "width": 256,
      "height": 256,
      "no_background": true,
      "seed": 4202,
      "style_image_from_layer": 0,
      "description": "Hairline cyan rift crack cluster on transparent background — branching micro-fractures, subtle glow, painterly stroke, matching the floor's brushwork and palette. Single decorative decal element.",
      "negative_description": "tile, grid, square edges, character, person, full scene, rectangular background"
    },
    {
      "index": 2,
      "name": "decal_rubble",
      "endpoint": "/generate-with-style-v2",
      "width": 256,
      "height": 256,
      "no_background": true,
      "seed": 4203,
      "style_image_from_layer": 0,
      "description": "Broken granite rubble chunks and chipped oval stone fragments on transparent background — scattered cluster suitable for wall-edge scatter, painterly hand-painted, same palette and shadow direction as the floor.",
      "negative_description": "tile, grid, square edges, character, person, full scene, rectangular background"
    },
    {
      "index": 3,
      "name": "prop_statue_silhouette",
      "endpoint": "/generate-with-style-v2",
      "width": 256,
      "height": 256,
      "no_background": true,
      "seed": 4204,
      "style_image_from_layer": 0,
      "description": "Cracked headless stone statue on small pedestal — dark stone mass silhouette, painterly painted, corner-readable as set dressing, NOT a gameplay blocker. Transparent background.",
      "negative_description": "tile, grid, square edges, full scene, rectangular background, multiple statues"
    },
    {
      "index": 4,
      "name": "accent_glow_mote",
      "endpoint": "/generate-with-style-v2",
      "width": 128,
      "height": 128,
      "no_background": true,
      "seed": 4205,
      "style_image_from_layer": 0,
      "description": "Small cyan rift glow accent with dust motes — soft selective magical highlight, painterly, matching floor brushwork. Single ambient element on transparent background.",
      "negative_description": "tile, grid, square edges, character, full scene, rectangular background"
    }
  ]
}
```

### 3. Execute script

`python STAGING/scripts/generate_painted_layers.py STAGING/scripts/spawn01_layers.json` çalıştır.

Beklenen:
- Layer 0 → ~30-60s (Pro 632×424)
- Layer 1-4 → ~30-60s each (style-locked, 4 paralel job olabilir ama 429 risk var — sequential safer)
- Total ~3-5 min
- 5 PNG dosyası `Assets/Art/Rooms/Backgrounds/Spawn_01/`'a düşer

### 4. Output report

`CODEX_DONE_spawn01_layers_generation.md` yaz:

```markdown
# Spawn_01 5-Layer Generation — REPORT

## Status: SUCCESS | PARTIAL | FAILED

## Per-layer log
| # | Layer | Endpoint | Size | Job ID | Status | Credits | PNG Path |
|---|---|---|---|---|---|---|---|
| 0 | floor_painted_granite | /generate-image-v2 | 632×424 | ... | completed | ... | Assets/Art/.../layer_00_floor.png |
| 1 | decal_rift_crack | /generate-with-style-v2 | 256×256 | ... | completed | ... | ... |
...

## Total credits used: X
## Remaining balance: Y / 5000
## Script path: STAGING/scripts/generate_painted_layers.py (reusable for future bg)
## Total wall time: Z minutes

## Sample preview (Layer 0 file size + dims confirmation only, don't paste base64)
floor: 1.2 MB, 632×424 PNG
...
```

## Critical constraints

- **Bütçe kontrol:** Her POST sonrası response.usage'tan credit oku, log'la. >10 credit/job ANORMAL — durdur ve flag at.
- **402 Insufficient credits** veya **429 Too many concurrent jobs** → script durur + log'lar
- **Timeout per job:** 5 dakika (300s poll loop limit), aşarsa fail + log
- **Sequential, not parallel** (429 riski azaltır)
- **Style ref handoff:** Layer 0 download edildikten SONRA Layer 1-4 başlar. style_image_from_layer field'i Layer 0 PNG'sini base64-encode edip GenerateWithStyleV2Request.style_images'a koymak demek.

## Hard limits

- KOD YAZMA proje kaynağına. Sadece STAGING/scripts/ ve Assets/Art/Rooms/Backgrounds/Spawn_01/.
- Token'ı log'a yazma (RedACTED veya hiç echo etme).
- Script reusable olsun — input JSON path argüman, output dir config'den.
- 10 dk max execution time
- Bittiğinde RAPOR + path'leri orkestrasyon için ver.
