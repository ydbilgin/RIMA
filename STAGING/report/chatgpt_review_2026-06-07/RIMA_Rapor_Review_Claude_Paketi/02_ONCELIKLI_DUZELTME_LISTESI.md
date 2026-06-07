# Claude İçin Öncelikli Düzeltme Listesi

Bu dosya sadece yapılacaklar sırasını verir. Detaylı gerekçeler `01_ACIMASIZ_REVIEW.md` dosyasındadır.

## P0 — Final güven kıran şeyler

1. Şekil 1–5’i ScreenshotMode ile yeniden al.
2. Debug marker, yeşil kare, yanlış UI state ve placeholder görselleri temizle.
3. “Kapı / arka duvar” terminolojisini “Rift portalı / arka kenar” olarak düzelt.
4. 25/26 template migrasyon farkını açıkla.
5. Test envanteri ile kayıtlı koşu sonucunu ayrı yaz.
6. Şekil 6, 8, 13, 14 placeholder kalmasın.
7. Türkçe karakter / encoding temizliği yap.

## P1 — Savunulabilirlik

8. Portal yönü = 1, exit slot = 3, entry anchor = 1 kararını açık yaz.
9. ScreenshotMode’un debug temizleme aracı olduğunu belirt.
10. UI↔JSON bölümünde ScriptableObject canonical / JSON exchange format ayrımını öne çıkar.
11. Round-trip / debounce testlerine gerçek test isimlerini ekle.
12. Placeholder skill’lerin draft’a sızmadığını yaz, doğru değilse önce kodu düzelt.
13. Ses durumunu demo SFX ve final prodüksiyon ses/müzik olarak ayır.
14. Reviewer-FAIL vakasını tabloya dök.

## P2 — Sunum gücü

15. Gate-slot için küçük şema ekle.
16. QC before/after görselini güçlendir.
17. Test + QC + bağımsız review üçlü kalite güvence tablosu ekle.
18. Warblade görselini temiz büyütülmüş render olarak koy.
19. Boss iddiasını demo seviyesine göre törpüle.
20. AI/PixelLab üretiminde kayıt/lisans/reproducibility cümlesi ekle.

## P3 — Kozmetik / güçlendirme

21. “AI ajanları araçtır; süreci geliştirici tasarladı” vurgusunu koru.
22. Caption’ları gösterilen şeyle eşleştir.
23. Çok iddialı pazarlama cümlelerini mühendislik diliyle değiştir.
