using UnityEngine;

public class TestDoorExit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[Playable Test] Player exited the room!");
        }
    }
}
