# RIMA Skill & Mechanic Evaluation — AGY (design / player-experience / genre-benchmark lens)

ACTIVE RULES: (1) think before answering (2) dense, no filler (3) ANALYSIS ONLY — write NO project files (4) label uncertain claims; do not invent code details — read the files.

NLM ACCESS: RIMA design canon lives in NotebookLM. Query it FIRST for design-intent (class fantasy, skill philosophy, combat pillars, roguelite vision):
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
If NLM auth fails, proceed with code + web + judgment and note it. Direct-read allowed: Assets/Scripts/**, CURRENT_STATUS.md, .claude/PROJECT_RULES.md, STAGING/**.

## What RIMA is
2D top-down action-ROGUELITE ARPG (Unity, URP 2D), Hades / Children-of-Morta / Dead Cells adjacent. Cyan "seal/rift" aesthetic, floating-island arena. Run-based ~10-min vertical-slice demo (Warblade only, 5 rooms + boss "Penitent Sovereign"). Cross-class is a later phase.

## Current skill/mechanic state (code ground-truth — verify by reading `Assets/Scripts/Skills/`)
- **10 classes in enum**, only **4 implemented**: Warblade (melee rage), Elementalist (caster), Shadowblade (stealth/assassin), Ranger (bow/traps). 6 unbuilt: Ravager, Ronin, Gunslinger, Brawler, Summoner, Hexer.
- **~70+ skills implemented**. e.g. Warblade: IronCharge/GravityCleave/SunderMark/Earthsplitter/Cleave/WarStomp/DeathBlow/BladeRush... Elementalist: Fireball/Meteor/ChainLightning/Blizzard/FrozenOrb/Blink/MirrorImage/LivingBomb... Shadowblade: ShadowStep/Backstab/Vanish/FanOfKnives/ShadowClone/DeathMark/Rupture... Ranger: AimedShot/MultiShot/Volley/RapidFire/ExplosiveTrap/BoneTrap/TetheringArrow...
- **SkillData**: tier {Common,Rare,Epic,Mythic,Legendary}, tags {Melee,Ranged,Dash,AOE,Fire,Ice,Lightning,Void,Poison,Physical,Summon,Trap,Passive}, damage, cooldown, isPassive, appliesEffect.
- **Slots**: 4 active (Q/E/R/F) + 2 secondary (Z/X after boss). **Combo window 0.6s exists but does nothing (only logs).**
- **DraftManager** = roguelite skill draft. **RageSystem** = Warblade resource (Fury@50/Bloodrage@80, builds on hits, decays out of combat).
- **Core verbs**: cursor-aim attack, dash (i-frames), rift-break.

## Mechanic bank (consider for pickups)
`F:/LaurethStudio/03_IDEAS/MECHANIC_BANK/_MEKANIK_BANKASI.md`. Especially **KATEGORİ 10 (M59-M68 combat)**, **KATEGORİ 11 (M69-M93 composition roguelite — orbiters, baton pass, mood swings, echo strikers, prism, etc.)**, and NEW **KATEGORİ 20 (M204-M212 Mina the Hollower combat-feel)**. Which bank mechanics would make RIMA's combat/build deeper or more readable?

## Your task (design / player-experience / genre-benchmark angle)
1. **Skill quality & distinctiveness** — are the ~70 skills fun, READABLE, and meaningfully distinct, or is there filler/redundancy (e.g. how many "deal damage in a line" clones)? Benchmark against Hades (few deep skills) vs Dead Cells (many) vs Path of Exile. Is "70 skills across 4 classes" the right shape for a 10-min roguelite, or is it over-produced breadth with shallow depth?
2. **Build-variety & synergy depth** — does the draft + tag + tier system create real builds and synergies, or just a list of picks? What's missing for "I built something" feeling (e.g. synergy detection, status combos, slot caps)?
3. **Combat identity** — what is RIMA's signature combat feel/verb? Is the Warblade rage loop satisfying? Is there a missing "hook" mechanic (compare Hades cast, Dead Cells freeze-combo, Mina burrow)?
4. **Reorganization verdict** — does the skill/mechanic system need reorganization from a DESIGN standpoint? What to cut, merge, deepen, or add. Demo-scope vs full-game.
5. **Bank pickups** — top 3-5 bank mechanics (M-numbers) that would most improve RIMA, each one line + why.
6. **Your own ideas** — propose richer models or new mechanics RIMA could adopt (status-combo system, signature verb, draft-synergy layer). Be generative — go beyond the bank.

~800-1100 words, bulleted. Cite skill names. Flag uncertainty.
