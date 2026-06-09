# RIMA Council — DEEP ARCHITECTURE LENS (Gemini 3.1 Pro High)

RIMA = 2D Unity ARPG. Demo için "kritik overlap temizliği" kararı veriyoruz. Sen DERİN MİMARİ / tasarım lensisin: doğru-uzun-vadeli single-source-of-truth, dual-authority riski, teknik borç. Aşağıdaki kanıt GREP-DOĞRULANMIŞ — file:line UYDURMA, verilen kanıta + mimari muhakemene dayan.

## Canlı mimari (grep-doğrulandı)
- Akış: MainMenu(MainMenuController, scene-backed) → CharacterSelect(ChamberSelectBootstrap primary) → _Arena
- `_Arena.unity` SADECE RoomRunDirector + IsoRoomBuilder içeriyor. Legacy subsystem'in hiçbiri canlı sahnede yok.
- Canlı çıkış kapısı = RoomRunDirector'daki nested RoomRunExitDoorTrigger.
- Softlock kökten fix'li (12s/90s timeout + ForceOpenExitDoorsFromAnyClearedState, 76/76 test).

## 3 OVERLAP HEDEFİ + gerçek bağımlılıklar
### HEDEF 1 — İki DungeonGraph
- CANLI: RIMA.MapDesigner.Room.Runtime.DungeonGraph (saf class, BuildDemoSequence)
- LEGACY: RIMA.DungeonGraph (Core/DungeonGraph.cs, MonoBehaviour, .Instance, [Obsolete] YOK)
- Legacy'ye bağlı: MiniMap, DungeonMapUI, MapFragment, DungeonWorldBuilder, RuntimeRoomManager (hepsi legacy _IsoGame subsystem) + 3 test. _Arena'da değil.
- Risk: aynı isim → CS0104 ambiguity + Inspector yanlış-class.

### HEDEF 2 — MainMenuScreen (auto-create) vs MainMenuController (canlı scene-backed)
- MainMenuScreen canlı kod ref'i sadece bir yorum. Sadece legacy _IsoGame sahnelerinde baked.
- [RuntimeInitializeOnLoadMethod] ile self-create AMA static _gameStarted + sahne-adı guard'ı (MainMenu/_Arena/CharacterSelect korunuyor). Ghost-menu sadece guard-dışı sahnede.
- OnPlayClicked → paralel CharacterSelectScreen (DontDestroyOnLoad) = chamber'ı bypass eden ikinci selection yolu.
- 4 test dosyası guard davranışını doğruluyor.

### HEDEF 3 — DoorTrigger + GateBehavior (legacy)
- _Arena'da component yok. AMA RoomClearVictoryTrigger.cs tipleri kullanıyor, ve RoomClearVictoryTrigger.ActivateExitDoors() CANLI RewardPickup.cs:163'ten çağrılıyor → tip silinirse derleme kırılır.
- Olası ÇİFT victory-authority: RoomClearVictoryTrigger vs RoomRunDirector.

## Sorular (her hedef için AYRI, mimari lens)
1. Her hedef ŞİMDİ: (A) [Obsolete]+rename, fiziksel sil ertele / (B) tüm legacy cluster+test+_IsoGame sahne sil + RoomClearVictoryTrigger ayır / (C) dokunma. NET A/B/C + mimari gerekçe.
2. HEDEF1: legacy'yi rename (LegacyIsoDungeonGraph) etmek isim çakışmasını davranış-nötr çözer mi, yoksa namespace-disambiguation yeterli mi? Uzun vadede legacy _IsoGame subsystem'i tamamen mi sökülmeli?
3. HEDEF2: chamber'ı bypass eden OnPlayClicked→CharacterSelectScreen ikinci yolu demo bütünlüğü için kesilmeli mi? Yoksa guard yeterli mi?
4. RoomClearVictoryTrigger vs RoomRunDirector çift-authority gerçek bir mimari sorun mu? Hangisi canlı tutulmalı?
5. [Obsolete]-önce-sil politikasıyla doğru faz ayrımı + commit sırası.

ÇIKTI: Her hedef tek-satır karar + mimari gerekçe + faz/commit önerisi. Kısa, kanıta dayalı.
