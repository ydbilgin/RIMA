namespace RIMA.Tests.Editor
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using RIMA.Editor.RoomDesigner;
    using RIMA.Runtime.Rooms;
    using RIMA.Systems.Map;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public sealed class RoomDesignerFaz15Tests
    {
        private readonly List<Object> cleanup = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = cleanup.Count - 1; i >= 0; i--)
            {
                if (cleanup[i] != null)
                {
                    Object.DestroyImmediate(cleanup[i]);
                }
            }

            cleanup.Clear();
        }

        [Test]
        public void DecalPainter_SameSeed_ProducesBitIdenticalPattern()
        {
            Tilemap decalsA = CreateTilemap("DecalsA");
            Tilemap decalsB = CreateTilemap("DecalsB");
            RoomBlueprint bpA = CreateBlueprint(12, 12, 42);
            RoomBlueprint bpB = CreateBlueprint(12, 12, 42);
            Sprite[] sprites = { CreateSprite("decal_a"), CreateSprite("decal_b"), CreateSprite("decal_c") };

            Assert.IsTrue(DecalPainter.PaintDecals(decalsA, bpA, sprites, 42, 1f));
            Assert.IsTrue(DecalPainter.PaintDecals(decalsB, bpB, sprites, 42, 1f));

            CollectionAssert.AreEqual(bpA.decalVariantIndex, bpB.decalVariantIndex);
        }

        [Test]
        public void PropPlacer_CombatArchetype_Places4MobAnd1Loot()
        {
            GameObject stageRoot = new GameObject("stage");
            cleanup.Add(stageRoot);
            stageRoot.AddComponent<Grid>();
            GameObject mobPrefab = CreatePrefab("MobPrefab");
            GameObject lootPrefab = CreatePrefab("LootPrefab");
            RoomBlueprint bp = CreateBlueprint(20, 20, 137);
            var specs = new[]
            {
                new PropSpec { prefab = mobPrefab, anchorTag = "MobSpawner", depthBandMin = 0, depthBandMax = 2 },
                new PropSpec { prefab = lootPrefab, anchorTag = "Loot", depthBandMin = 0, depthBandMax = 2 }
            };
            var anchors = RimaArchetypeGenerators.GetDefaultAnchorZones("combat", 137, 20, 20);

            List<GameObject> placed = PropPlacer.PlaceProps(stageRoot, anchors, specs, bp, 137);

            Assert.AreEqual(5, placed.Count);
            Assert.AreEqual(4, placed.FindAll(go => go.name.Contains("MobSpawner")).Count);
            Assert.AreEqual(1, placed.FindAll(go => go.name.Contains("Loot")).Count);
        }

        [Test]
        public void WangTransitionResolver_BiomeEdge_SelectsEdgeVariant()
        {
            Tilemap floor = CreateTilemap("Floor");
            Tile floorTile = CreateTile("floor");
            Tile northTransition = CreateTile("north_transition");
            Tile southTransition = CreateTile("south_transition");
            Tile sideTransition = CreateTile("side_transition");
            RoomBlueprint bp = CreateBlueprint(3, 3, 2501);
            bp.biomeType = BiomeType.Keep;
            bp.overrideVariantIndex = new bool[9];

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    floor.SetTile(new Vector3Int(x, y, 0), floorTile);
                }
            }

            Assert.AreEqual(1, WallAutoConnect.GetTransitionVariantIndex(BiomeType.Keep, BiomeType.Crypt, 4));
            Assert.IsTrue(BiomeTransitionPainter.ApplyBiomeTransitions(floor, bp, new TileBase[] { northTransition, southTransition, sideTransition }));
            Assert.AreSame(northTransition, floor.GetTile(new Vector3Int(1, 2, 0)));
        }

        private Tilemap CreateTilemap(string name)
        {
            GameObject root = new GameObject(name + "_Root");
            cleanup.Add(root);
            root.AddComponent<Grid>();
            GameObject tilemapObject = new GameObject(name);
            cleanup.Add(tilemapObject);
            tilemapObject.transform.SetParent(root.transform, false);
            return tilemapObject.AddComponent<Tilemap>();
        }

        private RoomBlueprint CreateBlueprint(int width, int height, int seed)
        {
            RoomBlueprint bp = ScriptableObject.CreateInstance<RoomBlueprint>();
            cleanup.Add(bp);
            bp.roomWidth = width;
            bp.roomHeight = height;
            bp.roomOrigin = Vector3Int.zero;
            bp.noiseSeed = seed;
            return bp;
        }

        private Sprite CreateSprite(string name)
        {
            Texture2D texture = new Texture2D(2, 2);
            cleanup.Add(texture);
            texture.SetPixels(new[] { Color.white, Color.white, Color.white, Color.white });
            texture.Apply();
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 2, 2), new Vector2(0.5f, 0.5f), 2f);
            sprite.name = name;
            cleanup.Add(sprite);
            return sprite;
        }

        private Tile CreateTile(string name)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.name = name;
            cleanup.Add(tile);
            return tile;
        }

        private GameObject CreatePrefab(string name)
        {
            GameObject prefab = new GameObject(name);
            cleanup.Add(prefab);
            prefab.AddComponent<SpriteRenderer>();
            return prefab;
        }
    }
}
