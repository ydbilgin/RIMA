#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using RIMA.MapDesigner.Brush.Data;

namespace RIMA.MapDesigner.Brush.Render.Editor
{
    public static class BiomeSkinApplier
    {
        private static BiomeSkinSO lastApplied;

        public static BiomeSkinSO LastApplied => lastApplied;

        public static int Apply(BiomeSkinSO skin)
        {
            if (skin == null) return 0;
            if (skin.layerRenderRules == null) return 0;

            Undo.IncrementCurrentGroup();
            int group = Undo.GetCurrentGroup();
            Undo.SetCurrentGroupName("Apply BiomeSkin: " + (skin.skinName ?? "Unnamed"));

            int touched = 0;
            try
            {
                foreach (var rule in skin.layerRenderRules)
                {
                    if (rule == null) continue;
                    touched += ApplyRuleToLayer(rule, skin);
                }
            }
            finally
            {
                Undo.CollapseUndoOperations(group);
            }

            lastApplied = skin;
            return touched;
        }

        private static int ApplyRuleToLayer(LayerRenderRule rule, BiomeSkinSO skin)
        {
            var container = GameObject.Find("Layer_" + rule.layer);
            if (container == null) return 0;

            Material mat = rule.overrideMaterial != null
                ? rule.overrideMaterial
                : MaterialCache.GetForAlphaMode(rule.alphaMode);

            int touched = 0;

            // SpriteRenderer children (L3-L6)
            foreach (var renderer in container.GetComponentsInChildren<SpriteRenderer>(true))
            {
                Undo.RecordObject(renderer, "Apply BiomeSkin Renderer");
                renderer.color = rule.tint * skin.globalTint;
                renderer.sortingOrder = rule.sortingOrder;
                if (mat != null)
                {
                    renderer.sharedMaterial = mat;
                }
                touched++;
            }

            // Tilemap (L1/L2)
            if (rule.layer == TargetLayer.L1 || rule.layer == TargetLayer.L2)
            {
                var tilemap = container.GetComponent<Tilemap>();
                if (tilemap == null) tilemap = container.GetComponentInChildren<Tilemap>(true);
                if (tilemap != null)
                {
                    Undo.RecordObject(tilemap, "Apply BiomeSkin Tilemap");
                    tilemap.color = rule.tint * skin.globalTint;
                    touched++;
                }

                var tilemapRenderer = container.GetComponentInChildren<TilemapRenderer>(true);
                if (tilemapRenderer != null)
                {
                    Undo.RecordObject(tilemapRenderer, "Apply BiomeSkin TilemapRenderer");
                    tilemapRenderer.sortingOrder = rule.sortingOrder;
                    touched++;
                }
            }

            return touched;
        }

        internal static class MaterialCache
        {
            private const string HardPath = "Assets/Art/Materials/Sprite_HardDefault.mat";
            private const string Soft8Path = "Assets/Art/Materials/Sprite_SoftAlpha8.mat";
            private const string Soft16Path = "Assets/Art/Materials/Sprite_SoftAlpha16.mat";

            private static Material hard;
            private static Material soft8;
            private static Material soft16;

            public static Material GetForAlphaMode(AlphaMode mode)
            {
                switch (mode)
                {
                    case AlphaMode.Hard:
                        return hard ?? (hard = LoadOrCreate(HardPath, Shader.Find("Sprites/Default"), 0f));
                    case AlphaMode.SoftAlpha8:
                        return soft8 ?? (soft8 = LoadOrCreate(Soft8Path, FindDitherShader(), 0.5f));
                    case AlphaMode.SoftAlpha16:
                        return soft16 ?? (soft16 = LoadOrCreate(Soft16Path, FindDitherShader(), 1.0f));
                    default:
                        return null;
                }
            }

            private static Shader FindDitherShader()
            {
                var s = Shader.Find("RIMA/DitheredSoftEdge");
                return s != null ? s : Shader.Find("Sprites/Default");
            }

            private static Material LoadOrCreate(string path, Shader shader, float ditherSpread)
            {
                var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (mat != null) return mat;
                if (shader == null) return null;

                EnsureFolder(System.IO.Path.GetDirectoryName(path).Replace('\\', '/'));
                mat = new Material(shader);
                if (mat.HasProperty("_DitherSpread"))
                {
                    mat.SetFloat("_DitherSpread", ditherSpread);
                }
                AssetDatabase.CreateAsset(mat, path);
                AssetDatabase.SaveAssets();
                return mat;
            }

            private static void EnsureFolder(string folderPath)
            {
                if (string.IsNullOrEmpty(folderPath)) return;
                if (AssetDatabase.IsValidFolder(folderPath)) return;

                var parts = folderPath.Split('/');
                string current = parts[0];
                for (int i = 1; i < parts.Length; i++)
                {
                    string next = current + "/" + parts[i];
                    if (!AssetDatabase.IsValidFolder(next))
                    {
                        AssetDatabase.CreateFolder(current, parts[i]);
                    }
                    current = next;
                }
            }
        }
    }
}
#endif
