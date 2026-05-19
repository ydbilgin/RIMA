# Codex Task: Generate Layers 1-4 (Style-Locked, Layer 0 Reference)

## Context

Spawn_01 Layer 0 (floor painted granite, 632×424) ZATEN ÜRETİLDİ + Unity'e import edildi:
- PNG: `Assets/Art/Rooms/Backgrounds/Spawn_01/layer_00_floor_painted_granite.png` (384 KB, 632×424)
- Job ID: `695eb800-7c1e-465b-989f-80304dbedfd4` (completed, $0.185)
- Seed: 4201

Şimdi Layer 1-4 generation gerekiyor — Layer 0 PNG'sini **style reference** olarak `/generate-with-style-v2` endpoint'ine vererek style-locked decal/prop'lar üret.

## Previous failure

Önceki dispatch (`STAGING/codex_generate_spawn01_layers.md`) Codex script'i yazdı ama **300s timeout** PixelLab Pro generation süresine yetmedi (Layer 0 ~10 dk sürdü, sadece poll yetersizdi — job aslında ilerledi ve sonradan tamamlandı). 

**Bu dispatch:** TIMEOUT 1500s (25 dk) per job + 4 layer only (Layer 0 skip).

## Görev

### Script revize

`STAGING/scripts/generate_painted_layers.py` mevcut. Şu değişiklik:
- `POLL_TIMEOUT_SECONDS = 1500` (300 → 1500)
- `POLL_INTERVAL_SECONDS = 20` (5 → 20, daha az API spam)
- Eğer halihazırda doğru ise teyit + commit

### Input config

`STAGING/scripts/spawn01_layers_1_to_4.json` yaz (Layer 0 skip, sadece 1-4):

```json
{
  "output_dir": "Assets/Art/Rooms/Backgrounds/Spawn_01",
  "style_reference_path": "Assets/Art/Rooms/Backgrounds/Spawn_01/layer_00_floor_painted_granite.png",
  "layers": [
    {
      "index": 1,
      "name": "decal_rift_crack",
      "endpoint": "/generate-with-style-v2",
      "width": 256,
      "height": 256,
      "no_background": true,
      "seed": 4202,
      "use_style_reference": true,
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
      "use_style_reference": true,
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
      "use_style_reference": true,
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
      "use_style_reference": true,
      "description": "Small cyan rift glow accent with dust motes — soft selective magical highlight, painterly, matching floor brushwork. Single ambient element on transparent background.",
      "negative_description": "tile, grid, square edges, character, full scene, rectangular background"
    }
  ]
}
```

### Response struktür LOCK (Layer 0'dan öğrenildi)

```python
GET /background-jobs/{id} → returns:
{
  "status": "completed",
  "last_response": {
    "type": "done",
    "images": [{"type": "base64", "base64": "...", "width": N}],
    "usage_cost_usd": 0.X,
    "seed": NNNN
  }
}
```

Script:
- POST /generate-with-style-v2 with `style_images: [{"image": "<base64>", "size": {"width":632,"height":424}}]` + description + image_size + seed + negative_description + no_background
- Style image base64 = `Assets/Art/Rooms/Backgrounds/Spawn_01/layer_00_floor_painted_granite.png` read + base64 encode
- Poll /background-jobs/{id} every 20s, max 1500s
- On completed: read `last_response.images[0].base64` → write to `Assets/Art/Rooms/Backgrounds/Spawn_01/layer_NN_<name>.png`
- Log usage_cost_usd per layer

### Execute

`python STAGING/scripts/generate_painted_layers.py STAGING/scripts/spawn01_layers_1_to_4.json` 

Sequential, 4 layers. Each ~5-10 dk. Total ~20-40 dk.

### Output

`CODEX_DONE_spawn01_layers_1_to_4.md`:
```markdown
# Spawn_01 Layers 1-4 — REPORT

## Status: SUCCESS | PARTIAL | FAILED

## Per-layer
| # | Name | Job ID | Status | Cost USD | PNG path | Size |
|---|---|---|---|---|---|---|
| 1 | decal_rift_crack | ... | completed | 0.X | Assets/.../layer_01_decal_rift_crack.png | 256x256 |
...

## Total cost USD: X
## Remaining balance: subscription Y/5000 + credits $Z
## Wall time: N min
```

## Hard limits

- KOD YAZMA proje kaynağına. Sadece STAGING/scripts/ ve Assets/Art/Rooms/Backgrounds/Spawn_01/.
- Token RedACTED logging
- Sequential generation (429 risk azalt)
- 40 dk max execution
- Eğer 402/429 hatası → durdur, log, return PARTIAL
- Style reference image (Layer 0) zaten Unity Asset path'inde — okunacak + base64 encode + style_images field
