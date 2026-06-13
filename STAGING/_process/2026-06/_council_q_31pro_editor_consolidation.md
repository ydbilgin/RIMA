# Council — ax Gemini 3.1 Pro (High) — DERIN MIMARI + TEST STRATEJISI lensi

LENS (sen=ax 3.1 Pro): derin mimari + test/regresyon stratejisi. Bu konsolidasyonu dogru ve TEKRAR-BOZULMAZ yapan tasarim.

## Durum: RIMA'da 3 ORTUSEN oyun/seviye editoru
1. ONCEDEN VAR: `Assets/Scripts/DevTools/InPlayMapPaintOverlay.cs` — F2, IMGUI (cirkin), self-bootstrap, Floor/Cliff/Prop tilemap boyar, `UnifiedDesignerCore` + JSON ile KAYDEDER (Editor ile ayni veri).
2. BENIM (yeni, eski'den habersiz): `Assets/Scripts/UI/BuildMode/*` — uGUI premium, prop+tile+walkability, overlap-hide (oyun UI gizler), undo, working-copy/no-pollution; kayit YOK; az once F2'ye de baglandi -> F2 CAKISMASI.
3. `RoomPainter/UnifiedDesignerCore.cs` + `Editor/MapDesigner/UnifiedMapDesigner.cs` — edit-mode editor, core'u paylasir.
Demo ~1 hafta, in-editor. Kullanici: tek GUZEL + SONSUZ-genisleyen-map + TEST-korumali editor istiyor; "tekrar bu sorunla (cakisma/keybind/overlap) karsilasmayalim".

## YANITLA (4)
1) KONSOLIDASYON MIMARISI: tek editore indir. Onerim: uGUI ON-YUZ (benim Build Mode) + `UnifiedDesignerCore` ARKA-UC + eski IMGUI F2 emekli. Bu dogru mu? Ana risk = uGUI front-end ile core'un veri/komut sozlesmesi. Hangi soyutlama sinirini onerirsin (front-end core'a nasil baglanir; eski editor-framework council'daki ISpaceMapper/IPlaceable/ILevelStore tohumlariyla nasil hizalanir)? Tek "EditorSession" / "tool registry" modeli?
2) REGRESYON-BOZULMAZLIK TEST STRATEJISI (kullanicinin #1 istegi): bu hata sinifini (ayni-tusa-iki-arac F2 cakismasi, keybind-layout, Build Mode acilmiyor, overlap-hide, placement, exclusivity/no-double-place, walkability, save/load roundtrip) yakalayacak otomatik test mimarisi. EditMode mi PlayMode mi, nasil yapilandirilir, `*ForValidation` data-proof'lari kalici teste nasil donusur? + "tek in-play tool/keybind kayit guard'i" (iki arac ayni tusu sahiplenemez) tasarimi. CI/headless calisabilir mi?
3) SONSUZ/GENISLEYEN MAP: RoomTemplateSO.bounds buyutme, runtime resize, chunked render, kamera/grid sinir — mimari yaklasim + risk + dirty-rect ile baglanti.
4) TMP yazi bozulmasi (kronik dynamic-atlas): kok fix (static atlas / pre-warm) mimari acidan dogru mu, simdi mi?

Cikti: konsolidasyon mimarisi + test stratejisi + AL/SONRA/ATLA.
