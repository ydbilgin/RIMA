# T7 reward-chip (DATA-01) — ADVERSARIAL COUNCIL (writer was cx; you are a DIFFERENT reviewer)

ACTIVE RULES: (1) think before judging (2) min code (3) surgical (4) BLOCKED if unclear.
You are HOSTILE. Do NOT approve reflexively. Read the actual changed code + find a regression/bug. Report <=12 lines to AGY_DONE: VERDICT (PASS/FAIL/RISK) + concrete findings + exact fix if any. Do NOT run Unity (it already compiles 0-error); code review only.

## Context (RIMA demo, 19 June; 8-dir canon LOCKED, surgical no-refactor)
cx implemented T7 (DATA-01): reward draft card "chip" must show a skill's trigger/outcome from the CANONICAL SkillData ScriptableObject, not a hard-coded pairs-with table.

cx report: `.cx_dispatch/CODEX_DONE_T7_reward_chip_impl_bugra_20260616-182918.md`
Changed files: `Assets/Scripts/UI/SkillOfferUI.cs`, `Assets/Scripts/Skills/SkillData.cs`, `Assets/Scripts/Skills/SkillDatabase.cs`, `Assets/Scripts/Core/Loc.cs`.
cx root-cause: chip used `ChainWindowTracker` static producer/consumer table; cx added draftTriggerText/draftOutcomeText/draftComboText to SkillData, SkillDatabase stamps draftOutcomeText from SO description, SkillOfferUI renders DraftInfoChip from SkillData/SO; old static pair chips removed. cx data-proof: Gravity Cleave chip resolved from cooldown/isPassive + description.

## Attack vectors — verify in the REAL code, be specific
1. **Mechanic vs display:** Did removing the static pair chip touch `ChainWindowTracker`'s actual COMBO MECHANIC (gameplay), or ONLY the card display? The combo system must still WORK; only the chip text source changed. Grep ChainWindowTracker usages — is the producer/consumer table still consumed by gameplay?
2. **Serialization:** 3 new fields on SkillData (a ScriptableObject?) — do existing SkillData assets still load? Any null/default issue? Is SkillData a SO or a plain class (affects asset impact)?
3. **Runtime stamping:** SkillDatabase stamping draftOutcomeText from description at runtime — when does it run? Any ordering/null risk (description empty)? Does it mutate shared SO state (cross-run leak)?
4. **Fallback:** cx flagged "empty draftOutcomeText -> description fallback until assets restamped." Is the fallback correct/legible for the demo's actual draft skills? Any skill that would now show blank/wrong?
5. **Loc keys:** TR+EN fallback keys added — do they resolve, or risk showing raw keys on the card?
6. **Scope:** Did cx touch anything beyond the chip (refactor creep in SkillData/SkillDatabase)?

Report VERDICT + findings.
