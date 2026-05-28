ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — new scene + minimal additions (4) BLOCKED if Player.prefab not playable.

NLM ACCESS: If you need RIMA design context:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<question>"

Amaç: User said clean slate — "tamamen temizleyelim sadece zemin ışıklandırma ve karakterimi koy karakterimi oynatabileyim". NO walls, NO room builder, NO objects. JUST: iso floor tilemap (b340684f) + URP 2D lighting + Player prefab + WASD movement. Karakterin gezebileceği boş bir iso arena. Goal — playable in play mode within 30-45 min.

---

# STREAM M — CLEAN PLAYABLE ISO ARENA

## ⚠️ Phase 0 — Internalize intent (150-250 words at top)

This is NOT a room builder, NOT a chatgpt_ref proof, NOT object placement. This is a CLEAN PLAYABLE TEST SCENE where the user can:
1. See an iso floor that doesn't look stupid (b340684f tiles, properly arranged, no gaps)
2. See lighting (URP 2D Global Light at minimum)
3. See their character (Player.prefab or Warblade.prefab)
4. Press WASD or arrow keys and MOVE the character

NO walls, NO room border, NO sockets, NO room generation logic. The character can walk off the floor — that's fine for now (we'll add boundary later).

Skim these existing parts before starting:
- `Assets/Prefabs/Player.prefab`
- `Assets/Prefabs/Characters/Warblade.prefab`
- `Assets/Scripts/Player/PlayerMovementController.cs` (read first 50 lines — does it handle Vector2 input?)
- `Assets/Scripts/Player/PlayerController.cs` (read first 50 lines)
- 16 tile assets at `Assets/ScriptableObjects/Floor/IsoTiles/tile_0..15.asset`

Write 150-250 words at top of CODEX_DONE describing the existing player movement system + your scene plan.

---

## Phase 1 — New Scene Setup (10-15 min)

### 1A. Create scene
- New scene: `Assets/Scenes/Test/PlayableArena.unity`
- Save immediately (before adding anything)

### 1B. Camera + Project Settings
- Main Camera: orthographic, ortho size 8, position (0, 0, -10), background dark navy `RGB(8, 10, 16)`
- PixelPerfectCamera: PPU 32 (matches Stream J iso tile setup)
- Verify Project Settings:
  - Transparency Sort Mode = Custom Axis (0, 1, 0) (already set in Renderer2D.asset per UNITY_ISO_TILEMAP_BRIEF.md)
  - URP 2D Renderer active (already set)

### 1C. Iso Grid + Tilemap
- New GameObject: `Floor` → add Grid component
  - cellLayout = Isometric
  - cellSize = (1, 0.5, 1)
  - cellSwizzle = XYZ
- Child of Floor: `Tilemap` → add Tilemap + TilemapRenderer components
  - Tilemap.orientation = XY
  - TilemapRenderer.sortingOrder = -10 (well below player)
  - Material = `Sprite-Lit-Default` (URP 2D)

### 1D. Paint floor (22×16 cell rectangle, centered)
- For each `(x, y)` where x in [-11, 10] and y in [-8, 7]:
  - Random tile index from 0..15 (deterministic seed for reproducibility — use `Random.InitState(42)`)
  - `tilemap.SetTile(new Vector3Int(x, y, 0), tileAssets[idx])`
- Use SetTiles batch API per UNITY_ISO_TILEMAP_BRIEF.md Q6 (Vector3Int[] + TileBase[] arrays, single SetTiles call, NOT loop)
- Wrap in `AssetDatabase.StartAssetEditing()` / `StopAssetEditing()` per safety rule

Approximate cell count: 22 × 16 = 352 floor tiles.

---

## Phase 2 — URP 2D Lighting (5-10 min)

### 2A. Global Light 2D (mood baseline)
- New GameObject: `Lighting/GlobalLight`
- Add Light 2D component
- Light Type = Global
- Color = `#1A2030` (cool blue-grey)
- Intensity = 0.35
- Target Sorting Layers = Default (or whatever floor + player use)

### 2B. Point Lights (atmosphere, 3-4 around the arena)
- 4 Point Light 2D children of `Lighting`:
  - `TorchLight_NW` at world (-5, 4, 0) — warm orange `#FFA040` intensity 0.9 range 4
  - `TorchLight_NE` at world (5, 4, 0) — same warm
  - `CrystalLight_SW` at world (-5, -4, 0) — cyan `#00FFCC` intensity 0.7 range 3
  - `CrystalLight_SE` at world (5, -4, 0) — cyan
- Falloff = soft

These positions are in orthogonal world coords — they'll appear at corresponding iso positions due to iso projection. Adjust if they look off-frame.

---

## Phase 3 — Player Setup (5-10 min)

### 3A. Read existing player components
Check `Assets/Prefabs/Player.prefab` and `Assets/Prefabs/Characters/Warblade.prefab`:
- Does the prefab have a Rigidbody2D? Collider2D? SpriteRenderer?
- Which prefab has the working movement controller attached?

Pick the prefab that has working WASD movement bound. If neither has clean movement, use Player.prefab.

### 3B. Instantiate in scene
- Position: `(0, 0, 0)` (center of arena)
- Make sure SpriteRenderer's material is `Sprite-Lit-Default` (so 2D lights affect it)
- Sorting layer matches what TilemapRenderer uses (Default)
- Sorting order = 0 (well above floor's -10)

### 3C. Verify movement script handles WASD/arrow keys
- Open `PlayerMovementController.cs` or `PlayerController.cs` — confirm it reads Input.GetAxis or new Input System
- If using new Input System, ensure InputAction asset is set in prefab
- If old Input Manager, ensure axes "Horizontal" / "Vertical" exist in Project Settings

### 3D. Camera follow (optional but recommended)
- Simple: Camera at fixed (0, 0, -10), arena small enough to fit in viewport
- Better: Add a CameraFollow script to camera targeting Player transform — smooth follow
- If a camera follow script already exists in `Assets/Scripts/Camera/`, use it. Else write a 20-line `CameraFollow.cs` (lerp position toward target).

---

## Phase 4 — Test (5 min)

### 4A. Editor play mode
- Save scene
- Press Play
- Verify:
  - Floor renders (iso diamond pattern, dark granite tiles visible)
  - 4 lights cast colored cones on nearby tiles
  - Player appears at center
  - WASD moves player
  - Player Y-sorts correctly (visually sits on floor, not floating)

### 4B. Screenshot
- Take SceneView screenshot in play mode mid-movement: `STAGING/s106_overnight/playable_arena_v1.png`

---

## Phase 5 — Report (5 min)

Write `CODEX_DONE_<profile>.md`:

```
# STREAM M - PLAYABLE ARENA - <profile> - 2026-05-25 <time>

## STATUS: DONE | PARTIAL | FAILED

## Phase 0 — Intent + existing player system summary (150-250 words)

## Phase 1 — Scene + tilemap
- Scene path: Assets/Scenes/Test/PlayableArena.unity
- Floor tiles painted: N
- Grid mode: Isometric (1, 0.5, 1)
- TilemapRenderer sortingOrder: -10

## Phase 2 — Lighting
- Global Light 2D: y intensity 0.35
- Point Lights: 4 (2 warm + 2 cyan)
- All lit material confirmed

## Phase 3 — Player
- Prefab used: Player.prefab OR Warblade.prefab
- Movement script: PlayerMovementController.cs OR PlayerController.cs
- WASD wired: y/n (which input system)
- SpriteRenderer lit material: y
- Sorting order: 0

## Phase 4 — Play mode test
- Play mode enters cleanly: y/n
- Floor visible: y/n
- Lights visible: y/n
- Player visible at center: y/n
- WASD moves player: y/n
- Y-sort looks correct: y/n
- Screenshot path: STAGING/s106_overnight/playable_arena_v1.png

## Compile check
- Unity console errors: 0
- Warnings: 0

## Time: N min
```

---

## Safety constraints (HARD)
- ❌ DO NOT touch existing wall/painter system (Stream J output stays intact)
- ❌ DO NOT modify Player.prefab or Warblade.prefab — instantiate as-is
- ❌ DO NOT modify movement scripts — verify existing behavior, write new if absolutely needed
- ❌ NO walls, NO room generator, NO sockets, NO objects beyond Player
- ✅ AssetDatabase batch wrap for SetTiles
- ✅ Single Refresh at end
- ✅ Single new scene file, single new screenshot, optional single new CameraFollow.cs

## Estimated total: 30-45 min

If Player.prefab won't move (broken setup), STOP with PARTIAL and tell orchestrator the specific blocker — DO NOT spend time fixing the player system.
