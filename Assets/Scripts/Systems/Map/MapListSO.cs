using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Systems.Map
{
    [CreateAssetMenu(menuName = "RIMA/Map/Map List")]
    public sealed class MapListSO : ScriptableObject
    {
        public List<string> mapSceneNames = new();
        public string startSceneName = "_IsoGame";
    }
}
