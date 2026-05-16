using System.Collections.Generic;
using System.Linq;

namespace RIMA.MapDesigner.Brush.Data
{
    public class ImportResult
    {
        public int variantCount;
        public List<ValidationIssue> issues = new List<ValidationIssue>();
        public AssetPoolSO pool;

        public bool Success => issues == null || issues.All(i => i.severity != ValidationIssueSeverity.Error);
    }
}
