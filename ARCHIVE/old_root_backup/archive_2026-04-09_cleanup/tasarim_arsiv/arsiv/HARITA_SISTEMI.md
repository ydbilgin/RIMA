# HARİTA SİSTEMİ — TASARIM DOKÜMANI
*2026-03-27 | Referans oyun analizi → bizim kısıtlarımız → karar*
*Kaynak: Ollama deepseek-r1:14b 7 bölüm + Claude sentezi*

---

## BÖLÜM 1 — REFERANS OYUNLAR NE YAPIYOR VE NEDEN?

Her oyunun harita sistemi o oyunun **core fantasy'sine** hizmet ediyor.
Anlamadan kopyalamak işe yaramaz. Önce neden o karar, sonra bizim için ne ifade ediyor.

---

### 1.1 HADES 1 — "Harita yok, ama hep yön biliyorsun"

**Sistem nasıl çalışıyor:**
- Ekranda hiçbir zaman tam harita görmüyorsun
- Her odada: 1-2 çıkış kapısı var, kapının üstünde **ödül ikonu** gösterilir (kalp, skill, para...)
- Ne seçeceğini ödüle bakarak seçersin — nereye gittiğini değil, ne alacağını seçersin
- Lineer ilerleme: Tartarus → Asphodel → Elysium → Styx → Boss
- Her "chamber"da 2-4 oda var, sıralı — kaçma yolu yok, sapma minimal
- Chaos kapısı (nadiren): farklı bir odaya geçiş, farklı ödül tipi

**Supergiant neden bu kararı aldı:**
- Oyunun narratifi "Zagreus yeraltından kaçmaya çalışıyor" — harita görmek bu fanteziye zarar verir
- Belirsizlik = gerilim. Her kapıyı açmak bir keşif hissettiriyor
- Ödül önizlemesi yeterli — "nereye gidiyorum" değil "ne alıyorum" sorusu odak noktası
- Binary choice (2 kapı) karmaşayı önlüyor, ama yine de ajans hissini koruyor

**Oyuncuların sevdiği:**
- Her run farklı hissediyor çünkü oda içerikleri her seferinde surpriz
- Keşif hissi canlı kalıyor — "bakalım bu odada ne var"
- Narratifle mükemmel uyum — harita görmek anlatıyı kırar

**Oyuncuların eleştirdiği:**
- Run'ın ortasında "kaç elite kaldı, kaç shop kaldı" bilemiyorsun
- Grudge-style sistemler için yetersiz: "bunu nasıl programladım, ne zaman tekrar göreceğim" takibi imkansız
- İleri planlama neredeyse sıfır — her karar anında

**Bizim için ne ifade ediyor:**
- "Ödül önizlemesi, oda tipi gizli" yapısı iyi bir fikir — ama tam bu değil
- Grudge sistemi bu yapıyla çalışmaz: oyuncunun elitleri takip etmesi gerekiyor
- Kapı üstü ikon sistemi → bizde de olabilir (ama elite için Grudge badge de göster)

---

### 1.2 HADES 2 — "Harita eklendi, neden?"

**Hades 1'den ne değişti:**
- Gerçek görsel harita eklendi — her bölgenin node'larını görüyorsun
- Yüzey (Surface) ve Yeraltı (Underworld) olarak iki farklı ilerleme yönü
- Crossroads hub: her act başında harita ekranı açılır
- Node tipleri ikonla etiketlendi — elite, shop, boss, özel oda

**Neden bu değişikliği yaptılar:**
- Hades 1 oyuncularından gelen en büyük şikayet: "ne zaman shop geliyor, ne kadar kaldı bilmiyorum"
- Daha uzun ve karmaşık içerik (yeni bölgeler, yeni mekanikler) kör navigasyonu zorlaştırdı
- Oyuncuya daha fazla ajan vermek istiyorlardı — Hades 1 biraz "sürükleniyor" hissettiriyordu
- Early Access sürecinde oyuncu geri bildirimiyle harita detayı arttırıldı

**Ne feda ettiler:**
- Keşif gerilimine bir miktar zarar verdiler — oda içeriği hâlâ gizli ama "ne tipi oda" biliniyor
- "Her şeyi gördüm" hissi — bazı oyuncular Hades 1'in kör sistemini daha immersive buldu

**Bizim için ne ifade ediyor:**
- Harita eklenmesi neden gerekti: karmaşıklık arttıkça kör navigasyon oyuncu için strese dönüşüyor
- Bizim Grudge + Build Crafting mekaniği karmaşıklık gerektirir → harita gerekli
- "Oda tipi görünür, içerik gizli" dengesi Hades 2'nin çözümü — ve işe yarıyor

---

### 1.3 SLAY THE SPIRE — "Tüm haritayı görüyorsun, ama bu bir problem mi?"

**Sistem nasıl çalışıyor:**
- Act başında tüm harita ekranda — baştan sona tüm node'lar görünür
- ~15-17 node per act, her node bir oda tipini temsil ediyor (Normal/Elite/Rest/Shop/? Event/Boss)
- Birden fazla başlangıç yolu, ortada birkaç kez birleşip ayrılıyor, boss'a tek yoldan giriyor
- Elite odalar opt-in — etrafından dolaşabilirsin ama kart ödülü daha iyi
- "?" odası: event (loot, zarar, değişim) — bilinmezlik haritanın içine gömülmüş
- Oyuncu act başında tüm rotayı kafasında planlıyor

**Mega Crit neden bu kararı aldı:**
- StS bir DECK BUILDING oyunu — elindeki desteni yönetmek için uzun vadeli plan şart
- "3 elite sonra Rest odam var, oraya kadar dayanabilirim mi?" sorusu her an oynanıyor
- Tam görünürlük deck building meta-game'inin bir parçası
- İçerik (kartlar, relics) çok fazla — oyuncunun çeşitlilik planlaması yapması gerekiyor

**Güçlü yanları:**
- Maksimum ajans: her karar bilinçli
- Deneyimli oyuncular "optimal rota" hesaplıyor — bu onlar için eğlenceli
- "Ben bu rotayı seçtim ve işe yaradı/yaramadı" — net öğrenme döngüsü

**Zayıf yanları:**
- "Solved puzzle" hissi: rotalar matematiksel olarak optimal hale gelince sürpriz ölüyor
- Yeni oyuncular için harita ezici görünebiliyor
- Aksiyon oyunlarına taşındığında: harita analizi yapmak, sonra oynamak — ritim kırılabilir

**Bizim için ne ifade ediyor:**
- Bizim oyun "BUILD INSANE" anı hissettirmeli, "rota optimizasyonu çözdüm" değil
- Tam görünür harita bizi planlama oyununa dönüştürebilir — istemiyoruz
- Ama Elite'lerin NEREDE olduğunu görmek (Grudge için) istiyoruz — bu StS'in tam görünürlüğünden farklı
- Kısmen StS: oda tipleri görünür. Ama tüm harita bir anda ekranda değil.

---

### 1.4 DEAD CELLS — "Biome'lar arası seçim, biome içi keşif"

**Sistem nasıl çalışıyor:**
- Oyun biome'lardan (bölgelerden) oluşuyor — her biri ayrı bir level
- Biome içi: tek akış scrolling level — odalar değil, sürekli alan (ama çıkış kapıları var)
- Biome'lar arası: birden fazla geçiş yolu var — farklı biome seçimleri farklı zorluklara götürür
- Kilitli biome'lar: belirli rune'ları toplamadan erişilemiyor (meta progression)
- Oyuncu "bu biome'dan X çıkışını alaydım, Y biome'a gitseydim" kararlarını verir

**Motion Twin neden bu kararı aldı:**
- Dead Cells bir aksiyon oyunu — harita navigasyonu değil, akıcı savaş odak noktası
- Sürekli level design hareket momentumunu koruyor (StS gibi "dur, haritaya bak" yok)
- Biome çeşitliliği replayability sağlıyor — her run farklı görsel/mekanik deneyim
- Kilitli biome'lar: oyuncuya "daha fazlasını keşfet" hedefi veriyor

**Bizim için ne ifade ediyor:**
- Aksiyon odaklı oyun: harita navigasyonu zamanını minimize et, savaş zamanını maximize et
- "Bak, planla, gir" ekranı kısa tutulmalı
- Discrete odalar (bizim yapımız) Dead Cells'den farklı — ama "biome vibes = act vibes" düşüncesi geçerli
- Act tasarımında görsel/mekanik çeşitlilik önemli (sadece stat scaling değil)

---

### 1.5 ENTER THE GUNGEON — "Kata tam harita, serbestçe dolas"

**Sistem nasıl çalışıyor:**
- Her kat girince tüm kat haritası açılır — tüm odalar, bağlantılar görünür
- Odaları serbestçe keşfedersin — sıra yok, istediğin odaya git
- Geçtikçe odalar "temizlendi" olarak işaretlenir
- Özel odalar (shop, gizli, boss) haritada çıkmaz — orada bulursun
- Boss odası haritanın en altında, her zaman biliniyor

**İşe yarayan yanı:**
- Savaş oyunu için ideal: "hangi odaya gireyim" kararı hızlı ve intuitive
- Gizli odalar gerçekten gizli — haritaya bakarak değil, fiziksel duvar tıklayarak bulunuyor
- Serbestlik hissi: "bu odayı atlayabilirim, şu odaya gideyim"

**Bizim için ne ifade ediyor:**
- Tam serbest navigasyon bizim sisteme uymaz — oda ödülleri ve sıra önemli
- Ama "kat içi keşif serbestliği" FAZ 4-5'te düşünülebilir (şimdilik scope dışı)
- Gizli oda mekanik: duvar arkası değil, koşul bazlı (hasar almadan temizle → gizli geçiş açılır)

---

### 1.6 RISK OF RAIN 2 — "Seçim yok, ama neden işe yarıyor?"

**Sistem nasıl çalışıyor:**
- Sahne → teleporter bul → aktifleştir → wave yok et → sonraki sahneye geç
- Sahne sırası belirli: Distant Roost/Titanic Plains → Wetland Aspect → ... → Moon
- Hiçbir yol seçimi yok — ama sahne IÇINDE serbest dolaşım var
- Zaman baskısı: sahne içinde ne kadar kalırsan düşmanlar güçleniyor

**Neden seçimsizlik işe yarıyor:**
- RoR2'nin core loop'u item synergy keşfi — item'lar nereden geldiğini seçmiyorsun, item'larla ne yapacağını seçiyorsun
- Sahne içi keşif (item chest konumları, mekanikler) zaten yeterli ajans sağlıyor
- Hız temposu yüksek — sahne seçimi olsaydı tempo kırılırdı

**Bizim için ne ifade ediyor:**
- Seçimsizlik bizim için işe yaramaz — Grudge programlaması, build crafting seçim gerektiriyor
- AMA: RoR2'nin FAZ 1 prototipi için ideal — "lineer 5 oda" ilk implementasyon RoR2 mantığı
- Tempo dersi: harita navigasyonu combat tempusunu bozmamalı

---

### 1.7 BALATRO — "Spatial harita yok, ante/blind ile ritim"

**Sistem nasıl çalışıyor:**
- Spatial harita yok — sıralı "blind" sistemi: Small Blind → Big Blind → Boss Blind
- Bu 3 blind bir "ante" oluşturuyor (act gibi)
- Oyuncu small blind veya big blind'ı atlayabilir (skip) — ama atlarsan chip kazanıyorsun, kart seçimi kaçırıyorsun
- Skip: "daha az güçlenme, daha fazla resource" tradeoff
- Ante 1'den Ante 8'e çıkıldıkça zorluk artar

**Neden işe yarıyor:**
- Harita yok ama ritim net: "3 el oynayacaksın, sonra yeni aşama"
- Skip mekaniği: riskleri atlamak vs mükemmel hazırlık — bu kendi başına bir karar oyunu
- Minimalism: kart oyununun odak noktası kart seçimi, navigasyon değil

**Bizim için ne ifade ediyor:**
- "Ante/blind" yapısı = bizim act/oda yapısı ile paralellik
- Skip fikri: Grudge elitini kasıtlı atlamak isteyebilirsin — ama Soul Dust kaçırırsın → bu tradeoff anlamlı
- Minimalist ritim: her oda temizle → ödül → sonraki; bu bizde de var, haritanın bunu bozmamaması lazım

---

### 1.8 PATH OF EXILE (ATLAS) + DİABLO 2 — ARPG Dersleri

**PoE Atlas — Target Farming:**
- Atlas endgame'de oyuncu istediği encounter tipini hedefleyebiliyor
- Belirli boss'ları tekrar çağırmak için özel mekanikler var
- "Ben bu hafta X farming yapıyorum" — yön belirleme
- Bu bizim Grudge sistemiyle doğrudan örtüşüyor: "Ben bu eliti bu şekilde programladım, şimdi geri geldiğinde hazırım"

**D2 Act Yapısı — Bilinen iskelet + rastgele iç:**
- D2'de her act'in ana yolu belirli (Tristram, Act 2 çölü...)
- Ama her dungeon'ın iç layout'u random
- Sonuç: her run aynı hissettirmiyor, ama kaybolmuyorsun
- 20 yıldır hâlâ "taze" — bilinen framework + random iç = sürdürülebilir

**Nioh — Görev Öncesi Bilgi:**
- Nioh'da göreve girmeden önce: zorluk seviyesi, düşman listesi, özel mekanikler gösterilir
- Oyuncu tam hazırlanabilir
- Sonuç: daha az "ucuz ölüm" hissi, daha fazla "hazır değildim" hissi (fair)

**Bizim için ne ifade ediyor:**
- PoE target farming = Grudge programlaması ile Elite'i "hedefleme" aynı his
- D2 framework = bizim act yapısı; bilinen act çerçevesi + rastgele oda konumları
- Nioh bilgi öncesi = Elite girişinde Grudge badge + düşman bilgisi göster

---

## BÖLÜM 2 — BİZİM KISITLARIMIZ

Referans oyunları inceledik. Şimdi bizim özel durumumuza bakıyoruz.

### 2.1 Grudge Sistemi Ne Gerektiriyor?

```
Elite'i nasıl öldürdüğünü hatırlıyor.
Kasıtlı programlama yapabilirsin.

Bu çalışmak için oyuncunun şunu bilmesi lazım:
  a) Bu eliti daha önce gördüm mü? (Badge var mı?)
  b) Eğer gördüysem, nasıl programladım? (Hangi direnci kazandı?)
  c) Bu eliti bu run'da tekrar görecek miyim? (Ne zaman, hangi odada?)
```

**Hades 1 burada başarısız:** "Bu eliti daha önce gördüm mü?" sorusunu cevaplayamazsın.
**StS tam görünür harita burada başarılı:** Nerede elite olduğunu görürsün.
**Ama StS'de Grudge yok** — sadece oda tipi bilmek yeterli.

Bizim ihtiyacımız: **Elite odanın haritada görünmesi + badge'inin haritada gösterilmesi.**

#### Grudge Kapsam Kuralları (kesin kararlar)

```
KAPSAM: Run-bazlı — her run sıfırlanır. Meta-run hafızası YOK.
  → Roguelite'ın "temiz başlangıç" felsefesini korur.
  → Her run bağımsız. Geçen run'da ne programladığın bu run'da önemli değil.

AKT İÇİ KAPSAM: Act-bazlı aktarım.
  → Act 1'de nasıl öldürdüysen, Act 2 ve 3'te o hafızayla gelir.
  → Aynı act içinde 2 kez Elite aynı türse: ilk ölümden sonra badge aktif.

SIFIRLAMA: Her yeni run'da tüm Grudge hafızası sıfır.
  → Badge'ler, direnç kazanımları, programlamalar — hepsi run sonu temizlenir.
```

#### Nemesis Elite Sistemi

Her run başında bir Elite türü **Nemesis** olarak belirlenir.
Bu elite tüm 3 Act'te en az bir kez garantili çıkar.

```
Nemesis Elite özellikleri:
  • Run başında rastgele 1 elite türü seçilir
  • Act 1, Act 2, Act 3'te haritada garantili en az 1 kez görünür
  • Act geçiş ekranında "Nemesis: [İsim]" olarak gösterilir
  • Diğer elitler: rastgele, tekrarsız veya tek seferlik olabilir
  • Nemesis dışı elitler 2. kez çıkabilir ama garanti değil

Neden bu karar:
  → "Her elite türü 2 act çıksın" → solo dev için çok fazla content
  → En az 1 elite garantili recurring → Grudge programlaması anlamlı
  → 1 Nemesis = run'un hikayesi: bu sefer bu eliti takip ettin
```

**Nemesis Elite Karşılaşma Ödülleri (kümülatif):**

| Karşılaşma | Ödül |
|------------|------|
| Act 1 — İlk karşılaşma | Standart elite ödülü (Soul Dust +10 + garantili upgrade) |
| Act 2 — İkinci karşılaşma | +20 Soul Dust bonus + ekstra kart seçimi (3 yerine 4 seçenek) |
| Act 3 — Son karşılaşma | +40 Soul Dust bonus + skill upgrade fırsatı + "Nemesis Çözüldü" rozeti |

*Nemesis Çözüldü rozeti: run puanına +200 ekler, leaderboard'da görünür.*

### 2.2 Build Crafting Tempo Ne Gerektiriyor?

```
Run boyunca build evrimi:
  Oda 1-4  → Zayıf, keşif modunda
  Oda 5-11 → Build netleşiyor, proc'lar zincirleniyor
  Oda 12+  → "Bu build insane" anı

Her oda = bir build kararı (3 kart seçimi).
Yanlış sırada oda tipi = build fırsat kaçırdı.
```

Bu şu anlama gelir: **Shop'u ne zaman göreceğini bilmek istiyorsun.** Soul Dust biriktirdim, ne zaman harcayacağım?

Hades 1'de bu bilgi yok — frustrating olabilir.
StS'de tam görünür — ama biz action game'iz, "shop planlaması" yapma baskısı yaratmak istemiyoruz.

Bizim ihtiyacımız: **Shop odası haritada görünür, ama tam harita analizi gerektirmeyen bir format.**

### 2.3 Solo Dev Kapsamı Ne Gerektiriyor?

```
Karmaşık dallanma = daha fazla oda sayısı = daha fazla content
FAZ 1'de: sadece temel çalışsın
FAZ 2'de: gerçek sistem devreye girsin
FAZ 3+: polish ve genişletme
```

StS'in tam dallanma haritası solo dev için çok içerik demek.
Hades 1'in lineer sistemi en az içerik — ama Grudge ile uyumsuz.

Bizim ihtiyacımız: **Minimal ama anlamlı seçim — 2 yol, act başında görünür.**

### 2.4 Aksiyon Oyunu Temposu Ne Gerektiriyor?

```
Combat → Ödül seç → Haritaya bak → Yürü → Combat
          (5 saniye)  (15 saniye max)  (3 saniye)

Haritaya bakma süresi 15 saniyeyi geçerse ritim bozulur.
```

StS'in harita analizi 2-3 dakika sürebilir — deck builder için normal, aksiyon oyunu için tempo öldürücü.

Bizim ihtiyacımız: **Harita bilgisi hızlı okunabilir, kararlar hızlı alınabilir.**

---

## BÖLÜM 3 — KARAR

### Ana Karar: "Düğüm Haritası — Yarı Görünür"

```
Her act BAŞINDA harita açılır.
Haritada oda TİPLERİ görünür: N (Normal) / E! (Elite) / S (Shop) / B (Boss) / ✦ (Flux)
Elite odalar varsa Grudge Badge ikonu da görünür: 🔥❄⚡☠
Her kavşakta 2 yol arasında seçim.
İçeride KİM olduğu — girmeden bilinmez.
```

**Bu sistem neden doğru:**

| İhtiyaç | Çözüm |
|---------|-------|
| Grudge planlama | Haritada E! + Badge ikonu |
| Shop bütçesi | Haritada S görünür |
| Sürpriz korunur | İçerideki düşman, item, event gizli |
| Aksiyon temposu | Harita okuma < 15 saniye |
| Solo dev scope | Basit node yapısı, az içerik |
| Build arc | Oda dağılımı aşamaya göre ayarlanmış |

---

## BÖLÜM 4 — YAPI DETAYLARI

### Act Yapısı: 3 Act + Final Boss

```
ACT 1 — "Giriş"     | 10 oda | 2 Elite | 1 Shop | 1 Boss
ACT 2 — "Derinlik"  | 12 oda | 3 Elite | 2 Shop | 1 Boss
ACT 3 — "Son"       | 10 oda | 3 Elite | 1 Shop | 1 Final Boss

Toplam: ~32 oda | Süre: 35-55 dk
Flux: max 1/run, herhangi bir normal odanın yerine (~%5-8)
```

Neden 3 act:
- Her boss sonrası 1 Boss Soul → 3 mutasyon fırsatı = yeterli güç eğrisi
- 4 act daha fazla Grudge döngüsü sağlar ama solo dev için +%30 içerik — FAZ 5'te opsiyon
- Run süresi kontrolü: 35-55 dk hedefi 3 act ile tutturulabilir

### Harita Görünümü

```
ACT 2 haritası örneği:

    [BAŞLANGIÇ]
         │
    ┌────┴────┐
    │         │
  [N]       [E!🔥]     ← E! kırmızı ikon, 🔥 = ateş direncine sahip
    │         │
    └────┬────┘
         │
    ┌────┴────┐
    │         │
  [S]       [N]         ← S mavi ikon = shop odası
    │         │
    └────┬────┘
         │
    ┌────┴────┐
    │         │
  [E!❄]     [✦]        ← ✦ = Flux odası (kırmızı ✦ = Corrupted)
    │         │
    └────┬────┘
         │
       [★ BOSS]
```

### Görünen vs Gizli Bilgiler

| Bilgi | Haritada Görünür mü? | Neden |
|-------|---------------------|-------|
| Oda tipi (N/E/S/B/Flux) | ✅ Evet | Grudge planlaması, shop bütçesi için şart |
| Elite'in Grudge Badge'i | ✅ Evet (ikon) | Programladığını hatırlamak için kritik |
| Elite'in adı / kim olduğu | ❌ Hayır | Giriş onay ekranında açıklanır, sürpriz korunur |
| Normal odanın wave içeriği | ❌ Hayır | Her oda açılırken keşif hissi yaşansın |
| Flux odası | ✅ Evet (✦) | Nadir ve anlamlı — kaçırmak trajedi olur |
| Gizli oda | ❌ Hayır | Koşul bazlı, haritada çıkmamalı |

### Oda Tipleri — Detay

**NORMAL ODA**
- 2-3 wave, her wavede 3-6 düşman
- Act 1 → 2 wave, Act 2 → 2-3 wave, Act 3 → 3 wave
- Temizle → oda sonu ekranı → 3 kart seçimi (skill/pasif/upgrade)
- HP recovery yok — sadece pasifler

**ELİTE ODA**
- Giriş onay ekranı:
  ```
  ┌──────────────────────────────────────────────────┐
  │  MOLTEN GRUNT + 2 Cinder Imp          [NEMESİS] │
  │  [🔥 Ateş Direnci: %35]                          │
  │  Act 1'de buz ile öldürüldü                      │
  │  Karşılaşma 2/3 — Ödül: +20 Soul Dust + 4 kart  │
  │  [GİRİŞ]                                         │
  └──────────────────────────────────────────────────┘
  ```
  Geri dön yok — elite odaya girince çıkamazsın. "Hazırlan, sonra gir."
- Combat: 1 ana elite + 2-3 minyon (minyonlar hafıza taşımaz)
- Öldürünce: yeni hafıza bildirimi → haritada bir sonraki elite ikonu güncellenir

**Ödül tablosu:**
| Elite tipi | Karşılaşma | Ödül |
|------------|------------|------|
| Normal elite | Herhangi | Soul Dust +10 + garantili upgrade şansı + 3 kart seçimi |
| **Nemesis Elite** | 1. (Act 1) | Soul Dust +10 + garantili upgrade + 3 kart seçimi |
| **Nemesis Elite** | 2. (Act 2) | Soul Dust +30 + garantili upgrade + **4** kart seçimi |
| **Nemesis Elite** | 3. (Act 3) | Soul Dust +50 + skill upgrade + 4 kart seçimi + Nemesis Çözüldü rozeti |

**SHOP ODASI**

Item satın alma iki modda çalışır:

```
[KÖR AL]  1x fiyat
  → Kategori seçiyorsun (Augment / Skill / Pasif)
  → Hangisi olduğunu görmeden satın alıyorsun
  → Açılınca ne çıkarsa çıkar

[SEÇ AL]  3x fiyat
  → 3 item gösterilir, istediğini seçiyorsun
  → Tam kontrol, tam bilgi
```

Fiyat örnekleri:
| Item tipi | Kör fiyat | Seçerek fiyat |
|-----------|-----------|---------------|
| Augment | 10 Dust | 30 Dust |
| Skill | 15 Dust | 45 Dust |
| Pasif | 20 Dust | 60 Dust |

Kör alma riski/ödülü: aynı kategoride nadir item çıkma şansı biraz daha yüksek — "kör alanı oyun ödüllendiriyor" hissi.

Servisler (sabit fiyat, seçimsiz):
- Skill Upgrade: 25 Dust
- Skill Silme: 40 Dust
- Reroll Shop (kör stoku yeniler): 15 Dust

Soul Dust bilgisi harita ekranında görünür (bütçe planlaması için).

**FLUX ODASI**
- Haritada ✦ ikonu — görünür, hedeflenebilir
- Normal ✦: seçim ekranı (önce ne al, sonra ne ver)
- Kırmızı ✦ Corrupted: önce mini encounter (daha güçlü düşman), sonra güçlü ama downside'lı seçimler
- Max 1/run

**GİZLİ ODA**
- Haritada görünmez — koşulla tetiklenir:
  - Odayı hiç hasar almadan temizle → gizli geçiş açılır
  - Veya belirli item/augment ile
- İçerik: Soul Dust +20 + augment, lore parçası, veya bir Grudge Badge sıfırlama

**REST ODASI — YOK**
- Bilinçli karar: HP recovery odası yok
- Act geçişinde +%30 max HP recovery var (boss sonrası reward ritüelinin parçası)
- Pasifler/augmentler: Bloodlust kill heal, Holy Endurance vb.

### Act Geçiş Ekranı

```
┌───────────────────────────────────────────────────────┐
│  ACT 1 TAMAMLANDI                                     │
│                                                       │
│  HP: +%30 recovery (Mevcut: 45 → 75)                 │
│  Soul Dust: +25                                       │
│                                                       │
│  BOSS SOUL — Hangi skili dönüştürmek istiyorsun?      │
│  [Ground Stomp ▸] [Fireball ▸] [Hemorrhage ▸]        │
│                                                       │
│  ★ NEMESİS: Molten Grunt [❄] — Karşılaşma 1/3        │
│    Act 2'de tekrar çıkacak (+20 Soul Dust + 4 kart)  │
│                                                       │
│  Diğer programlanan Grudge'lar:                       │
│  • Iron Sentinel [⚡] — Act 2'de görünebilir           │
│                                                       │
│  [ACT 2 HARİTASINI GÖR]                              │
└───────────────────────────────────────────────────────┘
```

---

## BÖLÜM 5 — SOLO DEV UYGULAMA SIRASI

### FAZ 1 — Minimum Viable (Core Loop Test)

```
Lineer 5 oda: Normal → Normal → Elite → Normal → Boss
Seçim yok, harita ekranı yok
Sadece oda tipleri, oda sonu ödülü, elite girişi çalışıyor
Grudge: elite hatırlaması implement edilir ama harita gösterilmez
```

Bu FAZ 1'in amacını karşılıyor: combat + skill acquisition + temel Grudge.

### FAZ 2 — Gerçek Harita

```
Act 1 tam çalışıyor:
  - 1 kavşak (2 yol seçimi)
  - 4 oda tipi: Normal, Elite, Shop, Boss
  - Harita ekranı (basit node visualı)
  - Grudge Badge haritada ikon olarak gösterilir
  - Act geçiş ekranı (HP recovery + Boss Soul seçimi)
```

### FAZ 3 — Tam Sistem

```
3 act tam implement
3 kavşak/act
Flux odası eklenir (normal varyant)
Gizli oda koşul mekaniği
Corrupted Flux (mini encounter + güçlü seçimler)
```

### FAZ 4-5 — Polishing

```
Path profilleri (meta unlock sonrası):
  Kanlı Yol: +1 Elite, -1 Normal (Grudge programcısı için)
  Tüccar Yolu: +1 Shop, -1 Normal (augment odaklı build için)
  Gizem Yolu: +1 gizli oda şansı (keşifçi için)

Harita animasyonları
Act biome görsel farklılıkları
4. Act opsiyonu (scope izin verirse)
```

---

## BÖLÜM 6 — TASARIM PRENSİPLERİ ÖZET

| Prensip | Referans | Uygulama |
|---------|----------|---------|
| Grudge planlanabilir | PoE target farming × D2 bilinen iskelet | Elite + Badge haritada görünür |
| Sürpriz korunur | Hades 1 keşif hissi | İçerik (düşman, event) gizli kalır |
| Harita analizi < 15 sn | RoR2 tempo × Dead Cells akış | Basit ikon sistemi, az seçenek |
| Shop bütçesi planlanabilir | StS full visibility × Hades 2 node map | Shop ikonu haritada |
| Aksiyon temposu bozulmaz | Balatro anti/blind ritmi | Haritaya bak → karar al → gir (hızlı döngü) |
| Pre-encounter bilgi | Nioh görev sistemi | Elite giriş onay ekranı |
| Solo dev scope | Dead Cells biome sınırlı | 3 act, minimal dallanma, kademeli FAZ implementasyon |

---

*Ollama araştırması (7 bölüm): HARITA_OYUN_ANALIZI_OLLAMA.md*
*Sentez: 2026-03-27 — tamamlandı.*
