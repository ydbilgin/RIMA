# SKILL SYSTEM TAXONOMY - 2026-05-06
(Status: LOCKED)

Bu belge RIMA skill sistemi icin kanonik taksonomi ve equip kurallarini tanimlar.

Kapsam:
- Class sayisi: 10
- Class listesi: Warblade, Shadowblade, Elementalist, Ranger, Gunslinger, Ravager, Ronin, Brawler, Summoner, Hexer
- Her class temel kit: LMB, RMB, F, 4 equip slotu
- LMB: resource filler, cooldown yok
- RMB: cooldown'lu supplement
- F: Ultimate
- Ghost Attack: Z/X slotu, baska class'in STRIKE skill'ini equip eder
- Draft sistemi: Hades-style, oda odulu olarak 3 secenek
- Echo Imprint: ayri sistemdir, run basina 4 imprint; bu dokumanin skill upgrade slotlariyla karistirilmaz

---

## 1. Aktif Tipler

Her aktif skill tam olarak asagidaki 4 tipten biri olmak zorundadir.

| Tip | Karar | Oyuncu girdisi | Sure / CD | Tasarim rolu |
|---|---|---|---|---|
| STRIKE | Anlik vurustur. Tek seferlik buyuk etki verir. | Oyuncu hedefler ve zamanlar. | Agir CD: 8-15s. | Burst, commit, hedef secimi. |
| ZONE | Sahaya entity koyar. Tetikleme ani ile odul ani ayni olmak zorunda degildir. | Oyuncu pozisyonlar. | Sure: 2-8s. | Alan kontrolu, setup, dusman yonlendirme. |
| REACTIVE | Kosul tabanli otomatik tetiklenir. | Oyuncu basmaz; kosul equip aninda belirlenir. | Skill bazli. | Savunma, tempo kirma, build otomasyonu. |
| STATE | Gecici mod degistirir. Class'in LMB/RMB/F davranisini degistirebilir. | Oyuncu aktif eder. | Sure: 4-10s. | Kisa sureli oyun tarzi degisimi. |

### 1.1 STRIKE

Karar:
- STRIKE anlik vurus kategorisidir.
- Oyuncu hedefleme ve zamanlama sorumlulugu alir.
- Cooldown agir olmalidir: 8-15s.
- Tek seferliktir; sahada kalici entity birakmaz.
- Etkisi buyuktur; hasar, crowd control, reposition veya execute olabilir.

Ornekler:
- Warblade: zemini parcalayan kesis.
- Gunslinger: headshot.
- Ravager: hookslam.

### 1.2 ZONE

Karar:
- ZONE sahaya entity koyar.
- Entity sureli kalir: 2-8s.
- Tetikleme ani ile odul ani ayridir.
- Dusmanin alana girmesi, alanda kalmasi veya entity ile temas etmesi sonuc uretir.

Ornekler:
- Hexer: curse field.
- Ranger: tripwire.
- Summoner: totem.
- Elementalist: lightning rod.

### 1.3 REACTIVE

Karar:
- REACTIVE oyuncu tarafindan basilan bir aktif degildir.
- Equip edildiginde kosul tanimlanir.
- Kosul gerceklesince otomatik tetiklenir.
- Tetiklendigi anda ses ve efekt zorunludur.

Standart kosul aileleri:

| Kosul ailesi | Tetik mantigi |
|---|---|
| HP threshold | Oyuncu veya hedef belli HP esigine iner. |
| Resource threshold | Resource belirli esige iner veya cikar. |
| Hit count | Oyuncu hit alir veya hedef belirli sayida hit yer. |
| Kill | Kill, execute veya tagged kill gerceklesir. |
| Dodge | Dodge, perfect dodge veya dodge sonrasi pencere tetiklenir. |

Ornekler:
- HP %50 altinda shock nova.
- 5 vurus alinca patlama.
- Perfect dodge sonrasi 2s damage buff.

Kisit: Dodge tetigi REACTIVE'in CD reset'ine sebep olamaz (Karar #58 movement uyumu).

### 1.4 STATE

Karar:
- STATE gecici mod degistirme skill'idir.
- Sure: 4-10s.
- Class'in LMB, RMB veya F calisma seklini degistirebilir.
- F aktifken STATE bloklanir.
- STATE aktifken F kullanimi skill bazli degil, global modal kuralla kontrol edilir.
- Modal cakisma yasaktir.

Ornekler:
- Warblade Berserk: LMB armor break uygular, hasari duser.
- Ronin Iaido: sonraki STRIKE instant kill / execute kosulu kazanir.
- Gunslinger Hot Reload: 5s boyunca heat tukenmez.

### 1.5 Aktif Tip Ekonomisi

Karar:
- 4 equip slotunda tip karisimi serbesttir.
- Oyuncu 4 STRIKE, 4 ZONE, 4 REACTIVE, 4 STATE veya karisik build kurabilir.
- Sistem draft soft-guidance ile yonlendirir; hard quota yoktur.
- Reroll varsa oyuncu istemedigi tipi reddedebilir.

Draft soft-guidance:

| Oda araligi | Agirlik |
|---|---|
| Oda 1-3 | STRIKE agirlikli. |
| Oda 4-8 | Tipler arasi esit agirlik. |
| Oda 9+ | STATE + REACTIVE agirlikli. |

### 1.6 Alt-Tag Kurali

Karar:
- Summoner ve Hexer icin yeni aktif tip acilmaz.
- Ozel class ihtiyaclari alt-tag ile cozulur.
- Alt-tag yeni tip degildir.

Alt-tag'ler:

| Class | Tag | Kural |
|---|---|---|
| Summoner | summon | Summoner STRIKE skill'leri summon tag alabilir. |
| Hexer | accumulation | Hexer STATE skill'leri accumulation tag alabilir. |

Ghost Attack kisiti:
- summon tag'li skill Ghost Attack slotuna koyulamaz.

---

## 2. Pasif Tipler

Pasif sistem 3 tipten olusur: KEYSTONE, MODIFIER, RESONANCE.

| Tip | Slot | Zorunlu mu? | Rolu |
|---|---:|---|---|
| KEYSTONE | 1 | Evet, run basinda zorunlu secim. | Class kitini koklu degistirir. |
| MODIFIER | 2 | Opsiyonel. | Always-on buff veya statik kosullu buff. |
| RESONANCE | 1 | Opsiyonel. | Cross-class tag koprusu ve build motoru. |

### 2.1 KEYSTONE

Karar:
- Run basinda 1 KEYSTONE zorunlu secilir.
- Her class icin 5-6 KEYSTONE bulunur.
- KEYSTONE class'in LMB, RMB veya F davranisini koklu degistirebilir.
- KEYSTONE build'in ana yonunu belirler.

Ornek:
- "Rage 50'de Ultimate acilir ama yari sure."

### 2.2 MODIFIER

Karar:
- MODIFIER always-on pasiftir.
- Kosulsuz veya statik kosullu buff olabilir.
- 2 slot opsiyoneldir.
- Her class icin 8-10 class modifier bulunur.
- Ek olarak 30 generic cross-class modifier bulunur.
- Generic modifier'lar tag eslesmesiyle calisir.

Ornekler:
- "Zehirli hedeflere %20 fazla hasar."
- "Stealth sonrasi ilk vurus +%50 hasar."

### 2.3 RESONANCE

Karar:
- RESONANCE cross-class build'in beynidir.
- 1 slot opsiyoneldir.
- Ghost Attack'in tasidigi family tag'lere tepki verebilir.
- Legendary upgrade'in biraktigi family tag'lere tepki verebilir.
- Buff zinciri, proc zinciri veya tag donusumu kurabilir.
- Identity Passive'i enhance edebilir; Identity Passive'in kendisini degistirmez.

Ornekler:
- "Bleed tag'li dusman olunce pierce buff."
- "Sheathe Window 0.6s -> 0.4s."

---

## 3. Upgrade Sistemi

Karar:
- Her skill maksimum 3 upgrade slotu alir.
- Upgrade draft'tan gelir.
- Upgrade icin resource harcanmaz.
- 3 slot dolduktan sonra yeni upgrade otomatik uygulanmaz.
- 3 slot doluysa REPLACE prompt acilir.
- Oyuncu eski upgrade'i dusurup yenisini almayi secmelidir.
- Echo Imprint sisteminden ayridir.

Echo Imprint ayrimi:

| Sistem | Limit | Scope |
|---|---:|---|
| Skill upgrade | Skill basina 3 upgrade. | Tek skill davranisi ve statlari. |
| Echo Imprint | Run basina 4 imprint. | Ayrica tanimli run-level imprint sistemi. |

### 3.1 Upgrade Sirasi

| Durum | Draft agirligi | Karar |
|---|---|---|
| Slot 1 bos | Common/Rare stat upgrade oncelikli: %70 | Ilk upgrade temel guc artisi verir. |
| Slot 1 dolu, Slot 2 bos | Common/Rare %60, Epic %40 | Ikinci upgrade stat veya ekstra ozellik olabilir. |
| Slot 2 dolu, Slot 3 bos | Epic/Legendary %75 | Ucuncu upgrade ekstra ozellik veya davranis degisikligi odaklidir. |
| 3 slot dolu | REPLACE prompt | Otomatik degisim yoktur; oyuncu secim yapar. |

### 3.2 Stat Upgrade

Stat upgrade Common/Rare agirliklidir.

Ornekler:
- Hasar +%.
- Cooldown -%.
- Alan buyutme.
- Sure uzatma.
- Projectile hizi +%.
- Resource gain +%.

### 3.3 Ekstra Ozellik Upgrade

Ekstra ozellik Epic veya Legendary olabilir.

Ornekler:
- ZONE icindeki dusmanlar yavaslar.
- REACTIVE her tetiklendiginde 1s invincibility verir.
- STRIKE knockback ekler.
- Skill vururken %X ihtimalle fireball firlatir.
- Skill vururken %X ihtimalle balta firlatir.

Modelleme kurali:
- Class ici veya generic ek davranis Epic olabilir.
- Cross-class family tag birakan davranis Legendary olmak zorundadir.
- Base skill'e cross-class proc eklenmez.

---

## 4. Cross-Family Carrier

Karar:
- Base skill'e cross-class proc koymak yasaktir.
- Cross-class family tag tasima sadece Legendary upgrade olarak izinlidir.
- Cross-family carrier class kimligini eritmemelidir.
- Locked Cross-Class Proc System ile cakisma yasaktir.

### 4.1 Izinli Model

Cross-family carrier su sekilde calisir:
- Base skill kendi class kimligini korur.
- Legendary upgrade skill'e baska family tag'i tasima yetenegi verir.
- Bu tasima yalnizca tag pip seviyesindedir.
- Baska class'in sigili, commit-beat gorseli veya ana proc kimligi kopyalanmaz.

### 4.2 Ornek Legendary Upgrade'ler

| Base skill | Legendary upgrade | Etki |
|---|---|---|
| Warblade Iron Charge | Fracture Bleeder | Vurusta %20 Bleed family tag. Ravager sigili yok; sadece tag pip. |
| Ranger Bone Trap | Cursed Snare | Yakalanan hedef Hexer Hexed family tag alir: 1 stack. |
| Brawler Counter Blow | Veil Backlash | Basarili counter sonrasi 1.5s Veil family tag. |

### 4.3 Gorsel Kural

Karar:
- Cross-family upgrade'ler sadece tag pip ekler.
- Class sigili eklemez.
- Class sigili sadece o class'i oynayan oyuncunun commit-beat proc'unda gorunur.
- Legendary carrier efektleri okunur olmalidir ama sahip class'in VFX kimligini bastirmamalidir.

---

## 5. Identity Passive

Karar:
- Her class'in 1 sabit Identity Passive'i vardir.
- Identity Passive upgrade edilemez.
- Identity Passive kosulsuz olarak aktif sistem parcasidir.
- Cross-class build'de ana class'in Identity Passive'i her zaman aktiftir.
- Ghost Attack slotundaki class'in Identity Passive'i aktif olmaz.
- RESONANCE pasifi Identity Passive'i enhance edebilir.
- Enhance harici slottan gelir; Identity Passive'in kendisi degismez.

### 5.1 Identity Passive Tablosu

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Warblade | Iron Verdict | 3. LMB hit | Mevcut commit-beat proc ile ayni. (Istisna: sadece Warblade'de Identity Passive = commit-beat proc'un kendisidir. Diger 9 class'ta Identity Passive ayri bir layer'dir, commit-beat proc'tan bagimsiz calisir.) |
| Elementalist | Element Affinity | RMB cast | %15 ihtimalle mevcut element branded mob'u retetikler. |
| Shadowblade | Scar Memory | Phase Step + STRIKE | Phase Step sonrasi 1.2s icinde STRIKE +%25 hasar. |
| Ranger | Distance Discipline | Range check | 6+ tile mesafede ZONE/STRIKE +%20 hasar. |
| Ravager | Carnage Pulse | HP threshold | HP <%50 iken tum Bleed tick'ler +%30. |
| Ronin | Sheathe Window | Stillness + STRIKE | 0.6s hareketsizlik sonrasi ilk STRIKE Opened state garantili. |
| Gunslinger | Heat Rhythm | Reload | Reload sirasinda %10 Heat venti. |
| Brawler | Crack Cadence | 4. LMB hit | 4. LMB combo sonrasi 0.8s tum hit Cracked +1. |
| Summoner | Soul Bond | Minyon olumu | Minyon oldugunde 1s i-frame penceresi. Abuse cap: ICD 4s (4s icinde sadece bir kez tetiklenir); 0.5s icinde gerceklesen grup olumu tek tetik sayilir; boss arenasinda i-frame 0.4s. |
| Hexer | Stack Pressure | 5. stack | Hedef 5+ stack'te otomatik Spread: 6m. Bir kez tetiklenir; 6-10 arasi tekrar tetiklenmez. Boss kuralı: boss'ta stack max 5 oldugundan Stack Pressure 5. stack'te tetiklenir; Spread radius boss arenasinda 3m'ye duser; CLASS_STATE_CONTRACT boss kuralina tabidir. |

### 5.2 Identity Passive ve Cross-Class Mantik

Karar:
- Class identity'sine uygun pasif proc durumlari Identity Passive ile temsil edilir.
- Bu sistem cross-class build'i guclendirir ama ana class kimligini korur.
- Ghost Attack sadece STRIKE tasir; Identity Passive tasimaz.
- RESONANCE, ana class Identity Passive'ini iyilestirebilir veya ona tag tepkisi ekleyebilir.

Ornek RESONANCE enhance:
- Ronin: "Sheathe Window 0.6s -> 0.4s."
- Ranger: "Distance Discipline tetiklenince 2s projectile speed +%15."
- Warblade: "Iron Verdict tagged hedefe vurursa 1s armor shred ekler."

---

## 6. Equip Kurallari

### 6.1 Temel Kit

| Slot | Kural |
|---|---|
| LMB | Resource filler. Cooldown yok. Class kitinin temel ritmi. |
| RMB | Cooldown'lu supplement. Class kitini tamamlar. |
| F | Ultimate. STATE ile modal cakisma kurallarina tabidir. |
| Equip Slot 1-4 | Aktif skill slotlari. STRIKE, ZONE, REACTIVE, STATE mix serbest. |
| Ghost Attack Z/X | Baska class'in STRIKE skill'ini equip eder. |

### 6.2 Aktif Slot Kurallari

Karar:
- 4 aktif equip slotu oyuncunun build alanidir.
- Tip quota yoktur.
- Draft sistemi tipleri soft-guidance ile dengeler.
- Reroll varsa oyuncu istemedigi tipi reddedebilir.
- REACTIVE skill equip edildiginde kosulu UI'da acik gosterilmelidir.
- STATE ve F ayni anda modal olarak calismaz.

### 6.3 Ghost Attack Kurallari

Karar:
- Ghost Attack yalnizca baska class'in STRIKE skill'ini alir.
- Ghost Attack, skill'in aktif davranisini tasir.
- Ghost Attack, kaynak class'in Identity Passive'ini tasimaz.
- summon tag'li skill Ghost Attack slotuna koyulamaz.
- Cross-family Legendary upgrade varsa Ghost Attack skill uzerindeki tag pip kurallarina uyar.

### 6.4 Passive Slot Kurallari

| Passive tipi | Slot | Equip kurali |
|---|---:|---|
| KEYSTONE | 1 | Run basinda zorunlu secim. |
| MODIFIER | 2 | Opsiyonel. Class veya generic tag eslesmeli olabilir. |
| RESONANCE | 1 | Opsiyonel. Cross-class tag koprusu kurar. |

---

## 7. Locked Rules Uyum

### 7.1 Cross-Class Proc System Uyumu

Karar:
- Base skill'lerde cross-class proc yoktur.
- Cross-class mantik Legendary upgrade veya RESONANCE uzerinden kurulur.
- Cross-family carrier sadece tag pip ekler.
- Class sigili sadece asil class proc'unda gorunur.

### 7.2 Echo Imprint Uyumu

Karar:
- Echo Imprint bu dokumanin parcasi degildir.
- Echo Imprint run basina 4 imprint olarak ayri kalir.
- Skill upgrade sistemi skill basina 3 upgrade olarak ayri kalir.
- Draft'ta skill upgrade cikmasi Echo Imprint hakkini tuketmez.

### 7.3 Summoner / Hexer Tip Uyumu

Karar:
- Class tasariminda aktif tip sayisi 4'te kilitlidir.
- Summoner icin yeni "summon skill type" acilmaz.
- Hexer icin yeni "accumulation skill type" acilmaz.
- Summoner ihtiyaci summon alt-tag'i ile cozulur.
- Hexer ihtiyaci accumulation alt-tag'i ile cozulur.

### 7.4 Kullanici Fikirlerinin Karsiligi

| Fikir | Kanonik karsilik |
|---|---|
| Skill'ler hem stat guclendirme hem ekstra davranis kazanabilsin. | Upgrade sistemi stat upgrade ve ekstra ozellik upgrade olarak ayrildi. |
| Skill vururken %X ihtimalle fireball veya balta firlatabilsin. | Epic ekstra ozellik veya cross-class ise Legendary carrier olarak modellendi. |
| Cross-class mantigi class identity'sine uygun pasif proc'larla guclensin. | Identity Passive sistemi eklendi. |
| Class tasariminda 4 tip artirilabilir. | 4 tip yeterli bulundu; Summoner/Hexer ihtiyaci alt-tag ile cozuldu. |

### 7.5 Final Locked Kurallar

- Aktif skill tipi sayisi: 4.
- Pasif tipi sayisi: 3.
- Her skill maksimum 3 upgrade alir.
- Upgrade draft'tan gelir; resource harcanmaz.
- 3 upgrade doluyken yeni upgrade otomatik uygulanmaz; REPLACE prompt gerekir.
- Cross-class proc base skill'e koyulmaz.
- Cross-family carrier sadece Legendary upgrade olabilir.
- Cross-family carrier class sigili degil, tag pip ekler.
- Her class'in 1 sabit Identity Passive'i vardir.
- Ghost Attack kaynak class Identity Passive'ini tasimaz.
- summon tag'li skill Ghost Attack slotuna koyulamaz.
- Echo Imprint ayri sistemdir.

---

## 8. Class Identity Constraints

Her class icin kanonik OWNS (kimlik mekanikleri) ve AVOIDS (baska class kimligi) listesi.
Bu tablo gelecek skill uretiminde cross-contamination kontrolu icin zorunludur.

| Class | OWNS (zorunlu kimlik tag'leri) | AVOIDS (baska class kimligi) |
|---|---|---|
| Warblade | armor-shred, sundered, broken, iron, verdict | bleed, hook, soul, curse, stealth |
| Shadowblade | veil, scar, phase, shadow, echo | armor-shred, bleed, curse, minion |
| Elementalist | fire, frost, lightning, earth, brand, rotation | bleed, soul, stealth, armor-shred |
| Ranger | distance, trap, mark, precision, tripwire | armor-shred, bleed, curse, soul, veil |
| Gunslinger | heat, reload, burst, vent, bullet | bleed, soul, curse, armor-shred, veil |
| Ravager | bleed, hook, aggression, carnage, rend | armor-shred, soul, curse, veil, brand |
| Ronin | iaido, stillness, opened, sheathe, precision | bleed, hook, curse, soul, brand |
| Brawler | cracked, shattered, brawl, counter, crack | bleed, soul, curse, veil, brand |
| Summoner | soul, summon, sacrifice, minion, bond | bleed, armor-shred, veil, heat, iaido |
| Hexer | hexed, curse, stack, accumulation, spread | bleed, armor-shred, veil, soul-bond, heat |

Not: AVOIDS listesi "bu tag'i hic kullanamazsin" demez; sinerji veya Legendary upgrade baglaminda sinirli kullanim Taxonomy §4 (Cross-Family Carrier) kurallarina tabidir.
