# Beat3CommitTrigger — StateMachineBehaviour + CombatHandler echo wire — execute every step, commit at end

## Context

Animation Spec LOCKED §4.3 (STAGING/animation_system_spec_LOCKED.md): Karar #122 T1 implementation. Beat3CommitTrigger is a StateMachineBehaviour attached to the Beat3 AnimatorState (NOT AnimationClip event — rejected for 8-dir blend tree double-fire risk).

**Lock decision L2:** StateMachineBehaviour pattern.

## STEP 1 — Read existing combat scripts

Read `Assets/Scripts/Combat/` — what does CombatHandler.cs look like? Is OnCommitBeat() already defined?

## STEP 2 — Beat3CommitTrigger StateMachineBehaviour

Create `Assets/Scripts/Combat/Beat3CommitTrigger.cs`:

```csharp
using UnityEngine;

/// <summary>
/// Fires OnCommitBeat exactly once per Beat3 playthrough regardless of 8-direction blend weights.
/// Karar #122 T1 — StateMachineBehaviour on Beat3 AnimatorState (AnimationClip event rejected).
/// </summary>
public class Beat3CommitTrigger : StateMachineBehaviour
{
    [SerializeField] private float impactNormalizedTime = 0.417f; // frame 5 of 12

    private bool _fired;
    private CombatHandler _combat;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo info, int layerIndex)
    {
        _fired = false;
        _combat = animator.GetComponent<CombatHandler>();
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
```

## STEP 3 — CombatHandler.OnCommitBeat()

Open `Assets/Scripts/Combat/CombatHandler.cs`. If it doesn't exist, create a minimal version.

Add method if missing:
```csharp
/// <summary>Karar #122 T1 — called by Beat3CommitTrigger on impact frame.</summary>
public void OnCommitBeat()
{
    // ICD check
    if (Time.time - _lastCommitBeatTime < commitBeatICD) return;
    _lastCommitBeatTime = Time.time;

    // TODO Faz 2+: invoke Echo Resonance proc
    // For now: just increment combo counter and log
    _comboCount++;
    Debug.Log($"[CombatHandler] OnCommitBeat — combo #{_comboCount}");
}

private float _lastCommitBeatTime = -999f;
private int _comboCount = 0;
[SerializeField] private float commitBeatICD = 1.2f; // Karar #122 T1
```

## STEP 4 — Compile check

`read_console` — 0 errors required.

## STEP 5 — Commit

```bash
git add Assets/Scripts/Combat/Beat3CommitTrigger.cs Assets/Scripts/Combat/CombatHandler.cs
git commit -m "[anim-spec] Beat3CommitTrigger StateMachineBehaviour + CombatHandler ICD stub

- Beat3CommitTrigger: fires OnCommitBeat once per Beat3 play (normalizedTime >= 0.417)
- CombatHandler: OnCommitBeat + 1.2s ICD + combo counter (Karar #122 T1 stub)
- Animation spec L2: StateMachineBehaviour over AnimationClip event (8-dir blend tree)"
```

## STEP 6 — Report

Write `STAGING/beat3_commit_trigger_report.md`:
```
# Beat3CommitTrigger Report

## Beat3CommitTrigger.cs
[created Y/N, impactNormalizedTime = 0.417]

## CombatHandler.OnCommitBeat
[added Y/N, ICD 1.2s Y/N]

## Compile
[0 errors Y/N]
```

Append `CODEX_DONE_laurethgame.md`:
```
## [2026-05-14] Beat3CommitTrigger + CombatHandler stub
- Beat3CommitTrigger: Y/N
- OnCommitBeat + ICD 1.2s: Y/N
- Compile: Y/N
- Commit: [hash]
```

## Constraints

- DO NOT wire Beat3CommitTrigger to any AnimatorController yet — that requires Warblade animation clips to exist first
- DO NOT implement Echo proc body — stub only (Faz 2)
- ICD 1.2s is LOCKED (Karar #122 T1)

## Source References

1. `Assets/Scripts/Combat/` — mevcut combat scriptler
2. `STAGING/animation_system_spec_LOCKED.md` §4.3 — implementation pattern verbatim
