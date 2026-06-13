I will start by listing the root directory to find `ANTIGRAVITY.md` and check the project structure.
I will view the `RULES.md` file to see if there are any specific guidelines for me.
I will view the `ANTIGRAVITY.md` file to see if there are any specific guidelines directed at me (Antigravity).
I will view the `CURRENT_STATUS.md` file to understand the current progress, structure, and active state of the project.
I will search for occurrences of `InPlayMapPaintOverlay` in the workspace to locate the file and understand its role.
I will search for scripts related to "BuildMode" or "Build" in the Assets directory.
I will read `BuildModeController.cs` to see how it binds to F2 or toggles build mode.
I will view the beginning of `InPlayMapPaintOverlay.cs` to see its key bindings and initialization.
I will search for `UnifiedDesignerCore` and `UnifiedMapDesigner` files in the workspace.
I will view the `UnifiedDesignerCore.cs` file to see how the editor backend functions and how it can be reused.
I will search for `RoomTemplateSO` references to see how it relates to `RoomData`.
I will search for the definition of `RoomTemplateSO` to see its fields and how it relates to `RoomData`.
I will view `RoomTemplateSO.cs` to understand the data model of RIMA's room templates.
I will search for `RoomData` definition to see what it is.
I will view the contents of `RoomData.cs` to see its fields.
I will read the council file `_council_q_35flash_editor_consolidation.md` to see the discussion from the 3.5 Flash perspective.
I will list the contents of the `STAGING/_process/2026-06/` directory to see other council notes and proposals.
I will view the `_council_a_31pro_editor_consolidation.md` file to see the analysis from the Gemini 3.1 Pro model.
I will search for `WorkingTemplate` in the BuildMode scripts to see how the placement controllers interact with it.
### 1) KONSOLİDASYON: En Yalın Yol
* **Pro/3.1'in Önerisi (Aşırı Mühendislik Tuzağı):** uGUI ön-yüzü ile `UnifiedDesignerCore` arka-yüzünü evlendirmek; araya `ICommand`, `ITool`, `GlobalInputRegistry` gibi katmanlar eklemek.
* **Flash/Yalın Eleştiri:** Bu yaklaşım net bir **erken soyutlama (early abstraction) tuzağıdır**. `UnifiedDesignerCore` arkada `RoomData` veri modelini kullanırken, uGUI Build Mode halihazırda `RoomTemplateSO` modelinin klonunu (`WorkingTemplate`) mutasyona uğratarak çalışıyor. Demo öncesi 1 haftada iki farklı veri modelini birleştirmeye çalışmak, çalışan demo kodunu çöp etme riski taşır.
* **En Yalın/En Ucuz Yol:** 
  1. **IMGUI Editörü Emekli Et:** `InPlayMapPaintOverlay.cs` içindeki `Bootstrap()` metodunu ve F2 dinleyicisini kaldır/devre dışı bırak. F2 tuşunu tamamen uGUI `BuildModeController`'a teslim et. Çakışma anında çözülür.
  2. **Mevcut Veri Akışını Koru:** uGUI Build Mode'un klon `RoomTemplateSO` (`WorkingTemplate`) üzerinden çalışmasına dokunma.
  3. **Yalın Kayıt Butonu:** `BuildModeController` içine sadece Editor-mode'da çalışan 5 satırlık bir "Save" fonksiyonu yaz. Çalışma kopyasını kaynak SO'ya kopyalasın:
     ```csharp
     #if UNITY_EDITOR
     UnityEditor.EditorUtility.CopySerialized(WorkingTemplate, sourceTemplate);
     UnityEditor.EditorUtility.SetDirty(sourceTemplate);
     UnityEditor.AssetDatabase.SaveAssets();
     #endif
     ```

---

### 2) TEST: Regresyon Önleyici Minimum Test Seti (5 Test)
Kullanıcının "tekrar olmasın" isteği için devasa bir test framework'ü yazmak yerine, **EditMode** altında çalışan ve saniyeler süren şu 5 test tüm hata sınıflarını yakalar:
1. **Keybind Registry Test:** Projede F2 ve Quote tuşlarını dinleyen tek aktif/enabled bileşenin `BuildModeController` olduğunu doğrula. (Keybind çakışmasını önler).
2. **Save/Load Roundtrip Test:** Bir `RoomTemplateSO` klonla, grid üzerinde bir tile boya, Save butonunun kullandığı kopyalama mantığını çağır ve kaynak SO'nun başarıyla güncellendiğini doğrula. (Kayıt kaybını önler).
3. **UI Overlap Test:** `BuildModeController.IsActive` true olduğunda, oyun içi diğer tüm ana Canvas'ların `enabled == false` olduğunu ve sadece Build Mode Canvas'ının açık olduğunu doğrula. (Görsel üst üste binmeyi önler).
4. **Tile/Walkability Consistency Test:** Grid hücresi "unwalkable" yapıldığında pathfinding otoritesinin `false` döndürdüğünü doğrula. (Zemin-lojik çakışmasını önler).
5. **Prop Bounds Test:** Bir prop'un oda sınırları dışına yerleştirilmeye çalışıldığında placement controller'ın bunu reddettiğini doğrula. (Placement hatalarını önler).

---

### 3) SONSUZ HARİTA: Büyük ama Sınırlı
* **Flash/Yalın Eleştiri:** Chunking, Dictionary tabanlı koordinat haritalama, dynamic loading ve dirty-rect mesh re-generation gibi yapılar demo öncesinde vakit kaybıdır.
* **Yalın Yol:** "Sonsuz" yerine **"Büyük ama Sabit Sınırlı"** (örn. 80x80 veya 100x100) bir harita alanı demo için fazlasıyla yeterlidir. Eğer çok gerekirse, 1D array'leri (`walkableGrid`, `overlayMask`) genişleten/kırpan basit bir "Resize Grid" butonu ekle. Chunking yapısını **ATLA**.

---

### 4) TMP YAZI BOZULMASI: En Ucuz Çözüm
* **Yalın Yol:** TextMeshPro dynamic atlas'ın `timeScale = 0` (pause) veya kamera zoom geçişlerinde UV bozma kronik hatasına karşı en ucuz ve kesin çözüm **Static Atlas**'tır. Oyunda kullanılan karakter setini (ASCII + Türkçe karakterler: ç, ğ, ı, ö, ş, ü vb.) Font Asset Creator ile bake edip kilitle. 2 dakikalık iş, sıfır kod, %100 kesin çözüm. **AL (Şimdi)**.

---

### 🛠️ Eylem Planı (AL / SONRA / ATLA)

* **AL (Şimdi):**
  * **TMP Static Atlas:** 2 dakikada bake et, görsel bozulmayı hemen çöz.
  * **IMGUI F2 Devre Dışı Bırak:** `InPlayMapPaintOverlay` bootstrap ve F2 dinlemeyi kapat.
  * **uGUI Build Mode'a Yalın Save:** `CopySerialized` ile çalışan SO klonunu aslına yazan butonu ekle.
  * **5 Maddelik Minimum Test Seti:** EditMode/PlayMode regresyon testlerini yaz.
* **SONRA (Demo Sonrası):**
  * **UnifiedDesignerCore Entegrasyonu:** Edit-mode ile Play-mode veri modellerini (RoomData vs RoomTemplateSO) ortaklaştırma işini demo sonrasına bırak.
* **ATLA (Aşırı Mühendislik):**
  * **Sonsuz Harita (Chunking/Dictionary):** Demo için büyük ama sınırları belirli grid yeterli.
  * **GlobalInputRegistry / ICommand Framework:** Sadece F2 çakışması ve tekil Undo yığını için jenerik framework yazma.

---

### Risk Sıralaması (Çalışanı Bozma Açısından)
1. **uGUI ve Core Entegrasyonu (En Yüksek Risk):** Farklı veri yapılarını zorla birleştirmek tüm boyama araçlarını bozabilir. *(Mitigation: Yalın Save ile atlatıldı).*
2. **IMGUI Kaldırma (Orta-Yüksek Risk):** Editör pencerelerinin `InPlayMapPaintOverlay` bağımlılığı varsa compile error oluşabilir. *(Mitigation: Dosyayı silmek yerine sadece in-game bootstrap'ini devredışı bırak).*
3. **TMP Static Atlas Değişikliği (Çok Düşük Risk):** Sadece görsel asset ayarıdır, kod etkilenmez.
4. **Minimum Test Eklenmesi (Sıfır Risk):** Mevcut runtime koduna dokunmayan, izole test sınıflarıdır.

