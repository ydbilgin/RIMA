using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RIMA.MapDesigner.Editor.Blueprint;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Tests
{
    public sealed class RoomSaveLoadServiceTests
    {
        private const string TestFolder = "Assets/Tests/_TestArtifacts";
        private readonly List<string> createdAssetPaths = new List<string>();

        [SetUp]
        public void SetUp()
        {
            EnsureFolder(TestFolder);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = createdAssetPaths.Count - 1; i >= 0; i--)
            {
                if (!string.IsNullOrEmpty(createdAssetPaths[i]))
                {
                    AssetDatabase.DeleteAsset(createdAssetPaths[i]);
                }
            }

            createdAssetPaths.Clear();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (!AssetDatabase.IsValidFolder(TestFolder))
            {
                return;
            }

            string[] guids = AssetDatabase.FindAssets(string.Empty, new[] { TestFolder });
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (!string.IsNullOrEmpty(path) && path.StartsWith(TestFolder + "/", StringComparison.Ordinal))
                {
                    AssetDatabase.DeleteAsset(path);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        [Test]
        public void SaveAsNew_CreatesAssetAtPath()
        {
            string path = NextAssetPath("created_room");

            RoomBlueprintSO room = SaveRoom(path, CreateCanvas(4), null, 101);

            Assert.IsNotNull(room);
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<RoomBlueprintSO>(path));
        }

        [Test]
        public void SaveAsNew_PersistsIntentMap()
        {
            BlueprintCanvas canvas = CreateCanvas(10);
            string path = NextAssetPath("intent_room");

            SaveRoom(path, canvas, null, 102);
            RoomBlueprintSO room = AssetDatabase.LoadAssetAtPath<RoomBlueprintSO>(path);
            (BlueprintCanvas loadedCanvas, _) = RoomSaveLoadService.Load(room);

            Assert.AreEqual(10, room.intentMap.Count);
            AssertCanvasEqual(canvas, loadedCanvas);
        }

        [Test]
        public void SaveAsNew_PersistsSeed()
        {
            const int savedSeed = 98765;
            string path = NextAssetPath("seed_room");

            RoomBlueprintSO room = SaveRoom(path, CreateCanvas(3), null, savedSeed);
            (_, int loadedSeed) = RoomSaveLoadService.Load(room);

            Assert.AreEqual(savedSeed, room.currentSeed);
            Assert.AreEqual(savedSeed, loadedSeed);
        }

        [Test]
        public void SaveAsNew_PersistsProfileReference()
        {
            BlueprintProfileSO profile = CreateProfileAsset();
            string path = NextAssetPath("profile_room");

            RoomBlueprintSO room = SaveRoom(path, CreateCanvas(2), profile, 103);

            Assert.AreSame(profile, room.profile);
        }

        [Test]
        public void Overwrite_UpdatesIntentMap()
        {
            string path = NextAssetPath("overwrite_room");
            RoomBlueprintSO room = SaveRoom(path, CreateCanvas(4), null, 104);
            var nextCanvas = new BlueprintCanvas(new Vector2Int(8, 8));
            nextCanvas.Paint(new Vector2Int(7, 7), "water", 1);

            RoomSaveLoadService.Overwrite(room, nextCanvas, 105);
            (BlueprintCanvas loadedCanvas, int loadedSeed) = RoomSaveLoadService.Load(room);

            Assert.AreEqual(1, loadedCanvas.Count);
            Assert.AreEqual("water", loadedCanvas.GetZoneAt(new Vector2Int(7, 7)));
            Assert.AreEqual(105, loadedSeed);
        }

        [Test]
        public void Load_RestoresCanvas()
        {
            BlueprintCanvas canvas = CreateCanvas(10);
            string path = NextAssetPath("load_room");

            RoomBlueprintSO room = SaveRoom(path, canvas, null, 106);
            (BlueprintCanvas loadedCanvas, _) = RoomSaveLoadService.Load(room);

            AssertCanvasEqual(canvas, loadedCanvas);
        }

        [Test]
        public void ListRoomsInFolder_FindsAllRoomAssets()
        {
            string firstPath = NextAssetPath("list_room_a");
            string secondPath = NextAssetPath("list_room_b");
            RoomBlueprintSO first = SaveRoom(firstPath, CreateCanvas(2), null, 107);
            RoomBlueprintSO second = SaveRoom(secondPath, CreateCanvas(3), null, 108);

            List<RoomBlueprintSO> rooms = RoomSaveLoadService.ListRoomsInFolder(TestFolder).ToList();

            Assert.Contains(first, rooms);
            Assert.Contains(second, rooms);
        }

        private RoomBlueprintSO SaveRoom(string path, BlueprintCanvas canvas, BlueprintProfileSO profile, int seed)
        {
            RoomBlueprintSO room = RoomSaveLoadService.SaveAsNew(
                canvas,
                profile,
                seed,
                path,
                System.IO.Path.GetFileNameWithoutExtension(path),
                "Test Room");
            createdAssetPaths.Add(path);
            return room;
        }

        private BlueprintProfileSO CreateProfileAsset()
        {
            BlueprintProfileSO profile = ScriptableObject.CreateInstance<BlueprintProfileSO>();
            profile.profileId = "test_profile";
            profile.gridSize = new Vector2Int(8, 8);
            string path = NextAssetPath("profile");
            AssetDatabase.CreateAsset(profile, path);
            createdAssetPaths.Add(path);
            AssetDatabase.SaveAssets();
            return profile;
        }

        private static BlueprintCanvas CreateCanvas(int cellCount)
        {
            var canvas = new BlueprintCanvas(new Vector2Int(8, 8));
            for (int i = 0; i < cellCount; i++)
            {
                canvas.Paint(new Vector2Int(i % 8, i / 8), i % 2 == 0 ? "path" : "grass", 1);
            }

            return canvas;
        }

        private string NextAssetPath(string stem)
        {
            return $"{TestFolder}/{stem}_{Guid.NewGuid():N}.asset";
        }

        private static void AssertCanvasEqual(BlueprintCanvas expected, BlueprintCanvas actual)
        {
            Assert.AreEqual(expected.GridSize, actual.GridSize);
            Assert.AreEqual(expected.Count, actual.Count);
            foreach (KeyValuePair<Vector2Int, string> pair in expected.IntentMap)
            {
                Assert.AreEqual(pair.Value, actual.GetZoneAt(pair.Key), $"Cell {pair.Key}");
            }
        }

        private static void EnsureFolder(string folderPath)
        {
            if (AssetDatabase.IsValidFolder(folderPath))
            {
                return;
            }

            string[] parts = folderPath.Split('/');
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                {
                    AssetDatabase.CreateFolder(current, parts[i]);
                }

                current = next;
            }
        }
    }
}
