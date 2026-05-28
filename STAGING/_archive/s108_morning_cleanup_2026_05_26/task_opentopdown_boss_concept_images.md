# Task: Open Area Top-Down Boss Fight Concept Images (3x gpt-image-1)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Amaç
Duvar üretim sorununa çözüm olarak "daha açık alan, minimal duvar" yaklaşımının görsel olarak nasıl durduğunu test etmek. Mevcut perspektif açısında tall isometric duvar sprite üretmek zor — daha saf top-down açıyla duvarlar sadece ince kenar şeridine iner, üretmesi trivial olur. 3 konsept görsel bu yaklaşımı test eder.

## Stil Yönü
- Kamera: ~90 derece saf top-down (Zelda BotW / Hades map view değil, gerçek kuş bakışı)
- Duvarlar: tall stone slab değil, ince kenar/shadow strip — zemin dokusu öne çıkar
- Alan: açık, geniş boss arena — duvar sadece sınır olarak
- Karakterler: aynı chibi 64x64 kadrosu (Warblade + Elementalist + boss)
- Renk: şimdilik Act 1 renk paleti (granite beige + cyan accent) ama duvar yükü minimal

## Üretilecek 3 görsel

### Görsel 1 — Açık Arena Genel (pure top-down)
**Dosya:** `STAGING/opentd_concept_01_arena_overview.png`
**Prompt:**
```
Pure top-down 2D pixel art, 90-degree overhead view, boss fight arena. 
Large open stone floor with subtle tile grid pattern, warm granite beige tones with faint cyan glowing cracks. 
Walls are minimal — just thin dark edge border/shadow strip at room perimeter, no tall wall sprites needed. 
Small chibi hero characters (sword warrior, mage with orb) face a large stone golem boss in open center. 
Floor props: rune circles, small scattered stones, glowing floor crystals. 
Style: clean readable top-down, Hades-inspired open arena. No isometric angle, pure overhead.
```

### Görsel 2 — Boss Arena Detay (zemin dokusu öne çıkar)
**Dosya:** `STAGING/opentd_concept_02_floor_detail.png`
**Prompt:**
```
Pure top-down overhead pixel art, 2D roguelite boss room close-up. 
Detailed stone floor tiles visible from directly above — cracked granite texture, cyan magical rune inscriptions baked into floor. 
Thin wall shadow border at edges only. 
Boss (large stone golem, seen from directly above — round chunky top silhouette) surrounded by 2 chibi heroes. 
AoE ground markers as concentric circles on floor. Torch shadow pools on floor. 
No perspective distortion. Clean shapes, readable at small scale. Diablo 1 / Hades visual reference.
```

### Görsel 3 — Wall-Free Arena (sadece zemin + obje)
**Dosya:** `STAGING/opentd_concept_03_wallless_arena.png`
**Prompt:**
```
Top-down 2D pixel art, completely open boss arena with zero wall sprites — boundary defined only by floor tile cutoff and darkness beyond. 
Stone floor tiles in center, fade to black void at edges (no wall needed, just darkness). 
Chibi Warblade hero (small, sword raised) vs large stone golem boss (top-down silhouette, glowing cyan eyes visible from above). 
Scattered floor props: broken column tops seen from above (circles), rune stones, candle pools. 
Style: minimal, elegant, like Hades arena floor. Pixel art 32px grid.
```

## Teknik

```python
import openai, base64, pathlib

client = openai.OpenAI()

images = [
    ("opentd_concept_01_arena_overview.png", """Pure top-down 2D pixel art, 90-degree overhead view, boss fight arena. Large open stone floor with subtle tile grid pattern, warm granite beige tones with faint cyan glowing cracks. Walls are minimal — just thin dark edge border/shadow strip at room perimeter, no tall wall sprites needed. Small chibi hero characters (sword warrior, mage with orb) face a large stone golem boss in open center. Floor props: rune circles, small scattered stones, glowing floor crystals. Style: clean readable top-down, Hades-inspired open arena. No isometric angle, pure overhead."""),
    ("opentd_concept_02_floor_detail.png", """Pure top-down overhead pixel art, 2D roguelite boss room close-up. Detailed stone floor tiles visible from directly above — cracked granite texture, cyan magical rune inscriptions baked into floor. Thin wall shadow border at edges only. Boss (large stone golem, seen from directly above — round chunky top silhouette) surrounded by 2 chibi heroes. AoE ground markers as concentric circles on floor. Torch shadow pools on floor. No perspective distortion. Clean shapes, readable at small scale. Diablo 1 / Hades visual reference."""),
    ("opentd_concept_03_wallless_arena.png", """Top-down 2D pixel art, completely open boss arena with zero wall sprites — boundary defined only by floor tile cutoff and darkness beyond. Stone floor tiles in center, fade to black void at edges (no wall needed, just darkness). Chibi Warblade hero (small, sword raised) vs large stone golem boss (top-down silhouette, glowing cyan eyes visible from above). Scattered floor props: broken column tops seen from above (circles), rune stones, candle pools. Style: minimal, elegant, like Hades arena floor. Pixel art 32px grid."""),
]

for filename, prompt in images:
    response = client.images.generate(
        model="gpt-image-1",
        prompt=prompt,
        size="1024x1024",
        quality="medium",
    )
    import base64
    img_data = base64.b64decode(response.data[0].b64_json)
    out_path = pathlib.Path("STAGING") / filename
    out_path.write_bytes(img_data)
    print(f"Saved: {out_path}")
```

Scripti `STAGING/gen_opentd_concepts.py` olarak yaz ve çalıştır.

## Başarı Kriterleri
- 3 PNG STAGING/'de mevcut: opentd_concept_01/02/03.png
- Her biri 1024x1024
- Konsol: "Saved: STAGING/opentd_concept_0X..." × 3

## Output → CODEX_DONE.md
- Üretilen 3 dosya listesi
- API hatası varsa ne
