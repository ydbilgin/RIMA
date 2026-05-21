# CODEX TASK — Rebuild IsoShowcaseRoom_S95 (Big v2 — 24×18)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

## Görev

`Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` sahnesini **TAMAMEN SİL/YENİDEN KUR** spec'e göre. Eski 10×8 cell sahne YANLIŞ — user "büyük oda yap, asset'ler üst üste durmasın" feedback'i verdi. Yeni spec: 24×18 cell, 17 wall, 1 floor hazard, 1 patch tipi, west wall 0 (ruin gap), face_NS yok (archived).

**THE SPEC IS AUTHORITATIVE.** Read it first. Then execute §14 build order. Do not improvise.

## Read Order

1. **`STAGING/ISO_SHOWCASE_ROOM_BIG_v2_FINAL.md`** — full spec ~450 satır. §0, §13, §14, §15.
2. **`.claude/PROJECT_RULES.md`** — RIMA rules.
3. **`STAGING/ASSET_2ND_AUDIT_S95.md`** — archived 6 assets (DO NOT reference these in scene).
4. **`Assets/Prefabs/Walls/pilot_a/`** — 3 wall prefabs (face_EW, corner_outer, arch_opening). face_NS prefab YOK.
5. **`Assets/Prefabs/Props/ShatteredKeep_PixelLab/`** — statue_00..13 + mounting_00..14.

## Execution

Use UnityMCP. Operate via:
- `manage_scene` — sahneyi sil + yeniden oluştur
- `manage_gameobject` — hierarchy + transform + components
- `manage_prefabs` — instantiate
- `manage_scriptable_object` — Tile SO creation
- `manage_asset` — Tile + script creation
- `execute_code` — Tilemap fill loop, screenshot, batch instantiation
- `read_console` — after each §14 step

## Critical Steps

### Step 1 — Scene Reset
- Sahne mevcut: `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` — bunu **AÇ**, **TÜM root GameObject'leri SİL**, sonra hierarchy §2'den yeniden kur.
- VEYA: Sahneyi SİL + yeni boş sahne oluştur aynı isimle.
- Hangi yolu seçtiğini raporda belirt.
- Camera: Orthographic, **size=7** (24×18 odayı kapsar), position centered on cell (12,9) via CellToWorld.
- URP 2D Global Light: color `#1A1A2A`, intensity **0.6** (Step 17'ye bırakma, baştan kur).

### Step 4 — Floor Fill (execute_code)
Use UnityMCP `execute_code` for batch. ~432 SetTile call mantıklı tek transaction'da:
```csharp
var seed = "IsoShowcaseRoom_S95".GetHashCode();
var rng = new System.Random(seed);
for (int cx = 0; cx < 24; cx++)
  for (int cy = 0; cy < 18; cy++) {
    int roll = rng.Next(100);
    var t = roll < 60 ? tileClean : roll < 90 ? tileWorn : tileChiseled;
    floorTilemap.SetTile(new Vector3Int(cx, cy, 0), t);
  }
// Manual overrides §3.2 (8 cells)
floorTilemap.SetTile(new Vector3Int(10,15,0), tileChiseled);
floorTilemap.SetTile(new Vector3Int(11,15,0), tileChiseled);
floorTilemap.SetTile(new Vector3Int(5,12,0), tileWorn);
floorTilemap.SetTile(new Vector3Int(7,10,0), tileWorn);
floorTilemap.SetTile(new Vector3Int(2,2,0), tileChiseled);
floorTilemap.SetTile(new Vector3Int(20,9,0), tileChiseled);
floorTilemap.SetTile(new Vector3Int(12,5,0), tileClean);
floorTilemap.SetTile(new Vector3Int(14,7,0), tileClean);
```

### Step 5 — Walls (17 pieces)
- **North (9):** N1..N9 — cy=17 boyunca cx=0,3,6,9,11,14,17,20,23
- **East (6):** E1..E6 — cx=23 boyunca cy=3,6,9,11,13,15. **Y=90 rotation** dene; garbled olursa flipX fallback.
- **South (2):** S1 (3,0) + S2 (20,0)
- **West: 0 piece** — cx=0 column dokunma, ruin gap intentional.

### Step 8 — Statue Visual Verification
- 14 statue prefab'tan **toppled** ve **intact + pedestal** variantları seç.
- Try statue_02, 05, 09 for toppled; 00, 03, 11 for intact.
- Visual inspection: Prefab'ı temp instantiate et, sprite'ı oku, "fallen/on-side" vs "standing + pedestal" karar ver.
- Pick'leri raporda belirt.

### Step 12 — Wall Decoration + Mounting (19 deco)
- Her hanging element (banner/cage/chain/lantern/skeleton/trophy) için **mounting prefab seç** (mounting_00..14'ten).
- Mounting prefab'ı önce wall piece child olarak instantiate et, sonra decoration sprite mounting child.
- Roles: banner pole, ceiling hook, wall shackle, lantern hook, shelf/peg.
- Pick'leri raporda belirt.

### Step 18 — Screenshot
- Camera @ cell (12,9), orthographic size 7
- Save `STAGING/screenshots/IsoShowcaseRoom_S95_v2.png` (1920×1080 ideal, 960×540 OK)
- If UnityMCP `ScreenCapture.CaptureScreenshot` works, use it. Otherwise note fallback.

## Allowed File Writes

- **MODIFY:** `Assets/Scenes/Demo/IsoShowcaseRoom_S95.unity` (overwrite)
- **CREATE (if missing):** `Assets/Data/Tiles/Act1_ShatteredKeep/iso_v01/*.asset` (3 Tile SOs)
- **CREATE (if RiftPulse2D missing):** `Assets/Scripts/Visual/RiftPulse2D.cs` (≤20 lines)
- **CREATE:** `STAGING/screenshots/IsoShowcaseRoom_S95_v2.png`
- **CREATE:** `STAGING/CODEX_DONE_iso_showcase_big_v2.md`

## Forbidden

- ARCHIVED asset reference YOK (face_NS, cave_moss, cracked_rubble, iron_grate_floor, pressure_plate, wooden_ladder)
- DO NOT modify `PathC_BaseTest.unity`.
- DO NOT modify Pilot A wall prefabs.
- DO NOT generate new PixelLab asset.
- DO NOT commit.

## Final Report

`STAGING/CODEX_DONE_iso_showcase_big_v2.md`:
- Scene reset path: full delete + rebuild / hierarchy clear
- Step-by-step §14 PASS/FAIL
- Decisions: east wall rotation path (Y=90 vs flipX), statue picks (S1/S2), mounting picks (WD1..WD19)
- §13 saçmalık checklist PASS/FAIL each
- Screenshot path + camera position
- Console 0 errors verify
- Flags/risks

## Effort

high — ~120 GameObject instantiate, 432 floor tile, 8 light, 3 tile SO, 1 screenshot. 30-40 min.
