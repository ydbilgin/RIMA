# Codex Task — Playable Room MVP v1 (Atmospheric Quality, No Mobs Yet)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

UnityMCP REQUIRED — Unity Editor açık olmalı.

---

## Görev — User Spec (HARD)

User reference image kalitesi (Hades/Diablo tarzı atmospheric dungeon room). Spec:

1. **Walls BAĞLANSIN** — corner pieces düzgün connect, floating wall YOK
2. **Saçma sapan gözükmesin** — coherent layout (random scatter değil)
3. **Reference-like atmospheric mood** — granite + cyan rift, torch lighting
4. **Warblade player içine yerleşsin** — WASD movement çalışsın
5. **Mob YOK** — sadece environment + player (mob sonraki iş)

Reference image özellikleri:
- Closed rectangular/octagonal room (walls form enclosure)
- Center brazier (cyan or orange flame)
- 2-3 pillars one side
- 2-3 statues other side
- Central ritual circle/altar
- Wall decorations (chains, banners, lantern)
- Cyan rift cracks on floor (overlay scatter)
- Atmospheric dark lighting

## Read These Files

1. `STAGING/_plans/progression/PROGRESSION_PLAN_v2_3_LOCK.md` — overall context
2. `CURRENT_STATUS.md` — IsoShowcaseRoom_S95 mevcut durumu
3. Asset pack inventory:
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/` (7 piece: face EW/NS, corner outer/inner, arch, T-junction, end_cap)
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/iso/` (3 granite variants)
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/rift_accents/` (cyan crack overlay)
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/pillars/` (3 pillar)
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/props/` (braziers, urns, rubble, crates, treasure)
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/statues/` (3 statue)
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/ritual/` (5 ritual elements)
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/wall_decoration/` (banners, chains, cage, lantern)
   - `Assets/Art/AssetPacks/Act1_ShatteredKeep/decals/` (cracks, bones, dust)
4. `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` — mevcut scene (23 wall + 14 prop + 320 floor + Painter LIVE)
5. `Assets/Art/Characters/Warblade/Rotations/warblade_south.png` — player sprite

## Workflow

### Step 1 — Open + Audit
- UnityMCP ile `IsoShowcaseRoom_S95.unity` aç (`mcp__UnityMCP__manage_scene`)
- Mevcut state'i incele:
  - Kaç wall placed, hangi pieces kullanılmış
  - Wall connections düzgün mü (corner pieces var mı)
  - Layout closed enclosure mı, açık mı
  - Player spawn var mı
- Screenshot `STAGING/screenshots/room_mvp_before.png`

### Step 2 — Wall Layout Audit + Fix
**HARD RULE: Walls MUST form closed enclosure.** Pilot A 7-piece kullanım kuralı:

```
KAPALI DİKDÖRTGEN LAYOUT (recommended for MVP):

       N (top — pilot_a_frame_4_face_NS × N)
   _______________________________
  |                               |
  | C                           C |   C = corner_outer (pilot_a_frame_2)
  |                               |
W |  (room interior)              | E
  |                               |
  |                               |
  | C                           C |
  |_______________________________|
       S (bottom — face_NS × N, includes 1 arch_opening for entry)

E/W walls = pilot_a_frame_1_face_EW × N
N/S walls = pilot_a_frame_4_face_NS × N
Corners = pilot_a_frame_2_corner_outer × 4
Entry arch = pilot_a_frame_3_arch_opening (1 in south wall)
```

- Suggested room size: 16×10 tile (160 floor tiles minimum)
- Wall pieces snap to grid (assume 1u per tile, walls at edges)
- Eski/wrong wall pieces varsa REMOVE et + yeniden yerleştir
- Audit kontrol: her wall piece bir komşu piece ile bağlantılı mı, gap var mı

### Step 3 — Floor Composition
- 16×10 floor tile fill (3 granite variant alternating randomly weighted: clean 60%, worn 30%, chiseled 10%)
- 8-12 cyan rift overlay decals scatter (rift_fracture_overlay placed at random tile centers, varied rotation)
- 4-6 crack decals (act1_decal_crack_var0-3) scatter
- 2-4 dust patches (act1_patch_dust_drift) low opacity

### Step 4 — Pillars + Statues + Ritual (kompozisyon)

**West side (3 pillars):**
- Pillar 1 (intact cyan crack) at (-5, 0, 3)
- Pillar 2 (chained) at (-5, 0, 0)
- Pillar 3 (broken granite) at (-5, 0, -3)

**East side (2 statues + 1 pillar):**
- Statue 1 (warrior_intact + pedestal_base) at (5, 0, 2)
- Statue 2 (warrior_toppled + pedestal_base) at (5, 0, -2)
- Pillar (intact cyan crack) at (5, 0, 0)

**Center (ritual circle):**
- Brazier (cyan_rift_flame) at (0, 0, 0) — center light source
- Ritual stone altar at (0, 0, -2) — slightly south of brazier
- Stone marker cyan at (-1.5, 0, 1.5) + (1.5, 0, 1.5) — ritual circle corners
- Tomb headstone at (0, 0, 3) — north of brazier
- Obelisk at (0, 0, -4) — far south corner

**Floor accents around brazier:**
- Bone chip decals scatter (4-6 around brazier base)
- Small rubble debris (act1_prop_rubble_debris_small × 3-4 at scattered positions)

### Step 5 — Wall Decorations
- North wall: 2-3 banners (purple + red variants) hanging
- East wall: hanging chains (chain_hanging_long × 2)
- West wall: hanging lantern + iron grate
- Skeleton_shackled at one corner
- Iron cage_hanging at another corner
- Ivy vines scatter (2-3)

### Step 6 — Atmospheric Lighting
- Center brazier emit Point Light (cyan color, intensity 2-3, range 8u)
- 2 corner braziers (brazier_orange_flame) at NE + SW corners, each with Point Light (orange, intensity 1.5, range 5u)
- Global ambient: dark (HSV 0, 0, 0.15) — `mcp__UnityMCP__manage_graphics` ile ambient color
- Optional: Light2D for URP 2D pixel light feel

### Step 7 — Warblade Player Setup
- Player GameObject yarat (varsa kullan):
  - Path: `Assets/Prefabs/Player/WarbladePlayer.prefab` (varsa)
  - Sprite: `Assets/Art/Characters/Warblade/Rotations/warblade_south.png`
  - Components: SpriteRenderer + Rigidbody2D (kinematic) + CircleCollider2D + Animator (eğer existing) + PlayerMovement script
- Spawn position: (0, 0, -5) — south entry near arch
- Camera: Main Camera follow Player (Cinemachine Virtual Camera 2D OR simple LateUpdate follow script)
- WASD movement script (varsa existing reuse, yoksa minimal `PlayerMovementController.cs` yaz: 4-yön WASD, 4u/s speed, no diagonal normalization for clean orth movement)

### Step 8 — Final Screenshot + Compile Check
- Save scene: `IsoShowcaseRoom_S95.unity`
- Screenshot `STAGING/screenshots/room_mvp_after.png` (game view, run mode kapalı)
- Run mode test: Play → Warblade WASD ile move ediyor mu (5sn test)
- Console check: 0 error, 0 warning
- Report: before vs after composition, player movement verification

## Output

1. Modified scene: `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity`
2. `STAGING/screenshots/room_mvp_before.png`
3. `STAGING/screenshots/room_mvp_after.png`
4. `STAGING/screenshots/room_mvp_playmode_warblade.png` (player visible in scene)
5. `CODEX_DONE_*.md`: Visual quality verdict, compile status, player movement OK/FAIL

## Kısıt

- **Mob YASAK** — sadece environment + player
- **Floating wall YASAK** — her wall corner/junction ile bağlantılı
- **Random scatter YASAK** — kompozisyon Step 4'teki konum sketch'ine sadık
- **Yeni asset gen YASAK** — sadece mevcut Act 1 pack'ten yararlan
- **Painter tool kullanım izinli** — `RimaWorldPainterWindow` ile floor + decal scatter hızlandırılabilir
- **Player movement existing reuse öncelik** — varsa eski WASD controller, yoksa minimal yeni yaz

## Effort
high
