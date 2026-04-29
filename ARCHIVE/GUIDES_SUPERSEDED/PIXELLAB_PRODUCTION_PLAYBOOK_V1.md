# PixelLab Production Playbook V1 (RIMA)

Date: 2026-04-22
Status: Ready for production
Note: V1 - direction strategy superseded by `PIXELLAB_ANIM_LOCKED_V2.md`.

## 1) Confirmed Baseline
- Final in-game character standard: `128x128`.
- User-confirmed live behavior:
  - `Animate with text NEW` at 128px can reach `16 frames`.
  - At 220px, `Animate with text NEW` was limited to lower max (observed 10).
- Production decision:
  - Keep all character base/idle/run/attack assets at `128x128`.
  - Use 220-only generation as optional temporary staging, not final deliverable.

## 2) Tool Roles (Do Not Mix Randomly)
- `Animate with text NEW`:
  - Use for main motion blocks and first-pass key sequences.
  - Preferred frame range: `8-16` depending on complexity.
- `Interpolate NEW`:
  - Use for in-between expansion and transition bridging (`A->B`, `B->C`).
  - Preferred for smoothing, timing, and transition stitching.
- `Edit image / Inpaint`:
  - Use for strong pose endpoints when model fails to reach desired silhouette.

## 3) Prompt Discipline
- Keep prompts short and structural.
- One generation = one motion intention.
- No camera angle text if camera is already set in UI.
- Keep hard constraints in all segments when weapon/hand logic is critical.

Template:
`[identity], [single action], [2-3 hard constraints], pixel art sprite`

Warblade hard constraints:
- `both hands on same long hilt`
- `right hand near crossguard, left hand near pommel`
- `heavy wide blade, low readable silhouette`

## 4) 8-Direction Strategy
- No mirror workflow: every direction is generated independently.
- Use locked order for every state: `SE -> E -> S -> NE -> N -> SW -> W -> NW`.
- Keep per-direction prompts explicit (`facing <direction>`) and keep Warblade hand/hilt constraints in every segment.
- If a direction fails silhouette/QC, regenerate that direction directly from its own clean keyframe.

## 5) Animation Construction Strategy
Use segmented chains. Do not over-prompt one giant sequence.

General:
1. Generate/choose keyframes (A, B, C).
2. Interpolate `A->B`.
3. Interpolate `B->C`.
4. Trim duplicate hold frames.
5. Normalize frame timing.

### Idle loop
- Pattern: `A -> B -> A`
- Target total frames: `6-10`.
- Keep displacement minimal.

### Run loop
- Pattern: `contact -> passing -> contact`.
- Target total frames: `8-12`.
- Preserve mass read and foot contact.

### Run -> Idle transition
- Build dedicated bridge:
  - `run_end -> idle_start` with short interpolate.
- Target frames: `3-6`.
- Avoid regenerating full idle from run every attempt.

### Attack
- Pattern: `windup (A) -> impact (B) -> recovery (C)`.
- Target frames: `6-12` by class/weapon weight.

## 6) QC Gate (Per Direction, Per Animation)
- Silhouette stable and readable.
- Weapon hand contact continuity preserved.
- No accidental scale pumping frame-to-frame.
- No unintended orientation drift.
- No dead/black frame artifacts.
- Transition clips connect cleanly:
  - `run -> run_idle_bridge -> idle`

## 7) Naming and Output Convention
- Keep directional suffixes strict:
  - `_S, _SE, _E, _NE, _N, _NW, _W, _SW`
- Keep state strict:
  - `base`, `idle`, `run`, `attack`, `transition`
- Export examples:
  - `warblade_idle_SE.png`
  - `warblade_run_NE.png`
  - `warblade_run_to_idle_W.png`

## 8) Minimal Production Loop
1. Pick one class and one state (`Warblade run`).
2. Produce only `SE` first.
3. QC pass.
4. Produce remaining directions directly in locked order (no mirror).
5. Build transition set (`run->idle`).
6. Integrate in engine and playtest.
7. Iterate only failing segments.

## 9) Immediate Next Batch (Recommended)
- Warblade:
  - `idle 8-dir`
  - `run 8-dir`
  - `run->idle transition 8-dir`
- Use this order:
  1. SE
  2. E
  3. S
  4. NE
  5. N
  6. SW
  7. W
  8. NW
