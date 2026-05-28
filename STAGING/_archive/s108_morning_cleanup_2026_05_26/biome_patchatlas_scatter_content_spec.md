# Biome PatchAtlas + ScatterBrush Content Spec — 3 biome production library
**Locked:** 2026-05-15 (S77 night)
**Karar refs:** #98 rift palette (cyan #00FFCC + violet #5A2A8A) / #121 Scatter Brush / #128 #129 organic patch overlay / #134 procedural pivot / #135 hybrid map workflow
**Decided by:** rima-design Opus content judgment (a56d4fe6fdc0a5175) — orchestrator approved
**Consumes:** STAGING/room_designer_master_spec_v3.md §2.7 + §2.8 + §3 stage D/G
**Codex Phase 1 baseline:** Assets/Data/PatchAtlas_Moss_ShatteredKeep.asset / PatchAtlas_Rift_Fracture.asset / Scatter_Stones_ShatteredKeep.asset / Scatter_Moss_Tufts.asset (stubs to extend)

---

## Design Philosophy

- **PixelLab create_map_object S76 dersi:** drifts to creature-like shapes when size > 24px or when prompt allows directional features. All map_object specs clamped 16–24px and use "flat ground decal, no creature, no character features, no silhouette" anchor.
- **Karar #128/#129 grid-bağımsız:** entries are world-coord world-space sprites placed by density+Perlin, not tile-locked. Sizes in px (sprite native), density as scalar 0–1.
- **Karar #98 rift palette ZORUNLU** for any rift content: cyan #00FFCC + violet #5A2A8A verbatim.
- **Density tuning:** dominant terrain reads first; organic overlay never exceeds ~35% visual coverage of base.
- **4 entries per atlas** (not 6–8): credit budget + S76 reject rate. 4 production-clean entries × rotation jitter ±20° × tint range ≈ effective variance of 8–10 entries at half credit cost.
- **Tight tint ranges:** ±~15 luminance, no hue shift. Wide ranges → muddy results.

---

## OUTPUT 1 — PatchAtlas Content Matrix

### Shattered Keep (3 atlases)

#### `PatchAtlas_Moss_ShatteredKeep` (extends Codex stub)

| Entry | Sprite Description | Size (px) | Density | Rot Jitter | Tint Range | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Dense moss tuft | irregular dense moss blob, top-down 35° | 20×20 | 0.40 | ±25° | #2A4520 → #3F5530 | `create_map_object: tiny dense moss tuft patch, top-down 35° view, 20×20 pixels, organic ragged irregular edges, muted dusty forest green, flat ground decal, no creature shape, no character features, no silhouette, washed-out desaturated` |
| Thin moss spread | sparse moss spread, top-down 35° | 24×16 | 0.35 | ±35° | #3A5028 → #4A5E38 | `create_map_object: thin sparse moss spread along ground, top-down 35° view, 24×16 pixels, oval irregular outline, muted olive forest green, flat ground decal, no creature shape, no character features, washed-out dusty` |
| Old moss curl | weathered moss curling at edge | 18×18 | 0.25 | ±40° | #364520 → #4A5530 | `create_map_object: weathered old moss curl ground patch, top-down 35° view, 18×18 pixels, irregular curling edge, muted dusty olive desaturated green, flat ground decal, no creature shape, no character features` |
| Moss + lichen mix | mossy patch with pale lichen flecks | 20×20 | 0.30 | ±30° | #2F4828 → #5C6240 | `create_map_object: small moss and pale lichen mixed ground patch, top-down 35° view, 20×20 pixels, irregular organic edge, muted dusty forest green with pale beige flecks, flat ground decal, no creature shape, no character features, washed-out` |

#### `PatchAtlas_Rift_Fracture_ShatteredKeep` (Karar #98 palette)

| Entry | Sprite Description | Size (px) | Density | Rot Jitter | Tint Range | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Cyan rift crack | thin cyan crackline | 24×12 | 0.18 | ±15° linear | #00FFCC → #1AD4B0 | `create_map_object: thin cracked cyan rift fracture line on stone, top-down 35° view, 24×12 pixels, jagged irregular crack edge, bright cyan glow #00FFCC with darker teal interior #1AD4B0, flat ground decal, no creature shape, no character features` |
| Violet seepage | dark violet seepage stain | 20×20 | 0.15 | ±360° | #5A2A8A → #4A1F70 | `create_map_object: dark violet rift seepage stain on stone ground, top-down 35° view, 20×20 pixels, irregular blooming radial stain, deep violet #5A2A8A center fading to darker #4A1F70 edge, flat ground decal, no creature shape, no character features` |
| Mixed rift fracture | cyan core + violet halo | 22×16 | 0.12 | ±20° | core #00FFCC, halo #5A2A8A | `create_map_object: rift fracture with bright cyan crack core surrounded by violet bleed halo, top-down 35° view, 22×16 pixels, jagged organic outline, cyan #00FFCC interior with violet #5A2A8A diffuse halo, flat ground decal, no creature shape, no character features` |
| Faint rift residue | residual rift stain, faded | 18×18 | 0.20 | ±360° | #3A2A55 → #2A1F40 | `create_map_object: faded residual rift stain on stone, top-down 35° view, 18×18 pixels, irregular blotchy outline, muted dusty violet desaturated, washed-out, flat ground decal, no creature shape, no character features` |

#### `PatchAtlas_Dust_ShatteredKeep` (new — base terrain organic breakup)

| Entry | Sprite Description | Size (px) | Density | Rot Jitter | Tint Range | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Dust drift | warm dust drift on stone | 24×20 | 0.30 | ±360° | #6A5C48 → #7C6A55 | `create_map_object: warm dust drift on stone ground, top-down 35° view, 24×20 pixels, soft irregular oval outline, muted dusty warm tan, flat ground decal, no creature shape, no character features, washed-out` |
| Ash smear | dark ash smear | 20×16 | 0.20 | ±360° | #3A332A → #4A4035 | `create_map_object: dark ash smear on stone, top-down 35° view, 20×16 pixels, soft elongated irregular smear, muted dusty dark brown-grey, flat ground decal, no creature shape, no character features` |
| Pale grit | pale grit accumulation | 18×18 | 0.25 | ±360° | #8A7E68 → #9A8E78 | `create_map_object: pale fine grit accumulation on stone, top-down 35° view, 18×18 pixels, irregular soft oval outline, muted pale dusty beige, flat ground decal, no creature shape, no character features, washed-out` |
| Mud puddle dry | dried mud crust | 22×22 | 0.15 | ±360° | #4A3C2A → #5A4A38 | `create_map_object: dried mud crust patch on stone, top-down 35° view, 22×22 pixels, irregular cracked rim outline, muted dusty warm brown, flat ground decal, no creature shape, no character features` |

### Alabaster Dawn (3 atlases — Faz 1.5)

#### `PatchAtlas_CreamDrift_AlabasterDawn` (reuses S76 cream drift `075242f4`)

| Entry | Sprite Description | Size (px) | Density | Rot Jitter | Tint Range | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Cream drift (existing) | S76 production-ready cream drift | 24×20 | 0.18 | ±360° | #E8DCC5 → #F0E5D0 | REUSE existing asset `075242f4` — do not regenerate |
| Pale ivory swirl | swirled ivory dust | 22×20 | 0.15 | ±360° | #ECE2CA → #F2E8D2 | `create_map_object: pale ivory dust swirl on pink stone, top-down 35° view, 22×20 pixels, soft irregular swirled oval outline, muted dusty pale ivory cream, flat ground decal, no creature shape, no character features, washed-out` |
| Rose blush spot | soft rose-pink dust spot | 18×18 | 0.12 | ±360° | #E5C5BC → #EFCFCA | `create_map_object: soft rose pink dust spot on pale stone, top-down 35° view, 18×18 pixels, irregular soft oval outline, muted dusty pale rose pink, flat ground decal, no creature shape, no character features, washed-out` |
| Mineral fleck cluster | pale mauve mineral fleck cluster | 20×16 | 0.10 | ±360° | #C8B8C5 → #D5C5D0 | `create_map_object: scattered mineral flecks cluster on cream stone, top-down 35° view, 20×16 pixels, irregular small fleck cluster outline, muted dusty pale mauve, flat ground decal, no creature shape, no character features` |

#### `PatchAtlas_CeremonialAccent_AlabasterDawn`

| Entry | Sprite Description | Size (px) | Density | Rot Jitter | Tint Range | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Gold inlay fragment | broken gold inlay fragment | 20×8 | 0.08 | ±15° linear | #B89A55 → #C8AA65 | `create_map_object: broken thin gold inlay fragment on stone, top-down 35° view, 20×8 pixels, thin elongated angular outline, muted dusty antique gold, flat ground decal, no creature shape, no character features` |
| Tarnished bronze flake | small tarnished bronze flake | 16×16 | 0.10 | ±360° | #7A6845 → #8A7855 | `create_map_object: small tarnished bronze flake on cream stone, top-down 35° view, 16×16 pixels, irregular angular flake outline, muted dusty tarnished bronze, flat ground decal, no creature shape, no character features` |
| Sun-bleached petal | dried petal remnant | 18×16 | 0.12 | ±360° | #D8C8B0 → #E2D2BA | `create_map_object: dried sun-bleached flower petal remnant on stone, top-down 35° view, 18×16 pixels, irregular wilted petal outline, muted dusty pale tan cream, flat ground decal, no creature shape, no character features` |
| Wax drip residue | dried wax drip puddle | 16×16 | 0.08 | ±360° | #E0D5BA → #E8DDC2 | `create_map_object: dried wax drip puddle residue on cream stone, top-down 35° view, 16×16 pixels, irregular pooled outline with raised lip, muted dusty pale cream wax, flat ground decal, no creature shape, no character features` |

#### `PatchAtlas_RiftTendril_AlabasterDawn` (Karar #98 — delicate first-stage contamination)

| Entry | Sprite Description | Size (px) | Density | Rot Jitter | Tint Range | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Thin rift tendril | very thin cyan tendril line | 24×8 | 0.06 | ±20° | #00FFCC → #66D8B5 | `create_map_object: very thin delicate cyan rift tendril line on cream stone, top-down 35° view, 24×8 pixels, fine wisp-like irregular line, bright cyan #00FFCC fading to pale teal #66D8B5, flat ground decal, no creature shape, no character features` |
| Violet bloom (small) | small violet bloom stain | 16×16 | 0.05 | ±360° | #5A2A8A → #6A3A9A | `create_map_object: small violet rift bloom stain on cream stone, top-down 35° view, 16×16 pixels, irregular soft radial bloom, muted dusty violet #5A2A8A core fading lighter at edge, flat ground decal, no creature shape, no character features` |
| Rift dust scatter | sparse violet+cyan dust scatter | 20×20 | 0.06 | ±360° | mixed #5A2A8A + #00FFCC dots | `create_map_object: sparse scattered rift dust specks on cream stone, top-down 35° view, 20×20 pixels, irregular scattered fine speck pattern, muted dusty violet and faint cyan specks, flat ground decal, no creature shape, no character features` |

### Cave (2 atlases — Faz 2)

#### `PatchAtlas_MineralVein_Cave`

| Entry | Sprite Description | Size (px) | Density | Rot Jitter | Tint Range | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Cold blue mineral vein | thin cold blue mineral vein | 24×10 | 0.20 | ±25° linear | #4A6A8A → #6080A0 | `create_map_object: thin cold blue mineral vein on slate stone, top-down 35° view, 24×10 pixels, jagged elongated vein outline, muted dusty cold blue with faint glow, flat ground decal, no creature shape, no character features` |
| Mineral cluster bloom | small mineral cluster | 18×18 | 0.15 | ±360° | #5A7A9A → #7090B0 | `create_map_object: small cold blue mineral cluster bloom on slate, top-down 35° view, 18×18 pixels, irregular crystalline cluster outline, muted dusty cold blue with subtle glow, flat ground decal, no creature shape, no character features` |
| Pale mineral dust | pale mineral dust spread | 22×20 | 0.20 | ±360° | #8A9AB0 → #98A8BC | `create_map_object: pale mineral dust spread on slate stone, top-down 35° view, 22×20 pixels, soft irregular spread outline, muted dusty pale blue-grey, flat ground decal, no creature shape, no character features, washed-out` |
| Wet seep stain | wet dark mineral seep | 20×16 | 0.12 | ±360° | #2A3A4A → #3A4A5A | `create_map_object: wet dark mineral seep stain on slate, top-down 35° view, 20×16 pixels, irregular wet pooled outline, muted dusty dark blue-grey wet sheen, flat ground decal, no creature shape, no character features` |

#### `PatchAtlas_Moss_Cave`

| Entry | Sprite Description | Size (px) | Density | Rot Jitter | Tint Range | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Cave moss patch | dim cool-green cave moss | 20×20 | 0.35 | ±25° | #2A4838 → #3A5848 | `create_map_object: dim cool-tinted cave moss patch on slate, top-down 35° view, 20×20 pixels, irregular organic outline, muted dusty cool forest green, flat ground decal, no creature shape, no character features, washed-out` |
| Pale fungal spread | pale fungal spread | 22×18 | 0.25 | ±360° | #6A6855 → #7A7868 | `create_map_object: pale fungal spread on slate stone, top-down 35° view, 22×18 pixels, irregular spread outline, muted dusty pale olive grey, flat ground decal, no creature shape, no character features` |
| Wet moss clump | wet glistening moss clump | 18×18 | 0.20 | ±30° | #2A4040 → #3A5050 | `create_map_object: wet glistening moss clump on slate, top-down 35° view, 18×18 pixels, irregular wet clumped outline, muted dusty cool dark green with faint sheen, flat ground decal, no creature shape, no character features` |
| Underground lichen | dim underground lichen | 16×16 | 0.20 | ±360° | #4A5240 → #5A6250 | `create_map_object: dim underground lichen patch on slate, top-down 35° view, 16×16 pixels, irregular flat lichen outline, muted dusty dark olive grey-green, flat ground decal, no creature shape, no character features` |

---

## OUTPUT 2 — ScatterBrush Content Matrix

### Shattered Keep (2 brushes)

#### `Scatter_Stones_ShatteredKeep` (extends Codex stub)

| Entry | Sprite | Size (px) | Perlin Freq | Threshold | Min/Max | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Small pebble | single small pebble | 6×4 | 0.18 | 0.55 | 12 / 28 | `create_object: tiny single pebble, top-down 35° view, 6×4 pixels, simple oval shape, muted dusty grey-brown stone, no shadow, transparent background, isolated small prop` |
| Medium stone | weathered stone | 10×8 | 0.14 | 0.60 | 6 / 14 | `create_object: small weathered stone, top-down 35° view, 10×8 pixels, irregular rounded shape, muted dusty grey-brown stone, no shadow, transparent background, isolated small prop` |
| Chipped rock | angular chipped rock | 12×10 | 0.12 | 0.65 | 3 / 8 | `create_object: angular chipped rock, top-down 35° view, 12×10 pixels, sharp irregular faceted shape, muted dusty grey stone, no shadow, transparent background, isolated small prop` |
| Twin pebble cluster | two-pebble cluster | 10×6 | 0.16 | 0.62 | 2 / 6 | `create_object: small cluster of two pebbles together, top-down 35° view, 10×6 pixels, two attached rounded shapes, muted dusty grey-brown stone, no shadow, transparent background, isolated small prop` |

#### `Scatter_Moss_Tufts_ShatteredKeep` (extends Codex stub)

| Entry | Sprite | Size (px) | Perlin Freq | Threshold | Min/Max | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Small moss tuft | tiny moss tuft | 6×6 | 0.22 | 0.50 | 10 / 24 | `create_object: tiny single moss tuft, top-down 35° view, 6×6 pixels, simple soft round mossy clump, muted dusty forest green, no shadow, transparent background, isolated small prop` |
| Spread moss | small spreading moss | 10×6 | 0.20 | 0.55 | 6 / 14 | `create_object: small spreading moss tuft, top-down 35° view, 10×6 pixels, soft elongated moss shape, muted dusty olive forest green, no shadow, transparent background, isolated small prop` |
| Dry moss clump | dried moss clump | 8×6 | 0.18 | 0.60 | 4 / 10 | `create_object: small dried moss clump, top-down 35° view, 8×6 pixels, irregular wilted moss shape, muted dusty olive desaturated, no shadow, transparent background, isolated small prop` |

### Alabaster Dawn (2 brushes — Faz 1.5)

#### `Scatter_CeremonialDebris_AlabasterDawn`

| Entry | Sprite | Size (px) | Perlin Freq | Threshold | Min/Max | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Pale stone chip | tiny pale stone chip | 6×4 | 0.18 | 0.58 | 10 / 22 | `create_object: tiny pale cream stone chip, top-down 35° view, 6×4 pixels, simple angular flake shape, muted dusty pale cream ivory, no shadow, transparent background, isolated small prop` |
| Ivory fragment | larger ivory fragment | 10×8 | 0.14 | 0.62 | 4 / 10 | `create_object: small ivory stone fragment, top-down 35° view, 10×8 pixels, angular broken shape, muted dusty pale ivory cream, no shadow, transparent background, isolated small prop` |
| Wax shard | dried wax shard | 8×6 | 0.16 | 0.60 | 6 / 12 | `create_object: small dried wax shard, top-down 35° view, 8×6 pixels, irregular soft chunk shape, muted dusty pale cream wax, no shadow, transparent background, isolated small prop` |
| Petal fragment | dried petal fragment | 8×6 | 0.20 | 0.55 | 8 / 18 | `create_object: small dried flower petal fragment, top-down 35° view, 8×6 pixels, soft curled petal shape, muted dusty pale tan rose, no shadow, transparent background, isolated small prop` |

#### `Scatter_PinkPebbles_AlabasterDawn`

| Entry | Sprite | Size (px) | Perlin Freq | Threshold | Min/Max | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Pink pebble | small pink pebble | 6×4 | 0.18 | 0.58 | 10 / 22 | `create_object: tiny pink stone pebble, top-down 35° view, 6×4 pixels, simple oval shape, muted dusty pale rose pink, no shadow, transparent background, isolated small prop` |
| Mauve stone | mauve mineral stone | 10×8 | 0.14 | 0.62 | 4 / 10 | `create_object: small mauve mineral stone, top-down 35° view, 10×8 pixels, irregular rounded shape, muted dusty pale mauve, no shadow, transparent background, isolated small prop` |
| Cream pebble | cream pebble | 8×6 | 0.16 | 0.60 | 6 / 14 | `create_object: small cream colored pebble, top-down 35° view, 8×6 pixels, soft rounded shape, muted dusty pale cream, no shadow, transparent background, isolated small prop` |

### Cave (2 brushes — Faz 2)

#### `Scatter_MineralChunks_Cave`

| Entry | Sprite | Size (px) | Perlin Freq | Threshold | Min/Max | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Mineral chip | tiny mineral chip | 6×4 | 0.18 | 0.58 | 12 / 26 | `create_object: tiny cold blue mineral chip, top-down 35° view, 6×4 pixels, simple angular crystal flake shape, muted dusty cold blue, no shadow, transparent background, isolated small prop` |
| Crystal shard | small crystal shard | 8×10 | 0.14 | 0.62 | 4 / 10 | `create_object: small cold blue crystal shard, top-down 35° view, 8×10 pixels, elongated angular crystal shape, muted dusty cold blue with faint glow, no shadow, transparent background, isolated small prop` |
| Mineral cluster | small mineral cluster | 10×8 | 0.12 | 0.65 | 2 / 6 | `create_object: small cold blue mineral cluster, top-down 35° view, 10×8 pixels, irregular multi-faceted cluster shape, muted dusty cold blue, no shadow, transparent background, isolated small prop` |
| Wet pebble | wet dark pebble | 6×4 | 0.20 | 0.55 | 8 / 18 | `create_object: small wet dark pebble, top-down 35° view, 6×4 pixels, simple oval glossy shape, muted dusty dark blue-grey wet sheen, no shadow, transparent background, isolated small prop` |

#### `Scatter_FungalGrowth_Cave`

| Entry | Sprite | Size (px) | Perlin Freq | Threshold | Min/Max | PixelLab MCP Prompt |
|---|---|---|---|---|---|---|
| Small mushroom | tiny pale mushroom | 6×8 | 0.20 | 0.55 | 6 / 16 | `create_object: tiny pale cave mushroom, top-down 35° view, 6×8 pixels, simple small cap-and-stem shape, muted dusty pale olive grey, no shadow, transparent background, isolated small prop` |
| Fungal cluster | small fungal cluster | 10×8 | 0.18 | 0.60 | 3 / 8 | `create_object: small pale fungal cluster, top-down 35° view, 10×8 pixels, irregular grouped soft shape, muted dusty pale olive grey, no shadow, transparent background, isolated small prop` |
| Dim glow-cap | dim glow-cap mushroom | 6×8 | 0.16 | 0.62 | 4 / 10 | `create_object: small dim glow cave mushroom, top-down 35° view, 6×8 pixels, simple cap-and-stem shape with subtle cold blue cap, muted dusty cold blue-tinted, no shadow, transparent background, isolated small prop` |

---

## OUTPUT 3 — Organic Feel Philosophy per Biome

**Shattered Keep.** "Ruined fortress aftermath" — dominant texture stays bare stone. Dust+ash patches (density 0.20–0.30) form the universal underlayer that breaks tile-edge regularity across the biome. Moss (0.25–0.40) clusters at corners/edges, never blanketing center floor. Rift patches deliberately sparse (0.06–0.20) — Karar #98 cyan/violet *punctuates* the muted earth palette, not competes. Scatter stones high-count (12–28 pebbles per chunk) dissolves straight tile edges into rubble noise. Stacking three sparse layers (dust + moss + rift) each subtle, additively dense — the "no kare kare" goal solved through layered sparsity.

**Alabaster Dawn.** "Pristine but melancholic" — organic overlay much lighter (0.05–0.18 across the board). Cream drift + pale ivory swirl dominant but low-density (biome wants stone-readable, not dust-readable). Rift tendrils lowest density anywhere (0.05–0.06) — canon is "contamination *starting*". Ceremonial debris (gold inlay, wax) scattered at low count implies abandoned ritual without clutter. Warm-pastel palette: pink stone + cream dust + faint mauve, with rift cyan the only cool intrusion. Organic feel comes from rotation jitter and tint variance more than density.

**Cave.** "Underground claustrophobic mineral network" — organic layer leans heavier (moss 0.20–0.35, mineral 0.15–0.20) pushing slate floor toward enclosed overgrowth. Cold blue mineral veins + crystal scatter provide glow accent pulling biome away from grey monotony. Fungal scatter (mushrooms, glow-caps) replaces Shattered Keep's pebble count → floor feels grown-into rather than walked-over. Cool-monochromatic palette: slate grey + cold blue + cool green moss, with pale fungal olive grey as desaturated relief. "No kare kare" solved through *maximum density of small organics*.

---

## OUTPUT 4 — Faz Roadmap

| Faz | Biome | Asset hedef | PixelLab gen count | Tahmini credit (1 cr/gen avg) |
|---|---|---|---|---|
| Faz 1 (Hafta 2) | Shattered Keep | 3 PatchAtlas (12 entries) + 2 ScatterBrush (7 entries) | 19 gen + ~30% reject buffer = 25 gen | ~25 credit |
| Faz 1.5 (Hafta 3) | + Alabaster Dawn | 3 PatchAtlas (11 entries, -1 reuse) + 2 ScatterBrush (7 entries) | 17 gen + 30% buffer = 22 gen | ~22 credit |
| Faz 2 (Hafta 4+) | + Cave | 2 PatchAtlas (8 entries) + 2 ScatterBrush (7 entries) | 15 gen + 30% buffer = 20 gen | ~20 credit |
| **Total** | All 3 biomes | 8 PatchAtlas + 6 ScatterBrush | 67 generations | ~67 credit |

### Faz 1 priority sequencing (sequential, do not parallelize)

1. `Scatter_Stones_ShatteredKeep` (4 entries) — lowest risk, S76-proven `create_object` flow
2. `Scatter_Moss_Tufts_ShatteredKeep` (3 entries)
3. `PatchAtlas_Dust_ShatteredKeep` (4 entries) — `create_map_object` flat decal warmup
4. `PatchAtlas_Moss_ShatteredKeep` (4 entries) — riskiest map_object (creature drift risk on moss)
5. `PatchAtlas_Rift_Fracture_ShatteredKeep` (4 entries) — last, after calibration

### Risk acknowledgments

- `create_map_object` rejection rate observed at ~25–35% in S76. 30% buffer covers this.
- If reject rate > 40% on Moss atlas: fall back to `create_tiles_pro` with `intendedRole=Patch` tagging (master spec v3 §1 multi-role) — explicit Phase 1.5 fallback path.
- Alabaster Dawn cream drift `075242f4` is single reused asset across phases — do not regenerate.

---

## Codex Phase 1 Stub Rename (housekeeping)

- `PatchAtlas_Rift_Fracture.asset` → `PatchAtlas_Rift_Fracture_ShatteredKeep.asset` (biome suffix consistency)
- `Scatter_Moss_Tufts.asset` → `Scatter_Moss_Tufts_ShatteredKeep.asset` (biome suffix consistency)
- Inspector reference repair after rename in `RoomRecipe_ShatteredKeep_Combat_01.asset` ve `Corridor_01.asset` (if they reference these by GUID this is automatic; manual rebind if name-based).
