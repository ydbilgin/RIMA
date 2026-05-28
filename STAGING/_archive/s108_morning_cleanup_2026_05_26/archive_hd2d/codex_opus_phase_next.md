# Phase 4a — Make It Glow (Lights + Pillar Protrusion + Sigil Emission + Bloom Boost)

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**NLM ACCESS:** If you need RIMA design context, query NLM first via:
`uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

**CAMERA LOCK (DO NOT MODIFY):** Main Camera position (12, 8, -12), rotation (35, 315, 0), orthographic size 9. FROZEN. Any drift triggers CameraLockController warning; do not change values, do not disable controller.

**PLAIN RECT FORBIDDEN:** Do not author any new footprint, and do not flatten any existing irregular footprint. Footprints are the responsibility of an earlier phase and are locked.

ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE.md AS THE VERY LAST STEP.

---

## Context (verbatim — what previous phases produced)

- **Phase 1 (commit b502fb79):** 6 textures + 6 materials + 5 modular prefabs (Wall_Straight/Cracked/Niche, Pillar, Floor_2x2/Sigil).
- **Phase 2 (commit dd6cf34):** RoomFootprint + WallModuleLibrary + RoomShellBuilder + 7 footprints + WallLib_ShatteredKeep.
- **Phase 3 (Codex atmosphere phase):** DungeonVolume_Profile.asset + 4 wall libraries (Standard/Damaged/Ritual/Rift) + TorchLight.prefab + RiftLight.prefab + WallSegment_Breach/Toppled/Heavy prefabs. SampleScene Room_Demo uses WallLib_Damaged.
- **Camera Lock + Irregular Footprints (commit 56dab735):** CameraLockController + 11 irregular footprints. SampleScene Room_Demo now uses `RF_BreachedArena_22x18` (interior breach gap pattern).

**Current visual state (`Assets/Screenshots/codex_camera_lock_v1.png`):** Floor reads as void black. Walls are dark gray planes. Characters barely lit. No warm light pools visible. Camera frame OK.

**Opus gap analysis (`STAGING/opus_gap_analysis.md`):** Three load-bearing findings:
1. Phase 3 created the light prefabs but the SampleScene has **0 TorchLight and 0 RiftLight instances**. The "6 torches + 2 rift lights" step never executed.
2. `RoomShellBuilder.cs` line 79 spawns pillars *at the wall edge position* instead of pushing them 0.4–0.6 units toward room interior — pillars are co-planar with walls, providing no silhouette break.
3. `FloorMat_SigilB._EmissionColor = (0,0,0)`. Sigil tiles are "sigil" by name only — they do not glow.

This dispatch closes all three findings + boosts bloom intensity. **No new C# classes. No PixelLab. No new textures.** Pure scene/material/asset edits.

---

## STEP 0 — Required reads

1. `CLAUDE.md` + `CODEX_DISPATCH.md`
2. `CURRENT_STATUS.md`
3. `STAGING/opus_gap_analysis.md` (the diagnosis driving this dispatch)
4. `Assets/Scripts/Environment/Modular/RoomShellBuilder.cs` (the one line you will modify)
5. `Assets/Scripts/Environment/Modular/WallModuleLibrary.cs` (read-only — confirm the `pillarPrefab` and `pillarEveryNCells` fields exist)
6. `Assets/Data/Environment/Footprints/RF_BreachedArena_22x18.asset` (read-only — know the interior gap positions before placing lights)
7. `Assets/Prefabs/Environment/Lighting/TorchLight.prefab` + `RiftLight.prefab` (the prefabs you will instance)
8. `Assets/Materials/Environment/FloorMat_SigilB.mat` (you will set emission)
9. `Assets/Settings/Environment/DungeonVolume_Profile.asset` (you will boost bloom)

---

## STEP 1 — Pillar protrusion (RoomShellBuilder.cs, 1 file, ~6 lines)

`Assets/Scripts/Environment/Modular/RoomShellBuilder.cs` line 77–80 currently:

```csharp
if (library.pillarPrefab != null && library.pillarEveryNCells > 0 && x % library.pillarEveryNCells == 0)
{
    Spawn(library.pillarPrefab, parent, position, Quaternion.identity);
}
```

Change to push pillars toward room interior on the camera-facing axis by `pillarInteriorOffset` cells. Add **one optional float field** to `WallModuleLibrary.cs` (additive, no break):

```csharp
[Header("Pillar protrusion")]
[Range(0f, 1f)] public float pillarInteriorOffset = 0.4f;  // cells toward room interior, 0 = co-planar with wall
```

Then in `BuildNorthWalls`, the pillar Spawn becomes:

```csharp
var pillarPosition = position + new Vector3(0f, 0f, -library.pillarInteriorOffset * footprint.cellSize);
Spawn(library.pillarPrefab, parent, pillarPosition, Quaternion.identity);
```

Apply the **mirror** treatment in `BuildWestWalls`. The west walls face +X (rotation Y=90°). Pillars on west walls need to push in +X direction. **However the current builder does not spawn pillars on west walls.** Add an equivalent pillar block in `BuildWestWalls` matching the north pattern (same `pillarEveryNCells` check, but with offset on +X axis):

```csharp
if (library.pillarPrefab != null && library.pillarEveryNCells > 0 && z % library.pillarEveryNCells == 0)
{
    var pillarPosition = position + new Vector3(library.pillarInteriorOffset * footprint.cellSize, 0f, 0f);
    Spawn(library.pillarPrefab, parent, pillarPosition, rotation);
}
```

No new methods, no refactor — just inline the additional Spawn calls.

Rules:
- Keep all existing field names / serialization compatibility. Do not rename.
- No comments unless WHY is non-obvious.
- After save, run `read_console` and confirm clean compile.

---

## STEP 2 — Set sigil emission (1 material file)

`Assets/Materials/Environment/FloorMat_SigilB.mat`:
- Set `_EmissionColor` from `(r:0, g:0, b:0, a:1)` to `(r:0.0, g:0.6, b:1.0, a:1)` (cyan glow, HDR intensity built into the channel)
- Confirm `m_LightmapFlags: 2` or `4` (already 4 — emission contributes; do not change)
- If material asset uses URP Lit shader (it does — verify shader GUID), ensure `_EMISSION` shader keyword is enabled in `m_ShaderKeywords` / `m_ValidKeywords` list. Add `_EMISSION` if missing.

No other material edits. Do not modify `WallMat_*` or `FloorMat_StoneA.mat`.

---

## STEP 3 — Boost sigil chance in all 4 wall libraries (4 asset files)

Open each library asset and update `sigilFloorChance`:

- `Assets/Data/Environment/WallLib_Standard.asset`: 0.05 → **0.12**
- `Assets/Data/Environment/WallLib_Damaged.asset`: 0.05 → **0.15**
- `Assets/Data/Environment/WallLib_Ritual.asset`: 0.05 → **0.25**
- `Assets/Data/Environment/WallLib_Rift.asset`: 0.05 → **0.20**

These ratios feel intentional. Do not blanket-set the same number.

Do NOT touch the legacy `WallLib_ShatteredKeep.asset` (kept as historical baseline).

---

## STEP 4 — Boost bloom in DungeonVolume_Profile

`Assets/Settings/Environment/DungeonVolume_Profile.asset`:
- Bloom intensity: 0.4 → **0.85**
- Bloom threshold: 0.9 → **0.7**
- Bloom scatter: leave at default (or 0.7 if not set)
- Vignette intensity: 0.35 → **0.45**
- All other settings untouched (color adjustments, white balance — preserve)

If editing the binary YAML proves fragile, use UnityMCP tooling (`manage_asset` or `manage_graphics`) to set the values via the Editor API rather than text-editing the YAML. Either path is acceptable; do not regress unrelated fields.

---

## STEP 5 — Place lights in SampleScene (the load-bearing visual fix)

Open `Assets/Scenes/SampleScene.unity`. Find `Room_Demo` GameObject (RoomShellBuilder with `RF_BreachedArena_22x18` footprint, library `WallLib_Damaged`).

The footprint cell size is 2.0 and origin is at footprint center. Footprint widthCells=11, heightCells=9. Interior breach occupies cells around the center; perimeter is the outer 1–2 cell ring.

Under `Room_Demo`, create a child empty GO named `Lighting`. Under `Lighting`, instance the following prefabs at the listed **local positions** (relative to Room_Demo). All Y values are in world units, Y=2.5 for torches (wall-mounted height), Y=1.2 for rift floor lights.

**6 TorchLight instances** (place around the OUTER perimeter, on solid floor cells, slightly inset from walls toward room interior by ~0.6 units to read as wall-mounted torches not buried inside walls):

| Index | Local Pos (x, y, z)   | Logical position                          |
|-------|-----------------------|-------------------------------------------|
| 1     | (-9.4, 2.5,  6.4)     | NW outer corner area                      |
| 2     | ( 0.0, 2.5,  7.4)     | N wall mid                                |
| 3     | ( 9.4, 2.5,  6.4)     | NE outer corner area                      |
| 4     | (-9.4, 2.5, -6.4)     | SW outer corner area                      |
| 5     | ( 0.0, 2.5, -7.4)     | S wall mid (note: S wall is also outer)   |
| 6     | ( 9.4, 2.5, -6.4)     | SE outer corner area                      |

**2 RiftLight instances** (place inside the BREACH GAP — the interior 5x3 hollow at the center). The breach center is approximately (0, *, 0). Use:

| Index | Local Pos (x, y, z)   | Logical position                          |
|-------|-----------------------|-------------------------------------------|
| 1     | (-2.5, 1.2, 0.0)      | West edge of interior breach              |
| 2     | ( 2.5, 1.2, 0.0)      | East edge of interior breach              |

Each instance must be a **Prefab Instance** of the source prefab (`PrefabUtility.InstantiatePrefab` in editor), not a fresh GameObject + Light copy. This keeps prefab linkage intact.

Naming: leave default (Unity will name them `TorchLight (1)`, `TorchLight (2)` etc.) — do not rename.

After placement: trigger `RoomShellBuilder.Rebuild()` (right-click → Rebuild on Room_Demo) so the pillar protrusion change from STEP 1 takes effect on the existing room shell.

Save the scene.

---

## STEP 6 — Verify

1. `read_console` MCP → must be clean (no errors, no warnings unrelated to camera lock drift).
2. Take a Game-View screenshot of SampleScene → `Assets/Screenshots/opus_phase4a_glow_v1.png`.
3. Visual check criteria — the screenshot MUST show:
   - 6 visible warm orange light pools along the room perimeter
   - 2 visible cold cyan light pools inside the interior breach
   - Pillars protruding from walls (silhouette breaks visible against floor)
   - At least one cyan-glowing sigil floor tile (since chance is now 0.15 on Damaged lib, on a ~33-tile floor we expect ~5 sigils)
   - Bloom halos visible on every light source
   - Player + 4 mobs lit by at least one torch (no longer near-black)

If any criterion fails, **note it in CODEX_DONE.md ISSUES** with the specific value/file. Do not iterate beyond one rebuild attempt.

---

## STEP 7 — Commit + report

Commit message:
```
[Codex] [S103 PHASE4a GLOW] 8 lights placed + sigil emission + pillar protrusion + bloom boost

- 6 TorchLight + 2 RiftLight instances placed in SampleScene Room_Demo/Lighting
- WallModuleLibrary: pillarInteriorOffset field added (default 0.4 cells)
- RoomShellBuilder: pillars now protrude from N and W walls into room interior
- W walls now spawn pillars matching N wall cadence
- FloorMat_SigilB._EmissionColor = (0, 0.6, 1.0) with _EMISSION keyword on
- sigilFloorChance bumped: Standard 0.12 / Damaged 0.15 / Ritual 0.25 / Rift 0.20
- DungeonVolume bloom intensity 0.85 (was 0.4), threshold 0.7 (was 0.9)
- DungeonVolume vignette intensity 0.45 (was 0.35)
- No new C# classes. No PixelLab. No new textures.

Co-Authored-By: Codex (GPT 5.5) <noreply@antigravity.dev>
```

Write to `CODEX_DONE.md` (append):
- STATUS / COMMIT / FILES_TOUCHED (max 10)
- LIGHTS_PLACED: 8 (6 torch + 2 rift)
- BUILDER_CHANGES: `pillarInteriorOffset` + W-wall pillar spawn
- MATERIAL_CHANGES: FloorMat_SigilB emission
- LIBRARY_CHANGES: 4 sigilFloorChance values
- VOLUME_CHANGES: bloom + vignette
- SCREENSHOT: `Assets/Screenshots/opus_phase4a_glow_v1.png`
- ISSUES (if any)
- NEXT_SIGNAL: `phase4a_glow_complete`

---

## Constraints

- HARD: Camera transform (12, 8, -12) rot (35, 315, 0) ortho 9 — FROZEN. Do not modify Main Camera or CameraLockController.
- HARD: Footprints — do not author any new footprint, do not flatten any existing one to a rectangle.
- HARD: No new C# classes. Only additive field on `WallModuleLibrary` + inline edit in `RoomShellBuilder`.
- HARD: No PixelLab calls. No new textures. No new prefabs.
- HARD: Do not touch `WallLib_ShatteredKeep.asset` (historical baseline).
- HARD: Allowed file writes:
  - `Assets/Scripts/Environment/Modular/RoomShellBuilder.cs`
  - `Assets/Scripts/Environment/Modular/WallModuleLibrary.cs`
  - `Assets/Materials/Environment/FloorMat_SigilB.mat`
  - `Assets/Data/Environment/WallLib_Standard.asset`
  - `Assets/Data/Environment/WallLib_Damaged.asset`
  - `Assets/Data/Environment/WallLib_Ritual.asset`
  - `Assets/Data/Environment/WallLib_Rift.asset`
  - `Assets/Settings/Environment/DungeonVolume_Profile.asset`
  - `Assets/Scenes/SampleScene.unity`
  - `Assets/Screenshots/opus_phase4a_glow_v1.png`
- If post-processing does not visibly bloom even after STEP 4 (URP asset has Post-Processing disabled): document in ISSUES, set `m_RenderPostProcessing: 1` on Main Camera (already set per current scene YAML), confirm URP renderer asset has Post-Processing enabled, **stop**. Do not author new render features.
- If `RoomShellBuilder` errors at Rebuild after STEP 1 edits: revert the W-wall pillar block only, keep the N-wall offset change, document in ISSUES.
- STOP after STEP 7.
