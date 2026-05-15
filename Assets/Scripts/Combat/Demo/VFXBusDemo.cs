using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Combat
{
    public class VFXBusDemo : MonoBehaviour
    {
        [SerializeField] private GameObject attacker;
        [SerializeField] private GameObject target;
        [SerializeField] private float damage = 24f;
        [SerializeField] private string element = "physical";
        [SerializeField] private string mobFamily = "default";
        [SerializeField] private string statusId = "bleed";

        [ContextMenu("Publish Hit")]
        public void PublishHit()
        {
            CombatEventBus.PublishHit(new HitEvent
            {
                worldPos = transform.position,
                attacker = attacker,
                target = target,
                damage = damage,
                element = element,
                isCrit = false,
                hitDirection = Vector2.right
            });

            HitFlashDriver flash = target != null ? target.GetComponentInChildren<HitFlashDriver>() : null;
            if (flash != null)
            {
                flash.Flash();
            }
        }

        [ContextMenu("Publish Kill")]
        public void PublishKill()
        {
            CombatEventBus.PublishKill(new KillEvent
            {
                worldPos = transform.position,
                killer = attacker,
                victim = target,
                mobFamily = mobFamily
            });
        }

        [ContextMenu("Publish Dash")]
        public void PublishDash()
        {
            CombatEventBus.PublishDash(new DashEvent
            {
                startPos = transform.position,
                endPos = transform.position + Vector3.right * 2f,
                dasher = attacker,
                duration = 0.2f
            });
        }

        [ContextMenu("Publish Status")]
        public void PublishStatus()
        {
            CombatEventBus.PublishStatusApplied(new StatusEvent
            {
                worldPos = transform.position,
                target = target,
                statusId = statusId,
                duration = 3f
            });
        }

        [ContextMenu("Publish CommitBeat (3)")]
        public void PublishCommitBeat3()
        {
            CombatEventBus.PublishCommitBeat(new CommitBeatEvent
            {
                worldPos = transform.position,
                attacker = attacker,
                beatIndex = 3
            });
        }
    }
}
