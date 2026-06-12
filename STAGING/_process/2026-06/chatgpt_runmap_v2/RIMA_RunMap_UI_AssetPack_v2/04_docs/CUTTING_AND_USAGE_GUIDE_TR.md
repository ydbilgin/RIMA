# RIMA — Kesme, Şeffaflaştırma ve Unity Kullanım Rehberi

## Kısa gerçek

ZIP içinde iki sürüm var:

1. `02_individual_assets_bgmagenta/`
   - Arka plan magenta `#FF00FF`.
   - Senin istediğin gibi, arkası tek renk.
   - Bunu Aseprite/Photoshop/GIMP içinde şeffaflaştırabilirsin.

2. `03_bonus_transparent_assets/`
   - Aynı assetlerin otomatik şeffaflaştırılmış sürümü.
   - Hızlı test için kullan.
   - Kenarlarda pembe kalıntı görürsen magenta sürümden kendin temizle. Piksel sanatı hassas iş, insanlığın hâlâ mucizevi biçimde batırabildiği bir alan.

---

# 1. Hangi dosyayı nerede kullanacaksın?

## Run Map node assetleri

Klasör:

```text
02_individual_assets_bgmagenta/nodes/
03_bonus_transparent_assets/nodes/
```

Dosyalar:

```text
node_combat.png
node_elite.png
node_boss.png
node_merchant.png
node_chest.png
node_forge.png
node_event.png
node_hidden.png
node_current_combat_glow.png
node_connection_kit.png
```

Kullanım:

- `node_combat`: savaş odası.
- `node_elite`: elit oda.
- `node_boss`: en üstteki boss odası.
- `node_merchant`: tüccar.
- `node_chest`: hazine.
- `node_forge`: demirci / yükseltme.
- `node_event`: etkinlik / soru işareti.
- `node_hidden`: reveal edilmemiş node.
- `node_current_combat_glow`: mevcut oda vurgusu.
- `node_connection_kit`: bağlantı çizgisi için stil referansı.

Önerilen UI boyutu:

```text
Node normal: 120×92 px veya 132×100 px
Boss node: 132×100 px ya da biraz daha büyük
Mevcut node glow: node ile aynı pozisyonda, node üstüne/altına overlay
```

En temiz yöntem:
- İkonu ve çerçeveyi sprite olarak kullan.
- Yazıları sonradan TextMeshPro ile bas.
- Bu assetlerde yazı var, ama finalde daha iyi kontrol için yazıyı sprite'tan ayırmak mantıklı olur. Evet, yine aynı şeyi ikiye bölüyoruz, çünkü UI böyle bir işkence.

---

# 2. Rarity Ribbon assetleri

Klasör:

```text
02_individual_assets_bgmagenta/ribbons/
03_bonus_transparent_assets/ribbons/
```

Dosyalar:

```text
reward_rarity_ribbon_common.png
reward_rarity_ribbon_rare.png
reward_rarity_ribbon_epic.png
```

Kullanım:
- Reward card'ın üst bandı.
- Kartın üstüne 10-20 px taşacak şekilde koyabilirsin.
- Yazıyı sprite içinde tutabilirsin.
- Lokalizasyon ileride büyürse yazısız ribbon üretip TMP ile yazı basmak daha mantıklı.

Önerilen final boyut:

```text
112×28 px orijinal brief boyutu
Kartların büyükse 160×40 px'e kadar büyüt
```

Unity:

```text
Image Type: Simple
Filter Mode: Point
Compression: None
```

---

# 3. Minimap marker assetleri

Klasör:

```text
02_individual_assets_bgmagenta/markers/
03_bonus_transparent_assets/markers/
```

Dosyalar:

```text
minimap_player_marker.png
minimap_room_tile.png
minimap_door_marker.png
```

Final kullanım boyutları:

```text
minimap_player_marker: 20×20 px
minimap_room_tile:     16×16 px
minimap_door_marker:   12×12 px
```

Unity tarafında bunları tek tek Sprite olarak import et.

Minimap üretimi:
- Her oda için `minimap_room_tile` çiz.
- Oyuncunun bulunduğu odanın üstüne `minimap_player_marker` bindir.
- Kapı/çıkış yönlerinde `minimap_door_marker` kullan.
- `minimap_frame` bunun çevresinde kalır.

---

# 4. Minimap frame

Klasör:

```text
02_individual_assets_bgmagenta/frame/
03_bonus_transparent_assets/frame/
```

Dosya:

```text
minimap_frame_280x220_style.png
```

Önerilen kullanım:
- UI panel olarak sağ üstte kullan.
- İç alan harita çizimi için boş bırakılmalı.
- Bu görsel stil referansı gibi de kullanılabilir.
- Finalde 9-slice temiz olsun istiyorsan frame'i şu parçalara bölmek daha iyi:

```text
minimap_corner_tl.png
minimap_corner_tr.png
minimap_corner_bl.png
minimap_corner_br.png
minimap_edge_top.png
minimap_edge_bottom.png
minimap_edge_left.png
minimap_edge_right.png
minimap_inner_bg.png
```

9-slice sınırı:

```text
18 px border
```

Unity ayarı:

```text
Sprite Editor > Border: L 18 / R 18 / T 18 / B 18
Image Type: Sliced
```

---

# 5. Magenta arka plan nasıl şeffaf yapılır?

## Aseprite
1. PNG'yi aç.
2. Magic Wand ile `#FF00FF` seç.
3. Tolerance: 0 veya çok düşük.
4. Delete.
5. Export PNG.

## Photoshop
1. Select > Color Range.
2. Magenta `#FF00FF` seç.
3. Fuzziness düşük tut.
4. Delete.
5. PNG olarak export et.

## GIMP
1. Colors > Color to Alpha.
2. Color: `#FF00FF`.
3. Export PNG.

## Unity içinde doğrudan kullanma
Unity otomatik chroma key yapmaz. Magenta görseli direkt koyarsan pembe arka plan görünür. Şaşırtıcı, motor telepati bilmiyor.

Bu yüzden:
- Ya `03_bonus_transparent_assets` kullan.
- Ya da magenta sürümü dışarıda temizle.

---

# 6. Unity import ayarları

Tüm UI sprite'ları için:

```text
Texture Type: Sprite (2D and UI)
Sprite Mode: Single
Pixels Per Unit: 100 veya UI standardın
Mesh Type: Full Rect
Filter Mode: Point
Compression: None
Generate Mip Maps: Off
Alpha Is Transparency: On
```

UI Canvas:

```text
Canvas Scaler: Scale With Screen Size
Reference Resolution: 1920 × 1080
Match: 0.5
```

---

# 7. Run map layout algoritması

Harita her zaman 3'lü olmayacak. Doğru mantık:

```text
Depth 0: 1 node, başlangıç
Depth 1-4: 1, 2 veya 3 node
Depth 5: 1 node, boss
```

Örnek:

```text
Depth 5:          Boss

Depth 4:      Elite      Forge

Depth 3: Merchant   Combat   Chest

Depth 2:      Event      Combat

Depth 1:      Elite      Chest

Depth 0:          Combat
```

Bağlantı:
- Her node bir üst depth'te 1 veya 2 node'a bağlanır.
- Bağlantılar çok çaprazlaşmasın.
- Her node'a en az bir yoldan ulaşılabilsin.
- Boss node'a en az 1-2 farklı yol bağlansın.

---

# 8. Dosyaları keserek kullanmak istersen

Zaten tek tek kesilmiş assetler var. Ama kaynak sheet'ten tekrar kesmek istersen:

## Node sheet
Kaynak:

```text
01_source_sheets/sheet_runmap_nodes.png
```

Kesilecek görsel parçalar:
- 1. satır soldan sağa:
  - combat
  - elite
  - boss
  - merchant
  - chest
- 2. satır soldan sağa:
  - forge
  - event
  - hidden
  - current node glow
  - connection kit

## Ribbon sheet
Kaynak:

```text
01_source_sheets/sheet_rarity_ribbons.png
```

Kesilecek parçalar:
- Üst: common
- Orta: rare
- Alt: epic

## Marker sheet
Kaynak:

```text
01_source_sheets/sheet_minimap_markers.png
```

Kesilecek parçalar:
- Sol: player marker
- Orta: room tile
- Sağ: door marker

## Frame sheet
Kaynak:

```text
01_source_sheets/sheet_minimap_frame.png
```

Kesilecek parça:
- Tüm frame.
- Finalde istersen 9-slice parçalarına ayır.

---

# 9. Claude'a verilecek direkt görev metni

```text
Bu ZIP içindeki RIMA_RunMap_UI_AssetPack_v2 klasörünü incele.

Hedef:
RIMA için STS2 tarzı, ama RIMA'nın dark fantasy/pixel-art estetiğine uygun dallanan koşu haritası UI sistemi kur.

Kullanılacak ana referans:
00_preview_mockups/preview_run_map_mockup.png

Kullanılacak assetler:
- 02_individual_assets_bgmagenta/ veya doğrudan 03_bonus_transparent_assets/
- Magenta #FF00FF arka planı şeffaf kabul et.
- Unity import ayarları: Point filter, no compression, no mipmap.

Harita mantığı:
- Başlangıç altta tek node.
- Boss üstte tek node.
- Ara depth'lerde 1, 2 veya 3 node olabilir.
- Her node bir üst depth'te 1 veya 2 node'a bağlanır.
- Her node'a ulaşılabilir olmalı.
- Bağlantılar kesik çizgi şeklinde görünmeli.
- Mevcut node cyan glow ile vurgulanmalı.
- Ziyaret edilen node tam renkli, görünen ama gidilmemiş node düşük opacity, gizli node siyah/gizli görünmeli.

Unity işleri:
1. RunMapScreen prefab oluştur.
2. RunMapNode prefab oluştur.
3. RunMapConnectionRenderer oluştur.
4. RoomType enum: Combat, Elite, Boss, Merchant, Chest, Forge, Event.
5. Node state: Current, Visited, Revealed, Hidden.
6. Preview mockup'a yakın görsel düzen kur.
7. Sağ altta legend ekle.
8. M tuşuyla açılıp kapanacak şekilde bağla.
```
