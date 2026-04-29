using UnityEngine;
using UnityEngine.Events;

namespace RIMA
{
    /// <summary>
    /// LMB ecol takip sistemi (stub — Faz 1 temel implementasyon).
    /// ForgeUI bu sistemi çağırır. LMB attack logic Faz 2'de bağlanır.
    /// </summary>
    public static class LMBEcolSystem
    {
        public static string ChosenEcol  { get; private set; } = "";
        public static int    EcolLevel   { get; private set; } = 0;

        public static UnityAction<string, int> OnEcolChanged;

        public static void SetEcol(string ecolName)
        {
            ChosenEcol = ecolName;
            EcolLevel  = 1;
            Debug.Log($"[LMBEcol] Ecol seçildi: {ecolName} Lv1");
            OnEcolChanged?.Invoke(ChosenEcol, EcolLevel);
        }

        public static void UpgradeEcol()
        {
            if (string.IsNullOrEmpty(ChosenEcol)) { Debug.LogWarning("[LMBEcol] Ecol seçilmemiş — yükseltme yok."); return; }
            EcolLevel = Mathf.Min(EcolLevel + 1, 3);
            Debug.Log($"[LMBEcol] {ChosenEcol} → Lv{EcolLevel}");
            OnEcolChanged?.Invoke(ChosenEcol, EcolLevel);
        }

        public static void Reset()
        {
            ChosenEcol = "";
            EcolLevel  = 0;
        }
    }
}
