# RIMA — Depth + Connected Wall-Run Recipe (S6)
**Opus, senior 2D environment art-director. 2026-05-31.**

Goal: take the Ruined Keep demo room from **flat + scattered isolated wall chunks** to the
`Assets/Art/ConceptRefs/chatgpt_ref_wall_anchor.png` look = **continuous edge-to-edge masonry
wall-runs enclosing back + sides, real value/height depth**, kept as a **HYBRID** with the
floating-island front edge (`STAGING/floor_perspective_concepts/03_wallless_improved.png`:
void + cyan rim on the OPEN/front side).

Camera/palette/collider rules are **already LOCKED** in `RUINED_KEEP_ROOM_LOOK_LOCK_S6.md`
(flat ortho, no tilt, PPU64, Custom-Axis Y-sort, pivot=bottom-center, void edge-stop). This doc
does **not** re-litigate those; it only fixes (a) flatness, (b) walls-don't-connect, plus the
exact perimeter plan and the sprite decision.

Unit convention used below: **1 cell = 64px = 1 world unit** (PPU64). "world unit" = "cell".

---

## 1. ROOT-CAUSE OF "FLAT" — what's missing vs chatgpt_ref

The ref reads deep because of FIVE things the current room lacks. Concrete targets:

### 1a. Wall visible-height ratio (biggest miss)
- In the ref, the back wall + arch occupy roughly the **top 35–45% of the frame**; the lit wall
  front-face alone is **~2.5–3 cells tall** of visible masonry above the floor line, and the
  arch peak is **~4 cells**.
- TARGET visible heights (above the floor contact line), at 64PPU:
  - **Solid wall run front face: 2.5 cells = 160px** (sprite art height incl. crenellated top ≈ 192px; bottom 0.5 cell is the foot that sits on/behind the floor edge).
  - **Corner pillar / buttress: 3 cells = 192px** (taller than the run → reads as a structural anchor, hides the run's end seam).
  - **Arch gate peak: 4 cells = 256px.**
  - **Low front parapet stub: 0.75 cell = 48px** (camera clearance; never blocks the hero).
- **Scale multiplier vs a 1-cell floor tile (64px):** wall-run = **2.5×**, pillar = **3×**,
  arch = **4×**, parapet = **0.75×**. The current scatter kit reads flat partly because chunks
  sit at ~1.5–2× and are spaced apart, so no element dominates the vertical.

### 1b. Value / contrast structure (second biggest miss)
The ref has a hard **dark-floor / lit-wall split**. Lock these values (sRGB hex), darkest→lightest:
| surface | hex | role |
|---|---|---|
| Void (beyond edge) | `#050407` | near-black, makes the island pop |
| Floor base | `#15131c` | dark indigo-slate (lift the current too-dark placeholder to here) |
| Floor torch-pool peak | `#3a2a1f` warm tint over floor | amber light landing on ground |
| Wall **front FACE** (lit) | `#3b3950` | the value that sells depth — must be CLEARLY lighter than floor |
| Wall **top / crenellation** (catching ambient) | `#4c4a63` | lightest masonry, rim of cool light |
| Wall **shadow side / recess** | `#211f2e` | between floor and face value |
| Cyan rift crack (emissive) | `#27e0c8` | sparing, <15% of pixels |
| Torch flame core | `#ffd27a` → halo `#ff9a24` | warmest point in frame |

Key rule: **floor value (#15131c, L≈13) must be a full step darker than the wall front face
(#3b3950, L≈30).** That ~17-point luminance gap IS the depth. The flat room failed because
floor and walls were near the same value.

### 1c. Layering / overlap depth cues
- The ref overlaps elements **front-over-back**: braziers overlap the wall base, banners hang
  IN FRONT of the wall face, the hero passes IN FRONT of pillars. Overlap = the cheapest depth
  cue and the current room has almost none (chunks float in their own cells, no overlap).
- RULE: every wall-run segment must be **partially overlapped at its base** by at least one
  foreground prop (brazier, rubble, banner-bottom) every 4–6 cells. Hero Y-sorts in front when
  standing below the wall foot line.

### 1d. Foreshortening (floor + wall)
- The ref floor tiles compress vertically as they recede (top rows ~70% the screen-height of
  bottom rows). Fake with a **0.85× vertical squash on floor-tile V-scale for the back 1/3** of
  the room (purely cosmetic; collider stays square). Optional, low priority — value contrast
  matters far more.
- Wall front faces are drawn flat-on (no side face — see §2); foreshortening lives only in the
  floor and in the height stagger of pillars vs runs.

### 1e. Contact shadows + torch gradient on walls (the "glue")
- **Contact shadow** under EVERY wall segment, pillar, prop, and the hero:
  soft oval, color `#000000`, **opacity 45%**, size = **1.1× the footprint width × 0.35 cell
  deep**, offset **+0.05 cell** below pivot. Without these, props look pasted-on (a top cause of
  "flat").
- **Torch light gradient on the wall face:** each wall torch casts a warm radial gradient ON the
  masonry behind/above it — peak `#ff9a24` at flame, falling to floor value over **~2.5 cell
  radius**, additive/soft-light blend at ~60% strength. This vertical warm-to-dark gradient on a
  tall flat face is the single strongest "I am standing inside a tall room" cue. Place a torch
  every **4–6 cells** along solid runs.

---

## 2. ROOT-CAUSE OF "WALLS DON'T CONNECT" + corrected algorithm

### 2a. Why the current 8 chunks can NOT fake a continuous run
I inspected the 8 kit sprites. Dimensions:
`wall_tall_intact 128×192`, `wall_mid_cracked 128×160`, `wall_low_parapet 128×96`,
`pillar_tall 64×160`, `pillar_broken 64×96`, `corner_buttress 128×176`, `rubble_pile 96×64`,
`arch_gate 160×192`.

**They are 3/4 isometric DIORAMA CHUNKS, not run-segments.** Each wall sprite is drawn as a
self-contained 3D box: it shows a front face AND a receding right-side face, a finished
crenellated top silhouette, AND its own complete left + right vertical end edges. Consequences:
- Butt two together and you get **two visible boxes with doubled side-faces and a hard seam** —
  never one wall. The lighting on each chunk's side face also points the same way, so the join
  looks like two objects clipping, not masonry continuing.
- Their tops are jagged/ruined per-chunk, so a row of them is a row of little towers, exactly the
  "scattered, doesn't connect" complaint.
- `corner_buttress` and `pillar_tall` ARE usable as-is (anchors/ends are SUPPOSED to be discrete
  objects). `arch_gate` is usable as-is (it's a single hero piece). `rubble_pile`,
  `wall_low_parapet`, `pillar_broken` remain fine as decorative break-up props.

**DECISION (firm): NEW tileable run-segment sprites are REQUIRED.** Tight-overlap faking with the
existing chunks will NOT achieve the ref's continuous edge-to-edge wall and will keep the
scattered look. Reasoning: the seam problem is structural (per-chunk side faces + per-chunk
finished ends), not a spacing problem; no amount of overlap removes a baked-in right-side face.

### 2b. Spec for the NEW tileable wall-run sprites (imagegen placeholder → PixelLab final)
Produce a **flat front-facing run kit** (NOT 3/4 box; this is the one place the room reads as a
true wall, like the ref's back wall):
- **`wall_run_mid` — 64×192** (1 cell wide, 2.5 cells visible face + 0.5 foot). Front face ONLY,
  drawn flat-on (no side face). **Left and right edges are seamless vertical butt-joins** (a
  brick course continues straight across the edge; the leftmost and rightmost pixel columns mirror
  so N copies tile into one unbroken wall). Flat crenellated top line at a CONSTANT height.
- **`wall_run_cracked` — 64×192**: same footprint + seamless edges, but with a cyan rift crack and
  a few displaced bricks. Drop-in variant for jitter WITHIN the run (see §2c).
- **`wall_run_low` — 64×96**: 0.75-cell parapet version, seamless edges, for the front/S stubs.
- **`wall_cap_left` / `wall_cap_right` — 64×192**: optional finished END pieces (a clean vertical
  jamb) for where a run terminates at a void gap — so the run END looks intentional, not torn.
- Keep `corner_buttress`, `pillar_tall`, `arch_gate`, `rubble_pile`, `pillar_broken` from the
  existing kit as anchors/breakers. Log every new imagegen output to
  `STAGING/IMAGEGEN_PLACEHOLDER_REGISTRY.md` (final art = PixelLab, per HARD rule).
- All: PPU64, pivot bottom-center, sorted on the "Entities" Custom-Axis layer, Sort Point = Pivot.

### 2c. Corrected placement algorithm — RUN-FIRST-THEN-VARY (replaces the old scatter-first rules)
The old `RUINED_KEEP_ORGANIC_COMPOSITION_RULES.md` is **scatter-first** (jitter positions ±5–15%,
wide gaps, cluster-in-corners). That produced the disconnected look and is **superseded for
perimeter walls** by this. (The old rules still apply to LOOSE rubble/decor INSIDE the run, not to
the wall line itself.)

```
STEP 1 — LAY THE SOLID RUN (continuous, on-grid, NO position jitter):
  For each perimeter segment marked SolidWallRun:
    place wall_run_mid tiles back-to-back, one per cell, EXACTLY on the grid line,
    pivots collinear, tops at CONSTANT height. Zero gaps. This is one unbroken wall.

STEP 2 — ANCHOR THE ENDS & CORNERS:
  At every run END (next to a VoidEdge/Entrance) and every room CORNER,
  place a corner_buttress or pillar_tall (3 cells tall) overlapping the last run tile
  by ~0.25 cell. The taller anchor hides the end seam and frames the gap.

STEP 3 — VARY *WITHIN* THE RUN (swap, don't move):
  Replace ~25–35% of wall_run_mid tiles in-place with wall_run_cracked (cyan crack)
  or wall_run_low (a crumbled dip). Vertical scale jitter allowed ±8% on the TOP only
  (crenellation height wobble) — pivots and the run line stay perfectly collinear.
  DO NOT translate tiles off the grid line. Variation = sprite swap + top wobble ONLY.

STEP 4 — OVERLAP FOREGROUND PROPS (depth glue):
  Every 4–6 cells along the run, overlap the base with a brazier / rubble_pile /
  banner-bottom (foreground prop Y-sorts in front of the run foot). Add wall torch +
  warm gradient (see §1e) at the same cadence.

STEP 5 — LOOSE SCATTER (old organic rules apply HERE ONLY):
  Outside the run line, on the open floor near corners and the altar, apply the old
  jitter/cluster/cause-and-effect rules for fallen pillars, rubble, leaning slabs.
```

Rule of thumb: **the wall LINE is engineered (collinear, gapless); the DECAY is decorative
(swaps + props + scatter).** Order matters: build the line first, break it second.

---

## 3. EXACT PERIMETER PLAN — demo room 16 wide × 12 deep

Floor cells indexed `(x,y)`, `x∈0..15` (W→E), `y∈0..11` (S→N). North (y=11) = back wall (tall),
South (y=0) = open front void. Origin bottom-left. Each label = the perimeter cell just OUTSIDE
the walkable floor on that edge (wall pivots sit on these cells, foot 0.25–0.5u outside walkable
per the LOCK). Hybrid target: **~70% solid / ~30% void**, fortress back + floating front.

### NORTH (y=11) — continuous tall run + centered arch + flanking banners/torches
```
x=0   corner_buttress (3c)            [NW anchor]
x=1   wall_run_mid
x=2   wall_run_mid  + wall_torch + warm gradient
x=3   wall_run_cracked  (+ hanging banner in front)
x=4   wall_run_mid
x=5   pillar_tall (3c)                [left arch flank] + freestanding brazier in front
x=6   arch_gate  (N-GATE, 160px≈2.5c wide, peak 4c, cyan rift) ─┐ centered
x=7   arch_gate continues  ───────────────────────────────────┘ (single 2.5c sprite spans x6–7)
x=8   pillar_tall (3c)                [right arch flank] + freestanding brazier in front
x=9   wall_run_mid  + wall_torch + warm gradient
x=10  wall_run_cracked (+ hanging banner)
x=11  wall_run_mid
x=12  wall_run_mid
x=13  wall_run_mid  + wall_torch
x=14  wall_run_cracked
x=15  corner_buttress (3c)            [NE anchor]
```
North = 100% SolidWallRun (no void). Arch centered at x6–7. Banners hang IN FRONT of run face;
torches every ~4 cells; braziers flank the gate. This wall fills the top of the frame = primary
depth.

### EAST (x=15) — mixed run + ONE void gap
```
y=11  corner_buttress (shared NE anchor, above)
y=10  wall_run_mid + wall_torch
y=9   wall_run_cracked
y=8   wall_run_mid
y=7   wall_cap_right                  [clean END jamb before gap]
y=6   VOID EDGE  ── cyan rim-light, hanging chains, falling-rock silhouette
y=5   VOID EDGE  ── (2-cell gap, frames the keep, floating-island danger)
y=4   wall_cap_left                   [clean START jamb after gap]
y=3   wall_run_mid + wall_torch
y=2   wall_run_low                    [transition down toward open S]
y=1   pillar_broken (1.5c)            [low SE-ish breaker]
y=0   → see SOUTH
```
East = ~67% solid, 33% void (the y5–6 gap).

### WEST (x=0) — mixed run + ONE void gap (asymmetric vs East)
```
y=11  corner_buttress (shared NW anchor, above)
y=10  wall_run_mid + wall_torch
y=9   wall_run_mid
y=8   wall_cap_right                  [END jamb before gap]
y=7   VOID EDGE  ── cyan rim, chains
y=6   VOID EDGE
y=5   VOID EDGE                       [3-cell gap — wider than East = asymmetry]
y=4   wall_cap_left                   [START jamb after gap]
y=3   wall_run_cracked + wall_torch
y=2   wall_run_low
y=1   rubble_pile (loose, overlapping foot)
y=0   → see SOUTH
```
West = ~58% solid, 42% void. East+West differ in gap width/position = asymmetric, ref-like.

### SOUTH (y=0) — OPEN FRONT VOID + low parapet stubs only (camera/player clearance)
```
x=0   VOID EDGE (cyan rim)
x=1   wall_run_low / parapet stub (0.75c)   [SW stub]
x=2   VOID EDGE
x=3   VOID EDGE
x=4   pillar_broken stub (low)               [breaker, never tall]
x=5..6 VOID EDGE  (open mouth — hero enters/exits here, framed)
x=7   VOID EDGE
x=8   VOID EDGE
x=9   rubble_pile (loose, low)
x=10  VOID EDGE
x=11  wall_run_low / parapet stub (0.75c)    [SE stub]
x=12  VOID EDGE
x=13  pillar_broken stub (low)
x=14  VOID EDGE
x=15  VOID EDGE (cyan rim)
```
South = ~80% open void, 20% low stubs. NOTHING here exceeds 0.75 cell tall (LOCK: never block
camera/hero). This is the floating-island front edge from `03_wallless_improved.png`: cyan rim,
shattered/stepped silhouette, hanging chains below.

### CENTER (interior, not perimeter)
- Rift altar / dais at ~(x7–8, y7) with a concentric cyan rune-circle decal (pulse), slightly
  off-axis per focal hierarchy. Floor torch-pools under each wall torch break floor monotony.

**Hybrid tally:** N 100% solid, E 67%, W 58%, S 20% → ~70% solid masonry / ~30% void edge.
Matches the LOCK's 60–75% solid target. Solid run encloses **back + upper sides** (fortress
silhouette); void opens the **front + mid-sides** (floating-island danger).

---

## 4. DEPTH-WITHOUT-CAMERA-TILT CHECKLIST (camera stays flat ortho — LOCKED)
All depth is faked in ART. Build/verify in this order (cheapest, highest-impact first):

1. **VALUE SPLIT (do first):** floor `#15131c` a full luminance step BELOW wall front face
   `#3b3950`; void `#050407` darkest. If only one thing ships, ship this.
2. **WALL HEIGHT:** runs 2.5c, pillars/buttress 3c, arch 4c above the floor line — tall flat
   front faces filling the top ~40% of frame.
3. **CONTACT SHADOWS:** `#000` 45%, 1.1× width × 0.35c oval under every wall/prop/hero. Glues
   everything to the ground; removes "pasted-on" flatness.
4. **OVERLAP / Y-SORT:** foreground props overlap wall bases every 4–6c; hero passes in front of
   pillars; banners hang in front of faces. Front-over-back = depth.
5. **TORCH WARM GRADIENT on wall faces:** `#ff9a24`→floor over ~2.5c radius, soft-light ~60%, one
   per 4–6c. Vertical warm-to-dark on a tall face = "inside a room."
6. **CYAN RIM ON VOID EDGES ONLY:** `#27e0c8` rim where floor meets void (front + side gaps),
   plus sparse cracks under walls. <15% of pixels. Outside-collider chains / falling-rock
   silhouettes / vertical cyan glow sell the drop.
7. **FLOOR FORESHORTENING (optional, last):** 0.85× vertical squash on back-1/3 floor tiles +
   slight top-row darkening. Cosmetic only; collider stays square.

---

## SUPERSEDES
- For PERIMETER WALL placement, this doc supersedes `RUINED_KEEP_ORGANIC_COMPOSITION_RULES.md`
  STEPS 1–4 (scatter-first). Those organic rules now apply ONLY to loose interior decor (§2c
  STEP 5). Camera/palette/collider remain governed by `RUINED_KEEP_ROOM_LOOK_LOCK_S6.md`.
