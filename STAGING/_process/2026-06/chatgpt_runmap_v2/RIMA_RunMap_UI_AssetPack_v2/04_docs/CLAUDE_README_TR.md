# RIMA — Run Map UI Asset Pack (Claude için)

Bu paket, RIMA'nın STS2-benzeri dallanan koşu haritası ve ilgili UI parçaları için üretilmiş **kullanılabilir görsel referans + asset pack** dosyalarını içerir.

## Paket yapısı
- `00_preview_mockups/preview_run_map_mockup.png`
  - Haritanın oyunda nasıl görünmesi gerektiğini gösteren ana mockup.
- `01_source_sheets/`
  - Toplu sheet görselleri. Asset'lerin genel stilini ve bir arada görünüşünü gösterir.
- `02_individual_assets_bgmagenta/`
  - Tek tek asset'ler. Arka plan **magenta (#FF00FF)** bırakıldı; istersen bunu chroma key ile şeffaflaştır.
- `03_bonus_transparent_assets/`
  - Ek olarak, aynı asset'lerin otomatik şeffaflaştırılmış sürümleri.
- `04_docs/`
  - Bu açıklama ve özgün brief.

## Stil yönü
- Tür: dark-fantasy pixel-art UI
- Etki: STS2 benzeri dallanan koşu haritası mantığı, ama RIMA estetiğine uyarlanmış
- Ana palet:
  - Cyan accent: `#00FFCC`
  - Ember / orange accent: `#E89020`
  - Slate: `#3A3D42`
  - Void / purple-black: `#3A1A4A` ve yakın koyu tonlar
- Filtre: point / no anti-aliasing

## Harita davranışı (tasarım niyeti)
- Dallanma **her zaman 3 node** olmak zorunda değil.
- Katmanlar (depth) duruma göre 1, 2 veya 3 node taşıyabilir.
- Başlangıç altta tek node.
- Boss en üstte tek node.
- Orta katmanlarda bazen 2'li, bazen 3'lü node dağılımı olmalı.
- Bağlantılar düz listeden ziyade dallanan, birleşen, tekrar ayrılan bir akış hissi vermeli.

## Asset listesi
### Node assets
`02_individual_assets_bgmagenta/nodes/`
1. `node_combat.png` — Savaş düğümü
2. `node_elite.png` — Elit düğümü
3. `node_boss.png` — Boss düğümü
4. `node_merchant.png` — Tüccar düğümü
5. `node_chest.png` — Hazine düğümü
6. `node_forge.png` — Demirci / forge düğümü
7. `node_event.png` — Etkinlik düğümü
8. `node_hidden.png` — Gizli / henüz reveal olmamış düğüm
9. `node_current_combat_glow.png` — Mevcut konum vurgulu düğüm
10. `node_connection_kit.png` — Kesik bağlantı parçaları + cyan diamond işaret

### Ribbons
`02_individual_assets_bgmagenta/ribbons/`
- `reward_rarity_ribbon_common.png`
- `reward_rarity_ribbon_rare.png`
- `reward_rarity_ribbon_epic.png`

### Minimap markerları
`02_individual_assets_bgmagenta/markers/`
- `minimap_player_marker.png`
- `minimap_room_tile.png`
- `minimap_door_marker.png`

### Minimap frame
`02_individual_assets_bgmagenta/frame/minimap_frame_280x220_style.png`

## Unity / kullanım notları
- Import Type: Sprite (2D and UI)
- Filter Mode: Point
- Compression: None
- Mipmaps: Off
- UI için Canvas Scaler: 1920x1080 referans
- Gerekirse node'ları biraz küçültüp kullan; bunlar **stil referansı + üretim tabanı** olarak düşünülmeli.
- `minimap_frame` için 9-slice denenecekse köşeleri sabit tut, iç alanı stretch et.

## Claude'a kısa yönlendirme
Claude, bu paketi kullanarak şu işleri yap:
1. Harita ekranını `preview_run_map_mockup.png` estetiğine yaklaştır.
2. Dallanma sistemini katman bazlı 1-2-3 node olabilecek şekilde kur.
3. `node_*` asset'lerini map UI prefab'ında kullan.
4. `node_current_combat_glow` sadece mevcut oda için görünsün.
5. `node_hidden` unrevealed node state'i için kullan.
6. `node_connection_kit` içindeki kesik segmentlerden bağlantı çiz.
7. `reward_rarity_ribbon_*` kartların üst etiketi için kullan.
8. `minimap_*` asset'lerini sağ üst minimap sistemine bağla.
9. `03_bonus_transparent_assets/` klasörünü direkt kullanabilir, ama gerekirse magenta sürümlerden yeniden temizleme yap.

## Not
Bu pack doğrudan son-production asset yerine, **görsel hedef + kullanılabilir başlangıç assetleri** olarak hazırlanmıştır. Gerekirse Claude bunları temel alıp daha modüler veya daha temiz sprite setleri üretebilir.


## Eklenen rehberler

- `FILE_INDEX_TR.md`: ZIP içindeki dosyaların adları ve boyutları.
- `CUTTING_AND_USAGE_GUIDE_TR.md`: Nasıl kesileceği, nasıl şeffaf yapılacağı, Unity'de nasıl kullanılacağı.
- `ASSET_MANIFEST_DETAILED.csv`: Asset listesi, boyutlar ve kullanım notları.
