#if UNITY_EDITOR
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace RIMA.Tests.BuildMode
{
    /// <summary>
    /// TMP garbled-text regression (consolidation item 4): the dynamic LiberationSans SDF - Fallback
    /// must be wired into BOTH the global TMP Settings fallback list AND the Jersey10 per-font
    /// fallback so any missing/Turkish glyph resolves at runtime. Guards against the empty-fallback
    /// state that caused the garbling.
    /// </summary>
    public class TmpFallbackWiredTests
    {
        private const string FallbackPath = "Assets/TextMesh Pro/Resources/Fonts & Materials/LiberationSans SDF - Fallback.asset";
        private const string JerseyPath = "Assets/Fonts/Jersey10/Jersey10-Regular SDF.asset";
        private const string TmpSettingsPath = "Assets/TextMesh Pro/Resources/TMP Settings.asset";

        [Test]
        public void FallbackFont_IsDynamic()
        {
            var fb = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FallbackPath);
            Assert.IsNotNull(fb, "LiberationSans SDF - Fallback asset must exist.");
            Assert.AreEqual(AtlasPopulationMode.Dynamic, fb.atlasPopulationMode,
                "The fallback must be DYNAMIC so it renders any missing glyph on demand.");
        }

        [Test]
        public void Jersey10_FallbackTable_ContainsDynamicFallback()
        {
            var fb = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(FallbackPath);
            var jersey = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(JerseyPath);
            Assert.IsNotNull(jersey, "Jersey10 SDF asset must exist.");
            Assert.IsNotNull(jersey.fallbackFontAssetTable);
            Assert.Greater(jersey.fallbackFontAssetTable.Count, 0,
                "Jersey10 per-font fallback must be non-empty (it was [] = garbled glyphs).");
            Assert.Contains(fb, jersey.fallbackFontAssetTable,
                "Jersey10 fallback must include the dynamic LiberationSans fallback.");
        }

        [Test]
        public void TmpSettings_GlobalFallback_ContainsDynamicFallback()
        {
            var fb = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(FallbackPath);
            var tmp = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(TmpSettingsPath);
            Assert.IsNotNull(tmp, "TMP Settings asset must exist.");

            var so = new SerializedObject(tmp);
            var list = so.FindProperty("m_fallbackFontAssets");
            Assert.IsNotNull(list, "m_fallbackFontAssets serialized field must exist.");
            Assert.Greater(list.arraySize, 0,
                "TMP Settings global fallback must be non-empty (it was [] = garbled glyphs project-wide).");

            bool found = false;
            for (int i = 0; i < list.arraySize; i++)
            {
                if (list.GetArrayElementAtIndex(i).objectReferenceValue == fb) { found = true; break; }
            }
            Assert.IsTrue(found, "TMP Settings fallback must include the dynamic LiberationSans fallback.");
        }
    }
}
#endif
