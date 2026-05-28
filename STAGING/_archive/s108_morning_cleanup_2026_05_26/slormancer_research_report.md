---
status: DRAFT_v1
faz: research
tarih: 2026-05-18
ozet: "The Slormancer multi-agent research → RIMA adapte tier rapor"
agents: Opus 4.7 (primary), Codex GPT-5.5 (pending), Gemini (failed)
sources: Steam page + Steam community + Slormite presskit + TheGamer + ExpertGameReviews + JoeReviews + PCGamesN + Slormancer Fandom Wiki + Slormbuilds.github.io
---

# The Slormancer — RIMA Karşılaştırma Raporu

## TL;DR (3 satır)
The Slormancer **pure ARPG** (persistent karakter, NOT roguelite), **GameMaker: Studio** engine, **3 sınıf × 3 specialization**, **Slorm Reaper weapon-defined build sistemi**, **isometric+top-down twin-stick** kontrol. RIMA'nın 2D top-down chibi roguelite mimarisi ile **kamera/genre/sprite mantığı farklı** ama **Slorm Reaper + Ancestral Skill Tree + Specialization** mekaniği RIMA'ya adapte değeri var.

**Önemli not:** RIMA roguelite (run-based), Slormancer ARPG (persistent). Birebir kopya YOK — sadece mekanik prensipler adapte edilebilir.

---

## 1. Slormancer Profili (Doğrulanmış Fact'ler)

| Boyut | Slormancer | RIMA | Uyum? |
|---|---|---|---|
| **Genre** | 2D ARPG Dungeon Crawler | 2D Top-Down Roguelite | ✗ farklı tür |
| **Engine** | GameMaker: Studio | Unity URP 2D | ✗ stack transfer YOK |
| **Karakter persistance** | Persistent (full ARPG) | Run-based death-reset | ✗ |
| **Kamera** | Isometric + Top-Down (Steam tags both) | High top-down 30-35° | ◐ kısmen benzer |
| **Aim sistemi** | Twin-stick controller | Mouse-cursor aim | ✗ |
| **Sprite stil** | Pixel art, detailed (resolution belirsiz, ~32-48px tahmini) | 64×64 chibi pixel art | ◐ ortak DNA |
| **Sınıf sayısı** | 3 base × 3 specialization = 9 effective | 10 base, no specs | RIMA daha fazla |
| **Skill/class** | 200+ abilities + upgrades + passives | 5+5+5 (LMB/RMB/F+4equip+Ghost) draft | RIMA daha skinny |
| **Element sistemi** | 150+ Ancestral skills 5 element (Fire/Ice/Thunder/Light/Shadow) | Elementalist 1 sınıfla sınırlı | RIMA daha local |
| **Build defining item** | Slorm Reapers (32→51→102 evolution chain) | Legendary items + Echo Imprint (4/run) | Karşılaştırılabilir mekanik |
| **Loot** | 120 unique weapons + 200 legendary affixes + infinite power scaling | Component/Combined/Legendary, run-based | ✗ scale farkı |
| **Endgame** | Forge (custom) + Rifts (escalation) + Arena (waves) + Library + Netherworld | Rift Break tier unlock (Phase 4-5) | Mekanik benzer ama meta seviye |
| **Procgen** | 7 environments × 40 affixes (enemies) | 7 oda tipi × MOB_COMPOSITION_RULES | ◐ |
| **Combat feel** | "Snappy, responsive, fast-paced", screen shake toggle | Hitstop+shake+dash+iframe (game feel bible) | ◑ benzer hedef |
| **Co-op** | YOK (en büyük review eleştirisi) | YOK (planlanmıyor) | uyumlu |

**Kaynaklar:** Steam page, Slormite Studios presskit, TheGamer class guide, ExpertGameReviews, PCGamesN, Slormancer Fandom wiki.

---

## 2. Slormancer'ın Üç Imzalı Mekaniği (Derinlemesine)

### 2A. Slorm Reapers — Weapon-Driven Build System
- **32 base + 19 normal evolutions + 51 Primordial evolutions = 102 toplam**
- Her Reaper'ın **build-defining unique effect**'i var (örn. "lightning staff that completely changed how my abilities functioned" — ExpertGameReviews)
- **Tüm sınıflar tüm Reaper'ları kullanabilir**, sadece weapon type visual değişir (stats sabit)
- Slormbuilds.github.io topluluğu Reaper-driven build paylaşıyor (Manareaper, Bloodthirst, etc.)
- **Önemli:** Build'in **temel taşı** — skill tree + Specialization + Reaper üçlüsünden Reaper en yüksek payı alıyor

**RIMA Adaptasyon değeri:** ⭐⭐⭐⭐⭐ (Tier S)
- RIMA'nın Echo Imprint sistemi (4/run) zaten benzer rolü oynuyor ama daha skinny
- Boss drop "Legendary Weapon" → 1 build-defining modifier şeklinde adapte edilebilir
- Karar #144 weaponless body + WeaponSR child SR sistemiyle **direkt uyumlu** (10 sınıf × 5 weapon × 3 component slot Karar #146 candidate ile birleşince Slorm Reaper benzeri)

### 2B. Ancestral Skill Tree — Element Cross-Class
- 150+ skill, 5 element (Fire/Ice/Thunder/Light/Shadow)
- Her sınıf SAME tree'ye erişir — element seçimi sınıf-agnostik
- Passive **one-per-line** kısıtı strategic build choice
- "Path of Exile's younger, more approachable cousin" (ExpertGameReviews)
- Free respec, deneme cesareti veriyor

**RIMA Adaptasyon değeri:** ⭐⭐⭐⭐ (Tier A)
- RIMA'nın Cross-Class Proc System + Shadow Echo Matrix (90 combo + 50 echo) zaten **benzer prensip**
- Slormancer "tüm sınıflar paylaşır" — RIMA "10 sınıf × cross-class" zaten ulaşmış
- Sadece **passive-line kısıtı** mekaniği RIMA için yeni: oyuncu **forced trade-off** yapar (passive A mi B mi)
- ⚠️ RIMA Karar #143-E layered pipeline ve roguelite kısıtla uyumlu değil — Ancestral Skill Tree run boyu persistent değil

### 2C. 3 Class × 3 Specialization Mekaniği
- Level 10'da unlock olan signature specialization, unique skill + skill tree
- Örnekler:
  - **Knight Distinguished:** Banners of War (random buff)
  - **Knight Haphazard:** Luck stacks (25/50/75/100) → Fortunate/Perfect cast
  - **Knight Enduring:** Deflect (damage reflect)
  - **Mage Devoted Scholar:** Emblems (3 spell school stacks)
  - **Mage Phlegmatic:** Temporal Clones (skill copies)
  - **Mage Arcane Commander:** 3 simultaneous Arcane Clones
  - **Huntress Architect:** Ballista turret + Serenity gauge
  - **Huntress Sharpshooter:** Critical + Tumble evasion
  - **Huntress Mist-Walker:** Ravenous Dagger + Poison
- **Switchable** — istediğin zaman specialization değiş

**RIMA Adaptasyon değeri:** ⭐⭐⭐ (Tier B)
- RIMA'da specialization = "verb identity" zaten (engage/break/execute Warblade, etc.)
- Specialization seçimi RIMA için **scope creep** olur — 10 sınıf × 3 spec = 30 effective class
- Ancak **Run içi temporary specialization** (Hades Aspect of X benzeri) bir Karar olabilir

---

## 3. Combat Feel — Slormancer vs RIMA

### Slormancer
- **"Snappy and responsive"** (Joe Reviews) — dash/parry/hitstop **resmi olarak yok** ama "frenetic" feedback
- **Big damage numbers** her saldırıda
- **Screen shake** toggleable option
- **Twin-stick aim** — hareket + nişan ayrı stick'ler
- **GameMaker engine** — physics/collision sınırlı, Diablo-style click ARPG değil

### RIMA
- **Input buffering** (KARAR #143-E)
- **Dash i-frame** + hitstop + shake + slowmo (game feel bible)
- **Mouse aim** (8-dir for sprite, mouse for skill)
- **Unity URP 2D + Pixel Perfect Camera**

**Karşılaştırma:** RIMA'nın combat feel **çok daha derin** zaten. Slormancer'dan combat feel açısından **al-acağımız çok az** — RIMA'nın game feel bible'ı zaten Slormancer'ın "frenetic" hedefini geçiyor.

---

## 4. Topluluk Praise/Crit (60h Player Insights)

### Sevilenler
- **Build experimentation freedom** — respec ücretsiz, deneme cesareti
- **Slorm Reaper variety** — 102 evolution chain ile build sonsuzluk
- **Quality of life** — loadouts, loot filters
- **Pixel art animation quality** — fluid, satisfying
- **Time-respect** — kısa campaign, derin endgame
- **Specialization swap** — sınıf-içi pivot

### Eleştirilenler
- **No co-op** (en büyük şikayet)
- **Endgame variety** — Library/Netherworld grind tekrar eden
- **Map design** — duvarlar/labirent/dead-end mobility'i engelliyor
- **Visual clarity** — skill effect spam görüş açısını bozuyor
- **Overwhelming UI** — başlangıç çok kafa karıştırıcı
- **Performance dips** — chaotic moments'ta FPS düşer
- **Legendary drop infrequency** — RNG açısından sinir bozucu
- **Collision detection** — environmental object'lerle çarpışma freeze ediyor

**RIMA için ders:**
- ⚠️ **UI overwhelming** problemi → RIMA HUD design 3-layer (HUD/TAB/ESC) ile baştan adres ediyor
- ⚠️ **Visual clarity** problemi → RIMA Skill Visual Contract + composition roles bunu önlüyor
- ⚠️ **Map dead-end/wall** problemi → RIMA Brush V1 Wang16 + composition roles benzer riski var, dikkat
- ⚠️ **Endgame variety** problemi → RIMA Rift Break Phase 4-5 design'da bu uyarı kritik

---

## 5. RIMA'ya Adapte Tier (Final Verdict)

### Tier S (Doğrudan al)
| # | Mekanik | RIMA implementasyonu | Karar # impact |
|---|---|---|---|
| S1 | **Slorm Reaper benzeri "Boss Drop Build-Defining Weapon"** | Boss kill → 1 unique weapon (1 build-defining modifier) | Karar #146 candidate (Weapon Component Swap) destekler |
| S2 | **One-per-line passive trade-off** | RIMA Echo Imprint sistemi 4 slot zaten benzer ama daha skinny — 6-8 slot trade-off forced choice expand edilebilir | Skill v3 önerisi |
| S3 | **Loadout sistemi (skill set save/swap)** | Run içi 2-3 loadout slot — boss vs minion için preset switch | HUD Design v2 |
| S4 | **Loot Filter (rarity threshold)** | Common/Uncommon yerden alınmaz, Rare+ pickup default | Karar #103 candidate |

### Tier A (Adapte et)
| # | Mekanik | RIMA implementasyonu | Notlar |
|---|---|---|---|
| A1 | **Specialization swap (Hades-like Aspect)** | Run sonunda Hub'da aspect değiştir (5-10 cost) | Cross-Class Skills üzerine bina edilebilir |
| A2 | **Element cross-class (5 element)** | RIMA'nın 10 sınıfı zaten verb-driven, ama Element tag system (Fire/Ice/Lightning) cross-class proc tetik mekaniği için Karar #146 candidate | Shadow Echo Matrix evolution |
| A3 | **Free respec + experimentation cesareti** | Run-içi tek skill yeniden seçim cost'u 25 ouric | UI confirmation flow |

### Tier B (Düşün, gerek yok)
| # | Mekanik | Neden tier B |
|---|---|---|
| B1 | **3 specialization per class** | RIMA'da 10 sınıf × 3 = 30 effective class scope creep |
| B2 | **Ancestral Skill Tree (persistent meta)** | RIMA roguelite — run-based — meta-progression Rift Break ile zaten kapalı |
| B3 | **Twin-stick aim** | RIMA mouse aim daha hassas, twin-stick controller-first değil |
| B4 | **120 unique weapons + 200 legendary affixes** | RIMA 64×64 chibi sprite sayısı kısıtlı, 120 sprite üretim maliyeti üzerinde |

### Tier X (Kesinlikle al-ma)
| # | Mekanik | Neden tier X |
|---|---|---|
| X1 | **Persistent ARPG progression** | RIMA roguelite genre — Karar mimarisini ihlal eder |
| X2 | **Procgen environment + 40 affix** | RIMA editor-authored RoomBank (Karar #143-E) — procgen environment scope ihlali |
| X3 | **Library/Netherworld grind** | RIMA Rift Break Phase 4-5'in **anti-pattern**'i — variety problemini şimdiden adresliyor |

---

## 6. Kamera Açısı — Spesifik Verdict (User'ın sorusu)

**Slormancer kamera ≠ RIMA kamera.**

- **Slormancer:** Steam page tag = "Isometric" + "Top-Down" (her ikisi de). Press kit specific açı vermiyor. YouTube video title = "2D Pixel-Art Twin-Stick". Karakter biraz **eğik** görünüyor — saf isometric (45° lateral) değil, 30-50° aralığı tahmini ama doğrulanamadı.
- **RIMA:** High top-down 30-35° (Hades match, Karar #100 LIVE LOCK)

**Sonuç:** Slormancer **daha çok isometric tilt** (40-50°), RIMA **daha az tilt** (30-35°). **Aynı değil** — RIMA daha "tepeden bakış", Slormancer daha "lateral perspective".

**Görsel referans:** Steam screenshot/YouTube gameplay video direkt karşılaştırma şart. Bu rapor public source'lara dayalı, kesin derece açısı **dev press kit'te yok**.

---

## 7. Opus Final Karar (Üç Soru)

### Soru 1: Slormancer kamera açısı RIMA ile aynı mı?
**Hayır.** Slormancer daha isometric (~40-50° tilt), RIMA daha top-down (30-35°). **Yakın ama eşit değil.** RIMA Karar #100 (chibi RESTORE high top-down) lock kalır.

### Soru 2: Slormancer'dan RIMA'ya ne almak değer?
**3 mekanik prensibi:**
1. **Slorm Reaper benzeri "build-defining weapon drop"** → Boss reward sistemine entegre (Karar #146 candidate ile birleşir)
2. **Loadout (skill preset save/swap)** → HUD v2 ekleme
3. **Loot filter (rarity threshold)** → UI quality of life

### Soru 3: Slormancer'dan **kesinlikle uzak durulması gereken** ne?
1. **Persistent ARPG progression** — RIMA roguelite genre'a karşıt
2. **Procgen environment 40-affix** — RIMA editor-authored Karar #143-E LIVE
3. **Library/Netherworld grind pattern** — endgame variety problemi RIMA'nın Rift Break tasarımının zaten kaçındığı tuzak

---

## 8. Aksiyon Önerileri

| Aksiyon | Öncelik | Owner |
|---|---|---|
| Slormancer YouTube gameplay 30-60dk izle (kamera açısı kesinlik için) | P2 | USER |
| Slorm Reaper "build-defining weapon" mekaniği RIMA Karar #146 candidate'a entegre öneri yaz | P1 | Opus + rima-design |
| Loadout sistemi HUD design v2 önerisi | P2 | rima-design |
| Loot filter UI quality of life RFC | P3 | rima-design |
| Mekanik bank M59-M68 listesine "Slorm Reaper Adaptation" mekaniği ekle (Tier S) | P2 | rima-doc |

---

## 9. Kaynaklar

- [The Slormancer Steam page](https://store.steampowered.com/app/1104280/The_Slormancer/)
- [Slormite Studios Presskit](https://www.slormitestudios.com/presskit)
- [The Slormancer Fandom Wiki — Slorm Reapers](https://slormancer.fandom.com/wiki/Slorm_Reapers)
- [Slormbuilds community](https://slormbuilds.github.io/)
- [TheGamer Class Specializations Guide](https://www.thegamer.com/the-slormancer-class-specializations-overview-guide/)
- [ExpertGameReviews The Slormancer Review](https://expertgamereviews.com/the-slormancer-review-a-pixel-perfect-arpg-that-respects-your-time-and-creativity/)
- [Joe Reviews The Slormancer](https://joereviews.com/the-slormancer-review-a-casual-arpg-with-surprising-depth/)
- [PCGamesN The Slormancer ARPG soars on Steam](https://www.pcgamesn.com/the-slormancer/new-arpg-out-now)
- [Steam Community 60-hour opinions discussion](https://steamcommunity.com/app/1104280/discussions/0/591773168390178277/)

---

## 10. Multi-Agent Durumu

| Agent | Durum | Katkı |
|---|---|---|
| **Opus 4.7 (this thread)** | ✅ Complete | 9 web source + Steam page + Slormite presskit + wiki — full data |
| **Codex GPT-5.5** | ✅ Complete | 12 source (Steam screenshot/trailer + Slormite patch notes + Slormbuilds + Fandom Reapers/Ancestral/Huntress) — independent analysis §11 |
| **Gemini** | ❌ Failed | Retry döngüsünde tıkandı, kullanılabilir veri yok |

**Final verdict Opus+Codex consensus:** §12

---

## 11. Codex Independent Findings (Opus'un kaçırdığı boyutlar)

Codex GPT-5.5, Opus'tan bağımsız olarak Steam patch notes + Slormbuilds.github.io + Fandom Ancestral Legacy/Huntress wiki'yi tarayıp **5 yeni önemli boyut** buldu:

### 11.1 Sprite oranı — Slormancer ≠ RIMA chibi
**Codex bulgusu:** Steam 1080p screenshot'ta karakter footprint ~45-70 ekran px yüksekliğinde, RIMA 64×64 chibi 2.5-3 head-height **compact**'inden **daha uzun ve slim**.
**Implication:** Slormancer pixel art uyumlu görünse de **chibi oranı korunmalı** — Karar #74/#100 lock kalır, Slormancer **proportional referans değil**.

### 11.2 Slorm "infinite currency", fixed slot değil
**Codex bulgusu:** Slorm sabit skill modifier slotu değil, kill-based **sonsuz currency**. Skill mastery kill ile artar, equipped skill'ler aynı mastery alır, Tier 1/2 upgrade seçimleri unlock.
**Implication:** Opus raporda "Slorm Reaper = modifier slot" söylenmişti — düzeltme: **Reaper = weapon**, Slorm = **mastery currency**. İki ayrı sistem.

### 11.3 10 gear slot itemization (Slormbuilds.io)
**Codex bulgusu:** Helmet / Amulet / Chest / Cape / Belt / Ring / Boots / Gloves / Bracers / Shoulders = 10 slot. Rarity katmanları: Normal / Magic / Rare / Epic / Legendary / Reaper / Mastery / Attribute (8 katman).
**Implication:** Slormancer **derin gear ekonomisi**. RIMA 4 equip slot lock'unu **bozmamalı** — bu derinlik 3 sınıfa yayılınca anlamlı, 10 sınıfa yayılınca scope ölür.

### 11.4 Patch 0.9 combat ekonomisi
**Codex bulgusu:**
- Attack speed → animation speed artırır (eski sistemde sadece DPS multiplier'dı)
- Cooldown reduction **cap** var
- "Fast Skill" tag = 0.3s **shared cooldown** (skill spam'ı engelliyor)
- Huntress **Tumble** = dash/movement utility

**Implication:** RIMA için **adapte değerli teknik prensipler**:
1. Attack speed = animation speed (RIMA Karar #144 weaponless body için zaten doğal)
2. Skill tag bazlı shared cooldown (4 equip slotu spam'ı önler)
3. CDR cap (build break önler)

### 11.5 Endgame Wrath 10 + Wrath 10+100 + Nether graft
**Codex bulgusu:** Endgame katmanları:
- **Battlefield Expeditions** (procgen run)
- **Slorm Temple** (Pure Slorm + Ultimatum farm)
- **Great Forge** (Rune + material craft)
- **Warlords / Netherworld** (Nether item + graft loop)
- **Wrath 10** (scaling tier)
- **Wrath 10+100** (infinite scaling)
- **Cataclysm** = boss-run boyu aktif kalan boss affix sistemi

**Implication:** Wrath 10+100 **çok ağır** — RIMA Faz 1 için scope dışı. Ama **3-5 kademeli Heat/Curse scaling** (Hades benzeri) Karar adayı olabilir.

### 11.6 Boss tasarımı eleştirisi (Codex)
**Codex'in net teşhisi:** "Slormancer boss tasarımı **affix/stat-check ARPG** modeline yakın. Hades gibi temiz **faz koreografisi** kanıtı sınırlı. Regeneration/Invulnerability affix'leri **sorunlu** olduğu için bizzat patch'lenmiş."
**Implication:** RIMA **boss tasarımı için Slormancer'a bakmamalı** — RIMA'nın 3-kanal telegraph standardı zaten Slormancer'dan üstün.

### 11.7 Codex Tier tablosu (Opus'tan farklı vurgular)

| Tier | Mekanik | Opus farkı |
|---|---|---|
| **S (Codex yeni)** | **Active skill'in kendi mini upgrade tree'si** (2-3 mastery branch) | Opus raporda yoktu — RIMA STRIKE/ZONE/REACTIVE/STATE'e direkt adapte EDEBİLİR |
| **S (Codex yeni)** | **Per-class 2-3 weapon/echo keystone** (Slormancer 120 yerine) | Opus "boss drop weapon" demişti — Codex daha yalın: sınıfı tanımlayan 2-3 keystone yeter |
| **A (Codex yeni)** | **Elite affix tooltip kartları** | Elite room varyasyonu — RIMA MOB_COMPOSITION_RULES.md'ye ek katman |
| **A (Codex yeni)** | **3-5 kademeli Heat/Curse scaling** | Wrath 10+100 yerine sade — Hades benzeri |
| **A (Codex yeni)** | **Nether graft Faz 2 craft sistemi** | Karar #146 Weapon Component Swap için doğrudan ilham |
| **B (Codex)** | Slorm infinite currency | RIMA roguelite temposunu boğar — sadece mastery XP veya Echo dust formunda |

---

## 12. COMBINED OPUS + CODEX FINAL VERDICT

### 12.1 Kamera açısı (User'ın asıl sorusu)

**Opus + Codex konsensus:** Slormancer kamera **isometric/oblique top-down karışım**, görsel açı ~**40-50°**. RIMA Karar #100 lock'u (30-35° high top-down) **DAHA AZ EĞİK**.

| Boyut | Slormancer | RIMA | Verdict |
|---|---|---|---|
| Kamera açısı | ~40-50° oblique | 30-35° high top-down | Yakın, **aynı değil** |
| Sprite oranı | Slim/tall (~45-70px tall, ~3.5-4 head) | Compact chibi (~3-4 head) | **Aynı değil** |
| Karar #100 etki | — | LOCK kalır | ✅ **Değişme yok** |

### 12.2 RIMA Adaptasyon Final Paketi (Combined)

**Opus + Codex consensus ile RIMA'ya alınacak ⭐⭐⭐⭐⭐ paket:**

**"Her skill için 3 mastery unlock + her sınıf için 2 weapon/echo keystone + elite/boss affix kartları + 3-5 kademeli endgame curse"**

Bu paket:
- ✅ Karar #74/#100/#144/#145 LIVE LOCK ile **uyumlu**
- ✅ Roguelite genre'yi **ihlal etmez**
- ✅ RIMA mevcut sistemlerine (Echo Imprint + Cross-Class Proc + Shadow Echo) **bina edilir**
- ✅ Solo-dev scope için **gerçekçi**

### 12.3 RIMA'ya 5 Yeni Karar Adayı (Bu rapordan çıkan)

| # | Karar Adayı | Tier | Sprint hedef |
|---|---|---|---|
| **Karar #147** | **Per-Skill Mastery Tree** — Her aktif skill için 3 mastery unlock (kill XP ile) → Tier 1 / Tier 2 / Tier 3 upgrade seçimi | Tier S | Sprint 16+ (after Brush V1 combat integration) |
| **Karar #148** | **Class Weapon Keystone** — Her sınıf için 2-3 build-defining weapon/echo keystone (boss reward) — Karar #146 Weapon Component Swap candidate'ı buna **eşit**, birleştirilebilir | Tier S | Sprint 18+ |
| **Karar #149** | **Elite Affix Tooltip System** — Elite room'larda 1-2 affix telegraph (Aegis / Anti-Heal / Berserk / Frenzy) | Tier A | Sprint 15 (after Sprint 14 combat) |
| **Karar #150** | **3-5 Kademeli Heat/Curse Scaling** — Hades benzeri endgame zorluk arttırma (Wrath 10+100 değil, 3-5 kademe) | Tier A | Phase 2 / Rift Break planning |
| **Karar #151** | **Loadout System (skill preset save/swap)** — Run içi 2-3 loadout slot — boss vs minion için | Tier S | HUD v2 |

### 12.4 RIMA'dan KESİNLİKLE Uzak Durulacak

| Tehlike | Slormancer'da | RIMA'ya neden tehlike |
|---|---|---|
| Persistent ARPG progression | Yes (full character persistence) | RIMA roguelite genre'yi ihlal eder |
| 120 unique weapon | Slorm Reaper sayısı | Solo-dev sprite üretim maliyeti |
| 10 gear slot | Slormbuilds.io itemization | RIMA 4 equip lock'unu bozar |
| Procgen environment 40-affix | Steam page | Karar #143-E editor-authored RoomBank ihlali |
| Wrath 10+100 infinite scaling | Endgame Netherworld | Scope death — Faz 1 için 5x fazla |
| Library/Netherworld grind | 60h player critique | RIMA Rift Break tasarımı bu tuzağı zaten kaçınıyor |
| Slormancer kamera açısı (~40-50°) | Steam tags | Karar #100 (30-35°) lock'u ihlal eder |

---

## 13. Aksiyon Planı (P0-P3)

| # | Aksiyon | Öncelik | Owner | Çıktı |
|---|---|---|---|---|
| 1 | Bu raporu user onayına sun | P0 | Opus → USER | User feedback |
| 2 | **Karar #147-#151** önerileri user onayında MASTER_KARAR_BELGESI.md'ye eklenir (rima-doc dispatch) | P0 (user onayı sonrası) | rima-doc | 5 yeni karar locked |
| 3 | Slormancer YouTube gameplay 30dk izle (kamera açısı görsel doğrulama) | P1 | USER | Visual sanity check |
| 4 | Mekanik Bank M59-M68 listesine **M69-M73** (Per-Skill Mastery Tree, Class Weapon Keystone, Elite Affix Tooltip, Heat/Curse Scaling, Loadout) **Tier S/A** etiketli eklenir | P1 | rima-doc | Bank update |
| 5 | RIMA Karar #146 (Weapon Component Swap) ile Karar #148 (Class Weapon Keystone) **birleştirme önerisi** RFC yaz | P2 | rima-design | RFC document |
| 6 | Sprint 14 planlama: Combat integration / Per-Skill Mastery Tree pilot Warblade'de prototype edilir mi? | P2 | rima-design | Sprint 14 spec |
| 7 | Slormancer presskit'ten kalan boyutlar (sound design, music, story) için 1 takip araştırma (rima-research) | P3 | rima-research | Supplementary report |

---

## 14. Sources (Combined Opus + Codex)

### Opus sources (Web fetch)
- [Steam — The Slormancer](https://store.steampowered.com/app/1104280/The_Slormancer/)
- [Slormite Studios Presskit](https://www.slormitestudios.com/presskit)
- [Slormancer Fandom Wiki — Slorm Reapers](https://slormancer.fandom.com/wiki/Slorm_Reapers)
- [Slormbuilds community](https://slormbuilds.github.io/)
- [TheGamer Class Specializations Guide](https://www.thegamer.com/the-slormancer-class-specializations-overview-guide/)
- [ExpertGameReviews The Slormancer Review](https://expertgamereviews.com/the-slormancer-review-a-pixel-perfect-arpg-that-respects-your-time-and-creativity/)
- [Joe Reviews The Slormancer](https://joereviews.com/the-slormancer-review-a-casual-arpg-with-surprising-depth/)
- [PCGamesN The Slormancer ARPG soars on Steam](https://www.pcgamesn.com/the-slormancer/new-arpg-out-now)
- [Steam Community 60-hour opinions](https://steamcommunity.com/app/1104280/discussions/0/591773168390178277/)

### Codex sources (cx_dispatch.py independent research)
- [Steam screenshot reference](https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/1104280/ss_3f44cf82abd3ad677fe416e7786285d1c5d677d9.1920x1080.jpg)
- [Release trailer](https://www.youtube.com/watch?v=ZSQDgjwvulc)
- [Slorm Temple update news](https://store.steampowered.com/news/app/1104280/view/3002197411186856531)
- [Patch 0.9 balance notes](https://www.slormitestudios.com/patch_0_9_0.php)
- [Slormbuilds crafting/item slots](https://slormbuilds.github.io/posts/crafting/)
- [Slormbuilds ancestral skill tree WIP](https://slormbuilds.github.io/posts/2448197582014567502/)
- [Fandom Ancestral Legacy](https://slormancer.fandom.com/wiki/Ancestral_Legacy)
- [Fandom Huntress class page](https://slormancer.fandom.com/wiki/The_Fierce_Huntress)
- [Steam release date / Netherworld notes](https://store.steampowered.com/news/posts/?appids=1104280&enddate=1745744297&feed=steam_community_announcements)
- [Steam Warlords / Legendary / Nether notes](https://store.steampowered.com/news/posts/?appids=1104280&enddate=1730889893&feed=steam_community_announcements)

---

## 15. Antigravity Inner-Doc Analysis (Opus + Codex tamamlayıcı)

**Antigravity (Google IDE agent)** RIMA iç dosyalarına (CLASS_SILHOUETTE_BIBLE.md, GDD, RIMA_GAME_FEEL_AND_MECHANICS_BIBLE.md, CameraFollow.cs, UIManager.cs, RimaUITheme.cs, Vivid Vulnerability palette, Karar mantığı) erişimle Slormancer karşılaştırması yaptı. **Opus + Codex dış-kaynak** baktığı için, Antigravity **RIMA-spesifik adapte mekaniği** 3 yeni boyutta çıkardı.

### 15.1 Antigravity'nin 3 yeni katkısı

**A1: Cursor-Based Active Camera Pan** ⭐⭐⭐⭐⭐ (Opus/Codex'te yoktu)
- **Slormancer mekaniği:** Kamera fare imlecine doğru hafifçe kayarak oyuncuya "ileriyi görme" avantajı sağlar
- **RIMA implementasyonu:** `CameraFollow.cs` içine player + cursor weighted center (örn. %80 player + %20 cursor offset)
- **Hangi sınıflar için kritik:** Ranger (kite + menzil), Gunslinger (Cursor Storm, Deadshot), Hexer (Blight Sigil placement) — aim-mechanic sınıflar
- **Karar #100 (30-35° top-down) ile uyumlu:** Aktif kamera açıyı değiştirmez, sadece offset ekler

**A2: Dynamic UI Clutter Control** ⭐⭐⭐⭐ (Opus generic "loot filter" söyledi, Antigravity RIMA-spesifik adapte)
- **Slormancer mekaniği:** Yoğun savaşlarda damage text + loot etiketleri otomatik küçülür/gruplaşır
- **RIMA tetik:** Threat points + ekrandaki enemy sayısı (Summoner minion + Hexer stack burst durumları)
- **RIMA implementasyonu:** `UIManager.cs` + `RimaUITheme.cs` içine "DamageNumberScale" function — threat ≥ X olunca damage number 0.7x scale + 0.5s group merge
- **Hangi durumlar için kritik:** Summoner orduları, Hexer Blight Sigil zone burst, boss multi-phase

**A3: AoE Telegraph Decal Palette** ⭐⭐⭐⭐ (Opus/Codex'te yoktu — RIMA palette-spesifik)
- **Slormancer mekaniği:** Kalabalık AoE alanları "zıt neon decal" ile yere işaretlenir (yer + skill telegraph)
- **RIMA implementasyonu:** Hexer Blight Sigil ve Ranger Bone Trap için **zıt neon decal pass**
  - Hexer: `#8B0000` (kan kırmızısı vurgu — Vivid Vulnerability uyumlu)
  - Ranger: `#7BA7BC` (cold-blue vurgu)
- **Hangi karar uyumu:** TASARIM/VISUAL_TONE_BIBLE.md "Ritual Catastrophe" palette + Skill Visual Contract — telegraph decal'ler base tonun **dışına** çıkmalı

### 15.2 Antigravity'nin doğrulayıcı katkıları (Opus/Codex zaten bulmuştu)

| Bulgu | Opus | Codex | Antigravity | Konsensus |
|---|---|---|---|---|
| Kamera açısı ≠ aynı | ✅ | ✅ | ✅ | LOCK (Karar #100 değişmez) |
| Silah evrimi yapma | (yok) | ✅ Tier B | ✅ "DİKKAT!" | LOCK (Karar #71/#80/#144 ihlali) |
| Skill modifier (Hades offer) | Tier S #2 | Tier S | ✅ "Iron Charge shockwave" örneği | Karar #147 PILOT ÖNERİ |
| Hitstop + Screen Shake referans | ◐ | ✅ "screenshake option" | ✅ "Earthsplitter/Curbstomp doğrulama" | RIMA bible zaten var, Slormancer = referans |
| Slormancer komedi tonu uymaz | (yok) | (yok) | ✅ "Karar #79 ihlali" | Yeni LOCK: tone uymaz |

### 15.3 Antigravity'nin kaçırdıkları (Opus + Codex daha derin)

Antigravity şu boyutları **söylemedi** (RIMA iç-doc odaklı olduğu için dış-kaynak detayları yok):
- Sprite oranı slim/tall vs chibi compact farkı (Codex yakaladı)
- Slorm = currency, NOT slot (Codex düzeltti)
- 10 gear slot itemization detayı (Codex)
- Patch 0.9 attack speed/CDR cap/Fast Skill (Codex)
- Wrath 10+100 endgame scaling (Codex)
- Endgame variety eleştirisi 60h player (Opus)
- Per-Skill Mastery Tree net önerisi (Opus + Codex)
- Class Weapon Keystone (Codex)
- Elite Affix Tooltip System (Codex)
- 3-5 Kademeli Heat/Curse Scaling (Codex)
- Loadout System (Opus)

### 15.4 Antigravity'den TÜREYEN 3 yeni Karar Adayı

| # | Karar Adayı | Tier | Sprint hedef | Owner |
|---|---|---|---|---|
| **Karar #152** | **Cursor-Based Active Camera Pan** — Player+cursor weighted center offset (%80+%20) `CameraFollow.cs` extension | S | Sprint 14 (Combat integration ile birlikte) | Codex impl |
| **Karar #153** | **Dynamic UI Clutter Control** — Threat-based damage number scale + loot text group merge | A | Sprint 15 (HUD overhaul içinde) | rima-codex |
| **Karar #154** | **AoE Telegraph Decal Pass** — Vivid Vulnerability dışı neon palette (Hexer #8B0000, Ranger #7BA7BC) skill decal'leri için | A | Sprint 16+ (Visual contract pass) | rima-asset + Codex shader |

⚠️ **Antigravity'nin söylediği Karar #71/#79/#80 sayıları doğrulanmalı** — MASTER_KARAR_BELGESI.md ile cross-check gerekir (NLM canonical). Önerinin **prensibi** geçerli, **numaralandırma** rima-doc tarafından konfirme edilmeli.

### 15.5 Final 8 Karar Adayı (Combined Opus + Codex + Antigravity)

| # | Karar | Kaynak | Tier |
|---|---|---|---|
| **#147** | Per-Skill Mastery Tree (3 unlock/skill) | Codex + Opus | S |
| **#148** | Class Weapon Keystone (2-3/sınıf, boss reward) | Codex + Opus | S |
| **#149** | Elite Affix Tooltip System | Codex | A |
| **#150** | 3-5 Kademeli Heat/Curse Scaling | Codex | A |
| **#151** | Loadout System (skill preset save/swap) | Opus | S |
| **#152** | **Cursor-Based Active Camera Pan** | **Antigravity** | **S** |
| **#153** | **Dynamic UI Clutter Control** | **Antigravity** | **A** |
| **#154** | **AoE Telegraph Decal Pass** | **Antigravity** | **A** |

### 15.6 Multi-Agent Consensus

| Boyut | Opus | Codex | Antigravity |
|---|---|---|---|
| Kamera açısı ≠ aynı | ✅ | ✅ | ✅ |
| Silah evrimi alma | (yok) | ✅ | ✅ |
| Slormancer tonu uymaz | (yok) | (yok) | ✅ |
| Per-Skill Mastery | ◐ | ✅ | ◐ |
| Class Weapon Keystone | ✅ Tier S | ✅ Tier S | ◐ |
| Active Camera | (yok) | (yok) | ✅ |
| UI Clutter Control | (loot filter generic) | (yok) | ✅ RIMA-specific |
| AoE Telegraph Decal | (yok) | (yok) | ✅ palette |

**Sonuç:** 3 agent **birbirini tamamladı** — dış kaynak (Opus + Codex) + iç doc (Antigravity) = 8 karar adayı paketi. Hiçbir agent tek başına 8'in hepsini bulamazdı.
