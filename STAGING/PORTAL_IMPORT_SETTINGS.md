# Portal Import Ayarları

Bu paket `Assets/Art/Portals/`, `Assets/Art/Telegraphs/` ve `Assets/Art/Boss/` altına üretim import'u olarak alındı.

## Standart

- Texture Type: Sprite (2D and UI)
- Sprite Mode: Single
- Filter Mode: Point
- Pixels Per Unit: 64
- Compression: Uncompressed
- Mip Maps: Kapalı
- Sprite Mesh Type: Full Rect
- Pivot: Center
- Alpha: Transparency açık

## Kapsam Dışı Bırakılanlar

- `portal_arch_elite.png`: Deprecated, import edilmedi. Canlı binding `portal_arch_elite_v2.png` üzerinden yapıldı.
- `portal_arch_boss_angled.png`: Parked, import edilmedi. Boss çıkışı merkez N soketinde frontal kullanır.
- `rune_reward.png` ve `rune_boss.png`: Needs regen, import edilmedi. Mevcut rune sprite'ları korunur.

Telegraph ve boss decal varlıkları sadece import edildi; portal wiring'e bağlanmadı.
