using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using RIMA.Systems.Map;

namespace RIMA.Environment
{
    /// <summary>
    /// Phase 1 glue — LOCK 1 (clear-to-unlock).
    ///
    /// RoomLoader.BuildRoomContent now wires gate unlock directly per room type:
    ///   - Combat rooms: mob-cleared → gate.Unlock() (inline delegate in BuildRoomContent).
    ///   - Reward rooms: fragment pickup → gate.Unlock() (RewardRoomAutoTrigger coroutine).
    ///   - Boss rooms:   boss death → RaiseDemoComplete() (WireBossDeathListener coroutine).
    ///
    /// MapFragmentBridge retains the Day 1 portal flow (Portal.OnEntered → draft → arm)
    /// for backwards compat and is the home for any future cross-cutting gate logic.
    ///
    /// useFragmentGateFlow flag: RETIRED (LOCK 1). All gate logic lives in RoomLoader + Bridge event wiring.
    /// </summary>
    [DisallowMultipleComponent]
    [MovedFrom(false, sourceNamespace: "RIMA.Environment", sourceClassName: "PortalRewardBridge")]
    public sealed class MapFragmentBridge : MonoBehaviour
    {
        [Tooltip("Optional explicit controller. Falls back to first PortalSpawnController in scene.")]
        public PortalSpawnController controller;

        // ── Day 1 portal state ────────────────────────────────────────────────────
        private readonly HashSet<Portal> _armed     = new HashSet<Portal>();
        private readonly List<Portal>   _subscribed = new List<Portal>();
        private Portal _pendingPortal;
        private bool _draftListenerHooked;

        // ── Unity lifecycle ───────────────────────────────────────────────────────

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
        }

        private void OnDisable()
        {
            UnhookDraftListener();
            UnsubscribeAllPortals();
        }

        private void Update()
        {
            if (controller == null) return;
            if (controller.ActivePortals == null) return;
            if (controller.ActivePortals.Count != _subscribed.Count)
                RefreshPortalSubscriptions();
        }

        // ── Day 1 portal helpers ──────────────────────────────────────────────────

        private void RefreshPortalSubscriptions()
        {
            UnsubscribeAllPortals();
            if (controller == null || controller.ActivePortals == null) return;

            foreach (Portal portal in controller.ActivePortals)
            {
                if (portal == null) continue;
                portal.OnEntered += HandlePortalEntered;
                _subscribed.Add(portal);
            }
        }

        private void UnsubscribeAllPortals()
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

            if (_armed.Contains(portal))
            {
                Debug.Log("[MapFragmentBridge] Armed portal re-entered → RoomLoader.LoadNext.");
                RoomLoader.LoadNext();
                return;
            }

            HookDraftListener();
            if (DraftManager.Instance == null)
            {
                Debug.LogWarning("[MapFragmentBridge] DraftManager.Instance null at portal entry.");
                return;
            }

            _pendingPortal = portal;
            DraftManager.Instance.TriggerDraftFromFragment(portal);
        }

        private void HandleSkillPicked(global::RIMA.SkillData _)
        {
            // Day 1: arm the pending portal for next-room transition.
            if (_pendingPortal == null) return;
            _armed.Add(_pendingPortal);
            Debug.Log("[MapFragmentBridge] Skill picked — portal armed for next-room transition.");
            _pendingPortal = null;
        }
    }
}
