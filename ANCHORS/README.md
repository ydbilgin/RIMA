# RIMA ANCHORS — Canonical Sprite Sources

**Bu klasör, PixelLab Character States workflow (Karar #145 v2) için anchor sprite kaynaklarını barındırır.**

**Bu sprite'lar PixelLab'a yüklenecek → "create character" → her birinin character ID'si alınacak → state workflow ile animation + variant üretim başlayacak.**

---

## Klasör Yapısı

```
ANCHORS/
├── README.md                 (bu dosya)
├── characters/               (10 oynanabilir sınıf anchor'ı — LIVE)
├── mobs/                     (Tier 1-2 düşman anchor'ları — Sprint 14+ üretim)
└── elites/                   (Tier 3 elite/boss anchor'ları — Phase 1.5+ üretim)
```

---

## characters/ — 10 Canonical Class Anchors (LIVE)

**Kaynak:** Image #16 v11 batch (drift-fix prompt sonrası), `STAGING/image16_split/`

| # | Dosya | Sınıf | Gender | Image #16 Slot |
|---|---|---|---|---|
| 01 | `01_warblade.png` | Warblade | M | (1,1) |
| 02 | `02_elementalist.png` | Elementalist | F | (1,2) |
| 03 | `03_ronin.png` | Ronin | M | (1,3) |
| 04 | `04_ravager.png` | Ravager | M | (1,4) |
| 05 | `05_hexer.png` | Hexer | F | (2,1) |
| 06 | `06_brawler.png` | Brawler | M | (2,2) |
| 07 | `07_shadowblade.png` | Shadowblade | M | (2,3) |
| 08 | `08_ranger.png` | Ranger | F | (2,4) |
| 09 | `09_summoner.png` | Summoner | F | (3,1) |
| 10 | `10_gunslinger.png` | Gunslinger | F | (4,4) [USER FIX: dark hair] |

**5M / 5F gender LOCK doğrulandı.**

---

## mobs/ — Tier 1-2 Mob Anchors (BEKLİYOR)

**Kaynak:** Sprint 14+ üretim — yeni prompt sheet `STAGING/mob_production_prompts_v1.md` (yazılacak)

Hedef Tier 1 (64×64) öncelik:
- Fracture Imp (mor demon)
- Rift Hound (küçük yırtık kurdu)
- Seam Crawler (cyan spider)
- Rift Acolyte (büyücü)

Hedef Tier 2 (96×96) sonra:
- Penitent Bruiser (kara hooded melee)
- Riftbound Augur (caster)
- Spire Choirling (ghost flying)
- Shard Walker (crystal hum)

⚠️ **Legacy 12 mob `Mobs/_archive_legacy_S86/` altında** — front-view / yanlış açı yüzünden REGEN gerekti.

---

## elites/ — Tier 3 Elite/Boss Anchors (PHASE 1.5+)

Hedef:
- Hollow Arbiter (crowned king — elite/mid-boss)
- Hollow Hulk (stone golem — elite)
- Plate Widow (boss spider)
- Relic Caster (book caster — elite)
- Final Boss: Architect (yeni concept)
- Tier 1 Boss: Penitent Sovereign (yeni concept)

---

## ANCHOR'LARDAN ÜRETİLECEKLER (Karar #145 v2 — 6 use case)

| Use # | Açıklama | Hedef |
|---|---|---|
| #1 | Animation states (idle/run/attack/hit/death × 5 dir) | Her anchor için ~26-31 gen, 10 char = ~300 gen |
| #2 | Enemy variant matrix ("armored", "elite captain") | Mob bazı: 8 mob × 3-4 variant = ~50 gen |
| #3 | Boss multi-phase ("enraged", "cracked", "final form") | 4 boss × 4 phase = ~50 gen |
| #4 | Class skin variants (Rift Break meta tier) | 10 sınıf × 2-3 tier = ~120 gen |
| #5 | State-to-state interpolation | Death/transform sequence — Phase 2 |
| #6 | Conditional variant (natural language) | Gender swap, scar, accessory, palette — broad use |

**5000 gen budget (2026-05-19)** → tüm 6 use case rahat üretim.

---

## NEXT STEP (USER)

1. **Inspect** `ANCHORS/characters/01-10_*.png` — visual sanity check
2. **PASS / REGEN** karar (per anchor)
3. **PASS olanlar:** PixelLab Web UI → "create character" → her birinin character ID'si al
4. **ID'leri** `memory/project_canonical_character_roster_lock.md` TBD'lerin yerine yaz (veya Opus'a ilet)
5. **REGEN gerekenler:** drift-spesifik v12 prompt tweak

**Lock tamamlanınca:** rima-doc dispatch → MASTER_KARAR_BELGESI.md "Canonical Roster LOCK" entry + Karar #145 v2 (6 use case) lock.
