# Task — Checker zemin + 15 Generated odaya prop auto-placement (Sonnet-MCP)

Amaç: 15 prop-fakiri Generated oda görsel olarak dolsun: (a) damalı zemin varyasyonu, (b) mevcut BridsonPoissonAutoPlacer + CompositionRoleMap ile prop yerleşimi. Karar: `STAGING/MODULAR_PROPS_DECISION_2026-06-05.md` (overlay-path ZATEN implemente — ona dokunma).

Envanter (CODEX_DONE_yasinderyabilgin.md §8, taze):
- `IsoRoomBuilder.cs:263-289` BuildFloor tek tile boyar → checker: ikinci floor tile referansı (serialized, null ise eski davranış) + `((x+y)&1)` seçimi.
- `BridsonPoissonAutoPlacer.cs` (:84-200) + `CompositionRoleMapGenerator.cs` (:29-95 clean center/edge/door pass'leri) HAZIR; testler var (`Assets/Tests/EditMode/Props/`, `Composition/`).
- 15 oda = `Assets/Data/Rooms/Generated/*.asset`, hepsi `props: []`.
- Prop havuzu = `Assets/Data/Props/PropRegistry.asset`.

Yap:
1. IsoRoomBuilder'a `floorTileAlt` (serialized, optional) + BuildFloor'da checker seçim. Null-safe: alt yoksa aynen eski.
2. Editor utility (menü item `RIMA/Rooms/Populate Generated Props`): 15 Generated SO'yu yükle → her biri için CompositionRoleMapGenerator.GenerateFromRoom + BridsonPoissonAutoPlacer.Generate → `PropPlacementData` listesine yaz (Undo + SetDirty + SaveAssets). Yoğunluk: merkez TEMİZ kalmalı (combat alanı) — role map'in clean-center pass'ine güven, density konservatif başlat.
3. Utility'yi ÇALIŞTIR (UnityMCP) → 15 SO'da props doluluk raporu (oda başına kaç prop).
4. Play-verify: _Arena'da bir run başlat (RoomRunDirector) → bir Generated odanın prop'lu + checker zeminli kurulduğunu gözle/raporla. Konsol 0 error.
5. EditMode testleri koş (Props + Composition + Room) — yeşil kalmalı.

Kısıtlar: sahne dosyası elle düzenleme YOK · overlay sistemine dokunma · checker tile asset'i için YENİ ASSET ÜRETME — mevcut floor tile varyantlarından birini kullan (Assets'te floor451 varyant grubu var; uygun koyu/açık ikili seç) · COMMIT YAPMA (cx diff-review + Opus play-verify sonrası).

Rapor: kısa — değişen dosyalar, oda-başına prop sayıları, test sonucu, gözlem.
