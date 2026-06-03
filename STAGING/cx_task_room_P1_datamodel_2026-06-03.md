ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / STAGING / memory files.

# Amaç
RIMA data-driven oda sistemi (Model B) FAZ 1 = VERI MODELI. Tam karar = STAGING/ROOM_SYSTEM_DECISION_2026-06-03.md (OKU). Bu faz SADECE YENI dosya ekler; mevcut gameplay/sahne/menu'ye DOKUNMA, hicbir sey arsivleme. Compile-clean olmali.

# Kapsam (SADECE bunlar)
Yeni namespace `RIMA.Systems.Rooms`, dosyalar `Assets/Scripts/Systems/Rooms/` altinda:

1. `RoomSizeClass.cs` — enum { Small, Medium, Large, BigArena }.

2. `RoomDefinitionSO.cs` — ScriptableObject [CreateAssetMenu menuName="RIMA/Rooms/Room Definition"]. Alanlar:
   - string roomId (Guid default), displayName
   - RIMA.RoomType roomType (MEVCUT enum reuse — `Assets/Scripts/Core/RoomType.cs`: Combat/Elite/Boss/Chest/Merchant/Forge/Event/Curse/Corridor). NOT: Start/Reward icin = simdilik Corridor/Chest kullan, enum'a EKLEME yapma (ayri karar).
   - RoomSizeClass sizeClass
   - int weight (pool agirlik, default 10), string[] tags
   - SHAPE: List<Vector2> polygonPoints (yurunebilir zemin dis-hat, iso world-space; kullanicinin "noktalari birlestir" modeli)
   - BAKED: List<Vector3Int> bakedFloorCells (Faz2 builder doldurur; simdilik bos olabilir ama alani ekle)
   - ANCHORS: Vector3 playerSpawn; List<Vector3> mobSpawns; Vector3 rewardAnchor; List<DoorAnchor> doors
   - RoomThemeSO theme (ref)
   - opsiyonel: Bounds cameraBounds
   - [Serializable] struct DoorAnchor { Vector3 position; RIMA.DoorDirection direction; } (DoorDirection MEVCUT — Assets/Scripts/Core/DoorTrigger.cs)

3. `RoomThemeSO.cs` — ScriptableObject [CreateAssetMenu "RIMA/Rooms/Room Theme"]. Alanlar: TileBase[] floorTiles; Sprite[] cliffSprites (8-yon veya mevcut CliffKit ref'i — sadece alan, doldurma); GameObject[] props; opsiyonel renk/lighting alanlari. Asset YOLU HARDCODE ETME — sadece ref alanlari.

4. `RoomPoolSO.cs` — ScriptableObject [CreateAssetMenu "RIMA/Rooms/Room Pool"]. Ya List<RoomDefinitionSO> definitions, YA DA folder-auto-discover (Resources/AssetDatabase ile Act1 klasoru). Metot: `RoomDefinitionSO PickRandom(RIMA.RoomType type, RoomSizeClass size, System.Random rng, ICollection<string> excludeRoomIds)` — tipe+boyuta gore filtrele, agirlikli rastgele, exclude'lari atla, anti-immediate-repeat. Runtime-safe (Resources veya serialized list; AssetDatabase SADECE #if UNITY_EDITOR auto-discover icin).

5. `RoomValidation.cs` (static, runtime-safe) — `ValidationResult Validate(RoomDefinitionSO def)`:
   - flood-fill: bakedFloorCells (varsa) TEK bagli bilesen mi (kopuk ada YOK)
   - playerSpawn var mi, en az 1 mobSpawn (Combat/Elite/Boss icin), en az 1 door
   - bool IsValid + List<string> messages dondur.

6. Klasor: `Assets/Data/Rooms/Act1/` + `Assets/Data/Rooms/RoomThemes/`. + ORNEK `Assets/Data/Rooms/Act1/RoomDef_Combat_Test_01.asset` (RoomDefinitionSO): basit ORGANIK polygon (orn. centli kare ya da kucuk cross, ~14x10 iso hucre kapsami), roomType=Combat, sizeClass=Medium, 1 playerSpawn + 2-3 mobSpawn + 1 door (North). bakedFloorCells BOS birak (Faz2 doldurur). Bu Faz2 builder'in test hedefi.

# YASAK
- MapFlowManager / RoomClearVictoryTrigger / DoorTrigger / RewardPickup / herhangi mevcut gameplay dosyasi DEGISTIRME.
- Sahne degistirme/yaratma YOK (Faz2). Menu degistirme YOK (Faz3). Arsivleme YOK (Faz6).
- RoomType enum'una uye EKLEME yok.

# Dogrulama
read_console ile compile 0-hata dogrula. Ornek asset olusunca `manage_asset`/AssetDatabase ile var oldugunu teyit et. CODEX_DONE.md'ye: olusturulan dosyalar listesi + compile durumu + ornek asset path + varsa BLOCKED notu.
