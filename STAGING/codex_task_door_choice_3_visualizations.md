# Codex Task — Door Choice Mechanic 3 Proposal Visualizations (Imagegen)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

3 ayrı door choice mechanic proposal'ı **imagegen** ile görselleştir — sen (Codex) imagegen tool'unu (gpt-image-1) kullanarak her proposal için 1 görsel üret. User görsel kıyaslama ile final karar verecek.

Reference doc: `STAGING/_research/BUFFER_FILL_DOOR_CHOICE_BRAINSTORM.md`

## Output

3 ayrı PNG, hepsi dark fantasy pixel art, isometric 30° dimetric, RIMA Shattered Keep theme:

| Dosya | İçerik |
|---|---|
| `STAGING/_pixellab_outputs/door_choice/proposal_A_echo_loom_fractures.png` | Echo Loom Fractures görseli |
| `STAGING/_pixellab_outputs/door_choice/proposal_B_mirror_remnant_triptych.png` | Mirror Remnant Triptych görseli |
| `STAGING/_pixellab_outputs/door_choice/proposal_C_defender_echo_silhouettes.png` | Defender Echo Silhouettes görseli |

**Boyut:** Her biri 1024×1024 (DALL-E native), pixel art, dark moody atmosphere.

## Proposal Specs

### A) Echo Loom Fractures (Codex'in #1 önerisi)

**Prompt for imagegen:**
```
A dark fantasy isometric dungeon room exit chamber with three vertical glowing cyan rift seams torn into the stone wall background, side by side. Each seam is a vertical tear in reality showing a glimpse of a different sub-room interior through the rift glow. The three seams have small floating glyph icons hovering in front of them: a crossed-blades glyph (combat room), a skull-crown glyph (elite room), and a coin glyph (shop room). Between and around the seams are remnants of a broken stone ward tablet on the floor, with cyan magical energy leaking from the cracks. Dark granite block walls surround the scene. Atmospheric particles drift in the air, with cyan glow illuminating the foreground. 30-degree dimetric isometric pixel art inspired by Hades and Diablo. Sharp pixel edges, no anti-aliasing, dark moody underground sanctuary atmosphere. Painterly chunky pixel art density. The room shows the player's perspective approaching from the south, with the three rift seams as the only exit.
```

### B) Mirror Remnant Triptych

**Prompt for imagegen:**
```
A dark fantasy isometric dungeon chamber with three broken standing mirrors arranged in a fan along the back wall. Each mirror is framed in dark weathered iron, the glass is cracked and partially missing. Inside each mirror's reflective surface, a distorted reflection of a different sub-room is visible: one shows a combat arena with weapon racks, one shows an elite enemy chamber with a throne, one shows a merchant's stall with stacked coins. The mirrors have a slight cyan magical shimmer on their cracked edges. Dark granite block walls surround the scene. The floor has dust scatter and broken mirror shards. 30-degree dimetric isometric pixel art inspired by Hades and Diablo. Sharp pixel edges, no anti-aliasing, dark moody underground sanctuary atmosphere. Painterly chunky pixel art density. The room shows the player approaching from the south, with three mirrors as the only choice exit.
```

### C) Defender Echo Silhouettes

**Prompt for imagegen:**
```
A dark fantasy isometric dungeon chamber with three ghostly translucent silhouettes of fallen warriors arranged at the back wall, glowing with a faint cyan magical aura. Each silhouette has a different pose suggesting a different room type: one silhouette holds a raised weapon ready for combat, one silhouette kneels wounded with a crown shard, one silhouette holds a merchant's pack. Behind each silhouette is a faint magical archway outline in the stone wall. Cyan magical wisps drift between the silhouettes. Dark granite block walls surround the scene. The floor has Death Imprint footprints showing the player's previous death trails. 30-degree dimetric isometric pixel art inspired by Hades and Diablo. Sharp pixel edges, no anti-aliasing, dark moody underground sanctuary atmosphere. Painterly chunky pixel art density. The room shows player perspective approaching from the south.
```

## Style Common Across All 3

- Pixel art (sharp pixel edges, no anti-aliasing, no smooth gradients)
- 30-degree dimetric isometric angle (2:1 ratio)
- Dark moody dungeon atmosphere (granite + cyan magical accents)
- Painterly chunky pixel art density (Hades/Diablo inspired)
- Palette: dark granite mid-tone, deep dark mortar, vivid cyan magical accent (#40D0E0 to #80F0FF)
- Atmospheric: ancient abandoned underground sanctuary

## Critical Requirements

- 3 ayrı PNG dosya
- Her biri 1024×1024
- RGBA transparent background tercih edilir (yoksa beyaz/koyu BG OK, user görsel kıyaslama için)
- NO TEXT in images (no labels)

## Effort

high — 3 ayrı imagegen call, prompt iterations gerekebilir.

## Output Confirmation

3 PNG path + visual description of each + iteration count if multi-try.
