# TASK: Import PixelLab Node Symbols → Unity — 2026-06-11

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS gerekmez (asset import işi).

## Görev
`STAGING/_process/2026-06/pixellab_symbols/` altındaki TÜM `*.png` node sembollerini Unity'ye import et + SpriteAtlas oluştur. Live UI'a BAĞLAMA (ayrı task). Sadece import + atlas + rapor.

## Kaynak
`STAGING/_process/2026-06/pixellab_symbols/*.png` (klasördeki ne varsa hepsi — combat, elite, boss, shop, chest, forge, event, curse_gate, ve varsa rest, unknown, player). Hepsi 64×64 RGBA (player 32×32 olabilir).

## Hedef
`Assets/Sprites/UI/MapNodes/` (yoksa oluştur). Dosya adlarını koru (`combat.png`, `elite.png`, ...).

## Import ayarları (HER dosya)
- Texture Type: Sprite (2D and UI)
- Sprite Mode: Single
- Pixels Per Unit: 64
- Mesh Type: Full Rect
- Filter Mode: Point
- Compression: None
- Generate Mip Maps: OFF
- Alpha Is Transparency: ON

## SpriteAtlas
`Assets/Sprites/UI/MapNodes/UI_MapNodes.spriteatlas` oluştur, tüm sembolleri ekle. Include in Build: ON, Point filter, Compression None, Padding 4.

## Live UI'a bağlama — YAPMA
Sadece raporla: bu semboller `MapNodeUI`/`MapNodeType` ↔ `RoomType` köprüsüyle node ikonu olarak kullanılacak. Wiring = ayrı task (enum köprüsü kararı bekliyor).

## Başarı Kriteri
1. Tüm PNG'ler `Assets/Sprites/UI/MapNodes/` altında doğru import ayarlarıyla
2. `UI_MapNodes.spriteatlas` oluşturuldu, sembolleri içeriyor
3. Unity console: import/compile error yok
4. Kaç sembol import edildi raporla (hangileri eksik varsa belirt)

## Commit
```
feat(ui): import PixelLab run-map node symbols + SpriteAtlas
```
