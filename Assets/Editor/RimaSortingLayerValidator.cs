using System;
using UnityEditor;
using UnityEngine;

namespace RIMA.Editor
{
    [InitializeOnLoad]
    public static class RimaSortingLayerValidator
    {
        private const string GroundLayerName = "Ground";
        private const string WallsLayerName = "Walls";
        private const string EntitiesLayerName = "Entities";
        private const string VfxLayerName = "VFX";
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
            changed |= EnsureLayerAfter(sortingLayers, GroundLayerName, "Default");
            changed |= EnsureLayerAfter(sortingLayers, WallsLayerName, GroundLayerName);
            changed |= EnsureLayerAfter(sortingLayers, EntitiesLayerName, WallsLayerName);
            changed |= EnsureLayerAfter(sortingLayers, VfxLayerName, EntitiesLayerName);
            // 2026-05-20 S95: Detail/Accent/Props orphan, atomic cleanup TagManager'dan sildi
            // changed |= EnsureLayerAfter(sortingLayers, "Detail", "Scatter");
            // changed |= EnsureLayerAfter(sortingLayers, "Accent", "Detail");
            // changed |= EnsureLayerAfter(sortingLayers, "Props", "Accent");

            if (!changed)
            {
                return;
            }

            tagManager.ApplyModifiedProperties();
            AssetDatabase.SaveAssets();
            Debug.Log("RIMA: Sorting layers ensured - Default / Ground / Walls / Entities / VFX");
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
