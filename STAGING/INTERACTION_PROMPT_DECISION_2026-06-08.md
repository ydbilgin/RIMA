# DECISION — Interaction-Prompt Convention + Test Automation (2026-06-08)

**Trigger:** ChatGPT design pack (`STAGING/_inbox/character_room_sketches_2026-06-08/`) proposed a duplicate-key fix + ~15 tests. User: "test otomasyonları yapabiliriz bunu council'le yine düşün." Council = cx + ax-3.1-Pro + ax-3.5-Flash → Opus synth. Grounded brief: `_process/2026-06/_council_interaction_prompt_tests_2026-06-08.md`. **User picked Convention A.**

## DECISION

**Convention A (LOCKED).** Key token stays baked in the loc string; the ONE prepending path is made token-aware. Reject the formatter refactor (B) for now — revisit B only when a key-rebinding / input-display feature is actually built. Council 2-1 for A (cx + Flash; 3.1-Pro argued B). PROJECT_RULES (surgical, no speculative abstraction) reinforce A.

## REAL BUG (only one — verified file:line)
`RewardPickup.cs:100` passes `Loc.T("reward.prompt.take")` (already `[G] Ödülü Al`) into `HUDController.SetInteractionPrompt`, which at `HUDController.cs:319` does `$"[G] {actionName}"` → **`[G] [G] Ödülü Al`**. Only the HUD-routed reward prompt is affected; 5 other prompt sites assign `Loc.T()` directly and are correct.

## FIX (surgical)
Make `HUDController.SetInteractionPrompt` token-aware: if the supplied text already starts with a bracket token (`[...]`), do NOT prepend `[G] `. Extract the compose logic into a pure testable helper. ~5-10 lines, zero loc-table changes, zero other call-site changes.

## TESTS (EditMode only — cx's set; PlayMode rejected as low-ROI)
`Assets/Tests/EditMode/InteractionPromptConventionTests.cs`:
- `PromptLocTables_TokenBearingKeysAreExactlyTheApprovedSet` (only the 6 approved keys carry a bracket token)
- `PromptLocTables_ApprovedPromptKeysHaveExactlyOneTokenInTrAndEn`
- `PromptLocTables_ApprovedPromptTokensMatchBetweenLanguages`
- `HudSetInteractionPrompt_RawActionAddsExactlyOneGToken`
- `HudSetInteractionPrompt_TokenizedActionDoesNotDoublePrepend`
- `RewardPickupHudRoute_LocalizedTakeRewardEndsWithExactlyOneGToken`

`Assets/Tests/EditMode/CharacterIdleSouthAssetTests.cs`:
- `ClassIdleSouthSprites_AllTenClassPathsExistOnDisk` (cheap regression guard against future asset deletion)

**Lint mechanism:** reflection over `Loc._tr`/`_en` (`BindingFlags.NonPublic|Static`), bracket-token regex. Under A the invariant is INVERTED: only the approved 6 keys may contain tokens, exactly one each, matching TR↔EN. No `InternalsVisibleTo` / `Loc.AllKeys()` production change.

## DISPOSITION OF ChatGPT'S 6 CLAIMS (ground-truth verified)
1. "Pedestals all show Warblade" → **STALE/FALSE.** Per-class `idle_south` binding (ChamberSelectBootstrap.cs:945-954); **cx disk check: 10/10 classes have the sprite, ZERO fallback.** No fix needed.
2. "Combat HUD visible in chamber" → **STALE/FALSE.** No HUD in CharacterSelect scene; HUDController only in `_Arena`; calls null-guarded. No fix needed.
3. "Single formatter adds key" (B) → **DEFERRED** (revisit with rebinding feature).
4. "Loc strings carry no key" (B) → **DEFERRED** (same).
5. "`[G] [G]` caught by tests" → **ACCEPTED** — the real bug + the test set above.
6. Pedestal scale / room width / asset language → **OUT OF SCOPE** (separate layout/design workstream; sketches in `_inbox/`).

## NOT BUILT (rejected as over-engineering)
- InteractionPromptFormatter class · stripping loc tables · PlayMode chamber prompt tests · pedestal-identity PlayMode test · HUD-off PlayMode test · distinct-sprite content test (10/10 present makes it moot; existence test covers regression).

## Advisor raw outputs
cx: `CODEX_DONE_yekta.md` · ax-3.1-Pro & ax-3.5-Flash: session transcript · brief: `_process/2026-06/_council_interaction_prompt_tests_2026-06-08.md`.
