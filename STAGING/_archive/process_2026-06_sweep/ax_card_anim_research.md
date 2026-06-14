Hazırladığım ayrıntılı raporu ve doğrudan Unity projenize kopyalayıp kullanabileceğiniz saf C# Coroutine tabanlı script şablonunu içeren **[card_juice_research.md](file:///C:/Users/ydbil/.gemini/antigravity-cli/brain/a735f3b7-b8f3-49e8-ab20-3737d605a534/card_juice_research.md)** dosyasını oluşturdum. 

Sorularınızın kısa, somut ve uygulanabilir cevapları aşağıdadır:

### (1) HOVER (Fare/Seçim Üstüne Gelince)
* **Scale-Up (Ölçek Büyütme):** 
  * *Nasıl:* Kartın `transform.localScale` değeri başlangıçtan hedefe interpolasyonla güncellenir.
  * *Değer Aralığı:* `1.0f` $\rightarrow$ `1.08f` (veya `1.10f`), `0.12s` sürede `EaseOutCubic` interpolasyonu.
* **Bring-to-Front (Öne Çıkarma):**
  * *Nasıl:* Karta geçici bir `Canvas` ve `GraphicRaycaster` bileşeni eklenip hover anında aktif edilir.
  * *Değer Aralığı:* `canvas.overrideSorting = true; canvas.sortingOrder = 10;` (Normal kartlar `0`'dadır).
* **Cyan Glow (Parlama):**
  * *Nasıl:* Kartın arkasındaki cyan (`#00FFFF`) renkli feathered görselin Alpha'sı ve Scale'i artırılır.
  * *Değer Aralığı:* Alpha: `0.0f` $\rightarrow$ `0.75f`. Glow Scale: `0.95f` $\rightarrow$ `1.12f`. Süre: `0.15s` (`EaseOutQuad`).
* **Hearthstone-Style Tilt (Eğilme & Parallax):**
  * *Nasıl:* Farenin kart merkezine göre konumu normalize edilip kart o yönde 3D döndürülür.
  * *Değer Aralığı:* Z ekseni sabit, X ve Y eksenlerinde maksimum `±7°`. Her karede `Quaternion.Slerp` ile `t = 12f * Time.deltaTime` yumuşatmasıyla takip.
* **Shadow (Gölge Kayması):**
  * *Nasıl:* Kartın gölge görselinin dikey pozisyonu aşağı kaydırılarak yükseklik hissi verilir.
  * *Değer Aralığı:* Gölge Y Offset: `-8f` $\rightarrow$ `-24f`. Gölge Alpha: `0.6f` $\rightarrow$ `0.3f` (kart yükseldikçe gölge silikleşir).

### (2) SELECT (Kart Seçilince)
* **Seçilen Kart (Büyüme + Merkeze Uçma):**
  * *Nasıl:* Kart önce `Anticipation` (büzülme) yapar, ardından ekranın merkezine (`Vector2.zero`) uçarak büyür.
  * *Değer Aralığı:* Önce `0.06s` boyunca `0.94f` scale'e büzülme (`EaseInQuad`). Ardından `0.35s` sürede merkeze hareket ve `1.25f` scale'e büyüme (`EaseOutBack`).
* **Seçilmeyen Kartlar (Kayma/Solma):**
  * *Nasıl:* Seçilmeyen kartlar yerçekimine kapılmış gibi ekranın altına doğru düşüp şeffaflaşır.
  * *Değer Aralığı:* Pozisyon Y: `Mevcut` $\rightarrow$ `Mevcut - 500f`. Alpha: `1.0f` $\rightarrow$ `0.0f`. Süre: `0.22s` (`EaseInCubic`). `interactable = false` ve `blocksRaycasts = false`.
* **Onay Efekti (Flash / Particle / Punch):**
  * *Nasıl:* Kart merkeze oturduğu an tüm ekranda kısa süreli cyan/beyaz panel yanıp söner ve karttan dışarı dairesel halka (Radial Wave) büyüyerek yayılır.
  * *Değer Aralığı:* Ekran Flash Alpha: `0.0f` $\rightarrow$ `0.6f` (0.04s) $\rightarrow$ `0.0f` (0.16s). Radial Wave Scale: `1.0f` $\rightarrow$ `3.0f`, Alpha: `1.0f` $\rightarrow$ `0.0f` (Süre: `0.30s`).

### (3) IDLE / GİRİŞ ANIMASYONLARI
* **Slide-in Stagger (Sıralı Giriş):**
  * *Nasıl:* Kartlar sahne dışından (aşağıdan) belirli gecikmelerle (stagger) yukarı fırlayarak gelir.
  * *Değer Aralığı:* Giriş Süresi: Kart başına `0.45s` (`EaseOutBack`). Gecikmeler: Kart 1 = `0.0s`, Kart 2 = `0.08s`, Kart 3 = `0.16s`. Başlangıç Y: `-800f` $\rightarrow$ Bitiş Y: `0f`.
* **Idle Floating / Bobbing (Yüzen Kartlar):**
  * *Nasıl:* Statikliği bozmak için her karta index tabanlı bir faz farkıyla (offset) sinüs dalgası uygulanır.
  * *Değer Aralığı:* Dikey Bobbing (Y): `±8` piksel (`Mathf.Sin(Time.time * 2.0f + phaseOffset)`). Rotasyon (Z): `±0.8°` (`Mathf.Cos(Time.time * 1.5f + phaseOffset)`). Faz Farkı: `index * 1.2f`.
* **Rarity Glow Pulse (Nadirliğe Göre Nabız):**
  * *Nasıl:* Nadirlik çerçeve ışığının parlaklığı yavaş bir sinüs dalgasıyla nefes alır.
  * *Değer Aralığı:* Glow Alpha: `0.2f` $\rightarrow$ `0.5f` (Frekans: `1.2f`).

### (4) GENEL JUICE PRENSİPLERİ
* **Timing (Zamanlama):** Giriş `0.45s`, Hover tepkisi anında olmalı (`0.10s - 0.12s`), Seçim/Onay `0.30s - 0.40s`.
* **Easing Tipleri:** 
  * `EaseOutCubic` $\rightarrow$ Hover ve pozisyon yumuşatmaları için (hızlı başlayıp yavaş durur).
  * `EaseOutBack` $\rightarrow$ Giriş fırlamaları ve merkeze uçma için (hedefi hafifçe aşar).
  * `EaseInQuad` $\rightarrow$ Anticipation (seçim öncesi geri çekilme) ve solmalar için.
* **Klavye/Gamepad Desteği:** uGUI Button bileşeninin `ISelectHandler` ve `IDeselectHandler` interface'leri dinlenerek hover coroutine'leri aynı şekilde tetiklenmelidir.

### (5) RIMA'NIN SKILLOFFERUI'Sİ İÇİN EN ETKİLİ 4 DOKUNUŞ (Öncelik Sırasıyla)
1. **Dinamik Kart Büyümesi & Sibling Canvas Sıralaması (Öncelik 1):** Hover anında kartın `1.08f` scale değerine çıkması ve `Canvas.sortingOrder` ile diğer kartların üzerine çizilmesi.
2. **Cyan Glow & Nabız Efekti (Öncelik 2):** RIMA'nın cyan seal-energy temasına uygun olarak, kart arkasındaki mühür ışığının idle durumunda yavaşça yanıp sönmesi (`Alpha: 0.15 - 0.35`), hover durumunda ise parlaması (`Alpha: 0.75`).
3. **3D Tilt (Eğilme) Efekti (Öncelik 3):** Fare hareketine göre kartın `RectTransform.localRotation` değerinin `Quaternion.Slerp` ile yumuşakça o yöne bükülmesi.
4. **Seçim Anında Anticipation ve Screen Flash (Öncelik 4):** Seçim onayında kartın önce büzülüp (`0.94f`) sonra merkeze fırlaması ve ekranın anlık cyan flash yapması.

*Kod şablonu, interpolasyon formülleri ve matematik detayları için **[card_juice_research.md](file:///C:/Users/ydbil/.gemini/antigravity-cli/brain/a735f3b7-b8f3-49e8-ab20-3737d605a534/card_juice_research.md)** dosyasını inceleyebilirsiniz.*

