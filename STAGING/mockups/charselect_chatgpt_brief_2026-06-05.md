# ChatGPT CharacterSelect Önerisi (2026-06-05) — GİRDİ, DİREKT BENİMSENMEYECEK

> ⚠️ Council notu: Bu ChatGPT'nin tasarım önerisi. Kullanıcı: "direkt olarak bunu benimseme, 
> sadece tüm agentlar faydalansın, kendi fikrini sunsun." Ayrıca bazı maddeleri kullanıcı 
> direktifleri OVERRIDE ediyor (örn. kilitli karakterler = OPAK SİYAH silüet; ChatGPT'nin 
> "tamamen siyah blob olmasın" maddesi GEÇERSİZ).

RIMA oyunu için bir CHARACTER SELECT ekranı tasarlamanı ve mümkünse HTML/CSS/JS veya Unity UI 
mantığına uygun şekilde yapılandırmanı istiyorum.

GENEL AMAÇ
Bu ekran premium bir oyun hissi vermeli. "Mockup gibi duran" bir ekran değil, gerçekten oyunun 
içinden alınmış gibi hissettiren bir karakter seçme ekranı olmalı.
Dark-fantasy + cyan rift + ruined keep atmosferi korunmalı.
Karakterler benim mevcut pixel-art karakterlerimle uyumlu görünmeli.

TEMEL TASARIM KARARLARI
- Toplam 10 karakter olacak.
- Tüm 10 karakter aynı ana sahnede görünecek.
- Karakterler zemindeki karolara mantıklı şekilde yerleştirilecek.
- Karakterler ne çok küçük ne çok büyük olacak; tile boyutuna göre doğal ölçekte duracak.
- Karakterler alt tarafta ayrı bir portrait strip veya platform üzerinde dizilmeyecek.
- Ekstra alt platform / podium / character pedestal istemiyorum.
- Seçili karakter öne büyütülmeyecek, ortaya çıkmayacak, ayrı hero showcase olmayacak.
- Sadece seçili olduğu belli olacak şekilde hafif glow / halka / aura / parıltı efekti olacak.
- Oyuncu karakterin ÜSTÜNE tıklayarak seçecek.
- Hover ve selected state net hissedilmeli.
- Karakter seçimi sahnenin içinden yapılmalı.

OYUN MANTIĞI
- Echo satın alınmayacak.
- Echo, run'lardan kazanılan oyun içi para birimi olacak.
- Kilitli karakterler oyun içinde run oynayarak biriken Echo ile açılacak.
- Ekranda "Echo satın al" gibi bir buton veya mağaza hissi olmayacak.
- Sadece oyuncunun mevcut Echo miktarı gösterilecek.
- Kilitli karakterlerde "Açmak için X Echo gerekir" mantığı olmalı.
- Yeterli Echo varsa buton "Kilidi Aç" olabilir.
- Açık karakterlerde buton "Seç" olabilir.

KARAKTER YERLEŞİMİ
- Tüm 10 karakter ana oda içinde görünmeli.
- Görsel düzen dağınık değil, temiz ve dengeli olmalı.
- Yerleşim grid hissi verebilir ama fazla tablo gibi durmamalı.
- 2 sıra halinde 5 + 5 veya buna benzer dengeli bir yerleşim olabilir.
- Her karakterin altında küçük isim etiketi olabilir.
- Kilitli karakterlerde küçük lock simgesi ve gerekli Echo bilgisi olabilir.
- Seçili karakterin altındaki alan veya çevresindeki glow ile seçili olduğu belli olmalı.

ARKA PLAN / ORTAM
- Mekan: RIMA evrenine uygun shattered keep / void-temple / ruined chamber.
- Zemin: büyük isometric taş karolar, cyan çatlaklar, az miktarda sihirli parıltı.
- Arka plan: mor/void gökyüzü, yıkık kolonlar, karanlık fantasy atmosferi.
- Sol ve sağ üstte veya uygun yerlerde büyük braziers / ateşler olabilir.
- Arka plan detaylı olsun ama karakterleri boğmasın.
- Arka plan oyun sahnesi gibi hissettirmeli, sadece dekoratif poster gibi değil.

UI YERLEŞİMİ
1) Sol Panel: Seçili karakterin adı · 3 kısa tag (HEAVY · MELEE · RAGE) · kısa motto · kısa açıklama · 
   pasif/kaynak sistemi açıklaması · stat barları (Hasar, Dayanıklılık, Hız, Kontrol, Zorluk)
2) Sağ Panel: Yetenekler başlığı · başlangıçta açık 3 aktif yetenek · altında kilitli ustalık/mastery 
   yetenekler · kısa açıklamalar · scroll yerine kompakt/temiz/premium
3) Üst Bar: sol üst "RIMA — KARAKTER SEÇ" · sağ üst mevcut Echo miktarı
4) Alt Kısım: ortada ana aksiyon butonu (SEÇ / KİLİDİ AÇ / YETERSİZ ECHO) · istenirse sol altta GERİ · 
   alt kısım fazla dolu olmayacak

GÖRSEL HİS
- "Gerçek oyun UI'ı" gibi; mockup/placeholder/web tasarımı gibi durmamalı
- Pixel-art karakterlerle uyumlu; UI çerçeveleri dark fantasy taş/metal karışımı
- Cyan glow ana vurgu rengi; okunabilirlik yüksek; sahne sıkışık görünmemeli
- Karakterlerin boyutları tile'larla orantılı

SEÇİLİ KARAKTER DAVRANIŞI
- Büyümesin, ortaya taşınmasın; hafif glow / ayak altı halka / outline / cyan parıltı
- Diğer karakterler sabit; tıklayınca sol+sağ panel güncellensin

KİLİTLİ KARAKTER DAVRANIŞI [⚠️ KULLANICI OVERRIDE: opak siyah silüet OLACAK]
- (ChatGPT: silüetleri görünsün, biraz pasif/karanlık — GEÇERSİZ)
- Tıklanabilir olsunlar; tıklanınca bilgileri panelde açılsın
- Yeterli Echo varsa "Kilidi Aç" butonu görünsün

TEKNİK İSTEK
- HTML/CSS/JS tek dosyada temiz mockup; Unity'de data-driven yapı
- Her karakter bir data object; selected/locked/unlocked state net

KARAKTERLER (10): Warblade, Elementalist, Ranger, Shadowblade, Ronin, Ravager, Gunslinger, 
Brawler, Summoner, Hexer. Varsayılan seçili: Warblade.

ÖZETLE EN KRİTİK NOKTALAR
- 10 karakter aynı sahnede; üstüne tıklayarak seçim; alt strip/platform yok
- Seçili karakter sadece glow; Echo satın alma yok; Echo run'lardan
- Kilitli karakterler oyun içi Echo ile açılıyor; temiz, premium, dengeli
