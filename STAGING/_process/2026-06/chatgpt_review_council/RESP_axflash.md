# RIMA ChatGPT REV2 Planı - Lean / Ship-Fast Skeptic İncelemesi (ax Flash Lens)

## ⛔ 1. KES LİSTESİ (P0'dan Düşürülecekler)
*   **Director Mode Shell Redesign (P0 -> POST):** 6-10 saatlik bu kalem tamamen elenmeli. Bootstrap hatası (F2/" ölü) nedeniyle demo akışında erişilmesi bile garanti değil. Çalışmayan bir özelliğin dış kabuğuna 1 gün harcamak intihardır.
*   **Screenshot Duplicate Detector / Automated Capture-Harness (P0 -> POST):** Ekran görüntüsü otomasyonu ve QA araçları POST'a kalmalı. 4 eksik ekran görüntüsü (09, 19, 20, 21) kodla değil, elle tetiklenip tek seferde doğru klasöre kaydedilmeli.
*   **Shared-Component Prefab Sistemi (P0 -> POST):** Director panellerini (Spawn/Stats) yeni ortak prefab yapısına geçirmek yerine, editörden mor/turuncu çerçeve gameobject'lerini `Active = false` yapmak yeterlidir.

## ⏱️ 2. 1.5 GÜNDE KESİN YAPILACAKLAR (Toplam ~6.5 saat)
1.  **Low-HP Vignette Düzeltmesi (30-60 dk):** Ekranı kaplayan kırmızı wash yerine kenarlara hafif vignette ve pulse eklenmesi. Maliyeti en düşük, görsel etkisi en yüksek iş.
2.  **HUD Bar & Slot Resize (1.5 - 2 saat):** HP/resource barlarının ve yetenek slotlarının 1080p'de okunabilir hale getirilmesi (büyütme). Demo izleyicisi için kritik.
3.  **Boss Presentation & Shop Residue Cleanup (2 saat):** Boss odasındaki merchant kalıntılarının temizlenmesi, boss sprite'ının pivot/scale ayarı ve yeşil can barının crimson renge çekilmesi.
4.  **Black Blob & Rim-Light (1 saat):** Siyah boşluk gibi duran kapı ve düşman sprite'larına basit bir rim-light veya iç hat detayı verilmesi.
5.  **Draft Synergy Text (1 saat):** "Iron Charge ile eşleşir" gibi anlamsız metinlerin yerine net etki açıklayan 1-2 cümle eklenmesi.

## 🪤 3. EN BÜYÜK ZAMAN TUZAĞI (1 Kalem)
*   **Director Shell Redesign (Shared Prefab & Viewport %55 kuralı):** Kod tarafında sürükle-bırak UI düzenlemeleri, Unity UI Canvas anchor bozulmaları ve layout element çakışmaları nedeniyle en az 1 tam günü yutacaktır ve demo akışında görünmeme riski %90'dır.

## 💡 4. EĞER SADECE 1 GÜN OLSAYDI (ACİL DURUM PLANI)
1.  **Boss Anchor/Scale & Room Cleanup** (2 saat)
2.  **HUD HP/Resource Bar & Slot Resize** (2 saat)
3.  **Low HP Vignette Fix** (30 dk)
4.  **Eksik Ekran Görüntülerini Elle Yakalama** (30 dk)
*Geri kalan her şey (Build Mode UX, Director, Codex, Settings, Telemetry) dondurulur ve POST'a atılır.*
