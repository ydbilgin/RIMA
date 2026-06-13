RIMA in-game seviye editörü framework'ü için **yalın, solo-dev odaklı ve aşırı-mühendislik karşıtı** süzgeç çıktısı aşağıdadır:

---

### 1) Mevcut/Önerilen Dışında Değer/Efor Oranı Yüksek Özellikler

*   **Ghost Shadow (İzometrik Yükseklik İllüzyonu)**
    *   **Açıklama:** Yerleştirilmekte olan "Ghost" objesinin tam altına, zemin iz düşümüne dinamik bir gölge dairesi veya dikey hiza çizgisi çizilmesi.
    *   **Efor:** **S** (Basit bir gölge sprite'ının Y pozisyonunu zemin hücresine eşitlemek).
    *   **Neden Yüksek Değer:** 2D İzometrik oyunlardaki en büyük derinlik algısı yanılgısını ("Bu taş havada mı duruyor, yerde mi?") sıfır karmaşıklıkla çözer.
*   **Visual Anchor / Pivot Helper (Hizalama Noktası)**
    *   **Açıklama:** Sprite'ların Unity import pivot ayarlarındaki ufak kaymaları göstermek adına, aktif Ghost üzerinde pivot noktasını belirten küçük bir kırmızı artı (+) işareti.
    *   **Efor:** **S** (Ghost altında ufak bir UI/Gizmo işareti).
    *   **Neden Yüksek Değer:** Prop yerleştirirken "neden bu sprite hücrenin ortasına değil de biraz soluna oturdu?" sorusuyla başlayan debug döngüsünü tamamen keser.
*   **Reference Image Overlay (Trace Map / Altlık Şablonu)**
    *   **Açıklama:** Editör arkasına veya önüne yarı şeffaf bir seviye taslağı (konsept çizim / eskiz) yükleyip, onun üzerinden çizim yapabilme.
    *   **Efor:** **S** (Basit bir UI RawImage alpha slider veya şeffaf bir SpriteRenderer).
    *   **Neden Yüksek Değer:** Konsept tasarımın oyuna tam ölçekli ve hızlı aktarımını sağlar; solo-dev için çizim süresini yarı yarıya indirir.

---

### 2) Aşırı Mühendislik (Over-engineering) Riski & Tuzak Tespiti

*   **Portability Soyutlaması (5 arayüz + adapter): Erken Soyutlama Tuzağı (Early Abstraction Trap)!**
    *   *Eleştiri:* `IGridSpace`, `IAssetCatalog`, `IPlacementValidator`, `ILevelStore`, `IPlaceable` soyutlamalarını henüz tek bir oyunda (`RIMA`) rüştünü ispatlamadan yazmak **büyük bir hatadır**. Kod yazılırken bu arayüzlerin imzaları 10 kez değişecektir.
    *   *Çözüm:* Kod doğrudan RIMA sınıflarına sıkı sıkıya bağlı (tightly coupled) yazılmalıdır. Editör bitip standalone paket aşamasına geçildiğinde (en son fazda) arayüzler koddan ayıklanmalıdır (refactor).
*   **Theming & Hotkey Config (Tema ve Kısayol Özelleştirme): Zaman Çöpü.**
    *   *Eleştiri:* Kendi editörünüz için özelleştirilebilir kısayol sistemi veya Light/Dark mode temaları yazmak solo geliştirici için tuzaktır. Kısayolları hardcode edin (W/E/R, Ctrl+Z, Delete), koyu temayı kilitleyin.
*   **Validation/Lint (Gelişmiş Kural Denetimi): Erken Aşama Fazlalığı.**
    *   *Eleştiri:* "Burada 2 kapı yan yana", "şu oda çok boş" gibi lint kuralları kurmak oyun mekanikleri oturmadan anlamsızdır. Sadece [WalkabilityMap.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Environment/WalkabilityMap.cs) üzerinden geçebilen `IsReachableFromPlayer` (çözülebilirlik kontrolü) yeterlidir.
*   **Layers (Katman Editörü): Gereksiz UI Yükü.**
    *   *Eleştiri:* Photoshop tarzı katman gizleme, kilitleme ve sıralama yönetimi in-game editör için aşırı kaçar. İzometrik derinlik sıralaması (`IsoSorter`) ve zemin/overlay Tilemap ayrımı tüm ihtiyaçları karşılar.

---

### 3) En Yüksek "Wow/Efor" 3 Özellik (Demo + Reusable)

1.   **Play-Test Toggle (Anında Test):**
     *   *Neden:* `"` tuşuna basınca kameranın geri çekilip oyunun durması; tekrar basınca karakterin kaldığı yerden anında devam etmesi. Editör-oyun geçişi arasındaki zaman boşluğunu sıfıra indirir.
     *   *Efor:* **M** (Kamera zoom geçişi + Input kilidi + pause toggle).
2.   **Procedural Stamp / Scatter Brush (Rastgele Dağıtıcı):**
     *   *Neden:* Çakıl taşı, çimen veya ağaç yerleştirirken 100 kere tıklamak yerine, fırça gibi sürükleyerek küçük varyasyonlar (rastgele açı/ölçek) ile doğal alanlar oluşturmak.
     *   *Efor:* **S/M** (Mouse drag sırasında radius-randomizer ile spawn).
3.   **Clipboard JSON Export/Import (Pano ile Seviye Paylaşımı):**
     *   *Neden:* Seviye verisinin tek tıkla kopyalanıp Discord'dan başkasına gönderilmesi ve onun da in-game tek tıkla yapıştırıp oynayabilmesi. Tasarım paylaşımını aşırı kolaylaştırır.
     *   *Efor:* **S** (`JsonUtility` + `GUIUtility.systemCopyBuffer`).

---

### 4) AL / SONRA / ATLA Çağrıları

#### 🟢 AL (Şimdi / En Yüksek Değer)
*   **Play-Test Toggle:** Demo için en kritik görsel/fonksiyonel geçiş.
*   **Clipboard JSON Export/Import:** Kayıt sistemini hem test edilebilir kılar hem de en ucuz persistence yoludur.
*   **Pivot/Shadow Helpers:** Yerleştirmedeki izometrik konumlandırma hatalarını başlamadan bitirir.

#### 🟡 SONRA (Vakit Kalırsa / Post-Demo)
*   **Eyedropper (Damlalık):** Sahnedeki bir nesneyi kopyalayıp fırçaya alma (hız için değerlidir ancak ilk başta palet de yeterlidir).
*   **Scatter Brush:** Çevre detaylarını doldurma hızını 10 kat artırır.
*   **Ambient Lighting (Işık Yerleştirme):** Atmosfer için harika bir wow efektidir ancak oynanışa doğrudan etkisi düşüktür.

#### 🔴 ATLA (Aşırı Mühendislik / Hiçbir Zaman)
*   **Arayüz Soyutlamaları (Erken Aşama):** [DirectorMode.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/DirectorMode.cs) ve [IsoRoomBuilder.cs](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs) doğrudan birbirini tanımalıdır. Arayüzler en son paket aşamasına saklanmalı.
*   **Hotkey Config & UI Theming:** Solo projede tamamen zaman kaybı.
*   **Katman Editörü (Layers):** Sprite'ların Y-sıralaması oyun tarafından çözüldüğü için ekstra katman yönetimine gerek yoktur.
*   **Görsel Undo History Paneli:** Ctrl+Z arkada çalışsın, geçmiş listesi çizen bir UI paneline gerek yok.

---
### Yapılan Çalışma Özeti
*   [CURRENT_STATUS.md](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/CURRENT_STATUS.md) ve [INGAME_BUILD_MODE_DESIGN_2026-06-13.md](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/STAGING/INGAME_BUILD_MODE_DESIGN_2026-06-13.md) incelenerek mevcut mimari analiz edildi.
*   Yalın geliştirme ve "Solo-dev" gözlüğüyle sistemdeki aşırı mühendislik tuzakları elendi.
*   Görsel wow etkisini en hızlı verecek 3 kritik özellik belirlenerek faz önceliklendirmesi (AL/SONRA/ATLA) yapıldı.

