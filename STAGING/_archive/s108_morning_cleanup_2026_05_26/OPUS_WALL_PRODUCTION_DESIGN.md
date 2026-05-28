# Opus Wall Production Design (Wave B — 2026-05-19)

## Concept tone calibration (anchors all decisions)

Direct visual audit of `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png` + each asset:

- **Concept walls** = cool neutral gray stacked stone, low saturation, ~3× character tall, side-face reveal at top. Top edge = brick blocks rising 3-4 rows above floor plane. Bottom edge = shorter battlement-style ridge.
- **Concept braziers** = stone column brazier at gate flanks, **luminous cyan/aquamarine flame** (NOT amber). Two per arch.
- **Concept gate arches** = dark gothic stone arches in walls, with cyan brazier glow flanking.

### Asset-by-asset reality (overrides Wave A "all 3 PixelLab perfect" claim)

| Asset | Tone | Verdict |
|---|---|---|
| `pixellab_wall_section_horizontal` (384×216) | Cool dark charcoal, side-face reveal, rubble base | **HERO** — top/bottom edges |
| `pixellab_wall_arch_section` (192×128) | **Neutral cool gray** — BEST concept match | **HERO** — gate walls |
| `pixellab_wall_corner` (192×128) | **Purple/violet saturated tint** — fails concept gray | **❌ PROBLEMATIC** color family wrong |
| `painterly_wall_01-12` | Warm brown stone | Tone clash, reclassify Act2/3 candidate |
| `gate_arch` | Dark stone arch + cyan accents | **HERO** — flanks |
| `wall_edge_stone` | Flat top-down strip | Superseded → archive |
| `wall_decoration_vines` | Transparent overlay | Keep accent |
| `painterly_prop_06_burning_brazier` | Amber flame, dark wood bowl | Base OK, flame needs cyan conversion |

---

## Decisions

### D1 — Hero wall set: **(c) HYBRID**

PixelLab `pixellab_wall_section_horizontal` + `pixellab_wall_arch_section` for all straight edges + gate sections. PixelLab `pixellab_wall_corner` REJECTED (purple-violet tone clash). Use `gate_arch` rotated/cropped as 4-corner pillar. Contingent: 3-4 gen mini-regen for proper concept-tone corner pieces if hybrid trick fails Step 6 visual test.

**Rationale:** 2/3 downloaded pieces match concept tone. Corner is the only mismatch, cheap to replace. `painterly_wall_xx` warm-brown structurally incompatible. Full regen (d) excessive when 2/3 already match.

### D2 — Brazier flame: **(b) CYAN Light2D OVERLAY on amber sprite**

Keep amber sprite. Add Unity Point Light2D (color `#00FFCC`, intensity 1.2, radius 2.0) + small additive cyan flame quad (alpha 0.7, blend mode Additive in URP) — color-shifts amber to cyan in-engine, ZERO new gen.

**Rationale:** Reuses canon `#00FFCC` rift signature. ZERO gen. Both braziers per concept. ⚠ **Karar #98 conflict** — see escalation below.

**Fallback:** If user keeps cyan rift-only, brazier flame = cooler blue `#7FB8FF` instead.

### D3 — Wall scale: **(b) PixelLab native size = ~3× character**

PixelLab 384×216 native PPU 32 = 3-4 cell height = concept ratio. No stretching.

**Trade-off:** Larger walls occlude ~1-2 cells along edges. Acceptable for spawn/intro rooms (low combat density). Reassess for combat-heavy rooms.

### D4 — Floor-wall transition band: **(a) SKIP**

Hero sprite has integrated rubble at base. Adding tilemap band would (i) add new layer for one purpose, (ii) clash with sprite-baked rubble, (iii) violate Karar #143 6-layer canonical + Map Plan v1.

**Fallback if hard edge persists:** scatter pass of `small_stones` from Universal pack — NOT a tile band.

### D5 — File pack cleanup

| Asset | Verdict | Reason |
|---|---|---|
| `wall_edge_stone.png` | **ARCHIVE** → `Assets/Art/_archive_faz1/walls_superseded/` | Flat top-down superseded; preserve GUID for revisit |
| `wall_decoration_vines.png` | **KEEP** | Hanging chains/vines accent (concept shows) |
| `painterly_wall_01-12` (12 files) | **KEEP, tag `WARM_TONE_VARIANT`** | Act 2 (Sunken Vault) or Act 3 (Ember Halls) candidate — classify NOT delete |
| `gate_arch.png`, `gate_spikes.png` | **KEEP** | Active hero |
| `painterly_prop_06_burning_brazier.png` | **KEEP** | D2 Light2D overlay reuse |
| `spawn01_faz1_polish_v1/v2/v3.png` (STAGING) | **DELETE** | Iteration screenshots, git covers history |
| `test_pixellab.png`, `test_api_dl.png` | **DELETE** | Test artifacts |
| 8× `keep_wall_v2`/`alabaster_wall` 32×32 PixelLab IDs | **DEFER IMPORT** | Future procedural tilemap rooms |
| `ab0f5ab4` alabaster_layered perimeter wall | **SKIP** | One-piece perimeter incompatible with 18×12 edge composition |

Total: 1 archive move, 5 deletions, 0 git rm (preserve GUIDs per `project_asset_pack_organization_lock`).

### D6 — Composition plan

**Room geometry:** 18×12 cells, cellSize 0.1667 (6 cells/unit). Interior X:0→3, Y:0→2 world units. Gates at (0, 1) and (3, 1).

**Convention:** SpriteRenderer parented to `Walls/{Top,Bottom,Left,Right}` empties. Pivot=BottomCenter for horizontals, BottomLeft for corner pillars. PPU 32. SortingLayer=Walls.

#### Spawn_01 (intro, ornate full perimeter)

| Position | Sprite | World (x,y) | SO | Notes |
|---|---|---|---|---|
| Top-left corner | `gate_arch` (crop right half) | (0.0, 2.0) | 100 | Corner pillar role |
| Top edge seg 1 | `pixellab_wall_section_horizontal` | (1.0, 2.0) | 100 | Native |
| Top edge seg 2 | same (flipX=true) | (2.0, 2.0) | 100 | Variation |
| Top-right corner | `gate_arch` (crop left, rotated) | (3.0, 2.0) | 100 | Mirror |
| Right edge top | `pixellab_wall_section_horizontal` | (3.0, 1.5) | 99 | |
| **Right gate** | `pixellab_wall_arch_section` | (3.0, 1.0) | 99 | HERO arch |
| Right edge bottom | `pixellab_wall_section_horizontal` | (3.0, 0.5) | 99 | |
| Bottom-right corner | `gate_arch` (rotated 180°) | (3.0, 0.0) | 98 | |
| Bottom edge seg 1 | `pixellab_wall_section_horizontal` (crop top 60%) | (2.0, 0.0) | 98 | Short battlement |
| Bottom edge seg 2 | same (flipX) | (1.0, 0.0) | 98 | |
| Bottom-left corner | `gate_arch` (rotated 180°) | (0.0, 0.0) | 98 | |
| Left edge bottom | `pixellab_wall_section_horizontal` | (0.0, 0.5) | 99 | |
| **Left gate** | `pixellab_wall_arch_section` (flipX) | (0.0, 1.0) | 99 | HERO arch mirror |
| Left edge top | `pixellab_wall_section_horizontal` | (0.0, 1.5) | 99 | |
| Brazier L + cyan Light2D | `painterly_prop_06_burning_brazier` | (0.45, 1.0) | 110 | Flanks left gate |
| Brazier R + cyan Light2D | `painterly_prop_06_burning_brazier` | (2.55, 1.0) | 110 | Flanks right gate |
| Vine accent ×2 | `wall_decoration_vines` | (0.8, 2.0), (2.2, 2.0) | 101 | Top hanging |

#### Spawn_02 (deeper room — variant)

Same skeleton + deltas:
- **Drop arch from LEFT side** (replace with plain horizontal) — only EXIT (right) is arched, closed-off feel
- **Single brazier** (right gate only) — darker/quieter
- **Bottom corner vines ×2** — overgrowth
- **No top vines** — cleaner silhouette
- **Mirror top/bottom flipX patterns** vs Spawn_01 — avoid identical tiling

~16 SpriteRenderers each room.

---

## Integration plan (Codex/UnityMCP step-by-step)

1. **Asset import:** PPU=32, Filter=Point, Compression=None, Pivot=BottomCenter (wall horizontal/arch), BottomLeft (corner pillar). Sprite Single mode.
2. **Wave 4 cleanup:** Delete current 14× painterly_wall SpriteRenderers from Spawn_01 + Spawn_02. Keep asset files.
3. **Spawn_01 composition:** 14 SpriteRenderers per D6 table. `Walls/Top|Bottom|Left|Right` empty parents. SortingLayer=Walls. Verify gate alignment.
4. **Spawn_02 composition:** Same + variant deltas.
5. **Brazier cyan conversion:**
   - Light2D Point, color=`#00FFCC` (OR `#7FB8FF` if scope strict), intensity=1.2, outer=2.0, inner=0.5
   - Child SR cyan flame quad, additive blend, color `#3DD9D9`
6. **Hero corner test gate:** Render Spawn_01 with gate_arch as corner pillars. If broken → STOP + mini-regen 3-4 corner pieces.
7. **Vine accent pass:** 4-5 vine SR additive blend.
8. **File pack cleanup per D5.**
9. **Screenshot verify:** `walls_v2_spawn01.png` + `walls_v2_spawn02.png` side-by-side with concept.
10. **Test pass:** RoomFlowTests + 1-2 wall-frame snapshot tests.

---

## Cost summary

- **PixelLab gen:** 0 baseline; up to 3-4 contingent (Step 6 corner failure path)
- **Codex/Unity work:** ~3 hours total
- **File operations:** 1 archive move, 4 deletions
- **Karar candidates:** Karar #98 cyan scope expansion (if D2.a accepted)

---

## Gaps for Wave C Codex review

1. **Corner piece resolution** — gate_arch crop trick valid OR mini-regen needed? Decide post-Step 6.
2. **Cyan flame canon scope** — Karar #98 strict (`#7FB8FF` fallback) vs expansion (`#00FFCC` brazier+rift). **USER ESCALATION.**
3. **Sort order at gate overlap** — brazier (SO 110) in front of arched wall (SO 99) at same world pos. Z-fighting check.
4. **`painterly_wall_xx` reclassification timing** — move to Act2 folder now vs wait for Act 2 design?
5. **Spawn_02 differentiation strength** — single brazier + closed-left-gate enough, or need unique mossier wall variant?

---

## Conflict check

- **Karar #100** (35° anchor): ✅ PRESERVED — all hero sprites at 35°
- **Karar #143** (6-layer painter): ✅ PRESERVED — walls L3 SpriteRenderer, no new layer
- **Karar #147** (Multi-Layer Painter LIVE): ✅ PRESERVED — operates on top of painter
- **Karar #148** (Branch D+E hybrid): ✅ PRESERVED — camera tilt + floor de-emph untouched
- **Karar #98** (cyan rift signature): ⚠ **CONDITIONAL CONFLICT** — D2 scope expansion. **USER DECISION REQUIRED.**

---

## ESCALATION — User must decide before Wave C

**Question:** Karar #98 reserves cyan `#00FFCC` for floor rift cracks. Concept art shows cyan brazier flames. Two paths:

| Path | Color | Effect |
|---|---|---|
| (A) **Scope expansion** — cyan everywhere "rift-corrupted" | `#00FFCC` brazier flame + rift cracks | Strengthens "contamination glow runs through the keep" theme. Cyan = rift signature wherever rift touches. |
| (B) **Strict rift-only** — brazier gets cooler blue, distinct from rift | Brazier `#7FB8FF`, rift `#00FFCC` | Preserves rift uniqueness as visual signature. Two cool colors (sky-blue + cyan) read distinct |

User picks A or B → Wave C dispatches Codex review with that choice locked.
