using UnityEngine;

namespace RIMA.Runtime.Scatter
{
    [System.Serializable]
    public class ScatterItem
    {
        public string category;
        public Sprite[] sprites;
        public float minScale = 0.8f;
        public float maxScale = 1.2f;
        public bool randomRotation = true;
        public int sortingOrder = 5;
    }
}
