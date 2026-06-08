using UnityEngine;

namespace RIMA
{
    public static class ClassUnlockPolicy
    {
        public const string UnlockPrefsPrefix = "rima_class_unlocked_";

        public static bool IsUnlocked(ClassType cls)
        {
            return cls == ClassType.Warblade ||
                   cls == ClassType.Elementalist ||
                   PlayerPrefs.GetInt(UnlockPrefKey(cls), 0) == 1;
        }

        public static int UnlockCost(ClassType cls)
        {
            return cls switch
            {
                ClassType.Ronin => 150,
                ClassType.Ravager => 150,
                ClassType.Gunslinger => 200,
                ClassType.Brawler => 200,
                ClassType.Summoner => 200,
                ClassType.Hexer => 250,
                _ => 0
            };
        }

        public static bool CanUnlockWithEcho(ClassType cls)
        {
            int cost = UnlockCost(cls);
            return !IsUnlocked(cls) && cost > 0 && EchoWallet.Balance >= cost;
        }

        public static bool TryUnlockWithEcho(ClassType cls)
        {
            if (!CanUnlockWithEcho(cls))
            {
                return false;
            }

            if (!EchoWallet.TrySpend(UnlockCost(cls)))
            {
                return false;
            }

            PlayerPrefs.SetInt(UnlockPrefKey(cls), 1);
            PlayerPrefs.Save();
            return true;
        }

        public static string UnlockPrefKey(ClassType cls) => UnlockPrefsPrefix + cls;
    }
}
