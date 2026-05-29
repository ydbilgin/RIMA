using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RIMA
{
    [CreateAssetMenu(fileName = "SkillIconRegistry", menuName = "RIMA/Skill Icon Registry")]
    public class SkillIconRegistry : ScriptableObject
    {
        [System.Serializable]
        public class Entry
        {
            public string key;
            public Sprite sprite;
        }

        public List<Entry> entries = new List<Entry>();

        public Sprite Get(string skillName)
        {
            string key = Normalize(skillName);
            if (string.IsNullOrEmpty(key)) return null;

            foreach (Entry entry in entries)
            {
                if (entry == null || entry.sprite == null) continue;
                if (entry.key == key) return entry.sprite;
            }

            return null;
        }

        public static string Normalize(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return string.Empty;

            var sb = new StringBuilder(value.Length);
            foreach (char c in value)
            {
                if (char.IsWhiteSpace(c)) continue;
                sb.Append(char.ToLowerInvariant(c));
            }
            return sb.ToString();
        }
    }
}
