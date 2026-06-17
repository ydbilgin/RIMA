# AX Flash Critique: Act-1 Room Capture Plan

## 1. En Basit / En Az Kırılgan Yol
Projede zaten editör modunda çalışan, `_Arena` sahnesini açıp `IsoRoomBuilder` ile odayı inşa eden ve sahne kamerasını odaklayıp PNG alan bir **`MapScreenshotTool`** (`Assets/Scripts/Editor/DevTools/MapScreenshotTool.cs`) bulunmaktadır. 
Act-1 odaları legacy `RoomLayoutJson` formatında olduğu için, en az kırılgan yol:
- Act-1 JSON'larını geçici bir editör script'i ile `RoomTemplateSO` asset'lerine dönüştürmek,
- Mevcut `MapScreenshotTool` aracıyla otomatik yakalamak.

## 2. Efor / Değer Dengesi (Demo Yarın)
- 6 odanın tamamını cliff-island olarak görselleştirmek yarınki demo için **gereksiz bir aşırı-mühendisliktir**.
- **Öneri:** 1 adet ana oda (örn. `act1_entry_hall`) için cliff-island görseli + 6 odanın tamamı için şematik 2D grid (JSON-to-tilemap verisi) yeterlidir. Bu kombinasyon akademik rapor anlatısı için fazlasıyla yeterlidir.

## 3. Yaklaşım Kritik (Kes-At)
- **Play + LiveRoomReloader:** BÜYÜK RİSK / OVER-ENGINEERING. `LiveRoomReloader` runtime bağımlılıkları barındırır, oynatıcı pozisyonlama gerektirir ve cliff tile güncellemeleri sınırlıdır (no-op).
- **Klon-sahne:** GEREKSİZ. Sahneyi kirletme korkusuyla yeni sahne üretmek konfigürasyon sapmasına yol açar.
- **Revert-capture (Editör Modu):** EN GÜVENLİSİ. `_Arena` sahnesini editörde aç, inşa et, ekran görüntüsü al ve sahneyi kaydetmeden kapat (revert). Zaten `MapScreenshotTool` bu no-leak mantığıyla çalışmaktadır.
