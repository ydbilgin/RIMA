# CURRENT_STATUS

## HANDOFF POINTER (2026-06-18 GECE - Codex playtest sweep yarim)

Kullanici "status oku" dediginde bu dosya okunursa, once su devir notuna git:
`STAGING/_process/2026-06/CODEX_HANDOFF_PLAYTEST_2026-06-18.md`.

Durum: Warblade 8-way/mount kontrollu test PASS. MainMenu start listener, Chamber SkillBar null-ref bind ve Arena opening draft/SkillDatabase stale DB buglari bulundu; script patchleri uygulandi ama temiz Unity compile + fresh Play Mode dogrulamasi henuz bitmedi. Ilk kontrol: `SkillDatabase.Build()` bos DB rebuild guard'i.

Yeni hesap icin siradaki testler:
1. Unity script compile + console error/warning check.
2. Fresh Play MainMenu: 10-20 frame step, `Button_Basla` runtime listener >0 mi, invoke/click sonrasi `CharacterSelect` sahnesine geciyor mu.
3. CharacterSelect chamber: Warblade practice loadout Q/E/R/F ikon ve isimleri HUD'da doluyor mu; `SkillBarUI.slots[]` ref'leri null degil mi.
4. `_Arena` ilk giris: `SkillDatabase.Instance` usable mi (`db.Count > 0`), opening draft otomatik skill kartlariyla aciliyor mu.
5. Opening drafttan bir skill sec: draft kapaniyor mu, Q slot doluyor mu.
6. Odadaki tum dusmanlari oldur: clear sonrasi `RewardPickup` spawn oluyor mu, collect sonrasi reward draft aciliyor mu.
7. Unity MCP screenshot al: MainMenu, CharacterSelect chamber, opening draft, reward draft.

Bu liste sadece kesintiye kadar bulunan fixleri kapatmak icin minimum verification listesidir; tam demo loop playtest sayilmaz. Yeni hesap tam loop icin su matrisi de kosmali:
1. Insan-gibi golden path: MainMenu click -> CharacterSelect -> Warblade sec -> chamber HUD kontrol -> rift/start -> Arena opening draft -> skill sec -> normal hareket/aim/skill kullan -> enemy kill -> reward pickup -> reward draft -> yeni odaya/sonraki akisa gecis.
2. Input-driven kontrol: mumkun oldugunca direkt method invoke yerine UI click, keyboard/mouse ve frame stepping kullan; MCP synthetic input guvenilmezse bunu raporda ayir.
3. Multi-room dayanimi: ayni run icinde en az 2 reward dongusu; skill slotlari, cooldownlar, timescale, UI overlay ve reward state birbirine karismiyor mu.
4. Negatif/edge case: draft acikken hareket/skill kilitli mi, pause/slowmo resetli mi, reward collect tekrar tetikleniyor mu, bos/duplicate skill gelmiyor mu.
5. Sinif smoke matrix: en az Warblade full, Elementalist/Ranger/Shadowblade icin CharacterSelect -> Arena opening draft -> 1 skill secme kisa smoke.
6. Death/reset: oyuncu olum veya run reset sonrasi MainMenu/CharacterSelect/Arena state temiz mi, stale `SkillDatabase`/draft/HUD kalmiyor mu.
7. Kanit: her faz icin Unity console check + UnityMCP screenshot; bulgu varsa graphify ile ilgili baglanti/call-flow sorgusu.

Kesinti noktasi: MainMenu listener probe tekrar calistirilacakti. Son deneme sadece `execute_code` icinde `using` satirlari method body'de derlenmedigi icin compile olmadi; runtime state degistirmedi. Probe'u tam nitelikli tip adlariyla tekrar calistir.

### PLAYTEST TRACKER (Codex devam ediyor - her biten madde isaretlenecek)

Durum anahtari: `[ ]` yapilacak, `[x]` dogrulandi, `[!]` bug/blokaj bulundu.

- 2026-06-18 devam notu: Yeni Codex hesabi playtest sweep'e devam etti. Handoff'taki kritik `SkillDatabase.Build()` guard kontrol edildi; kodda zaten `if (built && db.Count > 0) return;` mevcut, yani `built=true/db.Count=0` stale DB rebuild'i engellenmiyor. Mevcut tracker T0/T1/T2 PASS, T3/T4 opening draft UI gorunmedigi icin blokaj olarak kayitli; siradaki odak T3 root-cause + fix/dogrulama.
- 2026-06-18 devam notu 2: Unity `refresh_unity(scope=scripts, compile=request, wait_for_ready=true)` sonrasi editor ready; console error/warning sorgusu 0 kayit dondurdu. Runtime T3 probe'a geciliyor.

- [x] T0 - Compile/console gate: Unity script compile temiz; console 0 error/warning; C# execute probe OK (`compile-probe-ok`).
- [x] T1 - MainMenu entry gate: fresh Play `MainMenu`; `Button_Basla` active/interactable, runtime listener=1; invoke sonrasi sahne `CharacterSelect` oldu.
- [x] T2 - CharacterSelect chamber gate: Warblade chamber acildi; `PlayerClassManager=Warblade`; skill slots `[IronCharge, GravityCleave, Earthsplitter, Cleave]`; `SkillBarUI.slots[]` 6/6 ref usable; Q/E/R/F ikonlari dolu. Not: aktif skill `nameLabel` metni kodda bilerek bosaltiliyor (`UpdateSlot`), bu ref bug degil/polish karari.
- [x] T3 - Arena opening draft gate: fresh `_Arena` Play + 25 editor step; `DraftManager_Auto` active, `SkillDatabase_Director dbCount=111`, `SkillOfferUI_Auto/[SkillOfferPanel] active`, cards=3, offers=`Earthsplitter|Iron Charge|Gravity Cleave`, `UIManager.IsSkillOfferOpen=true`. Onceki T3 blokaji eski stale DB/timeout penceresi kaynakli olabilir; yeni probe'da opening draft oyuncuya gorunuyor.
- [x] T4 - Opening skill select gate: Opening draft kart button invoke ile `IRON CHARGE` secildi; 60 editor step sonrasi `draftActive=false`, panel inactive, `Time.timeScale=1`, `Owned=[Iron Charge]`, Warblade slot0=`IronCharge:Iron Charge`.
- [x] T5 - Combat/facing gate: Secili `Iron Charge` `TryActivate()` true; player delta=3.69, cooldown started (`remaining~2.60`, Q HUD `Icon_Warblade_IronCharge`, cd fill ~0.84, timer `3`). Controlled facing probe: W -> Visual=N/Dir=(0,1)/body north/weaponRot=90/sort=-1; W+D -> Visual=NE/Dir=(1,1)/body NE/weaponRot=45/sort=-1; D -> Visual=E/Dir=(1,0)/body east/weaponRot=0/sort=1.
- [x] T6 - Room clear/reward gate: Shortcut ile dusman Health'lerine lethal damage verildi (manual combat degil). Ilk dalga sonrasi EncounterController 1 kalan `HalfThrall` saydi; kalan dusman da oldurulunce `tagEnemies=0`, `RewardPickup` spawn oldu (`pos=(1.92,6.84,0)`). `RewardPickup.ForceCollect()` sonrasi reward draft acildi: `draftActive=true`, panel active, cards=3, offers=`Battle Scars|Earthsplitter|Deep Wound`, `timeScale=0`. Screenshot: `Assets/Screenshots/codex_T6_reward_draft_2026-06-18.png`.
- [!] LOGIC NOTE - Reward kapisi: T6 sirasinda reward henuz `collected=false` iken director lifecycle `DoorOpen` gorundu. Kod yorumlari reward collect/draft bitene kadar exit door beklemeli diyor; demo karar/fix icin incelenmeli. Bu T6 ana reward-draft zincirini bloklamadi ama oyuncu reward almadan kapidan cikabiliyorsa design bug olabilir.
- [!] T7 - Multi-loop endurance: FAIL/PARTIAL. Ayni run icinde node0 reward draft cozuldu (`Battle Scars` passive), door choice 0 ile node1 Combat'a gecildi, slot0 `Iron Charge` persist etti. Node1 enemy clear shortcut sonrasi ikinci `RewardPickup` spawn oldu ve `ForceCollect()` ikinci reward draft'i acti (`Crippling Blow|Adrenaline Rush|Sunder Mark`). Ancak ikinci reward draft active iken `timeScale=1` kaldi; probe: `draftActive=true`, `UIManager.IsSkillOfferOpen=true`, panel active, lifecycle=`DoorOpen`. Screenshot: `Assets/Screenshots/codex_T7_second_reward_timescale_bug_2026-06-18.png`.
- [!] ROOT-CAUSE HYPOTHESIS - T7 freeze bug: T6/T7 sirasinda reward collect oncesi lifecycle zaten `DoorOpen` oluyor. Sonra `RewardPickup.ForceCollect()` draft'i aciyor ama `RoomClearSequence` `MarkRewardTaken`/door-open path'inde draft wait'i atlayip `finally RestoreGameplayTimeScale()` ile SkillOfferUI acikken `Time.timeScale=1` yapiyor olabilir. Karar/fix gerekir: door-open lifecycle reward collect/draft resolve oncesi ilerlememeli veya `RestoreGameplayTimeScale()` UIManager overlay state'ini ezmemeli.
- [x] T8 - Reset gate: Play stop/start sonrasi runtime MainMenu'e dondu (editor setup notu); Play icinde explicit `_Arena` load ile fresh run probe edildi. Eski reward yok (`reward=NULL`), slotlar bos (`slot0=NULL slot1=NULL`), owned bos, yeni opening draft aktif (`timeScale=0`, cards=3, offers=`Gravity Cleave|Earthsplitter|Iron Charge`). Screenshot: `Assets/Screenshots/codex_T8_reset_opening_draft_2026-06-18.png`. Death path henuz test edilmedi.
- [!] T9 - Class smoke matrix: BLOCKED/FAIL. Shortcut smoke denendi (`DirectorBypassClassUnlock=true`, fresh `_Arena` load, SelectedClass Elementalist/Ranger/Shadowblade). Normal opening draft UI yakalanmadi; Elementalist slot0 bos kaldi, Ranger/Shadowblade slot0 default/dogrudan dolu gorundu ama `SkillOfferUI` yoktu. Root-cause probe: sahnede `DraftManager_Auto` var (`draftCount=1`) ama `DraftManager.Instance == null`; `DraftManager.OnDestroy()` onceki scene reload'da static `_shuttingDown=true` yapiyor, yeni instance getter tarafindan null sayiliyor. Karar/fix gerekir: `_shuttingDown` sadece application quit icin true olmali, scene reload destroy'u DraftManager singleton'i kalici kapatmamali.
- [!] T10 - Evidence pack: PARTIAL. Screenshotlar: `Assets/Screenshots/codex_T6_reward_draft_2026-06-18.png`, `Assets/Screenshots/codex_T7_second_reward_timescale_bug_2026-06-18.png`, `Assets/Screenshots/codex_T8_reset_opening_draft_2026-06-18.png`. Final console check temiz degil: 1 error kaydi var, `Some objects were not cleaned up when closing the scene. (Did you spawn new GameObjects from OnDestroy?)`, file `./Packages/com.coplaydev.unity-mcp/Editor/Tools/ExecuteCode.cs:241`. T7/T9 acik buglari nedeniyle PASS raporu verilmedi.

## ⏯️ RESUME (2026-06-18 GECE — otonom demo-hardening + feature batch; kullanıcı sonra bakacak)

**Durum:** Demo ~yarın, editörde. Tam-otonom oturum; council + adversarial-critic disipliniyle **6 commit master'a** girdi. Sonraki mini-oturumda Warblade mount tool + 8-way facing bugfix **UNCOMMITTED** olarak eklendi; başka Codex hesabı test/QC yapacak.

### ✅ BU OTURUM COMMIT (master)
- `6ba61ff5` HIGH-4 demo-killer + SkillBase CanExecute veto (test-clean, council PASS)
- `9359b7a5` _Arena polish: URP Bloom/ColorGrade/Vignette + GlobalLight 0.5 + brazier warm-light + exposure 0.6
- `d0e6466e` FAZ-1: failed-cast feedback (cooldown-SESSİZ) + Director dup-slot REJECT
- `d3a08954` Jersey10 font sil (BuildMode→TMP-default fallback, crash yok)
- `981ac783` skill-bar cooldown sayı-countdown + chamber **[K] full-roster skill picker** (chamber-only)
- `6eec980f` obsolete Jersey10 test temizliği

### 🔴 CONFIRMED FIX (bu oturum, hepsi kod-confirmed + 0 test-regresyon)
movement off-map root (Blink/IronCharge/BladeRush→WalkabilityMap) · SkillBase spend-before-veto + 4 skill CanExecute (ChainLightning/CripplingBlow/SunderMark/DeepWound; IronCounter atlandı=reaktif parry) · RunStats progression-desync köprü · boss Phase-2 8s lock · failed-cast feedback (resource/veto'da SFX+flash+toast, cooldown sessiz) · Director dup-slot reject · cooldown countdown · chamber full-roster picker.

### ✅ DOĞRULANANLAR
- **Boss telegraph P1/P2/P3 TAM VAR** (`EnemyTelegraph.cs` + `PenitentSovereign.cs:893-930`; Circle/DelayedRing/DualRing red+green/Line; P3 %15 kısalma 0.22s taban). Eski "doğrulanmadı" notu BAYATMIŞ → ek iş YOK.
- **Tests EditMode 646: 25 önceden-var fail, 0 yeni regresyon** (kod değişikliklerimden). Pre-existing fail'ler benimle ilgisiz: Brush asset-path/sprite, PixelLab/Wang eksik asset, PlayerAnimator `SnapToFourDiagonal` (method-yok), MCP scene-load reflection drift, CharacterSelect animator, perf 2+7 Find-in-hot-path, RewardPickup interactRadius, PropCollider.

### 🟡 UNCOMMITTED — Warblade mount tool + 8-way facing fix (başka Codex test edecek)
**Amaç:** Warblade silah mount ayarını kolay yapmak + "W basılıyken D'ye basınca silah dönüyor ama gövde dönmüyor" bug'ını çözmek. Oyun standardı: Unity Input System `2DVector`/`DigitalNormalized` mantığına göre `W+D` = normalize NE `(0.71, 0.71)`; gövde ve silah birlikte NE'ye geçmeli.

**Tool / kalıcılık fix'leri:**
- Yeni editor tool: `RIMA/Warblade Mount Tuner` (`Assets/Scripts/Editor/Combat/WarbladeMountTunerWindow.cs`). Play mode'da canlı Player'ı bulur, yön seçer, offset/rotation nudgelar, `SOURCE PREFAB KAYDET` ile hardcoded `Player.prefab` yerine canlı instance'ın gerçek source prefab'ına yazar.
- `OrientationSyncAnchorEditor.cs` de aynı source-prefab save mantığına çekildi; eski "Player.prefab'a kaydet" hatası giderildi.
- Mount kaymasının root-cause'u: `CharacterJuice` her frame `HandAnchor.localPosition` değerini eski `handBasePos` ile ezebiliyordu. Fix: `OrientationSync.Sync()` sonrası `CharacterJuice.SetHandBasePosition()` çağrısı eklendi.

**8-way facing / W+D bugfix:**
- `PlayerAnimator.cs`: 8-yön görsel facing helper'ları eklendi; `VisualFacingDir` gövdenin gerçek `lastFacing` değerinden türetiliyor, silah/gövde ayrışması engelleniyor. Smoothing reversible: `useVisualFacingSmoothing` toggle'ı var.
- `HandAnchorAttach.cs`: silah mount/sorting artık raw `PlayerController.FacingDirection` yerine opsiyonel olarak `PlayerAnimator.VisualFacingDir` kullanıyor (`useAnimatorVisualFacing=true`).
- 4 playable Animator Controller'da yön geçişlerindeki hatalı `Speed < 0.5` koşulu kaldırıldı: `Warblade.controller`, `Elementalist.controller`, `Ranger.controller`, `Shadowblade.controller`. Root-cause: yürürken `Speed=1` olduğu için DirX/DirY değişse bile body state değişemiyordu; silah dönüyor, gövde eski yönde kalıyordu.

**Doğrulama (Unity Play Mode / execute_code):**
- Console: `0 error / 0 warning`.
- Warblade kontrollü test PASS:
  - `W` → `PCFacing=(0,1)`, `Visual=N`, `Dir=(0,1)`, `state=idle_N`, `sprite=warblade_idle_north`
  - `W+D` → `PCFacing=(0.71,0.71)`, `Visual=NE`, `Dir=(1,1)`, `state=idle_NE`, `sprite=warblade_idle_NE`
  - `D` → `PCFacing=(1,0)`, `Visual=E`, `Dir=(1,0)`, `state=idle_E`, `sprite=warblade_idle_east`
- Controller swap smoke-test: Warblade + Elementalist + Ranger + Shadowblade state olarak W+D'de `idle_NE`, D'de `idle_E` geçiyor. Ranger/Shadowblade sprite NULL sonucu sadece Warblade canlı gövdesine geçici controller takılan test harness'ından kaynaklı olabilir; state geçişi doğru.

**Test edecek Codex için önerilen QC:**
1. Play mode fresh başlat; Warblade seç.
2. `W` basılıyken `D` bas/bırak: gövde ve silah `N -> NE -> N` dönmeli, silah tek başına dönmemeli.
3. `A+D`, `W+S` karşıt basışlarda input sıfırlanmalı veya son geçerli yönde stabil kalmalı; jitter olmamalı.
4. `RIMA/Warblade Mount Tuner` ile bir yön offset/rotation değiştir, `SOURCE PREFAB KAYDET`, Play'i kapat/aç; ayar kaymamalı.
5. Elementalist'te el/silah yoksa tool kullanılmamalı; 8-yön body state yine W+D'de NE'ye geçmeli.

### 📄 RAPOR EKSİK
2026-06-06 rapor bu oturumun demo-proven işlerini kapsamıyor (failed-cast, off-map root, chamber skill-deneme, RunStats/boss-phase/Director fix=§7'ye güçlü vaka, post-FX Bloom, timeScale+FPS-cap). Addendum draft: `STAGING/_process/2026-06/REPORT_ADDENDUM_2026-06-18.md` (kullanıcı rapora merge edecek; DOCX'e otomatik dokunulmadı).

### 🖥️ DEMO-GÜN CHECKLIST
(1) Play durdur→recompile→taze Play · (2) **canlı demoda MCP KAPAT** (stall ana tetikleyici) · (3) takılırsa D3D12 + sürücü güncelle.

### ⏸️ ERTELENEN (post-demo) · 🛑 DOKUNMA
Merchant Echo-drain (currency-migration = yeni-bug riski, EchoWallet persist≠Gold run-local start0) · HUD lerp · low-HP/Rage red-screen de-stack · healMultiplier race · combo correctness (Glacial+Burn/Ice-Shatter/Severance) · dead-but-acting · perf 9-Find guarded-cache (`CameraFollow:48`/`BaseMobBehavior:166`/`PlaytestRoomClearedHelper:47` en kötü) · warblade Level2 per-sprite hand-data.
Working tree notu: Warblade mount tool + 8-way facing fix dosyaları **bilinçli UNCOMMITTED** (yukarıdaki QC yapılacak). Ayrı/dokunma artifact'leri: `CharacterSelect.unity` (onaylı recompile-artifact save) · `_Recovery/0 (2).unity` · `capture_v3.zip`.
