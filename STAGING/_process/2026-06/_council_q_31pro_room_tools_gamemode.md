# RIMA Council — DEEP / ARCHITECTURE / DESIGN lens (Gemini 3.1 Pro)

RIMA = 2D izometrik ARPG, Unity (URP 2D). Demo ~20 Haziran, IN-EDITOR sunum. Repoyu okuyabilirsin (read tool).
Senin lens'in: DERİN MİMARİ / TASARIM yargısı. Uzun-vadeli temizlik + doğru soyutlama. Ama demo-risk-aware kal.

READ: Assets/Scripts/UI/DirectorMode.cs, Assets/Scripts/UI/BuildModeController.cs + Assets/Scripts/UI/BuildMode/*, Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs, Assets/Editor/Rooms/RoomTemplateBrowserWindow.cs, Assets/Scripts/MapDesigner/Room/{Runtime/IsoRoomBuilder.cs,Runtime/RoomRunDirector.cs,Runtime/RoomDecorationPass.cs,Data/RoomTemplateSO.cs}, CURRENT_STATUS.md.

## KONU ①+② — GAME MODE vs DEV/DIRECTOR MODE AYRIMI (demo-kritik)
`DirectorMode.cs:143` `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` HER sahnede otomatik kuruyor → normal play'de turuncu çerçeve + "SPAWN" paneli görünüyor, "oyun değil editör modu gibi". Oyun başlayınca Director "ÖDÜL SEÇ" geliyor; olması gereken: MainMenu → CharacterSelect (chamber) → temiz gameplay. RIMA menüde "Play Arena (F5)" (dev) + "Play From Main Menu" (gerçek) var.
SORU: Director Mode'u dev-only'a temiz ayırmanın EN DOĞRU mimari yolu? (a) bootstrap'ı koşullama (dev define / EditorPrefs / sahne-adı / launcher-flag), (b) tam explicit-launch. Demo Director'a bağımlı — bozma. "Game session başlatma" sorumluluğunu nerede merkezîleştirmeli? Risk + öneri.

## KONU ④ — EDİTÖR ↔ OYUN-İÇİ ODA ARAÇLARI BİRLEŞTİRME + REFRESH (en büyük, mimari ÇEKİRDEK SORU)
Editör: `RIMA/Map Designer` (UnifiedMapDesigner, "Build in Arena" → RoomTemplateBuildUtility.BuildInArena → `_Arena` sahnesi) + `RIMA/Room Browser` (RoomTemplateBrowserWindow). Oyun-içi: Build Mode (F2) RoomTemplateSO working-copy düzenleyip Save ediyor. Pipeline: RoomTemplateSO → IsoRoomBuilder + RoomDecorationPass (auto-prop).
Kullanıcı isteği: editör araçları + oyun-içi Build Mode EŞ-ZAMANLI; editörde kurulan/değişen oda oyun-içi listede görünsün, tersi de; "Refresh" butonu.
SORU: En temiz mimari? Tek-gerçek-kaynak = RoomTemplateSO asset'leri; her iki taraf aynı kataloğu mu okusun (AssetDatabase editör / Resources|Addressables runtime)? Refresh = AssetDatabase.Refresh + katalog re-scan + açık working-copy reconcile mı? Runtime Save → editör senkron (domain reload / FileSystemWatcher / manuel)? Consolidation'ın planladığı LevelEditor framework (IAssetCatalog/ILevelStore/ISpaceMapper) bu birleştirmeye nasıl hizmet eder — şimdi mi devreye alınmalı yoksa post-demo mu? Demo'ya sığar mı? Net MVP kapsamı + faz öner.

## KONU ③ — F2 TILE HİZALAMA (kısa)
Grid overlay (iso diamond, neighbour-midpoint) ile gerçek floor tile'ları oturmuyor. §3.5: CellLayout=Isometric, CellSize ~0.96x0.585, floor 64x64 @ PPU32, yerleştirme HER ZAMAN Grid API. En olası kök (cell-size vs pivot/PPU, overlay midpoint vs cell-center, anchor offset)? En hızlı doğrulama.

Çıktı: her konu için net karar/öneri, ≤ kısa. Demo-risk-aware.
