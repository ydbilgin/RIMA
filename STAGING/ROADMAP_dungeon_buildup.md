# RIMA Dungeon Buildup Roadmap (Piece-by-Piece, 35° Lock)

**Hedef:** `Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v4_inside_dungeon.png` (in flight, v3'ten sonra user feedback ile revize) — RIMA-specific concept (Hades reference DEĞİL).
**Lock:** Karar #100 35° camera angle preserved + Karar #150 fake iso pipeline candidate (diamond shape DROPPED, internal walls + irregular layout + perimeter off-screen LOCKED).
**Map size hedef:** 30+ × 20+ world tiles (v3 18×12 arena REJECTED).
**Disiplin:** Bir önceki faz pass etmeden sonraki faza geçmek YASAK. Her faz tek artifact + tek karar.
**User constraint:** No manual pixel art (user cannot draw). All progress via existing sprite + Unity composition + PixelLab regen.

---

## Faz 0 — Karar lockları (DONE bu konuşmada)

| Karar | Status |
|---|---|
| Camera angle 35° (v2 reference) | LOCKED — v1 50-60° REJECTED |
| Asset class count = 5 (top/bottom hero + side strip + arch + corner + accent pool) | LOCKED |
| Build order: skeleton → floor → decoration → lighting → post-FX → variant | LOCKED |
| Per-phase gate: tek screenshot + pass/regen/revize kararı | LOCKED |
| Scope creep YASAK: bir faz, tek tip iş | LOCKED |

---

## Faz 1 — Skeleton (NEXT)

**Hedef:** Mevcut sprite'larla 5 SR yerleştir, v2 silhouette match'liyor mu test et.

**Sahne mutasyonu:**
- `Spawn_01_NewTileSystem/L3_Walls/`'i tamamen temizle (Wave E placement'lar dahil)
- Yerine SADECE şunlar:
  - Top hero wall: `pixellab_wall_section_horizontal` (1 SR, full width)
  - Bottom hero wall: aynı sprite, flipY (1 SR, full width)
  - Left side: `pixellab_wall_arch_section` (1 SR, arch entegre)
  - Right side: aynı sprite, flipX (1 SR, arch entegre)
  - Optional corner pieces if needed (4 × `gate_arch`)
- L1 floor BOŞ kalır (Faz 2'ye kadar)
- Brazier, vine, character SCENE'de kalır ama OFF
- Sadece L3 walls görünür screenshot çıkar

**Artifact:** `STAGING/skeleton_v1_spawn01.png`

**Karar gate:** v2 ile yan yana baktığında silhouette tutuyor mu?
- ✅ **PASS** → Faz 2 floor'a geç
- 🔴 **FAIL** → Hangi sprite yetmiyor? (top wall depth? side strip thickness? arch position?) → tek-tip regen prompt yaz → 1-2 PixelLab gen

**Yasak:** floor paint, decoration ekleme, lighting değiştirme, post-FX, animation. Sadece 5 SR + screenshot.

---

## Faz 2 — Floor (skeleton PASS sonrası)

**Hedef:** Floor blackness'i öldür, paved stone surface kur.

**Sahne mutasyonu:**
- `Assets/Art/Tiles/F1/Tilesets/` altındaki 11 Wang tileset incele → en yakın v2 paved stone match'i seç (gri-mavi tinted, moss-friendly)
- L1_BaseFloor tilemap'i seçilen Wang tileset ile uniform paint et (18×12 cell)
- Hiçbir overlay/patch/scatter ekleme — sadece base tile

**Artifact:** `STAGING/skeleton_v2_floor_spawn01.png`

**Karar gate:** Floor tonality + repetition v2 ile tutuyor mu?
- ✅ **PASS** → Faz 3 decoration'a geç
- 🔴 **FAIL** → Hangi sorun? (renk yanlış / pattern çok belli / moss yok) → tek-tip regen veya overlay layer eklenebilir

**Yasak:** L2 overlay, L4 patches, L5 scatter — Faz 3'e kadar yok. Sadece L1.

---

## Faz 3 — Wall Decoration (floor PASS sonrası)

**Hedef:** Üst/alt wall'a v2'deki chain + banner + candle eklemek.

**Sahne mutasyonu:**
- Mevcut accent envanteri kontrol: `Assets/Art/AssetPacks/Act1_ShatteredKeep/` altında chain / banner / candle var mı?
  - YOK ise tek-tip dispatch: PixelLab gen (chain × 2, banner × 2, candle wall-mount × 1 = ~5 gen)
  - VAR ise direkt yerleştir
- L3 hero wall'lara overlay olarak ekle (her duvar 2-3 accent)
- Cyan rift glow sprite (mevcut + yeni) — arch içine + 1 wall crack noktasına
- Warm candle sprite (mevcut painterly_prop var mı?) — alt wall + corner

**Artifact:** `STAGING/skeleton_v3_decoration_spawn01.png`

**Karar gate:** Wall density v2 ile match'liyor mu?

**Yasak:** lighting setup, post-FX, contact shadow — Faz 4-5'e kadar yok.

---

## Faz 4 — Lighting (decoration PASS sonrası)

**Hedef:** Pure black ambient → dim dungeon mood.

**Sahne mutasyonu:**
- Global Light2D ekle: Intensity 0.15-0.20, Color `#1a1a2e` (dark navy)
- Per cyan rift: Point Light2D, Color `#00FFCC`, Intensity 1.2, Outer 2.0, Inner 0.5, Blend Additive
- Per warm candle: Point Light2D, Color `#FF9966`, Intensity 0.8, Outer 1.5, Inner 0.3

**Artifact:** `STAGING/skeleton_v4_lighting_spawn01.png`

**Karar gate:** Ambient + accent + warm/cool balance v2 ile match'liyor mu?

**Yasak:** post-FX volume, color grading, vignette — Faz 5'e kadar yok.

---

## Faz 5 — Post-FX (lighting PASS sonrası)

**Hedef:** Sprite collage → filmic dungeon.

**Sahne mutasyonu:**
- URP 2D Renderer Volume Profile ekle/edit:
  - Bloom: Threshold 1.1, Intensity 0.4, Scatter 0.6 (HDR emissive sprite'lara opt-in)
  - Vignette: Intensity 0.4, Rounded, Color `#000000`
  - Color Grading mode: HDR
  - Tonemapping: ACES
  - LUT: cool blue-grey shadow + warm desaturated mid (TODO: actual LUT texture gen)
- Contact shadow blob prefab — her prop/character altına child SR (64×32 ellipse, 35% black, Sorting Layer Floor +1)

**Artifact:** `STAGING/skeleton_v5_postfx_spawn01.png`

**Karar gate:** Fidelity 80%+ mı?
- ✅ **PASS** → Faz 6 variant rules'a geç
- 🔴 **FAIL** → Specific dimension hangileri çekiyor? Tek-tip iyileştirme

---

## Faz 6 — Variant Rules (single room PASS sonrası)

**Hedef:** Sub-room sequence (Karar #149) için 3 oda variant compose et.

**Variation table (8 boyut):**

| Boyut | Range |
|---|---|
| Arch count | 0/1/2 |
| Cyan rift count | 1-4 |
| Cyan rift position | corner/wall/center |
| Warm candle count | 0-3 |
| Scatter density | sparse/medium/dense |
| Wall accent (chain/banner) | 0-4 her duvarda |
| Floor patch type | crack/water/moss/blood/bare |
| Color tint per sub-room | base/+blue/+purple/+green |

**Sahne mutasyonu:**
- Spawn_02 + 1 yeni sub-room (Spawn_03_NewTileSystem) compose
- Spawn_01 = entry warmup, Spawn_02 = combat pocket, Spawn_03 = final mix (Karar #149 minimum 3-room slice)
- Her odanın 8-boyut profili `RoomTemplateSO.metadata` veya benzer SO field'a kaydedilir

**Artifact:** 3 screenshot + 1 sub-room sequence playthrough video

**Karar gate:** 3 oda **görsel olarak farklı ama aynı asset pool** kullanıyor mu?

---

## Out-of-scope (bu roadmap'te yok, ama sonra)

- Animation (brazier breath, rift pulse, banner sway) — Faz 5 sonrası
- Particles (dust puff, ember, mote) — Faz 6 sonrası
- VFX combat (skill effects) — separate track
- Enemy spawning + behavior — Codex Step 6 (Karar #149 vertical slice)
- Encounter pacing (breather room) — Karar #149 expansion
- Act 2 / Act 3 asset packs — Act 1 LIVE sonrası

## Disiplin reminderları

1. **Faz atlama YASAK.** Skeleton PASS olmadan floor'a, floor PASS olmadan decoration'a geçilmez.
2. **Scope creep YASAK.** Bir faz, tek tip iş. "Bunu da ekleyelim" düşüncesi = STOP, sonraki faza ertele.
3. **Regen disiplini:** PASS olamayan fazda **tek-tip regen** önerisi yaz (kaç gen, hangi prompt, hangi dosya). 5+ gen'lik "her şeyi yeniden çiz" yasak.
4. **Karar #100 35°** locked — v1 50-60° tekrar gündeme gelirse hard rule violation.
5. Mevcut sprite envanterini önce kullan — regen sonradan.

## Orchestrator dispatch queue (sıralı)

1. **Faz 1 dispatch:** Antigravity → `STAGING/ANTIGRAVITY_PROMPT_skeleton.md` (aşağıda yenisi)
2. **Faz 1 verdict:** PASS → Faz 2 dispatch, FAIL → regen task
3. **Faz 2-6:** sırayla tek tek

Bu döküman canlı — her faz sonrası status update.
