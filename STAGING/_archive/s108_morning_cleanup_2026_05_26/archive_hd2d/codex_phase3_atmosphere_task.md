# Phase 3 — Atmosphere + Lighting + 4 Room Library Variants

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NLM ACCESS:** If you need RIMA design context, query NLM first via:
`uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE.md AS THE VERY LAST STEP.

---

## Context

Phase 1 (b502fb79) + Phase 2 (dd6cf34) built modular textures + footprint-driven room shell builder. SampleScene BigArena (30x22) is structurally correct but visually flat: uniform directional light, gray ambient, no atmosphere, single wall mix style. Reference `STAGING/concepts/chatgpt_ref/` shows DRAMATIC lighting (warm torch + cold cyan), DARK ambient, ROOM STYLE variation (clean vs damaged vs ritual vs rift).

Phase 3 closes the visual gap WITHOUT decor objects or new floor textures (those come from PixelLab later). Pure lighting + wall variation pass.

---

## STEP 0 — Required reads

1. `CLAUDE.md` + `CODEX_DISPATCH.md`
2. `CURRENT_STATUS.md`
3. `STAGING/codex_phase2_room_shell_task.md` (the previous spec)
4. `STAGING/concepts/chatgpt_ref/` — confirm warm/cold contrast, damaged wall appearance
5. `Assets/Scripts/Environment/Modular/WallModuleLibrary.cs` — the data model you will extend
6. `Assets/Data/Environment/WallLib_ShatteredKeep.asset` — baseline you'll duplicate

---

## STEP 1 — Create 3 new wall variant prefabs

Path: `Assets/Prefabs/Environment/`

### 1a. `WallSegment_Breach.prefab`
- Cube primitive scale (2, 4, 0.5), material `WallMat_StoneB` (cracked)
- Add child Cube primitive scale (0.8, 1.5, 0.4) at local pos (0, -1.25, -0.1) using `WallMat_StoneA` — represents rubble pile at base of breach
- Pivot via empty parent at floor level
- Single combined BoxCollider on parent matching outer bounds (2, 4, 0.5)
- Purpose: heavily damaged wall section

### 1b. `WallSegment_Toppled.prefab`
- Cube primitive scale (2, 2.2, 0.5), material `WallMat_StoneB` — only HALF height of normal wall
- Pivot at floor level via empty parent
- BoxCollider auto (smaller than standard)
- Purpose: collapsed/partial wall, creates skyline variation

### 1c. `WallSegment_Heavy.prefab`
- Cube primitive scale (2, 5, 0.6), material `WallMat_StoneA` — TALLER + slightly thicker
- Pivot at floor
- BoxCollider auto
- Purpose: imposing fortified wall, increases skyline variation when mixed in

---

## STEP 2 — Extend WallModuleLibrary.cs (additive only)

Add optional fields. Do NOT remove existing fields.

```csharp
[Header("Damaged variants (optional)")]
public GameObject[] breachPrefabs;       // wall sections with rubble breach
public GameObject[] toppledPrefabs;      // half-height collapsed walls
public GameObject[] heavyPrefabs;        // taller imposing walls

[Range(0f, 1f)] public float breachChance = 0f;
[Range(0f, 1f)] public float toppledChance = 0f;
[Range(0f, 1f)] public float heavyChance = 0f;

[Header("Theme tint")]
public UnityEngine.Color ambientTintRGB = UnityEngine.Color.white;  // applied to material instances on built walls
public bool tintWalls = false;
```

Update `RoomShellBuilder.cs` to roll for these variants when picking a wall segment: weighted random (heavy > breach > toppled > variant > straight). Tint logic optional — if `tintWalls=true`, apply `ambientTintRGB` to MeshRenderer.sharedMaterial color slot at build time. Keep changes surgical and clearly grouped.

---

## STEP 3 — Create 4 WallLibrary asset variants

Path: `Assets/Data/Environment/`

All share the same prefab pool from Phase 1 + new variants from STEP 1. Differ in mix percentages and tint.

### 3a. `WallLib_Standard.asset` — clean baseline
- straight pool: [WallSegment_Straight]
- variant pool: [WallSegment_Niche]
- wallVariantChance: 0.15
- breachChance: 0
- toppledChance: 0
- heavyChance: 0.1
- pillarEveryNCells: 4
- tintWalls: false

### 3b. `WallLib_Damaged.asset` — heavy combat damage
- straight: [WallSegment_Straight]
- variant: [WallSegment_Cracked, WallSegment_Niche]
- wallVariantChance: 0.5
- breachChance: 0.25
- toppledChance: 0.2
- heavyChance: 0
- pillarEveryNCells: 5
- tintWalls: false

### 3c. `WallLib_Ritual.asset` — ceremonial chamber
- straight: [WallSegment_Straight]
- variant: [WallSegment_Niche, WallSegment_Niche, WallSegment_Cracked]   (niche-heavy)
- wallVariantChance: 0.5
- breachChance: 0
- toppledChance: 0
- heavyChance: 0.25
- pillarEveryNCells: 3                                                   (pillar-heavy)
- tintWalls: false

### 3d. `WallLib_Rift.asset` — broken architecture, cold corruption
- straight: [WallSegment_Straight]
- variant: [WallSegment_Cracked, WallSegment_Niche]
- wallVariantChance: 0.4
- breachChance: 0.15
- toppledChance: 0.3
- heavyChance: 0.1
- pillarEveryNCells: 4
- tintWalls: true
- ambientTintRGB: RGB(0.55, 0.7, 0.85) — cold cyan-blue tint

Replace existing `WallLib_ShatteredKeep.asset` references in SampleScene's RoomShellBuilder with `WallLib_Damaged` (it is the closest visual descendant).

---

## STEP 4 — Create 2 lighting prefabs

Path: `Assets/Prefabs/Environment/Lighting/`

### 4a. `TorchLight.prefab`
- Empty GameObject + Light component
- Light Type: Point
- Color: #FF8030 (warm orange)
- Intensity: 8
- Range: 6
- Shadows: Soft
- No sprite, no flicker animation, no particle — pure light source

### 4b. `RiftLight.prefab`
- Empty GameObject + Light component
- Light Type: Point
- Color: #7BBCFF (cold cyan-blue)
- Intensity: 5
- Range: 5
- Shadows: None

---

## STEP 5 — Atmosphere setup

### 5a. Global Volume profile
Create or edit `Assets/Settings/Environment/DungeonVolume_Profile.asset` (URP Volume Profile). Settings:
- Bloom: enabled, threshold 0.9, intensity 0.4
- Color Adjustments: contrast 15, post-exposure -0.5 (darker base)
- Vignette: intensity 0.35, smoothness 0.7
- White Balance: temperature -10 (slightly cool)

Apply this profile to the existing `Global Volume` GameObject in `SampleScene`. Keep Mode = Global.

### 5b. Lighting settings (per scene)
For SampleScene:
- Ambient Source: Color
- Ambient Color: RGB(0.05, 0.06, 0.10) — near-black with cold blue hint
- Directional Light intensity: 0.25 (drop from default 1.0)
- Directional Light color: RGB(0.7, 0.8, 1.0) — cool moonlight
- Directional Light rotation: (60, 330, 0)
- Skybox: keep URP default but it should be hidden by point lights + dark ambient

For RoomShowcase: same ambient + directional config.

---

## STEP 6 — SampleScene atmospheric demo

1. Open `Assets/Scenes/SampleScene.unity`
2. Existing `Room_Demo` GameObject (RoomShellBuilder with BigArena footprint): swap `library` field to `WallLib_Damaged.asset`. Rebuild.
3. Add child empty `Lighting` under Room_Demo. Place:
   - 6x TorchLight prefab instances along walls (at niche cell positions where possible). Y=2.5, slight offset from wall toward room interior by 0.5 units.
   - 2x RiftLight prefab instances at random sigil floor tile positions if any, else at (-5, 1, 5) and (5, 1, 5). Y=1.
4. Player + 4 mobs: keep current positions inside footprint, push slightly back to keep player in well-lit foreground.
5. Apply atmosphere settings from STEP 5.
6. Save scene.

---

## STEP 7 — RoomShowcase 4-variant comparison

1. Open `Assets/Scenes/RoomShowcase.unity` (Phase 2 created).
2. Replace existing 6 footprint demo with **4 mid-size MedRect_12x12 rooms** side-by-side (spacing 24 units between centers along X):
   - Room A at (-36, 0, 0): library = WallLib_Standard
   - Room B at (-12, 0, 0): library = WallLib_Damaged
   - Room C at (+12, 0, 0): library = WallLib_Ritual
   - Room D at (+36, 0, 0): library = WallLib_Rift
3. Each room gets 4x TorchLight at corner-ish wall positions + 1 RiftLight near center if it's Rift room.
4. Add labels above each room: TextMesh world-space billboarded labels "STANDARD" / "DAMAGED" / "RITUAL" / "RIFT" at y=8 above room center. Use Unity TextMesh (legacy 3D TextMesh, NOT TextMeshPro — keeps dependencies light).
5. Camera: position (0, 18, -32), rotation (30, 0, 0), orthographic size 18 — wide enough to see all 4 rooms.
6. Apply STEP 5 atmosphere settings.
7. Save.

---

## STEP 8 — Verify

1. `read_console` MCP → must be clean.
2. Screenshot SampleScene → `Assets/Screenshots/codex_phase3_atmosphere_v1.png`
3. Screenshot RoomShowcase → `Assets/Screenshots/codex_phase3_showcase_4libs.png`
4. Visual check:
   - SampleScene: dark moody, torch warm pools + rift cold pools visible, walls mixed straight/cracked/breach/toppled, ChatGPT_ref dramatic feel approached
   - RoomShowcase: 4 visibly distinct rooms with different damage states + tint

---

## STEP 9 — Commit + report

Commit message:
```
[Codex] [S103 PHASE3 ATMOSPHERE] URP volume + 2 light prefabs + 3 wall variants + 4 wall libs + atmospheric scenes

- DungeonVolume profile: bloom + dark contrast + vignette
- Ambient near-black cold blue + directional intensity 0.25
- TorchLight (warm) + RiftLight (cold) point light prefabs
- Wall variants: Breach (rubble), Toppled (half-height), Heavy (taller)
- 4 wall libraries: Standard / Damaged / Ritual / Rift (varying mix + tint)
- WallModuleLibrary extended: breachChance + toppledChance + heavyChance + ambientTintRGB
- SampleScene BigArena uses Damaged lib + 6 torches + 2 rift lights
- RoomShowcase: 4 MedRect rooms side-by-side, one per library, labeled
- No decor sprites (Phase 4), no new floor textures (PixelLab pipeline)

Co-Authored-By: Codex (GPT 5.5) <noreply@antigravity.dev>
```

Write to `CODEX_DONE.md`:
- STATUS / COMMIT / FILES_TOUCHED
- LIBRARIES_CREATED: 4 paths
- LIGHT_PREFABS_CREATED: 2 paths
- WALL_VARIANTS_CREATED: 3 paths
- SCREENSHOTS: 2 paths
- ISSUES
- NEXT_SIGNAL: "phase3_atmosphere_complete"

---

## Constraints

- HD-2D BACK-LEFT camera angle preserved (35° pitch, -45° yaw).
- NO decor sprites (torches, banners, altars, debris, statues) — Phase 4 PixelLab pipeline.
- NO new floor textures — Phase 4 PixelLab.
- NO new C# scripts outside the additive WallModuleLibrary + RoomShellBuilder edits.
- Z-sort (Custom Axis Y) stays enabled — do not modify Graphics Settings.
- Wall prefabs use existing materials (WallMat_StoneA / WallMat_StoneB) — do not create new materials.
- Light prefab Intensity values are HDR-friendly; if URP HDR off, the lights still render but reduce intensity proportionally.
- If URP Volume profile already has settings authored: PRESERVE them, layer new entries non-destructively.
- STOP after STEP 9. Phase 4 (decor sprites via PixelLab pipeline) is separate dispatch.
