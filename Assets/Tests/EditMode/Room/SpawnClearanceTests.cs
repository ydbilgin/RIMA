using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RIMA.Tests.Room
{
    /// <summary>
    /// F2 (2026-06-10 playtest fix) — spawn clearance helpers on RoomRunDirector.
    /// Guards against the "player spawns boxed in at the south cliff edge" soft-lock:
    /// the spawn resolver must prefer cells with a full 3x3 walkable clearance and
    /// only fall back to edge cells as a last resort.
    /// </summary>
    public class SpawnClearanceTests
    {
        private RoomTemplateSO template;

        [TearDown]
        public void TearDown()
        {
            if (template != null)
            {
                Object.DestroyImmediate(template);
            }
        }

        // rows[0] = TOP row (y = height-1) for readability; '1' = walkable.
        private RoomTemplateSO MakeTemplate(string[] rows)
        {
            int height = rows.Length;
            int width = rows[0].Length;
            template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.roomId = "test_spawn_clearance";
            template.bounds = new RectInt(0, 0, width, height);
            template.walkableGrid = new bool[width * height];
            for (int y = 0; y < height; y++)
            {
                string row = rows[height - 1 - y]; // rows[0] is top
                for (int x = 0; x < width; x++)
                {
                    template.walkableGrid[(y * width) + x] = row[x] == '1';
                }
            }

            return template;
        }

        [Test]
        public void HasWalkableClearance_TrueForInteriorCell()
        {
            MakeTemplate(new[]
            {
                "11111",
                "11111",
                "11111",
            });

            Assert.IsTrue(RoomRunDirector.HasWalkableClearance(template, new Vector2Int(2, 1)));
        }

        [Test]
        public void HasWalkableClearance_FalseOnBottomEdge()
        {
            MakeTemplate(new[]
            {
                "11111",
                "11111",
                "11111",
            });

            // y=0 cell: south neighbors are outside bounds → no clearance.
            Assert.IsFalse(RoomRunDirector.HasWalkableClearance(template, new Vector2Int(2, 0)));
        }

        [Test]
        public void HasWalkableClearance_FalseNextToBlockedCell()
        {
            MakeTemplate(new[]
            {
                "11111",
                "11011",
                "11111",
                "11111",
            });

            // (2,2) is blocked → any cell whose 3x3 ring touches it fails.
            Assert.IsFalse(RoomRunDirector.HasWalkableClearance(template, new Vector2Int(2, 1)));
        }

        [Test]
        public void TryFindBottomCenterWalkableCell_PrefersInteriorOverBottomEdge()
        {
            MakeTemplate(new[]
            {
                "11111",
                "11111",
                "11111",
                "11111",
            });

            Assert.IsTrue(RoomRunDirector.TryFindBottomCenterWalkableCell(template, out Vector2Int cell));
            // Old behavior picked y=0 (south cliff edge). New behavior must pick y=1 — the
            // lowest row that still has full 3x3 clearance — at the horizontal center.
            Assert.AreEqual(1, cell.y, "Fallback spawn must avoid the bottom cliff-edge row.");
            Assert.AreEqual(2, cell.x, "Fallback spawn must sit at the horizontal center.");
            Assert.IsTrue(RoomRunDirector.HasWalkableClearance(template, cell));
        }

        [Test]
        public void TryFindBottomCenterWalkableCell_LastResortWhenNoClearanceExists()
        {
            // 1-tile-wide corridor: NO cell has 3x3 clearance → must still return a walkable
            // cell (old lowest-Y behavior) instead of failing — never spawn-less.
            MakeTemplate(new[]
            {
                "00100",
                "00100",
                "00100",
            });

            Assert.IsTrue(RoomRunDirector.TryFindBottomCenterWalkableCell(template, out Vector2Int cell));
            Assert.AreEqual(new Vector2Int(2, 0), cell);
        }

        [Test]
        public void TryFindBottomCenterWalkableCell_FalseWhenNothingWalkable()
        {
            MakeTemplate(new[]
            {
                "000",
                "000",
            });

            Assert.IsFalse(RoomRunDirector.TryFindBottomCenterWalkableCell(template, out _));
        }

        [Test]
        public void TryFindNearestClearanceCell_FindsNearestInteriorCell()
        {
            MakeTemplate(new[]
            {
                "11111",
                "11111",
                "11111",
                "11111",
            });

            // Origin on the bottom edge (the generated templates' typical playerSpawn row).
            Assert.IsTrue(RoomRunDirector.TryFindNearestClearanceCell(template, new Vector2Int(2, 0), out Vector2Int cell));
            Assert.AreEqual(new Vector2Int(2, 1), cell, "Nearest clearance cell to (2,0) must be directly north.");
        }

        [Test]
        public void TryFindNearestClearanceCell_FalseWhenNoClearanceExists()
        {
            MakeTemplate(new[]
            {
                "111",
                "111",
            });

            // 3-wide x 2-tall room: no cell has a full 3x3 ring.
            Assert.IsFalse(RoomRunDirector.TryFindNearestClearanceCell(template, new Vector2Int(1, 0), out _));
        }
    }
}
