#if UNITY_EDITOR
using System.IO;
using RIMA.MapDesigner.Brush.Data;
using UnityEditor;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Editor
{
    public static class SliceTemplateFactory
    {
        private const string Folder = "Assets/Data/Brush/SliceTemplates";

        [MenuItem("RIMA/Legacy/Brush/Create Default Slice Templates")]
        public static void CreateAll()
        {
            EnsureFolder();
            CreateWang16();
            CreateOrganic512();
            CreateDetail512();
            CreateAccent512();
            CreateRadiusProfileDefault();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("RIMA: Default slice templates + radius profile created at " + Folder);
        }

        private static void EnsureFolder()
        {
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
                AssetDatabase.Refresh();
            }
        }

        private static void CreateWang16()
        {
            string path = $"{Folder}/L3_Wang16_Topdown.asset";
            if (AssetDatabase.LoadAssetAtPath<SliceLayoutTemplateSO>(path) != null) return;
            var t = ScriptableObject.CreateInstance<SliceLayoutTemplateSO>();
            t.templateName = "L3_Wang16_Topdown";
            t.masterSize = Vector2Int.zero;
            t.gutterSize = 1;
            t.defaultPivot = new Vector2(0.5f, 0.5f);
            t.wangAware = true;
            AssetDatabase.CreateAsset(t, path);
        }

        private static void CreateOrganic512()
        {
            string path = $"{Folder}/L4_Organic_512.asset";
            if (AssetDatabase.LoadAssetAtPath<SliceLayoutTemplateSO>(path) != null) return;
            var t = ScriptableObject.CreateInstance<SliceLayoutTemplateSO>();
            t.templateName = "L4_Organic_512";
            t.masterSize = new Vector2Int(512, 512);
            t.gutterSize = 16;
            t.defaultPivot = new Vector2(0.5f, 0.5f);
            t.wangAware = false;

            t.cells.Add(MakeCell("patch_hero", new RectInt(0, 0, 256, 256), SizeBucket.Hero, true, "patch_hero"));
            for (int i = 0; i < 4; i++)
            {
                int x = 256 + (i % 2) * 128;
                int y = (i / 2) * 128;
                t.cells.Add(MakeCell($"patch_medium_{i + 1}", new RectInt(x, y, 128, 128), SizeBucket.Medium, false, "patch_medium"));
            }
            for (int i = 0; i < 8; i++)
            {
                int x = (i % 4) * 64;
                int y = 256 + (i / 4) * 64;
                t.cells.Add(MakeCell($"patch_small_{i + 1}", new RectInt(x, y, 64, 64), SizeBucket.Small, false, "patch_small"));
            }
            for (int i = 0; i < 4; i++)
            {
                int x = 256 + (i % 4) * 32;
                int y = 384;
                t.cells.Add(MakeCell($"patch_micro_{i + 1}", new RectInt(x, y, 32, 32), SizeBucket.Micro, false, "patch_micro"));
            }
            AssetDatabase.CreateAsset(t, path);
        }

        private static void CreateDetail512()
        {
            string path = $"{Folder}/L5_Detail_512.asset";
            if (AssetDatabase.LoadAssetAtPath<SliceLayoutTemplateSO>(path) != null) return;
            var t = ScriptableObject.CreateInstance<SliceLayoutTemplateSO>();
            t.templateName = "L5_Detail_512";
            t.masterSize = new Vector2Int(512, 512);
            t.gutterSize = 8;
            t.defaultPivot = new Vector2(0.5f, 0.5f);
            t.wangAware = false;

            for (int i = 0; i < 4; i++)
            {
                int x = (i % 4) * 128;
                t.cells.Add(MakeCell($"detail_medium_{i + 1}", new RectInt(x, 0, 128, 128), SizeBucket.Medium, false, "detail_medium"));
            }
            for (int i = 0; i < 12; i++)
            {
                int x = (i % 4) * 64;
                int y = 128 + (i / 4) * 64;
                t.cells.Add(MakeCell($"detail_small_{i + 1}", new RectInt(x, y, 64, 64), SizeBucket.Small, false, "detail_small"));
            }
            for (int i = 0; i < 16; i++)
            {
                int x = (i % 8) * 32;
                int y = 320 + (i / 8) * 32;
                t.cells.Add(MakeCell($"detail_micro_{i + 1}", new RectInt(x, y, 32, 32), SizeBucket.Micro, false, "detail_micro"));
            }
            AssetDatabase.CreateAsset(t, path);
        }

        private static void CreateAccent512()
        {
            string path = $"{Folder}/L6_Accent_512.asset";
            if (AssetDatabase.LoadAssetAtPath<SliceLayoutTemplateSO>(path) != null) return;
            var t = ScriptableObject.CreateInstance<SliceLayoutTemplateSO>();
            t.templateName = "L6_Accent_512";
            t.masterSize = new Vector2Int(512, 512);
            t.gutterSize = 24;
            t.defaultPivot = new Vector2(0.5f, 0.5f);
            t.wangAware = false;

            t.cells.Add(MakeCell("rift_hero", new RectInt(96, 96, 320, 320), SizeBucket.Hero, true, "rift_hero"));
            t.cells.Add(MakeCell("rift_medium_1", new RectInt(0, 0, 96, 96), SizeBucket.Medium, false, "rift_medium"));
            t.cells.Add(MakeCell("rift_medium_2", new RectInt(416, 0, 96, 96), SizeBucket.Medium, false, "rift_medium"));
            t.cells.Add(MakeCell("rift_medium_3", new RectInt(0, 416, 96, 96), SizeBucket.Medium, false, "rift_medium"));
            t.cells.Add(MakeCell("rift_medium_4", new RectInt(416, 416, 96, 96), SizeBucket.Medium, false, "rift_medium"));
            for (int i = 0; i < 6; i++)
            {
                int x = (i % 3) * 96 + 96;
                int y = (i / 3) * 416 + 48;
                t.cells.Add(MakeCell($"rift_spark_{i + 1}", new RectInt(x, y, 48, 48), SizeBucket.Small, false, "rift_spark"));
            }
            AssetDatabase.CreateAsset(t, path);
        }

        private static void CreateRadiusProfileDefault()
        {
            string path = $"{Folder}/BrushRadiusProfile_Default.asset";
            if (AssetDatabase.LoadAssetAtPath<BrushRadiusProfileSO>(path) != null) return;
            var p = ScriptableObject.CreateInstance<BrushRadiusProfileSO>();
            p.PopulateDefaultSoftOverlap();
            AssetDatabase.CreateAsset(p, path);
        }

        private static SliceCell MakeCell(string name, RectInt rect, SizeBucket bucket, bool heroAllowed, params string[] tags)
        {
            return new SliceCell
            {
                cellName = name,
                rect = rect,
                bucket = bucket,
                tags = tags,
                usePivotOverride = false,
                heroAllowed = heroAllowed
            };
        }
    }
}
#endif
