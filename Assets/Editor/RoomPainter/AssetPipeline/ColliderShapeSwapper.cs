using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    /// <summary>
    /// D4: Swaps Collider2D component type on a prefab root while preserving size/offset/isTrigger.
    /// Supports Box, Circle, Capsule. Polygon is deferred to Phase 2.
    /// </summary>
    internal static class ColliderShapeSwapper
    {
        /// <summary>
        /// Replaces the first Collider2D on <paramref name="root"/> with one matching
        /// <paramref name="newShape"/>. Existing size / offset / isTrigger are migrated
        /// best-fit. Returns the new component, or null if root is null.
        /// </summary>
        public static Collider2D SwapShape(GameObject root, ColliderShape newShape)
        {
            if (root == null)
            {
                return null;
            }

            Collider2D existing = root.GetComponent<Collider2D>();

            // Capture current values for migration
            bool isTrigger = false;
            Vector2 offset = Vector2.zero;
            Vector2 size = new Vector2(1f, 1f);
            float radius = 0.5f;

            if (existing != null)
            {
                isTrigger = existing.isTrigger;
                offset = existing.offset;

                if (existing is BoxCollider2D box)
                {
                    size = box.size;
                    radius = Mathf.Max(size.x, size.y) * 0.5f;
                }
                else if (existing is CircleCollider2D circle)
                {
                    radius = circle.radius;
                    size = new Vector2(radius * 2f, radius * 2f);
                }
                else if (existing is CapsuleCollider2D capsule)
                {
                    size = capsule.size;
                    radius = Mathf.Max(size.x, size.y) * 0.5f;
                }

                Undo.DestroyObjectImmediate(existing);
            }

            Collider2D added = null;

            switch (newShape)
            {
                case ColliderShape.Circle:
                {
                    CircleCollider2D c = Undo.AddComponent<CircleCollider2D>(root);
                    c.isTrigger = isTrigger;
                    c.offset = offset;
                    // Conservative fallback if radius is near-zero
                    c.radius = radius > 0.001f ? radius : 0.5f;
                    added = c;
                    break;
                }

                case ColliderShape.Capsule:
                {
                    CapsuleCollider2D c = Undo.AddComponent<CapsuleCollider2D>(root);
                    c.isTrigger = isTrigger;
                    c.offset = offset;
                    c.direction = CapsuleDirection2D.Vertical;
                    // Conservative fallback
                    c.size = (size.x > 0.001f && size.y > 0.001f) ? size : new Vector2(1f, 1.5f);
                    added = c;
                    break;
                }

                default: // Box
                {
                    BoxCollider2D c = Undo.AddComponent<BoxCollider2D>(root);
                    c.isTrigger = isTrigger;
                    c.offset = offset;
                    c.size = (size.x > 0.001f && size.y > 0.001f) ? size : Vector2.one;
                    added = c;
                    break;
                }
            }

            if (added != null)
            {
                EditorUtility.SetDirty(root);
            }

            return added;
        }
    }
}
