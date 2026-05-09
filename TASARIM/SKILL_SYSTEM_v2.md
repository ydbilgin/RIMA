# RIMA Skill System v2 — LOCKED
# Tarih: 2026-05-09 | Opus + Codex sentezi | Onay: Yasin

Bu dosya RIMA'nın v2 skill sistemini tek yerde toplar. Tüm kararlar LOCKED.
Shadow Echo havuzu detayı: `TASARIM/SHADOW_ECHO_MATRIX.md`
v1 referansı: `TASARIM/SKILL_SYSTEM_TAXONOMY_2026-05-06.md` (LOCKED, korunur)

---

## 8 LOCKED KARAR (S47 Session)

### KARAR 1 — Warblade Identity: Verdict Ledger
**Mekanik:** Her commit-beat (3. LMB) = 1 Ledger stack. **3 stack** birikince sıradaki **herhangi bir skill** kullanımında Sundered durumunu garantili uygular. Stack'ler oda çıkışında sıfırlanır.
**Neden:** İron Verdict otomatik proc oyuncu hissetmiyordu. Sayılabilir hale getirildi. STRIKE-only kısıtlaması kaldırıldı (Codex: ZONE-leading build cezalandırılmasın).
**Değiştirir:** Iron Verdict (eski) → Verdict Ledger (yeni)

### KARAR 2 — Ranger Identity: Range Bands
**Mekanik:** 2 mesafe bandı:
- **<5 tile:** -%15 hasar (yakın ceza)
- **≥5 tile:** +%15 hasar + crit şansı (uzak bonus)
- Knockback YOK, mesafe indikatorü minimal
**Neden:** Distance Discipline tek eşik binary'di. 2 band oyuncuyu mesafe yönetimine zorlar. Knockback Ranger'ın kendi ZONE tuzaklarını bozardı (Codex bulgusu).
**Değiştirir:** Distance Discipline (eski) → Range Bands (yeni)

### KARAR 3 — Ravager Identity: Blood Tide
**Mekanik:** Aldığın her hit = +%6 STRIKE hasar bonus, 4s süre, **max 5 stack** (+%30). **Oda girişinde min 2 stack floor**. Yeni hit duration'ı refresh eder (independent timer değil).
**Neden:** Carnage Pulse pasif HP eşiğine bağlıydı. Blood Tide aktif öfke biriktirir. Floor cold-start sorununu çözer (Codex bulgusu).
**Değiştirir:** Carnage Pulse (eski) → Blood Tide (yeni)

### KARAR 4 — Summoner Identity: Necrotic Toll
**Mekanik:** Minyon ölümünde:
- 0.4s i-frame (KORUNUR — defansif güvenlik)
- 3s pencere açılır → bu pencerede sonraki herhangi skill **+%30 hasar + AoE patlama**
**Neden:** Eski Soul Bond sadece defansif/sıkıcıydı. Necrotic Toll i-frame KORUR (Codex: class-feel regresyon olmasın) + ofansif fırsat ekler.
**Değiştirir:** Soul Bond (eski) → Necrotic Toll (yeni)

### KARAR 5 — Cross-Class: Shadow Echo Sistemi
**Mekanik:** Z/X slotları KALDIRILDI. Cross-class kimliği üç katmanda verilir:
1. **Skill Evolution:** Q/E/R/F draft'ında cross-class evolution seçenekleri çıkar
2. **Shadow Echo:** Evolved skill ateşlendiğinde, kaynak sınıfın phantom shadow'u skill tipine göre konumlanıp Shadow Echo'sunu patlatır
3. **Ambient Aura:** Player etrafında sürekli RESONANCE renginde particle aura

**Pozisyon kuralı (skill tipine göre):**
| Skill Tipi | Shadow Pozisyonu |
|---|---|
| Melee STRIKE | Hedefin üstünde |
| Ranged STRIKE | Player'ın yanında (~24px omuz arkası, ters yön) |
| ZONE | Hedef noktada (cursor/yer) |
| Self-buff | Player'ın üstünde |

**Görsel spek:** alpha 0.3 max, cyan tint #00FFCC, süre 0.4s, Z-sort spawn Y'sinde.
**UI:** Ekran kenarında 1-frame icon + 12px text ("Hex Echo!" gibi).
**Havuz:** 10 sınıf × 5 Shadow Echo = 50 Echo. ~30 mevcut skill reuse, ~20 yeni shadow-spesifik.
**Değiştirir:** Z/X Ghost Attack slot sistemi (eski) → Shadow Echo (yeni)

### KARAR 6 — Upgrade Slotları: Label-Only Kategoriler
**Mekanik:** 3 upgrade slot UI'da Shape/Edge/Twist olarak **etiketlenir**:
- **Shape:** Skill formunu/davranışını değiştirir
- **Edge:** Sayısal yükseltme (hasar/CD/radius)
- **Twist:** Koşullu rider ("HP düşükken", "boss'a karşı")

Kullanıcı istediği slota istediğini koyabilir — **enforcement YOK**. Draft sistemi mevcut haliyle korunur. Playtest'te degenerate build görülürse v2'de enforcement düşünülür.
**Neden:** Tam kategori sistemi 2 hafta scope rewrite. Label %70 cognitive payoff veriyor (Codex bulgusu).
**Değiştirir:** Eski 3 boş slot (UI etiket eksik) → Shape/Edge/Twist label

### KARAR 7 — Resonance Pasifi: Altar Selection
**Mekanik:** RESONANCE pasifi sadece **Altar odasında** seçilir. **Act başına 1 Altar**, mutlaka **ana güzergahta** (atlanabilir branch'te değil), 3 seçenek sunulur. Random drop yok.
**Neden:** Cross-class köprüsü kritik karar — RNG'ye bırakılamaz. Altar build-defining moment yaratır. 1/Act scarcity korunur.
**Değiştirir:** Random Resonance drop (eski) → Altar selection (yeni)

### KARAR 8 — Ultimate Decay
**Mekanik:** Ultimate threshold (Perfect Condition) kullanılmayan her oda **-%10** düşer. **Floor cap -%40** (4 oda max). Ult cast olunca decay sıfırlanır. **HUD'da görünür bar.**
**Neden:** Stale ult önlenir, düzenli kullanım teşvik edilir. Floor cap passive-play trivializasyonu engeller (Codex bulgusu).
**Değiştirir:** Sabit threshold (eski) → Decay + floor (yeni)

---

## HOLD MEKANİK YASAĞI (LOCKED)

**v1'de hold mekaniği YOK.** Sadece tap input.

**Neden:**
- ActionCommitProfile felsefesine ters ("vuruşa başladıysan devam et")
- Hold timing yeni oyuncu eğrisini sertleştirir
- Animation budget riski (charge animation = +%30 sprite)
- RIMA henüz Hades 2 deneyim derinliğinde değil

**İstisna:** Ronin Iaido Stance zaten "sabit dur = Tension drop" — sınıf-spesifik mekanik, sistemik değil.
**v2:** Playtest sonrası gerekirse RMB'ye sınıfa özel hold eklenebilir.

---

## FİNAL KEYBIND (LOCKED — Progressive Reveal)

### Act 1 (8 combat input)
| Tuş | Eylem |
|---|---|
| **WASD** | Hareket |
| **LMB** | Temel saldırı |
| **RMB** | İkinci saldırı |
| **Q** | Skill 1 (Primary class) |
| **E** | Skill 2 (Primary class) |
| **R** | Skill 3 (Primary class) |
| **F** | Skill 4 (Primary class) |
| **V** | Ultimate |
| **Space** | Dash |
| **Z** | (gizli/disabled — Act 2 boss sonrası açılır) |
| **X** | (gizli/disabled — Act 2 boss sonrası açılır) |

### Act 2+ (10 combat input — Secondary Class unlock sonrası)
| Tuş | Eylem |
|---|---|
| **Z** | Secondary class STRIKE skill 1 |
| **X** | Secondary class STRIKE skill 2 |

### UI / World (her zaman aktif)
| Tuş | Eylem |
|---|---|
| **G** | Interact (chest/NPC/altar) |
| **M** | Map |
| **C** | Character/Skill panel |
| **Tab** | Minimap toggle |
| **Esc** | Pause/Settings |

**Progressive reveal:** Yeni oyuncu Act 1'de sade 8 input öğrenir, Act 2 boss sonrası secondary class aktive olunca Z/X açılır → 10 input. Hades 2 deneyim eğrisi gibi.

---

## ANIMATION ÖNCELİĞİ (Değişmedi)

1. Warblade (framework kurar)
2. Ronin (Sheathe/Draw kritik)
3. Shadowblade (Phase Step + Scar timing)
4. Ranger (projektil, hızlı kazanım)
5. Brawler (4-hit jab template)
6. Gunslinger
7. Elementalist
8. Ravager
9. Hexer
10. Summoner

**Animation Bible:** `TASARIM/ANIMATION_BIBLE.md` (LOCKED 2026-05-09)

---

## SCOPE FLAG (Sprint Planlama)

**Bu sprint kod gerekir:**
- Karar 1 (Verdict Ledger stack + room-exit hook)
- Karar 2 (range check on attack)
- Karar 5 — Shadow Echo sistemi (shader + spawn logic + UI flash)
- Karar 8 (decay tick + UI bar)

**Sonraki sprint (sınıflar mevcut olduktan sonra):**
- Karar 3 (Ravager class)
- Karar 4 (Summoner minion sistem)
- Karar 6 (UI label — trivial)
- Karar 7 (Altar oda tipi — Act yapısı)

**Shadow Echo geliştirme maliyeti:** ~3.5 gün (shader, spawn, UI, particle aura). Ek animation üretimi GEREKMİYOR (mevcut animasyonlar reuse).

---

## KORUMA: LOCKED RULES İHLALİ YOK

Bu kararlar mevcut LOCKED kuralları ihlal etmiyor:
- ✅ 10 sınıf listesi değişmedi
- ✅ Skill taxonomy 4 aktif + 4 pasif tip korundu
- ✅ LMB/RMB/skill/Ult yapısı korundu (Z/X kaldırıldı, Shadow Echo değiştirdi)
- ✅ Dash-cancel pencereleri değişmedi
- ✅ HP-Execute yasağı değişmedi
- ✅ Hexer Exclusivity korundu
- ✅ Identity Passive sistemi yapısı korundu (4 sınıfın passive'i değişti — taxonomy değil)

---

## CONFLICT RESOLUTIONS (NLM Audit 2026-05-09)

NLM tutarsızlık sorgusu 6 çakışma tespit etti. Çözümler:

### Çakışma 1: Hold Yasağı ↔ Aim Shot / Area Skill Placement
**Kaynak çakışan:** `AIM_SHOT_BOSS_PLACEMENT_SYSTEMS.md` — eski "hold past 0.25s threshold"
**Çözüm:** Aim Shot ve Area Skill Placement **TAP-MODE** sistemine geçti:
- Tap skill key → cursor reticle olur, aim/placement mode aktif
- Tap key again (veya LMB) → ateşler/yerleştirir
- Right-click veya ESC → mode iptal (CD harcanmaz)
- Ranger LMB tap → aim mode → tap → fire (eski "hold to charge" yerine)
**Hold yasağı KORUNUR.** Tap-mode hold değil, 2-stage activation.
**Dosya güncellendi:** `TASARIM/AIM_SHOT_BOSS_PLACEMENT_SYSTEMS.md` (UPDATED 2026-05-09 S47)

### Çakışma 2: Z/X Kaldırılması ↔ Secondary Class Progression
**Kaynak çakışan:** `SKILL_DRAFT_SYSTEM` — Act 2 secondary class +2 slot açar (=Z/X)
**Çözüm:** **Z/X korunur AMA sadece Secondary Class slot olarak. Ghost Attack rolü silindi, Shadow Echo aldı.**
- **Act 1:** 4 active slot (Q/E/R/F) + LMB/RMB/V/Space = 8 input. Z/X **görünmez/disabled**.
- **Act 2 Boss sonrası:** Secondary class seçilir → Z/X aktive olur, secondary class STRIKE skill'leri buraya gelir → 10 input
- **Shadow Echo** ayrı sistem — Q/E/R/F draft evolution'larıyla cross-class kimliği. Z/X ile bağlantısız.
- **HUD:** 6 slot tasarımı korunur ama Z/X Act 1'de soluk/kilitli görünür (`HUD_DESIGN_SPEC.md` korunur)
**Sonuç:** Player progression organik — Act 1 sade öğren, Act 2 expanded build.

### Çakışma 3: Altar Resonance ↔ 15-Node Map (REVIZE)
**Kaynak çakışan:** `dungeon_act1_map.md` — Mystery (Node 6b) DAL/opsiyonel, ana güzergahta değil
**Çözüm:** **Altar = pre-boss yeni node (Node 12.5 — "Altar of Resonance")**
- Boss'tan hemen önce eklenir (Node 12 → Altar → Node 13 Boss)
- Hades pre-boss room mantığı (atlanamaz, ana güzergah)
- Toplam node: 15 → 16 (1 ek node)
- Depth band: F3 (boss bölgesi)
- Combat olmaz — sadece RESONANCE pasif seçimi (3 seçenek)
**Dosya etkisi:** `dungeon_act1_map.md` 16. node eklenecek (sonraki sprint critical task)

### Çakışma 4: Verdict Ledger ↔ Class State Contract Broken/Sundered
**Kaynak çakışan:** `CLASS_STATE_CONTRACT.md` — Sundered = 3 Broken stack auto-convert
**Çözüm:** **İki yol da geçerli (parallel paths):**
- **Klasik path:** 3 Broken stack birikir → Sundered'a otomatik dönüşür (cross-class Brawler/Warblade köprüsü için kritik, KORUNUR)
- **Verdict Ledger path:** Warblade'in 3 commit-beat'i → sıradaki skill direkt Sundered (Warblade-only fast lane)
- Cross-class Brawler hâlâ Broken stack'leri biriktirebilir → Sundered'a köprü kurar
**Sonuç:** Warblade'in iki rotası var, cross-class köprü kırılmaz.

### Çakışma 5: Blood Tide ↔ Bleed Cross-Class Sinerji
**Kaynak çakışan:** Eski Carnage Pulse Bleed-focused, Hexer/Brawler bleed pasifleri sinerji kuruyordu
**Çözüm:** **Blood Tide'a Bleed bonus eklenir:**
- Her stack: **+%6 STRIKE hasar + %5 Bleed tick hasarı**
- Max 5 stack: +%30 STRIKE + %25 Bleed tick
- Cross-class Hexer/Brawler bleed pasifleri Blood Tide ile sinerji kurar (kayıp yok)
**Sonuç:** Yeni aktif mekanik + eski Bleed kimliği korundu.

### Çakışma 6: Shadow Echo Cyan Tint ↔ Shadowblade Echo Violet
**Kaynak çakışan:** `SHADOWBLADE_ECHO_SYSTEM.md` — Shadowblade'in kendi Echo'su violet (#indigo)
**Çözüm:** **Renk anlam ayrımı yapılır (feature, not bug):**
- **Cross-class Shadow Echo:** Cyan #00FFCC (Rift Energy) → "ödünç güç"
- **Shadowblade kendi Echo:** Violet/indigo (Class Energy) → "kendi gücü"
- Player iki rengi öğrenir: violet = sınıfımın enerjisi, cyan = cross-class
**Sonuç:** Renk dili güçlenir, görsel okunabilirlik artar.

---

## ÖZET — Tüm Çakışmalar Çözüldü

| # | Çakışma | Çözüm | Etkilenen Dosya |
|---|---|---|---|
| 1 | Hold yasak vs Aim Shot | Tap-mode (2-stage) | AIM_SHOT_BOSS_PLACEMENT (güncellendi) |
| 2 | Z/X vs Secondary Class | Z/X korundu, secondary class slotu | HUD_DESIGN_SPEC (korunur) |
| 3 | Altar vs Map | Mystery alt-tür | dungeon_act1_map (sonraki sprint not) |
| 4 | Verdict Ledger vs Broken | Parallel paths | CLASS_STATE_CONTRACT (korunur) |
| 5 | Blood Tide vs Bleed | Stack'e Bleed bonus | (mevcut sınıf pasifleri korunur) |
| 6 | Shadow Cyan vs Shadowblade Violet | Renk dili (feature) | SHADOWBLADE_ECHO_SYSTEM (korunur) |

**Sonuç:** 8 LOCKED karar tutarsız değil — eski dokümanlarla harmonize edildi.
