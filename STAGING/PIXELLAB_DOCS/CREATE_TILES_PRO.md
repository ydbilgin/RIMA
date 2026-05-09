# Create Tiles Pro
*Kaynak: https://www.pixellab.ai/docs/tools/create-tiles-pro*

## Özet
Text description'dan oyun haritası için tile varyasyonları üretir. Çoklu tile tipi ve konfigürasyon destekler.

## Tile Tipleri
- Square (top-down)
- Hex
- Hex pointy
- **Isometric** ← RIMA için ana kullanım
- Octagon

## Tile Boyutları

### Kare Boyutlar
16, 32, 48, **64**, 96, 128 px

### Dikdörtgen (Width×Height)
16×32, 32×64, 48×96, **64×128** ← RIMA duvar boyutu

## Parametreler
- **Description:** Text prompt ("medieval cobblestone")
- **View Angle:** Low top-down (default) / High top-down / Side
- **Tile Thickness:** Görsel kalınlık
- **Style Reference Tiles:** Stil tutarlılığı için referans image (opsiyonel)
- **Seed:** Randomizasyon kontrolü

## Maliyet
- 64×64 ve üstü: **25 gen**
- Küçük varyantlar: 20 gen
- Style reference tiles: 20–40 gen (karmaşıklığa göre)

## Kısıtlama
Tier 1 veya üstü abonelik gerekli.

## RIMA Kullanım Senaryosu
Production Playbook Adım 4-8 (F1/F2/F3 floor tiles + transitions).
MCP üzerinden `create_tiles_pro` ile çağrılabilir.
