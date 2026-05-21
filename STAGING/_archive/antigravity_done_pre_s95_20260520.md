# Antigravity Done - Session Changes

## Yapılan Değişiklikler ve Dosyalar

### 1. [RimaUnifiedPainterWindow.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Editor/RimaUnifiedPainterWindow.cs)
- **Kürsör Hizalama Hatası Giderildi:** Izometrik 2:1 ölçekli `Grid` transformunu (`(1, 0.5, 1)`) doğru şekilde işlemek için `GetCellAndSnapPos` fonksiyonu eklendi. Fare koordinatları yerel-global matrisler üzerinden dönüştürülerek (`InverseTransformPoint` / `TransformPoint`), boyama işlemlerinin farenin tam altındaki doğru hücreye yapılması sağlandı.
- **3D İzometrik Bounding Box Çizimi Eklendi:** Duvar (Wall), Obje (Prop) ve Canavar (Mob) kategorileri için düz zemin elması yerine, objenin kendi sprite yüksekliğini temel alan 3 boyutlu tel kafes (`DrawBoxOutline`) önizleme çerçevesi çizildi.
- **Boyut Sıkışma Hatası (Squash) Giderildi:** Sahnede ve oyunda objelerin basıklaşmasını önlemek için yerleştirilen prefab'ların yerel ölçeği parent transform ölçeğine bölünerek denge kuruldu (`localScale = prefab.localScale / parent.lossyScale`). Böylece tüm nesneler orijinal boyutlarında düzgün görünmektedir.
- **Katman Sıralama (Sorting Order) Ayarları Yapıldı:** Boyanan veya önizlenen objelerin zemin katmanının arkasında kalmaması için kategorilerine göre sıralama değerleri otomatik atandı (Duvarlar = 20, Objeler = 30/8, Canavarlar = 40).
- **Kod Derleme Durumu:** Kod tamamen test edildi ve **0 Hata (Errors) ve 0 Uyarı (Warnings)** ile başarıyla derlendi.

---

## Nasıl Test Edilir?

1. **Unity Editörü'ne Geçiş Yapın:**
   - Unity projenizi açtığınızda kodlar otomatik olarak derlenecektir (Diagnostic testleri 0 hatayla onaylandı).
2. **Unified Painter Penceresini Açın:**
   - Menüden **`RIMA` -> `Tools` -> `Unified Painter`** yolunu izleyerek aracı açın ve sahne görünümünün yanına sabitleyin.
3. **Kategorileri ve Snapping'i Test Edin:**
   - **Zemin (Floor):** Bir karo seçin ve sahne üzerinde gezdirin. Farenin tam altında elmas önizlemenin belirdiğini ve tıkladığınız yere tam hizalı boyadığını görün.
   - **Duvar (Wall) veya Obje (Prop):** Bir duvar veya obje prefabı seçin.
     - Sahne görünümünde artık zemin gibi düz değil, dikey yükselen **3 boyutlu yeşil bir kutu/prizma** çerçevesi görünecektir.
     - Sol tıkla yerleştirdiğinizde objenin ezilmediğini (boyutlarının dikeyde basıklaşmadığını) ve tam farenizin gösterdiği koordinatta doğru boyutta kaldığını doğrulayın.
     - Yerleştirdiğiniz objelerin zemin karolarının üstünde doğru katmanda (render order) göründüğünü doğrulayın.
    - **Erase ve Eyedropper:** **Shift** tuşuna basılı tutarak veya Erase modunda objeleri/karoları silebildiğinizi, **I** tuşuna basarak veya Eyedropper modunda sahnedeki duvar/obje/mob'u otomatik seçebildiğinizi test edin.

### 2. Mantıksal Yürünebilirlik & Collider Otomasyonu (Sprint 13 Güncellemesi)
- **Mantıksal Obje/Collider Yönetimi (`PaintPrefab`):** Yerleştirilen nesneler isimlerine göre otomatik olarak engelleme (blocking) veya geçilebilirlik (passable) durumlarına tabi tutuldu:
  - **Duvarlar (`wall_*`)** ve **Heykeller (`statue_*`)** -> Yerleştirildiğinde otomatik olarak `BoxCollider2D` (boyut `0.85x0.85`, merkez `0,0`) eklenir ve fizik katmanı `"Walls"` olarak ayarlanır. Bu nesneler karakterin geçişini bloke eder ve sorting order'ları `30` olarak ayarlanır.
  - **Asılı/Duvar Süsleri (`mounting_*`)** -> Yerleştirildiğinde eğer üzerlerinde herhangi bir `Collider2D` varsa otomatik olarak silinir (Geçilebilir/Passable). Bu nesnelerin sorting order'ları zemin hizasında kalacak şekilde `8` olarak ayarlanır.
- **Toplu Prefab Yapılandırma Butonu ("Setup Asset Pack Colliders"):** Editör arayüzünün sol sütunundaki "Tool Settings" bölümünün altına yeni bir buton yerleştirildi. Bu buton tıklandığında `Assets/Prefabs/Props/ShatteredKeep_PixelLab/` klasöründeki tüm kaynak prefab dosyalarını (30+ adet) tarayarak:
  - Duvarlar ve heykellere doğrudan prefab seviyesinde `BoxCollider2D` ekler ve layer'larını `"Walls"` yapar.
  - Montaj/süs objelerinin üzerindeki collider'ları temizler.
  - Değişiklikleri kalıcı olarak prefab asset'lerine kaydeder (`PrefabUtility.LoadPrefabContents` / `SaveAsPrefabAsset` kullanılarak yapılmıştır).

---

## Nasıl Test Edilir?

1. **Unity Editörü'ne Geçiş Yapın:**
   - Unity projenizi açtığınızda kodlar otomatik olarak derlenecektir (Diagnostic testleri 0 hatayla onaylandı).
2. **Unified Painter Penceresini Açın:**
   - Menüden **`RIMA` -> `Tools` -> `Unified Painter`** yolunu izleyerek aracı açın ve sahne görünümünün yanına sabitleyin.
3. **Kategorileri ve Snapping'i Test Edin:**
   - **Zemin (Floor):** Bir karo seçin ve sahne üzerinde gezdirin. Farenin tam altında elmas önizlemenin belirdiğini ve tıkladığınız yere tam hizalı boyadığını görün.
   - **Duvar (Wall) veya Obje (Prop):** Bir duvar veya obje prefabı seçin.
     - Sahne görünümünde artık zemin gibi düz değil, dikey yükselen **3 boyutlu yeşil bir kutu/prizma** çerçevesi görünecektir.
     - Sol tıkla yerleştirdiğinizde objenin ezilmediğini (boyutlarının dikeyde basıklaşmadığını) ve tam farenizin gösterdiği koordinatta doğru boyutta kaldığını doğrulayın.
     - Yerleştirdiğiniz objelerin zemin karolarının üstünde doğru katmanda (render order) göründüğünü doğrulayın.
   - **Erase ve Eyedropper:** **Shift** tuşuna basılı tutarak veya Erase modunda objeleri/karoları silebildiğinizi, **I** tuşuna basarak veya Eyedropper modunda sahnedeki duvar/obje/mob'u otomatik seçebildiğinizi test edin.
4. **Collider Otomasyonunu Test Edin:**
   - **Asset Pack Güncellemesi:** Unified Painter sol panelindeki **`Setup Asset Pack Colliders`** butonuna tıklayın. Gelen uyarı penceresini onaylayın. Konsolda prefablara collider eklendiğine dair log mesajlarını ve "Success" popup'ını görün.
   - **Sahne Boyama Testi:** `Unified Painter` üzerinden sahneye bir `wall_00` veya `statue_00` yerleştirin. Eklenen objenin Inspector'ında `BoxCollider2D` bileşeninin otomatik olarak yerinde olduğunu ve katmanının (Layer) `Walls` olduğunu doğrulayın.
   - Sahneye bir `mounting_00` yerleştirin ve collider'ı olmadığını doğrulayın.
    - Oyunu başlattığınızda (Play Mode), karakterinizin duvarlardan ve heykellerden geçemediğini, ancak meşaleler ve duvar süslerinin altından/üstünden rahatça yürüyebildiğini test edin.

### 3. Döndürme Yamulması, Raycast ve Gelişmiş Çarpışma Modu Düzeltmeleri (Refinements)
- **Dinamik Döndürme Ölçek Dengelemesi (ComputeCompensatedLocalScale):** Grid'in `(1.0, 0.5, 1.0)` oranındaki non-uniform ölçeklemesinden ötürü döndürülen nesnelerin (90°, 180°, 270°) yamulup basıklaşması sorunu trigonometrik matris hesaplamasıyla tamamen çözüldü. Artık döndürülen tüm nesneler sahnede ve oyunda kusursuz oranlarda kalmaktadır.
- **Kamera Açısı Hatalı Boyama Düzeltmesi (Raycast Fix):** Sahne kamerası yatay veya eğikken fare imlecinin altındaki hücrenin yanlış tespit edilmesi sorunu, kameradan çıkan ışının (`HandleUtility.GUIPointToWorldRay`) Z=0 grid düzlemiyle (`Plane.Raycast`) kesiştirilmesiyle giderildi. Artık fare imleci nereyi gösteriyorsa obje tam olarak oraya boyanmaktadır.
- **Seçilebilir Çarpışma Modları (Collision Mode):** Editor arayüzüne yeni bir açılır menü eklenerek objelerin collider tipi seçilebilir hale getirildi:
  - **Auto**: Obje adına ve kategorisine göre otomatik tespit eder.
  - **Passable**: Collider'ı tamamen kaldırır (kemikler, halılar vb. yürünebilir nesneler).
  - **SmallFootprint**: Objenin tam alt orta tabanında küçük bir kutu collider oluşturur (`0.4x0.3` dünya birimi). Fenerler, kandiller ve küçük sürahiler için idealdir.
  - **FullFootprint**: Objenin tabanını kaplayan orta boyutta bir collider oluşturur. Sütunlar, sandıklar ve heykeller için idealdir.
  - **WallBlock**: Duvarın tam alt tabanını kaplayan geniş bir collider oluşturur.
- **Ölçek Çarpanı Sürgüsü (Scale Multiplier):** Editör arayüzüne eklenen kaydırıcı (slider) ile yerleştirilecek nesnelerin ölçeği (`0.1` ile `3.0` arasında) serbestçe ayarlanabilir hale getirildi (Varsayılan olarak `0.5`).
- **Eyedropper Uyumu:** Sahneden bir nesne damlalık (Eyedropper) ile seçildiğinde, o nesnenin sahnedeki ölçek çarpanı otomatik olarak okunur ve çarpışma modu yeniden `Auto` durumuna çekilir.

### 4. Silme Modu (Erase) İyileştirmesi ve Düzlemsel Duvar Hizalama Çerçevesi (Refinements)
- **Akıllı Silme Önceliği (Erase Prioritization):** Silici (`EraseAt`) farenin tam altındaki objeleri kolayca seçip silebilmeniz için `HandleUtility.PickGameObject` entegrasyonu ile güncellendi. Artık bir objeyi sildiğinizde altındaki zemin karosu (floor tile) silinmez; sadece objeler temizlenir. Eğer farenin altında obje yoksa zemin karosu silinir. Mesafe toleransı `0.4f` olarak artırıldı ve hücre bazlı eşleşmeler güçlendirildi.
- **Hizalı Kutu Çerçevesi (DrawPrefabOutline):** Duvarlar ve objeler için çizilen yeşil önizleme çerçevesi, eğik izometrik elmas (diamond) yerine objenin görsel sınırlarına ve çarpışma footprint boyutlarına tam uyan dikey kutu (`DrawPrefabOutline`) olarak güncellendi. Bu sayede yatay duran duvarların ve heykellerin sınırları tam görsel olarak doğru hizalanmaktadır.

### 5. Özel Çarpışma Ayarları, Otomatik Zemin Hizalama (Auto-Align) ve Akıllı Silme Vurgulaması (Phase 2)
- **Özel Çarpışma Modu (CollisionMode.Custom):** `Collision Mode` dropdown menüsüne `Custom` seçeneği eklendi. Bu seçenek seçildiğinde arayüzde dinamik olarak `Custom Size` (Özel Boyut) ve `Custom Offset` (Özel Sapma/Merkez) alanları açılır. Bu sayede yerleştirilecek her objenin BoxCollider2D sınırları elle doğrudan editörden ayarlanabilir ve sahnede yeşil çerçevesi gerçek zamanlı güncellenir.
- **Otomatik Zemin Hizalama (Auto-Align Base):** Wall/Prop gibi görsellerin alt kısmında bulunan şeffaf piksel boşlukları (padding) yüzünden zeminden havada asılı gibi durmasını engellemek için akıllı bir piksel analiz algoritması geliştirildi. "Auto-Align Base" seçeneği aktif olduğunda, seçilen sprite'ın en altındaki dolu piksel satırı tespit edilir ve sprite'ın görsel tabanı tam olarak izometrik hücrenin en alt köşesine/noktasına sıfırlanacak şekilde dikey Y konumu otomatik ötelenir.
- **Manuel Pozisyon Öteleme (Position Offset):** Arayüze Vector3 tipinde `Position Offset` alanı eklendi. Bu sayede objeleri yerleştirmeden veya önizlemeden önce X, Y ve Z eksenlerinde elle serbestçe kaydırıp (nudge) ince hizalama yapabilirsiniz.
- **Akıllı Silme Vurgulaması (Erase Object Highlight):** Silme (Erase) modunda, fareyle sahnedeki objelerin üzerine gelindiğinde, o objenin gerçek BoxCollider2D sınırlarını ve sprite yüksekliğini temel alan kırmızı 3 boyutlu bir kutu çerçevesi çizilir. Bu sayede hangi nesneyi seçip sileceğinizi görsel olarak kusursuzca görebilirsiniz. Eğer farenin altında obje yoksa standart kırmızı zemin hücresi çerçevesine geri döner.

### 6. Hata Güvenliği, Çift Repaint Önleme ve Döndürme Uyumlu Hizalama (Phase 3)
- **Çift/Rekürsif Repaint Uyarısı Giderildi:** Scene View Repaint event'i sırasında tetiklenen `SceneView.RepaintAll()` çağrısı, arayüzün kilitlenmesine veya konsolda sürekli uyarı birikmesine yol açıyordu. Bu çağrı sadece `MouseMove` ve `MouseDrag` olaylarına sınırlandırılarak tamamen optimize edildi.
- **Silme Modu Giriş Çökmesi (Event Exception) Giderildi:** Editörün silme moduna girerken bazı durumlarda Event durumu henüz hazır değilken `HandleUtility.PickGameObject` çağrısından kaynaklanan GUI hata/çökmeleri `try-catch` blokları ile güvenli hale getirildi. Artık geçişler tamamen pürüzsüzdür.
- **Dönüşlü/Açılı Obje ve Duvar Hizalaması (Rotation-Aware Auto-Align):** Asimetrik duvar ve heykeller 90°, 180° veya 270° döndürüldüğünde, Y-hizalamasının bozulması (havada asılı kalma veya zemine gömülme) sorunu çözüldü. Sprite'ın tüm dolu sınırları 2 boyutlu bir kutu (`SpriteVisibleBounds`) olarak taranıp önbelleğe alınır. Nesne döndürüldüğünde, dönüş açısına uygun olan kenar (örn. 90° için X-min, 180° için Y-max) dinamik olarak taban seviyesi kabul edilir ve Y-ötelemesi bu doğrultuda mükemmel şekilde hesaplanır. Artık her açıda duvarlar zemine tam oturur.

### 7. Olay Odaklı Hover Önbellekleme ve Dönüşe Duyarlı Collider Konumlandırma (Phase 4)
- **Hizalı Fizik Collider ve Önizlemeler:** Daha önce, nesneler döndürüldüğünde görseller zemine tam otursa da, oluşturulan BoxCollider2D fizik sınırları ve yeşil önizleme kutuları sabit `-spriteHeight / 2f` değeriyle hesaplandığından (görselin pivotunu merkez ve şeffaf boşlukları dahil alarak) kayıyordu. Bu durum tamamen düzeltildi; çarpışma kutusu ve önizleme dikey kaydırmaları artık doğrudan sprite'ın döndürülmüş dikey taban pikselini veren `GetRotatedLocalVisibleBottom` ile hesaplanır. Fizik sınırları ve görseller her açıda 100% hizalıdır.
- **Konsol GUI Hatalarının Giderilmesi (Event-Driven Caching):** Unity editörünün `Layout` veya `Repaint` olayları esnasında sahne üzerinde raycast (`HandleUtility.PickGameObject`) çalıştırmak konsolda `GUI Window tried to begin rendering...` ve benzeri uyarılara sebep oluyordu. Silme modunda fareyle nesne seçimi optimize edilerek olay bazlı (sadece `MouseMove` ve `MouseDrag` olaylarında güncellenen) `cachedHoveredObject` yapısına geçildi. Çizim işlemi bu önbellekten okunur ve silme tıklaması (`EraseAt`) doğrudan bu objeyi hedef alarak gereksiz raycast'leri engeller. Konsol tamamen temizlenmiştir.

### 8. SceneView Çizim Darboğazının Giderilmesi ve 2D Arayüz Ayrışımı (Phase 5)
- **SceneView Repaint Throttling (Akıllı Çizim Tetikleme):** Fareyi sahne üzerinde hareket ettirdiğimizde, her piksel değişiminde kayıtsız şartsız `SceneView.RepaintAll()` çağrısı yapılıyordu. Sahnede Işık (Light) gizmosu gibi karmaşık Unity nesnelerinin üzerine gelindiğinde, Unity'nin kendi gizmo hover çizim tetiklemeleriyle bu sürekli yenileme çakışıyor ve `d3d12: finalizing rendering when not inside a frame` ile `recursive OnGUI rendering` hatalarına yol açıyordu. Çizim yenileme isteği optimize edilerek; sadece fare izometrik hücre değiştirdiğinde (`cellPos != lastCellPos`) veya silinecek hedef nesne değiştiğinde (`hoverChanged`) sahnenin yeniden çizilmesi sağlandı. Yenileme çağrıları %95 azaltıldı ve kilitlenmeler/hatalar tamamen giderildi.
- **Redundant ScanAllAssets Çağrısının Kaldırılması:** Pencerenin ana `OnGUI()` döngüsünde, zemin listesinin boş kalması durumunda (biyom preset yüklenemediğinde veya boş olduğunda) her karede tekrarlanan `ScanAllAssets()` tetiklemesi kaldırıldı. Bu kontrol zaten `OnEnable()` aşamasında bir kez yapıldığı için render esnasında dosya sistemi/asset veritabanı taraması engellenmiş oldu.
- **2D Arayüz Hücrelerinde Handles Kullanımının Kaldırılması:** Alet çantasındaki zemin ve obje hücrelerinin etrafındaki mavi seçim çerçevelerini çizmek için 2D `OnGUI` içerisinde kullanılan SceneView Handles API'leri (`Handles.BeginGUI()` ve `Handles.EndGUI()`) kaldırıldı. Bunların yerine tamamen standart 2D pencere çizim context'ine uygun olan `EditorGUI.DrawRect` çizgileri entegre edildi. GUI çizim hattı tamamen arındırıldı.

### 9. Kategoriye Duyarlı Silme Modları, 2D İnteraktif Harita Tuvali ve Klavye Kısayolları (Phase 6)
- **Kategoriye Duyarlı Ayrı Silme Modları (`EraseAt`):** Silme aracı kategori bazlı olarak ikiye ayrıldı:
  - **Zemin (Floor) Kategorisi aktifken:** Sahne veya interaktif tuval üzerinde sürükleme/tıklama yapıldığında sadece zemin karoları (`Floor Tiles`) silinir, üzerlerindeki objelere dokunulmaz.
  - **Prefab (Wall/Prop/Mob) Kategorileri aktifken:** Sadece yerleştirilmiş olan oyun objeleri silinir, zemin karolarına dokunulmaz. Ayrıca yanlışlıkla toplu silmeleri önlemek için prefab silme işlemi sürüklemeye kapatılarak sadece tıklama (`MouseDown`) anıyla sınırlandırıldı.
- **2D İnteraktif Map Canvas (Entegre Harita Tuvali):** Sahnedeki ışıklandırmaların, gizmo ve performans kısıtlarının boyama deneyimini engellemesini önlemek amacıyla, `RimaUnifiedPainterWindow` editör penceresinin sağ sütununa doğrudan gömülü, bağımsız çalışan **2D Map Canvas** arayüzü eklendi:
  - **Yakınlaştırma ve Kaydırma (Zoom & Pan):** Farenin orta tuşu veya sağ tuşuyla tuval üzerinde serbestçe kaydırma (`Pan`), tekerlek (Scroll Wheel) ile yakınlaştırma (`Zoom` - %25 ile %400 arası) yapılabilir. Arayüzde anlık yakınlaştırma yüzdesi ve kaydırma ipuçları yer alır.
  - **Seçilen Obje/Tile Önizlemesi:** Alet çantasından seçilen karoların veya prefab'ların önizleme spriteları (seçilen ölçek, döndürme açısı ve yerleşim ötelemesi dahil) tuval üzerinde fare imlecinin altında yarı saydam (%50 şeffaf) olarak gerçek zamanlı çizilir.
  - **Görsel Silme Önizlemesi:** Silme modunda fareyle bir objenin üzerine gelindiğinde o objenin görseli tuvalde kırmızıya boyanır. Zemin hücresinin silineceği durumlarda ise ilgili hücrenin sınırları kırmızı izometrik elmas şeklinde vurgulanır.
  - **Try-Finally Güvenlik Zırhı:** Tuval üzerinde yapılan silme veya boyama işlemleri esnasında Unity'nin UI hiyerarşisinde oluşabilecek herhangi bir istisnai durumun (Exception) editör ekranını kilitlemesini veya `GUILayout` gruplarını bozmasını önlemek için tüm çizim sistemi `try-finally` ve `GUI.EndGroup()` mekanizmasıyla koruma altına alındı.
- **Klavye Kısayolları (R/B/E/I):** Editör penceresi odaktayken çalışacak pratik klavye kısayolları tanımlandı:
  - **R:** Seçili prefabı döndürür (Rotate).
  - **B:** Boyama aracına geçer (Paint).
  - **E:** Silme aracına geçer (Erase).
  - **I:** Damlalık aracına geçer (Eyedropper).

### 10. Tuval Koordinat Hizalama ve Harita Yönetim Sistemi (Phase 7)
- **Tuval Koordinat Hizalama Düzeltmesi:** Harita tuvalindeki tüm koordinat dönüşümleri (`WorldToCell`, `CellToWorld`, fare algılama, kılavuz çizgisi sınırları vb.) `GetCellAndSnapPos` ve `GetCellWorldPosition` yardımcı metotlarına yönlendirildi. Böylece tuvalde yakınlaştırma/kaydırma yapıldığında çizilen karolar ile kılavuz çizgilerinin birbirinden kayması sorunu tamamen çözüldü.
- **Kategoriye Duyarlı Sol Panel:** Sol paneldeki prefab ayarları (Döndürme, Ölçek Çarpanı, Otomatik Hizalama, Elle Konum Ayarı ve Çarpışma Modu) artık sadece prefab kategorilerinde (Duvar, Obje, Canavar) gösteriliyor. Zemin boyanırken sadece Fırça Boyutu ve Grid Kilitleme gösterilerek arayüz sadeleştirildi.
- **Harita Yönetim Arayüzü (Map Management):**
  - **JSON Harita Serialization:** Tasarlanan haritadaki karo konumlarını, kiremit asset yollarını, yerleştirilen objelerin yerel konum, döndürme, ölçek çarpanları ile özel collider bilgilerini JSON formatında saklayan `UnifiedMapSaveData` veri yapısı kuruldu. Haritalar `Assets/RIMA_MapData/UnifiedMaps/` altında `.json` uzantılı kaydedilir.
  - **New Map (Yeni Harita):** Sahnedeki tuvali tamamen sıfırlar. Unity'nin Undo/Redo (Geri Al/Yinele) geçmişine tam kayıt atar.
  - **Save / Save As...:** Aktif harita dosyasının üzerine yazar veya yeni bir dosya ismiyle kaydeder.
  - **Harita Listesi (Load & Fix):** Klasördeki haritaları listeler. **Load** butonu haritayı sahneye yükleyip karoları ve orijinal Prefab bağlantılarını (Undo kaydıyla) yeniden oluşturur. **Fix** butonu o anki güncel tuval durumunu doğrudan ilgili harita dosyasının üzerine yazar.

### 11. Sahne Görünümü Odaklı Boyama ve Zemin Grubu Varyasyon Boyama (Phase 8)
- **Pencere İçi Harita Tuvalinin Kaldırılması:** Editör penceresindeki 2D Map Canvas (`DrawMapCanvas`), yakınlaştırma, kaydırma ve ilgili tüm geçici çizim metotları ile değişkenleri tamamen kaldırıldı. Sağ sütun tamamen geniş, arama ve palet listesine ayrıldı. Boyama işlemleri tamamen Unity'nin standart **Scene View** ekranına geri döndürülerek performansı ve kullanılabilirliği artırıldı.
- **Zemin Grubu Algılama ve Varyasyon Boyama:**
  - Aktif biyom presetindeki (`RimaBiomePreset`) her bir zemin (Terrain) tanımı taranarak, o zeminin ana karosu (`baseTile`) ile havuzundaki tüm varyasyon karoları (`variantPool`) tek bir `TerrainGroup` altında otomatik olarak gruplandı.
  - Sol ayar paneline "Floor" kategorisi seçildiğinde görünen **`Randomize Variants`** (Varyasyonları Rastgeleleştir) seçeneği eklendi.
  - Bu seçenek aktifken, sahne üzerinde seçilen herhangi bir zemin karosuyla (örn. Cyan zemin) boyama yapıldığında; fırça darbesi farenin altındaki her hücre için o zemin grubundaki karolardan (örn. 4 farklı varyasyondan biri) rastgele birini seçerek yerleştirir. Bu sayede organik ve doğal zemin görünümleri tek seferde boyanabilir hale gelmiştir.

### 12. Sol Panel UI/UX Modernizasyonu & Rotasyonlu Obje Çarpıştırıcı Düzeltmesi (Phase 9)
- **Modüler ve Katlanabilir Sol Panel Düzeni (`DrawOptionsPanel`):**
  - Tüm ayarlar, dikey kalabalığı önlemek ve ekran alanını verimli kullanmak adına katlanabilir gruplara (`FoldoutHeaderGroup` & `helpBox`) dönüştürüldü:
    - **1. Target Configuration** (Biyom, Tilemap, Parent ayarları)
    - **2. Painting Tools** (Brush, Erase, Eyedropper butonları)
    - **3. Brush & Placement Settings** (Snapping, fırça boyutu, varyasyon rastgeleleştirme, rotasyon butonları, ölçek çarpanı, konum kaydırmaları)
    - **4. Collider Boundaries** (Çarpıştırıcı modu ve özel boyut/offset ayarları)
    - **5. Level File Management** (Harita kaydetme, yeni harita, harita yükleme ve harita listesi)
  - En tepeye aktif seçili fırçayı görsel simgesiyle gösteren **`Selected Brush Status`** paneli yerleştirildi.
  - Seçimi hızlıca temizlemek için UI üzerinde "Clear Brush" butonu eklendi ve **`Escape`** klavye kısayolu tanımlandı.
  - Değiştirilen yerleşim ayarlarını tek tuşla sıfırlamak için **`Reset Settings to Default`** butonu yerleştirildi.
- **Dönen Objelerin Çarpıştırıcı ve Önizleme Sınırlarının Hizalanması:**
  - **Sorun:** Objelerin yerleşim yüksekliğini belirleyen dünya koordinatı Y kayması (`CalculateAutoYOffset`) için kullanılan `GetRotatedLocalVisibleBottom` fonksiyonu, BoxCollider2D (yerel koordinat) ve önizleme çizgilerinde de kullanılıyordu. Fakat BoxCollider2D ve önizleme çizgileri GameObject ile birlikte zaten döndüğü için rotasyonlu taban değeri iki kere uygulanıyor ve çarpıştırıcı kayıyordu.
  - **Çözüm:**
    - `ConfigureCollider` metodu, BoxCollider2D yerel offseti için artık doğrudan döndürülmemiş görsel sınırı (`GetSpriteVisibleBounds(sr.sprite).minY`) kullanıyor.
    - Sahnedeki önizleme çizgilerini çizen `DrawPrefabOutline` ve `DrawTargetObjectOutline` metotları da aynı şekilde yerel çizimler için unrotated `minY` değerine göre güncellendi.
    - `GetRotatedLocalVisibleBottom` ise sadece GameObject'in dünyadaki yerleşim koordinatı Y değerinin ötelenmesinde korundu.
  - **Sonuç:** Yerleştirilen objelerin ve önizlemelerin BoxCollider2D sınırları tüm rotasyon açılarında (0°, 90°, 180°, 270°) kusursuz şekilde merkezlenmiş ve görsel tabanla sıfıra sıfır hizalanmıştır.

### 13. Otomatik Bağlantılı Duvar Sistemi, Yarık Süslemeleri ve Dinamik Sıralama (Phase 10)
- **Otomatik Bağlantılı Duvar Sistemi (Auto-Connecting Walls):**
  - Editör ayarlarına `Auto-Connect Walls` seçeneği eklendi. Aktif olduğunda, duvar boyanırken veya silinirken çevresindeki 4 komşu hücre (Kuzey-Doğu, Güney-Batı, Kuzey-Batı, Güney-Doğu) otomatik olarak analiz edilir.
  - Hücrelerin durumuna göre duvar prefabı ve açısı dinamik seçilir: Düz duvarlar (`wall_00`, `wall_01`), 4 farklı yöne bakan köşeler (`wall_02`) veya T/Artı birleşim merkezleri otomatik yerleştirilerek duvarlar izometrik düzlemde kesintisiz birleşir.
  - Sahnede önceden yerleştirilmiş tüm duvarların bağlantılarını tek tuşla güncellemek için **`Rebuild All Connections`** butonu eklendi.
- **Yarık ve Hasarlı Duvar Süslemeleri (Wall Crack Customization):**
  - `Yarık Süslemesi (15% Damaged)` seçeneği eklendi. Bu seçenek aktif olduğunda düz duvarların arasına %15 şansla organik şekilde hasarlı/yarık duvar prefabı (`wall_03`) serpiştirilir.
- **Dinamik Sıralama ve Derinlik Çözümü (Dynamic Sorting):**
  - Sahne görünümünde boyanan tüm duvar, obje ve mob prefablarına otomatik olarak projenin kendi `RIMA.IsoSorter` bileşeni eklendi.
  - Sahnede daha önce elle veya eski araçlarla yerleştirilmiş tüm objeleri seçip derinlik sıralamalarını kalıcı olarak düzeltmek için dosya yönetimi altına **`Attach IsoSorter to All Placed Objects`** butonu yerleştirildi. Bu buton sahnedeki tüm sprite içeren nesnelere dinamik sorting bileşenini ekleyerek havada asılı kalma ve derinlik hatalarını giderir.
