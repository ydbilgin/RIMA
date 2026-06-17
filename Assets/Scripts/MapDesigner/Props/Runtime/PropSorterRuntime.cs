using UnityEngine;

namespace RIMA.MapDesigner.Props.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    [ExecuteAlways]
    public sealed class PropSorterRuntime : MonoBehaviour
    {
        public const string DefaultPropsLayerName = "Props";        // legacy name; default routing now below
        public const string UprightLayerName = "Entities";          // share the player's layer → props Y-interleave with characters
        public const string FloorDecalLayerName = "Decals";         // below Entities → characters always render on top

        [SerializeField] private PropDefinitionSO propDef;

        private SpriteRenderer sr;
        private static int cachedUprightLayerId = int.MinValue;
        private static int cachedDecalLayerId = int.MinValue;

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
                : ResolveDefaultLayerId(propDef.isFloorDecal);
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

        // Floor decals sit on "Decals" (under the player); upright props share the player's "Entities"
        // layer so the per-frame -y*100 order interleaves them with characters by depth.
        private static int ResolveDefaultLayerId(bool floorDecal)
        {
            if (floorDecal)
            {
                if (cachedDecalLayerId == int.MinValue) cachedDecalLayerId = SortingLayer.NameToID(FloorDecalLayerName);
                return cachedDecalLayerId;
            }
            if (cachedUprightLayerId == int.MinValue) cachedUprightLayerId = SortingLayer.NameToID(UprightLayerName);
            return cachedUprightLayerId;
        }
    }
}
