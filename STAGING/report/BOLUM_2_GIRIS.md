# Bölüm 2: Giriş

---

## 2.1 Problem: Tek Geliştiriciyle Kapsamlı Bir Oyun Yapmak

Bir aksiyon-roguelite oyunun yapılabilir olduğunu söylemek kolaydır. Gerçekte ne gerektirdiğine bakmak ise farklı bir tablo ortaya koyar.

Türün referans noktaları olan Hades, Dead Cells ve Slay the Spire, yıllar süren geliştirme süreçleri ve çok kişilik ekiplerle ortaya çıktı. Bu oyunlar yalnızca savaş mekaniklerini değil, her run'u farklı hissettiren oda çeşitliliğini, tutarlı bir görsel dili, yeniden oynama değerini taşıyan build derinliğini ve oturumlar arası anlam katan bir meta-ilerleme ekonomisini bir arada sunmak zorundaydı. Bunların her biri kendi başına bir uzmanlık alanı.

Bu proje, o çıtanın altında kalmayı hedeflemedi. RIMA; on oynanabilir sınıf, sınıfa özgü kaynak ekonomileri ve 80'den fazla yetenek içeren gerçek bir skill draft sistemi, dal dallanmasıyla şekillenen bir dungeon graph, prosedürel içerik için geliştirilmiş bir araç zinciri ve bunların tümünü birbirine bağlayan oynanabilir tam döngüyle tasarlandı. Bütün bunları tek bir geliştirici yapıyordu.

Bu, özünde bir kaynak sorunudur. Ticari bir roguelite normalde birbirinden farklı roller üstlenen bir ekip gerektirir: programcı, seviye tasarımcısı, sanatçı, QA uzmanı ve tüm bu rollerin çalışmasını koordine eden sistem mimarı. Tek kişilik bir projede bu rollerin hepsi aynı kişiye düşer. Eksilenin yalnızca zaman olmadığını söylemek gerekir; perspektif de eksilir. Kendi yazdığın kodu kendi gözden geçirmek, kendi tasarladığın seviyenin monoton olup olmadığını kendi fark etmek giderek güçleşir.

Bu gerçeklikle başa çıkmanın iki yolu vardır: projenin kapsamını kesmek ya da kapasitenin üzerine çarpan bir iş akışı kurmak. Bu projede ikinci yol seçildi.

---

## 2.2 Yaklaşım: İki Ayaklı Çözüm

RIMA'nın bu probleime yanıtı iki temel karardan oluşmaktadır.

**Birinci karar: veri-güdümlü mimari.** İçerik, sistemden ayrı tutulmalıdır. Her oyun odası ayrı bir Unity sahnesi değil, bir veri dosyasıdır. Skill'lerin tümü merkezi bir veritabanında tanımlanır. Düşman dalgaları, oda geçişleri ve ödül mekanizmaları sıkı sıkıya bağlı monolitik bir yapıya değil, birbirinden bağımsız ve değiştirilebilir bileşenlere dayanır.

Bu yaklaşımın pratik sonucu şudur: yeni bir oda eklemek, yeni bir ScriptableObject veri dosyası oluşturmak demektir — sahne editöründe elle çalışmak gerekmez. Yeni bir skill eklemek, veritabanına bir kayıt girmek demektir. Bu mimari, içerik ölçeklenmesinin bir kaç kat daha az iş maliyetiyle yapılabilmesini sağlar.

Araçlar da bu mimarinin bir parçasıdır. Zemin boyama ve uçurum yerleştirmeden oda şablonu içe aktarmaya kadar uzanan bir editör araç zinciri, geliştirme sürecinin içeriği test etme, doğrulama ve değiştirme maliyetini doğrudan etkiler. Araçsız bir veri mimarisinin değeri yarısına düşer; bu projede araç geliştirme, oyun geliştirmeyle eş zamanlı ilerledi.

**İkinci karar: yapay zekâ destekli çok-ajanlı geliştirme süreci.** Burada açıklanması gereken önemli bir ayrım var. Sık karşılaşılan bir yanılgı, "yapay zekâ kullanmak" ile "yapay zekâya söyle, çalıştır" olarak algılanmasıdır. Bu projede kurulan şey bunun değil.

Farklı rollere sahip yapay zekâ ajanları (kod yazma, tasarım danışmanlığı, çıktı inceleme, bilgi tabanı yönetimi) belirli kurallara göre bir araya getirildi. En temel kural şuydu: bir ajandan çıkan iş, o ajanın kendisi tarafından onaylanamaz. Kodu yazan ajan, yazdığı kodu incelemez; başka bir ajan inceler. Tasarım kararları konsey tartışmasına açılır, alternatifleri kayıt altına alan belgeler oluşturulur ve kullanıcı — yani bu projenin geliştiricisi — her kritik adımda son kararı verir.

Bu sürecin kuralları, ajanların kendiliğinden ortaya çıkardığı bir şey değil; denemeler ve gözlemlenen hatalardan öğrenilerek geliştirici tarafından tasarlandı ve zamanla rafine edildi. Ajanlar araçtır; süreci tasarlayan ve yöneten kişi geliştiricinin kendisidir.

İki karar bir arada ele alındığında, bir çarpan etkisi oluşturur. Veri-güdümlü mimari içeriğin üretilebilmesini kolaylaştırır; çok-ajanlı süreç ise bu içeriğin gözden geçirilmesini, test edilmesini ve doğrulanmasını mümkün kılar. Bu iki çark birlikte döndüğünde, tek geliştiricinin ulaşabileceği çıktı ölçeği anlamlı biçimde büyür.

---

## 2.3 Bu Raporun Kapsamı

Bu rapor, RIMA'nın tasarım kararlarını ve geliştirme sürecini belgelemektedir. Her bölüm belirli bir boyutu ele alır.

**Bölüm 3** oyunun kendisini anlatır: RIMA nedir, nasıl bir döngüsü vardır, hangi tasarım referanslarından ne alınmıştır ve build sistemi nasıl çalışır. **Bölüm 4** veri-güdümlü oda sistemini ve bu sistemle birlikte geliştirilen editör araç zincirini ele alır. **Bölüm 5** görsel üretim hattını açıklar: karakter sprite üretiminde PixelLab AI'ın nasıl kullanıldığını, içe aktarma sözleşmesinin neden gerekli olduğunu ve ortam asset'leriyle karakter üretiminin neden ayrı tutulduğunu. **Bölüm 6** çok-ajanlı geliştirme sürecini ele alır; rol tanımlarını, süreç kurallarını ve gerçek bir geliştirme gecesinde bu kuralların nasıl işlediğini aktarır. **Bölüm 7** test ve kalite güvencesi yaklaşımını anlatır: otomatik test altyapısı, görsel oda QC süreci ve bu sürecin gerçekten neleri yakaladığını. **Bölüm 8** geliştirme sürecinde karşılaşılan somut teknik ve süreç zorluklarını, her birinde izlenen teşhis ve çözüm yollarını aktarır.

---

## 2.4 Projenin Ulaştığı Yer

Raporun geri kalanına girmeden önce projenin şu an nerede olduğunu somut biçimde ortaya koymak gerekir.

RIMA, oynanabilir tam döngüye sahip çalışan bir aksiyon-roguelite prototipidir. Oyuncu Ana Menü'den başlar, yürünebilir Attunement Chamber'da on sınıftan birini seçer, oda oda ilerleyen bir run oynar, her üç odada bir 3-kart skill draft ekranıyla karşılaşır, dallanan kapılarla rota kararları verir, boss karşılaşmasına ulaşır ve run sonunda kazandığı Shattered Echo ile bir sonraki sınıfın kilidini açabilir. Bu döngünün tüm halkası çalışır durumda ve birbirine bağlıdır.

Sayısal olarak: 26 oda şablonu, 10 oynanabilir sınıf, 111 kayıtlı skill (~67'si çalışır implementasyona sahip), yaklaşık 490 otomatik test (son koşuda 410 PASS). Geliştirme sürecinde 100'ü aşkın karar belgesi yazıldı, her büyük mimari veya tasarım kararı belgelenerek arşivlendi. Bunlar yalnızca rakamlar değil; projenin mühendislik disipliniyle yönetildiğinin izleri.

---

*Kelime sayısı: ~1.200*
