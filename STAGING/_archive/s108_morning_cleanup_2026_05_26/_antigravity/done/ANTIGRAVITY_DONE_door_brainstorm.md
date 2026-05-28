# RIMA Dungeon Threshold (Oda-Arası Geçiş) Orijinal Tasarım Kurgusu
> **Hazırlayan:** Antigravity (Codex/GPT 5.5 High)
> **Hedef Dosya:** `STAGING/ANTIGRAVITY_DONE_door_brainstorm.md`
> **Kapsam:** RIMA'nın "Echo Imprint Cascade" lore'una ve 35° izometrik yapısına uygun, Hades klonu olmayan özgün geçiş konseptleri ve mühendislik çözümleri.

---

## 1. Mevcut Durum Analizi & Eleştirel Bakış (`concepts/rift_threshold_*.png`)

Mevcut Codex çıktısında üretilen `rift_threshold_locked_act1.png` ve türevleri incelendiğinde, tasarımın temel olarak **"Stone Arch + Cyan Portal Plane"** (Taş Kemer + Mavi Portal Düzlemi) formülüne dayandığı görülmektedir. Bu kurgu aşağıdaki nedenlerden dolayı RIMA için ciddi bir **"Hades Klonu"** problemi teşkil etmektedir:

1. **Kemer Mimarisi (Architectural Framing):** Klasik granit kemer (stone arch), Hades'teki ödül kapılarının (Chamber Doors) mimari iskeletini doğrudan taklit etmektedir. Kemer, oyuncuda "bir antik mimar buraya kapı inşa etmiş" hissi uyandırır. RIMA'nın dünyası ise inşa edilmiş değil, **parçalanmış ve yırtılmış** bir gerçekliğe sahiptir.
2. **Statik Yön Bağımlılığı (Directional Inflexibility):** 35° izometrik bir duvara gömülü taş kemer, oyuncunun bakış açısına göre aşırı derecede yön bağımlıdır. Oyuncu kemerin "arkasından" geldiğinde 2D sprite düz kalacak ve derinlik hissi çökecektir.
3. **Lore Kopukluğu:** "Echo Imprint Cascade" (her ölümün arenayı yeniden yazması) teması, dünyada temiz, el işçiliği taş kemerler değil; zamansal ve mekansal kırılmalar, çökmeler ve yozlaşmalar gerektirir.

**Alternatif Çözüm Vizyonu:** Geçiş noktalarını duvara gömülü "kapı çerçeveleri" olarak tasarlamak yerine; **yerçekimine meydan okuyan, mekandan bağımsız ve fizik kurallarını bükebilen kırılma merkezleri** olarak yeniden hayal etmeliyiz.

---

## 2. Orijinal Threshold Konseptleri

### KONSEPT 1: The Echo Anchor Monolith (Yankı Çapası Monoliti)

```
        ▲ [Süzülen Obsidyen Parçaları]
       ▲ ▲
      ◄ █ ►  ◄─── [Merkezde Dikey Mavi Işık Sütunu]
     ▲  █  ▲
    █████████ ◄─── [Yerdeki Çatlaklardan Yükselen Enerji]
   /  / | \  \
```

* **Form (Geometri/Strüktür/Silüet):** Yerçekimine karşı koyarak havada süzülen, düzensiz obsidyen (veya antik granit) parçalarından oluşan dikey bir monolit/dikilitaş silüeti. Ortasında hiçbir fiziksel çerçeve yoktur; parçalar dikey bir **cyan enerji sütunu** etrafında yörüngede döner gibi süzülür. Kilitliyken parçalar birbirine yakındır ve enerji sönüktür. Açıldığında parçalar dışa doğru genişler ve dikey enerji sütunu parlayarak geniş bir geçit oluşturur.
* **Lore Framing:** RIMA'nın "Echo Imprint Cascade" lore'una göre bu monolitler, gerçekliğin kırılma noktalarında geçmiş ölümlerin birikmesiyle oluşan "zamansal çapa" (temporal anchors) noktalarıdır. Oda temizlendiğinde, bu çapa geçmiş hatıraları serbest bırakarak bir sonraki odanın kapısını aralar.
* **Room Type Adaptation Stratejisi:** *Geometry & Symbol Swap*
  * **Combat/Corridor:** Standart 3 süzülen parça, ince cyan enerji.
  * **Elite/Boss:** Daha fazla sayıda, üzerinde parıldayan antik runeler olan devasa parçalar. Boss odasında parçalar kendi ekseninde dönen bir girdap oluşturur.
  * **Chest/Merchant/Forge:** 
    * *Chest:* Altın-cyan karışımı parçacıklar yayan, daha kavisli bir silüet.
    * *Merchant:* Parçaların süzülerek adeta bir oturak/tezgah formu oluşturduğu, dost canlısı bir fener gibi parlayan kurgu.
    * *Forge:* Metalik, kor ateş rengiyle sarmalanmış obsidyenler, aralarından cyan yerine turuncu kıvılcımlar fışkırır.
  * **Curse/Event:**
    * *Curse:* Siyah dikenler tarafından sarılmış, kan kırmızı çürümeyle kaplı kararmış taşlar.
    * *Event:* Sarmal (double-helix) şeklinde süzülen rünlü taşlar.
* **8-Dir Uygulanabilirlik:** Tamamen **Direction-Invariant** (Yön bağımsız). Monolit duvara gömülü olmadığı, zemin üzerinde süzülen izole bir prop olduğu için oyuncu 8 yönden hangisinden gelirse gelsin görsel olarak mükemmel durur. Tek bir sprite ve `flipX` animasyon varyasyonu için fazlasıyla yeterlidir.
* **Hades'ten Farkı:** *Hades'te kapılar duvara gömülü, kalın çerçeveli ahşap/taş yapılardır. RIMA'da Monolit, duvar dışı, yerçekimsiz süzülen ve mekandan bağımsız duran dinamik bir çapa noktasıdır.*
* **Referans:** *Hyper Light Drifter* monolith'leri ve *Diablo IV* Waypoint'lerinin süzülen, parçalı antik yapısı.
* **Maliyet Tahmini:** 1 base form × 9 variant (Sprite swap + palette swap) = **18 PixelLab Generation Kredisi**.

---

### KONSEPT 2: The Imprint Scar / Floor Rift (İz Skarı / Taban Yarığı)

```
   _________________________ [Oda Zemini]
  /   ____   ____   ____   /
 /   / __ \ / __ \ / __ \ /  ◄─── [Zeminin İzometrik Ayrılması]
/___/ /  \// /  \// /  \//___
     \ \__/ \ \__/ \ \__/    ◄─── [Aşağıdaki Derin Cyan Boşluk/Void]
      \________________/
```

* **Form (Geometri/Strüktür/Silüet):** Dikey hiçbir yapı içermez. Doğrudan zemin karolarının (floor tiles) yırtılarak birbirinden ayrılmasıyla oluşan izometrik bir yarıktır. Kilitliyken ince, sönük dumanlar çıkaran bir fay hattı gibidir. Açıldığında karolar izometrik eksende iki yana kayar ve alttaki bottomless (dipsiz) cyan boşluk ortaya çıkar. Boşluktan yukarıya doğru süzülen pikselli cyan parçacıklar yükselir.
* **Lore Framing:** Oyuncunun her ölümü zeminde kalıcı bir "iz" (imprint) bırakır. Bu yarıklar, gerçekliğin en zayıf olduğu fay hatlarıdır. Oyuncu geçiş yapmak için bu yarığın içine (zamansal boşluğa) adım atar ve adeta bir sonraki odaya "düşer".
* **Room Type Adaptation Stratejisi:** *Texture Overlay & Particle Swap*
  * **Combat/Corridor:** Standart düz çizgi yarık, cyan sis.
  * **Elite/Boss:** Dairesel, devasa bir çöküntü. Boss geçişinde yerdeki karolar havada süzülerek bir girdap oluşturur.
  * **Chest/Merchant/Forge:**
    * *Chest:* Yarığın kenarlarında parıldayan cyan kristal jeotları (geodes).
    * *Merchant:* Üzerine derme çatma ahşap bir köprü kurulmuş, yanında gaz lambası duran evcilleştirilmiş yarık.
    * *Forge:* Yarığın içinden cyan lav / kor ateş süzülen metal ızgaralı tasarım.
  * **Curse/Event:**
    * *Curse:* Yarığın kenarlarından sızan koyu kırmızı/mor temporal çürüme sıvısı.
* **8-Dir Uygulanabilirlik:** Karolar izometrik 2:1 oranında durduğundan, sadece iki ana duvar ekseniyle hizalanması gerekir (Kuzey-Batı -> Güney-Doğu veya Kuzey-Doğu -> Güney-Batı). Tek bir izometrik yarık sprite'ı ve bunun `flipX` kopyası, 8 yönden gelen oyuncular için kusursuz derinlik hissi sağlar.
* **Hades'ten Farkı:** *Hades'te geçişler her zaman dikey düzlemdedir ve odanın sınırındadır. RIMA'da Imprint Scar tamamen yatay düzlemdedir, zemin karolarının bir parçasıdır ve odanın tam ortasında bile konumlandırılabilir.*
* **Referans:** *Bastion*'ın havada inşa edilen/yıkılan yolları ve *Diablo* zindanlarındaki cehennem yarıkları.
* **Maliyet Tahmini:** 1 flat tile mask template × 9 variants = **12-15 Generation Kredisi**.

---

### KONSEPT 3: The Resonance Mirror (Rezonans Aynası)

```
        /\
      /    \   ◄─── [Çerçevesiz, Havada Süzülen Sıvı Cam]
     |  ░░  |  ◄─── [İçinde Bir Sonraki Odanın Pikselli Silüeti]
      \    /
        \/
        ||     ◄─── [Yerdeki Çatlağa Bağlanan Cyan Enerji Bağları]
```

* **Form (Geometri/Strüktür/Silüet):** Fiziksel bir çerçevesi olmayan, havada asılı duran, oval veya baklava (diamond) şeklinde sıvımsı bir cam kırığı. Kilitliyken kırık, mat ve karanlıktır (statik gürültü veya oyuncunun ölüm anını yansıtır). Açıldığında sıvı cam dalgalanır ve berraklaşarak içinde bir sonraki odanın ödülünün veya yapısının çok düşük çözünürlüklü, pikselli cyan yansımasını gösterir.
* **Lore Framing:** Bu aynalar, "Echo Imprint Cascade"in zamansal yansımalarıdır. Oyuncunun geçmiş yaşamlarının ve gelecek olasılıklarının kesiştiği yüzeylerdir. Camın içine girmek, zaman döngüsünde bir sonraki adıma geçmektir.
* **Room Type Adaptation Stratejisi:** *Visual Content & Shape Swap*
  * **Combat/Corridor:** Standart oval form, düz mavi cam.
  * **Elite/Boss:** 
    * *Elite:* Kenarları keskin cam kıymıklarıyla çevrili hırçın ayna.
    * *Boss:* Üç parçalı (triptych) devasa kırık ayna yapısı.
  * **Chest/Merchant/Forge:**
    * *Chest:* Altın sarısı rezonansla parlayan ve içinde sandık silüeti beliren ayna.
    * *Merchant:* Yuvarlak, yavaşça dönen, kenarlarında tılsımlar asılı ayna.
    * *Forge:* Metal kelepçelerle tutturulmuş, içinden dumanlar tüten ayna.
  * **Curse/Event:**
    * *Curse:* Kırmızı rünlerle çizilmiş, çatlaklarından siyah kan sızan ayna.
* **8-Dir Uygulanabilirlik:** Ayna **Billboard** (her zaman kameraya bakan) şeklinde kurgulanmıştır. 35° izometrik açıyla hafifçe öne eğiktir. Kamera açısına kilitli olduğu için oyuncunun yaklaşma yönünden tamamen bağımsızdır (Direction-Invariant). 
* **Hades'ten Farkı:** *Hades'te kapının ne vereceği tepesindeki statik ikonla bellidir. RIMA'da ayna diejetik olarak bir sonraki odanın yansımasını kendi içinde pikselli bir su yüzeyi gibi gösterir.*
* **Referans:** *Hyper Light Drifter* teleport panelleri ve *Diablo IV* Lilith aynaları.
* **Maliyet Tahmini:** 1 billboard template × 9 variants = **10 Generation Kredisi**.

---

### KONSEPT 4: The Chrono-Crypta Wall Seam (Zaman Aşımı Duvar Yarığı)

```
  [Taş Duvar]      [Süzülen Bloklar]      [Taş Duvar]
  ███████████      ▄   ▄   ▄    ▄      ███████████
  ███████████     ▐█▌ ▐█▌ ▐█▌  ▐█▌     ███████████
  ███████████ ◄───  \   |   /   / ◄─── ███████████
  ███████████       [Cyan Rift Seam]   ███████████
```

* **Form (Geometri/Strüktür/Silüet):** Duvar sistemine entegre ancak klasik bir kemer yerine, duvarda meydana gelmiş **şiddetli ve kararsız bir yapısal çöküntü**. Kapı açıklığını oluşturan devasa taş bloklar havada asılı kalmış, patlama anında donmuş gibi süzülmektedir. Blokların arasındaki boşluktan yoğun bir cyan temporal ışık sızmaktadır. Kilitliyken bloklar sıkışık ve kapalıdır. Açıldığında bloklar birbirinden uzaklaşarak süzülür ve aradaki yırtık genişler.
* **Lore Framing:** Zindanın duvarları tekrarlanan ölümlerin (Echo Cascade) yarattığı entropiye dayanamayarak parçalanmaktadır. Geçiş, inşa edilmiş bir kapı değil; çöken duvarın zamansal enerjiyle dondurulmuş halidir.
* **Room Type Adaptation Stratejisi:** *Geometry & Accent Swap*
  * **Combat/Corridor:** Klasik süzülen taş bloklar, standart yırtık.
  * **Elite/Boss:** 
    * *Elite:* Zincirlerle bağlı bloklar, kilit açıldığında zincirler kopup havada süzülür.
    * *Boss:* İki yandaki devasa heykellerin kırılıp havada süzülen parçaları arasından geçiş.
  * **Chest/Merchant/Forge:**
    * *Chest:* Altın yaldızlı bloklar, aralarından sızan parlak ışık huzmeleri.
    * *Merchant:* Üzerine renkli tenteler ve fenerler asılmış süzülen taşlar.
    * *Forge:* Kor haline gelmiş bloklar, aralarından sızan cyan buhar.
  * **Curse/Event:**
    * *Curse:* Koyu obsidyen bloklar, üzerlerinden akan kırmızı temporal sıvılar.
* **8-Dir Uygulanabilirlik:** İzometrik duvar sistemine gömülü olduğundan iki temel duvar açısı vardır (Kuzey-Batı ve Kuzey-Doğu duvarları). Tek bir yön sprite seti çizilip `flipX` yapıldığında, duvar yerleşim kurallarıyla mükemmel eşleşir. Derinlik sıralaması (SortingOrder) sayesinde oyuncu süzülen taşların "arasından" geçer.
* **Hades'ten Farkı:** *Hades'teki kapılar kusursuz işçilikle yapılmış saray kapılarıdır. Chrono-Crypta ise patlamış, havada asılı duran enkaz bloklarından oluşan dinamik bir duvar yırtığıdır.*
* **Referans:** *Bastion*'ın havada duran mimari enkaz estetiği.
* **Maliyet Tahmini:** 2 wall direction × 9 variants = **18-24 Generation Kredisi**.

---

## 3. Karşılaştırmalı Değerlendirme & Final Tavsiye

Aşağıdaki matris, RIMA'nın teknik kısıtları (PixelLab bütçesi, 8-yön performansı, lore uyumu ve özgünlük) doğrultusunda konseptleri puanlamaktadır:

| Tasarım Kriteri | K1: Echo Monolith | K2: Imprint Scar | K3: Resonance Mirror | K4: Chrono-Crypta |
| :--- | :---: | :---: | :---: | :---: |
| **Özgünlük (Hades Klonu Değil)** | ★★★★★ | ★★★★★ | ★★★★☆ | ★★★★☆ |
| **8-Yön / Açı Bağımsızlığı** | ★★★★★ | ★★★★☆ | ★★★★★ | ★★★☆☆ |
| **Production Bütçe Dostu** | ★★★★☆ | ★★★★★ | ★★★★★ | ★★★☆☆ |
| **Echo Imprint Lore Uyumu** | ★★★★☆ | ★★★★★ | ★★★★★ | ★★★★☆ |
| **Görsel "Wow" Etkisi** | ★★★★☆ | ★★★★★ | ★★★★☆ | ★★★★★ |
| **GENEL SKOR** | **92/100** | **96/100** | **88/100** | **80/100** |

### FINAL TAVSİYE: KONSEPT 2 — The Imprint Scar / Floor Rift

RIMA için en güçlü, en özgün ve bütçe dostu çözüm **KONSEPT 2 (The Imprint Scar)** tasarımıdır. Gerekçelerimiz:

1. **Anti-Hades İmzası:** Dikey kemer ve kapı tasarım dilini tamamen reddederek yatay düzleme (zemine) iner. Hades'i çağrıştırma ihtimali sıfırdır.
2. **Kusursuz Mühendislik Entegrasyonu:** İzometrik 2:1 karo ızgarasına (tile grid) doğrudan oturduğundan, URP 2D sprite sorting hatası yapma riski en düşük olan tasarımdır. Oyuncu yarığın üstüne basar veya içine dalar; duvar arkasında kalma veya yanlış sprite açısı gösterme problemi yaşanmaz.
3. **Lore Entegrasyonu:** "Echo Imprint Cascade" (Ölümün odayı yazması) kurgusuna en doğrudan hizmet eden tasarımdır. Yerdeki yarıklar, oyuncunun geçmişteki ölümlerinin yarattığı mekansal kırılmaları temsil eder.
4. **Bütçe Verimliliği:** Duvar mimarisi gerektirmediği için PixelLab generation işlemlerinde perspektif sapması (perspective drift) yaşanmaz. 2D düzlemsel bir tile mask ve sprite ile iş çözülebilir.

---

## 4. Bonus: 8-Dir Uygulanabilirlik İçin Pragmatik Engineering Önerileri

2D Pixel Art izometrik oyunlarda 8 yönlü oyuncu hareketi varken geçişlerin (threshold) sırıtmaması için uygulanabilecek akıllı teknik yöntemler:

### A. URP 2D Custom Shader: "Depth-Aware Void Mask" (Derinlik Maskesi)
Geçiş noktaları için basit bir sprite render etmek yerine, zemindeki karonun altına render edilen bir **Stencil Mask / Sprite Mask** kullanın.
* **Nasıl Çalışır:** Oyuncu yarığın üzerine geldiğinde, oyuncunun ayak bastığı pivot noktası yarığın derinliğinin içine girer. Custom shader, oyuncunun ayaklarını yarığın içindeki cyan sisin arkasında maskeler (fade-out).
* **Faydası:** Oyuncu geçide girdiğinde adeta yarığın "içine batıyor" veya "aşağı düşüyor" izlenimi yaratılır. Bu, 2D sprite'a 3D derinlik kazandırır.

```
 [Player Sprite] ◄─── (Yarığa girerken alt kısmı maskelenir)
 ═══════════════ ◄─── [Floor Level]
   \   Void  /
    \_______/    ◄─── [Yarığın İçi - Shader ile render edilen alan]
```

### B. "Symmetric Billboard" Tekniği (Monolith ve Mirror İçin)
Eğer **Monolith** veya **Mirror** seçilirse, sprite'ı Unity'de 3D düzlemde kameraya doğru hafifçe tilt edin (yaklaşık -35 derece).
* **Nasıl Çalışır:** `Transformed Sprite` bileşeni her zaman kameranın ortografik açısına paralel kalır. 
* **Faydası:** Oyuncu geçidin etrafında hangi yöne dönerse dönsün, portal her zaman ona doğru bakar. Bu yöntem *Don't Starve* ve *Ragnarok Online* gibi klasik izometrik oyunlarda 2D nesnelerin 3D gibi durmasını sağlayan altın standarttır.

### C. Universal Dynamic Cyan Overlay Shader (Room Type Tasarrufu)
9 farklı oda tipi için 9 farklı sprite seti üretmek yerine:
* **Teknik:** Sadece **1 adet gri tonlamalı (greyscale) base sprite** üretin.
* **Shader Görevi:** URP 2D Sprite Custom Shader kullanarak bu gri sprite üzerine oda tipine göre dinamik renk ve emisyon (emission glow) basın.
  * `Combat` -> Cyan (#00FFCC) renk, yavaş pulse.
  * `Forge` -> Turuncu/Kor (#FF4500) renk, hızlı kıvılcım gürültüsü (noise).
  * `Curse` -> Kan Kırmızı/Mor (#8B0000) renk, dalgalanan (sine wave) yavaş emisyon.
* **Bütçe Tasarrufu:** **%90 oranında PixelLab kredisi tasarrufu sağlar.** 1 sprite × 1 shader = 9 benzersiz varyasyon!

---

## 5. Uygulama Adımları (Next Steps)

1. **Tasarım Kararının Onaylanması:** Orchestrator (Claude) ve Yazar, yukarıdaki 4 konsept arasından nihai kararı verir (Tavsiyemiz: **Konsept 2 - Imprint Scar**).
2. **Base Sprite Üretimi:** Seçilen konseptin gri tonlamalı (greyscale) master şablonu PixelLab aracılığıyla üretilir.
3. **Shader ve VFX Kurulumu:** Zemin yırtığı ve içindeki dinamik cyan rift akıntısı için URP Shader Graph hazırlanır.
4. **Varyasyonların Entegrasyonu:** `SubRoomSequenceController` üzerinde oda tipine göre materyal/renk ataması tetiklenir.

---
*RIMA dünyasının parçalanmış gerçekliğini kapılarla değil, gerçekliğin kendi yaralarıyla yansıtın.*
