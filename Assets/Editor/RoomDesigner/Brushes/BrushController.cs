using System.Collections.Generic;
using System.Linq;
using RIMA.Editor.RoomDesigner;
using RIMA.RoomDesigner.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace RIMA.Editor.RoomDesigner.Brushes
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

            var labels = new[] { "B", "E", "I", "G", "C", "S" };
            var modes = new[] { BrushMode.Stamp, BrushMode.Eraser, BrushMode.Picker, BrushMode.Bucket, BrushMode.Circle, BrushMode.Soft };

            for (int i = 0; i < 6; i++)
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
                    case KeyCode.C: SetBrush(ctx, BrushMode.Circle); break;
                    case KeyCode.S: SetBrush(ctx, BrushMode.Soft); break;
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
                BrushMode.Circle => new CircleBrush(),
                BrushMode.Soft   => new SoftBrush(),
                _                => new StampBrush()
            };
        }

        public void OnInvoke(IRoomDesignerContext ctx, int mouseButton, Vector3Int cell)
        {
            if (ctx.ActiveLayer == RoomLayer.Prop)
            {
                Debug.Log("Room Designer Prop layer uses PropContainer placement, not tile painting.");
                return;
            }

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
            edits = edits.Where(e => e.Target != null).ToList();
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
