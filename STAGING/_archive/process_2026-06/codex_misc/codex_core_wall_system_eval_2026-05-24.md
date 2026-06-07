# Codex Task — Core Wall System Eval (CHATGPT spec v2, 2026-05-24)

ACTIVE RULES: (1) think before review (2) min output (3) decisive verdict (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Amaç

User ChatGPT'den YENİ kapsamlı CORE WALL SYSTEM spec'i aldı. 4 sheet, 40 piece, sade-temiz (variations DEFERRED to Edit Image later). 512×512 PixelLab Create Image Pro native production. Mevcut implementation (WallChainBuilder + 17 + asset_pack_v2 44 piece) ile uyumla.

**Bu eval task — ~30-40 dk. Üretim YOK, sadece analiz + verdict.**

## Yeni Spec Özet (4 sheet, 40 piece)

**Sheet 1 — Core Connector Kit (4×2, 8 piece, 512×512)**
1. Straight Connector | 2. Outer Corner | 3. Inner Corner | 4. End Column | 5. Door Col L | 6. Door Col R | 7. Alcove | 8. Protrusion

Source cell: 128×256, final 64×128 (downscale 2x).

**Sheet 2 — Basic Wall Span Kit (2×4, 8 piece, 512×512)**
1-3. Plain Middle Wall A/B/C | 4. Short Span 1x | 5. Med Span 2x | 6. Long Span 3x | 7. Low Wall 1x | 8. Low Wall 2x

Source cell: 256×128, final 128×64 to 256×64.

**Sheet 3 — Corner/Door/Shape Kit (2×4, 8 piece, 512×512)**
1-2. Outer Corner L/R | 3-4. Inner Corner L/R | 5. Door Arch 2w | 6. Door Arch 3w | 7. Open Gap | 8. Short Stop

Source cell: 256×128, final variable.

**Sheet 4 — Seam/Cleanup/Front Edge (4×4, 16 piece, 512×512)**
16 small overlay pieces (seams, jambs, base trim, front corners, cleanup, shadow patch, filler).

Source cell: 128×128, final 64×64.

## Spec Konflikleri (User'a flag)

Önceki kararlarla çelişen noktalar:

1. **Camera angle:** spec "high top-down 3/4 fake-isometric" diyor. Karar #148 (2026-05-24) HIGH TOP-DOWN 3/4 70-80°, project_high_top_down_3_4_lock memory. Önceki chatgpt_ref pure top-down 85-90° iddiası bu spec'le çelişir.

2. **Floor:** spec "64×32 diamond" diyor. Karar #149 (Iso Tilemap) bugün REVOKE edilmişti (Opus Iter 1 verdict). Bu spec iso tilemap'i yeniden aktive ediyor olabilir.

3. **Character scale:** spec "64 px karakter ölçeğine" diyor. RIMA Warblade actual 120 px. Conflict: ya assets scale up edilecek (×2), ya character scale down (yeni gen).

4. **Asset pack v2 (b40wgxvxm):** 44 piece, 7 sheet, 1024×1024 üretildi BUGÜN. Yeni spec 40 piece, 4 sheet, 512×512 + downscale. Üst üste binme: connectors var, walls var ama yeni spec sade (variations YOK), v2 sheet'lerinde cracked/broken/library variations VAR. Reconciliation gerekli.

## Görev — 10 Cevap

Her cevap < 100 kelime.

### Q1 — Yeni spec mevcut WallChainBuilder + 17 + asset_pack_v2 ile nasıl uyumlanır?

Şu an mevcut:
- 17 base asset (128×384 walls, commit fd00ad23)
- 44 asset_pack_v2 slices (varied sheet cells)
- WallChainBuilder.cs (GridToWorld x*2, y*1, cellSize 2,1)

Yeni spec dim (final): 64-128 wide connector, 64-256 wide spans, 32-96 tall.

Compatible mi? Uyum stratejisi?

### Q2 — 4 sheet için PixelLab Create Image Pro promptları yaz

Her sheet için tam prompt:
- Style anchor (chatgpt_ref + master_room_pilot)
- 4 ana spec rule (sade-temiz, no decor baked, fake-iso 3/4, edge-to-edge)
- Grid layout (4×2, 2×4, 4×4)
- Per-cell content list
- Magenta chroma-key BG

4 prompt block (Sheet 1-4 ayrı).

### Q3 — Her piece için final pixel size (40 piece tablo)

User spec özet:
- Connector: 64×128 (source 128×256)
- Plain wall: 128×64-96 (source 256×128)
- Low wall: 128×32-48 (source 256×64)
- Wall span 1x: 64-128 wide
- Wall span 2x: 128-192
- Wall span 3x: 192-256
- Seam: 32×64 to 64×64

40 piece için satır satır final px tablo.

### Q4 — Unity prefab hierarchy

Spec der: `WallPiece_Root + VisualSpriteRenderer + Collider2D + FootprintAnchor + LeftSocket + RightSocket + SeamSocket_Left + SeamSocket_Right + OptionalPropSocket`.

Mevcut WallChunk hierarchy: 5 socket (Left/Right/Torch/Banner/Seam).

Gap: SeamSocket_Left + SeamSocket_Right (2 yerine 1 var), OptionalPropSocket eksik. Patch önerin?

### Q5 — Room assembly algorithm

Spec der:
- RearWallChain: OuterCorner + WallSpan + ... + DoorArch + ... + OuterCorner
- SideWallChain: CornerConnector + WallSpan + InnerCorner/OuterCorner + WallSpan + EndColumn
- FrontEdge: LowWall + OpenGap + LowWall + FrontCorner
- Girinti: WallSpan + InnerCorner + ShortWall + InnerCorner + WallSpan
- Çıkıntı: WallSpan + OuterCorner + ShortWall + OuterCorner + WallSpan
- Kapı: DoorColumnLeft + DoorArch + DoorColumnRight

Mevcut WallChainBuilder algorithm bu pattern'leri destekliyor mu? Eksik logic?

### Q6 — Small/Medium/Large room piece counts

3 size için liste:
- Small (4×3 cells): kaç connector + span + seam + low wall
- Medium (6×4 cells): kaç piece
- Large (8×5 cells): kaç piece

Her size için MIN gerekli total.

### Q7 — Variations later via Edit Image — workflow

Spec: cracked, broken, mossy, library, prison VARIATIONS LATER via Edit Image.

PixelLab Edit Image workflow:
- Base sprite + variation prompt
- AI Freedom (medium) for variation
- Output: aynı piece'in variant'ı

Pipeline step by step ver.

### Q8 — MVP minimum piece list

Spec der "İlk MVP için sadece gerekli minimum parçaları işaretle."

Listele: MVP playable closed room için MIN piece list. ~8-12 piece bekleniyor.

### Q9 — "Sade ilk" mantığının teknik justifikasyon

User: "Varyasyon sonra gelir. Önce bağlanan duvar sistemi çalışmalı."

Engineering principle? Modular system stable foundation first, decoration second. 2-3 cümle.

### Q10 — Asset pack v2 (44 piece, bugün üretildi) durumu

44 slice mevcut. Yeni spec ile uyum:
- Hangi v2 piece'ler yeni spec'e maps?
- Hangi v2 pieces SUPERSEDED (artık kullanılmayacak)?
- Hangi v2 pieces KEEP (decoration variations için)?
- Discard list?

Net karar.

## Çıktı

`STAGING/codex_core_wall_system_eval_DONE.md` yaz:

```markdown
# Core Wall System Eval (CHATGPT spec v2, Codex 2026-05-24)

## Q1-Q10 cevaplar [her < 100 kelime]

## Verdict
- **GO / PARTIAL / RECONSIDER**
- Reconciliation gerekli mi? Hangi mevcut lock REVOKE?
- Sıralı dispatch plan: PixelLab Sheet 1 önce, sonra...

## Risk
[2-3 item]
```

git commit otomatik yapma.
