using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.SO
{
    [CreateAssetMenu(fileName = "AssetPackManifest", menuName = "RIMA/MapDesigner/Asset Pack Manifest")]
    public sealed class AssetPackManifestSO : ScriptableObject
    {
        public string packId;
        public string displayName;
        public List<PatchAtlasSO> atlases = new List<PatchAtlasSO>();
        public List<PropDefinitionSO> props = new List<PropDefinitionSO>();
        public List<AssetPackCategory> categories = new List<AssetPackCategory>();
    }

    [Serializable]
    public struct AssetPackCategory
    {
        public string categoryName;
        public List<string> atlasNames;
        public Sprite categoryIcon;
    }
}
