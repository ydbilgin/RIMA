# RIMA Mekanik Uygulanabilirlik Raporu - Codex laurethayday

Scope: Analiz only. Kod degisikligi yapilmadi.

Okunan kaynaklar:
- STAGING/mechanic_refs/Youtube_Mina_The_Hollower_Full_Transcript.md
- STAGING/mechanic_refs/Youtube_61_Mechanics_Detailed.md
- STAGING/MECHANIC_ADDITIONS_SYNTHESIS_2026-06-03.md
- CURRENT_STATUS.md
- NLM query: Sundered Beat, reward draft, room, HP economy, dash/parry constraints

Not: `Youtube_61_Mechanics_Detailed.md` dosyasinda baslik olarak Mechanic 1-53 var. Dosya adi 61 diyor ama bu dosyada 54-61 basliklari yok. Onceki synthesis recovered-24 notlarini duplicate yapmamak icin yalnizca karar-etkisi olan yerlerde referansladim.

## Mevcut Kodda Zaten Var / Yakindan Var Olanlar

- Dynamic wave trigger zaten var: `Assets/Scripts/Encounter/EncounterController.cs` acilis dalgasinda olen oranini hesaplayip `activeWave.nextWaveKillFraction` ile 2. dalgayi baslatiyor. `Assets/ScriptableObjects/Encounters/Act1_Wave_Pilot.asset` icinde `nextWaveKillFraction: 0.5` var.
- 3 kart Hades draft zaten var: `Assets/Scripts/Skills/DraftManager.cs`, `Assets/Scripts/Skills/SkillOfferGenerator.cs`, `Assets/Scripts/UI/SkillOfferUI.cs`. RewardPickup -> draft -> kapi acma akisi `Assets/Scripts/Core/RewardPickup.cs` ve `RoomClearVictoryTrigger.cs` tarafinda var.
- Rarity/tier weighted draft zaten var: `SkillOfferGenerator.WeightedPick()` Common/Rare/Epic/Mythic/Legendary agirliklarini kullaniyor.
- BREAK/SUNDER state tracking zaten var: `Assets/Scripts/Skills/SkillStateTracker.cs` Broken 3 stack -> Sundered auto-escalate yapiyor.
- Sundered Beat chain window zaten var: `Assets/Scripts/Skills/ChainWindowTracker.cs` Death Blow / Sunder Mark chain interlocklarini takip ediyor.
- Combat event bus zaten var: `Assets/Scripts/Combat/CombatEventBus.cs` OnHit, OnKill, OnDash, OnStatusApplied, OnCommitBeat, OnTelegraph eventleri mevcut.
- Dash + i-frame zaten var: `Assets/Scripts/Player/PlayerController.cs` `TryDash()`, `Health.SetImmune(true)`, `CombatEventBus.PublishDash()` kullaniyor. Dash-parry yok.
- Health/heal API zaten var: `Assets/Scripts/Core/Health.cs`. Heal kullanimlari: `ChestBehavior.cs`, `DraftManager.cs`, `WarbladePassives.cs`. Canon'a gore free sustain yasak; combat heal state/lifesteal kosullu olmali.
- Gate/fragment/draft/unlock akisi yakindan var: `Assets/Scripts/Systems/Map/RoomLoader.cs` fragment pickup -> draft -> gate unlock ve reward room gate akisini yurutuyor.
- Cross-class Echo altyapisi zaten var: `Assets/Scripts/CrossClass/*`, `DraftManager.cs` Echo offer inject/bind yapiyor.
- Status/stun/slow/trap altyapisi var: `StatusEffectSystem`, Ranger/Shadowblade/Elementalist skilllerinde stun, chill, trap, mark kullaniliyor.

## Mina the Hollower - Uygulanabilir Mekanikler

- Hollow / burrow yerine phase-dash lane: Oyuncu kisa sure zeminin altina degil, cyan seal icinden faz gecip projectile hasarindan korunur ama melee/ground slam vurabilir. Yer: movement/combat. Feasibility: MED-HARD; `PlayerController`, collision layer mask, projectile/enemy hit kurallari, room walkability. Risk: canonical max 1 skill movement ve dash i-frame stack yasağı nedeniyle universal degil, tek class skill movement olmali.
- Burrow flank vs shielded enemy: Shieldli/guard eden mob oyuncuya doner; phase-through veya dash-in timing basariliysa arkasindan BREAK stack alir. Yer: combat/enemy. Feasibility: MED; `EnemyTelegraph`, enemy AI/facing, `SkillStateTracker.Broken`, `PlayerController.OnDash`. Bu, literal burrowdan daha RIMA-fit.
- Burrow under rocks -> pick/throw heavy object: Zemindeki void shard/stone prop dash veya interact ile kaldirilip tek kullanimlik agir projectile olur. Yer: room/combat. Feasibility: HARD; runtime prop physics, pickup state, throw projectile, enemy collision. RIMA icin yatirim getirisi su an dusuk.
- Implicit bypass puzzle: Combat disi guard/wall altindan gecmek yerine sealed barrierin zayif anindan faz gecme. Yer: room. Feasibility: MED-HARD; `GateBehavior`, `RoomLoader`, collision state. RIMA'nin run hizina puzzle gibi degil, riskli shortcut gibi oturur.
- Assist/modifier list: Damage taken/done, healing multiplier, AI tracking, telegraph visibility gibi opsiyonel accessibility toggles. Yer: meta/settings. Feasibility: EASY-MED; `SettingsMenu`, `Health.incomingDamageMultiplier`, enemy AI tracking, `EnemyTelegraph`. Risk: cok fazla modifier oyuncu deneyimini bozabilir; preset Assist Mode yeter.
- Assist Mode preset: Uzun modifier listesi yerine 1-2 preset: Relaxed Combat, Strong Telegraphs. Yer: meta/settings. Feasibility: EASY; `SettingsMenu`, PlayerPrefs, simple multipliers.
- Side arms as found-lost temporary tool: Odada bulunan, tek elde tasinan, run/oda sonunda kaybolan temporary Echo/sidearm. Yer: reward/combat. Feasibility: MED; `RewardPickup`, `DraftManager`, `PlayerCrossClassBinding`, maybe temporary slot. RIMA'da permanent skill draft ile cakismamasi icin "one-room relic" olmali.
- Bone-up 4 choice level-up: Bones yerine fragment/gold milestone'da oyun durup 4 micro-upgrade secimi. Yer: reward/meta. Feasibility: EASY-MED; draft UI zaten var. Ancak RIMA'da 3-card draft zaten var, bu dogrudan tekrar olur. Sadece meta vendor veya Elite-after bonus icin kullan.
- Death twice currency loss: Ilk olumde currency korunur, arka arkaya ikinci olumde run/meta currency kaybi. Yer: meta/death. Feasibility: MED-HARD; `DeathScreenManager`, `RunStats`, `PlayerEconomy`, save/meta currency. RIMA'da roguelite loopa uyabilir ama Phase-1 combat feel icin erken degil.

### Mina - RIMA'ya Uymayan / Elenen Fikirler

- Platform jump burrow: RIMA top-down iso ARPG; jump mesafesi ve ledge platforming ana hareket degil. Oda dash-lane kuralina cevrilmedikce uymaz.
- Literal wall-under traversal puzzles: Zelda-like overworld puzzle temposu RIMA'nin combat-room pacingini yavaslatir. Sadece optional shortcut olarak dusunulmeli.
- Guard altindan gecme town puzzle: RIMA'da social/town puzzle yok; run odasinda anlamli degil.
- Frequent map-random sidearm loss: Mina exploration haritasinda iyi; RIMA'da her oda draft ve Echo sistemi varken oyuncuya ana progressioni belirsiz hissettirebilir. Oda-ici temporary pickup daha guvenli.
- Large modifier spreadsheet: Accessibility icin iyi ama RIMA'nin balance/test yuzeyini patlatir. Preset ve 4-6 kritik toggle yeter.

## Youtube_61_Mechanics_Detailed - Uygulanabilir Mekanikler

- #2 Action-based reward/time bonus: EXECUTE, BREAK, parry, no-hit veya fast clear aksiyonlari run skor/gold/fragment bonusu verir. Yer: combat/reward. Feasibility: EASY-MED; `CombatEventBus`, `RunStats`, `PlayerEconomy`, `RewardPickup`. Onceki synthesis bunu secondary olarak ele almisti; yeni not: sure yarisi degil, aksiyon kalitesi bonusu olsun.
- #4 Momentum mitigation: Dash-cancel, roll-out, hit-recovery azaltma gibi hareket kesintilerini oyuncu becerisiyle azaltma. Yer: movement/combat. Feasibility: EASY-MED; `PlayerController`, `BasicAttackProfile.dashCancelWindow`, `HitStop`. Kismen var, tuning isi.
- #6 Aggressive parry vs colored attacks: Cyan-tagged projectile/telegraph'a dogru dash -> parry, hasar/stun/Broken. Yer: combat. Feasibility: MED; `CombatEventBus.OnDash/OnTelegraph`, `EnemyTelegraph`, projectile layer. Onceki synthesis #7+#33 ile ayni ana fikir; halen en iyi Sundered Beat sinerjisi.
- #9 Interlocked 3-variable upgrade loop: Range/speed/capacity gibi birbirini tetikleyen basit upgrade aileleri. Yer: reward/meta. Feasibility: EASY; `SkillData`, `SkillOfferGenerator`, passive upgrades. Mevcut tier draft icine pasif upgrade aileleri olarak oturur.
- #10 Extraction push/pull: Bir oda/mini-branch icinde "cik simdi veya ekstra risk al" karari. Yer: room/reward. Feasibility: HARD; inventory/extract yok, route/gate/extra reward state gerekir. Buyuk fikir icin degerli ama Phase-1 degil.
- #11 Door hub / route memory: Kapilarin preview ve geri baglanti hissi, random room path kararini daha okunur yapar. Yer: room/meta map. Feasibility: MED; `MapFlowManager`, `RoomLoader`, portal/gate visuals. RIMA'nin portal preview karariyla uyumlu.
- #12 Anchor web: Bir noktaya tether atip cekme, obje/dugme degil enemy/void edge odakli kullanma. Yer: combat/room. Feasibility: HARD; yeni tether object, line renderer, pull physics. Onceki Cyan Echo Anchor fikrinin en iyi kaynagi.
- #13 Dynamic wave spawn: Ilk wave %50-70 temizlenince sonraki wave gelir. Yer: room/combat. Feasibility: TRIVIAL; zaten `EncounterController` ve `nextWaveKillFraction` var. Sadece per-encounter tuning + spawn VFX gerekir.
- #14 Arkham hit/counter time dilation: Her hit/counter kisa hit-stop ve impact flash ile lone-warrior gucu verir. Yer: combat/juice. Feasibility: TRIVIAL-EASY; `HitStop`, `ScreenShakeDriver`, `ImpactFrameDriver`, `CombatEventBus`. Zaten kismen var, polish/tuning.
- #16 XP pickup heal: Combat icinde risk alarak pickup toplamak heal verir; oda sonunda kalan pickup auto-collect ama heal etmez veya az heal eder. Yer: combat/reward. Feasibility: EASY-MED; `Health.Heal`, `RewardPickup` pattern, new pickup prefab. Canon nedeniyle sadece EXECUTE/Broken-kill state-gated olursa uygun.
- #17 Auto loot management: Gold/junk/metacurrency otomatik toplanir veya run sonunda satilir. Yer: meta/QOL. Feasibility: EASY-MED; `PlayerEconomy`, pickup scripts, save. Su an agir loot inventory yok; gereksiz erken.
- #21 Card weight/draw speed: Agir/powerful card daha nadir veya gec reveal olur; hafif/common daha sik. Yer: reward. Feasibility: EASY; `SkillOfferGenerator.WeightedPick`, `SkillData.tier`, `SkillOfferUI` reveal timing. Onceki synthesis #26 card-weight ile zaten secildi.
- #23 Modular tool parts: Skilllerin handle/shaft/tip gibi parcali augmentleri olur; ayni skillin davranisi parca secimine gore degisir. Yer: reward/meta. Feasibility: MED-HARD; `SkillData`, upgrade data, `DraftManager`, per-skill modifiers. RIMA'da Legendary upgrade sistemiyle sinirli uygulanmali.
- #26 Mid-fight hacking: Broken/Sundered hedefe kisa riskli channel/skill-check yapip execute damage multiplier al. Yer: combat. Feasibility: MED-HARD; `SkillStateTracker`, `DeathBlow`, UI/input channel, interrupt rules. Onceki synthesis belirtti; iyi ama rhythm'i bozmamali.
- #27 Dash+parry same input: Dusa dogru dash dogru timingde parry sayilir. Yer: movement/combat. Feasibility: MED; `PlayerController.TryDash`, `CombatEventBus.OnDash`, telegraph/projectile tags. #6 ile ayni uygulama.
- #30 Special enemy focus switch: Odaya nadir special girer ve oyuncunun onceligini aninda degistirir (healer, grabber, shield, summoner). Yer: combat/enemy. Feasibility: MED; enemy prefabs, `EncounterBankSO`, `ThreatBudget`, `EnemyTelegraph`. RIMA icin yuksek degerli, daha az sistemik risk.
- #31 Shield-lowered-by-kick analog: Shieldli enemy her zaman yuzunu doner; commit-beat/dash-parry/BREAK onu kisa sure acik birakir. Yer: combat/enemy. Feasibility: EASY-MED; enemy state, `SkillStateTracker.Broken`, `Health.TakeDamage` gate. Mina burrow flank fikrinin daha ucuz hali.
- #32 Ricochet shot: Ranger projectile tavadan/duvardan seker, puzzle-combat line solve yaratir. Yer: combat/room. Feasibility: HARD; projectile reflection, collision normals, authored bounce surfaces. Dusuk oncelik.
- #33 Fine movement / strafe control: Narrow passage veya heavy enemy fight icin yavas, hassas aim/facing modu. Yer: movement. Feasibility: MED; `PlayerController`, aim mode, input. Kismen dash/facing/mouse mode var; yeni mod eklemek kontrol karmasasi yaratabilir.
- #35 One-held item pickup: Oda icinde tek item tutulur, zamanlama/level geometriyle hem avantaj hem risk yaratir. Yer: room/combat. Feasibility: MED; temporary pickup slot, item activation, `RewardPickup` or new component. Mina sidearm fikriyle birlestirilebilir.
- #37 Stamina-based card influence: Iyi kartlar daha pahali stamina/resource ister; draft buildinde sadece alma degil oynatma maliyeti de karar olur. Yer: reward/meta. Feasibility: MED; skill costs/resources, `SkillData`, class controllers. Onceki synthesis recovered-24 icinde belirtmis; simdilik tasarim pass.
- #38 Reactor overload: Odada merkezi cyan seal countdown; durduramazsan oda hazard/boss buff/void burst olur. Yer: room/combat. Feasibility: HARD; room objective state, UI, enemy AI pressure, Gate/RoomLoader. Buyuk fikir olarak degerli.
- #41 Timed unfolding platform: Kisa sure acilan/kapanan floor/bridge/hazard. Yer: room. Feasibility: MED; Tilemap/collider toggles, `RoomTemplateSO`, telegraph VFX. Combat arenada mikro hazard olarak uyabilir.
- #43 Resonator: Ses/cyan pulse ile enemy yavaslatma ve barrier kaldirma; kaldirilan bariyer oyuncuyu da aciga cikarir. Yer: combat/room. Feasibility: MED; `StatusEffectSystem`, `GateBehavior`, barrier GameObject, `CombatEventBus.OnStatusApplied`. RIMA temasiyla cok uyumlu ve Sundered Beat'e iyi baglanir.
- #45 Persistent failure tracks: Olum/deneme sonrasi oda icinde onceki hareket izi veya danger memory kalir. Yer: meta/room. Feasibility: EASY-MED; `RunStats`, death snapshot, VFX trail. Roguelite run reset yapisinda sadece same-room retry varsa anlamli.
- #48 Returning projectile reload: Projectile geri dondukce tekrar atis hakki gelir; yakin dusman clutch fire rate yaratir. Yer: combat/class. Feasibility: MED-HARD; Ranger/Gunslinger projectile lifecycle, ammo/return logic. Spesifik class skill olarak iyi.
- #51 Death card: Olunce deck/skill parcalarindan sonraki run icin custom Echo/curse/relic uretilir. Yer: meta. Feasibility: HARD; meta progression, save, SkillData runtime generation. Onceki synthesis #59 olarak belirtti; buyuk meta yatirimi.
- #52 Self-damage bash: Anahtar/fragment yoksa HP odeyerek kapi/acik sandik/shortcut acarsin. Yer: room/reward. Feasibility: EASY-MED; `Health.TakeDamage`, `GateBehavior`, `RoomLoader`, draft/gate state. Canon HP baskisiyla uyumlu.
- #53 Nightmare fuel: Run boyunca biriken dread meter deck/draft havuzuna curse ekler; dusurmek icin riskli kararlar gerekir. Yer: meta/reward. Feasibility: HARD; run state, draft pollution, UI, save. Phase-2/Act-2 tension sistemi olabilir.

## Youtube_61 - Elenen veya Zayif Fit

- #1 Ball dribble/platform burden: Platformer physics uzerine kurulu; RIMA'da prop/sidearm olarak ancak dolayli kullanilir.
- #3 QTE typing: RIMA'nin commit-beat combatina ters; UI QTE savasa friction ekler.
- #7 Management-sim time acceleration: ARPG room loopunda gereksiz; sadece crafting meta varsa anlamli.
- #8 Co-op hot potato: RIMA single-player odakli.
- #15 Cover shooter: Top-down melee/ranged ARPG icin cover lock sistemi fazla agir.
- #18 Tile movement programming: Puzzle oyunu yapisi; RIMA combat odasina uymaz.
- #19 Ghost Trick sequence rewind: Puzzle narrative; RIMA'da sadece trap tutorial icin dolayli.
- #20 Survival inventory slot friction: RIMA loot/inventory loopunda hedef degil.
- #22 Detective anytime-accuse: Genre disi.
- #24 Ambient input idle: Genre disi.
- #25 Limb climbing, #36 surf cloud, #39 wall-run, #47 manual walking: traversal/platformer kaynakli; iso ARPG hareket kurallarina ters.
- #28 Typing attrition, #29 timing QTE: Input modelini bozar.
- #34 Context-sensitive Pikmin units: RIMA'da unit-command yok; sadece auto-target Echo icin dolayli.
- #40 Recursive levels, #42 separate hand/camera, #49 blindfold, #50 OS interaction: Genre disi veya scope disi.
- #44 No-walk follower illusion: Horror/time-saver; RIMA icin mekanik deger dusuk.
- #46 Surprise RTS: Kisa setpiece olabilir ama core roguelite loopu icin pahali.

## En Yuksek Deger / Dusuk Efor 5 Quick Win

1. Dynamic wave tuning + cyan spawn tell. Feasibility: TRIVIAL. Zaten `EncounterController.nextWaveKillFraction` var; per-room asset values ve spawn VFX yeter. Combat akisini hemen hizlandirir.
2. Hit/counter micro time-dilation tuning. Feasibility: TRIVIAL-EASY. `HitStop`, `ScreenShakeDriver`, `ImpactFrameDriver`, `CombatEventBus` hazir. Arkham hissini RIMA olceginde verir.
3. Shielded enemy lowered by BREAK/commit-beat. Feasibility: EASY-MED. Yeni enemy state + `SkillStateTracker.Broken` gate. Mina burrow flank ve Mouse PI shield fikrinin ucuz, RIMA-fit hali.
4. State-gated Echo Mote heal. Feasibility: EASY-MED. `Health.Heal`, `CombatEventBus.OnKill`, `SkillStateTracker` hazir. Canon geregi yalniz Broken/Sundered/EXECUTE kill gibi kazanilmis kosulda olmali.
5. Self-damage bash for optional gate/chest. Feasibility: EASY-MED. `Health.TakeDamage` + `GateBehavior`/`RoomLoader` state. HP ekonomisini bozmadan risk-reward ekler.

## 2 Buyuk Fikir / Yatirim Degeri

1. Cyan Echo Anchor + Resonator hybrid: Dash-parry veya resonator pulse cyan anchor birakir; anchor enemy pull, edge pressure, class-state detonation ve barrier-risk kararlarini baglar. Feasibility: HARD. Dokunacagi sistemler: `PlayerController`, `CombatEventBus`, `SkillStateTracker`, `StatusEffectSystem`, room edge/void collision, VFX. Deger: Sundered Beat'i sadece damage degil, uzamsal oda mekanigi yapar.
2. Void Dread / Death Card meta: Olum veya kotu oda kararlarindan dread birikir; draft havuzuna curse/death-card Echo ekler, oyuncu sonraki run icin kendine ait bir risk/guclenme parcasi uretir. Feasibility: HARD. Dokunacagi sistemler: `DeathScreenManager`, `RunStats`, save/meta currency, `DraftManager`, `SkillData`. Deger: roguelite kimligini guclendirir ama Phase-1 combat polish sonrasi.

## Sundered Beat (BREAK -> EXECUTE) Sinerji Listesi

- Dash-parry / cyan-tag counter (#6/#27): Dash'i kacis olmaktan cikarip BREAK baslatan agresif ritim girisine cevirir.
- Shield-lower enemy (#31 + Mina shield flank): Commit-beat veya Broken stack enemy guardini acar; EXECUTE penceresi okunur olur.
- Resonator (#43): Pulse enemy'yi yavaslatir veya Sundered penceresini sabitler; barrier kaldirma karari oyuncuyu EXECUTE kovalamaya iter.
- State-gated Echo Mote heal (#16): Heal sadece Broken/Sundered/EXECUTE kill'den geldiginde Sundered Beat'i hayatta kalma ekonomisine baglar.
- Dynamic wave (#13): Yeni dalga erken girince oyuncu BREAK->EXECUTE chain'i kesintisiz surdurur; oda temposu Hades'e yaklasir.
- Mid-fight hacking (#26): Broken/Sundered hedef uzerinde riskli channel ile execute multiplier verir; ancak combat UI QTE'ye donmemesi sart.
- Cyan Echo Anchor (#12/#43): BREAK noktasi uzamsal anchor olur; EXECUTE sadece hedef oldurme degil, oda geometrisini kullanma kararina donusur.
- Self-damage bash (#52): HP feda ederek ekstra combat/reward acmak, Sundered Beat kaynakli heal/lifesteal degeriyle dogrudan baglanir.
- Returning projectile reload (#48): Ranger/Gunslinger icin EXECUTE ya da Broken hit projectile return hizini etkileyebilir; clutch hissi yaratir.

## Kisa Karar Notu

- Literal Mina burrow RIMA'ya oldugu gibi alinmamali. En dogru ceviri: phase-dash / flank / shield-break / cyan anchor.
- Onceki synthesis'teki #14 dynamic wave, #26 card weight, #17 Echo Mote, #7+#33 Sundered Counter halen dogru. Yeni ek sinyal: #31 shield-lower enemy ve #43 Resonator, Sundered Beat'e dusuk-orta eforla daha iyi combat okunurlugu katar.
- Ilk implement sirasinda kod degil tasarim karari gerekiyorsa: Quick wins 1-3 mekanik risk dusuk; Big Idea 1 icin once Sundered Counter prototipi sart.

# RIMA Mekanik Uygulanabilirlik Raporu - Codex laurethayday

Scope: Analiz only. Kod degisikligi yapilmadi.

Okunan kaynaklar:
- STAGING/mechanic_refs/Youtube_Mina_The_Hollower_Full_Transcript.md
- STAGING/mechanic_refs/Youtube_61_Mechanics_Detailed.md
- STAGING/MECHANIC_ADDITIONS_SYNTHESIS_2026-06-03.md
- CURRENT_STATUS.md
- NLM query: Sundered Beat, reward draft, room, HP economy, dash/parry constraints

Not: `Youtube_61_Mechanics_Detailed.md` dosyasinda baslik olarak Mechanic 1-53 var. Dosya adi 61 diyor ama bu dosyada 54-61 basliklari yok. Onceki synthesis recovered-24 notlarini duplicate yapmamak icin yalnizca karar-etkisi olan yerlerde referansladim.

## Mevcut Kodda Zaten Var / Yakindan Var Olanlar

- Dynamic wave trigger zaten var: `Assets/Scripts/Encounter/EncounterController.cs` acilis dalgasinda olen oranini hesaplayip `activeWave.nextWaveKillFraction` ile 2. dalgayi baslatiyor. `Assets/ScriptableObjects/Encounters/Act1_Wave_Pilot.asset` icinde `nextWaveKillFraction: 0.5` var.
- 3 kart Hades draft zaten var: `Assets/Scripts/Skills/DraftManager.cs`, `Assets/Scripts/Skills/SkillOfferGenerator.cs`, `Assets/Scripts/UI/SkillOfferUI.cs`. RewardPickup -> draft -> kapi acma akisi `Assets/Scripts/Core/RewardPickup.cs` ve `RoomClearVictoryTrigger.cs` tarafinda var.
- Rarity/tier weighted draft zaten var: `SkillOfferGenerator.WeightedPick()` Common/Rare/Epic/Mythic/Legendary agirliklarini kullaniyor.
- BREAK/SUNDER state tracking zaten var: `Assets/Scripts/Skills/SkillStateTracker.cs` Broken 3 stack -> Sundered auto-escalate yapiyor.
- Sundered Beat chain window zaten var: `Assets/Scripts/Skills/ChainWindowTracker.cs` Death Blow / Sunder Mark chain interlocklarini takip ediyor.
- Combat event bus zaten var: `Assets/Scripts/Combat/CombatEventBus.cs` OnHit, OnKill, OnDash, OnStatusApplied, OnCommitBeat, OnTelegraph eventleri mevcut.
- Dash + i-frame zaten var: `Assets/Scripts/Player/PlayerController.cs` `TryDash()`, `Health.SetImmune(true)`, `CombatEventBus.PublishDash()` kullaniyor. Dash-parry yok.
- Health/heal API zaten var: `Assets/Scripts/Core/Health.cs`. Heal kullanimlari: `ChestBehavior.cs`, `DraftManager.cs`, `WarbladePassives.cs`. Canon'a gore free sustain yasak; combat heal state/lifesteal kosullu olmali.
- Gate/fragment/draft/unlock akisi yakindan var: `Assets/Scripts/Systems/Map/RoomLoader.cs` fragment pickup -> draft -> gate unlock ve reward room gate akisini yurutuyor.
- Cross-class Echo altyapisi zaten var: `Assets/Scripts/CrossClass/*`, `DraftManager.cs` Echo offer inject/bind yapiyor.
- Status/stun/slow/trap altyapisi var: `StatusEffectSystem`, Ranger/Shadowblade/Elementalist skilllerinde stun, chill, trap, mark kullaniliyor.

## Mina the Hollower - Uygulanabilir Mekanikler

- Hollow / burrow yerine phase-dash lane: Oyuncu kisa sure zeminin altina degil, cyan seal icinden faz gecip projectile hasarindan korunur ama melee/ground slam vurabilir. Yer: movement/combat. Feasibility: MED-HARD; `PlayerController`, collision layer mask, projectile/enemy hit kurallari, room walkability. Risk: canonical max 1 skill movement ve dash i-frame stack yasağı nedeniyle universal degil, tek class skill movement olmali.
- Burrow flank vs shielded enemy: Shieldli/guard eden mob oyuncuya doner; phase-through veya dash-in timing basariliysa arkasindan BREAK stack alir. Yer: combat/enemy. Feasibility: MED; `EnemyTelegraph`, enemy AI/facing, `SkillStateTracker.Broken`, `PlayerController.OnDash`. Bu, literal burrowdan daha RIMA-fit.
- Burrow under rocks -> pick/throw heavy object: Zemindeki void shard/stone prop dash veya interact ile kaldirilip tek kullanimlik agir projectile olur. Yer: room/combat. Feasibility: HARD; runtime prop physics, pickup state, throw projectile, enemy collision. RIMA icin yatirim getirisi su an dusuk.
- Implicit bypass puzzle: Combat disi guard/wall altindan gecmek yerine sealed barrierin zayif anindan faz gecme. Yer: room. Feasibility: MED-HARD; `GateBehavior`, `RoomLoader`, collision state. RIMA'nin run hizina puzzle gibi degil, riskli shortcut gibi oturur.
- Assist/modifier list: Damage taken/done, healing multiplier, AI tracking, telegraph visibility gibi opsiyonel accessibility toggles. Yer: meta/settings. Feasibility: EASY-MED; `SettingsMenu`, `Health.incomingDamageMultiplier`, enemy AI tracking, `EnemyTelegraph`. Risk: cok fazla modifier oyuncu deneyimini bozabilir; preset Assist Mode yeter.
- Assist Mode preset: Uzun modifier listesi yerine 1-2 preset: Relaxed Combat, Strong Telegraphs. Yer: meta/settings. Feasibility: EASY; `SettingsMenu`, PlayerPrefs, simple multipliers.
- Side arms as found-lost temporary tool: Odada bulunan, tek elde tasinan, run/oda sonunda kaybolan temporary Echo/sidearm. Yer: reward/combat. Feasibility: MED; `RewardPickup`, `DraftManager`, `PlayerCrossClassBinding`, maybe temporary slot. RIMA'da permanent skill draft ile cakismamasi icin "one-room relic" olmali.
- Bone-up 4 choice level-up: Bones yerine fragment/gold milestone'da oyun durup 4 micro-upgrade secimi. Yer: reward/meta. Feasibility: EASY-MED; draft UI zaten var. Ancak RIMA'da 3-card draft zaten var, bu dogrudan tekrar olur. Sadece meta vendor veya Elite-after bonus icin kullan.
- Death twice currency loss: Ilk olumde currency korunur, arka arkaya ikinci olumde run/meta currency kaybi. Yer: meta/death. Feasibility: MED-HARD; `DeathScreenManager`, `RunStats`, `PlayerEconomy`, save/meta currency. RIMA'da roguelite loopa uyabilir ama Phase-1 combat feel icin erken degil.

### Mina - RIMA'ya Uymayan / Elenen Fikirler

- Platform jump burrow: RIMA top-down iso ARPG; jump mesafesi ve ledge platforming ana hareket degil. Oda dash-lane kuralina cevrilmedikce uymaz.
- Literal wall-under traversal puzzles: Zelda-like overworld puzzle temposu RIMA'nin combat-room pacingini yavaslatir. Sadece optional shortcut olarak dusunulmeli.
- Guard altindan gecme town puzzle: RIMA'da social/town puzzle yok; run odasinda anlamli degil.
- Frequent map-random sidearm loss: Mina exploration haritasinda iyi; RIMA'da her oda draft ve Echo sistemi varken oyuncuya ana progressioni belirsiz hissettirebilir. Oda-ici temporary pickup daha guvenli.
- Large modifier spreadsheet: Accessibility icin iyi ama RIMA'nin balance/test yuzeyini patlatir. Preset ve 4-6 kritik toggle yeter.

## Youtube_61_Mechanics_Detailed - Uygulanabilir Mekanikler

- #2 Action-based reward/time bonus: EXECUTE, BREAK, parry, no-hit veya fast clear aksiyonlari run skor/gold/fragment bonusu verir. Yer: combat/reward. Feasibility: EASY-MED; `CombatEventBus`, `RunStats`, `PlayerEconomy`, `RewardPickup`. Onceki synthesis bunu secondary olarak ele almisti; yeni not: sure yarisi degil, aksiyon kalitesi bonusu olsun.
- #4 Momentum mitigation: Dash-cancel, roll-out, hit-recovery azaltma gibi hareket kesintilerini oyuncu becerisiyle azaltma. Yer: movement/combat. Feasibility: EASY-MED; `PlayerController`, `BasicAttackProfile.dashCancelWindow`, `HitStop`. Kismen var, tuning isi.
- #6 Aggressive parry vs colored attacks: Cyan-tagged projectile/telegraph'a dogru dash -> parry, hasar/stun/Broken. Yer: combat. Feasibility: MED; `CombatEventBus.OnDash/OnTelegraph`, `EnemyTelegraph`, projectile layer. Onceki synthesis #7+#33 ile ayni ana fikir; halen en iyi Sundered Beat sinerjisi.
- #9 Interlocked 3-variable upgrade loop: Range/speed/capacity gibi birbirini tetikleyen basit upgrade aileleri. Yer: reward/meta. Feasibility: EASY; `SkillData`, `SkillOfferGenerator`, passive upgrades. Mevcut tier draft icine pasif upgrade aileleri olarak oturur.
- #10 Extraction push/pull: Bir oda/mini-branch icinde "cik simdi veya ekstra risk al" karari. Yer: room/reward. Feasibility: HARD; inventory/extract yok, route/gate/extra reward state gerekir. Buyuk fikir icin degerli ama Phase-1 degil.
- #11 Door hub / route memory: Kapilarin preview ve geri baglanti hissi, random room path kararini daha okunur yapar. Yer: room/meta map. Feasibility: MED; `MapFlowManager`, `RoomLoader`, portal/gate visuals. RIMA'nin portal preview karariyla uyumlu.
- #12 Anchor web: Bir noktaya tether atip cekme, obje/dugme degil enemy/void edge odakli kullanma. Yer: combat/room. Feasibility: HARD; yeni tether object, line renderer, pull physics. Onceki Cyan Echo Anchor fikrinin en iyi kaynagi.
- #13 Dynamic wave spawn: Ilk wave %50-70 temizlenince sonraki wave gelir. Yer: room/combat. Feasibility: TRIVIAL; zaten `EncounterController` ve `nextWaveKillFraction` var. Sadece per-encounter tuning + spawn VFX gerekir.
- #14 Arkham hit/counter time dilation: Her hit/counter kisa hit-stop ve impact flash ile lone-warrior gucu verir. Yer: combat/juice. Feasibility: TRIVIAL-EASY; `HitStop`, `ScreenShakeDriver`, `ImpactFrameDriver`, `CombatEventBus`. Zaten kismen var, polish/tuning.
- #16 XP pickup heal: Combat icinde risk alarak pickup toplamak heal verir; oda sonunda kalan pickup auto-collect ama heal etmez veya az heal eder. Yer: combat/reward. Feasibility: EASY-MED; `Health.Heal`, `RewardPickup` pattern, new pickup prefab. Canon nedeniyle sadece EXECUTE/Broken-kill state-gated olursa uygun.
- #17 Auto loot management: Gold/junk/metacurrency otomatik toplanir veya run sonunda satilir. Yer: meta/QOL. Feasibility: EASY-MED; `PlayerEconomy`, pickup scripts, save. Su an agir loot inventory yok; gereksiz erken.
- #21 Card weight/draw speed: Agir/powerful card daha nadir veya gec reveal olur; hafif/common daha sik. Yer: reward. Feasibility: EASY; `SkillOfferGenerator.WeightedPick`, `SkillData.tier`, `SkillOfferUI` reveal timing. Onceki synthesis #26 card-weight ile zaten secildi.
- #23 Modular tool parts: Skilllerin handle/shaft/tip gibi parcali augmentleri olur; ayni skillin davranisi parca secimine gore degisir. Yer: reward/meta. Feasibility: MED-HARD; `SkillData`, upgrade data, `DraftManager`, per-skill modifiers. RIMA'da Legendary upgrade sistemiyle sinirli uygulanmali.
- #26 Mid-fight hacking: Broken/Sundered hedefe kisa riskli channel/skill-check yapip execute damage multiplier al. Yer: combat. Feasibility: MED-HARD; `SkillStateTracker`, `DeathBlow`, UI/input channel, interrupt rules. Onceki synthesis belirtti; iyi ama rhythm'i bozmamali.
- #27 Dash+parry same input: Dusa dogru dash dogru timingde parry sayilir. Yer: movement/combat. Feasibility: MED; `PlayerController.TryDash`, `CombatEventBus.OnDash`, telegraph/projectile tags. #6 ile ayni uygulama.
- #30 Special enemy focus switch: Odaya nadir special girer ve oyuncunun onceligini aninda degistirir (healer, grabber, shield, summoner). Yer: combat/enemy. Feasibility: MED; enemy prefabs, `EncounterBankSO`, `ThreatBudget`, `EnemyTelegraph`. RIMA icin yuksek degerli, daha az sistemik risk.
- #31 Shield-lowered-by-kick analog: Shieldli enemy her zaman yuzunu doner; commit-beat/dash-parry/BREAK onu kisa sure acik birakir. Yer: combat/enemy. Feasibility: EASY-MED; enemy state, `SkillStateTracker.Broken`, `Health.TakeDamage` gate. Mina burrow flank fikrinin daha ucuz hali.
- #32 Ricochet shot: Ranger projectile tavadan/duvardan seker, puzzle-combat line solve yaratir. Yer: combat/room. Feasibility: HARD; projectile reflection, collision normals, authored bounce surfaces. Dusuk oncelik.
- #33 Fine movement / strafe control: Narrow passage veya heavy enemy fight icin yavas, hassas aim/facing modu. Yer: movement. Feasibility: MED; `PlayerController`, aim mode, input. Kismen dash/facing/mouse mode var; yeni mod eklemek kontrol karmasasi yaratabilir.
- #35 One-held item pickup: Oda icinde tek item tutulur, zamanlama/level geometriyle hem avantaj hem risk yaratir. Yer: room/combat. Feasibility: MED; temporary pickup slot, item activation, `RewardPickup` or new component. Mina sidearm fikriyle birlestirilebilir.
- #37 Stamina-based card influence: Iyi kartlar daha pahali stamina/resource ister; draft buildinde sadece alma degil oynatma maliyeti de karar olur. Yer: reward/meta. Feasibility: MED; skill costs/resources, `SkillData`, class controllers. Onceki synthesis recovered-24 icinde belirtmis; simdilik tasarim pass.
- #38 Reactor overload: Odada merkezi cyan seal countdown; durduramazsan oda hazard/boss buff/void burst olur. Yer: room/combat. Feasibility: HARD; room objective state, UI, enemy AI pressure, Gate/RoomLoader. Buyuk fikir olarak degerli.
- #41 Timed unfolding platform: Kisa sure acilan/kapanan floor/bridge/hazard. Yer: room. Feasibility: MED; Tilemap/collider toggles, `RoomTemplateSO`, telegraph VFX. Combat arenada mikro hazard olarak uyabilir.
- #43 Resonator: Ses/cyan pulse ile enemy yavaslatma ve barrier kaldirma; kaldirilan bariyer oyuncuyu da aciga cikarir. Yer: combat/room. Feasibility: MED; `StatusEffectSystem`, `GateBehavior`, barrier GameObject, `CombatEventBus.OnStatusApplied`. RIMA temasiyla cok uyumlu ve Sundered Beat'e iyi baglanir.
- #45 Persistent failure tracks: Olum/deneme sonrasi oda icinde onceki hareket izi veya danger memory kalir. Yer: meta/room. Feasibility: EASY-MED; `RunStats`, death snapshot, VFX trail. Roguelite run reset yapisinda sadece same-room retry varsa anlamli.
- #48 Returning projectile reload: Projectile geri dondukce tekrar atis hakki gelir; yakin dusman clutch fire rate yaratir. Yer: combat/class. Feasibility: MED-HARD; Ranger/Gunslinger projectile lifecycle, ammo/return logic. Spesifik class skill olarak iyi.
- #51 Death card: Olunce deck/skill parcalarindan sonraki run icin custom Echo/curse/relic uretilir. Yer: meta. Feasibility: HARD; meta progression, save, SkillData runtime generation. Onceki synthesis #59 olarak belirtti; buyuk meta yatirimi.
- #52 Self-damage bash: Anahtar/fragment yoksa HP odeyerek kapi/acik sandik/shortcut acarsin. Yer: room/reward. Feasibility: EASY-MED; `Health.TakeDamage`, `GateBehavior`, `RoomLoader`, draft/gate state. Canon HP baskisiyla uyumlu.
- #53 Nightmare fuel: Run boyunca biriken dread meter deck/draft havuzuna curse ekler; dusurmek icin riskli kararlar gerekir. Yer: meta/reward. Feasibility: HARD; run state, draft pollution, UI, save. Phase-2/Act-2 tension sistemi olabilir.

## Youtube_61 - Elenen veya Zayif Fit

- #1 Ball dribble/platform burden: Platformer physics uzerine kurulu; RIMA'da prop/sidearm olarak ancak dolayli kullanilir.
- #3 QTE typing: RIMA'nin commit-beat combatina ters; UI QTE savasa friction ekler.
- #7 Management-sim time acceleration: ARPG room loopunda gereksiz; sadece crafting meta varsa anlamli.
- #8 Co-op hot potato: RIMA single-player odakli.
- #15 Cover shooter: Top-down melee/ranged ARPG icin cover lock sistemi fazla agir.
- #18 Tile movement programming: Puzzle oyunu yapisi; RIMA combat odasina uymaz.
- #19 Ghost Trick sequence rewind: Puzzle narrative; RIMA'da sadece trap tutorial icin dolayli.
- #20 Survival inventory slot friction: RIMA loot/inventory loopunda hedef degil.
- #22 Detective anytime-accuse: Genre disi.
- #24 Ambient input idle: Genre disi.
- #25 Limb climbing, #36 surf cloud, #39 wall-run, #47 manual walking: traversal/platformer kaynakli; iso ARPG hareket kurallarina ters.
- #28 Typing attrition, #29 timing QTE: Input modelini bozar.
- #34 Context-sensitive Pikmin units: RIMA'da unit-command yok; sadece auto-target Echo icin dolayli.
- #40 Recursive levels, #42 separate hand/camera, #49 blindfold, #50 OS interaction: Genre disi veya scope disi.
- #44 No-walk follower illusion: Horror/time-saver; RIMA icin mekanik deger dusuk.
- #46 Surprise RTS: Kisa setpiece olabilir ama core roguelite loopu icin pahali.

## En Yuksek Deger / Dusuk Efor 5 Quick Win

1. Dynamic wave tuning + cyan spawn tell. Feasibility: TRIVIAL. Zaten `EncounterController.nextWaveKillFraction` var; per-room asset values ve spawn VFX yeter. Combat akisini hemen hizlandirir.
2. Hit/counter micro time-dilation tuning. Feasibility: TRIVIAL-EASY. `HitStop`, `ScreenShakeDriver`, `ImpactFrameDriver`, `CombatEventBus` hazir. Arkham hissini RIMA olceginde verir.
3. Shielded enemy lowered by BREAK/commit-beat. Feasibility: EASY-MED. Yeni enemy state + `SkillStateTracker.Broken` gate. Mina burrow flank ve Mouse PI shield fikrinin ucuz, RIMA-fit hali.
4. State-gated Echo Mote heal. Feasibility: EASY-MED. `Health.Heal`, `CombatEventBus.OnKill`, `SkillStateTracker` hazir. Canon geregi yalniz Broken/Sundered/EXECUTE kill gibi kazanilmis kosulda olmali.
5. Self-damage bash for optional gate/chest. Feasibility: EASY-MED. `Health.TakeDamage` + `GateBehavior`/`RoomLoader` state. HP ekonomisini bozmadan risk-reward ekler.

## 2 Buyuk Fikir / Yatirim Degeri

1. Cyan Echo Anchor + Resonator hybrid: Dash-parry veya resonator pulse cyan anchor birakir; anchor enemy pull, edge pressure, class-state detonation ve barrier-risk kararlarini baglar. Feasibility: HARD. Dokunacagi sistemler: `PlayerController`, `CombatEventBus`, `SkillStateTracker`, `StatusEffectSystem`, room edge/void collision, VFX. Deger: Sundered Beat'i sadece damage degil, uzamsal oda mekanigi yapar.
2. Void Dread / Death Card meta: Olum veya kotu oda kararlarindan dread birikir; draft havuzuna curse/death-card Echo ekler, oyuncu sonraki run icin kendine ait bir risk/guclenme parcasi uretir. Feasibility: HARD. Dokunacagi sistemler: `DeathScreenManager`, `RunStats`, save/meta currency, `DraftManager`, `SkillData`. Deger: roguelite kimligini guclendirir ama Phase-1 combat polish sonrasi.

## Sundered Beat (BREAK -> EXECUTE) Sinerji Listesi

- Dash-parry / cyan-tag counter (#6/#27): Dash'i kacis olmaktan cikarip BREAK baslatan agresif ritim girisine cevirir.
- Shield-lower enemy (#31 + Mina shield flank): Commit-beat veya Broken stack enemy guardini acar; EXECUTE penceresi okunur olur.
- Resonator (#43): Pulse enemy'yi yavaslatir veya Sundered penceresini sabitler; barrier kaldirma karari oyuncuyu EXECUTE kovalamaya iter.
- State-gated Echo Mote heal (#16): Heal sadece Broken/Sundered/EXECUTE kill'den geldiginde Sundered Beat'i hayatta kalma ekonomisine baglar.
- Dynamic wave (#13): Yeni dalga erken girince oyuncu BREAK->EXECUTE chain'i kesintisiz surdurur; oda temposu Hades'e yaklasir.
- Mid-fight hacking (#26): Broken/Sundered hedef uzerinde riskli channel ile execute multiplier verir; ancak combat UI QTE'ye donmemesi sart.
- Cyan Echo Anchor (#12/#43): BREAK noktasi uzamsal anchor olur; EXECUTE sadece hedef oldurme degil, oda geometrisini kullanma kararina donusur.
- Self-damage bash (#52): HP feda ederek ekstra combat/reward acmak, Sundered Beat kaynakli heal/lifesteal degeriyle dogrudan baglanir.
- Returning projectile reload (#48): Ranger/Gunslinger icin EXECUTE ya da Broken hit projectile return hizini etkileyebilir; clutch hissi yaratir.

## Kisa Karar Notu

- Literal Mina burrow RIMA'ya oldugu gibi alinmamali. En dogru ceviri: phase-dash / flank / shield-break / cyan anchor.
- Onceki synthesis'teki #14 dynamic wave, #26 card weight, #17 Echo Mote, #7+#33 Sundered Counter halen dogru. Yeni ek sinyal: #31 shield-lower enemy ve #43 Resonator, Sundered Beat'e dusuk-orta eforla daha iyi combat okunurlugu katar.
- Ilk implement sirasinda kod degil tasarim karari gerekiyorsa: Quick wins 1-3 mekanik risk dusuk; Big Idea 1 icin once Sundered Counter prototipi sart.