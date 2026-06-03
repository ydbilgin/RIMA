# Tile Painter UI Implementation — Codex (gpt-5.5 xhigh)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Implement the painter UI overhaul. Two verdicts produced (agy visual/UX + your own engineering verdict). Both converged. Opus synthesized. You will IMPLEMENT — surgical single-file edit.

## INPUTS (read all 3 before coding)

1. `STAGING/s106_overnight/painter_v2_user_feedback.png` — user screenshot showing the broken state (text truncation, narrow panel, etc.)
2. `STAGING/s106_overnight/PAINTER_UI_VERDICT_AGY.md` — agy verdict (visual/UX/layout proposal, ASCII wireframe, breakpoints)
3. `STAGING/s106_overnight/PAINTER_UI_VERDICT_CODEX.md` — your own previous engineering verdict with exact code snippets

## TARGET FILE
- `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs` (currently ~550 lines)
- DO NOT touch `UnifiedMapDesigner.cs` (reflection wrapper relies on existing public API)

## SYNTHESIZED SPEC (Opus, takes BOTH verdicts)

### Critical bug to fix FIRST
`position.width` returns hidden window default (~320px) when embedded in `UnifiedMapDesigner` via reflection. Use `EditorGUIUtility.currentViewWidth - 24f` instead.

### Required constants (add at top of class)
```csharp
private const float SidePanelMinWidth = 160f;
private const float MainPanelMinWidth = 240f;
private const float ResizeHandleWidth = 8f;
private const float SideHeaderHeight = 46f;
private const float SideFooterHeight = 34f;
private const float ScrollbarWidth = 16f;
private const float SectionHeaderHeight = 22f;
private const float TileGap = 6f;
private const float MinTileCell = 76f;
private const float BadgeWidth = 42f;
private const float AccentStripeWidth = 4f;
private const float CompactThresholdWidth = 520f;
private const float CollapseSideBreakpoint = 600f;
private const float HideSideBreakpoint = 400f;
```

### Layout (top to bottom)
```
+----------------------------------------------------------+
| Toolbar row: [Tilemap field]  [Save Scene] [Clear]       |
+--SIDE PANEL (sticky parts)---+--MAIN PANEL---------------+
| [Library (N)]   [Search...]  | ACTIVE SELECTION CARD     |
| [Size: ===o=== 48px]          | [64px preview thumb]      |
|------------------------------|  Theme: Cobblestone (4 var)|
| ▼ COBBLESTONE  [4/4]         |  border = accent color     |
|  [t0] [t1]                   |                            |
|  [t2] [t3]                   | TOOL                       |
| ▼ CYAN VEINS   [3/3]         | [Paint] [Erase]            |
|  [t4] [t5]                   |                            |
|  [t6]                        | PAINT MODE                 |
| ► DIRT        [4/4]          | [Group] [Single]           |
| ► RITUAL      [5/5]          |                            |
| (scroll)                     | BRUSH SIZE                 |
|------------------------------| [1] [2] [3] [4] [5]        |
| [Refresh]   (footer fixed)   |                            |
+------------------------------+  SETTINGS                  |
                               |  [x] Random variant         |
                               +----------------------------+
| Status: Painted 3x3 (Cobblestone) at (-4, 2)              |
+----------------------------------------------------------+
```

### 1. Side panel — rect-partitioned (sticky header + scroll body + sticky footer)
- Header band: search field (ToolbarSearchField) + thumbnail size slider
- Scroll body: foldout per theme group, 2-3 columns auto from available width
- Footer band: Refresh button always visible
- Use Codex's spec from PAINTER_UI_VERDICT_CODEX.md section 2 (verbatim Rect approach)

### 2. Tile card
- Square thumbnail (no text label under — saves vertical space)
- Bottom-right index badge: `t0`, `t1` etc. semi-transparent
- 1px accent-colored border (theme color) — 2.5px when selected
- Tooltip on hover: tile name + tile_N
- Click → switch to Single mode + select

### 3. Active Selection Card (in main panel top)
- 64px preview thumbnail on left
- Group name + range (e.g. "Cobblestone (stone)  tile_0..3") on right
- 4 var loaded count
- Accent border around card (theme color) — large visual cue

### 4. Resize handle (between side and main)
- 8px wide
- 3-dot grip pattern (center)
- Hover state: cyan accent (#1ae0ff)
- AddCursorRect to ResizeHorizontal
- Drag updates `sidePanelWidth` within `[160, viewWidth - 240]`

### 5. Section helper
```csharp
private delegate void ContentDelegate();
private void DrawSection(string title, ContentDelegate content) { ... }
```
Use for: TOOL, PAINT MODE, BRUSH SIZE, SETTINGS

### 6. Compact mode (`viewWidth < 520`)
- Mode toggle: "Group" / "Single" (instead of "Group (theme batch)")
- Brush labels: "1", "2", ... (no "×1")
- Tool labels: keep "Paint" / "Erase"

### 7. Collapse mode (`viewWidth < 600`, `>= 400`)
- Side panel locks to 40px icon strip
- 4 theme color squares as quick-select buttons
- Toggle button to expand back

### 8. Hide mode (`viewWidth < 400`)
- Side panel hidden
- Hamburger button toggles overlay drawer
- Main panel uses full width

### 9. Status bar (footer)
- Remove `EditorGUILayout.HelpBox` from main panel
- Single-line footer band at bottom of window
- Shows: last action + active tilemap + active selection

### 10. Style cache (EnsureStyles)
```csharp
private GUIStyle sectionHeaderStyle;
private GUIStyle subtleLabelStyle;
private GUIStyle badgeStyle;
private GUIStyle activeThemeStyle;
private GUIStyle compactButtonStyle;
private GUIStyle libraryHeaderStyle;
private GUIStyle searchFieldStyle;
```

### 11. Foldout state
Each theme group has its own collapsed bool. Persist via `[SerializeField] private bool[] groupCollapsed`.

### 12. Search filter
Filter tiles by name AND by theme label. Index badge still shows. When search active, show all matches across groups (no grouping).

## TEST CRITERIA (you verify before saying DONE)

Open `RIMA > Map Designer` AND `RIMA > Tile Painter (Minimal)` standalone. Test at viewport widths: **1270, 800, 590, 390** (resize window).

Pass criteria:
- ✅ No clipped labels at any tested width
- ✅ At least 2 visible tile columns at 1270 / 800 widths
- ✅ Refresh button always visible at bottom of side panel
- ✅ Resize handle visible (8px + 3-dot grip), hover changes color
- ✅ At 590px: side panel auto-collapses to icon strip
- ✅ At 390px: side panel hidden, drawer button visible
- ✅ Brush size: all 5 choices reachable at every width
- ✅ Active Selection Card shows accent-bordered preview
- ✅ Tile click → switches Mode to Single + selects
- ✅ Foldout collapse/expand per theme works
- ✅ Search filter narrows tile list
- ✅ Console: 0 error, 0 warning

## DELIVERABLE
- Edited `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs`
- Verification: open painter in Unity, screenshot at 1270 + 590 widths → `STAGING/s106_overnight/painter_v3_widthX.png`
- Brief done report `STAGING/s106_overnight/PAINTER_UI_V3_DONE.md`
- Final CODEX_DONE_<profile>.md with `STATUS: DONE`

## TIME ESTIMATE
60-90 min at xhigh.

## CONSTRAINTS
- Single-file only (MinimalTilePainter.cs)
- Public API unchanged (menu item path, OnEnable, OnDisable)
- `UnifiedMapDesigner.cs` reflection wrapper still works
- No new dependencies
- Preserve `ThemeGroup` struct, `tiles` dynamic loading, theme-range data table
- Unity Editor compile must succeed (0 error)
