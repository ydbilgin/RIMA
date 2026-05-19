# RIMA Morning Briefing — S93 night work synthesis

## TLDR (3 sentences)
Overnight work moved RIMA forward: Ronin Day 1 is live with caveats, the tile-angle crisis has a clear answer, CB pivot is rejected for now, and a 48-skill bank exists but needs a minor revision pass. Biggest signal: RIMA's problem is not "no project identity"; it is unresolved pitch plus execution drift, and Death Imprint is the strongest candidate to turn Echo Imprint into a real signature. User needs to decide three things this morning: lock the floor hierarchy fix, continue RIMA over CB with a prototype-gated Death Imprint direction, and approve the skill-bank revision rules.

## ⭐ POST-SYNTHESIS UPDATE — Transform Squash KILLER FINDING

Free asset research (`STAGING/RESEARCH_DONE_free_asset_alternatives.md`) landed AFTER Codex synthesis. Key insight changes Decision 1:

**Transform Squash (highest-ROI tile angle fix):**
- Set Unity Tilemap parent `transform.localScale.y = cos(35°) = 0.819`
- ZERO new art, ZERO shader, ZERO PixelLab gen
- Optically foreshortens flat 90° tiles to match 35° character projection
- Reversible single-line config
- Tested live tonight (Spawn_01_Painterly_v3 grid) → applied successfully, math correct
- Result: tiles foreshorten as predicted; Painterly Pack hard borders still dominant (needs Branch D contrast fix on top)

**Critical insight from research:** "All AI generators (PixelLab, DALL-E, gpt-image-1) bake perspective into objects by default. Prompting for '35° tile' gets you an object floating in space viewed from 35°. That's why 1200+ gens failed. Fix: always prompt 'perfectly flat, straight top-down', apply angle OPTICALLY in Unity (Transform Squash). Stardew + Zelda actually work this way."

**Revised Decision 1 morning sequence:**
1. **Transform Squash test** (5 min, Tilemap parent Y = 0.819, reversible)
2. **Branch D floor de-emphasis A/B** (45 min, contrast/saturation cut)
3. **Branch E camera tilt 4-8°** (15 min smoke test, only if 1+2 not enough)

**Other research findings:**
- **32rogues** (CC0 itch.io, 32px) recommended as control set to validate the math
- **ComfyUI + SDXL + Make Seamless node** on RTX 5080 = autonomous scalable tile pipeline (long-term)
- **Tilesetter $25** considered but Transform Squash makes it lower priority

## What shipped overnight (concrete artifacts)
- Ronin Day 1 code is live: 12 files added, multiple class-manager/UI/basic-attack files modified, `dotnet build` passed, EditMode tests passed; Unity batchmode compile was blocked because the project is open elsewhere.
- Tile-angle architecture verdict: Branch A 35-degree tile regen rejected; Branch D floor de-emphasis is primary; Branch E camera tilt is only a reversible 4-8 degree smoke test after floor A/B.
- CB pivot verdict: immediate CB pivot rejected. RIMA should continue, borrow CB's pitch clarity, and avoid resetting 50-60% of current investment.
- Epic mechanic verdict: Echo Imprint Cascade is the strongest signature candidate, but review amends this to a prototype gate under a safer name: **Death Imprint** or **Architect Imprint**, because "Echo Cascade" conflicts with Karar #122.
- 4-class skill design bank exists: Warblade, Elementalist, Ranger, Shadowblade each have 4 active, 4 passive, 4 Echo trigger entries.
- Skill-bank review returned **NEEDS REVISION — minor**: 43/48 keep, 5 polish, no cuts, but tag schema and Echo trigger variety need approval.
- Research landed for Hades/Children of Morta/Death's Door pipeline and mechanic-bank mapping.
- Missing/incomplete inputs: `STAGING/RESEARCH_DONE_free_asset_alternatives.md` is not present; named memory files from the task were not found under `MEMORY/`, though `CURRENT_STATUS.md` references them as intended night memory.

## 3 strategic decisions queued for user

### Decision 1: Floor Perspective Lock
- Context: Character sprites are 35-degree high top-down, but floors are square top-down. Review says the mismatch is real but not solved by regenerating all tiles.
- Options: A) regen all floor tiles at fake 35-degree angle, B) flatten characters to 90 degrees, C) keep flat floor as background and push perspective into walls, props, patches, lighting, and optional tiny camera tilt.
- Recommendation: Choose C. Add Karar #148: floor is structurally background; Branch A rejected; test floor de-emphasis before camera tilt.
- Cost of wrong call: A burns gen budget and time on a PixelLab limitation; B violates locked character identity; skipping C keeps the flat/grid complaint alive.

### Decision 2: RIMA vs CB + Signature Mechanic
- Context: CB has a better one-line pitch but no Unity project, code, asset pipeline, or proven feel. RIMA has 6 months of systems, assets, Brush/Painter infrastructure, and now Ronin progress.
- Options: A) immediate CB pivot, B) continue RIMA unchanged, C) continue RIMA plus one-week pitch sprint plus prototype-gated Death Imprint.
- Recommendation: Choose C. Do not greenlight a 3-4 week feature yet; first write a 1-page prototype gate: 1 room, 1 class, 3 imprint types, save stub, readability caps, pass/fail criteria.
- Cost of wrong call: Pivoting now discards too much; continuing unchanged leaves the Hades-clone criticism; overcommitting Death Imprint without a gate risks death-tax clutter.

### Decision 3: Skill Bank Revision Rules
- Context: The 48-skill bank is structurally usable, but review found lopsided tags, copy-pasted Echo trigger rhythm, weak entries, and no Death Imprint hooks.
- Options: A) implement as-is, B) approve minor revision package, C) cut the pool now.
- Recommendation: Choose B. Approve 9→7+2 tag reclassification for Karar #65, Warblade/Shadowblade trigger anomalies, 5 polish changes, and `DeathImprintBehavior` reserved in the SkillDesignDoc schema.
- Cost of wrong call: As-is implementation bakes weak taxonomy into data; cutting now loses useful pool depth; delaying blocks Day 1 schema work.

## Pending implementation work (day-by-day plan if decisions go ahead)

Day 1: Duplicate one room scene; capture baseline vs floor tint/contrast de-emphasis only. Separately write the 1-page Death Imprint prototype gate. Lock SkillDesignDoc schema with 7 state tags + 2 meta-tags and `DeathImprintBehavior`.

Day 2: If A/B passes, apply floor de-emphasis through the active render path, not RoomTemplateSO only. Map Warblade/Ranger skill-bank entries to current code/data.

Day 3: Test camera tilt only after floor A/B, at 4/6/8 degrees, hard stop on shimmer or character distortion. Apply Elementalist/Shadowblade polish: Lightbreak hook, Blink simplification, Shadow Dance cost, deterministic Hemorrhage.

Day 4: Add passive data and class death-imprint one-liners. Keep Death Imprint as prototype data, not full content.

Day 5: Add Echo T1-T3 templates with Warblade parry and Shadowblade CP-finisher anomalies; enforce max 2 simultaneous Echo VFX.

Day 6: Add T4 Rift Proc detection with per-target ICD and tag consumption rules. Smoke-test one Cursed Room condition if Death Imprint gate passed.

Day 7: QC pass: resource loops, tag coverage, Ronin tension rhythm, 5 temporal combos, floor readability, and whether the pitch is now explainable in one sentence.

## Risks + open questions

- Ronin is live but not fully validated in Unity because batchmode compile was blocked; Sakura Veil is still a placeholder overlap/trigger deflect, not a real incoming-damage parry hook.
- Ronin tension canon conflict exists: implementation followed task contract (idle gain, movement drain), while newer NLM canon reportedly says moving gains and idle drains.
- Branch E camera tilt is not a commitment. SpriteRenderer does not auto-billboard; pixel shimmer and character foreshortening can kill it.
- Death Imprint naming must avoid "Echo Cascade" because Karar #122 already uses Echo Cascade in a resonance/altar context.
- Tag reclassification 9→7+2 updates Karar #65 and needs explicit approval.
- DaveX tweet is still blocked: x.com returned 402 and Nitter failed. User needs to share the tweet content manually if it matters.
- Free asset survey did not land before this synthesis.
- Memory sync appears incomplete: the task-listed memory files are referenced in status but not found in `MEMORY/`.

## What user should do FIRST (single concrete action)

Approve or reject this exact morning sequence: **floor de-emphasis A/B first, Death Imprint prototype gate second, skill-bank minor revision third, Ronin stress test continues without CB pivot.**
