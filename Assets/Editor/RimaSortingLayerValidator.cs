using System;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    [InitializeOnLoad]
    public static class RimaSortingLayerValidator
    {
        private const string PatchLayerName = "Patch";
        private const string ScatterLayerName = "Scatter";
        private const string DetailLayerName = "Detail";
        private const string AccentLayerName = "Accent";
        private const string PropsLayerName = "Props";
        private const string EntitiesLayerName = "Entities";
        private const string TagManagerPath = "ProjectSettings/TagManager.asset";

        static RimaSortingLayerValidator()
        {
            EnsureSortingLayers();
        }

        public static void EnsureSortingLayers()
        {
            var assets = AssetDatabase.LoadAllAssetsAtPath(TagManagerPath);
            if (assets == null || assets.Length == 0)
            {
                Debug.LogWarning($"RIMA: Could not load {TagManagerPath}; sorting layers were not validated.");
                return;
            }

            var tagManager = new SerializedObject(assets[0]);
            var sortingLayers = tagManager.FindProperty("m_SortingLayers");
            if (sortingLayers == null || !sortingLayers.isArray)
            {
                Debug.LogWarning("RIMA: TagManager sorting layer data was not found.");
                return;
            }

            var changed = false;
            changed |= EnsureLayerAfter(sortingLayers, PatchLayerName, "Default");
            changed |= EnsureLayerAfter(sortingLayers, ScatterLayerName, PatchLayerName);
            changed |= EnsureLayerAfter(sortingLayers, DetailLayerName, ScatterLayerName);
            changed |= EnsureLayerAfter(sortingLayers, AccentLayerName, DetailLayerName);
            changed |= EnsureLayerAfter(sortingLayers, PropsLayerName, AccentLayerName);
            changed |= EnsureLayerAfter(sortingLayers, EntitiesLayerName, PropsLayerName);

            if (!changed)
            {
                return;
            }

            tagManager.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            Debug.Log("RIMA: Sorting layers ensured — Patch / Scatter / Detail / Accent / Props / Entities (Karar #135 + S86 Sprint 9 R2 retrofit)");
        }

        private static bool EnsureLayerAfter(SerializedProperty sortingLayers, string layerName, string previousLayerName)
        {
            if (FindLayerIndex(sortingLayers, layerName) >= 0)
            {
                return false;
            }

            var previousIndex = FindLayerIndex(sortingLayers, previousLayerName);
            var insertIndex = previousIndex >= 0 ? previousIndex + 1 : sortingLayers.arraySize;
            sortingLayers.InsertArrayElementAtIndex(insertIndex);

            var layer = sortingLayers.GetArrayElementAtIndex(insertIndex);
            layer.FindPropertyRelative("name").stringValue = layerName;
            layer.FindPropertyRelative("uniqueID").intValue = CreateUniqueId(sortingLayers, layerName);

            return true;
        }

        private static int FindLayerIndex(SerializedProperty sortingLayers, string layerName)
        {
            for (var i = 0; i < sortingLayers.arraySize; i++)
            {
                var layer = sortingLayers.GetArrayElementAtIndex(i);
                if (layer.FindPropertyRelative("name").stringValue == layerName)
                {
                    return i;
                }
            }

            return -1;
        }

        private static int CreateUniqueId(SerializedProperty sortingLayers, string layerName)
        {
            var candidate = Math.Abs(Animator.StringToHash($"RIMA.SortingLayer.{layerName}"));
            while (candidate == 0 || ContainsUniqueId(sortingLayers, candidate))
            {
                candidate++;
            }

            return candidate;
        }

        private static bool ContainsUniqueId(SerializedProperty sortingLayers, int uniqueId)
        {
            for (var i = 0; i < sortingLayers.arraySize; i++)
            {
                var layer = sortingLayers.GetArrayElementAtIndex(i);
                if (layer.FindPropertyRelative("uniqueID").intValue == uniqueId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
