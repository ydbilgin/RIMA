#if UNITY_EDITOR
using System.Collections.Generic;
using NUnit.Framework;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Editor;
using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class PropPlacerTests
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
        public void OnClick_ValidTile_AppendsToTemplate()
        {
            PropPlacer placer = new PropPlacer();
            RoomTemplateSO template = CreateTemplate();
            PropDefinitionSO prop = CreateProp();

            placer.OnSceneClick(new Vector2Int(3, 3), prop, template, null);

            Assert.AreEqual(1, template.props.Count);
            Assert.AreEqual(new Vector2Int(3, 3), template.props[0].tilePosition);
            Assert.AreEqual(0, template.props[0].rotationSteps);
        }

        [Test]
        public void OnClick_InvalidTile_TemplateUnchanged()
        {
            PropPlacer placer = new PropPlacer();
            RoomTemplateSO template = CreateTemplate();
            PropDefinitionSO prop = CreateProp();

            placer.OnSceneClick(new Vector2Int(99, 99), prop, template, null);

            Assert.AreEqual(0, template.props.Count);
        }

        [Test]
        public void OnHover_UpdatesLastValidationResult()
        {
            PropPlacer placer = new PropPlacer();
            RoomTemplateSO template = CreateTemplate();
            PropDefinitionSO prop = CreateProp();
            CompositionRoleMap map = new CompositionRoleMap(template.bounds);
            map.SetRoleAt(new Vector2Int(3, 3), CompositionRole.DoorSafety);

            placer.OnHover(new Vector2Int(3, 3), prop, template, map);

            Assert.AreEqual(PropFootprintValidator.ValidationResult.ViolatesRoleConstraint, placer.LastValidationResult);
            Assert.AreEqual(new Vector2Int(3, 3), placer.LastHoveredTile);
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
            template.cameraBounds = new CameraBounds { tileRect = new RectInt(0, 0, 8, 8) };
            template.props = new List<PropPlacementData>();
            created.Add(template);
            return template;
        }
    }
}
#endif
