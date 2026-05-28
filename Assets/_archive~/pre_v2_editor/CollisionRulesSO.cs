#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Editor.MapDesigner
{
    [CreateAssetMenu(fileName = "DefaultCollisionRules", menuName = "RIMA/Map Designer/Collision Rules")]
    public class CollisionRulesSO : ScriptableObject
    {
        [Serializable]
        public class Rule
        {
            public string pattern;
            public RimaWorldPainterWindow.CollisionMode mode;
            public Vector2 customSize = Vector2.one;
            public Vector2 customOffset = Vector2.zero;
        }

        public List<Rule> rules = new List<Rule>();

        public bool TryResolve(string prefabName, out Rule matchedRule)
        {
            matchedRule = null;
            if (string.IsNullOrEmpty(prefabName) || rules == null)
            {
                return false;
            }

            string lowerName = prefabName.ToLowerInvariant();
            foreach (Rule rule in rules)
            {
                if (rule == null || string.IsNullOrWhiteSpace(rule.pattern))
                {
                    continue;
                }

                string pattern = rule.pattern.ToLowerInvariant();
                if (Matches(lowerName, pattern))
                {
                    matchedRule = rule;
                    return true;
                }
            }

            return false;
        }

        private static bool Matches(string value, string pattern)
        {
            if (pattern == "*")
            {
                return true;
            }

            if (pattern.StartsWith("*") && pattern.EndsWith("*") && pattern.Length > 2)
            {
                return value.Contains(pattern.Substring(1, pattern.Length - 2));
            }

            if (pattern.StartsWith("*"))
            {
                return value.EndsWith(pattern.Substring(1));
            }

            if (pattern.EndsWith("*"))
            {
                return value.StartsWith(pattern.Substring(0, pattern.Length - 1));
            }

            return value == pattern || value.Contains(pattern);
        }
    }
}
#endif
