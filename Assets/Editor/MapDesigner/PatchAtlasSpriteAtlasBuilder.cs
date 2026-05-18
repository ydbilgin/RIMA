#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using RIMA.MapDesigner.SO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace RIMA.MapDesigner.Brush.Editor
{
    public static class PatchAtlasSpriteAtlasBuilder
    {
        [MenuItem("RIMA/MapDesigner/Build SpriteAtlas from PatchAtlas")]
        public static void BuildSelectedOrAll()
        {
            PatchAtlasSO[] atlases = GetSelectedAtlases();
            if (atlases.Length == 0)
            {
                atlases = FindAllAtlases();
            }

            int built = 0;
            for (int i = 0; i < atlases.Length; i++)
            {
                if (Build(atlases[i]) != null)
                {
                    built++;
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[MapDesigner] Built or verified " + built + " SpriteAtlas asset(s) from PatchAtlasSO.");
        }

        public static SpriteAtlas Build(PatchAtlasSO patchAtlas)
        {
            if (patchAtlas == null || patchAtlas.variants == null || patchAtlas.variants.Length == 0)
            {
                return null;
            }

            string atlasPath = AssetDatabase.GetAssetPath(patchAtlas);
            string folder = string.IsNullOrEmpty(atlasPath) ? "Assets" : Path.GetDirectoryName(atlasPath).Replace("\\", "/");
            string assetName = !string.IsNullOrEmpty(patchAtlas.atlasId) ? patchAtlas.atlasId : patchAtlas.name;
            string spriteAtlasPath = folder + "/" + assetName + "_SpriteAtlas.spriteatlas";

            SpriteAtlas spriteAtlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(spriteAtlasPath);
            if (spriteAtlas == null)
            {
                spriteAtlas = new SpriteAtlas();
                spriteAtlasPath = AssetDatabase.GenerateUniqueAssetPath(spriteAtlasPath);
                AssetDatabase.CreateAsset(spriteAtlas, spriteAtlasPath);
            }

            var sprites = new List<Object>();
            for (int i = 0; i < patchAtlas.variants.Length; i++)
            {
                if (patchAtlas.variants[i] != null)
                {
                    sprites.Add(patchAtlas.variants[i]);
                }
            }

            if (sprites.Count > 0)
            {
                SpriteAtlasExtensions.Add(spriteAtlas, sprites.ToArray());
                EditorUtility.SetDirty(spriteAtlas);
            }

            return spriteAtlas;
        }

        private static PatchAtlasSO[] GetSelectedAtlases()
        {
            var atlases = new List<PatchAtlasSO>();
            Object[] selected = Selection.objects;
            for (int i = 0; i < selected.Length; i++)
            {
                PatchAtlasSO atlas = selected[i] as PatchAtlasSO;
                if (atlas != null)
                {
                    atlases.Add(atlas);
                }
            }

            return atlases.ToArray();
        }

        private static PatchAtlasSO[] FindAllAtlases()
        {
            var atlases = new List<PatchAtlasSO>();
            string[] guids = AssetDatabase.FindAssets("t:PatchAtlasSO");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                PatchAtlasSO atlas = AssetDatabase.LoadAssetAtPath<PatchAtlasSO>(path);
                if (atlas != null)
                {
                    atlases.Add(atlas);
                }
            }

            return atlases.ToArray();
        }
    }
}
#endif
