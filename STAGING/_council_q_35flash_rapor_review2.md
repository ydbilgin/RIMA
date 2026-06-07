# Council Sorusu — ChatGPT Rapor Review-2 Paketi (LEAN / SHIP-FAST LENS)

Sen RIMA council'inin PRAGMATİK danışmanısın. Görev: ChatGPT'nin 30 bulguluk rapor review paketine "en yalın yol + over-engineering eleştirisi" lensiyle bak. Rapor bir bitirme projesi final raporu; zaman SINIRLI (final teslimi yaklaşıyor), kullanıcı aynı zamanda oyunu da geliştiriyor.

## Read these files
- `STAGING/report/chatgpt_review_2026-06-07/RIMA_Rapor_Review_Claude_Paketi/01_ACIMASIZ_REVIEW.md` (30 bulgu)
- `STAGING/report/chatgpt_review_2026-06-07/RIMA_Rapor_Review_Claude_Paketi/02_ONCELIKLI_DUZELTME_LISTESI.md`

## Bağlam (doğrulanmış)
- Encoding bulgusu (#30) DOĞRU: §2.6/§3.5.6 ASCII Türkçe ile yazılmış (son edit turu kaynaklı) — kesin düzeltilecek
- ScreenshotMode aracı VAR (F12 + 6 preset + deterministik seed) — yeniden çekim teknik olarak kolay
- Rapor 8 gömülü şekil; ChatGPT 14 şekil istiyor (6 yeni/yeniden üretim)
- Skill draft guard'ı kodda var; test sayıları cx tarafından ayrıca doğrulanıyor

## Senin soruların (3 soru, lean lens)
1. **Maliyet/etki eleme:** 30 bulgudan hangileri 80/20 kuralına göre YAPILMAMALI ya da minimuma indirilmelİ? Özellikle şüphelendiklerim: #16 QC before/after (eski hatalı state'i yeniden yaratmak gerekir — "before" görseli yoksa maliyet yüksek), #21 gate-slot şeması, #22 Warblade render üretimi, #25 üçüncü tablo. Her biri için: dakika-maliyet tahmini + jüri-etki puanı (1-5) + verdict.
2. **Tek geçişte birleştirilebilecekler:** Hangi bulgular TEK metin-edit pass'inde toplanır (terminoloji + encoding + sayı düzeltmeleri + güvence cümleleri)? Hangi şekil işleri TEK Unity oturumunda toplanır (ScreenshotMode seansı)? Pratik iş paketlemesi öner: kaç paket, her paket ~ne kadar sürer, hangi sıra?
3. **Scope-creep alarmı:** ChatGPT'nin önerilerinde rapora YENİ iddia ekleten (ve dolayısıyla yeni savunma yükü doğuran) maddeler var mı? (Ör. #29 "prompt kayıtları dokümante edilmiştir" cümlesi — bu belgeleme gerçekten var mı bilmiyorsan yazdırma riski.) Bu tip riskli güvence cümlelerini listele.

## Çıktı formatı
(1) SKIP/MINIMIZE listesi (bulgu# + tek satır gerekçe + tahmini tasarruf), (2) iş paketleri tablosu (paket adı · içerdiği bulgular · süre tahmini · sıra), (3) riskli güvence cümleleri listesi. Kısa ve keskin. Türkçe.
