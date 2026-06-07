ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
Round-2 council: asset/code GAP AUDIT of the "Portal Only Updated Pack" against what RIMA already has. ANALYSIS ONLY. Write result to CODEX_DONE.md.

# Sources to read
1. `STAGING/_incoming/portal_pack_2026-06-06/RIMA_PortalOnly_Updated_Pack/docs/*.md` (6 docs) + `README.md` + `PACKAGE_MANIFEST.json`
2. Prior council decision (round 1): `STAGING/V2_PLAN_DECISION_2026-06-06.md`

# Context
The pack's socket model (ENTRY_S + EXIT_NW/N/NE, 1 facing direction, back-edge row) matches what we SHIPPED today (commit 20d1f09c: north-anchor row + south spawn). So the question is NOT "should we do this" — it's the asset/visual delta.

# Your lens: WHAT EXISTS vs WHAT NEEDS PRODUCTION (file paths as evidence)
1. **Cliff set (pack doc 04.A, 8 required + 5 optional pieces):** inventory our existing cliff sprites (directional SW/SE pieces, corners, endcaps?) — where do they live (Assets/... folders, tile assets), which of the pack's 8 minimum pieces do we already cover, which are genuinely missing? Does our cliff system (CliffAutoPlacer/DirectionalCliffTile or current IsoRoomBuilder cliff pass) even SUPPORT endcaps/inner-outer corners as distinct sprites?
2. **Portal set (doc 04.B):** what door/gate/portal sprites exist today (gateNorthSprite, runeCombatSprite/runeEliteSprite in IsoRoomBuilder; old Gate/Portal prefabs; any arch sprites — ArchGate from Chamber?). Pack wants 1 base arch + 5 skin variants (rune/icon/accent/particle). Gap list per variant. Could the Chamber's ArchGate sprite serve as the base arch TODAY?
3. **Prop set (doc 04.C, 11 props):** inventory existing props (echo_pedestal, brazier, pillars, bone piles, rubble, monolith, crystal...) under Assets — match against the 11; list missing. Note sizes/canvas of existing ones.
4. **VFX/overlay (doc 04.D, 6 items):** portal burst/idle core/rift crack decal/ring glow/dust/fog — what particle prefabs or VFX sprites exist (VFXRouter registry? SlashArcVFX? chamber attune pulse)?
5. **Entry portal / arrival effect (ENTRY_S):** today the player just teleports to south spawn. Any existing arrival VFX hook in RoomRunDirector.EnsurePlayerAtSpawn or chamber? Cost of a simple spawn-in effect (S/M)?
6. **Code support:** does current BuildExitDoors support per-type SPRITE variants (vs current single gateNorthSprite + 2 runes)? What's the minimal change for 5-type portal skins + reward text label? Size estimate.

Output in CODEX_DONE.md: per-category table (pack item | exists today (path) | missing | size to produce/wire) + a final "minimum production list" (only genuinely missing items, ranked by demo impact). No code changes.
