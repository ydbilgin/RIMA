# RIMA Room Pipeline — FINAL DECISION (Claude orchestrator)

**Tarih:** 2026-05-23 (S102 close)
**Bağlam:** ChatGPT'nin 6 görseli + 10 sorusu üzerine Claude (Opus 4.7) ve Codex (gpt-5.5) bağımsız değerlendirme yaptı. Bu doc iki yazının karşılaştırması, ayrışma noktalarının çözümü ve son karar/plan.

---

## Karşılaştırma matrisi

| Konu | Claude | Codex | Verdict |
|---|---|---|---|
| **Ana yön** | C primary, modular wall demoted | C primary, modular wall = sadece backwall landmark band | ✅ UYUM (çift HIGH confidence) |
| **PixelLab format** | Single-asset card library | 32x32 floor + 64x64 edge + 128x96 backwall card sheets | ✅ UYUM (Codex daha somut boyut verdi) |
| **Backwall yaklaşımı** | Monolitik dramatic tek silüet card | Parçalı N/W family separation (modular wall MVP reuse) | ⚠️ AYRIŞMA → MVP'de monolitik, scale'lendiğinde modular |
| **Combat density** | 8-12 aktör hard cap | 6-10 düşman + 1-3 ally cap | ✅ UYUM (~10 toplam) |
| **Floor tile boyut** | 256x256 (PixelLab single) | 32x32 tile (Tilemap-friendly) | ⚠️ AYRIŞMA → 256² üret, Aseprite split → 64×64 tile (PPU 64'le uyumlu) |
| **İlk asset batch** | 15 asset (floor 4 + edge 3 + decal 2 + prop 4 + backwall 2) | 15 asset (benzer dağılım) | ✅ UYUM (%80 örtüşme) |
| **Floor pipeline** | Tilemap + decor | Baked baseImage hızlı MVP → uzun vade Tilemap | ✅ Codex'in MVP→production geçişi mantıklı |
| **İlk oda arketipi** | Battered Hall → Rift Gate → Ritual | Shattered Keep Combat → Rift Gate → Ritual → Prison | ✅ İlk 3 aynı |
| **Sorting layer** | 8 katman | 10 katman (Front Edge + VFX layer ek) | ✅ Codex versiyonu DAHA İYİ |
| **RoomTemplate.cs uyumu** | Yeterli, DecorCategory enum genişlet | baseImage + wallPathLocalPoints + anchors + doorSockets zaten var | ✅ Codex aktüel field listesini doğruladı |
| **Camera bounds** | 480×270 ref res | 24×14 - 32×18 tile normal, 36×20 boss | ✅ Tile-bazlı daha somut |
| **Lighting** | Ambient 0.15 + brazier #FFA864 + rift #4DD4FF | Baked YOK, ayrık 2D Light prefab, base decor emissive < VFX emissive | ✅ Codex'in emissive ayrımı KRİTİK ekleme |
| **Bastion ref** | Form > readability > polish | Technical > form > clarity | ✅ Birleştir: 3 farklı eksende |
| **MVP step count** | 11 step | 10 step | ✅ UYUM |
| **LoRA training** | Paused kalsın, MVP başarı sonrası | (mention yok) | ✅ Claude önerisini koru |

**Ortak iki HIGH confidence + 3 ayrışma noktası (hepsi çözülebilir nuance, blocker değil).**

---

## SON KARAR

### Yön: **C (Fractured Chamber) — PRIMARY** | Confidence: **HIGH**

Modular Wall Shell MVP (S102 LATE, 4×4 sheet, 16 piece) **DEMOTED → yan track**. Sadece north backwall landmark band üretimi için kullanılır; full perimeter wall için DEĞİL. Eğer 16-piece sheet üretiminde edge match + cell discrimination FAIL ederse, MVP'de monolitik tek-parça backwall card'lara düşeriz (Claude yaklaşımı).

### RIMA oda formülü (LOCK)

> **RIMA odası = Fractured Dark Granite Floor Island + Low Broken Edges + Black/Rift Void + Cyan Rift Crack Decals + Amber Light Prefabs + (opt) North Landmark Backwall Card + Theme Prop Cluster + Clean Combat Center**

**Zorunlu katmanlar:** Floor, Edge, Void/BG, Collision path, Encounter anchors, Door/socket markers, Cyan crack decal seti, Amber 2D Light prefab.
**Opsiyonel:** North landmark, side card, theme prop cluster, fog/water overlay, ritual circle, banner/cage dressing.
**Hard rule:** Telegraph/VFX emissive > base decor emissive (combat readability korumak için).

### Karakter ölçeği + combat density (LOCK)

- 64×64 chibi sprite, PPU 64
- Normal combat oda: **6-10 düşman + 1-3 ally/player = ~10 aktör cap**
- Boss oda: 1 boss + 2-4 mob = ~5 aktör cap
- Oda boyutu: **normal 24×14 tile (~768×448), büyük 32×18 tile (~1024×576), boss 36×20 tile (~1152×640)** — 64x64 tile bazında (PPU 64'le uyumlu)

### PixelLab asset boyutları (LOCK)

- Floor tile: **64×64** (PPU 64 tilemap), 4-cell sheet → 4 variant
- Low edge card: **128×64** (yatay edge) veya **64×128** (dikey), transparent BG, baseline aligned
- Backwall landmark card: **monolitik 512×384** (MVP) → modular 4×2 sheet (faz 2)
- Prop: **64×64** veya **128×96** (büyük props), transparent BG
- Decal: **128×64** veya **256×128**, transparent BG, fully composable
- Light overlay sprite: **64×64** radial gradient, transparent

---

## İlk 15-asset batch (LOCK, üretim sırası)

| # | Asset | Boyut | Sheet/Single | Üretici |
|---|---|---|---|---|
| 1 | Floor tile — clean dark granite | 64² | 4-cell sheet A | PixelLab user |
| 2 | Floor tile — cracked variant | 64² | 4-cell sheet A | PixelLab user |
| 3 | Floor tile — rift glow variant | 64² | 4-cell sheet A | PixelLab user |
| 4 | Floor tile — broken corner | 64² | 4-cell sheet A | PixelLab user |
| 5 | Low edge — N face straight | 128×64 | 4-cell sheet B | PixelLab user |
| 6 | Low edge — W/E side straight | 64×128 | 4-cell sheet B | PixelLab user |
| 7 | Low edge — corner outer | 64² | 4-cell sheet B | PixelLab user |
| 8 | Rubble cluster (seam cover) | 64² | 4-cell sheet B | PixelLab user |
| 9 | Cyan rift crack — linear | 128×64 | single asset | PixelLab user |
| 10 | Cyan rift crack — branching | 256×128 | single asset | PixelLab user |
| 11 | Amber brazier (sprite + light overlay ayrı) | 64²+64² | single asset | PixelLab user |
| 12 | Broken pillar base | 64² | single asset | PixelLab user |
| 13 | Backwall landmark — Rift Gate (monolitik) | 512×384 | single asset | PixelLab user |
| 14 | Sarcophagus (theme prop) | 128×96 | single asset | PixelLab user |
| 15 | Torch wall sconce overlay | 32×64 | single asset | PixelLab user |

**Üretim akışı:** 1-4 → 5-8 (sequential, floor + edge baseline lock) → 9-12 paralel (decal + prop + light) → 13 (backwall card test) → 14-15 (tema genişleme).

**Codex devralıyor:** Sheet geldiği anda PNG crop + transparent BG + Unity import + .meta generation.

---

## Unity assembly (LOCK)

### Scaffolding
Mevcut `Assets/Scripts/Rooms/RoomTemplate.cs` zaten uygun (baseImage, wallPathLocalPoints, anchors, doorSocketsLocalPoints, enemySpawnPoints, cameraBounds). **DecorCategory enum genişlet:** Floor, Edge, Prop, Decal, BackwallLandmark, LightOverlay, FrontEdge.

### Sorting Layers (LOCK — Codex önerisi adapted)
```
SortingLayer        Order   İçerik
------------------------------------------------
BG_Void             -50    Siyah background quad + opt. parallax
Floor                 0    Tilemap floor tiles
FloorDecal            5    Rift cracks, blood, glow pool
Edge_LowWall         20    Low broken edges (Y-sorted)
Props                30    Brazier, pillar, sarcophagus (Y-sorted)
Character            40    Hero + enemies (Y-sorted)
FrontEdge            55    Foreground props (char önünde)
BackwallLandmark     50    North landmark cards (char arkasında ama yüksek)
LightOverlay         60    Amber additive glow sprites
VFX_Telegraph        80    Combat telegraph + spell VFX
UI                  100    HUD
```

**Y-sort kritik:** Edge + Props + Character aynı sort group, transform.y axis sort (URP 2D Renderer CustomAxisSort).

### MVP base room hızlı yol
Floor island + low edge'i template **baseImage** olarak bake et (RoomTemplate field zaten var). Overlay anchor'larla decor/props/light yerleştir. **Uzun vade:** Floor Tilemap + decor prefab anchors.

### Lighting (LOCK)
- Global 2D Light: ambient **0.12-0.15**
- Brazier: Point Light 2D, range 5, color amber `#FFA864`, intensity 1.5
- Rift crack: Point Light 2D, range 2.5, color cyan `#4DD4FF`, intensity 0.6
- **Backwall landmark için ayrı spot key light** (opsiyonel)
- **Hard rule:** Base decor sprite emissive **DÜŞÜK**, VFX/telegraph emissive **YÜKSEK** (readability)

---

## İlk 3 oda arketipi (LOCK, MVP batch)

### Room 1 — **Shattered Keep Combat Room** (Battered Hall)
- Baseline combat oda, en yüksek reuse, run'da ~60% sıklık
- Asset count: 8-10 unique, ~25 placement
- Test: pipeline "iş tutuyor mu?" + 6-10 enemy combat readability
- Boyut: 24×14 tile (~768×448)

### Room 2 — **Rift Gate Chamber**
- Transition / boss intro, Room 1 asset'lerin %80'i reuse + 1 dramatic backwall card (Rift Gate landmark) + büyük rift crack swirl decal
- Asset count: 10-12 unique
- Test: backwall landmark sistemi tek başına oda kimliği taşıyabiliyor mu? + door socket pipeline
- Boyut: 28×16 tile

### Room 3 — **Ritual Chamber** (boss-tier)
- Tema yoğunluğu test: sarcophagus, altar, ritual stones, merkez ritual circle
- Asset count: 12-15 unique
- Test: tematik genişleme kapasitesi + boss combat 5-aktör cap
- Boyut: 32×18 tile

**Faz 2 (MVP sonrası):** Prison Hold → Library Archive → Flooded Crypt → Transition Corridor.

---

## Referans takeaways (3-eksenli, sentez)

| Referans | Ne için |
|---|---|
| **Children of Morta** | TECHNICAL hedef: pixel art + modern 2D lighting kalitesi, karakter anim + dungeon atmosfer aynı anda okunur |
| **Bastion** | FORM hedef: fractured floating platform + edge language + void boundary, tam duvar zorunlu değil |
| **Hades** | CLARITY hedef: room-based run akışı, door/exit netliği, encounter arena netliği |
| **Curse of the Dead Gods** | ATMOSPHERE referansı, AMA dikkatli: çok karanlık + düşük kontrastlı yerlerden uzak dur (chibi 64×64 readability korunmalı) |

---

## Top 5 risk + mitigation (sentez)

1. **Visual fatigue** (tüm odalar aynı görünür) → Backwall landmark + theme prop cluster + opsiyonel renk paleti shift (Ritual = mor, Crypt = yeşil)
2. **PixelLab stil drift** (sheet'ler arası ton uyumsuzluğu) → Master color palette + ChatGPT_TOPDOWN ref attach + Aseprite post-quantize
3. **Combat readability kaybı** (cyan crack + amber light VFX ile karışır) → Base decor emissive DÜŞÜK, VFX emissive YÜKSEK hard rule
4. **Backwall scope creep** (5 boss room = 5 yeni card) → MVP monolitik test sonrası modular wall shell (S102 LATE) reactivation karar et
5. **Y-sort bug** (char prop önünden geçemiyor) → Room 1 MVP'de önce çöz, CompositeSortGroup + transform.y axis sort

---

## MVP plan (sentez, 11 step)

| Step | Aksiyon | Sahip | Çıktı | Blocker? |
|---|---|---|---|---|
| 1 | Final karar lock — bu doc | Claude → user onayı | Locked direction | YES |
| 2 | PixelLab Sheet A (4 floor tile, 64²) | User web UI | `STAGING/concepts/fractured_chamber/sheet_a_floor.png` | YES |
| 3 | PixelLab Sheet B (4 edge variant, 128×64 + 64×128 + 64² × 2) | User web UI | `STAGING/concepts/fractured_chamber/sheet_b_edge.png` | YES (paralel ile #2) |
| 4 | Codex: crop + transparent BG + Unity import (PPU 64) + Sprite Atlas | Codex bg dispatch | `Assets/Art/FracturedChamber/floor/*.png` + `edges/*.png` + .meta | NO (paralel #5 ile) |
| 5 | Aseprite: 2 cyan rift crack decal hand-paint | User Aseprite (~10 dk) | `Assets/Art/FracturedChamber/decals/*.png` | NO (paralel #4 ile) |
| 6 | Codex: Shattered Keep Combat Room — RoomTemplate ScriptableObject + 5×5 prefab compose + URP 2D Light setup | Codex bg dispatch | `Assets/Prefabs/Rooms/ShatteredKeep_Combat_v1.prefab` + scene `Assets/Scenes/Demo/CombatRoom_MVP.unity` | YES (4+5 done) |
| 7 | User playtest Room 1: hero sprite walk + 6-10 dummy enemy + combat readability | User | PASS/FAIL note | YES |
| 8 | PASS → PixelLab Rift Gate landmark card (512×384) + Sheet C (4 prop: brazier, pillar, sarcophagus, rubble cluster) | User web UI | 2 dosya | NO (paralel) |
| 9 | Codex import + Rift Gate Chamber + Ritual Chamber prefab compose | Codex bg | 2 prefab + 2 scene | YES (8 done) |
| 10 | User playtest 3 oda side-by-side | User | MVP DONE veya iterate | YES |
| 11 | Claude (Sonnet) → rima-doc dispatch: NLM sync + memory update + CURRENT_STATUS S103 | Claude orchestrator | Memory locked | END |

**Paralel pencereler:** #2-#3 (PixelLab sheet'ler), #4-#5 (Codex import + Aseprite decal), #8 paralel-internal. Geri kalan sequential.

**Tahmini süre:** ~1-1.5 session (S103-S104). User PixelLab ~2 saat, Codex compute ~3-4 saat, playtest ~1 saat.

**"MVP done" kabul kriteri:**
- 3 oda arketipi Unity'de prefab + scene olarak hazır
- 64×64 chibi hero sprite oda içinde dolaşabiliyor
- 6-10 enemy combat readability subjective PASS
- Y-sort doğru çalışıyor (char prop önünden/arkasından geçebilir)
- Cyan crack base emissive < telegraph VFX emissive (görsel onay)
- Same asset grammar 3 odada da çalıştı (Room 4-5 üretimi tahmin edilebilir)

---

## İş bölümü (LOCK)

| Görev | Sahip |
|---|---|
| PixelLab görsel üretim | **User web UI** (HARD RULE: no autonomous night) |
| Aseprite hand-paint decal | **User** (~5-10 dk/decal) |
| PNG crop + transparent BG + Unity import + .meta | **Codex (cx_dispatch.py background)** |
| ScriptableObject + Prefab compose + 2D Light setup | **Codex** |
| Color palette QC + visual review | **Claude orchestrator** |
| Playtest verdict | **User** |
| Design doc + memory + NLM sync | **Claude → rima-doc dispatch** |
| Y-sort bug debug | **Codex** + user verify in Unity |

---

## Demote / revoke / paused

| Konu | Status | Sebep |
|---|---|---|
| Full modular high wall 2D (Option A) | **REVOKED** | S98-101 denendi, seam + cell discrimination kalıcı sorun, fractured chamber'a geç |
| 2.5D hybrid room shell (Option B) | **REVOKED** | S57-58 zaten REVOKED, pixel-perfect + sorting + lighting riskler orantısız |
| Modular Wall Shell MVP (S102 LATE, 4×4 sheet) | **DEMOTED → yan track** | Sadece north backwall landmark band için; full perimeter için DEĞİL |
| Local Flux LoRA training (paused at step 30) | **PAUSED — DEFER** | MVP başarı sonrası tekrar değerlendir, hand-curated baseline yeterli |
| Combat density 20+ aktör (concept art 21_29_23 (3)) | **REVOKED** | Hard cap 10 aktör (gameplay ≠ concept) |

---

## Sonraki adım (user onayı bekliyor)

Bu kararı onaylıyorsan:
1. Bu doc memory'e canonical olarak sync edilir (rima-doc dispatch)
2. Step 2 başlar → PixelLab Sheet A prompt'unu yazıp sana sunarım, sen web UI'da üretirsin
3. Sheet geldikçe Codex sıraya alır

**Tek sorum:** Modular Wall Shell MVP (S102 LATE) — sen onunla devam etmek mi istiyorsun yoksa fractured chamber MVP başarısı sonrasına bırakalım mı? (Demote demek için onay lazım)
