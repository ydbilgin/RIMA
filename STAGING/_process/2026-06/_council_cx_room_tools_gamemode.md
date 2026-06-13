ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA editör/oyun-içi oda araçları + game/dev mode ayrımı + F2 tile hizalama için FEASIBILITY / REUSE lens'iyle çözüm önerisi. ANALİZ ONLY, kod DEĞİŞTİRME. Sonucu CODEX_DONE.md'ye yaz, ≤120 satır.

# Lens: KOD / FEASIBILITY / REUSE
Repoda NE ZATEN VAR, nasıl yeniden kullanılır, en az-kod yol nedir? Sıfırdan yazma önerme — mevcut sınıfları işaret et.

READ these files first:
- Assets/Scripts/UI/DirectorMode.cs (özellikle satır 140-200, Bootstrap + RuntimeInitializeOnLoadMethod)
- Assets/Scripts/UI/BuildModeController.cs
- Assets/Scripts/Editor/MapDesigner/UnifiedMapDesigner.cs (Build in Arena akışı)
- Assets/Editor/Rooms/RoomTemplateBrowserWindow.cs
- Assets/Scripts/MapDesigner/Room/Runtime/IsoRoomBuilder.cs + RoomRunDirector.cs + RoomDecorationPass.cs
- Assets/Scripts/MapDesigner/Room/Data/RoomTemplateSO.cs
- RoomTemplateBuildUtility (grep ile bul)
- Assets/Scenes/UI/MainMenu.unity, CharacterSelect.unity akışı (CharacterSelectScreen.cs / ChamberSelectBootstrap.cs)

## SORULAR
① GAME vs DIRECTOR MODE AYRIMI: DirectorMode.cs:143 `[RuntimeInitializeOnLoadMethod(AfterSceneLoad)]` her sahnede otomatik kuruyor → normal play'de "editör modu gibi" (turuncu çerçeve + SPAWN paneli). Repoda Director'ı dev-only yapacak MEVCUT bir flag/gate/dev-define/F5-launch yolu VAR MI? "Play Arena (F5)" ve "Play From Main Menu" menüleri nasıl çalışıyor (hangi sahneyi açıyor, Director'ı set ediyor mu)? En az-kod gate önerisi (mevcut mekanizmayı kullanarak).
② GERÇEK AKIŞ: MainMenu → CharacterSelect (chamber) → gameplay akışı zaten kurulu mu? Normal Play butonu hangi sahnede başlıyor (build index 0 = MainMenu)? Kullanıcı normal Play'de char-select görmüyor — neden (Director auto-bootstrap mı, yanlış başlangıç sahnesi mi)?
④ EDİTÖR ↔ OYUN-İÇİ ODA BİRLEŞTİRME: RoomTemplateSO tek-gerçek-kaynak mı? UnifiedMapDesigner + RoomTemplateBrowserWindow + in-game Build Mode aynı SO katalogunu mu okuyor yoksa ayrı yollar mı? "Refresh" için repoda yeniden-kullanılabilir katalog/asset-scan mekanizması var mı (IAssetCatalog vs)? Runtime Save (BuildModeController.SaveWorkingTemplate) editör tarafına yansıması için MEVCUT ne var? Demo'ya sığacak MVP kapsamı öner.
③ F2 TILE HİZALAMA: BuildPlacementController grid overlay + tile placement kodunu oku. Grid CellLayout/CellSize, GetCellCenterWorld kullanımı, overlay diamond'ın neighbour-midpoint hesabı doğru cell-center veriyor mu? floor tile pivot/PPU (64x64 @ PPU32 → 2 birim?) ile CellSize (~0.96x0.585) uyumsuz mu? En olası hizalama-kök + en hızlı doğrulama adımı.

Her konu için: ≤4 satır, somut dosya/satır referansı, az-kod + demo-risk-aware.
