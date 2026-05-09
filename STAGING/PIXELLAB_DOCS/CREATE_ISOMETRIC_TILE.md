# Create Isometric Tile
*Kaynak: https://www.pixellab.ai/docs/tools/create-isometric-tile*

## Özet
Init image veya text description'dan isometric tileset üretir.

## Parametreler

### Tile Shape
| Seçenek | Açıklama |
|---|---|
| Thick | Kalın isometric blok |
| Thin | İnce isometric tile |
| Block | Katı blok |
| Reference | Custom image'dan şekil/shading/görsel al |
| None | Şekil kısıtlaması yok |

### Description
Text prompt: "grass on top of dirt", "rocky path" gibi.

### Visual Controls
- **Outline:** Kenar tanımı
- **Shading:** Işıklandırma efektleri
- **Details:** İntricate görsel öğeler

### Tile Size
- **32×32** (önerilen — en iyi detay/okunabilirlik dengesi)
- **16×16** (retro stil veya alan kısıtı için)

### Init Image
- Şekil ve görünümü guide eder
- Yüksek değer = referansa daha yakın
- Düşük değer = yaratıcı özgürlük

### Advanced Options
- **Guidance Weight:** Text description'a bağlılık kontrolü
- **Load Previous Settings:** JSON'dan önceki ayarları yükle

### Diğer
- Target palette (renk ayarları)
- Output method (genel ayarlar)

## RIMA Notu
Playbook'ta Adım 1-3 için kullandığımız tool budur (W1/W2/OBW duvarları).
MCP üzerinden `create_isometric_tile` ile de çağrılabilir.
Tutarlı stil için aynı seed kullan.
