using UnityEngine;

namespace RIMA
{
    [CreateAssetMenu(fileName = "WeaponDatabase", menuName = "RIMA/Weapon Database")]
    public class WeaponDatabaseSO : ScriptableObject
    {
        [System.Serializable]
        public class WeaponEntry
        {
            public string classId;          // e.g. "Warblade"
            public string formId;           // e.g. "Base", "T2_Rift" (Karar #124 Faz 2)
            public GameObject weaponPrefab; // instantiated at runtime, parented to HandAnchor
            public Vector3 anchorOffset;    // HandAnchor.localPosition (Level 1 static)
            public Vector3 gripOffset;      // Level 2 world correction from hand anchor to weapon pivot
            public bool twoHanded;
            public bool orientBetweenHands;
            public float orientationOffsetDegrees;
            public Vector2[] handOffsets = new Vector2[8];
        }

        public WeaponEntry[] entries;

        public WeaponEntry GetWeapon(string classId, string formId = "Base")
        {
            foreach (var e in entries)
                if (e.classId == classId && e.formId == formId) return e;
            return null;
        }
    }
}

