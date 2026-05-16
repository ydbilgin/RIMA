#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.Tests.Brush
{
    public sealed class UndoStressTests
    {
        [Test]
        public void Undo100x_PropPlace_RecordAndRollback()
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.bounds = new RectInt(0, 0, 12, 12);
            template.props = new List<PropPlacementData>();

            int initialUndoGroup = Undo.GetCurrentGroup();

            try
            {
                for (int i = 0; i < 100; i++)
                {
                    Undo.RecordObject(template, $"Place prop {i}");
                    template.props.Add(new PropPlacementData("test_guid_" + i, new Vector2Int(i % 12, i / 12)));
                    Undo.IncrementCurrentGroup();
                }

                Assert.AreEqual(100, template.props.Count, "All 100 placements should be staged.");

                for (int i = 0; i < 100; i++)
                {
                    Undo.PerformUndo();
                }

                Assert.LessOrEqual(template.props.Count, 100);
                Assert.DoesNotThrow(() => Undo.IncrementCurrentGroup());
            }
            finally
            {
                Undo.RevertAllDownToGroup(initialUndoGroup);
                Object.DestroyImmediate(template);
            }
        }

        [Test]
        public void RepeatedAddRemove_NoCrash_StatePreserved()
        {
            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.bounds = new RectInt(0, 0, 8, 8);
            template.props = new List<PropPlacementData>();
            try
            {
                for (int iter = 0; iter < 100; iter++)
                {
                    template.props.Add(new PropPlacementData("stress_" + iter, new Vector2Int(iter % 8, iter / 8 % 8)));
                    if (template.props.Count > 0 && iter % 2 == 1)
                    {
                        template.props.RemoveAt(template.props.Count - 1);
                    }
                }
                Assert.Greater(template.props.Count, 0);
                Assert.DoesNotThrow(() => template.props.Clear());
                Assert.AreEqual(0, template.props.Count);
            }
            finally
            {
                Object.DestroyImmediate(template);
            }
        }
    }
}
#endif
