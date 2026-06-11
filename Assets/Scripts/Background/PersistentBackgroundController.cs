using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Background
{
    [DisallowMultipleComponent]
    public sealed class PersistentBackgroundController : MonoBehaviour
    {
        [System.Serializable]
        public sealed class BgLayerDef
        {
            public Sprite[] frames;
            public int sortingOrder;
            public Vector2 parallaxFactor;
            public float animFps;

            public BgLayerDef()
            {
            }

            public BgLayerDef(int sortingOrder, Vector2 parallaxFactor, float animFps = 0f)
            {
                this.sortingOrder = sortingOrder;
                this.parallaxFactor = parallaxFactor;
                this.animFps = animFps;
            }
        }

        [SerializeField] private bool enablePersistentBackground = false;
        [SerializeField] private BgLayerDef far = new BgLayerDef(-200, new Vector2(0.02f, 0.015f));
        [SerializeField] private BgLayerDef mid = new BgLayerDef(-150, new Vector2(0.05f, 0.04f));
        [SerializeField] private BgLayerDef front = new BgLayerDef(-100, new Vector2(0.10f, 0.06f));

        private Transform backgroundRoot;
        private readonly List<LayerAnimation> animations = new List<LayerAnimation>();

        private void Start()
        {
            BuildIfEnabled();
        }

        public void BuildIfEnabled()
        {
            if (!enablePersistentBackground || backgroundRoot != null)
            {
                return;
            }

            GameObject root = new GameObject("PersistentBackground");
            root.transform.SetParent(transform, false);
            backgroundRoot = root.transform;
            animations.Clear();

            BuildLayer(backgroundRoot, "Far", far);
            BuildLayer(backgroundRoot, "Mid", mid);
            BuildLayer(backgroundRoot, "Front", front);
        }

        private void BuildLayer(Transform parent, string name, BgLayerDef def)
        {
            if (def == null)
            {
                return;
            }

            GameObject layer = new GameObject(name);
            layer.transform.SetParent(parent, false);

            SpriteRenderer renderer = layer.AddComponent<SpriteRenderer>();
            renderer.sprite = def.frames != null && def.frames.Length > 0 ? def.frames[0] : null;
            renderer.sortingOrder = def.sortingOrder;
            renderer.drawMode = SpriteDrawMode.Simple;

            ParallaxLayer parallax = layer.AddComponent<ParallaxLayer>();
            parallax.factor = def.parallaxFactor;
            parallax.target = Camera.main;
            parallax.snapToPixel = true;
            parallax.pixelsPerUnit = 64;

            if (def.frames != null && def.frames.Length > 1 && def.animFps > 0f)
            {
                animations.Add(new LayerAnimation(renderer, def.frames, def.animFps));
            }
        }

        private void Update()
        {
            for (int i = 0; i < animations.Count; i++)
            {
                animations[i].Tick(Time.deltaTime);
            }
        }

        private sealed class LayerAnimation
        {
            private readonly SpriteRenderer targetRenderer;
            private readonly Sprite[] frames;
            private readonly float fps;
            private float timer;
            private int frameIndex;

            public LayerAnimation(SpriteRenderer targetRenderer, Sprite[] frames, float fps)
            {
                this.targetRenderer = targetRenderer;
                this.frames = frames;
                this.fps = fps;
                timer = 0f;
                frameIndex = 0;
            }

            public void Tick(float deltaTime)
            {
                if (targetRenderer == null || frames == null || frames.Length <= 1 || fps <= 0f)
                {
                    return;
                }

                timer += deltaTime;
                float frameDuration = 1f / fps;
                while (timer >= frameDuration)
                {
                    timer -= frameDuration;
                    frameIndex = (frameIndex + 1) % frames.Length;
                    targetRenderer.sprite = frames[frameIndex];
                }
            }
        }
    }
}
