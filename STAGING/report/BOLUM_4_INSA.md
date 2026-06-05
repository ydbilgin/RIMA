# Bölüm 4: İnşa — Veri-Güdümlü Oda Sistemi ve Geliştirme Araçları

## 4.1 Odanın Kimliği: Sahneden Veriye

RIMA'nın harita ve içerik mimarisinin temelinde basit ama köklü bir karar yatmaktadır: her oyun odası, Unity sahnesi olarak değil, bir veri dosyası olarak tanımlanır.

Geleneksel yaklaşımda her oda ayrı bir sahne dosyasıdır; düşman yerleşimi, çıkış kapıları ve zemin düzeni bu sahnenin içine gömülüdür. Bu yöntem küçük ölçekte hızlı sonuç verir, ancak içerik büyüdükçe yönetilemez hale gelir: 30 farklı oda için 30 sahne, 30 farklı konfigürasyon noktası ve her güncelleme için 30 ayrı açma-kapama döngüsü anlamına gelir. Tek geliştirici için bu yük orantısızdır.

RIMA'da bunun yerine her oda, `RoomTemplateSO` adı verilen bir ScriptableObject veri yapısında tanımlanır. Bu dosya; odanın sınırlarını, kapı soketlerini, oyuncu başlangıç noktasını, yürünebilir ızgara bilgisini, doluluğu ve zorluk etiketlerini içerir. Asıl görsel yapı, yani zemin, kenar, uçurum ve prop'lar, oyun başladığında bu veriden çalışma zamanında inşa edilir.

Bunu mümkün kılan sistem `IsoRoomBuilder`'dır. Tek bir arena sahnesi olan `_Arena` üzerinde çalışır; her yeni odaya geçildiğinde önceki oda temizlenir ve veri dosyasından yenisi sıfırdan kurulur. Bu tasarım, içerik ölçeklenebilirliği açısından kritik bir avantaj sağlar: yeni bir oda eklemek, yeni bir ScriptableObject dosyası oluşturmak demektir. Sahne düzenleyicisinde elle çalışmak gerekmez. Tek geliştirici, içerik ekleme ve test etme sürecini önemli ölçüde hızlandırır.

## 4.2 İzometrik Yüzen Ada: Zemin, Uçurum ve Otomatik Yerleşim

RIMA'nın görsel dili "yüzen izometrik ada" üzerine kuruludur. Her oda, boşlukta asılı duran koyu granit bir platformdur; karakterler bu adanın üzerinde hareket eder, düşmanlar onların etrafında dolaşır.

Bu görünümün oluşturulması birkaç ayrı katmanın doğru sırayla inşa edilmesini gerektirir. Önce zemin karoları yerleştirilir; projenin görsel kimliğine uygun olarak iki varyant mevcuttur: standart granit ve belirli odaları ayırt etmek için tasarlanmış kareli (checker) desen. Her iki varyant da aynı ızgara sistemini paylaşır.

Zemin döşendikten sonra ada kenarlarına uçurum (cliff) yerleştirilmesi gerekir. Bu işlem tamamen otomasyona bırakılmıştır. `CliffAutoPlacer` sistemi zemin ızgarasını tarar; bir karonun komşusu boş (void) ise, yani adanın kenarındaysa, o noktaya yöne uygun bir uçurum sprite'ı yerleştirir. Sekiz izometrik yön için farklı görünümler mevcuttur: güneybatıya bakan bir kenar ile güneydoğuya bakan bir kenarın görünümü farklıdır, çünkü izometrik perspektifte bu açılar farklı yüzleri öne çıkarır. Sistem bu farkı otomatik olarak hesaplar; geliştirici hiçbir uçurum karosunu elle yerleştirmek zorunda kalmaz.

[GÖRSEL: IsoRoomBuilder tarafından çalışma zamanında oluşturulan örnek oda — zemin + otomatik cliff yerleşimi + prop'lar]

## 4.3 Dış Kaynaklı İçerik: Oda Tasarımına Yeni Bir Yaklaşım

30'dan fazla oda şablonu hazırlamak, tek bir geliştirici için ciddi bir içerik yükü oluşturur. Eğer her oda elle çizilecekse zamanın büyük kısmı içerik üretimine gider ve sistem geliştirmek için zaman kalmaz. RIMA bu soruna ilginç bir çözüm getirmiştir.

Oda tasarımları için ChatGPT'den yapay zeka destekli bir "seviye tasarımcısı" olarak yararlanılmıştır. Ancak bu süreç gözetim dışı bırakılmamıştır. Bir oda tasarımının geçerli sayılabilmesi için belirli bir şema formatına uyması gerekiyordu: ASCII ızgara düzeni, kapı soketleri, zorluk etiketi ve prop listesini içeren JSON çıktısı. Bu format `RoomJsonImporter` aracına doğrudan beslenebilir durumdaydı. ChatGPT'den bu şemaya uygun oda paketleri üretmesi istendi.

Elde edilen tasarımlar doğrudan kabul edilmedi; bir eleme sürecinden geçirildi. Birden fazla bakış açısıyla değerlendirme yapılarak oda paketi incelendi ve çeşitlilik, zorluk dengesi ve tematik uygunluk kriterleri göz önünde bulundurularak seçimler yapıldı. Bu filtreleme sonucunda 15 oda dışarıdan temin edildi ve mevcut odalarla birleştirilerek 26 şablonluk bir havuz oluşturuldu.

Bu yaklaşım, "LLM'i kör biçimde kullanmak" ile "her şeyi elle yapmak" arasındaki bir denge noktasını temsil eder. Yapay zeka ham içerik üretir; geliştirici ise kalite filtresi olarak konumlanır. Her oda şablonu `_Arena` sahnesinde görsel olarak da doğrulanmış, 26 şablonun tamamı bu QC sürecinden geçirilmiştir.

| Oda Havuzu Özeti | Değer |
|---|---|
| Toplam oda şablonu | 26 |
| Dahili tasarım | ~11 |
| ChatGPT paketinden seçilen | 15 |
| QC uygulanan şablon | 26 |

[GÖRSEL: JSON → RoomTemplateSO akış şeması (ChatGPT çıktısı → RoomJsonImporter → ScriptableObject → IsoRoomBuilder → Oyun İçi Oda)]

## 4.4 Prop Yerleşimi: Oda Kompozisyonunun Otomasyonu

Her oda şablonu prop listesi taşır: bölgeye uygun dekor nesneleri (sandıklar, taşlar, kemikler, sütun kaidesi vb.). Ancak bu nesnelerin oda içindeki konumlarını tek tek belirlemek hem zaman alır hem de tekrarlayan bir işlemdir. RIMA bu süreci algoritmik olarak çözmüştür.

Prop yerleşimi için Bridson-Poisson disk örnekleme yöntemi kullanılmaktadır. Sezgisel anlatımla şöyle açıklanabilir: yürünebilir her zemin karesi potansiyel bir yerleştirme noktasıdır; algoritma bu noktaları rastgele seçer, ancak iki prop arasında belirli bir minimum mesafe garantiler. Böylece prop'lar birbirinin üzerine yığılmaz ve odada organik bir dağılım elde edilir.

Yerleştirme kararlarına ek bir kısıt daha rehberlik eder: kompozisyon rol haritası. Odanın merkezi "temiz alan" olarak işaretlenir; burada prop'lar görünmez, çünkü dövüş bu alanda geçer ve oyuncunun hareket alanının açık olması gerekir. Kapı girişleri de korunur; kapı önü daima geçilebilir durumda bırakılır. Kenar kuşakları ise dekorasyon için ayrılır. Tüm bu kurallar algoritmik olarak uygulandığından, 26 farklı oda şablonunda tutarlı bir yerleşim kalitesi elde edilir ve tek bir prop konumuna elle müdahale edilmesine gerek kalmaz.

## 4.5 Geliştirme Araçları: Oyunla Birlikte Büyüyen Bir Araç Zinciri

RIMA'nın en özgün mühendislik katkılarından biri, oyun kodu ile paralel olarak geliştirilen editör araç zinciridir. Bu araçlar olmadan 26 oda şablonu üretmek, QC sürecinden geçirmek ve görsel olarak doğrulamak imkânsıza yakın olurdu.

### Map Designer

Projenin ana editör aracı, birleşik bir 7-sekmeli yapıya sahip `Map Designer` penceresidir. Sekmelerin her biri harita oluşturmanın farklı bir boyutunu kapsar: zemin yerleşimi, uçurum düzenleme, nesne ekleme, portal tanımı, ışık yerleşimi ve katman yönetimi.

Araç tek bir ekranda tüm üretim döngüsünü kapsamaktadır: mevcut oda şablonlarına göz atma, zemin boyama (1, 3, 5 veya 10 karo boyutunda fırça), uçurum otomatik üretme ve önizleme görüntüleme. Bu tasarım kararı bilinçliydi; harita üretiminin her aşaması için ayrı pencereler açmak yerine tek merkezli bir iş akışı tercih edildi.

Araç, Unity editöründe çalışmanın yanı sıra, oynanış sırasında `F2` tuşuyla tetiklenebilen bir in-game katman (overlay) moduna sahiptir. Bu sayede geliştirici oyunu çalıştırırken, duraklatmadan ve editöre geçmeden zemin ile prop düzenlemesi yapabilir. Editör ve çalışma zamanı aynı `RoomData` veri yapısını paylaştığından, yapılan değişiklikler anında yansır.

### Room Browser

`Room Browser`, proje genelindeki tüm `RoomTemplateSO` dosyalarını listeleyen ve tek tıkla `_Arena` sahnesinde inşa eden bir editör penceresidir. Görevi basit ama kritiktir: kalite güvencesi için her şablonu görsel olarak doğrulamak, büyük bir zaman maliyeti doğurabilirdi. Room Browser sayesinde bu süreç minimuma inmiş; 26 oda şablonunun tamamı bu araç aracılığıyla kontrol edilmiştir.

Bu araç aynı zamanda sunum ve demo kolaylığı sağlar: hoca veya jüri karşısında herhangi bir oda anında oluşturulup gösterilebilir.

### RoomJsonImporter

Bu araç, ChatGPT'den veya başka bir kaynaktan gelen JSON formatındaki oda verilerini `RoomTemplateSO` varlıklarına dönüştürür. Şemayı doğrular (genişlik/yükseklik geçerliliği, kapı soketleri, oda tipi etiketleri), veriden şablonu oluşturur ve varlık veritabanına kaydeder. 15 dışarıdan temin edilen oda bu araç aracılığıyla sisteme dahil edilmiştir.

### Registry Baker

Zemin, uçurum ve prop varlıklarının çalışma zamanında doğru şekilde bulunabilmesi için bir kayıt defteri (registry) tutulmaktadır. `RuntimeAssetRegistryBaker` aracı, proje varlıklarını belirlenmiş kök dizinlerden tarar, meta veri bilgilerini (katman, y-sıralama bilgisi, pivot) çıkarır ve bunu çalışma zamanına aktarır. Bu araç olmadan yeni bir varlık eklemek, manuel kayıt defteri güncellemesi gerektirirdi; baker bu adımı otomatikleştirir.

[GÖRSEL: Map Designer 7-sekmeli pencere görünümü]
[GÖRSEL: Room Browser + _Arena'da kurulu oda]

## 4.6 Teknoloji Seçimi: Unity 6 ve URP 2D

### Neden Unity 6?

Bu projenin başladığı dönemde Unity 6, uzun dönemli destek (LTS) aşamasına yaklaşan ve kararlılık açısından güçlü bir seçimdi. Tek geliştirici için bu önemliydi: yeni sürümlerin getirdiği API kırılmalarıyla uğraşmak yerine mevcut araçlar üzerinde üretkenliği korumak öncelikti.

### Neden URP 2D?

Evrensel Render Pipeline'ın 2D modülü, projenin görsel hedefleri açısından doğal bir seçimdi. Birkaç temel gerekçe bu kararı şekillendirdi:

**Pixel-perfect kamera:** RIMA'nın tüm varlıkları piksel sanatı olarak üretilmektedir. URP 2D, piksel mükemmelliğini (pixel-perfect) yerleşik olarak destekler; bu sayede karakterler ve zemin karoları her çözünürlükte bulanmadan, doğru şekilde görüntülenir. Alternatif render pipeline'larında bu özellik ya yoktur ya da ek eklentiler gerektirir.

**2D ışık sistemi:** İzometrik yüzen ada görsel dilinde ışık kritik bir rol oynar. URP 2D'nin `Light2D` bileşeni, zemin üzerinde dinamik gölge ve parlama efektleri oluşturmayı mümkün kılar. Özellikle odaların tanımlayıcı rengi olan cyan enerji çizgileri üzerindeki parıltı, bu ışık sistemi aracılığıyla gerçek zamanlı olarak oluşturulmaktadır.

**Tilemap entegrasyonu:** Unity'nin `Tilemap` sistemi, URP 2D ile sorunsuz çalışır. İzometrik zemin için özelleştirilmiş hücre boyutları (0.96 × 0.585 birim) ve derinlik sıralaması (y-ekseni bazlı custom sort axis) doğrudan tilemap üzerinde tanımlanabilmektedir. Bu sayede karakterlerin zemin nesnelerinin arkasından veya önünden doğru biçimde geçmesi, ayrı bir shader veya manuel sıralama kodu gerektirmeden sağlanmaktadır.

Bu üç özelliğin bir arada bulunması, URP 2D'yi piksel sanatı izometrik oyunlar için pratik bir seçim haline getirmektedir. Mimari karar bu gerekçelere dayanmaktadır; başka bir pipeline'la da benzer sonuçlar elde edilebilirdi, ancak ek çalışma maliyeti söz konusu olurdu.

---

*Kelime sayısı: ~2.950*
