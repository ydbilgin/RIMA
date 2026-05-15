using System;
using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Combat
{
    public static class ProcLimiter
    {
        private static readonly Dictionary<string, float> lastTriggerTime = new Dictionary<string, float>();
        private static readonly Dictionary<string, int> frameTriggerCount = new Dictionary<string, int>();
        private static int currentFrame = -1;

        public static bool TryProc(string tag, float minIcdSeconds = 0.05f, int maxPerFrame = 4)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return false;
            }

            if (Time.frameCount != currentFrame)
            {
                frameTriggerCount.Clear();
                currentFrame = Time.frameCount;
            }

            int count;
            frameTriggerCount.TryGetValue(tag, out count);
            if (count >= maxPerFrame)
            {
                return false;
            }

            float last;
            if (lastTriggerTime.TryGetValue(tag, out last))
            {
                if (Time.time - last < minIcdSeconds)
                {
                    return false;
                }
            }

            lastTriggerTime[tag] = Time.time;
            frameTriggerCount[tag] = count + 1;
            return true;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnDomainReload()
        {
            lastTriggerTime.Clear();
            frameTriggerCount.Clear();
            currentFrame = -1;
        }
    }
}
