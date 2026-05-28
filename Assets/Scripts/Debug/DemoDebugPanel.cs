#if DEVELOPMENT_BUILD || UNITY_EDITOR
using RIMA.Systems.Map;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RIMA.DebugTools
{
    public sealed class DemoDebugPanel : MonoBehaviour
    {
        private bool _visible;
        private bool _godMode;
        private int _speedIndex;

        private static readonly float[] Speeds = { 1f, 2f, 0.25f };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Bootstrap()
        {
            if (FindFirstObjectByType<DemoDebugPanel>() != null) return;

            GameObject panel = new GameObject("DemoDebugPanel");
            DontDestroyOnLoad(panel);
            panel.AddComponent<DemoDebugPanel>();
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null && keyboard.f1Key.wasPressedThisFrame)
            {
                _visible = !_visible;
            }

            if (_godMode)
            {
                Health playerHealth = GetPlayerHealth();
                if (playerHealth != null)
                {
                    playerHealth.SetImmune(true);
                    playerHealth.RestoreToFull();
                }
            }
        }

        private void OnGUI()
        {
            if (!_visible) return;

            GUILayout.BeginArea(new Rect(12f, 12f, 210f, 330f), GUI.skin.box);
            GUILayout.Label("RIMA Debug");

            if (GUILayout.Button("Kill All Mobs")) KillAllMobs();

            bool nextGodMode = GUILayout.Toggle(_godMode, "God Mode");
            if (nextGodMode != _godMode)
            {
                _godMode = nextGodMode;
                GetPlayerHealth()?.SetImmune(_godMode);
            }

            if (GUILayout.Button($"Speed {Speeds[_speedIndex]:0.##}x")) CycleSpeed();
            if (GUILayout.Button("Force Room Clear")) RoomLoader.DebugForceRoomCleared();
            if (GUILayout.Button("Restart Room")) RoomLoader.JumpToRoom(RoomLoader.CurrentRoomIndex);
            if (GUILayout.Button("Next Room")) RoomLoader.LoadNext();

            GUILayout.Space(6f);
            GUILayout.Label("Jump Room");
            GUILayout.BeginHorizontal();
            for (int i = 0; i < 5; i++)
            {
                if (GUILayout.Button((i + 1).ToString(), GUILayout.Width(34f)))
                {
                    RoomLoader.JumpToRoom(i);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private static void KillAllMobs()
        {
            foreach (Health health in FindObjectsByType<Health>(FindObjectsSortMode.None))
            {
                if (health == null || health.IsDead || !HasEnemyTag(health.transform)) continue;
                health.TakeDamage(99999);
            }
        }

        private void CycleSpeed()
        {
            _speedIndex = (_speedIndex + 1) % Speeds.Length;
            Time.timeScale = Speeds[_speedIndex];
        }

        private static Health GetPlayerHealth()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            return player != null ? player.GetComponentInChildren<Health>() : null;
        }

        private static bool HasEnemyTag(Transform transform)
        {
            while (transform != null)
            {
                if (transform.CompareTag("Enemy")) return true;
                transform = transform.parent;
            }

            return false;
        }
    }
}
#endif
