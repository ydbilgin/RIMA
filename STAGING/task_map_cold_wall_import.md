# CODEX TASK — Map Cold Wall Import + Demo Fix

## Hedef
Demo sahnede (RoomPipelineTest veya Alabaster Dawn demo) wall tile'ları hala eski brown
wang_floor_wall.png kullanıyor. Yeni cold wall tileset Unity'ye import edilip sahneye uygulanacak.

## Execute every step below:

### Step 1 — Cold wall tileset dosyasını bul
`STAGING/TILESET_OUTPUT/F1_Wang_Cold_Wall_NEW/` klasöründe PNG + metadata.json dosyalarını listele.

### Step 2 — Wang importer'ı çalıştır
Mevcut Wang RuleTile importer (commit 5017622 ile eklendi) şu path'lerden birinde:
- `Assets/Scripts/Editor/` altında WangRuleTileImporter veya benzeri bir Editor script

Importer'ı cold wall tileset PNG'si + metadata.json üzerinde çalıştır.
Üretilen RuleTile asset'leri `Assets/Art/Tiles/F1/Generated/` altına kaydet.

### Step 3 — Demo sahnede wall tilemap'i güncelle
`Assets/Scenes/` altındaki demo sahneyi (RoomPipelineTest veya AlabaasterDawn veya benzeri) aç.
Wall tilemap layer'ında kullanılan eski brown tile'ları yeni cold wall RuleTile asset'leriyle değiştir.

### Step 4 — Sonucu doğrula
Unity Console'u oku — hata var mı?
Sahnede wall tile'ları artık soğuk taş/mavi-gri tonlarda görünüyor mu?

### Step 5 — Kaydet
Sahneyi kaydet. Değişiklikleri CODEX_DONE.md'ye yaz.

## Bağlam
- Proje: Pure 2D top-down, URP 2D Renderer, Pixel Perfect Camera
- Tile boyutu: 32x32, PPU: 64
- Hedef görsel: Alabaster Dawn / Loop Hero tarzı soğuk taş duvarlar
- Wang RuleTile: 16-tile sheet (4-bit Wang) + metadata.json → Unity RuleTile asset
- Cold wall tileset ID: bdca2623, STAGING/TILESET_OUTPUT/F1_Wang_Cold_Wall_NEW/
