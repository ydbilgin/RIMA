using UnityEngine;

namespace RIMA
{
    public static class EchoWallet
    {
        public const int DemoStartingEcho = 200;
        public const string EchoBalancePrefsKey = "rima_demo_echo_balance";

        public const int RoomAwardEcho = 3;
        public const int KillsPerAwardEcho = 5;
        public const int MinRunAward = 5;
        public const int MaxRunAward = 60;

        public static int Balance
        {
            get
            {
                EnsureInitialized();
                return PlayerPrefs.GetInt(EchoBalancePrefsKey, DemoStartingEcho);
            }
        }

        public static void EnsureInitialized()
        {
            if (PlayerPrefs.HasKey(EchoBalancePrefsKey)) return;

            PlayerPrefs.SetInt(EchoBalancePrefsKey, DemoStartingEcho);
            PlayerPrefs.Save();
        }

        public static void Add(int amount)
        {
            if (amount <= 0) return;

            PlayerPrefs.SetInt(EchoBalancePrefsKey, Balance + amount);
            PlayerPrefs.Save();
        }

        public static bool TrySpend(int amount)
        {
            if (amount <= 0) return true;

            int balance = Balance;
            if (balance < amount) return false;

            PlayerPrefs.SetInt(EchoBalancePrefsKey, balance - amount);
            PlayerPrefs.Save();
            return true;
        }

        public static int ComputeRunAward(RunStats stats)
        {
            int roomsCleared = stats != null ? stats.RoomsClearedForAward : 0;
            int kills = stats != null ? stats.KillsForAward : 0;
            int award = roomsCleared * RoomAwardEcho + kills / KillsPerAwardEcho;
            return Mathf.Clamp(award, MinRunAward, MaxRunAward);
        }

        public static int AwardRunIfNeeded(RunStats stats)
        {
            if (stats == null)
                stats = RunStats.Instance;

            if (stats.HasEchoAward)
                return stats.EchoAward;

            int award = ComputeRunAward(stats);
            if (!stats.TryMarkEchoAwarded(award))
                return stats.EchoAward;

            Add(award);
            return award;
        }
    }
}
