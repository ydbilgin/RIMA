# PixelLab Anim Locked V2 (RIMA)

Date: 2026-04-22
Status: Locked by Claude decision (no mirror)

## 1) Production Baseline (Locked)
- Sprite size: `128x128`
- `Animate with text NEW`: up to `16` frames at 128px (user-confirmed)
- `Interpolate NEW`: up to `16` frames
- No mirror: every direction is generated independently

## 1b) Tool Selection Rule (Locked — 2026-04-22)

| Animasyon Tipi | Araç | Gerekçe |
|----------------|------|---------|
| **Run, Idle** | Animate with text NEW — direkt | Basit döngü, tek gen/yön yeterli. 8 gen toplam. |
| **Attack, Skill (multi-phase)** | KF + Interpolate | Impact/peak frame kritik — el ile belirlenmeli. Windup→Impact→Recovery her segment ayrı. |
| **Single-phase skill** (windup/recovery yok) | Animate with text NEW — direkt | Frame seviyesi kontrol gerekmez; Animate yeterli. |

**Fallback kuralı:** Animate with text NEW çıktısı kabul edilemezse (kılıç kayboluyor, hilt tutarsız) → KF + Interpolate'e geç (4 gen/yön).

**KF + Interpolate ne zaman zorunlu:**
- Bir skillde birden fazla faz var (yüklenme → patlama → geri çekilme)
- Impact frame'in tam pozisyonu gameplay için kritik (hitbox zamanlaması)
- Zincirleme hareket: son frame bir sonraki segmentin first frame'i olacak

## 2) Transcript Findings Used For This Lock
Sources re-fetched on 2026-04-22:
- `8TRHAC3fUpo` (walk + direction workflows)
- `1CjxHZoZE_I` (Interpolate NEW)
- `zghUW8fGqsM` (Animate with text NEW / v3 workflow)
- `XdgK1KeN-3s` (frame-limit behavior + chaining)
- `8jt0f-9wHRQ` (8-direction rotate workflow)

Q1 direction handling without mirror:
- Common public workflow uses mirror for opposite sides (`8TRHAC3fUpo`).
- Rotate workflow warns that some directions are harder and need separate retries (`8jt0f-9wHRQ`, NE called out as harder).
- Project lock overrides mirror usage: all 8 directions are produced directly.

Q2 keyframe definition:
- Walk/run examples repeatedly use template/key-pose structure and iterative cleanup (`8TRHAC3fUpo`).
- Interpolate NEW always starts from explicit first/last frame pairs (`1CjxHZoZE_I`).
- Animate NEW chaining uses "last frame -> next reference" to build longer actions (`zghUW8fGqsM`, `XdgK1KeN-3s`).

Q3 frame guidance:
- 128px production baseline in this project is 16/16 for Animate and Interpolate.
- Tool behavior can vary by canvas/account; keep slider truth as final runtime source.

Q4 hard directions and workaround:
- NE is explicitly called harder in rotate workflow (`8jt0f-9wHRQ`).
- Workaround pattern: generate direct, fix key silhouette, then re-run from corrected init/reference frame.

Q5 Animate vs Interpolate split:
- Animate for main action block generation.
- Interpolate for bridge density and loop stitching.

Q6 direction-specific prompt changes:
- Direction wording is used in examples (camera/direction fields in workflow videos).
- For lock: keep state prompt constant, change only direction line + sword path line.

## 3) Tool Usage Rules — Attack/Skill Only (Locked)
Run ve Idle için bu kurallar geçerli değil — Section 1b'ye bak (Animate with text NEW direkt).

1. Build key poses first (`A`, `B`, and when needed `C`).
2. Generate each key pose with Animate NEW (`6-12` frames). Keep only the target contact/peak frame.
3. Bridge between poses with Interpolate NEW (`4-8` frames each bridge, max 16).
4. If motion is short, keep only best frames; do not keep padded noisy tails.
5. When quality drops, use newest clean frame as next reference and continue.

## 4) Production Order (Locked)
Use this exact direction order for every state:
1. `SE`
2. `E`
3. `S`
4. `NE`
5. `N`
6. `SW`
7. `W`
8. `NW`

## 5) Warblade Constraint Block (Paste Verbatim In Every Animate Prompt)
- `both hands on same long hilt`
- `right hand near crossguard, left hand near pommel`
- `heavy wide blade, low readable silhouette`

## 6) Per-Direction Keyframe Table

### 6.1 Idle
| Dir | Keyframes | Pose A | Pose B | Animate budget | Interpolate budget | Difficulty / workaround |
|---|---:|---|---|---:|---:|---|
| SE | 2 | Guard angled to lower-right | Weight shift to back foot, cape lag | 6 | 6 | Low |
| E | 2 | Side-on torso, blade trailing right | Micro torso dip, blade tip wobble | 6 | 6 | Low |
| S | 2 | Neutral guard, blade low center | Breath + slight shoulder rise | 6 | 6 (`A->B->A`) | Low |
| NE | 2 | Torso turned away, blade high rear-right | Return to low guard with clear hand spacing | 8 | 6 | High: keep hand spacing readable first |
| N | 2 | Back-facing guard, blade visible on one side | Shoulder roll + hilt tilt | 8 | 6 | Medium: avoid sword hiding behind body |
| SW | 2 | Guard angled to lower-left | Weight return to front foot | 6 | 6 | Low |
| W | 2 | Opposite-side silhouette, generated direct (no mirror) | Small hip sway, blade drag | 6 | 6 | Low |
| NW | 2 | Torso away-left, blade high rear-left | Settle to low left guard | 8 | 6 | Medium: hilt readability |

### 6.2 Run — Animate with text NEW (direkt, 1 gen/yön)
Tool: Animate with text NEW. KF gerekmez. Yön promptları için WARBLADE_RUN_GUIDE.md kullan.

| Dir | Motion description | Difficulty |
|---|---|---|
| SE | Right foot lead, diagonal lean lower-right, blade trailing right | Low |
| E | Right foot lead, strong side lean, blade behind hip line | Low |
| S | Right foot lead, slight forward lean, sword low center drag | Low |
| NE | Left foot lead, torso twist away, blade high-to-low transition | High — retry if hand spacing lost |
| N | Left foot lead, back-facing shoulder pump, blade offset side | High — retry if blade merges into spine |
| SW | Right foot lead, diagonal lean lower-left, blade trailing left | Low |
| W | Left foot lead, side lean, blade behind left hip | Low |
| NW | Left foot lead, diagonal lean upper-left, blade trailing left | High — retry if hand spacing lost |

### 6.3 Attack (Warblade 2H)
Segments: `windup (A) -> impact (B) -> recovery (C)`.

| Dir | Keyframes | Windup | Impact | Recovery | Animate budget | Interpolate budget | Difficulty / workaround |
|---|---:|---|---|---|---:|---:|---|
| SE | 3 | Blade loaded rear-right high | Diagonal sweep right-to-left across front | Settle to right-low guard | 10 | 6 | Low |
| E | 3 | Full side coil, sword behind torso line | Horizontal slash toward right edge | Recover with tip down trailing | 10 | 6 | Medium: avoid arm stretch artifacts |
| S | 3 | Blade high over right shoulder | Downward front chop centerline | Return low center guard | 10 | 6 | Medium: keep blade width consistent |
| NE | 3 | High rear-right coil with torso turn-away | Sweep bottom-left to upper-right (camera-relative) | Re-center with visible two-hand grip | 12 | 6 | High: regenerate from corrected windup key |
| N | 3 | Overhead rear load (back view) | Arc over top with visible blade edge offset | Recover to back-guard side offset | 12 | 6 | High: ensure sword not merged into torso |
| SW | 3 | Blade loaded rear-left high | Diagonal sweep left-to-right across front | Settle to left-low guard | 10 | 6 | Low |
| W | 3 | Side coil opposite E, generated direct (no mirror) | Horizontal slash toward left edge | Recover tip down trailing left | 10 | 6 | Medium |
| NW | 3 | High rear-left coil | Sweep bottom-right to upper-left | Recover left-side guard | 12 | 6 | Medium-high: fix silhouette before bridging |

## 7) Prompt Templates With Direction Variations
Use one state template plus direction line. Keep prompts short and structural.

### 7.1 Idle template
`warblade heavy knight idle breathing loop, [DIR_LINE], both hands on same long hilt, right hand near crossguard, left hand near pommel, heavy wide blade, low readable silhouette, pixel art sprite`

### 7.2 Run template
`warblade heavy knight running loop, [DIR_LINE], [RUN_CARRY_LINE], both hands on same long hilt, right hand near crossguard, left hand near pommel, heavy wide blade, low readable silhouette, pixel art sprite`

### 7.3 Attack template
`warblade heavy knight attack combo, [DIR_LINE], [ATTACK_PATH_LINE], both hands on same long hilt, right hand near crossguard, left hand near pommel, heavy wide blade, low readable silhouette, pixel art sprite`

### 7.4 Direction line map
- `S`: `facing south, camera low top-down`
- `SE`: `facing southeast, camera low top-down`
- `E`: `facing east, camera low top-down`
- `NE`: `facing northeast, camera low top-down`
- `N`: `facing north, camera low top-down`
- `NW`: `facing northwest, camera low top-down`
- `W`: `facing west, camera low top-down`
- `SW`: `facing southwest, camera low top-down`

### 7.5 Run carry line map
- `S/SE/E`: `sword carried low with trailing momentum on right side`
- `NE/N/NW`: `sword carried high-to-low transition with clear two-hand spacing`
- `W/SW`: `sword carried low with trailing momentum on left side`

### 7.6 Attack path line map
- `S`: `impact path top-to-bottom center`
- `SE`: `impact path upper-right to lower-left`
- `E`: `impact path back-to-front horizontal to the right`
- `NE`: `impact path bottom-left to upper-right`
- `N`: `impact path overhead rear-to-front arc`
- `NW`: `impact path bottom-right to upper-left`
- `W`: `impact path back-to-front horizontal to the left`
- `SW`: `impact path upper-left to lower-right`

## 8) QC Checklist Per Direction
Apply this checklist before accepting each exported direction clip.

- `SE`: baseline anchor. Confirm this first before continuing sequence.
- `E`: verify side silhouette does not thin out blade width.
- `S`: verify both hands stay separated on hilt, no hand merge.
- `NE`: verify torso turn-away still keeps sword readable; reject if hilt disappears.
- `N`: verify back-view arc still shows blade edge offset from torso.
- `SW`: verify direct generation (not mirrored artifacts in asymmetry details).
- `W`: verify direct generation keeps left-side carry consistent across frames.
- `NW`: verify high-risk occlusion frames; patch keyframe then re-bridge.

Global checks for all directions:
- No mirror artifacts
- No frame-to-frame scale pumping
- No dead/black frames
- Weapon grip continuity holds on every frame
- Run->idle bridge connects without pop

## 9) No-Mirror Rule (Explicit)
Mirror is disabled for this production line.

Rationale:
- Mirror hides asymmetry bugs until integration (cape split, shoulder detail, hand spacing).
- Warblade two-hand grip fidelity is direction-sensitive and breaks under flipped assumptions.
- Re-fetch findings show some directions are intrinsically harder; direct generation + per-direction correction is more stable than mirror-then-patch for final game quality.
