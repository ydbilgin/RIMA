# Codex Task — Review Opus Tile Angle Verdict (S93 night)

ACTIVE RULES: (1) think before deciding (2) honest review (3) BLOCKED if unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## Mission

Opus rima-design (S93 night) produced verdict: `STAGING/TILE_ANGLE_ARCHITECTURE_OPUS.md`. Recommendation:
- **Branch D (PRIMARY):** Hades model, flat tiles + de-emphasized floor (low contrast/saturation), L3/L4/L5 carry visual weight
- **Branch E (COMPLEMENTARY):** Camera tilt 8-12° via Unity Camera transform
- **Branch A REJECTED:** PixelLab `low top-down` is sprite-frame not ground-plane projection — cannot paint trapezoid

User sleeping, sabah verdict review yapacak. Your job: pragmatic technical reality check.

## Verify or challenge

1. **Branch E technical feasibility** — Camera tilt 8-12° in Unity 2D Pixel Perfect setup:
   - Does Pixel Perfect Camera break at tilted angle?
   - URP 2D Renderer + 2D Lights compatibility?
   - SpriteRenderer billboard behavior?
   - What's the safe tilt angle range without pixel grid breaking?

2. **Branch D execution risk** — "floor de-emphasis" via tint/saturation:
   - Where does `RoomTemplateSO` set tint currently? Add `floorContrastMultiplier` field feasibility?
   - MapLayerOrchestrator paint path can apply tint multiplier?
   - Test existing tiles with -20% saturation override — visual screenshot

3. **Branch A REJECT verification** — is PixelLab `low top-down` truly sprite-frame projection?
   - Spot-check existing `create_topdown_tileset` `view: "low top-down"` output (e.g. Test 1 `wang16_test.png`)
   - Confirm: tiles look like "squares with shading" not "trapezoids receding"
   - Independent confirmation of Opus's structural claim

4. **Karar #148 formalization** — should this be a new Karar?
   - MASTER_KARAR_BELGESI.md slot
   - Conflicts with existing Karar (especially #100, #143, Map Plan v1)?

## Required output

`STAGING/CODEX_DONE_review_tile_angle_verdict.md`:

```
# FINAL VERDICT
[Single paragraph: orchestrator should execute Opus plan as-is / with refinements / wait]

# 1. Branch E camera tilt feasibility
[Test plan + safe angle range + any blockers]

# 2. Branch D floor de-emphasis implementation
[RoomTemplateSO field + MapLayerOrchestrator path + tint workflow]

# 3. Branch A reject verification
[Visual evidence + sprite-frame vs ground-plane projection check]

# 4. Karar #148 draft
[Proposed Karar text, conflicts list]

# 5. Refinements
[Anything Opus missed or got wrong]

# 6. Morning action item
[Exactly one concrete next step]
```

Effort: medium. ~15-20 min. Light technical verification, not deep new research.
