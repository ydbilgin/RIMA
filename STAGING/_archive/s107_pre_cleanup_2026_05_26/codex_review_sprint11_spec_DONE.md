# Codex Spec Review -- Sprint 11 Natural Engine
Date: 2026-05-16
Reviewer: rima-codex (Claude Sonnet 4.6 wrapper, direct source analysis)
Verdict: PASS-WITH-CONDITIONS (85%)

---

## Review Matrix

| # | Check | Status | Evidence (file:line) |
|---|---|---|---|
| 1 | Forbidden list discipline | PASS | No forbidden items appear in deliverables. FocalCluster enum value defined but generator explicitly skips it (V1 skip). Boundary is clean. |
| 2 | File scope coverage | PASS-WITH-CONDITIONS | 8 files listed are structurally sufficient BUT see R1: partial class requires touching WallOverlayPainter.cs class declaration keyword. The MODIFY entry covers this, but spec text says "existing methods UNCHANGED" which may mislead Codex into not adding the `partial` keyword. |
| 3 | Sprint 9 compatibility (Wang encoding) | PASS | WangSliceGenerator.cs:87 -- `return $"wang_{ne}{nw}{se}{sw}"` produces wang_{NE}{NW}{SE}{SW}. Spec §2.4 states format `wang_{NE}{NW}{SE}{SW}`. EXACT MATCH. Example in spec: wang_1110 = NE+NW+SE filled, SW=0 -- confirmed correct against WangTagFromCorners ordering (ne, nw, se, sw). No drift. |
| 4 | Sprint 10 compatibility (RoomTemplateSO fields) | PASS | RoomTemplateSO.cs:14 -- `public RectInt bounds` accessible. Line 15 -- `public List<DoorSocket> doorSockets`. DoorSocket.cs:9 -- `public Vector2Int position` available for radius marking. All field names match spec §2.3 exactly. |
| 5 | WallOverlayPainter touchability | FAIL-MITIGATABLE | WallOverlayPainter.cs:9 declares `public sealed class WallOverlayPainter`. Spec §2.5 proposes `public partial class WallOverlayPainter`. C# requires the original class declaration to also carry the `partial` keyword for a multi-file partial split to compile. Current class is `sealed` (not `sealed partial`). Fix: change line 9 to `public sealed partial class WallOverlayPainter` in the MODIFY pass. Spec text "existing methods UNCHANGED, only new overload added" does not mention this structural touch -- Codex may skip it and produce a compile error. Alternatively, add the overload directly in-file (no partial needed, simpler, recommended). |
| 6 | Test isolation | PASS | Sprint 11 tests go under `Assets/Tests/EditMode/Composition/` -- no collision with existing `/Brush/` or `/Room/` subdirs confirmed by filesystem scan. TempTests path `Assets/TempTests/Composition/` is clean (TempTests dir does not yet exist). No collision risk. |
| 7 | OQ §6 resolutions soundness | PASS-WITH-CONDITIONS | OQ-1 runtime-gen recommendation is sound (1ms negligible). OQ-2 4-bit confirmed correct by source. OQ-3 1-tile WallBand + 2-tile DecoratedEdge is reasonable. OQ-4 radius 3 is correct. Condition: OQ resolutions are marked "Recommendation" but spec does not indicate whether Opus has signed off. If these are still open at implementation time, Codex may pick wrong values. Spec should mark them RESOLVED or require explicit signoff line before dispatch. |
| 8 | Exit criteria measurability | PASS-WITH-CONDITIONS | EC-1 through EC-8 are binary and unambiguous. EC-9 "No runtime non-integer scale" is verifiable by code search but slightly vague -- what constitutes "Sprint 11 code"? Only the 4 new Composition/ files, or also the WallOverlayPainter edit? EC-10 "Compile with UNITY_EDITOR stripped" -- correct guard but verification method unspecified (no test, no CI step defined). Both are low-risk but worth tightening. |
| 9 | V1.5/V2 boundary clarity | PASS | §9 roadmap is clean. V1.5 items (other painters, FocalCluster authoring, editor visualizer) are clearly deferred. V2 items (SpriteAtlas, AI tags) are even further deferred. No bleed-through found in §2 deliverables. FocalCluster enum value in §2.1 is a data placeholder, not an authoring tool -- acceptable. |
| 10 | Estimate accuracy | PASS | 1.5-2 days is appropriate. Scope is 4 new files (~250 LOC total) + 1 MODIFY (~30 LOC) + 3 test files (~150 LOC). The generator algorithm is deterministic and well-specified. Wang resolver has clear logic. No external API calls. If partial class issue (R1) is pre-resolved, 1.5 days is achievable. |

---

## Spec Drift

- **Wang example order vs. actual tag function:** Spec §2.4 example `wang_1110 = NE+NW+SE corners filled, SW empty`. WangSliceGenerator.WangTagFromCorners produces `wang_{ne}{nw}{se}{sw}` so position 3 = se, position 4 = sw. wang_1110 = NE=1, NW=1, SE=1, SW=0. Spec description is CORRECT. No drift.
- **DoorSocket position field:** Spec says "socket position" -- actual field name is `DoorSocket.position` (Vector2Int). Match confirmed. No drift.
- **partial class not flagged in MODIFY entry:** §3 says WallOverlayPainter.cs STATUS=MODIFY but does not specify that the class declaration keyword must be amended. This is implicit and may cause Codex to produce non-compilable code if it uses a separate .cs file for the partial extension.

---

## Risk List

- R1 (P1): `sealed class` vs `partial class` conflict in WallOverlayPainter. If Codex generates a separate partial file without first adding `partial` to the original class declaration, the project will not compile. Fix: either (a) spec must explicitly state "change line 9 to `public sealed partial class`" in the MODIFY instructions, or (b) spec must state "add overload directly in WallOverlayPainter.cs body, no partial split." Option (b) is simpler, recommended.

- R2 (P2): OQ §6 open questions not formally closed. If Opus has not signed off before Codex implements, Codex must choose defaults. Spec says "Opus Signoff Required Before Implementation" -- if this sign-off is missing at dispatch time, Codex should be instructed to use the recommended values as defaults and not block. Current spec language ("Required") may cause Codex to pause unnecessarily.

- R3 (P2): `WangContextResolver` depends on `BrushAssetVariant.variantId` matching the Wang tag string. Spec assumes this field exists and contains the tag. BrushAssetVariant.cs is not in the allowed file list and was not reviewed here -- if `variantId` is named differently or does not exist, Sprint 11 compile will fail. Recommend adding BrushAssetVariant.cs as a READ-ONLY allowed file so Codex can confirm the field name before implementing PickVariantForCase.

---

## Recommended Fixes

1. **§2.5 WallOverlayPainter -- clarify approach (HIGH PRIORITY):** Replace "partial class" approach with direct in-file addition. Change spec text to:
   ```
   // In Assets/Scripts/MapDesigner/WallOverlayPainter.cs
   // Add directly to the existing sealed class body -- no partial split needed.
   // Existing methods UNCHANGED. Add after PlaceWallSprite():
   public void PlaceWallSprite_ContextAware(...)
   ```
   This eliminates R1 entirely.

2. **§6 OQ resolutions -- mark as resolved:** Add a line under each OQ: `STATUS: RESOLVED -- use recommendation as default`. Remove "Required" from the header so Codex does not pause for signoff during autonomous implementation.

3. **§3 File scope -- add BrushAssetVariant.cs as READ-ONLY:** Add a row:
   ```
   Assets/Scripts/MapDesigner/Brush/Data/BrushAssetVariant.cs | READ-ONLY (field name verification)
   ```
   Codex must confirm `variantId` field exists before wiring PickVariantForCase. Addresses R3.

4. **§5 EC-9 / EC-10 -- tighten scope:** EC-9: specify "Sprint 11 scope = all files in §3 allowed list." EC-10: specify verification method -- "Run `dotnet build` with scripting define `UNITY_EDITOR` removed from PlayerSettings and confirm zero errors" or "Code review: grep for `UnityEditor` namespace in non-#if UNITY_EDITOR blocks."

---

## Sprint 12 forward-compat

- `CompositionRoleMap.GetRoleAt(Vector2Int pos)` is the integration point for Sprint 12 PropDefinitionSO. Spec §8 references this correctly. No forward-compat issues found.
- Sprint 12 will need `CompositionRoleMap` to be either (a) stored as a field on RoomTemplateSO or (b) generated at paint-time. OQ-1 recommends runtime-gen (no field added to RoomTemplateSO). Sprint 12 Props painter will need to call `CompositionRoleMapGenerator.GenerateFromRoom(room)` itself or receive the map as a parameter. Both paths are clean -- no blocking issue.
- `DoorSafety` radius is hardcoded at 3 in V1 (const). Sprint 12 Props will likely want to respect this radius for prop exclusion near doors. The const is accessible as `CompositionRoleMapGenerator.DoorSafetyRadius` -- Sprint 12 can read it without recomputing. Good design.
- `FocalCluster` enum value exists but is not populated by V1 generator. Sprint 12 Props may want FocalCluster zones for high-value prop placement. V1.5 authoring marker integration is correctly deferred. Sprint 12 should either (a) wait for V1.5 FocalCluster authoring, or (b) treat FocalCluster same as DecoratedEdge for props in V1 fallback. Recommend noting this in Sprint 12 spec as a dependency.

---

STATUS: DONE
COMPLETED:
- Full 10-item review checklist executed against live source files
- 3 risks identified (R1 P1, R2 P2, R3 P2)
- 4 concrete recommended fixes provided
- Sprint 12 forward-compat assessed
ERRORS: NONE
FILES_TOUCHED: STAGING/codex_review_sprint11_spec_DONE.md
NEXT_SIGNAL: "Review complete -- STAGING/codex_review_sprint11_spec_DONE.md written"
