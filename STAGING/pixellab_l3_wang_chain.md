# PixelLab L3 Wang Tileset Chain -- Vertical Slice Dispatch Spec

**Date:** 2026-05-16 S86 PREP-3
**Authority:** STAGING/sprite_strategy_FINAL_LOCK.md Section 2
**Scope:** 1 tileset only -- ShatteredKeep biome, Floor + Wall basic. 4 additional biomes DEFER.
**Status:** DISPATCHED (S86 PREP-3) — tileset_id `7b34aa6b-2031-455d-94e5-4322579c984e` üretildi, 25 tile (tileset15 format, 4×4 grid 128×128 PNG)

**S86 PREP-3 PATCH (Codex review blocker #4 + #5 fix):**
- `transition_description` artık REQUIRED parametre (`transition_size > 0` için zorunlu) — Section 2 tablosuna eklendi.
- PixelLab actual output: **15 unique Wang case** (format `tileset15`). Eski "16-23 tile" + "16 base + transition" wording iptal.
- L3 AssetPool import: **15 wall/transition variant**, 0000 (all-floor) `all_floor_reference` tag → AssetPool dışı (L1 zaten halleder).
- Canonical bit order: **NE-NW-SE-SW** (corner-major), variant tag: `wang_{ne}{nw}{se}{sw}`.

---

## 1. TOOL

`mcp__pixellab__create_topdown_tileset`

Mode: Wang corner numerology, lower (floor) to upper (wall) transition chain.
Generation time: ~100s per dispatch.

---

## 2. PARAMETERS (full, paste-ready)

| Parameter | Value | Notes |
|---|---|---|
| `tile_size` | `{"width": 32, "height": 32}` | RIMA L3 native size. Only 16 or 32 supported. |
| `lower_description` | see Section 3 | Floor tile prompt |
| `upper_description` | see Section 3 | Wall tile prompt |
| **`transition_description`** | **see Section 3 (REQUIRED when transition_size > 0)** | Wall-to-floor blend (e.g. "stone rubble base, rough wall-to-floor edge transition") |
| `transition_size` | `1.0` | Full wall transition. NOT 0.5 (partial). LOCK. |
| `view` | `"high top-down"` | RIMA Hades-match 30-35° tilt LOCK |
| `detail` | `"highly detailed"` | Fractured Epic tone preference |
| `shading` | `"detailed shading"` | Hades-style readability |
| `outline` | `"selective outline"` | Pixel art clarity |
| `text_guidance_scale` | `8` | Default; range 1-20 |
| `tile_strength` | `1.0` | Default; range 0.1-2 |
| `tileset_adherence` | `100` | Default; range 0-500 (strictness) |
| `tileset_adherence_freedom` | `500` | Default; range 0-900 |
| `lower_base_tile_id` | omit (first dispatch) | If chaining biomes later: fill with ShatteredKeep floor tile ID |
| `upper_base_tile_id` | omit (first dispatch) | If chaining biomes later: fill with ShatteredKeep wall tile ID |

**Expected output (S86 PREP-3 verified):** **15 unique Wang case** (format `tileset15`, 4×4 PNG grid 128×128).
Eski "16 base + transition" wording iptal (Codex review M5 finding) — gerçek PixelLab output esas alınır.

**Note on seed:** `create_topdown_tileset` does not expose a seed parameter per the MCP schema.
Record the returned tileset ID immediately -- this is the only way to re-fetch the exact output.

---

## 3. SHATTEREDKEEP BIOME PROMPT FORMULA

**Lower (floor) description:**
```
Fractured stone keep floor tiles, weathered slate, moss-touched cracks, top-down view, pixel art, muted gray-blue, hint of green moss, no UI, tileable
```

**Upper (wall) description:**
```
Fractured stone keep wall tiles with broken parapets, top-down 30-35 degree tilt, pixel art, darker gray, deeper cracks, occasional rune fragment, no UI, tileable
```

**Transition description (REQUIRED, transition_size=1.0):**
```
Stone rubble base, rough wall-to-floor edge transition, scattered debris and small chunks at wall foot
```

**Style notes:**
- Tone: Hades-style readability + Fractured Epic (deliberate decay, no gore)
- Palette target: muted stone gray, mossy edges, fractured slate, dim blue ambient
- Readability priority: floor must read as flat, wall must cast clear silhouette at 30-35 degree angle
- No "dark fantasy" phrasing (RIMA forbidden phrase -- use descriptors above instead)

**OPEN QUESTION for user:** Is there a ShatteredKeep palette reference PNG in the project
(e.g. under MEMORY/ or Assets/)? If yes, pass it as a style reference if the tool supports it.
Current spec uses generic Fractured Epic tone descriptors as fallback.

---

## 4. WANG CORNER NUMEROLOGY REFERENCE (4-bit NE-NW-SE-SW)

```
0000 = all floor       (skip -- L1 handles)
0001 = SW corner only
0010 = SE corner only
0011 = south edge (S wall)
0100 = NW corner only
0101 = west edge (W wall)
0110 = diagonal NW+SE  (rare)
0111 = U-shape open N
1000 = NE corner only
1001 = diagonal NE+SW  (rare)
1010 = east edge (E wall)
1011 = U-shape open W
1100 = north edge (N wall)
1101 = U-shape open E
1110 = U-shape open S
1111 = all wall (enclosed cell, rare)
```

Variant tag pattern used by BrushAtlasImporter: `wang_{bitstring}` e.g. `wang_0011`, `wang_1101`
All 16 Wang tiles map to Micro bucket (32x32). heroAllowed: FALSE.

---

## 5. PRE-DISPATCH CHECKLIST

- [ ] PixelLab MCP connected (after Claude restart, mcp__pixellab__* shows in deferred list -- load schema before calling)
- [ ] Credit budget confirmed (PixelLab Pro subscription active)
- [ ] Output directory exists: `STAGING/TILESET_OUTPUT/L3_Wang_Floor_Wall_v1/`
- [ ] Vertical slice scope confirmed: 1 tileset only, 4 additional biomes DEFER

To create output directory if missing:
```
New-Item -ItemType Directory -Force "F:\Antigravity Projeler\2d roguelite\RIMA\STAGING\TILESET_OUTPUT\L3_Wang_Floor_Wall_v1"
```

---

## 6. DISPATCH SEQUENCE

**Step 1 -- Dispatch**

Load tool schema:
```
ToolSearch query: "select:mcp__pixellab__create_topdown_tileset"
```

Then call with parameters from Section 2 + prompts from Section 3.

**Step 2 -- Poll**

Call `mcp__pixellab__get_topdown_tileset` with returned job/tileset ID.
Repeat until status = completed (~100s expected).

**Step 3 -- Download output**

Save the returned tile sheet PNG to:
`STAGING/TILESET_OUTPUT/L3_Wang_Floor_Wall_v1/master.png`

Save the tileset ID and any metadata to:
`STAGING/TILESET_OUTPUT/L3_Wang_Floor_Wall_v1/metadata.json`

**Step 4 -- QC** (see Section 7)

**Step 5 -- On PASS: Unity import**

Use Sprint 9 BrushAtlasImporter:
- Drop `master.png` into `Assets/Art/BrushAtlas/Intake/`
- Unity menu: RIMA > Brush > Import Atlas
- Template: `L3_Wang16_Topdown.asset`
- Target layer: L3
- Pool name: `AssetPoolSO_L3_ShatteredKeep`

---

## 7. QC CHECKLIST (Wang-specific)

**Structure checks (PATCHED — actual PixelLab output basis):**
- [ ] 15 Wang case present (tileset15 format, bitstring coverage 0001-1111 — 0000 all-floor case → `all_floor_reference` tag, AssetPool dışı)
- [ ] Each tile is exactly 32x32 native size
- [ ] PNG dimensions 128×128 (4×4 grid)
- [ ] Border transparent -- gutter inset >= 1px on all tile edges (no bleed into adjacent tile)

**Visual checks:**
- [ ] Wall silhouette readable at ~30-35 degree top-down angle
- [ ] Floor reads as flat surface -- no wall-like height cues
- [ ] Floor-to-wall transition smooth (transition_size=1.0 effect: no hard seam)
- [ ] Palette matches Fractured Epic ShatteredKeep tone (gray-blue, mossy, fractured slate)
- [ ] No obvious AI artifact patterns (symmetric banding, repeated noise patches)

**Adjacency test:**
- [ ] Paint 2-3 adjacent tiles in Aseprite or a test scene -- seams invisible at tile grid join
- [ ] Corner cases (diagonal 0110, 1001) tile correctly with edge cases (0011, 1010)

---

## 8. FAIL HANDLING

| Failure | Action |
|---|---|
| Style drift (wrong tone/palette) | Regen with stronger descriptors. Try more explicit palette words: "weathered blue-gray slate", "iron-stained mortar" |
| Wall silhouette unreadable | Add to upper_description: "strong top edge highlight, dark underside shadow, clear silhouette boundary" |
| Border bleed (tiles not transparent at edge) | Open master.png in Aseprite, manually add 1px transparent gutter around each Wang cell |
| Wang case missing (fewer than 16 base tiles) | Last resort: 16 separate 32x32 dispatches via `mcp__pixellab__create_object` (Strategy A explicit fallback per FINAL_LOCK Section 2) |
| Generation error / tool failure | Reload MCP schema via ToolSearch, retry once. If persists: use `mcp__pixellab__list_topdown_tilesets` to check if job queued |
| Total quality fail | Escalate to user -- do not auto-regen more than twice without user review |

---

## 9. COST AND TIME ESTIMATE

| Phase | Time | Credit (estimate) |
|---|---|---|
| 1 dispatch | ~100s (~2 min) | ~1 credit |
| 1 QC regen (worst case) | ~100s | ~1 credit |
| Manual QC review | ~2-3 min | 0 |
| Unity import | ~2 min | 0 |
| **Total worst case** | ~8-10 min | ~2 credit |

Note: PixelLab credit cost per `create_topdown_tileset` call is unconfirmed.
User reports actual cost after first dispatch (per FINAL_LOCK Section 15 open item).

---

## 10. OUTPUT FILES (post-PASS)

```
STAGING/TILESET_OUTPUT/L3_Wang_Floor_Wall_v1/
  master.png                   -- full Wang tile sheet (PixelLab output, 16-23 tiles)
  wang_grid_preview.png        -- QC reference: tiles labeled with bitstring (create manually or via Aseprite script)
  metadata.json                -- tileset ID, dispatch params, Wang case mapping
```

`metadata.json` minimum content (S86 dispatched actual):
```json
{
  "tileset_id": "7b34aa6b-2031-455d-94e5-4322579c984e",
  "dispatch_date": "2026-05-16",
  "tile_size": 32,
  "transition_size": 1.0,
  "biome": "ShatteredKeep",
  "wang_case_count": 15,
  "format": "tileset15",
  "png_dimensions": [128, 128],
  "grid": "4x4",
  "lower_base_tile_id": "a6ee4481-6fba-4d5e-aee4-74058438a2b5",
  "upper_base_tile_id": "ed7a07e1-f665-42e7-9cc6-a942a12186be",
  "status": "PASS | FAIL",
  "notes": ""
}
```

---

## 11. VERTICAL SLICE CONNECTION (post-PASS pipeline)

```
master.png
  -> Sprint 9 BrushAtlasImporter (Wang-aware slice, L3_Wang16_Topdown.asset template)
  -> L3_Wang16_Topdown SliceLayoutTemplateSO (cells: 16 Wang cases, Micro bucket, heroAllowed=false)
  -> AssetPoolSO_L3_ShatteredKeep (variants list, variantId = wang_{bitstring})
  -> Sprint 10 RoomTemplate test room
     -- paint L3 Wang tiles onto floor/wall boundary
     -- save to RoomBank
     -- PlayMode test: RoomBank.Pick -> spawn -> exit valid
  -> Sprint 13 M9 GO / NO-GO gate
```

Wang tile placement context at runtime:
BrushAtlasImporter tags each variant `wang_{bitstring}`. At paint time, CompositeStrokeExecutor reads
neighboring tile occupation to resolve bitstring, picks matching variant. No manual case selection.

---

## 12. DEFER (after vertical slice M9 GO)

- 4 additional biome Wang tilesets (biome names TBD -- candidates: Forest, Crypt, Desert, Volcano)
  Use `lower_base_tile_id` / `upper_base_tile_id` chaining to maintain style coherence across biomes
- transition_size=0.5 partial wall variants (softer floor-wall boundary option)
- Decoration tile overlays (rubble, moss patches as L4-L5 decals -- separate `create_object` dispatches)
- SpriteAtlas per-biome packing (V2 scope)

---

## 13. AUTHORITATIVE REFERENCES

- STAGING/sprite_strategy_FINAL_LOCK.md Section 2 (Wang Full 16 topology lock)
- STAGING/sprite_strategy_FINAL_LOCK.md Section 6 (Production cost, vertical slice scope)
- [[karar-143-layered-pipeline]] -- 6-layer context, L3 wall role
- [[pixellab-tool-inventory]] -- create_topdown_tileset: tile_size 16|32, transition_size, ~100s gen
- [[brush-tool-v1-design]] -- Sprint 9 BrushAtlasImporter, SliceLayoutTemplateSO, Wang-aware slicing
- [[pixellab-create-modes]] -- Pro/Standard comparison (character only, not relevant to tileset)
