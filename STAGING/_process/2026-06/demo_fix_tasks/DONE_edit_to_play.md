# DONE — Deterministik Edit-to-Play centerpiece (first-room lock)

Tarih: 2026-06-17 · Sonuc: **PARTIAL DONE** (deterministik oda mekanizmasi YAPILDI + dogrulandi; prop-render yari = SCOPE-GATE) · Console 0 error (1 pre-existing LIFE-01 teardown warn'i, benim degisikligimle ilgisiz) · `_Arena.unity` + `room_current.json` DOKUNULMADI (dirty=False).

## DETERMINISTIK-LOAD MEKANIZMASI (kok bulgu — onceki BLOCKED'in cozumu)
- **Onceki BLOCKED dogru teshis koymus AMA yanlis dosyayi isaret etmis.** `_Arena` canli yolu = `RoomRunDirector` + `IsoRoomBuilder` (RoomTemplateSO). `room_current.json` / `LiveRoomReloader` = AYRI yol (`RoomLoader` / Tool.exe) — `_Arena` sahnesinde `RoomLoader` SIFIR kez kullaniliyor (grep=0). Yani room_current.json pre-bake `_Arena` demo'sunda HICBIR runtime tarafindan OKUNMAZ → o yolla pre-bake = sahte kanit olurdu.
- **Gercek non-determinizm kaynagi:** `RoomRunDirector.BeginRun()` `forceDemoSequence=false` iken her run `runSeed=Random.Range(...)` ile yeniden atiyor → `roomBank.Pick(Combat, runSeed+nodeId, ...)` ilk oda icin her run FARKLI template donduruyor (donut/cross/blob).
- **EKLENEN (cerrahi, branching'e DOKUNMADAN):** `RoomRunDirector.cs`'e 2 inspector alani (`deterministicFirstRoom=true`, `firstRoomSeed=12345`) + `BuildCurrentRoom` icinde tek kosul: START node (`CurrentNodeId==graph.startId`) template-pick'i `firstRoomSeed` kullaniyor; DIGER tum node'lar `runSeed+nodeId` (per-run random) KORUNUYOR. forceDemoSequence/branching-seed mantigi degismedi.

## DOGRULAMA (data-proof, _Arena dev-direct play)
- **Determinizm:** 3 bagimsiz run (stop+replay) → 3/3 ayni: `combat_large_cross_01` bounds=(0,0,26,18) validExits=3 templateProps=8.
- **Branching SAGLAM:** node0 PINNED (cross_01) iken node1 hala per-run degisiyor (lshape ↔ hourglass farkli seed'lerde) → run-map "her run degisen harita" tezi bozulmadi.
- **Start node HEP Combat:** 6 random graf → 6/6 startId=0 startType=Combat (child 2-3 degisken, pick child-count'a karsi stabil).
- **Combat temiz:** 2 dusman spawn, player var (pos 5.28,4.10), lifecycle=Combat, console 0 error.

## EDIT-TO-PLAY CONTINUITY KANITI
- **Oda-geometri continuity = CALISIYOR.** Screenshot pair (ayni oda her run):
  - `STAGING/_process/2026-06/demo_fix_tasks/edit_to_play_room_RUN1.png`
  - `STAGING/_process/2026-06/demo_fix_tasks/edit_to_play_room_RUN2.png`
  - (Iki shot ayni cross_01 floor/cliff; ust uste opening-kit draft kart-sirasi SHUFFLE = ayri RNG, oda DEGIL.)
- **BEFORE (non-deterministik, onceki ajan):** `arena_dressing_RUNTIME_check.png` (prop'lar farkli/buyuk odada).

## PROP-RENDER YARISI = SCOPE-GATE (BLOCKED, kasitli)
- Start node Combat → `IsoRoomBuilder.BuildProps` `spawnProps=false` ile ATLANIYOR (RoomRunDirector satir 347-348, F1 fix). Olculdu: cross_01'in 8 templateProps'u play'de `renderedPropObjs=0`.
- `spawnProps=false` DOKUNMA-listesinde (F1 fix, soft-lock geri gelir) → override ETMEDIM.
- DECISION (`DEMO_BITIRME_DECISION` item 3+4): doseme prop'lari **KULLANICI elle** koyar (scene-child, builder temizlemeyen top-level root), Combat-disi/scene-child yoluyla — "verified sahneyi remote-agent bozmasin". Bu adim bilinçli olarak agent-disi. Mekanizma artik hazir: oda kilitli oldugu icin sabit dunya-poz prop'lar her run ayni floor'a oturur (onceki BLOCKER cozuldu).

## room_current.json
- Mevcut (`Assets/StreamingAssets/live/room_current.json`, 148KB, git'te commit'li, `PlayableArena_Test01`, schema 1.1) = Tool.exe/live-editor artifact'i. `_Arena` demo bunu OKUMAZ. Yenisini yazmak yanlis-kanit olurdu → DOKUNMADIM.

## DEGISEN DOSYA
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` (+11 satir alan, +tek pick-seed kosulu). Default ON (sahne re-serialize gerektirmez; C# default'lari devreye girer — dogrulandi).

## VERDICT
- Deterministik ilk-oda mekanizmasi: **CALISIYOR** (3/3 ayni oda, branching korunmus, console 0). Onceki BLOCKER cozuldu.
- Edit-to-Play oda-continuity: **CALISIYOR**.
- Prop-continuity (gorsel doseme): **kullanici elle adimi bekliyor** (Combat spawnProps gate = korunan F1 fix; agent doseme-yapmaz karari).
- Restore: playModeStartScene→MainMenu geri yuklendi, _Arena dirty=False (no debug-state-leak).
