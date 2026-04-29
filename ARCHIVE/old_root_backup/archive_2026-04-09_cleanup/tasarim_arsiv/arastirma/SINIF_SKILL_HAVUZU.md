# SINIF SKILL HAVUZU — Detaylı Tasarım
*2026-03-25 | Her sınıf: 8 aktif havuz + 4 pasif havuz + 2 ultimate havuz*
*Run başında: 4 aktif seç + 2 pasif aktif + 1 ultimate seç*

> **Felsefe:** Her skill 2-4 farklı MMORPG'nin en iyi parçasını birleştiriyor.
> Formatı oku: **Harmanlama** satırı skills'in nereden geldiğini gösteriyor.
> **Tek kaynak = tek boyutlu. Harmanlama = özgün.**

---

## SİSTEM NOTU — Neden 8 Havuz?

GW1 modeli: Her sınıfın büyük bir skill listesi var, run/build başında 4'ünü seçiyorsun.
Bu "seçim" zaten bir meta-oyun:
- Mage oynarken: Ice mi yoksa Fire mi? Her ikisi de güçlü ama farklı oynanış.
- Warrior oynarken: AoE Whirlwind mi, yoksa single-target burst mu?
- Dual class oluşturunca: iki havuzdan 4 seçiyorsun = gerçek build crafting

**Proc koşulları sistemi:**
Her skill'in isteğe bağlı bir **Zincirleme** notu var — önceki belirli bir skill atıldıysa bonus efekt tetiklenir. Bu zorunlu değil, ama keşfetmek oyunun derinliğini açıyor.

---

## WARBLADE — "Savaş Meydanının Efendisi"

**Core Fantasy:** Duruyorum. Savaşı ben tanımlıyorum.
**Kaynak Sistemi:** Rage — hasar verince +5/vuruş, hasar alınca +10. Max 100. Yavaşça düşüyor (boşta -5/sn).
**Oynanış Aralığı:** Yakın mesafe zorunlu — ama dövüşün ritmini sen kuruyorsun.

---

### AKTİF HAVUZ (8 skill — 4'ünü seç)

---

**[1] CHARGE**
Hedefe 8m hızla koş, çarparsan 1.5s stun. Rage +20.
*Zincirleme:* Stun'daki düşmana ilk saldırı → +80% hasar.
> Harmanlama: **WoW Charge** gap-close + stun mekanik / **KO Rush** momentum hissi / **L2 Tackle** yakın dövüş başlatıcı
> Neden: Her dövüşün başında "benim saham" anını tanımlar. Charge yoksa Warrior pasif hisseder.

---

**[2] MORTAL STRIKE**
Tek hedef, yüksek hasar. Hedefin 6s boyunca alacağı iyileşmeyi %50 azaltır.
*Zincirleme:* Charge ardından kullanılırsa → iyileşme azaltma %100'e çıkar.
> Harmanlama: **WoW Mortal Strike** — PvP ikonası, healer'ı etkisizleştirme / **BDO Power Strike** — animasyon ağırlığı, tek büyük vuruş / **KO Stab** — momentum finisher hissi
> Neden: Düşmanın kendi gücünü yok eder. "Artık iyileşemezsin" — psikolojik olarak ezici.

---

**[3] COLOSSUS SMASH**
Yer yarma saldırısı. 6s boyunca hedefe vurulan hasar +30% (tüm kaynaklar). Rage +15.
*Zincirleme:* Dual class: başka sınıfın yüksek hasarlı skill'iyle kombola → window içinde kullanmak zorunda.
> Harmanlama: **WoW Colossus Smash** — armor ignore window, burst setup / **BDO Ground Smash** — geniş alan, zemini parçalama / **D2 Battle Cry** — debuff + ekip çarpanı
> Neden: Tek başına ortalama. Doğru zamanda kullanılınca diğer her şeyi amplify ediyor.

---

**[4] WHIRLWIND**
2s boyunca dönerek etraftaki tüm düşmanlara sürekli hasar. Hareket edebilirsin ama CD yokken birikmiş Rage harcıyor (25/sn).
*Zincirleme:* Whirlwind sırasında Rage biter → otomatik durdurulur, yerine Cleave açılır.
> Harmanlama: **WoW Fury Whirlwind** — sürekli AoE kanalı / **D2 Barbarian Whirlwind** — hareket ederken spin, iconic / **TERA Berserker Cyclone Slash** — görsel ağırlık
> Neden: Çevrelenince paniklemek yerine "hepsine gir" hissi. Risk yüksek ama tatmin maksimum.

---

**[5] SHIELD SLAM**
Kalkanla vur, %100 guaranteed knockback 3m. Hedef duvarla temas ederse ekstra stun 1.5s. Rage spend: -20.
*Zincirleme:* Ground Stomp'tan hemen sonra → Shield Slam CD yarıya düşer.
> Harmanlama: **WoW Shield Slam** (Prot) — proc knockback, interrupt / **KO Ground Hammer** — zemin çarpması, ağır darbe / **TERA Lancer Debilitate** — pozisyon zorlaması
> Neden: Düşmanı sen istediğin yere koyuyorsun. Duvar koyma = arena tasarımı önemli hale geliyor.

---

**[6] EXECUTE**
Sadece HP'si %30'un altındaki düşmana kullanılabilir. %400 hasar. Rage tamamen boşaltır.
*Zincirleme:* Mortal Strike aktifken (hedef iyileşemiyor) kullanılırsa → %600 hasar.
> Harmanlama: **WoW Execute** — ikonik "şimdi öldürülebilir" penceresi / **D2 Berserk** — savunma ignore + büyük hasar / **KO Finish Attack** — HP eşiği finisher
> Neden: Her Warrior dövüşü bir "Execute'a götürme" süreci haline geliyor. Öldürme anı belirli ve dramatik.

---

**[7] HAMSTRING**
Düşmanı yavaşlatır (%50 hız azaltma 8s) ve küçük bleed ekler (3s DoT).
*Zincirleme:* Hamstring'li hedefe Charge → Charge stun süresi 3s'e çıkar.
> Harmanlama: **WoW Hamstring** — PvP kite önleme, klasik / **L2 Mortal Blow** — movement debuff + bleed kombinasyonu / **BDO Knee Break** — bacak vurma animasyonu, gerçekçilik
> Neden: Kaçmaya çalışan düşmana karşı "nereye gideceksin?" anı.

---

**[8] WAR STOMP**
Ayağı yere vur: 3m çevresindeki tüm düşmanlar 2s knockup. Rage +25.
*Zincirleme:* Whirlwind sırasında aktive edilirse → Whirlwind süresi +1s uzar.
> Harmanlama: **WoW Tauren War Stomp** — ırk yeteneği ama ikonik CC / **BDO Giant Ground Smash** — sismik etki, alan CC / **PoE Ancestral Call** — çoklu hedef anında etkileme
> Neden: Kalabalık odaya girince tek vuruşla her şeyi durdurmak. Nefes alma anı.

---

### SİNERJİ HARİTASI

| Tier | Skill'ler | Ne Yapar |
|------|-----------|----------|
| ⭐ **CORE COMBO** | **MORTAL STRIKE + EXECUTE** | İyileşme engelle → %600 finisher. Her dövüş bu iki skill arasındaki yolculuk |
| 🔗 **ZİNCİR** | **CHARGE**, **COLOSSUS SMASH** | Charge açıcı + Smash tüm hasarı amplify eder — sonraki her skill bunlara bağlı |
| 🔵 **BAĞIMSIZ** | Whirlwind, Shield Slam, Hamstring, War Stomp | Güçlü ama solo çalışır — build bağımsız seçimler |

---

### PASİF HAVUZ (4 pasif — 2'sini seç)

**[P1] BLOODLUST** — Her kill → 4s süre %8 lifesteal. Kill yoksa sönüyor.
> WoW Bloodlust + BDO Warrior kill buff | Ödül: agresif kalmanın nedeni.

**[P2] IRON WILL** — HP %30 altına düşünce → 3s %45 hasar azaltma. 15s cooldown.
> KO Iron Will + TERA Berserker passive | Güvenlik ağı: "tam oradan döndüm" anı.

**[P3] JUGGERNAUT** — Charge sırasında stun/root immune. Ayrıca: hareket ederken gelen hasar %15 azalır.
> WoW PvP yeteneği + GW2 Warrior stance | Tasarım notu: Duran Warrior kırılgan, hareket eden sağlam — hareket et!

**[P4] WRECKING BALL** — CC uyguladıktan sonraki ilk saldırı +100% hasar. 8s cooldown.
> WoW Tactician (CS reset) hissi + BDO pre-awakening koma vuruş | Döngü: CC → punch → CC → punch.

---

### ULTIMATE HAVUZ (2 ultimate — 1'ini seç)

**[U1] BLADESTORM** *(60s CD)*
5s boyunca spin: CC immune, her 0.5s'de çevre AoE. Düşmanlar ne yaparsa yapsın durmaz.
> **WoW Bladestorm** — ikonası, tam bir gücün sembolü / **D2 Whirlwind Barb** — süreklilik / **TERA Berserker Onslaught** — görsel momentum
> Moment: Çevrelenince aktive et. 5 saniye boyunca hiçbir şey seni durduramaz.

**[U2] AVATAR OF WAR** *(75s CD)*
10s boyunca: Rage max'ta kilitlenir, her 2s'de zemin çatlama AoE, tüm düşmanlar Warrior'ı hedefler.
> **WoW Warbreaker** — savaş çığlığı + debuff / **D2 Battle Cry** — ekip AoE / **KO War Cry** — tüm düşmanları çekme
> Moment: Takım yoksa bile "ben buradayım, hepinizle ilgileniyorum" anı.

---

---

## ELEMENTALİST — "Sonsuz Ritim"

**Core Fantasy:** Her şeyi yakıyorum. Ama önce ritmi buluyorum.
**Kaynak Sistemi:** Mana (0-100, yavaş regen) + **Elemental State** (Fire veya Frost — sonuncuya göre değişir)
**Mekanik Derinlik:** FFXIV Black Mage'den ilham: Fire spell kullanınca Mana hızlı düşer ama hasar yüksek. Frost spell kullanınca Mana geri gelir ama yavaş. Optimal oyun = ikisi arasında geçiş.

---

### AKTİF HAVUZ (8 skill — 4'ünü seç)

---

**[1] FIREBALL**
Orta hasar, ateş DoT 4s. Her cast → Fire State biriktirir (+1 Fire stack, max 5). 5 stack → bir sonraki Fire spell +50% hasar.
*Zincirleme:* 3 Fireball arka arkaya → 3. Fireball'da Living Bomb efekti ücretsiz tetiklenir.
> Harmanlama: **WoW Fire Mage Fireball** — bread & butter, stack building / **FFXIV Fire III** — Enochian state builder / **GW1 Rodgort's Invocation** — fire DoT spread concept
> Neden: Basit görünüyor ama her cast geleceği şekillendiriyor. "Nereye gidiyorum?"

---

**[2] FROSTBOLT**
Orta hasar, %30 slow 3s. Fire State tüketir (+Mana regen). Eğer hedef zaten yavaşsa → "Brain Freeze" → bir sonraki Frost spell anında (cast time yok).
*Zincirleme:* Fireball DoT aktifken Frostbolt → hedefe "Shatter" uygulanır (+%60 hasar aldırma).
> Harmanlama: **WoW Frost Mage Frostbolt** — CC + proc sistemi / **FFXIV Blizzard III** — Fire/Frost ritim switch / **D2 Blizzard Sorceress** — soğutma + zemin kontrolü
> Neden: Fire'ın karşıtı ama rakibi değil. Doğru anda Frost = hem regen hem CC hem combo tetik.

---

**[3] LIVING BOMB**
Hedef üzerine yerleştir, 5s sonra patlama + çevreye saçılır. Patlama anında canlıysa: 3 komşu düşmana da kopyalanır.
*Zincirleme:* Frostbolt slow'u altındaki hedefe → patlama yarıçapı 2x.
> Harmanlama: **WoW Living Bomb** — ikonik Fire talent, chain explosion / **GW1 Searing Flames** — alevleri yayma mekanik / **D2 Fire Enchant** — gecikmeli ateş release
> Neden: Çok kalabalık odalarda "sabırlı ölüm" kurulumu. 5 saniye bekliyorsun, sonra patlama.

---

**[4] BLINK**
6m anında ışınlanma. Işınlanma anında geçtiğin yerdeki düşmanlara hasar. Sonraki spell %20 ekstra hasar.
*Zincirleme:* Düşmanın tam içinden geçilerek ışınlanılırsa → düşmana brief stun 0.5s.
> Harmanlama: **WoW Mage Blink** — hareket ikonası, juke tool / **FFXIV Aetherial Manipulation** — pozisyon oyunu / **Hades Dash** — hareket = saldırı fırsatı
> Neden: Savunma değil saldırı hamlesi. Blink yaparken hasar veriyorsun — agresif kaçış.

---

**[5] FROZEN ORB**
Yavaş hareket eden enerji küresi fırlatırsın, 5s boyunca yoluna çıkan her şeyi chills. Kendin de yakından geçebilirsin (alan etkisi).
*Zincirleme:* Orb üzerinden Blink → Orb anında patlar, tüm chilled düşmanlar Frozen'a geçer (2s full freeze).
> Harmanlama: **WoW Frozen Orb** — DoT AoE topu, ekranda takip etmek tatminkar / **D2 Frozen Orb Sorceress** — ikonik build definer / **PoE Orb of Storms** — stationery + moving orb combos
> Neden: Strateji gerektiriyor: düşmanları Orb'un yoluna sokman gerekiyor. Arena kontrolü.

---

**[6] ARCANE BLAST**
Her cast bir öncekinden +20% hasar ama +30% mana maliyet. 4. cast sonrası Arcane Barrage tetiklenebilir hale gelir (tek seferlik güçlü atış, stack sıfırlar).
*Zincirleme:* Colossus Smash (Warblade dual class) window'unda Arcane Barrage → damage cap kaldırılır.
> Harmanlama: **WoW Arcane Mage** — escalating stack sistemi, karar noktaları / **FFXIV Enochian** — "bu durumu koru veya kaybet" / **ArcheAge Thunder Strike** — tek büyük final atış
> Neden: Karar oyunu: ne zaman "Barrage" atarsın? Çok erken = waste. Çok geç = mana yok.

---

**[7] METEOR**
1.5s kanal → işaretlenen alana büyük taş düşer, knockdown + büyük hasar. Kanallama sırasında savunmasızsın.
*Zincirleme:* Frozen/slowed hedef üzerine → knockdown 3s'e çıkar + hasar +50%.
> Harmanlama: **WoW Meteor** (Fire talent) — gecikme + büyük impakt / **D2 Meteor Sorceress** — alan engelleme, yerleştirme sanatı / **ArcheAge Meteor Strike** — PvP landmark skill
> Neden: Yavaş ama oyun değiştirici. Doğru anda Meteor = savaş bitti.

---

**[8] MIRROR IMAGE**
2 kopyан oluşturur, 8s hayatta kalırlar. Her kopyası rastgele skill atar. Sen hasar aldığında önce kopya alır.
*Zincirleme:* Kopyalar ölünce → ölüm patlaması (küçük AoE), hasar sana değil düşmanlara gider.
> Harmanlama: **WoW Mirror Image** — hayatta kalma + kaos yaratma / **GW1 Illusion of Weakness** — aldatmaca / **ArcheAge Illusion** — decoy mekanik
> Neden: Düşman artık neyi vuracağını bilmiyor. Panik yaratan skill.

---

### SİNERJİ HARİTASI

| Tier | Skill'ler | Ne Yapar |
|------|-----------|----------|
| ⭐ **CORE COMBO** | **ARCANE BLAST × 4 + ARCANE BARRAGE** | Escalate → tek atışta nuke. Diğer her skill bu rampa için zaman kazandırıyor |
| 🔗 **ZİNCİR** | **FIREBALL**, **FROSTBOLT** | Fire state biriktir → Frost ile Mana regen + Brain Freeze proc. Bu ikisi arasındaki ritim oyunun özü |
| 🔵 **BAĞIMSIZ** | Living Bomb, Blink, Frozen Orb, Meteor, Mirror Image | Güçlü ama build bağımsız seçimler |

---

### PASİF HAVUZ

**[P1] FIRE FOCUS** — Fire spell vurununca → diğer spell CD'leri -2s.
> WoW Kindling + FFXIV Firestarter proc

**[P2] BURNOUT RECOVERY** — Hasar alınca mana +8.
> GW1 enerji yönetimi + PoE Mind Over Matter

**[P3] SHATTER** — Frozen/slowed hedefe spell'ler +60% hasar.
> WoW Shatter (Frost mastery) + D2 Cold Mastery

**[P4] TEMPORAL FLUX** — Sen yavaşlattığın düşmanlar tüm kaynaklardan +25% hasar alır.
> WoW Temporal Displacement + FFXIV Vulnerability Up

---

### ULTIMATE HAVUZ

**[U1] FIRESTORM** *(50s CD)*
6s boyunca tüm alan ateş yağmuruna tutulur. Her saniye dalga. Mana her saniye -15.
> **GW1 Firestorm** + **WoW Pyroblast** empowered + **D2 Meteor+Orb** aynı anda

**[U2] TIME WARP** *(70s CD)*
8s boyunca her şey %60 yavaşlar (sen hariç). Spell'lerinin cast speed'i %50 artar.
> **WoW Heroism/Bloodlust** kavramı + **FFXIV LB3 Caster** control hissi + oyunun "bullet time"ı

---

---

## ROGUE — "Gölgede Ölüm"

**Core Fantasy:** Görmüyorsun. Zaten geç.
**Kaynak Sistemi:** Energy (0-100, hızlı regen, +15/sn) + Combo Points (0-5, skill başına birikir, finisher gerektirir)
**Mekanik Derinlik:** WoW Rogue modeli: builder → finisher döngüsü. Ama her finisher farklı bir şey yapıyor. Ne için biriktirdiğini bilmek zorundasın.

---

### AKTİF HAVUZ

---

**[1] BACKSTAB**
Hedefin arkasında olmayı gerektirir: %200 hasar + 3 Combo. Önden vurursan normal hasar, Combo yok.
*Zincirleme:* Shadowstep'ten hemen sonra (arka pozisyon garantili) → +%50 ekstra hasar bonusu.
> Harmanlama: **WoW Backstab** — pozisyon requirement, klasik risk/ödül / **BDO Dark Knight Shadow Eruption** — animasyon akıcılığı / **L2 Dagger Behind Attack** — arka vuruş kültürü
> Neden: Her saniye nerede durduğun önemli. Pozisyon = hasar denklem.

---

**[2] HEMORRHAGE**
Bleed 8s DoT uygular. 2 Combo üretir. Hedef kan kaybederken ölürse → yakındaki düşmanlara Hemorrhage yayılır.
*Zincirleme:* Bleed aktif düşmana Rupture finisher → hasar %100 artışla açılır.
> Harmanlama: **WoW Hemorrhage** — bleed + debuff applicator / **D2 Poison Nova** yayılma mantığı / **ArcheAge Shadowplay Throw** — multi-target bleed
> Neden: Tek düşmana uygulayıp ölünce yayılması temizleme anını tatminkar kılar.

---

**[3] RUPTURE**
5 Combo harcayarak güçlü bleed + hasar. Combo sayısı ne kadar yüksekse bleed süresi uzar (max 12s).
*Zincirleme:* Hedef zaten bleed altındaysa → Rupture anında tüm birikmiş bleed hasarını patlatır.
> Harmanlama: **WoW Rupture** — finisher-as-DoT / **WoW Eviscerate** — combo point power scaling / **GW1 Assassin Death Blossom** — çoklu etkili final
> Neden: Birden fazla kullanım vakası: hem finisher hem DoT setup.

---

**[4] SHADOWSTEP**
Hedefe anında ışınlan (8m), 0.5s stun. Energy -25.
*Zincirleme:* Evasion aktifken Shadowstep → Shadowstep CD sıfırlanır.
> Harmanlama: **WoW Shadowstep** — PvP tanımlayıcı, gap-closer / **BDO Shadow Chase** — hız ve akıcılık / **L2 Dagger Quick Step** — kısa range teleport, yakın combat
> Neden: Sadece yaklaşma değil — düşmanın saldırı animasyonunu kesiyor (interrupt hissi).

---

**[5] KIDNEY SHOT**
5 Combo harcayarak: 4s stun. Combo sayısına göre süre uzar.
*Zincirleme:* Mortal Strike (Warblade dual class) aktifken Kidney Shot → stun süresinde hedef iyileşemiyor.
> Harmanlama: **WoW Kidney Shot** — PvP'nin en nefret edilen ve en sevileni / **KO Rogue CC Combo** — stun zinciri / **ArcheAge Freerunner** — mobility + CC birleşimi
> Neden: 4 saniyelik stun = oyunun durduğu an. Her şeyi yeniden konumlanmak için kullan.

---

**[6] AMBUSH**
Sadece stealth'ten kullanılabilir. %300 hasar, 4 Combo, düşmanı %20 yavaşlatır.
*Zincirleme:* Stealth 3s'den uzun sürdüyse Ambush → +%100 ekstra hasar ("Cold Blood" benzeri).
> Harmanlama: **WoW Ambush** — stealth opener, tüm Rogue deneyiminin kristali / **GW1 Shadow Prison** — pozisyon kilitleme opener / **BDO Dark Knight Night Crow** — dive giriş animasyonu
> Neden: Rogue'un oyun başında hissettiği şey bu: "Seni görüyorum, sen beni görmüyorsun."

---

**[7] FAN OF KNIVES**
Anında 360° AoE, aktif tüm bleed/zehir/debuffleri tüm düşmanlara uygular. Energy -40.
*Zincirleme:* Hexer dual class: Hexer debuffleri de Fan of Knives ile yayılır.
> Harmanlama: **WoW Fan of Knives** — AoE interrupt + debuff spread / **D2 Blade Fury** — çok hedef spinning blade / **GW1 Assassin multi-hit** — çoklu hedef
> Neden: "Tek uygulamayla herkese servis" — özellikle Hexer/Summoner dual class ile inanılmaz.

---

**[8] EVASION**
4s boyunca %100 dodge. Her dodge → 1 Combo üretir. Kill'de CD sıfırlanır.
*Zincirleme:* Evasion bitince → sonraki saldırı guaranteed crit ("Bunu fark edemezsin").
> Harmanlama: **WoW Evasion** — survival cooldown / **BnS Aerial dodge** — timing oyunu / **TERA Slayer Overpower** — iframe + offensive çıkış
> Neden: Savunma değil, tuzak. Düşman saldırıyor, sen combo biriktiriyorsun.

---

### SİNERJİ HARİTASI

| Tier | Skill'ler | Ne Yapar |
|------|-----------|----------|
| ⭐ **CORE COMBO** | **HEMORRHAGE + RUPTURE** | Bleed uygula → finisher'da patlat (%100 extra hasar). Tek hedefe maksimum hasar döngüsü |
| 🔗 **ZİNCİR** | **AMBUSH**, **EVASION** | Ambush açıcı (4 CP garantili + bonus hasar), Evasion dodge'da CP biriktir + CD sıfırla |
| 🔵 **BAĞIMSIZ** | Backstab, Shadowstep, Kidney Shot, Fan of Knives | Solo güçlü — build'e göre ekle |

---

### PASİF HAVUZ

**[P1] PARRY RIPOSTE** — Perfect dodge timing (0.2s window) → Energy +30 + sonraki saldırı guaranteed crit.
> BnS Counter Frame + GW1 Way of Assassin

**[P2] SHADOW FOCUS** — Stealth'te hız +40%, ilk saldırı Energy maliyeti -%50.
> WoW Shadow Focus talent + BDO Dark Knight stealth state

**[P3] VENOMOUS WOUNDS** — Bleed tick yaptıkça Energy +2/tick regen.
> WoW Assassination passive + PoE DoT momentum

**[P4] MURDEROUS INTENT** — Düşman HP %35 altında → tüm Rogue saldırıları +30% hasar, hareket hızı +20%.
> WoW Find Weakness + D2 Deathblow passive

---

### ULTIMATE HAVUZ

**[U1] SHADOW DANCE** *(50s CD)*
8s: Her saldırıdan sonra otomatik stealth'e girersin. Ambush her 3s'de kullanılabilir. Gerçek anlamıyla kesintisiz burst.
> **WoW Shadow Dance** — PvP dominance window / **BDO Dark Knight full burst rotation** / **ArcheAge Darkrunner** assassination sequence

**[U2] MARKED FOR DEATH** *(45s CD)*
Hedef 12s işaretlenir: tüm saldırılar crit, hedef iyileşemez, çevre düşmanlar işaretliyi koordineli saldırır.
> **WoW Marked for Death** evolved / **GW1 Mark of Pain** — damage amplifier / **D2 Amplify Damage** — tüm hasar kaynağı genişletme

---

---

## RANGER — "Mesafe Hesabı"

**Core Fantasy:** Sana ulaşamazsın. Ve her saniye kayıp veriyorsun.
**Kaynak Sistemi:** Cooldown-only (resource bar yok — tüm güç zamanlama ve pozisyonda)
**Mekanik Derinlik:** GW1 Expertise: tüm CD'leri pasif olarak kısaltır → Expertise olmadan her Ranger zayıf hisseder. Expertise ise build-enabling pasif, mecburi değil.

---

### AKTİF HAVUZ

---

**[1] AIMED SHOT**
1.5s şarj → yüksek tek hedef hasar. Şarj tamam → crit şansı %50. Eğer hedef immobile'sa (root/frozen) → şarj anında ateşle.
*Zincirleme:* Concussive Arrow root ardından → anında Aimed Shot = kısa duraklamada devastate.
> Harmanlama: **WoW Aimed Shot** — şarj + kritik / **BDO Archer Evasive Explosive Arrow** — şarj hareketi / **GW1 Expert's Dexterity** — sürekli hassasiyet
> Neden: Her bir Aimed Shot için düşmanı durdurmayı planlamak gerekiyor. Kurulumlu hasar.

---

**[2] CONCUSSIVE ARROW**
Anında ok: 4m knockback + 2s root.
*Zincirleme:* Backward Dash sırasında kullanılırsa → uzaklık 6m olur + slow 3s eklenir.
> Harmanlama: **WoW Concussive Shot** — kite tool / **ArcheAge Trip Arrow** — knockback + root / **GW1 Crippling Shot** — anti-approach
> Neden: Yaklaşan düşmanlara "Hayır" demek. Tek skill ile mesafeyi yeniden tanımla.

---

**[3] SERPENT STING**
Zehir DoT 10s + "Weakened Armor" debuff (-20% savunma). Her 2s yenilenir (max 30s devam edebilir).
*Zincirleme:* Disengage + Serpent Sting → zehir daha geniş alana serpilir (atlama sırasında).
> Harmanlama: **WoW Serpent Sting** — DoT + armor debuff / **GW1 Poison Arrow** — süregelen zehir / **D2 Poison Javelin Amazon** — alan zehir
> Neden: Sürdürülebilir hasar. "Git, dönerken de canın düşüyor" stratejisi.

---

**[4] EXPLOSIVE TRAP**
Zemine yerleştir (hemen veya önceden). 3s sonra ayağa basan düşmana patlama + 3s slow. Zincir: Birden fazla trap aynı anda varsa → tetikleme zincirleniyor.
*Zincirleme:* Summoner dual class: minyonların üzerine de konulabilir (mobil trap).
> Harmanlama: **WoW Explosive Trap** — zemin kontrol / **GW1 Barbed Trap** — slow + damage / **D2 Fire Trap** — yerleştirme sanatı
> Neden: Odaya girince önce trap kur. Düşman nereye basarsa orası senin dövüş alanın.

---

**[5] MULTI-SHOT**
Delici ok: tüm düşmanlar üzerinden geçer. Her düşmana Serpent Sting stack uygular.
*Zincirleme:* 5+ düşman vurulursa → tüm Ranger CD'leri -3s.
> Harmanlama: **WoW Multi-Shot** — piercing AoE / **D2 Multiple Shot Bowazon** — kalabalık tarama / **GW1 Barrage** — arc AoE
> Neden: Sürü odalarda tek skill manyağa dönüşebiliyor. Risk: düz çizgi gerekiyor.

---

**[6] DISENGAGE**
6m geri atla, altındaki düşmanlara 3m slow alanı bırak. Hava'dayken hasar %30 azalır.
*Zincirleme:* Disengage + anında Aimed Shot → atlama sırasında ateş, ekstra hasar.
> Harmanlama: **WoW Disengage** — geri zıplama, ikonik hunter hamlesi / **BDO backward dash** + attack / **TERA Archer Breakaway** — hızlı retreat
> Neden: Yaklaşan düşmanı geri at, mesafe aç, hemen ateş. Bir hamlede üç şey.

---

**[7] BLACK ARROW**
DoT + özel: Bu DoT ile ölen düşman, öldüğü yerde 8s hayatta kalan bir "ruh" bırakır (Ranger'ın yanında savaşır).
*Zincirleme:* Summoner dual class: ruh, Summoner'ın minyonu olarak sayılır (Blood for Power tetikler).
> Harmanlama: **WoW Black Arrow** — DoT + summoning hybrid / **GW1 Apply Poison** — DoT + side effect / **D2 Shadow Master Trapper** — DoT sonucu summon
> Neden: Öldürdüğün her düşman geçici olarak çalışıyor. Kalabalık oda temizleyince ordu oluyor.

---

**[8] VOLLEY**
Belirlediğin 4m'lik alana 3s boyunca ok yağmuru. Her saniye hasar + slow. Sen hareket edebilirsin.
*Zincirleme:* Explosive Trap üzerine Volley → patlama slow + Volley slow = tam kilitlenme.
> Harmanlama: **WoW Volley** — zemin AoE / **GW1 Barrage** — sürekli ok / **ArcheAge Multiple Arrow** — alan yağmur
> Neden: Kapı önüne koy, düşmanların gelmesini bekle. "Burası güvenli değil" alanı.

---

### SİNERJİ HARİTASI

| Tier | Skill'ler | Ne Yapar |
|------|-----------|----------|
| ⭐ **CORE COMBO** | **CONCUSSIVE ARROW + AIMED SHOT** | Root → anında şarj atış. Her immobilize fırsatı tek vuruşta hasar ×2 oluyor |
| 🔗 **ZİNCİR** | **SERPENT STING + MULTI-SHOT**, **EXPLOSIVE TRAP + VOLLEY** | DoT uygula + AoE spread / Zemin slow katmanla + ok yağmuru = tam kilitlenme |
| 🔵 **BAĞIMSIZ** | Disengage, Black Arrow | Situasyonel ama oyun değiştirici |

---

### PASİF HAVUZ

**[P1] EXPERTISE** — Tüm CD'ler -%15. Skill maliyetleri -%20. *(Build-enabling pasif — GW1 Ranger DNA)*
> GW1 Expertise attribute — Ranger'ı Ranger yapan şey bu

**[P2] DEAD ZONE MASTERY** — Düşman 2m'ye girince otomatik Disengage tetiklenir. 12s cooldown.
> WoW Hunter kite + BDO auto-dash

**[P3] MARKED TARGET** — Her ok vuruşu "Marked" stack ekler. 5 stack → +30% tüm hasar alır.
> WoW Hunter's Mark + Lost Ark Tripod sisteminin hissi

**[P4] PREDATOR'S FOCUS** — 2s hareketsiz kaldıktan sonra: hasar +20%, crit +15%.
> WoW Steady Focus + D2 Strafe Amazon anchor

---

### ULTIMATE HAVUZ

**[U1] RAIN OF ARROWS** *(55s CD)*
5s tüm arena yağmur. Slow + tick hasar. Düşman nereye kaçarsa kaçsın.
> **GW1 Rain of Arrows** + **WoW Volley** empowered + **D2 Strafe** area coverage

**[U2] CALL OF THE WILD** *(60s CD)*
10s için 3 hayalet hayvan çağır. Bağımsız takip ederler, her biri farklı saldırır.
> **WoW Beast Mastery** + **GW1 Charm Animal** + **BDO Ranger** wolf companion

---

---

## BRAWLER — "Ölüm Eşiği"

**Core Fantasy:** Az canken daha tehlikeliyim. Bu bir hata değil, strateji.
**Kaynak Sistemi:** Fury — sadece hasar ALARAK dolar (+15/hasar). Vermek doldurmaz. HP düştükçe Fury daha hızlı dolar.
**Tasarım Felsefesi:** D2 Berserk + WoW Fury + GW2 Berserker. Risk kabul etmek = güç.

---

### AKTİF HAVUZ

---

**[1] BLOODLUST STRIKE**
Koni saldırı. HP düştükçe hasar artar: %100 HP = baz hasar, %30 HP = +%120 hasar.
*Zincirleme:* Fury %80+ → Bloodlust Strike sonrası Slaughter anında açılır (CD yok).
> Harmanlama: **WoW Enrage** — HP eşiği gücü / **D2 Berserk** — hasarı büyütme / **PoE Berserker "Pain Reaver"** — HP = power kaynak
> Neden: Oyuncuyu kasıtlı olarak zayıf konumda kalmaya teşvik ediyor. Tuhaf ama tatminkar.

---

**[2] WHIRLWIND**
2s spin AoE. AMA: her düşman vuruşu Berserker'ın savunmasını -5% düşürür (max -30%, 4s sonra normale döner).
*Zincirleme:* Savunma -30%'da Whirlwind → Fury bonus +20/spin.
> Harmanlama: **WoW Fury Whirlwind** + **D2 Barbarian WW** — momentum oyunu / **GW2 Berserker Whirlwind Strike** — savunmasız ama güçlü
> Neden: Spin yapmak eğlenceli ama her saniye daha kırılgan oluyorsun. Çıkış zamanlaması kritik.

---

**[3] FRENZIED LEAP**
Hedefe atla, iniş AoE. Eğer düşmana inersen: CD anında sıfırlanır.
*Zincirleme:* Arka arkaya 3 Frenzied Leap (3 farklı düşmana) → 3. sonrası "Frenzy" buff 5s: hasar +50%.
> Harmanlama: **PoE Leap Slam** — zemine çarpma, momentum / **D2 Jump Barbarian** — pozisyon atlama / **TERA Berserker Cyclone Slash** — giriş + çıkış combo
> Neden: Kalabalık alanda kurbağa gibi zıpla ve herkesi ez. Her iniş yeni bir fırsat.

---

**[4] RECKLESS SWING**
Devasa tek hasar — AMA 2s boyunca savunman %100 azalır (tam savunmasız). *Açık pencerede hasar alırsan:* Fury +40 + 0.8s invuln counter tetiklenir (vurulursun ama karşı koyarsın — savunmasızlık bait mekanizmasına dönüşüyor).
*Zincirleme:* Iron Will (Warblade dual class) aktifken → savunmasızlık süresi 0.5s'e iner.
> Harmanlama: **D2 Berserk skill** — tam savunma ignore, tam hasar / **GW2 Full Counter** — risk alarak maksimum hasar / **WoW Recklessness** — trade: savunma için saldırı
> Neden: En yüksek tek vuruş hasarı — ama oyuncu 2 saniye boyunca "tanrıya dua ediyor."

---

**[5] BLOODTHIRST**
Hızlı 5 vuruş. Her vuruş küçük iyileşme. HP düşükse iyileşme miktarı artar.
*Zincirleme:* HP %20 altında + Fury %100 → Bloodthirst 8 vuruşa yükselir.
> Harmanlama: **WoW Bloodthirst** — çok vuruş lifesteal / **BDO Berserker 100 Blows** — combo sayısı / **TERA Berserker Flatten** — hızlı art arda
> Neden: Az canken kendi kendini iyileştirmenin yolu. Savunma değil, saldırı ile hayatta kal.

---

**[6] INTIMIDATING SHOUT**
3m çevresindeki düşmanlar 3s panik: kaçarlar, saldıramazlar.
*Zincirleme:* Panikleyen düşmanlar üzerine Bloodlust Strike → hasar +100% (sırttan vuruluyor sayılır).
> Harmanlama: **WoW Intimidating Shout** — fear + reposition / **KO War Shout** — toplu CC / **TERA Berserker Axe Block** — alan yönetimi
> Neden: Kalabalık oda açıcısı. "Hepsini dağıt, tek tek çöz."

---

**[7] BARBARIC CHARGE**
Düz çizgide dalarken karşına çıkan her şeyi iter. Seni durduramazlar (stun/root immune).
*Zincirleme:* Charge sonu duvara vurursan: tüm itilen düşmanlar duvara sıkışırsa stun 2s.
> Harmanlama: **WoW Heroic Charge** + **GW2 Headbutt** — interrupt + charge / **D2 Charge Barbarian** — çizgi momentum
> Neden: Koridoru kesen düşmanları yarıp geçmek. Momentum = güç.

---

**[8] LAST RITES**
HP %15'in altında → aktive et: Anında %600 hasar. Sonra 4s full savunmasız kalırsın.
*Zincirleme:* Eğer bu hasar düşmanı öldürürse → savunmasızlık yarıya iner (2s).
> Harmanlama: **WoW Last Stand** inversed — son an güç patlaması / **D2 Deathblow** — öldürücü final vuruş / **PoE "Rite of Ruin"** — hayat ile oynama
> Neden: Gerçek "ya şimdi ya hiç" tasarımı. Çok riskli ama alternatif zaten ölmek.

---

### SİNERJİ HARİTASI

| Tier | Skill'ler | Ne Yapar |
|------|-----------|----------|
| ⭐ **CORE COMBO** | **BLOODLUST STRIKE + LAST RITES** | Düşük HP → Bloodlust max hasar açılıyor → Last Rites %600 finale. Risk aldığın için en büyük ödül |
| 🔗 **ZİNCİR** | **FRENZIED LEAP**, **BLOODTHIRST** | 3 ardı ardına Leap → Frenzy buff; Bloodthirst düşük HP'de hem hasar hem lifesteal döngüsü kuruyor |
| 🔵 **BAĞIMSIZ** | Whirlwind, Reckless Swing, Intimidating Shout, Barbaric Charge | Solo güçlü seçimler |

---

### PASİF HAVUZ

**[P1] UNYIELDING** — HP %70+ → alınan hasarın %20'si Fury'e dönüşür.
> PoE Berserker node + WoW Unending Rage

**[P2] RUTHLESS** — HP %30- → atk hızı +15%, lifesteal +5%.
> WoW Execute window hissi + D2 Fanaticism Aura effect

**[P3] WAR MACHINE** — Kill anında Fury +40 + 0.3s invuln.
> GW2 Berserker trait + WoW Fury Rampage

**[P4] PAIN IS POWER** — Her %10 HP kaybı → +5% toplam hasar (stacks, max %50 at 1 HP).
> PoE Blood Magic feel + WoW Furious Charge + TERA Berserker Growing Fury

---

### ULTIMATE HAVUZ

**[U1] BERSERK MODE** *(60s CD)*
15s: defense ignore, +200% hasar, tüm skill'ler sıfırlanır, Fury tükenene kadar devam eder.
> **D2 Berserk** + **WoW Enrage** + **GW2 Berserk Mode** + **PoE Berserker Ascendancy**

**[U2] WARCHIEF'S WRATH** *(65s CD)*
8s: her şey %50 yavaş, her vuruş Fury +30 üretir, ekran gri tonlara döner.
> **GW2 Full Counter** empowered + **D2 Concentrate** — slow-motion battle focus

---

---

## PALADIN — "Kutsal Ritim"

**Core Fantasy:** Hem kesilemiyorum hem öldürüyorum. Bu çelişki değil, tasarım.
**Kaynak Sistemi:** Holy Power (0-100, builder skill'lerle dolar, spender skill'lerle boşaltılır)
**Mekanik Derinlik:** FFXIV Paladin'in combo zinciri: Fast Blade → Riot Blade → Royal Authority. Her adım bir sonrakini güçlendiriyor. Bizde de builder sırası önemli.

---

### AKTİF HAVUZ

---

**[1] CRUSADER STRIKE**
Temel melee + Holy Power +25.
*Zincirleme:* Crusader Strike → Starfire Strike → Crusader Strike: Bu zincir devam ederse her 3. Crusader Strike'ta hasar +60%.
> Harmanlama: **WoW Crusader Strike** — Holy Power engine / **FFXIV Fast Blade combo başlangıcı** / **GW1 Smite** — kutsal dokunuş
> Neden: Metronom gibi. Her 5 saniyede bir basıyorsun, her seferinde bir adım ilerliyorsun.

---

**[2] DIVINE STORM**
360° melee spin, etraftaki herkese hasar + Holy Power +15/hedef.
*Zincirleme:* 3+ düşman vurulursa → Holy Power +50 (düz +15 × 3 yerine).
> Harmanlama: **WoW Divine Storm** — AoE melee rotasyon / **FFXIV Holy Spirit** hissi / **D2 Holy Bolt** — kutsal enerji yayılımı
> Neden: Kalabalık oda builder'ı. Tek düşmana karşı zayıf, kalabalığa karşı Holy Power motororu.

---

**[3] JUDGMENT**
Ranged holy blast (6m). Düşman debuffluysa +50% hasar. Holy Power +20.
*Zincirleme:* Hexer dual class: düşmanda Hexer debuff varsa → Judgment %100 ekstra hasar.
> Harmanlama: **WoW Judgment** — tek atış güçlendirici / **FFXIV Requiescat** — hasar + kaynak hazırlık / **GW1 Zealot's Fire** — koşullu patlamalar
> Neden: Çift rolü var: hem Holy Power builder hem setup tamamlayıcı.

---

**[4] CONSECRATION**
Ayağının altına 5s kutsal zemin yay. Düşmanlara tick hasar + Paladin'e Holy Power +5/sn. Sen hareket edebilirsin.
*Zincirleme:* Consecration + Battle Cry (Warblade dual class) → tüm çekilen düşmanlar kutsal zeminde mahsur.
> Harmanlama: **WoW Consecration** — pasif alan kontrolü / **D2 Holy Fire Aura** zemin versiyonu / **GW1 Balthazar's Spirit** — savaşırken kaynak kazanımı
> Neden: Dur ve savaş: yerleşik Paladin fantezisi. Bırakıp gitme.

---

**[5] HAMMER OF WRATH**
Sadece HP %20 altındaki düşmana kullanılabilir. Büyük hasar. Holy Power +30.
*Zincirleme:* Execute (Warblade dual class) + Hammer of Wrath: İkisi de HP eşiğine dayalı, ardarda kullan.
> Harmanlama: **WoW Hammer of Wrath** — execute window kutsal versiyonu / **D2 Fist of Heavens** — göksel çekiç / **GW1 Symbol of Wrath** — HP eşiği tetik
> Neden: "Artık sona geldin" anını farklı hissettiriyor. Execute değil, yargılama.

---

**[6] AVENGER'S SHIELD**
Kalkan fırlat: 3 düşmana sekip hasar verir, hepini siler (1s interrupt). Holy Power +15/düşman.
*Zincirleme:* 3 farklı düşmana sekerse → her düşman 2s slow alır.
> Harmanlama: **WoW Avenger's Shield** — Protection ikonası / **GW1 Shield Bash chain** / **FFXIV Shield Lob** — ranged interrupt
> Neden: Uzaktan interrupt + Holy Power farm. Yakın dövüşe gelmeden bile Paladin hazırlanıyor.

---

**[7] HOLY SHOCK**
Anında: Hedef düşmansa hasar + Holy Power +15. Hedef sen veya dostsan: HP iyileştirme.
*Zincirleme:* HP %30 altındayken kendi üzerine Holy Shock → iyileşme 3x.
> Harmanlama: **WoW Holy Shock** — versatile instant / **FFXIV Holy Spirit** tek hedef / **GW1 Reversal of Damage** — durum bazlı etki
> Neden: Tek skill, iki kullanım — nereye bastığın savaşın yönünü belirliyor.

---

**[8] SHIELD OF RETRIBUTION**
3s tam blok, engellenen tüm hasar biriktirilir. Sonra birikim → AoE olarak serbest bırakılır.
*Zincirleme:* Consecration üzerindeyken Shield of Retribution → serbest bırakılan AoE kutsal zeminde ikiye katlanır.
> Harmanlama: **WoW Shield of the Righteous** evolved — blok + hasar dönüşümü / **GW1 Reversal of Damage** — hasarı geri verme / **D2 Blessed Shield** — kalkan = silah
> Neden: "Vur bakalım" provokasyonu. Düşman çok vurursa kendi kendini öldürüyor.

---

### SİNERJİ HARİTASI

| Tier | Skill'ler | Ne Yapar |
|------|-----------|----------|
| ⭐ **CORE COMBO** | **CONSECRATION + SHIELD OF RETRIBUTION** | Kutsal zeminde blok patlat → AoE 2× hasar. Yerleşik Paladin fantezisinin özeti: taşınmaz, savaşı kendi alanında tanımlar |
| 🔗 **ZİNCİR** | **CRUSADER STRIKE zinciri**, **AVENGER'S SHIELD** | 3'lü ritim döngüsü Holy Power motororu; Shield uzaktan builder + interrupt |
| 🔵 **BAĞIMSIZ** | Divine Storm, Judgment, Hammer of Wrath, Holy Shock | Her biri güçlü — build'e göre ekle |

---

### PASİF HAVUZ

**[P1] DIVINE FOCUS** — 5m içindeki düşmana saldırı → tüm CD'ler -%10, 5s.
> GW1 Smiting Monk + WoW Sanct Aura

**[P2] HOLY ENDURANCE** — Hasar alınca max HP'nin %10'u 5s içinde geri gelir.
> WoW Paladin sustain kit + FFXIV Paladin survival

**[P3] SANCTIFIED WRATH** — Holy Power %80+ → builder skill'ler +50% Holy Power üretir. Holy Power %100'e ulaşınca → bir sonraki spender skill +40% hasar verir (döngü tamamlama ödülü — ritim kırdığında ceza, tamamladığında ödül).
> WoW Sacred Shield feel + FFXIV Requiescat empowered state

**[P4] LIGHT'S GRACE** — Spender skill kullandıktan sonra → bir sonraki builder skill free cast.
> FFXIV combo action zinciri + WoW Pursuit of Justice

---

### ULTIMATE HAVUZ

**[U1] AVENGING WRATH** *(75s CD)*
10s: %30 invuln + %50 hasar + knockback aura (her 2s etraftakileri iter).
> **WoW Avenging Wrath** + **FFXIV LB3 Paladin** feel + **D2 Fanaticism Aura**

**[U2] BLESSED HAMMER** *(80s CD)*
15s boyunca Paladin etrafında 5 çekiç döner. Her çekiç vuruşu Holy Power +5 + hasar.
> **D2 Hammerdin** — oyun tarihinin ikonası / **GW1 Ray of Judgment** / **WoW Consecration** walking versiyonu

---

---

## SUMMONER — "Lanet Ordusu"

**Core Fantasy:** Ben savaşmıyorum. Kontrol ediyorum.
**Kaynak Sistemi:** Charges (0-4, her 8s +1 otomatik. Minyon öldüğünde +1 anında)
**Mekanik Derinlik:** D2 Necromancer ordusunun taktiksel yönetimi + GW1 Minion Master'ın "feda et, patlat" döngüsü.

---

### AKTİF HAVUZ

---

**[1] RAISE SKELETON**
1 Charge → temel savaşçı iskelet (hızlı, zayıf, agresif). Max 3 aynı anda.
*Zincirleme:* 3 iskelet aynı anda → Rally Cry +40% hasar bonusu (normal +20% yerine).
> Harmanlama: **D2 Raise Skeleton** — Necro'nun omurgası / **WoW Raise Dead** hissi / **GW1 Animate Bone Fiend** — temel ama efektif
> Neden: Başlangıç noktası. Sonraki her şey bununla kombine.

---

**[2] SUMMON GOLEM**
2 Charge → 1 büyük Golem. Yavaş, çok güçlü. Yolu bloke ediyor, hasar alıyor. Max 1.
*Zincirleme:* Golem HP %20'ye düşünce → patlama modu: kendini patlatıp AoE hasar verir.
> Harmanlama: **D2 Clay Golem** — yavaş ama tank / **WoW Felguard** — dominant single pet / **GW1 Bone Horror** — alan yönetimi
> Neden: Golem bir kale. Kapıya koy, arkasından komuta et.

---

**[3] RALLY CRY**
Tüm minyonlar: +20% hasar ve hız, 10s.
*Zincirleme:* Tüm minyon tipleri mevcut (iskelet + golem + başka) → bonus +40%'a çıkar.
> Harmanlama: **GW1 "We Shall Return!"** — çığlık minyonları büyütür / **WoW Demonic Empowerment** / **D2 Battle Orders** — ekip wide buff
> Neden: Zamanlamalı buff: bos öncesi, yoğun dalga öncesi. Hazırlık ve ödül.

---

**[4] CORPSE EXPLOSION**
Düşman veya minyon cesedini patlat → AoE hasar. Ne kadar yakında ölü varsa o kadar büyük patlama.
*Zincirleme:* 3+ cesetle → zincir reaksiyon: patlama patlama tetikliyor.
> Harmanlama: **D2 Corpse Explosion** — oyun tarihinin ikonası / **WoW Unholy DK Death and Decay** konsepti / **GW1 Putrid Explosion** — ölü = kaynak
> Neden: Düşmanı öldürmek sadece başlangıç. Her ölü beden bir silah.

---

**[5] DEATH NOVA**
Seçilen minyonu feda et → 5m çevresine 8s zehir bulutu. Minyon ölüyor ama bıraktığı bulut devam ediyor.
*Zincirleme:* Hexer dual class: zehir bulutu Hexer debufflarını spread ediyor.
> Harmanlama: **GW1 Death Nova** — fedakârlık + zehir / **WoW Unholy Festering Strike** evolved / **D2 Poison Nova** — alan zehir
> Neden: "Ölümün anlamlı olması." Minyon ölürken hâlâ görev yapıyor.

---

**[6] COMMANDING STRIKE**
Belirli minyona emir: o düşmana güçlendirilmiş saldırı (4x hasar), komuta süresince invuln. *Aktif minyon yoksa:* Summoner kendisi direkt 2x hasar saldırısı yapar + 1 Charge kazanır (acil direct damage — minyonsuz kaldığında seni kurtarır).
*Zincirleme:* Golem'e emir → Golem hedef düşmana koşuyor ve onu duvara çarpıyor (stun).
> Harmanlama: **D2 Necro targeting** — aktif kontrol / **WoW Pet "Claw"** empowered / **GW1 Charge** — minyon odaklama
> Neden: Pasif izci değil, aktif komutan. Hedefi ve minyonu seçiyorsun.

---

**[7] DEATH HARVEST**
Etraftaki ölen her düşman/minyon → Summoner geçici hasar+hız bonusu alır. Her cesedin başına +5% (max +40%).
*Zincirleme:* Death Harvest aktifken Corpse Explosion → patlama hasarı +50%.
> Harmanlama: **WoW Demo Soul Shards** / **GW1 Necrotic Traversal** — ölüden enerji / **PoE Soul Strike** — momentum
> Neden: Büyük savaş sonrası en güçlü anın gelmesi. "Tüm enerji bende şimdi."

---

**[8] ANIMATE ARMOR**
Ölen düşmanı 15s için kendi safına geçir (orijinal statlarıyla). Sonra dağılıyor.
*Zincirleme:* Animate Armor ile canlandırılan düşman ölünce → Blood for Power tetiklenir.
> Harmanlama: **D2 Revive** — düşmanı kopyala / **WoW DK Raise Ally** — güçlü şeyi kendin için kullan / **GW1 Animate Bone Horror** stat kopyalama
> Neden: Boss'ın mini versiyonunu kendi yanında savaştırmak. Çok tatminkar.

---

### SİNERJİ HARİTASI

| Tier | Skill'ler | Ne Yapar |
|------|-----------|----------|
| ⭐ **CORE COMBO** | **CORPSE EXPLOSION + DEATH HARVEST** | Ölü patlat → hasar bonusu biriktir → Death Harvest aktifken Corpse Explosion +50%. Ölüm bir kaynak |
| 🔗 **ZİNCİR** | **RAISE SKELETON + RALLY CRY**, **DEATH NOVA + BLOOD FOR POWER** | İskelet besle-güçlendir; minyon feda döngüsü (feda → charge → çağır → feda) |
| 🔵 **BAĞIMSIZ** | Summon Golem, Commanding Strike, Animate Armor | Güçlü ama build bağımsız |

---

### PASİF HAVUZ

**[P1] ARMY'S STRENGTH** — Her canlı minyon → tüm hasar +5%. Max 3 minyon = +15%.
> GW1 Death Magic + D2 Skeleton Mastery

**[P2] BLOOD FOR POWER** — Minyon ölünce → 1 Charge + CD'ler -30%, 5s.
> WoW Soul Leech + D2 "ölümden güç" teması

**[P3] UNDYING HUNGER** — Minyonlar verdiği hasarın %3'ü Summoner'ı iyileştiriyor.
> D2 Life Tap + GW1 vampirik minyon builds

**[P4] CORPSE LORD** — Minyon öldürdüğü düşman otomatik patlayan cesetle başlıyor.
> D2 Corpse Explosion + GW1 Death Nova pasif hali

---

### ULTIMATE HAVUZ

**[U1] SACRIFICIAL PURGE** *(45s CD)*
Tüm minyonları feda et → devasa AoE patlama (her minyon başına hasar ve alan büyür).
> **GW1 sacrifice mechanic** + **D2 Corpse Explosion chain** + **WoW Dark Transformation** inversed

**[U2] SUMMON LEGION** *(65s CD)*
15s için maksimum minyon sayısı tüm tiplerden anında gelir.
> **D2 Necro full army** + **WoW Nether Portal** + **GW1 Minion Bomb** speed builds

---

---

## HEXER — "Ceza ve Çürüme"

**Core Fantasy:** Saldırmam gerekmiyor. Sen zaten çürüyorsun.
**Kaynak Sistemi:** Hex Stacks (düşman başına 0-10, 5s decay eğer hiçbir debuff uygulanmazsa)
**Mekanik Derinlik:** WoW Affliction'ın DoT yönetimi + GW1 Mesmer'ın "düşmanı kendi silahına dönüştürme" felsefesi + PoE Hexblast'ın stack patlatma sistemi.

---

### AKTİF HAVUZ

---

**[1] CORRUPTION**
Anında DoT 10s, 2 stack. Her Corruption ile yenilenir (süre uzar, stack birikmez, ama tazelenir). *İlk cast (0 stack hedef):* 3 stack verir + anlık küçük hasar (combat açılışı garantisi — stack yokken bile zayıf hissetmezsin).
*Zincirleme:* Agony aktifken Corruption yenilenirse → ikisi birlikte tick hızları artıyor (sinerji).
> Harmanlama: **WoW Corruption** — rotasyonun temeli / **D2 Amplify Damage** — uzayan etki / **GW1 Rotting Flesh** — süregelen çürüme
> Neden: Öğrenmesi kolay, optimize etmesi zor. Her savaşta ilk yapılan şey bu.

---

**[2] AGONY**
Yavaş yüklenen DoT: her tick +10% hasar artışı (max %100). 2 stack ekler.
*Zincirleme:* Max hasar seviyesine ulaşınca → Hexblast cooldown -5s.
> Harmanlama: **WoW Agony** — escalation / **PoE Wither stacks** — zaman içinde büyüyen tehdit / **D2 Decrepify** evolved
> Neden: Sabırlı Hexer'ın ödülü. "Bekle, şimdi patlat."

---

**[3] EMPATHY**
8s süre: Hedef skill kullanırsa kendine hasar verir (%40 orijinal saldırı gücünde).
*Zincirleme:* Empathy + Paladin'in Shield of Retribution (dual class) → ikisi aynı anda: düşman vurursa hem kendine zarar verir hem hasar geri döner.
> Harmanlama: **GW1 Mesmer Empathy** — tam bu skill, oyun tarihinin en özgün CC'si / **WoW Unstable Affliction** retribution / **D2 Iron Maiden** — saldıranı cezalandırma
> Neden: Düşmanın psikolojisini değiştiriyor. "Saldırmalı mıyım?" sorusunu yaratıyor.

---

**[4] HEXBLAST**
Hedefin tüm stack'lerini patlatır: her stack %100 hasar. 3 stack ile mi kullanırsın, 10 ile mi? Büyük fark.
*Zincirleme:* Max 10 stack + Hexblast → CD anında sıfırlanır + patlama yakınlardaki düşmanlara 2 stack yayıyor.
> Harmanlama: **PoE Hexblast** — isim aynı, konsept tam / **GW1 Fevered Dreams** — spreading hexes / **D2 Bone Spirit** — takip eden finisher
> Neden: Her savaşın kulminasyon anı. "Ne zaman basayım?" sorusu sürekli aklında.

---

**[5] PANDEMIC**
Aktif DoT'ları tüm yakın düşmanlara anında yay (4m çevre). Energy harcamaz, CD 12s.
*Zincirleme:* Rogue dual class Fan of Knives ile aynı anda → tüm debufflar hem yayıldı hem bleed eklendi.
> Harmanlama: **WoW Affliction Pandemic** — spread mechanic / **GW1 Mark of Pain** — AoE debuff / **D2 Poison Nova "refresh"** — toplu uygulama
> Neden: Tek düşmanda kurduğun düzeni bütün odaya anlık kopyala.

---

**[6] HAUNT**
Ruh hedefe uçar: Weakness debuff uygular + hedefi kışkırtır (mini-taunt). Hedef ölünce ruh Hexer'a geri döner ve +20 HP verir.
*Zincirleme:* Haunt + Battle Cry (Warblade dual class) → hem ruh hem Warrior taunt = düşman kimin peşinde bilerek karışıyor.
> Harmanlama: **WoW Haunt** — ruh + debuff + geri dönüş mekanik / **GW1 Mesmer Haunting Thoughts** / **D2 Revenant** benzeri
> Neden: Sürekli hareket eden bir debuff. Uzaktan uygulayıp "bekle" anı.

---

**[7] UNSTABLE AFFLICTION**
Hedef üzerine yerleştir: 6s sonra patlama. Eğer hedef bu 6s içinde iyileşmeye çalışırsa → patlama anında 4 stack ekler.
*Zincirleme:* Hedef stun/CC altındaysa → patlama garantili tam stack ile gerçekleşir.
> Harmanlama: **WoW Unstable Affliction** — PvP ikonası, iyileşmeyi cezalandırma / **GW1 Backfire** — skill kullanımı cezası / **PoE Punishment** — koşullu patlama
> Neden: Rakibin hamlelerini planlamasını bozuyor. "İyileşemem."

---

**[8] ENERVATE**
Anlık: Hedefin hareket hızı -50%, saldırı hızı -40%, 10s.
*Zincirleme:* 5+ stack üzerindeki hedef Enervate → debuff süre iki katına çıkar.
> Harmanlama: **ArcheAge Witchcraft Enervate** — zayıflatma iskeletidir / **GW1 Fevered Dreams** — toplu yavaşlatma / **D2 Decrepify** — her şeyi kısaltma
> Neden: Yavaş boss = kontrol senin. Tank olmadan tanklamak.

---

### SİNERJİ HARİTASI

| Tier | Skill'ler | Ne Yapar |
|------|-----------|----------|
| ⭐ **CORE COMBO** | **AGONY + HEXBLAST** | Max stack bekle → Hexblast CD sıfırla + zincir patlama. Hexer'ın tüm rotasyonu bu iki skill için var |
| 🔗 **ZİNCİR** | **CORRUPTION**, **PANDEMIC** | Corruption combat açılışı (3 stack garantisi); Pandemic tek hedefteki stack'ı tüm odaya kopyala |
| 🔵 **BAĞIMSIZ** | Empathy, Haunt, Unstable Affliction, Enervate | Situasyonel ama oyun değiştirici |

---

### PASİF HAVUZ

**[P1] DEBUFF THRESHOLD** — Hedef 5+ stack → tüm hasar +20%.
> PoE Wither + GW1 Insidious Parasite

**[P2] PUNISHING HEX** — Hexer'a saldıran düşmana 2 stack + hasar ricochet.
> GW1 Spiteful Spirit + D2 Iron Maiden + WoW Retaliation

**[P3] LINGERING CURSE** — Stack decay hızı %50 yavaşlar.
> PoE Hexmaster + GW1 Curses duration

**[P4] SOUL REND** — 10 stack hedefe uygulayınca → hedef 8s boyunca HP regen edemez.
> GW1 Drain Delusions combo + WoW Mortal Strike on DoT

---

### ULTIMATE HAVUZ

**[U1] HEXPLODE** *(60s CD)*
Hedefdeki tüm stack patlar, %100/stack + yakın düşmanlara 2 stack yayılır. Zincir patlama potansiyeli.
> **PoE Hexblast AoE** + **GW1 Mark of Pain** zincirleme + **WoW Seed of Corruption** explosion

**[U2] CURSE OF THE GRAVE** *(70s CD)*
12s: Alandaki tüm düşmanlar +50% hasar alır, evasion yok, hareket hızı -30%.
> **GW1 Mark of Pain** + **D2 Lower Resist Necro** + **ArcheAge Petrification** evolved

---

---

## ÇAPRAZ SİNERJİ TABLOSU — Dual Class Özel Etkileşimleri

> Bunlar döküman boyunca "Zincirleme" notlarında geçen ama ayrıca vurgulanması gereken hidden interactions.

| Kombo | Skill 1 | Skill 2 | Ne Oluyor |
|-------|---------|---------|-----------|
| **Warblade + Elementalist** | Colossus Smash (armor ignore 6s) | Arcane Blast × 4 + Arcane Barrage | Barrage armor ignore window'da → cap kaldırılıyor |
| **Rogue + Hexer** | Fan of Knives (tüm debuffleri yay) | Pandemic (tüm DoT'ları yay) | Aynı anda → oda tamamen debufflı |
| **Paladin + Hexer** | Empathy (saldırı = self-hasar) | Shield of Retribution (hasar geri döner) | Düşman hem kendi kendine zarar veriyor hem geri yiyor |
| **Ranger + Summoner** | Black Arrow (ölen düşman ruh bırakır) | Blood for Power (minyon ölünce +charge) | Ruh = minyon → ölünce charge geliyor → sonsuz döngü imkânı |
| **Brawler + Paladin** | Ruthless (HP %30- → lifesteal aktif) | Holy Endurance (hasar alınca HP geri gelir) | HP tam %30'da kalıcı dengeye giriyor → Ruthless permanentle aktif |
| **Elementalist + Hexer** | Fireball DoT + Shadowflame DoT | Pandemic | 2 farklı DoT aynı anda tüm odaya yayılıyor |
| **Summoner + Hexer** | Death Nova (minyon feda → zehir bulutu) | Hexer Pandemic | Zehir bulutu Hexer debufflarıyla yayılıyor |
| **Warblade + Paladin** | Battle Cry (taunt, düşmanlar geliyor) | Consecration + Shield of Retribution | Düşmanlar koşarak kutsal zeminde patlıyor |

---

*Dosya: SINIF_SKILL_HAVUZU.md*
*İlgili dosyalar: CLAUDE_SENTEZ.md (arketip isimleri, Grudge, sentez), OYUN_KONSEPT.md (framework)*
