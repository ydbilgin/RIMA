#if UNITY_EDITOR
using RIMA.MapDesigner.Room.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Editor
{
    public static class RoomTemplateLoader
    {
        public static LoadResult LoadIntoAuthoringScene(RoomTemplateSO template)
        {
            var result = new LoadResult();
            if (template == null)
            {
                result.message = "RoomTemplateSO is null.";
                return result;
            }
            if (template.prefabRef == null)
            {
                result.message = $"Template '{template.roomId}' has no prefabRef.";
                return result;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(template.prefabRef);
            if (instance == null)
            {
                result.message = $"Failed to instantiate prefab for '{template.roomId}'.";
                return result;
            }

            instance.name = template.roomId;
            result.instance = instance;
            result.success = true;
            result.message = $"Loaded '{template.roomId}' into authoring scene.";
            return result;
        }
    }
}
#endif
