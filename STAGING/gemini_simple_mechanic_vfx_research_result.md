Warning: True color (24-bit) support not detected. Using a terminal with true color enabled will result in a better visual experience.
Ripgrep is not available. Falling back to GrepTool.
# Gemini Araştırma Raporu: Basit Mekanik + Mantık Çerçevesi + VFX Şölen Oyun Kütüphanesi

**Tarih:** 15 Mayıs 2026
**Odak:** Solo Dev, 32px Asset, Multi-Mechanic Synergy, Koda Dayalı VFX

---

## 1. Trend & Pazar Analizi (2022-2025)

Steam pazarında 2024-2025 döneminde "Vampire Survivors" klonlarının doygunluğa ulaşmasıyla, oyuncular **"aktif katılım + zeka + görsel ödül"** üçgenine yöneldi. *Magicraft*, *Noita* ve *An Amazing Wizard* gibi oyunların satış başarıları bu hibrit alt-türün yükselişini kanıtlıyor.

### En Çok Satan / İlgi Gören 10 Emsal
1. **Magicraft:** Modern "Noita meets Gungeon". (Trend belirleyici, modüler büyü).
2. **Noita:** Fizik tabanlı, piksel bazlı kimyasal reaksiyon.
3. **Risk of Rain 2:** Eşya sinerjisi ve oyun kıran (game-breaking) loop'lar.
4. **An Amazing Wizard:** (2025) Dead Cells hızı + Magicka büyü birleştirme.
5. **Wizard of Legend 2:** (2024) Element komboları, hızlı savaş.
6. **Bio-Prototype:** Biyolojik parça birleştirerek zincirleme reaksiyon kurma.
7. **Nova Drift:** Modüler "gemini modifiye et" + devasa VFX partikülleri.
8. **Spell Disk:** Diskleri birbirine bağlayarak mana/enerji döngüleri yaratma.
9. **Shape of Dreams:** (2025) MOBA tarzı yetenek etkileşimleri.
10. **The Spell Brigade:** Co-op odaklı alan etkili büyü kombinasyonları.

### TikTok/YouTube Viral Pattern Analizi
Bu tür oyunların kısa form video dostu olmasının sebebi **"Beklenti -> Kaos -> Başarı"** döngüsünün 15 saniyeye sığmasıdır. 
- **Klip Yapısı:** Oyuncu "Bakın şu 3 basit yeteneği birleştirirsem ne olacak?" der (0-5 sn), tetiği çeker (5 sn), ekran devasa partiküller, titremeler ve hasar sayılarıyla dolarken boss saniyeler içinde erir (5-15 sn).
- **Neden İşe Yarıyor:** "Asset ucuz, his pahalı" kuralı. 32px bir karakterin tüm ekranı kaplayan, renkleri tersine çeviren bir kara delik yaratması zıtlık (contrast) yaratır ve beynin dopamin merkezini vurur.

### 2024-2025 Sürdürülebilirlik (Tahmin)
Vampire Survivors trendi **kaybediyor** çünkü oyuncu eylemi pasif. Oyuncular artık "build'i kendi zekamla kurdum" hissini arıyor. "Mantık Çerçeveli Sinerji" oyunları sürdürülebilir bir *niche* çünkü tasarımcı sadece parçaları veriyor, oynanışı (ve komik/epik anları) oyuncu kendi yaratıyor.

---

## 2. 10 Yeni Oyun Konsepti (Multi-Mechanic + 32px + VFX Şölen)

*Tüm konseptler Manuel Combat ve 32px/Uzak Kamera kısıtlamalarına uygundur.*

### Konsept 1: Chroma Weaver (Renk Sinerjisi)
- **Hook:** Ana renkleri ateşleyip düşman üzerinde ara renk reaksiyonları (siyah delik, beyaz patlama) yarattığın bir arena.
- **Mekanikler:** 1. Kırmızı Atış (Ateş/Hasar), 2. Mavi Atış (Buz/Yavaşlatma), 3. Sarı Atış (Elektrik/Stun).
- **Combo Kuralı:** Düşmana ardışık renkler vurulduğunda renk paleti karışır. Kırmızı + Mavi vurursan düşman Mor'a döner ve etrafındakileri içine çeken bir yerçekimi kuyusuna dönüşür. Kırmızı + Mavi + Sarı = Ekranı kaplayan Beyaz Işık patlaması.
- **Combat Akışı:** Boya fırlatır gibi kaçarak düşmanları renklendir, kalabalıkları ara renk kombinasyonlarıyla tek seferde temizle.
- **VFX Kancası:** Renklerin sıvı gibi birbirine karışması, Mor/Yeşil/Turuncu patlamalarda HDR Bloom ve Chromatic Aberration zıplamaları.
- **MVP Süresi:** 12 Hafta.
- **Emsal:** Magicka x Geometry Wars.
- **AI Uyumu:** 9/10 (State machine mantığı çok net).
- **Viral An:** Bir boss'a 3 ana rengi aynı anda vurdurup ekranın "Negatif" renklere dönüşerek devasa bir hasar sayısı çıkarması.

### Konsept 2: Tether Node (Zincirleme Katliam)
- **Hook:** Düşmanları ışık halatlarıyla birbirine bağla ve birine verdiğin hasarı tüm ağa katlayarak yansıt.
- **Mekanikler:** 1. Düğüm At (Dash ile düşmanın içinden geçip ona düğüm bırak), 2. Halat Çek (Düğümleri birbirine bağla), 3. Darbe İndir (Manuel yakın/uzak saldırı).
- **Combo Kuralı:** Ağdaki düşman sayısı arttıkça aktarılan hasar çarpanı artar. Ateş veya Elektrik elementi ağ üzerinden seker. Ağdaki biri ölürse, patlayarak bağlı olduğu diğer düşmanlara "şok dalgası" yollar.
- **Combat Akışı:** Dash atarak 10 düşmanı geometrik şekillerle bağla, sonra birine en güçlü vuruşunu yapıp tüm sürünün aynı anda yok olmasını izle.
- **VFX Kancası:** Neon iplerin gerilmesi, hasar iletildiğinde ipler boyunca akan parlak veri/elektrik dalgaları, eşzamanlı "pop" (patlama) animasyonları.
- **MVP Süresi:** 14 Hafta.
- **Emsal:** Risk of Rain 2 (Ukulele item) x Hades (Dash odaklı combat).
- **AI Uyumu:** 8/10 (Ağ/Graph algoritmaları gerektirir).
- **Viral An:** 50 düşmanı tek bir ipe dizip en zayıf olanına vurarak tüm haritayı domino taşı gibi patlatmak.

### Konsept 3: Kinetic Pinball (Momentum Sinerjisi)
- **Hook:** Silahın sensin; duvarlardan ve düşmanlardan sektikçe gücün katlanır.
- **Mekanikler:** 1. Fırlatıl (Yön belirle ve kendini at), 2. Sekme Noktası Koy (Haritaya anlık trambolin/duvar yerleştir), 3. Kinetik Boşalım (Biriken momentumu lazer/patlama olarak sal).
- **Combo Kuralı:** Yere basmadan ardışık sekme sayısı "Momentum Stacks" biriktirir. Duvarlardan değil, yerleştirdiğin portatif sekme noktalarından veya düşmanlardan sekersen stack çarpanı katlanır. Stack arttıkça karakter bir ışık topuna dönüşür.
- **Combat Akışı:** Arena etrafına sekme noktaları diz, aralarına pinball topu gibi fırla, 20 sekme sonrası devasa hızla boss'un içinden geç.
- **VFX Kancası:** Her sekmede şiddetlenen ekran sarsıntısı, karakterin arkasında uzayan ışık kuyruğu (Trail Renderer), zamanın anlık yavaşlaması (Hit Pause).
- **MVP Süresi:** 12 Hafta.
- **Emsal:** Dandara x One Step From Eden.
- **AI Uyumu:** 10/10 (Fizik motoru ve basit vektör matematiği).
- **Viral An:** Ekrana 10 trambolin koyup saniyede 50 kere sekerek oyunun ses bariyerini aştığı an.

### Konsept 4: Catalyst Drop (Kimyasal Reaksiyon)
- **Hook:** Zemini yanıcı, zehirli veya iletken sıvılarla boya, sonra uygun elementi atıp kaosu izle.
- **Mekanikler:** 1. Sıvı Fırlat (Su, Yağ, Asit), 2. Kıvılcım/Buz/Elektrik At, 3. Rüzgar İtmesi (Sıvıları haritada kaydır/birleştir).
- **Combo Kuralı:** Yağ + Ateş = Alan hasarı. Su + Elektrik = Stun ağı. Asit + Ateş = Zehirli Gaz Bulutu. Sıvıların üzerine rüzgar atarsan sıvılar birleşerek (Yağ + Asit = Patlayıcı Jel) yeni katmanlar oluşturur.
- **Combat Akışı:** Kaçarak alanı hazırla (setup), sürüyü bölgeye çek, tek kıvılcımla reaksiyonu başlat (payoff).
- **VFX Kancası:** Zemindeki sıvıların normal mapping ile parlaması, alevin sıvı üzerinde yılan gibi ilerleyerek yayılması (Cellular Automata hissi).
- **MVP Süresi:** 15 Hafta (Sıvı shader'ı zaman alabilir).
- **Emsal:** Divinity: Original Sin (Combat mantığı) x Brotato.
- **AI Uyumu:** 7/10 (Grid tabanlı sıvı yayılma mantığı Codex için karmaşık olabilir).
- **Viral An:** Haritanın %80'ini yağla kaplayıp boss geldiğinde tek bir ateş okuyla tüm ekranı cehenneme çevirmek.

### Konsept 5: Echo Cast (Zaman Yankıları)
- **Hook:** Kendi geçmiş hareketlerinin kopyalarını çıkarıp, saldırıları üst üste bindir.
- **Mekanikler:** 1. Saldırı (Klasik mermi), 2. Kayıt Başlat/Bitir, 3. Yankı Çağır.
- **Combo Kuralı:** Kayıt sırasında yaptığın hareketler ve atışlar bir "Yankı" olarak saklanır. Yankıyı çağırdığında hayaletin seninle aynı şeyleri tekrar eder. Birden fazla farklı yankı çağırıp, hepsini aynı noktaya odaklayarak (crossfire) devasa kombolar yaratırsın.
- **Combat Akışı:** Düşman dalgası gelmeden önce boşluğa 3 farklı yönden ateş eden kayıtlar al. Düşman gelince 3 hayaleti birden çağırıp ortada bir "kurşun cehennemi" yarat.
- **VFX Kancası:** Hayaletlerin glitchy hologram efektleri, mermilerin üst üste bindiği noktada oluşan uzay-zaman yırtılması (Distortion).
- **MVP Süresi:** 16 Hafta.
- **Emsal:** Transistor x Enter the Gungeon.
- **AI Uyumu:** 6/10 (Kayıt ve replay sistemi mimari dikkat ister).
- **Viral An:** 5 hayaletin boss'un etrafında yıldız şekli oluşturup aynı anda lazer ateşlemesi.

### Konsept 6: Polarity Shift (Manyetik Savaş)
- **Hook:** Düşmanlara Artı (+) veya Eksi (-) kutuplar ver, sonra onları birbirlerine çarpıştır.
- **Mekanikler:** 1. Artı Mermisi, 2. Eksi Mermisi, 3. Manyetik Alan Tetikleyici (Mıknatıs etkisi yaratır).
- **Combo Kuralı:** Zıt kutuplu düşmanlar tetiklendiğinde şiddetle birbirine çekilir ve çarpışıp ezilirler. Aynı kutuplu düşmanlar birbirlerini haritanın dışına / duvarlara iterler. Boss'a Eksi verip, etrafındaki 20 minyona Artı vererek minyonları mermi gibi boss'a fırlatabilirsin.
- **Combat Akışı:** Kutupları hızlıca ata (boya), pozisyon al, tetikleyiciyi açarak ekranı fiziksel bir çamaşır makinesine çevir.
- **VFX Kancası:** Kırmızı ve Mavi manyetik dalgalar, düşmanların çarpıştığı anda oluşan şiddetli "Squash & Stretch" ezilme animasyonları ve Directional Sparks.
- **MVP Süresi:** 13 Hafta.
- **Emsal:** Outland x Nova Drift.
- **AI Uyumu:** 9/10 (Unity fizik motoruna doğrudan entegre).
- **Viral An:** Devasa bir boss'un, etrafındaki 100 küçük düşmanı mıknatıs gibi kendine çekip kendi kendini öldürmesi.

### Konsept 7: Resonance Glyph (Geometrik Rünler)
- **Hook:** Yere rünler çiz, rünleri çizgilerle birleştirip kapalı çokgenler oluşturduğunda içini yok et.
- **Mekanikler:** 1. Dash Atarak Rün Bırak, 2. Rünleri Bağla (Mermi ile rünlere ateş et), 3. Patlat.
- **Combo Kuralı:** 3 rün bir üçgen oluşturduğunda içindeki her şey yanar. 5 rün bir yıldız oluşturduğunda kara delik yaratır. Rünler arasındaki mesafe büyüdükçe alanın içindeki hasar çarpanı logaritmik artar.
- **Combat Akışı:** Düşmanların etrafından daire çizerek koş, noktaları birleştir, "Kapanış" mermisini atarak devasa alanı temizle.
- **VFX Kancası:** Çizgiler birleştiğinde oluşan keskin geometrik parlamalar, çokgenin içindeki renklerin anlık olarak negatife dönmesi.
- **MVP Süresi:** 14 Hafta.
- **Emsal:** SNKRX (geometri odaklı) x Diablo (Rune craft).
- **AI Uyumu:** 8/10 (Noktaların poligon içi testi matematiksel bir problem).
- **Viral An:** Haritanın tamamını kaplayan devasa bir pentagram çizip haritadaki her varlığı tek tuşla silmek.

### Konsept 8: Orbitect (Yörünge Mühendisliği)
- **Hook:** Etrafında dönen modülleri bir güneş sistemi gibi yönet, yörüngeleri birbiriyle çakıştır.
- **Mekanikler:** 1. Yakın Yörüngeye Uydu Al, 2. Uzak Yörüngeye Uydu Al, 3. Yörüngeleri Daralt/Genişlet (Spin).
- **Combo Kuralı:** Ateş uydusu ile Rüzgar uydusunun yörüngesi kesişirse, etrafa ateş hortumları saçılır. Uyduların dönüş hızı manuel olarak artırılabilir; maksimum hızda uydular merkezkaç kuvvetiyle kopup mermi gibi fırlar.
- **Combat Akışı:** Farklı element uydularını topla, düşman arasına girip dönüş hızını artırarak bir testere gibi düşmanları biçe biçe ilerle.
- **VFX Kancası:** Dönen objelerin yarattığı dairesel Motion Blur, yörünge kesişimlerinde sürekli patlayan mini-novalar.
- **MVP Süresi:** 12 Hafta.
- **Emsal:** Vampire Survivors (Garlic mekaniği) ama %100 manuel kontrol.
- **AI Uyumu:** 10/10 (Dairesel hareket ve trigger enter).
- **Viral An:** 10 farklı yörüngeyi maksimum hıza çıkarıp bir disko topu gibi lazer saçarak ekranda FPS düşürecek kadar efekt yaratmak.

### Konsept 9: Frequency Blade (Ritim ve Frekans)
- **Hook:** Kılıç darbelerin ses frekansları (dalgalar) yayar, doğru ritimle vurduğunda frekanslar üst üste binip rezonans patlaması yapar.
- **Mekanikler:** 1. Hafif Vuruş (Hızlı, kısa dalga), 2. Ağır Vuruş (Yavaş, geniş dalga), 3. Frekans Kırıcı (Biriken dalgaları patlat).
- **Combo Kuralı:** Hafif vuruşların dalgaları henüz sönmeden ağır vuruşun dalgası onlara yetişirse dalgalar birleşir ("Constructive Interference"). Oyuncu kombolarını bir metronom gibi zamanlarsa tek bir vuruş tüm ekranı kaplayan bir tsunamiye dönüşür.
- **Combat Akışı:** "Tık-tık---BAM" ritmiyle vurarak dalgaları birleştir, sonra düşman sürüsünün üzerine dalgayı sal.
- **VFX Kancası:** Ekranın dalga boylarına göre fiziksel olarak dalgalanması (Screen Distortion/Shockwave shader), bas frekansında ekran sarsıntısı.
- **MVP Süresi:** 14 Hafta.
- **Emsal:** Crypt of the Necrodancer x Sekiro.
- **AI Uyumu:** 7/10 (Zamanlama/Ritim tabanlı buffer sistemleri gerektirir).
- **Viral An:** Mükemmel zamanlamayla 5 dalgayı birleştirip boss'un health bar'ını tek vuruşla silecek devasa bir şok dalgası yaratmak.

### Konsept 10: Vector Pierce (Işınlanma Vektörleri)
- **Hook:** Fırlattığın mızraklar haritaya saplanır, sonra sen o mızraklar arasında ışınlanarak aradaki her şeyi kesersin.
- **Mekanikler:** 1. Mızrak Fırlat (Duvara/Yere saplan), 2. Düşmana Fırlat (Düşmana saplan), 3. Vektör Işınlanması.
- **Combo Kuralı:** Işınlanma sırasında geçtiğin çizgi üzerindeki tüm düşmanlar "Yırtılma" hasarı alır. Düşmana saplanmış mızrağa ışınlanırsan patlama yaratır. Ne kadar çok mızrağı ardışık ışınlanmayla gezersen hasar logaritmik artar.
- **Combat Akışı:** Arena etrafına 5 mızrak sapla, boss'a 1 tane sapla. Duvarlardan duvarlara ışınlanıp son olarak boss'a yıldırım gibi in.
- **VFX Kancası:** Işınlanma sırasında karakterin tamamen bir ışık çizgisine dönüşmesi, kesilen düşmanlarda anime tarzı "Gecikmeli Hasar" (slash lines ve 1 sn sonra patlama) efekti.
- **MVP Süresi:** 13 Hafta.
- **Emsal:** Katana Zero x FFXV (Warp Strike).
- **AI Uyumu:** 9/10 (Sadece pozisyon eşitleme ve Raycast).
- **Viral An:** Havada asılı kalıp 15 mızrak arasında yarım saniyede zikzak çizerek ekranı animevari kesiklerle doldurmak.

---

## 3. VFX Juice Kütüphanesi (Koda Dayalı, Asset Değil)

Solo geliştiriciler için asset çizmek zaman kaybıdır. Bu teknikler 32px'lik düz beyaz bir kareyi bile efsanevi gösterebilir. *(Zorluk: 1=Çok Kolay, 10=Uzmanlık İster)*

| Teknik Adı | Ne Yapar? | Hangi Konsepte Uygun? | Solo Dev Zorluğu (1-10) |
| :--- | :--- | :--- | :--- |
| **Hit Pause (Sleep)** | Kritik vuruşlarda oyunu 0.05 ile 0.2 saniye tamamen dondurur. Vuruşun "ağırlığını" hissettirir. | Tüm yakın dövüş / Büyük reaksiyonlar | 1 (Sadece Time.timeScale = 0) |
| **Directional Screen Shake** | Sadece rastgele titreme değil, hasarın geldiği/gittiği vektör yönünde sert sarsıntı ve yavaşça merkeze dönüş. | Kinetic Pinball, Vector Pierce | 3 (Cinemachine Impulse / Basit kod) |
| **Chromatic Aberration Spike** | Patlama anında RGB kanallarını saliseler içinde birbirinden ayırıp geri birleştirir. Reality-breaking hissi verir. | Chroma Weaver, Echo Cast | 4 (URP Volume manipülasyonu) |
| **Squash & Stretch** | Karakter hızlandığında uzar, yere çarptığında yassılaşır. Sadece kod ile `transform.localScale` manipülasyonu. | Polarity Shift, Kinetic Pinball | 2 (Tweening kütüphanesi ile) |
| **Flash on Hit (Beyazlama)** | Düşman hasar aldığında sprite material'i 1 frame tamamen saf beyaza döner. | Tüm Oyunlar | 2 (Basit bir Shader Graph / Material swap) |
| **Trail Renderers** | Hızlı hareket eden her şeyin arkasında kaybolan, renkten saydama giden bir kurdele bırakır. | Orbitect, Vector Pierce | 1 (Unity Built-in Bileşen) |
| **Color Inversion on Kill** | Haritayı temizleyen devasa bir boss veya combo kill yapıldığında 0.1 saniyeliğine tüm ekran renkleri negatife döner. | Chroma Weaver, Resonance Glyph | 5 (Custom Full Screen Pass / URP) |
| **Time Dilation (Slo-Mo)** | Büyük bir combo hazırlanırken zaman %20'ye düşer, combo patladığında %100'e döner. | Catalyst Drop, Tether Node | 2 (TimeScale Lerp) |
| **Procedural Particle Bursts** | Tek bir sprite'ı olmayan, sadece renkli piksellerden oluşan geometri patlamaları. | Hepsi | 3 (Unity Particle System / VFX Graph) |
| **Shockwave Ripple** | Bir noktadan dışa doğru yayılan ve altındaki pikselleri büken görünmez bir dalga. | Frequency Blade, Polarity Shift | 6 (Shader Graph distortion + Render Texture) |
| **Additive Blending (Glow)** | Üst üste binen mermilerin renkleri toplanarak saf beyaz parlamaya doğru gider. | Echo Cast, Chroma Weaver | 1 (Material Blend Mode: Additive) |
| **Floating Damage Numbers** | Sadece yukarı çıkan değil; hasar büyüklüğüne göre fontun büyümesi, titremesi, zıplaması. | Hepsi | 3 (Object Pooling + Tween) |
| **Camera Zoom Punch** | Büyük vuruş anında kameranın fov/size değerinin aniden daralıp esnek şekilde geri sekmesi. | Vector Pierce, Tether Node | 3 (Cinemachine veya Lerp) |
| **Ghost Echoes** | Hızlı Dash sırasında geride bırakılan ve yavaşça saydamlaşıp kaybolan sprite kopyaları. | Echo Cast, Vector Pierce | 2 (SpriteRenderer kopyalama) |
| **Dynamic Light Flashes** | Patlamaların anlık olarak 2D Işık (URP 2D Light) kaynağı yaratıp etrafı aydınlatması. | Catalyst Drop, Magicraft türevleri | 4 (Performans yönetimi/Pooling ister) |

---

## 4. "Herkesin Yapabileceği" Filtresi (Fizibilite)

Yapay zeka (Codex/Claude) ve PixelLab desteğiyle çalışan bir solo dev için en büyük tuzaklar: **Aşırı state yönetimi (oyun çökmesi), Karmaşık AI Pathfinding, ve Custom Shader cehennemidir.**

- **Zaman Tüketici (Elenmeli):** *Catalyst Drop* (Sıvı dinamikleri ve grid tabanlı reaksiyonlar AI ile debug etmesi çok zordur). *Echo Cast* (Kayıt/Tekrar mekanizması bellek yönetimi ve bug yuvasıdır).
- **Tasarım Balance Zorluğu (Riskli):** *Frequency Blade* (Oyuncuların ritim algısı farklıdır, müzik/ses entegrasyonu kod bazlı gecikmeler yaratabilir). *Resonance Glyph* (Poligon algılamaları performans yiyebilir).
- **Solo Dev Yaygın Hatalar:** Polish Trap (Sürekli VFX ekleyip core loop'u unutmak). Scope Creep (3 element yerine 15 element yapmaya çalışmak). Çözüm: **Sadece 3 mekanik tut.**

### Filtreden Sağ Çıkanlar (En Kolay -> En Zor)
1. **Kinetic Pinball:** Fiziği Unity çözer, asset sıfıra yakındır (sadece karakter ve duvarlar), hissiyatı VFX kütüphanesiyle şaha kalkar.
2. **Orbitect:** Dairesel matematik basittir (Trigonometri). Çakışmalar `OnTriggerEnter2D` ile çözülür. Kod mimarisi lineerdir.
3. **Chroma Weaver:** Sadece renk ID'leri ve tag karşılaştırması kullanır (`if color == red+blue`). State machine AI ile çok hızlı yazılır.
4. **Polarity Shift:** Sadece `AddForce` ve çekim mantığı.
5. **Vector Pierce:** Basit Raycast ve Transform.position manipülasyonu.

---

## 5. Sentez Top 3 (Önerilenler)

Bu vizyon ve sınırlamalar ("Basit Mekanik, Mantık, Görsel Şölen") altında geliştirilmesi en garanti ve pazarlama potansiyeli en yüksek 3 proje:

### 1. Chroma Weaver (Renkli Kaos)
* **Neden Öneriyorum?** Çünkü kodlaması tamamen **mantıksal kurallara** (logic gates) dayanır. Kırmızı+Mavi=Mor kodu yapay zeka için dünyanın en basit state machine'idir. Oyuncu içinse inanılmaz bir zeka/keşif oyunu hissi yaratır (Magicka hissi). Görsel şölen tamamen Particle System ve Additive Material ile bedavaya elde edilir. Asset gerektirmez, daire ve karelerle bile prototiplenebilir.

### 2. Kinetic Pinball (Hız ve Momentum)
* **Neden Öneriyorum?** Oyunun mekaniği **Unity'nin yerleşik fizik motorunun üzerine kuruludur.** Senin bir mekanik kodlamana gerek kalmaz, momentum ve sekme zaten doğaldır. VFX Kütüphanesindeki `Hit Pause`, `Screen Shake` ve `Trail Renderer` eklendiğinde anında profesyonel hissettirir. "Sadece zıpla ve sek" çok basit bir eylemdir ama 20 kere sekip ışık hızına ulaşmak TikTok'ta saniyesinde viral olur.

### 3. Orbitect (Yörünge Mühendisliği)
* **Neden Öneriyorum?** "Vampire Survivors ama sen kontrol ediyorsun" pazarındaki en güçlü alternatif. Sadece tek bir script (`RotateAround`) ile oyunun %50'si biter. Oyuncunun etrafında dönen renkli uyduların birbirine çarptıkça ekranı partiküllere boğması, "gözü yormayan ama ekranı dolduran" (VFX Spectacle) kuralına %100 uyar. Düşmanların AI'ı sadece "oyuncuya doğru yürü" olacağı için geliştirme süresi (12 hafta) kesinlikle aşılmaz.
