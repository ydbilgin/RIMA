#if UNITY_EDITOR
using NUnit.Framework;

namespace RIMA.Editor.Rooms
{
    /// <summary>
    /// Targeted tests for RoomJsonImporter props-preservation edge cases.
    /// Lives alongside RoomJsonImporter so it compiles in the same editor assembly
    /// and can access the public test-only bridge methods.
    /// </summary>
    public class RoomJsonImporterPropsTests
    {
        // Minimal valid v2 JSON WITH a "props" key (empty array)
        private const string V2WithPropsKey =
            "{\"version\":2,\"id\":\"test_room\",\"roomType\":\"Combat\"," +
            "\"size\":{\"w\":5,\"h\":5}," +
            "\"walkable\":[\"#####\",\"#####\",\"#####\",\"#####\",\"#####\"]," +
            "\"spawn\":{\"x\":2,\"y\":2}," +
            "\"exitSlots\":{}," +
            "\"props\":[]," +
            "\"enemySpawns\":[]," +
            "\"notes\":\"\"}";

        // Minimal valid v2 JSON WITHOUT a "props" key
        private const string V2WithoutPropsKey =
            "{\"version\":2,\"id\":\"test_room\",\"roomType\":\"Combat\"," +
            "\"size\":{\"w\":5,\"h\":5}," +
            "\"walkable\":[\"#####\",\"#####\",\"#####\",\"#####\",\"#####\"]," +
            "\"spawn\":{\"x\":2,\"y\":2}," +
            "\"exitSlots\":{}," +
            "\"enemySpawns\":[]," +
            "\"notes\":\"\"}";

        // v1 JSON (no version field)
        private const string V1Json =
            "{\"roomId\":\"test_v1\",\"roomType\":\"Combat\"," +
            "\"width\":5,\"height\":5," +
            "\"grid\":[\"#####\",\"#####\",\"#####\",\"#####\",\"#####\"]," +
            "\"notes\":\"\"}";

        [Test]
        public void V2Json_WithPropsKey_HasPropsArrayTrue()
        {
            bool result = RoomJsonImporter.TestOnly_V2JsonHasPropsKey(V2WithPropsKey);
            Assert.IsTrue(result,
                "v2 JSON that contains a 'props' key should signal hasPropsArray=true (overwrite existing props).");
        }

        [Test]
        public void V2Json_WithoutPropsKey_HasPropsArrayFalse()
        {
            bool result = RoomJsonImporter.TestOnly_V2JsonHasPropsKey(V2WithoutPropsKey);
            Assert.IsFalse(result,
                "v2 JSON missing the 'props' key must NOT overwrite existing props (hasPropsArray=false).");
        }

        [Test]
        public void V1Json_NoPropsKey_HasPropsArrayFalse()
        {
            bool result = RoomJsonImporter.TestOnly_V2JsonHasPropsKey(V1Json);
            Assert.IsFalse(result,
                "v1 JSON (version < 2) must never set hasPropsArray=true.");
        }
    }
}
#endif
