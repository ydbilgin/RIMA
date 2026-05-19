# Codex Task — 5000 PixelLab Budget + System Direction Independent Review

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Scope
**NO CODE — strategic allocation review only.** Read Opus position, give independent verdict, propose alternative if you disagree.

## Background
User just got refresh of **5000 PixelLab generation credits**. Said "characters + map everything can be done from here, you decide, ask Codex too, Opus final decision".

Current state:
- v15d composition budget LOCKED via 3-agent verdict (Opus + Codex + Gemini from earlier today). See `STAGING/CODEX_DONE_BOONA_REVIEW.md` + `STAGING/GEMINI_DONE_BOONA_REVIEW.md`.
- `hybrid_asset_pipeline_lock` (S87 Karar #157): PixelLab → characters/mobs/props, Codex imagegen → tiles/walls/decals.
- 10 canonical class anchors LOCKED but PixelLab IDs TBD.
- 8-mob roster planned (Imp → Hulk).
- VFX track empty, autosprite MCP registered today, dual-pilot considered (dash trail + hitspark).
- Asset gap on v15d: L1=0 (macro fill), L8=0 (atmospheric), 22 sprite produced but not yet wired to zone arrays.

## Opus position (review this)

### Claim 1: Hibrit pipeline LOCK korunmalı — PixelLab budget'ını PixelLab güçlerine ayır, tile gen Codex'te kalsın.

### Claim 2: 5000 PixelLab allocation

| Kategori | Gen | % | Justification |
|---|---|---|---|
| Characters (10 anchor + states + anims) | ~500 | 10% | Critical path, roster LOCK ama TBD |
| Mobs (8-mob roster) | ~400 | 8% | Combat depth zorunlu |
| Hero props (cluster templates) | ~300 | 6% | v15d composition budget destek |
| VFX dual-pilot (PixelLab side of A/B) | ~100 | 2% | Provider LOCK için A/B |
| Iteration + regen reserve | ~3700 | 74% | Buffer |

### Claim 3: Phase order
1. Week 1: v15d composition Codex dispatch + Codex tile support
2. Week 1-2 parallel: Character anchor push (user web UI + MCP animate)
3. Week 2-3: VFX dual-pilot
4. Week 3+: Mob roster

### Claim 4: Defer to S90+
- Shader biome blending
- Full POI system
- Boss roster
- Cinematics

## Your job

Read the Opus position above. Then independently:

1. **Hybrid LOCK challenge**: Is keeping Codex imagegen for tiles RIGHT? Or should we unify on PixelLab with 5000 budget? Be specific — under what conditions would you switch? Look at: `MEMORY/project_hybrid_asset_pipeline_lock.md` (if exists; if not, infer from S87 LOCK note in MEMORY index).

2. **Allocation challenge**: 500 chars / 400 mobs / 300 props / 100 vfx / 3700 reserve — is the split right? Too much reserve? Too little on chars (10 anchors × ~50 gen each = 500 might be tight if state tweaks heavy)? Propose YOUR split with numbers.

3. **Phase order challenge**: Should map-fix (v15d composition) really be Week 1 priority? Or are characters more bottleneck since they're TBD and combat doesn't work without them?

4. **Missing categories**: What's Opus missing? UI/HUD assets? Item icons? Skill VFX? Boss prep? Audio cue sprites?

5. **5000 reality check**: With 5000 budget, can we attempt something previously skipped? E.g. multi-style character variants, full boss roster prep, environmental hazards?

## Output
Write to `STAGING/CODEX_DONE_5000_ALLOCATION_REVIEW.md`.

Structure:
```
# Codex Allocation Review

## Hybrid LOCK verdict
KEEP / REVISIT / SWITCH + reasoning

## Allocation counter-proposal
Table with your numbers. Justify deltas from Opus.

## Phase order verdict
AGREE / REORDER + reasoning

## Missing categories
List + rough gen budget each

## 5000 stretch suggestions
Things now possible that were previously deferred

## Confidence
HIGH / MEDIUM / LOW on each verdict + why
```

Keep ≤ 400 lines. Opinionated, evidence-based. Opus will synthesize.

## What NOT to do
- No code edits
- No new task files
- Don't repeat Opus position — give your INDEPENDENT take
