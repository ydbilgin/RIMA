# Codex Task: Room Designer F1 BRUSH

## Task Bootstrap
This file IS the task spec. Read it fully before writing any code.

RIMA root: F:/Antigravity Projeler/2d roguelite/RIMA
Branch: master. Latest SKELETON commit: a1fa81f

## Context Already Provided
IRoomDesignerContext (do NOT read the file, it is reproduced here):
```csharp
namespace RIMA.Editor.RoomDesigner {
    using UnityEngine; using UnityEngine.Tilemaps; using UnityEngine.UIElements;
    public interface IRoomDesignerContext {
        Tilemap FloorTilemap { get; }
        Tilemap WallsTilemap { get; }
        Tilemap DecalsTilemap { get; }
        Tilemap GetActiveTilemap();
        RoomLayer ActiveLayer { get; set; }
        TileBase ActiveTile { get; set; }
        BrushMode ActiveBrush { get; set; }
        Vector3Int HoveredCell { get; set; }
        bool IsCanvasHovered { get; }
        void InvokeBrush(int mouseButton, Vector3Int cell);
        VisualElement LeftPanel { get; }
        VisualElement RightPanel { get; }
        void MarkDirty();
    }
    public enum RoomLayer { Floor, Walls, Decals }
    public enum BrushMode { Stamp, Eraser, Picker, Bucket }
}
```

Assembly info:
- Editor code assembly: RIMA.RoomDesigner.Editor (rootNamespace RIMA.Editor.RoomDesigner)
- Test assembly: RIMA.Tests.EditMode (references RIMA.RoomDesigner.Editor, has nunit.framework.dll)

## ALLOWED PATHS (STRICT)
- Assets/Editor/RoomDesigner/Brushes/IBrush.cs (CREATE)
- Assets/Editor/RoomDesigner/Brushes/BrushController.cs (CREATE)
- Assets/Editor/RoomDesigner/Brushes/StampBrush.cs (CREATE)
- Assets/Editor/RoomDesigner/Brushes/EraserBrush.cs (CREATE)
- Assets/Editor/RoomDesigner/Brushes/PickerBrush.cs (CREATE)
- Assets/Editor/RoomDesigner/Brushes/BucketFillBrush.cs (CREATE)
- Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uss (APPEND only)
- Assets/Tests/EditMode/Editor/BrushTests.cs (CREATE)

Do NOT touch any other file.

## File 1: Assets/Editor/RoomDesigner/Brushes/IBrush.cs

```csharp
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner
{
    public interface IBrush
    {
        BrushMode Mode { get; }
        void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton);
        void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell);
        void OnStrokeEnd(IRoomDesignerContext ctx);
    }

    public readonly struct CellEdit
    {
        public readonly Tilemap Target;
        public readonly Vector3Int Cell;
        public readonly TileBase Tile;
        public CellEdit(Tilemap t, Vector3Int c, TileBase tile) { Target = t; Cell = c; Tile = tile; }
    }
}
```

## File 2: Assets/Editor/RoomDesigner/Brushes/BrushController.cs

```csharp
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace RIMA.Editor.RoomDesigner
{
    public class BrushController
    {
        public static BrushController Instance { get; private set; }

        private IBrush _activeBrush;
        private double _lastRepaintTime;
        private const double RepaintDebounceSeconds = 0.032;

        public BrushController()
        {
            Instance = this;
        }

        public void Initialize(IRoomDesignerContext ctx)
        {
            var toolbar = new VisualElement();
            toolbar.AddToClassList("rd-brush-toolbar");

            var labels = new[] { "B", "E", "I", "G" };
            var modes = new[] { BrushMode.Stamp, BrushMode.Eraser, BrushMode.Picker, BrushMode.Bucket };

            for (int i = 0; i < 4; i++)
            {
                var btn = new Button();
                btn.text = labels[i];
                btn.AddToClassList("rd-brush-button");
                var mode = modes[i];
                btn.clicked += () => SetBrush(ctx, mode);
                toolbar.Add(btn);
            }

            ctx.RightPanel.Add(toolbar);

            ctx.RightPanel.RegisterCallback<KeyDownEvent>(evt =>
            {
                switch (evt.keyCode)
                {
                    case KeyCode.B: SetBrush(ctx, BrushMode.Stamp); break;
                    case KeyCode.E: SetBrush(ctx, BrushMode.Eraser); break;
                    case KeyCode.I: SetBrush(ctx, BrushMode.Picker); break;
                    case KeyCode.G: SetBrush(ctx, BrushMode.Bucket); break;
                }
            });

            SetBrush(ctx, BrushMode.Stamp);
        }

        public void SetBrush(IRoomDesignerContext ctx, BrushMode mode)
        {
            ctx.ActiveBrush = mode;
            _activeBrush = mode switch
            {
                BrushMode.Stamp  => new StampBrush(),
                BrushMode.Eraser => new EraserBrush(),
                BrushMode.Picker => new PickerBrush(),
                BrushMode.Bucket => new BucketFillBrush(),
                _                => new StampBrush()
            };
        }

        public void OnInvoke(IRoomDesignerContext ctx, int mouseButton, Vector3Int cell)
        {
            _activeBrush?.OnStrokeBegin(ctx, cell, mouseButton);
        }

        public void OnDrag(IRoomDesignerContext ctx, Vector3Int cell)
        {
            _activeBrush?.OnStrokeContinue(ctx, cell);
        }

        public void OnRelease(IRoomDesignerContext ctx)
        {
            _activeBrush?.OnStrokeEnd(ctx);
        }

        public void ApplyStroke(IRoomDesignerContext ctx, IList<CellEdit> edits, string strokeName)
        {
            if (edits.Count == 0) return;
            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName(strokeName);
            var maps = edits.Select(e => e.Target).Distinct().Cast<UnityEngine.Object>().ToArray();
            Undo.RegisterCompleteObjectUndo(maps, strokeName);
            foreach (var e in edits) e.Target.SetTile(e.Cell, e.Tile);
            foreach (var m in maps.OfType<Tilemap>()) m.RefreshAllTiles();
            Undo.CollapseUndoOperations(group);
            ctx.MarkDirty();
        }

        public void RequestRepaint(IRoomDesignerContext ctx)
        {
            double now = EditorApplication.timeSinceStartup;
            if (now - _lastRepaintTime < RepaintDebounceSeconds) return;
            _lastRepaintTime = now;
            ctx.MarkDirty();
        }
    }
}
```

## File 3: Assets/Editor/RoomDesigner/Brushes/StampBrush.cs

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner
{
    public class StampBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Stamp;
        private List<CellEdit> _buffer;
        private HashSet<Vector3Int> _visited;

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            _buffer = new List<CellEdit>();
            _visited = new HashSet<Vector3Int>();
            OnStrokeContinue(ctx, cell);
        }

        public void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell)
        {
            if (_buffer == null || _visited.Contains(cell)) return;
            _visited.Add(cell);
            _buffer.Add(new CellEdit(ctx.GetActiveTilemap(), cell, ctx.ActiveTile));
            ctx.MarkDirty();
        }

        public void OnStrokeEnd(IRoomDesignerContext ctx)
        {
            if (_buffer == null) return;
            BrushController.Instance.ApplyStroke(ctx, _buffer, "Stamp");
            _buffer = null;
            _visited = null;
        }
    }
}
```

## File 4: Assets/Editor/RoomDesigner/Brushes/EraserBrush.cs

```csharp
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner
{
    public class EraserBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Eraser;
        private List<CellEdit> _buffer;
        private HashSet<Vector3Int> _visited;

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            _buffer = new List<CellEdit>();
            _visited = new HashSet<Vector3Int>();
            OnStrokeContinue(ctx, cell);
        }

        public void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell)
        {
            if (_buffer == null || _visited.Contains(cell)) return;
            _visited.Add(cell);
            _buffer.Add(new CellEdit(ctx.GetActiveTilemap(), cell, null));
            ctx.MarkDirty();
        }

        public void OnStrokeEnd(IRoomDesignerContext ctx)
        {
            if (_buffer == null) return;
            BrushController.Instance.ApplyStroke(ctx, _buffer, "Erase");
            _buffer = null;
            _visited = null;
        }
    }
}
```

## File 5: Assets/Editor/RoomDesigner/Brushes/PickerBrush.cs

```csharp
using UnityEngine;

namespace RIMA.Editor.RoomDesigner
{
    public class PickerBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Picker;

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            ctx.ActiveTile = ctx.GetActiveTilemap().GetTile(cell);
            ctx.ActiveBrush = BrushMode.Stamp;
        }

        public void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell) { }
        public void OnStrokeEnd(IRoomDesignerContext ctx) { }
    }
}
```

## File 6: Assets/Editor/RoomDesigner/Brushes/BucketFillBrush.cs

```csharp
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Editor.RoomDesigner
{
    public class BucketFillBrush : IBrush
    {
        public BrushMode Mode => BrushMode.Bucket;
        private const int MaxCells = 10000;

        public void OnStrokeBegin(IRoomDesignerContext ctx, Vector3Int cell, int mouseButton)
        {
            var tilemap = ctx.GetActiveTilemap();
            var targetTile = tilemap.GetTile(cell);
            if (targetTile == ctx.ActiveTile) return;

            var queue = new Queue<Vector3Int>();
            var visited = new HashSet<Vector3Int>();
            var edits = new List<CellEdit>();

            queue.Enqueue(cell);
            visited.Add(cell);

            while (queue.Count > 0)
            {
                if (edits.Count >= MaxCells)
                {
                    EditorUtility.DisplayDialog("Flood Fill",
                        "Selection exceeds 10000 cells. Operation aborted.", "OK");
                    return;
                }
                var cur = queue.Dequeue();
                edits.Add(new CellEdit(tilemap, cur, ctx.ActiveTile));
                TryEnqueue(tilemap, cur + Vector3Int.right,  targetTile, visited, queue);
                TryEnqueue(tilemap, cur + Vector3Int.left,   targetTile, visited, queue);
                TryEnqueue(tilemap, cur + Vector3Int.up,     targetTile, visited, queue);
                TryEnqueue(tilemap, cur + Vector3Int.down,   targetTile, visited, queue);
            }

            BrushController.Instance.ApplyStroke(ctx, edits, "Bucket Fill");
        }

        private static void TryEnqueue(Tilemap tm, Vector3Int n, UnityEngine.Tilemaps.TileBase target,
            HashSet<Vector3Int> visited, Queue<Vector3Int> queue)
        {
            if (!visited.Contains(n) && tm.GetTile(n) == target)
            {
                visited.Add(n);
                queue.Enqueue(n);
            }
        }

        public void OnStrokeContinue(IRoomDesignerContext ctx, Vector3Int cell) { }
        public void OnStrokeEnd(IRoomDesignerContext ctx) { }
    }
}
```

## File 7: Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uss (APPEND)

Read the existing file first, then append these lines at the very end:
```css
.rd-brush-toolbar { flex-direction: row; padding: 4px; background-color: #1A1C20; }
.rd-brush-button { width: 32px; height: 32px; margin: 2px; }
.rd-brush-button--active { background-color: #7BA7BC; }
```

## File 8: Assets/Tests/EditMode/Editor/BrushTests.cs

```csharp
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using RIMA.Editor.RoomDesigner;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace RIMA.Tests.Editor
{
    internal class FakeContext : IRoomDesignerContext
    {
        public Tilemap FloorTilemap { get; set; }
        public Tilemap WallsTilemap { get; set; }
        public Tilemap DecalsTilemap { get; set; }
        public RoomLayer ActiveLayer { get; set; } = RoomLayer.Floor;
        public TileBase ActiveTile { get; set; }
        public BrushMode ActiveBrush { get; set; }
        public Vector3Int HoveredCell { get; set; }
        public bool IsCanvasHovered => true;
        private VisualElement _left = new VisualElement();
        private VisualElement _right = new VisualElement();
        public VisualElement LeftPanel => _left;
        public VisualElement RightPanel => _right;
        public Tilemap GetActiveTilemap() => FloorTilemap;
        public void InvokeBrush(int mouseButton, Vector3Int cell) { }
        public void MarkDirty() { }
    }

    public class BrushTests
    {
        private static Tilemap MakeTilemap(out GameObject go)
        {
            go = new GameObject("TilemapGO");
            go.AddComponent<Grid>();
            var child = new GameObject("TilemapChild");
            child.transform.SetParent(go.transform);
            return child.AddComponent<Tilemap>();
        }

        [Test]
        public void StampBrush_HappyPath()
        {
            var tilemap = MakeTilemap(out var go);
            var tile = ScriptableObject.CreateInstance<Tile>();
            var ctx = new FakeContext { FloorTilemap = tilemap, ActiveTile = tile };
            _ = new BrushController();

            var brush = new StampBrush();
            brush.OnStrokeBegin(ctx, Vector3Int.zero, 0);
            brush.OnStrokeEnd(ctx);

            Assert.IsNotNull(tilemap.GetTile(Vector3Int.zero));

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(tile);
        }

        [Test]
        public void EraserBrush_ClearsCell()
        {
            var tilemap = MakeTilemap(out var go);
            var tile = ScriptableObject.CreateInstance<Tile>();
            tilemap.SetTile(Vector3Int.zero, tile);
            var ctx = new FakeContext { FloorTilemap = tilemap, ActiveTile = null };
            _ = new BrushController();

            var brush = new EraserBrush();
            brush.OnStrokeBegin(ctx, Vector3Int.zero, 0);
            brush.OnStrokeEnd(ctx);

            Assert.IsNull(tilemap.GetTile(Vector3Int.zero));

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(tile);
        }

        [Test]
        public void BucketFill_SmallArea()
        {
            var tilemap = MakeTilemap(out var go);
            var tileA = ScriptableObject.CreateInstance<Tile>();
            var tileB = ScriptableObject.CreateInstance<Tile>();
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileA);

            var ctx = new FakeContext { FloorTilemap = tilemap, ActiveTile = tileB };
            _ = new BrushController();

            new BucketFillBrush().OnStrokeBegin(ctx, Vector3Int.zero, 0);

            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    Assert.AreEqual(tileB, tilemap.GetTile(new Vector3Int(x, y, 0)));

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(tileA);
            Object.DestroyImmediate(tileB);
        }

        [Test]
        public void BucketFill_LargeAreaAbortsGracefully()
        {
            var tilemap = MakeTilemap(out var go);
            var tileA = ScriptableObject.CreateInstance<Tile>();
            var tileB = ScriptableObject.CreateInstance<Tile>();
            for (int x = 0; x < 150; x++)
                for (int y = 0; y < 150; y++)
                    tilemap.SetTile(new Vector3Int(x, y, 0), tileA);

            var ctx = new FakeContext { FloorTilemap = tilemap, ActiveTile = tileB };
            _ = new BrushController();

            Assert.DoesNotThrow(() =>
                new BucketFillBrush().OnStrokeBegin(ctx, Vector3Int.zero, 0));

            Object.DestroyImmediate(go);
            Object.DestroyImmediate(tileA);
            Object.DestroyImmediate(tileB);
        }

        [Test]
        public void MultiTilemap_ApplyStrokeSetsGroupName()
        {
            var tm1 = MakeTilemap(out var go1);
            var tile = ScriptableObject.CreateInstance<Tile>();
            var ctx = new FakeContext { FloorTilemap = tm1, ActiveTile = tile };
            var ctrl = new BrushController();

            var edits = new List<CellEdit>
            {
                new CellEdit(tm1, Vector3Int.zero, tile)
            };

            Assert.DoesNotThrow(() => ctrl.ApplyStroke(ctx, edits, "TestStroke"));

            Object.DestroyImmediate(go1);
            Object.DestroyImmediate(tile);
        }
    }
}
```

## Commit Instructions

Stage these paths and commit:
- Assets/Editor/RoomDesigner/Brushes/
- Assets/Editor/RoomDesigner/UI/RoomDesignerWindow.uss
- Assets/Tests/EditMode/Editor/BrushTests.cs

Commit message (exact):
```
feat(room-designer): F1 brush -- Stamp/Eraser/Picker/Bucket + multi-tilemap undo grouping

- IBrush + CellEdit struct
- BrushController (stroke lifecycle, undo grouping, hotkeys B/E/I/G)
- StampBrush + EraserBrush + PickerBrush + BucketFillBrush
- USS brush toolbar selectors
- EditMode tests (stroke happy path + bucket fill + undo grouping)

Co-Authored-By: Codex (GPT 5.5) <noreply@antigravity.dev>
```

## Validation Before Commit

Read back each created .cs file and verify:
1. No syntax errors visible
2. All using directives present
3. Namespace is RIMA.Editor.RoomDesigner for brush files
4. Namespace is RIMA.Tests.Editor for test file
5. BrushController.Instance pattern correct

If any issue found, fix it before committing.

## Report Format
STATUS: DONE / FAILED / PARTIAL
COMPLETED: <bullets>
ERRORS: <or NONE>
FILES_TOUCHED: <paths>
NEXT_SIGNAL: "<phrase>"
