# PixelLab Tier2 NEW Tools + 8 Direction Production Map

Date: 2026-04-22
Project: RIMA

## 1) Source Reliability Order
- Tier A (highest): Live UI evidence from your screenshots (`Screenshot_26`, `Screenshot_27`).
- Tier B: Official PixelLab YouTube transcripts (2025-2026 uploads).
- Tier C: Local docs (`PIXELLAB_PIPELINE.md`, `ASEPRITE_EXTENSION.md`, `PIXELLAB_API_V2.md`) because some limits are clearly stale.

## 2) What Is Confirmed Right Now
### 2.1 Animate with text NEW (live UI)
- `Screenshot_26`: Reference image is `220x220`.
- `Screenshot_26`: Frame slider is `4..10` (max 10 at this size).
- Conclusion: At least for your current Tier2 UI/account state, `220x220 => max 10` in Animate with text NEW.

### 2.2 Interpolate NEW (live UI)
- `Screenshot_27`: Frame slider is `4..16`.
- Tool uses `First frame` + `Last frame` + `Animation action`.
- Conclusion: Interpolate NEW currently gives a wider frame budget than Animate with text NEW in your observed setup.

### 2.3 Enhance with AI behavior (video-backed)
- Interpolate NEW video explicitly shows prompt enhancer in Creator (`1CjxHZoZE_I`, lines 23-24, 88-89, 113-115).
- It uses image context when rewriting action text (same transcript lines).
- Practical meaning: Keep base prompt short and structural; use Enhance with AI to expand wording, then manually trim if it over-stylizes.

## 3) Document vs Reality (Important Conflicts)
- Local pipeline says `128x128 => 4 frames` for Animate with text NEW (`PIXELLAB_PIPELINE.md`, lines 214-217).
- Official newer videos claim higher limits in some cases:
  - `XdgK1KeN-3s` says 64 gives 16 and 128 drops to 4 (lines 35-38, 69-71).
  - `zghUW8fGqsM` demonstrates 128 canvas with 16 frames in one case (lines 123-127).
  - `1CjxHZoZE_I` mentions 128 may do 16 and 256 around 8 (lines 60-67).
- Decision rule for production:
  - Always trust current live slider limits in your UI first.
  - Treat local docs as fallback guidance, not absolute caps.

## 4) Tier2 Production Strategy (RIMA-safe)
### 4.1 Core rule
- Use Animate with text NEW for key motion chunks.
- Use Interpolate NEW for frame expansion/smoothing.
- Prefer segmented chains over one long generation:
  - `A -> B`
  - `B -> C`
  - optional `C -> D`

### 4.2 Recommended chain
1. Create/choose clean keyframes (A, B, C).
2. Interpolate `A->B` with concise action text.
3. Interpolate `B->C` with concise action text.
4. Remove redundant hold frames.
5. Normalize timing in editor.

### 4.3 Prompt format (short and stable)
- Pattern: `identity + one action + hard constraints`.
- Keep one action per generation.
- Avoid camera text if camera is already set externally.
- Example style:
  - `male armored warrior shifts weight forward, both hands stay on same long sword hilt, sword remains heavy and low`

## 5) 8-Direction Animation: How People Actually Do It
Based on official tutorials, common workflow is:

### 5.1 Fast route (Character page)
- Generate one good direction first (often SE).
- Use bulk generation for remaining directions.
- Use mirror for opposite side when character is symmetrical enough (`8TRHAC3fUpo`, lines 77-87).
- Then manual cleanup for asymmetrical details (hair strap/sleeve/etc., lines 90-101).

### 5.2 Controlled route (Animate with Skeleton)
- Set camera view + reference direction.
- Insert walk/run template.
- Generate in small increments (often 2 frames at a time).
- Freeze context frame(s) to stabilize motion (`8TRHAC3fUpo`, lines 148-159).
- Clean each step before continuing, because accepted frames become next context (lines 186-190).

### 5.3 Motion transfer route (Animation-to-animation)
- Take existing clean walk template.
- Transfer motion structure onto new character.
- Use reference image for identity retention (`8TRHAC3fUpo`, lines 195-249).

### 5.4 Legacy rotate route (still useful fallback)
- Generate directions one by one.
- Retry difficult directions (especially NE in old workflow).
- Use rough init image to guide hard directions (`8jt0f-9wHRQ`, lines 30-38, 42-49).

## 6) RIMA 8-Direction Plan (Concrete)
For each state (`idle`, `run`, `attack`):
1. Produce high-quality master directions first: `S, SE, E, NE, N`.
2. Fill opposite directions using mirror where valid: `SW, W, NW`.
3. For mirrored directions, patch asymmetry manually (cape straps, shoulder marks, hair split).
4. Build animation per direction using segment interpolation:
   - idle micro-loop: same-pose loop or `A->B->A`
   - run: `contact -> passing -> contact` chain
   - run->idle transition: short dedicated segment, not full regenerate
5. QC checklist per direction:
   - silhouette stable
   - feet contact readable
   - weapon hand contact continuity
   - no accidental body scale drift

## 7) Warblade-Specific Guidance (Current Problem: heavy 2H sword)
- Keep sword mass readable by anchoring constraints in every segment:
  - `both hands on same long hilt`
  - `right hand near guard, left near pommel`
  - `blade stays heavy, low, and continuous`
- For run:
  - prefer drag/readable momentum pose in keyframes first
  - interpolate between those keys rather than asking one-shot full run
- For run->idle:
  - use short bridge segment (`run_end -> idle_start`) with Interpolate NEW
  - avoid regenerating full idle from run frame every time

## 8) Practical Limits to Use Immediately
- If your UI currently shows:
  - Animate with text NEW: `max 10` at 220x220
  - Interpolate NEW: `max 16`
- Then production default should be:
  - Animate with text NEW for structure (6-10)
  - Interpolate NEW for densifying and smoothing (8-16)

## 9) Key References
- Screenshots:
  - `C:\Users\ydbil\OneDrive\Belgeler\Lightshot\Screenshot_26.png`
  - `C:\Users\ydbil\OneDrive\Belgeler\Lightshot\Screenshot_27.png`
- Official videos:
  - `https://www.youtube.com/watch?v=1CjxHZoZE_I`
  - `https://www.youtube.com/watch?v=zghUW8fGqsM`
  - `https://www.youtube.com/watch?v=8TRHAC3fUpo`
  - `https://www.youtube.com/watch?v=XdgK1KeN-3s`
  - `https://www.youtube.com/watch?v=8jt0f-9wHRQ`
  - `https://www.youtube.com/watch?v=lPC5-b7ToPU`
- Local docs:
  - `F:\Antigravity Projeler\Pixellab\PIXELLAB_API_V2.md`
  - `F:\Antigravity Projeler\Pixellab\PIXELLAB_PIPELINE.md`
  - `F:\Antigravity Projeler\Pixellab\ASEPRITE_EXTENSION.md`
  - `F:\Antigravity Projeler\2d roguelite\RIMA\GUIDES\PIXELLAB_ANIM_QUICKREF.md`
