# Codex Task — Buffer Fill + Hades-Alternative Door Choice Brainstorm

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Görev

İki paralel tasarım brainstorm:

### A) Buffer Fill — PixelLab Phase 1 Batch Slots

Mevcut Phase 1 üretim planı:
- **Batch 4 (Medium 64-cell, 16 slot):** 8 active + 8 buffer
- **Batch 5 (Tiny 32-cell, 64 slot):** 8 active + 56 buffer

Bu boş slot'lar BOŞ KALMASIN — Act 1-4 tamamı için mantıklı item'larla doldur. Memory'den Act tema/material:

| Act | Tema | Material |
|---|---|---|
| 1 Shattered Keep | Failed shelter + Convergence | Dark granite + cyan rift |
| 2 Bleeding Wastes | Outside, rift hasarı | #3A2840 bone+rust |
| 3 Core Approach | Rift source | #0A0810 void+gold |
| 4 Nexus Core | Mirror convergence | Mirror reflection |

NLM query yap: `Act 1-4 visual material palette breakdown + decor/prop ideas per Act`

Constraints:
- Items reusable across multiple Acts mümkünse (universal style)
- Item başına Act-specific recolor (state pipeline ile sonra)
- Boyut tier:
  - 32-40 cell: pickup/decal/symbol/floor accent
  - 48-80 cell: prop/item/atmospheric decor
- Phase 2 state pipeline ile aynı base'den damage/decay variants gelecek

Buffer fill için item kategori önerileri:
- Şamdan/torch çeşidi (Act 1 lit/unlit, Act 2 dimmed, Act 3 unstable, Act 4 mirror flame)
- Bookshelf/scroll/lectern (Shattered Keep antik kütüphane elemanları)
- Trap mechanism (spike, pressure plate, swinging blade)
- Container variety (chest types: wooden/iron/cursed/mirror)
- Pickup categories (health potion, mana shard, key fragment, map fragment, soul echo)
- Rift artifacts (4 Act'a göre evolve eden tek base — primary buffer kullanım)
- UI/HUD pixel art (heart icon, mana icon, echo currency, fragment piece)
- Floor symbols/runes (ward circles, ritual marks, Act-specific glyphs)
- Wall decoration (banner variants — purple Act 1 / blood Act 2 / void Act 3 / mirror Act 4)
- Atmospheric (cobweb, dust mote, water drip, candle flicker, ember)

### B) Hades-Alternative Door Choice Mechanic — RIMA-Unique

Hades pattern: oda sonunda 3 kapı, her biri farklı reward sembol (combat/elite/boon/shop). Player seçer.

RIMA needs:
- Aynı seçim akışı (Karar #149 sub-room sequence: combat → elite → reward)
- AMA Hades clone DEĞİL — RIMA imzası
- Failed Shelter + Convergence lore'a uygun (rift'ten gelen alternatif gerçeklikler)
- Death Imprint mekaniğine bağlanabilir

Brainstorm:
- Visual metaphor: kapı yerine ne? (rift portal? mirror? echo silhouette? broken ward gate? fragmented tablet showing room preview?)
- Information shown: hangi room type olduğu (combat/elite/shop/event/boss) nasıl önceden gösterilir?
- Player interaction: tıklama? walk-through? choice prompt? aura field?
- Lore tutarlılık: neden BURADA seçim var? (rift uses player's deathmark to manifest possibilities? past defenders left "echoes" of alternate timelines?)
- Visual hierarchy: hangisi attractive/dangerous/safe görsel olarak
- Animation: idle / hover / commit (transit)
- Audio cue ideas

NLM query yap: `RIMA door choice mechanic + sub-room sequence + Death Imprint thematic link`

Output formatı:
- Hades pattern'i NEDEN sıradan göründüğünü 3-5 maddede
- RIMA için 3 farklı tasarım önerisi (visual + mechanic + lore)
- Boyut/spec önerileri (PixelLab gen için)
- En güçlü öneriyi gerekçeyle işaretle

## Çıktı

Tek dosya: `STAGING/_research/BUFFER_FILL_DOOR_CHOICE_BRAINSTORM.md`

İki ana bölüm:
1. **Buffer Fill List** — Tablo: Item / Size / Act usage / Production priority
2. **Door Choice Design** — 3 öneri + karşılaştırma + final recommendation

## Effort

high — derin düşün, kestirme YOK, Hades cliché tekrarlama
