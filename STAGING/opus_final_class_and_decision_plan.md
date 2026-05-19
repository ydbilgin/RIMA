---
status: OPUS_FINAL_VERDICT
faz: post-Brush_V1
tarih: 2026-05-18
ozet: "10 yeni karar + Image #14 class plan — Opus final, user onayına hazır"
authors: Opus 4.7 (final), reviewed by Codex (8-decision + image-diversity passes)
---

# RIMA — Opus Final Verdict + Net Class Plan

## TL;DR
- ✅ **10 yeni karar onaylandı** (#147-#156), 5-7 sprint kademeli ship
- ✅ **10 sınıf canonical lock korunur** — 11. class YOK, scope explosion red flagged
- ✅ **4 Image #14 slot canonical accept** (Ravager / Brawler / Ranger / Summoner)
- ✅ **(4,4) → Brawler Female variant** (state production via Karar #145)
- ✅ **(4,2) → Frost Elementalist skin** (cosmetic dil, ice skill ağacı YOK) ya da Hub Lore-Keeper NPC
- ⚠️ **Drift'ler regen:** (1,2) Ranger drift + (3,4) Elementalist red-drift devam — kabul **EDİLEMEZ**
- ⚠️ **Conditional regen:** Warblade armor + Ronin topknot + Hexer curse focus + Shadowblade purple
- 📋 **Variant netleştirme:** "Variant" = aynı sınıfın **alt skin'i** (Brawler male + Brawler female) — state variants Karar #145 workflow ile **otomatik** (mevcut LIVE)

---

## §1. Class Plan (NET — bu bu class olacak)

| # | Sınıf | Canonical Body Kaynağı | Status | Aksiyon |
|---|---|---|---|---|
| 1 | **Warblade** | Image #14 (1,1) | ◐ Conditional | **Regen:** beard ✓, armor brown leather + brass buckle gerek |
| 2 | **Elementalist** | Image #14'te YOK (canonical) | ❌ Drift | **Regen tam yeni:** honey-blonde low bun + dusty indigo crop top + cream sash + deep teal skirt. (3,4) red-drift kabul **EDİLEMEZ** |
| 3 | **Shadowblade** | Image #14 (2,3) | ◐ Conditional | **Regen:** soft purple accent + narrow phase-assassin silhouette gerek (yoksa Ronin/Warblade duplicate) |
| 4 | **Ranger** | Image #14 (2,4) | ✅ **CANONICAL ACCEPT** | Bleached-ivory uzun saç'a minor tweak, forest tones doğru |
| 5 | **Ravager** | Image #14 (1,4) | ✅ **CANONICAL ACCEPT** | Image #12 shirtless drift **DÜZELDİ** (dark blood-red armor + harness + studs) |
| 6 | **Ronin** | Image #14 (1,3) | ◐ Conditional | **Regen:** topknot/tail veya katana sheath cue gerek |
| 7 | **Gunslinger** | **Image #14 (4,4)** — Female canonical (5M/5F lock teyit) | ✅ **CANONICAL ACCEPT (USER FIX)** | Sleek dark outfit → grey-purple trench recolor + twin pistols WeaponSR child + Gunslinger pose |
| 8 | **Brawler** | Image #14 (2,2) | ✅ **CANONICAL ACCEPT** | Bandaged hands / boxing guard pose minor tweak |
| 9 | **Summoner** | Image #14 (3,1) | ◐ Conditional | **Regen:** staff/lantern gesture veya summoning hand language gerek |
| 10 | **Hexer** | Image #14 (2,1) | ◐ Conditional | **Regen:** curse focus / dark red curse accent gerek (yoksa generic hooded caster) |

### Üretim Sırası (Sprint 14)

**Faz A: Canonical Accept (anında kullan)**
- (1,4) Ravager
- (2,2) Brawler
- (2,4) Ranger (minor tweak)
- (3,1) Summoner

**Faz B: Conditional Regen (single-prompt tweak)**
- (1,1) Warblade armor color fix
- (1,3) Ronin samurai cue add
- (2,1) Hexer curse focus add
- (2,3) Shadowblade purple accent add

**Faz C: Tam Regen (yeni prompt)**
- Elementalist canonical (Image #14'te yok — drift devam ediyor)
- Gunslinger canonical (Image #14'te yok)

---

## §2. Variant / Skin Plan (User'ın asıl niyeti netleştirildi)

### Anladığım: "Variant" = aynı sınıfın alt skin'i (yeni class değil)
- Karakter seçim ekranı: zaten variant UI desteği var (Hades-style cosmetic skin selector)
- Production workflow: PixelLab Character States (Karar #145 LIVE) — aynı karakterin state variants (idle/run/attack/hit/parry/death) **otomatik**
- (4,4) bir SKİN olur → Brawler Female alt skin → onun state variants Karar #145 workflow ile **mevcut pipeline** üretir

### Pilot Skin Batch 1 (Sprint 17) — USER FIX'TEN SONRA GÜNCEL
**Not:** (4,4) artık Gunslinger Female CANONICAL anchor olarak Sprint 14'te üretilir (skin değil, base class anchor). Diversity expansion canonical Gunslinger ile sağlanır. Brawler Female skin Phase 2'ye taşındı.

| Slot | Class | Skin Adı | Anchor | Pilot mı? |
|---|---|---|---|---|
| **(4,2)** | **Elementalist** | **Frost Elementalist (cosmetic skin)** | (yeniden üretilecek canonical Elementalist) | ⚠️ Canonical Elementalist passes ETMEDEN pilot **YOK** |
| **(4,1)** | **Brawler** | **Brawler Monk Alt** (wraps + bandage hands eklenirse) | (2,2) male canonical | ✅ İkinci pilot adayı |

### Önemli Constraint (Codex onaylı)
- (4,2) Frost Elementalist hair teal/blue OK, ama **silhouette Elementalist olmalı** (open palm + rune disc + casting pose)
- Outfit lavender'dan **dusty indigo + cream sash + deep teal** family palette'e kaymalı
- **Frost = sadece kozmetik dil** — ice skill ağacı / yeni damage type / yeni passive **YASAK**

### Skin Batch 2 (Phase 2 — sonra)
- (3,2) Ravager Dark-Skin Female (dark blood-red armor + broader stance gerek)
- (4,1) Brawler Monk Alt (wraps + bandage hands eklenirse)
- Diğer 7 sınıf için "1 alt skin" sonra ek batch

---

## §3. Hub NPC Plan (Cinematic Layer V1 entegrasyonu)

### v1 — 3 NPC limit

| Rol | Slot adayı | İşlev |
|---|---|---|
| **Vendor / Crafter** | (3,3) silver-haired mentor | Upgrade + relic cleanup + weapon skin preview |
| **Mentor / Trainer** | (4,1) beige robe trainer veya (4,3) silver-bearded | Class tips + post-run failure lines |
| **Lore-Keeper / Alchemist** | (4,2) teal-haired (eğer Frost skin değilse) | Rift/element anomaly açıklaması |

### Yasak (Codex onaylı)
- v1'de **3'ten fazla NPC YOK** (writing + naming + animation + localization yükü)
- Full dialogue tree YOK — Cinematic Layer V1 one-line progression (zaten LIVE design)

---

## §4. 10 Yeni Karar — Opus Final Verdict (her birine son söz)

| # | Karar | Codex Verdict | Opus Final | Sprint |
|---|---|---|---|---|
| #147 | Per-Skill Mastery Tree (3 tier × 2-3 choice) | PASS | ✅ **CONFIRM PASS** | 16 |
| #148 | Class Weapon Keystone (10 weapon-bound skill, 1/sınıf) | MODIFY | ✅ **CONFIRM MODIFY** — Codex 1/sınıf önerisi mantıklı | 18 pilot + Phase 2 full |
| #149 | Elite Affix Tooltip v1 | PASS | ✅ **CONFIRM PASS** | 15 |
| #150 | 3-5 Heat/Curse Scaling | PASS | ✅ **CONFIRM PASS** | Phase 2 |
| #151 | Loadout System (preset, no combat-swap) | MODIFY | ✅ **CONFIRM MODIFY** — room-entrance swap yeter | 15 (HUD v2) |
| #152 | Cursor Active Camera (per-class config 0.10-0.22 weight) | MODIFY | ✅ **CONFIRM MODIFY** — default OFF for melee, ON for Ranger/Gunslinger/Hexer | **14** |
| #153 | UI Clutter Control (threat-point damage scale) | PASS | ✅ **CONFIRM PASS** | **14-15** |
| #154 | Telegraph Contract (outline + pulse + color + timing) | MODIFY | ✅ **CONFIRM MODIFY** — non-color primary cue zorunlu | 16 |
| **#155** | Class Skin Variant System | MODIFY/PASS | ✅ **CONFIRM** — pilot 3 → full 10 | 17 + Phase 2 |
| **#156** | Hub NPC System (3 NPC v1) | PASS tight scope | ✅ **CONFIRM PASS** | 18 (Cinematic Layer V1 ile) |

### Opus'un Codex'e eklediği oyun-geneli stratejik notlar
- **Sprint 14 başlangıçtan readability foundations:** Sonraki tüm sistemler #153 UI clutter + #152 camera offset'in üzerine inşa edilecek — yoksa #147 mastery + #149 affix + #154 telegraph **birikecek** ve oyuncu görüş alanı boğulacak. **Codex bu sırayı doğru yakaladı.**
- **#148 Class Weapon Keystone Karar #146 candidate ile birleştirilmeli:** Weapon Component Swap (600 combo runtime) önerisi **#148'in geliştirilmiş hali** — Karar #146'yı kaldırıp #148 altına merge etmek gerekir. Aynı şey, farklı isim.
- **Skin Pilot (#155) Sprint 17 doğru zaman:** Önce 10 canonical lock, sonra alt skin. (4,4) Brawler Female + (4,2) Frost Elementalist 3 pilot (üçüncüsü Brawler Monk alt) en ekonomik diversity ekleme.
- **Hub NPC (#156) Sprint 18 + Cinematic Layer V1:** Boss kill sonrası one-line progression — meta-progression backbone'a temel.

---

## §5. Birleşik Sprint Planı (FINAL — Brush V1 sonrası)

### Sprint 14 — Combat Integration + Readability Foundations (P0)
| İş | Tier | Çıktı |
|---|---|---|
| **Player skill system** (4 active type + 3 passive) | P0 | STRIKE/ZONE/REACTIVE/STATE + KEYSTONE/MODIFIER/RESONANCE LIVE |
| **#152** Cursor Active Camera (per-class config) | P0 | CameraFollow.cs extension |
| **#153** UI Clutter Control foundations | P0 | UIManager threat-point hook |
| **Image #14 Faz A canonical accept (4 sınıf)** | P0 | Ravager / Brawler / Ranger / Summoner PixelLab Character States üretim |

### Sprint 15 — Elite Combat + Drift Fix (P1)
| İş | Tier | Çıktı |
|---|---|---|
| **#149** Elite Affix Tooltip v1 (6 affix capped at 1/elite) | P1 | HUD tooltip + 6 affix behavior hook |
| **#151** Loadout v1 (room-entrance preset swap) | P1 | HUD v2 + 2 preset slot |
| **Image #14 Faz B conditional regen (4 sınıf)** | P1 | Warblade armor + Ronin topknot + Hexer curse + Shadowblade purple |

### Sprint 16 — Build Depth + Telegraph (P1)
| İş | Tier | Çıktı |
|---|---|---|
| **#147** Per-Skill Mastery Tree (3 tier × 2-3 choice) | P1 | Data + UI + skill mastery progression |
| **#154** Telegraph Contract | P1 | Per-skill: radius + lead time + pulse + color + outline + opacity |
| **Image #14 Faz C tam regen (Elementalist + Gunslinger)** | P1 | 2 sınıf canonical lock |

### Sprint 17 — Mastery Hardening + Skin Pilot (P1)
| İş | Tier | Çıktı |
|---|---|---|
| Mastery balance + Loadout v1 hardening | P1 | Playtest cycle |
| **#155** Skin Pilot Sprint (3 skin) | P1 | Brawler Female (4,4) + Frost Elementalist (4,2) + Brawler Monk (4,1) |

### Sprint 18 — Weapon Keystone Pilot + Hub NPC (P2)
| İş | Tier | Çıktı |
|---|---|---|
| **#148** Pilot 2 weapon keystone | P2 | Warblade Bloodseeker Axe + Hexer Bone Covenant Scepter (1 bound skill each) |
| **#156** Hub NPC Sprint | P2 | 3 NPC (Vendor + Mentor + Lore-Keeper) + Cinematic Layer V1 entegrasyonu |

### Phase 2 — Endgame + Full Keystone
| İş | Tier | Çıktı |
|---|---|---|
| **#148** Full 10 weapon-bound skill (Codex'in 1/sınıf tablosu) | P2 | Tüm sınıfların signature keystone'u + mastery branch modifier'ları |
| **#150** Curse Scaling (3-5 kademe) | P2 | Hades-style endgame difficulty |
| **#155** Skin Batch 2 (4 variant) | P2 | Ravager Female + Ranger Alt + Warblade Beard Repair + Hexer Hood Alt |
| Rift Break tier unlock (Phase 4-5 design) | P2 | Meta progression |

---

## §6. Karar #146 + #148 Birleştirme (Cleanup)

**Mevcut karmaşa:**
- Karar #146 candidate (BACKLOG): Weapon Component Swap — 10 sınıf × 5 weapon × 3 component × 4 option = 600 combo
- Yeni Karar #148: Class Weapon Keystone — 10 weapon-bound signature skill

**Opus karar:** İki aday **aynı sorunu** çözüyor — birleştir.
- **Karar #146 DELETED** (BACKLOG'tan çıkar)
- **Karar #148 EXPAND:** "Class Weapon Keystone + Modular Component" — keystone weapon body + 3 component slot × 2 option (component = skill modifier proxy)
- Effective combo: 10 weapon × 2³ component combinations = **80 build varyasyonu** (Karar #146'nın 600'üne karşı **gerçekçi scope**)

---

## §7. Yasaklananlar (LOCK — değişmez)

- ❌ **11. class** (4,2 Frost Mage veya 4,4 Assassin)
- ❌ **Path C skill genişleme** (20-30 always-available skill)
- ❌ **Slormancer kamera açısı** (~40-50°) — Karar #100 (30-35°) LIVE LOCK
- ❌ **Persistent ARPG progression** — RIMA roguelite genre
- ❌ **120 unique weapon** Slorm Reaper kopyası — solo-dev sprite scope death
- ❌ **10 gear slot** Slormbuilds itemization — Karar (4 equip slot) ihlali
- ❌ **Procgen environment 40 affix** — Karar #143-E editor-authored RoomBank
- ❌ **Wrath 10+100 infinite scaling** — Faz 1 scope death
- ❌ **3+ Hub NPC v1** — writing/naming/animation/localization yükü
- ❌ **Skin pilot >3 batch 1** — QC overload
- ❌ **Frost Elementalist için yeni skill ağacı** — cosmetic skin language only

---

## §8. User Aksiyon Listesi

| # | Aksiyon | Öncelik |
|---|---|---|
| 1 | Bu planı (§1-§7) onayla veya değiştir | P0 — şimdi |
| 2 | rima-doc dispatch → MASTER_KARAR_BELGESI.md'ye #147-#156 ekle (#146 deleted, #148 expanded) | P0 — onay sonrası |
| 3 | rima-asset dispatch → STAGING/character_production_prompts.md → Sprint 14 Faz A canonical accept (4 sınıf state variant prompts) | P1 — Sprint 14 başında |
| 4 | rima-asset dispatch → Sprint 15 Faz B regen prompts (4 sınıf conditional fix) | P1 — Sprint 15 |
| 5 | Sprint 17 Skin Pilot için (4,4) Brawler Female anchor üretimi | P1 — Sprint 17 |
| 6 | Hub NPC Sprint 18 için 3 NPC concept (Cinematic Layer V1 ile) | P2 — Sprint 18 |
| 7 | Karar #146 → #148 merge planı için rima-design RFC | P2 |

---

## §9. Sonuç

RIMA'nın **mevcut canonical 10-class kimliği LOCK kalır**. Image #14'ten 4 canonical accept, 4 conditional regen, 2 sınıf yeniden üretim ile **10 sınıf production-ready**. (4,4) Brawler Female ve (4,2) Frost Elementalist **skin variant** olarak Sprint 17 pilot'a girer. Hub NPC'leri Sprint 18'de Cinematic Layer V1 ile entegre olur.

10 yeni karar (#147-#156) **kademeli 5-7 sprint** sürede ship edilir. Sprint 14 readability foundations (#152 + #153) + 4 canonical accept ile başlar.

**Yasaklar net:** 11. class YOK, Slormancer kamera açısı al-ınmaz, scope explosion red flagged.

**Bir sonraki adım:** User onayı → MASTER_KARAR_BELGESI.md update + Sprint 14 spec yazımı.
