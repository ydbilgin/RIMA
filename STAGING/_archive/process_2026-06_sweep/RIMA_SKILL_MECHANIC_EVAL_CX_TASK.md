# RIMA Skill & Mechanic Evaluation — CX (systems / architecture / tech-debt lens)

ACTIVE RULES: (1) think before answering (2) min output, no filler (3) ANALYSIS ONLY — write NO code, NO file edits; this is an evaluation report (4) say BLOCKED if you cannot read needed files.

NLM ACCESS: RIMA design canon is in NotebookLM. Query it FIRST for any design-intent question (class fantasy, skill philosophy, roguelite vision, combat pillars):
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
If NLM auth fails, proceed with code + your own judgment and note it. Direct-read allowed: Assets/Scripts/**, CURRENT_STATUS.md, .claude/PROJECT_RULES.md, STAGING/**.

## What RIMA is
2D top-down action-ROGUELITE ARPG (Unity, URP 2D). Hades / Children-of-Morta adjacent. Cyan "seal/rift" aesthetic, floating-island arena. Run-based ~10-min vertical-slice demo (Warblade only, 5 rooms + boss "Penitent Sovereign"). Cross-class unlock is a later phase.

## Current skill/mechanic state (code ground-truth, 2026-05-30 — verify by reading)
- **ClassType enum = 10 classes**: Warblade, Elementalist, Shadowblade, Ranger, Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer. **Only 4 IMPLEMENTED**: Warblade, Elementalist, Shadowblade, Ranger.
- **~70+ skill classes** (SkillBase subclasses) under `Assets/Scripts/Skills/<Class>/`. Warblade ~14, Elementalist ~15, Shadowblade ~22, Ranger ~20.
- **SkillData** (ScriptableObject, `Skills/SkillData.cs`): skillName, description, tier {Common,Rare,Epic,Mythic,Legendary}, icon, damage, cooldown, tags[], classType, isPassive, passiveDescription, appliesEffect(StatusEffectType). **⚠️ DUPLICATE: a second `Combat/Skills/SkillData.cs` exists — investigate which is live, flag the debt.**
- **SkillTag**: Melee, Ranged, Dash, AOE, Fire, Ice, Lightning, Void, Poison, Physical, Summon, Trap, Passive.
- **Skill controllers** (per class, e.g. `Skills/Base/Warblade_SkillController.cs`): 4 primary slots Q/E/R/F (KeyBindManager) + 2 secondary Z/X (unlock after boss). **Combo window 0.6s exists but only Debug.Logs — no real combo bonus.**
- **DraftManager** (`Skills/DraftManager.cs`): roguelite skill draft/picks. SkillDatabase + SkillOfferGenerator + SkillIconRegistry.
- **RageSystem** (`Systems/Resources/RageSystem.cs`, Warblade resource, PlayerResourceBase): +1/hit-dealt, +5/hit-taken, +3/kill, decay 10/s after 1.5s. Fury@50, Bloodrage@80. Is this the ONLY class resource? Do other classes have one?
- **Core verbs**: cursor-aim attack (`Player/PlayerAttack.cs`), dash w/ i-frames (`Player/PlayerController.cs`), rift-break. Cross-class passives (`Systems/CrossClassPassive_WB_*.cs`).

## Mechanic bank (consider for pickups)
`F:/LaurethStudio/03_IDEAS/MECHANIC_BANK/_MEKANIK_BANKASI.md` — studio mechanic vocabulary. Especially **KATEGORİ 10 (M59-M68 combat)**, **KATEGORİ 11 (M69-M93 composition roguelite)**, and the NEW **KATEGORİ 20 (M204-M212 Mina the Hollower combat-feel)**: aggression-gated heal, rolling/deferred damage bar, slot-capped loadout, sweet-spot damage, at-risk currency, environmental-vulnerability boss phase, accessibility-contract. Identify which bank mechanics RIMA's architecture should adopt and HOW (data-model impact).

## Your task (systems/architecture/tech-debt angle)
1. **Data-model audit** — is SkillData/SkillTier/SkillTag/SkillBase a clean, scalable model? The duplicate SkillData.cs. Is the draft/tier/tag system coherent? Will it scale from 4 → 10 classes without rework?
2. **Reorganization verdict** — does the skill/mechanic system need MECHANICAL reorganization? Be concrete: what to consolidate, what to cut, what to refactor. Distinguish "demo-blocker" vs "later".
3. **Resource-system coherence** — RageSystem is Warblade-specific. Do the other 3 implemented classes have/need distinct resources? Propose a clean per-class-resource architecture (or argue against it).
4. **Combo system** — the 0.6s window logs but does nothing. Either wire it to a real mechanic or cut it — recommend.
5. **Bank pickups** — top 3-5 M-bank mechanics worth adopting, each with data-model implication + effort.
6. **Your own model** — propose a cleaner/richer skill-system architecture if you see one (slot/tag/synergy/draft). Free to go beyond the list.

Be concrete, cite file paths and class names. ~700-1000 words. Distinguish CONFIRMED (you read it) from INFERRED.
