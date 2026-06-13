# RIMA Council — LEAN / SHIP-FAST / OVER-ENGINEERING CRITIQUE lens (Gemini 3.5 Flash High)

RIMA = 2D izometrik ARPG, Unity (URP 2D). Demo ~20 Haziran, IN-EDITOR sunum. Repoyu okuyabilirsin.
Senin lens'in: EN YALIN YOL + AŞIRI-MÜHENDİSLİK ELEŞTİRİSİ. "Demo'ya ne GERÇEKTEN lazım, neyi post-demo'ya at?" Diğer advisor'lar mimari kuracak — sen kes/sadeleştir.

READ (gerekirse): Assets/Scripts/UI/DirectorMode.cs (satır 140-200), Assets/Scripts/UI/BuildModeController.cs, Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs, Assets/Editor/Rooms/RoomTemplateBrowserWindow.cs, CURRENT_STATUS.md.

## 4 SORUN — her biri için EN YALIN demo-fix + neyi ERTELEMELİ

① GAME vs DIRECTOR MODE: DirectorMode.cs:143 `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` her sahnede otomatik açılıyor → normal play "editör modu gibi" (çerçeve+SPAWN). Demo'ya en hızlı temiz fix? (örn. tek satır gate: sahne adı `_Arena`/dev-define dışında bootstrap'ı atla; ya da PlayerPrefs flag). Aşırı çözümden kaçın.

② GERÇEK AKIŞ: Oyun MainMenu → CharacterSelect (chamber) → gameplay ile başlamalı, Director "ÖDÜL SEÇ" ile değil. En yalın yol: normal Play'i MainMenu'den başlat + Director'ı gate'le. Yeni sistem kurma — mevcut "Play From Main Menu" yolunu standart yap. Doğru mu?

③ F2 TILE HİZALAMA: grid overlay (iso diamond) ile floor tile oturmuyor. En olası tek-kök + en hızlı doğrulama (3 adım). Büyük refactor önerme.

④ EDİTÖR ↔ OYUN-İÇİ BİRLEŞTİRME + REFRESH: kullanıcı editör Map Designer/Room Browser ile oyun-içi Build Mode'un senkron + Refresh'li çalışmasını istiyor. DEMO İÇİN BU GEREKLİ Mİ? En yalın MVP ne (örn. sadece "Refresh = aktif RoomTemplateSO'yu yeniden yükle" tek buton), gerisi post-demo mu? Aşırı-mühendislik riskini söyle.

Çıktı: her sorun ≤3 satır, "demo-fix" vs "post-demo" net ayrımı. Kısa.
