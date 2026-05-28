ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — A1 Floor Tileset Production via PixelLab MCP (create_tiles_pro)

**Amaç:** RIMA Fractured Chamber A1 floor tileset'ini PixelLab MCP `mcp__pixellab__create_tiles_pro` tool'unu çağırarak production-grade üret. 16 tile, Wang-uyumlu, Square top-down. Çıktıyı STAGING'e kaydet + QC raporu yaz.

User paralel olarak Workflow C (Wang16 4×4 sheet) deniyor — biz Workflow A'yı paralel test ediyoruz, hangisi temiz çıkarsa kullanılacak.

## MCP Tool Call Parametreleri

`mcp__pixellab__create_tiles_pro` çağrısı:

```yaml
description: "clean fractured dark granite floor with sparse low-emissive cyan rift hairline cracks, RIMA Shattered Keep dark fantasy pixel art, charcoal grey stone with broken rectangular flagstones and dust in mortar, restrained palette, walkable ground only, no walls, no pillars, no props, no characters, no UI, top-down ARPG perspective with subtle 3/4 styling"

tile_type: "square"  # top-down square (Wang-compatible)
tile_size: 64        # 64x64 px tile (PPU 64 ile birebir)
view_angle: "low top down"  # NOT "high top down" — 3/4 styling istiyoruz
thickness: 0         # düz floor, no visual thickness
n_tiles: 16          # Wang16 blob set için 16 tile
```

**KRİTİK NOTLAR:**
- `view_angle: "low top down"` (3/4 styling) — Karar #114 LOCK "85-90°" SLOW REVISION pending, gerçek üretim ~70-80° from horizon. User onayı sonrası memory update.
- `thickness: 0` — floor için thickness yok (düz zemin), Edge (A2) için 8-12 px kullanılacak
- `tile_type: "square"` — Wang16 ile uyumlu, RuleTile setup için ideal
- `n_tiles: 16` — Wang16 blob set complete coverage

## Çıktı yönetimi

PixelLab MCP async job_id döndürür. Sen:
1. Job submit et, job_id al
2. `mcp__pixellab__get_job_status` ile poll et (max 5 dk)
3. Status "completed" olunca `mcp__pixellab__get_spritesheet` veya benzeri ile sheet'i indir
4. Sheet'i kaydet: `STAGING/concepts/fractured_chamber/a1_floor_create_tiles_pro_v1.png`
5. Eğer ayrı tile'lar olarak gelirse: `STAGING/concepts/fractured_chamber/a1_floor_tiles/tile_01.png` ... `tile_16.png`

## QC raporu yaz

Çıktı geldikten sonra `STAGING/a1_floor_create_tiles_pro_qc.md` yaz:

```markdown
# A1 Floor Create Tiles Pro QC Report

## Üretim Detayları
- Job ID: [job_id]
- Üretim süresi: [X dk]
- Çıktı format: [single sheet veya individual tiles]
- Çıktı boyutu: [WxH px]
- Tile sayısı: [N]

## Wang16 Uyumluluk Değerlendirmesi
- [ ] 16 tile mı geldi?
- [ ] Tile'lar Wang16 blob pattern'a map edilebilir mi?
  - Isolated (4 void neighbor) var mı?
  - Cap tile'lar (1 neighbor) var mı? (N, S, E, W)
  - Corner tile'lar var mı? (NW, NE, SW, SE — inner/outer)
  - T-intersection tile'lar var mı?
  - Cross/Full tile var mı?
- [ ] Yoksa hangi varyant türleri var? (transition, blob, edge, vs.)

## Görsel QC (Accept Criteria)
- [ ] Tile'lar düz walkable ground gibi okunuyor mu (cliff/wall/stair değil)?
- [ ] Clean tile 5x5 blokta seamless tekrar ediyor mu?
- [ ] Cracked tile low-emissive mi (telegraph VFX'den parlak DEĞİL)?
- [ ] Rift glow tile thin cyan hairline mı (kalın bloom DEĞİL)?
- [ ] Stone palette tutarlı mı (charcoal grey, RIMA mood)?
- [ ] Outer corner sorunu var mı? (Discord'da bahsedilen tool quirk)
- [ ] View angle Low top-down mı (Side veya High DEĞİL)?

## RuleTile Uygunluk Skoru
[1-10 scale, 10 = direkt RuleTile setup edilebilir, 1 = unusable]

## Verdict
- [ ] PASS → Workflow A kullan, RuleTile setup için Unity dispatch ata
- [ ] PARTIAL → Bazı tile'lar yeniden üretilmeli (cell-spesifik regen)
- [ ] FAIL → Workflow C (Wang16 sheet) tek başına kullanılsın

## Next Step
[Codex'in önerisi]
```

## Eğer MCP tool fail ederse

Eğer `mcp__pixellab__create_tiles_pro` bu parametrelerle başarısız olursa:
1. Error mesajını raporla
2. Fallback parametre seti dene:
   - `view_angle: "high top down"` (eğer "low top down" tanınmazsa)
   - `n_tiles: 9` (eğer 16 izin verilmiyorsa)
3. Hâlâ fail ise BLOCKED yaz, user'a fallback olarak sadece Workflow C ile devam ettiğimizi belirt

## Önemli — DO NOT
- Diğer asset'leri (A2, A3, ...) bu turn üretme — sadece A1
- Unity'ye import etme bu turn (QC + verdict yeterli)
- Sheet'i otomatik slice etme — user QC sonrası karar verecek
- Codex'in kendi prompt'unu yazma — yukarıdaki MCP parametreleri sabit
