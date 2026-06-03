using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using RIMA.RoomPainter;

namespace RIMA.Tests.EditMode.RoomPainter
{
    /// <summary>
    /// EditMode coverage for the Unified Designer foundation: logical cliff solving, category
    /// routing, JSON round-trip parity (incl. portals), and the surface-agnostic core.
    /// </summary>
    public class UnifiedDesignerTests
    {
        private static RoomData NewRoom()
        {
            var r = ScriptableObject.CreateInstance<RoomData>();
            r.EnsureDefaults();
            return r;
        }

        private static void FillSquareFloor(RoomData room, int size)
        {
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    RoomDataMutator.PutFloorCell(room, "floor", new Vector3Int(x, y, 0), Vector3.zero, 0f, Vector2.one);
        }

        // ── RoomCliffSolver ────────────────────────────────────────────────
        [Test]
        public void CliffSolver_EmptyFloor_ReturnsNoCliffs()
        {
            var cliffs = RoomCliffSolver.Solve(new List<Vector3Int>(), 1);
            Assert.AreEqual(0, cliffs.Count);
        }

        [Test]
        public void CliffSolver_SolidBlock_PlacesEdgeCliffs_NotInterior()
        {
            var room = NewRoom();
            FillSquareFloor(room, 5);

            var cliffs = RoomCliffSolver.SolveFromRoom(room, 1);

            Assert.Greater(cliffs.Count, 0, "a solid island should have a camera-facing cliff edge");
            Assert.Less(cliffs.Count, 25, "interior cells must not all be cliffs");
            Assert.IsFalse(cliffs.Contains(new Vector3Int(2, 2, 0)),
                "the fully-surrounded interior cell must never be a cliff");
        }

        // ── DesignerCategoryMap ────────────────────────────────────────────
        [Test]
        public void CategoryMap_RoutesLayersAndKinds()
        {
            Assert.AreEqual(RoomLayer.Floor, DesignerCategoryMap.LayerFor(DesignerCategory.Floor));
            Assert.AreEqual(RoomLayer.Cliff, DesignerCategoryMap.LayerFor(DesignerCategory.Cliff));
            Assert.AreEqual(RoomLayer.Props, DesignerCategoryMap.LayerFor(DesignerCategory.Object));
            Assert.AreEqual(RoomLayer.Lighting, DesignerCategoryMap.LayerFor(DesignerCategory.Light));

            Assert.IsTrue(DesignerCategoryMap.IsTileCategory(DesignerCategory.Floor));
            Assert.IsTrue(DesignerCategoryMap.IsTileCategory(DesignerCategory.Cliff));
            Assert.IsFalse(DesignerCategoryMap.IsTileCategory(DesignerCategory.Object));
            Assert.IsTrue(DesignerCategoryMap.IsPortalCategory(DesignerCategory.Portal));
        }

        // ── RoomDataMutator.PutCategory ────────────────────────────────────
        [Test]
        public void Mutator_PutCategory_RoutesToCorrectCollection()
        {
            var room = NewRoom();
            var cell = new Vector3Int(1, 1, 0);

            RoomDataMutator.PutCategory(room, DesignerCategory.Floor, "f", cell, Vector3.zero, 0f, Vector2.one);
            Assert.AreEqual(1, room.floorCells.Count);

            RoomDataMutator.PutCategory(room, DesignerCategory.Cliff, "c", new Vector3Int(2, 0, 0), Vector3.zero, 0f, Vector2.one);
            Assert.AreEqual(1, room.cliffCells.Count);

            RoomDataMutator.PutCategory(room, DesignerCategory.Object, "o", new Vector3Int(3, 0, 0), Vector3.zero, 0f, Vector2.one);
            Assert.AreEqual(1, room.propPlacements.Count);
            Assert.AreEqual(RoomLayer.Props, room.propPlacements[0].layer);

            RoomDataMutator.PutCategory(room, DesignerCategory.Light, "l", new Vector3Int(4, 0, 0), Vector3.zero, 0f, Vector2.one);
            Assert.AreEqual(2, room.propPlacements.Count);
            Assert.AreEqual(RoomLayer.Lighting, room.propPlacements[1].layer);

            RoomDataMutator.PutCategory(room, DesignerCategory.Portal, "p", new Vector3Int(5, 0, 0), Vector3.zero, 0f, Vector2.one);
            Assert.AreEqual(1, room.portalPlacements.Count);
        }

        [Test]
        public void Mutator_RemoveCategory_RemovesFromCorrectCollection()
        {
            var room = NewRoom();
            var cell = new Vector3Int(7, 7, 0);
            RoomDataMutator.PutCategory(room, DesignerCategory.Portal, "p", cell, Vector3.zero, 0f, Vector2.one);
            Assert.AreEqual(1, room.portalPlacements.Count);

            RoomDataMutator.RemoveCategory(room, DesignerCategory.Portal, cell);
            Assert.AreEqual(0, room.portalPlacements.Count);
        }

        // ── JSON round-trip parity (portals must survive) ──────────────────
        [Test]
        public void Json_RoundTrip_PreservesPortals()
        {
            var room = NewRoom();
            room.roomId = "test_room";
            RoomDataMutator.PutPortal(room, "portal_combat", new Vector3Int(0, 3, 0),
                Vector3.zero, 0f, Vector2.one, 0, 42, "combat");

            RoomDataDTO dto = RoomDataJson.ToDto(room);
            var clone = NewRoom();
            RoomDataJson.ApplyTo(clone, dto);

            Assert.AreEqual(1, clone.portalPlacements.Count, "portal must survive JSON round-trip");
            Assert.AreEqual(42, clone.portalPlacements[0].targetNodeId);
            Assert.AreEqual("combat", clone.portalPlacements[0].roomTypeId);
        }

        // ── UnifiedDesignerCore (surface-agnostic) ─────────────────────────
        [Test]
        public void Core_PaintAndErase_RouteThroughCategory()
        {
            var core = new UnifiedDesignerCore();
            var room = NewRoom();
            core.SetActiveRoom(room);
            core.Category = DesignerCategory.Floor;
            core.SelectedAssetId = "floor";

            core.Paint(new Vector3Int(0, 0, 0), Vector3.zero);
            Assert.AreEqual(1, room.floorCells.Count);

            core.Erase(new Vector3Int(0, 0, 0));
            Assert.AreEqual(0, room.floorCells.Count);
        }

        [Test]
        public void Core_GenerateCliffsFromFloor_PopulatesCliffCells()
        {
            var core = new UnifiedDesignerCore();
            var room = NewRoom();
            FillSquareFloor(room, 5);
            core.SetActiveRoom(room);
            core.SouthClearCells = 1;

            int expected = RoomCliffSolver.SolveFromRoom(room, 1).Count;
            int generated = core.GenerateCliffsFromFloor(null, "cliff");

            Assert.AreEqual(expected, generated);
            Assert.AreEqual(expected, room.cliffCells.Count);
            Assert.Greater(generated, 0);
        }

        [Test]
        public void Core_ChangedEvent_FiresOnPaint()
        {
            var core = new UnifiedDesignerCore();
            core.SetActiveRoom(NewRoom());
            core.Category = DesignerCategory.Floor;

            int fired = 0;
            core.Changed += () => fired++;
            core.Paint(Vector3Int.zero, Vector3.zero);

            Assert.GreaterOrEqual(fired, 1);
        }
    }
}
