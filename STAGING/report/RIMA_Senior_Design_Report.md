# RIMA — Rift Avcıları · Senior Design Report

<!--
KTO Karatay Senior Design formatı. Bu markdown make_akademik_docx.py tarafından
DOCX'e dönüştürülür. Kapak, İçindekiler (TOC field), numaralı teal başlıklar ve
sayfa numarası docx üretici tarafından eklenir; bu dosya gövde içeriğini taşır.
Başlık konvansiyonu:  ## = Heading 1 (numaralı bölüm),  ### = Heading 2,  #### = Heading 3.
Figür satırı:  [Şekil N: açıklama | dosya.png]
-->

## 1 PROJE GENEL BAKIŞ

### 1.1 Proje Tanımı

RIMA (Rift Avcıları), tek bir geliştirici tarafından yapay zekâ destekli çok-ajanlı bir geliştirme süreci ve veri-güdümlü bir oyun mimarisi kullanılarak hayata geçirilen 2D izometrik bir aksiyon-roguelite oyunudur. Oyuncu, eşsiz güçlere ve birbirinden tamamen farklı kaynak ekonomilerine sahip savaşçı sınıflarından birini seçerek prosedürel olarak ilerleyen bir zindan koridorunda oda oda dövüşür; her geçişte yeni yetenekler edinerek daha güçlü bir "build" inşa eder. Başarısız bir run dahi sonraki oturumlar için kalıcı bir kazanım bırakır.

Projenin akademik tezi tek başına "bir oyun yapmak" değildir. RIMA, kasıtlı olarak bir **environment + dikey dilim (vertical slice) + yeniden-kullanılabilir oyun-içi geliştirici araç zinciri** olarak konumlanmıştır. Bu çerçevede değerlendirme ekseni içerik hacmine değil, sistem mimarisine ve mühendislik disiplinine ağırlık verir. Oyunun oynanabilir döngüsü tezin kanıtıdır; asıl katkı ise bu döngüyü mümkün kılan veri-güdümlü mimari, oyun-içi araç katmanı ve doğrulanabilir AI-destekli geliştirme sürecidir.

Projenin yanıtladığı temel soru şudur: tek kişilik bir ekip, ticari ölçekli örneklerin (Hades, Dead Cells, Slay the Spire) kapsamını birebir hedeflemeden, aynı türün temel döngülerini veri-güdümlü ve doğrulanabilir biçimde nasıl üretebilir?

### 1.2 Amaç ve Önem

Aksiyon-roguelite türü, savaş mekaniği, oda çeşitliliği, görsel dil, build derinliği ve meta-ilerleme ekonomisini bir arada gerektirir; bunların her biri kendi başına bir uzmanlık alanıdır. Ticari bir roguelite normalde programcı, seviye tasarımcısı, sanatçı, QA uzmanı ve sistem mimarından oluşan bir ekip ister. Tek kişilik bir projede bu rollerin tümü aynı kişiye düşer; eksilen yalnızca zaman değil, aynı zamanda perspektiftir: kendi yazdığı kodu kendi gözden geçirmek ve kendi tasarladığı seviyenin monotonluğunu kendi fark etmek giderek güçleşir.

Bu kaynak kısıtı karşısında iki yol vardır: kapsamı kesmek ya da mevcut kapasiteyi genişletecek bir iş akışı kurmak. RIMA ikinci yolu seçer ve bunu iki paralel mimari kararla yapar. Birincisi, içerik ile sistemi birbirinden ayıran veri-güdümlü mimaridir: her oda ayrı bir sahne değil, bir ScriptableObject veri dosyasıdır; her yetenek merkezi bir veritabanında tanımlanır. İkincisi, kod üretimi, tasarım danışmanlığı ve çıktı incelemesini farklı rollere dağıtan çok-ajanlı bir yazılım mühendisliği sürecidir. Projenin mühendislik önemi, büyük dil modellerini tekil bir "komut ver-çalıştır" aracı olarak değil; rol bazlı, doğrulama döngülü ve kalıcı bir bilgi tabanına sahip etmen-temelli (agentic) bir geliştirme sistemi olarak konumlandırmasında yatar.

### 1.3 Kullanılan Teknolojiler

Projenin teknoloji yığını, piksel-sanatı izometrik bir oyunun gereksinimleri ve tek-geliştirici üretkenliği gözetilerek seçilmiştir.

- **Oyun motoru:** Unity 6 — proje başladığı dönemde uzun dönemli destek (LTS) aşamasına yaklaşan, API kararlılığı yüksek bir sürüm; tek geliştirici için sürüm-kırılmalarıyla uğraşmak yerine üretkenliği korumak önceliklidir.
- **Render hattı:** Universal Render Pipeline (URP) 2D — pixel-perfect kamera, `Light2D` dinamik ışık sistemi ve izometrik `Tilemap` entegrasyonu için.
- **Programlama dili:** C# — Unity'nin birincil betik dili; bileşen-tabanlı (component-based) MonoBehaviour mimarisi.
- **Veri katmanı:** ScriptableObject — odalar, yetenekler, prop tanımları ve silah verisi için kod-dışı, sürümlenebilir veri varlıkları.
- **Giriş sistemi:** Unity Input System — WASD hareket, LMB/RMB saldırı ve Q/E/R/F yetenek tuşları.
- **Görsel üretim hattı:** PixelLab AI (karakter ve VFX sprite'ları, 8-yön), Imagen tabanlı `agy` boru hattı (ortam ve UI görselleri).
- **AI-destekli geliştirme katmanı:** Çok-ajanlı orkestrasyon (Codex/cx ve Gemini/ax dispatch), Model Context Protocol (MCP) üzerinden Unity Editor ve PixelLab sürümü, kalıcı bilgi tabanı için NotebookLM, kod-grafı analizi için graphify.

### 1.4 Proje Yapısı

Tüm oyun kodu `Assets/Scripts/` altında toplanmıştır: 625 C# dosyası, 37 alt-dizin. Klasör ağacı sistem sorumluluklarına göre ayrılmıştır; aşağıdaki tablo en büyük dizinleri dosya sayısıyla birlikte özetler:

| Dizin | .cs | İçerik |
|---|---|---|
| `MapDesigner/` | 98 | Oda sistemi, IsoRoomBuilder, RoomRunDirector, template/data |
| `Skills/` | 92 | Yetenek/kart sistemi, DraftManager, cross-class |
| `Systems/` | 52 | Çekirdek oyun sistemleri |
| `UI/` | 47 | BuildMode, DirectorMode, HUD, tooltip |
| `Combat/` | 45 | Dövüş, hasar, stat, juice |
| `Core/` | 34 | RewardPickup, çekirdek akış |
| `Environment/` | 32 | Çevre, ışık, prop |
| `Enemies/` | 32 | Düşman AI/spawn |
| `Runtime/` | 26 | Runtime orchestration |
| `Editor/` | 25 | Unity editör araçları (RoomJsonImporter vb.) |

Bu yapı, "içerik ile sistemin ayrılması" ilkesinin fiziksel yansımasıdır: oyun mekaniğini (`Skills/`, `Combat/`, `Enemies/`) tanımlayan kod ile içerik üreten araçları (`MapDesigner/`, `Editor/`) tanımlayan kod ayrı katmanlardadır. Geliştirme araçları, kod tabanının yaklaşık beşte birini oluşturur; bu, projenin "tooling environment" tezini kod organizasyonu düzeyinde de doğrular.

---

## 2 SİSTEM MİMARİSİ

### 2.1 Bileşen-Tabanlı ve Veri-Güdümlü Mimari

RIMA, Unity'nin bileşen-tabanlı (component-based) modelini veri-güdümlü (data-driven) bir tasarım disipliniyle birleştirir. Temel ilke şudur: davranış kodda, içerik veride yaşar. Bir oda nasıl inşa edileceğini bilen kod (`IsoRoomBuilder`) ile o odanın ne olduğunu tanımlayan veri (`RoomTemplateSO`) birbirinden ayrıdır. Aynı ayrım yetenekler (`SkillData` ↔ `SkillController`), prop'lar (`PropDefinitionSO` ↔ `BridsonPoissonAutoPlacer`) ve silahlar (`WeaponDatabaseSO` ↔ `HandAnchorAttach`) için de geçerlidir.

Bu ayrımın pratik sonucu, içerik ölçeklenmesinin maliyetini düşürmesidir: yeni bir oda eklemek yeni bir sahne kurmak değil, yeni bir veri varlığı oluşturmaktır; yeni bir yetenek eklemek veritabanına bir kayıt girmektir.

### 2.2 Sahne Akışı ve Manager Katmanı

Oyunun çalışma zamanı akışı birbirine bağlı birkaç sahne ve kalıcı bir yönetici katmanı üzerinden ilerler. Üst düzey akış şu biçimdedir:

```
MainMenu ──► Attunement Chamber (CharSelect) ──► _Arena (Run)
   ▲                                                  │
   │                                                  ▼
   └────────────── Victory / Death ◄── Boss ◄── Combat + Draft + Portal
```

`MainMenu` sahnesi oyunun giriş noktasıdır; "Oyna" seçeneği oyuncuyu yürünebilir karakter seçim mekânı olan Attunement Chamber'a taşır. Asıl run, tek bir kalıcı arena sahnesi olan `_Arena` üzerinde gerçekleşir: her oda geçişinde önceki oda temizlenir ve bir sonraki oda verisinden yeniden kurulur. Bu "tek sahne, akan içerik" (same-scene streaming) yaklaşımı, oda başına sahne yükleme maliyetini ortadan kaldırır.

Kalıcı sistem nesneleri (`Systems` GameObject'i) sahne geçişlerinde yaşamaya devam eder; bu nesne üzerindeki singleton yöneticiler HUD, ses, meta-ilerleme ve oda akışı koordinasyonunu yürütür. (Bu singleton yaşam-döngüsü tasarımının yanlış uygulanmasının yol açtığı bir hata ve çözümü Bölüm 11'de ele alınmaktadır.)

### 2.3 Veri → Sahne Boru Hattı

Mimarinin omurgası, veriden sahneye uzanan deterministik bir boru hattıdır:

```
RoomTemplateSO  ──►  IsoRoomBuilder  ──►  sahne oda (zemin+cliff+prop)
   (veri)              (inşa kodu)             │
                                                ▼
                                       RoomRunDirector
                                  (yaşam-döngüsü: build→cleared→reward→door)
                                                │
                                                ▼
                                       DungeonGraph.Generate(depthCount=5)
                                       (run-map: 8–11 düğüm, dallanan)
```

`RoomTemplateSO`, bir odanın salt-veri tanımıdır. Çalışma zamanında `IsoRoomBuilder` bu veriyi okur ve zemin karolarını, otomatik uçurum (cliff) sprite'larını ve bake edilmiş prop'ları sahneye yerleştirir. `RoomRunDirector` odanın yaşam döngüsünü (savaş → temizlik → ödül → portal) yönetir. Üst düzeyde `DungeonGraph`, run başında dallanan bir dungeon haritası üretir ve `RoomRunDirector`'a hangi odaların hangi sırayla kurulacağını bildirir. Bu üç katman birbirinden bağımsız değişebilir; bu da mimarinin hem okunabilirliğini hem genişletilebilirliğini artırır.

[Şekil 1: RIMA çalışan oyun ekranı — izometrik yüzen ada, HUD, oyuncu ve düşmanlar | fig_gameplay_hud.png]

---

## 3 VERİ MODELLERİ (ScriptableObject Tasarımı)

RIMA'da geleneksel bir ilişkisel veritabanı yoktur; içerik, Unity'nin ScriptableObject varlıkları olarak modellenir. Bu varlıklar projenin "veri katmanı"dır: kod-dışı, sürümlenebilir ve editör araçlarıyla düzenlenebilir. Aşağıda projenin temel veri modelleri alanlarıyla birlikte ele alınmaktadır.

### 3.1 RoomTemplateSO

Bir oyun odasının tek-gerçek-kaynak (canonical) tanımı. JSON, yalnızca dışa aktarım ve araç-entegrasyon formatıdır; ScriptableObject her zaman geçerli kaynaktır.

```
RoomTemplateSO
  string   roomId               // benzersiz oda kimliği
  RoomType roomType             // Combat / Elite / Reward / Boss / CharSelect
  int      width, height        // ızgara boyutları
  bool[]   walkableGrid         // hücre-başına yürünebilirlik
  Vector2Int playerSpawn        // güney giriş noktası (player_spawn_01)
  DoorSocket[] exitSockets      // door_NW_01 / door_N_01 / door_NE_01
  PropPlacement[] bakedProps    // önceden hesaplanmış prop konum+tip
  DifficultyTag difficulty      // zorluk etiketi
```

### 3.2 PropDefinitionSO

Bir dekor nesnesinin (sandık, taş, kemik, sütun kaidesi) görsel ve fiziksel tanımı. Bu varlık, prop'ların hem otomatik yerleşimini hem de oyun-içi Build Mode paletini besler.

```
PropDefinitionSO
  string   propId
  Sprite   sprite
  string   sortingLayerOverride // "Entities" (dik) / "Decals" (yer)
  bool     blocksWalkable       // collider üretimi tetikler mi
  Vector2  footprintSize        // taban-merkezli collider boyutu
  CompositionRole role          // clean-center / wall-band / edge-band
```

### 3.3 SkillData ve SkillDatabase

Tüm yetenekler merkezi bir `SkillDatabase`'de tutulur. Veritabanında toplam 111 skill kaydı vardır; bunların yaklaşık 67'si çalışır implementasyona sahiptir, kalanlar tasarım envanteri olarak beklemektedir.

```
SkillData
  string   skillId
  string   displayName
  ClassId  ownerClass           // veya neutral
  SkillTier tier                // Common/Rare/Epic/Mythic/Legendary
  bool     isImplemented        // GetPool filtresi bunu kullanır
  int      depthUnlock          // hangi odadan itibaren teklif edilir
  ChainWindowId[] chainHooks    // sinerji pencereleri
```

Kritik bir tasarım ayrımı `SkillDatabase.GetPool` yönteminde gizlidir: `!isImplemented` filtresi, implementasyonsuz bir skill'in hiçbir koşulda draft havuzuna girmemesini garanti eder. Codex ekranı tüm 111 kaydı tasarım envanteri olarak gösterirken, run içi draft yalnızca oynanabilir skill'leri çeker.

### 3.4 WeaponDatabaseSO ve BasicAttackProfile

Silah verisi, karakter sprite'ı ile silah sprite'ının nasıl birleştirileceğini ve temel saldırının nasıl davranacağını tanımlar.

```
WeaponDatabaseSO
  string   weaponId
  Sprite   weaponSprite
  Vector2  anchorOffset         // el-yuvası ofseti (HandAnchorAttach)
  Vector2  gripOffset           // tutuş hizalama
  bool     frontHandRender      // ön/arka el flip

BasicAttackProfile
  float    damage
  float    knockbackForce
  AttackTier tier               // light / heavy / execute (hit-pause eşlemesi)
```

Bu veri modelleri, projenin tüm içeriğinin kod değiştirmeden düzenlenebilir olmasını sağlar; yeni bir oda, prop, yetenek veya silah eklemek bir veri varlığı oluşturmaktan ibarettir.

---

## 4 ANA SİSTEMLER VE ÖZELLİKLER

### 4.1 Tam Oynanabilir Döngü

RIMA, oynanabilir tam döngüye sahip çalışan bir prototiptir. Oyuncu Ana Menü'den başlar, yürünebilir Attunement Chamber'da bir sınıf seçer, oda oda ilerleyen bir run oynar, her üç odada bir 3-kart skill draft ekranıyla karşılaşır, dallanan Rift portallarıyla rota kararları verir, boss karşılaşmasına ulaşır ve run sonunda kazandığı Shattered Echo ile bir sonraki sınıfın kilidini açabilir. Bu döngünün tüm halkaları çalışır ve birbirine bağlıdır.

**Attunement Chamber — yürünebilir karakter seçimi.** RIMA'nın karakter seçim ekranı statik bir menü değil, gerçek bir odadır. Oyuncu son oynadığı sınıfın bedeninde spawn olur ve WASD ile serbestçe dolaşır. Oda çevresindeki pedestal'larda sınıflar donmuş "echo"lar gibi bekler: açık sınıflar taş-grisi, kilitli olanlar opak siyah silüetlerdir. Bir silüete yaklaşıldığında sınıf paneli kayarak girer; oyuncu bir combat dummy üzerinde vuruş hissini deneyebilir. [G] tuşu karakteri o sınıfa dönüştürür; kuzey duvarındaki Rift kapısından çıkmak seçimi onaylar.

**Combat.** Oda aktif olduğunda düşman dalgaları sahneye girer. Oyuncu sınıfa özgü saldırı zinciri (LMB combo) ve yetenek seti (Q/E/R/F) ile dövüşür. Düşmanlar hasar aldıklarında geri savrulur; ağır saldırılar tam bir knockdown animasyonu başlatır — düşman yere düşer ve kalkış animasyonu bitene kadar i-frame (geçici dokunulmazlık) korumasına sahiptir. Son düşman da devrildiğinde oda "Cleared" durumuna geçer.

**3-Kart Skill Draft.** Her üç odada bir, oda ortasında bir ödül nesnesi belirir. [G] ile alındığında üç skill kartı açılır; her kart sınıfa ait ya da nötr bir pasif yetenek sunar ve tooltip mevcut build ile sinerji noktalarını gösterir. Oyuncu birini seçer ve yetenek kalıcı olarak run'a eklenir.

[Şekil 2: 3-kart skill draft ekranı ve ödül akışı | fig_draft_reward.png]

**Dallanan Rift Portalları.** Oda temizlenip ödül alındıktan sonra arka kenarda 1–3 arasında Rift portalı açılır (kod düzeyinde `door_NW_01 / door_N_01 / door_NE_01` soketleri). Her portalın simgesi hedef oda türünü işaret eder: olağan savaş, daha zorlu ama daha iyi ödüllü elite, ya da doğrudan ödül odası. Oyuncu hangi portaldan geçeceğine karar vererek run'un kısa vadeli akışını ve sonundaki build'i biçimlendirir.

**Boss Karşılaşması.** Run sonuna doğru graph'ta boss düğümüne ulaşıldığında boss arenasına giden portal açılır. Boss arenası boyut ve zorluk açısından standart odalardan ayrışır. Demo kapsamındaki boss, mevcut `hollow_hulk` sprite'ının büyütülmüş versiyonuyla temsil edilir ve telegraph sistemiyle altı saldırıya bağlanmıştır (bkz. §4.4). Tasarım vizyonundaki nihai boss — "The Architect" — zengin bir faz yapısıyla yol haritasındadır.

**Victory / Ölüm ve Meta-İlerleme.** Run sonunda oyuncuya kazandığı Shattered Echo miktarı gösterilir; hesap formülü `odaSayısı × 3 + öldürmeSayısı / 5` olup run başına 5–60 Echo ile sınırlıdır. Biriken Echo'lar bir sonraki oturumda kilitli sınıf silüetlerinden birini açmak için harcanır.

### 4.2 Build Sistemi: Skill Draft, Tier Kilitleri ve Sinerji

Skill'ler beş tier'a ayrılır ve draft sırasında görünme olasılıkları run derinliğine göre kilitlenir:

| Tier | Görünme Ağırlığı | Run Derinliği Kilidi |
|---|---|---|
| Common (Yaygın) | 55 | Kilitsiz, ilk odadan itibaren |
| Rare (Nadir) | 27 | Kilitsiz, ilk odadan itibaren |
| Epic | 12 | Oda 3 ve sonrası |
| Mythic | 5 | Oda 3+, yalnızca birincil sınıf |
| Legendary | 3 | Oda 7 ve sonrası |

Bu dağılım bilinçlidir: run'un ilk yarısında oyuncu temel kimliğini belirleyen yaygın/nadir skill'leri seçer; run ilerledikçe sinerji noktaları netleşir ve gerçek güç sıçramaları olan Epic+ tier'lar devreye girer. Legendary, build'in yönü belirginleştiğinde son çeyrekte ortaya çıkar. Skill bar slotları da bir güç eğrisi olarak doldurulur: açılış draft'ı Q slotuna yerleşir; sonraki oda-temizleme draft'ları sırasıyla E, R ve F slotlarını doldurur; boss sonrasında iki yedek slot (Z, X) açılır.

Build sisteminin ikinci katmanı **chain window** (zincir penceresi) mekanizmasıdır: beş farklı zincir penceresi tanımlıdır. Belirli skill'ler ard arda tetiklendiğinde birbirinin etkisini çarpar; draft ekranında bir kart aktif bir zincir penceresiyle etkileşiyorsa bu sinerji küçük bir görsel chip olarak gösterilir.

### 4.3 Sınıf Tasarımı: Warblade Örneği

Veri modeli on sınıfı destekler; demo'da Warblade ve Elementalist varsayılan açık ve uçtan uca oynanabilir, Ranger ile Shadowblade aynı pipeline üzerinden etkinleştirilebilir durumdadır. Her sınıf farklı bir kaynak mekanizmasına ve oynanış diline sahiptir.

Warblade, "yaklaş, sabitle, zırh kır, infaz et" mantığıyla çalışan ağır bir savaşçıdır. Kaynağı, yalnızca hasar vererek ve kitle kontrolü uygulayarak dolan, savaş dışında pasif eriyen bir Rage barıdır; bu, oyuncuyu sürekli temasa zorlar. Skill seti ("Iron Charge", "Gravity Cleave", "Iron Counter") savaşı düşmanı Broken veya Sundered durumuna yönlendirir; bu durumlardaki bir hedefe imza yetenek "Death Blow" uygulanabilir — Rage barını boşaltarak %400+ hasar veren bir infaz darbesi. Rage dolduğunda [V] ile "Bladestorm" ultimate devreye girer.

[Şekil 3: Warblade karakteri — omuz zırhı ve iki elli kılıç silueti | figures_v2/fig06_warblade.png]

### 4.4 Düşman AI, Dalga Sistemi ve Boss Telegraph

Düşman dalgaları `EncounterController` tarafından bağımsız yönetilir; tüm dalgalar bittiğinde `RoomRunDirector`'a "temizlendi" sinyali gönderilir. Bu ayrışma, düşman dalga tasarımının oda mimarisinden bağımsız güncellenmesini sağlar.

Düşman AI'sinin temelini `BaseMobBehavior` oluşturur. Bu sistem, bir combat-okunabilirlik vakasının da kaynağı olmuştur (ayrıntı Bölüm 9): `detectionRange` algılama menzili spawn mesafesinin altında kaldığında düşmanlar kalıcı Idle durumunda kalıyordu; kök neden veri-güdümlü doğrulamayla bulunup düzeltilmiştir.

Boss telegraph sistemi, ARPG konvansiyonuna uygun olarak saldırı öncesi tehlike alanlarını görselleştirir. Mevcut motor yeniden kullanılarak altı boss saldırısına bağlanmıştır: HolyLash (koni), FractureStrike (daire), ChainExplosion (gecikmeli halka), SovereignsWrath (dış halka + yeşil güvenli-bölge), FractureCharge (atılım çizgisi) ve ShackleThrow. Windup zamanlaması saldırı anıyla bit-bit senkronizedir; "yalan telegraph" (görselden farklı zamanda vuran saldırı) önlenmiştir. SovereignsWrath'ın yeşil güvenli-halkası, tehlike halkasından ayrı renkte çizilerek oyuncuya "nereye durmalı" bilgisini okunur biçimde verir.

### 4.5 Oda Akışı State Machine

Bir RIMA odasının yaşam döngüsü 7 durumlu bir durum makinesiyle yönetilir:

```
[Idle] ──► [Combat] ──(son düşman düştü)──► [Cleared]
                                                │
                          (oyuncu ödülü G ile aldı)
                                                ▼
                                         [RewardTaken]
                                                │ (Rift portalları açılır)
                                                ▼
                                          [DoorOpen]
                                                │ (oyuncu portala girer)
                                                ▼
                                          [Advancing] ──► [Idle]  (sonraki oda)
                                                      └──► [Victory] (son düğüm)
```

`RoomRunDirector` bu yedi durumu yönetir ve geçişleri kasıtlı olarak tek yönlü tutar: savaş bitmeden ödül alınamaz, ödül alınmadan portal açılmaz, portala girilmeden sonraki oda kurulmaz. Bu tasarım hem net bir ilerleme hissi sağlar hem de yarış koşullarını (race condition) önler. Ölüm, Victory dışındaki herhangi bir durumda gerçekleşebilir ve run'u Death Screen'e taşır.

### 4.6 Oyun Hissi (Game-Feel) Katmanı

Mekanik doğruluk oynanabilir bir prototip için yeterlidir; ancak bir aksiyon oyununun inandırıcı hissetmesi ek bir çalışma gerektirir. RIMA'da bu, ekran sarsıntısı, zaman dondurması ve ses geri bildirimini kapsayan bir game-feel katmanıyla sağlanır.

**Hit-pause ve ekran sarsıntısı.** Hafif vuruşlar 0.03 s, ağır vuruşlar 0.06 s, infaz darbesi 0.10 s süre boyunca zamanı dondurur; her tier'a eşleşen bir sarsıntı şiddeti tanımlıdır. Tüm efektler tek bir `FeelToggle` bayrağından kapatılabilir (erişilebilirlik).

**CombatJuice.** Hasar sayıları, hit-stop, ekran sarsıntısı ve kamera-punch bileşenlerinden oluşan `CombatJuice` katmanı `_Arena` sahnesine bağlanmış ve canlı doğrulanmıştır: vuruş → hasar sayısı, kill → freeze; `timeScale` değişimine bağışık, sahne-geçişinde sızıntısız ve Build Mode/Director geçişlerinde temiz. Bu katman kodda hazır olduğu hâlde sahneye bağlı değildi; keşfedilip aktive edilerek "az iş, çok his" örneği oluşturulmuştur.

**İnfaz dünya-prompt'u.** Broken veya Sundered durumundaki bir düşmanın iki birim içindeyken üzerinde altın renkli "[RMB] İnfaz" yazısı belirir. Renk seçimi kasıtlıdır: cyan, oyunun dünya dilinde Echo ve Rift enerjisine ayrıldığından infaz prompt'u bu alandan uzak tutulmuştur.

**Düşman okunabilirliği.** `EnemyReadable.shader`, herhangi bir renkte gerçek silüet outline üretir; ambient ışık 0.22'den 0.35'e çıkarılarak düşmanlar net biçimde öne çıkar, atmosfer korunur. Bu çalışmada gizli bir bug bulunmuştur: `EnsureVisibleSprite` her karede authored-material'i eziyordu (clobber), eski outline hiç render edilmiyordu; kök-nedene inilip düzeltildi.

**Ses.** İki katmanlı mimari: demo katmanında 18 CC0 lisanslı klip dokuz SFX kanalına bağlanmıştır (swing, hit, death, execute, oda-clear, draft, ambient, knockdown, UI); her kanal bağımsız volume kontrolüne sahiptir ve sahne-geçişinde ambient sızıntısını önleyen bir yaşam-döngüsü koruması vardır. Müzik yatağı olarak JaggedStone CC0 dungeon-ambience düşük volume (0.16, SFX-altı) ile döngüye alınmıştır. Prodüksiyon müziği ve sınıfa özgü SFX gelecek çalışma kapsamındadır.

**Skill bar sürükle-bırak yeniden sıralama.** Oyuncu bir slotu basılı tutup sürükleyerek iki slot'un skill'ini yer değiştirebilir; bu, `SkillController.slots[]` dizisine doğrudan yazılan kalıcı bir değişikliktir, görsel bir taşıma değil. LMB/RMB kimlik slotları sürükleme dışında tutulur.

**VFX-tabanlı skill sunumu.** Savaş okunabilirliği için karakter animasyon sprite'ları yerine VFX + kod-tween kombinasyonu kullanılır. Her skill bir çıkış hareketi (lunge/squash-stretch/recoil), mevcut VFX envanterinden bir efekt (`slash_arc`, `glacial_spike`, `frozen_orb`, `meteor`, 8-yön fireball) ve `Assets/Scripts/Combat/Juice/` sürücülerine bağlı bir impact reçetesiyle (hit-pause + sarsıntı + HitFlash) tanımlanır. Bu, sıfır yeni sprite üretimiyle skill eylemlerini okunur kılar; prodüksiyon animasyonu tamamlandığında reçeteler doğrudan o kaynağa geçebilir.

---

## 5 OYUN-İÇİ GELİŞTİRME ARAÇLARI (CENTERPIECE)

Projenin merkezî mühendislik katkısı, oyun çalışırken — Unity editörünü açıp kapatmadan — tasarım iterasyonu yapılmasını sağlayan oyun-içi araç katmanıdır. Bu araçlar graphify kod-grafı analizinde en bağlı (god-node) sınıflar arasındadır ve "bu proje bir tooling environment" tezinin somut kanıtıdır.

### 5.1 Build Mode (F2) — Edit-to-Play

Build Mode, oyun çalışırken `F2` tuşuyla açılan oyun-içi seviye editörüdür. Açıldığında oyun duraklatılır; geliştirici prop'ları yerleştirir, zemini düzenler, odayı kurar — sonra modu kapatıp **aynı odayı anında oynar**. Bu "edit-to-play" döngüsü, geleneksel "Unity'de düzenle → kaydet → Play'e geç" döngüsünü ortadan kaldırır.

`BuildModeController` (graphify'da en bağlı tool node'larından), tek-sahip bir `F2` registry'si üzerinden çalışan toggle'ı, üzerinde çalışılan `WorkingTemplate`'i ve edit↔play geçişini yönetir. Prop yerleştirme `BuildPlacementController` tarafından bir palet → hayalet (ghost) → tıkla akışıyla gerçekleştirilir. Palet veri-güdümlüdür: `BuildModeAssetCatalog`, `PropRegistrySO`'dan beslenir; A1 prop import çalışmasıyla palet 9'dan 19 prop'a çıkarılmıştır. Bu araç, Bölüm 2'de tanımlanan veri-güdümlü mimarinin doğrudan bir tüketicisidir: aynı `RoomTemplateSO` verisini hem oyun hem editör paylaşır, dolayısıyla değişiklikler anında yansır.

[Şekil 4: Build Mode (F2) — oyun-içi seviye editörü, prop paleti ve ızgara overlay'i | fig_buildmode_centerpiece.png]

### 5.2 Director Mode — Runtime UI Factory

Director Mode (`DirectorMode.cs`), sahne kurulumu gerektirmeden çalışma zamanında bir UI üreten geliştirici konsoludur. Açıldığında sekmeli bir IDE-dock düzeninde çalışır ve şu panelleri sunar: Stat slider'ları (oyuncu/düşman istatistiklerini canlı ayarlama), Spawn (düşman çağırma), Telemetry (CSV çıktısı), Prop, Map ve Free-cam. Bu oturumda Director Mode, eski "debug-overlay" görünümünden profesyonel bir dock layout'a (viewport ~%57, SDF font, ScrollRect'li kaydırılabilir skill listesi) taşınmıştır. Director Mode, kod-grafında en yüksek bağlantı derecesine sahip node'dur (bkz. Bölüm 10), çünkü oyunun hemen her sistemine canlı bir gözlem ve müdahale arayüzü sağlar.

[Şekil 5: Director Mode — runtime UI dock (Stat / Spawn / Telemetry panelleri) | fig_director_mode.png]

### 5.3 F1 Debug Paneli

`F1` tuşuyla açılan hafif bir geliştirme paneli; God Mode, Kill All ve oda-atlama (room-skip) gibi hızlı geliştirme komutları sunar. Build Mode ve Director Mode'dan farklı olarak F1, hızlı tek-seferlik müdahaleler için tasarlanmıştır.

Bu üç araç birlikte, "editörsüz geliştirme" iş akışını oluşturur: geliştirici çoğu tasarım iterasyonunu oyun çalışırken yapar, bu da test-değiştir-test döngüsünü belirgin biçimde kısaltır.

---

## 6 VERİ-GÜDÜMLÜ ODA SİSTEMİ VE MAJOR KOD ANALİZİ

### 6.1 Odanın Kimliği: Sahneden Veriye

Geleneksel yaklaşımda her oda ayrı bir sahne dosyasıdır; 30 oda için 30 sahne, 30 konfigürasyon noktası ve her güncelleme için 30 açma-kapama döngüsü demektir. Tek geliştirici için bu yük orantısızdır. RIMA'da her oda bir `RoomTemplateSO` veri yapısında tanımlanır; görsel yapı (zemin, kenar, uçurum, prop'lar) oyun başlayınca bu veriden çalışma zamanında inşa edilir. Bunu mümkün kılan `IsoRoomBuilder`, tek bir `_Arena` sahnesi üzerinde çalışır; her geçişte önceki oda temizlenir, yenisi sıfırdan kurulur.

### 6.2 İzometrik Yüzen Ada: Otomatik Uçurum Yerleşimi (Kod Analizi)

RIMA'nın görsel dili "yüzen izometrik ada" üzerine kuruludur: her oda, boşlukta asılı koyu granit bir platformdur. Bu görünüm birkaç katmanın doğru sırayla inşa edilmesini gerektirir; en zorlu katman uçurum (cliff) yerleşimidir ve tamamen otomatiktir.

Çalışma zamanında `IsoRoomBuilder`, zemin ızgarasını tarar ve her hücrenin SW (güneybatı) ile SE (güneydoğu) komşularının void olup olmadığını kontrol eder; güney cephesindeki hücrelere "içeri kıvrılma" (inward tuck) vektörüyle ön-yüz uçurum sprite'ı yerleştirir. Yön-bazlı komşu kontrolünün özü şu mantıktadır:

```
foreach (cell in floorCells):
    bool voidSW = IsVoid(cell + SW)
    bool voidSE = IsVoid(cell + SE)
    if (voidSW || voidSE):
        // güney cephesi → derinlik koruyan inward-tuck
        PlaceCliff(cell, tuck: true)
    else if (HasAnyVoidNeighbor(cell)):
        // diğer yönler → standart kenar uçurumu
        PlaceCliff(cell, tuck: false)
```

Bu yön-ayrımı kritiktir: erken sürümde algoritma yön bilgisinden yoksundu ve her void-komşu hücreye ayrım gözetmeksizin uçurum yerleştiriyordu; sonuç, güney cephesinde zemine taşan kümüksü sprite yığılmalarıydı (ayrıntı Bölüm 11, §11.2). Editör tarafında `CliffAutoPlacer` sekiz izometrik yön için daha kapsamlı bir directional mantık sunar ve şablonları editör zamanında hazırlar. Her iki sistemde de geliştirici tek bir uçurum karosunu elle yerleştirmez.

Adanın fiziksel sınırı üç boyutta zorlanır: **oda geometrisi** (statik sınır doğruluğu — donut odaların iç deliği için collider üretimi), **hareket güvenliği** (knockback itmesi, knockdown yayı ve elite-teleport hedeflerinin yürünebilir hücrelere clamp'lenmesi, diyagonal köşe-kesmenin engellenmesi) ve **doğrulama** (tünelleme analizi: kare başına kat edilen mesafe hücre boyutunun çok altındadır). Walkable test grupları tamamen yeşildir.

### 6.3 Otomatik Prop Yerleşimi

Her şablon bir prop listesi taşır. Konumlar elle değil, `BridsonPoissonAutoPlacer` editör aracıyla (`#if UNITY_EDITOR`) algoritmik üretilir ve `RoomTemplateSO`'ya bake edilir; çalışma zamanında `IsoRoomBuilder` bu önceden hesaplanmış konumları okur. Bridson-Poisson algoritması, yürünebilir hücreler üzerinde minimum örnekleme mesafesi `r` garantisiyle noktalar üretir; böylece prop'lar üst üste yığılmaz, organik dağılır. Bir kompozisyon-rol haritası ek kısıt getirir: oda merkezi "temiz merkez" olarak düşük yoğunlukta tutulur (dövüş alanı), kapı girişleri sıfır yoğunlukla korunur, kenar kuşakları dekorasyona ayrılır. Combat ve Elite oda tiplerinde prop'lar çalışma zamanında devre dışı bırakılarak savaş okunabilirliği artırılır.

### 6.4 Dış Kaynaklı İçerik ve JSON İçe Aktarma

26 oda şablonunun yaklaşık 11'i dahili tasarımdır; 15'i ChatGPT'den AI-destekli bir "seviye tasarımcısı" olarak alınmıştır. Süreç gözetimsiz bırakılmamıştır: geçerli bir tasarım, ASCII ızgara düzeni, kapı soketleri, zorluk etiketi ve prop listesi içeren belirli bir JSON şemasına uymalıdır; bu format doğrudan `RoomJsonImporter` aracına beslenebilir. Üretilen tasarımlar bir eleme sürecinden (çeşitlilik, zorluk dengesi, tematik uygunluk) geçirilmiş, seçilenler mevcut odalarla birleştirilerek 26 şablonluk havuz oluşturulmuştur.

| Oda Havuzu Özeti | Değer |
|---|---|
| Toplam oda şablonu | 26 |
| Dahili tasarım | ~11 |
| ChatGPT paketinden seçilen | 15 |
| QC uygulanan şablon | 26 |

### 6.5 Authored Portal Soketi Sistemi

Her `RoomTemplateSO`, oyuncunun giriş noktasını (`player_spawn_01`, yürünebilirlik doğrulamalı) ve arka kenarda üç çıkış soketi (`door_NW_01`, `door_N_01`, `door_NE_01`) tanımlar. Bu soketler validator kurallarına tabidir: her soket hücresi yürünebilir ve kuzey komşusu yürünemeyen kenar olmalı; güney koridoru (2 hücre) yürünebilir kalmalı; soketler birbirinden en az 3 hücre ayrı olmalıdır.

Portal açısı ile aktif çıkış slotu sayısı bağımsız kavramlardır. Görsel maliyeti sınırlamak için iki authored portal açısı kullanılır: N soketi için frontal kemer, NW için açılı kemer; NE, açılı kemerin çalışma zamanında yatay aynalanmasıyla (flipX) elde edilir. Çalışma zamanında `BuildExitDoors`, dungeon graph düğümünün çocuk dal sayısına bakar: tek çıkış için yalnızca N, iki çıkış için NW+NE (merkez boş, simetri korunur), üç çıkış için her üçü. Hiçbir şablon geçerli slot sayısını karşılayamazsa sistem sessizce başarısız olmaz: uyarı yazar, editör validasyonu düzeltmeyi zorlar ve merkez-anchor geri dönüş düzeneği fail-safe olarak devreye girer.

### 6.6 Geliştirme Araç Zinciri (Editör)

Oyun sistemleriyle paralel olarak bir editör araç zinciri tasarlanmıştır:

- **Map Designer:** Birleşik 8-sekmeli pencere (Rooms, Library, Floor, Cliff, Object, Portal, Light, Layers). Rooms sekmesi 6-modlu bir toolbar (Paint Walkable/Void, Set Entry/NW/N/NE) ve UI↔JSON çift-yönlü senkronizasyon içerir; her boya darbesi Unity Undo'ya entegredir. ScriptableObject canonical kaynaktır; JSON ~1 s debounce ile dışa aktarılır, diff-kontrolüyle gereksiz yazma önlenir.
- **Room Browser:** Tüm `RoomTemplateSO` dosyalarını listeler ve tek tıkla `_Arena`'da kurar; 26 şablonun tamamı bu araçla görsel doğrulanmıştır.
- **RoomJsonImporter:** JSON oda verisini şema-doğrulamasıyla `RoomTemplateSO`'ya dönüştürür.
- **RuntimeAssetRegistryBaker:** Zemin/uçurum/prop varlıklarının GUID, ad, etiket, katman ve tür meta verisini çalışma zamanına aktarır.
- **ScreenshotMode:** F12 ile açılan, 6 kamera preset'li, HUD'lu/HUD'suz, deterministik prop-seed'li sunum modu; aynı şablon + aynı seed her seferinde piksel-piksel aynı kompozisyonu üretir (imza-eşitlik testiyle doğrulanmıştır).

[Şekil 6: Map Designer 8-sekmeli pencere ve Room Browser ile _Arena'da kurulan oda | report_screenshots/11_map_designer.png]

---

## 7 GÖRSEL ÜRETİM HATTI

### 7.1 Sanat Yönü

Görsel tasarım, izometrik yüzen adalar ve onları çevreleyen mor void teması üzerine kuruludur. Zemin koyu slate/granit tonlarındadır; koyu zemin karakter ve düşmanları zemine yapıştıran bir silüet okunabilirliği sağlar. Cyan (#00FFCC) enerji çatlakları ve rün parıltıları zinde odaları, karar noktalarını ve aktif objeleri işaret eder; ancak ekran alanının yalnızca %5–8'ine hâkim olacak biçimde kullanılarak atmosferin baskın rengi korunur. Kahverengi tonlar bilinçli olarak dışlanmıştır. Bu dil, proje başında belgelenmiş bir "sanat yönü kilidi" olarak sabitlenmiştir.

### 7.2 Karakter Sprite Üretimi: 10 Sınıf × 8 Yön

Her sınıf sekiz yönde (K, KD, D, GD, G, GB, B, KB) idle sprite'a ihtiyaç duyar; toplam 80 sprite elle çizilemeyecek bir yüktür. Çözüm PixelLab AI'dir. Sekiz yönün tamamı bağımsız üretilmez: K, KD, D, GD, G beş yön AI ile üretilir; GB, B, KB ise yatay aynalama (flipX) ile türetilir. Bu, içerik maliyetini yaklaşık %37 düşürür ve aynalanan yönlerde stil tutarlılığını doğal olarak korur. Canvas boyutları sınıf silüetine göre değişir (120×120, 124×124 veya 128×128); görünür gövde her sınıfta ~64 piksel uzunluğundadır, kalan boşluk animasyon eylemleri için tampondur.

[Şekil 7: Oynanabilir sınıf dizilimi — 10 sınıfın idle_south sprite'ları | figures_v2/class_lineup_sheet.png]

### 7.3 Piksel-Art İçe Aktarma Disiplini

Grafik kalitesini korumak için katı bir içe aktarma sözleşmesi uygulanır:

- **Filtre modu:** Point (Nearest Neighbor) — enterpolasyonu engeller.
- **PPU (piksel başına birim):** 64 — tüm sprite'larda ortak, ölçekleme buglarını önler.
- **Sıkıştırma:** Yok — Unity'nin lossy sıkıştırması piksel kenarlarını bozar.
- **Ölçekleme:** Tam sayı çarpanı — kesirli ölçekleme bulanıklaştırır.

Bu sözleşme yalnızca kural değil, fiilen hata yakalayan bir mekanizmadır: bir skill ikonu partisi Bilinear filtre ile gelip gözle görülür bulanıklığa yol açtığında bir QC geçişinde tespit edilmiş ve etkilenen grup toplu olarak Point'e çevrilmiştir. Ayrıca her karakter sprite'ının pivotu ayak tabanına ayarlanır; proje başında 82 rotasyon sprite'ı varsayılan 0.5 (merkez) pivotla geldiğinden karakterler "havada asılı" görünüyordu. Her sprite'ın alpha kanalı taranarak ayak hizası ölçülmüş ve pivot oraya taşınmıştır. Oyuncu ve 16 mob sprite'ı tek bir "Entities" sorting layer'ına alınmış; `IsoSorter` Y-eksenine dayalı derinlik sıralamasını çalışma zamanında yönetir.

### 7.4 Silah Mount Sistemi

Karakter ve silah sprite'ları ayrı üretilir ve çalışma zamanında birleştirilir. `HandAnchorAttach`, `WeaponDatabaseSO`'daki `anchorOffset` ve `gripOffset` verisini kullanarak silahı el-yuvasına hizalar; ön/arka el flip mantığı yön değişiminde silahın doğru elde görünmesini sağlar. Bu oturumda Warblade silahının el hizası, kod değiştirilmeden yalnızca veri (anchorOffset/gripOffset) ayarıyla düzeltilmiştir — veri-güdümlü tasarımın silah katmanındaki bir örneği.

[Şekil 8: Silah mount — karakter + silah sprite birleşimi ve el-yuvası hizalama | fig_weapon_mount.png]

### 7.5 Ortam Asset'leri ve İki Araç Ayrımı

Karakter üretimi yalnızca PixelLab, ortam/UI üretimi yalnızca Imagen (`agy` boru hattı) ile yapılır; bu ayrım teknik değil estetiktir. Farklı motorlardan gelen sprite'lar stil uyumsuzluğuna yol açabileceğinden, oyuncunun sürekli baktığı karakterler tek kaynakta tutulur. Toplamda 115 PNG staging alanında birikmiş; bir bölümü doğrudan asset, bir bölümü stil/palet referansı olarak kullanılmıştır. Her iki araçtan gelen varlık, kullanılmadan önce görsel QC'den (sınır keskinliği, arka plan şeffaflığı, palet tutarlılığı, orantı) geçirilir.

### 7.6 Demo Düşman Kadrosu

Demo'da 12 düşman sprite'ı kullanılır; tümü ortak import sözleşmesini paylaşır (64×64, PPU 64, Point, sıkıştırma kapalı), chibi high-top-down (~65°) stil, 1 piksel outline, soğuk slate-gri + cyan-Rift palet. Kadro: fracture_imp, relic_caster, seam_crawler, plate_widow, hollow_arbitter, rift_gound, spire_choirling, shard_walker, penitent_bruiser, riftbound_augur, hollow_hulk (demo boss'u olarak da büyütülmüş kullanılır) ve rift_acolyte.

[Şekil 9: Act-1 düşman kadrosu kontak sheet'i — 12 sprite, Hollow Hulk boss olarak işaretli | figures_v2/mob_roster_sheet.png]

---

## 8 YAPAY ZEKÂ DESTEKLİ ÇOK-AJANLI GELİŞTİRME METODOLOJİSİ

### 8.1 Gerekçe: Kaynak Kısıtları

Bölüm 1'deki kaynak kısıtı — tek geliştiricinin programcı, tasarımcı, sanatçı ve QA rollerinin tamamını üstlenmesi — bu metodolojinin gerekçesidir. Süreç, bir dil modeline komut verip çıktısını doğrudan kabul etmekten farklıdır: farklı rollere sahip ajanların belirli kurallara göre etkileştiği, her kararın belgelendiği ve her çıktının bağımsız doğrulandığı bir yazılım mühendisliği süreci tasarlanmış ve işletilmiştir. Ajanlar araçtır; süreci tasarlayan ve yöneten geliştiricidir. Yaklaşım, sektörde hızla olgunlaşan agentic-AI pratiklerini akademik bir prototip üretimine uyarlama denemesidir.

### 8.2 Sanal Ekip Yapısı

Dört temel rol tanımlanmış ve farklı araçlarla doldurulmuştur:

| Rol | Araç / Model | Ne Yapar |
|---|---|---|
| Orkestratör | Claude (bu proje) | Görev dağıtımı, sıralama, sentez, nihai karar |
| Yazılımcı Ajan | Codex / cx dispatch | Kod yazma, Unity değişiklikleri, mekanik batch iş |
| Danışman Konsey | Gemini Pro/Flash, Opus | Tasarım kararları, mimari değerlendirme, risk analizi |
| İnceleyici (Reviewer) | Yazar'dan farklı ajan | Çıktı kalite kontrolü, hata yakalama |
| Bilgi Tabanı | NotebookLM (MCP üzerinden) | Tasarım kararlarının kalıcı, sorgulanabilir arşivi |
| Ortam Sürücüsü | Unity MCP, PixelLab MCP | Ajanların Unity editörünü ve görsel hatları doğrudan sürmesi |

Sürecin temeli yazar-denetçi ayrılığıdır: **bir ajandan çıkan iş, o ajanın kendisi tarafından onaylanamaz.** Bilgi tabanı rolündeki NotebookLM, projenin belleği olarak işlev görmüş; her ajan büyük bağlam okumak yerine bu tabanı MCP üzerinden sorgulamıştır; bu hem token tüketimini azaltmış hem de kararların oturumlar arası tutarlılığını sağlamıştır. MCP ayrıca ajanların Unity Editor üzerinde doğrudan işlem yapmasını (sahne nesnesi oluşturma, SO güncelleme, play-mode kontrolü) ve PixelLab'de sprite işi tetiklemesini mümkün kılmıştır.

### 8.3 Süreç Kuralları

Sürecin en özgün yönü, ajanların yeteneklerinden çok onları yönetmek için geliştirilen kurallardır; bu kurallar denemeler ve gözlenen hatalar üzerinden belgelenip zorunlu kılınmıştır:

- **Görev dosyaları:** Her iş bir görev belgesiyle başlar (ne yapılacak, hangi dosyalara dokunulacak, neye dokunulmayacak, başarı kriteri). Ajan ya kriteri karşılar ya "BLOCKED" yazar; sessizce yarım iş kabul edilmez.
- **Karar dökümanları:** Büyük her mimari/tasarım kararı `STAGING/` altına alternatifleri ve gerekçeleriyle belgelenir; 100'ü aşkın karar belgesi yazılmıştır.
- **Cross-review zorunluluğu:** Yazar ve reviewer aynı ajan olamaz. Örneğin Codex'in yazdığı knockdown sistemi Opus tabanlı bir ajanla, ax kanalıyla yazılan bir bileşen Codex tarafından incelenmiştir.
- **Doğrulama kanıtı:** Bir iş "bitti" sayılmadan önce derleme hatası olmadığı, ilgili testlerin geçtiği ve oyun içinde doğrulandığı raporlanmalıdır.
- **Unity'ye tek ajan kuralı:** Aynı anda tek Unity-süren ajan; aksi hâlde sahne dosyaları çakışır.
- **MCP ile ortam erişimi:** Ajan, sahne nesnesi/SO/sprite işlemleri için ayrı bir insan adımını beklemez; toplu içerik görevlerinde süreç hızını belirgin artırır.

### 8.4 Vaka: 10-Task Otonom Gece Kuyruğu

Kuralların nasıl bir araya geldiğini en yoğun gece gösterir. 10 görev bir karar belgesiyle tanımlanıp sıralanmış, dört paralel şeride (Codex-A, Codex-B, ax, Sonnet-MCP) ayrılmış (her şerit aynı sahne/sisteme dokunmayacak biçimde), ~11 commit üretilmiş, 9 görev tamamlanmış, 2 görev tanımsız kaldığı için BLOCKED raporlanmıştır. İki önemli hata review aşamasında yakalanmıştır: knockdown sistemindeki i-frame immunity sızıntısı (`OnDisable` temizleme eksikliği) ve çifte resistance uygulaması. Ayrıca bir ajanın sahne-restore işlemi başka bir ajanın değişikliğini silmiş; bu olay "tek-Unity-ajan" kuralının somut gerekçesi olmuştur.

### 8.5 Metodolojinin Değerlendirilmesi ve Sınırları

| İnceleme vakası | Bulgu | Kritik örnek | Sonuç |
|---|---|---|---|
| Knockdown sistemi (mimari review) | 2 majör | i-frame sızıntısı; çifte resistance | birleştirme öncesi düzeltildi |
| Oda görsel QC (26 şablon) | 2 FAIL + 9 şüpheli | ada dışına taşan prop'lar | sistemik fix + yeniden denetim |
| UI-JSON editörü (statik review) | 3 minör | satır sonu; Undo birleştirme; prop korunumu | mikro-fix, 65/65 test |
| Oyun hissi + SFX (ilk teslim FAIL) | 9 bulgu | giriş tamponu regresyonu; ambient sızıntısı | 9/9 düzeltildi, ikinci tur geçti |

**Test bağımsızlığı:** "Kodu yazan AI testleri de yazıyorsa bağımsızlık nasıl sağlanır?" sorusuna yanıt şudur: kabul kriterleri, test mantığı ve sınır durumları geliştirici tarafından tanımlanır; ajanlar yalnızca bu insan-tanımlı mantığı koda döker. Görsel QC süreci (Bölüm 9) otomatik testlerin kapsamadığı hataları yakalayan bağımsız bir doğrulama katmanıdır. **İnsan müdahalesi gerektiren alanlar:** tasarım zevki ve oyun hissi hiçbir zaman ajanlara devredilmemiştir; "bu oda sıkıcı mı", "bu knockdown ağır hissediyor mu", "bu güzel mi" kararları yalnızca gerçek oyun oturumundan sonra verilebilmiştir. **Ajan hataları ve korumalar:** sessiz başarısızlık (kota dolan bir Codex'in eski "done" dosyasını tekrar basması) en tehlikeli örüntüdür; bunun üzerine her DONE dosyasının zaman damgasını kontrol etme kuralı getirilmiştir.

Bu süreç doğru anlaşıldığında bir ayrım gerekir: ajanlar söyleneni yapan araçlardır; neyin, hangi sırayla, hangi kısıtlarla yapılacağını tasarlayan kişi geliştiricidir. Orchestrator/reviewer ayrımı, karar belgesi zorunluluğu, tek-Unity-ajan kuralı ve cross-review matrisi ajanlardan çıkmamıştır; bunlar bir mühendislik sürecinin tasarım kararlarıdır. Projenin katkısı yalnızca AI kullanımında değil, AI çıktısını denetlenebilir mühendislik adımlarına dönüştüren süreç tasarımındadır.

---

## 9 DOĞRULAMA: TEST VE KALİTE GÜVENCESİ

### 9.1 Tek Geliştiricinin Kalite Sorunu

Ekip büyüdükçe doğal dağılan kalite filtresi, tek-geliştiricili bir projede kendiliğinden ortaya çıkmaz. RIMA bu boşluğu üç katmanlı bir yaklaşımla kapatır: otomatik birim/entegrasyon testleri, sözleşme (contract) testleri ve sistematik görsel kalite güvencesi. AI'nin kod ürettiği bir bağlamda bu önem daha da artar: otomatik üretilen bir kod parçası derleniyor olabilir ama sistemin geri kalanıyla beklendiği gibi konuşup konuşmadığı ayrıca doğrulanmalıdır.

### 9.2 Test Altyapısı

Test paketi Unity Test Runner üzerinde EditMode ve PlayMode testlerinden oluşur. EditMode testleri sahne çalıştırılmadan senkron koşar; PlayMode probe'ları gerçek sahne yüklemesini, bileşen başlatmayı ve çok-kareli asenkron etkileşimleri test eder. Kategoriler: Bootstrap/Wiring, Combat/Encounter, Room/MapDesigner, Props/Composition, RoomPainter/Brush, Movement ve UIFlow.

| Ölçüt | Değer |
|---|---|
| EditMode test dosyası | 89 |
| PlayMode test dosyası | 12 |
| Bağımsız sözleşme dosyası | 4 |
| EditMode test tanımı | 508 |
| PlayMode test tanımı | 41 |
| **Toplam test tanımı** | **549** |
| Son kayıtlı EditMode koşusu | **410 PASS / 0 FAIL / 1 inconclusive** |

Tek belirsiz (inconclusive) test, Unity editörü dışında erişilemeyen bir sahneleme bağımlılığını kapsar; işlevsel bir hata değildir. Envanter sayısı (549), kayıtlı koşu tamamlandıktan sonra eklenen test gruplarını (walkable, JSON round-trip, gate-slot, screenshot imza) içerdiğinden koşu toplamından büyüktür.

### 9.3 Görsel Oda Kalite Güvence Süreci

Otomatik testlerin yakalayamadığı bir hata sınıfı vardır: kod açısından geçerli ama sahnede görsel yanlış görünen durumlar. Bunun için programatik bir görsel QC süreci kurulmuştur: her `RoomTemplateSO`, `_Arena`'da `IsoRoomBuilder` ile yeniden inşa edilmiş; her inşadan sonra zemin karosu, uçurum sprite ve prop sayısı kaydedilmiş, editör görüntüsünden bir ekran görüntüsü alınmıştır. İlk geçiş **15 tamam / 9 şüpheli / 2 başarısız** sonucu vermiştir.

Kök neden analizi iki ayrı hata ortaya çıkardı. Birincisi 6 şablonu aynı şekilde etkileyen sistematik bir prop hatasıydı: Bridson-Poisson geçişi, sınır hücreleri için koordinatları zemin hücre kümesinin dışına taşıyor; bu geçersiz koordinatlar şablona bake edilince `IsoRoomBuilder` prop'ları ada dışında konumlandırıyordu (`combat_large_diamond_01`'de 31 prop'tan birkaçı boşlukta). İkincisi `combatlarge_twin_basins_01`'de iki havuzu birleştiren tek-hücrelik boşluğun uçurum çözücüyü yanıltması, sprite'ların zeminin ortasından geçmesiydi. Düzeltmede prop koordinatları bake öncesi zemin kümesine karşı doğrulanmaya başlanmış, etkilenen 6 şablon yeniden tohumlanmış, uçurum boşluğu kapatılmıştır. `Treasure_01` ayrıca 14 hücreden 42 hücreye büyütülmüştür. Yeniden doğrulama kritik ölçütü `propOutsideFloor` sayacının sıfır olmasıydı:

```
combat_large_diamond_01:   floor=212, builtProps=4, propOutsideFloor=0 → ok=True
combatlarge_twin_basins_01: floor=599, builtProps=1, propOutsideFloor=0 → ok=True
Treasure_01:               floor=42,  builtProps=4, propOutsideFloor=0 → ok=True
[RoomQCFix] PASS
```

Bu sürecin kalıcı çıktısı, gelecekteki doldurucu geçişlerini de engelleyecek bir editör/üretim-zamanı zemin doğrulama kuralının koda işlenmiş olmasıdır.

### 9.4 Üçlü Kalite Güvencesi

| Katman | Ne yakalar | Örnek |
|---|---|---|
| Otomatik test | mantık/sözleşme hataları | gate-slot kuralları, JSON round-trip |
| Görsel QC | yapısal geçerli ama görsel yanlış | ada dışına taşan prop |
| Bağımsız inceleme | çalışan ama yanlış entegre kod | i-frame sızıntısı, giriş tamponu regresyonu |

Otomatik testler hız ve tekrarlanabilirlik sağlar; PlayMode probe'lar sistem sınırlarındaki hataları (singleton yaşam-döngüsü, sahne-geçiş etkileri) ortaya çıkarır; görsel QC veri-güdümlü içerik üretiminin görsel sapmalarını programatik raporla kanıtlanabilir kılar. Üç katmanın birlikte çalışması, tek-geliştirici + AI üretimi kombinasyonunda kalite güvencesine somut bir model sunar.

---

## 10 GRAPHIFY KOD-GRAFI AUDİT

### 10.1 Yöntem

Projenin "environment + tooling" tezini öznel iddiadan çıkarıp ölçülebilir bir kanıta dönüştürmek için tüm kod tabanı graphify ile bir bilgi grafına dönüştürülmüştür. Araç, C# kaynak dosyalarını parse ederek sınıflar ve bağımlılıklar arası bir grafik kurar, ardından bu grafiği topluluk-tespiti (community detection) ile kümelere ayırır. RIMA kod tabanı için sonuç: **6925 node / 118 community**.

### 10.2 God-Node Analizi: Tooling Tezinin Sayısal Kanıtı

Bir grafikte en yüksek bağlantı derecesine sahip node'lar ("god-node"), sistemin etrafında döndüğü merkezlerdir. RIMA'da en bağlı 10 node analiz edildiğinde çarpıcı bir sonuç ortaya çıkar: bunların **6'sı editör/araç (tooling) sınıfıdır**, yalnızca 4'ü oyun/runtime sınıfıdır. Bu, "RIMA aslında bir geliştirme-ortamıdır, üzerine oturan oyun ise bu ortamın bir tüketicisidir" tezinin doğrudan, sayısal kanıtıdır.

En bağlı node'lar:

| Node | Tür | Rol |
|---|---|---|
| DirectorMode (168) | Tooling | Runtime UI factory — en bağlı node |
| InPlayMapPaintOverlay (93) | Tooling | Oyun-içi harita boyama overlay'i |
| RoomPainterWindow (88) | Tooling | Editör oda boyama penceresi |
| ChamberSelectBootstrap (84) | Runtime | Karakter seçim bootstrap |
| LargeDungeonMapPainterBase (78) | Tooling | Harita boyama tabanı |
| CharacterSelectScreen (75) | Runtime | Karakter seçim ekranı |
| MinimalTilePainter (70) | Tooling | Karo boyama aracı |
| RuntimeRoomManager (69) | Runtime | Çalışma-zamanı oda yöneticisi |
| BuildPlacementController (66) | Tooling | Build Mode prop yerleştirme |
| RoomRunDirector (65) | Runtime | Oda yaşam-döngüsü |

[Şekil 10: Graphify — en bağlı 10 node; turuncu = editör araçları (6), mavi = oyun/runtime (4) | fig_graphify_godnodes.png]

### 10.3 Tam Graf Görünümü

Tam kod grafiği, 118 topluluğun renk-kodlu kümeler halinde dağılımını gösterir. Tooling katmanı (Map Designer, Build Mode, Director Mode, painter araçları) grafiğin merkezinde yoğun bir küme oluştururken, oyun sistemleri (Skills, Combat, Enemies) çevresel kümeler halinde bu merkeze bağlanır. Bu topoloji, kod organizasyonu düzeyinde (Bölüm 1.4) gözlenen "araçlar kod tabanının beşte biri" gözlemiyle tutarlıdır ve mimari iddiayı bağımsız bir metrikle destekler.

[Şekil 11: Graphify tam kod grafiği — 6925 node, 118 community | fig_graphify_full.png]

---

## 11 KARŞILAŞILAN ZORLUKLAR VE ÇÖZÜMLER

### 11.1 Donanım-Sürücü Uyumsuzluğu: RTX 5080 ve Unity 6 D3D12 Çökmesi

**Sorun.** Geliştirme ortamı RTX 5080 ekran kartına taşındıktan sonra play-mode başlar başlamaz oyun kendiliğinden kapanıyor, çökme hata-ayıklama verisi bırakmadan gerçekleşiyordu.

**Teşhis.** Crash yığını `CheckDeviceStatus → D3D12CommandList::PrepareExecute → GfxTaskExecutorD3D12` üzerinden geçiyordu — bir TDR (Timeout Detection and Recovery) durumu. Kök neden, Unity 6'nın varsayılan Direct3D 12 arka ucunun çok yeni RTX 5080 sürücüsüyle henüz tam uyumlu olmamasıydı.

**Çözüm.** `PlayerSettings` grafik API sıralaması değiştirilerek Direct3D 11 birincil konuma taşındı; `SystemInfo.graphicsDeviceType` artık `Direct3D11` döndürüyor, çökme tamamen ortadan kalktı. Ders: geliştirme ortamı değiştiğinde ilk adım varsayılan ayarların uygunluğunu sorgulamaktır.

### 11.2 İzometrik Uçurum Taşması: Görsel Hatada Kök Neden Aramak

**Sorun.** Ada kenarlarına otomatik yerleştirilen uçurum sprite'ları zemine taşıyor; özellikle güney/güneydoğu köşelerinde perde gibi yoğun yığılmalar oluşuyordu.

**Teşhis.** İlk müdahaleler (sprite boyutu küçültme, z-sıralama) yüzeysel yamalardı ve kalıcı çözmedi. Gerçek kaynak, uçurum algoritmasının yön bilgisinden yoksun olması, her void-komşu hücreye ayrım gözetmeksizin sprite yerleştirmesiydi; güneyde (SW/SE) void olan hücreler için "içeri kıvrılma" gerekirken bunlar diğer yönlerle aynı işleniyordu.

**Çözüm.** Sistem yön-bazlı komşu kontrolüne göre yeniden yazıldı (bkz. §6.2 kod listingi); SW/SE void hücreleri "inward tuck" vektörüyle yerleştirildi. Ders: görsel hatada yüzeysel yamalardan önce algoritmanın hangi koşulda tetiklendiği belirlenmelidir.

### 11.3 Singleton Guard'ının Sistemi Sessizce Kesmesi

**Sorun.** Sahne geçişlerinde oda temizleme akışı sessizce kopuyordu: düşmanlar ölünce ne ödül beliriyordu ne de portallar açılıyordu; hiç hata mesajı yoktu.

**Teşhis.** Merkezi `Systems` GameObject'i sahne yüklenirken yok ediliyordu. Üzerindeki bir singleton koruması, kopya tespitinde `Destroy(this)` yerine `Destroy(gameObject)` çağırıyor; böylece yalnızca fazla bileşen değil, tüm `Systems` nesnesi (oda geçiş koordinatörü dahil) siliniyordu.

**Çözüm.** `Destroy(gameObject)` → `Destroy(this)` ile yalnızca fazla bileşen yok edildi; nesnenin diğer bileşenleri sağlıklı kaldı ve akış düzeldi. Ders: paylaşılan bir GameObject üzerinde singleton uygulanırken hangi kapsamın (bileşen vs. nesne) hedeflendiği açıkça belirlenmelidir.

### 11.4 Combat-Okunabilirlik Vakası: "Yeşil-Assert ≠ Çalışıyor"

Bu vaka, projenin en güçlü mühendislik anlatılarından biridir ve "otomatik testlerin geçmesi, gerçekte çalışıyor olmak değildir" ilkesini somutlaştırır.

**Keşif.** Demo için alınan çok-state'li bir capture'da "combat" etiketli kareler incelendiğinde, bunların aslında draft veya death ekranı olduğu (KILLS sayacı 0) görüldü. Sentetik testler yeşil geçiyordu ama full-flow'u atlıyorlardı — oda-1 combat'ı gerçekte bozuktu, düşmanlar idle duruyordu.

**Kök neden (veri-güdümlü).** Tek-arena verify ile sorun izole edildi: `BaseMobBehavior.detectionRange = 8`, spawn mesafesi (9.12) küçük → düşmanlar kalıcı Idle'da kalıyordu (HalfThrall prefab'da değer 6). Token-gating sağlamdı; sorun yalnızca menzildeydi.

**Fix ve doğrulama.** `detectionRange` 8→12 yükseltildi, prefab güncellendi, savunmacı bir Player re-acquire eklendi (cerrahi). Gerçek full-flow (MainMenu → CharSelect → _Arena) ile doğrulandı: mob'lar gerçek (tagged) Warblade oyuncusuna chase → attack yapıyor, açılış dalgası kazanılabilir (kills 0→3), instant-death yok.

**Çok-katmanlı doğrulama.** Buna ek olarak bağımsız bir dış AI-review (ChatGPT kod + ekran görüntüsü), tek-arena verify'ın kaçırdığı derin riskleri işaret etti. **Verify-first** disipliniyle 5 P0 bulgudan **2'si reproduce etmedi** (boss-off-island = capture artefaktı; player-tag = zaten iyi → boş işten kaçınıldı), **3'ü gerçekti ve düzeltildi** (boss re-acquire idle; death-restart token-death; Penitent %14.2 açılış payı, combo 85→42). Bu zincir — otomatik test → dış AI-review → runtime-reproduce + yanlış-pozitif eleme — olgun bir mühendislik sürecini örnekler.

### 11.5 AI Ajan Güvenilirliği: Sessiz Başarısızlık ve Çakışan Değişiklikler

**Sorun.** Birden fazla AI ajanı paralel/ardışık çalışırken iki güvenilirlik sorunu çıktı: (1) sessiz başarısızlık — kota sınırına takılan bir ajan hata fırlatmak yerine eski "done" bildirimini bırakıp çıkıyordu; (2) geri alma çakışması — bir ajan sahneyi kendi referansına geri yüklerken başka bir ajanın değişikliğini siliyordu.

**Çözüm.** Her gönderim sonrası "done" dosyasının zaman damgası görev başlangıcıyla karşılaştırıldı; sahne içeren işler öncesi değişiklikler commit'lendi; tek-Unity-ajan kuralı benimsendi. Ders: AI-ajan pipeline'ında güven sözlü teminata değil doğrulama mimarisine dayanmalıdır.

---

## 12 SONUÇ VE GELECEK GELİŞTİRMELER

### 12.1 Ulaşılan Yer

Projenin başındaki soru — tek geliştirici kapsamlı bir aksiyon-roguelite'i nasıl yapabilir? — iki paralel yanıtla ele alındı: içerik ile sistemi ayıran veri-güdümlü mimari ve bunun üzerinde çalışan çok-ajanlı geliştirme süreci. Somut çıktılar:

- **26 oda şablonu** — 15'i dışarıdan temin, tamamı görsel QC'den geçirilmiş.
- **10 sınıflık veri altyapısı** — her biri farklı kaynak ekonomisi ve oynanış diliyle; 4 sınıfın çalışan controller'ı mevcut, demo'da Warblade ve Elementalist uçtan uca oynanabilir.
- **549 test tanımı envanteri** (508 EditMode + 41 PlayMode) — son kayıtlı EditMode koşusu 410 PASS / 0 FAIL / 1 inconclusive.
- **80 karakter sprite** — PixelLab, Point/PPU64 import sözleşmesiyle.
- **Oyun-içi araç katmanı** — Build Mode (F2), Director Mode, F1; 8-sekmeli Map Designer, Room Browser, RoomJsonImporter, ScreenshotMode.
- **Game-feel katmanı** — hit-pause tier'ları, CombatJuice, telegraph, FeelToggle, 18 CC0 SFX + 9 kanal.
- **Tam oynanabilir döngü** — MainMenu → Attunement Chamber → Combat → Skill Draft → Boss → Victory/Death → Meta-ilerleme.
- **Graphify kanıtı** — 6925 node / 118 community; en bağlı 10 node'un 6'sı tooling.

Metodoloji açısından, çok-ajanlı geliştirme sürecinin yalnızca "AI kullanmak" olmadığı gösterildi: rol tanımları, görev belgeleri, karar dökümanları, cross-review ve doğrulama-kanıtı gerekliliğinden oluşan bu süreç geliştirici tarafından tasarlandı ve işletildi.

### 12.2 Yol Haritası

- **Silah üretimi ve sınıf derinleştirme:** Kalan sınıfların benzersiz silahları (sprite + mekanik) ve placeholder skill'lerin gerçek implementasyonu en büyük içerik bloğudur.
- **Elementalist büyü VFX:** Particle/shader düzeyinde efekt üretimi; kod ve araç altyapısı hazır, kullanıcı onayıyla başlayacak. Elementalist'in 8-yön sprite seti PixelLab kredi top-up'ına bağlı olarak BLOCKED durumdadır; o zamana dek mevcut yön + flipX yeniden kullanılır.
- **Ses ve müzik:** Demo CC0 SFX + müzik yatağı yeterlidir; özgün müzik ve sınıfa-özgü prodüksiyon sesleri gelecek kapsamındadır.
- **Boss geliştirme ve Rift portalı entegrasyonu:** Nihai boss "The Architect"in zengin faz yapısı ve portal sprite'larının tam görsel entegrasyonu; Act 2+ için biyom çeşitliliği.
- **Denge ve playtest turları:** Skill ağırlıkları, tier dağılımı, can değerleri ve oda zorlukları şu an tasarım-temelli başlangıç değerleridir; gerçek oyuncu verisiyle denge turları gerekir.
- **Steam yayın hazırlığı:** Mağaza sayfası, sistem gereksinimleri, erişilebilirlik, Steamworks ve Early Access planlaması.

### 12.3 Çıkarılan Dersler

- **Mimari kararları erken kilitlemek zaman kazandırır.** Sahne-bazlıdan veri-güdümlüye geçiş geç alınsaydı üretilmiş içeriğin çoğu yeniden yazılırdı.
- **Testler "kod çalışıyor mu" sorusundan fazlasını sormalıdır.** Yapısal testleri geçen birçok oda prop'larını ada dışında bırakıyordu; görsel çıktı içeren projeler görsel doğrulama katmanı gerektirir. Combat-okunabilirlik vakası (§11.4) bunu en net biçimde gösterdi.
- **AI ajanlarına güven teyide dayanmalıdır.** Sessiz başarısızlık birden çok kez yaşandı; geri dönüşü zor işlemlerde ajan raporu bağımsız doğrulanmalıdır.
- **Tasarım zevki ve oyun hissi devredilemez.** Bir odanın sıkıcılığı, bir knockdown'ın ağırlığı, bir skill seçiminin anlamlılığı yalnızca gerçek oyun oturumundan sonra değerlendirilebilir; geliştirici son karar mercii olarak her zaman sürecin içindeydi.

### 12.4 Değerlendirme

Bu proje; veri-güdümlü mimari, çok-ajanlı geliştirme süreci ve programatik kalite güvencesinin tek-geliştirici ölçeğinde nasıl uygulanabileceğini belgelemektedir. Oynanabilir tam döngü, 10 sınıflık veri altyapısı, 26 oda şablonu, 549 test tanımı envanteri ve graphify ile sayısal olarak kanıtlanan tooling tezi ile ulaşılan kapsam, benimsenen mimari ve metodolojik kararların somut çıktısıdır. Temel mühendislik altyapısı bu çalışmanın kapsamı dahilinde tamamlanmış; ticari kaliteye taşıma yol haritası §12.2'de özetlenmiştir.

---

## Kaynakça

[1] Bridson, R. (2007). Fast Poisson disk sampling in arbitrary dimensions. *SIGGRAPH Sketches*, 22.

[2] Qian, C., Liu, W., Liu, Y., Chen, W., Yao, Y., & Zhou, Z. (2023). ChatDev: Communicative Agents for Software Development. *arXiv preprint arXiv:2307.07924*.

[3] Shaker, N., Togelius, J., & Nelson, M. J. (Eds.). (2016). *Procedural Content Generation in Games*. Springer.

[4] Schreiber, I., & Romero, B. (2012). *Game Design Concepts: An Introduction to Game Design and Game Analysis*. CRC Press.

[5] Supergiant Games. (2021). *Hades: The Design Philosophy Behind It* [GDC Talk]. Game Developers Conference.

[6] Unity Technologies. (2017). *ScriptableObject Architecture in Practice* [Unite Talk]. Unite Europe 2017.

[7] Adams, E., & Dormans, J. (2012). *Game Mechanics: Advanced Game Design*. New Riders.
