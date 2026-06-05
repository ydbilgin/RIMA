# Task — Register remaining 6 classes into SkillDatabase (data-level)

Amaç: SkillCodex/CharSelect can list all 10 classes' skills from `SkillDatabase`; today only Warblade/Elementalist/Shadowblade/Ranger are registered.

Read: `Assets/Scripts/Skills/SkillDatabase.cs` (existing 4 register helpers, pattern at :303-319) + `Assets/Scripts/UI/CharacterSelectScreen.cs:1515-1592` (placeholder skill names/descriptions for the missing six — use these as the data source).

Do:
1. Add register helpers + entries for Ronin, Ravager, Gunslinger, Brawler, Summoner, Hexer following the existing pattern. Ronin: real data assets exist under `Assets/Data/Skills/Ronin/` and scripts under `Assets/Scripts/Combat/Classes/Ronin/` — register the real ones for Ronin; placeholders for the other five.
2. CRITICAL GUARD: placeholder entries must NOT leak into run-time skill drafts. Check how `SkillOfferGenerator` pulls candidates; mark placeholder entries unimplemented/locked (add a simple flag like `isImplemented` to SkillData if needed, default true, false for placeholders) so draft generation filters them out. Codex/CharSelect listing may still show them.
3. Compile-clean check. Brief result report appended to this file's directory as `STAGING/_done_T6_skilldb.md` (what registered, how guard works, files touched). Do NOT commit — orchestrator reviews first.

Code-only, surgical: SkillDatabase.cs + SkillData.cs (+ SkillOfferGenerator only if the guard requires one line of filtering). No scene/asset edits.
