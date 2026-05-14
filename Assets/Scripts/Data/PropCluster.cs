using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Data
{
    [CreateAssetMenu(fileName = "PropCluster", menuName = "RIMA/Map/Prop Cluster")]
    public class PropCluster : ScriptableObject
    {
        public List<GameObject> propPrefabs = new List<GameObject>();
        public Vector2Int clusterSize = new Vector2Int(2, 2);
        public int minCount = 1;
        public int maxCount = 3;
        public float spacingMin = 1f;
    }
}
