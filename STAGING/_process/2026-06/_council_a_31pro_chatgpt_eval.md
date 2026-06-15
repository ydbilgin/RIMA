**Q1 (Bulgu Doğrulama & Demo Riski - Mimari Muhakeme)**
*   **003 (Build Mode modal'ı gizler):** DOĞRULANDI (`BuildModeController.cs` `HideOtherUiCanvases`). Demo'da TETİKLENMEZ. Koreografide draft/pause açıkken F2'ye basılmayacak.
*   **006 (Reward 90s-guard timeout):** DOĞRULANDI (`RewardPickup.cs` `DraftThenOpenExit`). Demo'da TETİKLENMEZ. Rehearsed bir sunumda 90 saniye ekranda beklenmez.
*   **007 (Opening-draft coroutine restart leak):** DOĞRULANDI (`RoomRunDirector.cs` `BeginRun`). Demo'da TETİKLENMEZ. Scripted akışta opening draft ortasında same-scene restart yapılmaz.
*   **008 (Build hotkey text-input yarışması):** DOĞRULANDI (`BuildPlacementController.cs` `HandleKeyboard`). **GERÇEK RİSK.** Sunucu Build Mode'da arama alanına "fence" yazarken F ve E tuşları room objelerini bozacaktır.
*   **SUSPECTED 002 (Director full-flow bootstrap):** Demo'da TETİKLENMEZ. Demo `_Arena`-direct başlatılacağı için (`DEMO_BUGSWEEP_SYNTHESIS` Madde A), MainMenu'den geçmeme sorunu tamamen by-pass edilmiş olur.
*   **SUSPECTED 004 (Draft serialization):** Demo'da TETİKLENMEZ. Room-clear ile reward draft'in aynı anda yarışması scripted path'te yaşanmayacak.

**Q2 (SCOPE - EN KRİTİK)**
**Karar:** **(c) choreograph+ucuz-fix+F12-panic**
**Gerekçe:** ChatGPT'nin GameTimeCoordinator (arbiter), draft-serialization ve 4-state machine tespitleri uzun vadeli mimari doğruluğa sahip olsa da; 5 gün kalmış, centerpiece'i kanıtlanmış ve rehearsed koşulacak bir demo için devasa bir regression riski taşır. Çalışan "golden-path"i kökünden değiştirmek, demo öncesi kabul edilemez bir hamledir. 
**Ayrım:**
*   **Demo İçin Zorunlu:** Yalnızca golden-path'i kırabilecek softlock'lar (çoklu-take/restart'ta patlayan event leak'leri) ve sunucunun canlıda tetikleyebileceği input çakışmaları (text input hotkey).
*   **Post-Demo:** Mimari state mülkiyeti (RIMA-001 Time-scale), serialization (RIMA-004), BuildMode FSM (RIMA-009). Bu açıklar demo sırasında "Draft açıkken F2 basma" disipliniyle (koreografi) yönetilecektir. F12 panic butonu fallback olarak elde tutulmalıdır.

**Q3 (Final Cerrahi Fix Listesi)**
*   **[YAP]** `DraftManager.cs:113-114` - `OnSecondaryClassSelected` anonim lambdasını named method'a çevir, `OnDisable/OnDestroy`'da unsubscribe et (Çoklu-take softlock'unu kesin çöz).
*   **[YAP]** `BuildPlacementController.cs` (HandleKeyboard içi) - Tuş event'lerinin en başına input field focus kontrolü (`TMP_InputField.isFocused` vb.) ekle (RIMA-008 riskini kapat).
*   **[YAP]** `DirectorMode.cs:658` - `hasCameraTarget` bayrağını oda değişimlerinde resetle (Kameranın void'e uçmasını engelle).
*   **[YAP]** `DraftManager.cs:195` (`ShowDraft`) - Fonksiyon başına `if (IsDraftActive) return;` ekle (Ucuz defansif guard).
*   **[ERTELEME / POST-DEMO]** RIMA-001 (GameTimeCoordinator), RIMA-002 (Director Bootstrap), RIMA-004 (DraftRequest kuyruğu), RIMA-006 (Reward timeout policy), RIMA-009 (BuildMode 4-state machine).
*   **[KOREOGRAFİ]** Demo `_Arena` sahnesinden direkt başlatılacak. MaxHP slider'ına dokunulmayacak. Menu/Draft açıkken F2 kullanılmayacak.

