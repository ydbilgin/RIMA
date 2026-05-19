> **2026-05-19 S94 update:** Karar #100 LOCK reinforced — RIMA canonical view = **Hades 35 derece angled high top-down**, NOT pure isometric. Earlier draft used "izometrik" loosely; corrected throughout.
> **Karar #149 LIVE:** Visual production now plans per sub-room (16x10 default). Each sub-room can have variant prop layout; Faz 1 (lighting + props + decals) applies to every sub-room in the encounter.

# RIMA Visual Production Plan — Concept'e Yakın Hedef (S94)

**Goal:** Codex'in Act 1 concept art (`Assets/Art/Reference/RIMA_Act1_Spawn01_concept_v1.png`) feeling'ini yakala — AMA asset'ler RIMA identity ile. Concept'teki "stones + grass" → RIMA "cracks + rifts + cosmic catastrophe".

## RIMA visual identity vs concept art elements

| Concept element | RIMA equivalent | Why |
|---|---|---|
| Stones + small pebbles scatter | **Crack patches + rubble fragments** | Shattered Keep = parçalanmış tapınak |
| Grass tufts + moss | **Cool moss + ash + dust drift** | Cool palette + ritüel catastrophe |
| Fallen leaves | **Bone fragments + ritual debris** | Antik temple, "Ritual Catastrophe" tema |
| Ambient grass swaying | **Rift seepage glow pulse** (cyan/violet) | Rift contamination signature |
| Lanterns flame | **Cyan rift glow + violet pulse** | RIMA energy = rift |
| Banners sway | **Tattered ritual banners + sigils** | Antik temple aesthetic |
| Combat impact (orange spark) | **White-core impact + class color tint** | Yudou rule, class identity |

**Identity lock:** RIMA Act 1 = Hades atmosphere + Bandit Knight readability + RIMA Rift Catastrophe theme. **Asset gen tüm bu prensiplere uymalı.**

## 5-Phase production plan (sıralı)

### PHASE 1 — Unity cleanup (15 dk)

Hedef: Sahnede sadece bu planın asset'leri kalsın, legacy temizlensin.

**Sahnede tutulacak:**
- L1_BaseFloor (Tilemap, granite_pure_noise)
- L3_Walls (top + bottom — geçici, yeni gen sonrası değişir)
- Gates (left + right — geçici)
- L4_Patches (moss + dust + crack — Act1 ready)
- L5_Scatter (small stones near gates)
- Warblade_Player

**Projede temizlenecek:**
- `Assets/Scenes/Archive/` — eski sandbox/backup scene'ler
- `Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/` — audit + Act1'e migrate edilmemiş olanlar
- `Assets/Resources/Environment/StoneDungeon/` — boş kalan klasörler
- `Assets/Art/Rooms/AssetPack/` — boş kalan klasörler (asset taşındı, dizinler kalmış)

### PHASE 2 — Core asset production (3-4 gün)

**Production order (her asset için en uygun tool ile):**

#### 2.1 Cool Granite Wall Cap (HADES-style tall block, 35°)

**Tool:** PixelLab Create Image Pro web UI (SEN yapıştır — gen budget koruma için)
**Save:** `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/granite_wall_cap_01.png`
**Size:** 256x128

Web UI prompt:
```
Top-down dungeon wall section at slight 30-degree angle showing visible height, cool weathered granite tall stone block construction, painterly hand-drawn 32-pixel-scale pixel art, ancient temple wall partially crumbled, subtle moss creeping at base, ancient stone block construction with mortar lines barely visible, tiny cracks and weathering scattered, top of wall slightly forward of base in screen space, muted cool gray with blue-violet undertones, NO bright color

Negative Prompt : border around image, frame, outline, watermark, text, logo, smooth gradient, anti-aliasing, 3d render, full 3D perspective, modern wall, clean perfect blocks, brick grid mortar, geometric pattern, repeating units, sandstone brown
```

Web UI settings: Size 256x128, tile_strength 0.2 (LOW — not seamless tile, sprite), outline lineless, shading basic, detail medium.

#### 2.2 Granite Arch Gate with Cyan Rift Glow (vertical sprite)

**Tool:** PixelLab Create Image Pro web UI (SEN yapıştır)
**Save:** `Assets/Art/AssetPacks/Act1_ShatteredKeep/gates/granite_arch_rift_01.png`
**Size:** 128x192

Web UI prompt:
```
Dungeon gate archway sprite for top-down game Act 1 Shattered Keep, cool weathered granite stone arch frame matching adjacent wall material, painterly hand-drawn 32-pixel-scale pixel art, ancient temple stone block construction, slight 30-degree top-down angle, inside the arch opening: dark void with subtle cyan and violet rift contamination glow at edges, no iron-banded wooden gate (open passage), thin moss at base where arch meets floor, faint sigil engravings barely visible on the keystone, top-down 30-degree view showing arch from front

Negative Prompt : border around image, frame around sprite, outline around sprite, watermark, text, logo, smooth gradient, anti-aliasing, 3d render, full 3D perspective, modern gate, clean perfect construction, brick grid mortar, geometric pattern, iron bars closed gate, bright fantasy colors, glowing white center
```

Web UI settings: Size 128x192, tile_strength 0.2, outline lineless, shading basic, detail medium, style reference image (use Wall Cap result from 2.1 for palette consistency).

#### 2.3 Painterly Granite Floor Variant (subtle stone-like, ne pure noise ne too patterned)

**Tool:** Codex gpt-image-1 (built-in, $0)
**Save:** `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/granite_painterly_01.png`
**Size:** 64x64 seamless

Codex task'ı bana yazdır, ben dispatch ederim. Prompt:
```
Seamless 64x64 cool weathered granite floor surface texture, top-down view, painterly hand-drawn 32-pixel-scale pixel art, very subtle stone weathering pattern (NOT recognizable shapes, NOT cobblestone), monolithic continuous ground, muted cool gray #3A3D42 to #4E5260 range with blue-violet undertones, sparse 1-2 pixel mineral specks scattered, NO border/frame/outline. Top edge pixel row must match bottom edge pixel row. Left column must match right column. Truly seamless.

Negative: cobblestone, brick, mortar, individual stones with gaps, recognizable shape pattern, bright color, dark border edge, smooth gradient anti-aliasing, 3d render
```

#### 2.4 Rift Seepage Patches (cyan/violet contamination, sparse)

**Tool:** PixelLab `create_map_object` MCP (otonom, transparent bg)
**Save:** `Assets/Art/AssetPacks/Act1_ShatteredKeep/rift_accents/`
**Variants:** 4 (thin crackline, small bloom, mixed halo, faint residue)

Ben dispatch edebilirim — `create_map_object` cheap (~5 gen per dispatch). Hepsi `transparent bg` çıkar.

#### 2.5 Crack Patches (organic crack pattern on floor)

**Tool:** PixelLab `create_map_object` MCP (otonom)
**Save:** `Assets/Art/AssetPacks/Act1_ShatteredKeep/patches/`
**Variants:** 3 (small fracture, large spread crack, multi-branch)

Ben dispatch.

#### 2.6 Ritual Debris Scatter (bone fragments + temple shards)

**Tool:** PixelLab `create_object` MCP
**Save:** `Assets/Art/AssetPacks/Act1_ShatteredKeep/scatter/`
**Variants:** 4 (ritual fragment, bone chip, broken pottery, sigil shard)

Ben dispatch.

### PHASE 3 — Animated effects (V3 web UI, SEN yapıyorsun)

**Tool:** PixelLab V3 Custom Frames (KF+Interp) web UI
**Why user does:** V3 start/end keyframe interpolation MCP'de yok, sadece web UI'da. Plus gen budget koruma.

Her efekt için ben sana **2 keyframe prompt + interpolate ayar** veririm. Sen PixelLab web UI'ya yapıştırırsın, anim PNG/spritesheet gelir.

**Efekt sırası:**

| # | Effect | Start frame | End frame | Frames | Loop |
|---|---|---|---|---|---|
| 3.1 | Cyan rift glow pulse (gates) | "dim cyan glow inside arch" | "bright cyan glow inside arch" | 6 | YES (loop pulse) |
| 3.2 | Crack glow flicker | "dark inert crack" | "cyan glowing crack hot" | 4 | YES (loop flicker) |
| 3.3 | Lantern flame flicker | "small steady amber flame" | "large flickering amber flame" | 6 | YES |
| 3.4 | Banner sway | "banner straight down" | "banner blown left 15°" | 8 | YES |
| 3.5 | Rift seepage pulse | "small cyan seep" | "expanded cyan seep" | 6 | YES |

Prompt template'lerini Phase 2 asset'leri tamamlanınca sana veririm (her efekt için ayrı dosya).

### PHASE 4 — Room editing automation (Brush V1 enhancement)

**Goal:** Sen Map Designer'da bir oda seçip, Brush V1 ile asset'leri **istediğin gibi swap edebilesin**.

**Mevcut altyapı:**
- Brush V1 (Map Designer/Brush Tool window) LIVE
- Asset Pack Browser (Map Designer/Asset Pack Browser) LIVE
- Multi-Layer Painter Inspector LIVE
- RoomTemplateSO data structure LIVE

**Eksik:**
- Asset Pack Browser'ın `Assets/Art/AssetPacks/` yeni hiyerarşiyi okuduğunu doğrula (Codex review gerek)
- Quick swap action ("bu odadaki tüm L4 patches'i variant 2'ye değiştir")
- Procedural placement preset (gate near = moss, wall near = debris, center = clear)

**Codex task:** Brush V1 enhancement (Phase 4 sonunda dispatch)

### PHASE 5 — Apply + iterate

> **Karar #149 sub-room aware:** "Combat Room" ve "Elite Room" artik 3-5 sub-room EncounterTemplateSO. Asagidaki "oda" referanslari = her sub-room (16x10 default) icin ayri kompozisyon. Spawn_01 = standalone room (encounter dis). Phase 2-3 asset'leri her sub-room template'e ayri uygulanir.

Spawn_01 → Combat Room → Elite Room sirasi:

1. Spawn_01'i Phase 2-3 yeni asset'leri ile yeniden compose (concept'e en yakin)
2. Combat Room olustur — her sub-room icin ayri RoomTemplateSO: sub-room 1 (giris/warmup), sub-room 2 (pressure), final sub-room (peak + reward)
3. Elite Room (daha cok L4 patch density, daha dramatic walls — her sub-room'a ayri apply)
4. Boss Room (Phase 6, future)

Her sub-room compose sonrasi screenshot vs concept reference compare → gap'i belirle → iterasyon.

## Unity scene cleanup actions (PHASE 1 detail)

Sahnede şu an temiz, project temizliği gerek:

### Project temizlik listesi

```
SİL (boş veya legacy):
- Assets/Art/Rooms/AssetPack/FloorStones/ (boş, asset _Universal'a taşındı)
- Assets/Art/Rooms/AssetPack/LargePatches/ (boş, Act1'e taşındı)
- Assets/Art/Tiles/F1/FlatTileset_GraniteV2/ (boş, Act1'e taşındı)
- Assets/Art/Tiles/F1/SeamlessV1/ (boş)
- Assets/Resources/Environment/StoneDungeon/Walls/ (boş)
- Assets/Sprites/Environment/RIMA_Painterly_Pack_v1/ (audit, sub-klasörler boşalmış olabilir)

AUDIT VE KARARA BAĞLA:
- Assets/Scenes/Archive/ (eski sandbox — sil veya korumaya karar)
- Assets/Tiles/Act1/decor_gate_01-04.asset (mevcut tile decoration — Act1 pack'e taşı?)
- Assets/Resources/ — eski legacy sprite var mı kontrol

KEEP:
- Assets/Art/Reference/ (concept art reference)
- Assets/Art/AssetPacks/ (yeni hiyerarşi)
- Assets/Art/Characters/ (sprite anchors)
- Assets/Scripts/ (kod)
- Assets/Editor/ (Map Designer tools)
- Active scene (RoomPipelineTest)
```

### Sahnede tutulacak GameObject hierarchy

```
Main Camera (Branch E tilt 6°)
Global Light 2D
EventSystem
Main Light (Directional, 2D Light fallback)
Spawn_01_NewTileSystem
├── L1_BaseFloor (Tilemap, granite_pure_noise + dark tint)
├── L3_Walls
│   ├── WallTop_0..N (temporary pebble stone — Phase 2.1 sonrası swap)
│   ├── WallBottom_0..N
│   ├── Gate_Entry (temporary dark arch — Phase 2.2 sonrası swap)
│   └── Gate_Exit
├── L4_Patches (semantic: gate önü moss+dust, wall altı crack+debris)
└── L5_Scatter (gate yanı stone cluster, wall base scatter)
Warblade_Player (sortingOrder 50)
```

## Generation budget tracking

| Tool | Kalan | Plan kullanım | Bittiği yer |
|---|---|---|---|
| PixelLab gen budget | ~4250 | Phase 2.4 + 2.5 + 2.6 = ~50-80 gen + V3 anims ~30 gen = ~110 | Phase 3 sonu |
| Codex gpt-image-1 (built-in) | $0 cost | Phase 2.3 floor variant = 1-2 call | Phase 2 sonu |
| Codex CLI gpt-image-1 | Need OPENAI_API_KEY | Yok | — |
| User PixelLab web UI (SEN paste) | Unlimited (subscription) | Phase 2.1 + 2.2 + Phase 3 anims | Phase 3 sonu |

**Total gen budget impact:** ~110 / kalan 4250 = %2.5 budget. Kalan budget Phase 5 iterasyon + Phase 6 boss + Phase 7 Combat/Elite room polish için.

## Decision queue (user approval needed)

1. **Plan onayla** — bu sıralı plan + RIMA identity reframe (stones+grass → cracks+rifts) OK mi?
2. **Phase 1 cleanup başlasın mı?** (~15 dk, project temizliği)
3. **Phase 2.1 wall cap prompt'unu PixelLab web UI'ya yapıştırmak için hazır mısın?** (sen paste, ben sahneye atarım)
4. **Phase 2.3 floor variant Codex'e dispatch edeyim mi?** (paralel)

Onaylarsan Phase 1 cleanup şimdi başlatırım, Phase 2'ye geçeriz.
