# RIMA Projesi - AI Prompting ve Görsel Standartlar Rehberi (PixelLab & Aseprite)

## 📌 Perspektif ve Kamera Standartları (HADES Tarzı)
RIMA, Hades benzeri Dark Fantasy bir Roguelite oyunudur. PixelLab üzerinde üretilecek varlıklar (sandıklar, duvarlar, sütunlar vb.) için her zaman aşağıdaki ayarlar **KESİN** olarak kullanılmalıdır:

1. **Camera View (Kamera Açısı):** 
   - Daima "high top-down" olarak ayarlanmalıdır. 
   - Kesinlikle "low top-down" veya "side" KULLANILMAMALIDIR (Özel bir UI portresi vs. değilse).

2. **Isometric (İzometrik) ve Oblique Seçimleri:**
   - **Isometric:** 	rue (Daima aktif olacak, sahneler elmas/baklava gridine uygun üretilecek).
   - **Oblique Projection:** alse (Eğik izdüşüm kesinlikle kullanılmayacak).

3. **Görsel Kalite ve Işık (Dark Fantasy Standardı):**
   - **Outline:** "single color black outline" (Sprite'ların sahnede belirgin okunabilmesi için ince siyah dış hat).
   - **Shading:** "medium shading" veya duruma göre "cel shading". Çok karışık painterly gölgeler piksel art okunabilirliğini bozabilir.
   - **Gölgeler ve Tonlar:** Kontrastı yüksek, karanlık zindan atmosferine (Mavi, Gri tonları arası) uyumlu soğuk ışık odaklı çıktılar hedeflenmelidir.

*Not (Claude için):* PixelLab Lua API'sini yapılandırırken kullanıcının "İzometrik obje üret" komutu için daima iew="high top-down" argümanını hazırlamayı unutma, aksi halde PixelLab arayüzü hata fırlatır.*
