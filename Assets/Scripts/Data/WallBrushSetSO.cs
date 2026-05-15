using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Data
{
    [CreateAssetMenu(menuName = "RIMA/Map/Wall Brush Set")]
    public sealed class WallBrushSetSO : ScriptableObject
    {
        public List<Sprite> horizontal = new List<Sprite>();
        public List<Sprite> vertical = new List<Sprite>();
        public List<Sprite> corner = new List<Sprite>();
        public Sprite doorwayGap;
        public string biomeKey;
    }
}
