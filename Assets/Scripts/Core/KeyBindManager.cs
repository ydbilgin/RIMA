using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA
{
    /// <summary>Gameplay actions the binding registry can rebind (CONTROL_SCHEME_SYNTHESIS_S6 §3/§7).</summary>
    public enum GameAction
    {
        MoveUp, MoveDown, MoveLeft, MoveRight,
        Dash, Attack, ClassSecondary, RiftBreak,
        Skill1, Skill2, Skill3, Skill4
    }

    /// <summary>
    /// Single source of truth for keyboard/mouse gameplay bindings (S6 BLOCK C1).
    /// Persists overrides as a single JSON blob in PlayerPrefs. Esc/Tab are reserved
    /// (overlay/pause routing) and cannot be bound; a key already owned by another action
    /// can't be silently stolen. Changing a binding fires <see cref="OnBindingsChanged"/> so
    /// consumers re-create their InputActions (fixes "SetKey wrote prefs but never rebuilt").
    ///
    /// Back-compat: the old slot API (GetBinding(int)/GetKeyName(int)/SetKey(int)/GetAllKeyNames)
    /// still works — slot 0..3 maps to Skill1..Skill4.
    /// </summary>
    public static class KeyBindManager
    {
        private const string PrefKey = "RIMA_Bindings_v2";

        // Default InputSystem binding paths (CONTROL_SCHEME_SYNTHESIS_S6 §2 + §7).
        private static readonly Dictionary<GameAction, string> Defaults = new Dictionary<GameAction, string>
        {
            { GameAction.MoveUp,         "<Keyboard>/w" },
            { GameAction.MoveDown,       "<Keyboard>/s" },
            { GameAction.MoveLeft,       "<Keyboard>/a" },
            { GameAction.MoveRight,      "<Keyboard>/d" },
            { GameAction.Dash,           "<Keyboard>/space" },
            { GameAction.Attack,         "<Mouse>/leftButton" },
            { GameAction.ClassSecondary, "<Mouse>/rightButton" },
            { GameAction.RiftBreak,      "<Keyboard>/v" },
            { GameAction.Skill1,         "<Keyboard>/q" },
            { GameAction.Skill2,         "<Keyboard>/e" },
            { GameAction.Skill3,         "<Keyboard>/r" },
            { GameAction.Skill4,         "<Keyboard>/f" },
        };

        // Reserved control paths — cannot be bound to a gameplay action (overlay/pause routing).
        private static readonly HashSet<string> Reserved = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "<Keyboard>/tab", "<Keyboard>/escape"
        };

        // The 4 bar skills in slot order — backs the legacy slot API.
        private static readonly GameAction[] SkillSlots =
            { GameAction.Skill1, GameAction.Skill2, GameAction.Skill3, GameAction.Skill4 };

        private static Dictionary<GameAction, string> _overrides;

        /// <summary>Raised on any binding change (set/reset/rebuild). Consumers re-create their InputActions.</summary>
        public static event Action OnBindingsChanged;

        // ── Persistence ────────────────────────────────────────────────────────────
        [Serializable] private class BindOverride { public string action; public string path; }
        [Serializable] private class OverrideList { public List<BindOverride> items = new List<BindOverride>(); }

        private static void EnsureLoaded()
        {
            if (_overrides != null) return;
            _overrides = new Dictionary<GameAction, string>();

            string json = PlayerPrefs.GetString(PrefKey, "");
            if (string.IsNullOrEmpty(json)) return;
            try
            {
                OverrideList list = JsonUtility.FromJson<OverrideList>(json);
                if (list?.items == null) return;
                foreach (BindOverride o in list.items)
                {
                    if (o != null && !string.IsNullOrEmpty(o.path) &&
                        Enum.TryParse(o.action, out GameAction a))
                    {
                        _overrides[a] = o.path;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[KeyBindManager] override parse failed, using defaults: {e.Message}");
            }
        }

        private static void Save()
        {
            var list = new OverrideList();
            foreach (KeyValuePair<GameAction, string> kv in _overrides)
                list.items.Add(new BindOverride { action = kv.Key.ToString(), path = kv.Value });
            PlayerPrefs.SetString(PrefKey, JsonUtility.ToJson(list));
            PlayerPrefs.Save();
        }

        // ── Query ──────────────────────────────────────────────────────────────────
        /// <summary>InputSystem binding path for an action (override if set, else default).</summary>
        public static string GetBinding(GameAction action)
        {
            EnsureLoaded();
            return _overrides.TryGetValue(action, out string p) ? p : Defaults[action];
        }

        /// <summary>Short display label for an action's current binding (e.g. "Q", "LMB", "Space").</summary>
        public static string GetKeyName(GameAction action) => PathToLabel(GetBinding(action));

        public static bool IsReserved(string path) => Reserved.Contains(path);

        /// <summary>The action currently bound to a path, or null. Backs the duplicate guard.</summary>
        public static GameAction? ActionBoundTo(string path)
        {
            foreach (GameAction a in Enum.GetValues(typeof(GameAction)))
                if (string.Equals(GetBinding(a), path, StringComparison.OrdinalIgnoreCase))
                    return a;
            return null;
        }

        // ── Mutate ─────────────────────────────────────────────────────────────────
        /// <summary>
        /// Rebind an action. Rejects reserved keys and a path already owned by a different action.
        /// Returns false with a human message in <paramref name="error"/> on rejection.
        /// </summary>
        public static bool TrySetBinding(GameAction action, string path, out string error)
        {
            EnsureLoaded();
            error = null;
            if (string.IsNullOrEmpty(path)) { error = "empty binding"; return false; }
            if (Reserved.Contains(path)) { error = $"{PathToLabel(path)} is reserved"; return false; }

            GameAction? owner = ActionBoundTo(path);
            if (owner.HasValue && owner.Value != action)
            {
                error = $"{PathToLabel(path)} already bound to {owner.Value}";
                return false;
            }

            if (string.Equals(Defaults[action], path, StringComparison.OrdinalIgnoreCase))
                _overrides.Remove(action); // back to default → drop the override
            else
                _overrides[action] = path;

            Save();
            OnBindingsChanged?.Invoke();
            return true;
        }

        public static void ResetToDefaults()
        {
            EnsureLoaded();
            _overrides.Clear();
            Save();
            OnBindingsChanged?.Invoke();
        }

        /// <summary>Re-apply bindings to live InputActions by notifying consumers (the missing "rebuild" step).</summary>
        public static void RebuildBindings() => OnBindingsChanged?.Invoke();

        // ── Display ────────────────────────────────────────────────────────────────
        public static string PathToLabel(string path)
        {
            if (string.IsNullOrEmpty(path)) return "?";
            switch (path)
            {
                case "<Mouse>/leftButton":   return "LMB";
                case "<Mouse>/rightButton":  return "RMB";
                case "<Mouse>/middleButton": return "MMB";
            }

            int slash = path.LastIndexOf('/');
            string key = slash >= 0 ? path.Substring(slash + 1) : path;
            switch (key.ToLowerInvariant())
            {
                case "space":                return "Space";
                case "leftshift": case "rightshift": case "shift": return "Shift";
                case "leftctrl":  case "rightctrl":  case "ctrl":  return "Ctrl";
                case "leftalt":   case "rightalt":   case "alt":   return "Alt";
                case "escape":               return "Esc";
                case "tab":                  return "Tab";
            }
            return key.Length == 1
                ? key.ToUpperInvariant()
                : char.ToUpperInvariant(key[0]) + key.Substring(1);
        }

        // ── Legacy slot API (slot 0..3 = Skill1..Skill4) ─────────────────────────────
        private static GameAction SlotAction(int slot) => SkillSlots[Mathf.Clamp(slot, 0, SkillSlots.Length - 1)];

        public static string GetBinding(int slot) => GetBinding(SlotAction(slot));
        public static string GetKeyName(int slot) => GetKeyName(SlotAction(slot));

        public static void SetKey(int slot, string keyName)
        {
            string path = $"<Keyboard>/{keyName.ToLowerInvariant()}";
            TrySetBinding(SlotAction(slot), path, out _);
        }

        public static string[] GetAllKeyNames()
        {
            var names = new string[SkillSlots.Length];
            for (int i = 0; i < SkillSlots.Length; i++) names[i] = GetKeyName(SkillSlots[i]);
            return names;
        }
    }
}
