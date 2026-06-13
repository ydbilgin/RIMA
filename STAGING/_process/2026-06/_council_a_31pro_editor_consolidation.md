**Konsey (Gemini 3.1 Pro - Yüksek) Yanıtlıyor: Derin Mimari ve Test Stratejisi**

RIMA'nın editör araçlarındaki bu parçalanma ve çakışma, organik büyümenin tipik bir sonucudur. Senin önerdiğin "Kullanıcı Arayüzü (uGUI) + Ortak Çekirdek (Core)" modeli kesinlikle doğru hedeftir. Ancak bu birleşimin sağlam kalması için keskin sınırlar ve katı sözleşmeler (contracts) oluşturmalıyız.

İşte derin mimari ve regresyon-korumalı test stratejisi planı:

### 1) KONSOLİDASYON MİMARİSİ: Tek "Truth" (Gerçeklik) Modeli

**Önerin DOĞRU.** IMGUI (F2) editörü tamamen silinmeli/emekli edilmelidir. uGUI (Build Mode) yeni ön-yüz, `UnifiedDesignerCore` ise arka-uç (back-end) olmalıdır.

**Mimari Soyutlama Sınırları ve Sözleşmeler:**

*   **Veri ve Depolama (`ILevelStore` / `UnifiedMapData`):** Tüm harita verisi (Tile, Prop, Walkability) saf C# veri sınıflarında tutulmalıdır. `UnifiedDesignerCore` bu verinin tek sahibidir (Single Source of Truth) ve JSON Load/Save işlemlerini yönetir. uGUI *asla* doğrudan veriyi manipüle etmemelidir.
*   **Komut Deseni (Command Pattern):** Undo/Redo mekanizması sadece uGUI'ye ait olmamalıdır. `ICommand` (örn. `PlaceTileCommand`, `RemovePropCommand`) arayüzü oluşturulmalıdır. uGUI, kullanıcının tıklamalarını `ICommand` nesnelerine dönüştürüp `UnifiedDesignerCore.Execute(ICommand cmd)` metoduna gönderir. Çekirdek (Core) kendi undo/redo yığınını (stack) tutar. Böylece Edit-Mode (Unity Editor) ve Play-Mode (uGUI) aynı işlemleri, aynı kurallarla, aynı undo/redo sistemiyle çalıştırır.
*   **Tool Registry (Araç Kayıt Sistemi):** `ITool` (Fırça, Silgi, Seçim) arayüzü tanımla. Araçlar (Tools), sadece koordinat alıp `ICommand` üreten mantık birimleridir. `ToolRegistry`, aktif aracı tutar. uGUI sadece `ActiveTool.ProcessInput(worldPos)` çağrısı yapar.
*   **Olay Odaklı Güncelleme (Event-Driven View):** Çekirdek veri değiştiğinde (örneğin bir tile boyandığında), core `OnMapDataChanged(dirtyRect)` event'ini fırlatır. Play-Mode renderer (Tilemap vb.) ve Editor-Mode renderer sadece bu event'i dinleyip kendini günceller. uGUI, renderer'ı bilmez.

### 2) REGRESYON-BOZULMAZLIK: Otomatik Test Mimarisi

Kullanıcının "#1 isteği" olan tekrar-bozulmazlık, manuel QA ile değil, yapısal testlerle sağlanır.

**A. Çakışma Önleyici "Keybind Guard" (Mimari Çözüm + Test)**
*   Sorun: İki farklı script'in `Input.GetKeyDown(KeyCode.F2)` beklemesi.
*   Çözüm: Merkezi bir `GlobalInputRegistry` (veya Input System actions). Sisteme "Özel/Öncelikli (Exclusive)" tuş kaydı özelliği eklenmelidir.
*   **Koruma Mantığı:** Eğer `BuildModeController` "F2" tuşunu Exclusive olarak kaydetmişse, eski `InPlayMapPaintOverlay` uyanıp "F2"yi kaydetmeye çalıştığında sistem **Exception (Hata) fırlatmalıdır**.
*   **Test (PlayMode):** Başlangıçta tüm UI ve Tool sistemlerini yükleyen bir PlayMode testi yazılır. İki sistem aynı tuşa "Exclusive" olarak abone olmaya çalışırsa test başarısız (Fail) olur. Bu, çakışmayı CI (Sürekli Entegrasyon) anında yakalar.

**B. "Roundtrip" (Gidiş-Dönüş) ve Core Testleri (EditMode)**
EditMode testleri saniyeler içinde çalışır ve core mantığı korur.
*   **Save/Load Roundtrip Testi:** Boş bir harita oluştur -> Çeşitli proplar ve tile'lar yerleştir (kod ile) -> JSON'a Serialize et -> Yeni bir instance'a Deserialize et -> İki verinin (prop pozisyonları, walkability matrisi) birebir aynı olduğunu `Assert.AreEqual` ile doğrula. Bu test asla atlanamaz.
*   **Walkability Testi:** "Duvar tile'ı koy, pathfinding 'kapalı' döndürsün", "Zemin koy, 'açık' döndürsün".

**C. Durum ve Görünürlük Testleri (PlayMode)**
*   **Overlap-Hide Testi:** Bir PlayMode test sahnesi kur. Ana oyun UI'ını aç. Sonra kod üzerinden `BuildModeController.Open()` çağır. Assert ile `MainMenuCanvas.enabled == false` ve `BuildModeCanvas.enabled == true` olduğunu doğrula. Kapatıldığında tam tersini doğrula.

*Not: Unity Test Runner (UTF) CI/headless ortamda komut satırı argümanlarıyla (`-runTests -batchmode -nographics`) mükemmel çalışır.*

### 3) SONSUZ/GENİŞLEYEN HARİTA (Chunking + Dirty Rects)

`RoomTemplateSO.bounds`'u tek bir devasa dizi yapmak yerine "Sonsuz Harita" için tek geçerli mimari **Sektör (Chunk)** tabanlı sistemdir.

*   **Mimari:** Haritayı `Dictionary<Vector2Int, MapChunk>` (örneğin 16x16 tile'lık parçalar) olarak yönet. Koordinat `(34, -10)` geldiğinde matematiksel olarak hangi chunk'a düştüğünü bul, o chunk bellekte yoksa Instantiate et (Dictionary'e ekle).
*   **Kamera/Grid Sınırı:** Kullanıcı sınırsız kaydırabilir. Görünen (Kamera Frustum'una giren) chunk'lar aktif edilir, diğerleri gizlenir veya RAM'den atılır.
*   **Dirty Rect Bağlantısı:** Bir alan güncellendiğinde (`dirty-rect`), sistem bu rect'in hangi chunk'lara denk geldiğini hesaplar ve sadece o chunk'ların mesh'ini/Tilemap'ini yeniden oluşturur. Bu performans için kritik.
*   **Risk:** Sınırsız büyüme Pathfinding (A*) algoritmasını felç edebilir. Navigasyon için "Hiyerarşik A*" veya chunk bazlı Node grafikleri kullanman gerekecek. Kayıt (Save) dosyasında sadece "içi boş olmayan (modified)" chunk'lar kaydedilmelidir.

### 4) TMP YAZI BOZULMASI (Dynamic Atlas)

*   **Mimari Açıdan Doğru Mu?** EVET. TextMeshPro'nun Dynamic Atlas sistemi, özellikle fallback fontlarla ve oyun sırasında anlık metin üretiminde (hasar sayıları, UI yazıları) kronik olarak UV/doku bozulmalarına (garbled text) yol açar.
*   **Çözüm:** Statik Atlas kullanmak, oyunda kullanılacak karakter setini (örneğin Latin + Türkçe karakterler) önceden bake edip kilitlemek tek kesin ve profesyonel çözümdür.
*   **Zamanlama (Şimdi mi?):** **AL (Şimdi yap).** Görsel bozulmalar kalite algısını çok hızlı düşürür. Yapılandırması 10 dakikalık bir iştir.

---

### EYLEM PLANI (AL / SONRA / ATLA)

*   **AL (Hemen Uygula):**
    *   **TMP Static Atlas Fix:** Hızlı kazanım, görsel kalite.
    *   **IMGUI Emekliliği ve uGUI/Core Konsolidasyonu:** F2 çakışmasını kökten çözmek ve tek "Gerçeklik Kaynağı (ILevelStore)" kurmak.
    *   **GlobalInputRegistry (Keybind Guard):** Çakışmaları çalışma zamanında hata vererek engelleyen yapıyı kurmak.
    *   **Roundtrip (Save/Load) EditMode Testi:** Konsolide edilen core'un veri kaybı yaşatmadığını kanıtlamak.
*   **SONRA (Planla):**
    *   **Sonsuz Harita (Chunking):** Core birleşimi bitmeden chunking'e girmek sistemi kırar. Önce mevcut bounds içinde core/uGUI birleşimini test et, sonra `ILevelStore` altındaki veri tutma yapısını Dictionary/Chunk modeline geçir.
    *   **PlayMode UI Overlap Testleri:** Önce manuel entegrasyonu bitir, kod stabilize olduktan sonra CI için bu testleri yaz.
*   **ATLA:**
    *   Geçici çözümler. (Örn: Eski F2 kodunun içine if(uGUI_acik_mi) yazmak). Eski kod yamasını atla, doğrudan silip birleştirme mimarisine geç.

