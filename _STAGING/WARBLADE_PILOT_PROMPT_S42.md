# Warblade Pilot — Anchor #1 (S42 v3)

**Tarih:** 2026-04-26 · **Pipeline:** Create Character → Create from Reference → Standard

## UI Settings

| Alan | Değer |
|---|---|
| Tab | **Create from Reference** |
| Generation Mode | **Standard** (Pro = NEW = atla) |
| Character Type | **Humanoid** |
| Camera View | **High Top-Down** ⭐ |
| Reference Image (South slot) | `_STAGING/chatgpt_pixel_grid_s42/clean_outputs/01_blue_armor_knight_64x64_transparent.png` (orijinal 64×64, upscale YOK) |
| Diğer 7 ref slot | **boş** (Hero Siege ref'leri tek angle'da) |
| Quick Preset (varsa) | **Heroic** |

**Output:** 64×64 (Reference mode'da output = ref size)

## Character Description (kopyala-yapıştır)

```
Male disciplined battle-worn human warrior, mature serious face, short dark hair, light beard.

Heavy dark steel plate armor over warm brown leather straps. Layered shoulder pauldrons. Battle-worn metal with subtle scratches. Brown leather belt and bracers.

Holds a massive two-handed greatsword resting against his right shoulder, blade pointing up and slightly back. Both hands on the hilt.

Color palette: dark steel gray, warm brown leather, ember orange and silver accents only. No bright blue magic. No purple glow. No green.

Face must be fully visible — no helmet covering the face, no full-face mask. Clean readable face even at small size.

Grounded serious dark fantasy. Heavy readable silhouette. Sharp pixel clusters. Limited palette 2-3 shade steps per material. No dithering. No painterly soft rendering. No cute chibi. No childish proportions.

Transparent background. No text. No labels. No UI.
```

## Workflow

1. PixelLab UI → Create Character → Create from Reference → Standard
2. Reference Images → **South** slot → orijinal 64×64 blue knight yükle
3. Character Type: Humanoid · Camera View: High Top-Down · Heroic preset
4. Description: yukarıdaki blok yapıştır
5. **Run**
6. Output (64×64) + `character_id` döner
7. PASS → PNG `_STAGING/anchors/anchor_male_warrior.png` olarak kaydet, `character_id`'yi `_STAGING/character_ids.md`'ye yaz
8. PASS sonrası → kalan 9 class + 6 mob aynı pipeline'la (Anchor #1 ref olarak kullanılır)

## QC 5 Kapı

| # | Kriter |
|---|---|
| 1 | Kamera: kafanın üstü görünür mü? Eye-level/portrait ise FAIL |
| 2 | Identity: greatsword + dark steel + ember palette tutmuş mu? |
| 3 | Palette: blue/purple/green sızıntı yok mu? |
| 4 | Face: net, simetrik, melted değil mi? |
| 5 | Outline: tek renk koyu, strong outline mı? |

5/5 = PASS · 4/5 = tweak · ≤3/5 = re-roll

## FAIL Tweak

- Kamera kayar → Description'a tek satır: `Top of head and shoulder caps clearly visible from above.`
- Palette sızıntı → negatif vurgu güçlendir (`Absolutely no blue glow.` 2-3 yerde)
- Yüz melted → `Face must be clean, symmetric, centered. No melted features.`
- Çok zayıf gövde → Custom slider Shoulder Width 1.2x

## Sonraki Adım (PASS sonrası)

Bu dosyada **sadece Warblade** var. Kalan 9 class + 6 mob için:
- Class identity blokları: `_STAGING/ROSTER_PROMPT_S42.md` (cell #2-#16, kamera/size/outline satırları çıkarılarak kullan)
- Per-class ref tablosu: `GUIDES/PIXELLAB_ANCHOR_PIPELINE_S42.md`
