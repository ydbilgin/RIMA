# LEVEL EDITOR UI — VISUAL DESIGN SPEC (2026-06-13)

> Lead UI-designer synthesis of two web research passes + a direct read of the SHIPPING code
> (`BuildModeUiStyle.cs`, `BuildPlacementController.cs`, `BuildTileBrushController.cs`).
> ONE concrete, buildable, premium dark Act1-toned design for RIMA's in-game level editor
> (Build Mode), focused on the ASSET BROWSER + the editor panels.
>
> Reality anchor (drove every decision): the existing Build Mode UI is 100% pure-runtime uGUI,
> built in code through `BuildModeUiStyle` factories using SOLID `Image` components only — no
> 9-slice sprites, no shaders, no external packages, ONE font (Jersey10 SDF). It is
> `ScreenSpaceOverlay`, so it is NOT MCP-screenshottable; the first pass MUST be right from spec.
> ASCII-safe body (no Turkish diacritics).

---

## 0. EXECUTIVE DECISION (read this first)

- **Asset strategy = PROCEDURAL-ONLY** (deterministic, code-defined). Justified in section 5.
  NOT a CC0 kit (cartoon-toned, off-canon, more rework than authoring clean rects), NOT
  imagegen chrome (unpredictable for exact 9-slice/palette + un-iterable because overlay UI is
  not screenshottable), NOT new packages (frozen Assets, 1-week, dependency risk).
- **Stay in uGUI.** UI Toolkit is the correct future home for a 10k-item browser, but for a
  demo with a known-small catalog it is a rewrite we do not need. (research pass 1, UI-Toolkit-vs-uGUI)
- **Keep ONE font (Jersey10 SDF).** The research recommends pairing Inter for body text; we
  DECLINE for the demo (see section 1.4 — importing/atlasing a second SDF font is a dependency +
  risk with no screenshot loop to catch fallout). Readability is solved with size/weight/spacing,
  not a second face.
- **Evolve `BuildModeUiStyle.cs`, do not rewrite.** It already does the hard-won premium things
  right (hairline border, ember accent bar, ember header underline, dark elevation hints, fixed
  rhythm). We EXTEND it with: a formal elevation/state token ramp, a procedural rounded 9-slice
  sprite generator, a `ButtonJuice` hover/press component, and four new factories
  (`MakeTabBar`, `MakeSearchField`, `MakeThumbnailGrid`, `MakeAssetCard`, plus `MakeScrollView`).
- **The #1 quality win = replace the vertical TEXT-button palette with a THUMBNAIL GRID** of
  asset cards (both research passes flagged this as the single biggest gap). Everything else is
  polish on top.

---

## 1. DESIGN TOKENS

All tokens live in `BuildModeUiStyle.cs` as `public static readonly Color` / `public const float`.
Every control reads from here (this single discipline is what reads as "designed", per both passes).

### 1.1 Palette (Act1 canon, hex on dark) — EXISTING + NEW

| Token | Hex / alpha | Role | Status |
|---|---|---|---|
| `AppCanvas` (conceptual) | the running game behind the panel | darkest layer (elevation 0) | (the game, not a fill) |
| `PanelBg` | `#16181C` @ 0.93 | panel surface (elevation 1) | EXISTING |
| `CardIdle` | `#22252B` @ 1.0 | asset card / raised tile (elevation 2) | NEW |
| `ButtonIdle` | `#2A2D32` @ 1.0 | button / chip fill (elevation 2) | EXISTING |
| `SurfaceHover` | `#34373D` @ 1.0 | hover lighten (elevation by lightness, not shadow) | NEW |
| `SurfacePressed` | `#1C1E22` @ 1.0 | press darken (depress, not raise) | NEW |
| `PanelBorder` | `#3A3D42` @ 1.0 | slate hairline edge | EXISTING |
| `BorderHover` | `#4A4E55` @ 1.0 | brightened edge on hover | NEW |
| `Ember` | `#E89020` @ 1.0 | PRIMARY / SELECTED / FOCUS (small areas only) | EXISTING |
| `EmberDim` | `#E89020` @ 0.55 | accent bars / underline / focus ring | EXISTING |
| `EmberGlow` | `#E89020` @ 0.22 | selected-card outer wash (very subtle) | NEW |
| `VoidPurple` | `#3A1A4A` @ 1.0 | rare structural accent (e.g. a category dot) | EXISTING |
| `HeaderText` | `#E8E8EC` @ 1.0 | high-emphasis text (never pure white) | EXISTING |
| `MutedText` | `#8A929C` @ 1.0 | medium-emphasis / secondary | EXISTING |
| `DisabledText` | `#8A929C` @ 0.45 | unavailable item / tool | NEW |
| `SelectedText` | `#16181C` @ 1.0 | dark text on ember fill | EXISTING |
| `HintBg` | `#101216` @ 0.95 | footer hint box (darker than panel) | EXISTING |
| `ScrollHandle` | `#3A3D42` @ 0.85 | thin scrollbar handle (muted slate) | NEW |
| `ScrollTrack` | `#16181C` @ 0.0 | near-invisible track | NEW |
| `CursorGreen` / `CursorRed` | existing world cursor tints | valid/invalid placement | EXISTING |

Rule (research pass 2, Material/Atmos dark-UI): **express depth by LIGHTER surfaces, not shadows.**
Elevation ramp = `PanelBg(#16181C) < CardIdle(#22252B)/ButtonIdle(#2A2D32) < SurfaceHover(#34373D)`.
Ember is reserved for SMALL selected/primary/focus areas only — never a large fill behind dense text.

### 1.2 Spacing scale (4/8 grid)

`Space1=4`, `Space2=8`, `Space3=12`, `Space4=16`, `Space6=24`. Existing `Padding=16` (=`Space4`),
`ItemGap=8` (=`Space2`) keep their names; new constants alias the scale. Grid gutters, card padding,
tab padding all snap to this scale.

### 1.3 Corner radii + the rounded-rect strategy

| Token | px | Applied to |
|---|---|---|
| `RadiusSm` | 4 | chips, search field, scrollbar handle |
| `RadiusMd` | 6 | buttons, asset cards |
| `RadiusLg` | 10 | panel outer frame |

**How rounding is achieved (decision):** the research lists 9-slice sprite vs shader/SDF.
We pick **procedurally GENERATED rounded 9-slice sprites** — `BuildModeUiStyle` creates a small
set of rounded-rect `Sprite`s at runtime (one per radius) via `Texture2D` + a signed-distance
fill, sets the 4 border values, and serves them with `Image.type = Sliced`. WHY over a shader:
zero new shader/asset dependency (Assets frozen), deterministic, crisp at any panel size, and
it slots into the existing all-`Image` factory model with one helper. (research pass 1: 9-slice =
simplest, zero runtime cost; SDF = future upgrade if per-corner dynamic radius is ever needed.)
Cache the generated sprites in statics (DisableDomainReload-safe like the existing `_font`).

### 1.4 Type scale (TMP, Jersey10 SDF only)

ONE font: `Jersey10-Regular SDF` (OFL) at `Assets/Fonts/Jersey10/`. Jersey10 is a tall, condensed
display/pixel face — excellent for a game-toned editor and on-brand. We hold the line at one font
(see decision 0): a second SDF face is a frozen-Assets dependency we cannot visually verify.

| Style | size | weight | spacing | color | Used for |
|---|---|---|---|---|---|
| `TitleH` (header) | 24 | Bold + UpperCase | 8 | HeaderText | panel header ("BUILD", "TILE BRUSH") — EXISTING |
| `TabLabel` | 16 | Bold + UpperCase | 4 | Muted/Selected | category tab labels |
| `Body` | 15 | Bold | 0 | Muted | button labels — EXISTING |
| `CardName` | 13 | SemiBold | 0.5 | HeaderText | asset card name (1-2 line, ellipsis) |
| `Hint` | 13 | Normal | 1.5 | Muted | hotkey hint box — EXISTING |
| `Status` | 13 | Normal | 0 | Muted | status line — EXISTING |
| `Count` / `Numeric` | 14 | Bold | 1.0 | Ember | radius / live counts |
| `Micro` | 11 | Normal | 1.0 | DisabledText | empty-state subtitle, tag pills |

Numeric readouts: Jersey10's digits are even-width enough for our short counts; no mono needed
for the demo. (Research's mono-for-numerics recommendation is deferred — overkill for "RADIUS 2".)

### 1.5 Elevation / shadow

Dark-UI convention = depth via lightness, NOT drop shadows (research pass 2). So:
- **No drop-shadow component anywhere.** (Also avoids the `UIEffect` dependency the research
  suggested — not worth a package import for a frozen-Assets demo.)
- Elevation is encoded purely as the surface-lightness ramp (1.1) + the existing 1px hairline
  border + a 1px faint top-highlight on the panel (a 1px `EmberDim`-free `#FFFFFF`@0.04 strip at
  the panel top inner edge) for the "premium surface" read. ONE new optional helper:
  `AddTopHighlight(rect)`.

### 1.6 State color matrix (THE reusable contract)

Applied identically by `ButtonJuice` (new) to every interactive control. Transitions = a short
coroutine color crossfade (0.08s) + scale punch (no DOTween dependency — research open-question (c)
answered: use coroutines).

| State | Fill | Border | Text | Scale | Accent bar |
|---|---|---|---|---|---|
| Idle | `ButtonIdle`/`CardIdle` | `PanelBorder` | `MutedText` | 1.00 | hidden |
| Hover | `SurfaceHover` | `BorderHover` | `HeaderText` | 1.03 | hidden |
| Pressed | `SurfacePressed` | `BorderHover` | `HeaderText` | 0.97 | hidden |
| Selected | `Ember` | `Ember` | `SelectedText` | 1.00 | VISIBLE (ember left bar) |
| Selected+Hover | `Ember` (keep) | `Ember` | `SelectedText` | 1.03 | visible |
| Disabled | `ButtonIdle` @0.5 | `PanelBorder` @0.5 | `DisabledText` | 1.00 | hidden |

This formalizes what `ApplySelected` already does (idle/selected) and ADDS the missing
hover/pressed/disabled states the research called out as the #1 absent behavior.

---

## 2. COMPONENT SPECS (exact)

> All sizes are in 1920x1080 reference-resolution px (the canvases already use
> `CanvasScaler ScaleWithScreenSize`, ref 1920x1080, match 0.5).

### 2.1 Panel — EXTEND `MakePanel`

- Outer frame: `PanelBorder` fill, rounded `RadiusLg` (now via generated 9-slice instead of a
  square Image). Inner `Bg` inset 1px = `PanelBg`, rounded `RadiusLg - 1`.
- Add a 1px top inner highlight strip (`AddTopHighlight`) = `#FFFFFF`@0.04, full width.
- Content rect inset by 1px border + `Padding(16)`. Unchanged contract: returns the content rect.
- Width `PanelWidth = 232` (EXISTING, keep). Height per panel set by caller.

### 2.2 Header — KEEP `MakeHeader` (already excellent)

UPPERCASE Jersey10 24 Bold, spacing 8, `HeaderText`; ember underline bar (40% width, 3px,
`Ember`) below. No change except the bar may use `RadiusSm` ends. Returns body-start offset.

### 2.3 Button (all states) — EXTEND `MakeButton` + add `ButtonJuice`

- Structure unchanged (transparent root `Button` target + `Border` frame + `Fill` + `Accent`
  bar + label), but `Border`/`Fill` now use the generated `RadiusMd` 9-slice sprite.
- Attach a `ButtonJuice` MonoBehaviour (NEW) implementing `IPointerEnter/Exit/Down/Up` to drive
  the full state matrix (1.6). `ApplySelected` stays the authority for the selected flag;
  `ButtonJuice` layers hover/press on top and respects selected (selected+hover keeps ember fill).
- Label: Jersey10 15 Bold, `MidlineLeft`, ellipsis. Selected = `SelectedText`.
- Height: caller sets via `LayoutElement.preferredHeight` (existing 38-40 rows).

### 2.4 Segmented toggle — FORMALIZE the existing PROP|TILE control

Already built inline in `BuildPlacementController`. Promote to `MakeSegmented(parent, labels[])`
returning the `ButtonStyle[]` so the pattern is reusable (tile sub-modes FLOOR|WALKABLE|OVERLAY
can adopt it too). Visual: a single rounded `RadiusMd` track (`PanelBg`), N equal segments with
1px dividers; selected segment = ember fill + dark text; the ember does NOT span the whole track
(small-area rule). Center-aligned labels (`TabLabel` style).

### 2.5 ASSET BROWSER (the centerpiece)

Replaces the vertical text-button palette in BOTH panels' "list" region. Composed of: tab bar +
search field + scrollable thumbnail grid (+ empty-state + scrollbar). Lives inside the existing
panel content rect, between the header/segmented top and the hint box bottom.

#### 2.5.1 Category tab bar — `MakeTabBar(content, categories, onSelect)` (data-driven)

- A horizontal row of chips, `RadiusSm`, height 26, `Space1(4)` gap, horizontally scrollable if
  they overflow (rare at 232px width — see layout note: tabs may wrap to 2 rows max).
- DATA-DRIVEN + EXTENSIBLE: takes a `IReadOnlyList<TabSpec{ string label; int count; Color dot }>`.
  Categories are supplied by the caller (e.g. derived from `PropRegistrySO` groups, or fixed
  TILES/PROPS/ENTITIES/LIGHTS), so adding a category later = add a list entry, no UI rewrite.
- Chip states: Idle = `ButtonIdle`, muted label; Hover = `SurfaceHover`; Selected = ember
  underline (2px) under the chip + `HeaderText` label (NOT an ember fill — keeps the bar calm and
  reserves ember-fill for the cards). Optional 4px `dot` color left of the label per category.
- Each chip shows its `count` as a trailing `Micro` number (e.g. "PROPS 12").

#### 2.5.2 Search field — `MakeSearchField(content, onChanged)`

- `TMP_InputField`, height 30, `RadiusSm`, fill `PanelBg`, 1px `PanelBorder`.
- Leading search glyph (procedural: a small ring + handle drawn into a generated sprite, OR a
  Jersey10 char fallback — kept simple, monochrome `MutedText`). Left pad 28 (glyph), right pad 8.
- Placeholder "Search..." in `MutedText`; typed text `HeaderText`.
- FOCUS = ember focus ring (border -> `Ember`, 1px). Supports `-` exclude (filter logic in caller).
- Behavior: filters the grid live by name (case-insensitive substring; `-term` excludes).

#### 2.5.3 Thumbnail GRID — `MakeThumbnailGrid(content)` + `MakeAssetCard(...)`

- `ScrollRect` (vertical) > `Viewport` (`RectMask2D`) > `Content` with `GridLayoutGroup`.
- `GridLayoutGroup`: `cellSize` default **96 x 116** (96 thumbnail + 20 name strip),
  `spacing = Space2(8)` both axes, `padding = 8` all sides, `constraint = FixedColumnCount`,
  **columns = 2** at the 232px panel (content width ~200 -> 2 x 96 + 8 gap fits). A thumbnail-size
  control (small `- / +` or slider) can switch to `cellSize 64x80` -> 3 columns (research:
  thumbnail-size control = high "real tool" signal, low effort; OPTIONAL P1).

**Asset card** (`MakeAssetCard`), the whole card is one click target (Fitt's Law):
- Root: `Button`, `CardIdle` fill, 1px `PanelBorder`, `RadiusMd`, attaches `ButtonJuice`.
- Thumbnail: `Image` top region 96x96, `preserveAspect = true` (object-fit: contain), centered on
  `PanelBg`. Source = the asset's existing sprite (`PropDefinitionSO.worldSprite` /
  `variantSprites[0]`; tile stamp sprite for tiles). NO new art needed — reuse game sprites.
- Name strip: bottom 20px, `CardName` (Jersey10 13), `HeaderText`, center, single line, ellipsis.
- States (via `ButtonJuice` + matrix 1.6):
  - Hover: fill -> `SurfaceHover`, border -> `BorderHover`, scale 1.03.
  - Selected: 2px `Ember` frame (border swap) + a faint `EmberGlow` 1px outer ring + a small
    ember check tick (procedural sprite) top-right corner; name -> `HeaderText` stays.
  - Disabled (asset unavailable / missing sprite): thumbnail @0.4, `DisabledText` name.
- Pooling: cards are pooled/reused on filter+scroll (catalog is small for the demo but pool anyway
  so a future large registry does not stutter — research pass 2 perf note).

#### 2.5.4 Empty-state — `MakeEmptyState(content, msg)`

Shown when the active tab/search yields zero cards: a centered block in the grid viewport —
a muted procedural glyph (e.g. a 32px outlined square), a `Body` line "No assets" and a `Micro`
subtitle "Try another category or clear search." All `DisabledText`. Hidden when cards exist.

#### 2.5.5 Scrollbar — `MakeSlimScrollbar(scrollRect)`

- Vertical `Scrollbar`, width **5px**, on the right inner edge of the viewport.
- Handle = `ScrollHandle` (`#3A3D42`@0.85), `RadiusSm`, min size 24px; Track = `ScrollTrack`
  (transparent). `Visibility = AutoHideAndExpandViewport`. Hover handle -> `SurfaceHover`.

### 2.6 Hint / status bar — KEEP `MakeHintBox` (already good)

`HintBg` box anchored bottom, 1px ember top-edge (`EmberDim`), Jersey10 13 `MutedText`, spacing 1.5.
Add a LIVE COUNT line for Build Mode (research/Sang-Hendrix lesson: design for high counts + a live
count from the start), e.g. `props 14  cells 320`. Status line (one line, ellipsis) sits just above.

---

## 3. LAYOUT (1920x1080)

Two panels, vertically centered, hugging the screen edges (matches the shipping anchors exactly —
left panel `anchor (0,0.5)`, right `anchor (1,0.5)`, inset by `Padding(16)`):

```
+---------------------------------------------------------------+
| [BUILD]  (LEFT, x=16, w=232, h~=680)        [TILE BRUSH]      |
|  ________________________            (RIGHT, -16, w=232, h~=460)|
| | HEADER  BUILD        |             ______________________   |
| | [ PROP | TILE ] seg  |            | HEADER TILE BRUSH    |  |
| | [Tabs: PROPS LIGHTS] |            | [FLOOR|WALK|OVERLAY] |  |
| | [ search...        ] |            | RADIUS 2             |  |
| | +------+ +------+    |   <canvas: | [ hint box        ] |  |
| | | card | | card |    |    running | |LMB.. 1 2 3 .. Z/Y| |  |
| | +------+ +------+    |    game,   | +--------------------+ |  |
| | | card | | card | sc |   Y-sorted | (right panel stays a |  |
| | +------+ +------+ ro |    lit)    |  compact mode column) |  |
| | | card | | card | ll |            +----------------------+  |
| | +------+ +------+    |                                       |
| | status line         |                                       |
| | [ hint + live count]|                                       |
| |_____________________|                                       |
+---------------------------------------------------------------+
```

- **LEFT = BUILD** (the asset browser lives here). Width 232, height grows to ~680 to give the
  grid room. Stack top->bottom: Header (34) -> Segmented PROP|TILE (40) -> Tab bar (26, +wrap row
  if needed) -> Search (30) -> **grid ScrollRect (flex, fills remaining)** -> status (20) ->
  hint+count box (~96). All gaps `Space2(8)`.
- **RIGHT = TILE BRUSH.** Stays the compact mode column (FLOOR/WALKABLE/OVERLAY as a segmented or
  the existing button column), RADIUS readout, hint box. The brush has no asset catalog, so NO grid
  here — keeping the asymmetry is correct (left = pick WHAT, right = pick HOW).
- **Grid columns:** 2 at default `cellSize 96` (content width ~200). 3 at the optional small size.
- **Responsive:** `CanvasScaler ScaleWithScreenSize` (ref 1920x1080, match 0.5) already handles
  other resolutions; panels stay edge-anchored and the grid is a `ScrollRect`, so shorter screens
  just scroll. No manual breakpoints needed for the demo.
- **Overlap-hide:** the existing enter-time "hide every other UI canvas" pass + `OwnCanvas` exempt
  stays as-is; both Build canvases are exempt so they never hide themselves.

---

## 4. EVOLVING `BuildModeUiStyle.cs` (extend, do NOT rewrite)

Keep all existing tokens, factories (`MakePanel`, `MakeHeader`, `MakeButton`, `ApplySelected`,
`MakeHintBox`, `Stretch`, `Top`, `Hex`, `Font`) and the `ButtonStyle` class — controllers depend
on them and the `*ForValidation` data-proof flow must keep working. ADD, in the same file/namespace:

1. **New token constants** (section 1.1/1.3/1.5/1.6): `CardIdle`, `SurfaceHover`, `SurfacePressed`,
   `BorderHover`, `EmberGlow`, `DisabledText`, `ScrollHandle`, `ScrollTrack`, radii, spacing aliases.
2. **`RoundedSprite(int radius)`** — static, cached: generates+returns a rounded-rect 9-slice
   `Sprite` (Texture2D SDF fill, border set for `type=Sliced`). DisableDomainReload-safe statics
   like `_font`/`_solidSprite`. Update `MakePanel`/`MakeButton` to use it (`Image.type=Sliced`).
3. **`ButtonJuice : MonoBehaviour`** (can be a nested/companion type) — pointer enter/exit/down/up
   coroutine crossfade + scale, drives the state matrix, respects the selected flag. `MakeButton`
   and `MakeAssetCard` attach it. No DOTween.
4. **`MakeSegmented(parent, string[] labels)`** -> `ButtonStyle[]` (promotes the inline PROP|TILE).
5. **`MakeTabBar(content, IReadOnlyList<TabSpec>, Action<int>)`** -> tab handles (data-driven).
6. **`MakeSearchField(content, Action<string>)`** -> `TMP_InputField` with glyph + focus ring.
7. **`MakeScrollGrid(content, Vector2 cellSize, int columns)`** -> returns the `Content` rect (with
   `GridLayoutGroup`) + the `ScrollRect`; internally builds Viewport+RectMask2D+slim scrollbar.
8. **`MakeAssetCard(gridContent, Sprite thumb, string name, Action onClick)`** -> a card
   `ButtonStyle`-like handle (background, thumbnail Image, name TMP, check tick, ButtonJuice).
9. **`MakeEmptyState(viewport, string msg)`** and **`AddTopHighlight(rect)`** helpers.

Controllers change MINIMALLY: `BuildPlacementController.EnsurePaletteUi` swaps the
`VerticalLayoutGroup` text-button loop for `MakeTabBar`+`MakeSearchField`+`MakeScrollGrid`+a
`MakeAssetCard` per `palette[i]` (thumbnail = `def.worldSprite`); selection still calls
`SelectPalette(i)` and the card uses `ApplySelected`/`ButtonJuice`. `BuildTileBrushController`
optionally adopts `MakeSegmented` for FLOOR|WALKABLE|OVERLAY. All `*ForValidation` hooks untouched.

---

## 5. ASSET STRATEGY = PROCEDURAL-ONLY (decision + justification)

**Chosen: procedural-only.** Every visual (rounded panels/buttons/cards, search glyph, check
tick, empty-state glyph, scrollbar, all color/spacing/type) is code-generated or token-driven in
`BuildModeUiStyle`. Thumbnails reuse EXISTING game sprites (prop/tile sprites already in the
project) — no new art. The only font is the already-present Jersey10 SDF.

Why not the others (research weighed, then overruled for THIS context):
- **NOT cc0-kit** (Kenney UI Pack, CC0): cartoon/fantasy tone; recoloring it to the restrained
  slate+ember Act1 canon is MORE work than generating clean rounded-rects, and it would not match.
- **NOT generated-imagegen**: AI UI chrome is unreliable for exact 9-slice borders + palette
  fidelity, AND the overlay UI is not MCP-screenshottable, so we cannot iterate to fix bad gens.
  Deterministic code styling is the only safe path with no visual feedback loop.
- **NOT hybrid w/ MIT packages** (UIEffect / Unity-UI-Rounded-Corners / SDF toolkit): all are good
  libraries, but adding packages to a FROZEN-Assets 1-week demo is dependency + recompile risk for
  a look we can hit procedurally. Shadows are not even wanted (dark-UI = depth by lightness).
- **NOT a 2nd font (Inter)**: importing+atlasing a second SDF face is a frozen-Assets dependency we
  cannot visually verify; Jersey10 + the size/weight/spacing scale carries readability for the demo.

This is fully license-clean (Jersey10 = OFL, already in repo; all art is the project's own),
matches Act1 canon exactly, and is robust without a screenshot loop.

### Asset list (what is "used", with source/license)

| Asset | Source | License | Note |
|---|---|---|---|
| Jersey10-Regular SDF | `Assets/Fonts/Jersey10/` (already in repo) | SIL OFL 1.1 | the ONE UI font; brand + body |
| Prop / tile sprites (thumbnails) | the project's existing game sprites (`PropDefinitionSO.worldSprite`, tile assets) | project-owned | reused as card thumbnails; no new art |
| Rounded-rect 9-slice, search glyph, check tick, empty glyph, scrollbar handle, top-highlight | generated at runtime by `BuildModeUiStyle` (procedural) | n/a (code) | no files; cached statics |

### Post-demo upgrade path (NOT for this week)

If the asset browser must scale to thousands of items: migrate ONLY that panel to UI Toolkit
(USS tokens mirroring these tokens + virtualized `ListView`/`TreeView`). If a richer icon system
is wanted later: add Lucide (ISC, no attribution) or game-icons.net (CC BY 3.0, credit line) as a
TMP sprite atlas. If softer depth is ever desired: `mob-sakai/UIEffect` (MIT). All deferred.

---

## 6. BUILD ORDER (so the first blind pass is excellent)

P0 (look + the centerpiece): tokens (1.1) + `RoundedSprite` + `ButtonJuice` state matrix +
`MakeScrollGrid` + `MakeAssetCard` -> swap the left palette to the thumbnail grid. This alone
turns "programmer art text list" into a premium catalog.
P1: `MakeTabBar` (data-driven categories) + `MakeSearchField` (filter + `-` exclude) +
`MakeSegmented` promotion + slim scrollbar + empty-state.
P2: thumbnail-size `- / +`, live-count line in the hint box, top-highlight polish.
P3 (post-demo): UI Toolkit migration of the grid only; optional icon atlas; optional UIEffect.

---

## 7. ACCEPTANCE (since UI is not screenshottable)

Spec-level checklist the implementation must satisfy (verify by reading code + `*ForValidation`):
- All controls read tokens from `BuildModeUiStyle` (no literal colors/sizes in controllers).
- Every interactive control has the full state matrix (idle/hover/pressed/selected/disabled).
- Left palette is a 2-column thumbnail grid of cards with full-card click targets; selection still
  drives `SelectPalette` and `ApplySelected`.
- Tab bar + search are data-driven (adding a category/filter = a list entry, no rewrite).
- No new packages, no new fonts, no new art files; `*ForValidation` hooks unchanged; canvases
  still `OwnCanvas`-exempt from the overlap-hide pass.
```
