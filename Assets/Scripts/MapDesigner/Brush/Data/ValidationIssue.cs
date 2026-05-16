using System;

namespace RIMA.MapDesigner.Brush.Data
{
    [Serializable]
    public class ValidationIssue
    {
        public ValidationIssueSeverity severity;
        public string code;
        public string message;
        public string subjectId;

        public ValidationIssue() { }

        public ValidationIssue(ValidationIssueSeverity severity, string code, string message, string subjectId = null)
        {
            this.severity = severity;
            this.code = code;
            this.message = message;
            this.subjectId = subjectId ?? string.Empty;
        }
    }
}
