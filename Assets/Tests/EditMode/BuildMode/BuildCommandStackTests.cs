#if UNITY_EDITOR
using NUnit.Framework;
using RIMA.UI.BuildMode;

namespace RIMA.Tests.BuildMode
{
    /// <summary>
    /// Pure-logic regression for the shared Build Mode undo/redo stack (consolidation item 5:
    /// "shared undo"). No scene needed — BuildCommandStack / IBuildOp are plain C#.
    /// </summary>
    public class BuildCommandStackTests
    {
        private sealed class FakeOp : IBuildOp
        {
            public int didDo;
            public int didUndo;
            public void Do() => didDo++;
            public void Undo() => didUndo++;
        }

        [Test]
        public void Execute_RunsDo_PushesUndo_ClearsRedo()
        {
            var s = new BuildCommandStack();
            var op = new FakeOp();
            s.Execute(op);

            Assert.AreEqual(1, op.didDo, "Execute must run Do exactly once.");
            Assert.AreEqual(1, s.UndoCount, "Execute must record one undo entry.");
            Assert.AreEqual(0, s.RedoCount, "Execute must leave the redo branch empty.");
            Assert.IsTrue(s.CanUndo);
            Assert.IsFalse(s.CanRedo);
        }

        [Test]
        public void Undo_RunsUndo_MovesToRedo()
        {
            var s = new BuildCommandStack();
            var op = new FakeOp();
            s.Execute(op);

            Assert.IsTrue(s.Undo());
            Assert.AreEqual(1, op.didUndo, "Undo must run the op's Undo.");
            Assert.AreEqual(0, s.UndoCount);
            Assert.AreEqual(1, s.RedoCount);
        }

        [Test]
        public void Redo_ReappliesDo_MovesBackToUndo()
        {
            var s = new BuildCommandStack();
            var op = new FakeOp();
            s.Execute(op);
            s.Undo();

            Assert.IsTrue(s.Redo());
            Assert.AreEqual(2, op.didDo, "Redo must re-run Do.");
            Assert.AreEqual(1, s.UndoCount);
            Assert.AreEqual(0, s.RedoCount);
        }

        [Test]
        public void FreshExecute_AfterUndo_ClearsRedoBranch()
        {
            var s = new BuildCommandStack();
            s.Execute(new FakeOp());
            s.Undo();
            Assert.AreEqual(1, s.RedoCount, "Pre-condition: there is a redo entry to discard.");

            s.Execute(new FakeOp());
            Assert.AreEqual(0, s.RedoCount, "A new Execute must invalidate the redo branch.");
            Assert.AreEqual(1, s.UndoCount);
        }

        [Test]
        public void Undo_OnEmpty_ReturnsFalse()
        {
            var s = new BuildCommandStack();
            Assert.IsFalse(s.Undo());
            Assert.IsFalse(s.Redo());
        }

        [Test]
        public void Clear_EmptiesBothStacks()
        {
            var s = new BuildCommandStack();
            s.Execute(new FakeOp());
            s.Undo();
            s.Clear();
            Assert.AreEqual(0, s.UndoCount);
            Assert.AreEqual(0, s.RedoCount);
        }

        [Test]
        public void LifoInterleave_TwoOps_UndoRevertsMostRecentFirst()
        {
            // Mirrors the prop-then-tile shared-stack interleave (consolidation item 5 last bullet):
            // place op A, then op B; the FIRST undo must revert B (most recent), not A.
            var s = new BuildCommandStack();
            var a = new FakeOp();
            var b = new FakeOp();
            s.Execute(a);
            s.Execute(b);

            s.Undo();
            Assert.AreEqual(1, b.didUndo, "First Undo must revert the most recent op (B) — LIFO.");
            Assert.AreEqual(0, a.didUndo, "First Undo must NOT revert the older op (A).");

            s.Undo();
            Assert.AreEqual(1, a.didUndo, "Second Undo reverts A.");
        }
    }
}
#endif
