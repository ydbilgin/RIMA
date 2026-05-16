using UnityEngine;

namespace RIMA.MapDesigner.Props.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteAlways]
    public sealed class PropSorterRuntime : MonoBehaviour
    {
        public const string DefaultPropsLayerName = "Props";

        [SerializeField] private PropDefinitionSO propDef;

        private SpriteRenderer sr;
        private static int cachedDefaultLayerId = int.MinValue;

        public PropDefinitionSO PropDef
        {
            get => propDef;
            set
            {
                propDef = value;
                Apply();
            }
        }

        public void Apply()
        {
            EnsureSpriteRenderer();
            if (propDef == null || sr == null) return;

            int targetLayerId = propDef.sortingLayerOverride != 0
                ? propDef.sortingLayerOverride
                : ResolveDefaultLayerId();
            if (targetLayerId != 0)
            {
                sr.sortingLayerID = targetLayerId;
            }

            switch (propDef.sortingMode)
            {
                case PropDefinitionSO.PropSortingMode.FixedOrder:
                    sr.sortingOrder = propDef.sortingOrder;
                    break;
                case PropDefinitionSO.PropSortingMode.AboveAll:
                    sr.sortingOrder = 32760;
                    break;
                case PropDefinitionSO.PropSortingMode.YPosition:
                    sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100f);
                    break;
            }
        }

        private void OnEnable()
        {
            Apply();
        }

        private void LateUpdate()
        {
            if (propDef == null || sr == null) return;
            if (propDef.sortingMode != PropDefinitionSO.PropSortingMode.YPosition) return;
            sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100f);
        }

        private void EnsureSpriteRenderer()
        {
            if (sr == null) sr = GetComponent<SpriteRenderer>();
        }

        private static int ResolveDefaultLayerId()
        {
            if (cachedDefaultLayerId != int.MinValue) return cachedDefaultLayerId;
            int id = SortingLayer.NameToID(DefaultPropsLayerName);
            cachedDefaultLayerId = id;
            return id;
        }
    }
}
