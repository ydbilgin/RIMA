# LIVE-TEST RECIPE — beat: room-transition-f1

> Oda gecisi saglikli mi + F1 stale-reward-leak: onceki odadan kalan reward objesi yeni odada kaliyor mu.
> Iki temizlik yolu: RoomRunDirector.DestroyActiveReward (TEK takipli ref) vs RuntimeRoomManager.ClearActiveRewards (sahne-genel sweep).

## KOD GERCEGI (recipe'nin dayanagi)
- `_Arena`'da LIVE manager = **RoomRunDirector** (GO `RoomRunDirector`, `forceDemoSequence: 1`, `buildOnStart: 1`, `useFixedDemoCamera: 1`, `depthCount: 5`). RuntimeRoomManager `_Arena`'da CANLI COMPONENT DEGIL (sadece referans).
- Oda gecisi yolu: kapi trigger (`RoomRunExitDoorTrigger`, interact = **G** tusu, oyuncu menzilde) -> `TryEnterDoor(choiceIndex)` -> `AdvanceTo` -> **`BuildCurrentRoom()`**.
- `BuildCurrentRoom()` temizligi (satir ~311) = **sadece `DestroyActiveReward()`** -> yalniz `activeReward` (TEK takipli RewardPickup ref) yok edilir. **Sahne-genel `FindObjectsByType<RewardPickup>` sweep YOK.**
- Sahne-genel sweep yalniz `RuntimeRoomManager.ClearActiveRewards()` (RRM.cs ~971-982) icinde -> bu yol `_Arena` demo akisinda CALISMAZ.
- Reward toplama (gercek yol): `RewardPickup` collider'a oyuncu girer (`OnTriggerEnter2D`/`OnTriggerStay2D` -> `playerInRange=true`) + **G** tusu -> `Collect()` -> `WasCollected=true` -> draft acilir. (NOT: `ForceCollect` = timeout auto-grant; bu bypass'tir, asagiya bak.)
- Reward spawn = `RoomClearSequence` icinde, oda temizlenince (`SpawnRewardPickup` satir 1261). MapFragment ise `RuntimeRoomManager` ozel; director demo akisinda fragment spawn etmez -> F1 odaginda asil risk = **RewardPickup leak** (collect EDILMEDEN gecis denenirse).

## LAUNCH (dev-direct _Arena)
1. Unity Editor'de sahne ac: `Assets/Scenes/_Arena.unity` (manage_scene load).
2. Play'e bas (manage_editor play). `buildOnStart: 1` -> RoomRunDirector otomatik ilk odayi kurar; acilis kit-draft'i acilir.
3. Acilis draft'ini GERCEKTEN ekrandan bir kart secerek kapat (1/2/3 ya da tiklama). Bypass yok: draft acik kalirsa kapi akisi farkli davranir.
> NOT full-flow DEGIL: MainMenu'den girilmez; F2/backquote burada AKTIF olur (dev-direct) ama bu beat F2 KULLANMAZ.

## ADIMLAR (gercek input, bypass YOK)
A. Oda 1: dusmanlari WASD + sol-klik (LMB) ile gercekten oldur, oda temizlensin (clear slow-mo blip gorunur).
B. Oda temizlenince merkeze RewardPickup (chest) spawn olur.
C. **KRITIK F1 ADIMI — reward'i KASTEN BIRAK**: chest'i TOPLAMA (G'ye basma). Bunun yerine, RewardAutoCollect timeout'tan ONCE (timeout`>0` ise hizli ol) acilan exit kapisina yuru.
   - Eger kapi reward toplanmadan acilmiyorsa (akis reward'i bekliyor): bu da bir veri -> "gecis reward'a kilitli" notu al, sonra alternatif: reward'i G ile topla (gercek), draft'i SEC, kapi acilinca gec.
D. Acik kapinin menziline gir -> prompt cikar ("enter rift") -> **G** tusu ile odayi gec.
E. Oda 2 kurulduktan SONRA durakla. Burada data-proof al (asagi).
F. (Tekrar) Oda 2'yi temizle, reward'i bu sefer GERCEK G ile topla, draft sec, Oda 3'e gec -> ardisik gecis temizligini dogrula.

## BEKLENEN (somut)
- Oda 2'ye gecince sahnede **0 adet** onceki-odadan-kalan RewardPickup olmali.
- Aktif odada en fazla 1 RewardPickup (yeni odanin kendi reward'i, o da ancak oda temizlenince).
- `RoomRunDirector.activeReward` Oda 2 build aninda **null** (DestroyActiveReward cagrildi).
- Onceki odanin chest GameObject'i Destroy edilmis (artik hierarchy'de yok).

## DATA-PROOF (execute_code — overlay/runtime state ZORUNLU)
Oda 2 kurulduktan sonra execute_code ile OKU:
1. `UnityEngine.Object.FindObjectsByType<RIMA.RewardPickup>(FindObjectsSortMode.None).Length` -> sahnedeki TUM RewardPickup sayisi. (tip ad: `RIMA.RewardPickup`, dosya `Assets/Scripts/Core/RewardPickup.cs`.)
   - Her biri icin: `go.name`, `go.activeInHierarchy`, `WasCollected` (public getter), `transform.position`.
2. RoomRunDirector private `activeReward` -> reflection ile oku (`typeof(RoomRunDirector).GetField("activeReward", BindingFlags.NonPublic|Instance)`); null mi?
3. `FindObjectsByType<RIMA.Encounter.MapFragment>` veya `RIMA...MapFragment` sayisi (tam namespace'i grep ile dogrula) -> 0 olmali (director demo akisinda fragment yok; varsa leak).
4. RoomRunDirector lifecycle state (reflection: `lifecycle` field -> `.State`) -> Oda 2'de `Combat`/build sonrasi beklenen state.
> Overlay UI (HUD reward prompt, draft paneli) MCP screenshot'a CIKMAZ -> bu beat'in PASS/FAIL'i TAMAMEN data-proof'a dayanir.

## SCREENSHOT
- Dunya gorseli: `scene_view` capture (Oda 2 dunyasi; chest gorunmemeli). game-view ScreenCapture 9.7.3 fix'i denenebilir ama overlay yine cikmaz.
- Reward/prompt/draft (overlay) icin screenshot DEGIL -> "data-proof" yaz.

## PASS / FAIL (olculebilir)
- **PASS**: Oda 2 build sonrasi `FindObjectsByType<RewardPickup>().Length == 0` (oda henuz temizlenmedi) VE `activeReward == null` VE onceki chest GO destroyed VE MapFragment sayisi == 0.
- **FAIL (F1 leak)**: Oda 2'de onceki odanin RewardPickup'i HALA mevcut (`Length >= 1` ve pozisyon Oda 1 koordinati / `WasCollected==false`) -> stale-reward-leak DOGRULANDI (cunku BuildCurrentRoom sahne-genel sweep yapmiyor, sadece takipli ref'i siliyor).

## BYPASS-TUZAKLARI (REWARD-02 dersi — yanlis-GREEN'e nasil dusulur)
1. **ForceCollect maskesi**: reward'i G ile toplamadan timeout'a birakirsan `ForceCollect()` menzilsiz auto-grant eder + chest'i Collect sonrasi yok eder -> leak GIZLENIR (yanlis GREEN). Bu beat'i test ederken reward'i ya GERCEK G ile topla ya da ACIKCA toplamadan birak; ForceCollect tetiklenirse logta `[Reward] ForceCollect` gorunur -> testi GECERSIZ say, tekrar et (timeout'tan once gec).
2. **AdvanceTo/BuildCurrentRoom'u execute_code'dan direkt cagirmak** = bypass. Gecis GERCEK G-kapisi ile olmali; aksi halde kapi/menzil/lifecycle yollari atlanir.
3. **ClearActiveRewards'a guvenmek**: o sahne-genel sweep `_Arena`'da CALISMIYOR (RuntimeRoomManager canli degil). "ClearActiveRewards var, leak olmaz" demek yanlis pozitif. Yalniz `DestroyActiveReward` (tek ref) calisir.
4. **Tek-frame okuma**: build hemen ardindan oku; ama draft acikken/coroutine kosarken erken okursan eski reward Destroy edilmeden gorunur -> kararsiz sonuc. Oda 2 build LOG'u (`[RoomRunDirector] Built node ...`) goruldukten SONRA oku.
5. **Sadece scene_view'e bakmak**: chest scene'de gorunmese bile reward GO inactive olarak yasayabilir -> mutlaka `FindObjectsByType` + `activeInHierarchy` say, goz kararina guvenme.
