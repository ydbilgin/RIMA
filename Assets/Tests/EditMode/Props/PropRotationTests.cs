#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class PropRotationTests
    {
        [Test]
        public void RotateClockwise_Increments90()
        {
            PropPlacementData data = new PropPlacementData("guid", Vector2Int.zero);
            Assert.AreEqual(0, data.rotationSteps);
            data.RotateClockwise();
            Assert.AreEqual(1, data.rotationSteps);
            data.RotateClockwise();
            Assert.AreEqual(2, data.rotationSteps);
            data.RotateClockwise();
            Assert.AreEqual(3, data.rotationSteps);
        }

        [Test]
        public void RotateClockwise_Wraps360To0()
        {
            PropPlacementData data = new PropPlacementData("guid", Vector2Int.zero);
            data.RotateClockwise();
            data.RotateClockwise();
            data.RotateClockwise();
            data.RotateClockwise();
            Assert.AreEqual(0, data.rotationSteps);
        }

        [Test]
        public void GetRotatedFootprint_Rotation90_SwapsWidthHeight()
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.footprintSize = new Vector2Int(3, 1);
            try
            {
                Vector2Int rotated = PropFootprintValidator.GetRotatedFootprint(prop, 1);
                Assert.AreEqual(1, rotated.x);
                Assert.AreEqual(3, rotated.y);
            }
            finally
            {
                Object.DestroyImmediate(prop);
            }
        }

        [Test]
        public void GetRotatedFootprint_Rotation270_SwapsWidthHeight()
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.footprintSize = new Vector2Int(2, 4);
            try
            {
                Vector2Int rotated = PropFootprintValidator.GetRotatedFootprint(prop, 3);
                Assert.AreEqual(4, rotated.x);
                Assert.AreEqual(2, rotated.y);
            }
            finally
            {
                Object.DestroyImmediate(prop);
            }
        }

        [Test]
        public void Validate_Rotation90_FitsRotatedFootprintInBounds()
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.footprintSize = new Vector2Int(3, 1);
            prop.forbiddenRoles = null;
            prop.distanceFromOtherProps = 0f;

            RoomTemplateSO template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.bounds = new RectInt(0, 0, 3, 3);
            template.props = new List<PropPlacementData>();

            try
            {
                PropFootprintValidator.ValidationResult notRotated = PropFootprintValidator.Validate(
                    prop, new Vector2Int(0, 0), 0, template, null, null, out _);
                Assert.AreEqual(PropFootprintValidator.ValidationResult.Valid, notRotated);

                PropFootprintValidator.ValidationResult rotated = PropFootprintValidator.Validate(
                    prop, new Vector2Int(0, 0), 1, template, null, null, out _);
                Assert.AreEqual(PropFootprintValidator.ValidationResult.Valid, rotated);

                PropFootprintValidator.ValidationResult outOfBoundsWhenRotated = PropFootprintValidator.Validate(
                    prop, new Vector2Int(0, 1), 1, template, null, null, out _);
                Assert.AreEqual(PropFootprintValidator.ValidationResult.OutOfBounds, outOfBoundsWhenRotated);
            }
            finally
            {
                Object.DestroyImmediate(template);
                Object.DestroyImmediate(prop);
            }
        }
    }
}
#endif
