using System;
using UnityEngine;

namespace RIMA.Rooms
{
    /// <summary>
    /// Spawns room decor overlays at template-authored anchor points.
    /// </summary>
    public class RoomDecorationSpawner : MonoBehaviour
    {
        [SerializeField] private ScriptableObject registry;

        public void Initialize(RoomTemplate template, int runSeed)
        {
            if (template == null || template.anchors == null)
            {
                return;
            }

            var rng = new System.Random(runSeed);
            var mirrorFlip = template.mirrorFlipAllowed && rng.NextDouble() < 0.5d;

            foreach (var anchor in template.anchors)
            {
                if (!anchor.required && rng.NextDouble() > anchor.spawnWeight)
                {
                    continue;
                }

                var decor = SpawnDecorAtAnchor(anchor, rng);
                if (decor == null)
                {
                    continue;
                }

                var localPos = anchor.localPos;
                if (mirrorFlip)
                {
                    localPos.x = -localPos.x;
                }

                decor.transform.SetParent(transform, false);
                decor.transform.localPosition = localPos;

                if (mirrorFlip)
                {
                    var localScale = decor.transform.localScale;
                    localScale.x *= -1f;
                    decor.transform.localScale = localScale;
                }
            }
        }

        private GameObject SpawnDecorAtAnchor(OverlayAnchor anchor, System.Random rng)
        {
            _ = registry;
            _ = anchor;
            _ = rng;

            // TODO: pick from future DecorRegistry by category, optionalTag, and rng.
            // TODO: instantiate the selected prefab for this anchor.
            return null;
        }
    }
}
