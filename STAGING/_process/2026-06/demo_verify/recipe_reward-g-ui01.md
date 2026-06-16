# LIVE-TEST RECIPE — beat `reward-g-ui01`

> Reward-al akisi (oda clear -> reward spawn -> yaklas -> **gercek G** -> 3-kart draft)
> + UI-01 (kart footer/description dikey serite cokuyor mu — SS-04 Gravity Cleave gibi).
> Unity'yi GERCEK input/akisla sur. **ForceCollect / DraftManager.ShowDraft() dogrudan cagrisi = BYPASS, YASAK.**

---

## LAUNCH (dev-direct _Arena)

1. Sahne: `Assets/Scenes/_Arena.unity` ac (full-flow MainMenu DEGIL — dev-direct).
2. Editor Play tusu (manage_editor enter_playmode). _Arena Play'de:
   - `RoomRunDirector` `Awake/Start` -> `BeginRun()` cagrilir (kod: RoomRunDirector.cs ~166-177).
   - playerPrefab spawn olur (~569), `EncounterController` ilk wave'i baslatir (~878-880).
3. _Arena dev-direct oldugu icin **F2 / backquote(`) / " AKTIF** (RIMA-002: bunlar sadece full-flow MainMenu'de inert). Bu beat icin gerekli degil — combat + G yeterli.
4. `read_console` -> 0 compile error / 0 NullRef teyit et (Play'e girmeden once + girince).

## ADIMLAR (gercek input, BYPASS YOK)

1. Player ile odadaki tum dusmanlari **gercek combat** ile oldur (LMB/skill). Bu, `EncounterController.OnRoomCleared` -> `HandleEncounterCleared` -> `RoomClearSequence`'i ATESLER (RoomRunDirector.cs ~1172-1188). Bypass yok: enemy'leri elle Destroy ETME, `RoomCleared.Invoke()` cagirma.
2. "ODA TEMIZLENDI" flash + ~0.5s bekleme sonrasi reward (`RewardPickup` GameObject adi: **"RewardPickup"**) oda merkezindeki yurunebilir hucreye spawn olur + scale-pop + cyan EchoPuffBurst (~1259-1271).
3. Player'i reward uzerine **YURUT** (WASD). Trigger'a girince/uzerine spawn olmussa "G — AL" prompt'u gorunur (OnTriggerEnter2D / OnTriggerStay2D / CheckInitialPlayerOverlap — REWARD-02 fix, RewardPickup.cs ~76-121).
4. **G tusuna bas** (Keyboard.current[Key.G].wasPressedThisFrame, RewardPickup.cs ~68-74). `playerInRange==true` SART; degilse G inert (REWARD-02'nin tam test ettigi durum).
5. Collect -> `DraftThenOpenExit()` -> `DraftManager.ShowDraft()` -> 3 kart slide-in (SkillOfferUI.BuildSkillCard ~464).
6. **UI-01 icin:** acilan 3 kartin her birinde description + tag-chip + (varsa) chain-chip + SEC butonunu incele. Ozellikle UZUN description'li bir kart (orn. Sundered-Beat / Gravity-Cleave benzeri) cikana kadar gerekirse run'i tekrarla; description'in dar bir dikey serite cokup cokmedigine bak.

## BEKLENEN (somut)

- Oda clear sonrasi `RewardPickup` adli GameObject sahnede var, sprite gorunur (cyan rift-shard ya da chest icon), `activeInHierarchy==true`.
- Player reward'a yaklasinca `playerInRange` true + prompt (HUDController.SetInteractionPrompt).
- **Gercek G** ile: `RewardPickup.WasCollected==true`, sprite/collider disabled, `DraftManager.IsDraftActive==true`, sahnede 3 adet `Card_0/Card_1/Card_2` GameObject (cardContainer altinda).
- Kart secince draft kapanir, exit kapilar acilir (RoomClearVictoryTrigger.ActivateExitDoors).
- **UI-01:** her kartin Desc TMP'si genis ve yatay sigar; metin dar tek-kolon dikey serite cokmez.

## DATA-PROOF (execute_code — overlay UI SS'a CIKMAZ, sart)

execute_code ile su state'leri oku/raporla:
1. Reward spawn:
   `GameObject.Find("RewardPickup")?.activeInHierarchy` + `GetComponent<RewardPickup>().WasCollected` (false olmali, G'den once).
2. playerInRange teyidi (G'den once): RewardPickup uzerinde `playerInRange` private field'i reflection ile oku
   (`typeof(RewardPickup).GetField("playerInRange", NonPublic|Instance)`) -> **true olmali**. Bu, REWARD-02'nin gercek kapisi: false ise G inert olur.
3. G sonrasi: `RewardPickup.WasCollected==true` (yoksa GameObject yok edilmis olabilir -> draft state'e bak).
4. Draft acildi mi: `DraftManager.Instance.IsDraftActive==true`.
5. Kart sayisi: `FindObjectsOfType` ile cardContainer cocuklari ya da `Card_0..Card_2` ara -> **3** olmali.
6. **UI-01 olcumu (kritik):** her `Card_i` altindaki `Desc` TMP icin `TMP_Text.GetComponent<RectTransform>().rect.width` + `preferredWidth`/`preferredHeight` ve satir sayisi (`textInfo.lineCount`) oku.
   - Cokme imzasi: rect.width ~ kucuk (kart -32 = ~248 beklenir; cok daha kucukse cokme) ya da `lineCount` asiri yuksek (her kelime ayri satir) -> dikey serit.
   - Ayni sekilde `TagRow` / `ChainChip` genislikleri.

## SCREENSHOT

- Dunya gorseli (reward spawn + player yaklasma): `manage_camera`/scene_view capture (game-view ScreenCapture 9.7.3 fix'i denenebilir; eskiden play'de patliyordu).
- **Draft karti (overlay UI): SS'a CIKMAZ -> data-proof zorunlu** (yukaridaki rect.width/lineCount). SS yerine execute_code cikti rakamlari delildir.

## PASS / FAIL (olculebilir)

- **REWARD-G PASS:** Gercek combat clear sonrasi RewardPickup spawn (activeInHierarchy=true) + player yaklasinca playerInRange=true + **gercek G** ile WasCollected=true + IsDraftActive=true + 3 kart. (ForceCollect cagirilmadan.)
- **REWARD-G FAIL:** G'ye basildi ama playerInRange=false ya da draft acilmadi (kart=0) -> REWARD-02 regresyonu.
- **UI-01 PASS:** her kart Desc rect.width ~ kart-genisligine yakin (~248) VE lineCount makul (<= ~4-5), metin dikey serite cokmuyor.
- **UI-01 FAIL:** Desc rect.width cok dar / lineCount patlamis (her kelime ayri satir) -> footer dikey serit cokmesi (SS-04 imzasi).

## BYPASS-TUZAKLARI (REWARD-02 dersi — yanlis-GREEN'e nasil dusulur)

1. **ForceCollect ile toplama:** `RewardPickup.ForceCollect()` ya da `Collect()` dogrudan cagirmak menzilsiz toplar -> draft acilir ama **gercek G yolu test EDILMEMIS** olur. Tam olarak REWARD-02'nin yanlis-pozitifi. **YASAK** — sadece klavye G.
2. **RewardAutoCollectTimeout:** kodda `RewardAutoCollectTimeoutSec = 0f` (auto-collect KAPALI). Eger bir testte bunu >0 yapip timeout'a birakirsan yine ForceCollect tetiklenir = bypass. Degistirme.
3. **DraftManager.ShowDraft() dogrudan cagirmak:** draft'i akistan kopuk acar -> "draft acildi" GREEN'i sahte. Draft yalniz Collect -> DraftThenOpenExit zincirinden gelmeli.
4. **Enemy'leri elle Destroy / RoomCleared.Invoke():** combat clear'i taklit eder, gercek encounter clear yolunu atlar. Gercek dusman olumu kullan.
5. **playerInRange'i reflection ile elle true yapmak:** prompt/trigger yolunu maskeler. Sadece OKU, YAZMA.
6. Eger yukaridakilerden biri kacinilmazsa (orn. tekrar uretilemiyor), recipe ciktisinda **"BYPASS KULLANILDI"** diye ACIKCA isaretle; aksi halde GREEN gecersiz.
