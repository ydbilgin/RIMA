ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA Act1 arena FAR backdrop görsellerinin HD→680×384 downscale + /pixelify (pixel-art) + PPU 32 import boru hattındaki teknik/feasibility risklerini değerlendir. ANALYSIS ONLY, kod değişikliği YOK.

# Bağlam
10 ChatGPT görseli (~1360x768 civarı HD, 16:9): `C:\Users\ydbil\Downloads\ssss\ChatGPT Image 12 Haz 2026 ... (1).png` ... `(10).png`.
Bunlar oyun arena tile platformunun ARKASINA gelecek **opaque FAR** parallax katmanı. Ortaları KASITLI boş düz koyu mor void (platform oraya oturacak). Üstte ince cyan rift band + kenarlarda uzak kale/ağaç silüetleri.
Pipeline: HD görsel → 680×384'e indir → /pixelify (PixelLab init image, AI Freedom 0) → Unity PPU 32 import → parallax 0.03. MID(fog) + FRONT(ember) ayrı transparent animasyon katmanı sonra gelecek.
Canon: void mor #3A1A4A, slate #3A3D42, cyan #2BD9D9 (ekranın ≤%15'i), LOW CONTRAST.

# Sorular (feasibility / pipeline lens — ESTETİK SIRALAMA DEĞİL)
1. **Gradient banding riski:** Bu görsellerin büyük kısmı düz koyu mor gradient (boş void). HD→680px downscale + pixelify + PPU 32 palette-quantize sonrası bu düz gradientlerde **bantlaşma (banding) / posterization** olur mu? Olursa nasıl önlenir (dither, hafif noise, palette genişletme)?
2. **İnce cyan çizgi downscale'i:** Üstteki ince/parlak cyan rift band 680px'e inerken **aliasing / kırılma / kaybolma** riski var mı? Korumak için ne yapılmalı (cyan'ı ayrı katmanda tutmak, downscale öncesi hafif kalınlaştırma)?
3. **AI Freedom 0 pixelify:** Init-image AI Freedom 0 bu kadar düz/boş kompozisyonda işe yarar mı, yoksa boş alanda halüsinasyon/desen uydurur mu? 680×384 hedef çözünürlük pixelify için uygun mu?
4. **Opaque FAR + parallax 0.03 overscan:** 680×384 @ PPU32 = 21.25×12 birim, viewport 20.15×11.34. Overscan yeterli mi, yoksa parallax kayması kenar gösterir mi? (math doğrula)
5. **Alt void çok düz:** Görsellerin alt yarısı tamamen düz mor. Platform onu kapatmazsa bu alan oyunda "ölü/yavan" görünür mü? FRONT ember katmanı bunu kurtarır mı, yoksa FAR'a hafif doku mu gerekir?

ANALYSIS ONLY, no code changes. Sonucu CODEX_DONE.md'ye yaz. Önceki audit'leri tekrar üretme.
