# RIMA Wall Inventory + Canon (Wave A — 2026-05-19)

**Mission:** Match concept `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png`. Decide wall production approach with existing inventory first; only gen if gap proven.

**Sources:**
- Sonnet rima-sonnet local file inspection (3D vs flat verdict)
- Orchestrator PixelLab MCP get_object on 3 priority IDs + 2 tag filters
- STAGING canon docs (NLM proxy)

---

## 1. File Assets — Visual verdict

### `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/painterly_wall_01-12`

**CRITICAL CORRECTION (Sonnet inspection):** Earlier orchestrator claim "flat top-down" was WRONG. These ARE 3D Hades-style angled walls with side-face reveal.

| File | Projection | Match to concept |
|---|---|---|
| `painterly_wall_01-03` corners (top) | 3D angled, top cap + front face | ✅ Match |
| `painterly_wall_04-05` edges (vertical) | 3D full side-face reveal | ✅ Match |
| `painterly_wall_06-08` corners (bottom) | 3D angled | ✅ Match |
| `painterly_wall_09-10` junctions (T+X) | 3D chunky columns | ✅ Match |
| `painterly_wall_11` door arch | 3D arch with depth | ✅ Match |
| `painterly_wall_12` alcove niche | 3D decorative panel | ✅ Match |
| `wall_edge_stone.png` | FLAT top-down strip | ❌ DROP — does not match Hades language |
| `wall_decoration_vines.png` | Sprite overlay (transparent bg) | 🟡 Use as accent overlay |

**Wave 4 already replaced wall_edge_stone × 14 with proper painterly_wall set in both Spawn_01 and Spawn_02.** Visual is much improved.

### `Assets/Art/AssetPacks/Act1_ShatteredKeep/gates/`
- `gate_arch.png` — 3D arch with cyan-touched edges. ✅ Strong concept match.

### `Assets/Art/AssetPacks/Act1_ShatteredKeep/props/`
- `painterly_prop_06_burning_brazier.png` — **AMBER/ORANGE flame** ❌ Concept shows **CYAN BLUE flame**. **MAJOR GAP.**

---

## 2. PixelLab object inventory — Priority IDs (visual inspection done)

### ⭐⭐⭐ Hero candidates (better match than file assets)

| Object ID | Description | Size | Style | Verdict |
|---|---|---|---|---|
| `b2703abf-63a2-41e9-bc90-a3e7cf5fcdd4` | Horizontal weathered stone dungeon wall section | **384×216** | 3D-perspective, dark gray stone + rubble base | ⭐⭐⭐ **PERFECT** — concept's hero horizontal wall match |
| `a3f9fcf1-7858-411f-9508-63500c2281f6` | Top-down 35° dungeon wall corner | **192×128** | L-shape 3D corner reveal + rubble | ⭐⭐⭐ **PERFECT** corner piece |
| `1d73e775-7024-4826-bab0-3a7600a53fdd` | Top-down 35° dungeon wall section | **192×128** | 3D wall + integrated door arch | ⭐⭐⭐ **PERFECT** door wall section |

**Tone observation:** PixelLab walls are slightly bluer/cooler than painterly_wall_01-12 (which is warmer gray). Concept art tone is closer to PixelLab (cool blue-gray with moss accents).

### Secondary (32×32 tiles — floor-wall transition?)

| Tag | Count | Use case |
|---|---|---|
| `keep_wall_v2` | 4 | 32×32 dark warm grey weathered wall tiles |
| `alabaster_wall` | 4 | 32×32 dungeon wall tiles |

These are too small for hero wall sprites. Possibly for tilemap-level floor↔wall transition band, NOT hero walls.

### Other PixelLab walls (need scan if budget allows)

Sonnet flagged `ab0f5ab4-4e6a-4e94-b8ab-3d9d5e31747f` (alabaster_layered_object_ids batch — single perimeter wall, gentle top highlight + bottom shadow). Not yet visually inspected.

Total PixelLab obj library: 97 objects. Confirmed hero wall candidates: **3**. Tile candidates: **8**. Not yet inspected: rest.

---

## 3. Canon answers (from NLM/STAGING)

### Q1 — Wall projection: 35° angled vs flat
**ANSWER: LOCKED 35° angled side-face reveal (Hades-style).**

> "Hades 35° feel comes from L3 wall overlay perspective + decoration angles + Unity camera setup, NOT tile angle." — Karar #100/#143 derived

> "Wall faces forward (top-down 3/4 view, ~30-35 degree pitch like Hades dungeon)." — asset_gen_asama1_batch.md Template C

> Production sizes spec: 384×216 horizontal / 424×632 vertical / 341×341 corner

L3_Walls = SpriteRenderer overlays, NEVER tilemap. Wave 4 implementation correct.

### Q2 — Brazier flame color: cyan vs amber
**ANSWER: UNRESOLVED. Decision needed by Opus.**

- Canon `#00FFCC` cyan signature = floor rift cracks / contamination only (Karar #98)
- Production brazier sprite = warm orange amber flame
- Concept art shows cyan blue brazier flames
- No karar locks brazier to a specific color

**Opus options:**
- (a) Keep amber (canon default, easy)
- (b) Regen brazier with cyan flame (~20 gen, matches concept)
- (c) Cyan Light2D overlay on amber brazier (cheap hybrid, current brazier stays)

### Q3 — Character/room scale
**ANSWER: Current production = wall ~2× character. Concept aspires to ~3-4× character.**

- 64px character at PPU 32 → 2×2 cells
- Wall sprite production spec = 128px (4 cells) → 2× character
- Concept reads as walls ~3-4× character → walls would need to be 192-256px tall
- 424×632 vertical wall spec exists in batch doc but not yet produced

**Opus decision needed:**
- (a) Accept current 2× wall height (smaller, more arcade-ish)
- (b) Use PixelLab `b2703abf` 384×216 horizontal section (larger, concept-match)
- (c) Gen new 424×632 vertical pieces (production spec, biggest)

---

## 4. Gaps for Opus design decision

| Gap | Description | Cost to resolve |
|---|---|---|
| **A** | Brazier flame color (amber/cyan/hybrid) | 0-20 gen |
| **B** | Wall set choice (painterly_wall_xx vs PixelLab b2703abf/a3f9fcf1/1d73e775 vs new gen) | 0 or 60+ gen |
| **C** | Wall scale ratio vs character (2× vs 3-4×) | Asset choice driven |
| **D** | Lighting pipeline (Light2D tint vs new prop) | Code/scene only |
| **E** | 32×32 wall tiles (keep_wall_v2, alabaster_wall) — floor-wall transition usage? | Already in inventory |

---

## 5. Existing usable assets summary

**For Opus to compose wall production approach:**

- **File assets (Hades-style 3D, Wave 4 implementation):**
  - `painterly_wall_01-12` (corners + edges + arch + alcove)
  - `gate_arch.png` (3D arch, cyan-touched)
  - `wall_decoration_vines.png` (accent overlay)

- **PixelLab hero walls (concept-matching, NOT yet imported to Unity):**
  - `b2703abf` 384×216 horizontal wall section
  - `a3f9fcf1` 192×128 corner
  - `1d73e775` 192×128 wall+arch

- **PixelLab secondary tiles:**
  - 4× `keep_wall_v2` 32×32
  - 4× `alabaster_wall` 32×32

- **Reference:**
  - `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png` (visual anchor)

---

## 6. Next steps (per orchestrator chain)

1. **Wave A.5 — Codex curation:** Codex reviews this inventory + verdicts. Confirms "doğru ürün kalsın, mantıksız ürün kalmasın" — which PixelLab objects + file assets to KEEP vs DROP, and which to PRIORITIZE.

2. **Wave B — Opus design:** Given curated inventory, designs wall production approach matching concept. Decides Gaps A-E.

3. **Wave C — Codex review:** Reviews Opus spec for realism/budget/RIMA constraint compliance.

4. **Wave D — Opus final:** Synthesizes Codex feedback into final wall production decision.

Then execution: import PixelLab assets to Unity (if chosen), Wave 4 redo with new asset set (if chosen), brazier color resolution.
