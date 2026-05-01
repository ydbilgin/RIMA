using NUnit.Framework;
using UnityEngine;
using RIMA;

namespace RIMA.Tests
{
    public class VFXTests
    {
        private GameObject go;

        [SetUp]
        public void SetUp()
        {
            go = new GameObject("VFX_Test");
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(go);
        }

        // ─── HandGlowVFX ─────────────────────────────────────────────────────

        [Test]
        public void HandGlowVFX_RequiresParticleSystem_AutoAdded()
        {
            go.AddComponent<HandGlowVFX>();
            Assert.IsNotNull(go.GetComponent<ParticleSystem>());
        }

        [Test]
        public void HandGlowVFX_SetColor_DoesNotThrow()
        {
            var vfx = go.AddComponent<HandGlowVFX>();
            // particleSystemRef is null in EditMode (Awake not called), ApplyVisuals guards with null check
            Assert.DoesNotThrow(() => vfx.SetColor(Color.red));
        }

        [Test]
        public void HandGlowVFX_SetCastState_True_DoesNotThrow()
        {
            var vfx = go.AddComponent<HandGlowVFX>();
            Assert.DoesNotThrow(() => vfx.SetCastState(true));
        }

        [Test]
        public void HandGlowVFX_SetCastState_False_DoesNotThrow()
        {
            var vfx = go.AddComponent<HandGlowVFX>();
            Assert.DoesNotThrow(() => vfx.SetCastState(false));
        }

        // ─── RiftGlowVFX ─────────────────────────────────────────────────────

        [Test]
        public void RiftGlowVFX_RequiresParticleSystem_AutoAdded()
        {
            go.AddComponent<RiftGlowVFX>();
            Assert.IsNotNull(go.GetComponent<ParticleSystem>());
        }

        [Test]
        public void RiftGlowVFX_SetColor_DoesNotThrow()
        {
            var vfx = go.AddComponent<RiftGlowVFX>();
            Assert.DoesNotThrow(() => vfx.SetColor(Color.blue));
        }

        [Test]
        public void RiftGlowVFX_SetCastState_True_DoesNotThrow()
        {
            var vfx = go.AddComponent<RiftGlowVFX>();
            Assert.DoesNotThrow(() => vfx.SetCastState(true));
        }

        [Test]
        public void RiftGlowVFX_SetCastState_False_DoesNotThrow()
        {
            var vfx = go.AddComponent<RiftGlowVFX>();
            Assert.DoesNotThrow(() => vfx.SetCastState(false));
        }
    }
}
