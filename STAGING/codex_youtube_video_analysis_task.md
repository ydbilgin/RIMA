# Codex Task — YouTube Shorts Video Analizi (RIMA VFX referansı)

**Tarih:** 2026-05-15 gece (S81)
**Profil:** yasinderyabilgin
**Effort:** low
**Background:** evet

## Görev

Şu YouTube shorts videoyu analiz et:

https://www.youtube.com/shorts/1X4Oq2X41ZU?feature=share

Kullanıcı bu videoyu RIMA için VFX referansı olarak paylaştı. Şu soruları cevapla:

### 1. Kamera Açısı
- Yaklaşık derece (top-down: 90°, isometric: 30°, RIMA mevcut Karar #100 = 35°)
- FOV genişliği (close cam mi, wide arena mi)
- Karakter ne kadar ekran payı kaplıyor

### 2. Silah Görselleştirme
- Silah karakterin önünde/yanında/arkasında mı?
- Silah sprite detayı yüksek mi düşük mü?
- Silah üzerinde VFX (trail, glow, particle) var mı?
- Karakterle silah arasında "katman" hissi var mı (silah half-hidden)

### 3. VFX Teknikleri
- Hangi VFX teknikleri kullanılmış (trail, screen shake, particle storm, hit pause, chromatic, bloom)
- Renk paleti (neon, dark, palette)
- Ekran ne kadar VFX dolu (yoğunluk 1-10)

### 4. Combat Hissi
- Manuel mi auto mi (player aktif aim/dodge var mı)
- Vuruşların ağırlığı (hit feel — Sifu/Hades/HLD seviyesi)
- Combo varsa kaç vuruşluk

### 5. RIMA için Uygulanabilir Çıkarımlar
RIMA mevcut state:
- 64px chibi karakter, 8 yön anim, Karar #100 35° high top-down
- Karar #123 weapon decouple (silah ayrı sprite, ele takılır)
- Karar #122 Beat3Commit T1 (3-hit combo charge LIVE)
- Hades-tarzı ARPG hedefi

Bu video RIMA için ne öneriyor:
- Açı değişmeli mi? (Karar #100 koruna mı revize mi?)
- Weapon sprite detayı azaltılmalı mı? (VFX-overlay yaklaşımı)
- Hangi VFX teknikleri Tier 1 zorunlu?

## Yasak
- Tahmini analiz yapma. Eğer video erişilemezse "video açılamadı" yaz, spekülasyon ÜRETME.
- YouTube oEmbed metadata yeterli değil. Mümkünse video transcript / thumbnail / preview frame analizi yap.
- Video açılamazsa: kullanıcıya screenshot atması için spesifik sorular hazırla (kamera açısı için ne ölç, silah katmanı için ne kontrol et vs).

## Output

`STAGING/codex_youtube_video_analysis_result.md` dosyasına yaz. Kısa, tablo bol, ~150-200 satır maksimum.
