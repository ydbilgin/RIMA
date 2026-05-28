# Codex Task — ChatGPT Spec vs Mevcut WallChainBuilder Eval (2026-05-24)

ACTIVE RULES: (1) think before reviewing (2) min output (3) surgical (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: ChatGPT'nin 7-infographic comprehensive spec'ini (CHATGPT_CRIT_ONERI/ klasörü + chat'teki tam text) mevcut RIMA WallChainBuilder implementation'ına karşı kıyasla. Net implementation gap report ver — hangi asset'ler üretilecek, hangi prefab eksik, hangi sistem genişletilecek.

**Bu eval task — kısa, ~30 dk. Üretim yok.**

## ChatGPT Spec Özeti (ana eleman sayıları)

**A — FLOOR / FOOTPRINT (6):** clean, cracked, rift cracked, ritual, flooded, broken edge

**B — CONNECTOR / COLUMN (10):**
1. Straight connector column
2. Outer corner connector  
3. Inner corner connector
4. End column
5. Door column left
6. Door column right
7. Alcove connector
8. Protrusion connector
9. Half-height connector
10. Tall connector column

**C — WALL SPAN (12):**
1. Wall span 1x (1 tile)
2. Wall span 2x (2 tile)
3. Wall span 3x (3 tile)
4. Cracked wall span
5. Broken wall span
6. Prison-bar wall
7. Library/bookcase wall
8. Ritual wall
9. Low parapet wall
10. Flooded low wall
11. (Variations cracked/broken için)
12. Stair/edge wall span

**D — SEAM / JOIN (14):**
1. Straight seam (straight-to-straight)
2. Corner seam (straight-to-corner)
3. Inner-corner seam
4. Outer-corner cap
5. Pillar-to-wall seam
6. Doorway jamb seam
7. Base shadow strip
8. Broken-wall rubble seam
9. Crack continuation horizontal
10. Crack continuation vertical
11. Crack corner continuation
12. Waterline transition seam
13. Low-parapet transition seam
14. End-cap seam

**E — SOCKET PROPS (13):**
1. Torch
2. Brazier
3. Banner
4. Chain
5. Skull chain
6. Shield plaque
7. Shelf
8. Bookshelf insert
9. Candle stand
10. Crate
11. Barrel
12. Small statue
13. Cyan rift crystal sprout

**TOPLAM: 55 yeni asset** (mevcut 17 wall asset 11'i wall span'lere maps eder).

## Mevcut RIMA Implementation (snapshot)

**17 wall asset (commit fd00ad23):**
- 6 floor (128×64): clean, cracked, rift_glow, broken, edge_light, debris
- 8 wall span (128×384): nw/ne × plain/variant/broken/doorway
- 1 corner (128×384): wall_n_corner
- 1 landmark (256×384): wall_n_landmark
- 1 pillar (64×384): wall_pillar_universal

**WallChainBuilder current (a05b46bff8ca62b97 agent çalışıyor):**
- 4 script: WallChunkData / WallChunk / WallChainBuilder / WallChunkLibrary
- 1 scene: DiamondRoom_v1.unity
- Footprint anchor + socket pattern (PARTIAL — ChatGPT'nin 6-socket'ı yerine 6 var ama eksik typing)
- Algorithm: polygon → connector + span + seam (seam logic placeholder)

## Eval Görevleri

### Q1 — Asset Mapping (mevcut 17 → ChatGPT 55)

Tablo: her mevcut asset hangi ChatGPT kategorisinin hangi item'ına maps?

| Mevcut asset | ChatGPT category | ChatGPT item | Notes |
|---|---|---|---|
| iso_floor_clean | A — Floor | clean stone floor | direct match |
| iso_floor_cracked | A — Floor | cracked stone floor | direct match |
| ... | ... | ... | ... |

### Q2 — Eksik Asset List (priority sıralı)

Üretim sırası önerin nedir? P0 (MVP zorunlu) / P1 (gerekli, sonra) / P2 (nice-to-have).

Örnek format:
```
P0 (MVP, ~10 asset):
1. Outer corner connector (3 var ama mevcut sadece N — NE/NW/SE/SW gerekli)
2. Door column left + right
3. Straight seam (8 seam'in en çok kullanılanı)
...

P1 (production, ~20 asset):
...

P2 (decorative, ~25 asset):
...
```

### Q3 — Mevcut Implementation Gap

Mevcut WallChainBuilder.cs algorithm'ı ChatGPT'nin "Connector → Span → Connector" mantığını uygulayabiliyor mu? Eksik fonksiyon/data var mı?

Spec dağılım: footprint anchor (✓), socket types (eksik: BackSocket, FrontSocket, ChainSocket vs şu an Torch/Banner/Seam), seam algorithm (placeholder).

Specific eksik:
1. ...
2. ...

### Q4 — 5 Oda Arketipi Implementation

ChatGPT 5 detayl oda örneği veriyor (Combat Arena, Ritüel, Hapishane, Kütüphane, Sular Altında Mezar). Her biri için:
- RoomFootprintPolygon vertices (önerilen)
- Hangi connector + wall span combo
- Hangi seam piece'ler kritik
- Hangi prop'lar uygun

Tablo halinde 5 oda.

### Q5 — PixelLab Batch Sheet Strategy

55 asset → PixelLab S-XL batch sheet'lerine nasıl böleceğiz?

| Sheet | İçerik | Boyut | Piece sayı |
|---|---|---|---|
| A | 8 missing connectors | 256×768 (4×2 grid) | 8 |
| B | ... | ... | ... |

(Karar #90 batch ekonomi + 2026-05-24 wall consistency strategy lock'a uygun)

## Çıktı

`STAGING/codex_chatgpt_spec_eval_DONE.md` yaz:

```markdown
# ChatGPT Spec Eval — RIMA Implementation Gap (2026-05-24)

## Q1 — Asset Mapping
[table]

## Q2 — Eksik Asset List (P0/P1/P2)
[lists]

## Q3 — Mevcut Implementation Gap
[specific list]

## Q4 — 5 Oda Arketipi Implementation
[table]

## Q5 — PixelLab Batch Sheet Plan
[table]

## Verdict
- Implementation alignment: [HIGH / MEDIUM / LOW]
- MVP path: [steps]
- Production path: [steps]
- Risks: [items]
```

git commit otomatik yapma.

## Effort

30-40 dk Codex eval. Üretim yok. Independent analysis.
