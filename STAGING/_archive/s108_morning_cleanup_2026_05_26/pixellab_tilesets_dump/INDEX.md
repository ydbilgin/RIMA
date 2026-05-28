# PixelLab Tileset Dump — 2026-05-14

11 tileset indirildi. Her tileset 2 dosya:
- `<id>.png` — 4×4 grid, 16 Wang tile, 128×128 px (32×32 her tile)
- `<id>.json` — full metadata (corner tipi + bounding_box + 4×4 wildcard pattern + spritesheet URL)

## Dosya yapısı (JSON içeriği)

```
{
  id, name, lower_description, upper_description, transition_description,
  tile_size: {32,32},
  transition_size: 0.0-0.5,
  base_tile_ids: { lower, upper },              ← chaining için
  tileset_data: {
    tiles: [16 adet — corners(NE/NW/SE/SW) + bounding_box + 4×4 pattern],
    terrain_types: ["lower","upper"],
    spritesheet_url: "https://backblaze.pixellab.ai/...",  ← public CDN
    spritesheet_layout: "tileset15_4x4"
  },
  format: "tileset15",
  pattern_system: { 4×4 wildcard, lower=0/upper=1/wildcard=255 }
}
```

## 11 tileset (kısa isim → id)

| Tip | İsim | ID |
|---|---|---|
| Alabaster | pink ↔ cream sand (transition=0) | `ea19bab2-fea4-4c36-b5ef-6db1d103cc74` |
| Shattered Keep | rubble ↔ moss (transition=0) | `9591f35a-2373-4150-b737-7b4620d1834c` |
| Shattered Keep | path ↔ debris-rift | `ecfee0a0-a5ec-4992-b435-1f1d3ae2dfdb` |
| Shattered Keep | wall ↔ debris-rift | `02a5a97b-9475-4bdb-b2e4-cde475068f4d` |
| Shattered Keep | wall ↔ path | `8c154e37-8c0a-450a-82fd-126cc8b35c97` |
| Shattered Keep | rubble ↔ wall | `9ffbb4d1-79d0-441d-8c23-e1df62e25644` |
| Shattered Keep | rubble ↔ path | `49913501-fd93-41df-8a9f-f24e97a7b76c` |
| Shattered Keep | debris floor ↔ rift (eski) | `4c962284-90f4-43ee-8192-2b108a77b6ca` |
| Alabaster | pink ↔ hexagon trace | `a1b63282-7bc3-4acb-a390-e82853b8168d` |
| Cave | slate ↔ mineral | `d43914a8-bd20-4aa4-9ded-f95a773062f9` |
| Cave | cold floor ↔ cold wall | `bdca2623-62ac-4624-9ffb-d1728f86e3c3` |

## İki indirme endpoint'i

- **MCP-only:** `https://api.pixellab.ai/mcp/tilesets/{id}/image` — auth gerekir, direkt curl 404
- **Public CDN:** `tileset_data.spritesheet_url` (Backblaze) — JSON içinden okunup direkt curl edilir
- **Public metadata:** `https://api.pixellab.ai/mcp/tilesets/{id}/metadata` — auth gerekmez

JSON içindeki `spritesheet_url` public — bu dump tüm 11 PNG'yi oradan çekti.
