# PixelLab ile İzometrik Tileset ve Duvar Üretim Rehberi

Bu rehber, PixelLab (pixellab.ai) kullanarak projenizin izometrik 2.5D sanat tarzına uygun zeminler, geçişli karolar (transition tiles) ve duvarlar üretmeniz için tasarlanmıştır.

---

## 1. İzometrik Duvarlar Uygun mu? Nasıl Üretilmeli?

### Duvarların Açı Uyumu
İzometrik bir oyunda duvarlar, zemin elmaslarının kenarlarıyla tam örtüşmelidir. 
- Standart izometrik projeksiyonda (2:1 piksel oranı), zemin çizgileri **26.56°** (veya matematiksel olarak yuvarlanmış **30°**) açıyla uzanır.
- Eğer ürettiğiniz duvarlar tamamen karşıdan düz (orthographic front-facing) veya yanlış bir açıda üretilirse, sahnede eğri dururlar ve 3D derinlik hissi kaybolur.
- **Doğru Yöntem:** Duvarlar her zaman izometrik açıda sağa veya sola meyilli (South-West veya South-East yönlü) olarak üretilmelidir.

### PixelLab Prompt Örnekleri (Duvarlar İçin)
Duvar üretirken şu anahtar kelimeleri kullanabilirsiniz:
> `"isometric 2.5D wall sprite, stone brick dungeon wall, diagonal 45 degree slope, pixel art style, 32-bit game asset, seamless tiling on bottom, high quality, unity compatible"`

### İpuçları (Hizalama ve Tutarlılık)
1. **Init Image (Görsel Referans):** PixelLab üzerinde sıfırdan üretmek yerine, elinizdeki en iyi duran duvarlardan birini (`wall_00` veya `wall_01`) **Init Image** (referans görsel) olarak yükleyin. Bu, yapay zekanın kameranın bakış açısını, ışığı ve piksel yoğunluğunu (PPU) birebir taklit etmesini sağlar.
2. **Piksel Boşlukları (Padding):** Algoritmaların ürettiği resimlerin altında bazen boş şeffaf alanlar kalabilir. Yeni eklediğimiz **Auto-Align Base** özelliği bu şeffaf boşlukları otomatik tespit edip sıfırlasa da, üretimlerin alt tabanını olabildiğince kesilmiş (cropped) tutmaya çalışın.

---

## 2. PixelLab ile Geçişli Karolar (Transition Tileset) Üretilebilir mi?

Evet! PixelLab'in tileset üretim motoru, geçiş karoları (terrain transition tiles) üretmek için son derece uygundur.

### Geçiş Karosu (Transition) Nedir?
İki farklı zemin türünün (örneğin çimen ile taş veya taş ile lav) birleştiği yerlerdeki yumuşak sınır çizgileridir. Standart bir geçiş seti; kenarlar (edges), dış köşeler (outer corners) ve iç köşeler (inner corners) içerir.

### PixelLab'de Geçiş Üretme Yöntemi:
1. **Tileset Modunu Kullanın:** Arayüzdeki tileset şablonunu seçin.
2. **Prompt Hazırlığı:** İki zemini ve geçişi tanımlayın.
   - Örnek: `"seamless transition tileset from dark stone bricks to hot red lava, top-down isometric view, pixel art, 32x32 tiles, clean borders"`
3. **Inpainting / Edit:** Eğer geçişlerin bazı pikselleri yapay duruyorsa, PixelLab'in **Inpainting** (maskeleyip yeniden üretme) fırçasıyla sınır çizgilerini düzeltebilir veya Aseprite entegrasyonuyla pikselleri yumuşatabilirsiniz.

---

## 3. Aynı Türden 4 Farklı Varyasyonu Rastgele Koymak Güzel Durur mu?

**Evet, kesinlikle çok daha profesyonel durur!**

### Neden Varyasyon Kullanmalıyız?
Eğer sahnede tüm duvarları veya zeminleri tek bir görselin sürekli tekrarıyla boyarsanız, **"Tiling Effect"** denilen yapay ızgara deseni oluşur. Bu durum oyunun derinliğini öldürür ve amatör bir hava verir.

### En İyi Dağıtım Stratejisi (Weighted Random):
4 adet aynı tür varyasyonunuz varsa (örn: 1. Normal Taş, 2. Çatlak Taş, 3. Yosunlu Taş, 4. Üzerinde Meşale/Zırh olan Taş):
- **Normal Taş (Ana karo):** Placements'ın %75-80'ini oluşturmalıdır.
- **Çatlak Taş:** %10 oranında serpiştirilmelidir.
- **Yosunlu Taş:** %8 oranında serpiştirilmelidir.
- **Meşaleli/Özel Taş:** %2-5 oranında odak noktalarına yerleştirilmelidir.

Bu ağırlıklı rastgele yerleşim (Weighted Random Placement), sahnenin el yapımı, zengin ve doğal görünmesini sağlar.
