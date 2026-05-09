# SKILL POOLS 10 CLASS - 2026-05-07

Kaynak kanon: `TASARIM/SKILL_SYSTEM_TAXONOMY_2026-05-06.md`

Notlar:
- Tum aktif skill'ler tam olarak STRIKE, ZONE, REACTIVE veya STATE tiplerinden biridir.
- STRIKE: CD 8-15s, anlik, sahada entity birakmaz.
- ZONE: 2-8s sureli entity koyar.
- REACTIVE: HP, Resource, Hit, Kill veya Dodge kosul ailesinden tetiklenir. Dodge tetigi REACTIVE CD reset'ine sebep olamaz.
- STATE: 4-10s surer ve F aktifken bloklanir.
- Summoner STRIKE skill'leri `summon` tag alir ve Ghost Attack'e koyulamaz.
- Hexer STATE skill'leri `accumulation` tag alir.

---

## Warblade

### Identity Passive

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Warblade | Iron Verdict | 3. LMB hit | Mevcut commit-beat proc ile ayni. (Istisna: sadece Warblade'de Identity Passive = commit-beat proc'un kendisidir. Diger 9 class'ta Identity Passive ayri bir layer'dir, commit-beat proc'tan bagimsiz calisir.) |

### KEYSTONE Secenekleri (5 adet)

| Isim | Aciklama |
|---|---|
| Verdict Engine | 3. LMB Iron Verdict yerine RMB'yi de sayar; her 3. kit hit'i Sundered uygular ama F suresi %25 kisalir. |
| Broken Oath | LMB hasari azalir; Broken hedeflere RMB her zaman armor shred uygular ve F ilk vurusunda bonus execute damage kazanir. |
| Heavy Sentence | RMB artik yavas ama daha genis bir commit vurusudur; Sundered hedefte CD'nin bir kismi iade edilir. |
| Iron Tribunal | F aktifken LMB combo zinciri 3 yerine 2 hit'te Iron Verdict proc eder; F bitince kisa self-slow verir. |
| Split Guard | LMB armor shred stack'ler; RMB stack'leri patlatir, F ise tum stack'leri daha buyuk bir tek vurus hasarina cevirir. |

### STRIKE Havuzu (4-5 skill)

| Isim | Aciklama | CD tahmini | Ornek tag |
|---|---|---:|---|
| Sunder Cleave | Oneki konide anlik agir kesis; Sundered uygular, zayif zirhi kirar. | 10s | `warblade, strike, sundered` |
| Iron Charge | Kisa mesafe ileri atilip tek hedefe carpma; Broken hedefte ekstra stagger. | 12s | `warblade, strike, broken` |
| Guard Splitter | Dar cizgide tek seferlik armor shred vurus; shield/armor hedeflere bonus. | 9s | `warblade, strike, armor-shred` |
| Execution Arc | Dairesel anlik balta/kilic savurusu; Sundered hedeflerde daha yuksek hasar. | 14s | `warblade, strike, execute` |
| Faultline Cut | Yere vurulan anlik catlatma; yakin hedefleri kisa stun eder, entity birakmaz. | 15s | `warblade, strike, fracture` |

### ZONE Havuzu (3-4 skill)

| Isim | Aciklama | Sure | Ornek tag |
|---|---|---:|---|
| Iron Break Line | Zeminde 4s kalan kirik hat; gecen dusmanlara armor shred uygular. | 4s | `warblade, zone, armor-shred` |
| Sunder Field | Kucuk alanda 6s baski alani; icindeki Broken hedefler daha fazla LMB hasari alir. | 6s | `warblade, zone, sundered` |
| Verdict Anvil | 3s kalan agir darbe noktasi; gecikmeli patlar ve Sundered hedefleri knock eder. | 3s | `warblade, zone, verdict` |
| Broken Rampart | 5s kalkan enkazi; icinden gecen dusmanlar yavaslar ve ilk hit'te Broken alir. | 5s | `warblade, zone, broken` |

### REACTIVE Havuzu (3 skill)

| Isim | Aciklama | Kosul ailesi | Efekt |
|---|---|---|---|
| Last Plate | Oyuncu dusuk HP'ye inince otomatik demir sok dalgasi cikar. | HP | Yakindaki dusmanlara Sundered + kisa damage reduction. |
| Third Judgment | Ayni hedef 3 hit yiyince otomatik armor break sesi ve vurus efekti tetikler. | Hit | Hedefe Broken + sonraki STRIKE'a bonus hasar. |
| Broken Trophy | Broken hedef oldurulunce kisa sureli savas ritmi kazanilir. | Kill | 3s LMB hit'leri armor shred ihtimali kazanir. |

### STATE Havuzu (2-3 skill)

| Isim | Aciklama | Sure | Ne degisir |
|---|---|---:|---|
| Iron Berserk | Kisa sureli agir saldiri modu. | 6s | LMB armor break uygular, LMB hasari duser; F aktifken bloklanir. |
| Trial Stance | Daha yavas ama daha kesin ritim. | 8s | RMB CD'si artar, ama vurursa Sundered garantiler; F aktifken bloklanir. |
| Splitter Form | Armor shred'i merkeze alan mod. | 5s | LMB stack biriktirir, RMB stack patlatir; F aktifken bloklanir. |

---

## Shadowblade

### Identity Passive

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Shadowblade | Scar Memory | Phase Step + STRIKE | Phase Step sonrasi 1.2s icinde STRIKE +%25 hasar. |

### KEYSTONE Secenekleri (5 adet)

| Isim | Aciklama |
|---|---|
| Veil Debt | Phase Step sonrasi LMB ilk vurus Scar uygular; RMB cooldown'u artar ama Scar'li hedefte daha sert vurur. |
| Memory Cut | Scar Memory bonusu STRIKE yerine RMB'ye de uygulanir; F sadece Scar'li hedeflerde tam sure calisir. |
| Deep Veil | LMB son hit kisa Veil penceresi acar; Veil icinde RMB hedefin arkasina phase eder. |
| Phantom Ledger | F aktifken her Scar patlamasi Phase Step charge'i uretir; LMB temel hasari azalir. |
| Silent Scar | RMB ses/uyari suresi uzar ama Scar uygulanan hedefe karsi sonraki STRIKE guaranteed crit olur. |

### STRIKE Havuzu (4-5 skill)

| Isim | Aciklama | CD tahmini | Ornek tag |
|---|---|---:|---|
| Veil Lunge | Hedefe anlik phase atilimi ve tek vurus; Scar uygular. | 9s | `shadowblade, strike, veil` |
| Scar Sever | Scar'li hedefte buyuk tek vurus; Scar yoksa normal hasar. | 12s | `shadowblade, strike, scar` |
| Phase Needle | Cizgisel anlik delici vurus; arkadan vurursa bonus hasar. | 10s | `shadowblade, strike, phase` |
| Eclipse Stab | Kisa kaybolma sonrasi tek hedefe execute denemesi. | 15s | `shadowblade, strike, execute` |
| Backtrace Cut | Son Phase Step konumuna bagli anlik geri kesis; entity birakmaz. | 13s | `shadowblade, strike, reposition` |

### ZONE Havuzu (3-4 skill)

| Isim | Aciklama | Sure | Ornek tag |
|---|---|---:|---|
| Veil Pocket | 4s kalan karanlik cep; icindeyken LMB ilk hit Scar uygular. | 4s | `shadowblade, zone, veil` |
| Scar Net | 5s kalan isaret agi; icinden gecen ilk hedef Scar alir ve yavaslar. | 5s | `shadowblade, zone, scar` |
| Phase Mark | 3s kalan hedef noktasi; icindeki dusmana STRIKE bonusu verir. | 3s | `shadowblade, zone, phase` |
| Dusk Corridor | 6s dar koridor; icinde dodge sonrasi 1s movement buff verir. | 6s | `shadowblade, zone, veil` |

### REACTIVE Havuzu (3 skill)

| Isim | Aciklama | Kosul ailesi | Efekt |
|---|---|---|---|
| Scar Refund | Scar'li hedef oldurulunce otomatik kisa Veil penceresi acar. | Kill | 1.5s Veil + sonraki LMB bonus hasar. |
| Pain Echo | Oyuncu 4 hit aldiginda en yakin dusmana Scar atar. | Hit | Scar + kisa knockback. |
| Perfect Fade | Perfect dodge sonrasi otomatik kisa damage buff. | Dodge | 2s STRIKE damage +%15; REACTIVE CD resetlemez. |

### STATE Havuzu (2-3 skill)

| Isim | Aciklama | Sure | Ne degisir |
|---|---|---:|---|
| Veilwalk | Gecici phase odakli mod. | 6s | LMB ilk hit Veil cikisi sayilir, RMB kisa teleport olur; F aktifken bloklanir. |
| Scar Trance | Scar patlatma modu. | 7s | Scar'li hedeflere STRIKE CD'si azalir, Scar olmayanlara hasar duser; F aktifken bloklanir. |
| Phantom Tempo | Daha hizli ama daha riskli saldiri ritmi. | 5s | Dodge sonrasi LMB hizlanir, gelen hasar artar; F aktifken bloklanir. |

---

## Elementalist

### Identity Passive

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Elementalist | Element Affinity | RMB cast | %15 ihtimalle mevcut element branded mob'u retetikler. |

### KEYSTONE Secenekleri (5 adet)

| Isim | Aciklama |
|---|---|
| Rotation Lock | LMB element sirasi sabitlenir; RMB mevcut element brand'ini guclendirir, F sadece son iki elementi patlatir. |
| Triune Brand | LMB her 3. hit'te farkli brand atar; RMB ayni hedefte 2 brand varsa bonus alir. |
| Overcast Cycle | RMB iki element birden cast eder ama Heat benzeri overload resource'u birikir; F overload'u temizler. |
| Pure Element | Run basinda tek element secilir; LMB/RMB hep o brand'i uygular, F daha kisa ama daha yogun olur. |
| Brand Conductor | F, sahadaki brand'leri tek hedefe ceker; LMB resource gain duser, RMB retetik ihtimali artar. |

### STRIKE Havuzu (4-5 skill)

| Isim | Aciklama | CD tahmini | Ornek tag |
|---|---|---:|---|
| Fire Lance | Anlik cizgisel fire vurus; branded hedefte ekstra burst. | 9s | `elementalist, strike, fire-brand` |
| Frost Snap | Hedef noktada anlik soguk patlama; branded hedefi kisa freeze eder. | 12s | `elementalist, strike, frost-brand` |
| Storm Pin | Tek hedefe hizli lightning darbesi; mevcut brand'i retetikleme sansi yuksek. | 10s | `elementalist, strike, lightning-brand` |
| Stone Pulse | Yakinda anlik toprak sok dalgasi; brand'li hedefleri stagger eder. | 13s | `elementalist, strike, earth-brand` |
| Prism Break | Hedefteki son brand'i anlik patlatir; element rotation bir adim ilerler. | 15s | `elementalist, strike, brand` |

### ZONE Havuzu (3-4 skill)

| Isim | Aciklama | Sure | Ornek tag |
|---|---|---:|---|
| Lightning Rod | 6s kalan iletken nokta; yakindaki branded hedefleri aralikli retetikler. | 6s | `elementalist, zone, lightning-brand` |
| Frost Ring | 5s buz halkasi; icindeki dusmanlari yavaslatir ve frost brand surelerini uzatir. | 5s | `elementalist, zone, frost-brand` |
| Ember Well | 4s alev cemberi; icindeki hedeflere fire brand baskisi uygular. | 4s | `elementalist, zone, fire-brand` |
| Earthen Prism | 7s kristal entity; brand'li hedeflerden gelen hit'leri kisa sure yankilar. | 7s | `elementalist, zone, earth-brand` |

### REACTIVE Havuzu (3 skill)

| Isim | Aciklama | Kosul ailesi | Efekt |
|---|---|---|---|
| Element Surge | Resource belirli esigi asinca mevcut element otomatik guclenir. | Resource | Sonraki RMB +%20 brand retetik sansi. |
| Brand Collapse | Brand'li hedef oldurulunce yakindaki ayni brand'ler retetiklenir. | Kill | Kucuk element patlamasi. |
| Guarded Spark | Oyuncu 3 hit aldiginda otomatik lightning nova atar. | Hit | Kisa knockback + mevcut element rotation ilerler. |

### STATE Havuzu (2-3 skill)

| Isim | Aciklama | Sure | Ne degisir |
|---|---|---:|---|
| Overchannel | Daha hizli element cast modu. | 6s | RMB CD'si azalir, resource maliyeti artar; F aktifken bloklanir. |
| Element Lock | Tek elemente kilitlenme modu. | 8s | LMB/RMB sadece mevcut element brand'i uretir; F aktifken bloklanir. |
| Prism Mind | Brand yonetim modu. | 5s | STRIKE mevcut brand'i patlatmak yerine kopyalar; F aktifken bloklanir. |

---

## Ranger

### Identity Passive

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Ranger | Distance Discipline | Range check | 6+ tile mesafede ZONE/STRIKE +%20 hasar. |

### KEYSTONE Secenekleri (5 adet)

| Isim | Aciklama |
|---|---|
| Long Watch | LMB menzili artar; 6+ tile bonusu 8+ tile'a tasinir ama daha buyuk olur. |
| Trap Doctrine | RMB artik trap kurulumunu hizlandirir; F aktifken trap tetikleri precision stack verir. |
| Close Quiver | Mesafe bonusu kalkar; LMB yakin hedefte daha hizli, RMB kisa geri adim kazanir. |
| Marked Horizon | LMB her 4. hit precision mark atar; RMB mark'li hedefte CD iadesi alir. |
| Deadeye Field | F aktifken ZONE/STRIKE uzaklik kontrolu yerine gorus hattina bakar; engel arkasina bonus yoktur. |

### STRIKE Havuzu (4-5 skill)

| Isim | Aciklama | CD tahmini | Ornek tag |
|---|---|---:|---|
| Deadeye Shot | Tek hedefe anlik precision atisi; 6+ tile'da bonus kazanir. | 10s | `ranger, strike, precision` |
| Piercing Arrow | Cizgisel delici ok; uzak hedeflerde daha yuksek hasar. | 11s | `ranger, strike, distance` |
| Snap Snare Shot | Hedefe anlik baglayan ok; trap'li hedefte kisa root. | 12s | `ranger, strike, trap` |
| Falcon Mark | Hedefi anlik isaretler ve hasar verir; sonraki ZONE tetigi bonus alir. | 9s | `ranger, strike, mark` |
| Rain Needle | Secilen noktaya anlik ok yagmuru darbesi; entity kalmaz. | 15s | `ranger, strike, precision` |

### ZONE Havuzu (3-4 skill)

| Isim | Aciklama | Sure | Ornek tag |
|---|---|---:|---|
| Tripwire | 6s kalan tel; gecen ilk dusmani root eder ve precision mark uygular. | 6s | `ranger, zone, trap` |
| Hunter's Perch | 5s kalan nisan alani; icinden atilan STRIKE menzil bonusu kazanir. | 5s | `ranger, zone, distance` |
| Bone Trap | 7s kapan; giren hedefi yavaslatir ve sonraki ok hasarini artirir. | 7s | `ranger, zone, trap` |
| Arrow Lane | 4s dar koridor; icindeki dusmanlar daha kolay precision hit alir. | 4s | `ranger, zone, precision` |

### REACTIVE Havuzu (3 skill)

| Isim | Aciklama | Kosul ailesi | Efekt |
|---|---|---|---|
| Kill Sight | Uzak mesafeden kill alinca otomatik precision buff gelir. | Kill | 3s ZONE/STRIKE projectile speed +%15. |
| Low Quiver | Resource dusuk esige inince kisa reload/aim yardimi tetikler. | Resource | Sonraki LMB resource gain +%30. |
| Roll Aim | Perfect dodge sonrasi otomatik kisa nisan penceresi acar. | Dodge | Sonraki STRIKE +%15 hasar; REACTIVE CD resetlemez. |

### STATE Havuzu (2-3 skill)

| Isim | Aciklama | Sure | Ne degisir |
|---|---|---:|---|
| Deadeye Focus | Hareketi azaltan odak modu. | 6s | LMB hiz duser, ZONE/STRIKE precision hasari artar; F aktifken bloklanir. |
| Trap Runner | Trap yerlesim modu. | 8s | RMB daha hizli trap kurar, LMB menzili duser; F aktifken bloklanir. |
| Farline Stance | Uzak menzil ritmi. | 5s | 6+ tile kontrolu daha sik uygulanir, yakin hedeflere hasar duser; F aktifken bloklanir. |

---

## Gunslinger

### Identity Passive

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Gunslinger | Heat Rhythm | Reload | Reload sirasinda %10 Heat venti. |

### KEYSTONE Secenekleri (5 adet)

| Isim | Aciklama |
|---|---|
| Hot Chamber | LMB Heat uretimi artar; RMB Heat harcayarak burst atar, F overheat yerine vent zinciri kurar. |
| Clean Reload | Reload sirasinda hareket cezasi azalir; Heat Rhythm iki kez tetiklenebilir ama LMB hasari duser. |
| Fan Burst | RMB tek atis yerine kisa burst olur; her mermi Heat uretir, F Heat'e gore uzar. |
| Cold Barrel | Heat mekanigi daha yavas dolar; LMB precision artar, RMB full Heat'te bonus kaybeder. |
| Last Round Law | Sarjorun son LMB atisi bonus hasar verir; RMB son mermiyle kullanilirsa CD iadesi alir. |

### STRIKE Havuzu (4-5 skill)

| Isim | Aciklama | CD tahmini | Ornek tag |
|---|---|---:|---|
| Deadeye Round | Tek hedefe anlik headshot; yuksek Heat'te ekstra hasar. | 12s | `gunslinger, strike, heat` |
| Fan the Hammer | Kisa konide anlik burst; sarjor bosalmaya yakinsa daha guclu. | 10s | `gunslinger, strike, burst` |
| Ricochet Shot | Tek atis seken hasar verir; her sekme Heat uretir. | 11s | `gunslinger, strike, burst` |
| Venting Blast | Anlik yakin patlama; mevcut Heat'i bosaltip hasara cevirir. | 14s | `gunslinger, strike, vent` |
| Reload Kick | Anlik tekme-atis kombosu; reload sonrasi 2s icinde bonus. | 9s | `gunslinger, strike, reload` |

### ZONE Havuzu (3-4 skill)

| Isim | Aciklama | Sure | Ornek tag |
|---|---|---:|---|
| Bullet Line | 4s kalan atis hatti; icindeki LMB mermileri daha hizli gider. | 4s | `gunslinger, zone, burst` |
| Smoke Reload | 5s duman alani; icinde reload hizlanir ve Heat vent sansi artar. | 5s | `gunslinger, zone, reload` |
| Hot Brass Field | 6s sicak kovan alani; dusmanlar icinde kalinca chip damage alir. | 6s | `gunslinger, zone, heat` |
| Killbox | 7s isaretli alan; burst hit'leri icerdeki hedeflere bonus kazanir. | 7s | `gunslinger, zone, burst` |

### REACTIVE Havuzu (3 skill)

| Isim | Aciklama | Kosul ailesi | Efekt |
|---|---|---|---|
| Emergency Vent | Heat ust esige cikinca otomatik vent tetikler. | Resource | Kucuk nova + Heat azalir. |
| Reload Tempo | Reload tamamlaninca otomatik kisa atis buff'i verir. | Resource (reload-complete event -- Resource ailesinin alt-kosulu) | 2s LMB fire rate +%15. |
| Kill Reload | Burst kill alinca otomatik bir miktar sarjor/Heat duzeni kazanilir. | Kill | Sonraki RMB CD'si kismen azalir. |

### STATE Havuzu (2-3 skill)

| Isim | Aciklama | Sure | Ne degisir |
|---|---|---:|---|
| Hot Reload | Gecici reload/Heat modu. | 5s | Heat tukenmez ama sure sonunda zorunlu vent olur; F aktifken bloklanir. |
| Bullet Time | Kisa hassas atis modu. | 4s | LMB yavaslar ama precision ve burst hasari artar; F aktifken bloklanir. |
| Brass Rush | Agresif atis modu. | 7s | RMB CD'si azalir, Heat daha hizli dolar; F aktifken bloklanir. |

---

## Ravager

### Identity Passive

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Ravager | Carnage Pulse | HP threshold | HP <%50 iken tum Bleed tick'ler +%30. |

### KEYSTONE Secenekleri (5 adet)

| Isim | Aciklama |
|---|---|
| Blood Forward | LMB Bleed uygulama sansi artar; HP <%50 iken RMB hook daha kisa CD alir. |
| Hook Hunger | RMB hook her zaman hedefi ceker; LMB hasari duser, F hook'lu hedeflerde daha sert patlar. |
| Open Wound | LMB Bleed stack'leri daha uzun surer; RMB stack patlatir ama yeni Bleed uygulamaz. |
| Redline Fury | HP dusukse F daha erken acilir; F aktifken LMB self-damage yerine lifesteal kazanir. |
| No Retreat | Geri hareket cezalanir; ileri hareket sonrasi LMB/RMB aggression bonusu alir. |

### STRIKE Havuzu (4-5 skill)

| Isim | Aciklama | CD tahmini | Ornek tag |
|---|---|---:|---|
| Hookslam | Hedefi anlik cekip yere vurur; Bleed uygular. | 13s | `ravager, strike, hook` |
| Gut Rip | Tek hedefe agir kesik; mevcut Bleed tick'lerini anlik odullendirir. | 12s | `ravager, strike, bleed` |
| Ravage Leap | Kisa ileri sicrama ve tek vurus; dusuk HP'de bonus aggression. | 10s | `ravager, strike, aggression` |
| Chain Breaker | Cizgisel zincir darbesi; hook'lu hedefte daha yuksek stagger. | 11s | `ravager, strike, hook` |
| Blood Verdict | Bleed'li hedefe execute denemesi; entity birakmaz. | 15s | `ravager, strike, bleed` |

### ZONE Havuzu (3-4 skill)

| Isim | Aciklama | Sure | Ornek tag |
|---|---|---:|---|
| Blood Circle | 5s kalan kan alani; icindeki Bleed tick'leri daha hizli calisir. | 5s | `ravager, zone, bleed` |
| Chain Pit | 6s zincir alani; giren ilk hedef yavaslar ve hook hedefi olur. | 6s | `ravager, zone, hook` |
| Carnage Ground | 4s aggression alani; icinde kill almak kisa buff verir. | 4s | `ravager, zone, aggression` |
| Wound Totem | 7s kalan vahsi isaret; yakindaki Bleed hedeflere baski uygular. | 7s | `ravager, zone, bleed` |

### REACTIVE Havuzu (3 skill)

| Isim | Aciklama | Kosul ailesi | Efekt |
|---|---|---|---|
| Red Pulse | Oyuncu HP <%50'ye inince otomatik carnage sok dalgasi cikar. | HP | Yakindaki Bleed hedeflerde tick hasari artar. |
| Blood Prize | Bleed'li hedef oldurulunce aggression buff tetikler. | Kill | 3s hareket hizi + LMB hasari. |
| Pain Hook | Oyuncu 3 hit aldiginda en yakin hedefe otomatik hook mark atar. | Hit | Hedef kisa slow + sonraki RMB bonus. |

### STATE Havuzu (2-3 skill)

| Isim | Aciklama | Sure | Ne degisir |
|---|---|---:|---|
| Carnage Rush | Dusuk savunmali agresif mod. | 6s | LMB Bleed sansi artar, gelen hasar artar; F aktifken bloklanir. |
| Chain Frenzy | Hook odakli mod. | 5s | RMB hook menzili artar, kacarsa self-slow verir; F aktifken bloklanir. |
| Blood Hunger | Bleed tuketim modu. | 8s | STRIKE Bleed stack'lerini hasara cevirir, yeni stack uretimi azalir; F aktifken bloklanir. |

---

## Ronin

### Identity Passive

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Ronin | Sheathe Window | Stillness + STRIKE | 0.6s hareketsizlik sonrasi ilk STRIKE Opened state garantili. |

### KEYSTONE Secenekleri (5 adet)

| Isim | Aciklama |
|---|---|
| Short Sheathe | Sheathe Window 0.6s yerine 0.4s olur; LMB hareketli hasari azalir. |
| Opened Law | LMB her 5. hit'te Opened uygular; RMB sadece Opened hedefte tam hasar verir. |
| Still Blade | Hareketsizken RMB CD'si daha hizli akar; F aktifken ilk STRIKE execute kosulu kazanir. |
| Draw Breath | LMB yavaslar ama her duraklama resource verir; RMB iaido vurusuna donusur. |
| One Cut Path | F tek buyuk iaido vurusuna donusur; LMB/RMB Opened setup'ina daha bagimli hale gelir. |

### STRIKE Havuzu (4-5 skill)

| Isim | Aciklama | CD tahmini | Ornek tag |
|---|---|---:|---|
| Iaido Flash | Anlik cizgisel kesis; Sheathe Window varsa Opened garantiler. | 10s | `ronin, strike, iaido` |
| Still Cut | Hareketsiz kullanilirsa daha sert tek hedef vurus. | 12s | `ronin, strike, stillness` |
| Open Vein | Opened hedefe agir tek kesis; hedef Opened degilse sadece normal hasar. | 11s | `ronin, strike, opened` |
| Cross Draw | X bicimli anlik iki kesis; entity kalmaz. | 14s | `ronin, strike, iaido` |
| Final Measure | Dusuk HP hedefte execute denemesi; setup ister. | 15s | `ronin, strike, opened` |

### ZONE Havuzu (3-4 skill)

| Isim | Aciklama | Sure | Ornek tag |
|---|---|---:|---|
| Stillness Circle | 5s sakin alan; icinde hareketsiz kalmak Sheathe Window'u hizlandirir. | 5s | `ronin, zone, stillness` |
| Opened Line | 4s kalan ince kesik izi; gecen hedefler Opened alir. | 4s | `ronin, zone, opened` |
| Blade Shrine | 7s odak noktasi; icinden cikan ilk STRIKE iaido bonusu kazanir. | 7s | `ronin, zone, iaido` |
| Quiet Step Field | 6s alan; icinde dodge sonrasi kisa stillness sayaci korunur. | 6s | `ronin, zone, stillness` |

### REACTIVE Havuzu (3 skill)

| Isim | Aciklama | Kosul ailesi | Efekt |
|---|---|---|---|
| Calm Return | Resource dusuk esige inince otomatik kisa stillness buff'i verir. | Resource | 2s boyunca Sheathe Window daha hizli dolar. |
| Opened Finish | Opened hedef oldurulunce sonraki STRIKE hazirlik bonusu alir. | Kill | 3s iaido hasari +%15. |
| Near Miss Draw | Perfect dodge sonrasi otomatik draw buff'i gelir. | Dodge | Sonraki STRIKE daha hizli cikar; REACTIVE CD resetlemez. |

### STATE Havuzu (2-3 skill)

| Isim | Aciklama | Sure | Ne degisir |
|---|---|---:|---|
| Iaido Stance | Cekis odakli kisa mod. | 6s | Sonraki STRIKE daha hizli ve Opened etkili olur; F aktifken bloklanir. |
| Still Mind | Hareketsizlik odakli mod. | 8s | Hareket etmezken LMB/RMB guclenir, hareket edince bonus kaybolur; F aktifken bloklanir. |
| Opened Flow | Opened hedeflere zincir tempo modu. | 5s | LMB Opened hedefte resource verir, diger hedefte hasar duser; F aktifken bloklanir. |

---

## Brawler

### Identity Passive

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Brawler | Crack Cadence | 4. LMB hit | 4. LMB combo sonrasi 0.8s tum hit Cracked +1. |

### KEYSTONE Secenekleri (5 adet)

| Isim | Aciklama |
|---|---|
| Shatter Rhythm | 4. LMB hit Shattered patlatir; RMB Cracked stack uretmez ama daha sert vurur. |
| Close Quarters | LMB yakin mesafe disinda zayiflar; RMB grapple vuruslari Cracked garantiler. |
| Counter Habit | RMB counter'a donusur; basarili counter sonrasi LMB 0.8s Cracked +1 kazanir. |
| Street Momentum | Dodge sonrasi LMB combo kaldigi yerden surer; F aktifken combo hizlanir. |
| Jawbreaker | F tek buyuk Shattered patlatmasina donusur; LMB/RMB Cracked stack odakli olur. |

### STRIKE Havuzu (4-5 skill)

| Isim | Aciklama | CD tahmini | Ornek tag |
|---|---|---:|---|
| Counter Blow | Anlik counter vurus; basarili zamanlamada Cracked uygular. | 10s | `brawler, strike, counter` |
| Shatter Uppercut | Tek hedefe yukari yumruk; Cracked hedefi Shattered'a cevirir. | 13s | `brawler, strike, shattered` |
| Elbow Crash | Kisa ileri adim ve tek vurus; yakin hedefte bonus stagger. | 9s | `brawler, strike, brawl` |
| Rib Break | Tek hedefe agir darbe; Cracked stack sayisina gore hasar artar. | 12s | `brawler, strike, cracked` |
| Ground Pound | Anlik yakin sok dalgasi; dusmanlari iter, entity birakmaz. | 15s | `brawler, strike, brawl` |

### ZONE Havuzu (3-4 skill)

| Isim | Aciklama | Sure | Ornek tag |
|---|---|---:|---|
| Fight Ring | 6s kalan kavga alani; icinde yakin hit'ler Cracked sansi kazanir. | 6s | `brawler, zone, brawl` |
| Broken Floor | 4s catlak zemin; icindeki hedefler yavaslar ve Shattered hasari artar. | 4s | `brawler, zone, cracked` |
| Corner Pressure | 5s baski alani; duvara yakin hedeflere ekstra brawl hasari verir. | 5s | `brawler, zone, pressure` |
| Haymaker Lane | 3s dar alan; icinde ilk STRIKE knockback kazanir. | 3s | `brawler, zone, brawl` |

### REACTIVE Havuzu (3 skill)

| Isim | Aciklama | Kosul ailesi | Efekt |
|---|---|---|---|
| Get Up Swing | HP dusuk esige inince otomatik yakin yumruk patlar. | HP | Yakindaki hedeflere Cracked + kisa damage reduction. |
| Combo Fever | 4 hit'lik seri tamamlaninca kisa buff tetikler. | Hit | 2s LMB hiz +%15. |
| Shattered Prize | Shattered hedef oldurulunce kisa brawl buff'i gelir. | Kill | Sonraki STRIKE +%20 stagger. |

### STATE Havuzu (2-3 skill)

| Isim | Aciklama | Sure | Ne degisir |
|---|---|---:|---|
| Brawl Mode | Kisa yakin dovus modu. | 6s | LMB combo hizlanir, menzil duser; F aktifken bloklanir. |
| Counter Guard | Savunmali counter modu. | 5s | RMB counter penceresi genisler, hareket hizi azalir; F aktifken bloklanir. |
| Shatter Drive | Stack patlatma modu. | 8s | STRIKE Cracked'i Shattered'a cevirir, yeni Cracked uretimi azalir; F aktifken bloklanir. |

---

## Summoner

### Identity Passive

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Summoner | Soul Bond | Minyon olumu | Minyon oldugunde 1s i-frame penceresi. Abuse cap: ICD 4s (4s icinde sadece bir kez tetiklenir); 0.5s icinde gerceklesen grup olumu tek tetik sayilir; boss arenasinda i-frame 0.4s. |

### KEYSTONE Secenekleri (5 adet)

| Isim | Aciklama |
|---|---|
| Soul Choir | LMB soul gain'i azalir; RMB ayni anda iki zayif minyon cagirir, F minyon sayisina gore buyur. |
| Sacrifice Crown | RMB minyon cagirmaz, mevcut minyonu feda eder; LMB sacrifice sonrasi bonus soul kazanir. |
| Loyal Dead | Minyonlar daha uzun yasar; F aktifken minyon olumu Soul Bond yerine hasar dalgasi uretir. |
| Grave Economy | LMB her 5. hit soul orb uretir; RMB maliyeti artar ama daha guclu minyon cagirir. |
| Hollow Commander | F minyonlara emir moduna donusur; LMB/RMB direkt hasari azalir, minyon hasari artar. |

### STRIKE Havuzu (4-5 skill)

| Isim | Aciklama | CD tahmini | Ornek tag |
|---|---|---:|---|
| Wraith Pounce | Gecici ruh minyonu anlik hedefe atlayip tek vurus yapar; kalici entity birakmaz. | 10s | `summoner, strike, summon, no-ghost` |
| Soul Spear | Bir minyon ruhunu anlik mizraga cevirir; sacrifice varsa bonus. | 12s | `summoner, strike, summon, sacrifice, no-ghost` |
| Bone Hand | Kisa sure beliren ruh eli tek hedefi vurur ve kaybolur. | 9s | `summoner, strike, summon, soul, no-ghost` |
| Grave Burst | Minyon konumundan anlik patlama; minyon yoksa oyuncu konumunda zayiflar. | 14s | `summoner, strike, summon, soul, no-ghost` |
| Last Command | Secilen minyona anlik saldiri emri; saldiri bitince minyon kaybolmaz. | 15s | `summoner, strike, summon, command, no-ghost` |

### ZONE Havuzu (3-4 skill)

| Isim | Aciklama | Sure | Ornek tag |
|---|---|---:|---|
| Soul Totem | 7s kalan totem; yakindaki minyonlara hasar ve hareket hizi verir. | 7s | `summoner, zone, soul` |
| Grave Patch | 6s ruh zemini; icindeki minyon olumleri soul orb uretir. | 6s | `summoner, zone, sacrifice` |
| Minion Gate | 5s portal; icinden gecen minyonlar hedefe daha hizli yonelir. | 5s | `summoner, zone, minion` |
| Hollow Ward | 4s koruma alani; minyonlara kisa damage reduction verir. | 4s | `summoner, zone, soul` |

### REACTIVE Havuzu (3 skill)

| Isim | Aciklama | Kosul ailesi | Efekt |
|---|---|---|---|
| Soul Emergency | Oyuncu HP dusuk esige inince en zayif minyon otomatik kendini feda eder. | HP | Kisa kalkan + soul patlamasi. |
| Grave Dividend | Minyonun vurdugu hedef oldugunde otomatik soul odulu kontrol edilir. | Kill | Soul orb uretir ve en yakin minyona kisa hiz verir. |
| Bond Lash | Oyuncu 3 hit aldiginda en yakin minyon otomatik karsi saldiri yapar. | Hit | Kucuk soul vurus + minyon taunt. |

### STATE Havuzu (2-3 skill)

| Isim | Aciklama | Sure | Ne degisir |
|---|---|---:|---|
| Soul Command | Minyon emir modu. | 8s | LMB hedef isaretler, RMB minyonlari odaklar; F aktifken bloklanir. |
| Sacrifice Rite | Feda odakli mod. | 6s | RMB mevcut minyonu patlatir, yeni summon maliyeti duser; F aktifken bloklanir. |
| Hollow Link | Oyuncu-minyon bag modu. | 5s | Minyon hasari artar, oyuncu direkt hasari azalir; F aktifken bloklanir. |

---

## Hexer

### Identity Passive

| Class | Identity Passive | Trigger | Etki |
|---|---|---|---|
| Hexer | Stack Pressure | 5. stack | Hedef 5+ stack'te otomatik Spread: 6m. Bir kez tetiklenir; 6-10 arasi tekrar tetiklenmez. Boss kuralı: boss'ta stack max 5 oldugundan Stack Pressure 5. stack'te tetiklenir; Spread radius boss arenasinda 3m'ye duser; CLASS_STATE_CONTRACT boss kuralina tabidir. |

### KEYSTONE Secenekleri (5 adet)

| Isim | Aciklama |
|---|---|
| Fifth Law | LMB stack uretimi azalir; 5. stack Spread daha guclu olur, RMB Hexed hedefte bonus kazanir. |
| Curse Ledger | RMB artik stack patlatmaz, stack'leri baska hedefe kopyalar; F stack sayisina gore uzar. |
| Withering Count | LMB her 3. hit Hexed uygular; RMB Hexed olmayan hedefte daha zayiftir. |
| Accursed State | STATE aktifken stack cap 5'te kalir ama accumulation hizlanir; F ile modal cakisma global kurala tabidir. |
| Black Arithmetic | F, en yuksek stack'li hedefi merkez alir; LMB/RMB stack dagitimi daha dar alana odaklanir. |

### STRIKE Havuzu (4-5 skill)

| Isim | Aciklama | CD tahmini | Ornek tag |
|---|---|---:|---|
| Curse Spike | Tek hedefe anlik lanet vurus; Hexed stack +1 uygular. | 9s | `hexer, strike, hexed` |
| Black Nail | Hexed hedefe agir tek vurus; 5 stack'e yakin hedefte bonus. | 12s | `hexer, strike, curse` |
| Stack Rend | Mevcut stack'leri anlik hasara cevirir; stack cap'i degistirmez. | 14s | `hexer, strike, stack` |
| Hex Bolt | Cizgisel anlik curse vurus; ilk hedefe Hexed uygular. | 10s | `hexer, strike, hexed` |
| Doom Pin | Dusuk HP Hexed hedefte execute denemesi. | 15s | `hexer, strike, curse` |

### ZONE Havuzu (3-4 skill)

| Isim | Aciklama | Sure | Ornek tag |
|---|---|---:|---|
| Curse Field | 6s lanet alani; icindeki hedeflere aralikli Hexed stack uygular. | 6s | `hexer, zone, hexed` |
| Stack Well | 7s birikim noktasi; stack'li hedefler icinde kalirsa Spread hazirligi artar. | 7s | `hexer, zone, stack` |
| Wither Circle | 5s alan; Hexed hedeflerin hareketini yavaslatir. | 5s | `hexer, zone, curse` |
| Bad Omen Mark | 4s isaretli nokta; icinde olen Hexed hedef stack yayar. | 4s | `hexer, zone, hexed` |

### REACTIVE Havuzu (3 skill)

| Isim | Aciklama | Kosul ailesi | Efekt |
|---|---|---|---|
| Stack Breakpoint | Hedef 5 stack'e ulasinca otomatik curse pulse cikar. | Hit (stack accumulation count -- Hit count ailesine dahil) | Kucuk Spread uyari efekti + yakindaki hedeflere 1 stack. |
| Dying Omen | Hexed hedef oldurulunce otomatik stack yayilimi tetikler. | Kill | En yakin hedefe Hexed +1. |
| Panic Curse | Oyuncu HP dusuk esige inince yakindaki hedeflere lanet atar. | HP | Kisa slow + Hexed +1. |

### STATE Havuzu (2-3 skill)

| Isim | Aciklama | Sure | Ne degisir |
|---|---|---:|---|
| Accumulation Trance | Stack biriktirme modu. | 8s | LMB/RMB Hexed stack daha hizli verir, direkt hasar azalir; F aktifken bloklanir. Tag: `accumulation`. |
| Curse Arithmetic | Stack hesaplama modu. | 6s | STRIKE stack tuketmek yerine stack'i kopyalar; F aktifken bloklanir. Tag: `accumulation`. |
| Wither State | Yavaslatma ve baski modu. | 5s | ZONE icindeki Hexed hedefler daha yavas stack kaybeder; F aktifken bloklanir. Tag: `accumulation`. |
