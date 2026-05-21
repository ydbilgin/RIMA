# CODEX TASK — Build IsoShowcaseRoom_S95 Scene via UnityMCP

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS:
  `uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"`
Direct-read sadece: STAGING/ISO_SHOWCASE_ROOM_COMPOSITION_S95.md (THE SPEC) / kod / .meta / prefab.

## Görev

Build a brand new Unity scene `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` strictly following the Opus composition spec.

**THE SPEC IS AUTHORITATIVE.** Read it first. Then execute §15 build order step-by-step. Do not improvise placements. If any step is ambiguous, BLOCK and flag — do not guess coordinates or prefab variants.

## Read Order (mandatory before any Unity write)

1. **`STAGING/ISO_SHOWCASE_ROOM_COMPOSITION_S95.md`** — full spec (729 lines). Read §0 (coordinate contract), §2 (hierarchy), §14 (saçmalık checks), §15 (build order), §16 (out-of-scope), §17 (done criteria).
2. **`.claude/PROJECT_RULES.md`** — RIMA rules + S59 asset constraints (PPU 64, sortingLayer canonical).
3. **`Assets/Prefabs/Walls/pilot_a/`** — inspect 3 existing wall prefabs to copy sortingLayer/sortingOrder/pivot pattern.
4. **`Assets/Prefabs/Props/ShatteredKeep_PixelLab/statue_00..13.prefab`** + **`mounting_00..14.prefab`** — preview the sprites in each to pick "toppled vs intact" statues and "banner pole / ceiling hook / wall shackle" mountings (spec §6.3 + §7).

## Execution

Use UnityMCP (Unity is open). Operate via:
- `mcp__UnityMCP__manage_scene` — create new scene
- `mcp__UnityMCP__manage_gameobject` — hierarchy + transform + components
- `mcp__UnityMCP__manage_prefabs` — instantiate prefabs
- `mcp__UnityMCP__manage_asset` — create Tile ScriptableObjects (§3.1 only if missing)
- `mcp__UnityMCP__manage_scriptable_object` — Tile SO creation
- `mcp__UnityMCP__execute_code` — for Tilemap fill (faster than 80 individual SetTile calls)
- `mcp__UnityMCP__read_console` — after EACH §15 step

## Build Order (§15 from spec)

Execute steps 1-17 in spec §15. After every step, `read_console` for errors. If error → fix, then continue. Never skip ahead.

**Critical sub-steps:**

### Step 1 — Scene scaffold
- New scene path: `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity`
- Do NOT copy from PathC_BaseTest (might drag legacy refs). Create blank, build hierarchy manually per §2.
- Camera: Orthographic, size=5, position centered approximately at world(0, 0, -10) — adjust after Floor_Tilemap exists so camera centers on cell (5, 3) via CellToWorld + offset.
- Add URP 2D Global Light (§8 global) — color `#1A1A2A`, intensity 0.25.

### Step 2 — Grid + Floor_Tilemap
- Grid component: CellLayout=Isometric, CellSize=(1, 0.5, 1).
- Floor_Tilemap **parent transform localScale = (1, 0.819, 1)** — Karar #148 mandatory.
- TilemapRenderer sortingLayer=`Ground`, mode=`IsoZAsY`.

### Step 3 — Iso Tile SO creation (only if missing)
Check `Assets/Data/Tiles/Act1_ShatteredKeep/iso_v01/`. If folder/assets absent:
- Create folder.
- For each PNG in `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/iso/`:
  - Create `Tile` ScriptableObject `act1_iso_granite_<variant>.asset`
  - Assign the matching sprite.
  - Save in `iso_v01/`.

### Step 4 — Floor fill
Use `execute_code` to run a one-shot C# block:
```csharp
// Pseudocode — Codex writes the real version
var seed = "IsoShowcaseRoom_S95".GetHashCode();
var rng = new System.Random(seed);
var tiles = new[] { tileClean, tileWorn, tileChiseled };
var weights = new[] { 60, 30, 10 };
for (int cx = 0; cx < 10; cx++)
  for (int cy = 0; cy < 8; cy++) {
    int roll = rng.Next(100);
    var t = roll < 60 ? tileClean : roll < 90 ? tileWorn : tileChiseled;
    floorTilemap.SetTile(new Vector3Int(cx, cy, 0), t);
  }
// Manual overrides (§3.2) — 8 cells
floorTilemap.SetTile(new Vector3Int(4,6,0), tileChiseled);
// ... rest of §3.2 table
```

### Step 5-15 — Walls, focal, props, decor, lights
Follow spec §4..§11 exactly. Every wall piece is `Instantiate(prefab) → SetParent(Walls_Root)`.

**For face_NS GameObject (§4.4):**
- Create empty GameObject under Walls_Root.
- Add SpriteRenderer with sprite from `pilot_a_test/pilot_a_frame_0_face_NS.png`.
- SortingLayer=Walls, sortingOrder=20.
- Pivot fix: prefer updating the PNG's TextureImporter to set custom pivot (0.5, 0.0313) — affects the imported Sprite asset.

**For east wall rotation risk (§14 open risk):**
- Try Y=90 rotation on face_EW first.
- After step 4 (east wall placement) → take a quick screenshot via UnityMCP (Game view).
- If east face renders edge-on/garbled → switch to face_NS sprite approach (SpriteRenderer GameObjects, no rotation).
- Document which path was taken in the final report.

**For statue/mounting variant picking (§6.3, §7):**
- For each prefab statue_00..13 you consider: read its Sprite asset's metadata or load it briefly into a temp GameObject and inspect the sprite. Pick by visual match to "toppled warrior" / "intact warrior + pedestal".
- Mounting variants: same approach for "banner pole" / "ceiling hook" / "wall shackle/bracket" / "lantern hook" / "shelf/peg".
- If you cannot determine from sprite alone → pick the lowest index that looks plausible, record your choice in the final report, do not block on this.

### Step 16 — Global light
Already added in Step 1. Verify.

### Step 17 — Final pass
- Save scene.
- `read_console` — must be 0 errors. Warnings document but don't block.
- Take Game-view screenshot:
  - Camera centered on world position of cell (5, 3) — use `Floor_Tilemap.CellToWorld(new Vector3Int(5, 3, 0))`.
  - Orthographic size 5.
  - Save PNG to `STAGING/screenshots/IsoShowcaseRoom_S95_first_build.png`.
- Try `mcp__UnityMCP__execute_code` with `ScreenCapture.CaptureScreenshot(path)` after a single frame render. If UnityMCP has a built-in screenshot tool, prefer that.

### Step 18 — Self-QC
Walk through spec §14 saçmalık checklist. For each item, look at the screenshot and confirm. Record PASS/FAIL per item in the final report.

## Allowed File Writes

- **CREATE:** `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` (+ .meta)
- **CREATE (if missing):** `Assets/Data/Tiles/Act1_ShatteredKeep/iso_v01/*.asset` (3 Tile SOs + metas)
- **CREATE (if pulse desired only):** `Assets/Scripts/Visual/RiftPulse2D.cs` (~20 lines, spec §16 explicit allowance)
- **CREATE:** `STAGING/screenshots/IsoShowcaseRoom_S95_first_build.png`
- **CREATE:** `STAGING/CODEX_DONE_iso_showcase_room_s95.md` (final report)
- **MODIFY (only if needed for pivot):** `Assets/Art/AssetPacks/Act1_ShatteredKeep/walls/pilot_a_test/pilot_a_frame_0_face_NS.png.meta` — custom pivot to (0.5, 0.0313)

## Forbidden

- DO NOT modify `PathC_BaseTest.unity` (live scene).
- DO NOT modify any Pilot A wall prefab itself.
- DO NOT generate any new PixelLab asset.
- DO NOT commit.
- DO NOT improvise spec deviations.

## Final Report Format

`STAGING/CODEX_DONE_iso_showcase_room_s95.md`:

```markdown
# CODEX DONE — Iso Showcase Room Build (S95)

## Build Result
- Scene path: ...
- Save status: ...
- Console: N errors, M warnings (list)

## Step Status (§15 build order)
1. Scene scaffold — PASS/FAIL + notes
2. Grid + Floor_Tilemap — ...
...
17. Final pass — ...

## Decisions Made
- East wall rotation path: rotation Y=90 / face_NS fallback
- Statue picks: S1 = statue_XX, S2 = statue_YY (sprite reads matched)
- Mounting picks: WD1 mounting_AA (banner pole), WD4 mounting_BB (shelf), etc.
- Tile assets: created N new / used existing

## Spec §14 Saçmalık Check (each item PASS/FAIL with evidence)
- Floating decoration: PASS / FAIL
- 3+ statues in row: PASS
- Brazier without light: ...
...

## Screenshot
- Path: STAGING/screenshots/IsoShowcaseRoom_S95_first_build.png
- Camera position: world (X, Y, Z)
- Visual notes: (e.g., "rift cyan light visible center-north, east arch readable")

## Flags / Risks for Orchestrator Review
- [Any deviation, ambiguity, or visual concern]
```

## Effort

high — ~80 GameObjects to instantiate, ~80 floor tiles, 5 lights, 3 tile SOs (maybe), 1 screenshot, ~150 UnityMCP tool calls expected. Allow 25-35 min runtime.

## Verification Before Done

Final report must answer: would a fresh viewer looking at the screenshot understand this is the "Forgotten Rift Antechamber" — combat sub-room with cyan rift focal, ritual altar secondary, toppled statue narrative beat? If the screenshot reads as "random asset dump" → step back, identify which §14 check failed, fix it, re-screenshot.
