ACTIVE RULES: (1) think carefully (2) score honestly (3) cite specific files/pixels (4) BLOCKED if Stream F output missing.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

**ANTIGRAVITY CRITICAL:** Respond INLINE only. Do NOT use file-write tools. ConPTY captures stdout.

Amaç: Stream F (Real Asset Visual Swap + Camera/Floor/Lighting) çıktısını VISION REVIEW yap. User feedback: "chatgpt_ref teki gibi görseli yakalayamamışız o açıda o derinlikte". Sen Gemini vision modelisin, screenshot'ları gerçekten GÖREBİLİYORSUN. Her 5 odanın `scene_v2.png` versiyonunu `chatgpt_ref` hedefleriyle karşılaştır, brutally honest score ver. User'a "bir tane gerçek oda" lazım — hangi oda en iyi proof?

---

# STREAM F VISION REVIEW — chatgpt_ref Likeness Scoring

## Phase 0 — INTERNALIZE chatgpt_ref INTENT (MANDATORY FIRST)

Before reading ANY Stream F output, you MUST first deeply study the chatgpt_ref target visuals and form an internal model of what the user wants.

### Open and STUDY these chatgpt_ref images carefully:
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (1).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (2).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (3).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (4).png`
- `STAGING/concepts/chatgpt_ref/new_chatgpt/ChatGPT Image 23 May 2026 21_11_22.png` (additional reference)
- `STAGING/concepts/chatgpt_ref/blueprint_room/*.png` (5 methodology PNGs)

### Describe in YOUR OWN WORDS (300-500 words) — this MUST appear at the TOP of your response:

1. **Perspective & depth:** What angle is the camera at? How does the depth illusion work? Where do tall walls "lean" toward the viewer?
2. **Mood & atmosphere:** Color palette? Lighting source(s)? What feels "dungeon" about it?
3. **Structural elements:** Which walls dominate? Where are doors? What setpieces? How do floor and walls relate?
4. **Visual language consistency:** What's the SAME across all reference images? (e.g., always cyan rifts, always torch glow, always 3/4 angle)
5. **Pixel details:** Sprite scale? Brick texture density? Where are seams hidden?

### State the "north star":
In 1-2 sentences, what is the user trying to achieve?

**You CANNOT proceed to scoring until you have written Phase 0. Skipping it = invalid review.**

---

## Wait condition

Before starting (after Phase 0), verify Stream F is DONE:
- Check `CODEX_DONE_<profile>.md` for "STREAM F" entry with STATUS: DONE
- Check `STAGING/s106_overnight/stream_e_rooms/<room>/scene_v2.png` exists for at least 1 room
- If not present yet, report WAITING and stop. Orchestrator will retry.

## Files to read

### Stream F outputs (the new renders)
- `STAGING/s106_overnight/stream_e_rooms/combat_basic/scene_v2.png`
- `STAGING/s106_overnight/stream_e_rooms/ritual_diamond/scene_v2.png`
- `STAGING/s106_overnight/stream_e_rooms/flooded_crypt/scene_v2.png`
- `STAGING/s106_overnight/stream_e_rooms/library_alcove/scene_v2.png`
- `STAGING/s106_overnight/stream_e_rooms/boss_arena/scene_v2.png`

### chatgpt_ref targets (the look we want)
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (1).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (2).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (3).png`
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 25 May 2026 00_18_45 (4).png`

### blueprint_room methodology (for grading structure correctness)
- `STAGING/concepts/chatgpt_ref/blueprint_room/ChatGPT Image 24 May 2026 23_42_09 (1).png` (asset groups)
- `STAGING/concepts/chatgpt_ref/blueprint_room/ChatGPT Image 24 May 2026 23_42_09 (2).png` (Library/Alcove)
- `STAGING/concepts/chatgpt_ref/blueprint_room/ChatGPT Image 24 May 2026 23_42_10 (5).png` (Open Front/Flooded)

### Comparison baseline (old placeholder version, for delta)
- `STAGING/s106_overnight/stream_e_rooms/combat_basic/scene.png` (old, placeholder)
- (same for the 4 others)

## Scoring rubric (be HARSH)

For each room, score 0-10 on each dimension:

1. **Real assets placed** (0-10): Are the actual sheet_2/3/4 wall sprites visible? Or still colored placeholder rectangles?
2. **Camera/depth illusion** (0-10): Does the scene have 3/4 depth feel? Or is it flat 90° top-down with no depth?
3. **Floor rendering** (0-10): Is there a visible floor tile? Or just dark background?
4. **Lighting/atmosphere** (0-10): Any torch glow / ambient mood? Or pitch dark / fully lit flat?
5. **chatgpt_ref overall likeness** (0-10): If you squint, does this look like the chatgpt_ref ARPG dungeon? Or still looks like a debug map?

Total per room = sum / 50.

## Pick the WINNER

After scoring, identify the ONE room that's most ready to be presented as user's "gerçek bir oda" proof. Justify why it won.

## Identify the WORST gaps (across all rooms)

What's still missing vs chatgpt_ref?
- Specific visual issues
- Whether the gap is fixable (and how) or requires new assets

## Output format (INLINE, NO FILE WRITES)

```markdown
# Stream F Vision Review — Antigravity — 2026-05-25 <time>

## Per-room scoring

| Room | Assets | Camera/Depth | Floor | Lighting | chatgpt_ref likeness | Total /50 |
|---|---|---|---|---|---|---|
| Combat Basic | ?/10 | ?/10 | ?/10 | ?/10 | ?/10 | ?/50 |
| Ritual Diamond | ?/10 | ?/10 | ?/10 | ?/10 | ?/10 | ?/50 |
| Flooded Crypt | ?/10 | ?/10 | ?/10 | ?/10 | ?/10 | ?/50 |
| Library Alcove | ?/10 | ?/10 | ?/10 | ?/10 | ?/10 | ?/50 |
| Boss Arena | ?/10 | ?/10 | ?/10 | ?/10 | ?/10 | ?/50 |

## Winner: <room name>

Why this is the best proof room: <2-3 sentences>

## Delta vs old (scene.png → scene_v2.png)
- Average improvement: <N → M / 50>
- Biggest visual win: <description>
- Biggest remaining gap: <description>

## Critical gaps still present

### Visual gaps (fixable now)
1. ...
2. ...

### Visual gaps (need new assets or major work)
1. ...
2. ...

## Recommended next iteration (for the winner room)

If user wants the winner to be even closer to chatgpt_ref, what's the next 30-60 min Codex task?

## Verdict

Is ANY of the 5 rooms now an acceptable "gerçek bir oda" proof for the user?
- YES: <which one + why>
- NO: <why + what's needed>

## Files opened
- <list every PNG you actually inspected>
```

## Constraints
- Be HARSH — user is going to look at these and judge. Don't sugar-coat.
- If a room still has placeholder visuals, score LOW on "Real assets placed" — that's the WHOLE point of Stream F.
- Compare side-by-side visually. Mention specific pixel differences (e.g., "rear wall in scene_v2 has visible brick texture vs solid blue in scene")
- Do NOT trust Codex's CODEX_DONE.md claims if visuals don't back them up. Visual evidence wins.

## Estimated time: 15-25 min (most of it = looking at images carefully)
