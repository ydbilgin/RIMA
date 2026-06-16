# LIVE-TEST RECIPE — Beat: `director-backquote`

> Odak: backquote (`) tusu DirectorMode'u toggle ediyor mu (NEREDE aktif/inert) + overlay-bleed fix calisti mi (Test state'de overlay sizmiyor mu).
> Unity'yi CALISTIRMADAN once: tek-Unity-ajan kurali. Overlay UI screenshot'a CIKMAZ -> DATA-PROOF sart.

## Kod gercekleri (dogrulandi — Assets/Scripts/UI/DirectorMode.cs)
- Backquote handler: `Update()` L196 -> `if (keyboard.backquoteKey.wasPressedThisFrame && !BuildModeController.IsActive) ToggleState();`
  -> backquote AKTIF sadece Build Mode KAPALI iken. Build Mode aciksa backquote INERT (desync-guard).
- F2 / quote (") = AYRI beat -> `BuildModeController.Update()` L180/184 (bu recipe'nin konusu DEGIL).
- Overlay-bleed fix:
  - `Awake()` L186: `SetOverlayVisible(false)` (dev-direct giriste TEMIZ baslar).
  - `SetState()` else-branch L324: Test state'e gecince `SetOverlayVisible(false)`; Director'a gecince L317 `SetOverlayVisible(true)`.
  - `SetOverlayVisible` L2080: `overlayCanvasGo.SetActive(visible)` (canvas adi = `Canvas_DirectorOverlay`, ScreenSpaceOverlay sortingOrder=950).
- Bootstrap: `RuntimeInitializeOnLoadMethod(AfterSceneLoad)` L150. `_Arena` GameEntryScenes{MainMenu,CharacterSelect} DISINDA -> DirectorMode self-bootstrap eder (GameObject "DirectorMode", DontDestroyOnLoad). State default = Test, ActiveTab = Spawn.

## LAUNCH (dev-direct)
1. Editor'de `Assets/Scenes/_Arena.unity` ac (`manage_scene` load) — full-flow MainMenu DEGIL.
2. Play'e bas (`manage_editor` play). DirectorMode AfterSceneLoad'da kendini kurar.
3. Baslangic durumu data-proof ile teyit (asagi): State=Test, overlay GIZLI.

## ADIMLAR (gercek input — BYPASS YOK)
- A0. (Baseline) Play basladiktan sonra hicbir tusa basmadan baslangic state'i oku.
- A1. backquote (`) bas (gercek Keyboard input). -> Director'a gecmeli, overlay GORUNMELI.
- A2. backquote (`) tekrar bas. -> Test'e donmeli, overlay GIZLENMELI (bleed fix).
- A3. (Inert dogrulama) Once F2 ile Build Mode'u AC (BuildModeController.IsActive=true), sonra backquote bas. -> backquote INERT olmali (state DEGISMEMELI). Ardindan F2 ile Build Mode'u kapat (temizlik).

> Not: Gercek tus enjekte etmek icin `execute_code` ile InputSystem event (`InputSystem.QueueStateEvent` / Keyboard backquote) kullan; UI buton click veya direkt `ToggleState()` cagrisi = BYPASS, kullanma (asagi bkz).

## BEKLENEN (somut)
- A0: `State == Test`, overlay GIZLI (`Canvas_DirectorOverlay.activeInHierarchy == false`), `Time.timeScale == 1`.
- A1: `State == Director`, overlay GORUNUR (`activeInHierarchy == true`), `Time.timeScale == 0`.
- A2: `State == Test`, overlay GIZLI (`activeInHierarchy == false`), `Time.timeScale == 1` (bleed fix kaniti).
- A3: backquote'tan ONCE ve SONRA `State` AYNI (Build Mode aciksa toggle olmaz).

## DATA-PROOF (execute_code — overlay UI screenshot'a cikmadigi icin ZORUNLU)
Her adimdan sonra oku:
- `DirectorMode.Instance.State` (enum: Director/Test).
- Overlay aktiflik: `GameObject.Find("DirectorMode")` altinda `Canvas_DirectorOverlay` transform'unu bul -> `go.activeInHierarchy`.
  (Alternatif: `DirectorMode` GO child'i `Canvas_DirectorOverlay`; `transform.Find` ile cek.)
- `Time.timeScale` (Director=0, Test=1 — state-overlay tutarliligi).
- A3 icin: `BuildModeController.IsActive` (backquote oncesi true oldugunu + backquote sonrasi State degismedigini birlikte logla).
- Ornek dogrulama: `Debug.Log($"state={DirectorMode.Instance.State} overlayActive={canvas.activeInHierarchy} ts={Time.timeScale} buildActive={BuildModeController.IsActive}")` -> sonra `read_console` ile oku.

## SCREENSHOT
- Overlay UI ScreenSpaceOverlay (sortingOrder 950) -> screenshot'a CIKMAZ -> overlay icin DATA-PROOF (yukari).
- Dunya gorseli istenirse: `scene_view` capture (Director state'de free-cam/timeScale=0 sahne). Game-view ScreenCapture 9.7.3 fix'i denenebilir ama overlay UI yine cikmaz; karar = scene_view + data-proof.

## PASS / FAIL (olculebilir)
- PASS: A0 overlay false + A1 overlay true & State=Director & ts=0 + A2 overlay FALSE & State=Test & ts=1 + A3 State degismedi (backquote inert).
- FAIL (herhangi biri):
  - A2'de overlay hala true (activeInHierarchy) -> bleed fix CALISMADI.
  - A1'de overlay false / State degismedi -> backquote handler kirik veya BuildModeController.IsActive yanlis true.
  - A3'te State degisti -> inert guard kirik (Build Mode iken desync riski).

## BYPASS-TUZAKLARI (REWARD-02 dersi — yanlis-GREEN nasil olur)
- TUZAK 1: `DirectorMode.Instance.ToggleState()` veya `SetState(Director)` execute_code'dan DIREKT cagirmak -> bu backquote yolunu (L196 input + `!BuildModeController.IsActive` guard) ATLAR. ForceCollect gibi sahte-yesil. KULLANMA — gercek tus event enjekte et.
- TUZAK 2: `*ForValidation` API'leri (ornek `HasPanelForValidation`, `EnsureOverlayReadyForValidation`) cagirmak -> bunlar overlay'i ZORLA kurar/gosterir, gercek toggle/bleed yolunu test ETMEZ. Recipe'de YASAK.
- TUZAK 3: Overlay'i `rootGroup.alpha` ile olcmek -> bleed fix `overlayCanvasGo.SetActive()` ile calisir; alpha 1 kalip canvas inactive olabilir. Mutlaka `Canvas_DirectorOverlay.activeInHierarchy` oku.
- TUZAK 4: A3'u Build Mode KAPALI iken kosmak -> guard hicbir zaman devreye girmez, sahte-PASS. A3 oncesi `BuildModeController.IsActive == true` oldugunu data-proof ile DOGRULA.
- TUZAK 5: Tek toggle test edip "calisiyor" demek -> bleed fix SADECE Director->Test yonunde gorulur. A1+A2 ikisini de kos; A2 overlay=false sart.
