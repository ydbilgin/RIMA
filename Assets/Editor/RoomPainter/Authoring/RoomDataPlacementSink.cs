using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    internal sealed class RoomDataPlacementSink
    {
        private readonly HashSet<Vector3Int> _paintedCells = new HashSet<Vector3Int>();
        private bool _isPainting;
        private bool _isWallStroke;
        private int _mouseButton;
        private Vector3Int _strokeStart;
        private Vector3Int _strokeEnd;
        private float _rotationDegrees;

        public bool OnSceneGUI(
            SceneView sceneView,
            RoomDataAuthoringController controller,
            RoomDataComposer composer,
            AssetEntry selectedAsset,
            RoomLayer targetLayer)
        {
            if (controller == null || controller.ActiveRoom == null || composer == null)
            {
                return false;
            }

            if (composer.PreviewRoot == null || composer.PreviewGrid == null)
            {
                composer.Compose(controller.ActiveRoom);
            }

            Event e = Event.current;
            Grid grid = composer.PreviewGrid != null ? composer.PreviewGrid : Object.FindAnyObjectByType<Grid>();
            if (grid == null)
            {
                Handles.Label(Vector3.zero, "Room Painter: no preview grid");
                return true;
            }

            Vector3 mouseWorld = MouseWorld(e);
            Vector3Int cell = grid.WorldToCell(mouseWorld);
            Vector3 snappedWorld = grid.GetCellCenterWorld(cell);

            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            HandleUtility.AddDefaultControl(controlId);

            if (e.type == EventType.Repaint)
            {
                DrawAuthoringGhost(selectedAsset, targetLayer, snappedWorld, cell);
                if (_isWallStroke)
                {
                    DrawWallStroke(grid);
                }
            }

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.R)
            {
                _rotationDegrees = (_rotationDegrees + 90f) % 360f;
                e.Use();
                sceneView.Repaint();
                return true;
            }

            if (!HasPaintableSelection(selectedAsset))
            {
                return true;
            }

            if (e.type == EventType.MouseDown && e.alt && e.button == 0 && targetLayer == RoomLayer.Wall)
            {
                PlaceExactWallCell(controller, composer, selectedAsset, cell);
                e.Use();
                sceneView.Repaint();
                return true;
            }

            if (e.type == EventType.MouseDown && !e.alt && (e.button == 0 || e.button == 1))
            {
                BeginStroke(e.button, cell, targetLayer);
                MutateAtCell(controller, composer, selectedAsset, targetLayer, grid, cell, snappedWorld, e.shift);
                e.Use();
                sceneView.Repaint();
                return true;
            }

            if (e.type == EventType.MouseDrag && _isPainting)
            {
                Vector3Int nextCell = e.shift ? AxisLockedCell(_strokeStart, cell) : cell;
                Vector3 nextWorld = grid.GetCellCenterWorld(nextCell);
                MutateAtCell(controller, composer, selectedAsset, targetLayer, grid, nextCell, nextWorld, e.shift);
                e.Use();
                sceneView.Repaint();
                return true;
            }

            if (e.type == EventType.MouseUp && _isPainting && e.button == _mouseButton)
            {
                CommitStroke(controller, composer, selectedAsset, targetLayer);
                e.Use();
                sceneView.Repaint();
                return true;
            }

            return true;
        }

        private void BeginStroke(int button, Vector3Int cell, RoomLayer targetLayer)
        {
            _isPainting = true;
            _isWallStroke = targetLayer == RoomLayer.Wall && button == 0;
            _mouseButton = button;
            _strokeStart = cell;
            _strokeEnd = cell;
            _paintedCells.Clear();
        }

        private void MutateAtCell(
            RoomDataAuthoringController controller,
            RoomDataComposer composer,
            AssetEntry selectedAsset,
            RoomLayer targetLayer,
            Grid grid,
            Vector3Int cell,
            Vector3 snappedWorld,
            bool axisLocked)
        {
            if (_isWallStroke)
            {
                _strokeEnd = axisLocked ? AxisLockedCell(_strokeStart, cell) : cell;
                return;
            }

            if (_paintedCells.Contains(cell))
            {
                return;
            }

            _paintedCells.Add(cell);

            RoomData room = controller.ActiveRoom;
            Undo.RecordObject(room, _mouseButton == 1 ? "Erase Room Data" : "Paint Room Data");
            if (_mouseButton == 1)
            {
                EraseCell(room, targetLayer, cell);
            }
            else
            {
                PlaceCell(room, selectedAsset, targetLayer, cell, snappedWorld, _rotationDegrees);
            }

            controller.MarkDirty();
            composer.Compose(room);
        }

        private void CommitStroke(
            RoomDataAuthoringController controller,
            RoomDataComposer composer,
            AssetEntry selectedAsset,
            RoomLayer targetLayer)
        {
            if (_isWallStroke && targetLayer == RoomLayer.Wall)
            {
                RoomData room = controller.ActiveRoom;
                Undo.RecordObject(room, "Paint Wall Segment");
                WallPiece piece = BuildWallPiece(selectedAsset);
                RoomDataMutator.AppendWallRun(room, _strokeStart, _strokeEnd, piece.pieceId, piece.footprint);
                controller.MarkDirty();
                composer.Compose(room);
            }

            _isPainting = false;
            _isWallStroke = false;
            _paintedCells.Clear();
        }

        private static void PlaceCell(
            RoomData room,
            AssetEntry selectedAsset,
            RoomLayer targetLayer,
            Vector3Int cell,
            Vector3 snappedWorld,
            float rotation)
        {
            string key = AssetKey(selectedAsset);
            if (targetLayer == RoomLayer.Floor)
            {
                RoomDataMutator.PutFloorCell(room, key, cell, snappedWorld, rotation, Vector2.one);
                return;
            }

            if (targetLayer == RoomLayer.Cliff)
            {
                RoomDataMutator.PutCliffCell(room, key, cell, snappedWorld, rotation, Vector2.one);
                return;
            }

            Vector2 scale = selectedAsset.metadata != null ? selectedAsset.metadata.defaultScale : Vector2.one;
            RoomDataMutator.PutProp(room, key, cell, snappedWorld, rotation, scale, targetLayer);
        }

        private static void ReplaceTile(
            List<RoomData.TileCellRecord> cells,
            string key,
            Vector3Int cell,
            Vector3 snappedWorld,
            float rotation)
        {
            for (int i = cells.Count - 1; i >= 0; i--)
            {
                if (cells[i].cell == cell)
                {
                    cells.RemoveAt(i);
                }
            }

            cells.Add(new RoomData.TileCellRecord
            {
                assetGuidOrName = key,
                cell = cell,
                worldPos = snappedWorld,
                rotation = rotation,
                scale = Vector2.one
            });
        }

        private static void EraseCell(RoomData room, RoomLayer targetLayer, Vector3Int cell)
        {
            if (targetLayer == RoomLayer.Floor)
            {
                RoomDataMutator.RemoveFloorCell(room, cell);
                return;
            }

            if (targetLayer == RoomLayer.Cliff)
            {
                RoomDataMutator.RemoveCliffCell(room, cell);
                return;
            }

            if (targetLayer == RoomLayer.Wall)
            {
                RoomDataMutator.RemoveWallCell(room, cell);
                RemoveWallAtCell(room.wallSegments, cell);
                return;
            }

            RoomDataMutator.RemoveProp(room, cell, targetLayer);
        }

        private static void RemoveTileAtCell(List<RoomData.TileCellRecord> cells, Vector3Int cell)
        {
            for (int i = cells.Count - 1; i >= 0; i--)
            {
                if (cells[i].cell == cell)
                {
                    cells.RemoveAt(i);
                }
            }
        }

        private static void RemovePropAtCell(List<RoomData.PropPlacement> placements, Vector3Int cell, RoomLayer layer)
        {
            for (int i = placements.Count - 1; i >= 0; i--)
            {
                if (placements[i].cell == cell && placements[i].layer == layer)
                {
                    placements.RemoveAt(i);
                }
            }
        }

        private static void RemoveWallAtCell(List<WallSegment> segments, Vector3Int cell)
        {
            for (int i = segments.Count - 1; i >= 0; i--)
            {
                if (LineContainsCell(segments[i].fromCell, segments[i].toCell, cell))
                {
                    segments.RemoveAt(i);
                    return;
                }
            }
        }

        private static bool LineContainsCell(Vector3Int a, Vector3Int b, Vector3Int cell)
        {
            foreach (Vector3Int point in GridLine(a, b))
            {
                if (point == cell)
                {
                    return true;
                }
            }

            return false;
        }

        private static IEnumerable<Vector3Int> GridLine(Vector3Int a, Vector3Int b)
        {
            int x0 = a.x;
            int y0 = a.y;
            int x1 = b.x;
            int y1 = b.y;
            int dx = Mathf.Abs(x1 - x0);
            int dy = Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                yield return new Vector3Int(x0, y0, a.z);
                if (x0 == x1 && y0 == y1)
                {
                    break;
                }

                int e2 = err * 2;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        private void PlaceExactWallCell(
            RoomDataAuthoringController controller,
            RoomDataComposer composer,
            AssetEntry selectedAsset,
            Vector3Int cell)
        {
            if (controller == null || controller.ActiveRoom == null || composer == null || !HasPaintableSelection(selectedAsset))
            {
                return;
            }

            RoomData room = controller.ActiveRoom;
            WallPiece piece = BuildWallPiece(selectedAsset);
            Undo.RecordObject(room, "Paint Exact Wall Cell");
            RoomDataMutator.RemoveWallCell(room, cell);
            room.wallCells.Add(new WallCell
            {
                cell = cell,
                kind = SegmentKind.SolidWall,
                shape = WangShape.Single,
                rotation = _rotationDegrees,
                pieceId = piece.pieceId,
                height = 1f
            });
            controller.MarkDirty();
            composer.Compose(room);
        }

        private WallPiece BuildWallPiece(AssetEntry selectedAsset)
        {
            Sprite sprite = selectedAsset.sprite;
            if (sprite == null && selectedAsset.prefab != null)
            {
                SpriteRenderer renderer = selectedAsset.prefab.GetComponentInChildren<SpriteRenderer>();
                sprite = renderer != null ? renderer.sprite : null;
            }

            string key = AssetKey(selectedAsset);
            return new WallPiece
            {
                prefab = selectedAsset.prefab,
                sprite = sprite,
                straightSprite = sprite,
                footprint = FootprintFromSpriteSize(sprite),
                displayName = selectedAsset.AssetObject != null ? selectedAsset.AssetObject.name : selectedAsset.path,
                pieceId = key
            };
        }

        private static Vector2Int FootprintFromSpriteSize(Sprite sprite)
        {
            if (sprite == null)
            {
                return Vector2Int.one;
            }

            return new Vector2Int(
                Mathf.Max(1, Mathf.CeilToInt(sprite.rect.width / 64f)),
                Mathf.Max(1, Mathf.CeilToInt(sprite.rect.height / 64f)));
        }

        private static string AssetKey(AssetEntry selectedAsset)
        {
            if (!string.IsNullOrEmpty(selectedAsset.path))
            {
                string guid = AssetDatabase.AssetPathToGUID(selectedAsset.path);
                if (!string.IsNullOrEmpty(guid))
                {
                    return guid;
                }
            }

            Object asset = selectedAsset.AssetObject;
            return asset != null ? asset.name : string.Empty;
        }

        private static void DrawAuthoringGhost(
            AssetEntry selectedAsset,
            RoomLayer targetLayer,
            Vector3 snappedWorld,
            Vector3Int cell)
        {
            Color color = targetLayer == RoomLayer.Wall
                ? new Color(1f, 0.72f, 0.18f, 0.8f)
                : new Color(0.25f, 0.9f, 1f, 0.65f);
            Handles.color = color;
            Handles.DrawWireCube(snappedWorld, new Vector3(1f, 1f, 0f));
            Handles.Label(snappedWorld + Vector3.up * 0.6f, targetLayer + " " + cell);

            Sprite sprite = PreviewSprite(selectedAsset);
            if (sprite == null || sprite.texture == null)
            {
                return;
            }

            Vector2 center = HandleUtility.WorldToGUIPoint(snappedWorld);
            Rect drawRect = new Rect(center.x - 24f, center.y - 24f, 48f, 48f);
            Rect textureRect = sprite.textureRect;
            Rect texCoords = new Rect(
                textureRect.x / sprite.texture.width,
                textureRect.y / sprite.texture.height,
                textureRect.width / sprite.texture.width,
                textureRect.height / sprite.texture.height);

            Handles.BeginGUI();
            Color previous = GUI.color;
            GUI.color = color;
            GUI.DrawTextureWithTexCoords(drawRect, sprite.texture, texCoords, true);
            GUI.color = previous;
            Handles.EndGUI();
        }

        private void DrawWallStroke(Grid grid)
        {
            Handles.color = new Color(1f, 0.72f, 0.18f, 1f);
            Handles.DrawAAPolyLine(
                4f,
                grid.GetCellCenterWorld(_strokeStart),
                grid.GetCellCenterWorld(_strokeEnd));
        }

        private static Sprite PreviewSprite(AssetEntry selectedAsset)
        {
            if (selectedAsset.sprite != null)
            {
                return selectedAsset.sprite;
            }

            if (selectedAsset.prefab == null)
            {
                return null;
            }

            SpriteRenderer renderer = selectedAsset.prefab.GetComponentInChildren<SpriteRenderer>();
            return renderer != null ? renderer.sprite : null;
        }

        private static bool HasPaintableSelection(AssetEntry selectedAsset)
        {
            return !string.IsNullOrEmpty(selectedAsset.path)
                && (selectedAsset.sprite != null || selectedAsset.prefab != null);
        }

        private static Vector3 MouseWorld(Event e)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Mathf.Abs(ray.direction.z) > Mathf.Epsilon)
            {
                float t = -ray.origin.z / ray.direction.z;
                return ray.origin + ray.direction * t;
            }

            return ray.origin;
        }

        private static Vector3Int AxisLockedCell(Vector3Int anchor, Vector3Int cell)
        {
            int dx = cell.x - anchor.x;
            int dy = cell.y - anchor.y;
            if (Mathf.Abs(dx) >= Mathf.Abs(dy))
            {
                return new Vector3Int(cell.x, anchor.y, cell.z);
            }

            return new Vector3Int(anchor.x, cell.y, cell.z);
        }
    }
}
