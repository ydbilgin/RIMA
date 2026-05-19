using UnityEngine;

namespace RIMA.MapDesigner
{
    /// <summary>
    /// Simple camera follow for shader blend testing.
    /// </summary>
    public class SimpleCameraFollow : MonoBehaviour
    {
        public Transform target;
        [SerializeField] private float smoothSpeed = 5f;
        [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);

        private void LateUpdate()
        {
            if (target == null) return;
            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
        }
    }
}
