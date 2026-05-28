# Codex Task — Ritual Chamber Concept via $imagegen (Built-in) (2026-05-24)

ACTIVE RULES: (1) think before generating (2) min code (3) surgical (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: Full ritual chamber concept image üret. **Built-in `image_gen` tool kullan** (gpt-image-1, $imagegen) — bdkrtgasb dispatch'inde kullandığın aynı pipeline. gpt-image-2 (skill version) bu setup'ta çalışmıyor.

## Görev

1. Built-in `image_gen` tool ile ritual chamber concept üret
2. Boyut: 1024×1024 (built-in max square)
3. Output: `STAGING/concepts/ritual_chamber_imagegen_v1.png`

## Prompt

```
RIMA Shattered Keep ritual chamber room, top-down 3/4 angle view (HIGH TOP-DOWN, ~70-80 degrees from horizon, Hades / Children of Morta / Diablo III camera reference, NOT true isometric diamond grid).

Diamond/irregular room footprint with bevelled corners, NOT square box. Two visible wall chains — back wall along top edge and side wall along right/left edges. Front edge open or low parapet for combat visibility.

Center of room: stone ritual altar with floating cyan rift crystal (1m tall, glowing), concentric stone runes carved into floor radiating outward from altar, glowing softly with cyan light.

Walls: Shattered Keep gothic dark grey stone masonry with deep cracks emitting subtle cyan rift glow. Stone pillars at every corner junction (seam-cover style, Diablo II Orientation Index 12 pattern). One stone arch doorway visible with empty black void interior (no wooden door panel). NO banners baked into walls.

Lighting: warm orange torch flames mounted on walls (wall-mounted brazier sockets), cyan rift glow emanating from floor cracks and central crystal, dramatic chiaroscuro contrast — cool/warm color play.

Style: Vivid Vulnerability tonal model — bold saturated colors, dramatic atmospheric lighting, NOT grimdark despair NOT cartoon, pixel art aesthetic with limited palette (~32 colors).

Camera framing: top-down 3/4 perspective showing full room interior — floor plan visible, walls have slight 3/4 thickness top cap, character-scale reference would be roughly 1:5 to 1:6 ratio (small heroic figure vs monumental room).

Reference inspiration: Hades altar rooms, Children of Morta dungeon chambers, Diablo II ritual sites.
```

## Verification

1. Output PNG exists at requested path
2. Image size ~1024×1024
3. RIMA palette tutarlı (cyan rift + warm torch + dark stone)

## Çıktı Raporu

`STAGING/codex_ritual_chamber_DONE.md` yaz:
- Output path
- Image size verified
- Visual observations
- Issues / blockers

git commit otomatik yapma.
