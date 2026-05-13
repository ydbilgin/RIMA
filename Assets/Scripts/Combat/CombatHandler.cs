using UnityEngine;

public class CombatHandler : MonoBehaviour
{
    private float _lastCommitBeatTime = -999f;
    private int _comboCount = 0;

    [SerializeField] private float commitBeatICD = 1.2f; // Karar #122 T1

    /// <summary>Karar #122 T1 - called by Beat3CommitTrigger on impact frame.</summary>
    public void OnCommitBeat()
    {
        // ICD check
        if (Time.time - _lastCommitBeatTime < commitBeatICD) return;
        _lastCommitBeatTime = Time.time;

        // TODO Faz 2+: invoke Echo Resonance proc
        // For now: just increment combo counter and log
        _comboCount++;
        Debug.Log($"[CombatHandler] OnCommitBeat - combo #{_comboCount}");
    }
}
