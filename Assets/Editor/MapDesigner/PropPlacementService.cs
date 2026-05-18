#if UNITY_EDITOR
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Editor
{
    public static class PropPlacementService
    {
        public static GameObject PlaceEntry(AssetPackEntry entry, Transform parent, Vector3 worldPos, bool registerUndo = true)
        {
            if (entry == null || entry.sprite == null || parent == null)
            {
                return null;
            }

            string objectName = string.IsNullOrEmpty(entry.displayName) ? "PlacedSprite" : entry.displayName;
            GameObject placedObject = CreatePlacedObject(objectName, parent, worldPos, registerUndo, "Place Asset Pack Sprite");

            SpriteRenderer renderer = placedObject.AddComponent<SpriteRenderer>();
            renderer.sprite = entry.sprite;
            renderer.sortingOrder = entry.defaultSortingOrder;

            if (entry.collisionPreset.blocksMovement && entry.collisionPreset.colliderShape != ColliderShape.None)
            {
                AssetPackBrowserWindow.AttachAutoCollider(placedObject, entry.sprite, entry.collisionPreset);
            }

            return placedObject;
        }

        public static GameObject PlacePropDefinition(PropDefinitionSO prop, Transform parent, Vector3 worldPos, string namePrefix, bool registerUndo = true)
        {
            if (prop == null || prop.visual == null || parent == null)
            {
                return null;
            }

            string objectName = string.IsNullOrEmpty(namePrefix)
                ? (string.IsNullOrEmpty(prop.propId) ? prop.name : prop.propId)
                : namePrefix;
            GameObject placedObject = CreatePlacedObject(objectName, parent, worldPos, registerUndo, "Place Blueprint Prop");

            SpriteRenderer renderer = placedObject.AddComponent<SpriteRenderer>();
            renderer.sprite = prop.visual;
            renderer.sortingOrder = (prop.blocksMovement || prop.hasCollision) ? 30 : 0;

            CollisionPreset collisionPreset = CollisionPreset.ForProp(prop, "VerticalProps");
            if (collisionPreset.blocksMovement && collisionPreset.colliderShape != ColliderShape.None)
            {
                AssetPackBrowserWindow.AttachAutoCollider(placedObject, prop.visual, collisionPreset);
            }

            return placedObject;
        }

        private static GameObject CreatePlacedObject(string objectName, Transform parent, Vector3 worldPos, bool registerUndo, string undoName)
        {
            var placedObject = new GameObject(objectName);
            if (registerUndo)
            {
                Undo.RegisterCreatedObjectUndo(placedObject, undoName);
            }

            placedObject.transform.SetParent(parent, true);
            placedObject.transform.position = worldPos;
            return placedObject;
        }
    }
}
#endif
