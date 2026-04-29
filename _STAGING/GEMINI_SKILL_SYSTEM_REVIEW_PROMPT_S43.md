# Gemini Task — RIMA Skill System Review S43

You are Gemini running as an independent design reviewer for the RIMA Unity project.

## Hard Scope

Perform only this task:

Review RIMA's character skills, mob skills, and boss skills for design consistency with the current game concept. Write one markdown report to:

`F:/Antigravity Projeler/2d roguelite/RIMA/_STAGING/SKILL_SYSTEM_FEEDBACK_GEMINI_S43.md`

Do not edit any other file.
Do not modify source code.
Do not modify design docs.
Do not move, delete, rename, or archive anything.
Do not create additional files.

## Project Goal To Use As Evaluation Lens

RIMA is a top-down 2D roguelite action game:

- Hades-like room combat
- Slay the Spire-like skill draft
- MMORPG-style class identity and dual-class build crafting
- Act 1 starts with one primary class
- Act 1 boss unlocks secondary class
- Run fantasy: "this build is insane"
- Combat should be fast, readable, telegraphed, and build-driven
- Generic MMO filler skills are bad
- Every mob should ask a gameplay question
- Boss phases should teach and escalate mechanics, not just increase stats

## Files To Read

Read these first:

1. `F:/Antigravity Projeler/2d roguelite/RIMA/CURRENT_STATUS.md`
2. `F:/Antigravity Projeler/2d roguelite/RIMA/SYSTEM_MAP.md`
3. `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/GDD.md`
4. `F:/Antigravity Projeler/2d roguelite/RIMA/_STAGING/SKILL_REVIZYON_PLANI.md`
5. `F:/Antigravity Projeler/2d roguelite/RIMA/_STAGING/MOB_BOSS_REDESIGN_S42.md`
6. `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`
7. `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/CROSS_CLASS_SKILL_MATRIX.md`
8. `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/COMBAT_ROSTER.md`
9. `F:/Antigravity Projeler/2d roguelite/RIMA/TASARIM/BOSS_DESIGN.md`

Also inspect these folders briefly:

10. `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Data/Skills/`
11. `F:/Antigravity Projeler/2d roguelite/RIMA/Assets/Data/CrossClass/`

Avoid archived folders unless one of the active files explicitly says an archive file is the current source. Do not read `Library/` or `PackageCache/`.

## Current Source Priority

If documents conflict, use this priority:

1. `CURRENT_STATUS.md`
2. `_STAGING/SKILL_REVIZYON_PLANI.md`
3. `_STAGING/MOB_BOSS_REDESIGN_S42.md`
4. `TASARIM/GDD.md`
5. `TASARIM/SINIF_VE_SKILL_KARAR_BELGESI.md`
6. `TASARIM/CROSS_CLASS_SKILL_MATRIX.md`
7. `TASARIM/COMBAT_ROSTER.md`
8. `TASARIM/BOSS_DESIGN.md`
9. Unity asset files

Important: `_STAGING/SKILL_REVIZYON_PLANI.md` and `_STAGING/MOB_BOSS_REDESIGN_S42.md` are newer than several main TASARIM files. Treat older docs as potentially stale and call out drift clearly.

## What To Evaluate

### 1. Character Skills

For each class:

- Warblade
- Elementalist
- Shadowblade
- Ranger
- Ravager
- Ronin
- Gunslinger
- Brawler
- Summoner
- Hexer

Evaluate:

- Does the skill kit match the class fantasy?
- Are there generic MMO filler skills?
- Are there too many overlapping skills?
- Are there skills that conflict with RIMA's fast top-down roguelite combat?
- Are there skills that are too slow, too passive, too stat-buff-like, or too hard to read in pixel art?
- Which skills should be kept?
- Which skills should be renamed, removed, or redesigned?
- If you propose a replacement skill, describe it in one compact paragraph.

### 2. Mob Skills

Evaluate the Act 1 mob redesign in `_STAGING/MOB_BOSS_REDESIGN_S42.md`:

- Fracture Imp
- Shard Walker
- Seam Crawler
- Penitent Bruiser
- Chain Warden Echo
- Relic Caster
- Riftbound Augur
- Hollow Hulk

Check:

- Does each mob ask a unique gameplay question?
- Is any mob too generic?
- Are any skills unfair, unclear, or too hard to telegraph?
- Are any mob combinations likely to create unavoidable damage?
- Should any mob skill be changed to better challenge Warblade first, then later ranged classes?

Also compare briefly against `TASARIM/COMBAT_ROSTER.md` and call out which version should be active.

### 3. Boss Skills

Evaluate:

- Penitent Sovereign current 3-phase redesign from `_STAGING/MOB_BOSS_REDESIGN_S42.md`
- Older 2-phase Penitent Sovereign from `TASARIM/BOSS_DESIGN.md`
- Echo Twin concept
- Fracture Sovereign concept
- Architect final boss concept

Check:

- Which boss version fits the game better?
- Are there overlapping attacks that could create unfair unavoidable situations?
- Are phase changes meaningful?
- Does each phase teach/escalate a different decision?
- Which attacks should be cut or changed?

### 4. Cross-Class System

Evaluate `TASARIM/CROSS_CLASS_SKILL_MATRIX.md` against the newer skill revision plan.

Check:

- Which cross-class rows are stale?
- Which skill names no longer match current design?
- Which cross-class ideas are still good?
- Which should be rebuilt from scratch?
- Propose a better method for rebuilding the matrix.

### 5. Implementation Risk

Check current Unity skill assets:

- `Assets/Data/Skills/`
- `Assets/Data/CrossClass/`

Report:

- Which assets look stale or old compared to current docs?
- What should be implemented first?
- What should not be implemented yet?

## Output Format

Write the report in Turkish.

Use this exact structure:

```markdown
# RIMA Skill System Feedback — Gemini S43

**Date:** 2026-04-28
**Author:** Gemini
**Scope:** Character skills, mob skills, boss skills, cross-class consistency.

## Executive Verdict

## Source Drift / Document Conflicts

## Character Skill Review

### Warblade
...

### Elementalist
...

(repeat all 10 classes)

## Mob Skill Review

## Boss Skill Review

## Cross-Class Review

## Implementation Risk

## Highest Priority Changes

## Final Recommendation
```

## Report Rules

- Be direct and critical.
- Do not be polite if a skill is generic or stale.
- Prefer actionable recommendations.
- Do not rewrite the entire design bible.
- Do not invent a completely new game.
- Respect the current RIMA direction.
- If a skill is good, say keep it.
- If a skill is bad, say why and propose a replacement.
- If a document is stale, name the exact file.
- If a recommendation depends on playtest, label it as "PLAYTEST NEEDED".

## Final Check Before Writing

Before saving the report, verify:

- The output file path is exactly `_STAGING/SKILL_SYSTEM_FEEDBACK_GEMINI_S43.md`
- No other files were edited
- All 10 character classes are covered
- Mob, boss, cross-class, and implementation risk sections are present

