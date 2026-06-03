using System.IO;
using UnityEditor;
using UnityEngine;

namespace RIMA.RoomPainter.Editor
{
    internal static class RoomThumbnailBaker
    {
        private const int Size = 256;

        public static string Bake(RoomData room, Transform previewRoot)
        {
            if (room == null || previewRoot == null)
            {
                return string.Empty;
            }

            RoomDataAuthoringController.EnsureAssetFolder(RoomDataAuthoringController.ThumbnailsFolder);

            Bounds bounds = CalculateBounds(previewRoot);
            GameObject cameraObject = new GameObject("[RoomThumbnailCamera]");
            cameraObject.hideFlags = HideFlags.HideAndDontSave;
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0f, 0f, 0f, 0f);
            camera.transform.position = new Vector3(bounds.center.x, bounds.center.y, -10f);
            camera.transform.rotation = Quaternion.identity;
            camera.orthographicSize = Mathf.Max(2f, Mathf.Max(bounds.extents.x, bounds.extents.y) * 1.25f);

            RenderTexture previousTarget = RenderTexture.active;
            RenderTexture rt = RenderTexture.GetTemporary(Size, Size, 24, RenderTextureFormat.ARGB32);
            Texture2D texture = new Texture2D(Size, Size, TextureFormat.RGBA32, false);

            string assetPath = RoomDataAuthoringController.ThumbnailsFolder + "/" + room.roomId + ".png";
            try
            {
                camera.targetTexture = rt;
                camera.Render();
                RenderTexture.active = rt;
                texture.ReadPixels(new Rect(0, 0, Size, Size), 0, 0);
                texture.Apply();

                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);
                File.WriteAllBytes(fullPath, texture.EncodeToPNG());
                AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            }
            finally
            {
                camera.targetTexture = null;
                RenderTexture.active = previousTarget;
                RenderTexture.ReleaseTemporary(rt);
                Object.DestroyImmediate(texture);
                Object.DestroyImmediate(cameraObject);
            }

            return assetPath;
        }

        private static Bounds CalculateBounds(Transform root)
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                return new Bounds(Vector3.zero, new Vector3(8f, 8f, 1f));
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            if (bounds.size.x < 1f || bounds.size.y < 1f)
            {
                bounds.Expand(2f);
            }

            return bounds;
        }
    }
}
