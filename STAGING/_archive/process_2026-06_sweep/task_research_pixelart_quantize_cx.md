# TASK: PixelLab/imagegen çıktısını GERÇEK 8/16-bit pixel-art'a indirgeme — TEKNİK FİZİBİLİTE (cx laurethayday)

ACTIVE RULES: (1) think before coding (2) min output, no speculation (3) surgical — sadece araştır + 1 rapor + opsiyonel küçük PoC script (4) BLOCKED yaz belirsizse.

NLM ACCESS: gerekirse:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<soru>"
Direct-read: STAGING + skills.

Amaç: RIMA'nın üretilen görselleri (PixelLab tiles_pro floor451 + codex $imagegen portal/reward `STAGING/imagegen/portal_reward/`) "gerçek pixel art" hissi versin = sınırlı palet (8-bit ~16-32 renk / 16-bit ~256), temiz renk bantları, anti-alias/HD-bulanıklık yok. SORU: bunu yapan bir TOOL var mı (proje içi veya dış), YOKSA biz YAPABİLİR MİYİZ? Teknik fizibilite + somut implementasyon.

## ARAŞTIR
1. **Proje içi mevcut:** `pixelify` ve `pixel-cleanup` skill'leri ne yapıyor (SKILL.md oku — `~/.claude/skills/` veya proje skills). Palette quantize / palette snap / outlier merge yetenekleri zaten var mı? Eksik ne?
2. **Kütüphaneler (Python tercih, RIMA pipeline'a uyar):** Pillow `Image.quantize(colors, method=...)` (MEDIANCUT/MAXCOVERAGE/libimagequant), `libimagequant`/pngquant, `didder` (ordered/blue-noise dithering CLI), scikit-image, `hitherdither`. Hangisi pixel-art için en temiz sonuç?
3. **Palet kilidi:** sabit küratörlü palet (RIMA on-brand: koyu slate/iron + cyan #00FFCC + mor void) ile quantize → tutarlı görünüm. Palet dosyası (.gpl/.hex/.png) + "nearest color snap". DB16/AAP-64 gibi hazır paletler vs RIMA-özel palet.
4. **Dithering:** AÇIK mı KAPALI mı (pixel-art tile'da genelde KAPALI/az; gradyan azaltma için ordered dither opsiyon).
5. **PPU/snap:** zaten doğru çözünürlükteyse (PixelLab 64px) sadece RENK quantize yeter mi, yoksa downscale→upscale (nearest) ile blok-pixel de gerek mi?

## ÇIKTI: `STAGING/RESEARCH_PIXELART_QUANTIZE_CX.md`
- VERDICT: tool VAR mı / YAPILIR mı (evet/hayır + neden).
- Önerilen yaklaşım (1 net pipeline): hangi kütüphane + palet stratejisi + dither kararı + adımlar.
- **Küçük PoC script iskeleti** (Python, Pillow): bir PNG'yi al → N-renk palete quantize (opsiyonel sabit palet) → kaydet. ÇALIŞTIRMA, sadece iskele + nasıl entegre (skill mi, cx_dispatch util mi).
- RIMA entegrasyon noktası: yeni `/pixelquant` skill mi, mevcut pixel-cleanup'a ek mi.
ASCII, Türkçe, kısa.
