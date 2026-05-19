# RIMA: THE ULTIMATE GAME FEEL & MECHANICS BIBLE
*Bu doküman, sektördeki en başarılı ARPG, Roguelite, MMORPG ve Platform oyunlarından derlenmiş, RIMA'nın oynanışını "AAA Indie" standartlarına ulaştırmak için tasarlanmış eksiksiz ve nihai mekanik ansiklopedisidir.*

---

## BÖLÜM 1: KONTROL, HAREKET VE AKICILIK (MOVEMENT & TRAVERSAL)
*Bir oyunun iyi olup olmadığı, karakterin boş bir odada yürürken verdiği histen anlaşılır.*

### 1.1. Input Buffering (Dead Cells / Super Smash Bros)
*   **Oyun Örneği:** *Dead Cells*
*   **Nasıl Çalışır:** Oyuncu kılıç sallama animasyonunun bitmesine 0.2 saniye kala "Takla (Dash)" tuşuna basarsa, oyun bu komutu yok saymaz. Bir "Buffer" (bekleme) havuzuna atar ve kılıç animasyonu bittiği milisaniye taklayı tetikler.
*   **RIMA'ya Entegrasyonu:** Action kuyruklama (Queue) sistemi yazılmalıdır. Oyuncu asla "tuş basmadı" hissine kapılmaz, kontroller "yağ gibi" (buttery smooth) hissedilir.

### 1.2. Coyote Time & Jump Leniency (Celeste / Dead Cells)
*   **Oyun Örneği:** *Celeste*
*   **Nasıl Çalışır:** Karakter bir çıkıntının (ledge) ucundan boşluğa düştükten sonraki ilk 5 frame (veya 0.1 saniye) içinde oyuncu hala "Zıplama/Dash" tuşuna basabilir. Oyun fiziken boşlukta olsan bile seni affeder.
*   **RIMA'ya Entegrasyonu:** Alabaster Dawn tarzı boşluklardan otomatik atlama (Auto-jump) yaparken veya tuzakların üzerinden geçerken oyuncuyu affeden 0.1 saniyelik gizli tolerans pencereleri.

### 1.3. Corner Rounding (Köşe Yuvarlama)
*   **Oyun Örneği:** *Enter the Gungeon*
*   **Nasıl Çalışır:** Karakter dar bir koridora girerken omzunun ucu duvara çarparsa karakter durmaz. Çarpışma motoru (Collider) karakteri otomatik olarak boşluğa doğru kaydırır.
*   **RIMA'ya Entegrasyonu:** Zindan kapılarından veya kayaların arasından geçerken sıfır takılma hissi sağlar. Vector2.Slide mekaniği ile çözülür.

### 1.4. Z-Ekseni İllüzyonu (Alabaster Dawn / CrossCode)
*   **Oyun Örneği:** *CrossCode*
*   **Nasıl Çalışır:** 2D Top-Down oyunlarda "Zıplama" aslında Y ekseninde bir sahte kavis çizmektir. Altında gölge (Shadow) sabit bir hatta ilerlerken karakter Sprite'ı yukarı kalkıp iner.
*   **RIMA'ya Entegrasyonu:** Uçurumlu odalar tasarlanır. Uçuruma gelindiğinde Dash tuşu karakterin gölgesini karşıya yollarken, Sprite havada kavis çizer. Odaya 3D derinlik (Z-Axis) kazandırır.

---

## BÖLÜM 2: SAVAŞ, VURUŞ HİSSİ VE "JUICE" (COMBAT FEEL)
*Vuruşların kağıt keser gibi değil, balyoz gibi hissettirmesi.*

### 2.1. Hit-Stop ve Hit-Lag (Monster Hunter / Hollow Knight)
*   **Oyun Örneği:** *Monster Hunter*
*   **Nasıl Çalışır:** Dev bir kılıç düşmana çarptığında oyunun zamanı (Time.timeScale) kelimenin tam anlamıyla 0.05 ile 0.15 saniye arası tamamen durur.
*   **RIMA'ya Entegrasyonu:** Ravager çift baltayla vurduğunda, balta düşmanın bedenine temas ettiği o Frame'de oyun donmalıdır. Bu, vuruşun ağırlığını (momentum) oyuncuya fiziksel olarak hissettirir.

### 2.2. Directional Screen Shake (Yönlü Ekran Titremesi)
*   **Oyun Örneği:** *Nuclear Throne*
*   **Nasıl Çalışır:** Ekran rastgele titremez. Silah ne tarafa ateşlendiyse (veya kılıç ne tarafa savrulduysa), kamera tam zıt yöne sarsılıp geri gelir.
*   **RIMA'ya Entegrasyonu:** Vuruş yönüne göre (Vector2 impulse) kamerayı sarsan Cinemachine Impulse sistemi kurulmalıdır.

### 2.3. Dash-Strike ve Animation Canceling (Hades)
*   **Oyun Örneği:** *Hades*
*   **Nasıl Çalışır:** Hiçbir saldırının "Recovery" (silahı geri çekme) animasyonunu beklemek zorunda değilsindir. Saldırı hasarı vurduğu an Dash atarak animasyon iptal edilir.
*   **RIMA'ya Entegrasyonu:** Oyuncunun saldırı sonrasındaki hantallığını siler atar. Savaşlar "Vur-Kaç-Vur" şeklinde dans gibi akar.

### 2.4. Hitbox Leniency (Kayıran Çarpışma Kutuları)
*   **Oyun Örneği:** *Dead Cells*
*   **Nasıl Çalışır:** Düşman mermilerinin Hitbox'ı merminin grafiğinden %10 daha küçüktür (Sıyrılmak kolaydır). Oyuncunun kılıcının Hitbox'ı grafiğinden %15 daha büyüktür (İsabet ettirmek kolaydır).
*   **RIMA'ya Entegrasyonu:** Oyuncunun kendini bir "Tanrı" gibi hissetmesini sağlayan görünmez bir psikolojik tasarım numarasıdır.

---

## BÖLÜM 3: YAPAY ZEKA VE KALABALIK YÖNETİMİ (AI & CROWD CONTROL)
*Kaos iyi tasarlanmadığında adaletsizlik hissiyatı verir.*

### 3.1. Attack Token System (Saldırı Jetonları)
*   **Oyun Örneği:** *DOOM (2016) / Assassin's Creed*
*   **Nasıl Çalışır:** Odada 20 düşman vardır ama oyunun sadece 2 adet "Melee Token" ve 1 adet "Ranged Token"ı (Saldırı Jetonu) vardır. Sadece elinde Jeton olan düşmanlar saldırır, diğerleri oyuncunun etrafında tehditkar şekilde dolaşır.
*   **RIMA'ya Entegrasyonu:** Oyuncunun 20 düşman tarafından aynı saniyede öldürülmesini engeller. Savaş adil, sinematik ve ritmik hale gelir.

### 3.2. Flanking ve Kiting Dinamikleri
*   **Oyun Örneği:** *Left 4 Dead*
*   **Nasıl Çalışır:** Uzak dövüşçüler (Okçular) oyuncu onlara yaklaşınca "Kiting" (Geri çekilerek vurma) yapar. Yakın dövüşçüler ise oyuncunun direkt üstüne koşmak yerine sağdan ve soldan etrafını sarmaya (Flanking) çalışır.
*   **RIMA'ya Entegrasyonu:** Yapay zeka dümdüz bir Pathfinding (yol bulma) yerine, oyuncunun baktığı açının tersine doğru hareket etmeye çalışmalıdır.

---

## BÖLÜM 4: BOSS MEKANİKLERİ VE RAID SİSTEMLERİ (MMORPG -> ROGUELITE)
*Boss savaşları çok canı olan süngerler değil, ölümcül bulmacalardır.*

### 4.1. Stagger / Break Gauge (Denge Çubuğu)
*   **Oyun Örneği:** *Lost Ark / Sekiro*
*   **Nasıl Çalışır:** Boss ölümcül bir saldırıya hazırlanırken altında sarı bir bar (Stagger Bar) çıkar. Oyuncu 4 saniye içinde o barı sıfırlayacak kadar "Ağır Hasar" vurursa saldırı iptal olur ve boss sersemler.
*   **RIMA'ya Entegrasyonu:** Oyuncuyu kaçmak yerine tehlikenin üzerine gidip risk almaya teşvik eden muazzam bir combat ritmidir.

### 4.2. Pixel-Perfect Telegraphing (Görsel İşaretçiler)
*   **Oyun Örneği:** *Final Fantasy XIV (FFXIV)*
*   **Nasıl Çalışır:** Saldırının geleceği zeminler, saldırıdan 2-3 saniye önce Neon Kırmızı (geometrik olarak kusursuz) alanlarla boyanır (Daire, Koni, Çapraz). İçi yavaşça dolar, dolduğunda patlar.
*   **RIMA'ya Entegrasyonu:** Oyuncu hasar yediğinde asla "Nereden geldiğini görmedim" demez. Adil ölüm hissiyatı yaratır. Ayrıca "Donut AoE" (sadece bossun dibinin güvenli olduğu alanlar) oyuncunun reflekslerini şaşırtır.

### 4.3. Tethering (Zincirleme) ve Arena Değişimleri
*   **Oyun Örneği:** *World of Warcraft / Path of Exile*
*   **Nasıl Çalışır:** Boss oyuncuya kandan bir lazer zincir atar. Oyuncu zıt yöne koşarak (belli bir mesafeyi aşarak) zinciri koparana kadar boss ölümsüz olur. Veya Boss %50 cana geldiğinde arenanın ışıklarını söndürür (Phase 2), oyuncu sadece yeteneklerin ışığıyla savaşmak zorunda kalır.
*   **RIMA'ya Entegrasyonu:** Boss savaşlarını tek düze olmaktan çıkarıp epik bir faza taşır.

---

## BÖLÜM 5: ÇEVRE VE ELEMENT SİNERJİLERİ (ENVIRONMENTAL CHEMISTRY)
*Zindan sadece bir zemin değil, taktiksel bir silahtır.*

### 5.1. Chemistry Engine (Kimya Motoru)
*   **Oyun Örneği:** *Divinity: Original Sin 2 / Zelda: Breath of the Wild*
*   **Nasıl Çalışır:** Çevredeki elementler birbiriyle tepkimeye girer.
*   **RIMA'ya Entegrasyonu:** 
    *   *Su Birikintisi + Elektrik Skilli* = Suyun içindeki herkes Stunlanır (Şok alanı).
    *   *Yağ Birikintisi + Ateş Skilli* = Tüm yağ alev alır, saniye başı hasar veren bir duvar oluşturur.
    *   *Zehir Birikintisi + Patlama Skilli* = Zehirli bulut odaya yayılır.
    *   Böylece "Su Elementalist" ve "Ateş Elementalist" karakterlerinin oynanışı tamamen farklılaşır.

### 5.2. Çevresel Silahlar (Destructibles & Hazards)
*   **Oyun Örneği:** *Spelunky*
*   **Nasıl Çalışır:** Etraftaki kazık tuzakları sadece oyuncuya değil, düşmana da hasar verir. Oyuncu düşmanı Knockback (Geri itme) yeteneği ile kazıklara fırlatıp tek atabilir. Etraftaki kırılabilir testiler, mermileri engellemek için siper olarak kullanılabilir.

---

## BÖLÜM 6: PSİKOLOJİ, SES VE GÖRSEL MÜHENDİSLİK (VFX & AUDIO)
*Gözün gördüğünü kulak doğrulamazsa, oyun hissi sahtedir.*

### 6.1. Audio Ducking (Ses Boğulması)
*   **Oyun Örneği:** *DOOM*
*   **Nasıl Çalışır:** Çok büyük bir yetenek patladığında (veya boss kükrediğinde) oyunun arka plan müziği ve ufak sesleri saniyelik olarak %50 kısılır (Low-pass filter). Patlamanın sesi yankılanır. Bu, beynin gücü algılamasını 10 kat artırır.

### 6.2. Pitch Shifting (Rastgele Ses Tonu Değişimi)
*   **Nasıl Çalışır:** Oyuncu 10 kere kılıç salladığında aynı "Svuş" sesi çalarsa beyin 3. vuruştan sonra sahteliği anlar. Kılıç sesinin Pitch (İncelik/Kalınlık) değeri her vuruşta 0.9 ile 1.1 arasında rastgele değiştirilmelidir.

### 6.3. Squash & Stretch (Esneklik ve Canlılık)
*   **Nasıl Çalışır:** Karakter yere düştüğünde Sprite 0.1 saniyeliğine Y ekseninde kısalır (basılır), X ekseninde uzar (yayılır). Zıplarken ise Y ekseninde uzar. Bu Disney'in 12 temel animasyon kuralından biridir. 

### 6.4. Loot Psychology (Ganimet Hissiyatı)
*   **Oyun Örneği:** *Diablo 3 / Path of Exile*
*   **Nasıl Çalışır:** Efsanevi (Legendary) bir eşya düştüğünde tok, tatmin edici bir "Tınnn" sesi çalar ve eşyadan havaya doğru belirgin bir ışık huzmesi yükselir. Bu pavlov şartlanması yaratır.

---

## BÖLÜM 7: RIMA ÖZEL ÇÖZÜMLER (MİMARİ YAKLAŞIM)
PixelLab AI boru hattımızın getirdiği (1-Piece karakter) zorunluluğunu aşmanın ve oyun hissini korumanın hileleri.

### 7.1. "Silahı Sırta Asma" İkilemi ve Çözümü
Kullanıcının *"Güvenli odada baltaları arkaya alma efekti daha sade gelir mi?"* sorusunun nihai mühendislik cevabı:
*   **Neden Zor?** 1-Piece piksel kuralında silah karakterin parçasıdır. Bunu yapmak her karakter için ekstra 4 (yön) x 2 (animasyon) = 8 yeni AI üretimi demektir. 16 karakterde 128 resimlik bir enflasyon yaratır.
*   **Çözüm 1: Shader Dissolve (Büyülü Yok Oluş):** Kod ile silahın olduğu renk paletleri algılanır ve sadece savaş dışındayken silah "dijital piksellere veya büyü tozuna" dönüşerek şeffaflaşır. Savaşa girildiğinde parlayarak ele gelir.
*   **Çözüm 2: Hades Pacing (Psikolojik Rahatlama):** Zagreus silahı hiç bırakmaz. RIMA'da karakter güvenli odaya girdiğinde karakter hızını (Movement Speed) %30 artırıp, kamerayı geriye çekersek (Zoom Out) ve müziği sadece Ambient (Ambiyans) seviyesine indirirsek; oyuncu "Silah elinde olmasına rağmen" %100 güvende ve keşif modunda hisseder. En masrafsız ve etkili çözüm budur.
