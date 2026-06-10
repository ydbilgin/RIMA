I will start by searching for files related to `EncounterController`, `RoomRunDirector`, and `DraftManager` in the project root to pinpoint the files we need to read and analyze.
I will list the contents of the project directory to locate the code and assets.
I will run a command to search for C# files related to the critical classes in the `Assets` directory.
I will find the exact path of the C# scripts for `EncounterController`, `RoomRunDirector`, and `DraftManager`.
I will read `EncounterController.cs` to analyze the mob spawning logic and why some mob sprites are missing/black.
I will search for `ThreatBudget` to locate its definition and see how it spawns enemies.
I will read `ThreatBudget.cs` to inspect the spawning logic.
I will view the `RoomRunDirector.cs` file.
I will find the definition and context of `StartRoomEncounter` inside `RoomRunDirector.cs` using PowerShell.
I will view lines 800 to 950 of `RoomRunDirector.cs` to analyze the exact room encounter initialization.
I will search for `ResolveEncounterWave` in `RoomRunDirector.cs`.
I will view lines 980 to 1050 of `RoomRunDirector.cs`.
I will view lines 1040 to 1100 of `RoomRunDirector.cs`.
I will list the enemy prefabs in the `Assets/Prefabs/Enemies/` directory to see what they are.
I will search the `Assets/Scripts` folder for any occurrences where materials, shaders, or sprite renderers are modified on enemies.
I will search the project documentation/status files for references to "black", "invisible", "sprite", or "mob" to see if there is existing analysis of this issue.
I will search `CURRENT_STATUS.md` for information regarding enemy sprites, black/invisible enemies, or shader issues.
I will search the repository files for comments or text mentioning "siyah" (black) or "gorun" (appear/visible) to see if this has been documented or discussed in code comments.
I will run a command to find the `EncounterBankSO` assets and `EncounterWaveSO` assets in the project.
I will search for ScriptableObject files in the `Assets/` directory that relate to encounters, waves, or enemies.
I will look for C# files with names containing "Sprite", "Animation", "Render", or "Direction" in `Assets/Scripts` to see how enemy sprites are rendered or calibrated.
I will view the `EnemyAnimator.cs` script to see how enemy sprite animations are selected and rendered.
I will view `Act1_Wave_Pilot.asset` to see which enemy prefabs are wired to the waves.
I will search for the GUIDs referenced in `Act1_Wave_Pilot.asset` to see which enemy prefabs they map to.
I will view `Act1_EncounterBank_Pilot.asset` to see how waves are organized and referenced.
I will search for the GUID `b2bf1be61eb98e644b07f9ec84431d69` to find the exact asset name.
I will search recursively for the wave asset matching that GUID.
I will inspect `PlaceholderSprite.cs` to see if it plays a role in placeholder rendering for enemies or objects.
I will find all `.mat` (Material) files in the `Assets/` directory.
I will run a command to search the enemy prefab files for SpriteRenderer configurations, focusing on whether any have a null or missing sprite, or a black color, or use a specific material.
I will list all files in the `Assets/Scripts/Enemies` directory.
I will read the contents of `EnemyPlaceholder.cs` to see if it sets up temporary rendering or colors for enemies.
I will list the files in the `Assets/Scripts/Encounter` directory.
I will view `EncounterWaveSO.cs` to understand the wave and enemy entry structures.
I will search `FractureImp.prefab` to inspect the structure of its GameObjects and components (Animator, SpriteRenderer, EnemyAnimator).
I will inspect the material reference in `FractureImp.prefab` to see what material is being assigned to the SpriteRenderer.
I will print the materials assigned to each enemy prefab to check if any of them use incorrect materials (such as a 2D lit material or a missing material).
I will search for the material asset that corresponds to the GUID `a97c105638bdf8b4a8650670310a4cd3` to see which shader/material it represents.
I will check the GUIDs of the materials in `Assets\Materials`.
I will check the GUIDs of the materials in `Assets\Art\Materials`.
I will search all `.meta` files under `Assets` for the GUID `a97c105638bdf8b4a8650670310a4cd3` to identify which material asset this is.
I will check if the boss prefab `PenitentSovereign.prefab` has `EnemyAnimator` or similar scripts attached.
I will list the contents of the `Assets/Resources` directory.
I will list the directories inside `Assets/Resources` to see if a `Prefabs` folder exists.
I will list all files inside `Assets\Resources\Prefabs` recursively to see if there are fallback prefabs located there.
I will read `DraftManager.cs` to understand the class draft system and investigate why the skill icon bar is empty.
I will search for `ClassType` to see how many classes are defined in the game and what their types are.
I will search for skill controllers in the scripts folder to see which classes have dedicated controller components implemented.
I will search for scripts related to class selection or character selection UI.
I will view `CharacterSelectController.cs` to see what classes are selectable in the character selection menu.
I will search for `ClassUnlockPolicy.cs` to see which character classes are unlocked by default.
I will view `ClassUnlockPolicy.cs` to see what classes are unlocked in the game.
I will search `CURRENT_STATUS.md` for "P0" tasks to list the critical cleanups and prioritize them for the demo presentation.
I will list all files in the `STAGING` directory to locate the master plan or any cleanup/P0 task list files.
I will search for occurrences of "P0" in the `STAGING` directory to see the detailed list of 8-P0 tasks.
I will list all P0 tasks found in the STAGING files to compile the complete "8-P0" list.
I will list the first 100 lines of `STAGING\NEW_SESSION_WORKLIST_S6.md` to see the worklist items and their priorities.
I will view lines 140 to 180 of `STAGING\DESIGN_LOCK_DEMO_S6.md` to see the game-feel punch-list which contains additional P0 tasks.
I will search the project root for references to "8-P0" or similar to see if there is an explicit list of 8 P0 tasks.
I will view `CODEX_TASK_yekta.md` to analyze the detailed descriptions of the 8-P0 cleanup items.
I will view the contents of `CODEX_DONE_yekta.md` to see what previous analysis or details have been saved for this task.
I will view the contents of `CODEX_DONE_yasinderyabilgin.md` to see if there is any relevant analysis.
I will view `CODEX_DONE.md` in the root directory.
I will view `AGY_DONE_ydbilgin.md` to see what results have been stored from previous agent dispatches.
I will check the other `AGY_DONE_` files for analysis results about the mob visibility and skill icon bar issues.
I will find and inspect `SkillOfferGenerator.cs` to check if room depth is correctly processed for draft selections.
I will view `SkillOfferGenerator.cs`.
I will list all scene files (`.unity`) in the project directory.
I will view `ProjectSettings/EditorBuildSettings.asset` to see which scenes are registered in the build pipeline.
I will find the path to `MainMenuScreen.cs` to analyze `AutoInit` and why it should be disabled.

I will view `MainMenuScreen.cs`.
I will search for `MapFragment` in the project to understand what this task refers to.
I will check where `class MapFragment` is defined in the scripts.
I will view `Assets\Scripts\Core\MapFragment.cs`.
I will view `Assets/Scripts/Environment/MapFragment.cs` to compare it to the legacy one.
I will search for `MapFragment` prefab files in the project.
I will check the GUIDs of the legacy (Core) and canonical (Environment) MapFragment scripts.
I will inspect the attached scripts on the `MapFragment` prefabs.
I will search for any active references to the legacy `MapFragment` script GUID (`81cb176bbb9aea243b289acf0b7dcdd6`) across the codebase.
I will search the project files for references to "kapı" (door), "gate", or "door consolidation" to understand the door consolidation task.
I will search for `IsoRoomBuilder.cs` to analyze how exit doors are generated.
I will search for `BuildExitDoors` in `IsoRoomBuilder.cs`.
I will view lines 750 to 800 of `IsoRoomBuilder.cs` to analyze the door spawning logic.
I will view lines 800 to 860 of `IsoRoomBuilder.cs` to inspect `CreateExitDoorObject`.
I will search for `ConfigureExitDoors` in `RoomRunDirector.cs`.
I will view lines 1660 to 1750 of `RoomRunDirector.cs` to check how exit doors/gates are configured and if there's a need for consolidation.
I will search for `RoomRunExitDoorTrigger` to see how door triggers are handled.
I will search for the definition of `RoomRunExitDoorTrigger` in all C# scripts.
I will view `RoomRunDirector.cs` from line 1800 to the end to inspect `RoomRunExitDoorTrigger`.
I will locate `HandleEncounterCleared` in `RoomRunDirector.cs` using PowerShell.
I will view the `RoomClearSequence` method in `RoomRunDirector.cs` starting from line 1170 to check what happens after a room or boss encounter is cleared.
I will search for `victory` or `suppressClassSelect` in `RoomRunDirector.cs` using PowerShell.
I will search for the definition of `BuildDemoSequence` to see how the demo run structure is configured.
I will locate `HUDController.cs` and `SkillBarUI.cs` to check how they initialize.
I will view `SkillBarUI.cs`.
RIMA projesinin birkaç saat sonraki sunumu için **LEAN/SHIP-FAST** merceğinden hazırladığım detaylı analiz ve teşhis sonuçları aşağıdadır:

---

### 1) Mob Görünmeme / Siyah Görünme Sorunu (SIYAH RENDER)

#### **Kök Nedenler**
1. **URP Lit Material Sorunu (Siyah/Görünmez Render):** 
   - Projedeki 12 düşman prefabından 5 tanesi (`HollowMite.prefab`, `TheWound.prefab`, `PenitentSovereign.prefab`, `SeamCrawler_Elite.prefab`, `VoidThrall_Elite.prefab`) URP'nin varsayılan lit materyalini (`Sprite-Lit-Default.mat`, GUID: `a97c105638bdf8b4a8650670310a4cd3`) kullanmaktadır.
   - Arena sahnelerinde URP 2D ışık kaynağı (Light) bulunmadığı durumlarda, bu materyali kullanan düşmanlar tamamen siyah/görünmez olarak render edilir. Diğer moblar (`FractureImp`, `ChainWarden` vb.) varsayılan unlit `Sprites-Default` materyalini kullandığı için sorunsuz görünmektedir.
2. **Build/Runtime Prefab Bulunamama Sorunu (Null Prefab):** 
   - [RoomRunDirector.cs:L1070-1081](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L1070-L1081) dosyasında fallback düşman prefabı `AssetDatabase.LoadAssetAtPath` ile çözülmektedir. Bu kod sadece Editor playtest'inde çalışır; standalone build alındığında prefab `null` dönecektir ve odalar savaşa girmeden otomatik temizlenecektir.
   - Boss prefabı [RoomRunDirector.cs:L943-L947](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L943-L947) dosyasında diskte var olmayan `Assets/Resources/Prefabs/Enemies/Boss/PenitentSovereign` yolundan `Resources.Load` edilmeye çalışılmakta, standalone build'de boss da spawn olamamaktadır.

#### **En Yalın Fix (Minimum Efor)**
* **Spawning Override (Runtime):** Düşmanlar spawn edilirken SpriteRenderer materyallerini unlit `Sprites/Default` materyaline zorlayın:
  - **Standart Düşmanlar:** [EncounterController.cs:L135-151](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Encounter/EncounterController.cs#L135-L151) bloğunda düşman spawn edildikten sonra `SpriteRenderer` materyalini `Shader.Find("Sprites/Default")` ile oluşturulmuş unlit bir materyal ile güncelleyin.
  - **Boss:** [RoomRunDirector.cs:L882-895](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L882-L895) dosyasında boss spawn edildikten sonra benzer şekilde materyalini unlit yapın.
* **Prefab Serialize (Inspector):** Editor-only `AssetDatabase` veya eksik `Resources` yollarına güvenmek yerine, fallback prefabları `RoomRunDirector` inspector bileşeninde doğrudan sürükleyip bırakarak (serialize) atayın.

---

### 2) Skill-Icon Boş / Boş Bar Sorunu

#### **Kök Nedenler**
1. [DraftManager.cs:L70-L74](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/Skills/DraftManager.cs#L70-L74) dosyasında starter kit'ler (`ClassKits`) sadece `Warblade` ve `Elementalist` için tanımlanmıştır.
2. Diğer sınıflar (`Ranger`, `Shadowblade` vb.) seçildiğinde başlangıç kit becerileri tanımlı olmadığından, ilk oda başladığında beceri slotları doldurulamaz ve skill bar tamamen boş kalır.
3. `ClassUnlockPolicy` varsayılan olarak sadece `Warblade` ve `Elementalist` sınıflarını açık tutmaktadır. Diğer sınıflar kilitlidir; ancak debug veya hileyle seçilmeleri durumunda boş bar hatası kaçınılmazdır.

#### **En Yalın Çözüm (Demo Sınırlandırması)**
* **Sınıfları 2'ye Sınırla (EN DÜŞÜK RİSK):** Sunuma birkaç saat kalmışken diğer sınıflar için kit becerileri eklemeye çalışmak ve bunların skill controller entegrasyonlarındaki olası bug'ları çözmeye uğraşmak aşırı risklidir.
* En güvenli yol, [CharacterSelectController.cs:L158-L170](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/CharacterSelectController.cs#L158-L170) dosyasındaki `GetDefaultClasses()` listesini sadece **Warblade** ve **Elementalist** dönecek şekilde kısıtlamaktır. Gridde sadece bu 2 stabil sınıf görünecek ve sunum sıfır riskle tamamlanacaktır.

---

### 3) 8-P0 Cleanup Önceliklendirmesi (Yalın Sıra)

#### **Şart Olanlar (Mandatory)**
1. **`MainMenuScreen.AutoInit` Devre Dışı Bırakılması (SART):** [MainMenuScreen.cs:L25-L34](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/MainMenuScreen.cs#L25-L34) dosyasındaki legacy `[RuntimeInitializeOnLoadMethod]` niteliğine sahip metodu devre dışı bırakın (veya silin). Aksi takdirde, sahneler arası geçişte eski menü arkada gizlice spawn olup Input ve EventSystem'i bozmaktadır.
2. **Build Settings Tek-Yol Entegrasyonu (SART):** `EditorBuildSettings.asset` dosyasında sadece `MainMenu`, `CharacterSelect` ve `_Arena` sahnelerinin açık (enabled: 1) olduğundan emin olun. Sahneler arası geçiş akışı bu 3 ana omurgaya sadık kalmalıdır.
3. **`DraftManager` Depth Güncellemesi (SART):** Draft derinlik hesaplamasının `RoomRunDirector.CurrentNodeId + 1` üzerinden ilerlemesi. Bu zaten `GetLiveRoomDepth()` ile halihazırda projenize gömülmüştür.

#### **Erteleme / Over-kill / Zaten Yapılmış Olanlar**
* **`boss->victory` (Erteleme / Over-kill):** RIMA demosunun ana can damarlarından biri, boss öldükten sonra attune olunan dual-class sinerjisini (`Warblade + Elementalist` kombinasyonunu) oyuncuya son odada (`post-boss Combat` odası) oynatarak hissettirmektir. Boss ölümünden hemen sonra direkt Victory ekranına atlamak bu deneyimi öldürür. Mevcut doğrusal 6 odalı akış (`Combat x3 → Merchant → Boss → Post-Boss Combat → Victory`) korunmalıdır.
* **`opening-draft timeout fallback` (Zaten Hazır):** [RoomRunDirector.cs:L248-261](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/MapDesigner/Room/Runtime/RoomRunDirector.cs#L248-L261) dosyasında 90 saniye içinde seçim yapılmazsa otomatik olarak ilk kit becerisini atayan koruma mekanizması zaten yazılmıştır.
* **`HUD/SkillBar retry` (Zaten Hazır):** [SkillBarUI.cs:L475-491](file:///F:/Antigravity%20Projeler/2d%20roguelite/RIMA/Assets/Scripts/UI/SkillBarUI.cs#L475-L491) dosyasında player geç spawn olsa bile controller'ları her 0.5 saniyede bir aramaya devam eden late resolution döngüsü zaten hazırdır.
* **`MapFragment prefab repoint` (Zaten Hazır):** Projedeki iki `MapFragment` prefabı da zaten canonical `Environment/MapFragment.cs` scriptine bağlıdır; legacy Core scriptine dair hiçbir referans kalmamıştır.
* **Kapı Konsolidasyonu (Zaten Hazır):** Canlı sahnelerde legacy `Gate`/`Portal` kodları zaten kullanılmamaktadır, tamamen `RoomRunExitDoorTrigger` sistemi aktiftir.

---

### 4) Test Otomasyonu Değerlendirmesi

#### **Test Otomasyonu Mantıklı Mı?**
* **MANTIKSIZ (Aşırı Mühendislik):** Birkaç saat sonra gerçekleşecek bir canlı sunum öncesinde test otomasyonu yazmak vakit kaybıdır. Sunum öncesi en kritik adım **manuel playtest** ve **build testleridir**. 

#### **Yine de Yazılacaksa En Yüksek Değerli 3 Test (Yalın Liste)**
Eğer sunum sonrasında veya QA ekipleri için otomatik regresyon testi yazılacaksa, en yüksek faydayı sağlayacak 3 test şunlardır:

1. **Düşman Prefab Sprite & Material Kontrolü (EditMode - Kolay):** 
   - `Assets/Prefabs/Enemies` altındaki tüm prefabları tarar. Her prefabın `SpriteRenderer.sprite != null` olduğunu doğrular ve materyalinin bir URP Lit materyali olmadığını (siyah render'ı önlemek için) assert eder.
2. **Sınıf Kit Kapsama Kontrolü (EditMode - Kolay):** 
   - `ClassUnlockPolicy` içinde kilitsiz/açık görünen her sınıfın `DraftManager.ClassKits` içinde 3 geçerli starter beceriye sahip olduğunu assert eder.
3. **Combat-Clear Softlock Kontrolü (PlayMode - Orta):** 
   - Test sahnesine oda şablonu yükler, düşmanları spawn eder, canlarını sıfırlayarak yok eder ve `RoomRunDirector.LifecycleState == RoomRunLifecycleState.DoorOpen` olduğunu ve kapı trigger collider'larının aktifleştiğini doğrular.

