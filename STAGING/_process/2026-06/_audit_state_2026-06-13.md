# RIMA Tam Oyun Audit — Lens 2: Oyun-Durumu State Makinesi + Director Mode Etkileşimleri

Tarih: 2026-06-13 · Kapsam: `Assets/Scripts/` ŞU ANKİ hali (git history'ye bakılmadı).
Önem: 🔴 DEMO-KRİTİK · 🟠 HIGH · 🟡 MEDIUM · ⚪ LOW

---

## 1. Death / Respawn / timeScale zinciri

### 🔴 D-1 — Ölü oyuncu, ölüm ekranında hâlâ saldırabilir (PlayerAttack disable EDİLMİYOR)
`DeathScreenManager.cs:139-140` (ölümde) ve `:168-169` (reset'te) yalnızca `PlayerController.enabled`'i değiştiriyor. Oyuncunun saldırı bileşeni `PlayerAttack` AYRI bir component ve hiçbir yerde devre dışı bırakılmıyor. `PlayerAttack.Update()` (`PlayerAttack.cs:226-249`) her frame `attackAction.WasPressedThisFrame()` okuyor — `Time.timeScale=0` Update'i durdurmaz, Input System polling timeScale'den bağımsızdır.
Sonuç: ölüm ekranı açıkken sol/sağ tıklama hâlâ saldırı/skill girişi üretir; `behavior.OnUpdate(..., Time.deltaTime)` cooldown'ları dondursa da giriş kuyruğa girebilir, üstelik R/restart tuşuyla yarışır.
**Fix:** `DeathSequence` ve `CancelDeathForDemo`'da `PlayerController` ile birlikte `PlayerAttack`'i de (varsa) `enabled = false/true` yap; tek bir `SetPlayerActive(bool)` helper'ı simetriyi garanti eder.

### 🟠 D-2 — Ölüm sıralaması: DeathScreen slowMo penceresinde Director'a girmek timeScale çatışması yaratıyor
`DeathScreenManager.DeathSequence` 1.5s boyunca `timeScale=0.15` (`:98-99`), sonra `0` (`:101`) yazıyor. `DirectorMode.ResolveTimeScaleForState` (`:261-262`) yalnızca `IsDeathActiveForDemo` true ise 0 döndürüyor — `isDead` slowMo başında true olduğu için bu kısmen korunuyor. ANCAK slowMo penceresinde (`yield WaitForSecondsRealtime`) kullanıcı backtick ile Director'a girip Test'e dönerse, `SetState(Test)` `timeScale=0` (death aktif) bırakır; coroutine devam edip `timeScale=0` yazınca tutarlı. Asıl risk: Director Test moduna dönüş anında death coroutine HENÜZ `0.15` fazındaysa, Director `0` yazar, sonra coroutine `WaitForSecondsRealtime` bitince yine `0` yazar → görsel olarak slowMo "atlanır" ama kilit yok.
**Fix:** Düşük riskli; `DeathSequence` başında State'i kilitle veya slowMo'yu `IsDeathActiveForDemo` true iken Director toggle'ını yok say. Demo prova ile doğrula.

### 🟡 D-3 — `RoomRunDirector.ClearSlowMoBlip` ile DeathScreen slowMo çift-sahip
`RoomRunDirector.cs:1336-1355` oda temizlenince `timeScale`'i kademeli geri yükler; aynı anda oyuncu ölürse `DeathSequence` `timeScale=0.15→0` yazar. İki coroutine de `Time.timeScale`'in tek sahibi olduğunu varsayıyor. `ClearSlowMoBlip` `:1342` "başkası değiştirdiyse bırak" guard'ına sahip (iyi), ama `DeathSequence`'te böyle bir guard yok — death her zaman kazanır, bu istenen davranış. Kilit yok, sadece geri-yükleme yarışı.
**Fix:** Gerekli değil; davranış kabul edilebilir. İzleme amaçlı not.

### 🟡 D-4 — `DeathScreenManager.OnDisable` yalnızca `isDead` iken timeScale=1 yapıyor
`:77-81`. Scene unload / restart anında `isDead=false` ama `timeScale` Director tarafından 0'a çekilmişse, DeathScreen disable olurken sıfırlanmaz. Pratikte `RestartRun` (`:177`) zaten 1 yazıyor, bu yüzden kapalı; ama Director pause + sahne değişimi kombinasyonunda savunma zayıf.
**Fix:** `OnDisable`'da koşulsuz `Time.timeScale = 1f` (zaten restart yolları 1 yazıyor, zararsız).

---

## 2. Spawn / Despawn yaşam döngüsü

### 🔴 S-1 — Director-spawn düşman ölümü KILL sayılmıyor + juice/telemetri tetiklemiyor
Kill sayacı `CombatEventBus.PublishKill` üzerinden işliyor (`BasicAttackBehaviorBase.cs:93`) ve `RunStats.OnKill` (`RunStats.cs:171-175`) bunu dinliyor. Ama PublishKill SADECE basic-attack ölüm dalında çağrılıyor; `SkillRuntime.DealDamage` yolunda PublishKill YOK (grep: SkillRuntime'da hiç PublishKill yok). Yani skill/mermi ile öldürülen düşmanlar kill sayılmaz, ScreenShake/HitPause/Vignette kill-juice'u almaz.
Ayrıca Director-spawn düşmanları (`DirectorMode.cs:926-938`) `RuntimeRoomManager.OnEnemyDied`'a abone DEĞİL (RRM yalnızca kendi spawn'larını `:540/575/611`'de bağlar), bu yüzden `aliveEnemies` sayacına da girmez — istenen olabilir ama demo'da "kill" göstergeleri Director düşmanlarında çalışmaz.
**Fix (demo-kritik kısım):** `BasicAttackBehaviorBase` dışındaki ölümlerde de PublishKill tetiklendiğinden emin ol (merkezî: `SkillRuntime.DealDamage` içinde `hp.IsDead` olunca PublishKill). En azından LMB-only demo akışında doğrula.

### 🟠 S-2 — `RegisterKill()` boş gövde — ölü kod, yanıltıcı
`DeathScreenManager.cs:84-86` `RegisterKill()` tamamen boş; `RuntimeRoomManager.cs:811` her düşman ölümünde çağırıyor. `KillCount => RunStats.Kills` (`:44`) gerçek kaynağı RunStats. Yani `RegisterKill` no-op; çağrı zinciri yanıltıcı, birisi "kill burada sayılıyor" sanabilir.
**Fix:** `RegisterKill` ve `RuntimeRoomManager:810-811` çağrısını sil (kill RunStats/CombatEventBus'ta sayılıyor), ya da gövdesine yorum: "kill RunStats üzerinden, burası kasıtlı no-op".

### 🟠 S-3 — Director-spawn düşmanına eklenen OnDeath listener'ı temizlenmiyor (closure leak)
`DirectorMode.cs:933-938`: her spawn'da `health.OnDeath.AddListener(() => directorSpawnedEnemies.Remove(tracked))`. `ClearDirectorSpawns` (`:992-1003`) ve `EraseDirectorEnemyAt` GameObject'i Destroy ediyor ama listener'ı RemoveListener etmiyor. GameObject yok olunca UnityEvent listener'ı da gider (Health objeyle birlikte ölür), bu yüzden gerçek leak DEĞİL; ancak `DemoQuickReset` sonrası tekrar tekrar spawn → her birinde yeni closure, hepsi obje ömrüyle sınırlı. Düşük risk.
**Fix:** Gerekli değil; obje-ömürlü. İzleme notu.

### 🟡 S-4 — Spawn-cap status metni hardcoded İngilizce (Loc bypass)
`DirectorMode.cs:920-922`: `spawnStatusText.text = $"Spawn cap reached ({MaxDirectorSpawnedEnemies})."` — diğer tüm status'ler `Loc.T(...)` kullanırken bu çift-dilli sistemi atlıyor. Build/prop status'lerinin tamamı da hardcoded İngilizce (`:1255,1259,1338,1348,1354,1400` vb.).
**Fix:** Loc key'leri ekle; demo TR ise tutarsız görünür. Düşük risk, kozmetik.

### 🟡 S-5 — Director-spawn düşmanı `OverlapCircle(Default)` reserve kontrolünü atlıyor
Director spawn (`:926`) düşmanı doğrudan Instantiate ediyor, RRM'in `:790` walkability/overlap reservasyonu yok. Director düşmanı duvar/void içine yerleşebilir. Director için kabul edilebilir (manuel yerleştirme), ama AI void'e sıkışabilir.
**Fix:** Gerekli değil (Director = manuel kontrol).

---

## 3. DirectorMode ↔ Oyun Etkileşimi

### 🔴 DM-1 — Director açıkken (timeScale=0) tıklama hem spawn hem OYUNCU SALDIRISI tetikliyor
`DirectorMode.UpdateSpawnTool` (`:903`) `IsPointerOverDirectorUi()` ile UI üstündeki tıklamayı eler. Ama dünyaya tıklayınca (UI dışı) `Mouse.leftButton.wasPressedThisFrame` ile spawn yapar — AYNI frame'de `PlayerAttack.Update` (`:239`) de `attackAction.WasPressedThisFrame()` true görür. PlayerController/PlayerAttack Director state'ten habersiz; ikisi de aynı LMB'yi tüketir. timeScale=0 olduğu için saldırı animasyonu ilerlemez ama giriş kaydı/`inputBuffer.RequestAttack` kuyruğa girer ve Test'e dönünce boşalır.
**Fix (demo-kritik):** Director Director-state'e girerken `PlayerController` + `PlayerAttack`'i disable et (D-1 ile aynı helper). `SetState`'te `state==Director ? disable : enable`.

### 🟠 DM-2 — `Bootstrap` + `OnEnable`/`OnDestroy` event simetrisi: çift-abonelik riski yönetilmiş ama `OnDamageApplied` `Instance` yarışı
`OnEnable` (`:218-219`) önce `-=` sonra `+=` yapıyor (idempotent, iyi). `OnDestroy` (`:201`) `-=` yapıyor. `Loc.OnLanguageChanged` `Awake`'te `+=` (`:164`), `OnDestroy`'da `-=` (`:203`), `RebuildOverlayRuntime`'da `-=/+=` (`:669-670`) — simetrik. Sızıntı görünmüyor. Ancak `Bootstrap` (`:140-151`) `RuntimeInitializeOnLoadMethod` ile `DontDestroyOnLoad` singleton kurar; `#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR` dışında DirectorMode tipi DERLENMEZ (`:1, :2795 #endif`). Release build'de `DeathScreenManager`/diğerleri DirectorMode'a referans VERMİYOR (FindObjectsByType<DeathScreenManager> tersine), bu yüzden derleme güvenli. Doğrulandı.
**Fix:** Gerekli değil. Not.

### 🟠 DM-3 — `FindObjectsByType<DeathScreenManager>` her `ResolveTimeScaleForState` çağrısında (pahalı)
`DirectorMode.cs:265-269` `FindDeathScreenManager()` her state değişiminde `FindObjectsByType` yapıyor. State değişimi seyrek (toggle) olduğu için Update-içi DEĞİL — kabul edilebilir. Ama `SetState` ayrıca aynı-state dalında (`:234-239`) da çağırıyor; her `ApplyStateText` öncesi tarama olası.
**Fix:** Düşük öncelik; `DeathScreenManager` referansını cache'le (ilk bulduğunda sakla).

### 🟡 DM-4 — `directorCamera = Camera.main` cache'i sahne değişiminde bayatlıyor
`CacheCameraTarget` (`:558-570`) `directorCamera`'yı bir kez yakalıyor, `hasCameraTarget` true olunca bir daha güncellemiyor. Restart/scene reload sonrası eski (Destroy edilmiş) kamera referansı kalabilir → free-cam çalışmaz veya NRE. DirectorMode `DontDestroyOnLoad`, kamera değil.
**Fix:** Sahne yüklenince `directorCamera=null; hasCameraTarget=false` sıfırla (SceneManager.sceneLoaded aboneliği) ya da null-check'te `Camera.main`'i yeniden çek.

### 🟡 DM-5 — `statToastRoutine` / coroutine'ler `DontDestroyOnLoad` obje üstünde, sahne geçişinde durmuyor
DirectorMode `DontDestroyOnLoad` olduğu için `PlayStatToast`, `PopSpawn` coroutine'leri sahne değişse de yaşar. `PopSpawn` (`:1113`) hedef `target != null` guard'ı ile güvenli. `statToastRoutine` (`:2072-2077`) yeni başlatmadan önce eskisini durduruyor (iyi). Leak yok ama sahne geçişinde toast yarıda kalabilir.
**Fix:** Gerekli değil; guard'lar yeterli.

### 🟡 DM-6 — `static event OnStateChanged / OnTabChanged` scene reload'da sıfırlanmıyor
`DirectorMode.cs:42-43` static event'ler. Aboneler `-=` yapmazsa ve DirectorMode `DontDestroyOnLoad` tek instance olduğundan, abone tarafı (varsa) leak edebilir. Grep: bu event'lere abone bulunamadı (kullanılmıyor) — şimdilik zararsız.
**Fix:** Kullanılmıyorsa sil; kullanılacaksa abonelerde `-=` zorunlu.

---

## 4. Genel C# Tuzakları

### 🟠 G-1 — `PlayerProjectile` lifetime Destroy timeScale=0'da hiç çalışmaz, mermi havada donar
`PlayerProjectile.cs:63` `Destroy(gameObject, lifetime)` SCALED time kullanır. Director pause (timeScale=0) sırasında mermi süresi dolmaz, ekranda asılı kalır; Test'e dönünce normal. Demo'da Director'a girip çıkınca eski mermiler birikebilir. Düşük-orta risk.
**Fix:** Kabul edilebilir; istenirse Director'a girerken aktif mermileri temizle.

### 🟡 G-2 — `Health.OnDeath` null-coalescing `Awake`'te kuruluyor ama dışarıdan `AddComponent<Health>` sonrası Awake garanti
`DirectorMode.cs:933-935` spawn edilen prefab'da Health yoksa `AddComponent<Health>()`; UnityEvent'ler `Awake`'te `??= new` (`Health.cs:43-45`) ile kurulur, AddComponent Awake'i hemen çağırır → `OnDeath.AddListener` (`:938`) güvenli. Doğrulandı, sorun yok.

### 🟡 G-3 — `telemetryDeathListeners` / `telemetryFirstHitTimes` Director düşmanı yok edilince sözlükte yetim Health key'i kalabilir
`OnDamageAppliedTelemetry` (`:2221-2230`) hedef Health'e OnDeath listener ekler ve sözlüklere yazar. Düşman `EraseDirectorEnemyAt` ile Destroy edilirse (ölmeden), `CompleteTelemetryTtk` çağrılmaz → `telemetryFirstHitTimes[deadHealth]` ve `telemetryDeathListeners[deadHealth]` yetim kalır (Unity-null key). `ClearTelemetry` (`:2263`) temizler ama o ana kadar sözlük şişer; `ClearTelemetryDeathListeners` (`:2284-2295`) `pair.Key != null` guard'ı ile Unity-null'ı atlar (RemoveListener çağırmaz, zaten obje ölü). Gerçek leak küçük.
**Fix:** Düşük öncelik; spawn/erase'te telemetri sözlüklerinden de düşür ya da periyodik prune.

### ⚪ G-4 — `Health.TakeDamage` 100% DR halinde min 1 hasar (TODO E1 işaretli, bilinen)
`Health.cs:54-55` `Mathf.Max(1, ...)` — `incomingDamageMultiplier=0` olsa bile 1 hasar geçer. Kodda TODO ile işaretli, bilinen tasarım boşluğu. Demo'da Unyielding/immune ayrı `immune` guard'ı (`:51`) ile karşılanıyor.
**Fix:** DR semantiği tanımlanınca; demo-bloker değil.

---

## Özet Tablo

| Önem | ID | Başlık | Dosya:Satır |
|------|----|--------|-------------|
| 🔴 | D-1 | Ölü oyuncu ölüm ekranında saldırabilir (PlayerAttack disable yok) | DeathScreenManager.cs:139-140 |
| 🔴 | S-1 | Skill/mermi ile ölüm KILL sayılmıyor + juice yok (PublishKill sadece basic-attack) | BasicAttackBehaviorBase.cs:93 / SkillRuntime (eksik) |
| 🔴 | DM-1 | Director'da tıklama hem spawn hem oyuncu saldırısı tetikliyor | DirectorMode.cs:903-909 / PlayerAttack.cs:239 |
| 🟠 | D-2 | Death slowMo penceresinde Director toggle timeScale yarışı | DeathScreenManager.cs:98-101 |
| 🟠 | S-2 | `RegisterKill()` boş no-op, yanıltıcı çağrı zinciri | DeathScreenManager.cs:84-86 / RuntimeRoomManager.cs:811 |
| 🟠 | S-3 | Director-spawn OnDeath listener RemoveListener edilmiyor (obje-ömürlü) | DirectorMode.cs:933-938 |
| 🟠 | DM-3 | `FindObjectsByType<DeathScreenManager>` cache'siz | DirectorMode.cs:265-269 |
| 🟠 | G-1 | Mermi lifetime Destroy timeScale=0'da donuyor | PlayerProjectile.cs:63 |
| 🟡 | D-3 | RoomRunDirector slowMo ↔ Death slowMo çift-sahip | RoomRunDirector.cs:1336 |
| 🟡 | D-4 | DeathScreen.OnDisable koşullu timeScale reset | DeathScreenManager.cs:77-81 |
| 🟡 | S-4 | Spawn/Build status hardcoded İngilizce (Loc bypass) | DirectorMode.cs:920-922 |
| 🟡 | DM-4 | `directorCamera` cache scene reload'da bayatlıyor | DirectorMode.cs:558-570 |
| 🟡 | DM-6 | static event'ler scene reload'da sıfırlanmıyor (kullanılmıyor) | DirectorMode.cs:42-43 |
| 🟡 | G-3 | Telemetri sözlüklerinde yetim Health key (erase yolu) | DirectorMode.cs:2221-2230 |
| ⚪ | G-4 | 100% DR'de min 1 hasar (TODO işaretli) | Health.cs:54-55 |

**Toplam:** 15 bulgu · 🔴 3 · 🟠 5 · 🟡 6 · ⚪ 1
**En kritik demo riski:** D-1 + DM-1 aynı kök (PlayerAttack Director/Death state'inden habersiz, hiç disable olmuyor) → tek `SetPlayerActive(bool)` helper'ı ile çözülür.
