using UnityEngine;
using RIMA.Combat;

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
        _comboCount++;
        Debug.Log($"[CombatHandler] OnCommitBeat - combo #{_comboCount}");

        // A6: drive the finisher juice — HitPauseDriver + ScreenShakeDriver + VFXRouter
        // all subscribe to CombatEventBus.OnCommitBeat. The live Beat3 path never published it
        // (only VFXBusDemo did), so finisher hitstop/shake never fired in real combat.
        CombatEventBus.PublishCommitBeat(new CommitBeatEvent
        {
            worldPos = transform.position,
            attacker = gameObject,
            beatIndex = 3
        });
    }
}
