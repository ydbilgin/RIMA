using System;
using UnityEngine;

namespace RIMA.Combat
{
    /// <summary>
    /// Binds two transforms with a dynamic two-point chain, tether, or beam.
    /// </summary>
    public class ChainBinder : MonoBehaviour
    {
        private static readonly int ColorId = Shader.PropertyToID("_Color");
        private static readonly int ScrollSpeedId = Shader.PropertyToID("_ScrollSpeed");

        private Transform caster;
        private Transform target;
        private ChainBinderConfig config;
        private LineRenderer line;
        private Material materialInstance;
        private float expireTime;

        public void Bind(Transform caster, Transform target, ChainBinderConfig config)
        {
            this.caster = caster;
            this.target = target;
            this.config = config;

            if (caster == null || target == null || config == null)
            {
                Unbind();
                return;
            }

            expireTime = Time.time + Mathf.Max(0.01f, config.duration);
            EnsureLineRenderer();
            ApplyConfig();
            UpdateLine();
        }

        public void Unbind()
        {
            Destroy(gameObject);
        }

        private void Update()
        {
            if (caster == null || target == null || Time.time >= expireTime)
            {
                Unbind();
                return;
            }

            UpdateLine();
        }

        private void OnDestroy()
        {
            if (materialInstance != null)
            {
                Destroy(materialInstance);
            }
        }

        private void EnsureLineRenderer()
        {
            if (line == null)
            {
                line = GetComponent<LineRenderer>();
            }

            if (line == null)
            {
                line = gameObject.AddComponent<LineRenderer>();
            }

            line.positionCount = 2;
            line.useWorldSpace = true;
            line.textureMode = LineTextureMode.Tile;
            line.alignment = LineAlignment.View;
            line.numCapVertices = 0;
        }

        private void ApplyConfig()
        {
            line.startWidth = config.width;
            line.endWidth = config.width;
            line.sortingOrder = config.sortingOrder;
            line.startColor = config.tint;
            line.endColor = config.tint;

            if (materialInstance != null)
            {
                Destroy(materialInstance);
                materialInstance = null;
            }

            if (config.chainMaterial != null)
            {
                materialInstance = new Material(config.chainMaterial);
                materialInstance.SetColor(ColorId, config.tint);
                materialInstance.SetFloat(ScrollSpeedId, config.scrollSpeed);
                line.material = materialInstance;
            }
        }

        private void UpdateLine()
        {
            Vector3 start = caster.position + config.casterAnchorOffset;
            Vector3 end = target.position + config.targetAnchorOffset;
            float distance = Vector3.Distance(start, end);

            line.SetPosition(0, start);
            line.SetPosition(1, end);

            if (materialInstance != null)
            {
                materialInstance.mainTextureScale = new Vector2(Mathf.Max(0.01f, distance), 1f);
            }
        }
    }

    /// <summary>
    /// Runtime configuration for a chain binder line renderer.
    /// </summary>
    [Serializable]
    public class ChainBinderConfig
    {
        public Material chainMaterial;
        public float width = 0.3f;
        public float scrollSpeed = 2f;
        public Color tint = Color.white;
        public float duration = 3f;
        public int sortingOrder = 5;
        public Vector3 casterAnchorOffset;
        public Vector3 targetAnchorOffset;
    }
}
