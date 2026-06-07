# Task: Analyze a YouTube Short (VISUAL + AUDIO) on weapon-to-hand attachment for a 2D action game

ACTIVE RULES: (1) think before answering (2) be concrete + timestamped (3) only claim what you actually observed in the video; if you cannot access the video frames/audio, SAY SO explicitly and state exactly what you fell back to (4) BLOCKED if you cannot watch it at all.
NLM ACCESS: N/A for this task — do not query NLM. Direct-read only this file.
RESPOND INLINE in your final answer (captured into AGY_DONE_*.md). Do NOT write to scratch/external files.

## You are Gemini — USE YOUR NATIVE YOUTUBE VIDEO UNDERSTANDING
Analyze this YouTube Short as a VIDEO (frames + audio track), NOT just the page text:
**https://www.youtube.com/shorts/DXpEIk1EsEU**
If your runtime supports passing a YouTube URL as video input, do that. Watch the whole short. Listen to the audio.

## Project context (so your takeaways are relevant)
RIMA = 2D **top-down chibi pixel-art** action-roguelite (Hades/Children-of-Morta vibe). URP 2D, PPU 64, ~64px characters, **8 directions** (5 sprites + 3 mirrored). Current LOCKED weapon approach: **weaponless character body + a separate weapon SPRITE mounted on a "HandAnchor" child + an OrientationSync script** that rotates/flips the weapon per facing-direction; during an attack the weapon is HIDDEN and replaced by a painterly slash-arc VFX flipbook.
**The user's specific question:** the video attaches the weapon to the hand *directly*. But OUR characters will likely have **actual visible hands drawn in the sprite**. So a directly-attached separate weapon sprite may NOT line up with the drawn hand across all 8 directions / animation frames. We need to know what's transferable and what breaks.

## What to extract (structured, timestamped where possible)

### A. VISUAL — how is the weapon attached to the hand?
1. Is it a 3D rig (bone/IK on a 3D hand) or a 2D sprite pinned to a point? 2D skeletal (Spine/DragonBones) or frame-by-frame?
2. Does the character have **visible hands**? If yes, how does the weapon align to the hand — single pivot point, parent bone, per-frame redraw?
3. How does the weapon follow the hand during **idle, walk, and attack/swing**? Is the weapon a static sprite that rotates, or animated separately?
4. Directions: is it top-down/8-dir, side-view, or 3/4? How is facing handled (flip, rotate, redraw)? Does the hand+weapon stay aligned when facing changes?
5. The SWING: is the weapon sprite itself animated through the arc, or hidden/replaced by a VFX trail/slash? Any motion smear, afterimage, or trail?
6. Art style + resolution: pixel vs HD vs vector. Pivot/anchor visible? Any tells about the tooling (Unity, Spine, Godot, etc.)?

### B. AUDIO — what is the sound doing?
1. SFX on swing vs on hit (whoosh vs impact) — layered? pitch? timing relative to the visual contact frame?
2. Is there hit-stop / a beat of silence on impact? Music ducking? Any audio "juice" that sells the hit?
3. Voice/commentary? Does the creator explain the technique (transcribe key lines)?

### C. TAKEAWAYS for RIMA (the point of this)
1. Given the **visible-hands** concern: does the video's method even apply to a character with drawn hands? What would line up vs misalign across 8 directions?
2. 3 concrete things RIMA could ADOPT from this video, and 2 it should AVOID/reject given our top-down 8-dir pixel constraints.
3. Your verdict: for a character with visible hands + 8-dir top-down, is a **HandAnchor-pinned separate weapon sprite** viable, or does the video suggest a different approach (e.g. weapon baked into per-direction frames, or hand+weapon drawn together, or weapon-hidden-during-swing + VFX)?

## Deliverable
A tight, structured inline report (A / B / C). Lead with whether you could actually watch the video. Timestamp observations. End with the C verdict in 3-5 bullet points.
