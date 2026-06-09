ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA demo "kritik overlap temizliği" için 3 hedef grubun GÜVENLİ aksiyonunu feasibility/reuse lensinden belirle. ANALİZ ONLY — kod DEĞİŞTİRME. Sonucu CODEX_DONE.md'ye yaz.

# Bağlam (grep-DOĞRULANDI — yeniden audit etme, bunu kullan; ama şüphen varsa Test-Path/grep ile DOĞRULA)

## Canlı mimari
- Akış: MainMenu(MainMenuController, scene-backed) → CharacterSelect(ChamberSelectBootstrap primary) → _Arena
- `Assets/Scenes/_Arena.unity` SADECE şu component'leri içeriyor: RoomRunDirector + IsoRoomBuilder (+IsoRoomBuilderTester). Legacy subsystem'in hiçbiri canlı sahnede YOK.
- Canlı çıkış kapısı = RoomRunDirector içindeki nested `RoomRunExitDoorTrigger` sınıfı.
- Softlock zaten kökten fix'li (commit 7489e2de: 12s/90s timeout + ForceOpenExitDoorsFromAnyClearedState, 76/76 test).

## HEDEF 1 — İki DungeonGraph isim çakışması
- CANLI: `RIMA.MapDesigner.Room.Runtime.DungeonGraph` (saf class, RoomRunDirector kullanıyor, BuildDemoSequence var)
- LEGACY: `RIMA.DungeonGraph` (`Assets/Scripts/Core/DungeonGraph.cs`, MonoBehaviour, `.Instance` singleton). [Obsolete] YOK.
- Legacy'ye derlenen dosyalar: MiniMap.cs, DungeonMapUI.cs, MapFragment.cs, Systems/Map/DungeonWorldBuilder.cs, Core/RuntimeRoomManager.cs (kendisi de legacy) + testler DungeonGraphTests.cs (`AddComponent<DungeonGraph>`), RoomFlowTests.cs, PlaytestScenarios.cs.
- Hiçbiri _Arena'da değil (legacy _IsoGame map subsystem). RİSK: aynı isim → CS0104 ambiguity tuzağı + Inspector'da yanlış-class ekleme.

## HEDEF 2 — MainMenuScreen (auto-create) vs MainMenuController (scene-backed canlı)
- Canlı kod referansı SADECE UIManager.cs:239 yorum satırı. Component olarak yalnız legacy `_IsoGame*.unity` sahnelerinde baked.
- Kendini `[RuntimeInitializeOnLoadMethod]` ile yaratıyor AMA guard'lı: static `_gameStarted` + sahne-adı guard listesi (MainMenu, _Arena, CharacterSelect, _IsoGame, test sahneleri dahil → canlı demo yolu ZATEN korunuyor). Ghost-menu riski sadece guard-DIŞI sahneden başlatınca.
- `OnPlayClicked` → paralel `CharacterSelectScreen` (DontDestroyOnLoad) spawn ediyor = chamber'ı bypass eden ikinci yol.
- Test bağımlılığı: TimeScaleGuardTests.cs, UIFlowContractTests.cs, CharacterSelectTests.cs (`AddComponent<MainMenuScreen>`), Contracts/UIFlowContract.cs — guard davranışını doğrulamak için var.

## HEDEF 3 — DoorTrigger + GateBehavior (legacy, başlıkta "// LEGACY 2026-06-07")
- _Arena'da component olarak YOK (canlı çıkış nested RoomRunExitDoorTrigger).
- Tip-bağımlılığı: `RoomClearVictoryTrigger.cs` içinde `FindObjectsByType<DoorTrigger>()` + GateBehavior kullanıyor. `RoomClearVictoryTrigger.ActivateExitDoors()` static metodu CANLI `RewardPickup.cs:163`'ten çağrılıyor. Tip silinirse RoomClearVictoryTrigger derlenmez → RewardPickup kırılır.
- Diğer kullanıcılar: RuntimeRoomManager, Map/Runtime/RoomLoader, Editor/DungeonSetup, Map/Runtime/RoomInstance (hepsi legacy).

# Sorular (her hedef için AYRI cevap)
1. Her hedef için ŞİMDİ güvenli aksiyon: (A) [Obsolete] + gerekirse rename ile çakışma kır, fiziksel sil sonraki faza ertele; (B) tüm legacy cluster+test+_IsoGame sahnelerini şimdi sil + RoomClearVictoryTrigger'ı ayır; (C) dokunma. Tek-satır NET A/B/C + neden.
2. HEDEF1 çakışması legacy class'ı rename (örn LegacyIsoDungeonGraph) ederek davranış değiştirmeden çözülür mü? HEDEF2 guard zaten canlıyı koruyor — [Obsolete]+guard-genişletme yeterli mi, yoksa OnPlayClicked'in CharacterSelectScreen-spawn yolunu kesmek ŞART mı (demo'da chamber'ı bypass eden ikinci yol risk mi)?
3. RoomClearVictoryTrigger CANLI mı? RewardPickup ondan static çağırıyor AMA RoomRunDirector kendi exit/softlock zincirine sahip → ÇİFT victory-authority var mı? Varsa hangisi canlı?
4. [Obsolete]-önce-sil politikası: hangi dosyalar bu commit'te 0-risk [Obsolete] alabilir, hangileri fiziksel-sil için ayrı faz ister?
5. Önerilen commit-sırası + her adımın doğrulaması (compile + hangi test suite).

# Çıktı formatı
Her hedef için tek-satır karar + birleşik güvenli aksiyon planı + commit sırası. Reuse/feasibility lensi. Önceki audit'i TEKRARLAMA. CODEX_DONE.md'ye yaz.
