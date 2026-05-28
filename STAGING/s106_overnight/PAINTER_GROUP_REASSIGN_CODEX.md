# Painter Group Reassignment Feature — Codex (xhigh)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Tile Painter'a kullanıcı isteği üzerine "tile group reassignment" özelliği ekle. Kullanıcı istediği tile'ı istediği gruba taşıyabilsin. Tile_7 default'ta Dirt grubunda olabilir ama kullanıcı bunu Cyan grubuna almak istiyorsa, UI'dan yapsın.

## TARGET FILE
- `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs` (~922 lines, recently overhauled to v3 — `position.width` bug fix + side panel + Active Selection Card vb.)

## CURRENT STATE
- `ThemeGroup` struct hard-codes `(Label, Start, Count, Accent)` ranges
- `DefaultGroups` array: Cobblestone 0..3, Cyan 4..6, Dirt 7..10, Rune 11..15
- Tile-to-group resolution: `tileIndex >= group.Start && tileIndex < group.Start + group.Count`

## NEW REQUIREMENT

User can override per-tile group assignment. Example: user clicks tile_7, opens context menu, selects "Move to: Cyan Veins" → from now on tile_7 paints under Cyan theme when group mode active.

### Acceptance criteria

1. **Right-click on tile thumbnail** in side panel → context menu (`GenericMenu`):
   - `Move to: Cobblestone (stone)`
   - `Move to: Cyan Veins (accent)`
   - `Move to: Dirt (variation)`
   - `Move to: Ritual Rune (focal)`
   - `─────────`
   - `Reset to default group`
   - Show checkmark on the tile's current group (visual feedback)

2. **Persistence:** override map serialized as `Dictionary<int, int>` (tile index → group index)
   - Unity doesn't natively serialize Dictionary; use parallel arrays `[SerializeField] private List<int> overrideTileIndices` + `[SerializeField] private List<int> overrideGroupIndices`
   - Load into runtime `Dictionary<int, int>` in `OnEnable`
   - Save back to parallel arrays on every change + on `OnDisable`

3. **Tile filtering by group:** when group mode active and user paints, `PickTile` should consider:
   - Default range from `ThemeGroup`
   - PLUS tiles re-assigned to that group via override
   - MINUS tiles that were originally in that range but re-assigned elsewhere

   Example: Cyan group (default 4..6). User moved tile_5 to Dirt. Cyan's effective set = `{4, 6}`. Dirt's effective set = original {7..10} + `{5}` = `{5, 7, 8, 9, 10}`.

4. **Side panel rendering:** tile cards display under their EFFECTIVE group (the override if set, else default).
   - Group header count `[N/N]` reflects effective count (default - moved out + moved in)
   - Index badges still show `t5`, `t11` etc. (raw tile index, not relative)

5. **Reset functionality:**
   - Per-tile: context menu "Reset to default group" → removes override for that tile
   - Global: "Reset all overrides" button in side panel header or settings section
   - Confirm dialog on global reset

6. **UI accent on overridden tiles:**
   - Tile card with override has small "dot" icon in corner (besides index badge) indicating it's not in its default group
   - OR a slight tint on the badge BG color matching the OVERRIDE group's accent
   - Subtle, not disruptive

7. **Active Selection Card:** if a tile is in single mode, show its current effective group (with override applied) plus a note "(custom)" if overridden.

## CONSTRAINTS

- Single-file edit (`MinimalTilePainter.cs`)
- Preserve all v3 features (Active Card, breakpoints, side panel, foldout, search)
- No new dependencies
- No changes to `UnifiedMapDesigner.cs` (reflection wrapper)
- Public API unchanged
- 0 error 0 warning
- Existing painted floor tilemap NOT affected by code changes

## TEST CRITERIA

1. Right-click any tile in side panel → context menu shows
2. Select "Move to: Cyan" on tile_7 → tile_7 now appears in Cyan group; Dirt group count drops to 3
3. Paint with Cyan theme group → tile_7 included in random picks
4. Reopen painter window → override persists
5. Reset tile_7 → goes back to Dirt
6. Global "Reset all" → all overrides cleared
7. Active Selection Card shows correct group with "(custom)" tag if applicable
8. Console: 0 error, 0 warning
9. validate_script PASS
10. Screenshot of UI with one override applied: `STAGING/s106_overnight/painter_v4_override_demo.png`

## DELIVERABLE
- Edited `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs`
- Brief report `STAGING/s106_overnight/PAINTER_GROUP_REASSIGN_REPORT.md`
- Screenshot demonstrating override
- Final `CODEX_DONE_<profile>.md` with `STATUS: DONE`

## TIME ESTIMATE
~45-60 min at xhigh.
