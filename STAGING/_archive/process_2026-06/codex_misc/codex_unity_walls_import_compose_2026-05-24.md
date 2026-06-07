# Codex Task — 17 Wall Asset Unity Import + Test Scene Compose (2026-05-24)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

Amaç: 17 wall asset'i (bdkrtgasb commit fd00ad23) Unity'ye import et, wall prefab'larını oluştur, **1 test scene compose et**, Warblade karakteri yerleştir, Play mode'da görünür olsun. Hedef: 1-2 saatte playable visual.

**UnityMCP zorunlu.** Unity açık olmalı (user'a hatırlat).

---

## Source Assets

**17 PNG at:** `STAGING/concepts/fractured_chamber/iso_assets/`

```
Walls (11):
  wall_nw_mid_plain.png       (128x384)
  wall_nw_mid_variant.png     (128x384)
  wall_nw_mid_broken.png      (128x384)
  wall_nw_doorway.png         (128x384)
  wall_ne_mid_plain.png       (128x384)
  wall_ne_mid_variant.png     (128x384)
  wall_ne_mid_broken.png      (128x384)
  wall_ne_doorway.png         (128x384)
  wall_n_corner.png           (256x384)
  wall_n_landmark.png         (256x384)
  wall_pillar_universal.png   (64x384)

Floors (6):
  iso_floor_clean.png         (128x64)
  iso_floor_cracked.png       (128x64)
  iso_floor_rift_glow.png     (128x64)
  iso_floor_broken.png        (128x64)
  iso_floor_edge_light.png    (128x64)
  iso_floor_debris.png        (128x64)
```

---

## Step 1 — Unity Editor State Check

UnityMCP ile:
1. `mcpforunity://editor_state` oku — Unity açık mı, compiling mi
2. `mcpforunity://project_info` oku — projenin URP config'i
3. `mcp__UnityMCP__read_console` — mevcut error/warning yok mu

Eğer Unity kapalı → BLOCKED yaz ve user'a "Unity'yi açın" mesajı bırak.

---

## Step 2 — Sprite Asset Import (17 PNG)

**Target folder:** `Assets/Art/Walls/Act1_ShatteredKeep/HighTopDown_3_4/`

Eğer klasör yoksa oluştur. Eğer eski wall asset'ler varsa orada (S99-S102 dönemi), DOKUNMA — sadece YENİ klasöre yaz.

**Import settings (her sprite):**
- `Pixels Per Unit`: 64 (LOCKED Karar #74)
- `Filter Mode`: Point (no filter)
- `Compression`: None
- `Sprite Mode`: Single
- `Pivot`: Custom — Bottom-Center
  - Wall (128×384): pivot at (0.5, 0.0) — wall'un altı zemine değer
  - Wall_n_landmark (256×384): pivot (0.5, 0.0)
  - Pillar (64×384): pivot (0.5, 0.0)
  - Floor (128×64): pivot (0.5, 0.5) — floor center
- `Max Size`: 512 (yeterli, 384 cap altında)
- `Generate Physics Shape`: false (manuel collider)

UnityMCP `manage_asset` veya AssetDatabase + TextureImporter API.

**Verify:** 17 sprite asset Asset/Art/Walls/.../ altında, AssetDatabase.Refresh sonrası `read_console` clean.

---

## Step 3 — Wall Prefab Generation (17 prefab)

**Target folder:** `Assets/Prefabs/Environment/Walls/Act1_ShatteredKeep/HighTopDown_3_4/`

Her sprite için 1 prefab:
- GameObject root (asset adıyla aynı, örn `wall_nw_mid_plain`)
- **SpriteRenderer** component:
  - Sprite: ilgili import edilen sprite
  - Sorting Layer: "Default" (veya `Walls` varsa)
  - Order in Layer: 0 (Y-sort dynamic olacak)
  - Material: default sprite material
- **BoxCollider2D** component:
  - Size: sprite bounds'un bottom 64px (gameplay collision sadece ground footprint)
  - Offset: y = -160 (canvas alt 64px)
  - Wall: 128×64 collider
  - Landmark: 256×64
  - Pillar: 64×64
  - Floor: collider YOK (walkable)
- **(Opsiyonel) Y-sort component**: Eğer RIMA'da varsa `YSortByTransform.cs` benzeri ekle, axis = (0, 1, 0). Yoksa atla, Custom Axis Tilemap setting yeter.

Floor prefab'lar collider YOK, sadece SpriteRenderer + SortingLayer = "Floor".

UnityMCP `manage_prefabs` veya `execute_code` ile editor script.

**Verify:** 17 prefab `Assets/Prefabs/Environment/Walls/.../` altında, hiçbiri missing reference, `read_console` clean.

---

## Step 4 — Test Scene Compose

**Scene path:** `Assets/Scenes/Demo/TopDownTest_HighTopDown_3_4.unity` (NEW)

Eğer `ModularKitRoomTest.unity` varsa onu base alabilirsin (UnityMCP `manage_scene get`), ama yeni scene tercih.

### 4a. Scene setup
- New scene oluştur, URP 2D template
- **Main Camera**:
  - Orthographic
  - Size: 5 (PPU=64 ile ~640px viewport height)
  - Component: `PixelPerfectCamera`:
    - PPU: 64
    - Filter Mode: Point
    - Upscale Render Texture: ON
    - Pixel Snapping: ON
    - Crop Frame: None
- **Global Light 2D**: intensity 0.6, color cool grey #8090A0

### 4b. Floor layer (Tilemap veya manual sprites)
Manuel ile başla (Tilemap setup kompleks olabilir):
- 10×10 grid floor sprite GameObject'leri instantiate
- Random `iso_floor_clean / cracked / rift_glow / debris` mix
- Spacing: x=128, y=64 (sprite boyutuna göre)
- Sorting Layer: "Floor"
- Parent: `WorldRoot/Floor` empty GameObject

### 4c. Wall composition (1 oda, ~10-12 wall + 4 pillar)
"Diamond-ish footprint" yapay (rectangular ama köşelerde bevel):
- **Back wall chain (N family)**: 4-5 wall_n_corner/landmark/pillar
- **Right wall chain (NE)**: 3 wall_ne_mid_plain/variant
- **Left wall chain (NW)**: 3 wall_nw_mid_plain/variant
- **Front edge**: open (ön açık parapet — wall placement yok)
- **Pillars (4 köşede)**: wall_pillar_universal x4 at corners
- **1 doorway**: wall_ne_doorway veya wall_nw_doorway

Wall positioning rules:
- Z = 0 (2D)
- Sorting Layer: "Walls"
- Y position: floor row'a göre Y-sort tarafından order belirlenecek
- Spacing: piece width'e göre (128 grid, pillar arası 128 px)

Parent: `WorldRoot/Walls` empty GameObject

### 4d. Warblade character (yer tutucu)
Eğer `Assets/Prefabs/Characters/Warblade/Warblade_Test.prefab` veya benzer varsa onu instantiate. Yoksa basit GameObject:
- SpriteRenderer + `Assets/Art/Characters/Warblade/Rotations/warblade_south.png`
- Position: oda merkezi
- Sorting Layer: "Characters"
- Y-sort

### 4e. Lighting accent
- 2 Point Light 2D (warm torch sim):
  - Color #FF8000
  - Intensity 1.2
  - Outer Radius 3
  - Position: 2 NW pillar yakını
- 1 Point Light 2D (cyan rift accent):
  - Color #00DDFF
  - Intensity 0.8
  - Outer Radius 2
  - Position: oda ortası

### 4f. Save scene

UnityMCP `manage_scene save`.

---

## Step 5 — Verification

1. `mcp__UnityMCP__read_console` — error/warning yok
2. Scene'i Play mode'a sok (mcp__UnityMCP__manage_editor play_mode start)
3. Wait 2-3 sec
4. Stop play mode
5. read_console clean mi

---

## Step 6 — Screenshot (opsiyonel ama önerilen)

Eğer UnityMCP screenshot capability varsa:
- Scene view veya Game view screenshot al
- Path: `STAGING/screenshots/topdown_test_compose_v1.png`

Yoksa atla.

---

## Constraints

- **DOKUNMA**: existing `RimaWorldPainterWindow.cs`, `WallOverlayPainter.cs`, ne de mevcut S99-S102 wall prefab'ları
- **CREATE ONLY**: yeni klasör + yeni prefab + yeni scene
- **HER DEĞIŞİKLİKTE**: AssetDatabase batch wrapper kullan (HARD RULE — feedback_unity_safety_protocol.md)
- **CONSOLE CHECK**: Her major adımdan sonra `read_console` (HARD RULE)

---

## Çıktı Raporu

`STAGING/codex_unity_walls_compose_DONE.md` yaz:
- Created files list (sprites + prefabs + scene)
- Console status (clean / errors found + how resolved)
- Play mode test result (loaded / failed)
- Screenshot path (varsa)
- Visual observations (her şey görünüyor mu, Y-sort doğru mu, lighting OK mi)
- Issues / blockers
- Next step önerisi (örn: "Y-sort ince ayar gerekli", "lighting çok soğuk")

git commit otomatik yapma — orchestrator review sonrası.

---

## Effort Estimate

1-2 saat Codex execution. UnityMCP tool call sayısı ~50-100.
