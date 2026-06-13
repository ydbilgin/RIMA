# LaurethStudio Playbook — RIMA'dan Çıkarılan Taşınabilir Düzen (2026-06-13)

> **Amaç:** RIMA (capstone Unity 2D ARPG) sürecinde evrimleşen disiplinlerden, **RIMA'dan bağımsız, her projeye taşınabilir** olanları çıkarmak. Bu dosya = "studio playbook" tohumu. Her madde için: RIMA'da nasıl doğdu → genel form → taşıma maliyeti → öncelik (S/A/B).
>
> **Öncelik anahtarı:** S = her projede gün-1'den olmalı · A = ilk hafta · B = proje olgunlaşınca / duruma göre.
>
> **Kaynak okumalar:** `.claude/PROJECT_RULES.md`, `MEMORY.md`, `project_modular_design_philosophy.md`, `reference_dispatch_skills.md`, `reference_directormode_validation_api.md`, `feedback_*`, global `~/.claude/commands/bootstrap-project.md`, `CURRENT_STATUS.md`.

---

## KATEGORİ 1 — Multi-Agent Orkestra Düzeni

### 1.1 — Orchestrator Bulk Yapmaz (Conductor disiplini)
- **RIMA'daki kanıt:** PROJECT_RULES "Orchestrator Context Koruma (S63)" — "ORCHESTRATOR BULK İŞ YAPMAZ, istisna yok". Orchestrator'ın context'i milyon-token bile olsa dolduğunda kalite düşüyordu; kod/batch/commit işleri dispatch'e taşındı. Sonnet=orkestra-only, Opus=nadir karar.
- **Genel form:** Sürücü model (orchestrator) sadece DAĞITIR + SENTEZLER + DOĞRULAR + commit'ler. Mekanik bulk (kod yazma, dosya batch, doc batch) her zaman ayrı-context bir işçiye gider. "< 3 dosya okuma / < 5 satır düzenleme = direkt yap; aksi = dispatch" eşiği.
- **Taşıma maliyeti:** Doküman (PROJECT_RULES şablonu) + `bootstrap-project` zaten ADIM 1'de öğretiyor. Ek maliyet yok.
- **Öncelik:** **S**

### 1.2 — writer ≠ reviewer (Council pattern)
- **RIMA'daki kanıt:** `feedback_review_roster_autonomous.md` — otonom run'larda yazanın kendi işini onaylaması yasak. Council = rima-qc (kural-vs-kod) + ax Gemini 3.1 Pro (bağımsız correctness) + ax Opus 4.6 (kritik) → orchestrator sentez → PASS/FAIL. Demo-tools Faz 3 review bu rosterle 0 blocker geçti.
- **Genel form:** Her üretim çıktısı, üreten model DIŞINDA en az bir bağımsız tanık tarafından gözden geçirilir. Kritik kararlarda 2-3 bağımsız model + bir sentezleyici. "Blind-commit riskini düşürür."
- **Taşıma maliyeti:** Global `/council` skill zaten var (cx ‖ ax-pro ‖ ax-flash ‖ opus-agent). Sıfır taşıma — config ile her projede çalışır.
- **Öncelik:** **S**

### 1.3 — Dispatch Routing Matrisi (hangi iş hangi modele)
- **RIMA'daki kanıt:** PROJECT_RULES "Agent Routing (S61)" + `reference_dispatch_skills.md`. Net matris: kod/Unity/mekanik=cx (Codex), hızlı bilgi/lean=ax Flash, derin/mimari/vision=ax Pro, kritik review=ax Opus, analiz=Sonnet subagent. `/cx /ax_flash /ax_pro /ax_opus` skill'leri swap+restore+CWD+timeout'u kapsülledi.
- **Genel form:** İş türü → model eşlemesi açık bir tabloda kilitlenir; her iş türünün varsayılan bir işçisi vardır. Kırılgan dispatch pattern'leri (model swap, CWD anchor, timeout) skill içine gömülür — orchestrator ham komut yazmaz.
- **Taşıma maliyeti:** Dispatch skill'leri zaten **GLOBAL + dinamik kök** (CWD'den `.claude` arar → açık projeye anchor). Sıfır taşıma. Routing tablosu bootstrap-project ADIM 1'de.
- **Öncelik:** **S**

### 1.4 — Tek-Instance Serileştirme Kuralı
- **RIMA'daki kanıt:** `reference_dispatch_skills.md` — "ax SERIALIZATION (tek ax/agy süreç)" çünkü paylaşılan `settings.json` model-swap çakışıyor; Unity-süren task'ler tek socket yüzünden seri. cx tek-instance.
- **Genel form:** Paylaşılan tek bir state'i (config dosyası, editor socket, auth) süren araçlar aynı anda tek instance çalıştırılır; paralellik ancak farklı state izole edilebiliyorsa mümkün. Skill içinde lock/try-finally restore.
- **Taşıma maliyeti:** Skill'lere gömülü (try/finally restore). Doküman notu yeter.
- **Öncelik:** **A**

---

## KATEGORİ 2 — Dosya / Süreç Hijyeni

### 2.1 — STAGING: LIVE vs _process Ayrımı
- **RIMA'daki kanıt:** `project_staging_process_convention.md` — STAGING'de 884 dosya birikmişti, 337'si süreç artifact'iydi. Kök neden: council/dispatch/done/review dosyalarının üst-seviyeye yazılması → LIVE kararlar bulunamıyordu, nlm-sync çöp tarıyordu. ÇÖZÜM: üst-seviye=SADECE LIVE (DECISION/PLAN/SPEC/aktif TASK), geri kalan `STAGING/_process/<YYYY-MM>/`.
- **Genel form:** Çalışma alanının kökü = sadece KALICI/LIVE belgeler. Geçici süreç çıktıları (görüş, taslak, ara rapor, tamamlanmış görev) tarihli alt klasöre iner. `_` prefix = otomatik sync/tarama bunları atlar. Git-tracked kalır (izlenebilirlik), asla silinmez sadece taşınır.
- **Taşıma maliyeti:** bootstrap-project ADIM 2 zaten `STAGING/` + `STAGING/_archive/` kuruyor; `_process/<YYYY-MM>/` + `archive_staging_process.ps1` (whitelist, idempotent) eklenebilir. Düşük.
- **Öncelik:** **A**

### 2.2 — One LIVE Version Rule
- **RIMA'daki kanıt:** PROJECT_RULES "Iteration Cleanup (S86)" — bir dosya v1→vN iterasyonunda sadece son LIVE versiyon ana dosyada kalır, eskiler `_archive/`'a taşınır. Birikme → drift kaynağı.
- **Genel form:** İterasyon yapılan herhangi bir artifact'in tek bir canlı kopyası vardır; eski versiyonlar arşive iner (silinmez). "Hangi versiyon geçerli?" sorusu hiçbir zaman belirsiz olmaz.
- **Taşıma maliyeti:** Doküman kuralı + `/lint` / `/phase-close` tetikleyicileri. Düşük.
- **Öncelik:** **A**

### 2.3 — CURRENT_STATUS Lean RESUME Bloğu
- **RIMA'daki kanıt:** `feedback_current_status_lean.md` — her session başa yeni blok eklendi, eskiler silinmedi → 1011 satır / 83K token / session başında context'in %30+'ı. ÇÖZÜM: max 1 aktif RESUME bloğu (~50 satır); yeni blok yazılırken eski SİLİNİR (git history'de kalır). RIMA'nın güncel CURRENT_STATUS.md bunu kanıtlıyor (~30 satır, tek blok, "Önceki bloklar git history'de").
- **Genel form:** "Nereden devam" durum dosyası tek bir güncel bloktur, append-only değil replace. Tarihçe versiyon kontrolünde; çalışan bellek yalın kalır. Index dosyaları da max 1 satır/girdi, detay ayrı dosyada.
- **Taşıma maliyeti:** bootstrap-project ADIM 2 zaten boş RESUME şablonu yazıyor. "replace, prepend değil" kuralını `/save-session` skill'ine + şablona ekle. Düşük.
- **Öncelik:** **S**

### 2.4 — Decision Dosyası Formatı (council → karar → kilit)
- **RIMA'daki kanıt:** `*_DECISION_<tarih>.md` deseni (INVERSION, MODULAR_ABILITY, DEMO_TOOLS_SCOPE, BACKDROP). Her biri: council görüşleri → tek sentezlenmiş karar → "KİLİT" / kullanıcı-override notu. Sonradan memory index sadece pointer tutar.
- **Genel form:** Önemli her karar tarihli bir karar dosyasında kilitlenir: alınan görüşler + nihai karar + gerekçe + (varsa) kullanıcı-override işareti. Memory/index sadece dosyaya pointer + 1 satır özet. Geri dönüldüğünde "neden böyle kararlaştık" kaybolmaz.
- **Taşıma maliyeti:** `/council` skill'i zaten karar dökümanı üretiyor (`STAGING/<KONU>_DECISION_<tarih>.md`). Sıfır taşıma. Format şablonu doküman.
- **Öncelik:** **A**

---

## KATEGORİ 3 — Kanıt Disiplini

### 3.1 — Data-Proof > İddia (kanıtsız "bitti" yok)
- **RIMA'daki kanıt:** bootstrap-project "Doğrulama kanıtsız 'bitti' DENMEZ". Demo-tools Faz 4 "runtime data-proof": prop place 2→sil→1, spawn 12→cap 10, stat ✓/badKey ✗ — sayısal assert'lerle. Global `verification-before-completion` skill.
- **Genel form:** Bir işin tamamlandığı iddiası, ancak çalıştırılmış bir doğrulama komutunun çıktısıyla birlikte yapılır. "Çalışmalı" değil "çalıştı, işte çıktı". Başarı iddiasından önce kanıt.
- **Taşıma maliyeti:** Global `verification-before-completion` skill mevcut. Sıfır taşıma. Dispatch header'a "kanıt raporla" satırı.
- **Öncelik:** **S**

### 3.2 — *ForValidation Hook Pattern (runtime tool'a test API'si göm)
- **RIMA'daki kanıt:** `reference_directormode_validation_api.md` — DirectorMode runtime tool'una `*ForValidation` public API gömüldü (`SpawnSelectedEnemyAtForValidation`, `DirectorSpawnedEnemyCountForValidation`, `TelemetryDpsForValidation`...). Sebep: ScreenSpaceOverlay UI screenshot'a girmiyor (`feedback_screenspaceoverlay`), UI-tıklaması kırılgan. `execute_code` ile reflection → deterministik sürüş, sayısal kanıt.
- **Genel form:** Bir runtime/etkileşimli araç (UI tool, agent, servis) yapıldığında, içine programatik bir doğrulama yüzeyi (test hook'ları, durum sorguları) gömülür. Böylece araç, kırılgan görsel/manuel doğrulama olmadan otomatik test edilir. "Görselle değil durum-objesiyle doğrula" ilkesinin somut hali.
- **Taşıma maliyeti:** Mimari pattern (kod-disiplini) — doküman olarak playbook'a girer, "etkileşimli tool yazarken validation hook ekle" kuralı. Skill'leştirilemez (proje-özgü kod), ama prensip taşınabilir.
- **Öncelik:** **A**

### 3.3 — BLOCKED if Unclear (tahmin etme)
- **RIMA'daki kanıt:** Karpathy kural 4 + dispatch header zorunlu satırı: "(4) BLOCKED if unclear." Muğlak kriter → orchestrator'a sor, sessizce partial implement etme. Modular philosophy'de de "demand evidence of GENUINELY equivalent duplication first" — varsayımla ilerleme cezalandırıldı.
- **Genel form:** İşçi, belirsizlik karşısında tahmin yürütüp yarım iş üretmek yerine açıkça BLOCKED bildirir ve sorar. Sessiz-partial = en pahalı hata türü (sonra fark edilir).
- **Taşıma maliyeti:** Dispatch header tek satır (ACTIVE RULES). Sıfır taşıma — bootstrap-project zaten zorunlu kılıyor.
- **Öncelik:** **S**

### 3.4 — Yanlış-Negatif Farkındalığı (ölçüm aracını doğrula)
- **RIMA'daki kanıt:** `feedback_screenspaceoverlay_not_in_screenshot.md` — "prompt çalışmıyor" yanlış teşhisi konuldu, gerçekte panel `active=True, alpha=1.0` idi; screenshot aracı overlay UI'yi yakalamıyordu. Ölçüm aracının kör noktası, yanlış sonuca yol açtı.
- **Genel form:** Bir doğrulama negatif verdiğinde, önce ölçüm aracının o şeyi görebildiğinden emin ol. "Araç göremiyor" ≠ "şey yok". Kanıt yolu, ölçtüğü şeye uygun seçilir (durum-objesi vs piksel).
- **Taşıma maliyeti:** Doküman (kanıt disiplini notu). Sıfır.
- **Öncelik:** **B**

---

## KATEGORİ 4 — Bilgi Yönetimi

### 4.1 — Drift Hiyerarşisi (canonical > memory > draft)
- **RIMA'daki kanıt:** PROJECT_RULES "Drift Hierarchy": (1) NLM canonical (notebook) > (2) Local memory (point-in-time, 14+ gün stale risk) > (3) Prompt iteration / STAGING draft (en oynak). Çakışmada yukarıdaki kazanır, alttaki revize edilir.
- **Genel form:** Bilginin tek bir canonical kaynağı vardır (tasarım KB). Yerel bellek = noktasal gözlem, bayatlayabilir. Taslaklar = en oynak. Çelişkide her zaman canonical kazanır; alt katman ona göre düzeltilir. Her memory dosyasına "X gün eski, doğrula" uyarısı.
- **Taşıma maliyeti:** Süreç + araç (NLM notebook + `/nlm` + `/nlm-sync` global skill'leri). Yeni projede notebook ID kurulumu gerekir (bootstrap ADIM 0 soruyor). Orta.
- **Öncelik:** **A**

### 4.2 — Index + Topic Memory Formatı
- **RIMA'daki kanıt:** `MEMORY.md` = kısa pointer index (girdi başına ~1 satır), detay ayrı `feedback_*` / `project_*` / `reference_*` topic dosyalarında. Frontmatter (name/description/node_type/type). Stale uyarısı otomatik.
- **Genel form:** Bellek iki katmanlı: yalın index (pointer + 1 satır özet) + derin topic dosyaları (frontmatter'lı). Index session başında ucuz okunur, detay sadece tetik eşleşince açılır.
- **Taşıma maliyeti:** Memory dizin yapısı + frontmatter şablonu. Doküman + bootstrap'a memory iskeleti eklenebilir. Düşük-orta.
- **Öncelik:** **A**

### 4.3 — nlm-sync Süreci (recursive, _-prefix hariç)
- **RIMA'daki kanıt:** `/nlm-sync` global skill — TASARIM/MEMORY/STAGING recursive taranır, `_*` ve EXPERIMENTS hariç. Kararlar session sonunda push. orphan cleanup-dry/cleanup.
- **Genel form:** Kalıcı belgeler periyodik olarak canonical KB'ye senkronlanır; geçici (`_`-prefix) olanlar otomatik dışlanır. Sync = phase-close/session-end ritüeli.
- **Taşıma maliyeti:** `/nlm-sync` global, parametrize (`NLM_NOTEBOOK_ID`, `NLM_REPO`). Sıfır taşıma — config ile çalışır.
- **Öncelik:** **B** (NLM kurulduktan sonra)

---

## KATEGORİ 5 — Karar Verme

### 5.1 — Council → Tek Karar → Kilit
- **RIMA'daki kanıt:** Inversion/Modular/Demo-scope kararları hep aynı akış: çoklu-model görüşü → orchestrator sentez → tek karar dosyası. Modular philosophy kararı 3-advisor council + Opus synthesis ile alındı.
- **Genel form:** Çok-sistemli/geri-dönüşü pahalı kararlar tek bir modelin yargısına bırakılmaz; bağımsız görüşler toplanır, bir sentezleyici tek karara indirir, karar kilitlenir. (Çoğu küçük karar buna gerek duymaz — eşik: 2+ sistemi kesen VE orchestrator tek başına çözemiyor.)
- **Taşıma maliyeti:** `/council` global skill. Sıfır.
- **Öncelik:** **A**

### 5.2 — Kullanıcı-Override Belgeleme
- **RIMA'daki kanıt:** DEMO_TOOLS kararında "kullanıcı council'ı override edip prop/light placement'ı demo'ya kattı" açıkça yazılı. Modular kararda "kullanıcı override'ın belgelenmesi" prensibi.
- **Genel form:** Kullanıcı bir council/öneriyi geçersiz kıldığında bu karar dosyasına işaretle yazılır — gelecekte "neden mantığa aykırı görünen bu seçim?" sorusu cevaplı kalır. Override gizlenmez, belgelenir.
- **Taşıma maliyeti:** Karar dosyası şablonuna "Kullanıcı override:" alanı. Doküman. Sıfır.
- **Öncelik:** **B**

---

## KATEGORİ 6 — Kod Disiplini

### 6.1 — Karpathy 4 Kuralı
- **RIMA'daki kanıt:** PROJECT_RULES "Universal Coding Principles (Karpathy 4)": (1) kod yazmadan önce düşün/varsayım listele (2) minimum kod, spekülasyon yok (3) cerrahi — sadece görev dosyaları (4) hedef-odaklı, BLOCKED if unclear. Her dispatch ilk satırı: `ACTIVE RULES: ...`.
- **Genel form:** Tüm kod-üreten işçiler dört evrensel ilkeye bağlıdır: düşün-önce, minimum-kod, cerrahi-değişiklik, hedef-odaklı-doğrulama. Bu satır her görev dağıtımının başına inline gömülür (işçi context'i taşımadığı için her seferinde hatırlatılır).
- **Taşıma maliyeti:** Tek satır dispatch header + PROJECT_RULES bölümü. bootstrap-project zaten içeriyor. Sıfır.
- **Öncelik:** **S**

### 6.2 — ACTIVE RULES / NLM ACCESS / Amaç Header Zorunluluğu
- **RIMA'daki kanıt:** PROJECT_RULES "Sub-Agent NLM Access (S91)" + dispatch skill'leri — her dispatch prompt'unun başında 3 zorunlu öğe: `ACTIVE RULES` satırı + `NLM ACCESS` bloğu + `# Amaç`. Yoksa subagent dosyaları kendi okur → context israfı + drift. `rima-conventions` skill bu eksikleri yakalıyor.
- **Genel form:** Her görev dağıtımı standart bir başlık taşır: aktif kurallar + bağlam-erişim yolu + net amaç. İşçi context taşımadığı için bu başlık "minimum çalışabilir brief"tir. Bir lint/convention aracı eksik başlığı yakalar.
- **Taşıma maliyeti:** Dispatch skill şablonlarına gömülü. Convention-checker skill (RIMA'nın `rima-conventions`'ı gibi) proje-lokal yazılır. Düşük.
- **Öncelik:** **S**

### 6.3 — Test/Build GATE
- **RIMA'daki kanıt:** PROJECT_RULES ax routing'de "S-boyutlu izole kod (dene→build+QC gate→FAIL'de cx'e eskalasyon)". Demo-tools her faz EditMode test (3/3) ile kapandı. VFX sprint 29/29 yeşil.
- **Genel form:** Hızlı/ucuz işçinin çıktısı bir build+test kapısından geçer; geçemezse daha güçlü işçiye eskale edilir. "Yeşil test = ilerleme izni." Faz kapanışı test geçişine bağlı.
- **Taşıma maliyeti:** Süreç kuralı + `/phase-close` (test çalıştırma adımı içerir). Global. Düşük.
- **Öncelik:** **A**

---

## KATEGORİ 7 — Build-Safety / Teknik Standartlar

### 7.1 — Resources-First + Editor Fallback (`#if UNITY_EDITOR` / AssetDatabase tuzağı)
- **RIMA'daki kanıt:** CURRENT_STATUS "YARIN FLAG (1): rift_crystal `#if UNITY_EDITOR` yüklü → standalone build'de palette boş (Resources'a taşı)". Bu session 2 kez yakalanan tuzak: editor-only API (AssetDatabase) runtime yolda kullanılınca standalone build'de sessizce boş döner.
- **Genel form:** Runtime'da çalışması gereken hiçbir kod, editor-only/build-time-only bir API'ye bağlı olamaz. Varlık yükleme runtime-güvenli yoldan (Resources/Addressables benzeri) yapılır; editor-only yol sadece opsiyonel fallback'tir. "Editörde çalıştı" ≠ "build'de çalışır" — derleme sembolüyle ayrılmış kod ayrı test edilir.
- **Genelleştirilmiş ilke (araç-bağımsız):** Geliştirme-ortamı API'leriyle dağıtım-ortamı API'lerini karıştırma; runtime yol her zaman dağıtımda mevcut olandan beslenir.
- **Taşıma maliyeti:** Doküman (teknik standart listesi) + convention-checker'a kural. Unity'ye özgü ama prensip her engine/runtime'a taşınır. Düşük.
- **Öncelik:** **A** (Unity projelerinde **S**)

### 7.2 — Editor State'i Diskten Düzenleme Yasağı
- **RIMA'daki kanıt:** bootstrap-project ADIM 2.6 "(Unity ise) sahne dosyalarını diskte düzenleme yasağı + runtime-build tercihi". UnityMCP server instructions de "sahne dosyalarını diskte editleme" uyarısı.
- **Genel form:** Karmaşık serileştirilmiş state (sahne, prefab, binary asset) elle/metin olarak düzenlenmez; aracın kendi API'si üzerinden mutasyon yapılır. Manuel düzenleme = sessiz bozulma riski.
- **Taşıma maliyeti:** Doküman + Unity-proje notu (bootstrap zaten yazıyor). Sıfır.
- **Öncelik:** **A** (Unity'de **S**)

---

## ⭐ İLK 5 — Yeni LaurethStudio Projesi, Gün-1 Sırası

Yeni bir proje klasörü açıldığında SIRAYLA kurulacak ilk 5 şey:

1. **`/bootstrap-project` çalıştır** — Orkestra rolleri + çalışma kuralları + dosya iskeleti (CLAUDE.md, `.claude/PROJECT_RULES.md`, CURRENT_STATUS.md, STAGING/) tek seferde kurulur. (Kategori 1.1, 1.3, 2.1, 2.3, 6.1, 6.2 hepsi bu adımda iner.) → *Bu skill, aşağıdaki 2-5'i içermeyen / zayıf kalan kısımları güncellenecek başlıca aday.*

2. **PROJECT_RULES'a kanıt disiplinini + dispatch header'ı kilitle** — Karpathy-4, `ACTIVE RULES / NLM ACCESS / # Amaç` zorunlu header, "kanıtsız bitti yok". (Kategori 3.1, 3.3, 6.1, 6.2.) bootstrap bunu yazıyor; doğrula ve eksikse tamamla.

3. **Routing + dispatch skill'lerini bağla** — `/cx /ax_flash /ax_pro /ax_opus /council` zaten GLOBAL + dinamik kök; yeni projede otomatik o köke anchor olduğunu **smoke-doğrula** (bir dummy dispatch → dosya doğru köke düşüyor mu). writer≠reviewer council rosterini onayla. (Kategori 1.2, 1.3, 5.1.)

4. **STAGING hijyenini + One-LIVE kuralını aktive et** — `STAGING/_process/<YYYY-MM>/`, `_archive/`, `archive_staging_process.ps1`; CURRENT_STATUS lean (tek RESUME, replace-not-prepend). Decision dosyası formatı şablonu. (Kategori 2.1, 2.2, 2.3, 2.4.)

5. **Bilgi katmanını kur** — Tasarım KB (NLM notebook ID) varsa `/nlm-sync` ile PROJECT_RULES'u push; yoksa "sonra" notu. İki-katmanlı memory (index + topic) iskeleti + drift hiyerarşisi kuralı. Convention-checker skill'ini (proje-lokal, RIMA'nın `rima-conventions`'ı gibi) tohumla. (Kategori 4.1, 4.2, 6.2.)

> **Not:** Build-safety standartları (Kategori 7) proje-tipine bağlı; Unity ise gün-1'de S, değilse engine'in eşdeğer "dev-API ≠ ship-API" tuzağı dökümante edilir.

---

## Taşıma Maliyeti Özeti (ne nereye)

| Pratik | Form | Hâli hazırda |
|---|---|---|
| Orchestrator-bulk-yapmaz, Routing, writer≠reviewer, Council, Karpathy-4, dispatch header | bootstrap-project + GLOBAL skill | **VAR** — config ile çalışır |
| `/cx /ax_flash /ax_pro /ax_opus /council /nlm /nlm-sync /verification-before-completion` | GLOBAL skill | **VAR** — dinamik kök |
| STAGING _process, One-LIVE, lean RESUME, decision format | Doküman + ufak script (`archive_staging_process.ps1`) | Kısmen — bootstrap'a eklenecek |
| *ForValidation hook, Resources-first, editor-edit yasağı | Teknik-standart dokümanı + convention-checker kuralı | Prensip taşınır; kod proje-özgü |
| Drift hiyerarşisi, index+topic memory | Süreç + memory iskeleti | NLM kurulumuna bağlı |
| Convention-checker (rima-conventions benzeri) | PROJE-LOKAL skill | Her projede yeniden tohumlanır |

**Sonuç:** Taşınabilir düzenin ~%70'i zaten GLOBAL skill/komut olarak mevcut ve `bootstrap-project` ile iniyor. Eksik olanlar büyük ölçüde **doküman-kuralı** (STAGING _process konvansiyonu, One-LIVE, build-safety standartları, *ForValidation pattern'i) — bunlar `bootstrap-project` skill'ine birer paragraf + birer şablon olarak eklenince LaurethStudio playbook'u "gün-1 tek komut" haline gelir.
