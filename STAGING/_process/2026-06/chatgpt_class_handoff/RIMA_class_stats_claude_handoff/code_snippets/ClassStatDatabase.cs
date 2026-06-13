using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Balance
{
    [CreateAssetMenu(menuName = "RIMA/Balance/Class Stat Database", fileName = "ClassStatDatabase")]
    public class ClassStatDatabase : ScriptableObject
    {
        public List<ClassStatProfile> profiles = new();

        public ClassStatProfile GetProfile(ClassType classType)
        {
            foreach (var profile in profiles)
            {
                if (profile != null && profile.classType.Equals(classType))
                    return profile;
            }

            Debug.LogWarning($"No ClassStatProfile found for {classType}.");
            return null;
        }
    }
}
