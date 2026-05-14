namespace RIMA.Editor.RoomDesigner.Palette
{
    using System;

    public static class BiomeFilter
    {
        public const string All = "all";
        public const string Keep = "keep";
        public const string Crypt = "crypt";
        public const string Volcanic = "volcanic";
        public const string Unknown = "unknown";

        public static string BiomeOf(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return Unknown;
            }

            string normalized = assetPath.Replace('\\', '/');
            if (ContainsPathSegment(normalized, Keep))
            {
                return Keep;
            }

            if (ContainsPathSegment(normalized, Crypt))
            {
                return Crypt;
            }

            if (ContainsPathSegment(normalized, Volcanic))
            {
                return Volcanic;
            }

            return Unknown;
        }

        public static bool Matches(string assetPath, string activeBiome)
        {
            if (string.Equals(activeBiome, All, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return string.Equals(BiomeOf(assetPath), activeBiome, StringComparison.OrdinalIgnoreCase);
        }

        private static bool ContainsPathSegment(string normalizedPath, string segment)
        {
            string needle = "/" + segment + "/";
            return normalizedPath.IndexOf(needle, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
