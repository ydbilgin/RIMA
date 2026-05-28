# Codex Task — Wall System Brief Technical Evaluation (2026-05-24)

ACTIVE RULES: (1) think before reviewing (2) min output, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: ChatGPT'nin "Diamond Two-Wall Pipeline" wall system brief'ini RIMA mevcut pipeline'a karşı teknik olarak değerlendir. Production cost + size schema + Unity integration + risk analizi ver. Orchestrator paralel kendi analizini yaptı; bu Codex independent eval.

---

## Konteks

**Mevcut LIVE (S103):**
- 17 final asset (commit fd00ad23): 6 floor (128×64) + 11 wall (128/256/64 × 384)
- Pillar seam-cover LIVE (Karar 2026-05-24)
- HIGH TOP-DOWN 3/4 70-80° (Karar #114 reaffirm + #148)
- PPU=64 LOCKED
- Memory: `C:\Users\ydbil\.claude\projects\F--Antigravity-Projeler-2d-roguelite-RIMA\memory\project_pillar_seam_cover_lock_2026_05_24.md`
- Spec: `STAGING\concepts\fractured_chamber\iso_assets\` (17 PNG)

**Brief önerisi (ChatGPT):**
- Connector-column-driven modular system
- ~60 piece total: 10 connector + 12 wall span + 14 seam + 12 prop + tall variants
- Diamond/irregular room footprint (NOT square)
- 2 visible wall chain (back/top + sides), front open/parapet
- Brief detayı: bu task file'ı yazan orchestrator'ın chat thread'inde mevcut (ChatGPT prompt'unun tamamı user'dan geldi)

**Orchestrator önerisi (paralel eval için):**
- 64-unit grid alignment (PPU=64'e uyumlu)
- 6 batch sheet PixelLab S-XL ile (~1-1.5 saat total)
- Sheet boyutları:
  - A: 256×768 (8 column 64×384)
  - B: 256×768 (8 short span 64×384)
  - C: 768×384 (4 long span 192×384)
  - D: 256×768 (16 seam 32×384)
  - E: 256×768 (4 specialty 128×384)
  - F: 256×256 (16 prop 64px)

---

## Görev — Technical Eval Çıktısı

Aşağıdaki 5 soruyu kesin cevapla. Her cevap < 100 kelime.

### Q1 — Mevcut 17 asset brief'in hangi piece'lerine MAP edilir?

Liste yap: 
- bdkrtgasb commit fd00ad23 dosyaları (17 PNG)
- Brief'in 60 piece listesi
- 1:1 mapping (hangisi hangisine karşılık geliyor, hangisi tamamen eksik)

### Q2 — Size schema doğru mu?

Orchestrator önerisi: 64-unit grid. PPU=64 LOCK ile uyum kontrolü:
- Floor tile 128×64 = 2×1 grid units
- Connector column 64×384 = 1×6 grid units
- Wall span medium 128×384 = 2×6 grid units (mevcut wall mid'imiz)
- Wall span long 192×384 = 3×6 grid units

Bu schema Unity Tilemap + custom WallSnapPlacer ile pixel-perfect mi? Y-sort Custom Axis (0,1,0) ile uyumlu mu? Risk var mı?

### Q3 — PixelLab batch sheet sizes native destekli mi?

Doğrula:
- 256×768 S-XL native? (verified 2026-05-24)
- 768×384 (4:2 aspect) native? Veya 4×4 mod gerekli mi?
- 256×256 native? (4×4 prop grid)

Risk: hangi sheet PixelLab'da problem çıkarır?

### Q4 — Unity prefab hierarchy ve combine doğruluğu

Orchestrator önerisi:
```
WallSystem/
├── ConnectorColumn (8 prefab variant)
├── WallSpan (16 prefab)
├── SeamOverlay (16 prefab)
└── SocketProp (16 decor)
```

Snap-to-grid yaklaşımı 64-unit step. WallSpanPlacer otomatik fill?
- Edge case: irregular polygonal room (diamond) → connector column placement algoritması?
- AI-gen pixel mismatch tolerance → seam piece overlay sufficient mı?
- Y-sort + occlusion: connector column ön/arka render order doğru mu?

### Q5 — Production effort gerçekçi mi?

Orchestrator tahmin: 6 sheet × 5-10 dk PixelLab + 30 dk Codex auto-extract = 1-1.5 saat.

Gerçek mi? Aşağıdaki adımları her sheet için tek tek hesapla:
1. PixelLab S-XL generation (network + gen time)
2. PNG download
3. Codex chroma-key alpha cleanup
4. Codex grid slice (cell extract)
5. Each piece QC + transparency verify
6. Unity import + sprite slice metadata

Eğer 1-1.5 saat yanlışsa, real estimate ver.

---

## Çıktı format (single file)

`STAGING/codex_wall_system_eval_2026-05-24.md` yaz, format:

```markdown
# Wall System Brief — Codex Technical Eval (2026-05-24)

## Q1 — Mapping
[answer]

## Q2 — Size schema
[answer]

## Q3 — PixelLab native compat
[answer]

## Q4 — Unity integration
[answer]

## Q5 — Real production time
[answer]

## Verdict
- **GO / PARTIAL / RECONSIDER**
- Rationale: [1-2 sentence]

## Eğer GO ise — recommended dispatch order
1. [first sheet]
2. [next]
...
```

git commit otomatik yapma — orchestrator review sonrası.
