using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;

namespace RIMA.Tests
{
    /// <summary>
    /// EditMode test that scans all project C# scripts for common Unity performance
    /// anti-patterns. Produces a structured report and fails if CRITICAL issues exist.
    ///
    /// Categories:
    ///   CRITICAL — per-frame Find/FindObjectOfType/FindObjectsByType calls (inside Update/LateUpdate/FixedUpdate)
    ///   HIGH     — per-frame allocations (new List, new Dictionary, LINQ .ToList/.ToArray in hot paths)
    ///   MEDIUM   — Debug.Log in Update (GC + IO overhead in builds)
    ///   LOW      — GetComponent in Update (should cache in Awake/Start)
    ///
    /// The test PASSes if zero CRITICAL issues remain. HIGH/MEDIUM/LOW are warnings only.
    /// </summary>
    public class PerformanceAntiPatternTests
    {
        private static readonly string ScriptsRoot = Path.Combine(Application.dataPath, "Scripts");

        // Method names that run every frame
        private static readonly string[] HotMethods = {
            "Update", "LateUpdate", "FixedUpdate"
        };

        // ── CRITICAL: scene-wide search in hot path ──────────────────────────
        private static readonly Regex RxFind = new Regex(
            @"GameObject\s*\.\s*(Find|FindGameObjectWithTag|FindGameObjectsWithTag|FindWithTag)\s*\(",
            RegexOptions.Compiled);

        private static readonly Regex RxFindObjectOfType = new Regex(
            @"(FindObjectOfType|FindObjectsOfType|FindAnyObjectByType|FindObjectsByType)\s*[<(]",
            RegexOptions.Compiled);

        // ── HIGH: per-frame heap allocations ─────────────────────────────────
        private static readonly Regex RxNewCollection = new Regex(
            @"new\s+(List|Dictionary|HashSet|Queue|Stack|LinkedList)\s*<",
            RegexOptions.Compiled);

        private static readonly Regex RxLinqAlloc = new Regex(
            @"\.(ToList|ToArray|ToDictionary|Select|Where|OrderBy|GroupBy)\s*\(",
            RegexOptions.Compiled);

        // ── MEDIUM: Debug.Log in Update ──────────────────────────────────────
        private static readonly Regex RxDebugLog = new Regex(
            @"Debug\s*\.\s*(Log|LogWarning|LogError)\s*\(",
            RegexOptions.Compiled);

        // ── LOW: GetComponent in Update (should cache) ───────────────────────
        private static readonly Regex RxGetComponent = new Regex(
            @"(GetComponent|GetComponentInChildren|GetComponentInParent)\s*[<(]",
            RegexOptions.Compiled);

        // ── Method boundary detection ────────────────────────────────────────
        // Matches "void Update()", "private void LateUpdate()", etc.
        private static readonly Regex RxMethodDecl = new Regex(
            @"(private|protected|public|internal|static|\s)*(virtual\s+|override\s+|abstract\s+)?(void|IEnumerator|bool|int|float)\s+(\w+)\s*\(",
            RegexOptions.Compiled);

        private enum Severity { CRITICAL, HIGH, MEDIUM, LOW }

        private struct Issue
        {
            public Severity severity;
            public string file;
            public int line;
            public string method;
            public string pattern;
            public string lineContent;
        }

        [Test]
        public void ScanForPerformanceAntiPatterns()
        {
            Assert.IsTrue(Directory.Exists(ScriptsRoot), $"Scripts root not found: {ScriptsRoot}");

            var csFiles = Directory.GetFiles(ScriptsRoot, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Contains(Path.DirectorySeparatorChar + "Editor" + Path.DirectorySeparatorChar))
                .Where(f => !f.Contains(Path.DirectorySeparatorChar + "Tests" + Path.DirectorySeparatorChar))
                .ToArray();

            var issues = new List<Issue>();

            foreach (var filePath in csFiles)
            {
                ScanFile(filePath, issues);
            }

            // ── Build Report ─────────────────────────────────────────────────
            var sb = new StringBuilder();
            sb.AppendLine("========================================");
            sb.AppendLine("  RIMA Performance Anti-Pattern Report");
            sb.AppendLine("========================================");
            sb.AppendLine($"  Scanned: {csFiles.Length} files");
            sb.AppendLine($"  Issues found: {issues.Count}");
            sb.AppendLine();

            int criticalCount = 0;
            int highCount = 0;

            foreach (var severity in new[] { Severity.CRITICAL, Severity.HIGH, Severity.MEDIUM, Severity.LOW })
            {
                var group = issues.Where(i => i.severity == severity).ToList();
                if (group.Count == 0) continue;

                sb.AppendLine($"── {severity} ({group.Count}) ──────────────────────────────");
                foreach (var issue in group)
                {
                    string relPath = issue.file.Replace(Application.dataPath, "Assets");
                    sb.AppendLine($"  [{severity}] {relPath}:{issue.line}");
                    sb.AppendLine($"    Method: {issue.method}()");
                    sb.AppendLine($"    Pattern: {issue.pattern}");
                    sb.AppendLine($"    Code: {issue.lineContent.Trim()}");
                    sb.AppendLine();
                }

                if (severity == Severity.CRITICAL) criticalCount = group.Count;
                if (severity == Severity.HIGH) highCount = group.Count;
            }

            sb.AppendLine("── Summary ──────────────────────────────");
            sb.AppendLine($"  CRITICAL: {criticalCount}");
            sb.AppendLine($"  HIGH:     {highCount}");
            sb.AppendLine($"  MEDIUM:   {issues.Count(i => i.severity == Severity.MEDIUM)}");
            sb.AppendLine($"  LOW:      {issues.Count(i => i.severity == Severity.LOW)}");
            sb.AppendLine("========================================");

            Debug.Log(sb.ToString());

            // Fail on CRITICAL issues only
            Assert.AreEqual(0, criticalCount,
                $"{criticalCount} CRITICAL performance anti-patterns found in hot paths. See console for full report.");
        }

        // ─── File Scanner ────────────────────────────────────────────────────

        private void ScanFile(string filePath, List<Issue> issues)
        {
            string[] lines;
            try { lines = File.ReadAllLines(filePath); }
            catch { return; }

            // First pass: identify which line ranges belong to hot methods
            var hotRanges = FindHotMethodRanges(lines);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int lineNum = i + 1;

                // Skip comments
                string trimmed = line.TrimStart();
                if (trimmed.StartsWith("//")) continue;
                if (trimmed.StartsWith("/*")) continue;
                if (trimmed.StartsWith("*")) continue;

                // Determine if this line is inside a hot method
                string hotMethod = null;
                foreach (var range in hotRanges)
                {
                    if (lineNum >= range.startLine && lineNum <= range.endLine)
                    {
                        hotMethod = range.methodName;
                        break;
                    }
                }

                if (hotMethod == null) continue; // Only scan hot paths

                // CRITICAL: scene-wide searches
                if (RxFind.IsMatch(line))
                {
                    issues.Add(new Issue
                    {
                        severity = Severity.CRITICAL,
                        file = filePath, line = lineNum,
                        method = hotMethod,
                        pattern = "GameObject.Find* in hot path",
                        lineContent = line
                    });
                }

                if (RxFindObjectOfType.IsMatch(line))
                {
                    issues.Add(new Issue
                    {
                        severity = Severity.CRITICAL,
                        file = filePath, line = lineNum,
                        method = hotMethod,
                        pattern = "FindObject(s)OfType in hot path",
                        lineContent = line
                    });
                }

                // HIGH: heap allocations
                if (RxNewCollection.IsMatch(line))
                {
                    issues.Add(new Issue
                    {
                        severity = Severity.HIGH,
                        file = filePath, line = lineNum,
                        method = hotMethod,
                        pattern = "new Collection<> allocation in hot path",
                        lineContent = line
                    });
                }

                if (RxLinqAlloc.IsMatch(line))
                {
                    issues.Add(new Issue
                    {
                        severity = Severity.HIGH,
                        file = filePath, line = lineNum,
                        method = hotMethod,
                        pattern = "LINQ allocation (.ToList/.Where etc.) in hot path",
                        lineContent = line
                    });
                }

                // MEDIUM: Debug.Log
                if (RxDebugLog.IsMatch(line))
                {
                    issues.Add(new Issue
                    {
                        severity = Severity.MEDIUM,
                        file = filePath, line = lineNum,
                        method = hotMethod,
                        pattern = "Debug.Log in hot path (GC + IO overhead)",
                        lineContent = line
                    });
                }

                // LOW: GetComponent
                if (RxGetComponent.IsMatch(line))
                {
                    issues.Add(new Issue
                    {
                        severity = Severity.LOW,
                        file = filePath, line = lineNum,
                        method = hotMethod,
                        pattern = "GetComponent in hot path (should cache)",
                        lineContent = line
                    });
                }
            }
        }

        // ─── Brace-counting method range finder ──────────────────────────────

        private struct MethodRange
        {
            public string methodName;
            public int startLine;
            public int endLine;
        }

        private List<MethodRange> FindHotMethodRanges(string[] lines)
        {
            var ranges = new List<MethodRange>();

            for (int i = 0; i < lines.Length; i++)
            {
                var match = RxMethodDecl.Match(lines[i]);
                if (!match.Success) continue;

                string methodName = match.Groups[4].Value;
                if (!IsHotMethod(methodName)) continue;

                // Find opening brace
                int braceStart = -1;
                for (int j = i; j < lines.Length && j < i + 5; j++)
                {
                    if (lines[j].Contains("{"))
                    {
                        braceStart = j;
                        break;
                    }
                }

                if (braceStart < 0) continue;

                // Count braces to find method end
                int depth = 0;
                int endLine = braceStart;
                for (int j = braceStart; j < lines.Length; j++)
                {
                    foreach (char c in lines[j])
                    {
                        if (c == '{') depth++;
                        else if (c == '}') depth--;
                    }

                    if (depth <= 0)
                    {
                        endLine = j;
                        break;
                    }
                }

                ranges.Add(new MethodRange
                {
                    methodName = methodName,
                    startLine = i + 1, // 1-indexed
                    endLine = endLine + 1
                });
            }

            return ranges;
        }

        private bool IsHotMethod(string name)
        {
            // Direct hot methods
            foreach (var hot in HotMethods)
                if (name == hot) return true;

            // Also catch methods called FROM hot methods — we track known callers
            // For now, we scan direct hot methods + common delegation patterns
            return false;
        }

        // ─── Helper: scan for methods called from Update (indirect hot paths) ─

        [Test]
        public void ScanIndirectHotPaths()
        {
            // This test specifically scans for methods that are called from Update/LateUpdate/FixedUpdate
            // and themselves contain expensive operations.
            // It uses a two-pass approach: first find call sites in hot methods, then scan those methods.

            Assert.IsTrue(Directory.Exists(ScriptsRoot), $"Scripts root not found: {ScriptsRoot}");

            var csFiles = Directory.GetFiles(ScriptsRoot, "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Contains(Path.DirectorySeparatorChar + "Editor" + Path.DirectorySeparatorChar))
                .Where(f => !f.Contains(Path.DirectorySeparatorChar + "Tests" + Path.DirectorySeparatorChar))
                .ToArray();

            var issues = new List<Issue>();
            int totalIndirect = 0;

            foreach (var filePath in csFiles)
            {
                string[] lines;
                try { lines = File.ReadAllLines(filePath); }
                catch { continue; }

                var hotRanges = FindHotMethodRanges(lines);
                if (hotRanges.Count == 0) continue;

                // Collect method names called from hot paths
                var calledMethods = new HashSet<string>();
                var rxCall = new Regex(@"(\w+)\s*\(", RegexOptions.Compiled);

                foreach (var range in hotRanges)
                {
                    for (int i = range.startLine - 1; i < range.endLine && i < lines.Length; i++)
                    {
                        foreach (Match m in rxCall.Matches(lines[i]))
                        {
                            string called = m.Groups[1].Value;
                            // Skip known safe / built-in calls
                            if (called == "if" || called == "for" || called == "while" ||
                                called == "return" || called == "new" || called == "typeof" ||
                                called == "nameof" || called == "switch" || called == "catch") continue;
                            calledMethods.Add(called);
                        }
                    }
                }

                // Now find those methods in the same file and scan them
                for (int i = 0; i < lines.Length; i++)
                {
                    var match = RxMethodDecl.Match(lines[i]);
                    if (!match.Success) continue;
                    string methodName = match.Groups[4].Value;
                    if (!calledMethods.Contains(methodName)) continue;
                    if (IsHotMethod(methodName)) continue; // already scanned

                    // Find this method's range
                    int braceStart = -1;
                    for (int j = i; j < lines.Length && j < i + 5; j++)
                    {
                        if (lines[j].Contains("{")) { braceStart = j; break; }
                    }
                    if (braceStart < 0) continue;

                    int depth = 0;
                    int endLine = braceStart;
                    for (int j = braceStart; j < lines.Length; j++)
                    {
                        foreach (char c in lines[j])
                        {
                            if (c == '{') depth++;
                            else if (c == '}') depth--;
                        }
                        if (depth <= 0) { endLine = j; break; }
                    }

                    // Scan this indirect hot method
                    for (int ln = i; ln <= endLine && ln < lines.Length; ln++)
                    {
                        string line = lines[ln];
                        int lineNum = ln + 1;

                        if (RxFind.IsMatch(line))
                        {
                            issues.Add(new Issue
                            {
                                severity = Severity.CRITICAL,
                                file = filePath, line = lineNum,
                                method = $"{methodName} (called from hot path)",
                                pattern = "GameObject.Find* in INDIRECT hot path",
                                lineContent = line
                            });
                            totalIndirect++;
                        }

                        if (RxFindObjectOfType.IsMatch(line))
                        {
                            issues.Add(new Issue
                            {
                                severity = Severity.CRITICAL,
                                file = filePath, line = lineNum,
                                method = $"{methodName} (called from hot path)",
                                pattern = "FindObject(s)OfType in INDIRECT hot path",
                                lineContent = line
                            });
                            totalIndirect++;
                        }

                        if (RxNewCollection.IsMatch(line))
                        {
                            issues.Add(new Issue
                            {
                                severity = Severity.HIGH,
                                file = filePath, line = lineNum,
                                method = $"{methodName} (called from hot path)",
                                pattern = "new Collection<> in INDIRECT hot path",
                                lineContent = line
                            });
                        }
                    }
                }
            }

            // Report
            var sb = new StringBuilder();
            sb.AppendLine("========================================");
            sb.AppendLine("  INDIRECT Hot Path Scan Report");
            sb.AppendLine("========================================");
            foreach (var issue in issues)
            {
                string relPath = issue.file.Replace(Application.dataPath, "Assets");
                sb.AppendLine($"  [{issue.severity}] {relPath}:{issue.line}");
                sb.AppendLine($"    Method: {issue.method}");
                sb.AppendLine($"    Pattern: {issue.pattern}");
                sb.AppendLine($"    Code: {issue.lineContent.Trim()}");
                sb.AppendLine();
            }

            int critCount = issues.Count(i => i.severity == Severity.CRITICAL);
            sb.AppendLine($"  CRITICAL (indirect): {critCount}");
            sb.AppendLine($"  HIGH (indirect):     {issues.Count(i => i.severity == Severity.HIGH)}");

            Debug.Log(sb.ToString());

            Assert.AreEqual(0, critCount,
                $"{critCount} CRITICAL indirect hot path issues found. See console for report.");
        }
    }
}
