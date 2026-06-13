#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;

namespace RIMA.UI.BuildMode
{
    /// <summary>
    /// A single reversible Build Mode edit (place / erase). Commands are NOT state snapshots:
    /// each op owns exactly what it needs to re-apply (Do) and to roll back (Undo), so the stack
    /// never serializes the whole room. See design doc Phase 2 (BuildCommandStack: IBuildOp Do/Undo
    /// for place/erase; wire Ctrl+Z / Ctrl+Y).
    /// </summary>
    public interface IBuildOp
    {
        /// <summary>Apply (or re-apply on redo) this edit.</summary>
        void Do();

        /// <summary>Reverse this edit.</summary>
        void Undo();
    }

    /// <summary>
    /// Linear undo/redo command stack for Build Mode placement. Push committed ops via Execute;
    /// Undo pops the undo stack onto the redo stack; Redo does the inverse. Any fresh Execute clears
    /// the redo stack (standard editor semantics — a new edit invalidates the redo branch).
    ///
    /// DisableDomainReload safety: this is a plain object owned by BuildPlacementController and
    /// re-created / cleared on enable+disable, so no statics survive across play sessions.
    /// </summary>
    public sealed class BuildCommandStack
    {
        private readonly List<IBuildOp> undoStack = new List<IBuildOp>();
        private readonly List<IBuildOp> redoStack = new List<IBuildOp>();

        public int UndoCount => undoStack.Count;
        public int RedoCount => redoStack.Count;
        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

        /// <summary>Run the op for the first time and record it for undo. Clears the redo branch.</summary>
        public void Execute(IBuildOp op)
        {
            if (op == null) return;
            op.Do();
            undoStack.Add(op);
            redoStack.Clear();
        }

        /// <summary>Record an op that has ALREADY been applied (caller called Do itself).</summary>
        public void Push(IBuildOp op)
        {
            if (op == null) return;
            undoStack.Add(op);
            redoStack.Clear();
        }

        /// <summary>Undo the most recent op. Returns false when there is nothing to undo.</summary>
        public bool Undo()
        {
            if (undoStack.Count == 0) return false;
            int last = undoStack.Count - 1;
            IBuildOp op = undoStack[last];
            undoStack.RemoveAt(last);
            op.Undo();
            redoStack.Add(op);
            return true;
        }

        /// <summary>Redo the most recently undone op. Returns false when there is nothing to redo.</summary>
        public bool Redo()
        {
            if (redoStack.Count == 0) return false;
            int last = redoStack.Count - 1;
            IBuildOp op = redoStack[last];
            redoStack.RemoveAt(last);
            op.Do();
            undoStack.Add(op);
            return true;
        }

        /// <summary>Drop all history (called when Build Mode disables / domain reload is disabled).</summary>
        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
    }
}
#endif
