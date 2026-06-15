### Q1: stat→damage MATH & Canlı Integration
- **DamageCalculator MATH:** **Unit yeter.** Hasar hesaplama formülünün ve lineer artışın doğruluğu birim testlerle matematiksel olarak zaten kanıtlanmıştır.
- **LMB Attack to Enemy Hit:** **Canlı test ŞART.** [DirectorMode](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/DirectorMode.cs) stat slider değişiminin gerçek bir fiziksel LMB atağıyla düşman canını eksilttiğini ve hasar logunu tetiklediğini doğrulamak, zincirdeki tek canlı entegrasyon halkasıdır.

### Q2: G-collect & G-trigger Belirsizliği
- **G-collect (G-trigger):** **Canlı test ŞART.** [RewardPickup](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Core/RewardPickup.cs) içindeki `Keyboard.current[InteractKey].wasPressedThisFrame` girdisinin ve tetikleyici alanın (trigger) runtime'da taslak ekranını açtığını kanıtlamak, simüle klavye girdisiyle doğrulanması gereken tek girdisel belirsizliktir.

### Q3: Demo Sunumu ve Kanıt Yükümlülüğü
- **Sunum Yöntemi:** **Data-proof Rapor** (Console logları + telemetry CSV çıktısı + runtime durum dump).
- **Etkisi:** Video yokluğunda, görsel render veya animasyon akışlarının teyidine gerek kalmaz; odağı tamamen durum geçişleri ve loglanabilir verilerin doğruluğuna kaydırır.

---

### Item Değerlendirmeleri
1. **stat→damage (LMB Entegrasyonu):** **Canlı test ŞART.** Değişen stat verisinin fiziksel vuruş anında hasar paketine doğru yansıyıp yansımadığını kanıtlayan tek adımdır.
2. **G-collect (G-trigger):** **Canlı test ŞART.** Oyuncunun fiziksel etkileşim alanına girip butona bastığı andaki girdi kontrol akışını doğrulamak için gereklidir.
3. **Telemetry CSV:** **Unit yeter.** DPS/TTK hesaplama mantığı ve string oluşturma yapısı [DirectorMode](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/DirectorMode.cs) kodunda açıkça kanıtlandığı için canlı dosya yazımı marjinal değer üretir.

