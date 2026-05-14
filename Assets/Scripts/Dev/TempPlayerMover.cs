using UnityEngine;

namespace RIMA.Dev
{
    public class TempPlayerMover : MonoBehaviour
    {
        public float speed = 10f;

        private void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector3 movement = new Vector3(x, y, 0).normalized * (speed * Time.deltaTime);
            transform.position += movement;

            if (Camera.main != null)
            {
                var camPos = Camera.main.transform.position;
                camPos.x = Mathf.Lerp(camPos.x, transform.position.x, 5f * Time.deltaTime);
                camPos.y = Mathf.Lerp(camPos.y, transform.position.y, 5f * Time.deltaTime);
                Camera.main.transform.position = camPos;
            }
        }
    }
}