# Codex Task — Wall Room ITER v3: N-only wall with CONNECTOR+SPAN pattern + correct floor scale

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Amaç
v2 render FAIL nedenleri:
1. **Wall span yok** — sadece Sheet 1 connector'lar (pillar) sıralanmış, aralara wall span konmamış → duvarlar "pillar sırası" gibi
2. **Floor PPU yanlıştı** — düzeltildi, AssetPostprocessor floor için PPU 32 olarak güncellendi (zaten 16 floor reimport edildi)
3. **4 yön denedi ama Sheet 1+2 sadece N-facing 3/4 sprite** — S/W/E için ayrı sprite yok (sade ilk MVP)

v3 hedef: chatgpt_ref Image 1 tarzı **N-only wall + floor + karakter ortada** sade görüntü. Sadece kuzey duvar görünür, S/E/W invisible collider'larla kapatılır.

## Context
- Mevcut scene: `Assets/Scenes/Test/WallRoomTest_v1.unity` (v2 build var — temizlenip yeniden kurulacak)
- 12 wall prefab: `Assets/Prefabs/Environment/Walls/AssetPackV3/` 
  - 8 connector (Sheet 1): Connector_Straight, Connector_OuterCorner, Connector_InnerCorner, Connector_End, Connector_DoorLeft, Connector_DoorRight, Connector_Alcove, Connector_Protrusion
  - 4 wall span (Sheet 2): Codex'in MVP'de seçtiği 4 piece. WallChunkLibrary_v3'te kontrol et hangileri var. Genelde WallSpan_PlainMidA, WallSpan_PlainMidB, WallSpan_Long, OuterCorner_L gibi.
- Floor 15 tile asset: `Assets/ScriptableObjects/Tiles/Floor/` (tile_0..tile_14, tile_15 hariç). Hepsi 32px PPU 32 = 1 unit/tile.
- Karakter prefab: `Assets/Prefabs/Characters/Warblade.prefab`

## Layout Spec — N-only wall, 12 unit wide × 8 unit tall floor

### Floor (Tilemap):
- Grid GameObject CellSize (1, 1, 0)
- Tilemap_Floor — TilemapRenderer sortingLayer "Floor" (yoksa default), sortingOrder -10
- Paint 12 × 8 = 96 cell, X aralığı -6..+5, Y aralığı -4..+3
- Tile dağılım: %80 tile_0 (plain), %15 tile_1 (variant plain), %5 mix (tile_4 cracked, tile_8 mossy)

### N Wall (kuzey duvar, görünür):
- Konum: floor üst kenarı (Y = +4 civarı, sprite Bottom-Center pivot)
- Pattern: connector + span + span + span + span + span + connector
- Genişlik: 7 piece × ~1.78 unit (sprite width 107px / PPU 64) ≈ 12.5 unit
- BASIT yöntem: x_step = 2 unit kullan (round number), 7 piece * 2 = 14 unit (floor 12 unit, biraz taşar ama kenarlarda corner durur)
- Position (Y = +4 sabit):
  - x=-6: corner_outer_L (eğer Library'de varsa, yoksa Connector_OuterCorner)
  - x=-4: WallSpan_PlainMidA
  - x=-2: WallSpan_PlainMidB
  - x=0: WallSpan_Long (geniş ise tek alır)
  - x=+2: WallSpan_PlainMidA (tekrar)
  - x=+4: WallSpan_PlainMidB
  - x=+6: corner_outer_R (eğer yoksa Connector_OuterCorner mirror)

Eğer WallChunkLibrary'de outer corner L/R yoksa, sadece Connector_OuterCorner kullan başta + sonda. SpriteRenderer.flipX = true sağ köşede.

### S/E/W Invisible walls (collider only):
- 3 boş GameObject + her birine BoxCollider2D:
  - InvisibleWall_S: pos (0, -4.5, 0) size (14, 1) — floor alt kenarı
  - InvisibleWall_E: pos (6.5, 0, 0) size (1, 8) — floor sağ kenarı
  - InvisibleWall_W: pos (-6.5, 0, 0) size (1, 8) — floor sol kenarı

### Karakter:
- Warblade prefab instantiate (0, 0, 0) — oda merkezi

### Camera:
- Main Camera: Orthographic size = 4.5 (floor + wall framing), pos (0, 1, -10)
- Background: dark gray #1A1F2E (memory'deki blue tone)

## Tasks

### 1. Scene rebuild
- `WallRoomTest_v1.unity` aç
- Mevcut Room_Walls altındaki TÜM child'ları sil
- Mevcut Grid/Tilemap_Floor'u sil (yeniden kurulacak)
- Mevcut Warblade'i sil
- Camera, Directional Light kalsın (ayarları güncelle)

### 2. Floor Tilemap (12×8 cell)
- Grid + Tilemap_Floor yeniden kur
- 96 cell paint et (yukarıdaki dağılımla)
- Tile.asset'leri Assets/ScriptableObjects/Tiles/Floor/ klasöründen ref al

### 3. N Wall (7 piece)
- Library'den WallChunkData lookup yap, prefab'ları al
- 7 PrefabUtility.InstantiatePrefab call
- Yukarıdaki position math
- Parent: Room_Walls

### 4. Invisible S/E/W walls
- 3 empty GameObject + BoxCollider2D
- Layer: "Wall" (yoksa default — Walls collider için)

### 5. Karakter
- Warblade prefab instantiate (0, 0, 0)

### 6. Camera + Lighting
- Camera ortho size 4.5, pos (0, 1, -10), bg #1A1F2E
- Directional Light intensity 1.0

### 7. Save + render
- Scene save
- Screenshot Scene View (top-down, oda framing): `STAGING/concepts/asset_pack_v3/test_room_render_v3.png`
- Screenshot Game View (camera framing): `STAGING/concepts/asset_pack_v3/test_room_game_v3.png`
- Screenshot için resolution 1280×720 veya benzeri

### 8. Compile + console check
- 0 error, scene dirty save

## Definition of Done
- WallRoomTest_v1.unity: floor görünür (cobblestone), N wall connector+span pattern ile düzgün, karakter ortada, camera N wall'a bakıyor
- Floor tile'ları 1×1 unit (32px PPU 32), wall sprite'ları 2×4 unit (128×256 PPU 64), oran 4:1 doğru
- 2 screenshot kaydedildi
- Console 0 error
- "AssetPackV3 room v3 ready for QC"

## Critical Reminders
- Sheet 1 + 2 sadece N-facing 3/4 view sprite — S/E/W için bunları KULLANMA
- Wall span'ları DUVAR ARALARINA koy, connector'lar sadece köşelerde
- Floor PPU 32 zaten reimport edildi, scale doğru olmalı
- Y coordinate negative S için (floor bottom edge), positive N için (floor top edge)
