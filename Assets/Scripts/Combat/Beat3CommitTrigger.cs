using UnityEngine;

/// <summary>
/// Fires OnCommitBeat exactly once per Beat3 playthrough regardless of 8-direction blend weights.
/// Karar #122 T1 - StateMachineBehaviour on Beat3 AnimatorState (AnimationClip event rejected).
/// </summary>
public class Beat3CommitTrigger : StateMachineBehaviour
{
    [SerializeField] private float impactNormalizedTime = 0.417f; // frame 5 of 12

    private bool _fired;
    private CombatHandler _combat;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo info, int layerIndex)
    {
        _fired = false;
        _combat = animator.GetComponentInParent<CombatHandler>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo info, int layerIndex)
    {
        if (!_fired && info.normalizedTime >= impactNormalizedTime)
        {
            _fired = true;
            _combat?.OnCommitBeat();
        }
    }
}
