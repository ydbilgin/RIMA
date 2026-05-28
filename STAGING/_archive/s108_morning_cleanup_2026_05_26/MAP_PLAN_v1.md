# RIMA Map Plan v1 (Opus draft — 2026-05-18 S91)

**Status:** DRAFT — pending Codex review + Opus final
**Goal:** "Map plan netleşsin ki ne üreteceğimizi bilelim" — user direktifi 2026-05-18
**Source basis:** NLM canonical (30ddffa5-...), v15h LIVE state, 5000 PixelLab allocation LOCK, blueprint-first + layered terrain memory LOCK'ları, Phase 1.5 RoomData spec DRAFT (570 satır)

---

## 1. Vizyon (Hades+STS hibrit)

RIMA dünyası **açık dünya değil, Diablo-tarzı dağınık dungeon değil**. Şu hibrit:

- **Hades benzeri kapalı arena akışı** → her oda kendi içinde combat encounter veya event
- **Slay the Spire benzeri makro graph** → odalar arası seçim, branching, "git/dur" stratejisi
- **3-fork visibility** → harita başlangıçta tamamen kapalı, Map Fragment'lar topladıkça önündeki 1-2 oda ikon olarak görünür

Tema: **"Fractured Epic"** + **"Vivid Vulnerability"** — yıkık ihtişam, parçalanmış geometri, biome-spesifik palet.

---

## 2. MVP / Phase 1 Scope (LOCK)

**Hedef:** Tek class (Warblade) ile Act 1 başından sonuna **10 dk kesintisiz loop**.

| Item | MVP value |
|---|---|
| Biome | 1 (Shattered Ruins / Sunken Keep) |
| Oda tipi (aktif) | 5 (Combat, Elite, Unknown, Shop[opsiyonel], Boss) |
| Oda sayısı / koşu | 8-9 (NLM: 5-6 Combat + 1 Elite + 0-1 Shop + 1-2 Unknown + 1 Boss) |
| Boss | Penitent Sovereign Faz 1 sadece |
| Class | Warblade only |
| Run süresi | ~10 dk |

**Phase 2+ ekler:** Forge, Curse Gate, Event (10 statik event), Spirit Encounter, Rest dedicated node, Act 2/3 biome.

---

## 3. Oda Tipleri Roster (Tam game — 9 tip)

| Tip | İkon | MVP? | Mekanik özet | Drop / Reward |
|---|---|---|---|---|
| **Combat** | ⚔️ | ✓ | Standart düşman dalgası, kilit, temizle → skill draft | Map Fragment + 3-pick skill |
| **Elite** | 💀 | ✓ | Affix'li elite mob (HP×2.5 + 1 affix), garantili Rare+ ödül | %12 Max HP + Rare+ skill + %25 Relic |
| **Boss** | 👑 | ✓ (F1) | Act sonu, Fracture Echo | %50 Max HP + Relic + Class Legendary |
| **Unknown** | ❓ | ✓ | İkon yok, rastgele içerik | %25 combat / %15 mini-boss / %15 gizli shop / %15 spirit / vs |
| **Shop** | 🛒 | ✓ opsiyonel | Shards harca: HP/Max HP/Skill Tier/Relic | — |
| **Forge** | 🔨 | Phase 2 | 3 tab: Ecol / Skill / Item combine, visit başına 1 işlem | — |
| **Curse Gate** | 🌀 | Phase 2 | %15 Max HP feda → Legendary kestirme + curse riski | %60 legendary + curse debuff |
| **Event** | 📜 | Phase 2 | 10 statik ikili-seçim event | Değişken (skill / HP / lore / shards) |
| **Spirit Encounter** | 👁️ | Phase 3 (Act 2+) | Koşullu build bonus teklifi | Build bonus + lore |

---

## 4. Biome Roster (Tam game — 4 act)

| Act | Biome | Palet | Tema | MVP? |
|---|---|---|---|---|
| **Act 1 (F1)** | Shattered Ruins / Sunken Keep | Soğuk gri/granit + buz mavisi + cyan rift | Parçalanmış kale | ✓ |
| **Act 2 (F2)** | Bleeding Wastes | Derin mor + kızıl altın | Kanayan/yozlaşmış orman+kemiklik | Phase 2 |
| **Act 3 (F3)** | Core Approach | Void siyah + akkor altın | Gerçeklik incelmesi, kozmik boşluk | Phase 3 |
| **Final** | Nexus Core | Saf beyaz/siyah + class VFX | Nexus | Phase 3 |

**Şu an v15h LIVE composition** = Act 1 Shattered Ruins'a denk (cobble + dirt + stone path). Hedef MVP biome zaten v15h üzerinden test ediliyor.

---

## 5. Map Generation Philosophy

### 5a. Branching
- **Node 1-3:** Lineer (tutorial — sistem ve combat ritmi öğretme)
- **Node 4+:** 3-4 fork dallanır
- **Fork prob (eski memory map_system):** Fork 1 = %40 W (wider), Fork 3 = %25 W
- **Act sonuna doğru:** convergence → boss tek hat

### 5b. Topology (Act 1 — LOCK from Karar #62)
**Toplam 15 node:**
- 1 entry
- 6 combat
- 2 elite
- 2 rest
- 1 shop
- 1 curse gate
- 1 mystery
- 1 boss

**Toplam game:** Act 1 + Act 2 + Act 3 + Final = 31-37 oda, 55-70 dk koşu.

### 5c. Visibility (Map Fragment system)
- Başlangıçta tamamen kapalı
- Map Fragment topla → önündeki 1-2 node sadece **ikon** görünür (içerik gizli)
- Smart spawn: blind 100%, 1-step %50, 2+ step %10
- M key toggle (DungeonMapUI.cs)

### 5d. Scaling — Threat Points
| Act | Combat budget | Elite budget |
|---|---|---|
| Act 1 | 8-12 pt | 14-18 pt |
| Act 2 | 14-20 pt | 22-28 pt |
| Act 3 | 20-28 pt | 30-40 pt |

Mob fiyatları: swarm 1pt, grunt 2pt, special 4pt → procedural wave spawn budget'a göre.

---

## 6. Room Sizes & Grid (Kanonik LOCK)

Tile = 32×32, PPU = 64.

| Tip | Grid | Oynanır alan | Notlar |
|---|---|---|---|
| Corridor (Linear) | 10×24 | 6×20 | Dar geçit |
| Corridor (LShape) | 24×18 | 18×12 | L-dönüş koridor |
| Combat Small | 24×18 | 18×12 | Standart |
| Combat Medium | 24×18 | 18×12 | Standart varyant |
| Combat Large | 28×20 | 22×14 | Geniş arena |
| Elite Arena | 28×22 | 22×16 | Affix mob alanı |
| Boss Arena | 40×30 | 34×22 | Mekanik alanı |
| Rest / Shop | 16×12 veya 20×16 | tüm grid | Combat'sız yan oda |
| Shrine | 16×12 | 12×8 | Küçük altar odası |
| Treasure | 16×12 veya 20×16 | tüm grid | Sandık etrafı |

**Mevcut v15h** = 36×22 = Combat Large ile Elite arası → **eşleştirilebilir**. Composition pipeline'ı tek bir grid'e değil, room-template grid'lerine bağlamak gerekiyor.

---

## 7. Room Library — Hibrit Procedural + Polish

**Yaklaşım (memory + NLM):**
- **Procedural Generator** → makro zone + RoomData blueprint
- **Polish Editor** → designer'ın manuel tweaks
- **Source-of-Truth** → RoomTemplateSO ScriptableObject

### 7a. Library/ klasörü mevcut state (LIVE)
`Assets/Data/Rooms/Library/` — 10 RoomTemplateSO:
1. `Spawn_01.asset` — entry
2. `Combat_Small_01.asset`
3. `Combat_Medium_01.asset`
4. `Combat_Large_01.asset`
5. `Corridor_Linear_01.asset`
6. `Corridor_LShape_01.asset`
7. `Elite_01.asset`
8. `Boss_Intro_01.asset`
9. `Shrine_01.asset`
10. `Treasure_01.asset`

**Eksik MVP için:** Shop, Unknown, Boss arena (Boss_Intro var, asıl arena yok). Combat variant sayısı az (variety < 3 per size).

### 7b. MVP Library expansion target
| Tip | Mevcut | Target MVP | Eksik |
|---|---:|---:|---|
| Spawn | 1 | 1 | — |
| Combat_Small | 1 | 3 | +2 |
| Combat_Medium | 1 | 3 | +2 |
| Combat_Large | 1 | 2 | +1 |
| Corridor_Linear | 1 | 2 | +1 |
| Corridor_LShape | 1 | 1 | — |
| Elite | 1 | 2 | +1 |
| Boss_Intro | 1 | 1 | — |
| Boss_Arena | 0 | 1 | +1 |
| Shop | 0 | 1 | +1 |
| Unknown | 0 | 1 | +1 |
| Shrine | 1 | 1 | — |
| Treasure | 1 | 1 | — |
| **TOTAL** | **10** | **20** | **+10 yeni RoomTemplate** |

---

## 8. v15h LIVE Status + Connection

**v15h composition pipeline** şu an PROCEDURAL paint-and-populate yapıyor:
- Zone blueprint (path/grass/stone/wall/water/feature) — semantic paint
- AutoPopulator → rule-based prop placement
- Adjacency rules → transition decals
- v15d composition budget (20% neg space + 70/20/10 floor + 3 cluster cap)

**v15h metrics (LIVE):**
- Cells 375, FloorFillCoverage 0.848 ✓
- L1=375 (base floor full), L2=318 (decoration), L3=47 (props)
- Warblade spawned + movement ✓
- Wang tiles 6/16 (RuleTile yarım — fix gerekli)
- L5-L8 atmospheric = 0 (legacy unbounded cap, henüz wire değil)

**v15h ≠ RoomTemplate.** v15h pipeline = composition test. RoomTemplate = stored layout.
**Köprü gerekli:** v15h pipeline çıktısı → RoomTemplateSO snapshot olarak Library'ye yazılabilmeli.

---

## 9. Phase 1.5 RoomData Spec Entegrasyonu

**Mevcut DRAFT:** `STAGING/PHASE_1_5_ROOMDATA_SPEC_DRAFT.md` (570 satır)
- 4 SO: RoomData, BiomePalette, PropDef, AdjacencyRule
- 4 brush: Tile / Decal / Prop / Encounter
- Chunked renderer (no GameObject-per-decal)
- Ogmo layer model (8 layer infrastructure)
- Migration plan: v15h composition → RoomData snapshot
- **5 open question — BLOCKER:**
  1. ?
  2. ?
  3. ?
  4. ?
  5. ?

→ Bu 5 soruyu Opus resolve etmesi gerekiyor. Map Plan final'inden önce DRAFT açılmalı.

---

## 10. Asset Production Roadmap (5000 PixelLab + Codex hybrid)

### W1 (şu an + 1 hafta)
| Workstream | İçerik |
|---|---|
| **Codex tile/wall** | Shattered Ruins biome — floor variants (stone/cobble/dirt), wall set, path transition, basic decal |
| **PixelLab character** | Warblade state tweaks (existing 2656075d) + Elementalist anchor + Gunslinger anchor |
| **Codex hero prop** | 3 cluster template (combat focal/support/blocker) |
| **Unity** | v15h fix (RuleTile + Warblade overlap), RoomTemplate save-from-scene tool |

### W2-3
| Workstream | İçerik |
|---|---|
| **PixelLab mob** | 4-6 mob roster (imp/grunt/special tipleri) |
| **PixelLab char** | 7 anchor batch (10 anchor LOCK olunca) |
| **VFX** | A/B dash trail + hitspark (PixelLab vs autosprite vs Codex) |
| **Library** | +10 RoomTemplate variant (Combat × 5, Boss arena, Shop, Unknown, vs) |

### W4+
| Workstream | İçerik |
|---|---|
| **Skill VFX** | 3-5 class skill VFX per class |
| **HUD/UI** | Skill ikon set + portrait + HP/Rage bar |
| **Hazard** | Rift crack, cursed pool, spike |
| **DungeonMapUI** | Map graph UI implementation (eğer hâlâ yoksa) |

**5000 budget'tan v15h+RoomTemplate workstream'i için tahmini ~800 gen:**
- Tile/wall biome (Codex 0 PixelLab)
- Hero prop 100 (PixelLab)
- Char Warblade state tweaks 50 (PixelLab)
- 2 anchor 200 (PixelLab)
- Mob 4-6 roster 300 (PixelLab)
- VFX dash+hitspark 100 (PixelLab)
- Buffer 50

---

## 11. Open Decisions (Opus final için)

1. **MVP Library count:** 10 mevcut + 10 yeni = 20 mu, yoksa NLM "8-9 oda" hedefine sıkışıp az variation mı?
2. **v15h ↔ RoomTemplate köprüsü:** v15h composition snapshot → asset olarak save edilebilir mi, yoksa RoomTemplate bağımsız mı tasarlanacak?
3. **Phase 1.5 RoomData spec — 5 open question:** Opus ne zaman resolve edecek? Map Plan final'inden önce mi sonra mı?
4. **DungeonMapUI implementation:** STS-style map UI hâlâ var mı, yok mu? (memory'de 19 günlük spec var)
5. **MVP biome scope:** Shattered Ruins tam mı, yoksa Sunken Keep alt-variant mı? (NLM her ikisini de Act 1 diyor)
6. **Boss arena production:** Boss_Intro_01 var, asıl Boss arena spawn nereden?
7. **Threat budget enemy roster:** 8 mob roster mevcut/eksik durumu nedir? (mob production öncelik)
8. **Composition pipeline canonical:** v15h auto-populate primary mi, yoksa designer-handcrafted RoomTemplate primary mi?

---

## 12. Önerilen Sıra (Phase 1 ship için)

| Adım | İçerik | Sahibi | ETA |
|---|---|---|---|
| 1 | Bu plan'ı Codex review (scope realism, asset budget, gap) | Codex | 30 dk |
| 2 | Opus final + 8 open decision karara bağla | Opus | 1 saat |
| 3 | v15h fix (RuleTile + overlap) | Sonnet+UnityMCP | 1 saat |
| 4 | Library +10 RoomTemplate expansion (handcrafted veya v15h snapshot) | Sonnet/designer | 1-2 hafta |
| 5 | DungeonMapUI implement/audit (eğer eksikse) | Sonnet | 2-3 gün |
| 6 | Mob roster production (Imp→Hulk) | PixelLab + Sonnet wiring | 1 hafta |
| 7 | Phase 1.5 RoomData chunked renderer impl | Codex (review) + Sonnet (impl) | 1-2 hafta |
| 8 | Playable Act 1 (8-9 oda akışı + Boss F1) end-to-end | Tüm | Phase 1 ship |

---

**End of MAP_PLAN_v1 (Opus draft).** Codex review için bekliyor.
