# Task: Cute-Style Boss Fight Concept Images (3x gpt-image-1)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: Query NLM first for boss details:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "Act 1 boss kim, boss fight mechanic nedir, boss görsel tasarımı"

## Amaç
Şu anki karanlık gotik stil yerine "cute/stylized" ambiyansın üretim kolaylığı açısından ne kadar uygulanabilir olduğunu test etmek için 3 adet referans konsept görseli üretmek. Aynı karakterler + aynı boss arketipi, farklı görsel dil.

## Karakterler (CANONICAL — değiştirme)
- **Warblade** — genç chibi, longsword, Act 1 primary hero
- **Elementalist** — orb taşıyan büyücü chibi
- **Boss:** Act 1 Shattered Keep boss — NLM'den çek, bulamazsan "stone golem / corrupted guardian" arketipi kullan

## Üretilecek 3 görsel

Her görsel ayrı bir Python scripti ile `gpt-image-1` API'sına POST atılacak ve STAGING/'e kaydedilecek.

### Görsel 1 — Oda Genel Görünüm (top-down)
**Dosya:** `STAGING/boss_concept_01_room_overview.png`
**Prompt:**
```
Top-down 2D pixel art, cute chibi style, boss fight room in a fantasy dungeon. 
Rounded stone floor tiles in warm beige/cream tones. Simple rounded pillar props in corners. 
Cute glowing cyan cracks on floor as decoration. Two small chibi hero characters (one sword warrior, one mage with orb) face a large cute stone golem boss in center. 
Boss is big, round-shaped, friendly-scary design. Soft color palette, readable silhouettes. 
64x64 tile grid visible. No dark gritty textures. Stardew Valley / Undertale aesthetic influence.
```

### Görsel 2 — Boss Yakın Çekim (3/4 view)
**Dosya:** `STAGING/boss_concept_02_boss_closeup.png`
**Prompt:**
```
2D pixel art, cute chibi top-down perspective (85-90 degree camera), boss fight encounter closeup. 
Large cute stone golem boss (round body, glowing cyan eyes, simple chunky stone arms) looming over small chibi Warblade hero with tiny sword. 
Floor: warm cream stone tiles with simple rounded edges. Props: cute small barrels, round glowing crystals, tiny skull decorations. 
Soft lighting, warm orange torch glow + cool cyan boss glow. 
Readable shapes at small pixel scale. Friendly-scary tone, not grimdark.
```

### Görsel 3 — Full Party vs Boss (action pose)
**Dosya:** `STAGING/boss_concept_03_party_action.png`
**Prompt:**
```
2D pixel art top-down view, cute chibi style, epic boss fight moment. 
3 chibi heroes (sword warrior, mage with orb, hooded ranger) surround a large cute stone golem boss. 
Boss has simple AoE attack pattern shown with cute rounded ground marker circles in soft red/orange. 
Room props: round pillars, glowing cyan rune tiles, cute treasure chests pushed to corners. 
Color palette: warm stone beige + cyan accent + soft orange torch. 
Clean readable silhouettes. Fantasy roguelite, Hades-meets-Stardew visual tone.
```

## Teknik

```python
import openai, base64, os, pathlib

client = openai.OpenAI()  # OPENAI_API_KEY env var

images = [
    ("boss_concept_01_room_overview.png", PROMPT_1),
    ("boss_concept_02_boss_closeup.png", PROMPT_2),
    ("boss_concept_03_party_action.png", PROMPT_3),
]

for filename, prompt in images:
    response = client.images.generate(
        model="gpt-image-1",
        prompt=prompt,
        size="1024x1024",
        quality="medium",
    )
    img_data = base64.b64decode(response.data[0].b64_json)
    out_path = pathlib.Path("STAGING") / filename
    out_path.write_bytes(img_data)
    print(f"Saved: {out_path}")
```

Scripti `STAGING/gen_boss_concepts.py` olarak yaz ve çalıştır.

## Başarı Kriterleri
- 3 PNG dosyası STAGING/'de mevcut
- Her biri 1024x1024
- Konsol çıktısı "Saved: STAGING/boss_concept_0X..." × 3

## Output
Tamamlandığında CODEX_DONE.md'ye yaz:
- Hangi 3 dosya üretildi
- Herhangi bir API hatası olduysa ne
- NLM'den çekilen boss detayı (varsa)
