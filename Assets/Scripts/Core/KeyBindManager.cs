using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA
{
    /// <summary>
    /// Skill tuş atamalarını yönetir. PlayerPrefs ile kalıcı.
    /// Default: Q, E, R, F
    /// </summary>
    public static class KeyBindManager
    {
        private static readonly string[] DefaultKeys = { "q", "e", "r", "f" };
        private static readonly string[] SlotPrefKeys = {
            "SkillKey_0", "SkillKey_1", "SkillKey_2", "SkillKey_3"
        };

        private static readonly string[] DisplayNames = { "Q", "E", "R", "F" };

        /// <summary> Slot index → InputSystem binding path </summary>
        public static string GetBinding(int slot)
        {
            string key = PlayerPrefs.GetString(SlotPrefKeys[slot], DefaultKeys[slot]);
            return $"<Keyboard>/{key}";
        }

        /// <summary> Slot index → Display name (büyük harf) </summary>
        public static string GetKeyName(int slot)
        {
            string key = PlayerPrefs.GetString(SlotPrefKeys[slot], DefaultKeys[slot]);
            return key.ToUpper();
        }

        /// <summary> Slot'a yeni tuş ata </summary>
        public static void SetKey(int slot, string keyName)
        {
            keyName = keyName.ToLower();
            PlayerPrefs.SetString(SlotPrefKeys[slot], keyName);
            PlayerPrefs.Save();
        }

        /// <summary> Varsayılanlara dön </summary>
        public static void ResetToDefaults()
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerPrefs.SetString(SlotPrefKeys[i], DefaultKeys[i]);
            }
            PlayerPrefs.Save();
        }

        /// <summary> Tüm tuş isimlerini al </summary>
        public static string[] GetAllKeyNames()
        {
            var names = new string[4];
            for (int i = 0; i < 4; i++)
                names[i] = GetKeyName(i);
            return names;
        }
    }
}
