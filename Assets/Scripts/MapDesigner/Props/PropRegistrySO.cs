using System.Collections.Generic;
using UnityEngine;

namespace RIMA.MapDesigner.Props
{
    [CreateAssetMenu(menuName = "RIMA/MapDesigner/Props/PropRegistry", fileName = "PropRegistry")]
    public sealed class PropRegistrySO : ScriptableObject
    {
        [SerializeField] private List<PropDefinitionSO> allProps = new List<PropDefinitionSO>();

        private Dictionary<string, PropDefinitionSO> guidToProp;

        public IReadOnlyList<PropDefinitionSO> AllProps => allProps;

        public PropDefinitionSO ResolveGuid(string guid)
        {
            if (string.IsNullOrEmpty(guid)) return null;
            if (guidToProp == null) RebuildIndex();
            return (guidToProp != null && guidToProp.TryGetValue(guid, out PropDefinitionSO prop)) ? prop : null;
        }

        public void RebuildIndex()
        {
            guidToProp = new Dictionary<string, PropDefinitionSO>();
            if (allProps == null) return;

            for (int i = 0; i < allProps.Count; i++)
            {
                PropDefinitionSO prop = allProps[i];
                if (prop == null) continue;

#if UNITY_EDITOR
                string assetPath = UnityEditor.AssetDatabase.GetAssetPath(prop);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    string editorGuid = UnityEditor.AssetDatabase.AssetPathToGUID(assetPath);
                    if (!string.IsNullOrEmpty(editorGuid))
                    {
                        if (prop.propId != editorGuid)
                        {
                            prop.propId = editorGuid;
                            UnityEditor.EditorUtility.SetDirty(prop);
                        }
                        guidToProp[editorGuid] = prop;
                        continue;
                    }
                }
#endif

                if (!string.IsNullOrEmpty(prop.propId))
                {
                    guidToProp[prop.propId] = prop;
                }
            }
        }

#if UNITY_EDITOR
        public void EditorAddProp(PropDefinitionSO prop)
        {
            if (prop == null) return;
            if (allProps == null) allProps = new List<PropDefinitionSO>();
            if (!allProps.Contains(prop)) allProps.Add(prop);
            RebuildIndex();
        }
#endif
    }
}
