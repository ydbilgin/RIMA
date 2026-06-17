# GÖREV: Act-1 oda görselleri + _Arena Şekil 6 capture (TEK Unity ajanı)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical (4) BLOCKED if unclear.

UNITY ERROR CHECK: iş bitince read_console (Error+Warning); kendi hatanı ÇÖZ, ilgisiz/önceden-var hatayı BİLDİR, raporda console durumunu yaz.

🛑 **HARD RULE — NO DEBUG/STATE LEAK:** İş bitince Unity, başladığın temiz duruma DÖNECEK: aktif sahne `_Arena` (isDirty=false), oluşturduğun scratch sahne KAYDEDİLMEDEN kapatılacak. `_Arena`'nın tilemap'ini/objelerini DEĞİŞTİRME. playModeStartScene'e dokunma. "Hiçbir şey birbirine karışmamalı."

## Bağlam / amaç
Akademik rapora "veri-güdümlü oda sistemi" görsel kanıtı: 6 Act-1 odasının TEMİZ Unity render'ları + 2×3 contact sheet + _Arena oda görseli (yeni Şekil 6). Kullanıcı: "bütün odaların resimleri" + JSON (JSON entegrasyonu writer'da, sen sadece görselleri üret).

## Capture yöntemi (memory-kanıtlı)
- Saf Unity render, masaüstü ASLA çıkmaz: `manage_camera action=screenshot capture_source=game_view include_image=false max_resolution=1024 output_folder=...`. (Editor pencereleri çekilemez — sadece game/scene view; bu görev için game_view yeterli.)
- Alternatif kanıtlı yöntem `RoomLoaderMenu.cs` içinde: ortho cam, size 14, pos (16,12,-10), solid bg — istersen execute_code ile bunu replikle (render-texture → PNG). Hangi yöntem daha temiz kadrajsa onu kullan.

## Oda yükleme API (execute_code — dosya-dialog YOK)
`RIMA.Editor.Map.RoomLoaderMenu.LoadRoomJsonToActiveScene(string jsonPath)` → odayı AKTİF sahnenin floor tilemap'ine kurar (pool+registry otomatik bulunur). Floor tilemap yoksa kod kendi oluşturur (`CreateFloorTilemap`).

## 6 Act-1 odası
```
Assets/Data/Map/Act1_ShatteredKeep/json/act1_entry_hall.json        (Entry Hall, 32x24, spawn)
Assets/Data/Map/Act1_ShatteredKeep/json/act1_west_chamber.json
Assets/Data/Map/Act1_ShatteredKeep/json/act1_east_corridor.json
Assets/Data/Map/Act1_ShatteredKeep/json/act1_treasure_vault.json
Assets/Data/Map/Act1_ShatteredKeep/json/act1_north_antechamber.json
Assets/Data/Map/Act1_ShatteredKeep/json/act1_shattered_throne.json  (Throne, ending)
```

## Adımlar
1. Mevcut durumu kaydet (aktif sahne = _Arena). **Yeni boş scratch sahne aç** (`manage_scene create` veya `EditorSceneManager.NewScene` additive değil) — _Arena'ya DOKUNMA. Grid+Floor tilemap garanti et.
2. Her oda için: `LoadRoomJsonToActiveScene(path)` → recompile/refresh bekle → game_view capture → `STAGING/report/figures_2026-06-18/rooms/<room_id>.png`. Her yüklemeden önce önceki oda içeriğini temizle (RoomLoader zaten `PhaseH_RoomContent`'i yeniler; tilemap'i de temizle ki odalar üst üste binmesin).
3. **2×3 contact sheet** üret (Python PIL): 6 oda thumbnail'ı, her birinin altında etiket = display_name + boyut (örn "Entry Hall · 32×24"). Çıktı: `STAGING/report/figures_2026-06-18/fig_rooms_grid.png`. Temiz beyaz/slate bg, okunaklı etiket.
4. **_Arena Şekil 6:** scratch'i KAPAT (kaydetme) → `_Arena`'yı yeniden aç → game_view capture → `STAGING/report/figures_2026-06-18/fig_arena_room.png`. (Mevcut `Assets/Screenshots/report_arena_gameview_test.png` örnek; daha iyi/aydınlık kadraj çekebilirsen çek.)
5. read_console (0 error hedef). Aktif sahne _Arena + isDirty=false doğrula (LEAK YOK).

## Çıktı
`STAGING/_process/2026-06/ROOM_CAPTURE_DONE.md` — üretilen PNG yolları (6 oda + grid + arena), her odanın boyutu, contact-sheet yolu, console durumu, LEAK-YOK teyidi (aktif sahne+dirty).
Bana dönüş ≤10 satır: kaç oda çekildi + grid yolu + arena yolu + console + leak-yok teyidi.
