# DEMO SUNUM PLANI — 2026-06-13 (CANLI HOCA SUNUMU)

> **Kaynak:** CURRENT_STATUS.md + DEMO_TOOLS_SCOPE_DECISION + DEMO_POLISH_BACKLOG + 3 audit raporu (combat/state/unity_bindings) + kod doğrulaması (Grep/Read).
> **Bağlam:** Bitirme projesi bütünleme teslimi, hocaya CANLI sunum (Steam/oyuncu değil). Not = SİSTEMLER. İki ayak: "editörsüz dengeliyorum" (stat tuning + telemetry) + "editörsüz içerik yerleştiriyorum" (prop/ışık).
> **Doğrulama:** Her satır dosya/koddan teyitli. Emin olunmayan yerler UNCERTAIN işaretli.

---

## (A) VAAT → DURUM TABLOSU (kanıtlı)

| # | Vaat | Durum | Kanıt |
|---|------|-------|-------|
| 1 | **Temel combat system** | ✅ ÇALIŞIYOR | Merkezi hasar yolu `SkillRuntime.DealDamage → DamageCalculator → Health` (audit_combat). LMB basic attack packetized + stat-scaled (`BasicAttackBehaviorBase.cs:70-79`). Crash/0-bölme/negatif-loop YOK (audit_combat özet: "DEMO-KRİTİK bulgu YOK"). |
| 2 | **Birden fazla oynanabilir sınıf** | ✅ ÇALIŞIYOR (5/10) | Gerçek skill controller'ı olan 5 sınıf: Warblade, Elementalist, Shadowblade, Ranger, Ronin (`PlayerClassManager.cs:121-159` switch). CharacterSelect 10 sınıf gösteriyor ama 5'i controller'lı; geri kalan 5 (Ravager/Gunslinger/Brawler/Summoner/Hexer) controller'sız. **10 stat profili VAR** (`Resources/Balance/Classes/*_StatProfile.asset` ×10). Demo recipe: 5 buton kullan, 10 değil. |
| 3 | **Sınıfa özgü yetenek + kaynak mekaniği** | ✅ ÇALIŞIYOR | Sınıf-başı kaynak sistemi: Warblade=RageSystem, Elementalist=ManaSystem, Shadowblade=EnergySystem, Ranger=FocusSystem, Ronin=TensionSystem (`PlayerClassManager.cs:141-158`). Her sınıf `AddIfMissing<XSystem>` ile kendi kaynağını alıyor. Q/E/R/F skill'leri sınıfa özel (Cleave, Fireball, vb.). |
| 4 | **Enemy AI + farklı düşman davranışları** | ✅ ÇALIŞIYOR | İki AI tabanı: `EnemyAI` + `BaseMobBehavior`; tier sistemi (`EnemyTier.cs`: Elite/Champion/MiniBoss). Arena mob prefab'ları (FractureImp vb.) `BaseMobBehavior` kullanıyor. **DİKKAT:** iki taban ayrışık → bazı skill hedef-bulma fonksiyonları sadece `EnemyAI` tarıyor (B6, aşağıda risk). |
| 5 | **Oda bazlı ilerleme + karşılaşma yapısı** | ✅ ÇALIŞIYOR | `RoomRunDirector` + `EncounterController.OnRoomCleared → HandleEncounterCleared → RoomClearSequence` (`RoomRunDirector.cs:866,1164,1190`). Oda temizle → reward → exit door → sonraki oda zinciri tam. RoomType.Boss terminal değil, post-boss combat odası var. |
| 6 | **Elite + boss tasarımı** | ✅ ÇALIŞIYOR | Boss: `PenitentSovereign` (prefab + `PenitentSovereign.cs` + `BossIntroController.cs`); boss Health.OnDeath → room clear (`RoomRunDirector.cs:904`). Elite/Champion tier `EnemyTier.cs`'te tanımlı. **DİKKAT:** tier RENK ayrımı bara uygulanmıyor (B9, kozmetik). |
| 7 | **Oda sonu ability selection / progression** | ✅ ÇALIŞIYOR | `DraftManager` 3-kart draft sistemi: run-start opening kit draft (`RoomRunDirector.cs:234-268`) + room-clear reward draft (`SpawnRewardPickup` → draft). Boş loadout başlangıcı, her oda Q/E/R/F dolduruyor. |
| 8 | **İlk boss sonrası dual-class** | ✅ ÇALIŞIYOR (kodda tam) | **Dual-class gate Boss room clear'da tetikleniyor** (`RoomRunDirector.cs:1199-1237`): `CurrentRoomType==Boss && SecondaryClass==None → TriggerClassSelection() → secondary controller eklenir (`PlayerClassManager.AddSecondaryController`) → unlock draft → post-boss combat odasında birleşik kit OYNANIR`. Cross-class Echo sistemi (`PlayerCrossClassBinding`, `CrossClassSkillManager`) bağlı. **UNCERTAIN:** Canlı demoda boss'a kadar oynanıp dual-class'ın gerçekten tetiklendiği uçtan-uca PROVA edilmedi — kod yolu sağlam ama süre uzun (10-dk slice'a sığmayabilir). |
| 9 | **Ölüm → yeniden başlama roguelite loop** | ✅ ÇALIŞIYOR | `DeathScreenManager` ölüm ekranı + `RestartRun()` (`SceneManager.LoadScene("_Arena")`) tam scene reload. Demo için ek: **QUICK RESET butonu** (`DirectorMode.cs:2552 → DemoQuickReset → Revive + CancelDeathForDemo + ClearDirectorSpawns`). B3/B7 fix UYGULANMIŞ (uncommitted; git status'ta DeathScreenManager.cs + DirectorMode.cs modified). |

**SONUÇ: 9/9 vaat ÇALIŞIYOR.** İki tanesinde önemli şerh: #2 (5/10 sınıf demo-güvenli) ve #8 (kod tam ama uçtan-uca prova edilmedi).

**BONUS — Tooling anlatısı (sunumun parçası):** Oyun-içi **Director Mode** (`DirectorMode.cs`, ~2800 satır, `RuntimeInitializeOnLoadMethod` ile sahnesiz boot). Sekmeler: Stats (canlı stat tuning, slider clamp'li), Spawn (cap=10 düşman), Telemetry (5sn DPS + TTK + CSV export), Prop/Light placement. AI-destekli tool zinciri (cx/ax dispatch) + bu in-game tool = mühendislik derinliği kanıtı.

---

## (B) RUN-OF-SHOW (10-15 dk canlı senaryo)

> **Sahne hazırlığı (sunumdan ÖNCE, projeksiyon kapalı):** `_Arena` yükle, konsolu TEMİZLE (`read_console clear` — audit_unity_bindings: 17 transient hata edit-mode noise, runtime'da 0). Warblade ile gir. Director toggle tuşunu (backquote `` ` ``) bir kez test et (UNCERTAIN: TR-Q klavyede çalışıyor mu — F1 alternatifi varsa not al).

### Beat 0 — Açılış anlatısı (1 dk, konuşma)
> "Bu bir 2D roguelite ARPG. Size önce çekirdek sistemleri, sonra bunları geliştirmek için yazdığım **editörsüz oyun-içi geliştirici aracını** göstereceğim. AI-destekli bir tool zinciri kurdum — artık tasarım iterasyonlarını Unity editörünü açıp kapatmadan, oyun çalışırken yapıyorum. Bu altyapı sayesinde bundan sonra çok daha hızlı ilerleyeceğim." → **Vaat: tooling derinliği.**

### Beat 1 — Sınıf seçimi + temel combat (2 dk)
- CharacterSelect ekranı: 10 sınıflık roster göster, **Warblade seç** (kanıtı en sağlam controller). → **Vaat 2, 3.**
- Arena'ya gir, birkaç düşmana **LMB temel saldırı** ile vur. Damage popup'ları + hit juice görünür. → **Vaat 1, 4.**
- 1-2 cümle: "Her sınıfın kendine özgü kaynağı var — Warblade'de Rage, Elementalist'te Mana." → **Vaat 3.**

### Beat 2 — Oda ilerleme + ability draft (2 dk)
- Bir odayı temizle → "ODA TEMİZLENDİ" flash → **3-kart reward draft** açılır, bir skill seç (Q dolar). → **Vaat 5, 7.**
- 1 cümle: "Run boş loadout'la başlıyor, her oda yeteneklerini sen kuruyorsun — roguelite progression." → **Vaat 7.**

### Beat 3 — DIRECTOR MODE: canlı stat tuning (3-4 dk — SUNUMUN YILDIZI)
- Backquote ile Director aç. **Stats sekmesi.** → projeksiyonda overlay görünür (screenshot'a çıkmaz, sorun değil — canlı sunum).
- **physPower slider'ını 50 → 250 çek.** → **MUTLAKA LMB temel saldırı ile vur** (Q/E/R/F bypassStatScaling, slider'a SAĞIR!).
- Telemetry sekmesi: DPS/TTK sayısı canlı değişir. → **Vaat 1 + tooling.**
- (Varsa) **debugGlobalDamageMult slider'ı** = evrensel kol: bunu çekince LMB dahil her packetized hasar ölçeklenir. **DİKKAT:** çıplak-TakeDamage yollarına (Ravager LMB, düşman hasarı, DoT) etkisiz — Warblade LMB ile göster.
- CSV export: "denge verisini Excel'e atıyorum" anı.
- 1 cümle: "Tasarımcı editör açmadan dengeyi runtime'da ayarlasın diye yazdım." → **tooling derinliği.**

### Beat 4 — DIRECTOR MODE: spawn + prop/ışık yerleştirme (2 dk)
- **Spawn sekmesi:** palette'den düşman seç, tıkla → 3-5 düşman spawn (cap=10). → **Vaat 4 + tooling.**
- **Prop/Light placement:** rift_crystal yerleştir (SpriteRenderer + Light2D cyan). → "editörsüz içerik yerleştiriyorum" ayağı.
- Test moduna dön (backquote), spawn'lanan düşmanlarla savaş.

### Beat 5 — Ölüm → roguelite reset (1 dk)
- Düşmanların seni öldürmesine izin ver (veya debugMult ile kendini zayıflat). Ölüm ekranı açılır. → **Vaat 9.**
- **QUICK RESET butonuna bas** → anında dirilirsin, HP full, spawn'lar temizlenir. → "ölümü tek tuşla geri alabiliyorum, demo akışı bozulmuyor" (Hades god-mode refleksi). → **Vaat 9 + tooling.**

### Beat 6 — Boss + dual-class (anlatı, OYNANMAYABİLİR — bkz. D bölümü) (1-2 dk)
- Boss room'a kadar oynama uzun. **Strateji:** ya boss'u önceden kurulu bir state'ten göster, ya da kod/tasarım dokümanıyla anlat. → **Vaat 6, 8.**

### Beat 7 — Kapanış (1 dk)
- "Çekirdek 9 sistemin hepsi çalışıyor. Bu in-game tool + AI dispatch zinciri sayesinde geri kalan içeriği (5 ek sınıf, ek boss'lar) hızla ekleyeceğim." → tooling kapanış.

---

## (C) RİSK + FALLBACK TABLOSU

| Beat | Ne ters gidebilir (audit kanıtı) | Canlı fallback |
|------|----------------------------------|----------------|
| **B3 stat tuning** | physPower slider'ı yanlışlıkla bir **skill (Q/E/R/F) ile** test edilirse hasar DEĞİŞMEZ (audit_combat: 52/54 yetenek `bypassStatScaling:true`). Hoca "stat sistemi çalışmıyor" sanır. | **KESİN KURAL: sadece LMB temel saldırı ile göster.** Şüphede kalırsan debugGlobalDamageMult kullan (LMB'ye etki eder). |
| **B3 Director'da tıklama** | ~~PlayerAttack disable edilmiyordu~~ → **FİX'LENDİ** (`523ca242`: SetPlayerActive — Director pause + ölüm ekranında saldırı kapalı, data-proof'lu). | Risk kalktı; yine de spawn'ı net boş alana tıkla. |
| **B3 Telemetry pause'da** | Director'da (pause) Telemetry sekmesi açılınca 5sn pencere boşalıp **DPS=0** gösterebilir (B5, unscaled-time). | DPS'i **Test modunda** (timeScale=1) hasar verdikten hemen SONRA göster, sonra pause et. DPS-freeze fix uygulandıysa sorun yok (kontrol et). |
| **B3 debugMult=0** | Slider 0'a çekilse bile her vuruş **min 1 hasar** geçer (Health.cs:54 floor, B11). "Hasarı sıfırladım" dersen telemetry yalanlar. | "Sıfır" deme; "minimuma indirdim" de. |
| **B4 spawn/skill kill** | ~~Skill kill'leri sayılmıyordu~~ → **FİX'LENDİ** (`523ca242`: merkezi PublishKillIfDead — skill kill'leri de juice tetikliyor, data-proof'lu). Ravager LMB DPS'te hâlâ görünmez (audit_combat #2, DEFER). | Ravager'ı LMB-DPS demosunda KULLANMA; gerisi serbest. |
| **B4 prop görünmez** | Sprite'sız prop riski (`prop_b50be24..._7_5` boş SpriteRenderer, audit_unity). ~~`floor_riftcrack` Resources dışı~~ → **FİX'LENDİ** (2026-06-13 gece: GUID korunarak Resources'a taşındı, GroundCrack decal artık görünür). | rift_crystal yerleştir (Resources'ta VAR, doğrulandı). Boş quad çıkarsa sil, başka yere koy. |
| **B5 QUICK RESET** | `DemoQuickReset` `ClearDirectorProps()` de çağırıyor → reset **yerleştirdiğin prop'ları da siler** (`DirectorMode.cs:1020`). ~~PlayerAttack~~ → fix'lendi (`523ca242`). | Prop demosunu reset'ten ÖNCE yap; reset'i en son göster. "Sahneyi temiz başlatıyor" diye anlat. |
| **B5 timeScale çatışması** | Ölüm (timeScale=0) + Director toggle yarışı (B4/D-2) → ölü oyuncu + akan düşman + death screen aynı anda. | Ölünce **Director'a girme**, doğrudan QUICK RESET'e bas. |
| **Genel — müzik** | `music_demo` Resources'ta YOK → **arka plan müziği çalmaz** (audit_unity HIGH). | Bahsetme; sessiz demo normal karşılanır. İstersen "ses tasarımı sonraki faz" de. |
| **Genel — konsol** | Edit-mode'da 17 transient hata (RoomRunDirector/JSON, audit_unity). | Sunumdan ÖNCE konsolu temizle; runtime'da gerçek hata yok. |
| **B6 boss/dual-class** | Boss'a kadar oynamak 10-dk slice'ı aşar. ~~UNCERTAIN~~ → **KANITLANDI** (2026-06-13 gece Play-Mode data-proof: gate→seçim→controller 1→2, ManaSystem, 4 slot canlı; rapor `_process/2026-06/_dualclass_proof_2026-06-13.md`. Echo bind'i ayrı draft adımı — test dışı). | Oynama, ANLAT (bkz. D) — artık "kanıtlı çalışıyor" diyebilirsin. |

**En riskli 2 beat:** (1) **B3 stat tuning** — yanlış skill seçimi tüm "stat sistemi" iddiasını çökertir (koreografi disiplini şart). (2) **B6 boss/dual-class** — canlı oynanması zaman/prova riski yüksek.

---

## (D) EKSİKLER / RİSKLİ VAATLER İÇİN KONUŞMA STRATEJİSİ

**1. Dual-class (Vaat 8) canlı tetiklenemezse — DÜRÜST + KANITLI anlat:**
> "Dual-class sistemi tam kodlu, bağlı ve **otomatik testle kanıtlı**: ilk boss'u yenince oyun ikincil sınıf seçimi açıyor; testte Warblade+Elementalist seçiminde skill controller 1→2'ye çıktı, mana sistemi eklendi, ikincil kitin 4 slotu canlı geldi." → İstersen `RoomRunDirector.cs:1199-1237` + kanıt raporu `_process/2026-06/_dualclass_proof_2026-06-13.md` ekranda aç. **Asla "yok" deme — "kanıtlı çalışıyor, canlı boss-run zaman aldığı için test kanıtı üzerinden gösteriyorum" de.**

**2. 5/10 sınıf (Vaat 2) — fazlasını değil, derinliği vurgula:**
> "Şu an 5 sınıf tam oynanabilir durumda (controller + kaynak + skill kit). Diğer 5'inin stat profilleri ve seçim ekranı hazır; in-game tool ve AI dispatch zincirim sayesinde controller'larını hızla tamamlıyorum." → 10 `*_StatProfile.asset` + CharacterSelect roster bunu görsel destekler. **Vaadi karşıladın (birden fazla = ✅); 10'u söz vermediysen 5 zaten yeterli.**

**3. Elite/boss tier görselliği (Vaat 6) — mekanik var, kozmetik eksik:**
> Tier sistemi mekanik olarak çalışıyor (`EnemyTier.cs`), ama bar rengi ayrımı henüz bağlı değil (B9). Sorulmazsa değinme; sorulursa "tier mantığı çalışıyor, görsel ayrım polish backlog'unda" de.

**4. Tooling anlatısını eksiklerin önüne koy:** Her "henüz yok" sorusunda → "bunu hızlandırmak için tam da o in-game tool + AI zincirini kurdum" mesajına bağla. Eksik = yavaşlık değil, **altyapı yatırımının gerekçesi.**

**5. Genel ton:** Bu bir capstone — hoca SİSTEM mimarisine not veriyor, içerik hacmine değil. "9 çekirdek sistemin tamamı işliyor + bunları geliştiren meta-tool yazdım" mesajı, eksik içerikten çok daha güçlü. Gösteremeyeceğin şeyi ekranda canlı oynatma (indie kuralı); kod/doküman ile kanıtla.

---
*LIVE doküman. Üst seviye doğru. Prova sonrası UNCERTAIN maddeleri (dual-class uçtan-uca, TR-klavye backquote, DPS-freeze fix durumu) güncelle.*

---

## (E) F1 DEBUG PANELİ — CEPTEKİ YEDEK TOOL'LAR (council keşfi 2026-06-13 gece)

`DemoDebugPanel` (F1 ile açılır) zaten şunları içeriyor — YENİ KOD GEREKMEDEN sunumda kullanılabilir:
| Buton | Ne yapar | Sunumda ne işe yarar |
|---|---|---|
| **God Mode** (toggle) | Her frame SetImmune+RestoreToFull | Ölüm riskini tamamen kapatır — riskli beat'lerden önce aç |
| **Kill All Mobs** | Enemy tag'li tüm Health'leri öldürür | Oda temizliğini bekletmeden draft/kapı akışına geç |
| **Force Room Clear / Restart Room / Next Room / oda atlama** | Oda akış kontrolü | Boss odasına hızlı ulaşmak / akışı hocaya hızlı gezdirmek |

Anlatı: "Director Mode sunum tool'um, F1 paneli geliştirme debug tool'um — ikisini de kendim yazdım" (tooling hikâyesini güçlendirir, gizlenecek bir şey değil).
DİKKAT: God Mode açıkken stat-tuning hasar beat'i NORMAL çalışır (gelen hasarı keser, çıkan hasarı değil) ama ölüm→Quick Reset beat'ini göstermeden önce God Mode'u KAPAT.
