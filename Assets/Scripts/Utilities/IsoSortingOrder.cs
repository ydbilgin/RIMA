using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteAlways]
public class IsoSortingOrder : MonoBehaviour
{
    [SerializeField] private int sortOffset = 0;
    [SerializeField] private float multiplier = 100f;
    private SpriteRenderer sr;

    private void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateOrder();
    }

    private void LateUpdate()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        UpdateOrder();
    }

    private void UpdateOrder()
    {
        if (sr == null) return;
        sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * multiplier) + sortOffset;
    }

#if UNITY_EDITOR
    [MenuItem("RIMA/Tools/Attach IsoSortingOrder to Selected")]
    private static void AttachToSelected()
    {
        int count = 0;
        foreach (GameObject selected in Selection.gameObjects)
        {
            if (selected == null) continue;
            SpriteRenderer[] renderers = selected.GetComponentsInChildren<SpriteRenderer>(true);
            foreach (SpriteRenderer renderer in renderers)
            {
                if (renderer == null) continue;
                GameObject go = renderer.gameObject;
                if (go.GetComponent<IsoSortingOrder>() != null) continue;
                Undo.AddComponent<IsoSortingOrder>(go);
                count++;
            }
        }

        Debug.Log("[IsoSortingOrder] Attached to " + count + " selected SpriteRenderer GameObjects.");
    }
#endif
}
