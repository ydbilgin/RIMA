using System;

namespace RIMA.RoomDesigner.Core
{
    public static class SeedPipeline
    {
        public static int DeriveSubSeed(int masterSeed, string domain)
        {
            unchecked
            {
                int hash = 216613626;
                string key = domain ?? string.Empty;
                for (int i = 0; i < key.Length; i++)
                {
                    hash ^= key[i];
                    hash *= 16777619;
                }

                hash ^= masterSeed;
                hash *= 16777619;
                return hash == int.MinValue ? Math.Abs(hash + 1) : Math.Abs(hash);
            }
        }
    }
}
