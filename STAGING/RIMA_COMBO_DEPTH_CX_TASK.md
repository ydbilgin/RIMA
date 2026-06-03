# RIMA Combo-Depth Analysis — CX (combo-engine feasibility + combinatorial scaling)

ACTIVE RULES: (1) think before answering (2) min output, no filler (3) ANALYSIS ONLY — no code, no file edits (4) BLOCKED if unreadable.

NLM ACCESS (worked before): uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>".

## READ FIRST
- `STAGING/RIMA_SKILL_MECHANIC_FINAL_REPORT_S6.md` (the locked design verdict + skill feasibility)
- `STAGING/RIMA_CANON_BRIEF_FROM_NLM.md` (NLM canon: Commit-Beat, 9-family tags Fracture/Veil/Pierce/Bleed/Echo/Cut/Pressure/Strike/Rift, 3+ families → Rift proc, states broken/burning/scar/marked)

## The question (user's, verbatim intent)
"Should there be MORE skills? Are the skills COMPATIBLE with each other? Can they COMBO? Can MANY combos be made?" Think about combo richness, not architecture.

## Grounding — WB 12 demo skills → states they apply/consume (from SkillDatabase.cs descriptions)
- Iron Charge: applies **Stun** 1.5s, +15 Rage · Cleave: spends 20 Rage, dmg scales w/ Rage · Deep Wound: **Bleed** DoT · Sunder Mark: **Sundered** (armor −40/−60 if Death Blow active) · Crippling Blow: **Slow+Stun**, "%600 execute if chained w/ Death Blow" · Earthsplitter: **Broken**+Stun, +25 Rage · Blade Rush: knockback, +15 Rage · Gravity Cleave: **Pull** (group enemies) · Iron Counter: parry/reflect · Iron Crush: +30% basic dmg buff, "chains w/ Bladestorm" · Battle Surge: Rage-spend→heal · Death Blow: execute <30% HP, %400/%600.
- **CONFIRMED reality:** tags INERT (SkillDatabase.Add never sets them), no ChainWindowTracker, commit-beat carries no payload → skills currently DO NOT combo in code (isolated triggers).

## Your task (systems / combinatorics lens)
1. **Combo-engine model:** what is the minimal data/code that turns N skills into M combos? Define the **state-application → state-consumption matrix** (which states are produced by what, consumed by what for a bonus). Is a state-bus the right substrate, or chain-windows, or tag-families — or layered?
2. **Combinatorial scaling:** with 12 WB skills + ~7 states, how many MEANINGFUL combos are theoretically reachable? Does combo count scale with skill COUNT, with STATE count, or with the interaction-matrix density? Quantify (rough).
3. **More skills vs richer interactions:** from an engine standpoint, does adding more skills increase combos linearly, or does enriching the state-matrix (each skill produces+consumes states) increase combos combinatorially? Give the lever that maximizes combos per unit of dev effort.
4. **Demo feasibility:** can the current 12 WB skills be made combo-dense with ONLY the ChainWindowTracker + state-bus, or do skills need re-authoring? List which of the 12 are "isolated" (produce/consume nothing) and need a state hook.
5. **Phase-2 explosion:** cross-class (2 classes) + 9 families + Rift capstone — model the combo-count growth. Where does it over-scale into balance hell?

~700-900 words. CONFIRMED vs INFERRED. Quantify where you can.
