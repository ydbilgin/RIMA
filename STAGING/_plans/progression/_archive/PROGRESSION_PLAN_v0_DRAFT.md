# RIMA Progression Plan v0 — Draft for Codex Review

**Hedef:** Map fragment progression + reward + room type sistemi için bütünsel plan. Mevcut compact sheet asset'leri kullanarak. Yeni gen YOK.

**Status:** v0 DRAFT — Codex feedback'i sonrası v1, sonra orchestrator karar.

---

## 1. Mevcut Görsel Envanter (compact sheets, hazır)

| Sheet | Path | İçerik |
|---|---|---|
| **#1 Threshold lineup** | `STAGING/concepts/compact_sheets/01_threshold_lineup.png` | 8 threshold konsept yan yana (C1 Scar Compass, C2 Loom, C3 Slabs, C4 Rib, A1 Anchor, A2 Scar, A3 Mirror, A4 Seam) |
| **#2 Hades reward doors** | `STAGING/concepts/compact_sheets/02_hades_style_reward_doors.png` | 2 threshold (Scar Compass + Imprint Scar) × 6 reward icon (Essence/Gold/Shard/Rune/Health/Boss Key) |
| **#3 Reward drops** | `STAGING/concepts/compact_sheets/03_reward_drops_gallery.png` | 8 reward drop (Echo Orb/Memory Shard/Gold Pile/Skill Rune/Health Orb/Map Fragment/Curse Stone/Boss Key) |
| **#4 Map progression marks** | `STAGING/concepts/compact_sheets/04_map_progression_marks.png` | 6 progression mark (Thread Weave/Scar Path/Fragment Map/Rune Trail/Tapestry/Compass Sweep) |
| **#5 Rift Threshold (Codex orig)** | `STAGING/concepts/rift_threshold_*_act1.png` | 4 state (locked/active/portal/final) — Hades framed door style (DEPRECATED, Hades clone risk) |
| **#6 Scar Compass Ring showcase** | `STAGING/concepts/threshold_gallery/C1_scar_compass_ring/showcase.png` | Codex top tavsiye, isolated render |

---

## 2. Önerilen Sistem (v0)

### 2a. Threshold (oda-arası geçit) Karar

**Birincil seçenek:** **A2 Imprint Scar / Floor Rift** (Antigravity FINAL TAVSİYE, skor 96/100)
- Zemin yarığı, kemer YOK
- 2:1 iso grid'e doğrudan oturur
- Player rift'e "düşer" hissi
- 35° iso projection optimal

**İkincil seçenek:** **C1 Scar Compass Ring** (Codex top, skor 19/20)
- Floor compass + scar needle
- Direction-explicit (yön diegetic)
- 8-dir natural

**Karar gate (Codex review):** A2 vs C1, hangisi RIMA için?

### 2b. Room Type Mapping (9 tip → threshold variant)

Antigravity'nin "Universal Dynamic Cyan Overlay Shader" önerisi → 1 base sprite × shader-driven color = 9 variant. Production %90 tasarruf.

| Room Type | Threshold visual variant | Reward icon (door) | Floor drop |
|---|---|---|---|
| **Combat** | Standard cyan rift | Echo Essence orb | Essence + Gold (small) |
| **Elite** | Heavier knot/sigil + larger arc | Memory Shard | Shard + Gold (med) |
| **Boss** | Sealed mass + sigil | Boss Key | Boss-specific |
| **Chest** | Gold-flecked rift | Gold Pile | Gold (large) + Shard |
| **Merchant** | Trade tag accent | (no icon, friendly) | (NPC interaction) |
| **Forge** | Ember-orange secondary | Skill Rune | Rune + craft material |
| **Event** | Asymmetric pale | (question mark) | Variable |
| **Curse** | Frayed red-black | Curse Stone (warning) | Risk reward |
| **Corridor** | Minimal faint | (none, transit) | (no drop) |

### 2c. Reward Drop Canonical (Sheet 3)

| Drop | Mantık | Pickup |
|---|---|---|
| **Echo Essence Orb** | Currency/Energy resource — skill cost veya XP | Combat reward |
| **Memory Shard** | Permanent meta upgrade currency (Hades darkness benzeri) | Elite/Boss reward |
| **Gold Pile** | Standard currency (Merchant trade) | Combat/Chest |
| **Skill Rune** | Skill upgrade pickup — equipped olduğunda skill modify eder | Forge/Elite |
| **Health Orb** | Instant heal | Combat/Curse risk reward |
| **Map Fragment** | Progression token — collect N → reveal full map | Every room (low drop rate) veya specific room types |
| **Curse Stone** | High-risk modifier — Hades infernal arms benzeri | Curse room only |
| **Boss Key** | Unlock next-floor boss room | Boss room cleared → Key drops |

### 2d. Map Fragment Progression (Sheet 4 — Fragment Map mark seçildi)

**Mekanik:**
1. Her oda clear sonrası **1 Map Fragment** drop (basit, anlaşılır)
2. Player UI'de fragment map progressively assembles (Sheet 4'teki "Fragment Map" 3-frame animation)
3. **5 fragment** → mini-floor map reveal (sonraki 5 odanın connections + reward icons görünür)
4. **Boss room key fragment** ayrı (sadece Boss key drop, fragment count'a girmez)

**Alternatif (Codex feedback için):** 
- Fragment sadece **Elite + Chest + Event** room'lardan drop (rarity)
- Boss room **3 fragment** drop, mini-map complete olur
- Curse room fragment **kayıp** olabilir (risk)

### 2e. Rune Sistemi (Sheet 3 Skill Rune drop)

**v0 öneri:**
- Rune = **passive skill modifier** — equipped olduğunda 1 skill behavior değiştirir
- Drop yeri: **Forge** + **Elite** room
- Slot sayısı: 3 (player'da equipped slot)
- Rune rarity: Common / Rare / Mythic (renk kodlu — beyaz/cyan/purple)

**Codex skill code'tan referans:**
- Warblade IronCounter passive zaten code'da — bunu rune-driven yapabiliriz
- Elementalist ArcaneSurge — rune ile modify
- Ranger ExplosiveTrap — rune ile area artar

**Codex review:** Bu rune sistem var olan skill code'una nasıl bağlanır?

### 2f. Echo Imprint Cascade Integration

**v0:** Reward drop'ları VE map fragment "death imprint" tema ile:
- Each death → 1 fragment LOST (memory fragmentation)
- Each clear → 1 fragment GAIN (memory imprint)
- Map UI = "dungeon memory tapestry" → fragments assemble into directional weave
- Sheet 4 "Tapestry" mark idea = bu konseptin direct visual

**Codex review:** Bu tema reward sistem'le çelişiyor mu yoksa zenginleştiriyor mu?

---

## 3. Açık Sorular (Codex'e Review Sorusu)

1. **Threshold seçim**: A2 Imprint Scar vs C1 Scar Compass — RIMA için hangisi? (Floor rift vs Floor compass)
2. **Map Fragment drop rate**: Her oda mı, sadece bazı oda mı?
3. **Rune sistem**: Slot sayısı 3 mü daha mı, drop rate?
4. **Curse Stone**: High-risk + high-reward modifier doğru mu, yoksa permanent debuff mı?
5. **Echo Imprint integration**: Death = fragment LOSS dengeli mi, yoksa frustrate mı?
6. **Universal shader yaklaşımı**: 1 base sprite × shader-driven color = 9 room variant, gerçekçi mi?
7. **Boss Key vs Map Fragment**: Ayrı tutmak mı doğru yoksa Boss Key = N fragment olarak abstract etmek mi?

---

## 4. Mevcut Memory Cross-Reference

Reading from local memory + CURRENT_STATUS:

- **Path C Hybrid LOCK** (project memory): Codex painted floor + wall pieces + 119 PNG object overlay. Reward/threshold yeni sistem buna eklenir.
- **Karar #149 Sub-Room Encounter LOCK**: Combat/Elite = 3-5 sub-room sequence. Reward sadece encounter-final. **Map fragment her sub-room mı, sadece encounter-final mi?** Codex check.
- **Karar #150 Act 1 Envanter LIVE**: 119 PNG mevcut. Bunlardan hangileri threshold/reward asset?
- **Skill Bank V1 Revision Needed**: 48-skill bank Opus review. 9→7+2 tag reclassification candidate. Rune sistem bu bank'a nasıl bağlanır?

---

## 5. Production Cost (v0 Estimate)

**Bu plan için yeni gen ihtiyacı:**
- ✅ Threshold base sprite: **HAZIR** (Sheet 6 Scar Compass showcase, veya Sheet 1'den seçim)
- ✅ Reward drops: **HAZIR** (Sheet 3, 8 farklı drop)
- ✅ Map fragment + map UI: **HAZIR** (Sheet 4 Fragment Map mark)
- 🟡 Room type variants: **shader implement** (Unity URP 2D shader — Codex task)
- 🟡 Threshold integrated into Painter: **code implement** (RimaWorldPainterWindow)
- ❌ Rune visual icon set: 8-12 rune icon — gen lazım (32px batch, 1 dispatch)
- ❌ Boss key visual: **HAZIR** (Sheet 3'te var)

**Yeni gen tahmini:** 0-1 batch (sadece rune icon set isteğe bağlı)

---

## Codex Review Request

Aşağıdaki sorulara cevap ver + bu plan'a v1 önerisi:

1. **Plan tutarlılık**: Yukarıdaki 6 madde (2a-2f) birbirleriyle çelişiyor mu?
2. **Karar #149 sub-room ile uyum**: Reward sistem sub-room sequence ile bağlı mı?
3. **Skill code integration**: Rune sistemi mevcut Warblade/Elementalist/Ranger/Shadowblade/Ronin skill kodu ile nasıl entegre?
4. **Açık 7 soruya** (yukarıda) öneri ver
5. **Production cost** doğru mu, kayıp gen var mı?
6. **v1 önerisi**: Bu draft'i güncelleyerek v1 yaz, `STAGING/PROGRESSION_PLAN_v1_CODEX.md` olarak kaydet

Stil: terse, decision-oriented, kararsızlık YOK ("X seçilmeli çünkü Y"). Kod yazma — sadece plan refinement.
