using System.Collections.Generic;
using TMPro;
using UnityEngine;
using RIMA.Audio;
using RIMA.Combat;

namespace RIMA
{
    /// <summary>
    /// T2 — [RMB] Execute world-space prompt.
    /// Shows "[RMB] İnfaz" above the nearest Broken/Sundered enemy when within 2 units.
    /// Reuses the ChamberSelectBootstrap prompt pattern: world-space TextMeshPro, no Canvas.
    ///
    /// Attach to the Player GameObject (same as PlayerController/DeathBlow).
    /// Self-bootstraps: lazily finds the DeathBlow skill, tracks enemy states.
    /// Hides automatically when DeathBlow fires (listens to OnKill/state-consumed path via
    /// polling, since DeathBlow consumes the Broken/Sundered state on hit).
    ///
    /// Constraints:
    ///   - No new UI system: reuses TMP world-space text only.
    ///   - Cyan RESERVED for player/Rift — prompt text is white/gold, NOT cyan.
    ///   - Single prompt: closest Broken/Sundered target only.
    ///   - Calls HitPauseDriver.TriggerExecutePause + ScreenShakeDriver.TriggerExecuteShake
    ///     + AudioManager.Play(ExecutePayoff) when the player fires RMB and a target is consumed.
    /// </summary>
    [DisallowMultipleComponent]
    public class ExecutePromptDriver : MonoBehaviour
    {
        [Header("Detection")]
        [SerializeField] private float detectRadius = 2f;
        [SerializeField] private LayerMask enemyLayer;

        [Header("Prompt appearance")]
        [SerializeField] private float promptFontSize = 4.2f;
        [SerializeField] private Color promptColor = new Color(1f, 0.92f, 0.55f, 1f); // warm gold, NOT cyan
        [SerializeField] private Vector3 promptOffset = new Vector3(0f, 0.85f, 0f);

        // World-space TMP label (no Canvas — same as ChamberSelectBootstrap.CreateWorldText)
        private TextMeshPro promptLabel;
        private GameObject currentTarget;   // the enemy currently showing the prompt
        private bool wasShowingPrompt;

        private void Awake()
        {
            if (enemyLayer.value == 0)
                enemyLayer = LayerMask.GetMask("Enemy");

            CreatePromptLabel();
        }

        private void CreatePromptLabel()
        {
            var go = new GameObject("ExecutePrompt_WorldLabel");
            go.transform.SetParent(null, true);   // world-space, unparented so it follows target manually
            promptLabel = go.AddComponent<TextMeshPro>();
            promptLabel.fontSize = promptFontSize;
            promptLabel.color = promptColor;
            promptLabel.alignment = TextAlignmentOptions.Center;
            promptLabel.enableWordWrapping = false;
            promptLabel.sortingLayerID = SortingLayer.NameToID("UI");
            promptLabel.sortingOrder = 300;
            promptLabel.text = "[RMB] İnfaz";
            promptLabel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (promptLabel != null)
                Destroy(promptLabel.gameObject);
        }

        private void Update()
        {
            Health nearest = FindNearestExecutableTarget();

            if (nearest != null)
            {
                // Show prompt above target
                currentTarget = nearest.gameObject;
                promptLabel.transform.position = nearest.transform.position + promptOffset;
                promptLabel.gameObject.SetActive(true);
                wasShowingPrompt = true;

                // Pulse alpha for visibility
                float pulse = 0.80f + 0.20f * Mathf.Sin(Time.time * 6f);
                promptLabel.color = new Color(promptColor.r, promptColor.g, promptColor.b, pulse);
            }
            else
            {
                if (wasShowingPrompt)
                {
                    promptLabel.gameObject.SetActive(false);
                    wasShowingPrompt = false;
                    currentTarget = null;
                }
            }
        }

        /// <summary>
        /// Called by DeathBlow (or any execute path) when an execute successfully fires.
        /// Fires execute juice: freeze + shake + SFX.
        /// </summary>
        public static void OnExecuteFired()
        {
            AudioManager.Play(Sfx.ExecutePayoff);
            HitPauseDriver.Instance?.TriggerExecutePause();
            ScreenShakeDriver.Instance?.TriggerExecuteShake();
        }

        // ── internals ────────────────────────────────────────────────────────────

        private Health FindNearestExecutableTarget()
        {
            Vector2 pos = transform.position;
            var hits = Physics2D.OverlapCircleAll(pos, detectRadius, enemyLayer);
            float best = float.MaxValue;
            Health bestHp = null;

            foreach (var h in hits)
            {
                if (h.gameObject == gameObject) continue;
                var hp = h.GetComponent<Health>();
                if (hp == null || hp.IsDead) continue;

                var state = h.GetComponent<SkillStateTracker>();
                if (state == null) continue;
                bool markedForExecution = state.Has(SkillStateTracker.Broken)
                                       || state.Has(SkillStateTracker.Sundered);
                if (!markedForExecution) continue;

                float d = Vector2.Distance(pos, h.transform.position);
                if (d < best) { best = d; bestHp = hp; }
            }

            return bestHp;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.6f, 0f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}
