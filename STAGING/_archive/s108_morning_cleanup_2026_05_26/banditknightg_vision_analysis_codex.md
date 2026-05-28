# BanditKnightG Vizyon Analizi - RIMA Transfer

Kaynaklar: `STAGING/twitter_research/2055256885637374172/notes.md`, `contact_sheet.jpg`, `frame_001.png`, `frame_002.png`, `frame_003.png`, `2055256885637374172_1.mp4`.
Onceki `notes.md` bu klibi "vibrant 2.5D pixel/HD hybrid stealth RPG" olarak ozetliyor ve RIMA icin damage/reward popup okunabilirligini, dekoratif interior prop kullanimini borrow; surekli loot/combat numeral spamini reject diye isaretliyor. Bu dosya ayni notu tekrar etmek yerine kompozisyon, VFX, UI ve karar adayi seviyesine indirger.

## Tek-cumle stil ozeti

BanditKnightG, temiz path ile edge-biased prop yogunlugunu ayirip karakterleri doygun outline/VFX katmani uzerinden one cikariyor, cunku sahne detayli olsa bile oyuncu okuma onceligi daima "silhouette -> hit/reward feedback -> hedef gorev" sirasinda kaliyor.

## Frame 1 Analiz

**Kompozisyon:** Taverna/saloon sahnesi genis bir interior arena gibi kurulmus. Sol/alt bolgede yuvarlak masa ve tabureler hero/mid prop agirligini tasiyor; sag ustte merdiven ve duvar edge zone'u yogun. Walkable path orta yatay bantta ve merdiven onunde acik bir koridor olarak kaliyor. Clutter oyuncunun hareket hattina degil, kenar ve masa cevresi adaciklarina yigiliyor.

**Palet:** Sicak kahverengi zemin ve panel duvar ana palet; mavi/beyaz loot beam, yesil slime/loot, sari gorev metni ve altin damage sayisi accent. Mood light kaynaklari mum/avize, dikey mavi beamler ve item sparkle olmak uzere 3 katmanli. Karakterler background'dan daha doygun lacivert/kirmizi ve daha sert outline ile ayriliyor.

**UI:** HP/MP ust-sol, quest objective ust-sag, skill list/hotbar alt-sol, gold/reward alt-sag. UI dort koseye itildigi icin merkez arena temiz kaliyor. Damage feedback sag-alt uca yakin ama combat cluster'a bagli; gold counter devasa beyaz rakamla alt-sagda ve oyuncu ekonomisini surekli hatirlatiyor.

**Damage:** "111" cok buyuk sari-turuncu gradient, siyah stroke ve shadow ile ekrandaki en agresif text. "FEVER!!" keyword daha kucuk ama parlak yesil/beyaz; combo/chain metni kirmizi-altin ile altinda. Hiyerarsi: damage number > keyword > combo label > gold. Trash damage ile crit ayrimi sayi boyutu ve FEVER keyword uzerinden yapiliyor.

**VFX:** Dikey blue loot beamler, beyaz star sparkle, kucuk mavi triangular shard ve radial glow birlikte kullaniliyor. Opacity yuksek ama narrow beam oldugu icin walkable alanin tamamini kapatmiyor. Crit/reward emphasis timing'i hit anindan sonra 0.2-0.5 sn civari parlak peak, sonra sparkle decay gibi okunuyor.

**Siluet:** Player koyu lacivert govde + beyaz/mavi glow ile zemin kahvesinden kopuyor. NPC'lerde beyaz/kirmizi kostum bloklari ve kalin pixel outline var. Bazi loot beamler silhouette'i kisa sureligine yaksa da oyuncu govdesi koyu kontrast sayesinde kaybolmuyor.

**Prop density:** Formul: 1 hero prop cluster (buyuk yuvarlak masa + bar/merdiven), 8-12 mid prop (tabure, item pedestal, kasa/loot, kucuk masa), 12-20 accent (sparkle, coin, candle, slime, kucuk item). Yogunluk kenarlarda ve hero prop cevresinde; orta path yaklasik %45-55 temiz.

**Anim read:** MP4 timeline'da taverna bolumu ardarda loot pickup ve combat feedback gosteriyor. Attack/hit read uzun anticipatory telegraph'ten cok impact feedback'e yaslaniyor: temas aninda mavi-beyaz flash, kisa hit pause hissi, sonra damage/FEVER sayisi ve coin/gold artisi. Player hareketi hizli, feedback ise hareketten daha buyuk.

## Frame 2 Analiz

**Kompozisyon:** Orman/glade sahnesinde merkez-sagda buyuk agac hero prop olarak oyuncu cluster'ini kismen maskeliyor. Beige yol yatay S-curve gibi sahneyi ikiye boluyor ve walkable path'i direkt okuturken, cim/bitki/kaya detaylari yol disina itilmis. Agac gorsel agirlik merkezi; karakterler agacin sol/alt ve govde cevresinde toplanarak prop arkasindan gecme hissi veriyor.

**Palet:** Yesil ana palet cok doygun ama path acik krem renk ile kontrast kuruyor. Accentler pembe cicek, mavi/yasil item name, beyaz hit flash. Mood light kaynaklari soft sun patches ve beyaz combat flash; interior kadar noktasal degil, daha diffuse. Karakterler background'a gore daha sert pixel edge ve daha koyu outline tasiyor.

**UI:** Bu frame'de UI az gorunuyor; merkezde item/loot label stack sahnenin ana feedback'i. Text cluster karakterlerin ustunde, ekran kosesi UI'sindan cok dunya-ici tooltip gibi calisiyor. Bu, loot okunabilirligini artiriyor ama RIMA icin combat kalabaliginda riskli.

**Damage:** Damage number yok; bunun yerine loot/isim text hiyerarsisi var. Yesil "Holy Knight's", beyaz "Level Armor", beyaz "Parasite ax", mavi "Demon ax / Hall Gorgonzola" gibi renk ayrimi rarity veya kategori hissi veriyor. Font buyuklugu karakter boyuna yaklasiyor; bu sadece pickup burst icin kabul edilebilir.

**VFX:** Beyaz hit/pickup glow karakterin onunde patliyor; kenarlarda soft green light bloblar var. Sparkle frame 1 kadar yogun degil, ana overlay text ve white flash. Opacity merkezde cok yuksek; agacla birlikte karakteri bir an kapatiyor ama highlight olarak is goruyor.

**Siluet:** Player/mob outline agac ve cim dokusu karsisinda kalin siyah stroke ile korunuyor. Beyaz summon/ally silueti bilincli olarak overexposed; bu karakter kimliginden cok "aktif efekt/loot state" okumasina hizmet ediyor. Agac govdesi player'in bir kismini kapatsa bile koyu outline ve pembe/koyu kostum farki silueti geri getiriyor.

**Prop density:** Formul: 1 hero prop (buyuk agac), 3-5 mid prop (kaya, cicek cluster, yol parcasi adasi, ufak bitki), 10-18 accent (cim noise, isik lekesi, yaprak/cicek). Bu frame'de prop yogunlugu daha organik ve edge-biased; yolun ortasi temiz, detay yol disi ve agac cevresinde.

**Anim read:** Sampled timeline'da outdoor bolumde hareket path uzerinden okunuyor; attack feedback ic mekandan daha az gorsel patlama kullaniyor. Telegraph daha cok karakter pozisyonu ve hedefe yaklasma ile okunuyor. Hit/pickup aninda beyaz flash ve floating label stack ortaya cikiyor; hit pause hissi taverna kadar sert degil.

## Frame 3 Analiz

**Kompozisyon:** Noble house/kale interior sahnesi daha negatif alanli. Ust duvar bandi genis ve sade; kirmizi perde, tablo ve lavabo edge prop olarak dizilmis. Walkable path merkez-yatay alanda bos; alt-sol bilardo/koltuk, sag duvar ve ust pencere dekorasyonu clutter zone. Combat cluster ust-orta/sagda, mob cluster alt-orta edge'e yakin.

**Palet:** Zeytin-bej duvar, koyu kahve zemin, koyu turkuaz hali ana palet. Accentler kirmizi perde, parlak yesil loot, mavi smoke/afterimage, sari quest ve damage. Mood light daha az: oda ambient'i + karakter/VFX glow + ust-sag UI highlight. Background saturation dusuk, karakter/VFX saturation yuksek.

**UI:** HP/MP ust-sol, quest ust-sag, skill list alt-sol, gold alt-sag. Bu frame UI sistemi frame 1 ile ayni ama sahne daha bos oldugu icin damage "106" ve gold "76,701G" daha baskin. Quest objective'in sari/yesil check kombosu okunakli; combat hedefiyle rekabet etmiyor.

**Damage:** "106" buyuk sari-turuncu gradient, siyah stroke ve altta kirmizi combo marker. Frame 1'deki "FEVER!!" yok; bu da crit/fever ile normal chain arasinda keyword farki oldugunu gosteriyor. Damage text hedef cluster'dan sag-alt tarafa atiliyor, karakterleri kapatmiyor.

**VFX:** Player etrafinda mavi smoke/afterimage ve beyaz sparkle; loot/steal aninda parlak yesil obje. VFX frame 1'e gore daha kontrollu: narrow smoke trail + small sparkle. Crit emphasis yerine stealth/steal feedback gibi okunuyor; opacity orta-yuksek ama sure kisa olmali.

**Siluet:** Koyu lacivert player zeminle yakin degerde, bu nedenle mavi glow ve beyaz edge highlight kritik. Mob/guard alt kisimda kirmizi unlem iconlari ile ayriliyor. Perde gibi kirmizi prop karakter kirmizilariyla rekabet edebilir; RIMA'da class accent renklerinin prop accentleriyle carpismamasi gerekir.

**Prop density:** Formul: 0-1 hero prop (pencere/perde cluster veya bilardo masasi ekran disina yakin), 5-7 mid prop (perde, lavabo, tablo, koltuk, sehpa, bilardo), 6-10 accent (candle, loot, papers, smoke, exclamation). Path temizlik orani frame 1'den yuksek; sahne "stealth room" gibi daha okunakli.

**Anim read:** Video sampling'inde interior infiltration bolumunde temas/hit ardindan mavi afterimage, steal/gold popup ve damage chain birlikte geliyor. Telegraph dusman unlem iconlari ve hedef level/nameplate ile veriliyor. Hit pause cok uzun degil; 60 fps hareket korunurken VFX/text ekran uzerinde daha uzun kaliyor.

## RIMA Transfer Kurallari (BORROW)

### TR-1: Edge-biased prop density
- **Ne:** Walkable path'i %45-60 temiz tut; hero/mid/accent prop yogunlugunu kenar, duvar, obstacle ve objective cevresine yig.
- **Nereye uygular:** Karar #118 Hybrid Tile Composition 4-layer ve Karar #143 6-Layer Map Architecture adayini genisletir.
- **Bedel:** Map Designer paint pass, tile/detail layer validasyonu, collision/path mask kontrolu.
- **Risk:** Fazla temiz path RIMA odalarini bos gosterebilir; L5 detail ve L6 accent bu boslugu path disinda doldurmali.

### TR-2: Hero + mid + accent prop formulu
- **Ne:** Her ekran okunabilirligi icin "1 hero prop + 3-8 mid prop + 8-18 accent" araligini oda tipine gore hedefle; tavern/combat room ust banda, stealth room alt banda yakin kullansin.
- **Nereye uygular:** Karar #135 Procedural+Paint Hybrid ve Karar #75 create_map_object revision adayina baglanir.
- **Bedel:** Prop library taxonomy, room archetype density presets, generator parametresi.
- **Risk:** Sayisal hedefler dogmatik uygulanirsa organiklik azalir; Map Designer override gerekli.

### TR-3: Corner-anchored UI, center-clean combat
- **Ne:** Core UI dort koseye sabitlenir; merkezde sadece dunya-ici anlik feedback kalir. HP/MP ust-sol, objective ust-sag, skill/hotbar alt-sol, reward/gold alt-sag modeli RIMA icin referans alinabilir.
- **Nereye uygular:** Yeni UI layout karar adayi gerekir; Karar #100b Wide Arena FOV ile birlikte degerlendirilmeli.
- **Bedel:** HUD layout prototype, 640x360 reference resolution testleri, localization-safe text sizing.
- **Risk:** RIMA'nin class silhouette ve arena okunurlugu, alt-sol skill list cok buyurse zarar gorebilir.

### TR-4: Damage hierarchy with rare keyword emphasis
- **Ne:** Damage text hiyerarsisi "number > rare keyword > combo/minor label" olmali; FEVER/crit keywordleri sadece elite, crit, execute veya class-defining proc aninda cikmali.
- **Nereye uygular:** Karar #137 VFX Router'a baglanir; CombatEventBus tag'lerine `crit_keyword`, `elite_hit`, `trash_hit` ayrimi ekler.
- **Bedel:** DamageText prefab varyantlari, font/stroke palette, ProcLimiter kurallari.
- **Risk:** Her hit'e keyword eklenirse onceki notes.md'nin reject ettigi numeral clutter RIMA'da da tekrar eder.

### TR-5: Narrow high-brightness VFX, not full-screen wash
- **Ne:** Parlaklik yuksek olabilir ama sekil dar tutulmali: vertical beam, star sparkle, small radial burst, short afterimage. Ekranin tamamini beyazlatma yerine hedef odakli overlay.
- **Nereye uygular:** Karar #137 VFX Router ve Karar #100 Chibi 64x64 + 35 camera ile uyumlu.
- **Bedel:** VFX prefab budget tablosu, alpha/duration caps, mobile readability capture pass.
- **Risk:** 64x64 chibi karakterlerde fazla glow silhouette bible'i bozabilir.

### TR-6: Character saturation over background saturation
- **Ne:** Background paleti zengin ama daha dusuk saturation/value contrast; class silhouette ve weapon accentleri daha doygun, kalin outline/rim light ile ayrilir.
- **Nereye uygular:** Karar #80 Silhouette Bible ve Karar #138 2-layer draw order'u destekler.
- **Bedel:** Class palette bible revizyonu, environment accent color blacklist/limits.
- **Risk:** RIMA class renkleri prop accentleriyle carpisirsa karakter identity zayiflar.

### TR-7: World-label burst only for loot spikes
- **Ne:** Loot/item name stack kullanilabilir ama sadece pickup burst, chest, boss reward veya shop identify aninda; combat sirasinda surekli name stack yasak.
- **Nereye uygular:** Karar #137 VFX Router'a `loot_burst_label` route'u ekler; yeni reward feedback karari gerekebilir.
- **Bedel:** Loot label pooling, rarity color palette, overlap limiter.
- **Risk:** Metin yigilmasi localization ve okunurluk riski tasir; Turkish/English uzun item adlari test edilmeli.

### TR-8: Stealth-room negative space
- **Ne:** Noble house frame'indeki gibi stealth/interaction odalarinda daha fazla negatif alan birak; tehlike iconlari, guard nameplate ve objective okunurlugunu dekor yogunlugunun ustune koy.
- **Nereye uygular:** Karar #143 6-Layer Map Architecture ve Faz 1 stealth/room archetype karar adayina baglanir.
- **Bedel:** Room archetype masks, guard patrol path clearance, visual QC checklist.
- **Risk:** Roguelite combat odalarina ayni bosluk uygulanirsa aksiyon enerjisi duser.

## REJECT (RIMA icin uymayan)

### RJ-1: Surekli buyuk gold/damage ekonomisi
- **Neden uymaz:** RIMA'da her odada devasa gold counter ve damage sayilari class silhouette, enemy telegraph ve arena okunurlugunu bastirir.
- **Yerine:** Karar #137 ProcLimiter ile trash hit minimal, crit/elite hit buyuk; reward counter sadece pickup burst ve run-summary anlarinda buyurur.

### RJ-2: Full loot-name stack spam
- **Neden uymaz:** Frame 2'deki item name cluster gosterisli ama RIMA'nin 640x360 referansinda cok hizli yer kaplar.
- **Yerine:** Rarity icon + tek satir pickup toast; detayli item adlari inventory/loot panelinde.

### RJ-3: Prop accentlerinin class accentleriyle rekabeti
- **Neden uymaz:** Kirmizi perde, yesil slime/loot, mavi glow gibi accentler RIMA class renkleriyle carpistiginda Karar #80 silhouette identity zedelenir.
- **Yerine:** Environment accent palette her biom/class testinde sinirlanir; class-critical hue'lar prop icin dusuk saturation kullanir.

### RJ-4: Close vertical spectacle camera
- **Neden uymaz:** BanditKnightG daha zoomed/spectacle-heavy okuyor; RIMA Karar #100 ve #100b ile 35 derece, wide arena FOV hedefliyor.
- **Yerine:** VFX/damage boyutlari RIMA'nin wide arena capture'inda test edilmeli; close-up polish sadece menu/finisher anlarina ayrilmali.

### RJ-5: UI text-heavy skill list as permanent combat HUD
- **Neden uymaz:** Alt-sol text skill list 1080p'de okunur, fakat RIMA'nin referans cozunurlugunde skill isimleri merkez aksiyondan alan calar.
- **Yerine:** Icon hotbar + tooltip-on-focus; text sadece remap/settings veya tutorial overlay'de.

## Karar Adayi Onerileri (onemli sira ile)

1. **Karar #144-aday - Room Density Formula** - TR-1/TR-2 referansi, Faz 1. 1 paragraph rationale: RIMA'nin procedural+paint hybrid pipeline'i guzel tile uretmekten daha fazlasina ihtiyac duyuyor; her oda tipinin walkable/clutter/hero prop orani kilitlenirse hem generator hem Map Designer ayni okunurluk hedefini tutturur. Bu karar #118, #135 ve #143'u pratik bir QC metrigine cevirir.

2. **Karar #145-aday - Combat Feedback Hierarchy** - TR-4/TR-5/TR-7 referansi, Faz 1. 1 paragraph rationale: Damage/VFX/reward feedback ayni anda patladiginda RIMA'nin class silhouette ve enemy telegraph onceligi bozulabilir. `trash_hit`, `crit_hit`, `elite_hit`, `loot_burst`, `objective_reward` hiyerarsisi Karar #137 VFX Router icinde kilitlenirse polish artarken clutter sinirlanir.

3. **Karar #146-aday - Corner HUD at 640x360** - TR-3 referansi, Faz 1.5. 1 paragraph rationale: BanditKnightG'nin UI'si dort koseye dagitildigi icin merkez aksiyon temiz kaliyor; RIMA bunu direkt kopyalamadan 640x360 ref res icin icon-first HUD standardi belirlemeli. HP/MP, objective, hotbar ve reward alanlari simdiden tanimlanirsa ileride UI borcu azalir.

4. **Karar #147-aday - Environment Accent Budget** - TR-6/RJ-3 referansi, Faz 1.5. 1 paragraph rationale: RIMA'nin 10 class silhouette bible'i guclu bir identity varligi; environment prop ve VFX accentleri ayni hue alanini isgal ederse bu varlik zayiflar. Her biome/room icin accent hue budget tanimlamak asset uretiminde ve visual QC'de netlik saglar.

5. **Karar #75-revizyon adayi - create_map_object Prop Role Tags** - TR-2 referansi, Faz 1.5. 1 paragraph rationale: `create_map_object` sadece "guzel prop" uretmek icin degil, prop'un oda icindeki gorevini de tasimak icin kullanilmali. Hero/mid/accent tagleri, collision footprint ve layer hedefi prompt/metadata seviyesinde isaretlenirse asset pipeline generator ile daha iyi konusur.

6. **Karar #148-aday - Stealth Interaction Room Negative Space** - TR-8 referansi, Faz 2. 1 paragraph rationale: BanditKnightG'nin noble house frame'i, stealth/interaction odasinda boslugun polish eksikligi degil okunurluk araci oldugunu gosteriyor. RIMA stealth veya objective odalari ekleyecekse guard path, clue/loot ve danger iconlari icin combat odalarindan farkli density hedefi gerekir.

## Codex'in QC onerisi

**3 gunluk production'da prototiplenebilir (deadline 2026-05-18):**

- **Karar #144-aday Room Density Formula:** 2-3 oda archetype screenshot'i uzerinden walkable/clutter/hero prop oran checklist'i cikabilir.
- **Karar #145-aday Combat Feedback Hierarchy:** Karar #137 VFX Router icin prefab/tag tablosu ve ProcLimiter kural seti dokumante edilebilir; full implement sart degil.
- **Karar #75-revizyon adayi Prop Role Tags:** PixelLab/Codex prompt sablonuna `hero/mid/accent`, `collision`, `layer` alanlari eklenebilir.

**Faz 1.5'a defer edilmeli:**

- **Karar #146-aday Corner HUD at 640x360:** UI prototipi gerekir; combat sistemi ve resolution testleri olmadan kilitlemek erken.
- **Karar #147-aday Environment Accent Budget:** Class palette bible ile birlikte bakilmali; acele hue yasaklari asset uretimini daraltabilir.

**Faz 2'ye defer edilmeli:**

- **Karar #148-aday Stealth Interaction Room Negative Space:** RIMA'nin stealth/interaction room hedefi netlesmeden oda bosluk kurali final karar olmamali.
