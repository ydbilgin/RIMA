using System.IO;
using UnityEngine;
using UnityEditor;

namespace RIMA
{
    public static class IsoDummyRenderer
    {
        const int SIZE = 256;
        const float CAM_SIZE = 1.4f;
        const string OUT_DIR = "TASARIM/ISO_DUMMY_REFS";

        static readonly Color C_BODY  = new Color(0.55f, 0.55f, 0.55f);
        static readonly Color C_JOINT = new Color(0.40f, 0.40f, 0.40f);
        static readonly Color C_SWORD = new Color(0.80f, 0.80f, 0.85f);

        [MenuItem("RIMA/Render ISO Dummy")]
        static void RenderAll()
        {
            string absOut = Path.GetFullPath(
                Path.Combine(Application.dataPath, "..", "..", OUT_DIR));
            Directory.CreateDirectory(absOut);

            var root = MakeMannequin();
            var cam  = MakeCamera();

            string[] dirs   = { "S",   "W",   "N",    "E"  };
            float[]  angles = {  0f,   90f,  180f,   270f  };

            for (int i = 0; i < 4; i++)
            {
                root.transform.rotation = Quaternion.Euler(0f, angles[i], 0f);
                SavePNG(cam, Path.Combine(absOut, "Dummy_" + dirs[i] + ".png"));
                Debug.Log("[IsoDummy] Saved: Dummy_" + dirs[i] + ".png");
            }

            Object.DestroyImmediate(cam.gameObject);
            Object.DestroyImmediate(root);
            Debug.Log("[IsoDummy] 4 PNG ready -> " + absOut);
            EditorUtility.RevealInFinder(absOut);
        }

        static GameObject MakeMannequin()
        {
            var r = new GameObject("__IsoDummy__");
            AddBox(r, "Torso",      new Vector3( 0.00f,  0.15f, 0), new Vector3(0.28f, 0.36f, 0.14f), C_BODY);
            AddBox(r, "Head",       new Vector3( 0.00f,  0.56f, 0), new Vector3(0.20f, 0.22f, 0.18f), C_BODY);
            AddBox(r, "ArmL",       new Vector3(-0.24f,  0.18f, 0), new Vector3(0.11f, 0.32f, 0.11f), C_JOINT);
            AddBox(r, "ArmR",       new Vector3( 0.24f,  0.18f, 0), new Vector3(0.11f, 0.32f, 0.11f), C_JOINT);
            AddBox(r, "LegL",       new Vector3(-0.10f, -0.25f, 0), new Vector3(0.12f, 0.30f, 0.12f), C_BODY);
            AddBox(r, "LegR",       new Vector3( 0.10f, -0.25f, 0), new Vector3(0.12f, 0.30f, 0.12f), C_BODY);
            // Kılıç: elin bulunduğu yer = kolun alt ucu (ArmR center 0.18, height 0.32 → alt = 0.02)
            // Guard = el (y=0.02), blade = guard üstünden yukarı uzar
            AddBox(r, "SwordGuard", new Vector3( 0.30f,  0.02f, 0), new Vector3(0.13f, 0.04f, 0.04f), C_JOINT);
            AddBox(r, "Sword",      new Vector3( 0.30f,  0.34f, 0), new Vector3(0.04f, 0.60f, 0.04f), C_SWORD);
            return r;
        }

        static void AddBox(GameObject parent, string label, Vector3 pos, Vector3 scale, Color col)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = label;
            go.transform.SetParent(parent.transform, false);
            go.transform.localPosition = pos;
            go.transform.localScale    = scale;
            Object.DestroyImmediate(go.GetComponent<BoxCollider>());

            var mr  = go.GetComponent<MeshRenderer>();
            var sh  = Shader.Find("Universal Render Pipeline/Lit");
            if (sh == null) sh = Shader.Find("Standard");
            var mat = new Material(sh);
            mat.color = col;
            if (mat.HasProperty("_Metallic"))   mat.SetFloat("_Metallic",   0f);
            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.1f);
            if (mat.HasProperty("_Glossiness")) mat.SetFloat("_Glossiness", 0.1f);
            mr.sharedMaterial    = mat;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mr.receiveShadows    = false;
        }

        static Camera MakeCamera()
        {
            var go  = new GameObject("__IsoRenderCam__");
            var cam = go.AddComponent<Camera>();
            go.transform.position = new Vector3(3f, 3f, -3f);
            go.transform.LookAt(new Vector3(0f, 0.15f, 0f));
            cam.orthographic     = true;
            cam.orthographicSize = CAM_SIZE;
            cam.clearFlags       = CameraClearFlags.SolidColor;
            cam.backgroundColor  = new Color(0, 0, 0, 0);
            cam.cullingMask      = ~0;
            cam.nearClipPlane    = 0.1f;
            cam.farClipPlane     = 30f;
            cam.allowHDR         = false;
            cam.allowMSAA        = false;
            return cam;
        }

        static void SavePNG(Camera cam, string path)
        {
            var rt = new RenderTexture(SIZE, SIZE, 24, RenderTextureFormat.ARGB32);
            cam.targetTexture = rt;
            cam.Render();
            RenderTexture.active = rt;
            var tex = new Texture2D(SIZE, SIZE, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, SIZE, SIZE), 0, 0);
            tex.Apply();
            RenderTexture.active = null;
            cam.targetTexture    = null;
            File.WriteAllBytes(path, tex.EncodeToPNG());
            Object.DestroyImmediate(rt);
            Object.DestroyImmediate(tex);
        }
    }
}
