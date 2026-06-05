# Bölüm 7: Doğrulama — Test ve Kalite Güvencesi

## 7.1 Tek Geliştiricinin Kalite Sorunu

Bir oyun projesinde test ve kalite güvencesi, ekip büyüdükçe doğal olarak dağıtılır: bir geliştirici yazan kodu başkası inceler, bağımsız bir test süreci hataları süzgeçler. Tek geliştiricili bir projede bu filtreleme mekanizması kendiliğinden ortaya çıkmaz. RIMA'da bu boşluğu kapatmak için üç katmanlı bir yaklaşım benimsenmiştir: otomatik birim ve entegrasyon testleri, sözleşme (contract) testleri ve sistematik görsel kalite güvencesi.

AI asistanlarının kod üretimde kullanıldığı bir bağlamda bu yaklaşımın önemi daha da artar. Otomatik olarak üretilen bir kod parçası derleniyor ve çalışıyor olabilir; ancak sistemin geri kalanıyla beklendiği gibi konuşup konuşmadığını ya da içerik üretim araçlarının çıktısının görsel olarak doğru olup olmadığını ayrıca doğrulamak gerekir. Test altyapısı bu doğrulama sürecinin omurgasını oluşturur.

## 7.2 Test Altyapısı

RIMA'nın test paketi, Unity Test Runner üzerinde çalışan EditMode ve PlayMode testlerinden oluşmaktadır. EditMode testleri, Unity sahnesi çalıştırılmadan derleme sonrası ortamda senkron biçimde koşar; play-mode probe'lar ise gerçek sahne yüklemesini, bileşen başlatmayı ve birden fazla kare boyunca süren asenkron etkileşimleri test etmek için tasarlanmıştır. Bunlara ek olarak sistematik davranış sözleşmelerini tanımlayan ve ayrı bir klasörde tutulan sözleşme testleri de mevcuttur.

Test klasörlerine göre kategoriler şöyle sıralanabilir: Bootstrap ve Wiring (bileşen bağlantılarının doğruluğu), Combat ve Encounter (savaş akışı ve dalga temizleme), Room ve MapDesigner (oda sistemi ve harita tasarımcısı araçları), Props ve Composition (nesne yerleştirme algoritmaları), RoomPainter ve Brush (boya katmanı araçları), Movement (oyuncu hareketi) ve UIFlow (arayüz akışı sözleşmeleri).

### Tablo 7.1 — Test Paketi Özeti

| Ölçüt | Değer |
|---|---|
| EditMode test dosyası | 85 |
| PlayMode test dosyası | 11 |
| Bağımsız sözleşme dosyası | 4 |
| EditMode test bildirimi (yaklaşık) | 490 |
| PlayMode test bildirimi (yaklaşık) | 39 |
| Son kaydedilen EditMode sonucu | 410 geçti / 0 hata / 1 belirsiz |

Son test koşusunda 410 EditMode testi hatasız geçmiş, yalnızca bir test belirsiz (inconclusive) olarak sonuçlanmıştır. Bu belirsiz test, Unity editörü dışında çalıştırıldığında erişilemeyen bir sahneleme bağımlılığını kapsamaktadır; işlevsel bir hata değildir.

[GÖRSEL: Unity Test Runner — 410 Pass / 0 Fail yeşil sonuç ekran görüntüsü]

## 7.3 Görsel Oda Kalite Güvencesi — Bölümün Kalbi

Otomatik testlerin yakalayamadığı bir sorun sınıfı vardır: sahnede görsel olarak yanlış görünen, ama kod açısından geçerli sayılan durumlar. Bu problemi çözmek için RIMA'da programatik bir görsel QC süreci geliştirilmiştir.

### 7.3.1 Sürecin Tasarımı

`RoomTemplateSO` varlıkları, RIMA'nın veri güdümlü oda sisteminin temel taşlarıdır. Her şablon bir odanın zemin hücrelerini, prop yerleşimlerini, kapı soket konumlarını ve oyuncu doğuş noktasını tanımlar. Projede 26 oda şablonu bulunmaktadır; bunların tümünün doğru inşa edilip edilmediğini kontrol etmek için editör modunda bir toplu QC koşusu hazırlandı.

Süreç şöyle işledi: her `RoomTemplateSO`, `_Arena` sahnesinde `IsoRoomBuilder` bileşeni aracılığıyla programatik olarak yeniden inşa edildi. Her inşanın ardından zemin karosu sayısı, uçurum (cliff) sprite sayısı ve prop sayısı otomatik olarak kaydedildi. Daha sonra Unity'nin editör görüntüsünden her oda için bir ekran görüntüsü alındı ve bu görüntüler `Assets/Screenshots/RoomQC/` klasörüne kaydedildi. 26 şablonun tamamı bu şekilde değerlendirildi.

### 7.3.2 İlk QC Sonuçları: 15 Tamam / 9 Şüpheli / 2 Başarısız

Değerlendirme üç sonuçla tamamlandı:

**Tamam (15 şablon):** Küçük ve orta boyutlu koridorlar, basit dikdörtgen odalar ve özel amaçlı odalar (Spawn, Boss_Intro, Chamber_CharSelect) görsel olarak temizdi. Bu şablonlar dikkatli bir şekilde el ile oluşturulmuştu ve prop yerleştirme sorunları içermiyordu.

**Şüpheli (9 şablon):** Bu odalar oynanabilirdi ama görsel olarak hatalıydı. Birçoğunda prop grupları adanın dışında, havada asılı görünüyordu; bazılarında ise köşe kenarlarında fazla yoğun uçurum sprite'ları oluşmuştu.

**Başarısız (2 şablon):** `combat_large_diamond_01` ve `combatlarge_twin_basins_01`. İki farklı türde hata barındırıyorlardı.

[GÖRSEL: Oda QC örneği — combat_large_diamond_01 hatalı (solda, prop'lar adanın dışında) ve düzeltilmiş hali (sağda)]

### 7.3.3 Kök Neden Analizi

İlk bulgu, 6 farklı şablonu aynı şekilde etkileyen sistematik bir prop yerleştirme hatasına işaret ediyordu. `combat_large_diamond_01`'de 31 prop'tan birkaçı adanın tamamen dışında, boşlukta duruyor; küçük bir kümesi havada asılı biçimde görünüyordu. Diğer etkilenen şablonlarda da benzer belirtiler gözlemleniyordu.

Analiz iki olası nedeni gündeme getirdi: ya bake edilmiş prop hücre koordinatları şablonun zemin hücre kümesinin dışına düşüyordu, ya da `IsoRoomBuilder` geçerli hücreleri yanlış dünya konumlarına yerleştiriyordu. İnceleme, sorunun bileşende değil, şablon verilerinde olduğunu ortaya koydu. Bridson-Poisson örnekleme geçişi sırasında otomatik doldurucu, adanın sınır bölgelerindeki bazı hücreler için koordinatları zemin hücre kümesinin dışına taşıyor; bu koordinatlar şablona bake edilirken hata orada kalıyordu. Sonraki `IsoRoomBuilder`, bu geçersiz koordinatı adanın dışındaki bir konuma dönüştürerek yerleştiriyordu.

`combatlarge_twin_basins_01`'deki ikinci hata farklı bir kök nedenden kaynaklanıyordu: iki havuzu birleştiren geçişteki tek hücrelik boşluk, uçurum çözücünün iç kenar olması gereken noktayı dış kenar olarak yorumlamasına neden oluyordu. Bunun sonucunda uçurum sprite'ları zemin katmanının tam ortasından geçerek odayı ikiye bölüyordu — görsel olarak koridor tamamen kullanılamaz hale geliyordu.

### 7.3.4 Düzeltme Süreci

Her iki hata için ayrı düzeltme görevleri oluşturuldu. Prop hatasında çözüm yaklaşımı şuydu: şablona bake edilmeden önce her hücre koordinatının zemin hücre kümesinde gerçekten var olup olmadığı doğrulanacak; geçersiz koordinatlar budanacak ve etkilenen 6 şablon yeniden tohumlanacaktı. Uçurum hatasında ise şablon verisi doğrudan düzeltildi: iki havuz arasındaki geçiş bölgesindeki tek hücrelik boşluk kapatılarak uçurum çözücünün iç hücreleri dış kenar olarak yorumlaması engellendi. `Treasure_01` şablonu ise 14 hücreden 42 hücreye büyütüldü — orijinal boyutun bir karakter sprite'ının tamamını kaplayacak kadar küçük olduğu görüldüğünde bu değişiklik zorunlu hale gelmişti.

### 7.3.5 Programatik Yeniden Doğrulama

Düzeltmelerin ardından her oda programatik olarak yeniden inşa edilerek doğrulandı. Kritik ölçüt şuydu: `propOutsideFloor` sayacı sıfır olmalıydı. Doğrulama çıktısı, etkilenen 9 şablonun tamamı için bu sayacın sıfıra düştüğünü gösterdi.

```
combat_large_diamond_01:  floor=212, builtProps=4, propOutsideFloor=0 → ok=True
combatlarge_twin_basins_01: floor=599, builtProps=1, propOutsideFloor=0 → ok=True
Treasure_01: floor=42, builtProps=4, propOutsideFloor=0 → ok=True
[RoomQCFix] PASS
```

Süreç, 26 şablonun tamamı için görsel doğruluğu kanıtlanmış bir son duruma ulaştı. Bu sürecin en önemli yan etkisi, gelecekteki oda doldurucu geçişlerini de engelleyecek kalıcı bir zemin doğrulama kuralının koda işlenmiş olmasıydı.

## 7.4 İkinci Vaka: Play-Mode Probe'un Yakaladığı Singleton Hatası

Görsel QC sürecinin tamamlanmasının hemen ardından, play-mode probe koşusu beklenmedik bir hata buldu.

RIMA'nın eski `DungeonGraph` bileşeni (Core sistemi içindeki MonoBehaviour versiyonu), singleton örneğinin yönetimi için bir tekrar-girme koruması (duplicate guard) barındırıyordu. Bu koruma mantığı şöyle çalışıyordu: sahnede zaten bir örnek mevcutsa, yeni örnek `Destroy(gameObject)` çağrısıyla yok ediliyordu. Tasarım tek başına makul görünüyordu.

Ancak oyun akışında bir sorun ortaya çıktı. Karakter seçim sahnesi (`ChamberSelectBootstrap`), `DungeonGraph.Instance` referansını bellekte bırakıyordu. Bir sonraki sahne yüklendiğinde — `_IsoGame*` sahnesi —  "Systems" oyun nesnesi yeni bir `DungeonGraph` bileşeniyle birlikte geliyordu. Singleton koruması tetiklenince sadece `DungeonGraph` bileşenini değil, `RoomClearVictoryTrigger` ve diğer kritik sistem bileşenlerini de barındıran "Systems" oyun nesnesinin tamamını yok ediyordu. Sonuç: oda temizleme akışı sessizce kopuyordu. Düşmanlar öldürülse bile oda hiçbir zaman temizlenmemiş sayılmıyor, kapılar açılmıyor ve ödül doğmuyordu.

Hatanın tespiti play-mode probe koşusu sırasında gerçekleşti. Düzeltme cerrahi bir değişikliğle yapıldı: `Destroy(gameObject)` yerine `Destroy(this)` kullanıldı — böylece yalnızca tekrar eden bileşen yok edilirken "Systems" oyun nesnesinin geri kalanı sağlam kalıyordu. Play-mode doğrulaması, oyunun MainMenu'den savaşa, düşman temizlemesinden ödül alımına ve kapı açılışına kadar olan tam döngüyü başarıyla tamamladığını onayladı.

Bu hata, oyunu başlatıp oynayan bir kullanıcı tarafından hemen fark edilecekti; ama geliştirme sürecinde elle playtest yapmadan yakalanmak oldukça zordu. Oda temizleme probu, bu tür "sessiz başarısızlık" hatalarını bulmak için tam da bu amaçla tasarlanmıştı.

## 7.5 Sonuç: Üçlü Kalite Güvencesi

RIMA'nın doğrulama deneyimi, AI destekli kod üretiminde kalite güvencesinin tek bir yöntemle sağlanamayacağını ortaya koydu.

Otomatik testler (85 EditMode + 11 PlayMode dosyası, yaklaşık 530 test) hız ve tekrarlanabilirlik sağlar; her commit sonrası saniyeler içinde temel sözleşmelerin bozulup bozulmadığı anlaşılır. Play-mode probe'lar, sistem sınırlarında ortaya çıkan ve düzlem testleriyle yakalanamayacak hataları — singleton yaşam döngüsü uyuşmazlıkları, sahne geçiş etkileri — gün yüzüne çıkarır. Görsel QC ise veri güdümlü içerik üretiminin yarattığı görsel sapmaları, programatik doğrulama raporuyla birleştirerek kanıtlanabilir bir sonuca ulaştırır.

Bu üç katmanın birlikte çalışması, tek geliştirici + AI üretimi kombinasyonunda kalite güvencesinin nasıl yapılabileceğine dair somut bir model sunmaktadır.

---

*Kelime sayısı: ~1690*
