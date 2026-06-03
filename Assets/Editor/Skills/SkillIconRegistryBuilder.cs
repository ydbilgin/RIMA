using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    public static class SkillIconRegistryBuilder
    {
        private const string IconFolder = "Assets/Sprites/UI/Icons";
        private const string ResourceFolder = "Assets/Resources";
        private const string RegistryPath = "Assets/Resources/SkillIconRegistry.asset";

        private static readonly string[] ClassPrefixes =
        {
            "Warblade_",
            "Elementalist_",
            "Shadowblade_",
            "Ranger_"
        };

        private static readonly string[] WarbladeSkillNames =
        {
            "Iron Charge",
            "Cleave",
            "Deep Wound",
            "Sunder Mark",
            "Crippling Blow",
            "Earthsplitter",
            "Blade Rush",
            "Gravity Cleave",
            "Iron Counter",
            "Iron Crush",
            "Ironclade Momentum",
            "Blood Drinker",
            "Wrath Protocol",
            "Tempered Fury",
            "Berserker's Resolve",
            "Battle Surge",
            "Death Blow"
        };

        [MenuItem("RIMA/Utilities/Rebuild Skill Icon Registry")]
        public static void Rebuild()
        {
            EnsureResourcesFolder();

            SkillIconRegistry registry = AssetDatabase.LoadAssetAtPath<SkillIconRegistry>(RegistryPath);
            if (registry == null)
            {
                registry = ScriptableObject.CreateInstance<SkillIconRegistry>();
                AssetDatabase.CreateAsset(registry, RegistryPath);
            }

            registry.entries ??= new List<SkillIconRegistry.Entry>();
            registry.entries.Clear();

            var seen = new HashSet<string>();
            foreach (string guid in AssetDatabase.FindAssets("t:Texture2D", new[] { IconFolder }))
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (!string.Equals(Path.GetExtension(path), ".png", StringComparison.OrdinalIgnoreCase)) continue;

                Sprite sprite = LoadSprite(path);
                if (sprite == null)
                {
                    Debug.LogWarning($"[SkillIconRegistryBuilder] Sprite load failed: {path}");
                    continue;
                }

                string key = KeyFromPath(path);
                if (string.IsNullOrEmpty(key) || !seen.Add(key))
                {
                    Debug.LogWarning($"[SkillIconRegistryBuilder] Duplicate or empty icon key skipped: {path} -> {key}");
                    continue;
                }

                registry.entries.Add(new SkillIconRegistry.Entry { key = key, sprite = sprite });
            }

            registry.entries.Sort((a, b) => string.CompareOrdinal(a.key, b.key));
            EditorUtility.SetDirty(registry);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            ReportWarbladeMatches(registry);
            Debug.Log($"[SkillIconRegistryBuilder] Rebuilt {RegistryPath} with {registry.entries.Count} icons.");
        }

        private static void EnsureResourcesFolder()
        {
            if (!AssetDatabase.IsValidFolder(ResourceFolder))
                AssetDatabase.CreateFolder("Assets", "Resources");
        }

        private static Sprite LoadSprite(string path)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null) return sprite;

            foreach (UnityEngine.Object asset in AssetDatabase.LoadAllAssetsAtPath(path))
            {
                if (asset is Sprite subSprite) return subSprite;
            }

            return null;
        }

        private static string KeyFromPath(string path)
        {
            string name = Path.GetFileNameWithoutExtension(path);
            if (name.StartsWith("Icon_", StringComparison.OrdinalIgnoreCase))
                name = name.Substring("Icon_".Length);

            foreach (string prefix in ClassPrefixes)
            {
                if (!name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) continue;
                name = name.Substring(prefix.Length);
                break;
            }

            return SkillIconRegistry.Normalize(name);
        }

        private static void ReportWarbladeMatches(SkillIconRegistry registry)
        {
            int matched = 0;
            var missing = new List<string>();

            foreach (string skillName in WarbladeSkillNames)
            {
                if (registry.Get(skillName) != null) matched++;
                else missing.Add(skillName);
            }

            Debug.Log($"[SkillIconRegistryBuilder] Warblade icon matches: {matched}/{WarbladeSkillNames.Length}");
            if (missing.Count > 0)
                Debug.LogWarning($"[SkillIconRegistryBuilder] Missing Warblade icons: {string.Join(", ", missing)}");
        }
    }
}
