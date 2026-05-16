#if UNITY_EDITOR
using NUnit.Framework;
using RIMA.MapDesigner.Brush.Editor.Utilities;

namespace RIMA.Tests.Brush
{
    public sealed class DependencyReportGeneratorTests
    {
        [Test]
        public void BuildReport_ContainsTitle()
        {
            string report = DependencyReportGenerator.BuildReport();
            Assert.IsNotEmpty(report);
            StringAssert.Contains("RIMA Brush Tool Dependency Report", report);
        }

        [Test]
        public void BuildReport_ContainsKnownSections()
        {
            string report = DependencyReportGenerator.BuildReport();
            StringAssert.Contains("PropDefinitionSO assets", report);
            StringAssert.Contains("SliceLayoutTemplateSO assets", report);
            StringAssert.Contains("AssetPoolSO assets", report);
            StringAssert.Contains("RoomTemplateSO assets", report);
            StringAssert.Contains("Assembly Definitions", report);
        }

        [Test]
        public void BuildReport_ContainsGeneratedTimestamp()
        {
            string report = DependencyReportGenerator.BuildReport();
            StringAssert.Contains("Generated:", report);
            StringAssert.Contains("UTC", report);
        }
    }
}
#endif
