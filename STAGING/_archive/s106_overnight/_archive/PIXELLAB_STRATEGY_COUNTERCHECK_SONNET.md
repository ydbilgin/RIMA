# PixelLab Strategy Counter-Check — rima-sonnet — 2026-05-25

## Q1. Aspect ratio waste

**Verdict: Marginal for torch/brazier at 64px, unacceptable for column at 128px.**

`create_1_direction_object` at size=64 returning 16 candidates is the clear winner for Wall Torch and Brazier (both 32×64 native). Yes, upper half of the square is wasted canvas, but the tool still interprets the prompt as a vertical prop and will generate vertical forms. The 16-candidate return offsets the waste dramatically.

Column at 128×128 is different. 64×128 column padded to 128×128 is a high-profile object where aspect matters for silhouette read-at-a-glance in the painter canvas preview. At 128px the tool returns only 4 candidates, shrinking the selection advantage. Use `create_map_object` for Column: non-square canvas, 1 candidate per call, but run 3 calls for 3 candidates total. Net cost: 3 credits vs 1 credit for 4 candidates. The quality-per-asset traded is worth it for the single most perimeter-critical object (32 counted across reference images).

**Decision rule:** Use `create_1_direction_object` only when expected aspect ratio waste is below 30% OR candidate count return is 16. Otherwise, `create_map_object` with repeat calls.

## Q2. Style locking via style_images

**Verdict: Pass b340684f tiles as style_images AND include palette hex in prompt text — belt-and-suspenders is worth it at no extra credit cost.**

`style_images` locks texture grain and shading style. Prompt hex description (`#3A3D42 stone, #00FFCC cyan accent`) locks color intent. These address different generation axes and do not conflict.

Pass 2-4 floor tiles from b340684f (stone floor variants, ideally a cyan-rune tile to reinforce the cyan accent). Keep style_images count to 4 or fewer at size=64 to stay within the documented constraint (max 8 at size≤85). Zero additional credit cost.

## Q3. Socket-paint vs Pre-gen architecture

**Verdict: Option B — auto-selection from registry by socket subtype.**

Code refs from `RoomPainterWindow.cs`:
- Line 27: `PropSocket` is already a named `BrushMode` enum value
- Line 63: `private SocketType propSocketType = SocketType.Torch;` — painter already tracks a single active socket subtype per brush stroke
- Line 165: `KeyCode.S` hotkey bound to `PropSocket` brush mode

The painter's existing design intent: user picks a socket *type* (Torch, not specific prefab), paints at world position, generation resolves the asset. This is Option B.

Option A (manual prefab dropdown per socket): breaks the "world's easiest tool" goal by forcing prefab picker mid-paint stroke.
Option C (replace socket with direct object brush): loses ability to swap/re-randomize variants post-layout.

Option B with a `PropRegistry` ScriptableObject (socket subtype → List of prefab variants) means: paint TorchSocket, get `wall_torch_v1` or `wall_torch_v2` at random. Add manual override pin when user wants specific variant.

## Q4. Multi-candidate selection

**Rubric for Sonnet (non-vision cross-check):**

After PixelLab returns candidates:
- **Silhouette at 24px cell** — does the prop read as a distinct shape when rendered at painter's default `cellPx = 24` (line 56)?
- **Vertical clearance** — tall props (column, archway) must not exceed 4 cell heights in proportion or they clip the painter canvas grid
- **Palette anchor** — reject candidates missing the #00FFCC cyan accent if the object is a light source or portal
- **Transparency edges** — check for halo artifacts on remove-BG result; reject if edge bleed extends more than 2px

Keep 3-4 from 16 wall torch candidates, 2 from 4 column candidates. Final target: 12-14 total across 5 P0 objects.

Antigravity handles visual fidelity in parallel — Sonnet does structural/UX fitness only.

## Q5. Animation workflow for props

**Verdict: n_frames sprite sheet approach, NOT animate_object.**

`animate_object` in PixelLab is documented for character-class objects with defined state model. Non-character props (spike trap, pressure plate, chest open/close, portal pulse) do not map to character state vocabulary cleanly. Attempting to pass a brazier as "character" source will produce undefined or corrupted results.

Use established RIMA pipeline rule: `n_frames` numbered list + `reference_image_base64` from the generated static prop. Request 4-6 frames for fire loop (brazier, torch), 2 frames for mechanical (trap: armed/triggered), 3 frames for portal pulse. This matches the [[feedback-state-vs-n-frames-cost-lock]] hard rule — `create_object_state` is explicitly forbidden as 4-8× more expensive.

## Top 3 recommendations to orchestrator

1. **Split tool by object height-to-width ratio**: Torch + Brazier → `create_1_direction_object` size=64 (16 candidates each). Column + Archway → `create_map_object` (non-square, repeat 2-3 calls). Altar → `create_1_direction_object` size=96 if landscape-leaning, `create_map_object` if square-ish. Do not default all to `create_1_direction_object`.

2. **Bind PropRegistry ScriptableObject to existing painter socket architecture now** (before assets arrive). RoomPainterWindow already has `propSocketType = SocketType.Torch` at line 63 — socket type enum is live. Create registry stub so 12-16 generated prefabs slot directly without painter refactor.

3. **Sonnet handles structural candidate screening, Antigravity handles visual screening** — divide the 41-candidate triage by role.

## Disagreements with Antigravity (anticipated)

- **Aspect ratio waste tolerance**: Antigravity likely accepts the 128×128 column square padding as a credit-efficiency win. This analysis argues quality of the single most perimeter-critical object (32 instances) outweighs candidate count.
- **Style_images priority**: Antigravity may weight prompt-engineering over style_images. This analysis argues both simultaneously, no extra cost.
- **Socket abstraction**: Antigravity may favor Option C (direct object brush) as "cleaner". This analysis favors B on grounds that painter's existing `SocketType` architecture is already built.
