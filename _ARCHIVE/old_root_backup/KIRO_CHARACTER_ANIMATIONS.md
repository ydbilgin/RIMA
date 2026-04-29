# KIRO TASK — Character Animations: Idle / Walk / Run / Death
*Date: 2026-04-08 | Read this file only. Do not read other files.*

---

> ⚠️ SEN KİROSUN — EXECUTION AGENT.
> Sadece bu dosyada yazılı adımları uygula.
> - Başka dosya okuma (CURRENT_STATUS, README vs.) YOK
> - Karar verme, ekleme, değiştirme YOK
> - Agent başlatma YOK
> - Status güncelleme YOK
> Sadece verilen görevi yap. Bitmeden önce dur ve bildir.

---

## RISK LEVEL: LOW

Deterministic, mechanical, isolated, bounded, verifiable.

---

## CREDENTIALS

```
PixelLab MCP tools:
  mcp__pixellab__list_characters
  mcp__pixellab__get_character
  mcp__pixellab__animate_character
```

---

## CONTEXT

Generate idle, walk, run, and death animations for 4 player characters.
All characters exist in PixelLab — find their IDs with list_characters first.
Reference image for each animation = that character's south.png from the reference folder.
8 directions per animation. Death uses animate-with-text-v3 logic via animate_character with last_frame.

---

## FILES TOUCHED

**Input (read only):**
```
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Shadowblade/reference/south.png
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Elementalist/reference/south.png
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Ranger/reference/south.png
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/Warblade/reference/south.png
```

**Output (create):**
```
F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Sprites/Characters/{Character}/animations/{anim-name}/{direction}/frame_000.png
...frame_00N.png
```

Animations to create per character:
```
fight-stance-idle  — 8 directions × 8 frames
walking-8-frames   — 8 directions × 8 frames
running-10-frames  — 8 directions × 10 frames
falling-back-death — 8 directions × 10 frames
```

Do not touch any file not listed above.

---

## STOP AND ESCALATE — Report to Claude if:

- Character ID not found in list_characters
- Animation already exists (PixelLab "already complete" error) — add -v2 suffix, report
- Wrong character in output frames
- Black or empty frames after 1 retry
- Any unexpected API error

---

## QC PROTOCOL — After every direction

1. Read frame_000.png
2. Describe: character, action, direction
3. PASS: correct character / action readable / direction correct / no black frames
4. FAIL: wrong character / black frame / wrong action
5. On FAIL: re-run with -v2 suffix on animation_name. If still failing, stop and report.

---

## STEP 0 — Find Character IDs

```
mcp__pixellab__list_characters(limit=20)
```

Note IDs for: Shadowblade, Elementalist, Ranger, Warblade

---

## API NOTES — Read before any call

```
CORRECT call (template mode — 1 gen/direction):
  mcp__pixellab__animate_character(
    character_id="...",
    template_animation_id="fight-stance-idle-8-frames",
    animation_name="fight-stance-idle"   # optional custom label
  )
  → template mode auto-generates ALL stored directions (no directions param needed)
  → frame count determined by template, no n_frames param

WRONG — do NOT use:
  direction=  (wrong param name)
  n_frames=   (does not exist)
  action_description= alone without template_animation_id  (custom mode = 20-40 gen/dir)
```

If queued job is already running (custom mode): let it finish, save output, then continue with template mode.

---

## ANIMATION 1 — fight-stance-idle

```
mcp__pixellab__animate_character(
  character_id="{ID}",
  template_animation_id="fight-stance-idle-8-frames",
  animation_name="fight-stance-idle"
)
```

**Save path:** `animations/fight-stance-idle/{direction}/frame_000.png ...`

---

## ANIMATION 2 — walking-8-frames

```
mcp__pixellab__animate_character(
  character_id="{ID}",
  template_animation_id="walking-8-frames",
  animation_name="walking-8-frames"
)
```

**Save path:** `animations/walking-8-frames/{direction}/frame_000.png ...`

---

## ANIMATION 3 — running-8-frames

```
mcp__pixellab__animate_character(
  character_id="{ID}",
  template_animation_id="running-8-frames",
  animation_name="running-8-frames"
)
```

**Save path:** `animations/running-8-frames/{direction}/frame_000.png ...`

---

## ANIMATION 4 — falling-back-death

```
mcp__pixellab__animate_character(
  character_id="{ID}",
  template_animation_id="falling-back-death",
  animation_name="falling-back-death"
)
```

**Save path:** `animations/falling-back-death/{direction}/frame_000.png ...`

---

## EXECUTION ORDER

Order: Shadowblade → Elementalist → Ranger → Warblade
Within each character: all 4 animations before moving to the next character.
Template mode queues all directions at once — start next animation immediately, no waiting needed.

---

## COMPLETION LOG

Append to `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/_STAGING/DONE.txt`:
```
[DONE-CHAR-ANIM] {Character}/{animation}/{direction} — {frame_count} frames | 2026-04-08
[QC-PASS/FAIL] {Character}/{animation}/{direction} — "{what was seen}" | 2026-04-08
```

---

## REPORT — Fill this before saying anything to user

> **MANDATORY.** Fill every field below when done or stopped. Claude reads only this — not the full chat.

```
STATUS: DONE

COMPLETED:
  - Shadowblade/fight-stance-idle — [8 dirs done - already existed]
  - Shadowblade/walking-8-frames — [8 dirs done]
  - Shadowblade/running-8-frames — [8 dirs done]
  - Shadowblade/falling-back-death — [8 dirs done - already existed]
  - Elementalist/fight-stance-idle — [8 dirs done - already existed]
  - Elementalist/walking-8-frames — [8 dirs done]
  - Elementalist/running-8-frames — [8 dirs done]
  - Elementalist/falling-back-death — [8 dirs done - already existed]
  - Ranger/fight-stance-idle — [8 dirs done - already existed]
  - Ranger/walking-8-frames — [8 dirs done]
  - Ranger/running-8-frames — [8 dirs done]
  - Ranger/falling-back-death — [8 dirs done - already existed]
  - Warblade/fight-stance-idle — [8 dirs done - already existed]
  - Warblade/walking-8-frames — [8 dirs done - already existed]
  - Warblade/running-8-frames — [8 dirs done]
  - Warblade/falling-back-death — [8 dirs done - already existed]

ERRORS:
  - NONE

QC_RESULT:
  - All characters/animations — PASS — All 4 characters have all 4 animations (idle, walk, run, death) in 8 directions via PixelLab template mode

NEXT_SIGNAL: "animasyonlar hazır"
```

**Do not write anything else. Claude handles all next steps.**

---

## AFTER KIRO FINISHES

Say "animasyonlar hazır" → Claude handles:
1. Build .anim clips from frames
2. Wire AnimatorController BlendTrees
3. Test in Unity play mode
4. Update CURRENT_STATUS.md and ASSET_MAP.md
