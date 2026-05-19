---
status: LOCKED
faz: 1
tarih: 2026-05-13
ozet: "Ambient Idle animasyon sistemi — 10 karakter kişilik bekleme hareketi"
---

# RIMA — Ambient Idle Animasyon Sistemi
*S65 | 2026-05-13*

> **KURAL:** Her animasyon üretiminde bu belgeye bakılır. Ambient idle prompt yazılmadan önce karakter kişiliği + spesifik hareket + teknik kısıtlar kontrol edilir.

---

## Sistem Tanımı

**Base Idle:** 2-3 frame, hafif nefes/ağırlık kayması döngüsü. PixelLab built-in Idle preset (2 seçenek) veya Custom V3.

**Ambient Idle:** X saniye hareketsiz kaldıktan sonra tetiklenen karakter-özgü kişilik animasyonu. Oyunun kalbidir — oyuncu karakterin "yaşadığını" bu anda hisseder. Her karakterin kendi stili ve hikâyesi var.

**Unity Animator:** `Idle (loop) → [timer X sn] → Ambient Idle (1x play) → Idle (loop)`

---

## Teknik Kısıtlar

| Parametre | Değer |
|-----------|-------|
| Tool | Custom Animation V3 |
| Frame range | 4–16 frames |
| Keep First Frame | AÇIK — idle poza sorunsuz dönüş |
| Maliyet | 3 gen/yön × 4 yön = 12 gen/anim |
| 10 karakter toplam | ~120 gen |
| Base sprite | Mevcut anchor sprite (Create State gerekmez) |

**Create State ne zaman gerekir?**
Ambient idle, mevcut base sprite üzerinde animasyon — Create State GEREKMEZ. Silah kaldırma/koyma gibi kalıcı görsel değişim istenirse (ör. silah omuzda yeni bir idle pozu) Create State ayrıca üretilir ve yeni bir animasyon ağacı kurulur.

---

## 10 Karakter — Ambient Idle Tasarımları

### 01 — Warblade
**Kişilik:** Taş gibi. Sakin. Tehlikeli. Beklemeyi bilen.
**Hareket:** *Omuz Yaslaması* — greatsword alçak garddan kaldırılarak sağ omuza yaslanır (ağzı geriye), flat of blade omuzda, sol el kemer tokasına takılır, baş hafifçe hafifçe sola döner (tarama), resettles.
**Neden:** "Ben beklemeye alışığım" — ağırlık ve sabır.
**Frames:** 8
**Prompt ipucu:** Greatsword raised from low guard, flat of blade rests on right shoulder, left hand hooks loosely at the belt, head turns slightly left scanning, weight settles back on heels.

---

### 02 — Elementalist
**Kişilik:** Meraklı. Ritmik. Dur durak bilmez zihin.
**Hareket:** *Element Oyunu* — rune disc avuçtan yükselir ve yavaş sekiz şeklinde yörünge çizer, serbest el havada küçük iz deseni çizer (ember trail bırakır), disc avuca döner.
**Neden:** Kendini tutamaz — büyüyü bile beklerken pratik yapar.
**Frames:** 10
**Prompt ipucu:** Rune disc rises from right palm and traces a slow figure-eight orbit, left hand traces a small pattern in the air leaving faint ember trails, disc drifts back to rest in palm.

---

### 03 — Shadowblade
**Kişilik:** Paranoyak. Hiper-farkında. Av gibi bekler.
**Hareket:** *Dagger Flip* — sağ hançer reverse grip'ten parmak rulosuyla ileri flip yapar (normal grip), sonra geri reverse grip'e döner. Sol hançer kıpırdamaz. Baş minimal sağa bakar.
**Neden:** Adrenalin düşmez, enerji bir yere çıkmalı — ve bu bile bir beceridir.
**Frames:** 6
**Prompt ipucu:** Right dagger rolls forward off the fingers in a single controlled flip from reverse grip to forward grip and back, left dagger stays tight at the ribs, head glances right briefly.

---

### 04 — Ranger
**Kişilik:** Bağımsız. Doğayla iç içe. Titiz, her detay önemli.
**Hareket:** *Ok İncelemesi* — sağ el quiver'dan bir ok çeker, yukarı kaldırır ve ucu inceler (head eğilir), ok quiver'a döner. Kısa ve kesin.
**Neden:** Silahını savaşa hazır tutar — bu bekleyiş değil, hazırlık.
**Frames:** 8
**Prompt ipucu:** Right hand draws one arrow from the quiver, raises it briefly to eye level examining the tip, quiver bounces slightly on the back, arrow returned precisely.

---

### 05 — Ravager
**Kişilik:** Sabırsız. Saldırgan. Tutulmuş enerji.
**Hareket:** *Balta Çarpışması* — her iki kısa kompakt balta bel hizasında öne doğru tek sert çarpışma, küçük kıvılcım, omuzlar geri yuvarlanır, wide aggressive stance'e resettles.
**Neden:** "Neden hâlâ buradayız? SAVAŞALIM." — beklemeyi sevmez.
**Frames:** 5
**Prompt ipucu:** Both short compact axes brought together sharply at waist level for a single impatient clash, small spark on contact, shoulders roll back, wide aggressive stance resettles.

---

### 06 — Ronin
**Kişilik:** Mutlak sessizlik. Tek hareket — kesin. BDO Musa esinli.
**Hareket:** *Seme Baskısı* — 2-3 frame tam durma (olağandışı), sol baş parmak tsuba'yı (guard) birkaç pixel yukarı iter (kısmi çekiş, kın'ın dudağından ayrılma), bırakır, sessizce resettles. Ses çıkarılmaz ama hissedilir.
**Neden:** İaido'nun özü — çekiş öncesi zihinsel baskı. Düşmanı çöktüren hareket değil, hareketsizliktir.
**Frames:** 6
**Prompt ipucu:** Complete stillness for two frames, left thumb presses the tsuba up slightly in a partial draw, blade separates a few pixels from the saya mouth, releases, hand resettles on the saya.

---

### 07 — Gunslinger
**Kişilik:** Kinetik. Şovmen. Güveni kayalık.
**Hareket:** *Tabanca Çevirimi* — sağ tabanca bel kılıfından tek el fırlamasıyla havaya kısa toss, tek barrel rotasyonu, temiz yakalanış, kılıfa döner. Trençkot sallana. Kafa hafif tilt.
**Neden:** "Bak ne kadar iyi." — performans için yaşar.
**Frames:** 7
**Prompt ipucu:** Right pistol flips up from the hip in a short toss, rotates one barrel turn in the air, caught cleanly, returned to hip. Trench coat sways. Head tilts slightly.

---

### 08 — Brawler
**Kişilik:** Zıpır. Ritmik. Her zaman ısınıyor.
**Hareket:** *Hava Kombinasyonu* — hızlı sol-sağ iki hava crosı (hedefe değil, boşa), her gauntlette küçük arcane kıvılcım, vücut ritimle sekiyor, guard'a geri dönüş.
**Neden:** Ritimden çıkarsa kaybolur — hareket onun meditasyonu.
**Frames:** 5
**Prompt ipucu:** Quick left-right air jab pair into empty space (warming up, not attacking), small arcane flash on each gauntlet knuckle, body bounces with the rhythm, settles back to chin guard.

---

### 09 — Summoner
**Kişilik:** Tefekküri. Karanlık bağ. Ölülere fısıldar.
**Hareket:** *Ruh Fısıltısı* — sol el feneri hafifçe kaldırır ve öne eğilir, küçük bir wisp alevden çıkar ve çevresinde kısa bir daire çizer, başı hafifçe eğer (fısıltı), wisp aleve döner.
**Neden:** Ordusu her zaman yakınında — onlarla konuşur, komuta eder, dinler.
**Frames:** 9
**Prompt ipucu:** Left hand raises the soul lantern slightly and leans in close, a small cyan wisp emerges from the flame and orbits once, head dips slightly as if whispering, wisp returns to the flame.

---

### 10 — Hexer
**Kişilik:** Eksantrik. Mırıldanır. Lanetlerle dolu zihin.
**Hareket:** *Grimoire Çevirme* — sol el grimoire'ı belden çekip tek elle açar, sayfalar hızla yeşil enerji parıltısıyla geçer, çarpar kapanır, bele geri takılır.
**Neden:** Her an yeni bir lanet formülü düşünüyor — bekleme süresi bile verimli.
**Frames:** 8
**Prompt ipucu:** Left hand draws the grimoire from the belt and cracks it open, pages flutter rapidly with a green energy glow on the spread, snaps shut, tucked back at the belt. Hood rocks slightly.

---

## Create State Senaryoları (Ne Zaman Lazım)

Ambient idle tek başına animasyon — Create State gerekmez. Şu durumlarda Create State açılır:

| Senaryo | Create State Açıklaması | Kimin İçin |
|---------|------------------------|------------|
| Combat Idle | Silah kaldırılmış, diz bükümlü muharebe duruşu | Tüm 10 karakter (Karar bekliyor) |
| Death State | Yere serilmiş, mağlup | Tüm 10 karakter |
| Ronin Iaido Stance | Katana çekilmiş, iki el hazır, tam hareketsiz | Ronin |
| Hexer Lich Form | Yarı saydam ghostal form | Hexer |
| Ravager Berserk Mode | Kan sıçramış, çılgın öne eğik duruş | Ravager |
| Brawler Overdrive | Arcane enerji saçan yumruklar, gerilmiş gövde | Brawler |
| Frozen (status) | Buz kristali içinde donmuş karakter | Mob/Karakter status |

---

## Üretim Önceliği

| Öncelik | Animasyon | Ne Zaman |
|---------|-----------|----------|
| P0 | Base Idle (2-3f nefes loop) | Anchor üretilince hemen |
| P1 | Ambient Idle (kişilik anim) | Base Idle PASS sonrası |
| P2 | Combat Idle State | Faz 1 combat loop otururken |
| P3 | Death State | Combat animasyonlar PASS sonrası |
| P4 | V Burst/Skill States | Faz 2 |

---

## PixelLab Prompt Şablonu (Ambient Idle)

```
[Karakter_adı] ambient idle, [silah/ekipman referansı], [hareket tanımı başlangıç],
[hareket zirvesi — spesifik ekipman/vücut hareketi], [dönüş/resettles],
chibi 64x64 high top-down ~30-35°, [N] frames.
```

---

*Bu belge kapalıdır. Revizyon → MASTER_KARAR_BELGESI.md'ye yeni karar.*
