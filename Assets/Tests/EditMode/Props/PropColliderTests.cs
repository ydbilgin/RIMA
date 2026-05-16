#if UNITY_EDITOR
using NUnit.Framework;
using RIMA.MapDesigner.Props;
using RIMA.MapDesigner.Props.Runtime;
using UnityEngine;

namespace RIMA.Tests.Props
{
    public sealed class PropColliderTests
    {
        [Test]
        public void EnsureCollider_BlocksWalkable_AddsBoxCollider()
        {
            GameObject go = new GameObject("propColliderTest");
            try
            {
                go.AddComponent<SpriteRenderer>();
                PropColliderAutoBuilder builder = go.AddComponent<PropColliderAutoBuilder>();
                PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
                prop.footprintSize = new Vector2Int(2, 3);
                prop.blocksWalkable = true;
                builder.PropDef = prop;

                BoxCollider2D box = builder.EnsureCollider();
                Assert.IsNotNull(box);
                Assert.AreEqual(2f, box.size.x);
                Assert.AreEqual(3f, box.size.y);
                Assert.AreEqual(1f, box.offset.x);
                Assert.AreEqual(1.5f, box.offset.y);
                Object.DestroyImmediate(prop);
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void EnsureCollider_DoesNotBlockWalkable_NoCollider()
        {
            GameObject go = new GameObject("propColliderNoBlock");
            try
            {
                go.AddComponent<SpriteRenderer>();
                PropColliderAutoBuilder builder = go.AddComponent<PropColliderAutoBuilder>();
                PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
                prop.footprintSize = new Vector2Int(1, 1);
                prop.blocksWalkable = false;
                builder.PropDef = prop;

                BoxCollider2D box = builder.EnsureCollider();
                Assert.IsNull(box);
                Assert.IsNull(go.GetComponent<BoxCollider2D>());
                Object.DestroyImmediate(prop);
            }
            finally
            {
                Object.DestroyImmediate(go);
            }
        }

        [Test]
        public void EnsureCollider_Rotation90_SwapsBoxSize()
        {
            GameObject go = new GameObject("propColliderRotated");
            try
            {
                go.AddComponent<SpriteRenderer>();
                PropColliderAutoBuilder builder = go.AddComponent<PropColliderAutoBuilder>();
                PropDefinitionSO prop = ScriptableObject.CreateInstance<PropDefinitionSO>();
                prop.footprintSize = new Vector2Int(3, 1);
                prop.blocksWalkable = true;
                builder.PropDef = prop;
                builder.RotationSteps = 1;

                BoxCollider2D box = builder.EnsureCollider();
                Assert.IsNotNull(box);
                Assert.AreEqual(1f, box.size.x);
                Assert.AreEqual(3f, box.size.y);
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
