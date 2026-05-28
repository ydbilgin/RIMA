Warning: True color (24-bit) support not detected. Using a terminal with true color enabled will result in a better visual experience.
YOLO mode is enabled. All tool calls will be automatically approved.
YOLO mode is enabled. All tool calls will be automatically approved.
Ripgrep is not available. Falling back to GrepTool.
Attempt 1 failed: You have exhausted your capacity on this model. Your quota will reset after 1s.. Retrying after 5110ms...
Attempt 1 failed: You have exhausted your capacity on this model. Your quota will reset after 1s.. Retrying after 5194ms...
# RIMA Projesi İzometrik Zindan Duvarı Referans Araştırması

## 1. Giriş

RIMA gibi 2D roguelite oyunlarda, prosedürel olarak üretilen (procedural generation) zindanların görsel olarak tatmin edici ve oyun mekanikleri açısından okunabilir olması son derece kritiktir. İzometrik perspektif, oyuncuya derinlik hissi verirken taktiksel alanı genişletir. Geleneksel matematikte 30 derecelik bir açı kullanılmasına rağmen, "Pixel Art" disiplininde temiz ve tırtıksız (jagged-free) çizgiler elde etmek için her 2 yatay piksele karşılık 1 dikey pikselin çizildiği **26.56 derecelik (2:1 oranı)** "dimetrik" açı kullanılır. Bu oran, endüstri standardı olarak "İzometrik Pixel Art" olarak kabul edilir.

Aşağıdaki araştırma, RIMA'nın duvar mimarisi (ölçüler, köşe birleşimleri, tiling mantığı) için kopyalanmak üzere değil; **çözümlenmek ve referans alınmak üzere** özenle seçilmiş 5 farklı varlık (asset) paketini içermektedir. Bu paketler, 3 ücretsiz ve 2 ücretli seçenekten oluşan ideal bir dengeye ("sweet spot") sahiptir.

---

## 2. Referans Paket Adayları (Tablo)

Aşağıdaki tabloda listelenen paketler, RIMA'nın görsel dilini oluştururken "Sistemi nasıl kurmalıyım?" sorusuna yanıt verecek anatomik referanslardır.

| Paket Adı & Yazar | URL (Durum: Aktif) | Lisans & Fiyat | Boyut & Parça Analizi | Tiling Mantığı & Yön | Varyasyon (Hasar/Doku) | RIMA Uyum & Katkısı |
| :--- | :--- | :--- | :--- | :--- | :--- | :--- |
| **Isometric Miniature Dungeon**<br>*(Kenney)* | [Kenney.nl](https://kenney.nl/assets/isometric-miniature-dungeon) | Ücretsiz / CC0 (Kamu Malı) | ~70 Parça (Ana duvarlar, zeminler). Tile: ~64x32 zemin tabanlı. | Blok Tabanlı (Küp şeklinde). 4 yönlü kullanım. | Temiz, Varyasyon düşük. Modüler sadelik odaklı. | **HIGH:** En temel "Tile" anatomisini, boyut oranlarını ve collider mantığını anlamak için kusursuz bir sıfır noktası referansı. |
| **Isometric Walls "Library"**<br>*(Jaqmarti)* | [Itch.io](https://jaqmarti.itch.io/isometric-walls-library) | Ücretsiz (İsteğe Bağlı Ödeme) | ~20-30 Duvar Parçası. 2048x2048 Sprite Sheet. | Header (Üst) + Body (Gövde) Split. Yüksek duvarlar. | Kitaplıklı boşluklar, detaylı el çizimi doku. | **MEDIUM:** Uzun/yüksek zindan duvarlarının perspektifi bozmadan nasıl parçalara ayrılacağını (split rendering) öğretir. |
| **Isometric Dungeon Tileset**<br>*(Nilsen303)* | [Itch.io](https://nilsen303.itch.io/isometric-dungeon-tileset) | Ücretsiz (Ticari Serbest, Satış Yasak) | ~15 Duvar/Zemin. Genelde 64x64 veya 64x128. | Single Piece (Tek Parça). Uç birleşimleri. | Klasik gri taş dokusu, hafif yosun detayları. | **HIGH:** Az sayıda sprite ile "Seamless" (kesintisiz) bir koridor hissiyatının nasıl verileceğini doğrudan gösterir. |
| **Twilight Dungeons Isometric**<br>*(Night Quest Games)* | [Itch.io](https://nightquestgames.itch.io/twilight-dungeons-isometric-asset-pack) | $2.00 / CC BY-ND 4.0 | Detaylı kapılar, pencereler, mahzen duvarları. 256x132. | Modular + Decor (Duvar yüzeyi 128px + 4px kalınlık). | Gotik/Vampir temalı duvarlar, meşaleler. | **LOW:** Çözünürlüğü RIMA'dan yüksek olabilir fakat derinlik/ışıklandırma (highlight) hilelerini incelemek için çok güçlü bir kaynaktır. |
| **Isometric Dungeon Designer**<br>*(GalefireRPG)* | [Itch.io](https://galefirerpg.itch.io/isometric-dungeon-designer) | $4.99 / Standart Itch.io Lisansı | 100+ Duvar, İç/Dış köşe, T-Birleşim, Sütunlar. | Advanced Modular (Wang tarzı auto-tile uyumlu). | Kırık, yosunlu, çatlak duvarlar, molozlar. | **HIGH:** Procedural Generation için gereken *tüm anatomik parça listesini* (Kuzey, Güney, T-Junction vb.) kusursuz özetler. |

---

## 3. Endüstri Standardı Gözlemleri (Mühendislik ve Tasarım)

Yukarıdaki paketlerin incelenmesiyle ortaya çıkan, 2D izometrik roguelite projelerinde kullanılan değişmez endüstri kuralları şunlardır:

### A. Duvar Boyut Standartları (Wall Dimensions)
İzometrik pixel art'ta grid sistemi genellikle eşkenar dörtgen şeklindedir. Zemin karoları standart olarak `64x32` pikseldir. Zindan duvarları ise, zemin karesinin bir kenarına oturacak şekilde tasarlanır.
*   **Kısa Duvarlar:** Genellikle `64x64` veya `64x96` piksel tuval (canvas) kullanılarak çizilir. Bu, karakterin duvarın arkasında kaldığını rahatça göstermeye yetecek bir yüksekliktir.
*   **Derinlik (Thickness):** Duvarın sadece bir "karton" gibi görünmemesi için üst kısmında her zaman bir derinlik/kalınlık (header) çizilir. Bu kalınlık piksel sanatta genellikle zemin ile aynı eğimde (2:1) çizilen koyu veya açık bir şerit halindedir (örn. Twilight Dungeons paketindeki 4 piksellik kalınlık algısı).

### B. Yönlendirme ve Birleşim Anatomisi (Direction Handling)
Dinamik olarak oluşturulan zindanlarda, duvarların birbirine bağlanması (Auto-tiling/Bitmasking) en büyük sorundur. Bir duvar sisteminin "tam" sayılabilmesi için GalefireRPG paketinde gördüğümüz şu varyasyonlara sahip olması şarttır:
1.  **Straight Walls (Düz Duvarlar):** Sağ-Üst (Kuzeydoğu) ve Sol-Üst (Kuzeybatı) yönüne uzanan düz parçalar.
2.  **Inner Corners (İç Köşeler):** Odanın içine doğru kırılan "V" şeklindeki birleşimler.
3.  **Outer Corners (Dış Köşeler):** Odanın dışına doğru sivrilen çıkıntılı köşeler.
4.  **T-Junctions (T-Kavşakları):** Koridorların ikiye ayrıldığı noktalar.
5.  **End Caps (Uç Kapaklar/Sütunlar):** Yarım kalan bir duvarın havada asılı durmaması için sonlandırıcı taş bloklar veya sütunlar.

### C. Kusursuz Döşeme Mantığı (Smooth Tiling Pattern)
Zindan duvarlarında tekrar hissini (gridy look) gizlemek için temel iki yöntem izlenir:
*   **Single Piece (Tek Parça Modeli):** Duvar yüksekliği ve dokusu tek bir sprite içindedir (Nilsen303 paketi gibi). Çizimi ve Unity Tilemap'e entegrasyonu (özellikle Z-as-Y sorting için) daha kolaydır.
*   **Header+Body Split (Ayrık Model):** Duvarın yükselen gövdesi (Body) ile tepesindeki taş zemin (Header) ayrı çizilir (Jaqmarti yaklaşımı). Bu sayede geliştirici, aynı duvar gövdesini üst üste ekleyerek istediği yükseklikte bir uçurum veya uzun bir sur yaratabilir. Wang Tiling (komşuluk maskeleme) sistemleriyle kusursuz çalışır.

### D. Çevresel Varyasyonlar ve Yıpranma (Variation)
Zindan teması gereği yıpranmayı hissettirmelidir. İncelediğimiz profesyonel paketlerde varyasyonlar rastgele (random noise) olarak sahneye eklenir:
*   **Aşınma Türleri:** Yosun (Mossy), Çatlak (Cracked), Kan veya İsli dokular.
*   **Geçiş Elemanları:** Hasarlı veya yıkılmış bir duvar (Damaged), genellikle tek başına konmaz. Yıkık duvar parçasının dibine, zemin karosuyla geçişi yumuşatacak "ufak moloz yığınları" veya "dökülen taşlar" çizilir. Bu, pikselin zemine "oturmasını" sağlar.

---

## 4. RIMA İçin Ne Öğretir? (Pratik Çıkarımlar)

RIMA, prosedürel oluşturulan haritalara sahip olacağından, duvar çizimleri sanatsal olmaktan çok **"matematiksel ve modüler"** olmak zorundadır. Yukarıdaki referanslardan şu stratejiler uygulanmalıdır:

1.  **Modüler Sprite İhtiyaç Listesi:**
    RIMA'nın sanatçıları çizime başlamadan önce GalefireRPG'nin parça listesini kopyalayarak boş bir "şablon atlası" oluşturmalıdır. (1 Düz X, 1 Düz Y, 1 Dış Köşe, 1 İç Köşe vb.). Çizimler, duvarların birbirine geçeceği (overlap) piksel payları bırakılarak yapılmalıdır.
2.  **Işık ve Gölge Tutarlılığı:**
    Tüm paketlerde ışık kaynağı genellikle ekranın Sol-Üst (Top-Left) kısmından vurur. RIMA duvarlarında Kuzeydoğu'ya (Sağ üste) giden duvarların yüzeyi her zaman daha aydınlık çizilmeli, Kuzeybatı'ya giden yüzeyler ise daha koyu bir tonda bırakılarak sahnenin izometrik üç boyutluluğu (3D form algısı) pekiştirilmelidir.
3.  **Occlusion (Gizleme) Hazırlığı:**
    Oyun motorunda (Unity) oyuncu duvarın arkasına geçtiğinde duvarın saydamlaşması (Fade/Occlusion) gerekecektir. Kenney ve Nilsen303 paketlerindeki gibi duvarları "kare grid tabanlı" bağımsız nesneler olarak tasarlamak, Unity'nin Isometric Z-as-Y tilemap yapısında saydamlaştırma shader'larının (shader graph) sorunsuz çalışmasını sağlayacaktır.

Sonuç olarak; sanat stilini (Style Match) RIMA'nın kendi konsept tasarımı belirleyecek olsa da, **piksellerin köşe matematiklerini, gölge geçişlerini ve gerekli sprite sayısını** planlamak için *GalefireRPG* (yapı mekaniği için) ve *Nilsen303* (temiz piksel sanat geçişleri için) paketleri geliştirme sürecinde ana referans ekranında (Moodboard/PureRef) sürekli açık tutulmalıdır.
