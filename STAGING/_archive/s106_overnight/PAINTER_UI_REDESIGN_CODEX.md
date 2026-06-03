# Tile Painter UI Redesign — Codex (gpt-5.5 xhigh), ENGINEERING/IMGUI LENS

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: User mevcut Map Designer Tile Painter sekmesinden memnun değil — "çekilmiyor ayarlanmıyor, daha güzel olsun, kullanıcı dostu olsun". Sen Unity IMGUI engineering lens'inden bak (resize, dynamic layout, no text cutoff, responsive). Antigravity paralel UX/visual hierarchy verdict veriyor. Ben (Opus) iki verdict'i sentezleyip implement edeceğim.

## CURRENT STATE
- Mevcut UI screenshot: `STAGING/s106_overnight/painter_v2_user_feedback.png` (USER RECORDED, 1270×780, **READ THIS FIRST**)
- Kod dosyası: `Assets/Scripts/Editor/MapDesigner/MinimalTilePainter.cs` (550 satır, IMGUI EditorWindow — son revision 2026-05-25 ~20:05)
- Yer aldığı container: `UnifiedMapDesigner.cs` (tab-based wrapper, reflection ile MinimalTilePainter embed eder)

## OBSERVED PROBLEMS (from user screenshot)

Read the screenshot file first. Concrete bugs:
1. **Text truncation** — neredeyse her label kesik (group headers, brush size buttons, active theme display, mode toggle)
2. **Side panel verimsiz** — 56px thumbnail tek sütun, ortalama 2 tile görünüyor, scroll zorunlu
3. **Resize handle invisible** — 4px stripe, user "çekilmiyor" diyor (fark etmiyor)
4. **Refresh button kesiliyor** — bottom alanda fit etmiyor
5. **GUILayout overflow** — `EditorGUILayout.GetControlRect(false, 60f)` row height static, içerdiği elements daha geniş olunca taşıyor

## CURRENT CODE STRUCTURE (for reference)

```csharp
// MinimalTilePainter.cs ~ key methods:
void OnGUI() {
    sidePanelWidth = Mathf.Clamp(sidePanelWidth, 160f, position.width - 220f);
    BeginHorizontal();
    DrawSidePanel();       // BeginVertical(Width=sidePanelWidth)
    DrawResizeHandle();    // 4px stripe
    DrawMainPanel();       // BeginVertical(ExpandWidth)
    EndHorizontal();
}
void DrawSidePanel() {
    BeginScrollView()  // tile grid
    for each ThemeGroup: DrawGroupBlock()  // header bar + flow-wrap thumbnails
    Button("Refresh tile assets")  // below scroll
}
void DrawMainPanel() {
    BeginScrollView()
    ObjectField, Toolbar(Mode), DrawGroupModeControls() or DrawSingleModeControls(),
    Toolbar(Tool), Toolbar(Brush 1..5), HelpBox(status), Button row, Foldout(debug)
}
```

## YOUR JOB

Provide an engineering-grade plan to fix all 5 bugs above. Output a refactor spec (concrete edits to MinimalTilePainter.cs).

### Specific deliverables:

#### 1. **Truncation fix strategy**
   - Move from inline `[X/Y]` count in header → trailing badge with right-align Rect
   - Use `GUILayoutUtility.GetRect` with `GUILayout.ExpandWidth(true)` + explicit `MinWidth`
   - For Toolbar buttons → `GUIContent` with `tooltip` + min width per button
   - Word-wrap on Active theme display

#### 2. **Side panel layout**
   - Manual `Rect`-based 2-column grid (or N-column auto from `sidePanelWidth - scrollbar`)
   - Thumbnail size determined by `(sidePanelWidth - padding - scrollbar) / desiredColumns`
   - Header row sticky (always visible) — `Rect`-based sticky band on top of scroll
   - Refresh button outside the scroll, always visible at bottom

#### 3. **Resize handle visibility**
   - 6-8px wide accent stripe with vertical dot pattern (3 grey dots) — Inspector convention
   - Hover state: lighten color
   - `EditorGUIUtility.AddCursorRect` ✓ already there
   - Add `DragHandle` style if available, else custom

#### 4. **Responsive breakpoints**
   - `position.width < 600` → side panel collapsed to 40px icon strip (collapse toggle)
   - `position.width < 400` → side panel hidden entirely, controls only
   - `sidePanelWidth` clamped within `[160, position.width - 240]`
   - Brush button labels switch to "1, 2, 3, 4, 5" when width narrow

#### 5. **Code structure**
   - Pull magic numbers into `const` at top
   - Extract `DrawSection(header, ContentDelegate)` helper for consistent section styling
   - Use `GUIStyle` cached in `EnsureStyles()` for: section header, subtle label, badge, button

### CONSTRAINTS
- Keep `MinimalTilePainter.cs` single-file (do not split into multiple files)
- Public API (menu item, OnEnable behavior) unchanged so `UnifiedMapDesigner` wrapper still works
- No new dependencies
- Preserve theme-range data (`ThemeGroup` struct) — extensible for future tiles
- Preserve `tiles` list dynamic loading

### DELIVERABLE
- Single file: `STAGING/s106_overnight/PAINTER_UI_VERDICT_CODEX.md`
- 800-1200 words
- Include: concrete code snippets for #1-5 (`Rect`/`GUILayout` examples, GUIStyle init)
- Final line: `VERDICT: <APPROACH SUMMARY>` (e.g., `VERDICT: 2-col side grid + sticky headers + 8px dragbar + responsive collapse at <600px`)

### TIME ESTIMATE
~30-45 min at xhigh effort.
