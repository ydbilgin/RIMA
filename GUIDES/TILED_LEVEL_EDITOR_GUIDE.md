# RIMA Tiled Level Editor Guide

## Giris
RIMA oda sistemi Tiled map editor + SuperTiled2Unity pipeline uzerinden calisiyor.
Bu rehber yeni bir level designer veya dev'in bagimsiz calisabilmesi icin hazirlandi.

## 1. Tiled Kurulumu
- https://www.mapeditor.org/download adresinden Windows icin son surumu indir
- Kur ve ac
- File > Open Project > RIMA repo'sunda Editor/TiledProjects/RIMA.tiled-project sec

## 2. Yeni Oda Olusturma
1. File > New Map
2. Orientation: Isometric
3. Tile size: 128 x 64
4. Map size: basit oda icin 8x8, buyuk oda icin 12x12 veya 16x16
5. Kaydet: Editor/TiledMaps/Act1/yeni_oda.tmx

## 3. Layer Yapisi

| Layer    | Tip          | Aciklama                                       |
|----------|--------------|------------------------------------------------|
| Floor    | Tile Layer   | Zemin tile'lari (act1_keep)                    |
| Walls    | Tile Layer   | Duvar tile'lari (act1_walls)                   |
| Spawns   | Object Layer | GateSocket ve MobSpawnPoint point object'leri  |
| Metadata | Object Layer | Sadece map-level property tasiyor              |

## 4. GateSocket Ekleme
1. Spawns layeri sec
2. Sol menude Object Tool (O)
3. Haritaya point object yerles (kuzey kenari ortasina)
4. Object Properties panelinde:
   - Name: GateSocket_N
   - Custom Properties:
     - socketId: g0_n
     - direction: N  (N/S/E/W)
     - gateVisual: arch

## 5. MobSpawnPoint Ekleme
1. Spawns layeri sec
2. Point object yerles (oda icine)
3. Custom Properties:
   - spawnTier: basic  (basic/elite/boss)
   - spawnTags: hollow  (virgul ayirici: hollow,shieldbearer)

## 6. Map-level Properties
Haritayi sec (hicbir layer/object sectmeden) > Properties paneli:
- roomType: combat
- biome: keep
- difficulty: 1
- combatQuestion: basic_swarm
- roomId: yeni_oda

## 7. Kaydet + Unity Import
1. Tiled'da Ctrl+S
2. Unity otomatik degisikligi algilar (SuperTiled2Unity file watcher)
3. Assets/_Generated/Rooms/Act1/yeni_oda.prefab olusur/guncellenir
4. Console'da [RimaTmxPostProcessor] satirlari gorursun

## 8. Prefabi Sahneye Koy
1. Project penceresinde Assets/_Generated/Rooms/Act1/ ac
2. Prefabi Hierarchy'e drag-drop
3. GateSocket: sari/yesil kure gizmo (Scene view)
4. MobSpawnPoint: kirmizi kup gizmo (Scene view)

## 9. Sik Sorunlar

### Pivot Kayiyor
- Her tileset dosyasinda (act1_keep.tsx, act1_walls.tsx) unity:pivot property = 0.5,0 olmali
- Unity: Edit > Project Settings > Graphics > Transparency Sort Mode = Custom Axis, (0, 1, 0)

### Y-Sort Bozuk
- Sorting layers sirasi dogru mu? Ground < Walls < Entities

### SuperTiled2Unity Import Calismiyorsa
- Console'daki hata mesajini oku
- Package Manager > SuperTiled2Unity > Reimport
- Eger git URL hatasi veriyorsa: GitHub releases sayfasindan .unitypackage indir
  URL: https://github.com/Seanba/SuperTiled2Unity/releases

### Yeni Tile Asset Eklendiginde
1. Assets/Art/Tiles/Act1/F1/ dizinine PNG'yi koy
2. Tiled'da tileset ac > F5 (Reload)

## 10. Pipeline Ozet

Tiled (.tmx) -> SuperTiled2Unity import -> RimaTmxPostProcessor -> prefab
                                                                    GateSocket MB
                                                                    MobSpawnPoint MB
                                                                    RoomMetadata MB

## 11. Pivot Fix - Teknik Detay

Isometric pivot mismatch en yaygin sorundur. Cozum:

**Tiled tarafinda:**
- act1_keep.tsx: unity:pivot property = 0.5,0
- act1_walls.tsx: unity:pivot property = 0.5,0

**Unity tarafinda:**
- Edit > Project Settings > Graphics
- Transparency Sort Mode: Custom Axis
- Transparency Sort Axis: X=0, Y=1, Z=0

Bu ayarlar olmadan wall sprite'lar floor sprite'larla z-fight yapar ve Y-sort calismaz.
