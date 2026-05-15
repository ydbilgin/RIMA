using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Combat
{
    public class HitPauseDriver : MonoBehaviour
    {
        public static HitPauseDriver Instance { get; private set; }

        [SerializeField] private float pauseDuration = 0.06f;
        [SerializeField] private float pauseTimeScale = 0f;

        private Coroutine pauseCoroutine;
        private float previousTimeScale = 1f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnEnable()
        {
            CombatEventBus.OnHit += HandleHit;
        }

        private void OnDisable()
        {
            CombatEventBus.OnHit -= HandleHit;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void HandleHit(HitEvent e)
        {
            TriggerPause(pauseDuration);
        }

        public void TriggerPause(float duration)
        {
            if (pauseCoroutine != null)
            {
                StopCoroutine(pauseCoroutine);
                Time.timeScale = previousTimeScale;
            }

            pauseCoroutine = StartCoroutine(PauseRoutine(Mathf.Max(0f, duration)));
        }

        private IEnumerator PauseRoutine(float duration)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = Mathf.Clamp01(pauseTimeScale);
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = previousTimeScale;
            pauseCoroutine = null;
        }
    }
}
