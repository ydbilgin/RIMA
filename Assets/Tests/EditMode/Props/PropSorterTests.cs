#if UNITY_EDITOR
using NUnit.Framework;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Runtime;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class PropSorterTests
    {
        [Test]
        public void Apply_FixedOrderMode_UsesSortingOrderField()
        {
            GameObject go = new GameObject("sorterFixed");
            try
            {
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                PropSorterRuntime sorter = go.AddComponent<PropSorterRuntime>();
                PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
                prop.sortingMode = PropDefinitionSO.PropSortingMode.FixedOrder;
                prop.sortingOrder = 42;
                sorter.PropDef = prop;

                sorter.Apply();
                Assert.AreEqual(42, sr.sortingOrder);
                Object.DestroyImmediate(prop);
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void Apply_AboveAllMode_UsesHighSortingOrder()
        {
            GameObject go = new GameObject("sorterAboveAll");
            try
            {
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                PropSorterRuntime sorter = go.AddComponent<PropSorterRuntime>();
                PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
                prop.sortingMode = PropDefinitionSO.PropSortingMode.AboveAll;
                sorter.PropDef = prop;

                sorter.Apply();
                Assert.AreEqual(32760, sr.sortingOrder);
                Object.DestroyImmediate(prop);
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void Apply_YPositionMode_AppliesNegativeYScaledOrder()
        {
            GameObject go = new GameObject("sorterY");
            try
            {
                go.transform.position = new Vector3(0f, 2.5f, 0f);
                SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
                PropSorterRuntime sorter = go.AddComponent<PropSorterRuntime>();
                PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
                prop.sortingMode = PropDefinitionSO.PropSortingMode.YPosition;
                sorter.PropDef = prop;

                sorter.Apply();
                Assert.AreEqual(-250, sr.sortingOrder);
                Object.DestroyImmediate(prop);
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }
    }
}
#endif
