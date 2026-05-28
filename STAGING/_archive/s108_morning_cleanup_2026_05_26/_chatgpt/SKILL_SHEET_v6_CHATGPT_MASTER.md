# RIMA Skill Sheet v6 — ChatGPT BATCH Master Document

**Kullanım:** Tek bir ChatGPT konuşmasında, 10 sprite + bu döküman'ı yükle, "**BATCH START**" yaz. ChatGPT 10 class sheet'i sırayla üretecek.

---

## ⚡ BATCH START — ChatGPT'ye İlk Mesaj

```
Sen RIMA adlı 2D top-down isometric roguelite oyunu için 10 ARDIŞIK skill demo composite sheet üreteceksin. Bu doküman SPEC. Sırayla ürettiğin 10 PNG'yi tek konuşmada tamamlayacaksın.

KURAL 1 — Sprite Identity (HARD):
Upload edilen 10 sprite (01_warblade_south.png ... 10_summoner_south.png) REFERENCE. Her class composite üretirken o class'ın sprite'ını inceleyip anatomy/clothing/hair/skin/weapon BİREBİR koru. Karakter uydurma YASAK.

KURAL 2 — Style:
Pixel art game illustration, 30-35° angled isometric perspective, chibi proportions (sprite reference ile aynı kafa/vücut oranı), painterly Hades+Diablo synthesis mood. NOT cinematic, NOT flat icon, NOT photographic, NOT anime cel-shaded.

KURAL 3 — Scene Composition:
Her skill panel'inde:
- Karakter sprite-faithful action pose
- Class signature weapon görünür (aşağıda her class için yazılı)
- Karşı Act 1 mob (variety şart, panel başına farklı)
- Mob mid-hit reaction (skill'e uygun: knocked, sliced, frozen, burned, hexed, etc.)
- Skill VFX active painterly (geometric primitive arc DEĞİL)
- Act 1 environment hint (granite floor + cyan rift accent background)
- Panel altında skill name caption (12-16px white, küçük)

KURAL 4 — YASAKLAR:
- Programmatic compositing (sprite paste + primitive geometry) YASAK
- Modern UI overlay YASAK
- Test render look YASAK
- Stay/Break/Carry image elementi YASAK

KURAL 5 — Batch Akış:
Aşağıdaki Section 4'teki 10 class'ı SIRAYLA üreteceksin:
01 Warblade → 02 Ronin → 03 Gunslinger → 04 Ranger → 05 Elementalist → 06 Shadowblade → 07 Ravager → 08 Hexer → 09 Brawler → 10 Summoner

Her class için:
1. O class'ın sprite reference'ını incele
2. O class'ın skill listesi + visual hint'lerine sadık ol
3. Tek composite PNG üret (grid layout aşağıda her class için yazılı)
4. PNG hazır olduğunda bir sonraki class'a otomatik geç
5. 10 tamamlanınca "BATCH COMPLETE" yaz

Şimdi başla: ilk olarak 01 Warblade composite'ini üret.
```

---

## 2. ACT 1 MOB ROSTER (her panel'de farklı mob)

- Bone Walker — kemik iskelet, kılıç
- Bone Archer — kemik iskelet, yay
- Cyan Slime — yeşil-cyan jelly
- Goblin — yeşilimsi humanoid, dirty leather
- Imp Demon — kırmızı küçük şeytan, küçük boynuzlar
- Specter Ghost — soluk beyaz hayalet
- Wraith Specter — koyu mor hayalet, yırtık pelerin
- Skull / Animated Skull — uçan kemik kafatası
- Bat — koyu yarasa
- Dungeon Rat — kahverengi büyük fare
- Husk — gri yarı-çürük zombi
- Giant Spider — kara siyah örümcek
- Ground Crawler — sürüngen sürünen yaratık
- Rat King — toplu sıçan kümesi
- Cyan Wisp — küçük cyan enerji topu

---

## 3. CLASS WEAPON CANONICAL

- **Warblade:** large two-handed greatsword (steel blade, brown leather grip)
- **Ronin:** katana (curved single-edged blade, traditional tsuka grip)
- **Gunslinger:** twin pistols (revolver-style, dual-wielded)
- **Ranger:** longbow (recurve, wood + sinew string)
- **Elementalist:** orb staff (wooden staff topped with floating arcane orb)
- **Shadowblade:** dual daggers (curved, reverse-grip)
- **Ravager:** large two-handed axe (heavy iron bladed)
- **Hexer:** cursed totem (skull-topped staff, hanging fetishes)
- **Brawler:** iron gauntlets (knuckle-plated, no weapon — fists are weapons)
- **Summoner:** open tome + floating sigil (book held in one hand, glowing sigil hovering)

---

## 4. 10 CLASS BATCH ORDER + SKILL LIST + GRID

### 01 — WARBLADE (14 skill, 5×3 grid, 1792×1024)

Reference: `01_warblade_south.png`
Weapon: large two-handed greatsword

Skills:
1. **Battle Surge** — Vücudundan kırmızı energy aura patlaması, etrafındaki mob'lar geri savrulur
2. **Blade Rush** — İleri dash + arkasında çelik streak trail, dash sonu mob biçilmiş
3. **Cleave** — Yatay wide arc swing, mob orta-bel hizasında ikiye ayrılmış
4. **Crippling Blow** — Aşağı diz-arkası saldırı, mob tek dizini çökmüş, sarı debuff icon
5. **Death Blow** — Devasa overhead slam, mob yere göçmüş, kırmızı şok dalgası
6. **Deep Wound** — Yan slash, mob'tan kanama drip, kırmızı DoT marker
7. **Earthsplitter** — Greatsword'u yere gömme, ground crack cyan/sarı erupting
8. **Gravity Cleave** — Wide arc + mob'ları kendine doğru çeken cyan gravity well
9. **Iron Charge** — Hızlı şarj + omuz darbesi, mob duvara çakılmış
10. **Ironclad Momentum** — Pasif aura, karakter etrafında iron particle vortex
11. **Iron Counter** — Defensive stance + counter-swing, mob counter ile vurulmuş mid-hit
12. **Iron Crush** — Vertical slam down, mob impact crater içinde, taş parçaları savruluyor
13. **Sunder Mark** — Mob üzerine yaratılmış kırmızı kırık armor sigil, debuff
14. **War Stomp** — Yere ayak vurma, radial shockwave dalgası, mob'lar yerle bir

Output: `01_warblade_v6.png` (1792×1024)

---

### 02 — RONIN (4 skill, 2×2 grid, 1024×1024)

Reference: `02_ronin_south.png`
Weapon: katana

Skills:
1. **Final Draw** — Hızlı katana çekiş + 1-shot sweep, mob arkasında ikiye ayrılmış
2. **Iaido Stance** — Katana kabzasına el, sakin defensive pose, cyan focus aura
3. **Quickdraw** — Lightning-fast katana flash, mob ile arasında beyaz çizgi trail
4. **Sakura Veil** — Sakura petal vortex, mob petals içinde, kırmızı blade glow

Output: `02_ronin_v6.png` (1024×1024)

---

### 03 — GUNSLINGER (8 skill, 4×2 grid, 1792×1024)

Reference: `03_gunslinger_south.png`
Weapon: twin pistols

Skills:
1. **Twin Fire** — Her iki pistol aynı anda ateşler, 2 mob'a aynı anda hit
2. **Ricochet Shot** — Tek bullet path zigzag arc, 3 mob'a sırayla seker
3. **Smoke Round** — Gri smoke cloud içinde mob silüetleri
4. **Quick Reload** — Hızlı pistol reload, ejected shell case'leri uçuyor
5. **Powder Burst** — Pistol namlu çıkışında alevli explosion, mob alev içinde
6. **Fan The Hammer** — Hammer fan, 6 hızlı bullet trail, mob multi-hit
7. **Bullet Time** — Karakter sakin slow-motion, etrafta bullet'lar havada donmuş
8. **Dead Eye** — Headshot anı, mob alından bullet exit + kırmızı sis

Output: `03_gunslinger_v6.png` (1792×1024)

---

### 04 — RANGER (20 skill, 2 sheet — 5×2 + 5×2, her biri 1792×1024)

Reference: `04_ranger_south.png`
Weapon: longbow

Sheet A (Skill 1-10):
1. **Aimed Shot** — Charged longbow shot, ışıklı ok trail, mob göğsünden geçer
2. **Barbed Net Shot** — Diken-örülü net mob'u kuşatmış, mob hareketsiz
3. **Black Arrow** — Siyah ok mid-flight, dark shadow trail
4. **Bone Trap** — Yere yerleştirilmiş kemik diş trap, mob içinde sıkışmış
5. **Concussive Arrow** — Ok başında blast halkası, mob sersem confused state
6. **Disengage** — Geri-jump + havada ok ateşler, mob yere doğru
7. **Explosive Trap** — Patlayan trap, narıncı alev küresi, mob savruluyor
8. **Final Strike** — Son ok charged, çok büyük ışıklı projectile
9. **Flare** — Yere atılmış parlak flare, etrafa narıncı aydınlık
10. **Hunters Step** — Hızlı sideways dash + ok ateşler

Output A: `04a_ranger_v6.png` (1792×1024)

Sheet B (Skill 11-20):
11. **Marked Detonate** — Mob üzerinde işaret patlar, sarı patlama
12. **Multi Shot** — 3 ok aynı anda fan-spread şeklinde
13. **Pinning Shot** — Ok mob'u duvara çivilemiş, mob asılı
14. **Point Blank** — Yakın mesafe shot, mob yüzüne ok
15. **Predators Mark** — Mob'un üzerinde kırmızı predator işareti, parlıyor
16. **Rapid Fire** — Hızlı ardışık 5+ ok, hepsi mid-flight
17. **Sweep Volley** — Wide arc ok yağmuru, 4-5 mob hit
18. **Tethering Arrow** — Ranger'dan mob'a uzanan parlak cyan tether
19. **Volley** — Yukarıdan yağmur gibi ok yağışı, mob altında
20. **Wireline Trap** — İki nokta arası gerilmiş tel trap, mob takılmış

Output B: `04b_ranger_v6.png` (1792×1024)

---

### 05 — ELEMENTALIST (15 skill, 5×3 grid, 1792×1024)

Reference: `05_elementalist_south.png`
Weapon: orb staff

Skills:
1. **Arcane Blast** — Açık staff'tan büyük arcane purple energy küresi
2. **Arcane Surge** — Staff etrafında dönen mor energy ring'ler
3. **Blink** — Işınlanma, 2 silüet (önce-sonra konum), cyan particle
4. **Blizzard** — Yukarıdan ice shard yağmuru, mob donmuş ortada
5. **Chain Lightning** — Mob'lar arası zigzag electric arc, 3 mob seri vurulmuş
6. **Combustion** — Karakterden radial alev aurası, mob alev içinde
7. **Fireball** — Klassik orange fireball mid-flight, mob önünde
8. **Frost Wall** — Yere dikilmiş ice block wall, mob arkasında
9. **Frozen Orb** — Yere atılmış ice orb dönüyor + buzlu shard'lar çıkıyor
10. **Glacial Spike** — Yerden çıkan dev ice spike, mob üzerine geçirilmiş
11. **Living Bomb** — Mob üzerinde alev aura çiçek, patlama öncesi pulsing
12. **Meteor** — Yukarıdan büyük fiery meteor mid-fall, gölgesi yerde
13. **Mirror Image** — Karakterin 2 illusion clone'u yanında
14. **Prism Beam** — Staff'tan rainbow beam mob'a, mob ışık içinde
15. **Solar Flare** — Yukarıdan büyük parlak gold solar disc, mob ışıkta yanıyor

Output: `05_elementalist_v6.png` (1792×1024)

---

### 06 — SHADOWBLADE (22 skill, 3 sheet — 8+8+6, her biri 1792×1024)

Reference: `06_shadowblade_south.png`
Weapon: dual daggers

Sheet A (Skill 1-8, 4×2):
1. **Ambush** — Karanlık silüetten aniden çıkan dagger strike
2. **Backstab** — Mob arkasından çift dagger, kanlı icon
3. **Backstab Mark** — Mob sırtında pulsing kırmızı işaret
4. **Chain Cull** — 3 mob arasında zigzag dagger trail, 3'ünden kan
5. **Death Mark** — Mob başı üstünde kafatası işareti, mor glow
6. **Evasion** — Dodge spin, karakter ışıklı silüet (afterimage)
7. **Fan Of Knives** — Karakter etrafında 6 dagger fan-yayılmış spinning
8. **Hemorrhage** — Mob'tan büyük kan fışkırma, ground'da kan göleti

Output A: `06a_shadowblade_v6.png` (1792×1024)

Sheet B (Skill 9-16, 4×2):
9. **Kidney Shot** — Hızlı strike + mob stunned state (yıldız animasyonu)
10. **Mirage Blade** — Karakter 3 illusion'ı dagger spin
11. **Night Aperture** — Karakter etrafında karanlık disc, içine düşman çekiliyor
12. **Phase Step** — Faded silüet step, partial transparency
13. **Preparation** — Karakter cool pose, dagger'lara odaklanmış, mor aura
14. **Rupture** — Mob göğsünden kan fışkırması, kırmızı bleeding mark
15. **Severance** — Çift dagger orta-savurma, mob 2 parça
16. **Shadow Clone** — Yanında karanlık clone, ikisi aynı anda strike

Output B: `06b_shadowblade_v6.png` (1792×1024)

Sheet C (Skill 17-22, 3×2):
17. **Shadow Pin** — Daggers mob'u yere mıhlamış, mob immobil
18. **Shadow Step** — Karanlık portal step, çıkış noktasında siyah duman
19. **Smoke Veil** — Karakter etrafında siyah duman bulutu, silüet
20. **Toxic Eruption** — Karakter etrafında yeşil zehirli gas patlama
21. **Vanish** — Karakter dumana karışmış, %50 görünmez
22. **Veil Burst** — Karanlık duman patlama, mob kör-confused

Output C: `06c_shadowblade_v6.png` (1792×1024)

---

### 07 — RAVAGER (8 skill, 4×2 grid, 1792×1024)

Reference: `07_ravager_south.png`
Weapon: large two-handed axe

Skills:
1. **Berserk** — Karakter etrafında rage aura kırmızı, gözler parlıyor
2. **Axe Throw** — Axe mid-flight dönerken hava, mob önünde
3. **Earthcrack** — Axe yere indi, ground crack genişliyor
4. **Bloodthirst** — Karakter ağzında diş + kan, mob'tan kan damlıyor lifesteal
5. **War Cry** — Karakter haykırış pose, ses dalgaları radial
6. **Whirlwind** — Karakter spinning + axe wide arc, etrafında mob'lar savruluyor
7. **Reckless Strike** — Hızlı offensive strike, defense yok pose
8. **Crimson Roar** — Karakter kırmızı roar enerji koni mob'lara doğru

Output: `07_ravager_v6.png` (1792×1024)

---

### 08 — HEXER (8 skill, 4×2 grid, 1792×1024)

Reference: `08_hexer_south.png`
Weapon: cursed totem (skull-topped staff)

Skills:
1. **Curse Mark** — Mob başı üstünde mor pentagram işareti
2. **Decay Aura** — Karakter etrafında yeşil-mor çürüme bulutu
3. **Necrosis** — Mob vücudunda dağılan siyah çürüme spots
4. **Hex Bolt** — Karakter elinden mor energy bolt, mob önünde
5. **Shackle Curse** — Mor metaphysical chain karakterden mob'a, mob immobil (statik chain frame)
6. **Soul Drain** — Karakter ile mob arası mor soul tether, mob'tan ışık akıyor
7. **Death Wail** — Karakter mor scream pose, mor ses dalgası
8. **Plague Touch** — Karakter mob'a el değdirmiş, yeşil hastalık spreading

Output: `08_hexer_v6.png` (1792×1024)

---

### 09 — BRAWLER (8 skill, 4×2 grid, 1792×1024)

Reference: `09_brawler_south.png`
Weapon: iron gauntlets (fists)

Skills:
1. **Jab** — Hızlı straight punch, mob suratından darbe
2. **Cross** — Power cross punch, mob savruluyor
3. **Uppercut** — Aşağıdan yukarı punch, mob havada
4. **Earth Slam** — İki yumruk yere indi, ground impact ring
5. **Body Lock** — Mob'u yakalayıp tutmuş, mob struggling
6. **Power Strike** — Charged büyük punch, narıncı energy aura
7. **Iron Stance** — Defensive pose, yumruklar yukarıda, gri stone aura
8. **Knockout** — Final punch, mob'un kafası geriye gitmiş, yıldız animasyon

Output: `09_brawler_v6.png` (1792×1024)

---

### 10 — SUMMONER (8 skill, 4×2 grid, 1792×1024)

Reference: `10_summoner_south.png`
Weapon: open tome + floating sigil

Skills:
1. **Summon Wisp** — Karakter yanında cyan wisp orb hovering
2. **Spirit Bind** — Mob'u beyaz spirit chain ile kelepçelemiş (statik chain frame)
3. **Pact Drain** — Karakter elinden mob'a kara energy tether
4. **Soul Link** — Karakter ile mob arası altın soul link line
5. **Ethereal Guard** — Karakter etrafında ışıklı spirit shield orb'ları
6. **Familiar Strike** — Karakterin familiar (küçük spirit) mob'a saldırıyor
7. **Beacon** — Yere atılmış parlak signal/beacon ışık sütunu
8. **Sacrifice** — Karakter yanında summon kaybolurken altın enerji geri akıyor

Output: `10_summoner_v6.png` (1792×1024)

---

## 5. BATCH COMPLETION CHECKPOINTS

ChatGPT her class bittikten sonra şunu doğrula:
- ✅ Karakter canonical sprite'a sadık mı (anatomy/clothing/hair/weapon)?
- ✅ TÜM skill'ler grid'de görünüyor mu (atlama yok)?
- ✅ VFX painterly mi (geometric primitive değil)?
- ✅ Mob proper pixel art mob mu (renkli kare değil)?
- ✅ 30-35° iso perspective doğru mu?
- ✅ Panel altında skill name caption okunaklı mı?

Eğer "no" varsa, o class'ı **regenerate** et ondan sonra devam.

10 class bittiğinde: "BATCH COMPLETE — 10 sheet ready (12 PNG total — Ranger 2 sheet, Shadowblade 3 sheet, diğerleri 1 sheet)" yaz.

---

## 6. SENİN UPLOAD EDECEKLERİN (ChatGPT'ye)

1. **Sprite klasörü:** `STAGING/_chatgpt/sprites_south/` içindeki 10 PNG
2. **v4 reference (kalite baseline, opsiyonel):** `STAGING/concepts/skill_sheets_v4/01_warblade_v4_in_action.png`
3. **Bu master doküman:** `STAGING/_chatgpt/SKILL_SHEET_v6_CHATGPT_MASTER.md`

**Senin tek mesajın:** "BATCH START" — sonra ChatGPT 10 class'ı sırayla üretsin.
