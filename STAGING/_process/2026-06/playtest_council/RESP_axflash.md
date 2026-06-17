# RIMA Playtest Council — Advisor Report: axflash (Gemini 3.5 Flash)

---

## EKSEN A: Bug Fix & Endüstri Araştırması

### B1 🔴 Reward-flow stall (Kök Neden & Endüstriyel Mimari)

#### 1. Kök-Neden Hipotezi (RIMA)
* **Kullanıcı Failure Modu (Toplandı ama Skill verilmedi):** `RewardPickup.cs` içindeki `DraftThenOpenExit` coroutine'i tetiklenir. Eğer `DraftManager.Instance` null ise veya `EnsureDependencies()` sırasında `offerUI` ya da `offerGenerator` çözülemezse, `ShowDraft()` erken döner. Bu durumda `IsDraftActive` false kalır. Coroutine'deki `while (draft.IsDraftActive)` döngüsü anında sonlanır, çıkış kapıları açılır (`ActivateExitDoors`), ödül nesnesi yok edilir (`Destroy(gameObject)`) fakat oyuncuya hiçbir kart sunulmamış olur.
* **Canlı Bulgu Failure Modu (Cleared sonrası activeReward=null, timeScale=0.3 stuck):** `RoomRunDirector.cs:RoomClearSequence` içinde yavaşlatma efekti (`ClearSlowMoBlip`) başlatılır. Bu sırada ödül (`SpawnRewardPickup`) sahneye yerleştirilir. Ancak eğer ödül, şablon sınırları dışında (cliff boşluğu veya yürünemeyen donut deliği) spawn olursa, oyuncu tetiğe giremez ve `G` tuşuna basamaz. `RewardAutoCollectTimeoutSec = 0f` (otomatik toplama kapalı) olduğu için coroutine `while (!activeReward.WasCollected)` döngüsünde sonsuza dek takılı kalır. Coroutine sonlanamadığı için `finally` bloğundaki `RestoreGameplayTimeScale()` tetiklenemez; oyun $0.3$ timescale'de asılı kalır ve kapılar asla açılmaz.

#### 2. Endüstri Nasıl Çözüyor?
* **Hades (Supergiant Games) — Decoupled State & Anchor Points:** Hades'te oda temizlendiğinde kapıların açılması ve ödülün toplanması **birbirine bağımlı değildir.** Son düşman öldüğünde kapılar anında açılır ve ödül (Boon, Darkness vb.) önceden elle yerleştirilmiş güvenli spawn noktalarında (Spawn Anchor) belirir. Ödül alınmasa bile oyuncu odadan çıkabilir. Bu durum softlock ihtimalini sıfıra indirir.
* **Enter the Gungeon / Binding of Isaac — Grid Validation & Recovery:** Isaac ve Gungeon'da, sandık/ödül spawn edilmeden önce hedef koordinatın yürünebilir bir tile olup olmadığı "navigasyon gridi" üzerinden doğrulanır. Eğer hedef koordinat geçersizse (uçurum, çukur), en yakın geçerli tile'a "snap" edilir. Gungeon'da ödülün toplanması kapıları bloke etmez.
* **Dead Cells (Motion Twin) — Magnetic Recovery:** Eğer bir nesne ulaşılamaz bir platformda veya boşlukta kalırsa, oyun fizik motoru nesneyi en yakın geçerli platform yüzeyine kaydırır veya oyuncu yaklaştığında nesneyi mıknatıs (magnetize) etkisiyle oyuncuya çeker.

#### 3. Demo-Scope Cerrahi Çözüm (3 Gün)
* **[DEMO-KRİTİK] Decouple & Fallback Spawn:** Ödül spawn koordinatını belirlerken eğer geçerli hücre bulunamazsa, doğrudan **oyuncunun mevcut pozisyonunu (`player.position`)** spawn noktası olarak kullanın.
* **[DEMO-KRİTİK] Safety Timeout:** `RewardAutoCollectTimeoutSec` değerini `12.0f` saniyeye geri çekin. Eğer oyuncu ulaşılamazlık veya algılama hatası yüzünden ödülü alamazsa, sistem otomatik olarak ilk kartı envantere eklesin (`ForceCollect`) ve akışı çözsün.
* **[DEMO-KRİTİK] TimeScale Güvencesi:** `RoomClearSequence` içindeki `finally` bloğu haricinde, `ClearSlowMoBlip` coroutine'inin başına ve sonuna ek güvenlik olarak timescale sıfırlama komutları yerleştirin.

---

### B2 🟠 Wave Boyutu Çok Küçük (Wave-after-Wave Combat)

#### 1. Kök-Neden Hipotezi (RIMA)
`EncounterController.cs` üzerindeki dalga tanımları (`EncounterWaveSO`) oda zorluk katsayısı (`encounterDifficulty`) ile çarpılarak spawn edilir. Ancak varsayılan pilot datada dalga başına düşen mob bütçesi son derece düşüktür (dalga başına 1-2 mob) ve dalga sayısı 2 ile sınırlıdır.

#### 2. Endüstri Nasıl Çözüyor?
* **Hades — Spawn Director & Budget Escalation:** Hades, aktif dövüşü yöneten bir "Spawn Director" kullanır. Oda tipine göre (Combat, Elite, Mini-boss) bir bütçe (Point/Credit) belirlenir. Düşmanlar bu bütçe bitene kadar dalga dalga spawn edilir.
* **Cadence Pattern (Tetikleme Koşulu):** Bir sonraki dalgayı tetiklemek için mevcut dalgadaki tüm düşmanların ölmesini beklemek temposu düşürür. Hades, **aktif düşman sayısı toplam dalga boyutunun %20'sinin altına indiğinde** veya belirli bir süre geçtiğinde yeni dalgayı otomatik olarak çağırır. Bu, dövüşün sürekli yüksek tempoda kalmasını sağlar.

#### 3. Demo-Scope Cerrahi Çözüm (3 Gün)
* **[DEMO-KRİTİK] Dalga ve Düşman Artırımı:** Mevcut `EncounterBankSO` içindeki Combat odası bütçesini 2 katına çıkarın.
* **[POST-DEMO] Cadence Controller:** `EncounterController` içine `SpawnNextWaveWhenPercentRemaining` parametresi ekleyerek düşmanların %25'i kaldığında bir sonraki dalgayı tetikleyen mantığı entegre edin.

---

### B3 🟠 M-Overlay Bleed (Arayüz Sızıntısı)

#### 1. Kök-Neden Hipotezi (RIMA)
`RunMapOverlay.cs` haritayı Unity'nin eski `OnGUI()` (IMGUI) sistemiyle çizer. Bu sistem, UGUI canvas yapısından bağımsız olarak ekran kartı çizim döngüsünün en sonunda render edilir. `OnGUI` içindeki arka plan karartması (`new Color(0.03f, 0.02f, 0.06f, 0.88f)`) yarı saydam olduğundan, arkadaki UGUI tabanlı Draft kartları sızar. Ayrıca harita açıkken tuş girdileri bloke edilmediği için çakışma yaşanır.

#### 2. Endüstri Nasıl Çözüyor?
* **Modal Stack & Exclusive UI State (Hades / Dead Cells):** Endüstri standartlarında açık olan paneller bir "Modal Stack" (Yığın) içinde tutulur. En üstteki arayüz (örn. Map) aktifken, alt katmanlardaki raycast'ler ve girdi okumaları tamamen devre dışı bırakılır. Arka plan dim/scrim efekti opak bir maske ile tamamen kapatılır veya arkadaki dünya blurlanır.

#### 3. Demo-Scope Cerrahi Çözüm (3 Gün)
* **[DEMO-KRİTİK] Input Block & Opacity:** `RunMapOverlay.OnGUI` metodunun başına `if (UIManager.Instance != null && UIManager.Instance.IsAnyOverlayOpen) return;` kontrolü ekleyin. Böylece draft kartları ekrandayken haritanın açılması engellenir.
* **[DEMO-KRİTİK] Auto-Close:** Harita açıkken esc veya başka bir menü tetiklendiğinde `show = false` yapın. Çizilen arka plan renginin alpha değerini `0.88f` yerine `1.0f` (tam opak) yaparak arkadaki sızıntıyı görsel olarak tamamen kesin.

---

### B4 🟠 Background Void (Boşluk Hissiyatı)

#### 1. Kök-Neden Hipotezi (RIMA)
Arena cliff-adası siyah arka plan üzerinde yüzmektedir. Kamera takip sistemi (`CameraFollow.cs`) odanın dış sınırlarına kadar uzandığı için oyuncu kenarlara yaklaştığında kamera oda dışındaki düz siyah boşluğu kadraja alır.

#### 2. Endüstri Nasıl Çözüyor?
* **Risk of Rain 2 / Dead Cells — Layered Depth & Parallax:** 2D platformer ve 3/4 top-down oyunlarda uzay boşluğu hissi vermemek için arka plana çok katmanlı (layered) parallax scroll eden sis, toz bulutu, yıldız veya yıkık kale siluetleri yerleştirilir. Kamera sınırları (Camera Constraints), ekranın hiçbir zaman oynanabilir alan dışındaki boşluğu göstermeyeceği şekilde oda sınırlarından içeriye doğru "aspect ratio" kadar daraltılır.

#### 3. Demo-Scope Cerrahi Çözüm (3 Gün)
* **[DEMO-KRİTİK] Camera Clamp Refinement:** `CameraFollow.cs` içindeki sınırları (bounds) her yönden `1.5` birim içeri çekin. Kamera hiçbir zaman uçurum kenarından dışarıyı göstermesin. Arka plana düz siyah yerine koyu mor/mavi tonlarında statik bir "abyss" görseli yerleştirin.

---

### B5 🟡 UI/Görsel Sorunlar & B6 🟡 Instrumentation Gap

#### 1. Sorunlar & Kök-Nedenler
* **Agresif Kırmızı Vignette:** Oyuncunun canı düştüğünde tetiklenen vignette efekti çok yüksek opaklık ve genişliğe sahip.
* **Siyah Siluet Moblar:** Düşman sprite'ları üzerindeki gölge/ışıklandırma (URP 2D Light) eksik veya SpriteRenderer materyali "Unlit" yerine yanlış "Lit" ayarında kaldığı için karanlıkta siluete dönüşüyor.
* **Instrumentation (Debug Log) Eksikliği:** Play-mode testlerinde oyun içinde ne olduğunu (dalga tetiklendi, ödül spawn oldu vb.) izleyecek bir debug konsolu yok.

#### 2. Demo-Scope Cerrahi Çözüm (3 Gün)
* **[DEMO-KRİTİK] Vignette & Light Fix:** Düşük can vignette maksimum opaklığını `0.4f` ile sınırlayın. Düşman sprite materyallerini URP 2D Unlit/Sprite-Default olarak değiştirerek ışıklandırmadan bağımsız net okunmalarını sağlayın.
* **[POST-DEMO] Debug HUD Console:** Sunum sırasında jüri önünde olası bir hatayı anında loglamak için ekranın sol üst köşesine kapatılabilir basit bir UI Log paneli yerleştirin (ScrollRect + Text + `Application.logMessageReceived` dinleyicisi).

---
---

## EKSEN B: Gameplay Tasarımı

### 1. Hades-tarzı Wave Tasarımı
* **Oda Tiplerine Göre Dalga Planı (Demo):**
  * **Normal Combat:** 3 Wave. Dalga 1: 3-4 zayıf mob (FractureImp). Dalga 2: 2 FractureImp + 1 Ranged mob. Dalga 3: 4 FractureImp.
  * **Elite Combat:** 2 Wave. Dalga 1: 1 Mini-boss/Elite mob (yüksek HP/Zırh) + 2 Imp yardımı. Dalga 2: 4 Hızlı Imp.
* **Escalation Eğrisi:** Her oda geçildiğinde düşmanların hasar ve hız değerleri procedural olarak %5 artırılmalıdır.
* **Spawn Cadence:** Düşman spawn anında zemin üzerinde 0.8 saniyelik bir **"telegraph dairesi" (VFX spawn-portal/summoning circle)** belirmeli, ardından düşman fiziksel olarak sahneye girmelidir. Bu, ani spawn ölümlerini engeller (Industry Standard: Hades'teki kırmızı spawn halkaları).

### 2. Boss-Mob Tasarımı (Penitent Sovereign)
* **Ölçek (Scale Factor):** Standart düşman boyutunun **1.8×** katı olmalıdır. Hem tehditkâr durmalı hem de karmaşık dövüş anında oyuncu tarafından net okunabilmelidir.
* **Minimum Dövüş Hissiyatı (Sunum Gereksinimleri):**
  * **Telegraphing:** Boss büyük bir balta savurma (Cleave) yapmadan önce baltasının parlaması (VFX) ve yerde kırmızı bir koni (cone indicator) çizilmesi şarttır.
  * **Posture/Stagger Bar:** Boss'un HP barının altında gri bir "Duruş" (Posture) barı olmalıdır. Oyuncu LMB komboları ile vurdukça bu bar dolmalı, dolduğunda Boss 1.5 saniye sersemlemelidir (Stagger). Bu oyuncuya hasar penceresi açar.
  * **Aşama (Phase) Geçişi:** Boss canı %50'ye indiğinde 1 saniyelik bir roar (kükreme) VFX'i eşliğinde "Enraged" durumuna geçmeli, saldırı hızı %30 artmalıdır.

### 3. Mob Boyutları & Okunabilirlik
* **İdeal Oyuncu-Mob Oranı:** Top-down 3/4 ARPG'lerde oyuncu sprite boyutu referans alındığında, standart moblar oyuncunun **0.8×** katı (sürü psikolojisi yaratmak için ufak), bosslar ise **1.8× ila 2.2×** katı olmalıdır.
* **Siluet Sorunu Çözümü:** Piksel sanatının arka planla karışmaması için düşmanların etrafına **1 piksellik koyu/siyah outline** çizilmeli veya shader seviyesinde hafif bir rim-light (kenar ışıltısı) eklenmelidir.

---
---

## EKSEN C: Playtest Planı

### 1. Yapılandırılmış Playtest Checklist (Golden Path)

| Sistem / Akış | Ne Test Edilmeli? | PASS Kriteri |
| :--- | :--- | :--- |
| **Combat Feel** | Warblade LMB combo ve Q yeteneği (Iron Charge) | Kombonun son vuruşunda düşmanların geri itilmesi (knockback) ve hafif ekran sarsıntısı. |
| **Wave Pacing** | Dalgaların birbirini takip etme süresi | Önceki dalga bittiğinde maksimum 1.5 sn içinde yeni düşmanların spawn portallarının belirmesi. |
| **Reward Flow** | Son dalga temizliği -> Chest spawn -> G interact -> Skill Draft | Ödülün kesinlikle yürünebilir alanda spawn olması, timescale'in yavaşlaması, G ile toplanıp 3 kartlık seçimin hatasız açılması. |
| **Run-map Nav** | M tuşu ile harita açılıp/kapatılması | Haritanın arka plandaki tüm UI'ı opak şekilde örtmesi ve girdi sızıntısı yapmaması. |
| **Boss Fight** | Boss'un telegraph yetenekleri ve HP/Posture barı | Posture barı kırıldığında boss'un stagger animasyonuna girmesi, telegraph alanlarının yerde net görünmesi. |
| **Ölüm / Revive** | Can barı 0 olduğunda ölüm ekranı gelmesi | Death screen gelmesi, timescale=0 olması ve "Quick Reset" butonunun oyuncuyu ful canla güvenle oda 1'e döndürmesi. |

### 2. "İyi Oynanış" (Demo-Ready) Metrikleri
* **Hit-Stop (Juice):** Oyuncu kritik bir darbe vurduğunda veya darbe aldığında oyunun $0.05$ saniye boyunca duraklaması (Hit-stop). Bu, darbe hissiyatını fizikselleştirir (Örn: Dead Cells).
* **Ekran Sarsıntısı (Screen Shake):** Yeteneğe göre (örn. Earthsplitter) kameranın hafifçe sarsılması. Maksimum genlik $0.1$ birimi geçmemelidir.
* **Okunabilirlik (Readability):** Düşmanların saldırı hazırlığı (wind-up) animasyonları en az 12 frame olmalı ve oyuncunun reaksiyon vermesine imkan tanımalıdır.

### 3. Test Bölünmesi (Otomatik vs Manuel)

#### (a) Otomatik Testler (Benim `execute_code` ile doğrulayabileceklerim)
* **Dungeon Graph Doğrulaması:** DAG yapısının döngü içermediğinin ve demo rotasının (Combat->Combat->Merchant->Combat->Boss) her zaman 5 odalı kurulduğunun kod seviyesinde assert edilmesi.
* **Skill Database Sağlamlığı:** Tüm database'in taranıp `skillType` alanı null olan veya retired listesinde olup da kitlerde kullanılan skill olup olmadığının taranması.
* **Walkability Harita Uyumu:** Hücrelerin koordinat bazlı yürünebilirlik verileri ile oda sınırlarının koordinatlarının matematiksel olarak eşleştiğinin doğrulanması.

#### (b) Manuel Testler (Görsel ve Kullanıcı Gözlemi)
* **Sprite Sorting / Depth:** Oyuncu veya boss uçurum kenarında dururken kılıcın ya da sprite katmanlarının cliff arkasına düzgün sorting olup olmadığı (IsoSorter testleri).
* **HUD Vignette Rahatsızlığı:** Vignette renginin ve yoğunluğunun aksiyonu kapatıp kapatmadığının gözlemlenmesi.
* **G-Tuşu Algılama Hissiyatı:** Ödül sandığının yanına gelindiğinde "G ile Aç" promptunun çıkma ve kaybolma anlarının görsel akıcılığı.

---
## ÖNCELİK SIRALAMASI (3 GÜNLÜK DEMO PLANI)

### MUST-FIX (Demo-Kritik / Bugün Yapılması Şart)
1. **B1: Reward-flow stall çözümü** (Timeout safety-net eklenmesi ve spawn fallback'inin player pozisyonuna çekilmesi).
2. **B3: M-Overlay girdi engellemesi ve tam opak arka plan** (Bleed ve çakışma engelleme).
3. **B5: Vignette opaklık sınırlaması ve mob silüet ışık/shader düzeltmesi** (Görsel okunabilirlik).

### SHOULD-FIX (Yarın / Vakit Kalırsa)
1. **B2: Wave bütçesinin iki katına çıkarılması** (Dövüş temposunu artırma).
2. **B4: Kamera sınırlarının uçurum dışını göstermeyecek şekilde daraltılması** (Void görünümünü engelleme).

### POST-DEMO (19 Haziran Sonrası)
1. **B6: Oyun içi HUD Debug Konsolu overlay'i.**
2. **EncounterController için yüzdelik dalga tetikleme cadence mantığı.**
3. **Boss aşama geçiş animasyonları ve detaylı telegraph VFX'leri.**
