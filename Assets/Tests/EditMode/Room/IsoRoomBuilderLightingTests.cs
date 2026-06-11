using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using RIMA.MapDesigner.Room.Runtime;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.Tests.Room
{
    public sealed class IsoRoomBuilderLightingTests
    {
        private readonly List<Object> created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = created.Count - 1; i >= 0; i--)
            {
                if (created[i] != null)
                {
                    Object.DestroyImmediate(created[i]);
                }
            }

            created.Clear();
        }

        [Test]
        public void Build_NoLightingProfile_CreatesEmptyLightingRoot()
        {
            IsoRoomBuilder builder = CreateRig();
            RoomTemplateSO template = CreateTemplate();
            template.lightingProfile = null;

            builder.Build(template);

            Transform lightingRoot = FindChild(builder.transform, "Lighting");
            Assert.IsNotNull(lightingRoot);
            Assert.AreEqual(0, lightingRoot.childCount);
            Assert.AreEqual(1, CountDirectChildrenNamed(builder.transform, "Lighting"));
        }

        [Test]
        public void Build_LightingProfile_CreatesGlobalAndPointLightsUnderLightingRoot()
        {
            IsoRoomBuilder builder = CreateRig();
            RoomTemplateSO template = CreateTemplate();
            RoomLightingProfileSO profile = ScriptableObject.CreateInstance<RoomLightingProfileSO>();
            created.Add(profile);
            profile.globalColor = Color.gray;
            profile.globalIntensity = 1.25f;
            profile.pointLights.Clear();
            profile.pointLights.Add(new RoomLightingProfileSO.PointLightSpec
            {
                normalizedRoomPosition = new Vector2(0.25f, 0.25f),
                color = Color.red,
                intensity = 0.7f,
                innerRadius = 0.25f,
                outerRadius = 2f
            });
            profile.pointLights.Add(new RoomLightingProfileSO.PointLightSpec
            {
                normalizedRoomPosition = new Vector2(0.75f, 0.75f),
                color = Color.blue,
                intensity = 0.9f,
                innerRadius = 0.5f,
                outerRadius = 3f
            });
            template.lightingProfile = profile;

            builder.Build(template);

            Transform lightingRoot = FindChild(builder.transform, "Lighting");
            Assert.IsNotNull(lightingRoot);
            Assert.AreEqual(3, lightingRoot.childCount);
            Assert.AreEqual(3, CountLight2DChildren(lightingRoot));
            Assert.AreEqual("Global Light 2D", lightingRoot.GetChild(0).name);
            Assert.AreEqual("Point Light 2D 1", lightingRoot.GetChild(1).name);
            Assert.AreEqual("Point Light 2D 2", lightingRoot.GetChild(2).name);
        }

        private IsoRoomBuilder CreateRig()
        {
            GameObject gridObject = new GameObject("TestGrid");
            created.Add(gridObject);
            Grid grid = gridObject.AddComponent<Grid>();

            GameObject groundObject = new GameObject("GroundTilemap");
            created.Add(groundObject);
            groundObject.transform.SetParent(gridObject.transform, false);
            Tilemap groundTilemap = groundObject.AddComponent<Tilemap>();

            GameObject collisionObject = new GameObject("CollisionTilemap");
            created.Add(collisionObject);
            collisionObject.transform.SetParent(gridObject.transform, false);
            Tilemap collisionTilemap = collisionObject.AddComponent<Tilemap>();

            GameObject builderObject = new GameObject("IsoRoomBuilder");
            created.Add(builderObject);
            IsoRoomBuilder builder = builderObject.AddComponent<IsoRoomBuilder>();
            SetPrivate(builder, "grid", grid);
            SetPrivate(builder, "groundTilemap", groundTilemap);
            SetPrivate(builder, "collisionTilemap", collisionTilemap);
            SetPrivate(builder, "floorTile", CreateTile("floor"));
            SetPrivate(builder, "collisionTile", CreateTile("collision"));
            return builder;
        }

        private Tile CreateTile(string name)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.name = name;
            created.Add(tile);
            return tile;
        }

        private RoomTemplateSO CreateTemplate()
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            created.Add(template);
            template.roomId = "lighting_test";
            template.roomType = RIMA.RoomType.Combat;
            template.bounds = new RectInt(0, 0, 5, 5);
            template.walkableGrid = null;
            template.playerSpawn = new PlayerSpawnSocket { position = new Vector2Int(2, 2) };
            return template;
        }

        private static Transform FindChild(Transform parent, string childName)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.name == childName)
                {
                    return child;
                }
            }

            return null;
        }

        private static int CountDirectChildrenNamed(Transform parent, string childName)
        {
            int count = 0;
            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).name == childName)
                {
                    count++;
                }
            }

            return count;
        }

        private static int CountLight2DChildren(Transform parent)
        {
            int count = 0;
            for (int i = 0; i < parent.childCount; i++)
            {
                Component[] components = parent.GetChild(i).GetComponents<Component>();
                for (int c = 0; c < components.Length; c++)
                {
                    if (components[c] != null && components[c].GetType().FullName == "UnityEngine.Rendering.Universal.Light2D")
                    {
                        count++;
                        break;
                    }
                }
            }

            return count;
        }

        private static void SetPrivate(object target, string fieldName, object value)
        {
            target.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance).SetValue(target, value);
        }
    }
}
