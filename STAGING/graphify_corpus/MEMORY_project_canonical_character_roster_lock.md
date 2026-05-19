---
name: canonical-character-roster-lock
description: "RIMA 10-class CANONICAL ANCHOR LOCK. Image #16 v11 batch slot → class → PixelLab character ID. Karar #145 anchor source."
metadata: 
  node_type: memory
  type: project
  originSessionId: 20cf7214-b515-4cce-814f-df3b0dd176f2
---

# RIMA Canonical Character Roster — LOCK (Image #16 v11 batch)

**LOCK tarihi:** 2026-05-18 S87
**Kaynak:** Image #16 (v11 prompt batch, drift-fix sonrası)
**Slot dosyaları:** `STAGING/image16_split/`
**v11 prompt:** `STAGING/character_production_prompts.md`
**v10 archive:** `STAGING/_archive/character_production_prompts_v10.md`

---

## CANONICAL ANCHOR MAPPING

| # | Sınıf | Gender | Image #16 Slot | Slot dosyası | PixelLab Character ID | Lock Durumu |
|---|---|---|---|---|---|---|
| 1 | Warblade | M | (1,1) | `r1c1_warblade_v11.png` | TBD (user yükleyecek) | ⏳ Bekliyor |
| 2 | Elementalist | F | (1,2) | `r1c2_elementalist_v11.png` | TBD | ⏳ Bekliyor |
| 3 | Ronin | M | (1,3) | `r1c3_ronin_v11.png` | TBD | ⏳ Bekliyor |
| 4 | Ravager | M | (1,4) | `r1c4_ravager_v11_CANONICAL.png` | TBD (yeni create character ile gelecek) | ⏳ Bekliyor |
| 5 | Hexer | F | (2,1) | `r2c1_hexer_v11.png` | TBD | ⏳ Bekliyor |
| 6 | Brawler | M | (2,2) | `r2c2_brawler_v11_CANONICAL.png` | TBD | ⏳ Bekliyor |
| 7 | Shadowblade | M | (2,3) | `r2c3_shadowblade_v11.png` | TBD | ⏳ Bekliyor |
| 8 | Ranger | F | (2,4) | `r2c4_ranger_v11_CANONICAL.png` | TBD | ⏳ Bekliyor |
| 9 | Summoner | F | (3,1) | `r3c1_summoner_v11.png` | TBD | ⏳ Bekliyor |
| 10 | **Gunslinger** | F | (4,4) | `r4c4_gunslinger_v11_USER_FIX.png` | TBD | ⏳ Bekliyor (USER FIX: dark hair) |

**Gender 5M/5F LOCK doğrulandı:**
- Male (5): Warblade, Ronin, Ravager, Brawler, Shadowblade
- Female (5): Elementalist, Hexer, Ranger, Summoner, Gunslinger

---

## SPARE / VARIANT / NPC SLOTS

| Slot | Slot dosyası | Önerilen kullanım |
|---|---|---|
| (3,2) | `r3c2_variant_or_npc.png` | Phase 2 skin batch veya NPC (user inceleyecek) |
| (3,3) | `r3c3_variant_or_npc.png` | Phase 2 skin batch veya NPC |
| (3,4) | `r3c4_variant_or_npc.png` | Phase 2 skin batch veya NPC |
| (4,1) | `r4c1_variant_or_npc.png` | Sprint 18 Hub Mentor NPC adayı |
| (4,2) | `r4c2_variant_or_npc.png` | Sprint 17 Skin Pilot (Frost Elementalist veya Ranger Alt) |
| (4,3) | `r4c3_variant_or_npc.png` | Sprint 18 Hub Mentor NPC adayı |

---

## USE #6 — Conditional Variant Application (Karar #145 expanded)

**Proven workflow (2026-05-18 — concept lock, eski test ID'leri deprecate):**
- Anchor: mevcut canonical class anchor (PixelLab create character)
- State prompt: natural language manipülasyon ("make him female", "add facial scar", "change palette to frost")
- Result: aynı identity preserved + state-level modification

**Scope (broad):**
- Gender swap ✅ proven
- Specific accessory ("add bone necklace", "remove cape")
- Specific outfit element ("change boots to greaves")
- Wound/condition ("add facial scar across left cheek")
- Age variation
- Ethnicity / skin variation
- Outfit palette change
- Body type adjustment
- Pose subtle shift
- Hair variation

**~~⚠️ HIGH GEN COST~~ REVOKED 2026-05-18:** 5000 gen kredi 2026-05-19'da geliyor. Anchor lock sonrası state workflow **serbestçe** kullanılabilir.

**RIMA implications (gen budget rahat):**
- **Sprint 14-15 Animation Production:** 10 sınıf full state set (idle/run/attack/hit/death/parry × 5 dir = ~300 gen) — budget'ın 6%'si
- **Sprint 17 Skin Pilot:** 3-4 variant full state set (~120 gen) — budget'ın 2.5%'i
- **Phase 2 diversity:** her sınıf 1-2 alternate variant (Use #6) = 10-20 gen
- **Boss multi-phase:** "make him enraged", "make him cracked", "make him final form" — 4 phase × 4 boss = ~50 gen
- **Mob variant matrix:** 8 mob × 3-4 variant = ~50 gen
- **TOPLAM hedef kullanım:** ~530 gen = 10% of 5000 budget

---

## NEXT STEPS (user action required)

| # | Aksiyon | Owner |
|---|---|---|
| 1 | Image #16 slot'larını inceleyip 10 canonical adaylarını **PASS/REGEN** olarak işaretle | USER |
| 2 | PASS olan slot'ları PixelLab'a yükle → her birinin character ID'sini al → bu memory'de TBD'lerin yerine yaz | USER |
| 3 | REGEN gereken slot'lar için v12 prompt tweak yaz (drift-spesifik) | Opus + USER |
| 4 | LOCK tamamlanınca: rima-doc dispatch → MASTER_KARAR_BELGESI.md'ye **Canonical Roster LOCK** entry | rima-doc |
| 5 | Karar #145 v2 (6 use case): `MASTER_KARAR_BELGESI.md` update | rima-doc |

---

## CROSS-LINKS
- [[pixellab-character-states-workflow]] — Karar #145 expanded (6 use case)
- [[weaponless-animation-v1]] — Karar #144 silahsız body
- [[class-colors]] — palette canonical
- [[character-visual-identity]] — saç/zırh canonical
- [[class-genders]] — 5M/5F lock
