<!-- NOT: Savunma versiyonunda Bölüm 4 (Görsel) Bölüm 2'nin arkasına taşınabilir - jüri akış önerisi -->

# RIMA — Bitirme Projesi Final Raporu (TASLAK 2026-06-06)

---

## İçindekiler

- Özet / Abstract
- [1. Giriş](#1-giriş)
- [2. Oyun: RIMA ve Oynanabilir Döngü](#2-oyun-rima-ve-oynanabilir-döngü)
  - 2.6 Oyun Hissi Katmanı
- [3. Veri-Güdümlü Oda Sistemi ve Geliştirme Araçları](#3-veri-güdümlü-oda-sistemi-ve-geliştirme-araçları)
  - 3.5.6 Rooms Sekmesi Oda Editörü ve UI-JSON Çift Yönlü Senkronizasyon
  - 3.5.7 Screenshot Modu
- [4. Görsel Üretim Hattı](#4-görsel-üretim-hattı)
- [5. Yapay Zekâ Destekli Çok-Ajanlı Geliştirme Metodolojisi](#5-yapay-zekâ-destekli-çok-ajanlı-geliştirme-metodolojisi)
- [6. Doğrulama: Test ve Kalite Güvencesi](#6-doğrulama-test-ve-kalite-güvencesi)
- [7. Karşılaşılan Zorluklar ve Çözümler](#7-karşılaşılan-zorluklar-ve-çözümler)
- [8. Yol Haritası ve Sonuç](#8-yol-haritası-ve-sonuç)
- [Şekil Listesi](#şekil-listesi)

---

<!-- KAYNAK: OZET_ABSTRACT.md -->

# Özet / Abstract

---

## Özet

Bu çalışma, tek geliştirici tarafından yapay zekâ destekli çok-ajanlı bir geliştirme süreci ve veri-güdümlü oyun mimarisi kullanılarak hayata geçirilen 2D izometrik bir aksiyon-roguelite oyunu olan RIMA'yı sunmaktadır. Projenin temel sorusu şudur: tek kişilik bir ekip, kapsamlı bir roguelite oyununu nasıl üretebilir?

Bunu mümkün kılmak için iki paralel yaklaşım izlenmiştir. İlki, içerik ve sistemin birbirinden ayrıldığı veri-güdümlü bir mimaridir; oyun odaları ScriptableObject veri dosyaları olarak tanımlanmış, zemin-uçurum-prop yerleşimi çalışma zamanında ve otomatik olarak inşa edilmiş, JSON tabanlı içe aktarma araçlarıyla dış kaynaklı oda tasarımları sisteme entegre edilmiştir. İkincisi, kod üretimi, tasarım danışmanlığı ve çıktı incelemesini farklı rollere dağıtan çok-ajanlı bir yazılım mühendisliği sürecidir. Bu süreçte yazar-reviewer ayrımı, karar dökümanı zorunluluğu ve doğrulama kanıtı gerekliliği temel ilkeler olarak uygulanmıştır.

Proje; veri modeli on sınıfı destekleyecek şekilde kurulmuş olup demo kapsamında 4 sınıf uçtan uca oynanabilir durumdadır ve kalan sınıflar aynı veri hattı üzerinden eklenmektedir; 26 oda şablonu, 549 test tanımı envanteri (son kayıtlı koşu 410 PASS) ve MainMenu'den boss karşılaşmasına uzanan oynanabilir tam döngüyle sonuçlanmıştır. Test altyapısı EditMode ve PlayMode testlerine ek olarak görsel oda kalite güvencesi sürecini kapsamakta; bu süreç gerçek hataları sistematik biçimde tespit etmiş ve düzeltmiştir.

**Anahtar kelimeler:** roguelite, prosedürel içerik, veri-güdümlü tasarım, çok-ajanlı yapay zekâ, oyun geliştirme

---

## Abstract

This work presents RIMA, a 2D isometric action-roguelite game developed by a single developer using an AI-assisted multi-agent development process and a data-driven game architecture. The central question guiding the project is: how can a solo developer produce a roguelite game of meaningful scope and quality?

Two parallel approaches were adopted to address this challenge. The first is a data-driven architecture in which content and systems are decoupled. Game rooms are defined as ScriptableObject data files; floor, cliff, and prop placement are constructed at runtime through automated systems; and externally authored room designs are integrated via a JSON import pipeline. The second is a multi-agent software engineering process in which code generation, design consultation, and output review are distributed across specialized agents. Core principles include author-reviewer separation, mandatory decision documentation, and verification evidence requirements for every completed task.

The project delivers a playable end-to-end loop spanning MainMenu, a walkable diegetic character selection space (Attunement Chamber), room-by-room combat with 3-card skill drafts, branching door choices, and a boss encounter, supported by a ten-class data model (4 classes fully playable end-to-end in the demo build, with the remaining classes ready to integrate via the same pipeline), 26 room templates, and 549 automated test definitions (last recorded run: 410 PASS). The quality assurance infrastructure extends beyond unit testing to include a programmatic visual room QC process, which identified and resolved systematic errors in prop placement across multiple room templates.

**Keywords:** roguelite, procedural content generation, data-driven design, multi-agent artificial intelligence, game development



---

<!-- KAYNAK: BOLUM_2_GIRIS.md -->

## 1. GİRİŞ

---

### 1.1 Problem: Tek Geliştiriciyle Kapsamlı Bir Oyun Yapmak

Aksiyon-roguelite türünde bir oyun geliştirmek, geniş kapsamlı mekanik ve içerik entegrasyonu gerektirmektedir.

Türün referans noktaları olan Hades, Dead Cells ve Slay the Spire, yıllar süren geliştirme süreçleri ve çok kişilik ekiplerle ortaya çıktı. Bu oyunlar yalnızca savaş mekaniklerini değil, her run'u farklı hissettiren oda çeşitliliğini, tutarlı bir görsel dili, yeniden oynama değerini taşıyan build derinliğini ve oturumlar arası anlam katan bir meta-ilerleme ekonomisini bir arada sunmak zorundaydı. Bunların her biri kendi başına bir uzmanlık alanı.

Bu proje, ticari ölçekteki bu örneklerin kapsamını birebir yakalamayı değil; aynı türün temel döngülerini tek geliştirici ölçeğinde veri-güdümlü ve doğrulanabilir bir prototip olarak hayata geçirmeyi hedefledi. RIMA; on sınıflık bir veri altyapısı (demo kapsamında 4 sınıf uçtan uca oynanabilir), sınıfa özgü kaynak ekonomileri ve 80'den fazla yetenek içeren gerçek bir skill draft sistemi, dal dallanmasıyla şekillenen bir dungeon graph, prosedürel içerik için geliştirilmiş bir araç zinciri ve bunların tümünü birbirine bağlayan oynanabilir tam döngüyle tasarlandı. Bütün bunları tek bir geliştirici yapıyordu.

Bu, özünde bir kaynak sorunudur. Ticari bir roguelite normalde birbirinden farklı roller üstlenen bir ekip gerektirir: programcı, seviye tasarımcısı, sanatçı, QA uzmanı ve tüm bu rollerin çalışmasını koordine eden sistem mimarı. Tek kişilik bir projede bu rollerin hepsi aynı kişiye düşer. Eksilenin yalnızca zaman olmadığını söylemek gerekir; perspektif de eksilir. Kendi yazdığın kodu kendi gözden geçirmek, kendi tasarladığın seviyenin monoton olup olmadığını kendi fark etmek giderek güçleşir.

Bu kısıt karşısında iki yaklaşım değerlendirilebilir: projenin kapsamını kesmek ya da mevcut kapasiteyi genişletecek bir iş akışı kurmak. Bu projede ikinci yol tercih edilmiştir.

---

### 1.2 Yaklaşım: İki Ayaklı Çözüm

Belirtilen kaynak kısıtlarına karşı projede iki temel mimari yaklaşım benimsenmiştir.

**Birinci karar: veri-güdümlü mimari.** İçerik, sistemden ayrı tutulmalıdır. Her oyun odası ayrı bir Unity sahnesi değil, bir veri dosyasıdır. Skill'lerin tümü merkezi bir veritabanında tanımlanır. Düşman dalgaları, oda geçişleri ve ödül mekanizmaları sıkı sıkıya bağlı monolitik bir yapıya değil, birbirinden bağımsız ve değiştirilebilir bileşenlere dayanır.

Bu yaklaşımın pratik sonucu şudur: yeni bir oda eklemek, yeni bir ScriptableObject veri dosyası oluşturmak demektir — sahne editöründe elle çalışmak gerekmez. Yeni bir skill eklemek, veritabanına bir kayıt girmek demektir. Bu mimari, içerik ölçeklenmesinin bir kaç kat daha az iş maliyetiyle yapılabilmesini sağlar.

Araçlar da bu mimarinin bir parçasıdır. Zemin boyama ve uçurum yerleştirmeden oda şablonu içe aktarmaya kadar uzanan bir editör araç zinciri, geliştirme sürecinin içeriği test etme, doğrulama ve değiştirme maliyetini doğrudan etkiler. Araçsız bir veri mimarisinin değeri yarısına düşer; bu projede araç geliştirme, oyun geliştirmeyle eş zamanlı ilerledi.

**İkinci karar: yapay zekâ destekli çok-ajanlı geliştirme süreci.** Yapay zekâ entegrasyonunun kapsamını netleştirmek adına bir ayrım yapılmalıdır: yaygın olan "bir dil modeline komut ver, çalıştır" yaklaşımı bu projede benimsenen sistem değildir.

Farklı rollere sahip yapay zekâ ajanları (kod yazma, tasarım danışmanlığı, çıktı inceleme, bilgi tabanı yönetimi) belirli kurallara göre bir araya getirildi. En temel kural şuydu: bir ajandan çıkan iş, o ajanın kendisi tarafından onaylanamaz. Kodu yazan ajan, yazdığı kodu incelemez; başka bir ajan inceler. Tasarım kararları konsey tartışmasına açılır, alternatifleri kayıt altına alan belgeler oluşturulur ve kullanıcı — yani bu projenin geliştiricisi — her kritik adımda son kararı verir.

Bu sürecin kuralları, ajanların kendiliğinden ortaya çıkardığı bir şey değil; denemeler ve gözlemlenen hatalardan öğrenilerek geliştirici tarafından tasarlandı ve zamanla rafine edildi. Ajanlar araçtır; süreci tasarlayan ve yöneten kişi geliştiricinin kendisidir.

Mimarinin sunduğu veri-güdümlülük ile çok-ajanlı sürecin getirdiği doğrulama disiplini birleştiğinde geliştirme verimliliği artmaktadır: veri-güdümlü mimari içerik üretim maliyetini düşürürken, çok-ajanlı süreç bu içeriğin gözden geçirilmesini, test edilmesini ve doğrulanmasını sistematik biçimde mümkün kılmaktadır.

---

### 1.3 Raporun Yapısı

Rapor sekiz bölümden oluşmaktadır. Oyun tasarımı ve oynanabilir döngü (Bölüm 2) ile veri-güdümlü mimari ve araç zinciri (Bölüm 3), projenin tasarım ve mühendislik temellerini sunmaktadır. Görsel üretim hattı (Bölüm 4) ve çok-ajanlı geliştirme metodolojisi (Bölüm 5) projeye özgü teknik katkıları ele almaktadır. Test ve kalite güvencesi (Bölüm 6) ile karşılaşılan zorluklar (Bölüm 7) doğrulama sürecini ve çözüm yaklaşımlarını belgelemektedir. Yol haritası ve sonuç (Bölüm 8) projenin mevcut durumunu ve gelecek adımlarını ortaya koymaktadır.

---

### 1.4 Projenin Ulaştığı Yer

Raporun geri kalanına girmeden önce projenin şu an nerede olduğunu somut biçimde ortaya koymak gerekir.

RIMA, oynanabilir tam döngüye sahip çalışan bir aksiyon-roguelite prototipidir. Oyuncu Ana Menü'den başlar, yürünebilir Attunement Chamber'da on sınıftan birini seçer, oda oda ilerleyen bir run oynar, her üç odada bir 3-kart skill draft ekranıyla karşılaşır, dallanan kapılarla rota kararları verir, boss karşılaşmasına ulaşır ve run sonunda kazandığı Shattered Echo ile bir sonraki sınıfın kilidini açabilir. Bu döngünün tüm halkası çalışır durumda ve birbirine bağlıdır.

Sayısal olarak: 26 oda şablonu, veri modeli 10 sınıfı ve geniş bir skill havuzunu destekleyecek şekilde kurulmuştur; demo kapsamında 4 sınıf uçtan uca oynanabilir durumdadır ve kalan sınıflar aynı veri hattı üzerinden eklenmektedir; 111 kayıtlı skill (~67'si çalışır implementasyona sahip), 549 test tanımı envanteri (508 EditMode + 41 PlayMode; son kayıtlı koşu 410 PASS). Geliştirme sürecinde 100'ü aşkın karar belgesi yazıldı, her büyük mimari veya tasarım kararı belgelenerek arşivlendi. Bunlar yalnızca rakamlar değil; projenin mühendislik disipliniyle yönetildiğinin izleri.

---

*Kelime sayısı: ~1.200*



---

<!-- KAYNAK: BOLUM_3_OYUN.md -->

## 2. OYUN: RIMA VE OYNANABİLİR DÖNGÜ

---

### 2.1 RIMA Nedir?

RIMA, 2D izometrik bir aksiyon-roguelite oyunudur. Oyuncuyu eşsiz güçlere ve birbirinden tamamen farklı kaynak ekonomilerine sahip on farklı savaşçı sınıfından birini seçerek, procedural olarak ilerleyen bir zindan koridorunda oda oda ilerlemeye, her geçişte yeni yetenekler edinerek daha güçlü bir build inşa etmeye davet eder. Oyun döngüsü; oda seçimi, savaş mekanikleri ve kalıcı karakter gelişimine dayanmaktadır: hangi yeteneği alacağı kadar hangi odaya gireceği de önem taşır; başarısız bir run bile ileriki oturumlar için kalıcı bir kazanım bırakır.

Oyunun dünyası ve hikâyesi, "The Fracturing" (Büyük Kırılma) adı verilen kozmik bir felaketin ardından şekillenir. Dünyalar arasında yayılan "Rift March" tehdidini durdurmak amacıyla "The Architect" adlı varlık, boyutlar arasındaki tüm bağlantıları kalıcı olarak koparmıştır. Bu müdahalenin bedeli ağır olmuş; dünya, altlarında sonsuz bir kozmik boşluğun (Void) uzandığı yüzen taş adalara dönüşmüştür. Dungeon mimarisi kasıtlı bir tasarımın değil, bu kırılmanın ürünüdür: her oda, eski bir amaca hizmet etmiş yapıların kalıntısıdır.

Oyuncu bu yıkımın hem bir faili hem de hayatta kalan bir kalıntısıdır (bkz. Şekil 12). Elinde tuttuğu kimlik parçacıkları — "Shattered Echo"lar — aynı zamanda oyunun meta-ilerleme para birimidir. Hikâyesel olarak bu parçacıklar, The Fracturing sırasında dağılmış kendi kimliğinizin unutulmuş yüzlerini temsil eder: yeni bir sınıfın kilidini açmak, kayıp bir yüzü yeniden giyinmek anlamına gelir. Oyunun tonal manifestosu "Vivid Vulnerability" (Canlı Kırılganlık) olarak tanımlanmaktadır; bu ifade, oyunun kaybı ne avutucu ne de hafife alan, bunun yerine her anın ağırlığını sessizce taşıyan melankolik bir atmosferi simgeler. Yıkımın ortasında neon cyan enerjisinin çatlaklar arasında parlaması bu tonu görsel düzeyde pekiştirir: tehlikeli ve dengesiz Rift enerjisi ile onu zapt etmeye çalışan çatlayan antik mühürler, oyunun renk dilinin çekirdeğini oluşturur.

---

### 2.2 Referans Oyunlar: RIMA'nın Esinlendiği ve Özgünleştirdiği Unsurlar

RIMA, roguelite türünün birbirinden farklı üç önemli temsilcisini referans almakta ve bunların unsurlarını kendi tasarım diline çevirmektedir. Bu referanslar doğrudan kopya değil; her birinden belirli bir tasarım kararı alınmış, RIMA'nın özgün bağlamına uyarlanmıştır. Aşağıdaki tablo bu konumlanmayı özetlemektedir.

| Kriter | Hades | Dead Cells | Slay the Spire | **RIMA** |
|---|---|---|---|---|
| **Oda akışı** | Sabit oda seti, sırayla gelir | Procedural oda + biyom geçişi | Sabit düğüm, sırayla | Procedural dungeon graph, 8–11 düğüm (depthCount=5), her run yeniden üretilir |
| **Harita dallanması** | Sınırlı çatallanma | Her odadan birkaç yol, biyom seçimi | StS-tipi ağaç harita, tip etiketleri | 1–3 dallı kapılar; her kapı farklı oda türünü işaret eder |
| **Build oluşturma** | Her odada silah veya destek seçimi | Silah + rune build, geçici yükseltme | 3-kart deck draft, sinerji odaklı | Her 3 odada 1 kez 3-kart skill draft; tier derinlik kilitleri |
| **Karakter seçimi** | Statik menü + arka plan karakterleri | Statik menü | Statik menü | Yürünebilir "Attunement Chamber" — diegetic mekân |
| **Meta-ilerleme** | Darkness + hediye sistemi, run arası | Scroll of Prowess, modifiye kalıcı | Yok (saf roguelite) | Shattered Echo para birimi; yeni sınıf kilidi açma |

Hades'in bu listedeki en belirgin katkısı, karakter seçimini bir menüden çıkarıp oyun dünyasının bir parçası haline getirmesidir. Hades'te Zagreus'un çıkış öncesi Yeraltı sarayında dolaşması ve karakterlerle konuşması nasıl dünyayla bütünleşmiş bir deneyim sunuyorsa, RIMA'nın Attunement Chamber'ı da aynı diegetic yaklaşımı izler; fark olarak oyuncu burada karakterleri dinlemek yerine bedenlerine fiziksel olarak bürünür.

Dead Cells'in katkısı daha çok tempo anlayışında hissedilir: aksiyon combat'ın roguelite ilerleme döngüsünün tam merkezine yerleşmesi, her ödülün bir sonraki zorluk için hazırlık anlamı taşıması. RIMA da combat'ı yalnızca bir engel olarak değil, build kararlarının test edildiği asıl sahne olarak tasarlar.

Slay the Spire'ın etkisi ise en çok skill draft sisteminde görülür. Derinlik kilitleri — nadir yeteneklerin ancak run'un ilerleyen bölümlerinde görünmesi — bu oyunun tasarım mantığından doğrudan alınmış bir karardır. Yetenek havuzu, oyunun erken, orta ve geç aşamalarında oyuncu gelişimini kademeli olarak artıracak şekilde yapılandırılmıştır: başlangıçta temel yönelim belirlenir, ortada sinerji noktaları netleşir, geç aşamada ise Epic ve üzeri tier'lar devreye girer.

---

### 2.3 Tam Oynanabilir Döngü Anlatısı

RIMA'da bir run, mantıksal ve deneyimsel olarak birbirini izleyen birkaç aşamadan oluşur. Geliştirilen entegrasyon sayesinde, run döngüsünün tüm aşamaları birbirine bağlı şekilde çalışmaktadır: oyuncu MainMenu'dan başlayıp Victory ya da Death ekranına ulaşabilmekte, aralarında geçiş yapabilmekte ve biriktirdiği meta-parayı bir sonraki run'da harcayabilmektedir.

### Ana Menü

Oyuncu, oyunu başlatınca karşılaştığı ana menü; yeni bir run başlatmayı, sınıf codex'ini açmayı ve ayarları yönetmeyi sunar. Menü aynı zamanda oyunun görsel tonunu kurar: koyu arka plan, ölçülü cyan vurgular ve sınıf ikon tasarımları. Codex ekranı, oyuncunun mevcut oturumda açılmış tüm skill'leri sınıf bazlı olarak incelemesine olanak tanır; bu sayede henüz run başlamadan mümkün kombinasyonlar keşfedilebilir ve bir strateji oluşturulabilir. "Oyna" seçeneği oyuncuyu doğrudan Attunement Chamber'a taşır.

### Attunement Chamber — Yürünebilir Karakter Seçimi

[Şekil 1: Attunement Chamber genel görünümü — pedestal'lar ve sınıf silüetleri]

RIMA'nın karakter seçim ekranı, pek çok roguelite oyununun aksine statik bir menü değildir. Oyuncu, son oynadığı sınıfın bedeninde gerçek bir odaya spawn olur ve WASD tuşlarıyla serbestçe dolaşır. Odanın çevresi boyunca pedestal'lar üzerinde on farklı sınıf, donmuş "echo"lar gibi bekledirilir: kilidi açılmış sınıflar taş-grisi tonda görünürken, henüz açılmamış olanlar opak siyah silüetler olarak durur. Bu ayrım hem görsel hem de anlatısal bir işlev görür; kaybedilmiş kimliklerin henüz yeniden kazanılmamış olduğu fikri, tek bakışta okunabilir.

Bir silüete yaklaştığında, o sınıfın yeteneklerini ve kaynağını özetleyen paneller ekranın kenarından kayarak girer (bkz. Şekil 1). Oyuncu oradaki combat dummy (eğitim mankeni) üzerinde sınıfın vuruş hissini gerçek zamanlı olarak deneyebilir — seçim yapmadan önce nasıl bir karakter oynayacağını somut biçimde anlayabilir. [G] tuşuna basıldığında karakterin bedeni cyan bir enerji emilimi efektiyle o sınıfa dönüşür (bkz. Şekil 2); hikâyesel olarak bu, unutulmuş bir kimlik yüzünü yeniden giymektir. Seçimi onaylamak için kuzey duvarındaki Rift kapısından yürüyerek çıkmak yeterlidir.

[Şekil 2: [G] tuşuyla bürünme anı — cyan enerji efekti]

Bu tasarım kararının değeri, menüyü oyunun kendi dilini konuşan bir mekâna dönüştürmesinde yatar. Oyuncu asıl run'a başlamadan önce bile dungeon atmosferinin içindedir; sınıfını denemiş, bir seçim yapmış ve o seçimi fiziksel bir eylemle onaylamıştır. Geleneksel bir menü butonu tıklaması bunu yapamaz.

### Run — Oda Oda İlerleme

[Şekil 3: Tipik bir combat odası — checker zemin + prop düzeni + aktif düşman grubu]

Attunement Chamber'dan çıkan oyuncu, run'un ilk odasına girer (bkz. Şekil 3). RIMA'nın run yapısı, aktif _Arena ortamında çalışan `DungeonGraph.Generate(depthCount=5)` çağrısıyla oluşturulan değişken bir dungeon graph üzerine inşa edilmiştir. Bu graph her run başında yeniden üretilir ve dal yapısına bağlı olarak 8–11 düğüm arasında değişir; standart savaş odaları, elite karşılaşmaları, ödül odaları, dal birleşim noktaları ve son düğüm olarak boss arenası içerir. (Projenin erken aşamasında geliştirilen eski Core DungeonGraph bileşeninin 12 sabit düğümlü basit yapısı, aktif run sistemiyle karıştırılmamalıdır.) Hangi odaya girileceği ise tamamen oyuncunun elindedir: dallanan kapı sistemi, run'un her aşamasında oyuncuya aktif bir rota kararı verir.

**Combat:** Oda aktif olduğunda düşman dalgaları sahneye girer. Oyuncu, seçtiği sınıfa özgü saldırı zinciri ve yetenek seti ile dövüşür. Düşmanlar hasar aldıklarında geri savrulur; ağır saldırılar tam bir knockdown animasyonu başlatır — karakter yere düşer, bir süre yerde kalır ve kalkış animasyonunun bittiği ana kadar i-frame (geçici dokunulmazlık) korumasına sahiptir. Bu knockdown sistemi hem görsel geri bildirim hem de taktiksel bir fırsat penceresi olarak çalışır: yerde olan bir düşman saldırı açısından daha savunmasız ama i-frame süresi içinde hasar almaz. Son düşman da devrildiğinde oda "Cleared" (Temizlendi) durumuna geçer; oda-clear SFX'i ve görsel geri bildirim tetiklenir.

**3-Kart Skill Draft:** Her üç odada bir, oda ortasında bir ödül nesnesi belirir. Oyuncu [G] tuşuyla ödülü aldığında ekrana üç skill kartı açılır. Her kart, oyuncunun sınıfına ait ya da nötr pasif bir yetenek sunar. Üzerine gelindiğinde tooltip, yeteneğin ne yaptığını ve mevcut build ile olan sinerji noktalarını gösterir. Oyuncu üç karttan birini seçer ve o yetenek kalıcı olarak o run'a eklenir (bkz. Şekil 4). Bu an, run'un en yüksek karar yüklü kesimidir: yanlış seçim build'in tutarsızlaşmasına yol açabileceği gibi beklenmedik bir kombinasyon tamamen yeni bir oynanış kapısı açabilir.

[Şekil 4: 3-kart skill draft ekranı — tooltip açık, sinerji chip'i görünür]

**Dallanan Rift Portalları:** Oda temizlendikten ve ödül alındıktan sonra arka kenarda (back edge) 1 ila 3 arasında çıkış portalı açılır. Her Rift portalının üzerinde yer alan simge, hedefteki oda türünü işaret eder: olağan bir savaş mı, daha zorlu ama daha iyi ödüllü bir elite odası mı, yoksa doğrudan bir ödül odası mı? Oyuncu hangi çıkış portalından geçeceğine karar vererek run'un kısa vadeli akışını biçimlendirir (bkz. Şekil 5). Tehlikeyi mi yoksa güvenliği mi tercih eder; bu tercihler run'un sonundaki build'i doğrudan şekillendirir.

[Şekil 5: Çift veya üçlü kapı çıkışı — oda türü simgeleri görünür]

**Run Haritası [M]:** Oyuncu istediği anda [M] tuşuna basarak o ana kadar gezdiği düğümleri ve önündeki olası yolları gösteren run harita katmanını açabilir. Bu katman, hangi oda türlerinin kaç adım uzakta olduğunu görselleştirerek karar verme sürecine destek olur.

**Boss Karşılaşması:** Run'un sonuna doğru graph'ta boss düğümüne ulaşıldığında, boss arenasına giden Rift portalı açılır. Boss arenası hem boyut hem zorluk açısından standart odalardan belirgin biçimde ayrışır; ayrıca mekânik tasarımı da o ana kadar run boyunca edinilmiş skill'lerin birlikte kullanımını sınar. Demo kapsamındaki boss karşılaşması, boss düğümüne ulaşan ve temel saldırı/zafer akışını tamamlayan bir karşılaşmadır; tasarım vizyonundaki nihai boss — The Fracturing'in mimarı olan varlık — ise oyuncunun run boyunca öğrendiklerini ona karşı uygulayacağı zengin bir faz yapısıyla yol haritasında yer almaktadır. Bossun devrilmesiyle ekranda kısa bir slow-motion geçişi yaşanır ve Victory akışı başlar.

### Victory / Ölüm ve Meta-İlerleme

Run sonunda — ister zaferle ister ölümle kapansın — oyuncuya o run'dan kazandığı Shattered Echo miktarı gösterilir. Echo hesabı; geçilen oda sayısının 3 ile çarpımı ile toplam düşman öldürme sayısının 5'e bölümünün toplanmasıyla elde edilir (formül: `odaSayısı × 3 + öldürmeSayısı / 5`). Tek bir run'dan kazanılabilecek miktar en az 5, en fazla 60 Echo ile sınırlıdır; bu sınır, erken oyun ile geç oyun arasındaki güç farkını kontrol altında tutar ve oyuncunun her run'u değerli hissetmesini sağlar.

Biriken Echo'lar, bir sonraki oturumda Attunement Chamber'daki kilitli sınıf silüetlerinden birinin kilidini açmak için harcanabilir. Demo sürümünde dört sınıf baştan kilitsizdir; kalan sınıfların maliyeti sınıfın güç eşiğine göre farklılık gösterir: Ronin ve Ravager 150, Gunslinger/Brawler/Summoner 200, Hexer ise 250 Echo gerektirir. Sistem, oyunculara her run'un boşa gitmediğini hissettirir: kazanılan run heyecanını tatmin ederken, kaybedilen run bir sonrakine yatırım yapılmış bir zemin bırakır. Bu yapıyla RIMA, saf roguelite anlayışının "her şeyi sıfırla" getiri hesabını, meta-ilerlemenin "her adım önemli" güvencesiyle dengeler. Oyunun ilerleyen aşamalarında bu ekonominin küresel harita geliştirmeleri ve ekipman modifikasyonları için de kullanılması planlanmaktadır; mevcut sürümde sınıf kilidi açma birincil harcama yolunu oluşturmaktadır.

---

### 2.4 Build Sistemi — Skill Draft, Tier Kilitleri ve Sinerji

#### 2.4.1 Skill Database ve Tier Sistemi

RIMA'nın tüm yetenekleri merkezi bir SkillDatabase'de tutulmaktadır. Bu veritabanında toplam 111 skill kaydı mevcuttur; bunların 67'si oynanabilir ve çalışır durumda implementasyona sahipken, 44'ü tasarım aşamasında olan placeholder olarak beklemektedir. Placeholder kayıtlar SkillDatabase'de tasarım envanteri olarak tutulur ve skill codex ekranında 'yakında' satırları olarak görünür; ancak run içi draft havuzuna girmezler — teklif üretici yalnızca implementasyonu tamamlanmış (`isImplemented`) ve sınıf filtresinden geçen skill'leri sunar. Veri modeli on sınıfı ve geniş bir skill havuzunu destekleyecek şekilde kurulmuştur; demo kapsamında 4 sınıf uçtan uca oynanabilir durumdadır ve kalan sınıflar aynı veri hattı üzerinden eklenmektedir. On sınıfın her biri farklı bir kaynak mekanizmasına ve farklı bir oynanış diline kurulmuştur; Rage barını hasar vererek dolduran savaşçıdan, combo zincirleri üzerine inşa edilmiş hızlı dövüşçüye, elementel yıkım üretmeye odaklanan büyücüden yavaş birikim ve ani patlama döngüsüne dayanan diğer sınıflara kadar her biri ayrı bir oyun tarzını temsil eder.

Skill'ler beş tier'a ayrılmakta ve draft sırasında bu tier'ların görünme olasılıkları şu şekilde kalibre edilmiştir:

| Tier | Görünme Ağırlığı | Run Derinliği Kilidi |
|---|---|---|
| Common (Yaygın) | 55 | Kilitsiz, ilk odadan itibaren |
| Rare (Nadir) | 27 | Kilitsiz, ilk odadan itibaren |
| Epic | 12 | Oda 3 ve sonrası |
| Mythic | 5 | Oda 3+, yalnızca oyuncunun birincil sınıfı |
| Legendary | 3 | Oda 7 ve sonrası |

Bu olasılık dağılımı bilinçli olarak tasarlanmıştır. Run'un ilk yarısında oyuncu, karakterinin temel kimliğini ve oynanış yönünü belirleyen yaygın ve nadir skill'leri seçer. Bu aşamada seçimler nispeten geniş bir alana yayılır: oyuncu hangi ekseni güçlendireceğine henüz tam karar vermemiştir ve sistem buna izin verir. Run ilerledikçe sinerji noktaları netleşir ve gerçek güç sıçramaları olan Epic ve üzeri tier'lar devreye girer. Legendary skill, run'un son çeyreğinde ortaya çıkar; o noktada build'in yönü artık belirginleşmiş olduğundan seçim gerçek anlamda ağırlık taşır — doğru Legendary, kurulu sinerjileri zirveye taşırken yanlış seçim build'i dağıtabilir. Bu yapı, her run'un hem ilk hem son skill seçimini anlamlı kılar.

#### 2.4.2 Warblade Örneği — Bir Sınıfın Tasarım Dili

[Şekil 6: Warblade karakteri — omuz zırhı ve iki elli kılıç silueti]

On sınıfın tamamını tek tek açıklamak bu raporun kapsamının dışında kalır; ancak Warblade sınıfı üzerinden RIMA'nın sınıf tasarım felsefesini somutlaştırmak mümkündür.

Warblade, "yaklaş, sabitle, zırh kır, infaz et" mantığıyla çalışan ağır bir savaşçıdır (bkz. Şekil 6). Omuz zırhlı silueti ve devasa iki elli kılıcıyla görsel olarak tanınabilirdir. Temel kaynağı, yalnızca hasar vererek ve düşmanlara kitle kontrolü (CC) uygulayarak doldurulan bir Rage barıdır; savaş dışında pasif olarak erir. Bu kaynak tasarımı, oyuncuyu sürekli temasa zorlar ve savunmacı bir oyun biçimini cezalandırır.

Warblade'in skill seti bu kaynak döngüsünü destekler. "Iron Charge" ile hedefe atılarak sersemletme, "Gravity Cleave" ile düşmanları kesen bir çekim alanı oluşturma ve "Iron Counter" ile reaktif bir savuşturma penceresi açma; bu beceriler savaşı her zaman düşmanı Broken (Sersemlemiş) veya Sundered (Parçalanmış Zırh) durumuna doğru yönlendirir. Bu durumlardaki bir hedefe imza yetenek "Death Blow" uygulanabilir: Rage barını tamamen boşaltarak yüzde dörtyüz veya üzeri hasar veren bir infaz darbesi. Rage barı dolduğunda ise [V] tuşuyla "Bladestorm" ulimate'i devreye girer; beş saniyelik kitle kontrolü bağışıklığı ve sürekli dönen AoE hasarıyla oyuncu kısa süreliğine durdurulamaz hale gelir.

Bu döngünün anlamlı kılınması, skill draft sırasındaki kararlarla gerçekleşir: bir Warblade oyuncusu seçtiği skill'lerin Rage kazanımını mı hızlandırdığını, Sundered durumunun hasarını mı çarptığını, yoksa Death Blow sonrasındaki boş Rage barını mı daha hızlı doldurmayı sağladığını değerlendirerek seçim yapar. Bu seçimlerin toplamı, her run'un farklı hissettiren bir Warblade versiyonunu ortaya çıkarır.

Benzer mantık diğer dokuz sınıf için de geçerlidir. Her sınıfın kaynak ekonomisi, imza yeteneği ve savaş döngüsü birbirinden yeterince farklılaştırılmıştır; aynı oyuncu arka arkaya iki farklı sınıfla oynadığında yalnızca varlık değil, düşünme biçimi de değişir. Bu çeşitlilik, tekrar oynama değerinin temel motorunu oluşturur.

#### 2.4.3 Chain Window ve Sinerji Gösterimi

Build sisteminin ikinci katmanını chain window (zincir penceresi) mekanizması oluşturmaktadır. Oyunun içinde 5 farklı zincir penceresi tanımlıdır. Bunlar, belirli skill'lerin ard arda tetiklendiğinde birbirinin etkisini çarpan zamanlanmış pencerelerdir: örneğin bir sersemletme yeteneği kullanıldıktan sonra belirli bir zaman dilimi içinde başka bir yetenek çalıştırılırsa, ikinci yeteneğin hasarı veya etkisi önemli ölçüde artar. Bu pencerenin kaçırılması zincirin kırılması anlamına gelir ve oyuncuyu bir sonraki fırsata hazırlanmaya yönlendirir.

Skill draft ekranında, seçilebilecek bir kart mevcut build'deki aktif bir zincir penceresiyle etkileşime giriyorsa bu sinerji küçük bir görsel chip olarak kartın üzerinde gösterilir. Bu sayede oyuncu, seçim yapmadan önce build'inin nasıl büyüyeceğini okuyabilir; sezgisel bir "birleşim var" sinyali alır ve seçimini buna göre biçimlendirebilir. Sinerji sistemi oyuncuyu daha derin bir anlayışa zorlamaz; ama bunu keşfetmek isteyenlere build'i sıradışı bir yöne taşıma kapısı açar. Bu katmanlı tasarım, hem ilk kez oynayan için yalın hem de ileri düzey oyuncu için stratejik bir deneyim sunar.

---

### 2.5 Oda Akışı State Machine

Bir RIMA odasının yaşam döngüsü 7 durumdan oluşan bir durum makinesiyle yönetilir. Aşağıdaki şema bu geçişleri özetlemektedir:

```
[Idle]
   │
   ▼
[Combat] ◄── Düşman dalgaları aktif; oyuncu hasar verilebilir
   │
   ▼  (son düşman düşürüldü)
[Cleared]
   │
   ▼  (oyuncu ödül nesnesini G ile alıyor)
[RewardTaken]
   │
   ▼  (kapı veya kapılar açılıyor)
[DoorOpen]
   │
   ▼  (oyuncu bir kapıya giriyor)
[Advancing]
   │             │
   ▼             ▼  (graph'ta son düğüm geçildi)
[Idle]        [Victory]
```

*[Savunma öncesi UML durum diyagramı olarak yeniden çizilecektir.]*

`RoomRunDirector` bileşeni bu yedi durumu (Idle, Combat, Cleared, RewardTaken, DoorOpen, Advancing, Victory) yönetir ve her geçişi bir koşul karşılandığında tetikler. Geçişler kasıtlı olarak tek yönlüdür: savaş bitmeden ödül alınamaz, ödül alınmadan kapılar açılmaz, kapıdan girmeden bir sonraki oda kurulmaz. Bu tasarım hem oyunculara net ve adil bir ilerleme hissi sunar hem de sistemin kendi içinde yarış koşullarına (race condition) düşmesini önler.

Durum makinesinin bir diğer işlevi de combat sistemini oda geçişlerinden ayırmaktır. EncounterController, düşman dalgalarını bağımsız olarak yönetir ve tüm dalgalar bittiğinde RoomRunDirector'a "temizlendi" sinyali gönderir. Bu ayrışma, düşman dalga tasarımının oda mimarisinden bağımsız olarak güncellenmesini mümkün kılar; yeni bir oda eklenmesi var olan savaş mantığını bozmaz, yeni bir düşman türü eklenmesi mevcut oda geçiş akışını etkilemez.

Ölüm, Victory dışındaki herhangi bir durumda gerçekleşebilir ve run'u doğrudan Death Screen'e taşır. Run, Victory durumuna ulaştığında ise kısa bir slow-motion geçişinin ardından kazanılan Echo hesabı ve özet ekranı gösterilir. Her iki sonuç da meta-ilerleme sistemine beslenerek bir sonraki run'un başlangıç koşullarını günceller; ölüm bir son değil, döngünün kaçınılmaz bir halkasıdır. RIMA bu halkayı bilinçli olarak kısa ve ritimli tasarlar: ortalama bir run, sınıfın oynanış hızına ve alınan kararlara göre değişmekle birlikte, oyuncuya her seferinde yeni bir deneme yapma isteği bırakacak uzunlukta tutulur.

---

### 2.6 Oyun Hissi Katmanı

Mekanik sistemlerin doğru çalışması oynanabilir bir prototip oluşturmak için yeterlidir; ancak bir aksiyon oyununun inandırıcı hissettirmesi için ayrı bir çalışma gerekir. RIMA'da bu çalışma ekran sarsıntısı, zaman dondurması ve ses geri bildirimini kapsayan bir "oyun hissi" katmanının eklenmesiyle gerçekleştirildi.

**Hit-pause ve ekran sarsıntısı.** Hafif vuruşlar 0.03 saniye, ağır vuruşlar 0.06 saniye, infaz darbesi 0.10 saniye süre boyunca zamanı dondurur. Bu değerler, hafif/ağır/infaz darbeleri arasında hissedilir ağırlık farkı yaratmak amacıyla yapılan demo tuning çalışmalarıyla belirlenmiştir. Her hit-pause tieriyle eşleşen bir ekran sarsıntısı şiddeti de tanımlanmıştır; knockdown ve infaz animasyonlarına özgü sarsıntı profilleri bunlara eklenmiştir. Tüm bu efektler tek bir FeelToggle bayrağından kapatılabilmektedir; bu, erişilebilirlik ihtiyacı olan oyuncular için pratik bir seçenektir.

**İnfaz dünya-prompt'u.** Broken veya Sundered durumundaki bir düşmanın iki birim içindeyken üzerinde altın renkli "[RMB] İnfaz" yazısı belirir. Renk seçimi kasıtlıdır: cyan, oyunun dünya dilinde Echo ve Rift enerjisine ayrıldığı için infaz prompt'u bu alandan bilinçlice uzak tutuldu. İnfaz işaretinin görünmesi, oyuncuya taktiksel bir pencere açtığı sinyalini verir; [RMB] tuşuyla gerçekleştirilen DeathBlow darbesini zaman dondurma + sarsıntı + SFX paketi izler.

**Ses entegrasyonu.** Demo kapsamında temel SFX geri bildirimi CC0 lisanslı kliplerle entegre edilmiştir; özgün müzik, sınıfa özgü prodüksiyon sesleri ve adaptif müzik katmanları gelecek çalışma kapsamındadır. Somut olarak: 18 adet CC0 lisanslı ses klibi projeye dahil edildi (kaynak: Kenney ses paketi, lisans dosyasıyla birlikte). Dokuz SFX kanalı tanımlandı: swing, hit, death, execute, oda-clear, skill draft, ambient, knockdown ve genel UI. Her kanal bağımsız volume kontrolüne sahiptir; sahne geçişlerinde ambient ses sızıntısını önleyen bir yaşam-döngüsü koruması da bu entegrasyon sırasında eklendi.

**Dash giriş tamponu.** Oyuncu bir dash girişi yaptığında bu talep 0.08 saniye, bir saldırı girişi yaptığında ise 0.18 saniye süre boyunca tutulur. Bu iki tamponun birbirine karışmaması özellikle dikkat gerektiren bir uygulama detayıydı; çerçeve analizi doğruladı.

**Metodoloji notu.** Bu çalışma, "yazar ile gözden geçiren aynı ajan olamaz" kuralının pratik değerini net biçimde ortaya koyan bir vaka oluşturdu. İlk teslimde bağımsız bir gözden geçirici 9 hata bildirdi: dash tamponunun saldırı girişlerini yutması, chamber ambient sesinin sahne değişimine rağmen başka sahnelerde sızmaya devam etmesi, feel-toggle'ın infaz ve knockdown efektlerini durduramaması, sahne-unload anında infaz prompt'unun patlaması, DoT hasarının ses spamı üretmesi ve freeze'lerin üst üste birikmesi bunlar arasındaydı. 9 bulgunun tamamı düzeltildi; ikinci inceleme geçiş verdikten sonra iş tamamlandı kabul edildi. Bu süreç, oyuncuya sunulmadan önceki bir filtre aşaması olarak işten sonra göz kaçırılan hataları sistematik biçimde yakaladı.

---

*Kelime sayısı: ~2.405 (markdown işaretleri, tablo ayraçları ve görsel yer tutucular hariç)*



---

<!-- KAYNAK: BOLUM_4_INSA.md -->

## 3. VERİ-GÜDÜMLÜ ODA SİSTEMİ VE GELİŞTİRME ARAÇLARI

### 3.1 Odanın Kimliği: Sahneden Veriye

RIMA'nın harita mimarisi, geleneksel sahne tabanlı yapı yerine veri-güdümlü bir yaklaşıma dayanmaktadır: her oyun odası, Unity sahnesi olarak değil, bir veri dosyası olarak tanımlanır.

Geleneksel yaklaşımda her oda ayrı bir sahne dosyasıdır; düşman yerleşimi, çıkış kapıları ve zemin düzeni bu sahnenin içine gömülüdür. Bu yöntem küçük ölçekte hızlı sonuç verir, ancak içerik büyüdükçe yönetilemez hale gelir: 30 farklı oda için 30 sahne, 30 farklı konfigürasyon noktası ve her güncelleme için 30 ayrı açma-kapama döngüsü anlamına gelir. Tek geliştirici için bu yük orantısızdır.

RIMA'da bunun yerine her oda, `RoomTemplateSO` adı verilen bir ScriptableObject veri yapısında tanımlanır [3, 6]. Bu dosya; odanın sınırlarını, kapı soketlerini, oyuncu başlangıç noktasını, yürünebilir ızgara bilgisini, doluluğu ve zorluk etiketlerini içerir. Asıl görsel yapı, yani zemin, kenar, uçurum ve prop'lar, oyun başladığında bu veriden çalışma zamanında inşa edilir.

Bunu mümkün kılan sistem `IsoRoomBuilder`'dır. Tek bir arena sahnesi olan `_Arena` üzerinde çalışır; her yeni odaya geçildiğinde önceki oda temizlenir ve veri dosyasından yenisi sıfırdan kurulur. Bu tasarım, içerik ölçeklenebilirliği açısından kritik bir avantaj sağlar: yeni bir oda eklemek, yeni bir ScriptableObject dosyası oluşturmak demektir. Sahne düzenleyicisinde elle çalışmak gerekmez. Tek geliştirici, içerik ekleme ve test etme sürecini önemli ölçüde hızlandırır.

### 3.2 İzometrik Yüzen Ada: Zemin, Uçurum ve Otomatik Yerleşim

RIMA'nın görsel dili "yüzen izometrik ada" üzerine kuruludur. Her oda, boşlukta asılı duran koyu granit bir platformdur; karakterler bu adanın üzerinde hareket eder, düşmanlar onların etrafında dolaşır. Bu sunum tercihinin ardında bilinçli bir tasarım kararı yatar: çoğu oyun odası kasıtlı olarak kapalı duvarlarla çevrilmiş bir oda yerine yüzen bir savaş adası biçiminde sunulmaktadır. Bu seçim hem okunabilirliği korur hem de prosedürel düzende birleştirme karmaşıklığını azaltır; aynı zamanda RIMA'nın dünyasına özgü boşlukta asılı kalma kimliğini pekiştirir.

Bu görünümün oluşturulması birkaç ayrı katmanın doğru sırayla inşa edilmesini gerektirir (bkz. Şekil 7). Önce zemin karoları yerleştirilir; projenin görsel kimliğine uygun olarak iki varyant mevcuttur: standart granit ve belirli odaları ayırt etmek için tasarlanmış kareli (checker) desen. Her iki varyant da aynı ızgara sistemini paylaşır.

Zemin döşendikten sonra ada kenarlarına uçurum (cliff) yerleştirilmesi gerekir. Bu işlem tamamen otomasyona bırakılmıştır. Çalışma zamanında `IsoRoomBuilder`, zemin ızgarasını tarar ve her hücrenin SW (güneybatı) ile SE (güneydoğu) komşularının void (boşluk) olup olmadığını kontrol eder; güney cephesinde bulunan hücrelere ön-yüz uçurum sprite'ı yerleştirilir. Editör aracı olan `CliffAutoPlacer` ise sekiz izometrik yön için daha kapsamlı directional mantık sunar ve oda şablonlarını editör zamanında hazırlamak için kullanılır. Her iki sistemde de geliştirici hiçbir uçurum karosunu elle yerleştirmek zorunda kalmaz.

[Şekil 7: IsoRoomBuilder tarafından çalışma zamanında oluşturulan örnek oda — zemin + otomatik cliff yerleşimi + prop'lar]

Görsel yapının yanı sıra adanın fiziksel sınırlarının da tutarlı biçimde zorlanması gerekir. Bu katman üç boyutuyla ele alınmaktadır.

**Oda geometrisi.** Sistematik bir denetim, on ayrı boşluk tespit etti: donut tipindeki odaların iç deliğinde collider eksikti; mob'lar kinematik hareket kullandığından walkable ızgarayı hiç sorgulamıyordu. Bu boşlukları kapatmak için WalkabilityMap artık spawn öncesinde doğrudan şablonun `walkableGrid` verisinden başlatılmakta; iç delik collider'ı üretim sırasında otomatik olarak oluşturulmaktadır.

**Hareket güvenliği.** Knockback itmesi ve knockdown yayı (MoveArc) varlıkları adanın dışına taşıyabiliyordu. Mob hareketi, knockback itmesi, knockdown yayı ve elite-teleport hedefleri yürünebilir hücrelere clamp'lendi; diyagonal köşe-kesme (iki çapraz hücre arasından sızma) de engellendi.

**Doğrulama.** Tünelleme analizi, mevcut hareket hızlarında kare başına kat edilen mesafenin hücre boyutunun çok altında kaldığını göstererek bu riskin ihmal edilebilir düzeyde olduğunu doğruladı. Knockback ve Walkable test gruplarının tamamı yeşil sonuç verdi. Bu katman, oyuncunun veya düşmanın görsel ada sınırını ihlal etmesini engelleyerek floating-island temasının oynanış güvenliğiyle çelişmesini önler.

### 3.3 Dış Kaynaklı İçerik: Oda Tasarımına Yeni Bir Yaklaşım

30'dan fazla oda şablonu hazırlamak, tek bir geliştirici için ciddi bir içerik yükü oluşturur. Eğer her oda elle çizilecekse zamanın büyük kısmı içerik üretimine gider ve sistem geliştirmek için zaman kalmaz. RIMA bu soruna ilginç bir çözüm getirmiştir.

Oda tasarımları için ChatGPT'den yapay zeka destekli bir "seviye tasarımcısı" olarak yararlanılmıştır. Ancak bu süreç gözetim dışı bırakılmamıştır. Bir oda tasarımının geçerli sayılabilmesi için belirli bir şema formatına uyması gerekiyordu: ASCII ızgara düzeni, kapı soketleri, zorluk etiketi ve prop listesini içeren JSON çıktısı. Bu format `RoomJsonImporter` aracına doğrudan beslenebilir durumdaydı. ChatGPT'den bu şemaya uygun oda paketleri üretmesi istendi.

Elde edilen tasarımlar doğrudan kabul edilmedi; bir eleme sürecinden geçirildi. Birden fazla bakış açısıyla değerlendirme yapılarak oda paketi incelendi ve çeşitlilik, zorluk dengesi ve tematik uygunluk kriterleri göz önünde bulundurularak seçimler yapıldı. Bu filtreleme sonucunda 15 oda dışarıdan temin edildi ve mevcut odalarla birleştirilerek 26 şablonluk bir havuz oluşturuldu.

Bu yaklaşım, "LLM'i kör biçimde kullanmak" ile "her şeyi elle yapmak" arasındaki bir denge noktasını temsil eder. Yapay zeka ham içerik üretir; geliştirici ise kalite filtresi olarak konumlanır. Her oda şablonu `_Arena` sahnesinde görsel olarak da doğrulanmış; ilk QC geçişinde 15 tamam / 9 şüpheli / 2 başarısız sonucu alınmış, ardından tamir yapılarak 26 şablonun tamamı yapısal doğrulamadan geçirilmiştir (bkz. Bölüm 6, §6.3).

| Oda Havuzu Özeti | Değer |
|---|---|
| Toplam oda şablonu | 26 |
| Dahili tasarım | ~11 |
| ChatGPT paketinden seçilen | 15 |
| QC uygulanan şablon | 26 |

[Şekil 8: JSON → RoomTemplateSO akış şeması (ChatGPT çıktısı → RoomJsonImporter → ScriptableObject → IsoRoomBuilder → Oyun İçi Oda)]

### 3.4 Prop Yerleşimi: Oda Kompozisyonunun Otomasyonu

Her oda şablonu prop listesi taşır: bölgeye uygun dekor nesneleri (sandıklar, taşlar, kemikler, sütun kaidesi vb.). Ancak bu nesnelerin oda içindeki konumlarını tek tek belirlemek hem zaman alır hem de tekrarlayan bir işlemdir. RIMA bu süreci algoritmik olarak çözmüştür.

Prop yerleşimi için `BridsonPoissonAutoPlacer` editör aracı kullanılmaktadır [1]. Bu araç yalnızca Unity Editörü içinde (`#if UNITY_EDITOR`) çalışır ve üretilen konumları `RoomTemplateSO` varlığına bake ederek kaydeder; çalışma zamanında `IsoRoomBuilder` bu önceden hesaplanmış konumları okuyarak prop'ları doğrudan yerleştirir. Algoritma, yürünebilir zemin hücreleri üzerinde minimum örnekleme mesafesi `r` garantisiyle rastgele yerleştirme noktaları oluşturur; bu sayede iki prop arasındaki minimum mesafe korunur ve odada organik bir dağılım elde edilir. Böylece prop'lar birbirinin üzerine yığılmaz ve odada organik bir dağılım elde edilir.

Yerleştirme kararlarına ek bir kısıt daha rehberlik eder: kompozisyon rol haritası. Odanın merkezi "temiz merkez" olarak işaretlenir; burada prop yoğunluğu belirgin biçimde düşük tutulur, çünkü dövüş bu alanda geçer ve oyuncunun hareket alanının açık olması gerekir. Kapı girişleri ve duvar kuşakları sıfır yoğunlukla korunur; kapı önü daima geçilebilir durumda bırakılır. Kenar kuşakları ise dekorasyon için ayrılır. Tüm bu kurallar algoritmik olarak uygulandığından, 26 farklı oda şablonunda tutarlı bir yerleşim kalitesi elde edilir ve tek bir prop konumuna elle müdahale edilmesine gerek kalmaz.

### 3.5 Geliştirme Araçları: Oyunla Birlikte Büyüyen Bir Araç Zinciri

Geliştirme sürecinde oyun sistemleriyle paralel olarak bir editör araç zinciri tasarlanmıştır. Bu araçlar olmadan 26 oda şablonu üretmek, QC sürecinden geçirmek ve görsel olarak doğrulamak imkânsıza yakın olurdu.

#### 3.5.1 Map Designer

Projenin ana editör aracı, birleşik bir 8-sekmeli yapıya sahip `Map Designer` penceresidir. Sekmelerin her biri harita oluşturmanın farklı bir boyutunu kapsar: oda şablonu yönetimi (Rooms), kütüphane (Library), zemin yerleşimi (Floor), uçurum düzenleme (Cliff), nesne ekleme (Object), portal tanımı (Portal), ışık yerleşimi (Light) ve katman yönetimi (Layers). Rooms sekmesi, `RoomTemplateSO` varlıklarının ön kapısı olarak hizmet vermektedir.

Araç tek bir ekranda tüm üretim döngüsünü kapsamaktadır: mevcut oda şablonlarına göz atma, zemin boyama (1, 3, 5 veya 10 karo boyutunda fırça), uçurum otomatik üretme ve önizleme görüntüleme. Bu tasarım kararı bilinçliydi; harita üretiminin her aşaması için ayrı pencereler açmak yerine tek merkezli bir iş akışı tercih edildi (bkz. Şekil 9).

Araç, Unity editöründe çalışmanın yanı sıra, oynanış sırasında `F2` tuşuyla tetiklenebilen bir in-game katman (overlay) moduna sahiptir. Overlay açıldığında `Time.timeScale = 0` ile oyun duraklatılır; geliştirici editöre geçmeden zemin ile prop düzenlemesi yapabilir, overlay kapatıldığında önceki zaman ölçeği geri yüklenir. Editör ve çalışma zamanı aynı `RoomData` veri yapısını paylaştığından, yapılan değişiklikler anında yansır.

#### 3.5.2 Room Browser

`Room Browser`, proje genelindeki tüm `RoomTemplateSO` dosyalarını listeleyen ve tek tıkla `_Arena` sahnesinde inşa eden bir editör penceresidir. Görevi basit ama kritiktir: kalite güvencesi için her şablonu görsel olarak doğrulamak, büyük bir zaman maliyeti doğurabilirdi. Room Browser sayesinde bu süreç minimuma inmiş; 26 oda şablonunun tamamı bu araç aracılığıyla kontrol edilmiştir (bkz. Şekil 10).

Bu araç aynı zamanda sunum ve demo kolaylığı sağlar: hoca veya jüri karşısında herhangi bir oda anında oluşturulup gösterilebilir.

#### 3.5.3 RoomJsonImporter

Bu araç, ChatGPT'den veya başka bir kaynaktan gelen JSON formatındaki oda verilerini `RoomTemplateSO` varlıklarına dönüştürür. Şemayı doğrular (genişlik/yükseklik geçerliliği, kapı soketleri, oda tipi etiketleri), veriden şablonu oluşturur ve varlık veritabanına kaydeder. 15 dışarıdan temin edilen oda bu araç aracılığıyla sisteme dahil edilmiştir.

#### 3.5.4 Registry Baker

Zemin, uçurum ve prop varlıklarının çalışma zamanında doğru şekilde bulunabilmesi için bir kayıt defteri (registry) tutulmaktadır. `RuntimeAssetRegistryBaker` aracı, proje varlıklarını belirlenmiş kök dizinlerden tarar, GUID, görüntüleme adı, etiket, katman ve nesne türü (kind) gibi meta veri bilgilerini çıkarır ve bunu çalışma zamanına aktarır. Bu araç olmadan yeni bir varlık eklemek, manuel kayıt defteri güncellemesi gerektirirdi; baker bu adımı otomatikleştirir.

[Şekil 9: Map Designer 8-sekmeli pencere görünümü]
[Şekil 10: Room Browser + _Arena'da kurulu oda]

### 3.5.5 Authored Portal Soketi Sistemi

RIMA'nın veri-güdümlü yaklaşımı oda geçişlerinin nasıl tanımlandığında da kendini göstermektedir. Her `RoomTemplateSO`, oyuncunun odaya giriş noktasını (güney spawn noktası, yürünebilirlik doğrulamalı) ve arka kenarda (back edge) üç adet çıkış soketi tanımlar: `door_NW_01`, `door_N_01` ve `door_NE_01`. Oyun evreninde "Rift portalı" olarak adlandırılan bu çıkış noktaları, kod mimarisinde tarihsel olarak `door_NW_01 / door_N_01 / door_NE_01` soket kimlikleriyle temsil edilir; rapor genelinde tasarım dili portal, kod düzeyi referanslar ise gerçek tanımlayıcı adlarıyla verilmiştir. Bu soketler yalnızca konuma değil, belirli validator kurallarına da tabidir: her soketin bulunduğu hücre yürünebilir ve kuzey komşusu yürünemeyen kenar tipi olmalı; portalın güney koridoru (2 hücre) yürünebilir kalmalı; ve soketler birbirinden en az 3 hücre ayrı olmalıdır.

Bu noktada portal yönü ile çıkış slotu arasındaki kavramsal ayrımı netleştirmek gerekir. Portal açı sayısı ve aktif çıkış slotu sayısı birbirinden bağımsız kavramlardır. Sistem üç çıkış soketi destekler (`door_NW_01/door_N_01/door_NE_01`); görsel üretim maliyetini sınırlamak için yalnızca iki authored portal açısı kullanılır: merkez N soketi için frontal kemer, NW soketi için açılı kemer. NE soketi ise açılı kemerin çalışma zamanında yatay aynalanmasıyla (flipX) elde edilir — ek asset üretimi gerektirmez. Graph 1 çıkış üretirse yalnızca N soketi aktif olur, 2 çıkış üretirse NW+NE (merkez boş, simetri korunur), 3 üretirse her üçü birden. Bu karar 8 yönlü portal asset üretimini kapsam dışında bırakarak maliyeti düşürür ve rota seçimini oyuncu için okunur tutar. Giriş noktası ise kalıcı bir portal objesi değil, güney kenarındaki `player_spawn_01` spawn anchor'ıdır.

Çalışma zamanında `BuildExitDoors` sistemi, o anın dungeon graph düğümünün kaç çocuk dalı olduğuna bakarak hangi soketlerin aktif hale geleceğini deterministik biçimde belirler: tek çıkış için yalnızca merkez N soketi, iki çıkış için NW+NE kanatları (merkez soketi boş bırakılarak simetri korunur), üç çıkış için her üç soket birden. Bu seçim kuralı hem asimetrik hem de düzensiz şekilli odalar için güvenli çalışır; çünkü soketlerin konumu template'e bake edilmiştir ve runtime'da geometriyle çakışma ihtimali yoktur. Herhangi bir template geçerli slot sayısını karşılayamazsa sistem bir geliştirici güvenlik ağı olarak devreye girer: bir uyarı yazarak durumu raporlar, editör validasyonu şablonun düzeltilmesini zorlar ve merkez-anchor geri dönüş düzeneği geliştirme sırasında sahne çökmesini önleyen fail-safe olarak işlev görür. Sistem hiçbir koşulda sessizce başarısız olmaz.

Bu sistemin tamamlanması sürecinde soket konvansiyonu oturularak sabitlendi, mevcut socket alanlarında sıfır şema değişikliği yapıldı. Validator 8 yeni kural aldı (konumsal geçerlilik, yürünebilirlik, komşu türü, mesafe kısıtları) ve projedeki 25 oda şablonu eski `door_W`/`door_E` soket adlarından NW/N/NE konvansiyonuna taşındı. Projedeki 26 küratörlü oda şablonunun 25'i run odasıdır ve gate-slot migrasyonuna dahil edilmiştir; Attunement Chamber (Chamber_CharSelect) karakter seçim akışına özel `riftExit` soketi kullandığından bu migrasyonun dışında tutulmuştur. Map Designer Rooms sekmesine kapı soketlerini önizleme etiketleriyle gösteren bir görünüm eklendi. Otomatik testler merkez-boş iki-kapı kuralını ve bileşen başlangıç sıralamasını (child-index assert) doğrulamaktadır; derleme sonrası bağımsız bir mimari inceleme yürütülerek sıfır hatayla sonuçlandı.

Portal görsel açısı da bu süreçte netleştirildi: çok-danışmanlı bir değerlendirme sonucunda N soketi için cepheden bakan portal kemeri formu, NW soketi için hafif açılı portal kemeri, NE soketi için NW'nin yatay eksen aynası (flipX) kararlaştırıldı. Bu üçlü sistem hem görsel tutarlılığı hem de izometrik derinlik algısını korur.

Bu yaklaşım, "authored veri + prosedürel seçim" kombinasyonunun somut bir örneğidir: tasarımcı (veya editör aracı) soketlerin nerede olduğunu belirler, run graph ise kaçının görüneceğini belirler. İki katman birbirinden bağımsız değişebilir ve bu da sistemin hem okunabilirliğini hem de genişletilebilirliğini artırır.

### 3.5.6 Rooms Sekmesi Oda Editörü ve UI-JSON Çift Yönlü Senkronizasyon

Map Designer'ın Rooms sekmesi, oda şablonlarının editör içinde doğrudan düzenlenmesini sağlayan 6 modlu bir toolbar içermektedir: Paint Walkable, Paint Void, Set Entry, Set NW, Set N ve Set NE. Tıkla-sürükle ile çalışan bu modların tamamı Unity'nin Undo sistemine entegre edilmiştir; her boya darbesi geri alınabilir.

Bu bölümde açıklanan editörün amacı yalnızca oda boyamak değil, veri kaynağı çakışmasını önlemektir. ScriptableObject oyun içi canonical kaynak, JSON ise dışa aktarım ve araç-entegrasyon formatıdır; görsel editör, importer ve test hattı aynı veriyi iki ayrı gerçeklik gibi taşımaz. Bu mimari tutarlılık ilkesi, oda verisinin birden fazla yerde tutulup çelişkiye düşmesi sorununu baştan engeller.

**Canonical kaynak kararı.** Bu sistemde yukarıda tanımlanan mimari ilke esas alındı: ScriptableObject dosyası her zaman tek ve geçerli kaynak konumundadır; JSON, dışa aktarım ve değişim formatı olarak tanımlandı (schema v2, Y-eksen çevirme düzeltmesiyle). Bu ayrım, bir oda verisinin birden fazla yerde tutulması ve çelişkiye düşmesi sorununu baştan önlemektedir.

Dışa aktarım otomatikleştirildi: her şablon düzenleme sonrasında yaklaşık 1 saniyelik bir debounce gecikmesinin ardından JSON'a yazılır; editör penceresi kapatılırken de bekleyen değişiklikler temizlenir (flush). Diff kontrolü sayesinde içerik değişmemişse dosya yazılmaz — aynı şablon üst üste iki kez dışa aktarıldığında sıfır dosya yazma gerçekleştiği doğrulandı. 26 oda şablonunun tamamının JSON çıktıları `STAGING/rooms_json/` altında tutulmaktadır.

`RIMA/Rooms/Export All JSON (v2)` menü komutu tüm şablonları tek tıkla dışa aktarır; v2 format import desteği de RoomJsonImporter'a eklendi. Round-trip testleri SO → JSON → SO dönüşümünün kayıpsız olduğunu doğrulamaktadır (bu davranış `RoomTemplateJsonRoundTripTests` ve `RoomTemplateSaveLoadTests` test sınıflarıyla doğrulanmıştır).

Bu çalışma sırasında RoomJsonImporter'da kritik bir hata da tespit edilip düzeltildi: importer, import işlemi sırasında hedef şablonun mevcut prop listesini siliyordu. Bu davranış, elle yerleştirilmiş prop verilerinin her import döngüsünde kaybolmasına yol açabilirdi.

Bağımsız bir inceleme üç küçük bulgu bildirdi: satır sonu standardının dosyalar arasında tutarsızlığı, sürükleme eyleminin tek Undo adımına sığdırılması gereksinimi ve props anahtarı içermeyen v2 dosyası yüklendiğinde mevcut prop'ların korunmaması. Bu bulgular bir mikro-düzeltme turuyla kapatıldı; düzeltme sonrasında tüm otomatik testler 65/65 sonuçla geçti.

### 3.5.7 Screenshot Modu

Raporlama ve sunum amacıyla tutarlı, tekrarlanabilir oyun içi görüntüler almak üzere bir `ScreenshotMode` aracı geliştirildi. ScreenshotMode yalnızca bir kamera preset yöneticisi değil, rapor ve sunum görsellerinden geliştirme arayüzü kalıntılarını arındıran bir sunum modudur. F12 tuşuyla açılan bu mod; 6 önceden tanımlı kamera preset'i sunar (F11/F10 ile döngüsel gezinme), HUD'lu ve HUD'suz varyant arasında geçişe olanak tanır ve ana menüye de entegre edilmiştir.

Sistemin en dikkat çekici özelliği deterministik prop seed'idir: aynı oda şablonu ile aynı seed değeri her seferinde piksel-piksel aynı prop kompozisyonunu üretir. Bu davranış imza eşitliği (signature equality) testiyle doğrulandı. Deterministik üretim, sunum ve akademik raporlarda kullanılan görüntülerin kaynağını bağımsız olarak yeniden üretilebilir kılmaktadır. Bu araç aracılığıyla 12 örnek kare üretildi.

### 3.6 Teknoloji Seçimi: Unity 6 ve URP 2D

#### 3.6.1 Neden Unity 6?

Bu projenin başladığı dönemde Unity 6, uzun dönemli destek (LTS) aşamasına yaklaşan ve kararlılık açısından güçlü bir seçimdi. Tek geliştirici için bu önemliydi: yeni sürümlerin getirdiği API kırılmalarıyla uğraşmak yerine mevcut araçlar üzerinde üretkenliği korumak öncelikti.

#### 3.6.2 Neden URP 2D?

Evrensel Render Pipeline'ın 2D modülü, projenin görsel hedefleri açısından doğal bir seçimdi. Birkaç temel gerekçe bu kararı şekillendirdi:

**Pixel-perfect kamera:** RIMA'nın tüm varlıkları piksel sanatı olarak üretilmektedir. URP 2D, piksel mükemmelliğini (pixel-perfect) yerleşik olarak destekler; bu sayede karakterler ve zemin karoları her çözünürlükte bulanmadan, doğru şekilde görüntülenir. Alternatif render pipeline'larında bu özellik ya yoktur ya da ek eklentiler gerektirir.

**2D ışık sistemi:** İzometrik yüzen ada görsel dilinde ışık kritik bir rol oynar. URP 2D'nin `Light2D` bileşeni, zemin üzerinde dinamik gölge ve parlama efektleri oluşturmayı mümkün kılar. Özellikle odaların tanımlayıcı rengi olan cyan enerji çizgileri üzerindeki parıltı, bu ışık sistemi aracılığıyla gerçek zamanlı olarak oluşturulmaktadır.

**Tilemap entegrasyonu:** Unity'nin `Tilemap` sistemi, URP 2D ile sorunsuz çalışır. İzometrik zemin için özelleştirilmiş hücre boyutları (0.96 × 0.585 birim) ve derinlik sıralaması (y-ekseni bazlı custom sort axis) doğrudan tilemap üzerinde tanımlanabilmektedir. Bu sayede karakterlerin zemin nesnelerinin arkasından veya önünden doğru biçimde geçmesi, ayrı bir shader veya manuel sıralama kodu gerektirmeden sağlanmaktadır.

Bu üç özelliğin bir arada bulunması, URP 2D'yi piksel sanatı izometrik oyunlar için pratik bir seçim haline getirmektedir. Mimari karar bu gerekçelere dayanmaktadır; başka bir pipeline'la da benzer sonuçlar elde edilebilirdi, ancak ek çalışma maliyeti söz konusu olurdu.

---

*Kelime sayısı: ~2.950*



---

<!-- KAYNAK: BOLUM_5_GORSEL.md -->

## 4. GÖRSEL ÜRETİM HATTI

### 4.1 Sanat Yönü

Görsel tasarım; izometrik yüzen adalar ve bunları çevreleyen mor tonlardaki boşluk (void) teması üzerine kurulmuştur. Bu nedenle alan izometrik yüzen taş adalardan oluşur; altında derinliği belli olmayan mor bir boşluk bulunur. Zemin rengi soluk veya açık gri değil, koyu slate/granit tonlarındadır. Koyu zemin, karakteri ve düşmanları zemine yapıştıran siluet okunabilirliği sağlar. Cyan (#00FFCC) enerji çatlakları ve rün parıltıları ise zinde odaları, karar noktalarını ve aktif objeleri işaret eder; ancak ekran alanının yalnızca yüzde beş ila sekizine hâkim olacak biçimde kullanılarak atmosferin baskın rengi korunur. Ada kenarlarından aşağı uzanan kalın koyu gri taş cliffler, boyuta derinlik verir ve sınırı oyuncuya sezdirir; kahverengi tonlar bu nedenle bilinçli olarak dışlanmıştır. Bu dil, referans görsel olarak izometrik konsept taslaklarından çıkarılmış ve belgelenmiş bir "sanat yönü kilidi" olarak proje başında sabitlenmiştir.

### 4.2 Karakter Sprite Üretimi: 10 Sınıf × 8 Yön

RIMA, on sınıflık bir kadro barındıracak şekilde tasarlanmıştır. Her sınıfın sekiz yönde (K, KD, D, GD, G, GB, B, KB) idle animasyonuna ihtiyaç duyduğu bölüm 2'de tespit edilmiştir. Toplamda 80 karakter sprite dosyası üretmek gerekmiştir. Bunun tamamını elle çizmek tek geliştirici için haftalarca çalışma anlamına gelirdi.

Çözüm olarak PixelLab AI platformundan yararlanılmıştır (bkz. Şekil 11). Ancak sekiz yönün tamamını bağımsız olarak üretmek verimli değildir: K, KD, D, GD ve G olmak üzere beş yön AI ile üretilmiş; GB, B ve KB ise bu yönlerin yatay eksende aynalaması (flipX) ile türetilmiştir. Bu karar, içerik maliyetini yaklaşık yüzde 37 oranında düşürmüştür; üstelik aynalama ile elde edilen yönlerde stil tutarlılığı doğal olarak korunmuştur.

[Şekil 11: Örnek bir sınıfa ait 8 yön sprite sheet — doğrudan üretilen ve aynalanan yönler işaretlenmiş]

Her sınıf için belirlenmiş canvas boyutları sınıfın silüet büyüklüğüne göre farklılık gösterir. Brawler, Elementalist ve Warblade için 120×120 piksel; Gunslinger, Hexer, Ravager, Shadowblade ve Summoner için 124×124 piksel; Ranger ve Ronin için 128×128 piksel kare canvas kullanılmıştır. Görünür karakter gövdesi her sınıfta yaklaşık 64 piksel uzunluğundadır; boşluk ise animasyon eylemleri sırasında karakterin canvas dışına taşmaması için tampon görevi görür.

Stil tutarlılığının korunması ayrı bir süreç gerektirmiştir. Her sınıf için PixelLab'e verilen prompt, ortaklaşa tanımlanmış bir "silüet standardı" belgesine dayanır; bu belgede her sınıfın beden oranı, silahlı/silahsız duruş niteliği ve ayırt edici görsel unsurları tanımlıdır. Üretilen her sprite görsel olarak denetlenmiş ve gerektiğinde prompt güncellenerek yeniden üretilmiştir. Proje, süreç boyunca üretilen tüm karakterleri tek bir kayıt altında takip etmiş; hangi sınıfın hangi yönünün hangi üretim oturumuna ait olduğu ilgili belgelere yazılmıştır.

### 4.3 Piksel-Art İçe Aktarma Disiplini

Grafik varlıklarının oyun içi kalitesini korumak amacıyla belirli içe aktarma kuralları belirlenmiştir. RIMA, her karaktere ve çevre asset'ine uygulanan içe aktarma sözleşmesini şöyle tanımlamaktadır:

- **Filtre modu:** Point (Nearest Neighbor) — piksel-art için bilinçli olarak seçilmiştir; GPU'nun kareler arasında enterpolasyon yapmasını engeller.
- **Piksel başına birim (PPU):** 64 — tüm sprite'larda ortak, tutarsız ölçekleme buglarını önler.
- **Sıkıştırma:** Yok — Unity'nin varsayılan lossy sıkıştırması piksel kenarlarını bozduğu için devre dışı bırakılmıştır.
- **Ölçekleme:** Tam sayı çarpanı — kesirli ölçekleme bulanıklaşmaya yol açtığından integer scaling zorunlu kılınmıştır.

Bu sözleşmenin pratikte önemi şudur: geliştirme sürecinde skill ikonlarının bir partisi Unity'e Bilinear filtre ile gelmiştir. Bu durum oyun içinde gözle görülür bulanıklaşmaya neden olmuş ve bir kalite kontrol geçişinde tespit edilerek etkilenen asset grubunun tamamı toplu düzeltmeyle Point filter'a çevrilmiştir. Bu vaka, sözleşmenin yalnızca kural değil, fiilen hata yakalayan bir mekanizma olduğunu göstermiştir.

Her karakter sprite'ının pivot noktası karakterin ayak tabanına yakın bir konuma ayarlanmıştır; bu sayede isometrik zemindeki konum hesaplaması ve derinlik sıralama Unity tarafından doğru biçimde gerçekleştirilir. Proje başında 82 rotasyon sprite'ının tamamı varsayılan 0.5 (merkez) pivot ile geldiğinden karakterler zeminde "havada asılı" görünüyordu. Bu sorunu gidermek için her sprite'ın alpha kanalı taranarak ayak hizası ölçüldü ve pivot o noktaya taşındı. Eş zamanlı olarak oyuncu ve 16 mob sprite'ı tek bir "Entities" sorting layer'ına alındı; `IsoSorter` bileşeni Y-eksenine dayalı derinlik sıralamasını çalışma zamanında yönetir. Editör tarafındaki ince ayarlar için `RIMA/Characters/Calibration` adında özel bir editör penceresi yazıldı; bu pencere, pivot ve sorting offset değerlerini tüm varlık grubu için toplu düzeltmeye imkân tanır.

### 4.4 Ortam Asset'leri ve İki Araç Arasındaki Ayrım

Karakter sprite'larında PixelLab kullanılması bilinçli bir karardır: bu platform iskelet bazlı tutarlı bir üretim süreci sunar ve stil referansı ile yeniden üretim arasındaki sürekliliği korur. Ancak karakterlere özgü bu tutarlılık ihtiyacı, tek renkli zemin karoları veya UI arka planları için o denli kritik değildir.

Bu nedenle ortam ve arayüz asset'leri için farklı bir araç tercih edilmiştir: Imagen (Google AI görüntü üretimi) tabanlı bir boru hattı, agy komutuyla entegre edilmiştir. Konsept oda görselleri, zemin ve duvar doku taslamaları, UI arka plan görüntüleri bu yolla üretilmiştir. Toplamda 115 PNG staging alanında birikmiş; bunların bir bölümü doğrudan asset olarak kullanılmış, bir bölümü ise stil referansı ve renk paleti kaynağı olarak değerlendirilmiştir.

İki aracın kullanım alanları birbiriyle örtüşmez biçimde ayrıştırılmıştır: karakter üretimi yalnızca PixelLab, ortam/UI üretimi yalnızca Imagen. Bunun nedeni teknik değil estetiktir; farklı üretim motorlarından gelen sprite'lar stil uyumsuzluğuna yol açabileceğinden, oyuncunun sürekli baktığı karakterler tek kaynakta tutulmuştur.

Her iki araçtan gelen asset'ler proje ekibine (tek kişi olsa da) teslim edilmeden önce görsel kalite kontrolünden geçirilmiştir. Bu kontrol; sınır keskinliği, arka plan şeffaflığı, palet tutarlılığı ve orantı denetimini kapsar. Animasyon üretimi ise proje süresince bilinçli olarak ertelenmiştir. Knockdown gibi hareketler, ek sprite gerektirmeksizin kod tabanlı eğme ve squash teknikleriyle gerçekleştirilmiş; bu karar bölüm 2'de ayrıntılı biçimde ele alınmıştır. Animasyon üretim kararları yalnızca kullanıcı onayıyla başlatılmakta, bağımsız olarak tetiklenmemektedir.

[Şekil 12: Attunement Chamber pedestal yakın çekimi — sınıf heykeli, [G] prompt'u ve bürünme etkileşimi | *(yeniden çekilecek)*]

Varlık üretiminde jeneratif araçlardan akademik prototip kapsamında yararlanılmış; üçüncü taraf ses varlıklarında CC0 lisans dosyaları projeye dahil edilmiştir.

Sonuç olarak görsel üretim hattı, üç ayrı disiplinin entegrasyonundan oluşmaktadır: sanat yönü kararları (atmosfer ve okunabilirlik dengesi), AI tabanlı üretim (PixelLab ve Imagen, ayrışık kullanım alanlarıyla) ve içe aktarma disiplini (Point/PPU 64/sıkıştırmasız sözleşme). Bu üç katmanın birlikte işlemesi, tek geliştirici tarafından tutarlı bir görsel dil oluşturulmasını mümkün kılmıştır.

---

*Kelime sayısı: ~1060*



---

<!-- KAYNAK: BOLUM_6_SUREC.md -->

## 5. YAPAY ZEKÂ DESTEKLİ ÇOK-AJANLI GELİŞTİRME METODOLOJİSİ

---

### 5.1 Çok-Ajanlı Geliştirme Sürecinin Gerekçesi: Kaynak Kısıtları

Bölüm 1'de ortaya konulan kaynak kısıtı — tek geliştiricinin programcı, tasarımcı, sanatçı ve QA rollerin tamamını üstlenmesi — bu bölümde ele alınan çok-ajanlı geliştirme sürecinin temel gerekçesidir. Bu süreç, bir dil modeline komut verip çıktısını doğrudan kabul etmekten farklıdır. Farklı rollere sahip ajanların belirli kurallara göre birbirleriyle etkileştiği, her kararın belgelendiği ve her çıktının bağımsız olarak doğrulandığı bir yazılım mühendisliği süreci tasarlanmış ve işletilmiştir [2]. Ajanlar araçtır; süreci tasarlayan ve yöneten ise geliştiricidir.

---

### 5.2 Sanal Ekip Yapısı

RIMA'nın geliştirme sürecinde dört temel rol tanımlanmış ve bu roller farklı araçlarla doldurulmuştur.

| Rol | Araç / Model | Ne Yapar |
|---|---|---|
| **Orkestratör** | Claude Sonnet (bu proje) | Görev dağıtımı, sıralama, sentez, nihai karar |
| **Yazılımcı Ajan** | Codex / cx dispatch | Kod yazma, Unity değişiklikleri, mekanik batch iş |
| **Danışman Konsey** | Gemini 3.1 Pro, Gemini 3.5 Flash, Opus | Tasarım kararları, mimari değerlendirme, risk analizi |
| **İnceleyici (Reviewer)** | Yazar'dan farklı ajan | Çıktı kalite kontrolü, hata yakalama |
| **Bilgi Tabanı** | NotebookLM | Tasarım kararlarının kalıcı, sorgulanabilir arşivi |

Sürecin temelini oluşturan yazar-denetçi ayrılığı ilkesi uyarınca: **bir ajandan çıkan iş, o ajanın kendisi tarafından onaylanamaz.** Kodu yazan Codex'in ürettiği iş, başka bir ajan tarafından incelenir; danışman konseyin önerdiği mimari karar, orkestratör tarafından sentezlenir ve kullanıcı onayına sunulur.

Bilgi tabanı rolünü üstlenen NotebookLM, projenin belleği olarak işlev görmüştür. Tasarım kararları, oyun mekanik tartışmaları ve sistem spesifikasyonları buraya senkronize edilmiş; her ajan büyük bağlam okumaya gitmek yerine bu tabanı sorgulamıştır. Bu, hem token tüketimini azaltmış hem de kararların oturum başından oturum sonuna tutarlı kalmasını sağlamıştır.

---

### 5.3 Süreç Kuralları: Geliştiricinin Tasarladığı Pipeline

Bu sürecin en özgün yönü, ajanların yeteneklerinden ziyade ajanları yönetmek için geliştirilen kurallardır. Bu kurallar, denemeler ve gözlemlenen hatalar üzerinden belgelenmiş ve proje boyunca zorunlu kılınmıştır.

**Görev dosyaları.** Her iş, bir görev belgesiyle başlar. Görev belgesi; neyin yapılacağını, hangi dosyalara dokunulacağını, neyin kesinlikle dokunulmayacağını ve başarının ne anlama geldiğini açık biçimde tanımlar. Ajan bu belgeyi alır, tamamlar ve bir "done" raporu üretir. Bu yapı, ajanı doğrulama yapılabilir bir taahhüde bağlar: ya tanımlanan kriteri karşılamıştır ya da "BLOCKED" yazar ve neden devam edemediğini raporlar. Sessizce yarım bırakılan iş kabul edilmez.

**Karar dökümanları.** Mimari, oyun tasarımı veya teknik bir konuda büyük karar verildiğinde, bu karar `STAGING/` klasörüne bir belge olarak yazılır. Kararın içeriği, hangi alternatiflerin değerlendirildiği, neden seçilmediği ve kararı onaylayan danışmanların görüşleri kayıt altına alınır. Bu belgeler; ilerleyen oturumlarda neden belirli bir yola gidildiğini açıklar ve aynı tartışmanın yeniden başlamasını önler.

**Yazar ile gözden geçiren (reviewer) rollerinin aynı ajanda birleşmemesi zorunludur.** Cross-review zorunluluğu, sürece gömülü bir ilkedir. Örneğin, Codex'in yazdığı knockdown (yere düşürme) sistemi, Opus modeliyle çalışan bir ax-Opus ajan tarafından incelenmiştir; aynı ax kanalıyla yazılan ölüm-decal bileşeni ise Codex tarafından gözden geçirilmiştir. İnceleme, yalnızca "kod çalışıyor mu" sorusunu değil, "mimari bozuldu mu, başka sistemlere zarar var mı" sorusunu da kapsar.

**Doğrulama kanıtı zorunludur.** Bir iş "bitti" sayılmadan önce Unity'de derleme hatası olmadığı, ilgili testlerin geçtiği ve oyun içinde elle doğrulandığı raporlanmak zorundadır. Yalnızca kod yazıldığını bildiren raporlar kabul edilmemektedir. Bu kural, pratikte defalarca değer kanıtlamıştır: birden fazla vakada testler veya oyun içi doğrulama, kod gözden bakışta gözden kaçan hataları ortaya çıkarmıştır.

**Unity'ye tek ajan kuralı.** Birden fazla ajan aynı anda Unity Editor üzerinde çalışırsa, sahne dosyaları birbirinin üzerine yazılır veya çakışan durum ortaya çıkar. Bu nedenle, Unity ile etkileşim gerektiren işler sıraya sokulur; paralel çalışmak için aynı sahne dosyasına dokunmayan işler seçilir ya da önce commit yapılır.

---

### 5.4 Vaka: 10-Task Otonom Gece Kuyruğu

Yukarıdaki kuralların nasıl bir araya geldiğini göstermek için projenin belki de en yoğun geliştirme gecesine bakmak yeterlidir.

Kullanıcı bir akşam belirli bir yönergeler bütünüyle geliştirme sürecini otonom bırakmıştır: konsey görevleri nasıl rotalamalı, doğru ajanlara vermelidir, işler cross-review'dan geçmelidir. Sabaha kadar şunlar gerçekleşmiştir:

- 10 görev bir karar belgesiyle tanımlanmış ve sıralanmıştır (`QUEUE10_ROUTING_DECISION_2026-06-05.md`).
- Görevler dört paralel lane'e (şeride) ayrılmıştır: Codex-A, Codex-B, ax (Gemini), Sonnet-MCP.
- Her lane, diğeriyle aynı sahne veya sisteme dokunmayacak biçimde seçilmiştir.
- ~11 commit üretilmiştir.
- 9 görev tamamlanmış, 2 görev tanımsız kaldığı için BLOCKED olarak kullanıcıya raporlanmıştır.

Bu gece içinde **iki önemli hata** review aşamasında yakalanmış ve yayına girmeden önce düzeltilmiştir: knockdown sistemindeki dokunulmazlık sızıntısı (`OnDisable` temizleme eksikliği — karakterin kalıcı hasar bağışıklığı kazanmasına neden oluyordu) ve aynı sistemdeki çifte direnç uygulaması (`resistancePreApplied` bayrağının tutarsız kullanımı). Her iki hata da "kod çalışıyor" bakışıyla fark edilmesi zor türdendi; bağımsız review bu vakaları erken yakalamıştır. Teknik kök neden analizi Bölüm 7'de ele alınmaktadır.

Gece boyunca dikkat çekici bir başka olay daha yaşanmıştır: bir ajandan kaynaklanan sahne restore işlemi, o sırada başka bir ajanın aynı sahneye uyguladığı değişikliği silmiştir. Bu, birden fazla ajanın aynı anda Unity sahnesi üzerinde çalışmasının neden kural ile engellendiğini somut olarak göstermiştir. Olayın ardından bu kural yazılı hale getirilmiş ve sonraki tüm oturumlarda uygulanmıştır.

---

### 5.5 Metodolojinin Değerlendirilmesi ve Sınırları

Sürecin güçlü ve yetersiz kaldığı alanları nesnel biçimde ortaya koymak metodolojik dürüstlüğün gereğidir.

Bu bölümde kullanılan "Reviewer-FAIL" terimi, kodu üreten ajandan bağımsız bir inceleme ajanının birleştirme öncesi gerçek hata tespit ettiği durumları ifade eder. Aşağıdaki tablo bu inceleme vakalarını ve sonuçlarını özetlemektedir.

| İnceleme vakası | Bulgu | Kritik örnek | Sonuç |
|---|---|---|---|
| Knockdown sistemi (mimari review) | 2 majör | i-frame immunity sızıntısı; çifte resistance uygulaması | birleştirme öncesi düzeltildi |
| Oda görsel QC (26 şablon) | 2 başarısız + 9 şüpheli | ada dışına taşan prop'lar | yerleştiricide sistemik doğrulama fix'i + yeniden denetim |
| UI-JSON editörü (statik review) | 3 minör | satır sonu standardı; sürükleme Undo birleştirme; props korunumu | mikro-fix turu, 65/65 test |
| Oyun hissi + SFX paketi | 9 bulgu, ilk teslim FAIL | giriş tamponu regresyonu; ambiyans ses sızıntısı; erişilebilirlik bayrağı bypass'ı | 9/9 düzeltildi, ikinci tur geçti |

**Yapay zekâ destekli test metodolojisine dair bir açıklama:** "Kodu yazan yapay zekâ, testleri de yazıyorsa bağımsızlık nasıl sağlanır?" sorusu bu sürecin en önemli metodolojik noktasıdır. RIMA'daki yaklaşım şöyledir: kabul kriterlerini, test mantığını ve sınır durumlarını tanımlamak geliştirici (bu çalışmanın yazarı) tarafından yapılmıştır; ajanlar yalnızca bu insan-tanımlı mantığı koda dökme işlemini üstlenmiştir. Ek olarak görsel QC süreci (Bölüm 6.3), otomatik testlerin kapsamadığı hataları insan gözlemiyle yakalayan bağımsız bir doğrulama katmanı olarak işlev görmüştür.

**Başarılı olan süreçler:** Mekanik kod üretimi bu sürecin en güçlü tarafıdır. Knockdown fizik sistemi, tooltip bağlantı noktası, SkillDatabase kaydı, checker zemin algoritması gibi teknik ama iyi tanımlanmış işler, kaliteli ve az review döngüsüyle tamamlanmıştır. Aynı şey toplu işler için de geçerlidir: 15 oda projesine özellik dağıtımı, 19 sprite ikonun import ayarlarının toplu düzeltilmesi, 10 sınıf veri kaydı gibi tekrarlı ama hacimli işler dakikalar içinde yapılmıştır. Test yazımı da verimli bir alan olmuştur; bir test matrisinin tasarımı danışman konseye bırakılmış, Codex testleri yazmış, Unity Test Runner'da 8 EditMode + 2 PlayMode test yeşil sonuç vermiştir.

**İnsan müdahalesi gerektiren alanlar:** Tasarım zevki ve oyun hissi hiçbir zaman ajanlara devredilememiştir. Karakterin ne kadar hızlı hareket etmesi gerektiği, knockdown animasyonunun hangi çerçevede ne hissettiği, bir oda düzeninin sıkıcı mı yoksa akılda kalıcı mı olduğu gibi kararlar yalnızca gerçek bir insan oyun oturumundan sonra verilebilmiştir. Görsel onay da bu kapsamdadır; ajanlar ekran görüntüsü üretip tanımlayabilir ama "bu güzel mi" sorusu kullanıcıya aittir. Nihai kararlar her zaman geliştiricide kalmıştır: ajan önerir, geliştirici onaylar veya reddeder.

**Yaşanan ajan hataları ve kurulan korumalar:** Sessiz başarısızlık en tehlikeli örüntüdür. Bir Codex profili hesap kotasına takıldığında, eski bir "bitti" dosyasını tekrar basıp hata vermeden sona ermiştir; bu durum bir görevin gerçekten tamamlandığı yanılgısına yol açmıştır. Bunun üzerine dispatch sonrası her DONE dosyasının zaman damgası kontrol edilmesi kuralı getirilmiştir. Benzer biçimde, bir ajan göreve verilmeyen dosyalara dokunmaya meyillidir; bu nedenle görev belgelerinde "sadece şu dosyalara dokunulacak, başka hiçbir şeye el sürülmeyecek" şartı açıkça yazılmaktadır. Birkaç vakada bu kural ihlal edilmiş ve elle geri alınmak zorunda kalınmıştır.

**Öğrencinin rolü: sistem mimarı ve son karar mercii.** Bu sürecin doğru anlaşılabilmesi için bir ayrım gereklidir. Ajanlar, söyleneni yapan araçlardır. Neyin yapılacağını, hangi sırayla, hangi kısıtlarla, hangi rolün ne zaman devreye gireceğini tasarlayan kişi geliştiricinin kendisidir. Orchestrator/reviewer ayrımı, karar belgesi zorunluluğu, Unity tek-ajan kuralı, cross-review matrisi bunların hiçbiri ajanlardan çıkmamıştır. Bunlar bir mühendislik sürecinin tasarım kararlarıdır ve bu kararlar, tıpkı oyunun kod mimarisi gibi, geliştirici tarafından oluşturulmuş ve zaman içinde rafine edilmiştir. Bu anlamda geliştirici, kodu satır satır yazan kişi olmaktan çok, süreci işler kılan sistemi kuran kişidir. Bu nedenle projenin katkısı yalnızca yapay zekâ kullanımında değil, yapay zekâ çıktısını denetlenebilir mühendislik adımlarına dönüştüren süreç tasarımındadır.

---

*Kelime sayısı: ~2150*



---

<!-- KAYNAK: BOLUM_7_DOGRULAMA.md -->

## 6. DOĞRULAMA: TEST VE KALİTE GÜVENCESİ

### 6.1 Tek Geliştiricinin Kalite Sorunu

Bir oyun projesinde test ve kalite güvencesi, ekip büyüdükçe doğal olarak dağıtılır: bir geliştirici yazan kodu başkası inceler, bağımsız bir test süreci hataları süzgeçler. Tek geliştiricili bir projede bu filtreleme mekanizması kendiliğinden ortaya çıkmaz. RIMA'da bu boşluğu kapatmak için üç katmanlı bir yaklaşım benimsenmiştir: otomatik birim ve entegrasyon testleri, sözleşme (contract) testleri ve sistematik görsel kalite güvencesi.

AI asistanlarının kod üretimde kullanıldığı bir bağlamda bu yaklaşımın önemi daha da artar. Otomatik olarak üretilen bir kod parçası derleniyor ve çalışıyor olabilir; ancak sistemin geri kalanıyla beklendiği gibi konuşup konuşmadığını ya da içerik üretim araçlarının çıktısının görsel olarak doğru olup olmadığını ayrıca doğrulamak gerekir. Test altyapısı bu doğrulama sürecinin omurgasını oluşturur.

### 6.2 Test Altyapısı

RIMA'nın test paketi, Unity Test Runner üzerinde çalışan EditMode ve PlayMode testlerinden oluşmaktadır. EditMode testleri, Unity sahnesi çalıştırılmadan derleme sonrası ortamda senkron biçimde koşar; play-mode probe'lar ise gerçek sahne yüklemesini, bileşen başlatmayı ve birden fazla kare boyunca süren asenkron etkileşimleri test etmek için tasarlanmıştır. Bunlara ek olarak sistematik davranış sözleşmelerini tanımlayan ve ayrı bir klasörde tutulan sözleşme testleri de mevcuttur.

Test klasörlerine göre kategoriler şöyle sıralanabilir: Bootstrap ve Wiring (bileşen bağlantılarının doğruluğu), Combat ve Encounter (savaş akışı ve dalga temizleme), Room ve MapDesigner (oda sistemi ve harita tasarımcısı araçları), Props ve Composition (nesne yerleştirme algoritmaları), RoomPainter ve Brush (boya katmanı araçları), Movement (oyuncu hareketi) ve UIFlow (arayüz akışı sözleşmeleri).

#### Tablo 6.1 — Test Envanteri ve Son Kayıtlı Koşu Özeti

**Envanter (2026-06-07 itibarıyla):**

| Ölçüt | Değer |
|---|---|
| EditMode test dosyası | 89 |
| PlayMode test dosyası | 12 |
| Bağımsız sözleşme dosyası | 4 |
| EditMode test tanımı | 508 |
| PlayMode test tanımı | 41 |
| **Toplam test tanımı** | **549** |

**Son kayıtlı koşu (TestResults_EditMode.summary.txt):**

| Ölçüt | Değer |
|---|---|
| Kayıtlı EditMode koşu | 410 PASS / 0 FAIL / 1 inconclusive |
| Walkable fizik grubu (Knockback + Walkable) | 16 + 16 = 32 test, tamamı yeşil |
| JSON round-trip grubu (SO → JSON → SO) | 5 test, tamamı yeşil |
| Screenshot deterministik imza testi | 1 test, yeşil |
| Gate-slot / kapı soketi grubu (merkez-boş + child-index) | yeni testler dahil, tamamı yeşil |

Kayıtlı koşu özetinde (TestResults_EditMode.summary.txt) 410 EditMode testi hatasız geçmiş (bkz. Şekil 13), yalnızca bir test belirsiz (inconclusive) olarak sonuçlanmıştır. Bu belirsiz test, Unity editörü dışında çalıştırıldığında erişilemeyen bir sahneleme bağımlılığını kapsamaktadır; işlevsel bir hata değildir. Test envanter sayısı (508 EditMode + 41 PlayMode = 549 toplam, 2026-06-07 itibarıyla) kayıtlı koşu tamamlandıktan sonra eklenen test gruplarını (walkable, round-trip, gate-slot, screenshot) içerdiğinden koşu toplamından büyüktür; tablodaki ek gruplar bu toplama dahil olmayan sonraki geliştirme turlarının çıktısıdır.

[Şekil 13: Unity Test Runner — 410 Pass / 0 Fail yeşil sonuç ekran görüntüsü]

### 6.3 Görsel Oda Kalite Güvence Süreci

Otomatik testlerin yakalayamadığı bir sorun sınıfı vardır: sahnede görsel olarak yanlış görünen, ama kod açısından geçerli sayılan durumlar. Bu problemi çözmek için RIMA'da programatik bir görsel QC süreci geliştirilmiştir.

#### 6.3.1 Sürecin Tasarımı

`RoomTemplateSO` varlıkları, RIMA'nın veri güdümlü oda sisteminin temel taşlarıdır. Her şablon bir odanın zemin hücrelerini, prop yerleşimlerini, kapı soket konumlarını ve oyuncu doğuş noktasını tanımlar. Projede 26 oda şablonu bulunmaktadır; bunların tümünün doğru inşa edilip edilmediğini kontrol etmek için editör modunda bir toplu QC koşusu hazırlandı.

Süreç şöyle işledi: her `RoomTemplateSO`, `_Arena` sahnesinde `IsoRoomBuilder` bileşeni aracılığıyla programatik olarak yeniden inşa edildi. Her inşanın ardından zemin karosu sayısı, uçurum (cliff) sprite sayısı ve prop sayısı otomatik olarak kaydedildi. Daha sonra Unity'nin editör görüntüsünden her oda için bir ekran görüntüsü alındı ve bu görüntüler `Assets/Screenshots/RoomQC/` klasörüne kaydedildi. 26 şablonun tamamı bu şekilde değerlendirildi.

#### 6.3.2 İlk QC Sonuçları: 15 Tamam / 9 Şüpheli / 2 Başarısız

Değerlendirme üç sonuçla tamamlandı:

**Tamam (15 şablon):** Küçük ve orta boyutlu koridorlar, basit dikdörtgen odalar ve özel amaçlı odalar (Spawn, Boss_Intro, Chamber_CharSelect) görsel olarak temizdi. Bu şablonlar dikkatli bir şekilde el ile oluşturulmuştu ve prop yerleştirme sorunları içermiyordu.

**Şüpheli (9 şablon):** Bu odalar oynanabilirdi ama görsel olarak hatalıydı. Birçoğunda prop grupları adanın dışında, havada asılı görünüyordu; bazılarında ise köşe kenarlarında fazla yoğun uçurum sprite'ları oluşmuştu.

**Başarısız (2 şablon):** `combat_large_diamond_01` ve `combatlarge_twin_basins_01`. İki farklı türde hata barındırıyorlardı (bkz. Şekil 14).

[Şekil 14: Oda QC örneği — combat_large_diamond_01 hatalı (solda, prop'lar adanın dışında) ve düzeltilmiş hali (sağda)]

#### 6.3.3 Kök Neden Analizi

İlk bulgu, 6 farklı şablonu aynı şekilde etkileyen sistematik bir prop yerleştirme hatasına işaret ediyordu. `combat_large_diamond_01`'de 31 prop'tan birkaçı adanın tamamen dışında, boşlukta duruyor; küçük bir kümesi havada asılı biçimde görünüyordu. Diğer etkilenen şablonlarda da benzer belirtiler gözlemleniyordu.

Analiz iki olası nedeni gündeme getirdi: ya bake edilmiş prop hücre koordinatları şablonun zemin hücre kümesinin dışına düşüyordu, ya da `IsoRoomBuilder` geçerli hücreleri yanlış dünya konumlarına yerleştiriyordu. İnceleme, sorunun bileşende değil, şablon verilerinde olduğunu ortaya koydu. Bridson-Poisson örnekleme geçişi sırasında otomatik doldurucu, adanın sınır bölgelerindeki bazı hücreler için koordinatları zemin hücre kümesinin dışına taşıyor; bu koordinatlar şablona bake edilirken hata orada kalıyordu. Sonraki `IsoRoomBuilder`, bu geçersiz koordinatı adanın dışındaki bir konuma dönüştürerek yerleştiriyordu.

`combatlarge_twin_basins_01`'deki ikinci hata farklı bir kök nedenden kaynaklanıyordu: iki havuzu birleştiren geçişteki tek hücrelik boşluk, uçurum çözücünün iç kenar olması gereken noktayı dış kenar olarak yorumlamasına neden oluyordu. Bunun sonucunda uçurum sprite'ları zemin katmanının tam ortasından geçerek odayı ikiye bölüyordu — görsel olarak koridor tamamen kullanılamaz hale geliyordu.

#### 6.3.4 Düzeltme Süreci

Her iki hata için ayrı düzeltme görevleri oluşturuldu. Prop hatasında çözüm yaklaşımı şuydu: şablona bake edilmeden önce her hücre koordinatının zemin hücre kümesinde gerçekten var olup olmadığı doğrulanacak; geçersiz koordinatlar budanacak ve etkilenen 6 şablon yeniden tohumlanacaktı. Uçurum hatasında ise şablon verisi doğrudan düzeltildi: iki havuz arasındaki geçiş bölgesindeki tek hücrelik boşluk kapatılarak uçurum çözücünün iç hücreleri dış kenar olarak yorumlaması engellendi. `Treasure_01` şablonu ise 14 hücreden 42 hücreye büyütüldü — orijinal boyutun bir karakter sprite'ının tamamını kaplayacak kadar küçük olduğu görüldüğünde bu değişiklik zorunlu hale gelmişti.

#### 6.3.5 Programatik Yeniden Doğrulama

Düzeltmelerin ardından her oda programatik olarak yeniden inşa edilerek doğrulandı. Kritik ölçüt şuydu: `propOutsideFloor` sayacı sıfır olmalıydı. Doğrulama çıktısı, etkilenen 9 şablonun tamamı için bu sayacın sıfıra düştüğünü gösterdi.

```
combat_large_diamond_01:  floor=212, builtProps=4, propOutsideFloor=0 → ok=True
combatlarge_twin_basins_01: floor=599, builtProps=1, propOutsideFloor=0 → ok=True
Treasure_01: floor=42, builtProps=4, propOutsideFloor=0 → ok=True
[RoomQCFix] PASS
```

QC süreci üç aşamada tamamlandı: ilk geçiş 15 şablonu temiz, 9 şablonu şüpheli, 2 şablonu başarısız olarak işaretledi; ardından tamir görevleri çalıştırıldı; son doğrulama geçişinde etkilenen tüm şablonlar için `propOutsideFloor=0` ve `ok=True` sonucu elde edilerek yapısal PASS onaylandı. Bu sürecin en önemli yan etkisi, gelecekteki oda doldurucu geçişlerini de engelleyecek kalıcı bir editör/üretim zamanı zemin doğrulama kuralının koda işlenmiş olmasıydı.

### 6.4 İkinci Vaka: Play-Mode Probe'un Yakaladığı Singleton Hatası

Play-mode probe koşusu, görsel QC'nin hemen ardından başka bir hata ortaya çıkardı: sahne geçişlerinde oda temizleme akışı sessizce kopuyordu. `Destroy(this)` yerine `Destroy(gameObject)` kullanan singleton koruma mantığı tüm "Systems" nesnesini siliyordu. Düzeltmenin ardından tam döngü play-mode'da doğrulandı. (Teknik kök neden analizi için bkz. Bölüm 7, §7.3.)

### 6.5 Sonuç: Üçlü Kalite Güvencesi

RIMA'nın doğrulama deneyimi, AI destekli kod üretiminde kalite güvencesinin tek bir yöntemle sağlanamayacağını ortaya koydu. Üç katmanın her biri farklı hata türlerini yakalar:

| Katman | Ne yakalar | Örnek |
|---|---|---|
| Otomatik test | mantık/sözleşme hataları | gate-slot seçim kuralları, JSON round-trip |
| Görsel QC | yapısal geçerli ama görsel yanlış durumlar | ada dışına taşan prop |
| Bağımsız inceleme | çalışan ama yanlış entegre kod | i-frame sızıntısı, giriş tamponu regresyonu |

Otomatik testler (89 EditMode + 12 PlayMode dosyası, 549 test tanımı envanteri; son kayıtlı EditMode koşusunda 410 PASS / 0 FAIL / 1 inconclusive) hız ve tekrarlanabilirlik sağlar; her commit sonrası saniyeler içinde temel sözleşmelerin bozulup bozulmadığı anlaşılır. Play-mode probe'lar, sistem sınırlarında ortaya çıkan ve düzlem testleriyle yakalanamayacak hataları — singleton yaşam döngüsü uyuşmazlıkları, sahne geçiş etkileri — gün yüzüne çıkarır. Görsel QC ise veri güdümlü içerik üretiminin yarattığı görsel sapmaları, programatik doğrulama raporuyla birleştirerek kanıtlanabilir bir sonuca ulaştırır.

Bu üç katmanın birlikte çalışması, tek geliştirici + AI üretimi kombinasyonunda kalite güvencesinin nasıl yapılabileceğine dair somut bir model sunmaktadır.

---

*Kelime sayısı: ~1690*



---

<!-- KAYNAK: BOLUM_8_ZORLUKLAR.md -->

## 7. KARŞILAŞILAN ZORLUKLAR VE ÇÖZÜMLER

Proje boyunca teknik, görsel ve süreç kaynaklı pek çok engelle karşılaşıldı. Bu bölümde, geliştirme sürecini en çok etkileyen beş vaka problem-teşhis-çözüm-ders formatında ele alınmaktadır.

---

### 7.1 Donanım-Sürücü Uyumsuzluğu: RTX 5080 ve Unity 6 D3D12 Çökmesi

**Sorun**

Geliştirme ortamının RTX 5080 ekran kartına taşınmasının ardından Unity Editor'da play-mode başlatılır başlatılmaz oyun kendiliğinden kapanıyordu. Çökme hatası herhangi bir hata ayıklama verisi elde etmeye fırsat vermeden gerçekleşmekteydi; sahne bile yüklenemeden sonlanıyordu ve bu durum tanı sürecini önemli ölçüde güçleştirdi.

**Teşhis**

Crash yığını incelendiğinde hata zincirinin `CheckDeviceStatus → D3D12CommandList::PrepareExecute → GfxTaskExecutorD3D12` üzerinden geçtiği görüldü. Bu, Windows'un GPU sürücüsünü "yanıt vermiyor" olarak işaretlediği ve zorla sıfırladığı bir TDR (Timeout Detection and Recovery) durumuna işaret ediyordu. Kök neden, Unity 6'nın varsayılan grafik arka ucunun Direct3D 12 olması ve bu arka ucun, piyasaya çok yeni girmiş RTX 5080 sürücüsüyle henüz tam uyumlu olmamasıydı.

**Çözüm**

`PlayerSettings` altındaki grafik API sıralaması değiştirilerek Direct3D 11 birincil konuma taşındı ve bu değişiklik bir commit olarak kaydedildi. Editör yeniden başlatıldıktan sonra `SystemInfo.graphicsDeviceType` değerinin `Direct3D11` döndürdüğü doğrulandı; play-mode çökmesi tamamen ortadan kalktı ve projenin geri kalanı bu yapılandırmayla sürdürüldü. Bu vaka, en yeni nesil donanım ile güncel motor versiyonu kombinasyonunda düşük düzeyli uyumsuzlukların ortaya çıkabileceğini; geliştirme ortamı değiştiğinde ilk adımın varsayılan ayarların uygunluğunu sorgulamak olması gerektiğini göstermektedir.

---

### 7.2 İzometrik Uçurum (Cliff) Taşması: Görsel Hatada Kök Neden Aramak

**Sorun**

İzometrik ada haritaları oluşturulurken ada kenarlarına otomatik yerleştirilen uçurum sprite'ları (cliff tiles) zemine taşıyordu. Özellikle güney ve güneydoğu köşelerinde kümüksü, perde gibi görünen yoğun bir sprite yığılması oluşuyordu; bu durum oyun dünyasının derinlik algısını bozuyor ve adanın altının "kirlenmesine" yol açıyordu.

**Teşhis**

İlk müdahaleler, sprite boyutunu küçültmek veya z-sıralamasını düzenlemek gibi yüzeysel yamalar oldu; ancak hiçbiri sorunu kalıcı biçimde çözmedi. Sorunun gerçek kaynağı, uçurum yerleştirme algoritmасının yön bilgisinden yoksun olmasıydı. Algoritma her boşluk komşusu olan hücreye ayrım gözetmeksizin bir uçurum sprite'ı yerleştiriyordu. Oysa doğrudan güneyde (SW/SE) boşluk olan hücreler için derinlik algısını koruyan "içeri kıvrılma" (inward tuck) davranışı gerekirken, algoritma bunları diğer yönlerle aynı şekilde işliyordu. Sonuç: güney cephesinde sprite'lar görünür zemine taşıyordu.

**Çözüm**

Uçurum yerleştirme sistemi yön-bazlı komşu kontrolüne göre yeniden yazıldı. Her hücre için çevreleyen sekiz yönün void (boşluk) mı yoksa zemin mi olduğu ayrı ayrı sorgulandı; SW ve SE yönünde boşluk bulunan hücreler "içeri tuck" vektörüyle yerleştirildi. Bu sayede güney cephesindeki sprite'lar artık adanın gövdesi içine çekildi ve zemin düzlemine taşmadı. Bu vaka, görsel hata tespitinde yüzeysel yamalardan (sprite boyutu, katman sıralaması) önce algoritmanın hangi koşullarda tetiklendiğinin belirlenmesinin uzun vadede daha sağlam sonuç verdiğini ortaya koymuştur.

---

### 7.3 Singleton Guard'ının Sistemi Sessizce Kesmesi

**Sorun**

Sahne geçişleri sırasında oda temizleme akışı çalışmıyordu: düşmanların tamamı ortadan kaldırıldıktan sonra ne ödül beliriverdi ne de bir sonraki odaya geçişi sağlayan kapılar aktif hale geldi. Sistem herhangi bir hata mesajı üretmiyordu; oyun sessizce kilitleniyordu.

**Teşhis**

İncelemede `Systems` adlı merkezi GameObject'in sahne yüklenirken yok edildiği görüldü. Bu nesne üzerindeki MonoBehaviour'lardan biri yineleme önleme amacıyla klasik bir singleton koruması (`DontDestroyOnLoad` + kopya kontrolü) içeriyordu. Sorun şuydu: kopya tespit edildiğinde koruma `Destroy(this)` yerine `Destroy(gameObject)` çağırıyordu. Bu yüzden sadece fazladan olan bileşen değil, üzerinde yaşam döngüsü yöneten tüm bağımlı bileşenler de dahil olduğu halde `Systems` nesnesi tamamen siliniyordu. Oda geçiş koordinatörü de bu GameObject üzerinde yaşadığı için her geçişte akış sessizce kopuyordu.

**Çözüm**

Singleton koruma mantığı yeniden yazıldı: `Destroy(gameObject)` yerine `Destroy(this)` kullanılarak yalnızca fazla olan bileşenin kendisi yok edildi. Böylece nesnenin diğer bileşenleri sağlıklı kalmaya devam etti ve oda geçiş akışı beklenen şekilde çalışır hale geldi. `Destroy(this)` bileşen kapsamında, `Destroy(gameObject)` ise nesne kapsamında çalıştığından, paylaşılan bir GameObject üzerinde singleton koruması uygulanırken hangi kapsamın hedeflendiği açıkça belirlenmeli ve belgelenmelidir.

---

### 7.4 Yapay Zeka Ajan Güvenilirliği: Sessiz Başarısızlık ve Çakışan Değişiklikler

**Sorun**

Geliştirme sürecinin belirli bir aşamasında birden fazla yapay zeka ajanı (cx kod ajanı ve çeşitli Gemini ax çağrıları) paralel veya arka arkaya çalıştırılmaktaydı. İki farklı güvenilirlik sorunu ortaya çıktı.

Birincisi "sessiz başarısızlık": bir kodlama görevi gönderilen ajan kota sınırına takıldığında hata fırlatmak yerine bir önceki çalışmadan kalan tamamlandı bildirimini olduğu gibi bırakıyor ve çıkıyordu. Sonuç, istenilen değişiklik hiç yapılmadan "bitti" raporu alınmasıydı.

İkincisi "geri alma çakışması": bir ajan doğrulama yaparken sahneyi kendi referans durumuna geri yüklüyor, bunu yaparken bir başka ajanın aynı sahneye yazdığı değişiklikleri siliyordu.

**Çözüm**

Her görev gönderiminin ardından tamamlandı bildirim dosyasının zaman damgası, görev başlangıç zamanıyla karşılaştırılarak eskiye ait bir bildirim olup olmadığı kontrol edilmeye başlandı. Sahne dosyası içeren işler öncesinde değişiklikler commit'lenerek kayıt altına alındı, böylece herhangi bir ajan geri yüklemesi commit geçmişinden öteye geçemedi. Sahnede eş zamanlı değişiklik yapabilecek ikinci bir ajan çalıştırmamak için tek-Unity-ajanı kuralı benimsendi. Bu kurallar belgelenerek ilerleyen görev kuyruklarında otomatik olarak uygulanmaya başlandı. Yapay zekâ ajanlarından oluşan pipeline'da güven sözlü teminata değil doğrulama mimarisine dayanmalıdır; özellikle geri dönüşü zor işlemlerde (sahne değişikliği, silme, commit) ajan sonucunu bağımsız bir yolla teyit etmek vazgeçilmez bir adımdır.

---

### 7.5 Görsel Kalite Güvencesi Açığı: Yapısal "PASS" Görsel "Başarısızlık"

Prop dağıtım algoritması zemin hücreleri yerine bounding-box içinde koordinat ürettiğinden, yapısal testler geçer görünürken birden fazla odada prop'lar adanın dışında boşlukta asılı kalıyordu. Çözüm olarak her yerleşimde hedef hücrenin geçerli zemin kümesinde (`LastFloorCells`) bulunduğu doğrulanmaya başlandı ve sistematik ekran-görüntüsü tabanlı QC süreci kuruldu. (Ayrıntılı süreç, tespit edilen 2 FAIL + 9 şüpheli oda ve yeniden doğrulama çıktısı için bkz. Bölüm 6, §6.3.)

---

*Kelime sayısı: yaklaşık 1.310*



---

<!-- KAYNAK: BOLUM_9_SONUC.md -->

## 8. YOL HARİTASI VE SONUÇ

---

### 8.1 Ulaşılan Yer

Bu projenin başındaki soruyu hatırlatmak gerekir: tek geliştirici, kapsamlı bir aksiyon-roguelite nasıl yapılabilir?

Proje bu soruya iki paralel yanıtla başladı: içerik ve sistemleri birbirinden ayıran veri-güdümlü bir mimari, ve bu mimarinin üzerinde çalışan çok-ajanlı bir geliştirme süreci. Raporun sonunda bu yanıtların pratikte ne ürettiğine bakmak yerinde olur.

RIMA, oynanabilir tam döngüye sahip çalışan bir oyundur. Ana Menü'den başlayan, yürünebilir Attunement Chamber'da karakter seçimiyle devam eden, oda oda combat ve 3-kart skill draft içeren, dallanan kapılarla şekillenen ve boss karşılaşmasıyla kapanan bir run döngüsü — bunların tamamı birbirine bağlı ve çalışır durumdadır. Projenin somut çıktıları şu biçimde özetlenebilir:

- **26 oda şablonu** — 15 dışarıdan temin edilmiş, tamamı görsel QC'den geçirilmiş
- **10 sınıflık veri altyapısı** — her biri farklı kaynak ekonomisi ve oynanış diliyle tanımlanmış; demo kapsamında 4 sınıf uçtan uca oynanabilir, kalan 6 aynı pipeline üzerinden eklenmeye hazır
- **549 test tanımı envanteri** (508 EditMode + 41 PlayMode) — son kayıtlı EditMode koşusunda 410 PASS / 0 FAIL / 1 inconclusive
- **80 karakter sprite** — PixelLab ile üretilmiş, Point/PPU64 import sözleşmesiyle
- **8-sekmeli Map Designer** (Rooms sekmesi UI-JSON editörü dahil), Room Browser/RoomJsonImporter, ScreenshotMode araç zinciri
- **Ses katmanı:** 18 CC0 klip, 9 SFX kanalı, sahne-geçiş-güvenli ambient sistemi
- **Oyun hissi katmanı:** hit-pause tier'ları, infaz dünya-prompt'u, dash giriş tamponu, FeelToggle erişilebilirlik bayrağı
- **Tam oynanabilir döngü:** MainMenu → Attunement Chamber → Combat → Skill Draft → Boss → Victory/Death → Meta-ilerleme

Araç zinciri de bağımsız bir çıktı olarak değerlendirilebilir. Map Designer, Room Browser ve RoomJsonImporter; yeni içerik eklemeyi, test etmeyi ve görsel olarak doğrulamayı elle çalışmaktan çok daha az maliyetli bir hale getirdi. Bu araçlar olmadan 26 oda şablonunun tamamını üretmek, QC sürecinden geçirmek ve yeniden doğrulamak mümkün olmazdı.

Metodoloji açısından bakıldığında, çok-ajanlı geliştirme sürecinin yalnızca "AI kullanmak" olmadığı pratikte gösterildi. Rol tanımları, görev belgeleri, karar dökümanları, cross-review zorunluluğu ve doğrulama kanıtı gerekliliğinden oluşan bu süreç, geliştirici tarafından tasarlandı ve işletildi. Ajanlar araçtı; süreç ise bir mühendislik kararıydı.

---

### 8.2 Yol Haritası: Sırada Ne Var?

Projenin mevcut durumu oynanabilir bir prototip seviyesindedir. Bir ticari sürüme ulaşmak için tamamlanması gereken işler birkaç ana başlık altında toplanmaktadır.

**Silah üretimi ve sınıf derinleştirme.** Warblade sınıfının silahı (Warblade) zaten uçtan uca çalışır durumda. Kalan dokuz sınıfın her biri için benzersiz bir silah üretimi, hem sprite hem de mekanik açısından tamamlanması gereken en büyük içerik bloğudur. Paralel olarak on sınıfın her birindeki placeholder skill'lerin gerçek implementasyonla doldurulması, build derinliğini ve tekrar oynama değerini doğrudan etkiler.

**Elementalist büyü efektleri (VFX).** Büyü sınıflarının görsel kimliği için tasarlanan efektlerin particle sistemi ve shader düzeyinde üretimi, görsel kalite açısından bir sonraki önemli adımdır. Bu iş kod ve araç altyapısı hazır olduğu için beklemektedir; üretim kararları kullanıcı onayıyla başlatılacaktır.

**Ses ve müzik.** Demo kapsamında temel SFX geri bildirimi CC0 lisanslı kliplerle entegre edilmiştir; 18 adet CC0 lisanslı ses klibi ve 9 SFX kanalı bağlanmış, sahne geçişlerinde ambient ses sızıntısını önleyen yaşam-döngüsü koruması da bu aşamada eklenmiştir. Özgün müzik, sınıfa özgü prodüksiyon sesleri ve adaptif müzik katmanları gelecek çalışma kapsamındadır. Bu alan, prototip kalitesinden çıkma sürecinin görünmez ama kritik bileşenlerinden biri olmaya devam etmektedir.

**Portal görsel entegrasyonu ve boss geliştirme.** Kapı soket sistemi ve görsel açı kararları tamamlandı; bir sonraki adım portal/Rift portalı sprite'larının oyun içi görünümüne tam entegrasyonu ve mevcut boss arenasının görsel ile mekanik açısından derinleştirilmesidir. Demo kapsamında boss düğümüne ulaşan ve temel saldırı/zafer akışını tamamlayan bir boss karşılaşması bulunmaktadır; daha zengin faz yapısı ve telegraph çeşitliliği yol haritasına bırakılmıştır. Act 2 ve sonrasını mümkün kılmak için yeni boss tasarımları, biyom çeşitliliği ve artan zorluk eğrisi gerekecektir. Bu, projenin içerik derinliği açısından en uzun vadeli çalışmasıdır.

**Denge ve playtest turları.** Skill ağırlıkları, tier dağılımı, düşman can değerleri ve oda zorluk etiketleri şu an tasarım kararına dayalı başlangıç değerlerine sahiptir. Gerçek oyuncu verisine dayalı denge turları olmadan bu sayıların ne kadar doğru oturduğu bilinemez. Bu çalışma bir kez değil, her büyük içerik eklemesinin ardından tekrarlanması gereken bir süreçtir.

**Steam yayın hazırlığı.** Mağaza sayfası, sistem gereksinimleri, erişilebilirlik seçenekleri, Steamworks entegrasyonu ve Early Access yayın planlaması; oyunun teknik tamamlanmasından bağımsız olarak kendi zamanını ve dikkatini gerektiren ayrı bir iş kümesidir.

---

### 8.3 Çıkarılan Dersler

Bu proje boyunca hem teknik hem de süreç boyutunda bazı şeyler beklendiği gibi gitmedi ve bu sapmaların her biri öğretici oldu.

**Mimari kararları erken kilitlemek zaman kazandırır.** Oyunun en büyük teknik yeniden yapılanması, sahne-bazlı oda anlayışından veri-güdümlü mimariye geçiş oldu. Bu karar geç alınsaydı, o noktaya kadar üretilmiş içeriğin büyük bölümü yeniden yazılmak zorunda kalırdı. Mimari seçimler küçük projede bile sonradan pahalı hale gelebilir; erken kilitlenmesi gereken şeyler erken belirlenmeli.

**Testler "kod çalışıyor mu" sorusundan daha fazlasını sormak zorundadır.** Bir sistemin yapısal testleri geçmesi, görsel olarak doğru göründüğü anlamına gelmez. 26 oda şablonundan birçoğu, birim testlerde geçer görünürken prop'ları adanın dışında bırakıyordu. Görsel çıktı içeren projelerde test altyapısının da görsel doğrulama katmanı içermesi gerekir.

**Yapay zekâ ajanlarına güven, teyide dayanmalıdır.** "Ajan başardı dedi" ifadesi, gerçek çıktı incelenmeden kabul edilmemelidir. Sessiz başarısızlık — ajanın hata vermeden eksik iş bırakması — pratikte birden fazla kez yaşandı. Geri dönüşü zor işlemlerde (sahne değişikliği, dosya silme, commit) ajanın raporunu bağımsız bir yolla doğrulamak vazgeçilmez bir adımdır.

**Tasarım zevki ve oyun hissi devredilemez.** Kod üretimi, toplu içerik oluşturma ve hata yakalama için çok-ajanlı süreç net bir değer üretti. Ama bir odanın sıkıcı hissettirip hissettirmediği, bir knockdown animasyonunun yeterince ağır görünüp görünmediği ya da bir skill seçiminin gerçekten anlamlı gelip gelmediği — bu kararlar yalnızca gerçek bir insan oyun oturumundan sonra verilebilir. Geliştirici, son karar mercii olarak her zaman sürecin içindeydi.

---

### 8.4 Değerlendirme

Bu proje; veri-güdümlü mimari, çok-ajanlı geliştirme süreci ve programatik kalite güvencesinin tek geliştirici ölçeğinde nasıl uygulanabileceğini belgelemektedir. Oynanabilir tam döngü, 10 sınıf, 26 oda şablonu ve 549 test tanımı envanteri (son kayıtlı koşu 410 PASS) ile ulaşılan kapsam, benimsenen mimari ve metodolojik kararların somut çıktısıdır. Projenin sürdürülmesi ve ticari kaliteye taşınması için gerekli yol haritası Bölüm 8.2'de özetlenmiş olup temel mühendislik altyapısı bu çalışmanın kapsamı dahilinde tamamlanmıştır.

---

*Kelime sayısı: ~850*

---

---

## Kaynakça

[1] Bridson, R. (2007). Fast Poisson disk sampling in arbitrary dimensions. *SIGGRAPH Sketches*, 22.

[2] Qian, C., Liu, W., Liu, Y., Chen, W., Yao, Y., & Zhou, Z. (2023). ChatDev: Communicative Agents for Software Development. *arXiv preprint arXiv:2307.07924*.

[3] Shaker, N., Togelius, J., & Nelson, M. J. (Eds.). (2016). *Procedural Content Generation in Games*. Springer. (Erişim: pcgbook.com)

[4] Schreiber, I., & Romero, B. (2012). *Game Design Concepts: An Introduction to Game Design and Game Analysis*. CRC Press.

[5] Supergiant Games. (2021). *Hades: God Mode and the Design Philosophy Behind It* [GDC Talk]. Game Developers Conference.

[6] Unity Technologies. (2017). *ScriptableObject Architecture in Practice* [Unite Talk]. Unite Europe 2017.

[7] Adams, E., & Dormans, J. (2012). *Game Mechanics: Advanced Game Design*. New Riders.

---

## Şekil Listesi

| No | Açıklama | Önerilen dosya (STAGING/report_screenshots/) |
|---|---|---|
| Şekil 1 | Attunement Chamber genel görünümü — pedestal'lar ve sınıf silüetleri | 02_chamber_overview.png |
| Şekil 2 | [G] tuşuyla bürünme anı — cyan enerji efekti | 03_chamber_g_prompt.png |
| Şekil 3 | Tipik bir combat odası — checker zemin + prop düzeni + aktif düşman grubu | 05_combat.png |
| Şekil 4 | 3-kart skill draft ekranı — tooltip açık, sinerji chip'i görünür | 06_draft_cards.png |
| Şekil 5 | Çift veya üçlü kapı çıkışı — oda türü simgeleri görünür | 07_branching_doors.png |
| Şekil 6 | Warblade karakteri — omuz zırhı ve iki elli kılıç silueti | *(önerilecek dosya yok — üretilecek)* |
| Şekil 7 | IsoRoomBuilder çalışma zamanında örnek oda — zemin + cliff + prop'lar | 04_run_room_overview.png |
| Şekil 8 | JSON → RoomTemplateSO akış şeması | *(diyagram — üretilecek)* |
| Şekil 9 | Map Designer 8-sekmeli pencere görünümü | 11_map_designer.png |
| Şekil 10 | Room Browser + _Arena'da kurulu oda | 12_room_browser_scene.png |
| Şekil 11 | Örnek sınıfa ait 8 yön sprite sheet — doğrudan üretilen ve aynalanan yönler | *(sprite sheet — üretilecek)* |
| Şekil 12 | Attunement Chamber pedestal yakın çekimi — sınıf heykeli, [G] prompt'u ve bürünme etkileşimi | *(yeniden çekilecek)* |
| Şekil 13 | Unity Test Runner — 410 Pass / 0 Fail yeşil sonuç | *(ekran görüntüsü — üretilecek)* |
| Şekil 14 | Oda QC örneği — combat_large_diamond_01 hatalı ve düzeltilmiş hali | *(QC ekran görüntüsü — üretilecek)* |

