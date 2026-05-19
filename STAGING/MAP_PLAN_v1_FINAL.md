# RIMA Map Plan v1 FINAL (Opus + Codex review synthesis — 2026-05-18 S91)

**Status:** LOCK — Opus final after Codex review (`STAGING/CODEX_REVIEW_MAP_PLAN_DONE.md`)
**Replaces:** `STAGING/MAP_PLAN_v1.md` (draft)
**Why locked now:** "Map plan netleşsin ki ne üreteceğimizi bilelim" — user direktifi 2026-05-18
**Sources:** NLM canonical (30ddffa5-...), v15h LIVE state, Codex evidence-backed review, current code grep (RoomType.cs, DungeonGraph.cs, RoomTemplateSO.cs, LegacyRuntimeRoomManager.cs)

---

## 0. Codex review verdict + Opus response

**Codex verdict:** ACCEPT_WITH_MODS — direction correct, mods required.

**Critical Codex corrections accepted:**
- Room taxonomy LIVE enum = `Combat, Elite, Boss, Chest, Merchant, Forge, Event, Curse, Corridor` (Plan v1 drifted with Shop/Unknown/Spirit/Treasure/Shrine naming)
- DungeonGraph runtime = 12 fixed + up to 2 optional fork nodes (not NLM's 15)
- Library template bounds = PROTOTYPE scale (6×6 to 16×10), NOT NLM's 24×18-40×30 canonical
- ThreatBudget DOES NOT EXIST (only `EncounterSlot.cs` + count-based spawn)
- WaveSpawner DOES NOT EXIST
- DungeonMapUI + MapFragment ZATEN LIVE (audit, not implement)
- v15h Wang 6/16 → 6 placed (not 6 variants); 16 tile + rule asset all present
- v15h L5-L8 → missing profile pool entries, not missing code path
- v15h 2 Warblade → bug/stale scene artifact (composer destroy-then-create, but Destroy may be skipping)

**Codex error corrected by Opus:** 5000 PixelLab allocation memory file EXISTS at `~/.claude/projects/F--Antigravity-Projeler-2d-roguelite-RIMA/memory/project_5000_pixellab_allocation_lock.md` (Codex looked in repo `memory/`, but auto-memory lives in user .claude profile). Budget collision validation IS evidence-backed.

---

## 1. Vizyon (NLM canonical — unchanged)

Hades-arena + STS-graph hibrit. 4 act, 3-fork branching after node 3, Map Fragment visibility, "Fractured Epic" theme.

---

## 2. MVP / Phase 1 Scope (LOCK — Codex HIGH #1 applied)

**MVP = vertical slice, NOT full Act 1.**

| Item | MVP value | Live state |
|---|---|---|
| Class | Warblade only | LIVE (PixelLab `2656075d`) |
| Biome | 1 — **Shattered Ruins / Shattered Keep** (canonical single name; Sunken Keep flavor only) | v15h LIVE composition |
| Oda tipi (aktif) | 5 — Combat, Elite, Boss, Merchant, Chest | Code enum live |
| Oda sayısı / koşu | 8-9 oda | DungeonGraph supports |
| Boss | Penitent Sovereign F1 only | New `Boss_Arena_01.asset` needed |
| Run süresi | ~10 dk | |
| Map visibility | Map Fragment system | LIVE (`MapFragment.cs`, `LegacyRuntimeRoomManager.cs:653-669`) |
| DungeonMapUI | M key toggle | LIVE (`Assets/Scripts/UI/DungeonMapUI.cs`) |

**Phase 2+ unlock:** Forge, Curse, Event, Spirit (Act 2 only). Act 2/3 biome. Full DungeonGraph 12-14 node.

---

## 3. Oda Tipleri (Code-authoritative enum — Codex HIGH #2)

**Bu enum kanonik. Plan dokümanı, brush, asset, library hepsi BU isimleri kullanır:**

```
public enum RoomType  // Assets/Scripts/Core/RoomType.cs
{
    Combat,    // ⚔️ standart wave
    Elite,     // 💀 affix mob + Rare+ ödül
    Boss,      // 👑 act sonu
    Chest,     // 📦 sandık 3-pick (was: Treasure)
    Merchant,  // 🛒 shop (was: Shop)
    Forge,     // 🔨 craft 3-tab (Phase 2)
    Event,     // 📜 statik ikili event (Phase 2)
    Curse,     // 🌀 risk/reward (Phase 2)
    Corridor   // — geçit/spawn/rest dahil generic non-combat (was: Shrine, Spawn, Rest)
}
```

**NLM "Unknown/Spirit/Shrine" mapping:**
- **Unknown** = graph-time hidden facade — `Combat | Elite | Merchant | Curse | Event` öncesi ikon gizleme. ENUM eklenmez, UI/graph layer'da.
- **Spirit Encounter** = Phase 3 (Act 2+). Şimdilik kapsam dışı.
- **Shrine/Rest/Spawn** = `Corridor` enum altında tag system ile alt-tür.

**MVP aktif:** Combat, Elite, Boss, Chest, Merchant, Corridor (Spawn/Rest tag).
**Phase 2:** Forge, Curse, Event.
**Phase 3:** Spirit (UI layer).

---

## 4. Biome Roster (LOCK)

| Act | Biome name (kanonik) | MVP? |
|---|---|---|
| **Act 1** | **Shattered Ruins / Shattered Keep** (tek isim) | ✓ |
| Act 2 | Bleeding Wastes | Phase 2 |
| Act 3 | Core Approach | Phase 3 |
| Final | Nexus Core | Phase 3 |

v15h composition LIVE = Shattered Ruins. Asset palette unified under one name.

---

## 5. Map Generation Philosophy (Code-authoritative)

### 5a. Live state
- **DungeonGraph.cs**: branching + reveal state + current-node + map reveal hooks LIVE
- **DungeonMapUI.cs**: M-key toggle + node visibility/rebuild LIVE
- **MapFragment.cs**: spawn-on-clear + `RevealAhead(steps)` LIVE
- **Topology runtime:** 12 fixed + up to 2 optional fork nodes (NOT NLM's 15)

### 5b. Branching
- Node 1-3: lineer
- Node 4+: 3-4 fork
- Act sonu: convergence

### 5c. Visibility
- Map Fragment topla → 1-2 node ileri ikon görünür
- Smart spawn: blind 100% / 1-step 50% / 2+ step 10% (memory)

### 5d. Scaling — Threat Points
**CRITICAL: ThreatBudget DOES NOT EXIST in code.** Pre-MVP task:

→ Yeni sistem: `ThreatBudgetSO` + `EncounterSelector` (Codex MED → Opus PROMOTE HIGH).
- Mevcut: `EncounterSlot.cs` + `LegacyRuntimeRoomManager.SpawnEnemies(count, forceElite, ...)` (count-based)
- Hedef: budget-based wave spawn
- Act 1 combat: 8-12 pt budget; mob fiyatları swarm 1pt / grunt 2pt / special 4pt

**MVP-blocker:** ThreatBudget olmadan combat scaling rastgele. Implement before vertical slice ship.

---

## 6. Room Sizes (LIVE library actual vs NLM canonical)

**Codex finding:** Mevcut Library bounds prototype scale (6×6 to 16×10), NLM canonical (24×18-40×30) değil.

**Karar:** İki çözüm var, Opus seçimi:

| Option | Sonuç | Maliyet |
|---|---|---|
| A — Library'yi NLM sizes'a expand et | Combat M = 24×18, Boss = 40×30. Yeniden author. | Yüksek (10 template re-do) |
| **B — Prototype scale'i RIMA canonical olarak LOCK et** | Combat S=8×6, M=12×8, L=16×10, Elite=10×8, Boss arena=20×14 (new), Corridor=12×4 | Düşük (mevcut korunur) |

**Opus seçimi: B (prototype scale LOCK).**

Gerekçe:
- v15h kendi 36×22 composition test ediyor, ama bu auto-populator playground. Library = runtime.
- 8×6 combat oynanır (Hades küçük arena hissi), 16×10 = large arena enough
- NLM canonical sizes 32×32 tile × 0.5 PPU ratio'ya göre — RIMA PPU=64 = 2× zoom. Visual scale farklı, oynanır alan benzer
- 10 template re-author maliyeti yüksek, ölçek değişikliği prototype phase için ertelenebilir

**YENİ LOCK — Room sizes (32px tile, PPU=64):**

| Tip | Live bounds | MVP target |
|---|---|---|
| Spawn | 8×6 | 8×6 |
| Combat_Small | 8×6 | 8×6 (4-6 enemy spawn) |
| Combat_Medium | 12×8 | 12×8 (6-8 enemy) |
| Combat_Large | 16×10 | 16×10 (8-12 enemy) |
| Elite | 10×8 | 10×8 (1 Elite + 3-4 grunt) |
| Boss_Intro | 14×10 | 14×10 (dramatic corridor) |
| Boss_Arena | NEW | 20×14 (single boss + mechanics space) |
| Corridor_Linear | 12×4 | 12×4 |
| Corridor_LShape | 10×8 | 10×8 |
| Chest | 6×6 | 6×6 |
| Merchant | NEW | 10×8 |
| Shrine (Corridor sub-tag) | 8×8 | 8×8 |

---

## 7. Room Library — Hibrit Procedural + Polish (LOCK)

### 7a. Canonical pipeline (Codex HIGH #4)
**RoomData/RoomTemplate = primary source-of-truth.**
**v15h composition pipeline = authoring generator only (not runtime canonical).**

### 7b. Pipeline flow
```
v15h composer (auto-populate playground)
    ↓ snapshot
RoomTemplateSO (.asset, designer polish)
    ↓ runtime load
DungeonGraph → spawn from RoomBank.Pick(type)
    ↓ runtime spawn
LegacyRuntimeRoomManager → instantiate
```

### 7c. v15h-to-RoomTemplate snapshot path (Codex HIGH §11 #2 → ACCEPT)
- `RoomTemplateSaver.cs:14-18, 92-100` infrastructure mevcut
- Yeni menu item: "Tools/RIMA/Map Designer/Save v15h to RoomTemplate"
- Workflow: v15h compose → designer beğenir → Save → `Assets/Data/Rooms/Library/Combat_Medium_02.asset`
- Phase 1.5 RoomData spec ile alignment: RoomData "designer authored", v15h sadece kaynak

### 7d. MVP Library expansion target (Codex MED #5 — 20 polish, 12-14 MVP gate)

| Tip | Mevcut | MVP gate | Polish target | Eksik MVP |
|---|---:|---:|---:|---|
| Spawn | 1 | 1 | 1 | — |
| Combat_Small | 1 | 2 | 3 | +1 |
| Combat_Medium | 1 | 2 | 3 | +1 |
| Combat_Large | 1 | 1 | 2 | — |
| Corridor_Linear | 1 | 1 | 2 | — |
| Corridor_LShape | 1 | 1 | 1 | — |
| Elite | 1 | 2 | 2 | +1 |
| Boss_Intro | 1 | 1 | 1 | — |
| **Boss_Arena** | 0 | 1 | 1 | **+1 NEW** |
| Chest | 1 (Treasure_01 rename) | 1 | 1 | rename |
| **Merchant** | 0 | 1 | 1 | **+1 NEW** |
| Shrine (Corridor) | 1 | 1 | 1 | — |
| Unknown facade | — | UI layer | UI layer | UI work |
| **TOTAL** | **10** | **14** | **19** | **+4 (+UI)** |

---

## 8. v15h LIVE Status + Fix List (Codex evidence-corrected)

### 8a. Live metrics (verified)
- Cells 375, FloorFillCoverage 0.848 ✓
- L1=375 base, L2=318 decoration, L3=47 props
- Warblade spawned + movement ✓
- 16 Wang tiles + RuleTile all asset present
- 6 Wang PLACED (not 6 variants) — RuleTile coverage issue
- L5-L8 = 0 (profile pool entries missing, NOT code missing)
- 2 Warblade likely Destroy-skip bug in composer

### 8b. v15h fix priorities (Opus final)

| Fix | Priority | Why |
|---|---|---|
| 2 Warblade overlap bug | **HIGH** | Game-breaking visual; composer `RimaV15hPlayableComposer.cs:77, :532-576` Destroy might not catching old instance |
| Wang RuleTile coverage 6→target ~30 | **MED** | Visual polish; current 6 placed gives "isolated patches" feel |
| L5-L8 profile pool entries | **LOW** | Atmospheric polish; not MVP-critical (Codex LOW) |
| Re-author Library for canonical sizes | **DEFER** | Phase 2 unless playability problem; Option B locked |

---

## 9. Phase 1.5 RoomData Spec — 5 Open Questions RESOLVED (Codex suggested + Opus final)

| # | Question | Opus DECISION |
|---|---|---|
| 1 | Chunk renderer strategy (ChunkMesh vs BatchedSprites) | **BatchedSprites first via `IRoomChunkRenderer` interface. Benchmark/replace with ChunkMesh in 1.5D if perf demands.** Lower integration risk, swap-friendly. |
| 2 | RoomData serialization (SO vs JSON vs binary) | **Unity ScriptableObject = authoritative asset. JSON only for debug/diff tooling (optional editor utility).** Inspector + Unity-native + Phase 1.5 spec says SO. |
| 3 | Deterministic selector (MackySoft direct vs RIMA wrapper) | **RIMA-owned `RimaWeightedSelector` wrapper. MackySoft Choice = internal implementation, not public gameplay API.** Decoupling for portability. |
| 4 | Wang16 edge owner (brush vs WangResolver) | **`WangResolver` re-computes on dirty bounds. Brush records intent + hard features only.** Cleaner separation, fixes v15h Wang 6/30 issue too. |
| 5 | Entity vs VisualPlacement L6/L7 | **Collider/interactable/combat-relevant = `EntityRecord` with prefab. Pure decorative = `VisualPlacementData`. Shrine fragments → EntityRecord.** |

Phase 1.5 spec'i bu kararlarla update edilmeli (1.5A Data phase başlamadan).

---

## 10. Asset Production Roadmap (5000 PixelLab budget — verified)

5000 allocation LOCK ([[project_5000_pixellab_allocation_lock.md]]) ile fit:

### Wave 1 (immediate, ~1 hafta)
| Workstream | Asset | Estimated gen | Sahibi |
|---|---|---:|---|
| Codex tile/wall | Shattered Ruins biome tiles complete (gpt-image-1) | 0 PixelLab | Codex review |
| PixelLab char | Warblade state tweaks (existing 2656075d) | 50 | Manual web V3 |
| Codex hero prop | 3 cluster template (combat focal/support/blocker) | 0 PixelLab | Codex review |
| Unity Sonnet | v15h fix (2 Warblade + Wang coverage) | — | Sonnet+UnityMCP |
| Unity Sonnet | RoomTemplate save-from-v15h tool | — | Sonnet+UnityMCP |
| Unity Sonnet | RoomTemplate rename (Treasure_01 → Chest_01) | — | Sonnet+UnityMCP |
| Unity Codex+Sonnet | ThreatBudgetSO + EncounterSelector new system | — | Codex impl + Sonnet wire |

### Wave 2 (1-2 hafta)
| Workstream | Asset | Estimated gen |
|---|---|---:|
| PixelLab char | Elementalist + Gunslinger anchors (10-anchor LOCK setup) | 200 |
| PixelLab mob | 4 MVP mob roster (imp/grunt/special elite) | 250 |
| Library | +4 RoomTemplate (Combat S/M variant, Elite_02, Boss_Arena_01, Merchant_01) | 0 |
| VFX | Dash trail + hitspark A/B pilot | 100 |
| Unity | Unknown facade UI layer | — |

### Wave 3 (2-4 hafta)
| Workstream | Asset | Estimated gen |
|---|---|---:|
| PixelLab char | 7 anchor batch | 700 |
| PixelLab mob | +4 mob variants | 200 |
| Library | Polish 19 templates | 0 |
| HUD | Skill icons + portrait crops | 150 |
| Boss | Penitent Sovereign F1 sprite + VFX | 100 |

**Total Wave 1-3 ~1750 PixelLab gen (35% of 5000)** — Mid-checkpoint at 3150 stays comfortable.

---

## 11. Önerilen Sıra — Phase 1 Ship Roadmap

| # | Adım | Sahibi | ETA | Blocker |
|---|---|---|---|---|
| 1 | v15h 2 Warblade overlap fix + Wang coverage 6→30 | Sonnet+UnityMCP | 1 saat | — |
| 2 | RoomTemplate `Treasure_01` → `Chest_01` rename + asset taxonomy normalize | Sonnet+UnityMCP | 30 dk | — |
| 3 | **ThreatBudgetSO + EncounterSelector** sistem (NEW pre-MVP critical) | Codex impl review + Sonnet wire | 1-2 gün | — |
| 4 | v15h-to-RoomTemplate snapshot menu (RoomTemplateSaver kullan) | Sonnet+UnityMCP | 1 gün | #1 |
| 5 | Library +4 yeni: `Combat_Small_02`, `Combat_Medium_02`, `Elite_02`, `Boss_Arena_01`, `Merchant_01` | Sonnet+UnityMCP (v15h snapshot ile) | 2-3 gün | #4 |
| 6 | DungeonMapUI audit/integration test (mevcut + MVP path validation) | Sonnet review | 1 gün | — |
| 7 | PixelLab Warblade state tweaks + Elementalist + Gunslinger anchors | Manual user (V3 UI) | 2-3 gün | — |
| 8 | Phase 1.5 RoomData spec update with 5 question answers + start 1.5A Data phase | Codex impl | 1 hafta | #3 |
| 9 | Mob roster 4 MVP (Imp/Grunt/Special Elite) PixelLab production | Manual user | 1 hafta | — |
| 10 | VFX A/B dash trail + hitspark pilot | Manual user / autosprite | 3 gün | — |
| 11 | End-to-end MVP playtest (Warblade Act 1 vertical slice 8-9 oda + Boss F1) | Manual | Ship gate | All above |

---

## 12. NLM canonical drift (sync needed)

**Codex finding doğrulanan drift'ler:**
- NLM "15-node Act 1" → live 12+2 fork (NLM stale; karar #62 reference doğru ama implementation farklı)
- NLM "library_room_01_..._10" → actual library naming farklı (`Combat_Small_01.asset` vs `library_room_01_combat_shatteredkeep.asset`)
- NLM Shop/Unknown/Spirit/Treasure → live Merchant/Chest/(no Unknown enum)/Spirit-not-implemented

**NLM sync action:** Bu plan FINAL'in özetini `/nlm-sync` ile NotebookLM'e push et — drift hierarchy gereği (NLM canonical > local memory > prompt). Sonra memory file `project_map_plan_v1_lock.md` yaz.

---

## 13. Definite Decisions Summary (LOCK)

1. ✅ MVP = vertical slice 8-9 rooms (not full Act 1)
2. ✅ Room taxonomy = LIVE enum (Combat/Elite/Boss/Chest/Merchant/Forge/Event/Curse/Corridor)
3. ✅ MVP active types = Combat, Elite, Boss, Chest, Merchant, Corridor
4. ✅ Biome canonical name = "Shattered Ruins / Shattered Keep" (Sunken Keep flavor only)
5. ✅ Room sizes = prototype scale LOCK (8×6-20×14), NOT NLM 24×18-40×30
6. ✅ Library MVP gate = 14 templates, polish target = 19
7. ✅ Boss_Arena_01 = NEW asset (not Boss_Intro_01 reuse)
8. ✅ Composition pipeline = RoomData/RoomTemplate primary, v15h authoring generator
9. ✅ v15h fix priority: 2 Warblade HIGH → Wang coverage MED → L5-L8 LOW → re-author DEFER
10. ✅ ThreatBudget = NEW pre-MVP critical task (Codex HIGH #3)
11. ✅ DungeonMapUI = AUDIT not implement (LIVE)
12. ✅ Phase 1.5 5 questions resolved (see §9)
13. ✅ Mob roster MVP = 4 (not 8); 8 = polish target
14. ✅ **Visual layer architecture = Hybrid Monolithic Background + Gameplay Overlay (Option C, see §13b)**
15. ✅ v15h tile composition pipeline = DEPRECATED for production, kept for procedural/Unknown
16. ✅ Per-room painted background = PixelLab create_topdown_tileset OR gpt-image-1 (~14-19 MVP)

---

## 13b. CRITICAL Architecture Decision — Monolithic vs Tile Composition (Opus 2026-05-18 S91 EOD)

**Trigger:** User feedback "anlamsız tilelar var, büyük resim üretip oradan kesme mantığına mı geçeceğiz?"

**Three options evaluated:**

| Option | Visual coherence | Procedural variation | MVP gen cost | Asset reuse | Verdict |
|---|---|---|---|---|---|
| A — Pure tile composition (current v15h) | ✗ "scattered patches" | ✓ infinite | Low (existing tiles) | High | **Reject** — visual problem proven by v15h screenshot |
| B — Pure monolithic per-room | ✓ guaranteed | ✗ each room hardcoded | ~14 images for MVP (~$0.6 gpt-image-1 OR PixelLab create_topdown) | Low | Partial — kills procedural |
| **C — Hybrid: monolithic painted background + gameplay overlay** | ✓ background painted per room | ✓ on gameplay/Unknown rooms | ~14-19 images for MVP + tile reuse | Medium | **ACCEPT** — best of both |

### Option C — LOCK

**Visual layer = ONE painted background per RoomTemplate** (PixelLab create_topdown_tileset or gpt-image-1, ~512×320 to 640×480 painted scene)
**Gameplay layer = RoomTemplate data** (collision grid, spawn sockets, gates, interactables — designer-authored, data-driven)
**Tile composition (v15h-style) = DEPRECATED for production rooms**, kept ONLY for:
- Procedural Unknown room mystery facade
- Quick test/iteration scaffolds
- Future Phase 3 procedural overlay variation

### Why Option C wins

1. **Hades model proven** — Hades does exactly this: hand-painted unique background per room + gameplay collision overlay
2. **Visual coherence guaranteed** — no more "scattered patch" v15h problem
3. **Asset budget compatible** — 14-19 backgrounds × ~$0.04 gpt-image-1 = ~$0.6 MVP (or 14 PixelLab create_topdown ≈ 280 gen from 5000 allocation, fits comfortably)
4. **Data portability** — gameplay data stays SO-driven (Phase 1.5 RoomData spec compatible)
5. **Designer workflow** — paint your background in PixelLab/Photoshop, drop in RoomTemplate, polish gameplay overlay in Editor
6. **Renderer-agnostic** — background = sprite quad, gameplay = tile data; future 3D port retains data layer ([[project_3d_portability_strategy]])

### Phase 1.5 RoomData spec impact

§9 Q1 (Chunk renderer) **REVISED:** BatchedSprites still primary, but **add `BackgroundLayer` sprite quad rendering before tile layers** in `IRoomChunkRenderer`. Background is single SpriteRenderer per room, not chunked.

§9 Q5 (Entity vs Visual) UNCHANGED.

### v15h composer fate

v15h auto-populate composer → **archive after this turn.**
- `Pro_Redesign_v15h_Playable_CombatRoom` GameObject = test scaffolding, deactivate or delete after first painted background test
- Tile assets (Wang dirt/cobble) **kept** for transition decals or procedural Unknown facade
- AutoPopulator code path **kept** for procedural future, not removed

### Production workflow (NEW)

```
1. Designer concept: "Combat_Medium_02 = arena with broken statues + cyan rift cracks"
2. PixelLab create_topdown_tileset OR gpt-image-1 painted scene 640×480 → import to Assets/Art/Rooms/Backgrounds/
3. Create RoomTemplateSO.asset with:
   - background_sprite ref → painted image
   - bounds = 12×8 tile grid
   - collision_grid = designer-painted in RoomTemplate editor (Tile/Pass)
   - spawn_sockets = enemy positions
   - gates = N/S/E/W
   - decoration_overlay = OPTIONAL props on top (interactables only)
4. RoomBank.Pick(type) → RoomTemplate → runtime spawn renders background sprite + collision + entities
```

### Background art RULE (User feedback 2026-05-18 EOD)

**"Flat top-down square tile" YASAK. "Angled perspective oval/rounded surface" ZORUNLU.**

Hades / Sea of Stars / Octopath Traveler tarzı **30-35° angled top-down**:
- Floor surface foreshortened (üst kenar dar, alt kenar geniş)
- Tile/stone shapes ROUNDED/OVAL (kare köşeler değil)
- No visible square grid lines
- Subtle elevation hint (props cast small shadows showing depth)
- "Fake 3D" hissi — düz değil, derinlikli

**Asset prompt template (Shattered Ruins biome painted background):**

```
A top-down 2D pixel art game scene, viewed from a 30-35 degree angled perspective
(NOT flat top-down). The floor is a continuous painted surface of cracked granite
flagstones in cool gray with cyan rift cracks. Stones are ROUNDED, OVAL, organic
shapes (not square grid tiles) with subtle foreshortening — top edges compressed,
bottom edges wider — creating a fake-3D depth feel. Soft directional shadows hint
at elevation. Painterly, no harsh grid lines, no visible tile boundaries.
Resolution: 640x480 or 512x320. Style: Hades / Sea of Stars angled top-down
painted background.

Negative Prompt: flat top-down, square tile grid, visible grid lines, isometric
diamond tiles, axonometric, sharp square stones, repeating tile pattern.
```

**Rejected approaches:**
- PixelLab `create_topdown_tileset` produces square tiles → use `create_object` (full scene) or gpt-image-1 painted scene instead
- Codex gpt-image-1 tile sets (current v15h art) → these are flat top-down squares, look like "anlamsız tile patches" → DEPRECATE
- Wang transition tiles → only useful if visible boundary needed (Phase 3 procedural Unknown rooms only)

### Asset production roadmap update (override §10)

**Wave 1 (immediate, ~3-5 days)** — REPLACES tile-heavy plan:
| Asset | Producer | Gen estimate |
|---|---|---|
| 5 painted RoomTemplate backgrounds (Spawn_01, Combat_S/M/L_01, Elite_01) | PixelLab create_topdown_tileset OR gpt-image-1 | 5 images |
| 1 Boss_Arena_01 background | PixelLab/gpt | 1 |
| 1 Merchant_01 background | PixelLab/gpt | 1 |
| 1 Chest_01 background (Treasure_01 rename + repaint) | PixelLab/gpt | 1 |
| Warblade re-download (user manual, PixelLab character section) | User V3 UI | — |
| RoomTemplate editor extension for background_sprite field | Sonnet+UnityMCP | — |
| Background layer renderer in `IRoomChunkRenderer` | Codex impl + Sonnet wire | — |
| **Total Wave 1 MVP** | | **~9 painted images** |

**Wave 2 polish:** +5 alternate backgrounds for variety (Combat variants, etc.) = ~14 painted backgrounds total for MVP polish target.

### Immediate action (2026-05-18 EOD)

1. ✅ **Characters deleted** from RoomPipelineTest scene (Warblade_v15h_Player + Player + PlayerGlow) — DONE this turn
2. ⏳ User re-downloads Warblade from PixelLab character section (manual V3 UI)
3. ⏳ User places Warblade in scene at known spawn point (or new RoomTemplate-instantiated room)
4. ⏳ Next session: First painted RoomTemplate background test (1 image → Spawn_01.asset → playtest)

---

## 14. Out of MVP scope (defer)

- Forge / Curse / Event / Spirit oda tipleri
- Act 2/3/Final biome
- 10-class roster (Phase 2 character batch)
- Boss F2/F3 phases
- L5-L8 atmospheric polish layer
- v15h composition ↔ Library size mismatch resolve (Library re-author)
- DungeonMapUI new node icon set / animation polish
- 8-mob roster expansion
- HUD final art / skill VFX expansion beyond 1-2 pilot
- Map Fragment "smart spawn" rate tuning (live default OK)

---

**End of MAP_PLAN_v1_FINAL.**
**LOCK timestamp:** 2026-05-18 S91
**Next action:** Memory write `project_map_plan_v1_lock.md` + CURRENT_STATUS update + v15h fix dispatch (Task #6).
