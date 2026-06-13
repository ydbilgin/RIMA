#if UNITY_EDITOR
using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Tests.BuildMode
{
    /// <summary>
    /// Row-major grid math the Build Mode tile/walkability brush relies on (consolidation item 5:
    /// "walkability/overlay" + the LocalIndex idx = (ly*width)+lx contract). No scene / AssetDatabase.
    /// </summary>
    public class RoomTemplateGridLogicTests
    {
        private RoomTemplateSO t;

        [SetUp]
        public void SetUp()
        {
            t = ScriptableObject.CreateInstance<RoomTemplateSO>();
            t.roomId = "grid_logic_test";
            t.roomType = RIMA.RoomType.Combat;
            t.bounds = new RectInt(0, 0, 4, 3);  // 4 wide, 3 tall
            t.walkableGrid = new bool[4 * 3];
            t.overlayMask = new int[4 * 3];
        }

        [TearDown]
        public void TearDown()
        {
            if (t != null) Object.DestroyImmediate(t);
        }

        [Test]
        public void Walkable_RowMajor_IndexedYTimesWidthPlusX()
        {
            // Mark cell (x=2, y=1) walkable via the row-major index (y*w+x = 1*4+2 = 6).
            t.walkableGrid[1 * 4 + 2] = true;

            Assert.IsTrue(t.IsWalkable(new Vector2Int(2, 1)), "Edited cell must read back walkable.");
            Assert.IsFalse(t.IsWalkable(new Vector2Int(2, 0)), "A different row at the same x stays void.");
            Assert.IsFalse(t.IsWalkable(new Vector2Int(1, 1)), "A different x in the same row stays void.");
        }

        [Test]
        public void Walkable_OutOfBounds_ReturnsFalse()
        {
            Assert.IsFalse(t.IsWalkable(new Vector2Int(-1, 0)));
            Assert.IsFalse(t.IsWalkable(new Vector2Int(4, 0)), "x == width is out of bounds.");
            Assert.IsFalse(t.IsWalkable(new Vector2Int(0, 3)), "y == height is out of bounds.");
        }

        [Test]
        public void Overlay_RowMajor_ReadsBack()
        {
            // overlayMask: 0 = none, otherwise 1-based tile index.
            t.overlayMask[2 * 4 + 3] = 5; // cell (x=3, y=2)
            Assert.AreEqual(5, t.GetOverlayTileIndex(new Vector2Int(3, 2)));
            Assert.AreEqual(0, t.GetOverlayTileIndex(new Vector2Int(0, 0)), "Unset cell is overlay 0.");
        }

        [Test]
        public void Overlay_OutOfBounds_ReturnsZero()
        {
            Assert.AreEqual(0, t.GetOverlayTileIndex(new Vector2Int(99, 99)));
        }

        [Test]
        public void Walkable_BoundsOffset_RespectsXMinYMin()
        {
            // A non-zero-origin room must still resolve row-major from its bounds origin.
            t.bounds = new RectInt(10, 20, 4, 3);
            t.walkableGrid = new bool[4 * 3];
            t.walkableGrid[1 * 4 + 2] = true; // local (2,1)
            Assert.IsTrue(t.IsWalkable(new Vector2Int(12, 21)), "World cell (xMin+2, yMin+1) is walkable.");
            Assert.IsFalse(t.IsWalkable(new Vector2Int(2, 1)), "Local coords without the origin offset are out of bounds.");
        }
    }
}
#endif
