# RIMA — Sınıf Oynanış Kılavuzu
*Versiyon: 2026-04-14 | Kaynak: SINIF_VE_SKILL_KARAR_BELGESI.md v3*

Her sınıf için kaynak sistemi, temel döngü, pratik kombinasyonlar ve oynanış tarzı açıklanmıştır.

---

## İçindekiler
1. [Warblade](#warblade)
2. [Elementalist](#elementalist)
3. [Shadowblade](#shadowblade)
4. [Ranger](#ranger)
5. [Ravager](#ravager)
6. [Ronin](#ronin)
7. [Gunslinger](#gunslinger)
8. [Brawler](#brawler)
9. [Summoner](#summoner)
10. [Hexer](#hexer)

---

## ⚔️ 1. WARBLADE

**Kimlik:** Yaklaş. Sabitle. Zırhı kır. İnfaz et.

**Kaynak — Rage (0-100)**
Her vuruşta +10 Rage, CC'li düşmana vurunca +20. Savaş bitince (oda temizse) Rage drainlanmaz — bir sonraki odaya dolu girebilirsin. Rage boşaldığında hasar vermez, ama Bladestorm'a girdiğinde sıfırlanır.

**Temel Hareket**
- **LMB — Iron Combo:** 3 vuruşluk zincirleme. Sweep → overhead → shoulder ram. Son vuruş küçük knockback ve +15 Rage verir. 0.8 saniye LMB'ye basmazsan zincir sıfırlanır. Hızlı ve tutarlı hasar kaynağın.
- **RMB — Rage Outlet:** Rage 30+ olunca kullanılabilir. Çevrendeki düşmanlara kısa AoE patlaması, Rage −30. Hem hasar hem itme — paketleri bozan, nefes alan buton.

**[V] — BLADESTORM**
Rage 100 dolunca tetiklenir. 5 saniye boyunca dönerek tüm çevreye her 0.5 saniyede %120 AoE hasar verirsin, CC immune. Kullanırken hareket edebilirsin. Odayı temizlemenin en garantili yolu.

---

### Nasıl Oynanır?

Warblade **yakın mesafe ekseni** üzerine inşa edilmiş, yüksek burst ve CC kontrollü bir bruiser'dır. Her oda aynı mantıkla açılır: yaklaş, düşmanı sabitle, zırhını kır, öldür.

**Temel Döngü:**
1. **Iron Charge** ile 8 metre atla — hedef 1.5 saniye stun olur, sana +20 Rage gider.
2. Stun sırasında **Sunder Mark** uygula — 8 saniye zırh %40 azalır.
3. **Iron Crush** aç — 6 saniye boyunca tüm hasarın %30 artar.
4. **Crippling Blow** vur — büyük hasar + iyileşme %50 kesilir.
5. HP %30'un altına düşürünce **Death Blow** ile bitir (%400 hasar).

Bu döngü tek hedefi hızlı ve temiz öldürür. Birden fazla düşman varsa **War Stomp** ile 3 metre knockup yap, **Gravity Cleave** ile çek, **Rage Outlet** ile patlat.

**Önemli Sinerjiler:**
- **Iron Charge → Crippling Blow:** İlk vuruş stun'daysa iyileşme kesimi %100'e çıkar.
- **Sunder Mark + Iron Crush:** İkisi aynı anda aktifken hasar katlanır. Her rotation'da önce Mark, sonra Crush.
- **Battle Surge + Rage Outlet:** Battle Surge aktifken her Rage harcaması HP+%5 verir. Rage Outlet'i döngüsel kullanırsan kendin iyileştirir gibi oynar.
- **Deep Wound:** Bleed DoT açar ve Rage+35 verir — Iron Crush penceresi açıkken kullanırsan bleed tick ×2 hızlı döner.
- **Iron Counter:** Yakın dövüşte bir darbeyi karşılarsan %180 hasar + Rage+25. Ama bu bir "bekle-kazan" butonu değil — pasif bırakma, sadece tepki ver.

**Build Yönleri:**
- **Execution (Tek Hedef Boss):** Iron Charge + Crippling Blow + Iron Crush + Death Blow
- **Control Breaker (Kalabalık):** Gravity Cleave + War Stomp + Sunder Mark + Death Blow
- **Last Stand (Hayatta Kalma):** Ironclad Momentum + Iron Counter + Battle Surge + Death Blow

**Playtest Notu:** Iron Counter, Ronin'e benzer bir counter kimliği yaratıyor. İframe büyütme ekleme — defansif pencere yeterli.

---

## 🔥 2. ELEMENTALİST

**Kimlik:** Her şeyi yakıyorum. Ama önce ritmi buluyorum.

**Kaynak — Mana (0-100, +8/sn) + Elemental State (Fire/Frost, max 5 stack)**
Mana otomatik dolar — bunun tek sınırı harcama hızın. Elemental State ise aktif elementine göre birikir: Fire State Fire skilleriyle, Frost State Frost skilleriyle. State stack sayısı skill güçlendirmelerini ve chain bonus koşullarını etkiler.

**Temel Hareket**
- **LMB — Rift Bolt:** Hareket halinde atılabilir, hiç CD yok. Her 3. bolt "empowered" olur — daha büyük, +1 Elemental State verir. Mana+3/isabet. Ana hasar kaynağın ve State yükleme aracın.
- **RMB — Element Switch:** Fire ↔ Frost anında geçiş, CD yok. Geçişte aktif element tipinde küçük nova patlaması. Bu geçişi ritmik kullanmak sınıfın öz kimliği — sürekli tek elementte kalmak verimsiz.

**[V] — INFERNO**
Mana 100 dolunca: 7 saniye arena-wide ateş yağmuru. En geniş AoE burst. Genellikle Fire State birikince girilir.

---

### Nasıl Oynanır?

Elementalist bir **ritim caster**'dır. "Tek element" mantığıyla oynayan oyuncu verimsiz kalır — asıl güç element geçişlerinden ve chain bonuslarından gelir.

**Temel Döngü (Fire Burst):**
1. **Combustion** aç — 8 saniye tüm Fire spelller instant cast, mana maliyeti ×2 (Fire State 5 stack varsa maliyet artmaz).
2. Combustion penceresinde **Fireball** × 3 at — 3.'sünde Living Bomb ücretsiz çıkar.
3. **Living Bomb** patlamadan önce **Element Switch** ile Frost geç — nova patlaması, Glacial Spike ile slowed hedefe Living Bomb patlama bonusu kazanırsın.
4. **Meteor** ile bitir — slowed/frozen hedefte knockdown 3s + %50 hasar bonusu.

**Frost Döngüsü:**
1. **Blizzard** aç — kanal gereksiz, hareket ederken bölge slow+tick.
2. **Glacial Spike** ile hat temizle — Fire State tüketirse hasar artar.
3. **Frozen Orb** ile geniş alan kapat.
4. Blizzard altında Meteor — knockdown 4s.

**Önemli Sinerjiler:**
- **Fireball DoT + Glacial Spike:** Ateş DoT aktif hedefe buz hattı vurursan Freeze 2s + DoT anında patlar.
- **Frozen Orb + Blink:** Orb üzerinden Blink geçersen Orb patlar, Frozen 2s.
- **Arcane Blast → Barrage:** 4 cast stack'te Barrage açılır. Cap 4 — cross-class ile kaldırılamaz.
- **Arcane Surge + Blink:** Surge aktifken Blink'in vardığı noktada patlama çıkar, sonraki Meteor/Frozen Orb mana maliyeti sıfır.
- **Mirror Image (3 kopya):** Kopyalar ölünce aktif element tipinde ölüm patlaması yapar — Fire'deysen yanma AoE, Frost'taysan freeze nova.

**Build Yönleri:**
- **Fire Burst:** Combustion + Fireball + Living Bomb + Meteor
- **Frost Lock:** Glacial Spike + Blizzard + Frozen Orb + Meteor
- **Arcane Storm:** Arcane Surge + Chain Lightning + Arcane Blast + Blink

**Playtest Notu:** Element Switch'in aktif state'i UI'da net gösterilmesi kritik — oyuncu hangi elementte olduğunu kaçırırsa ritim çöker.

---

## 🗡️ 3. SHADOWBLADE

**Kimlik:** Görmüyorsun. Zaten geç.

**Kaynak — Energy (0-100, +15/sn) + Combo Points (0-5)**
Energy pasif dolar, skill kullanımını kısıtlamaz. Asıl kaynak Combo Points — her LMB +1 CP, bazı skilller direkt veriyor. CP 5'e dolunca finisher skilleri güçleniyor (Rupture, Kidney Shot, Toxic Eruption). CP harcanır, sıfırlanır.

**Temel Hareket**
- **LMB — Quick Stab:** 2 hızlı hançer, her hit +1 CP. Stealth'ten kullanılırsa otomatik Backstab bonusu uygulanır — konumlanma gereksiz.
- **RMB — Shadow Slip:** 2 metre anlık kaçış, CD 1s. Hedefe gitmiyor, sadece yön değiştiriyorsun. Konumlanma ve kıl payı kaçış için.

**[V] — SHADOW DANCE**
Energy 100 + CP 5: 8 saniye her saldırı sonrasında kısa stealth girer. Bleed Lord veya Assassin döngüsünü sürdürürken burst penceresi açar.

---

### Nasıl Oynanır?

Shadowblade üç farklı tarzda oynanabilir ama hepsi CP birikmesi → finisher patlatması üzerine kuruludur. Low rotation lock — çok sayıda yolun açık olması sınıfı esnek yapar.

**Döngü 1 — Assassin:**
1. **Vanish** ile stealth gir.
2. **Ambush** ile aç — sadece stealth'ten %300 hasar + 4 CP + slow.
3. 3s+ stealth varsa Cold Blood bonus (+%100 hasar) aktif.
4. **Backstab** ekle — Shadowstep sonrasına denk gelirse +%50 hasar.
5. CP 5'e dolunca **Kidney Shot** (3 CP'den de kullanılabilir, 5'te max etki).

**Döngü 2 — Bleed Lord:**
1. **Hemorrhage** ile bleed DoT başlat — öldürünce 3 komşuya yayılır.
2. **Fan of Knives** ile tüm aktif debuffları çevreyayıl — 360°, bleed herkeste.
3. CP 5'te **Rupture** ile bleed patlatılır, birikmiş hasar anında çıkar.
4. **Toxic Eruption** ile tüm debuffları tüket — her stack başına %150 hasar, yakına yayılır.

**Döngü 3 — Phantom:**
1. **Mirage Blade** ile hareket ederken afterimage bırak.
2. **Shadowstep** ile hedefe ışınlan — afterimage'lar hedefe atılır, %250 hasar + 1s stun.
3. Stun penceresinde CP doldur, **Kidney Shot** ile uzat.

**Önemli Sinerjiler:**
- **Hemorrhage + Rupture:** Bleed aktif hedefe Rupture basarsan birikmiş hasar anında patlar.
- **Evasion + Shadowstep:** Evasion aktifken Shadowstep CD'si sıfırlanır — neredeyse sonsuz konumlanma.
- **Preparation:** Mobilite + stealth CD'leri sıfırlar (Shadowstep, Shadow Slip, Vanish, Evasion). Ofansif finisher CD'lerine dokunmaz — abuse kapalı.
- **Kidney Shot (3-5 CP):** Artık 3 CP'den kullanılabilir. Tam stun süresi için 5 CP iste ama 3 CP'de de kontrol finisher olarak kullanıyorsun.

**Build Yönleri:**
- **Assassin:** Ambush + Vanish + Backstab + Preparation
- **Bleed Lord:** Hemorrhage + Toxic Eruption + Rupture + Fan of Knives
- **Phantom:** Mirage Blade + Shadowstep + Evasion + Kidney Shot

**Playtest Notu:** Mirage Blade Ronin'in afterimage alanına yakın — fark şu: Shadowblade'de afterimage stun+hasar veriyor (aktif tetik), Ronin'de deception/positioning aracı.

---

## 🏹 4. RANGER

**Kimlik:** Sana ulaşamazsın. Her saniye kayıp veriyorsun.

**Kaynak — Focus (0-100)**
4+ metre mesafede +10/sn dolar. 2 metreden yakına düşersen −20/sn hızla boşalır. Focus 75+: tüm hasar +%25. Focus 100: sonraki skill bedava cast. Bu sistem bir "kite ödülü" — uzakta kalırsan güçleniyorsun, yakına girilirse cezalanıyorsun.

**Temel Hareket**
- **LMB — Quick Arrow:** Anında, CD yok, hareket halinde atılabilir. Focus+4. Kite döngüsünün temeli — Tactical Roll kaçışlarının arasında sürekli hasar ve Focus doldurma.
- **RMB — Tactical Roll:** Hareket yönüne kısa geri atlama + aynı anda 1 ok. Focus+10, CD 1.2s. Ne zaman basarsan o yöne atlıyor — sadece geri değil, yandan da kaçabilirsin.

**[V] — RAIN OF ARROWS**
30 saniye sabit CD (Focus değil): 5 saniye tüm arena yağmur. Boss odasında ve kalabalık odalarda en etkili.

---

### Nasıl Oynanır?

Ranger **uzak mesafe kite** sınıfıdır. Asla durup "sniper" gibi oynamak zorunda değilsin — hareket halinde atabiliyorsun. Asıl güç mesafe ve tuzak kontrolünden gelir.

**Temel Döngü (Sniper):**
1. 4+ metre mesafede **Quick Arrow** ile Focus doldur.
2. Focus 75'te hasar bonusu başlar, **Aimed Shot** (hold 1s → %250 + guaranteed crit) ile büyük hasar.
3. **Tethering Arrow** ile hedefe zincir — 8+ birim uzağa çıkarsan %300 kritik kanama + Focus+50.
4. Focus 100'de **Rapid Fire** (8 hızlı ok, maliyet yok).
5. Yaklaşırlarsa **Disengage** ile 6 metre geri atla.

**Tuzak Döngüsü:**
1. **Explosive Trap** ile oda girişine tuzak kur.
2. Düşmanları **Concussive Arrow** knockback ile tuzağa yönlendir.
3. **Barbed Net Shot** ile root — **Disengage sonrasında ağ 4m alana yayılır**.
4. Root altında **Aimed Shot** — hasar ×2.
5. **Volley** ile alan kapat.

**Önemli Sinerjiler:**
- **Disengage + Aimed Shot:** Atlama sırasında instant ateş bonusu.
- **Point Blank (Master):** 2 metre altına girilirse Focus × %3 hasar + knockback. Kite sınıfı olduğun için bu kurtarma butonu — hasar vermek için yaklaşma, ama yaklaşılırsa panikmeden kullan.
- **Flare (Focus+40):** Utility olmanın ötesine geçti — neredeyse bir hasar rotasyonu adımı.
- **Multi-Shot:** 5+ düşmana isabet edince tüm CD -3s — kalabalık odada rotasyon çok hızlanır.

**Build Yönleri:**
- **Sniper (Boss):** Aimed Shot + Tethering Arrow + Rapid Fire + Flare
- **Trap Master (Oda Kontrolü):** Explosive Trap + Volley + Barbed Net Shot + Disengage
- **Kite Lord:** Concussive Arrow + Black Arrow + Multi-Shot + Point Blank

**Playtest Notu:** Point Blank execute ekleme — Breacher/Gunslinger alanına kayar. Knockback + hasar yeterli.

---

## 👊 5. RAVAGER

**Kimlik:** Az canken daha tehlikeliyim. Bu hata değil, strateji.

**Kaynak — Fury (0-100)**
Sadece **hasar alarak** dolar — +15/vuruş alındığında. HP düştükçe Fury daha hızlı dolar. Hasar vermek Fury üretmez (Warblade'den farkı). Bu "kayıp vererek güçlen" mekaniği sınıfın özü.

**Temel Hareket**
- **LMB — Brutal Swing:** Geniş yay balta salınımı, 1-3 düşmana çarpabilir. Son 1 saniyede hasar aldıysan Fury+20 (normalde +12). Ağır görünür ama input snappy.
- **RMB — Battle Cry:** 1.5 metre çevreyakındaki düşmanları sana yönlendirir + 0.4s kısa zırh. Fury+15. CD 2s. Hem Fury doldurma hem düşman toplama.

**[V] — BERSERK MODE**
Fury 100: 15 saniye savunma yoksay + %150 hasar. Her kill en uzun CD'li skill'i sıfırlar. Bir kere girdin mi zincirleme kill döngüsü başlar.

---

### Nasıl Oynanır?

Ravager düşmanların seni vurmasını **istiyor**. Hayatta kalmak için kaçmak yerine düşman içine dalmak sınıfın mantığı. Sıfır hasar almak Fury üretmez — kötü oynanış.

**Temel Döngü:**
1. **Battle Cry** ile düşmanları topla, Fury doldur.
2. **Bloodlust Strike** ile koni saldırı — HP %30 altındayken +%120 hasar.
3. **Whirlwind** ile spin — her vuruş savunma -%5 (max -%30), Fury dolmaya devam.
4. Fury %80+: **Frenzied Leap** → hit'te CD sıfırlanır → tekrar leap mümkün.
5. Fury 100: **BERSERK** — her kill CD sıfırlar, döngü bitmez.

**Risk Döngüsü (Glass Cannon):**
1. **Reckless Swing** — devasa tek hasar ama 2 saniye tam savunmasız. Risk kabul et.
2. Savunmasızken hasar alırsan: Fury+40 + 0.8s invuln kazanırsın.
3. **Blood-Drunk Leap** ile Fury'nin %30'unu harca → her 10 Fury başına %20 ekstra hasar.
4. HP %40 altındayken lifesteal +2s CC bağışıklığı.

**Önemli Sinerjiler:**
- **Death Wish + Fury:** 5 saniye HP 1'in altına düşemez, Fury ×3 dolar. En tehlikeli anda en güçlü.
- **Undying Tenacity:** Alınan hasar %40 yoksay + HP %20 altına düşemez + her darbe +10 Fury. Death Wish ile bitince biriken Fury'nin %20'si kadar HP yenilenir.
- **Bloodthirst:** HP %20 + Fury %100'de 8 vuruşa çıkar — en kritik anda lifesteal en yüksek.
- **Iron Grab (≤3m):** Yakala, tut, fırlat. Duvara çarpan bir düşmanı başka bir düşmana fırlatırsan ikisi de stun.
- **Intimidating Shout (stagger):** Artık düşman kaçmıyor — yerinde sendeliyor. Melee paketi dağıtma cezası yok.

**Build Yönleri:**
- **Glass Cannon (Yüksek Risk):** Reckless Swing + Bloodlust Strike + Blood-Drunk Leap + Death Wish
- **Fury Engine (Stabil):** Undying Tenacity + Bloodthirst + Whirlwind + Shatter Armor
- **Crowd Crusher (Kalabalık):** Iron Grab + Barbaric Charge + Intimidating Shout + Frenzied Leap

---

## ⚔️ 6. RONİN

**Kimlik:** Çek. Kes. Bitir.

**Kaynak — Draw Tension (0-100)**
Hareket halinde +20/sn dolar. Quickdraw kullanınca +30. 3 saniye hareketsiz kalırsan −30/sn (oda temizse drain yok). Tension 100'de sonraki Quickdraw ×2 hasar. Durmak ceza — sürekli hareket etmek ödül.

**Temel Hareket**
- **LMB — Sheath Walk:** Hareket halinde hafif slash, Tension+5. 3 ardışık vuruş sonunda öne kısa atılım + güçlü son darbe. İframes yok, skill slot tutmuyor — her zaman mevcut.
- **RMB — Drawn Edge:** Basılı tut = Tension birikiyor. Bırak = birikim oranında tek slash. Skill CD'lerine dokunmaz, tüm Tension bonusları uygulanır.

**[V] — MUGEN NO KIRI**
Tension 100: 5 saniye her input anında draw-cut, CD yok, cut anlarında iframes. En güçlü hayatta kalma + burst penceresi.

---

### Nasıl Oynanır?

Ronin bir **hareket canavarı** — durduğunda zayıflıyor, koşarken güçleniyor. Iaido Stance ve Quickdraw etrafında dönen high-ceiling burst combolar var ama temel mantık basit: hareket et, Tension doldur, kes.

**Iaido Döngüsü (Ana Combo):**
1. Hareket ederken Tension doldur.
2. **Iaido Stance** aç (0.8s) — hareketsizken Tension dolar, sonraki Quickdraw guaranteed crit.
3. **Quickdraw Slash** ile çık — Tension 80+: +%100 hasar + hitstop.
4. Çıkışta **Iaido Stance CD sıfırlanır** — tekrar stance'e girebilirsin.
5. Tension tekrar dolunca **Void Cleave** ile bitir — tüm Tension'ı 10m koni olarak boşalt.

**Hareket Döngüsü:**
1. **Wind Step** ile 3 yönde hızlı koş — her adımda slash + Tension+10, 3 yönde tamamlanırsa +40 bonus.
2. **Haste Dash** ile sürgün — **Iai Pressure** aktifse Tension bonusları da uygulanır.
3. **Flash Draw** ile 3 hedefe ışınla-kes.
4. **Crescent Arc** ile daire çizerek geç — 4+ hedefte Tension anında 100.

**Önemli Sinerjiler:**
- **Iaido Stance → Quickdraw → Iaido:** Stance çıkışı CD sıfırlar — loop mümkün ama Tension tükeniyor.
- **Phantom Step (3 afterimage):** Düşmanlar saldırdığında afterimage'a saldırıyor — konumlanma ve hasar engelleme bir arada.
- **Blade Veil + Quickdraw:** Deflect anında Quickdraw +%150 hasar.
- **Counter Draw:** 0.5s pencere, %200 hasar + knockback + Tension+30 — Warblade Iron Counter'dan farkı iframes yok, daha riskli.
- **Void Cleave:** Ekran-wide değil, yönlü 10m koni. Finisher öncesi hedefi gör, yön sıraya al.

**Build Yönleri:**
- **Iaido Burst (Boss):** Quickdraw + Iaido Stance + Tension 100 + Void Cleave
- **Phantom Dance (Kalabalık):** Haste Dash + Wind Step + Phantom Step + Flash Draw
- **Wave Clear:** Mille Feuille Cut + Crescent Arc + Iai Pressure + Wind Step

**Playtest Notu:** Phantom Step Shadowblade afterimage'ına temada benziyor. Fark mekanik: Ronin'de deception/positioning, Shadowblade'de aktif hasar tetikleme.

---

## 🔫 7. GUNSLİNGER

**Kimlik:** Dur, nişan al değil — koş, ateş et, bitir.

**Kaynak — Heat (0-100)**
Her atışta +8 Heat. Heat 100 = Overheat: 3 saniye hasar +%50 + muzzle flash AoE, ardından 2 saniye forced cooldown (Dual Fire, Fan the Hammer, Bullet Rain kilitli). Overheat hem ödül hem ceza — kontrollü tetiklemek büyük bonus, istemeden girmek seni kör bırakır.

**Temel Hareket**
- **LMB — Dual Fire:** İki silahtan eşzamanlı, basılı tut = otomatik ateş. Heat+6/atış. Hareket kesmeden.
- **RMB — Hip Shot:** Yana kısa kayma + aynı anda tek mermi. Heat+10. CD 0.8s. Günslinger'ın imzası — kayarken ateş.

**[V] — FULL METAL STORM**
Heat 100: 5 saniye konum kilidi yok, dual-fire sürer, her ateş AoE muzzle flash. En etkili kalabalık odaları temizleme aracı.

---

### Nasıl Oynanır?

Gunslinger **mobil burst assassin**. Durma, pozisyon al, ateş et — bu yaklaşım sınıfa uymuyor. Her şey hareket halindeyken tetiklenecek şekilde tasarlanmış. Heat yönetimi en kritik beceri.

**Mobile Assassin Döngüsü:**
1. **Rift Dash** ile öne atıl — geçerken AoE %120, Heat 50+: mesafe ×2.
2. Dash sonrasında **Quickdraw** — guaranteed crit + Heat+20.
3. **Critical Shot** ile tek hedef %300.
4. Hedef ≤2m'ye girerse **Point Blank Execute** — %400 hasar + Heat anında 100 → Overheat.
5. Overheat'teyken **Rift Dash + Reload Dance** ile çıkar — Reload Dance tüm CD'leri -%20, Heat −30.

**Heat Engine Döngüsü:**
1. **Fan the Hammer** ile 6 hızlı atış, Heat+40.
2. **Burning Ammo** açık — tüm mermiler ateş DoT.
3. Heat 100'e doğru yaklaşırken **Bullet Rain** aç — Overheat sırasında hasar ×1.5, alan ×2.
4. Overheat bonus bittikten sonra **Reload Dance** — sıfırla, tekrar.

**Önemli Sinerjiler:**
- **Rift Dash → Quickdraw:** Dash hemen sonrasında Quickdraw = guaranteed crit.
- **Dead Eye + Fan the Hammer:** 2 saniye tüm mermiler crit, Heat üretimi +%50 — Overheat çok hızlı.
- **Smoke Grenade (5m):** Artık bağımsız oda kontrolü yapıyor. Kör+yavaş 3 saniye içinde kalabalığa dalmak çok daha güvenli.
- **Ricochet:** 3 farklı hedefe sekerse Heat −30 + CD sıfırlanır — Heat yönetiminin en kolay parçası.
- **Suppression Fire:** Hat boyunca iter — köşeye ya da Smoke alanına yönlendirme.

**Build Yönleri:**
- **Heat Engine:** Fan the Hammer + Burning Ammo + Bullet Rain + Full Metal Storm
- **Mobile Assassin:** Rift Dash + Quickdraw + Critical Shot + Point Blank Execute
- **Crowd Suppressor:** Suppression Fire + Smoke Grenade + Ricochet + Bullet Rain

**Playtest Notu:** Point Blank Execute tam shotgun sınıfına dönüşmesin — shell/reload mekaniği eklenmeden büyütülmemeli.

---

## 👊 8. BRAWLER

**Kimlik:** Durma. Vur. Ritim bul. Tekrar.

**Kaynak — Charge (0-5)**
Her vuruşta +1 Charge. 3 saniye hareketsiz kalırsan sıfırlanır. Her Charge: hasar +%10, hız +%3. 5 Charge = **Charged State**: sonraki skill +%50 güçlenir, Charge sıfırlanır. Sürekli saldırıda kalman hem hasar hem hız demek.

**Temel Hareket**
- **LMB — Jab:** Tek hızlı yumruk, Charge+1. Hızlıca tıklanırsa 4'lü oto-kombo (+1 Charge/hit). Her şey buradan başlar.
- **RMB — Weave:** Kısa yan adım. Gelen saldırı sırasında basarsan Charge+2. Perfect timing (0.2s pencere): Charge+2 + 0.3s iframes. CD yok — ritim oyununun kalbi. İframes'i sadece perfect timing'de alırsın.

**[V] — OVERDRIVE**
5 saniye: her vuruş Charged State gibi davranır, Charge azalmaz. Bütün skillerin anında %50 güçlenmiş — odayı temizlemenin en hızlı penceresi.

---

### Nasıl Oynanır?

Brawler **sürekli saldırı ritmi** üzerine kurulu. Durmak Charge sıfırlar — ceza büyük. High rotation lock var, ama ritmi bulunca sınıf çok tatmin edici oynanır. İki ana yol var: Momentum Strike ile büyük tek vuruş, Unstoppable Force ile sürekli tempo.

**Combo Machine Döngüsü:**
1. **Jab** × 4 ile Charge doldur (veya **Mach Punch** ile +4 Charge).
2. 5 Charge → Charged State. Sonraki skill +%50.
3. **Rush Combo** (5m atılım + 3 vuruş) — son vuruşta Charged State bonus.
4. **Aerial Rave** ile düşmanı havaya kaldır — 3 hava vuruşu, Charge korunur.
5. **Momentum Strike** ile bitir — 5 Charge = %500 tek vuruş.

**Counter Fighter Döngüsü:**
1. **Weave** ile düşman saldırısını oku — perfect timing Charge+2 + iframes.
2. **Counter Blow** ile karşılık ver — %200 hasar + Charge+3.
3. Charged State: **Shockwave Slam** ile AoE.
4. **Repulse** ile itersek → 4+ düşman = anında 5 Charge.
5. Charged State'te **Guard Break** — savunma -%60 + stun 1s.

**Önemli Sinerjiler:**
- **Guard Break (+3 Charge):** Artık ritim planına dahil — generic shred değil, Charge motor.
- **Repulse:** 4+ düşman = Charge 5 — Empower hattını anında açar.
- **Momentum Strike (Empower spender):** Tek büyük darbe yolu — Charged State'te %500.
- **Unstoppable Force (Overdrive spender):** 4s Charge azalmaz, hız +%50, her dash = Rush Combo — sürekli tempo yolu.
- **Seismic Stomp + Aerial Rave:** Stomp ile havaya kaldır → Aerial Rave havadakilere +%100 hasar.

**Build Yönleri:**
- **Combo Machine:** Mach Punch + Rush Combo + Aerial Rave + Momentum Strike
- **Ground Breaker:** Shockwave Slam + Seismic Stomp + Guard Break + Overdrive
- **Counter Fighter:** Counter Blow + Repulse + Cyclone Drive + Unstoppable Force

**Playtest Notu (Faz 2 izle):** Empower (Momentum Strike) ve Overdrive (Unstoppable Force) yolları hâlâ aynı çıktıya yakınsıyor — "neden ikisini birden almayayım?" sorusunu yanıtla.

---

## 💀 9. SUMMONER

**Kimlik:** Ben savaşmıyorum. Feda ediyorum. Ve feda anı en güçlü andır.

**Kaynak — Charges (0-4)**
Her 8 saniyede otomatik +1 Charge. Bir minyon ölünce anında +1 Charge. Minyonları çağırmak Charge harcar, minyonları feda etmek Charge iade eder. Ekonomiyi yönetmek sınıfın özü.

**Temel Hareket**
- **LMB — Command Strike:** Minyon varsa tüm minyonları imlece yönlendirir + kısa staff darbesi. Minyon yoksa kendi saldırısı (%80 hasar). Asla boşta kalmaz.
- **RMB — Soul Dart:** Kısa menzilli ruh mermisi, +0.5 Charge, CD 1s. Minyon yokken hayatta kalma — minyon varken hedefi işaretler, minyonlar öncelik verir.

**[V] — ARMY OF THE DEAD**
Tüm 4 Charge doluyken: 6 saniye tüm minyonlar +%150 hasar ve ölümsüz. Bu pencerede feda etme — yaşasınlar.

---

### Nasıl Oynanır?

Summoner **oda yönetimi ve ekonomi** sınıfı. Minyonları "koru ve büyüt" değil — minyonları **stratejik olarak feda et** ve her fedadan maksimum değer çıkar. Army Commander ve Sacrifice Engine iki farklı zihin yapısı gerektirir.

**Sacrifice Engine Döngüsü:**
1. **Raise Skeleton** × 3 çağır — 3 iskelet.
2. **Rally Cry** — tüm minyonlar +%20 hasar+hız 10s.
3. Düşmanlar aralarına girince **Mass Sacrifice** — her minyon konumunda %150 AoE patlama. 3+ minyon = +3 Charge, düşmanlar 3s grounded.
4. Charge dolunca tekrar çağır, döngü sürer.
5. **Blood for Power** ile feda → Charge+1 + tüm CD -%30.

**Army Commander Döngüsü:**
1. **Summon Golem** (2 Charge) — tank, HP %20'de patlama.
2. **Raise Skeleton** × 3 (3 Charge) — saldırı gücü.
3. **Commanding Strike** ile Golem'e emir → Golem hedefi duvara çarpar, stun.
4. **Soul Siphon Totem** aç — 8s 5m çevresindeki düşmanlardan 0.5 Charge/sn emer.
5. [V] ile Army of the Dead — tüm minyonlar invuln, maksimum baskı.

**Önemli Sinerjiler:**
- **Corpse Explosion:** Düşman veya minyon cesedi patlatır — Mass Sacrifice sonrası 3+ cesetle zincir patlama.
- **Bone Shield (5s):** Bir minyonu 5 saniye kalkan yap — hasar absorbe edince Charge+1. Golem ile 2× absorbe.
- **Dark Pact:** HP -%12 → Charge olmadan minyon çağır. 15s internal CD — sonsuz döngü kapalı.
- **Lich Form:** 10s ghostal — fiziksel hasar immune. Lich Form sırasında feda → hasar ×3.
- **Death Nova:** 1 minyonu feda → 8s zehir bulutu. Hexer ile oynarken zehir bulutu Hexer debufflarını yayar.

**Build Yönleri:**
- **Sacrifice Engine:** Blood for Power + Death Nova + Mass Sacrifice + Rally Cry
- **Army Commander:** Raise Skeleton + Summon Golem + Commanding Strike + Soul Siphon Totem
- **Lich Burst:** Lich Form + Dark Pact + Corpse Explosion + Bone Shield

---

## 🔮 10. HEXER

**Kimlik:** Sabır. 10'a gelince sen bitiyorsun.

**Kaynak — Hex Stacks (0-10 / düşman başına)**
Her düşman kendi stack sayısını taşır, 5 saniye hasar almadan decay eder.

**Faz sistemi:**
- 0-3 stack: **Debuff Phase** — temel efektler.
- 4-6 stack: **Pressure Phase** — skill güçleri +%20.
- 7-9 stack: **Overload Phase** — düşman aldığı hasarın +%30'u fazladan alır.
- 10 stack: **HEXBLAST** — %100/stack patlama, kill'de CD sıfırlanır, yakına 2 stack yayılır.

**Temel Hareket**
- **LMB — Hex Bolt:** Hızlı mermi, isabette 1 stack. Overload (7+) hedefte 2 stack verir. Ana stack biriktirme aracı.
- **RMB — Curse Grasp:** 2.5m el uzantısı, anında 2 stack. Pressure Phase (4+) hedefte 0.5s root da uygular.

**[V] — HEX CASCADE**
Bir hedefte 10 stack → tüm düşmanlara 3 stack kopyalanır. Kalabalık odada ilk hedefe Hexblast'tan sonra tetikle — oda geneli anında Pressure Phase'e giriyor.

---

### Nasıl Oynanır?

Hexer **sabır ve patlama** sınıfı. İlk 5-7 saniyede stack biriktirir, sonra patlarsın. High rotation lock var — doğru sıralamayla oynamak önemli. Ama Pandemic ve Mass Hex gibi safety valve'lar var.

**Patient DoT Döngüsü:**
1. **Corruption** ile 3 stack + DoT başlat.
2. **Agony** ile süregelen DoT ekle — durdurulamaz, 2 stack/tick (Pressure Phase'de 3).
3. **Empathy** ile lanet at — saldırıların %30'u kendine döner, her refleksiyon +1 stack.
4. Pressure Phase'e girilince **Hex Overload** aç — 6s her hasar +1 stack.
5. Stack 10'a gelince **Hexblast**.
6. Kill'de CD sıfır → yakına 2 stack yayıldı → döngü devam.

**Hex Overload Döngüsü (Hızlı):**
1. **Corruption** ile stack başlat.
2. **Hex Overload** — Corruption sonrasında 10s'ye çıkar.
3. **Mass Hex** ile tüm düşmanlara 2 stack (HP -%8, Pressure'dakilere 3).
4. **Pandemic** ile tek hedefteki tüm stack'ları yaya — Overload Phase hedefte +3 kopya.
5. **Hexblast** zinciri.

**Önemli Sinerjiler:**
- **Pandemic:** Tek hedefteki tüm stack'ları yakın düşmanlara kopyalar. Overload Phase hedefte kopyalanan stack +3 — oda geneli temizliğin anahtarı.
- **Haunt:** Hayalet bağla, takip+tick+3 stack — 10'a ulaşınca otomatik Hexblast.
- **Unstable Affliction:** Dispel/heal edilirse patlama + stun — iyileşen boss'a karşı kritik.
- **Enervate:** Hız -%50, saldırı -%40, 10s. 5+ stack'te süre ×2 — oda kontrolü.
- **Cursed Mirror (%100):** Sana uygulanan her debuff düşmana tam güçle yansır. Artık niş pick değil — her oynayışta değerli.
- **Soul Bargain:** HP -%20 → 5 stack anında. Hedef Pressure Phase'deyse Overload'a iter, Hexblast tetiklenmez — kontrollü kullan.

**Build Yönleri:**
- **Patient DoT:** Corruption + Agony + Pandemic + Hexblast
- **Hex Overload:** Hex Overload + Empathy + Mass Hex + Hexblast
- **Soul Burst:** Soul Bargain + Haunt + Unstable Affliction + Hex Overload

**Playtest Notu (Faz 2 izle):** Rotation lock HIGH — Corruption + Hex Overload + Hexblast lineer döngüsü kırılıyor mu? Pandemic ve Mass Hex bridge olarak yeterli mi?

---

## Karşılaştırma Özeti

| Sınıf | Zorluk | Tempo | Öğrenme Eğrisi | Faz |
|---|---|---|---|---|
| Warblade | Kolay | Yüksek | Düşük | Faz 1 |
| Elementalist | Orta | Orta | Orta | Faz 2 |
| Shadowblade | Orta | Yüksek | Orta | Faz 2 |
| Ranger | Orta | Orta | Orta | Faz 2 |
| Ravager | Kolay | Yüksek | Düşük-Orta | Faz 2 |
| Ronin | Zor | Çok Yüksek | Yüksek | Faz 3 |
| Gunslinger | Orta | Çok Yüksek | Orta-Yüksek | Faz 3 |
| Brawler | Orta | Yüksek | Orta | Faz 3 |
| Summoner | Zor | Düşük | Yüksek | Faz 4 |
| Hexer | Zor | Düşük | Yüksek | Faz 4 |

---

*Kaynak: SINIF_VE_SKILL_KARAR_BELGESI.md v3 (2026-04-14)*
*Audit kaynağı: RIMA_full_skill_audit_claude_ready.md*
