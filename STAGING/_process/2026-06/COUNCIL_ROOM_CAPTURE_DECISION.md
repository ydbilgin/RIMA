# LOCKED KARAR — Act-1 odalarını cliff-island olarak yakalama (council 3/3 sentez)

Kaynak: council cx + ax Pro + ax Flash (`_council_*_roomcap.md`). Orchestrator (Opus 4.8) sentezi.

## Konverjans (kanıtlı)
- Canonical pipeline: `RoomRunDirector.BuildCurrentRoom()` → `IsoRoomBuilder.Build(template)` → floor + **BuildCliffs(floorCells)** (procedural cliff, JSON-guid'den DEĞİL) + boundary + markers + lighting + props (`IsoRoomBuilder.cs:96-137`, `:458-501`).
- Ada-görünümü `_Arena` sahne RIG'inden: L0_Void (`_Arena.unity:2489-2522`) + Main Camera ortho/dark (`:2132-2174`) + Global Light 2D (`:1527-1550`). Bare sahnede bunlar YOK → ilk denemenin düz/çirkin çıktısının sebebi.
- Legacy `RoomLoaderMenu.LoadRoomJsonToActiveScene` → `RoomLoader.LoadJsonToScene` SADECE floor/walls/props boyar, cliff/rig YOK (`RoomLoader.cs:46-51`). İlk builder'ın tuzağı buydu.

## REDDEDİLEN yollar
- **Play + LiveRoomReloader (ax Pro önerisi):** REDDEDİLDİ. cx kanıtı: `LiveRoomReloader.ApplyCliffTiles` yalnız JSON hücreleri tile-guid taşırsa çalışır (`:206-229`) → Act1 JSON'larında **no-op, cliff üretmez**. ax Flash: runtime-dep + oyuncu-pozisyon + over-engineering. Demo-yarın için risk.
- **Bare/klon scratch sahne:** REDDEDİLDİ — _Arena rig'i (void/light/cam) olmadan ada görünmez.

## LOCKED yaklaşım (editor-mode revert-capture, no-leak)
1. **_Arena'yı editörde aç** (play YOK). Build sonrası sahneyi **KAYDETME** → kapat/revert (no-leak, git temiz).
2. Odayı canonical IsoRoomBuilder ile kur. İki aday API (executor hangisi gerçekten varsa onu kullansın, doğrula):
   - `RoomTemplateBuildUtility.BuildInArena(template)` (cx: `RoomTemplateBuildUtility.cs:15-60,97-114`)
   - mevcut **`MapScreenshotTool`** (ax Flash: `Assets/Scripts/Editor/DevTools/MapScreenshotTool.cs`) — zaten _Arena+IsoRoomBuilder+kamera-odak+PNG, no-leak mantığı.
3. **Oda kaynağı:** ÖNCE oyunun gerçek kullandığı `RoomTemplateSO` asset'lerini bul (RoomBank). Varsa ONLARI kullan (en faithful, conversion-sız). Yoksa Act1 JSON → transient `RoomTemplateSO` çevir.
4. Capture: `manage_camera game_view` (veya tool'un PNG'i) → `STAGING/report/figures_2026-06-18/rooms_island/<room_id>.png`.

## VERIFY-FIRST GATE (hata önleme — ZORUNLU)
İLK odayı (entry_hall) kur+capture → **ada-görünümü doğrula** (cliff kenarları + void bg + ışık VAR mı?). 
- Ada görünüyorsa → kalan 5 odayı batch'le + 2×3 island contact-sheet (`fig_rooms_island_grid.png`).
- Ada GÖRÜNMÜYORSA → DUR, `BLOCKED` yaz (sebep + denenen API), 6 yanlış PNG üretme. Orchestrator görsel review yapacak.

## Figür anlatısı (council 3/3: İKİSİ BİRDEN)
- **Schematic grid** (mevcut `fig_rooms_grid.png`, JSON→tilemap) = veri-güdümlü KANIT → rapora "veri" figürü olarak KALIR.
- **Island captures** = oyun-gerçeği görünümü → yan yana/ayrı figür. Eski bare `rooms/*.png` ÇÖPe (game-truth değil).

## No-leak teyidi (HARD)
İş bitince: aktif sahne `_Arena` + isDirty=false, hiçbir sahne kaydedilmemiş, console 0 error. "Hiçbir şey birbirine karışmamalı."
