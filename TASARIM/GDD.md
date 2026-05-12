---
status: REFERENCE
faz: 1
tarih: 2026-04-09
ozet: "Game Design Document - ana referans"
---
# GAME DESIGN DOCUMENT — RIMA
*Son güncelleme: 2026-03-30 | Solo dev — Antigravity | Unity 6.3 LTS, 2D URP | Steam hedefi*

> **[REFERENCE — S43+]** Bu belge Kasim 2025 (S41) lore baseline'ini icerir. Skill isimleri/PPU/yon sistemi gibi mekanik detaylar icin canonical kaynaklar: `MASTER_KARAR_BELGESI.md` (Kararlar #1-70), `SINIF_VE_SKILL_KARAR_BELGESI.md`, `TASARIM/dungeon_act1_map.md`, `TASARIM/map_fragment_system.md`, `SYSTEM_MAP.md`. Lore/elevator pitch/temel pacing icin bu dosya hala gecerli -- Master kararlarla desync olan bolumler icin Master oncelik.

---

## 1. OYUN TANIMI

### Elevator Pitch

> "Hades'in oda yapısında MMORPG dual-class build crafting'ini oynuyorsun.
> Act 1'i tek başına geçiyorsun. Boss'u öldürünce oyun ikiye kırılıyor —
> ve o noktadan sonra build tamamen başka bir şeye dönüşüyor."

**Tür:** Top-down 2D roguelite aksiyon (High Top-Down ~35° Diablo 2/PoE açısı — Karar #45)
**Platform:** PC — Steam
**Engine:** Unity 6.3 LTS, 2D URP
**Art:** Pixel art — Aseprite + PixelLab
**Hedef süre:** 55-70 dakikalık run

### Tek Cümle Özeti

MMORPG dual-class sistemi × Slay the Spire skill acquisition × Hades oda yapısı — run'un tam ortasında ikiye kırılan bir build crafting roguelite.

### Temel His

**"Bu build insane."**

MMORPG oynayan herkesin bir kez yaşadığı an: cooldown'ların senkrona girmesi, proc'ların üst üste binmesi, düşmanların eriyip gitmesi. RIMA, bu anı roguelite formatında, her run'da farklı şekilde yaratmayı hedefliyor.

### Referans Oyunlar

| Oyun | Ne Alıyoruz |
|------|-------------|
| **Hades** | Oda yapısı, hub → dungeon → boss akışı, atmosfer |
| **Slay the Spire** | Skill acquisition modeli — oda sonrası 3 kart teklifi |
| **Guild Wars 1** | Dual class felsefesi — 2 sınıftan en iyi 4'ü seç |
| **Enter the Gungeon** | Top-down 2D pixel art kalite referansı |
| **Children of Morta** | URP ışıklandırma, büyük sprite, atmosfer |
| **Shadow of Mordor** | Grudge / Nemesis sistemi ilhamı |
| **Lost Ark / WoW / FFXIV** | Sınıf kimliği, resource bar hissi |

---

## 2. CORE LOOP

### Run Akışı

```
HUB
  └─ 1 primary class seç (10 class'tan — Karar #4)
  └─ Act 1 başlar (8-9 oda — sadece primary class skill'leri)
       └─ Her combat/elite oda: düşmanları temizle → Skill Draft (3 seçenek)
       └─ Her 3 combat oda → Echo Imprint sunusu (LMB/RMB/Dash güçlendirme)
       └─ Oda tipleri: Combat, Elite (+Shop opsiyonel)
  └─ Act 1 Boss öldür → Max HP %50 yenileme
       └─ ──── KIRILMA NOKTASI ────
       └─ 2 random secondary class belirir → 1 seç
       └─ +2 aktif slot açılır (toplam 6)
       └─ Cross-class Pasif aktive olur
  └─ Act 2 başlar (9-11 oda — primary + secondary karışık draft)
       └─ Spirit Encounter eklenir (1 tane)
       └─ Shop eklenir (1 tane)
  └─ Act 2 Boss öldür → Cross-class Ultimate açılır
  └─ Act 3 başlar (9-11 oda — tam dual-class, %45/%45 draft)
       └─ Legendary tier mevcut (nadir)
  └─ Act 3 Boss öldür
  └─ Final (5-6 oda + boss)
       └─ Ölüm: Hub'a dön | Zafer: 3 kapanış seçeneği
```

### Oyuncu Güç Eğrisi

| Aşama | Hissettirilen Şey |
|-------|-------------------|
| Act 1 başlangıcı | "Bu class ne yapıyor, anlıyorum." |
| Act 1 ortası | "Build şekillenmeye başladı." |
| Act 1 boss sonrası | "Tamam. Oyun şimdi başka bir şeye dönüştü." |
| Act 2 ortası | "Ben artık build kuruyorum." |
| Geç Act 2 | "Bu skill kombinasyonu... beklemedim." |
| Act 3 | "Bu build insane." |

### Skill Draft Sistemi

Her combat odayı temizledikten sonra 3 seçenek sunulur:
- **Yeni skill ekle** — mevcut primary/secondary skill havuzundan
- **Mevcut skilli tier atlatır** — Common → Rare → Epic → Legendary
- **Echo Imprint** — LMB / RMB / Dash döngüsünü güçlendiren pasif

**Offer garantisi:** 3 seçenek içinde minimum 1 tanesi "yeni skill VEYA tier upgrade" olmalı.

**Echo Imprint Sunusu:** Her 3 combat odada bir kez, normal Draft'a EK olarak sunulur. 3 kategori: Strike Form (LMB), Outlet Form (RMB), Surge Form (Dash/Resource). Run başına max 4 Imprint.

**Tag Sinerji Bonusu:** Aktif 6 skill slotundan 2 tanesi aynı tag taşıyorsa otomatik pasif bonus aktive olur (draft sonrası hesaplanır). Tam tablo: `TASARIM/ROOM_MECHANICS.md`.

### Aktif Slot Sistemi

Her class'ın 12 skill'i var ama bir run'da hepsini alamazsın — kasıtlı. Draft kararları anlamlı kalsın diye slot sistemi sınırlı:

```
Act 1 başı:        4 aktif slot  (sadece primary class skill'lerinden doldurulur)
Act 1 boss sonrası: +2 slot açılır = toplam 6 aktif slot
                   (yeni slotlar secondary class skill'leriyle doldurulabilir)
```

Act 1'de 4 slotla 12 skill havuzundan seçim yapıyorsun. Her draft "bunu mu alsam, şunu mu upgrade etsem?" sorusunu zorluyor. Secondary class gelince 2 yeni slot daha açılıyor — ama bu sefer iki class havuzu var ve hâlâ her şey sığmıyor.

**Draft Oranları (hangi havuzdan kart çıkıyor):**

| Aşama | Primary | Secondary | Nötr |
|-------|---------|-----------|------|
| Act 1 | %100 | — | — |
| Act 2 başı | %60 | %25 | %15 |
| Act 2 ortası | %50 | %35 | %15 |
| Act 3 | %45 | %45 | %10 |

> Nötr = iki class'a da fayda sağlayan cross-class köprü skill'leri

---

## 3. HARITA SİSTEMİ

### Kısmi Görünür Harita

Tam Slay the Spire gibi açık değil, tam Hades gibi kör de değil.
- Mevcut floor görünür (5-7 oda), ötesi kapalı
- Kapılar oda tipini gösterir (ikon + renk)
- Seçim var — hangi yoldan gideceğini düşünebilirsin

### Yırtık Harita Mekaniği

Her oda temizlenince zemine bir **harita parçası** düşer — almak zorundasın (Hades'teki Ambrosia mantığı).

- 1 saniyelik **ritual animasyon**: parça yerine oturur, mürekkep reveal
- Parça alınmadan sonraki odaya geçilemiyor
- **Puzzle yok** — tamamen estetik; harita parçalanmış bir dünyanın görsel anlatısı

---

## 4. ODA TİPLERİ

| İkon | Tip | Açıklama |
|------|-----|----------|
| ⚔️ | **Combat** | Standart oda. Her temizlemede skill offer. |
| 💀 | **Elite** | Daha zor düşmanlar. Rare+ ödül + küçük HP yenilemesi. |
| 👁️ | **Spirit Encounter** | Bir ruh varlıkla karşılaşma → koşullu build bonusu (bakınız: §5). |
| 🛒 | **Shop** | HP satın al, max HP arttır, skill tier atlatma, gear. |
| 🌀 | **Curse Gate** | HP harca → büyük build bonusu. Risk-reward kapısı. |
| 🎲 | **Event** | İki seçenekli hikaye / karar odası. |
| ❓ | **Unknown** | İkon yok. Ne olduğu belli değil. |

**Unknown oda içeriği (gizli ağırlıklar):**
Combat %25, mini-boss %20, gizli shop %15, spirit %15, tuzak %10, max HP odası %10, minor reward %5.
Boş oda yok.

**HP nasıl dolduruluyor?**
Rest odası yok. HP dağıtık kaynaklardan gelir: Shop, Elite ödülü, Curse Gate terk edince küçük HP, bazı Event sonuçları.

---

## 5. SPİRİT ENCOUNTER SİSTEMİ

Hades'teki tanrı odalarının işlevini — "Bu oda bana bir şey ekleyecek" — RIMA evrenine özgü ruh varlıklarıyla çözüyoruz.

**Fark:** Koşullu offer. Nasıl oynadığın ne teklif göreceğini etkiler.

| Ruh | Tema | Koşullu Offer |
|-----|------|---------------|
| 🔥 **Forge Wraith** | Saldırı, zırh kırma | Koşulsuz — her zaman 2-3 seçenek |
| 🐾 **Shadow Hound** | Hareket, reposition | Son odayı 5 saniyede temizlediysen +1 seçenek |
| 🩸 **Blood Oracle** | HP trade, lifesteal | HP %60 altındaysa en güçlü seçenek görünür |
| 👁️ **Void Seer** *(Faz 2)* | Kaos, şans | İkonlar kapalı — kör seçim |
| ⚔️ **Fallen Champion** *(Faz 2)* | Kombo, zincir | Son 3 odayı chain ile geçtiysen unlock |
| 💀 **Ancient Relic** *(Faz 3)* | Güçlü ama bedelli | Her offer'a curse eşlik eder |

---

## 6. SKILL TİER SİSTEMİ

| Tier | Renk | Efekt |
|------|------|-------|
| **Common** | ⬜ Beyaz | Temel versiyon |
| **Rare** | 🟦 Mavi | +%30 sayısal güç + küçük mekanik ekleme |
| **Epic** | 🟣 Mor | +%60 sayısal güç + anlamlı mekanik değişim |
| **Legendary** | 🟡 Altın | Skill'in çalışma biçimi kökten değişir |

Tier atlatmak = sadece güçlendirme değil, **skill'in nasıl çalıştığını değiştirme.**

---

## 7. CROSS-CLASS SİSTEMİ

Act 1 boss öldükten sonra açılan kırılma noktasının üç katmanı var:

### Cross-class Pasif

Secondary class seçilince otomatik aktive olur. İki class'ın arasındaki köprü görevi görür — her kombo için farklı bir pasif bonus.

Örnek: **Warblade + Shadowblade → Iron Shadow** arketipi
- Cross-class Pasif: Stun'daki düşmana verilen hasarın %20'si Energy olarak geri döner *(Warblade'in CC'si Shadowblade'in kaynağını doldurur)*

### Cross-class Ultimate

Act 2 boss öldükten sonra açılır. Her 28 class kombinasyonu için benzersiz bir Ultimate yeteneği. Bunlar class'ların tek başına yapamayacağı şeyleri yapar — iki class sinerjisinin doruk noktası.

| Kombinasyon | Arketip | Cross-class Ultimate |
|-------------|---------|---------------------|
| Warblade + Elementalist | **Runeguard** | Runic Eruption |
| Warblade + Shadowblade | **Iron Shadow** | Killing Floor |
| Warblade + Ranger | **Vanguard** | Spearhead |
| Warblade + Brawler | **Juggernaut** | Unstoppable Force |
| Elementalist + Shadowblade | **Arcane Shadow** | Phantom Burst |
| Elementalist + Hexer | **Voidcaster** | Void Collapse |
| *(+ 22 diğer kombo)* | | |

28 kombinasyonun tamamı `MASTER_SINIF_VE_CROSSCLASS.md` dosyasında.

---

## 8. META-PROGRESSION — SHATTERED ECHOES

Run'dan run'a taşınan ilerleme sistemi. Roguelite'ta her ölüm "sıfırlama" değil — bazı şeyler kalıcı olarak açılıyor.

**Shattered Echoes** = meta para birimi. Run boyunca düşman öldürmekten ve oda tamamlamaktan toplanır, Hub'a dönünce kalıcı olarak birikiyor.

**Echoes ne için kullanılır:**
- Yeni class'ların kilidi açma (bakınız §9)
- Hub NPC'lerinden kalıcı küçük güçlendirmeler
- Yırtık haritayı zenginleştiren upgrade'ler (Cartographer)

**Lore bağı:** Echoes = Fracturing'de saçılan class "yüzleri." Toplayınca bütünleşiyorsun. Her yeni class açılışı bir anı parçasını geri getiriyor.

---

## 9. SINIFLARA GENEL BAKIŞ

Her class benzersiz bir kaynak mekaniğine, 12 skill'e ve imza bir burst yeteneğine sahip.
Run başında 1 class seçilir; Act 1 boss'tan sonra ikinci class eklenir.

| Class | Fantasy | Kaynak | [V] Burst |
|-------|---------|--------|-----------|
| **Warblade** | "Yaklaş. Sabitle. Zırh kır. İnfaz et." | Rage — hasar vererek dolar (Dominance) | Bladestorm |
| **Elementalist** | "Her şeyi yakıyorum. Ama önce ritmi buluyorum." | Mana + Elemental State (Convergence) | Trinity Storm |
| **Shadowblade** | "Görmüyorsun. Zaten geç." | Energy + Combo Points (Predation) | Shadow Dance |
| **Ranger** | "Sana ulaşamazsın. Her saniye kayıp veriyorsun." | Focus — mesafeyle birikir (Kill Zone) | Rain of Arrows |
| **Ravager** | "Az canken daha tehlikeliyim. Bu hata değil, strateji." | Fury — hasar alarak dolar (Carnage) | Berserk Mode |
| **Ronin** | "Çek. Kes. Kın. Bir nefeste." | Tension — iaido draw cycle (Flow Cut) | Final Draw |
| **Gunslinger** | "Mermin yok. Senin zamanın da yok." | Heat — ateş→soğutma ritmi (Showtime) | Last Round |
| **Brawler** | "Düşersen kalk. Ama önce yumruğum kalkar." | Charge — kombo darbeleri (Crowd Hype) | Overdrive |
| **Summoner** | "Ben savaşmıyorum. Feda ediyorum. Ve feda anı en güçlü andır." | Charges — zaman ve ölümle (Grave Chorus) | Army of the Dead |
| **Hexer** | "Sabır. 10'a gelince sen bitiyorsun." | Hex Stacks — düşman başına 0-10 (Dread) | Hex Cascade |

> 10-class roster LOCKED (MASTER #4 — 2026-04-11). Paladin/Crusader/Lancer eski iterasyonlardan kaldırıldı.

### Class Unlock Sistemi

Her class = Fracturing'de saçılan bir "yüz." Unlock = o yüzü geri kazanmak.

| Class | Nasıl Açılır |
|-------|-------------|
| Warblade | Başlangıçta açık |
| Elementalist | 80 Echo |
| Ranger | 80 Echo |
| Shadowblade | 150 Echo VEYA Act 1'i 3 kez tamamla |
| Ravager | 150 Echo VEYA Warblade ile Act 2 boss öldür |
| Summoner | 200 Echo VEYA üst üste 3 run'da Act 2'ye ulaş |
| Hexer | 200 Echo + Elementalist ile run bitir |
| Brawler | 350 Echo + herhangi class ile Act 3'e ulaş |

---

## 10. DEMO SINIF: WARBLADE ⚔️

**Rage VEREREK dolar** — her vuruşta +10, CC'li düşmana +20, boşta -5/sn.
**[V] BLADESTORM** — Rage 100: 5s spin, CC immune, her 0.5s AoE hasar.

**Chain sistemi:** Birini kullanınca diğerinin bonusunu tetikler. Rotasyon bu zincirlerin senkronuna dayanır.

| # | İsim | Efekt | Chain Bonusu |
|---|------|-------|--------------|
| 1 | **Iron Charge** ★ | 8m dash + 1.5s stun, Rage+20 | Stun'daki hedefe ilk vuruş → +%80 hasar |
| 2 | **Crippling Blow** | Büyük hasar + iyileşme -%50 (6s) | Iron Charge sonrası → iyileşme -%100 |
| 3 | **Iron Crush** | 6s: tüm hasar +%30 | Burst window açıkken → katlanır |
| 4 | **Gravity Cleave** | 4m çekme + %140 hasar, 0.8s slow | Iron Charge sonrası → çekilenler 1.5s stun |
| 5 | **Sunder Mark** | 8s zırh -%40 | Death Blow aktifken → zırh -%60 |
| 6 | **War Stomp** | 3m knockup 2s, Rage+25 | Bladestorm sırasında → +1s uzar |
| 7 | **Ironclad Momentum** | 6s: hasar %30 yok sayılır, 10 hasar = +10 Rage | War Stomp sonrası → %50'ye çıkar |
| 8 | **Iron Counter** | 0.8s pencere: vurulursa %180 karşı + Rage+25 + stun | Rage 80+ → 2× tetiklenir |
| 9 | **Blade Rush** | 6m dash + çizgideki herkese %120, Rage+15/hedef | 3+ hedef → Rage+50 |
| 10 | **Battle Surge** | 8s: Rage harcaması = HP +%5 | Rage 80+'ta → süre 12s |
| 11 | **Deep Wound** | Bleed DoT 8s + Rage+20 | Iron Crush window → bleed tick 2× |
| 12 | **Death Blow** | SADECE HP<%30: %400 hasar, Rage boşaltır | Crippling Blow aktifken → %600 |

**Build Eksenleri:**
- **Execution:** Iron Charge → Crippling Blow → Iron Crush → Death Blow
- **Control Breaker:** Gravity Cleave → War Stomp → Sunder Mark → Death Blow
- **Last Stand:** Ironclad Momentum → Iron Counter → Battle Surge → Death Blow

---

## 11. DEMO SINIF: ELEMENTALİST 🔥

**Kaynak:** Mana (0-100, +8/sn) + Elemental State — Fire veya Frost stack'ları (0-5).
**[V] INFERNO** — Mana 100: 7s arena-wide ateş yağmuru.

**Rhythm sistemi:** Fire ve Frost skill'leri sırayla kullanınca birbirinin etkisini patlatır. Sadece spam değil, ritim yönetimi.

| # | İsim | Efekt | Chain Bonusu |
|---|------|-------|--------------|
| 1 | **Fireball** ★ | Orta hasar + ateş DoT 4s, Fire State+1 | 3 ard arda → 3.'de Living Bomb ücretsiz |
| 2 | **Glacial Spike** | 6m buz hattı: %40 slow + %180 hasar, Frost State+2 | Fireball DoT aktifken → Freeze 2s + DoT hasarı tek seferde patlar |
| 3 | **Living Bomb** | 5s sonra patlama, öldürünce 3 komşuya kopyalanır | Glacial Spike slow altındaki hedefe → patlama yarıçapı 2× |
| 4 | **Blink** | 6m ışınlanma, geçilen düşmanlara hasar, sonraki spell +%20 | Düşmanın içinden geçince → 0.5s stun |
| 5 | **Frozen Orb** | Yavaş hareket eden küre, yolundakileri 5s chill | Orb üzerinden Blink → Orb patlar, Frozen 2s |
| 6 | **Arcane Blast** | Her cast +%20 hasar (+%30 mana maliyet). 4. cast Barrage açar | — |
| 7 | **Meteor** | 1.5s kanal → büyük AoE knockdown | Frozen/slowed hedef → knockdown 3s + hasar +%50 |
| 8 | **Mirror Image** | 2 kopya 8s, hasar önce kopyaya gelir | Kopyalar ölünce → AoE ölüm patlaması |
| 9 | **Chain Lightning** | 5 hedefe sekiyor | Yavaşlamış hedef → 7 seki |
| 10 | **Arcane Surge** | 8s: mana regen +%100, cast süresi -%50 | Blink sonrası → varış noktasında patlama, sonraki Meteor mana maliyeti 0 |
| 11 | **Combustion** | 8s: tüm Fire spell instant cast, mana maliyet ×2 | Fire State 5 stack → mana maliyet artışı yok |
| 12 | **Blizzard** | 3s kanal → 5m alana 8s slow+tick | Meteor'dan önce kullanılırsa → knockdown 4s |

**Build Eksenleri:**
- **Fire Burst:** Combustion → Fireball → Living Bomb → Meteor
- **Frost Lock:** Glacial Spike → Blizzard → Frozen Orb → Meteor
- **Arcane Storm:** Arcane Surge → Chain Lightning → Arcane Blast → Blink

---

## 12. DEMO SINIF: SHADOWBLADE 🗡️

**Kaynak:** Energy (0-100, +15/sn) + Combo Points (0-5).
**[V] SHADOW DANCE** — Energy 100 + CP 5: 8s her saldırı sonrası otomatik stealth.

**Combo sistemi:** CP biriktirmek finisher skill'lerin gücünü artırır. Stealth'ten çıkınca tek büyük hasar, ardından tekrar gizlenme döngüsü.

| # | İsim | Efekt | Chain Bonusu |
|---|------|-------|--------------|
| 1 | **Backstab** ★ | Arkadan: %200 hasar + 3CP. Önden: normal hasar | Shadowstep sonrası → +%50 hasar |
| 2 | **Hemorrhage** | Bleed 8s DoT + 2CP, öldürünce yakına yayılır | Bleed aktif hedefe Rupture → hasar +%100 |
| 3 | **Rupture** | 5CP finisher: bleed + hasar, CP'ye göre süre uzar | Zaten bleed varsa → birikmiş hasar anında patlar |
| 4 | **Shadowstep** | Hedefe 8m ışınlan, 0.5s stun, Energy-25 | Evasion aktifken → CD sıfırlanır |
| 5 | **Kidney Shot** | 5CP: 4s stun, CP'ye göre uzar | — |
| 6 | **Ambush** | SADECE stealth'ten: %300 hasar + 4CP + %20 slow | 3s+ stealth → Cold Blood +%100 hasar |
| 7 | **Fan of Knives** | 360° AoE, tüm aktif debuffları tüm düşmanlara uygular | — |
| 8 | **Evasion** | 4s %100 dodge, her dodge = +1CP | Evasion bitince → sonraki saldırı guaranteed crit |
| 9 | **Mirage Blade** | 3s: geçilen konumlara gölge bırakır, dokunan düşman %100 hasar+1CP | Shadowstep sonrası → tüm gölgeler hedefe atılır, %250 hasar + 1s stun |
| 10 | **Toxic Eruption** | 5CP: hedefteki tüm debuffları patlatır, her stack %150 hasar | Hemorrhage aktif → debufflar tüketilmez, süreleri sıfırlanır + hasar +%50 |
| 11 | **Preparation** | Tüm Shadowblade CD'leri sıfırla | Evasion aktifken → CD 60s |
| 12 | **Vanish** | Savaşta anlık stealth 3s, 50s CD | Vanish sonrası Ambush → Cold Blood garantili |

**Build Eksenleri:**
- **Assassin:** Ambush → Vanish → Backstab → Preparation
- **Bleed Lord:** Hemorrhage → Toxic Eruption → Rupture → Fan of Knives
- **Phantom:** Mirage Blade → Shadowstep → Evasion → Kidney Shot

---

## 13. DEMO SINIF: RANGER 🏹

**Kaynak:** Focus — 4m+ uzakta: +10/sn | 2m altında: -20/sn. Focus 75+: hasar +%25 | Focus 100: sonraki skill ücretsiz.
**[V] RAIN OF ARROWS** — 30s sabit CD: 5s tüm arena yağmur.

**Mesafe sistemi:** Yakına gelince Focus eriyip hasar bonusu kaybedilir. Düşmanı uzakta tutmak hem kaynak yönetimi hem savaş taktiğidir.

| # | İsim | Efekt | Chain Bonusu |
|---|------|-------|--------------|
| 1 | **Aimed Shot** ★ | 1.5s şarj → büyük hasar + %50 crit | Root/stun altındaki hedefe → instant |
| 2 | **Concussive Arrow** | Knockback 4m + root 2s | Disengage sırasında → uzaklık 6m + slow 3s |
| 3 | **Barbed Net Shot** | Ağ fırlatır, 2s root + 4s kanama | Disengage sonrası → ağ 4m alana yayılır, tüm düşmanları sabitler |
| 4 | **Explosive Trap** | Zemine tuzak, 3s sonra patlama + slow 3s | — |
| 5 | **Multi-Shot** | Delici ok: tüm düşmanlardan geçer, her birine kanama stack | 5+ düşman → tüm CD -3s |
| 6 | **Disengage** | 6m geri atla, slow alanı bırak | Disengage + Aimed Shot → atlama sırasında instant ateş |
| 7 | **Black Arrow** | DoT + özel: bu DoT ile ölen düşman 8s ruh bırakır | — |
| 8 | **Volley** | 4m alana 3s yağmur, slow + tick | Explosive Trap üzerine → tam kilitlenme |
| 9 | **Rapid Fire** | 2s kanal: 8 hızlı ok, Focus-30 | Focus 100 → 10 ok, maliyet yok |
| 10 | **Tethering Arrow** | Hedefe bağ ok: 4s 4m çember içine hapseder | Ranger 8m+ uzağa çıkarsa → zincir kopar, %300 kritik + Focus +%50 |
| 11 | **Flare** | Stealth açığa çıkar + 6s slow alanı, Focus+20 | — |
| 12 | **Point Blank** | ≤2m: ×3 hasar + 5m knockback. Focus 100 gerektirir | Disengage sonrası → CD sıfırlanır |

**Build Eksenleri:**
- **Sniper:** Aimed Shot → Tethering Arrow → Rapid Fire → Flare
- **Trap Master:** Explosive Trap → Volley → Barbed Net Shot → Disengage
- **Kite Lord:** Concussive Arrow → Black Arrow → Multi-Shot → Point Blank

---

## 14. DİĞER SINIFLAR — ÖZET

Bu 6 class Faz 2-5'te açılır. Tam skill tabloları `SINIF_VE_SKILL_KARAR_BELGESI.md` dosyasında.

### Ravager 👊
**Fantasy:** "Az canken daha tehlikeliyim."
**Kaynak:** Fury — SADECE hasar alarak dolar. HP düştükçe daha hızlı.
**Ayrım:** Warblade Rage'i vererek doldurur. Ravager alarak. İkisi kombinlenince "ver de al" döngüsü oluşur.
**[V] Berserk Mode** — kan siklozu (2.5 yarıçap pasif AoE + 0.5s single-target darbe), kill +0.8s (max +3s).
**Eksenler:** Glass Cannon / Fury Engine / Crowd Crusher

### Ronin 🗡️
**Fantasy:** "Çek. Kes. Kın. Bir nefeste."
**Kaynak:** Tension — iaido draw cycle (sheath/draw kimliği LOCKED).
**Mekanik:** Pre-draw timing → frame-perfect cut → state apply (Opened).
**[V] Final Draw** — empowered iaido cast, Perfect Condition trigger.
**Eksenler:** Frame-Perfect Counter / Draw Pressure / Stillness

### Gunslinger 🔫
**Fantasy:** "Mermin yok. Senin zamanın da yok."
**Kaynak:** Heat — ateş→soğutma ritmi. Heat MAX değil ZERO ulti açar (Karar #54).
**Mekanik:** Run-and-gun, dual-pistol, kinetic okuma.
**[V] Last Round** — Heat ZERO trigger empowered burst.
**Eksenler:** Heat Cycle / Charge-Release / Mobile Burst

### Brawler 👊
**Fantasy:** "Düşersen kalk. Ama önce yumruğum kalkar."
**Kaynak:** Charge — 5 stack Cracked → Shattered (Brawler-only state).
**Mekanik:** Mash + timing rotation. Whiff/evade body movement → dodge into whiff (Off-Balance).
**[V] Overdrive** — Crowd Hype V, Charged State bankalanabilir RMB ile.
**Eksenler:** Shattered Engine / Counter-Body / Crowd Hype

### Summoner 💀
**Fantasy:** "Ben savaşmıyorum. Feda ediyorum. Ve feda anı en güçlü andır."
**Kaynak:** Charges (0-4) — her 8s +1, minyon ölünce +1 anında.
**Mekanik:** Minyon topla → en güçlü anda hepsini feda et → patlama hasarı + Charge patlaması.
**[V] Army of the Dead** — tüm Charge doluyken: 6s minyonlar +%150, ölümsüz.
**Eksenler:** Sacrifice Engine / Army Commander / Lich Burst

### Hexer 🔮
**Fantasy:** "Sabır. 10'a gelince sen bitiyorsun."
**Kaynak:** Hex Stacks — düşman başına 0-10. Faz sistemi: 0-3 Debuff / 4-6 Pressure (+%20 güç) / 7-9 Overload (+%30 hasar alır) / **10 → HEXBLAST**.
**[V] Hex Cascade** — bir hedefte 10 stack: tüm düşmanlara 3 stack kopyalanır.
**Eksenler:** Patient DoT / Hex Overload / Soul Burst

---

## 15. BOSS SİSTEMİ

### Tasarım Felsefesi

Her boss, **build'inizin bir zayıflığını test eder.** Narratif bunu destekler, önde yürümez.

### Act 1 — Rotating Guardians (3 varyant)

Her run Act 1 boss'u bu üçünden biridir. Oyuncunun Act 1'i ezberlemesini önler.

| Boss | Mekanik Odak | Nasıl Zorluyor |
|------|-------------|----------------|
| **The Iron Warden** | Kalkan / zırh | Dönemsel kalkan — kırılmadan hasar verilemiyor |
| **The Void Warden** | Teleport + alan kontrolü | Kaybolur, void zone bırakır, geri gelir — pozisyon şart |
| **The Chain Warden** | Bağlama + kısıtlama | Zincir fırlatır — hareketsiz kalırsan Rage/Momentum kaybı |

### Act 2 — The Fractured King (sabit, 2 faz)

- **Faz 1:** Normal savaş — kırık dünya kalıntılarından oluşan construct.
- **Faz 2 tetikleme:** HP bazlı değil — oyuncu 10 saniyede 3+ farklı skill kullanırsa açılır. *(Agresif ve çeşitli oynanışı ödüllendirir.)*
- **Faz 2:** Construct parçalanır, her parça bağımsız döner. Tamamen farklı saldırı seti.

### Act 3 — The Hollow Sovereign (build-adaptive)

Bu boss, elinizdeki skill tag'lerini okur ve buna göre adaptasyon yapar:

| Senin Build'in | Boss'un Cevabı |
|---------------|----------------|
| Çoğunlukla AoE | Zırh katmanı + sıkı gruplaşma |
| Single target ağırlıklı | Birden fazla bağımsız hedef spawnar |
| HP harcayan build | Lifesteal'i bloke eden alan |
| Uzun cooldown'lu skill ağırlığı | Sık interrupt atakları |

### Final — The Nexus Core (çok fazlı, build-aware)

- **Faz 1:** Primary class'ının signature skill'ine karşı gelen mekanik
- **Faz 2:** Cross-class pasif'inizin zayıflığını hedef alır
- **Faz 3 (Legendary varsa):** Legendary skill'inizin yansımasını sahaya çıkarır

**Grudge / Nemesis Bağlantısı:** Aynı boss'a 3+ kez öldüyseniz, o boss o kill method'unu hatırlar → +%35 extra direnç uygular.

---

## 16. HİKAYE — RIMA

> *"The Fracturing bir felaket değildi yalnızca. Bir tercihti.*
> *Ve sen, o tercihin hem faili hem kalıntısısın."*

### Dünya

Dünyalar arasında yayılan, geri çevrilemez bir tüketim vardı. Onu durdurmak için bağlantılar tek elden, tek anda, geri dönüşsüz koparıldı — **The Fracturing**. Hesap yapıldı, bedel kabul edildi. Ama bedel çok fazla çıktı.

O karar ikiye ayrıldı:

- **Nexus Core** — kararın donmuş iradesi. Değişemiyor, şüphe edemiyor. Seni "kopyalamıyor": senin olabileceğin yüzleri biliyor, çünkü aynı kaynaktan geliyor. O durdu. Sen devam ettin.
- **Sen** — kararın bedelini taşıyan, nedenini unutmuş, hâlâ değişebilen parça. Her run biraz daha hatırlıyorsun.

**Kahraman mı? Suçlu mu? Kurban mı?** — Kasıtlı olarak belirsiz bırakıldı.

### Mekanik ↔ Lore Bağı

| Mekanik | Lore Anlamı |
|---------|-------------|
| Tek class ile başlama | Yalnızca bir yüzünü hatırlıyorsun |
| Secondary class Act 1 boss'tan sonra | Daha derine inince başka bir yüz geri geliyor |
| Run'da ölüp tekrar dönme | Mühür — ceza değil, zorunluluk |
| Nexus Core'un faz taklidi | Senin kaynaklarını biliyor, ondan geldi |

### Hub Karakterleri

| Karakter | Rol | İz Bırakan Satır |
|----------|-----|------------------|
| **The Ferryman** | Meta-progression | Run 9'da ilk kez oturur — tanıklığını anlatır, her şeyi bilmiyor |
| **Vrel** | Craft / upgrade | *"Bu silahı daha önce tutmuş biri vardı. Ellerin aynı şekli taşıyor."* |
| **Sister Mourne** | HP / healing | *"Senin kaybın... farklı."* |
| **The Cartographer** | Harita upgrade | Haritasında eski bir el yazısı notu |

### Hikaye Açılış Sırası (9 run)

```
Run 1-3:  Ferryman seni "the Bearer" diye çağırır — açıklamadan.
          Cartographer haritasında: "Bu yolu bilen biri vardı. Artık bilmiyor."

Run 4-6:  Nexus Core, açmadığın skill'leri kullanıyor.
          Vrel: "Bu silahı daha önce tutmuş biri vardı. Ellerin aynı şekli taşıyor."

Run 7-8:  Echo fragment → karar anı anısı (eller titriyor, yüz yok)
          Mourne: "Senin kaybın... farklı."

Run 9:    Ferryman ilk kez oturuyor. Tanıklığını anlatıyor — her şeyi bilmiyor,
          bazı yerlerde kendini düzeltiyor. Oyuncu boşlukları dolduruyor.

Run 10+:  Threshold'da üç kapı belirir.
```

### Üç Kapanış

| Kapı | Ton | Bedel |
|------|-----|-------|
| **"Kal"** | Melankolik huzur | Loop devam eder — ama artık senin seçimin |
| **"Kır"** | Gerçek risk, gerçek kayıp | Bazı NPC'ler yok olur |
| **"Taşı"** | En saf roguelite sonu | Gerçeği öğrendin, yine de döneceğin kapıya yürüyorsun |

---

## 17. ACT ORTAMLARI VE GÖRSEL TON

| Act | Ortam | Palet | Hissettirilen |
|-----|-------|-------|--------------|
| **Act 1** | Shattered Ruins — kırılmış ama heybetli | Gümüş beyaz + buz mavisi `#7BA7BC` | Ayna parçaları gibi keskin; soğuk ama parlak |
| **Act 2** | Bleeding Wastes — derin yara, canlı | Derin mor + kızıl altın `#9E4FE0` / `#C8502A` | Dünyanın kalbinden kanayan renk |
| **Act 3** | Core Approach — gerçeklik inceliyor | Void siyah + incandescent altın `#FFD700` | Kozmik, aşkın; her şeyin kaynağında |
| **Final** | Nexus Core — tüm class renkleri | Saf beyaz + siyah + class VFX renkleri | Ayna — senin her şeyini biliyor |

**Genel ton: Fractured Epic.** Dünya kırılmış ama görsel olarak DRAMATIK ve CANLI — Hades benzeri. Void karanlığına karşı parlak kontrastlar: buz mavisi, parlaklık, derin morlar, incandescent altın. Grimdark değil: karakterler ifadeli ve güçlü, düşmanlar bu varlığı seçmiş gibi görünür, harabeler büyük bir şeyin kalıntısı gibi heybetli. Yırtık harita / eski mürekkep estetiği. Güçlü class siluetleri.

**Teknik:** Unity 6.3 LTS, URP 2D, Global Light 2D + Point Light 2D. Pixel art isometric — sprite boyutları: karakter 128×128 canvas (PPU=64), boss 256×256, zemin tile 64×32, duvar tile 64×96.

---

## 18. ÜRETİM FAZLARI

### Faz 0 — Proje İskeleti ✅
Unity kurulumu, URP 2D, Input System, klasör yapısı, ScriptableObject şemaları, temel scene akışı.

### Faz 1 — Combat Prototype 🔄 (şu an burada)
- Oynanabilir karakter: 8-yön hareket, dash, temel saldırı
- 1 class: Warblade (6 skill aktif)
- Rage sistemi
- Temel düşman AI
- Oda temizleme loop'u
- Skill offer UI (Common tier)
- **Hedef:** Combat hissi çalışıyor mu?

### Faz 2 — First Playable Loop
- 4 class demo seti
- Act 1 (8-9 oda)
- Elite oda + shop
- Act 1 boss (Iron Warden implementasyonu)
- Skill draft sistemi + Echo Imprint temel set
- Yırtık harita sistemi

### Faz 3 — Secondary Class Break
- Boss sonrası class seçim ekranı
- +2 aktif slot (toplam 6)
- Cross-class passive sistemi (28 kombo)
- Mixed draft oranları
- Echo Imprint full set (10 class)
- Tag Sinerji Bonusu aktif
- Spirit Encounter (3 tip)

### Faz 4 — Full Demo
- Act 2 + Fractured King boss
- Cross-class Ultimate
- Epic skill tier
- Event odaları, Curse Gate, Unknown
- Temel meta-progression (Echo toplanır + harcanır)
- **Hedef:** 30-45 dakikalık tam run

### Faz 5 — Early Access
- Tüm 8 class
- Act 3 + Final Boss
- Legendary tier
- Tüm 28 cross-class kombo
- Grudge / Nemesis sistemi
- Tam meta-progression

---

## 19. TEMEL TASARIM KARARLARI

> Bu bölüm tasarım sürecinde alınan önemli kararları özetler.

**"İki class run başında değil, Act 1 sonrası"**
Oyuncu önce tek class kimliğini öğrenir. Boss öldürmek "başarı hissi" ve secondary class'ı "ödül" olarak sunar. Ayrıca Act 1 balance'ı çok daha temiz.

**"Act 1'de 4 slot, Act 1 boss sonrası +2 = toplam 6"**
Her class'ın 12 skill'i var ama bir run'da en fazla 6 alabilirsin — ve bunların çoğu zaman iyi bir mix yapman gerekiyor. Act 1'de sadece 4 slotla başlamak: build'in kimliğini erken kurmana zorluyor, "her şeyi alayım" seçeneğini kapatıyor. Sonraki +2 slot hem ödül hem yeni karar: iki slot için iki class'tan hangisini seçeceksin?

**"Rest odası yok"**
HP trade kararları olmadan roguelite gerginliği kaybolur. HP birden fazla kaynaktan, dağınık olarak geliyor — bu kasıtlı.

**"Adaptive boss için HP bazlı faz değil, oynanış bazlı faz"**
Fractured King'in Faz 2'si HP'ye göre değil, 10 saniyede 3 farklı skill kullanımına göre açılıyor. Pasif oynayan zaten zor geçer — ama agresif oynayanı ödüllendiriyor.

**"Zorluk modu — ertelenmiş (Faz 2)"**
3 zorluk seviyesi planlandı: Echo (kolay) / Rift (normal) / Fracture (zor) + bonus Void (permadeath). Temel loop çalışmadan sabit zorluk anlamlı değil. Skill Draft + Curse odası + Act 1 boss hazır olunca eklenecek.

