# LIVE-TEST RECIPE — beat: `f2-buildmode`

> Amac: F2 ile Build Mode'un GERCEK input'la (tus + fare) acildigini, prop'un GERCEK
> tiklamayla yerlestigini, prop'un kalici oldugunu + oyunun devam ettigini, panelin
> render edildigini KANITLAMAK. Overlay UI MCP screenshot'a CIKMAZ -> data-proof sart.
> DERS REWARD-02: `*ForValidation` API'leri F2-toggle'i + fare click'ini + UI-hover guard'ini
> ATLAR. Onlarla "GREEN" almak yanlis-pozitiftir. Asagida acikca isaretli.

---

## LAUNCH (dev-direct `_Arena`)
- Sahne: `Assets/Scenes/_Arena.unity`. Bu sahneyi Editor'de AC (full-flow MainMenu DEGIL).
- Play'e bas. `_Arena` GameEntryScenes degil -> `DirectorMode.Bootstrap` calisir (DirectorMode.cs L150-167)
  ve `BuildModeController.Bootstrap` (RuntimeInitializeOnLoadMethod AfterSceneLoad) controller'i kurar.
  Bu sayede F2 AKTIF. (Full-flow MainMenu'de DirectorMode bootstrap'i GameEntryScenes guard'iyla
  ATLANIR -> `EnterBuildMode` L224-228 `DirectorMode.Instance == null` ile erken doner -> F2 INERT.)
- Oyuna gir: oda yuklensin, oyuncu spawn olsun, RoomRunDirector.CurrentTemplate dolu olsun
  (prop yerlesimi + working-copy bunu ister; bos ise CreateWorkingTemplate L344 warning verir).

## ADIMLAR (gercek input — bypass YOK)
1. Sahnede oyuncu hareket edebildigini dogrula (1-2 sn normal gameplay; timeScale=1).
2. **F2'ye bas.** -> Build Mode acilmali. (Toggle: BuildModeController.Update L180-181 -> Toggle -> EnterBuildMode.)
3. Sol BUILD panelinde bir prop kartina TIKLA (asset browser, Props sekmesi default acik).
4. Dunyada UI panelinin USTUNDE OLMAYAN bir hucreye fare goturup ghost'un YESIL oldugu yerde **sol-tikla** (yerlestir).
5. **F2'ye tekrar bas** -> Build Mode kapanmali, oyun (timeScale=1) DEVAM etmeli.
6. Yerlestirilen prop'un sahnede HALA durdugunu + oyuncunun yeniden hareket ettigini dogrula.
7. (Opsiyonel TILE) F2 ile tekrar gir -> ust segmented'da **TILE**'a tikla -> bir hucreye sol-tikla (floor paint).

## BEKLENEN (somut)
- F2 -> `BuildModeController.IsActive == true`; kamera geri cekilir (ortho ~9); timeScale=0;
  DirectorMode state = Director; sol BUILD paneli + (TILE secilince) sag TILE paneli gorunur.
- Prop tiklayinca: `BuildPlacementController.PlacedCountForValidation()` 0 -> 1; sahnede `prop_*` GameObject olusur.
- F2 ile cikinca: IsActive=false, timeScale=1, prop SAHNEDE KALIR (placed instance destroy edilmez;
  sadece ghost+grid+command-history dusulur — SetBuildModeActive(false) L199-211).
- Diger UI canvas'lari (HUD/reward) Build Mode'da gizlenir, cikinca geri gelir (Hide/RestoreOtherUiCanvases).

## DATA-PROOF (execute_code — overlay UI screenshot'a cikmaz, ZORUNLU)
F2'ye GERCEK bastiktan SONRA (adim 2/4/5'in ardindan) durumu OKU; ASLA EnterBuildMode/ForValidation cagirma:
- `BuildModeController.IsActive` (bool) -> giris/cikis kaniti.
- `BuildModeController.Instance.WorkingTemplate != null` -> session working-copy kuruldu.
- `Time.timeScale` -> giriste 0, cikista 1.
- `DirectorMode.Instance.State` -> giriste `Director`.
- Panel render kaniti (GERCEK GameObject'ler, controller alanlari degil):
  `GameObject.Find("BuildPaletteCanvas")` -> activeInHierarchy=true; alti `Status`/`Tabs`/`GridRow` mevcut.
  (TILE icin) `GameObject.Find("BuildTileBrushCanvas")` activeInHierarchy=true.
  Canvas `.enabled==true`; bir TMP_Text bul + `.text` bos degil (okunabilirlik kaniti).
- Prop yerlesim kaniti (GERCEK click'ten SONRA oku):
  `BuildPlacementController.Instance.PlacedCountForValidation()` artmis olmali.
  Sahnede `GameObject.Find` ile `prop_` prefix'li nesne aktif.
- Persist + oyun-devam kaniti (F2 cikis SONRASI):
  prop GameObject hala aktif (destroy edilmemis) + `Time.timeScale==1` + oyuncu PlayerController aktif.

## SCREENSHOT
- Dunya/prop gorseli: `scene_view` capture (game-view ScreenCapture 9.7.3 fix'i DENENEBILIR; patlarsa scene_view).
  Yerlestirilen prop sprite'i dunyada gorunmeli.
- Panel/HUD/ghost (overlay UI): SCREENSHOT'A CIKMAZ -> sadece yukaridaki DATA-PROOF gecerli, ekran goruntusu ARAMA.

## PASS / FAIL (olculebilir)
PASS hepsi dogruysa:
- F2 (gercek tus) sonrasi IsActive true; tekrar F2 sonrasi false.
- Giriste timeScale=0 + State=Director; cikista timeScale=1.
- BuildPaletteCanvas activeInHierarchy=true + en az 1 dolu TMP_Text.
- GERCEK fare click'iyle PlacedCount +1 ve `prop_*` GameObject olustu.
- F2 cikis sonrasi prop GameObject HALA aktif + timeScale=1 + oyuncu hareket edebiliyor.
FAIL: F2 hicbir sey yapmiyorsa / IsActive degismiyorsa / panel canvas yoksa-pasifse /
PlacedCount artmiyorsa / cikista prop kayboluyorsa / timeScale 0'da kaliyorsa.

## BYPASS-TUZAKLARI (REWARD-02 dersi — yanlis-GREEN'e nasil dusulur)
1. **`*ForValidation` ile "GREEN"**: `PlaceForValidation/SelectToolForValidation/EnterBuildMode()`
   dogrudan cagrilirsa F2-toggle, fare click ve **`IsPointerOverUi` hover guard'i** ATLANIR. Bu,
   "F2 calisiyor + click yerlestiriyor" tezini KANITLAMAZ. -> Yerlestirme MUTLAKA gercek fare sol-tik
   ile; toggle MUTLAKA gercek F2 ile. ForValidation sadece read-back (PlacedCount okumak) icin.
2. **Sessiz erken-return (no-op'u "calisti" sanmak)**: `EnterBuildMode` su durumlarda HICBIR SEY
   yapmadan doner -> UIManager.IsAnyOverlayOpen veya Draft aktif/pending (L218-221), DirectorMode.Instance
   null (L224), Camera.main null (L231). F2 bastin ama hicbir log/degisim yoksa BU guard'lardan biridir;
   "inert dogru" deme — once reward/draft/overlay KAPALI ve oda yuklu oldugundan emin ol, sonra tekrar dene.
3. **Full-flow ile test edip "F2 inert, demek bozuk" demek**: MainMenu'den girdiysen F2 ZATEN inert
   (tasarim). Beat'i SADECE dev-direct `_Arena`'da test et.
4. **UI uzerinden tiklamayi "place" sanmak**: panel uzerine sol-tik `IsPointerOverUi()` ile yutulur,
   PlacedCount artmaz — bu FAIL degil, panel-DISI bos hucreye tikla.
5. **Ghost KIRMIZI iken tiklamak**: gecersiz hucrede (walkable degil / footprint disi) click yerlestirmez;
   yesil ghost'lu hucreye tikla. Kirmizida PlacedCount artmazsa bu beklenen, bug degil.
6. **Cikista prop'u command-stack ile karistirmak**: cikista undo-history dusulur ama placed instance'lar
   SAHNEDE KALIR. "history temizlendi" = "prop silindi" DEGIL; prop GameObject'in aktif kaldigini ayrica dogrula.
