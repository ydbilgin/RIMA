# CLASS_SILHOUETTE_BIBLE.md
**LOCKED 2026-05-13 -- Karar #80 -- RIMA 10 Sinif Siluet Standardi**
**KRITIK KURAL: 1 sinif = 1 silah = 1 silhouette. Variant YOK. Silah degisimi = sinif degisimi. (Karar #72 + #80)**

---

## Niyet

Bu belge, RIMA'nin 10 oynanabilir sinifinin her birinin gorsel kimligini tek bir referans noktasinda sabitleyen siluet standardidir. Karar #80 ile kilitlenmistir. Amac: farkli araclarda (PixelLab, Gemini konsept, Unity QC) uretilen sprite'larin ayni silhouette kuralina baglanmasi; V1 4 sinif ile V2 6 sinifin 64x64 chibi top-down ekranda birbirinden net ayirt edilebilmesi.

Ikinci niyeti: her sinifa ait anti-pattern listesi, uretim sirasinda tasarim kaymasini (design drift) onler. Bir sinif icin uretilen varligi kabul etmeden once bu belgedeki 6 alan ile karsilastirilir. Skor degil; gecti / gecmedi kontrolu. PixelLab prompt hook alani her sinifin "ilk generation" prompt temelini verir, bu sekilde siniflar arasi tutarlilik saglanir.

---

## Tech Constraints (Hatirlatma Tablosu)

| Alan | Deger |
|---|---|
| Sprite | 64x64 chibi top-down |
| PPU | 64 |
| Yon | 4 kardinal (N/S/E + W=flipX) |
| View | ~30-35 derece high top-down (Hades eslesme) |
| Silah | 1 sinif = 1 silah = 1 silhouette (variant YOK) |
| Animasyon canvas | 252x252 (PixelLab v3 zorunlu, Unity'ye 128x128 crop) |
| Visual pin | Into Samomor (Sang Hendrix RPG Maker MZ) |
| Lokomotion | Run (Walk YOK), 6 frame, 4 yon |
| Pipeline | PixelLab Create Character native silahlı 1-piece (Karar #72) |

---

## Cross-Class Kurallar (LOCKED)

1. Silah = siluet'in %40-50'si -- silah degisimi = sinif degisimi, variant uretilmez
2. Renk paleti her sinifa 1 dominant accent (neon spektrum), siniflar arasi cakisma yasak
3. Sinif silhouette slot'u esiz -- V2 6 sinif, V1 4'le shape space'de cakismaz
4. Anti-pattern her sinif icin listede; QC'de her madde kontrol edilir
5. Silah hep elde / hazir pozisyon -- combat-out ayrı state YOK, "puf" mekanigi YOK (Karar #71)
6. Ronin istisna: sheath/draw kimliginin parcasi, yasak kapsamı disinda (Karar #71 notu)

---

## 1. Warblade -- PILOT TAM PROFIL

**Cinsiyet:** Erkek | **Sprint:** V1 (Faz 1 Primary) | **Counter Arketip:** absorb/break (Karar #57)

### 6 Alan

1. **Silhouette Dominant Shape:** Omuz + iki-el kılıc dominant. Govde genis, pos ileri egik. 64px'te kuzey yonunde omuz V-shape okunabilir, guney yonunde kılıc govdenin onunu kapatir.
2. **Weapon Size + Grip:** Iki-el greatsword, sprite'in ~%45'i. Iki el, yan tutuş (horizontal carry), low guard pozisyonu. Sirt'a asili veya kinalı versiyon yok.
3. **Pose Archetype:** Low guard -- sword tip yere yakin, omuz ileri, baş hafif one egik. Alçak merkez agirlik, saldırıya hazir değil, savunmada bekliyor.
4. **Color Palette:** main #4F3A2C (koyu deri zırh) + accent #C09455 (pirinç detay/tokalar) + danger #D43F3F (kan/ofke pulse, skill VFX)
5. **Animation Rhythm:** Heavy -- vuruslar gec ama agir, recovery uzun, dash agir savrulma. Dash-cancel window: %60-75 (Karar #67). Hizli gibi hissettirmez; agirlik animasyon/partikul'den gelir, input response'dan degil (Karar #13).
6. **Visual Identity Tag:** "Her vurusunu hakeden agir kilıç ustası -- kaba kuvvet degil, biriktirilmis kontrol."

### Yonlere Gore Siluet

- **N (arka):** Sirt + sword tip yukari cikiyor, omuz V-shape cok net, bas kucuk.
- **S (on):** Kilıc govdenin onunde ust yarıyi gizliyor, bas one egik, gomgok siluet.
- **E (profil):** Klasik knight silueti, kilıc yatay yer kaplar, omuz one cikar.
- **W:** E flipX (simetrik sinif -- Karar #72 uyumlu).

### PixelLab Prompt Hook

"64x64 chibi top-down character, male heavy warrior with two-handed greatsword, dark leather armor with brass buckle accents, low guard stance sword tip near ground, broad shoulders forward-leaning posture, view 35 degree high top-down ARPG angle, dark brown body #4F3A2C brass accent #C09455, 4 directions front back east west, pixel art PPU 64, Into Samomor palette reference"

### Anti-Pattern

- Ince zırh veya hafif silah -- Warblade kalın, kagıt zırh degil
- Suslü veya parlak dekorasyon -- isci estetigi, mucadele kıyafeti
- Pastel veya parlak renk -- mat kahve/pirinc kanonik
- Hızlı animasyon ritmi -- heavy = agır, recovery uzun
- Kilıc sırtta veya kinalı -- silah her zaman elde, hazır tutuşta (Karar #71)
- HP-execute gorsel kue (HP<30% renk degisimi vb.) -- Karar #56 yasak

---

## 2. Ranger -- ORTA DETAY

**Cinsiyet:** Kadın | **Sprint:** V1 (Faz 2) | **Identity:** Tactical rift hunter, dungeon/ruins avcısı -- forest archer degil (Karar #37)

### 6 Alan

1. **Silhouette Dominant Shape:** Uzun dikey siluet + yay dominant. Asimetrik: sol elde compound bow govdenin disina cikar, sag el ok ceker. Ince govde, genis omuz-yay genisligi.
2. **Weapon Size + Grip:** Compound bow (sol el), sprite'in ~%40-45'i. Her zaman elde at-rest pozisyon -- cekme animasyonu disinda bile elde tasiyor (Karar #71).
3. **Pose Archetype:** Drawn bow rest -- yay indirilmis ama sol elde tutulu, govde hafif yandan, taktik avcı durusu. Distance discipline: dusmanla arayi koruyan postür.
4. **Color Palette:** main #2A3520 (koyu orman yesil/zırh) + accent #7BA7BC (cold blue -- sınıf aksent, Karar tablosu) + danger #FFB000 (trap/mark ateşleme VFX amber)
5. **Animation Rhythm:** Sniping -- hareket anında, atis gecikmeli (hazırlık süresi var), Focus buildup ritmi. Dash-cancel window: %30-55 (Karar #67).
6. **Visual Identity Tag:** "Ulasamazsin -- her saniye kayip veriyorsun. Tuzak + mesafe kontrolu."

### Yon Notu

- N: Yay arkadan cikiyor, omuz asimetrik. S: Yay one uzaniyor, kiz silueti okunuyor. E: Profil net -- uzun yay sol elde dis tarafa cikar. W: 4 yon ayri uretim (asimetrik sinif, flipX yok).

### Anti-Pattern

- Run-and-gun hareketi -- Gunslinger arketipine kacis yasak (Karar #37)
- Kilıc veya yakin silah -- compound bow tek silah
- Kısa govde / genis duruş -- Warblade'e benzer shape space, dikey fark korunmali
- Pasif, saldirmazlik posturu -- tactical hunter, savunmada degil kontrolde

### PixelLab Prompt Hook

"64x64 chibi top-down female ranger, compound bow in left hand at-rest low position, dark forest armor with cold blue trim, slim upright posture distance-keeping stance, view 35 degree high top-down, cold blue accent #7BA7BC, pixel art PPU 64, 4 separate directional sprites"

---

## 3. Shadowblade -- ORTA DETAY

**Cinsiyet:** Erkek | **Sprint:** V1 (Faz 2) | **Identity:** Void aesthetic, positional, Rift Scar/collapse geometrisi (Karar #15)

### 6 Alan

1. **Silhouette Dominant Shape:** Ince dikey siluet + postür dominant. Twin blades vucuda yakin, reverse-grip. Warblade'den ince, Ranger'dan daha kompakt. Karanlık siluet: siyah/koyu mor govde.
2. **Weapon Size + Grip:** Twin blades (ikiz kisa kiliclar), her zaman reverse-grip, vucuda yakin tutuyor (Karar #71). Sprite'in ~%30-35'i -- silah kucuk ama tutuş esiz.
3. **Pose Archetype:** Blade reverse grip -- kiliclar ters tutulmus, dirsek arka, cok ince profil. Phase Step veya faz gecisinde bile silahlar kaybolmaz.
4. **Color Palette:** main #1A1025 (neredeyse siyah koyu mor) + accent #5A2A8A (void purple, Karar tablosu) + danger #00FFCC (void crack/Rift Scar neon teal, Rift collapse VFX)
5. **Animation Rhythm:** Fluid -- hizli, akici, faz gecisi smooth. Dash-cancel window: %15-25 (erken cancel, Karar #67). Her vuruş bir sonrakine akar.
6. **Visual Identity Tag:** "Gormeden once hissedilir -- pozisyonel, void estetigi, Rift Scar biriktirir ve cokertiyor."

### Yon Notu

- N: Ince sirt silueti, kiliclar yanda sarkiyor. S: Ince on, kiliclar one cikiyor. E: Profil ince, reverse-grip belirgin. W: 4 yon ayri uretim (asimetrik sinif).

### Anti-Pattern

- WoW Rogue kopyasi -- eski tasarım iptal edildi (Karar #15)
- Kiliclar embed glow -- YASAK (Karar tablosu: "embedded glow YASAK")
- Genis omuz/agir zırh -- Warblade'e kacis yasak
- Kiliclar belinden asilı veya sırtta -- silah hep elde, hazır tutuşta (Karar #71)

### PixelLab Prompt Hook

"64x64 chibi top-down male shadowblade, twin short blades in reverse-grip held close to body, near-black deep purple armor with void purple accents, slim upright posture, no embedded blade glow, view 35 degree high top-down, void purple accent #5A2A8A, pixel art PPU 64, 4 separate directional sprites"

---

## 4. Elementalist -- ORTA DETAY

**Cinsiyet:** Kadın | **Sprint:** V1 (Faz 2) | **Identity:** Silahsiz sinif -- buyuler el jestleriyle, VFX engine-side (Karar tablosu)

### 6 Alan

1. **Silhouette Dominant Shape:** Uzun robe dominant + el/kol pozisyonu. Silahsiz tek sinif. Baş okunabilir (honey-blonde, Karar #43). Robe alt kesimi genis, ust govde ince.
2. **Weapon Size + Grip:** YOK. Silah pass yok. Eller buyü jesti pozisyonunda (one uzanmis, or kucuk acilmis). Buyular ve fırlatmalar dogrudan el jestleriyle (Karar #71 silahsiz istisna).
3. **Pose Archetype:** Academic controlled -- dimduz, denge odakli, buyü hazırlama postu. Sakinlik ve kontrol ifadesi. Sac: warm honey-blonde, arkaya topuz (Karar #43).
4. **Color Palette:** main #2A1F35 (koyu mor-gri cüppe, element-agnostik) + accent #FFF000 (element-agnostik sari/gold, aktif buyü highlight) + danger/element rengi runtime-da degisir (Fire=#D43F3F, Frost=#00FFCC, Light=#FFD700)
5. **Animation Rhythm:** Fluid cast -- buyü sekansı ritimli ama sakin. Cast windup iptal edilemez (Karar #67 casters kural). Buyü patlama sonrasi kisa recovery.
6. **Visual Identity Tag:** "Silahsiz kontrolcu -- el jestleriyle element degistirir, robe silueti ile esiz."

### Yon Notu

- N: Robe genisligi arka siluette belirgin, eller yanlarda. S: Yuz okunuyor (honey-blonde sac, Karar #45 notu). E: Uzun robe profil, el one uzaniyor. W: 4 yon ayri uretim (asimetrik).

### Anti-Pattern

- Asa veya herhangi bir silah -- silahsiz sinif, buyü eller ile (Karar tablosu)
- Sac siyah veya koyu -- honey-blonde kanonik, robe ile kayniyor (Karar #43)
- Agir zırh veya omuz kapagi -- robe silueti korunmali
- Hızlı dash-cancel -- windup iptal edilemez (Karar #67)

### PixelLab Prompt Hook

"64x64 chibi top-down female elementalist, no weapon, long dark robe element-agnostic, hands raised in casting gesture, warm honey-blonde hair bun, controlled academic posture, view 35 degree high top-down, dark purple robe #2A1F35 golden accent #FFF000, pixel art PPU 64, 4 separate directional sprites"

---

## 5. Ravager -- ISKELET

**Cinsiyet:** Erkek | **Sprint:** V2 (Faz 3) | **V Dolum:** Carnage (kill chain)

### 6 Alan

1. **Silhouette Dominant Shape:** Agir + omuz baskın, Warblade'den daha vahsi -- silah ve govde birlikte kütlesel
2. **Weapon Size + Grip:** Buyük iki-el silah (balta veya agir warhammer), sprite ~%45-50, iki el
3. **Pose Archetype:** Berserk ready -- agirlik one, taarruz pozisyonu, hic dinlenmeden
4. **Color Palette:** main #3A1A0A (koyu kirmizi-kahve) + accent #D43F3F (kan kirmizi) + danger #FF4400 (Berserk Mode kan siklozu VFX)
5. **Animation Rhythm:** Heavy+ -- Warblade'den daha yavaş ve daha agir, BERSERK MODE'da hizlanma ani
6. **Visual Identity Tag:** "Durdurulamaz kan furyan -- Berserk Mode'da alan AoE."

### Anti-Pattern

- Warblade ile ayni shape space -- Ravager daha kaba, daha vahsi, renk kaymasi (koyu kirmizi vs kahve)
- Hassas zırh veya metalik parlaklik -- savaşci, agir
- Hizli animasyon ritmi normal halde

### PixelLab Prompt Hook

"64x64 chibi top-down male ravager berserker, large two-handed battle axe or warhammer, dark blood-red rough armor, aggressive forward-leaning berserk posture, view 35 degree high top-down, dark red-brown #3A1A0A blood red accent #D43F3F, pixel art PPU 64, 4 directions"

---

## 6. Ronin -- ISKELET

**Cinsiyet:** Erkek | **Sprint:** V2 (Faz 3) | **V Dolum:** Flow Cut | **Istisna:** sheath/draw kimliği (Karar #71)

### 6 Alan

1. **Silhouette Dominant Shape:** Ince + katana dominant, BDO Musa tarzı iaido silueti, dar profil
2. **Weapon Size + Grip:** Tek el katana, sprite ~%40, sag elde; kın sol belde (sheath/draw kimliği, Karar #71 istisna)
3. **Pose Archetype:** Iaido low draw -- katana kında veya cekiliyor, govde yanda, Draw Tension buildup
4. **Color Palette:** main #1A1A2E (koyu lacivert/siyah) + accent #C8A87A (soluk altin/pirinc, katana sapı) + danger #FFF000 (Draw Tension patlama flash)
5. **Animation Rhythm:** Timing chain -- kesmek anlık ama hazırlık uzun; Dash-cancel window: %15-25 (Karar #67, Shadow ile ayni grup)
6. **Visual Identity Tag:** "Tek kesik, sonsuz sessizlik -- iaido ustası, Draw Tension biriktirip serbest birakar."

### Anti-Pattern

- Warblade ile shape cakismasi -- Ronin ince, katana tek el, sheath var
- Katana hep cekili -- sheath/draw kimligi bozulur
- Agir zırh -- hafif/ince kimono/hakama benzeri

### PixelLab Prompt Hook

"64x64 chibi top-down male ronin, single katana right hand with sheath on left hip, dark navy near-black kimono style, iaido draw stance low profile, view 35 degree high top-down, dark navy #1A1A2E pale gold accent #C8A87A, pixel art PPU 64, 4 directions"

---

## 7. Gunslinger -- ISKELET

**Cinsiyet:** Kadın | **Sprint:** V2 (Faz 3) | **Identity:** Rift-tech dual-pistol, kinetic run-and-gun (Karar #38) | **Sac:** Deep auburn red (Karar #44)

### 6 Alan

1. **Silhouette Dominant Shape:** Trenckoat + dual pistol, iki silah yanlarda, kinetik hareket postu
2. **Weapon Size + Grip:** Ikili tabanca (dual pistol), iki elde, rift-tech estetigi -- klasik western degil (Karar #38)
3. **Pose Archetype:** Kinetic run-and-gun -- her zaman harekette, silahlar one veya yanda hazır, kadin silüeti korunmali
4. **Color Palette:** main #1A1520 (koyu gri-mor trenckoat) + accent #FF4400 (ateş turuncu/rift muzzle flash) + danger #FF0080 (Heat ZERO ulti VFX neon pembe)
5. **Animation Rhythm:** Fast -- erken dash-cancel %30-55 (Karar #67), hareket surekli, atis hizlı
6. **Visual Identity Tag:** "Rift-tech ikili tabanca -- her zaman hareket, kadin okunurlugu trenckoat altinda."

### Anti-Pattern

- Western/kovboy estetigi -- yasak (Karar #38)
- Tek tabanca -- dual pistol kanonik
- Agir zırh -- trenckoat silueti korunmali
- Ranger ile mesafe-disiplin benzerligi -- run-and-gun vs kite-control net ayri

### PixelLab Prompt Hook

"64x64 chibi top-down female gunslinger, dual rift-tech pistols both hands, dark grey-purple trench coat, kinetic movement pose guns raised, deep auburn red hair, view 35 degree high top-down, dark grey-purple #1A1520 fire orange accent #FF4400, pixel art PPU 64, 4 directions"

---

## 8. Brawler -- ISKELET

**Cinsiyet:** Erkek | **Sprint:** V2 (Faz 3) | **State:** Shattered (Sundered degil -- Karar #55) | **Counter Arketip:** whiff/evade body movement (Karar #57)

### 6 Alan

1. **Silhouette Dominant Shape:** Genis omuz + yumruk dominant, silahsiz sinif #2, kas kütlesi silueti
2. **Weapon Size + Grip:** YOK. Eller yumruk pozisyonu, boks/dövüş sanatı gribi, diz ve dirsek vurular icin duruş
3. **Pose Archetype:** Fighting stance -- weight low, one egik, eller boks guard
4. **Color Palette:** main #2A1A10 (koyu deri/deri kemer) + accent #FF8C00 (turuncu Charge indicator) + danger #FFB000 (Crowd Hype V burst amber)
5. **Animation Rhythm:** Mash+timing -- kombo ritmi surukler, Charge birikir; Dash-cancel %60-75 (Karar #67, Warblade ile ayni agır grup)
6. **Visual Identity Tag:** "Kombo yumruk/tekme -- whiff'e girerek karsi acar, Charge biriktirip Overdrive fusler."

### Anti-Pattern

- Herhangi bir silah -- silahsiz dövüscü
- Sundered VFX -- Warblade-only, Brawler Shattered (vücut fissurlari) kullanir (Karar #55)
- Ince govde -- kas kütlesi silueti korunmali
- Elementalist ile karis -- Elementalist akademik, Brawler agresif beden

### PixelLab Prompt Hook

"64x64 chibi top-down male brawler, no weapon bare fists raised in fighting guard stance, heavy muscular build dark leather wrappings on hands, low fighting crouch aggressive posture, view 35 degree high top-down, dark leather #2A1A10 orange accent #FF8C00, pixel art PPU 64, 4 directions"

---

## 9. Summoner -- ISKELET

**Cinsiyet:** Kadın | **Sprint:** V2 (Faz 4) | **V Dolum:** Grave Chorus

### 6 Alan

1. **Silhouette Dominant Shape:** Robe + totem/staff dominant, minyon yonlendirme jesti -- Elementalist'ten uzun staff veya obje ile ayirt edilir
2. **Weapon Size + Grip:** Staff veya totem (tek el veya iki el), sprite ~%35-40, buyük baş ustunde
3. **Pose Archetype:** Conductor stance -- eller minyon yonlendirme pozisyonu, staff yukarida veya yanda
4. **Color Palette:** main #0A1A0A (cok koyu yesil-siyah) + accent #00FF88 (neon yesil minyon glow) + danger #88FF00 (Grave Chorus V burst yesil-sari)
5. **Animation Rhythm:** Fluid summon -- buyü caglama ve minyon yollamak akici, Minyon yonlendirme V burst unique
6. **Visual Identity Tag:** "Minyon ordusu -- Grave Chorus ile alan kontrolu, Summoner hep arkan."

### Anti-Pattern

- Elementalist ile siluet cakismasi -- Summoner staff var, minyon jesti esiz, renk paketi tamamen ayri (yesil vs mor/sari)
- Atas veya direkt dövüş pozisyonu -- Summoner minyon arkasinda
- Kötü minyon okumasi -- Summoner silüeti minyon siluetinden net ayirt edilmeli

### PixelLab Prompt Hook

"64x64 chibi top-down female summoner, long dark staff or totem raised overhead, very dark green-black robe, conductor gesture hands directing, neon green glowing accents, view 35 degree high top-down, dark green-black #0A1A0A neon green accent #00FF88, pixel art PPU 64, 4 directions"

---

## 10. Hexer -- ISKELET

**Cinsiyet:** Kadın | **Sprint:** V2 (Faz 4) | **V Dolum:** Dread | **State:** Hex Stack (max 10, Karar #21)

### 6 Alan

1. **Silhouette Dominant Shape:** Cupe + grimoire/kitap dominant, yuz one egik lanet okuyucu postu -- Elementalist/Summoner'dan koyu/macabre estetikle ayirt edilir
2. **Weapon Size + Grip:** Grimoire/lanet kitabi (iki el, govdenin onunde) veya hancer (tek el), sprite ~%30-35
3. **Pose Archetype:** Curse reader -- govde hafif one egik, kitap onde tutulmus, bas one bakiyor
4. **Color Palette:** main #1A0A1A (cok koyu mor-siyah) + accent #8B0000 (koyu kirmizi/lanet hex tonu) + danger #FF0000 (Hex 10 stack ödül patlama kirmizi)
5. **Animation Rhythm:** Accumulation -- stack birikmesi yavas ama odül ani ve buyuk; cast windup iptal edilemez (caster grubu, Karar #67)
6. **Visual Identity Tag:** "Lanet biriktiricisi -- 10 stack Hexblast'a dogru yavaş birikim, tam odulde ani patlama."

### Anti-Pattern

- Erkek dark wizard generigi -- Hexer kadin, Karar #34
- Elementalist ile estetik karis -- Hexer macabre/lanet, Elementalist akademik/element-agnostik; renk paketleri tamamen ayri
- Kirmizi renk Ravager'la cakismasi -- Hexer koyu krimzi/karanlık, Ravager kan kirmizisi/ates; ton tonu farkli tutmak zorunda

### PixelLab Prompt Hook

"64x64 chibi top-down female hexer, dark grimoire spellbook held before chest, very dark purple-black robe hood up, curse reader posture body slightly forward, dark red accents, view 35 degree high top-down, dark purple-black #1A0A1A dark red accent #8B0000, pixel art PPU 64, 4 directions"

---

## Cross-Reference Matrix

| Sinif | Tier | Cinsiyet | Dominant Shape | Accent Hex | Rhythm | Silah |
|---|---|---|---|---|---|---|
| Warblade | V1 | E | omuz+greatsword | #C09455 | heavy | Greatsword (iki el) |
| Ranger | V1 | K | uzun+compound bow | #7BA7BC | sniping | Compound bow (sol el) |
| Shadowblade | V1 | E | ince+twin blades | #5A2A8A | fluid | Twin blades (reverse-grip) |
| Elementalist | V1 | K | robe+el jesti | #FFF000 | fluid cast | YOK (silahsiz) |
| Ravager | V2 | E | kütlesel+battleaxe | #D43F3F | heavy+ | Battleaxe/warhammer (iki el) |
| Ronin | V2 | E | ince+katana | #C8A87A | timing chain | Katana (sag el) + kin |
| Gunslinger | V2 | K | trenckoat+dual pistol | #FF4400 | fast | Dual pistol (iki el) |
| Brawler | V2 | E | kas+yumruk | #FF8C00 | mash+timing | YOK (silahsiz) |
| Summoner | V2 | K | robe+staff | #00FF88 | fluid summon | Staff/totem |
| Hexer | V2 | K | cupe+grimoire | #8B0000 | accumulation | Grimoire/hancer |

---

## Production Notes

- PixelLab Create Character v3 -- sinif basina 1 generation, 4 yon native veya 4 ayri seed
- Canvas: 252x252 (v3 zorunlu), Unity'ye 128x128 crop, PPU=64
- Manuel cleanup 5-15 dk/sprite (Karar #72 hibrit pipeline)
- Siluet test: 64px small + 16px micro thumbnail -- 4 sinif yan yana ayirt edilebilmeli
- Renk palette cakisma testi: accent hex'ler birbirinden hue+brightness farkli olmali, yan yana koyulunca yanlis sinif okunamali
- Karar #71: Silah Single-State -- QC'de her sprite'ta silah elde kontrol edilir; "puf" mekanigi var mi kontrol edilir
- Asimetrik siniflar (Ranger/Shadowblade/Elementalist/Gunslinger/Summoner/Hexer): W=flipX YASAK, 4 yon ayri uretim
- Simetrik siniflar (Warblade, Ravager, Brawler): W=E flipX mumkun -- Ronin borderline (kin sol bel, flipX dikkat)

---

## Cross-References

- Karar #71: Silah Gorünürlük Single-State LOCKED
- Karar #72: Silahlı 1-piece PixelLab native pipeline
- Karar #80: Class Silhouette Bible 6-alan standart (bu belge)
- Karar #13: Tum classlar hizli hissettirmez -- agirlik gorsel/animasyondan
- Karar #15: Shadowblade tam redesign (WoW Rogue iptal)
- Karar #34: Cinsiyet 5E/5K kilitlendi
- Karar #37: Ranger identity -- tactical rift hunter, forest archer degil
- Karar #38: Gunslinger -- western yasak, rift-tech dual-pistol
- Karar #43: Elementalist sac rengi -- honey-blonde
- Karar #44: Gunslinger sac rengi -- deep auburn red
- Karar #53: 4 kardinal yon S/E/N/W kilitlendi
- Karar #55: Brawler state = Shattered (Sundered degil)
- Karar #57: Counter arketip ayrimi (Warblade/Ronin/Brawler)
- Karar #67: Dash-cancel window per-class
- Bagimli: SKILL_VISUAL_CONTRACT.md (Section E Identity Compliance Check -- siluet readable at 64px)
- Bagimli: MASTER_KARAR_BELGESI #71 + #72 + #80
- Bagimli: SINIF_VE_SKILL_KARAR_BELGESI.md (class anchor tablosu)

---

## Open Questions

1. Ravager silah secimi net degil (battleaxe vs warhammer) -- uretim oncesi bir QC kararı gerekiyor
2. Summoner staff vs totem acik -- minyon gorünumu ile koordineli olmali, uretim oncesi belirtilmeli
3. Hexer grimoire vs hancer secimi -- ikisi birden mi (iki elde kitap + belde hancer) veya tek secim mi?
4. V2 siniflar icin color palette hex degerleri taslak -- Into Samomor mat env ile cakisma testi yapilmadi; uretim baslamadan once QC renk testi onerilir
5. Ronin W=flipX kullanilabilir mi sorgusu acik -- kin sol belde olduğundan flipX yanlis taraf gosterebilir; 4 yon ayri uretim daha guvenli
