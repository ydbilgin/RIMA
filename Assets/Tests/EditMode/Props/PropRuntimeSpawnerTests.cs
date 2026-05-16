#if UNITY_EDITOR
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Runtime;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class PropRuntimeSpawnerTests
    {
        [Test]
        public void Spawn_NullTemplate_ReturnsEmpty()
        {
            PropRuntimeSpawner spawner = new PropRuntimeSpawner();
            PropRegistrySO registry = ScriptableObject.CreateInstance<PropRegistrySO>();
            GameObject parent = new GameObject("parent");
            try
            {
                PropRuntimeSpawner.SpawnResult result = spawner.Spawn(null, registry, parent.transform);
                Assert.AreEqual(0, result.requested);
                Assert.AreEqual(0, result.spawned);
            }
            finally
            {
                Object.DestroyImmediate(parent);
                Object.DestroyImmediate(registry);
            }
        }

        [Test]
        public void Spawn_UnknownGuid_IncrementsUnresolved()
        {
            PropRuntimeSpawner spawner = new PropRuntimeSpawner();
            PropRegistrySO registry = ScriptableObject.CreateInstance<PropRegistrySO>();
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.bounds = new RectInt(0, 0, 8, 8);
            template.props = new List<PropPlacementData>
            {
                new PropPlacementData("unknown_guid", new Vector2Int(2, 2))
            };
            GameObject parent = new GameObject("parent");
            try
            {
                PropRuntimeSpawner.SpawnResult result = spawner.Spawn(template, registry, parent.transform);
                Assert.AreEqual(1, result.requested);
                Assert.AreEqual(0, result.spawned);
                Assert.AreEqual(1, result.unresolved);
            }
            finally
            {
                Object.DestroyImmediate(parent);
                Object.DestroyImmediate(registry);
                Object.DestroyImmediate(template);
            }
        }

        [Test]
        public void Spawn_KnownProp_InstantiatesGameObject()
        {
            PropRuntimeSpawner spawner = new PropRuntimeSpawner();
            PropRegistrySO registry = ScriptableObject.CreateInstance<PropRegistrySO>();
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.propId = "spawn_test_prop";
            prop.footprintSize = new Vector2Int(1, 1);
            prop.blocksWalkable = true;
            registry.EditorAddProp(prop);
            registry.RebuildIndex();

            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.bounds = new RectInt(0, 0, 8, 8);
            template.props = new List<PropPlacementData>
            {
                new PropPlacementData("spawn_test_prop", new Vector2Int(3, 4)) { rotationSteps = 0 }
            };
            GameObject parent = new GameObject("parent");
            try
            {
                PropRuntimeSpawner.SpawnResult result = spawner.Spawn(template, registry, parent.transform);
                Assert.AreEqual(1, result.requested);
                Assert.AreEqual(1, result.spawned);
                Assert.AreEqual(0, result.unresolved);
                Assert.AreEqual(1, result.instances.Count);
                GameObject instance = result.instances[0];
                Assert.IsNotNull(instance.GetComponent<SpriteRenderer>());
                Assert.IsNotNull(instance.GetComponent<PropSorterRuntime>());
                Assert.IsNotNull(instance.GetComponent<PropColliderAutoBuilder>());
                Assert.AreEqual(3f, instance.transform.localPosition.x);
                Assert.AreEqual(4f, instance.transform.localPosition.y);
            }
            finally
            {
                Object.DestroyImmediate(parent);
                Object.DestroyImmediate(template);
                Object.DestroyImmediate(prop);
                Object.DestroyImmediate(registry);
            }
        }

        [Test]
        public void Spawn_TenRoomLibrary_RendersUnder2Seconds()
        {
            PropRuntimeSpawner spawner = new PropRuntimeSpawner();
            PropRegistrySO registry = ScriptableObject.CreateInstance<PropRegistrySO>();
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.propId = "perf_test_prop";
            prop.footprintSize = new Vector2Int(1, 1);
            prop.blocksWalkable = true;
            registry.EditorAddProp(prop);
            registry.RebuildIndex();

            List<RoomTemplateSO> rooms = new List<RoomTemplateSO>();
            List<GameObject> parents = new List<GameObject>();
            try
            {
                for (int r = 0; r < 10; r++)
                {
                    RoomTemplateSO room = ScriptableObject.CreateInstance<RoomTemplateSO>();
                    room.bounds = new RectInt(0, 0, 12, 10);
                    room.props = new List<PropPlacementData>();
                    for (int i = 0; i < 20; i++)
                    {
                        room.props.Add(new PropPlacementData("perf_test_prop", new Vector2Int(i % 12, (i / 12) % 10)));
                    }
                    rooms.Add(room);
                    parents.Add(new GameObject($"perf_parent_{r}"));
                }

                Stopwatch sw = Stopwatch.StartNew();
                int totalSpawned = 0;
                for (int i = 0; i < rooms.Count; i++)
                {
                    PropRuntimeSpawner.SpawnResult result = spawner.Spawn(rooms[i], registry, parents[i].transform);
                    totalSpawned += result.spawned;
                }
                sw.Stop();

                Assert.AreEqual(200, totalSpawned, "Each room contributes 20 props (10x20).");
                Assert.Less(sw.ElapsedMilliseconds, 2000, $"Spawn took {sw.ElapsedMilliseconds} ms, expected < 2000.");
            }
            finally
            {
                foreach (GameObject p in parents) if (p != null) Object.DestroyImmediate(p);
                foreach (RoomTemplateSO r in rooms) if (r != null) Object.DestroyImmediate(r);
                Object.DestroyImmediate(prop);
                Object.DestroyImmediate(registry);
            }
        }
    }
}
#endif
