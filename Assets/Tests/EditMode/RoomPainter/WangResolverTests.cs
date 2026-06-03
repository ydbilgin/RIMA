using System.Collections.Generic;
using NUnit.Framework;
using RIMA.RoomPainter;
using UnityEngine;

namespace RIMA.Tests.RoomPainter
{
    public sealed class WangResolverTests
    {
        // CCW table (matches Unity Quaternion.Euler(0,0,+theta) render). Bits: 1=N,2=E,4=S,8=W.
        [TestCase(0, WangShape.Single, 0f)]
        [TestCase(1, WangShape.End, 180f)]      // N
        [TestCase(2, WangShape.End, 90f)]       // E
        [TestCase(3, WangShape.Corner, 90f)]    // N+E
        [TestCase(4, WangShape.End, 0f)]        // S
        [TestCase(5, WangShape.Straight, 90f)]  // N+S
        [TestCase(6, WangShape.Corner, 0f)]     // E+S
        [TestCase(7, WangShape.T, 90f)]         // N+E+S, open W
        [TestCase(8, WangShape.End, 270f)]      // W
        [TestCase(9, WangShape.Corner, 180f)]   // N+W
        [TestCase(10, WangShape.Straight, 0f)]  // E+W
        [TestCase(11, WangShape.T, 180f)]       // N+E+W, open S
        [TestCase(12, WangShape.Corner, 270f)]  // S+W
        [TestCase(13, WangShape.T, 270f)]       // N+S+W, open E
        [TestCase(14, WangShape.T, 0f)]         // E+S+W, open N
        [TestCase(15, WangShape.Cross, 0f)]
        public void Resolve4_MapsEveryMaskToExpectedShapeAndRotation(int mask, WangShape shape, float rotation)
        {
            Vector3Int origin = Vector3Int.zero;
            HashSet<Vector3Int> occupied = new HashSet<Vector3Int>();
            if ((mask & 1) != 0) occupied.Add(origin + Vector3Int.up);
            if ((mask & 2) != 0) occupied.Add(origin + Vector3Int.right);
            if ((mask & 4) != 0) occupied.Add(origin + Vector3Int.down);
            if ((mask & 8) != 0) occupied.Add(origin + Vector3Int.left);

            WangResult result = WangResolver.Resolve4(origin, occupied.Contains);

            Assert.AreEqual(mask, result.neighborMask);
            Assert.AreEqual(shape, result.shape);
            Assert.AreEqual(rotation, result.rotationDegrees);
        }

        [Test]
        public void ReorientWallCells_UpdatesNeighborToTWhenNewRunMeetsStraight()
        {
            RoomData room = ScriptableObject.CreateInstance<RoomData>();
            room.roomId = "wang_rebuild_test";
            room.EnsureDefaults();

            RoomDataMutator.AppendWallRun(
                room,
                new Vector3Int(-1, 0, 0),
                new Vector3Int(1, 0, 0),
                "test_wall",
                Vector2Int.one);

            RoomDataMutator.AppendWallRun(
                room,
                new Vector3Int(0, -1, 0),
                new Vector3Int(0, -1, 0),
                "test_wall",
                Vector2Int.one);

            WallCell center = FindWallCell(room, Vector3Int.zero);
            Assert.AreEqual(WangShape.T, center.shape);
            Assert.AreEqual(0f, center.rotation);

            Object.DestroyImmediate(room);
        }

        [Test]
        public void RoomDataJson_RoundTripsCellListsOnly()
        {
            RoomData source = ScriptableObject.CreateInstance<RoomData>();
            source.roomId = "json_roundtrip_test";
            source.displayName = "JSON Roundtrip Test";
            source.EnsureDefaults();

            RoomDataMutator.PutFloorCell(source, "floor_guid", Vector3Int.zero, Vector3.zero, 0f, Vector2.one);
            RoomDataMutator.PutCliffCell(source, "cliff_guid", Vector3Int.right, Vector3.right, 90f, Vector2.one);
            RoomDataMutator.PutProp(source, "prop_guid", Vector3Int.up, Vector3.up, 180f, Vector2.one, RoomLayer.Props);
            RoomDataMutator.AppendWallRun(source, Vector3Int.left, Vector3Int.left, "wall_guid", Vector2Int.one);

            RoomDataDTO dto = RoomDataJson.ToDto(source);
            RoomData copy = ScriptableObject.CreateInstance<RoomData>();
            RoomDataJson.ApplyTo(copy, dto);

            Assert.AreEqual(source.roomId, copy.roomId);
            Assert.AreEqual(1, copy.floorCells.Count);
            Assert.AreEqual(1, copy.cliffCells.Count);
            Assert.AreEqual(1, copy.propPlacements.Count);
            Assert.AreEqual(1, copy.wallCells.Count);
            Assert.AreEqual("wall_guid", copy.wallCells[0].pieceId);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(copy);
        }

        [Test]
        public void RoomDataJson_ToDto_MigratesLegacyWallSegmentsToCells()
        {
            // A legacy room that only has wallSegments (never migrated to wallCells)
            // must NOT lose its walls when serialized: ToDto folds segments->cells
            // before the DTO (which omits wallSegments) drops them.
            RoomData source = ScriptableObject.CreateInstance<RoomData>();
            source.roomId = "segment_migration_test";
            source.EnsureDefaults();
            source.wallSegments.Add(new WallSegment
            {
                kind = SegmentKind.SolidWall,
                fromCell = new Vector3Int(0, 0, 0),
                toCell = new Vector3Int(2, 0, 0),
                piece = new WallPiece { pieceId = "legacy_wall", footprint = Vector2Int.one },
                height = 1f
            });
            Assert.AreEqual(0, source.wallCells.Count, "precondition: no cells yet");

            RoomDataDTO dto = RoomDataJson.ToDto(source);
            RoomData copy = ScriptableObject.CreateInstance<RoomData>();
            RoomDataJson.ApplyTo(copy, dto);

            Assert.AreEqual(3, copy.wallCells.Count, "3-cell run survives round-trip");
            Assert.AreEqual("legacy_wall", copy.wallCells[0].pieceId);

            Object.DestroyImmediate(source);
            Object.DestroyImmediate(copy);
        }

        [Test]
        public void MigrateSegmentsToCells_ClearsSegments_SoErasedWallsStayErased()
        {
            // Regression guard: ToDto migrates on every write, so if wallSegments were not
            // cleared after migration the stale segments would re-apply and resurrect a wall
            // the user erased in F2. Migrate must clear segments and leave cells authoritative.
            RoomData room = ScriptableObject.CreateInstance<RoomData>();
            room.roomId = "segment_resurrection_test";
            room.EnsureDefaults();
            room.wallSegments.Add(new WallSegment
            {
                kind = SegmentKind.SolidWall,
                fromCell = new Vector3Int(0, 0, 0),
                toCell = new Vector3Int(3, 0, 0),
                piece = new WallPiece { pieceId = "legacy_wall", footprint = Vector2Int.one },
                height = 1f
            });

            RoomDataJson.ToDto(room); // first write migrates + must clear segments
            Assert.AreEqual(4, room.wallCells.Count, "4-cell run migrated");
            Assert.AreEqual(0, room.wallSegments.Count, "segments cleared after migrate");

            RoomDataMutator.RemoveWallCell(room, new Vector3Int(1, 0, 0));
            Assert.AreEqual(3, room.wallCells.Count, "one cell erased");

            RoomDataJson.ToDto(room); // second write must NOT resurrect the erased cell
            Assert.AreEqual(3, room.wallCells.Count, "erased wall stays erased on re-save");
            Assert.IsFalse(
                room.wallCells.Exists(c => c.cell == new Vector3Int(1, 0, 0)),
                "stale segment must not resurrect the erased cell");

            Object.DestroyImmediate(room);
        }

        private static WallCell FindWallCell(RoomData room, Vector3Int cell)
        {
            for (int i = 0; i < room.wallCells.Count; i++)
            {
                if (room.wallCells[i].cell == cell)
                {
                    return room.wallCells[i];
                }
            }

            Assert.Fail("Missing wall cell " + cell);
            return default;
        }
    }
}
