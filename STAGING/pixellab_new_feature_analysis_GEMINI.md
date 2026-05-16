# PixelLab Character States — Feature Analysis
**Source:** https://youtu.be/oCJWxfEwX-o  
**Video Title:** "PixelLab Character States: The New Way to Animate Sprites"  
**Published:** 2026-05-16 | Duration: 11:15  
**Analysis Date:** 2026-05-16  
**Research Method:** Gemini CLI video analysis (default model) + yt-dlp metadata fallback  

---

## 1. Feature Name and Summary

**Feature:** Character States (integrated into PixelLab Web UI V3)

Character States is a new workflow layer inside the PixelLab V3 character creator that lets you define a specific pose as a fixed "state" before generating animations. Instead of having the AI invent an animation from a neutral default, you first generate a static pose frame (the "state"), then use that as the locked first frame for any animation. This produces cleaner, more consistent animation sequences anchored to a user-defined starting position.

---

## 2. Workflow Steps Demonstrated

1. Go to Characters page, generate or select an existing base character (V3 mode, camera angle, size).
2. Click "Create State" on the character card.
3. Enter a text prompt for the pose (e.g., "fighting pose", "mid-walk", "laying down"). Built-in pose templates are also available.
4. Generate the static state frame — a single image of the character in that pose, preserving original identity.
5. From the state, click "Add your first animation."
6. Choose "Custom Animation V3," enter an animation prompt (e.g., "walk loop"), and set frame count.
7. Keep the "first frame" toggle enabled — the state becomes the definitive anchor frame.
8. Generate the animation sequence.
9. Optional: open in Pixelorama (browser-based pixel editor) for manual cleanup of minor artifacts.
10. Repeat per direction needed; mirror symmetrical directions rather than re-generating them.

---

## 3. Inputs Required

- Base character sprite (generated or existing)
- Text prompt for the pose (state definition)
- Text prompt for the animation action
- Frame count setting
- Optional: a second frame for interpolation (start → end transition workflow)

---

## 4. Outputs Produced

- Single static frame (the State) — preserves proportions and identity
- Multi-frame animation sequence (demonstrated: 7–8 frames)
- Directional animations per direction (SE, E, NE demonstrated; W directions mirrored)
- Custom canvas size supported — demo used 32×44 inside a square canvas, proving tight pixel-art dimensions work natively

---

## 5. Credit Cost

Not mentioned in the video.

---

## 6. Fit with RIMA's Locked Pipeline

| RIMA Pipeline Lock | Compatibility | Notes |
|---|---|---|
| Create Image Pro (Karar #100) | AUGMENTS | Character States builds on top of an existing base character, not a replacement for the initial generation step |
| Reference image style strength | COMPATIBLE | Base character identity is established first via reference; states inherit that identity |
| 8-direction with mirror (5 produce + 3 mirror) | DIRECTLY ENABLES | Video explicitly demonstrates SE/E/NE production + W mirroring. Matches RIMA's 5+3 split exactly |
| Weapon-less production (Karar #144) | STRONGLY RECOMMENDED | Video warns asymmetrical designs break mirroring; weapon-free bodies are ideal — weapons added in Unity as child renderers |
| Idle + Run animation (Karar #42) | DIRECTLY SERVES | Both idle and run loops can be defined as separate states, then animated cleanly from a locked first frame |
| Keyframe + Interpolation (Karar #47) | DIRECTLY SERVES | State-to-state interpolation is explicitly shown as an input mode (start frame → end frame → generate transition) |

---

## 7. Replaces or Augments?

**Augments** — this is an additional workflow layer inserted between base character generation and animation. It does not replace Create Image Pro, Create Character, or the existing animate_character/create_character_state MCP endpoints. It is the visual UI realization of the `create_character_state` backend concept, now exposed as a first-class V3 workflow rather than a raw API call.

The practical implication: Character States are not available as a new MCP endpoint (as of this video). The workflow happens in the Web UI V3, consistent with the existing RIMA decision that character production runs in the web UI (Char→Web UI V3 LOCK).

---

## 8. Speed Comparison

The video demonstrates that a full 8-direction walk cycle can be completed in under 5 minutes using the state + animation + mirror approach. No explicit benchmark against the old workflow is given, but the speed gain comes from:
- Eliminating failed animation attempts caused by the AI picking a random starting pose
- Mirroring 3 of 8 directions rather than generating them

---

## 9. Quality at 64×64 / Chibi Resolution

Yes — demonstrated at 32×44 effective pixel dimensions inside a square canvas. The feature explicitly supports tight, custom-bounded pixel art. For RIMA's 64×64 sprites (or the locked 64×64 native size), this should work without modification. The small canvas sizing advice (use a tight bounding box, not a square) applies directly to our pipeline.

---

## 10. Limitations and Caveats

- **Artifacts are expected:** Minor hallucinations occur (e.g., mouth opening during walk, extra environmental elements). Manual Pixelorama cleanup is shown as part of the normal workflow, not an edge case.
- **Some directions require rerolls:** South-facing walks in particular can generate janky or too-fast results and need a complete reroll (can't fix in Pixelorama).
- **Symmetry dependency for mirroring:** If a character has asymmetrical design elements (shoulder armor, holster, weapon), mirroring breaks. RIMA's weapon-less production (Karar #144) already eliminates this risk for the body — weapons are attached in Unity.
- **Web UI only (as of this video):** No indication of a new MCP endpoint for States; the workflow is UI-driven, consistent with the Char→Web UI V3 LOCK.
- **Credit cost unknown:** Video does not disclose credits per generation.

---

## Top 3 Takeaways for RIMA

### INTEGRATE immediately
**Character States for all 10 class animations.** The state-anchored animation workflow directly solves the biggest risk in our batch production: AI picking an inconsistent starting pose that drifts across frames. For each class, generate a "neutral idle state" first, then animate from it. This applies to idle (Karar #42) and run loops identically.

### INTEGRATE immediately  
**State-first for direction sets.** Generate S, SW, W, NW, N (the 5 produce directions) as separate states, then animate each. Mirror NE from NW, E from W, SE from SW. The video's demonstrated workflow matches our 5+3 mirror plan exactly — Character States makes this faster and more reliable.

### DEFER (monitor for MCP endpoint)
**Interpolation between states for skill animations.** The state-to-state interpolation mode (Karar #47 KF+Interp) is shown in the video as a UI workflow. It is not yet confirmed as a new MCP endpoint. Watch PixelLab Discord for API/MCP update. When it ships as an endpoint, it will directly serve our skill animation phase. Do not block current batch production on this.

---

*Model used: Gemini CLI default (settings.json pinned model). Video access via direct YouTube URL processing. Metadata extracted via yt-dlp as supplement.*
