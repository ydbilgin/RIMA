#if UNITY_EDITOR
using System;
using RIMA.MapDesigner.Composition;
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Props.Editor
{
    public sealed class PropPlacer
    {
        public bool IsActive { get; set; }
        public PropFootprintValidator.ValidationResult LastValidationResult { get; private set; }
        public string LastFailureDetail { get; private set; } = string.Empty;
        public Vector2Int LastHoveredTile { get; private set; }
        public bool HasHover { get; private set; }
        public int CurrentRotation { get; private set; }

        public void RotateClockwise()
        {
            CurrentRotation = (CurrentRotation + 1) & 3;
        }

        public void ResetRotation()
        {
            CurrentRotation = 0;
        }

        public void OnSceneClick(
            Vector2Int tilePos,
            PropDefinitionSO selectedProp,
            RoomTemplateSO targetTemplate,
            CompositionRoleMap roleMap)
        {
            PropFootprintValidator.ValidationResult result = PropFootprintValidator.Validate(
                selectedProp,
                tilePos,
                CurrentRotation,
                targetTemplate,
                roleMap,
                targetTemplate != null ? targetTemplate.props : null,
                out string detail);

            LastValidationResult = result;
            LastFailureDetail = detail;
            LastHoveredTile = tilePos;
            HasHover = true;

            if (result != PropFootprintValidator.ValidationResult.Valid || targetTemplate == null || selectedProp == null)
            {
                return;
            }

            if (targetTemplate.props == null)
            {
                targetTemplate.props = new System.Collections.Generic.List<PropPlacementData>();
            }

            Undo.RecordObject(targetTemplate, "Place Prop");
            int variantIdx = PickVariantIndex(selectedProp, tilePos);
            var placement = new PropPlacementData(ResolveGuid(selectedProp), tilePos)
            {
                rotationSteps = CurrentRotation,
                placedByUser = global::System.Environment.UserName ?? string.Empty,
                variantIndex = variantIdx
            };
            targetTemplate.props.Add(placement);
            EditorUtility.SetDirty(targetTemplate);
        }

        public void OnHover(
            Vector2Int tilePos,
            PropDefinitionSO selectedProp,
            RoomTemplateSO targetTemplate,
            CompositionRoleMap roleMap)
        {
            LastValidationResult = PropFootprintValidator.Validate(
                selectedProp,
                tilePos,
                CurrentRotation,
                targetTemplate,
                roleMap,
                targetTemplate != null ? targetTemplate.props : null,
                out string detail);
            LastFailureDetail = detail;
            LastHoveredTile = tilePos;
            HasHover = true;
        }

        public void DrawPreview(
            Vector2Int tilePos,
            PropDefinitionSO selectedProp,
            bool isValid,
            Color validColor,
            Color invalidColor)
        {
            if (!IsActive || selectedProp == null) return;

            Vector2Int size = PropFootprintValidator.GetRotatedFootprint(selectedProp, CurrentRotation);
            if (size.x <= 0 || size.y <= 0) size = Vector2Int.one;

            Color fill = isValid ? validColor : invalidColor;
            fill.a *= 0.18f;
            Color outline = isValid ? validColor : invalidColor;
            outline.a = 0.95f;

            var rect = new Rect(tilePos.x, tilePos.y, size.x, size.y);
            Vector3[] corners =
            {
                new Vector3(rect.xMin, rect.yMin, 0f),
                new Vector3(rect.xMax, rect.yMin, 0f),
                new Vector3(rect.xMax, rect.yMax, 0f),
                new Vector3(rect.xMin, rect.yMax, 0f)
            };
            Handles.DrawSolidRectangleWithOutline(corners, fill, outline);
        }

        private static string ResolveGuid(PropDefinitionSO propDef)
        {
            if (propDef == null) return string.Empty;
            string path = AssetDatabase.GetAssetPath(propDef);
            string guid = string.IsNullOrEmpty(path) ? string.Empty : AssetDatabase.AssetPathToGUID(path);
            if (!string.IsNullOrEmpty(guid)) return guid;
            return string.IsNullOrEmpty(propDef.propId) ? propDef.name : propDef.propId;
        }

        public static int PickVariantIndex(PropDefinitionSO propDef, Vector2Int tilePos)
        {
            return propDef != null ? propDef.PickVariantIndexForTile(tilePos) : -1;
        }
    }
}
#endif
