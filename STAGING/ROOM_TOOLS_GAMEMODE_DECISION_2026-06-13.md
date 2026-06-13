# DECISION — Game/Dev Mode Ayrımı + Editör↔Oyun Oda Araçları + F2 Tile Hizalama (2026-06-13)

Council: cx (feasibility/reuse) + ax 3.1 Pro (mimari) + ax 3.5 Flash (lean). Synthesis = Opus.
Kaynak görüşler: `_process/2026-06/_council_{cx,q_31pro,q_35flash}_room_tools_gamemode.md`.
Kullanıcı 4 sorun bildirdi (image 8-11). Demo ~20 Haz, IN-EDITOR.

## ① + ② GAME MODE vs DEV/DIRECTOR MODE AYRIMI
**Kök (cx):** Gerçek oyun akışı ZATEN var + çalışıyor: MainMenu(build 0) → CharacterSelect(1) → `_Arena`.
- `RIMA/Play From Main Menu` = `PlayFromStartScene.cs` (pref `RIMA_PlayFromStartScene`) → playModeStartScene = MainMenu.
- `RIMA/Play Arena (F5)` = `RimaDevShortcuts.cs` → PlayableArena_Test01 + play.
- Kullanıcı açık `_Arena`'dan Play'e bastı → char-select atlandı + `_Arena`'nın DirectorMode (`DirectorMode.cs:143` koşulsuz `RuntimeInitializeOnLoadMethod`) + run-start 3-kart draft (`RoomRunDirector.cs:200`) çıktı → "editör modu gibi".

**KARAR:**
1. Varsayılan Play = full-flow (MainMenu). PlayFromStartScene pref ON varsayılan.
2. DirectorMode + run-start auto-draft + BuildMode auto-bootstrap'ı full-flow oyunda KAPAT, dev/F5'te AÇIK tut. En-az-kod gate = mevcut `RIMA_PlayFromStartScene` pref'i / F5 dev-launch path'i (cx). Hardcoded scene-name DEĞİL (gerçek oyun da `_Arena`'yı kullanıyor → scene-name yetersiz).
3. DEMO Build Mode/Director sunumu = F5/dev path (tools açık). Oyuncu akışı = temiz.
**Risk:** düşük (mevcut infra reuse). Test: full-flow Play → char select gelir, Director YOK; F5 → tools açık.

## ③ F2 BUILD MODE TILE HİZALAMA — ✅ FIXED (commit'te)
**Kök (ax Flash + Opus empirik, cx'in "doğru" görüşünü çürüttü):** `BuildPlacementController.SetDiamond` köşeleri komşu hücre-merkezi midpoint'i = paylaşılan KENAR orta noktası → 0.96×0.585 elmas yerine **0.48×0.30 eksen-hizalı yarı-boy dikdörtgen** → overlay floor ile uyuşmuyor.
**FIX:** köşeler = `GetCellCenterWorld(cell) ± grid.cellSize/2` (iso-doğru, "rect math" yasağı placement'a ait, çizim'e değil). Play-mode'da doğrulandı: overlay artık kesintisiz iso lattice, floor ile hizalı.
**Açık kalan (verify-first):** floor tile `floor451` 64×64 @ PPU64 = 1×1 world vs cell 0.96×0.585 → tile-to-cell 1:1 oturuyor mu görsel teyit; gerekirse cellSize kalibrasyonu (ax Pro/cx). Düşük öncelik (overlay düzeldi).

## ④ EDİTÖR ↔ OYUN-İÇİ ODA ARAÇLARI BİRLEŞTİRME + REFRESH
**3 advisor HEMFİKİR:** LevelEditor framework'e (ILevelStore/IAssetCatalog) ŞİMDİ geçme — demo'ya 7 gün kala dosya I/O refactor = patlama riski.
**Tek-gerçek-kaynak = `RoomTemplateSO` asset'leri** (cx). Editör araçları zaten `AssetDatabase.FindAssets("t:RoomTemplateSO")` + `RefreshRoomTemplates`/`RefreshList` kullanıyor → reuse var.
**DEMO MVP:**
1. In-game Build Mode'a **"Refresh" butonu**: working-copy at → aktif `RoomTemplateSO`'yu AssetDatabase'den yeniden oku → `IsoRoomBuilder.Build` ile sahneyi yeniden kur. (#if UNITY_EDITOR; player no-op.)
2. In-game "Save" zaten SO'ya yazıyor (`SaveWorkingTemplate` CopySerialized). Editör tarafı kendi Refresh'iyle güncel listeyi/preview'u görür → "son kaydeden kazanır", manuel senkron.
3. **FileSystemWatcher / canlı IPC YOK** (ax Flash: soft-lock/NRE riski).
**cx UYARISI:** yeni oda `RoomBankSO`'ya otomatik girmez → runtime run'da görünmesi için manuel bank/ref. (Demo'da manuel ekle.)
**POST-DEMO:** JSON `ILevelStore` (runtime save/load) + Addressables `IAssetCatalog`; AssetDatabase bağımlılığını editor-only pakete izole et.

## UYGULAMA SIRASI
1. ✅ ③ overlay fix (DONE, commit) → floor 1:1 görsel teyit
2. ①② game/dev gate + full-flow default → test (char select gelir, Director temiz)
3. ④ in-game Refresh butonu MVP
