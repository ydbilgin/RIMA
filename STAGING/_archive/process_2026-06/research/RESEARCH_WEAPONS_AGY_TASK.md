# Research: per-class weapon variants + weapon-swap + weapon-driven skill evolution — AGY (breadth lens)

ACTIVE RULES: (1) think before answering (2) concrete + sourced — name the games and HOW they do it (3) ground every idea in RIMA's 10 classes below (4) flag conflicts with LOCKED direction.

NLM ACCESS: query RIMA canon when useful:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

## The idea (future / Phase-2 exploration)
Each class has a base weapon, but could equip VARIANTS that change feel/playstyle, and the class's skills EVOLVE based on the equipped weapon. Examples the user gave: Ranger bow -> crossbow -> a different bow type; Gunslinger revolver -> shotgun -> faster weapons. Explore this for ALL 10 classes.

## Your lens (AGY = breadth / how other games do it)
Pull SPECIFIC mechanics + how they work from: **Hades 1 weapon Aspects, Hades 2 weapons/Aspects**, Dead Cells (weapon affixes/scaling, weapon archetypes), Monster Hunter (weapon CLASSES change the whole moveset), Risk of Rain 2 (item-driven skill mods), Wizard of Legend (arcana), Nuclear Throne / Enter the Gungeon (gun variety + synergies), Brotato (weapon classes), Hyper Light Drifter, Children of Morta (per-character weapon identity), Skul (swap heads = swap moveset). For each: how is the weapon acquired/swapped, does it change stats/moveset/skills, and how is power-creep controlled.

## RIMA — the 10 classes (ground variants in these identities)
- **Warblade** HEAVY·MELEE·RAGE — 3-step iron combo, anger fuels destruction (base: greatsword)
- **Elementalist** CASTER·RHYTHM·ELEMENTS — Fire/Frost/Lightning, empowered 3rd bolt
- **Shadowblade** AGILE·STEALTH·EXECUTE — Veil Strike into shadow, delete & vanish (daggers)
- **Ranger** RANGED·PRECISION·TRAPS — tap shot / hold to aim, traps set kill zone (bow)
- **Ravager** HEAVY·BLEED·FRENZY — stacks wounds, faster as enemies fall
- **Ronin** FAST·PARRY·IAIJUTSU — counter-window, one clean draw one kill (katana)
- **Gunslinger** RANGED·HEAT·CHAMBERS — 6-chamber revolver, crits on 6th & 12th shot
- **Brawler** MELEE·ARMOR·MOMENTUM — absorb hits -> punch power (fists/gauntlets)
- **Summoner** ·RIFT — summon-based (query NLM for kit)
- **Hexer** ·CURSE — curse/debuff caster (query NLM for kit)

RIMA core: signature verb "**Sundered Beat**" (BREAK guard -> EXECUTE); cursor-aim; 4 skills Q/E/R/F + ult; per-class resource; weapon is a SEPARATE sprite attached via HandAnchor (8-dir by code), so visual weapon-swap is cheap. Roguelite: skill-draft between rooms. LOCKED: cursor-aim, floating-island, PixelLab sprites, cyan-sparing, painterly VFX, NLM canon's "12 skills/class depth-not-breadth".

## OUTPUT — write to `STAGING/RESEARCH_WEAPONS_AGY.md` (+ your AGY_DONE file)
1. **Per-class weapon-variant table:** for each of the 10 classes, 2-4 weapon variants (name + 1-line: how it changes the feel/playstyle within the class fantasy).
2. **Weapon-swap model options:** how the player gets/swaps weapons (mid-run drop? draft choice? between-rooms? meta-unlock?) — with the source game for each model + a recommendation.
3. **Weapon-driven skill-evolution model:** how do the 4 skills change per equipped weapon? (same slot, different behavior/projectile/range/break-power per weapon) — with examples for 2-3 classes.
4. **Power-creep / scope control:** how to keep this from becoming unmanageable (variant count, shared vs unique skills).
End with "TOP picks that fit RIMA's Sundered-Beat identity best."
