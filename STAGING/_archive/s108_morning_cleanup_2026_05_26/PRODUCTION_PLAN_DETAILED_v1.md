# RIMA Production Plan Detailed — v1 (Evidence-Backed)

> **Author:** rima-design (Opus)
> **Status:** Codex review PASS (no blocking revisions, 2 optional cleanups applied inline). User Antigravity review pending. Pilot Batch 1.1 onay bekliyor.
> **Date:** 2026-05-20 (S95 LATE NIGHT)
> **Master spec parent:** [`STAGING/OBJECT_PRODUCTION_MASTER_SPEC_v1.md`](./OBJECT_PRODUCTION_MASTER_SPEC_v1.md) (v2 LIVE)
> **Wall+object plan parent:** [`STAGING/PRODUCTION_PLAN_WALLS_OBJECTS_S95.md`](./PRODUCTION_PLAN_WALLS_OBJECTS_S95.md)
> **Codex review:** [`STAGING/CODEX_DONE_production_plan_review_v1.md`](./CODEX_DONE_production_plan_review_v1.md) — VERDICT PASS, pilot dispatch ready YES
> **Budget remaining:** 2,433 / 5,000 gen (subscription active, $0 credit pool — production hard-capped by subscription)

> **Object ID notasyon notu (Codex cleanup):** Evidence/reference bölümlerinde **kısaltılmış 8-karakter prefix'ler** kullanılır (kompakt envanter referansı için, `list_objects` çıktı format'ı). **Dispatch-critical parent ID'ler** (Batch 1.5 `create_object_state` parent gibi) **tam UUID** form'da verilir. Kısaltılmış prefix'i dispatch payload'a kopyalama.

> **Prompt copy/paste notu (Codex cleanup):** Bu plan'daki "exact prompt" bölümleri PixelLab dispatch payload'ı için temiz. Evidence prose'unda geçen 3rd-party isimleri ("Hades-style") veya genre label ("dark fantasy game skill icon" envanter ismi) **prompt payload'a kopyalanmayacak** — sadece tarihsel referans.

> [!IMPORTANT]
> **Sorting Layer Reconciliation:** Atomic layer clean-up ([CODEX_DONE_wall_alignment_layer_cleanup_atomic_s95.md](file:///F:/Antigravity Projeler/2d roguelite/RIMA/STAGING/CODEX_DONE_wall_alignment_layer_cleanup_atomic_s95.md)) sonrasında projenin canonical sorting layer seti `[Default, Ground, Walls, Entities, VFX]` olarak kesinleşmiştir. Bu belgedeki (v1) tüm tarihsel "Floor" katman/sorting layer referansları teknik uygulamada **Ground** layer'ı olarak ele alınmalıdır.

---

## 1. PixelLab Envanter Özeti (Evidence Layer)

`list_objects` çağrıları (4 page, offset 0/50/100/150/200), `list_characters`, `list_isometric_tiles`, `list_topdown_tilesets`, `list_tiles_pro` ile çekilen toplam envanter:

| Kaynak | Toplam | Status notu |
|---|---|---|
| `list_objects` | **225 object** | 5 sayfa (offset 0/50/100/150/200) |
| `list_characters` | **17 character** | 10 canonical anchor + 7 deneme |
| `list_isometric_tiles` | **4 tile** | 64px granite floor variants (S95 isometric pivot) |
| `list_topdown_tilesets` | **25 tileset** | Wang16 deneyleri (Karar #142 → split LOCK CB'ye taşındı) |
| `list_tiles_pro` | **55 tile** | create_tiles_pro deneyleri (4-type batch) |

### 1a. Object Kategori Breakdown (225 total)

| Kategori | Tag / Pattern | Sayı | Yorum |
|---|---|---|---|
| Act 1 mob silhouettes | `act1_mob_s95` | 16 | Skeletal hand / chibi mob batch S95 |
| Act 1 wall pieces (büyük) | `act1_wall_pieces_s95` | 4 | 128x128 — **CRITICAL REFERENCE** (4 farklı stil çıktı) |
| Act 1 statue / ritual | `act1_statue_ritual_s95` | 14 | 64x64 — treasure pile + cobblestone + decoration |
| Act 1 mounting apparatus | `act1_mounting_apparatus_s95` | 16 | 64x64 — **CRITICAL** wall mount + lit torch örnekleri |
| Act 1 named props (60-100 range) | `Act 1 Shattered Keep ...` (no tag) | ~25 | Pillar 128, archway 128, ritual 128, wall_section 128, prison 96, statue 96, mob ölü 192 — geniş çeşitlilik |
| Painterly (generic dawn-pipeline tests) | `Painterly pixel art` | ~20 | 64/128 mixed, Karar #143 layered pipeline test fragments |
| Weapons (S82) | `weapon_*_s82` | 6 + 6 review | Hexer staff / Gunslinger pistol / Shadowblade dagger / Ronin katana / Ravager greatsword |
| Other weapons | unnamed | 4 | Soul lash, hammer, compound bow, greatsword |
| Rift / decor (cliff/scatter) | `tile_*_s82`, `scatter_*_f1` | ~20 | Rift pool, cliff drop, dirt patch, rubble, moss, stones |
| Keep wall/decal V2 (S88) | `keep_decal_v2`, `keep_wall_v2` | ~12 | 32x32 deneme |
| Alabaster Dawn (S92) | `alabaster_*` | 8 | 32x32 decal + wall |
| Hitspark / dash VFX | unnamed | 2 (anim) | 11127e69 hitspark, 58c183a0 dash trail — **VFX EVIDENCE** |
| Scene mockup (16:9) | unnamed | 10 | 256x256 — Antigravity inspo mockups |
| Skill icon batch | `dark fantasy game skill icon` + Karar #79 6 cross-class | ~24 | 64x64 — UI Faz 4 reference |
| Misc (broken pillar, dust patch, rift pool) | unnamed | ~10 | Rubble heap 80, pillar segment 64x96, rift dust 64, hexagonal scatter |

### 1b. Key Reference Objects (visual inspection done)

**Wall references (act1_wall_pieces_s95 4-piece batch, NO `view` param, NO `item_descriptions`):**

| ID | Şey | Görsel gözlem | Drift verdict |
|---|---|---|---|
| `65c99904` | flat brick grid | 4×4 stone tile pattern, **flat top-down floor görünümü** | DRIFT — wall değil zemin gibi |
| `8530799c` | L-corner stone | Köşe parça **flat top-down** L şekli | DRIFT — köşe ama dikey değil |
| `a52f6711` | Stone archway | **Frontal billboard**, simetrik kemer | **WANTED STYLE** — Hades-style ortografik |
| `abf9c178` | Cracked wall + cyan rift | **Frontal billboard**, dikey mortar lines, cyan crack | **WANTED STYLE** — billboard, granite + cyan accent |

**Conclusion:** `view` parametresiz batch 4'ten 2'si billboard, 2'si flat. Stil tutarlılığı **%50**. Master spec Karar #2 PILOT (`view="side"` explicit) doğru karar — drift'i sıkılaştırır.

**Pillar/Archway references (no tag, 128x128):**

| ID | Şey | Görsel gözlem | Verdict |
|---|---|---|---|
| `3b5bc2c7` | Intact granite pillar | **Tam dikey billboard**, base + capital + cyan veins, "high top-down" tarzı ama frontal | GOLDEN reference — Faz 1.6 pillar batch base |
| `3077db6c` | Narrow archway w/ cyan portal | Dikey kemer + portal glow, billboard | Faz 1.1 arch piece için kuvvetli kanıt |
| `36f3331f` | Wider archway w/ cyan portal | Symmetric kemer + portal, billboard | Aynı |

**Mounting apparatus reference (act1_mounting_apparatus_s95, 16 piece batch, 64x64):**

| ID | Şey | Görsel gözlem | Verdict |
|---|---|---|---|
| `7227fa35` | Dagger mounted | Tek dagger blade — beklenmedik (mounting hardware değil weapon olarak yorumlandı) | DRIFT — prompt "pure mounting" cue yetersiz |
| `f88e821a` | **Wall sconce with LIT torch** | Side-view bracket + turuncu alev | **STAR REFERENCE** — Faz 1.5 Void Flame için Act 1 cyan repaint hedefi |

**Conclusion:** 16-piece mounting batch karışık çıktı. Bazıları weapon yorumlandı. Wall_decoration_pure_attachment_only LOCK (Karar) için **lit torch örneği f88e821a** mevcut — Void Flame'in Act 1 cyan versiyonu bu base'den `create_object_state` ile türetilebilir, **yeni dispatch gereksiz olabilir**.

**Statue / Ritual reference (act1_statue_ritual_s95, 14 piece batch, 64x64):**

| ID | Şey | Görsel gözlem | Verdict |
|---|---|---|---|
| `e899a33d` | Treasure pile | Sarı altın + draba küme | Floor clutter ref |
| `f1ed6cce` | Cobblestone floor pattern | Düz top-down zemin grid (taş dösemesi) | DRIFT — sandalyenin tanrıçası değil, zemin gibi |

**Conclusion:** Statue batch yarı drift. RIMA için "decoration object" semantically zayıf — Faz 1.6 interior ruined pieces ÖNCESİ açıklama daha net olmalı.

**Mob reference (act1_mob_s95, 16 piece batch, 64x64):**

| ID | Şey | Görsel gözlem | Verdict |
|---|---|---|---|
| `e8695fff` | Skeletal hand | Top-down beige skeleton el, isolated | Mob silhouette örneği — kullanılabilir ama mob "tam karakter" değil "el" |

**Conclusion:** Mob batch drift — "mob enemy" prompt'u el / parça olarak yorumlandı. Faz 2 (karakter / mob V3 web UI USER manual) **doğru karar** — MCP batch ile drift çok yüksek.

**VFX reference (animations, 64x64):**

| ID | Şey | Görsel gözlem | Verdict |
|---|---|---|---|
| `11127e69` | Hitspark cyan burst | 7-frame anim, sharp radial flare | **HIGH QUALITY** — `animate_object` kanıtı, Faz 3 slash VFX için worth çoğaltmak |
| `58c183a0` | Cold blue dash trail | 9-frame anim, dynamic swirl | **HIGH QUALITY** — Faz 3 hareket trail için kanıt |

**Conclusion:** `animate_object` pipeline GÜVENİLİR. Faz 3 VFX yüksek başarı ihtimali var.

**Rift / decor reference:**

| ID | Şey | Görsel gözlem | Verdict |
|---|---|---|---|
| `60502d16` | Violet rift dust | view="high top-down", soft mor bulut | Rift state evidence — Karar #5 state_of overlay |
| `6b52751d` | Broken stone pillar | view="high top-down", 64x96 dikey kırık sütun | Faz 1.6 ruin piece için kanıt |

### 1c. Tile / Tileset Notu

- 25 `topdown_tileset` + 55 `tiles_pro` — büyük çoğunluğu **Karar #142 (Hades-Style + CB Wang Split LOCK)** öncesi deneme. RIMA için artık YASAK pipeline (Wang16 CB'ye taşındı).
- 4 `isometric_tile` — S95 isometric pivot LIVE, kullanımda (granite floor PathC).
- **Üretim planı yeni tileset üretmeyecek** — mevcut isometric_tile yeterli. Faz 0 kapsam dışı.

### 1d. Character Notu

- 10 canonical anchor PixelLab ID locked (Karar Canonical Char Roster v2)
- 7 deneme character (Slight forward lean, samurai, Young woman mage vs.) — production değil
- **Faz 2 MCP dispatch YASAK** — `feedback_pixellab_character_via_web_ui_v3` HARD LOCK gereği user V3 web UI'da manual gen

---

## 2. Faz 1 — Demo MVP Asset (Görsel Bütünlük)

**Amaç:** Demo room playable seviyesinde görsel komple. Mevcut character batch (Warblade anchor) yeterli, mob da V3 web UI'a delegate edildi (Faz 2). Bu faz **PixelLab object dispatch'e odaklı**.

**Total range:** ~190-280 gen reserve (master spec range + risk buffer)

**Pilot zorunlu:** Batch 1.1 ilk dispatch — `view="side"` + `item_descriptions` field forward doğrulaması.

---

### Batch 1.1 — Wall Face Pack (Template A v2) — **PILOT**

> [!NOTE]
> **Sorting Layer Note:** Bu batch ile üretilen ve yerleştirilen tüm duvar/zemin elemanları projenin güncel canonical sorting layer seti (`[Default, Ground, Walls, Entities, VFX]`) uyarınca `Walls` ve `Ground` layer'larına atanacaktır (eski "Floor" layer'ı yerine `Ground` kullanılmalıdır).

#### Tool + Parameters
- **Tool:** `mcp__pixellab__create_object`
- **Params:**
  - `size=128`, `view="side"`, `directions=1`, `n_frames=4`, `object_view=None`
  - `item_descriptions=[…4 entries…]` (her piece için per-frame identity)

#### Exact Prompt Text

**`description` (shared style anchor, master spec Karar #7 formülü):**
```
ancient stone keep wall pieces, granite gray #3A3D42 base with cyan #00FFCC mineral veins, weathered mortar lines, exposed lighter stone #5A5F66, tall vertical wall billboard from side perspective filling most of 128px canvas height with narrow transparent margins, painterly pixel art, no outline, isolated on transparent background
```

**`item_descriptions` (per-frame, n_frames=4):**
```
[
  "wall face south view, flat single facet, granite weathered, cyan veins along mortar",
  "wall face east view, perpendicular flat single facet, granite weathered, cyan veins along mortar",
  "outer corner piece, two granite facets meeting at 90 degrees both visible, weathered cyan",
  "arched doorway opening, rough archway through granite wall with stone keystone, cyan accent"
]
```

#### Reference / Evidence

- **`a52f6711` (act1_wall_pieces_s95, 128, view=n/a)** — Stone archway frontal billboard çıktı. View parametresi olmadan **şans eseri** Hades-style billboard çıktı. Bu kanıt `view="side"` explicit ile daha kontrollü tekrar üretilebilir.
- **`abf9c178` (act1_wall_pieces_s95, 128, view=n/a)** — Cracked wall frontal billboard + cyan rift accent. Aynı şans eseri Hades-style. Color HEX (#00FFCC) prompt'ta yok ama çıktıda **doğru cyan tonu yakalanmış** — bu Act 1 material adıyla cued olduğunu gösterir.
- **`3b5bc2c7` (intact pillar, 128, view=n/a)** — Tam dikey billboard, granite + cyan veins. **Bizim batch'in beklenen kalitesinin altın referansı**. Pillar geometry değil ama palette + canvas occupancy bu seviyede.
- **`65c99904` (flat brick grid)** + **`8530799c` (flat L-corner)** — DRIFT örnekleri. `view` param eksikliği yüzünden top-down çıktı. Bizim batch'te `view="side"` explicit bu drift'i önler.

**Pilot risk:** Daha önce **NoVIEW** ile 4 piece batch'ten 2/4 başarı (50% billboard rate). **`view="side"` explicit ile expected 4/4 billboard** — ama bu API'nin item_descriptions forward'una bağlı, kanıtlı değil.

#### Beklenen Output
- 4 candidate (n_frames=4, status="review")
- Her frame 128×128 transparent BG
- Stil tutarlılığı: tüm 4 piece granite + cyan + dikey billboard
- `get_object` → inline preview → görsel review

#### Bütçe + Sıra
- **Reserve gen:** 40 (upper bound, master spec Karar #9 range 25-40)
- **Sıra:** 1.1 — Faz 1'in ilk batch'i, **bu plan'ın PILOT'u**

#### Risk + Pilot Strategy

**Risk 1 — `item_descriptions` field MCP wrapper'da forward edilmiyor olabilir:**
- Doğrulama: dispatch sonrası `get_object` her frame'i ayrı identity ile gösterirse forward OK; aynı 4 kopya görünürse forward FAIL.
- **Plan B (FAIL):** Direct REST API path kullan (master spec Codex Iter 2 caveat — REST dispatch validated)
- **Plan C (REST fail):** 4 ayrı `create_object` n_frames=1 dispatch (face_S, face_E, corner, arch ayrı). Cost: 4 × ~20 = 80 gen (2x pahalı, ama %100 control).

**Risk 2 — Stil drift (4 piece tutarsız):**
- Drift varsa: `select_object_frames(indices=[best])` ile iyileri al, zayıf piece'i ayrı n_frames=1 redo.

**Risk 3 — `object_view=None` zayıf çıktı:**
- A/B test: aynı prompt `object_view="sidescroller"` ile düşük cost (15-25 gen) redo.
- **YASAK:** `object_view="top-down"` side wall ile (master spec Karar #6 untested blend).

**Risk 4 — Color HEX taşımıyor:**
- `abf9c178` örneğinde HEX prompt'ta olmasa da cyan tonu doğru — model "ancient stone keep" tematik anchor'ı tanıyor. Risk DÜŞÜK.

---

### Batch 1.2 — Wall Damaged Variants (Yeni Object Batch, NOT state_of)

#### Tool + Parameters
- **Tool:** `mcp__pixellab__create_object`
- **Params:** `size=128`, `view="side"`, `directions=1`, `n_frames=4`, `object_view=None`
- `item_descriptions`: 4 damaged variant

#### Exact Prompt Text

**`description`:**
```
ancient stone keep damaged wall pieces, granite gray #3A3D42 base with cyan #00FFCC mineral veins, broken top profile with scattered stones, exposed lighter stone #5A5F66 inner core, tall vertical wall billboard from side perspective filling most of 128px canvas height with narrow transparent margins, painterly pixel art, no outline, isolated on transparent background
```

**`item_descriptions`:**
```
[
  "wall face south damaged, upper third broken away, jagged silhouette top, granite cyan veins exposed in fracture",
  "wall face east damaged, perpendicular facet with upper half collapsed, broken edge, weathered cyan crack",
  "outer corner damaged, one facet shorter than the other from collapse, two granite walls meeting at 90 degrees",
  "ruined half wall, only lower portion remains, broken top with scattered debris on top edge, granite cyan veins"
]
```

#### Reference / Evidence

- **`abf9c178` (cracked wall + cyan rift, 128)** — Cyan crack overlay kanıtı, ama silhouette **intact**. Bu batch silhouette **değişik** olmalı (Karar #5 — collapsed silhouette = yeni object, state_of yetmez).
- **`6b52751d` (broken pillar, 64x96, view="high top-down")** — Üst kırık silhouette örneği. Pillar olmasına rağmen "broken top" kavramı görsel kanıtlı.
- **Pilot risk:** Damaged silhouette + side billboard kombinasyonu **kanıtlı değil** (mevcut wall örnekleri intact veya pillar). Drift olası.

#### Beklenen Output
- 4 candidate, 128×128 transparent BG
- Her piece: üst kısmı kırık silhouette farklı, alt yarı granite + cyan korunur
- Bu 4 piece, Batch 1.1 4 piece ile **eşleşmeli** (her intact'a karşılık damaged)

#### Bütçe + Sıra
- **Reserve gen:** 40
- **Sıra:** 1.2 — Batch 1.1 pilot sonrası fire (1.1 başarıysa)

#### Risk + Pilot Strategy

- Plan B (Batch 1.1 başarısızsa): Bu batch ertelenir, Plan B/C re-strategy 1.1'e uygulanır.
- Drift varsa: Half-wall iyi çıkarsa diğer 3 piece daha tutarlı n_frames=1 ayrı dispatch (Plan C). 80 gen toplam.
- `item_descriptions` zaten Batch 1.1 pilotunda doğrulanmış olur — burada tekrar test yok.

---

### Batch 1.3 — Wall Mountings (Template C v2)

#### Tool + Parameters
- **Tool:** `mcp__pixellab__create_object`
- **Params:** `size=64`, `view="side"`, `directions=1`, `n_frames=16`, `object_view=None`
- `item_descriptions`: 16 entry (master spec Template C v2 array kullanılır)

#### Exact Prompt Text

**`description`:**
```
Act 1 Shattered Keep wall-mounted decorations, iron #2A2D32 + bronze #8B7355 + cyan accent #00FFCC palette, weathered painterly pixel art, no outline, isolated on transparent background, side perspective wall-attached items with mounting hardware, decoration only without wall background
```

**`item_descriptions`** (16 entry — master spec Template C v2'den, no changes):
```
[
  "iron sconce empty bracket, wrought iron #2A2D32 mount",
  "iron chain hook short, weathered wrought iron",
  "torch wall bracket weathered, iron #2A2D32 with rust",
  "small ceremonial pennant cyan #00FFCC trim, cloth banner",
  "bronze #8B7355 decorative plaque, embossed sun motif",
  "iron #2A2D32 candle holder twin arms, drip catcher",
  "hanging chain link short, wrought iron #2A2D32 links",
  "small stone gargoyle head mount, weathered granite",
  "iron #2A2D32 ring door knocker, ornate edge",
  "bronze #8B7355 ceremonial mask wall mount, blank eyes",
  "small wooden shield round wall display, iron rim",
  "iron #2A2D32 weapon rack empty, two hooks",
  "hanging skull trophy small, bone white with iron chain",
  "bronze #8B7355 sun emblem wall plaque, radiant rays",
  "small lantern hook unlit, iron #2A2D32 with chain",
  "cyan #00FFCC rift crystal wall growth small, void mineral"
]
```

#### Reference / Evidence

- **`act1_mounting_apparatus_s95` 16-piece batch** (7227fa35, f88e821a, f7763267 etc.) — Mevcut 16-piece batch, view=n/a, **karışık çıktı**:
  - `f88e821a` = Side-view bracket + lit orange torch — **STAR örnek**, prompt "decoration only without wall background" cue'su iyi çalışmış
  - `7227fa35` = Tek dagger blade — DRIFT, "mounting apparatus" prompt zayıf, weapon yorumlandı
  - Diğer 14 piece muhtemelen benzer drift mix

- **Bizim batch'in farkı:**
  - `view="side"` explicit (önceki batch view=n/a)
  - **Wall_decoration_pure_attachment_only LOCK** uyumlu — "decoration only without wall background" cue strict
  - 16 farklı item açık ifade (önceki batch'in net prompt'u bilmediğimiz için kıyaslanamaz)

**Pilot risk:** 16-frame batch dispatch'in tutarlı stil + 16 farklı identity verme kabiliyeti **kanıtlı değil**. Master spec Karar #4 production rule LIVE ama henüz dispatched değil.

#### Beklenen Output
- 16 candidate, 64×64 transparent BG
- Stil tutarlılığı + her item ayrı identity
- `select_object_frames(indices=[en iyi 8-10], common_tag="act1_wall_mount_s95_v2")` ile finalize

#### Bütçe + Sıra
- **Reserve gen:** 40 (master spec Karar #9 range 25-40)
- **Sıra:** 1.3 — Batch 1.1 + 1.2 pilot'ları başarı sonrası

#### Risk + Pilot Strategy

- **Risk 1 — 16-frame stil drift:** Pilot 1.1'de 4-frame doğrulandıysa 16-frame ekstrapolasyon **orta risk** (frame sayısı arttıkça stil tutarlılığı azalır olası).
- **Plan B:** 2× 8-frame batch (mounting type bölme: 8 light + 8 heavy)
- **Plan C:** 4× 4-frame batch (16 piece'i mantık gruplara böl: 4 sconce + 4 plaque + 4 chain + 4 ornament)
- **`item_descriptions` array length = n_frames** (master spec Karar #4 deterministic kontrol) — 16 entry zorunlu

---

### Batch 1.4 — Floor Clutter (Template B v2)

#### Tool + Parameters
- **Tool:** `mcp__pixellab__create_object`
- **Params:** `size=32`, `view="low top-down"`, `directions=1`, `n_frames=64`, `object_view="top-down"`
- `item_descriptions`: 64 entry (master spec Template B v2 array tam kullanılır)

#### Exact Prompt Text

**`description` (shared anchor):**
```
Act 1 Shattered Keep dropped items and floor clutter on stone floor, cyan #00FFCC + gold #C9A227 + crimson #8C1F1F accent palette, weathered painterly pixel art, no outline, isolated on transparent background, top-down view dropped on stone floor, small footprint sprite
```

**`item_descriptions`** (64 entry — master spec Template B v2'den TAM listede, 16 base + 3 variant pass):
```
[
  # Base 16 items (lines 1-16)
  "gold coin pile small, golden #C9A227 stack",
  "silver coin pile small, silvery shine",
  "red health potion vial, crimson #8C1F1F liquid",
  "blue mana potion vial, deep blue liquid",
  "green herbal potion vial, sage green liquid",
  "ancient rune stone with cyan #00FFCC glow",
  "ancient rune stone with violet glow",
  "ancient rune stone with gold #C9A227 glow",
  "rusty iron key, brown corroded",
  "brass ornate key, golden ornate",
  "small skull fragment, bone white",
  "broken pottery shard, terracotta",
  "dropped torch stub extinguished, charred wood",
  "bloodstain dark crimson #8C1F1F",
  "small cyan #00FFCC mineral chunk",
  "weathered scroll rolled, parchment beige",
  # Variant pass 1 (color/state shift, lines 17-32)
  "gold coin single coin, tarnished",
  "silver coin single coin, blood stained",
  "red potion bottle larger, sealed",
  "blue potion bottle larger, sealed",
  "green potion bottle larger, sealed",
  "rune fragment broken cyan",
  "rune fragment broken violet",
  "rune fragment broken gold",
  "iron key broken half",
  "brass key broken half",
  "skull fragment larger piece",
  "broken pottery shard larger",
  "ash pile from torch, gray powder",
  "smaller bloodstain spatter",
  "cyan mineral cluster larger",
  "scroll unrolled partially, parchment with writing",
  # Variant pass 2 (rotation/orientation, lines 33-48)
  "gold coin pile rotated, side view",
  "silver coin pile rotated, side view",
  "red potion vial knocked over, spilled",
  "blue potion vial knocked over, spilled",
  "green potion vial knocked over, spilled",
  "rune stone flat lying, cyan",
  "rune stone flat lying, violet",
  "rune stone flat lying, gold",
  "iron key on side, rust pattern",
  "brass key on side, ornate detail",
  "skull viewed from above, eye sockets",
  "pottery shard pile multiple pieces",
  "torch stub on side, splintered",
  "blood streak elongated",
  "cyan mineral shard fragment small",
  "scroll case cylindrical, brass cap",
  # Variant pass 3 (size/material variation, lines 49-64)
  "tiny gold coin single small",
  "tiny silver coin single small",
  "tiny health potion vial small",
  "tiny mana potion vial small",
  "tiny herb potion vial small",
  "rune dust pile cyan glow",
  "rune dust pile violet glow",
  "rune dust pile gold glow",
  "iron lock fragment broken",
  "brass lock fragment broken",
  "bone fragment small white",
  "ceramic dust pile gray",
  "wood splinter pile brown",
  "blood drop single droplet",
  "cyan dust speck tiny",
  "parchment scrap torn small"
]
```

#### Reference / Evidence

- **`e899a33d` (treasure pile, 64x64)** — Top-down gold pile çıktı, palette + isolation iyi. 32x32'ye küçültülmüş hali için iyi cue.
- **`scatter_*_f1` batch (dirt/rubble/moss/stones, 32-48px)** — Top-down isolated decal kanıtı. 4× scatter type her biri 4 piece, **toplam 16 işler şekilde üretilmiş**. Bu batch sample'lar kalite kanıtı.
- **`keep_decal_v2` batch (32x32, 6 piece)** — 32x32 floor decal kanıtı, "isolated transparent background" çalışmış.
- **`alabaster_decal` batch (32x32, 4 piece)** — Aynı pattern, palette farklı.

**Pilot risk:** 64-frame batch dispatch **kanıtlı değil**. Master spec Codex Iter 2 cevabı (Açık Soru #7): "use 64 explicit entries for first production. Do not A/B unless quality poor." Codex bunu **safe path** olarak verdi. Ama 64-frame dispatch'in `item_descriptions` field'ı 64 entry forward edebilmesi MCP wrapper'a bağlı.

#### Beklenen Output
- 64 candidate, 32×32 transparent BG
- `select_object_frames(indices=[best 32-48], common_tag="act1_floor_clutter_s95_v1")` ile finalize
- Beklenen success rate: 32-48 piece kullanılabilir (50-75%)

#### Bütçe + Sıra
- **Reserve gen:** 50 (master spec range 30-50 üst sınır)
- **Sıra:** 1.4 — Batch 1.3 pilot başarı sonrası

#### Risk + Pilot Strategy

- **Risk 1 — `item_descriptions` 64 entry forward FAIL:** Plan B/C — 4× 16-frame batch bölmesi (16 base, 16 variant1, 16 variant2, 16 variant3). Cost: 4 × ~25 = 100 gen (2x pahalı).
- **Risk 2 — 64-frame stil drift (frame sayısı yüksekliği):** Codex Iter 2 cevabı bu kabul edilebilir verdict verdi, A/B etme. Drift varsa Plan B.
- **Risk 3 — 32px scale çok küçük + 64 unique item ezici:** Plan B (16-frame x 4 batch) zaten bunu mitige eder.

---

### Batch 1.5 — Void Flame Act 1 (3 state)

#### Tool + Parameters

**Strateji:** `create_object_state` mevcut `f88e821a` (wall sconce + lit torch) üzerinden state olarak türet. **Yeni dispatch DAHA AZ riskli.**

- **Tool:** `mcp__pixellab__create_object_state`
- **Params:** `object_id="f88e821a-18c0-4c04-a63a-a7849409ffff"` (Wall sconce lit torch base)
- 3 state dispatch:
  - State A: `mounted_lit` (Act 1 cyan repaint: turuncu alev → cyan alev)
  - State B: `mounted_dim` (alev kısık, mavi-gri tonlar)
  - State C: `floor_stand_lit` (zemine düşmüş, ayakta yanan torch)

#### Exact Prompt Text

**State A — mounted_lit (Act 1 cyan):**
```
edit color and flame: orange flame replaced by cyan #00FFCC void flame, bracket iron stays #2A2D32, void energy glow, painterly pixel art, no outline, transparent background
```

**State B — mounted_dim:**
```
edit color and intensity: cyan flame dimmed to faint blue-gray ember, bracket dark iron #2A2D32 unchanged, void energy fading, painterly pixel art, no outline, transparent background
```

**State C — floor_stand_lit:**
```
edit geometry: bracket removed, torch handle vertical standing on floor with cyan #00FFCC void flame top, base flame glow on floor, painterly pixel art, no outline, transparent background
```

> **Karar #5 caveat:** State C (geometry değişim: bracket → floor stand) **eşik üstü** olabilir, state_of overlay yetmezse yeni object dispatch.

#### Reference / Evidence

- **`f88e821a` (mounted lit sconce, 64x64, view=n/a)** — STAR örnek, mevcut **side bracket + lit orange torch**. Bu base mükemmel:
  - Side perspective ✓
  - Wall mount bracket ✓
  - Lit flame on top ✓
  - Sadece **rengi değiştirmek** Karar #5'e göre **state_of OK** (color edit)
- State C (floor stand) geometry değişim → **risky**. Plan B: yeni object dispatch (15-25 gen).

**Pilot risk:** State A + State B çok güvenli (color edit). State C orta risk.

#### Beklenen Output
- 3 state, her biri 64×64 transparent BG, `f88e821a` parent'ından türemiş
- Unity entegrasyon: PointLight2D + VoidFlameFlicker.cs ile kullanılır

#### Bütçe + Sıra
- **Reserve gen:** 30 (master spec Karar #9 state range 8-15 × 2-3 state)
- **Sıra:** 1.5 — Batch 1.1-1.4 sonrası, **veya** parallel (bağımsız parent)

#### Risk + Pilot Strategy

- **Plan B (State C state_of yetmez):** Yeni object dispatch `floor_stand_lit_void_flame_act1` (Template benzer Template C v2 single piece, 15-25 gen)
- **Stil tutarlılığı:** state_of palette + canvas + geometry ana frame'den miras alır, **drift riski düşük**
- **Karar #5 LIVE check:** State C output incelendiğinde silhouette > %30 değişmiş ise yeni object'e geç (RIMA heuristic)

---

### Batch 1.6 — Interior Ruined Pieces (Codex onaylı SPLIT)

> **Codex Q6 önerisi LIVE:** Split strategy. Half-wall pieces (yarı duvar) → `view="side"` (Batch 1.1 ailesi). Rubble + collapsed ceiling → `view="high top-down"` (floor decoration). İki sub-batch.
> **Karar gate:** Bu split Batch 1.1 pilot başarısı sonrası fire. Side view kalitesi proven olduktan sonra confident.

---

#### Batch 1.6a — Half-Wall Pieces (side view, Batch 1.1 ailesi)

##### Tool + Parameters
- **Tool:** `mcp__pixellab__create_object`
- **Params:** `size=128`, `view="side"`, `directions=1`, `n_frames=4`, `object_view=None`
- `item_descriptions`: 4 half-wall entry

##### Exact Prompt Text

**`description`:**
```
ancient stone keep ruined half wall pieces, granite gray #3A3D42 base with cyan #00FFCC mineral veins, lower portion remains 40 percent height, broken top edge silhouette with rubble at base, exposed lighter stone #5A5F66 inner core, tall vertical wall billboard from side perspective filling 128px canvas height with narrow transparent margins, painterly pixel art, no outline, isolated on transparent background
```

**`item_descriptions`:**
```
[
  "ruined half wall NS facing south, lower 40 percent remains, jagged broken top, granite cyan veins exposed",
  "ruined half wall EW facing east, perpendicular axis lower 40 percent remains, broken top edge",
  "ruined half wall NS damaged severe, only 25 percent height remains, scattered stones at base",
  "ruined half wall EW damaged severe, only 25 percent height remains, scattered stones at base"
]
```

##### Reference / Evidence
- Same as Batch 1.1 + 1.2 (a52f6711, abf9c178, 3b5bc2c7 reference).
- Damaged silhouette evidence from Batch 1.2 (assumed proven post-pilot).

##### Beklenen Output
- 4 candidate, 128×128 transparent BG, side billboard

##### Bütçe + Sıra
- **Reserve gen:** 40
- **Sıra:** 1.6a — Batch 1.1 + 1.2 başarı sonrası

---

#### Batch 1.6b — Rubble + Collapsed Ceiling (high top-down, floor decoration)

##### Tool + Parameters
- **Tool:** `mcp__pixellab__create_object`
- **Params:** `size=128`, `view="high top-down"`, `directions=1`, `n_frames=4`, `object_view="top-down"`
- `item_descriptions`: 4 floor ruin entry

> **`view="high top-down"` seçimi sebebi:** Bu piece'ler floor decoration (zemin üzerine bırakılan ruin), dikey wall billboard DEĞİL. Karar #6 mapping: tall prop → high top-down + object_view="top-down".

##### Exact Prompt Text

**`description`:**
```
Act 1 Shattered Keep floor ruin pieces, granite gray #3A3D42 + exposed lighter stone #5A5F66 + cyan #00FFCC mineral accents, broken debris scattered on stone floor, high top-down view 35 degrees tilt, painterly pixel art, no outline, isolated on transparent background
```

**`item_descriptions`:**
```
[
  "rubble pile small mound, broken granite blocks with cyan crystal fragments, dome shape silhouette",
  "rubble pile large blocking passage, multiple broken granite blocks scattered, cyan accents",
  "collapsed ceiling debris fragment, heavy granite slab fallen with crack pattern, cyan mineral in fracture",
  "shattered stone block cluster, three large broken pieces with mortar and cyan vein fragments"
]
```

##### Reference / Evidence

- **`6b52751d` (broken pillar 64x96, view="high top-down")** — Üst kırık silhouette + footprint kanıtı.
- **`60502d16` (rift dust patch, 64x64, view="high top-down")** — Top-down soft silhouette kanıtı.
- **`075242f4` (rubble heap prop, 80x80)** — Rubble pile kanıtı, mevcut ama 80px (planımız 128 büyük scale). Stil OK.

**Pilot risk:** 128x128 high top-down + collapsed silhouette **kanıtlı değil**. Mevcut 128px batch'ler view=n/a, mostly intact pillar/wall. Yıkık 128 yeni territory.

##### Beklenen Output
- 4 candidate, 128×128 transparent BG
- Floor footprint: zeminde yatay yer kaplayan ruin piece'ler

##### Bütçe + Sıra
- **Reserve gen:** 40
- **Sıra:** 1.6b — 1.6a sonrası, paralel kabul

##### Risk + Pilot Strategy

- **Risk 1 — view="high top-down" + collapsed silhouette belirsiz:** Plan B — 4 ayrı n_frames=1 dispatch (80 gen). Plan C — view="low top-down" A/B test (1 piece, 20 gen).
- **Risk 2 — scattered debris artefact (kompozisyon dağınık):** "isolated on transparent background, single piece centered" cue strict.

---

### Faz 1 Toplam Bütçe (Codex Q6 SPLIT entegre)

| Batch | Reserve gen | Cumulative |
|---|---|---|
| 1.1 Wall Face Pack (PILOT) | 40 | 40 |
| 1.2 Wall Damaged Variants | 40 | 80 |
| 1.3 Wall Mountings (16) | 40 | 120 |
| 1.4 Floor Clutter (64) | 50 | 170 |
| 1.5 Void Flame (3 state) | 30 | 200 |
| 1.6a Half-wall side | 40 | 240 |
| 1.6b Rubble high top-down | 40 | 280 |
| **Faz 1 reserve total** | **280** | — |
| Plan B/C fallback buffer (worst case drift) | +80-100 | 360-380 |

**Faz 1 hard upper bound:** ~380 gen (worst case all-fallback). 2,433 bütçeden **%16 max**.

---

## 3. Faz 2 — Karakter & Mob (V3 Web UI, USER Manual)

**Önemli:** PixelLab MCP dispatch YASAK. `feedback_pixellab_character_via_web_ui_v3` HARD LOCK gereği user web UI V3'te manuel gen. Bu faz **RIMA PixelLab gen budget DIŞINDA** (V3 ayrı pricing).

**Memory HARD LOCK gate:** `feedback_character_state_planning_before_production` — her karakter/mob için state listesi **üretim ÖNCESİ** zorunlu user onay.

### 2.1 — Warblade Anchor State Production (Codex Q2: 17-state MVP LIVE)

> **Codex önerisi LIVE:** 17-state MVP first, cross-class 6 slot DEFER. Sebep: cross-class wiring code-side henüz LIVE değil (Shadowblade ComboPointSystem gap memory). Cross-class wiring proven olduğunda 23 full target.

#### State Listesi MVP — 17 State LIVE (Karar #100 + Anchor LOCK)

**Movement (10 state):**
- `idle_S`, `idle_SE`, `idle_E`, `idle_NE`, `idle_N` (5 produce + 3 mirror = 8-dir)
- `walk_S`, `walk_SE`, `walk_E`, `walk_NE`, `walk_N` (5 produce + 3 mirror)

**Combat (6 state):**
- `attack_strike_S`, `attack_strike_E` (sword vertical, sword horizontal)
- `attack_heavy_S`, `attack_heavy_E` (slow telegraphed)
- `hit_react`, `death`

**Signature (1 state):**
- `rage_burst` (class signature, Karar locked)

**Total: 17 state MVP production**

#### Cross-Class 6 Slot — DEFER (post code-side wiring proven)

`cross_class_slot_1` … `cross_class_slot_6` (Karar #79 ghost VFX trigger placeholder) — cross-class wiring LIVE olduğunda Faz 2.1b dispatched.

**Trigger gate:** ShadowBlade ComboPointSystem wiring proof + Karar #79 ghost VFX implementation LIVE → 6 slot V3 web UI dispatch (USER manual).

#### V3 Web UI Prompt Template

**Anchor character_id:** `2656075d-d113-4f18-a6c1-94b5a6b8bf65` (canonical roster v2)

**Base prompt (V3 anchor format, weaponless body Karar #144):**
```
Warblade, weaponless muscular fighter, leather harness over linen tunic, blood-red sash, weathered iron pauldrons, calloused hands ready, painterly pixel art, no outline, transparent background, isometric 35 degree view, isolated, full body
```

**State-specific cues** (each state appends to anchor):
```
[idle_S]    : standing relaxed facing south, weight on right leg, ready stance
[idle_SE]   : standing relaxed facing south-east, three-quarter view
[walk_S]    : walking south, mid-stride right foot forward, body shifted slightly
[attack_strike_S] : front-arm forward strike motion, body torqued, southward
[rage_burst]: feet planted wide, arms raised, blood-red aura energy
... etc
```

#### Reference / Evidence

- **`2656075d` Warblade canonical anchor** — Mevcut, S86 V3 web UI ile production proven (Karar Anchor Cohesion).
- **8-dir LOCK** (`feedback_8dir_mirror_production_strategy`) — 5 produce + 3 mirror, kanıtlı pipeline.
- **Cross-class slot** (Karar #79) — 6 ghost VFX placeholder slot, sprite üretim sırasında base body yeterli, ghost VFX Faz 3'te.

#### USER Action

1. User canonical anchor (`2656075d`) V3 web UI'da load et
2. State listesi (23 state) için V3'te tek tek prompt fire
3. Output: 23 sprite × 5 direction = 115 sprite (8-dir mirror sonrası)
4. Unity import: `Assets/Art/Characters/Warblade/V3_*` klasörü

#### Bütçe + Sıra

- **PixelLab RIMA budget: 0 gen** (V3 ayrı)
- **V3 generation cost:** User kendi V3 sub'undan
- **Sıra:** 2.1 — Faz 1 (görsel komple demo room) sonra. Eğer demo room'da combat playable seviye gerekmiyorsa **paralel** ilerleyebilir.

#### Risk + Pilot Strategy

- **Risk 1 — V3 stil drift (S87 Karar #145 v2 6 use case'te variant gen kanıtlı):** Düşük risk
- **Risk 2 — 23 state ölçek aşırı:** **Pilot V3 batch öneri:** önce 5 idle + 5 walk = 10 state (movement core), validate, sonra combat + signature.
- **Risk 3 — Cross-class 6 slot prematür:** Cross-class wiring kod tarafında henüz LIVE değil olabilir (Shadowblade ComboPointSystem gap memory). **Slot defer öneri:** sadece movement + combat + signature = 17 state.

### 2.2-2.5 — Diğer Karakter / Mob (defer plan)

Faz 2.2-2.5 (Elementalist, Ranger, Shadowblade + 5 mob), 2.1 pilot sonrası prompt template'ler tekrarlı pattern ile yazılır. **Bu plan dosyası 2.1 detaylı + 2.2-2.5 outline only** (early scope buffer).

---

## 4. Faz 3 — VFX & Animasyon

**Total range:** ~85-145 gen reserve (master spec'te 85-125, buffer ile)

### 3.1 — Void Flame Flicker Animation

#### Tool + Parameters
- **Tool:** `mcp__pixellab__animate_object`
- **Params:** `object_id=<Batch 1.5 State A object_id>` (mounted_lit cyan void flame)
- `animation_description`: flicker cycle

#### Exact Prompt Text
```
gentle cyan void flame flicker cycle, flame top edge undulates 1.5 to 3 Hz, brightness pulse soft sine wave, no position shift, painterly pixel art, no outline, transparent background, 8 frames loopable
```

#### Reference / Evidence

- **`11127e69` (hitspark VFX, 7-frame anim)** — `animate_object` PROVEN, dynamic VFX yüksek kalite.
- **`58c183a0` (dash trail, 9-frame anim)** — Aynı, smooth loop kanıtı.

**Pilot risk:** DÜŞÜK. Animate pipeline kanıtlı.

#### Beklenen Output
- 8-frame loopable cyan flame flicker, 64×64 transparent BG

#### Bütçe + Sıra
- **Reserve gen:** 15-25 (animate_object cost, 8 frame)
- **Sıra:** 3.1 — Faz 1.5 sonrası

#### Risk + Pilot Strategy
- Drift varsa: shorter animation (4 frame) re-dispatch

---

### 3.2 — Slash VFX (64 n_frames=4 anim grid)

#### Tool + Parameters
- **Tool:** `mcp__pixellab__create_object` (yeni base) → sonra `animate_object`
- **Veya alternatif:** Mevcut `11127e69` hitspark base'inden `create_object_state` ile slash variant türetme

> **Pivot karar:** State_of OK için color/glow edit yeter — slash silhouette **yeni geometry** (radyal burst → directional slash). Yeni object dispatch öneri.

- **Params (yeni object):** `size=64`, `view="side"`, `directions=1`, `n_frames=4`, `object_view=None`
- `item_descriptions`: 4 slash direction

#### Exact Prompt Text

**`description`:**
```
sword slash VFX trail arc, cyan #00FFCC + white core energy, motion smear shape, painterly pixel art, no outline, isolated on transparent background, single arc gesture
```

**`item_descriptions`:**
```
[
  "slash arc curve from upper left to lower right, cyan trail with white core, sharp leading edge",
  "slash arc curve from upper right to lower left, mirrored, cyan trail",
  "vertical chop arc top to bottom, cyan trail with white core",
  "horizontal sweep arc left to right, cyan trail flat curve"
]
```

#### Reference / Evidence

- **`11127e69` hitspark** — Cyan radial burst kanıtı, kalite yüksek. Slash arc benzer cycle ama directional.
- **Pilot risk:** Slash arc shape kanıtlı değil (mevcut sadece radial). Yeni geometry.

#### Beklenen Output
- 4 arc direction, 64×64 transparent BG, animate sonrası 4-frame loop her biri

#### Bütçe + Sıra
- **Reserve gen:** 25-40 (base) + 4 × 15 (animate) = 85 worst case
- **Sıra:** 3.2 — 3.1 sonrası

#### Risk + Pilot Strategy
- Plan B: 1 arc piece pilot (15-25 gen), kalite OK ise diğer 3 batch.

---

### 3.3 — Dust/Spark Floor VFX (32 n_frames=16)

#### Tool + Parameters
- **Tool:** `mcp__pixellab__create_object`
- **Params:** `size=32`, `view="low top-down"`, `directions=1`, `n_frames=16`, `object_view="top-down"`
- `item_descriptions`: 16 dust/spark variant

#### Exact Prompt Text

**`description`:**
```
floor VFX dust spark particles, gray dust + cyan ember accents, painterly pixel art, no outline, isolated on transparent background, top-down view small particle effect
```

**`item_descriptions`:**
```
[
  "dust puff small gray cloud center",
  "dust puff medium gray cloud spreading",
  "dust puff large dispersing fading",
  "ember spark cyan single tiny",
  "ember spark cyan cluster three",
  "ember spark cyan ring expanding",
  "footprint dust trail elongated",
  "impact crater dust burst radial",
  "blood mist red small droplets",
  "blood splatter medium spread",
  "stone shard tiny gray fragment",
  "stone shard cluster three pieces",
  "rune glow puff cyan magic",
  "rune glow puff violet magic",
  "smoke wisp gray fading upward",
  "smoke wisp gray dense lingering"
]
```

#### Reference / Evidence

- **`60502d16` rift dust patch** — top-down dust kanıtı.
- **`scatter_dirt_f1` 4-piece** + **`scatter_rubble_f1` 4-piece** — 32px isolated decal kanıtı.

#### Beklenen Output
- 16 candidate, 32×32 transparent BG
- Floor projectile / hit trigger VFX library

#### Bütçe + Sıra
- **Reserve gen:** 25-40
- **Sıra:** 3.3 — 3.2 sonrası

#### Risk + Pilot Strategy
- 16-frame batch kanıtlı (Batch 1.3 mounting da 16-frame, pilot sonrası).

---

### 3.4 — Decor Silhouette Idle Animation (OPTIONAL)

#### Karar: DEFER

Mevcut prop (statue, archway) idle animation **MVP scope dışı**. Faz 1.5 Void Flame flicker yeterli atmosfer. Bu batch **Sıra 4+** (Act 1 sonrası polish).

---

### Faz 3 Toplam Bütçe

| Batch | Reserve gen |
|---|---|
| 3.1 Void Flame Flicker | 25 |
| 3.2 Slash VFX | 85 (worst) |
| 3.3 Dust/Spark VFX | 40 |
| 3.4 Decor idle (DEFER) | 0 |
| **Faz 3 reserve total** | **~150 worst case** |

---

## 5. Faz 4 — UI Polish & Detay

**Total range:** ~70-110 gen reserve

### 4.1 — Item Icons (32 n_frames=64)

#### Tool + Parameters
- **Tool:** `mcp__pixellab__create_object`
- **Params:** `size=32`, `view="low top-down"` (icon view), `directions=1`, `n_frames=64`, `object_view="top-down"`
- `item_descriptions`: 64 item icon

#### Exact Prompt Text

**`description`:**
```
Act 1 Shattered Keep RPG item icons, isolated on transparent background, painterly pixel art, no outline, top-down icon view, small square footprint, cool palette grays + cyan + gold + crimson accents
```

**`item_descriptions`** (64 item icon, Faz 1.4 floor clutter ile **OVERLAP RISKLI** — distinct kept):
```
# Consumable (1-16): 5 potion + 5 elixir + 6 food
"red health potion small icon, vial corked"
"red health potion large icon, vial corked"
"blue mana potion small icon, vial corked"
"blue mana potion large icon, vial corked"
"green herbal potion small icon, vial corked"
"purple antidote elixir icon, vial wax-sealed"
"yellow stamina elixir icon, vial cork"
"orange fire-resist elixir icon"
"cyan ice-resist elixir icon"
"silver mystery elixir icon"
"bread loaf brown icon, rounded crusty"
"cheese wedge yellow icon, cut wedge shape"
"apple red icon, round with leaf"
"meat haunch icon, roasted bone visible"
"fish fillet icon, raw scales visible"
"hardtack biscuit icon, weathered ration"
# Quest (17-24): 8 quest item
"old map parchment rolled icon"
"old key bronze ornate icon"
"red gem ruby cut icon"
"blue gem sapphire cut icon"
"green gem emerald cut icon"
"black stone shard rift fragment icon"
"silver locket pendant icon"
"folded letter wax-sealed icon"
# Material (25-40): 16 craft material
"gold ingot small icon"
"silver ingot small icon"
"iron ingot small icon"
"bronze ingot small icon"
"cyan rift crystal raw icon"
"violet rift crystal raw icon"
"gold rift crystal raw icon"
"granite stone chunk icon"
"leather scrap rolled icon"
"linen cloth rolled icon"
"wood plank icon"
"bone fragment large icon"
"feather single white icon"
"thread spool brown icon"
"glass bead clear icon"
"silver chain link icon"
# Equipment hint (41-56): 16 wearable hint icon
"sword small icon iron"
"sword small icon silver"
"axe small icon iron"
"mace small icon iron"
"dagger small icon iron"
"bow small icon wood"
"staff small icon wood"
"shield small round iron"
"helm small icon iron"
"breastplate icon iron"
"gloves icon leather"
"boots icon leather"
"ring icon gold band"
"amulet icon silver pendant"
"cloak icon dark folded"
"belt icon leather buckle"
# Currency (57-64): 8 currency
"single gold coin icon"
"single silver coin icon"
"copper coin icon"
"gold coin pile small icon"
"silver coin pile small icon"
"cyan rift shard small currency icon"
"violet rift shard currency icon"
"gold rift shard currency icon"
```

> **OVERLAP NOTU:** Bazı icon (gold coin, red potion, rune crystal) Batch 1.4 floor clutter ile **işlevsel overlap**. Düşünce: 1.4 floor 32px sprite (yere düşmüş hali), bu batch icon 32px (UI envanter sembolü). **Visual style farklı** (icon = centered, clean; clutter = scattered, dropped). KEEP BOTH.

#### Reference / Evidence

- **Skill icon batch (`dark fantasy game skill icon`, 24 piece, 64px)** — Icon style kanıtı, ama 64px. 32px aynı pipeline.
- **`scatter_*_f1` decals 32px** — 32px isolated decal kanıtı.

**Pilot risk:** 64-frame icon batch **kanıtlı değil**. Master spec Codex Iter 2 verdict: safe path. Plan B aynı: 4× 16-frame batch bölme (cost 4× ~25 = 100 gen).

#### Beklenen Output
- 64 icon candidate, 32×32 transparent BG
- `select_object_frames(indices=[best 48-60])` finalize

#### Bütçe + Sıra
- **Reserve gen:** 50 (master spec range 30-50 max)
- **Sıra:** 4.1 — Faz 3 sonrası

#### Risk + Pilot Strategy
- Aynı 1.4 (64-frame) — plan B/C zaten oturmuş.

---

### 4.2 — HUD Elements

#### Karar: PixelLab DIŞI

HUD elements (HP bar, rage bar, skill slot frames, map UI) **vector / sharp pixel art**. PixelLab painterly style HUD'a uygunsuz. Codex `gpt-image-1` imagegen veya manual SVG / Photoshop.

**Bütçe (RIMA gen):** 0 gen. PixelLab budget DIŞI.

---

### 4.3 — Boss Prop (128 n_frames=4)

#### Tool + Parameters
- **Tool:** `mcp__pixellab__create_object`
- **Params:** `size=128`, `view="high top-down"`, `directions=1`, `n_frames=4`, `object_view="top-down"`
- `item_descriptions`: 4 boss-tier landmark prop

#### Exact Prompt Text

**`description`:**
```
Act 1 Shattered Keep boss tier landmark props, granite gray #3A3D42 + cyan #00FFCC void energy accents, monumental scale piece, high top-down 35 degree view, painterly pixel art, no outline, isolated on transparent background
```

**`item_descriptions`:**
```
[
  "stone throne ornate, granite base + cyan rift crystals embedded in backrest, monumental",
  "ritual altar large, granite slab + cyan void runes + sacrificial brazier",
  "rift portal device tall, cyan energy orb on granite pedestal, dripping void energy",
  "ancient guardian statue toppled, broken granite warrior fallen with cyan crack"
]
```

#### Reference / Evidence

- **`3b5bc2c7` intact pillar 128px** — Monumental scale + cyan veins kanıtı.
- **`c35acb5a` ritual prop 128px** — Ritual altar tipinde mevcut object var (görsel henüz incelenmedi ama tag `act1_*` indica diyor).

**Pilot risk:** 4-piece boss batch kanıtlı pattern (Batch 1.1 + 1.2 zaten 4-piece pilot validated olur).

#### Beklenen Output
- 4 monumental candidate, 128×128 transparent BG
- Boss room landmark library

#### Bütçe + Sıra
- **Reserve gen:** 40
- **Sıra:** 4.3 — Act 1 boss room hazırlık

---

### Faz 4 Toplam Bütçe

| Batch | Reserve gen |
|---|---|
| 4.1 Item Icons (64) | 50 |
| 4.2 HUD (PixelLab dışı) | 0 |
| 4.3 Boss Prop | 40 |
| **Faz 4 reserve total** | **90** |

---

## 6. Toplam Bütçe Planı (Range-Based, Codex Q6 SPLIT entegre)

> [!NOTE]
> **Sorting Layer Note:** Toplam bütçe planında zemin elemanları için geçen tüm tarihsel "Floor" sorting layer referansları canonical `Ground` layer'ına karşılık gelmektedir.

| Faz | Reserve gen | Cumulative | % of 2,433 |
|---|---|---|---|
| Faz 1 (Demo MVP, Q6 split LIVE) | 280 (380 worst) | 280-380 | 12-16% |
| Faz 2 (V3 USER, ayrı budget, 17-state MVP) | 0 | 280-380 | — |
| Faz 3 (VFX) | 150 worst | 430-530 | 18-22% |
| Faz 4 (UI Polish) | 90 | 520-620 | 21-25% |
| **All-Faz total** | **~520-620** | — | **~25% max** |

**Marj:** Kalan ~%75 (≈1,810 gen) Act 2 / Act 3 / iterasyon / cross-act variant için.

**Faz sıra:**
1. **PILOT (Hafta 1):** Batch 1.1 (40 gen). Sonuç görsel review.
2. **Faz 1 production (Hafta 1-2):** 1.2-1.6 (200 gen).
3. **Faz 2 USER parallel (Hafta 1-3):** V3 web UI, RIMA budget'tan bağımsız.
4. **Faz 3 (Hafta 2-3):** 1.5 sonrası 3.1 → 3.2 → 3.3 (150 worst).
5. **Faz 4 (Hafta 3+):** 4.1 + 4.3 (90 gen).

**Trigger gate (master spec Karar #9):** Her batch sonrası `STAGING/RIMA_PixelLab_BalanceLog.md` güncel `usage` log.

---

## 7. Açık Sorular — Codex Cevapları LIVE (User Override Edebilir)

> [!NOTE]
> **Sorting Layer Note:** Pilot stratejisinde ve açık sorularda zemin objeleri için kullanılan "Floor" sorting layer ismi teknik uygulamada `Ground` olarak ele alınacaktır.

Codex review v1 her soruya cevap verdi. Aşağıda her Q için **Codex önerisi LIVE default** + **Opus rationale**. User Antigravity review'da override edebilir.

### Q1 — Pilot Strategy: Batch 1.1 direct vs mini pilot?

**Codex önerisi (LIVE):** Batch 1.1 direct 4-piece pilot.

**Rationale:** Mini pilot (n_frames=1 tek piece) sadece `view="side"` + wrapper acceptance doğrular — **per-frame identity separation'ı doğrulamaz**. Direct 4-piece pilot tüm 4 unknown'ı (view, item_descriptions forward, n_frames=4 style coherence, HEX palette) tek dispatch'te test eder. Mini pilot **eksik kapsama** sebebiyle reject.

**User override yolu:** Mini pilot tercih edilirse Batch 1.1'i 1 piece n_frames=1 (`face_S` ile) önce dispatch et (15-25 gen), sonra remaining 3 piece tekrar 4-piece batch gibi dispatch. Cost: +15-25 gen ekstra, ama wrapper validation izole edilmiş olur.

---

### Q2 — Faz 2 Warblade State Listesi: 23 vs 17 (cross-class defer)?

**Codex önerisi (LIVE):** **17 state MVP first** (cross-class 6 slot defer).

**Rationale:** Cross-class wiring code-side henüz LIVE değil (Shadowblade ComboPointSystem gap memory). Production prematür. 17-state MVP: 10 movement + 6 combat + 1 signature. Cross-class wiring proven olduğunda 23-state full production hedef.

**User override yolu:** 23 full istiyorsan kabul, ama cross-class slot sprite'ları ghost VFX trigger gelene kadar **kullanılmaz** asset olarak Unity'de bekler. Hot path warning.

---

### Q3 — Faz 3 Slash VFX: Yeni object vs state_of?

**Codex önerisi (LIVE):** **Yeni object dispatch** (Batch 3.2).

**Rationale:** Hitspark → slash arc = geometry/silhouette değişimi (radial burst → directional arc). Karar #5 görsel heuristic gereği state_of clean default değil. State_of trial **sadece budget pressure yüksekse** worth — şu an bütçe %24 max kullanım, pressure DÜŞÜK. Yeni object dispatch direct.

**User override yolu:** State_of pilot (15 gen) düşük cost — meraktan dene denilirse OK. Başarısızsa yeni object dispatch (toplam 100 gen worst), başarılıysa 70 gen kazanç.

---

### Q4 — Faz 4 Item Icons (4.1) ile Floor Clutter (1.4) Overlap?

**Codex önerisi (LIVE):** **İki batch ayrı kalır.**

**Rationale:** Budget headroom yeterli (~%24 max). Visual semantics farklı: floor/drop sprite = scattered, edge-rough, perspective tilt; UI icon = centered, clean, sembolik. Rebase **sadece Batch 1.4 output unusually clean ve icon-like çıkarsa** yapılır (post-hoc save).

**User override yolu:** Floor clutter (1.4) sonrası output review'da temiz çıkanlardan icon'a rebase olabilir. Bu Batch 4.1 öncesi karar gate.

---

### Q5 — Wall Damaged Variants Bütçe (Karar #5)?

**Codex onayı (LIVE):** Bütçe OK.

**Rationale:** Master spec Iter 2'de zaten acceptable verildi. Plan'da Batch 1.2 reserve 40 + Plan C buffer 80 = max 120 gen. 2,433 bütçeden %5. Master spec ile uyum.

---

### Q6 — Faz 1.6 Interior Ruined: tek high top-down batch vs split?

**Codex önerisi (LIVE):** **Split strategy quality öncelikli ise.**

**Rationale:** Half-wall pieces (yarı duvar) Batch 1.1'in side view stil ailesinde kalmalı (görsel tutarlılık). Rubble + collapsed ceiling true floor decoration → high top-down. İki sub-batch +20-40 gen ama stil tutarlılığı belirgin daha iyi.

**Karar gate (Codex önerisi):** Bu **pilot blocker DEĞİL** — Batch 1.1 pilot başarısı sonrası karar verilir. Side view kalitesi proven olduğunda split confident.

**User override yolu:** Tek batch high top-down kalır, +20-40 gen tasarruf. Stylistic compromise: half-walls high top-down'da floor object gibi görünür.

---

### Codex Cevapları Plan Etkisi Özeti

| Q | Codex önerisi LIVE | Plan değişimi |
|---|---|---|
| Q1 | Batch 1.1 direct pilot | **No change** — plan zaten bu |
| Q2 | 17-state MVP, cross-class defer | **Faz 2.1 LIVE = 17 state** (23 plan target) |
| Q3 | Yeni object slash VFX | **No change** — Batch 3.2 zaten yeni object |
| Q4 | İki batch ayrı | **No change** — plan zaten ayrı |
| Q5 | Bütçe OK | **No change** — sanity confirmed |
| Q6 | Split half-wall side + rubble high top-down | **Faz 1.6 LIVE = split** (post Batch 1.1 confirm) |

**Net production değişim:**
- Faz 2.1 state listesi 23 → **17 LIVE** (defer 6 cross-class)
- Faz 1.6 tek batch → **2 sub-batch LIVE** (half-wall + rubble ayrı view)
- Batch 1.6a (half-wall, view="side"): 2 piece n_frames=4 partial — veya 2 piece ayrı n_frames=1
- Batch 1.6b (rubble + collapsed ceiling, view="high top-down"): 2 piece n_frames=4 partial

**Bütçe etki:** Faz 1.6 split = 40 + 25-40 = 65-80 gen (önceki 40 gen reserve'in ~%65-100 ekstra). Faz 1 total revize:

| Batch | Reserve gen | Cumulative |
|---|---|---|
| 1.1 Wall Face Pack (PILOT) | 40 | 40 |
| 1.2 Wall Damaged Variants | 40 | 80 |
| 1.3 Wall Mountings (16) | 40 | 120 |
| 1.4 Floor Clutter (64) | 50 | 170 |
| 1.5 Void Flame (3 state) | 30 | 200 |
| **1.6a Half-wall (side)** | **25-40** | **225-240** |
| **1.6b Rubble + ceiling (high top-down)** | **25-40** | **250-280** |
| **Faz 1 revize total** | **~280 reserve** | — |

Total plan worst case: 280 (Faz 1) + 150 (Faz 3) + 90 (Faz 4) = **520 gen worst**. 2,433 bütçeden **%21**. Hâlâ güvenli marj.

---

## 8. Pilot Dispatch Önerisi (İlk Gerçek Üretim)

**ÖNERI:** Batch 1.1 — Wall Face Pack — pilot.

**Sebep (4-test single batch):**
1. **`view="side"` parameter test** — `act1_wall_pieces_s95` (view=n/a) batch'inde %50 drift gözlendi. Side explicit ile %75-100 hedef.
2. **`item_descriptions` field forward test** — MCP wrapper'dan forward edilip edilmediği bu batch'te belli olur (master spec Codex Iter 2 caveat).
3. **n_frames=4 batch style coherence test** — 4-piece tek dispatch stil tutarlılığı master spec Karar #4 production rule LIVE doğrulama.
4. **Granite + cyan palette HEX cue test** — `abf9c178` batch'inde HEX prompt'ta olmadan cyan doğru çıkmış. HEX explicit ile daha güçlü kanıt.

**Pilot başarısı = Faz 1 production GO** (1.2-1.6 + tüm faz sonrası).
**Pilot başarısızlığı = Plan B (REST direct) veya Plan C (4× ayrı dispatch) re-strategy.**

**Cost:** 25-40 gen (1.5% bütçe). **Fail-fast guarantee, risk MINIMAL.**

---

## 9. Codex Review Hazırlık

Bu plan dosyası `cx_dispatch.py` ile Codex `high` effort review için gönderilecek. Codex'ten beklenen kontroller:

1. **PixelLab API constraint compliance:**
   - `size` enum içinde mi? (master spec: 32, 64, 128, 256 + create_isometric_tile max 64)
   - `n_frames` enum [1, 4, 16, 64] doğru kullanılmış mı?
   - `view` valid string mi? (`"side"`, `"low top-down"`, `"high top-down"`)
   - `object_view` valid value veya None mı?

2. **`item_descriptions` field MCP wrapper forward — ACTIONABLE caveat:**
   - Pilot dispatch verify step belirlenmiş mi?
   - Plan B (REST direct) hazır mı?

3. **Prompt formülü Karar #7 uyum:**
   - Genre label YASAK ("dark fantasy", "horror") — check
   - 3rd-party name YASAK ("Hades") — check
   - HEX color usage — check

4. **Evidence layer yeterli mi?**
   - Her batch için reference object verilmiş mi?
   - Drift gözlemler dürüst yansıtılmış mı (cherry-picked değil)?
   - Pilot risk işaretleri net mi?

5. **Bütçe master spec Karar #9 ile uyum:**
   - Range-based (lower-upper) mı?
   - Worst case fallback hesaplı mı?
   - Cumulative + buffer total master spec ile çelişiyor mu?

6. **State listesi karakter (Faz 2.1) memory HARD LOCK uyum:**
   - `feedback_character_state_planning_before_production` — state listesi üretim öncesi var mı?
   - User onay gate işaretlenmiş mi?

**Codex task dosyası:** `STAGING/CODEX_TASK_production_plan_review_v1.md` (sonraki adım).

---

## 10. Hard Constraints Hatırlatma

- **PixelLab dispatch BU SPEC'TE YAPILMAYACAK** — sadece hazır prompt + sıra.
- **Pilot first:** İlk dispatch Batch 1.1 (40 gen). User onay sonrası fire.
- **Karar #144 weaponless body + Karar #100 35° + Karar #149 mirror archway + Karar #142 Hades-Style overlay** uyumlu.
- **Memory LOCK uyum:**
  - `feedback_pixellab_character_via_web_ui_v3` HARD LOCK (Faz 2 V3 USER manual)
  - `feedback_character_state_planning_before_production` HARD LOCK (Faz 2 state listesi öncesi)
  - `feedback_wall_decoration_pure_attachment_only` HARD LOCK (Batch 1.3 wall_mounting + Batch 1.5 Void Flame)
  - `feedback_user_cannot_draw_full_autonomy_required` HARD LOCK (Aseprite/manual fallback YASAK)
- **NLM-First Context:** Bu plan dosyası NLM sync kapsamında olabilir (orchestrator karar).
- **Geri dönülebilir:** Sadece dosya yazma, kod/asset/scene değişimi yok.

---

> **Durum:** DRAFT — Codex review + user Antigravity review bekliyor. User pilot dispatch onayı sonrası Batch 1.1 fire.
