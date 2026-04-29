# RIMA ve Ravenswatch Karşılaştırma – Genel Bakış

Ravenswatch, karanlık fantazi temalı bir top-down aksiyon roguelike oyunu olup **aktif keşif, zaman yönetimi ve çeşitlilik** üzerine kuruludur【29†L338-L343】【31†L139-L146】. Her kahramanın benzersiz mekanikleri, 3 farklı açık dünya haritası, görevler (masal karakterleriyle etkileşimler), *talent* (yetenek) gelişimi ve sonrasında yüksek tempolu epik patron savaşları sunar. RIMA ise piksel sanatıyla roguelite bir aksiyon oyunu; mevcutta **sınıf kimlikleri** ve “ikiye kırılma” mekaniğine odaklanmaktadır. Ravenswatch’tan adapte edilebilecek fikirler RIMA’ya **oyun döngüsünde derinlik, çeşitlilik ve akış** katabilir. Aşağıda Ravenswatch’ın somut özelliklerinden RIMA’ya uygulanabilecek 15 örnek listelenmiştir. Her birinde kısa açıklama, neden RIMA’ya uyduğu, uygulanma zorluğu ve prototip önerisi verilmiştir.  

**Temel Özellikler:** Ravenswatch’ın tanıtımında oyun içi öne çıkan mekanikler şöyle sıralanmıştır: “3 açık haritayı keşfetme” (masalsı temalarla)【29†L338-L343】; “Masal kahramanlarıyla ilişkiler kurarak ödüller kazanma”【29†L338-L343】; karakter hikâyelerini seferler boyunca açığa çıkarma【29†L344-L346】; *talent*, eşyalar ve “dream” birleşimlerinden güçlü buildler kurma【29†L359-L362】; her seviye sonunda yeni yetenek seçme【29†L359-L362】; 200’den fazla talent ve 50 eşyayla geniş kombinasyonlar【29†L371-L372】; rastgele haritalar ve farklı aktiviteler【29†L371-L374】; 3 gün/3 gece zaman sınırı ile sonrasında patron dövüşü【29†L350-L353】【31†L139-L146】; 50’den fazla benzersiz düşman ve epik patronlar【29†L354-L358】. 

## 1. Ravenswatch’tan Uyarlanabilecek Özellikler  

- **1. Çok Karakterli Sınıflar / Kahraman Çeşitliliği:** Ravenswatch’ta 9 farklı kahraman vardır ve her biri (Kırmızı Başlıklı Kız, Sun Wukong, Snow Queen vb.) kendine has mekaniklere sahiptir (örneğin Kırmızı Başlıklı Kız gece yarısı kurt formuna dönüşür)【16†L114-L122】. *Uygunluk:* RIMA zaten 8 farklı sınıfa sahip; her sınıfın oynanış hissini daha da belirginleştirmek (karakteristik “personalite” katmanları eklemek) sınıf kimliğini güçlendirir【13†L122-L127】. *Zorluk:* Orta (her sınıfa özgün derinlik eklemek zaman alır). *Prototip:* Tek sınıf için özgün bir mekanik (örneğin bir aşamadan sonra kit değişimi) eklenip oynanış test edilir.  

- **2. Dinamik Sınıf Modları (Gece/Gündüz veya 2. Form):** RW’da Kırmızı Başlıklı Kız her bölümün gece döngüsünde zorunlu olarak kurt formuna dönüşerek kitini değiştirir【16†L114-L122】. *Uygunluk:* RIMA’nın “iki sınıf kırılması” konsepti ile benzer; her biri için “ikinci form” veya mod eklemek ilginç olabilir (ör. savaşta özel bir tetikleyiciyle ikinci mod açılması). *Zorluk:* Yüksek (sanat, animasyon, denge açısından ağır). *Prototip:* Bir sınıfa basit bir form dönüşümü veya ikinci yetenek seti eklenip test edilir (örneğin belirli bir skoru geçince aktive olan mod).  

- **3. Harita Zamanlayıcısı ve Patron Öncesi Keşif:** RW bölümleri haritaları rastgele oluşturur ve oyuncuya genelde **~20 dakika keşif süresi** verip deneyim, yükseltme toplamasını sağlar【31†L139-L146】. Süre sonunda (veya isteğe bağlı miniyarıtışlar tamamlanınca) patron çıkar. *Uygunluk:* RIMA’da her “bölüm” için bir zaman sınırı getirmek (ör. odalar yerine açık harita veya belirli süre içinde ilerleme) ve sonunda patron savaşı tetiklemek çekici olabilir. Bu, oyuncuya sürekli harekette kalma hissi verir【31†L139-L146】. *Zorluk:* Orta (zamanlayıcı eklentisi orta zorluk; oyun akışına etkisi dikkatlice ayarlanmalı). *Prototip:* Basit bir haritada zamanlayıcı ekleyip,  süre sonunda patronu çağıran demo yapılır.  

- **4. Yan Görevler / İlgi Noktaları:** RW’da “Reverie sakinleri” (Üç Küçük Domuz, Sinbad, Peri Morgan vb.) oyuncudan yardım ister; bu görevleri yapınca ödül kazanılır【29†L338-L343】. *Uygunluk:* RIMA’da seviye içi rastgele etkinlikler veya mini görevler ile oynanışı renklendirmek mümkün. Bu, keşif motivasyonu sağlar ve oyuncuya seçim hissi katar. *Zorluk:* Orta-Yüksek (NPC/diyalog sistemi veya etkinlik tasarımı lazım). *Prototip:* Tek bir NPC ve basit görev (ör. belirli canavarları temizle) ekleyerek mekanik test edilir.  

- **5. Yetenek (Talent) ve Eşya Gelişimi:** RW’da her seviye sonunda rastgele *talent* (perk) seçenekleri sunulur; 200’ün üzerinde talent ve 50 eşya ile güçlü kombinasyonlar oluşturulur【29†L359-L362】【29†L371-L372】. *Uygunluk:* RIMA da yetenek ve eşya sistemine açılabilir; özellikle çok sınıflı yapıda farklı yetenek dalları (talentler) eklenirse derinlik artar. *Zorluk:* Yüksek (sistem gerektirir, dengelenmesi zor). *Prototip:* Tek bir sınıf için birkaç talent seçeneği ve basit eşya eklenip oynanış değişimi gözlemlenir.  

- **6. Rastgele Harita Düzeni ve Etkinlikler:** Ravenswatch haritaları rastgele üretilir ve “gizli objeler” ve etkinliklerle doludur【31†L139-L146】. *Uygunluk:* RIMA zaten oda tabanlı olabilir; rastgele veya içerik varyasyonu eklenmesi keşfi cazip kılar. *Zorluk:* Orta (harita jeneratörü gerekebilir). *Prototip:* Mevcut düzene basit bir rastgele harita algoritması ekleyerek test edilebilir.  

- **7. Patron Dövüşü ve Zorluk Katmanları:** RW’da her harita sonunda epik çok aşamalı patronlar bulunur【29†L354-L358】. *Uygunluk:* RIMA’da her “Act” sonunda daha karmaşık patron dövüşleri eklenerek heyecan artırılabilir. *Zorluk:* Orta (patron tasarımı zaman alır). *Prototip:* Bir veya iki yeni patron aşaması tasarlanıp denetlenir (farklı saldırı desenleri).  

- **8. Savaş Hızı ve Denge (Tempo):** Kullanıcı yorumlarına göre RW savaşları “tight, akıcı ve tatmin edici” bulunmuş【13†L122-L127】【24†L192-L201】. *Uygunluk:* RIMA’nın da ilk önceliği akıcı ve okuması kolay dövüş olmak; RW’dan vurgu (hasar sesleri, pozisyonlama) alınabilir. *Zorluk:* Düşük-Orta (mevcut combat engine uyarlaması). *Prototip:* Bir sınıfın saldırı hızı ve feedback’lerini ışıklandırarak test edilir.  

- **9. İtem / Ekipman Kombinasyonları:** RW’da “yıldız taşları” (Star of Fate), dilek kuyuları ve eşyalar (set bonus) bulunur【22†L212-L220】. *Uygunluk:* Eğer RIMA’da öğe sistemi geliştirilecekse, set bonusları veya kullanımla geliştiren mekanikler (örn. her 5 kere yapılan eşyalar vb.) düşünülebilir. *Zorluk:* Yüksek (çok fazla denge gerektirir). *Prototip:* Küçük bir eşya seti ekleyip bir set bonusu ile test edilebilir.  

- **10. Kooperatif (Çok Oyunculu):** Ravenswatch 4 oyuncuya kadar kooperatif oynanış sunar【31†L151-L158】. *Uygunluk:* RIMA halihazırda tek oyuncuya odaklı; co-op eklemek solo geliştirici için yüksek maliyet. Bu özellik genellikle sonraya bırakılır. *Zorluk:* Çok yüksek. *Prototip:* Erken aşamada önerilmez.  

- **11. Güçlü Mekanik Temalar (Zincir ve Tematik Uyarlama):** RW kahramanları masal karakterlerinden alıntıdır ve yetenekleri bu hikâyelerden etkilenir (örn. Romeo & Juliet’ın “Kiss” mekaniği ikili etkileşim sağlar【22†L198-L204】). *Uygunluk:* RIMA dünyasında benzer fragmente temalar olabilir (örn. bir sınıf hakkındaki hikâye ipuçları ya da mekaniğe yansıtma). *Zorluk:* Orta. *Prototip:* Bir sınıfa küçük bir arka plan ekleyerek oynanışına entegre eder.  

- **12. Zorluk Seviyeleri ve Modlar:** RW’da 4 farklı zorluk seviyesi ve özelleştirilebilir modlar var【29†L374-L377】. *Uygunluk:* RIMA için zorluk ayarı ve seçenek modları erken erişim/normal oynanış kadar, ileride güncellemeyle eklenebilir. *Zorluk:* Orta. *Prototip:* Temel bir ‘hardcore’ modu ekleyip oynanış ölçütleri oluşturulur.  

- **13. İlerleme ve Kilit Açma (Meta-Progression):** RW’da her run’da yeni kahramanlar ve içerikler açılır【29†L368-L370】. *Uygunluk:* RIMA’da zaten “bir önceki koşuda eksiğiniz kalmışsa devam edersiniz” gibi meta mekanikler olmalı. *Zorluk:* Orta (sistem mantığı ekleme). *Prototip:* Yeni koşularda basit bir ödül sistemi (örn. yeni perk) ekleyerek test.  

- **14. Görsel / Sanatsal İfade:** Ravenswatch “karanlık fantezi + çizgi roman” stilini benimser【29†L347-L349】. *Uygunluk:* RIMA piksel tarzında olsa da **atmosfer** ve renk paleti bu esinle karanlık, zengin renk tonları içerebilir. *Zorluk:* Düşük (mevcut sanatsal rehber değiştirilebilir). *Prototip:* Bir sahnede gölgeli tonlar, vurgulu konturlar ve gizemli efektler test edilir.  

- **15. VR/Gerçek Zamanlı Geri Bildirim (VFX/SFX):** RW vurgulu efektler ve seslerle güçlü geri bildirim verir (karakterlerin dönüşümü, saldırı efektleri vs.). *Uygunluk:* RIMA’da pikselli olmasına rağmen benzer netlik gereklidir; vurulma efektleri ve karanlık tema uyumlu sesler ile güçlendirilebilir. *Zorluk:* Orta. *Prototip:* Bir saldırıya parlak çatlama efekti ekleyerek oynanışı izler.  

## 2. Entegrasyon Planı

Aşağıdaki tablo, her önerilen özelliğin RIMA’nın hangi sistemlerine değeceğini, olası çakışma veya riskleri özetler. Her feature’ı RIMA’nın mevcut sistemleriyle eşleştirdik:

| Özellik                            | İlgili RIMA Sistemi                 | Entegrasyon Notu / Çakışma                                                                   |
|------------------------------------|-------------------------------------|---------------------------------------------------------------------------------------------|
| **Karakter/Ekip Gelişimi (Talent)**      | Karakter sınıfı ve yetenek ağacı   | Varolan spirit/path mekanizmasıyla örtüşebilir; ek karmaşıklık riski. Mevcut hissiyatı bozmadan dengelenmeli.  |
| **Harita Zamanlayıcı ve Patron**    | Core döngü, Zorluk eğrisi          | RIMA’nın “boss sonrası kırılma” mekanizmasıyla uyumlu. Süre kısıtı kavramı eklenirken oynanış hızı gereği test edilmeli.     |
| **Yan Görev/PoI Etkinlikleri**     | Oyun akışı, Harita/oda düzeni       | Harita akışına dalgalanma getirir; odak dağılabilir. Amaşaç netleştirilmeli, zorunlu olmayıp keşif motivasyonu sağlar. |
| **Karakter Hikâyesi / Metinler**      | İlerleme, Lore                     | Oyuncuyu türbülere sokabilir. Hikâye eklendiğinde kalite baskısı artar; RIMA özgün dünyasına ayarlanmalı.    |
| **Görsel/Tematik uyum**            | Atmosfer, UI/UX, VFX               | Kaynak eksikliği (solo) nedeniyle daha saf piksel sanatına uygunlaştırılmalı. Comic ögeler az, net çizgiler çok olmalı. |
| **VFX/SFX Geri Bildirimi**        | Geri bildirim, Çarpma hissi        | Geliştirilmesi önemli; RWs kadar çekici efekt (patlama, kan efekti) önerilir ama piksel tarzıyla uyumlu olmalı.|
| **Sınıf Dönüşümleri / İkinci Form**    | Sınıf kimliği, Denge               | RIMA’nın ikili sınıf yapısına paralel, ama fazla yeni form eklemek scope uçar. Sadece ana forma alternatif bir mod eklenmeli. |
| **Eşya / Gear Setleri**           | Ekipman sistemi, Loot/Reward       | RIMA’da şu an eşya sistemi yoksa aşırı genişletilmiş bir sistem çıkar. Sınırlı ve basit set efektleri düşük riskle denenebilir. |
| **Harita Rastgeleliği**           | Oda/Alan akışı                     | Varolan dungeon/oda düzenine uygun, çok karmaşık olmamalı. Az sayıda rastgele kombinasyon yeterli olabilir. |
| **Patron Çok Aşamalı Dövüşler**     | Patron tasarımı, Boss dönemeci     | RIMA da patron sonrası dönüşüm vurgulu. Aşamalı tasarım benzetilebilir; denge için tek boss orta formu olabilir. |
| **Zorluk ve Mod Ayarları**        | Zorluk eğrisi                      | RIMA’da temel normal/hard gibi basit modlar olması yeterli; RW’ın 4+ moduna ihtiyaç yok.                                                             |
| **Kooperatif**                    | -                                   | Solo geliştirici için dışı. Not: Tek oyuncu olarak düşünülmeli.                                                                                 |
| **Metagame İlerleme (Unlock)**      | Metagame, Kilit açma              | RIMA’da halihazırda spirit/kalıcı yükseltmeler var; benzer şekilde yeni ruhlar veya içerik açmaya devam edilmeli. Scope büyümesine dikkat.        |
| **Envanter / Eşya Augment**       | Loot, Envanter (yarınca)          | Ciddi ek veri gerektirir; sadece basit silah/artı stats içeren eşyalar önerilir. Çok fazla detay bloat yaratır.                                 |
| **Benlik Hikâyesi / Tema**         | Tema, Oyun Kimliği                 | RWs hikâye tematik. RIMA’da da “kırık dünya” temasına odaklanılmalı. Göstermelik düşman isimleri veya çizimlerle tema yansıtılabilir.    |

Bu entegrasyon planında özellikle şu riskler öne çıkar: Harita rastgeleliği ve PoI etkinlikler, RIMA’nın oda tabanlı akışını karmaşıklaştırabilir. İkinci form eklenmesi, doğrudan dual-class konseptiyle örtüşmekle birlikte çok fazla ek yük getirebilir. Eşya/set sistemi ve kooperatif, kapsam olarak solo dev için çok büyük. Bu yüzden **öncelikle düşük riskli* olanlar uygulanmalı (örn. varolan sistemlere takviye, zamanlayıcı vb.), yüksek riski özellikler (eşya sistemi, co-op) ileriye bırakılmalı veya sadeleştirilmelidir. 

## 3. Yol Haritası ve Aşamalandırma

Aşağıda önceliklendirilmiş bir MVP odaklı yol haritası ve tahmini süreler verilmiştir. Her aşama kritik fonksiyonel kabul kriterleriyle birlikte listelenmiştir. Kaynaklar sınırlı olduğu için “temel oynanış” temeline sadık kalınmış, kapsam kontrollü büyütülmüştür.

```mermaid
gantt
    dateFormat  YYYY-MM-DD
    title RIMA Adaptasyon Yol Haritası
    section PROTOTİP / Çekirdek
    Sınıf Yetenek Ağacı & Talent Seçimi    :done,    des1, 2026-05-01, 2w
    Zamanlayıcı & Patron Döngüsü           :done,    des2, 2026-05-15, 3w
    Hedef: Temel Dövüş, Yetenek Sistemi İşler  
    Hedef: Zaman sayacı ve patron/spawn düzeni çalışır  
    section GÖREVLER & GÜÇLENDİRME
    PoI / Yan Görev Mekaniği               :active,  des3, 2026-06-01, 2w
    Metagame İlerleme / Ödül Sistemi       :active,  des4, 2026-06-15, 2w
    Hedef: Tek NPC ve görev doğru tetiklenir  
    Hedef: Koşu sonunda yeni ödül mekanizması (gelişmiş spirit)  
    section DENGE & SONRASI
    Patron Çeşitliliği ve Zorluk Katmanları:        des5, 2026-07-01, 3w
    Görsel / Ses Geri Bildirimleri         :        des6, 2026-07-20, 2w
    Hedef: Yeni patron aşaması dengelenir  
    Hedef: Yeni VFX/SFX entegre edilir, geri bildirim iyi.  
    section Son Test & Temizlik
    Toplu Oynanış Testi & Rafine         :         des7, 2026-08-01, 2w
    Hedef: Tüm yeni sistemler entegre, büyük bug kalmaz.
```

- **Sınıf Yetenek Ağacı & Talent Seçimi (2 hafta):** RWs’tan ilhamla, her karakter seviyesinde birkaç yükseltme seçeneği sunma. *Kabul:* Oyuncu seviye atladığında 2-3 özgün yetenek/arTTırma arasından seçim yapabilmeli (örn. saldırı hızı, hasar, defans).  
- **Zamanlayıcı & Patron Döngüsü (3 hafta):** Haritalara ~10–15 dakikalık bir sayaç ekleyerek bu süre sonunda boss ortaya çıksın. *Kabul:* Sayaç dolduğunda boss çıkar, oyuncu isterse süreden önce boss’u çağırabilir. Süre bittiğinde oyuncu boss’a teleport edilir.  
- **PoI / Yan Görev Mekaniği (2 hafta):** Örneğin bir NPC ekleyerek onun belirttiği basit bir görev gerçekleştirme (en az 5 canavar öldür veya sandığı aç). *Kabul:* Görev tamamlandığında küçük sağlık/daha güçlü bir buff ödülü verilsin.  
- **Metagame İlerleme / Ödül Sistemi (2 hafta):** Her run sonu oyuncunun kalıcı bir ödül almasını sağlama. *Kabul:* Koşu sonunda bir spirit (ruhu) veya kalıcı istatistik iyileştirmesi açılabilmeli.  
- **Patron Çeşitliliği ve Zorluk (3 hafta):** Ek olarak bir patron varyasyonu ve zorluk seviyesi ekleme. *Kabul:* Ek patron aşaması düzgün çalışmalı ve normal akış bozulmamalı.  
- **Görsel / Ses Geri Bildirimleri (2 hafta):** Temel saldırılara canlı efektler ve vurulma sesi ekleme. *Kabul:* Vurma efekti net göründüğünde, oyuncu çarpma duygusunu hissetmeli.  
- **Toplu Oynanış Testi & Temizlik (2 hafta):** Tüm özellikler birlikte test edilir. *Kabul:* Kırılan mekanikler düzeltilir, büyük hatalar kaldırılır, performans optimizasyonu yapılır.

Her aşama sonunda oynanışın “çekirdeği” sağlam kalmalı. MVP aşamasında yalnızca en önemli yenilikler (yetenek ağacı, zamanlayıcı, bir görev vb.) eklenip test edilip temizlensin. Ek özellikler (ör. ek patronlar, zorluk modu) ikinci kademe olarak konumlandırıldı. Kabul kriterlerinde özellikle **RIMA’nın “iki sınıf kırılması”** mekanizmasının kesintiye uğramadığı, oyun temel döngüsünün akışının korunması vurgulanmıştır. Zaman çizelgesi süresel olarak tahminidir ve geliştirme hızına göre esneyebilir. 

## 4. Tasarım Notları ve Büyümeden Kaçınma

RIMA’nın en güçlü yanı “iki sınıf kırılması” fikridir ve buna dokunulmamalıdır. Yeni özellikler, bu çekirdeğe hizmet etmeli veya ona gölge düşürmemelidir. Örneğin yan görevler, zamanlayıcı veya talent sistemi eklemek, oyuncunun “iki sınıf combo’sunu güçlendirme” motivasyonunu pekiştirmeli, onun yerine geçmemelidir. Tasarım sırasında şunlara dikkat etmelidir:

- **Odak: Temel Dövüş ve Kontroller** – Önce hareket ve saldırı hissiyatı kusursuz olmalı. Zamanlayıcı veya görevler getirildiğinde bile, oyuncu öncelikle “başka sınıf ile nasıl savaşacağım?” sorusuna odaklanabilmeli【13†L122-L127】.  
- **Basitlik ve Okunabilirlik** – Yetenekler çok karmaşık hale gelmemeli; RIMA aksiyonu yoğun olduğundan bilgi yükü düşürülmeli. RW’den transfer edilen “talent” seçenekleri bile anlaşılır kalmalı.  
- **Kademeli Karmaşıklık** – Yavaş yavaş yeni mekanikler açılmalı. İlk sürümde bir sınıfa 2-3 talent, 1 görev, 1 boss eklenmesi yeter. Tüm RW fikirlerini birden uygulamak iş yükünü aşıp erken sürümü bozar.  
- **Kusursuz Combat Geri Bildirimi** – Her fikir uygulandığında “vuruş hissi” bozulmasın. RWs kullanıcıları vuruluşları çok beğenmiş【13†L122-L127】; RIMA’da da görsel ve ses geri bildirim eksikliği olmamalı.  
- **Scope Bloat’a Dikkat** – RW’da 200+ talent, 50 eşya, çoklu zorluk modu vb. var. RIMA’nın mevcut altyapısı için hemen bu kadarı değil, küçük örneklerle başlayın. Her yeni eklenen sistem “Güçlü çekirdek + gerekli bonus” mantığıyla sınırlandırılmalı.  

Özetle: Önce **hareket, vuruş hissi, bir sınıfın keyfi, yetenek yükseltme sistemi ve boss kırılma anı** mükemmelleştirilmeli. Bu parçalar sağlam olursa, sonraki özelliklerin eklenmesi işe yarar. Eğer bunlar eksik kalırsa, “fazla sistem” oyunu toparlamaz. Yani önce en temel oyun keyfini garantileyin, sonra yeniliklere geçin. 

## 5. Görsel/Audio Rehberi

Ravenswatch’ın çizgi roman havası yerine RIMA için karanlık, bulanık piksel estetiği uygun. Yine de RWs’dan şu geri bildirim öğeleri adapte edilebilir:

- **Kontrastlı Siluetler:** RW’da karakterler ve düşmanlar güçlü siluetlere sahip. RIMA’da sprite’lar net hatlı olmalı; düşmanlarla karışmamalı. RWs’ın yıkıcı saldırı animasyonları gibi (mesela Beowulf’un ejderhası) benimsenebilir – ama piksel halinde.  
- **VFX Efektleri:** RW patlama, kan sıçrama, ışık parlaması gibi efektlerle vurma hissini arttırıyor【16†L114-L122】. RIMA’da minimalistik de olsa özgün “atkın isabeti” etkisi olmalı. Örneğin, vurulan canavarda saniyelik kırmızı bir patlama (kan efekti) ve yatay bir çizgi. Çok fazla grenleme yerine sabit desenler tercih edilmeli (piksel sanat kuralı).  
- **Ses Tasarımı:** RW sesleri tok ve güçlü bulundu【13†L122-L127】. RIMA’da ışın silahı yerine ağır metal çarpma sesi gibi basit dokular kullanılabilir. Her sınıfa uygun kendine özgü vurma ve üstün atlama seseğine odaklanın.  
- **UI/XP Bilişsel Yönergeler:** RW’da bilgi göstergeleri (zaman sayacı, sağlık barları) açıkça görünür. RIMA’da metin, ikon ve bilgi panelleri okunaklı, ekranda dağılmadan öne çıkmalı. Zamanlayıcı koyulacaksa ekran köşesinde basit bir sayaç iyi olur.  
- **Görsel Tema ve Palet:** RW “gotik masal” teması için mor, lacivert gibi renkler kullandı. RIMA’da da benzer **karanlık palet** seçilmeli: koyu gri, mürekkep siyahı üzerine vurgu kırmızılar, maviler. (Örneğin boss odasında hafif sis efekti). Bu, stilin tutarlılığı için önemli.  

Özetle, RW’dan öğrenilecek en kritik nokta *vurma-akıcı his* ve *yüksek kontrastlı efektler*. RIMA’nın tek piksel ve sınırlı detaylı çizimlerinde bu his, belli başlı piksel efektleri ve çarpma sesleriyle verilmelidir. Süslü, bulanık efektlerden kaçının; her pikselli görsel aksiyon anında net kalmalı. 

## 6. Ravenswatch vs RIMA Karşılaştırma Tablosu

| Özellik                       | Ravenswatch Uygulaması                                  | RIMA Şu Anki Durumu           | Uyarlama Önerisi                                               | Risk                          |
|-------------------------------|--------------------------------------------------------|-------------------------------|-----------------------------------------------------------------|-------------------------------|
| **Karakter Çeşitliliği**       | 9 kahraman, her biri farklı yetenek/kontrol stili【16†L114-L122】  | 8 sınıf (Warblade, Shadowblade vb.)       | Her sınıfa özgün motifler, saldırı animasyonları eklenebilir  | Düşük (zaten benzer yapı)     |
| **Dinamik Form Değişimi**      | Kırmızı Başlıklı Kız gece kurt oluyor; sınıfa özel ikinci kit【16†L114-L122】  | Sınıf başına sabit kit; 2. sınıf kırılma planlı | Yalnızca bir sınıfa ikinci mod (ör. belirli koşulda kit değişimi) eklemesi  | Yüksek (çok iş)               |
| **Zaman Sınırı (3Gün/3Gece)**  | 20 dk keşif sonra zorunlu patron🕒【29†L350-L353】【31†L139-L146】 | Süresiz (odaların hepsinde serbest ilerleme)  | Her bölüm için sayaç; süresi bitince boss ortaya çıksın       | Orta (oyun akışı etkilenebilir)|
| **Yan Görev / PoI**           | Masal kahramanları görev verir (ödül kırmızı ipuçları)【29†L338-L343】 | Yok                              | Tek NPC ve basit görev sistemi ekle                           | Orta (akış dalgalanır)        |
| **Harita Düzeni**             | Rastgele üretilen açık haritalar, gizli upgrade ✔【31†L139-L146】 | Sabit oda-bölüm düzeni         | Rastgele oda yerleşimi, küçük harita varyasyonları          | Orta (algı bozulmazsa)       |
| **Talents / Yetenek Sistemi**   | Seviye-upda talent seçimi, 200+ seçenek【29†L359-L362】   | Ruh/ödül tabanlı yükseltme mevcut  | Her seviye sonrası 2-3 basit talent seçeneği sun            | Orta (fazla seçenek risk)     |
| **Eşya / Gear Setleri**       | 50 eşya, set bonusları, dilek kuyuları【22†L212-L220】     | Eşya sistemi yok veya basit    | Sınırlı sayıda efsunlu eşya, setten ziyade tekli bonuslar   | Yüksek (çok denge işi)        |
| **Bölüm Sonu Patron**         | Zaman veya görev sonrası zorunlu epik patron💀【29†L350-L353】 | Patronlar var; akış içinde     | Boss sabit kalabilir; ek minijel öncesi zayıflatma imkanı  | Düşük (kolay eklenir)        |
| **Oyuncu Dönüşü**            | 4 kişiye kadar co-op【31†L151-L158】                     | Tek oyuncu (odak)            | Şimdilik hayır; gelecekte düşünülür                      | Çok yüksek (sadece solo)      |
| **Zorluk/Modlar**            | 4 zorluk seviyesi, modlar【29†L374-L377】                | Tek zorluk (dengeli)         | Normal/Hard gibi mod ekleme                                | Düşük (basit ekleme)         |
| **Metagame İlerleme**        | Her run sonrası yeni kahraman veya yükseltme açma【29†L368-L370】 | Ruh sistemi var (kalıcı güç)  | Ek spirit veya yetenek kilitleri ekle                       | Orta (daha fazla menü)       |
| **Düşman Çeşitliliği**        | 50+ farklı düşman, her haritada yenilenen mini-bosslar【29†L354-L358】 | Az düşman çeşidi (tasarlanıyor) | Sayı artırılabilir ama aynı standartta kalite zor sağlar    | Orta (iş yükü)              |
| **UI / Bilgilendirme**       | Zaman sayacı, anlatıcı yok (tutorial eleştiri aldı)【31†L162-L167】 | Oyuncu genelde okumalı vs el yordamı  | Basit açıklayıcı UI ekle (zamanlayıcı, görev ikonu vb.)    | Düşük (yapılması kolay)      |
| **Görsel Stil**             | Canlı renkler, koyu fantezi/szinematik tarz【29†L347-L349】   | Piksel ve retro estetik     | Palet sıkı, gölgeli vurgu; çok parlak efekt yok            | Düşük (iki stil harmanı)     |
| **Geri Bildirim (VFX/SFX)**   | Titreşim, yankı, parlama efekti (kritik vuruşlar)【16†L114-L122】  | Basit pikselli efektler    | Parlama, parçacık pikseller, tok patlama sesleri           | Orta (optimizasyon)         |

Bu tablo özetle gösterir ki, **uygun** özellikler (örneğin karakter çeşitliliği, zamanlayıcı, patronlar) RIMA çekirdeğini tamamlar; **yüksek riskli** özellikler (devasa eşya sistemi, co-op) solo geliştirici için ertelenmeli. En düşük riskle başlayıp ardından orta riskli fonksiyonlar eklenmelidir. 

## Kaynaklar

- Ravenswatch resmi Steam sayfası, Özellikler bölümü【29†L338-L343】【29†L359-L362】  
- TryHardGuides Ravenswatch incelemesi (Erik Hodges)【16†L92-L100】【31†L139-L146】  
- Ravenswatch oyuncu yorumları ve tartışmaları【13†L122-L127】【31†L139-L146】  

 (Yukarıdaki kaynaklar, ilgili bölümlerden alınmıştır. Her bir öneri o bölümlere dayalıdır.)