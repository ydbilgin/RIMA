---
status: LOCKED_PENDING_PIXELLAB_ID
faz: 1
tarih: 2026-05-18
ozet: "10 sınıf canonical anchor LOCK — Image #16 v11 batch slot mapping"
source: STAGING/image16_split/
karar: #145 v2 (6 use case), #144 weaponless body, #100 chibi 30-35°, #74 64×64
---

# RIMA Canonical Character Anchors — LOCK

**Bu dosya, RIMA 10 sınıfının canonical anchor karakter sprite'larını lockler.**
**Tüm animation state'leri ve variant'lar bu anchor'lardan üretilecek (Karar #145 v2 — 6 use case).**

---

## ANCHOR ROSTER (10 sınıf)

| # | Sınıf | Gender | Anchor Slot | Anchor Sprite Dosyası | PixelLab Character ID | Status |
|---|---|---|---|---|---|---|
| 1 | **Warblade** | M | (1,1) | `STAGING/image16_split/r1c1_warblade_v11.png` | TBD (yeni create character) | ⏳ Lock pending |
| 2 | **Elementalist** | F | (1,2) | `STAGING/image16_split/r1c2_elementalist_v11.png` | TBD | ⏳ Lock pending |
| 3 | **Ronin** | M | (1,3) | `STAGING/image16_split/r1c3_ronin_v11.png` | TBD | ⏳ Lock pending |
| 4 | **Ravager** | M | (1,4) | `STAGING/image16_split/r1c4_ravager_v11_CANONICAL.png` | TBD | ⏳ Lock pending |
| 5 | **Hexer** | F | (2,1) | `STAGING/image16_split/r2c1_hexer_v11.png` | TBD | ⏳ Lock pending |
| 6 | **Brawler** | M | (2,2) | `STAGING/image16_split/r2c2_brawler_v11_CANONICAL.png` | TBD | ⏳ Lock pending |
| 7 | **Shadowblade** | M | (2,3) | `STAGING/image16_split/r2c3_shadowblade_v11.png` | TBD | ⏳ Lock pending |
| 8 | **Ranger** | F | (2,4) | `STAGING/image16_split/r2c4_ranger_v11_CANONICAL.png` | TBD | ⏳ Lock pending |
| 9 | **Summoner** | F | (3,1) | `STAGING/image16_split/r3c1_summoner_v11.png` | TBD | ⏳ Lock pending |
| 10 | **Gunslinger** | F | (4,4) | `STAGING/image16_split/r4c4_gunslinger_v11_USER_FIX.png` | TBD | ⏳ Lock pending (USER FIX: dark hair) |

**5M / 5F LOCK doğrulandı:**
- Male (5): Warblade, Ronin, Ravager, Brawler, Shadowblade
- Female (5): Elementalist, Hexer, Ranger, Summoner, Gunslinger

---

## ANCHOR ÖZELLİKLERİ (Karar LOCK'lar)

| Boyut | Değer | Karar |
|---|---|---|
| Canvas size | 64×64 | #74 |
| Camera angle | High top-down 30-35° | #100 |
| Body convention | Silahsız body + WeaponSR child SR | #144 |
| State workflow | PixelLab Character States | #145 v2 |
| Direction setup | 5 produce (S, SE, E, NE, N) + 3 mirror (SW, W, NW) | — |
| PPU | 64 | #100 |

---

## ANCHOR'DAN ÜRETİLECEKLER (Karar #145 v2 — 6 use case)

### Use #1 — Animation States (PRIMARY)
Her anchor için per-direction state set:
- `idle_{dir}` (4f anchor)
- `midwalk_{dir}` (6f run anchor)
- `attack_anticipation_{dir}` (3-seg)
- `dash_lean_{dir}` (optional 3f)
- `hit_recoil_{dir}` (3f)
- `death_start_{dir}` (6-8f)

Per class: ~26-31 gen, 10 sınıf toplam: ~300 gen

### Use #6 — Conditional Variants (BROAD)
Natural language state prompt ile manipülasyon:
- Gender swap ("make her female")
- Specific accessory ("add cracked helmet")
- Wound/condition ("add facial scar across left cheek")
- Outfit element ("change boots to greaves")
- Age variation
- Ethnicity / palette change

Per class 1-2 variant: 10-20 gen toplam

### Phase 2 — Class Skin Variants (Use #4)
Per class meta-progression tier outfit (Rift Break):
- Tier 1: base outfit (anchor)
- Tier 2-4: variant outfits

Per class 3 variant tier: ~30 gen, 10 sınıf: ~300 gen

---

## ANCHOR LOCK CRITERIA (Per slot inspect)

Her anchor slot için PASS criteria:

### Genel
- [ ] 64×64 canvas, sprite fills ~85-90% vertically
- [ ] 30-35° high top-down camera (NOT side, NOT iso, NOT character portrait)
- [ ] Chibi 3-4 head proportion (big head, short legs)
- [ ] Silahsız body (no weapons in body sprite)
- [ ] Hard pixel edges, no anti-aliasing
- [ ] Face features readable (eyes/mouth/hair distinct pixels)
- [ ] Soft oval ground shadow at feet

### Per-class identity (canonical match)
- [ ] **Warblade:** brown leather + brass buckle + beard
- [ ] **Elementalist:** honey-blonde low bun + dusty indigo crop + cream sash + teal skirt
- [ ] **Ronin:** black hair samurai topknot + dark navy kimono
- [ ] **Ravager:** dark blood-red armor + leather harness + iron studs
- [ ] **Hexer:** dark purple-black robe + hood + dark red hex-rune accent
- [ ] **Brawler:** dark skin + bald + leather wrappings + boxing guard
- [ ] **Shadowblade:** narrow profile + dark purple armor + void purple accent glow
- [ ] **Ranger:** bleached-ivory ponytail + forest green asymmetric armor + cold blue accent
- [ ] **Summoner:** long dark hair + indigo green-black robe + cyan trim + summoning gesture
- [ ] **Gunslinger:** dark hair + brown skin + grey-purple trench + rift bandana (USER FIX: NO red hair)

---

## NEXT STEP (USER)

1. **Inspect** her 10 anchor slot dosyasını (`STAGING/image16_split/r1c1_*` ... `r4c4_*`)
2. **PASS / REGEN** kararını ver (her slot için)
3. **PASS olanlar:** PixelLab Web UI → "create character" → upload slot dosyası → karakter oluştur
4. **PixelLab character ID'lerini al** → bu dosyada TBD'lerin yerine yaz (veya Opus'a ilet)
5. **REGEN gerekenler:** drift-spesifik v12 prompt tweak iste
6. **Lock tamamlanınca:** Karar #145 v2 + Canonical Roster LOCK → MASTER_KARAR_BELGESI.md
