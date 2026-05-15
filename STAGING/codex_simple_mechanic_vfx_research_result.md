# Codex Research - Basit Mekanik + Mantik Cercevesi + VFX Solen Oyun Kutuphanesi

Tarih: 2026-05-15  
Profil: yasinderyabilgin  
Kapsam: 2018-2025 agirlikli, manuel/aktif combat, minimal asset ile kod tabanli juice, multi-system combo.  
Not: Resmi satis verisi bulunmayan oyunlar ana kanit tablosunda "kanit" olarak kullanilmadi. Tahmin kaynaklari "tahmin" diye isaretlendi.

---

## 0. Kisa Sonuc

Bu vizyonun net pazari var: oyuncuya 32px/uzak kamera sade karakter ver, ama her vurusun arkasina proc, durum, geometri, kaynak, alan izi ve VFX zinciri koy. Satis emsalleri "artistik pahali animasyon" degil, "basit sprite + sistem karmasasi + ekrani dolduran feedback" tarafinda guclu.

En guclu tasarim formulu:

| Katman | Ne olmali | Neden |
|---|---|---|
| Combat | Manuel aim / dodge / melee veya twin-stick | Auto-attack hissini kirar, klipte oyuncu ustaligi gorunur |
| Basit mekanikler | 3-5 adet okunur rule | Solo dev icin algoritmik, UI/anim maliyeti dusuk |
| Baglayici mantik | Etiket tabanli combo: element, status, shape, terrain mark, momentum | Her yeni skill elle scriptlenmez, data-driven olur |
| Juice | Hit pause, kamera yumrugu, trail, glow, flash, shader dissolve | Asset yerine kodla pahali gorunum |
| Kamera | Uzak/top-down/2.5D | 32px karakterleri affeder, efektleri tasir |

En iyi 3 konsept:

1. **Sigilstorm** - cizilen isaretler + status cascade + proc itemleri.
2. **Gravemark Arena** - yer izleri + gravity yank + momentum chain.
3. **Fuse Choir** - gecikmeli fitil combolari + resource drain + bullet geometry.

---

## 1. Pattern Haritasi - Kanitli Emsaller

Kaynak ilkesi:
- "Resmi" = Steam haberleri, yayinci/dev basini, GamesPress basin bulteni, sirket/PR PDF.
- "Tahmin" = SteamRev, GameRevenueData, GameSensor vb. Bu tabloda kanit olarak kullanilmadi.
- "Red flag" = oyunun satis/pattern degeri var ama RIMA vizyonuna birebir uygulanmayacak taraflari var.

| # | Oyun | Yil | Gelistirici | Aciklanan satis | Kaynak | Takim | RIMA uygunluk |
|---|---:|---:|---|---:|---|---|---|
| 1 | Wizard of Legend | 2018 | Contingent99 | 500K+ | https://gamingcypher.com/wizard-of-legend-celebrates-nocturne-update-and-500000-copies-sold/ | 2 kisi | Yuksek |
| 2 | Risk of Rain 2 | 2019 EA / 2020 | Hopoo Games | 4M+ Steam | https://store.steampowered.com/news/app/632360/view/3030332860924633588 | cekirdek 2 kisi, sonra buyudu | Yuksek pattern, 3D maliyet red |
| 3 | Magicraft | 2024 | Wave Game | 700K+ | https://www.gamespress.com/en-GB/Magicraft-Launches-Free-Collaboration-DLC-with-Dave-The-Diver | kucuk takim | Cok yuksek |
| 4 | Nova Drift | 2019 EA / 2024 1.0 | Chimeric | 400K+ | https://wnhub.io/news/investment/item-45332 | mostly solo | Cok yuksek |
| 5 | Brotato | 2023 | Blobfish | 10M+ | https://www.gamespress.com/Evil-Empire-Set-to-Bring-Spud-Packing-Punches-to-10-Million-Strong-Bro | solo basladi | Pattern yuksek, auto-combat red |
| 6 | Halls of Torment | 2024 | Chasing Carrots | 1M+ Steam | https://store.steampowered.com/news/app/2218750/view/4512135723845697648 | kucuk takim | Pattern yuksek, auto-combat red |
| 7 | Skul: The Hero Slayer | 2021 | SouthPAW Games | 2M+ | https://prtimes.jp/main/html/rd/p/000001463.000010988.html | kucuk takim | Orta-yuksek |
| 8 | Warm Snow | 2022 | BadMudStudio | 2M+ | https://www.superpixel.com/article/530808/warm-snow-has-sold-over-2-million-copies-total | kucuk takim | Yuksek |
| 9 | Gunfire Reborn | 2020 EA / 2021 | Duoyi Games | 1M+ 2020, 2.5M+ sonra | https://thegg.net/press-releases/the-fps-rpg-roguelite-gunfire-reborn-has-now-sold-over-1-million-copies-worldwide/ | buyukce takim | Yuksek pattern, FPS maliyet red |
| 10 | Roboquest | 2023 | RyseUp Studios | 1M+ | https://steamcommunity.com/app/692890/announcements/ | kucuk takim | Yuksek pattern, FPS maliyet red |
| 11 | Neon Abyss | 2020 | Veewo Games | 1M+ | https://newsroom.smilegate.com/en/eng/1753943786 | kucuk takim | Yuksek |
| 12 | Dead Cells | 2018 1.0 | Motion Twin | 10M+ | https://www.gamespress.com/Dead-Cells-Has-Sold-More-Than-10-Million-Copies | 8-10 cekirdek | Pattern yuksek, animasyon maliyeti red |
| 13 | Cult of the Lamb | 2022 | Massive Monster | 4.5M+ | https://www.vgchartz.com/article/462837/cult-of-the-lamb-sales-top-45-million-units/ | >3 | Combat parca degeri var, asset red |
| 14 | Sifu | 2022 | Sloclap | 3M+ | https://www.gamespress.com/en-US/Sifu-Delivers-a-Knockout-Performance-With-Three-Million-Sales---Two-Fr | >3 | Combat feel dersi, 32px degil |
| 15 | Shape of Dreams | 2025 | Lizard Smoothie | 500K+ | https://zebrapartners.com/wp-content/uploads/2025/09/Ext.-NEOWIZ-Shape-of-Dreams-500k-Copies-09-30-2025.pdf | ogrenci/kucuk ekip baslangici | Yuksek pattern |
| 16 | Hades | 2020 | Supergiant Games | 1M+ | https://www.gamedeveloper.com/business/-i-hades-i-has-sold-over-1-million-copies-in-under-two-years | >3 | Klonlama yasak, polish referansi |

### 1.1 Wizard of Legend

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Dash, basic arcana, signature, 4 spell slot, relic, robe stat, elemental typing |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Oyuncu spell slotlarini hareket, kontrol, burst ve finisher olarak dizer. Derinlik, tek tek spelllerin karmasikligindan degil, cooldown/knockback/element/hitstun sirasindan gelir. Iki kisilik ekip icin ders: her spell "etiket + hareket profili + cooldown + hit reaction" datasina indirgenebilir. |
| VFX juice | Element trail, ekran flash, impact burst, dash smear, enemy knockback, renkli projectile shape. |
| Asset minimallik | 8/10 sade. Karakter kucuk, spell efektleri buyuk. |
| RIMA dersi | 32px karakter + buyuk spell VFX pazarlanabilir. Magic fantasy icin pahali animasyon gerekmez. |

### 1.2 Risk of Rain 2

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Aim, dodge/mobility skill, proc item, on-kill trigger, cooldown skill, boss objective |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Itemler birbirini tetikler: kritik vurursun, kritik bleed tetikler, bleed on-hit itemleri besler, on-kill AoE temizler. Basit itemler tek basina okunur; birlikte exponansiyel olur. |
| VFX juice | Proc zincirleri, missile swarm, lightning chain, laser beam, ekran dolusu damage feedback. |
| Asset minimallik | 4/10. 3D oldugu icin RIMA icin fazla maliyetli. |
| RIMA dersi | Sistemi 2D/32px'e tasinabilir: item proc graph ana altin damar. |

### 1.3 Magicraft

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Wand, spell node, modifier, trigger, summon, projectile, relic |
| Ana mekanik sayisi | 7 |
| Combo/synergy | Spell ve modifierlar zincirlenir: projectile sayisi, sekerleme, orbit, summon, cast-on-hit ve split gibi kurallar birlesir. Oyuncu "tarif" bulur; oyunun kalbi editor gibi spell graph'tir. |
| VFX juice | Particle barrage, glowing trails, screen flash, layered projectile storms, renkli elemental patlamalar. |
| Asset minimallik | 8/10. Kamera uzak, karakter sade, asil satilan sey ekranda buyu kalabaligi. |
| RIMA dersi | En yakin modern emsal. RIMA mekanik kutuphanesi icin "spell graph + VFX pool" modeli kullanilabilir. |

### 1.4 Nova Drift

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Ship body, weapon, shield, mod tree, thrust/steer, enemy waves |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Her run'da gemi kimligi parcalardan kurulur. Modlar "projectile behavior", "resource conversion", "summon/drone", "shield trigger" gibi alt sistemleri carpar. |
| VFX juice | Neon line art, laser bloom, particle debris, shockwave, enemy explosion, high contrast silhouette. |
| Asset minimallik | 9/10. Geometrik asset ve shader/particle agirlikli. |
| RIMA dersi | Solo dev icin ideal: sprite yerine parametric shape + VFX. |

### 1.5 Brotato

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Weapon slots, item stats, wave timer, shop, character trait, enemy density |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Karakter traitleri item havuzunu yonlendirir. Basit statlar stacklenir; build "attack speed + lifesteal + range + elemental" gibi okunur. |
| VFX juice | Damage number, hit burst, enemy splat, projectile density, wave clear pop. |
| Asset minimallik | 10/10. Asiri sade. |
| Red flag | Auto-attack. RIMA bunu klonlamaz; sadece item economy ve ucuz asset dersini alir. |

### 1.6 Halls of Torment

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Class, traits, gear, ability evolution, shrine/objective, boss wave |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Trait secimleri ability davranisini degistirir. Gear ve class birlikte "tek tuslu ama kararli" build yaratir. |
| VFX juice | Diablo benzeri karanlik zemin uzerine projectile, AoE rune, crit number, death burst. |
| Asset minimallik | 7/10. Retro render stili maliyeti saklar. |
| Red flag | Auto-combat agir. RIMA manuel combat kullanmali. |

### 1.7 Skul: The Hero Slayer

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Skull swap, skill cooldown, dash, item set, inscription, boss pattern |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Skull kimligi aktif skill setini degistirir; item inscriptionlari build arketipini belirler. "Form degistirme + item set" cok okunur bir kombinasyon dili verir. |
| VFX juice | Slash arc, afterimage, screen shake, boss flash, hit stop, transformation effect. |
| Asset minimallik | 5/10. Pixel art guzel ama animasyon maliyeti RIMA'dan yuksek. |
| RIMA dersi | Form degistirme sistemi kucuk karakterde de calisir. |

### 1.8 Warm Snow

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Flying sword, sect/class, relic slot, melee, dash, elemental proc |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Relic slotlari farkli tetik kurallari verir; ayni relic farkli slota takilinca davranis degisir. Bu, az assetle cok build yaratir. |
| VFX juice | Sword orbit, slash trail, freeze/fire effect, screen pulse, boss hit flash. |
| Asset minimallik | 6/10. 2D ama daha buyuk karakter. |
| RIMA dersi | "Ayni nesne farkli slota takilinca farkli kural" solo dev icin cok verimli. |

### 1.9 Gunfire Reborn

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Gun, element, hero skill, scroll, ascension, co-op revive |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Silah elementleri ve scrolllar status cascade uretir: corrosion + fire = explosion, shock + corrosion = miasma gibi. Basit status ikilileri zengin build verir. |
| VFX juice | Element overlay, crit number, muzzle flash, explosion, status icon, projectile trail. |
| Asset minimallik | 3/10. FPS maliyeti yuksek. |
| RIMA dersi | Element ikili reaksiyon matrisi 2D'ye cok uygun. |

### 1.10 Roboquest

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Fast movement, gun, class perk, elemental weapon, route choice, boss |
| Ana mekanik sayisi | 6 |
| Combo/synergy | FPS movement ile roguelite perk secimi birlesir; silah affixleri class perkleriyle tetiklenir. |
| VFX juice | Muzzle flash, wall-run speed lines, impact spark, enemy pop, colored projectile spam. |
| Asset minimallik | 4/10. Stylized 3D, RIMA icin fazla. |
| RIMA dersi | Tempo: dodge/slide kadar "surekli ileri iten combat" degeri var. |

### 1.11 Neon Abyss

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Run-and-gun, jump, item stack, pet egg, dungeon rule unlock, weapon |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Itemlerin pasif etkileri sinirsiz stacklenir; petler ek tetik ve yan fayda verir. Dungeon kurallari run meta'sini degistirir. |
| VFX juice | Neon bullet, explosion, pet projectile, item aura, screen pop. |
| Asset minimallik | 6/10. Pixel/2D ama cok item asset ister. |
| RIMA dersi | Pet/summon sistemi 32px oyunda VFX generator gibi kullanilabilir. |

### 1.12 Dead Cells

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Weapon pair, skill, roll, status, mutation, biome route |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Silah affixleri status ister; turret burn verir, ana silah burn hedefe crit atar, mutation kill/crit uzerinden kaynak dondurur. |
| VFX juice | Hit stop, slash arc, parry flash, camera kick, enemy dissolve, blood/debris. |
| Asset minimallik | 4/10. Animasyon kalitesi pahali. |
| RIMA dersi | Affix dili ve status-trigger grammar alinabilir, animasyon seviyesi alinmaz. |

### 1.13 Cult of the Lamb

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Melee, dodge, curse, tarot, room reward, base sim |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Combat kismi basit; tarot ve curse run davranisini degistirir. Asil ders combat disi meta ile kisa run ritmini baglamasi. |
| VFX juice | Curse burst, enemy pop, ritual flash, screen tint, squash. |
| Asset minimallik | 3/10. Marka/animasyon/karakter pahali. |
| Red flag | RIMA icin cult/base sim asset yuku fazla. |

### 1.14 Sifu

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Light/heavy, dodge/parry, posture, age/death, weapon pickup, arena hazard |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Yaslanma sistemi basit death loop'a anlam verir; combat combo sistemden cok timing ve posture uzerinden akar. |
| VFX juice | Hit pause, camera framing, impact sound, environment break, slow motion finisher. |
| Asset minimallik | 1/10. RIMA icin maliyetli. |
| RIMA dersi | Vurus hissi: ekrandaki her temas okunmali ve kameradan destek almali. |

### 1.15 Shape of Dreams

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | MOBA-style skill, memory modifier, character, boss, co-op/solo, item |
| Ana mekanik sayisi | 6 |
| Combo/synergy | Skill kitleri MOBA gibi az sayida ama okunur; memory/modifier katmani her skillin davranisini degistirir. |
| VFX juice | Skill telegraph, AoE bloom, projectile trail, burst, hit number, boss phase effect. |
| Asset minimallik | 5/10. 2.5D/modern efekt agirlikli. |
| RIMA dersi | "MOBA skill + roguelite modifier" RIMA icin cok iyi ama 32px sadeleştirilmeli. |

### 1.16 Hades

| Baslik | Not |
|---|---|
| Basit mekanik envanteri | Attack, special, cast, dash, boon, weapon aspect, room reward |
| Ana mekanik sayisi | 7 |
| Combo/synergy | Boon sistemi basit slotlari tanrisal modifierlarla degistirir; duo boonlar kombinasyonu odullendirir. |
| VFX juice | Dash smear, hit flash, god-color palette, impact burst, boss phase spectacle. |
| Asset minimallik | 1/10. RIMA icin fazla pahali. |
| Red flag | Hades klonu yasak. Ders sadece "slot bazli modifier" ve "renk kodlu tanri kimligi". |

---

## 2. Multi-System Simple Mechanic + VFX Spectacle Patternleri

| # | Pattern | En iyi 3 emsal | Solo dev maliyeti | Kombinasyon zenginligi | RIMA notu |
|---|---|---|---:|---:|---|
| 1 | Element kombosu | Wizard of Legend, Gunfire Reborn, Magicraft | 4/10 | 9/10 | 5 element x 5 reaksiyon matrisi yeter |
| 2 | Modifier zinciri | Magicraft, Nova Drift, Dead Cells | 5/10 | 10/10 | Data-driven node graph yap |
| 3 | Item proc synergy | Risk of Rain 2, Brotato, Shape of Dreams | 4/10 | 10/10 | On-hit/on-kill/on-dash/on-status event bus |
| 4 | Skill slot modifier | Hades, Shape of Dreams, Warm Snow | 4/10 | 8/10 | Her skill "base + rune" olur |
| 5 | Form/stance swap | Skul, Warm Snow, Wizard of Legend | 5/10 | 8/10 | 32px karakterde renk/silhouette ile cozulur |
| 6 | Status effect cascade | Gunfire Reborn, Dead Cells, Halls of Torment | 4/10 | 9/10 | burn -> oil -> explosion gibi |
| 7 | Resource sharing/drain | Nova Drift, Hades, Sifu | 3/10 | 7/10 | HP, heat, mana, shield ayni havuza baglanabilir |
| 8 | Spatial trigger / ground mark | Noita pattern, Cult rooms, Magicraft AoE | 5/10 | 9/10 | Iz birak, sonra izleri patlat |
| 9 | Time delay / fuse | Brotato wave economy, bomb skills, Magicraft delayed cast | 3/10 | 8/10 | 1-3 sn gecikme klipte beklenti yaratir |
| 10 | Position-based damage | Sifu, Dead Cells, Wizard of Legend | 4/10 | 7/10 | Arkadan/duvara carparsa/uzaktan bonus |
| 11 | Momentum / combo counter | Sifu, Risk of Rain 2, ULTRAKILL pattern | 4/10 | 8/10 | Hasar degil VFX carpani da artsin |
| 12 | Summon/pet as VFX engine | Neon Abyss, Magicraft, Halls of Torment | 5/10 | 8/10 | Pet animasyonu yerine projectile emitter |

### 2.1 Patternlerin Uygulama Notlari

Element kombosu:
- En az 4 element yeter: fire, ice, shock, void.
- Her element hem status hem VFX paleti verir.
- Ikili reaksiyonlar tasarim maliyetini dusurur: fire+ice = steam blind, shock+void = chain pull.

Modifier zinciri:
- Skill verisi: cast point, projectile shape, damage tag, trigger list.
- Modifier verisi: multiply, split, orbit, delay, seek, repeat.
- VFX verisi skillden degil tagdan gelir; "fire projectile" otomatik fire trail alir.

Item proc synergy:
- Event bus: OnHit, OnKill, OnDash, OnReload, OnStatusApply, OnCrit, OnPickup.
- Her item yalnizca event dinler ve yeni event firlatir.
- Sonsuz zincir riskine internal cooldown gerekir.

Spatial trigger:
- Zemin decal/mark 4-8 saniye yasar.
- Baska skill mark'a degince patlar, doner, seker veya kopyalanir.
- Kamera uzak oldugu icin mark okunurlugu cok onemli.

Time delay:
- Fitil, echo, delayed strike, returning projectile.
- Oyuncu gecikmeyi ogrenince kendini zeki hisseder.
- VFX: sayac halkasi + ses tik + final bloom.

---

## 3. 10 Yeni Oyun Konsepti

### 3.1 Sigilstorm

| Alan | Icerik |
|---|---|
| Hook | 32px buyucu, arenaya cizdigi isaretleri element zincirleriyle patlatir. |
| 3-5 mekanik | Manuel aim bolt; dodge roll; zemin sigil cizme; 4 element status; on-hit relic proc. |
| Combo kural | Her skill hedefe element, zemine sigil etiketi koyar; ayni sigile ikinci element gelirse reaksiyon patlar. |
| Combat akisi | WASD + mouse aim, sol tik bolt, sag tik sigil, shift dodge, Q/E iki skill. Tempo: 8-12 saniyelik combo pencereleri. |
| VFX kancasi | Sigil halkalari once parlar, sonra element rengine gore zincir patlama yapar. |
| MVP sure | 14-16 hafta |
| AI pipeline uygunluk | 9/10. Codex event bus, status matrix, pooling ve data-driven skill editor yazar. |
| Emsal/fark | Magicraft + Wizard of Legend. Fark: spell graph yerine zemin sigil grameri. |
| TCK 228/klon | Kumar yok. Magicraft klonu olmamak icin wand editor degil arena mark dili. |

### 3.2 Gravemark Arena

| Alan | Icerik |
|---|---|
| Hook | Her saldiri zeminde kuvvet izi birakir; izleri cekerek dusmanlari birbirine carptirirsin. |
| 3-5 mekanik | Manuel melee arc; gravity pull; ground mark; wall slam bonus; momentum meter. |
| Combo kural | Marklar birbirine baglanir; pull skill en yakin 3 marki merkez yapar, dusman carpismalari bonus proc uretir. |
| Combat akisi | Twin-stick yakin dovus, hizli dash, 20-40 sn arena dalgalari. |
| VFX kancasi | Siyah-beyaz distortion halkalari, dust burst, carpma aninda freeze frame. |
| MVP sure | 12-14 hafta |
| AI pipeline uygunluk | 8/10. Physics-lite vector math ve collision eventleri algoritmik. |
| Emsal/fark | Sifu hit feel + Risk of Rain proc. Fark: 32px top-down slam puzzle. |
| TCK 228/klon | Kumar yok. Hades/Sifu klonu degil; arena vector sistemi ana kimlik. |

### 3.3 Fuse Choir

| Alan | Icerik |
|---|---|
| Hook | Mermilerin fitilleri var; sen ritmi kurarsin, ekran gecikmeli bir koroya donusur. |
| 3-5 mekanik | Manual shot; fuse timer; echo clone projectile; resource heat; reload burst. |
| Combo kural | Ayni anda patlayan 3+ fuse "chord" olur ve ekstra efekt dogurur: push, freeze, chain, heal. |
| Combat akisi | Mouse aim, aktif reload, heat yonetimi, gecikmeli patlama planlama. |
| VFX kancasi | Patlamadan once halka sayaclari senkronize olur; chord aninda bloom spike. |
| MVP sure | 12 hafta |
| AI pipeline uygunluk | 9/10. Timer scheduler, projectile pool, chord detector net kod isleri. |
| Emsal/fark | Nova Drift + Gunfire Reborn. Fark: delay grameri ana mekanik. |
| TCK 228/klon | Kumar yok. Bullet heaven degil, manuel zamanlama shooter. |

### 3.4 Relic Socket Brawler

| Alan | Icerik |
|---|---|
| Hook | Ayni relic, takildigi slota gore saldiri, dodge veya savunma kuralini degistirir. |
| 3-5 mekanik | Melee combo; relic socket; dodge trigger; status tag; room reward. |
| Combo kural | 4 slot var: Strike, Dodge, Aura, Finish. Relic davranisi slota gore degisir. |
| Combat akisi | Kisa odalar, 2-3 dusman tipi, manuel dodge ve finisher. |
| VFX kancasi | Her slot ekranda farkli katman: Strike trail, Dodge afterimage, Aura ring, Finish flash. |
| MVP sure | 14 hafta |
| AI pipeline uygunluk | 8/10. Slot-based behavior injection cok kodlanabilir. |
| Emsal/fark | Warm Snow + Dead Cells. Fark: relic sayisi az, slot davranisi cok. |
| TCK 228/klon | Kumar yok. Lootbox yok; run ici secim. |

### 3.5 Bloomgun Orchard

| Alan | Icerik |
|---|---|
| Hook | Mermiler dusman oldurmezse tohuma donusur; sonra hasat edip saldiriya cevirirsin. |
| 3-5 mekanik | Manual gun; seed miss mechanic; harvest pulse; enemy lure; plant element. |
| Combo kural | Iskalayan/zemine saplanan mermi tohumdur; ustunden gecince veya skillle hasat edilince yeni projectile patlar. |
| Combat akisi | Arena shooter; oyuncu bilerek iskalayip sonra rota cizer. |
| VFX kancasi | Tohumlar renkli filiz, sonra ekran dolusu petal projectile. |
| MVP sure | 12-15 hafta |
| AI pipeline uygunluk | 8/10. Projectile lifecycle ve ground pickup sistemleri net. |
| Emsal/fark | Atomicrops fikri + Nova Drift. Fark: farming sim yok, combat-only seed grammar. |
| TCK 228/klon | Kumar yok. Asset yuku bitki ikonlariyla sinirli. |

### 3.6 Shatterline Monk

| Alan | Icerik |
|---|---|
| Hook | Yumruklarin dusmanlara catlak cizer; dogru aciyla vurursan butun zincir kirilir. |
| 3-5 mekanik | Melee aim cone; crack mark; angle bonus; parry freeze; shatter finisher. |
| Combo kural | Dusmanlara yonlu catlak etiketi koyulur; ikinci vurus catlak acisina yakinsa shatter cascade baslar. |
| Combat akisi | Az dusman, cok okunur timing; Sifu hissi ama 32px uzak kamera. |
| VFX kancasi | Beyaz catlak shaderi, parry freeze, shatter cam parcaciklari. |
| MVP sure | 13 hafta |
| AI pipeline uygunluk | 7/10. Aci hesaplari ve hit validation kodu kolay, feel ayari zor. |
| Emsal/fark | Sifu + Wizard of Legend. Fark: martial art animasyonu degil mark/angle sistemi. |
| TCK 228/klon | Kumar yok. Sifu klonu degil, top-down crack puzzle. |

### 3.7 Hexrail Courier

| Alan | Icerik |
|---|---|
| Hook | Arena icinde ray hatlari kurup uzerinden ataklarini hizlandiran bir kurye savascisin. |
| 3-5 mekanik | Dash rail; manual slash; parcel bomb; switch node; speed multiplier. |
| Combo kural | Ray uzerinde atilan skill kopyalanir veya hizlanir; node rengi element davranisini belirler. |
| Combat akisi | Hareket once, saldiri sonra; oyuncu rotalar kurar. |
| VFX kancasi | Neon rail trail, speed afterimage, node patlamalari. |
| MVP sure | 14-16 hafta |
| AI pipeline uygunluk | 8/10. Graph/node path sistemi Codex icin uygun. |
| Emsal/fark | Nova Drift + Risk proc. Fark: ship degil top-down dash rail. |
| TCK 228/klon | Kumar yok. Rayline tek-geometri diline dusmemek icin item/status katmani ekli. |

### 3.8 Ember Debt

| Alan | Icerik |
|---|---|
| Hook | Buyuler guc verir ama borc biriktirir; borcu dusmana aktarmazsan sen patlarsin. |
| 3-5 mekanik | Manual spell; debt meter; transfer hit; cleanse zone; risk reward relic. |
| Combo kural | Her guclu skill debt ekler; debt dolmadan hedefe transfer edilirse ekstra status olur. |
| Combat akisi | Agresif gir-cik; oyuncu kaynak riskini surekli dusmana yikar. |
| VFX kancasi | Karakter etrafinda kirmizi catlayan aura, transferde lightning tether. |
| MVP sure | 12 hafta |
| AI pipeline uygunluk | 9/10. Resource state machine, threshold effect, UI bar. |
| Emsal/fark | Hades heat + Gunfire status. Fark: run modifier degil anlik combat borcu. |
| TCK 228/klon | Kumar yok. Risk-odul mekanigi para/lootbox degil run ici. |

### 3.9 Mirror Ammo

| Alan | Icerik |
|---|---|
| Hook | Attigin her mermi aynadan geri doner; asil hasar donus yolunu planlamaktir. |
| 3-5 mekanik | Manual projectile; mirror shard; return path; backstab bonus; ammo recall. |
| Combo kural | Mermi dusmana giderken zayif, geri donerken guclu; aynalar aci degistirir. |
| Combat akisi | Twin-stick aim + recall tusu; oyuncu dusman arkasina rota kurar. |
| VFX kancasi | Cam trail, return beam, backstab flash, shard explosion. |
| MVP sure | 10-12 hafta |
| AI pipeline uygunluk | 8/10. Projectile path ve ricochet hesabi net. |
| Emsal/fark | Wizard projectile + Dead Cells backstab affix. Fark: recall rota ana kimlik. |
| TCK 228/klon | Kumar yok. Klon riski dusuk. |

### 3.10 Pulse Tax

| Alan | Icerik |
|---|---|
| Hook | Her odada dusmanlardan vergi gibi enerji toplar, sonra tek buyuk pulse ile harcarsin. |
| 3-5 mekanik | Manual hit; tax mark; collection radius; pulse spend; enemy elite debt. |
| Combo kural | Markli dusman vuruldukca enerji borcu birikir; oldurmeden once pulse atarsan borc AoE'ye donusur. |
| Combat akisi | Dusmanlari hemen oldurmek yerine hazirlayip toplu patlatma. |
| VFX kancasi | Markli dusmanlar sayacla parlar; pulse tum arenayi halka halka temizler. |
| MVP sure | 12-14 hafta |
| AI pipeline uygunluk | 9/10. Mark/debt/pulse eventleri data-driven. |
| Emsal/fark | Halls trait economy + Risk proc. Fark: auto degil aktif harvest. |
| TCK 228/klon | Kumar yok. Economy kelimesi oyun ici enerji, para degil. |

---

## 4. VFX Juice Teknikleri - Solo Dev Tarifi

| # | Teknik | Ne yapar | Referans | Unity uygulama | Kod maliyeti |
|---|---|---|---|---|---:|
| 1 | Hit pause | Vurusu agir ve net hissettirir | Sifu, Dead Cells | Time.timeScale 0.04-0.08 sn, unscaled coroutine | 30-60 satir |
| 2 | Screen shake | Impact ve patlama agirligi verir | Wizard of Legend | Cinemachine Impulse veya custom camera noise | 30-80 |
| 3 | Camera punch zoom | Kritik anlarda kamerayi tek kare yaklastirir | Sifu | FOV/orthographic size tween | 20-50 |
| 4 | Chromatic aberration spike | Buyuk patlamayi pahali gosterir | Risk of Rain 2 | URP Volume override tween | 30-70 |
| 5 | Hit flash | Dusmanin vuruldugunu tek karede anlatir | Dead Cells | MaterialPropertyBlock ile beyaz renk | 40-80 |
| 6 | Sprite squash/stretch | Kucuk karaktere fizik hissi verir | Brotato/Skul | Transform scale tween | 20-50 |
| 7 | Trail renderer | Hiz, slash ve projectile okunurlugu | Wizard of Legend | TrailRenderer veya VFX Graph strip | 20-80 |
| 8 | Particle burst pool | Her hitte ucuz parca patlatir | Magicraft | Object pool + ParticleSystem.Emit | 50-120 |
| 9 | Bloom intensity spike | Buyuyu parlatir | Nova Drift | URP Bloom intensity tween | 30-60 |
| 10 | Vignette flash | Hasar/tehlike algisi verir | Hades | PostProcess/URP vignette color tween | 30-60 |
| 11 | Damage numbers | Build gucunu gorunur yapar | Brotato | TMP pool, curve tween | 80-150 |
| 12 | Kill confetti | Wave clear odulu verir | Brotato | Small particle burst + random color | 40-80 |
| 13 | Enemy dissolve | Ceset asset ihtiyacini siler | Dead Cells | Shader dissolve threshold | 80-160 |
| 14 | Outline pulse | Onemli hedefi okutur | Halls of Torment | Sprite outline shader veya duplicate sprite | 50-120 |
| 15 | Telegraph ring | Gecikmeli saldiriyi adil yapar | Hades/Roboquest | Decal sprite scale/fill radial | 60-120 |
| 16 | Shockwave sprite | Patlamayi buyuk gosterir | Magicraft | Additive ring sprite scale/fade | 30-80 |
| 17 | Afterimage | Dash ve speed hissi verir | Skul | Sprite snapshot pool, fade material | 60-120 |
| 18 | Palette swap | Element kimligini asset olmadan verir | Wizard of Legend | Material palette index veya LUT | 60-140 |
| 19 | Recoil shake | Silaha agirlik verir | Gunfire Reborn | Weapon sprite/camera local offset tween | 30-70 |
| 20 | Slow-mo finishing | Son dusman/elite kill klip ani yaratir | Sifu | Time dilation + bloom + audio duck | 60-140 |

### 4.1 Juice Kurallari

- Her efekt pool'lanmali; instantiate/destroy combat icinde yok.
- Renkler sistem etiketiyle baglanmali: fire kirmizi, ice camgobegi, shock sari, void mor.
- Kamera shake stacklenmemeli; amplitude clamp kullan.
- Hit pause sadece oyuncu aksiyonuna verilmeli, her tick damage'e verilmemeli.
- VFX okunurlugu icin dusman silhouette her zaman en ust katmanda kalmali.
- 32px karakterde animasyon yerine afterimage, squash, weapon arc ve hit flash daha ucuzdur.

---

## 5. Sentez Onerisi - En Guclu 3

### 5.1 Top 1 - Sigilstorm

| Baslik | Deger |
|---|---|
| Niye top 3 | USP net: zemine ciz, elementle patlat. Magicraft'in spell-crafting doyumunu daha okunur ve daha RIMA-uygun bir arena gramerine indirger. |
| Pazar emsali | Magicraft 700K+ resmi aciklandi; Wizard of Legend 500K+ resmi basinla duyuruldu. |
| 12-16 hf risk | Orta. Risk: sigil okunurlugu ve reaksiyon matrisi balance. Cozum: MVP'de 4 element, 6 reaksiyon, 12 relic. |
| Viral klip ani | Oyuncu 5 sigili zincirler, tek bolt ile tum arenayi renk renk patlatir. |

### 5.2 Top 2 - Gravemark Arena

| Baslik | Deger |
|---|---|
| Niye top 3 | Mekanik az ama fizik hissi cok. 32px karakter, dust/shockwave ile pahali gorunur. |
| Pazar emsali | Risk of Rain 2 4M+ Steam, Sifu 3M+; ikisinin dersini 2D minimal arena sistemine indirir. |
| 12-16 hf risk | Dusuk-orta. Risk: carpma fiziklerinin bugli hissedilmesi. Cozum: tam fizik yerine kinematic vector resolution. |
| Viral klip ani | 20 dusman tek pull ile iki mark arasinda sikisip domino gibi patlar. |

### 5.3 Top 3 - Fuse Choir

| Baslik | Deger |
|---|---|
| Niye top 3 | Delay/fuse sistemi cok ucuz ama klipte dramatik. Oyuncu "simdi patlayacak" beklentisini kurar. |
| Pazar emsali | Nova Drift 400K+ dev-postmortem aktarimi, Gunfire Reborn 1M+ resmi basin. |
| 12-16 hf risk | Dusuk. Risk: gecikme oyuncuya pasif hissettirebilir. Cozum: aktif reload ve recall/premature detonate tusu. |
| Viral klip ani | 12 fitil ayni anda chord olur, ekran bloom spike ile temizlenir. |

---

## 6. Uygulama Cercevesi

### 6.1 RIMA icin sistem mimarisi onerisi

| Sistem | Sorumluluk | Basit veri modeli |
|---|---|---|
| CombatEventBus | Hit/kill/dash/status eventlerini yayinlar | eventType, source, target, tags, position, value |
| StatusMatrix | Element/status reaksiyonunu cozer | statusA, statusB, reactionId |
| AbilityDef | Aktif skill davranisi | cast, projectile, damageTags, vfxTags, cooldown |
| ModifierDef | Skill veya projectile degistirir | trigger, operation, scalar, addTag, spawn |
| VFXRouter | Tag'dan efekt secer | tag, prefab, color, intensity, shake |
| GroundMarkSystem | Sigil/mark/decal lifecycle | id, owner, tags, radius, expiry, charges |
| ProcLimiter | Sonsuz zinciri engeller | procId, cooldown, maxPerFrame |

### 6.2 MVP 16 haftalik plan

| Hafta | Is |
|---:|---|
| 1 | Top-down controller, aim, dodge, hitbox |
| 2 | Enemy spawner, 3 dusman tipi, health/damage |
| 3 | CombatEventBus, damage pipeline |
| 4 | VFX pool, hit flash, screen shake, hit pause |
| 5 | 4 element status, UI ikonlari |
| 6 | StatusMatrix 6 reaksiyon |
| 7 | 6 aktif skill |
| 8 | 12 relic/proc item |
| 9 | Ground mark/sigil sistemi |
| 10 | 2 boss, telegraph ring |
| 11 | Run reward/shop |
| 12 | Balance pass 1, input feel |
| 13 | Juice pass: bloom, trails, afterimage |
| 14 | Content: 10 skill, 24 relic, 4 enemy |
| 15 | Steam demo loop, tutorial text minimum |
| 16 | Bugfix, trailer capture, build polish |

### 6.3 Klon riski kontrol listesi

| Risk | Kirmizi cizgi | Guvenli yol |
|---|---|---|
| Vampire Survivors klonu | Auto-attack, pasif yurumeli combat | Manuel aim, dodge, timing, active reload |
| Hades klonu | Olympian boon, room-by-room isometric slash aynisi | 32px arena, sigil/mark/fuse grammar |
| Noita klonu | Pixel physics sandbox, terrain destruction, wand copy | Deterministic marks/projectiles, sinirli reaksiyon matrisi |
| Brotato klonu | 6 auto weapon + shop aynisi | Manual skill slots + proc relic |
| TCK 228 | Para karsiligi sans/lootbox | Premium oyun, run ici RNG parasiz, kozmetik net satis |

---

## 7. Kaynaklar

- Magicraft 700K+ press: https://www.gamespress.com/en-GB/Magicraft-Launches-Free-Collaboration-DLC-with-Dave-The-Diver
- Magicraft 500K Steam haber: https://store.steampowered.com/news/app/2103140/view/520826430987174047
- Brotato 10M+ press: https://www.gamespress.com/Evil-Empire-Set-to-Bring-Spud-Packing-Punches-to-10-Million-Strong-Bro
- Halls of Torment 1M+ Steam haber: https://store.steampowered.com/news/app/2218750/view/4512135723845697648
- Risk of Rain 2 4M+ Steam haber: https://store.steampowered.com/news/app/632360/view/3030332860924633588
- Wizard of Legend 500K+ press: https://gamingcypher.com/wizard-of-legend-celebrates-nocturne-update-and-500000-copies-sold/
- Skul 2M+ press: https://prtimes.jp/main/html/rd/p/000001463.000010988.html
- Warm Snow 2M+ developer-shared news: https://www.superpixel.com/article/530808/warm-snow-has-sold-over-2-million-copies-total
- Gunfire Reborn 1M+ press: https://thegg.net/press-releases/the-fps-rpg-roguelite-gunfire-reborn-has-now-sold-over-1-million-copies-worldwide/
- Roboquest 1M+ Steam community announcement hub: https://steamcommunity.com/app/692890/announcements/
- Neon Abyss 1M+ Smilegate newsroom: https://newsroom.smilegate.com/en/eng/1753943786
- Dead Cells 10M+ press: https://www.gamespress.com/Dead-Cells-Has-Sold-More-Than-10-Million-Copies
- Cult of the Lamb 4.5M+ announced: https://www.vgchartz.com/article/462837/cult-of-the-lamb-sales-top-45-million-units/
- Sifu 3M+ press: https://www.gamespress.com/en-US/Sifu-Delivers-a-Knockout-Performance-With-Three-Million-Sales---Two-Fr
- Shape of Dreams 500K+ press PDF: https://zebrapartners.com/wp-content/uploads/2025/09/Ext.-NEOWIZ-Shape-of-Dreams-500k-Copies-09-30-2025.pdf
- Hades 1M+ official tweet reported: https://www.gamedeveloper.com/business/-i-hades-i-has-sold-over-1-million-copies-in-under-two-years
- Nova Drift 400K+ postmortem/interview: https://wnhub.io/news/investment/item-45332

---

## 8. Son Karar

RIMA icin en dogru yon "tek mantik dili derin" degil, "az sayida basit mekanigi ortak event/status/mark dilinde birbirine baglayan spectacle sandbox"tir. Asset pahasi dusuk tutulur: 32px karakter, uzaktan kamera, okunur siluet. Pahali hissi veren taraf particle, shader, camera, hit pause ve proc zinciridir.

Implementasyon onerisi: once **Sigilstorm** prototipi. Cunku hem Magicraft/Wizard of Legend pazar kanitina yakin, hem de RIMA'nin 2D top-down pipeline'ina en temiz oturan sistemdir.
