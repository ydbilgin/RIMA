# Multi-Layer BG Asset Strategy - Codex Verdict

## Q1: /generate-image-v2 size enum
OpenAPI has no enum/pattern list for `/generate-image-v2` `image_size`. It is a numeric object, not a preset list:

```json
{
  "properties": {
    "width": {
      "type": "integer",
      "maximum": 792.0,
      "minimum": 16.0,
      "title": "Width",
      "description": "Image width in pixels (16 to aspect-ratio max)"
    },
    "height": {
      "type": "integer",
      "maximum": 688.0,
      "minimum": 16.0,
      "title": "Height",
      "description": "Image height in pixels (16 to aspect-ratio max)"
    }
  },
  "additionalProperties": false,
  "type": "object",
  "required": ["width", "height"],
  "title": "ImageSize"
}
```

Endpoint description also says: minimum 16x16; maximum depends on aspect ratio, examples 512x512 square and 688x384 16:9. Output count is by max dimension: above 170px returns 1 image.

632x424 supported: YES by OpenAPI schema. It is not listed as an enum because no enum exists; it fits width <= 792 and height <= 688. Caveat: the exact aspect-ratio validator is not enumerated in the spec, so a live POST is the only stronger proof, but the OpenAPI evidence does not mark 632x424 as Web UI-only.

## Q2: Endpoint suitability (5 endpoints)
| Endpoint | Hades multi-layer fit | Use case |
|---|---|---|
| `POST /generate-image-v2` | HIGH | Best for full painted room bases and rectangular backgrounds. Supports text prompt, optional reference images, optional style image, automatic background removal toggle, and aspect-ratio-sized outputs such as 632x424 / 688x384. |
| `POST /generate-with-style-v2` | HIGH for coherence, LIMITED for wide bases | Best for making decals/overlays feel like the same hand, because it matches style reference images. Output schema caps width/height at 512, so it is better for 512x384, 512x512, 256x256, and 128x128 stack pieces than for 632px-wide floor bases. |
| `POST /create-tiles-pro` | MEDIUM/LOW | Useful for top-down square tile variations or biome material studies. Not ideal for Hades-style full-room painted composition because tile outlines and repeated variation bias will fight the painterly non-grid requirement. |
| `POST /create-tileset` | LOW for painted BG, MEDIUM for procedural terrain | Useful when the goal is a Wang/terrain tileset with transitions and compatible tile metadata. Risky for RIMA painted backgrounds because 16/32px tiles and seamless constraints will read tile-y unless heavily overpainted/manual-stacked. |
| `POST /create-isometric-tile` | NO | Wrong camera family and too small: isometric tile endpoint, 16x16 to 64x64. RIMA target is angled top-down painted floor, not isometric blocks. |

## Q3: Strategy verdict
Recommendation: C, with `/generate-image-v2` kept for the rectangular floor base and `/generate-with-style-v2` used for style-locked stack pieces.

Why:
- Hades / Alabaster Dawn style depends more on coherent brushwork, palette, shadow language, and accent color than on tileable procedural variety.
- A single flat scene (A) loses the compositional flexibility that the multi-layer painter plan is trying to buy; decals let Spawn_01 variants stay cheap without repainting the whole floor.
- Pure tileset strategy (D) pushes toward visible repetition/grid logic, which conflicts with the existing prompts: rounded irregular stone, no square grid, loose painted oil aesthetic.

Risks: `/generate-with-style-v2` cannot produce 632px-wide floor bases because its schema caps width/height at 512; use it for 256/128 overlays or 512-sized alternates, and use `/generate-image-v2` with a style image for the 632x424 base.

## Q4: Spawn_01 concrete production (3-5 layers)
Assumption for credit estimate: the project balance is generation-count based; estimate 1 generation call = 1 credit. The OpenAPI response exposes `usage.usd`, not a fixed credit table, so this is a planning estimate. Sizes above 170px return 1 image; 128px in `/generate-with-style-v2` returns 4 candidates in one request.

| Layer | Endpoint | Size | Prompt theme | Credits |
|---|---|---|---|---|
| 0 floor | `/generate-image-v2` | 632x424, ~3:2 | Painted angled top-down Spawn_01 cracked cool-gray granite floor, compressed far edge, no square grid, faint moss, muted Hades-tone shadows. | 1 |
| 1 rift crack decal | `/generate-with-style-v2` | 256x256, 1:1 transparent overlay | Hairline cyan rift crack cluster with subtle glow and branching micro-fractures, matching approved floor brushwork. | 1 |
| 2 rubble decal | `/generate-with-style-v2` | 256x256, 1:1 transparent overlay | Broken granite rubble chunks and chipped oval stone fragments for wall-edge scatter, same palette and shadow direction. | 1 |
| 3 statue silhouette decal | `/generate-with-style-v2` | 256x256, 1:1 transparent overlay | Cracked headless statue/pedestal silhouette, dark stone mass, corner-readable but not a gameplay blocker. | 1 |
| 4 glow accent | `/generate-with-style-v2` | 128x128, 1:1 transparent overlay | Small cyan rift glow accent and dust motes for selective magical highlight. | 1 request; 4 candidates |

Total credits: 5 planned generation calls (of 4524 remaining). If billing counts every 128px candidate separately, worst-case total becomes 8 image credits; use 256x256 then downscale if strict one-output accounting is required.

## Q5: Plan Section 5 revision
632x424 is supported by REST evidence, so do not replace the floor strategy with 512-only. Revise the size table note to remove the Web UI-only uncertainty and add the style-stack rule.

Exact diff suggestions for `STAGING/MULTILAYER_PAINTER_PLAN_v1.md`:

```diff
-| **632x424** | ~3:2 | 9.875x6.625 wu (best 4:3 match for 8x6) | **Default for Spawn_01 and 8x6 rooms** |
+| **632x424** | ~3:2 | 9.875x6.625 wu (best 4:3 match for 8x6) | **Default for Spawn_01 and 8x6 rooms; REST `/generate-image-v2` supported by numeric ImageSize schema** |

-- Layer 0 (floor base) = 632x424 painted floor - covers room
+- Layer 0 (floor base) = 632x424 `/generate-image-v2` painted floor, `no_background=false`, style image optional - covers room

-- Layer 1 (rift crack center) = 256x256 painted accent - placed mid-room
+- Layer 1 (rift crack center) = 256x256 `/generate-with-style-v2` painted transparent decal - placed mid-room

-- Layer 2 (statue silhouette) = 128x128 painted prop - corner offset
+- Layer 2+ (props/accents) = 256x256 or 128x128 `/generate-with-style-v2` style-locked transparent decals - corner/edge offsets

-- **MCP coverage note:** PixelLab MCP `create_object` caps at 256x256, `create_map_object` at 400px. Sizes >=424 are **Web UI Create Image Pro ONLY** (or REST API `api.pixellab.ai/v2/docs` per S92 Discord update).
+- **REST coverage note:** PixelLab REST `/generate-image-v2` uses numeric `image_size` (`width` 16-792, `height` 16-688; aspect-ratio max; no enum), so 632x424 is not Web UI-only. PixelLab MCP caps are separate: `create_object` 256x256 and `create_map_object` 400px.
```

## Final recommendation
Do C as a hybrid style-locked stack: generate one 632x424 painted floor base with `/generate-image-v2`, then generate 3-4 transparent 256/128 decals with `/generate-with-style-v2` using the approved base/style reference. This keeps the Hades-like painted read, preserves multi-layer room variation, and avoids the tile-grid risk of tileset endpoints.
