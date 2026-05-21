using System.Collections.Generic;
using UnityEngine;

namespace RIMA.Map
{
    public class RoomInstance : MonoBehaviour
    {
        public string roomId;
        public bool isLocked;
        public List<GameObject> mobInstances = new List<GameObject>();
        public List<global::RIMA.DoorTrigger> doors = new List<global::RIMA.DoorTrigger>();

        public void OnEnter()
        {
            if (isLocked) LockDoors();
        }

        public void OnExit()
        {
        }

        public void LockDoors()
        {
            isLocked = true;
            SetDoorsActive(false);
        }

        public void UnlockDoors()
        {
            isLocked = false;
            SetDoorsActive(true);
        }

        private void SetDoorsActive(bool active)
        {
            if (doors == null) return;

            for (int i = 0; i < doors.Count; i++)
            {
                if (doors[i] != null) doors[i].SetActive(active);
            }
        }
    }
}
