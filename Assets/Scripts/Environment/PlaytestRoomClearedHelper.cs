using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;
using RIMA.Systems.Map;

namespace RIMA.Environment
{
    /// <summary>
    /// Day 2 playtest debug helper. Two ways to fire RoomLoader.OnRoomCleared:
    /// 1. Press K (manual). 2. Auto-fire when all GameObjects whose name contains
    /// mobNameContains are gone (after at least one was spawned).
    /// </summary>
    public sealed class PlaytestRoomClearedHelper : MonoBehaviour
    {
        public string mobNameContains = "FractureImp";
        public Key manualFireKey = Key.K;

        private int _peakCount = -1;
        private bool _firedThisWave;

        private void Update()
        {
            if (Keyboard.current != null && Keyboard.current[manualFireKey].wasPressedThisFrame)
            {
                Debug.Log("[PlaytestHelper] K pressed -> RoomLoader.OnRoomCleared");
                FireRoomCleared();
                _firedThisWave = true;
                return;
            }

            int alive = CountAlive();
            if (_peakCount < 0 && alive > 0) _peakCount = alive;
            if (_peakCount > 0 && alive == 0 && !_firedThisWave)
            {
                Debug.Log($"[PlaytestHelper] All '{mobNameContains}' dead -> RoomLoader.OnRoomCleared");
                FireRoomCleared();
                _firedThisWave = true;
            }

            // Reset wave if new mobs respawn
            if (alive > _peakCount) { _peakCount = alive; _firedThisWave = false; }
        }

        private int CountAlive()
        {
            int n = 0;
            var all = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (var g in all)
                if (g.activeInHierarchy && g.name.Contains(mobNameContains)) n++;
            return n;
        }

        private static void FireRoomCleared()
        {
            var field = typeof(RoomLoader).GetField("OnRoomCleared",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var del = field?.GetValue(null) as System.Action;
            del?.Invoke();
        }
    }
}
