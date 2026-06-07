# Codex Task — Ritual Chamber Room Concept via gpt-image-2 (2026-05-24)

ACTIVE RULES: (1) think before generating (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: gpt-image-2 skill ile RIMA Shattered Keep ritual chamber concept üret (2048×2048 high-res). Bu konsept Dispatch B'nin (modüler wall pack) style reference'ı olacak. Ayrı ChatGPT hesabıyla paralel oda + modüler yapı pipeline'ının first half'i.

## Skill kullanımı

**Skill: gpt-image-2** (Codex skill, ChatGPT Plus subscription, no API key)

Skill açıklaması: "Generate images with GPT Image 2 (ChatGPT Images 2.0) inside Claude Code, using your existing ChatGPT Plus or Pro subscription. Supports text-to-image, image-to-image editing, style transfer, and multi-reference composition via the local Codex CLI."

## Prompt

```
RIMA Shattered Keep ritual chamber room, high top-down 3/4 angle (camera ~70-80 degrees from horizon, Hades / Children of Morta / Diablo III reference angle, NOT true isometric diamond).

Room footprint: diamond/irregular shape with bevelled corners (NOT square box). Two visible wall chains — back/upper wall chain and side wall chain. Front/lower edge open or low parapet for combat readability. Player entry point at south edge.

Center of room: ritual altar with floating cyan rift crystal, glowing runes carved into stone floor in concentric circles. One Warblade character (mature heroic male, 5-6 head proportions, dark armor with cold blue accents #66AAFF, two-handed greatsword in hand) standing at center facing player.

Walls: Shattered Keep gothic stone masonry, dark grey stone blocks with deep cracks emitting subtle cyan rift glow. Pillars at corner junctions as seam-cover (Diablo II Orientation Index 12 pattern). Stone arch doorway with empty void interior (no wooden door panel baked in). No banners baked into walls (banner = separate prop, not in this concept).

Lighting: warm orange torch light from wall-mounted braziers (separate props), cyan rift glow from floor cracks and ritual crystal, dramatic chiaroscuro. Cool/warm contrast — Hades theatrical mythic + Salt and Sanctuary chibi-but-serious tone.

Style: pixel art aesthetic at 2048×2048 (will be downscaled to true pixel art separately). Vivid Vulnerability tonal model — bold colors, dramatic lighting, NOT grimdark despair NOT cute cartoon. 

Composition: high top-down 3/4 angle showing room floor plan plus subtle wall thickness top cap. Wall pieces clearly modular (will be reproduced as connector-column + wall span + seam overlay system).

DO NOT generate isometric 45-degree diamond camera. DO NOT bake torch flames into walls. DO NOT bake banners. DO NOT generate true square room — must be diamond/irregular footprint with corner bevels.
```

## Reference inputs (multi-reference)

Aşağıdaki dosyaları gpt-image-2 multi-reference slot'larına ekle:

1. **chatgpt_ref dosyaları** (RIMA primary visual target):
   - `STAGING/CHATGPT_TOPDOWN/*.png` (1-3 örnek seç)
2. **Mevcut wall asset palette anchor**:
   - `STAGING/concepts/fractured_chamber/iso_assets/wall_nw_mid_plain.png`
   - `STAGING/concepts/fractured_chamber/iso_assets/iso_floor_rift_glow.png`
3. **Warblade character placement reference**:
   - `Assets/Art/Characters/Warblade/Rotations/warblade_south.png`
4. **Master room composition**:
   - `STAGING/concepts/master_room_pilot/room_v1_gptimage.png` (varsa)

## Output

- Path: `STAGING/concepts/ritual_chamber_room_concept_gpt_image_2_v1.png`
- Boyut: 2048×2048 (veya gpt-image-2 native max)
- Format: PNG
- 2-3 variation üret (seed farklı), en iyisi seçilsin

## Verification

1. PNG dosyası dosya sisteminde var
2. Boyut 2048×2048 veya daha büyük
3. RIMA palette + composition uyum:
   - High top-down 3/4 angle (NOT 45 iso)
   - Diamond/irregular footprint
   - Cyan rift accents
   - Warblade in center

## Çıktı raporu

`STAGING/codex_room_concept_report_2026-05-24.md` yaz:
- Generated file path(s)
- Quality observations (RIMA fit, composition, palette)
- Issues (varsa)
- Recommendation for Dispatch B input

git commit otomatik yapma — orchestrator review sonrası.
