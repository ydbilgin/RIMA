# CODEX TASK — Rift Threshold Original Design Brainstorm + Imagegen

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

---

## Hedef

RIMA dungeon room-to-room threshold için **orijinal görsel kurgu** üret + concept image render et. "Hades stone arch + cyan portal" clone'u değil — daha özgün, lore-driven, 8-yön uygulanabilir.

## Bağlam

- RIMA = 35° isometric ARPG, 2D pixel art (Hades + Diablo karışımı, ama signature kendisi)
- Lore signature: **"Echo Imprint Cascade"** — die, room remembers, each death writes the arena
- Visual signature: **cyan rift** (floor cracks, wall accents, energy)
- Karakterler 8-yön hareket (sprite + flipX = 8 view coverage)
- Renderer: URP 2D + Pixel Perfect Camera, PPU=64, sprite 128×128 standard
- Production budget kıt: PixelLab 1 gen = 20-40 credit
- Mevcut Codex output (`STAGING/concepts/rift_threshold_*_act1.png`) = stone arch + cyan portal = **Hades clone problemi**

## Room Type Taksonomisi

9 oda tipi, her birinde farklı threshold visual:
1. Combat — savaş arenası ("open passage")
2. Elite — challenge ("heavy gate / mark")
3. Boss — final ("ritualistic / sealed / symbol")
4. Chest — loot ("reward herald / shiny")
5. Merchant — NPC ("friendly / marked / busy")
6. Forge — upgrade ("industrial / metal / ember")
7. Event — narrative ("mysterious / inviting")
8. Curse — risk ("warning / bloody / dim")
9. Corridor — basit ("minimum / mundane")

## Görev — 2 Aşama

### Aşama 1: Concept Brainstorm (Markdown report)

`STAGING/CODEX_DONE_door_brainstorm.md` yaz:

3-5 orijinal threshold **KONSEPT**:
- **Form** (geometri/strüktür/silüet) — 100-200 kelime
- **Lore framing** — neden böyle görünüyor, RIMA story-world içinde ne ifade ediyor
- **Room type adaptation** — palette swap mı, symbol swap mı, geometry swap mı (production minimal kalsın)
- **8-dir uygulanabilirlik** — sprite kaç yönlü gen, flipX yeterli mi, billboard/shader trick gerek mi
- **Production maliyet** — base form gen × variant strategy = toplam gen tahmini

Her konsept için **Hades'ten farkı net belirtilmeli**:
- "Hades'te X'tir, biz Y yapıyoruz çünkü Z"
- Diablo / Hyper Light Drifter / Bastion / Solomon's Boneyard gibi diğer ARPG referansları OK
- Mevcut Codex output'unu eleştir + alternatif sun

**Final tavsiye:** 3-5'in hangisi RIMA için en güçlü? Neden? Skor matrisi (originality / lore fit / production cost / 8-dir feasibility).

### Aşama 2: Concept Imagegen (gpt-image-1)

En güçlü 2 konsept için **görsel render** üret:
- Output: `STAGING/concepts/door_brainstorm/concept_<N>_<name>.png`
- Format: 128×128 PNG, transparent corners (chroma key remove)
- 4 state göster: locked / active / portal / final (eğer konseptin state'leri varsa)
- Eğer konsept "9 room type variant" istiyorsa, en az 3 variant göster (combat/boss/chest gibi)

### Aşama 3: Final Report

`STAGING/CODEX_DONE_door_brainstorm.md` sonuna ekle:
- Render edilen görseller liste + path
- Her görselin alpha analysis (transparent px count, opaque px count, corner check)
- Karşılaştırma matrisi: mevcut Codex output VS yeni konseptler
- Orchestrator için next-step öneri: hangi konseptle PixelLab dispatch?

## Kısıtlar

- Asıl çıktı = **özgünlük + lore + production efficiency**, görsel cilası değil
- Yeni Codex pixel paint YOK — sadece imagegen (gpt-image-1)
- BLOCKED if: imagegen API erişimi yok, Image output write izni yok

## Başarı Kriteri

- En az 3 ÖZGÜN konsept (Hades clone değil)
- 2 konsept rendered (render alpha clean ✓)
- Skor matrisi karşılaştırması
- Orchestrator için clear next-step

## Output Files

| File | İçerik |
|---|---|
| `STAGING/CODEX_DONE_door_brainstorm.md` | Brainstorm report + skor matrisi + next-step |
| `STAGING/concepts/door_brainstorm/concept_1_*.png` | Konsept 1 render (4 state veya 3 variant) |
| `STAGING/concepts/door_brainstorm/concept_2_*.png` | Konsept 2 render |
| `STAGING/concepts/door_brainstorm/comparison.png` (opsiyonel) | Side-by-side görsel karşılaştırma |

## Dispatch

Bu task `cx_dispatch.py` ile background:
```bash
python "F:/Antigravity Projeler/2d roguelite/RIMA/cx_dispatch.py" \
  --task-file STAGING/codex_task_door_brainstorm.md --effort high
```

Run in background, notify on complete.
