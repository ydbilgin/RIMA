# RIMA Silah Üretim Paketi — ChatGPT Danışma Turu (2026-06-07)

Bu zip, RIMA (Unity 6, 2D top-down chibi pixel-art roguelite) projesinin **silah sprite üretimi + karaktere ele yerleştirme** konusunda senden detaylı spec çıkarmanı sağlamak için hazırlandı.

## SIRAYLA OKU
1. **Bu README** — görevin çerçevesi
2. **`01_CANON_WEAPONS.md`** — 10 sınıfın kanonik silah tablosu (tasarım kutsal kitabından; DEĞİŞTİRİLEMEZ kurallar burada)
3. **`02_PRODUCTION_CONSTRAINTS.md`** — üretim altyapısı gerçekleri: PixelLab API limitleri, canlı çalışan örnek silahın ölçüleri, Unity attach sistemi mimarisi, ÇÖZÜLMESİ GEREKEN boyut çelişkisi
4. **`03_CODE/`** — gerçek Unity kodu (4 dosya):
   - `OrientationSync.cs` — silahın 8 yön rotasyon/flip/sort senkronu (yön başına `weaponRotations[8]` + `handOffsets[8]`)
   - `HandAnchorAttach.cs` — silahı handAnchor'a takan runtime sistem
   - `WeaponDatabaseSO.cs` — silah veri kaydı
   - `OrientationSyncAnchorEditor.cs` — el-anchor offset'lerini SceneView'da ayarlayan editör aracı
5. **`04_ASSETS/`** — gerçek sprite'lar:
   - `Warblade_Greatsword.png` (64×16) — CANLI ÇALIŞAN tek silah; stil referansın bu
   - `warblade_idle_south.png` (120×120 canvas, karakter ~64px efektif) — ölçek ilişkisi için

## SENDEN İSTENEN (çıktı formatı 02'nin sonunda)
1. **Boyut çelişkisini çöz** (02'de detay): kanon tablo "büyük canvas üret → Unity'de küçült" diyor, canlı pratik "hedef boyutta üret" — hangisi, neden?
2. **Çizim açısı + pivot konvansiyonunu kilitle** (yatay-sağ vs dikey vs 45°; grip pivot kuralı)
3. **Sınıf başına silah sprite spec'i**: boyut (px), form, grip noktası, çizim açısı, PixelLab `description` metni (İngilizce, item_descriptions[] için hazır)
4. **Batch kompozisyonu**: hangi silahlar aynı batch'te (8-item/batch limiti, boyut karıştırılamaz)
5. **Ele yerleştirme planı**: 8 yön için handAnchor offset/rotation stratejisi + ÖZEL DURUMLAR: Elementalist diski elde DEĞİL avuç üstünde süzülür · Shadowblade/Ravager/Gunslinger çift silah = tek sprite + off-hand flipX · Ranger yayı SOL elde · Ronin kınI (gövdeye baked mi ayrı sprite mı?) · Brawler silahsız
6. Karakter canvas'ı 120×120 ama çizim ~64px — attach matematiğinde buna dikkat et (kod zaten anchor-offset'le çözüyor; senin önerin offset değerleri değil, ÜRETİM spec'i olsun)

⚠️ KIRMIZI ÇİZGİLER: "1 sınıf = 1 silah = 1 silüet" kuralı sabit · Elementalist'e asa/değnek YASAK · Gunslinger'a western estetiği YASAK · Shadowblade hançerlerine gömülü glow YASAK · 8 yön sprite üretimi YOK (tek açı + kod rotasyonu) · PPU 64 + Point filter sabit.
