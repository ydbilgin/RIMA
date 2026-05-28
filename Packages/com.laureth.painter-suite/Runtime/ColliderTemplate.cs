using System.Collections.Generic;
using UnityEngine;

namespace LaurethStudio.PainterSuite.Runtime
{
    public enum ShapeKind { Box, Circle, Polygon, Edge }

    /// <summary>
    /// Serialized shape data, copyable across GameObjects and scenes.
    /// Apply via PainterSuite Editor "Apply Template" or programmatically via
    /// ColliderTemplate.ApplyTo(GameObject).
    /// </summary>
    [System.Serializable]
    public sealed class ColliderShape
    {
        public ShapeKind kind = ShapeKind.Box;

        // Box / Circle / Polygon / Edge shared
        public Vector2 offset;
        public bool isTrigger;

        // Box
        public Vector2 boxSize = Vector2.one;

        // Circle
        public float circleRadius = 0.5f;

        // Polygon (single path) / Edge
        public Vector2[] points;
    }

    /// <summary>
    /// Reusable collider template -- 1..N shapes per template.
    /// Drop on GameObject (drag-drop in Painter window template panel) to apply all shapes.
    /// </summary>
    [CreateAssetMenu(menuName = "LaurethStudio/Painter Suite/Collider Template", fileName = "ColliderTemplate")]
    public sealed class ColliderTemplate : ScriptableObject
    {
        [Tooltip("Display name shown in the template library panel.")]
        public string templateName = "New Template";

        [Tooltip("Optional preview thumbnail.")]
        public Texture2D thumbnail;

        [Tooltip("Shape stack. Applied in order; each becomes a separate Collider2D component on target.")]
        public List<ColliderShape> shapes = new List<ColliderShape>();
    }
}
