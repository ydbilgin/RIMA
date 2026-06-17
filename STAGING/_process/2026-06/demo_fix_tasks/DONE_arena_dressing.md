# DONE — _Arena prop dressing (ILK PASS) → **BLOCKED**

Tarih: 2026-06-17 · Sonuc: **BLOCKED** (kok-neden: ilk combat odasi non-deterministik) · Scene HEAD'e geri alindi (sifir net degisiklik) · Console 0 error/0 warning.

## Ne denendi
6 PixelLab prop (BrokenPillar, StatueFragment, IronBrazier, Barrel, RubblePile, ClothBanner) `_Arena.unity`'ye dekoratif scene-child olarak konuldu; her biri SpriteRenderer + worldSprite + PropSorterRuntime ("Props" sorting layer + Y-sort, runtime SpawnSingle deseniyle birebir). Solid 4'une kucuk merkezi BoxCollider2D (0.45x0.30, Default layer = boundary tilemap ile ayni fizik grubu), floor-decor 2'si collider'siz.

## KOK-NEDEN (neden BLOCKED)
1. **`IsoRoomBuilder/Props` runtime'da SILINIYOR.** `IsoRoomBuilderTester.Start()` → `builder.Build()` → `ClearPrevious()` → `DestroyChildren(propsContainer)`. Bu kapsayicidaki loose scene-child'lar play'de yok oluyor. (Cozdum: yeni top-level `ArenaDressing` root'una tasidim — builder bunu temizlemiyor.)
2. **ASIL BLOKER — ilk oda non-deterministik.** _Arena'da `RoomRunDirector` serialized degerleri: `forceDemoSequence=FALSE` + `runSeed` her `BeginRun`'da `UnityEngine.Random` ile yeniden atiliyor. `roomBank.Pick()` deterministik ama girdi seed her run farkli → ilk combat odasi her oyunda DEGISIYOR: olculdu `combat_large_donut_01` (373 cell, 3 cikis) → `combat_large_cross_01` (2 cikis) → `combatlarge_organic_blob_01`. Floor geometrisi+spawn'lar her run tamamen farkli. Sabit dunya-pozisyonlu prop'lar bir run'da kenara oturuyor, sonraki run'da bosluga (void) dusuyor. Kanit: donut'a hizalanan 6 prop, sonraki cross-run'da 6/6'sindan 5'i `onFloorCell=False`.
3. Bu, CURRENT_STATUS RESUME'unun kilitli sirasindaki on-kosulu dogruluyor: **"Arena doseme ... + room_current.json pre-bake/commit [user+cx]"**. Oda kilitlenmeden prop'lar hizalanamaz.

## Neden ileri gidilmedi (ACTIVE RULES)
- RoomRunDirector seed/branching mantigina DOKUNMADIM (gameplay logic, scope disi; run-map "her run degisen harita" tezini bozar).
- 5 prop'u bosluga birakmak estetik cope = bos odadan kotu → kabul edilmez.
- "Nav bozulursa geri al" → nav riski ship olmadan once geri alindi.

## Nav-test
Calisan oda(lar)da Player Dynamic Rigidbody2D + CapsuleCollider2D (layer "Player"), Player<->Default carpisma=TRUE (solid prop'lar fiziksel bloklar, boundary tilemap ile ayni grup). Tam wave/soft-lock testi YAPILMADI — prop'lar zaten odaya hizalanamadigi icin anlamsizdi; BLOCKER hizalama asamasinda yakalandi.

## Temizlik (no-debug-state-leak)
- `ArenaDressing` root + 6 prop silindi; `IsoRoomBuilder/Props` orijinal 1 child'a dondu.
- Gecici null'lanan `playModeStartScene` → `MainMenu.unity`'ye geri yuklendi.
- `_Arena.unity` `git checkout` ile HEAD'e geri alindi (byte-identical, git status temiz); editor diskten yeniden yuklendi. scene dirty=False.

## UNBLOCK yolu (siradaki)
1. Ilk combat odasini KILITLE: ya `forceDemoSequence=true` (sabit demo dizisi) ya da `room_current.json` pre-bake/commit (RESUME planı, user+cx).
2. Oda sabitlenince: bu rapordaki donut-rim pozisyonlari (W -5.3,5.3 · NW -3.4,9.4 · N 3.8,11.4 · NE 8.2,10.5 · SW -2.9,2.6 · E 9.1,8.2) yeniden uygulanir; prop'lar `ArenaDressing` top-level root'a (builder temizlemez) konur; ardindan tam nav+wave playtest.

## Ekler
- Before screenshot (bos oda kaniti): `STAGING/_process/2026-06/demo_fix_tasks/arena_dressing_BEFORE.png`
- Edit-mode after (saved-scene diamond'da hizali): `STAGING/_process/2026-06/demo_fix_tasks/arena_dressing_AFTER_editmode.png`
- Runtime mismatch kaniti (prop'lar farkli/buyuk odada): `STAGING/_process/2026-06/demo_fix_tasks/arena_dressing_RUNTIME_check.png`
