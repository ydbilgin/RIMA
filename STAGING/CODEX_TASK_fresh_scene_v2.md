# Codex Task — Fresh Scene PlayableRoom_v2 (Iso Setup + Painter Ready)

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

UnityMCP REQUIRED — Unity Editor açık + Scene/Game window docked (just verified).

---

## Bağlam

Önceki `IsoShowcaseRoom_S95.unity` Unity Editor crash'ine sebep oldu (native access violation, scene corruption). Quarantine edildi → `IsoShowcaseRoom_S95.unity.corrupted_2026_05_21`. **O dosyaya DOKUNMA, açma, reference verme.**

Sıfırdan yeni scene yarat: `Assets/Scenes/Demo/PlayableRoom_v2.unity`

## Görev — Sıfırdan İso Scene Setup

User Painter ile boyamak istiyor — Scene view iso görünmeli, math doğru çalışmalı.

Memory `project-isometric-floor-pivot-s95` LOCK referans:
- Grid cellLayout = **Isometric Z As Y** (4)
- cellSize = **(1, 0.5, 1)** diamond ratio
- Transparency Sort Axis = (0, 1, 0)

## Workflow

### Step 1 — New Empty Scene
1. UnityMCP `manage_scene` ile yeni empty scene yarat: `Assets/Scenes/Demo/PlayableRoom_v2.unity`
2. Scene aç (load)
3. Default Main Camera + Directional Light EditorScene'inden geliyor — Main Camera tut, Directional Light sil (2D iso için)

### Step 2 — Isometric Grid Hierarchy

```
Grid (root GameObject)
├─ Component: Grid
│   ├─ Cell Layout: Isometric Z As Y (cellLayout=4)
│   ├─ Cell Size: (1, 0.5, 1)
│   └─ Cell Swizzle: XYZ
├─ FloorTilemap (child)
│   ├─ Component: Tilemap
│   └─ Component: TilemapRenderer
│       ├─ Sorting Layer: "Floor" (yarat eğer yoksa)
│       └─ Order in Layer: 0
├─ WallTilemap (child)
│   ├─ Component: Tilemap
│   └─ Component: TilemapRenderer
│       ├─ Sorting Layer: "Walls" (yarat eğer yoksa)
│       └─ Order in Layer: 20
├─ Props_Root (child empty GameObject)
│   └─ (prop GameObject'ler için parent)
└─ Lighting_Root (child empty GameObject)
    └─ (Light2D obje'leri için parent)
```

### Step 3 — Project Settings

UnityMCP `manage_graphics`:
- `Edit > Project Settings > Graphics > Camera Settings > Transparency Sort Mode`: **Custom Axis**
- `Transparency Sort Axis`: **(0, 1, 0)** Y dominant

### Step 4 — Camera Setup

Main Camera:
- Projection: **Orthographic**
- Ortho Size: **5**
- Position: **(0, 0, -10)**
- Rotation: **(0, 0, 0)** — sprite iso angle baked-in, camera rotation YOK
- Background: dark gray (HSV 0,0,0.1)

### Step 5 — Tile Asset Creation (varsa reuse)

3 granite variant için Tile asset yarat:
- `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/granite_clean.asset`
- `granite_worn.asset`
- `granite_chiseled.asset`

Source sprite path:
- `Assets/Art/AssetPacks/Act1_ShatteredKeep/floor_tiles/iso/act1_iso_granite_clean.png`
- `act1_iso_granite_worn.png`
- `act1_iso_granite_chiseled.png`

Eğer Tile asset'ler zaten varsa reuse.

### Step 6 — Floor Paint (16×10 grid)

FloorTilemap'e 16×10 cell paint:
- Random weighted: clean 60%, worn 30%, chiseled 10%
- Cell coord range: (0,0) to (15,9) — Isometric layout otomatik diamond places
- Programmatic paint via `tilemap.SetTile(new Vector3Int(x, y, 0), tile)`

### Step 7 — Wall Paint (Perimeter Rectangle)

WallTilemap'e perimeter rectangle:
- RuleTile asset: `Assets/Art/Tilesets/Act1_WallRuleTile.asset` (mevcut, önceki dispatch yarattı)
- North row (y=9): 16 cell paint (RuleTile auto-pick face_NS + corners)
- South row (y=0): 14 cell paint + 2 cell manual arch_opening center
- East col (x=15): 8 cell (y=1..8)
- West col (x=0): 8 cell (y=1..8)
- 4 corner auto-handled by RuleTile

Arch opening:
- WallTilemap'e separate Tile asset `Act1_WallArchOpening.asset` (mevcut)
- Manual SetTile at south wall center (cells (7,0) ve (8,0))

### Step 8 — Warblade Player

Player GameObject yarat:
- Name: "Player_Warblade"
- Components:
  - SpriteRenderer: sprite = `Assets/Art/Characters/Warblade/Rotations/warblade_south.png`
  - Sorting Layer: "Player" (yarat eğer yoksa)
  - Rigidbody2D: kinematic
  - CircleCollider2D: radius 0.3
  - `Assets/Scripts/Player/PlayerMovementController.cs` (mevcut, önceki dispatch yazdı)
  - `Assets/Scripts/Utilities/IsoSortingOrder.cs` (mevcut, önceki dispatch yazdı)
- Spawn position: **(7.5, 4.5, 0)** (room center)

### Step 9 — Camera Follow (basit)

Main Camera GameObject'ine basit follow script ekle (varsa reuse veya minimal yaz):

```csharp
public class CameraFollow2D : MonoBehaviour {
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10);
    void LateUpdate() {
        if (target != null) transform.position = target.position + offset;
    }
}
```

Eğer mevcut `CameraFollow2D.cs` veya benzer varsa o reuse.
Target: Player_Warblade GameObject.

### Step 10 — Test + Verification

1. Scene save: `PlayableRoom_v2.unity`
2. Scene view'da görsel kontrol:
   - Grid lines diamond pattern (iso) — square DEĞİL
   - Floor 16×10 diamond layout
   - Walls perimeter rectangle iso projection
   - Player center'da Warblade sprite
3. Painter window aç (`RIMA/Tools/World Painter` veya benzer menu):
   - Floor Paint test: 1 cell paint et, doğru hücreye gidiyor mu?
   - Wall Paint test: 1 cell paint et, RuleTile auto-connect mi?
4. Play mode test (5sn):
   - WASD ile Warblade hareket ediyor mu?
   - Camera follow çalışıyor mu?
   - Console error check
5. Screenshots:
   - `STAGING/screenshots/playable_room_v2_scene.png` (Scene view, iso görünür)
   - `STAGING/screenshots/playable_room_v2_game.png` (Game view, player visible)

### Step 11 — Compile Check (HARD RULE 2026-05-21 LOCK)

- `read_console` çağır
- Error/warning varsa **OTOMATIK fix et** + recheck
- Hâlâ hata varsa BLOCKED + raporla

## Output

1. `Assets/Scenes/Demo/PlayableRoom_v2.unity` — yeni scene
2. (gerekirse) 3 Tile asset granite_clean/worn/chiseled
3. `STAGING/screenshots/playable_room_v2_scene.png`
4. `STAGING/screenshots/playable_room_v2_game.png`
5. `CODEX_DONE_*.md`: setup verification + Painter test + player movement OK/FAIL + compile result

## Kısıt

- Corrupted scene `IsoShowcaseRoom_S95.unity.corrupted_2026_05_21` **AÇMA, REFERENCE ETME**
- Existing asset reuse (RuleTile, Player movement script, IsoSortingOrder)
- Yeni asset gen YASAK (sadece scene + tile asset + minor script)
- Cell Layout `Isometric Z As Y` HARD LOCK — değiştirme
- Camera rotation YASAK
- Mob YASAK (sadece environment + player)

## Effort
medium (1-2 saat)
