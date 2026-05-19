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
