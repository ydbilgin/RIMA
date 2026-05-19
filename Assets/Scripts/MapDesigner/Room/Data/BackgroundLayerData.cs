using UnityEngine;

namespace RIMA.MapDesigner.Room.Data
{
    [System.Serializable]
    public class BackgroundLayerData
    {
        public string layerName = "Layer";
        public Sprite sprite;
        [Tooltip("Z-order. Reserved range for painted backgrounds: -200..-50. Gameplay 0..49, foreground overlays 50..199, HUD 200+.")]
        public int sortingOrder = -100;
        public Vector2 offset = Vector2.zero;
        public Vector2 scale = Vector2.one;
        public Color tint = Color.white;
        public bool visible = true;
    }
}
