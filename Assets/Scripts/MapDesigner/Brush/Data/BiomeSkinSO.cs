using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Brush.Data
{
    [CreateAssetMenu(fileName = "BiomeSkin_New", menuName = "RIMA/Brush/Biome Skin", order = 103)]
    public class BiomeSkinSO : ScriptableObject
    {
        public string skinName;
        public BrushPackSO defaultBrushPack;
        public List<LayerRenderRule> layerRenderRules = new List<LayerRenderRule>();
        public Color globalTint = Color.white;
        [Range(0f, 1f)] public float ambientLightIntensity = 0.35f;
    }

    [Serializable]
    public class LayerRenderRule
    {
        public TargetLayer layer;
        public AlphaMode alphaMode = AlphaMode.Hard;
        public Color tint = Color.white;
        public Material overrideMaterial;
        public int sortingOrder;
    }
}
