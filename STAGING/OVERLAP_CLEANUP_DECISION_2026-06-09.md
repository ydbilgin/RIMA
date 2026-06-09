# OVERLAP CLEANUP DECISION — 2026-06-09

**Karar mercii:** Council (cx + ax 3.1 Pro + ax 3.5 Flash + ChatGPT-web) → Opus sentez.
**Kapsam:** Demo öncesi "kritik overlap temizliği" — 3 hedef grup. Politika: [Obsolete]-önce-sonra-sil.
**Bağlam:** Tüm iddialar grep-doğrulandı (file:line uydurma yok). Canlı demo sahnesi `_Arena` = SADECE RoomRunDirector + IsoRoomBuilder.

## Advisor özetleri
- **ax 3.1 Pro (deep):** Hepsi (A) — [Obsolete]+rename + HEDEF2 bypass kes + HEDEF3 RewardPickup→RoomRunDirector decouple. 3-commit Strangler Fig.
- **ax 3.5 Flash (lean):** Hepsi (C) — DOKUNMA. Demo çalışıyor, testler yeşil. Temizlik=over-engineering riski.
- **ChatGPT-web:** Middle — DungeonGraph ayır · MainMenuScreen guard · DoorTrigger/GateBehavior legacy **bırak**. Boyut: S.

## KARAR (her hedef tek-satır)

### HEDEF 1 — İki DungeonGraph isim çakışması → **A (kısmi): RENAME**
Legacy `RIMA.DungeonGraph` (`Assets/Scripts/Core/DungeonGraph.cs`) → `LegacyIsoDungeonGraph` (dosya+class+[Obsolete]). **Gerekçe:** demo/random yol AYRIMI zaten var (`forceDemoSequence`); kalan tek footgun = isim çakışması (CS0104 + Inspector). Shop/oda kodu yazmadan önce kalıcı kapatılır. İzole, verify'lı, revert-kolay commit. `.meta` GUID korunur → legacy `_IsoGame` sahne refleri kırılmaz.

### HEDEF 2 — MainMenuScreen vs MainMenuController → **C (no-op): guard zaten yeterli**
Canlı demo sahneleri (MainMenu/_Arena/CharacterSelect) guard listesinde → ghost-menu canlı akışta imkansız. `OnPlayClicked` bypass'ı canlı MainMenu sahnesinde (MainMenuController kullanır) **erişilemez**. Kod değişikliği YOK; 4 test'i kırma riski alınmaz. (Opsiyonel: dosya başına `// LEGACY` notu — düşük değer, atlandı.)

### HEDEF 3 — DoorTrigger + GateBehavior → **C: legacy bırak**
`RoomClearVictoryTrigger` (CANLI `RewardPickup.cs:163`'ten static çağrılıyor) bu tiplere derleme-bağımlı. Canlı kapı/zafer otoritesi ZATEN `RoomRunDirector` (kendi exit+softlock zinciri, `_Arena`'da `DoorTrigger` yok → legacy çağrı no-op). 3.1 Pro'nun decouple önerisi **REDDEDİLDİ**: yeni softlock-fix'li (7489e2de) canlı zinciri demo-öncesi refactor = sıfır fayda, en yüksek risk.

## DEMO-SONRASI FAZ (deferred)
Tüm legacy `_IsoGame` map subsystem'i fiziksel sökümü: `LegacyIsoDungeonGraph`, MiniMap, DungeonMapUI, MapFragment, DungeonWorldBuilder, RuntimeRoomManager, Map/Runtime/RoomLoader+RoomInstance, Systems/Map/RoomLoader, DoorTrigger, GateBehavior, RoomClearVictoryTrigger + bağlı testler + `_IsoGame*.unity` sahneleri. RewardPickup→RoomRunDirector decouple bu fazda.

## COMMIT
1. `refactor: rename legacy DungeonGraph → LegacyIsoDungeonGraph (break name collision)` — isole. Verify: Unity compile 0-error + DungeonGraphTests/RoomFlowTests/PlaytestScenarios + UI contract testleri yeşil.

Sonra demo Faz 1 (camera single-authority).
