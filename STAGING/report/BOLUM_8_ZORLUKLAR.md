# Bölüm 8 — Karşılaşılan Zorluklar ve Çözümler

Proje boyunca teknik, görsel ve süreç kaynaklı pek çok engelle karşılaşıldı. Bu bölümde, geliştirme sürecini en çok etkileyen beş vaka problem-teşhis-çözüm-ders formatında ele alınmaktadır.

---

## 8.1 Donanım-Sürücü Uyumsuzluğu: RTX 5080 ve Unity 6 D3D12 Çökmesi

**Sorun**

Geliştirme ortamının RTX 5080 ekran kartına taşınmasının ardından Unity Editor'da play-mode başlatılır başlatılmaz oyun kendiliğinden kapanıyordu. Çökme hiçbir şans tanımıyor, sahneyi bile yükleyemeden sonlanıyordu; dolayısıyla hata ayıklamak için herhangi bir log veya ekran görüntüsü elde etmek son derece güçleşti.

**Teşhis**

Crash yığını incelendiğinde hata zincirinin `CheckDeviceStatus → D3D12CommandList::PrepareExecute → GfxTaskExecutorD3D12` üzerinden geçtiği görüldü. Bu, Windows'un GPU sürücüsünü "yanıt vermiyor" olarak işaretlediği ve zorla sıfırladığı bir TDR (Timeout Detection and Recovery) durumuna işaret ediyordu. Kök neden, Unity 6'nın varsayılan grafik arka ucunun Direct3D 12 olması ve bu arka ucun, piyasaya çok yeni girmiş RTX 5080 sürücüsüyle henüz tam uyumlu olmamasıydı.

**Çözüm**

`PlayerSettings` altındaki grafik API sıralaması değiştirilerek Direct3D 11 birincil konuma taşındı ve bu değişiklik bir commit olarak kaydedildi. Editör yeniden başlatıldıktan sonra `SystemInfo.graphicsDeviceType` değerinin `Direct3D11` döndürdüğü doğrulandı; play-mode çökmesi tamamen ortadan kalktı ve projenin geri kalanı bu yapılandırmayla sürdürüldü.

**Ders**

En yeni nesil donanım ile en güncel oyun motoru versiyonu bir arada kullanıldığında, matür bir motor sürümüyle eski donanımda hiç görülmeyecek olan düşük düzeyli etkileşimler ortaya çıkabilir. Geliştirme ortamı değiştiğinde ilk adım varsayılan ayarların o ortam için gerçekten uygun olup olmadığını sorgulamak olmalıdır; yoksa hata ayıklamanın büyük bölümü sıfır getirili alanlarda harcanır.

---

## 8.2 İzometrik Uçurum (Cliff) Taşması: Görsel Hatada Kök Neden Aramak

**Sorun**

İzometrik ada haritaları oluşturulurken ada kenarlarına otomatik yerleştirilen uçurum sprite'ları (cliff tiles) zemine taşıyordu. Özellikle güney ve güneydoğu köşelerinde kümüksü, perde gibi görünen yoğun bir sprite yığılması oluşuyordu; bu durum oyun dünyasının derinlik algısını bozuyor ve adanın altının "kirlenmesine" yol açıyordu.

**Teşhis**

İlk müdahaleler, sprite boyutunu küçültmek veya z-sıralamasını düzenlemek gibi yüzeysel yamalar oldu; ancak hiçbiri sorunu kalıcı biçimde çözmedi. Sorunun gerçek kaynağı, uçurum yerleştirme algoritmасının yön bilgisinden yoksun olmasıydı. Algoritma her boşluk komşusu olan hücreye ayrım gözetmeksizin bir uçurum sprite'ı yerleştiriyordu. Oysa doğrudan güneyde (SW/SE) boşluk olan hücreler için derinlik algısını koruyan "içeri kıvrılma" (inward tuck) davranışı gerekirken, algoritma bunları diğer yönlerle aynı şekilde işliyordu. Sonuç: güney cephesinde sprite'lar görünür zemine taşıyordu.

**Çözüm**

Uçurum yerleştirme sistemi yön-bazlı komşu kontrolüne göre yeniden yazıldı. Her hücre için çevreleyen sekiz yönün void (boşluk) mı yoksa zemin mi olduğu ayrı ayrı sorgulandı; SW ve SE yönünde boşluk bulunan hücreler "içeri tuck" vektörüyle yerleştirildi. Bu sayede güney cephesindeki sprite'lar artık adanın gövdesi içine çekildi ve zemin düzlemine taşmadı. Ek olarak, yoğun uçurum bulunan büyük odalardaki görsel kümülenme önemli ölçüde azaldı.

**Ders**

Görsel bir hata tespit edildiğinde ilk içgüdü "sprite'ı biraz küçültelim" veya "katman sırasını değiştirelim" şeklinde olmaktadır. Ancak görüntüde bozulmaya yol açan şey çoğunlukla algoritmanın bağlamdan habersiz çalışmasıdır. Yama uygulamadan önce hatanın tam olarak hangi koşullarda tetiklendiğini belirlemek, uzun vadede çok daha sağlam bir çözüme ulaştırır.

---

## 8.3 Singleton Guard'ının Sistemi Sessizce Kesmesi

**Sorun**

Sahne geçişleri sırasında oda temizleme akışı çalışmıyordu: düşmanların tamamı ortadan kaldırıldıktan sonra ne ödül beliriverdi ne de bir sonraki odaya geçişi sağlayan kapılar aktif hale geldi. Sistem herhangi bir hata mesajı üretmiyordu; oyun sessizce kilitleniyordu.

**Teşhis**

İncelemede `Systems` adlı merkezi GameObject'in sahne yüklenirken yok edildiği görüldü. Bu nesne üzerindeki MonoBehaviour'lardan biri yineleme önleme amacıyla klasik bir singleton koruması (`DontDestroyOnLoad` + kopya kontrolü) içeriyordu. Sorun şuydu: kopya tespit edildiğinde koruma `Destroy(this)` yerine `Destroy(gameObject)` çağırıyordu. Bu yüzden sadece fazladan olan bileşen değil, üzerinde yaşam döngüsü yöneten tüm bağımlı bileşenler de dahil olduğu halde `Systems` nesnesi tamamen siliniyordu. Oda geçiş koordinatörü de bu GameObject üzerinde yaşadığı için her geçişte akış sessizce kopuyordu.

**Çözüm**

Singleton koruma mantığı yeniden yazıldı: `Destroy(gameObject)` yerine `Destroy(this)` kullanılarak yalnızca fazla olan bileşenin kendisi yok edildi. Böylece nesnenin diğer bileşenleri sağlıklı kalmaya devam etti ve oda geçiş akışı beklenen şekilde çalışır hale geldi.

**Ders**

Paylaşılan bir GameObject üzerinde singleton koruması uygulanırken kapsam son derece önemlidir. `Destroy(this)` bileşen kapsamında, `Destroy(gameObject)` ise nesne kapsamında çalışır. Sessiz başarısızlık üreten bu tür hatalar özellikle tanı koymayı güçleştirir; dolayısıyla bir singleton Guard'ı yazılırken tam olarak neyin yok edileceği bilinçli olarak belirlenmeli ve tercih belgelenmelidir.

---

## 8.4 Yapay Zeka Ajan Güvenilirliği: Sessiz Başarısızlık ve Çakışan Değişiklikler

**Sorun**

Geliştirme sürecinin belirli bir aşamasında birden fazla yapay zeka ajanı (cx kod ajanı ve çeşitli Gemini ax çağrıları) paralel veya arka arkaya çalıştırılmaktaydı. İki farklı güvenilirlik sorunu ortaya çıktı.

Birincisi "sessiz başarısızlık": bir kodlama görevi gönderilen ajan kota sınırına takıldığında hata fırlatmak yerine bir önceki çalışmadan kalan tamamlandı bildirimini olduğu gibi bırakıyor ve çıkıyordu. Sonuç, istenilen değişiklik hiç yapılmadan "bitti" raporu alınmasıydı.

İkincisi "geri alma çakışması": bir ajan doğrulama yaparken sahneyi kendi referans durumuna geri yüklüyor, bunu yaparken bir başka ajanın aynı sahneye yazdığı değişiklikleri siliyordu.

**Çözüm**

Her görev gönderiminin ardından tamamlandı bildirim dosyasının zaman damgası, görev başlangıç zamanıyla karşılaştırılarak eskiye ait bir bildirim olup olmadığı kontrol edilmeye başlandı. Sahne dosyası içeren işler öncesinde değişiklikler commit'lenerek kayıt altına alındı, böylece herhangi bir ajan geri yüklemesi commit geçmişinden öteye geçemedi. Sahnede eş zamanlı değişiklik yapabilecek ikinci bir ajan çalıştırmamak için tek-Unity-ajanı kuralı benimsendi. Bu kurallar belgelenerek ilerleyen görev kuyruklarında otomatik olarak uygulanmaya başlandı.

**Ders**

Yapay zeka ajanlarının oluşturduğu pipeline'da güven, sözlü teminata değil doğrulama mimarisine dayanmalıdır. "Ajan başarılı dedi" ifadesi, gerçek çıktının incelenmesiyle teyit edilmedikçe yetersiz bir kanıttır. Özellikle geri dönüşü zor işlemlerde (sahne değişikliği, silme, commit) ajan sonucunu bağımsız bir yolla doğrulamak vazgeçilmez bir adımdır.

---

## 8.5 Görsel Kalite Güvencesi Açığı: Yapısal "PASS" Görsel "Başarısızlık"

**Sorun**

Oda şablonlarına (RoomTemplateSO) prop yerleştirme sistemi eklendiğinde, editörden alınan yapısal sayımlar tutarlı ve mantıklı görünüyordu: zemin karosu sayısı, uçurum sayısı, prop sayısı hepsi beklenen aralıktaydı. Ancak bu sayıların ardında yatan görsel gerçeklik çok farklıydı. QC süreci ekran görüntüsü tabanlı hale getirildiğinde birden fazla odada prop'ların adanın dışında, boşlukta yüzdüğü görüldü. `combat_large_diamond_01` odasında otuz bir prop'tan birkaçı, adaya hiç değmeksizin tamamen void alanda asılı duruyordu.

**Teşhis**

Prop dağıtım algoritmasının zemin hücrelerinden örnekleme yapmak yerine sınırlayıcı kutu (bounding box) içinde rastgele koordinat ürettiği belirlendi. Bu yaklaşım küçük, düzenli odalarda nadiren sorun çıkarıyordu; ancak girintili, çentikli veya geniş boşluklara sahip organik şekilli odalarda koordinatların önemli bir kısmı zemin kümesinin dışına düşüyordu. Yapısal test bu durumu yakalayamıyordu çünkü prop nesnesinin var olması ve sayısının doğru olması yeterliydi; prop'un gerçekte nerede durduğu denetlenmiyordu.

**Çözüm**

Her prop yerleşiminden önce hedef hücrenin geçerli zemin kümesinde (`LastFloorCells`) yer alıp almadığını doğrulayan bir kontrol eklendi. Ayrıca sistematik bir QC süreci kuruldu: her oda şablonu editör modunda IsoRoomBuilder aracılığıyla inşa edildikten sonra ekran görüntüsü alındı ve sonuçlar bir rapor tablosunda (bkz. `STAGING/ROOM_QC_REPORT_2026-06-05.md`) FAIL / SAÇMA-suspect / OK şeklinde sınıflandırıldı. Bu süreç iki FAIL ve sekiz ek şüpheli odayı tespit etti.

**Ders**

Birim testleri ve yapısal kontroller bir sistemin var olduğunu doğrular; sistemin doğru göründüğünü doğrulamaz. Görsel çıktı içeren projelerde "çalışıyor" ile "doğru görünüyor" arasındaki uçurumu kapatmanın tek yolu ekran görüntüsü tabanlı, insan gözüyle onaylanan bir görsel QC adımı eklemektir. Bu adım ne kadar erken rutin hale getirilirse, görsel hataların commit'ler arasında birikimine o kadar az izin verilir.

---

*Kelime sayısı: yaklaşık 1.310*
