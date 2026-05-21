# CODEX TASK — Isometric Tilemap Grid Setup S95

ACTIVE RULES: (1) think before coding (2) min code, no speculation (3) surgical — listed files only (4) BLOCKED if unclear.

NLM ACCESS: If you need RIMA design context, query NLM first via:
  uvx --from notebooklm-mcp-cli nlm notebook query 30ddffa5-292f-4248-8e77-68074af901be "<your question>"
Direct-read sadece: CURRENT_STATUS.md / .claude/PROJECT_RULES.md / kod / STAGING / memory files.

## Goal
`PathC_BaseTest.unity` sahnesindeki Floor Grid'i Normal Tilemap'ten **Isometric Z as Y** Tilemap'e çevir. Gerçek PixelLab isometric tile'lar henüz üretilmedi — bu task sadece grid setup + placeholder diamond tile ile visual proof.

## Context
- Mevcut: `Grid` (Normal cellLayout) → `Floor_Tilemap` (square tiles, visible seam sorunu)
- Hedef: `Grid` (IsometricZAsY) → `Floor_Tilemap` (diamond tiles, seam yok)
- PPU: 64. İsometric tile: 64px wide = 1 unit, diamond height 32px = 0.5 unit
- Tile size: 64px. Cell size: (1, 0.5, 1)
- Gerçek PixelLab tile'lar sonraki task'ta import edilecek. Bu task: grid doğru mu çalışıyor?

## Scene
`Assets/Scenes/Demo/PathC_BaseTest.unity`

## Steps

### Step 1 — Scene aç
UnityMCP ile sahneyi aç.

### Step 2 — Grid → Isometric Z as Y
SerializedObject ile Grid component'ini değiştir:
```csharp
var gridGO = GameObject.Find("Grid");
var grid = gridGO.GetComponent<Grid>();
var so = new UnityEditor.SerializedObject(grid);
so.FindProperty("m_CellLayout").intValue = (int)GridLayout.CellLayout.IsometricZAsY;
so.FindProperty("m_CellSize").vector3Value = new Vector3(1f, 0.5f, 1f);
so.ApplyModifiedProperties();
```

### Step 3 — Floor_Tilemap temizle
```csharp
var tilemap = gridGO.GetComponentInChildren<UnityEngine.Tilemaps.Tilemap>();
tilemap.ClearAllTiles();
var renderer = tilemap.GetComponent<UnityEngine.Tilemaps.TilemapRenderer>();
renderer.sortOrder = UnityEngine.Tilemaps.TilemapRenderer.SortOrder.TopLeft;
```

### Step 4 — Placeholder diamond tile oluştur
C# ile programmatik 64×64 diamond PNG oluştur, kaydet:
- Path: `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/placeholder_iso.png`
- Diamond mask: pixel (x,y) inside diamond if `|x-32|/32 + |y-32|/16 <= 1` (64px wide, 32px tall diamond, centered vertically in 64px square)
- Renk: `#3A3D42` (Act 1 granite base)
- Kenar (1px): `#5A5D62` (hafif highlight)
- Sprite pivot: (0.5, 0.25) — diamond'ın alt köşesi
- PPU: 64
- Tile asset: `Assets/Data/Tiles/Act1_ShatteredKeep/isometric_v01/placeholder_iso.asset`

### Step 5 — Test grid paint
8×8 isometric alan boyat (BoundsInt loop, tüm hücrelere placeholder_iso tile):
```csharp
// y: -4 to 3, x: -4 to 3
for (int x = -4; x < 4; x++)
    for (int y = -4; y < 4; y++)
        tilemap.SetTile(new Vector3Int(x, y, 0), placeholderTile);
```

### Step 6 — Camera ayarla
`Main Camera` ortho size: 5.5 (8x8 iso alanı frame'ler).

### Step 7 — Scene kaydet + Screenshot
- `UnityEditor.EditorApplication.SaveScene()`
- Screenshot: `STAGING/codex_iso_setup_v01/iso_grid_test_v01.png`

## Success Criteria
1. Grid.cellLayout = IsometricZAsY (Inspector'da görünür)
2. Screenshot'ta 8×8 diamond grid görünüyor — kare yok, diamond var
3. Tile'lar arasında gap yok (flush kenarlar)
4. 0 console error

## Output
- `STAGING/CODEX_DONE_iso_setup_s95.md` — per-step verdict + 4-gate (grid type / diamond shape / no gap / 0 error)
- `STAGING/codex_iso_setup_v01/iso_grid_test_v01.png`

## Deviations
Herhangi bir adım BLOCKED veya beklenmedik Unity API farklılığı varsa BLOCKED yaz, tahmin etme.
