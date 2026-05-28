#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using LaurethStudio.PainterSuite.Runtime;

namespace LaurethStudio.PainterSuite.Editor.Colliders
{
    /// <summary>
    /// Save / apply / browse ColliderTemplate ScriptableObject assets.
    /// </summary>
    public static class ColliderTemplateService
    {
        private const string DefaultDir = "Assets/PainterTemplates";

        public static ColliderTemplate SaveAsTemplate(GameObject src, string name)
        {
            if (src == null) return null;
            var colliders = src.GetComponents<Collider2D>();
            if (colliders.Length == 0)
            {
                EditorUtility.DisplayDialog("Save Template", "Target has no Collider2D components to save.", "OK");
                return null;
            }

            EnsureDir();
            string safe = string.IsNullOrEmpty(name) ? src.name + "_Template" : name;
            string path = AssetDatabase.GenerateUniqueAssetPath($"{DefaultDir}/{safe}.asset");

            var tpl = ScriptableObject.CreateInstance<ColliderTemplate>();
            tpl.templateName = safe;
            foreach (var c in colliders) tpl.shapes.Add(SerializeShape(c));
            AssetDatabase.CreateAsset(tpl, path);
            AssetDatabase.SaveAssets();
            return tpl;
        }

        public static void ApplyTemplate(ColliderTemplate tpl, GameObject target)
        {
            if (tpl == null || target == null) return;
            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName($"Apply Template: {tpl.templateName}");

            foreach (var s in tpl.shapes) AddShape(s, target);

            Undo.CollapseUndoOperations(group);
            EditorUtility.SetDirty(target);
        }

        public static ColliderTemplate[] FindAllTemplates()
        {
            string[] guids = AssetDatabase.FindAssets("t:ColliderTemplate");
            var list = new ColliderTemplate[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                list[i] = AssetDatabase.LoadAssetAtPath<ColliderTemplate>(AssetDatabase.GUIDToAssetPath(guids[i]));
            }
            return list;
        }

        private static void EnsureDir()
        {
            if (!Directory.Exists(DefaultDir))
            {
                Directory.CreateDirectory(DefaultDir);
                AssetDatabase.Refresh();
            }
        }

        private static ColliderShape SerializeShape(Collider2D c)
        {
            var s = new ColliderShape { offset = c.offset, isTrigger = c.isTrigger };
            switch (c)
            {
                case BoxCollider2D b: s.kind = ShapeKind.Box; s.boxSize = b.size; break;
                case CircleCollider2D circ: s.kind = ShapeKind.Circle; s.circleRadius = circ.radius; break;
                case PolygonCollider2D p:
                    s.kind = ShapeKind.Polygon;
                    s.points = p.pathCount > 0 ? p.GetPath(0) : new Vector2[0];
                    break;
                case EdgeCollider2D e:
                    s.kind = ShapeKind.Edge;
                    s.points = e.points;
                    break;
            }
            return s;
        }

        private static void AddShape(ColliderShape s, GameObject target)
        {
            switch (s.kind)
            {
                case ShapeKind.Box:
                    var b = Undo.AddComponent<BoxCollider2D>(target);
                    b.size = s.boxSize; b.offset = s.offset; b.isTrigger = s.isTrigger;
                    break;
                case ShapeKind.Circle:
                    var ci = Undo.AddComponent<CircleCollider2D>(target);
                    ci.radius = s.circleRadius; ci.offset = s.offset; ci.isTrigger = s.isTrigger;
                    break;
                case ShapeKind.Polygon:
                    var p = Undo.AddComponent<PolygonCollider2D>(target);
                    if (s.points != null && s.points.Length >= 3)
                    {
                        p.pathCount = 1;
                        p.SetPath(0, s.points);
                    }
                    p.offset = s.offset; p.isTrigger = s.isTrigger;
                    break;
                case ShapeKind.Edge:
                    var e = Undo.AddComponent<EdgeCollider2D>(target);
                    if (s.points != null && s.points.Length >= 2) e.points = s.points;
                    e.offset = s.offset; e.isTrigger = s.isTrigger;
                    break;
            }
        }
    }
}
#endif
