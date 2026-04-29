# MOB TASARIMI — RIMA
*Son güncelleme: 2026-03-29 | Tüm düşman tipleri, PixelLab promptları, Ollama lore*

---

## MOB FELSEFESİ

### "Generic Canavar Yok"

RIMA'da her düşman, The Fracturing'in bir sonucu. Rastgele ejderha, goblin, iskelet yok.
Her mob bir soruyu yanıtlıyor: **"Çatlama sana ne yaptı?"**

Üç kategori:

| Kategori | Tanım | Örnekler |
|----------|-------|----------|
| **Fractured** | Bir zamanlar canlıydı, The Fracturing onu parçaladı ve yanlış birleştirdi | Shard Walker, Void Thrall, Remnant Host |
| **Rift-Born** | Çatlaklar olmadan var olamazdı, rift onları yarattı | Seam Crawler, Hollow Mite, Rift Maw |
| **Emergent** | Hâlâ çıkıyor — tam burada değil, tam öte tarafta da değil | Fracture-Born, The Wound |

### Grudge Sistemiyle Bağlantı

Mob'lar nasıl öldüğünü hatırlıyor (Grudge). Bu MOB TASARIMIYLA şöyle birleşiyor:
- **Shard Walker** yakıldığında → sonraki Shard Walker ateşe direnç kazanır, artık taşları ateşte tuttuşturulmuş görünür
- **Void Thrall** büyüyle öldürülünce → rift çatlağı büyüye kapanır, void enerjisi koyulaşır
- **Class Mimic** → zaten Grudge'ın görsel formu: senin taktiklerini kopyalıyor

---

## GRUNT TİER — 32×32px (Sürü, Hızlı Tasarım)

### 1. SHARD WALKER
**Kategori:** Fractured | **Aktlar:** 1, 2, 3 (her akt renk değişir)

**Lore:**
The Fracturing anında parçalanan bir savaşçının kalıntısı. Bedeni birden fazla boyuta dağıldı,
geri dönerken parçalar yanlış sıraya girdi. Artık ne tam burada ne de öte tarafta.

**Görsel:**
- 8-12 adet kırık taş/kemik parçası, insansı şekilde dizilmiş
- Parçalar arasında boşluklar var — dolgu yok, aralar karanlık
- Aralardan çatlak ışığı sızıyor (Act 1: soğuk mavi, Act 2: mor, Act 3: altın)
- Baş kısmı en büyük parça, kollar iki yana uzanan ince şeritler
- Yürüme animasyonunda parçalar hafif sallanıyor, birbirinden ayrılıp toplanıyor

**Davranış:**
- Orta hız yaklaşım + parça fırlatma (2-3 parçasını fırlatır, sonra toplanır)
- Hasar aldığında birkaç parçası dağılır ama geri gelir
- Öldüğünde parçalar gerçekten dağılır, küçük hasar alanı oluşturur

**PixelLab Prompt:**
```
"A humanoid figure assembled from floating broken stone shards and bone fragments,
 gaps between the pieces showing cold blue light bleeding through,
 no solid body just hovering shards in a vague warrior shape,
 dark fantasy enemy, top-down perspective"
```

**Animasyon (Aseprite):**
```
Idle:   4 frame — parçalar hafif salınıyor (6 FPS)
Walk:   6 frame — salınım artıyor, parçalar ayrılıp kapanıyor (10 FPS)
Attack: 5 frame — bir kol parçası fırlıyor, geri geliyor (12 FPS)
Death:  6 frame — parçalar patlar gibi dağılır, glow söner (8 FPS)
```

---

### 2. SEAM CRAWLER
**Kategori:** Rift-Born | **Aktlar:** 1, 2

**Lore:**
İki boyutun arasındaki boşlukta evrimleşti. Katı dünyayı, rift duvarlarını kullanarak hareket ediyor.
Tam anlamıyla zemin çatlaklarında yaşıyor — onlar onsuz var olamazdı, o da onsuz.

**Görsel:**
- Sadece **üstten görünen kısmı** var: iki uzun pençe + omurga çizgisi
- Gövdesinin %80'i zemin çatlağının altında — görünmüyor
- Çatlağa yapışık siluet, çatlak boyunca kayar
- Karanlık, yassı form — sanki çatlak kendisi canlıymış gibi
- Pençeler çatlak kenarlarına tutunuyor

**Davranış:**
- Zemin çatlaklarında kayar — sadece çatlak üzerinde hareket edebilir
- Oyuncunun üzerine gelince: pençeler yukarı çıkar, 3 vuruş yapar, iner
- Ambush: oyuncu çatlak üzerinde yürüyorsa ani saldırı
- Çatlak yoksa var olamaz — çatlaksız odada spawn olmaz

**PixelLab Prompt:**
```
"Top-down view of a creature living inside a floor crack, only its two long claws
 and spine ridge visible above the crack surface, dark shadowy form pressed flat
 against the fissure, horror fantasy enemy"
```

**Animasyon:**
```
Slide:   4 frame — pençeler çatlak boyunca kayar (10 FPS, loop)
Attack:  6 frame — pençeler yukarı çıkar, vurur, iner (12 FPS)
Death:   4 frame — zemine çekilir, çatlak kapanır (6 FPS)
```

---

### 3. ECHO HOUND
**Kategori:** Fractured | **Aktlar:** 2, 3

**Lore:**
Hafıza parçaları yırtıcı içgüdü kazandığında ortaya çıkan şey. Echo Hound bir varlık değil,
bir anının hayaleti. Var olduğunu sanıyor — ama her hareketi 0.3 saniye gecikmiş.

**Görsel:**
- Yarı transparan köpek benzeri form (tam transparan değil, %70 opacity)
- Her hareketiyle geriden gelen bir "echo" bırakıyor (%30 opacity afterimage)
- Gözler beyaz glow — başka ışık yok
- Vücut ana rengi: koyu indigo/lacivert, kenarlar daha soluk
- Yüzeyi yok — içi dolgudan değil enerji çizgilerinden oluşuyor

**Davranış:**
- Hızlı hareket + "blink" (3 tile ileriye atlar, eski konumda echo kalır)
- Echo da saldırıyor — iki noktadan aynı anda hasar
- Grup halinde (2-3): echo'lar çakışınca alan hasar verir
- Gürültüye gidiyor — player yakında olmasına gerek yok, ses çekiyor

**PixelLab Prompt:**
```
"A ghostly wolf-like creature, semi-transparent indigo form with white glowing eyes,
 motion afterimage trailing behind it, no solid body just energy lines and silhouette,
 predatory low crouching stance, dark fantasy enemy sprite"
```

**Animasyon:**
```
Idle:   4 frame — hafif titreyerek duruyor, echo soluklaşıp beliriyor (6 FPS)
Run:    6 frame — çok hızlı, blink efektiyle (14 FPS)
Attack: 5 frame — blink + saldırı + geri çekilme (12 FPS)
Death:  8 frame — echo önce kayboluyor, sonra ana form (6 FPS)
```

---

### 4. VOID THRALL
**Kategori:** Fractured | **Aktlar:** 1, 2

**Lore:**
Rift, bazı insanların içinden geçti. Dışarısı sağlam kaldı ama içi void doldu.
Void Thrall'lar bunu fark etmiyor. Yürüyorlar, saldırıyorlar — ama göğsündeki yarıktan
karanlık sızıyor. Öldüğünde iki yarı ayrı ayrı savaşmaya devam ediyor.

**Görsel:**
- Normal insansı silüet — zırhlı piyade görünümlü
- Göğüsten bele dikey bir çatlak: içinden void/karanlık enerji sızıyor
- Sızıntı rengi: Act 1 → soğuk mor, Act 2 → derin void siyahı
- Yüz gizli (miğfer veya kapüşon)
- Çatlak nefes alırken genişleyip daralıyor (idle animasyon)

**Davranış:**
- Standart yakın dövüş yaklaşımı (orta hız)
- Hasar alınca çatlak genişliyor (görsel Grudge hafızası)
- **Ölünce:** İKİYE BÖLÜNÜR — iki Half-Thrall, her biri %35 HP, daha hızlı
- Half-Thrall'lar ayrı hareket eder, ayrı öldürülür
- Burst hasarla önlemek isteyebilirsin (ikiye bölünmeden)

**PixelLab Prompt:**
```
"A humanoid warrior in dark armor with a deep glowing crack running vertically
 through the chest and torso, dark void energy seeping from the fissure,
 sinister medieval soldier enemy, top-down perspective"
```

**Animasyon:**
```
Idle:    4 frame — çatlak genişleyip daralıyor (6 FPS)
Walk:    6 frame — normal yürüyüş, çatlak sallanıyor (10 FPS)
Attack:  5 frame — ileri atılma + vuruş (12 FPS)
Split:   8 frame — ÖLÜM animasyonu: ikiye bölünme (6 FPS) ← özel
```

---

### 5. HOLLOW MITE
**Kategori:** Rift-Born | **Aktlar:** 1 (sürü), 2 (düz)

**Lore:**
Çatlak parçacıkları organik form kazandığında ortaya çıkıyor. Mite (akar) değil —
sadece boyutu ve hareketi akar gibi. İçi tamamen boş. Küçük ama ölümcül sürülerde geliyor.

**Görsel:**
- 32×32'de çok küçük görünüyor (sprite alanının %30'u) — böyle olmalı
- Altıgen veya kırkayak benzeri form, 6-8 bacak
- Dış kabuk: koyu kahve-siyah
- İçi: tamamen görünür şeffaf (hollow) — ortası boş
- Boş içten hafif ışık sızıyor (çatlak teması)
- Sürü halinde 4-8 birden spawn olur

**Davranış:**
- Çok hızlı, zigzag hareket
- Hasar: küçük ama birikiyor (sürü taktik)
- Öldüğünde: küçük patlama (1-2 tile), sürüyü dağıtır
- Elite/Boss ile birlikte spawn olduğunda dikkat dağıtıcı

**PixelLab Prompt:**
```
"A small hollow insect-like creature with a transparent body showing empty interior,
 dark exoskeleton with six legs, tiny glowing core visible inside the hollow shell,
 swarm enemy, top-down view, dark fantasy"
```

---

## ELİTE TİER — 64×64px (Tekil, Güçlü)

### 6. THE TWICE-BORN
**Kategori:** Fractured | **Aktlar:** 1, 2

**Lore:**
Öldü. Rift'e düştü. Geri döndü — ama iki bedenle. Tek ruh, iki form.
İkisi de hangisinin gerçek olduğunu bilmiyor. Savaşırlarken bile birbiriyle iletişim kuruyorlar.

**Görsel:**
- İKİ adet 32×32 sprite gibi, ama bağlantılı (64×64 alanında yan yana)
- Aralarında ince altın/glow bir ip/ışık bağı
- Biri: kalkan tutan, savunma duruşu
- Diğeri: kılıç tutan, saldırı duruşu
- Giysiler özdeş ama her biri biraz farklı hasarlı
- İp bağlandıkları nokta kalplerinde — göğüste

**Davranış:**
- Biri saldırırken diğeri savunuyor (koordineli)
- İp: birine verilen hasarın %50'si diğerine gidiyor
- Birini öldürünce ip kopuyor → diğeri berserk (2x hasar, 1.5x hız)
- **Strateji:** İkisini aynı anda patlat (AOE) veya sırayla odaklan ve berserki yönet

**PixelLab Prompt:**
```
"Two identical dark-robed warriors connected by a glowing golden thread at their
 chests, one holding a shield in defensive stance and one with a sword raised,
 both have matching but differently damaged armor, tethered duo elite enemy,
 top-down dark fantasy"
```

**Animasyon:**
```
İkisi ayrı animasyon sprite'ı — ama Unity'de tek bir "enemy" olarak işleniyor.
Savaş animasyonları koordineli: A saldırırken B savunma pozisyonu alıyor.
```

---

### 7. FRACTURE-BORN ⭐
**Kategori:** Emergent | **Aktlar:** 2, 3

**"Mob'dan çıkan şey" konseptinin ana uygulaması.**

**Lore:**
Bu taraf için yaratılmadı. Öte taraftan sürünüyor. Çatlak onun doğum kanalı.
Her spawn bir ölüm mücadelesi — eğer çatlakta durdurulursa asla tam anlamıyla var olamaz.

**Görsel (Spawn Animasyonu — 4 Aşama):**
```
AŞAMA 0 (Uyarı, 1.5s):
  Zemininde ince bir çatlak beliriyor (1px, soğuk beyaz-mavi)
  Çatlak yavaşça genişliyor (1px → 4px)
  Altından koyu enerji/duman yükseliyor

AŞAMA 1 (Eller Çıkıyor, 1s):
  İki uzun pençe çatlaktan uzanıyor
  Pençeler kenar tuttu, çekiniyor
  Hâlâ öldürülebilir — hasar alınca geri çekiliyor

AŞAMA 2 (Baş ve Omuzlar, 1s):
  Baş ve omuzlar görünüyor
  Form: ince, uzun, böcek-benzeri yüz, beyaz gözler
  Çatlak genişliyor, enerji daha belirgin

AŞAMA 3 (Tam Çıkış):
  Tüm vücut çekiliyor
  Çatlak kapanıyor
  Artık normal elite enemy gibi savaşıyor
```

**Görsel (Tam Form):**
- Uzun, ince, tırmanmaya adapte edilmiş form
- Kollar vücuttan uzun (çatlaktan tutunmak için evrimleşmiş)
- Omurga üstünde çatlak izi — nereden çıktığının izi
- Renk: neredeyse siyah, sadece eklem yerlerinde void glow

**Davranış:**
- Spawn esnasında Aşama 1-2'de vurulursa: instant kill + bonus drop
- Tam çıkınca: hızlı yakın dövüş, saldırılarında kısa çatlak açıyor
- Ölünce: gövdesi çatlayarak aşağı çekiliyor (ters spawn)

**PixelLab Prompts (ayrı ayrı):**

```
SPAWN AŞAMA 1 (eller):
"Two long dark clawed hands reaching up from a glowing floor crack,
 fingers gripping the crack edges, void energy rising from below,
 horror ambush, dark fantasy top-down view"

TAM FORM:
"A tall thin creature that just crawled from a dimensional rift,
 disproportionately long arms evolved for climbing through cracks,
 near-black body with glowing seams at joints, residual crack scar on spine,
 unsettling predator, dark fantasy elite enemy top-down"
```

**Animasyon (64×64px):**
```
Spawn:  18 frame — tüm 4 aşama (6 FPS, en kritik animasyon)
Idle:   6 frame — omurga çatlağı nefes alıyor (4 FPS)
Walk:   8 frame — uzun adımlar, kollar sürükleniyor (8 FPS)
Attack: 8 frame — kolları uzatarak çalıyor (10 FPS)
Death:  10 frame — ters spawn, zemine geri çekiliyor (6 FPS)
```

---

### 8. SPORE HOLLOW
**Kategori:** Emergent (Kolonize) | **Aktlar:** 2

**Lore:**
Boş bir insan kabuğu. Ruhunu rift aldı — içi tamamen boş kaldı.
Rift sporları bu boşluğu kolonize etti. Şimdi içinde ne olduğunu bilmiyor.
Sporlar onun çatlaklarından dünyaya çıkıyor — o sadece taşıyıcı.

**Görsel:**
- Standart insansı siluet — soluk, gri-bej
- Vücudun çeşitli yerlerinde çatlaklar var
- **Bu çatlaklardan mantar ve rift sporları büyüyor** — turuncu-kahve rengi
- Sporlar vücuttan dışarı fışkırıyor, özellikle omuzlar, sırt
- Yüz: boş, gözler yok — sadece düz yüz yüzeyi
- Yürüme animasyonu: ağır, yolunu bilmiyor gibi sendeliyor

**Davranış:**
- Yavaş yaklaşım
- Her adımda zemine spor bulut bırakıyor (yavaşlatıcı alan)
- Hasar alınca sporları fırlatıyor (projectile)
- **Ölüm:** büyük spor patlaması (5 tile radius, kısa süre poison/slow)
- Strateji: uzaktan öldür, ölüm patlamasından kaç

**PixelLab Prompt:**
```
"A hollow human shell with a blank featureless face and empty eyes,
 orange-brown mushroom-like growths and spores bursting from cracks
 across the body especially shoulders and back,
 shambling slow enemy, body colonized by fungal organisms,
 dark fantasy top-down 64x64"
```

---

### 9. RIFT MAW
**Kategori:** Rift-Born | **Aktlar:** 3

**Lore:**
Rift ağzı bir yer değil, açlık. Duvardan zemine yerleşti ve büyüdü.
Hareket etmiyor — etmesi gerekmiyor. Her şeyi kendine çekiyor.

**Görsel:**
- Yuvarlak/oval bir rift açıklığı, duvara veya zemine gömülü
- Kenarlar: dişli, dünyadan koparılmış çatlak parçaları (teeth-like)
- İçi: tam siyah (void) + çok uzaktan altın/mavi glow
- Kenarlar yaşıyor gibi titriyor — nefes alıyor
- Çekme efekti: etrafındaki küçük partiküller sürekli içine doğru akıyor

**Davranış:**
- Hareket etmez — sabit
- Çekim alanı: 6 tile radius, oyuncuyu içine doğru çeker (sürekli)
- Spawn: küçük düşmanlar üretiyor (2-3 Hollow Mite / 8s)
- Hasar: içine giren her şeye her saniye hasar
- Öldürmek: öncelikli hedef — çekim ve spawn durduğunda çok daha kolay

**PixelLab Prompt:**
```
"A large circular rift opening embedded in the wall, jagged reality-torn edges
 like teeth surrounding a pure void interior, faint golden glow deep inside,
 particles visibly being pulled inward, pulsing alive edges,
 stationary hazard enemy, top-down dark fantasy"
```

---

## ÖZEL MOB'LAR — Lore-Critical

### 10. CLASS MIMIC
**Kategori:** Fractured | **Aktlar:** 2, 3

**Lore:**
Bir savaşçı rift'e düştü. Geri dönemedi — ama dövüş hafızası döndü.
Hafıza şu an önündeki savaşçıyı görüyor ve şekilleniyor. Sen ne isen o da o.

**Görsel:**
- Oyuncunun primary class siluetinin %70 transparan, distorted kopyası
- Renk: ana rengin desaturated + hafif void moru tint
- Hareketleri gecikmiş (0.1s) ve hafif bozuk
- Göz bölgesinde void siyahı — gözleri yok
- Kullandığı skill'ler: oyuncunun skill'lerinin soluk kopyası

**Davranış:**
- Oyuncunun primary class skilllerinin basit versiyonlarını kullanıyor
- Warblade oynuyorsan Mimic de kılıç kullanıyor
- Grudge ile entegre: öldürüldüğü methoda göre bir sonraki Mimic farklı class alıyor
- Her odada farklı class olabilir

**PixelLab Prompt (her class için ayrı, örnek Warblade için):**
```
"A distorted translucent mirror copy of a dark armored warrior,
 desaturated void-tinted silhouette with no eyes, only void where face should be,
 movements slightly delayed and corrupted, dark spirit enemy,
 top-down dark fantasy elite"
```

---

### 11. REMNANT HOST
**Kategori:** Fractured | **Aktlar:** 3

**Lore:**
Rift, bir bedene üç farklı ruhun kalıntısını doldu.
Her ruh farklı bir savaşçıdan — hiçbiri bu bedene ait değil.
Sürekli kimin kontrol edeceği üzerine iç savaş yaşanıyor.

**Görsel:**
- Insansı form ama sürekli titreyerek değişiyor
- Her ruh kontrolde iken bedene hafif renk tint'i geliyor:
  - Ruh 1 (Savaşçı): Kırmızı tint, kalın siluet
  - Ruh 2 (Büyücü): Mavi tint, rünler beliriyor
  - Ruh 3 (Avcı): Yeşil tint, yay materyalizuyor
- Geçiş anlarında form "glitch" yapıyor — pixeller dağılıp toplanıyor

**Davranış:**
- Her 15 saniyede ruh değişiyor (görsel geçiş: 1s)
- Ruh 1: yakın dövüş, fizik hasar, fizik direnci
- Ruh 2: büyü, AOE, büyü direnci
- Ruh 3: uzun menzil, zehir, zehir direnci
- Renk = direnç tipi ipucu

**PixelLab Prompt:**
```
"A humanoid figure glitching between three different warrior forms,
 currently showing a red-tinted heavy fighter with pixels dissolving at edges,
 three souls fighting for control, unstable dark entity,
 top-down dark fantasy elite enemy"
```

---

### 12. THE WOUND
**Kategori:** Rift-Born | **Aktlar:** 1, 2, 3 (özel oda)

**Lore:**
Yara bir yer değil. Yara bir şey. Ve bu yara bilinçlendi.
Orada duruyor çünkü orada olmak onun varoluş biçimi.
Yakınındaki her şeyi iyileştiriyor — ama bu şefkatten değil,
yara büyüsün diye.

**Görsel:**
- Havada asılı, oval düzensiz yara benzeri rift (duvar bağımsız)
- Kenarlar: kanlı kırmızı-mor, organik görünüm
- İçi: void siyahı, ama kenar çevresinde kırmızı glow
- Hafif yukarı-aşağı sallanma (yüzüyor)
- Etrafında kırmızı partiküller dönerek yakındaki düşmanlara gidiyor (görsel iyileştirme efekti)

**Davranış:**
- Pasif: yakınındaki tüm düşmanlara 2 HP/s yeniliyor
- Saldırı: periyodik olarak gerçeklik kırıkları (shard) fırlatıyor
- Öldürülünce: **beyaz flash** — iyileştirme kesilir, tüm düşmanlar %20 HP hasar alır
- Önce bunu öldür — odaya girince hemen fark etmelisin

**PixelLab Prompt:**
```
"A floating oval wound in reality, hovering in mid-air,
 organic ragged crimson-purple edges pulsing with life, void interior,
 red particle tendrils extending to nearby enemies to heal them,
 conscious injury made manifest, dark fantasy unique enemy"
```

---

## AKT DAĞILIMI

| Mob | Act 1 | Act 2 | Act 3 | Notlar |
|-----|-------|-------|-------|--------|
| Shard Walker | ✅ (mavi) | ✅ (mor) | ✅ (altın) | Renk değişiyor |
| Seam Crawler | ✅ | ✅ | ❌ | Act 3 zeminleri yok |
| Echo Hound | ❌ | ✅ | ✅ | Void daha derin |
| Void Thrall | ✅ | ✅ | ❌ | Çok "insan" Act 3 için |
| Hollow Mite | ✅ (sürü) | ✅ | ❌ | Sürü hissi Act 1-2 |
| The Twice-Born | ✅ (nadir) | ✅ | ❌ | Elite, Act 1 zor |
| Fracture-Born | ❌ | ✅ | ✅ | Spawn mekaniği orta+ |
| Spore Hollow | ❌ | ✅ | ❌ | Act 2'ye özgü tema |
| Rift Maw | ❌ | ❌ | ✅ | Sadece Act 3 |
| Class Mimic | ❌ | ✅ | ✅ | Player build okur |
| Remnant Host | ❌ | ❌ | ✅ | 3 ruh = late game |
| The Wound | ✅ (nadir) | ✅ | ✅ | Özel oda |

---

## ANİMASYON ÜRETIM SIRASI (Solo Dev Öncelik)

```
FAZ 1 — Sadece bunlar:
  ✅ Shard Walker (grunt, her yerde lazım)
  ✅ Void Thrall (grunt, ikonik split mekanik)
  ✅ The Twice-Born (elite, Act 1 boss öncesi)

FAZ 2 — Demo'ya girecekler:
  ✅ Seam Crawler
  ✅ Echo Hound
  ✅ Fracture-Born (spawn animasyonu kritik — önce bu)
  ✅ The Wound (special oda)

FAZ 3 — Full game:
  ✅ Hollow Mite
  ✅ Spore Hollow
  ✅ Rift Maw
  ✅ Class Mimic
  ✅ Remnant Host
```

---

## OLLAMA PROMPTLARI

### Sistem Prompt (Her Mob için Lore Üretimi)

```python
RIMA_LORE_SYSTEM = """Sen RIMA oyununun lore yazarısın.
RIMA: 2D roguelite, dünya "The Fracturing" ile kırıldı, her şeyde çatlaklar var.
Oyuncu her run'da kırığın içinden geçiyor, hem dünyayı hem kendini yeniden kuruyor.
Düşmanlar Generic değil — her biri kırılmanın bir sonucu.

Codex girişleri için format:
- 2-3 cümle, birinci veya üçüncü kişi gözlemci
- Melankolik, kısa, şiirsel ama net
- Oyunun öğretici değil — gözlemler, sorular, bulgular
- TR dilinde yaz"""

def get_mob_codex(mob_name, mob_description):
    return f"""Mob: {mob_name}
Açıklama: {mob_description}

Bu mob için:
1. Oyun içi codex girişi (2-3 cümle)
2. Gözlemci notu (1 cümle — başka bir savaşçının not defterinden)
3. Başlık önerisi (3-4 kelime, şiirsel)"""
```

### Boss Monolog Üretici

```python
BOSS_MONOLOG_SYSTEM = """RIMA oyununun boss savaşlarında metin çıkıyor.
Her boss kırılmanın farklı bir yönünü temsil ediyor.
Monologlar:
- 1-2 cümle (daha uzun olursa kimse okumaz)
- Boss'un neyi temsil ettiğini hissettir
- Tehditkâr ama filozofik olabilir
- TR yaz"""

# Örnek kullanım:
bosses = {
    "Iron Warden": "Düzenin savunucusuydu, şimdi herkesi aynı kalıba sokmaya çalışıyor",
    "Fractured King": "Hem kazanan hem kaybeden — kendini kaybetmeden kazanamadı",
    "Hollow Sovereign": "Build tag'lerini okuyor, zayıflığını biliyor, bunu silah olarak kullanıyor",
    "Nexus Core": "Primary class'ı taklit ediyor — seni kendinle öldürmeye çalışıyor"
}
```

### Ölüm Ekranı Metni Üretici

```python
DEATH_SCREEN_SYSTEM = """RIMA oyununda her ölümde kısa bir metin çıkıyor.
Roguelite — ölüm döngüsünün parçası, üzücü değil anlamlı.
Metinler:
- 1 cümle, maksimum
- Kırık dünya ve kırık kimlik teması
- İkinci kişi ("sen") kullanabilir
- Ne kadar ilerlediğini yansıtmadan genel olsun
- 20 farklı metin üret"""

death_prompt = "RIMA oyunu için 20 ölüm ekranı metni yaz. Her biri farklı açıdan 'çatlak içinde kaybolmak' temasını işlesin."
```

### Ruh Karşılaşması Diyaloğu

```python
SPIRIT_DIALOGUE_SYSTEM = """RIMA'da spirit encounter'larda ruhlar konuşuyor.
Ruhlar: eski savaş ekollerinin yankıları, düşmüş şampiyonlar, kırık bölgelerde takılı kalmış varlıklar.
Onlar sana güç veriyor — ama yardımseverlikten değil, kendi nedenleriyle.
Her ruhun farklı kişiliği var. Diyaloglar:
- 2-3 satır konuşma
- Ne verdiklerini söylesin (skill/buff)
- Neden verdiklerini ima etsin — tam söylemesin
- Gizemli ama net"""

spirits = [
    "Forge Wraith — hayalet demirci, silah güçlendirir",
    "Shadow Hound — saldırı hızı verir, sizi ölüme koşturur",
    "Blood Oracle — HP karşılığı güç verir"
]
```

### Oda Açıklama Üretici (UI alt yazı için)

```python
ROOM_DESC_SYSTEM = """RIMA'da her oda tipinde alt köşede kısa bir metin beliriyor.
Oda tipleri: combat, elite, shop, shrine, spirit, rest, mystery.
Metinler:
- Max 5 kelime
- Atmosfer versin, bilgi değil
- TR"""

room_types = {
    "combat": "Kırık yol, hayatta kalan yok",
    "elite": "Burası daha önce kapanmış",
    "shop": "Satılık ne kaldı ki",
    "shrine": "Bir şey istiyor burada",
    "spirit": "Yankı mı, ses mi",
    "rest": "Dinlenme değil, erteleme",
    "mystery": "Çatlak burada farklı"
}
# Bunları Ollama ile genişlet: her oda tipi için 10'ar varyasyon
```

---

## SANAT ÜRETIM SIRASI (PixelLab Workflow)

```
Her mob için standart workflow:

1. Ollama → lore text üret (kodex, notlar)
2. PixelLab Generate (Bitforge, 32×32 veya 64×64):
   - Prompt: yukarıdaki mob promptları
   - View: low top-down
   - Direction: S
   - Outline: single color black
   - Shading: medium
3. Aseprite → import, palette'e uyarla, düzelt
4. PixelLab Animate (text standard, 4 frame):
   - idle / walk / attack
5. Aseprite → animasyonları birleştir, tag'le
6. Export → sprite sheet

Önce grunt'ları bitir, sonra elite'lere geç.
Boss'lar her zaman skeleton animate kullan.
```
