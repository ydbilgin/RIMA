# CODEX DONE - Overnight Skill Draft UI

- PNG path: `STAGING/concepts/overnight/08_skill_draft_3choice.png`
- Card layout readability verdict: PASS. Three horizontal cards are clear at 1280x800, with distinct icon areas, titles, body copy, footer copy, and visible center alignment. Card 3 title is the tightest but remains readable.
- Tier color scheme final onerisi:
  - Common: warm white / silver border, low glow.
  - Rare: cyan border, moderate rift glow.
  - Epic: violet / magenta border, stronger inner glow.
  - Legendary: rusty gold border, strongest ornamentation and highlight.
- Implementation notes (UGUI vs UI Toolkit): Use UGUI for the runtime reward draft screen. It fits animated cards, controller/gamepad focus, ornate sliced sprites, hover/selection tweens, and pixel-perfect tuning better for first playable. UI Toolkit can be kept for editor/debug reward tooling, but the in-game draft modal should be a UGUI prefab with 3 reusable card slot prefabs, rarity style data, and explicit navigation order.
