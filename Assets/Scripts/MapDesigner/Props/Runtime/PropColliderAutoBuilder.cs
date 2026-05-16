using UnityEngine;

namespace RIMA.MapDesigner.Props.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class PropColliderAutoBuilder : MonoBehaviour
    {
        [SerializeField] private PropDefinitionSO propDef;
        [SerializeField] private int rotationSteps;

        public PropDefinitionSO PropDef
        {
            get => propDef;
            set => propDef = value;
        }

        public int RotationSteps
        {
            get => rotationSteps;
            set => rotationSteps = value;
        }

        public BoxCollider2D EnsureCollider()
        {
            if (propDef == null) return null;
            if (!propDef.blocksWalkable) return null;

            BoxCollider2D existing = GetComponent<BoxCollider2D>();
            if (existing != null) return existing;

            BoxCollider2D box = gameObject.AddComponent<BoxCollider2D>();
            ApplyFootprint(box);
            return box;
        }

        private void Awake()
        {
            EnsureCollider();
        }

        private void OnValidate()
        {
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            if (box != null) ApplyFootprint(box);
        }

        private void ApplyFootprint(BoxCollider2D box)
        {
            if (propDef == null || box == null) return;

            Vector2Int size = RotatedSize(propDef.footprintSize, rotationSteps);
            box.size = new Vector2(Mathf.Max(1, size.x), Mathf.Max(1, size.y));
            box.offset = new Vector2(box.size.x * 0.5f, box.size.y * 0.5f);
        }

        private static Vector2Int RotatedSize(Vector2Int original, int rotationSteps)
        {
            int normalized = ((rotationSteps % 4) + 4) % 4;
            return (normalized == 1 || normalized == 3)
                ? new Vector2Int(original.y, original.x)
                : original;
        }
    }
}
