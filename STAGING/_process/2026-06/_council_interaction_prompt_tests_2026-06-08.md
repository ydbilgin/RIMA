# COUNCIL — Interaction-Prompt Convention + Test Automation (2026-06-08)

**Source:** ChatGPT design pack `STAGING/_inbox/character_room_sketches_2026-06-08/` (CLAUDE_PROMPT.md + INTERACTION_PROMPT_TEST_AUTOMATION.md + UI_PROMPT_STANDARD.md). User asked: "test otomasyonları yapabiliriz bunu council'le yine düşün."

**This council decides ONLY the interaction-prompt + test-automation question.** Room layout / pedestal-scale / asset-language items from the pack are a SEPARATE design workstream — out of scope here.

---

## GROUND TRUTH (Explore agent, file:line verified — do NOT re-derive, reason FROM this)

Localization shipped 2026-06-07 (commit `934bff11`), `Assets/Scripts/Core/Loc.cs`: `Loc.T(key, args...)`, two hardcoded C# dicts `_tr`/`_en` (~96 keys), PlayerPrefs `rima.lang`.

**CURRENT CONVENTION = "A" (key token baked INTO the loc string):**
6 loc keys carry an input-key token, identical token in BOTH languages:
- `chamber_select.prompt.attune` = `[G] Bürün — {0}` / `[G] Attune — {0}`
- `chamber_select.prompt.unlock` = `[G] Kilidi Aç — {0} ...` / `[G] Unlock — {0} ...`
- `chamber_select.prompt.enter_rift` = `[G] Rift'e Gir` / `[G] Enter Rift`
- `combat.prompt.execute` = `[RMB] İnfaz` / `[RMB] Execute`
- `reward.prompt.take` = `[G] Ödülü Al` / `[G] Take Reward`
- `death.btn.retry` = `TEKRAR DENE [R]` / `RETRY [R]`

**Call sites:** 5 of 6 assign `Loc.T(...)` DIRECTLY to a TMP label → no concat → CORRECT, no dup.
(ChamberSelectBootstrap.cs:216-243, ExecutePromptDriver.cs:68/88, RewardPickup.cs:98)

**THE ONE REAL BUG (reproducible):** `RewardPickup.cs:100` passes `Loc.T("reward.prompt.take")` (already `[G] Ödülü Al`) into `HUDController.SetInteractionPrompt(actionName)`, which at `HUDController.cs:319` does `interactionText.text = $"[G] {actionName}"` → **`[G] [G] Ödülü Al`**. Only HUD-routed reward prompt is affected.

**`InteractionPromptFormatter` does NOT exist.** No central formatter; no UI-text lint tests. Only `Assets/Tests/Contracts/UIFlowContract*` (pause-restore pairs).

**ChatGPT's other 4 diagnoses are STALE / contradicted by code:**
- "Pedestals all show Warblade" → FALSE. `ChamberSelectBootstrap.cs:945-954` `LoadClassIdleSouthSprite(cls)` binds per-class `idle_south`; missing → GENERIC silhouette (NOT Warblade). *Caveat: if 9/10 class sprites are absent on disk, all 9 render the SAME generic silhouette → could LOOK like "repeated" in play. Needs a disk check.*
- "Combat HUD visible in chamber" → FALSE. CharacterSelect scene has no HUD; `HUDController` only exists in `_Arena`; all `HUDController.Instance?.` calls are null-guarded.
- "Pedestal too big / room too wide" → layout/design item, NOT a test-automation item.

So of ChatGPT's 5 listed suggestions: **#5 (dup-key test) targets a REAL bug; #3+#4 (formatter / strip keys) are a real convention choice; #1 (pedestal identity) + #2 (HUD off) appear already-satisfied in code** (verify #1 by sprite-existence check).

---

## PIVOTAL FORK — pick convention, it dictates the tests

**Convention A (KEEP current):** key token lives in the loc string; callers assign `Loc.T()` directly. Fix = make the ONE prepending path (`HUDController.SetInteractionPrompt`) key-aware (don't re-prepend if text already has a token), OR feed it action-only text. Minimal churn.
- Tests that fit A: "final prompt has EXACTLY ONE key token", "`SetInteractionPrompt` never double-prepends".
- Con: input keys (non-translatable) are duplicated across TR+EN; rebinding G→F = edit every string ×2 langs; "lint: strings carry no key" is INVERTED (strings are SUPPOSED to carry keys).

**Convention B (ChatGPT — flip it):** loc strings = action text only; ONE `InteractionPromptFormatter.Format(keyToken, locText)` adds the key everywhere. Edit 6 strings ×2 langs + all call sites + new formatter.
- Tests that fit B: "no loc string contains a key token" (clean invariant), formatter unit tests (idempotent, strips a stray baked token + warns), "exactly one token in final prompt".
- Pro: keys (physical, non-translatable) separated from translatable text; single source; dup-bug becomes structurally impossible; lint invariant is clean & strong; standard i18n practice.
- Con: moderate refactor right before demo; touches the just-shipped Loc tables.

---

## QUESTIONS FOR EACH ADVISOR

1. **A or B?** Given: bilingual already shipped, no key-rebinding feature exists yet, demo-bound, project rule = minimal/surgical change. Is B's cleanliness worth the churn, or is A + a key-aware prepend + targeted lint enough? Give a decisive pick + 1-2 line why.
2. **Exact test set** under your chosen convention: which EditMode + PlayMode test files, and the 4-8 highest-value assertions (avoid redundant/low-ROI tests). Where do strings get enumerated for the lint (reflect over `Loc._tr`/`_en`? expose a test hook?) — propose the mechanism.
3. **Scope of build:** besides the 1 bug + tests, should we ALSO add a regression-guard test for pedestal-identity (#1) and HUD-off-in-chamber (#2) since code already satisfies them, or is that wasted effort? 
4. **Sprite-existence reality check on #1:** worth a test asserting each of the 10 classes resolves a DISTINCT sprite (not all the same generic silhouette)? Or out of scope?

Keep it lean. We want a buildable decision, not an essay.
