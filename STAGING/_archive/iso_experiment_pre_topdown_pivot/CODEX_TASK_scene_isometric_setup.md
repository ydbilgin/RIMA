# Codex Task — Scene Isometric Setup (Painter Ready)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

UnityMCP REQUIRED.

---

## Görev

`IsoShowcaseRoom_S95.unity` sahnesini **proper Isometric Tilemap setup** yapısına convert et. User Painter ile boyamak istiyor — Scene view iso görünmeli, math doğru çalışmalı.

Memory `project-isometric-floor-pivot-s95` LOCK referans alınmalı.

## Workflow

### Step 1 — Scene Audit + Cleanup
1. `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` aç
2. Mevcut manual placed wall GameObject'ler (önceki dispatch'te 48 wall manuel + Tilemap migration sonrası 0 olmalı, doğrula)
3. Eğer hâlâ orphan manuel obje varsa temizle:
   - 23 wall manual (eski drift)
   - 14 prop manual
   - 320 manual floor tile GameObject (eğer Tilemap'e migrate edilmediyse)
4. Korunan: brazier light source GameObject'leri + player + camera

### Step 2 — Isometric Grid Setup

**Grid GameObject (root):**
- Component: `Grid`
- `Cell Layout`: **Isometric Z As Y** (cellLayout=4)
- `Cell Size`: **(1, 0.5, 1)** — diamond ratio 2:1
- `Cell Swizzle`: XYZ

**FloorTilemap (Grid child):**
- Component: `Tilemap` + `TilemapRenderer`
- Sorting Layer: "Floor"
- Order in Layer: 0

**WallTilemap (Grid child):**
- Component: `Tilemap` + `TilemapRenderer`
- Sorting Layer: "Walls"
- Order in Layer: 20
- (RuleTile asset `Act1_WallRuleTile.asset` mevcut, paint için hazır)

**Props_Root (Grid child empty):**
- Parent for individual prop GameObjects (statue, pillar, brazier vs.)

**Lighting_Root (Grid child empty):**
- Parent for Light2D objects

### Step 3 — Project Settings

UnityMCP `manage_graphics`:
- `Edit > Project Settings > Graphics > Camera Settings > Transparency Sort Mode`: **Custom Axis**
- `Transparency Sort Axis`: **(0, 1, 0)** — Y dominant for iso depth

### Step 4 — Camera Setup

Main Camera (varsa) kontrol:
- Projection: **Orthographic**
- Ortho Size: **5** (game view yakın görünüm, fazla geniş değil)
- Position: (0, 0, -10) standart 2D
- Rotation: (0, 0, 0) — sprite zaten iso angle ile rendered, camera rotation YOK

### Step 5 — Floor Paint (Mevcut 3 Granite Variant)

Mevcut 3 iso floor variant ile **16×10 grid floor** paint et (Painter veya kod):
- `act1_iso_granite_clean.png` — 60%
- `act1_iso_granite_worn.png` — 30%
- `act1_iso_granite_chiseled.png` — 10%

FloorTilemap'e Tile asset'leri yarat (varsa reuse):
- `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/granite_clean.asset`
- `granite_worn.asset`, `granite_chiseled.asset`

Veya direct sprite reference Tile asset.

Random pool paint için:
- `RandomTile` (Unity 2D extras package) kullan
- Veya basit script: per-cell random.value > 0.4 ? clean : (random > 0.7 ? chiseled : worn)

### Step 6 — Wall Test (Mevcut Pilot A RuleTile)

`Act1_WallRuleTile.asset` (mevcut, önceki dispatch) ile **16×10 perimeter rectangle** paint et:
- WallTilemap'e RuleTile asset ile çiz
- North wall row 9: 16 cell (face_NS auto-pick)
- South wall row 0: 14 cell + 2 cell arch_opening (manual)
- East/West walls: face_EW
- 4 corner: corner_outer (RuleTile auto-pick)

### Step 7 — Verification

1. Scene View'ı **2D mode** topgle aktif
2. Görsel kontrol: Floor diamond pattern iso layout görünüyor mu (90° rotation YOK, diamond düzeni)
3. Walls perimeter rectangle olarak iso projection ile çevreliyor mu
4. Painter aç (`RIMA/Tools/World Painter`):
   - Floor Paint mode test: 1-2 cell paint et, doğru cell'e gidiyor mu?
   - Wall Paint mode test: 1-2 cell paint et, RuleTile auto-connect mi?
5. Scene'i save et: `IsoShowcaseRoom_S95.unity`

### Step 8 — Screenshot

`STAGING/screenshots/iso_scene_ready.png` (Scene view + Game view yan yana, painter window open)

## Output

1. Modified scene: `IsoShowcaseRoom_S95.unity` — Isometric Tilemap proper
2. 3 Tile asset (granite_clean/worn/chiseled) `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/`
3. Project Settings transparency sort axis updated
4. `STAGING/screenshots/iso_scene_ready.png`
5. `CODEX_DONE_*.md`: verification result + Painter test status

## Compile Check (HARD RULE — 2026-05-21 LOCK)

- Console hatalarını OTOMATIK fix et: dispatch sonrası `read_console` çağır
- Error/warning varsa çöz + recheck
- Hâlâ hata varsa BLOCKED + raporla

## Kısıt

- Mevcut prop/light asset'leri **silmeyin** — sadece manual wall/floor cleanup
- Existing Pilot A RuleTile asset reuse — yeni RuleTile gen YOK
- Existing 3 granite variant reuse — yeni floor gen YOK
- Camera rotation YASAK — sprite iso baked-in zaten
- Cell Size (1, 0.5, 1) HARD LOCK — değiştirme

## Effort
medium (1-2 saat)
