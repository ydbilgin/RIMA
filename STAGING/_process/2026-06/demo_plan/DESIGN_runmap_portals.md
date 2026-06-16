# DESIGN — G3: Run-Map Portal Minimap (alt-bar portal HUD)

> Planlama dokumani (read-only ajan). Demo 19 Haziran. 8-yon sprite canon LOCKED;
> oda canon = cliff-tile yuzen ada / `_Arena.unity` dev-direct. Bu dokuman G2 (blue-beam
> teleport gecisi) ile entegre olur ve mevcut DungeonGraph branching uzerine kurulur.
> Gercek kod adlari grep ile dogrulandi (paket ornek sinif adlari KULLANILMADI).

## 0. Gercek mimari (grep + read dogrulamasi)

Calisan run-map sistemi (LIVE, `_Arena`'da kayitli):
- `Assets/Scripts/MapDesigner/Room/Runtime/DungeonGraph.cs` — `DungeonNode{ id, depth, roomType, childIds }`; `DungeonGraph.Generate(seed, depthCount)` branching DAG (mid-mix Combat50/Elite20/Chest15/Merchant15, no-consec-Elite, ForceOneMerchant) + `BuildDemoSequence()` lineer fallback.
- `Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs` — run sahibi. **Veri kaynagi kapilari ZATEN var:**
  - `public List<DungeonNode> CurrentChoices` => `graph.ChildrenOf(CurrentNodeId)` — **N children = N portal.** (G3'un tam ihtiyaci.)
  - `public int CurrentNodeId`, `public DungeonNode CurrentNode`, `CurrentRoomType`, `IsRunComplete`.
  - `public void TryEnterDoor(int choiceIndex)` — lifecycle `MarkAdvancing()` -> `AdvanceTo(choiceIndex)` -> `BuildCurrentRoom()`. **Tek dogru gecis API'si.** (G3 tiklamasi BUNU cagirmali, AdvanceTo'yu degil — lifecycle guard'i atlamamak icin.)
  - Kapilar = `RoomRunExitDoorTrigger` (her child = bir kapi; G ile girilir, `choiceIndex` tasir).
- `Assets/Scripts/MapDesigner/Room/Runtime/RunMapOverlay.cs` — **M tusu** IMGUI tam-ekran StS overview (tum DAG, depth satirlari, current-node highlight, `ColorFor(roomType)` renkleri). Calisiyor, demo'da verified.
- `Assets/Scripts/UI/Map/*` (`MapPanelUI/MapNodeUI/MapGraphData/MapProgressController`) = **AYRI, BAGLI DEGIL** uGUI "tablet" preview; kendi `MapGraphData.CreateFiveNodePlaceholder()` placeholder'ini cizer, DungeonGraph'a wire EDILMEMIS (eski "enum koprusu" blokeri). **G3 icin bunu yeniden kullanma/yeniden kurma** — terkedilmis kol, demo riskini artirir.

**Sonuc:** G3 = ne IMGUI full-overlay'i sismanlatmak ne de tablet-panel'i diriltmek; **YENI hafif alt-bar HUD** (`CurrentChoices` ile beslenir). Bu en dusuk-risk yol.

## 1. UI layout — ekran alti portal bar

- **Konum:** ekran alti orta, safe-area icinde (alt kenardan ~24px yukarida). HUD HP/skill barlari ile cakismamali — onlar genelde alt-sol/alt-orta; portal bar'i HP barinin **uzerine** (y offset ~96px) ya da alt-saga koy; final yer kullanici gorsel teyidiyle.
- **Yapi:** yatay merkezli flow. **N node = N portal** (CurrentChoices.Count). Tipik 1-3 (DAG `NodeCountAtDepth` mid-depth'te 2-3 child; depth0/son-depth 1).
- **Node ikonu:** 56-64px kare/yuvarlak; ortada roomType sembolu, altinda kisa TR etiket (SAVAS/ELIT/SANDIK/TUCCAR/BOSS). Renk = `RunMapOverlay.ColorFor` paleti ile **birebir ayni** (tutarlilik): Combat slate, Elite mor, Boss kirmizi-ember, Chest amber, Merchant teal. Act-1 canon (slate/void-mor/ember) ile uyumlu.
- **Bar gorunurlugu:** SADECE lifecycle `DoorOpen` (oda temizlendi + reward alindi) iken goster/fade-in. Combat sirasinda gizli (bilgi kirliligi yok, Director-bleed dersi). `IsRunComplete` -> bar gizli (post-boss son oda, child yok).
- **Current/hover feedback:** hover -> node buyur + cyan glow + roomType tooltip (ileride). Bos durum (henuz acilmamis) -> bar yok.
- **Etiket:** bar ustunde ince "KOSU YOLU — sonraki oda sec" alt-yazi (Loc key, TR+EN Loc.cs mevcut). M-overlay full-harita ile ayni dil.

## 2. Veri kaynagi

- **Tek kaynak:** `RoomRunDirector.CurrentChoices` (= `graph.ChildrenOf(CurrentNodeId)`). Her `DungeonNode.roomType` -> ikon + renk; index = portal/door `choiceIndex`. **Bar node sayisi = kapi sayisi = child sayisi** (3 child -> 3 portal node). Hicbir yeni graph/ekonomi gerekmez.
- **Senkron:** bar, `RoomRunExitDoorTrigger` kapilariyla **birebir ayni `choiceIndex` siralamasini** kullanir (kapilar `ConfigureExitDoors` icinde `i` index'iyle Configure ediliyor). Node[i] tiklama -> ayni hedef oda (kapi[i] ile birebir).
- **Refresh:** `BuildCurrentRoom` sonu / `OpenExitDoors` aninda bar yeniden cizilir. Yeni public hook gerekirse: `RoomRunDirector.RoomCleared` UnityEvent zaten var; ya da kucuk `event Action OnChoicesReady` ekle (DoorOpen'da fire). Minimal-degisiklik tercih.

## 3. Etkilesim modeli

Iki yol, ikisi de ayni `TryEnterDoor(choiceIndex)`'e baglanir (kanon tek-API):
- **(A) Walk-into (default, mevcut):** oyuncu kapiya yuruyup G — degismez, dokunma. Portal bar = navigasyon yardimcisi + hedef onizleme.
- **(B) Click-to-enter (G3 yeni):** node'a tiklama -> `director.TryEnterDoor(i)`. Sadece `DoorOpen` iken aktif; tiklama -> G2 blue-beam gecisi -> `AdvanceTo` -> yeni oda. Combat-during clickleri yok say (bar gizli zaten).
- **Guard:** `TryEnterDoor` lifecycle `MarkAdvancing()` ile korunmali — cifte-advance / yaris kosulu engellenir (zaten kod boyle). G3 ASLA `AdvanceTo`'yu dogrudan cagirmaz.

## 4. G2 (blue-beam teleport) entegrasyonu

- Tetikleyici hem walk-into-door hem portal-node-click, **AYNI** `TryEnterDoor(choiceIndex)`'e gider. G2 gecis efekti `AdvanceTo` -> `BuildCurrentRoom` ARASINA girer (oyuncu -> mavi isin/beam, kamera portala zoom, sonra harita yuklenir). 
- Onerilen kanca: `TryEnterDoor` -> (G2 coroutine `PortalTransition(choiceIndex)`) -> beam+zoom bitince `AdvanceTo(choiceIndex)`. Boylece G3 (secim) ve G2 (gecis film'i) tek noktada bulusur; portal bar sadece "hangi index" der, G2 "nasil gecilir" der.
- Mevcut `RoomRunDoorLocatorPulse` (acik kapi cyan halka) G2 beam'in baslangic-vfx'i olarak yeniden kullanilabilir (yeni asset gereksiz).

## 5. Asset bosluklari

- **Node ikonlari:** roomType basina 1 sembol (Combat/Elite/Boss/Chest/Merchant) — 5 ikon. Kismen var: `UI/RIMA/RIMA_UI_Node_Chest` (chest) repo'da. Eksik: Combat/Elite/Boss/Merchant node sembolu. Kaynak = PixelLab MCP (RIMA-stil, seffaf, no-dark-fantasy) ya da gecici renk+harf placeholder (demo-grade, M-overlay zaten harf+renk kullaniyor — paritede kal). **Demo icin renk+TR-etiket placeholder YETER**; ikonlar nice-to-have.
- **Bar cercevesi/chrome:** opsiyonel; IMGUI/uGUI duz panel + Act-1 cizgi yeterli. ChatGPT modular UI asset pack frame'leri buraya beslenebilir ama demo-blocker DEGIL.
- **Font:** Jersey10 SDF (repo'da, garbled-fix sonrasi saglikli).

## 6. Risk / oneri

- **DUSUK risk:** veri kaynagi (`CurrentChoices`) + gecis API'si (`TryEnterDoor`) + door-index esleme ZATEN var; G3 = ince bir sunum katmani. Yeni ekonomi/graph yok.
- **ORTA risk:** click-to-enter input — `_Arena` Input System (new) ile pointer; en guvenli demo = **walk-into (A) birincil + bar salt-onizleme**, click (B) varsa bonus. Director-bleed dersi: bar SADECE DoorOpen'da gorunsun, draft/Director overlay acikken gizle.
- **CANON:** M-overlay (full StS harita) KORUNUR; G3 alt-bar = onun mini/aksiyon-ici tamamlayicisi (M = strateji, bar = bir-sonraki-adim). Cakismasin: M acikken bar gizlenebilir.
- **YAPMA:** `UI/Map/MapPanelUI` tablet-panel'i diriltme (terkedilmis, wire degil); IMGUI overlay'i alt-bar'a zorlama (ayri concern); 4-cardinal/no-flip onerisi (REDDEDILDI).
- **Bagimlilik:** G3 etkilesim akisi (click->gecis) G2'ye baglidir; G2 yoksa portal bar yine **salt-onizleme** olarak demo-degerli (walk-into G zaten calisiyor).
