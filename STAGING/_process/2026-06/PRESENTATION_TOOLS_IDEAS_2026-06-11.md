# RIMA — Sunum/Demo Araçları Fikirleri (Council, 2026-06-11)

> Kullanıcının isteği: sunum yaparken işe yarayacak fonksiyonel Unity araçları. 3 danışman bağımsız düşündü (Opus showcase-tasarım · Gemini tür/endüstri · Sonnet pratik-Unity). Bu dosya = toplanmış fikirler. **Kullanıcı dönünce sunulacak + hangileri yapılacak sorulacak.**

## Mimari ilke (3 danışman ortak)
Tek `DebugToolkit.cs` (hotkey + IMGUI) + tek `PresenterLayer` prefab. Hepsi `#if DEMO_BUILD || DEVELOPMENT_BUILD` derleme bayrağı arkasında → production'a sızmaz. Sahneye tek component eklenince çalışır.

---

## TIER 1 — Düşük efor, yüksek etki (önce bunlar; demo riskinin ~%80'ini siler)

| # | Araç | Ne yapar (Unity üzerinden) | Hook | Tuş | Efor |
|---|---|---|---|---|---|
| 1 | **God-Mode** | Alınan hasarı 0'a sabitler | `PlayerHealth.TakeDamage()` guard | G | S |
| 2 | **Full Heal** | Canı maxHP'ye set eder | `PlayerHealth.SetHealth(max)` | H | S |
| 3 | **Give Gold/Skill** | Sabit altın + test skill verir | `RunStats` + `DraftManager.ForceGrantSkill()` | F1 | S |
| 4 | **Kill-All** | Aktif tüm düşmanları öldürür | `EncounterController` enemy listesi → `Die()` | K | S |
| 5 | **Instant Reset** | Run'ı baştan başlatır | `RoomRunDirector.ForceRestart()` (RunStats sıfır, graph rebuild) | Shift+R | S/M |
| 6 | **Debug HUD Overlay** | FPS, oda, enemy count, HP, aktif skill gösterir | IMGUI `OnGUI`, mevcut component'lerden çek | F8 | M |
| 7 | **Presenter Mode** | Tüm debug/overlay gizle → temiz HUD master switch | `PresenterModeController`, debug Canvas `SetActive(false)` | F9 | S |

**Not:** 1-5 bir günde kurulur, canlı sunumda ölme/softlock riskini sıfırlar.

---

## TIER 2 — Orta efor, "wow" + kontrol

| # | Araç | Ne yapar (Unity üzerinden) | Hook | Efor |
|---|---|---|---|---|
| 8 | **Jump-to-Node / Room Skip** | İstenen oda tipine atlar (boss/reward/combat) | `RoomRunDirector.JumpToNode(type)` | M |
| 9 | **Enemy/Boss Spawn Dial** | Seçilen mob/boss'u fare konumuna spawn eder | `EncounterController.SpawnEnemy(id,pos)` + mini dropdown | S/M |
| 10 | **Free-Cam + Zoom** | WASD pan + scroll zoom, sahneyi/asset'i gezme | `DebugCamera.cs`, Cinemachine/follow disable | M |
| 11 | **Cinematic Slow-Mo** | `Time.timeScale=0.3` + yakın kamera; ulti/boss anı | timeScale toggle + `fixedDeltaTime` koru | M |
| 12 | **Screenshot Capture** | HUD-gizli temiz frame yakala (marketing shot) | `ScreenCapture.CaptureScreenshot` + Presenter Mode | F12 | M |

---

## TIER 3 — Showcase / fuar (M-L, zaman kalırsa)

| # | Araç | Ne yapar (Unity üzerinden) | Efor |
|---|---|---|---|
| 13 | **Boss-Rush Showcase** | Run loop'u atlayıp boss'ları art arda sergiler, full-build verir (`ShowcaseSequencer`) | M |
| 14 | **Asset Gallery Sahnesi** | Yeni UI chrome + node sembolleri + sınıfları grid'de sergileyen vitrin scene | L |
| 15 | **Attract / Demo Loop** | Kimse oynamazken (60s idle) input-playback ile otomatik demo döngüsü (fuar standı) | M/L |
| 16 | **Telemetri CSV Logger** | Ölüm/seçim/geçiş'i `persistentDataPath/telemetry.csv`'ye yazar; ölüm heatmap | M |

---

## Danışman örnekleri (referans)
- **Hades (Supergiant):** press build'de data-driven debug menü, boon'ları anında ver
- **Dead Cells (Motion Twin):** stream'de canlı "give weapon" + trailer için düşman-dondur combat frame
- **Slay the Spire:** uzaktan okunur büyük-font streamer modu
- **Spelunky:** attract mode = input-playback (AI değil)
- **Vlambeer (Nuclear Throne):** milisaniye restart → fuar akışı

## Convergence (3 danışman da vurguladı)
God-mode · Instant reset · Debug HUD · Presenter mode toggle · `#if DEMO_BUILD` guard.

---

## KULLANICIYA SORULACAK (dönünce)
1. Hangi tier'ler? (öneri: **Tier 1 tamamı** + Tier 2'den slow-mo + free-cam)
2. Tek `DebugToolkit.cs`'e mi toplansın, ayrı mı?
3. Asset Gallery (yeni chrome+sembolleri sergileme) sunumda istiyor musun? — emek görünür olur
4. Telemetri/playtest verisi sunumda toplansın mı?
