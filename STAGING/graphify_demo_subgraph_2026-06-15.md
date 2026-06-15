# RIMA demo-critical graphify SUBGRAPH (condensed for ChatGPT)

> AST-only extraction (6925-node full graph'ten demo-kritik 7 dosyaya filtrelendi). 
CAVEAT: call-edge'lerde generic-metod-ismi gurultusu var (.Add/.Log/.SetActive/.Round/.Bind yanlis hedefe baglanmis olabilir). 
TOPOLOJI (god-class, dosya-coupling) guvenilir; tek tek call-edge'leri kodla DOGRULA.


## RewardPickup.cs  (14 node)
- `RewardPickup` [L7]  in=2 out=11
- `.Collect()` [L96]  in=3 out=4
- `.DraftThenOpenExit()` [L171]  in=2 out=3
- `.ClearPlayerRange()` [L129]  in=3 out=1
- `.ShowPrompt()` [L115]  in=3 out=1
- `.ForceCollect()` [L89]  in=1 out=3
- `.EnsurePromptVisuals()` [L138]  in=3 out=0
- `.Update()` [L62]  in=1 out=1
- `.OnTriggerEnter2D()` [L70]  in=1 out=1
- `RewardPickup.cs` [L1]  in=0 out=2
- `.Awake()` [L28]  in=1 out=1
- `.OnTriggerExit2D()` [L78]  in=1 out=1
- `RIMA` [L5]  in=1 out=0
- `.Reset()` [L56]  in=1 out=0

## DraftManager.cs  (43 node)
- `DraftManager` [L16]  in=2 out=40
- `.ShowDraft()` [L195]  in=7 out=8
- `.ShowOpeningKitDraft()` [L285]  in=5 out=6
- `.HandleActivePick()` [L488]  in=2 out=9
- `.OnOfferSelected()` [L413]  in=5 out=5
- `.AssignActive()` [L678]  in=8 out=1
- `.EnsureDependencies()` [L554]  in=8 out=0
- `.ForcePickFirstOpeningKitSkill()` [L262]  in=4 out=4
- `.TryDirectorAssignSkill()` [L389]  in=2 out=6
- `.BuildBandReplaceCandidates()` [L744]  in=5 out=1
- `.FinishPick()` [L722]  in=5 out=1
- `.OnReplaceChosen()` [L512]  in=1 out=5
- `.MaybeInjectEchoOffer()` [L344]  in=4 out=2
- `.HandlePassivePick()` [L464]  in=4 out=2

## RoomRunDirector.cs  (86 node)
- `RoomRunDirector` [L86]  in=2 out=63
- `.BuildCurrentRoom()` [L279]  in=7 out=17
- `.RoomClearSequence()` [L1195]  in=5 out=10
- `.BeginRun()` [L176]  in=4 out=7
- `.StartRoomEncounter()` [L822]  in=3 out=8
- `.HandleMerchantRoom()` [L926]  in=6 out=5
- `.ForceOpenExitDoorsFromAnyClearedState()` [L1619]  in=6 out=3
- `.SpawnBossDirectly()` [L885]  in=3 out=5
- `RoomRunExitDoorTrigger` [L1824]  in=2 out=6
- `.ConfigureExitDoors()` [L1690]  in=5 out=3
- `RoomRunLifecycle` [L27]  in=1 out=6
- `.TryResolvePlayerSpawnWorld()` [L587]  in=4 out=3
- `.OpenExitDoors()` [L1597]  in=4 out=3
- `.EnsurePlayerAtSpawn()` [L542]  in=2 out=5

## BuildModeController.cs  (21 node)
- `BuildModeController` [L32]  in=2 out=18
- `.EnterBuildMode()` [L211]  in=2 out=7
- `.ExitBuildMode()` [L292]  in=2 out=6
- `.OnDestroy()` [L138]  in=1 out=5
- `.RestoreCameraRig()` [L468]  in=5 out=0
- `.StartZoom()` [L419]  in=3 out=2
- `.EnsureOwns()` [L192]  in=2 out=2
- `.DestroyWorkingTemplate()` [L401]  in=4 out=0
- `.HideOtherUiCanvases()` [L504]  in=3 out=1
- `.RestoreOtherUiCanvases()` [L527]  in=3 out=1
- `.Toggle()` [L199]  in=2 out=2
- `.CreateWorkingTemplate()` [L332]  in=2 out=1
- `.SaveWorkingTemplate()` [L360]  in=2 out=1
- `.ZoomRoutine()` [L439]  in=2 out=1

## BuildPlacementController.cs  (74 node)
- `BuildPlacementController` [L47]  in=2 out=64
- `.Label()` [L1145]  in=41 out=0
- `.EnsurePaletteUi()` [L788]  in=15 out=4
- `.SetBuildModeActive()` [L182]  in=4 out=12
- `.UpdateStatus()` [L1061]  in=11 out=2
- `.RebuildCards()` [L929]  in=7 out=6
- `.HandleCursor()` [L346]  in=3 out=9
- `.SetActiveTool()` [L218]  in=1 out=10
- `.Eyedropper()` [L579]  in=4 out=5
- `.SelectPalette()` [L1027]  in=5 out=4
- `.RebuildGhost()` [L603]  in=4 out=5
- `.CommitPlace()` [L424]  in=4 out=4
- `.ValidatePlacement()` [L379]  in=5 out=3
- `.ApplyPlace()` [L489]  in=4 out=4

## DirectorMode.cs  (173 node)
- `DirectorMode` [L38]  in=2 out=166
- `.Stretch()` [L2912]  in=24 out=0
- `.Anchor()` [L2903]  in=22 out=0
- `.EnsureOverlayReadyForValidation()` [L562]  in=16 out=4
- `.CreateText()` [L2888]  in=20 out=0
- `.CreateButton()` [L2879]  in=16 out=0
- `.CreateFill()` [L2853]  in=15 out=1
- `.AddTelemetryPanel()` [L1741]  in=4 out=11
- `.CreatePanel()` [L2862]  in=14 out=1
- `.AddClassSkillPanel()` [L1588]  in=4 out=10
- `.SetState()` [L296]  in=5 out=9
- `.BuildOverlay()` [L682]  in=2 out=12
- `.AddStatsPanel()` [L1653]  in=4 out=10
- `.AddSpawnPanel()` [L839]  in=4 out=8

## DamageCalculator.cs  (5 node)
- `DamageCalculator` [L16]  in=1 out=2
- `.Calculate()` [L23]  in=1 out=2
- `.GetDefenseMultiplier()` [L81]  in=2 out=0
- `DamageCalculator.cs` [L1]  in=0 out=2
- `RIMA.Balance` [L3]  in=1 out=0

## CROSS-FILE call edges (demo dosyalar arasi — gercek coupling)
- RewardPickup.`.ForceCollect()` --calls--> RoomRunDirector.`.RoomClearSequence()`
- RewardPickup.`.DraftThenOpenExit()` --calls--> DraftManager.`.ShowDraft()`
- RoomRunDirector.`.OpeningKitDraftSequence()` --calls--> DraftManager.`.HideDraft()`
- RoomRunDirector.`.OpeningKitDraftSequence()` --calls--> DraftManager.`.ShowOpeningKitDraft()`
- RoomRunDirector.`.OpeningKitDraftSequence()` --calls--> DraftManager.`.ForcePickFirstOpeningKitSkill()`
- RoomRunDirector.`.RoomClearSequence()` --calls--> DraftManager.`.HideDraft()`
- DraftManager.`.TryDirectorAssignSkill()` --calls--> DirectorMode.`.AssignSelectedSkillToSlot()`
- BuildModeController.`.OnDestroy()` --calls--> BuildPlacementController.`.SetBuildModeActive()`
- BuildModeController.`.EnterBuildMode()` --calls--> DirectorMode.`.SetState()`
- BuildModeController.`.EnterBuildMode()` --calls--> DirectorMode.`.ShowTab()`
- BuildModeController.`.EnterBuildMode()` --calls--> BuildPlacementController.`.SetBuildModeActive()`
- BuildModeController.`.ExitBuildMode()` --calls--> BuildPlacementController.`.SetBuildModeActive()`
- BuildModeController.`.ExitBuildMode()` --calls--> DirectorMode.`.SetState()`
- BuildModeController.`.ExitBuildMode()` --calls--> DirectorMode.`.ShowTab()`