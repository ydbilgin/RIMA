#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class PropFootprintValidatorTests
    {
        private readonly List<Object> created = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            foreach (Object obj in created)
            {
                if (obj != null) Object.DestroyImmediate(obj);
            }
            created.Clear();
        }

        [Test]
        public void Validate_NullPropDef_ReturnsInvalidArgument()
        {
            var result = PropFootprintValidator.Validate(null, Vector2Int.zero, CreateTemplate(), null, null, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.InvalidArgument, result);
        }

        [Test]
        public void Validate_NullTemplate_ReturnsInvalidArgument()
        {
            var result = PropFootprintValidator.Validate(CreateProp(), Vector2Int.zero, null, null, null, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.InvalidArgument, result);
        }

        [Test]
        public void Validate_OutOfBoundsFootprint_ReturnsOutOfBounds()
        {
            PropDefinitionSO prop = CreateProp();
            prop.footprintSize = new Vector2Int(2, 2);
            var result = PropFootprintValidator.Validate(prop, new Vector2Int(7, 7), CreateTemplate(), null, null, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.OutOfBounds, result);
        }

        [Test]
        public void Validate_OverlapsExistingProp_ReturnsOverlapsExistingProp()
        {
            PropDefinitionSO prop = CreateProp();
            var existing = new List<PropPlacementData> { new PropPlacementData(prop.propId, new Vector2Int(2, 2)) };
            var result = PropFootprintValidator.Validate(prop, new Vector2Int(2, 2), CreateTemplate(), null, existing, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.OverlapsExistingProp, result);
        }

        [Test]
        public void Validate_RequiresWalkableButNonWalkable_ReturnsViolatesWalkableConstraint()
        {
            PropDefinitionSO prop = CreateProp();
            prop.requiresWalkableTile = true;
            var result = PropFootprintValidator.Validate(prop, new Vector2Int(0, 0), CreateTemplate(), null, null, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.ViolatesWalkableConstraint, result);
        }

        [Test]
        public void Validate_OnDoorSafetyRole_ReturnsViolatesRoleConstraint()
        {
            PropDefinitionSO prop = CreateProp();
            RoomTemplateSO template = CreateTemplate();
            CompositionRoleMap map = CreateRoleMap(template, new Vector2Int(3, 3), CompositionRole.DoorSafety);
            var result = PropFootprintValidator.Validate(prop, new Vector2Int(3, 3), template, map, null, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.ViolatesRoleConstraint, result);
        }

        [Test]
        public void Validate_OnWallBandRole_ReturnsViolatesRoleConstraint()
        {
            PropDefinitionSO prop = CreateProp();
            RoomTemplateSO template = CreateTemplate();
            CompositionRoleMap map = CreateRoleMap(template, new Vector2Int(3, 3), CompositionRole.WallBand);
            var result = PropFootprintValidator.Validate(prop, new Vector2Int(3, 3), template, map, null, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.ViolatesRoleConstraint, result);
        }

        [Test]
        public void Validate_OnFocalClusterRole_ReturnsValid()
        {
            PropDefinitionSO prop = CreateProp();
            RoomTemplateSO template = CreateTemplate();
            CompositionRoleMap map = CreateRoleMap(template, new Vector2Int(3, 3), CompositionRole.FocalCluster);
            var result = PropFootprintValidator.Validate(prop, new Vector2Int(3, 3), template, map, null, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.Valid, result);
        }

        [Test]
        public void Validate_WithinMinDistance_ReturnsTooCloseToOtherProp()
        {
            PropDefinitionSO prop = CreateProp();
            prop.distanceFromOtherProps = 2f;
            var existing = new List<PropPlacementData> { new PropPlacementData(prop.propId, new Vector2Int(2, 2)) };
            var result = PropFootprintValidator.Validate(prop, new Vector2Int(3, 2), CreateTemplate(), null, existing, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.TooCloseToOtherProp, result);
        }

        [Test]
        public void Validate_NullRoleMap_SkipsRoleCheck_StillValidates()
        {
            PropDefinitionSO prop = CreateProp();
            var result = PropFootprintValidator.Validate(prop, new Vector2Int(3, 3), CreateTemplate(), null, null, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.Valid, result);
        }

        [Test]
        public void Validate_NullForbiddenRoles_TreatsAsEmpty()
        {
            PropDefinitionSO prop = CreateProp();
            prop.forbiddenRoles = null;
            RoomTemplateSO template = CreateTemplate();
            CompositionRoleMap map = CreateRoleMap(template, new Vector2Int(3, 3), CompositionRole.DoorSafety);
            var result = PropFootprintValidator.Validate(prop, new Vector2Int(3, 3), template, map, null, out _);
            Assert.AreEqual(PropFootprintValidator.ValidationResult.Valid, result);
        }

        private PropDefinitionSO CreateProp()
        {
            var prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.propId = "test_prop";
            prop.distanceFromOtherProps = 0f;
            created.Add(prop);
            return prop;
        }

        private RoomTemplateSO CreateTemplate()
        {
            var template = ScriptableObject.CreateInstance<RoomTemplateSO>();
            template.bounds = new RectInt(0, 0, 8, 8);
            template.cameraBounds = new CameraBounds { tileRect = new RectInt(1, 1, 6, 6) };
            template.props = new List<PropPlacementData>();
            created.Add(template);
            return template;
        }

        private static CompositionRoleMap CreateRoleMap(RoomTemplateSO template, Vector2Int tile, CompositionRole role)
        {
            var map = new CompositionRoleMap(template.bounds);
            map.SetRoleAt(tile, role);
            return map;
        }
    }
}
#endif
