# Tile Painter UI Engineering Verdict

The screenshot confirms the reported issue: the Tile Painter is technically usable, but the IMGUI layout behaves like a fixed-width inspector that has been squeezed into a narrower embedded viewport. The result is not a visual polish problem first; it is a layout contract problem. `MinimalTilePainter.OnGUI()` assumes `position.width`, fixed row heights, and default toolbar sizing will fit. Inside `UnifiedMapDesigner`, the painter is invoked through reflection inside a scroll view, so the visible width can be smaller than the hidden window's `position.width`. Controls then clip instead of adapting.

The refactor should stay single-file and surgical. Keep `OnEnable`, the menu item, `ThemeGroup`, `tiles`, dynamic loading, and `UnifiedMapDesigner` reflection compatibility. The change should replace width-guessing with explicit rect allocation, predictable minimum widths, and breakpoint-driven side panel behavior.

## 1. Truncation Fix Strategy

Root cause: group headers and control rows render long strings into one rect. Toolbars rely on `GUILayout.Toolbar` default widths, then clip labels when the parent layout is narrow.

Change group headers to split text from count. The label gets all available middle space; the count becomes a fixed trailing badge.

```csharp
Rect h = GUILayoutUtility.GetRect(SidePanelMinWidth, GroupHeaderHeight,
    GUILayout.ExpandWidth(true), GUILayout.MinWidth(SidePanelMinWidth));
Rect stripe = new Rect(h.x, h.y, AccentStripeWidth, h.height);
Rect badge = new Rect(h.xMax - BadgeWidth - 6f, h.y + 3f, BadgeWidth, h.height - 6f);
Rect label = new Rect(h.x + 10f, h.y, badge.x - h.x - 14f, h.height);
EditorGUI.DrawRect(stripe, g.Accent);
GUI.Label(label, new GUIContent(g.Label, g.Label), groupHeaderStyle);
GUI.Label(badge, $"{loaded}/{g.Count}", badgeStyle);
if (GUI.Button(h, GUIContent.none, GUIStyle.none)) SelectTheme(groupIdx, g, loaded);
```

For toolbars, use `GUIContent[]`, tooltips, and calculated min widths. Avoid long labels when compact.

```csharp
GUIContent[] modeLabels = compactControls
    ? new[] { new GUIContent("Group", "Theme batch"), new GUIContent("Single", "Single tile") }
    : new[] { new GUIContent("Group theme", "Paint variants from one theme"),
              new GUIContent("Single tile", "Paint one selected tile") };
mode = (Mode)GUILayout.Toolbar((int)mode, modeLabels,
    GUILayout.MinWidth(compactControls ? 156f : 240f), GUILayout.Height(22f));
```

For active theme, stop using one clipped label. Allocate a taller rect and word-wrap.

```csharp
activeThemeStyle.wordWrap = true;
float h = activeThemeStyle.CalcHeight(new GUIContent(activeText), viewWidth - 28f);
Rect line = EditorGUILayout.GetControlRect(false, Mathf.Max(30f, h + 8f));
GUI.Label(new Rect(line.x + 10f, line.y + 4f, line.width - 14f, line.height - 8f),
    activeText, activeThemeStyle);
```

## 2. Side Panel Layout

Root cause: the current side panel uses a flow layout per group with a user slider, but the screenshot shows a single narrow column where only two or three useful tiles are visible. The refresh button is outside the scroll, but because the panel is not rect-partitioned, the scroll consumes the usable height and the bottom affordance becomes easy to lose.

Replace `DrawSidePanel()` with fixed rect partitioning: sticky header, scroll body, fixed refresh footer. The grid should compute columns from available width, defaulting to 2 columns when the panel is around 220 px.

```csharp
Rect panel = GUILayoutUtility.GetRect(sidePanelWidth, 1f,
    GUILayout.Width(sidePanelWidth), GUILayout.ExpandHeight(true));
Rect sticky = new Rect(panel.x, panel.y, panel.width, SideHeaderHeight);
Rect footer = new Rect(panel.x + 6f, panel.yMax - FooterHeight, panel.width - 12f, 24f);
Rect scroll = new Rect(panel.x, sticky.yMax + 4f, panel.width,
    panel.height - sticky.height - FooterHeight - 8f);
DrawLibraryHeader(sticky);
sideScroll = GUI.BeginScrollView(scroll, sideScroll, contentRect);
DrawGroupsRectGrid(contentRect.width);
GUI.EndScrollView();
if (GUI.Button(footer, new GUIContent("Refresh", "Re-scan tile assets"))) LoadTileAssets();
```

The grid should not rely on `GUILayout` rows. Compute the tile card size from the actual panel width.

```csharp
float usable = Mathf.Max(1f, sidePanelWidth - PanelPad * 2f - ScrollbarWidth);
int columns = Mathf.Clamp(Mathf.FloorToInt((usable + TileGap) / MinTileCell), 1, 4);
float card = Mathf.Floor((usable - TileGap * (columns - 1)) / columns);
for (int j = 0; j < g.Count; j++)
{
    int col = j % columns;
    int row = j / columns;
    Rect cell = new Rect(x + col * (card + TileGap), y + row * (card + LabelHeight + TileGap),
        card, card + LabelHeight);
    DrawTileCard(cell, g.Start + j, g.Accent);
}
```

## 3. Resize Handle Visibility

Root cause: a 4 px grey stripe is below perception threshold in a dense dark editor. Make it 8 px, draw a hover state, and add a three-dot grip. Preserve `AddCursorRect`.

```csharp
Rect handle = GUILayoutUtility.GetRect(ResizeHandleWidth, 1f,
    GUILayout.Width(ResizeHandleWidth), GUILayout.ExpandHeight(true));
bool hover = handle.Contains(Event.current.mousePosition);
EditorGUI.DrawRect(handle, hover ? handleHoverColor : handleColor);
Vector2 c = handle.center;
for (int i = -1; i <= 1; i++)
    EditorGUI.DrawRect(new Rect(c.x - 1f, c.y + i * 7f - 1f, 2f, 2f), gripDotColor);
EditorGUIUtility.AddCursorRect(handle, MouseCursor.ResizeHorizontal);
```

## 4. Responsive Breakpoints

Use the visible IMGUI width, not only `position.width`.

```csharp
float viewWidth = Mathf.Max(320f, EditorGUIUtility.currentViewWidth - 24f);
bool hideSidePanel = viewWidth < 400f;
bool collapseSidePanel = viewWidth < 600f && !hideSidePanel;
float maxSide = Mathf.Max(SidePanelMinWidth, viewWidth - MainPanelMinWidth);
sidePanelWidth = Mathf.Clamp(sidePanelWidth, SidePanelMinWidth, maxSide);
```

At `<600`, draw a 40 px icon strip with a toggle and four theme color buttons. At `<400`, draw only controls and a compact selected-tile summary. Brush labels switch from `"1x1"` to `"1"` to preserve all five choices.

```csharp
GUIContent[] brushLabels = viewWidth < 520f ? BrushShortLabels : BrushFullLabels;
brushSize = GUILayout.Toolbar(brushSize - 1, brushLabels,
    GUILayout.MinWidth(BrushMinButtonWidth * 5f), GUILayout.Height(22f)) + 1;
```

## 5. Code Structure

Add constants near `TileFolder`, then cache styles once. This keeps the refactor readable without splitting the file.

```csharp
private delegate void ContentDelegate();
private const float SidePanelMinWidth = 160f;
private const float MainPanelMinWidth = 240f;
private const float ResizeHandleWidth = 8f;
private const float SideHeaderHeight = 46f;
private const float FooterHeight = 34f;
private const float ScrollbarWidth = 16f;
private const float TileGap = 6f;
private const float MinTileCell = 76f;
private const float BadgeWidth = 42f;
```

```csharp
private void DrawSection(string title, ContentDelegate content)
{
    Rect h = GUILayoutUtility.GetRect(1f, SectionHeaderHeight, GUILayout.ExpandWidth(true));
    EditorGUI.DrawRect(h, sectionHeaderBg);
    GUI.Label(new Rect(h.x + 8f, h.y, h.width - 16f, h.height), title, sectionHeaderStyle);
    using (new EditorGUILayout.VerticalScope(sectionBodyStyle))
        content();
}
```

`EnsureStyles()` should cache `sectionHeaderStyle`, `subtleLabelStyle`, `badgeStyle`, `activeThemeStyle`, and `compactButtonStyle`. Set alignment, padding, clipping, and word-wrap explicitly. Do not allocate textures per frame; keep existing `Color` drawing or add a single `MakeTexture` helper if a style background is required.

Testing should cover standalone `RIMA/Tile Painter (Minimal)` and embedded `RIMA/Map Designer` at widths 1270, 800, 590, and 390. Pass criteria: no clipped labels in the main control panel, at least two visible tile columns at normal width, refresh always visible, visible drag affordance, and all brush/tool/mode choices reachable.

VERDICT: 2-col auto side grid + sticky library header/footer + 8px dotted dragbar + responsive collapse at <600px and hidden side panel at <400px
