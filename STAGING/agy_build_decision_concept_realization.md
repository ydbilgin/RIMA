RIMA için hedeflediğin "karanlık, gizemli, neon-vurgulu izometrik rogue-lite" görünümü (Art North Star) çok güçlü bir sanat yönü. Bu vizyonu Unity'de performanstan veya oynanıştan ödün vermeden hayata geçirmek için gereken kararları ve **cx'in uygulayacağı net build planını** aşağıda sunuyorum.

### 🏛 YÖNTEM KARARI: (A) Modüler Tile + Işıklandırma/Post-Process

**Karar:** Konsept görünümünü gerçeğe dönüştürmenin EN İYİ yolu **(A) Modüler tile + DirectionalCliff + Dinamik Işıklandırma/Bloom** kombinasyonudur.

**Gerekçesi:**
1. **Oynanış ve Grid Uyumu:** Roguelite türü prosedürel (rastgele) oda üretimine dayanır. Konsept illüstrasyonlarını bütün olarak kullanmak (B veya C) oda şekillerini kısıtlar, çarpışma (collision) hesaplamalarını zorlaştırır ve Z-sorting (karakterin objelerin arkasına geçmesi) hatalarına yol açar.
2. **Dinamik Aydınlatma (URP 2D):** Gerçekten o karanlık atmosferi ve parlayan cyan rünleri elde etmek için Unity'nin URP 2D Global ve Point light sistemlerine ihtiyacımız var. 2D ışıklar, modüler tile'ların normal haritalarıyla (varsa) veya sprite'larıyla çok daha doğal etkileşime girer.
3. **Esneklik:** 9-slice (D) yöntemi fazla köşeli ve yapay durabilir. Modüler sistem, `CliffAutoPlacer` ile birleştiğinde organik ada kenarlarını anında ve hatasız üretmeni sağlar.

---

### 🔍 MEVCUT SORUNLARIN ANALİZİ
*   **Soluk/Beyaz Zemin:** URP 2D projelerinde, `Global Light 2D`'nin yoğunluğu (intensity) 1.0 veya daha yüksekse ve rengi saf beyazsa, dokuların orijinal kontrastını yok edip soluklaştırır. Ayrıca Tilemap materyali `Sprite-Lit-Default` yerine Unlit ise ışıkla etkileşime girmez.
*   **Kahverengi Uçurumlar (Cliffs):** Konseptteki "koyu gri taş" hissiyatı yerine eski assetler (`ref_kit_b`) kullanılıyor.
*   **Eksik Parıltı:** Cyan çatlakların parlaması için projenin URP Global Volume (Bloom) ayarlarına sahip olması gerekir.

---

### 🛠 CX İÇİN ADIM ADIM BUILD PLANI

Aşağıdaki plan, `cx`'in Unity projesinde sırasıyla uygulayacağı somut dosya, script ve asset değişikliklerini içerir.

#### ADIM 1: Koyu Granit Zemin (Dark Slate Floor) Düzeltmesi
Zemini solukluktan kurtarıp o "koyu granit" dokusuna kavuşturuyoruz.
*   **Tilemap Ayarı:** `_IsoGame` sahnesindeki `Floor` Tilemap objesini seç. `Tilemap Renderer` bileşenindeki `Color` değerini saf beyazdan **koyu gri/maviye (Örn: Hex `#60606B`)** çek. Bu, dokuyu anında koyulaştıracaktır.
*   **Materyal:** `Floor` Tilemap'in materyalinin kesinlikle `Sprite-Lit-Default` (URP 2D) olduğundan emin ol.
*   **Global Işık:** Sahnedeki `Global Light 2D` objesini bul. Rengini hafif mor/lacivert bir tona (`#2A2840`) getir ve `Intensity` değerini **0.5 - 0.6** arasına düşür. Ortam karanlık olmalı ki cyan parlasın.

#### ADIM 2: %5-8 Oranında Cyan Çatlak/Rün Entegrasyonu
Çatlakları zemine dengeli bir şekilde yayıyoruz.
*   **Script Yaklaşımı (Prosedürel Oda):** Oda üretimini yapan (Map Designer / RoomGenerator) script'e bir ağırlık (weight) sistemi ekle.
    *   Zemin oluşturulurken %95 ihtimalle `floor451`'in standart varyantlarından birini seç.
    *   **%5 ihtimalle** `floor451`'in **cyan damarlı/çatlaklı** varyantını seç.
*   **Alternatif (Tile Palette):** Unity'nin `RuleTile` veya `Random Tile` özelliğini kullanarak bir fırça oluştur. Bu fırçanın içine 1 adet cyan çatlaklı tile, 19 adet normal tile koy. Fırçayla boyadığında oran otomatik %5 olacaktır.

#### ADIM 3: Kalın ve Koyu Gri Uçurumlar (Dark Cliffs)
Kahverengi uçurumları atıp, konseptteki kalın, koyu uçurumları sisteme entegre ediyoruz.
*   **Asset Güncellemesi:** Kahverengi `ref_kit_b` sprite'larını kullanma. Eğer elinde PixelLab varsa, yeni "Koyu Gri/Slate İzometrik Uçurum (Dark Slate Isometric Cliff)" tile'ları üret. Yoksa, mevcut uçurum sprite'larını bir resim editöründe Desaturate (renksizleştir) edip Levels ayarıyla iyice koyulaştır (veya Unity içinden Sprite Color'ı `#303035` yap).
*   **Entegrasyon:** Bu koyu gri sprite'ları `DirectionalCliffTile` assetinin içine ata.
*   **Oto-Yerleşim:** `RoomCliffSolver` ve `CliffAutoPlacer` scriptlerinin bu yeni `DirectionalCliffTile` setini referans aldığından emin ol. Oda grid'inin en dış sınırına bunlar otomatik dizilecek.

#### ADIM 4: Mor Void (Atmosfer) Arkaplanı ve Z-Sorting
Uzayda süzülme hissini ve derinliği vermek için arka planı kuruyoruz.
*   **Katman Düzeni (Sorting Layers):** Editörde şu sıralamayı kur (En alttan en üste):
    1.  `Background` (Arkaplan)
    2.  `Cliffs` (Uçurumlar - Y-pivot'ları üstte olmalı)
    3.  `Floor` (Zemin)
    4.  `Entities` (Karakter, düşman, objeler)
*   **Void Kurulumu:** Sahneye bir adet `Sprite Renderer` içeren obje ekle, adını `Void_BG` yap. Sorting Layer'ını `Background` (-100 order) seç. İçine `KitC_BG` veya `BgKit_RefC` serisinden o girdaplı mor (purple swirl) dokuyu yerleştir.
*   **Kamera Takibi:** `Void_BG` objesini ya ana kameranın bir child'ı yap (sabit kalsın) ya da basit bir Parallax scripti bağlayarak kamerayla birlikte çok hafif kaymasını sağla.

#### ADIM 5: HDR ve Bloom (Glow Etkisi)
Cyan çizgilerin konseptteki gibi parlamasını (glow) sağlıyoruz.
*   **Post-Processing Kurulumu:** Sahneye bir `Global Volume` objesi ekle. Yeni bir Profile oluştur.
*   **Bloom Ekle:** Volume'a `Add Override -> Post-processing -> Bloom` ekle. `Intensity` değerini aç (Örn: 1.5), `Threshold` değerini 1.0 civarında tut.
*   **Cyan Tile HDR Ayarı:** Cyan damarlı tile sprite'larının parlaması için Unity Color Picker'da o cyan renginin etrafına bir "Point Light 2D" (renk: `#00FFCC`, intensity: 1.5, radius: ufak) koyabilirsin. Veya eğer proje HDR destekliyorsa, bu tile'ların üstüne denk gelecek görünmez bir Tilemap layer'ında "Color: HDR #00FFCC (Intensity 2)" vererek Bloom'un sadece o çatlakları yakalamasını sağlayabilirsin. (En temizi çatlaklı tile prefabı oluşturup içine ufak bir 2D point light koymaktır).

Bu build planı, `cx`'e (veya projeyi uygulayacak geliştiriciye) vizyonunu birebir, Unity'nin 2D ekosistemini bozmadan teknik olarak nasıl uygulayacağını gösteren eksiksiz bir yol haritasıdır.

