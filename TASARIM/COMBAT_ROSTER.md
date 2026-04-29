# COMBAT_ROSTER.md — RIMA Düşman Tasarımı

> **[S42 SYNC — FAZ 1 GÜNCEL]** Faz 1 mob listesi S42 versiyonuyla senkronize edildi. Aktif enemy source: `_STAGING/MOB_BOSS_REDESIGN_S42.md`. Act 2-3 bölümleri henüz S42 sync bekliyor.

> Act bazlı, build-test odaklı düşman roster'ı.
> Mevcut MOB_TASARIMI.md ile çakışmaz — bu combat karar belgesi, o lore+prompt belgesi.

---

# FAZ 1 — S42 ONAYLI MOB ROSTER (8 MOB)

> Kaynak: `_STAGING/MOB_BOSS_REDESIGN_S42.md`. Aşağıdaki listeyi kullan; Act 1 bölümündeki eski tanımlar S42 öncesi versiyondur.

**Tasarım felsefesi:** Her mob bir oyuncu alışkanlığını kırar ve bunu spesifik bir skill ile yapar. Auto-attack ≤%25 DPS. Tell max 1.5s, window min 0.4s.

**Spawn bütçesi (oda başı 8-12 threat point):**
| Mob | HP | Threat Point | Max |
|---|---|---|---|
| Fracture Imp | 25 | 1pt | max 4 |
| Shard Walker | 75 | 3pt | max 2 |
| Seam Crawler | 60 | 3pt | max 2 |
| Penitent Bruiser | 180 | 4pt | max 1 |
| Chain Warden Echo | 140 | 4pt | max 1 |
| Relic Caster | 50 | 4pt | max 1 |
| Riftbound Augur | 70 | 3pt | max 1 |
| Hollow Hulk | 350 | 8pt | max 1 (Elite/Unknown only) |

**Anti-pattern (asla aynı odada):**
- Bruiser + Warden + Hulk → 3 ağır mob, tempo ölür
- 2× Caster → çift Aegis Mark loop
- Augur + Hulk → Time Shudder içinde Quake dodge imkansız
- **Seam Crawler + Chain Warden erken odalarda birlikte spawn YASAK** (combo çok zorlu)
- 4× Imp + Crawler + Walker → görsel kalabalık, target priority confusion

**Önerilen encounter'lar:**
- "Triple Threat" (8pt): Walker + Imp×3 + Crawler
- "Lockdown" (10pt): Warden + Crawler + Imp×3
- "Execution Test" (9pt): Caster + Warden + Imp×2
- "Aura Trap" (10pt): Bruiser + Augur + Walker
- "Mini-Boss Solo" (8pt, Elite oda): Hollow Hulk solo

---

## M01 — Fracture Imp
- **Role:** Trash / Swarm
- **Fracture Trait:** Çatlaktan fışkırmış, gövdesinde ışık sızdıran açık deliklerden rift enerjisi damlıyor.
- **Silüet:** 48×48, sivri uzun kollar, gövde küçük üçgen, başı silüetin %40'ı.
- **Auto-attack:** Yok. Temas hasarı 5 (negligible).
- **Skill 1 — Rift Lunge:** 0.4s cup-back tell, 0.5m ileri sıçrayış + 12 hasar. Recovery 0.6s (savunmasız window).
- **Skill 2 — Death Splatter:** Ölünce 0.3s sonra 1m radius rift goo (3s slow %20).
- **Codex Adjustment:** Death Splatter slow → player başına max 1 aktif debuff.
- **Warblade Counter:** LMB sweep tek vuruşta öldürür. Dash attack 3-4 imp grubunu temizler.
- **Synergy:** Shard Walker'la → Walker odağı çekerken Imp'ler arkadan lunge. Splatter goo'lar dash lane'ini kapatır.

---

## M02 — Shard Walker
- **Role:** Ranged Caster
- **Fracture Trait:** Vücudu yarısından kopmuş kristal levhalardan oluşuyor; her atışta bir levha kopup fırlatılıyor.
- **Silüet:** 112×112, dik dar gövde, omuzdan 3-4 keskin shard çıkıntısı.
- **Auto-attack:** Yok.
- **Skill 1 — Triple Shard:** 0.8s tell (3 shard parlar), 3 ardışık projectile (15° yelpaze), her biri 18 hasar.
- **Skill 2 — Fracture Burst:** Death triggered, 0.5s sonra 2m radius patlama (25 hasar).
- **Codex Adjustment:** Triple Shard → minimum range deadzone var (Warblade dash-in geçerli yanıt).
- **Warblade Counter:** Dash forward (yelpazeden geç), LMB-LMB-Ram. Ram knockback ile death burst'ten kaç.
- **Synergy:** Bruiser ile → Bruiser'ı kite ederken Walker arkadan triple shard.

---

## M03 — Seam Crawler
- **Role:** Skirmisher (hit-and-run)
- **Fracture Trait:** Zemindeki çatlaklarda yaşıyor; üst silüeti bir parça ışık ve gölge.
- **Silüet:** 96×96 ama yatay (geniş, 30px yükseklik), zemine yapışık, 6 pençe çıkıntısı.
- **Auto-attack:** Yok.
- **Skill 1 — Submerge (pasif):** %50 zaman zemin altında. Underground iken hasar almaz. 1.5m radius dim distortion shader.
- **Skill 2 — Burst Strike:** 1.0s yer altı approach (gölge telegraph), zeminden fırlayış 1m radius bite (28 hasar + 0.5s knockback). Burst sonrası 1.4s exposed.
- **Codex Adjustment:** Submerge strict max duration 2s + visible distortion zorunlu. Chain-submerge yasak.
- **Warblade Counter:** Gölgeyi takip et, fırladığı an dash-back + LMB combo.
- **Synergy:** Chain Warden ile → Warden zincirleri kilitterken Crawler burst strike. **ERKEN ODALARDA BİRLİKTE SPAWN YASAK.**

---

## M04 — Penitent Bruiser
- **Role:** Bruiser (telegraphed heavy)
- **Fracture Trait:** Fracturing enerjisini içine kapattı; göğsünden mor ışık dalgaları yayılıyor.
- **Silüet:** 128×128, omuzları aşağı çökmüş, kambur gövde, baş eğik.
- **Auto-attack:** Slow melee swing (1.0s telegraph, 30 hasar) — pozisyonel pressure, ana tehdit değil.
- **Skill 1 — Anti-Heal Aura (pasif):** 3m radius, içindeyken lifesteal/heal %50 azalır.
- **Skill 2 — Penitent Surge:** 1.2s tell, 3m radius AOE itme + 35 hasar + 0.5s stagger. Window 1.5s.
- **Warblade Counter:** Aura'yı zorla → 3m içine gir, Surge'den dodge, LMB sweep.
- **Synergy:** Imp ile → aura içinde Imp goo + Bruiser swing = sandwich.

---

## M05 — Chain Warden Echo
- **Role:** Charger / Mobility Punisher
- **Fracture Trait:** Eski hapishane muhafızının yankısı; zincirleri rift enerjisinden örülmüş.
- **Silüet:** 128×128, ağır zırh göğsü, omuzlardan 2 zincir uçuşan tendril.
- **Auto-attack:** Yok.
- **Skill 1 — Triple Chain:** 0.7s tell, 3 zincir 45° yelpazede (6m menzil). Hit = 1.5s slow %50.
- **Skill 2 — Chain Pull:** 1.0s tell, Warb 4m boss yönüne çekilir + 20 hasar. **Dash-immune.** Pull sonrası 1.2s window.
- **Codex Adjustment:** Chain Pull → dash-immune ama Iron Counter/parry ile kırılabilir.
- **Warblade Counter:** Triple Chain → dash side. Chain Pull → erken parry timing veya pull sonrası burst.
- **Faz 2 Note:** Pull dash-immune → Ranger/Gunslinger için gerçek tehdit.
- **Synergy:** Seam Crawler + Warden = en yüksek gerilim. **ERKEN ODALARDA BERABER SPAWN YASAK.**

---

## M06 — Relic Caster
- **Role:** Summoner / Spawner + buff support
- **Fracture Trait:** Elinde tuttuğu kırık relikvarın içinden minik rift fragmanları çağırıyor.
- **Silüet:** 80×80 (KÜÇÜK — execution priority cue), ince uzun gövde, dik kristal kırığı.
- **Auto-attack:** Yok.
- **Skill 1 — Summon Shardling:** 1.5s channel (relikvar parlar), 1.5m radius'ta 1 Shardling spawn (15 HP). CD 6s. Kanaldayken savunmasız.
- **Skill 2 — Aegis Mark:** 0.5s tell, en yakın allied mob'a 3s damage shield (%50 reduction). CD 5s. Mark'lı mob altın aura.
- **Codex Adjustment:** Aegis Mark shield break reward — hem target hem caster 0.5s stagger.
- **Warblade Counter:** EXECUTION TARGET. Channeling sırasında dash-in + LMB combo.
- **Synergy:** Chain Warden + Caster → Warden Mark'lı, Pull immune, Warb kapana sıkışır.

---

## M07 — Riftbound Augur
- **Role:** Debuffer / Disruptor
- **Fracture Trait:** Gözleri rift enerjisiyle dolu, etrafındaki tüm zaman algısını bozuyor.
- **Silüet:** 96×96, eğik duruş, gözleri yerine 3 rift fragmanı (tepe çıkıntısı), uzun pelerin.
- **Auto-attack:** Yok.
- **Skill 1 — Mark of Folly:** 1.0s tell (gözünden mor ışın), Warb'a 5s "Folly" (dash CD %50 uzar). Tell süresinde sight-break ile iptal.
- **Skill 2 — Time Shudder:** 0.8s tell, 4m radius 2s "stutter" (skill animasyonlar %30 yavaş, Rage gain yavaş). Window: alandan çık veya dash.
- **Warblade Counter:** Sight-break Folly'yi iptal eder. Shudder dışında savaş.
- **Synergy:** Penitent Bruiser ile → aura içinde + Time Shudder = heal yok hem yavaş.

---

## M08 — Hollow Hulk *(Elite variant / rare spawn)*
- **Role:** Mini-boss tier
- **Fracture Trait:** Fracturing içini boşalttı; gövdesi içinde rift enerjisi sallanan bir kovuk.
- **Silüet:** 160×160, masif kareli omuzlar, kollar gövdeden büyük, boyun çukurundan rift ışığı (headless titan).
- **Auto-attack:** Yok.
- **Skill 1 — Quake Slam:** 1.4s tell (kol yukarı kalkar), 3m radius AOE (60 hasar). Window 1.8s recovery.
- **Skill 2 — Cavity Pulse (pasif):** HP %50 altında aktive: her 4s 2m radius pulse (25 hasar). Phase change cue.
- **Skill 3 — Fracture Charge:** 1.0s tell, 6m düz çizgi charge (40 hasar + knockback). Dash perpendicular ile dodge. Window 2.0s.
- **Warblade Counter:** Quake telegraph net ama overlapping skill okuma testi. Cavity Pulse aktifken Hulk içinde değil dışında savaş.
- **Synergy:** Chain Warden ile → Warden Pull Warb'ı Hulk'un Quake AOE içine çeker.

---

---

## TASARIM İLKELERİ (Bu belge için)

Her düşman bir soru sormak zorunda:
> "Hangi oyuncu alışkanlığını kırar?"

Combat roller değil, davranış soruları tasarımı sürdürüyor:
- "Yerinde dursan cezalandırırım" → Zone Controller
- "Tek hedefe odaklanırsan gözden kaçarım" → Execution Target
- "Dash kullandığın anda vururum" → Mobility Punisher
- "AoE yoksa çözüm bulamazsın" → Swarm Pressure
- "Sustain varsa da işe yaramaz" → Anti-Heal

---

# ACT 1 — SHATTERED RUINS

## Combat Özeti

Act 1 savaş dili: **Düzen ve ritim öğretisi.**
Düşmanlar tehlikeli ama okunabilir. Her tehdit açıkça telegraf ediyor.
Oyuncu dodge'u, hedef önceliğini, melee-ranged mesafe yönetimini öğrenir.
Karmaşıklık az ama yanlış karar anında cezalandırır.

**Act 1'in Öğrettikleri:**
- Ranged + melee kombinasyonu nasıl yönetilir
- Kalabalıkta önce kim öldürülür
- Yerinde durmanın bedeli nedir
- Temel status (stun, slow) ne zaman devreye girer

---

## NORMAL DÜŞMANLAR (Act 1)

### 1. Shard Walker *(mevcut)*
- **Tür:** Grunt / Fractured
- **Combat Rolü:** Ranged pressure + death AoE
- **Oyuncuya Sorduğu Soru:** "Mesafeyi nasıl yönetirsin?"
- **Ana Davranış:** Orta mesafeden shard fırlatır (3'lü yayılım), ölünce patlama
- **Zorladığı Build:** Close-range melee — yaklaşmak zor, uzak durmak da değil
- **Zayıf Yönü:** Hareketli oyuncuya isabet ettirmesi zor; dash build'leri kolayca geçer
- **Siluet:** Çok parçalı, dağınık insansı; gap'lerden ışık sızıyor
- **Boyut:** ~~64px~~ → **112px** (player boyutuna yakın grunt — tehdit hissettirmeli)

---

### 2. Seam Crawler *(mevcut)*
- **Tür:** Skirmisher / Rift-Born
- **Combat Rolü:** Fast flanker, anti-kiting
- **Oyuncuya Sorduğu Soru:** "Çevrenin farkında mısın?"
- **Ana Davranış:** Zemin çatlaklarında kayar, çıkış animasyonu öncesinde görünmez
- **Zorladığı Build:** Turret-style caster; sabit duran oyuncuyu punish eder
- **Zayıf Yönü:** Yavaş ama öngörülü — çıkış noktası okunabilir
- **Siluet:** Yassı, zemine yapışık; sadece pençe ve omurga görünür — ama GENİŞ (yatay tehdit)
- **Boyut:** ~~48px~~ → **96px** (zemine yapışık ama geniş — yatay kaplama alanı büyük)

---

### 3. Void Thrall *(mevcut)*
- **Tür:** Splitter / Fractured
- **Combat Rolü:** Priority target — öldür ama dikkatli öldür
- **Oyuncuya Sorduğu Soru:** "Hedef önceliğin var mı?"
- **Ana Davranış:** Ölünce iki HalfThrall'a bölünür; yarılar daha hızlı
- **Zorladığı Build:** AoE — thrall'ı AoE ile öldürünce iki sorun olur
- **Zayıf Yönü:** Single target burst ile temiz öldürülünce split kontrollü
- **Siluet:** Uzun, ince, void tendrilleri; soluk mor aura
- **Boyut:** ~~96px~~ → **128px** (player boyutunda — split olmadan da tehdit hissettirmeli)

---

### 4. Chain Warden
- **Tür:** Controller / Fractured
- **Combat Rolü:** Mobility Check — dash ve kite'ı cezalandırır
- **Lore:** Hapisane muhafızının kalıntısı. Zincirleri hâlâ uçuşuyor, ama artık onu kontrol eden yok.
- **Oyuncuya Sorduğu Soru:** "Dash her şeyi çözer mi?"
- **Ana Davranış:**
  - 3 zincir fırlatır — her biri ayrı yöne, birbirinden 45° açıyla
  - İsabet → 1.5s yavaşlama (Chill değil, fiziksel slow)
  - Dash ile kaçınılır ama "dash = tanklama değil" öğretir
- **Zorladığı Build:** Melee sustain (yerinde duruyorsa zincirleri birden yer)
- **Zayıf Yönü:** Zincir fırlatma sonrası 2s açık pencere — telegraph açık
- **Oda Kompozisyonu:** Seam Crawler ile → oyuncu hem kaçamaz hem flankan yiyor
- **Siluet:** Ağır zırhlı ama zincirler silueti karmaşıklaştırıyor — zincirler belirgin
- **Boyut:** ~~80px~~ → **128px** (player boyutunda controller — görsel ağırlık zorunlu)
- **Varyant:** "Rusted Warden" — daha yavaş ama zincir hasarı daha yüksek (Act 1 geç)

---

### 5. Penitent
- **Tür:** Bruiser / Emergent
- **Combat Rolü:** Sustain Drain — iyileşmeyi baskılar
- **Lore:** Kendini cezalandırmak için var oldu. Yakınındaki her şeyi de cezalandırıyor.
- **Oyuncuya Sorduğu Soru:** "HP'yi kaynak olarak kullanıyor musun?"
- **Ana Davranış:**
  - Aura yayar: yakınında HP kazanma %50 azalır (Weakened benzeri ama HP'ye özel)
  - Yavaş, direkt melee — ama önce aura sorununu çözmen gerekiyor
  - Öldürülünce aura 3s daha devam eder
- **Zorladığı Build:** Lifesteal + on-hit heal build — aurada işe yaramaz
- **Zayıf Yönü:** Yavaş → kite edilebilir, range build için neredeyse ücretsiz
- **Oda Kompozisyonu:** Shard Walker ile → kaçmak zorunda ama kaçarken heal yok
- **Siluet:** Ağır, yuvarlak, içine çökmüş; omuzlar düşük; aura dalgası görünür
- **Boyut:** ~~80px~~ → **128px** (bruiser, player boyutunda — aura görsel olarak etkileyici olmalı)

---

### 6. Relic Caster
- **Tür:** Support / Fractured
- **Combat Rolü:** Execution Target — önce bu öldürülmeli
- **Lore:** Bir zamanlar mühür tutan büyücü. Mührü kırıldı ama büyüyü bilmeye devam ediyor.
- **Oyuncuya Sorduğu Soru:** "Önce kimi öldürürsün?"
- **Ana Davranış:**
  - Yakın düşmana kalkan verir (2s, kırılabilir ama zaman alır)
  - Kendisi zayıf — öldürmek kolay ama diğerleri kalkan alıyor
  - Kalkan verme cooldown: 4s — window var
- **Zorladığı Build:** Burst — önce caster öldürülmezse burst'ün tüketir
- **Zayıf Yönü:** En kırılgan düşman — sadece öncelik sorunu
- **Oda Kompozisyonu:** Chain Warden + Relic Caster → zincirlenirken caster'ı öldürmeye çalışmak
- **Siluet:** İnce, yüksek; elinde kırık relikvar; siluet net ve tek
- **Boyut:** **80px** (kasıtlı fragile support — oda içindeki en küçük figür, kolay hedef okunması zorunlu)
- **Varyant:** "Heretic Caster" (Act 2) — kalkan yerine anti-heal debuff

---

### 7. Fracture Imp
- **Tür:** Swarm Skirmisher / Rift-Born
- **Combat Rolü:** AoE Bait — AoE olmayanı boğar, AoE olanı kolayca temizlenir
- **Lore:** Çatlaktan fışkırdı. Küçük, ahmak ama hızlı ve çok.
- **Oyuncuya Sorduğu Soru:** "Yavaş build'inle sürüyü nasıl yönetirsin?"
- **Ana Davranış:**
  - 3-4 aynı anda spawn — her biri zayıf
  - Hızlı melee, önce çevreliyor sonra saldırıyor
  - Tek başına tehlike yok, kalabalıkta overwhelm eder
- **Zorladığı Build:** Single-target burst — teke tek temizler ama yorulur
- **Zayıf Yönü:** AoE veya cleave ile tek hareket
- **Oda Kompozisyonu:** Shard Walker ile — Walker odağı çekerken Imp'ler çevreliyor
- **Siluet:** Küçük, sivri; net ve farklı — diğer düşmanlarla karışmıyor
- **Boyut:** **48px** (true swarm tipi — kasıtlı küçük, ama 32px altı olmaz)

---

### 8. Ruin Hulk
- **Tür:** False Threat / Fractured
- **Combat Rolü:** Spatial Blocker — büyük görünür, az tehdit
- **Lore:** Bir zamanlar dev bir muhafız golem'iydi. Fracturing onu mahvetti — hâlâ büyük, ama içi boş. Adımları sarsıyor ama yumruğu artık boş hava kesiyor.
- **Oyuncuya Sorduğu Soru:** "Büyük olan her şeyden mi kaçıyorsun?"
- **Ana Davranış:**
  - 160px gövde — odaya girince intimidating
  - **Gerçek tehdit düşük:** Yavaş, her saldırı 2.5s telegraph (zemin titremesi, kol yavaşça kalkar)
  - Saldırı menzili büyük ama isabet penceresi dar — hareket edersen neredeyse sıfır hasar
  - HP'si görsel büyüklüğüyle orantısız düşük — birkaç hit
  - Alan tutar: geniş hitbox, koridorları tıkar, hedefleme sırasını bozar
- **Asıl Tehdidi:** Dikkat dağıtıcı. Arkasında başka düşman varken onu "büyük boss" sanıp odaklanırsın.
- **Zorladığı Build:** Yok — bu öğretici bir düşman. Büyük ama kolay. Refleks düzeltici.
- **Zayıf Yönü:** Her şey. Hızlı build'ler için ücretsiz hasar.
- **Oda Kompozisyonu:** Chain Warden + Ruin Hulk → oyuncu Hulk'tan kaçar, Warden zincirler
- **Siluet:** Masif, köşeli, golem benzeri — ama görünür çatlaklar ve kopuk parçalar. Açıkça bozuk.
- **Boyut:** **160px** (görsel büyüklük kasıtlı — oyuncu okumayı öğrenmeli)
- **Tasarım Notu:** Bu düşman yenilmez görünmeli ama öldürülünce "o kadar mı?" dedirtmeli.

---

## ELİTE DÜŞMANLAR (Act 1)

### E1. Iron Warden *(miniboss adayı, elite olarak da kullanılabilir)*
- **Tür:** Elite Bruiser / Fractured
- **Combat Rolü:** Oda Baskısı + Faz Testi
- **Oyuncuya Sorduğu Soru:** "Hareketli düşmana karşı burst'ün var mı?"
- **Ana Davranış:**
  - Faz 1: ağır melee + 360° zırhla knockback
  - %50 HP: saldırı hızı artar, zincir atağı ekler
  - Öldürülünce secondary class seçimi açar (act 1 boss olarak tasarlandı)
- **Zorladığı Build:** Yavaş ramp-up build'ler — Warden baskıyı keser
- **Oda Kompozisyonu:** Yalnız (boss) veya 2 Relic Caster ile (elite varyantı)
- **Siluet:** Devasa, köşeli zırh; boyundan büyük omuzlar
- **Boyut:** **256px** (boss) / **192px** (elite varyantı) — odaya girince "bu çok büyük" dedirtmeli

---

### E2. The Twice-Born *(mevcut)*
- **Tür:** Elite Pair / Fractured
- **Combat Rolü:** Resource Pressure — ikisini aynı anda yönet
- **Oyuncuya Sorduğu Soru:** "AoE mi yoksa focus fire mı?"
- **Ana Davranış:** Hasar birbirine %50 bölünür; biri ölünce diğeri berserk
- **Zorladığı Build:** AoE — her ikisini zayıflatır ama berserk'i önlemez
- **Siluet:** İkisi birbirinin aynası — ama renk ton farkı var (birincil/ikincil)
- **Boyut:** ~~80px~~ → **128px + 128px** (elite pair — her biri player boyutunda, ikisi birden baskılayıcı)

---

### E3. Fracture Knight
- **Tür:** Elite Skirmisher / Emergent
- **Combat Rolü:** Mirror Threat — oyuncunun dash'ini taklit eder
- **Lore:** The Fracturing anında bir şampiyonun hareket örüntüsünü emdi. Artık onun gibi hareket ediyor.
- **Oyuncuya Sorduğu Soru:** "Dash'ini ne zaman ve neden kullanıyorsun?"
- **Ana Davranış:**
  - Oyuncu dash kullandıktan 0.5s sonra Fracture Knight de dash yapar — aynı yönde
  - "Mirror dash" görsel cue ile belirtilir — öğrenilebilir
  - Yüksek hasar ama savunmasız hemen sonrası pencere var
- **Zorladığı Build:** Dash-heavy build — kendi silahıyla vurulur
- **Zayıf Yönü:** Mirror dash sonrası 1s stun window
- **Oda Kompozisyonu:** Chain Warden ile → dash'ini Warden için kullanırsan Knight sırtında
- **Grudge Bağlantısı:** ✅ — özellikle dash ile öldürülünce sonraki Fracture Knight daha çok mirror eder
- **Boyut:** ~~80px~~ → **160px** (elite skirmisher — oyuncudan açık büyük, ama hızlı — "büyük ama hızlı" paradoksu tehdit hissini artırır)

---

### E4. The Reliquary
- **Tür:** Elite Construct / Fractured
- **Combat Rolü:** Zone Control + Buff Engine
- **Lore:** Mühür odası muhafızı. Artık mühür kırık ama o hâlâ burada.
- **Oyuncuya Sorduğu Soru:** "Tampon bölgeyi kırabilir misin?"
- **Ana Davranış:**
  - Sabit (hareket etmez), etrafında 4 shard döner
  - Shard'lar yakın düşmanlara buff verir (+hasar, +hız)
  - Shard'lar tek tek kırılabilir — ama her shard kırılınca Reliquary hasar verir
  - 4 shard bitince savunmasız
- **Zorladığı Build:** Burst build'ler shard'ları patlatmak ister ama hasar alır
- **Oda Kompozisyonu:** Fracture Imp ile → Imp'ler bufflanınca overwhelm
- **Siluet:** Yuvarlak, simetrik; dönün shard'lar belirgin
- **Boyut:** ~~96px~~ → **160px** (sabit ama büyük — hareket etmediği için büyüklüğü tehdit sinyali olmalı)

---

## ACT 1 KOMPOZİSYON TAVSİYELERİ

**En iyi temel kombinasyonlar:**
- Shard Walker × 2 + Fracture Imp × 3 → ranged/swarm karışımı, AoE/single test
- Chain Warden + Void Thrall → mobility kilidi + split baskısı
- Penitent + Shard Walker × 2 → heal yok, kite de zor
- Relic Caster + Seam Crawler × 2 → önce caster ama flanker baskısı

**Elite + support:**
- Fracture Knight + Relic Caster × 2 → Knight kalkan alıyor, dash kullanınca counter
- The Reliquary + Fracture Imp × 4 → bufflanmış sürü, reliquary odağı çekiyor

**En yüksek gerilim odası:**
- Chain Warden + Seam Crawler × 2 + Relic Caster → hareket kilidi + flanker + destek katmanı
- The Twice-Born + Penitent × 2 → hasar bölünmüş, heal yok, berserk baskısı

**Grudge sistemine en iyi bağlananlar:**
- Fracture Knight (en belirgin) → dash ile öldürülünce mirror güçlenir
- Shard Walker → ateşle öldürülünce ateş direnci + görsel değişim
- The Twice-Born → fiziksel hasar ile öldürülünce berserk daha uzun

**Görsel olarak en ayırt edici aileler:**
- Fractured ailesi (parçalı, gap'lerden ışık) vs Rift-Born (zemine yapışık, organik)

---

# ACT 2 — BLEEDING WASTES

## Combat Özeti

Act 2 savaş dili: **Sustain baskısı ve kaynak yönetimi.**
Düşmanlar artık sadece hasar vermiyor — iyileşeni engelliyor, destek veriyor, birikim yapıyor.
Oyuncu build'inin sınırlarını keşfeder; AoE+sustain combo'ları zorlanır.

**Act 2'nin Öğrettikleri:**
- Status effect önceliklendirme
- Destek hedefleri erken eliminasyon
- Kaynaktan tasarruf (mana, cooldown, HP)
- Pozisyonel zone control altında hareket

---

## NORMAL DÜŞMANLAR (Act 2)

### 1. Mire Stalker
- **Tür:** Skirmisher / Rift-Born
- **Combat Rolü:** Mobility Punisher + Zone Control
- **Lore:** Bataklığın ta kendisi hareket ediyor. Bir hedefe kilitlenince bataklık onun etrafında büyüyor.
- **Oyuncuya Sorduğu Soru:** "Çok yerinde durdun mu?"
- **Ana Davranış:**
  - Oyuncunun durduğu zemine bataklık bırakır (3s sonra oluşur — telegraph)
  - Bataklıkta duran oyuncu Slowed + Decayed (hafif anti-heal)
  - Kendisi hızlı ama hasarı düşük
- **Zorladığı Build:** Turret + sustain → sabit duruyorsa bataklık üstünde iyileşiyor
- **Zayıf Yönü:** Bataklık hareketliyle işe yaramaz — dash/kite build'e neredeyse ücretsiz
- **Boyut:** 64px, uzun bacaklar, bataklık damlıyor

---

### 2. Rot Priest
- **Tür:** Support / Fractured
- **Combat Rolü:** Anti-Heal Engine — Execution Target
- **Lore:** Yozlaşmış bir şifacı. Artık iyileştirdiği şey onu daha fazla tüketiyor.
- **Oyuncuya Sorduğu Soru:** "Sustain build'in Rot Priest varken işe yarıyor mu?"
- **Ana Davranış:**
  - Yakın düşmanlara Regeneration verir (2 HP/s, 5s)
  - Oyuncuya Rot Curse uygular: sonraki 8s heal alma %70 azalır
  - Kendisi çok zayıf ama curse her zaman önce çözülmeli
- **Zorladığı Build:** Lifesteal, on-hit heal, flask build — sustain'in gider
- **Zayıf Yönü:** Kırılgan, önce o ölünce tüm buffs düşer
- **Oda Kompozisyonu:** Thorn Brute ile → önce Priest ama Brute baskı kuruyor
- **Boyut:** 64px, eğik, çürük tören kıyafeti

---

### 3. Thorn Brute
- **Tür:** Bruiser / Emergent
- **Combat Rolü:** Anti-Melee — dikenli refleks
- **Lore:** Acıdan beslenmeye başladı. Her hasar aldığında daha sert büyüyor.
- **Oyuncuya Sorduğu Soru:** "Melee ile direktten vurursan ne olur?"
- **Ana Davranış:**
  - Hasar aldığında 0.5s thorn aura aktif → temas hasarı
  - Direkt melee isabeti: oyuncuya hasar miktarının %30'u geri döner
  - Yavaş ama yüksek HP
- **Zorladığı Build:** Melee burst / close-range AoE — kendi hasarını yer
- **Zayıf Yönü:** Yavaş; range/projectile build için sadece yürüyen bir hedef
- **Oda Kompozisyonu:** Mire Stalker ile → bataklıkta kalmak zorunda, Brute saldırıyor
- **Boyut:** 96px, dikenlerle kaplı, şişirilmiş üst gövde

---

### 4. Carrion Weaver
- **Tür:** Controller / Rift-Born
- **Combat Rolü:** Summon Pressure + Zone Denial
- **Lore:** Çürüyen her şeyi dokuyor. Hareketleri mekanizm gibi, amaçsız değil.
- **Oyuncuya Sorduğu Soru:** "Küçükleri yok etmeden büyüğü öldürürsün, büyük daha hızlı dokuyor."
- **Ana Davranış:**
  - 3-4s'de bir küçük "Rotling" spawn eder (HP: 15, hızlı melee)
  - Kendisi orta mesafede durur, Rotling yönetir
  - Öldürülünce son bir Rotling burst'ü (3-4 aynı anda)
- **Zorladığı Build:** Single-target focused — Weaver'a ulaşamadan Rotlings overwhelm
- **Zayıf Yönü:** AoE → Rotlings temizlenir, Weaver açıkta
- **Oda Kompozisyonu:** Rot Priest ile → Priest Weavera buff veriyor, önce kim?
- **Boyut:** 80px (Weaver) + 24px (Rotling)

---

### 5. Blood Lancer
- **Tür:** Skirmisher / Fractured
- **Combat Rolü:** Sustain Enemy + Burst Punisher
- **Lore:** Kendi kanını silah yaptı. Kan döküldükçe güçleniyor.
- **Oyuncuya Sorduğu Soru:** "Hasar verirken hasar alıyor musun?"
- **Ana Davranış:**
  - Oyuncuya hasar verince HP kazanır (lifesteal)
  - %30 HP altında: hasar +50%, lifesteal 2x
  - "Low HP = daha tehlikeli" → erken öldürmek mantıklı ama zor
- **Zorladığı Build:** DoT (poison, burn) — Lancer lifesteal ile DoT'u karşılar, zaman uzuyor
- **Zayıf Yönü:** Burst — low HP'ye düşmeden önce öldürülünce mekanik aktif olmaz
- **Boyut:** 72px, uzun mızrak, kan damlıyor

---

### 6. Husk Thrower
- **Tür:** Ranged / Fractured
- **Combat Rolü:** AoE Punisher — kalabalık alanlarda zorunlu hareket
- **Lore:** Kendi bedeninin parçalarını fırlatıyor. Her atıştan sonra biraz daha küçülüyor.
- **Oyuncuya Sorduğu Soru:** "Gruplandın mı? Hasar al."
- **Ana Davranış:**
  - Lobbing projectile (arkust atar) — ground impact, 2s sonra patlama
  - Impact point görünür (cue) — dodge penceresi var ama sınırlı
  - Yakında birden fazla düşman varsa chain splash
- **Zorladığı Build:** Melee toplanması gereken build → splash hasardan kaçmak için ayrılmak zorunda
- **Zayıf Yönü:** Uzak durulursa kendi ground impact'ini de yer; melee ile yaklaşan rahat
- **Boyut:** 64px, şişirilmiş beden, kollar fırlatma pozisyonunda

---

### 7. Decay Anchor
- **Tür:** Construct / Emergent
- **Combat Rolü:** Zone Control — hareket kilidi
- **Lore:** Durmak için var oldu. Çevresindeki her şeyin de durmasını istiyor.
- **Oyuncuya Sorduğu Soru:** "Kiting stratejin zone control altında çalışıyor mu?"
- **Ana Davranış:**
  - Sabit (hareket etmez)
  - Etrafında Decay Zone: 4-tile radius, içinde Slow + HP regen bloke
  - Kendisi saldırmaz ama yok edilemezse zone kalıcı
  - Yüksek HP, zırhlı
- **Zorladığı Build:** Kiting build — zone içinde kalmak zorunda kalınca kite bozuluyor
- **Zayıf Yönü:** Sabit → saldırı açısı her zaman biliniyor; range build için basit
- **Oda Kompozisyonu:** Blood Lancer + Decay Anchor → zone içinde Lancer'a hasar verilirse tehlikeli
- **Boyut:** 80px (köklenmiş görünüm, bacak yok)

---

## ELİTE DÜŞMANLAR (Act 2)

### E1. The Devourer
- **Tür:** Elite Bruiser / Emergent
- **Combat Rolü:** Scaling Threat — ne kadar geç öldürürsen o kadar güçlenir
- **Lore:** Her ölümden bir şeyler emdi. Artık kendisi tam olarak ne olduğunu bilmiyor.
- **Oyuncuya Sorduğu Soru:** "Hızlı burst çözümün var mı yoksa zaman içinde mi kazanırsın?"
- **Ana Davranış:**
  - Odada her düşman ölünce +10% hasar ve hareket hızı kazanır
  - Son düşman olunca: form değiştir, hız x2, hasar x1.5
  - Tek başına neredeyse tehlikesiz — ama son kalması felakettir
- **Zorladığı Build:** AoE temizleme → Devourer hızla power spike yapar
- **Zayıf Yönü:** Önce Devourer → sonra oda temizlenir, ama bunu yapmak zordur çünkü diğerleri engeller
- **Grudge Bağlantısı:** ✅ Güçlü aday — öldürme tipine göre Act 3'te güçlendirilmiş "Named Devourer" spawn

---

### E2. Thornmother
- **Tür:** Elite Controller / Emergent
- **Combat Rolü:** Zone Denial + Summon Synergy
- **Lore:** Çürüyen ormanın kalbi. Her diken onu yaşatıyor.
- **Oyuncuya Sorduğu Soru:** "Zone içinde savaşabilir misin?"
- **Ana Davranış:**
  - Etrafına 3 Thorn Wall çizer (hareket bloğu, temas hasarı)
  - Thorn Walls arasına Mire Stalker spawn eder (2 adet)
  - Thorn Walls yıkılabilir ama HP'si yüksek
- **Zorladığı Build:** Melee burst → Thornmother'a ulaşmak için duvarları yıkmak gerek
- **Oda Kompozisyonu:** Husk Thrower ile → wall içinde lobbing projectile
- **Boyut:** 112px, köklere bağlı, dikenler taç gibi

---

### E3. Withered Apostle
- **Tür:** Elite Debuffer / Fractured
- **Combat Rolü:** Build Shutdown — hangi build olursa olsun bir şeyini kapatır
- **Lore:** Bir zamanlar herkese vaaz verirdi. Şimdi sadece alıyor.
- **Oyuncuya Sorduğu Soru:** "Build'inin yüzde kaçı bu düşmana karşı işe yarıyor?"
- **Ana Davranış:**
  - Her 8s'de bir random Curse uygular: Weakened / Anti-Heal / Slow / Cooldown Extend
  - Curse stack'lenir — uzun combat'ta 3 ayrı curse olabilir
  - Kendisi zayıf ama önce onu öldürmek diğerlerini açık bırakır
- **Zorladığı Build:** Herkesi zorlar ama en çok cooldown bağımlı build'leri
- **Oda Kompozisyonu:** Thorn Brute + Blood Lancer ile → curse + anti-melee + lifesteal = felakete
- **Boyut:** 80px, soluk, eğilmiş

---

### 8. Bog Giant *(Act 2 False Threat)*
- **Tür:** False Threat / Rift-Born
- **Combat Rolü:** Distraction Tank — büyük, korkutucu, ama zararsız yönetilir
- **Lore:** Bataklık kütlesini hapsetti ve ayağa kaldırdı. Büyük ama aptal. Fracturing onu güçlendirmedi — sadece büyüttü.
- **Oyuncuya Sorduğu Soru:** "Boyut = tehdit mi? Arkasındaki Rot Priest'i gördün mü?"
- **Ana Davranış:**
  - 192px gövde — Act 2'de ilk görünce intimidating
  - **Gerçek tehdit çok düşük:** Yavaş, her atak çok telegraphed, bataklık zeminde kayıyor (daha da yavaşlıyor)
  - Ölünce 3-4 Mire patch bırakır — tek gerçek tehdidi budur
  - Varlığıyla oyuncunun görüş alanını kapatır (büyük sprite = kamera gizleme)
- **Asıl Tehdidi:** Arkasını kapatır. Rot Priest veya Blood Lancer arkasına saklanır.
- **Zorladığı Build:** Hiçbiri — sadece dikkat dağıtıcı
- **Zayıf Yönü:** Her şey. Çok HP ama hasar vermez.
- **Oda Kompozisyonu:** Bog Giant + Rot Priest → Giant önde, Priest arkada — Giant'ı yok sayıp Priest'i bulmak asıl karar
- **Siluet:** Devasa bataklık kütlesi, bitki/çamur karışımı — organik, pürüzlü, yavaş. Hiçbir şeyi keskin değil.
- **Boyut:** **192px** — kasıtlı büyük, kasıtlı zayıf
- **Tasarım Notu:** Ruin Hulk'un Act 2 versiyonu. Oyuncu Act 1'de öğrendiyse burada hemen "sahte tehdit" okur.

---

## ACT 2 KOMPOZİSYON TAVSİYELERİ

**En iyi temel kombinasyonlar:**
- Rot Priest + Mire Stalker × 2 → heal yok + bataklık + flanker
- Carrion Weaver + Blood Lancer → micro-management + sustain enemy
- Thorn Brute + Husk Thrower × 2 → melee zorlanır, menzil de zorlanır

**Elite + support:**
- The Devourer + Mire Stalker × 3 → sürüyü temizle ama Devourer güçleniyor
- Thornmother + Husk Thrower × 2 → zone içinde lobbing, çıkış yok

**En yüksek gerilim odası:**
- Withered Apostle + The Devourer + Rot Priest → triple priority, triple curse

**Grudge sistemine en iyi bağlananlar:**
- The Devourer → "Named" olma potansiyeli en yüksek
- Blood Lancer → lifesteal ile öldürülünce Act 3'te lifesteal direnci

---

# ACT 3 — CORE APPROACH

## Combat Özeti

Act 3 savaş dili: **Adaptasyon ve otomasyon cezası.**
Düşmanlar oyuncunun alışkanlıklarına cevap verir. Aynı stratejiyle geçilmez.
Oyuncu bilinçli karar almak zorundadır — otomatik pilot çalışmaz.

**Act 3'ün Öğrettikleri:**
- Her düşman bir soru sormak zorunda → oyuncu her odada aktif düşünmeli
- Pattern okuma → tepki verme değil, öngörme
- Susturulan skill → alternatif

---

## NORMAL DÜŞMANLAR (Act 3)

### 1. Echo Striker
- **Tür:** Skirmisher / Emergent
- **Combat Rolü:** Mirror Combat — en son kullanılan skill'i taklit eder
- **Lore:** Savaş sesini emdi. Artık o sesin ta kendisi.
- **Oyuncuya Sorduğu Soru:** "Kullandığın skill'den kaçabilir misin?"
- **Ana Davranış:**
  - Oyuncunun en son kullandığı skill'in basit bir versiyonunu kopyalar
  - Kopyalanmış skill oyuncuya yönelir (hasar orijinalin %60'ı)
  - Copy 4s gecikmeyle gelir — süre içinde öldürülürse copy iptal
- **Zorladığı Build:** AoE skill → kendi AoE'si kendine döner; mobility skill → echo dash
- **Zayıf Yönü:** 4s pencerede öldürülmeli — burst gerekli
- **Boyut:** 80px, oyuncunun mirroru gibi görünür ama bozuk

---

### 2. The Witness
- **Tür:** Support / Emergent
- **Combat Rolü:** Execution Target — öldürülmezse tüm düşmanlar güçlenir
- **Lore:** Sadece bakıyor. Ama baktığı her şey daha güçlü oluyor.
- **Oyuncuya Sorduğu Soru:** "Bu oda içindeki en tehlikeli düşman hangisi — ve o kalabalığın içinde mi?"
- **Ana Davranış:**
  - Saldırı yapmaz, kaçmaya çalışır
  - Hayatta olduğu sürece: tüm düşmanlara +25% hasar
  - Öldürülürse: buff kalkar, Witness 3s sonra yeni bir noktada spawn
  - Odadan kaçamaz ama hareket hızı yüksek
- **Zorladığı Build:** AoE → Witness orada ama kalabalıkta kaybolabilir
- **Zayıf Yönü:** Çok düşük HP — bulunursa anında ölür; sorun bulmak
- **Boyut:** 48px, soluk, yok olmaya yakın siluet

---

### 3. Null Knight
- **Tür:** Bruiser / Fractured (bozulmuş)
- **Combat Rolü:** Resource Pressure — skill'leri geçici olarak susturur
- **Lore:** Savaş yeteneği The Fracturing tarafından silinmek üzereydi. Ama tamamen silinmedi.
- **Oyuncuya Sorduğu Soru:** "Skill olmadan savaşabilir misin?"
- **Ana Davranış:**
  - Melee hasar → hedef skilli 3s cooldown uzar
  - Özel saldırı (8s cd): "Null Pulse" — 2s süre tüm skill'ler pasif
  - Yüksek HP, orta hız
- **Zorladığı Build:** Cooldown yönetimi kritik olan build — skill döngüsünü keser
- **Zayıf Yönü:** Null Pulse animasyonu okunabilir — dodge edilebilir
- **Boyut:** 80px, zırh çatlıyor, içinden void akar

---

### 4. Goldfract Sentry
- **Tür:** Construct / Emergent
- **Combat Rolü:** Anti-Ranged — projectile reflect
- **Lore:** Altın çatlaklar artık savunma için var. Hiçbir şey geçemiyor.
- **Oyuncuya Sorduğu Soru:** "Ranged build'in bu düşmana ne yapacak?"
- **Ana Davranış:**
  - Projectile refleksi: kendine gelen projectile'ları geri fırlatır
  - Melee'ye tamamen açık — ama Act 3'te melee zor
  - Zaman zaman altın bariyeri aktif eder (3s, tüm projectile'lar bloke)
- **Zorladığı Build:** Ranged/caster → kendi ateşini yer
- **Zayıf Yönü:** Melee ile direkt — sentry saldırmaz, sadece yansıtır
- **Boyut:** 80px, statik, altın kaplamalar

---

### 5. Archivist
- **Tür:** Controller / Emergent
- **Combat Rolü:** Positioning Punisher — oyuncunun hareket pattern'ini kaydeder
- **Lore:** Her şeyi yazıyor. Senin hareketlerini de.
- **Oyuncuya Sorduğu Soru:** "Hep aynı rotayı mı izliyorsun?"
- **Ana Davranış:**
  - 6s boyunca oyuncunun pozisyonlarını kaydeder
  - 7. saniyede: kaydedilen her pozisyona bir void spike bırakır
  - Aynı rotayı izlemek = tüm spike'lar üstünde
  - Her 15s'de tekrar kaydeder
- **Zorladığı Build:** Predictable kiting pattern — spiral, duvar boyunca koşmak cezalandırılır
- **Zayıf Yönü:** Pattern değiştirilirse spike'lar ıssız yere düşer; random hareket çözüm
- **Boyut:** 72px, yüzen kitaplar etrafında, nazik görünüm

---

### 6. Void Anchor (Act 3 Varyantı)
- **Tür:** Construct / Rift-Born
- **Combat Rolü:** Skill Zone Denial
- **Lore:** Act 2'deki Decay Anchor'ın Act 3 evrimi. Artık bedenini değil yeteneklerini hedef alıyor.
- **Ana Davranış:** (Decay Anchor benzeri ama farklı)
  - Zone içinde skill kullanımı +50% cooldown artar
  - Zone'dan çıkınca cooldown normale döner
  - Oda merkezi veya koridora stratejik yerleştirilir
- **Zorladığı Build:** Cooldown döngüsü bağımlı build — zone içinde kilitlenir
- **Boyut:** 80px

---

## ELİTE DÜŞMANLAR (Act 3)

### E1. The Recursion
- **Tür:** Elite Adaptive / Emergent
- **Combat Rolü:** Build Counter — hasar tipine direnç kazanır
- **Lore:** Her öldürme girişimini kaydetti. Şimdi o kayıtlara göre var oluyor.
- **Oyuncuya Sorduğu Soru:** "Tek hasar tipine bağımlı mısın?"
- **Ana Davranış:**
  - Her 5 saniyede bir en çok aldığı hasar tipini analiz eder
  - %30 direnç kazanır o tipe karşı
  - Oyuncu farklı tip değiştirince analiz sıfırlanır (5s)
  - "Adaptation" cue: renk değişimi
- **Zorladığı Build:** Mono-damage build — tek elementin görmezden gelinir
- **Zayıf Yönü:** Mixed damage build → Recursion her analizi boşa çıkar
- **Grudge Bağlantısı:** ✅ En güçlü aday — "Named Recursion" haline geldiğinde baştan build counter

---

### E2. Mirror Paladin
- **Tür:** Elite Mirror / Fractured
- **Combat Rolü:** Skill Reflection + Anti-Build
- **Lore:** Bir şampiyonun gölgesi. Ama gölge şimdi kendi ışığını yapıyor.
- **Oyuncuya Sorduğu Soru:** "Build'ine karşı kendi build'in."
- **Ana Davranış:**
  - Açılışta oyuncunun equipped skill'lerini "tarar" (2s)
  - Her equipped skill için bir "echo" saldırısı oluşturur
  - Oyuncu skill değiştirirse tarama yenilenir
- **Zorladığı Build:** Spesifik combo build → kendi combo'su döner
- **Zayıf Yönü:** Erken burst (2s tarama bitince aktif) → tarama tamamlanamaz
- **Boyut:** 96px, oyuncunun renk şemasında ama bozuk

---

### Miniboss Adayı: The Unfinished
- **Tür:** Miniboss / Emergent
- **Combat Rolü:** Act 1 ve Act 2 dilinin sentezi
- **Lore:** Ne olacağına karar verilemedi. Hâlâ değişiyor.
- **Ana Davranış:**
  - Faz 1: Act 1 savaş dili — chain, shard, melee pressure
  - %60 HP: Act 2 dili — rot curse, decay zone, sustain drain
  - %30 HP: Act 3 dili — echo strike, mirror move, null pulse
  - Her faz geçişinde görsel dönüşüm
- **Tasarım Amacı:** Oyunculara Act 3 öncesinde tüm Act savaş dillerini hatırlatır
- **Boyut:** 128px, değişken form

---

## ACT 3 KOMPOZİSYON TAVSİYELERİ

**En iyi temel kombinasyonlar:**
- Echo Striker + Null Knight → skill kullanırsa echo döner, kullanmazsa Null Pulse
- The Witness + Archivist → Witness bul, ama Archivist pattern kaydediyor
- Goldfract Sentry × 2 + Null Knight → ranged çalışmıyor, melee önerilir ama null pulse

**Elite + support:**
- The Recursion + Void Anchor → zone içinde kilitli, tek element işe yaramıyor
- Mirror Paladin + The Witness → önce Witness ama Paladin sana dönüyor

**En yüksek gerilim odası:**
- Echo Striker + Archivist + Null Knight → her alışkanlık cezalandırılıyor

**Grudge sistemine en iyi bağlananlar:**
- The Recursion → en güçlü Nemesis adayı
- Echo Striker → kopyaladığı skill türüne göre özelleşir

---

# PROTOTIP VE ÜRETİM KARARLARI

## İlk Prototip İçin Çekirdek Düşman Seti

Sadece bunlarla başla — geri kalanı sonra:

| Düşman | Act | Neden önce |
|---|---|---|
| Shard Walker | 1 | Mevcut, ranged/swarm prototype |
| Void Thrall | 1 | Mevcut, split mechanic testi |
| Chain Warden | 1 | Mobility check öğretisi |
| Rot Priest | 2 | Sustain counter — core sistemi test eder |
| Echo Striker | 3 | Act 3 dili — en özgün mekanik |

**5 düşman, 3 act'in tamamını test eder.**

---

## Vertical Slice İçin Önerilen Düşman Sayısı

| Act | Normal | Elite | Miniboss |
|---|---|---|---|
| Act 1 | 4 (Walker, Thrall, Warden, Relic Caster) | 1 (Twice-Born) | Iron Warden |
| Act 2 | 4 (Mire Stalker, Rot Priest, Thorn Brute, Husk Thrower) | 1 (The Devourer) | — |
| Act 3 | 3 (Echo Striker, Null Knight, The Witness) | 1 (The Recursion) | — |

**Toplam: 11 normal + 3 elite + 1 boss = yönetilebilir vertical slice.**

---

## PixelLab Üretim İçin En Uygun Aileler

**En kolay üretilenler (net siluet, az detay, tekrar üretilebilir):**
- Fracture Imp (32px swarm) → basit, çok sayıda
- Relic Caster (ince, standart insansı)
- Decay Anchor (statik, simetrik)
- The Witness (48px, minimalist)

**Orta zorlukta (aile varyantları iyi çıkar):**
- Shard Walker ailesi → Act 1/2/3 renk varyantı kolay
- Thorn Brute → dikenlerin silueti belirgin
- Blood Lancer → uzun silah silueti

**Zor (özel siluet, PixelLab tutarsız olabilir):**
- Seam Crawler → zemine yapışık form PixelLab'ı zorlar, Aseprite hybrid önerilen
- Echo Striker → oyuncuya benzeyecek ama farklı — tutarlılık riski
- The Unfinished → şekil değişiyor, birden fazla sprite gerekiyor
