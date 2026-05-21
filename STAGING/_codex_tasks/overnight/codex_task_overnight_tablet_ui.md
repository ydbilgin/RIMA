# CODEX TASK — Overnight: Kırık Taş Tablet MapPanel UI

ACTIVE RULES: (1) think before coding (2) min code (3) surgical (4) BLOCKED if unclear.

Codex built-in imagegen tool. Output → `STAGING/concepts/overnight/`.

---

## Hedef

Karar #63 LOCK: "**Kırık Taş Tablet**" Map UI mockup.

## Görsel Spec

**3 PNG render:**

### Render 1: `05_tablet_mappanel_act1.png` (1280×800)
TAB full-screen MapPanel — StS-style.

İçerik:
- Background: dark dungeon backdrop
- Center: **Kırık Taş Tablet** — abstract grid, **paslı altın çerçeve**, cyan rift çatlakları
- 15 node graph (Act 1):
  - Revealed nodes (current run): visible node icon + type symbol
  - Open nodes (puslu): semi-transparent silhouette
  - Hidden nodes: full obscured
- Boss node at top: **8-fragment slot indicator** (filled count / 8)
- Branch nodes (B01, B02): asymmetric placement
- Connectors: cyan rift line between revealed nodes
- Player position marker: small cyan rift seam on current node
- Side: legend (icon meanings)

### Render 2: `06_tablet_minimap_128.png` (800×600)
Sol-üst köşe HUD MiniMap (Hades-style 128×128) — ekran context'i göster.

İçerik:
- Game scene background (current room arena rendered loosely)
- Top-left corner: 128×128 minimap overlay (paslı altın çerçeve + cyan rift)
- Within minimap: micro node graph (5-7 node visible window)
- Player marker
- Health/fragment counter at corner (UI HUD context)

### Render 3: `07_tablet_4act_evolution.png` (1280×320, 4×1 row)
4-Act görsel evrim — Karar #63 zorunlu.

| Slot | Act | Visual |
|---|---|---|
| 1 | **Act 1** | Castle carvings — granite, paslı altın, cyan rift basic |
| 2 | **Act 2** | Damarlı et (veined flesh) — organic, bone, rust accent |
| 3 | **Act 3** | Yüzen parçalar (floating pieces) — void background, gold flecks |
| 4 | **Act 4** | Ayna (mirror) — reflective, ethereal, multi-color rift |

Her slot 320×320 zone, kompakt tablet showcase.

## Stil

- Pixel art UI mockup (Hades + StS hybrid)
- Cyan rift signature
- Painterly, RIMA Style Manifesto compliant
- Frame ornate but not Hades-clone

## Output

3 PNG yukarıdaki path'lerde.

## Final Report

`STAGING/CODEX_DONE_overnight_tablet_ui.md`:
- 3 PNG path + alpha analysis
- Tablet metaforu lore-fit verdict
- Implementation notes (Unity UGUI canvas? UI Toolkit? Atlas?)

Background.
