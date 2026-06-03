using RIMA.MapDesigner.Room.Data;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Runtime
{
    public sealed class IsoRoomBuilderTester : MonoBehaviour
    {
        [SerializeField] private IsoRoomBuilder builder;
        [SerializeField] private RoomTemplateSO template;
        [SerializeField] private bool buildOnStart = true;

        private void Start()
        {
            if (buildOnStart && builder != null && template != null)
            {
                builder.Build(template);
            }
        }

        [ContextMenu("Rebuild")]
        private void Rebuild()
        {
            if (builder != null)
            {
                builder.Build(template);
            }
        }
    }
}
