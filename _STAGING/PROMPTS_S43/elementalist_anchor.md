# Elementalist — Anchor #1 Prompt (S43, v2)

**Tool:** PixelLab Create image S-XL (new)
**Target:** 128×128 native, Anchor #1 PNG → v3 Create Character ref → 4 yön (S/E/N/W)

**v2 değişiklik (v1 FAİL → re-roll #1):**
1. Bun position düzeltme — top bun çıkıyordu, low-at-nape vurgusu güçlendi
2. Chibi blokla — 7-head realistic proportion eklendi
3. Ground shadow YASAK — runtime'da Unity'de eklenecek, sprite'a bake yok
4. Antique gold filigree görünür kıl — 2-3px ornamental edge

## UI

| Alan | Değer |
|---|---|
| Direction | South (facing camera) |
| View | **Low top-down** |
| Detail | Low detailed |
| Outline | Single color outline |
| Width × Height | **128 × 128** |
| Transparent BG | ✓ |

## Description

```
A female arcane warrior, mid-20s, athletic mature adult build with realistic 7-head body proportion — head fits about 1/7 of total height, NOT cute, NOT big-head, NOT chibi, NOT super-deformed. Classic dark-fantasy ARPG pixel art with heavy single-tone dark outline and painterly weathered shading. Camera 30-35° low top-down ARPG 3/4 view — face fully readable but slightly tilted forward, mild leg foreshortening with feet positioned slightly closer to camera than the head, top of the shoulders and crown of the head faintly visible from above. Diablo 2 / Last Epoch / Cursemark dungeon-crawler vibe, muted cool palette dominated by DUSTY INDIGO on top and skirt with warm gold-cream signature accent on the orb.

She stands FACING DIRECTLY SOUTH — body fully squared to camera, chest centerline pointing 100% toward the viewer, NO body twist, NO contrapposto lean, NO diagonal angle, NO weight shift to one side. Both shoulders perfectly horizontal and level. Both feet pointing directly forward toward the camera, hip-width apart, weight evenly centered. Body centerline vertical, chest and shoulder line tilted slightly forward toward the camera (top-down tilt only, no left-right rotation). Warm golden honey-blonde hair tied in a LOW rounded bun positioned at the LOWER BACK of the skull at neck level — bun sits at the nape, NOT on top of the head, NOT a high ponytail, NOT a top-knot. The bun reads as a small blonde mass at the back of the neck just below the rear hairline. Two short golden-blonde loose strands frame the face at cheek level. Fair skin, calm focused expression.

She wears a tight sleeveless DUSTY-INDIGO crop top ending high above the navel — the entire toned midriff is bare and fully visible from the front. Short asymmetric DUSTY-INDIGO miniskirt sitting low on the hips, with a clearly visible 2-3 pixel ANTIQUE GOLD FILIGREE ornamental edge running along the skirt hem. Black fitted tights, mid-calf brown-leather boots, fingerless brown-leather gloves, narrow gold-trim belt with a small dark pouch on one hip.

She holds a small softly-glowing gold-cream orb at chest height — the orb hovers just above one open palm raised in front of her body, signature warm accent emitting a 1-2 pixel cream glow halo. The other arm hangs relaxed at her side, hand empty.

Palette: DUSTY INDIGO dominant (top + skirt), cream-ivory trim, antique gold filigree, warm fair skin, dark leather brown, gold-cream orb glow. NO grey-dominant, NO desaturated muddy palette.

Transparent background. Character only — NO baked ground shadow, NO ellipse beneath feet, NO contact shadow, NO drop shadow. Shadow will be composited in the game engine as a separate runtime sprite.

Avoid: ground shadow, ellipse shadow, soft oval shadow under feet, contact shadow, drop shadow, baked shadow, top bun, high bun, top-knot, high ponytail, bun on crown of head, long robe, full-length dress, ankle-length cloth, cloak, cape, hood, staff, quarterstaff, polearm, bikini, swimsuit, eye-level front portrait, side view, diagonal hip twist, contrapposto lean, body twist, diagonal body angle, weight shifted to one side, chibi, super-deformed, big-head proportion, top-down bird-eye view, scholar attire, brown hair, dark hair.
```

## QC 5 kapı (v2 — gölge kriteri çıktı, 4 kamera ipucu girdi)

1. **Identity** — LOW bun at nape ✓ (tepede DEĞİL), bare midriff ✓, cropped top ✓, miniskirt ✓ (2-3px filigree edge görünür), orb chest height ✓
2. **Kamera 30-35°** — 4 ipucu hepsi ✓:
   - Foreshortening: bacak/ayak kafadan görece büyük, ayaklar resmin dibinde belirgin
   - Üst yüzey: omuz üstü + saç crown + bun üstü hafif görünür
   - Body tilt: göğüs/omuz çizgisi hafif öne yatık (dikey duvar gibi değil)
   - Feet position: ayaklar body centerline'ın hafif önünde (kameraya yakın)
3. **Negation** — robe, cape, staff, hood, top-bun, ground shadow yok
4. **Palette** — dusty indigo dominant, gold filigree edge görünür, warm fair skin, no grey-mud
5. **Outline + face + proportion** — tek koyu ton outline, eritilmiş yüz yok, **7-head realistic proportion (NOT chibi)**

5/5 PASS → Anchor #1 lock → `_STAGING/anchors/elementalist/elementalist_anchor_128.png` rename + lock
4/5 → tweak revize, 1 re-roll
3/5 veya altı → identity blok revize gerekebilir

**v1 sonuç (referans):** 3.5/5 — top bun (kapı 1 FAIL) + chibi sınırı (kapı 5 ⚠️) + filigree görünmez (kapı 4 ⚠️). v2 bu üçünü hedefliyor.

## Sonraki adım (PASS sonrası)

1. PNG `_STAGING/anchors/elementalist/elementalist_anchor_128.png` rename + lock
2. PixelLab v3 Create Character → Reference Standard mode
3. Ref slot: anchor PNG (128×128, upscale YOK)
4. Direction: South (4 yön için baz)
5. character_id al → kayıt: `_STAGING/character_ids.md`
6. 4 yön (S/E/N/W) Anchor sprite üret (Rotate veya MCP)
7. `mcp__pixellab__animate_character` via id → idle/run/attack/skill/hit
