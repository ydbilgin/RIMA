# Tile Painter UI Redesign Verdict — Antigravity (Gemini 3 Pro)

## 1. First Impressions
The current Tile Painter UI is severely crippled by a layout calculation bug: it clamps `sidePanelWidth` using the scriptable object's own unrendered `position.width` (defaulting to 320px), squeezing the entire main panel into a tiny 200px strip and leaving the rest of the 1270px window completely blank. Text truncation is rampant across all buttons and headers, rendering labels unreadable. The lack of collapsible groups, dynamic columns, and an active selection preview forces the user to paint blindly.

## 2. Layout Proposal

```
+-----------------------------------------------------------------------------------+
| Unified Map Designer: [Active Tilemap: Floor] [Refresh Assets] [Active Scene Path]|
+-----------------------------------------------------------------------------------+
| [ Tile Painter (Active) ] [ Room Builder ]                                        |
+-----------------------------------------------------------------------------------+
| Shortcuts: P=Paint | E=Erase | 1-5=Brush Size | T=Cycle Theme | Tab=Toggle Mode   |
+-----------------------------------------------------------------------------------+
|  SIDE PANEL (150px - 40% Width)   | |  MAIN PANEL (Flexible Width)                |
|  [Search Tiles...           ]     | |  +---------------------------------------+  |
|  [Size: ===o=== 48px        ]     | |  | SELECTED: Cobblestone Base            |  |
|                                   |G|  | [Large Icon] Theme: tile_0..3 (STONE)  |  |
|  ▼ COBBLESTONE (STONE)   [4/4]    |R|  |              4 variations loaded      |  |
|  +----+ +----+ +----+             |I|  +---------------------------------------+  |
|  | t0 | | t1 | | t2 |             |P|                                           |
|  +----+ +----+ +----+             | |  TOOL SELECTION                           |
|  ▼ CYAN VEINS (ACCENT)   [3/3]    |H|  [ Paint (P) ]  [ Erase (E) ]               |
|  +----+ +----+                    |A|                                           |
|  | t4 | | t5 |                    |N|  PAINT MODE                               |
|  +----+ +----+                    |D|  [ Theme Group (Tab) ]  [ Single Tile ]   |
|  ► DIRT (VARIATION)      [4/4]    |L|                                           |
|  ► RITUAL RUNE (FOCAL)   [5/5]    |E|  BRUSH SIZE                               |
|                                   | |  [ 1 ] [ 2 ] [ 3 ] [ 4 ] [ 5 ]            |
|                                   | |                                           |
|                                   | |  SETTINGS & UTILITIES                     |
|                                   | |  [x] Random variant within theme          |
|                                   | |  [ Save Scene ]   [ Clear Tilemap ]       |
+-----------------------------------------------------------------------------------+
| STATUS: Painted 3x3 (Cobblestone) at (-4, 2) | Active Tilemap: Floor              |
+-----------------------------------------------------------------------------------+
```

## 3. Side Panel Spec
* **Columns**: Flow-wrapped dynamically. At the default 200px width and **48px** thumbnail size, it displays **3 columns** of tiles.
* **Theme Group Headers**: Rendered as collapsible foldout banners with a dark background tint. A 4px vertical accent stripe on the left edge indicates the group's theme color (Stone = Gray, Cyan = Cyan, Dirt = Brown, Rune = Pink). A small collapse/expand arrow sits on the left, and a quick-select icon is placed on the far right.
* **Thumbnail Size**: Default to **48px × 48px** (scaled down from 56px). Keep the slider range at 32px to 64px. Remove the text labels under tiles to save vertical space; instead, overlay a small semi-transparent index badge (`t0`, `t1`) on the bottom-right corner of the thumbnail. Display the full tile name on mouse hover tooltip.
* **Scaling (16 → 64 → 200 tiles)**: Foldouts allow collapsing inactive groups. Add a "Collapse All / Expand All" shortcut bar and a standard search filter bar (`EditorGUILayout.ToolbarSearchField`) at the top of the side panel.

## 4. Main Panel Spec
* **Section Order**: Active Preview Card (Top) → Tool Selection (Paint/Erase) → Paint Mode (Theme/Single) → Brush Size → Settings & Utilities (Bottom).
* **Brush Size**: Replace the text-heavy `"1×1"..."5×5"` toolbar with a compact segmented bar: `[ 1 ] [ 2 ] [ 3 ] [ 4 ] [ 5 ]`.
* **Group/Single Toggle**: Use a segmented toolbar containing `[ Theme Group ]` and `[ Single Tile ]` rather than verbose names.
* **Active Preview**: Place a dedicated **Active Selection Card** at the top of the main panel, featuring a large 64px preview thumbnail, the active group/tile name, loaded count, and border glow matching the theme's accent color.

## 5. Resize Handle Spec
* **Visual Grip**: Draw a subtle 1px vertical divider inside a 6px clickable drag zone. On hover/drag, the line highlights in the RIMA Cyan accent (`#1ae0ff`). Add a central vertical 3-dot grip pattern.
* **Limits**: Min width: `150px` (fits 2 columns). Max width: `40%` of window width.
* **Bug Fix**: Use `EditorGUIUtility.currentViewWidth` to dynamically calculate the host window width, preventing clamping lockups.

## 6. Color & Typography Hierarchy
* **Typography**: H1 (Title) = 12px Bold White; H2 (Section) = 10px Bold Steel Blue with a thin divider line underneath; H3 (Muted info) = 9px Regular.
* **Accent Color**: Apply the active theme's accent color to the Scene View cursor diamond and the Active Preview border.
* **Status**: Remove the large Info `HelpBox` from the main panel. Reroute all text feedback to the unified footer status bar.

## 7. Responsive Breakpoints
* **Width >= 600px**: Full two-column grid.
* **Width 400px - 599px**: Side panel locks to 150px (2 columns). Main panel switches to compact labels.
* **Width < 400px**: Side panel collapses completely. A "Library ☰" button toggles it as an overlay drawer.

## 8. Final Verdict
1. Fix `currentViewWidth` clamping bug.
2. Add Active Selection Preview card.
3. Convert side panel headers to Collapsible Foldouts.
4. Replace text-heavy controls with segmented numeric/short button bars.
5. Polish the resize handle with hover highlights.
6. Reroute main panel status helpbox to the bottom status bar.

VERDICT: OVERHAUL — The UI layout must be overhauled because the nested window width calculations are fundamentally broken, and the controls lack the basic visual feedback, compactness, and collapsibility required for a functional map-making workflow.
