# Araştırma — RIMA silah sprite üretimi: hangi araç, nasıl batch? (LEAN/PRAGMATİK)

Sen RIMA projesinin araştırma danışmanısın. WEB + PixelLab dokümantasyonu araştır (https://api.pixellab.ai/v1 docs, pixellab.ai sitesi, MCP tool şemaları). Proje bağlamı aşağıda — dosya okumana gerek yok.

## Bağlam (veri)
- Hedef: 2D izometrik-görünümlü top-down roguelite için SİLAH sprite'ları. Karakter efektif 64px (canvas 120x120 ama çizim ~64px), PPU 64, Point filter, chibi pixel-art.
- İlk parti: Ranger yayı (64px), Shadowblade hançer (32-40px, tek sprite + runtime flipX off-hand), Elementalist rün-diski (40px). Sonra 10 sınıfın kalan silahları (toplam ~10-15 parça + varyantlar).
- Silah başına TEK açı sprite (8 yön KODLA çözülüyor: rotation+flipY). Transparan arka plan ŞART.
- Elimizde canlı referans: 64x16 greatsword sprite (cyan, on-brand).
- Bilinen kısıt (eski araştırmadan, DOĞRULA): PixelLab `create_1_direction_object`'ta `size` ve `style_images` BİRLİKTE verilemez; en büyük style-ref çıktı boyutunu belirler; item tier'ları: ≤42px→64 item, ≤85px→16 item, ≤170px→4, üstü→1.

## Sorular
1. **PixelLab araç seçimi:** Tek-açı silah sprite'ı için en doğru araç hangisi: `create_1_direction_object` (item_descriptions[] ile batch) mi, `create_8_direction_object` mi, `create_map_object` mi? "Asset pack" tarzı toplu üretim diye ayrı bir özellik/endpoint var mı (web UI'da veya API'de)? Güncel limitler/tier'lar yukarıdaki bilgiyle aynı mı?
2. **Batch stratejisi:** item_descriptions[] ile kaç silah tek istekte üretmek pratik? Aynı batch'te FARKLI boyut isteyemediğimize göre (boyut=en büyük style-ref), 32-40px küçük silahlar ve 64px orta/büyük silahlar AYRI batch mi olmalı? style_images[] başına kaç ref (≤8 doğru mu, ≤256px doğru mu)?
3. **Style tutarlılığı:** Mevcut 64x16 greatsword'ü + karakter sprite'ını style_images olarak vermek (hibrit ref) hâlâ en iyi pratik mi, yoksa PixelLab'ın yeni style/palette parametreleri mi var (örn. style reference, AI freedom, palette lock)?
4. **İmagegen alternatifi (Gemini/GPT image üretimi):** 32-64px pixel-perfect silah sprite'larını genel imagegen modellerine üretip (büyük canvas → downscale + quantize/palette-snap pipeline) PixelLab kalitesine yaklaşmak gerçekçi mi? Küçük sprite'larda bilinen sorunlar (anti-alias, tutarsız piksel grid) ve bunun pixel-cleanup ile kurtarılma oranı? KISA verdict: silahlar için PixelLab vs imagegen.
5. **Ele yerleştirme için üretim gereksinimleri:** Tek-açı silah sprite'ının "karakterin eline takılabilir" olması için üretimde nelere dikkat edilmeli: çizim açısı (yatay mı, 45° diyagonal mi), grip/sap noktasının konumu (pivot konvansiyonu), padding, boşluk payı? PixelLab çıktısında pivot/anchor metadata'sı geliyor mu yoksa Unity'de elle mi set edilir?

## Çıktı formatı
Her soru için kısa, kanıtlı cevap (kaynak URL'leriyle). Sonda: "ÖNERİLEN ÜRETİM REÇETESİ" — batch kompozisyonu (hangi silahlar hangi batch'te, hangi araçla, hangi parametrelerle) tek tablo. Türkçe.
