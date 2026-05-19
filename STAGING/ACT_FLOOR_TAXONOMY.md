# Act Floor Material Taxonomy (NLM canonical, 2026-05-18 S92)

Source: NLM notebook 30ddffa5 query — `biome_prompt_templates.md`, `biome_patchatlas_scatter_content_spec.md`, `MASTER_KARAR_BELGESI.md` (Karar #100, #143).

## Universal rules (ALL acts)

**Endpoints by layer (Karar #143):**
- L1/L2 Base Floor → `mcp__pixellab__create_tiles_pro`
- L4 Large Patch Overlay → `mcp__pixellab__create_map_object` (transparent)
- L5/L6 Detail/Scatter Accent → `mcp__pixellab__create_object` (transparent)
- L3 Wall Overlay → `mcp__pixellab__create_object` (separate Hades-style perimeter cap)

**`create_tiles_pro` settings (canonical, NLM LOCKED):**
- `tile_type: square_topdown`
- `tile_size: 32`
- `tile_view: "top-down"` (**PURE — NOT "low" / "high"**) — because low top-down leaks wall stripes into floor
- `tile_depth_ratio: 0` (no extrusion)
- `outline_mode: "segmentation"` (no outline artifacts)
- Numbered prompt: `"1). ... 2). ... 16 tile"`

Hades 35° feel comes from L3 wall overlay perspective + decoration angles + Unity camera setup, NOT tile angle.

**Universal forbidden words:**
- Geometric pattern: `flagstone, slab, mortar, brick, cobble, masonry, cut-stone`
- "tile" word (triggers pattern) — use `"floor surface"` instead
- `regular, grid, pattern, uniform, repeating`
- `baked light, shadow cast, directional`
- `3d render, smooth gradient, anti-aliasing`

**Past lessons (don't repeat):**
- ❌ Mix features in one tile (clean+moss+rift together) — AI overemphasizes one feature
- ❌ "flagstone/slab/mortar" — triggers brick grid
- ❌ `tile_view: "low top-down"` — side wall stripes leak (per NLM)
- ❌ Warm brown in F1 base (Karar #100 cold lock)
- ❌ Bright green moss (F1 grey-green, F2 violet-grey)
- ❌ `outline_mode: "outline"` — use segmentation
- ❌ Decal via `create_tiles_pro` — force seamless. Use `create_object` for transparent decals
- ❌ Expecting >16 tiles per batch — PixelLab fixed 16 for 32px

---

## Act 1: Shattered Keep / Sunken Keep (PRIORITY — first build)

**Theme:** Fragmented ancient order, cold stone, ritual catastrophe.

### Base Floors

| Material | Layer | Palette | Variants | Endpoint |
|---|---|---|---|---|
| Cool Granite / Rubble | L1/L2 dominant | `#3A3D42` to `#4E5260`, shadow `#252830` | 16 | `create_tiles_pro` |
| Worn Stone Path | L1/L2 secondary | `#4A4842` paler smooth | 16 | `create_tiles_pro` |

Forbidden for F1: warm brown, bright green, geometric grid.

### Patch Overlays (L4)

| Material | Palette | Variants | Endpoint |
|---|---|---|---|
| Dust drift | `#6A5C48` → `#7C6A55` | 3-4 | `create_map_object` |
| Ash smear | `#3A332A` → `#4A4035` | 3-4 | `create_map_object` |
| Mud crust | `#4A3C2A` → `#5A4A38` | 3-4 | `create_map_object` |
| Grit | `#8A7E68` → `#9A8E78` | 3-4 | `create_map_object` |
| Cool Cave Moss | `#5A6B5A`, `#2A4520` → `#5C6240` | 4 (dense tuft / thin spread / old curl / lichen mix) | `create_map_object` |
| Rift contamination | Cyan `#00FFCC` + Violet `#5A2A8A` | 4 (crackline / seepage / mixed halo / faint residue) | `create_map_object` |

Size 16-24px, transparent bg, "flat ground decal, no creature, no character features".
Rift forbidden: blood/horror — use "Ritual Catastrophe" framing.

### Scatter Accents (L5/L6)

| Material | Variants | Endpoint |
|---|---|---|
| Stones (small pebble / medium stone / chipped rock / twin pebble cluster) | 4 | `create_object` |
| Moss tufts (dense / dried / lichen) | 3 | `create_object` |

Prompt anchor: "top-down 35° view, isolated small prop, no shadow, transparent background".

### L3 Wall Overlay

Separate batch (Hades-style perimeter cap, NOT tileset). `create_object` endpoint per Karar #143.

---

## Act 2: Bleeding Wastes (deferred — produce after Act 1)

**Theme:** Living corrupted wound, dark bog, ossuary.

| Material | Layer | Palette | Variants | Endpoint |
|---|---|---|---|---|
| Corrupted Bog | L1/L2 dominant | `#3A2840`, shadow `#1F1428` | 16 | `create_tiles_pro` |
| Corrupted Moss | L4 | `#5A4870` | 8 | `create_map_object` |
| Weathered Bone | L5 | `#A89880` ivory | 8 (fragments / skulls / ribs) | `create_object` |
| Dried Blood | L4 | `#5E2A35` dark crimson | 8 | `create_map_object` |
| Dark Roots | L4/L5 | `#3A2820` | 8 | `create_map_object` |
| Rust Ember | L6 | `#C8502A` | 4 sparse | `create_object` |

Forbidden: generic "swamp green", normal green moss.

---

## Act 3: Core Approach (deferred)

**Theme:** Transcendental cosmic, thinning reality, voids.

| Material | Layer | Palette | Variants | Endpoint |
|---|---|---|---|---|
| Void Substrate | L1/L2 dominant | `#0A0810` void, rim `#3A4858` | 16 | `create_tiles_pro` |
| Incandescent Sigils | L4/L6 | `#FFD700` gold, `#8B6914` carved | 8 (half-erased / carved fragments) | `create_object` |
| Star Fragments / Cosmic Dust | L5 | `#E8DFC0` pale gold | 8 | `create_object` |
| Void Bleed | L6 | `#4F2A6B` violet | 8 | `create_map_object` |

Forbidden: warm earth tones, normal floor tiles.

---

## Alabaster Dawn (Phase 1.5 sub-biome, deferred)

| Material | Layer | Palette | Variants | Endpoint |
|---|---|---|---|---|
| Pale Stone | L1/L2 dominant | Ivory, cream, rose blush | 16 | `create_tiles_pro` |
| Cream Drift | L4 | `#E8DCC5` → `#F0E5D0` | 3-4 | `create_map_object` |
| Ivory Swirl | L4 | `#ECE2CA` → `#F2E8D2` | 3-4 | `create_map_object` |
| Rose Blush | L4 | `#E5C5BC` → `#EFCFCA` | 3-4 | `create_map_object` |
| Mauve Flecks | L4 | `#C8B8C5` → `#D5C5D0` | 3-4 | `create_map_object` |
| Delicate Rift | L6 | Cyan `#00FFCC` → `#66D8B5`, Violet `#5A2A8A` → `#6A3A9A` | 3 (thin line / small bloom / sparse) | `create_map_object` |
| Gold Inlay | L5 | `#B89A55` | 3-4 | `create_object` |
| Tarnished Bronze | L5 | `#7A6845` | 3-4 | `create_object` |
| Dried Petal | L5 | `#D8C8B0` | 3-4 | `create_object` |

---

## Cave (Phase 2 biome, deferred)

| Material | Layer | Palette | Variants | Endpoint |
|---|---|---|---|---|
| Slate Stone | L1/L2 dominant | Slate dark grey | 16 | `create_tiles_pro` |
| Cave Moss / Fungal | L4 | `#2A4838` → `#3A5848`, `#6A6855` → `#7A7868` | 4 | `create_map_object` |
| Mineral Veins | L4 | `#4A6A8A` → `#6080A0` cold blue | 4 | `create_map_object` |
| Wet Seep | L4 | `#2A3A4A` → `#3A4A5A` | 4 | `create_map_object` |
| Mineral Chunks / Mushrooms | L5/L6 | Cold blue crystal, glow-cap | 3-4 each | `create_object` |

---

## Nexus Core (Final, Phase 5+)

**Theme:** Mirror chamber, Architect fight.
- Pure white, pure black, dynamic class VFX colors
- Detailed taxonomy TBD

---

## Act 1 production order (immediate)

1. **Cool Granite Base** 16 variants — `create_tiles_pro` PURE top-down + segmentation (RE-DO, prior attempts had wrong angle)
2. **Worn Stone Path Base** 16 variants — `create_tiles_pro` PURE top-down + segmentation (RE-DO)
3. **Dust + Ash + Moss patches** — 4 variants each → `create_map_object` (already have 4 patches from earlier dispatch, partial)
4. **Rift contamination patches** — 4 variants → `create_map_object`
5. **L3 Wall Overlay** — Hades-style perimeter caps → `create_object` separate batch
6. **Stones + moss tufts scatter** — 4+3 variants → `create_object`

Estimated cost: ~$3-4 USD (16+16 tile batches + ~20 decal batches).
Subscription remaining: ~4280/5000 generation budget headroom.

---

## Source of truth

This doc supersedes ad-hoc tile_view choices in earlier dispatches. Always cross-reference before generation.
