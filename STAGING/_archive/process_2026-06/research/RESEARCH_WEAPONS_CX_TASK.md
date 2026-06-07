# Research: per-class weapon variants + weapon-swap + weapon-driven skill evolution — CX (feasibility lens)

ACTIVE RULES: (1) think before answering (2) tie every idea to what's buildable on RIMA's actual systems (3) no speculation about systems that don't exist — read the code to confirm (4) flag conflicts with LOCKED direction.

NLM ACCESS (optional):
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

## The idea (future / Phase-2 exploration)
Each class could equip weapon VARIANTS (Ranger: bow->crossbow->other bow; Gunslinger: revolver->shotgun->faster) that change feel, and skills EVOLVE based on the equipped weapon. Explore for all 10 classes — but YOUR job is HOW to build it.

## Your lens (CX = implementation feasibility on RIMA's real code)
First, READ the relevant systems to understand what exists, then judge cost:
- The weapon attach system: search Assets/Scripts for HandAnchor / weapon mount / WeaponDatabase / OrientationSync. How is a weapon sprite attached today (separate sprite, 8-dir by code per memory)? How hard is swapping the weapon sprite at runtime?
- The skill system: how are class skills defined (SkillData / skill controllers per class, e.g. Assets/Scripts/Skills/Warblade/*)? How would a skill "evolve" based on equipped weapon — data-driven variant, conditional behavior, or swapped skill component?
- The draft system (DraftManager): could weapon choice ride the existing between-room draft, or does it need a new acquisition flow?

## Deliver (RANKED by ROI / lowest-risk-highest-impact)
1. **Weapon-swap architecture options** for RIMA: (A) cosmetic-only sprite swap, (B) weapon = stat/behavior modifier on existing skills, (C) weapon = swaps the whole skill set. For each: what code changes, what data model (ScriptableObject?), and cost S/M/L.
2. **Weapon-driven skill-evolution model** that fits RIMA's skill code with least churn — concrete: which class/file would you prototype first and how.
3. **Acquisition flow** recommendation (reuse DraftManager vs new system) + cost.
4. **Scope/power-creep control** (how many variants is sane, shared base + weapon-modifier vs full unique sets).
Map ideas to RIMA's 10 classes (Warblade greatsword, Ranger bow, Gunslinger revolver, Ronin katana, Shadowblade daggers, Brawler fists, Elementalist caster, Ravager heavy, Summoner RIFT, Hexer CURSE).
LOCKED: cursor-aim, floating-island, PixelLab sprites, "12 skills/class depth-not-breadth" canon, painterly VFX.

## OUTPUT
Write the full ranked answer to `STAGING/RESEARCH_WEAPONS_CX.md` (also lands in CODEX_DONE_<profile>.md). End with "TOP build-order if we did this in Phase-2".
