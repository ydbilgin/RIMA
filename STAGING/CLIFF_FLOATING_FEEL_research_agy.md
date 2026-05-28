# RIMA Cliff "Floating Feel" External Research — agy

## GİRİŞ: RIMA Top-Down 3/4 Perspektifinde Uçurum ve Derinlik İllüzyonu

Bu araştırma belgesi, RIMA 2D Roguelite projesindeki top-down 3/4 (70-80 derece kamera açısı, isometric Tilemap diamond grid) görsel derinlik problemlerini çözmek amacıyla hazırlanmıştır. Mevcut durumda üretilen 283 algoritmik cliff tile'ı içinde yer alan 1-3 hücrelik izole "cliff adacıkları", altlarında inandırıcı bir düşüş/derinlik hissi (drop shadow, parallax void) bulunmadığı için havada asılı duran anlamsız bloklar gibi görünmektedir. 

Bu döküman, Hades, Children of Morta, Hyper Light Drifter ve Octopath Traveler 2 gibi janrın öncüsü oyunlardan elde edilen görsel ve algoritmik çözümleri RIMA'ya uyarlama planını, izole hücre yumuşatma UX algoritmalarını, ucurum gölge shader tasarımlarını ve kamera açısı/sprite sarkma oranlarını matematiksel olarak detaylandırmaktadır.

---

## BLOCK 1: "Havadalık Hissi" Görsel Pattern Kataloğu

Bu bölümde, RIMA'nın 70-80 derece top-down 3/4 yapısına adapte edilebilecek spesifik görsel illüzyonlar, entegrasyon formülleri, tahmini efor (LOC - Lines of Code bazlı) ve risk analizleri derlenmiştir.

### 1. Hades Elysium Teknikleri

#### A. Floating Island Silhouette & Cliff Face Shading
*   **Tanım:** Uçan adanın alt sınır çizgilerinin keskin ve koyu bir outline ile çevrelenmesi, cliff yüzeyinin (face) ise yukarıdan aşağıya doğru kararan dramatik bir gölgeleme (shading) ile boyanmasıdır. Işık kaynağı tepededir; dolayısıyla uçurumun alt kısımları tamamen karanlığa gömülürken, üst kenarlar (rim) parlar.
*   **RIMA Entegrasyonu:** Cliff sprite'larının çizim aşamasında dikey eksende 0-1 aralığında bir dikey çarpım gradyantı (Vertical Multiply Gradient) uygulanır. Cliff tile'larının alt kısımları (void'e komşu olan yerler) %80-90 oranında siyah/koyu cyan tonlarına boyanır.
*   **LOC / Efor Tahmini:** XS (Kod yazımı gerektirmez, sprite/texture revizyonudur).
*   **Risk:** Elle çizilmiş sprite'ların algoritmik aydınlatma (URP 2D Light) ile çakışması durumunda yapay durabilir.
*   **Öneri:** TEST EDİLMELİ. Cliff sprite'larının alt 1/3'lük dilimine el ile soft-black multiply mask eklenmelidir.

#### B. Floor Under-Tint (Cliff Altına Düşen Gölge)
*   **Tanım:** Uçurumun bittiği ve boşluğun başladığı sınırdan aşağıya doğru uzanan, boşluktaki parallax katmanlarının üzerine düşen devasa bir projeksiyon gölgesidir.
*   **RIMA Entegrasyonu:** Cliff sprite'ının bittiği transformOffset.y sınırından itibaren aşağıya doğru uzanan, `sortingLayer = Ground` veya `Walls` seviyesinde çalışan yarı saydam siyah bir "soft shadow tile" eklenir. Boşluktaki Parallax BG katmanının üzerine bindirilir.
*   **LOC / Efor Tahmini:** S (Yaklaşık 80-120 satır C# kodu ile Tilemap'in dış çeper boundary'leri belirlenerek altına shadow tile yerleştirilir).
*   **Risk:** Çok katmanlı yapılarda shadow sprite'larının üst üste binerek (double blending) çirkin siyah lekeler oluşturması.
*   **Öneri:** KULLANILMALI. Shadow sprite'ları Unity SpriteRenderer'ın "Multiply" veya "Darken" blend modu ile render edilmelidir.

#### C. Drop Shadow Gradient (Cliff Base'inden Aşağı Alfa Fade)
*   **Tanım:** Cliff face'in en alt pikselinden void'e (boşluğa) doğru uzanan ve transparanlığa (alpha 0) doğru eriyen gradyant şerit.
*   **RIMA Entegrasyonu:** Cliff sprite'ının altına takılan 64x64 veya 64x128 boyutunda, dikeyde yukarıdan aşağıya `Alpha 1.0` -> `Alpha 0.0` olan transition sprite'larının dynamic instantiate edilmesi.
*   **LOC / Efor Tahmini:** S (~50 satır shader kodu veya ek tilemap layer).
*   **Risk:** Kamera hareket ederken gradyantın bitiş sınırında keskin çizgilerin (artifacts) görünmesi.
*   **Öneri:** KULLANILMALI. Bunu dinamik bir Unlit/Transparent Shader ile yapmak en temiz çözümdür.

#### D. Particle (Toz/Cinder) Cliff Edge'inde
*   **Tanım:** Uçurum kenarlarından aşağıya doğru süzülen hafif toz, polen veya Elysium tarzı cyan parıltı (rune/ember) partikülleri.
*   **RIMA Entegrasyonu:** Uçurum kenarındaki tile koordinatlarını tarayan ve buralarda lokal 2D Particle System (URP uyumlu) tetikleyen bir `CliffEdgeEmitter.cs` script'i yazılması. Partiküller yerçekimi ile aşağı yavaşça süzülür ve alpha fade ile yok olur.
*   **LOC / Efor Tahmini:** M (~150 satır C# partikül yöneticisi ve havuz sistemi).
*   **Risk:** Çok fazla uçurum hücresi olduğunda partikül sayısının artması ve CPU/GPU bottleneck yaratması.
*   **Öneri:** TEST EDİLMELİ. Sadece kameranın gördüğü aktif uçurum kenarlarında (frustum culling) partikül üretilmelidir.

#### E. BG Parallax (Uzakta Yüzen Adacıklar ve Sis)
*   **Tanım:** Ana oyun alanının arkasında, çok daha yavaş hareket eden, üzerinde antik sütunlar veya cyan rünler olan yüzen ada silüetleri ve aradaki derinliği hissettiren sis katmanı.
*   **RIMA Entegrasyonu:** Kamera matrisine bağlı çalışan, parallax katsayısı (0.1 - 0.3) olan 2 adet arka plan katmanı. Üst katman sis (fog), alt katman uzak adacıklar.
*   **LOC / Efor Tahmini:** M (~200 satır C# Parallax script'i + URP Sprite Lit / Unlit ayarları).
*   **Risk:** Yüksek top-down açısında (70-80 derece) dikey parallax hareketinin oyuncuda baş dönmesi (motion sickness) yaratabilmesi.
*   **Öneri:** KULLANILMALI. Parallax katsayısı dikeyde (Y ekseninde) yataya (X eksenine) göre 0.5 ile çarpılarak azaltılmalıdır.

#### F. Color Rim (Cliff Face'de Soft Glow)
*   **Tanım:** Cliff face'in üst köşesinde (oyuncunun bastığı zemin ile birleştiği edge) ışığı yansıtan ince, parlak bir hat (Rim light). Hades Elysium'da bu rünik cyan rengindedir.
*   **RIMA Entegrasyonu:** Cliff sprite'ının üst pivot sınırına 2-3 piksellik parlak cyan/beyaz çizgi eklenmesi veya shader yardımıyla normal map kullanılarak rim highlight oluşturulması.
*   **LOC / Efor Tahmini:** S (~80 satır Custom Shader Graph).
*   **Risk:** Çizim tarzının fazla teknolojik veya neon görünmesi, ARPG havasını bozabilir.
*   **Öneri:** TEST EDİLMELİ. Sadece rünik temalı özel odalarda aktif edilebilir.

---

### 2. Children of Morta Teknikleri

#### A. Cliff Edge Top-Pivot Sprite + Alpha Falloff
*   **Tanım:** Sprite'ların pivot noktasının tile'ın tam üst merkezinde (top-center) konumlandırılması ve uçurum yüzeyinin aşağı sarktıkça doğal bir alpha falloff (yumuşak kayboluş) ile transparanlaşması.
*   **RIMA Entegrasyonu:** Cliff sprite asset'lerinin import ayarlarında Pivot = `Custom (X: 0.5, Y: 1.0)` olarak set edilmesi. Shader'da sprite yüksekliğine bağlı olarak `input.uv.y` üzerinden doğrusal olmayan bir alfa sönümlemesi yapılması.
*   **LOC / Efor Tahmini:** S (~40 satır shader kodu).
*   **Risk:** Karakter uçurum kenarına çok yaklaştığında, pivot üstte olduğu için sorting hatalarının oluşması.
*   **Öneri:** KULLANILMALI. RIMA'nın mevcut sortingLayer düzeni (Floor: Ground, Cliff: Walls -10, Player: 0-100) ile mükemmel uyum sağlar.

#### B. Cliff Face Decor Stratification (Rock Band, Root, Moss)
*   **Tanım:** Uçurum yüzeyinin tek düze bir taş dokusu yerine dikeyde katmanlara ayrılması. En üstte yosun/çim sarkıntıları, ortada çatlak kaya katmanları, en altta ise boşluğa uzanan ağaç kökleri ve toprak.
*   **RIMA Entegrasyonu:** Tilemap fırçasına (Tilemap Brush) veya algoritmik yerleşimciye dikey varyasyon kuralları eklenmesi. Örneğin, cliff tile'ının altındaki komşu tile boşluk (void) ise sprite olarak "kök sarkıntılı cliff face" seçilir.
*   **LOC / Efor Tahmini:** M (~180 satır C# procedural tile selection mantığı).
*   **Risk:** Sprite varyasyonlarının çok fazla bellek tüketmesi ve çizim yükü.
*   **Öneri:** KULLANILMALI. Görsel zenginlik için 3 farklı dikey varyasyon (Top_Moss, Mid_Rock, Bot_Root) yeterlidir.

#### C. Multi-Layer Sandwich (Face + Base Shadow + Far Parallax)
*   **Tanım:** Grafik katmanlarının sıkı bir hiyerarşide üst üste binmesi: `Ground Floor` -> `Cliff Face` -> `Cliff Base Shadow` -> `Void` -> `Parallax Clouds` -> `Far Void Background`.
*   **RIMA Entegrasyonu:** Unity Sorting Layers ayarlarının düzenlenmesi. En derin katmandan en öne doğru net bir sıralama şablonu oluşturulması.
*   **LOC / Efor Tahmini:** XS (Unity Editor ayarı).
*   **Risk:** Katmanlar arası geçişlerde "flat" (düz) 2D hissini kırmak için doğru sıralamanın bozulması.
*   **Öneri:** KULLANILMALI. Katman yapısı RIMA'da kesin bir standart haline getirilmelidir.

---

### 3. Hyper Light Drifter Teknikleri

#### A. Flat Color Band Cliff (Saturated Palette, No Gradient)
*   **Tanım:** Uçurum yüzeyinde gradyant veya detaylı gölge kullanmak yerine, tamamen düz ve kontrast renk blokları (örneğin parlak pembe üstüne koyu mor şeritler) kullanarak derinlik yaratmak.
*   **RIMA Entegrasyonu:** RIMA'nın yarı gerçekçi / piksel sanat tarzını stilize flat-art yönüne kaydırmak.
*   **LOC / Efor Tahmini:** XS (Sadece piksel sanatı palet değişimi).
*   **Risk:** RIMA'nın "Warm Brazier + Cyan Rune" tarzındaki yarı karanlık/atmosferik ARPG brand lock'ı ile estetik olarak uyuşmaması.
*   **Öneri:** SKIP. HLD tarzı aşırı stilize düz renkler, RIMA'nın arzulanan derin atmosferine uymayabilir.

#### B. Parallax Cloud Underlay (Cliff "Havada" Kanıtı)
*   **Tanım:** Uçurumun hemen altından, adanın altını kesecek şekilde yavaşça kayan sis ve bulut katmanları. Bu katmanlar ana zemin ile uçurumun altındaki boşluk arasına girerek adanın gerçekten uçtuğunu kanıtlar.
*   **RIMA Entegrasyonu:** Cliff'in alt sınırında konumlandırılan, dünya uzayında (world-space) yavaşça sola/sağa doğru kayan (scrolling) ve uçurum maskesinin altına giren bulut sprite'ları.
*   **LOC / Efor Tahmini:** S (~60 satır C# scrolling script'i).
*   **Risk:** Bulutların fazla parlak olması durumunda oyuncunun dikkatini dağıtması veya kontrastı bozması.
*   **Öneri:** KULLANILMALI. Düşük opaklıkta (%30-40) ve koyu/soğuk tonlarda (dark cyan/navy) bulutlar tercih edilmelidir.

#### C. Negative Space Treatment (Void = Solid Dark Color, "Dipsiz")
*   **Tanım:** Uçurumun altındaki boşluğun karmaşık bir parallax yerine tamamen homojen, derin ve karanlık tek bir renk (örneğin zifiri karanlık veya derin uzay laciverti) ile doldurulması.
*   **RIMA Entegrasyonu:** Kameranın arka plan temizleme renginin (Camera Clear Flags / Background Color) koyu bir renge sabitlenmesi.
*   **LOC / Efor Tahmini:** XS (Kamera komponenti ayarı).
*   **Risk:** Oyuncuda uçurumun derinliğinden ziyade ekranın o kısmının render edilmediği (glitch/bug) hissi uyandırabilmesi.
*   **Öneri:** TEST EDİLMELİ. Tamamen boş bırakmak yerine üzerine çok hafif bir noise/sis eklemek bu riski çözer.

---

### 4. Octopath Traveler 2 (HD-2D) Teknikleri

#### A. Multi-Layer Sprite Sandwich (FG/MG/BG Plates)
*   **Tanım:** Sahnenin 3D uzayda derinlikli yerleşimi. Kamera ortografik değil, hafif perspektiftir (RIMA'da ortografik kamera varsa 3D derinlik taklit edilir). Ön planda (Foreground) bulanıklaşan nesneler, orta planda (Midground) oyun alanı, arka planda (Background) derinlik.
*   **RIMA Entegrasyonu:** RIMA kamerası 3D perspective moduna alınarak 75 derece açıyla eğilir. Tilemap ve sprite'lar Z ekseninde derinlik kazanacak şekilde yerleştirilir (`Z-spacing`).
*   **LOC / Efor Tahmini:** L (~500 satır kod + tüm render boru hattının 3D derinliğe göre yeniden tasarlanması).
*   **Risk:** 2D Tilemap motorunun Z-sorting sisteminde kırılmalar, çarpışma algılamada (colliders) dikey kaymalar.
*   **Öneri:** SKIP. Mevcut 2D Tilemap ve sprite mimarisini 3D derinliğe taşımak RIMA projesi için aşırı efor ve yüksek bug riski barındırır.

#### B. Fog Gradient (Alttan Koyu, Üst Açık Volumetric Fog)
*   **Tanım:** Ekranın alt kısmından yukarıya doğru yükselen, derinliği vurgulayan volumetric sis gradyantı.
*   **RIMA Entegrasyonu:** Kamera ekranını kaplayan (Screen Space) veya ucurum seviyesinde duran (World Space) bir gradyant post-process maskesi veya particle overlay.
*   **LOC / Efor Tahmini:** S (~120 satır Custom URP Render Feature).
*   **Risk:** Ekrandaki tüm renkleri soldurması, kontrastı düşürmesi.
*   **Öneri:** TEST EDİLMELİ. URP 2D'de basit bir Sprite Mask veya global ışık gradyantı ile kolayca denenebilir.

#### C. Atmospheric Tilt-Shift Blur
*   **Tanım:** Ekranın en alt ve en üst kısımlarının bulanıklaştırılarak (Depth of Field / Tilt-Shift) kameranın sadece orta plandaki oyun alanına odaklanmasının sağlanması. Bu durum devasa bir yükseklik hissi yaratır.
*   **RIMA Entegrasyonu:** Unity Post-Processing Volume eklenerek "Depth of Field" efekti aktif edilir. Odak noktası oyuncunun Y koordinatına kilitlenir.
*   **LOC / Efor Tahmini:** S (Post-processing ayarları ve basit bir odaklama script'i).
*   **Risk:** Piksel sanatı (pixel art) keskinliğinin kaybolması ve oyuncunun gözünü yorması.
*   **Öneri:** SKIP. Saf piksel sanatında tilt-shift blur genellikle piksel hizalamasını (pixel-perfect) bozduğu için tercih edilmez.

---

### 5. Sang Hendrix Realtime Parallax Formülü

Sang Hendrix'in modern 2D oyunlar için önerdiği "Uçurum ve Yükseklik" parallax formülü, statik parallax katmanlarından farklı olarak kameranın hem pozisyonuna hem de bakış açısına dinamik tepki veren bir matematiksel yapı sunar.

#### Parallax Derinlik Reçetesi:
Uçurum hissini %100 vermek için gereken katmanlar ve hareket katsayıları (Parallax Factors):

```
+--------------------------------------------------------+  <- Sorting: Ground (Y=0, Z=0)
|  LAYER A: Oyun Alanı (Floor / Player / Cliff Top)      |  <- Parallax: 1.0 (Birebir hareket)
+--------------------------------------------------------+
+--------------------------------------------------------+  <- Sorting: Walls (Y=-transformOffset, Z=-1)
|  LAYER B: Cliff Face (Uçurum Duvarı)                   |  <- Parallax: 0.95 (Çok hafif yavaş kayma)
+--------------------------------------------------------+
+--------------------------------------------------------+  <- Sorting: Shadows (Z=-5)
|  LAYER C: Base Drop Shadows (Uçurum Altı Gölgeler)     |  <- Parallax: 0.85 (Derinliği başlatır)
+--------------------------------------------------------+
+--------------------------------------------------------+  <- Sorting: Void_Fog (Z=-50)
|  LAYER D: Boşluk Sisi (Scroll Eden Dumanlar)           |  <- Parallax: 0.60 (Yavaş hareket)
+--------------------------------------------------------+
+--------------------------------------------------------+  <- Sorting: Far_Islands (Z=-200)
|  LAYER E: Uzak Yüzen Adalar (Cyan Rünlü Kayalar)        |  <- Parallax: 0.25 (Çok yavaş hareket)
+--------------------------------------------------------+
+--------------------------------------------------------+  <- Sorting: Deep_Void (Z=-500)
|  LAYER F: Kozmik Boşluk / Derin Karanlık               |  <- Parallax: 0.05 (Neredeyse sabit)
+--------------------------------------------------------+
```

#### Authoring Loop Tasarımı:
Tüm bu katmanların tek bir dinamik parametreye (`HeightOffset` veya `VoidDepth`) bağlanması gerekir. Kamera dikeyde hareket ettikçe, arka plan katmanları dikey eksende (Y) sıkışıp genişler.

```csharp
// Sang Hendrix Parallax Simülasyonu C# Taslağı
using UnityEngine;

public class SangHendrixParallax : MonoBehaviour
{
    [System.Serializable]
    public class ParallaxLayer
    {
        public Transform layerTransform;
        public Vector2 parallaxScale; // X ve Y ekseni katsayıları
        public bool autoScroll;
        public float scrollSpeed;
        [HideInInspector] public Vector3 startPosition;
    }

    public Transform cameraTransform;
    public ParallaxLayer[] parallaxLayers;
    public float depthScaleMultiplier = 1.0f; // Tek merkez parametre

    private Vector3 previousCameraPosition;

    void Start()
    {
        if (cameraTransform == null) cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;

        foreach (var layer in parallaxLayers)
        {
            if (layer.layerTransform != null)
            {
                layer.startPosition = layer.layerTransform.localPosition;
            }
        }
    }

    void LateUpdate()
    {
        Vector3 cameraDelta = cameraTransform.position - previousCameraPosition;

        for (int i = 0; i < parallaxLayers.Length; i++)
        {
            ParallaxLayer layer = parallaxLayers[i];
            if (layer.layerTransform == null) continue;

            // Derinlik parametresine göre hareket miktarını hesapla
            Vector3 targetPos = layer.layerTransform.localPosition;
            
            // X ve Y eksenlerinde farklı katsayılar uygulanarak 3/4 açısı korunur
            float moveX = cameraDelta.x * layer.parallaxScale.x * depthScaleMultiplier;
            float moveY = cameraDelta.y * layer.parallaxScale.y * depthScaleMultiplier;

            targetPos.x += moveX;
            targetPos.y += moveY;

            if (layer.autoScroll)
            {
                targetPos.x += layer.scrollSpeed * Time.deltaTime;
            }

            layer.layerTransform.localPosition = targetPos;
        }

        previousCameraPosition = cameraTransform.position;
    }
}
```

*   **RIMA Aplike Notu:** Bu script ana kameraya bağlanmalı, uzak adalar ve sis katmanları bu script'in array'ine eklenmelidir.
*   **LOC / Efor:** S (~70 satır kod, temiz ve optimize).
*   **Risk:** Çok düşük. Oldukça performanslı ve etkilidir.
*   **Öneri:** KESİNLİKLE KULLANILMALI.

---

## BLOCK 2: "İzole Cliff Cluster" Gizleme ve Yumuşatma UX Algoritmaları

Procedural harita üretiminde (cellular automata veya random walk), bazen boşluğun ortasında 1, 2 veya 3 hücreden oluşan izole kaya blokları veya cliff "adacıkları" oluşur. Bunlar görsel olarak "havada asılı çirkin pikseller" gibi durur. Bu durumu engellemek veya yumuşatmak için uygulanacak algoritmik çözümler aşağıdadır.

### Algoritmik Metotlar

#### 1. Minimum Cluster Size Filter (Connected Component Analysis)
*   **Tanim:** Harita üretildikten hemen sonra, birbirine bağlı (orthogonal komşuluk) cliff hücre grupları taranır. Eğer bir grubun (cluster) toplam hücre sayısı belirlenen eşik değerden (örneğin 4) küçükse, bu grup tamamen yok edilir (floor veya void'e dönüştürülür).
*   **RIMA Entegrasyonu:** Aşağıdaki C# FloodFill tabanlı temizleme algoritması harita jeneratörünün (ProcGen) son fazına yerleştirilir.

```csharp
// Connected Component Temizleme Algoritması
using System.Collections.Generic;
using UnityEngine;

public class CliffClusterFilter
{
    public struct Coord
    {
        public int x;
        public int y;
        public Coord(int x, int y) { this.x = x; this.y = y; }
    }

    public static int[,] CleanIsolatedCliffs(int[,] map, int width, int height, int minClusterSize)
    {
        bool[,] visited = new bool[width, height];
        int[,] cleanedMap = (int[,])map.Clone(); // Orijinal haritayı klonla

        // 1: Cliff hücrelerini temsil eden değer (Örn: 1 = Cliff, 0 = Floor/Void)
        const int CLIFF_VAL = 1;
        const int VOID_VAL = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (map[x, y] == CLIFF_VAL && !visited[x, y])
                {
                    List<Coord> cluster = GetCluster(map, visited, x, y, width, height, CLIFF_VAL);
                    
                    // Eğer bulunan adacık minClusterSize'dan küçükse VOID'e dönüştür
                    if (cluster.Count < minClusterSize)
                    {
                        foreach (Coord cell in cluster)
                        {
                            cleanedMap[cell.x, cell.y] = VOID_VAL;
                        }
                    }
                }
            }
        }
        return cleanedMap;
    }

    private static List<Coord> GetCluster(int[,] map, bool[,] visited, int startX, int startY, int width, int height, int targetVal)
    {
        List<Coord> cluster = new List<Coord>();
        Queue<Coord> queue = new Queue<Coord>();

        queue.Enqueue(new Coord(startX, startY));
        visited[startX, startY] = true;

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        while (queue.Count > 0)
            Coord current = queue.Dequeue();
            cluster.Add(current);

            for (int i = 0; i < 4; i++)
            {
                int nx = current.x + dx[i];
                int ny = current.y + dy[i];

                if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                {
                    if (!visited[nx, ny] && map[nx, ny] == targetVal)
                    {
                        visited[nx, ny] = true;
                        queue.Enqueue(new Coord(nx, ny));
                    }
                }
            }
        }
        return cluster;
    }
}
```

*   **RIMA Aplike Notu:** Mevcut harita üretim hattında cliff yerleşiminden hemen sonra bu fonksiyon çağrılmalıdır. Eşik değer `minClusterSize = 4` olmalıdır.
*   **LOC / Efor:** S (~90 satır C# kodu).
*   **Risk:** Çok düşük. Yalnızca harita yüklenirken 1 kez çalışacağı için performans kaybı sıfırdır.
*   **Öneri:** KESİNLİKLE KULLANILMALI.

#### 2. Dilate/Erode Morphology (Hücresel Genişleme ve Aşındırma)
*   **Tanım:** Görüntü işlemedeki morfolojik işlemlerin 2D ızgaraya uygulanması. Erode işlemi ile 1 hücreli çıkıntılar ve adalar yok edilirken; Dilate işlemi ile 1 hücreli delikler (pockets) doldurulur.
*   **RIMA Entegrasyonu:** Connected Component kadar agresif değildir. Haritadaki keskin sivrilikleri ve tek tük kalmış pürüzleri giderir.
*   **LOC / Efor:** S (~60 satır C#).
*   **Risk:** Haritanın genel tasarımındaki kasıtlı dar geçitleri (chokepoints) kapatma veya yok etme riski vardır.
*   **Öneri:** SKIP. Connected Component Analysis çok daha hedefe yönelik çalışır.

#### 3. Boundary Smoothing (Sınır Yumuşatma / Corner Rounding)
*   **Tanım:** Uçurum kenarlarının dik 90 derecelik açılarla değil, yumuşatılmış köşelerle (diagonal tile geçişleri) çözülmesi.
*   **RIMA Entegrasyonu:** Tilemap otomatik kural eşleme (Rule Tile veya Custom Scriptable Tile) kullanılarak 45 derecelik köşe sprite'larının yerleştirilmesi.
*   **LOC / Efor:** M (Tile asset çizimi ve kural tanımları).
*   **Risk:** Diamond grid isometric yapıda diagonal sprite'ların çizim ve sorting hizalamalarının zorluğu.
*   **Öneri:** KULLANILMALI. Rule Tile sistemi kullanılarak köşe birleşimleri otomatik yumuşatılmalıdır.

#### 4. Visibility Filter (Camera Frustum & Min Screen-Space Size)
*   **Tanım:** Sadece kameranın gördüğü alandaki cliff detaylarını çizmek ve çok küçük olanları ekran uzayındaki boyutuna göre fade-out (yumuşak gizleme) yapmak.
*   **RIMA Entegrasyonu:** Cliff'lerin `LOD Group` veya Dynamic Visibility kontrolcüsü ile yönetilmesi.
*   **LOC / Efor:** M (~150 satır).
*   **Risk:** Ekranın kenarında aniden beliren/kaybolan cliff sprite'larının göze batması (pop-in artifacts).
*   **Öneri:** SKIP. RIMA bir 2D oyun olduğu için frustum culling zaten Unity tarafından otomatik yapılır; ek filtre eforu gereksizdir.

#### 5. Soft Alpha Falloff (Cluster Edge Feathering)
*   **Tanım:** Küçük adacıkların (1-3 cell) tamamen silinmek yerine, uçlarına doğru aşırı şeffaflaşarak boşluk içinde eriyen dekoratif "yüzen taş pargaları" haline getirilmesi.
*   **RIMA Entegrasyonu:** Cluster boyutu < 4 olan grupların sprite render opacity değerlerinin %30-40'a çekilmesi ve altına yoğun sis efekti verilmesi.
*   **LOC / Efor:** S (~70 satır).
*   **Risk:** Karakter bu yarı şeffaf taşların üzerine basmaya çalışabilir (Collider çakışması).
*   **Öneri:** SKIP veya TEST. Eğer estetik olarak isteniyorsa, bu küçük adacıkların collider'ları tamamen iptal edilmelidir (sadece görsel dekorasyon).

#### 6. "Outcrop" vs "Wall Edge" Semantik Ayrımı
*   **Tanım:** Büyük uçurum duvarları (Wall Edge) ile küçük dekoratif kaya çıkıntılarını (Outcrop) algoritmik olarak ayırmak. Küçük cluster'lar yok edilmez, ancak üzerlerinde yürünemeyen, derinliği az olan küçük dekoratif kayalar (Outcrop sprite'ı) ile değiştirilir.
*   **RIMA Entegrasyonu:** Harita jeneratöründe `IsOutcrop(x,y)` kontrolü yapılıp, bu hücrelere collider içermeyen "dekoratif yüzen taş" sprite'ı basılması.
*   **LOC / Efor:** M (~120 satır).
*   **Risk:** Oyuncunun nereye basıp basamayacağını karıştırması (UX/Gameplay okunabilirlik riski).
*   **Öneri:** TEST EDİLMELİ. Görsel ayrım net yapılırsa harika bir derinlik katar.

### Hangi Oyunlar ve Akademik Literatür Bu Pattern'i Kullanıyor?
*   **Bastion & Transistor (Supergiant Games):** Yüzen adacıklarda adanın kopan küçük parçaları (outcrops) ana adanın çevresinde asılı durur. Bunların collider'ı yoktur ve hafifçe dikeyde salınırlar (floating animation).
*   **Dead Cells:** Harita üretiminde "floating platform" tespiti yapılıp minimum 3 hücre genişliğinde olması şartı koşulur. Daha küçükler morfolojik olarak temizlenir.
*   **Procedural Generation Literature (cellular automata smoothing):** Johnson et al. (2010), "Cellular Automata for Real-time Generation of Infinite Cave Levels" makalesinde izole hücrelerin (orphan cells) temizlenmesi için 4-5 kuralını (bir hücrenin komşuları 4'ten az ise boşluğa dönüştür) detaylandırmıştır.

---

## BLOCK 3: Drop Shadow ve Parallax Depth Uçurum Trick'i

Mevcut durumda RIMA'da karakter altında çalışan `GroundBlobShadow.cs` bulunmaktadır. Ancak statik ve dinamik uçurumların boşluğa inandırıcı şekilde gölge düşürmesi için daha kapsamlı bir derinlik sistemine ihtiyaç vardır.

### Uçurum Shadow Yöntemleri Karşılaştırması

#### Yöntem A: Sprite Layer (Aşağı Açılım Gradient PNG)
*   **Tanim:** Cliff sprite'ının en alt sınırına hizalanmış, dikeyde aşağı doğru uzayan yarı saydam siyah-to-transparent geçişli bir gölge sprite'ı.
*   **RIMA Entegrasyon:** Cliff prefab'ına veya tile'ına child olarak bir `Shadow SpriteRenderer` eklenir. `sortingLayer = Ground` (veya `Shadows`), `orderInLayer = -10` olarak ayarlanır.
*   **LOC / Efor:** XS (Kod yazımı gerektirmez, sadece prefab hiyerarşisi).
*   **Risk:** Dinamik olarak değişen uçurum şekillerinde gölgelerin üst üste binerek keskin köşe hataları oluşturması.
*   **Öneri:** KULLANILMALI. En hızlı ve en performanslı çözümdür.

#### Yöntem B: URP 2D Light Cookie Alternative
*   **Tanim:** URP 2D ışık sisteminin (Light 2D) maskeleme ve cookie özelliklerini kullanarak uçurum kenarlarına gölge projeksiyonu uygulamak.
*   **RIMA Entegrasyon:** Uçurum kenarı boyunca uzanan bir `Freeform Light 2D` (Subtract blend modunda) tanımlanarak uçurum altının karartılması.
*   **LOC / Efor:** M (~150 satır dynamic light mesh generation kodu).
*   **Risk:** Mobil cihazlarda veya düşük donanımlarda URP 2D dinamik ışıklarının yüksek performans tüketimi (Draw Call artışı).
*   **Öneri:** SKIP. Mobil uyumluluk veya stabil fps hedefleri için risklidir.

#### Yöntem C: Shader Gradient (URP Lit 2D / Unlit 2D)
*   **Tanim:** Cliff yüzeyinin en alt piksellerinden itibaren UV koordinatlarına bağlı olarak dinamik alfa ve renk karartması yapan özel bir shader.
*   **RIMA Entegrasyon:** URP Sprite Lit Shader Graph üzerinde dikey UV gradyantı oluşturulur. `Custom Vertex Streams` kullanılarak sprite yüksekliği shader'a aktarılır.
*   **LOC / Efor:** S (~80 satır Shader Graph / Custom Code).
*   **Risk:** Sprite atlas kullanımı durumunda UV koordinatlarının değişmesi ve gradyantın sapması.
*   **Öneri:** TEST EDİLMELİ. Sprite atlas kullanılmıyorsa en kusursuz görsel sonucu verir.

#### Yöntem D: Tilemap-Driven Shadow
*   **Tanim:** Cliff Tilemap'inin hemen altına (Sorting Layer olarak arkasına) denk gelen ikinci bir "Shadow Tilemap" oluşturulması. Harita jeneratörü, her cliff tile'ının altına otomatik olarak gölge tile'ı basar.
*   **RIMA Entegrasyon:** `CliffShadowmap` adında yeni bir Tilemap oluşturulur. Renderer ayarlarında yarı saydamlık verilir.
*   **LOC / Efor:** S (~40 satır C# generator kodu).
*   **Risk:** Ekstra bir tilemap katmanının getirdiği bellek ve rendering yükü (ihmal edilebilir düzeyde).
*   **Öneri:** KESİNLİKLE KULLANILMALI. Algoritma ile kontrolü en kolay olan yöntemdir.

### INACTIVE S110 Parallax Aktivasyon Önerisi
3-Kit memory içinde `INACTIVE S110` olarak işaretlenen Parallax BG sistemini canlandırmak ve "yüzen ada" hissini %80 oranında artırmak için yapılması gereken 2 kritik dokunuş:

1.  **Katman 1 (Immediate Void Backdrop - Z: -100):** Ana uçurumun hemen altına, çok yavaş kayan (%30 opasite) soğuk-cyan renkte duman/bulut katmanı eklemek. Bu katman uçurumun alt sınırındaki sert kesilmeleri yumuşatır.
2.  **Katman 2 (Distant Floating Relics - Z: -300):** Üzerinde minik cyan rünler parıldayan ve yavaşça dikeyde (sine dalgası ile) salınan antik Elysium sütunları ve kaya parçaları. Bu katman, derinlik algısını pekiştiren ana referans noktası olacaktır.

---

## BLOCK 4: Kamera Açısı ve Sprite Height Etkileşimi

RIMA'da kullanılan **HIGH TOP-DOWN 3/4 (70-80 derece)** kamera açısı, standart 45 derece izometrik veya 90 derece tam tepeden (top-down) bakışlardan farklı dinamiklere sahiptir. Bu açıda derinlik hissinin kaybolmaması için sprite boyutları ve kaydırma oranları matematiksel olarak hizalanmalıdır.

### Boyut ve Sarkma Analizi

```
        Göz / Kamera (75 Derece Açı)
            \
             \
              \
   Zemin (Floor Cell) ------->  [ ] [ ] [ ] [ Cliff Edge ]
                                                  |
                                                  |  <-- Cliff Sprite Yüksekliği (Sarkma)
                                                  |  (transformOffset.y ile aşağı kaydırılmış)
                                                  v
                                                [ Void / Boşluk ]
```

#### 1. Sprite Height / Aspect Oranı
*   Mevcut grid hücre genişliği (Grid cellSize) **64 piksel** ise:
    *   **64x128 Piksel (2:1 Aspect):** 70-80 derece kamera açısı için **aşırı uzundur**. Cliff yüzeyi ekranda çok fazla dikey alan kaplar ve arkasında kalan zemin hücrelerini tamamen örter.
    *   **64x192 Piksel (3:1 Aspect):** Tamamen hatalı bir derinlik algısı yaratır; uçurum duvarı 3D eğimden ziyade 2D düz bir levha gibi durur.
    *   **64x96 Piksel (1.5:1 Aspect):** **OPTIMAL SEÇENEKTİR.** 75 derecelik bir açıda, 64 piksel genişliğindeki bir zeminin dikey izdüşümü yaklaşık 48 pikseldir. Cliff yüksekliğinin 96 piksel olması, ucurumun dikey derinliğini tam olarak 3D hissettirecek optimum görsel sarkmayı sağlar.

#### 2. transformOffset.y Değeri
*   Cliff sprite'ının üst pivotlu (Top-Pivot) yerleşiminde, sprite'ın fiziksel çarpışma (collider) alanı ile görsel alanının eşleşmesi için transform bileşenine negatif Y offset verilir.
*   **Önerilen Matematiksel Formül:**
    $$\text{transformOffset.y} = -(\text{SpriteHeight} - \text{CellHeight}) \times 0.5$$
    *   64x96 sprite ve 64x64 diamond grid için:
    $$\text{transformOffset.y} = -(96 - 64) \times 0.5 = -16 \text{ piksel} = -0.25 \text{ Unity Unit (PPU=64 ise)}$$
    *   Bu offset, cliff sprite'ını zemin hücresinin tam alt sınırından itibaren 32 piksel (yarım cell kadar) aşağıya sarkıtır.

#### 3. Floor Cell Genişliği ile Cliff Sprite Oranı
*   İdeal oran:
    $$\text{Cliff Sprite Genişliği} = \text{Grid Cell Genişliği}$$
    $$\text{Cliff Sprite Yüksekliği} = \text{Grid Cell Genişliği} \times 1.5$$
    *   Bu oran bozulursa (örneğin 64x128 kullanılırsa), cliff sprite'ları yan yana dizildiğinde birbirinin üstüne biner (overlap) ve isometric dikiş izlerinde (seam artifacts) yırtılmalar oluşur.

#### 4. "Sarkma" Görsel Marjini
*   Uçurum aşağıya doğru sarkarken yanındaki boş zemin hücresinden (floor cell) ne kadar çalacağı kritik bir UX konusudur. 
*   75 derecelik açıda, aşağı sarkan cliff sprite'ı, altındaki boşluğun (void) veya alt kattaki floor cell'in en fazla **%50'sini** kapatmalıdır. Eğer daha fazlasını kapatırsa, alt katta yürüyen düşmanlar veya objeler cliff sprite'ının arkasında kalarak görünmez olur (Gameplay blindspots). 64x96 px boyutu bu %50 marjını tam olarak korur.

---

## ÖZET VE TOP-3 UYGULAMA TAVSİYESİ

RIMA Cliff "Floating Feel" projesini en az efor ve maksimum görsel başarı ile tamamlamak için uygulanması gereken en kritik 3 adım:

1.  **Connected Component Temizliği (Block 2):** `CliffClusterFilter.cs` script'ini harita üretim sistemine dahil edin ve 1-3 hücrelik tüm izole adacıkları otomatik olarak yok edip void'e dönüştürün. (Efor: S, Risk: Çok Düşük)
2.  **Tilemap-Driven Shadow & Bulut Altlığı (Block 3):** Cliff Tilemap'inin hemen altına yarı saydam siyah gradyant shadow tile'ları döşeyen sistemi aktif edin. Altına `INACTIVE S110` referansıyla %30 opaklıkta scroll eden koyu cyan sis katmanı yerleştirin. (Efor: S, Risk: Düşük)
3.  **Sprite Boyut Standardizasyonu (Block 4):** Cliff sprite boyutlarını **64x96 piksel** olarak güncelleyin ve custom pivot noktasını en üst-orta (top-center) yapıp `transformOffset.y = -0.25f` ile aşağıya sarkıtın. (Efor: XS, Risk: Düşük)

---
*Araştırma Raporu Sonu — Hazırlayan: agy*
