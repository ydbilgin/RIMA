ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

# Codex Task — RIMA PixelLab Prompt Grammar Guide

**Amaç:** PixelLab'ın kendi resmi prompt yazma kılavuzu var mı bul + RIMA'nın bugüne kadar öğrendiği prompt patterns ile sentezle + tek başvuru dokümanı üret. Sonraki tüm PixelLab üretim taleplerinde bu doc kullanılacak (Claude orchestrator, Codex dispatch, kullanıcı manuel).

## Görev 1 — Resmi PixelLab Prompt Guide var mı?

Şunları tara:
1. **Local docs:** `PixelLabDocs/*.md` — tüm 40+ doc oku (özellikle general.md, character.md, camera.md, color.md, consistent-style.md, faq.md, create-sl-image-pro.md, create-image-flux.md, edit-image-pro.md, inpaint-v3.md)
2. **Web research:** PixelLab resmi sitesinde "prompt guide", "prompt tips", "prompt best practices" başlıkları ara (`pixellab.ai/docs` URL'leri)
3. **MCP tool dokümantasyonu:** PixelLab MCP'nin tool description'larında prompt guidance var mı kontrol et (`mcp_docs.md` veya benzer)
4. **Discord pinned messages:** Erişim varsa PixelLab Discord (https://discord.gg/pBeyTBF8T7) pinned/featured messages

Bulduğun resmi guide'ları özetle: hangi sayfada ne var, RIMA için relevant olan ne?

## Görev 2 — RIMA Öğrenilen Patterns Sentezi

Aşağıdakileri include et:

### Bugün (2026-05-23, S102 close) öğrenilenler:
- ✅ "-" bullet format > numbered list (text bias düşürür)
- ❌ "Cell 1: NAME, Cell 2: NAME..." → AI label çizer (baked text)
- ✅ "no text, no labels, no captions, no numbers, no watermarks" negative MUTLAKA olmalı
- ❌ Wang16 16-cell tek prompt'la zorlamak → Wang LOGIC tutmuyor
- ❌ Create Tiles Pro structured Wang için → random variant generator, structured değil
- ⚠️ Create S-XL Pro max **512×512 square** (768/1024 destek YOK)
- ⚠️ Non-square ratio: 16:9'a kadar, max 688×384 tarzı
- ✅ Tek-asset > multi-cell sheet kalite (PixelLab single-asset modunda en güçlü)
- ✅ "transparent background" explicit yazılmazsa alpha gelmez
- ✅ Shared style lock (palette + lighting + perspective) tüm batch için tekrar

### Önceki dönemden (S99-S101 lessons):
- create_object_state pahalı (4-8x n_frames'e göre) → KULLANMA
- n_frames + reference_image_base64 → style chain için ideal
- 8-dir karakter: 5 sprite üret (S/SE/E/NE/N), 3 mirror (flipX)
- Reference-first asset production (ChatGPT_TOPDOWN ref attach)
- Character: 64x64 chibi, PPU 64, tile 32x32 OR 64x64 (RIMA top-down)
- View angle: actual production ~70-80° (Karar #114 LOCK "85-90°" revize pending)

## Çıktı Dosyası

**`MEMORY/reference_pixellab_prompt_grammar.md`**

Format:

```markdown
---
name: pixellab-prompt-grammar
description: RIMA PixelLab prompt yazma kılavuzu — resmi guide özeti + öğrenilen patterns
metadata:
  type: reference
---

# RIMA PixelLab Prompt Grammar

## 1. Resmi PixelLab Guide Özeti
[Pixellab.ai docs'tan bulduğun resmi prompt guidance — sayfa referansıyla]

## 2. Tool Capabilities (RIMA-relevant)
[Hangi tool ne işe yarar, RIMA için optimum kullanım — tablo formatı]

## 3. RIMA-Spesifik Prompt Templates

### 3.1 Floor Tile (single asset)
[Copy-paste template]

### 3.2 Floor Tile (Wang16 attempt)
[Düzeltilmiş bugünün prompt'u + neden işliyor]

### 3.3 Wall/Backwall (modular sheet)
[A6 backwall layout template]

### 3.4 Character (8-dir reuse, S99 pattern)
[5+3 mirror approach]

### 3.5 Prop (single asset, transparent BG)
[Template]

### 3.6 Decal (single asset, transparent overlay)
[Template]

### 3.7 Sheet Composition (4-cell, 2x2)
[Multi-cell template]

## 4. Hard Rules (DO and DO NOT)

### DO
- [list]

### DO NOT
- [list with reasons — örn: "Cell 1: NAME format kullanma → text baking"]

## 5. Common Failure Modes + Mitigation
[Bugün gibi yaşanan FAIL'ler ve nasıl önlenir]

## 6. Negative Prompt Library (RIMA defaults)
[Her asset tipi için copy-paste negative prompt]

## 7. Tool Selection Decision Tree
[Asset tipine göre hangi tool → karar ağacı]

## 8. Style Lock Block (her prompt'un sonuna eklenir)
[RIMA Shattered Keep stil sabitleri — 1 paragraf copy-paste]

## 9. Update Log
- 2026-05-23: Initial draft (S102 close)
```

## Önemli Notlar
- Türkçe başlık + İngilizce prompt templates (PixelLab İngilizce çalışır)
- Her prompt template **copy-paste hazır** olmalı (kullanıcı düzenleme yapmadan kullanabilmeli)
- Resmi PixelLab guide bulamazsan, "Resmi guide bulunamadı, yalnız RIMA öğrenilenler kullanıldı" not düş
- Çıktı uzunluğu hedef: 300-500 satır (kapsamlı ama özlü)
- MEMORY/INDEX.md'ye otomatik entry ekle (Active liste altı)

## Output Konumu
- `MEMORY/reference_pixellab_prompt_grammar.md` (ana doc)
- `MEMORY/INDEX.md` update (1 satır entry)
- Codex done summary'de path'i bildir
