ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — Asset Production Prompts v1 (Fractured Chamber Pipeline)

**Amaç:** RIMA Fractured Chamber pipeline'ının 8 asset üretim görevi için production-grade prompt setleri, sheet layout spec'leri, Unity import settings ve max-boyut testlerini içeren tek bir referans dokümanı üret. Kod YAZMA, asset ÜRETME — sadece yazılı spec/prompt çıktısı.

User PixelLab web UI'da (manuel) bu prompt'lar ile asset üretecek. Codex bunları yazıp, sheet layout'larını netleştirecek, S-XL/Create Tiles Pro max boyut limit testlerini yapacak.

## Çıktı dosyası
`STAGING/asset_production_prompts_v1.md` — Markdown, her asset için ayrı section, kopyala-yapıştır prompt'lar.

## Pipeline KARARLARI (LOCK — buradan sapma)

- **Pipeline yönü:** Full 2D Fractured Chamber + Modüler Backwall Band (Claude+Codex consensus, HIGH confidence)
- **Karakter:** 64×64 chibi, PPU 64
- **Tile baseline:** 64×64 (PPU 64 ile uyumlu)
- **Stil:** Dark fantasy pixel art, RIMA Shattered Keep, fractured granite, cyan rift cracks, amber torch highlights, dark dungeon atmosphere
- **Combat density cap:** ~10 aktör (6-10 enemy + 1-3 ally)
- **Açı:** Near-pure top-down ~85-90° (Diablo / Children of Morta / Hades). 3/4 sprite styling subtle. NO true iso (30/45°)
- **Backwall:** MODÜLER (NOT monolitik) — 16-piece 4×4 sheet, kombinasyonla scale alır
- **Doorway:** 2 unique sprite (vertical N/S + horizontal W/E) — rotate/mirror ile 4 yön coverage
- **Decals:** Aseprite hand-paint (transparent, cyan rift cracks)
- **Backwall workflow:** Tek-step Create Image Pro 4×4 sheet (init image opsiyonel — sen test et değer mi)
- **Light:** Baked DEĞİL — torch/brazier sprite + ayrı 2D Light prefab
- **Hard rule:** Base decor sprite emissive DÜŞÜK, VFX/telegraph emissive YÜKSEK (combat readability)

## Ön-test (Codex direkt yapacak)

### Test 1: PixelLab Create Image S-XL maximum boyut limitleri
PixelLab MCP `mcp__pixellab__create_image_pro` veya web UI dokümantasyonunda:
- S-XL "new" mode default max boyut nedir?
- Aspect ratio değiştirilebilir mi? 768×340 dışında hangi kombinasyonlar mümkün?
- 1024² veya 1024×768 mümkün mü?
- Bul ve raporla.

### Test 2: Create Tiles Pro durumu
- `mcp__pixellab__create_topdown_tileset` veya benzer tool — şu an stable mi yoksa hâlâ experimental mi?
- Outer corner bug aktif mi?
- Floor tileset için kullanılabilir mi (seamless tile output)?
- Cell boyutu / tile count parametreleri ne?

Test sonuçlarını dokümanın başına "## Tool Capability Findings" başlığı altında yaz. Bu bulgular A1 ve A6'nın final workflow'unu netleştirecek.

## Asset listesi (Codex bu 8 için tam spec üretecek)

### A1 — Floor Tileset
- **Hedef:** Fractured dark granite floor, 4 variant (clean, cracked, rift glow, broken corner)
- **Boyut:** 64×64 per tile (PPU 64)
- **Workflow A (test):** Create Tiles Pro — seamless 16-tile set, 4 base variant + transitions
- **Workflow B (fallback):** Create Image Pro 4-cell sheet, 256×256, cell 128×128, downscale to 64×64 in Unity import
- **Hangisi önerilen?** Test 2 sonucuna göre karar ver, ikisi için de prompt yaz

### A2 — Edge 4-cell Sheet
- **Hedef:** Low broken stone edges, fractured granite kenar parçaları
- **Cell layout:**
  - Cell 1 (top-left): N face straight low edge (128×64)
  - Cell 2 (top-right): W/E side straight low edge (64×128)
  - Cell 3 (bottom-left): Corner outer (64×64)
  - Cell 4 (bottom-right): Rubble cluster seam cover (64×64)
- **Boyut:** Sheet 256×256, transparent BG, baseline aligned, edge match GEREKMİYOR
- **Workflow:** Create Image Pro 4-cell sheet
- **Constraint:** Tüm cell aynı stone color + same light direction + same baseline height

### A3 — Cyan Rift Crack Decal (linear)
- **Hedef:** Cyan glowing crack, zeminde overlay (transparent BG)
- **Boyut:** 128×64
- **Workflow:** Aseprite hand-paint (user manuel)
- **Spec:** Cyan #4DD4FF base, lighter cyan glow halo, jagged geometric crack pattern, transparent BG. Codex spec yaz: pixel-by-pixel reference description (user'ın referans alacağı), 2-3 reference image link Discord/Pinterest'ten (opsiyonel)
- Emissive level: ORTA (telegraph VFX'ten düşük)

### A4 — Cyan Rift Crack Decal (branching)
- **Hedef:** Branching/web cyan crack
- **Boyut:** 256×128
- **Workflow:** Aseprite hand-paint
- **Spec:** A3 ile aynı renk/stil, daha karmaşık branching pattern (3-5 dal)

### A5 — Prop 4-cell Sheet
- **Hedef:** 4 prop sprite, transparent BG
- **Cell layout:**
  - Cell 1: Amber brazier (idle, lit flame ayrı asset olarak Aseprite veya Unity particle ile)
  - Cell 2: Broken pillar base (granite, fractured top)
  - Cell 3: Rubble pile (dark stone chunks)
  - Cell 4: Candle cluster (3 candles on base, amber glow)
- **Boyut:** Sheet 256×256, cell 64×64 veya 128×128 (Codex karar — hangi prop büyük gerekiyor)
- **Workflow:** Create Image Pro 4-cell sheet

### A6 — Backwall Modular Set (16-piece, 4×4 sheet) [MOST CRITICAL]
- **Hedef:** Modular north wall band — kombinasyonla normal/büyük/koridor odaların backwall'ı oluşturulur
- **16 piece içeriği (ChatGPT 21_29_29 önerisi, RIMA-adapted):**

| Cell | İçerik | Kullanım |
|---|---|---|
| 1,1 | Left End cap | Backwall'ın sol kenarı, kapanış |
| 1,2 | Mid Broken A | Tekrar edilebilir mid section, hafif damage |
| 1,3 | Mid Broken B | Mid section variant, farklı damage pattern |
| 1,4 | Mid Stone Cover | Temiz/sağlam mid section (seam gizleme) |
| 2,1 | Mid Torch socket | Boş torch yuvası (overlay torch sprite buraya yapışacak) |
| 2,2 | Mid Banner | Hanging banner mid section (rouge/red flag) |
| 2,3 | Damaged Mid | Kısmen yıkılmış mid (void leak alttan) |
| 2,4 | Open Gap | Duvar arasında void leak (cyan rift glow) |
| 3,1 | Boss Gate Center | Boss room landmark — büyük arch + closed gate |
| 3,2 | Rift Gate Center | Ritual/transition — center'da cyan rift portal |
| 3,3 | Prison Bars Center | Prison oda landmark — iron bars + dim glow |
| 3,4 | Library Shelf Center | Library oda — bookshelf flush wall |
| 4,1 | Crypt Statue Center | Crypt — center stone statue niche |
| 4,2 | Inner Corner NW | İç köşe (oda iç-bükey köşe) |
| 4,3 | Inner Corner NE | İç köşe (ayna NW veya farklı) |
| 4,4 | Right End cap | Backwall'ın sağ kenarı, kapanış |

- **Boyut:** Sheet TOTAL 1024×1024 (cell 256×256) veya 768×768 (cell 192×192). Codex test 1 sonucuna göre karar ver.
- **Cell aspect:** Her cell ~3:4 (yatay küçük, dikey büyük) — backwall yüksek dikey öğe
- **Workflow:** Create Image Pro 4×4 sheet (single gen)
- **Constraint (KRİTİK):**
  - Tüm cell aynı baseline (alt kenar), aynı top cap height
  - Aynı granite stone color, aynı light direction (üstten hafif aydınlatma)
  - Cell-to-cell seam edge match (Left End → Mid → Mid → Right End yan yana koyunca seamless görünmeli)
  - PILLAR-LESS — duvar üzerinde pillar yok (overlay olarak ayrı)
  - NO baked torches/banners (Mid Torch cell sadece boş socket; banner cell istisnai, banner kendi cell'inde)
  - Center landmark cells (Boss Gate, Rift Gate, Prison Bars, vs.) Mid cell genişliği ile aynı (yan yana koyunca aynı baseline)
- **Prompt'ta belirt:** "4×4 grid, 16 modular wall band pieces, each cell self-contained, all share same baseline at bottom, same stone palette, same lighting from above. Cells designed to tile horizontally seamlessly when placed adjacent. Left End and Right End cap cells visually close the run."
- **Init image opsiyonu:** Eğer Test 1'de S-XL gpt-image-1 init image işliyorsa, A6 için 2-step workflow değerlendir:
  1. gpt-image-1 ile 1024² illüstratif ref concept üret
  2. Onu Create Image Pro 4×4 sheet'e init olarak ver
  Karar ver: ek değer var mı? (Codex'in görüşü)

### A7 — Doorway Sprites (2 single)
- **A7a (vertical):** N/S kapısı için, 96×128 dikey, transparent BG
  - Stone archway, granite, dark interior (void görünür)
  - NO wood door (sadece stone arch)
  - Optional torch socket yan tarafta
- **A7b (horizontal):** W/E kapısı için, 128×96 yatay, transparent BG
  - Aynı stone archway, perspektif W/E'ye uyumlu
  - Mirror flipX ile diğer yön
- **Workflow:** Create Image Pro 2 single asset
- **Constraint:** A6 backwall sheet ile aynı stone palette + baseline alignment

### A8 — Theme Props (3 single)
- **A8a Sarcophagus:** 128×96, granite tomb, hafif crack, optional cyan rift glow seam
- **A8b Ritual Stone:** 96×96, altar-like stone, top'ta cyan rune glow
- **A8c Prison Cage:** 96×128 dikey, iron bars cage, interior darkness
- **Workflow:** Create Image Pro 3 single asset
- **Constraint:** A5 prop sheet ile aynı stil/scale uyumu

## Unity Import Settings (her asset için)
- **PPU:** 64
- **Sprite Mode:**
  - Single asset → Single
  - Sheet → Multiple, Grid by Cell Size (cell boyutu asset spec'e göre)
- **Filter Mode:** Point (no filter)
- **Compression:** None (pixel art preservation)
- **Pivot:** Bottom (Y-sort için)
- **Mesh Type:** Tight (transparent edge optimization)
- **Read/Write:** Disabled (memory optimization)
- Codex bu settings'i her asset section'da kısa bir tablo olarak ver

## Üretim Sırası ve Dependency Tree
Codex bu dependency tree'yi dokümanın sonuna koysun:

```
Phase 1 (foundation, blocker):
  A1 floor → A2 edge → Battered Hall test scene

Phase 2 (paralel):
  A3 + A4 decals (Aseprite, ~10 dk total)
  A5 prop sheet
  → Battered Hall full compose test

Phase 3 (identity):
  A6 backwall 16-piece sheet → Rift Gate Chamber test
  A7 doorway 2 sprite → door socket pipeline test

Phase 4 (theme):
  A8 theme props → Ritual Chamber + 5+ oda
```

## Çıktı Format Spec (Codex zorunlu uyacak)

Dokümanın başında:
1. **## Tool Capability Findings** — Test 1 + Test 2 raporu, 3-5 cümle
2. **## Final Workflow Decisions** — Test sonuçlarına dayalı A1 ve A6 final workflow lock

Sonra her asset için:
```markdown
## A[N] — [Asset adı]
**Hedef:** [tek cümle]
**Boyut:** [exact pixel]
**Workflow:** [tool + step sayısı]
**PixelLab Prompt (kopyala-yapıştır hazır):**
> [prompt metni — single paragraph, English, RIMA constraints + asset-specific detail]

**Negative Prompt:**
> [varsa]

**Unity Import:**
| Setting | Value |
|---|---|
| ... | ... |

**Notes / Risks:** [varsa]
```

Sonda:
- **## Üretim Sırası ve Dependency Tree**
- **## Open Questions** — Codex'in user'a sormak istediği netlik noktaları (boyut, stil, opsiyon)

## Önemli Notlar
- Türkçe başlık + İngilizce prompt karması OK (PixelLab prompt'ları İngilizce çalışır)
- Her PixelLab prompt'unda şu sabitler olmalı: "dark fantasy pixel art, fractured granite, cyan rift cracks, amber torch highlights, top-down ARPG 85-90 degree angle, NO characters, NO UI, transparent background where applicable"
- Stil reference: ChatGPT_TOPDOWN render örnekleri (STAGING/concepts/chatgpt_ref/new_chatgpt/*.png) — Codex bu görselleri AÇMASI gerekli, prompt'lara stil tonunu yansıtmalı
- Çıktı uzunluğu hedefi: ~500-700 satır (her asset detaylı ama özlü)
- BLOCKED durum: Eğer Test 1 veya Test 2'de tool capability bulamazsan, asset-specific BLOCKED note koy + alternatif öner
