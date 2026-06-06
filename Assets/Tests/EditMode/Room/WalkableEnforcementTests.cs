using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RIMA.Environment;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Room
{
    /// <summary>
    /// EditMode tests for walkable enforcement (Task: walkable_enforcement_2026-06-07).
    ///
    ///  A. IsoRoomBuilder builds collision tiles on inner rim cells (donut hole boundary).
    ///  B. WalkabilityMap.InitFromTemplate seeds from RoomTemplateSO, not floor tilemap.
    ///  C. WalkabilityMap.ClampVelocityToWalkable prevents diagonal corner-cutting and
    ///     stops actors that would enter void cells (player, mob, knockback probe).
    ///  D. PropColliderAutoBuilder assigns the "Default" layer to its collider GameObject.
    /// </summary>
    public sealed class WalkableEnforcementTests
    {
        private readonly List<Object> created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = created.Count - 1; i >= 0; i--)
            {
                if (created[i] != null)
                    Object.DestroyImmediate(created[i]);
            }
            created.Clear();
        }

        // ─── A: Inner rim boundary ────────────────────────────────────────────

        /// <summary>
        /// A donut template (5×5 with a 1-cell center hole) must produce a collision tile
        /// on the hole cell itself. Walkable rim cells do NOT get collision tiles.
        /// Also verifies that the outer boundary ring (beyond the walkable area) gets tiles.
        /// </summary>
        [Test]
        public void BuildBoundary_DonutTemplate_HoleCellGetsCollisionTileAndWalkableRimDoesNot()
        {
            // 5×5 donut: walkable everywhere except center cell (2,2).
            //  walkableGrid row-major (y first, then x):
            //    row y=0: all true  → indices 0-4
            //    row y=1: all true  → indices 5-9
            //    row y=2: true true FALSE true true  → indices 10-14 (index 12 = hole)
            //    row y=3: all true  → indices 15-19
            //    row y=4: all true  → indices 20-24
            //
            // BuildBoundary: places a collision tile on cells that are:
            //   (a) NOT in floorCells, AND
            //   (b) have at least one walkable neighbor (8-dir).
            //
            // The hole cell (2,2) satisfies both: it is non-walkable and surrounded by
            // walkable cells (1,2),(3,2),(2,1),(2,3),(1,1),(3,1),(1,3),(3,3).
            // The walkable rim cells (1,2),(3,2) etc. are IN floorCells → no collision tile.
            RoomTemplateSO template = CreateDonutTemplate5x5();
            IsoRoomBuilder builder = CreateRig(out _, out Tilemap collisionTilemap, template);

            builder.Build(template);

            // Hole cell must have a collision tile (non-walkable + has walkable neighbors).
            Assert.IsNotNull(collisionTilemap.GetTile(new Vector3Int(2, 2, 0)),
                "Center hole cell (2,2) must receive a collision tile.");

            // Walkable rim cells adjacent to the hole must NOT have collision tiles.
            Assert.IsNull(collisionTilemap.GetTile(new Vector3Int(1, 2, 0)),
                "Walkable rim cell (1,2) must NOT receive a collision tile.");
            Assert.IsNull(collisionTilemap.GetTile(new Vector3Int(3, 2, 0)),
                "Walkable rim cell (3,2) must NOT receive a collision tile.");
            Assert.IsNull(collisionTilemap.GetTile(new Vector3Int(2, 1, 0)),
                "Walkable rim cell (2,1) must NOT receive a collision tile.");
            Assert.IsNull(collisionTilemap.GetTile(new Vector3Int(2, 3, 0)),
                "Walkable rim cell (2,3) must NOT receive a collision tile.");

            // Outer boundary ring (one cell beyond the walkable area bounds) must have tiles.
            Assert.IsNotNull(collisionTilemap.GetTile(new Vector3Int(-1, 0, 0)),
                "Outer boundary cell (-1,0) must receive a collision tile.");
            Assert.IsNotNull(collisionTilemap.GetTile(new Vector3Int(5, 4, 0)),
                "Outer boundary cell (5,4) must receive a collision tile.");
        }

        /// <summary>
        /// A fully-walkable 3×3 room has no inner holes; no interior cell should receive
        /// a collision tile (only the 1-cell external ring gets tiles).
        /// </summary>
        [Test]
        public void BuildBoundary_SolidRoom_NoInnerCollisionTiles()
        {
            RoomTemplateSO template = CreateSolidTemplate(3, 3);
            IsoRoomBuilder builder = CreateRig(out _, out Tilemap collisionTilemap, template);

            builder.Build(template);

            // Interior walkable cells must NOT have collision tiles.
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    TileBase tile = collisionTilemap.GetTile(new Vector3Int(x, y, 0));
                    Assert.IsNull(tile, $"Walkable cell ({x},{y}) must not have a collision tile.");
                }
            }
        }

        // ─── B: WalkabilityMap.InitFromTemplate ───────────────────────────────

        /// <summary>
        /// After InitFromTemplate with a donut grid, IsWalkableWorld returns false for
        /// the hole cell and true for walkable cells — regardless of tilemap contents.
        /// </summary>
        [Test]
        public void WalkabilityMap_InitFromTemplate_HoleCellReturnsNotWalkable()
        {
            // Build a WalkabilityMap with NO floor tilemap — template should be sole source.
            GameObject mapObject = new GameObject("WalkabilityMap_Test");
            created.Add(mapObject);
            WalkabilityMap walkMap = mapObject.AddComponent<WalkabilityMap>();
            // No floorTilemap assigned.

            RoomTemplateSO template = CreateDonutTemplate5x5();
            walkMap.InitFromTemplate(template);

            // We need WorldToCell conversion. Use a Grid with default cell size 1×1.
            // For a grid with cellSize=1, WorldToCell(x, y, 0) = FloorToInt(worldPos).
            // Template bounds origin is (0,0), so cell (2,2) world center ≈ (2.5, 2.5)
            // but WorldToCell on a default Grid returns floor(pos) = (2,2).
            // We verify via IsWalkable(Vector3Int) directly.
            Assert.IsFalse(walkMap.IsWalkable(new Vector3Int(2, 2, 0)),
                "Hole cell (2,2) must not be walkable after InitFromTemplate.");
            Assert.IsTrue(walkMap.IsWalkable(new Vector3Int(0, 0, 0)),
                "Corner cell (0,0) must be walkable after InitFromTemplate.");
            Assert.IsTrue(walkMap.IsWalkable(new Vector3Int(4, 4, 0)),
                "Corner cell (4,4) must be walkable after InitFromTemplate.");
        }

        /// <summary>
        /// InitFromTemplate(null) clears the template grid and reverts to tilemap-based
        /// lookup (permissive/false without tilemap).
        /// </summary>
        [Test]
        public void WalkabilityMap_InitFromTemplate_NullClearsGrid()
        {
            GameObject mapObject = new GameObject("WalkabilityMap_NullClear_Test");
            created.Add(mapObject);
            WalkabilityMap walkMap = mapObject.AddComponent<WalkabilityMap>();

            RoomTemplateSO template = CreateDonutTemplate5x5();
            walkMap.InitFromTemplate(template);
            // Hole is non-walkable.
            Assert.IsFalse(walkMap.IsWalkable(new Vector3Int(2, 2, 0)));

            walkMap.InitFromTemplate(null);
            // No tilemap assigned → returns false for all cells (no data).
            Assert.IsFalse(walkMap.IsWalkable(new Vector3Int(0, 0, 0)),
                "Without template grid and without tilemap, IsWalkable must return false.");
        }

        // ─── C: ClampVelocityToWalkable ───────────────────────────────────────

        /// <summary>
        /// Moving directly into a void cell should result in zero velocity.
        /// </summary>
        [Test]
        public void ClampVelocity_DirectlyIntoVoid_ZeroVelocity()
        {
            WalkabilityMap walkMap = CreateWalkabilityMapFromTemplate(CreateDonutTemplate5x5());

            // Standing at (1,2) (walkable), moving right (+x) toward hole (2,2).
            // At dt=1 the next pos would be at x=2,y=2 → hole → must zero out.
            // Use a small dt so the velocity resolves to (2,2).
            Vector3 currentPos = new Vector3(1.5f, 2.5f, 0f);
            Vector2 vel = new Vector2(1f, 0f); // moving right, speed 1 unit/s
            float dt = 0.9f; // next pos x=2.4, still maps to cell (2,2) via floorInt

            // IsWalkableWorld uses FloorToInt approximation when no tilemap.
            Vector2 result = WalkabilityMap.ClampVelocityToWalkable(walkMap, currentPos, vel, dt);

            Assert.AreEqual(Vector2.zero, result,
                "Velocity toward void cell must be zeroed out.");
        }

        /// <summary>
        /// Diagonal movement into a void center cell: the diagonal is blocked but both
        /// axis-aligned slides are individually walkable, so X-slide is returned.
        /// Also tests that moving diagonally into a true corner (both slides blocked) → zero.
        /// </summary>
        [Test]
        public void ClampVelocity_DiagonalIntoVoidCenter_SlidesThenZeroWhenBothBlocked()
        {
            // Layout 3×3: all walkable EXCEPT center (1,1).
            //    walkableGrid row-major:
            //    y=0: true true true   → indices 0,1,2
            //    y=1: true FALSE true  → indices 3,4,5  (4 = hole)
            //    y=2: true true true   → indices 6,7,8
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            created.Add(template);
            template.bounds = new RectInt(0, 0, 3, 3);
            template.walkableGrid = new[]
            {
                true, true, true,
                true, false, true,
                true, true, true
            };
            WalkabilityMap walkMap = CreateWalkabilityMapFromTemplate(template);

            // At (0.4, 0.4), moving (+1,+1), dt=0.9 → diagonal target = cell(1,1) = void.
            // X-slide target = cell(1,0) = walkable → returns X-only velocity.
            Vector3 pos = new Vector3(0.4f, 0.4f, 0f);
            Vector2 vel = new Vector2(1f, 1f);
            Vector2 result = WalkabilityMap.ClampVelocityToWalkable(walkMap, pos, vel, 0.9f);
            Assert.AreEqual(new Vector2(1f, 0f), result,
                "Diagonal into center void with walkable X-slide: should return X-only velocity.");

            // Now create a layout where the corner (2,2) is void AND its slides are also void.
            // Layout: only (0,0) and (1,0) walkable — everything else void.
            RoomTemplateSO cornerTemplate = ScriptableObject.CreateInstance<RoomTemplateSO>();
            created.Add(cornerTemplate);
            cornerTemplate.bounds = new RectInt(0, 0, 3, 3);
            cornerTemplate.walkableGrid = new[]
            {
                true,  true,  false,
                false, false, false,
                false, false, false
            };
            WalkabilityMap cornerMap = CreateWalkabilityMapFromTemplate(cornerTemplate);

            // At (0.5, 0.1), moving (+1,+1), dt=0.9.
            // Diagonal → cell(1,1) = void.
            // X-slide → cell(1,0) = walkable → X-slide succeeds.
            Vector3 pos2 = new Vector3(0.5f, 0.1f, 0f);
            Vector2 vel2 = new Vector2(1f, 1f);
            Vector2 result2 = WalkabilityMap.ClampVelocityToWalkable(cornerMap, pos2, vel2, 0.9f);
            Assert.AreEqual(new Vector2(1f, 0f), result2,
                "Diagonal with walkable X-slide should yield X-only velocity.");

            // At (0.5, 0.1), moving (+1,+1), dt=0.9 on a map where X-slide (1,0) is walkable
            // but with a bigger step that hits void on both slides → zero.
            // Use a map with only (0,0) walkable.
            RoomTemplateSO singleCell = ScriptableObject.CreateInstance<RoomTemplateSO>();
            created.Add(singleCell);
            singleCell.bounds = new RectInt(0, 0, 3, 3);
            singleCell.walkableGrid = new[]
            {
                true,  false, false,
                false, false, false,
                false, false, false
            };
            WalkabilityMap singleMap = CreateWalkabilityMapFromTemplate(singleCell);

            // At (0.1, 0.1), moving (+1,+1) dt=0.9 → diagonal = (1,1) = void,
            // X-slide = (1,0) = void, Y-slide = (0,1) = void → zero.
            Vector3 pos3 = new Vector3(0.1f, 0.1f, 0f);
            Vector2 result3 = WalkabilityMap.ClampVelocityToWalkable(singleMap, pos3, new Vector2(1f, 1f), 0.9f);
            Assert.AreEqual(Vector2.zero, result3,
                "Diagonal with both slides void: must return zero (no corner-cutting).");
        }

        /// <summary>
        /// Probe: DoKnockback-style per-frame clamp stops actor at void boundary.
        /// Simulates the KnockbackReceiver per-frame check without running a coroutine.
        /// </summary>
        [Test]
        public void ClampVelocity_KnockbackProbe_StopsAtVoidBoundary()
        {
            WalkabilityMap walkMap = CreateWalkabilityMapFromTemplate(CreateDonutTemplate5x5());

            // Actor at cell (1,2) (walkable), knockback pushes right → hole at (2,2).
            Vector3 pos = new Vector3(1.5f, 2.5f, 0f);
            Vector2 knockVel = new Vector2(10f, 0f);
            float dt = 0.09f; // at this speed, next pos x=2.4 → cell(2,2) → hole

            Vector2 result = WalkabilityMap.ClampVelocityToWalkable(walkMap, pos, knockVel, dt);

            Assert.AreEqual(Vector2.zero, result,
                "Knockback toward hole must be stopped at the boundary.");
        }

        /// <summary>
        /// Walkability allows normal movement when target cell is walkable.
        /// </summary>
        [Test]
        public void ClampVelocity_WalkableTarget_PassesThrough()
        {
            WalkabilityMap walkMap = CreateWalkabilityMapFromTemplate(CreateDonutTemplate5x5());

            // Actor at (0.5, 0.5), moving right toward (1.5, 0.5) → cell(1,0) walkable.
            Vector3 pos = new Vector3(0.5f, 0.5f, 0f);
            Vector2 vel = new Vector2(1f, 0f);
            float dt = 0.99f;

            Vector2 result = WalkabilityMap.ClampVelocityToWalkable(walkMap, pos, vel, dt);

            Assert.AreEqual(vel, result,
                "Movement toward a walkable cell must not be clamped.");
        }

        /// <summary>
        /// Null walkMap is permissive — returns desired velocity unchanged.
        /// </summary>
        [Test]
        public void ClampVelocity_NullWalkMap_Permissive()
        {
            Vector2 vel = new Vector2(3f, 2f);
            Vector2 result = WalkabilityMap.ClampVelocityToWalkable(null, Vector3.zero, vel, 0.016f);
            Assert.AreEqual(vel, result, "Null walkMap must return velocity unchanged.");
        }

        // ─── D: PropColliderAutoBuilder layer ─────────────────────────────────

        /// <summary>
        /// A blocking prop's collider GameObject must be on the "Default" layer so both
        /// player and enemy bodies are blocked by the same physics group as the boundary.
        /// </summary>
        [Test]
        public void PropColliderAutoBuilder_BlockingProp_AssignsDefaultLayer()
        {
            int defaultLayer = LayerMask.NameToLayer("Default");
            if (defaultLayer < 0)
            {
                Assert.Ignore("'Default' layer not found in this project — skipping.");
                return;
            }

            // Create a minimal blocking PropDefinitionSO.
            RIMA.MapDesigner.Props.PropDefinitionSO def = ScriptableObject.CreateInstance<RIMA.MapDesigner.Props.PropDefinitionSO>();
            created.Add(def);
            SetPropDefBlocking(def);

            GameObject propObject = new GameObject("TestProp");
            created.Add(propObject);
            propObject.AddComponent<SpriteRenderer>();

            RIMA.MapDesigner.Props.Runtime.PropColliderAutoBuilder builder =
                propObject.AddComponent<RIMA.MapDesigner.Props.Runtime.PropColliderAutoBuilder>();
            SetPrivate(builder, "propDef", def);
            builder.EnsureCollider();

            Assert.AreEqual(defaultLayer, propObject.layer,
                "Blocking prop collider GameObject must be on the Default layer.");
            Assert.IsNotNull(propObject.GetComponent<BoxCollider2D>(),
                "Blocking prop must have a BoxCollider2D.");
        }

        // ─── Helpers ─────────────────────────────────────────────────────────

        private RoomTemplateSO CreateDonutTemplate5x5()
        {
            // 5×5 with center hole at local (2,2).
            // walkableGrid is row-major: index = (y * width) + x
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            created.Add(template);
            template.roomId = "donut_test_5x5";
            template.roomType = RIMA.RoomType.Combat;
            template.bounds = new RectInt(0, 0, 5, 5);
            template.walkableGrid = new bool[25];
            for (int i = 0; i < 25; i++) template.walkableGrid[i] = true;
            // Hole: local y=2, x=2 → index = (2*5)+2 = 12
            template.walkableGrid[12] = false;
            template.playerSpawn = new PlayerSpawnSocket { position = Vector2Int.zero };
            return template;
        }

        private RoomTemplateSO CreateSolidTemplate(int width, int height)
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            created.Add(template);
            template.roomId = "solid_test";
            template.roomType = RIMA.RoomType.Combat;
            template.bounds = new RectInt(0, 0, width, height);
            template.walkableGrid = new bool[width * height];
            for (int i = 0; i < template.walkableGrid.Length; i++) template.walkableGrid[i] = true;
            template.playerSpawn = new PlayerSpawnSocket { position = Vector2Int.zero };
            return template;
        }

        private IsoRoomBuilder CreateRig(out Grid grid, out Tilemap collisionTilemap, RoomTemplateSO template)
        {
            GameObject gridObject = new GameObject("TestGrid");
            created.Add(gridObject);
            grid = gridObject.AddComponent<Grid>();

            GameObject groundObject = new GameObject("GroundTilemap");
            created.Add(groundObject);
            groundObject.transform.SetParent(gridObject.transform, false);
            groundObject.AddComponent<Tilemap>();
            groundObject.AddComponent<TilemapRenderer>();

            GameObject collisionObject = new GameObject("CollisionTilemap");
            created.Add(collisionObject);
            collisionObject.transform.SetParent(gridObject.transform, false);
            collisionTilemap = collisionObject.AddComponent<Tilemap>();

            GameObject builderObject = new GameObject("IsoRoomBuilder");
            created.Add(builderObject);
            IsoRoomBuilder builder = builderObject.AddComponent<IsoRoomBuilder>();
            SetPrivate(builder, "grid", grid);
            SetPrivate(builder, "groundTilemap", groundObject.GetComponent<Tilemap>());
            SetPrivate(builder, "collisionTilemap", collisionTilemap);
            SetPrivate(builder, "floorTile", CreateTile("floor"));
            SetPrivate(builder, "collisionTile", CreateTile("collision"));
            return builder;
        }

        private WalkabilityMap CreateWalkabilityMapFromTemplate(RoomTemplateSO template)
        {
            GameObject go = new GameObject("WalkMap_Probe");
            created.Add(go);
            WalkabilityMap wm = go.AddComponent<WalkabilityMap>();
            wm.InitFromTemplate(template);
            return wm;
        }

        private Tile CreateTile(string tileName)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.name = tileName;
            created.Add(tile);
            return tile;
        }

        private static void SetPrivate(object target, string fieldName, object value)
        {
            FieldInfo field = target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null) throw new System.Exception($"Field '{fieldName}' not found on {target.GetType().Name}");
            field.SetValue(target, value);
        }

        private static void SetPropDefBlocking(RIMA.MapDesigner.Props.PropDefinitionSO def)
        {
            def.blocksWalkable = true;
        }
    }
}
