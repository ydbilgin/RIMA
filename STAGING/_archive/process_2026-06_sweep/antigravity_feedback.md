# 🛸 Antigravity (Codex) System V2 Mimari & Kod Derin İnceleme Raporu

**Tarih**: 2026-05-25  
**Proje**: RIMA — 2D İzometrik ARPG  
**Dosya**: `STAGING/antigravity_feedback.md` (Derin C# Kod İncelemesi)  

---

## 1. Giriş

`Walls/V2` altındaki runtime C# dosyalarını (`WallChainRoomBuilder.cs`, `RoomSpec.cs`, `WallPiece.cs`, `WallPieceData.cs`, `WallPieceRegistry.cs`) satır satır, fonksiyon bazında inceledim. Kodun **Logic-First** yaklaşımı ve editor entegrasyonu temiz olsa da, arka planda derleme hatası vermeyen ancak **sahne renderında, fizikte ve görsel yerleşimde ciddi çökmelere, çakışmalara ve performans kayıplarına yol açacak 5 büyük algoritmik hata** tespit ettim. 

Aşağıda bu hataları C# kod referansları ve matematiksel modelleriyle birlikte detaylandırıyorum.

---

## 2. Kod Seviyesinde 5 Büyük Algoritmik Hata

### 🔴 BUG 1: Köşelerde 3 Prefab Üst Üste Yığılma Hatası (Triple Prefab Collision at Corners)
*   **Hatalı Kod Bölgesi**: `BuildRearChain` (satır 301), `BuildSideChain` (satır 315) ve `PlaceCornersAtJunctions` (satır 512).
*   **Hata Detayı**: `BuildRearChain` ve `BuildSideChain` algoritmaları, duvar zincirlerini `startX`'ten `endX`'e (veya `startY`'den `endY`'ye) **sınır hücrelerini hariç tutmadan** çizer. Yani köşedeki bir hücre (örneğin sol üst köşe olan `(0, 5)`) hem arka duvar (Rear) zinciri tarafından kaplanır hem de yan duvar (Side) zinciri tarafından kaplanır.
*   Üstüne üstlük, `PlaceCornersAtJunctions` fonksiyonu bu hücrede `openSides >= 2` tespit eder ve üzerine bir de `OuterCorner` prefabı spawn eder.
*   **Sonuç**: Köşe hücresinde **aynı koordinatta 3 farklı prefab** (Rear Wall, Side Wall ve Outer Corner) üst üste spawn edilir! Bu durum:
    1.  Sprireların üst üste binerek piksel titremesi (Z-fighting) yapmasına,
    2.  Fizik motorunda (`BoxCollider2D`) üst üste binen colliderların çakışmasına,
    3.  Gereksiz GameObject üretiminden ötürü performans israfına sebep olur.
*   **Kod Çözümü**: Duvar zincirlerini dolduran `FillRunWithSpans` fonksiyonu, zincirin başındaki (`start`) ve sonundaki (`end`) hücrelerin köşe hücresi olup olmadığını kontrol etmeli ve buralarda düz duvar spawn etmek yerine sadece köşe soketi bırakmalıdır.

---

### 🔴 BUG 2: Diamond (Baklava) Oda Algoritmasının Çarpık Kaması (Broken Diamond Footprint)
*   **Hatalı Kod Bölgesi**: `ComputeFootprint` içindeki `RoomShapeType.Diamond` case'i (satır 74-88).
*   **Hata Detayı**: Kodda diamond hücre üretimi şu şekilde kurgulanmış:
    ```csharp
    int rowWidth = Mathf.Clamp(spec.diamondTopWidthCells, 1, Mathf.Max(1, spec.widthCells));
    for (int row = 0; row < spec.heightCells; row++)
    {
        int y = spec.heightCells - 1 - row;
        if (row == spec.heightCells - 1 && spec.heightCells > 1)
            rowWidth = spec.widthCells;

        AddMirroredRow(set, spec.widthCells, y, rowWidth);
        int step = GetDiamondStep(spec, row);
        rowWidth = Mathf.Min(spec.widthCells, rowWidth + step * 2);
    }
    ```
*   Bu döngü `row = 0`'dan (en üst satır) başlar ve aşağı indikçe `rowWidth` değerini sürekli artırır (`rowWidth + step * 2`). Ancak **genişliği azaltan hiçbir mantık yoktur!**
*   **Sonuç**: Bu algoritma bir "Baklava" (Diamond) üretmez. Tepesi dar, tabanı geniş asimetrik bir **Yamuk (Trapezoid) veya Üçgen Kama** üretir. Gerçek bir diamond odanın orta satıra kadar genişlemesi, orta satırdan sonra ise daralması gerekir. Mevcut kod matematiksel olarak hatalıdır.
*   **Kod Çözümü**: Satır sayısı (`heightCells`) ikiye bölünmeli; döngü yarısına kadar `rowWidth`'i artırmalı, yarısından sonra ise aynı adım oranıyla azaltmalıdır.

---

### 🔴 BUG 3: Dar Koridorlarda Sahte Köşe İstilası (Spurious Corners in 1-Cell Corridors)
*   **Hatalı Kod Bölgesi**: `PlaceCornersAtJunctions` içindeki komşuluk testi (satır 512-527).
*   **Hata Detayı**: Kod, bir hücrenin etrafındaki void (boşluk) sayısına bakar:
    ```csharp
    int openSides = (n ? 0 : 1) + (s ? 0 : 1) + (e ? 0 : 1) + (w ? 0 : 1);
    if (openSides >= 2)
    {
        SpawnPiece(WallPieceType.OuterCorner, WallDirection.Any, GetCellWorld(c.x, c.y, horizontal: true));
    }
    ```
*   Eğer bir oda tasarımı içinde **1 hücre genişliğinde düz bir koridor** varsa, koridorun içindeki herhangi bir düz hücrenin (örneğin sağa giden koridor) kuzeyi boşluk, güneyi boşluktur. Bu durumda `openSides = 2` olur.
*   **Sonuç**: Algoritma dar koridorun içindeki **tüm düz hücreleri birer "Dış Köşe" (OuterCorner) olarak algılar** ve koridoru köşe prefablarıyla doldurur! Koridor çizgisi tamamen bozulur.
*   **Kod Çözümü**: `openSides >= 2` tek başına yetersizdir. Karşıt kenarların (N-S veya E-W) ikisinin birden açık olması durumu (düz koridor hattı) dışlanmalı, sadece komşu kenarların (L-şeklinde örn: N ve W) açık olduğu durumlar dış köşe olarak kabul edilmelidir.

---

### 🔴 BUG 4: Kısa Duvarlarda Geçersiz Konnektör Birleşimi (Connector Overlap in Short Walls)
*   **Hatalı Kod Bölgesi**: `FillRunWithSpans` (satır 395-448).
*   **Hata Detayı**: Kod, herhangi bir duvar koşusunun (`length`) başına ve sonuna birer `Connector` yerleştirir:
    ```csharp
    // Connector at start
    SpawnPiece(WallPieceType.Connector, WallDirection.Any, GetCellWorld(cursor, fixedCoord, horizontal));
    ...
    // Connector at end
    SpawnPiece(WallPieceType.Connector, WallDirection.Any, GetCellWorld(end, fixedCoord, horizontal));
    ```
*   Eğer duvardan dışa doğru 1 hücrelik bir çıkıntı veya 1-2 hücre uzunluğunda kısa bir duvar segmenti varsa:
    *   `length = 1` ise: Hem başlangıç hem bitiş konnektörü **aynı hücreye üst üste iki kez** spawn edilir!
    *   `length = 2` ise: Hücre 0 ve Hücre 1'e iki konnektör yan yana basılır, araya hiçbir duvar paneli gelmez.
*   **Sonuç**: Kısa bağlantı duvarlarında görseller üst üste biner, aradaki duvar paneli kaybolur ve kırık bir görüntü oluşur.
*   **Kod Çözümü**: `length <= 2` ise konnektör yerleştirme algoritması bypass edilmeli ve doğrudan 1x veya 2x düz duvar paneli yerleştirilmelidir.

---

### 🔴 BUG 5: Niche / Protrusion Köşe Konumlandırma Hatası (Niche Corner Floating Offset)
*   **Hatalı Kod Bölgesi**: `PlaceFormulaCorners` (satır 566-580).
*   **Hata Detayı**: Niş (niche) veya çıkıntıların köşelerini formülle yerleştirmeye çalışan kod:
    ```csharp
    int x = side == "right" ? spec.widthCells : 0;
    SpawnPiece(cornerType, WallDirection.Any, GetCellWorld(anchor, x, horizontal: false));
    ```
*   Burada `x` değeri sabit olarak odanın orijinal genişliği (`spec.widthCells`) alınır. Ancak nişler odaya doğru içeri girintilidir (derinliği `depth` kadardır).
*   **Sonuç**: Niş içeri göçmesine rağmen köşe parçası orijinal duvar hizasında (`spec.widthCells` koordinatında) havada asılı (floating) olarak spawn edilir! İçeri göçen duvar paneli ile köşe arasında boşluk kalır.
*   **Kod Çözümü**: `PlaceFormulaCorners` fonksiyonuna ilgili niş veya çıkıntının derinlik değeri (`depth`) parametre olarak geçilmeli ve `x` koordinatı `spec.widthCells - depth` (veya sol taraf için `0 + depth`) olarak ötelenmelidir.

---

## 3. Mimari Açıdan Ek Eksiklikler

### A) Soket Tabanlı Yerleşim vs Grid Tabanlı Hizalama Çelişkisi
`WallPieceData` içinde `leftSocketLocal` ve `rightSocketLocal` gibi soket tanımları bulunmasına rağmen, `WallChainRoomBuilder.cs` duvarları yerleştirirken bu soketleri **tamamen göz ardı etmektedir**. Yerleşim tamamen `cursor * cellSize` matematiksel grid adımına dayalıdır.
*   **Risk**: Eğer PixelLab'den gelen gerçek spriteların pivot noktaları veya genişlikleri tam olarak 1 unit (100 piksel) değilse, grid tabanlı yerleşim parçalar arasında açılmalara sebep olacaktır. Soket tabanlı zincirleme (snap chaining) kodunun devreye alınması şarttır.

### B) Çoklu Köşe Yönü (Rotation) Eksikliği
Kodda köşeler spawn edilirken yön değeri `WallDirection.Any` olarak paslanır:
```csharp
SpawnPiece(WallPieceType.OuterCorner, WallDirection.Any, ...);
```
İzometrik izdüşümde köşenin yönü (Kuzey, Güney, Doğu, Batı) görsel olarak tamamen farklı spritelar gerektirir. Tek bir prefabı yön tayin etmeden basmak, tüm köşelerin aynı yöne bakarak çarpık görünmesine yol açar.

---

## 4. Claude İçin Hazırlanmış Derin Dispatch Promptu

*Aşağıdaki metni kopyalayıp Claude (Orchestrator) sohbetine doğrudan yapıştırabilirsiniz:*

```markdown
/goal RIMA Wall System V2 - Algorithmic Deep Code Fixes (C# Priority 1)

Claude, Antigravity (Codex) `WallChainRoomBuilder.cs` dosyasını derinlemesine inceledi ve sistemin çalışmasını engelleyen/çarpıtan 5 kritik algoritmik hata tespit etti. Lütfen STAGING/antigravity_feedback.md belgesini oku ve Antigravity'ye (Codex) şu derin C# kod düzeltme görevlerini dispatch et:

1. BUG 1 FİX (Corner Duplication): FillRunWithSpans ve zincir kurucuları revize et; köşe hücrelerinde düz duvar spawn edilmesini engelle, buraları sadece Corner spawner'a bırak.
2. BUG 2 FİX (Diamond Wedge): ComputeFootprint içindeki Diamond algoritmasını iki aşamalı (ortaya kadar genişleyen, ortadan sonra daralan) gerçek baklava formülüne geçir.
3. BUG 3 FİX (Spurious Corridor Corners): PlaceCornersAtJunctions içindeki openSides testini güncelle; karşıt açık kenarları (N-S / E-W düz koridor) dışla, sadece L-köşe durumlarında OuterCorner yerleştir.
4. BUG 4 FİX (Short Wall Connector Overlap): FillRunWithSpans içine length <= 2 kontrolü ekle; konnektör basmak yerine doğrudan düz duvar yerleştirilmesini sağla.
5. BUG 5 FİX (Niche Corner Floating Offset): PlaceFormulaCorners yapısına derinlik (depth) parametresi ekle; köşe koordinatlarını niş/çıkıntı derinliğine göre içeri/dışarı ötele.
6. CORNER ROTATION: PlaceCornersAtJunctions içinde hangi kenarların boş olduğunu tespit ederek OuterCorner ve InnerCorner parçalarına doğru WallDirection parametresini ata.

Lütfen bu kritik C# düzeltmelerini onaylayıp CODEX_TASK.md'yi bu doğrultuda hazırla.
```
