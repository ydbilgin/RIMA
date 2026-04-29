# ChatGPT Derin Araştırma Promptu
*Bu dosyayı kopyalayıp ChatGPT'ye yapıştır. "Derin araştırma" modunu aç.*

---

## BAĞLAM: GELİŞTİRDİĞİM OYUN

Ben solo bir game developer'ım ve aşağıdaki oyunu geliştiriyorum. Senden bu oyun için ilgili tüm oyunlardan derin araştırma yapmanı, gerçek verilerle desteklenen bulgular getirmeni ve sistemlerimi iyileştirmem için somut öneriler üretmeni istiyorum.

### Oyunum Nedir

**Tür:** Flat top-down 2D roguelite aksiyon
**Platform:** PC — Steam
**Engine:** Unity 6.3 LTS

**Tek cümle:** MMORPG dual-class sistemi + Slay the Spire skill acquisition + Hades oda yapısı + Grudge nemesis sistemi.

**Temel his:** MMORPG oynayan birinin "bu build insane" anını — cooldown'ların senkrona girmesi, proc'ların üst üste binmesi, düşmanların eriyip gitmesi — solo roguelite formatında yaratmak.

---

## OYUNUN SİSTEMLERİ (DETAYLI)

### 4 Temel Sütun

**SÜTUN 1 — ROLE BREAK (Dual-Class)**
Run başında 2 sınıf seçilir (8 sınıftan). İki mod:
- **Fusion modu:** İki sınıfın 12'şer aktif skill havuzu karışık sunulur. Oyuncu "Signature + Weighted Draft" sistemiyle oda oda 4 aktif skill biriktirir. Asla "12 skill'den seç" ekranı görmez — her oda 3 teklif gelir, birikerek inşa edilir.
- **Stance modu** (meta-progression unlock): [TAB] ile iki tam kit arasında geçiş.
28 benzersiz sınıf kombinasyonu, her birinin kendine özgü arketip adı, cross-class pasifi ve cross-class ultimate'ı var.

**SÜTUN 2 — ROTATION ROGUE (Skill Rotasyonu)**
- Slot sistemi: Q/W/E/R + F/Space — 4-6 aktif skill
- Her skill 1-2 "tag" taşır: ⚓ ANCHOR / ▶ OPENER / ⚡ CHAIN / ↑ BUILDER / ↓ SPENDER / 💥 FINISHER / ⬡ CONTROL / ✦ AMPLIFIER
- Proc koşulları: "Charge sonrası Mortal Strike → iyileşme -%100" gibi zincirleme bonuslar
- Skill acquisition: Oda sonrası 3 teklif — erken odalar Core skill ağırlıklı, geç odalar Master skill ağırlıklı
- Slot 5-6 sadece FLUX veya Boss Soul ile açılır

**SÜTUN 3 — GRUDGE (Adaptif Düşman)**
- Elite düşmanlar nasıl öldürüldüklerini hatırlıyor (ateş, buz, bleed, stun vb.)
- Bir sonraki karşılaşmada o yönteme +%35 direnç kazanmış geliyor
- Grudge Badge haritada görünür (🔥❄⚡☠)
- Nemesis Elite: run boyunca 3 act'te de karşına çıkan, ölçekli ödüllü özel elite
- Scope: run-bazlı (run bitince sıfırlanır, meta hafıza yok)

**SÜTUN 4 — SINIF BURST (Kaynak Sistemi)**
Her sınıfın kendine özgü kaynağı var, [V] ile sınıfa özel burst patlaması tetiklenir.

### 8 Sınıf — Kaynak Sistemleri

| Sınıf | Kaynak | Dolma | [V] Burst |
|-------|--------|-------|-----------|
| **Warblade** | Rage (0-100) | Hasar al/ver | Bladestorm — 5s spin CC immune AoE |
| **Elementalist** | Mana (0-100, +8/sn regen) + Fire/Frost State | Otomatik regen | Inferno — 7s arena-wide ateş |
| **Rogue** | Energy (0-100, +15/sn) + Combo Points (0-5) | Otomatik regen | Shadow Dance — 8s her saldırı sonrası stealth |
| **Ranger** | Focus (0-100) — 4m+ uzakta +10/sn, 2m altında -20/sn | Mesafe bazlı | Rain of Arrows — 30s sabit CD |
| **Brawler** | Fury (0-100) — SADECE hasar ALARAK dolar | Hasar almak | Berserk Mode — 15s defense ignore +%200 hasar |
| **Paladin** | Holy Power (0-100) — Builder/Spender döngüsü | Builder skill'ler | Avenging Wrath — 10s %30 invuln +%50 hasar |
| **Summoner** | Charges (0-4, auto +1/8s) | Otomatik + minyon ölümü | Army of Dead — 6s tüm minyonlar +%150 |
| **Hexer** | Hex Stacks (0-10 per enemy, 5s decay) | Her skill +1-3 stack | Hex Cascade — tüm düşmanlara 3 stack kopyala |

### Hexer Faz Sistemi (önemli mekanik)
```
0-3 Stack:  Debuff Phase   — zayıf tick hasar
4-6 Stack:  Pressure Phase — tüm skill'ler +%20 güç
7-9 Stack:  Overload Phase — düşman +%30 daha fazla hasar alır
10 Stack:   HEXBLAST       — anında patlama, CD sıfır
Her faz geçişinde: görsel+ses feedback
```

### Her Sınıfın 12 Aktif Skill Havuzu — Seçim Mimarisi

**Neden 12:** C(12,4) = 495 kombinasyon vs C(8,4) = 70. Her run'da ortalama %60'ı görünür → replayability.

**Skill tier sistemi:**
- **Core** (8 skill): Tek başına güçlü, setup gerektirmez
- **Advanced** (3 skill): Sinerji gerektirir, 1+ class skill ile çalışır
- **Master** (1 skill): 2+ class skill olmadan offer pool'a girmez

**Oda teklif ağırlıkları:**
- Oda 1-3: Core %60 / Advanced %20 / Master %0
- Oda 4-7: Core %35 / Advanced %40 / Master %10
- Oda 8+: Core %20 / Advanced %30 / Master %30

### 28 Cross-Class Kombinasyon (örnek)

| Combo | Arketip | Cross-Class Ultimate |
|-------|---------|---------------------|
| Warblade + Hexer | Doombringer | Death Sentence — CC + Hex Stack + Rage patlaması |
| Brawler + Paladin | Fallen Saint | Last Rites — 8s ölümsüzlük + her vuruş heal + hasar reflect |
| Summoner + Hexer | Plague Doctor ⭐ | Pandemic — summonlar explode → Hex Cascade → yeni summon dalgası |
| Rogue + Ranger | Phantom Hunter | Ghost Protocol — 5s bullet time + marked + stealth |
| Elementalist + Hexer | Voidcaster | Void Collapse — tüm DoT'lar anında patlar |

### Güçlenme Sistemleri

```
A. Oda sonu ödülü → skill ekle / upgrade et / pasif al (her oda)
B. Shop → Soul Dust harca (nadir oda)
C. FLUX/Reforge → skill değiştir (run başına max 1, %5-8 şans)
D. Boss Soul → skill kalıcı mutasyona uğrar (3 tip: HASAR/CC/PROC)
E. Sınıf Burst → [V] tuşu
F. Meta progression → Hub'da Soul Dust harca, kalıcı unlock
```

**Slot gating:** Slot 1-4 normal oda ödülleri. Slot 5 sadece FLUX/Elite. Slot 6 sadece Boss Soul.

**Rift Shard** (max 3/run, [G]): Primary bar doldur VEYA 1 cross-class ult charge ver.

### Skill Tag Sistemi

| Tag | Sembol | Tanım |
|-----|--------|-------|
| ANCHOR | ⚓ | Tek başına güçlü, setup gerektirmez |
| OPENER | ▶ | Zincirin ilk adımı |
| CHAIN | ⚡ | Belirli skill sonrası bonus tetiklenir |
| BUILDER | ↑ | Kaynak/setup üretir |
| SPENDER | ↓ | Kaynağı/setup'ı harcar |
| FINISHER | 💥 | Koşullu büyük hasar |
| CONTROL | ⬡ | CC: stun/root/slow/knockback — başlangıçta gizli |
| AMPLIFIER | ✦ | Diğer skill'lerin gücünü artırır — başlangıçta gizli |

CONTROL ve AMPLIFIER ilk kez tetiklenince açılır ("SİNERJİ BULUNDU" bildirimi — Hades Duo Boon'dan ilham).

### Kayıt Defteri (Codex alternatifi)
Codex yok. Progressive disclosure sistemi:
1. Skill tooltip her zaman görünür (ama chain detail gizli)
2. İlk kez zincir tetiklenince: bildirim + Kayıt Defteri'ne eklenir
3. Hub'da Kayıt Defteri: sadece gerçekten tetiklenmiş zincirler görünür
4. Cross-class sinerji bildirimleri: "YENİ SİNERJİ BULUNDU"

### Grudge + Nemesis Elite Detayı
```
Normal elite: Grudge Badge taşır, Act içinde yeniden çıkabilir
Nemesis Elite: Run başında belirlenir, Act 1/2/3'te garantili çıkar
  Act 1 karşılaşma: standart elite ödülü
  Act 2 karşılaşma: +Soul Dust 20 + 1 bonus skill kart
  Act 3 karşılaşma: +Soul Dust 40 + garantili upgrade + "Nemesis Çözüldü" rozeti
```

---

## ARAŞTIRMAMI İSTEDİĞİM KONULAR

Lütfen aşağıdaki her konu için derinlemesine araştırma yap. Reddit thread'lerini, forum tartışmalarını, oyun incelemelerini ve topluluk feedback'lerini dahil et. Oyunları bizzat analiz et, sadece genel izlenim değil somut mekanik detaylar ver.

---

### KONU 1 — DUAL-CLASS / MULTİ-CLASS SİSTEMLERİ

**Araştır:**
- Guild Wars 1: Primary/secondary class sistemi. 1329 skill'den 8 seç. Oyuncu topluluğu hangi combo'ları "broken" buldu, hangileri unutuldu kaldı? Reddit ve oyun forumlarında hangi dual-class meta'ları efsane oldu?
- Guild Wars 2: Elite Specialization sistemi nasıl dual-class hissini değiştirdi?
- Throne and Liberty: Silah bazlı dual-spec sistemi. Oyuncu feedback'i nasıl? Silah değiştirme anında mı yoksa planlı mı kullanılıyor?
- Black Desert Online: Awakening ve Succession sistemi. İki build arasındaki geçiş nasıl? Oyuncular hangisini tercih ediyor ve neden?
- Final Fantasy XIV: Job sistemi — Scholar/Summoner ayrımı, multi-job oynama. Hangi kombinasyonlar popüler?
- Path of Exile: Ascendancy sınıfları. Hangi Ascendancy combinations "S-tier" build'ler üretiyor? Reddit r/pathofexile'da hangi build kılavuzları en çok upvote aldı?
- Dungeons & Dragons (tabletop): Multiclass sistemi. Optimizer toplulukları hangi 2-3 class kombinasyonlarını kırık buluyor?

**Benim oyunuma nasıl uyarlanır:**
- Hangi "dual-class fantazisi" oyuncularda gerçekten "bu build insane" hissi yaratıyor?
- En tatmin edici dual-class combo tipleri neler? (tank+damage, CC+burst, DoT+amp, vb.)
- Oyuncular dual-class'ta genellikle ne zaman sinerjili hissediyor — erken mi, geç mi?

---

### KONU 2 — ROGUELITE SKILL ACQUISITION SİSTEMLERİ

**Araştır:**
- **Slay the Spire:** 3 kart teklifi sistemi. Oyuncular hangi anda "build tıklandı" hissediyor? r/slaythespire'da en çok tartışılan "build-defining picks" neler? Card synergy "enabler + payoff" mantığı nasıl çalışıyor?
- **Hades:** Boon sistemi. Duo Boon discovery mekaniği oyuncularda nasıl bir his yaratıyor? Hangi Boon kombinasyonları toplulukta "legendary run" olarak anılıyor?
- **Dead Cells:** Blueprint sistemi ve skill scroll'lar. Oyuncular build'lerini ne zaman hissediyorlar?
- **Monster Train:** "Train" formatında çok katlı build inşası. Sinerji sistemi nasıl çalışıyor?
- **Balatro:** Joker sinerji sistemi — tamamen farklı tür ama "build click" anı çok güçlü. Ne yapıyor?
- **Dicey Dungeons:** Sınıf bazlı farklı acquisition sistemi. Her sınıf neden farklı hissettiriyor?

**Benim oyunuma nasıl uyarlanır:**
- "Weighted Draft" sistemi doğru mu? Erken Core / geç Master ağırlığı ideal mi?
- "Signature skill" (ilk oda garantili) mekaniği iyi bir karar mı?
- "Slot 5-6 gating" (Boss Soul gerekli) oyuncu motivasyonu için doğru mu?
- Tag sistemi (OPENER → CHAIN → FINISHER zinciri) Slay the Spire'ın "enabler+payoff" mantığıyla örtüşüyor mu?

---

### KONU 3 — KAYNAK SİSTEMLERİ VE SINIF KİMLİĞİ

**Araştır:**
- **World of Warcraft:** Rage, Mana, Energy, Combo Points, Holy Power, Runes, Fury sistemleri. Hangi kaynak sistemi oyuncular tarafından "en tatmin edici" bulundu ve neden? r/classicwow ve r/wow'da hangi sınıflar "rotation hissi" için övülüyor?
- **Lost Ark:** Identity Gauge sistemi (Bard, Sorceress, Berserker, Deathblade). Hangi sınıfın identity sistemi oyuncuları en çok tatmin etti?
- **FFXIV:** Black Mage'in "rotation ritmi" — Astral Fire/Umbral Ice döngüsü. Yeni başlayanlar ile uzmanlar arasındaki fark neden bu kadar büyük?
- **Path of Exile:** Flask sistemi ve unique build mechanics (Cyclone spin, Blade Flurry kanal, Toxic Rain cloud). Hangileri "satisfying loop" yarattı?
- **Hollow Knight:** Soul sistemi — basit ama etkili. Neden çalışıyor?
- **Hades:** Darkness, Gold, Keys — roguelite'ta meta-currency multi-resource nasıl hissettiriyor?

**Benim oyunuma nasıl uyarlanır:**
- Ranger'ın "Focus" sistemi (uzakta dolar, yakında boşalır) benzeri mekanikler var mı? Başarılı örnekler?
- Brawler'ın "sadece hasar alarak dolar" sistemi — WoW Warrior Rage benzeri — oyuncuların bunu sevip sevmediği?
- Hexer'ın 4 faz sistemi (0-3 / 4-6 / 7-9 / 10) — Path of Exile veya FFXIV'de benzeri multi-threshold sistemler var mı? Nasıl hissettiriyor?
- Paladin'in Builder/Spender döngüsü — WoW Holy Power ile aynı. Bu sistemi oyuncular sevdi mi, neden?

---

### KONU 4 — ADAPTIF DÜŞMAN VE NEMESİS SİSTEMLERİ

**Araştır:**
- **Shadow of Mordor / Shadow of War:** Nemesis sistemi. Oyuncuların gerçekten "anlamlı" bulduğu anlar nelerdi? Toplulukta hangi "legendary nemesis hikayesi" anlatıldı? Neden Shadow of Mordor'un bu sistemi o kadar övüldü ama başka oyunlar kopyalamadı?
- **Hades:** Boss'ların adaptif diyalogları (kayıp sayısına göre değişen tepkiler). Oyuncular bunu ne kadar fark etti?
- **Into the Breach:** Düşman niyetleri görünür. Bu "hafıza" değil ama oyuncu davranışını nasıl şekillendiriyor?
- **FTL:** Pilot ve mekanik beceri geliştikçe değişen deneyim — adaptasyon oyuncu yönünden.
- **Hades 2:** Yeni mekanikler düşman adaptasyonu için ne getirdi?
- **MMO Boss Hafızası:** WoW, FFXIV, BDO boss'larında "boss senin build'ine göre adaptasyon" var mı? Yoksa neden yok?

**Benim oyunuma nasıl uyarlanır:**
- Grudge sistemi (ölme şeklini hatırlıyor, o yönteme direnç) — bu tip "soft memory" sistemi oyuncularda nasıl bir davranış değişikliği yaratır?
- "Kasıtlı programlama" (Nemesis'i buz ile öldürüyorum ki Act 3'te buz direnci kazansın, ben zaten ateş build yapıyorum) — bu tür meta-thinking oyuncuları tatmin eder mi?
- Nemesis Elite 3 act boyunca garantili çıkar mı yoksa sadece şans mı? Araştır.

---

### KONU 5 — BUILD CRAFTING VE "BUILD CLICK" ANI

**Araştır:**
- **Path of Exile:** "Build enabling item" sistemi. Belirli bir unique item olmadan build çalışmıyor. Oyuncular bu "build click" anını nasıl tarif ediyor? r/pathofexile en çok upvote alan "my build finally came together" postları.
- **Slay the Spire:** İlk kez "Silent infinite loop" veya "Defect orb explosion" kurulduğunda oyuncular ne hissediyor? Topluluk bu anları nasıl tanımlıyor?
- **Monster Train:** "Seraph + Monstrous" gibi combo'lar oyuncularda ne yaratıyor?
- **Vampire Survivors:** "Bu build çalışıyor" anı ne zaman geliyor ve neden bu kadar tatmin edici?
- **MMORPG theory crafting toplulukları:** WoW, FFXIV, BDO için r/wow r/ffxiv r/BlackDesertOnline'da "build guide" kültürü nasıl çalışıyor? Hangi guide'lar en çok etkileşim aldı?
- **Diablo 3 / 4:** Set bonus sistemi. Oyuncular "set tamamlandı" anını nasıl tanımlıyor?

**Benim oyunuma nasıl uyarlanır:**
- Tag sistemi (OPENER→CHAIN→FINISHER) "build click" anını ne zaman ve nasıl üretebilir?
- Dual-class cross-class pasif otomatik aktif oluyor — bu "set bonus" hissine benziyor mu?
- 28 kombinasyondan kaçının "build-defining" hissi vermesi gerekiyor?

---

### KONU 6 — SLOT SİSTEMİ VE CHOICE PARALYSIS (SEÇİM FELCI)

**Araştır:**
- **Hades:** 6 tanrı, her birinden birden fazla boon — oyuncu nasıl karar veriyor? "Fırsat maliyeti" hissi nasıl yaratılıyor?
- **Slay the Spire:** "Kart almamak" bazen daha mı iyi? "Skip" kültürü toplulukta nasıl gelişti?
- **Across the Obelisk:** 4 kişilik takım, her birinin kartları — seçim yükü nasıl dengeleniyor?
- **Akademik çalışmalar:** "Choice overload" oyunlarda nasıl tezahür ediyor? Barry Schwartz'ın "The Paradox of Choice" kavramı oyun tasarımında nasıl uygulanıyor?
- **Guild Wars 1:** 1329 skill var ama sadece 8 seçiyorsun. Bu "okyanus içinde özgürlük" hissi oyunculara nasıl geliyor?

**Benim oyunuma nasıl uyarlanır:**
- "12 skill havuzu ama hiçbir zaman hepsini görmüyorsun" sistemi seçim felcini önlüyor mu?
- Slot 5-6 gating doğru bir sınırlama mı? Oyuncular "6 slot" hedefinden önce mi "bu build tamam" hissediyor?
- Tag sistemi (OPENER/CHAIN/FINISHER) seçim yönlendirmesinde yeterli mi?

---

### KONU 7 — ROGUELITE TEMPO VE GÜÇLENME EĞRİSİ

**Araştır:**
- **Hades:** Güç eğrisi — ilk run ile 50. run arasındaki fark. Oyuncular ne zaman "artık güçlüyüm" hissediyor?
- **Dead Cells:** Scroll sistemi ve biome progression. Yeni oyuncular ile veteranlar arasındaki fark.
- **Risk of Rain 2:** Zaman bazlı item scaling. "Snowball" etkisi nasıl kontrol altında tutuluyor?
- **Binding of Isaac:** "Item synergy ceiling" — mümkün olan en güçlü build ne kadar sürüyor?
- **WoW Classic vs Retail:** Güçlenme eğrisi farkı. Classic'te oyuncular ne zaman "peak" hissettiklerini anlatıyor?
- **Lost Ark:** Legion Raid gear progression — "gear check" sistemi neden tartışmalı?

**Benim oyunuma nasıl uyarlanır:**
- "Oda 1-3 (Core) → Oda 4-7 (Advanced) → Oda 8+ (Master)" eğrisi doğru mu?
- Boss Soul mutasyon sistemi (Act 1 Enhance → Act 2 Corrupt → Act 3 Ascend) tempo açısından çalışıyor mu?
- Dual-class güçlenme: hangi oda'da "bu build gerçekten tıklandı" hissi gelmeli?

---

### KONU 8 — SINIF ÇEŞİTLİLİĞİ VE REPLAYABILITY

**Araştır:**
- **Hades:** 6 silah, her birinin 4 "Aspect" varyantı. Oyuncular tüm silahları ne zaman deniyor? "Main weapon" sendromu oluşuyor mu?
- **Dead Cells:** 5 sınıf benzeri build yolları (brutality/tactics/survival). Oyuncular hangi yolu tercih ediyor ve neden?
- **Slay the Spire:** 5 karakter. Ironclad, Silent, Defect, Watcher — hangisi en az oynanıyor ve neden?
- **WoW Classic:** 9 sınıf. Toplulukta hangi sınıflar "tier S" sayıldı, hangileri "tier C"? Bu nasıl değişti?
- **Path of Exile:** Her League'de hangi sınıf en popüler? r/pathofexile "starter build" tartışmaları.
- **Darkest Dungeon:** 17 sınıf. Oyuncular tüm sınıfları deniyor mu? Hangileri "unutulmuş" sayılıyor?

**Benim oyunuma nasıl uyarlanır:**
- 8 sınıfın her biri eşit replayability sağlamalı mı, yoksa "main class" kültürü kabul edilebilir mi?
- Hexer ve Summoner gibi "karmaşık" sınıflar yeni oyuncular için ne zaman sunulmalı?
- Hangi 2 sınıfın "ilk run için" ideal olduğu belirlenmeli mi?

---

### KONU 9 — GÖRSEL FEEDBACK VE JUICE

**Araştır:**
- **Hades:** Hit feedback, blood VFX, screen shake — oyuncular bu konuda ne söylüyor?
- **Enter the Gungeon:** "Game feel" topluluk tartışmaları — neden bu kadar satisfying hissettiriyor?
- **Dead Cells:** Parry sistemi ve "juice" — oyuncu geri bildirimleri.
- **Slay the Spire:** Minimal görsel ama güçlü ses feedback. Nasıl çalışıyor?
- **Vampire Survivors:** Minimal sprite ama büyük güç hissi. Pixel art'ta "juice" nasıl yapılıyor?
- **FFXIV:** Skill animation "weight" hissi — özellikle physical DPS sınıflarında.

**Benim oyunuma nasıl uyarlanır:**
- Tag sistemi (CHAIN tetiklenince bildirim) görsel olarak nasıl tasarlanmalı?
- Hex Phase geçişleri (0-3 / 4-6 / 7-9 / 10) nasıl görselleştirilmeli?
- "Bu build insane" anında ekran ne yapmalı?

---

### KONU 10 — META PROGRESSION VE UZUN VADELİ MOTİVASYON

**Araştır:**
- **Hades:** Darkness sistemi ve Mirror of Night — oyuncular kaç run sonra tam açıyor? Bu süre keyifli mi?
- **Dead Cells:** Blueprint sistemi — oyuncular hangi blueprint'leri önce açmak istiyor?
- **Rogue Legacy 2:** Heir systemi — kalıcı şeyler bırakmak motivasyonu nasıl etkiliyor?
- **Binding of Isaac:** Item unlock sistemi — "achievement unlocks new item" döngüsü uzun vadede nasıl çalışıyor?
- **WoW Season system:** Her sezon yeni meta, yeni gear tier — kalıcı vs geçici progression dengesi.
- **Path of Exile League systemi:** Her 3 ayda yeni mechanic, tüm karakter sıfırlanıyor. Oyuncular bunu seviyor mu?

**Benim oyunuma nasıl uyarlanır:**
- Meta progression ağacında en çok motive edici unlock hangisi olmalı?
- "Soul Dust harca → yeni sınıf aç" sistemi yeterli motivasyon mu?
- Contract sistemi (run başı isteğe bağlı şart) replayability'yi artırıyor mu?

---

## ARAŞTIRMA FORMATI

Her konu için şu formatta yanıt ver:

### [Konu Adı]

**Araştırma Bulguları:**
- [Oyun adı]: [Somut mekanik bilgi] — [Kaynak: Reddit post / forum / community wiki / inceleme]
- ...

**Topluluk Söylemi (ne sevdi, ne sevmedi):**
- ...

**Benim Oyunuma Uyarlama Önerileri:**
1. [Somut öneri — hangi sistemi nasıl değiştirmemi öneriyor]
2. ...

**Risk Analizi:**
- [Bu önerinin uygulanmaması durumunda oluşabilecek sorunlar]

---

## EK: DİKKAT EDİLECEK OYUNLAR LİSTESİ

Bu oyunları araştırırken mutlaka incele:

**MMORPG'ler:**
- World of Warcraft (Vanilla / Classic / Dragonflight)
- Final Fantasy XIV (Endwalker / Dawntrail)
- Guild Wars 1 ve 2
- Black Desert Online
- Lost Ark
- Throne and Liberty
- Lineage 2 (klasik)
- Ragnarok Online (klasik)
- Runescape (OSRS ve RS3)
- Blade & Soul

**Roguelite / Roguelike:**
- Slay the Spire
- Hades ve Hades 2
- Dead Cells
- Binding of Isaac: Repentance
- Risk of Rain 2
- Monster Train
- Across the Obelisk
- Balatro
- Vampire Survivors
- Rogue Legacy 2
- Darkest Dungeon 1 ve 2
- Into the Breach
- FTL: Faster Than Light
- Dicey Dungeons

**Action RPG:**
- Path of Exile 1 ve 2
- Diablo 2, 3 ve 4
- Torchlight 2 ve 3
- Wolcen: Lords of Mayhem
- Grim Dawn
- Magicka serisi

**Single Player Action:**
- Hollow Knight
- Elden Ring / Dark Souls
- Control
- Hades (zaten yukarıda)
- Transistor (Supergiant)
- Pyre (Supergiant)

**Platform Benzerleri (genre crossing):**
- Children of Morta (pixel art + roguelite + skill tree)
- Enter the Gungeon (top-down bullet hell)
- The Binding of Isaac (zaten yukarıda)
- Noita (simülasyon + roguelite)

---

## ÖNEMLİ NOT

Bu araştırma için Reddit (r/slaythespire, r/pathofexile, r/hades, r/roguelikes, r/roguelites, r/wow, r/ffxiv, r/BlackDesertOnline, r/Eldenring), Steam inceleme bölümleri, oyun wiki'leri ve akademik oyun tasarımı makalelerini kullan. Genel izlenim değil, **somut topluluk geri bildirimi ve mekanik analiz** istiyorum.

Araştırmanı Türkçe yaz.

---

## BÖLÜM 2 — MEVCUT SKİLL TASARIMLARIMI DEĞERLENDİR

Aşağıda oyunumun 8 sınıfının tasarladığım 12'şer aktif skill'ini okuyacaksın. Her sınıf için şunları değerlendir:

1. **Hangi skill'ler referans oyunlardan doğrudan kopyalanmış veya çok benzeri var?** (İsim veya mekanik bazında)
2. **Hangi skill'ler özgün veya ilginç bir twist içeriyor?**
3. **Hangi skill'ler "flat" veya sıkıcı — "bu WoW'da var zaten, neden seçeyim" hissi yaratan?**
4. **Her sınıf için en güçlü 3 skill kombinasyonu (4'ten 3 slot) nedir ve neden?**
5. **Tag dağılımı dengeli mi? Çok fazla FINISHER, çok az CONTROL gibi sorunlar var mı?**
6. **Core/Advanced/Master tier atamaları mantıklı mı?**
7. **Somut iyileştirme önerileri:** Değiştirmemi, birleştirmemi veya kaldırmamı önerdiğin skill'ler.

Yanıt formatı şu olsun:

```
### [Sınıf Adı] DEĞERLENDİRME

**Güçlü skill'ler (koru bunları):**
- [Skill adı]: [Neden güçlü/özgün]

**Zayıf / değiştir:**
- [Skill adı]: [Sorun] → [Öneri]

**Referans oyunlarla örtüşme:**
- [Skill adı]: [Hangi oyunun hangi skill'ine benziyor]

**Tag analizi:**
- [Sorun varsa]

**Önerilen top 3 combo (4 slottan):**
1. [Skill A + B + C + D] — [Neden]

**Genel not:**
- [Sınıfın kimliği ne kadar net, core fantasy'ye uyuyor mu]
```

---

### ⚔️ WARBLADE — 12 Skill Listesi

**Core Fantasy:** "Duruyorum, geçemiyorsun. Ben gittikçe tehlikeliyim."
**Kaynak:** Rage (0-100) — hasar verince +5/vuruş, alınca +10, boşta -5/sn
**[V] Burst:** BLADESTORM — Rage %100: 5s spin, CC immune, her 0.5s AoE

| # | İsim | Tag | Tür | Efekt | Chain Koşulu → Chain Bonus |
|---|------|-----|-----|-------|--------------------------|
| 1 | Charge | ▶⬡ | Core | 8m dash+stun 1.5s, Rage+20 | Stun'daki hedefe ilk saldırı → +%80 hasar |
| 2 | Mortal Strike | ⚡💥 | Core | Büyük hasar + iyileşme -%50 (6s) | Charge sonrası → iyileşme -%100 |
| 3 | Colossus Smash | ✦ | Core | 6s window: tüm hasar kaynakları +%30 | Dual-class burst window ile → amplify katlanır |
| 4 | Whirlwind | ⚓↓ | Core | 2s spin AoE, hareket edilebilir, Rage -25/sn | Rage biterken → Cleave açılır |
| 5 | Shield Slam | ⬡ | Core | Knockback 3m garantili, duvara çarparsa +1.5s stun, Rage -20 | War Stomp sonrası → CD yarıya |
| 6 | Execute | 💥 | Core | SADECE HP<%30: %400 hasar, Rage boşaltır | Mortal Strike aktifken → %600 hasar |
| 7 | Hamstring | ⬡▶ | Core | %50 slow 8s + bleed 3s DoT | Hamstring'li hedefe Charge → stun 3s |
| 8 | War Stomp | ⬡↑ | Core | 3m knockup 2s, Rage+25 | Whirlwind sırasında → +1s uzar |
| 9 | Heroic Leap | ▶⬡ | Advanced | 12m atla, iniş AoE 3m, Rage+15 | İnişte 2+ düşman → Rage+30 |
| 10 | Rallying Cry | ✦↑ | Advanced | 8s: her Rage harcaması = HP +%5 iyileşme | Rage %80+'ta aktive → süre 12s |
| 11 | Rupture Strike | ⚡↑ | Advanced | Bleed DoT 8s + Rage+20 | Colossus Smash window'unda → bleed tick 2× hızlı |
| 12 | Last Man Standing | ⚓↑ | Master | 4s: HP 1 altına düşemez, sonra Rage tamamen dolar | Execute ile birlikte → ölümsüzlük içinde %600 |

**Build eksenleri:** "Executioner" (Charge+MS+Smash+Execute) / "Bleed Lord" (Hamstring+Rupture Strike+Whirlwind+Rallying Cry) / "Last Stand" (LMS+War Stomp+Shield Slam+Heroic Leap)

---

### 🔥 ELEMENTALİST — 12 Skill Listesi

**Core Fantasy:** "Her şeyi yakıyorum. Ama önce ritmi buluyorum."
**Kaynak:** Mana (0-100, +8/sn regen) + Elemental State (Fire veya Frost)
**[V] Burst:** INFERNO — Mana %100: 7s arena-wide ateş yağmuru

| # | İsim | Tag | Tür | Efekt | Chain Koşulu → Chain Bonus |
|---|------|-----|-----|-------|--------------------------|
| 1 | Fireball | ↑▶ | Core | Orta hasar + ateş DoT 4s, Fire State +1 (max 5) | 3 ard arda → 3.'de Living Bomb ücretsiz |
| 2 | Frostbolt | ⬡↑ | Core | Orta hasar + %30 slow 3s, Fire State tüketir | Fireball DoT aktifken → Shatter (+%60 hasar amplifikasyonu) |
| 3 | Living Bomb | ⚡↓ | Core | 5s sonra patlama, öldürünce 3 komşuya kopyalanır | Frostbolt slow altında → patlama yarıçapı 2× |
| 4 | Blink | ▶⚓ | Core | 6m ışınlanma, geçilen düşmanlara hasar, sonraki spell +%20 | Düşmanın içinden geçilirse → 0.5s stun |
| 5 | Frozen Orb | ⬡⚓ | Core | Yavaş hareket eden küre, yolundakileri 5s chill | Orb üzerinden Blink → Orb patlar, chilled=Frozen 2s |
| 6 | Arcane Blast | ↑↓ | Core | Her cast +%20 hasar ama +%30 mana maliyet, 4. cast Barrage açar | Colossus Smash (Warblade dual) window'da Barrage → cap kaldırılır |
| 7 | Meteor | 💥⬡ | Core | 1.5s kanal → büyük AoE knockdown | Frozen/slowed hedef → knockdown 3s + hasar +%50 |
| 8 | Mirror Image | ⚓ | Core | 2 kopya 8s, her kopya random skill atar, hasar önce kopyaya gelir | Kopyalar ölünce → ölüm patlaması AoE |
| 9 | Chain Lightning | ⚓✦ | Advanced | 5 hedefe sekiyor, her seki ayrı hasar, Storm element açar | Hedef yavaşlamışsa → 7 seki |
| 10 | Mana Shield | ↑⚓ | Advanced | 6s: hasar HP yerine Mana'ya gelir | Mana %100'ken aktive → +2s süre |
| 11 | Combustion | ✦▶ | Advanced | 8s: tüm Fire spell'ler instant cast, mana maliyet ×2 | Fire State 5 stack'ta aktive → mana maliyet artışı yok |
| 12 | Blizzard | ⬡↑ | Master | 3s kanal → 5m alana 8s devam eden slow+tick hasar | Meteor'dan önce → Meteor knockdown 4s'e çıkar |

**Build eksenleri:** "Fire Burst" (Combustion+Fireball+Living Bomb+Meteor) / "Frost Control" (Frostbolt+Blizzard+Frozen Orb+Meteor) / "Arcane Storm" (Chain Lightning+Arcane Blast+Mana Shield+Blink)

---

### 🗡️ ROGUE — 12 Skill Listesi

**Core Fantasy:** "Görmüyorsun. Zaten geç."
**Kaynak:** Energy (0-100, +15/sn) + Combo Points (0-5)
**[V] Burst:** SHADOW DANCE — Energy %100+CP 5: 8s her saldırı sonrası otomatik stealth

| # | İsim | Tag | Tür | Efekt | Chain Koşulu → Chain Bonus |
|---|------|-----|-----|-------|--------------------------|
| 1 | Backstab | ▶↑ | Core | Arkadan: %200 hasar+3CP. Önden: normal — CP yok | Shadowstep sonrası → +%50 hasar |
| 2 | Hemorrhage | ↑ | Core | Bleed 8s DoT+2CP, öldürünce yakına yayılır | Bleed aktif hedefe Rupture → hasar +%100 |
| 3 | Rupture | 💥↓ | Core | 5CP finisher: bleed+hasar, CP sayısına göre süre uzar (max 12s) | Zaten bleed varsa → birikmiş hasar anında patlar |
| 4 | Shadowstep | ▶⬡ | Core | Hedefe anında ışınlan 8m, 0.5s stun, Energy-25 | Evasion aktifken → CD sıfırlanır |
| 5 | Kidney Shot | ⬡↓ | Core | 5CP: 4s stun, CP sayısına göre uzar | Mortal Strike (Warblade dual) aktifken → stun'da iyileşme yok |
| 6 | Ambush | 💥▶ | Core | Sadece stealth'ten: %300 hasar+4CP+%20 slow | 3s+ stealth → "Cold Blood" +%100 ekstra hasar |
| 7 | Fan of Knives | ⚓⬡ | Core | 360° AoE, tüm aktif bleed/zehir/debuffları tüm düşmanlara uygular, Energy-40 | Hexer dual → Hexer debuffları da yayılır |
| 8 | Evasion | ⚓↑ | Core | 4s %100 dodge, her dodge=+1CP, kill=CD sıfırlanır | Evasion bitince → sonraki saldırı guaranteed crit |
| 9 | Deadly Poison | ↑▶ | Advanced | 10s silah zehiri: her saldırı ayrı zehir DoT uygular | Hexer dual → her zehir DoT 1 Hex stack uygular |
| 10 | Sprint | ▶⚓ | Advanced | 3s hız +%100, geçtiğin düşmanlara hasar | Sprint sırasında Backstab → arka pozisyon garantilenir |
| 11 | Preparation | ↑✦ | Advanced | Tüm Rogue skill CD'lerini sıfırla, 90s CD | Evasion aktifken → Preparation kendi CD'si 60s'ye iner |
| 12 | Vanish | ⚓⬡ | Master | Savaşta anlık stealth, tüm düşmanlar hedef kaybeder, 3s, 50s CD | Vanish sonrası Ambush → "Cold Blood" garantili |

**Build eksenleri:** "Assassin" (Ambush+Vanish+Backstab+Preparation) / "Bleeder" (Hemorrhage+Deadly Poison+Rupture+Fan of Knives) / "Duelist" (Evasion+Sprint+Shadowstep+Kidney Shot)

---

### 🏹 RANGER — 12 Skill Listesi

**Core Fantasy:** "Sana ulaşamazsın. Her saniye kayıp veriyorsun."
**Kaynak:** Focus (0-100) — 4m+ uzakta +10/sn dolar, 2m altında -20/sn boşalır
**Focus 75+:** +%25 hasar | **Focus 100:** sonraki skill free cast
**[V] Burst:** RAIN OF ARROWS — 30s sabit CD: 5s tüm arena yağmur

| # | İsim | Tag | Tür | Efekt | Chain Koşulu → Chain Bonus |
|---|------|-----|-----|-------|--------------------------|
| 1 | Aimed Shot | 💥⚡ | Core | 1.5s şarj → büyük tek hasar+%50 crit. Hedef immobile=anında | Concussive Arrow root'u sonrası → guaranteed instant |
| 2 | Concussive Arrow | ⬡▶ | Core | Knockback 4m + root 2s | Backward Dash sırasında → uzaklık 6m + slow 3s |
| 3 | Serpent Sting | ↑▶ | Core | Zehir DoT 10s + armor debuff -%20, max 30s devam | Disengage+Serpent Sting → daha geniş alana zehir |
| 4 | Explosive Trap | ⬡↑ | Core | Zemine tuzak, 3s sonra patlama+slow 3s | Summoner dual → trap minyona konulabilir (mobil) |
| 5 | Multi-Shot | ⚓↑ | Core | Delici ok: tüm düşmanlardan geçer, her birine Serpent Sting stack | 5+ düşman vurulursa → tüm CD -3s |
| 6 | Disengage | ▶⬡ | Core | 6m geri atla, slow alanı bırak, havada hasar -%30 | Disengage+anında Aimed Shot → atlama sırasında ateş, ekstra hasar |
| 7 | Black Arrow | ↑⚡ | Core | DoT + özel: bu DoT ile ölen düşman 8s ruh bırakır | Summoner dual → ruh minyon sayılır (Blood for Power) |
| 8 | Volley | ⬡⚓ | Core | 4m alana 3s yağmur, slow+tick, hareket edebilirsin | Explosive Trap üzerine → tam kilitlenme combo |
| 9 | Rapid Fire | ⚓↓ | Advanced | 2s kanal: 8 hızlı ok, toplam>Aimed Shot, Focus -30 | Focus 100'de başlarsa → 10 ok, Focus maliyeti yok |
| 10 | Spirit Wolf | ↑⚓ | Advanced | 12s hayalet kurt, bağımsız saldırır, Serpent Sting otomatik uygular | Summoner dual → minyon sayılır, Blood for Power |
| 11 | Flare | ⬡✦ | Advanced | 6m flare: stealth düşmanları açığa çıkarır + 6s slow alanı, Focus+20 | Rogue dual → Vanish/stealth iptal eder, CP kaybettirir |
| 12 | Point Blank | 💥⬡ | Master | ≤2m yakınlıkta: ×3 hasar + 5m knockback, Focus 100 gerektirir | Disengage sonrası anında → CD sıfırlanır |

**Build eksenleri:** "Sniper" (Aimed Shot+Concussive Arrow+Rapid Fire) / "Trap Master" (Explosive Trap+Volley+Flare+Serpent Sting) / "Hunter Pack" (Spirit Wolf+Black Arrow+Multi-Shot+Disengage)

---

### 👊 BRAWLER — 12 Skill Listesi

**Core Fantasy:** "Az canken daha tehlikeliyim. Bu hata değil, strateji."
**Kaynak:** Fury (0-100) — SADECE hasar ALARAK dolar (+15/vuruş). HP düştükçe daha hızlı.
**[V] Burst:** BERSERK MODE — Fury %100: 15s defense ignore + %200 hasar + tüm CD sıfır

| # | İsim | Tag | Tür | Efekt | Chain Koşulu → Chain Bonus |
|---|------|-----|-----|-------|--------------------------|
| 1 | Bloodlust Strike | ⚡💥 | Core | Koni saldırı, HP'ye göre hasar artar (%100HP=baz, %30HP=+%120) | Fury %80+ → Slaughter anında açılır |
| 2 | Whirlwind | ⚓↓ | Core | 2s spin AoE, her düşman vuruşu savunma -%5 (max -%30) | Savunma -%30'da → Fury +20/spin |
| 3 | Frenzied Leap | ▶↑ | Core | Hedefe atla, iniş AoE, hit=CD anında sıfırlanır | 3 ard arda farklı hedefe → 5s Frenzy buff +%50 hasar |
| 4 | Reckless Swing | 💥↓ | Core | Devasa tek hasar, 2s tam savunmasız | Hasar alırsan savunmasızlıkta → Fury+40 + 0.8s invuln |
| 5 | Bloodthirst | ⚓↑ | Core | Hızlı 5 vuruş, her vuruş küçük iyileşme, HP düşükse daha fazla | HP<%20 + Fury%100 → 8 vuruşa yükselir |
| 6 | Intimidating Shout | ⬡ | Core | 3m çevresinde 3s panik/kaçar | Panikleyen düşmana Bloodlust Strike → +%100 hasar |
| 7 | Barbaric Charge | ▶⬡ | Core | Düz çizgide her şeyi iter, stun/root immune | Duvara çarparsa → itilen düşmanlar stun 2s |
| 8 | Last Rites | 💥 | Core | SADECE HP<%15: %600 hasar, sonra 4s savunmasız | Eğer öldürürse → savunmasızlık 2s'ye iner |
| 9 | Iron Grab | ⬡▶ | Advanced | Yakala (≤2m): 1.5s hold, seçtiğin yöne fırlat, Fury+30 | Fırlatılan düşman 3.'e çarparsa → her ikisi de stun |
| 10 | War Cry | ↑✦ | Advanced | 8s: Fury üretimi ×2 | Fury %50 altındayken aktive → süre 12s |
| 11 | Shatter Armor | ✦▶ | Advanced | Hedefin savunması -%40, 10s (tüm kaynaklar yararlanır) | Warblade dual Colossus Smash window'unda → savunma -%60 |
| 12 | Death Wish | ⚓↑ | Master | 5s: HP 1 altına düşemez, Fury ×3 hızlı dolar, sonra 8s +%80 hasar | Fury %100'e ulaşırsa içinde → [V] Burst anında tetiklenebilir |

**Build eksenleri:** "Glass Cannon" (Reckless Swing+Bloodlust Strike+Last Rites+Death Wish) / "Control Brawler" (Iron Grab+Barbaric Charge+Intimidating Shout+Frenzied Leap) / "Fury Engine" (War Cry+Bloodthirst+Whirlwind+Shatter Armor)

---

### ⚖️ PALADİN — 12 Skill Listesi

**Core Fantasy:** "Hem kesilemiyorum hem öldürüyorum. Bu çelişki değil, tasarım."
**Kaynak:** Holy Power (0-100, builder skill'lerle dolar, spender skill'lerle boşaltılır)
**[V] Burst:** AVENGING WRATH — 3 mükemmel Builder→Spender zinciri: 10s %30 invuln + %50 hasar

| # | İsim | Tag | Tür | Efekt | Chain Koşulu → Chain Bonus |
|---|------|-----|-----|-------|--------------------------|
| 1 | Crusader Strike | ↑▶ | Core | Temel melee + Holy Power+25 | Crusader→Judgment→Crusader zinciri → her 3.'de +%60 hasar |
| 2 | Divine Storm | ↑✦ | Core | 360° melee AoE + HP+15/hedef | 3+ düşman vurulursa → HP+50 |
| 3 | Judgment | ↑⚡ | Core | Ranged holy blast 6m, debuffluysa +%50 hasar | Hexer dual: Hexer debuff → +%100 hasar |
| 4 | Consecration | ↑⚓ | Core | 5s kutsal zemin, tick hasar + HP+5/sn | Warblade Battle Cry combo → düşmanlar koşarak zeminde patlıyor |
| 5 | Hammer of Wrath | 💥⚡ | Core | SADECE HP<%20 hedefe: büyük hasar + HP+30 | Execute (Warblade dual) ile arka arkaya → her ikisi de HP eşiği |
| 6 | Avenger's Shield | ⬡↑ | Core | Kalkan fırlat: 3 hedefe sekip silence + HP+15/düşman | 3 farklı hedefe sekerse → her biri 2s slow |
| 7 | Holy Shock | ⚓↑ | Core | Düşman=hasar+HP+15 / kendin=iyileşme | HP<%30'da kendin üzerine → iyileşme ×3 |
| 8 | Shield of Retribution | ⚡↓ | Core | 3s blok, engellenen hasar birikir → AoE olarak serbest | Consecration üzerindeyken → AoE ×2 |
| 9 | Blessed Weapon | ↑✦ | Advanced | 12s: tüm melee +15 HP/hit (normal 5-10 yerine) | HP %80+'ta başlatılırsa → süre 18s |
| 10 | Lay on Hands | ⚓↓ | Advanced | Anlık tam HP iyileşme, 90s CD | HP sıfırlandıktan sonraki 5s → HP üretim ×2 |
| 11 | Devotion Aura | ✦↑ | Advanced | 8s: sen+minyonlar -%30 hasar, HP+5/sn | Summoner dual 3+ minyon → süre 12s |
| 12 | Divine Sacrifice | ⚡↑ | Master | 5s: minyon hasarının %60'ını üstlen, HP ×3 üretirsin | Fallen Saint (Brawler dual) → üstlenilen hasar Fury doldurur |

**Build eksenleri:** "Tank Rhythm" (Crusader Strike+Consecration+Shield of Retribution+Blessed Weapon) / "Holy Burst" (Divine Storm+Judgment+Avenging Wrath+Lay on Hands) / "Army Support" (Devotion Aura+Divine Sacrifice+Consecration+Hammer of Wrath)

---

### 💀 SUMMONER — 12 Skill Listesi

**Core Fantasy:** "Ben savaşmıyorum. Feda ediyorum. Ve feda anı en güçlü andır."
**Kaynak:** Charges (0-4, auto +1/8s; minyon ölünce +1 anında; feda edince +charge)
**[V] Burst:** ARMY OF THE DEAD — tüm charge doluyken: 6s tüm minyonlar +%150 hasar ve ölümsüz

| # | İsim | Tag | Tür | Efekt | Chain Koşulu → Chain Bonus |
|---|------|-----|-----|-------|--------------------------|
| 1 | Raise Skeleton | ↑▶ | Core | 1 Charge → melee iskelet (max 3). 3 birden=Rally Cry +%40 | 3 iskelet varken → sonraki iskelet +%20 hasar katkısı |
| 2 | Summon Golem | ⚓⬡ | Core | 2 Charge → 1 tank Golem. HP<%20=kendini patlatır AoE | Golem'e Commanding Strike → duvara çarpma stun |
| 3 | Rally Cry | ✦ | Core | Tüm minyonlar +%20 hasar+hız 10s | Karışık minyon tipleri varsa → bonus +%40'a çıkar |
| 4 | Corpse Explosion | 💥⚡ | Core | Düşman veya minyon cesedini patlatır, AoE | 3+ cesetle → zincir patlama |
| 5 | Death Nova | ⚡⬡ | Core | 1 minyonu feda: 8s zehir bulutu bırakır | Hexer dual → zehir bulutu Hexer debufflarını yayar |
| 6 | Commanding Strike | ✦⬡ | Core | Seçili minyona emir ×4 hasar+invuln; minyon yoksa Summoner ×2 atar | Golem'e emir → hedefi duvara çarpar |
| 7 | Blood for Power | ↑↓ | Core | Minyon feda → Charge+1 + tüm CD -%30 | 3 minyon ard arda feda → Charge+3 + [V] Burst meter +30 |
| 8 | Bone Shield | ⚓↑ | Core | 3s: minyon kalkan olur, gelen hasar absorbe → Charge+1 | Golem kalkan olursa → absorbe ×2 |
| 9 | Raise Archer | ↑⚓ | Advanced | 1 Charge → uzak archer iskelet (max 2), Ranger dual=Serpent Sting otomatik | 2 archer+1 melee → Rally Cry +%50'ye çıkar |
| 10 | Sacrificial Pact | ↓⚓ | Advanced | Tüm minyonlar intihar: HP+%8/minyon + tüm Charge geri | Boss fight tüm feda → +%40 HP tek seferlik |
| 11 | Dark Pact | ↑ | Advanced | HP -%12 → Charge olmadan minyon çağır | Brawler dual → HP kaybı Fury doldurur (Dark Army combo) |
| 12 | Lich Form | ✦⚓ | Master | 10s: Summoner ghostal (melee immune), tüm minyonlar +%60 hasar | Lich Form sırasında minyon feda → feda hasarı ×3 |

**Build eksenleri:** "Sacrifice Engine" (Blood for Power+Death Nova+Sacrificial Pact+Rally Cry) / "Army Commander" (Raise Skeleton+Raise Archer+Commanding Strike+Rally Cry) / "Lich Burst" (Lich Form+Dark Pact+Corpse Explosion+Bone Shield)

---

### 🔮 HEXER — 12 Skill Listesi

**Core Fantasy:** "Sabır. 10'a gelince sen bitiyorsun."
**Kaynak:** Hex Stacks per enemy (0-10, 5s decay)
**Faz sistemi:** 0-3 Debuff / 4-6 Pressure (+%20 güç) / 7-9 Overload (+%30 hasar alır) / 10 HEXBLAST
**[V] Burst:** HEX CASCADE — bir hedefte 10 stack: tüm düşmanlara 3 stack kopyalanır

| # | İsim | Tag | Tür | Efekt | Chain Koşulu → Chain Bonus |
|---|------|-----|-----|-------|--------------------------|
| 1 | Corruption | ▶↑ | Core | Anında 3 stack + 4s orta DoT | Corruption→Agony ard arda → Agony başlangıç hızı ×2 |
| 2 | Agony | ↑ | Core | Süregelen DoT, 2 stack/tick, durdurulamaz | Hedef Pressure Phase'deyken → tick 3'e çıkar |
| 3 | Pandemic | ✦⚡ | Core | Bir hedefteki TÜM stack'ları yakın düşmanlara kopyalar | Hedef Overload Phase'de → kopyalanan stack +3 ekstra |
| 4 | Hexblast | 💥↓ | Core | 10 stack patlaması: %100/stack, kill=CD sıfır, yakına 2 stack yayılır | 3+ hedef Pressure Phase'deyken → zincirlenerek tüm odaya |
| 5 | Empathy | ⚡⬡ | Core | Lanet: düşmanın saldırılarının %30'u kendine döner | Hedef Overload Phase'de → dönen hasar %60'a çıkar |
| 6 | Haunt | ↑⬡ | Core | Hayalet bağla: takip eder+tick+3 stack, 10 stack=otomatik Hexblast | Fan of Knives (Rogue dual) → Haunt stack'ları tüm düşmanlara |
| 7 | Unstable Affliction | 💥⚡ | Core | Dispel/heal edilirse → anında patlama+stun | Hedef stun/CC altındayken → guaranteed full stack |
| 8 | Enervate | ⬡✦ | Core | Hız -%50, saldırı hızı -%40, 10s | 5+ stack hedefte → süre ×2 |
| 9 | Mass Hex | ▶⬡ | Advanced | Görüntüdeki TÜM düşmanlara aynı anda 2 stack, HP -%8 maliyet | Pressure Phase'deki düşmanlar varsa → 3 stack uygular |
| 10 | Silence Hex | ⬡↑ | Advanced | 5s özel yetenek kilidi (elite burst suppressed) + 3 stack | Boss fight 7+ stack varken → silence 8s |
| 11 | Cursed Mirror | ⚡↑ | Advanced | 8s: sana uygulanan her debuff → düşmana %50 güçle yansır + 2 stack/yansıma | Enervate sana uygulanırsa → düşman kendi slow'unu alır |
| 12 | Soul Bargain | 💥↓ | Master | HP -%25 feda → hedefe anında 5 stack | Hedef zaten Overload (7-9) → anında 10'a tamamlar, Hexblast |

**Build eksenleri:** "Patient DoT" (Corruption+Agony+Pandemic+Hexblast) / "Mass Hexer" (Mass Hex+Enervate+Cursed Mirror+Silence Hex) / "Soul Burst" (Soul Bargain+Haunt+Unstable Affliction+Hexblast)

---

## DEĞERLENDİRME İÇİN EK SORULAR

Skill listelerini okuduktan sonra şu genel sorulara da yanıt ver:

**A. Sınıflar arası denge:**
- Hangi sınıf en özgün core fantasy'ye sahip?
- Hangi sınıfın skill seti "bu sınıfı neden oynayayım?" sorusunu en zayıf cevaplıyor?
- Warblade ile Brawler çok benzer mi? (Her ikisi melee, her ikisinde knockback/CC var, Whirlwind her ikisinde de var)

**B. Dual-class sinerjileri:**
- Skill listelerine bakarak hangi dual-class kombinasyonunun EN İYİ sinerji üreteceğini tahmin et
- Hangi dual-class kombinasyonu skill bazında EN AZ sinerji üretiyor?
- Summoner + Ranger (Spirit Wolf/Black Arrow/Raise Archer çakışması) çalışıyor mu?

**C. Tag dengesi analizi:**
- 96 skill'de FINISHER sayısı fazla mı az mı?
- BUILDER/SPENDER dengesi var mı?
- Sınıf başına CONTROL skill sayısı tutarlı mı?

**D. Sektörel karşılaştırma:**
- WoW'daki hangi sınıflarla en çok örtüşüyor? (Warblade=Arms Warrior, Rogue=Subtlety Rogue, vb.)
- Bu örtüşmeler "tanıdık his" açısından avantaj mı, yoksa "kopyalanmış" hissi mi yaratır?
- Hexer faz sistemi (0-3/4-6/7-9/10) FFXIV Black Mage'in Astral Fire/Umbral Ice sistemine ne kadar benziyor?

**E. Solo dev kapsamı:**
- 96 aktif skill'i dengeli ve tatmin edici hissettirmek için hangi minimum test sürecini önerirsin?
- Hangisi önce implement edilmeli, hangisi FAZ 4'e bırakılabilir?
