# Antigravity Task — Isometric Floor Tile Import + Dungeon Paint

## Bağlam
Unity 2D roguelite projesi. Isometric tilemap LIVE durumda.
- Scene: `Assets/Scenes/Demo/PathC_BaseTest.unity`
- Tilemap: Isometric layout (cellSize 1, 0.5, 1)
- Tile klasörü: `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/`

16 adet yeni PNG Assets klasörüne indirildi. Bunları Unity'ye import et, Tile asset oluştur, dungeon grid'e boyat.

## Adımlar

### 1. Unity Refresh
Assets menüsünden Refresh yap (Ctrl+R veya Assets > Refresh) — 16 yeni PNG'yi Unity'nin görmesi için.

### 2. Her PNG için Texture Import Ayarı
Aşağıdaki 16 dosyanın TextureImporter ayarlarını UnityMCP `manage_asset` ile set et:
- Texture Type: Sprite (2D and UI)
- Sprite Mode: Single
- Pixels Per Unit: 64
- Filter Mode: Point (no filter)
- Compression: None
- Generate Mip Maps: false
- Wrap Mode: Clamp

Dosya listesi (hepsi `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/` altında):
```
floor_granite_v1.png, floor_granite_v2.png, floor_granite_v3.png, floor_granite_v4.png
floor_cyan_v1.png, floor_cyan_v2.png, floor_cyan_v3.png, floor_cyan_v4.png
floor_dirt_v1.png, floor_dirt_v2.png, floor_dirt_v3.png, floor_dirt_v4.png
floor_ritual_v1.png, floor_ritual_v2.png, floor_ritual_v3.png, floor_ritual_v4.png
```

### 3. Her PNG için Tile Asset Oluştur
Her sprite için bir `Tile` asset oluştur. Mevcut placeholder nasıl yapılmışsa aynı yöntemle:
- Mevcut referans: `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/placeholder_iso.asset`
- Yeni tile adları: `tile_floor_granite_v1.asset`, `tile_floor_granite_v2.asset` ... (16 adet)
- Her Tile'ın sprite'ı kendi PNG'si olacak

### 4. Dungeon Grid'i Boyat
`PathC_BaseTest.unity` sahnesindeki Tilemap üzerine boyat:
- **Ana materyal:** `tile_floor_granite_v1` — grid'in tamamını önce bununla doldur (16×10 alan)
- **Varyasyon:** Bazı hücrelere (yaklaşık %20 random) `floor_granite_v2`, `v3`, `v4` karıştır — monotonluğu kır
- Cyan ve ritual tile'ları şimdilik kullanma (ileriki aşama için rezerve)
- Grid origin: (0,0), 16 geniş × 10 yüksek

### 5. Console Kontrol
Değişiklikler sonrası `read_console` ile error/warning kontrol et. Compile error varsa raporla.

### 6. Sonuç Raporu
Tamamlanınca kısa rapor yaz:
- Kaç tile import edildi
- Tile asset'ler oluşturuldu mu
- Grid boyandı mı
- Herhangi bir hata var mı

## Notlar
- UnityMCP stdio mode — Unity açık ve bağlı olmalı
- Placeholder asset/PNG'ye dokunma, sadece yeni 16 dosyayı işle
- İşlemi adım adım yap, her major adım sonrası console kontrol et
