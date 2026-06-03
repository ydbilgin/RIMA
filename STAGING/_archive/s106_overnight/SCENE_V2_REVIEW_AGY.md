# Scene v2 vs M3 Reference — Antigravity Visual Review

ACTIVE RULES: (1) think before answering (2) net, no waffle (3) visual judgment lens (4) BLOCKED if can't be concrete.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Codex Cycle 1 implemented PlayableArena scene to roughly match Hades Elysium reference M3. User wants autonomous multi-AI iteration. Your visual review will guide Codex Cycle 2. Reference comparison image already prepared.

## INPUTS (read in order)

1. **`STAGING/s106_overnight/scene_v2_vs_M3.png`** — side-by-side (Cycle 1 left, M3 reference right). **THIS IS YOUR PRIMARY INPUT.**
2. `STAGING/s106_overnight/SCENE_V2_REPORT.md` — Codex's self-assessment
3. `STAGING/s106_overnight/walless_v1_batch2_M3.png` — clean reference
4. `STAGING/s106_overnight/scene_v2_match_attempt.png` — game view of Cycle 1

## CONTEXT — Codex Cycle 1 changes
- Cleared 2533-tile dev floor → repainted compact 41-cell floating arena
- 5 ritual rune, 20 cyan vein, 16 cobblestone, 0 dirt
- Player + Camera + Rigs at origin
- 24 Kit B cliffs around perimeter
- Kit C BG layers visible
- 2 warm + 2 cyan Light2D at corners
- Global Light 2D intensity 0.3

## YOUR JOB

Review side-by-side. For each visual element below, rate Cycle 1 (1-5 stars) and give SPECIFIC actionable feedback for Codex Cycle 2:

### Critical elements to score
1. **Arena shape** — diamond/hex floating island silhouette
2. **Central rune circle focal** — M3 has BIG glowing cyan circle in center; Cycle 1 has scattered rune tiles
3. **Brazier corners** — M3 has 4 distinct warm flame braziers; Cycle 1 has 2 dim warm lights
4. **Cliff face perimeter** — M3 cliffs vary in style; Cycle 1 cliffs uniform
5. **Lightning/storm BG** — M3 has purple electric streaks; Cycle 1 missing
6. **Nebula/void atmosphere** — M3 has cyan + purple gas; Cycle 1 has cyan only
7. **Columns/statues** — M3 has stone columns at corners; Cycle 1 doesn't
8. **Tonal contrast** — M3 dark void + bright arena focal; Cycle 1 too dim overall
9. **Cyan glow intensity** — M3 rune ring + cliff glow; Cycle 1 needs more glow
10. **Overall "looks alive"** — M3 storytelling presence; Cycle 1 sterile

### For each gap, provide:
- "Cycle 2 should: [specific action]"
- "Achievable via: [Light2D intensity / new sprite / particle / shader / decal / scale]"

### Prioritized Cycle 2 punch list
Top 5 changes Codex should make in next dispatch, ranked by impact-to-effort ratio. Be concrete (Unity commands / component names / numeric values).

## CONSTRAINTS for Cycle 2 spec you propose
- NO PixelLab generation (user halted)
- NO new sprite art (use existing Kit A/B/C + lights + existing PixelLab inventory at `STAGING/PIXELLAB_INVENTORY_CATALOG_2026_05_25.md` if applicable)
- Can use Unity built-ins: Particle System, Sprite Mask, additive lights, decals
- Existing Lighting: Warm Torch Light NW/NE, Cyan Crystal Light SW/SE (has LightPulse)

## DELIVERABLE
Write to `STAGING/s106_overnight/SCENE_V2_REVIEW_VERDICT_AGY.md`. Max 600 words.

Sections:
1. **Score card** (10 elements × 1-5 stars)
2. **Top 5 Cycle 2 changes** (impact / effort / specific action)
3. **Overall:** Is Cycle 1 close enough to ship as a playable demo, or does Cycle 2 mandatory?
4. **VERDICT line:** `VERDICT: <SHIP-NOW | ITERATE-CYCLE-2 | MAJOR-REWORK>`
