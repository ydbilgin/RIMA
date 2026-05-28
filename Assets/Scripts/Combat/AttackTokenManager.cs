using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Combat
{
    public enum AttackTokenType { Melee, Ranged }

    public class AttackTokenManager : MonoBehaviour
    {
        private static AttackTokenManager instance;
        private static bool _shuttingDown;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            instance = null;
            _shuttingDown = false;
        }

        public static AttackTokenManager Instance
        {
            get
            {
                if (_shuttingDown) return null;
                if (instance == null && Application.isPlaying)
                {
                    var go = new GameObject(nameof(AttackTokenManager));
                    instance = go.AddComponent<AttackTokenManager>();
                }

                return instance;
            }

            private set => instance = value;
        }

        [SerializeField] private int maxMeleeTokens = 2;
        [SerializeField] private int maxRangedTokens = 1;

        private readonly Dictionary<GameObject, AttackTokenType> activeOwners = new Dictionary<GameObject, AttackTokenType>();
        private int _activeMelee;
        private int _activeRanged;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnApplicationQuit()
        {
            _shuttingDown = true;
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
                _shuttingDown = true;
            }
        }

        public bool TryConsumeToken(GameObject enemy, AttackTokenType type)
        {
            if (enemy == null) return false;

            if (activeOwners.ContainsKey(enemy))
                return false;

            if (!HasAvailableToken(type))
                return false;

            activeOwners.Add(enemy, type);
            Increment(type);
            return true;
        }

        public void ReturnToken(GameObject enemy, AttackTokenType type)
        {
            if (enemy == null) return;
            if (!activeOwners.TryGetValue(enemy, out var activeType)) return;
            if (activeType != type) return;

            activeOwners.Remove(enemy);
            Decrement(type);
        }

        public void OnEnemyDeath(GameObject enemy)
        {
            if (enemy == null) return;
            if (!activeOwners.TryGetValue(enemy, out var type)) return;

            activeOwners.Remove(enemy);
            Decrement(type);
        }

        private bool HasAvailableToken(AttackTokenType type)
        {
            return type == AttackTokenType.Ranged
                ? _activeRanged < maxRangedTokens
                : _activeMelee < maxMeleeTokens;
        }

        private void Increment(AttackTokenType type)
        {
            if (type == AttackTokenType.Ranged)
                _activeRanged++;
            else
                _activeMelee++;
        }

        private void Decrement(AttackTokenType type)
        {
            if (type == AttackTokenType.Ranged)
                _activeRanged = Mathf.Max(0, _activeRanged - 1);
            else
                _activeMelee = Mathf.Max(0, _activeMelee - 1);
        }
    }
}
