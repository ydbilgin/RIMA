# Camera Lock + Footprint Refactor (No Plain Rectangles)

**ACTIVE RULES:** (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

**CAMERA LOCK:** Do NOT modify Main Camera transform, projection, or ortho size beyond what STEP 1 explicitly requires. Position (12, 8, -12), Rotation (35, 315, 0), Orthographic size 9. After STEP 1, this is FROZEN forever.

**NLM ACCESS:** If you need RIMA design context, query NLM first via:
`uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
Direct-read only: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

ALWAYS WRITE YOUR RESULT SUMMARY TO CODEX_DONE.md AS THE VERY LAST STEP.

---

## Context

User feedback 2026-05-23: (1) "dümdüz odalar istemiyorum" — no plain rectangles, every room must be irregular. (2) "açımız aynı kalacak onu sabitleyelim oyun ekranında" — camera angle locked across all scenes.

This task does TWO independent things in one dispatch:
- **STEP 1-3:** CameraRig_HD2D prefab + CameraLockController.cs + apply to all 3 scenes
- **STEP 4-6:** Refactor 4 plain-rect footprints → irregular variants + create 4 NEW irregular footprints

---

## STEP 0 — Required reads

1. `CLAUDE.md` + `CODEX_DISPATCH.md`
2. `CURRENT_STATUS.md`
3. `MEMORY/feedback_camera_lock_hd2d.md` — camera lock spec (just authored)
4. `Assets/Data/Environment/Footprints/*.asset` — note current 7 ASCII grids (especially the 4 plain rects)
5. `Assets/Scripts/Environment/Modular/RoomFootprint.cs` — ASCII parser convention

---

## STEP 1 — CameraLockController.cs

**New file:** `Assets/Scripts/Camera/CameraLockController.cs`
**Namespace:** `RIMA.Camera`

```csharp
[ExecuteAlways, RequireComponent(typeof(Camera))]
public class CameraLockController : MonoBehaviour
{
    public Vector3 lockedPosition  = new Vector3(12f, 8f, -12f);
    public Vector3 lockedEuler     = new Vector3(35f, 315f, 0f);
    public float   lockedOrthoSize = 9f;
    public bool    enforceInPlayMode = true;
    public bool    enforceInEditor   = true;
    public bool    logDrift          = true;

    Camera cam;

    void OnEnable()  { cam = GetComponent<Camera>(); Apply(); }

    void LateUpdate()
    {
        bool shouldEnforce = (Application.isPlaying ? enforceInPlayMode : enforceInEditor);
        if (!shouldEnforce || cam == null) return;
        if (transform.position != lockedPosition || transform.eulerAngles != lockedEuler || cam.orthographicSize != lockedOrthoSize)
        {
            if (logDrift) Debug.LogWarning($"[CameraLock] Drift detected on {gameObject.name}, restoring locked values.");
            Apply();
        }
    }

    void Apply()
    {
        transform.position    = lockedPosition;
        transform.eulerAngles = lockedEuler;
        cam.orthographic      = true;
        cam.orthographicSize  = lockedOrthoSize;
    }
}
```

No code comments unless WHY is non-obvious.

---

## STEP 2 — CameraRig_HD2D.prefab

**New file:** `Assets/Prefabs/Camera/CameraRig_HD2D.prefab`

- Root empty GO `CameraRig_HD2D` at (0, 0, 0).
- Child: Camera GO named `MainCamera_HD2D`, tag = `MainCamera`. Attach Camera + AudioListener + URP UniversalAdditionalCameraData + the new `CameraLockController` component.
- Camera component values set to STEP 1 lock values (the controller restores them anyway, but inspector should show correct defaults).

---

## STEP 3 — Apply lock to 3 existing scenes

For each scene (`Assets/Scenes/SampleScene.unity`, `RoomShowcase.unity`, plus any other `*.unity` containing a Main Camera that the project has):

- Open scene
- Locate Main Camera GO (tag `MainCamera`)
- Add `CameraLockController` component (use STEP 1 defaults — no per-scene tweaks)
- Force-set transform + ortho size to lock values
- Save scene

If a scene needed a different framing (e.g., RoomShowcase wide ortho 18 was set to see all rooms): you must reframe by repositioning ROOMS, not the camera. For RoomShowcase: scale the room spacing down or shift rooms closer to origin so they fit within the locked camera's view. Do NOT increase ortho size to compensate.

---

## STEP 4 — Refactor 4 plain-rect footprints

Each of these footprints is currently a perfect rectangle. Rewrite their ASCII to introduce AT LEAST 2 irregularities (notch, jut, offset, asymmetric corner cut). Keep total cell count within ±15% of original.

### 4a. `RF_SmallRect_8x8.asset` → IRREGULAR
Current: 4x4 plain. New (4x4 with NE corner cut + W notch):
```
###.
####
####
.###
```

### 4b. `RF_MedRect_12x12.asset` → IRREGULAR
Current: 6x6 plain. New (6x6 with 2 notches):
```
######
######
####..
######
######
..####
```

### 4c. `RF_WideArena_24x16.asset` → IRREGULAR  
Current: 12x8 plain. New (12x8 with corner cuts + center jut on south):
```
..########..
############
############
############
############
############
############
....####....
```

### 4d. `RF_BigArena_30x22.asset` → IRREGULAR (still big, no longer flat)
Current: 15x11 plain. New (15x11 with NE alcove + SW alcove + S jut):
```
###############
###############
###############
###############
###############
###############
###############
###########....
###############
###############
....###########
```

Verify each new ASCII parses correctly: rows match `heightCells`, columns match `widthCells`, only `#` and `.` characters.

---

## STEP 5 — Create 4 NEW irregular footprints

Path: `Assets/Data/Environment/Footprints/`

### 5a. `RF_Cross_18x18.asset` — 9x9 cells, large cross shape
```
...###...
...###...
...###...
#########
#########
#########
...###...
...###...
...###...
```

### 5b. `RF_Asymmetric_20x14.asset` — 10x7 cells, asymmetric organic shape
```
..########
.#########
##########
##########
##########
.#########
...#######
```

### 5c. `RF_OffsetRect_16x12.asset` — 8x6 cells, two offset rectangles stitched
```
####....
######..
########
########
..######
....####
```

### 5d. `RF_BreachedArena_22x18.asset` — 11x9 cells, large arena with breach gaps in interior
```
###########
###########
####...####
###.....###
###.....###
###.....###
####...####
###########
###########
```

Wait — the inner gaps would create islands. Verify in builder: `RoomShellBuilder` builds walls for any boundary edge, so interior holes create interior walls. This is OK — it produces a room with a central platform around a pit / alcove. If the pit needs to be sealed (no inner walls), update the spec to fill the interior. For Phase 4 visual, INTERIOR walls around a sunken pit reads as "broken floor / fallen section" which fits ChatGPT_ref. Keep as-is unless the builder errors.

Total new footprints: 4. Combined with the 4 refactored, we now have 11 footprints, ZERO plain rectangles.

---

## STEP 6 — Update SampleScene to use BreachedArena and verify

1. Open `SampleScene.unity`
2. Find `Room_Demo` GameObject (RoomShellBuilder)
3. Swap `footprint` field to `RF_BreachedArena_22x18.asset`
4. Call Rebuild()
5. Move Player + 4 mobs to valid floor positions inside the breached arena footprint (avoid interior gap cells)
6. Camera stays locked — do NOT touch.
7. Save scene.

---

## STEP 7 — Verify

1. `read_console` MCP — clean.
2. Screenshot `SampleScene` after camera lock + breached arena rebuild → `Assets/Screenshots/codex_camera_lock_v1.png`
3. Screenshot `RoomShowcase` (4-variant) → `Assets/Screenshots/codex_camera_lock_showcase.png`
4. Visual check:
   - Camera lock: both screenshots show identical FOV/angle, no inspector drift
   - SampleScene: BreachedArena visible with interior alcove/breach effect
   - Showcase: 4 rooms fit within locked camera view (rooms repositioned closer if needed)

---

## STEP 8 — Commit + report

Commit message:
```
[Codex] [S103 CAMERA LOCK + IRREGULAR FOOTPRINTS] CameraLockController + 8 irregular footprints (4 refactored + 4 new)

- CameraLockController.cs enforces pos (12,8,-12) rot (35,315,0) ortho 9 in LateUpdate
- CameraRig_HD2D.prefab self-contained camera + lock
- All 3 scenes (SampleScene, RoomShowcase, others) get CameraLockController
- 4 plain rects refactored to irregular: SmallRect, MedRect, WideArena, BigArena
- 4 new irregular footprints: Cross, Asymmetric, OffsetRect, BreachedArena
- SampleScene Room_Demo now uses RF_BreachedArena_22x18
- ZERO plain rectangle footprints remain — user requirement satisfied

Co-Authored-By: Codex (GPT 5.5) <noreply@antigravity.dev>
```

Write `CODEX_DONE.md`:
- STATUS / COMMIT / FILES_TOUCHED
- IRREGULAR_FOOTPRINTS_TOTAL: 11
- NEW_FOOTPRINTS: 4 paths
- REFACTORED_FOOTPRINTS: 4 paths
- CAMERA_LOCK_SCENES: list
- SCREENSHOTS: 2 paths
- ISSUES
- NEXT_SIGNAL: "camera_lock_irregular_footprints_complete"

---

## Constraints

- HARD: Camera transform values from STEP 1 are FROZEN — never modify in any future task.
- HARD: Plain rectangle footprints FORBIDDEN. Every new footprint must have ≥ 2 irregularities.
- Do NOT touch Phase 3 work (atmosphere, wall libraries) — that task is in flight; this task is independent.
- Do NOT modify Player / Mob_* / Boss_* sprites — only positions inside new footprints.
- Allowed file writes: `Assets/Scripts/Camera/*`, `Assets/Prefabs/Camera/*`, `Assets/Data/Environment/Footprints/*`, scene files in `Assets/Scenes/`, screenshots.
- If `RoomShellBuilder` errors on interior gaps (BreachedArena): document the issue in CODEX_DONE.md ISSUES section and fall back to a footprint that fills interior. Do NOT modify the builder.
- STOP after STEP 8.
