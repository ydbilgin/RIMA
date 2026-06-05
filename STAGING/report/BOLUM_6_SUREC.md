# Bölüm 6 — Süreç: Yapay Zekâ Destekli Çok-Ajanlı Geliştirme Metodolojisi

---

## 6.1 Sorun: Tek Kişiyle Stüdyo İşi

Bağımsız oyun geliştirme, özü itibarıyla bir kaynak sorunudur. Ticari bir aksiyon-roguelite proje; bir programcı, bir tasarımcı, bir sanatçı, bir QA uzmanı ve tüm bu rollerin üzerinde bir yapı kuran sistem mimarından oluşan bir ekip gerektirir. RIMA, bu rollerin tamamını tek bir geliştirici tarafından üstlenildiği bir bitirme projesidir. Ölçek farkı açıktır: Hades'in geliştirme ekibi onlarca kişiden oluşurken, RIMA'nın ekibi tek kişiden oluşmaktadır.

Bu gerçeklikle başa çıkmanın iki yolu vardır. İlki, projenin kapsamını minimuma indirgemektir. İkincisi, varolan kapasitenin üzerine çarpan bir iş akışı kurmaktır. Bu projede ikinci yol tercih edilmiştir; bunun için yapay zekâ araçlarından oluşan bir "sanal ekip" kurulmuştur.

Ancak bu noktada sık karşılaşılan bir yanılgıyı açıkça ortaya koymak gerekir: Burada anlatılan şey, bir dil modeline "şu kodu yaz" demek değildir. Bu tür kullanım, bir yapay zekâ asistanıyla yapılan sohbetten ibarettir ve çıktısı sohbet kalitesindedir. Bu projede kurulan şey, farklı rollere sahip ajanların belirli kurallara göre birbirleriyle etkileştiği, her kararın belgelendiği ve her çıktının bağımsız olarak doğrulandığı bir yazılım mühendisliği sürecidir. Ajanlar araçtır; süreci tasarlayan ve yöneten ise geliştiricidir.

---

## 6.2 Sanal Ekip Yapısı

RIMA'nın geliştirme sürecinde dört temel rol tanımlanmış ve bu roller farklı araçlarla doldurulmuştur.

| Rol | Araç / Model | Ne Yapar |
|---|---|---|
| **Orkestratör** | Claude Sonnet (bu proje) | Görev dağıtımı, sıralama, sentez, nihai karar |
| **Yazılımcı Ajan** | Codex / cx dispatch | Kod yazma, Unity değişiklikleri, mekanik batch iş |
| **Danışman Konsey** | Gemini 3.1 Pro, Gemini 3.5 Flash, Opus | Tasarım kararları, mimari değerlendirme, risk analizi |
| **İnceleyici (Reviewer)** | Yazar'dan farklı ajan | Çıktı kalite kontrolü, hata yakalama |
| **Bilgi Tabanı** | NotebookLM | Tasarım kararlarının kalıcı, sorgulanabilir arşivi |

Temel ilke basittir: **bir ajandan çıkan iş, o ajanın kendisi tarafından onaylanamaz.** Kodu yazan Codex'in ürettiği iş, başka bir ajan tarafından incelenir; danışman konseyin önerdiği mimari karar, orkestratör tarafından sentezlenir ve kullanıcı onayına sunulur.

Bilgi tabanı rolünü üstlenen NotebookLM, projenin belleği olarak işlev görmüştür. Tasarım kararları, oyun mekanik tartışmaları ve sistem spesifikasyonları buraya senkronize edilmiş; her ajan büyük bağlam okumaya gitmek yerine bu tabanı sorgulamıştır. Bu, hem token tüketimini azaltmış hem de kararların oturum başından oturum sonuna tutarlı kalmasını sağlamıştır.

---

## 6.3 Süreç Kuralları: Geliştiricinin Tasarladığı Pipeline

Bu sürecin en özgün yönü, ajanların yeteneklerinden ziyade ajanları yönetmek için geliştirilen kurallardır. Bu kurallar, denemeler ve gözlemlenen hatalar üzerinden belgelenmiş ve proje boyunca zorunlu kılınmıştır.

**Görev dosyaları.** Her iş, bir görev belgesiyle başlar. Görev belgesi; neyin yapılacağını, hangi dosyalara dokunulacağını, neyin kesinlikle dokunulmayacağını ve başarının ne anlama geldiğini açık biçimde tanımlar. Ajan bu belgeyi alır, tamamlar ve bir "done" raporu üretir. Bu yapı, ajanı doğrulama yapılabilir bir taahhüde bağlar: ya tanımlanan kriteri karşılamıştır ya da "BLOCKED" yazar ve neden devam edemediğini raporlar. Sessizce yarım bırakılan iş kabul edilmez.

**Karar dökümanları.** Mimari, oyun tasarımı veya teknik bir konuda büyük karar verildiğinde, bu karar `STAGING/` klasörüne bir belge olarak yazılır. Kararın içeriği, hangi alternatiflerin değerlendirildiği, neden seçilmediği ve kararı onaylayan danışmanların görüşleri kayıt altına alınır. Bu belgeler; ilerleyen oturumlarda neden belirli bir yola gidildiğini açıklar ve aynı tartışmanın yeniden başlamasını önler.

**Yazar eşit reviewer değildir.** Cross-review zorunluluğu, sürece gömülü bir ilkedir. Örneğin, Codex'in yazdığı knockdown (yere düşürme) sistemi, Opus modeliyle çalışan bir ax-Opus ajan tarafından incelenmiştir; aynı ax kanalıyla yazılan ölüm-decal bileşeni ise Codex tarafından gözden geçirilmiştir. İnceleme, yalnızca "kod çalışıyor mu" sorusunu değil, "mimari bozuldu mu, başka sistemlere zarar var mı" sorusunu da kapsar.

**Doğrulama kanıtı zorunludur.** Bir iş "bitti" sayılmadan önce Unity'de derleme hatası olmadığı, ilgili testlerin geçtiği ve oyun içinde elle doğrulandığı raporlanmak zorundadır. Yalnızca kod yazıldığını bildiren raporlar kabul edilmemektedir. Bu kural, pratikte defalarca değer kanıtlamıştır: birden fazla vakada testler veya oyun içi doğrulama, kod gözden bakışta gözden kaçan hataları ortaya çıkarmıştır.

**Unity'ye tek ajan kuralı.** Birden fazla ajan aynı anda Unity Editor üzerinde çalışırsa, sahne dosyaları birbirinin üzerine yazılır veya çakışan durum ortaya çıkar. Bu nedenle, Unity ile etkileşim gerektiren işler sıraya sokulur; paralel çalışmak için aynı sahne dosyasına dokunmayan işler seçilir ya da önce commit yapılır.

---

## 6.4 Vaka: 10-Task Otonom Gece Kuyruğu

Yukarıdaki kuralların nasıl bir araya geldiğini göstermek için projenin belki de en yoğun geliştirme gecesine bakmak yeterlidir.

Kullanıcı bir akşam belirli bir yönergeler bütünüyle geliştirme sürecini otonom bırakmıştır: konsey görevleri nasıl rotalamalı, doğru ajanlara vermelidir, işler cross-review'dan geçmelidir. Sabaha kadar şunlar gerçekleşmiştir:

- 10 görev bir karar belgesiyle tanımlanmış ve sıralanmıştır (`QUEUE10_ROUTING_DECISION_2026-06-05.md`).
- Görevler dört paralel lane'e (şeride) ayrılmıştır: Codex-A, Codex-B, ax (Gemini), Sonnet-MCP.
- Her lane, diğeriyle aynı sahne veya sisteme dokunmayacak biçimde seçilmiştir.
- ~11 commit üretilmiştir.
- 9 görev tamamlanmış, 2 görev tanımsız kaldığı için BLOCKED olarak kullanıcıya raporlanmıştır.

Bu gece içinde **iki önemli hata** review aşamasında yakalanmıştır ve ikisi de yayına girmeden önce düzeltilmiştir:

1. **Dokunulmazlık sızıntısı (immunity leak):** Knockdown sistemini yazan ajan, karakter yere düştüğünde verilen dokunulmazlık penceresinin süre dolduğunda kapatılmadığı bir durumu fark etmemiştir. Bu, karakterin kalıcı olarak hasar almaz hale gelmesi anlamına gelir. Hatayı, kodu yazan Codex değil, review yapan ax-Opus ajan `OnDisable` temizleme eksikliği olarak tespit etmiştir.

2. **Çifte direnç uygulaması (double resistance):** Aynı knockdown sisteminde, hasara direncin hesaplamada iki kez uygulandığı bir başka hata da yine review sırasında açığa çıkmıştır. Hem doğru hem de yanlış kullanılan `resistancePreApplied` bayrağının aynı anda varlığı, karakterin tasarlanandan çok daha az hasar almasına yol açıyordu.

Bu iki hata, "kod çalışıyor" bakışıyla fark edilmesi zor türden hatalardır. Karakterin normal bir oyun oturumunda çalışıyormuş gibi görünmesi, ama yalnızca kenar senaryolarda bozulması, bu tür hataların oyun testlerinde geç ortaya çıkmasına neden olur. Yazar-reviewer ayrımı ve bağımsız inceleme bu vakaları erken yakalamıştır.

Gece boyunca dikkat çekici bir başka olay daha yaşanmıştır: bir ajandan kaynaklanan sahne restore işlemi, o sırada başka bir ajanın aynı sahneye uyguladığı değişikliği silmiştir. Bu, birden fazla ajanın aynı anda Unity sahnesi üzerinde çalışmasının neden kural ile engellendiğini somut olarak göstermiştir. Olayın ardından bu kural yazılı hale getirilmiş ve sonraki tüm oturumlarda uygulanmıştır.

---

## 6.5 Dürüst Değerlendirme

Bir süreç raporu, ancak nerede işe yaradığını ve nerede yetersiz kaldığını dürüstçe aktarıyorsa gerçek değer taşır.

**İşe yarayan alanlar:** Mekanik kod üretimi bu sürecin en güçlü tarafıdır. Knockdown fizik sistemi, tooltip bağlantı noktası, SkillDatabase kaydı, checker zemin algoritması gibi teknik ama iyi tanımlanmış işler, kaliteli ve az review döngüsüyle tamamlanmıştır. Aynı şey toplu işler için de geçerlidir: 15 oda projesine özellik dağıtımı, 19 sprite ikonun import ayarlarının toplu düzeltilmesi, 10 sınıf veri kaydı gibi tekrarlı ama hacimli işler dakikalar içinde yapılmıştır. Test yazımı da verimli bir alan olmuştur; bir test matrisinin tasarımı danışman konseye bırakılmış, Codex testleri yazmış, Unity Test Runner'da 8 EditMode + 2 PlayMode test yeşil sonuç vermiştir.

**İnsan kalan alanlar:** Tasarım zevki ve oyun hissi hiçbir zaman ajanlara devredilememiştir. Karakterin ne kadar hızlı hareket etmesi gerektiği, knockdown animasyonunun hangi çerçevede ne hissettiği, bir oda düzeninin sıkıcı mı yoksa akılda kalıcı mı olduğu gibi kararlar yalnızca gerçek bir insan oyun oturumundan sonra verilebilmiştir. Görsel onay da bu kapsamdadır; ajanlar ekran görüntüsü üretip tanımlayabilir ama "bu güzel mi" sorusu kullanıcıya aittir. Nihai kararlar her zaman geliştiricide kalmıştır: ajan önerir, geliştirici onaylar veya reddeder.

**Yaşanan ajan hataları ve kurulan korumalar:** Sessiz başarısızlık en tehlikeli örüntüdür. Bir Codex profili hesap kotasına takıldığında, eski bir "bitti" dosyasını tekrar basıp hata vermeden sona ermiştir; bu durum bir görevin gerçekten tamamlandığı yanılgısına yol açmıştır. Bunun üzerine dispatch sonrası her DONE dosyasının zaman damgası kontrol edilmesi kuralı getirilmiştir. Benzer biçimde, bir ajan göreve verilmeyen dosyalara dokunmaya meyillidir; bu nedenle görev belgelerinde "sadece şu dosyalara dokunulacak, başka hiçbir şeye el sürülmeyecek" şartı açıkça yazılmaktadır. Birkaç vakada bu kural ihlal edilmiş ve elle geri alınmak zorunda kalınmıştır.

**Öğrencinin rolü: sistem mimarı ve son karar mercii.** Bu sürecin doğru anlaşılabilmesi için bir ayrım gereklidir. Ajanlar, söyleneni yapan araçlardır. Neyin yapılacağını, hangi sırayla, hangi kısıtlarla, hangi rolün ne zaman devreye gireceğini tasarlayan kişi geliştiricinin kendisidir. Orchestrator/reviewer ayrımı, karar belgesi zorunluluğu, Unity tek-ajan kuralı, cross-review matrisi bunların hiçbiri ajanlardan çıkmamıştır. Bunlar bir mühendislik sürecinin tasarım kararlarıdır ve bu kararlar, tıpkı oyunun kod mimarisi gibi, geliştirici tarafından oluşturulmuş ve zaman içinde rafine edilmiştir. Bu anlamda geliştirici, kodu satır satır yazan kişi olmaktan çok, süreci işler kılan sistemi kuran kişidir.

---

*Kelime sayısı: ~2150*
