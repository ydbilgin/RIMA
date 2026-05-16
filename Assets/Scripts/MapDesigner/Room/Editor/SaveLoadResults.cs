#if UNITY_EDITOR
using System.Collections.Generic;
using RIMA.MapDesigner.Room.Validation;
using UnityEngine;

namespace RIMA.MapDesigner.Room.Editor
{
    public class SaveResult
    {
        public bool success;
        public string prefabPath;
        public string templateAssetPath;
        public bool guidPreserved;
        public List<RoomValidationIssue> issues = new List<RoomValidationIssue>();
    }

    public class LoadResult
    {
        public bool success;
        public GameObject instance;
        public string message;
    }
}
#endif
