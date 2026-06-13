#if DEMO_BUILD || DEVELOPMENT_BUILD || UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA.UI.BuildMode
{
    /// <summary>
    /// Lightweight single-owner key registry for in-Play authoring tools (consolidation item 1).
    ///
    /// The F2 keybind conflict (two tools polling F2 at once -> double-toggle / fighting) is the #1
    /// bug class the editor-consolidation decision exists to prevent. This registry makes ownership
    /// EXPLICIT: a tool calls <see cref="RegisterExclusive"/> for the keys it polls; a SECOND tool
    /// trying to claim an already-owned key gets a clear error (and a false return) instead of a
    /// silent conflict. The owning tool can re-register the same key (idempotent) and release it.
    ///
    /// This is the "hafif guard" the decision asks for — NOT a full GlobalInputRegistry framework
    /// (that is post-demo). It only tracks ownership + surfaces conflicts; it does not poll input.
    ///
    /// DisableDomainReload-safe: the dictionary is a plain static. Under DisableDomainReload it can
    /// carry stale owners across play sessions, so a registration whose recorded owner is now a
    /// fake-null (destroyed) UnityEngine.Object is treated as free and silently re-claimed.
    /// </summary>
    public static class InPlayToolKeyRegistry
    {
        private static readonly Dictionary<Key, Object> _owners = new Dictionary<Key, Object>();

        /// <summary>
        /// Claim <paramref name="key"/> for <paramref name="owner"/>. Returns true if the owner now
        /// holds the key. Returns false (and logs a clear error) ONLY when a DIFFERENT, still-alive
        /// owner already holds it. Re-claiming a key you already own is a no-op success. A key whose
        /// recorded owner has been destroyed (fake-null under DisableDomainReload) is treated as free.
        /// </summary>
        public static bool RegisterExclusive(Key key, Object owner)
        {
            if (owner == null)
            {
                Debug.LogError($"[InPlayToolKeyRegistry] RegisterExclusive({key}) called with a null owner; ignored.");
                return false;
            }

            if (_owners.TryGetValue(key, out Object existing))
            {
                // ReferenceEquals avoids Unity's overloaded == (which reports a destroyed object as
                // null): if the SAME object re-registers, succeed idempotently.
                if (ReferenceEquals(existing, owner)) return true;

                // existing != null uses Unity's lifetime-aware ==: a destroyed owner is "null" here,
                // so a leaked stale claim from a previous play session is reclaimed cleanly.
                if (existing != null)
                {
                    Debug.LogError(
                        $"[InPlayToolKeyRegistry] Key '{key}' is already owned by '{Name(existing)}'. " +
                        $"'{Name(owner)}' cannot also poll it — only ONE tool may own a key (this is the " +
                        "F2-conflict guard). Release it from the current owner first.");
                    return false;
                }
            }

            _owners[key] = owner;
            return true;
        }

        /// <summary>
        /// Release <paramref name="key"/> if (and only if) <paramref name="owner"/> currently holds
        /// it. Returns true on release. Releasing a key you do not own is a safe no-op (returns false).
        /// </summary>
        public static bool Release(Key key, Object owner)
        {
            if (_owners.TryGetValue(key, out Object existing) &&
                (ReferenceEquals(existing, owner) || existing == null))
            {
                _owners.Remove(key);
                return true;
            }
            return false;
        }

        /// <summary>The live owner of <paramref name="key"/>, or null if free / the owner was destroyed.</summary>
        public static Object OwnerOf(Key key)
        {
            if (_owners.TryGetValue(key, out Object existing) && existing != null) return existing;
            return null;
        }

        /// <summary>True iff <paramref name="owner"/> is the current live owner of <paramref name="key"/>.</summary>
        public static bool Owns(Key key, Object owner)
        {
            return owner != null && ReferenceEquals(OwnerOf(key), owner);
        }

        /// <summary>Drop ALL ownership (test teardown / fresh session reset).</summary>
        public static void ClearAll()
        {
            _owners.Clear();
        }

        private static string Name(Object o) => o != null ? o.name : "<destroyed>";
    }
}
#endif
