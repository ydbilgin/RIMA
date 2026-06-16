# LIVE-TEST RECIPE — TELEMETRY beat (RIMA demo)

> Beat: Telemetry/RunStats wiring gercek mi? reward collected sayaci + damage telemetry (DPS/TTK/event count + source breakdown) gercek input ile dolu mu, panel verisi sahte/mock degil mi.
> Unity'yi ASLA bypass ile dogrulama: telemetry SADECE `SkillRuntime.OnDamageApplied` ile beslenir; reward sayaci SADECE `RunStats.RecordRewardCollected()` ile artar.

## MIMARI (okuma ile dogrulandi — IKI ayri sistem)
- **RunStats** (`Assets/Scripts/Core/RunStats.cs`): DDOL singleton. Sayar: kills, roomsCleared, **rewardsCollected**, roomReached, runTimeSeconds. **DAMAGE TUTMAZ** (damage field'i yok). `RecordRewardCollected()` yalniz `RewardPickup.Collect()` icinden (satir 142) cagrilir.
- **DirectorMode telemetry** (`Assets/Scripts/UI/DirectorMode.cs`): Telemetry tab. `SkillRuntime.OnDamageApplied += OnDamageAppliedTelemetry` (OnEnable satir 244). Her gercek hasar -> telemetryRecords + telemetryDamageBySource[source] + DPS(5sn pencere) + TTK (ilk-hit -> olum). Event invoke: `SkillRuntime.DealDamage` (satir 194) ve `DealDamageRaw` (satir 157). Yani LMB/Q/E/R/F gercek vurus -> kayit.

## LAUNCH (dev-direct)
1. Unity Editor'de sahne ac: `Assets/Scenes/_Arena.unity` (canonical temiz arena; full-flow MainMenu DEGIL).
2. Play bas. _Arena dev-direct oldugu icin Director/BuildMode AKTIF (RIMA-002: bunlar SADECE _Arena'da calisir, MainMenu'de inert).
3. read_console ile 0 error dogrula (compile + runtime). Hata varsa once coz.

## ADIMLAR (gercek input — bypass YOK)
### A) Damage telemetry
1. Bir dusman spawn et: backquote (`) ile Director overlay'i ac -> "Spawn" tab -> bir enemy sec -> arena'ya tikla (gercek spawn). Director'i kapatmak icin tekrar backquote (Test state'e don, oyun akar).
2. Oyuncu ile dusmana yaklas, **LMB** ile birkac kez vur (gercek attack input; LMB stat-scaled gercek hasar yolu). Istersen Q/E/R/F de kullan (hepsi DealDamage'tan gecer).
3. Dusmani **OLDUR** (TTK olcumu icin ilk-hit -> death gerekli).
4. backquote -> Director overlay -> "Telemetry" tab'ina tikla. (Tab rail'de "Telemetry" butonu, satir 812.)

### B) Reward collected sayaci
5. Test state'e don (oyun aksin). Reward'i GERCEK topla: reward sprite'ina yuru, prompt cikinca **G** tusu (RewardPickup InteractKey = G, satir 15). NOT: ForceCollect (timeout auto-grant) de RecordRewardCollected'i tetikler AMA bu bir BYPASS yolu — manuel G ile topla (asagi BYPASS-TUZAKLARI).

## BEKLENEN (somut)
- A: Telemetry panelinde DPS > 0, Event Count = atilan vurus sayisi, TTK = "x.xxs" (--degil), source breakdown'da Skill satiri damage + % dolu.
- B: rewardsCollected 0 -> 1 olur (her gercek G ile +1).

## DATA-PROOF (execute_code — overlay UI screenshot'a CIKMAZ, sart)
DirectorMode'un *ForValidation hook'lari ile oku (satir 585-604):
```csharp
var dm = RIMA.DirectorMode.Instance;
int events = dm.TelemetryEventCountForValidation();          // telemetryRecords.Count
float dps   = dm.TelemetryDpsForValidation();                 // CalculateTelemetryDps(now)
int skillDmg = dm.TelemetrySourceDamageForValidation(RIMA.Combat.DamageSourceType.Skill);
string csv  = dm.ExportTelemetryCsvForValidation();           // ham kayit satirlari
int rewards = RIMA.RunStats.RewardsCollected;                 // reward sayaci
int kills   = RIMA.RunStats.Kills;
// TTK paneli text: dm uzerinden private; CSV'de per-event veri var, TTK icin death gozlemi yeterli.
Debug.Log($"events={events} dps={dps} skillDmg={skillDmg} rewards={rewards} kills={kills}\nCSV:\n{csv}");
```
- Telemetry panel GameObject canli mi: `GameObject.Find("Panel_Telemetry")?.activeInHierarchy` veya DirectorMode.Instance != null + ActiveTab == DirectorTab.Telemetry.
- Reward objesi gercekten yok oldu mu (Collect -> Destroy): reward GameObject null/destroyed.

## SCREENSHOT
- Dunya: scene_view capture (dusman + reward konumu). Game-view ScreenCapture'i 9.7.3 fix ile deneyebilirsin ama overlay yine CIKMAZ.
- Telemetry paneli overlay UI -> screenshot'a CIKMAZ -> **data-proof yaz** (yukaridaki execute_code log'u). Panel gorseli icin ekran goruntusu kanit DEGIL.

## PASS / FAIL (olculebilir)
PASS hepsi gerekli:
- `events` == atilan gercek vurus sayisi (>=3).
- `dps` > 0 (vurustan hemen sonra okunursa).
- `skillDmg` > 0 ve CSV'de o kadar satir.
- TTK: dusman olduktan sonra panel "--" DEGIL, x.xxs gosterir (CSV/death dogrulamasi).
- `rewards` G basildiktan sonra tam +1 artar (0->1).
FAIL: events=0 veya dps=0 vurustan sonra; rewards artmaz; CSV bos; panel acilmaz.

## BYPASS-TUZAKLARI (REWARD-02 dersi — yanlis-GREEN onleme)
1. **ForceCollect ile reward = SAHTE GREEN.** `ForceCollect()` (RewardPickup satir 128) menzilsiz Collect cagirir -> rewardsCollected artar AMA gercek G/menzil yolu kirik olsa bile yesil verir (REWARD-02 tam bu tuzaktan dustu). **Manuel G ile topla**, ForceCollect/timeout'a guvenme. Coroutine guard 90sn — beklersen otomatik tetiklenebilir, hizli topla.
2. **execute_code ile RecordRewardCollected()'i elle cagirma** — sayaci pompalar, wiring'i test etmez. Yalniz oku, yazma.
3. **Telemetry: health.TakeDamage() dogrudan cagrilirsa kayit TUTULMAZ.** Telemetry SADECE SkillRuntime.DealDamage/DealDamageRaw -> OnDamageApplied yolundan beslenir. Director "kill"/debug damage veya non-SkillRuntime hasar event'i atesleMEZ -> bunlarla test etme; gercek LMB/skill vurusu kullan.
4. **DirectorMode disabled iken hasar = kayitsiz.** Abonelik OnEnable'da (satir 244), OnDisable'da kopar (251). Telemetry'nin saymasi icin DirectorMode.Instance canli/enabled olmali; once overlay'i bir kez ac (backquote) ki Instance kurulsun, sonra vur.
5. **DPS 5sn pencere + Director pause saati dondurur** (TelemetryClock, satir 2671). Vurduktan cok sonra okursan DPS 0'a duser (pencere kayar) — bu dogru davranis, "0 gordum FAIL" deme; vurustan ~hemen sonra oku. Director state'inde DPS DONAR (telemetryHasLastDps), gercek DPS icin Test state'te oku.
6. **FinalDamage <= 0 kayit edilmez** (satir 2488). 0-hasar vurusu (immune/dead target) event uretmez; canli dusmana gercek hasar ver.
