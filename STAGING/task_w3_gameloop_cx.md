ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.
NLM ACCESS: uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>". Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / code / scenes / STAGING / memory.

Amaç: MainMenu -> CharacterSelect -> _IsoGame -> Death/Victory OYNANABİLİR döngüsünü kur. (3-ajan research workflow tüm kök-nedenleri buldu; aşağıdaki adımlar kesin.) Unity instance açık, UnityMCP profilinden eriş. Satır no'ları İPUCU — içerikle bul, kaymış olabilir.

## A) BUILD SETTINGS — _IsoGame ekle
`ProjectSettings/EditorBuildSettings.asset` m_Scenes'e EKLE (append, mevcutları silme/reorder etme): enabled:1, path: Assets/Scenes/_IsoGame.unity, guid: 170024b9cdc150349a6633ade977ccee. Hedef sıra: [0]MainMenu [1]CharacterSelect [2]_IsoGame [3]_FazMVP_Demo_s99 [4]PlayableArena_Test01. (Tercihen UnityEditor `EditorBuildSettings.scenes` API ile, GUID tutarlı kalsın.)

## B) CHARACTERSELECT load target -> _IsoGame (4 yer, hepsi gerekli)
1. `Assets/Scenes/UI/CharacterSelect.unity` ~line 2442 (ENABLED CharacterSelectScreen, fileID 1936231926): `gameSceneName: PlayableArena_Test01` -> `gameSceneName: _IsoGame`. (Serialized değer C# default'u EZER, o yüzden scene asset şart.)
2. Aynı dosya ~line 2303 (disabled legacy CharacterSelectController): `gameSceneName: RoomPipelineTest` -> `gameSceneName: _IsoGame`.
3. `Assets/Scripts/UI/CharacterSelectScreen.cs` ~line 19: default `"PlayableArena_Test01"` -> `"_IsoGame"`.
4. `Assets/Scripts/UI/CharacterSelectController.cs` ~line 25: default `"RoomPipelineTest"` -> `"_IsoGame"`.

## C) CLASS-CARRY FIX (KRİTİK — yoksa hep Warblade oynanır)
KÖK-NEDEN: PlayerClassManager.Instance CharacterSelect'te null (orada PlayerClassManager yok + DontDestroyOnLoad yok) -> OnStartRun'daki `if (Instance != null)` SetPrimaryClass'ı atlar -> seçim kaybolur -> _IsoGame Warblade'i zorlar. FIX = static carrier:
1. `Assets/Scripts/Systems/PlayerClassManager.cs`: sınıfa static alan ekle: `public static ClassType SelectedClass = ClassType.None;`
2. Aynı dosya `SetPrimaryClass(ClassType type)`: `if (type == ClassType.None) return;` guard'ından SONRA ekle: `SelectedClass = type;`
3. Aynı dosya `Start()`: `applyPrimaryOnStart` dalını DEĞİŞTİR: eski `if (applyPrimaryOnStart) SetPrimaryClass(PrimaryClass);` -> `if (applyPrimaryOnStart) { var chosen = SelectedClass != ClassType.None ? SelectedClass : ClassType.Warblade; SetPrimaryClass(chosen); }`. (Standalone _IsoGame launch = None -> Warblade default korunur; CharSelect'ten gelince taşınan sınıf uygulanır. SIRA önemli: Pending/SelectedClass kontrolü applyPrimaryOnStart dalının İÇİNDE, default Warblade'den ÖNCE.)
4. `Assets/Scripts/UI/CharacterSelectScreen.cs` `OnStartRun()`: LoadScene'den ÖNCE, KOŞULSUZ ekle: `PlayerClassManager.SelectedClass = selectedClass;` (mevcut `if (Instance != null) Instance.SetPrimaryClass(...)` çağrısını KORU, zararsız).
5. `Assets/Scripts/UI/CharacterSelectController.cs` `ApplySelectedClass()`: aynı `PlayerClassManager.SelectedClass = <seçilen class>;` satırını ekle (legacy path tutarlılığı).
NOT: CharacterSelectScreen.IsUnlocked zaten sadece 4 controller'lı sınıfı (Warblade/Elementalist/Ranger/Shadowblade) seçtiriyor, locked kartlarda START kapalı -> "controller not found" riski YOK. GEVŞETME, sadece doğrula.

## D) DEATH — Main Menu butonu
`Assets/Scripts/Core/DeathScreenManager.cs`:
1. `mainMenuButton` Button field ekle (restartButton yanına).
2. `EnsurePanelControls()` içinde (restartButton bloğundan sonra) mainMenuButton'ı CreateButton ile yarat ("MAIN MENU", RimaUITheme.PanelBorder/TextPrimary); 3 buton (TRY AGAIN, MAIN MENU, WISHLIST) sığacak şekilde anchor'ları yeniden düzenle. onClick -> LoadMainMenu.
3. Metod ekle: `private void LoadMainMenu() { Time.timeScale = 1f; if (Application.isPlaying) SceneManager.LoadScene("MainMenu"); }` (SceneManager using zaten var).
4. Safety: `private void OnDisable() { if (isDead) Time.timeScale = 1f; }` (donmuş timeScale sızıntısını önle).

## E) VICTORY trigger (MVP: tüm mob ölünce)
KÖK-NEDEN: _IsoGame'de boss yok, mob'lar in-scene prefab instance (runtime-spawn değil), "Systems" objesindeki room-manager `RIMA.LegacyRuntimeRoomManager` = OLMAYAN class (missing script). Yani victory tetiği yok.
1. YENİ dosya `Assets/Scripts/Core/RoomClearVictoryTrigger.cs` (namespace RIMA, MonoBehaviour): `Start()`'ta `GameObject.FindGameObjectsWithTag("Enemy")` ile canlı mob'ları topla; her birinin `Health` (GetComponent ?? GetComponentInChildren) `OnDeath`'ine bir kerelik listener ekle; canlı sayaç 0'a inince (guard bool ile bir kez) `RIMA.Systems.Map.RoomLoader.RaiseDemoComplete()` çağır. Start'ta 0 mob varsa HİÇBİR ŞEY yapma (boş oda otomatik kazandırma). Dedupe/guard stilini Systems/Map/RoomLoader.cs:248-260'tan al. Health.OnDeath kullan (CombatEventBus.OnKill DEĞİL — skill/DoT kill'leri kaçırabilir).
2. `_IsoGame.unity`'de "Systems" root'u altına bu component'i taşıyan bir GameObject ekle (inspector ref gerekmez, auto-find).
3. DemoCompleteOverlay zaten MainMenuButton + PlayAgainButton'a sahip (timeScale=1 restore ediyor) -> SADECE DOĞRULA, kod değişikliği YOK.

## F) (Opsiyonel, riskliyse ATLA) "Systems" objesindeki broken LegacyRuntimeRoomManager missing-script component'ini kaldır. RuntimeRoomManager.cs'e DOKUNMA.

## G) VERIFY
read_console error/warning = 0; Unity/dotnet compile temiz. (Play-mode testini Opus yapacak.) CODEX_DONE.md'ye: A-E ne yapıldı + console/compile durumu PASS/FAIL, F yapıldı mı.
