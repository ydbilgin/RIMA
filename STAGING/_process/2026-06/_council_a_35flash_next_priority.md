Aşağıda, storyboard hedeflerinize ve kod analizine göre en yalın, hızlı ("ship-fast") aksiyon planı yer almaktadır.

---

### **Q1 & Q2: Teknik Bulgular**
* **Q1 (F1 Leak):** **Non-issue / Güvenli.** Storyboard, sızıntı barındıran legacy `RoomLoader` akışından kaçınır. [RoomRunDirector](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs)'daki [AdvanceTo](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L1785) $\rightarrow$ [BuildCurrentRoom](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L279) $\rightarrow$ [DestroyActiveReward](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L1728) yolu, eski ödülü bellekten temizleyerek güvenli bir geçiş sağlar.
* **Q2 (Centerpiece - F2 Toggle):** **Kablolanmış ve Hazır.** [BuildModeController](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/BuildModeController.cs) kamera zoom/follow kontrolünü devralır, çakışan UI canvas'larını gizler ve F2 tuşunu [InPlayToolKeyRegistry](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/BuildModeController.cs#L134) üzerinden tekelleştirir. 
  * *Tek Limitasyon:* [ExitBuildMode](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/BuildModeController.cs#L292) sırasında geçici `WorkingTemplate` yok edildiğinden yerleştirilen prop sadece sahne üzerinde GameObject olarak kalır (Asset'e kaydedilmez). Video akışı için (aynı odada devam eden combat testi) bu durum tamamen yeterlidir.

---

### **Q3: Prioritizasyon Listesi (20 Haz Öncesi En Yüksek Kaldıraç)**

1. **Build Mode (F2) & Placement Doğrulama (Centerpiece):** Kamera rig (URP/Legacy PixelPerfectCamera + CameraZoom + CameraFollow) devre dışı kalma/geri gelme geçiş pürüzsüzlüğünü ve prop yerleştirme tetikleyicilerini test etmek.
2. **DirectorMode Stat $\rightarrow$ Damage (physPower) Doğrulama:** basic-attack LMB (sol tık) hasarının `physPower` slider değişiminde (50'den 250'ye çekerken hasarın 10'dan 50'ye çıkması) ölçeklendiğini doğrulamak.
3. **Telemetry CSV Export Doğrulama:** [DirectorMode](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/DirectorMode.cs)'daki [ExportTelemetryCsv](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/DirectorMode.cs#L2703) butonunun CSV verisini işletim sistemi panosuna (`GUIUtility.systemCopyBuffer`) hatasız kopyaladığını doğrulamak.
4. **F1 Golden-Path Akış Teyidi:** Combat odasını temizledikten sonra kapıdan geçişin console'da herhangi bir hata fırlatmadığından emin olmak.
5. **UI Fix Hazırlıkları (Kod Tarafı):** Canvas hizalamalarını koddan hazırlayıp, görsel teyidi kullanıcıya bırakmak.

---

### **Q4: SKIP Listesi (Aşırı Mühendislik / Pas Geçilecekler)**

* **SKIP: Runtime'da template verisini diske kaydetmek:** Build Mode'dan çıkınca template klonunun silinmesi ve sadece sahnedeki prop objelerinin kalması video için yeterlidir. Kalıcı kaydetme/JSON serialize sistemleriyle vakit kaybetmeyin.
* **SKIP: Q/E/R/F yeteneklerinin stat ölçeklendirmesini düzeltmek:** Yetenekler tasarımsal olarak stat ölçeklemesine sağırdır (`bypassStatScaling: true`). Koreografide **sadece LMB basic attack** kullanarak hasar değişimini gösterin.
* **SKIP: Sonraki odaya geçişte yerleştirilen prop'ların leak etmesi:** Placed proplar root GameObject olarak yaratılır ve yeni oda kurulurken temizlenmez. Video sonraki oda yüklenir yüklenmez donup biteceği için bu bellek sızıntısını göz ardı edin.
* **SKIP: CSV'yi fiziksel dosyaya yazdırma:** Clipboard kopyalaması (`systemCopyBuffer`) çalıştığı sürece Excel/VSCode'a doğrudan yapıştırma (Ctrl+V) yapabilmek sunum için daha pratiktir, dosya gezgini geliştirmesi yapmayın.

