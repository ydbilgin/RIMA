# RIMA Demo Polish Brainstorm (Lens: ax Pro + cx + ax Flash)

## Hedef
Yarınki editör demosu için, **sıfır veya minimum PixelLab üretimi** ile, tamamen motor gücünü (engine juice) ve mevcut asset'leri kullanarak jürinin görsel deneyimini maksimize etmek.

## Fikirler ve Analiz

### 1. Global URP Bloom & Color Grading
- **Açıklama:** URP Global Volume ekleyerek hafif bir Bloom (Threshold'u yüksek tutarak sadece ışıkları ve VFX'leri parlatmak) ve Color Grading (kontrast artırımı, gölgeleri hafif soğuk, ışıkları sıcak tonlara çekmek) uygulamak.
- **Etiket:** `[engine/kod]`
- **Etki:** Yüksek (Tüm ekranın atmosferini anında değiştirir)
- **Efor:** 0.5 saat
- **Jüri-Görünürlüğü:** Sürekli (%100)

### 2. Hit-Stop (Frame Freeze) & Screen Shake
- **Açıklama:** Ağır darbelerde (melee-arc, ground-crack) zamanı çok kısa süreliğine (0.05s - 0.1s) durdurmak (Time.timeScale = 0) ve kameraya hafif bir shake eklemek. Vuruş hissini yeni sprite eklemeden 10x artırır.
- **Etiket:** `[engine/kod]`
- **Etki:** Yüksek
- **Efor:** 1 saat
- **Jüri-Görünürlüğü:** Savaş ve Aksiyon anlarında

### 3. Dinamik 2D Işıklandırma Entegrasyonu
- **Açıklama:** `SkillVfx` partiküllerine (özellikle cast-flash, trail, chain-bolt) Unity 2D Point Light eklemek. Global ışığı biraz kısıp ortamı karanlıklaştırarak yetenek kullanıldığında etrafın aydınlanmasını sağlamak.
- **Etiket:** `[engine/kod | asset-reuse]`
- **Etki:** Yüksek
- **Efor:** 1 saat
- **Jüri-Görünürlüğü:** Yetenek kullanımlarında

### 4. Trail Renderers & Additive Materials
- **Açıklama:** Mevcut kılıç savurma (melee-arc) veya fırlatılan yeteneklere Unity'nin yerleşik Trail Renderer'ını eklemek. Mevcut VFX sprite'larının materyallerini 'Additive' yaparak parlamalarını sağlamak.
- **Etiket:** `[engine/kod | asset-reuse]`
- **Etki:** Orta/Yüksek
- **Efor:** 0.5 saat
- **Jüri-Görünürlüğü:** Aksiyon anlarında

### 5. Hasar Sayıları (Damage Numbers) & Floating Text
- **Açıklama:** Vuruşlarda havaya doğru sıçrayıp kaybolan (ease-out bounce) küçük hasar sayıları çıkarmak. Saf UI/TextMeshPro ve kod ile halledilir, çok tatmin edicidir.
- **Etiket:** `[engine/kod]`
- **Etki:** Orta
- **Efor:** 1 saat
- **Jüri-Görünürlüğü:** Savaş anlarında

### 6. HUD Micro-Animations (Lerp & Scale)
- **Açıklama:** Can barlarının anında azalması yerine lerp ile (arkada kırmızı bir iz bırakarak) azalması. `ShowToast` bildirimlerinin ekrana ease-out ile kayarak girmesi.
- **Etiket:** `[engine/kod]`
- **Etki:** Orta
- **Efor:** 0.5 saat
- **Jüri-Görünürlüğü:** Sürekli / UI değişimlerinde

### 7. "Centerpiece" Odak Objeleri (Editor-Demo İçin)
- **Açıklama:** Demo odasının merkezine odak çekecek, 2D ışıkla aydınlatılmış bir obje yerleştirmek. Mevcut prop'lar veya decal'lar renklendirilerek kullanılabilir.
- **Etiket:** `[asset-reuse | PixelLab-gen:0]`
- **Etki:** Düşük/Orta
- **Efor:** 0.5 saat
- **Jüri-Görünürlüğü:** Başlangıçta ve odada dolaşırken

---

## İlk 5 Öneri (Öncelik Sırasıyla)
1. **URP Bloom & Color Grading:** En ucuz eforla en büyük görsel devrim.
2. **Hit-Stop & Screen Shake:** Aksiyon "juice" hissini sprite olmadan zirveye taşır.
3. **Dinamik 2D Işıklar:** VFX'lerin çevreyle etkileşime girmesi oyunu çok daha "pahalı" gösterir.
4. **Hasar Sayıları (Damage Numbers):** Oyuncu geri bildirimi için kritik, yapımı çok basit.
5. **HUD Lerp & Toast Animasyonları:** Arayüzün ucuz/statik görünmesini engeller, kalite algısını artırır.
