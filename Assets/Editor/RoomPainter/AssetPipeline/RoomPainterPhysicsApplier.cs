using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public static class RoomPainterPhysicsApplier
    {
        public static void Apply(GameObject target, RoomPainterAsset asset)
        {
            if (target == null || asset == null)
            {
                return;
            }

            if (!asset.isBlock && !asset.isTrigger)
            {
                if (!asset.respectPrefabColliders)
                {
                    RemovePhysics(target);
                }

                EditorUtility.SetDirty(target);
                return;
            }

            Rigidbody2D body = target.GetComponent<Rigidbody2D>();
            if (body == null)
            {
                body = Undo.AddComponent<Rigidbody2D>(target);
            }

            body.bodyType = asset.bodyType;
            EditorUtility.SetDirty(body);

            Collider2D collider = GetOrCreateCollider(target, asset.colliderShape);
            ConfigureCollider(collider, asset);

            int layer = LayerMask.NameToLayer(asset.physicsLayerName);
            if (layer >= 0)
            {
                target.layer = layer;
            }
            else if (!string.IsNullOrEmpty(asset.physicsLayerName))
            {
                Debug.LogWarning("RoomPainter: Physics layer not found: " + asset.physicsLayerName, target);
            }

            EditorUtility.SetDirty(target);
        }

        private static void RemovePhysics(GameObject target)
        {
            Collider2D[] colliders = target.GetComponents<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
            {
                Undo.DestroyObjectImmediate(colliders[i]);
            }

            Rigidbody2D body = target.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                Undo.DestroyObjectImmediate(body);
            }
        }

        private static Collider2D GetOrCreateCollider(GameObject target, ColliderShape shape)
        {
            Collider2D existing = FindCollider(target, shape);
            if (existing != null)
            {
                return existing;
            }

            switch (shape)
            {
                case ColliderShape.Circle:
                    return Undo.AddComponent<CircleCollider2D>(target);
                case ColliderShape.Capsule:
                    return Undo.AddComponent<CapsuleCollider2D>(target);
                case ColliderShape.Polygon:
                    return Undo.AddComponent<PolygonCollider2D>(target);
                default:
                    return Undo.AddComponent<BoxCollider2D>(target);
            }
        }

        private static Collider2D FindCollider(GameObject target, ColliderShape shape)
        {
            switch (shape)
            {
                case ColliderShape.Circle:
                    return target.GetComponent<CircleCollider2D>();
                case ColliderShape.Capsule:
                    return target.GetComponent<CapsuleCollider2D>();
                case ColliderShape.Polygon:
                    return target.GetComponent<PolygonCollider2D>();
                default:
                    return target.GetComponent<BoxCollider2D>();
            }
        }

        private static void ConfigureCollider(Collider2D collider, RoomPainterAsset asset)
        {
            if (collider == null)
            {
                return;
            }

            collider.isTrigger = asset.isTrigger;

            switch (collider)
            {
                case BoxCollider2D box:
                    box.size = ClampSize(asset.colliderSize);
                    break;
                case CircleCollider2D circle:
                    circle.radius = Mathf.Max(0.01f, Mathf.Max(asset.colliderSize.x, asset.colliderSize.y) * 0.5f);
                    break;
                case CapsuleCollider2D capsule:
                    capsule.size = ClampSize(asset.colliderSize);
                    break;
                case PolygonCollider2D polygon:
                    Vector2 size = ClampSize(asset.colliderSize);
                    polygon.pathCount = 1;
                    polygon.SetPath(0, new[]
                    {
                        new Vector2(-size.x * 0.5f, -size.y * 0.5f),
                        new Vector2(-size.x * 0.5f, size.y * 0.5f),
                        new Vector2(size.x * 0.5f, size.y * 0.5f),
                        new Vector2(size.x * 0.5f, -size.y * 0.5f)
                    });
                    break;
            }

            EditorUtility.SetDirty(collider);
        }

        private static Vector2 ClampSize(Vector2 size)
        {
            return new Vector2(Mathf.Max(0.01f, size.x), Mathf.Max(0.01f, size.y));
        }
    }
}
