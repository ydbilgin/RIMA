# Codex Task — Wall Spec Review + Concept-Match Audit (Wave C)

ACTIVE RULES: (1) think before deciding (2) verify math + measurements (3) concept-match priority (4) BLOCKED if data unclear.

NLM ACCESS:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

---

## Mission

Wave C of escalation chain — review Opus wall production design + audit against concept art + answer user's 3 new concerns. Output goes to Wave D Opus for synthesis + final decision.

**User context (CRITICAL):**
1. Path A LOCKED — Karar #98 cyan extended to include containment lamps (braziers). Behavior differentiation: brazier = steady slow breath, rift = fast pulse + particles.
2. User reports: walls + character scale inconsistent in current Wave 4 implementation.
3. User reports: walls "yan yana öylesine duruyor, birleşmiş halde durmuyor" (side by side, not joined). Asks: should walls overlap (iç içe geçir) or use natural-look technique?
4. User reports: overall scene composition far from concept — "her şey saçma sapan yerlerde."

**Output file:** `STAGING/CODEX_DONE_wall_spec_review.md`

---

## Source files (READ FIRST)

1. `STAGING/OPUS_WALL_PRODUCTION_DESIGN.md` — Opus Wave B spec, MUST be reviewed completely
2. `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png` — visual anchor (target)
3. `Assets/Screenshots/walls_v1_spawn01.png` — Wave 4 current state (the problem)
4. `Assets/Screenshots/walls_v1_spawn02.png` — Wave 4 current state Spawn_02
5. `STAGING/RIMA_WALL_INVENTORY_AND_CANON.md` — inventory data
6. PixelLab MCP available: `mcp__pixellab__get_object` for hero sprite inspection

Wall sprites in pack (downloaded by orchestrator):
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pixellab_wall_section_horizontal.png` (384×216)
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pixellab_wall_corner.png` (192×128) — Opus rejected as purple-tinted
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pixellab_wall_arch_section.png` (192×128)

Plus existing `painterly_wall_01-12` (Wave 4 LIVE).

---

## Review tasks

### Task 1 — Verify Opus D1-D6 coherence

For each decision (D1 hero set hybrid, D2 cyan flame, D3 native scale, D4 skip band, D5 cleanup, D6 composition tables) — PASS/FAIL/REVISE verdict. Be specific about what fails.

### Task 2 — VERIFY SCALE MATH (USER CONCERN #1)

Opus says: "Room geometry: 18×12 cells, cellSize = 0.1667 (6 cells/unit). Interior X:0→3, Y:0→2 world units. Gates at (0, 1) and (3, 1)."

But earlier scene inspection said: "L1_BaseFloor tilemap bounds: Center: (9.00, 6.00, 0.00), Extents: (9.00, 6.00, 0.00)" — i.e. **world coords are 0..18 × 0..12, NOT 0..3 × 0..2**.

Possible Opus error: confused Unity Grid `cellSize` property with world coordinates. Real scene:
- World extent 0..18 × 0..12 = 18×12 world units
- Each cell = 1×1 world unit (standard tilemap)
- Gates at world (0.5, 4.91) and (17.5, 4.91)

If Opus's coordinates are wrong, D6 composition table is wrong (sprites would be in tiny 3×2 area). Verify which is correct, recompute D6 if needed.

### Task 3 — Wall + character scale consistency (USER CONCERN #1 cont.)

Concept art ratio: character ~1/15 room width, wall ~3× character height.

After verifying real scene scale:
- Warblade_Player at scale (0.85, 0.85, 1), positioned at (9, 4.91, 0)
- Warblade sprite native ~64×64? Verify via execute_code reflect.
- pixellab_wall_section_horizontal at native 384×216 — at PPU 32, this is 12×6.75 world units
- Room is 18×12 world units

If wall sprite is 12 units wide in a 18-unit room → 1-2 sprites cover one edge. Sounds reasonable. But verify:
- Wall sprite world dimensions at native PPU (32 or whatever Unity import gives)
- Character world dimensions
- Required ratio: wall ~3× char height

Provide CORRECTED scale plan if Opus's was wrong.

### Task 4 — Wall connection technique (USER CONCERN #2 — primary)

User wants walls to LOOK JOINED, not "yan yana öylesine duruyor." Currently Wave 4 shows visible cut lines between sprite pieces.

Evaluate 5 connection techniques:

| # | Technique | How it works | Pros | Cons |
|---|---|---|---|---|
| A | **Overlap 20-30%** | Place sprites with overlap, hard edge inside overlap zone | Simple, no shader | Lossy edges, sprite gets cut by neighbor |
| B | **Alpha edge masking** | Soft alpha at sprite L/R edges (post-import) | Smooth blend | Per-sprite edit, lossy |
| C | **Tileable middle + corner caps** | Middle section seamless tile, corners cap | Industry standard | Needs sprite to be tileable horizontally |
| D | **Hide-seam decals** | Place small decals/cracks/vines exactly at seams | Cheap, hides cuts | Needs N-1 decals per edge |
| E | **9-slice + shader blend** | Unity sprite 9-slice with edge alpha gradient | Most flexible | Requires shader work |

Recommend BEST for our 3 PixelLab sprites considering:
- `pixellab_wall_section_horizontal` is NOT tileable (has unique rubble at base)
- Sprites have hard rectangular edges in their PNG
- We want natural concept-art look

User's specific question: "iç içe geçirerek mi yapılacak doğal görünüm yapılacak" — should they overlap or look natural? Codex recommends.

### Task 5 — Concept-match audit (USER CONCERN #3)

Compare Opus D6 composition plan vs concept_v1.png. Specifically:
- Wall verticality (3-4× character)? Opus says PixelLab native gets us there — verify with actual sprite dimensions
- Brazier placement at gate flanks? Opus D6 puts at (0.45, 1.0) and (2.55, 1.0) — these coords are inside Opus's wrong scale. Recompute.
- Vine accents at top wall? Opus has 2 — concept has chains too (concept shows hanging chains)
- Gate arch verticality + depth? Verify gate_arch.png shows depth match
- Floor wall transition (Opus D4 SKIP) — is sprite-baked rubble actually hiding the seam OR do we still need scatter?
- Character size in concept ~1/15 room — current Warblade scale 0.85 in 18-unit room. Adjust?

Provide PASS/FAIL for each concept element + corrective action.

### Task 6 — Concept fidelity rating

After reviewing Opus plan + your corrections, give a 0-100% concept fidelity prediction for the final scene if Wave D + Wave E executed correctly. If <80%, recommend additional changes.

### Task 7 — Implementation steps refinement

Opus's 10-step integration plan: critique + revise if needed. Specifically:
- Order of operations correct?
- Any missing prerequisite (e.g. CameraFollow.SetBounds, sprite import settings, etc.)?
- Test pass adequate?

---

## Output structure (`STAGING/CODEX_DONE_wall_spec_review.md`)

```markdown
# Codex Wave C — Wall Spec Review

## VERDICT: PASS / PASS_WITH_REVISIONS / FAIL

## 1. D1-D6 coherence verdict
[per-decision PASS/REVISE/FAIL + rationale]

## 2. Scale math correction
[show real numbers — room dimensions, sprite world sizes, character size, ratios]

## 3. Wall+character scale specific plan
[concrete adjustments]

## 4. Wall connection technique recommendation
[A/B/C/D/E recommendation + rationale + implementation specifics]

## 5. Concept-match audit
[table: concept element | Opus plan status | corrective action]

## 6. Predicted fidelity %
[N% + reasoning]

## 7. Revised integration plan
[step-by-step, replacing Opus's if needed]

## 8. Open questions for Wave D Opus
[any unresolved sub-decisions]
```

---

## Hard rules

- DO read Opus spec + concept image + Wave 4 screenshots fully before deciding
- DO verify scale math (do the arithmetic, don't trust Opus's coords)
- DO recommend connection technique definitively (don't punt to "user decides")
- DO NOT redesign from scratch — refine Opus's plan
- DO NOT skip user's 3 concerns
- Path A (cyan scope) is LOCKED — don't revisit

## Effort

~25-30 min review work.