#if UNITY_EDITOR
using NUnit.Framework;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Tests.Room
{
    public sealed class RoomTemplateWalkableGridTests
    {
        [Test]
        public void IsWalkable_EmptyGrid_FallsBackToBounds()
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.bounds = new RectInt(0, 0, 5, 4);
            template.walkableGrid = null;

            Assert.IsTrue(template.IsWalkable(new Vector2Int(0, 0)));
            Assert.IsTrue(template.IsWalkable(new Vector2Int(4, 3)));
            Assert.IsFalse(template.IsWalkable(new Vector2Int(5, 0)));
            Assert.IsFalse(template.IsWalkable(new Vector2Int(-1, 0)));

            Object.DestroyImmediate(template);
        }

        [Test]
        public void IsWalkable_PopulatedGrid_ReturnsCorrectValue()
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.bounds = new RectInt(0, 0, 3, 2);
            template.walkableGrid = new bool[]
            {
                true, false, true,
                false, true, false
            };

            Assert.IsTrue(template.IsWalkable(new Vector2Int(0, 0)));
            Assert.IsFalse(template.IsWalkable(new Vector2Int(1, 0)));
            Assert.IsTrue(template.IsWalkable(new Vector2Int(2, 0)));
            Assert.IsFalse(template.IsWalkable(new Vector2Int(0, 1)));
            Assert.IsTrue(template.IsWalkable(new Vector2Int(1, 1)));
            Assert.IsFalse(template.IsWalkable(new Vector2Int(2, 1)));

            Object.DestroyImmediate(template);
        }

        [Test]
        public void IsWalkable_OutOfBounds_ReturnsFalse()
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.bounds = new RectInt(2, 2, 3, 3);
            template.walkableGrid = new bool[9];
            for (int i = 0; i < 9; i++) template.walkableGrid[i] = true;

            Assert.IsFalse(template.IsWalkable(new Vector2Int(0, 0)));
            Assert.IsFalse(template.IsWalkable(new Vector2Int(5, 5)));
            Assert.IsTrue(template.IsWalkable(new Vector2Int(2, 2)));
            Assert.IsTrue(template.IsWalkable(new Vector2Int(4, 4)));

            Object.DestroyImmediate(template);
        }
    }
}
#endif
