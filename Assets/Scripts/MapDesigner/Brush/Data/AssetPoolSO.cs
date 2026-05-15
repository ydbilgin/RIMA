using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace RIMA.MapDesigner.Brush.Data
{
    [CreateAssetMenu(fileName = "AssetPool_New", menuName = "RIMA/Brush/Asset Pool", order = 100)]
    public class AssetPoolSO : ScriptableObject
    {
        public string poolName;
        public AssetCategory category;
        public List<Sprite> sprites = new List<Sprite>();
        public List<float> spriteWeights = new List<float>();
        public List<TileBase> tiles = new List<TileBase>();
        public List<GameObject> prefabs = new List<GameObject>();
        public Vector2Int nativeSize = new Vector2Int(64, 64);
        public bool supportsRotation = true;
        public bool supportsFlip = true;
        public bool isSoftEdge = false;
    }
}
