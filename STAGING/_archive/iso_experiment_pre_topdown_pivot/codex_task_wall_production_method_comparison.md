# Codex Task — Wall Production Method Comparison

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Görev

RIMA Act 1 Shattered Keep duvarları üretimi için **PixelLab Web UI** ve **MCP** tool'larından hangi yöntem(ler) **en verimli + en kontrollü** sonuç verir, derinlemesine karşılaştırma yap.

## Hedef RIMA Wall Set (Phase 1)

| Piece | Hedef W×H | Adet | Önem |
|---|---|---|---|
| wall_tall_straight (perimeter ana) | 96×160 | 1 | P0 |
| wall_tall_corner | 96×160 | 1 | P0 |
| wall_archway | 128×160 | 1 | P0 |
| wall_endcap_column | 48×160 | 1 | P0 |
| wall_low_straight | 96×96 | 1 | P1 |
| wall_low_corner | 96×96 | 1 | P1 |
| wall_low_endcap | 48×96 | 1 | P1 |
| wall_low_T_junction | 96×96 | 1 | P1 |

Toplam: 8 wall piece, doğru dimension + unified style + iso 30° + dark granite + cyan rift accent.

## Bilinen Kısıtlar

1. `create_object` MCP'de "Unknown tool" hatası → kullanılamaz
2. `create_map_object` MCP'de var ama W×H verilmesine rağmen **SQUARE'a clamp ediyor** (test: 96×160 → 96×96 çıktı)
3. Web UI `Create Image Pro V3` non-square destekliyor (output size dropdown'da 32×32 → 688×384 16:9 kadar)
4. Web UI `Edit Image (Pro)` mevcut image transform edebiliyor (reference olarak)
5. Web UI `Same Style (Pro)` referans alarak yeni asset türetiyor

## Karşılaştırılacak Yöntemler

### Method A — 3D Box Outline + Edit Image (Pro)
- Codex/Python PIL ile box outline sheet üret (256×256 veya 512×512)
- 4-16 iso box outline grid layout
- Web UI Edit Image Pro'ya yükle
- Prompt: "Transform each 3D box outline into a RIMA Shattered Keep dark granite wall..."
- AI doldur → exact dimension walls
- Pros: Dimension kontrol, iso angle baked, single call multi-item
- Cons: Box outline kalitesi etkili, 16-item kalitesi 4-item'dan düşük olabilir

### Method B — Plain Canvas + Multi-Cell Prompt (Create Image Pro V3)
- Boş canvas (256×256 veya 512×512)
- Prompt'ta cell-by-cell description ("TL: tall wall, TR: corner...")
- AI grid'i prompt'tan anlayıp doldursun
- Pros: Tek call, sadece prompt
- Cons: AI grid'i respect etmeyebilir (önceki create_object denemesinde 1 obje çıktı), risk

### Method C — Same Style (Pro) İterasyon
- Tek wall üret (96×160 hedefli, dimension fix yöntemi ile)
- Üretileni reference olarak ver
- Same Style Pro ile diğer 7 wall'ı türet (corner, archway, endcap...)
- Pros: Style consistency garanti
- Cons: 8 ayrı call, ~240 gen toplam

### Method D — Kenney Pack Re-Theme (Edit Image Pro)
- Kenney'in 4 stone wall PNG'sini bundle et (256×256 sheet)
- Edit Image Pro: "Re-theme to RIMA Shattered Keep dark granite + cyan rift"
- Pros: Kenney anatomi + RIMA stil + tek call
- Cons: Kenney'in dimensions sandstone proportions (256×512), RIMA scale'e uymayabilir; CC0 lisans OK ama derivative work tartışılır

### Method E — Iterative Reference Build
- İlk wall'ı tek üret (mevcut 96×96 column candidate aday)
- O wall reference → Same Style ile corner üret
- Corner+wall reference → archway türet
- Her step bir öncekini ref alır, style sürekli aktarılır
- Pros: Sürekli iteration, kalite artıyor
- Cons: 8 ayrı call, 240+ gen

### Method F — create_map_object × 8 Direct
- 8 ayrı dispatch, her wall için 1 call
- Square clamp sorunu nedeniyle 96×96, 128×128, 48×48 dimensions
- Pros: MCP doğrudan, otomatik
- Cons: Dimension clamp, style drift, 240 gen, no batch

### Method G — Big Asset Pack Canvas + Iterative Reference (USER PROPOSAL)
- 512×512 veya 688×384 canvas (Web UI output size'larda mevcut)
- 4×4 grid (16 wall slot) veya mixed grid
- İlk iterasyon: 4-8 walls
- Sonraki iterasyonlar: önceki output'u ref olarak ver → ek walls + variants
- Pros: Big pack, style consistency, multi-Act expansion potential
- Cons: Test edilmemiş, AI big canvas multi-cell handling belirsiz

## Görev Detayı

1. **Her method için derinlemesine değerlendir:**
   - Cost (gen tahmini, +/- 20%)
   - Style consistency (1-10)
   - Dimension control (1-10)
   - Multi-Act expansion fit (Act 2-4 re-theme kolaylığı)
   - Failure mode (ne yanlış gidebilir, recovery cost)
   - RIMA-specific fit (8 piece hedefe ne kadar uyar)

2. **Method kombinasyonları öner:**
   - Hangi method'ları SIRALI kullanmak mantıklı? (ör. A → G → D → Multi-Act Edit Pro)
   - Hibrit pattern var mı?

3. **En güçlü 2 yöntem seç, gerekçe ile:**
   - PRIMARY: ana production yöntemi
   - SECONDARY: backup/fallback

4. **Concrete production sequence öner:**
   - Sprint 1: ...
   - Sprint 2: ...
   - Sprint N: ...
   - Her sprint'in cost + output beklentisi

5. **Risk catalog:**
   - Hangi yöntemde hangi failure mode olası?
   - Mitigation strategies

## Çıktı

Tek dosya: `STAGING/_research/WALL_PRODUCTION_METHOD_COMPARISON.md`

Bölümler:
1. Executive Summary (3-5 madde, son tavsiye)
2. Method-by-method analysis tablo
3. Combination recommendations
4. Final production sequence
5. Risk catalog

## Effort

high — bu Phase 1 production startup kararı, drift maliyetli, derin düşün.
