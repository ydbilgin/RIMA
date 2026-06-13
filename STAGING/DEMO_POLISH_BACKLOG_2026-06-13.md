# DEMO POLISH BACKLOG — 2026-06-13 (gece)

> **Kaynak:** Fable 5 salt-okuma taraması. Kapsam: DirectorMode.cs, Health.cs, DeathScreenManager.cs, SkillVfx.cs, EnemyTier.cs, BaseMobBehavior.cs, SkillRuntime.cs, DamageCalculator.cs, PlayerProjectile.cs, Fireball.cs + playtest notları (`STAGING/playtest_caps_2026-06-13/fable/_NOTES.md`, `opus/_NOTES.md`).
> **Bağlam:** Yarın hocaya CANLI sunum; not = SİSTEMLER. İki ayak: "editörsüz dengeliyorum" (stat tuning + telemetry CSV) + "editörsüz içerik yerleştiriyorum" (prop/ışık).

---

## BÖLÜM 1 — Mantıksızlık Bulguları (önem sıralı)

### 🔴 B1 — Fireball (ve tüm skill projectile'ları) hem stat-scale hem TELEMETRY DIŞI
- **Kanıt:** `Assets/Scripts/Skills/Elementalist/Fireball.cs:73` → `proj.Init(...)` çağırıyor, `SetDamagePacket` HİÇ çağrılmıyor. `Assets/Scripts/Skills/PlayerProjectile.cs:88-100`: `hasDamagePacket=false` olduğunda else dalı ham `hp.TakeDamage(finalDamage)` çalıştırıyor → `DamageCalculator` yok, `SkillRuntime.OnDamageApplied` event'i ATEŞLENMİYOR.
- **Neden mantıksız:** Demonun iki bel kemiği aynı anda kırılıyor: sunumcu physPower'ı 5×'e çeker → Fireball hasarı DEĞİŞMEZ; üstüne Fireball vuruşları Telemetry sekmesine ve CSV'ye hiç düşmez. "Editörsüz dengeliyorum" anlatısı amiral gemisi skill'de sessizce çöker. (Playtest'teki 5× kanıtı packetized DealDamage yoluyla alınmıştı — Fireball'un gerçek vuruş yolu bu DEĞİL.)
- **`SetDamagePacket` kullanan TEK yerler:** `CastRhythmBehavior.cs:81`, `HeatGaugeBehavior.cs:95,142`, `ShotCadenceBehavior.cs:57` (yani sadece LMB/RMB basic-attack projectile'ları).
- **En ucuz fix:** `PlayerProjectile.cs` else dalını `SkillRuntime.DealDamage(hp, damage, popup:false, attacker, hitDirection)` (int-overload) ile değiştir → tek dosya, ~3 satır; telemetry + debugGlobalDamageMult anında kazanılır. Tam stat-scale istenirse `Init`'e packet parametresi eklemek M iş, demo-sonrası.

### 🔴 B2 — Q/E/R/F skill'lerinin TAMAMI `bypassStatScaling: true` — stat slider'ları onlara işlemiyor
- **Kanıt:** `Assets/Scripts/Skills/SkillRuntime.cs:107-122` legacy int-overload her paketi `bypassStatScaling: true` ile kuruyor (satır 120, TODO E2 ile itiraf edilmiş). Grep: ~58 çağrı — Warblade (`Cleave.cs:48`, `GravityCleave.cs:49`...), Shadowblade, Ranger, Elementalist nuke'ları, Ronin'in tamamı bu overload'ı kullanıyor. `DamageCalculator.cs:30-31`: bypass=true → statMultiplier=1.
- **Tek istisna kolu:** `DamageCalculator.cs:45,52` — `debugGlobalDamageMult` bypass'tan BAĞIMSIZ uygulanıyor → bu slider HER ŞEYİ etkiliyor.
- **Demo-güvenli liste (stat slider'a tepki veren + telemetry'ye düşen):**
  - LMB/RMB basic attack'lar — `BasicAttackBehaviorBase.cs:79` packetized (physPower/abilityPower ✓)
  - Ranger/Elementalist basic projectile'ları (ShotCadence/CastRhythm/HeatGauge) ✓
  - `debugGlobalDamageMult` slider'ı → tüm skill'ler dahil her şey ✓ (telemetry zaten skill'lerde çalışıyor; sadece stat scale yok)
  - **GÜVENSİZ:** physPower/abilityPower slider'ı + herhangi bir Q/E/R/F skill'i kombinasyonu; Fireball ise her açıdan güvensiz (B1).
- **En ucuz fix (kod değil, KOREOGRAFİ):** Yarınki stat-tuning beat'ini LMB basic attack + physPower VEYA debugGlobalDamageMult slider'ı ile yap. Kod fix'i (int-overload'ı scale'e açmak) balans davranışını topyekûn değiştirir — demo gecesi DOKUNMA.

### 🔴 B3 — "QUICK RESET" butonu ÖLÜ: onClick listener'ı YOK
- **Kanıt:** `Assets/Scripts/UI/DirectorMode.cs:2373-2376` — buton ve label oluşturuluyor, `onClick.AddListener` HİÇ çağrılmıyor. Ekranın altında her an görünür durumda.
- **Neden mantıksız:** Sunumcu (veya hoca "şuna bas" derse) basar → hiçbir şey olmaz. Canlı demoda çalışmayan görünür buton = en ucuz itibar kaybı.
- **En ucuz fix:** Ya kaldır (1 satır yorum), ya da B7'deki DEMO RESET'e bağla (önerilen — Bölüm 3 #1).

### 🟠 B4 — `Time.timeScale`'in iki sahibi var: Director × DeathScreen çatışması
- **Kanıt:** `DirectorMode.cs:228,233` her state geçişinde timeScale yazıyor (Director=0, Test=1). `DeathScreenManager.cs:96-99` ölümde slowmo→0 yazıyor.
- **Senaryo:** Oyuncu ölür (timeScale=0, panel açık) → sunumcu refleksle ` basar (Director) → tekrar ` (Test) → `SetState` timeScale=1 yapar → ölü oyuncu + akan düşmanlar + death screen AYNI ANDA. Sunumda kafa karıştırır.
- **En ucuz fix:** `DirectorMode.SetState` içinde Test'e dönerken DeathScreen aktifse timeScale'i 0 bırakan tek guard (~4 satır). DEMO RESET ile birlikte paketlenebilir.

### 🟠 B5 — Telemetry DPS penceresi unscaled-time: pause'da gösterilemeden buharlaşıyor
- **Kanıt:** `DirectorMode.cs:2219-2242` `CalculateTelemetryDps` `Time.unscaledTime` kullanıyor; kayıtlar da unscaled damgalı (`SkillRuntime.cs:152`). DPS penceresi 5 sn (`DirectorMode.cs:115`).
- **Neden mantıksız:** Tool'un doğal akışı "hasar ver → ` ile durdur → Telemetry sekmesini hocaya göster". Pause GERÇEK zamanı durdurmaz → sunumcu sekmeyi açana kadar 5 sn'lik pencere boşalır, DPS=0.0 görünür. Pause-incele aracında kendi kendini silen metrik.
- **En ucuz fix:** `State == Director` iken DPS hesabını dondur, son hesaplanan değeri göster (~5 satır).

### 🟠 B6 — `SkillRuntime.FindNearestEnemy` sadece `EnemyAI` tarıyor; arena mobları `BaseMobBehavior`
- **Kanıt:** `SkillRuntime.cs:30` `FindObjectsByType<EnemyAI>`. `Assets/Prefabs/Enemies/FractureImp.prefab:145` yalnız BaseMobBehavior GUID'i (`f02a8792...`) içeriyor; EnemyAI GUID'i (`f97ab131...`) prefab'ta YOK.
- **Etki:** `VeilBurst.cs:28`, `DeathMark.cs:23`, `BackstabMark.cs:20`, `CrossClassEcho.cs:139-140` Director-spawn düşmanlarda hedef bulamaz → Shadowblade demo'da bu skill'ler sessizce boşa gider. (Hemorrhage ailesi etkilenmez — `Hemorrhage.cs:52-65` Physics2D+Health tabanlı, doğru çalışır.)
- **En ucuz fix:** `SkillRuntime.FindNearestEnemy`'yi Hemorrhage pattern'ine çevir (OverlapCircle + Health + !Player) ~8 satır. Veya koreografi: Shadowblade'de bu üç skill'i yarın GÖSTERME.

### 🟠 B7 — Ölüm geri dönüşsüz; tek "retry" tam scene reload = Director kurulumunu da siler
- **Kanıt:** `Health.cs:74` Heal'de `if (IsDead) return;` guard. `Health.RestoreToFull` (81-85) IsDead'e BAKMIYOR — ölüyü teknik olarak diriltebilir AMA `DeathScreenManager.isDead` flag'i, timeScale=0 ve `PlayerController.enabled=false` (`DeathScreenManager.cs:134-139`) geri alınmaz → yarım canlanma. `RestartRun` (142-153) `SceneManager.LoadScene("_Arena")` → Director'ın yerleştirdiği prop'lar, spawn'lar (runtime instantiate, scene-bound) UÇAR.
- **Neden mantıksız:** "Editörsüz içerik yerleştirdim" sahnesini kurduktan sonra tek ölüm her şeyi sıfırlıyor; sunumcu 5 dakikalık kurulumu kaybeder.
- **En ucuz fix (~25 satır, 2 dosya):** `DeathScreenManager`'a public `CancelDeathForDemo()` (isDead=false, panel kapat, timeScale=1, PlayerController enable) + DirectorMode'un ölü QUICK RESET butonunu `playerHealth.RestoreToFull() + CancelDeathForDemo() + ClearDirectorSpawns()` zincirine bağla. B3+B4+B7 tek pakette kapanır.

### 🟡 B8 — Yerelleştirme yarım: Loc altyapısı varken hardcoded İngilizce + ASCII Türkçe karışımı
- **Kanıt:** `DirectorMode.cs:891` `"Spawn cap reached"`; Build sekmesi tamamen hardcoded İngilizce (`1168, 1172, 1205, 1251, 1261, 1267, 1313`); `:561` `"BASLAT"/"DIRECTOR'A DON"` (ş/ö yok); `:717` `"yakinda"`. Spawn/Stats/Telemetry sekmeleri ise düzgün `Loc.T` kullanıyor.
- **Neden mantıksız:** Loc.cs TR+EN zaten hazır (proje kuralı: yeniden kurma, key ekle). Aynı ekranda yarı TR-ASCII yarı EN görünüm "yarım kalmış" izlenimi verir.
- **En ucuz fix:** ~10 Loc key ekle (S, cx'lik mekanik iş).

### 🟡 B9 — EnemyTier renk sistemi kendi kendiyle çelişiyor; "kırmızı kare" karışıklığı
- **Kanıt:** `EnemyTier.cs:23-30` `GetTierColor` (Elite=turuncu, Champion=mor, MiniBoss=altın) tanımlı ama HİÇBİR yerde bar'a uygulanmıyor; `UpdateFill` (103) her tier'da kırmızı→yeşil HP lerp'i. Tier ayrımı görsel olarak mevcut değil = ölü tasarım. `TierLabel` (87-91) sprite'sız boş SpriteRenderer. `CreateWhiteSprite` (106-112) düşman başına yeni Texture2D (cache yok).
- **Playtest düzeltmesi:** Fable notundaki "full canda KIRMIZI bar" büyük olasılıkla bar değil — `BaseMobBehavior.cs:122-135`'teki 48×48 kırmızı fallback placeholder sprite (sprite yüklenemeyince devreye girer). İkisi ayrı şey. **Yarın sabah kontrol:** spawn paletindeki 4 prefab'ın gerçek sprite'la geldiğini gözle doğrula; kırmızı kare görünüyorsa sprite import sorunudur, HP bar değil.
- **En ucuz fix:** Bar BG'sine `GetTierColor(tier)` ver (1 satır) → Elite/Champion ayrımı bedavaya görünür olur.

### 🟡 B10 — Ölü UI chrome: çalışmayan dört eleman ekranda
- **Kanıt:** `SelectionInspector` (`DirectorMode.cs:2349-2363`) "NO SELECTION / ID / HP / AI" statik metin, hiçbir kod güncellemiyor; `MinimapMini` (2342-2347) boş çerçeve; `BuildWorldCursorOverlay` (2332-2340) üç boş Fill, SetActive(false), hiç kullanılmıyor; Map sekmesi "yakinda" stub (717).
- **Neden mantıksız:** Hoca sistemlere not veriyor — tıklanınca hiçbir şey yapmayan/boş duran UI "vitrin" izlenimi verir. İndie kuralı: gösteremeyeceğin şeyi ekrana koyma.
- **En ucuz fix:** Map tab butonunu disable+dim (tabs listesinde tek satır), SelectionInspector+MinimapMini'yi `SetActive(false)` (2 satır). Inspector'ı gerçekten bağlamak Backlog #11.

### 🟡 B11 — Hasar 1'e floor'lanıyor: "0 hasar / god-mode" gösterilemez
- **Kanıt:** `Health.cs:54-55` (TODO E1) + `DamageCalculator.cs:58-60` `Mathf.Max(1, ...)`. `debugGlobalDamageMult` slider'ı min=0 (`DirectorMode.cs:1501`) ama 0'a çekince bile her vuruş 1 hasar geçer.
- **Neden mantıksız:** Slider 0'ı vadediyor, sistem 1 veriyor — sunumcu "hasarı sıfırladım" derse telemetry onu yalanlar.
- **En ucuz fix:** Slider min'ini 0.1 yap (1 sayı) — sistemi değiştirmeden vaadi düzeltir.

### ✅ B12 — Kapanmış playtest bulgusu (doğrulama yeterli)
- Fable notu #2 "ölüyken class-switch çalışıyor" → güncel kodda guard VAR: `DirectorMode.cs:1735-1741` (`playerHealth.IsDead` kontrolü + `player_dead` status). Kod fix'i yapılmış; yarın provada bir kez gözle doğrula.

### ❔ UNCERTAIN — yarın provada 2 dakikalık kontrol listesi
1. **Free-cam dönüşü:** Director'da WASD ile uzaklaşıp Test'e dönünce kamera oyuncuyu takibe geri dönüyor mu? (Follow script'i bu taramada doğrulanmadı.) Dönmüyorsa "Home=oyuncuya snap" Backlog #7'ye dahil.
2. **TR klavyede backquote (`):** Türkçe-Q düzende toggle tuşu sorunsuz mu? Değilse F1 alternatifi tek satır.
3. **maxHP slider max=300** (`DirectorMode.cs:1496`): sınıf baseline'ları 300'ü aşıyorsa slider kelepçeler — profile değerlerine göre kontrol.
4. **R tuşu sürprizi:** Ölüyken Director'dayken bile R scene reload tetikler (`DeathScreenManager.cs:71`) — sunumcu bilsin.

---

## BÖLÜM 2 — Indie-Lens Backlog

Öncelik: **S**=sunumu taşır / **A**=belirgin katkı / **B**=güzel olur. Efor: **S**=cx 1-2 saat / **M**=yarım gün / **L**=demo-sonrası. Referans kültür: Hades god-mode, Dead Cells custom mode, Noita/Brotato debug konsolu, Vampire Survivors hızlı iterasyon.

| # | Fikir | Ne / Neden etkileyici | Öncelik | Efor | Zaman |
|---|---|---|---|---|---|
| 1 | **DEMO RESET (tek tuş dirilt)** | Ölü QUICK RESET butonunu gerçek işe bağla: RestoreToFull + CancelDeathForDemo + spawn temizle + timeScale arbitraj (B3+B4+B7). Hades god-mode refleksi: sunumcu ölümü artık felaket değil, tek tık. | S | S | ÖNCE |
| 2 | **Stat değişim toast'u** | `OnStatSliderChanged`'de ekran ortasına 1.2 sn `PHYSPOWER 50 → 250` (Jersey10, büyük, renk kodlu, eski→yeni). Hocanın gözü slider'da değil oyunda — kanıtı oyun ekranına bas. | S | S | ÖNCE |
| 3 | **DPS freeze (pause'da)** | B5 fix'i: Director state'inde son DPS donsun. Telemetry sekmesi pause'da anlamlı kalır — tool'un varlık sebebi bu. | S | S | ÖNCE |
| 4 | **Telemetry CSV → dosyaya yaz** | Pano yerine `STAGING/telemetry_<saat>.csv` + status'ta tam yol. "İşte CSV'si, Excel'de açıyorum" anı panodan çok daha ikna edici. (`DirectorMode.cs:2247` tek satır değişir + File.WriteAllText.) | A | S | ÖNCE |
| 5 | **Preset butonları: TANK / GLASS CANNON / BASELINE** | Stats sekmesine 3 buton, hazır stat setleri. Tek tıkta dramatik fark = Vampire Survivors iterasyon hızı hissi; slider sürüklemekten daha sahne-dostu. | A | S | ÖNCE |
| 6 | **Undo: son yerleştirmeyi geri al (Ctrl+Z)** | `directorPlacedProps`/`directorSpawnedEnemies` zaten liste — son elemanı sil ~10 satır. Yanlış tıklama sahnede panik yaratmasın. | A | S | ÖNCE |
| 7 | **Slow-mo toggle (örn. T, timeScale 0.25)** | VFX/vuruş anını hocaya ağır çekimde göster — Dead Cells juice sergisi. DİKKAT: B4 timeScale guard'ıyla birlikte yapılmalı, yoksa üçüncü yazar ekleriz. | A | S | ÖNCE (guard'la) |
| 8 | **Spawn dust-puff** | `SpawnSelectedEnemy` içinde `SkillVfx.ImpactBurst(position, VfxElement.Arcane)` = 1 satır; PopSpawn zaten var. Spawn anı "pat" diye hissedilir. | B | S | ÖNCE (bedava) |
| 9 | **Map tab gizle + ölü chrome temizliği** | B10: stub/boş elemanları kaldır. Negatifi silmek de polish'tir. | A | S | ÖNCE |
| 10 | **Işık tuning slider'ları (Build sekmesi)** | Seçili prop'un Light2D intensity/radius/renk slider'ı — rift_crystal cyan'ını CANLI değiştir. "Editörsüz içerik" ayağının ışık taçlandırması; in-game tool felsefesinin sıradaki doğal adımı. Ayrıca opus notundaki "rift_crystal ışık ince ayarı"nı tool'un kendisiyle çözer = meta-kanıt. | A | M | ÖNCE-gece (cx) / yoksa SONRA |
| 11 | **Selection Inspector'ı gerçekten bağla** | Director'da mob'a tıkla → ad/HP/state/tier görüntüle (+ "Kill" butonu). Çerçevesi zaten duruyor (B10) — ölü chrome'u değere çevirir. Noita debug-inspect hissi. | B | M | SONRA |
| 12 | **Canlı DPS sparkline** | Telemetry'de son 30 sn'lik mini çizgi grafik (Image dizisi, LineRenderer'sız). CSV'nin canlı önizlemesi. | B | M | SONRA |
| 13 | **Encounter-wave editörü** | Spawn sekmesi+: mob/adet/gecikme listesi kur → "dalga başlat" → telemetry ile ölç. "Editörsüz dengeliyorum"un encounter boyutu; in-game tool #2. | B | L | SONRA |
| 14 | **Kamera yolu kaydedici** | Director free-cam waypoint'leri kaydet → oynat. Fragman/b-roll çekimi oyun içinden; in-game tool #3. | B | L | SONRA |
| 15 | **FPS guard (otomatik spawn temizleme)** | Spawn cap=10 zaten koruyor (`DirectorMode.cs:117`) — gerçek ihtiyaç doğana kadar YAGNI. | B | S | SONRA |
| 16 | **Slider ghost mark (önceki değer çentiği)** | Slider üstünde reset-öncesi değerin soluk işareti — "nereden nereye" tek bakışta. Toast (#2) varken nice-to-have. | B | S | SONRA |

---

## BÖLÜM 3 — YARIN İÇİN İLK 3 (bu gece cx dispatch'lik)

### 1) DEMO RESET paketi (B3 + B4 + B7) — `DirectorMode.cs` + `DeathScreenManager.cs`
- `DeathScreenManager.CancelDeathForDemo()` public metodu: `isDead=false`, panel SetActive(false), `Time.timeScale=1`, PlayerController enable.
- DirectorMode QUICK RESET onClick: `playerHealth.RestoreToFull()` (IsDead'e bakmıyor, zaten çalışır) → `CancelDeathForDemo()` → `ClearDirectorSpawns()` → status mesajı.
- `SetState`'e DeathScreen-aktif guard'ı.
- **Gerekçe:** Sunumcu ölümü demonun tek geri-dönüşsüz felaketi; fix 2 dosyada izole; `*ForValidation` pattern'i hazır olduğundan `execute_code` ile data-proof testi yazılabilir. Bonus: ekrandaki ölü buton da dirilmiş olur.
- **Risk:** Düşük — yeni state eklenmiyor, mevcut state'ler geri sarılıyor.

### 2) Projectile telemetry yolu (B1) — `PlayerProjectile.cs` tek dosya
- Else dalındaki `hp.TakeDamage(finalDamage)` → `finalDamage = SkillRuntime.DealDamage(hp, damage, popup:false, attacker: attacker != null ? attacker : gameObject, hitDirection)` (int-overload; `PublishSkillHit` çift çağrısını kaldırmaya dikkat — overload zaten publish ediyor).
- **Gerekçe:** Fireball = demonun en gösterişli skill'i; bu fix'le vuruşları telemetry+CSV+debugMult'a girer. bypassStatScaling davranışı korunur (balans değişmez), yani risk minimal ama demo anlatısı kurtulur. physPower beat'i yine LMB ile yapılır (B2 koreografi notu).
- **Risk:** Düşük — tek dal; DoT/knockback akışı `onHit` üzerinden zaten ayrı.

### 3) Sunum parlaklık paketi: Stat toast + DPS freeze (+1 satır dust-puff) — `DirectorMode.cs` içi
- Toast: `OnStatSliderChanged`'de eski değeri yakala, ekran-ortası 1.2 sn TMP fade (unscaled). DPS freeze: `State==Director` iken `CalculateTelemetryDps` cache döndürsün. Dust-puff: `SpawnSelectedEnemy`'ye `SkillVfx.ImpactBurst` satırı.
- **Gerekçe:** Üçü de DirectorMode-içi (~40-50 satır), sahne etkisi/efor oranı en yüksek üçlü; hocanın göreceği ilk "wow" anları. Overlay runtime-built olduğundan prefab/scene diff riski yok.
- **Risk:** Çok düşük — salt görsel, oyun mantığına dokunmuyor.

**Bilinçli dışarıda bırakılanlar:** B6 (FindNearestEnemy) — Shadowblade'in etkilenen 3 skill'i yarın gösterilmeyecekse fix yerine koreografi notu yeterli; B2 kod fix'i — balansı topyekûn değiştirir, demo gecesi dokunulmaz; #10 ışık slider'ları — M efor, ancak gece vakti kalırsa.
