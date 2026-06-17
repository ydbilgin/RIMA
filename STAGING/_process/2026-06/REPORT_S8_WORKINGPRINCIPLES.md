# Rapor §8 Ek Alt-Başlık — Bölüm 8.6

> Bu dosyanın içeriği BOLUM_8_ZORLUKLAR.md'ye 8.6 numaralı yeni alt-başlık olarak eklenir.
> Yerleştirme noktası: 8.5'in "Ders" bloğu bittikten hemen sonra, *Kelime sayısı* satırından önce.

---

## 8.6 İnsan-YZ İş Bölümü ve Çalışma Prensipleri

Proje boyunca gerçekleştirilen geliştirme çalışmalarının önemli bir bölümü, farklı rollere sahip yapay zekâ araçlarıyla iş birliği içinde yürütüldü. Bu alt-başlık söz konusu iş birliğinin nasıl yapılandırıldığını, hangi disiplinlerin uygulandığını ve bu yaklaşımın ölçülebilir çıktılara nasıl yansıdığını açıklamaktadır.

### Orkestrasyon Katmanı

Geliştirme sürecinin tamamında yönlendirme, önceliklendirme ve kabul kriterleri belirleme sorumluluğu geliştiricinin kendisinde kaldı. Yapay zekâ araçları bu kararları almadı; aldıkları kararları değil, onlara verilen kararları uyguladılar.

Tüm koordinasyon, geliştiricinin etkileşim kurduğu terminalde çalışan bir **orkestratör** katmanı (Claude Code) üzerinden yönetildi. Orkestratörün görevi tek bir büyük işi doğrudan çözmeye çalışmak değildi; işi anlamlı parçalara bölmek, her parçayı uygun uygulayıcıya yönlendirmek, çıktıları bir araya getirmek ve sonuçları geliştirici onayına sunmaktı.

### Ajan Rolleri

Sistemin üç işlevsel katmanı bulunmaktadır:

**Uygulayıcılar** — Kod yazan, dosya değiştiren, doğrulama komutu çalıştıran ajanlardır. `builder-opus` (Claude alt-ajanı) karmaşık çok-dosya değişiklikleri için; `crafter-sonnet` mekanik ve yinelemeli görevler için kullanıldı. Aynı zamanda **cx** (Codex / gpt-5.5) ve **ax** (Gemini 3.1 Pro, Gemini 3.5 Flash) arka planda dispatch edildi; her biri kendi güçlü olduğu alanda — sırasıyla derin statik analiz ve hızlı bilgi-toplama — görev aldı.

**Denetçiler** — `auditor-opus`, salt-okunur erişimle çalışır; yazan ajandan bağımsız olarak çıktıyı inceler ve doğrulama raporunu doğrudan geliştiriciye iletir. Bu katmanın varlığı, uygulayıcı ile denetçi arasındaki ayrımı kurumsal bir kural olarak hayata geçirir: **yazan denetlemez**.

**Danışman Konseyi** — Tek bir sistemin ötesine geçen, birden fazla bileşeni aynı anda etkileyen kararlar için cx + ax Pro + ax Flash üçlüsünden oluşan bir danışma paneli oluşturuldu. Konsey bağımsız bakış açıları üretir; sentezi geliştirici yapar. Bu yöntem, proje boyunca demo kapsam kilitleme, ses mimarisi ve görsel kalite kararları gibi geri dönüşü güç seçimlerde uygulandı.

### Temel Disiplinler

Sistemin işleyebilmesi için aşağıdaki disiplinler proje kuralı olarak belgelendi ve her oturumda uygulandı:

- **İddia ≠ kanıt:** Bir ajanın "başarılı" demesi, işin tamamlandığının kanıtı sayılmadı. Her kritik çıktı veri tabanlı doğrulama (birim testi, ekran görüntüsü, runtime API çağrısı) ile teyit edildi. Bu prensibin somutlaştığı örnek 8.4 no'lu vakada ayrıntılı olarak ele alınmıştır.
- **Tek Unity ajanı:** Aynı anda sahnede değişiklik yapan birden fazla ajan çalıştırılmadı. Bu kural, eş zamanlı sahne değişikliklerinin birbirini sildiği geri alma çakışması vakasından sonra benimsendi (bkz. 8.4).
- **Oturumlararası hafıza:** Yapay zekâ araçları her oturumda geçmiş bağlamı yeniden taramak zorunda kalmadı; tasarım kararları, teknik tercihler ve geçmiş hatalar kalıcı bellek dosyaları ve NotebookLM bilgi tabanı aracılığıyla oturumdan oturuma taşındı. Kod tabanının kendisi ise graphify aracıyla 6.925 düğümlü bir bilgi grafiğine dönüştürülerek mimari sorularda ajanların kaynak tüketimini en aza indiren, sorgulanabilir bir hafıza katmanı oluşturuldu.

### Mühendislik Çıktısına Yansıması

Bu yaklaşımın asıl değeri, prompt mühendisliği tekniklerinde değil; tek bir geliştiricinin aynı anda birden fazla teknik katmanı yönetebilmesini mümkün kılan yapısal disiplinde yatmaktadır. Oyun mekaniklerini kapsayan C# kod tabanı, oyun içi seviye editörü araç zinciri, piksel-art sprite üretim hattı ve görsel kalite güvence süreci — bunların tamamı paralel olarak ilerledi. Bu ölçeğin tek bir geliştirici tarafından gerçekleştirilmesi, yalnızca araç kullanımıyla değil; araçlar arasındaki iş bölümünü açıkça tanımlayan, her çıktıyı bağımsız denetimden geçiren ve kararları kalıcılaştıran sistematik bir süreç disipliniyle mümkün oldu.

---

*Bu alt-başlık BOLUM_8_ZORLUKLAR.md sonuna (8.5 Ders bloğu bittikten sonra, kelime sayısı satırından önce) eklenir. Yaklaşık 530 kelime.*
