# Bölüm 3: Oyun — RIMA ve Oynanabilir Döngü

---

## 3.1 RIMA Nedir?

RIMA, 2D izometrik bir aksiyon-roguelite oyunudur. Oyuncuyu eşsiz güçlere ve birbirinden tamamen farklı kaynak ekonomilerine sahip on farklı savaşçı sınıfından birini seçerek, procedural olarak ilerleyen bir zindan koridorunda oda oda ilerlemeye, her geçişte yeni yetenekler edinerek daha güçlü bir build inşa etmeye davet eder. Oyunun temel döngüsü seçim, savaş ve büyüme üçgenine oturur: hangi yeteneği alacağınız kadar hangi odaya gireceğiniz de önem taşır; başarısız bir run bile ileriki oturumlar için kalıcı bir kazanım bırakır.

Oyunun dünyası ve hikâyesi, "The Fracturing" (Büyük Kırılma) adı verilen kozmik bir felaketin ardından şekillenir. Dünyalar arasında yayılan ve geri döndürülemez bir tüketim tehdidi olan "Rift March"ı durdurmak için "The Architect" adlı varlık, boyutlar arasındaki tüm bağlantıları tek elden, tek anda ve geri dönüşsüz biçimde koparmıştır. Hesap yapılmış, bedel kabul edilmiştir; ama bedel beklenenin çok üstüne çıkmıştır. Dünya, altlarında sonsuz bir kozmik hiçliğin (Void) uzandığı, kenarları paramparça edilmiş yüzen taş adalara dönüşmüştür; mermer sütunlar havada asılı kalmış, taş merdivenlerin yarısı boşluğa düşmüştür. Dungeon'ın mimarisi kasıtlı değil, kırılmıştır: her oda hâlâ bir amaca hizmet etmiş eski bir yapının kalıntısıdır, ama kenarları yırtılmıştır.

Oyuncu bu yıkımın hem bir faili hem de hayatta kalan bir kalıntısıdır. Elinde tuttuğu kimlik parçacıkları — "Shattered Echo"lar — aynı zamanda oyunun meta-ilerleme para birimidir. Hikâyesel olarak bu parçacıklar, The Fracturing sırasında dağılmış kendi kimliğinizin unutulmuş yüzlerini temsil eder: yeni bir sınıfın kilidini açmak, kayıp bir yüzü yeniden giyinmek anlamına gelir. Oyunun tonal manifestosu "Vivid Vulnerability" (Canlı Kırılganlık) olarak tanımlanmaktadır; bu ifade, oyunun kaybı ne avutucu ne de hafife alan, bunun yerine her anın ağırlığını sessizce taşıyan melankolik bir atmosferi simgeler. Yıkımın ortasında neon cyan enerjisinin çatlaklar arasında parlaması bu tonu görsel düzeyde pekiştirir: tehlikeli ve dengesiz Rift enerjisi ile onu zapt etmeye çalışan çatlayan antik mühürler, oyunun renk dilinin çekirdeğini oluşturur.

---

## 3.2 Referans Oyunlar: RIMA Ne Alıyor, Ne Koyuyor?

RIMA, roguelite türünün birbirinden farklı üç önemli temsilcisini referans almakta ve bunların unsurlarını kendi tasarım diline çevirmektedir. Bu referanslar doğrudan kopya değil; her birinden belirli bir tasarım kararı alınmış, RIMA'nın özgün bağlamına uyarlanmıştır. Aşağıdaki tablo bu konumlanmayı özetlemektedir.

| Kriter | Hades | Dead Cells | Slay the Spire | **RIMA** |
|---|---|---|---|---|
| **Oda akışı** | Sabit oda seti, sırayla gelir | Procedural oda + biyom geçişi | Sabit düğüm, sırayla | Procedural dungeon graph, 12 düğüm + boss, her run yeniden üretilir |
| **Harita dallanması** | Sınırlı çatallanma | Her odadan birkaç yol, biyom seçimi | StS-tipi ağaç harita, tip etiketleri | 1–3 dallı kapılar; her kapı farklı oda türünü işaret eder |
| **Build oluşturma** | Her odada silah veya destek seçimi | Silah + rune build, geçici yükseltme | 3-kart deck draft, sinerji odaklı | Her 3 odada 1 kez 3-kart skill draft; tier derinlik kilitleri |
| **Karakter seçimi** | Statik menü + arka plan karakterleri | Statik menü | Statik menü | Yürünebilir "Attunement Chamber" — diegetic mekân |
| **Meta-ilerleme** | Darkness + hediye sistemi, run arası | Scroll of Prowess, modifiye kalıcı | Yok (saf roguelite) | Shattered Echo para birimi; yeni sınıf kilidi açma |

Hades'in bu listedeki en belirgin katkısı, karakter seçimini bir menüden çıkarıp oyun dünyasının bir parçası haline getirmesidir. Hades'te Zagreus'un çıkış öncesi Yeraltı sarayında dolaşması ve karakterlerle konuşması nasıl dünyayla bütünleşmiş bir deneyim sunuyorsa, RIMA'nın Attunement Chamber'ı da aynı diegetic yaklaşımı izler; fark olarak oyuncu burada karakterleri dinlemek yerine bedenlerine fiziksel olarak bürünür.

Dead Cells'in katkısı daha çok tempo anlayışında hissedilir: aksiyon combat'ın roguelite ilerleme döngüsünün tam merkezine yerleşmesi, her ödülün bir sonraki zorluk için hazırlık anlamı taşıması. RIMA da combat'ı yalnızca bir engel olarak değil, build kararlarının test edildiği asıl sahne olarak tasarlar.

Slay the Spire'ın etkisi ise en çok skill draft sisteminde görülür. Derinlik kilitleri — nadir yeteneklerin ancak run'un ilerleyen bölümlerinde görünmesi — bu oyunun tasarım mantığından doğrudan alınmış bir karardır. Run'un ritmi böylece, başlangıçta geniş bir temel kurulması, ortada sinerji noktalarının bulunması ve sonda gerçek güç sıçramalarının gerçekleşmesi şeklinde üç akt gibi hissedilir.

---

## 3.3 Tam Oynanabilir Döngü Anlatısı

RIMA'da bir run, mantıksal ve deneyimsel olarak birbirini izleyen birkaç aşamadan oluşur. Geliştirme sürecinin bu bölümdeki en önemli başarısı, bu aşamaların tamamının çalışır durumda ve birbirine bağlı olmasıdır: oyuncu gerçekten MainMenu'dan başlayıp Victory ya da Death ekranına ulaşabilen, aralarında geçiş yapabilen ve biriktirdiği meta-parayı bir sonraki run'da harcayabilen eksiksiz bir döngüyü deneyimleyebilmektedir.

### Ana Menü

Oyuncu, oyunu başlatınca karşılaştığı ana menü; yeni bir run başlatmayı, sınıf codex'ini açmayı ve ayarları yönetmeyi sunar. Menü aynı zamanda oyunun görsel tonunu kurar: koyu arka plan, ölçülü cyan vurgular ve sınıf ikon tasarımları. Codex ekranı, oyuncunun mevcut oturumda açılmış tüm skill'leri sınıf bazlı olarak incelemesine olanak tanır; bu sayede henüz run başlamadan mümkün kombinasyonlar keşfedilebilir ve bir strateji oluşturulabilir. "Oyna" seçeneği oyuncuyu doğrudan Attunement Chamber'a taşır.

### Attunement Chamber — Yürünebilir Karakter Seçimi

[GÖRSEL: Attunement Chamber genel görünümü — pedestal'lar ve sınıf silüetleri]

RIMA'nın karakter seçim ekranı, pek çok roguelite oyununun aksine statik bir menü değildir. Oyuncu, son oynadığı sınıfın bedeninde gerçek bir odaya spawn olur ve WASD tuşlarıyla serbestçe dolaşır. Odanın çevresi boyunca pedestal'lar üzerinde on farklı sınıf, donmuş "echo"lar gibi bekledirilir: kilidi açılmış sınıflar taş-grisi tonda görünürken, henüz açılmamış olanlar opak siyah silüetler olarak durur. Bu ayrım hem görsel hem de anlatısal bir işlev görür; kaybedilmiş kimliklerin henüz yeniden kazanılmamış olduğu fikri, tek bakışta okunabilir.

Bir silüete yaklaştığında, o sınıfın yeteneklerini ve kaynağını özetleyen paneller ekranın kenarından kayarak girer. Oyuncu oradaki combat dummy (eğitim mankeni) üzerinde sınıfın vuruş hissini gerçek zamanlı olarak deneyebilir — seçim yapmadan önce nasıl bir karakter oynayacağını somut biçimde anlayabilir. [E] tuşuna basıldığında karakterin bedeni cyan bir enerji emilimi efektiyle o sınıfa dönüşür; hikâyesel olarak bu, unutulmuş bir kimlik yüzünü yeniden giymektir. Seçimi onaylamak için kuzey duvarındaki Rift kapısından yürüyerek çıkmak yeterlidir.

[GÖRSEL: [E] tuşuyla bürünme anı — cyan enerji efekti]

Bu tasarım kararının değeri, menüyü oyunun kendi dilini konuşan bir mekâna dönüştürmesinde yatar. Oyuncu asıl run'a başlamadan önce bile dungeon atmosferinin içindedir; sınıfını denemiş, bir seçim yapmış ve o seçimi fiziksel bir eylemle onaylamıştır. Geleneksel bir menü butonu tıklaması bunu yapamaz.

### Run — Oda Oda İlerleme

[GÖRSEL: Tipik bir combat odası — checker zemin + prop düzeni + aktif düşman grubu]

Attunement Chamber'dan çıkan oyuncu, run'un ilk odasına girer. RIMA'nın run yapısı, sabit bir çekirdek düğüm listesiyle tanımlanmış 12 düğümlü bir dungeon graph üzerine inşa edilmiştir. Bu graph; standart savaş odaları, elite karşılaşmaları, ödül odaları, dal birleşim noktaları ve en sonda boss arenasını içerir. Her yeni run bu graph üzerinden oynatılır; hangi oda türleriyle ne sırayla karşılaşılacağı kısmen rastgele atanır. Hangi odaya girileceği ise tamamen oyuncunun elindedir: dallanan kapı sistemi, run'un her aşamasında oyuncuya aktif bir rota kararı verir.

**Combat:** Oda aktif olduğunda düşman dalgaları sahneye girer. Oyuncu, seçtiği sınıfa özgü saldırı zinciri ve yetenek seti ile dövüşür. Düşmanlar hasar aldıklarında geri savrulur; ağır saldırılar tam bir knockdown animasyonu başlatır — karakter yere düşer, bir süre yerde kalır ve kalkış animasyonunun bittiği ana kadar i-frame (geçici dokunulmazlık) korumasına sahiptir. Bu knockdown sistemi hem görsel geri bildirim hem de taktiksel bir fırsat penceresi olarak çalışır: yerde olan bir düşman saldırı açısından daha savunmasız ama i-frame süresi içinde hasar almaz. Son düşman da devrildiğinde oda "Cleared" (Temizlendi) durumuna geçer ve müzik tonu değişir.

**3-Kart Skill Draft:** Her üç odada bir, oda ortasında bir ödül nesnesi belirir. Oyuncu [G] tuşuyla ödülü aldığında ekrana üç skill kartı açılır. Her kart, oyuncunun sınıfına ait ya da nötr pasif bir yetenek sunar. Üzerine gelindiğinde tooltip, yeteneğin ne yaptığını ve mevcut build ile olan sinerji noktalarını gösterir. Oyuncu üç karttan birini seçer ve o yetenek kalıcı olarak o run'a eklenir. Bu an, run'un en yüksek karar yüklü kesimidir: yanlış seçim build'in tutarsızlaşmasına yol açabileceği gibi beklenmedik bir kombinasyon tamamen yeni bir oynanış kapısı açabilir.

[GÖRSEL: 3-kart skill draft ekranı — tooltip açık, sinerji chip'i görünür]

**Dallanan Kapılar:** Oda temizlendikten ve ödül alındıktan sonra arka duvarda 1 ila 3 arasında kapı açılır. Her kapının üzerinde yer alan simge, hedefteki oda türünü işaret eder: olağan bir savaş mı, daha zorlu ama daha iyi ödüllü bir elite odası mı, yoksa doğrudan bir ödül odası mı? Oyuncu hangi kapıdan geçeceğine karar vererek run'un kısa vadeli akışını biçimlendirir. Tehlikeyi mi yoksa güvenliği mi tercih eder; bu tercihler run'un sonundaki build'i doğrudan şekillendirir.

[GÖRSEL: Çift veya üçlü kapı çıkışı — oda türü simgeleri görünür]

**Run Haritası [M]:** Oyuncu istediği anda [M] tuşuna basarak o ana kadar gezdiği düğümleri ve önündeki olası yolları gösteren run harita katmanını açabilir. Bu katman, hangi oda türlerinin kaç adım uzakta olduğunu görselleştirerek karar verme sürecine destek olur.

**Boss Karşılaşması:** Run'un sonuna doğru graph'ta boss düğümüne ulaşıldığında, boss arenasına giden kapı açılır. Boss arenası hem boyut hem zorluk açısından standart odalardan belirgin biçimde ayrışır; ayrıca mekânik tasarımı da o ana kadar run boyunca edinilmiş skill'lerin birlikte kullanımını sınar. RIMA'nın nihai boss tasarımı, oyunun tonal manifestosunu somutlaştırır: The Fracturing'in mimarı olan ve her şeyin kaynağı konumundaki varlık, oyuncunun tüm run boyunca öğrendiklerini ona karşı uygulayacağı şekilde tasarlanmıştır. Bossun devrilmesiyle ekranda kısa bir slow-motion geçişi yaşanır ve Victory akışı başlar.

### Victory / Ölüm ve Meta-İlerleme

Run sonunda — ister zaferle ister ölümle kapansın — oyuncuya o run'dan kazandığı Shattered Echo miktarı gösterilir. Echo hesabı; oda geçişleri, düşman öldürme başarıları ve boss devirme ödüllerinin birikimiyle oluşur. Tek bir run'dan kazanılabilecek miktar en az 5, en fazla 60 Echo ile sınırlıdır; bu sınır, erken oyun ile geç oyun arasındaki güç farkını kontrol altında tutar ve oyuncunun her run'u değerli hissetmesini sağlar.

Biriken Echo'lar, bir sonraki oturumda Attunement Chamber'daki kilitli sınıf silüetlerinden birinin kilidini açmak için harcanabilir. Bir sınıfın kilidini açmanın standart maliyeti 200 Echo'dur; bu, yaklaşık üç ila dört başarılı run demektir. Sistem, oyunculara her run'un boşa gitmediğini hissettirir: kazanılan run heyecanını tatmin ederken, kaybedilen run bir sonrakine yatırım yapılmış bir zemin bırakır. Bu yapıyla RIMA, saf roguelite anlayışının "her şeyi sıfırla" getiri hesabını, meta-ilerlemenin "her adım önemli" güvencesiyle dengeler. Oyunun ilerleyen aşamalarında bu ekonominin küresel harita geliştirmeleri ve ekipman modifikasyonları için de kullanılması planlanmaktadır; mevcut sürümde sınıf kilidi açma birincil harcama yolunu oluşturmaktadır.

---

## 3.4 Build Sistemi — Skill Draft, Tier Kilitleri ve Sinerji

### Skill Database ve Tier Sistemi

RIMA'nın tüm yetenekleri merkezi bir SkillDatabase'de tutulmaktadır. Bu veritabanında toplam 111 skill kaydı mevcuttur; bunların 67'si oynanabilir ve çalışır durumda implementasyona sahipken, 44'ü tasarım aşamasında olan placeholder olarak beklemektedir. On sınıfın her biri farklı bir kaynak mekanizmasına ve farklı bir oynanış diline kurulmuştur; Rage barını hasar vererek dolduran savaşçıdan, combo zincirleri üzerine inşa edilmiş hızlı dövüşçüye, elementel yıkım üretmeye odaklanan büyücüden yavaş birikim ve ani patlama döngüsüne dayanan diğer sınıflara kadar her biri ayrı bir oyun tarzını temsil eder.

Skill'ler beş tier'a ayrılmakta ve draft sırasında bu tier'ların görünme olasılıkları şu şekilde kalibre edilmiştir:

| Tier | Görünme Ağırlığı | Run Derinliği Kilidi |
|---|---|---|
| Common (Yaygın) | 55 | Kilitsiz, ilk odadan itibaren |
| Rare (Nadir) | 27 | Kilitsiz, ilk odadan itibaren |
| Epic | 12 | Oda 3 ve sonrası |
| Mythic | 5 | Oda 3+, yalnızca oyuncunun birincil sınıfı |
| Legendary | 3 | Oda 7 ve sonrası |

Bu kalibrasyon kasıtlıdır. Run'un ilk yarısında oyuncu, karakterinin temel kimliğini ve oynanış yönünü belirleyen yaygın ve nadir skill'leri seçer. Bu aşamada seçimler nispeten geniş bir alana yayılır: oyuncu hangi ekseni güçlendireceğine henüz tam karar vermemiştir ve sistem buna izin verir. Run ilerledikçe sinerji noktaları netleşir ve gerçek güç sıçramaları olan Epic ve üzeri tier'lar devreye girer. Legendary skill, run'un son çeyreğinde ortaya çıkar; o noktada build'in yönü artık belirginleşmiş olduğundan seçim gerçek anlamda ağırlık taşır — doğru Legendary, kurulu sinerjileri zirveye taşırken yanlış seçim build'i dağıtabilir. Bu yapı, her run'un hem ilk hem son skill seçimini anlamlı kılar.

### Warblade Örneği — Bir Sınıfın Tasarım Dili

[GÖRSEL: Warblade karakteri — omuz zırhı ve iki elli kılıç silueti]

On sınıfın tamamını tek tek açıklamak bu raporun kapsamının dışında kalır; ancak Warblade sınıfı üzerinden RIMA'nın sınıf tasarım felsefesini somutlaştırmak mümkündür.

Warblade, "yaklaş, sabitle, zırh kır, infaz et" mantığıyla çalışan ağır bir savaşçıdır. Omuz zırhlı silueti ve devasa iki elli kılıcıyla görsel olarak tanınabilirdir. Temel kaynağı, yalnızca hasar vererek ve düşmanlara kitle kontrolü (CC) uygulayarak doldurulan bir Rage barıdır; savaş dışında pasif olarak erir. Bu kaynak tasarımı, oyuncuyu sürekli temasa zorlar ve savunmacı bir oyun biçimini cezalandırır.

Warblade'in skill seti bu kaynak döngüsünü destekler. "Iron Charge" ile hedefe atılarak sersemletme, "Gravity Cleave" ile düşmanları kesen bir çekim alanı oluşturma ve "Iron Counter" ile reaktif bir savuşturma penceresi açma; bu beceriler savaşı her zaman düşmanı Broken (Sersemlemiş) veya Sundered (Parçalanmış Zırh) durumuna doğru yönlendirir. Bu durumlardaki bir hedefe imza yetenek "Death Blow" uygulanabilir: Rage barını tamamen boşaltarak yüzde dörtyüz veya üzeri hasar veren bir infaz darbesi. Rage barı dolduğunda ise [V] tuşuyla "Bladestorm" ulimate'i devreye girer; beş saniyelik kitle kontrolü bağışıklığı ve sürekli dönen AoE hasarıyla oyuncu kısa süreliğine durdurulamaz hale gelir.

Bu döngünün anlamlı kılınması, skill draft sırasındaki kararlarla gerçekleşir: bir Warblade oyuncusu seçtiği skill'lerin Rage kazanımını mı hızlandırdığını, Sundered durumunun hasarını mı çarptığını, yoksa Death Blow sonrasındaki boş Rage barını mı daha hızlı doldurmayı sağladığını değerlendirerek seçim yapar. Bu seçimlerin toplamı, her run'un farklı hissettiren bir Warblade versiyonunu ortaya çıkarır.

Benzer mantık diğer dokuz sınıf için de geçerlidir. Her sınıfın kaynak ekonomisi, imza yeteneği ve savaş döngüsü birbirinden yeterince farklılaştırılmıştır; aynı oyuncu arka arkaya iki farklı sınıfla oynadığında yalnızca varlık değil, düşünme biçimi de değişir. Bu çeşitlilik, tekrar oynama değerinin temel motorunu oluşturur.

### Chain Window ve Sinerji Gösterimi

Build sisteminin ikinci katmanını chain window (zincir penceresi) mekanizması oluşturmaktadır. Oyunun içinde 5 farklı zincir penceresi tanımlıdır. Bunlar, belirli skill'lerin ard arda tetiklendiğinde birbirinin etkisini çarpan zamanlanmış pencerelerdir: örneğin bir sersemletme yeteneği kullanıldıktan sonra belirli bir zaman dilimi içinde başka bir yetenek çalıştırılırsa, ikinci yeteneğin hasarı veya etkisi önemli ölçüde artar. Bu pencerenin kaçırılması zincirin kırılması anlamına gelir ve oyuncuyu bir sonraki fırsata hazırlanmaya yönlendirir.

Skill draft ekranında, seçilebilecek bir kart mevcut build'deki aktif bir zincir penceresiyle etkileşime giriyorsa bu sinerji küçük bir görsel chip olarak kartın üzerinde gösterilir. Bu sayede oyuncu, seçim yapmadan önce build'inin nasıl büyüyeceğini okuyabilir; sezgisel bir "birleşim var" sinyali alır ve seçimini buna göre biçimlendirebilir. Sinerji sistemi oyuncuyu daha derin bir anlayışa zorlamaz; ama bunu keşfetmek isteyenlere build'i sıradışı bir yöne taşıma kapısı açar. Bu katmanlı tasarım, hem ilk kez oynayan için yalın hem de ileri düzey oyuncu için stratejik bir deneyim sunar.

---

## 3.5 Oda Akışı State Machine

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

`RoomRunDirector` bileşeni bu yedi durumu (Idle, Combat, Cleared, RewardTaken, DoorOpen, Advancing, Victory) yönetir ve her geçişi bir koşul karşılandığında tetikler. Geçişler kasıtlı olarak tek yönlüdür: savaş bitmeden ödül alınamaz, ödül alınmadan kapılar açılmaz, kapıdan girmeden bir sonraki oda kurulmaz. Bu tasarım hem oyunculara net ve adil bir ilerleme hissi sunar hem de sistemin kendi içinde yarış koşullarına (race condition) düşmesini önler.

Durum makinesinin bir diğer işlevi de combat sistemini oda geçişlerinden ayırmaktır. EncounterController, düşman dalgalarını bağımsız olarak yönetir ve tüm dalgalar bittiğinde RoomRunDirector'a "temizlendi" sinyali gönderir. Bu ayrışma, düşman dalga tasarımının oda mimarisinden bağımsız olarak güncellenmesini mümkün kılar; yeni bir oda eklenmesi var olan savaş mantığını bozmaz, yeni bir düşman türü eklenmesi mevcut oda geçiş akışını etkilemez.

Ölüm, Victory dışındaki herhangi bir durumda gerçekleşebilir ve run'u doğrudan Death Screen'e taşır. Run, Victory durumuna ulaştığında ise kısa bir slow-motion geçişinin ardından kazanılan Echo hesabı ve özet ekranı gösterilir. Her iki sonuç da meta-ilerleme sistemine beslenerek bir sonraki run'un başlangıç koşullarını günceller; ölüm bir son değil, döngünün kaçınılmaz bir halkasıdır. RIMA bu halkayı bilinçli olarak kısa ve ritimli tasarlar: ortalama bir run, sınıfın oynanış hızına ve alınan kararlara göre değişmekle birlikte, oyuncuya her seferinde yeni bir deneme yapma isteği bırakacak uzunlukta tutulur.

---

*Kelime sayısı: ~2.405 (markdown işaretleri, tablo ayraçları ve görsel yer tutucular hariç)*
