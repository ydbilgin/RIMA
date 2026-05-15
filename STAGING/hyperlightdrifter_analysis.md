# Hyper Light Drifter — Derin Analiz (RIMA + LaurethStudio)

**Oyun:** Hyper Light Drifter (Steam app 257850)
**Studyo:** Heart Machine
**Yonetmen:** Alex Preston (Alx Preston)
**Muzik:** Disasterpeace (Rich Vreeland)
**Cikis:** 31 Mart 2016
**Steam puani:** Very Positive — 93% positive (10.390 EN review)
**Recent:** Very Positive — 90% positive (son 30 gunde 103 review)
**Metacritic:** 84
**Cozunurluk:** 480x270 native (pixel-perfect upscale)
**Tarih (analiz):** 2026-05-16
**Hedef:** RIMA Karar #143 6-layer pipeline + Karar #149+ adayi + LaurethStudio universal pattern

---

## 0. Yonetici Ozeti (TL;DR)

HLD; minimalist pixel art + neon palette + dash-tabanli combat ucgenini bir tek "ses tonu" altinda kilitleyebildigi icin halen referans. Heart Machine'in temel kurali: **480x270 native cozunurluk + flat color taban + ustte gradient/vignette/glow**. Kombat felsefesi: "dash bir kacis degil, kontrol mekanigidir" — Steam'de en cok atif yapilan cumle. Steam topluluk verisi sunlari soyluyor:

- **Sevilen #1:** Combat akiciligi (dash + sword + gun ucgeni). Ornek: 425 helpful upvote'lu yorumda "this has everything I've ever wanted in a 2D action-adventure game".
- **Sevilen #2:** Sessiz anlatim — "There's something about that lack of text that transports you to a different space" (Alex Preston, Unwinnable interview).
- **Sevilen #3:** Disasterpeace muzigi — 140 dakikalik synth-driven ambient + combat sting'leri "rooms boyunca shifting layers" yapisinda.
- **Sikayet #1:** Boss savaslari "visual mess" — ekran sarsintisi + flash + neon bloom carpinca cursor goremiyorsun.
- **Sikayet #2:** Photosensitivity (epileptic seizure riski) — 546 upvote'luk en cok upvote alan yorum bunu uyariyor.
- **Sikayet #3:** Tutorial yokluk + iconic dialog confusion — Steam'de "shard 4 toplaminca upgrade" sistemini herkes kesfedemiyor.

RIMA icin kritik takeaway: **L4 transition + L5 detail + L6 accent** uclusu icin HLD'nin "flat fill + soft glow ustte" rules'u dogrudan adapte edilebilir; **Karar adayi #149-152** onerileri Bolum 8'de.

---

## 1. Visual Direction (RIMA icin kritik)

### 1.1 Cozunurluk + grid felsefesi
HLD native **480x270** cizilir, sonra integer scale ile upscale edilir. Preston (Game Developer interview) bu kararin gerekcesini soyle aciklar: "tight constraints forced deliberate design choices, limiting detail exploration but creating beneficial design rules that shaped the final aesthetic". RIMA 128px native pipeline'i icin paralel: kucuk grid'in dayattigi asama ekonomisini disiplin olarak gor, tile detayini DECAL/L5'e tasi.

### 1.2 Palet daraligi + signature accent
- **Genel mantik:** Her zone tek bir baskin renk ailesini kilitler. "Snowy north / forest west / aquatic east / desert south" + Central Hub — her birinde DISTINCTIVELY different palette (CGMagazine review).
- **Signature accent:** HLD pink/magenta + cyan glow — protagonist'in kabasinda kirmizi pelerin + cyan kilic. **idrawwearinghats.blogspot.com** analizine gore Drifter "split complimentary colour scheme" kullanir: red dominant, light blue tonic, dark blue mediator.
- **Karanlik vs aydinlik kuralı:** "The colours in the bright places compliment one another, while in the darker areas use an analogous colour scheme" (idrawwearinghats). Bright = blue/orange complementary, dark = blue/indigo/aqua analogous.
- **RIMA bagi:** Karar #143 6-layer mimarisinde L6 accent layer'in is tanimi tam olarak HLD'nin glow + signature accent kuraline denk dusuyor.

### 1.3 Flat fill + gradient overlay teknigi
Heart Machine'in en yayilan teknik kurali (Pixel Art Perfection / Aidan Moher kaynak):
- **Taban:** Buyuk flat color blocklari, minimal dithering.
- **Ust katman 1:** Gradient overlay (her zone bir gradient).
- **Ust katman 2:** Vignette (kenar koyulasmasi).
- **Ust katman 3:** Soft light bloom (glowy nodes).

Preston soyluyor: "I embrace color; bright, saturated, neon, muted, whatever works" (Game Developer postmortem). Sword & Sworcery EP'den ilham — "flat color basis, then overlaying large gradients or a vignette on top to introduce subtle complexity".

### 1.4 Karar #143 6-layer'a HLD haritalama

| RIMA Layer | HLD Karsiligi | Aksiyon |
| :--- | :--- | :--- |
| L1 floor base | Flat zone color block | Tek dominant ton, dithering minimum |
| L2 floor variation | Sword & Sworcery flat patches | Floor'a 3-4 farkli flat varyasyon, hue +-5 |
| L3 wall overlay | Decay machinery silhouettes | Wall'a "broken platforms / skeletal remains" silhouette |
| L4 transition | Bright/dark hue donusu | HLD'nin complementary -> analogous gecisini taklit |
| L5 detail | Decals (rhombus motif) | Rhombus + glyph mikro decals |
| L6 accent | Signature pink/magenta glow | Tek bir signature accent renk her room'da |

### 1.5 Silhouette netligi
Hem protagonist hem dusman EXTREMELY basit: gozler disinda yuz yok. idrawwearinghats: "intentional restraint prevents personality imposition, allowing player identification". RIMA Brawler/Warblade icin: facial detay yerine **silhouette + signature accessory** lock'la.

### 1.6 Animasyon frame ekonomisi
Heart Machine "pushed animation frame counts and enemy density as much as gameplay suited" (Game Developer). Ozellikle Drifter'in **3-frame attack** zinciri legendary — windup / hit / recovery. RIMA Warblade v7 icin paralel: 3-segment attack KF+Interp pipeline halihazirda Karar uyumlu.

### 1.7 Camera + game feel
Top-down 3/4 (yaklaski 30°). Hit edildiğinde **mikro hitstop + screen shake**. Sikayet konusu: boss'ta shake fazla. RIMA'nin Feel Toggles memory'si "Shake/Vignette/Hitstop ON" — HLD'nin asirisina dusmemek icin BOSS-SPECIFIC shake amplitude yarisi (0.5x) lock'lanmali.

---

## 2. Combat Feel (RIMA combat v4 icin)

### 2.1 Dort temel hareket
1. **Sword combo:** 3-hit zinciri (uctu da farkli windup), CGMagazine: "the sword doesn't swing as fast as you can press the button" — yani button mash KAPALI, ritim ZORUNLU.
2. **Gun:** Mermi limitli — sword vurusu mermi sarji (ekonomi loop).
3. **Dash:** Kisa mesafe, iframe'li, CHAIN dash ust upgrade ile aciliyor.
4. **Grenade (upgrade):** Bomb ekleme.

### 2.2 Dash felsefesi (en cok atif yapilan)
Steam Review #6 (170 helpful, 18.5h): **"the dash is how you control the combat, and it was a unique perspective"**. Bu cumle HLD'nin combat tasariminin tek-cumle ozeti. Dash bir KACIS degil; dusmana **acidan girme** araci. Kazanma formulu: "moving in for a quick attack, getting out of the way with a dash, and finding another angle to drive in before the enemy has an opportunity to counter".

### 2.3 Iframe + chain dash dengesi
- Launch'ta dash iframe YOKTU. Patch sonrasi eklendi -> community boldu -> 3 gun sonra rebalance (Wikipedia reception).
- Lesson: **Dash iframe'i birinci gun gondermek yerine playtest'e bakarak ekle.**
- Chain dash: ust seviye upgrade — RIMA'nin "dash v2, projectile deflect" projesi (memory: project_combat_architecture) HLD chain dash mantigiyla uyumlu. Limit: ardisik 3 dash sonra fatigue / cooldown.

### 2.4 Hit feedback
- **Damage number YOK.** Renk + frame freeze + shake.
- **Hitstop:** ~50-80ms per impact (community measurement, oyle hissediliyor).
- **Screen shake:** Hafif sword hit, orta gun hit, agir grenade. Boss patlamalarinda asiri.
- **Death animasyonu:** Standing back up animation Steam'de sikayet konusu — "far too long". RIMA icin lesson: respawn animasyon <0.8s tut.

### 2.5 Enemy telegraph
Her dusmanin GOZE CARPAN windup'i var: glow pre-flash, shoulder rotation, ses sinyali. Wikipedia: combat "fluid, demanding, and fair" (GameSpot Kevin VanOrd). "Fair" kelimesinin kosulu: telegraph her zaman gorunur olmasi.

### 2.6 Boss tasarim
- **Phase transitions:** Her boss 2-3 fazli, faz gecislerinde arena degisiyor.
- **Pattern-based:** Dark Souls okul; ezberle + reaksiyon hibrit.
- **Sikayet:** "Boss fights are a visual mess with everything exploding, shaking, screaming and flashing, making it impossible to maintain dashing rhythm as the cursor is barely visible with constant screen shaking" (Steam discussion).
- **Sikayet:** "beating the bosses is not about skill, it is about luck" (azinlik gorus, ama mevcut).
- **RIMA boss tasarimi icin lesson:** Boss arenasinda screen shake + bloom MUTLAKA azaltma toggle'i koy.

### 2.7 Combat ekonomisi (charge-back loop)
Wikipedia: "the player's ammunition instead charges when hitting enemies and objects with the energy sword". Bu LOOP en oven yorumlardan biri:
- Sword vur -> mermi kazan -> mermi at -> riskli durumda dash et.
- RIMA'nin Rage sistemine paralel — Rage kazanmak icin sword combo zorunlulugu HLD ekonomisinden direkt esinlenebilir.

### 2.8 En cok ovulen combat alintilari
- Steam user [425 helpful, 10.8h]: "this has everything I've ever wanted in a 2D action-adventure game" (Zelda meets Bloodborne).
- Steam user [360 helpful, 42.5h]: "This game is brutally hard at first, but makes you feel skilled."
- Steam user [160 helpful, 73.0h]: "Mechanic wise the game is fair. It can be difficult, but never feels unfair."
- Steam user [309 helpful, 5.9h]: "I was so ready to give this game a negative review" — combat ogrenilince fikir degisiyor (LATE-CLICK pattern).

---

## 3. Map / World Design

### 3.1 Hub-and-spoke yapisi
**Central Town** + **4 yon** (north snow / west forest / east aquatic / south desert). Klasik Zelda LttP yapisi. Her yon farkli bir buyuk monster + boss + zone palette.

### 3.2 Discovery + secret yogunlugu
CGMagazine: "The over-world is absolutely littered with things to find and discover". Steam user [193 helpful]: **"secrets densely packed across a vibrant alien world"**. Saklanan icerik:
- Health vials (max HP +1)
- Gear shards (weapon/skill upgrade currency)
- Lore monolitleri
- Ortusturulen kapilar (sword vurusu ile aciliyor)
- Kucuk ledge altinda gizli yollar

### 3.3 Verticality + cliff'ler
HLD top-down olmasina ragmen **elevation** kullaniyor — cliff edges, aşağı atlama, dash ile ust platform gecis. RIMA Karar #143 Asama 2 elevation pair'i icin direkt referans: HLD'de elevation **flat color bloklari + drop-shadow gradient** ile hissettiriliyor (gercek Z yok, fake Z var).

### 3.4 Atmospheric storytelling
"Skeletal remains of previous drifters, broken platforms, and architectural ruin convey technological collapse" (idrawwearinghats). Hicbir text yok; environment "shows" — yani dunyanin gectigi felaketi sadece manzara anlatiyor.

### 3.5 Backtracking + shortcut
- Warp system: kesfedilen lokasyonlar arasinda fast travel.
- Shortcut unlock: ileri zone'lardan geri donen kapilar.
- Steam user [170 helpful]: optional side content "tedious filler compared to core experience" — backtracking ASIRI yapilirsa sikayet ureniyor.

### 3.6 RIMA Map System bagi
Memory `project_map_system.md`: STS2 DungeonMapUI + 3-fork depth. HLD doğrudan bir node-graph degil **continuous open** dunya. RIMA'nin run-based yapisina HLD'den alinabilecek tek sey: **hub world ile ana 3-fork arasinda visual environmental teaser** (biriraktan rate'i artirma).

---

## 4. Audio + Soundtrack (Disasterpeace)

### 4.1 Genel ozellikler
- Toplam: 140 dakika.
- Tarz: minimal synth-driven ambient + occasional rhythmic combat tracks.
- Yapi: "shifting atmospheric layers building throughout each room".
- Sound first, melody second: "leaning heavily on sounds themselves, which often came first, with music written specifically for these sounds instead of vice-versa" (Disasterpeace blog).

### 4.2 Sparse vs dense gecisler
- Hub town: ambient dokusu, cok hafif.
- Combat zone girisi: percussion devreye giriyor — "as variations add percussion, the intent is to create a sense of confrontation".
- Boss climax: "reaching climax towards the end to match the intensity of final battles".

### 4.3 SFX layering
- Sword vurus: kuru, kisa, low-mid frequency.
- Dash: short whoosh + hafif blur SFX.
- Hit alma: bright sting + screen freeze ile sync.
- **Voice acting yok.** RIMA icin zaten plan bu (memory'de teyit).

### 4.4 LaurethStudio Caterpillar/Wingspan icin bag
HLD'nin "sparse ambient + selective rhythmic sting" formulu Wingspan'in cozy ambient hedefiyle KISMEN ortusur. Caterpillar icin daha sicak palet (ahsap perc, akustik) tercih edilse bile **layer-on-room-transition** mekanik aynisi alinabilir.

---

## 5. Storytelling (Silent Narrative)

### 5.1 Sifir text yaklasimi
- **NPC dialog:** Konusma balonu icinde **piktografik comic strip** (UI Breakdown / Medium kaynak).
- **Localization sifir:** Hicbir dile ceviri gerekmiyor — universally understandable.
- **Lore:** Environment + monolith glyph + cinematic cutscene.

### 5.2 Visual symbology
- **Rhombus motif:** Title screen, level architecture, merchandise, UI fioritura — her yerde tekrarlanan tek shape.
- **Mysterious glyphs:** Anlami acik degil ama tutarli alfabe gibi gozukuyor.
- **Color symbolism:** Pink/magenta = power/ancient, cyan = drifter, sari = currency.

### 5.3 Mood through env+music
Steam user [100 helpful]: **"We want something else. We look for a feeling."** — emotional impact analizi. Yalniz + melankolik atmosfer environment + Disasterpeace muzigi ile yaratiliyor, kelime yok.

### 5.4 LaurethStudio universal pattern
HLD pattern Caterpillar/Wingspan icin gec-uygulanabilir mi?
- **Caterpillar:** Cozy oyun olarak text-light olabilir ama tum text'siz risky — toy/skill onboarding dile dayanır.
- **Wingspan-tarzi:** Yabani hayvan etkilesimleri icin pictographic UI ALINABILIR. **LOCK karari: Studio universal "max 1 ekranda 12 kelime" rule.**
- **CircuitBreaker:** Aksiyon agirlikli — RIMA ile ayni pipeline'da skill text gerekir.

### 5.5 RIMA'da kullanmama gerekcesi
RIMA'da skill kart sisteminde **80 cross-class skill** var (memory: project_cross_class_skills). Bu kadar mekanik derinlikte text-zero gitmek confusion ureniyor. **HLD'nin sessiz anlatimini RIMA REJECT etmeli** (Bolum 9'da tekrarliyorum).

---

## 6. Difficulty + Accessibility

### 6.1 Launch difficulty + backlash
- Launch'ta dash iframe yok, healing manual, respawn animation uzun.
- Steam discussions ilk hafta: "Way too difficult to be enjoyable".
- Patch 1: dash iframe + i-frame eklendi.
- 3 gun sonra rebalance (Wikipedia reception).

### 6.2 Accessibility patches
- Photosensitivity uyari ekrani eklendi.
- Brightness slider geldi.
- Screen shake intensity slider yok (eksiklik — RIMA icin lesson).

### 6.3 Hard but fair tradeoff
Preston Unwinnable: **"I don't wanna water down the experience...it's still a hard game. It's a pretty hard game, and I don't think we need to be apologetic about that."**

### 6.4 Inverse difficulty curve
Steam discussion: "the difficulty gets easier as you progress - some people enjoy being challenged for the whole game, but this one only gets easier as it goes on". Boss snap-difficulty ust seviyede dustugu icin SON saatler tatmin azaliyor.

### 6.5 RIMA difficulty mode plani icin etki
Memory `project_rima_backlog.md`: "Difficulty modes (Ph 3+)". HLD'den 3 lesson:
1. **Day-1 dash iframe lock:** Iframe ekle (tartismayi sonradan acmak yerine bastan dahil et).
2. **Screen shake amplitude slider:** SHIP DAY-1 — accessibility.
3. **Boss ortalamasi run boyu duz tut** — inverse curve'e dusme.

---

## 7. Steam Reviews — En Cok Soylenenler

### 7.1 Top 5 Begenilen
1. **Combat akiciligi (dash + combo + chain).** Steam user [425 helpful, 10.8h]: "this has everything I've ever wanted in a 2D action-adventure game". Neden begenilmis: dash bir defansif ve ofansif arac olarak ayni mekanikte.
2. **Atmosfer + muzik kombinasyonu.** Steam user [128 helpful, 155.5h]: "this was a game so cool that my mind was blown simply watching intro". Disasterpeace + neon palet sinerji.
3. **Sessiz anlatim & dunya merak.** Steam user [193 helpful, 5.7h]: "secrets densely packed across a vibrant alien world". Lore yerine environment puzzle.
4. **Hard-but-fair denge.** Steam user [160 helpful, 73h]: "Mechanic wise the game is fair. It can be difficult, but never feels unfair." Soulslike ile karsilastirma.
5. **Beceri kazanma hissi (mastery loop).** Steam user [360 helpful, 42.5h]: "This game is brutally hard at first, but makes you feel skilled." Late-click pattern: "I was so ready to give this game a negative review" (309 helpful) sonra cevriliyor.

### 7.2 Top 3 Sikayet
1. **Photosensitivity / epileptic risk.** Steam user [546 helpful, 0.7h refunded]: **"if you have even the slightest amount of photosensitivity, this will hurt"**. EN COK upvote olan yorum. Bloom + flash + shake birikimi.
2. **Boss visual mess.** Steam discussion: "Boss fights are a visual mess with everything exploding, shaking, screaming and flashing, making it impossible to maintain dashing rhythm as the cursor is barely visible".
3. **Healing + iframe yokluk (launch sorunu).** Steam discussion: "the devs seem to have been imitating Zelda but forgot that in those games getting hit gives you a few seconds of invulnerability to recover, whereas here you get hit and die".

### 7.3 "Buy if you like X" patterni
Karsilastirilan oyunlar (siklik sirasi):
- **Zelda: A Link to the Past** (en sik, Preston'in kendi referansi).
- **Dark Souls / Bloodborne** (zorluk + boss ezber + risk-reward).
- **Diablo** (loot/upgrade + isometric — ama HLD top-down).
- **Bastion / Transistor** (Supergiant atmosferi, sanat tonu).
- **Sword & Sworcery EP** (palet, sessizlik, ambient muzik).
- **Nausicaä of the Valley of the Wind** (Studio Ghibli — Wikipedia reception).

---

## 8. RIMA icin BORROW Listesi (Her madde net aksiyon)

Her madde -> spesifik RIMA dosya/sistem + onerilen Karar adayi.

1. **L6 accent layer'a tek-zone-tek-signature-accent kurali ekle.**
   - HLD'de pink/magenta tum oyunda signature; her zone palette degisse bile glow accent KORUNUR.
   - RIMA: faz1 zone Cyan rift, faz2 Mor void, faz3 Turuncu cinder gibi BIR tek accent LOCK.
   - **Karar adayi #149:** "Her RIMA zone'una tek signature accent renk LOCK; L6 accent painter sadece o tek hue'yu basar".

2. **Floor pipeline'a "flat fill + gradient overlay + vignette + soft glow" 4-katman zorunlulugu ekle.**
   - HLD'nin Heart Machine kurali — Sword & Sworcery'den miras.
   - RIMA: `MapLayerOrchestrator.cs` icinde her room render'inda gradient + vignette pass zorunlu.
   - **Karar adayi #150:** "L1+L2 floor uzerine her room'da 1 gradient overlay + 1 vignette pass zorunlu".

3. **Dash combat felsefesini codification et — "dash kontroldur, kacis degil".**
   - HLD'nin tek-cumle combat ozeti.
   - RIMA combat v4 docunda explicit yaz; designer skill yapilarinda dash-into-attack combo'sunu reward et.
   - **Karar adayi #151:** "Dash sonrasi 0.3s pencerede sword vurusu +20% damage; tasarim kuralı".

4. **3-frame attack ekonomisini RIMA Warblade/Brawler icin standart yap.**
   - HLD legendary 3-frame: windup / hit / recovery. Memory'de Warblade v7 zaten 3-segment.
   - Aksiyon: tum melee class'larina ayni 3-frame budget LOCK; PixelLab KF+Interp pipeline'da template hazirlanmali.

5. **Photosensitivity / accessibility slider'larini DAY-1 ship et.**
   - HLD'nin en cok upvote alan yorumu (546) sebep.
   - RIMA: Settings menusune Screen Shake (0-100%), Bloom (0-100%), Flash Reduction (toggle) day-1.
   - **Karar adayi #152:** "Day-1 ship: shake/bloom/flash slider zorunlu; default 70%, max 100%".

6. **NPC dialog UI: pictographic balon + 1-2 kelime hibrit.**
   - HLD full pictographic — RIMA full text. Hibrit orta yol: balon icinde icon + kisa kelime.
   - Avantaji: i18n maliyeti azalir, tonality korunur.
   - Localization memory: hardcoding ZATEN ban — bu kararla ortusur.

7. **Decay/ruin environment storytelling pass'i ekle.**
   - HLD: skeletal previous drifters, broken platforms, machinery ruins.
   - RIMA: Karar #143 6-layer faz sonu "ruin pass" — bir onceki run'dan kalan player skeleton/loot kalintisi env'e gomulur (meta-progression hint).

8. **Combat ammo loop: melee ile ranged sarji.**
   - HLD'nin ekonomi loop'unun kalbi — Steam'de en cok ovulen mekaniklerden.
   - RIMA Rage sistemine direkt adapte edilebilir: melee combo Rage kazandirir, Rage skill harcar.

9. **Mikro hitstop standardi: 50-80ms her impact.**
   - HLD'nin "meaty hit" hissinin teknik kayna.
   - RIMA Feel Toggles: Hitstop ON ama amplitude tablosu eksik — table ekle.

10. **Inverse difficulty curve'unden kac.**
    - HLD'nin uc sikayeti: ileri saatlerde kolaylasiyor.
    - RIMA run-based yapisinda zaten difficulty scale var; lesson: faz3 boss'lari faz1 boss'larindan ALGILANAN olarak daha zor olmali (sayisal degil, telegraph karmasikligi).

11. **Rhombus / signature shape motif lock.**
    - HLD: rhombus her yerde — UI/level/menu.
    - RIMA: zaten Rift Crack VFX (memory'de) bir signature shape adayi. Rift'in kristal kırığını TUM UI'da motif yap (HP bar koselerinde, skill kartlarinda, map node'larinda).

12. **Boss arenasinda kamera shake amplitude'unu yariya dusur.**
    - HLD'nin no.2 sikayeti.
    - RIMA: Boss-specific shake multiplier 0.5x; bloom 0.7x.

---

## 9. RIMA icin REJECT Listesi (Yapma)

1. **Tam sessiz anlatim REDDET.**
   - HLD: 0 text. Cunku sadece kesif + atmosfer odakli.
   - RIMA: 80 cross-class skill, kart text'i, build planlama. Text-zero gitmek skill clarity'i oldurur.
   - Hibrit cozum: dialog pictographic + kelime; skill kart text-rich.

2. **Photosensitivity-asiri bloom REDDET.**
   - HLD'nin 546-upvote'luk en buyuk sikayeti.
   - RIMA Lighting WIP: 0.35 global intensity zaten dusuk — bu KORUNMALI. HLD seviyesinde bloom layer pipeline'a girmemeli.

3. **Inverse difficulty curve REDDET.**
   - HLD ileri saatler kolaylasiyor — kotu MOOD bitisi.
   - RIMA: faz sonu boss'lari TELEGRAPH karmasikligi + add yogunlugu ile zorlasmali.

4. **Long death/respawn animation REDDET.**
   - HLD sikayeti: "far too long standing back up animation".
   - RIMA: respawn animasyon <0.8s LOCK. Run failure -> map screen <2s.

5. **Dash iframe'i sonradan eklemek REDDET.**
   - HLD launch hatasi: iframe yok -> patch -> rollback drama.
   - RIMA: iframe DAY-1 dash mekanigine dahil; balanslama playtest sonrasi DEGIL bastan.

---

## 10. LaurethStudio Universal Pattern (Caterpillar / CircuitBreaker / Wingspan)

HLD'den oyun-bagimsiz, Studio-level cikarim:

1. **Her Studio oyunu BIR signature accent renk LOCK'lar.**
   - HLD = pink/magenta; her zone palette degisse bile bu glow korunur.
   - RIMA = cyan rift, CircuitBreaker = orange spark, Caterpillar = mor butterfly, Wingspan-derivative = yesil canopy.
   - **Universal kural:** Studio brand book'a gir. Oyun trailer'larinda 1 saniye icinde signature accent ile ayirt edilebilmeli.

2. **Native low-resolution + integer scale Studio standardi.**
   - HLD = 480x270; RIMA = 128px native; CircuitBreaker pixel art route alirsa benzer; Caterpillar 2D illustration ise farkli.
   - **Universal kural:** Pixel art yapan her Studio oyunu native cozunurlugu PROD start'inda LOCK'lar; sonradan degistirme yasak.

3. **"Flat fill + gradient overlay + vignette + soft glow" pixel art shader paketi.**
   - HLD'nin teknik DNA'si.
   - **Universal kural:** Studio shared shader library'ye bu 4-pass paket eklenmeli; her oyunda ayni Inspector defaults.

4. **Disasterpeace pattern — "sound first, melody second".**
   - HLD muzik yapim metodu.
   - **Universal kural:** Studio composer brief: zone/room atmosfer dokusu once, melodi sonra. SFX ve muzik ayni timbral aile.

5. **Day-1 accessibility slider paketi.**
   - HLD launch'ta YOKTU, sonradan eklendi — community zarari.
   - **Universal kural:** Settings menusune Screen Shake / Bloom / Flash Reduction / Photosensitive Mode slider'lari Studio template'i.

6. **Pictographic comic-strip dialog (text-light oyunlar icin).**
   - HLD localization-zero stratejisi.
   - **Universal kural:** Casual / cozy oyunlarda pictographic balon birinci tercih; sadece RIMA gibi mekanik-agir oyunlarda text-rich.

---

## 11. Karar Adaylari Ozeti (RIMA backlog'a yazilacak)

| ID | Karar | Hedef Faz | Etkilenen Sistem |
| :--- | :--- | :--- | :--- |
| #149 | Her zone tek signature accent renk LOCK; L6 accent painter sadece o hue | Karar #143 yan-karar | `WallOverlayPainter`, `AccentPainter` |
| #150 | L1+L2 floor uzerine her room 1 gradient + 1 vignette pass zorunlu | #143 yan-karar | `MapLayerOrchestrator`, shader |
| #151 | Dash sonrasi 0.3s pencerede sword vurusu +20% damage | Combat v4 | Skill system, Warblade/Brawler |
| #152 | Day-1 shake/bloom/flash slider zorunlu (default 70%, max 100%) | Settings/Accessibility | UI Settings menu |

---

## 12. Kaynaklar

- Steam store sayfasi: https://store.steampowered.com/app/257850/Hyper_Light_Drifter/
- Steam reviews top-rated: https://steamcommunity.com/app/257850/reviews/?browsefilter=toprated&snr=1_5_100010_
- Steam discussions difficulty: https://steamcommunity.com/app/257850/discussions/0/353916584658391586/
- Steam discussions appeal: https://steamcommunity.com/app/257850/discussions/0/365163686039178637/
- Wikipedia: https://en.wikipedia.org/wiki/Hyper_Light_Drifter
- Unwinnable interview Alex Preston: https://unwinnable.com/2015/12/02/interview-with-alex-preston-the-creator-of-hyper-light-drifter/
- Game Developer / Road to IGF: https://www.gamedeveloper.com/design/road-to-the-igf-heart-machine-s-i-hyper-light-drifter-i-
- Pixel Art Perfection (Aidan Moher): https://aidanmoher.com/blog/2014/04/videogames/pixel-art-perfection-heart-machines-hyper-light-drifter/
- Art direction analysis (idrawwearinghats): http://idrawwearinghats.blogspot.com/2014/04/art-direction-analysis-of-hyper-light.html
- UI breakdown (Medium / Space Ape Games): https://medium.com/the-space-ape-games-experience/hyper-light-drifter-ui-breakdown-c2d9cfe0a192
- CGMagazine review: https://www.cgmagonline.com/review/game/hyper-light-drifter-pc-review/
- Game Maker Blog analysis: https://gamemakerblog.com/2023/02/12/hyper-light-drifter-an-analysis-of-adventure-and-thrills/
- Disasterpeace OST Bandcamp: https://music.disasterpeace.com/album/hyper-light-drifter
- Disasterpeace blog: https://disasterpeace.com/blog/tag.feature.Hyper+Light+Drifter
- Kotaku review: https://kotaku.com/hyper-light-drifter-the-kotaku-review-1769432532
- Game Informer review: https://gameinformer.com/games/hyper_light_drifter/b/pc/archive/2016/04/06/game-informer-hyper-light-drifter-review.aspx
- Metacritic: https://www.metacritic.com/game/hyper-light-drifter/
- Hyper Light Drifter Wiki (Fandom): https://hyperlightdrifter.fandom.com/wiki/Hyper_Light_Drifter

---

**Hazirlayan:** Claude Opus 4.7 (1M context) — RIMA agent
**Sentez baslangic kaynak sayisi:** 14
**Recommended next action:** Karar #149-152 adaylarini PROJECT_RULES gunlugu icin oneri olarak yatirim; RIMA `MapLayerOrchestrator.cs` icine gradient+vignette pass kontrolunu Karar #143 reviewinde ele al.
