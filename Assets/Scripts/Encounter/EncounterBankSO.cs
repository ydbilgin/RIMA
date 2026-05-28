using UnityEngine;

namespace RIMA.Encounter
{
    [CreateAssetMenu(menuName = "RIMA/Encounter/Encounter Bank", fileName = "EncounterBank_New", order = 221)]
    public class EncounterBankSO : ScriptableObject
    {
        public EncounterWaveSO[] waves;
        public AnimationCurve difficultyCurve = AnimationCurve.Linear(0f, 1f, 1f, 1f);

        public EncounterWaveSO PickWave(float difficulty, int seed)
        {
            if (waves == null || waves.Length == 0)
                return null;

            float scaledDifficulty = difficultyCurve != null ? difficultyCurve.Evaluate(difficulty) : difficulty;
            int bestIndex = 0;
            float bestDistance = float.MaxValue;

            for (int i = 0; i < waves.Length; i++)
            {
                EncounterWaveSO wave = waves[i];
                if (wave == null) continue;

                float distance = Mathf.Abs(wave.threatBudget - scaledDifficulty);
                if (distance < bestDistance)
                {
                    bestIndex = i;
                    bestDistance = distance;
                }
            }

            int offset = waves.Length > 1 ? Mathf.Abs(seed) % waves.Length : 0;
            for (int i = 0; i < waves.Length; i++)
            {
                int index = (bestIndex + offset + i) % waves.Length;
                if (waves[index] != null)
                    return waves[index];
            }

            return null;
        }
    }
}
