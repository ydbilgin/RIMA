# Wall Production Pipeline — S99 LATE state (2026-05-22 evening)

## Current pivot
**Modular atlas pipeline DROPPED. Continuous source art pipeline PRIMARY.**

PixelLab 16-piece modular kit (commit `32f204b7`) Lego problem ürettü — her piece kendi başına 3D blok, yan yana koyunca tile-mate etmiyor. ChatGPT plan + Opus initial + Codex review 3-perspective convergence ile karar:

- **Continuous source art primary** — bir wall direction'ı tek painting üret → Python slice → tile-mate Unity sprite'lar
- **Modular kit secondary** — salvage/filler için kalsın (corner_NE, M11 broken, M15 short_half kullanılabilir)
- **V2 IMAGEGEN fallback** — 5 sprite (commit `cecf4872`) hero piece olarak kalsın (archway_v2 + corner_SE_v2 + collapsed_stub_v2 daha detaylı)

## Style anchor (LOCKED)
**Image #17 (S99 LATE)** — N wall section 384×128, dark slate granite + tek dramatic cyan rift crack + moss top edge + Hades-iso angle (top + front face visible).

PASS verdict S99 LATE evening. Style chain anchor olarak future production'da style image upload edilecek.

Save path (önerilen): `STAGING/concepts/n_wall_section_v2.png` (user'ın kaydetmesi pending)

## Workflow tools — current pick order

| Use case | Tool | Notlar |
|---|---|---|
| Per-piece style chain | **PixelLab Create Image Pro** | Style image slot ayrı, dimension kilitlemez (20 gen/call) |
| Cheap iteration single | PixelLab Create Image S-XL (new) | Init image dimension lock var (bkz. `feedback_pixellab_init_image_dimension_lock`) |
| Atlas approach (multi-piece in one painting) | PixelLab Create Image Pro 688×384 or 632×424 | Tek painting → slice |
| Fallback | Codex gpt-image-1 IMAGEGEN | 5 v2 sprite already produced, black BG sorunu |

## Critical fixes pending
- **V2 IMAGEGEN prefab sortingLayer** — "Default" → "Walls" (Codex review flag, urgent pre-production)
- **Magenta BG insight** — ChatGPT review: black BG luma threshold near-black wall shadow yer → magenta veya true transparent kullan (V2 sprite'larda already-applied black BG eats shadows confirmed)

## Production scope locked
**Vertical slice room için minimum:**
- 10 chunks (N strip + S edge + W + E + 4 corner + 1 doorway/archway + 1 broken insert)
- 8 overlays (rubble × 2, cyan crack, torch, banner, dust strip, 2 small decals)
- NOT 32 + 36 = 68 ChatGPT'nin full library önerisi (defer post-vertical-slice)

## ChatGPT ref visual target
`STAGING/concepts/chatgpt ref/` — 8 Hades-iso dungeon room reference. Style target. Wall'ları + floor + light + props birlikte. Continuous painted scene (modular değil), Hades-tier ambiance.

## Discord help pending
PixelLab community'e question hazır:
- Title: "Modular tile-mating wall set across different aspect ratios — workflow?"
- Body: per-piece style chain blocked by init dimension lock; asking Pro tool alternative
- Attach: image #17 (N wall section)
- Approach A (per-direction style chain) vs B (atlas painting → slice) flexible

Cevap geldikçe pipeline refine edilecek.

## Hazır dispatch (yeni session için)
1. **Codex Pro atlas slice + import** — eğer PixelLab Pro 688×384 atlas üretilirse
2. **Codex per-direction slice + import** — eğer Discord ayrı gen approach önerirse
3. **Codex sortingLayer fix + Sonnet cleanup Batch A** — V2 prefab fix + dead Editor script cleanup (paralel yapılabilir)

## Related
- [[feedback_pixellab_init_image_dimension_lock]] — workflow blocker
- [[project_modular_pipeline_lock]] — earlier modular approach (REVISE — now continuous primary)
- [[project_act1_shattered_keep_lore_lock]] — visual style canonical
- [[feedback_chatgpt_pixellab_hybrid_workflow]] — workflow precedent
- [[PIXELLAB_TOOL_GUIDE]] — tool capability reference
