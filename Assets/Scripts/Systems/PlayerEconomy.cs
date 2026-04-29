using UnityEngine;
using UnityEngine.Events;

namespace RIMA
{
    /// <summary>
    /// Run içi gold tracker. Singleton — sahne başında bir GameManager'a ekle.
    /// </summary>
    public class PlayerEconomy : MonoBehaviour
    {
        public static PlayerEconomy Instance { get; private set; }

        [SerializeField] private int startingGold = 0;

        public int Gold { get; private set; }

        /// <summary>Gold değişince ateşlenir — arg: yeni toplam.</summary>
        public UnityEvent<int> OnGoldChanged = new UnityEvent<int>();

        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            Gold = startingGold;
        }

        public void AddGold(int amount)
        {
            Gold = Mathf.Max(0, Gold + amount);
            OnGoldChanged.Invoke(Gold);
            Debug.Log($"[Economy] +{amount} Altın → Toplam: {Gold}");
        }

        public bool TrySpend(int amount)
        {
            if (Gold < amount) return false;
            Gold -= amount;
            OnGoldChanged.Invoke(Gold);
            Debug.Log($"[Economy] -{amount} Altın → Toplam: {Gold}");
            return true;
        }

        /// <summary>Run bitimine (death/reset) sıfırla.</summary>
        public void ResetGold()
        {
            Gold = startingGold;
            OnGoldChanged.Invoke(Gold);
        }
    }
}
