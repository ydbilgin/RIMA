# Shattered Keep — FAR Backdrop Seçim Kararı (2026-06-12)

**Bağlam:** Act1 arena tile platformunun ARKASINA gelecek opaque FAR parallax katmanı. 10 ChatGPT görseli (`C:\Users\ydbil\Downloads\ssss\`) üretildi. Ortaları kasıtlı boş (platform oturacak → "void'de yüzen kale"). Hedef: 680×384 downscale → /pixelify (AI Freedom 0) → PPU 32 import, parallax 0.03. MID(fog)+FRONT(ember) ayrı transparent animasyon katmanları sonra.

## Council (cx feasibility + Gemini 3.1 Pro + Gemini 3.5 Flash)

**Oybirliği:**
- **#1 = açık ara en iyi** (üç advisor da #1 sıraladı): dengeli kompozisyon, okunur kenar silüetleri, cyan <%10 (canon-uyumlu), temiz orta+alt.
- **Ortası boş = doğru sanat yönetimi kararı.** Platform + VFX merkeze gelecek, arkanın low-frequency olması okunurluk için altın kural.
- **ELE:** #2, #8 (cyan şimşek çok kalın/parlak, canon ≤%15 ihlali, gameplay'i ezer) · #7 (ember/turuncu FAR'da baked → FRONT katmanına ait, çakışır) · #6 (havada çok kaya → pixelify'da çamur).

**Ayrışma — kitbash vs tek seçim:** 3.1 Pro harmanlama öneriyor (#1 kule + #10 cyan); Flash "aşırı-mühendislik, birini seç geç" diyor. **KARAR:** önce #1 tek başına pilot; kitbash sadece #1 zayıf kalırsa loop'ta.

## KARAR — change loop kısa listesi

| Rol | Görsel | Gerekçe |
|---|---|---|
| **Pilot (1.)** | **#1** | Üç advisor #1; dengeli, canon-uyumlu cyan |
| Alternatif | **#10** | En tema-uyumlu (kırık-cam cyan çatlaklar), 3.1 Pro favorisi |
| Alternatif | **#4** | En temiz kadraj, en az pixelify-noise, Flash favorisi |
| Ele | #2, #6, #7, #8 | cyan çok parlak / kaya çamuru / ember baked |

## Pipeline must-do (cx — pilot protokolü, batch'ten ÖNCE)
1. **Tek görsel pilot** (#1), 10'u birden batch'leme.
2. Downscale **Lanczos** ile 680×384.
3. **Cyan rift'i 2-3px kalınlaştır** (downscale'de kaybolmasın — koruma, parlaklık artışı DEĞİL).
4. Pixelify **AI Freedom 0**; boş void'i uydurma desenden koru (gerekirse void'i pixelify sonrası maskele/yeniden doldur).
5. Quantize: **dither + ayrılmış koyu-mor rampa + ayrılmış cyan paleti girişleri** (banding'i ve cyan kaybını önler). Void'e **1-3 luma** çok hafif non-directional dither.
6. **Unity'de PPU 32 + parallax 0.03 + gerçek platform silüeti** ile incele → ANCAK ONDAN SONRA batch.

## Overscan doğrulama (cx)
680×384 @ PPU32 = 21.25×12 birim vs viewport 20.15×11.34 → kenar marjı 0.55/0.33 birim/kenar. Parallax 0.03'te güvenli kamera hareketi ±18.3 birim yatay / ±11 dikey. Fixed Act1 arena için **yeterli**. (Uzun scroll'da re-anchor/büyütme gerekir — bizde gerekmiyor.)

## Açık riskler (loop'ta izle)
- Düz alt void platform tarafından tam kapanmazsa "ölü alan" görünebilir → FRONT ember + hafif FAR dither kurtarır.
- İnce cyan'ın pixelify sonrası kopması → pilot'ta birebir kontrol.
