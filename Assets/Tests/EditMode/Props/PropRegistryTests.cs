#if UNITY_EDITOR
using NUnit.Framework;
using RIMA.MapDesigner.Props;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class PropRegistryTests
    {
        [Test]
        public void ResolveGuid_NullOrEmpty_ReturnsNull()
        {
            PropRegistrySO registry = ScriptableObject.CreateInstance<PropRegistrySO>();
            try
            {
                Assert.IsNull(registry.ResolveGuid(null));
                Assert.IsNull(registry.ResolveGuid(string.Empty));
            }
            finally
            {
                Object.DestroyImmediate(registry);
            }
        }

        [Test]
        public void ResolveGuid_UnknownGuid_ReturnsNull()
        {
            PropRegistrySO registry = ScriptableObject.CreateInstance<PropRegistrySO>();
            try
            {
                registry.RebuildIndex();
                Assert.IsNull(registry.ResolveGuid("nonexistent-guid"));
            }
            finally
            {
                Object.DestroyImmediate(registry);
            }
        }

        [Test]
        public void ResolveGuid_ByPropId_ReturnsRegisteredProp()
        {
            PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
            prop.propId = "test_prop_alpha";

            PropRegistrySO registry = ScriptableObject.CreateInstance<PropRegistrySO>();
            try
            {
                registry.EditorAddProp(prop);
                registry.RebuildIndex();

                PropDefinitionSO resolved = registry.ResolveGuid("test_prop_alpha");
                Assert.IsNotNull(resolved);
                Assert.AreEqual(prop.propId, resolved.propId);
            }
            finally
            {
                Object.DestroyImmediate(registry);
                Object.DestroyImmediate(prop);
            }
        }

        [Test]
        public void RebuildIndex_RebuildsAfterAdd()
        {
            PropRegistrySO registry = ScriptableObject.CreateInstance<PropRegistrySO>();
            PropDefinitionSO first = ScriptableObject.CreateInstance<PropDefinitionSO>();
            first.propId = "first_prop";
            PropDefinitionSO second = ScriptableObject.CreateInstance<PropDefinitionSO>();
            second.propId = "second_prop";
            try
            {
                registry.EditorAddProp(first);
                registry.RebuildIndex();
                Assert.IsNotNull(registry.ResolveGuid("first_prop"));
                Assert.IsNull(registry.ResolveGuid("second_prop"));

                registry.EditorAddProp(second);
                registry.RebuildIndex();
                Assert.IsNotNull(registry.ResolveGuid("first_prop"));
                Assert.IsNotNull(registry.ResolveGuid("second_prop"));
            }
            finally
            {
                Object.DestroyImmediate(registry);
                Object.DestroyImmediate(first);
                Object.DestroyImmediate(second);
            }
        }
    }
}
#endif
