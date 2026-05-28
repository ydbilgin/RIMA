using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    public static class VisualSection
    {
        public static void Draw(RoomPainterAsset asset, GameObject sceneInstance)
        {
            asset.tint = EditorGUILayout.ColorField("Tint", asset.tint);

            Material currentMaterial = string.IsNullOrEmpty(asset.materialOverridePath)
                ? null
                : AssetDatabase.LoadAssetAtPath<Material>(asset.materialOverridePath);
            Material nextMaterial = (Material)EditorGUILayout.ObjectField("Material", currentMaterial, typeof(Material), false);
            asset.materialOverridePath = nextMaterial != null ? AssetDatabase.GetAssetPath(nextMaterial) : string.Empty;

            asset.castShadow = EditorGUILayout.Toggle("Cast Shadow", asset.castShadow);
            asset.receiveLight = EditorGUILayout.Toggle("Receive Light", asset.receiveLight);

            using (new EditorGUI.DisabledScope(sceneInstance == null))
            {
                if (GUILayout.Button("Apply Visual"))
                {
                    ApplyVisual(sceneInstance, asset);
                }
            }
        }

        private static void ApplyVisual(GameObject sceneInstance, RoomPainterAsset asset)
        {
            if (sceneInstance == null || asset == null)
            {
                return;
            }

            Material material = string.IsNullOrEmpty(asset.materialOverridePath)
                ? null
                : AssetDatabase.LoadAssetAtPath<Material>(asset.materialOverridePath);
            SpriteRenderer[] renderers = sceneInstance.GetComponentsInChildren<SpriteRenderer>();
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            block.SetColor("_Color", asset.tint);
            for (int i = 0; i < renderers.Length; i++)
            {
                Undo.RecordObject(renderers[i], "Apply Room Painter Visual");
                if (material != null)
                {
                    renderers[i].sharedMaterial = material;
                }

                renderers[i].SetPropertyBlock(block);
                renderers[i].shadowCastingMode = asset.castShadow
                    ? UnityEngine.Rendering.ShadowCastingMode.On
                    : UnityEngine.Rendering.ShadowCastingMode.Off;
                renderers[i].receiveShadows = asset.receiveLight;
                EditorUtility.SetDirty(renderers[i]);
            }
        }
    }
}
