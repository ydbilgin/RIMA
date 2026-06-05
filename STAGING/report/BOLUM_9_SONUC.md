# Bölüm 9: Sonuç ve Gelecek Çalışmalar

---

## 9.1 Ulaşılan Yer

Bu projenin başındaki soruyu hatırlatmak gerekir: tek geliştirici, kapsamlı bir aksiyon-roguelite nasıl yapılabilir?

Proje bu soruya iki paralel yanıtla başladı: içerik ve sistemleri birbirinden ayıran veri-güdümlü bir mimari, ve bu mimarinin üzerinde çalışan çok-ajanlı bir geliştirme süreci. Raporun sonunda bu yanıtların pratikte ne ürettiğine bakmak yerinde olur.

RIMA, oynanabilir tam döngüye sahip çalışan bir oyundur. Ana Menü'den başlayan, yürünebilir Attunement Chamber'da karakter seçimiyle devam eden, oda oda combat ve 3-kart skill draft içeren, dallanan kapılarla şekillenen ve boss karşılaşmasıyla kapanan bir run döngüsü — bunların tamamı birbirine bağlı ve çalışır durumdadır. On sınıf, her birinin farklı kaynak ekonomisi ve oynanış dili, 26 oda şablonu ve yaklaşık 490 otomatik test bu döngünün zeminini oluşturur.

Araç zinciri de bağımsız bir çıktı olarak değerlendirilebilir. Map Designer, Room Browser ve RoomJsonImporter; yeni içerik eklemeyi, test etmeyi ve görsel olarak doğrulamayı elle çalışmaktan çok daha az maliyetli bir hale getirdi. Bu araçlar olmadan 26 oda şablonunun tamamını üretmek, QC sürecinden geçirmek ve yeniden doğrulamak mümkün olmazdı.

Metodoloji açısından bakıldığında, çok-ajanlı geliştirme sürecinin yalnızca "AI kullanmak" olmadığı pratikte gösterildi. Rol tanımları, görev belgeleri, karar dökümanları, cross-review zorunluluğu ve doğrulama kanıtı gerekliliğinden oluşan bu süreç, geliştirici tarafından tasarlandı ve işletildi. Ajanlar araçtı; süreç ise bir mühendislik kararıydı.

---

## 9.2 Yol Haritası: Sırada Ne Var?

Projenin mevcut durumu oynanabilir bir prototip seviyesindedir. Bir ticari sürüme ulaşmak için tamamlanması gereken işler birkaç ana başlık altında toplanmaktadır.

**Silah üretimi ve sınıf derinleştirme.** Warblade sınıfının silahı (Warblade) zaten uçtan uca çalışır durumda. Kalan dokuz sınıfın her biri için benzersiz bir silah üretimi, hem sprite hem de mekanik açısından tamamlanması gereken en büyük içerik bloğudur. Paralel olarak on sınıfın her birindeki placeholder skill'lerin gerçek implementasyonla doldurulması, build derinliğini ve tekrar oynama değerini doğrudan etkiler.

**Elementalist büyü efektleri (VFX).** Büyü sınıflarının görsel kimliği için tasarlanan efektlerin particle sistemi ve shader düzeyinde üretimi, görsel kalite açısından bir sonraki önemli adımdır. Bu iş kod ve araç altyapısı hazır olduğu için beklemektedir; üretim kararları kullanıcı onayıyla başlatılacaktır.

**Ses ve müzik.** Mevcut prototipte ses sistemi henüz kurulmamıştır. Combat geri bildirimi (darbe, knockdown, yetenek sesleri), oda atmosferi ve run'a göre değişen müzik katmanları oyun hissini temelden etkiler. Bu alan, prototipten oynanabilir bir ürüne geçişin görünmez ama kritik bileşenidir.

**Boss çeşitliliği ve Act 2+.** Mevcut boss yapısı tek karşılaşmayla sınırlıdır. Act 2 ve sonrasını mümkün kılmak için yeni boss tasarımları, biome çeşitliliği ve artan zorluk eğrisi gerekir. Bu, projenin içerik derinliği açısından en uzun vadeli çalışmasıdır.

**Denge ve playtest turları.** Skill ağırlıkları, tier dağılımı, düşman can değerleri ve oda zorluk etiketleri şu an tasarım kararına dayalı başlangıç değerlerine sahiptir. Gerçek oyuncu verisine dayalı denge turları olmadan bu sayıların ne kadar doğru oturduğu bilinemez. Bu çalışma bir kez değil, her büyük içerik eklemesinin ardından tekrarlanması gereken bir süreçtir.

**Steam yayın hazırlığı.** Mağaza sayfası, sistem gereksinimleri, erişilebilirlik seçenekleri, Steamworks entegrasyonu ve Early Access yayın planlaması; oyunun teknik tamamlanmasından bağımsız olarak kendi zamanını ve dikkatini gerektiren ayrı bir iş kümesidir.

---

## 9.3 Çıkarılan Dersler

Bu proje boyunca hem teknik hem de süreç boyutunda bazı şeyler beklendiği gibi gitmedi ve bu sapmaların her biri öğretici oldu.

**Mimari kararları erken kilitlemek zaman kazandırır.** Oyunun en büyük teknik yeniden yapılanması, sahne-bazlı oda anlayışından veri-güdümlü mimariye geçiş oldu. Bu karar geç alınsaydı, o noktaya kadar üretilmiş içeriğin büyük bölümü yeniden yazılmak zorunda kalırdı. Mimari seçimler küçük projede bile sonradan pahalı hale gelebilir; erken kilitlenmesi gereken şeyler erken belirlenmeli.

**Testler "kod çalışıyor mu" sorusundan daha fazlasını sormak zorundadır.** Bir sistemin yapısal testleri geçmesi, görsel olarak doğru göründüğü anlamına gelmez. 26 oda şablonundan birçoğu, birim testlerde geçer görünürken prop'ları adanın dışında bırakıyordu. Görsel çıktı içeren projelerde test altyapısının da görsel doğrulama katmanı içermesi gerekir.

**Yapay zekâ ajanlarına güven, teyide dayanmalıdır.** "Ajan başardı dedi" ifadesi, gerçek çıktı incelenmeden kabul edilmemelidir. Sessiz başarısızlık — ajanın hata vermeden eksik iş bırakması — pratikte birden fazla kez yaşandı. Geri dönüşü zor işlemlerde (sahne değişikliği, dosya silme, commit) ajanın raporunu bağımsız bir yolla doğrulamak vazgeçilmez bir adımdır.

**Tasarım zevki ve oyun hissi devredilemez.** Kod üretimi, toplu içerik oluşturma ve hata yakalama için çok-ajanlı süreç net bir değer üretti. Ama bir odanın sıkıcı hissettirip hissettirmediği, bir knockdown animasyonunun yeterince ağır görünüp görünmediği ya da bir skill seçiminin gerçekten anlamlı gelip gelmediği — bu kararlar yalnızca gerçek bir insan oyun oturumundan sonra verilebilir. Geliştirici, son karar mercii olarak her zaman sürecin içindeydi.

---

## 9.4 Kapanış

RIMA, tek bir soruyu yanıtlamak için başladı: yapılabilir mi? Prototip, yapılabilir olduğunu gösterdi. Oynanabilir döngü kapandı, araç zinciri çalışıyor ve metodoloji belgelendi.

Ama asıl soru bu değildi. Asıl soru şuydu: iyi oynanabilir mi, tekrar oynamak istenilir mi, başka birinin oynamak isteyeceği bir şey mi?

Bunu yanıtlamak için daha fazla oyuncu, daha fazla playtest ve daha fazla içerik gerekiyor. Yol haritası bu iş için var. Proje bitmedi; bir sonraki aşamaya hazır.

---

*Kelime sayısı: ~850*
