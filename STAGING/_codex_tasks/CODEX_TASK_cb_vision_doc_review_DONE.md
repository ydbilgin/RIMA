# CB VISION_DOC Review - Codex Verdict

## Overall: REVISE

This document is strong enough in identity and direction, but not clean enough to save as a locked VISION_DOC yet.

The main identity promise is clear: **Real-Time Generative Action Roguelike** is stated loudly, repeatedly, and as a user lock.

The review fails on four save-blocking points:

1. Anti-clone coverage is good but incomplete for ARPG-adjacent risks.
2. Phase 2 timing and content load are not coherent enough.
3. Taxonomy expansion claims do not fully match the enumerated lists.
4. Decision Log misses several explicit locks/open boundaries and contains some items not fully grounded in the body.

Verdict: **REVISE**.

Not FAIL, because the core document is usable and the issues are bounded edits.

Not PASS, because the document is intended as a long-term truth document and current inconsistencies would create confusion 6 months later.

## Section-by-section

| Section | Status | Issue (if any) |
|---|---|---|
| 0. Future-self | PASS | Strong pickup block. It names CB, pivot timing, MVP_PLAN, README, sub-genre lock, orchestrator setup, and original school/RIMA context. |
| 1. Vision | PASS | One-sentence vision is crisp and aligned with the genre lock. |
| 2. Sub-genre lock | PASS | "Real-Time Generative Action Roguelike" is explicit, locked, and semantically decomposed. |
| 2.1 Market place | PASS | Correctly positions CB as rare combination, not empty niche. |
| 2.2 Public labels | PASS | Public/internal label split is useful and not overcomplicated. |
| 2.3 Capsule lines | PASS | Strong player/streamer/press language. |
| 3. Design pillars | PASS | Pillars are stable and map cleanly to gameplay systems. |
| 4. Player fantasy | PASS | Clear, memorable, and actionable. |
| 5. Core loop | PASS | Second-to-second, room, run, and meta loops are readable. |
| 6. Combat grammar | PASS | Input and trigger/drop grammar are clear. |
| 7. Environmental Cascade Combat | PASS | Good internal pattern name and strong guardrails. |
| 8. Element taxonomy | REVISE | 5 to 11 element expansion is coherent, but downstream tile/status taxonomy does not fully absorb the extra elements. |
| 9. Tile-state taxonomy | REVISE | MVP state list is good, but post-MVP tile-state expansion is underspecified relative to 11 elements. |
| 10. Hybrid taxonomy | REVISE | Claims around "~12 hybrid" are not backed by enough named hybrids. |
| 11. Status hybrid taxonomy | PASS | MVP status hybrids are clear; Faz 2 additions are reasonable. |
| 12. Trigger weapon philosophy | PASS | Universal trigger plus class affinity is well justified. |
| 13. Class system philosophy | PASS | RoR2 chassis + Form ultimate is clear. |
| 14. Full class roster vision | PASS | 3 to 5 to 8 to 12+ class progression is conceptually coherent. |
| 15. Skill variant philosophy | REVISE | Variant count is inconsistent with Phase 2 class count. |
| 16. Form ultimate philosophy | PASS | Cooldown, duration, and identity are explicit. |
| 17. Act/floor structure | PASS | MVP and long-term structures are understandable. |
| 18. Atlas/endgame vision | REVISE | Vision is exciting, but Phase 4 scope needs guardrail language so it does not read like guaranteed delivery. |
| 19. Voltage vision | PASS | Strong Heat/map-mod analogue with phased scope. |
| 20. Currency vision | PASS | Spark/Cinder/Echo split is clean. |
| 20.4 Item/Loot open decision | PASS | Three options are clear and the Faz 2 playtest boundary is strong. |
| 21. Relic/modifier vision | PASS | SO + interpreter is appropriate and anti-scope-creep. |
| 22. Enemy family vision | PASS | MVP and later family growth is readable. |
| 23. Boss design vision | PASS | Boss rules are actionable and tied to arena state. |
| 24. Art direction | PASS | Pixel/top-down/PPU/color grammar are clear. |
| 25. VFX readability | PASS | Rules protect tile readability. |
| 26. UI/HUD philosophy | PASS | HUD essentials and banned UI patterns are useful. |
| 27. Audio fantasy | PASS | Audio pillars support cascade payoff. |
| 28. Market positioning | PASS | Tags, price, KPI, and market notes are useful. |
| 29. Anti-clone guardrails | REVISE | Good base, but ARPG clone risks need explicit coverage. |
| 30. Long-term content phases | REVISE | Timeline overlaps and Phase 2 load are the biggest coherence problem. |
| 31. Explicit non-goals | PASS | Strong exclusion list. |
| 32. Decision log | REVISE | Mostly good, but missing important locks and includes body-weak decisions. |
| Folder structure | PASS | Documented and future-self useful. |
| Lifecycle block | PASS | Good maintenance rules, especially "never change" lock lines. |

## Criteria verdicts

| Criterion | Status | Verdict |
|---|---|---|
| 1. Sub-genre lock clarity | PASS | Lock is clear and user override is respected. |
| 2. Future-self pickup benefit | PASS | Strong restart map for 1-6 months later. |
| 3. Item/Mastery tracking | PASS | Clear open decision with three options and playtest boundary. |
| 4. Anti-clone coverage | REVISE | Missing Last Epoch/Diablo 4 style ARPG risks. |
| 5. Phase progression coherence | REVISE | Phase 2 timing and content load conflict. |
| 6. Class/Element/Tile taxonomy expansion | REVISE | Conceptual chain is good, enumeration is incomplete. |
| 7. Decision log completeness | REVISE | Missing locks/open markers for several body decisions. |
| 8. Format + readability | REVISE | Mostly readable, but numbering and internal-doc symbol usage need cleanup. |

## Critical issues (MUST FIX before save)

1. **Anti-clone table needs ARPG-adjacent risks.**

Current Section 29 covers Hero Siege, RoR2, Magicraft, Noita, Hades, Magicka, and Vampire Survivors.

That is a good first set.

However the review prompt explicitly asks whether risks like Last Epoch and Diablo 4 are missing.

They are missing.

This matters because CB uses:

- class fantasy
- elemental builds
- Act/floor structure
- modifier progression
- possible light loot
- possible mastery tree
- ARPG-lite market positioning

Without explicit Last Epoch/Diablo 4/PoE-style mitigation, the anti-clone section under-protects the biggest scope-drift path.

Required fix:

- Add at least Last Epoch and Diablo 4 to Section 29.2, or add one combined row for "modern ARPGs: Diablo 4 / Last Epoch / PoE".
- State the mitigation in CB terms: terrain authorship, manual trigger, short run structure, no permanent loot treadmill in MVP, and no passive tree until playtest demands it.

2. **Phase 2 timing overlaps or conflicts with MVP timing.**

Section 30 says:

- Phase 1: Week 1-16
- Phase 2: Months 4-6
- Phase 3: Months 6-12
- Phase 4: Months 12+

Week 1-16 is already roughly 4 months.

So "Phase 2 - Months 4-6" begins at the end of or during MVP, depending on how counted.

This is ambiguous for a future-self doc.

Required fix:

- Rename Phase 2 to "Post-MVP Months 1-3" or "Months 5-8 from project start".
- Or explicitly say Month 4 is overlap/stabilization, not new content production.

3. **Phase 2 content load is too dense for the stated 3-6 month window.**

Phase 2 currently includes:

- 2 new classes
- skill variant unlock system
- 36 variants
- 25-30 modifiers
- Act 2
- Act 2 boss
- Voltage 0-8
- new enemy families
- item/mastery decision point

That is not impossible as long-term ambition, but it is not coherent as a clean 3-month phase unless the document marks it as stretch-heavy.

The highest-risk part is "5 class + skill variant + 2 Act" together.

Required fix:

- Either split Phase 2 into Phase 2A/2B.
- Or mark variants/new classes/Act 2 as prioritized gates.
- Or reduce Phase 2 promise to one major axis: content expansion OR variant system OR second Act.

4. **Skill variant count conflicts with Phase 2 class count.**

Section 15.2 says:

- "Every class x 4 skill x 3 variant = 12 variant/class"
- "3 class x 12 = 36 variant Faz 2 sonu"

Section 30 says Phase 2 has 5 classes.

If variants apply to every class, Phase 2 would be 5 x 12 = 60 variants.

If variants apply only to MVP 3 classes, the document must say that clearly.

Required fix:

- Choose one wording:
- "Phase 2 variants cover MVP 3 classes only: 36 variants."
- Or "Phase 2 has 5 classes and 60 variants, with new classes receiving minimal default kits first."

No new design decision is needed here; the current text just needs internal consistency.

5. **Hybrid taxonomy count is not fully backed.**

Section 10 says:

- MVP 3 hybrids
- Faz 2-3 list contains 6 more named hybrids
- Total named = 9
- Then it claims "~12 hybrid"

The chain from MVP to vision is not complete.

Required fix:

- Either list 3 additional example hybrids.
- Or change the claim from "~12 hybrid" to "~9+ hybrid".

6. **Tile-state expansion is weaker than element expansion.**

Section 8 expands elements from 5 to 11.

Section 9 locks MVP 7 base tile states but does not give a matching post-MVP tile-state taxonomy.

Later sections mention acid, magnetic, poison, gas, lava, and dark zones, but the taxonomy section itself does not consolidate them.

Required fix:

- Add "Faz 2-3 tile-state vision" under Section 9.
- Include at minimum Acid, Magnetic, Poison/Web, Gas, Lava, Dark.

7. **Decision Log misses important body-level decisions.**

Section 32 is useful, but it does not fully reflect the doc.

Missing or underrepresented decisions include:

- Sub-genre public label/capsule split.
- No auto-combat/manual skill ceiling as locked identity.
- Pure top-down 2D pixel art as locked art direction.
- MVP has no skill variants.
- Item/loot/mastery open boundary explicitly tied to Phase 2 playtest.
- Atlas is Phase 4 vision, not guaranteed MVP/post-MVP commitment.
- Anti-clone guardrails as locked protection.

Required fix:

- Add rows for the missing locks.
- Make sure OPEN/TARGET/LOCKED markers are consistently used.

8. **Some Decision Log rows are weakly grounded in the body.**

Examples:

- "RIMA Map Designer port" appears as a decision log item but is not clearly explained in the main sections.
- "Pivot timing = Option C Hybrid" appears in future-self notes, but not as its own body section.
- "Karar #143 6-layer composition SIL" is a legacy-cleanup decision but the doc does not explain why it belongs in CB vision.

Required fix:

- Either add a short body note for these items.
- Or move them out of VISION_DOC Decision Log into pivot/migration docs.

9. **Section numbering is not fully aligned with the review spec.**

The review spec refers to Section 0.

The draft has a future-self block, but it is not numbered "0".

The folder structure and lifecycle blocks are also outside the 1-32 numbering.

This is not fatal, but for a long-term truth document it creates reference drift.

Required fix:

- Rename future-self block to "## 0. FUTURE-SELF PICKUP NOTU".
- Decide whether folder structure and lifecycle are appendices or numbered sections.

## Minor suggestions (nice-to-have)

1. In Section 2, explicitly say "Cascade ARPG label was rejected by user override" once.

The Decision Log already says it.

Putting one respectful sentence in Section 2 would make the override visible at the lock point.

2. In Section 5.3, "100 run hedefi = 35-40 saat" is useful, but 100 x 20-25 min is 33-42 hours.

The current line is close enough.

No change required unless you want arithmetic precision.

3. In Section 8, "Gas" and "Volatile" risk naming collision.

Gas tile and Volatile hybrid/status concepts can coexist, but the text should keep them visually distinct.

4. In Section 13.1, class kit lists "1 Drop/Trigger control" while Section 6 says RMB is universal current trigger drop.

This can read as class-bound if scanned quickly.

Clarify that class kit includes skills, while drop/trigger remains universal.

5. In Section 17.2, "2 Act linear" is understandable, but "Act 1+Act 2 linear run" would be clearer.

6. In Section 18.3, "Run 1-30" style milestones are useful but may read too exact.

Consider marking them as target bands, not promises.

7. In Section 20.4, "Mastery tree (PoE-lite)" should emphasize that this is not chosen.

It is already marked defer/open, but one extra warning would prevent future scope drift.

8. In Section 21.2, "100+ modifiers" for Phase 4 is fine as vision.

Mark it as content-pipeline dependent.

9. In Section 24.2, element color binding is strong.

But color blind support in Section 26 should cross-reference symbol/shape binding.

10. In Section 28.1, market positioning uses concrete sales numbers.

If these came from a prior benchmark, link or cite the market reference doc later.

11. In Section 28.3, "Loot" as banned primary tag is correct until item economy exists.

If light loot is chosen later, this row will need review.

12. In Section 30, "Steam Workshop / mod support" and "Co-op" are correctly optional.

Keep them optional and do not move them into earlier phases.

13. In Section 31, "Sandbox/editor" conflicts slightly with possible mod support.

This is fine if the distinction is "no player-facing editor before Phase 4+".

14. In the folder structure block, "06_VISION_DOC.md" is clear.

After this review passes, ensure the staged draft gets saved to that filename, not only left in STAGING.

15. The doc mixes Turkish and English heavily.

That is acceptable for internal design, but public-facing strings should later be extracted into clean English/Turkish variants.

## Anti-clone coverage gaps

- **Last Epoch risk:** class mastery, skill specialization, elemental build identity, and ARPG progression fantasy.

Why it matters:

Last Epoch's risk is not just loot.

It is "my class skill tree defines my build".

CB must protect "arena state + manual trigger defines my build moment" from becoming pure skill-tree optimization.

Suggested mitigation target:

CB skill depth should modify terrain authorship and trigger behavior, not replace arena reading.

- **Diablo 4 risk:** class fantasy, seasonal power chase, legendary affix loop, and glossy ARPG mass-clear.

Why it matters:

CB already promises 30-50 mob peaks and $14.99 ARPG-lite positioning.

Without a Diablo-style guardrail, future item/mastery pressure can pull the project into permanent loot treadmill expectations.

Suggested mitigation target:

No permanent gear treadmill in MVP; run builds should be readable through modifiers, terrain, and trigger choices.

- **PoE risk:** Atlas, map mods, passive-web temptation, and build spreadsheet drift.

Why it matters:

The doc uses PoE Atlas and map-mod analogues.

That is fine as inspiration, but the anti-clone section should state what CB will not copy.

Suggested mitigation target:

Atlas is route/content selection, not an economy/trade/passive-tree simulator.

- **Hero Siege risk is covered, but should be sharpened.**

Current mitigation is good.

It should explicitly mention no full loot economy before terrain grammar proves itself.

- **RoR2 risk is covered, but item pickup feel remains open.**

Current Section 20.4 handles this well.

Anti-clone section should cross-reference that decision boundary.

- **Magicraft risk is covered.**

The "builds spells vs builds rooms" sentence is strong.

- **Noita risk is covered.**

The controlled/discrete distinction is strong.

- **Hades risk is covered.**

The setup-payoff distinction is strong.

- **Magicka risk is covered.**

The "recipes on the floor" distinction is strong.

- **Vampire Survivors risk is covered.**

The auto-combat/passive expectation guardrail is strong.

## Eksik/Yanlis decisions (Decision Log)

- Missing: **Manual skill / no auto-combat LOCKED.**

This appears in pillars, input scheme, and non-goals.

It deserves a Decision Log row.

- Missing: **2D pure top-down pixel art LOCKED.**

Art direction locks are important enough for Decision Log.

- Missing: **MVP has no skill variants LOCKED.**

Section 15.1 says this clearly.

Decision Log should record it because it prevents Phase 1 scope creep.

- Missing: **Atlas is Phase 4 vision only TARGET/OPEN, not committed production scope.**

Section 18 and 30 imply this, but the log should protect it.

- Missing: **Anti-clone guardrails LOCKED.**

Section 29 says hard rules.

Decision Log should make this governance-level.

- Missing: **No full loot economy in MVP LOCKED.**

Section 20.4 says MVP has no item/loot decision.

Decision Log only says Item/Mastery tree is DEFER/OPEN.

Add an explicit "MVP no loot system" row if that is intended.

- Missing: **Public-facing label split LOCKED or TARGET.**

"Real-Time Generative Action Roguelike", "Battlefield Alchemy Roguelike", and "Environmental Cascade Combat" have different roles.

This split is useful and should not drift accidentally.

- Potentially wrong/weak: **RIMA Map Designer port row.**

It may be true, but it is not developed in this VISION_DOC body.

Either ground it or move it to migration docs.

- Potentially wrong/weak: **Karar #143 cleanup row.**

It is a historical cleanup note.

It may belong in pivot decision memory, not long-term CB vision.

- Potentially ambiguous: **Year 1 target 100K sales uses TARGET.**

This marker is correct.

Keep it as TARGET, not LOCKED.

- Correct: **Item/Mastery tree = OPEN.**

The marker is correct because the doc defers it to Phase 2 playtest.

- Correct: **Sub-genre = LOCKED (user).**

This marker is correct and should stay.

## Detailed criterion notes

### 1. Sub-genre lock clarity - PASS

The title-level identity is consistent.

Section 1 says:

- Circuit Breaker - Real-Time Generative Action Roguelike.

Section 2 says:

- Type name LOCK.
- This sentence is final.
- Cannot pivot.

The semantic table is useful:

- Real-Time = not turn-based.
- Generative = player builds arena state.
- Action = manual aim/dash/timing.
- Roguelike = run-based.

The public-facing split is also good.

The only improvement is to surface the "Cascade ARPG was overridden by user lock" line in Section 2 itself, not only in Decision Log.

Status remains PASS because the lock is not ambiguous.

### 2. Future-self pickup benefit - PASS

The 9-item pickup note succeeds.

It answers:

- What project is this?
- Where does it live?
- Is this before or after RIMA?
- Which document is vision vs MVP vs roadmap?
- What is the non-negotiable genre lock?
- What should the user open next?
- Where is the folder map?
- What technical orchestration carries over?
- What was the original school-project context?

This is enough for a 6-month return.

The only formatting fix is numbering it as Section 0.

### 3. Item/Mastery decision tracking - PASS

Section 20.4 is one of the strongest parts of the draft.

It clearly names three approaches:

- No loot.
- Light loot.
- Mastery tree.

It states the anti-clone risk:

- Full loot economy can turn into Hero Siege clone.

It also captures the user-requested RoR2/Megabonk pickup feel:

- Chest/box/drop spectacle and map exploration pleasure.

The boundary is good:

- No decision in MVP.
- Decide at Phase 2 using playtest data.

This should stay.

### 4. Anti-clone coverage - REVISE

Section 29 is directionally strong.

The hard rules are useful and specific.

The game-based table has good mitigation language.

The problem is coverage.

The table misses the exact ARPG risks that become dangerous once CB includes:

- Act structure.
- Class roster.
- Skill variants.
- Possible mastery tree.
- Possible light loot.
- Atlas mode.

Last Epoch and Diablo 4 should be represented.

PoE may also need a row because Atlas and map modifiers are directly referenced.

### 5. Phase progression coherence - REVISE

Phase 1 is coherent.

Phase 3 and Phase 4 are coherent as vision.

Phase 2 is the problem.

The timing is unclear because Week 1-16 already consumes roughly 4 months.

Phase 2 "Months 4-6" either overlaps MVP or leaves only about 2 months after MVP.

The content load is also too dense.

The doc should either stretch Phase 2 or split it.

Phase 4 reads acceptable as 12+ month vision, but it should use more "target/vision" language around Atlas, 12+ classes, pinnacle bosses, and optional co-op/mod support.

### 6. Class/Element/Tile taxonomy expansion - REVISE

Class growth is coherent:

- 3 MVP.
- 5 Phase 2.
- 8 Phase 3.
- 12+ Phase 4.

Element growth is coherent:

- 5 MVP.
- 11 by Phase 3.

Tile and hybrid growth are less coherent.

The doc should connect added elements to tile states and hybrid states more explicitly.

The hybrid count especially needs correction.

If the document claims 12 hybrids, it should name enough examples or say the rest are future fill.

### 7. Decision Log completeness - REVISE

The Decision Log is useful and mostly aligned.

It has 22 rows and captures many major decisions.

But it is not complete enough for a truth document.

The missing rows are not minor; they are the locks that protect scope:

- no auto-combat
- no MVP variants
- 2D pure top-down
- no MVP loot
- anti-clone guardrails

Also, legacy/migration items need grounding or relocation.

### 8. Format + readability - REVISE

Markdown is generally clean.

Tables are readable.

The Turkish/English mix is acceptable for an internal document.

The main problems:

- Future-self block is not numbered Section 0.
- Folder structure and lifecycle are outside the numbered structure.
- Some headings use emoji/symbol markers that may conflict with internal ASCII-only discipline.
- Some claims use exact numbers while the backing list is incomplete.

This is a cleanup-level REVISE, not a structural failure.

## Final verdict

- [ ] APPROVE - User CB klasorune tasiyabilir
- [x] REVISE - Su 3-5 critical fix gerekli
- [ ] FAIL - Major rework gerekli

Minimum revise set:

1. Add Last Epoch / Diablo 4 / PoE-style ARPG anti-clone rows.
2. Fix Phase 2 timeline overlap and content load.
3. Fix skill variant count vs 5-class Phase 2.
4. Fix hybrid/tile taxonomy count/list mismatch.
5. Complete Decision Log with missing locks and clean weak legacy rows.

After those edits, likely verdict becomes PASS.

CB VISION_DOC REVIEW COMPLETE
