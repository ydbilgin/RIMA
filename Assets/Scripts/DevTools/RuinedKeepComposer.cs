#if UNITY_EDITOR || DEVELOPMENT_BUILD

using System.Collections.Generic;
using RIMA.RoomPainter;
using UnityEngine;

namespace RIMA.DevTools
{
    public sealed class RuinedKeepComposer : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private List<WallSegment> _segments = new List<WallSegment>();
        [SerializeField] private Transform _parent;

        [ContextMenu("Compose Ruined Keep")]
        public void Compose()
        {
            if (_grid == null)
            {
                Debug.LogWarning("RuinedKeepComposer: no Grid assigned.");
                return;
            }

            if (_segments == null || _segments.Count == 0)
            {
                Debug.LogWarning("RuinedKeepComposer: no wall segments authored.");
                return;
            }

            Transform parent = ResolveParent();
            ClearChildren(parent);

            HashSet<Vector3Int> occupied = new HashSet<Vector3Int>();
            foreach (WallSegment s in _segments)
            {
                switch (s.kind)
                {
                    case SegmentKind.SolidWall:
                        RegisterCreated(WallRunBuilder.BuildRun(_grid, s.fromCell, s.toCell, s.piece, parent, occupied));
                        break;
                    case SegmentKind.Anchor:
                    case SegmentKind.Entrance:
                        RegisterCreated(WallRunBuilder.PlaceOne(_grid, s.fromCell, s.piece, parent));
                        break;
                    case SegmentKind.VoidEdge:
                    case SegmentKind.BrokenGap:
                        break;
                }
            }
        }

        private Transform ResolveParent()
        {
            if (_parent != null) return _parent;

            GameObject existing = GameObject.Find("RuinedKeep_Walls");
            if (existing != null)
            {
                _parent = existing.transform;
                return _parent;
            }

            GameObject created = new GameObject("RuinedKeep_Walls");
            RegisterCreated(created);
            _parent = created.transform;
            return _parent;
        }

        private void ClearChildren(Transform parent)
        {
            if (parent == null) return;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyObject(parent.GetChild(i).gameObject);
            }
        }

        private static void RegisterCreated(List<GameObject> created)
        {
            if (created == null) return;
            foreach (GameObject go in created) RegisterCreated(go);
        }

        private static void RegisterCreated(GameObject go)
        {
            if (go == null) return;

#if UNITY_EDITOR
            if (!Application.isPlaying)
                UnityEditor.Undo.RegisterCreatedObjectUndo(go, "Compose Ruined Keep");
#endif
        }

        private static void DestroyObject(GameObject go)
        {
            if (go == null) return;

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.Undo.DestroyObjectImmediate(go);
                return;
            }
#endif

            Object.Destroy(go);
        }
    }
}

#endif // UNITY_EDITOR || DEVELOPMENT_BUILD
