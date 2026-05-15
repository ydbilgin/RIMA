using System;
using UnityEngine;

namespace RIMA.Combat
{
    public static class CombatEventBus
    {
        public static event Action<HitEvent> OnHit;
        public static event Action<KillEvent> OnKill;
        public static event Action<DashEvent> OnDash;
        public static event Action<StatusEvent> OnStatusApplied;
        public static event Action<CommitBeatEvent> OnCommitBeat;

        public static void PublishHit(HitEvent e)
        {
            OnHit?.Invoke(e);
        }

        public static void PublishKill(KillEvent e)
        {
            OnKill?.Invoke(e);
        }

        public static void PublishDash(DashEvent e)
        {
            OnDash?.Invoke(e);
        }

        public static void PublishStatusApplied(StatusEvent e)
        {
            OnStatusApplied?.Invoke(e);
        }

        public static void PublishCommitBeat(CommitBeatEvent e)
        {
            OnCommitBeat?.Invoke(e);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnDomainReload()
        {
            OnHit = null;
            OnKill = null;
            OnDash = null;
            OnStatusApplied = null;
            OnCommitBeat = null;
        }
    }

    public struct HitEvent
    {
        public Vector3 worldPos;
        public GameObject attacker;
        public GameObject target;
        public float damage;
        public string element;
        public bool isCrit;
        public Vector2 hitDirection;
    }

    public struct KillEvent
    {
        public Vector3 worldPos;
        public GameObject killer;
        public GameObject victim;
        public string mobFamily;
    }

    public struct DashEvent
    {
        public Vector3 startPos;
        public Vector3 endPos;
        public GameObject dasher;
        public float duration;
    }

    public struct StatusEvent
    {
        public Vector3 worldPos;
        public GameObject target;
        public string statusId;
        public float duration;
    }

    public struct CommitBeatEvent
    {
        public Vector3 worldPos;
        public GameObject attacker;
        public int beatIndex;
    }
}
