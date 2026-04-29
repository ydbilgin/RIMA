# RIMA - Full Skill Audit + Gelişim Planı

Bu dosya, Claude ve benzeri modeller için **Markdown-öncelikli kanonik inceleme** olarak hazırlanmıştır.

## Claude için en rahat format

| Konu | Öneri |
|---|---|
| En rahat okuduğu format | **Markdown (.md)** |
| DOCX okur mu? | Evet, ama büyük tablolarda yapı bazen Markdown kadar temiz ayrışmaz. |
| En iyi yükleme sırası | 1) Bu `.md` dosyası 2) Orijinal `SINIF_VE_SKILL_KARAR_BELGESI.md` 3) İstediğin görev promptu |
| Kullanım amacı | Balance review, skill revize planı, class overlap kontrolü, DLC niş değerlendirmesi |
| Claude'a verilecek kısa komut | `Use this markdown as the current canonical class audit. Preserve structure, compare only against this baseline, and suggest minimal surgical edits.` |

## Master Audit Özeti

| Class | Strongest Build Axis | Neden Baskın | Weakest Skill | Neden Zayıf | Overlap Yaratan Skill | Rotation-Lock | Surgical Fix | Post-fix Kimlik |
|---|---|---|---|---|---|---|---|---|
| Warblade | Execution | Iron Charge → Crippling Blow → Iron Crush → Death Blow tek hedefi en temiz şekilde öldürüyor. | Deep Wound | Bleed hattı class fantasy ve kill tempo açısından ana execution planının gerisinde kalıyor. | Iron Counter | Med | Deep Wound: Rage+20 → Rage+35 | Daha net bir armor-breaker / execution bruiser olur. |
| Elementalist | Fire Burst | Combustion cast frictioni siliyor; Fireball ve Living Bomb ritmi Meteor ile hızlı bitiriyor. | Mirror Image | Savunma değeri var ama kill tempo ve build sinerjisi diğer slotlara göre düşük. | Mirror Image | Med | Mirror Image: 2 kopya → 3 kopya | Daha belirgin bir stance-dancing battle mage olur. |
| Shadowblade | Bleed Lord | Hem single target hem room spread üretiyor; stealth zorunluluğu düşük, payoff yüksek. | Kidney Shot | 5 CP stun genelde lethal finisher yerine pahalı kontrol harcaması gibi kalıyor. | Mirage Blade | Low | Kidney Shot: 5 CP hard gate → 3 CP minimum gate | Assassin değil sadece stealth user değil; gerçek finisher-rogue kimliği daha net olur. |
| Ranger | Sniper | Focus eşikleriyle en iyi ölçeklenen ve boss conversionı en güçlü olan eksen. | Flare | PvE’de stealth reveal niş kalıyor; çoğu zaman sadece küçük slow alanı ve az Focus. | Point Blank | Med | Flare: Focus+20 → Focus+40 | Uzak mesafe baskısı daha güvenli, yakın ceza ise hâlâ anlamlı kalır. |
| Ravager | Fury Engine | En stabil risk-ödül çizgisi burada; Fury güvenilir doluyor ve kit parçaları birbirini besliyor. | Intimidating Shout | Fear/scatter, melee bruiser’ın paketi toplama arzusuna ters düşüyor. | Shatter Armor | Low | Intimidating Shout: fear/flee → stagger | Daha güvenli ama hâlâ vahşi bir berserker olur. |
| Ronin | Iaido Burst | Iaido Stance + Quickdraw + Tension 100 + Void Cleave sınıfın en net ve en tatmin edici hattı. | Phantom Step | Tactical deception faydası var ama doğrudan burst/draw araçları kadar yüksek değer üretmiyor. | Phantom Step | Med | Phantom Step: 2 afterimage → 3 afterimage | Daha belirgin bir iaido duelist olur, rogue klonuna kaymaz. |
| Gunslinger | Mobile Assassin | Rift Dash + Quickdraw + Critical Shot + Point Blank Execute en yüksek burst-per-input hattı. | Smoke Grenade | Daha çok dual-class enabler gibi çalışıyor; kendi damage race’ine az katkı veriyor. | Smoke Grenade | Med | Smoke Grenade: 3m → 5m | Akimbo mobility assassin kimliği daha belirginleşir. |
| Brawler | Combo Machine | Charge üretimi ve harcama akışı burada en net; en tatlı vuruş ritmi bu eksende. | Guard Break | Ritim classı için fazla generic kalıyor; cadence değiştirmeden sadece shred veriyor. | Counter Blow | High | Guard Break: +2 Charge → +3 Charge | Tek output yerine tempo brawler + burst spender olarak iki yüzlü çalışır. |
| Summoner | Sacrifice Engine | Feda ekonomisi en yüksek room swing’i burada üretiyor; sınıfın gerçek farkı burada. | Bone Shield | Koruma sağlıyor ama board presence’i düşürdüğü için ana feda-ekonomiye kıyasla zayıf kalıyor. | Death Nova | Med | Bone Shield: 3s → 5s | Daha net bir commander-sacrificer hibriti olur. |
| Hexer | Hex Overload | Passive sabrı kırıp stack gain’i hızlandırıyor; Hexblast çıkışı çok baskın hale geliyor. | Cursed Mirror | Rakibe bağımlı ve reaktif; diğer bütün slotlar gibi proaktif stack üretmiyor. | Haunt | High | Cursed Mirror: %50 yansıma gücü → %100 | Sadece tek hedef stack-bomb değil; oda geneline bulaşan gerçek curse specialist olur. |

## Warblade

| Alan | Değer |
|---|---|
| Core Fantasy | Yaklaş. Sabitle. Zırhı kır. İnfaz et. |
| Kaynak | Rage (0-100) — hasar VEREREK +10/vuruş, CC'li düşmana +20, boşta -5/sn *(savaş dışında — oda temizse — drain yok)* |
| Burst | BLADESTORM — Rage %100: 5s spin, CC immune, her 0.5s AoE %120 hasar |
| Güçlü eksen | Execution - Iron Charge → Crippling Blow → Iron Crush → Death Blow tek hedefi en temiz şekilde öldürüyor. |
| Zayıf halka | Deep Wound - Bleed hattı class fantasy ve kill tempo açısından ana execution planının gerisinde kalıyor. |
| Overlap riski | Iron Counter - Ronin benzeri counter kimliği yaratıyor. |
| Rotation-Lock | Med |
| Gelişim hedefi | Execution hattını güçlendirirken control alt eksenini diri tutmak. |
| Gelişim sonrası class hissi | Daha net bir armor-breaker / execution bruiser olur. |

### Build Eksenleri
| Eksen | Skill paketi |
|---|---|
| Execution | Iron Charge + Crippling Blow + Iron Crush + Death Blow |
| Control Breaker | Gravity Cleave + War Stomp + Sunder Mark + Death Blow |
| Last Stand | Ironclad Momentum + Iron Counter + Battle Surge + Death Blow |

### Tüm Skill Planı
| Girdi | Skill | Mevcut işlev | Gelişim kararı | Gelişim sonrası ne olur |
|---|---|---|---|---|
| LMB | Iron Combo | 3 vuruşluk melee zincir (sweep → overhead → shoulder ram). Her vuruş Rage+8, 3. vuruş küçük knockback + Rage+15. 0.8s duraksama = combo sıfırlanır. | Korunur | Execution ve armor-break kimliğini temiz tutar. |
| RMB | Rage Outlet | Rage 40+ iken aktif. Kısa AoE patlaması, Rage −30, çevredeki düşmanlar sendeliyor. Rage boşaltırken hasar verir, CD 1.5s. | Eşik hafif düşür | Rage 40+ → 30+ düşün. Düşük yoğunluklu odalarda rage dump erişimi artar. |
| Core | Iron Charge | 8m dash + 1.5s stun, Rage+20 | Korunur | Execution ve armor-break kimliğini temiz tutar. |
| Core | Crippling Blow | Büyük hasar + iyileşme -%50 (6s) | Korunur | Execution ve armor-break kimliğini temiz tutar. |
| Core | Iron Crush | 6s: tüm hasar +%30 | Korunur | Execution ve armor-break kimliğini temiz tutar. |
| Core | Gravity Cleave | Silahı yere çarpar, 4m çapında çeker + %140 hasar, 0.8s slow | Çekim netliği artır | Çap veya çekim gücü az miktar artarsa Control Breaker ekseni daha güvenilir olur. |
| Core | Sunder Mark | Hedefe işaret: 8s zırh -%40, tüm hasar bonusu görünür | Korunur | Execution ve armor-break kimliğini temiz tutar. |
| Core | War Stomp | 3m knockup 2s, Rage+25 | Korunur | Execution ve armor-break kimliğini temiz tutar. |
| Core | Ironclad Momentum | 6s: alınan hasar %30 yok sayılır + her 10 hasar = +10 Rage | Korunur | Execution ve armor-break kimliğini temiz tutar. |
| Core | Iron Counter | 0.8s pencere: vurulursa %180 karşı saldırı + Rage+25 + 0.5s stun | Kimlik sınırı koru | Defansif pencere kalsın ama iframe büyütme verme; Ronin benzeşmesi artmasın. |
| Advanced | Blade Rush | 6m dash + çizgideki herkese %120, Rage+15/hedef | Korunur | Execution ve armor-break kimliğini temiz tutar. |
| Advanced | Battle Surge | 8s: her Rage harcaması = HP +%5 *(harcama sonrası 2s internal CD — bu sürede ikinci harcama heal üretmez)* | Korunur | Execution ve armor-break kimliğini temiz tutar. |
| Advanced | Deep Wound | Bleed DoT 8s + Rage+20 | Kaynak buffı | Rage üretimini 35 civarına çıkar. Bleed hattı execution planına daha iyi bağlanır. |
| Master | Death Blow | SADECE HP<%30: %400 hasar, Rage boşaltır | Korunur | Execution ve armor-break kimliğini temiz tutar. |

### Class Sonucu
| Kalem | Sonuç |
|---|---|
| Tek cerrahi düzeltme | Deep Wound: Rage+20 → Rage+35 |
| En kritik dikkat notu | Execution hattını güçlendirirken control alt eksenini diri tutmak. |
| Revize sonrası beklenen sonuç | Daha net bir armor-breaker / execution bruiser olur. |

## Elementalist

| Alan | Değer |
|---|---|
| Core Fantasy | Her şeyi yakıyorum. Ama önce ritmi buluyorum. |
| Kaynak | Mana (0-100, +8/sn) + Elemental State (Fire veya Frost, max 5 stack) |
| Burst | INFERNO — Mana %100: 7s arena-wide ateş yağmuru |
| Güçlü eksen | Fire Burst - Combustion cast frictioni siliyor; Fireball ve Living Bomb ritmi Meteor ile hızlı bitiriyor. |
| Zayıf halka | Mirror Image - Savunma değeri var ama kill tempo ve build sinerjisi diğer slotlara göre düşük. |
| Overlap riski | Mirror Image - Summoner benzeri kopya/çağrı hissi veriyor. |
| Rotation-Lock | Med |
| Gelişim hedefi | Element switch ritmini öldürmeden Fire/Frost/Arcane üçlüsünü ayrıştırmak. |
| Gelişim sonrası class hissi | Daha belirgin bir stance-dancing battle mage olur. |

### Build Eksenleri
| Eksen | Skill paketi |
|---|---|
| Fire Burst | Combustion + Fireball + Living Bomb + Meteor |
| Frost Lock | Glacial Spike + Blizzard + Frozen Orb + Meteor |
| Arcane Storm | Arcane Surge + Chain Lightning + Arcane Blast + Blink |

### Tüm Skill Planı
| Girdi | Skill | Mevcut işlev | Gelişim kararı | Gelişim sonrası ne olur |
|---|---|---|---|---|
| LMB | Rift Bolt | Hızlı rift enerji mermisi, Mana+3/isabet. Her 3. bolt empowered (daha büyük, +1 Elemental State). Hareket halinde atılabilir. | Korunur | Element switch ritmini ve burst kontrolünü korur. |
| RMB | Element Switch | Fire State ↔ Frost State anında geçiş. Geçiş anında küçük nova patlaması (aktif element tipinde). CD yok — ritmik kullanım için tasarlandı. | Korunur, UI ile desteklenir | Aktif state göstergesi çok net olursa ritim classı daha rahat okunur. |
| Core | Fireball | Orta hasar + ateş DoT 4s, Fire State+1 | Korunur | Element switch ritmini ve burst kontrolünü korur. |
| Core | Glacial Spike | 6m buz hattı: hattaki düşmanlar %40 slow + %180 hasar, Frost State+2. Fire State 1 stack tüketir | Korunur | Element switch ritmini ve burst kontrolünü korur. |
| Core | Living Bomb | 5s sonra patlama, öldürünce 3 komşuya kopyalanır | Fire payoff koru | Ücretsiz 3. Fireball hattı ana burst kimliğini taşımaya devam eder. |
| Core | Blink | 6m ışınlanma, geçilen düşmanlara hasar, sonraki spell +%20 | Korunur | Element switch ritmini ve burst kontrolünü korur. |
| Core | Frozen Orb | Yavaş hareket eden küre, yolundakileri 5s chill | Korunur | Element switch ritmini ve burst kontrolünü korur. |
| Core | Arcane Blast | Her cast +%20 hasar, +%30 mana maliyet. 4. cast Barrage açar. Cap: 4 stack max, cross-class ile kaldırılamaz | Cap okunurluğu artır | 4 stack cap UI’de görünürse oyuncu Barrage penceresini daha bilinçli oynar. |
| Core | Meteor | 0.5s wind-up → büyük AoE knockdown (hareket devam eder, kanallamaz) | Korunur | Element switch ritmini ve burst kontrolünü korur. |
| Core | Mirror Image | 2 kopya 8s, hasar önce kopyaya gelir | Savunma değerini yükselt | 3 kopya veya daha belirgin ölüm patlaması ile pick rate toparlar. |
| Advanced | Chain Lightning | 5 hedefe sekiyor | Korunur | Element switch ritmini ve burst kontrolünü korur. |
| Advanced | Arcane Surge | 8s Arcane Field: mana regen +%100, cast süresi -%50 | Arcane ekseni sabit tut | Blink sonrası bedava Meteor/Frozen Orb pencereyi bozmadan büyü patlamasını netleştirir. |
| Advanced | Combustion | 8s: tüm Fire spell instant cast, mana maliyet ×2 | Korunur | Element switch ritmini ve burst kontrolünü korur. |
| Master | Blizzard | 1s cast → bölge bağımsız 8s slow+tick (kanal gerekmez, hareket devam eder) | Korunur | Element switch ritmini ve burst kontrolünü korur. |

### Class Sonucu
| Kalem | Sonuç |
|---|---|
| Tek cerrahi düzeltme | Mirror Image: 2 kopya → 3 kopya |
| En kritik dikkat notu | Element switch ritmini öldürmeden Fire/Frost/Arcane üçlüsünü ayrıştırmak. |
| Revize sonrası beklenen sonuç | Daha belirgin bir stance-dancing battle mage olur. |

## Shadowblade

| Alan | Değer |
|---|---|
| Core Fantasy | Görmüyorsun. Zaten geç. |
| Kaynak | Energy (0-100, +15/sn) + Combo Points (0-5) |
| Burst | SHADOW DANCE — Energy %100 + CP 5: 8s her saldırı sonrası stealth |
| Güçlü eksen | Bleed Lord - Hem single target hem room spread üretiyor; stealth zorunluluğu düşük, payoff yüksek. |
| Zayıf halka | Kidney Shot - 5 CP stun genelde lethal finisher yerine pahalı kontrol harcaması gibi kalıyor. |
| Overlap riski | Mirage Blade - Ronin afterimage oyununa fazla yaklaşıyor. |
| Rotation-Lock | Low |
| Gelişim hedefi | CP-finisher ekonomisini korurken stealth ve bleed yollarını eşdeğer tutmak. |
| Gelişim sonrası class hissi | Assassin değil sadece stealth user değil; gerçek finisher-rogue kimliği daha net olur. |

### Build Eksenleri
| Eksen | Skill paketi |
|---|---|
| Assassin | Ambush + Vanish + Backstab + Preparation |
| Bleed Lord | Hemorrhage + Toxic Eruption + Rupture + Fan of Knives |
| Phantom | Mirage Blade + Shadowstep + Evasion + Kidney Shot |

### Tüm Skill Planı
| Girdi | Skill | Mevcut işlev | Gelişim kararı | Gelişim sonrası ne olur |
|---|---|---|---|---|
| LMB | Quick Stab | Sol → sağ 2 hızlı hançer, her combo +1 CP, Energy+5. Stealth'ten kullanılırsa otomatik Backstab bonusu uygulanır. | Korunur | Combo Point ve finisher ekonomisini sağlam tutar. |
| RMB | Shadow Slip | 2m anlık kaçış daşı (imleç yönünde), CD 1s. Shadowstep'ten farkı: hedefe gitmez, sadece konumlanma; skill slot tutmaz. | Korunur | Combo Point ve finisher ekonomisini sağlam tutar. |
| Core | Backstab | Arkadan: %200 hasar+3CP. Önden: normal | Korunur | Combo Point ve finisher ekonomisini sağlam tutar. |
| Core | Hemorrhage | Bleed 8s DoT+2CP, öldürünce yakına yayılır | Korunur | Combo Point ve finisher ekonomisini sağlam tutar. |
| Core | Rupture | 5CP finisher: bleed+hasar, CP'ye göre süre uzar | Korunur | Combo Point ve finisher ekonomisini sağlam tutar. |
| Core | Shadowstep | Hedefe 8m ışınlan, 0.5s stun, Energy-25 | Korunur | Combo Point ve finisher ekonomisini sağlam tutar. |
| Core | Kidney Shot | 5CP: 4s stun, CP'ye göre uzar (max 5s) | CP eşiğini indir | 3 CP minimum gate ile control finisher gerçek opsiyon olur. |
| Core | Ambush | Sadece stealth'ten: %300 hasar+4CP+%20 slow | Korunur | Combo Point ve finisher ekonomisini sağlam tutar. |
| Core | Fan of Knives | 360° AoE, tüm aktif debuffları tüm düşmanlara uygular | Debuff spread uzmanı olarak kalır | Bleed Lord ekseninin oda temizleme kimliğini taşımaya devam eder. |
| Core | Evasion | 4s %100 dodge, her dodge=+1CP | Korunur | Combo Point ve finisher ekonomisini sağlam tutar. |
| Advanced | Mirage Blade | 3s: geçilen konumlara gölge bırakır, dokunan düşman %100 hasar+1CP | Ronin ayrımı keskinleştir | Stun yerine pathing/zone payoff öne çıkarılırsa afterimage farkı netleşir. |
| Advanced | Toxic Eruption | 5CP: hedefteki tüm debuffları patlatır (tüketir). Her zehir/kanama stack başına %150 hasar, yakına yayılır | Korunur | Combo Point ve finisher ekonomisini sağlam tutar. |
| Advanced | Preparation | Mobilite + stealth CD'lerini sıfırla (Shadowstep, Shadow Slip, Vanish, Evasion). Ofansif finisher CD'leri etkilenmez. 90s CD | Korunur | Ofansif finisher resetlememesi abuse’u kapalı tutar. |
| Master | Vanish | Savaşta anlık stealth, 3s, 50s CD | Korunur | Assassin ekseninin garantili açılış butonu olarak kalmalı. |

### Class Sonucu
| Kalem | Sonuç |
|---|---|
| Tek cerrahi düzeltme | Kidney Shot: 5 CP hard gate → 3 CP minimum gate |
| En kritik dikkat notu | CP-finisher ekonomisini korurken stealth ve bleed yollarını eşdeğer tutmak. |
| Revize sonrası beklenen sonuç | Assassin değil sadece stealth user değil; gerçek finisher-rogue kimliği daha net olur. |

## Ranger

| Alan | Değer |
|---|---|
| Core Fantasy | Sana ulaşamazsın. Her saniye kayıp veriyorsun. |
| Kaynak | Focus (4m+: +10/sn | 2m-: -20/sn) — Focus 75+: +%25 hasar | Focus 100: next skill free cast |
| Burst | RAIN OF ARROWS — 30s sabit CD: 5s tüm arena yağmur |
| Güçlü eksen | Sniper - Focus eşikleriyle en iyi ölçeklenen ve boss conversionı en güçlü olan eksen. |
| Zayıf halka | Flare - PvE’de stealth reveal niş kalıyor; çoğu zaman sadece küçük slow alanı ve az Focus. |
| Overlap riski | Point Blank - Gunslinger / shotgun alanına fazla yaklaşıyor. |
| Rotation-Lock | Med |
| Gelişim hedefi | Kite class hissini koruyup yakın mesafe çöküşünü biraz yumuşatmak. |
| Gelişim sonrası class hissi | Uzak mesafe baskısı daha güvenli, yakın ceza ise hâlâ anlamlı kalır. |

### Build Eksenleri
| Eksen | Skill paketi |
|---|---|
| Sniper | Aimed Shot + Tethering Arrow + Rapid Fire + Flare |
| Trap Master | Explosive Trap + Volley + Barbed Net Shot + Disengage |
| Kite Lord | Concussive Arrow + Black Arrow + Multi-Shot + Point Blank |

### Tüm Skill Planı
| Girdi | Skill | Mevcut işlev | Gelişim kararı | Gelişim sonrası ne olur |
|---|---|---|---|---|
| LMB | Quick Arrow | Anında tek ok, düşük hasar, Focus+4. Hareket halinde atılabilir, hiç CD yok. Kite döngüsünün temeli. | Korunur | Focus yönetimi ve kite baskısını korur. |
| RMB | Tactical Roll | Hareket yönüne kısa geri atlama + atlama sırasında 1 ok anında serbest ateş. Focus+8, CD 1.2s. Ne zaman basarsan o yöne atlıyor. | Odak geri kazanımını hafif artır | Focus+8 → +10 civarı bir buff yakın baskı altında toparlanmayı kolaylaştırır. |
| Core | Aimed Shot | Instant = %150 hasar; hold 1s = %250 + guaranteed crit (hold zorunlu değil) | Korunur | Focus yönetimi ve kite baskısını korur. |
| Core | Concussive Arrow | Knockback 4m + root 2s | Korunur | Focus yönetimi ve kite baskısını korur. |
| Core | Barbed Net Shot | Ağ fırlatır, 2s root + 4s kanama %40/sn | Trap Master omurgası | Disengage sonrası alan yayılımı build’i ayakta tutar. |
| Core | Explosive Trap | Zemine tuzak, 3s sonra patlama+slow 3s | Korunur | Focus yönetimi ve kite baskısını korur. |
| Core | Multi-Shot | Delici ok: tüm düşmanlardan geçer, her birine 1 wound stack (tam bleed için Barbed Net Shot gerekli) | Korunur | Focus yönetimi ve kite baskısını korur. |
| Core | Disengage | 6m geri atla, slow alanı bırak | Korunur | Focus yönetimi ve kite baskısını korur. |
| Core | Black Arrow | DoT + özel: bu DoT ile ölen düşman 8s ruh bırakır | Korunur | Focus yönetimi ve kite baskısını korur. |
| Core | Volley | 4m alana 3s yağmur, slow+tick | Korunur | Focus yönetimi ve kite baskısını korur. |
| Advanced | Rapid Fire | Burst: 8 hızlı ok 1s içinde, hareket kesilmez, Focus-30 | Korunur | Focus yönetimi ve kite baskısını korur. |
| Advanced | Tethering Arrow | Hedefe zincir ok: bağlanır, Ranger uzaklaştıkça zincir gerer ve hasar artar (4s, pasif prison yok) | Korunur | Mesafe-ödül kimliğini en iyi taşıyan skill. |
| Advanced | Flare | Stealth açığa çıkar + 6s slow alanı, Focus+20 | Kaynak buffı | Focus+40 ile utility butonundan tempo butonuna yaklaşır. |
| Master | Point Blank | ≤2m: mevcut Focus × %3 hasar + 5m knockback. Focus 60=×1.8, Focus 100=×3. Hard gate yok | Master olarak kalır ama sınır korunur | Hasar kalsın; ekstra execute ekleme, yoksa Breacher/Gunslinger alanına kayar. |

### Class Sonucu
| Kalem | Sonuç |
|---|---|
| Tek cerrahi düzeltme | Flare: Focus+20 → Focus+40 |
| En kritik dikkat notu | Kite class hissini koruyup yakın mesafe çöküşünü biraz yumuşatmak. |
| Revize sonrası beklenen sonuç | Uzak mesafe baskısı daha güvenli, yakın ceza ise hâlâ anlamlı kalır. |

## Ravager

| Alan | Değer |
|---|---|
| Core Fantasy | Az canken daha tehlikeliyim. Bu hata değil, strateji. |
| Kaynak | Fury (0-100) — SADECE hasar alarak +15/vuruş, HP düştükçe daha hızlı |
| Burst | BERSERK MODE — Fury %100: 15s defense ignore + %150 hasar. Her kill → en uzun CD'li skill sıfırlanır (bedava reset yok) |
| Güçlü eksen | Fury Engine - En stabil risk-ödül çizgisi burada; Fury güvenilir doluyor ve kit parçaları birbirini besliyor. |
| Zayıf halka | Intimidating Shout - Fear/scatter, melee bruiser’ın paketi toplama arzusuna ters düşüyor. |
| Overlap riski | Shatter Armor - Warblade’in Sunder Mark hattına fazla yakın. |
| Rotation-Lock | Low |
| Gelişim hedefi | Low-HP danger fantasy’yi güçlendirip düşman dağıtma cezalarını azaltmak. |
| Gelişim sonrası class hissi | Daha güvenli ama hâlâ vahşi bir berserker olur. |

### Build Eksenleri
| Eksen | Skill paketi |
|---|---|
| Glass Cannon | Reckless Swing + Bloodlust Strike + Blood-Drunk Leap + Death Wish |
| Fury Engine | Undying Tenacity + Bloodthirst + Whirlwind + Shatter Armor |
| Crowd Crusher | Iron Grab + Barbaric Charge + Intimidating Shout + Frenzied Leap |

### Tüm Skill Planı
| Girdi | Skill | Mevcut işlev | Gelişim kararı | Gelişim sonrası ne olur |
|---|---|---|---|---|
| LMB | Brutal Swing | Geniş yay balta salınımı, 1-3 düşmana çarpabilir. Fury+12/isabet, son 1s içinde hasar aldıysan Fury+20. Ağır hissi görsel ağırlıktan geliyor ama input snappy. | Korunur | Risk-ödül meleesini ve Fury motorunu korur. |
| RMB | Battle Cry | Kısa kükreme, 1.5m çevredeki düşmanları sana yönlendirir + 0.4s kısa zırh. Fury+15. CD 2s. Fury doldurma döngüsünü hızlandırır. *(İsim değişti: Taunt → Battle Cry — mekanik aynı)* | Korunur | Fury doldurma ve toplama aracı olarak sınıf açılışını sağlamlaştırır. |
| Core | Bloodlust Strike | Koni saldırı, HP'ye göre hasar artar (%30HP=+%120) | Korunur | Risk-ödül meleesini ve Fury motorunu korur. |
| Core | Whirlwind | 2s spin AoE, her vuruş savunma -%5 (max -%30) | Korunur | Risk-ödül meleesini ve Fury motorunu korur. |
| Core | Frenzied Leap | Hedefe atla, iniş AoE, hit=CD sıfır | Korunur | Risk-ödül meleesini ve Fury motorunu korur. |
| Core | Reckless Swing | Devasa tek hasar, 2s tam savunmasız | Risk penceresi korunur | Tam savunmasızlık kalsın; sınıfın kumar hissi buradan geliyor. |
| Core | Bloodthirst | Hızlı 5 vuruş, her vuruş küçük iyileşme | Korunur | Risk-ödül meleesini ve Fury motorunu korur. |
| Core | Intimidating Shout | 3m çevresinde 3s panik/kaçar | Fear yerine stagger | Melee paketi dağıtmak yerine yerinde bozan utility hâline gelir. |
| Core | Barbaric Charge | Düz çizgide her şeyi iter, stun/root immune | Korunur | Risk-ödül meleesini ve Fury motorunu korur. |
| Core | Undying Tenacity | 4s: alınan hasar %40 yok sayılır, HP %20 altına düşemez, alınan her darbe +10 Fury | Korunur | Risk-ödül meleesini ve Fury motorunu korur. |
| Advanced | Iron Grab | Yakala (≤2m): 1.5s hold, fırlat, Fury+30 | Menzil mikro buff | ≤2m → 3m yapılırsa yakın hedef yakalama daha az sinir bozucu olur. |
| Advanced | Blood-Drunk Leap | Hedefe sıçrar, %120 hasar. Fury'nin %30'unu tüketir, her 10 Fury +%20 hasar | Korunur | Risk-ödül meleesini ve Fury motorunu korur. |
| Advanced | Shatter Armor | Hedefin savunması -%40, 10s | Korunur | Risk-ödül meleesini ve Fury motorunu korur. |
| Master | Death Wish | 5s: HP 1 altına düşemez, Fury ×3 hızlı dolar | Korunur | Master kimliğini taşıyan en net “ölmeden delir” butonu. |

### Class Sonucu
| Kalem | Sonuç |
|---|---|
| Tek cerrahi düzeltme | Intimidating Shout: fear/flee → stagger |
| En kritik dikkat notu | Low-HP danger fantasy’yi güçlendirip düşman dağıtma cezalarını azaltmak. |
| Revize sonrası beklenen sonuç | Daha güvenli ama hâlâ vahşi bir berserker olur. |

## Ronin

| Alan | Değer |
|---|---|
| Core Fantasy | Çek. Kes. Bitir. |
| Kaynak | Draw Tension (0-100) — hareket halinde +20/sn, Quickdraw'da +30, 3s hareketsiz = -30/sn *(savaş dışında — oda temizse — drain yok)*. Tension 100: sonraki Quickdraw ×2 hasar |
| Burst | MUGEN NO KIRI — Tension 100: 5s her input instant draw-cut, CD yok, cut anlarında iframes |
| Güçlü eksen | Iaido Burst - Iaido Stance + Quickdraw + Tension 100 + Void Cleave sınıfın en net ve en tatmin edici hattı. |
| Zayıf halka | Phantom Step - Tactical deception faydası var ama doğrudan burst/draw araçları kadar yüksek değer üretmiyor. |
| Overlap riski | Phantom Step - Shadowblade’in afterimage alanına yaklaşıyor. |
| Rotation-Lock | Med |
| Gelişim hedefi | Tension hareket ekonomisini koruyup sahte-stealth hissini gerçek positioning’e çevirmek. |
| Gelişim sonrası class hissi | Daha belirgin bir iaido duelist olur, rogue klonuna kaymaz. |

### Build Eksenleri
| Eksen | Skill paketi |
|---|---|
| Iaido Burst | Quickdraw Slash + Iaido Stance + Tension 100 + Void Cleave |
| Phantom Dance | Haste Dash + Wind Step + Phantom Step + Flash Draw |
| Wave Clear | Mille Feuille Cut + Crescent Arc + Iai Pressure + Wind Step |

### Tüm Skill Planı
| Girdi | Skill | Mevcut işlev | Gelişim kararı | Gelişim sonrası ne olur |
|---|---|---|---|---|
| LMB | Sheath Walk | Hareket halinde hafif slash, Tension+5. 3 ardışık = öne kısa atılım + güçlü son darbe. Quickdraw Slash'tan farkı: iframes yok, skill slot tutmuyor, ama her zaman mevcut. | Korunur | Hareket-Tension-kesim ritmini korur. |
| RMB | Drawn Edge | Basılı tut = Tension birikiyor (max 1s). Bırak = birikim oranında tek slash. Skill CD'lerine dokunmaz, tüm Tension bonusları uygulanır. Temel ama derin. | Korunur | Slot tutmadan Tension harcayan temel hareket olarak sınıfı derinleştirir. |
| Core | Quickdraw Slash | Anlık katana çekimi, %180 ST hasar | Korunur | Hareket-Tension-kesim ritmini korur. |
| Core | Haste Dash | İleri slide, geçilen düşmanlara %120, afterimage bırakır. *Tension bonusları yalnızca Iai Pressure aktifken uygulanır* | Ronin çizgisinde kal | Tension bonuslarının sadece Iai Pressure ile açılması sınıfı spam dash’e düşürmez. |
| Core | Mille Feuille Cut | 3m önde 5 slash fan, her biri %80 AoE | Korunur | Hareket-Tension-kesim ritmini korur. |
| Core | Iaido Stance | 1s stance: hareketsiz durulsa bile Tension dolar, sonraki Quickdraw guaranteed crit | Akış sıkılaştır | 1s → 0.8s gibi küçük hız artışı kilit hissini azaltır. |
| Core | Wind Step | 3 hızlı yön değişimi + her adımda kısa slash, Tension +10/adım | Korunur | Hareket-Tension-kesim ritmini korur. |
| Core | Counter Draw | 0.5s pencere: gelen saldırıyı karşıla → %200 hasar + knockback, Tension +30 | Korunur | Hareket-Tension-kesim ritmini korur. |
| Core | Phantom Step | 2 konuma afterimage bırak 3s; düşmanlar afterimage'a saldırırken Ronin konumunu gizler *(tactical deception — MMO aggro değil)* | Deception payoff artır | 3 afterimage veya çarpışmada hafif slash ile pick değeri yükselir. |
| Core | Blade Veil | 1s deflect penceresi, Tension +20/deflect | Korunur | Hareket-Tension-kesim ritmini korur. |
| Advanced | Crescent Arc | Daire çizerek geçme, çizgideki her düşmana %140, son hedefe %280 | Korunur | Hareket-Tension-kesim ritmini korur. |
| Advanced | Flash Draw | Görüş alanındaki en yakın 3 düşmana ışınla-kes, her biri %160 | Korunur | Hareket-Tension-kesim ritmini korur. |
| Advanced | Iai Pressure | 6s: her Haste Dash Quickdraw bonusunu taşır | Korunur | Hareket-Tension-kesim ritmini korur. |
| Master | Void Cleave | Tension'ın tamamını boşalt → önündeki 10m koni içindeki tüm düşmanlara %15/Tension hasar, animasyon boyunca dokunulmaz *(yönlü finisher, ekran-wide değil)* | Korunur | Yönlü finisher olması ekran temizleme saçmalığını engeller. |

### Class Sonucu
| Kalem | Sonuç |
|---|---|
| Tek cerrahi düzeltme | Phantom Step: 2 afterimage → 3 afterimage |
| En kritik dikkat notu | Tension hareket ekonomisini koruyup sahte-stealth hissini gerçek positioning’e çevirmek. |
| Revize sonrası beklenen sonuç | Daha belirgin bir iaido duelist olur, rogue klonuna kaymaz. |

## Gunslinger

| Alan | Değer |
|---|---|
| Core Fantasy | Dur, nişan al değil — koş, ateş et, bitir. |
| Kaynak | Heat (0-100) — her ateşte +8. 100 = Overheat: 3s hasar +%50 + muzzle flash AoE, ardından 2s forced cooldown (Dual Fire + Fan the Hammer + Bullet Rain kilitli; Rift Dash, Critical Shot, Quickdraw kullanılabilir). Heat yönetimi gerçek trade-off |
| Burst | FULL METAL STORM — Heat 100: 5s position-lock yok, dual-fire, her ateş AoE muzzle flash |
| Güçlü eksen | Mobile Assassin - Rift Dash + Quickdraw + Critical Shot + Point Blank Execute en yüksek burst-per-input hattı. |
| Zayıf halka | Smoke Grenade - Daha çok dual-class enabler gibi çalışıyor; kendi damage race’ine az katkı veriyor. |
| Overlap riski | Smoke Grenade - Shadowblade utility alanına sarkıyor. |
| Rotation-Lock | Med |
| Gelişim hedefi | Heat’i gerçek trade-off tutup close burst hattını ayrıcalıklı kılmak. |
| Gelişim sonrası class hissi | Akimbo mobility assassin kimliği daha belirginleşir. |

### Build Eksenleri
| Eksen | Skill paketi |
|---|---|
| Heat Engine | Fan the Hammer + Burning Ammo + Bullet Rain + Full Metal Storm |
| Mobile Assassin | Rift Dash + Quickdraw + Critical Shot + Point Blank Execute |
| Crowd Suppressor | Suppression Fire + Smoke Grenade + Ricochet + Bullet Rain |

### Tüm Skill Planı
| Girdi | Skill | Mevcut işlev | Gelişim kararı | Gelişim sonrası ne olur |
|---|---|---|---|---|
| LMB | Dual Fire | İki silahtan eşzamanlı tek mermi, Heat+6 her ateşte. Tıkla ya da basılı tut = otomatik ateş. Hareket kesmeden ateş edilir. | Korunur | Heat yönetimi ve mobil burst hattını korur. |
| RMB | Hip Shot | Yana kısa kayma + aynı anda tek hedefli mermi, Heat+10. CD 0.8s. Hem konumlanma hem hasar — Gunslinger'ın tanımlayıcı hareketi. | Korunur | Kimlik kurucu hareket; slide+shot ritmi korunmalı. |
| Core | Rift Dash | Öne slide/takla, geçerken iki silahla ateş, 3m AoE %120 | Korunur | Heat yönetimi ve mobil burst hattını korur. |
| Core | Quickdraw | İki silahla anında tek hedef %180 burst | Korunur | Heat yönetimi ve mobil burst hattını korur. |
| Core | Bullet Rain | 3s süre, 5m alana yağmur, Heat +30 | Korunur | Heat yönetimi ve mobil burst hattını korur. |
| Core | Critical Shot | 1 mermi %300 ST | Korunur | Heat yönetimi ve mobil burst hattını korur. |
| Core | Smoke Grenade | 3m duman, içindeki düşman kör+yavaş 3s | Alan buffı | 3m → 5m ile kendi başına daha iyi oda kontrolü verir. |
| Core | Fan the Hammer | 1s içinde 6 hızlı ateş, Heat +40 | Korunur | Heat yönetimi ve mobil burst hattını korur. |
| Core | Suppression Fire | 4m hat boyunca iter + %80 hasar | Kontrol değerini biraz büyüt | Push hissi güçlü kalmalı; duvar/alan oynanışını destekler. |
| Core | Dead Eye | 2s: tüm mermiler crit, Heat üretimi +%50 | Korunur | Heat yönetimi ve mobil burst hattını korur. |
| Advanced | Ricochet | Mermi 3 düşmana sekiyor, her sekişte +%20 hasar | Korunur | Heat yönetimi ve mobil burst hattını korur. |
| Advanced | Reload Dance | Geri çekilme + reload: tüm skill CD -%20, Heat -30 | Korunur | Heat boşaltma ve tempo reset fonksiyonu sınıfı tek ritme kilitlemez. |
| Advanced | Burning Ammo | 8s: tüm mermiler ateş DoT | Korunur | Heat yönetimi ve mobil burst hattını korur. |
| Master | Point Blank Execute | ≤2m: %400 hasar, Heat anında 100 → Overheat tetikler | Finisher olarak tutulur | Tam shotgun sınıfına dönüşmemesi için shell/reload sistemi eklenmeden büyütülmemeli. |

### Class Sonucu
| Kalem | Sonuç |
|---|---|
| Tek cerrahi düzeltme | Smoke Grenade: 3m → 5m |
| En kritik dikkat notu | Heat’i gerçek trade-off tutup close burst hattını ayrıcalıklı kılmak. |
| Revize sonrası beklenen sonuç | Akimbo mobility assassin kimliği daha belirginleşir. |

## Brawler

| Alan | Değer |
|---|---|
| Core Fantasy | Durma. Vur. Ritim bul. Tekrar. |
| Kaynak | Charge (0-5) — her vuruşta +1, 3s hareketsiz = sıfırlanır. Her Charge: hasar +%10, hız +%3. 5 Charge = Charged State: sonraki skill +%50 güçlenir, Charge sıfırlanır |
| Burst | OVERDRIVE — 5s: her vuruş Charged State gibi davranır, Charge azalmaz |
| Güçlü eksen | Combo Machine - Charge üretimi ve harcama akışı burada en net; en tatlı vuruş ritmi bu eksende. |
| Zayıf halka | Guard Break - Ritim classı için fazla generic kalıyor; cadence değiştirmeden sadece shred veriyor. |
| Overlap riski | Counter Blow - Ronin counter alanına fazla yaklaşıyor. |
| Rotation-Lock | High |
| Gelişim hedefi | Empower ve Overdrive yollarını gerçekten iki ayrı spend hattına dönüştürmek. |
| Gelişim sonrası class hissi | Tek output yerine tempo brawler + burst spender olarak iki yüzlü çalışır. |

### Build Eksenleri
| Eksen | Skill paketi |
|---|---|
| Combo Machine | Mach Punch + Rush Combo + Aerial Rave + Momentum Strike |
| Ground Breaker | Shockwave Slam + Seismic Stomp + Guard Break + Overdrive |
| Counter Fighter | Counter Blow + Repulse + Cyclone Drive + Unstoppable Force |

### Tüm Skill Planı
| Girdi | Skill | Mevcut işlev | Gelişim kararı | Gelişim sonrası ne olur |
|---|---|---|---|---|
| LMB | Jab | Tek hızlı yumruk, Charge+1. Hızlıca tıklanırsa 4'lü oto-kombo (her hit +1 Charge). Brawler'ın ritim kaynağı — her şey buradan başlar. | Korunur | Charge ritmini ve yakın dövüş temposunu korur. |
| RMB | Weave | Kısa yan adım. Gelen saldırı bu adım sırasında gelirse: Charge+2 bonus. **Perfect timing (0.2s pencere): Charge+2 + 0.3s iframes.** Aktif dodge, Rift Parry'den farklı: CD yok, iframes yalnız perfect timing'de açılır — ritim oyununun kalbi. | Mükemmel timing ödülünü koru | Skill ceiling burada; sade ama güçlü kalmalı. |
| Core | Mach Punch | 4 hızlı yumruk, +4 Charge | Korunur | Charge ritmini ve yakın dövüş temposunu korur. |
| Core | Shockwave Slam | Yumruk yere, 4m şok dalgası AoE | Korunur | Charge ritmini ve yakın dövüş temposunu korur. |
| Core | Tornado Kick | 360° döner tekme, +2 Charge | Korunur | Charge ritmini ve yakın dövüş temposunu korur. |
| Core | Rush Combo | 5m atılır + 3 vuruş zinciri, +3 Charge | Korunur | Charge ritmini ve yakın dövüş temposunu korur. |
| Core | Guard Break | Hedef savunma -%40, 6s, +2 Charge | Charge buffı | +3 Charge ile generic shred butonu ritim planına daha iyi bağlanır. |
| Core | Repulse | Çevredeki tüm düşmanları iter, +1 Charge/düşman | Forked spend için yön ver | Anında 5 Charge üretimi empower hattını açar; Overdrive hattından ayrıştırmak için güzel kalır. |
| Core | Counter Blow | 0.4s pencere: gelen vuruşa %200 karşı punch + Charge +3 | Korunur | Charge ritmini ve yakın dövüş temposunu korur. |
| Core | Aerial Rave | Düşmanı havaya atar, 3 hava vuruşu, Charge korunur | Korunur | Charge ritmini ve yakın dövüş temposunu korur. |
| Advanced | Cyclone Drive | 2s döner hareket, temas edene %100/tur, Charge dolmaya devam | Korunur | Charge ritmini ve yakın dövüş temposunu korur. |
| Advanced | Seismic Stomp | 6m hat boyunca tüm düşmanlar 1.5s havaya kalkar | Korunur | Charge ritmini ve yakın dövüş temposunu korur. |
| Advanced | Momentum Strike | Charge sayısı × çarpan: 5 Charge = %500 tek vuruş | Empower spender olarak işaretle | Tek büyük darbe yolu burada kalırsa Overdrive branch ile ayrım netleşir. |
| Master | Unstoppable Force | 4s: Charge azalmaz, hız +%50, her dash = otomatik Rush Combo | Overdrive branch taşıyıcısı | Bu skill sürekli tempo yolunu temsil etmeli; tek vuruş burstle karışmamalı. |

### Class Sonucu
| Kalem | Sonuç |
|---|---|
| Tek cerrahi düzeltme | Guard Break: +2 Charge → +3 Charge |
| En kritik dikkat notu | Empower ve Overdrive yollarını gerçekten iki ayrı spend hattına dönüştürmek. |
| Revize sonrası beklenen sonuç | Tek output yerine tempo brawler + burst spender olarak iki yüzlü çalışır. |

## Summoner

| Alan | Değer |
|---|---|
| Core Fantasy | Ben savaşmıyorum. Feda ediyorum. Ve feda anı en güçlü andır. |
| Kaynak | Charges (0-4, auto +1/8s; minyon ölünce +1 anında) |
| Burst | ARMY OF THE DEAD — tüm Charge doluyken: 6s tüm minyonlar +%150, ölümsüz |
| Güçlü eksen | Sacrifice Engine - Feda ekonomisi en yüksek room swing’i burada üretiyor; sınıfın gerçek farkı burada. |
| Zayıf halka | Bone Shield - Koruma sağlıyor ama board presence’i düşürdüğü için ana feda-ekonomiye kıyasla zayıf kalıyor. |
| Overlap riski | Death Nova - Hexer tarzı alan DoT/debuff yayma hissine yaklaşıyor. |
| Rotation-Lock | Med |
| Gelişim hedefi | Army fantasy ile sacrifice fantasy arasındaki seçimleri daha anlamlı kılmak. |
| Gelişim sonrası class hissi | Daha net bir commander-sacrificer hibriti olur. |

### Build Eksenleri
| Eksen | Skill paketi |
|---|---|
| Sacrifice Engine | Blood for Power + Death Nova + Mass Sacrifice + Rally Cry |
| Army Commander | Raise Skeleton + Summon Golem + Commanding Strike + Soul Siphon Totem |
| Lich Burst | Lich Form + Dark Pact + Corpse Explosion + Bone Shield |

### Tüm Skill Planı
| Girdi | Skill | Mevcut işlev | Gelişim kararı | Gelişim sonrası ne olur |
|---|---|---|---|---|
| LMB | Command Strike | Minyon varsa: imlece en yakın düşmana hepsini yönlendirir + Summoner kısa staff darbesi. Minyon yoksa: Summoner'ın kendi saldırısı (%80 hasar). Dual fonksiyonlu — asla boşta kalmaz. | Korunur | Minyon ve sacrifice ekonomisini sağlam tutar. |
| RMB | Soul Dart | Kısa menzilli ruh mermisi, +0.5 Charge üretir, 1s CD. Minyonlar yokken hayatta kalma aracı; varken mark koyar, minyonlar mark'lı hedefe öncelik verir. | Korunur | Minyon yokken boşta kalmama sorununu çözen kritik araç. |
| Core | Raise Skeleton | 1 Charge → melee iskelet (max 3) | Korunur | Minyon ve sacrifice ekonomisini sağlam tutar. |
| Core | Summon Golem | 2 Charge → 1 tank Golem. HP<%20=patlama AoE | Korunur | Minyon ve sacrifice ekonomisini sağlam tutar. |
| Core | Rally Cry | Tüm minyonlar +%20 hasar+hız 10s | Korunur | Minyon ve sacrifice ekonomisini sağlam tutar. |
| Core | Corpse Explosion | Düşman veya minyon cesedini patlatır, AoE | Korunur | Minyon ve sacrifice ekonomisini sağlam tutar. |
| Core | Death Nova | 1 minyonu feda: 8s zehir bulutu | Korunur | Minyon ve sacrifice ekonomisini sağlam tutar. |
| Core | Commanding Strike | Seçili minyon ×4 hasar+invuln; minyon yoksa Summoner ×2 | Commander ekseninin omurgası | Tek minyonu öne çıkarma fikri korunmalı. |
| Core | Blood for Power | Minyon feda → Charge+1 + tüm CD -%30 | Korunur | Minyon ve sacrifice ekonomisini sağlam tutar. |
| Core | Bone Shield | 3s: minyon kalkan olur, hasar absorbe → Charge+1 | Süre buffı | 3s → 5s ile savunma yatırımı gerçek karşılık alır. |
| Advanced | Soul Siphon Totem | 8s totem: 5m çevresindeki düşmanlardan ruh emer, 0.5 Charge/sn üretir | Korunur | Minyon ve sacrifice ekonomisini sağlam tutar. |
| Advanced | Mass Sacrifice | Tüm aktif minyonları anında feda eder, her biri konumunda %150 AoE patlama | Korunur | Sacrifice Engine ekseninin en net payoff tuşu. |
| Advanced | Dark Pact | HP -%12 → Charge olmadan minyon çağır. 15s internal CD (sonsuz sacrifice döngüsü engeli) | Bedel görünürlüğü artır | HP kaybı net UI ile gösterilirse risk daha anlaşılır olur. |
| Master | Lich Form | 10s: Summoner ghostal (fiziksel hasar immune, sihirsel/elementel hasar tam alır), minyonlar +%60 | Korunur | Minyon ve sacrifice ekonomisini sağlam tutar. |

### Class Sonucu
| Kalem | Sonuç |
|---|---|
| Tek cerrahi düzeltme | Bone Shield: 3s → 5s |
| En kritik dikkat notu | Army fantasy ile sacrifice fantasy arasındaki seçimleri daha anlamlı kılmak. |
| Revize sonrası beklenen sonuç | Daha net bir commander-sacrificer hibriti olur. |

## Hexer

| Alan | Değer |
|---|---|
| Core Fantasy | Sabır. 10'a gelince sen bitiyorsun. |
| Kaynak | Hex Stacks (0-10/düşman, 5s decay) |
| Burst | HEX CASCADE — bir hedefte 10 stack: tüm düşmanlara 3 stack kopyalanır |
| Güçlü eksen | Hex Overload - Passive sabrı kırıp stack gain’i hızlandırıyor; Hexblast çıkışı çok baskın hale geliyor. |
| Zayıf halka | Cursed Mirror - Rakibe bağımlı ve reaktif; diğer bütün slotlar gibi proaktif stack üretmiyor. |
| Overlap riski | Haunt - Summoner benzeri takipçi/harici ajan hissi veriyor. |
| Rotation-Lock | High |
| Gelişim hedefi | Rotation lock’ı azaltıp stack-spread ve delayed detonation oyununu açmak. |
| Gelişim sonrası class hissi | Sadece tek hedef stack-bomb değil; oda geneline bulaşan gerçek curse specialist olur. |

### Build Eksenleri
| Eksen | Skill paketi |
|---|---|
| Patient DoT | Corruption + Agony + Pandemic + Hexblast |
| Hex Overload | Hex Overload + Empathy + Mass Hex + Hexblast |
| Soul Burst | Soul Bargain + Haunt + Unstable Affliction + Hex Overload |

### Tüm Skill Planı
| Girdi | Skill | Mevcut işlev | Gelişim kararı | Gelişim sonrası ne olur |
|---|---|---|---|---|
| LMB | Hex Bolt | Hızlı projectile, isabette 1 Hex Stack. Overload (7+) hedefte: bolt 2 stack uygular. Hızlı ve sürekli — stack'leri biriktirir. | Korunur | Stack ekonomisinin temel taşı. |
| RMB | Curse Grasp | 2.5m büyük el uzantısı: temas edene anında 2 stack. Pressure Phase (4+) hedefte: 0.5s root da uygular. Yakın mesafe baskısı için; Hexer'ın tek melee anı. | Korunur | Hex stack yayma ve patlatma hattını korur. |
| Core | Corruption | Anında 3 stack + 4s orta DoT | Korunur | Hex stack yayma ve patlatma hattını korur. |
| Core | Agony | Süregelen DoT, 2 stack/tick, durdurulamaz | Korunur | Hex stack yayma ve patlatma hattını korur. |
| Core | Pandemic | Bir hedefteki TÜM stack'ları yakın düşmanlara kopyalar | Rotation kırıcı olarak korunur | Tek hedefe kilitlenmeyi oda geneline çeviren ana buton. |
| Core | Hexblast | 10 stack: %100/stack, kill=CD sıfır, yakına 2 stack yayılır | Korunur | Hex stack yayma ve patlatma hattını korur. |
| Core | Empathy | Lanet: saldırıların %30'u kendine döner. Her refleksiyon → +1 Hex Stack (max 2 stack/sn) | Korunur | Hex stack yayma ve patlatma hattını korur. |
| Core | Haunt | Hayalet bağla: takip+tick+3 stack, 10=otomatik Hexblast | Kimlik ayrımı netleştir | Takip hasarı kalsın ama summon hissi büyütülmesin. |
| Core | Unstable Affliction | Dispel/heal edilirse → patlama+stun | Korunur | Hex stack yayma ve patlatma hattını korur. |
| Core | Enervate | Hız -%50, saldırı hızı -%40, 10s | Korunur | Hex stack yayma ve patlatma hattını korur. |
| Advanced | Mass Hex | Görüntüdeki TÜM düşmanlara 2 stack, HP -%8 | Korunur | Hex stack yayma ve patlatma hattını korur. |
| Advanced | Hex Overload | 6s: hedef Pressure Phase (4+) veya üstündeyken aldığı her hasar +1 stack kazandırır (max 2 stack/sn, per-target cap), bu sürede Hexblast ×2 hasar | Dikkatli korunur | Bu skill baskın ama classı eğlenceli yapan hızlanma da burada; capler sabit kalmalı. |
| Advanced | Cursed Mirror | 8s: sana uygulanan her debuff → düşmana %50 güçle yansır | Reaktif değer buffı | Yansıma gücü %100 olursa niş pick olmaktan çıkabilir. |
| Master | Soul Bargain | HP -%25 → hedefe anında 5 stack | Bedel biraz yumuşatılabilir | HP -25 → -20 küçük rahatlama verir ama gene de tehlikeli kalır. |

### Class Sonucu
| Kalem | Sonuç |
|---|---|
| Tek cerrahi düzeltme | Cursed Mirror: %50 yansıma gücü → %100 |
| En kritik dikkat notu | Rotation lock’ı azaltıp stack-spread ve delayed detonation oyununu açmak. |
| Revize sonrası beklenen sonuç | Sadece tek hedef stack-bomb değil; oda geneline bulaşan gerçek curse specialist olur. |

## Özel Tasarım Soruları

| Soru | Cevap |
|---|---|
| 1) Shadowblade vs Ronin | Esas sorun tema çakışması; oynanış farkı hâlâ var. Shadowblade’i pratikte ayıran tek ana mekanik Combo Point → finisher ekonomisi. |
| 2) Ranger Focus yakın mesafe cezası | Yakın baskı 3 saniye sürerse -60 Focus kaybı yaratır. Bu, 80 Focus’tan 20’ye düşürüp 75+ damage bonusunu kapatır; oda baskısını zorlaştırır ama tek başına classı çökertmez. |
| 3) Brawler 5-Charge fork | Şu an iki yol hâlâ aynı çıktıya yakınsar. Empower ve Overdrive gerçek ayrım kazanmak için Momentum Strike’ın tek-vuruş spender, Unstoppable Force hattının ise sürekli tempo spender olarak ayrışması gerekir. |
| 4) Hexer rotation lock | Lock yaratanlar: Corruption, Soul Bargain, Hex Overload, Hexblast. Lock kıranlar: Pandemic, Mass Hex, Haunt, Empathy, Enervate. |
| 5) Breacher DLC nişi | Gerçek bir niş açar, ama sadece shell economy + pump reload ritmi + armor/stagger kimliğiyle gelirse. Aksi halde Gunslinger Point Blank Execute ile gereksiz çakışır. |