---
status: AUDIT
faz: post-Brush-V1
tarih: 2026-05-18
ozet: "Map designer için audit — neyimiz var, ne eksik, güzel map görmek için aksiyon listesi"
---

# Map Designer Readiness Audit — "Güzel Bi Map Görmek"

## TL;DR
**Brush V1 tool SHIP-READY**, ama CONTENT (props/rooms/lighting/anchor) sparse. Güzel bi map görmek için **~7 kritik eksiklik**, **2-3 sprint çalışması** veya **1 hafta odaklı production**.

**En hızlı yol:** Mevcut StoneDungeon Wang16 tile set + Brush V1 ile **1 sample room** kompoze et + 5 prop üret + lighting ekle + 1 mob yerleştir → **2-3 günde güzel demo room**.

---

## ✅ NEYİMİZ VAR (LIVE)

### Tool / System
- **Brush V1 system** — 321/321 EditMode PASS, full editor tool (S9-S13 sprint LIVE)
- **PropRuntimeSpawner** wired (S13 P0 fix, dc4fe8f commit)
- **PropFootprintValidator + Rotation** (S13 LIVE)
- **PropRegistrySO + Postprocessor** (S13 LIVE — runtime GUID lookup)
- **BridsonPoissonAutoPlacer** (S13 LIVE)
- **DependencyReportGenerator** (S13 LIVE — Menu: RIMA → MapDesigner → Brush → Generate Dependency Report)

### Tiles (Wang16 set)
- **StoneDungeon:** 6 floor pro + **16 wall pro Wang16 complete** ✅
- MossyCrypt: 3 floor pro (Wang16 INCOMPLETE — 13 wall tile + 3 ekstra floor eksik)
- Act1 spritesheet: floor_tiles + wall_tiles + decor_tiles
- pit_dark + pit_edge

### Brush definitions (14+ LIVE)
Default pack: Clean Stone Floor, Dark Variation, Hades Wall Cap, Dirt Patch, Moss Soft Oval, Crack Scatter, Rubble Cluster, Mossy Broken Edge, Rift Scar, Battle Aftermath, Edge Trim, Rift Damaged Corner

### BiomeSkin presets (4 LIVE)
HadesNet, Grimdark Mix, Soft Painter, Bold Graphic

### Decoration sprites
- Chest folder
- Gate folder
- `RIMA_wall_torch.png` (1 torch sprite)

### Scenes
- `_FazMVP_Demo.unity` (existing demo)
- `Phase1_ProceduralMap_Test.unity`
- `Phase2_WeaponAttach_Test.unity`
- `RoomPipelineTest.unity`

### Light system (scripts var, asset sparse)
- `LightFlicker.cs`
- `RoomMoodLightPool.cs`
- `PropLightKind.cs`
- 1 wall torch sprite

### Character anchors (Karar #145 v2 hazır)
- `ANCHORS/characters/` 10 canonical anchor LIVE (PixelLab create pending — user task)
- `Skill_*.asset` × 16 — 10 sınıfın bazı skill data'ları
- `CCS_*.asset` × 10 — cross-class skill data'ları
- `SpriteHandData_Warblade_*` × 3 (Idle, Walk, Attack) — Karar #144 weapon child SR wired

---

## ❌ EKSİKLİKLER (Güzel map için)

### 🔴 P0 (Map demo için MUTLAKA gerekli)

| # | Eksiklik | Mevcut | Hedef | Owner |
|---|---|---|---|---|
| 1 | **Room template Library boş** | 0 dosya (sadece `.gitkeep`) | **5-10 room template** (combat/corridor/elite/treasure/shrine) | USER (Brush V1 editor'da kompoze) |
| 2 | **Prop variety yok** | 1 barrel | **8-12 prop** (urn, crate, statue, candle, banner, bones, debris, chains, brazier, altar, chest, treasure pile) | USER (PixelLab) + Opus prop prompt |
| 3 | **Lighting kompozisyonu** | scripts var, sahnede yerleştirilmemiş | URP 2D Light setup demo room'da (torch + ambient + rim) | USER (Unity scene) |

### 🟡 P1 (Daha iyi görünüm için)

| # | Eksiklik | Mevcut | Hedef | Owner |
|---|---|---|---|---|
| 4 | **Mob anchor production-ready değil** | 4 candidate var, animation yok | 4 mob × idle anim = 8 anim states minimum | Sprint 14+ (USER PixelLab) |
| 5 | **Character anchor PixelLab ID yok** | 10 anchor sprite var, ID yok | 10 PixelLab character ID + idle anim states | USER (PixelLab create character) |
| 6 | **MossyCrypt Wang16 incomplete** | 3 floor tile | 6 floor + 16 wall tile (StoneDungeon parity) | Opus prompt + Codex/USER üretim |
| 7 | **Parallax architectural shell yok** | spec var (Karar #143-E + Room Staging) | shell layer sprite + sahnede wire | Phase 1.5 |

### 🟢 P2 (Polish — sonradan)

| # | Eksiklik | Notlar |
|---|---|---|
| 8 | Decal pass (Karar #154 candidate) | Sprint 16+ telegraph contract |
| 9 | Cursor active camera (#152) | Sprint 14 P0 from 10-decision plan |
| 10 | UI clutter control (#153) | Sprint 14-15 |

---

## SENİN ÜRETMEN GEREKENLER (USER)

### Hemen (1-2 günde güzel demo için)

**1. Prop sprite üretimi (8-12 adet)** — En kritik eksiklik
PixelLab veya Codex/SD ile üret. RIMA palette (Vivid Vulnerability / Fractured Epic tone):

| # | Prop | Boyut | Öncelik |
|---|---|---|---|
| A | Urn (broken/intact) | 32×48 | P0 |
| B | Wooden crate (large/small) | 32×32, 48×48 | P0 |
| C | Stone column (intact/broken) | 32×64 | P0 |
| D | Candle + candleholder | 16×24 | P0 |
| E | Stone statue (kneeling/standing) | 48×96 | P1 |
| F | Banner (hanging) | 32×64 | P1 |
| G | Bones / skeleton remains | 32×32 | P1 |
| H | Chains (hanging/coiled) | 16×64 | P1 |
| I | Brazier (lit/cold) | 32×48 | P1 |
| J | Altar (small/large) | 48×64 | P1 |
| K | Treasure pile (gold + gems) | 48×32 | P2 |
| L | Debris pile (rubble) | 32×32 | P2 |

Prompt template: chibi pixel art prop için Opus yazabilir (söyle başlasın).

**2. Room template kompoze (5-10 adet)** — Brush V1 editor'da
- 2× Combat room (open arena)
- 2× Corridor (lineer geçit)
- 1× Elite room (mid-size, 1 elite spawn)
- 1× Treasure room (chest spawn)
- 1× Shrine room (altar centerpiece)
- 1× Spawn room (run başlangıç)
- 1× Exit room (act sonu)
- 1× Boss intro room (Phase 1.5)

**3. Lighting setup** — Unity URP 2D Lights
- 1-2 torch (FreeForm Light) per duvar
- 1 ambient global light (low intensity, deep blue/teal tone)
- LightFlicker.cs torch'larda enable
- RoomMoodLightPool.cs ile spawnable

**4. Character + Mob anchor PixelLab create**
- 10 character + 4 candidate mob → PixelLab "create character" → ID al
- Karar #145 Use #1 idle state üret (en azından South direction)

### Sprint 14+ kapsamında (1-2 hafta sonra)

**5. MossyCrypt Wang16 complete** — 16 wall + 3 ekstra floor üretim (StoneDungeon parity)
**6. Mob animation states** — 4 candidate × idle/walk/hit (3 state × 4 mob = 12 state, ~30 gen budget)
**7. Parallax architectural shell** — Phase 1.5+ (Karar #143-E)

---

## EN HIZLI YOL — "BUGÜN GÜZEL BİR MAP" (3 Adım)

### Adım 1 (Tonight) — Prop batch generate
Opus sana 8-12 prop için chibi pixel art prompt yazar → PixelLab Create Image Pro V3 ile batch → 8-12 PNG → Unity import

### Adım 2 (Tomorrow) — Sample room compose
Brush V1 editor'da:
- StoneDungeon biome seç
- 6×4 room shape brush'la kapla (floor + wall + Wang16 auto)
- Yeni prop'ları yerleştir (PropPlacer + R rotation hotkey)
- BridsonPoissonAutoPlacer ile scatter (bones, debris)
- Save → `Assets/Data/Rooms/Library/sample_room_01.asset`

### Adım 3 (Tomorrow PM) — Lighting + screenshot
- Unity scene aç (`_FazMVP_Demo.unity` veya yeni)
- Sample room template instantiate
- 2-3 torch yerleştir (RIMA_wall_torch + LightFlicker enable)
- Ambient light: deep blue-teal #1A2438, intensity 0.35
- Camera pos + Pixel Perfect Camera config
- Screenshot al → işte güzel map!

**Toplam süre:** 8-12 saat odaklı work.

---

## OPUS / CODEX İŞ LİSTESİ (sen onaylayınca başlar)

| # | İş | Owner | Çıktı |
|---|---|---|---|
| 1 | **Prop production prompts** (12 prop) | Opus + rima-asset | `STAGING/prop_production_prompts_v1.md` |
| 2 | **Sample room composition checklist** | Opus | `STAGING/sample_room_01_composition.md` (Brush V1 step-by-step) |
| 3 | **Lighting setup guide** | Opus | `STAGING/demo_room_lighting_setup.md` (Unity URP 2D Light specs) |
| 4 | **MossyCrypt Wang16 prompt** | Opus | Codex'e dispatch — 16 wall + 3 floor tile üretim |
| 5 | **Mob production prompts** (4 candidate animation) | Opus | `STAGING/mob_animation_states_prompts.md` |
| 6 | Sprint 14 spec writing | Opus | Combat integration + #152 camera + #153 UI clutter |

---

## ÖNERİM

**Bugün:** Adım 1 başlat — prop production prompts yaz → user PixelLab batch üretsin.

**Yarın:** Adım 2 — sample room compose + Adım 3 — lighting setup.

**3 günde:** Beautiful sample room screenshot.

Sonra Sprint 14 başlar — combat integration + mob animation + 10 karar implementation.

Hangisinden başlayalım? **Prop prompts** mı, **Sprint 14 spec** mi, yoksa ikisini parallel mi?
