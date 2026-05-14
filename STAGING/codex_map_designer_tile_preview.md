# Codex Task: Map Designer — Wang Tile Preview Panel Enhancement

## Context

`Assets/Editor/RimaMapDesignerWindow.cs` is an EditorWindow for RIMA's map system. It has a right panel (DrawRightPanel method) with procedural generation tools. We need to ADD a tile preview section that shows all 16 Wang tiles with semantic names and their sprite previews, so the user knows what each tile combination looks like.

## What to Add

In `DrawRightPanel()`, ABOVE the existing procedural generation foldout, add a **Wang Tile Preview** section:

### Wang Tile Corner Names (key 0..15, key = NW<<3|NE<<2|SW<<1|SE)

```csharp
private static readonly string[] CornerKeyNames = {
    "All Floor",        // 0: 0000
    "SE Corner",        // 1: 0001
    "SW Corner",        // 2: 0010
    "S Edge",           // 3: 0011
    "NE Corner",        // 4: 0100
    "E Edge",           // 5: 0101
    "NE↔SW Diag",       // 6: 0110
    "No NW",            // 7: 0111
    "NW Corner",        // 8: 1000
    "NW↔SE Diag",       // 9: 1001
    "W Edge",           // 10: 1010
    "No NE",            // 11: 1011
    "N Edge",           // 12: 1100
    "No SW",            // 13: 1101
    "No SE",            // 14: 1110
    "All Wall",         // 15: 1111
};

// Maps corner key → sprite asset index (CornerWangTileSetSO.tiles[index])
private static readonly int[] KeyToIndex = { 6, 7, 10, 9, 2, 11, 4, 15, 5, 14, 1, 8, 3, 0, 13, 12 };
```

### Tile Preview Panel

Add this as a FOLDOUT in DrawRightPanel (title: "Wang Tile Preview"):

```
[Active layer info: name + lower/upper labels]
4×4 grid of 16 tiles:
  - Each cell: 40×40px sprite preview (use AssetPreview.GetAssetPreview or EditorGUIUtility.ObjectContent)
  - Below sprite: corner name label (small font, centered, 2-line)
  - Highlight: currently selected terrain (0=floor, 1=wall) — floor tiles (key<8-ish) in blue tint, wall tiles in brown tint
  - Tooltip: full corner description on hover
```

### Implementation

Add a new `[SerializeField] private bool tilePreviewFoldout = true;` field.

Add a new method `DrawTilePreviewPanel(MapLayer activeLayer)`:

```csharp
private void DrawTilePreviewPanel(MapLayer activeLayer)
{
    tilePreviewFoldout = EditorGUILayout.Foldout(tilePreviewFoldout, "Wang Tile Preview", true);
    if (!tilePreviewFoldout) return;

    if (activeLayer == null || activeLayer.tileSet == null)
    {
        EditorGUILayout.HelpBox("Assign a CornerWangTileSetSO to the active layer to see previews.", MessageType.Info);
        return;
    }

    var ts = activeLayer.tileSet;
    string lowerLabel = "lower (floor)";
    string upperLabel = "upper (wall)";
    EditorGUILayout.LabelField($"Lower: {lowerLabel}  |  Upper: {upperLabel}", EditorStyles.miniLabel);

    float previewSize = 40f;
    float labelHeight = 28f;
    float cellW = previewSize + 4f;
    float cellH = previewSize + labelHeight;
    int cols = 4;

    for (int key = 0; key < 16; key++)
    {
        int col = key % cols;
        if (col == 0) EditorGUILayout.BeginHorizontal();

        int spriteIdx = KeyToIndex[key];
        TileBase tile = (spriteIdx >= 0 && spriteIdx < ts.tiles.Length) ? ts.tiles[spriteIdx] : null;

        Texture2D preview = tile != null ? AssetPreview.GetAssetPreview(tile) : null;
        bool isWall = (key == 15);
        bool isFloor = (key == 0);

        GUILayout.BeginVertical(GUILayout.Width(cellW));

        // Tinted background
        Color bg = isWall ? new Color(0.45f, 0.28f, 0.16f, 0.35f)
                 : isFloor ? new Color(0.16f, 0.28f, 0.45f, 0.35f)
                 : new Color(0.25f, 0.25f, 0.25f, 0.2f);
        Rect bgRect = GUILayoutUtility.GetRect(cellW, cellH);
        EditorGUI.DrawRect(bgRect, bg);

        // Sprite preview
        Rect previewRect = new Rect(bgRect.x + 2f, bgRect.y + 2f, previewSize, previewSize);
        if (preview != null)
            GUI.DrawTexture(previewRect, preview, ScaleMode.ScaleToFit);
        else
            EditorGUI.DrawRect(previewRect, new Color(0.15f, 0.15f, 0.15f, 0.8f));

        // Name label
        Rect labelRect = new Rect(bgRect.x, bgRect.y + previewSize + 2f, cellW, labelHeight);
        GUI.Label(labelRect, CornerKeyNames[key], new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleCenter, wordWrap = true });

        GUILayout.EndVertical();

        if (col == cols - 1) EditorGUILayout.EndHorizontal();
    }
    // Close last row if not multiple of 4
    if (16 % cols != 0) EditorGUILayout.EndHorizontal();

    // Repaint while previews load
    if (Event.current.type == EventType.Repaint) Repaint();
}
```

Call `DrawTilePreviewPanel(layers.Count > 0 ? layers[activeLayerIndex] : null)` at the TOP of `DrawRightPanel()`, before the existing tools section.

### Lower/Upper Labels from TileSet

Add public string fields to `CornerWangTileSetSO`:

```csharp
[Header("Terrain Labels")]
public string lowerTerrainLabel = "lower (floor)";
public string upperTerrainLabel = "upper (wall)";
```

Use `ts.lowerTerrainLabel` and `ts.upperTerrainLabel` in the preview panel instead of hardcoded strings.

## Steps

1. Add `lowerTerrainLabel` and `upperTerrainLabel` fields to `Assets/Scripts/Systems/Map/CornerWangTileSetSO.cs`
2. Add `CornerKeyNames[]`, `KeyToIndex[]` static arrays and `DrawTilePreviewPanel()` to `RimaMapDesignerWindow.cs`
3. Add `tilePreviewFoldout` bool field
4. Call `DrawTilePreviewPanel()` at start of `DrawRightPanel()`
5. Update `FloorWall_CornerWangTileSet.asset` label fields: lowerTerrainLabel="Rubble Floor", upperTerrainLabel="Broken Stone Wall"
6. Update `RubblePath_CornerWangTileSet.asset`: lowerTerrainLabel="Rubble Floor", upperTerrainLabel="Worn Stone Path"
7. Check console: 0 errors
8. Open Map Designer, assign FloorWall SO to Base layer, verify tile preview grid shows 16 tiles with names
9. Commit

## Commit message

`[map-designer] Wang tile preview panel — 16 tile grid with corner names, sprite previews, terrain labels`
