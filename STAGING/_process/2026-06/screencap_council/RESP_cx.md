# RESP_cx - Council Full Capture Brief (2026-06-17)

Kaynak dayanak: `.cx_dispatch/CODEX_TASK_COUNCIL_BRIEF_FULLCAPTURE_2026-06-17_bugra_20260617-114652.md`, graphify query, `rg` taramalari, hedefli script okumasi. Belirsiz olan yerlerde `BELIRSIZ` yazildi. Bu dosya capture/verify plani ve karar notudur; kod degisikligi yapilmadi.

## A - Eksiksiz ekran/state/mode envanteri

### 0. Mevcut capture kapsami

- Mevcut ana capture seti: `STAGING/_process/2026-06/demo_screenshots/01..13`. Bu set MainMenu, Settings, CharacterSelect, arena opening, fullflow gameplay, opening draft, HUD, RunMap, Director, BuildMode, Codex, Pause, TAB/SkillBar yakalamis.
- Ek mevcut alt klasorler: `interactive/` draft/runmap varyantlari ve `weaponless_test/` attack pozlari.
- Sonuc: kullanici hakli. Bu sadece baseline. Asil capture matrisi 13 degil, yaklasik 60+ state olmali.

### 1. Menu ve run baslangici

- [GOLDEN] MainMenu - scene-backed live menu. Ulasim: `Assets/Scenes/UI/MainMenu.unity`, `MainMenuController`, Play butonu -> CharacterSelect. Alt-state: ilk acilis/backdrop, button hover/pressed, Settings acilis butonu varsa, Quit hover. Not: `MainMenuScreen` legacy; live otorite `MainMenuController`.
- [EDGE] Legacy MainMenuScreen - BELIRSIZ live degil. Ulasim: eski self-created menu, kodda obsolete. Capture sadece regression kontrolu icin gerekebilir, demo golden path degil.
- [GOLDEN] Settings - Ulasim: ESC/Pause -> Settings veya menu ayarlari. Alt-state: Gameplay bolumu, Accessibility/feel bolumu, Audio bolumu, keybind listesi, rebind dinleme state'i, duplicate/invalid bind hata state'i, ESC cancel, slider degismis state. Not: kodda gercek tab degil, tek panel icinde bolumler var.
- [GOLDEN] CharacterSelect klasik ekran - Ulasim: MainMenu -> CharacterSelect, `CharacterSelectScreen`. Alt-state: her class hover, her class selected, kilitli class, kilit acilabilir class, kilit acilamaz/unaffordable class, skill tooltip hover, Start enabled/disabled, Back.
- [GOLDEN] Attunement Chamber - Ulasim: CharacterSelect scene icinde `ChamberSelectBootstrap`, klasik canvas gizlenir. Alt-state: oda genel gorunumu, Echo station idle, station yakinda prompt, locked/unaffordable prompt, unlock prompt, confirm prompt, attune animasyonu/busy, secili class glow, dummy yakinda, dummy class-select overlay acik, TAB ile klasik overlay acik, rift/door prompt, run baslatma transition.
- [GOLDEN] Opening kit draft - Ulasim: run baslangici / `DraftManager` delayed draft. Alt-state: pending/delay, 3 kart visible, card hover, card picked flash, slot full replace mode, skip replace, tooltip visible, draft kapandi.
- [EDGE] Language/localization state - Ulasim: Settings language row. Alt-state: TR/EN text swap. Demo icin bir capture yeterli; post-demo full i18n matrix.

### 2. HUD ve oyun ici sabit UI

- [GOLDEN] Gameplay HUD normal - Ulasim: `_Arena`, ilk combat. Alt-state: full HP, normal resource bars, room label `ODA n/6 - TIP`, skillbar idle, cooldown active, selected/current class identity.
- [GOLDEN] SkillBar yakin plan - Ulasim: gameplay, mouse hover slot. Alt-state: LMB tooltip, RMB tooltip, active skill tooltip, empty/locked slot, cooldown, drag/drop if enabled, basic attack profile damage text.
- [GOLDEN] CharacterHPBar - Ulasim: gameplay. Alt-state: full HP, damage taken, low HP, death/zero. Low-HP vignette setting ile birlikte capture edilmeli.
- [GOLDEN] PassiveStatusUI - Ulasim: status effect uygula. Alt-state: no passive, one icon, multi-stack icon, tooltip hover.
- [GOLDEN] BossHealthBar - Ulasim: Boss room. Alt-state: hidden, intro/full HP, mid HP, low HP, death/empty. Boss phase monologlariyla birlikte capture edilmeli.
- [GOLDEN] RoomMonolog - Ulasim: boss intro/phase beat, `RoomMonologController.Say`. Alt-state: title line, dialogue line, skip/any-key dismiss.
- [EDGE] MiniMap - Ulasim: `HUDController.showMiniMap=false` default. Alt-state: disabled authored panel. Capture not golden unless flag acilir.
- [EDGE] CharacterSheet/TAB overlay - Ulasim: TAB. Alt-state: closed/open, stat/resource list, active class, secondary class varsa, overlay while combat paused 0.1x.
- [EDGE] TooltipSystem generic - Ulasim: codex/skillbar/character select hover. Alt-state: screen-edge positioning, delayed show, hide on exit.

### 3. Oda tipleri ve oda dongusu

- [GOLDEN] Combat room - Ulasim: `_Arena`, DemoSequence node 0/1/3/post-boss. Alt-state: entry room label, wave spawn ani, mid-combat, enemy telegraph, player hit flash, hit-stop/impact frame, low HP red/vignette, enemy kill, room clear slow-mo, `ODA TEMIZLENDI`, reward spawn pop, reward pickup idle, reward interact, draft acik, draft kapali, doors open.
- [GOLDEN] Elite room - Ulasim: procedural graph / non-linear path, `RoomType.Elite`. DemoSequence'de fixed linear path icinde yok gibi gorunuyor; BELIRSIZ golden path. Alt-state: Combat ile ayni + elite room tint/affix/elite enemy if present.
- [GOLDEN] Merchant room - Ulasim: `_Arena` fixed demo sequence: Combat -> Combat -> Merchant -> Combat -> Boss -> post-boss Combat. `RoomRunDirector.HandleMerchantRoom`. Alt-state: entry label `TUCCAR`, 3 ShopStand, afford/unafford, purchase success, purchase failed, optional buy, exit doors already open. Not: Merchant ChestUI/ForgeUI degil; shop stand runtime controller.
- [EDGE] Chest room - Ulasim: `RoomType.Chest` procedural graph or room bank. Alt-state: chest/reward room entry, no combat immediate clear, reward pickup spawn, reward draft, door open. `ChestUI` exists but RoomRunDirector live path likely reward pickup/draft; ChestUI live kullanimi BELIRSIZ.
- [EDGE] Forge room - Ulasim: `RoomType.Forge` enum/legacy RuntimeRoomManager. Alt-state: Forge1 LMB ecol choice, Forge2 upgrade, card hover, choose/upgrade. Fixed demo sequence icinde yok; live `_Arena` path BELIRSIZ.
- [GOLDEN] Boss room - Ulasim: `_Arena` node Boss, `SpawnBossDirectly`. Alt-state: boss intro, boss full HP, telegraph/attack, phase beats at 50/33 if reached, boss low HP, boss death, room clear, dual-class selection, unlock draft pending/active, exit to post-boss combat.
- [EDGE] Event room - Ulasim: procedural graph / `RoomType.Event`. Alt-state: event room art, interaction prompt, reward/branch result. Live event implementation BELIRSIZ.
- [EDGE] Curse/Corridor - Ulasim: enum var. Live room bank/golden path BELIRSIZ; capture only if generated or authored.

### 4. Reward, draft, echo, class pick

- [GOLDEN] Reward pickup world state - Ulasim: combat clear. Alt-state: chest/rift sprite spawn pop, bob idle, player in range prompt, collected, WasCollected data true.
- [GOLDEN] Reward draft - Ulasim: reward interact. Alt-state: 3 skill/gold/heal/echo cards, hover, selected, confirm flash, tooltip, replacement mode, skip.
- [GOLDEN] Cross-class Echo card - Ulasim: reward offer type `CrossClassEcho` or boss unlock sequence. Alt-state: cyan echo treatment, hover tooltip, pick/grant.
- [GOLDEN] Dual-class selection - Ulasim: boss clear when no SecondaryClass, `ClassSelectionUI`. Alt-state: open modal, class hover, class selected, confirm, blocked by Director overlay guard.
- [EDGE] Forge draft - Ulasim: ForgeUI. Alt-state: Lv1 choose, Lv2 upgrade, chosen ecol, no chosen ecol error. BELIRSIZ in current demo sequence.
- [EDGE] ChestUI cards - Ulasim: ChestBehavior.Open legacy path. Alt-state: heal/gold/skill card, hover, choose. BELIRSIZ if live.

### 5. Harita ve path UI

- [GOLDEN] RunMapOverlay - Ulasim: `_Arena`, `RunMapOverlay`, key M. Alt-state: hidden, visible, current node highlight, room type colors, branching rows, blocked by UIManager modal, ESC close.
- [GOLDEN] DungeonMapUI - Ulasim: `DungeonMapUI`, key M when instance exists. Alt-state: visited, current pulse, step-1 exits, step-2 fog preview, missed paths, black fog panel, room symbols Combat/Elite/Boss/Chest/Merchant/Event/Forge.
- [EDGE] MapPanelUI tablet - Ulasim: `RIMA.UI.Map.MapPanelUI`, placeholder graph by default. BELIRSIZ live surface. Alt-state: placeholder, active graph, visited/nonvisited connections.
- [EDGE] MapProgressController - Ulasim: key M if component present. BELIRSIZ if coexists with above.
- [EDGE] MiniMap - Ulasim: HUD flag disabled. Capture only if re-enabled.

### 6. DirectorMode

- [GOLDEN] Director closed/test state - Ulasim: backquote toggles only if DirectorMode exists and no blocking UI. Alt-state: gameplay normal, no overlay.
- [GOLDEN] Director open base - Ulasim: backquote. Alt-state: camera paused/freecam, overlay visible, bottom telemetry strip.
- [GOLDEN] Spawn tab - Alt-state: no wave loaded, palette loaded, enemy selected highlight, spawn ghost, spawned enemy count, cap reached at 10, erase enemy, clear spawns.
- [GOLDEN] Class&Skill tab - Alt-state: class selection, skill/class info, current primary/secondary. Exact rows should be captured from live overlay.
- [GOLDEN] Stats tab - Alt-state: sliders live values, slider changed, reset, save preset, export JSON, no PlayerClassManager error.
- [GOLDEN] Build tab inside Director - Alt-state: director prop palette, prop selected, prop ghost, place, erase, clear props. When BuildModeController.IsActive, legacy Director prop tool is suppressed.
- [EDGE] Map tab - Current code creates empty panel `MAP yakinda`. Capture as unfinished/BELIRSIZ live value.
- [GOLDEN] Telemetry tab - Alt-state: empty metrics, damage events present, source bars LMB/RMB/Skill/Dot/Minion/Item/Unknown, clear telemetry, export CSV.
- [EDGE] Director overlay blocked - Ulasim: open draft/pause/codex/class selection/victory. Alt-state: Director state may remain but overlay hidden by bleed guard.

### 7. BuildMode / in-play editor

- [GOLDEN] BuildMode entry - Ulasim: F2 or Quote while no blocking UI, DirectorMode exists, Camera.main exists. Alt-state: camera zoom out, gameplay paused, other root UI canvases hidden, grid overlay active, Build palette visible.
- [GOLDEN] PROP tool - Alt-state: PROP selected, category tabs, search empty/nonempty, asset card idle/hover/selected/disabled, ghost valid, ghost invalid red, LMB placed, RMB erased, rotate with brackets, flip with F, eyedropper E, Ctrl+Z undo, Ctrl+Y redo, status label.
- [GOLDEN] TILE tool - Alt-state: TILE selected, FloorPaint mode, WalkableToggle mode, OverlayPaint mode, brush radius 1/2/3, cursor in-bounds green, cursor out-of-bounds red, LMB paint, RMB erase, undo/redo, active tool exclusivity (prop ghost hidden).
- [GOLDEN] Build session lifecycle - Alt-state: working copy created, source asset untouched, SaveWorkingTemplate editor-only success, save no-copy false, exit destroys working copy, camera rig restored, other canvases restored.
- [EDGE] Legacy InPlayMapPaintOverlay - Ulasim: compile define `RIMA_LEGACY_MAPPAINT` only. Current default retired; capture not golden except proof that it is absent.
- [EDGE] LiveTool ToolMain - Ulasim: `Assets/Scenes/LiveTool/ToolMain.unity`, `ToolBootstrap`. Separate authoring tool, not in golden demo unless explicitly presented.

### 8. Debug/tool overlays

- [GOLDEN] DemoDebugPanel - Ulasim: F1 in UNITY_EDITOR or DEVELOPMENT_BUILD. Alt-state: hidden, visible, god mode off/on, speed 1x/2x/0.25x, kill all mobs, force room clear, restart room, next room, jump room buttons.
- [EDGE] HUDEditor - Ulasim: Escape+H, `HUDEditorManager`. Alt-state: edit mode on/off, drag HUD element, resize, grid snap, overlap warning. Demo golden path degil ama UI tooling state.
- [EDGE] ScreenshotMode - Ulasim: development/editor helper. Capture preset states if screenshot automation uses it.

### 9. Son durumlar ve transitions

- [GOLDEN] DeathScreen - Ulasim: player lethal damage. Alt-state: slow-mo pre-panel, fade-in panel, random death quote, run stats, restart button, main menu button, R key restart.
- [GOLDEN] DemoCompleteOverlay - Ulasim: terminal post-boss Combat clear. Alt-state: victory backdrop, run summary, next-class teaser/silhouette present or fallback text, main menu, play again. Wishlist hidden by constant.
- [GOLDEN] Door/gate open - Ulasim: room clear or Merchant immediate open. Alt-state: locked door, unlock VFX, room-type rune sprite, interact prompt, transition triggered.
- [GOLDEN] RoomTransitionFX - Ulasim: door/room change. Alt-state: fade out, loading/transition midpoint, fade in. Needs video or timed screenshots.
- [GOLDEN] Portal/rift - Ulasim: Chamber exit, reward/fragment portal if present. Alt-state: idle, proximity prompt, activation.
- [EDGE] Panic recovery - Ulasim: F12. Alt-state: no visible UI by design, but data/log proof should be captured.

## B - Fonksiyonel verify: gorsel-yakala + runtime-assert recetesi

Genel kural: her state icin iki kanit alin. 1) screenshot/video frame, 2) runtime data/assert. Screenshot tek basina yeterli degil.

### BuildMode verify

- Entry proof: execute_code ile `BuildModeController.Instance.Toggle()` veya `EnterBuildMode()` cagir. Assert: `BuildModeController.IsActive == true`, `DirectorMode.Instance.State == Director`, `BuildPlacementController.Instance.GridOverlayActiveForValidation() == true`, foreign Canvas enabled state false, `BuildModeController.ActiveWorkingTemplate != null` eger live RoomTemplate varsa.
- Asset selection proof: `BuildPlacementController.Instance.SelectFirstPropForValidation()`. Assert: return true, status/card selected gorseli, `SelectedDef` dogrudan private oldugu icin public `PlaceForValidation` sonucu ile teyit.
- Prop placement proof: `PlaceForValidation(new Vector2Int(x,y))`. Assert: return true, `PlacedCountForValidation()` onceki + 1, working copy `props.Last().origin == (x,y)`, `placedByUser == "BuildMode"`, spawned runtime object grid cell `grid.WorldToCell(obj.transform.position)` footprint icinde. Dogru hucre kaniti bu olmali, piksel bakisi degil.
- Grid-snap proof: mouse/world input yerine validation cell kullan; UI testte ayrica `Grid.WorldToCell(mouseWorld)` ile beklenen cell hesaplanip `PropPlacementData.origin` ile karsilastir.
- Erase proof: `EraseForValidation(cell)` return true, `PlacedCountForValidation()` -1.
- Undo/redo proof: place -> undo -> count eski, redo -> count yeni. Tile ve prop ayni `BuildCommandStack` uzerinden interleave test edilmeli.
- Tile proof: `SelectToolForValidation(1)` ile Tile aktif. LMB/RMB private oldugu icin ya PlayMode testte InputSystem event simule edilmeli ya da kucuk validation hook eklenmeli: `PaintTileForValidation(cell, mode, paint)`. Assert: working copy `walkableGrid`/`overlayMask` ve live Tilemap ayni cell'de degisti; undo geri aldi.
- Working-copy proof: EnterBuildMode sonrasi `ActiveWorkingTemplate` source template ile `ReferenceEquals == false`; cikista null; source asset sadece `SaveWorkingTemplate` ile kirlenir.
- Compile/build proof: Standalone non-development build icin `DEMO_BUILD` yoksa BuildMode kodu compile edilmez. ProjectSettings su an `Standalone: RIMA_LIVE_TOOL`; bu nedenle demo build ya Development Build olmali ya da `DEMO_BUILD` define eklenmeli.

### DirectorMode verify

- Bootstrap proof: full-flow MainMenu -> CharacterSelect -> _Arena sonrasi assert: `DirectorMode.Instance != null`. Eger null ise backquote imkansizdir.
- Backquote guard proof: overlay yokken `DirectorMode.Instance.ToggleState()` -> `State == Director`, overlay visible; draft/pause/codex acikken `IsBlockingUiOpen == true` ve backquote inert/hide.
- Spawn proof: `SelectFirstSpawnEnemyForValidation()`, `SpawnSelectedEnemyAtForValidation(pos)`, assert `DirectorSpawnedEnemyCountForValidation()` +1, spawned object tag `Enemy`, name suffix `_Director`, position `SnapWorld(pos)`.
- Spawn cap proof: 12 spawn dene, assert count 10. Zaten `DirectorModeValidationTests.SpawnValidationStopsAtDirectorCap` var.
- Stat slider proof: `SetStatForValidation("physPower", 177f)` assert `PlayerClassManager.Instance.EnsureCurrentPrimaryStats().physPower == 177`; `debugGlobalDamageMult` 50 input -> clamp 5. Zaten EditMode test var.
- Telemetry proof: damage event yarat, assert `TelemetryEventCountForValidation() > 0`, DPS > 0, source bucket artar, `ExportTelemetryCsvForValidation()` header+satir dondurur.
- Prop proof: `SelectFirstPropForValidation`, `PlaceSelectedPropAtForValidation`, count +1, erase count -1.

### Draft/reward/door verify

- Opening/reward draft: `DraftManager.ShowDraft()` veya reward collect path. Assert: `DraftManager.Instance.IsDraftActive == true`, `UIManager.Instance.IsSkillOfferOpen == true`, `Time.timeScale == 0`. Card click sonrasi assert draft false, granted skill/gold/heal/echo state degisti.
- Reward pickup: clear room -> assert `RewardPickup` exists, `WasCollected == false`; interact/ForceCollect -> `WasCollected == true`; draft pending/active dogru.
- Door transition: room clear -> assert lifecycle reaches DoorOpen, exit doors active/unlocked, `RoomRunDirector.CurrentNodeId` transition sonrasi degisir, player spawn cell valid.
- Merchant: entering Merchant should spawn `ShopRoomController` and 3 `ShopStand`; doors immediately open. Purchase assert Echo balance azalir veya insufficient log/status.
- Boss/dual class: boss Health death -> `ClassSelectionUI.IsOpen == true` veya secondary class prompt; selection sonrasi `SecondaryClass != None`, draft pending/active resolved, door opens.
- Death: player `Health.TakeDamage(99999)` -> after realtime sequence assert `DeathScreenManager.IsDeathActiveForDemo == true`, `Time.timeScale == 0`, player inactive.
- Victory: terminal clear -> `DemoCompleteOverlay.IsActive == true`, `Time.timeScale == 0`, buttons visible.

### Capture automation format

- Her capture maddesi icin dosya adi: `NN_category_state_expected.png`.
- Her fonksiyonel assert icin json/log: `NN_category_state_assert.json` icinde pre-state, action, post-state, pass/fail, BELIRSIZ alanlari.
- Fail durumunda screenshot yine alinsin ama dosya adi `FAIL_...` olsun. Gorsel ve data ayrilmamali.

## C - Asset UI/UX profesyonel tasarim

### Ortak prensipler

- Ikon + etiket: BuildMode asset kartlari, Director spawn/prop butonlari, draft kartlari sadece text olmamali. Her secilebilir seyde thumbnail/icon, isim, kisa kategori, maliyet/validlik bilgisi olmali.
- Secili vurgu: tek aktif tool/asset net gorunmeli. Border, fill, glow ve status satiri ayni secimi soylemeli. Sadece renk degil, ikon/outline da degismeli.
- Hover bilgisi: kart hover -> tooltip veya alt bilgi. Tooltip mouse'u kapatmamali, ekran kenarina carpmamali.
- Grid ve spacing: asset palette 2 veya 3 kolon sabit kart boyu; kartlar esit yukseklik; search, category tabs, status, help text ayrik. Sikisik debug IMGUI gorunumu sadece F1 icin kabul edilebilir.
- Tipografi: baslik, sekme, kart ismi, aciklama, maliyet ayni scale sisteminde. Kart icinde uzun metin auto-size veya kisaltilmis tooltip.
- Feedback: place/erase/choose aksiyonunda kisa toast, ses, pulse. Invalid action kirmizi ghost + sebep metni.
- Game-editor standardi: Unity Tile Palette mantigi (tool, palette, inspector, scene overlay), Hades/Dead Cells okunabilir combat HUD'u, Slay the Spire map/draft netligi referans alinabilir.

### BuildMode quick-win (2 gun)

- PROP/TILE segmented control ve category tabs zaten var; en buyuk kazanc asset card thumbnail'larini netlestirmek, selected border'i daha kalin yapmak, disabled kartlara neden metni eklemek.
- Search bar bos state: `No assets` yerine kategoriye gore `No props match` / `No tiles match`.
- Status label: secili asset, cell, valid/invalid reason, undo count tek satirda profesyonel HUD strip.
- Ghost: valid yesil, invalid kirmizi, footprint outline; cell center snap gorselini gridle hizala.
- Tile modes: Floor/Walkable/Overlay icin ikonlu 1/2/3 chips; radius stepper `[-] 1 [+]` gibi gorunmeli.
- Save state: runtime-only oldugu icin UI'da `Runtime copy` / `Save Editor Only` rozetini gostermek demo anlatimini kurtarir.

### DirectorMode quick-win (2 gun)

- Spawn/Build listeleri thumbnail grid'e donmeli veya en azindan ikon chip + label + selected state. IMGUI/debug hissi azalt.
- Stats tab slider row'lari: stat name, numeric value input, reset mini button. Changed state amber nokta.
- Telemetry tab: DPS/TTK/events ust metrik kartlari, source bars altinda consistent colors. Export/Clear icon button + label.
- Build tab: Director prop placement ile BuildMode prop palette ayni dilde olmali. Eger F2 BuildMode profesyonelse, Director Build tab'i ya gizle ya da `legacy prop` diye kisitli goster.

### Draft/HUD quick-win (2 gun)

- Draft card: type badge (SKILL/HEAL/GOLD/ECHO), rarity color, icon, one-line effect summary, hover tooltip. Kart hover'da digerleri dim zaten var; bunu koru.
- SkillBar: LMB/RMB ve active skill slot tooltipleri capture edilmeli; cooldown numeric veya radial okunurlugu iyilestirilmeli.
- HUD: room label, room status, passive icons ve boss bar hiyerarsisi ayni frame/token ailesinde olmali. Low HP efekti Settings toggle ile dogrulanmali.

### Post-demo

- Runtime BuildMode asset inspector: selected prop stats, footprint preview, tags, collision/walkability flags.
- Undo history panel.
- Palette favorites/recent.
- True map panel unification: RunMapOverlay, DungeonMapUI, MapPanelUI tek canli sistemde birlestirilmeli.
- Full controller/gamepad navigation states.
- Accessibility pass: colorblind-safe selected/invalid states, larger font mode.

## D - F2 / backquote calismiyor: kok neden ve temiz yol

### Kanitlar

- `BuildModeController` F2 ve Quote'u self-bootstrap ile sahipleniyor, ancak `EnterBuildMode()` icinde `DirectorMode.Instance == null` ise erken return ediyor. Bu durumda F2 basilir ama gorunur sonuc yok.
- `DirectorMode` backquote'u sadece kendi instance'i varsa dinler. Instance yoksa backquote tamamen oludur.
- `DirectorMode` ve `BuildModeController` sadece `DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR` altinda compile edilir. ProjectSettings Standalone define su an sadece `RIMA_LIVE_TOOL`; `DEMO_BUILD` yok. Development Build degilse Player build'de bu siniflar yoktur.
- `DemoDebugPanel` de sadece `DEVELOPMENT_BUILD || UNITY_EDITOR`; `DEMO_BUILD` tek basina F1 panelini getirmez.
- `DirectorMode.CheckAndSpawn` MainMenu/CharacterSelect sahnelerinde spawn etmez, `_Arena` gibi diger sahnelerde spawn etmeli. Kod `sceneLoaded` hook'u da kuruyor; bu yuzden full-flow'da `_Arena` yuklenince spawn etmesi beklenir. Eger full-flow'da yine yoksa ya static bootstrap/compile sembolu yoktur, ya `_Arena` yuklenmiyordur, ya da build stripped/non-development'tir.
- Backquote guard: `IsBlockingUiOpen()` true ise backquote inert. Opening draft, pause, codex, settings, TAB, class selection, victory varken Director bilerek acilmaz. BuildMode da draft/overlay varken girmeyi reddeder.

### Tek hipotez sirasi

1. En olasi demo build kok neden: Standalone build Development Build degil ve `DEMO_BUILD` define yok. Sonuc: F2/backquote kodu build'de compile edilmez.
2. Editor/full-flow kok neden: oyuncu F2/backquote'u opening draft veya baska blocking UI acikken deniyor. Sonuc: guard dogru calisir ama kullaniciya sessiz gorunur.
3. Daha dusuk ama kritik: full-flow `_Arena` yuklenince `DirectorMode.Instance` olusmuyor. Kodun niyeti comment'e gore full-flow'da dev overlay'i kapatmakti; mevcut `sceneLoaded` kodu bunu kismen tersine cevirmis. Runtime assert ile karar verilmeli.

### Temiz demo yolu

- 19 Haz canli sunum icin en temiz yol: demo build Development Build olarak alinacak VEYA Standalone define'a `DEMO_BUILD` eklenecek. Aksi halde F2/backquote yok.
- Full-flow da desteklenecekse: `_Arena` scene load sonrasi `DirectorMode.Instance != null` assert'i zorunlu. Eger null cikarsa `DirectorMode` bootstrap'i MainMenu/CharacterSelect'i atlayip `_Arena` icin spawn edecek sekilde netlestirilmeli.
- Kullanici deneyimi icin: F2/backquote blocking UI varken sessiz kalmamali. Demo quick-win: status toast/log `Close draft/pause to enter Build/Director` veya F12 panic docs. Kod degisikligi gerekiyorsa kucuk ve risk dusuk.
- Alternatif yol: canli sunum dev-direct `_Arena` ile baslatilsin. Bu en az riskli operasyonel plan ama kullanici MainMenu->game flow'da F2 bekliyorsa beklentiyi karsilamaz.
- Oneri: ikisini de yap. Build sembolunu duzelt; `_Arena` full-flow assert'i ekle/manuel verify et; demo runbook'ta F2/backquote sadece blocking UI kapaliyken kullanilir yaz.

### Minimum runtime assert komutlari

- Full-flow `_Arena` acildiktan sonra execute_code:
  - `DirectorMode.Instance != null`
  - `BuildModeController.Instance != null`
  - `BuildModeController.Instance.Toggle(); BuildModeController.IsActive == true`
  - `UIManager.Instance == null || !UIManager.Instance.IsAnyOverlayOpen`
  - `DraftManager.Instance == null || (!DraftManager.Instance.IsDraftActive && !DraftManager.Instance.IsDraftPending)`
- Build sembol assert'i: Player build logunda `DEMO_BUILD` veya Development Build aktif oldugunu yazdir. ProjectSettings'e bakmak tek basina yetmez; Build Settings de Development olabilir.

## Kisa capture oncelik listesi

1. MainMenu, Settings, Chamber, class select/chamber prompts, opening draft.
2. Combat full loop: entry, wave spawn, mid-combat, telegraph, hit/impact, low HP, clear, reward pickup, reward draft, door open.
3. Merchant room, Boss room, dual-class, post-boss combat, victory.
4. F2 BuildMode all PROP/TILE substates + asserts.
5. Backquote Director all tabs + spawn/stat/telemetry asserts.
6. Pause, Codex per class, TAB character sheet, SkillBar/passive/tooltip closeups.
7. Death, transition FX, portal/door prompt.
8. Edge rooms: Elite, Chest, Forge, Event, Curse/Corridor only if reachable or explicitly spawned.


