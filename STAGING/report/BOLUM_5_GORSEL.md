# Bölüm 5 — Görsel Üretim Hattı

## 5.1 Sanat Yönü

RIMA'nın görsel dili tek bir kararla özetlenebilir: oyuncu, taşıdığı karanlıkla yüzleşmek için mor bir boşluğa (void) savrulan bir savaşçıdır. Bu duygu zemine yansımalıdır. Bu nedenle alan izometrik yüzen taş adalardan oluşur; altında derinliği belli olmayan mor bir boşluk bulunur. Zemin rengi soluk veya açık gri değil, koyu slate/granit tonlarındadır. Koyu zemin, karakteri ve düşmanları zemine yapıştıran siluet okunabilirliği sağlar. Cyan (#00FFCC) enerji çatlakları ve rün parıltıları ise zinde odaları, karar noktalarını ve aktif objeleri işaret eder; ancak ekran alanının yalnızca yüzde beş ila sekizine hâkim olacak biçimde kullanılarak atmosferin baskın rengi korunur. Ada kenarlarından aşağı uzanan kalın koyu gri taş cliffler, boyuta derinlik verir ve sınırı oyuncuya sezdirir; kahverengi tonlar bu nedenle bilinçli olarak dışlanmıştır. Bu dil, referans görsel olarak izometrik konsept taslaklarından çıkarılmış ve belgelenmiş bir "sanat yönü kilidi" olarak proje başında sabitlenmiştir.

## 5.2 Karakter Sprite Üretimi: 10 Sınıf × 8 Yön

RIMA, on oynanabilir sınıfa sahip bir aksiyon-roguelite olarak tasarlanmıştır. Her sınıfın sekiz yönde (K, KD, D, GD, G, GB, B, KB) idle animasyonuna ihtiyaç duyduğu bölüm 3'te tespit edilmiştir. Toplamda 80 karakter sprite dosyası üretmek gerekmiştir. Bunun tamamını elle çizmek tek geliştirici için haftalarca çalışma anlamına gelirdi.

Çözüm olarak PixelLab AI platformundan yararlanılmıştır. Ancak sekiz yönün tamamını bağımsız olarak üretmek verimli değildir: K, KD, D, GD ve G olmak üzere beş yön AI ile üretilmiş; GB, B ve KB ise bu yönlerin yatay eksende aynalaması (flipX) ile türetilmiştir. Bu karar, içerik maliyetini yaklaşık yüzde 37 oranında düşürmüştür; üstelik aynalama ile elde edilen yönlerde stil tutarlılığı doğal olarak korunmuştur.

[GÖRSEL: Örnek bir sınıfa ait 8 yön sprite sheet — doğrudan üretilen ve aynalanan yönler işaretlenmiş]

Her sınıf için belirlenmiş canvas boyutları sınıfın silüet büyüklüğüne göre farklılık gösterir. Brawler, Elementalist ve Warblade için 120×120 piksel; Gunslinger, Hexer, Ravager, Shadowblade ve Summoner için 124×124 piksel; Ranger ve Ronin için 128×128 piksel kare canvas kullanılmıştır. Görünür karakter gövdesi her sınıfta yaklaşık 64 piksel uzunluğundadır; boşluk ise animasyon eylemleri sırasında karakterin canvas dışına taşmaması için tampon görevi görür.

Stil tutarlılığının korunması ayrı bir süreç gerektirmiştir. Her sınıf için PixelLab'e verilen prompt, ortaklaşa tanımlanmış bir "silüet standardı" belgesine dayanır; bu belgede her sınıfın beden oranı, silahlı/silahsız duruş niteliği ve ayırt edici görsel unsurları tanımlıdır. Üretilen her sprite görsel olarak denetlenmiş ve gerektiğinde prompt güncellenerek yeniden üretilmiştir. Proje, süreç boyunca üretilen tüm karakterleri tek bir kayıt altında takip etmiş; hangi sınıfın hangi yönünün hangi üretim oturumuna ait olduğu ilgili belgelere yazılmıştır.

## 5.3 Piksel-Art İçe Aktarma Disiplini

Bir sprite'ı üreterek projeye almak yeterli değildir; nasıl içe aktarıldığı, oyun sahnesinde nasıl görüneceğini doğrudan belirler. RIMA, her karaktere ve çevre asset'ine uygulanan katı bir içe aktarma sözleşmesi tanımlamıştır:

- **Filtre modu:** Point (Nearest Neighbor) — piksel-art için bilinçli olarak seçilmiştir; GPU'nun kareler arasında enterpolasyon yapmasını engeller.
- **Piksel başına birim (PPU):** 64 — tüm sprite'larda ortak, tutarsız ölçekleme buglarını önler.
- **Sıkıştırma:** Yok — Unity'nin varsayılan lossy sıkıştırması piksel kenarlarını bozduğu için devre dışı bırakılmıştır.
- **Ölçekleme:** Tam sayı çarpanı — kesirli ölçekleme bulanıklaşmaya yol açtığından integer scaling zorunlu kılınmıştır.

Bu sözleşmenin pratikte önemi şudur: geliştirme sürecinde skill ikonlarının bir partisi Unity'e Bilinear filtre ile gelmiştir. Bu durum oyun içinde gözle görülür bulanıklaşmaya neden olmuş ve bir kalite kontrol geçişinde tespit edilerek etkilenen asset grubunun tamamı toplu düzeltmeyle Point filter'a çevrilmiştir. Bu vaka, sözleşmenin yalnızca kural değil, fiilen hata yakalayan bir mekanizma olduğunu göstermiştir.

Her karakter sprite'ının pivot noktası karakterin ayak tabanına yakın bir konuma (bottom-ish) ayarlanmıştır; bu sayede isometrik zemindeki konum hesaplaması ve derinlik sıralama Unity tarafından doğru biçimde gerçekleştirilir.

## 5.4 Ortam Asset'leri ve İki Araç Arasındaki Ayrım

Karakter sprite'larında PixelLab kullanılması bilinçli bir karardır: bu platform iskelet bazlı tutarlı bir üretim süreci sunar ve stil referansı ile yeniden üretim arasındaki sürekliliği korur. Ancak karakterlere özgü bu tutarlılık ihtiyacı, tek renkli zemin karoları veya UI arka planları için o denli kritik değildir.

Bu nedenle ortam ve arayüz asset'leri için farklı bir araç tercih edilmiştir: Imagen (Google AI görüntü üretimi) tabanlı bir boru hattı, agy komutuyla entegre edilmiştir. Konsept oda görselleri, zemin ve duvar doku taslamaları, UI arka plan görüntüleri bu yolla üretilmiştir. Toplamda 115 PNG staging alanında birikmiş; bunların bir bölümü doğrudan asset olarak kullanılmış, bir bölümü ise stil referansı ve renk paleti kaynağı olarak değerlendirilmiştir.

İki aracın kullanım alanları birbiriyle örtüşmez biçimde ayrıştırılmıştır: karakter üretimi yalnızca PixelLab, ortam/UI üretimi yalnızca Imagen. Bunun nedeni teknik değil estetiktir; farklı üretim motorlarından gelen sprite'lar stil uyumsuzluğuna yol açabileceğinden, oyuncunun sürekli baktığı karakterler tek kaynakta tutulmuştur.

Her iki araçtan gelen asset'ler proje ekibine (tek kişi olsa da) teslim edilmeden önce görsel kalite kontrolünden geçirilmiştir. Bu kontrol; sınır keskinliği, arka plan şeffaflığı, palet tutarlılığı ve orantı denetimini kapsar. Animasyon üretimi ise proje süresince bilinçli olarak ertelenmiştir. Knockdown gibi hareketler, ek sprite gerektirmeksizin kod tabanlı eğme ve squash teknikleriyle gerçekleştirilmiş; bu karar bölüm 3'te ayrıntılı biçimde ele alınmıştır. Animasyon üretim kararları yalnızca kullanıcı onayıyla başlatılmakta, bağımsız olarak tetiklenmemektedir.

[GÖRSEL: Attunement Chamber sahnesi — 10 sınıf istasyonu, mor void arka plan, cyan rün detayları]

Sonuç olarak görsel üretim hattı, üç ayrı disiplinin entegrasyonundan oluşmaktadır: sanat yönü kararları (atmosfer ve okunabilirlik dengesi), AI tabanlı üretim (PixelLab ve Imagen, ayrışık kullanım alanlarıyla) ve içe aktarma disiplini (Point/PPU 64/sıkıştırmasız sözleşme). Bu üç katmanın birlikte işlemesi, tek geliştirici tarafından tutarlı bir görsel dil oluşturulmasını mümkün kılmıştır.

---

*Kelime sayısı: ~1060*
