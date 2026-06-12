# RIMA — ChatGPT Görsel/Işık Review İsteği (2026-06-11)

ChatGPT repo'ya erişebiliyor (github.com/ydbilgin... RIMA). Aşağıdaki bağlam + ekteki screenshot klasörüyle review iste.

## Bu session ne değişti (ışık revizyonu + BG altyapısı)
Oda görsel-güzelleştirme FAZ1 (IŞIK) yapıldı + canlı doğrulandı.

**Bulunan ve düzeltilen kök-sorun (çift-global):**
- `_Arena` sahnesinde zaten bir **Global Light2D intensity 1.4 (beyaz)** vardı; oda profilleri buna **ek** 0.22 global ekliyordu → URP 2D'de toplanıyor → ambient ~1.6, beyaz hakim → "moody diorama" oluşmuyordu (sahne yıkanıyordu).
- **Fix:** sahne global'i **1.4 → 0.22 soğuk** (tek ambient kaynağı) · oda profillerinde global **0'landı** · zemin `RoomEnvironment_SpriteLit` (URP 2D lit) + "Floor" layer · ember/cyan point-light'lar.

**Player okunaklılığı (Hades tarzı):**
- Player **Sprite-Lit** olduğu için 0.22 ambient'te kararıyordu → Player.prefab'a **HeroLight2D** (Point, warm #FFD18C, intensity 1.2, radius 6, follow) eklendi → hero hep okunaklı + etrafında sıcak havuz.

**Ember pozisyon fix:** ember (0.18/0.82, 0.72) cross-odalarda void-köşeye düşüyordu → zemin-güvenli orta-banda (0.32/0.68, 0.5) taşındı, radius 3.5→5.5.

**NLM EXACT kanon (referans):** slate zemin #3A3D42 · void MOR #3A1A4A · cyan rift #00FFCC ≤%15 · ember #E89020 brazier · ambient 0.22 · random YASAK / anchored-detail / merkez-temiz.

**BG (kod hazır, default-OFF):** `PersistentBackgroundController` — oda teardown'undan bağımsız kalıcı parallax BG container (FAR/MID/FRONT + ParallaxLayer). Görsel henüz atanmadı (PixelLab pixel-native gelecek).

## İncelenecek repo dosyaları (ChatGPT için)
- `Assets/Scripts/MapDesigner/Room/Data/RoomLightingProfileSO.cs` (ışık profil veri modeli)
- `Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs` → `ApplyLighting()` (~656)
- `Assets/Data/Rooms/Lighting/RoomLightingProfile_*.asset` (Combat/Boss/Special değerleri)
- `Assets/Scripts/Background/PersistentBackgroundController.cs`

## Screenshot klasörü
`STAGING/review_2026-06-11/screenshots/` (eski demo akışı 01-37 = ESKİ ışık · `screenshot-20260611-152114/152427` = YENİ ışık before/after · ax'in ekleyeceği `current_*` = güncel-ışıklı tüm sahneler).

## Review SORULARI (ChatGPT'den net cevap iste)
1. **Işık landing mi?** Yeni screenshot'larda (152427 + current_*) moody-diorama + sıcak hero-pool + cyan cliff okunuyor mu? Canon'a (mor void / cyan rift ≤%15 / ember warm / ambient karanlık) uyuyor mu? Eksik ne?
2. **Oda güzelliği/kompozisyon:** Hangi oda hâlâ boş/düz duruyor? "Çerçeveli diorama" (kenar-yoğun, merkez-temiz, oda-başına-1-focal) için FAZ2 dekorasyonda öncelik ne olmalı?
3. **Hero follow-light** yaklaşımı doğru mu? intensity/radius/renk tuning önerisi?
4. **Sunum etkisi:** Jüriye en çok fark yaratacak **3 görsel iyileştirme** (kalan süre kısıtlı, demo-safe olmalı)?
5. **Teknik risk:** Işık kurulumunda çift-global dışında gözden kaçan risk (build'de Light2D, performans, sorting-layer) var mı?

## Kısıt
Karakter/mob materyalleri UNLIT kalmalı (siyah-render bug'ı). ISO/45°-diamond YOK. Wang painter demo-öncesi YOK. Full-screen video BG YOK (katmanlı parallax). Demo akış mantığına dokunma.
