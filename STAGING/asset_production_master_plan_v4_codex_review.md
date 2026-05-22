# Asset Production Master Plan v4 -- Codex Review

Review date: 2026-05-22

Source caveats:
- `STAGING/research_outpainting_inpaint_stitching.md` was missing at the requested path, and `rg` found no matching research file.
- `memory/reference_pixellab_production_knowledge.md` and `memory/reference_pixellab_create_tiles_pro_4type.md` were missing at the requested paths, and `rg` found no matching filenames.
- NotebookLM fallback failed because authentication is expired. This review uses v1/v2/v3/v4, the local PixelLab docs, the Act 1 layout JSON, and the prior Codex v1 review.
- A local `create-character-pro` doc was not found. Boss cap comments below treat the 256 rule as a project/user lock plus PixelLab T1 edit/inpaint constraint, not as locally verified `create_character_pro` documentation.

## 1. v4 vs v1/v2/v3 evolution check

Verdict: mostly clean, but v4 needs budget and automation wording tweaks before it should be treated as dispatch-ready.

| Evolution point | Verdict | Notes |
|---|---|---|
| L1 Pivot Hades-iso | PASS | v1 pure top-down -> v2 Hades-iso pending -> v3 LOCKED -> v4 confirmed. This is sane. |
| L10 Parca-parca stitching | TWEAK | The 9-step sequence is technically sound as a hybrid production workflow, but not fully autonomous. Aseprite combine, palette snap, and manual spot-fix are user/manual steps in this environment. |
| L11 Boss 256 cap | TWEAK | 256x256 cap is sane and consistent with PixelLab T1 edit/inpaint limits, but local docs do not verify a `create_character_pro` max. Keep as a project lock, not an external-doc claim. |
| L12 Tile seam HYBRID | PASS | Wang16 for repeatable generic transitions plus Inpaint v3 for special seams is better than pure Wang or pure Inpaint. It controls cost and keeps hand polish reserved for hero seams. |
| L13 6-layer floor | PASS_WITH_TWEAK | L1/L2/L3/L4/L5/L6 are not redundant. Missing operational detail: density budgets, sorting/pivot/collision policy, and mapping from layout floor zones to layer masks. |

The v1 review TWEAKs still partly stand: tile size reconciliation note, L8 painter default fix scheduling, asset folder/catalog lock, and reward/gate/hazard assets are not closed by v4.

## 2. RULE 1-13 sequencing

Verdict: TWEAK.

RULE 3 being first is correct and carries the prior Codex recommendation forward: the Hades-iso wall prototype is the highest-risk visual gate for every object batch.

The problem is wording: v4 Section 5 says "The sequence is the execution order" but lists RULE 1 and RULE 2 before RULE 3. Section 0 and Section 7 correctly say RULE 3 is the first dispatch. Fix by either moving RULE 3 to the top of Section 5 or explicitly saying Section 7 is the dispatch order.

RULE 1 floor before RULE 4 wall full library is sensible after RULE 3 PASS. It is not a hard technical dependency for wall generation, but it gives Unity review context, anchors floor palette, and keeps full wall production from moving ahead without a scene base.

RULE 11 Boss, RULE 12 Combat Showcase, and RULE 13 Planning UI are correctly deferred to Phase L/M. They should stay out of the immediate asset-base dispatch loop.

## 3. Wall scope A budget verify

Verdict: TWEAK_NEEDED. v4 undercounts wall scope A.

User-decided Wall scope A:
- 24 standard: 8 classes x 3 variants.
- 9 hero: 3 classes x 3 variants, Arch + Throne backdrop + Gate, all 128x128, parca-parca.

Corrected budget:

| Scope | v4 claim | Corrected math | Verdict |
|---|---:|---:|---|
| Standard wall sprites | 3 batches x ~40 = ~120 gen | 24 sprites / 4-capacity batch = 6 batches. 6 x ~40 = ~240 gen. | TWEAK |
| Hero parca-parca | 9 x 60 = 540 gen | 2 create_object calls at 20-40 each + 1 Inpaint v3 at 20 = 60-100 per hero. 9 x 60-100 = 540-900 gen. | TWEAK |
| Total wall scope A | ~660 gen | ~780-1140 gen before rework. | TWEAK |

Also clarify classification: v4 RULE 4 includes `archway_n` as a wall class while user scope A also lists Arch as a hero class. Avoid double-counting by naming the standard arch module separately from hero Arch variant A, or by moving hero Arch out of the 24 standard count.

## 4. Stitching pipeline feasibility

Verdict: HYBRID, not autonomous.

L10 sequence assessment:

| Step | Feasibility | Notes |
|---|---|---|
| 1 Plan division | Autonomous | Codex can plan exact piece dimensions and seam masks. |
| 2 Generate piece 1 | Autonomous if user approves PixelLab generation | MCP can generate, but task forbids generation now. |
| 3 Generate piece 2 with style reference | Autonomous if source output is available | Feasible in principle with style reference/state workflow. |
| 4 Aseprite combine | User/manual in current environment | `aseprite` is not available on PATH. Could be automated later only if a CLI/tooling path is installed and exact placement rules are specified. |
| 5 Mask seam | Partly automatable | A deterministic mask can be generated, but v4 says Aseprite layer workflow. |
| 6 Inpaint v3 seam | User manual | Local docs say Inpaint v3 is available in Aseprite/Pixelorama extensions only and costs 20 gen per use. |
| 7 Palette snap | User/manual | Aseprite Indexed Mode is mandatory per v4/research claim, but no local CLI path is available. |
| 8 Spot-fix manual | User/manual | Subjective pixel cleanup cannot be assumed autonomous. |
| 9 Export | User/manual unless CLI added | Export is trivial once Aseprite process is available. |

Important doc correction: local `inpaint-v3.md` confirms minimum 32x32, maximum 256x256 with selection for larger canvases, and 20 generations per use. It does not document "palette-locking"; treat palette-locking as claimed/untested project knowledge, not official local-doc proof.

Operational impact: orchestrator cannot autonomously produce final hero walls today. Expect 5-15 minutes Aseprite labor per hero sprite, so 9 hero sprites means roughly 45-135 minutes user labor, with 90 minutes as a practical planning estimate.

## 5. Boss 256 cap math sanity

Verdict: PASS_WITH_DOC_CAVEAT.

Math:
- 256 px at PPU 64 = 4 Unity units.
- Warblade 64 px at PPU 64 = 1 Unity unit.
- Boss visual ratio = 4x character height/footprint if both imported at scale 1.

This is coherent and matches the stated KEMURLUK ratio target from the v3/v4 reference notes. If screen real estate feels crowded, scale in Unity instead of generating larger source sprites.

Animation:
- 18-24 frames at 256x256 is feasible for Unity.
- A single vertical atlas at 24 frames would be 256x6144, which is under common max texture limits but awkward to edit and import.
- Recommendation: export per-animation sheets or separate frame folders: idle 4 frames, attack 6 frames, hit 4 frames, death 4-8 frames. This keeps import settings, clip slicing, and rework smaller.

## 6. RULE-by-RULE PASS/TWEAK verdict

| Rule | Status in v4 | Codex verdict | Cost sanity | Dependency check |
|---|---|---|---|---|
| RULE 1 Floor base | PENDING_LOCK | ready-as-stated | 25 gen is sane for 64px create_tiles_pro without style refs; style refs can raise cost later. | Should run after RULE 3 PASS if visual order is preserved. |
| RULE 2 Floor transition | PENDING_LOCK | tweak-needed | 110 gen is plausible, but style_images can make tile calls 20-40 each. | Correctly depends on RULE 1 output. |
| RULE 3 Wall prototype | PENDING_LOCK | safe-to-lock-now | 20-40 is plausible; 64x96 may land at 40. | Correct first action. |
| RULE 4 Wall full library | PENDING_LOCK | tweak-needed | v4 125-200 is too low for Wall scope A. Corrected scope A is ~780-1140 including hero set. | Depends on RULE 3 PASS. Clarify relation to RULE 1. |
| RULE 5 Prop 32x32 | PENDING_DECISION | ready-after-list-approval | v4 says 30 gen, prior table says 20 gen for 32px. Mark estimate 20-30 until live cost confirmed. | Needs item list approval and should wait for RULE 3 object quality signal. |
| RULE 6 Prop 48-80px | PENDING_DECISION | ready-after-list-approval | 30-60 gen is sane. | Depends on object quality gate. |
| RULE 7 Prop 88-168px | PENDING_DECISION | tweak-needed | Cost should split native 4-slot calls from parca-parca per-asset overhead. | Depends on RULE 3 and L10 manual capacity. |
| RULE 8 Decal layer | PENDING_DECISION | tweak-needed | 105-120 gen plausible, but create_object vs create_tiles_pro cost labels are mixed. | Depends on RULE 1/2 material plan and L13 density target. |
| RULE 9 Hero anchor list | PENDING_DECISION | ready-as-stated | 300-600 user-paced if 5-10 assets x 3 acts x 20-40, but high end can exceed 600 if 10 assets all cost 40 across 3 acts = 1200. | Correctly after RULE 7. |
| RULE 10 Small variant scope | PENDING_DECISION | ready-after-scope-narrow | 200-400 for ~10 state calls is sane. | Correctly after RULE 5 and RULE 9. |
| RULE 11 Boss sprite | PENDING Phase L | ready-with-doc-caveat | 80-160 plausible for base + 3-4 states, but local create-character-pro doc missing. | Correctly deferred. |
| RULE 12 Combat showcase | PENDING Phase L | ready-as-deferred-scope | Dev estimate is plausible as a planning bucket, not a commitment. | Correctly independent of boss sprite except scene polish. |
| RULE 13 Planning UI | PENDING Phase M | ready-as-deferred-scope | No gen cost. | Correctly deferred. |

## 7. Risks: parca-parca production at scale

Main risk: Wall scope A turns parca-parca into a production line, not a one-off trick.

Scale:
- 9 hero sprites.
- Minimum generation calls: 18 create_object + 9 Inpaint v3 = 27 generation/edit calls.
- Manual passes: 9 Aseprite combine/palette/spot-fix/export cycles.
- User labor: about 45-135 minutes, likely around 90 minutes if each hero averages 10 minutes.

Mitigations:
- Run one pilot hero before committing all 9. Use a smaller non-critical prop or one hero gate/arch.
- Add a hard "stitch acceptance checklist": no visible seam at 100%, palette snap passes, transparent edges clean, Unity pivot verified.
- Limit parca-parca to the three hero classes only. Keep 64x96 modular wall segments native.
- Prepare deterministic templates for 128x128 hero split masks so user Aseprite time drops below 5 minutes per sprite.
- If pilot fails, fallback to native 128/168 create_object and accept lower detail rather than expanding manual rework.

## 8. NEW unknowns

Open unknowns or still-standing v1 review items:
- Tile size reconciliation: v4 locks 64x64, but the prior review's "S98 supersedes older 32px lock" source note still needs to be written somewhere canonical.
- L8 Painter default FAIL fix scheduling: prior review said live code default path can still load `PaintMode.Isometric` when prefs are missing. v4 does not schedule a fix.
- Asset folder/catalog structure lock: import path, naming, SpriteAtlas grouping, and prefab catalog ownership are still not locked.
- Phase L/M UI design details: correctly deferred, but still no scope boundaries for planning UI or combat UI implementation.
- PixelLab balance: v4 uses 2265; NotebookLM fallback failed and no live balance check was available through shell. Use stated 2265 per task instruction.
- `create-character-pro` doc: local docs do not verify the exact create-character 256 cap. Keep L11 as a project/user lock.
- Inpaint v3 palette locking: local docs do not document palette-locking. Aseprite Indexed Mode should remain mandatory.
- Act 1 layout assets: `rare` chest, `shattered_keep_throne_key`, locked door/gate socket, and pickup/relic assets remain under-modeled in the asset plan.

## 9. Recommendation -- next lock priority

Recommend one action: lock and run RULE 3 wall prototype first, but include a single parca-parca pilot decision immediately after prototype PASS before approving all 9 hero wall sprites.

Reason: RULE 3 still tests the highest-risk shared assumption, PixelLab `high top-down` wall readability. RULE 1 floor base is simpler and low-risk, but it does not answer whether the chosen Hades-iso object pipeline can carry the game's visual identity. The parca-parca pilot should happen before bulk hero scope because the real bottleneck is not gen budget; it is manual Aseprite labor and seam quality.

## 10. Final 3-sentence verdict

Plan v4 is TWEAK-needed, not rework: the lock evolution is sane, but the dispatch order wording, Wall scope A budget, and autonomous stitching claim need correction. Top next lock remains RULE 3 wall prototype, followed by one parca-parca pilot before committing to 9 hero wall sprites. If RULE 3 proceeds tonight, time-to-first-asset-in-Unity is roughly 45-90 minutes for one generated wall prototype, import, placement, and screenshot review, assuming PixelLab generation is approved and responsive.
