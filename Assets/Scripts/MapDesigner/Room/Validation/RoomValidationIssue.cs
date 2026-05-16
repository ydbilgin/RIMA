using System;

namespace RIMA.MapDesigner.Room.Validation
{
    public enum ValidationSeverity { Error, Warning, Info }

    [Serializable]
    public class RoomValidationIssue
    {
        public ValidationSeverity severity;
        public string code;
        public string message;
        public string roomId;

        public RoomValidationIssue() { }

        public RoomValidationIssue(ValidationSeverity severity, string code, string message, string roomId = null)
        {
            this.severity = severity;
            this.code = code;
            this.message = message;
            this.roomId = roomId ?? string.Empty;
        }
    }
}
