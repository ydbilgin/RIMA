# KIRO TASK — Animation Batch 3
*Updated: 2026-04-05 | Read this file, apply in order. Do not read other files.*

---

## PIXELLAB API

**Endpoint:** `https://api.pixellab.ai/mcp`
**Authorization:** `Bearer 037c442d-d3cf-4f38-83a9-707e05dc62b0`

---

## FOLDER STRUCTURE RULE

All frames saved as:
```
Assets/Sprites/Characters/{Character}/animations/{animation-name}/{direction}/frame_000.png
Assets/Sprites/Characters/{Character}/animations/{animation-name}/{direction}/frame_001.png
...
```

Direction folder names (exact spelling):
`north` · `north-east` · `north-west` · `south` · `south-east` · `south-west` · `east` · `west`

For enemies:
```
Assets/Sprites/Enemies/{EnemyName}/animations/{animation-name}/{direction}/frame_000.png
```

---

## STATUS (read before starting)

| Character | Animation | Status |
|---|---|---|
| Shadowblade | walking-6-frames (8 dir) | ✅ Done in PixelLab — download |
| Shadowblade | running-slide (8 dir) | ⏳ Processing in PixelLab — download when ready |
| Shadowblade | falling-back-death (8 dir) | ❌ Not produced — generate now |
| Shadowblade | running-6-frames (8 dir) | ⚠️ 5/8 dirs exist — delete all, regenerate 8 |
| Shadowblade | lead-jab (8 dir) | ⚠️ 6/8 dirs exist — delete all, regenerate 8 |
| Shadowblade | fight-stance-idle-8-frames (8 dir) | ❓ Check with list_characters |
| Warblade | walking-8-frames — east direction | ❌ Placeholder (366 bytes, empty) — generate |
| Warblade | falling-back-death | ✅ All 8 dirs complete — DO NOT TOUCH |

---

## MANDATORY QC PROTOCOL — APPLY AFTER EVERY DIRECTION

1. After `animate_character` completes, **read frame_000.png** of that direction
2. **Describe what you see**: What is the character doing?
3. **PASS**: The pose matches the expected action (see task-specific criteria below)
4. **FAIL**: Wrong action (e.g. idle pose when walking is expected, standing when death is expected)
5. On **FAIL**: re-generate with a different `animation_name` suffix (add `-v2`, `-v3`) and more specific description
6. On **PASS**: save files, proceed to next direction

---

## PART 1 — SHADOWBLADE

**Character ID:** Run `mcp__pixellab__list_characters()` — find "Shadowblade" and note the ID.

**Style reference:** `Assets/Sprites/Characters/Shadowblade/Shadowblade_S.png`

**Character description for all prompts:**
```
dark fantasy roguelite top-down perspective, low angle (60% top 40% profile view),
shadowy assassin with twin daggers, dark cloak with hood,
rift energy (blue-purple crack light) visible on blades,
high detail pixel art, detailed shading, selective outline,
cold dark palette: deep navy, muted purples, dark grays with faint teal accent
```

### Step 1 — Download completed animations

**walking-6-frames** is complete. Use `get_character` to find the ZIP URL and download.

Save each direction to:
```
Assets/Sprites/Characters/Shadowblade/animations/walking-6-frames/{direction}/frame_000.png
... (6 frames × 8 directions)
```

**running-slide** — if processing is complete, download the same way:
```
Assets/Sprites/Characters/Shadowblade/animations/running-slide/{direction}/frame_000.png
... (8 directions × N frames)
```

---

### Step 2a — falling-back-death (8 directions) — NEW GENERATION

QC pass criteria: **character is visibly falling or stumbling backward, not standing**

```
mcp__pixellab__animate_character(
  character_id=[SHADOWBLADE_ID],
  animation_name="falling-back-death",
  direction="[DIRECTION]",
  n_frames=8,
  action_description="shadowy assassin character staggers and falls backward, collapsing to the ground, death sequence, no loop, final frame shows character lying flat on the ground, dark cloak spread out"
)
```

Directions: `south` · `north` · `west` · `east` · `south-west` · `south-east` · `north-west` · `north-east`

**Apply QC after each direction.**

Save to:
```
Assets/Sprites/Characters/Shadowblade/animations/falling-back-death/{direction}/frame_000.png
... frame_007.png
```

---

### Step 2b — running-6-frames (8 directions) — FULL REGENERATION

> ⚠️ Delete the entire folder first, then regenerate all 8 directions from scratch.

Delete: `Assets/Sprites/Characters/Shadowblade/animations/running-6-frames/`

QC pass criteria: **character is clearly running (leaning forward, legs moving fast)**

```
mcp__pixellab__animate_character(
  character_id=[SHADOWBLADE_ID],
  animation_name="running-6-frames",
  direction="[DIRECTION]",
  n_frames=6,
  action_description="shadowy assassin sprinting fast, body leaning forward, running toward [DIRECTION], dark cloak trailing behind, agile movement"
)
```

Directions: `south` · `north` · `west` · `east` · `south-west` · `south-east` · `north-west` · `north-east`

Save to:
```
Assets/Sprites/Characters/Shadowblade/animations/running-6-frames/{direction}/frame_000.png
... frame_005.png
```

---

## PART 2 — WARBLADE FIXES

**Character ID:** `f3465121-2282-4faa-a955-60b29fd2a628`
**PixelLab size:** 96px

> ⚠️ After all Warblade changes are done, run **RIMA → Build Warblade Animations** in Unity.

---

### 2a — walking-8-frames / east direction (FIX ONLY)

The `east/` direction has 8 placeholder frames (366 bytes each, empty).

> ⚠️ PixelLab may generate all 8 directions at once. If so: only overwrite the `east/` directory. Do NOT touch other directions.

QC pass criteria: **character is walking toward the right/east direction**

```
mcp__pixellab__animate_character(
  character_id="f3465121-2282-4faa-a955-60b29fd2a628",
  animation_name="walking-8-frames-east-fix",
  direction="east",
  n_frames=8,
  action_description="armored warrior walking eastward, moving to the right, greatsword at side, walk cycle animation"
)
```

Save to (overwrite existing):
```
Assets/Sprites/Characters/Warblade/animations/walking-8-frames/east/frame_000.png
... frame_007.png
```

---

## COMPLETION LOG

When all tasks are done, append to `Assets/_STAGING/DONE.txt`:

```
[DONE-BATCH3] Shadowblade/walking-6-frames — downloaded | YYYY-MM-DD
[DONE-BATCH3] Shadowblade/running-slide — downloaded | YYYY-MM-DD
[DONE-BATCH3] Shadowblade/falling-back-death — 8 dirs generated | YYYY-MM-DD
[DONE-BATCH3] Shadowblade/running-6-frames — 8 dirs regenerated | YYYY-MM-DD
[DONE-BATCH3] Warblade/walking-8-frames/east — fixed | YYYY-MM-DD
```

> lead-jab and fight-stance-idle removed from this batch — handled in Aseprite (attack + idle = quality-critical).
> Warblade fight-stance-idle SW removed — handled in Aseprite.

---

## AFTER KIRO FINISHES — Claude Code will handle

When user says "kiro batch 3 bitti" Claude Code will:
1. `AssetDatabase.Refresh()` — import new sprites
2. Run RIMA → Build Warblade Animations
3. Build Shadowblade animator controller (new builder needed — Claude will create)
4. Health scan: all mob SR colors white, no null sprites
5. Screenshot in play mode
6. Update CURRENT_STATUS
