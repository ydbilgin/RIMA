---
name: PixelLab prompt structure
description: Structured [CHARACTER]/[ACTION]/[CONSTRAINTS] block format produces better PixelLab output than free-form prompts
type: feedback
---

Net adımlar ve blok yapısı PixelLab'dan daha iyi çıktı üretiyor.

**Why:** Kullanıcı gözlemi — yapılandırılmış promptlar daha tutarlı sonuç veriyor.

**How to apply:**
Her PixelLab animasyon promptu şu 3 bloktan oluşur:
1. `[CHARACTER]` — TYPE/HEAD/BODY/LIMBS/EXTRA/CLOTHING/HANDS/SILHOUETTE/COLOR
2. `[ACTION]` — anatomik dil, frame bazlı: "Frame 1 (impact): left foot planted, right shoulder driven back 8px"
3. `[CONSTRAINTS]` — SIZE LOCK / FOOTPRINT LOCK / ANCHOR (değişmez, her promptta aynı)

Kapanış: "2D Fantasy RPG spritesheet layout. Clean pixel clusters, no noise, no anti-aliasing."

Şablon: `STAGING/PIXELLAB_PROMPT_TEMPLATE.md`
