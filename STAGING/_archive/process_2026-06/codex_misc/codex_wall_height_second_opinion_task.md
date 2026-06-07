ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — Second Opinion: ISO Wall Yükseklik Karar Destek

**Amaç:** RIMA için optimum iso wall sprite yüksekliği? Claude (orchestrator) Gemini'nin önerisi ile mevcut RIMA spec arasında karar verecek. Sen bağımsız teknik analiz + verdict döneceksin. Kod YAZMA, asset üretme — sadece karar destek dokümanı.

## Bağlam

### Mevcut RIMA spec (Claude'un kararı)
- PPU 64 (LOCK, karakter 64×64 chibi ile uyumlu, Karar #114)
- Floor: 128×64 (iso 2:1 paralelkenar)
- **NW/NE wall: 128×96 (1.5 tile high)**
- N-corner: 128×128
- Pillar: 64×96

### Gemini 3.5 Flash önerisi
- PPU 100 (default Unity 2D)
- Floor: 256×128 (iso 2:1 paralelkenar)
- **Wall (straight + corner + doorway): 256×256 (kare, 4 tile high)**
- Pivot Bottom Center
- Grid Cell Size: 2.56×1.28 unit
- Y-Sort dinamik

### chatgpt_ref görselleri (görsel hedef)
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_46 (1).png` — combat room iso
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (4).png` — boss room iso
- `STAGING/concepts/chatgpt_ref/ChatGPT Image 22 May 2026 16_12_48 (5).png` — ritual iso
- Bu görsellerde wall: char yüksekliğinin **~3-4 katı** (dramatic dramatic iso wall)

### Char ölçeği
- 64×64 chibi, görsel olarak ~64-80px effective height (Warblade sprite)
- chatgpt_ref'te char ~80-100px görünür, wall ~250-320px → ratio ~3:1 ile 4:1

## Sorular (madde madde cevapla)

### 1. Wall yükseklik optimum: 96 / 128 / 192 / 256?

| Option | Boyut | Pro | Con |
|---|---|---|---|
| A | 128×96 | Kompakt, üretim hızlı | Dramatic iso hissi YOK, char'a göre düşük (1.2:1 ratio) |
| B | 128×128 | Kare, orta yükseklik | Char'a göre yeterli değil (1.6:1) |
| C | 128×192 | Char'a göre dramatic (2.4:1), chatgpt_ref yakın | Daha çok detay üretim |
| D | 128×256 | Gemini'ye yakın (3.2:1), max dramatic | PixelLab S-XL ile zor (256 boyutta), PPU 64'te orantısız |

Hangisi optimum? Net argüman + verdict.

### 2. PPU 64 vs PPU 100 migration?

Gemini PPU 100 öneriyor. RIMA PPU 64 LOCK (char baseline). PPU 100'e migrate için:
- Tüm char sprite (10 class × 8-dir) revize/scale
- Mevcut Wang16 floor revize
- Tile sistem değişikliği

Migration değer mi yoksa PPU 64'te kalalım Gemini'yi half-scale benimseyelim?

### 3. Wall genişliği = floor genişliği kuralı (Gemini insight)

Gemini: "Duvarın alt izometrik taban çizgisi, zemin karosunun üst iki kenarına (NW ve NE kenarları) sıfırlanır."

Bizim spec wall 128 width = floor 128 width → ✅ uyumlu.

Bu kural RIMA için kritik mi yoksa aralarda gap olabilir mi?

### 4. Char-wall scale ratio

chatgpt_ref'te char-wall ratio ~1:3 ile 1:4. RIMA char 64-80px effective:
- 1:3 ratio → wall 192-240px
- 1:4 ratio → wall 256-320px

RIMA için ideal ratio nedir? Combat readability, dramatic atmosphere, üretim kolaylığı arasında trade-off.

### 5. Final verdict

3 cümle özet:
- Optimum wall size: [128×96 / 128×128 / 128×192 / 128×256 / başka]
- Confidence: [low / medium / high]
- Rationale: [tek cümle ana sebep]

## Çıktı

`STAGING/codex_wall_height_verdict.md`:

```markdown
# Codex Second Opinion — ISO Wall Yükseklik Verdict

## Q1: Optimum Wall Yükseklik
[A/B/C/D + net argüman, 3-5 cümle]

## Q2: PPU 64 vs 100 Migration
[verdict + sebep, 3 cümle]

## Q3: Wall Width = Floor Width Kuralı
[verdict + RIMA için kritik mi, 2-3 cümle]

## Q4: Char-Wall Ratio
[ideal ratio + RIMA için sayısal değer, 3 cümle]

## Q5: Final Verdict (özet)
- Wall size: [final]
- Confidence: [level]
- Rationale: [tek cümle]

## Bonus: Gemini önerisinin değerli kısımları
[hangi insight'lar entegre edilmeli, 3-5 madde]
```

## Önemli notlar
- chatgpt_ref görsellerini AÇ ve incele (görsel oran analizi şart)
- Karar TEKNİK + üretim + estetik trade-off
- Kısa ve özlü cevap (3-5 dk read)
- Code YAZMA, asset üretme — sadece markdown verdict doc
- Reference: NotebookLM design knowledge query'leyebilirsin gerekirse
