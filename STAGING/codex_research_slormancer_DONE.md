# The Slormancer Mekanik Arastirma Raporu

## Kaynak indeksi

| ID | Kaynak |
|---|---|
| S1 | Steam store/API: https://store.steampowered.com/app/1104280/The_Slormancer/ |
| S2 | Steam screenshot/video refs: https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1104280/ss_3f44cf82abd3ad677fe416e7786285d1c5d677d9.1920x1080.jpg |
| S3 | Release trailer: https://www.youtube.com/watch?v=ZSQDgjwvulc |
| S4 | Slorm Temple update: https://store.steampowered.com/news/app/1104280/view/3002197411186856531 |
| S5 | Release date / Netherworld notes: https://store.steampowered.com/news/posts/?appids=1104280&enddate=1745744297&feed=steam_community_announcements |
| S6 | Warlords / Legendary / Nether notes: https://store.steampowered.com/news/posts/?appids=1104280&enddate=1730889893&feed=steam_community_announcements |
| S7 | Patch 0.9 balance notes: https://www.slormitestudios.com/patch_0_9_0.php |
| S8 | Slormbuilds crafting/item slots: https://slormbuilds.github.io/posts/crafting/ |
| S9 | Slormbuilds ancestral skill tree WIP: https://slormbuilds.github.io/posts/2448197582014567502/ |
| S10 | Fandom Ancestral Legacy: https://slormancer.fandom.com/wiki/Ancestral_Legacy |
| S11 | Fandom Huntress class page: https://slormancer.fandom.com/wiki/The_Fierce_Huntress |
| S12 | Fandom Slorm Reapers: https://slormancer.fandom.com/wiki/Slorm_Reapers |

## Karsilastirma tablosu

| Alan | Slormancer bulgusu | RIMA karsilastirmasi | Kaynak |
|---|---|---|---|
| Kamera / perspektif | 2D ARPG dungeon crawler. Steam tag tarafinda hem Top-Down hem Isometric sinyali var; resmi screenshot ve trailer, gercek 3D kamera degil, pixel-art ortografik oblique top-down okuyor. Binalar on cephe gosteriyor, zemin net okunuyor, karakterler dikey sprite. Tahmini gorsel aci RIMA'nin 30-35 derece Hades match kararindan biraz daha dik, yaklasik 40-50 derece ARPG/isometric melez. | RIMA Karar #100 icin dogrudan model degil. Kamera okunurlugu alinabilir, fakat aci RIMA'nin 35 derece high top-down lock'unu degistirmemeli. | S1, S2, S3 |
| Sprite / sanat | Steam acikca carefully crafted pixel art diyor. Screenshotlarda karakterler chibiye yakin fakat RIMA'nin 64x64 2.5-3 head-height compact chibi oranindan daha uzun ve slim. 1080p screenshotta oyuncu/NPC gorunen footprint yaklasik 45-70 ekran px yuksekliginde; native sprite kesin degil, ama RIMA 64x64 canvas lock'una birebir kanit degil. | Pixel art ve koyu dungeon palette uyumlu. Karar #74/#100 64x64 chibi korunmali; Slormancer oranlari fazla ince/uzun kalabilir. | S1, S2 |
| Sinif sistemi | 3 oynanabilir sinif: Mighty Knight, Fierce Huntress, Mischievous Mage. Steam, her sinif icin 200+ Ability/Upgrade/Passive, her active Ability icin kendi skill tree, 150+ Ancestral Skill Tree node'u ve 120 Slorm Reaper weapon soyler. Huntress wiki 3 specialization ve support skill yapisini gosteriyor. | RIMA 10 sinifli daha genis roaster. Slormancer derinligi sinif sayisi az oldugu icin tasinmis. RIMA'da ayni derinligi 10 sinifa yaymak scope riskidir. | S1, S10, S11 |
| Skill modifier / Slorm | "Slorm" sabit skill modifier slotu degil; dusmandan kazanilan sonsuz currency gibi calisir. Skill/upgrade/passive ve Ancestral node'lara yatirilir. Patch 0.9, skill mastery'nin kill bazli oldugunu, equipped skill'lerin ayni mastery aldigini ve later skills'in mastery seviyesi dusuk olsa bile Tier 1/2 upgrade secimleriyle basladigini soyluyor. Slormbuilds da Ancestral tree'de 150 node ve Slorm ile node unlock/upgrade mantigini acikliyor. | RIMA icin en guclu ders: skill basina sabit socket degil, "skill mastery unlock -> 2-3 upgrade secimi". Ancak Slormancer'daki sonsuz Slorm grind'i RIMA oda-odul roguelite temposuna agir gelir. | S7, S9 |
| External build modifiers | Slorm Reapers drop edilen, level alan, evrimlesen build-defining weapon'lar. Steam 120 weapon der; wiki ornekleri primary/secondary skill hasarini sifirlayan, cooldown paylastiran veya otomatik crystal firing gibi build'i kokten degistiren etkiler gosterir. Runes, Ultimatums ve Ancestral tree ek katmanlar. | RIMA cross-class/Shadow Echo icin iyi referans: tek item/echo, build davranisini degistirsin. Ama 120 weapon + infinite upgrade RIMA Faz 1 disi. | S1, S7, S12 |
| Loot / itemization | Rarity: Normal, Magic, Rare, Epic, Legendary. Steam 200+ unique legendary affix ve infinite upgrade der. Slormbuilds 10 gear slotu sayar: Helmet, Amulet, Chest, Cape, Belt, Ring, Boots, Gloves, Bracers, Shoulders. Item stat katmanlari Normal/Magic/Rare/Epic/Legendary/Reaper/Mastery/Attribute olarak ayrilir. Nether item ve graft sistemi endgame'de off-slot legendary/stat transferine izin verir. Set bonus kaniti bulamadim; design daha cok affix/crafting/graft merkezli. | RIMA'da 4 equip slotu ve oda odulu yeterli. Slormancer loot katmani RIMA icin full copy degil, sadece 1 legendary affix + 1 graft benzeri meta craft alinabilir. | S1, S6, S8 |
| Roguelite mi ARPG mi | Temel kimlik persistent ARPG. Steam "Collection and Progression" vurgular; skills, items, Slorm Reapers ve 3 class arasinda surekli switch var. Procedural adventures, Battlefield, Temple, Forge, Netherworld roguelite kokusu verir ama permadeath/run reset yok. Death endgame'de run reward kaybina donebilir; karakter progression kalir. | RIMA run-based oda akisi Slormancer'dan daha roguelite. Slormancer'i roguelite model degil, ARPG progression/loot modeli olarak kullan. | S1, S4, S5 |
| Combat feel | Steam fast-paced gameplay, screenshakes, damage numbers, bindable controls, controller support der. Patch 0.9 attack speed'in animation speed'i artirdigini, cooldown reduction cap'ini, Fast Skill tag'inin 0.3s cooldown paylasimini anlatir. Huntress Tumble hareket/dash islevi gorur. Global Hades dash i-frame, hitstop veya cancel-window sistemi icin net kaynak bulamadim. | RIMA game-feel bible (input buffer, dash i-frame, hitstop, shake) daha action odakli. Slormancer'dan alinacak sey cooldown/fast-skill ekonomi ve screenshake option; hitstop/dash tasarimi RIMA kendi lock'undan gelmeli. | S1, S7, S11 |
| Boss / elite tasarimi | Steam 40+ monster/boss affix, unique monsters, elites and bosses der. Warlords update 25 Warlord hedefinden, Cataclysm'lerin boss run boyunca aktif kalmasindan, boss affix tooltiplerinden ve Regeneration/Invulnerability gibi sorunlu affixlerin ayarlanmasindan bahseder. Bu daha cok ARPG affix/stat-check boss tasarimi; Hades gibi temiz faz koreografisi kaniti sinirli. | RIMA boss fazlari ve 3-kanal telegraph standardi Slormancer'dan daha net olmalidir. Slormancer affix sistemi elite oda varyasyonu icin iyi, boss ana tasarimi icin fazla loot-ARPG. | S1, S6 |
| Endgame | Endgame katmanlari: Battlefield Expeditions, Slorm Temple, Great Forge, Warlords/Netherworld, Wrath 10 ve Wrath 10+100 scaling. Temple Pure Slorm/Ultimatums, Forge Runes/material, Netherworld Warlord/Nether item/graft loop verir. Leaderboard icin guvenilir resmi kaynak bulamadim. | RIMA Faz 1 icin sadece 1-2 scaling axis alinmali: room modifier + boss affix + post-run craft. Wrath 10+100 ve Nether graft economy uzun vadeli. | S1, S4, S5, S6, S7 |

## RIMA karar uyumu

- Karar #74 (boyut hiyerarsi 2^n + PPU=64): Slormancer pixel-art okunurlugu uyumlu, ama sprite olcekleri birebir alinmamali. RIMA 64x64 hero, 96/128 mob tier korunur.
- Karar #100 (64x64 chibi + 30-35 derece high top-down): Slormancer'in ARPG oblique kamerasi referans olabilir, fakat aci daha dik okuyor. Kamera lock degismemeli.
- Karar #144 (silahsiz body + Weapon Child SR): Slormancer weapon/build sistemi tematik olarak uyumlu, ancak gorsel olarak tek-sprite weapon holder gibi duruyor. RIMA, Reaper benzeri build-defining weapon etkilerini runtime WeaponSR ile cozebilir.
- Karar #145 (PixelLab Character States augment): Slormancer'daki specialization/support skill ve Reaper evrimleri, Character States ile visual state/phase variant uretimine iyi ilham verir. Replace degil augment mantigi korunmali.

## RIMA adaptasyon tier tablosu

| Tier | Mekanik | Karar uyumu | Codex degerlendirme |
|---|---|---|---|
| S | Active skill'in kendi mini upgrade tree'si | #100/#145 uyumlu | RIMA STRIKE/ZONE/REACTIVE/STATE aktiflerine 2-3 mastery branch olarak dogrudan adapte et. |
| S | Build-defining weapon/echo effect | #144 cok uyumlu | Slorm Reaper'in 120'lik genisligini alma; her sinif icin 2-3 weapon/echo affix yeter. |
| S | Loadout / flexible respec felsefesi | #144/#145 uyumlu | RIMA'da oyuncuyu build denemeye it. Cezali respec yerine run ici secim ve hub loadout. |
| A | Ancestral tree / shared passive web | #100 uyumlu | 150 node yerine 20-30 node meta tree veya run relic pool olarak adapte et. |
| A | Boss/elite affix tooltipleri | #74/#100 uyumlu | Elite rooms icin affix karti kullan; boss faz telegraph'i RIMA standardi ile yeniden yaz. |
| A | Wrath scaling | #100 uyumlu | Wrath 10+100 alma. 3-5 kademeli Heat/Curse scale yeter. |
| A | Nether graft/off-slot legendary transfer | #144 uyumlu | Faz 2+ craft sistemi olabilir. Faz 1'de 4 slot ve 1 legendary effect yeter. |
| B | Slorm infinite currency grind | Kismen uyumlu | RIMA roguelite temposunu bogar. Sadece mastery XP veya Echo dust gibi kisitli formda kullan. |
| B | 10 gear slotu itemization | #144 ile teknik uyumlu ama scope disi | RIMA 4 equip slot lock'unu bozma. Slormancer bu derinligi 3 sinifa yayiyor. |
| X | Kamera acisini Slormancer'a cekmek | #100 ile celisir | RIMA 30-35 derece Hades-match lock korunmali. |
| X | Full 120 Reaper, 200+ affix, infinite upgrade kopyasi | #74/#100/#144 ile teknik mumkun ama scope disi | Faz 1 solo-dev kapsaminda al-ma. Sistem, RIMA'nin oda odulu hizini yutar. |

## Sonuc

The Slormancer, RIMA icin kamera veya boss koreografi modeli degil; buildcraft modeli. En degerli alinacak parca, "az sayida aktif skill + her aktifin kendi upgrade tree'si + build-defining weapon/echo effect + rahat respec" zinciri. RIMA'nin 10 sinif, 64x64 chibi, 30-35 derece kamera, weapon child SR ve Character States lock'lari korunursa Slormancer'dan alinacak en guvenli paket su olur: her skill icin 3 mastery unlock, her sinif icin 2 weapon/echo keystone, elite/boss affix kartlari ve 3-5 kademeli endgame curse. Full ARPG loot economy, 10 gear slotu, 120 weapon ve infinite Slorm grind'i ise RIMA Faz 1 icin scope disi.
