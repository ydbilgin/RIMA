using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using RIMA.Systems.Map;

namespace RIMA.Environment
{
    /// <summary>
    /// Phase 1 glue: Portal.OnEntered → DraftManager.TriggerDraftFromFragment.
    /// Tracks per-portal "armed" state set by DraftManager.OnSkillPicked.
    /// Armed portal re-entry calls RoomLoader.LoadNext (stub in Phase 1).
    ///
    /// S110 LATE naming refactor: previously PortalRewardBridge. "Portal" runtime
    /// object is the visual rift; this bridge translates entry into a MapFragment
    /// (skill draft) reward + Gate transition.
    ///
    /// Day 2 additive opt-in: set useFragmentGateFlow=true to activate the
    /// MapFragment → DraftManager → Gate → RoomLoader pipeline alongside (or instead
    /// of) the Day 1 Portal flow. Day 1 Portal flow is untouched when false (default).
    /// </summary>
    [DisallowMultipleComponent]
    [MovedFrom(false, sourceNamespace: "RIMA.Environment", sourceClassName: "PortalRewardBridge")]
    public sealed class MapFragmentBridge : MonoBehaviour
    {
        [Tooltip("Optional explicit controller. Falls back to first PortalSpawnController in scene.")]
        public PortalSpawnController controller;

        [Tooltip("Day 2+ Fragment+Gate flow. If false, falls back to Day 1 Portal flow.")]
        public bool useFragmentGateFlow = false;

        // ── Day 1 state ───────────────────────────────────────────────────────────

        private readonly HashSet<Portal> _armed = new HashSet<Portal>();
        private readonly List<Portal> _subscribed = new List<Portal>();
        private bool _draftListenerHooked;

        // ── Day 2 state ───────────────────────────────────────────────────────────

        private readonly List<Gate> _gateSubscriptions = new List<Gate>();

        private void OnEnable()
        {
            if (controller == null)
            {
#if UNITY_2023_1_OR_NEWER
                controller = Object.FindFirstObjectByType<PortalSpawnController>();
#else
                controller = Object.FindObjectOfType<PortalSpawnController>();
#endif
            }

            HookDraftListener();
            RefreshPortalSubscriptions();

            // Day 2 fragment subscription (always register; handler branches on useFragmentGateFlow)
            MapFragment.OnAnyFragmentPickedUp += HandleAnyFragmentPickedUp;
        }

        private void OnDisable()
        {
            MapFragment.OnAnyFragmentPickedUp -= HandleAnyFragmentPickedUp;

            UnhookGateSubscriptions();
            UnhookDraftListener();
            UnsubscribeAll();
        }

        private void Update()
        {
            // Light poll: portals can spawn after OnEnable. Cheap because ActivePortals is a tiny list.
            if (controller == null) return;
            if (controller.ActivePortals == null) return;
            if (controller.ActivePortals.Count != _subscribed.Count)
            {
                RefreshPortalSubscriptions();
            }
        }

        private void RefreshPortalSubscriptions()
        {
            UnsubscribeAll();
            if (controller == null || controller.ActivePortals == null) return;

            foreach (var portal in controller.ActivePortals)
            {
                if (portal == null) continue;
                portal.OnEntered += HandlePortalEntered;
                _subscribed.Add(portal);
            }
        }

        private void UnsubscribeAll()
        {
            for (int i = 0; i < _subscribed.Count; i++)
            {
                if (_subscribed[i] != null) _subscribed[i].OnEntered -= HandlePortalEntered;
            }
            _subscribed.Clear();
        }

        private void HookDraftListener()
        {
            if (_draftListenerHooked) return;
            if (DraftManager.Instance == null) return;
            DraftManager.Instance.OnSkillPicked.AddListener(HandleSkillPicked);
            _draftListenerHooked = true;
        }

        private void UnhookDraftListener()
        {
            if (!_draftListenerHooked) return;
            if (DraftManager.Instance != null)
                DraftManager.Instance.OnSkillPicked.RemoveListener(HandleSkillPicked);
            _draftListenerHooked = false;
        }

        private void HandlePortalEntered(Portal portal)
        {
            if (portal == null) return;

            // Armed → next room transition (Gate).
            if (_armed.Contains(portal))
            {
                Debug.Log("[MapFragmentBridge] Armed gate re-entered → RoomLoader.LoadNext.");
                RoomLoader.LoadNext();
                return;
            }

            // First entry → open draft. DraftManager picks one offer; OnSkillPicked then arms this portal.
            HookDraftListener(); // late safety: DraftManager.Instance may have been null at OnEnable.
            if (DraftManager.Instance == null)
            {
                Debug.LogWarning("[MapFragmentBridge] DraftManager.Instance null at gate entry.");
                return;
            }

            _pendingPortal = portal;
            DraftManager.Instance.TriggerDraftFromFragment(portal);
        }

        private Portal _pendingPortal;

        private void HandleSkillPicked(global::RIMA.SkillData _)
        {
            if (useFragmentGateFlow)
            {
                // Day 2: skill picked → unlock all gates that were awaiting a fragment
                UnlockAllAwaitingGates();
                return;
            }

            // Day 1: arm the pending portal for next-room transition
            if (_pendingPortal == null) return;
            _armed.Add(_pendingPortal);
            Debug.Log("[MapFragmentBridge] Skill picked — gate armed for next-room transition.");
            _pendingPortal = null;
        }

        // ── Day 2 — Fragment+Gate flow ────────────────────────────────────────────

        private void HandleAnyFragmentPickedUp(MapFragment fragment)
        {
            if (!useFragmentGateFlow) return;

            Debug.Log($"[MapFragmentBridge] Fragment picked up at {fragment.transform.position} — triggering draft.");

            HookDraftListener(); // late safety: DraftManager may not have existed at OnEnable
            if (DraftManager.Instance == null)
            {
                Debug.LogWarning("[MapFragmentBridge] DraftManager.Instance null at fragment pickup.");
                return;
            }

            DraftManager.Instance.TriggerDraftFromFragment(null); // null Portal is accepted per DraftManager line 134
        }

        private void UnlockAllAwaitingGates()
        {
            UnhookGateSubscriptions();

#if UNITY_2023_1_OR_NEWER
            var allGates = Object.FindObjectsByType<Gate>(FindObjectsSortMode.None);
#else
            var allGates = Object.FindObjectsOfType<Gate>();
#endif
            foreach (var gate in allGates)
            {
                if (gate == null) continue;
                if (gate.CurrentState != Gate.State.AwaitingFragment) continue;

                gate.OnPlayerEntered += HandleGateEntered;
                _gateSubscriptions.Add(gate);
                gate.Unlock();
                Debug.Log($"[MapFragmentBridge] Gate unlocked at {gate.transform.position}");
            }
        }

        private void HandleGateEntered(Gate gate)
        {
            Debug.Log($"[MapFragmentBridge] Player entered gate at {gate.transform.position} → LoadNext.");
            RoomLoader.LoadNext();
        }

        private void UnhookGateSubscriptions()
        {
            foreach (var gate in _gateSubscriptions)
            {
                if (gate != null) gate.OnPlayerEntered -= HandleGateEntered;
            }
            _gateSubscriptions.Clear();
        }
    }
}
