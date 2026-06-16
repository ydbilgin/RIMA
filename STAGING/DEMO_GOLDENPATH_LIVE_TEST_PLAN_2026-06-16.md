# DEMO GOLDEN-PATH LIVE-TEST PLAN — 2026-06-16

> Demo: 19 Haziran (3 gun). Bu plan = 7 recipe'nin skeptik sentezi (REWARD-02 dersiyle sertlestirildi).
> 7 beat, beat-sirali, gercek-input zorunlu. Her beat: LAUNCH + ADIMLAR + DATA-PROOF + PASS/FAIL + BYPASS-UYARISI.

---

## ORCHESTRATOR NOTU (en ust — once oku)

- **ORCHESTRATOR SERI KOSACAK (tek-Unity-ajan kurali):** ayni anda TEK Unity-suren ajan. Beat'leri SIRAYLA
  yurut; paralel Unity dispatch = embedded-python kopru cokmesi (2026-06-13 dersi).
- **HER BEAT SONRASI `read_console`** (0 error/0 NullRef sart). Beat'e GIRMEDEN once de oku (kirli state'le baslama).
- **Sahne TEKRAR-yuklemesi:** her beat dev-direct `_Arena` ister; bir onceki beat Play'i kapat → sahneyi
  yeniden yukle/Play'e gir (DontDestroyOnLoad singleton'lar — DirectorMode/RunStats/Pause/Codex — onceki
  beat'ten kirli kalabilir; ozellikle panels-quality ve telemetry stale-instance riski).
- **Sira gerekcesi:** B0→B1 (temel render+oda) → B2 (combat/stat) → B3 (reward/draft) → B4 (telemetry,
  B2+B3'un olusturdugu kayitlari okur, sira ZORUNLU) → B5 (room transition, B3 reward'a dayanir) →
  B6 (F2 build) → B7 (director backquote) → B8 (panels). Reward/combat ureten beat'ler telemetri'den ONCE.
- **Genel bypass-zemin:** `*ForValidation` API'leri yalniz READ-BACK icin (PlacedCount okumak gibi). Hicbir
  beat'te toggle/place/collect/state-degisimi ForValidation/Force/direkt-metodla YAPILMAZ — gercek tus/click sart.
- **DOGRULANMIS KOD GERCEGI (REWARD-02 cross-check, bu sentezde duzeltildi):**
  - `RoomRunDirector.cs:1194` → `RewardAutoCollectTimeoutSec = 0f` → **auto-collect KAPALI.**
    `:1282` guard `> 0f` → **ForceCollect ASLA otomatik tetiklenmez.** ⇒ telemetry/room recipe'lerindeki
    "90sn bekle, ForceCollect auto-tetiklenir, hizli ol" UYARISI **GECERSIZ/yanlis** (o 90f = `DraftThenOpenExit`
    guard'i, RewardPickup.cs:221 — reward degil draft). Tester'a: ForceCollect SADECE elle cagrilirsa olur;
    log'da `[Reward] ForceCollect` gorursen biri elle cagirmistir = test gecersiz.
  - Tus haritasi (BuildModeController.cs:180/184 + DirectorMode.cs:196): **F2 VE quote(") = Build Mode toggle**
    (ayni Toggle); **backquote(`) = Director toggle** (DirectorMode, `!BuildModeController.IsActive` guard'li).
    ⇒ B6 = F2/quote (build), B7 = backquote (director). Karistirma.
  - Aktiflik kosulu: `_Arena` GameEntryScenes{MainMenu,CharacterSelect} DISINDA → DirectorMode+BuildMode
    self-bootstrap → F2/quote/backquote **AKTIF**. Full-flow MainMenu'de bootstrap GUARD'lanir → hepsi **INERT**
    (tasarim). **TUM bu beat'ler SADECE dev-direct `_Arena`'da; full-flow'da "inert, bozuk" demek = yanlis-FAIL.**

---

## BEAT 0 — panels-quality / HUD + Pause + Settings + Codex (render+okunabilirlik)

**Neden once:** combat'tan bagimsiz; HUD/panel render kirilmissa sonraki beat'lerin gozlemi de supheli.
Stale-instance riski en yuksek beat → temiz sahneyle bas.

**LAUNCH:** `_Arena.unity` yukle → Play. ~1-2 sn bekle (HUDController LateBind retry'li). `read_console` → 0 error.

**ADIMLAR (gercek input):**
1. (A/HUD) Aksiyon yok; player'a bind olmasini bekle.
2. (B/Pause) **GERCEK ESC** bas (Keyboard escape → UIManager.OnEsc → OpenPause). UIManager.OpenPause() ELLE cagirma.
3. (C/Settings) Pause acikken `Btn_SETTINGS`'e **gercek pointer click** (EventSystem). Pointer simule edilemezse
   UIManager.OpenSettings() = YARI-BYPASS → ciktida "input-bypass: render OK, click-path DOGRULANMADI" yaz, PASS sayma.
4. (D/Codex) ESC→Pause→`Btn_SKILL CODEX` click. Bir sinif butonuna tikla → liste degismeli.
5. (E) ESC ile geri/kapat zinciri.

**DATA-PROOF (execute_code — overlay SS'a CIKMAZ):**
- Her panel: `GameObject.Find("[PauseMenuUI]"/"[SettingsMenuUI]"/"[SkillCodexUI]")` → **CanvasGroup.alpha==1 &&
  blocksRaycasts==true** (activeInHierarchy TEK BASINA YETMEZ — panel kapaliyken de true).
- Pause: Panel childCount, btnCount==5 (`Btn_*`), Title.text=='PAUSED', rect ~356x366.
- Settings: headers>=4, sliders==3, rect ~400x640, Pause alpha==0.
- Codex: classBtns==10, secili sinif `Skill_*` rows>0, sinif degisince rows degisir.
- HUD: HPLabel.text dolu (bos/'0' degil), ResFill!=null, EchoBalance dolu, font asset null DEGIL (garbled=Jersey10 bozuk).
- Stale-instance: her panel GO sayisini logla (>1 ise DontDestroyOnLoad kirliligi → temiz sahne gerek).

**PASS:** P1 Pause(alpha1+blocks+btn5+PAUSED) · P2 Settings(alpha1+slider3+header>=4) · P3 Codex(alpha1+class10+rows>0)
· P4 HUD(HPLabel dolu+ResFill+Echo) · P5 tasma yok+text dolu+font OK · P6 ust-panel acikken Pause alpha==0.
**FAIL:** biri saglanmaz / console panel-build hatasi.

**BYPASS-UYARISI:**
- 🪤 activeInHierarchy=="acik" sayma → alpha==1 && blocksRaycasts esas.
- 🪤 OpenPause/OpenSettings/OpenSkillCodex elle cagirmak = ForceCollect-tuzaginin esi (tus/click yolu kirik olabilir).
- 🪤 "Panel var" != "guzel/dolu": text bos / font bozuk / tasma olabilir → ornek TMP.text + font-null + rect-tasma proxy.
- 🪤 Codex rows==0 (SkillDatabase build edilmemis) panel-acik GREEN'i ile karistirma.
- ⚠️ **"Guzel mi" gozle teyit:** overlay SS cikmaz → proxy metrik PASS ≠ estetik PASS. Estetik = KULLANICI editor'de
  bakar (juri provasi). Bunu "kullanici-gorsel-teyit BEKLEMEDE" diye AYRI isaretle, otomatik PASS sayma.

---

## BEAT 1 — room-transition oda-1 kurulum (golden-path zemin) + draft kapatma

**Neden:** sonraki tum beat'ler oda+draft akisinin saglikli kurulmasina dayanir. (room-transition'in F1-leak
kismi B5'te; burada sadece "oda kuruldu, acilis draft'i gercekten kapandi" zemini.)

**LAUNCH:** `_Arena.unity` (yeni Play) → `buildOnStart:1` ilk odayi kurar, acilis kit-draft'i acilir. `read_console` 0 error.

**ADIMLAR (gercek input):**
1. Acilis draft'ini **ekrandan gercek bir kart secerek** kapat (1/2/3 ya da tiklama). Draft acik kalirsa kapi akisi degisir.

**DATA-PROOF:** `DraftManager.Instance.IsDraftActive==false` (draft kapandi) · `RoomRunDirector` canli ·
oyuncu PlayerController aktif · `Time.timeScale==1`.

**PASS:** acilis draft secimle kapandi, oda kuruldu, oyuncu hareket edebilir, 0 console error.
**FAIL:** draft kapanmiyor / oda kurulmuyor / spawn yok / console error.

**BYPASS-UYARISI:**
- 🪤 `DraftManager.ShowDraft()`/secimi execute_code'dan zorlamak = akistan kopuk. Sadece gercek kart secimi.

---

## BEAT 2 — stat-damage (LMB lineer, Q/E/R/F stat-deaf)

**Tez:** physPower artinca SADECE LMB temel hasari LINEER artar; Q/E/R/F=`bypassStatScaling` → slider'a SAGIR.
**Kod-anchor:** DamageCalculator.cs:30-38 (physPower/100f, bypass→1f) · BasicAttackBehaviorBase.cs:70-79
(LMB SCALED) · SkillRuntime.cs:120,141 (int-overload bypass=true) · DirectorMode.cs:537 SetStatForValidation,
:595 TelemetrySourceDamageForValidation.

**LAUNCH:** `_Arena` Play. Warblade spawn (Player tag), PlayerController+PlayerAttack enabled, console temiz.
**maxHP slider'INA DOKUNMA** (HP gorsel yalan + min-1 floor → oran bozar).

**ADIMLAR (gercek input):**
1. **backquote(`)** ile Director overlay ac → Stats sekmesi. (NOT: Director = backquote; F2/quote DEGIL.)
2. Spawn sekmesi → enemy sec → arenada **net bos noktaya** tikla (overlay disi; raycast riski).
3. Stats physPower=50. Overlay'i KAPAT / Test state (timeScale=1 — pause'da hasar islemez).
4. Dusmana yaklas, **SADECE LMB** ile TAM 1 combo-adim vur. Dusen HP = **D1** (Health.CurrentHP onces-sonra delta).
5. Yeni dusman + ayni baslangic HP. physPower=250, Test state.
6. Ayni LMB combo-adimi → **D2**. Bekle: **D2 ≈ 5 × D1** (250/50).
7. (Negatif kol) physPower 250'de KAL, dusmana **Q (veya E/R/F)** ile vur → hasar DEGISMEZ (phys50 ile ayni).

**DATA-PROOF (execute_code, iki katman):**
- Math: `PlayerClassManager.Instance.CurrentPrimaryStats.physPower` = 50/250 dogrula ·
  `DirectorMode.Instance.SetStatForValidation("physPower",250f)` **true donmeli** (false=slider baglanmadi, sonraki olcum anlamsiz).
- Real-hit: dusman `Health.CurrentHP` (Health.cs:15) vurus oncesi/sonrasi delta · LMB:
  `TelemetrySourceDamageForValidation(<LMB DamageSourceType>)` phys250'de phys50'nin ~5× · Q/E/R/F source-total degismez.

**PASS:** D2/D1 ∈ [4.5,5.5] (rounding+min-1 floor toleransi) VE Q/E/R/F deltasi phys50↔250 degismez (±floor).
**FAIL:** LMB oran ~1× (slider islemedi) VEYA Q/E/R/F oran ~5× (yanlis overload, bypass kirildi).

**BYPASS-UYARISI:**
- 🪤 Tezi Q/E/R/F ile test etmek (52/54 yetenek bypass=true → slider sagir). Tez SADECE LMB ile.
- 🪤 `DealDamage(int)`/`DealDamageRaw` direkt cagirmak (int-overload bypass=true) → stat'i atlar, gercek LMB DEGIL.
- 🪤 `debugGlobalDamageMult` ile kanitlamak → bypass'tan BAGIMSIZ her pakete isler; tezi maskeler. Slider'i 1'de tut.
- 🪤 timeScale=0/pause'da olcmek → vurus islemez, 0 delta = yanlis-FAIL. Test state'te olc.
- 🪤 maxHP slider oynatmak → min-1 floor oran bozar. Tam can ile basla.
- 🪤 SetStatForValidation donus degerini gormezden gelmek → false=baglanmadi; once true teyit.

---

## BEAT 3 — reward-g-ui01 (oda clear → reward → GERCEK G → 3-kart draft + UI-01 footer cokmesi)

**⭐ REWARD-02 EPICENTER:** "GREEN" burada bypass'la maskelendi (ForceCollect menzilsiz topladi). Bu beat
**SADECE gercek combat + gercek G** ile.

**LAUNCH:** `_Arena` Play → BeginRun → ilk wave. (Onceki beat'in draft'i kapali olmali — kirli state F2/reward
guard'larini erken-return'e dusurur.) `read_console` 0 error (Play oncesi+sonrasi).

**ADIMLAR (gercek input, BYPASS YOK):**
1. Odadaki tum dusmanlari **gercek combat** ile oldur (LMB/skill) → EncounterController.OnRoomCleared →
   RoomClearSequence. Enemy'leri elle Destroy ETME, `RoomCleared.Invoke()` cagirma.
2. "ODA TEMIZLENDI" flash + ~0.5s → `RewardPickup` (GO adi "RewardPickup") oda merkezine spawn (scale-pop+cyan puff).
3. Player'i reward uzerine **YURUT** (WASD). "G — AL" prompt'u gorunmeli
   (OnTriggerEnter2D/Stay2D/CheckInitialPlayerOverlap — REWARD-02 fix, RewardPickup.cs:76-121).
4. **GERCEK G** bas (RewardPickup.cs:70-71: `playerInRange && Keyboard.current[Key.G].wasPressedThisFrame`).
   `playerInRange==true` SART; degilse G inert (= REWARD-02'nin tam test ettigi kapi).
5. Collect → DraftThenOpenExit → DraftManager.ShowDraft → 3 kart slide-in (SkillOfferUI.BuildSkillCard).
6. (UI-01) 3 kartin description+tag-chip+(varsa)chain-chip+SEC incele. UZUN description'li kart cikana kadar
   gerekirse run tekrarla (Sundered-Beat/Gravity-Cleave benzeri); description dar dikey serite cokuyor mu?

**DATA-PROOF (execute_code — overlay SS'a CIKMAZ, sart):**
- Spawn: `GameObject.Find("RewardPickup")?.activeInHierarchy` + `WasCollected==false` (G'den once; public getter, RewardPickup.cs:19).
- **playerInRange (G'den once, REWARD-02 gercek kapisi):** reflection `typeof(RewardPickup).GetField("playerInRange",
  NonPublic|Instance)` → **true olmali** (false ise G inert). SADECE OKU, yazma.
- G sonrasi: `WasCollected==true` · `DraftManager.Instance.IsDraftActive==true` · cardContainer alti / `Card_0..2` = **3**.
- **UI-01 olcumu (kritik):** her `Card_i` altindaki `Desc` TMP →
  `RectTransform.rect.width` + `preferredWidth`/`preferredHeight` + `textInfo.lineCount`.
  Cokme imzasi: rect.width ~ cok kucuk (beklenen ~248=kart-32; cok daha kucuk=cokme) VEYA lineCount asiri yuksek
  (her kelime ayri satir). TagRow/ChainChip genisligi de oku.

**PASS (REWARD-G):** gercek combat clear → RewardPickup spawn(active) → yaklasinca playerInRange=true → **gercek G**
→ WasCollected=true + IsDraftActive=true + 3 kart (ForceCollect cagirilmadan).
**FAIL (REWARD-G):** G basildi ama playerInRange=false / draft acilmadi (kart=0) → REWARD-02 regresyonu.
**PASS (UI-01):** her Desc rect.width ~248 VE lineCount makul (<=~4-5), dikey serite cokmuyor.
**FAIL (UI-01):** Desc rect.width cok dar / lineCount patlamis (her kelime ayri satir) → SS-04 footer cokmesi.

**BYPASS-UYARISI:**
- 🪤 **ForceCollect()/Collect() direkt** → menzilsiz toplar, gercek G yolu test EDILMEZ = REWARD-02 yanlis-pozitifi. YASAK.
- ✅ **Auto-collect KAPALI (kod-teyit):** `RewardAutoCollectTimeoutSec=0f` → timeout ForceCollect ASLA tetiklenmez.
  Beklemek ZARARSIZ (eski recipe'lerin "90sn ForceCollect auto-tetiklenir, hizli ol" uyarisi GECERSIZ — o 90f=draft guard).
  Yine de log'da `[Reward] ForceCollect` gorursen biri elle cagirmis = test GECERSIZ.
- 🪤 `DraftManager.ShowDraft()` direkt → akistan kopuk sahte-GREEN. Draft yalniz Collect→DraftThenOpenExit'ten gelmeli.
- 🪤 Enemy elle Destroy / RoomCleared.Invoke → gercek encounter-clear yolunu atlar.
- 🪤 playerInRange'i reflection ile elle true yapmak → trigger yolunu maskeler. SADECE OKU.

---

## BEAT 4 — telemetry (damage telemetry + reward sayaci)

**Neden bu sirada:** B2 (combat/LMB) + B3 (reward G) GERCEK kayit uretir; telemetry onlari OKUR. Sira ZORUNLU.
**Mimari (2 ayri sistem):** RunStats (RunStats.cs, DDOL) = kills/rewardsCollected sayar, DAMAGE TUTMAZ.
DirectorMode telemetry = `SkillRuntime.OnDamageApplied` (OnEnable:244 abone) → records+bySource+DPS(5sn)+TTK.

**LAUNCH:** `_Arena` Play. `read_console` 0 error. (Director telemetri'nin saymasi icin DirectorMode.Instance
canli/enabled olmali → overlay'i bir kez backquote ile ac ki Instance kurulsun, SONRA vur.)

**ADIMLAR (gercek input):**
1. backquote → Director → Spawn → enemy sec → arena'ya tikla. backquote ile Test'e don (oyun aksin).
2. Dusmana **LMB** ile birkac kez vur (>=3 vurus; hepsi DealDamage'tan gecer). Q/E/R/F de eklenebilir.
3. Dusmani **OLDUR** (TTK = ilk-hit→death).
4. backquote → Director → **Telemetry** tab → veriyi gor.
5. (Reward sayaci) Test state'e don, reward'i **GERCEK G** ile topla (B3'teki gibi). (NOT: ForceCollect=bypass; ayrica
   timeout=0f oldugu icin zaten auto-tetiklenmez.)

**DATA-PROOF (execute_code, DirectorMode *ForValidation hook, satir 585-604):**
```csharp
var dm = RIMA.DirectorMode.Instance;
int events = dm.TelemetryEventCountForValidation();
float dps  = dm.TelemetryDpsForValidation();
int skillDmg = dm.TelemetrySourceDamageForValidation(RIMA.Combat.DamageSourceType.Skill);
string csv = dm.ExportTelemetryCsvForValidation();
int rewards = RIMA.RunStats.RewardsCollected;
int kills   = RIMA.RunStats.Kills;
```
- Panel canli: `GameObject.Find("Panel_Telemetry")?.activeInHierarchy` veya ActiveTab==Telemetry.
- Reward GO Collect→Destroy: reward null/destroyed.

**PASS:** events==gercek vurus sayisi(>=3) · dps>0 (vurustan HEMEN sonra) · skillDmg>0 + CSV'de o kadar satir ·
TTK death sonrasi "x.xxs" ("--" degil) · rewards G basinca tam +1 (0→1).
**FAIL:** events=0/dps=0 vurustan sonra · rewards artmaz · CSV bos · panel acilmaz.

**BYPASS-UYARISI:**
- 🪤 ForceCollect/RecordRewardCollected() elle cagirmak → sayaci pompalar, wiring test etmez. Manuel G.
- ✅ Eski "90sn coroutine guard, beklersen ForceCollect" uyarisi GECERSIZ (timeout=0f, kod-teyit). Beklemek zararsiz.
- 🪤 `health.TakeDamage()` direkt → telemetry SADECE SkillRuntime.DealDamage/DealDamageRaw→OnDamageApplied'tan beslenir.
  Director debug-damage / non-SkillRuntime hasar event uretMEZ → gercek LMB/skill.
- 🪤 DirectorMode disabled iken vurmak → OnDisable abonelik koptu, kayitsiz. Once overlay ac (Instance kursun).
- ⚠️ DPS 5sn pencere + Director pause saati DONDURUR → vurustan cok sonra / Director state'te okursan DPS duser/donar;
  "0 gordum FAIL" deme. Vurustan ~hemen sonra, Test state'te oku.
- 🪤 FinalDamage<=0 kayitsiz (immune/dead target). Canli dusmana gercek hasar.

---

## BEAT 5 — room-transition-f1 (oda gecisi + stale-reward-leak)

**Odak:** Oda 2'ye gecince onceki odadan kalan RewardPickup leak ediyor mu.
**Kod gercegi:** `_Arena` LIVE manager=RoomRunDirector (RuntimeRoomManager CANLI DEGIL). Gecis: kapi trigger
(`RoomRunExitDoorTrigger`, interact=**G**) → TryEnterDoor → AdvanceTo → `BuildCurrentRoom()`. Temizlik (~311) =
**sadece `DestroyActiveReward()`** (TEK takipli ref); sahne-genel `FindObjectsByType<RewardPickup>` sweep YOK
(o yol RuntimeRoomManager.ClearActiveRewards'ta = `_Arena`'da CALISMAZ). ⇒ F1 riski = collect EDILMEMIS reward leak'i.

**LAUNCH:** `_Arena` Play → buildOnStart ilk oda + acilis draft. Acilis draft'ini **gercek kart secimiyle** kapat.

**ADIMLAR (gercek input):**
A. Oda 1: dusmanlari WASD+LMB ile gercekten oldur (clear blip).
B. Merkeze RewardPickup (chest) spawn.
C. **KRITIK F1: reward'i KASTEN BIRAK** (G'ye basma). Exit kapisina yuru.
   - Kapi reward toplanmadan acilmiyorsa → "gecis reward'a kilitli" notu al; sonra reward'i gercek G ile topla,
     draft SEC, kapi acilinca gec (alternatif yol).
D. Acik kapi menziline gir → "enter rift" prompt → **G** ile gec.
E. Oda 2 kurulduktan SONRA durakla → data-proof.
F. (Tekrar) Oda 2'yi temizle, reward'i bu sefer GERCEK G ile topla, draft sec, Oda 3'e gec → ardisik temizlik dogrula.

**DATA-PROOF (Oda-2 build LOG'u `[RoomRunDirector] Built node...` GORULDUKTEN sonra oku — erken okuma kararsiz):**
1. `Object.FindObjectsByType<RIMA.RewardPickup>(FindObjectsSortMode.None).Length` → her biri name/activeInHierarchy/
   WasCollected/position.
2. `RoomRunDirector.activeReward` reflection (`GetField("activeReward",NonPublic|Instance)`) → null mi.
3. MapFragment sayisi (`FindObjectsByType<...MapFragment>`, namespace grep'le dogrula) → 0 (director demo'da fragment yok).
4. RoomRunDirector lifecycle.State reflection → Oda-2 build sonrasi beklenen state.

**PASS:** Oda-2 build sonrasi `FindObjectsByType<RewardPickup>().Length==0` (henuz temizlenmedi) VE activeReward==null
VE onceki chest GO destroyed VE MapFragment==0.
**FAIL (F1 leak):** Oda-2'de onceki odanin RewardPickup'i HALA var (Length>=1, pozisyon Oda-1 koord / WasCollected==false)
→ stale-reward-leak DOGRULANDI (BuildCurrentRoom sahne-genel sweep yapmiyor).

**BYPASS-UYARISI:**
- 🪤 ForceCollect maskesi: reward'i toplamadan birakirsan ELLE ForceCollect cagirma → chest'i siler, leak GIZLENIR.
  ✅ Auto-tetiklenmez (timeout=0f); log'da `[Reward] ForceCollect` cikarsa testi GECERSIZ say, tekrarla.
- 🪤 AdvanceTo/BuildCurrentRoom execute_code'dan direkt → kapi/menzil/lifecycle atlanir. Gecis GERCEK G-kapisi ile.
- 🪤 ClearActiveRewards'a guvenmek → `_Arena`'da CALISMAZ (RuntimeRoomManager canli degil). Sadece DestroyActiveReward.
- 🪤 Tek-frame erken okuma (draft/coroutine kosarken) → eski reward Destroy edilmeden gorunur. Build LOG'undan sonra oku.
- 🪤 Sadece scene_view'e bakmak → reward GO inactive yasayabilir. FindObjectsByType+activeInHierarchy say.

---

## BEAT 6 — f2-buildmode (F2/quote ile Build Mode + GERCEK click prop yerlestirme)

**Amac:** F2 GERCEK tus + GERCEK fare click ile prop yerlesir, kalici, oyun devam eder, panel render olur.
**Tus:** F2 VEYA quote(") = Build Mode toggle (BuildModeController.cs:180/184). backquote DEGIL (o B7).

**LAUNCH:** `_Arena` Play → DirectorMode.Bootstrap (cs:150-167) + BuildModeController.Bootstrap (AfterSceneLoad) →
F2 AKTIF. Oda yuklensin, oyuncu spawn, RoomRunDirector.CurrentTemplate dolu (bos ise CreateWorkingTemplate:344 warning).
**Full-flow'da DEGIL:** MainMenu'de bootstrap GameEntryScenes guard'iyla atlanir → EnterBuildMode:224-228
`DirectorMode.Instance==null` erken-return → F2 INERT (tasarim, "bozuk" deme).

**ADIMLAR (gercek input):**
1. 1-2 sn normal gameplay (timeScale=1, oyuncu hareket ediyor).
2. **F2 (gercek tus)** → Build Mode acilmali (Update:180-181 → Toggle → EnterBuildMode).
3. Sol BUILD panelinde bir prop kartina TIKLA (Props sekmesi default).
4. Dunyada **UI panel USTUNDE OLMAYAN** bir hucreye fare → ghost YESIL iken **sol-tikla** (yerlestir).
5. **F2 tekrar** → Build Mode kapanmali, oyun (timeScale=1) DEVAM.
6. Prop SAHNEDE HALA durdugunu + oyuncu yeniden hareket ettigini dogrula.
7. (Ops/TILE) F2 → ust segmented TILE → hucreye sol-tikla (floor paint).

**DATA-PROOF (F2'ye GERCEK bastiktan SONRA oku; EnterBuildMode/ForValidation TOGGLE icin ASLA cagirma):**
- `BuildModeController.IsActive` (giris/cikis) · `Instance.WorkingTemplate != null` · `Time.timeScale` (giris=0, cikis=1)
  · `DirectorMode.Instance.State`==Director (giriste).
- Panel render (GERCEK GO): `GameObject.Find("BuildPaletteCanvas")` activeInHierarchy=true + alti Status/Tabs/GridRow +
  Canvas.enabled + bir TMP_Text.text bos degil. (TILE: `GameObject.Find("BuildTileBrushCanvas")` active.)
- Prop yerlesim (GERCEK click SONRASI, READ-BACK): `BuildPlacementController.Instance.PlacedCountForValidation()` 0→1 +
  sahnede `prop_*` GO aktif.
- Persist+devam (F2 cikis sonrasi): prop GO hala aktif + timeScale==1 + PlayerController aktif.

**PASS:** F2(gercek)→IsActive true; tekrar F2→false · giris timeScale=0+State=Director, cikis timeScale=1 ·
BuildPaletteCanvas active+dolu TMP · GERCEK click ile PlacedCount+1 ve prop_* olustu · cikis sonrasi prop hala aktif+
timeScale=1+oyuncu hareket.
**FAIL:** F2 hicbir sey yapmiyor / IsActive degismiyor / panel yok-pasif / PlacedCount artmiyor / cikista prop kayboluyor /
timeScale 0'da kaliyor.

**BYPASS-UYARISI:**
- 🪤 `PlaceForValidation/SelectToolForValidation/EnterBuildMode()` direkt → F2-toggle + fare click + **IsPointerOverUi
  hover guard** ATLANIR. ForValidation SADECE read-back (PlacedCount). Toggle=gercek F2, yerlesim=gercek sol-tik.
- 🪤 Sessiz erken-return'u "calisti" sanmak: EnterBuildMode su durumda no-op → UIManager.IsAnyOverlayOpen / Draft
  aktif-pending (218-221) / DirectorMode.Instance null (224) / Camera.main null (231). F2 bastin degisim yoksa
  "inert dogru" DEME — once reward/draft/overlay KAPALI + oda yuklu emin ol, tekrar dene.
- 🪤 Full-flow'da test edip "F2 inert=bozuk" demek → tasarim. SADECE dev-direct `_Arena`.
- 🪤 UI uzerine tiklamayi "place" sanmak → IsPointerOverUi yutar, PlacedCount artmaz; panel-DISI hucreye tikla.
- 🪤 Ghost KIRMIZI iken tiklamak → gecersiz hucre, yerlesmez (FAIL degil); yesil ghost'lu hucre.
- 🪤 Cikista prop'u command-stack ile karistirmak → undo-history dusulur ama placed instance SAHNEDE KALIR.
  "history temizlendi" ≠ "prop silindi"; prop GO aktif kaldigini ayrica dogrula.

---

## BEAT 7 — director-backquote (backquote toggle + Test-state overlay-bleed fix + Build Mode inert guard)

**Odak:** backquote(`) DirectorMode toggle (NEREDE aktif/inert) + Test state'e gecince overlay sizmiyor mu.
**Kod (DirectorMode.cs):** Update:196 `backquoteKey.wasPressedThisFrame && !BuildModeController.IsActive → ToggleState`
(backquote AKTIF sadece Build Mode KAPALI iken; aciksa INERT desync-guard) · Awake:186 SetOverlayVisible(false) ·
SetState:317 Director→overlay true / :324 Test→overlay false · SetOverlayVisible:2080 `overlayCanvasGo.SetActive(visible)`
(canvas `Canvas_DirectorOverlay`, ScreenSpaceOverlay sortingOrder 950). Bootstrap default State=Test.

**LAUNCH:** `_Arena` Play → DirectorMode AfterSceneLoad self-bootstrap. Baslangic: State=Test, overlay GIZLI.

**ADIMLAR (gercek input — tus enjekte: `InputSystem.QueueStateEvent`/Keyboard backquote; UI-click/ToggleState() = BYPASS):**
- A0 (baseline): Play sonrasi hicbir tusa basmadan baslangic state oku.
- A1: backquote bas → Director'a gec, overlay GORUNMELI.
- A2: backquote tekrar → Test'e don, overlay GIZLENMELI (bleed fix).
- A3 (inert): once **F2 ile Build Mode AC** (IsActive=true), sonra backquote bas → INERT (state DEGISMEMELI).
  Ardindan F2 ile Build Mode kapat (temizlik).

**DATA-PROOF (her adim sonrasi):**
- `DirectorMode.Instance.State` (Director/Test).
- Overlay: `GameObject.Find("DirectorMode")` alti `Canvas_DirectorOverlay` → **`go.activeInHierarchy`** (alpha DEGIL).
- `Time.timeScale` (Director=0, Test=1).
- A3: `BuildModeController.IsActive` backquote oncesi true + backquote sonrasi State degismedi (birlikte logla).

**PASS:** A0 overlay=false · A1 overlay=true & State=Director & ts=0 · A2 overlay=FALSE & State=Test & ts=1 (bleed fix)
· A3 State degismedi (backquote inert).
**FAIL:** A2'de overlay hala true (bleed fix calismadi) · A1 overlay false/State degismedi (handler kirik / IsActive yanlis true)
· A3 State degisti (inert guard kirik).

**BYPASS-UYARISI:**
- 🪤 `ToggleState()`/`SetState(Director)` direkt → backquote input + `!BuildModeController.IsActive` guard'i ATLAR
  (ForceCollect-esi). Gercek tus event enjekte et.
- 🪤 `*ForValidation` (HasPanelForValidation/EnsureOverlayReadyForValidation) → overlay'i ZORLA kurar, gercek toggle/bleed
  yolunu test ETMEZ. YASAK.
- 🪤 Overlay'i `rootGroup.alpha` ile olcmek → bleed fix `SetActive()` ile; alpha 1 kalip canvas inactive olabilir.
  Mutlaka `Canvas_DirectorOverlay.activeInHierarchy`.
- 🪤 A3'u Build Mode KAPALI iken kosmak → guard hic devreye girmez, sahte-PASS. A3 oncesi IsActive==true data-proof ile dogrula.
- 🪤 Tek toggle test edip "calisiyor" demek → bleed fix SADECE Director→Test'te gorulur. A1+A2 ikisini de kos; A2 overlay=false sart.

---

## SON KONTROL
- Tum beat'ler bittiginde: `read_console` son tarama (0 birikmis error/NullRef).
- "Kullanici-gorsel-teyit BEKLEMEDE" isaretli ogeler (B0 estetik) listesini kullaniciya raporla — bunlar otomatik PASS DEGIL.
- Herhangi bir beat'te bypass kacinilmaz olduysa ciktida **"BYPASS KULLANILDI"** ACIKCA isaretle; aksi halde o beat GREEN'i gecersiz.
